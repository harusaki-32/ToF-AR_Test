/*
 * Copyright 2018,2019,2020,2021,2022 Sony Semiconductor Solutions Corporation.
 *
 * This is UNPUBLISHED PROPRIETARY SOURCE CODE of Sony Semiconductor
 * Solutions Corporation.
 * No part of this file may be copied, modified, sold, and distributed in any
 * form or by any means without prior explicit permission in writing from
 * Sony Semiconductor Solutions Corporation.
 *
 */
using System;
using System.Collections;
using SensCord;

namespace TofAr.V0.Coordinate
{
    /// <summary>
    /// TofAr Coordinateコンポーネントとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>GetPropertyによる変換座標の取得</description></item>
    /// </list>
    /// </summary>
    public class TofArCoordinateManager : Singleton<TofArCoordinateManager>, IDisposable, IStreamStoppable
    {
        /// <summary>
        /// コンポーネントのバージョン番号
        /// </summary>
        public string Version
        {
            get
            {
                return ComponentVersion.version;
            }
        }

        private ColorPointProperty[] colorPointPropertyArray = new ColorPointProperty[0];
        private DepthPointProperty[] depthPointPropertyArray = new DepthPointProperty[0];
        private CameraPointProperty[] colorToCameraPointPropertyArray = new CameraPointProperty[0];
        private CameraPointProperty[] depthToCameraPointPropertyArray = new CameraPointProperty[0];

        /// <summary>
        /// アプリケーション一時停止開始時デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        public delegate void ApplicationPausingEventHandler(object sender);
        /// <summary>
        /// アプリケーション一時停止開始時
        /// </summary>
        public static event ApplicationPausingEventHandler OnApplicationPausing;

        /// <summary>
        /// アプリケーション復帰開始時デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        public delegate void ApplicationResumingEventHandler(object sender);
        /// <summary>
        /// アプリケーション復帰開始時
        /// </summary>
        public static event ApplicationResumingEventHandler OnApplicationResuming;

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                OnApplicationPausing?.Invoke(this);
            }
            else
            {
                OnApplicationResuming?.Invoke(this);
            }
        }

        private bool IsValidDepthRange(Int16 depth)
        {
            return (depth >= 100) && (depth <= 32000);
        }

        private void MapDepthToColor(short[] depthFrame, int wDepth, int hDepth, int wInDepth, int hInDepth)
        {
            float divGX = 1.0f / this.GX;
            float dx4 = this.DX * 4;
            float divGY = 1.0f / this.GY;
            float dy4 = this.DY * 4;

            for (var h = 0; h < hInDepth; h++)
            {
                for (var w = 0; w < wInDepth; w++)
                {
                    var index = h * wInDepth + w;
                    var depth = depthFrame[index];

                    if (!this.IsValidDepthRange(depth))
                    {
                        continue;
                    }

                    int x, y;
                    if (this.DX == 0)
                    {
                        x = this.depthXcache[w];
                        if (x == -1)
                        {
                            x = -1;
                            y = -1;
                        }
                        else
                        {
                            y = (int)((h - this.OY + dy4 / depth) * divGY + 0.5f);

                            if (y < 0 || y >= hDepth)
                            {
                                x = -1;
                                y = -1;
                            }
                        }
                    }
                    else
                    {
                        y = this.depthYcache[h];
                        if (y == -1)
                        {
                            x = -1;
                            y = -1;
                        }
                        else
                        {
                            x = (int)((w - this.OX + dx4 / depth) * divGX + 0.5f);

                            if (x < 0 || x >= wDepth)
                            {
                                x = -1;
                                y = -1;
                            }
                        }
                    }
                    this.colorPointPropertyArray[index].x = x;
                    this.colorPointPropertyArray[index].y = y;
                }
            }
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public DepthToColorProperty GetProperty<T>(DepthToColorProperty value)
        {
            if (this.SettingsProperty == null)
            {
                return null;
            }
            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            if (value.colorPoints == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "color points are null");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (value.depthFrame == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "depth frame is null");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (value.depthFrame.Length != SettingsProperty.depthHeight * SettingsProperty.depthWidth)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"depth size {this.SettingsProperty.depthWidth} {this.SettingsProperty.depthHeight} is out of range.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (this.colorPointPropertyArray.Length != this.SettingsProperty.depthHeight * this.SettingsProperty.depthWidth)
            {
                Array.Resize(ref this.colorPointPropertyArray, this.SettingsProperty.depthHeight * this.SettingsProperty.depthWidth);
            }

            int wDepth, hDepth, hInDepth, wInDepth;
            wDepth = this.SettingsProperty.colorWidth;
            hDepth = this.SettingsProperty.colorHeight;
            wInDepth = this.SettingsProperty.depthWidth;
            hInDepth = this.SettingsProperty.depthHeight;

            if (wDepth != this.previousSettings.colorWidth || hDepth != this.previousSettings.colorHeight ||
                wInDepth != this.previousSettings.depthWidth || hInDepth != this.previousSettings.depthHeight)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is different size to settings.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            var initialValue = new ColorPointProperty() { x = -1, y = -1 };
            Utils.FillArray<ColorPointProperty>(this.colorPointPropertyArray, initialValue);

            MapDepthToColor(value.depthFrame, wDepth, hDepth, wInDepth, hInDepth);

            value.colorPoints = this.colorPointPropertyArray;

            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public DepthToCameraProperty GetProperty<T>(DepthToCameraProperty value)
        {
            if (this.SettingsProperty == null)
            {
                return null;
            }

            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            if (value.cameraPoints == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "camera points are null");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (value.depthFrame == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "depth frame is null");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (value.depthFrame.Length != SettingsProperty.depthHeight * SettingsProperty.depthWidth)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"depth size {this.SettingsProperty.depthWidth} {this.SettingsProperty.depthHeight} is out of range.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            if (this.depthToCameraPointPropertyArray.Length != this.SettingsProperty.depthHeight * this.SettingsProperty.depthWidth)
            {
                Array.Resize(ref this.depthToCameraPointPropertyArray, this.SettingsProperty.depthHeight * this.SettingsProperty.depthWidth);
            }

            int hInDepth, wInDepth;
            wInDepth = this.SettingsProperty.depthWidth;
            hInDepth = this.SettingsProperty.depthHeight;

            var initialValue = new CameraPointProperty() { x = float.NaN, y = float.NaN, z = float.NaN };
            Utils.FillArray<CameraPointProperty>(this.depthToCameraPointPropertyArray, initialValue);

            for (var h = 0; h < hInDepth; h++)
            {
                for (var w = 0; w < wInDepth; w++)
                {
                    var index = h * wInDepth + w;

                    var depth = value.depthFrame[index];
                    if (!this.IsValidDepthRange(depth))
                    {
                        continue;
                    }

                    this.depthToCameraPointPropertyArray[index] = this.DepthPointToCameraPoint(w, h, depth);
                }
            }
            value.cameraPoints = this.depthToCameraPointPropertyArray;

            for (var i = 0; i < value.cameraPoints.Length; i++)
            {
                value.cameraPoints[i].x = value.cameraPoints[i].x / 1000f;
                value.cameraPoints[i].y = -value.cameraPoints[i].y / 1000f;
                value.cameraPoints[i].z = value.cameraPoints[i].z / 1000f;
            }

            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public DepthPointToColorPointProperty GetProperty<T>(DepthPointToColorPointProperty value)
        {
            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            if (value.depth < 0)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"depth {value.depth} is invalid.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            int wDepth, hDepth;
            wDepth = this.SettingsProperty.colorWidth;
            hDepth = this.SettingsProperty.colorHeight;
            if (wDepth != this.previousSettings.colorWidth || hDepth != this.previousSettings.colorHeight)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is different size to settings.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            float divGX = 1.0f / this.GX;
            float dx4 = this.DX * 4;
            float divGY = 1.0f / this.GY;
            float dy4 = this.DY * 4;

            int depthX, depthY;
            depthX = value.depthPoint.x;
            depthY = value.depthPoint.y;

            var depth = value.depth;
            if (!this.IsValidDepthRange(depth))
            {
                value.colorPoint = new ColorPointProperty { x = -1, y = -1 };
                return value;
            }

            int x, y;
            if (this.DX == 0.0f)
            {
                x = this.depthXcache[depthX];
                y = (int)((depthY - this.OY + dy4 / depth) * divGY + 0.5f);

                if (y < 0 || y >= hDepth)
                {
                    x = -1;
                    y = -1;
                }
            }
            else
            {
                x = (int)((depthX - this.OX + dx4 / depth) * divGX + 0.5f);
                y = this.depthYcache[depthY];

                if (x < 0 || x >= wDepth)
                {
                    x = -1;
                    y = -1;
                }
            }
            value.colorPoint = new ColorPointProperty { x = x, y = y };

            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public DepthPointToCameraPointProperty GetProperty<T>(DepthPointToCameraPointProperty value)
        {
            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            if (value.depth < 0)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"depth {value.depth} is invalid.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            value.cameraPoint = this.DepthPointToCameraPoint(value.depthPoint.x, value.depthPoint.y, value.depth);

            value.cameraPoint.y = -value.cameraPoint.y / 1000f;
            value.cameraPoint.x = value.cameraPoint.x / 1000f;
            value.cameraPoint.z = value.cameraPoint.z / 1000f;

            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public ColorToDepthProperty GetProperty<T>(ColorToDepthProperty value)
        {
            if (this.SettingsProperty == null)
            {
                return null;
            }

            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (value.depthPoints == null || value.depthFrame == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value contents is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (value.depthFrame.Length != this.SettingsProperty.depthHeight * this.SettingsProperty.depthWidth)
            {
                return null;
            }
            if (this.depthPointPropertyArray.Length != this.SettingsProperty.colorHeight * this.SettingsProperty.colorWidth)
            {
                Array.Resize(ref this.depthPointPropertyArray, this.SettingsProperty.colorHeight * this.SettingsProperty.colorWidth);
            }


            int wDepth, hDepth, hInDepth, wInDepth;
            wDepth = this.SettingsProperty.colorWidth;
            hDepth = this.SettingsProperty.colorHeight;
            wInDepth = this.SettingsProperty.depthWidth;
            hInDepth = this.SettingsProperty.depthHeight;

            if (wDepth != this.previousSettings.colorWidth || hDepth != this.previousSettings.colorHeight ||
                wInDepth != this.previousSettings.depthWidth || hInDepth != this.previousSettings.depthHeight)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is different size to settings.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            var initialValue = new DepthPointProperty() { x = -1, y = -1 };
            Utils.FillArray<DepthPointProperty>(this.depthPointPropertyArray, initialValue);

            if (this.DX == 0)
            {
                MapFromColorXCache(value.depthFrame, wDepth, hDepth, wInDepth, hInDepth, false);
            }
            else
            {
                MapFromColorYCache(value.depthFrame, wDepth, hDepth, wInDepth, hInDepth, false);
            }
            value.depthPoints = this.depthPointPropertyArray;
            return value;
        }

        private void MapFromColorXCache(short[] depthFrame, int wDepth, int hDepth, int wInDepth, int hInDepth, bool mapToCamera)
        {
            float divGY = 1.0f / GY;
            float dy4 = DY * 4;

            var colorPixelHasSize = ((float)hDepth / (float)hInDepth) > 1.0f;

            for (var x = 0; x < wDepth; x++)
            {
                var colorX = this.colorXcache[x];
                if (colorX == -1)
                {
                    continue;
                }

                for (var h = hInDepth - 1; h > 0; h--)
                {
                    var depth = depthFrame[h * wInDepth + colorX];

                    if (!this.IsValidDepthRange(depth))
                    {
                        continue;
                    }

                    int ymin, ymax;
                    if (colorPixelHasSize)
                    {
                        ymin = (int)(((h - 0.5f) - this.OY + dy4 / depth) * divGY + 0.5f);
                        ymax = (int)(((h + 0.5f) - this.OY + dy4 / depth) * divGY + 0.5f);
                    }
                    else
                    {
                        ymin = ymax = (int)((h - this.OY + dy4 / depth) * divGY + 0.5f);
                    }

                    for (var y = ymin; y <= ymax; y++)
                    {
                        if (y < 0 || hDepth <= y)
                        {
                            continue;
                        }

                        int index = y * wDepth + x;

                        if (mapToCamera)
                        {
                            this.colorToCameraPointPropertyArray[index] = this.DepthPointToCameraPoint(colorX, h, depth);
                        }
                        else
                        {
                            this.depthPointPropertyArray[index] = new DepthPointProperty { x = colorX, y = h };
                        }

                    }
                }
            }

        }

        private void MapFromColorYCache(short[] depthFrame, int wDepth, int hDepth, int wInDepth, int hInDepth, bool mapToCamera)
        {
            float divGX = 1.0f / GX;
            float dx4 = DX * 4;

            var colorPixelHasSize = ((float)wDepth / (float)wInDepth) > 1.0f;

            for (var h = 0; h < hDepth; h++)
            {
                var colorY = this.colorYcache[h];
                if (colorY == -1)
                {
                    continue;
                }

                for (int w = wInDepth - 1; w > 0; w--)
                {
                    var depth = depthFrame[colorY * wInDepth + w];

                    if (!this.IsValidDepthRange(depth))
                    {
                        continue;
                    }

                    int xmin, xmax;
                    if (colorPixelHasSize)
                    {
                        xmin = (int)(((w - 0.5f) - this.OX + dx4 / depth) * divGX + 0.5f);
                        xmax = (int)(((w + 0.5f) - this.OX + dx4 / depth) * divGX + 0.5f);
                    }
                    else
                    {
                        xmin = xmax = (int)((w - this.OX + dx4 / depth) * divGX + 0.5f);
                    }

                    for (var x = xmin; x <= xmax; x++)
                    {
                        if (x < 0 || wDepth <= x)
                        {
                            continue;
                        }

                        int index = h * wDepth + x;

                        if (mapToCamera)
                        {
                            this.colorToCameraPointPropertyArray[index] = this.DepthPointToCameraPoint(w, colorY, depth);
                        }
                        else
                        {
                            this.depthPointPropertyArray[index] = new DepthPointProperty { x = w, y = colorY };
                        }
                    }
                }
            }
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public ColorToCameraProperty GetProperty<T>(ColorToCameraProperty value)
        {
            if (this.SettingsProperty == null)
            {
                return null;
            }

            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });

            }
            if (value.cameraPoints == null || value.depthFrame == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value contents is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }
            if (value.depthFrame.Length != this.SettingsProperty.depthHeight * this.SettingsProperty.depthWidth)
            {
                return null;
            }
            if (this.colorToCameraPointPropertyArray.Length != this.SettingsProperty.colorHeight * this.SettingsProperty.colorWidth)
            {
                Array.Resize(ref this.colorToCameraPointPropertyArray, this.SettingsProperty.colorHeight * this.SettingsProperty.colorWidth);
            }

            int wDepth, hDepth, hInDepth, wInDepth;
            wDepth = this.SettingsProperty.colorWidth;
            hDepth = this.SettingsProperty.colorHeight;
            wInDepth = this.SettingsProperty.depthWidth;
            hInDepth = this.SettingsProperty.depthHeight;
            if (wDepth != this.previousSettings.colorWidth || hDepth != this.previousSettings.colorHeight ||
                wInDepth != this.previousSettings.depthWidth || hInDepth != this.previousSettings.depthHeight)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is different size to settings.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            var initialValue = new CameraPointProperty() { x = float.NaN, y = float.NaN, z = float.NaN };
            Utils.FillArray<CameraPointProperty>(this.colorToCameraPointPropertyArray, initialValue);

            if (this.DX == 0)
            {
                MapFromColorXCache(value.depthFrame, wDepth, hDepth, wInDepth, hInDepth, true);
            }
            else
            {
                MapFromColorYCache(value.depthFrame, wDepth, hDepth, wInDepth, hInDepth, true);
            }

            value.cameraPoints = this.colorToCameraPointPropertyArray;
            for (var i = 0; i < value.cameraPoints.Length; i++)
            {
                value.cameraPoints[i].x = value.cameraPoints[i].x / 1000f;
                value.cameraPoints[i].y = -value.cameraPoints[i].y / 1000f;
                value.cameraPoints[i].z = value.cameraPoints[i].z / 1000f;
            }
            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public CameraPointToDepthPointProperty GetProperty<T>(CameraPointToDepthPointProperty value)
        {
            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            value.cameraPoint.x = value.cameraPoint.x * 1000f;
            value.cameraPoint.y = -value.cameraPoint.y * 1000f;
            value.cameraPoint.z = value.cameraPoint.z * 1000f;


            var d = this.previousSettings.d;
            value.depthPoint.x = (int)(value.cameraPoint.x * d.fx / value.cameraPoint.z + d.cx);
            value.depthPoint.y = (int)(value.cameraPoint.y * d.fy / value.cameraPoint.z + d.cy);

            if ((value.depthPoint.x < 0) ||
                (value.depthPoint.x > (this.previousSettings.depthWidth - 1)) ||
                (value.depthPoint.y < 0) ||
                (value.depthPoint.y > (this.previousSettings.depthHeight - 1)))
            {
                value.depthPoint.x = -1;
                value.depthPoint.y = -1;
            }

            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public CameraPointToColorPointProperty GetProperty<T>(CameraPointToColorPointProperty value)
        {
            if (value == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "value is null.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            value.cameraPoint.x = value.cameraPoint.x * 1000f;
            value.cameraPoint.y = -value.cameraPoint.y * 1000f;
            value.cameraPoint.z = value.cameraPoint.z * 1000f;

            var c = this.previousSettings.c;
            float x = value.cameraPoint.x * this.invR.a + value.cameraPoint.y * this.invR.b +
                        value.cameraPoint.z * this.invR.c;
            float y = value.cameraPoint.x * this.invR.d + value.cameraPoint.y * this.invR.e +
                        value.cameraPoint.z * this.invR.f;
            float z = value.cameraPoint.x * this.invR.g + value.cameraPoint.y * this.invR.h +
                        value.cameraPoint.z * this.invR.i;

            value.colorPoint.x = (int)(x * c.fx / z + c.cx);
            value.colorPoint.y = (int)(y * c.fy / z + c.cy);

            if ((value.colorPoint.x < 0) ||
                (value.colorPoint.x > (this.previousSettings.colorWidth - 1)) ||
                (value.colorPoint.y < 0) ||
                (value.colorPoint.y > (this.previousSettings.colorHeight - 1)))
            {
                value.colorPoint.x = -1;
                value.colorPoint.y = -1;
            }

            return value;
        }

        private IEnumerator Start()
        {
            yield return null;
            if (Tof.TofArTofManager.Instance != null)
            {
                Tof.TofArTofManager.Instance.CalibrationSettingsLoaded.AddListener(UpdateSettingsCallback);
            }
            TofArManager.Instance.AddSubManager(this);
        }

        private bool isUnPaused = true;
        private bool IsStreamPausing { get; set; }

        /// <summary>
        /// *TODO+ B（使われるのでpublicのままにする）
        /// ストリームを停止する
        /// </summary>
        [Obsolete("PauseStream is obsolete and will be removed in a future version")]
        public void PauseStream()
        {
            this.isUnPaused = false;
            this.IsStreamPausing = this.IsStreamActive;
        }

        /// <summary>
        /// *TODO+ B（使われるのでpublicのままにする）
        /// ストリームを再開する
        /// </summary>
        [Obsolete("UnpauseStream is obsolete and will be removed in a future version")]
        public void UnpauseStream()
        {
            if (this.isUnPaused)
            {
                return;
            }
            this.isUnPaused = true;
            if (this.IsStreamPausing && !this.IsStreamActive)
            {
                this.IsStreamPausing = false;

                SetupConfigProperty();
            }
        }

        #region InternalManager

        private float GX, GY, OX, OY, DX, DY;
        private int[] colorXcache = new int[0];
        private int[] colorYcache = new int[0];
        private int[] depthXcache = new int[0];
        private int[] depthYcache = new int[0];
        private Tof.Matrix invR;
        private Tof.Vector invT;


        private CameraPointProperty DepthPointToCameraPoint(int X, int Y, Int16 Z)
        {
            if (Z < 100 || Z > 32000)
            {
                return new CameraPointProperty { x = float.NaN, y = float.NaN, z = float.NaN };
            }
            float sx = Z / this.previousSettings.d.fx;
            float sy = Z / this.previousSettings.d.fy;

            return new CameraPointProperty
            {
                x = (X - this.previousSettings.d.cx) * sx,
                y = (Y - this.previousSettings.d.cy) * sy,
                z = (float)Z
            };
        }

        /// <summary>
        /// ストリーム
        /// </summary>
        [Obsolete("Stream is obsolete and will be removed in a future version. Always return null")]
        public Stream Stream
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 実測FPS
        /// </summary>
        public float FrameRate { get; } = 0;

        //reference to make sure that the properties in the C++ are lined up properly
        private Tof.CalibrationSettingsProperty previousSettings;

        /// <summary>
        /// *TODO+ B deprecated?
        /// キャリブレーション設定
        /// </summary>
        public Tof.CalibrationSettingsProperty SettingsProperty
        {
            get
            {
                if (previousSettings != Tof.TofArTofManager.Instance?.CalibrationSettings)
                {
                    SetupConfigProperty();
                }

                return this.previousSettings;
            }
        }

        private void SetupConfigProperty()
        {
            this.previousSettings = Tof.TofArTofManager.Instance?.CalibrationSettings;

            if (previousSettings == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "No calibration settings available.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            float det, A, B, C, D, E, F, G, H, I;
            var R = this.previousSettings.R;
            var T = this.previousSettings.T;

            A = R.e * R.i - R.f * R.h;
            B = -(R.d * R.i - R.f * R.g);
            C = R.d * R.h - R.e * R.g;
            D = -(R.b * R.i - R.c * R.h);
            E = R.a * R.i - R.c * R.g;
            F = -(R.a * R.h - R.b * R.g);
            G = R.b * R.f - R.c * R.e;
            H = -(R.a * R.f - R.c * R.d);
            I = R.a * R.e - R.b * R.d;

            det = R.a * A + R.b * B + R.c * C;
            if (det == 0)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArCoordinateDepthToColor cannot find the determinant for matrix R.");
                throw new ApiException(new SensCord.Status() { cause = ErrorCause.InvalidArgument });
            }

            this.invR.a = A / det;
            this.invR.b = D / det;
            this.invR.c = G / det;
            this.invR.d = B / det;
            this.invR.e = E / det;
            this.invR.f = H / det;
            this.invR.g = C / det;
            this.invR.h = F / det;
            this.invR.i = I / det;

            this.invT.x = (-T.x) * this.invR.a + (-T.y) * this.invR.b + (-T.z) * this.invR.c;
            this.invT.y = (-T.x) * this.invR.d + (-T.y) * this.invR.e + (-T.z) * this.invR.f;
            this.invT.z = (-T.x) * this.invR.g + (-T.y) * this.invR.h + (-T.z) * this.invR.i;

            float dfx, dfy, dcx, dcy;
            dfx = this.previousSettings.d.fx;
            dfy = this.previousSettings.d.fy;
            dcx = this.previousSettings.d.cx;
            dcy = this.previousSettings.d.cy;

            float fx, fy, cx, cy;
            fx = this.previousSettings.c.fx;
            fy = this.previousSettings.c.fy;
            cx = this.previousSettings.c.cx;
            cy = this.previousSettings.c.cy;

            this.GX = dfx / fx / this.invR.a * this.invR.i;
            this.GY = dfy / fy / this.invR.e * this.invR.i;
            this.OX = dcx - (fx * this.invR.c / this.invR.i + cx) * this.GX;
            this.OY = dcy - (fy * this.invR.f / this.invR.i + cy) * this.GY;
            this.DX = (fx * this.invT.x / this.invR.i * this.GX) / 4.0f;
            this.DY = (fy * this.invT.y / this.invR.i * this.GY) / 4.0f;

            if (this.DX < this.DY)
            {
                this.DX = 0.0f;
            }
            else
            {
                this.DY = 0.0f;
            }

            int wOutDepth, hOutDepth, wInDepth, hInDepth;
            wOutDepth = this.previousSettings.colorWidth;
            hOutDepth = this.previousSettings.colorHeight;
            wInDepth = this.previousSettings.depthWidth;
            hInDepth = this.previousSettings.depthHeight;

            this.colorXcache = new int[wOutDepth];
            for (var x = 0; x < wOutDepth; x++)
            {
                int s = (int)(x * this.GX + this.OX + 0.5f);
                if (s < 0 || s >= wInDepth)
                {
                    s = -1;
                }
                this.colorXcache[x] = s;
            }

            this.colorYcache = new int[hOutDepth];
            for (var y = 0; y < hOutDepth; y++)
            {
                int s = (int)(y * this.GY + this.OY + 0.5f);
                if (s < 0 || s >= hInDepth)
                {
                    s = -1;
                }
                this.colorYcache[y] = s;
            }

            this.depthXcache = new int[wInDepth];
            for (var X = 0; X < wInDepth; X++)
            {
                int s = (int)((X - this.OX) / this.GX + 0.5f);
                if (s < 0 || s >= wOutDepth)
                {
                    s = -1;
                }
                this.depthXcache[X] = s;
            }

            depthYcache = new int[hInDepth];
            for (var Y = 0; Y < hInDepth; Y++)
            {
                int s = (int)((Y - this.OY) / this.GY + 0.5f);
                if (s < 0 || s >= hOutDepth)
                {
                    s = -1;
                }
                this.depthYcache[Y] = s;
            }
        }

        /// <summary>
        /// trueの場合ストリーミングを行っている
        /// </summary>
        [Obsolete("IsStreamActive is obsolete and will be removed in a future version")]
        public bool IsStreamActive { get; private set; }

        /// <summary>
        /// ストリーミングを停止する
        /// </summary>
        [Obsolete("StopStream is obsolete and will be removed in a future version")]
        public void StopStream()
        {
            this.IsStreamActive = false;
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        [Obsolete("StartStream is obsolete and will be removed in a future version")]
        public void StartStream()
        {
            this.IsStreamActive = true;
        }

        private void UpdateSettingsCallback(Tof.CalibrationSettingsProperty settings)
        {
            this.SetupConfigProperty();
        }

        /// <summary>
        /// オブジェクト破棄
        /// </summary>
        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArCoordinateManager.Dispose()");
            TofArManager.Instance?.RemoveSubManager(this);
            if (Tof.TofArTofManager.Instance != null)
            {
                Tof.TofArTofManager.Instance.CalibrationSettingsLoaded.RemoveListener(UpdateSettingsCallback);
            }
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        [Obsolete("GetProperty<T> is obsolete and will be removed in a future version. Always return null")]
        public T GetProperty<T>(T value) where T : class, IBaseProperty, new()
        {
            return null;
        }

        /// <summary>
        /// コンポーネントプロパティを設定する
        /// </summary>
        /// <typeparam name="T">入力パラメータクラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        [Obsolete("SetProperty<T> is obsolete and will be removed in a future version")]
        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
        }

        /// <summary>
        /// Propertyリスト取得
        /// </summary>
        /// <returns>Propertyのリスト</returns>
        [Obsolete("GetPropertyList is obsolete and will be removed in a future version. Always return empty list")]
        public string[] GetPropertyList()
        {
            return new string[0];
        }
        #endregion
    }
}



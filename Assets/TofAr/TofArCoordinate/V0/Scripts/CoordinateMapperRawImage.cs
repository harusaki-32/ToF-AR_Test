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
using TofAr.V0.Tof;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace TofAr.V0.Coordinate
{
    /// <summary>
    /// カメラ映像をuGUI RawImageオブジェクトに表示する
    /// <para>端末の回転方向に自動追従して映像を回転する。</para>
    /// </summary>
    public class CoordinateMapperRawImage : MonoBehaviour
    {
        /// <summary>
        /// <para>true: 端末の回転方向に応じて表示を回転させる</para>
        /// <para>false: 表示の自動回転を行わない</para>
        /// <para>デフォルト値：true</para>
        /// </summary>
        public bool AutoRotation = true;

        /// <summary>
        /// <para>true: RawImageの親オブジェクト領域内に最大化表示を行う</para>
        /// <para>false: RawImageの親オブジェクト領域内に最大化表示を行わない</para>
        /// <para>デフォルト値：true</para>
        /// </summary>
        public bool Maximize;

        /// <summary>
        /// アスペクト比
        /// </summary>
        public float AspectRatio = 1.333f;

        private RectTransform parentRectTransform;
        private RectTransform rectTransform;
        private Vector2 latestParentSize;

        private int imageRotation;

        private RawImage rawImage;
        private Texture2D depthTexture;
        private short[] depthTextureBuffer = new short[0];

        private short[] depthBuffer = new short[0];

        // managers
        private TofArCoordinateManager coordinateManager;
        private TofArTofManager tofManager;

        private int latestColorHeight;
        private int latestColorWidth;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (transform.parent != null)
            {
                parentRectTransform = transform.parent.GetComponent<RectTransform>();
            }
            rectTransform = GetComponent<RectTransform>();
        }
#endif

        private void Awake()
        {
            tofManager = TofArTofManager.Instance;
            coordinateManager = TofArCoordinateManager.Instance;
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
            rectTransform = GetComponent<RectTransform>();
            rawImage = GetComponent<RawImage>();
            depthTexture = new Texture2D(0, 0, TextureFormat.RGBA4444, false);
            depthTexture.filterMode = FilterMode.Point;
        }

        private void OnEnable()
        {
            TofArManager.OnDeviceOrientationUpdated += OnDeviceOrientationChanged;

            UpdateRotation();

            StartCoroutine(Process());
        }

        private void OnDisable()
        {
            TofArManager.OnDeviceOrientationUpdated -= OnDeviceOrientationChanged;

            StopCoroutine(Process());
        }

        void Update()
        {
            UpdateImageSize();
        }

        private void UpdateImageSize()
        {
            // get current aspect ratio
            bool aspectRatioUpdated = false;

            if (coordinateManager.SettingsProperty == null)
            {
                return;
            }

            var colorWidth = coordinateManager.SettingsProperty.colorWidth;
            var colorHeight = coordinateManager.SettingsProperty.colorHeight;

            if (latestColorWidth != colorWidth || latestColorHeight != colorHeight)
            {
                AspectRatio = (float)colorWidth / colorHeight;
                latestColorWidth = colorWidth;
                latestColorHeight = colorHeight;
                aspectRatioUpdated = true;
            }

            float width, height;
            if (Maximize)
            {
                var parentWidth = parentRectTransform.rect.width;
                var parentHeight = parentRectTransform.rect.height;

                if (imageRotation == 90 || imageRotation == 270)
                {
                    parentWidth = parentRectTransform.rect.height;
                    parentHeight = parentRectTransform.rect.width;
                }

                if (!aspectRatioUpdated &&
                    Mathf.Approximately(latestParentSize.x, parentWidth) &&
                    Mathf.Approximately(latestParentSize.y, parentHeight))
                {
                    return;
                }

                latestParentSize = new Vector2(parentWidth, parentHeight);

                width = parentWidth;
                height = parentHeight;

                var parentAspectRatio = parentWidth / parentHeight;
                if (parentAspectRatio > AspectRatio)
                {
                    width = height * AspectRatio;
                }
                else
                {
                    height = width / AspectRatio;
                }
            }
            else
            {
                if (!aspectRatioUpdated)
                {
                    return;
                }

                height = rectTransform.rect.height;
                width = height * AspectRatio;
            }

            rectTransform.sizeDelta = new Vector2(width, height);
        }

        private IEnumerator Process()
        {
            //process is called once before start is called so we have to wait
            yield return new WaitForEndOfFrame();
            //var currentDeviceOrientation = DeviceOrientation.Unknown;
            while (isActiveAndEnabled)
            {

                ProcessCoordinate();

                yield return null;
            }
        }

        private void UpdateRotation()
        {
            if (AutoRotation)
            {
                imageRotation = TofArManager.Instance.GetDeviceOrientation();

                if (rectTransform != null)
                {
                    rectTransform.localRotation = Quaternion.Euler(0f, 0f, imageRotation);
                }
            }
            else
            {
                imageRotation = (int)(rectTransform.localRotation.eulerAngles.z);
            }
        }

        private void OnDeviceOrientationChanged(DeviceOrientation previousDeviceOrientation, DeviceOrientation newDeviceOrientation)
        {
            UpdateRotation();
        }

        private bool MapToDepth(DepthPointProperty[] depthPoints, int width, int height, int depthWidth)
        {
            try
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int colorIndex = (y * width) + x;

                        if (depthPoints == null)
                        {
                            continue;
                        }

                        var depthPoint = depthPoints[colorIndex];

                        if (depthPoint.x == -1 || depthPoint.y == -1)
                        {
                            depthTextureBuffer[colorIndex] = 32001;
                        }
                        else
                        {
                            var depthIndex = depthPoint.x + depthPoint.y * depthWidth;

                            if (depthBuffer == null || depthIndex >= depthBuffer.Length)
                            {
                                depthTextureBuffer[colorIndex] = 32001;
                            }
                            else
                            {
                                depthTextureBuffer[colorIndex] = depthBuffer[depthIndex];
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                return false;
            }

            return true;
        }

        private void ProcessCoordinate()
        {
            if (tofManager == null || coordinateManager == null)
            {
                return;
            }
            if (!tofManager.IsStreamActive)
            {
                return;
            }
            if (tofManager.DepthData == null || tofManager.DepthData.Data == null || coordinateManager.SettingsProperty == null)
            {
                return;
            }

            int depthPointCount = tofManager.DepthData.Data.Length;

            if (depthPointCount != depthBuffer.Length)
            {
                Array.Resize(ref depthBuffer, depthPointCount);
            }
            Buffer.BlockCopy(tofManager.DepthData.Data, 0, depthBuffer, 0, depthPointCount * 2);

            var colorToDepthConfig = new ColorToDepthProperty
            {
                depthFrame = depthBuffer
            };

            var settingsProperty = coordinateManager.SettingsProperty;
            var height = settingsProperty.colorHeight;
            var width = settingsProperty.colorWidth;
            var depthWidth = settingsProperty.depthWidth;
            var depthHeight = settingsProperty.depthHeight;

            if (tofManager.DepthData.Data.Length != depthWidth * depthHeight)
            {
                return;
            }
            var colorToDepth = coordinateManager.GetProperty<ColorToDepthProperty>(colorToDepthConfig);
            if (colorToDepth == null || colorToDepth.depthPoints == null)
            {
                return;
            }
            DepthPointProperty[] depthPoints = (DepthPointProperty[])colorToDepth.depthPoints.Clone();

            if (depthTextureBuffer.Length != width * height)
            {
                Array.Resize(ref depthTextureBuffer, width * height);
            }

            if (!MapToDepth(depthPoints, width, height, depthWidth))
            {
                return;
            }

            if (depthTextureBuffer.Length == width * height)
            {
                rawImage.texture = ShortToTexture2D(depthTextureBuffer, width, height);
            }
        }

        private Texture2D ShortToTexture2D(short[] tex, int width, int height)
        {
            if (depthTexture.width != width || depthTexture.height != height)
            {
#if UNITY_2021_1_OR_NEWER
                depthTexture.Reinitialize(width, height);
#else
                depthTexture.Resize(width, height);
#endif
            }
            GCHandle handle = GCHandle.Alloc(tex, GCHandleType.Pinned);
            try
            {
                depthTexture.LoadRawTextureData(handle.AddrOfPinnedObject(), width * height * sizeof(short));
                depthTexture.Apply();
            }
            catch (UnityException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            finally
            {
                handle.Free();
            }
            return depthTexture;
        }
    }
}

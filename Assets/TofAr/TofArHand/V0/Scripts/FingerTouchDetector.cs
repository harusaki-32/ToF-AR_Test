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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TofAr.V0.Tof;
using TensorFlowLite.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// 指先の平面タッチ推定を行う。 指先の平面タッチ推定は指先が平面に触れていることを推定する。
    /// </summary>
    public class FingerTouchDetector : MonoBehaviour
    {
        #region Public Interfaces

        /// <summary>
        /// 手種別
        /// </summary>
        public enum HandSide
        {
            /// <summary>
            /// 右手
            /// </summary>
            RightHand,
            /// <summary>
            /// 左手
            /// </summary>
            LeftHand,
        }

        /// <summary>
        /// タッチ状態
        /// </summary>
        public enum TouchState
        {
            /// <summary>
            /// タッチしていない
            /// </summary>
            NoTouch,
            /// <summary>
            /// タッチしている
            /// </summary>
            Touch,
        }

        /// <summary>
        /// 認識結果クラス
        /// </summary>
        public class DetectResult
        {
            /// <summary>
            /// 認識結果
            /// </summary>
            public IDictionary<HandSide, IDictionary<HandPointIndex, TouchState>> Result = new Dictionary<HandSide, IDictionary<HandPointIndex, TouchState>>();
        }

        [SerializeField]
        private TFLiteRuntime.ExecMode execMode = TFLiteRuntime.ExecMode.EXEC_MODE_GPU;

        /// <summary>
        /// TensorFlowLiteの実行モード
        /// </summary>
        public TFLiteRuntime.ExecMode ExecMode
        {
            get => this.execMode;
            set
            {
                if (this.execMode != value)
                {
                    this.execMode = value;
                    this.CleanupDetector();
                    this.PrepareModelFile();
                    this.PrepareDetector();
                }
            }
        }

        [SerializeField]
        private bool autoStart = true;

        /// <summary>
        /// 認識対象の指先
        /// </summary>
        [Flags]
        public enum TargetFinger
        {
            /// <summary>
            /// 対象なし
            /// </summary>
            None = 0x0000,

            /// <summary>
            /// 右親指
            /// </summary>
            RightThumb = 0x0001,

            /// <summary>
            /// 右人差し指
            /// </summary>
            RightIndex = 0x0002,

            /// <summary>
            /// 右中指
            /// </summary>
            RightMiddle = 0x0004,

            /// <summary>
            /// 右薬指
            /// </summary>
            RightRing = 0x0008,

            /// <summary>
            /// 右小指
            /// </summary>
            RightPinky = 0x0010,

            /// <summary>
            /// 左親指
            /// </summary>
            LeftThumb = 0x0020,

            /// <summary>
            /// 左人差し指
            /// </summary>
            LeftIndex = 0x0040,

            /// <summary>
            /// 左中指
            /// </summary>
            LeftMiddle = 0x0080,

            /// <summary>
            /// 左薬指
            /// </summary>
            LeftRing = 0x0100,

            /// <summary>
            /// 左小指
            /// </summary>
            LeftPinky = 0x0200,
        }

        /// <summary>
        /// 認識対象の指先。複数選択可能
        /// <para>デフォルト: RightIndex(右手の人差し指)</para>
        /// </summary>
        public TargetFinger targetFingers = TargetFinger.RightIndex;

        /// <summary>
        /// 推定完了時イベント
        /// </summary>
        [Serializable]
        public class FingerTouchDetectedEvent : UnityEvent<DetectResult> { }
        /// <summary>
        /// 推定結果通知イベント
        /// </summary>
        [SerializeField]
        public FingerTouchDetectedEvent OnFingerTouchDetected;
        /// <summary>
        /// trueの場合推定中である
        /// </summary>
        public bool IsEstimating { get; private set; } = false;

        #endregion

        #region Private Fields

        private string internalModelPath = "finger_touch.tflite";
        private string overrideModelPath = "/data/local/tmp/tofar/config/finger_touch.tflite.bytes";

        private SynchronizationContext sc = SynchronizationContext.Current;

        // Sky Detecter (DNN)
        private TFLiteRuntime touchDetecter;

        private short[] depthBuffer;
        // depth map width & height
        private int dw, dh;

        private float[] input;

        private float fx = 0, fy = 0, cx = 0, cy = 0;

        private const int cropXY = 60;

        private string ModelFilePath { get; set; } = string.Empty;

        private bool IsIntialized { get; set; } = false;

        private ScreenOrientation screenOrientation = ScreenOrientation.Portrait;

        #endregion Private Fields

        void Start()
        {
            this.IsIntialized = false;
            this.IsEstimating = false;
            this.ModelFilePath = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}finger_touch.tflite.bytes";
            this.sc = SynchronizationContext.Current;

            if (File.Exists(overrideModelPath))
            {
                var fi = new FileInfo(overrideModelPath);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"model: {overrideModelPath} {fi.CreationTime.ToString("yyyy/MM/dd HH:mm:ss")}");
                this.ModelFilePath = overrideModelPath;
            }
            else
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"model: internal");
            }

            this.PrepareModelFile();
            this.PrepareDetector();
            if (this.autoStart)
            {
                this.StartEstimation();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                this.CleanupDetector();
                this.CleanupModelFile();
            }
            else
            {
                this.PrepareModelFile();
                this.PrepareDetector();
            }
        }

        private void OnEnable()
        {
            this.PrepareModelFile();
            this.PrepareDetector();
        }

        private void OnDisable()
        {
            this.CleanupDetector();
            this.CleanupModelFile();
        }

        /// <summary>
        /// 推定処理を開始する
        /// </summary>
        public void StartEstimation()
        {
            if (!this.IsIntialized)
            {
                throw new InvalidOperationException("FingerTouchDetector has not initialized.");
            }
            this.IsEstimating = true;
        }
        /// <summary>
        /// 推定処理を停止する
        /// </summary>
        public void StopEstimation()
        {
            if (!this.IsIntialized)
            {
                throw new InvalidOperationException("FingerTouchDetector has not initialized.");
            }
            this.IsEstimating = false;
        }

        private void PrepareModelFile()
        {
            if ((overrideModelPath == this.ModelFilePath) || string.IsNullOrEmpty(this.ModelFilePath))
            {
                return;
            }
            var asset = Resources.Load(internalModelPath) as TextAsset;
            if (asset != null)
            {
                var reader = new BinaryReader(new MemoryStream(asset.bytes));
                File.WriteAllBytes(this.ModelFilePath, reader.ReadBytes(asset.bytes.Length));
            }
        }

        private void CleanupModelFile()
        {
            if ((overrideModelPath == this.ModelFilePath) || string.IsNullOrEmpty(this.ModelFilePath))
            {
                return;
            }
            if (File.Exists(this.ModelFilePath))
            {
                File.Delete(this.ModelFilePath);
            }
        }

        private void PrepareDetector()
        {
            if (string.IsNullOrEmpty(this.ModelFilePath))
            {
                return;
            }

            try
            {
                // TFLite
#if UNITY_EDITOR  || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                this.touchDetecter = new TFLiteRuntime(this.ModelFilePath, TFLiteRuntime.ExecMode.EXEC_MODE_CPU, 2);
#else
                this.touchDetecter = new TFLiteRuntime(this.ModelFilePath, this.execMode);
#endif
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"[FingerTouchDetector] Failed to initialize TFLite: {e.Message}");
            }
            finally
            {
                if (this.ModelFilePath != null && File.Exists(this.ModelFilePath))
                {
                    File.Delete(this.ModelFilePath);
                }
            }

            this.input = touchDetecter.getInputBuffer()[0];

            TofArTofManager.OnFrameArrived += this.TofFrameArrived;
            TofArHandManager.OnFrameArrived += this.HandFrameArrived;
            TofArManager.OnScreenOrientationUpdated += this.ScreenOrientationUpdated;

            this.IsIntialized = true;
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"FingerTouchDetector initialized.");
        }

        private void CleanupDetector()
        {
            TofArTofManager.OnFrameArrived -= this.TofFrameArrived;
            TofArHandManager.OnFrameArrived -= this.HandFrameArrived;
            TofArManager.OnScreenOrientationUpdated -= this.ScreenOrientationUpdated;

            this.input = null;
            this.touchDetecter?.Dispose();
            this.touchDetecter = null;
            this.IsIntialized = false;
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"FingerTouchDetector cleanuped.");
        }

        private void ScreenOrientationUpdated(ScreenOrientation previousScreenOrientation, ScreenOrientation newScreenOrientation)
        {
            this.screenOrientation = newScreenOrientation;
        }

        private void TofFrameArrived(object sender)
        {
            if (!this.IsEstimating)
            {
                return;
            }

            if (fx == 0)
            {
                var prop = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
                var intrinsics = prop.intrinsics;
                fx = intrinsics.fx;
                fy = intrinsics.fx;
                cx = intrinsics.cx;
                cy = intrinsics.cy;

                var cam = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
                dw = cam.width;
                dh = cam.height;
            }

            DepthData dd = TofArTofManager.Instance?.DepthData;
            if (dd == null)
            {
                return;
            }
            short[] depth = dd.Data;
            if (depth == null)
            {
                return;
            }

            if (depthBuffer == null || depthBuffer.Length != depth.Length)
            {
                depthBuffer = new short[depth.Length];
            }

            lock (depthBuffer)
            {
                for (int i = 0; i < depth.Length; i++)
                {
                    depthBuffer[i] = depth[i];
                }
            }
        }

        private static IDictionary<TargetFinger, HandPointIndex> fingerHandPointMap = new Dictionary<TargetFinger, HandPointIndex>()
        {
            { TargetFinger.RightThumb,  HandPointIndex.ThumbTip },
            { TargetFinger.RightIndex,  HandPointIndex.IndexTip },
            { TargetFinger.RightMiddle, HandPointIndex.MidTip },
            { TargetFinger.RightRing,   HandPointIndex.RingTip },
            { TargetFinger.RightPinky,  HandPointIndex.PinkyTip },
            { TargetFinger.LeftThumb,   HandPointIndex.ThumbTip },
            { TargetFinger.LeftIndex,   HandPointIndex.IndexTip },
            { TargetFinger.LeftMiddle,  HandPointIndex.MidTip },
            { TargetFinger.LeftRing,    HandPointIndex.RingTip },
            { TargetFinger.LeftPinky,   HandPointIndex.PinkyTip },
        };

        private static IDictionary<TargetFinger, IList<HandStatus>> allowHandStatusMap = new Dictionary<TargetFinger, IList<HandStatus>>()
        {
            { TargetFinger.RightThumb,  new List<HandStatus>() { HandStatus.BothHands, HandStatus.RightHand } },
            { TargetFinger.RightIndex,  new List<HandStatus>() { HandStatus.BothHands, HandStatus.RightHand } },
            { TargetFinger.RightMiddle, new List<HandStatus>() { HandStatus.BothHands, HandStatus.RightHand } },
            { TargetFinger.RightRing,   new List<HandStatus>() { HandStatus.BothHands, HandStatus.RightHand } },
            { TargetFinger.RightPinky,  new List<HandStatus>() { HandStatus.BothHands, HandStatus.RightHand } },
            { TargetFinger.LeftThumb,   new List<HandStatus>() { HandStatus.BothHands, HandStatus.LeftHand } },
            { TargetFinger.LeftIndex,   new List<HandStatus>() { HandStatus.BothHands, HandStatus.LeftHand } },
            { TargetFinger.LeftMiddle,  new List<HandStatus>() { HandStatus.BothHands, HandStatus.LeftHand } },
            { TargetFinger.LeftRing,    new List<HandStatus>() { HandStatus.BothHands, HandStatus.LeftHand } },
            { TargetFinger.LeftPinky,   new List<HandStatus>() { HandStatus.BothHands, HandStatus.LeftHand } },
        };

        private static IDictionary<TargetFinger, IList<PoseIndex>> disAllowPoseMap = new Dictionary<TargetFinger, IList<PoseIndex>>()
        {
            { TargetFinger.RightThumb,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    PoseIndex.Peace,
                    PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    PoseIndex.OK,
                    PoseIndex.MiddleAndRingIn,
                    PoseIndex.ThreeFingers,
                    //PoseIndex.ThumbUp,
                    //PoseIndex.Tel,
                    PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    //PoseIndex.Heart,
                }
            },
            { TargetFinger.RightIndex,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    //PoseIndex.Shot,
                    //PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    //PoseIndex.Pistol,
                    PoseIndex.OK,
                    //PoseIndex.MiddleAndRingIn,
                    //PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    PoseIndex.Tel,
                    //PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },
            { TargetFinger.RightMiddle,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    //PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    //PoseIndex.OK,
                    PoseIndex.MiddleAndRingIn,
                    //PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    PoseIndex.Tel,
                    PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },
            { TargetFinger.RightRing,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    //PoseIndex.OK,
                    PoseIndex.MiddleAndRingIn,
                    //PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    PoseIndex.Tel,
                    PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },
            { TargetFinger.RightPinky,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    //PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    //PoseIndex.OK,
                    //PoseIndex.MiddleAndRingIn,
                    PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    //PoseIndex.Tel,
                    //PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },

            { TargetFinger.LeftThumb,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    PoseIndex.Peace,
                    PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    PoseIndex.OK,
                    PoseIndex.MiddleAndRingIn,
                    PoseIndex.ThreeFingers,
                    //PoseIndex.ThumbUp,
                    //PoseIndex.Tel,
                    PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    //PoseIndex.Heart,
                }
            },
            { TargetFinger.LeftIndex,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    //PoseIndex.Shot,
                    //PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    //PoseIndex.Pistol,
                    PoseIndex.OK,
                    //PoseIndex.MiddleAndRingIn,
                    //PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    PoseIndex.Tel,
                    //PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },
            { TargetFinger.LeftMiddle,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    //PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    //PoseIndex.OK,
                    PoseIndex.MiddleAndRingIn,
                    //PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    PoseIndex.Tel,
                    PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },
            { TargetFinger.LeftRing,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    //PoseIndex.OK,
                    PoseIndex.MiddleAndRingIn,
                    //PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    PoseIndex.Tel,
                    PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },
            { TargetFinger.LeftPinky,
                new List<PoseIndex>()
                {
                    PoseIndex.Fist,
                    PoseIndex.Shot,
                    PoseIndex.Peace,
                    //PoseIndex.ThumbIn,
                    //PoseIndex.PinkyOut,
                    //PoseIndex.OpenPalm,
                    PoseIndex.Pistol,
                    //PoseIndex.OK,
                    //PoseIndex.MiddleAndRingIn,
                    PoseIndex.ThreeFingers,
                    PoseIndex.ThumbUp,
                    //PoseIndex.Tel,
                    //PoseIndex.Fox,
                    PoseIndex.PreSnap,
                    PoseIndex.Heart,
                }
            },

        };

        private void HandFrameArrived(object sender)
        {
            if (!this.IsEstimating)
            {
                return;
            }

            if (depthBuffer == null)
            {
                return;
            }

            DetectResult result = new DetectResult();

            PoseIndex leftPose, rightPose;
            TofArHandManager.Instance.HandData.GetPoseIndex(out leftPose, out rightPose);

            var hd = TofArHandManager.Instance.HandData.Data;
            foreach (TargetFinger targetFinger in Enum.GetValues(typeof(TargetFinger)))
            {
                var invalidPose = false;
                if (targetFinger == TargetFinger.None)
                {
                    continue;
                }
                var handSide = targetFinger.HasFlag(TargetFinger.RightThumb) || targetFinger.HasFlag(TargetFinger.RightIndex)
                    || targetFinger.HasFlag(TargetFinger.RightMiddle) || targetFinger.HasFlag(TargetFinger.RightRing)
                    || targetFinger.HasFlag(TargetFinger.RightPinky) ? HandSide.RightHand : HandSide.LeftHand;
                if (this.targetFingers.HasFlag(targetFinger))
                {
                    if (allowHandStatusMap[targetFinger]?.Contains(hd.handStatus) == true)
                    {
                        var pose = handSide == HandSide.RightHand ? rightPose : leftPose;
                        if (disAllowPoseMap[targetFinger]?.Contains(pose) == true)
                        {
                            invalidPose = true;
                        }
                    }
                    else
                    {
                        invalidPose = true;
                    }
                }
                else
                {
                    invalidPose = true;
                }

                if (!invalidPose)
                {
                    EstimateTouch(targetFinger, handSide, hd, ref result);
                }
            }
            this.sc?.Post((s) =>
            {
                this.OnFingerTouchDetected?.Invoke(result);
            }, null);
        }

        private DetectResult EstimateTouch(TargetFinger targetFinger, HandSide handSide, RecognizeResultProperty handData, ref DetectResult result)
        {
            var joints = (handSide == HandSide.RightHand) ? handData.featurePointsRight : handData.featurePointsLeft;
            var handPointIndex = fingerHandPointMap[targetFinger];

            Vector3 indexTip = joints[(int)handPointIndex];

            FillInput(indexTip);

            // TFLite 処理
            float[][] output = touchDetecter.forward();

            //Debug.Log("output[0].Length=" + output[0].Length);
            //Debug.Log("output[0][0]=" + output[0][0]);
            //Debug.Log("output[0][1]=" + output[0][1]);

            if (!result.Result.ContainsKey(handSide))
            {
                result.Result.Add(handSide, new Dictionary<HandPointIndex, TouchState>());
            }

            try
            {
                if (output[0][1] > output[0][0])
                {
                    result.Result[handSide].Add(handPointIndex, TouchState.Touch);
                    //TofArManager.Logger.WriteLog(LogLevel.Debug, $"FingerTouchDetector: Touch!!");
                }
                else
                {
                    result.Result[handSide].Add(handPointIndex, TouchState.NoTouch);
                    //TofArManager.Logger.WriteLog(LogLevel.Debug, $"FingerTouchDetector: No Touch");
                }
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                throw;
            }
            catch (IndexOutOfRangeException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                throw;
            }

            return result;
        }

        private void FillInput(Vector3 indexTip)
        {
            // 手の位置から 2D へ射影
            //tx, ty: デプスマップ上の座標
            float tx = fx * indexTip.x / indexTip.z + cx;
            float ty = -fy * indexTip.y / indexTip.z + cy;

            //Debug.Log("fx: " + fx + " cx: " + cx);
            //Debug.Log("tx: " + tx + " ty: " + ty);

            // 201118 DOI 60mm 切り抜きにする
            float pixel60mmx = fx * 0.06f / indexTip.z;
            float pixel60mmy = fy * 0.06f / indexTip.z;

            // 解像度は 60x60 (DNN の最初に 2x2 圧縮される)
            // 2D の周辺をクロップ
            int ex = (int)(tx + (cropXY / 2) * pixel60mmx / cropXY);
            int ey = (int)(ty + (cropXY / 2) * pixel60mmy / cropXY);
            for (int y = 0; y < cropXY; y++)
            {
                //int sy = (int)(ty - CROP_XY / 2 + y);
                // 201118 DOI 60mm 切り抜きにする
                int sy = (int)(ty + (y - cropXY / 2) * pixel60mmy / cropXY);

                for (int x = 0; x < cropXY; x++)
                {
                    //int sx = (int)(tx - CROP_XY / 2 + x);
                    // 201118 DOI 60mm 切り抜きにする
                    int sx = (int)(tx + (x - cropXY / 2) * pixel60mmx / cropXY);

                    float val;
                    if (sy < 0 || dh <= sy || sx < 0 || dw <= sx)
                    {
                        val = 1;
                    }
                    else
                    {
                        // depth の値は手中心 +- 127mm であってる？
                        short depth = 0;
                        var index = -1;
                        switch (this.screenOrientation)
                        {
                            case ScreenOrientation.Portrait:
                                index = sx * dw + (ey - sy);
                                break;
                            case ScreenOrientation.PortraitUpsideDown:
                                index = (ex - sx) * dw + sy;
                                break;
                            case ScreenOrientation.LandscapeRight:
                                index = (ey - sy) * dw + (ex - sx);
                                break;
                            default:  //LandscapeLeft
                                index = sy * dw + sx;
                                break;
                        }
                        if ((0 <= index) && (index < this.depthBuffer.Length))
                        {
                            depth = depthBuffer[index];
                        }
                        val = Mathf.Max(0, Mathf.Min(1, (depth - indexTip.z * 1000 + 127) / 255.0f));
                    }

                    input[y * cropXY + x] = val;
                }
            }
        }
    }
}

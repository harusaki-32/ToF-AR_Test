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
using TensorFlowLite.Runtime;
using TofAr.V0.Color;
using TofAr.V0.Tof;
using UnityEngine;

namespace TofAr.V0.Face
{
    /// <summary>
    /// 対象プラットフォーム
    /// </summary>
    public enum TargetPlatform
    {
        /// <summary>
        /// 無し
        /// </summary>
        None = 0,
        /// <summary>
        /// Android
        /// </summary>
        Android = 1,
        /// <summary>
        /// iOS
        /// </summary>
        iOS = 2,
        /// <summary>
        /// 全て
        /// </summary>
        Everything = ~0
    }

    /// <summary>
    /// 入力ソース
    /// </summary>
    public enum InputSource
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Default = 0,
        /// <summary>
        /// カラー
        /// </summary>
        Color = 1,
        /// <summary>
        /// コンフィデンス
        /// </summary>
        Confidence = 2
    }

    /// <summary>
    /// 認識速度と精度
    /// </summary>
    public enum ProcessMode
    {
        /// <summary>
        /// 低速高精度
        /// </summary>
        Accurate = 0,
        /// <summary>
        /// 高速低精度
        /// </summary>
        Fast = 1
    }

    /// <summary>
    /// Face推定処理クラス
    /// </summary>
    public class FaceEstimator : MonoBehaviour
    {
        private enum NetworkType
        {
            FaceDetection,
            FaceLandmarks
        }

        // DNN input/output size
        private const int WidthLandmark = 192;
        private const int HeightLandmark = 192;

        private const int WidthDetection = 256;
        private const int HeightDetection = 256;

        // feature point count (ARCore)
        private const int POINT_COUNT = 468;
        private const int POINT_COUNT_IRIS = 10;

        [HideInInspector]
        public TargetPlatform TargetPlatform = TargetPlatform.Android;

        /// <summary>
        /// Face推定で使用するTFLiteのExecMode
        /// </summary>        
        public TFLiteRuntime.ExecMode ExecMode = TFLiteRuntime.ExecMode.EXEC_MODE_CPU;

        /// <summary>
        /// Face検出で使用するTFLiteのExecMode
        /// </summary>        
        public TFLiteRuntime.ExecMode ExecModeDetection = TFLiteRuntime.ExecMode.EXEC_MODE_CPU;

        /// <summary>
        /// Face推定で使用するスレッド数
        /// </summary>        
        public int ThreadsNum = 1;

        /// <summary>
        /// Face検出で使用するスレッド数
        /// </summary>        
        public int ThreadsNumDetection = 1;

        private TFLiteRuntime faceLandmarkRuntime;
        private TFLiteRuntime faceDetectionRuntime;

        private FaceDetector faceDetector;
        private string networkNameLandmarksHighPerformance = "facelandmark.tflite";
        private string networkNameLandmarksHighAccuracy = "face_landmark_with_attention.tflite";
        private string networkNameDetection = "facedetectionback.tflite";

        /// <summary>
        /// Face推定の認識速度と精度
        /// </summary>
        public ProcessMode LandmarkProcessMode = ProcessMode.Accurate;
        private ProcessMode landmarkProcessMode;

        // DNN input buffer (allocated in Runtime side)
        private float[] inputLandmarks;
        private float[] inputDetection;

        // internal variables
        // pixel position
        private float faceCenterX = -1, faceCenterY = -1;
        // pixel size
        private float faceSize;

        /// <summary>
        /// face feature 2D points (on Screen)
        /// </summary>
        private Vector2[] points;


        private Vector3[] globalPositions;
        private Vector3[] tmpLocalPositions;

        /// <summary>
        /// face feature 3D points (local)
        /// </summary>
        private TofArVector3[] localPositions;
        private Vector3 facePosition;
        private Quaternion faceRotation;

        /// <summary>
        /// Color intrinsics
        /// </summary>
        private float fx = 0;
        private float fy = 0;
        private float cx = 0;
        private float cy = 0;

        private int colorWidth = 0;
        private int colorHeight = 0;

        private float fxDepth;
        private float fyDepth;
        private float cxDepth;
        private float cyDepth;

        private int tofWidth;
        private int tofHeight;

        /// <summary>
        /// Axis
        /// </summary>
        private Vector3 axisY = new Vector3();
        private Vector3 axisX = new Vector3();

        private int rotation;

        private FaceDetectorType initDetectorType;

        private bool enableNoiseReduction = true;
        private int nrTaps = 5;
        private float nrValue = 0.9f;

        [SerializeField]
        private InputSource inputSourceType;

        [System.Serializable]
        private class DeviceAttributesJson
        {
            public string defaultFaceEstimatorInputSource = null;
            public bool enableFaceNR = true;
            public int faceNRTaps = 5;
            public float faceNRValue = 0.9f;
        }

        /// <summary>
        /// 入力ソースタイプ
        /// </summary>
        public InputSource InputSourceType
        {
            get { return inputSourceType; }

            set
            {
                inputSourceType = value;
                if (inputSourceType == InputSource.Default)
                {
                    // read from config
                    var attributesString = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>().TrimmedDeviceAttributesString;
                    string inputSourceTypeString = JsonUtility.FromJson<DeviceAttributesJson>(attributesString)?.defaultFaceEstimatorInputSource;

                    switch (inputSourceTypeString)
                    {
                        case "confidence":
                            inputSourceType = InputSource.Confidence; break;
                        case "color":
                        default:
                            inputSourceType = InputSource.Color; break;
                    }
                }
            }
        }

        /// <summary>
        /// trueの場合Face推定を使用可能である
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                return isAvailable;
            }
        }
        private bool isAvailable = false;

        void Awake()
        {
            this.InputSourceType = inputSourceType;
            

            if (TofArManager.Instance.UsingIos)
            {
                if ((TargetPlatform != TargetPlatform.iOS && TargetPlatform != TargetPlatform.Everything))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"FaceEstimator deactivated for ios");
                    isAvailable = false;
                    return;
                }
            }
            else
            {
                if ((TargetPlatform != TargetPlatform.Android && TargetPlatform != TargetPlatform.Everything))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"FaceEstimator deactivated for Android");
                    isAvailable = false;
                    return;
                }
            }

            // check for tf file
            TextAsset asset = Resources.Load(networkNameLandmarksHighPerformance) as TextAsset;
            if (asset == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to find {networkNameLandmarksHighPerformance} inside Resources. Deactivating FaceEstimator.");
                isAvailable = false;
                return;
            }

            if (!TofArManager.Instance.UsingIos)
            {
                asset = Resources.Load(networkNameLandmarksHighAccuracy) as TextAsset;
                if (asset == null)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to find {networkNameLandmarksHighAccuracy} inside Resources. Deactivating FaceEstimator.");
                    isAvailable = false;
                    return;
                }
            }
            else
            {
                if (this.LandmarkProcessMode == ProcessMode.Accurate)
                {
                    this.LandmarkProcessMode = ProcessMode.Fast;
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Process Mode Accurate not supported on this platform. Switching to Fast");
                }
            }
            

            asset = Resources.Load(networkNameDetection) as TextAsset;
            if (asset == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to find {networkNameDetection} inside Resources. Deactivating FaceEstimator.");
                isAvailable = false;
                return;
            }

            isAvailable = true;

            points = new Vector2[POINT_COUNT];
            globalPositions = new Vector3[POINT_COUNT];

            int pointCount = this.landmarkProcessMode == ProcessMode.Accurate ? POINT_COUNT + POINT_COUNT_IRIS : POINT_COUNT;
            localPositions = new TofArVector3[pointCount];
            tmpLocalPositions = new Vector3[pointCount];
        }

        private void OnEnable()
        {
            if (isAvailable)
            {
                TofArTofManager.OnStreamStarted += OnTofStreamStarted;
                TofArTofManager.OnFrameArrived += OnTofFrameArrived;
                TofArColorManager.OnStreamStarted += OnColorStreamStarted;
                TofArColorManager.OnFrameArrived += OnColorFrameArrived;
                TofArFaceManager.OnStreamStarted += OnFaceStreamStarted;
                TofArTofManager.Instance?.CalibrationSettingsLoaded.AddListener(OnCalibrationLoaded);
                this.initDetectorType = FaceDetectorType.Internal_ARKit;
                SetDetectorType();
            }
        }

        private void OnDisable()
        {
            if (isAvailable)
            {
                TofArFaceManager.OnStreamStarted -= OnFaceStreamStarted;
                TofArColorManager.OnFrameArrived -= OnColorFrameArrived;
                TofArColorManager.OnStreamStarted -= OnColorStreamStarted;
                TofArTofManager.OnStreamStarted -= OnTofStreamStarted;
                TofArTofManager.OnFrameArrived -= OnTofFrameArrived;
                TofArTofManager.Instance?.CalibrationSettingsLoaded.RemoveListener(OnCalibrationLoaded);
                TofArFaceManager.Instance.DetectorType = initDetectorType;
            }
        }

        private void SetDetectorType()
        {
            TofArFaceManager.Instance.DetectorType = FaceDetectorType.External;

            if (TofArFaceManager.Instance.IsStreamActive)
            {
                OnFaceStreamStarted(null);
            }

            if (TofArColorManager.Instance?.IsStreamActive == true)
            {
                OnColorStreamStarted(null, null);
            }

            if (TofArTofManager.Instance?.IsStreamActive == true)
            {
                OnTofStreamStarted(null, null, null, null);
            }
        }

        private void OnColorStreamStarted(object sender, Texture2D ColorTexture)
        {
            ResolutionProperty config = TofArColorManager.Instance.GetProperty<ResolutionProperty>();

            if (config != null)
            {
                this.colorWidth = config.width;
                this.colorHeight = config.height;
            }

            this.faceCenterX = -1;
            this.faceCenterY = -1;

            GetIntrinsics();
        }

        private void OnTofStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, PointCloudData pointCloudData)
        {
            Camera2ConfigurationProperty conf = TofArTofManager.Instance.GetProperty<Camera2ConfigurationProperty>();

            if (conf != null)
            {
                this.tofWidth = conf.width;
                this.tofHeight = conf.height;
            }

            this.faceCenterX = -1;
            this.faceCenterY = -1;

            GetIntrinsics();
        }

        private void OnCalibrationLoaded(CalibrationSettingsProperty calibrationSettings)
        {
            GetIntrinsics();
        }

        private void OnFaceStreamStarted(object sender)
        {
            lock (this)
            {
                if (TofArManager.Instance.UsingIos && this.LandmarkProcessMode == ProcessMode.Accurate)
                {
                    this.LandmarkProcessMode = ProcessMode.Fast;
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Process Mode Accurate not supported on this platform. Switching to Fast");
                }
                this.landmarkProcessMode = this.LandmarkProcessMode;

                string networkFaceLandmarkPath = null;
                string networkFaceSsdPath = null;
                try
                {
                    if (this.landmarkProcessMode == ProcessMode.Fast)
                    {
                        networkFaceLandmarkPath = LoadFileFromResources(networkNameLandmarksHighPerformance);
                    }
                    else
                    {
                        networkFaceLandmarkPath = LoadFileFromResources(networkNameLandmarksHighAccuracy);
                    }
                    networkFaceSsdPath = LoadFileFromResources(networkNameDetection);

                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"[FaceEstimator Init] Initialize with execMode={this.ExecMode} and threadsNum={this.ThreadsNum}");

                    faceLandmarkRuntime = new TFLiteRuntime(networkFaceLandmarkPath, this.ExecMode, this.ThreadsNum);
                    faceDetectionRuntime = new TFLiteRuntime(networkFaceSsdPath, this.ExecModeDetection, this.ThreadsNumDetection);
                }
                catch (Exception e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"[FaceEstimator Init] Failed to initialize TFLite: {e.Message}");
                }
                finally
                {
                    if (networkFaceLandmarkPath != null && File.Exists(networkFaceLandmarkPath))
                    {
                        File.Delete(networkFaceLandmarkPath);
                    }

                    if (networkFaceSsdPath != null && File.Exists(networkFaceSsdPath))
                    {
                        File.Delete(networkFaceSsdPath);
                    }
                }

                if (faceLandmarkRuntime == null || faceDetectionRuntime == null)
                {
                    return;
                }

                faceDetector = new FaceDetector();

                inputLandmarks = faceLandmarkRuntime.getInputBuffer()[0];
                inputDetection = faceDetectionRuntime.getInputBuffer()[0];

                int pointCount = this.landmarkProcessMode == ProcessMode.Accurate ? POINT_COUNT + POINT_COUNT_IRIS : POINT_COUNT;

                Array.Resize(ref localPositions, pointCount);
                Array.Resize(ref tmpLocalPositions, pointCount);


                var attributesString = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>().TrimmedDeviceAttributesString;
                var attributes = JsonUtility.FromJson<DeviceAttributesJson>(attributesString);
                if (attributes != null)
                {
                    this.enableNoiseReduction = attributes.enableFaceNR;
                    this.nrTaps = attributes.faceNRTaps;
                    this.nrValue = attributes.faceNRValue;
                }
            }
        }

        private void GetIntrinsics()
        {
            TofArTofManager manager = TofArTofManager.Instance;

            if (manager != null)
            {
                var calibs = manager.CalibrationSettings;

                this.cx = calibs.c.cx;
                this.cy = calibs.c.cy;


                this.fx = calibs.c.fx;
                this.fy = calibs.c.fy;

                this.cxDepth = calibs.d.cx;
                this.cyDepth = calibs.d.cy;
                this.fxDepth = calibs.d.fx;
                this.fyDepth = calibs.d.fy;
            }
        }

        private void OnTofFrameArrived(object sender)
        {
            lock (this)
            {
                if (this.inputSourceType != InputSource.Confidence)
                {
                    return;
                }

                if (!TofArTofManager.Instance.IsStreamActive || !TofArFaceManager.Instance.IsStreamActive)
                {
                    return;
                }

                if (TofArFaceManager.Instance.DetectorType == FaceDetectorType.Internal_ARKit)
                {
                    return;
                }

                if (faceDetector == null)
                {
                    return;
                }

                if (tofWidth == 0 || tofHeight == 0)
                {
                    return;
                }

                if (this.fxDepth == 0 || this.fyDepth == 0 || this.cxDepth == 0 || this.cyDepth == 0)
                {
                    return;
                }

                DetectFace();
            }
        }

        private void OnColorFrameArrived(object sender)
        {
            lock (this)
            {
                if (this.inputSourceType != InputSource.Color)
                {
                    return;
                }

                if (!TofArColorManager.Instance.IsStreamActive || !TofArFaceManager.Instance.IsStreamActive)
                {
                    return;
                }

                if (TofArFaceManager.Instance.DetectorType == FaceDetectorType.Internal_ARKit)
                {
                    return;
                }

                if (faceLandmarkRuntime == null || faceDetectionRuntime == null || faceDetector == null)
                {
                    return;
                }

                if (colorWidth == 0 || colorHeight == 0)
                {
                    return;
                }

                if (this.fx == 0 || this.fy == 0 || this.cx == 0 || this.cy == 0)
                {
                    return;
                }

                DetectFace();
            }
        }

        private void DetectFace()
        {
            ColorData colorData = TofArColorManager.Instance?.ColorData;
            ConfidenceData confidenceData = TofArTofManager.Instance?.ConfidenceData;

            if (colorData == null && this.inputSourceType == InputSource.Color)
            {
                return;
            }

            if (confidenceData == null && this.inputSourceType == InputSource.Confidence)
            {
                return;
            }

            int sw = this.inputSourceType == InputSource.Color ? colorWidth : tofWidth;
            int sh = this.inputSourceType == InputSource.Color ? colorHeight : tofHeight;

            this.rotation = TofArManager.Instance.GetScreenOrientation();

            // R8 G8 B8 ...
            byte[] colorRawData = colorData?.Data;
            var colorType = colorData?.Type;
            short[] confidenceRawData = confidenceData?.Data;

            Vector2 ay2 = new Vector2(this.axisY.x, this.axisY.y);
            double rad = -Math.Atan2(ay2.x, ay2.y);

            // x -> position x
            float dxx = (float)Math.Cos(rad) * (this.faceSize / HeightLandmark);
            float dxy = (float)Math.Sin(rad) * (this.faceSize / HeightLandmark);
            // y -> position y
            float dyx = (float)Math.Cos(rad + Math.PI / 2) * (this.faceSize / HeightLandmark);
            float dyy = (float)Math.Sin(rad + Math.PI / 2) * (this.faceSize / HeightLandmark);

            int width = sw;
            int height = sh;

            if (this.rotation == 90 || this.rotation == 270)
            {
                width = sh;
                height = sw;
            }

            // calculate face pose
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            bool faceIsDetected = false;

            FaceLogic.DetectFaceInternal(this, ref inputSourceType, ref colorRawData, ref confidenceRawData, ref width, ref height, ref colorType, ref rotation,
                ref dxx, ref dxy, ref dyx, ref dyy, ref faceCenterX, ref faceCenterY, (int)NetworkType.FaceDetection, (int)NetworkType.FaceLandmarks, landmarkProcessMode,
                WidthDetection, HeightDetection, WidthLandmark, HeightLandmark, ref inputDetection, ref inputLandmarks,
                ref faceIsDetected, ref axisX, ref axisY, ref pos, ref rot,
                ref fx, ref fy, ref cx, ref cy, ref fxDepth, ref fyDepth, ref cxDepth, ref cyDepth,
                POINT_COUNT, ref faceSize, ref points, ref globalPositions, ref tmpLocalPositions,
                this.enableNoiseReduction, this.nrTaps, this.nrValue,
                () =>
                {
                    float[][] output0 = faceDetectionRuntime.forward();

                    List<FaceDetectorResult> results = faceDetector.GetResults(output0, 0.7f, 0.3f);

                    if (results.Count >= 1)
                    {
                        FaceDetectorResult result = results[0];
                        FaceRect rect = result.rect;

                        faceCenterX = (rect.x + rect.width * 0.5f) * width;
                        faceCenterY = (rect.y + rect.height * 0.5f) * height;

                        faceSize = (rect.width * width + rect.height * height) / 2 * 1.5f;
                    }
                },
                () => { return faceLandmarkRuntime.forward(); }
            );

            SetResults(faceIsDetected, pos, rot);
        }

        private void SetResults(bool faceIsDetected, Vector3 pos, Quaternion rot)
        {
            lock (this)
            {
                FaceResult faceResult;
                ulong faceTimestamp = (ulong)Utils.GetUnixTimestampAsNanoSeconds();

                if (faceIsDetected)
                {
                    facePosition = pos;
                    faceRotation = rot;

                    for (int i = 0; i < tmpLocalPositions.Length; i++)
                    {
                        localPositions[i] = new TofArVector3(tmpLocalPositions[i].x, tmpLocalPositions[i].y, tmpLocalPositions[i].z);
                    }

                    // set estimated results
                    faceResult = new FaceResult()
                    {
                        vertices = localPositions,
                        indices = FaceGeometry.indices,
                        uvs = FaceGeometry.uvs,
                        trackingState = TrackingState.Tracking,
                        pose = new TofArTransform(facePosition, faceRotation, new TofArVector3(1, 1, 1)),
                        timestamp = faceTimestamp
                    };

                    TofArFacialExpressionEstimator.Instance.GetMappedBlendshapes(ref faceResult);

                }
                else
                {
                    // set estimated results
                    faceResult = new FaceResult()
                    {
                        trackingState = TrackingState.None,
                        timestamp = faceTimestamp
                    };

                }
                TofArFaceManager.Instance.SetEstimatedResult(faceResult);
            }
        }

        private string LoadFileFromResources(string filePath)
        {
            string asset_path = null;
            string local_path = null;

            try
            {
                asset_path = filePath;
                local_path = Application.persistentDataPath + "/" + asset_path;

                FileInfo file_info = new FileInfo(local_path);
                file_info.Directory.Create();

                TextAsset asset = Resources.Load(asset_path) as TextAsset;
                if (asset != null)
                {
                    Stream s = new MemoryStream(asset.bytes);
                    BinaryReader br = new BinaryReader(s);
                    File.WriteAllBytes(local_path, br.ReadBytes(asset.bytes.Length));
                }
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (IOException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (System.Net.WebException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (TimeoutException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }

            return local_path;
        }


    }
}

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
using SensCord;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TofAr.V0.Modeling
{
    /// <summary>
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>Modelingデータの取得</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>録画ファイルの再生</description></item>
    /// </list>
    /// </summary>
    public class TofArModelingManager : Singleton<TofArModelingManager>, IDisposable, IStreamStoppable, IDependManager
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

        /// <summary>
        /// *TODO+ B
        /// ストリームキー
        /// </summary>
        public const string StreamKey = "tofar_modeling_camera2_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にModelingデータのストリームを開始する
        /// </summary>
        public bool autoStart = false;

        [SerializeField]
        private uint updateInterval = 3;
        /// <summary>
        /// Depthデータをモデリング処理に入力する間隔フレーム数(デフォルト値:3)
        /// </summary>
        public uint UpdateInterval
        {
            get
            {
                return this.updateInterval;
            }
            set
            {
                this.updateInterval = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.updateInterval = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private uint estimateInterval = 3;
        /// <summary>
        /// 3DMesh出力を行うUpdate間隔
        /// </summary>
        public uint EstimateInterval
        {
            get
            {
                return this.estimateInterval;
            }
            set
            {
                this.estimateInterval = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.estimateInterval = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private float depthScale = 0.001f;
        /// <summary>
        /// Depthデータのスケール係数
        /// </summary>
        public float DepthScale
        {
            get
            {
                return this.depthScale;
            }
            set
            {
                this.depthScale = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.depthScale = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private float depthFar = 3200f;

        /// <summary>
        /// この設定値以遠のDepthピクセルをモデリング対象外とする(デフォルト値:3200)
        /// </summary>
        [Obsolete]
        public float DepthFar
        {
            get
            {
                return this.depthFar;
            }
            set
            {
                this.depthFar = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.depthFar = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private bool estimateUpdatedSurface = false;
        /// <summary>
        /// <para>true: 3DMesh出力時に変化があった部分のみ更新する</para>
        /// <para>false: 3DMesh出力時に全てのMeshを更新する</para>
        /// <para>(デフォルト値:false)</para>
        /// </summary>
        public bool EstimateUpdatedSurface
        {
            get
            {
                return this.estimateUpdatedSurface;
            }
            set
            {
                this.estimateUpdatedSurface = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.estimateUpdatedSurface = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private bool isProcessConfidence = false;
        /// <summary>
        /// <para>true: Confidenceデータをモデリングに利用する</para>
        /// <para>false: Confidenceデータをモデリングに利用しない</para>
        /// <para>(デフォルト値:false)</para>
        /// </summary>
        public bool IsProcessConfidence
        {
            get
            {
                return this.isProcessConfidence;
            }
            set
            {
                this.isProcessConfidence = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.isProcessConfidence = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private float voxelSize = 0.05f;
        /// <summary>
        /// ボクセルサイズ (デフォルト値:0.05)
        /// </summary>
        public float VoxelSize
        {
            get { return this.voxelSize; }
            set
            {
                this.voxelSize = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.voxelSize = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private bool enableVoxelPruning = false;
        /// <summary>
        /// ボクセル剪定の有無 (デフォルト値:false)
        /// </summary>
        public bool EnableVoxelPruning
        {
            get { return this.enableVoxelPruning; }
            set
            {
                this.enableVoxelPruning = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.enableVoxelPruning = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private uint numMaxBlocks = 1000;
        /// <summary>
        /// 最大ブロック数 (デフォルト値:1000)
        /// </summary>
        public uint NumMaxBlocks
        {
            get { return this.numMaxBlocks; }
            set
            {
                this.numMaxBlocks = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.numMaxBlocks = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }


        [SerializeField]
        private float depthConfidenceThresh = 0.5f;
        /// <summary>
        /// 深度信頼マップの閾値 (デフォルト値:0.5)
        /// </summary>
        public float DepthConfidenceThresh
        {
            get { return this.depthConfidenceThresh; }
            set
            {
                this.depthConfidenceThresh = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.depthConfidenceThresh = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private float minDepthMeasurement = 0.2f;
        /// <summary>
        /// 深度測定の最小距離 (デフォルト値:0.2)
        /// </summary>
        public float MinDepthMeasurement
        {
            get { return this.minDepthMeasurement; }
            set
            {
                this.minDepthMeasurement = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.minDepthMeasurement = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private float maxDepthMeasurement = 5.0f;
        /// <summary>
        /// 深度測定の最大距離 (デフォルト値:5.0)
        /// </summary>
        public float MaxDepthMeasurement
        {
            get
            {
                if (!this.settingsRead)
                {
                    ReadModelingSettingsFromJson();
                }
                return this.maxDepthMeasurement;
            }
            set
            {
                this.maxDepthMeasurement = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.maxDepthMeasurement = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private float weightMax = 30.0f;
        /// <summary>
        /// 最大ウェイト値 (デフォルト値:30.0)
        /// </summary>
        public float WeightMax
        {
            get { return this.weightMax; }
            set
            {
                this.weightMax = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.weightMax = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private bool enableFrustumCulling = true;
        /// <summary>
        /// 視錐台カリングの有無 (デフォルト値:true)
        /// </summary>
        public bool EnableFrustumCulling
        {
            get { return this.enableFrustumCulling; }
            set
            {
                this.enableFrustumCulling = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.enableFrustumCulling = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private bool defineTargetSpace = false;
        /// <summary>
        /// ターゲットスペース定義の有無 (デフォルト値:false)
        /// </summary>
        public bool DefineTargetSpace
        {
            get { return this.defineTargetSpace; }
            set
            {
                this.defineTargetSpace = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.defineTargetSpace = value;
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private Vector3 minTargetSpace = Vector3.zero;
        /// <summary>
        /// ターゲットスペース境界ボックス最小値の[X,Y,Z]
        /// </summary>
        public Vector3 MinTargetSpace
        {
            get { return this.minTargetSpace; }
            set
            {
                this.minTargetSpace = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.minTargetSpace = new float[3] { minTargetSpace.x, minTargetSpace.y, minTargetSpace.z };
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private Vector3 maxTargetSpace = Vector3.zero;
        /// <summary>
        /// ターゲットスペース境界ボックス最大値の[X,Y,Z]
        /// </summary>
        public Vector3 MaxTargetSpace
        {
            get { return this.maxTargetSpace; }
            set
            {
                this.maxTargetSpace = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.maxTargetSpace = new float[3] { maxTargetSpace.x, maxTargetSpace.y, maxTargetSpace.z };
                TofArModelingManager.Instance.SetProperty(modelingSettings);
            }
        }

        [SerializeField]
        private bool isVoxelProjection = true;

        /// <summary>
        /// VoxelProjectionの有効/無効
        /// </summary>
        public bool IsVoxelProjection
        {
            get
            {
                if (!this.settingsRead)
                {
                    ReadModelingSettingsFromJson();
                }
                return this.isVoxelProjection;
            }
            set
            {
                this.isVoxelProjection = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.isVoxelProjection = value;
                TofArModelingManager.Instance.SetProperty<ModelingSettingsProperty>(modelingSettings);
            }
        }

        [SerializeField]
        private bool enableFakeSparseDepthPoints = false;

        /// <summary>
        /// 疑似Sparse深度データの有効/無効
        /// </summary>
        public bool EnableFakeSparseDepth
        {
            get
            {
                return this.enableFakeSparseDepthPoints;
            }
            set
            {
                this.enableFakeSparseDepthPoints = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.enableFakeSparseDepth = value;
                TofArModelingManager.Instance.SetProperty<ModelingSettingsProperty>(modelingSettings);
            }
        }

        [SerializeField]
        private int targetFakeSparseDepthPoints = 1000;

        /// <summary>
        /// 疑似Sparse深度ポイント数
        /// </summary>
        public int TargetFakeSparseDepthPoints
        {
            get
            {
                return this.targetFakeSparseDepthPoints;
            }
            set
            {
                this.targetFakeSparseDepthPoints = value;
                var modelingSettings = TofArModelingManager.Instance.GetProperty<ModelingSettingsProperty>();
                modelingSettings.targetFakeSparseDepthPoints = value;
                TofArModelingManager.Instance.SetProperty<ModelingSettingsProperty>(modelingSettings);
            }
        }

        [SerializeField]
        private bool enableConfidenceCorrection = true;
        /// <summary>
        /// trueの場合はConfidence値がconfidenceCorrectionThresholdより小さいピクセルはDepth値をconfidenceCorrectionInvalidValueとする
        /// <para>デフォルト値:true</para>
        /// </summary>
        public bool EnableConfidenceCorrection
        {
            get
            {
                return this.enableConfidenceCorrection;
            }
            set
            {
                this.enableConfidenceCorrection = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.enableConfidenceCorrection = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private ushort confidenceCorrectionThreshold = 0;
        /// <summary>
        /// ConfidenceCorrection処理の閾値
        /// <para>デフォルト値:0</para>
        /// </summary>
        public ushort ConfidenceCorrectionThreshold
        {
            get
            {
                return this.confidenceCorrectionThreshold;
            }
            set
            {
                this.confidenceCorrectionThreshold = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.confidenceCorrectionThreshold = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        [SerializeField]
        private ushort confidenceCorrectionInvalidValue = 32001;
        /// <summary>
        /// ConfidenceCorrection処理でDepthピクセルに設定する無効値
        /// <para>デフォルト値:32001</para>
        /// </summary>
        public ushort ConfidenceCorrectionInvalidValue
        {
            get
            {
                return this.confidenceCorrectionInvalidValue;
            }
            set
            {
                this.confidenceCorrectionInvalidValue = value;
                var runtimeSettings = TofArModelingManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeSettings.confidenceCorrectionInvalidValue = value;
                TofArModelingManager.Instance.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
            }
        }

        private bool isUnPaused = true;
        private bool streamOpenErrorOccured = false;

        [System.Serializable]
        private class deviceAttributesJson
        {
            public string modelingIsVoxelProjection = "";
            public string modelingDefaultMaxDepthMeasurement = "";
        }

        private Stream stream = null;
        /// <summary>
        /// ストリーム
        /// </summary>
        public Stream Stream
        {
            get
            {
                if (isUnPaused && this.stream == null && !this.streamOpenErrorOccured)
                {
                    var tofARStream = TofArManager.Instance.Stream;
                    if (tofARStream != null)
                    {
                        if (!this.settingsRead)
                        {
                            ReadModelingSettingsFromJson();
                        }
#if UNITY_EDITOR
                        try
                        {
#endif
                        this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArModelingManager.StreamKey);

                        this.Stream?.RegisterFrameCallback(StreamCallBack);
                        TofArTofManager.Instance?.AddManagerDependency(this);

                        var runtimeSettings = this.stream?.GetProperty<RuntimeSettingsProperty>();
                        if (runtimeSettings != null)
                        {
                            runtimeSettings.depthScale = this.depthScale;
                            runtimeSettings.estimateInterval = this.estimateInterval;
                            runtimeSettings.estimateUpdatedSurface = this.estimateUpdatedSurface;
                            runtimeSettings.isProcessConfidence = this.isProcessConfidence;
                            runtimeSettings.updateInterval = this.updateInterval;
                            runtimeSettings.enableConfidenceCorrection = this.enableConfidenceCorrection;
                            runtimeSettings.confidenceCorrectionThreshold = this.confidenceCorrectionThreshold;
                            runtimeSettings.confidenceCorrectionInvalidValue = this.confidenceCorrectionInvalidValue;
                            this.stream?.SetProperty(runtimeSettings);
                        }
                        

                        // initialize
                        var modelingSettings = new ModelingSettingsProperty()
                        {
                            defineTargetSpace = this.defineTargetSpace,
                            depthConfidenceThresh = this.depthConfidenceThresh,
                            enableFrustumCulling = this.enableFrustumCulling,
                            enableVoxelPruning = this.enableVoxelPruning,
                            isVoxelProjection = this.isVoxelProjection,
                            maxDepthMeasurement = this.maxDepthMeasurement,
                            maxTargetSpace = new float[3] { this.maxTargetSpace.x, this.maxTargetSpace.y, this.maxTargetSpace.z },
                            minTargetSpace = new float[3] { this.minTargetSpace.x, this.minTargetSpace.y, this.minTargetSpace.z },
                            minDepthMeasurement = this.minDepthMeasurement,
                            numMaxBlocks = this.numMaxBlocks,
                            voxelSize = this.voxelSize,
                            weightMax = this.weightMax,
                            enableFakeSparseDepth = this.enableFakeSparseDepthPoints,
                            targetFakeSparseDepthPoints = this.targetFakeSparseDepthPoints
                        };
                        this.stream?.SetProperty(modelingSettings);
#if UNITY_EDITOR
                        }
                        catch (ApiException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                            this.streamOpenErrorOccured = true;
                        }
#endif
                    }
                    else
                    {
                        this.streamOpenErrorOccured = true;
                    }
                }
                return this.stream;
            }
        }


        /// <summary>
        /// trueの場合ストリーミングを行っている
        /// </summary>
        public bool IsStreamActive
        {
            get
            {
                return this.stream?.IsStarted ?? false;
            }
        }

        private bool IsStreamPausing { get; set; }

        /// <summary>
        /// 最新のModelingデータ
        /// </summary>
        public ModelingData ModelingData { get; private set; }

        /// <summary>
        /// ストリーミング開始時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        public delegate void StreamStartedEventHandler(object sender);
        /// <summary>
        /// ストリーミング開始通知
        /// </summary>
        public static event StreamStartedEventHandler OnStreamStarted;

        /// <summary>
        /// *TODO+ B
        /// ストリーミング開始通知
        /// </summary>
        [Obsolete("Using the static OnStreamStarted is recommended, which is more stable than StreamStarted")]
        private event StreamStartedEventHandler StreamStarted
        {
            add { OnStreamStarted += value; }
            remove { OnStreamStarted -= value; }
        }

        /// <summary>
        /// ストリーミング終了時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        public delegate void StreamStoppedEventHandler(object sender);
        /// <summary>
        /// ストリーミング終了通知
        /// </summary>
        public static event StreamStoppedEventHandler OnStreamStopped;

        /// <summary>
        /// *TODO+ B
        /// ストリーミング終了通知
        /// </summary>
        [Obsolete("Using the static OnStreamStopped is recommended, which is more stable than StreamStopped")]
        private event StreamStoppedEventHandler StreamStopped
        {
            add { OnStreamStopped += value; }
            remove { OnStreamStopped -= value; }
        }

        /// <summary>
        /// 新規フレーム到着時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        public delegate void FrameArrivedEventHandler(object sender);
        /// <summary>
        /// 新しいフレームの到着通知
        /// </summary>
        public static event FrameArrivedEventHandler OnFrameArrived;

        /// <summary>
        /// *TODO+ B
        /// 新しいフレームの到着通知
        /// </summary>
        [Obsolete("Using the static OnFrameArrived is recommended, which is more stable than FrameArrived")]
        private event FrameArrivedEventHandler FrameArrived
        {
            add { OnFrameArrived += value; }
            remove { OnFrameArrived -= value; }
        }

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

        /// <summary>
        /// 実測FPS
        /// </summary>
        public float FrameRate { get; private set; }
        private int frameCount = 0;
        private float fromFpsMeasured = 0f;
        private float cameraRotZ = 0f;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool CameraPoseCallbackDelegate(int cameraIndex);
        static private CameraPoseCallbackDelegate cameraPoseCallbackDelegate;

        private IntPtr cameraPoseCallback = IntPtr.Zero;
        private Vector3 T = Vector3.zero;
        private Matrix4x4 cRT = new Matrix4x4();

        private float cameraPoseSended = 0f;

        private static bool isStopping = false;
        private static object modelingLock = new object();

        private bool settingsRead = false;

        /// <summary>
        /// *TODO+ B
        /// カメラポーズを更新された時にModeling処理で適用する
        /// </summary>
        /// <param name="cameraIndex">TODO+ C</param>
        /// <returns>TODO+ C</returns>
        [AOT.MonoPInvokeCallback(typeof(CameraPoseCallbackDelegate))]
        private static bool CameraPoseCallback(int cameraIndex)
        {
            lock (modelingLock)
            {
                if (isStopping)
                {
                    return false;
                }
                //        TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArModeling] CameraPoseCallback : cameraIndex={cameraIndex}");
                TofArModelingManager.Instance?.UpdateModelingCameraPose();

                return true;
            }

        }

        private void UpdateModelingCameraPose()
        {

            var currentCameraPose = TofArManager.Instance.GetProperty<CameraPoseProperty>();
            //TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArModeling] currentCameraPose.position.x={currentCameraPose.position.x} currentCameraPose.rotation.x={currentCameraPose.rotation.x}");
            var modelingCameraPose = this.CreateCameraPose(currentCameraPose);
            //TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArModeling] CameraPoseCallback() InternalCameraPoseProperty -> (" +
            //    $"{modelingCameraPose[0]}, {modelingCameraPose[1]}, {modelingCameraPose[2]}, " +
            //    $"{modelingCameraPose[3]}, {modelingCameraPose[4]}, {modelingCameraPose[5]}, " +
            //    $"{modelingCameraPose[6]}, {modelingCameraPose[7]}, {modelingCameraPose[8]}, " +
            //    $"{modelingCameraPose[9]}, {modelingCameraPose[10]}, {modelingCameraPose[11]})");
            this.SetProperty(new InternalCameraPoseProperty() { pose = modelingCameraPose });
        }

        private void ReadModelingSettingsFromJson()
        {
            var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
            var deviceAttributes = JsonUtility.FromJson<deviceAttributesJson>(devcap.TrimmedDeviceAttributesString);

            if (bool.TryParse(deviceAttributes?.modelingIsVoxelProjection, out bool isProjection))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Use projection: {isProjection}");
                this.isVoxelProjection = isProjection;
            }

            if (float.TryParse(deviceAttributes?.modelingDefaultMaxDepthMeasurement, out float maxDepth))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Max depth measurement: {maxDepth}");
                this.maxDepthMeasurement = maxDepth;
            }

            this.settingsRead = true;
        }

        private void Start()
        {
            TofArManager.Instance.AddSubManager(this);

            if (this.autoStart)
            {
                this.StartCoroutine(this.StartProcess());
            }
        }

        private void OnEnable()
        {
            TofArManager.OnDeviceOrientationUpdated += OnDeviceOrientationChanged;

            OnDeviceOrientationChanged(DeviceOrientation.LandscapeLeft, TofArManager.Instance.GetProperty<DeviceOrientationsProperty>().deviceOrientation);
        }

        private void OnDisable()
        {
            TofArManager.OnDeviceOrientationUpdated -= OnDeviceOrientationChanged;
        }

        private void OnDeviceOrientationChanged(DeviceOrientation previousDeviceOrientation, DeviceOrientation newDeviceOrientation)
        {
            cameraRotZ = 0f;
            DeviceOrientation devOr = newDeviceOrientation;
            switch (devOr)
            {
                case DeviceOrientation.PortraitUpsideDown:
                    cameraRotZ = 180f; break;
                case DeviceOrientation.LandscapeLeft:
                    cameraRotZ = -90f; break;
                case DeviceOrientation.LandscapeRight:
                    cameraRotZ = 90f; break;
            }
        }

        private IEnumerator StartProcess()
        {
            var externalTofSource = Utils.FindFirstGameObjectThatImplements<IExternalStreamSource>(true);
            //wait for the ToF to start
            if (Tof.TofArTofManager.Instance != null && (Tof.TofArTofManager.Instance.autoStart || externalTofSource != null))
            {
                while (!TofAr.V0.Tof.TofArTofManager.Instance.IsStreamActive)
                {
                    yield return null;
                }
            }
            Instance.StartStream();
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArModelingManager.Dispose()");
            TofArTofManager.Instance?.RemoveManagerDependency(this);
            if (this.stream != null)
            {
                if (this.stream.IsStarted)
                {
                    this.StopStream();
                }
                this.Stream?.Dispose();
                this.stream = null;
            }

            if (this.streamPlay != null)
            {
                if (this.streamPlay.IsStarted)
                {
                    this.streamPlay.Stop();
                }
                this.streamPlay.Dispose();
                this.streamPlay = null;
            }
        }

        private void Update()
        {
            this.fromFpsMeasured += Time.unscaledDeltaTime;
            if (this.fromFpsMeasured >= 1.0f)
            {
                this.FrameRate = this.frameCount / this.fromFpsMeasured;
                this.fromFpsMeasured = 0;
                this.frameCount = 0;
            }

            // Updating camera pose for modeling in the MuitiNode mode.
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.MultiNode && this.IsStreamActive)
            {
                //TofArManager.Instance.ForceUpdateCameraPose();
                this.cameraPoseSended += Time.unscaledDeltaTime;
                if (this.cameraPoseSended >= 0.1f)
                {
                    this.UpdateModelingCameraPose();
                    this.cameraPoseSended = 0;
                }
            }
        }

        internal void StartStream(CameraSettingsProperty cameraSettings)
        {
            if (!this.IsStreamActive && this.isUnPaused)
            {
                if (this.streamPlay != null && this.streamPlay.IsStarted)
                {
                    this.StopPlayback();
                }
                if (cameraSettings != null)
                {
                    this.SetupCameraPoseCallback();
                    cameraSettings.cameraPoseCallbackAddress = (ulong)this.cameraPoseCallback.ToInt64();
                    this.SetProperty(cameraSettings);
                }

                { // Flip coordinates
                    var runtimeSettings = this.GetProperty<RuntimeSettingsProperty>();
                    runtimeSettings.transformation[5] = 0;
                    runtimeSettings.transformation[6] = 1;
                    runtimeSettings.transformation[9] = 1;
                    runtimeSettings.transformation[10] = 0;
                    this.SetProperty<RuntimeSettingsProperty>(runtimeSettings);
                }

#if !UNITY_EDITOR
                TofArManager.Instance.autoTrackCameraPose = true;
#endif
                TofArManager.Instance.ForceUpdateCameraPose();
                this.SetProperty(new InternalCameraPoseProperty() { pose = this.CreateCameraPose(TofArManager.Instance.GetProperty<CameraPoseProperty>()) });

                try
                {
                    this.Stream?.Start();

                    if (this.startingPlayback)
                    {
                        this.IsPlaying = true;
                    }

                    OnStreamStarted?.Invoke(this);
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to start modeling stream. \n" + e.Message);
                    this.Stream?.Dispose();
                    this.stream = null;
                    return;
                }

            }
        }

        internal void StartStream(CameraSettingProperty cameraSetting)
        {
            CameraSettingsProperty cameraSettings = null;
            if (cameraSetting != null)
            {
                cameraSettings = new CameraSettingsProperty() { settings = new CameraSettingProperty[] { cameraSetting } };
            }
            this.StartStream(cameraSettings);
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        public void StartStream()
        {
            if (Tof.TofArTofManager.Instance.ProcessTargets?.processDepth == false)
            {
                TofArManager.Logger.WriteLog(LogLevel.Info, "Process depth is disabled. Model won't be processed.");
            }
            else if (TofArTofManager.Instance.ProcessTargets?.processConfidence == false)
            {
                TofArManager.Logger.WriteLog(LogLevel.Info, "Process confidence is disabled. Model might not be processed.");
            }

            var cameraSetting = this.CreateCameraSettings(TofArTofManager.Instance.CalibrationSettings);
            this.StartStream(cameraSetting);
        }

        /// <summary>
        /// 依存するManagerから要求されたストリーミング再スタートを開始する
        /// </summary>
        /// <param name="requestSource">要求元</param>
        public void RestartStreamByDependManager(object requestSource)
        {
            if (requestSource is IDependedManager)
            {
                this.StopStream(requestSource);
            }
        }

        /// <summary>
        /// 依存するManagerから要求されたストリーミング再スタート後処理
        /// </summary>
        /// <param name="requestSource">要求元</param>
        public void FinalizeRestartStreamByDependManager(object requestSource)
        {
            if (requestSource is IDependedManager)
            {
                this.StartStream();
            }
        }

        /// <summary>
        /// ストリーミングを停止する
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        public void StopStream(object sender = null)
        {
            if (this.IsStreamActive)
            {
                lock (modelingLock)
                {
                    isStopping = true;
                }
                try
                {
                    this.Stream?.Stop();
                    OnStreamStopped?.Invoke((sender == null) ? this : sender);
                }
                finally
                {
                    isStopping = false;
                }
            }
            this.streamOpenErrorOccured = false;
        }

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

            if (pause && this.IsStreamActive)
            {
                this.StopStream();
                this.IsStreamPausing = true;
            }
            else if (!pause && this.IsStreamPausing)
            {
                this.StartStream();
                this.IsStreamPausing = false;
            }
        }

        static private void StreamCallBack(Stream stream)
        {
            try
            {
                lock (Instance.stopLock)
                {
                    if (!TofArModelingManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    var frame = stream.GetFrame();

                    try
                    {
                        if (frame.Channels.Length > 0)
                        {
                            var modelingChannel = frame.Channels[(int)ChannelIds.Modeling];
                            var rawData = modelingChannel.GetRawData();

                            instance.ModelingData = new ModelingData(rawData);
                            instance.frameCount++;

                        }
                    }
                    finally
                    {
                        frame.Dispose();
                    }
                    OnFrameArrived?.Invoke(instance);
                }
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            
        }

        /// <summary>
        /// コンポーネントプロパティを取得する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>() where T : class, IBaseProperty, new()
        {
            if (this.isUnPaused || this.IsPlaying)
            {

                T property = new T();
                int bufferSize = 1024;
                //handle types that need a larger buffer
                if (property is CameraSettingsProperty)
                {
                    bufferSize = 2048;
                }
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(property.Key, ref property, bufferSize);
                return property;
            }
            return null;
        }

        /// <summary>
        /// シリアライズ用バッファサイズを指定してコンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <param name="bufferSize">シリアライズ用バッファサイズ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value, int bufferSize) where T : class, IBaseProperty, new()
        {
            if (this.isUnPaused || this.IsPlaying)
            {
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(value.Key, ref value, bufferSize);
                return value;
            }
            return null;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (this.isUnPaused || this.IsPlaying)
            {
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(ref value);
                return value;
            }
            return null;
        }

        /// <summary>
        /// コンポーネントプロパティを設定する
        /// </summary>
        /// <typeparam name="T"> A</typeparam>
        /// <param name="value">入力パラメータ</param>
        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (this.isUnPaused)
            {
                if (value is SensCord.FrameRateProperty) // Don't set framerate property from outside to avoid accidental changes
                {
                    return; // Throw exception? 
                }
                else if (value is RecordProperty)
                {
                    if ((value as RecordProperty).Enabled)
                    {
                        this.Stream?.SetProperty(new SensCord.FrameRateProperty() { Num = 30 });
                    }
                    else
                    {
                        this.Stream?.SetProperty(new SensCord.FrameRateProperty() { Num = 1 });
                    }
                }
                else if (value is CameraSettingsProperty)
                {
                    if ((value as CameraSettingsProperty).settings == null)
                    {
                        (value as CameraSettingsProperty).settings = new CameraSettingProperty[0];
                    }
                }
                this.Stream?.SetProperty<T>(value);

                if (value is ModelingSettingsProperty)
                {
                    var modelingSettingsProp = value as ModelingSettingsProperty;
                    this.defineTargetSpace = modelingSettingsProp.defineTargetSpace;
                    this.depthConfidenceThresh = modelingSettingsProp.depthConfidenceThresh;
                    this.enableFakeSparseDepthPoints = modelingSettingsProp.enableFakeSparseDepth;
                    this.enableFrustumCulling = modelingSettingsProp.enableFrustumCulling;
                    this.enableVoxelPruning = modelingSettingsProp.enableVoxelPruning;
                    this.isVoxelProjection = modelingSettingsProp.isVoxelProjection;
                    this.maxDepthMeasurement = modelingSettingsProp.maxDepthMeasurement;
                    this.maxTargetSpace = new Vector3(modelingSettingsProp.maxTargetSpace[0], modelingSettingsProp.maxTargetSpace[1], modelingSettingsProp.maxTargetSpace[2]);
                    this.minDepthMeasurement = modelingSettingsProp.minDepthMeasurement;
                    this.minTargetSpace = new Vector3(modelingSettingsProp.minTargetSpace[0], modelingSettingsProp.minTargetSpace[1], modelingSettingsProp.minTargetSpace[2]);
                    this.numMaxBlocks = modelingSettingsProp.numMaxBlocks;
                    this.targetFakeSparseDepthPoints = modelingSettingsProp.targetFakeSparseDepthPoints;
                    this.voxelSize = modelingSettingsProp.voxelSize;
                    this.weightMax = modelingSettingsProp.weightMax;
                }

            }
        }

        /// <summary>
        /// Propertyリストを取得する
        /// </summary>
        /// <returns>Propertyリスト</returns>
        public string[] GetPropertyList()
        {
            if (this.isUnPaused)
            {
                return this.Stream?.GetPropertyList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// *TODO+ B（使われるのでpublicのままにする）
        /// ストリームを停止する
        /// </summary>
        public void PauseStream()
        {
            if (!this.isUnPaused)
            {
                return;
            }
            this.isUnPaused = false;
            this.IsStreamPausing = this.IsStreamActive;
            if (this.stream != null)
            {
                if (this.IsStreamActive)
                {
                    this.StopStream();
                }
                this.Stream?.Dispose();
                this.stream = null;
            }
        }

        /// <summary>
        /// *TODO+ B（使われるのでpublicのままにする）
        /// ストリームを再開する
        /// </summary>
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
                this.StartStream();
            }
        }

        private void SetupCameraPoseCallback()
        {
            cameraPoseCallbackDelegate = new CameraPoseCallbackDelegate(CameraPoseCallback);
            this.cameraPoseCallback = Marshal.GetFunctionPointerForDelegate(cameraPoseCallbackDelegate);
            //        TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArModeling] Set TofArModelingCameraSettingsProperty this.cameraPoseCallback={this.cameraPoseCallback} 32:{this.cameraPoseCallback.ToInt32()} 64:{this.cameraPoseCallback.ToInt64()}");
        }

        internal CameraSettingProperty CreateCameraSettings(CalibrationSettingsProperty sp)
        {
            //           TofArManager.Logger.WriteLog(LogLevel.Debug, "Create camera settings called");
            var cameraSettings = new CameraSettingProperty();

            cameraSettings.width = sp.depthWidth;
            cameraSettings.height = sp.depthHeight;
            cameraSettings.stride = sp.depthWidth;

            cameraSettings.intrinsics[0] = sp.d.fx;
            cameraSettings.intrinsics[1] = 0.0f;
            cameraSettings.intrinsics[2] = sp.d.cx;
            cameraSettings.intrinsics[3] = 0.0f;
            cameraSettings.intrinsics[4] = sp.d.fy;
            cameraSettings.intrinsics[5] = sp.d.cy;

            this.CacheExternalParameter(sp);

            return cameraSettings;
        }

        internal void CacheExternalParameter(CalibrationSettingsProperty sp)
        {
            this.cRT[0, 0] = sp.R.a; this.cRT[0, 1] = sp.R.d; this.cRT[0, 2] = sp.R.g;
            this.cRT[1, 0] = sp.R.b; this.cRT[1, 1] = sp.R.e; this.cRT[1, 2] = sp.R.h;
            this.cRT[2, 0] = sp.R.c; this.cRT[2, 1] = sp.R.f; this.cRT[2, 2] = sp.R.i;

            this.T.x = sp.T.x * -0.001f;
            this.T.y = sp.T.y * -0.001f;
            this.T.z = sp.T.z * -0.001f;
        }

        internal float[] CreateCameraPose(CameraPoseProperty pose)
        {
            var result = new float[12];


            var quatZ = Quaternion.Euler(0, 0, cameraRotZ);

            var quatY = Quaternion.Euler(0, 90, 0);
            var quatX = Quaternion.Euler(-90, 0, 0);
            var quatRot2Smart = quatY * quatX * quatZ;

            var quatA = new Quaternion();
            quatA.w = pose.Rotation.w;
            quatA.x = -pose.Rotation.x;
            quatA.y = -pose.Rotation.z;
            quatA.z = -pose.Rotation.y;

            var wPc_r = new Vector3(pose.Position.x, pose.Position.z, pose.Position.y);

            var quatS = quatA * quatRot2Smart;
            var wRc_s = Matrix4x4.Rotate(quatS);

            var wRc_t = wRc_s * this.cRT;
            var wPc_t = (Vector3)(wRc_t * this.T) + wPc_r;

            // wRc, wPcの設定
            result[0] = wPc_t.x;
            result[1] = wPc_t.y;
            result[2] = wPc_t.z;

            result[3] = wRc_t[0, 0];
            result[4] = wRc_t[0, 1];
            result[5] = wRc_t[0, 2];
            result[6] = wRc_t[1, 0];
            result[7] = wRc_t[1, 1];
            result[8] = wRc_t[1, 2];
            result[9] = wRc_t[2, 0];
            result[10] = wRc_t[2, 1];
            result[11] = wRc_t[2, 2];

            return result;
        }


        #region PLAYBACK

        /// <summary>
        /// *TODO+ B
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_modeling_stream";

        private Stream streamPlay = null;
        /// <summary>
        /// TODO+ C
        /// </summary>
        public Stream StreamPlay
        {
            get
            {

                if (this.streamPlay == null)
                {
#if UNITY_EDITOR
                    try
                    {
#endif
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArModelingManager.streamPlayerKey);

                    this.streamPlay?.RegisterFrameCallback(StreamCallBack);
#if UNITY_EDITOR
                    }
                    catch (ApiException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                    }
#endif
                }


                return this.streamPlay;

            }
        }

        /// <summary>
        /// trueの場合、録画ファイルを再生している
        /// </summary>
        public bool IsPlaying { private set; get; } = false;
        private bool isPlayingWithTof = false;
        private bool startingPlayback = false;

        /// <summary>
        /// 録画ファイル再生中のToFストリームをソースとして再生を開始する
        /// </summary>
        public void StartPlayback()
        {
            if (Instance.IsPlaying)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Already in playback mode. Needs to call StopPlayback() first.");
                return;
            }
            if (Instance.IsStreamActive)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Stream already running or in playback mode. Needs to call StopPlayback() first.");
                return;
            }
            Instance.Stream.SetProperty(new SensCord.FrameRateProperty() { Num = 15 }); // tells component to use tof playback stream
            Instance.startingPlayback = true;
            try
            {
                Instance.StartStream();
                Instance.isPlayingWithTof = true;
            }
            finally
            {
                Instance.startingPlayback = false;
            }
        }

        /// <summary>
        /// 指定されたパス内の録画ファイルの再生を開始する(既に再生中の場合は呼び出し不可)
        /// </summary>
        /// <param name="path">再生する録画ファイルを含むディレクトリのパス</param>
        public void StartPlayback(string path)
        {
            if (Instance.IsPlaying)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Already in playback mode. Needs to call StopPlayback() first.");
                return;
            }
            if (Instance.IsStreamActive)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Stream currently running. Need to stop first.");
                return;
            }

            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default && !Utils.DirectoryContainsNoSymlinks(path))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"path {path} contained symlink");
                return;
            }

            int tryCount = 3;

            var play = new PlayProperty();
            play.TargetPath = path;
            play.StartOffset = 0;
            play.Count = 0;
            play.Speed = PlaySpeed.BasedOnFramerate;
            play.Mode.Repeat = true;
            for (int t = 0; t < tryCount; t++)
            {
                var stream = Instance.StreamPlay;

                stream.SetProperty<PlayProperty>(play);

                try
                {
                    stream.GetProperty<ModelingSettingsProperty>();
                }
                catch (ApiException) // Fails for first time when using DebugServer
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Start playback failed. Trying again ");
                    // dispose stream and try again
                    Instance.streamPlay.Dispose();
                    Instance.streamPlay = null;

                    continue;
                }

                stream.Start();

                Instance.IsPlaying = true;

                if (OnStreamStarted != null)
                {
                    OnStreamStarted(this);
                }
                break;
            }



        }

        /// <summary>
        /// 録画ファイルの再生を停止する
        /// </summary>
        public void StopPlayback()
        {
            if (Instance.streamPlay != null && Instance.streamPlay.IsStarted)
            {
                Instance.streamPlay.Stop();

                if (OnStreamStopped != null)
                {
                    OnStreamStopped(this);
                }
            }
            else if (Instance.IsStreamActive && Instance.isPlayingWithTof)
            {
                Instance.isPlayingWithTof = false;
                Instance.StopStream();
            }
            else
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Currently not in playback mode.");
            }

            Instance.IsPlaying = false;
        }

        #endregion
    }
}

/*
 * Copyright 2018,2019,2020,2021,2022,2023 Sony Semiconductor Solutions Corporation.
 *
 * This is UNPUBLISHED PROPRIETARY SOURCE CODE of Sony Semiconductor
 * Solutions Corporation.
 * No part of this file may be copied, modified, sold, and distributed in any
 * form or by any means without prior explicit permission in writing from
 * Sony Semiconductor Solutions Corporation.
 *
 */
using SensCord;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TofAr.V0.Color;
using System.Threading;

namespace TofAr.V0.Tof
{
    /// <summary>
    /// *TODO+ B
    /// 特別なDepth設定UID
    /// </summary>
    internal enum DepthConfigSpecialUIDs : int
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        RecordingPlayback = -9999,
        /// <summary>
        /// TODO+ C
        /// </summary>
        LoopedRecordingPlayback = -9998,
        /// <summary>
        /// TODO+ C
        /// </summary>
        ExternalFunction = -9997
    }

    /// <summary>
    /// ToFカメラとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>ToFカメラとの接続管理</description></item>
    /// <item><description>Depthデータの取得</description></item>
    /// <item><description>Confidenceデータの取得</description></item>
    /// <item><description>PointCloudデータの取得</description></item>
    /// <item><description>DepthデータのTexture2D変換</description></item>
    /// <item><description>ConfidenceデータのTextture2D変換</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>ストリーム異常停止検出通知</description></item>
    /// <item><description>ToFカメラ搭載有無の確認</description></item>
    /// <item><description>カメラキャリブレーション情報の管理</description></item>
    /// <item><description>録画ファイルの再生</description></item>
    /// </list>
    /// </summary>
    public class TofArTofManager : Singleton<TofArTofManager>, IDisposable, IStreamStoppable, IDependedManager
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
        /// *TODO+ B（使われるのでpublicのままにする）
        /// ストリームキー
        /// </summary>
        public const string StreamKey = "tofar_tof_camera2_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にToFデータのストリームを開始する
        /// </summary>
        public bool autoStart = false;

        /// <summary>
        /// <para>true: TofとColor同時ストリームの場合Colorカメラを自動変更する</para>
        /// <para>false: Colorカメラを自動変更しない</para>
        /// <para>デフォルト値: true</para>
        /// </summary>
        [HideInInspector]
        public bool autoSwitchColorCameraId = true;

        /// <summary>
        /// 処理対象のチャネルを取得する
        /// </summary>
        public ProcessTargetsProperty ProcessTargets { get; private set; }


        [SerializeField]
        [Tooltip("isProcessDepth is deprecated, please use processDepth instead")]
        [Obsolete("isProcessDepth is deprecated, please use processDepth instead")]
        internal bool isProcessDepth = true;
        [SerializeField]
        internal bool processDepth = true;
        [SerializeField]
        [Tooltip("isProcessConfidence is deprecated, please use processConfidence instead")]
        [Obsolete("isProcessConfidence is deprecated, please use processConfidence instead")]
        internal bool isProcessConfidence = true;
        [SerializeField]
        internal bool processConfidence = true;
        [SerializeField]
        [Tooltip("isProcessPointCloud is deprecated, please use processPointCloud instead")]
        [Obsolete("isProcessPointCloud is deprecated, please use processPointCloud instead")]
        internal bool isProcessPointCloud = true;
        [SerializeField]
        internal bool processPointCloud = true;
        [SerializeField]
        [Tooltip("isProcessTexture is deprecated, please use processTexture instead")]
        [Obsolete("isProcessTexture is deprecated, please use processTexture instead")]
        private bool isProcessTexture;
        [SerializeField]
        private bool processTexture = true;

        /// <summary>
        /// <para>true: Start時にAutoCalibアプリで計測したカメラ内部パラメータを自動ロードする</para>
        /// <para>false: Start時にカメラ内部パラメータをロードしない</para>
        /// <para>デフォルト値:true</para>
        /// </summary>
        public bool autoloadCalibration = true;

        private SynchronizationContext context;
        private byte[] depthBuffer = new byte[0];
        private byte[] confBuffer = new byte[0];
        private bool isUnPaused = true;
        private bool streamOpenErrorOccured = false;


        private const int tofStartTries = 10;
        private const float unexpectedStop_fusion = 5.0f;

        [HideInInspector]
        [SerializeField]
        private bool useDefaultStreamDelay = true;

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
                        var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
                        if (devcap != null && !String.IsNullOrEmpty(devcap.TrimmedDeviceAttributesString))
                        {
                            var devAttrib = JsonUtility.FromJson<deviceAttributesJson>(devcap.TrimmedDeviceAttributesString);

                            timeToDetectStreamUnexpectedStop = devAttrib.tofMgrTimeToDetectStreamUnexpectedStop;
                        }

                        try
                        {
                            //make sure the TofArManager stream itself is opened first
                            this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArTofManager.StreamKey);
                            this.ProcessTargets = new ProcessTargetsProperty() { processDepth = this.processDepth, processConfidence = this.processConfidence, processPointCloud = this.processPointCloud };

                            this.stream?.SetProperty<ProcessTargetsProperty>(this.ProcessTargets);
                            this.stream?.RegisterFrameCallback(this.StreamCallBack);

                            if (useFrontCameraAsDefault)
                            {
                                SetFrontCameraAsDefault();
                            }
                        }
                        catch (ApiException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                            this.streamOpenErrorOccured = true;
                        }
                    }
                    else
                    {
                        this.streamOpenErrorOccured = true;
                    }

                }
                return this.stream;

            }
        }

        private void SetFrontCameraAsDefault()
        {
            var defaultConfig = this.stream?.GetProperty<Camera2DefaultConfigurationProperty>();

            if (defaultConfig != null && defaultConfig.lensFacing != (int)LensFacing.Front)
            {
                var defaultFrontConfig = GetDefaultFrontCameraConfig();
                if (defaultFrontConfig != null)
                {
                    var newConfig = new Camera2SetConfigurationIdProperty()
                    {
                        uid = defaultFrontConfig.uid
                    };

                    this.stream?.SetProperty(newConfig);
                    this.lensFacingChanged = true;
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        [Range(0, 10)]
        private int streamDelay = 0;
        /// <summary>
        /// フレームの送出を指定フレーム数遅延させる
        /// </summary>
        public int StreamDelay
        {
            get => streamDelay;
            set
            {
                streamDelay = value;
                SetProperty(new DelayProperty { numFramesDelay = value });
                V0.TofArManager.Logger.WriteLog(V0.LogLevel.Debug, $"Tof delay set to {value}");
            }
        }

        /// <summary>
        /// ストリーム異常停止とみなすまでの停止時間(秒)
        /// <para>デフォルト値:1.5</para>
        /// </summary>
        public float timeToDetectStreamUnexpectedStop = 1.5f;

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

        private bool isStreamPausing = false;

        /// <summary>
        /// 最新のDepthデータ
        /// </summary>
        public DepthData DepthData { get; private set; }
        /// <summary>
        /// 最新のConfidenceデータ
        /// </summary>
        public ConfidenceData ConfidenceData { get; private set; }
        /// <summary>
        /// 最新のPointCloudデータ
        /// </summary>
        public PointCloudData PointCloudData { get; private set; }

        /// <summary>
        /// trueの場合データのTexture2D変換処理を行っている
        /// </summary>
        [Obsolete("IsProcessTexture is deprecated, please use ProcessTexture instead")]
        public bool IsProcessTexture { get => processTexture; private set => processTexture = value; }
        /// <summary>
        /// trueの場合データのTexture2D変換処理を行っている
        /// </summary>
        public bool ProcessTexture { get => processTexture; private set => processTexture = value; }
        /// <summary>
        /// Depthデータを変換したTexture2D
        /// </summary>
        public Texture2D DepthTexture { get; private set; }
        /// <summary>
        /// Confidenceデータを変換したTexture2D
        /// </summary>
        public Texture2D ConfidenceTexture { get; private set; }

        /// <summary>
        /// ストリーミング開始時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="depthTexture">Depthテクスチャー</param>
        /// <param name="confidenceTexture">Confidenceテクスチャー</param>
        /// <param name="pointCloudData">PointCloudデータ</param>
        public delegate void StreamStartedEventHandler(object sender, Texture2D depthTexture, Texture2D confidenceTexture, PointCloudData pointCloudData);
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
        /// ストリーム異常停止検出時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        public delegate void DetectStreamUnexpectedStopEventHandler(object sender);
        /// <summary>
        /// ストリーム異常停止検出通知
        /// </summary>
        public static event DetectStreamUnexpectedStopEventHandler OnDetectStreamUnexpectedStop;

        /// <summary>
        /// *TODO+ B
        /// ストリーム異常停止検出通知
        /// </summary>
        [Obsolete("Using the static OnDetectStreamUnexpectedStop is reccomended, which is more stable than DetectStreamUnexpectedStop")]
        private event DetectStreamUnexpectedStopEventHandler DetectStreamUnexpectedStop
        {
            add { OnDetectStreamUnexpectedStop += value; }
            remove { OnDetectStreamUnexpectedStop -= value; }
        }

        private IList<IDependManager> dependedManagers = null;

        private List<IDependManager> autoStoppedDependManagers = new List<IDependManager>();

        /// <summary>
        /// 依存Managerを追加する
        /// </summary>
        /// <param name="dependManager">依存Manager</param>
        public void AddManagerDependency(IDependManager dependManager)
        {
            // initialize stream first if possible
            if (this.Stream == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Tof stream couldn't be opened. This might lead to unwanted results.");
            }
            if (this.dependedManagers == null)
            {
                this.dependedManagers = new List<IDependManager>();
            }
            this.dependedManagers.Add(dependManager);
        }

        /// <summary>
        /// 依存Managerを削除する
        /// </summary>
        /// <param name="dependManager">依存Manager</param>
        public void RemoveManagerDependency(IDependManager dependManager)
        {
            this.dependedManagers?.Remove(dependManager);
        }

        /// <summary>
        /// 設定リスト変更時デリゲート
        /// </summary>
        /// <param name="configs">新しい設定リスト</param>
        public delegate void AvailableConfigurationsChanged(CameraConfigurationsProperty configs);
        /// <summary>
        /// 設定リスト変更通知
        /// </summary>
        public static event AvailableConfigurationsChanged OnAvailableConfigurationsChanged;

        /// <summary>
        /// 実測FPS
        /// </summary>
        public float FrameRate { get; private set; }
        private int frameCount = 0;
        private float fromFpsMeasured = 0f;
        private float lastTimeFrameRecieved;
        private float lastUpdateTime;

        [SerializeField]
        private float desiredFrameRate = 30f;

        /// <summary>
        /// 設定希望FPS。設定は次回ストリーミング開始時に有効となる。
        /// </summary>
        public float DesiredFrameRate
        {
            get
            {
                return desiredFrameRate;
            }
        }

        [SerializeField]
        private bool useFrontCameraAsDefault = false;

        /// <summary>
        /// ToFカメラとColorカメラの同期設定を指定する
        /// <para>デフォルト値:Master</para>
        /// </summary>
        public CameraSynchronization cameraSynchronization = CameraSynchronization.Master;

        /// <summary>
        /// *TODO+ B 内部処理用
        /// シリアライズ可能なCamera設定
        /// </summary>
        [System.Serializable]
        public class deviceCameraSettingsJson
        {
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string id = null;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public int defaultDepthResolutionWidth = 0;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public int defaultDepthResolutionHeight = 0;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public int depthDelay = 0;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public int depthDelayAREngine = 0;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public int depthDelayAREngineBody = 0;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public int depthDelayARFoundation = 0;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public int depthDelayARFoundationARImage = 0;
        }


        [System.Serializable]
        private class calibrationTransformationJson
        {
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string useRotation = null;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string useTranslation = null;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string isDepthIntrinsicsFromColor = null;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string useAutoCalibIntrinsics = null;
        }

        [System.Serializable]
        private class deviceAttributesJson
        {
            /// <summary>
            /// TODO+ C
            /// </summary>
            public deviceCameraSettingsJson[] cameraSettings = null;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public calibrationTransformationJson calibrationTransformation = null;
            /// <summary>
            /// TODO+ C
            /// </summary>
            public float tofMgrTimeToDetectStreamUnexpectedStop = 1.5f;

            public string fixedFrontDepthCameraId = null;
        }

        private bool useRotation = true;
        private bool useTranslation = true;
        private bool isDepthIntrinsicsFromColor = false;
        private bool useAutoCalibIntrinsics = false;

        private CameraConfigurationProperty currentTofConfig;

        /// <summary>
        /// 遅延フレーム数デフォルト復帰時イベント
        /// </summary>
        [System.Serializable]
        public class SetDefaultStreamDelayEventHandler : UnityEvent<int> { }
        /// <summary>
        /// 遅延フレーム数デフォルト復帰時通知
        /// </summary>
        public SetDefaultStreamDelayEventHandler SetDefaultStreamDelay;


        private bool lensFacingChanged = false;

        private bool tofSettingsInitialized = false;

        private void OnEnable()
        {
            TofArColorManager.OnStreamStarted += OnColorStreamStarted;
            TofArColorManager.OnStreamStopped += OnColorStreamStopped;
        }

        private void OnDisable()
        {
            TofArColorManager.OnStreamStarted -= OnColorStreamStarted;
            TofArColorManager.OnStreamStopped -= OnColorStreamStopped;
        }

        private void OnColorFrameArrived(object sender)
        {
            var calibProperty = GetProperty<Tof.CalibrationSettingsProperty>();

            if (calibProperty != null)
            {
                if (calibProperty.c.fx == 0 || calibProperty.c.fy == 0 ||
                  calibProperty.c.cx == 0 || calibProperty.c.cy == 0)
                {
                    return;
                }
            }
            
            var config = GetProperty<Tof.Camera2ConfigurationProperty>();
            string depthCameraId = "0";
            if (config != null)
            {
                depthCameraId = config.cameraId;
            }

            TofArColorManager.OnFrameArrived -= OnColorFrameArrived;
            calibrationSettings.c = calibProperty.c;
            this.context.Post((s) =>
            {
                // save to persistent path if file not available
                SaveCalibrationToPersistentPath(calibrationSettings, defaultColorId, depthCameraId);
                CalibrationSettingsLoaded?.Invoke(calibrationSettings);
            }, null);
        }

        

        
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (this.stream == null)
            {
                return;
            }

            if (this.ProcessTargets.processDepth != this.processDepth 
                || this.ProcessTargets.processConfidence != this.processConfidence 
                || this.ProcessTargets.processPointCloud != this.processPointCloud)
            {

                try
                {
                    this.ProcessTargets = new ProcessTargetsProperty() { processDepth = this.processDepth, processConfidence = this.processConfidence, processPointCloud = this.processPointCloud };
                    SetProperty<ProcessTargetsProperty>(this.ProcessTargets);
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set process targets: " + e.Message);
                }
            }            
        }

        private void OnColorStreamStarted(object sender, Texture2D colorTexture)
        {
            var colorConfig = TofArColorManager.Instance.GetProperty<ResolutionProperty>();
            if (colorConfig != null)
            {
                defaultColorWidth = colorConfig.width;
                defaultColorHeight = colorConfig.height;
                defaultColorId = colorConfig.cameraId;
                bool reportFailure = false;
                LoadSettingsInternal(reportFailure);
            }

            if (!IsStreamActive && TofArColorManager.Instance.IsStreamActive && TofArManager.Instance.UsingIos)
            {
                TofArColorManager.OnFrameArrived += OnColorFrameArrived;
            }
        }

        private void OnColorStreamStopped(object sender)
        {
            TofArColorManager.OnFrameArrived -= OnColorFrameArrived;
        }

        private void Start()
        {
            // ObsoleteとなったisProcessXX の値を名前を修正したフィールドに移行する
            // この処理はObsoleteフィールドを削除するまでの一時的なものである
#pragma warning disable CS0618 // Type or member is obsolete
            this.processDepth = this.isProcessDepth;
            this.processConfidence = this.isProcessConfidence;
            this.processPointCloud = this.isProcessPointCloud;
            this.ProcessTexture = this.isProcessTexture;
#pragma warning restore CS0618 // Type or member is obsolete

            this.context = SynchronizationContext.Current;
            TofArManager.Instance.TofAutoStart = this.autoStart;

            TofArManager.Instance.AddSubManager(this);
            TofArManager.OnCameraApiChanged += OnCameraApiChanged;

            currentTofConfig = GetProperty<Camera2ConfigurationProperty>();

            if (this.autoStart)
            {
                this.StartCoroutine(this.StartProcess());
            }

            if (!this.tofSettingsInitialized)
            {
                InitTofSettings();
            }
        }

        private void OnCameraApiChanged()
        {
            var availableConfigs = GetProperty<CameraConfigurationsProperty>();

            if (useFrontCameraAsDefault)
            {
                SetFrontCameraAsDefault();
            }

            OnAvailableConfigurationsChanged?.Invoke(availableConfigs);
        }

        private void InitTofSettings()
        {
            // Use desired fps at beginning
            try
            {
                TofArTofManager.Instance.SetProperty<FrameRateProperty>(new FrameRateProperty() { desiredFrameRate = this.desiredFrameRate });
            }
            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            try
            {
                //apply the stream delay from the Inspector
                SetProperty(new DelayProperty { numFramesDelay = streamDelay });
            }
            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            // load default camera id (if available)
            if (TofArColorManager.Instance != null)
            {
                var defaultColor = TofArColorManager.Instance.GetProperty<DefaultResolutionProperty>();
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Set camera Id to " + defaultColor.cameraId);
                defaultColorId = defaultColor.cameraId;
            }
            ParseDeviceCapability();

            if (autoloadCalibration)
            {
                LoadAllDepthSettings();
            }
            lastUpdateTime = Time.time;
            lastTimeFrameRecieved = Time.time;

            this.tofSettingsInitialized = true;
        }

        private void ParseDeviceCapability()
        {
            var capability = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();

            if (Application.platform == RuntimePlatform.IPhonePlayer || capability.modelName.Contains("iPhone") || capability.modelName.Contains("iPad"))
            {
                this.useRotation = false;
                this.useTranslation = false;
                this.isDepthIntrinsicsFromColor = true;
                //this.useAutoCalibIntrinsics = true;
            }
            else
            {
                //if it doesnt exist default to true so we do use the JSON
                var deviceAttributes = JsonUtility.FromJson<deviceAttributesJson>(capability.TrimmedDeviceAttributesString);
                this.useRotation = (deviceAttributes?.calibrationTransformation?.useRotation == "false") ? false : true;
                this.useTranslation = (deviceAttributes?.calibrationTransformation?.useTranslation == "false") ? false : true;
                this.isDepthIntrinsicsFromColor = deviceAttributes?.calibrationTransformation?.isDepthIntrinsicsFromColor == "true" ? true : false;
                this.useAutoCalibIntrinsics = deviceAttributes?.calibrationTransformation?.useAutoCalibIntrinsics == "true" ? true : false;
            }
        }


        private IEnumerator StartProcess()
        {
            yield return null;
            var defConf = Instance.GetProperty<Camera2DefaultConfigurationProperty>();
            if (!this.lensFacingChanged)
            {
                Instance.SetProperty(new Camera2SetConfigurationIdProperty { uid = defConf.uid });
                yield return null;
            }
            Instance.StartStream(true);
        }

        private List<IPreProcessTofData> preProcessors = new List<IPreProcessTofData>();

        /// <summary>
        /// Tofデータ送出前処理を登録する
        /// </summary>
        /// <param name="preProcessTof">Tofデータ処理クラス</param>
        public void RegisterTofPreProcessing(IPreProcessTofData preProcessTof)
        {
            preProcessors.Add(preProcessTof);
        }
        /// <summary>
        /// Tofデータ送出前処理を登録解除する
        /// </summary>
        /// <param name="preProcessTof">Tofデータ処理クラス</param>
        public void UnregisterTofPreProcessing(IPreProcessTofData preProcessTof)
        {
            preProcessors.Remove(preProcessTof);
        }


        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            this.dependedManagers?.Clear();
            this.dependedManagers = null;
            TofArManager.Instance?.RemoveSubManager(this);
            TofArManager.OnCameraApiChanged -= OnCameraApiChanged;
            this.CloseStream();
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        public void CloseStream()
        {
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
                this.IsPlaying = false;
                if (this.streamPlay.IsStarted)
                {
                    this.StopStream();
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

            lastUpdateTime = Time.time;
            if (IsStreamActive && !isTofStarting)
            {
                if (lastUpdateTime - lastTimeFrameRecieved > timeToDetectStreamUnexpectedStop)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Tof stopped unexpectedly");
                    if (OnDetectStreamUnexpectedStop != null)
                    {
                        OnDetectStreamUnexpectedStop.Invoke(this);
                    }
                    lastTimeFrameRecieved = lastUpdateTime;
                }
            }
        }

        /// <summary>
        /// ToFカメラが端末に搭載されているか確認する
        /// </summary>
        /// <returns>true：ToFカメラが端末に搭載されている。false：ToFカメラが端末に搭載されていない</returns>
        public bool CheckDevice()
        {
            bool success = true;

            bool streamOpened = false;

            try
            {
                var tofARStream = TofArManager.Instance.Stream;
                if (this.stream == null && tofARStream != null)
                {
                    this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArTofManager.StreamKey);
                    if (this.stream != null)
                    {
                        streamOpened = true;
                    }
                }
                if (this.stream == null)
                {
                    throw new ApiException(new Status() { level = ErrorLevel.Fail, cause = ErrorCause.InvalidOperation });
                }

                var configProperty = TofArTofManager.Instance.GetProperty<CameraConfigurationsProperty>();

                if (configProperty == null || configProperty.configurations == null || configProperty.configurations.Length == 0)
                {
                    throw new ApiException(new Status() { level = ErrorLevel.Fail, cause = ErrorCause.InvalidOperation });
                }
                if (configProperty.configurations.Length == 1 && configProperty.configurations[0].name.Length == 0)
                {
                    throw new ApiException(new Status() { level = ErrorLevel.Fail, cause = ErrorCause.InvalidOperation });
                }
                if (configProperty.configurations[0].name == "camera2recording")
                {
                    throw new ApiException(new Status() { level = ErrorLevel.Fail, cause = ErrorCause.InvalidOperation });
                }
            }
            catch (ApiException)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to initialize ToF. Deactivate ToF related samples");

                success = false;
            }

            if (streamOpened)
            {
                this.stream?.Dispose();
                this.stream = null;
            }
            return success;
        }

        /// <summary>
        /// TODO+ C 内部処理用？
        /// </summary>
        public bool IsTofStarting
        {
            get
            {
                return isTofStarting || isTofWithColorStarting;
            }
        }
        private bool isTofStarting = false;
        private bool isTofWithColorStarting = false;

        /// <summary>
        /// TODO+ C
        /// </summary>
        /// <param name="externalSource">TODO+ C</param>
        public void SetStreamDelayToDefault(IExternalStreamSource externalSource = null)
        {
            try
            {
                var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
                if (devcap != null && !String.IsNullOrEmpty(devcap.TrimmedDeviceAttributesString))
                {
                    var defaultNN = JsonUtility.FromJson<deviceAttributesJson>(devcap.TrimmedDeviceAttributesString);
                    var cameraConfig = this.GetProperty<CameraConfigurationProperty>();

                    foreach (var setting in defaultNN.cameraSettings)
                    {
                        if (setting.id.Equals(cameraConfig.cameraId))
                        {
                            if (externalSource != null)
                            {
                                this.StreamDelay = externalSource.GetStreamDelay(setting);
                            }
                            else
                            {
                                this.StreamDelay = setting.depthDelay;
                            }


                            if (SetDefaultStreamDelay != null)
                            {
                                SetDefaultStreamDelay.Invoke(this.StreamDelay);
                            }

                            return;
                        }
                    }
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Could not find settings for id {cameraConfig.cameraId}");

                }
                else
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Could not load the device capabibility ");
                }
            }
            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set delay: " + e.Message);
            }
        }


        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="configuration">ストリーミングに使用するカメラ設定</param>
        /// <param name="isProcessTexture">カメラデータのTexture2D変換処理を実施するかどうか</param>
        public void StartStream(CameraConfigurationProperty configuration, bool isProcessTexture = false)
        {
            StartStream(configuration, null, isProcessTexture);
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="configuration">ストリーミングに使用するConfiguration</param>
        /// <param name="metadataProperties">ストリーミングで使用するメタデータ</param>
        /// <param name="isProcessTexture">カメラデータのTexture2D変換処理を実施するかどうか</param>
        public void StartStream(CameraConfigurationProperty configuration, List<ITofMetadataProperty> metadataProperties, bool isProcessTexture = false)
        {
            StartCoroutine(StartStreamCoroutine(configuration, metadataProperties, isProcessTexture));
        }

        private IEnumerator SetConfigBeforeStart(CameraConfigurationProperty configuration)
        {
            bool streamStillStopping = true;
            //don't loop infinitely
            for (int tries = 0; tries <= tofStartTries && streamStillStopping; tries++)
            {
                try
                {
                    if (configuration.uid == (int)DepthConfigSpecialUIDs.RecordingPlayback ||
                        configuration.uid == (int)DepthConfigSpecialUIDs.LoopedRecordingPlayback ||
                        configuration.uid == (int)DepthConfigSpecialUIDs.ExternalFunction)
                    {
                        this.SetProperty(configuration);
                    }
                    else
                    {
                        var configProperty = new SetConfigurationIdProperty();
                        configProperty.uid = configuration.uid;

                        this.SetProperty(configProperty);
                    }
                    streamStillStopping = false;
                }
                catch (SensCord.ApiException e)
                {
                    if (tries >= tofStartTries)
                    {
                        throw e;
                    }
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Tof not yet stopped: error {e.Message}");
                    Stream.Stop();
                    streamStillStopping = true;
                }
                if (streamStillStopping)
                {
                    yield return new WaitForSeconds(0.15f);
                }
            }
        }

        private void StopDependManagers(ref List<IDependManager> autoStoppedDependManagers)
        {
            foreach (var dependedManager in this?.dependedManagers)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"dependedManager:{dependedManager}");
                if (dependedManager.IsStreamActive)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"stop dependedManager:{dependedManager}");
                    try
                    {
                        dependedManager.RestartStreamByDependManager(this);
                        autoStoppedDependManagers.Add(dependedManager);
                    }
                    catch (SensCord.ApiException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                    }
                }
            }
        }

        private void RestartDependManagers(List<IDependManager> autoStoppedDependManagers)
        {
            // Restart dependend managers
            foreach (var dependedManager in autoStoppedDependManagers)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"dependedManager:{dependedManager}");
                if (!dependedManager.IsStreamActive)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"restart dependedManager:{dependedManager}");
                    try
                    {
                        dependedManager.FinalizeRestartStreamByDependManager(this);
                    }
                    catch (SensCord.ApiException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                    }
                }
            }
        }

        private IEnumerator StartStreamCoroutine(CameraConfigurationProperty configuration, List<ITofMetadataProperty> metadataProperties, bool isProcessTexture = false)
        {
            if (!this.tofSettingsInitialized)
            {
                InitTofSettings();
            }

            try
            {
                // Stop dependend managers before start stream
                if (this?.dependedManagers != null)
                {
                    StopDependManagers(ref autoStoppedDependManagers);
                }

                if (!this.IsStreamActive && !isTofStarting && this.isUnPaused)
                {
                    isTofStarting = true;

                    if (this.streamPlay != null && this.streamPlay.IsStarted)
                    {
                        this.StopPlayback();
                    }

                    this.processTexture = isProcessTexture;
                    //search for IExternalStreamSource instances
                    var externalSource = Utils.FindFirstActiveGameObjectThatImplements<IExternalStreamSource>();
                    if (externalSource != null)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, $"StartStream to ExternalStreamSource : {externalSource.GetType().Name}");
                        yield return externalSource.WaitForStreamStart();
                        configuration = externalSource.StartStream(configuration);
                        if (configuration != null && configuration.name == "failed")
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, "external source returned a failed config");
                            isTofStarting = false;
                            isTofWithColorStarting = false;

                            RestartDependManagers(autoStoppedDependManagers);
                            autoStoppedDependManagers.Clear();
                            yield break;
                        }
                    }
                    if (useDefaultStreamDelay)
                    {
                        SetStreamDelayToDefault(externalSource);
                    }
                    else
                    {
                        this.StreamDelay = this.streamDelay;
                    }
                    if (configuration != null)
                    {
                        pauseCurrentConfig = configuration;
                        yield return SetConfigBeforeStart(configuration);
                    }
                    else
                    {
                        configuration = GetProperty<CameraConfigurationProperty>();
                        pauseCurrentConfig = configuration;
                    }
                    if (configuration.isFusion)
                    {
                        ConfigureFusion(ref configuration);
                    }
                    currentTofConfig = configuration;

                    if (this.ProcessTexture)
                    {
                        var currentConfiguration = Instance.GetProperty<CameraConfigurationProperty>();
                        CreateTofTextures(currentConfiguration.width, currentConfiguration.height);
                    }
                    yield return StartInOrder(metadataProperties);
                }

            }
            finally
            {
                isTofStarting = false;
                isTofWithColorStarting = false;

                RestartDependManagers(autoStoppedDependManagers);
                autoStoppedDependManagers.Clear();
            }
        }

        private void ConfigureFusion(ref CameraConfigurationProperty configuration)
        {
            //fusion takes a while to get started
            timeToDetectStreamUnexpectedStop = unexpectedStop_fusion;
            //using fusion requires color
            var colorRes = TofArColorManager.Instance?.GetProperty<ResolutionProperty>();
            if (colorRes != null)
            {
                defaultColorHeight = colorRes.height;
                defaultColorWidth = colorRes.width;
                LoadSettings(configuration.colorCameraId, colorRes.height, colorRes.width,
                    configuration.cameraId, configuration.fusionSourceHeight, configuration.fusionSourceWidth,
                     false);
            }
            else
            {
                LoadSettings();
                colorRes = new ResolutionProperty { height = defaultColorHeight, width = defaultColorWidth };
            }

            TofArColorManager.Instance?.StartStream(true);

            //check that the configuration ratio matches - if it doesn't, swap to the correct one
            if (configuration.width / (float)configuration.height != colorRes.width / (float)colorRes.height)
            {
                var availableConfs = GetProperty<CameraConfigurationsProperty>();
                foreach (var conf in availableConfs.configurations)
                {
                    if (conf.isFusion && conf.isDepthPrivate == configuration.isDepthPrivate &&
                        conf.fusionSourceWidth == configuration.fusionSourceWidth &&
                        conf.fusionSourceHeight == configuration.fusionSourceHeight)
                    {
                        if (conf.width / (float)conf.height == colorRes.width / (float)colorRes.height)
                        {
                            configuration = conf;
                            var configProperty = new SetConfigurationIdProperty();
                            configProperty.uid = configuration.uid;

                            this.SetProperty(configProperty);
                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Colorと同時にストリーミングを停止する。
        /// </summary>
        public void StopStreamWithColor()
        {
            var cameraStartOrder = TofArManager.Instance.CameraStartOrder;

            if (cameraStartOrder == CameraStartOrder.ColorToDepth)
            {
                TofArTofManager.Instance.StopStream(false);
                Color.TofArColorManager.Instance.StopStream(false);
            }
            else
            {
                Color.TofArColorManager.Instance.StopStream(false);
                TofArTofManager.Instance.StopStream(false);
            }
        }

        /// <summary>
        /// TofとColor同時ストリーミングを開始する
        /// </summary>
        /// <param name="tofConf">Tofストリーミングに使用するConfiguration</param>
        /// <param name="colorConf">Colorストリーミングで使用するConfiguratio</param>
        /// <param name="processTextureTof">TofデータのTexture2D変換処理を実施するかどうか</param>
        /// <param name="processTextureColor">ColorデータのTexture2D変換処理を実施するかどうか</param>
        /// <param name="metadataPropertiesTof">Tofストリーミングで使用するメタデータ</param>
        /// <param name="metadataPropertiesColor">Colorストリーミングで使用するメタデータ</param>
        public void StartStreamWithColor(CameraConfigurationProperty tofConf, Color.ResolutionProperty colorConf, bool processTextureTof, bool processTextureColor, List<ITofMetadataProperty> metadataPropertiesTof = null, List<IColorMetadataProperty> metadataPropertiesColor = null)
        {
            if (this.isTofWithColorStarting)
            {
                return;
            }
            this.isTofWithColorStarting = true;

            if (autoSwitchColorCameraId && tofConf != null)
            {
                if (colorConf == null)
                {
                    colorConf = TofArColorManager.Instance.GetProperty<ResolutionProperty>();
                }
                if (colorConf != null && colorConf.cameraId != tofConf.cameraId)
                {
                    var colorConfigs = TofArColorManager.Instance.GetProperty<AvailableResolutionsProperty>();
                    //use the same resolution if we can
                    bool noGoodResFound = true;
                    ResolutionProperty bestEffortResolution = null;
                    int resolutionDifference = 1000000;
                    foreach (var res in colorConfigs.resolutions)
                    {
                        if (res.cameraId == tofConf.colorCameraId)
                        {
                            if (colorConf.height == res.height && colorConf.width == res.width)
                            {
                                colorConf = res;
                                noGoodResFound = false;
                                break;
                            }
                            int resDif = Mathf.Abs(colorConf.height - res.height) + Mathf.Abs(colorConf.width - res.width);
                            if (bestEffortResolution == null || resDif < resolutionDifference)
                            {
                                resolutionDifference = resDif;
                                bestEffortResolution = res;
                            }
                        }
                    }
                    if (noGoodResFound && bestEffortResolution != null)
                    {
                        colorConf = bestEffortResolution;
                    }
                }
            }

            if (colorConf != null)
            {
                try
                {
                    TofArColorManager.Instance.SetProperty(colorConf);
                }
                catch (SensCord.ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "failed to set color configuration: " + Utils.FormatException(e));
                }
            }

            var cameraStartOrder = TofArManager.Instance.CameraStartOrder;

            // Stop dependend managers here first to avoid problems on shared camera devices
            if (this?.dependedManagers != null)
            {
                StopDependManagers(ref autoStoppedDependManagers);
            }

            if (cameraStartOrder == CameraStartOrder.ColorToDepth)
            {
                Color.TofArColorManager.Instance.StartStream(colorConf, metadataPropertiesColor, processTextureColor);
                TofArTofManager.Instance.StartStream(tofConf, metadataPropertiesTof, processTextureTof);
            }
            else
            {
                if (colorConf != null)
                {
                    defaultColorWidth = colorConf.width;
                    defaultColorHeight = colorConf.height;
                    defaultColorId = colorConf.cameraId;
                }
                TofArTofManager.Instance.StartStream(tofConf, metadataPropertiesTof, processTextureTof);
                Color.TofArColorManager.Instance.StartStream(colorConf, metadataPropertiesColor, processTextureColor);
            }
        }

        /// <summary>
        /// Colorと同時にストリーミングを開始する。Colorはデフォルト解像度設定で開始する。
        /// </summary>
        /// <param name="tofConf">ストリーミングに使用するToFカメラ設定</param>
        /// <param name="processTextureTof">ToFカメラデータのTexture2D変換処理を実施するかどうか</param>
        /// <param name="processTextureColor">ColorカメラデータのTexture2D変換処理を実施するかどうか</param>
        public void StartStreamWithColor(CameraConfigurationProperty tofConf, bool processTextureTof, bool processTextureColor)
        {
            StartStreamWithColor(tofConf, null, processTextureTof, processTextureColor);
        }

        /// <summary>
        /// Colorと同時にストリーミングを開始する。Colorはデフォルト解像度設定で開始する。カメラデータのTexture2D変換処理は有効となる。
        /// </summary>
        /// <param name="tofConf">ストリーミングに使用するToFカメラ設定</param>
        /// <param name="processTextureTof">ToFカメラデータのTexture2D変換処理を実施するかどうか</param>
        public void StartStreamWithColor(CameraConfigurationProperty tofConf, bool processTextureTof = true)
        {
            StartStreamWithColor(tofConf, null, processTextureTof, true);
        }

        private IEnumerator StartInOrder(List<ITofMetadataProperty> metadataProperties)
        {
            if (TofArManager.Instance == null)
            {
                yield return null;
            }
            CameraStartOrder startOrder = TofArManager.Instance.CameraStartOrder;

            if (startOrder == CameraStartOrder.ColorToDepth && (TofArManager.Instance.ColorAutoStart || TofArManager.Instance.WasPaused))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Wait for Color to finish");
                while (!TofArManager.Instance.ColorStarted)
                {
                    yield return null;
                }

                TofArManager.Logger.WriteLog(LogLevel.Debug, "Color finished");
            }
            try
            {
                Stream s = this.Stream;

                if (s != null)
                {
                    this.Stream?.Start();

                    SetMetadata(metadataProperties);
                    isTofStarting = false;
                    isTofWithColorStarting = false;



                    try
                    {
                        this.SetProperty<FrameRateProperty>(new FrameRateProperty() { desiredFrameRate = this.desiredFrameRate });
                    }
                    catch (ApiException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set desired framerate: " + e.Message);
                    }

                    if (autoloadCalibration)
                    {
                        LoadSettings();
                    }
                    this.lastTimeFrameRecieved = this.lastUpdateTime;


                    if (OnStreamStarted != null)
                    {
                        OnStreamStarted(this, this.DepthTexture, this.ConfidenceTexture, this.PointCloudData);
                    }

                    TofArManager.Instance.TofStarted = true;
                }
            }
            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message + e.StackTrace);
            }
            finally
            {
                isTofStarting = false;
                isTofWithColorStarting = false;
            }
            yield return null;
        }

        private void SetMetadata(List<ITofMetadataProperty> metadataProperties)
        {
            if (metadataProperties != null)
            {
                foreach (var property in metadataProperties)
                {
                    if (property is FrameRateProperty)
                    {
                        this.SetProperty((FrameRateProperty)property);
                    }
                }
            }
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="isProcessTexture">カメラデータのTexture2D変換処理を実施するかどうか</param>
        public void StartStream(bool isProcessTexture = false)
        {
            this.StartStream(null, isProcessTexture);
        }

        /// <summary>
        /// ストリーミングを停止する
        /// </summary>
        public void StopStream()
        {
            this.StopStream(true);
        }

        /// <summary>
        /// ストリーミングを停止する
        /// </summary>
        /// <param name="isRestart">trueの場合はストリームを中止してすぐ再起動する、falseの場合は再起動せずに終了する</param>
        public void StopStream(bool isRestart = true)
        {
            StopStreamInternal(isRestart);

            if (TofArManager.Instance != null)
            {
                TofArManager.Instance.TofStarted = false;
            }
        }


        private void StopStreamInternal(bool isRestart = true)
        {
            if (this.streamPlay != null && this.streamPlay.IsStarted)
            {
                this.StopPlayback();
            }
            if (this.Stream != null)
            {
                SetProperty(new SharedStreamsProperty { isRestart = isRestart });

                //catch the case where the previous stop failed
                bool wasActive = this.IsStreamActive;
                this.Stream?.Stop();

                // clear texture
                this.ConfidenceTexture = null;
                this.DepthTexture = null;

                if (wasActive)
                {
                    var externalSource = Utils.FindFirstActiveGameObjectThatImplements<IExternalStreamSource>();
                    if (externalSource != null)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, $"StopStream to ExternalStreamSource : {externalSource.GetType().Name}");
                        externalSource.StopStream();
                    }

                    if (OnStreamStopped != null)
                    {
                        OnStreamStopped(this);
                    }

                }

                this.streamOpenErrorOccured = false;
            }
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
                this.StopStreamInternal();
                this.isStreamPausing = true;
            }
            else if (!pause && this.isStreamPausing)
            {
                this.StartStream(this.ProcessTexture);
                this.isStreamPausing = false;
            }
        }

        private void StreamCallBack(Stream stream)
        {
            try
            {
                lock (Instance.stopLock)
                {
                    if (!TofArTofManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    instance.lastTimeFrameRecieved = instance.lastUpdateTime;
                    var frame = stream.GetFrame();
                    if (frame.Channels.Length == 0)
                    {
                        frame.Dispose();
                        return;
                    }
                    try
                    {
                        if (TofArManager.Instance.UsingIos && CalibrationSettingsStatus == CalibrationSettingsStatusType.BasicCalibration)
                        {
                            // fetch calibration settings if not available yet

                            var calibProperty = GetProperty<Tof.CalibrationSettingsProperty>();

                            if (calibProperty.d.fx > 0 && calibProperty.d.fy > 0)
                            {
                                string resFolder = string.Format("c{4}_{3}x{2}_c{5}_{1}x{0}", currentTofConfig.height, currentTofConfig.width, calibProperty.colorHeight, calibProperty.colorWidth, defaultColorId, currentTofConfig.cameraId);

                                calibrationSettings = calibProperty;
                                loadedSettings[resFolder] = new PreparedSettings { settings = calibrationSettings, status = CalibrationSettingsStatusType.LoadedWithRequestedColorResolution, isFromAutoCalib = false };
                                CalibrationSettingsStatus = CalibrationSettingsStatusType.LoadedWithRequestedColorResolution;

                                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Apply new calibration {calibrationSettings.d.fx}-{calibrationSettings.d.fy}");
                                instance.context.Post((s) =>
                                {
                                    CalibrationSettingsLoaded?.Invoke(calibrationSettings);
                                }, null);
                                
                            }
                            
                        }

                        instance.frameCount++;

                        foreach (Channel channel in frame.Channels)
                        {
                            long channelId = channel.Id;
                            if (instance.ProcessTargets.processDepth && channelId == (long)ChannelIds.Depth)
                            {
                                ProcessDepth(channel);
                            }
                            else if (instance.ProcessTargets.processConfidence && channelId == (long)ChannelIds.Confidence)
                            {
                                ProcessConfidence(channel);
                            }
                            else if (instance.ProcessTargets.processPointCloud && channelId == (long)ChannelIds.PointCloud)
                            {
                                ProcessPointCloud(channel);
                            }
                        }

                    }
                    finally
                    {
                        frame.Dispose();
                    }
                    if (OnFrameArrived != null)
                    {
                        OnFrameArrived(instance);
                    }
                }
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
        }

        private void ProcessDepth(Channel depthChannel)
        {
            var rawData = depthChannel.GetRawData();
            Instance.DepthData = new DepthData(rawData);
            foreach (var processor in preProcessors)
            {
                Instance.DepthData.Data = processor.ParseDepthData(Instance.DepthData.Data);
            }
            if (Instance.processTexture)
            {
                var srcBuffer = Instance.DepthData.Data;
                var destBufferSize = srcBuffer.Length * 2;
                if (Instance.depthBuffer.Length != destBufferSize)
                {
                    Array.Resize(ref Instance.depthBuffer, destBufferSize);
                }

                {
                    Buffer.BlockCopy(srcBuffer, 0, Instance.depthBuffer, 0, Instance.depthBuffer.Length);
                }

                Instance.context.Post((s) =>
                {
                    if (Instance.DepthTexture != null && Instance.depthBuffer.Length > 0)
                    {
                        Instance.DepthTexture.LoadRawTextureData(Instance.depthBuffer);
                        Instance.DepthTexture.Apply();
                    }
                }, null);
            }
        }

        private void ProcessConfidence(Channel confidenceChannel)
        {
            var rawData = confidenceChannel.GetRawData();
            Instance.ConfidenceData = new ConfidenceData(rawData);
            foreach (var processor in preProcessors)
            {
                Instance.ConfidenceData.Data = processor.ParseConfidenceData(Instance.ConfidenceData.Data);
            }
            if (Instance.processTexture)
            {
                var srcBuffer = Instance.ConfidenceData.Data;
                var destBufferSize = srcBuffer.Length * 2;
                if (Instance.confBuffer.Length != destBufferSize)
                {
                    Array.Resize(ref Instance.confBuffer, destBufferSize);
                }

                {
                    Buffer.BlockCopy(srcBuffer, 0, Instance.confBuffer, 0, Instance.confBuffer.Length);
                }

                Instance.context.Post((s) =>
                {
                    if (Instance.ConfidenceTexture != null && Instance.confBuffer.Length > 0)
                    {
                        Instance.ConfidenceTexture.LoadRawTextureData(Instance.confBuffer);
                        Instance.ConfidenceTexture.Apply();
                    }
                }, null);
            }
        }

        private void ProcessPointCloud(Channel pointCloudChannel)
        {
            var rawData = pointCloudChannel.GetRawData();
            if (Instance.PointCloudData == null)
            {
                Instance.PointCloudData = new PointCloudData(rawData);
            }
            else
            {
                Instance.PointCloudData.UpdateFromRawData(rawData);
            }
            foreach (var processor in preProcessors)
            {
                Instance.PointCloudData.Points = processor.ParsePointCloudData(Instance.PointCloudData.Points);
            }
        }

        /// <summary>
        /// 処理対象のチャネルを設定する
        /// </summary>
        /// <param name="depth">trueの場合Depthチャネルの処理を有効化する</param>
        /// <param name="confidence">trueの場合Confidenceチャネルの処理を有効化する</param>
        /// <param name="pointCloud">trueの場合PointCloudチャネルの処理を有効化する</param>
        public void SetProcessTargets(bool depth, bool confidence, bool pointCloud)
        {
            var property = new ProcessTargetsProperty() { processDepth = depth, processConfidence = confidence, processPointCloud = pointCloud };
            this.SetProperty<ProcessTargetsProperty>(property);
            this.ProcessTargets = property;
        }

        private void CreateTofTextures(int width, int height)
        {


            if (this.DepthTexture == null ||
                this.DepthTexture.width != width ||
                this.DepthTexture.height != height)
            {
                this.DepthTexture = new Texture2D(width, height, TextureFormat.RGBA4444, false);

                DepthTexture.filterMode = FilterMode.Point;
            }

            if (this.ConfidenceTexture == null ||
                this.ConfidenceTexture.width != width ||
                this.ConfidenceTexture.height != height)
            {
                this.ConfidenceTexture = new Texture2D(width, height, TextureFormat.RGBA4444, false);
                ConfidenceTexture.filterMode = FilterMode.Point;
            }
        }

        /// <summary>
        /// コンポーネントプロパティを取得する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>() where T : class, IBaseProperty, new()
        {
            if ((this.isUnPaused && this.Stream != null) || (this.IsPlaying && this.StreamPlay != null))
            {
                T property = new T();
                int bufferSize = 1024;
                //handle types that need a larger buffer
                if (property is CameraConfigurationsProperty)
                {
                    bufferSize = 16384;
                }
                var stream = this.IsPlaying ? this.StreamPlay : this.Stream;
                stream?.GetProperty<T>(property.Key, ref property, bufferSize);

                if (property is Camera2DefaultConfigurationProperty)
                {
                    Camera2DefaultConfigurationProperty defaultConfig = property as Camera2DefaultConfigurationProperty;
                    if (this.useFrontCameraAsDefault && defaultConfig.lensFacing != (int)LensFacing.Front)
                    {
                        var defaultFrontConfig = GetDefaultFrontCameraConfig();

                        if (defaultFrontConfig != null)
                        {
                            return defaultFrontConfig as T;
                        }
                    }
                }

                return property;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// シリアライズ用バッファサイズを指定してコンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">BaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <param name="bufferSize">シリアライズ用バッファサイズ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value, int bufferSize) where T : class, IBaseProperty, new()
        {
            if ((this.isUnPaused && this.Stream != null) || (this.IsPlaying && this.StreamPlay != null))
            {
                var stream = this.IsPlaying ? this.StreamPlay : this.Stream;
                stream?.GetProperty<T>(value.Key, ref value, bufferSize);

                if (value is Camera2DefaultConfigurationProperty)
                {
                    Camera2DefaultConfigurationProperty defaultConfig = value as Camera2DefaultConfigurationProperty;
                    if (this.useFrontCameraAsDefault && defaultConfig.lensFacing != (int)LensFacing.Front)
                    {
                        var defaultFrontConfig = GetDefaultFrontCameraConfig();

                        if (defaultFrontConfig != null)
                        {
                            return defaultFrontConfig as T;
                        }
                    }
                }

                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value) where T : class, IBaseProperty
        {
            if ((this.isUnPaused && this.Stream != null) || (this.IsPlaying && this.StreamPlay != null))
            {
                int bufferSize = 1024;
                if (value is CameraConfigurationsProperty)
                {
                    bufferSize = 16384;
                }
                var stream = this.IsPlaying ? this.StreamPlay : this.Stream;

                stream?.GetProperty<T>(value.Key, ref value, bufferSize);

                if (value is Camera2DefaultConfigurationProperty)
                {
                    Camera2DefaultConfigurationProperty defaultConfig = value as Camera2DefaultConfigurationProperty;
                    if (this.useFrontCameraAsDefault && defaultConfig.lensFacing != (int)LensFacing.Front)
                    {
                        var defaultFrontConfig = GetDefaultFrontCameraConfig();

                        if (defaultFrontConfig != null)
                        {
                            return defaultFrontConfig as T;
                        }
                    }
                }

                return value;
            }
            else
            {
                return null;
            }
        }

        private Camera2DefaultConfigurationProperty GetDefaultFrontCameraConfig()
        {
            var configs = GetProperty<CameraConfigurationsProperty>();

            if (configs != null)
            {
                string defaultFrontId = null;
                int defaultWidth = 0;
                int defaultHeight = 0;
                var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
                if (devcap != null && !String.IsNullOrEmpty(devcap.TrimmedDeviceAttributesString))
                {
                    var devAttrib = JsonUtility.FromJson<deviceAttributesJson>(devcap.TrimmedDeviceAttributesString);
                    defaultFrontId = devAttrib?.fixedFrontDepthCameraId;
                    foreach (var setting in devAttrib?.cameraSettings)
                    {
                        if (defaultFrontId == setting.id)
                        {
                            defaultWidth = setting.defaultDepthResolutionWidth;
                            defaultHeight = setting.defaultDepthResolutionHeight;
                        }
                        
                    }
                }

                CameraConfigurationProperty firstConfig = null;
                foreach (var config in configs.configurations)
                {
                    if (config.lensFacing == (int)LensFacing.Front)
                    {
                        if (firstConfig == null)
                        {
                            firstConfig = config;
                        }
                        if (!String.IsNullOrEmpty(defaultFrontId) && config.cameraId.Equals(defaultFrontId))
                        {
                            firstConfig = config;
                            if (defaultWidth == 0 || defaultHeight == 0)
                            {
                                break;
                            }
                            if (firstConfig.width == defaultWidth && firstConfig.height == defaultHeight)
                            {
                                break;
                            }
                        }
                    }
                }

                if (firstConfig != null)
                {
                    var defaultConfig = new Camera2DefaultConfigurationProperty()
                    {
                        name = firstConfig.name,
                        uid = firstConfig.uid,
                        lensFacing = firstConfig.lensFacing,
                        frameRate = firstConfig.frameRate,
                        isFusion = firstConfig.isFusion,
                        fusionSourceHeight = firstConfig.fusionSourceHeight,
                        fusionSourceWidth = firstConfig.fusionSourceWidth,
                        isDepthPrivate = firstConfig.isDepthPrivate,
                        unambiguousRange = firstConfig.unambiguousRange,
                        cameraId = firstConfig.cameraId,
                        colorCameraId = firstConfig.colorCameraId,
                        width = firstConfig.width,
                        height = firstConfig.height,
                        intrinsics = firstConfig.intrinsics,
                        isCalibrated = firstConfig.isCalibrated
                    };

                    return defaultConfig;
                }

            }

            return null;
        }

        private bool isSpecialConfigs = false;
        private int prevUID = -1;

        /// <summary>
        /// コンポーネントプロパティを設定する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (value is FrameRateProperty)
            {
                this.desiredFrameRate = (value as FrameRateProperty).desiredFrameRate;
            }

            int newId = prevUID;
            bool isConfigProp = false;
            if (value is CameraConfigurationProperty)
            {
                isConfigProp = true;
                newId = (value as CameraConfigurationProperty).uid;
            }

            if (value is Camera2SetConfigurationIdProperty)
            {
                isConfigProp = true;
                newId = (value as Camera2SetConfigurationIdProperty).uid;
            }


            if (this.isUnPaused && this.Stream != null)
            {
                this.Stream?.SetProperty<T>(value);
                if (isConfigProp)
                {
                    if (newId == (int)DepthConfigSpecialUIDs.ExternalFunction || newId == (int)DepthConfigSpecialUIDs.ExternalFunction || newId == (int)DepthConfigSpecialUIDs.ExternalFunction)
                    {
                        isSpecialConfigs = true;
                        OnAvailableConfigurationsChanged?.Invoke(GetProperty<CameraConfigurationsProperty>());
                    }
                    else if (newId != prevUID && isSpecialConfigs)
                    {
                        isSpecialConfigs = false;
                        OnAvailableConfigurationsChanged?.Invoke(GetProperty<CameraConfigurationsProperty>());
                    }
                    prevUID = newId;
                }
            }
        }

        /*public uint GetPropertyCount()
        {
            return this.Stream?.GetPropertyCount();
        }*/

        /// <summary>
        /// Propertyリストを取得する
        /// </summary>
        /// <returns>Propertyリスト</returns>
        public string[] GetPropertyList()
        {
            return this.Stream?.GetPropertyList();
        }

        private CameraConfigurationProperty pauseCurrentConfig;
        private bool isColorPaused = false;

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
            this.isStreamPausing = this.IsStreamActive;
            if (this.stream != null)
            {
                if (this.IsStreamActive)
                {
                    if (TofArColorManager.Instantiated && TofArColorManager.Instance.IsStreamActive)
                    {
                        this.StopStreamWithColor();
                        TofArColorManager.Instance.PauseStream();
                        this.isColorPaused = true;
                    }
                    else
                    {
                        this.StopStream();
                        this.isColorPaused = false;
                    }
                }
                this.CloseStream();
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
            if (this.isStreamPausing)
            {
                this.isStreamPausing = false;
                if (this.isColorPaused)
                {
                    this.isColorPaused = false;
                    TofArColorManager.Instance.UnpauseStream();
                    this.StartStreamWithColor(this.pauseCurrentConfig, this.ProcessTexture);
                }
                else
                {
                    this.StartStream(this.pauseCurrentConfig, this.ProcessTexture);
                }
            }
        }

        #region settingsLoading

#if UNITY_IOS
        private string fallbackSettings = "979.858826,979.858826,636.952942,360.374817,186.003343,186.003343,123.793625,71.909279,1,0,0,0,1,0,0,0,1,0,0,0,1280,720";
#else
        private string fallbackSettings = "694.21552,687.00808,646.46436,381.8128,291.112915,291.112915,158.926208,123.750786,0.987782563,-0.029007993,0.153114808,0.01237445,0.994020625,0.108489034,-0.155346327,-0.105268864,0.982235198,10.7455509,-23.21650917,22.23503754,1280,720";
#endif

        /// <summary>
        /// キャリブレーションの種類
        /// </summary>
        public enum CalibrationSettingsStatusType
        {
            /// <summary>
            /// デフォルトキャリブレーション
            /// </summary>
            BasicCalibration,
            /// <summary>
            /// 現在のカメラ解像度に対し、ToFカメラのキャリブレーション情報が読み込まれているが、Colorカメラは未キャリブレーション状態である
            /// </summary>
            LoadedWithDefaultColorResolution,
            /// <summary>
            /// 現在のカメラ解像度に対し、ToFカメラおよびColorカメラのキャリブレーション情報が読み込まれている
            /// </summary>
            LoadedWithRequestedColorResolution
        }

        /// <summary>
        /// カメラキャリブレーション情報の状態
        /// </summary>
        public CalibrationSettingsStatusType CalibrationSettingsStatus { get; private set; }
        CalibrationSettingsProperty calibrationSettings;
        /// <summary>
        /// カメラキャリブレーション情報
        /// </summary>
        public CalibrationSettingsProperty CalibrationSettings
        {
            get
            {
                if (calibrationSettings == null)
                {
                    LoadSettings();
                }
                return calibrationSettings;
            }
            set
            {
                calibrationSettings = value;
                //when the value is set externally we need to inform anyone expecting the value to change
                CalibrationSettingsLoaded.Invoke(value);
                //update the buffer if needed
                var currentConf = GetProperty<CameraConfigurationProperty>();
                if (currentConf != null)
                {
                    loadedSettings[string.Format("c{4}_{3}x{2}_c{5}_{1}x{0}", value.depthHeight, value.depthWidth, value.colorHeight, value.colorWidth, currentConf.colorCameraId, currentConf.cameraId)] =
                                          new PreparedSettings { settings = value, status = CalibrationSettingsStatusType.LoadedWithRequestedColorResolution };
                }
            }
        }

        private CalibrationSettingsProperty SetupConfigProperty(CalibrationSettingsProperty previousSettings)
        {
            if (!TofArManager.Instance.UsingIos)
            {
                //internal calib needs to be before adjustment
                SetProperty(previousSettings);
            }
            bool wasFusion = currentTofConfig?.isFusion ?? false;
            if (wasFusion)
            {
                float xscale = (float)currentTofConfig.width / previousSettings.colorWidth;
                float yscale = (float)currentTofConfig.height / previousSettings.colorHeight;

                var fusionSettings = new CalibrationSettingsProperty
                {
                    colorHeight = previousSettings.colorHeight,
                    colorWidth = previousSettings.colorWidth,
                    c = previousSettings.c,
                    R = new Matrix { a = 1, e = 1, i = 1 },//previousSettings.R,
                    T = new Vector { x = 0, y = 0, z = 0 },//previousSettings.T,
                    isCalibrated = previousSettings.isCalibrated,
                    depthWidth = currentTofConfig.width,
                    depthHeight = currentTofConfig.height,
                    d = new InternalParameter
                    {
                        cx = previousSettings.c.cx * xscale,
                        fx = previousSettings.c.fx * xscale,
                        cy = previousSettings.c.cy * yscale,
                        fy = previousSettings.c.fy * yscale
                    }
                };

                return fusionSettings;
            }
            else
            {

                return previousSettings;
            }
        }

        private struct PreparedSettings
        {
            public CalibrationSettingsProperty settings;
            public CalibrationSettingsStatusType status;
            public bool isFromAutoCalib;
        }

        private Dictionary<string, PreparedSettings> loadedSettings = new Dictionary<string, PreparedSettings>();

        /// <summary>
        /// キャリブレーション設定リストをクリアする
        /// </summary>
        public void ClearLoadedSettings()
        {
            loadedSettings.Clear();
        }

        /// <summary>
        /// カメラキャリブレーション設定ロード時デリゲート
        /// </summary>
        [System.Serializable]
        public class CalibrationSettingsLoadedEventHandler : UnityEvent<CalibrationSettingsProperty> { }
        /// <summary>
        /// カメラキャリブレーション設定ロード通知
        /// </summary>
        public CalibrationSettingsLoadedEventHandler CalibrationSettingsLoaded;
        /// <summary>
        /// カメラキャリブレーション情報ロード時に使用するデフォルトColorカメラ解像度Width。デフォルト値:1280
        /// </summary>
        public int defaultColorWidth = 1280;
        /// <summary>
        /// カメラキャリブレーション情報ロード時に使用するデフォルトColorカメラ解像度Height。デフォルト値:720
        /// </summary>
        public int defaultColorHeight = 720;
        /// <summary>
        /// カメラキャリブレーション情報ロード時に使用するデフォルトColorカメラID。デフォルト値:"0"
        /// </summary>
        public string defaultColorId = "0";

        /// <summary>
        /// カメラキャリブレーション設定ロード失敗時デリゲート
        /// </summary>
        [System.Serializable]
        public class CalibrationSettingsFailedEventHandler : UnityEvent<int, int, int, int> { }
        /// <summary>
        /// カメラキャリブレーション設定ロード失敗通知
        /// </summary>
        public CalibrationSettingsFailedEventHandler CalibrationSettingsFailed;

        /// <summary>
        /// キャリブレーション設定をロードする
        /// </summary>
        /// <returns>キャリブレーション設定データ</returns>
        public CalibrationSettingsProperty LoadSettings()
        {
            return LoadSettingsInternal(true);
        }

        private CalibrationSettingsProperty LoadSettingsInternal(bool reportFailure)
        {
            int depth_h = 240;
            int depth_w = 320;
            string cameraId = "0";

            try
            {
                var currentConfiguration = GetProperty<CameraConfigurationProperty>();

                if (currentConfiguration != null)
                {
                    if (currentConfiguration.isFusion)
                    {
                        depth_h = currentConfiguration.fusionSourceHeight;
                        depth_w = currentConfiguration.fusionSourceWidth;
                        cameraId = currentConfiguration.cameraId;
                    }
                    else
                    {
                        depth_h = currentConfiguration.height;
                        depth_w = currentConfiguration.width;
                        cameraId = currentConfiguration.cameraId;
                    }
                }
            }
            catch (ApiException)
            {
                depth_h = 240;
                depth_w = 320;
                cameraId = "0";
            }
            return LoadSettings(cameraId, depth_h, depth_w, reportFailure);
        }

        CameraConfigurationsProperty configurations;

        private void LoadAllDepthSettings()
        {
            //dont overwrite the calibration
            var currentCalib = CalibrationSettings;
            var alreadyLoaded = new Hashtable();
            configurations = GetProperty<CameraConfigurationsProperty>();
            if (configurations != null)
            {
                foreach (var config in configurations.configurations)
                {
                    if (config.isFusion)
                    {
                        string keyformat = string.Format("{2} {0}x{1} {3}", config.fusionSourceWidth, config.fusionSourceHeight, config.cameraId, config.isDepthPrivate);
                        if (!alreadyLoaded.ContainsKey(keyformat))
                        {
                            LoadSettings(config.cameraId, config.fusionSourceHeight, config.fusionSourceWidth, false);
                            alreadyLoaded[keyformat] = true;
                        }
                    }
                    else
                    {
                        string keyformat = string.Format("{2} {0}x{1} {3}", config.width, config.height, config.cameraId, config.isDepthPrivate);
                        if (!alreadyLoaded.ContainsKey(keyformat))
                        {
                            LoadSettings(config.cameraId, config.height, config.width, false);
                            alreadyLoaded[keyformat] = true;
                        }
                    }
                }
            }
            calibrationSettings = currentCalib;
        }

        /// <summary>
        /// キャリブレーション設定をロードする
        /// </summary>
        /// <param name="depth_id">カメラID</param>
        /// <param name="depth_h">縦解像度</param>
        /// <param name="depth_w">横解像度</param>
        /// <param name="reportFailure">キャリブレーション読み込みが失敗した場合、通知イベントを発行するかどうか</param>
        /// <returns>キャリブレーション設定データ</returns>
        public CalibrationSettingsProperty LoadSettings(string depth_id, int depth_h, int depth_w, bool reportFailure)
        {
            int color_h = defaultColorHeight;
            int color_w = defaultColorWidth;
            string color_id = defaultColorId;
            if (configurations == null)
            {
                configurations = GetProperty<CameraConfigurationsProperty>();
            }
            if (IsStreamActive && configurations != null)
            {
                foreach (var conf in configurations.configurations)
                {
                    if (conf.cameraId == depth_id)
                    {
                        color_id = conf.colorCameraId;
                        break;
                    }
                }
            }
            return LoadSettings(color_id, color_h, color_w, depth_id, depth_h, depth_w, reportFailure);
        }

        /// <summary>
        /// キャリブレーション設定をロードする
        /// </summary>
        /// <param name="color_cameraId">ColorカメラID</param>
        /// <param name="color_h">Color縦解像度</param>
        /// <param name="color_w">Color横解像度</param>
        /// <param name="depth_cameraId">DepthカメラID</param>
        /// <param name="depth_h">Depth縦解像度</param>
        /// <param name="depth_w">Depth横解像度</param>
        /// <param name="reportFailure">キャリブレーション読み込みが失敗した場合、通知イベントを発行するかどうか</param>
        /// <returns>キャリブレーション設定データ</returns>
        public CalibrationSettingsProperty LoadSettings(string color_cameraId, int color_h, int color_w, string depth_cameraId, int depth_h, int depth_w, bool reportFailure)
        {
            if (string.IsNullOrEmpty(color_cameraId))
            {
                color_cameraId = "0";
            }
            if (string.IsNullOrEmpty(depth_cameraId))
            {
                depth_cameraId = "0";
            }

            string resFolder = string.Format("c{4}_{3}x{2}_c{5}_{1}x{0}", depth_h, depth_w, color_h, color_w, color_cameraId, depth_cameraId);

            if (loadedSettings.ContainsKey(resFolder))
            {
                calibrationSettings = SetupConfigProperty(loadedSettings[resFolder].settings);
                TofArManager.Logger.WriteLog(LogLevel.Debug, String.Format("using previously loaded settings for {0} : {1}", resFolder, calibrationSettings));
                SetDeviceParameters(loadedSettings[resFolder].isFromAutoCalib && loadedSettings[resFolder].status == CalibrationSettingsStatusType.LoadedWithRequestedColorResolution);
                CalibrationSettingsStatus = TofArManager.Instance.UsingIos ? CalibrationSettingsStatusType.BasicCalibration : loadedSettings[resFolder].status;
                CalibrationSettingsLoaded?.Invoke(calibrationSettings);
                return calibrationSettings;
            }
            else
            {
                try
                {
                    return LoadSettingsFromAutoCalib(resFolder, depth_w, depth_h, color_w, color_h, depth_cameraId, reportFailure);
                }
                catch (NotSupportedException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, String.Format("failed to load settings for {0}", resFolder));
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "reason was " + e.Message);
                    //failed to load, fall through to fallback
                }

                if (TofArManager.Instance.UsingIos)
                {
                    // (for iOS only) load settings from local, if available
                    try
                    {
                        return LoadSettingsFromLocal(resFolder, depth_w, depth_h, color_w, color_h, depth_cameraId, color_cameraId, reportFailure);
                    }
                    catch (NotSupportedException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, String.Format("failed to load settings for {0} from Local", resFolder));
                        TofArManager.Logger.WriteLog(LogLevel.Debug, "reason was " + e.Message);
                        //failed to load, fall through to fallback
                    }
                }
                else
                {
                    // try loading settings from resources, if available
                    try
                    {
                        return LoadSettingsFromResource(resFolder, depth_w, depth_h, color_w, color_h, depth_cameraId, color_cameraId, reportFailure);
                    }
                    catch (NotSupportedException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, String.Format("failed to load settings for {0} from Resources", resFolder));
                        TofArManager.Logger.WriteLog(LogLevel.Debug, "reason was " + e.Message);
                        //failed to load, fall through to fallback
                    }
                }
                

                
                

                //load the fallback settings
                return LoadSettingsFallback(resFolder, depth_w, depth_h, color_w, color_h, depth_cameraId, reportFailure);
            }
        }

        //throws an exeption if it can't load
        private CalibrationSettingsProperty LoadSettingsFromAutoCalib(string resFolder, int depth_w, int depth_h, int color_w, int color_h, string depth_cameraId, bool reportFailure)
        {
            string settingsString = null;

            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.MultiNode)
            {
                //attempt to load the data from where autocalib saves it
                string autocalibDataDir = Application.persistentDataPath + "/../../SonySemiconductorSolutionsCorporation/AutoCalib/DataFolder/";
                string filename = autocalibDataDir + resFolder + "/Calibration.txt";
                if (Utils.PathIsSymlink(filename))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "calib file " + filename + "was a symlink");
                }
                else
                {
                    try
                    {
                        settingsString = System.IO.File.ReadAllText(filename);
                    }
                    catch (System.IO.IOException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                try
                {
                    using (var uri = new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse",
                new object[] { "content://jp.co.sonysemicon.tofar.calibcontentprovider.provider/calibration" }))
                    {
                        using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                        {
                            using (AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                            {
                                using (var contentResolver = currentActivityObject.Call<AndroidJavaObject>("getContentResolver"))
                                {
                                    if (contentResolver != null)
                                    {
                                        using (var cursor = contentResolver.Call<AndroidJavaObject>("query", new object[] { uri, null, resFolder, null, null }))
                                        {
                                            if (cursor != null)
                                            {
                                                cursor.Call<bool>("moveToFirst");
                                                settingsString = cursor.Call<string>("getString", new object[] { 0 });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (AndroidJavaException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
                }

            }
            if (settingsString != null)
            {
                if (!ParseSettingsString(settingsString, depth_w, depth_h, color_w, color_h, depth_cameraId))
                {
                    CalibrationSettingsStatus = CalibrationSettingsStatusType.LoadedWithDefaultColorResolution;
                    loadedSettings[resFolder] = new PreparedSettings { settings = CalibrationSettings, status = CalibrationSettingsStatusType.LoadedWithDefaultColorResolution, isFromAutoCalib = true };
                    if (reportFailure)
                    {
                        CalibrationSettingsFailed.Invoke(color_w, color_h, depth_w, depth_h);
                    }
                    TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("loaded default settings for {1}: {0}", settingsString, resFolder));
                    SetDeviceParameters(false);
                }
                else
                {
                    CalibrationSettingsStatus = CalibrationSettingsStatusType.LoadedWithRequestedColorResolution;
                    TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("loaded settings for {1}: {0}", settingsString, resFolder));
                    loadedSettings[resFolder] = new PreparedSettings { settings = CalibrationSettings, status = CalibrationSettingsStatusType.LoadedWithRequestedColorResolution, isFromAutoCalib = true };
                    SetDeviceParameters(true);
                }

                calibrationSettings = SetupConfigProperty(calibrationSettings);
                CalibrationSettings.isCalibrated = true;

                if (CalibrationSettingsLoaded != null)
                {
                    CalibrationSettingsLoaded.Invoke(CalibrationSettings);
                }
                return CalibrationSettings;
            }
            throw new NotSupportedException("no calibration found in autocalib");
        }

        //throws an exeption if it can't load
        private CalibrationSettingsProperty LoadSettingsFromResource(string resFolder, int depth_w, int depth_h, int color_w, int color_h, string depth_cameraId, string color_cameraId, bool reportFailure)
        {
            DeviceCapabilityProperty property = TofAr.V0.TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
            string deviceName = property.modelGroupName;
            string path = $"DeviceProfiles/Calibrations/{deviceName}/{resFolder}";

            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Loading calibration from {path}");

            var calibration = Resources.Load(path) as TextAsset;

            if (calibration == null)
            {
                //no calib for this resolution, find default
                string defaultPath = $"DeviceProfiles/Calibrations/{deviceName}/{string.Format("c{4}_{3}x{2}_c{5}_{1}x{0}", depth_h, depth_w, 720, 1280, color_cameraId, depth_cameraId)}";
                calibration = Resources.Load(defaultPath) as TextAsset;

                //else find any for the phone for the camera id
                if (calibration == null)
                {
                    var availableCalibs = Resources.LoadAll<TextAsset>($"DeviceProfiles/Calibrations/{deviceName}");
                    // tof is inactive, but color is active
                    if (!IsStreamActive && TofArColorManager.Instance != null && (TofArColorManager.Instance.IsStreamActive || TofArColorManager.Instance.IsPlaying))
                    {
                        // Try to load any calibration that matches current color id and resolution
                        string startString = $"c{color_cameraId}_{color_w}x{color_h}";
                        calibration = GetResourceStartsWith(availableCalibs, startString) ?? calibration;
                        
                        if (calibration == null)
                        {
                            // Try to load any calibration that matches current color id only
                            startString = $"c{color_cameraId}_";
                            calibration = GetResourceStartsWith(availableCalibs, startString) ?? calibration;
                        }
                    }
                    else
                    {
                        // Try to load any calibration that matches current depth id
                        string containsString = $"_c{depth_cameraId}_";
                        calibration = GetResourceContains(availableCalibs, containsString) ?? calibration;
                    }
                }
            }

            if (calibration != null)
            {
                return ParseCalibrationSettings(calibration.text, resFolder, depth_w, depth_h, color_w, color_h, depth_cameraId, reportFailure);
            }
            throw new NotSupportedException("no calibration found in resources");
        }

        //throws an exeption if it can't load
        private CalibrationSettingsProperty LoadSettingsFromLocal(string resFolder, int depth_w, int depth_h, int color_w, int color_h, string depth_cameraId, string color_cameraId, bool reportFailure)
        {
            string directory = $"{Application.persistentDataPath}{System.IO.Path.DirectorySeparatorChar}recordings{System.IO.Path.DirectorySeparatorChar}Calibrations";
            string path = $"{directory}{System.IO.Path.DirectorySeparatorChar}{resFolder}.txt";

            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Loading calibration from {path}");

            if (!System.IO.File.Exists(path))
            {
                if (System.IO.Directory.Exists(directory))
                {
                    var availableCalibFiles = System.IO.Directory.GetFiles(directory);
                    // tof is inactive, but color is active
                    if (!IsStreamActive && TofArColorManager.Instance != null && (TofArColorManager.Instance.IsStreamActive || TofArColorManager.Instance.IsPlaying))
                    {
                        // Try to load any calibration that matches current color id and resolution
                        string startString = $"c{color_cameraId}_{color_w}x{color_h}";
                        path = GetPathStartsWith(availableCalibFiles, startString) ?? path;
                        
                        if (!System.IO.File.Exists(path))
                        {
                            // Try to load any calibration that matches current color id only
                            startString = $"c{color_cameraId}_";
                            path = GetPathStartsWith(availableCalibFiles, startString) ?? path;
                        }
                    }
                    else
                    {
                        // Try to load any calibration that matches current depth id only
                        string containString = $"_c{depth_cameraId}_";
                        path = GetPathContains(availableCalibFiles, containString) ?? path;
                    }
                }
            }

            if (System.IO.File.Exists(path))
            {
                string calibration = System.IO.File.ReadAllText(path);

                if (calibration != null)
                {
                    return ParseCalibrationSettings(calibration, resFolder, depth_w, depth_h, color_w, color_h, depth_cameraId, reportFailure);
                }
            }

            
            throw new NotSupportedException("no calibration found in local");
        }



        private CalibrationSettingsProperty LoadSettingsFallback(string resFolder, int depth_w, int depth_h, int color_w, int color_h, string depth_cameraId, bool reportFailure)
        {
            CalibrationSettingsStatus = CalibrationSettingsStatusType.BasicCalibration;
            ParseSettingsString(fallbackSettings, depth_w, depth_h, color_w, color_h, depth_cameraId);
            loadedSettings[resFolder] = new PreparedSettings { settings = CalibrationSettings, status = CalibrationSettingsStatusType.BasicCalibration };
            TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("loaded fallback settings for {0}: {1}", resFolder, fallbackSettings));
            calibrationSettings.isCalibrated = false;
            if (reportFailure)
            {
                CalibrationSettingsFailed.Invoke(color_w, color_h, depth_w, depth_h);
            }
            if (CalibrationSettingsLoaded != null)
            {
                CalibrationSettingsLoaded.Invoke(CalibrationSettings);
            }
            return CalibrationSettings;
        }

        private CalibrationSettingsProperty ParseCalibrationSettings(string calibration, string resFolder, int depth_w, int depth_h, int color_w, int color_h, string depth_cameraId, bool reportFailure)
        {
            
            if (!ParseSettingsString(calibration, depth_w, depth_h, color_w, color_h, depth_cameraId))
            {
                CalibrationSettingsStatus = TofArManager.Instance.UsingIos ? CalibrationSettingsStatusType.BasicCalibration : CalibrationSettingsStatusType.LoadedWithDefaultColorResolution;
                loadedSettings[resFolder] = new PreparedSettings { settings = CalibrationSettings, status = CalibrationSettingsStatus, isFromAutoCalib = false };
                if (reportFailure)
                {
                    CalibrationSettingsFailed.Invoke(color_w, color_h, depth_w, depth_h);
                }
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("loaded default settings for {0}: {1}", resFolder, calibration));
            }
            else
            {
                CalibrationSettingsStatus = TofArManager.Instance.UsingIos ? CalibrationSettingsStatusType.BasicCalibration : CalibrationSettingsStatusType.LoadedWithDefaultColorResolution;
                loadedSettings[resFolder] = new PreparedSettings { settings = CalibrationSettings, status = CalibrationSettingsStatus, isFromAutoCalib = false };
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("loaded settings for {0}: {1}", resFolder, calibration));
            }

            SetDeviceParameters(false);

            calibrationSettings.isCalibrated = false;
            try
            {
                calibrationSettings = SetupConfigProperty(calibrationSettings);
            }
            catch (ApiException exception)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(exception));
                if (reportFailure)
                {
                    CalibrationSettingsFailed.Invoke(color_w, color_h, depth_w, depth_h);
                }
            }

            if (CalibrationSettingsLoaded != null)
            {
                CalibrationSettingsLoaded.Invoke(CalibrationSettings);
            }
            return CalibrationSettings;
        }

        private string GetPathStartsWith(string[] paths, string startString)
        {
            foreach (var cal in paths)
            {
                string fileRelative = System.IO.Path.GetFileName(cal);
                if (fileRelative.StartsWith(startString))
                {
                    return cal;
                }
            }

            return null;
        }

        private string GetPathContains(string[] paths, string containString)
        {
            foreach (var cal in paths)
            {
                string fileRelative = System.IO.Path.GetFileName(cal);
                if (fileRelative.Contains(containString))
                {
                    return cal;
                }
            }

            return null;
        }

        private TextAsset GetResourceStartsWith(TextAsset[] paths, string startString)
        {
            foreach (var cal in paths)
            {
                if (cal.name.StartsWith(startString))
                {
                    return cal;
                }
            }

            return null;
        }

        private TextAsset GetResourceContains(TextAsset[] paths, string containString)
        {
            foreach (var cal in paths)
            {
                if (cal.name.Contains(containString))
                {
                    return cal;
                }
            }

            return null;
        }

        void SetDeviceParameters(bool forceWrite)
        {
            //dont set parameters except on the devices that need it
            if (useAutoCalibIntrinsics || forceWrite)
            {
                try
                {
                    var calibProperty = GetProperty<Tof.Camera2ConfigurationProperty>();

                    Tof.Camera2IntrinsicsProperty intrinsics = new Tof.Camera2IntrinsicsProperty
                    {
                        cx = calibrationSettings.d.cx,
                        cy = calibrationSettings.d.cy,
                        fx = calibrationSettings.d.fx,
                        fy = calibrationSettings.d.fy,
                        k1 = calibProperty?.intrinsics?.k1 ?? 0,
                        k2 = calibProperty?.intrinsics?.k2 ?? 0,
                        k3 = calibProperty?.intrinsics?.k3 ?? 0,
                        p1 = calibProperty?.intrinsics?.p1 ?? 0,
                        p2 = calibProperty?.intrinsics?.p2 ?? 0
                    };
                    Instance.SetProperty<Camera2IntrinsicsProperty>(intrinsics);
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message + e.StackTrace);
                }
            }
        }



        bool ParseSettingsString(string inputText, int depthwidth, int depthheight, int targetColorWidth, int targetColorHeight, string depthId)
        {
            var pasteSetting = inputText.Split(',');
            if (pasteSetting.Length < 20)
            {
                throw new System.ArgumentException("the given text must have 20 floats separated by commas then 2 ints");
            }
            int colorwidth = defaultColorWidth;
            int colorheight = defaultColorHeight;
            if (pasteSetting.Length > 21)
            {
                colorwidth = int.Parse(pasteSetting[20]);
                colorheight = int.Parse(pasteSetting[21]);
            }


            float fxColor = float.Parse(pasteSetting[0]);
            float fyColor = float.Parse(pasteSetting[1]);
            float cxColor = float.Parse(pasteSetting[2]);
            float cyColor = float.Parse(pasteSetting[3]);

            float fxDepth = float.Parse(pasteSetting[4]);
            float fyDepth = float.Parse(pasteSetting[5]);
            float cxDepth = float.Parse(pasteSetting[6]);
            float cyDepth = float.Parse(pasteSetting[7]);


            if (!this.useRotation && !this.useTranslation)
            {
                if (configurations == null)
                {
                    configurations = GetProperty<CameraConfigurationsProperty>();
                }
                CameraConfigurationProperty[] confProperty = configurations.configurations;
                if (confProperty != null)
                {
                    foreach (var cameraConfig in confProperty)
                    {
                        if (depthwidth == cameraConfig.width && depthheight == cameraConfig.height && cameraConfig.cameraId.Equals(depthId))
                        {

                            if (cameraConfig.intrinsics.cx > (depthwidth / 2f + 50) || cameraConfig.intrinsics.cx < (depthwidth / 2f - 50) ||
                                cameraConfig.intrinsics.cy > (depthheight / 2f + 50) || cameraConfig.intrinsics.cy < (depthheight / 2f - 50))
                            {
                                cxDepth = depthwidth / 2f;
                                cyDepth = depthheight / 2f;
                            }
                            else
                            {
                                cxDepth = cameraConfig.intrinsics.cx;
                                cyDepth = cameraConfig.intrinsics.cy;
                            }

                            cxColor = colorwidth / 2;
                            cyColor = colorheight / 2;

                            break;
                        }
                    }
                }
            }
            if (this.isDepthIntrinsicsFromColor)
            {
                //work out the fx and fy fromt the color data
                fxDepth = fxColor * depthwidth / colorwidth;
                fyDepth = fyColor * depthwidth / colorwidth;
            }

            Matrix R = new Matrix
            {
                a = 1,
                b = 0,
                c = 0,
                d = 0,
                e = 1,
                f = 0,
                g = 0,
                h = 0,
                i = 1
            };
            if (this.useRotation)
            {
                R.a = float.Parse(pasteSetting[8]);
                R.b = float.Parse(pasteSetting[9]);
                R.c = float.Parse(pasteSetting[10]);
                R.d = float.Parse(pasteSetting[11]);
                R.e = float.Parse(pasteSetting[12]);
                R.f = float.Parse(pasteSetting[13]);
                R.g = float.Parse(pasteSetting[14]);
                R.h = float.Parse(pasteSetting[15]);
                R.i = float.Parse(pasteSetting[16]);
            }
            Vector T = new Vector { x = 0, y = 0, z = 0 };
            if (this.useTranslation)
            {
                T.x = float.Parse(pasteSetting[17]);
                T.y = float.Parse(pasteSetting[18]);
                T.z = float.Parse(pasteSetting[19]);
            };

            var config = new CalibrationSettingsProperty
            {
                colorWidth = targetColorWidth,
                colorHeight = targetColorHeight,

                depthWidth = depthwidth,
                depthHeight = depthheight,

                c = new InternalParameter
                {
                    fx = fxColor * targetColorWidth / colorwidth,
                    fy = fyColor * targetColorWidth / colorwidth,
                    cx = cxColor * targetColorWidth / colorwidth,
                    cy = cyColor * targetColorHeight / colorheight
                },
                d = new InternalParameter
                {
                    fx = fxDepth,
                    fy = fyDepth,
                    cx = cxDepth,
                    cy = cyDepth
                },
                R = R,
                T = T
            };
            calibrationSettings = config;
            return colorwidth == targetColorWidth && colorheight == targetColorHeight;
        }

        /// <summary>
        /// Saves calibration files to persistent path so that they can be loaded at a later time when parameters haven't been obtained by the stream yet
        /// </summary>
        /// <param name="calibrationSettings">Calibration settings</param>
        /// <param name="color_cameraId">Camera ID for color</param>
        /// <param name="depth_cameraId">Camera ID for depth</param>
        private void SaveCalibrationToPersistentPath(CalibrationSettingsProperty calibrationSettings, string color_cameraId, string depth_cameraId)
        {
            string resFolder = string.Format("c{4}_{3}x{2}_c{5}_{1}x{0}", calibrationSettings.depthHeight, calibrationSettings.depthWidth, calibrationSettings.colorHeight, calibrationSettings.colorWidth, color_cameraId, depth_cameraId);
            string pathRecordings = $"{Application.persistentDataPath}{System.IO.Path.DirectorySeparatorChar}recordings";
            string pathCalibrations = $"{pathRecordings}{System.IO.Path.DirectorySeparatorChar}Calibrations";
            string pathCalibrationFile = $"{pathCalibrations}{System.IO.Path.DirectorySeparatorChar}{resFolder}.txt";

            if (!System.IO.File.Exists(pathCalibrationFile))
            {
                if (!System.IO.Directory.Exists(pathCalibrations))
                {
                    System.IO.Directory.CreateDirectory(pathCalibrations);
                }

                // Make to string
                string content = $"{calibrationSettings.c.fx},{calibrationSettings.c.fy}," +
                    $"{calibrationSettings.c.cx},{calibrationSettings.c.cy}," +
                    $"{calibrationSettings.d.fx},{calibrationSettings.d.fy}," +
                    $"{calibrationSettings.d.cx},{calibrationSettings.d.cy}," +
                    $"{calibrationSettings.R.a},{calibrationSettings.R.b},{calibrationSettings.R.c}," +
                    $"{calibrationSettings.R.d},{calibrationSettings.R.e},{calibrationSettings.R.f}," +
                    $"{calibrationSettings.R.g},{calibrationSettings.R.h},{calibrationSettings.R.i}," +
                    $"{calibrationSettings.T.x},{calibrationSettings.T.y},{calibrationSettings.T.z}," +
                    $"{calibrationSettings.colorWidth},{calibrationSettings.colorHeight}";

                try
                {
                    // Save to file
                    System.IO.File.WriteAllText(pathCalibrationFile, content);
                }
                catch (System.IO.IOException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to write to file due to {e.Message}");
                }
            }
        }


        #endregion

        #region PLAYBACK

        private bool useSkv = false;

        /// <summary>
        /// *TODO+ B
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_tof_stream";
        private const string streamPlayerSkvKey = "player_skv_tof_stream";

        private Stream streamPlay = null;
        /// <summary>
        /// 再生ストリーム
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
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(this.useSkv ? TofArTofManager.streamPlayerSkvKey : TofArTofManager.streamPlayerKey);
                    this.streamPlay?.RegisterFrameCallback(this.StreamCallBack);
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

        private void CheckPlaybackFile(string path, out bool isSymlink, out bool isSkv)
        {
            isSymlink = false;

            isSkv = false;

            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                var attribs = System.IO.File.GetAttributes(path);
                bool isDirectory = attribs.HasFlag(System.IO.FileAttributes.Directory);

                if (isDirectory && !Utils.DirectoryContainsNoSymlinks(path))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"path {path} contained symlink");
                    return;
                }
                else if (Utils.PathIsSymlink(path))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"path {path} is symlink");
                    return;
                }

                isSkv = !isDirectory && new System.IO.FileInfo(path).Extension == ".skv";
            }
        }

        /// <summary>
        /// 指定されたパス内の録画ファイルの再生を開始する
        /// </summary>
        /// <param name="path">再生する録画ファイルを含むディレクトリのパス</param>
        public void StartPlayback(string path)
        {

            if (this.IsPlaying)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Already in playback mode. Needs to call StopPlayback() first.");
                return;
            }
            bool isSymlink;
            CheckPlaybackFile(path, out useSkv, out isSymlink);
            if (isSymlink)
            {
                return;
            }

            if (Instance.useSkv != useSkv && Instance.streamPlay != null)
            {
                Instance.streamPlay.Dispose();
                Instance.streamPlay = null;
            }
            Instance.useSkv = useSkv;

            int tryCount = 3;

            this.ProcessTexture = true;
            var play = new PlayProperty();
            play.TargetPath = path;
            play.StartOffset = 0;
            play.Count = 0;
            play.Speed = PlaySpeed.BasedOnFramerate;
            play.Mode.Repeat = true;

            // When using DebugServer, try at least two times, since it cannot get properties at initial start
            for (int i = 0; i < tryCount; i++)
            {
                Stream stream = Instance.StreamPlay;

                stream.SetProperty<PlayProperty>(play);

                int width;
                int height;

                try
                {
                    // get width and height
                    var imgProperty = stream.GetProperty<ImageProperty>();

                    width = imgProperty.Width;
                    height = imgProperty.Height;
                }
                catch (ApiException) // Fails for first time when using DebugServer
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Start playback failed. Trying again ");

                    Instance.streamPlay.Dispose();
                    Instance.streamPlay = null;

                    continue;
                }

                Instance.IsPlaying = true;

                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Tof: {width}x{height}");

                CreateTofTextures(width, height);

                stream.Start();
                if (stream.IsStarted)
                {
                    Instance.IsPlaying = true;
                    if (OnStreamStarted != null)
                    {
                        OnStreamStarted(this, this.DepthTexture, this.ConfidenceTexture, this.PointCloudData);
                    }

                    if (autoloadCalibration && !useSkv)
                    {
                        LoadSettings();
                    }
                    this.lastTimeFrameRecieved = this.lastUpdateTime;
                }

                break;
            }


        }

        /// <summary>
        /// 録画ファイルの再生を停止する
        /// </summary>
        public void StopPlayback()
        {
            if (Instance.streamPlay == null || !Instance.streamPlay.IsStarted)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Currently not in playback mode.");
                return;
            }

            Instance.streamPlay.Stop();

            Instance.IsPlaying = false;

            if (OnStreamStopped != null)
            {
                OnStreamStopped(this);
            }


        }

        #endregion
    }
}

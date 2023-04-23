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
using System;
using System.Collections;
using System.Runtime.InteropServices;
using MessagePack;
using SensCord;
using UnityEngine;
using TofAr.V0.Slam;
using TofAr.V0.Tof;

namespace TofAr.V0.Body
{
    /// <summary>
    /// TofAr Bodyコンポーネントとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>Bodyデータの取得</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>Body推定結果通知</description></item>
    /// <item><description>録画ファイルの再生</description></item>
    /// </list>
    /// </summary>
    public class TofArBodyManager : Singleton<TofArBodyManager>, IDisposable, IStreamStoppable, IDependManager
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
        public const string StreamKey = "tofar_body_camera2_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にBodyデータのストリームを開始する
        /// </summary>
        public bool autoStart = false;

        private bool isUnPaused = true;
        private bool streamOpenErrorOccured = false;

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
#if UNITY_EDITOR
                        try
                        {
#endif
                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Trying to open body stream");
                            this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArBodyManager.StreamKey);
                            this.Stream.RegisterFrameCallback(StreamCallBack);

                            if (autoSetRuntimeMode && !runtimeModesApplied)
                            {
                                runtimeModesApplied = true;
                                SetDefaultRuntimeMode();
                            }
                            RuntimeMode = runtimeMode;

                            var recognizeConfigProperty = this.stream.GetProperty<SV2.RecognizeConfigProperty>();
                            recognizeConfigProperty.noiseReductionLevel = noiseReductionLevel;
                            this.SetProperty<SV2.RecognizeConfigProperty>(recognizeConfigProperty);

                            TofArTofManager.Instance?.AddManagerDependency(this);
#if UNITY_EDITOR
                        }
                        catch (ApiException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                            this.streamOpenErrorOccured = true;
                        }
#endif
                        try
                        {
                            this.stream.SetProperty(new DetectorTypeProperty { detectorType = detectionType });
                        }
                        catch (ApiException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
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
        /// 最新のBodyデータ
        /// </summary>
        public BodyData BodyData { get; private set; }

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
        /// ストリーミング終了時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        public delegate void StreamStoppedEventHandler(object sender);

        /// <summary>
        /// ストリーミング終了通知
        /// </summary>
        public static event StreamStoppedEventHandler OnStreamStopped;

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

        /// <summary>
        /// Body推定完了時デリゲート
        /// </summary>
        /// <param name="bodyResults">Body推定結果</param>
        public delegate void OnBodyPoseEstimatedHandler(BodyResults bodyResults);

        /// <summary>
        /// Body推定結果通知
        /// </summary>
        public static event OnBodyPoseEstimatedHandler OnBodyPoseEstimated;

        private Pose slamPose = new Pose();

        private ResultHandlerProperty ResultHandler { get; set; } = null;
        private bool waitForTof = false;
        private bool checkForTof = true;

        /// <summary>
        /// Body Detection type
        /// automatically sets the property
        /// </summary>
        [SerializeField]
        private BodyPoseDetectorType detectionType = BodyPoseDetectorType.Internal_SV2;

        /// <summary>
        /// BodyPose認識種別
        /// </summary>  
        public BodyPoseDetectorType DetectorType
        {
            get
            {
                return detectionType;
            }
            set
            {
                detectionType = value;
                try
                {
                    SetProperty(new DetectorTypeProperty { detectorType = value });
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                }
            }
        }

        
        [SerializeField]
        private SV2.NoiseReductionLevel noiseReductionLevel = SV2.NoiseReductionLevel.Low;
        
        /// <summary>
        /// スムージングモード
        /// </summary>
        public SV2.NoiseReductionLevel NoiseReductor
        {
            get
            {
                return this.noiseReductionLevel;
            }
            set
            {
                this.noiseReductionLevel = value;
            }
        }

        [System.Serializable]
        private class DeviceAttributesJson
        {
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string defaultBodyRuntimeMode = "";
        }

        private bool runtimeModesApplied = false;

        /// <summary>
        /// 実行モードの自動設定ON/OFF
        /// </summary>
        [SerializeField]
        private bool autoSetRuntimeMode = true;

        /// <summary>
        /// 実行モード
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private SV2.RuntimeMode runtimeMode = SV2.RuntimeMode.Gpu;

        /// <summary>
        /// 実行モードの指定
        /// </summary>
        public SV2.RuntimeMode RuntimeMode
        {
            get
            {

                if (autoSetRuntimeMode && !runtimeModesApplied)
                {
                    runtimeModesApplied = true;
                    SetDefaultRuntimeMode();
                }

                return runtimeMode;
            }
            set
            {
                if (Array.FindIndex(this.SupportedRuntimeModes, x => x == value) < 0)
                {
                    throw new NotSupportedException($"RuntimeMode {value} not supported on this platform");
                }
                this.runtimeMode = value;
                if (stream != null)
                {
                    var recognizeConfigProperty = stream.GetProperty<SV2.RecognizeConfigProperty>();
                    recognizeConfigProperty.runtimeMode = this.runtimeMode;
                    stream.SetProperty(recognizeConfigProperty);
                }
            }
        }

        /// <summary>
        /// 実行モードの自動設定ON/OFF
        /// </summary>
        public bool RuntimeModeAutoSet
        {
            get { return autoSetRuntimeMode; }
            set
            {
                this.autoSetRuntimeMode = value;
            }
        }

        /// <summary>
        /// 実行モードの取得
        /// </summary>
        /// <param name="mode">モード名称</param>
        /// <returns>実行モード</returns>
        private SV2.RuntimeMode GetRuntimeMode(string mode)
        {
            if (mode.Equals("GPU"))
            {
                return SV2.RuntimeMode.Gpu;
            }
            if (mode.Equals("COREML"))
            {
                return SV2.RuntimeMode.CoreML;
            }

            return SV2.RuntimeMode.Cpu;
        }

        /// <summary>
        /// サポートする実行モードリスト
        /// </summary>
        public SV2.RuntimeMode[] SupportedRuntimeModes
        {
            get
            {
                if (TofArManager.Instance.UsingIos)
                {
                    return new SV2.RuntimeMode[] {
                        SV2.RuntimeMode.Cpu,
                        SV2.RuntimeMode.Gpu,
                        SV2.RuntimeMode.CoreML
                    };
                }
                else
                {
                    return new SV2.RuntimeMode[] {
                        SV2.RuntimeMode.Cpu,
                        SV2.RuntimeMode.Gpu};
                }
            }
        }

        private void SetDefaultRuntimeMode()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
            var deviceAttribs = JsonUtility.FromJson<DeviceAttributesJson>(devcap.TrimmedDeviceAttributesString);
            if (deviceAttribs != null)
            {
                runtimeMode = GetRuntimeMode(deviceAttribs.defaultBodyRuntimeMode);
            }
        }


        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate bool BodyResultHandlerDelegate(IntPtr bodyResultsPointer, UInt64 bufferSize);

        private IEnumerator Start()
        {
            TofArManager.Instance.AddSubManager(this);
            try
            {
                SetProperty(new DetectorTypeProperty { detectorType = detectionType });
            }
            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
            }
            if (this.autoStart)
            {
                var externalTofSource = Utils.FindFirstGameObjectThatImplements<IExternalStreamSource>(true);
                //wait for the ToF to start
                if (Tof.TofArTofManager.Instance != null && (Tof.TofArTofManager.Instance.autoStart || externalTofSource != null) && detectionType == BodyPoseDetectorType.Internal_SV2)
                {
                    while (!TofAr.V0.Tof.TofArTofManager.Instance.IsStreamActive)
                    {
                        yield return null;
                    }
                }

                TofArBodyManager.Instance.StartStream();
            }
        }


        private void OnEnable()
        {
            TofArManager.OnScreenOrientationUpdated += OnScreenOrientationChanged;

            var orientationsProperty = TofArManager.Instance.GetProperty<DeviceOrientationsProperty>();
            if (orientationsProperty != null)
            {
                OnScreenOrientationChanged(ScreenOrientation.LandscapeLeft, orientationsProperty.screenOrientation);
            }

            TofArSlamManager.OnFrameArrived += OnSlamFramArrived;
            TofAr.V0.Tof.TofArTofManager.OnStreamStarted += TofOnStreamStarted;
        }

        private void OnDisable()
        {
            TofArManager.OnScreenOrientationUpdated -= OnScreenOrientationChanged;
            TofArSlamManager.OnFrameArrived -= OnSlamFramArrived;
            TofAr.V0.Tof.TofArTofManager.OnStreamStarted -= TofOnStreamStarted;
        }

        private void TofOnStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, Tof.PointCloudData pointCloudData)
        {
            // restart stream, if body was already running
            if (this.IsStreamActive)
            {
                this.checkForTof = false;
                StopStream();
                StartStream();
            }
            else if (this.waitForTof)
            {
                this.checkForTof = false;
                this.waitForTof = false;
                StartStream();

            }
        }


        private void OnSlamFramArrived(object sender)
        {
            var slamData = TofArSlamManager.Instance?.SlamData?.Data;
            if (slamData != null)
            {
                slamPose.position = slamData.position;
                slamPose.rotation = slamData.rotation;
            }
        }

        private void OnScreenOrientationChanged(ScreenOrientation prev, ScreenOrientation curr)
        {
            var orProp = new SV2.CameraOrientationProperty();
            switch (curr)
            {
                case ScreenOrientation.Portrait:
                    orProp.cameraOrientation = SV2.CameraOrientation.Portrait;//Portrait
                    break;
                case ScreenOrientation.PortraitUpsideDown:
                    orProp.cameraOrientation = SV2.CameraOrientation.PortraitUpsideDown;//PortraitUpsideDown
                    break;
                case ScreenOrientation.LandscapeLeft:
                    orProp.cameraOrientation = SV2.CameraOrientation.LandscapeLeft;//LandscapeLeft
                    break;
                case ScreenOrientation.LandscapeRight:
                    orProp.cameraOrientation = SV2.CameraOrientation.LandscapeRight;//LandscapeRight
                    break;
                default:
                    orProp.cameraOrientation = SV2.CameraOrientation.Portrait;//Portrait
                    break;
            }

            SetProperty(orProp);
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArBodyManager.Dispose()");
            TofArTofManager.Instance?.RemoveManagerDependency(this);
            TofArManager.OnScreenOrientationUpdated -= OnScreenOrientationChanged;

            TofArManager.Instance?.RemoveSubManager(this);

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
                    if (!TofArBodyManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    var frame = stream.GetFrame();

                    try
                    {
                        var channel = frame.Channels[(int)ChannelIds.Body];
                        var rawData = channel.GetRawData();

                        instance.BodyData = new BodyData(rawData);
                        //for SV1 and SV2 set the source pose to be the camera pose
                        //ARKit and AREngine use a world reference frame
                        if (instance.BodyData.Data.frameDataSource == FrameDataSource.TofArSV2BodySkeleton)
                        {
                            int rotation = TofArManager.Instance.GetDeviceOrientation();
                            foreach (var b in instance.BodyData.Data.results)
                            {
                                b.pose.position = instance.slamPose.position.GetVector3();
                                b.pose.rotation = instance.slamPose.rotation.GetQuaternion() * Quaternion.Euler(0, 0, rotation);
                            }
                        }

                        if (OnBodyPoseEstimated != null)
                        {
                            OnBodyPoseEstimated(instance.BodyData.Data);
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
                    instance.frameCount++;
                }
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            /*catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            catch (NullReferenceException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            catch (IndexOutOfRangeException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }*/
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        public void StartStream()
        {
            if (!this.IsStreamActive && this.isUnPaused)
            {
                if (Array.FindIndex(this.SupportedRuntimeModes, x => x == this.runtimeMode) < 0)
                {
                    throw new NotSupportedException($"Can't start body stream. RuntimeMode {this.runtimeMode} not supported on this platform");
                }

                if (this.streamPlay != null && this.streamPlay.IsStarted)
                {
                    this.StopPlayback();
                }

                if (this.checkForTof)
                {
                    bool isTofStarting = Tof.TofArTofManager.Instance?.IsTofStarting == true;
                    if (isTofStarting)
                    {
                        this.waitForTof = true;
                        return;
                    }
                }
                this.checkForTof = true;
                this.ResultHandler = null;
                if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
                {
                    this.ResultHandler = this.GetProperty<ResultHandlerProperty>();
                }
                this.Stream?.Start();

                if (this.startingPlayback)
                {
                    this.isPlaying = true;
                }

                if (OnStreamStarted != null)
                {
                    OnStreamStarted(this);
                }
            }
        }

        /// <summary>
        /// 依存するManagerから要求されたストリーミング再スタートを開始する
        /// </summary>
        /// <param name="requestSource">要求元</param>
        public void RestartStreamByDependManager(object requestSource)
        {
            if (requestSource is IDependedManager)
            {
                switch (this.DetectorType)
                {
                    case BodyPoseDetectorType.Internal_SV2:
                    case BodyPoseDetectorType.External:
                        this.StopStream(requestSource);
                        break;
                }
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
                switch (this.DetectorType)
                {
                    case BodyPoseDetectorType.Internal_SV2:
                    case BodyPoseDetectorType.External:
                        this.StartStream();
                        break;
                }
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
                this.ResultHandler = null;
                this.Stream?.Stop();
                if (OnStreamStopped != null)
                {
                    OnStreamStopped((sender == null) ? this : sender);
                }
            }
            this.streamOpenErrorOccured = false;
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
                var bufferSize = 1024;
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
                int bufferSize = 1024;
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(value.Key, ref value, bufferSize);
                return value;
            }
            return null;
        }

        /// <summary>
        /// コンポーネントプロパティを設定する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (this.isUnPaused)
            {
                this.Stream?.SetProperty<T>(value);
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
            return null;
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
                this.stream.Dispose();
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

        private void Update()
        {
            this.fromFpsMeasured += Time.unscaledDeltaTime;
            if (this.fromFpsMeasured >= 1.0f)
            {
                this.FrameRate = this.frameCount / this.fromFpsMeasured;
                this.fromFpsMeasured = 0;
                this.frameCount = 0;
            }
        }

        /// <summary>
        /// *TODO+ B
        /// 推定結果を設定する
        /// </summary>
        /// <param name="result">*TODO+ C 推定結果</param>
        /// <param name="frameDataSource">*TODO+ C SkeletonのSource</param>
        public void SetEstimatedResult(BodyResult result, FrameDataSource frameDataSource)
        {
            this.SetEstimatedResults(new BodyResults() { results = new BodyResult[] { result } }, frameDataSource);
        }

        /// <summary>
        /// *TODO+ B
        /// 複数の推定結果を設定する
        /// </summary>
        /// <param name="results">*TODO+ C 推定結果のリスト</param>
        /// <param name="frameDataSource">*TODO+ C SkeletonのSource</param>
        public void SetEstimatedResults(BodyResults results, FrameDataSource frameDataSource)
        {
            if (detectionType != BodyPoseDetectorType.External)
            {
                throw new System.InvalidOperationException("Results can only be set if DetectionType is External");
            }
            results.frameDataSource = frameDataSource;
            //for AREngine and SV2 set the source pose to be the camera pose
            //ARKit uses a world reference frame
#pragma warning disable CS0618 // SV1 Body recognition will remove in future version
            if (frameDataSource == FrameDataSource.TofArSV1BodySkeleton)
#pragma warning restore CS0618
            {
                // int rotation = -TofArManager.Instance.GetDeviceOrientation();
                foreach (var b in results.results)
                {
                    b.pose.position = slamPose.position.GetVector3();
                    b.pose.rotation = slamPose.rotation.GetQuaternion();// * Quaternion.Euler(0, 0, rotation);
                }
            }
            if (frameDataSource == FrameDataSource.AREngineBodySkeleton)
            {
                foreach (var b in results.results)
                {
                    b.pose.position = slamPose.position.GetVector3();
                    b.pose.rotation = slamPose.rotation.GetQuaternion();
                }
            }

            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                if ((this.ResultHandler != null) && (this.ResultHandler.handler != 0))
                {
                    var handler = Marshal.GetDelegateForFunctionPointer<BodyResultHandlerDelegate>(new IntPtr(this.ResultHandler.handler));

                    var bytes = MessagePackSerializer.Serialize<BodyResults>(results);
                    var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    var handlePtr = handle.AddrOfPinnedObject();
                    var bufferSize = new UIntPtr((uint)bytes.Length);
                    try
                    {
                        handler(handlePtr, (UInt64)bytes.Length);
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
            }
            else
            {
                if (this.IsStreamActive)
                {
                    Instance.BodyData = new BodyData(results);

                    if (OnBodyPoseEstimated != null)
                    {
                        OnBodyPoseEstimated(Instance.BodyData.Data);
                    }

                    this.frameCount++;
                }
            }
        }

        #region PLAYBACK

        /// <summary>
        /// *TODO+ B
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_body_stream";

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
                    //make sure the TofArManager stream itself is opened first
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArBodyManager.streamPlayerKey);

                    this.streamPlay?.RegisterFrameCallback(TofArBodyManager.StreamCallBack);
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
        public bool IsPlaying
        {
            get
            {
                return isPlaying || isPlayingCsv;
            }
        }

        private bool isPlaying = false;
        private bool isPlayingCsv = false;
        private bool isPlayingWithTof = false;
        private bool startingPlayback = false;

        /// <summary>
        /// 録画ファイル再生中のToFストリームをソースとして再生を開始する
        /// </summary>
        public void StartPlayback()
        {
            if (Instance.IsPlaying || Instance.isPlayingCsv)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Already in playback mode. Needs to call StopPlayback() first.");
                return;
            }
            if (Instance.IsStreamActive)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Stream currently running. Needs to stop first.");
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
        /// 指定されたパス内の録画ファイルの再生を開始する
        /// </summary>
        /// <param name="path">再生する録画ファイルを含むディレクトリのパス</param>
        public void StartPlayback(string path)
        {
            if (Instance.IsPlaying || Instance.isPlayingCsv)
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

            string extension = new System.IO.FileInfo(path).Extension;
            if (extension == ".csv")
            {
                StartCoroutine(PlaybackCsv(path));
                Instance.isPlayingCsv = true;
                return;
            }

            var play = new PlayProperty();
            play.TargetPath = path;
            play.StartOffset = 0;
            play.Count = 0;
            play.Speed = PlaySpeed.BasedOnFramerate;
            play.Mode.Repeat = true;

            var stream = Instance.StreamPlay;

            stream.SetProperty<PlayProperty>(play);
            stream.Start();

            if (stream.IsStarted)
            {
                Instance.isPlaying = true;

                if (OnStreamStarted != null)
                {
                    OnStreamStarted(this);
                }
            }
        }



        private string ReadLine(System.IO.StreamReader reader)
        {
            const int MAX_STR_LEN = 1024;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            int cInt;
            while ((cInt = reader.Read()) != -1)
            {
                char c = (char)cInt;
                if (c == '\n' || sb.Length >= MAX_STR_LEN)
                {
                    break;
                }
                sb.Append(c);
            }

            return sb.ToString();
        }

        private IEnumerator PlaybackCsv(string csvPath)
        {
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default && !Utils.DirectoryContainsNoSymlinks(csvPath))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"path {csvPath} contained symlink");
                yield break;
            }

            using (System.IO.StreamReader reader = new System.IO.StreamReader(csvPath))
            {
                this.isPlayingCsv = true;
                ReadLine(reader); // HEADER

                const int offset = 3;

                int numJoints = Enum.GetValues(typeof(JointIndices)).Length - 1;

                //OnStreamStarted(this);
                yield return null;

                while (this.isPlayingCsv)
                {

                    try
                    {
                        string line = ReadLine(reader);
                        if (line != null)
                        {
                            string[] tokens = line.Split(',');
                            if (tokens.Length > 2)
                            {
                                BodyResults results = new BodyResults()
                                {
                                    results = new BodyResult[1]
                                };

                                results.results[0] = new BodyResult();
                                results.results[0].trackingState = TrackingState.Tracking;

#pragma warning disable CS0618 // SV1 Body recognition will remove in future version
                                SV1.BodyShot bodyShot = (SV1.BodyShot)Enum.Parse(typeof(SV1.BodyShot), tokens[2]);
                                results.results[0].bodyShot = bodyShot;
#pragma warning restore CS0618

                                results.results[0].joints = new HumanBodyJoint[numJoints];

                                for (int i = 0; i < numJoints; i++)
                                {
                                    int idX = offset + i * 3 + 0;
                                    int idY = idX + 1;
                                    int idZ = idX + 2;

                                    HumanBodyJoint bodyJoint = new HumanBodyJoint();
                                    TofArVector3 joint;

                                    if (idX < tokens.Length - 1)
                                    {
                                        float.TryParse(tokens[idX], out float jX);
                                        float.TryParse(tokens[idY], out float jY);
                                        float.TryParse(tokens[idZ], out float jZ);

                                        joint = new TofArVector3(jX, jY, jZ);
                                        bodyJoint.tracked = jZ > 0;
                                        bodyJoint.anchorPose.position = joint;
                                        bodyJoint.index = (int)i;
                                        if (parentJoints.ContainsKey((JointIndices)i))
                                        {
                                            bodyJoint.parentIndex = (int)parentJoints[(JointIndices)i];
                                        }
                                    }

                                    results.results[0].joints[i] = bodyJoint;
                                }

                                Instance.BodyData = new BodyData(results);
                            }
                        }
                    }
                    catch (ArgumentException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to parse. {e.Message}");
                    }

                    if (reader.EndOfStream)
                    {
                        reader.DiscardBufferedData();
                        reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                        ReadLine(reader); // HEADER
                    }

                    yield return new WaitForSecondsRealtime(1f / 60f);
                }
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
            else if (Instance.isPlayingCsv)
            {
                Instance.isPlayingCsv = false;
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

            Instance.isPlaying = false;
        }

        private System.Collections.Generic.Dictionary<JointIndices, JointIndices> parentJoints = new System.Collections.Generic.Dictionary<JointIndices, JointIndices>()
        {
            { JointIndices.Spine4, JointIndices.Spine1 },
            { JointIndices.Neck1, JointIndices.Spine4 },
            { JointIndices.RightShoulder1, JointIndices.Spine4 },
            { JointIndices.RightArm, JointIndices.RightShoulder1 },
            { JointIndices.RightForearm, JointIndices.RightArm},
            { JointIndices.LeftShoulder1, JointIndices.Spine4 },
            { JointIndices.LeftArm, JointIndices.LeftShoulder1 },
            { JointIndices.LeftForearm, JointIndices.LeftArm},
            { JointIndices.RightUpLeg, JointIndices.Spine1},
            { JointIndices.RightLeg, JointIndices.RightUpLeg},
            { JointIndices.RightFoot, JointIndices.RightLeg},
            { JointIndices.LeftUpLeg, JointIndices.Spine1},
            { JointIndices.LeftLeg, JointIndices.LeftUpLeg},
            { JointIndices.LeftFoot, JointIndices.LeftLeg},
            { JointIndices.Head, JointIndices.Neck1},
            { JointIndices.Nose, JointIndices.Head},
            { JointIndices.RightEye, JointIndices.Nose},
            { JointIndices.LeftEye, JointIndices.Nose},
        };

        #endregion
    }
}

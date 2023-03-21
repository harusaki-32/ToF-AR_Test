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
using SensCord;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;

namespace TofAr.V0.Color
{
    /// <summary>
    /// Colorカメラとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>Colorカメラとの接続管理</description></item>
    /// <item><description>Colorデータの取得</description></item>
    /// <item><description>ColorデータのTexture2D変換</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>録画ファイルの再生</description></item>
    /// </list>
    /// </summary>
    public class TofArColorManager : Singleton<TofArColorManager>, IStreamStoppable, IDependedManager
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
        public const string StreamKey = "tofar_color_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にColorデータのストリームを開始する
        /// </summary>
        public bool autoStart = false;

        [SerializeField]
        [Tooltip("isProcessTexture is deprecated, please use processTexture instead")]
        [Obsolete("isProcessTexture is deprecated, please use processTexture instead")]
        private bool isProcessTexture;
        [SerializeField]
        private bool processTexture = true;
        [SerializeField]
        private ColorFormat colorFormat;

        [SerializeField]
        private bool autoFocus = true;
        [SerializeField]
        private float focusDistance = 0;
        [SerializeField]
        private bool autoExposure = true;
        [SerializeField]
        private long exposureTime = 0;
        [SerializeField]
        private long frameDuration = 0;
        [SerializeField]
        private int sensitivity = 0;
        [SerializeField]
        private FlashMode flashMode = FlashMode.Off;
        [SerializeField]
        private bool autoWhiteBalance = true;
        [SerializeField]
        private WhiteBalanceMode whiteBalanceMode = WhiteBalanceMode.Off;
        [SerializeField]
        private bool lensStabilization = false;

        private SynchronizationContext context;

        private YUVLayout yuvLayout;
        private NativeArray<byte> ydata;
        private NativeArray<byte> uvdata;
        private NativeArray<byte> colorDataBuffer;
        private static Dictionary<string, TextureFormat> colorTextureFormats = new Dictionary<string, TextureFormat>()
        {
            { ColorRawDataType.YUV420, TextureFormat.YUY2 },
            { ColorRawDataType.BGRA, TextureFormat.BGRA32 },
            { ColorRawDataType.BGR, TextureFormat.RGB24 },
            { ColorRawDataType.RGB, TextureFormat.RGB24 },
            { ColorRawDataType.RGBA, TextureFormat.RGBA32 },
        };

        private bool isUnPaused = true;
        private bool streamOpenErrorOccured = false;

        [HideInInspector]
        [SerializeField]
        [Range(0, 10)]
        private int streamDelay;

        /// <summary>
        /// フレーム送出遅延数
        /// </summary>
        public int StreamDelay
        {
            get => streamDelay;
            set
            {
                streamDelay = value;
                SetProperty(new DelayProperty { numFramesDelay = value });
                V0.TofArManager.Logger.WriteLog(V0.LogLevel.Debug, $"color delay set to {value}");
            }
        }

        private bool lensFacingChanged = false;
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
                        //make sure the TofArManager stream itself is opened first
                        this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArColorManager.StreamKey);
                        this.Stream?.RegisterFrameCallback(this.StreamCallBack);

                        this.stream?.SetProperty(new FormatConvertProperty()
                        {
                            format = this.colorFormat
                        });

                        this.context = SynchronizationContext.Current;

                        if (useFrontCameraAsDefault)
                        {
                            var defaultConfig = this.stream?.GetProperty<DefaultResolutionProperty>();
                            if (defaultConfig != null && defaultConfig.lensFacing != (int)LensFacing.Front)
                            {
                                var defaultFrontConfig = GetDefaultFrontCameraConfig();
                                if (defaultFrontConfig != null)
                                {
                                    var frontResolutionProperty = new ResolutionProperty()
                                    {
                                        cameraId = defaultFrontConfig.cameraId,
                                        width = defaultFrontConfig.width,
                                        height = defaultFrontConfig.height,
                                        lensFacing = defaultFrontConfig.lensFacing
                                    };

                                    this.stream?.SetProperty(frontResolutionProperty);
                                    this.lensFacingChanged = true;
                                }
                            }
                        }
#if UNITY_EDITOR
                        }
                        catch (ApiException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                            this.streamOpenErrorOccured = true;
                        }
#endif
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
        /// Colorデータ
        /// </summary>
        public ColorData ColorData { get; private set; }

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
        /// Colorデータを変換したTexture2D
        /// </summary>
        public Texture2D ColorTexture { get; private set; }
        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        public Texture2D YTexture { get; private set; }
        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        public Texture2D UVTexture { get; private set; }

        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        public int CurrentYWidth { get; private set; }
        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        public int OriginalYWidth { get; private set; }
        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        public int YHeight { get; private set; }
        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        public int UVWidth { get; private set; }
        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        public int UVHeight { get; private set; }

        [SerializeField]
        private float desiredFrameRate = 30f;

        [SerializeField]
        private bool useFrontCameraAsDefault = false;

        [HideInInspector]
        [SerializeField]
        private bool useDefaultStreamDelay = true;

        /// <summary>
        /// 設定希望FPS。設定は次回ストリーミング開始時に有効となる。
        /// </summary>
        public float DesiredFrameRate
        {
            get
            {
                return desiredFrameRate;
            }

            set
            {
                desiredFrameRate = value;

                TofArColorManager.Instance.SetProperty<FrameRateProperty>(new FrameRateProperty() { desiredFrameRate = desiredFrameRate });

            }
        }

        private List<IPreProcessColorData> preProcessors = new List<IPreProcessColorData>();

        /// <summary>
        /// Colorデータ送出前の処理を登録する
        /// </summary>
        /// <param name="preProcessColor">データ処理クラス</param>
        public void RegisterColorPreProcessing(IPreProcessColorData preProcessColor)
        {
            preProcessors.Add(preProcessColor);
        }
        /// <summary>
        /// 登録されたColorデータ送出前の処理を登録解除する
        /// </summary>
        /// <param name="preProcessColor">データ処理クラス</param>
        public void UnregisterColorPreProcessing(IPreProcessColorData preProcessColor)
        {
            preProcessors.Remove(preProcessColor);
        }

        /// <summary>
        /// ストリーミング開始後デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="ColorTexture">Colorテクスチャー</param>
        public delegate void StreamStartedEventHandler(object sender, Texture2D ColorTexture);

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
        /// ストリーミング終了後デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
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
        /// <param name="sender">送信元オブジェクト</param>
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

        private IList<IDependManager> dependedManagers = null;

        /// <summary>
        /// 依存Managerを追加する
        /// </summary>
        /// <param name="dependManager">依存Manager</param>
        public void AddManagerDependency(IDependManager dependManager)
        {
            // initialize stream first if possible
            if (this.Stream == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Color stream couldn't be opened. This might lead to unwanted results.");
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
        /// 実測FPS
        /// </summary>
        public float FrameRate { get; private set; }
        private int frameCount = 0;
        private float fromFpsMeasured = 0f;

        /// <summary>
        /// *TODO+ B json内部処理用
        /// Color延長の設定
        /// </summary>
        [System.Serializable]
        public class deviceCameraSettingsJson
        {
            public string id = null;
            public int colorDelay = 0;
            public int colorDelayAREngine = 0;
            public int colorDelayAREngineBody = 0;
            public int colorDelayARFoundation = 0;
            public int colorDelayARFoundationARImage = 0;
        }

        /// <summary>
        /// *TODO+ B json内部処理用
        /// デバイス設定のリスト
        /// </summary>
        [System.Serializable]
        private class deviceAttributesJson
        {
            /// <summary>
            /// *TODO+ B
            /// カメラ開始の順番
            /// </summary>
            public int cameraStartOrder = 0;

            /// <summary>
            /// デバイス設定のリスト
            /// </summary>
            public deviceCameraSettingsJson[] cameraSettings = null;

            public string fixedFrontColorCameraId = null;
        }

        /// <summary>
        /// 解像度変更時デリゲート
        /// </summary>
        /// <param name="configs">解像度プロパティ</param>
        public delegate void AvailableResolutionsChanged(AvailableResolutionsProperty configs);
        /// <summary>
        /// 解像度の変更通知
        /// </summary>
        public static event AvailableResolutionsChanged OnAvailableResolutionsChanged;

        /// <summary>
        /// ストリーミング起動中のステータス
        /// </summary>
        public bool IsColorStarting { get; private set; }

        /// <summary>
        /// ストリーミングデフォルト遅延設定用イベント
        /// </summary>
        [System.Serializable]
        public class SetDefaultStreamDelayEventHandler : UnityEvent<int> { }

        /// <summary>
        /// ストリーミングデフォルト遅延設定通知
        /// </summary>
        public SetDefaultStreamDelayEventHandler SetDefaultStreamDelay;

        private void Start()
        {
            // ObsoleteとなったisProcessXX の値を名前を修正したフィールドに移行する
            // この処理はObsoleteフィールドを削除するまでの一時的なものである
#pragma warning disable CS0618 // Type or member is obsolete
            this.ProcessTexture = this.isProcessTexture;
#pragma warning restore CS0618 // Type or member is obsolete

            TofArManager.Instance.ColorAutoStart = this.autoStart;

            TofArManager.Instance.AddSubManager(this);

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                new WebCamTexture();
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
            if (this.autoStart)
            {
                this.StartCoroutine(this.StartProcess());
            }
        }


        private IEnumerator StartProcess()
        {
            yield return null;
            var defResolution = Instance.GetProperty<DefaultResolutionProperty>();
            if (!this.lensFacingChanged)
            {
                Instance.SetProperty(new ResolutionProperty { cameraId = defResolution.cameraId, width = defResolution.width, height = defResolution.height, lensFacing = defResolution.lensFacing });
            }
            Instance.StartStream(true);
        }

        /// <summary>
        /// オブジェクト破棄
        /// </summary>
        public void Dispose()
        {
            this.dependedManagers?.Clear();
            this.dependedManagers = null;

            TofArManager.Instance?.RemoveSubManager(this);
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArColorManager.Dispose()");
            OnStreamStarted = null;
            OnStreamStopped = null;
            OnFrameArrived = null;
            if (colorDataBuffer.IsCreated)
            {
                colorDataBuffer.Dispose();
            }

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
                this.IsStreamPausing = true;
            }
            else if (!pause && this.IsStreamPausing)
            {
                this.StartStream(this.ProcessTexture);
                this.IsStreamPausing = false;
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

            lock (arrayLock)
            {
                if (nv21Updated)
                {
                    nv21Updated = false;
                    UpdateNV21Textures();
                }
                if (rgbUpdated)
                {
                    rgbUpdated = false;
                    UpdateRgbTextures();
                }
            }
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="configuration">ストリーミングで使用する解像度</param>
        /// <param name="isProcessTexture">trueの場合、カメラデータのTexture2D変換処理を実施する</param>
        public void StartStream(ResolutionProperty configuration, bool isProcessTexture = true)
        {
            StartStream(configuration, null, isProcessTexture);
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="configuration">ストリーミングで使用する解像度</param>
        /// <param name="metadataProperties">ストリーミングで使用するメタデータ</param>
        /// <param name="isProcessTexture">trueの場合、カメラデータのTexture2D変換処理を実施する</param>
        public void StartStream(ResolutionProperty configuration, List<IColorMetadataProperty> metadataProperties, bool isProcessTexture = true)
        {
            StartCoroutine(StartStreamCoroutine(configuration, metadataProperties, isProcessTexture));
        }


        /// <summary>
        /// ストリームの遅延設定を設定ファイルから行う
        /// </summary>
        public void SetStreamDelayToDefault(IExternalColorStream externalSource = null)
        {
            try
            {
                var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
                if (devcap != null && !String.IsNullOrEmpty(devcap.TrimmedDeviceAttributesString))
                {
                    var defaultNN = JsonUtility.FromJson<deviceAttributesJson>(devcap.TrimmedDeviceAttributesString);

                    var cameraConfig = this.GetProperty<ResolutionProperty>();
                    string id = cameraConfig.cameraId == "" ? "0" : cameraConfig.cameraId;

                    foreach (var setting in defaultNN.cameraSettings)
                    {
                        if (setting.id.Equals(id))
                        {
                            if (externalSource != null)
                            {
                                this.StreamDelay = externalSource.GetStreamDelay(setting);
                            }
                            else
                            {
                                this.StreamDelay = setting.colorDelay;
                            }


                            if (SetDefaultStreamDelay != null)
                            {
                                SetDefaultStreamDelay.Invoke(this.streamDelay);
                            }

                            return;
                        }
                    }
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Could not find settings for id {id}");

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
                    catch (NullReferenceException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
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
                    catch (NullReferenceException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                    }
                    catch (SensCord.ApiException e)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                    }
                }
            }
        }

        private IEnumerator StartStreamCoroutine(ResolutionProperty configuration, List<IColorMetadataProperty> metadataProperties, bool processTexture = true)
        {
            if (!this.IsStreamActive && this.isUnPaused && !IsColorStarting)
            {
                IsColorStarting = true;
                var autoStoppedDependManagers = new List<IDependManager>();
                try
                {
                    // Stop dependend managers before start stream
                    if (this?.dependedManagers != null)
                    {
                        StopDependManagers(ref autoStoppedDependManagers);
                    }

                    var externalSource = Utils.FindFirstActiveGameObjectThatImplements<IExternalColorStream>();
                    if (externalSource != null)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, $"StartStream to ExternalStreamSource : {externalSource.GetType().Name}");
                        yield return externalSource.WaitForStreamStart();
                        externalSource.StartStream();
                        //don't try to set the color resolution if we are external
                        configuration = null;
                    }
                    if (this.streamPlay != null && this.streamPlay.IsStarted)
                    {
                        this.StopPlayback();
                    }
                    if (useDefaultStreamDelay)
                    {
                        SetStreamDelayToDefault(externalSource);
                    }
                    else
                    {
                        this.StreamDelay = this.streamDelay;
                    }
                    this.ColorData = null;
                    if (configuration != null)
                    {
                        this.pauseCurrentResolution = configuration;
                        bool streamStillStopping = true;

                        for (int tryCount = 5; tryCount > 0 && streamStillStopping; tryCount--)
                        {
                            try
                            {
                                this.SetProperty(configuration);
                                streamStillStopping = false;
                            }
                            catch (SensCord.ApiException)
                            {
                                TofArManager.Logger.WriteLog(LogLevel.Debug, "Color not yet stopped");
                                Stream.Stop();
                                streamStillStopping = true;
                            }
                            if (streamStillStopping)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                        }
                    }
                    this.ProcessTexture = processTexture;
                    var currentConfiguration = Instance.GetProperty<ResolutionProperty>();
                    if (this.ProcessTexture)
                    {
                        SetupTexture(currentConfiguration);
                    }

                    this.CurrentYWidth = currentConfiguration.width;
                    this.OriginalYWidth = currentConfiguration.width;
                    this.YHeight = currentConfiguration.height;

                    this.UVWidth = (currentConfiguration.width / 2) * sizeof(short);
                    this.UVHeight = (currentConfiguration.height / 2);

                    yield return StartInOrder(metadataProperties);

                }
                finally
                {
                    IsColorStarting = false;

                    RestartDependManagers(autoStoppedDependManagers);
                }
            }
        }

        private void SetupTexture(ResolutionProperty currentConfiguration)
        {
            var formatConvertProperty = this.GetProperty<FormatConvertProperty>();
            switch (formatConvertProperty.format)
            {
                case ColorFormat.YUV420:
                    if (this.YTexture == null)
                    {
                        this.YTexture = new Texture2D(currentConfiguration.width, currentConfiguration.height, TextureFormat.Alpha8, false);
                    }
                    else
                    {
#if UNITY_2021_1_OR_NEWER
                        this.YTexture.Reinitialize(currentConfiguration.width, currentConfiguration.height);
#else
                        this.YTexture.Resize(currentConfiguration.width, currentConfiguration.height);
#endif
                    }
                    var blackYData = new byte[YTexture.width * YTexture.height];
                    for (var i = 0; i < blackYData.Length; i++)
                    {
                        blackYData[i] = 16;
                    }
                    this.YTexture.LoadRawTextureData(blackYData);
                    this.YTexture.Apply(false);

                    if (this.UVTexture == null)
                    {
                        this.UVTexture = new Texture2D(currentConfiguration.width / 2, currentConfiguration.height / 2,
                            TextureFormat.RGBA4444, false);
                    }
                    else
                    {
#if UNITY_2021_1_OR_NEWER
                        this.UVTexture.Reinitialize(currentConfiguration.width / 2, currentConfiguration.height / 2);
#else
                        this.UVTexture.Resize(currentConfiguration.width / 2, currentConfiguration.height / 2);
#endif
                    }
                    var blackUVData = new byte[UVTexture.width * UVTexture.height * 2];
                    for (var i = 0; i < blackUVData.Length; i++)
                    {
                        blackUVData[i] = 128;
                    }
                    this.UVTexture.LoadRawTextureData(blackUVData);
                    this.UVTexture.Apply(false);
                    // パディングバイトはあるかどうかまだわからない
                    Shader.SetGlobalFloat("_YUVShader_paddingCutOff", 1.0f);
                    break;
                default:
                    if (colorTextureFormats.ContainsKey(formatConvertProperty.format.ToString()))
                    {
                        if (this.ColorTexture?.format != colorTextureFormats[formatConvertProperty.format.ToString()])
                        {
                            DestroyImmediate(this.ColorTexture);
                            this.ColorTexture = new Texture2D(currentConfiguration.width, currentConfiguration.height, colorTextureFormats[formatConvertProperty.format.ToString()], false);
                        }
                        else
                        {
#if UNITY_2021_1_OR_NEWER
                            this.ColorTexture.Reinitialize(currentConfiguration.width, currentConfiguration.height);
#else
                            this.ColorTexture.Resize(currentConfiguration.width, currentConfiguration.height);
#endif
                        }
                    }
                    break;
            }
        }

        private IEnumerator StartInOrder(List<IColorMetadataProperty> metadataProperties = null)
        {
            try
            {
                if (TofArManager.Instance == null)
                {
                    yield return null;
                }
                CameraStartOrder startOrder = TofArManager.Instance.CameraStartOrder;

                if (startOrder == CameraStartOrder.DepthToColor && (TofArManager.Instance.TofAutoStart || TofArManager.Instance.WasPaused))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Wait for Tof to finish");
                    while (!TofArManager.Instance.TofStarted)
                    {
                        yield return null;
                    }

                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Tof finished");
                }


                this.Stream?.Start();

                try
                {
                    var focusModeProperty = this.GetProperty<FocusModeProperty>();
                    focusModeProperty.autoFocus = this.autoFocus;
                    focusModeProperty.distance = Mathf.Max(focusModeProperty.minDistance, Mathf.Min(focusModeProperty.maxDistance, this.focusDistance));
                    this.SetProperty(focusModeProperty);
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set metadata: " + e.Message);
                }

                try
                {
                    var exposureModeProperty = this.GetProperty<ExposureModeProperty>();
                    exposureModeProperty.autoExposure = this.autoExposure;
                    exposureModeProperty.exposureTime = Math.Max(exposureModeProperty.minExposureTime, Math.Min(exposureModeProperty.maxExposureTime, this.exposureTime));
                    exposureModeProperty.frameDurarion = Math.Max(exposureModeProperty.minFrameDurationForCurrentResolution, Math.Min(exposureModeProperty.maxFrameDuration, this.frameDuration));
                    exposureModeProperty.sensitibity = Math.Max(exposureModeProperty.minSensitivity, Math.Min(exposureModeProperty.maxSensitivity, this.sensitivity));
                    exposureModeProperty.flash = exposureModeProperty.flashAvailable ? this.flashMode : FlashMode.Off;
                    this.SetProperty(exposureModeProperty);
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set metadata: " + e.Message);
                }

                try
                {
                    var whiteBalanceModeProperty = this.GetProperty<WhiteBalanceModeProperty>();
                    whiteBalanceModeProperty.autoWhiteBalance = this.autoWhiteBalance;
                    whiteBalanceModeProperty.mode = this.whiteBalanceMode;
                    this.SetProperty(whiteBalanceModeProperty);
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set metadata: " + e.Message);
                }

                try
                {
                    var lensStabilizationProperty = this.GetProperty<StabilizationProperty>();
                    lensStabilizationProperty.lensStabilization = this.lensStabilization;
                    this.SetProperty(lensStabilizationProperty);
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set metadata: " + e.Message);
                }

                try
                {
                    this.SetProperty(new FrameRateProperty() { desiredFrameRate = this.desiredFrameRate });
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Failed to set metadata: " + e.Message);
                }


                SetMetadata(metadataProperties);
                IsColorStarting = false;

                if (OnStreamStarted != null)
                {
                    OnStreamStarted(this, this.ColorTexture);
                }


                TofArManager.Instance.ColorStarted = true;


                yield return null;
            }
            finally
            {
                IsColorStarting = false;
            }
        }

        private void SetMetadata(List<IColorMetadataProperty> metadataProperties)
        {
            if (metadataProperties != null)
            {
                foreach (var property in metadataProperties)
                {
                    if (property is StabilizationProperty)
                    {
                        this.SetProperty((StabilizationProperty)property);
                    }
                    else if (property is WhiteBalanceModeProperty)
                    {
                        this.SetProperty((WhiteBalanceModeProperty)property);
                    }
                    else if (property is ExposureModeProperty)
                    {
                        this.SetProperty((ExposureModeProperty)property);
                    }
                    else if (property is FocusModeProperty)
                    {
                        this.SetProperty((FocusModeProperty)property);
                    }
                    else if (property is FrameRateProperty)
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
                TofArManager.Instance.ColorStarted = false;
            }
        }

        private void StopStreamInternal(bool isRestart = true)
        {
            if (this.streamPlay != null && this.streamPlay.IsStarted)
            {
                this.StopPlayback();
            }
            SetProperty(new SharedStreamsProperty { isRestart = isRestart });
            bool wasActive = IsStreamActive;
            this.Stream?.Stop();

            if (wasActive)
            {
                var externalSource = Utils.FindFirstActiveGameObjectThatImplements<IExternalColorStream>();
                if (externalSource != null)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"StopStream to ExternalStreamSource : {externalSource.GetType().Name}");
                    externalSource.StopStream();
                }

                if (OnStreamStopped != null)
                {
                    OnStreamStopped(this);
                }

                // clear texture
                this.YTexture = null;
                this.UVTexture = null;
                this.ColorTexture = null;
                this.nv21Updated = false;
                this.rgbUpdated = false;

            }
            this.streamOpenErrorOccured = false;
        }

        private void StreamCallBack(Stream stream)
        {
            try
            {
                lock (Instance.stopLock)
                {
                    if (!TofArColorManager.Instantiated)
                    {
                        return;
                    }


                    {

                        var frame = stream.GetFrame();
                        if (frame.Channels.Length <= (int)ChannelIds.Color)
                        {
                            frame.Dispose();
                            return;
                        }
                        try
                        {
                            var colorChannel = frame.Channels[(int)ChannelIds.Color];
                            var rawData = colorChannel.GetRawData();
                            ColorData = new ColorData(rawData);
                            foreach (var processor in preProcessors)
                            {
                                ColorData.Data = processor.ParseColorData(ColorData.Data);
                            }

                            frameCount++;
                            //  TofArManager.Logger.WriteLog(LogLevel.Debug, $"color cw{CurrentYWidth} ow{OriginalYWidth} yh{YHeight} with buffer {ColorData.Data.Length}");

                            if (ProcessTexture)
                            {
                                var srcBuffer = ColorData.Data;

                                /*  
                                 *  BGRAのメモリパターン
                                 *  
                                 *  [BGRA 0] [BGRA 1] .. [BGRA n]
                                 *  
                                 *  NV21のメモリパターン
                                 * 
                                 *  Y, U, Vの値は８BITずつ
                                 * 
                                 *  [Y 0] [Y 1] [Y 2] .. [Y n]  [VU 0] [VU 1] .. [VU n/2]
                                 *  
                                 */
                                if (ColorData.Type == ColorRawDataType.NV21)
                                {
                                    ProcessNV21Data(srcBuffer);
                                }
                                else if ((ColorData.Type == ColorRawDataType.BGRA) || (ColorData.Type == ColorRawDataType.RGB) || (ColorData.Type == ColorRawDataType.RGBA))
                                {
                                    ProceessRGBData(srcBuffer);
                                }
                            }
                        }
                        finally
                        {
                            frame.Dispose();
                        }
                        if (OnFrameArrived != null)
                        {
                            OnFrameArrived(this);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            /*catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            catch (IndexOutOfRangeException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }*/
        }

        private bool nv21Updated = false, rgbUpdated = false;
        private object arrayLock = new object();

        private void ProcessNV21Data(byte[] srcBuffer)
        {
            lock (arrayLock)
            {
                var colorDataSize = srcBuffer.Length;
                if (colorDataBuffer.Length != colorDataSize)
                {
                    if (colorDataBuffer.IsCreated)
                    {
                        colorDataBuffer.Dispose();
                    }
                    colorDataBuffer = new NativeArray<byte>(colorDataSize, Allocator.Persistent);
                }
                try
                {
                    colorDataBuffer.CopyFrom(srcBuffer);
                }
                catch (ArgumentException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                    return;
                }

                if (srcBuffer.Length < 16)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"color data buffer length {srcBuffer.Length} < header length {16}");
                    return;
                }

                var header = colorDataBuffer.GetSubArray(0, 16);
                var headerBytes = header.ToArray();
                GCHandle handle = GCHandle.Alloc(headerBytes, GCHandleType.Pinned);
                yuvLayout = (YUVLayout)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(YUVLayout));
                handle.Free();

                int ySize = YHeight * yuvLayout.yStride;
                int uvSize = UVHeight * yuvLayout.uvStride;

                ydata = colorDataBuffer.GetSubArray(header.Length, ySize);
                uvdata = colorDataBuffer.GetSubArray(header.Length + yuvLayout.vByteOffset, uvSize);

                nv21Updated = true;
            }
        }

        private void UpdateNV21Textures()
        {
            if ((YTexture == null) || (UVTexture == null))
            {
                return;
            }
            // Post()が実行されるまで新しいストリームになっているかのチェック。
            int oneRowPadDoubleCheck = (yuvLayout.yStride - OriginalYWidth);
            int thisFramesHeightDoubleCheck = (yuvLayout.vByteOffset + oneRowPadDoubleCheck) / yuvLayout.yStride;
            if (thisFramesHeightDoubleCheck != YHeight)
            {
                return;
            }

            //　行の先端にパディングバイトがあるため、テクスチャーを拡大しないといけない。
            if (yuvLayout.yStride > CurrentYWidth)
            {

                // パディングバイトを無視するためのシェーダーグローバル　texture uv の u軸をいじる
                Shader.SetGlobalFloat("_YUVShader_paddingCutOff", CurrentYWidth / (float)yuvLayout.yStride);

                CurrentYWidth = yuvLayout.yStride;
            }
            if (YTexture.width != yuvLayout.yStride || YTexture.height != YHeight)
            {
#if UNITY_2021_1_OR_NEWER
                YTexture.Reinitialize(yuvLayout.yStride, YHeight);
#else
                YTexture.Resize(yuvLayout.yStride, YHeight);
#endif
            }

            if (UVTexture.width != yuvLayout.uvStride / sizeof(short) || UVTexture.height != UVHeight)
            {
#if UNITY_2021_1_OR_NEWER
                UVTexture.Reinitialize(yuvLayout.uvStride / sizeof(short), UVHeight);
#else
                UVTexture.Resize(yuvLayout.uvStride / sizeof(short), UVHeight);
#endif
            }

            YTexture.LoadRawTextureData(ydata);
            UVTexture.LoadRawTextureData(uvdata);

            YTexture.Apply(false);
            UVTexture.Apply(false);
        }


        private void ProceessRGBData(byte[] srcBuffer)
        {
            lock (arrayLock)
            {
                var colorDataSize = srcBuffer.Length;
                if (colorDataBuffer.Length != colorDataSize)
                {
                    if (colorDataBuffer.IsCreated)
                    {
                        colorDataBuffer.Dispose();
                    }
                    colorDataBuffer = new NativeArray<byte>(colorDataSize, Allocator.Persistent);
                }
                try
                {
                    colorDataBuffer.CopyFrom(srcBuffer);
                }
                catch (ArgumentException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                    return;
                }
                rgbUpdated = true;
            }
        }

        private void UpdateRgbTextures()
        {
            if (ColorTexture == null)
            {
                return;
            }

            ColorTexture.LoadRawTextureData(colorDataBuffer);
            ColorTexture.Apply(false);
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
                if (property is AvailableResolutionsProperty)
                {
                    bufferSize = 16384;
                }
                if (property is MetadataEntriesProperty)
                {
                    bufferSize = 4096;
                }
                var stream = this.IsPlaying ? this.StreamPlay : this.Stream;

                stream?.GetProperty<T>(property.Key, ref property, bufferSize);

                if (property is DefaultResolutionProperty)
                {
                    DefaultResolutionProperty defaultConfig = property as DefaultResolutionProperty;
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
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <param name="bufferSize">シリアライズ用バッファサイズ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value, int bufferSize) where T : class, IBaseProperty
        {
            if (this.isUnPaused || this.IsPlaying)
            {
                var stream = this.IsPlaying ? this.StreamPlay : this.Stream;

                stream?.GetProperty<T>(value.Key, ref value, bufferSize);

                if (value is DefaultResolutionProperty)
                {
                    DefaultResolutionProperty defaultConfig = value as DefaultResolutionProperty;
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
            if (this.isUnPaused || this.IsPlaying)
            {
                int bufferSize = 1024;
                if (value is AvailableResolutionsProperty)
                {
                    bufferSize = 16384;
                }
                if (value is MetadataEntriesProperty)
                {
                    bufferSize = 4096;
                }
                var stream = this.IsPlaying ? this.StreamPlay : this.Stream;

                stream?.GetProperty<T>(value.Key, ref value, bufferSize);

                if (value is DefaultResolutionProperty)
                {
                    DefaultResolutionProperty defaultConfig = value as DefaultResolutionProperty;
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

        private DefaultResolutionProperty GetDefaultFrontCameraConfig()
        {
            string defaultFrontId = null;
            var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>()?.TrimmedDeviceAttributesString;
            if (devcap != null)
            {
                var devAttribs = JsonUtility.FromJson<deviceAttributesJson>(devcap);
                defaultFrontId = devAttribs?.fixedFrontColorCameraId;
            }
            var defaultConfig = this.stream?.GetProperty<DefaultResolutionProperty>();
            if (defaultConfig != null)
            {
                var configs = GetProperty<AvailableResolutionsProperty>();
                if (configs != null)
                {
                    ResolutionProperty firstConfig = null;
                    foreach (var config in configs.resolutions)
                    {
                        if (config.lensFacing == (int)LensFacing.Front)
                        {
                            config.width = defaultConfig.width;
                            config.height = defaultConfig.height;
                            if (firstConfig == null)
                            {
                                firstConfig = config;
                            }
                            if (!String.IsNullOrEmpty(defaultFrontId) && config.cameraId.Equals(defaultFrontId))
                            {
                                firstConfig = config;
                                break;
                            }
                        }
                    }
                    if (firstConfig != null)
                    {
                        defaultConfig = new DefaultResolutionProperty()
                        {
                            lensFacing = firstConfig.lensFacing,
                            cameraId = firstConfig.cameraId,
                            width = firstConfig.width,
                            height = firstConfig.height,
                        };
                    }
                    return defaultConfig;
                }
            }
            

            return null;
        }

        /// <summary>
        /// コンポーネントプロパティを設定する
        /// </summary>
        /// <typeparam name="T">BaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (value is FrameRateProperty)
            {
                this.desiredFrameRate = (value as FrameRateProperty).desiredFrameRate;
            }
            else if (value is FocusModeProperty)
            {
                this.autoFocus = (value as FocusModeProperty).autoFocus;
                this.focusDistance = (value as FocusModeProperty).distance;
            }
            else if (value is ExposureModeProperty)
            {
                this.flashMode = (value as ExposureModeProperty).flash;
                this.autoExposure = (value as ExposureModeProperty).autoExposure;
                this.exposureTime = (value as ExposureModeProperty).exposureTime;
                this.frameDuration = (value as ExposureModeProperty).frameDurarion;
                this.sensitivity = (value as ExposureModeProperty).sensitibity;
            }
            else if (value is WhiteBalanceModeProperty)
            {
                this.autoWhiteBalance = (value as WhiteBalanceModeProperty).autoWhiteBalance;
                this.whiteBalanceMode = (value as WhiteBalanceModeProperty).mode;
            }
            else if (value is StabilizationProperty)
            {
                this.lensStabilization = (value as StabilizationProperty).lensStabilization;
            }

            if (this.isUnPaused)
            {
                this.Stream?.SetProperty<T>(value);

                if (value is FunctionStreamParametersProperty)
                {
                    OnAvailableResolutionsChanged?.Invoke(GetProperty<AvailableResolutionsProperty>());
                }
            }
        }

        /// <summary>
        /// コンポーネントプロパティを設定する
        /// </summary>
        /// <param name="value">入力パラメータ</param>
        public void SetProperty(FormatConvertProperty value)
        {
            if (this.isUnPaused)
            {
                if (this.IsStreamActive)
                {
                    throw new ApiException(new Status() { level = ErrorLevel.Fail, cause = ErrorCause.InvalidOperation });
                }
                else
                {
                    this.Stream?.SetProperty<FormatConvertProperty>(value);
                }
            }
        }

        /// <summary>
        /// Propertyのリストを取得する
        /// </summary>
        /// <returns>Propertyのリスト</returns>
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

        private ResolutionProperty pauseCurrentResolution;

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
            if (this.IsStreamPausing)
            {
                this.IsStreamPausing = false;
                this.StartStream(this.pauseCurrentResolution, this.ProcessTexture);
            }
        }

        #region PLAYBACK

        /// <summary>
        /// *TODO+ B
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_color_stream";

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
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArColorManager.streamPlayerKey);

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

            // When using DebugServer, try at least two times, since it cannot get properties at initial start
            for (int t = 0; t < tryCount; t++)
            {
                var stream = Instance.StreamPlay;

                this.ColorData = null;

                stream.SetProperty<PlayProperty>(play);

                int width = 1280;
                int height = 720;

                try
                {
                    var imgProperty = stream.GetProperty<ResolutionProperty>();

                    width = imgProperty.width;
                    height = imgProperty.height;
                }
                catch (ApiException) // Fails for first time when using DebugServer
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Start playback failed. Trying again ");
                    // dispose stream and try again
                    Instance.streamPlay.Dispose();
                    Instance.streamPlay = null;

                    continue;
                }

                var formatProperty = stream.GetProperty<FormatConvertProperty>();

                var format = formatProperty.format.ToString();

                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Recording uses {width}x{height} - {format}");

                this.ProcessTexture = true;

                {
                    if (this.ColorTexture != null)
                    {
                        Destroy(this.ColorTexture);
                    }

                    if (colorTextureFormats.ContainsKey(format))
                    {
                        this.ColorTexture = new Texture2D(width, height, colorTextureFormats[format], false);
                    }

                    if (this.YTexture != null)
                    {
                        Destroy(this.YTexture);
                    }
                    this.YTexture = new Texture2D(width, height, TextureFormat.Alpha8, false);
                    var blackYData = new byte[YTexture.width * YTexture.height];
                    for (var i = 0; i < blackYData.Length; i++)
                    {
                        blackYData[i] = 16;
                    }
                    this.YTexture.LoadRawTextureData(blackYData);
                    this.YTexture.Apply();

                    if (this.UVTexture != null)
                    {
                        Destroy(this.UVTexture);
                    }
                    this.UVTexture = new Texture2D(width / 2, height / 2,
                        TextureFormat.RGBA4444, false);
                    var blackUVData = new byte[UVTexture.width * UVTexture.height * 2];
                    for (var i = 0; i < blackUVData.Length; i++)
                    {
                        blackUVData[i] = 128;
                    }
                    this.UVTexture.LoadRawTextureData(blackUVData);
                    this.UVTexture.Apply();

                    this.CurrentYWidth = this.YTexture.width;
                    this.OriginalYWidth = this.YTexture.width;
                    this.YHeight = this.YTexture.height;

                    this.UVWidth = this.UVTexture.width * sizeof(short);
                    this.UVHeight = this.UVTexture.height;

                    // パディングバイトはあるかどうかまだわからない
                    Shader.SetGlobalFloat("_YUVShader_paddingCutOff", 1.0f);
                }


                stream.Start();

                if (stream.IsStarted)
                {
                    Instance.IsPlaying = true;

                    if (OnStreamStarted != null)
                    {
                        OnStreamStarted(this, this.ColorTexture);
                    }
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

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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
#if PLATFORM_ANDROID && UNITY_2018_3_OR_NEWER
using UnityEngine.Android;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TofAr.V0
{
    /// <summary>
    /// 有効な端末方向
    /// </summary>
    [Flags]
    public enum EnabledOrientation
    {
        /// <summary>
        /// ホームボタンが下側にある
        /// </summary>
        Portrait = 1,
        /// <summary>
        /// ホームボタンが上側にある
        /// </summary>
        PortraitUpsideDown = 2,
        /// <summary>
        /// ホームボタンが右側にある
        /// </summary>
        LandscapeLeft = 4,
        /// <summary>
        /// ホームボタンが左側にある
        /// </summary>
        LandscapeRight = 8
    }


    /// <summary>
    /// ColorストリームとDepthストリームを開始する順番
    /// </summary>
    public enum CameraStartOrder
    {
        /// <summary>
        /// 任意の順番でColorとDepthのストリームを開始できる
        /// </summary>
        Default,
        /// <summary>
        /// Color -> Depthの順番でストリームを開始する必要がある
        /// </summary>
        ColorToDepth,
        /// <summary>
        /// Depth -> Colorの順番でストリームを開始する必要がある
        /// </summary>
        DepthToColor
    }

    /// <summary>
    /// 端末固有設定ロード元
    /// </summary>
    public enum ConfigSource
    {
        /// <summary>
        /// 端末固有設定が見つからない
        /// </summary>
        None,
        /// <summary>
        /// 端末内固有設定を端末内の設定ファイルよりロードした
        /// </summary>
        LocalFile,
        /// <summary>
        /// 端末内固有設定をSDKデフォルト値でロードした
        /// </summary>
        Default
    }

    /// <summary>
    /// 端末種別
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// Android
        /// </summary>
        Android,

        /// <summary>
        /// iOS
        /// </summary>
        Ios
    }

    /// <summary>
    /// SDK共通機能を提供
    /// </summary>
    public class TofArManager : Singleton<TofArManager>, IDisposable
    {
        private static Logger logger;
        /// <summary>
        /// ログ出力処理
        /// </summary>
        public static Logger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = new Logger();
                    logger.OnLogLevelChanged += (logLevel) =>
                    {
                        var streamHolders = GameObject.FindObjectsOfType(typeof(MonoBehaviour)).OfType<IStreamHolder>();
                        foreach (var streamHolder in streamHolders)
                        {
                            try
                            {
                                streamHolder.Stream?.SetProperty(new LogLevelProperty() { logLevel = logLevel });
                            }
                            catch (ApiException e)
                            {
                                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                            }
                        }
                    };
                }
                return logger;
            }
        }

        /// <summary>
        /// *TODO+ B
        /// 端末内のCamera設定ファイル名
        /// </summary>
        internal static readonly string DeviceAttributesFileName = "tofar_device_attributes.json";
        /// <summary>
        /// *TODO+ B
        /// 端末内のCamera設定のパス
        /// </summary>
        internal static readonly string SpecialDeviceAttributesFilePath = $"/data/local/tmp/tofar/config/{DeviceAttributesFileName}";

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
        public const string StreamKey = "tofar_stream";

        private bool streamOpenErrorOccured = false;

        private SensCord.Stream stream = null;
        /// <summary>
        /// ストリーム
        /// </summary>
        public SensCord.Stream Stream
        {
            get
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                try
                {
                    if (this.stream == null && !this.streamOpenErrorOccured)
                    {
                        WaitForPermissions();

                        var senscordCore = TofArManager.Instance.SensCordCore;



                        if (this.connectionExists)
                        {
                            sw.Start();
                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Trying to open stream");
                            this.stream = senscordCore.OpenStream(StreamKey);
                            if (this.RuntimeSettings.runMode == RunMode.Default)
                            {
                                if (this.stream != null)
                                {
#if PLATFORM_IOS
                                currentDeviceCapability = this.stream.GetProperty<DeviceCapabilityProperty>();
                                this.CheckDeviceProfiles();
#endif
                                    if (currentDeviceCapability != null)
                                    {
                                        this.stream.SetProperty<DeviceCapabilityProperty>(currentDeviceCapability);
                                    }
                                }
                            }
                            sw.Stop();

                            this.stream?.SetProperty(new PlatformConfigurationProperty()
                            {
                                platformConfigurationIos = new PlatformConfigurationIos()
                                {
                                    sessionFramerate = iOSSessionFramerate
                                }
                            });
                        }
                        else
                        {
                            throw new ApiException(new Status());
                        }

                    }
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                    this.streamOpenErrorOccured = true;
                    this.connectionExists = false;
                    sw.Stop();
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Timeout after {sw.ElapsedMilliseconds}ms");
                }
                return this.stream;
            }
        }

        private bool connectionExists = true;

        /// <summary>
        /// trueの場合、毎フレームごとにcameraPoseTrackerからCameraPoseを取得する。
        /// </summary>
        [HideInInspector]
        public bool autoTrackCameraPose = false;
        /// <summary>
        /// nullでは無い場合、CameraPoseを取得しキャッシュする。CameraPoseはCameraPosePropertyを介し他のコンポーネントへ公開される。
        /// </summary>
        [HideInInspector]
        public Transform cameraPoseTracker = null;

        /// <summary>
        /// *TODO+ B
        /// Debug時にCameraPoseを更新する間隔
        /// </summary>
        private uint cameraPoseUpdateIntervalMsOnEditorDebbug = (1000 / 15);
        private DateTime prevCameraPoseUpdated = DateTime.MinValue;
        private CameraPoseProperty cameraPoseCache = new CameraPoseProperty();

        private CameraPoseProperty GetCameraPoseWithCache()
        {
            if ((RuntimeSettings.runMode == RunMode.Default))
            {
                return this.Stream?.GetProperty<CameraPoseProperty>();
            }
            else
            {
                var result = this.cameraPoseCache;
                var interval = 0u;
                if (!this.streamOpenErrorOccured)
                {
                    interval = this.cameraPoseUpdateIntervalMsOnEditorDebbug;
                }
                if ((DateTime.Now - this.prevCameraPoseUpdated).Milliseconds > interval)
                {
                    result = this.Stream?.GetProperty<CameraPoseProperty>();
                    this.prevCameraPoseUpdated = DateTime.Now;
                }
                return result;
            }
        }

        private RuntimeSettingsProperty runtimeSettings = new RuntimeSettingsProperty();

        /// <summary>
        /// 実行時設定
        /// </summary>
        public RuntimeSettingsProperty RuntimeSettings
        {
            get
            {
                if (!this.runtimeSettingsInitialized)
                {
                    if (((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.WindowsPlayer)
                        || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.OSXPlayer))
                        && Application.isPlaying)
                    {
                        this.runtimeSettings.runMode = RunMode.MultiNode;
                    }
                    else
                    {
                        this.runtimeSettings.runMode = RunMode.Default;
                    }
                    var debugCanvas = GameObject.Find("DebugServerCanvas");
                    this.runtimeSettings.isRemoteServer = debugCanvas != null;
                    this.runtimeSettingsInitialized = true;
                }
                return this.runtimeSettings;
            }
        }
        private bool runtimeSettingsInitialized = false;

        /// <summary>
        /// 画面回転時のデリゲート
        /// </summary>
        /// <param name="previousScreenOrientation">回転前の画面向き</param>
        /// <param name="newScreenOrientation">回転後の画面向き</param>
        public delegate void OnScreenRotationChangedEvent(ScreenOrientation previousScreenOrientation, ScreenOrientation newScreenOrientation);

        /// <summary>
        /// 画面方向変更通知
        /// </summary>
        public static event OnScreenRotationChangedEvent OnScreenOrientationUpdated;

        /// <summary>
        /// *TODO+ B
        /// 画面方向変更通知
        /// </summary>
        [Obsolete("use the static OnScreenOrientationUpdated instead")]
        private event OnScreenRotationChangedEvent OnScreenOrientationChanged
        {
            add { OnScreenOrientationUpdated += value; }
            remove { OnScreenOrientationUpdated -= value; }
        }
        /// <summary>
        /// 端末回転時デリゲート
        /// </summary>
        /// <param name="previousDeviceOrientation">回転前の端末向き</param>
        /// <param name="newDeviceOrientation">回転後の端末向き</param>
        public delegate void OnDeviceRotationChangedEvent(DeviceOrientation previousDeviceOrientation, DeviceOrientation newDeviceOrientation);
        /// <summary>
        /// デバイス回転変更通知
        /// </summary>
        public static event OnDeviceRotationChangedEvent OnDeviceOrientationUpdated;
        /// <summary>
        /// *TODO+ B
        /// デバイス回転変更通知
        /// </summary>
        [Obsolete("use static OnDeviceOrientationUpdated instead")]
        private event OnDeviceRotationChangedEvent OnDeviceOrientationChanged
        {
            add { OnDeviceOrientationUpdated += value; }
            remove { OnDeviceOrientationUpdated -= value; }
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

        private void OnValidate()
        {
            if (this.stream == null)
            {
                return;
            }


            this.stream.SetProperty(new PlatformConfigurationProperty()
            {
                platformConfigurationIos = new PlatformConfigurationIos()
                {
                    sessionFramerate = iOSSessionFramerate
                }
            });
        }

        void Start()
        {
            if (((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.WindowsPlayer)
                || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.OSXPlayer))
                && Application.isPlaying)
            {
                this.RuntimeSettings.runMode = RunMode.MultiNode;
                //this.SetProperty(this.RuntimeSettings);
                this.currentDeviceOrientation = this.Stream?.GetProperty<DeviceOrientationsProperty>();
            }
            this.InitializeFpsDump();
        }

        /// <summary>
        /// *TODO+ B Userに勝手に閉じられると困る
        /// ストリームを解除する
        /// </summary>
        private void CloseStream()
        {
            if (this.stream != null)
            {
                if (this.stream.IsStarted)
                {
                    this.stream.Stop();
                }
                this.Stream?.Dispose();
                this.stream = null;
            }
        }


        /// <summary>
        /// デバイス回転の検出間隔(ミリ秒)
        /// </summary>
        public uint deviceOrientationUpdateIntervalMs = 66;
        /// <summary>
        /// TofArServerとの接続時のデバイス回転の検出間隔(ミリ秒)
        /// </summary>
        public uint deviceOrientationUpdateIntervalMsOnEditorDebbug = 500;
        private DateTime prevDeviceOrientationsUpdated = DateTime.MinValue;
        private DeviceOrientationsProperty currentDeviceOrientation = new DeviceOrientationsProperty();

        private DeviceOrientationsProperty GetDeviceOrientationsWithCache()
        {
            var interval = this.deviceOrientationUpdateIntervalMs;
            if ((RuntimeSettings.runMode == RunMode.MultiNode) && !this.streamOpenErrorOccured)
            {
                interval = this.deviceOrientationUpdateIntervalMsOnEditorDebbug;
            }

            if ((DateTime.Now - this.prevDeviceOrientationsUpdated).Milliseconds > interval)
            {
                this.currentDeviceOrientation = this.Stream?.GetProperty<DeviceOrientationsProperty>();
                this.prevDeviceOrientationsUpdated = DateTime.Now;
            }
            return this.currentDeviceOrientation;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>() where T : class, IBaseProperty, new()
        {
            T property = new T();
            if (property is DeviceOrientationsProperty)
            {
                var deviceOrientationsProperty = (this.GetDeviceOrientationsWithCache() as T);
                return deviceOrientationsProperty;
            }
            if (property is CameraPoseProperty)
            {
                return (this.GetCameraPoseWithCache() as T);
            }

            /*if (property is DirectoryListProperty && this.RuntimeSettings.runMode == RunMode.MultiNode) // For debug mode, enumerate directories on client side
            {
                var dirListProp = this.Stream?.GetProperty<DirectoryListProperty>();
                string root = dirListProp.path;
                var subDirectories = new System.IO.DirectoryInfo(root).EnumerateDirectories().Select(x => x.Name).ToList();

                dirListProp.directoryList = subDirectories;

                return (dirListProp as T);
            }*/

            int buffersize = 1024;
            if (property is DeviceCapabilityProperty || property is DirectoryListProperty)
            {
                buffersize = 16192;
            }

            if (connectionExists)
            {
                this.Stream?.GetProperty<T>(property.Key, ref property, buffersize);
            }
            return property;
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
            if (value is DeviceOrientationsProperty)
            {
                var deviceOrientationsProperty = (this.GetDeviceOrientationsWithCache() as T);
                return deviceOrientationsProperty;
            }
            if (value is CameraPoseProperty)
            {
                return (this.GetCameraPoseWithCache() as T);
            }

            if (connectionExists)
            {
                this.Stream?.GetProperty<T>(value.Key, ref value, bufferSize);
            }

            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (value is DeviceOrientationsProperty)
            {
                var deviceOrientationsProperty = (this.GetDeviceOrientationsWithCache() as T);
                return deviceOrientationsProperty;
            }
            if (value is CameraPoseProperty)
            {
                return (this.GetCameraPoseWithCache() as T);
            }

            int buffersize = 1024;
            if (value is DeviceCapabilityProperty)
            {
                buffersize = 16192;
            }


            if (connectionExists)
            {
                this.Stream?.GetProperty<T>(value.Key, ref value, buffersize);
            }

            return value;
        }

        /// <summary>
        /// コンポーネントプロパティを設定する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (value is CameraPoseProperty)
            {
                this.cameraPoseCache = (value as CameraPoseProperty);
            }
            if (connectionExists)
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
            return this.Stream?.GetPropertyList();
        }

        private Core sensCordCore = null;
        /// <summary>
        /// *TODO+ B internalにしたい（SensCordCoreを使用するサンプルがあるのでpublicのままにする）
        /// SensCordのCoreクラス
        /// </summary>
        public Core SensCordCore
        {
            get
            {
                if (sensCordCore == null)
                {

#if !UNITY_EDITOR && !UNITY_IOS && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_OSX
                    this.CheckDeviceProfiles(); // only on device
#endif
                    this.SetSensCordFilePath();
                    this.sensCordCore = new Core();
                }
                return this.sensCordCore;
            }
        }
        private DeviceCapabilityProperty currentDeviceCapability = null;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        /// <summary>
        /// TODO+ C UnityEditor用でアプリ開発には不要な情報なのでリファレンスに書かなくても良いかも
        /// </summary>
        [HideInInspector]
        public bool enableNetworkDebugging = false;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [HideInInspector]
        public string debugServerIpAddress = "127.0.0.1";
        /// <summary>
        /// TODO+ C
        /// </summary>
        [HideInInspector]
        public string debugServerPort = "8080";
        /// <summary>
        /// TODO+ C
        /// </summary>
        [HideInInspector]
        public TextAsset deviceProfile;
        /// <summary>
        /// Type of device to which to connect
        /// </summary>
        [HideInInspector]
        public DeviceType debugServerDevice = DeviceType.Android;
        /// <summary>
        /// Timeout for debug server
        /// </summary>
        [HideInInspector]
        public int serverConnectionTimeout = 10000;

        /// <summary>
        /// ADBコマンド種別
        /// </summary>
        public enum AdbCommand
        {
            /// <summary>
            /// adb push
            /// </summary>
            Push,

            /// <summary>
            /// adb pull
            /// </summary>
            Pull,

            /// <summary>
            /// adb remove
            /// </summary>
            Remove,

            /// <summary>
            /// adb forward
            /// </summary>
            Forward
        }

#endif

#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN

        /// <summary>
        /// Standalone player用サーバー接続設定
        /// </summary>
        [Serializable]
        public class ServerConnectionSettings
        {
            /// <summary>
            /// trueの場合ネットワーク接続を行う
            /// falseの場合、USB接続を行う
            /// </summary>
            public bool enableNetworkDebugging = true;

            /// <summary>
            /// サーバーのIPアドレス
            /// </summary>
            public string debugServerIpAddress = "127.0.0.1";

            /// <summary>
            /// サーバーのポート番号
            /// </summary>
            public string debugServerPort = "8080";

            /// <summary>
            /// adbへのフルパス
            /// </summary>
            public string fullPathToAdb = string.Empty;
        }

        /// <summary>
        /// Get path to ServerConnectionSettings.json
        /// </summary>
        private string GetServerConnectionSettingsFilePath()
        {
            var serverConnectionSettings = new ServerConnectionSettings();
            var serverConnectionSettingsDirPath = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}{Path.DirectorySeparatorChar}TofAR";
            var serverConnectionSettingsFilePath = $"{serverConnectionSettingsDirPath}{Path.DirectorySeparatorChar}ServerConnectionSettings.json";
            return serverConnectionSettingsFilePath;
        }

        public ServerConnectionSettings ServerConnectionSettingsForStandalone { get; private set; }

#endif

        [SerializeField]
        [HideInInspector]
        private int iOSSessionFramerate = 60;

        /// <summary>
        /// *TODO+ B　内部処理用
        /// SensCordパスを指定する
        /// </summary>
        private void SetSensCordFilePath()
        {

#if UNITY_EDITOR

            this.enableNetworkDebugging = EditorPrefs.GetBool("NetworkDebug", false);
            this.debugServerIpAddress = this.enableNetworkDebugging ? EditorPrefs.GetString("IP", "127.0.0.1") : "127.0.0.1";
            this.debugServerPort = EditorPrefs.GetString("Port", "8080");

#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN

            this.ServerConnectionSettingsForStandalone = new ServerConnectionSettings();
            var serverConnectionSettingsFilePath = this.GetServerConnectionSettingsFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(serverConnectionSettingsFilePath));
            if (!File.Exists(serverConnectionSettingsFilePath))
            {
                File.WriteAllText(serverConnectionSettingsFilePath, JsonUtility.ToJson(this.ServerConnectionSettingsForStandalone));
            }
            else
            {
                this.ServerConnectionSettingsForStandalone = JsonUtility.FromJson<ServerConnectionSettings>(File.ReadAllText(serverConnectionSettingsFilePath));
            }
            this.enableNetworkDebugging = this.ServerConnectionSettingsForStandalone.enableNetworkDebugging;
            this.debugServerIpAddress = this.enableNetworkDebugging ? this.ServerConnectionSettingsForStandalone.debugServerIpAddress : "127.0.0.1";
            this.debugServerPort = this.ServerConnectionSettingsForStandalone.debugServerPort;

            TofArManager.Logger.WriteLog(LogLevel.Debug, $"serverConnectionSettingsFilePath:{serverConnectionSettingsFilePath} enableNetworkDebugging:{this.enableNetworkDebugging} debugServerIpAddress:{this.debugServerIpAddress} debugServerPort:{this.debugServerPort}");

            if (!this.enableNetworkDebugging)
            {
                Utils.TcpForward(this.ServerConnectionSettingsForStandalone.fullPathToAdb);
            }
#endif

            // config
            var configDirectoryPath = Application.temporaryCachePath + "/TofAr";
            if (!Directory.Exists(configDirectoryPath))
            {
                Directory.CreateDirectory(configDirectoryPath);
            }

            UpdateConfigFile(configDirectoryPath);

            // SENSCORD_FILE_PATHの設定
            if ((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.WindowsPlayer)
                || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.OSXPlayer))
            {
#if UNITY_EDITOR_WIN
                var pluginPath = $"{Application.dataPath}{Path.DirectorySeparatorChar}TofAr{Path.DirectorySeparatorChar}TofAr{Path.DirectorySeparatorChar}Plugins{Path.DirectorySeparatorChar}x86_64{Path.DirectorySeparatorChar}";
                this.TryCopyFile($"{pluginPath}allocator_sample.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}allocator_sample.dll");
                this.TryCopyFile($"{pluginPath}component_client.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_client.dll");
                //this.TryCopyFile($"{pluginPath}component_player.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_player.dll");
                this.TryCopyFile($"{pluginPath}tcp_connection.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}tcp_connection.dll");
                SensCord.Environment.SetSensCordFilePath(configDirectoryPath);
#elif UNITY_STANDALONE_WIN
                var pluginPath = $"{Application.dataPath}{Path.DirectorySeparatorChar}Plugins{Path.DirectorySeparatorChar}x86_64{Path.DirectorySeparatorChar}";
                this.TryCopyFile($"{pluginPath}allocator_sample.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}allocator_sample.dll");
                this.TryCopyFile($"{pluginPath}component_client.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_client.dll");
                //this.TryCopyFile($"{pluginPath}component_player.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_player.dll");
                this.TryCopyFile($"{pluginPath}tcp_connection.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}tcp_connection.dll");
                SensCord.Environment.SetSensCordFilePath(configDirectoryPath);
#elif UNITY_EDITOR_OSX
                var pluginPath = $"{Application.dataPath}{Path.DirectorySeparatorChar}TofAr{Path.DirectorySeparatorChar}TofAr{Path.DirectorySeparatorChar}Plugins{Path.DirectorySeparatorChar}OSX{Path.DirectorySeparatorChar}";
                this.TryCopyFile($"{pluginPath}allocator_sample.bundle", $"{configDirectoryPath}{Path.DirectorySeparatorChar}allocator_sample.bundle");
                this.TryCopyFile($"{pluginPath}component_client.bundle", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_client.bundle");
                //this.TryCopyFile($"{pluginPath}component_player.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_player.dll");
                this.TryCopyFile($"{pluginPath}tcp_connection.bundle", $"{configDirectoryPath}{Path.DirectorySeparatorChar}tcp_connection.bundle");
                SensCord.Environment.SetSensCordFilePath(configDirectoryPath);
#elif UNITY_STANDALONE_OSX
                var pluginPath = $"{Application.dataPath}{Path.DirectorySeparatorChar}Plugins{Path.DirectorySeparatorChar}";
                this.TryCopyFile($"{pluginPath}allocator_sample.bundle", $"{configDirectoryPath}{Path.DirectorySeparatorChar}allocator_sample.bundle");
                this.TryCopyFile($"{pluginPath}component_client.bundle", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_client.bundle");
                //this.TryCopyFile($"{pluginPath}component_player.dll", $"{configDirectoryPath}{Path.DirectorySeparatorChar}component_player.dll");
                this.TryCopyFile($"{pluginPath}tcp_connection.bundle", $"{configDirectoryPath}{Path.DirectorySeparatorChar}tcp_connection.bundle");
                SensCord.Environment.SetSensCordFilePath(configDirectoryPath);
#endif
            }
            else
            {
                var libraryDirectoryPath = string.Empty;
                var libraryDirectoryPath64 = string.Empty;
#if UNITY_ANDROID
                //32 bit library
                libraryDirectoryPath = "/data/data/" + Application.identifier + "/lib";

                //64 bit library
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaObject pkgmngr = context.Call<AndroidJavaObject>("getPackageManager");
                AndroidJavaObject appinfo = pkgmngr.Call<AndroidJavaObject>("getApplicationInfo", Application.identifier, 1024 /*PackageManager.GET_SHARED_LIBRARY_FILES*/);

                libraryDirectoryPath64 = appinfo.Get<string>("nativeLibraryDir");
#elif UNITY_IOS
                libraryDirectoryPath = string.Empty;

                var appRootPath = Application.dataPath.Replace("/Data", "");
                libraryDirectoryPath64 = string.Empty;
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/allocator_sample.framework"}:";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_client.framework"}:";
	        libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_player.framework"}:";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/tcp_connection.framework"}:";
                //                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_pseudo_image.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_tof.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_color.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_hand.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_body.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_mesh.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_coordinate.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_segmentation.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_slam.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/component_tofar_face.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/HandRecognizer-tflite.framework:"}";
                libraryDirectoryPath64 += $"{appRootPath + "/Frameworks/BodyRecognizer-tflite.framework:"}";
#endif


                string recordingPath = Application.persistentDataPath + "/recordings";


                try
                {
                    if (!System.IO.Directory.Exists(recordingPath))
                    {
                        System.IO.Directory.CreateDirectory(recordingPath);
                    }
                    string recordingFile = recordingPath + "/recordings";
                    if (!System.IO.File.Exists(recordingFile))
                    {
                        File.Create(recordingFile);
                    }
                }
                catch (IOException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
                }
                catch (UnauthorizedAccessException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
                }
                catch (ArgumentException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
                }
                catch (NotSupportedException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
                }

                SensCord.Environment.SetSensCordFilePath(libraryDirectoryPath + ":" + libraryDirectoryPath64 + ":" + configDirectoryPath + ":" + recordingPath);

            }
        }

        private void UpdateConfigFile(string configDirectoryPath)
        {
            var xmls = Resources.LoadAll("xml", typeof(TextAsset));
            if (xmls == null)
            {
                return;
            }
            foreach (var xml in xmls)
            {
                var t = (TextAsset)xml;
                if (t == null)
                {
                    continue;
                }
                var filePath = String.Format("{0}/{1}.xml", configDirectoryPath, t.name);
                if (filePath == null)
                {
                    continue;
                }
                try
                {
                    if (((Application.platform == RuntimePlatform.WindowsEditor)
                        || (Application.platform == RuntimePlatform.OSXEditor))
                        && (t.name == "senscord"))
                    {
#if UNITY_EDITOR
                        // modify to debbuging on Unity Editor
                        var xmlPath = AssetDatabase.GetAssetPath(t);
                        if (xmlPath != null)
                        {
                            var doc = XDocument.Load(xmlPath);
                            if (doc != null)
                            {
                                var root = doc.Root;
                                if (root != null)
                                {
                                    SetServerAddress(root);

                                    SetClientInstance(root);
                                }

                            }
                            doc.Save(filePath);
                        }
#endif
                    }
                    else
                    {
                        try
                        {
                            File.WriteAllText(filePath, t.text, Encoding.UTF8);
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
                            var doc = XDocument.Load(filePath);
                            if (doc != null)
                            {
                                var root = doc.Root;
                                if (root != null)
                                {
                                    SetServerAddress(root);

                                    SetClientInstance(root);
                                }

                            }
                            doc.Save(filePath);
#endif
                        }
                        catch (IOException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, String.Format("{0} {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                        }
                    }
                }
                catch (ArgumentException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, String.Format("{0} {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
                }
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        private void SetServerAddress(XElement root)
        {
            var instances = root.Elements("instances");
            {
                var instance = instances.SelectMany(e => e.Elements("instance")).Where(a => a.Attribute("name").Value == "client_instance.0").FirstOrDefault();
                if (instance != null)
                {
                    var argumentList = instance.Elements("arguments")?.SelectMany(e => e.Elements("argument"));
                    if (argumentList != null)
                    {
                        var address = argumentList.Where(a => a.Attribute("name").Value == "address")?.FirstOrDefault();
                        if (address != null)
                        {
                            if (enableNetworkDebugging)
                            {
                                if (System.Text.RegularExpressions.Regex.Match(debugServerIpAddress, @"^\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b").Success)
                                {
                                    address.SetAttributeValue("value", $"{debugServerIpAddress}:{debugServerPort}");
                                }
                                else
                                {
                                    TofArManager.Logger.WriteLog(LogLevel.Debug, "No valid ip address: Using 127.0.0.1 as default");
                                    address.SetAttributeValue("value", "127.0.0.1:9999");
                                }
                            }
                            else
                            {
                                address.SetAttributeValue("value", "127.0.0.1:9999");
                            }
                        }
                        var timeout = argumentList.Where(a => a.Attribute("name").Value == "reply_timeout_msec")?.FirstOrDefault();
                        if (timeout != null)
                        {
                            timeout.SetAttributeValue("value", serverConnectionTimeout.ToString());
                        }
                    }

                }

            }
        }

        private void SetClientInstance(XElement root)
        {
            var streams = root.Elements("streams")?.SelectMany(e => e.Elements("stream"));
            if (streams != null)
            {
                foreach (var stream in streams)
                {
                    if (stream != null)
                    {
                        var client = new XElement("client");
                        if (client != null)
                        {
                            client.SetAttributeValue("instanceName", "client_instance.0");
                            stream.AddFirst(client);
                        }
                    }
                }
            }
        }
#endif

        private void CleaunpCacheFolder()
        {
            var configDirectoryPath = Application.temporaryCachePath + "/TofAr";

            if (!Directory.Exists(configDirectoryPath))
            {
                Directory.CreateDirectory(configDirectoryPath);
            }

            TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArManager] cleanup start files:{Directory.GetFiles(configDirectoryPath).Length}");
            foreach (var file in Directory.GetFiles(configDirectoryPath))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArManager] cleanup {file}");
                try
                {
                    File.Delete(file);
                }
                catch (IOException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                }
                catch (UnauthorizedAccessException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                }
                catch (ArgumentException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                }
                catch (NotSupportedException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                }
            }
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArManager] cleanup end files:{Directory.GetFiles(configDirectoryPath).Length}");
        }


        private bool TryCopyFile(string sourceFile, string destinationFile)
        {
            try
            {
                File.Copy(sourceFile, destinationFile, true);
                return true;
            }
            catch (IOException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            catch (UnauthorizedAccessException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            catch (NotSupportedException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
            return false;
        }

        private bool checkedForIos = false;
        private bool usingIos = false;

        /// <summary>
        /// iOS端末チェック
        /// </summary>
        public bool UsingIos
        {
            get
            {
                if (!checkedForIos)
                {
                    checkedForIos = true;
                    
                    if (this.RuntimeSettings.runMode == RunMode.MultiNode)
                    {
                        var deviceCapability = GetProperty<DeviceCapabilityProperty>();
                        usingIos = deviceCapability?.modelName?.Contains("iPhone") == true || deviceCapability?.modelName?.Contains("iPad") == true;
                    }
                    else
                    {
                        usingIos = Application.platform == RuntimePlatform.IPhonePlayer;
                    }
                }

                return usingIos;
            }
        }

        /// <summary>
        /// 有効な端末方向
        /// </summary>
        [HideInInspector]
        public EnabledOrientation EnabledOrientations;

        private IList<IStreamStoppable> SubManagers { get; set; }

        private bool cameraStartOrderNotLoaded = true;
        private CameraStartOrder cameraStartOrder;
        /// <summary>
        /// ColorストリームとDepthストリームを開始する順番
        /// </summary>
        public CameraStartOrder CameraStartOrder
        {
            get
            {
                if (cameraStartOrderNotLoaded)
                {
                    GetJsonValues();
                }

                return cameraStartOrder;
            }
        }

        //public bool FirstCameraStarted { get; set; } = false;
        /// <summary>
        /// *TODO+ B 内部処理用 internalにしたい
        /// Tofストリームか開始されたかどうか
        /// </summary>
        internal bool TofStarted { get; set; } = false;
        /// <summary>
        /// *TODO+ B 内部処理用 internalにしたい
        /// Colorストリームか開始されたかどうか
        /// </summary>
        internal bool ColorStarted { get; set; } = false;

        /// <summary>
        /// *TODO+ B 内部処理用 internalにしたい
        /// TofManagerがAutoStartに指定されたかどうか
        /// </summary>
        internal bool TofAutoStart { get; set; } = false;
        /// <summary>
        /// *TODO+ B 内部処理用 internalにしたい
        /// ColorManagerがAutoStartに指定されたかどうか
        /// </summary>
        internal bool ColorAutoStart { get; set; } = false;

        /// <summary>
        /// *TODO+ B 内部処理用 internalにしたい
        /// TofArManagerが停止になったかどうか
        /// </summary>
        internal bool WasPaused { get; private set; } = false;

        private DeviceOrientationsProperty currentOrientationProperty = new DeviceOrientationsProperty();

        private int deviceOrientation = 0;
        private int screenOrientation = 0;

        private bool deviceOrientationUpdated = false;
        private bool screenOrientationUpdated = false;

        private bool waitedForPermissions = false;

        private void WaitForPermissions()
        {
            bool userHasAuthorizationAtStartup = HasUserAuthorization();

            if (!userHasAuthorizationAtStartup)
            {
                StartCoroutine(RequestPermissions());

                while (true)
                {
                    if (HasUserAuthorization())
                    {
                        break;
                    }
                }
            }

            waitedForPermissions = !userHasAuthorizationAtStartup;
        }

        private bool HasUserAuthorization()
        {
#if PLATFORM_ANDROID && UNITY_2018_3_OR_NEWER
            return Permission.HasUserAuthorizedPermission(Permission.Camera);
#elif PLATFORM_IOS
            return Application.HasUserAuthorization(UserAuthorization.WebCam);
#else
            return true;
#endif
        }

        IEnumerator RequestPermissions()
        {
#if PLATFORM_ANDROID && UNITY_2018_3_OR_NEWER
            // Unity by default skips permission request when VR or ARCore is enabled
            //if (UnityEngine.XR.XRSettings.enabled)
            {
                if (!HasUserAuthorization())
                {
                    Permission.RequestUserPermission(Permission.Camera);
                }
            }
#elif PLATFORM_IOS
            if (!HasUserAuthorization())
            {
                yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            }
#endif
            yield return null;
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofAr.OnDestroy()");
            lock (this.fpsDumpLock)
            {
                this.fpsDumpEnabled = false;
            }
            if (this.SubManagers != null)
            {
                //create a fixed copy of the list so the remove calls don't cause issue
                var managersList = this.SubManagers.ToArray();
                foreach (var subManager in managersList)
                {
                    subManager.Dispose();
                }
                this.SubManagers.Clear();
            }

            OnDeviceOrientationUpdated = null;
            OnScreenOrientationUpdated = null;
            this.CloseStream();

            if (this.sensCordCore != null)
            {
                this.sensCordCore.Dispose();
                this.sensCordCore = null;
            }
        }

        /// <summary>
        /// *TODO+ B
        /// 全てのストリームを停止する
        /// </summary>
        [Obsolete("This function is being moved to internal - it is used for pausing the streams, not for stopping them")]
        internal void StopStreams()
        {
            if (this.SubManagers != null)
            {
                foreach (var subManager in this.SubManagers)
                {
                    subManager.PauseStream();
                }
            }
        }

        /// <summary>
        /// *TODO+ B
        /// 全てのストリームを再開する
        /// </summary>
        [Obsolete("This function is being moved to internal - it is used for unpausing the streams, not for starting them")]
        internal void StartStreams()
        {
            if (this.SubManagers != null)
            {
                foreach (var subManager in this.SubManagers)
                {
                    subManager?.UnpauseStream();
                }
            }
        }

        /// <summary>
        /// *TODO+ B internalにしたい
        /// ManagerをSubManagerリストに追加する
        /// </summary>
        /// <param name="subManager">*TODO+ C Manager</param>
        internal void AddSubManager(IStreamStoppable subManager)
        {
            if (this.SubManagers == null)
            {
                this.SubManagers = new List<IStreamStoppable>();
            }

            this.SubManagers.Add(subManager);
        }

        /// <summary>
        /// *TODO+ B internalにしたい
        /// ManagerをSubManagerリストから削除する
        /// </summary>
        /// <param name="subManager">*TODO+ C Manager</param>
        internal void RemoveSubManager(IStreamStoppable subManager)
        {
            if (this.SubManagers == null)
            {
                this.SubManagers = new List<IStreamStoppable>();
            }

            this.SubManagers.Remove(subManager);
        }

        private class deviceAttributesJson
        {
            public int cameraStartOrder = 0;
        }

        private class deviceVersionJson
        {
            public string version = "";
        }

        private void GetJsonValues()
        {
            var capability = GetProperty<DeviceCapabilityProperty>();

            var depthcorr = JsonUtility.FromJson<deviceAttributesJson>(capability.TrimmedDeviceAttributesString);
            if (depthcorr != null)
            {
                cameraStartOrder = (CameraStartOrder)depthcorr.cameraStartOrder;

                TofArManager.Logger.WriteLog(LogLevel.Debug, "Camera start order is " + cameraStartOrder);
                cameraStartOrderNotLoaded = false;
            }
        }

        void Update()
        {
#if UNITY_EDITOR

#if !UNITY_2019_1_OR_NEWER
            if (UnityEditor.PlayerSettings.virtualRealitySupported)
#endif // !UNITY_2019_1_OR_NEWER
#if !UNITY_2019_1_OR_NEWER || XR_SUPPORTED
            {
                EnabledOrientations = 0;

                if (UnityEditor.PlayerSettings.defaultInterfaceOrientation == UnityEditor.UIOrientation.LandscapeLeft)
                {
                    EnabledOrientations = EnabledOrientation.LandscapeLeft;
                }
                else if (UnityEditor.PlayerSettings.defaultInterfaceOrientation == UnityEditor.UIOrientation.LandscapeRight)
                {
                    EnabledOrientations = EnabledOrientation.LandscapeRight;
                }
                else if (UnityEditor.PlayerSettings.defaultInterfaceOrientation == UnityEditor.UIOrientation.Portrait)
                {
                    EnabledOrientations = EnabledOrientation.Portrait;
                }
                else if (UnityEditor.PlayerSettings.defaultInterfaceOrientation == UnityEditor.UIOrientation.PortraitUpsideDown)
                {
                    EnabledOrientations = EnabledOrientation.PortraitUpsideDown;
                }
                else if (UnityEditor.PlayerSettings.defaultInterfaceOrientation == UnityEditor.UIOrientation.AutoRotation)
                {
                    if (UnityEditor.PlayerSettings.allowedAutorotateToLandscapeLeft)
                    {
                        EnabledOrientations |= EnabledOrientation.LandscapeLeft;
                    }

                    if (UnityEditor.PlayerSettings.allowedAutorotateToLandscapeRight)
                    {
                        EnabledOrientations |= EnabledOrientation.LandscapeRight;
                    }

                    if (UnityEditor.PlayerSettings.allowedAutorotateToPortrait)
                    {
                        EnabledOrientations |= EnabledOrientation.Portrait;
                    }

                    if (UnityEditor.PlayerSettings.allowedAutorotateToPortraitUpsideDown)
                    {
                        EnabledOrientations |= EnabledOrientation.PortraitUpsideDown;
                    }
                }

                UnityEditor.EditorUtility.SetDirty(this);

            }
#endif // !UNITY_2019_1_OR_NEWER || XR_SUPPORTED
#endif // UNITY_EDITOR
            if (!this.connectionExists)
            {
                return;
            }
            if (this.autoTrackCameraPose && (this.RuntimeSettings.runMode == RunMode.Default))
            {
                this.UpdateCameraPoseCache();
            }

            if (Application.isPlaying)
            {
                if (this.RuntimeSettings.runMode == RunMode.Default)
                {
                    SetRotationProperty();
                }

                GetRotationProperty();
            }
        }

        private void UpdateCameraPoseCache()
        {
            if (this.cameraPoseTracker != null)
            {
                if ((this.cameraPoseCache.Position != this.cameraPoseTracker.transform.position) || (this.cameraPoseCache.Rotation != this.cameraPoseTracker.transform.rotation))
                {
                    this.cameraPoseCache.Position = this.cameraPoseTracker.transform.position;
                    this.cameraPoseCache.Rotation = this.cameraPoseTracker.transform.rotation;
                    this.ForceUpdateCameraPose();
                }
            }
        }

        /// <summary>
        /// TofArManager.cameraPoseTrackerのCameraPoseを強制的に取得し、内部キャッシュを更新する
        /// </summary>
        public void ForceUpdateCameraPose()
        {
            if (this.autoTrackCameraPose && (this.RuntimeSettings.runMode == RunMode.Default))
            {
                //TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("[TofArSlam][SlamOutput] : TofArManager.ForceUpdateCameraPose() 01 -> ({0},{1},{2})-({3},{4},{5},{6})",
                //    cameraPoseCache.position.x, cameraPoseCache.position.y, cameraPoseCache.position.z, 
                //    cameraPoseCache.rotation.x, cameraPoseCache.rotation.y, cameraPoseCache.rotation.z, cameraPoseCache.rotation.w));
                this.SetProperty(this.cameraPoseCache);

                //var property = this.GetProperty<CameraPoseProperty>();
                //TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("[TofArSlam][SlamOutput] : TofArManager.ForceUpdateCameraPose() 02 -> ({0},{1},{2})-({3},{4},{5},{6})",
                //   property.position.x, property.position.y, property.position.z,
                //   property.rotation.x, property.rotation.y, property.rotation.z, property.rotation.w));
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

            if (this.waitedForPermissions)
            {
                if (pause)
                {
                    this.waitedForPermissions = false; // OnApplicationPause is called for some reason at startup when asking for permissions -> prevent from deleting all files
                }

                return;
            }
            if (pause)
            {
                bool useTofAndColorBefore = Instance.ColorStarted && Instance.TofStarted;

                if (useTofAndColorBefore)
                {
                    Instance.WasPaused = true;
                }

                Instance.ColorStarted = false;
                Instance.TofStarted = false;

                this.CleaunpCacheFolder();
            }
            else
            {
                if (Instance.WasPaused)
                {
                    StartCoroutine(ResumeApp());
                }

                this.SetSensCordFilePath();
            }
        }

        private IEnumerator ResumeApp()
        {
            while (!Instance.ColorStarted || !Instance.TofStarted)
            {
                yield return null;
            }

            Instance.WasPaused = false;
        }


        DeviceOrientation previousDeviceOrientation = DeviceOrientation.LandscapeLeft;
        ScreenOrientation previousScreenOrientation = ScreenOrientation.LandscapeLeft;


        private void SetRotationProperty()
        {
            var deviceOrientation = Input.deviceOrientation;
            var screenOrientation = Screen.orientation;

            bool screenOrientationChanged = currentOrientationProperty.screenOrientation != screenOrientation;
            bool deviceOrientationChanged = currentOrientationProperty.deviceOrientation != deviceOrientation;

            if (screenOrientationChanged)
            {
                previousScreenOrientation = currentOrientationProperty.screenOrientation;

                currentOrientationProperty.screenOrientation = screenOrientation;

                SetProperty<DeviceOrientationsProperty>(currentOrientationProperty);

                this.screenOrientationUpdated = true;

            }

            if (deviceOrientationChanged)
            {
                previousDeviceOrientation = currentOrientationProperty.deviceOrientation;

                currentOrientationProperty.deviceOrientation = deviceOrientation;

                SetProperty<DeviceOrientationsProperty>(currentOrientationProperty);

                this.deviceOrientationUpdated = true;
            }

        }



        private void GetRotationProperty()
        {
            if (!this.connectionExists)
            {
                return;
            }
            if (this.RuntimeSettings.runMode == RunMode.Default)
            {
                if (this.deviceOrientationUpdated || this.screenOrientationUpdated)
                {
                    var orientationProperty = GetProperty<DeviceOrientationsProperty>();

                    var deviceOrientation = orientationProperty.deviceOrientation;
                    var screenOrientation = orientationProperty.screenOrientation;

                    // invoke
                    if (this.screenOrientationUpdated && screenOrientation == currentOrientationProperty.screenOrientation)
                    {
                        this.screenOrientationUpdated = false;

                        OnScreenOrientationUpdated?.Invoke(previousScreenOrientation, screenOrientation);
                    }
                    if (this.deviceOrientationUpdated && deviceOrientation == currentOrientationProperty.deviceOrientation)
                    {
                        this.deviceOrientationUpdated = false;

                        OnDeviceOrientationUpdated?.Invoke(previousDeviceOrientation, deviceOrientation);
                    }
                }
            }
            else
            {
                try
                {
                    var orientationProperty = GetProperty<DeviceOrientationsProperty>();

                    var deviceOrientation = orientationProperty.deviceOrientation;
                    var screenOrientation = orientationProperty.screenOrientation;

                    if (deviceOrientation != currentOrientationProperty.deviceOrientation)
                    {
                        var oldDeviceOrientation = currentOrientationProperty.deviceOrientation;
                        currentOrientationProperty.deviceOrientation = deviceOrientation;

                        OnDeviceOrientationUpdated?.Invoke(oldDeviceOrientation, deviceOrientation);
                    }

                    if (screenOrientation != currentOrientationProperty.screenOrientation)
                    {
                        var oldScreenOrientation = currentOrientationProperty.screenOrientation;
                        currentOrientationProperty.screenOrientation = screenOrientation;

                        OnScreenOrientationUpdated?.Invoke(oldScreenOrientation, screenOrientation);
                    }
                }
                catch (ApiException e)
                {
                    Logger.WriteLog(LogLevel.Debug, "Error: " + e.Message + " - " + e.Cause);
                    this.connectionExists = false;
                }
            }
        }


        //for unit tests
        /// <summary>
        /// 強制的の画面方向と端末方向のキャッシュを更新する
        /// </summary>
        public void ForceUpdateDeviceOrientation()
        {
            currentOrientationProperty.screenOrientation = ScreenOrientation.AutoRotation;
            currentOrientationProperty.deviceOrientation = DeviceOrientation.Unknown;
        }

        /// <summary>
        /// *TODO+ B
        /// デバイス回転を取得する
        /// </summary>
        /// <returns>
        /// デバイス回転方向(単位:度)を下記値で返却
        /// <list type="bullet">
        /// <item><description>0 : DeviceOrientation.LandscapeLeft</description></item>
        /// <item><description>90 : DeviceOrientation.PortraitUpsideDown</description></item>
        /// <item><description>180 : DeviceOrientation.LandscapeRight</description></item>
        /// <item><description>270 : DeviceOrientation.Portrait</description></item>
        /// </list>
        /// </returns>
        [Obsolete("use the TofArManager.GetDeviceOrientation() instead")]
        internal int GetDeviceImageRotation()
        {
            return this.GetDeviceOrientation();
        }

        /// <summary>
        /// デバイス回転を取得する
        /// </summary>
        /// <returns>
        /// デバイス回転方向(単位:度)を下記値で返却
        /// <list type="bullet">
        /// <item><description>0 : DeviceOrientation.LandscapeLeft</description></item>
        /// <item><description>90 : DeviceOrientation.PortraitUpsideDown</description></item>
        /// <item><description>180 : DeviceOrientation.LandscapeRight</description></item>
        /// <item><description>270 : DeviceOrientation.Portrait</description></item>
        /// </list>
        /// </returns>
        public int GetDeviceOrientation()
        {
            DeviceOrientation currentOrientation = currentOrientationProperty.deviceOrientation;
            if (currentOrientation == DeviceOrientation.Unknown)
            {
                if (this.RuntimeSettings.runMode == RunMode.Default)
                {
                    SetRotationProperty();
                }
                var orientationProperty = this.Stream?.GetProperty<DeviceOrientationsProperty>();
                if (orientationProperty != null)
                {
                    currentOrientation = orientationProperty.deviceOrientation;
                    currentOrientationProperty = orientationProperty;
                }
            }

            switch (currentOrientation)
            {
                case DeviceOrientation.Portrait:
                    this.deviceOrientation = 270;
                    break;
                case DeviceOrientation.PortraitUpsideDown:
                    this.deviceOrientation = 90;
                    break;
                case DeviceOrientation.LandscapeLeft:
                    this.deviceOrientation = 0;
                    break;
                case DeviceOrientation.LandscapeRight:
                    this.deviceOrientation = 180;
                    break;
            }

            return this.deviceOrientation;
        }

        /// <summary>
        /// *TODO+ B
        /// 画面方向を取得する
        /// </summary>
        /// <returns>
        /// 画面方向(単位:度)を下記値で返却
        /// <list type="bullet">
        /// <item><description>0 : ScreenOrientation.LandscapeLeft</description></item>
        /// <item><description>90 : ScreenOrientation.PortraitUpsideDown</description></item>
        /// <item><description>180 : ScreenOrientation.LandscapeRight</description></item>
        /// <item><description>270 : ScreenOrientation.Portrait</description></item>
        /// </list>
        /// </returns>
        [Obsolete("use the TofArManager.GetScreenOrientation() instead")]
        internal int GetScreenImageRotation()
        {
            return this.GetScreenOrientation();
        }

        /// <summary>
        /// 画面方向を取得する
        /// </summary>
        /// <returns>
        /// 画面方向(単位:度)を下記値で返却
        /// <list type="bullet">
        /// <item><description>0 : ScreenOrientation.LandscapeLeft</description></item>
        /// <item><description>90 : ScreenOrientation.PortraitUpsideDown</description></item>
        /// <item><description>180 : ScreenOrientation.LandscapeRight</description></item>
        /// <item><description>270 : ScreenOrientation.Portrait</description></item>
        /// </list>
        /// </returns>
        public int GetScreenOrientation()
        {
            ScreenOrientation currentOrientation;
            if (currentOrientationProperty != null)
            {
                currentOrientation = currentOrientationProperty.screenOrientation;
                if (currentOrientation == (ScreenOrientation)0)
                {
                    if (this.RuntimeSettings.runMode == RunMode.Default)
                    {
                        SetRotationProperty();
                    }

                    currentOrientationProperty = this.Stream?.GetProperty<DeviceOrientationsProperty>();
                    if (currentOrientationProperty != null)
                    {
                        currentOrientation = currentOrientationProperty.screenOrientation;
                    }
                }
            }
            else
            {
                currentOrientation = Screen.orientation;
            }

            switch (currentOrientation)
            {
                case ScreenOrientation.Portrait:
                    this.screenOrientation = 270;
                    break;
                case ScreenOrientation.PortraitUpsideDown:
                    this.screenOrientation = 90;
                    break;
                case ScreenOrientation.LandscapeLeft:
                    this.screenOrientation = 0;
                    break;
                case ScreenOrientation.LandscapeRight:
                    this.screenOrientation = 180;
                    break;
            }

            return this.screenOrientation;
        }

        #region DeviceProfiles

        private void CheckDeviceProfiles()
        {
            string commonPath = "data/local/tmp/tofar/";
            string configPath = commonPath + "/config";
            string jsonPath = $"{configPath}/{DeviceAttributesFileName}";

            var modelName = this.GetModelName();
            string modelGroupName = GetModelGroupName(modelName);

            // Read from /data/local/tmp/tofar
            string jsonContentTmp = GetProfileFromTmp(jsonPath);
            string jsonContentResources = "";
            string modelGroupNameTmp;
            System.Version profileVersionTmp;
            GetProfileNameAndVersion(jsonContentTmp, out modelGroupNameTmp, out profileVersionTmp);

            if (profileVersionTmp == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"No profile found inside /data/local/tmp");
            }

            // Check profiles inside Resources
            if (modelGroupName.Equals("") && modelGroupNameTmp != null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Set model group name to {modelGroupNameTmp}");
                modelGroupName = modelGroupNameTmp;
            }

            TextAsset latestProfile = null;
            System.Version profileVersionLatest = null;

            GetLatestProfile(modelGroupName, out latestProfile, out profileVersionLatest);

            if (latestProfile != null)
            {
                jsonContentResources = latestProfile.text;
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Latest profile: {latestProfile.name} - v{profileVersionLatest.ToString()}");
            }

            if (profileVersionLatest == null && profileVersionTmp == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"No (valid) profiles inside resources as well as inside tmp path");
                return;
            }

            bool rewriteConfig = true;

            if (profileVersionTmp != null && profileVersionLatest != null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Comparing current version {profileVersionTmp.ToString()} to latest version {profileVersionLatest.ToString()}");
                // don't rewrite if latest version is 'older' or equal to current version 
                rewriteConfig = profileVersionLatest.CompareTo(profileVersionTmp) > 0;
            }
            else if (profileVersionTmp != null)
            { // nothing inside profiles, but something inside common
                rewriteConfig = false;
            }

            if (!rewriteConfig)
            {
                // compare with tmp file
                if (profileVersionTmp != null)
                {
                    currentDeviceCapability = new DeviceCapabilityProperty()
                    {
                        deviceAttributesString = jsonContentTmp,
                        modelName = modelName,
                        modelGroupName = modelGroupName
                    };

                    configSource = ConfigSource.LocalFile;

                    return;

                }
            }

            // json file not found or too old
            TofArManager.Logger.WriteLog(LogLevel.Debug, "Profile not found inside common path or too old. Using latest one from resources.");


            bool configPathExists = CreateConfigDirectories(commonPath, configPath);


            // save to %COMMON_PATH%/tofar_device_attributes.json (create path if necessary)
            // Only projects with external write permission can save the file
            if (configPathExists)
            {
                WriteConfigToStorage(jsonPath, jsonContentResources);
            }

            currentDeviceCapability = new DeviceCapabilityProperty()
            {
                deviceAttributesString = jsonContentResources,
                modelName = modelName,
                modelGroupName = modelGroupName
            };

            configSource = ConfigSource.Default;
        }

        private void WriteConfigToStorage(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Successfully written to {path}");
            }
            catch (IOException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to write json file to {path}\n{e.Message}");
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to write json file to {path}\n{e.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to write json file to {path}\n{e.Message}");
            }
            catch (NotSupportedException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to write json file to {path}\n{e.Message}");
            }
        }

        private bool CreateConfigDirectories(string commonPath, string configPath)
        {
            bool configPathExists = false;

            try
            {
                if (!Directory.Exists(commonPath))
                {
                    Directory.CreateDirectory(commonPath);
                }

                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }

                configPathExists = true;
            }
            catch (IOException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to create directiories: {e.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to create directiories: {e.Message}");
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to create directiories: {e.Message}");
            }
            catch (NotSupportedException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to create directiories: {e.Message}");
            }

            return configPathExists;
        }

        private void GetLatestProfile(string modelGroupName, out TextAsset latestProfile, out System.Version profileVersionLatest)
        {
            latestProfile = null;
            profileVersionLatest = null;

            if (!modelGroupName.Equals(""))
            {
                var profiles = Resources.LoadAll("DeviceProfiles", typeof(TextAsset));

                foreach (var profile in profiles)
                {
                    var t = (TextAsset)profile;
                    var groups = System.Text.RegularExpressions.Regex.Match(t.name, @"(.*)_v\d{1,3}.\d{1,3}.\d{1,3}").Groups;
                    if (groups != null && groups.Count > 1 && groups[1].Value.Equals(modelGroupName))
                    {
                        System.Version profileVersion;
                        string profileName;
                        GetProfileNameAndVersion(t.text, out profileName, out profileVersion);

                        if ((profileVersion != null)
                            && (profileVersionLatest == null || profileVersion.CompareTo(profileVersionLatest) > 0))
                        {
                            latestProfile = t;
                            profileVersionLatest = profileVersion;
                        }
                    }
                }
            }
        }

        private string GetModelGroupName(string modelName)
        {
            if (modelName == null)
            {
                return "";
            }

            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Model name is {modelName}");

            var modelgroup = Resources.Load("DeviceProfiles/modelgroup") as TextAsset;
            if (modelgroup != null)
            {
                string[] content = modelgroup.text.Split('\n');

                foreach (string line in content)
                {

                    string[] keyValuePair = System.Text.RegularExpressions.Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                    if (keyValuePair.Length > 1)
                    {
                        string currentModelName = keyValuePair[1].Trim('\r', '\n', '\"');
                        if (currentModelName.Equals(modelName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            string modelGroupName = keyValuePair[0];

                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Found {modelGroupName} - {currentModelName}");

                            return modelGroupName;
                        }
                    }
                }
                // get key that has model name as value
            }

            TofArManager.Logger.WriteLog(LogLevel.Debug, "Device might not be supported");

            return "";

        }

        private string GetModelName()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (this.currentDeviceCapability != null)
                {
                    return this.currentDeviceCapability.modelName;
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                try
                {
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        AndroidJavaClass build = new AndroidJavaClass("android.os.Build");

                        string modelName = build.GetStatic<string>("MODEL");

                        return modelName;
                    }
                }
                catch (AndroidJavaException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to get model name\nReason: {e.Message}");
                }
            }

            return null;
        }

        private void GetProfileNameAndVersion(string profileContent, out string name, out System.Version version)
        {
            version = null;
            name = null;

            if (profileContent == null || profileContent.Equals(""))
            {
                return;
            }

            var attributes = JsonUtility.FromJson<deviceVersionJson>(profileContent.Trim(' ', '[', ']', '\n', '\r'));

            if (attributes != null)
            {
                string versionEntry = attributes.version;

                if (versionEntry != null && !versionEntry.Equals(""))
                {
                    string[] tokens = versionEntry.Split(new string[] { "_v" }, StringSplitOptions.None);

                    if (tokens.Length > 0)
                    {
                        string versionStr = tokens[tokens.Length - 1];

                        try
                        {
                            version = new System.Version(versionStr);

                            string[] tokensName = new string[tokens.Length - 1];
                            Array.Copy(tokens, tokensName, tokens.Length - 1);

                            name = String.Join("", tokensName);
                        }
                        catch (ArgumentException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to get version from {versionStr}\n{e.Message}");
                        }
                        catch (FormatException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to get version from {versionStr}\n{e.Message}");
                        }
                        catch (OverflowException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to get version from {versionStr}\n{e.Message}");
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to get version from {versionStr}\n{e.Message}");
                        }
                    }
                }
            }
        }

        private string GetProfileFromTmp(string jsonTmp)
        {
            string content = "";

            if (File.Exists(jsonTmp))
            {
                if (Utils.PathIsSymlink(jsonTmp))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "JSON file " + jsonTmp + "was a symlink");
                }
                else
                {
                    content = File.ReadAllText(jsonTmp);
                }
            }

            return content;
        }

        private ConfigSource configSource = ConfigSource.None;

        /// <summary>
        /// 端末固有設定ロード元を取得する
        /// </summary>
        /// <returns>端末固有設定ロード元</returns>
        public ConfigSource GetConfigSource()
        {
            return configSource;
        }
        #endregion

        private bool fpsDumpEnabled = false;
        private const string fpsDumpSettingsFileName = "tofar_fpsdump_enabled";
        private const string fpsDumpFileName = "tofar_fpsdump";
        private object fpsDumpLock = new object();

        private void InitializeFpsDump()
        {
            var path = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{fpsDumpSettingsFileName}";
            this.fpsDumpEnabled = File.Exists(path);
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"fpsDumpSettingsFile:{path} fpsDumpEnabled:{this.fpsDumpEnabled}");
            this.StartCoroutine(this.FpsDump());
        }

        private IEnumerator FpsDump()
        {
            if (!this.fpsDumpEnabled)
            {
                yield break;
            }

            var outputFileName = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{fpsDumpFileName}_{DateTime.Now:yyyyMMddHHmmss.fff}.csv";
            using (var outputFile = File.CreateText(outputFileName))
            {
                var targets = new string[]
                {
                    "TofArBodyManager", "TofArColorManager", "TofArFaceManager", "TofArHandManager",
                    "TofArMeshManager", "TofArModelingManager", "TofArPlaneManager", "TofArSegmentationManager",
                    "TofArSlamManager", "TofArTofManager"
                };
                outputFile.Write("Timestamp");
                outputFile.Write(",App");
                foreach (var target in targets)
                {
                    outputFile.Write($",{target}");
                }
                outputFile.Write("\n");

                var fromLastOutput = 0f;
                var appFrameCount = 0;
                var outputInterval = 1.0f;
                while (this.fpsDumpEnabled)
                {
                    if (fromLastOutput > outputInterval)
                    {
                        lock (this.fpsDumpLock)
                        {
                            outputFile.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                            outputFile.Write($",{appFrameCount / fromLastOutput:0.000}");
                            foreach (var target in targets)
                            {
                                var fps = 0f;
                                foreach (IStreamStoppable manager in this.SubManagers)
                                {
                                    var typeName = manager.GetType().Name;
                                    if (typeName == target)
                                    {
                                        fps = manager.FrameRate;
                                        break;
                                    }
                                }
                                outputFile.Write($",{fps:0.000}");
                            }
                            outputFile.Write("\n");
                            outputFile.Flush();

                            fromLastOutput = 0f;
                            appFrameCount = 0;
                        }
                    }
                    fromLastOutput += Time.unscaledDeltaTime;
                    appFrameCount++;

                    yield return null;
                }
            }
        }
    }
}

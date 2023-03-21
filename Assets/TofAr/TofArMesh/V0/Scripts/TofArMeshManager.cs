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
using System.Collections.Generic;
using SensCord;
using UnityEngine;


namespace TofAr.V0.Mesh
{
    /// <summary>
    /// TofAr Meshコンポーネントとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>Mesh生成パラメータの設定</description></item>
    /// <item><description>Meshデータの取得</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>録画ファイルの再生</description></item>
    /// </list>
    /// </summary>
    public class TofArMeshManager : Singleton<TofArMeshManager>, IDisposable, IStreamStoppable, IDependManager
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
        public const string StreamKey = "tofar_mesh_camera2_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にMeshデータのストリームを開始する
        /// </summary>
        public bool autoStart = false;

        [SerializeField]
        private int meshReductionLevel = 0;

        /// <summary>
        /// Meshリダクションレベル
        /// <para>0:リダクションなし</para>
        /// <para>1～:リダクション実施</para>
        /// <para>デフォルト=0</para>
        /// </summary>
        public int MeshReductionLevel
        {
            get { return meshReductionLevel; }
            set
            {
                this.meshReductionLevel = value;
                var algorithmConfigProperty = GetProperty<AlgorithmConfigProperty>();
                algorithmConfigProperty.meshReductionLevel = this.meshReductionLevel;
                SetProperty(algorithmConfigProperty);
            }
        }

        [SerializeField]
        private int resetTrianglePeriod = 4;

        /// <summary>
        /// トライアングル計算の間隔フレーム数
        /// <para>デフォルト=4</para>
        /// </summary>
        public int ResetTrianglePeriod
        {
            get { return resetTrianglePeriod; }
            set
            {
                this.resetTrianglePeriod = value;
                var algorithmConfigProperty = GetProperty<AlgorithmConfigProperty>();
                algorithmConfigProperty.resetTrianglePeriod = this.resetTrianglePeriod;
                SetProperty(algorithmConfigProperty);
            }
        }

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
                        this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArMeshManager.StreamKey);

                        this.Stream?.RegisterFrameCallback(StreamCallBack);

                        TofAr.V0.Tof.TofArTofManager.Instance?.AddManagerDependency(this);
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


        private bool useMeshDataA = true;
        private MeshData meshDataA, meshDataB;

        /// <summary>
        /// 最新のMeshデータ
        /// </summary>
        public MeshData MeshData { get; private set; }

        /// <summary>
        /// ストリーミング開始のデリゲート
        /// </summary>
        /// <param name="sender"></param>
        public delegate void StreamStartedEventHandler(object sender);

        /// <summary>
        /// ストリーミング開始通知
        /// </summary>
        public static event StreamStartedEventHandler OnStreamStarted;

        /// <summary>
        /// Obsolete("Using the static OnStreamStarted is recommended, which is more stable than StreamStarted") *TODO+ B
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
        /// <param name="sender"></param>
        public delegate void StreamStoppedEventHandler(object sender);

        /// <summary>
        /// ストリーミング終了通知
        /// </summary>
        public static event StreamStoppedEventHandler OnStreamStopped;

        /// <summary>
        /// Obsolete("Using the static OnStreamStopped is recommended, which is more stable than StreamStopped") *TODO+ B
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
        /// <param name="sender"></param>
        public delegate void FrameArrivedEventHandler(object sender);

        /// <summary>
        /// 新しいフレームの到着通知
        /// </summary>
        public static event FrameArrivedEventHandler OnFrameArrived;

        /// <summary>
        /// Obsolete("Using the static OnFrameArrived is recommended, which is more stable than FrameArrived") *TODO+ B
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

        private bool waitForTof = false;
        private AlgorithmConfigProperty startConfig = null;
        private bool checkForTof = true;

        private void Start()
        {
            TofArManager.Instance.AddSubManager(this);

            if (this.autoStart)
            {
                this.StartCoroutine(this.StartProcess());
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
        /// 破棄時処理
        /// </summary>
        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArMeshManager.Dispose()");
            TofAr.V0.Tof.TofArTofManager.Instance?.RemoveManagerDependency(this);
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

        private void OnEnable()
        {
            TofAr.V0.Tof.TofArTofManager.OnStreamStarted += TofOnStreamStarted;
        }

        private void OnDisable()
        {
            TofAr.V0.Tof.TofArTofManager.OnStreamStarted -= TofOnStreamStarted;
        }

        private void TofOnStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, Tof.PointCloudData pointCloudData)
        {
            // restart stream, if mesh was already running
            if (this.IsStreamActive)
            {
                this.checkForTof = false;
                var currentConfig = GetProperty<AlgorithmConfigProperty>();
                StopStream();
                StartStream(currentConfig);
            }
            else if (this.waitForTof)
            {
                this.checkForTof = false;
                this.waitForTof = false;
                StartStream(startConfig);

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
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="configuration">ストリーミングに使用するMesh生成パラメータ</param>
        public void StartStream(AlgorithmConfigProperty configuration)
        {
            if (!this.IsStreamActive && this.isUnPaused)
            {
                if (this.checkForTof)
                {
                    bool isTofStarting = Tof.TofArTofManager.Instance?.IsTofStarting == true;
                    if (isTofStarting)
                    {
                        startConfig = configuration;
                        this.waitForTof = true;
                        return;
                    }
                }
                this.checkForTof = true;

                if (this.streamPlay != null && this.streamPlay.IsStarted)
                {
                    this.StopPlayback();
                }
                if (configuration != null)
                {
                    this.SetProperty(configuration);
                }

                this.Stream?.Start();

                if (this.startingPlayback)
                {
                    this.IsPlaying = true;
                }

                if (OnStreamStarted != null)
                {
                    OnStreamStarted(this);
                }
            }
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        public void StartStream()
        {
            if (Tof.TofArTofManager.Instance.ProcessTargets?.processPointCloud == false)
            {
                TofArManager.Logger.WriteLog(LogLevel.Info, "Process point cloud is disabled. Mesh won't be processed.");
            }
            this.StartStream(null);
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
                this.Stream?.Stop();
                if (OnStreamStopped != null)
                {
                    OnStreamStopped((sender == null) ? this : sender);
                }

                this.meshDataA = null;
                this.meshDataB = null;
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

        private List<IPreProcessMeshData> preProcessors = new List<IPreProcessMeshData>();

        /// <summary>
        /// Meshデータ送出前の処理を登録する
        /// </summary>
        /// <param name="preProcessMesh">Meshデータ処理クラス</param>
        public void RegisterMeshPreProcessing(IPreProcessMeshData preProcessMesh)
        {
            preProcessors.Add(preProcessMesh);
        }

        /// <summary>
        /// Meshデータ送出前の処理を登録解除する
        /// </summary>
        /// <param name="preProcessMesh">Meshデータ処理クラス</param>
        public void UnregisterMeshPreProcessing(IPreProcessMeshData preProcessMesh)
        {
            preProcessors.Remove(preProcessMesh);
        }

        static private void StreamCallBack(Stream stream)
        {
            try
            {
                lock (Instance.stopLock)
                {
                    if (!TofArMeshManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    var frame = stream.GetFrame();

                    try
                    {
                        var meshChannel = frame.Channels[(int)ChannelIds.Mesh];
                        var rawData = meshChannel.GetRawData();

                        if (instance.useMeshDataA)
                        {
                            if (instance.meshDataA == null)
                            {
                                instance.meshDataA = new MeshData(rawData);
                            }
                            else
                            {
                                instance.meshDataA.UpdateFromRawData(rawData);
                            }
                            instance.MeshData = instance.meshDataA;
                            instance.useMeshDataA = false;
                        }
                        else
                        {
                            if (instance.meshDataB == null)
                            {
                                instance.meshDataB = new MeshData(rawData);
                            }
                            else
                            {
                                instance.meshDataB.UpdateFromRawData(rawData);
                            }
                            instance.MeshData = instance.meshDataB;
                            instance.useMeshDataA = true;
                        }

                        foreach (var processor in instance.preProcessors)
                        {
                            var updatedMesh = processor.ParseMeshData(new MeshDataSet { vertexes = instance.MeshData.vertices, triangles = instance.MeshData.trianglesBuffer, resetTriangle = instance.MeshData.resetTriangle });
                            instance.MeshData.resetTriangle = updatedMesh.resetTriangle;
                            instance.MeshData.vertices = updatedMesh.vertexes;
                            instance.MeshData.trianglesBuffer = updatedMesh.triangles;
                            instance.MeshData.trianglesCount = updatedMesh.triangles.Length / 3;
                            instance.MeshData.verticesCount = updatedMesh.vertexes.Length / 3;
                        }
                        instance.frameCount++;
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
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }*/
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
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (this.isUnPaused)
            {
                if (value is FrameRateProperty) // Don't set framerate property from outside to avoid accidental changes
                {
                    return; // Throw exception? 
                }
                else if (value is RecordProperty)
                {
                    if ((value as RecordProperty).Enabled)
                    {
                        this.Stream?.SetProperty(new FrameRateProperty() { Num = 30 });
                    }
                    else
                    {
                        this.Stream?.SetProperty(new FrameRateProperty() { Num = 1 });
                    }
                }

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

        #region PLAYBACK

        /// <summary>
        /// *TODO+ B
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_mesh_stream";

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
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArMeshManager.streamPlayerKey);

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
            Instance.Stream.SetProperty(new SensCord.FrameRateProperty() { Num = 15 }); // tells component to open tof playback stream
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
            Instance.StreamPlay.SetProperty<PlayProperty>(play);

            for (int t = 0; t < tryCount; t++)
            {
                var stream = Instance.StreamPlay;

                stream.SetProperty<PlayProperty>(play);

                try
                {
                    stream.GetProperty<AlgorithmConfigProperty>();
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

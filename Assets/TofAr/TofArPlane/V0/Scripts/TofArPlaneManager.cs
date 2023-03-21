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
using TofAr.V0.Tof;
using UnityEngine;

namespace TofAr.V0.Plane
{
    /// <summary>
    /// TofAr Planeコンポーネントとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>平面検出パラメータの設定</description></item>
    /// <item><description>平面検出データの取得</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>平面検出基準点追加</description></item>
    /// <item><description>平面検出基準点削除</description></item>
    /// <item><description>録画ファイルの再生</description></item>
    /// </list>
    /// </summary>
    public class TofArPlaneManager : Singleton<TofArPlaneManager>, IDisposable, IStreamStoppable, IDependManager
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
        /// *TODO+ B StreamKey は元々のReferenceには無い。Publicなのか要確認
        /// ストリームキー
        /// </summary>
        public const string StreamKey = "tofar_plane_camera2_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にPlaneデータのストリームを開始する
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
                        this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArPlaneManager.StreamKey);
                        this.Stream?.RegisterFrameCallback(StreamCallBack);
                        TofArTofManager.Instance?.AddManagerDependency(this);
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

        /// <summary>
        /// 実測FPS
        /// </summary>
        public float FrameRate { get; private set; }
        private int frameCount = 0;
        private float fromFpsMeasured = 0f;

        private bool IsStreamPausing { get; set; }

        /// <summary>
        /// 最新のPlaneデータ
        /// </summary>
        public PlaneData PlaneData { get; private set; }

        private bool waitForTof = false;
        private AlgorithmConfigProperty startConfig = null;
        private bool checkForTof = true;

        /// <summary>
        /// ストリーミング開始時デリゲート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="config"></param>
        public delegate void StreamStartedEventHandler(object sender, AlgorithmConfigProperty config);

        /// <summary>
        /// ストリーミング開始通知
        /// </summary>
        public static event StreamStartedEventHandler OnStreamStarted;

        /// <summary>
        /// *TODO+ B Obsolete("Using the static OnStreamStarted is recommended, which is more stable than StreamStarted")
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
        /// *TODO+ B Obsolete("Using the static OnStreamStopped is recommended, which is more stable than StreamStopped")
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
        /// *TODO+ B Obsolete("Using the static OnFrameArrived is recommended, which is more stable than FrameArrived")
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
        /// 平面検出基準点追加時デリゲート
        /// </summary>
        /// <param name="idx"></param>
        public delegate void PlaneAddedEventHandler(int idx);

        /// <summary>
        /// 平面検出基準点追加通知
        /// </summary>
        public static event PlaneAddedEventHandler OnPlaneAdded;

        /// <summary>
        /// *TODO+ B Obsolete("PlaneAdded is deprecated, please use OnPlaneAdded")
        /// 平面検出基準点追加通知
        /// </summary>
        [Obsolete("PlaneAdded is deprecated, please use OnPlaneAdded")]
        private event PlaneAddedEventHandler PlaneAdded
        {
            add { OnPlaneAdded += value; }
            remove { OnPlaneAdded -= value; }
        }

        /// <summary>
        /// 平面検出基準点削除時デリゲート
        /// </summary>
        /// <param name="idx"></param>
        public delegate void PlaneRemovedEventHandler(int idx);

        /// <summary>
        /// 平面検出基準点削除通知
        /// </summary>
        public static event PlaneRemovedEventHandler OnPlaneRemoved;

        /// <summary>
        /// *TODO+ B Obsolete("PlaneRemoved is deprecated, please use OnPlaneRemoved")
        /// 平面検出基準点削除通知
        /// </summary>
        [Obsolete("PlaneRemoved is deprecated, please use OnPlaneRemoved")]
        private event PlaneRemovedEventHandler PlaneRemoved
        {
            add { OnPlaneRemoved += value; }
            remove { OnPlaneRemoved -= value; }
        }

        private void Start()
        {
            TofArManager.Instance.AddSubManager(this);

            if (this.autoStart)
            {
                this.StartCoroutine(this.StartProcess());
            }
        }

        /// <summary>
        /// *TODO+ B StartProcess は元々のReferenceには無い。privateの間違いでは?
        /// </summary>
        /// <returns>*TODO+ B</returns>
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

            Instance.StartStream(null);
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            TofArTofManager.Instance?.RemoveManagerDependency(this);
            TofArManager.Instance?.RemoveSubManager(this);
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArPlaneManager.Dispose()");

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
            // restart stream, if plane was already running
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

        /// <summary>
        /// 平面検出基準点を追加する
        /// </summary>
        public void AddPlane()
        {
            var algorithmConfigs = GetProperty<AlgorithmConfigsProperty>();

            int nConfigs = algorithmConfigs.configurations.Length + 1;

            if (nConfigs <= 8)
            {
                var configurations = new AlgorithmConfigProperty[nConfigs];

                algorithmConfigs.configurations.CopyTo(configurations, 0);

                configurations[nConfigs - 1] = new AlgorithmConfigProperty();

                algorithmConfigs.configurations = configurations;

                SetProperty<AlgorithmConfigsProperty>(algorithmConfigs);

                if (OnPlaneAdded != null)
                {
                    OnPlaneAdded.Invoke(nConfigs - 1);
                }
            }

        }

        /// <summary>
        /// 平面検出基準点を削除する
        /// </summary>
        /// <param name="idx">削除する平面検出基準点</param>
        //public PlaneArrangement RemovePlane(PlaneArrangement plane)
        public void RemovePlane(int idx)
        {
            // remove configuration
            var algorithmConfigs = GetProperty<AlgorithmConfigsProperty>();
            //don't remove a plane if the index is out of range
            if (idx < 0 || idx >= algorithmConfigs.configurations.Length)
            {
                return;
            }
            int nConfigs = algorithmConfigs.configurations.Length - 1;

            var configurations = new AlgorithmConfigProperty[nConfigs];

            if (idx > 0)
            {
                Array.Copy(algorithmConfigs.configurations, 0, configurations, 0, idx);
            }
            if (idx < nConfigs)
            {
                Array.Copy(algorithmConfigs.configurations, idx + 1, configurations, idx, algorithmConfigs.configurations.Length - (idx + 1));
            }
            algorithmConfigs.configurations = configurations;

            // Set new configuration list
            SetProperty<AlgorithmConfigsProperty>(algorithmConfigs);

            if (OnPlaneRemoved != null)
            {
                OnPlaneRemoved.Invoke(idx);
            }
        }

        /// <summary>
        /// 全ての平面を削除する
        /// </summary>
        public void ClearPlanes()
        {
            var configurations = new AlgorithmConfigsProperty { configurations = new AlgorithmConfigProperty[0] };
            SetProperty(configurations);
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

        private List<IPreProcessPlaneData> preProcessors = new List<IPreProcessPlaneData>();

        /// <summary>
        /// Planeデータ送出前の処理を登録する
        /// </summary>
        /// <param name="preProcessTof"></param>
        public void RegisterPlanePreProcessing(IPreProcessPlaneData preProcessTof)
        {
            preProcessors.Add(preProcessTof);
        }

        /// <summary>
        /// Planeデータ送出前の処理を登録解除する
        /// </summary>
        /// <param name="preProcessTof"></param>
        public void UnregisterPlanePreProcessing(IPreProcessPlaneData preProcessTof)
        {
            preProcessors.Remove(preProcessTof);
        }


        private void StreamCallBack(Stream stream)
        {
            try
            {
                lock (Instance.stopLock)
                {
                    if (!TofArPlaneManager.Instantiated)
                    {
                        return;
                    }

                    var frame = stream.GetFrame();

                    try
                    {
                        var channel = frame.Channels[(int)ChannelIds.Plane];
                        var rawData = channel.GetRawData();
                        PlaneData = new PlaneData(rawData);
                        foreach (var prepro in preProcessors)
                        {
                            PlaneData.Data = prepro.ProcessPlaneData(PlaneData.Data);
                        }
                    }
                    finally
                    {
                        frame.Dispose();
                    }
                    if (OnFrameArrived != null)
                    {
                        OnFrameArrived(Instance);
                    }
                    Instance.frameCount++;
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
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="config">ストリーミングに使用する平面検出パラメータ</param>
        public void StartStream(AlgorithmConfigProperty config)
        {
            if (this.checkForTof)
            {
                bool isTofStarting = Tof.TofArTofManager.Instance?.IsTofStarting == true;
                if (isTofStarting)
                {
                    startConfig = config;
                    this.waitForTof = true;
                    return;
                }
            }
            this.checkForTof = true;

            if (!this.IsStreamActive && this.isUnPaused)
            {
                if (this.streamPlay != null && this.streamPlay.IsStarted)
                {
                    this.StopPlayback();
                }
                var algorithmConfigs = new AlgorithmConfigsProperty()
                {
                    configurations = new AlgorithmConfigProperty[0]
                };

                SetProperty<AlgorithmConfigsProperty>(algorithmConfigs);

                if (config != null)
                {
                    this.SetProperty(config);
                }
                if (Tof.TofArTofManager.Instance.ProcessTargets?.processPointCloud == false)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Info, "Process point cloud is disabled. Plane won't be processed.");
                }
                this.Stream?.Start();

                if (this.startingPlayback)
                {
                    this.IsPlaying = true;
                }

                if (OnStreamStarted != null)
                {
                    OnStreamStarted(this, config);
                }
            }
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        public void StartStream()
        {
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
                int bufferSize = 1024;
                //handle types that need a larger buffer
                if (property is AlgorithmConfigsProperty)
                {
                    bufferSize = 4096;
                }
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(property.Key, ref property, bufferSize);
                return property;
            }
            return null;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する
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
        /// シリアライズ用バッファサイズを指定してコンポーネントプロパティを取得する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (this.isUnPaused || this.IsPlaying)
            {
                int bufferSize = 1024;
                if (value is AlgorithmConfigsProperty)
                {
                    bufferSize = 4096;
                }
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
        /// *TODO+ B PauseStream はreferenceに記載がない （使われるのでpublicのままにする） 
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
        /// *TODO+ B UnpauseStream はreferenceに記載がない （使われるのでpublicのままにする）
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

        #region PLAYBACK

        /// <summary>
        /// *TODO+ B StreamPlayerKey はreferenceに記載がない
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_plane_stream";

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
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArPlaneManager.streamPlayerKey);

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
                if (stream.IsStarted)
                {
                    Instance.IsPlaying = true;

                    if (OnStreamStarted != null)
                    {
                        OnStreamStarted(this, null);
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

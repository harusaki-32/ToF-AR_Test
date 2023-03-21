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
using System.Runtime.InteropServices;
using MessagePack;
using SensCord;
using TofAr.V0.Color;
using TofAr.V0.Tof;
using UnityEngine;

namespace TofAr.V0.Segmentation
{
    /// <summary>
    /// TofAr Segmentationコンポーネントとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>Segmentationデータの取得</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>Segmentation推定結果通知</description></item>
    /// </list>
    /// </summary>
    public class TofArSegmentationManager : Singleton<TofArSegmentationManager>, IDisposable, IStreamStoppable, IDependManager
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
        public const string StreamKey = "tofar_segmentation_camera2_stream";
        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にSegmentationデータのストリームを開始する
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
                        this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArSegmentationManager.StreamKey);
                        this.Stream?.RegisterFrameCallback(StreamCallBack);
                        TofArColorManager.Instance?.AddManagerDependency(this);
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
        /// 最新のSegmentationデータ
        /// </summary>
        public SegmentationData SegmentationData { get; private set; }

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
        /// 新規フレームの到着時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        public delegate void FrameArrivedEventHandler(object sender);
        /// <summary>
        /// 新規フレーム到着通知
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
        //private int frameCount = 0;
        private float fromFpsMeasured = 0f;

        private Dictionary<string, int> frameCountPerSegmentation = new Dictionary<string, int>();


        /// <summary>
        /// Segmentation推定完了時デリゲート
        /// </summary>
        /// <param name="segmentationResults">Segmentation推定結果</param>
        public delegate void OnSegmentationEstimatedHandler(SegmentationResults segmentationResults);
        /// <summary>
        /// Segmentation推定結果通知
        /// </summary>
        public static event OnSegmentationEstimatedHandler OnSegmentationEstimated;

        private ResultHandlerProperty ResultHandler { get; set; } = null;
        private RuntimeSettingsProperty runtimeSettings = null;

        private bool waitForColor = false;
        private bool checkForColor = true;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate bool SegmentationResultHandlerDelegate(IntPtr segmentationResultsPointer, UInt64 bufferSize);

        private IEnumerator Start()
        {
            TofArManager.Instance.AddSubManager(this);

            if (this.autoStart)
            {
                var externalColorSource = Utils.FindFirstGameObjectThatImplements<Color.IExternalColorStream>(true);
                //wait for Color to start
                if (Color.TofArColorManager.Instance != null && (Color.TofArColorManager.Instance.autoStart || externalColorSource != null))
                {
                    while (!Color.TofArColorManager.Instance.IsStreamActive)
                    {
                        yield return null;
                    }
                }

                TofArSegmentationManager.Instance.StartStream();
            }
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                this.ResultHandler = this.GetProperty<ResultHandlerProperty>();
            }
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArSegmentationManager.Dispose()");
            TofArColorManager.Instance?.RemoveManagerDependency(this);
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
                    if (!TofArSegmentationManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    if (instance.runtimeSettings.isRemoteServer)
                    {
                        return;
                    }

                    var frame = stream.GetFrame();

                    try
                    {
                        if (frame.Channels.Length < (int)ChannelIds.Segmentation)
                        {
                            return;
                        }
                        var channel = frame.Channels[(int)ChannelIds.Segmentation];
                        var rawData = channel.GetRawData();

                        instance.SegmentationData = new SegmentationData(rawData);
                        if (OnSegmentationEstimated != null)
                        {
                            OnSegmentationEstimated(instance.SegmentationData.Data);
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
        private void OnEnable()
        {
            TofAr.V0.Color.TofArColorManager.OnStreamStarted += ColorOnStreamStarted;
        }

        private void OnDisable()
        {
            TofAr.V0.Color.TofArColorManager.OnStreamStarted -= ColorOnStreamStarted;
        }

        private void ColorOnStreamStarted(object sender, Texture2D colorTexture)
        {
            // restart stream, if segmentation was already running
            if (this.IsStreamActive)
            {
                this.checkForColor = false;
                StopStream();
                StartStream();
            }
            else if (this.waitForColor)
            {
                this.checkForColor = false;
                this.waitForColor = false;
                StartStream();
            }
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        public void StartStream()
        {
            //need to handle color satarting at the same time here
            this.runtimeSettings = TofArManager.Instance.RuntimeSettings;

            if (!this.IsStreamActive && this.isUnPaused)
            {
                if (this.checkForColor)
                {
                    bool isColorStarting = Color.TofArColorManager.Instance?.IsColorStarting == true;
                    if (isColorStarting)
                    {
                        this.waitForColor = true;
                        return;
                    }
                }
                this.checkForColor = true;

                if (this.streamPlay != null && this.streamPlay.IsStarted)
                {
                    this.StopPlayback();
                }
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
        /// <returns>Propertyのリスト</returns>
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
                float frameRateAvg = 0;
                int segmentationCount = 0;
                foreach (var frames in frameCountPerSegmentation.Values)
                {
                    if (frames > 0)
                    {
                        frameRateAvg += (frames / this.fromFpsMeasured);
                        segmentationCount++;
                    }
                }
                if (segmentationCount > 0)
                {
                    frameRateAvg /= segmentationCount;
                }
                this.FrameRate = frameRateAvg;
                this.fromFpsMeasured = 0;


                var keys = new List<string>(frameCountPerSegmentation.Keys);
                foreach (var key in keys)
                {
                    frameCountPerSegmentation[key] = 0;
                }
            }
        }

        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        /// <param name="result">TODO+ C</param>
        public void SetEstimatedResult(SegmentationResult result)
        {
            this.SetEstimatedResults(new SegmentationResults() { results = new SegmentationResult[] { result } });
        }



        /// <summary>
        /// TODO+ C 内部処理用
        /// </summary>
        /// <param name="results">TODO+ C</param>
        public void SetEstimatedResults(SegmentationResults results)
        {
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                if ((this.IsStreamActive || TofArManager.Instance.RuntimeSettings.isRemoteServer) &&
                    (this.ResultHandler != null) && (this.ResultHandler.handler != 0))
                {
                    var handler = Marshal.GetDelegateForFunctionPointer<SegmentationResultHandlerDelegate>(new IntPtr(this.ResultHandler.handler));

                    var bytes = MessagePackSerializer.Serialize<SegmentationResults>(results);
                    var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    var handlePtr = handle.AddrOfPinnedObject();
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
                    Instance.SegmentationData = new SegmentationData(results);

                    if (OnSegmentationEstimated != null)
                    {
                        OnSegmentationEstimated(Instance.SegmentationData.Data);
                    }
                }
            }

            if (results.results.Length > 0)
            {
                var resultName = results.results[0].name;

                if (!frameCountPerSegmentation.ContainsKey(resultName))
                {
                    frameCountPerSegmentation.Add(resultName, 0);
                }
                frameCountPerSegmentation[resultName]++;


            }

        }

        #region PLAYBACK

        /// <summary>
        /// *TODO+ B
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_segmentation_stream";

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
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArSegmentationManager.streamPlayerKey);

                    this.streamPlay?.RegisterFrameCallback(TofArSegmentationManager.StreamCallBack);
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
        /// trueの場合、再生中である
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
        }

        private bool isPlaying = false;
        private bool isPlayingWithTof = false;
        private bool startingPlayback = false;

        /// <summary>
        /// 再生を開始する
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
        /// 再生を開始する
        /// </summary>
        /// <param name="path">再生ファイルパス</param>
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

        /// <summary>
        /// 再生を終了する
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

            Instance.isPlaying = false;
        }

        #endregion
    }
}

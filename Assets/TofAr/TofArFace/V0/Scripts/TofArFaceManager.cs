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
using TofAr.V0.Color;
using UnityEngine;

namespace TofAr.V0.Face
{
    /// <summary>
    /// 顔の認識を管理する
    /// </summary>
    public class TofArFaceManager : Singleton<TofArFaceManager>, IDisposable, IStreamStoppable, IDependManager
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
        /// ストリームキー
        /// </summary>
        public const string StreamKey = "tofar_face_camera2_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にFaceデータのストリームを開始する
        /// </summary>
        public bool autoStart = false;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void StreamCallBackDelegate(IntPtr stream, IntPtr privateData);

        private bool isUnPaused = true;
        private bool IsStreamPausing { get; set; }
        private bool streamOpenErrorOccured = false;

        /// <summary>
        /// Faceデータ
        /// </summary>
        public FaceData FaceData { get; private set; }

        /// <summary>
        /// ストリーミング開始後デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        public delegate void StreamStartedEventHandler(object sender);

        /// <summary>
        /// ストリーミング開始通知
        /// </summary>
        public static event StreamStartedEventHandler OnStreamStarted;

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
        /// 新規フレーム到着時デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        public delegate void FrameArrivedEventHandler(object sender);

        /// <summary>
        /// 新しいフレームの到着通知
        /// <para>本コンポーネントではデバイス内で実行している時のみ発生する</para>
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
        /// Face推定結果デリゲート
        /// </summary>
        /// <param name="faceResults">Face認識結果</param>
        public delegate void OnFaceEstimatedHandler(FaceResults faceResults);

        /// <summary>
        /// Face推定結果通知
        /// </summary>
        public static event OnFaceEstimatedHandler OnFaceEstimated;

        /// <summary>
        /// 実測FPS
        /// </summary>
        public float FrameRate { get; private set; }
        private int frameCount = 0;
        private float fromFpsMeasured = 0f;


        private ResultHandlerProperty ResultHandler { get; set; } = null;

        /// <summary>
        /// trueの場合、録画ファイルを再生している
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
        }

        private bool isPlaying = false;

        /// <summary>
        /// Face Detection type
        /// automatically sets the property
        /// </summary>
        [SerializeField]
        private FaceDetectorType detectionType = FaceDetectorType.External;

        /// <summary>
        /// Face検出方法
        /// </summary>
        public FaceDetectorType DetectorType
        {
            get
            {
                return detectionType;
            }
            set
            {
                try
                {
                    SetProperty(new DetectorTypeProperty { detectorType = value });
                    detectionType = value;
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                }
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate bool FaceResultHandlerDelegate(IntPtr faceResultsPointer, UInt64 bufferSize);

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
                        try
                        {
                            this.stream = TofArManager.Instance.SensCordCore.OpenStream(StreamKey);
                            TofArColorManager.Instance?.AddManagerDependency(this);
                            this.stream?.RegisterFrameCallback(StreamCallBack);
                        }
                        catch (ApiException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, $"{e.GetType().Name} : {e.Message}¥n{e.StackTrace}");
                            this.streamOpenErrorOccured = true;
                        }

                        try
                        {
                            this.stream?.SetProperty(new DetectorTypeProperty { detectorType = this.detectionType });
                        }
                        catch (ApiException e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message);
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

        private IEnumerator Start()
        {
            TofArManager.Instance.AddSubManager(this);

            if (this.autoStart)
            {
                // wait for color/tof (if using Android)
                if (!TofArManager.Instance.UsingIos)
                {
                    var externalColorSource = Utils.FindFirstGameObjectThatImplements<Color.IExternalColorStream>(true);


                    if (Color.TofArColorManager.Instance != null && (Color.TofArColorManager.Instance.autoStart || externalColorSource != null))
                    {
                        while (!Color.TofArColorManager.Instance.IsStreamActive)
                        {
                            yield return null;
                        }
                    }

                    var externalTofSource = Utils.FindFirstGameObjectThatImplements<IExternalStreamSource>(true);

                    if (Tof.TofArTofManager.Instance != null && (Tof.TofArTofManager.Instance.autoStart || externalTofSource != null))
                    {
                        while (!Tof.TofArTofManager.Instance.IsStreamActive)
                        {
                            yield return null;
                        }
                    }
                }

                TofArFaceManager.Instance.StartStream();
            }
        }

        /// <summary>
        /// オブジェクト破棄
        /// </summary>
        public void Dispose()
        {
            TofArColorManager.Instance?.RemoveManagerDependency(this);

            if (this.stream != null)
            {
                if (this.stream.IsStarted)
                {
                    this.StopStream();
                }
                this.stream.Dispose();
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
        }

        /// <summary>
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
        /// 依存するManagerから要求されたストリーミング再スタートを開始する
        /// </summary>
        /// <param name="requestSource">要求元</param>
        public void RestartStreamByDependManager(object requestSource)
        {
            if (requestSource is IDependedManager)
            {
                switch (this.DetectorType)
                {
                    case FaceDetectorType.External:
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
                    case FaceDetectorType.External:
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

                this.FaceData = null;
            }
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        public void StartStream()
        {
            if (!this.IsStreamActive && this.isUnPaused)
            {
                try
                {
                    this.ResultHandler = null;
                    if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
                    {
                        this.ResultHandler = this.GetProperty<ResultHandlerProperty>();
                    }
                    this.Stream?.Start();

                    if (OnStreamStarted != null)
                    {
                        OnStreamStarted(this);
                    }
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"{e.GetType().Name}:{e.Message}¥n{e.StackTrace}");
                }
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
        /// Propertyリスト取得する
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

        static private void StreamCallBack(Stream stream)
        {
            try
            {
                lock (Instance.stopLock)
                {
                    if (!TofArFaceManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    var frame = stream.GetFrame();

                    try
                    {
                        var faceChannel = frame.Channels[(int)ChannelIds.Face];
                        var rawData = faceChannel.GetRawData();

                        instance.FaceData = new FaceData(rawData);

                        if (OnFaceEstimated != null)
                        {
                            OnFaceEstimated(Instance.FaceData.Data);
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
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"FaceManager streamcallback error: {e.Message}");
            }
        }

        /// <summary>
        /// 推定結果
        /// </summary>
        /// <param name="result">Face認識結果</param>
        public void SetEstimatedResult(FaceResult result)
        {
            this.SetEstimatedResults(new FaceResults() { results = new FaceResult[] { result } });
        }

        /// <summary>
        /// 推定結果
        /// </summary>
        /// <param name="results">Face認識結果</param>
        public void SetEstimatedResults(FaceResults results)
        {
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                if ((this.ResultHandler != null) && (this.ResultHandler.handler != 0))
                {
                    var handler = Marshal.GetDelegateForFunctionPointer<FaceResultHandlerDelegate>(new IntPtr(this.ResultHandler.handler));

                    var bytes = MessagePackSerializer.Serialize<FaceResults>(results);
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
                    Instance.FaceData = new FaceData(results);

                    if (OnFaceEstimated != null)
                    {
                        OnFaceEstimated(Instance.FaceData.Data);
                    }

                    this.frameCount++;
                }
            }

        }

        #region PLAYBACK

        /// <summary>
        /// *TODO+
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_face_stream";

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
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArFaceManager.streamPlayerKey);
                    this.streamPlay?.RegisterFrameCallback(TofArFaceManager.StreamCallBack);
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
        /// 指定されたパス内の録画ファイルの再生を開始する
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

            var play = new PlayProperty();
            play.TargetPath = path;
            play.StartOffset = 0;
            play.Count = 0;
            play.Speed = PlaySpeed.BasedOnFramerate;
            play.Mode.Repeat = true;

            var stream = Instance.StreamPlay;

            stream.SetProperty<PlayProperty>(play);
            stream.Start();

            Instance.isPlaying = true;

            if (OnStreamStarted != null)
            {
                OnStreamStarted(this);
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
            else if (Instance.IsStreamActive && Instance.IsPlaying)
            {
                Instance.StopStream();
            }

            Instance.isPlaying = false;
        }


        #endregion
    }

}

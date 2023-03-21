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
using AOT;
using SensCord;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TofAr.V0.Slam
{
    /// <summary>
    /// TODO+ C Slamはリファレンスに載せる？
    /// </summary>
    public class TofArSlamManager : Singleton<TofArSlamManager>, IDisposable, IStreamStoppable
    {
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
        public const string StreamKey = "tofar_slam_camera2_stream";

        public bool autoStart = false;

        private bool isUnPaused = true;
        private bool streamOpenErrorOccured = false;

        private Stream stream = null;
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
                        this.stream = TofArManager.Instance.SensCordCore.OpenStream(TofArSlamManager.StreamKey);
                        this.Stream?.RegisterFrameCallback(StreamCallBack);
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

        public SlamData SlamData { get; private set; }

        public delegate void StreamStartedEventHandler(object sender);
        public static event StreamStartedEventHandler OnStreamStarted;

        public delegate void StreamStoppedEventHandler(object sender);
        public static event StreamStoppedEventHandler OnStreamStopped;

        public delegate void FrameArrivedEventHandler(object sender);
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

        private bool waitForTof = false;
        private Transform startTrackTarget = null;
        private bool checkForTof = true;

        /// <summary>
        /// Default Camera Pose Source
        /// </summary>
        [SerializeField]
        private CameraPoseSource cameraPoseSource = CameraPoseSource.Gyro;

        /// <summary>
        /// Camera Pose Source on Android
        /// </summary>
        [SerializeField]
        private CameraPoseSource cameraPoseSourceAndroid = CameraPoseSource.Gyro;

        /// <summary>
        /// Camera Pose Source on iOS
        /// </summary>
        [SerializeField]
        private CameraPoseSource cameraPoseSourceIOS = CameraPoseSource.ARKitOrARFoundation;

        public CameraPoseSource CameraPoseSource
        {
            get
            {
                return this.cameraPoseSource;
            }
            set
            {
                this.cameraPoseSource = value;
            }
        }

        internal IDictionary<CameraPoseSource, string> presetTrackers = new Dictionary<CameraPoseSource, string>()
        {
            { CameraPoseSource.Gyro, "Prefabs/GyroPoseTracker" },
            { CameraPoseSource.ARKitOrARFoundation, "Prefabs/MainCameraPoseTracker" },
#if UNITY_EDTOR
            { CameraPoseSource.InternalEngine, "Prefabs/MainCameraPoseTracker" },
            { CameraPoseSource.InternalEngine02, "Prefabs/MainCameraPoseTracker" }
#else
            { CameraPoseSource.InternalEngine, "Prefabs/KPoseTracker" },
            { CameraPoseSource.InternalEngine02, "Prefabs/SdsPoseTracker" }
#endif
        };

        /// <summary>
        /// Auto assine proper tracker automatically.
        /// </summary>
        [SerializeField]
        internal bool autoAssignTracker = true;
        public bool AutoAssignTracker
        {
            get
            {
                return this.autoAssignTracker;
            }
        }

        /// <summary>
        /// The Pose Tracker
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public GameObject trackerObject = null;

        private GameObject tracker = null;

        private delegate void CameraPoseSourceChangeHandlerDelegate(CameraPoseSource newCameraPoseSource);

        [MonoPInvokeCallback(typeof(CameraPoseSourceChangeHandlerDelegate))]
        private static void CameraPoseSourceChanging(CameraPoseSource newCameraPoseSource)
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"CameraPoseSource changing newCameraPoseSource:{newCameraPoseSource}");
        }

        [MonoPInvokeCallback(typeof(CameraPoseSourceChangeHandlerDelegate))]
        private static void CameraPoseSourceChanged(CameraPoseSource newCameraPoseSource)
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"CameraPoseSource changed newCameraPoseSource:{newCameraPoseSource}");
            Instance.StartCoroutine(Instance.ChangeCameraPoseSourceOnServer(newCameraPoseSource));
        }

        [MonoPInvokeCallback(typeof(CameraPoseSourceChangeHandlerDelegate))]
        private static void CameraPoseSourceChangeError(CameraPoseSource newCameraPoseSource)
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"CameraPoseSource change error newCameraPoseSource:{newCameraPoseSource}");
        }

        private IEnumerator ChangeCameraPoseSourceOnServer(CameraPoseSource newCameraPoseSource)
        {
            yield return new WaitForSeconds(1f);
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"ChangeCameraPoseSourceOnServer will restart stream with CameraPoseSource:{newCameraPoseSource}");
            if (this.IsStreamActive)
            {
                this.StopStream();
                this.CameraPoseSource = newCameraPoseSource;
//                this.SetProperty<CameraPoseSourceProperty>(new CameraPoseSourceProperty() { cameraPoseSource = newCameraPoseSource });
                this.StartStream();
            }
        }

        private IEnumerator Start()
        {
            TofArManager.Instance.AddSubManager(this);

#if UNITY_ANDROID
            this.cameraPoseSource = this.cameraPoseSourceAndroid;
#elif UNITY_IOS
            this.cameraPoseSource = this.cameraPoseSourceIOS;
#endif
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"cameraPoseSource:{cameraPoseSource} cameraPoseSourceAndroid:{cameraPoseSourceAndroid} cameraPoseSourceiOS:{cameraPoseSourceIOS}");

            if (TofArManager.Instance.RuntimeSettings.isRemoteServer)
            {
                var functionPointerChanging = Marshal.GetFunctionPointerForDelegate(new CameraPoseSourceChangeHandlerDelegate(CameraPoseSourceChanging));
                var functionPointerChanged = Marshal.GetFunctionPointerForDelegate(new CameraPoseSourceChangeHandlerDelegate(CameraPoseSourceChanged));
                var functionPointerChngeError = Marshal.GetFunctionPointerForDelegate(new CameraPoseSourceChangeHandlerDelegate(CameraPoseSourceChangeError));
                TofArSlamManager.Instance.SetProperty(new CameraPoseSourceChangeHandlerProperty()
                {
                    changingHandler = (UInt64)functionPointerChanging.ToInt64(),
                    changedHandler = (UInt64)functionPointerChanged.ToInt64(),
                    errorHandler = (UInt64)functionPointerChngeError.ToInt64(),
                });
            }

            if (this.autoStart)
            {
                if (TofAr.V0.Color.TofArColorManager.Instantiated)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (TofAr.V0.Tof.TofArTofManager.Instantiated)
                {
                    yield return new WaitForEndOfFrame();
                }

                yield return null;
                TofArSlamManager.Instance.StartStream();
            }
        }

        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArSlamManager.Dispose()");
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
            // restart stream, if slam was already running
            if (this.IsStreamActive)
            {
                this.checkForTof = false;
                StopStream();
                Transform trackTarget = this.tracker?.transform.parent;
                StartStream(trackTarget);
            }
            else if (this.waitForTof)
            {
                this.checkForTof = false;
                this.waitForTof = false;
                StartStream(this.startTrackTarget);

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
                    if (!TofArSlamManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    var frame = stream.GetFrame();

                    try
                    {
                        var channel = frame.Channels[(int)ChannelIds.Slam];
                        var rawData = channel.GetRawData();
                        instance.SlamData = new SlamData(rawData);
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
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }*/
        }

        public void StartStream()
        {
            this.StartStream(null);
        }

        public void StartStream(Transform trackTarget)
        {
            if (!this.IsStreamActive && this.isUnPaused)
            {
                if (this.checkForTof)
                {
                    bool isTofStarting = Tof.TofArTofManager.Instance?.IsTofStarting == true;
                    if (isTofStarting)
                    {
                        startTrackTarget = trackTarget;
                        this.waitForTof = true;
                        return;
                    }
                }
                this.checkForTof = true;

                if (this.streamPlay != null && this.streamPlay.IsStarted)
                {
                    this.StopPlayback();
                }
                this.TryAssignTrackerFromExternalConnector();
                this.AttachPoseTracker(trackTarget);
                if (!TofArManager.Instance.RuntimeSettings.isRemoteServer)
                {
                    this.SetProperty(new CameraPoseSourceProperty() { cameraPoseSource = this.cameraPoseSource });
                }
                if (Tof.TofArTofManager.Instance?.ProcessTargets?.processDepth == false)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Info, "Process depth is disabled. Slam might not be processed.");
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

        public void StopStream()
        {
            if (this.IsStreamActive)
            {
                this.Stream?.Stop();
                this.DettachPoseTracker();
                if (OnStreamStopped != null)
                {
                    OnStreamStopped(this);
                }
            }
            this.streamOpenErrorOccured = false;
        }

        public T GetProperty<T>() where T : class, IBaseProperty, new()
        {

            if (this.isUnPaused || this.IsPlaying)
            {
                T property = new T();
                if (property is CameraPoseProperty)
                {
                    return TofArManager.Instance.GetProperty<T>();
                }
                var bufferSize = 1024;
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(property.Key, ref property, bufferSize);
                return property;
            }
            return null;
        }

        public T GetProperty<T>(T value, int bufferSize) where T : class, IBaseProperty, new()
        {
            if (this.isUnPaused || this.IsPlaying)
            {
                if (value is CameraPoseProperty)
                {
                    return TofArManager.Instance.GetProperty<T>(value, bufferSize);
                }
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(value.Key, ref value, bufferSize);
                return value;
            }
            return null;
        }

        public T GetProperty<T>(T value) where T : class, IBaseProperty
        {
            if (this.isUnPaused || this.IsPlaying)
            {
                if (value is CameraPoseProperty)
                {
                    return TofArManager.Instance.GetProperty<T>(value);
                }
                int bufferSize = 1024;
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(value.Key, ref value, bufferSize);
                return value;
            }
            return null;
        }

        public void SetProperty<T>(T value) where T : class, IBaseProperty
        {
            var sourceProp = value as CameraPoseSourceProperty;
            if (sourceProp != null)
            {
                this.cameraPoseSource = sourceProp.cameraPoseSource;
            }

            if (this.isUnPaused)
            {
                this.Stream?.SetProperty<T>(value);
            }
        }

        public string[] GetPropertyList()
        {
            if (this.isUnPaused)
            {
                return this.Stream?.GetPropertyList();
            }
            return null;
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

        private void AttachPoseTracker(Transform trackTarget)
        {
            if (autoAssignTracker)
            {
                var prefab = Resources.Load<GameObject>(this.presetTrackers[this.CameraPoseSource]);
                this.trackerObject = prefab;
            }
            this.tracker = GameObject.Instantiate(this.trackerObject);
            trackTarget = (trackTarget == null) ? this.tracker.GetComponent<ITrackTargetFinder>().GetTrackTarget() : trackTarget;
            this.tracker.transform.parent = trackTarget;
            this.tracker.transform.localPosition = Vector3.zero;
            this.tracker.transform.localRotation = Quaternion.identity;
            TofArManager.Instance.cameraPoseTracker = this.tracker.transform;
            TofArManager.Instance.autoTrackCameraPose = true;
        }

        private void DettachPoseTracker()
        {
            if (TofArManager.Instance != null)
            {
                TofArManager.Instance.autoTrackCameraPose = false;
                TofArManager.Instance.cameraPoseTracker = null;
            }
            if (this.tracker != null)
            {
                GameObject.Destroy(this.tracker);
                this.tracker = null;
            }
        }

        private void TryAssignTrackerFromExternalConnector()
        {
            var externalConnectors = GameObject.FindObjectsOfType<Transform>().Where(o => o.GetComponent<IExternalSlamConnector>() != null).Select(o => o.GetComponent<IExternalSlamConnector>()).ToArray();
            if (externalConnectors.Length > 1)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "[TofArSlam] multiple external slam connectors are found.");
                throw new ApiException(new Status() { cause = ErrorCause.InvalidArgument, level = ErrorLevel.Fail });
            }
            if (externalConnectors.Length == 1)
            {
                this.cameraPoseSource = externalConnectors[0].GetCameraPoseSource();
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArSlam] cameraPoseSource updated to {this.cameraPoseSource} by {externalConnectors[0].GetType().Name}.");
                var prefab = Resources.Load<GameObject>(this.presetTrackers[this.CameraPoseSource]);
                this.trackerObject = prefab;
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
        /// 再生ストリームキー
        /// </summary>
        private const string streamPlayerKey = "player_slam_stream";

        private Stream streamPlay = null;
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
                    this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(TofArSlamManager.streamPlayerKey);

                    this.streamPlay?.RegisterFrameCallback(TofArSlamManager.StreamCallBack);
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

        public bool IsPlaying { private set; get; } = false;
        private bool isPlayingWithTof = false;
        private bool startingPlayback = false;

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

        public void StartPlayback(string path)
        {
            if (this.IsPlaying)
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

            this.TryAssignTrackerFromExternalConnector();
            this.AttachPoseTracker(null);

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
                    stream.GetProperty<CameraPoseSourceProperty>();
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
                        OnStreamStarted(this);
                    }
                }
                break;
            }
        }

        public void StopPlayback()
        {

            this.DettachPoseTracker();
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

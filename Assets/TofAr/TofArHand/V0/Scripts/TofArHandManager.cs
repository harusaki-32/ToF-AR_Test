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
using SensCord;
using TofAr.V0.Tof;
using UnityEngine;
using UnityEngine.Events;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// TofAr Handコンポーネントとの接続を管理する
    /// <para>下記機能を有する</para>
    /// <list type="bullet">
    /// <item><description>手認識設定</description></item>
    /// <item><description>Handデータの取得</description></item>
    /// <item><description>ストリーム開始イベント通知</description></item>
    /// <item><description>ストリーム終了イベント通知</description></item>
    /// <item><description>フレーム到着通知</description></item>
    /// <item><description>ジェスチャー推定結果通知</description></item>
    /// <item><description> 録画ファイルの再生</description></item>
    /// </list>
    /// </summary>
    public class TofArHandManager : Singleton<TofArHandManager>, IDisposable, IStreamStoppable, IDependManager
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
        public const string StreamKeyTFLite = "tofar_hand_camera2_tflite_stream";

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にHandデータのストリームを開始する
        /// </summary>
        public bool autoStart = false;

        [SerializeField]
        private ProcessLevel processLevel = ProcessLevel.HandPoints;
        [SerializeField]
        private RecogMode recogMode = RecogMode.OneHandHoldSmapho;
        //[SerializeField]
        [Obsolete("rotCorrection is obsolete and will be removed in a future version")]
        private RotCorrection rotCorrection = RotCorrection.On;

        /// <summary>
        /// 実行モードの自動設定ON/OFF
        /// </summary>
        [SerializeField]
        private bool autoSetRuntimeMode = true;

        /// <summary>
        /// 実行モード
        /// </summary>
        [SerializeField, SerializeStateField("autoSetRuntimeMode", false)]
        private RuntimeMode runtimeMode = RuntimeMode.Cpu;

        /// <summary>
        /// 実行モード（再設定時）
        /// </summary>
        [SerializeField, SerializeStateField("autoSetRuntimeMode", false)]
        private RuntimeMode runtimeModeAfter = RuntimeMode.Cpu;


        [SerializeField]
        private int intervalFramesNotRecognized = 10;
        [SerializeField]
        private int framesForDetectNoHands = 3;
        [SerializeField]
        private bool trackingMode = true;
        [SerializeField]
        private bool temporalRecognitionMode = false;
        [SerializeField]
        private int nPointsThreads = 2;
        [SerializeField]
        private int nRegionThreads = 2;
        [SerializeField]
        private NoiseReductionLevel noiseReductionLevel = NoiseReductionLevel.Low;

        private bool propertyChanged = false;

        /// <summary>
        /// 認識ステップの指定
        /// </summary>
        public ProcessLevel ProcessLevel
        {
            get { return processLevel; }
            set
            {
                this.processLevel = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// 認識モードの指定
        /// </summary>
        public RecogMode RecogMode
        {
            get { return recogMode; }
            set
            {
                if (Array.FindIndex(this.SupportedRecogModes, x => x == value) < 0)
                {
                    throw new NotSupportedException($"RecogMode {value} not supported on this platform");
                }
                this.recogMode = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// 手の回転補正有無
        /// </summary>
        [Obsolete("RotCorrection is obsolete and will be removed in a future version")]
        public RotCorrection RotCorrection
        {
            get { return rotCorrection; }
            set
            {
                this.rotCorrection = value;
                //this.propertyChanged = true;
            }
        }

        /// <summary>
        /// 実行モード(前段処理)の指定
        /// </summary>
        public RuntimeMode RuntimeMode
        {
            get
            {

                if (autoSetRuntimeMode && !runtimeModesApplied)
                {
                    runtimeModesApplied = true;
                    SetDefaultRuntimeMode();
                    this.propertyChanged = true;
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
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// 実行モード(後段処理)の指定
        /// </summary>
        public RuntimeMode RuntimeModeAfter
        {
            get
            {
                if (autoSetRuntimeMode && !runtimeModesApplied)
                {
                    runtimeModesApplied = true;
                    SetDefaultRuntimeMode();
                    this.propertyChanged = true;
                }

                return runtimeModeAfter;
            }
            set
            {
                if (Array.FindIndex(this.SupportedRuntimeModes, x => x == value) < 0)
                {
                    throw new NotSupportedException($"RuntimeMode {value} not supported on this platform");
                }
                this.runtimeModeAfter = value;
                this.propertyChanged = true;
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

#if UNITY_EDITOR
        /// <summary>
        /// 実行モード(前段処理)の指定(Editor用)
        /// </summary>
        public RuntimeMode RuntimeMode_Editor
        {
            get { return runtimeMode; }
            set
            {
                this.runtimeMode = value;
            }
        }

        /// <summary>
        /// 実行モード(後段処理)の指定（Editor用）
        /// </summary>
        public RuntimeMode RuntimeModeAfter_Editor
        {
            get { return runtimeModeAfter; }
            set
            {
                this.runtimeModeAfter = value;
            }
        }
#endif

        /// <summary>
        /// 実行モードの取得
        /// </summary>
        /// <param name="mode">モード名称</param>
        /// <returns>実行モード</returns>
        public RuntimeMode GetRuntimeMode(string mode)
        {
            if (mode.Equals("GPU"))
            {
                return RuntimeMode.Gpu;
            }

            if (mode.Equals("DSP"))
            {
                return RuntimeMode.Dsp;
            }

            if (mode.Equals("XNNPACK"))
            {
                return RuntimeMode.XnnPack;
            }

            if (mode.Equals("COREML"))
            {
                return RuntimeMode.CoreML;
            }

            return RuntimeMode.Cpu;
        }

        /// <summary>
        /// トラッキングモード
        /// </summary>
        public bool TrackingMode
        {
            get { return trackingMode; }
            set
            {
                this.trackingMode = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// 一時的認識モード
        /// </summary>
        public bool TemporalRecognitionMode
        {
            get { return temporalRecognitionMode; }
            set
            {
                this.temporalRecognitionMode = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// サポートする実行モードリスト
        /// </summary>
        public RuntimeMode[] SupportedRuntimeModes
        {
            get
            {
                if (TofArManager.Instance.UsingIos)
                {
                    return new RuntimeMode[] {
                        RuntimeMode.Cpu,
                        RuntimeMode.Gpu,
                        RuntimeMode.XnnPack,
                        RuntimeMode.CoreML};
                }
                else
                {
                    return new RuntimeMode[] {
                        RuntimeMode.Cpu,
                        RuntimeMode.Gpu,
                        RuntimeMode.XnnPack};
                }
            }
        }
        /// <summary>
        /// サポートする処理レベルリスト
        /// </summary>
        public ProcessLevel[] SupportedProcessLevels
        {
            get
            {
                return new ProcessLevel[] {
                    ProcessLevel.HandPoints,
                    ProcessLevel.HandCenterOnly };
            }
        }
        /// <summary>
        /// サポートする認識モードリスト
        /// </summary>
        public RecogMode[] SupportedRecogModes
        {
            get
            {
                bool usingIos = TofAr.V0.TofArManager.Instance.UsingIos;
                if (usingIos)
                {
                    return new RecogMode[] {
                        RecogMode.Face2Face
                    };
                }
                else
                {
                    return new RecogMode[] {
                        RecogMode.OneHandHoldSmapho,
                        RecogMode.Face2Face,
                        RecogMode.HeadMount
                    };
                }
            }
        }


        /// <summary>
        /// 手認識基本設定
        /// <para>本プロパティの設定時、コンポーネント内部では手認識エンジンの再スタートが行われる</para>
        /// </summary>
        public int IntervalFramesNotRecognized
        {
            get { return intervalFramesNotRecognized; }
            set
            {
                this.intervalFramesNotRecognized = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// このフレーム数のNoHandsが連続して検出されると intervalFramesNotRecognized で指定されたインターバル動作を開始する (デフォルト:3)
        /// </summary>
        public int FramesForDetectNoHands
        {
            get { return framesForDetectNoHands; }
            set
            {
                this.framesForDetectNoHands = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// Pointスレッド数。NNLibrary=TFLiteの時有効。
        /// </summary>
        public int NPointThreads
        {
            get { return nPointsThreads; }
            set
            {
                this.nPointsThreads = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// Regionスレッド数。NNLibrary=TFLiteの時有効。
        /// </summary>
        public int NRegionThreads
        {
            get { return nRegionThreads; }
            set
            {
                this.nRegionThreads = value;
                this.propertyChanged = true;
            }
        }

        /// <summary>
        /// スムージングモード
        /// </summary>
        public NoiseReductionLevel NoiseReductionLevel
        {
            get { return noiseReductionLevel; }
            set
            {
                this.noiseReductionLevel = value;
                this.propertyChanged = true;
            }
        }


        /// <summary>
        /// ジェスチャー推定データ読み取り開始のデリゲート
        /// </summary>
        /// <param name="Buffer">FrameData配列</param>
        /// <param name="FramesPerGesture">ジェスチャー認識を行うフレーム数</param>
        /// <param name="topindex">ジェスチャー推定バッファの先頭インデックス</param>
        /// <param name="size">ジェスチャー推定バッファのサイズ</param>
        public delegate void GestureDataReadEventHandler(FrameData[] Buffer, int FramesPerGesture, int topindex, int size);
        /// <summary>
        /// 右手ジェスチャー推定データ読み取り開始通知
        /// </summary>
        public static event GestureDataReadEventHandler OnLeftGestureDataRead;

        /// <summary>
        /// 右手ジェスチャー推定データ読み取り開始通知
        /// </summary>
        public static event GestureDataReadEventHandler OnRightGestureDataRead;

        /// <summary>
        /// 手認識ライブラリー読み込み失敗時イベント
        /// </summary>
        [System.Serializable]
        public class HandLibraryLoadFailedEventHandler : UnityEvent<string> { }
        /// <summary>
        /// 手認識ライブラリー読み込み失敗通知
        /// </summary>
        public HandLibraryLoadFailedEventHandler HandLibraryLoadFailed;

        /// <summary>
        /// *TODO+ B
        /// 右手ジェスチャー推定データ読み取り開始通知
        /// </summary>
        [Obsolete("OnGestureDataReadLeft is deprecated. please use OnLeftGestureDataRead")]
        private event GestureDataReadEventHandler OnGestureDataReadLeft
        {
            add { OnLeftGestureDataRead += value; }
            remove { OnLeftGestureDataRead -= value; }
        }
        /// <summary>
        /// *TODO+ B
        /// 右手ジェスチャー推定データ読み取り開始通知
        /// </summary>
        [Obsolete("OnGestureDataReadRight is deprecated. please use OnRightGestureDataRead")]
        private event GestureDataReadEventHandler OnGestureDataReadRight
        {
            add { OnRightGestureDataRead += value; }
            remove { OnRightGestureDataRead -= value; }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void StreamCallBackDelegate(IntPtr stream, IntPtr privateData);

        private int FramesPerGesture = 5;

        /// <summary>
        /// trueの場合ジェスチャー認識機能が有効である
        /// </summary>
        public bool IsGestureEstimating { get; private set; } = false;

        private GestureIndex[] gestureResultListLeft, gestureResultListRight;

        private HandStatus[] handStatusListLeft, handStatusListRight;

        private float timeElapsedLeft = 0f;
        private float timeElapsedRight = 0f;

        private object Lock = new object();

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にジェスチャー推定処理を開始する
        /// </summary>
        public bool autoStartGestureEstimation = false;
        /// <summary>
        /// ジェスチャー推定に使用するフレーム数 (デフォルト:4)。
        /// <para>このフレーム数のうち gestureRecogThreshold% のフレームが同一ジェスチャーと判定されたものを推定結果とする。</para>
        /// </summary>
        internal int gestureEstimationFrames = gestureEstimationFrames30FPS;

        private const int gestureEstimationFrames30FPS = 4;
        private const int gestureEstimationFrames15FPS = 2;

        private object lockGesture = new object();

        /// <summary>
        /// ジェスチャー推定完了閾値 (デフォルト:0.75)。
        /// </summary>
        internal float gestureRecogThreshold = 0.75f;

        private const string localGestureFileAndroid = "/data/local/tmp/tofar/config/hand_gesture.bytes";
        private string pathToLocalFile = localGestureFileAndroid;

        private const string NnpFile = "hand_gesture.nnp";
        private TFLiteEstimator tfEstimator = null;

        /// <summary>
        /// TODO+ C
        /// </summary>
        private bool[] GestureIndexMask = new bool[]
            {
                true, // 0      //None,
                true,           //Others,
                true,           //Bloom,
                true,           //AirTap,
                true,          //SnapFinger,
                true, // 5      //FingerThrow,
                true,           //HandThrow,
                true,           //Shoot,
                true,           //Punch,
                false,          //Milk,
                false, // 10    //Bye,
                true,           //HandSwipe,
                true,           //ThumbTap,
                true,           //TurnKnob,
                true, // 14     //Finish,
                true,           //Eat,
                true,            //Twinkle
                true,           //Hobby,
                true,           //Beard,
                true,           //Nose,
                true, //20      //ComeOn,
                true,           //Flick,
                true,           //Darts,
                true,           //Chop,
                true, //24      //ReverseSwipe
            };

        /// <summary>
        /// TODO+ C
        /// </summary>
        private float[] GestureIndexNotifyInterval = new float[]
            {
                0.5f, // 0      //None,
                0.5f,           //Others,
                0.5f,           //Bloom,
                0.2f,           //AirTap,
                0.5f,          //SnapFinger,
                0.5f, // 5      //FingerThrow,
                0.5f,           //HandThrow,
                0.5f,           //Shoot,
                0.5f,           //Punch,
                0.5f,          //Milk,
                0.5f, // 10    //Bye,
                0.5f,           //HandSwipe,
                0.2f,           //ThumbTap,
                0.5f,           //TurnKnob,
                0.5f, // 14     //Finish,
                0.5f,           //Eat,
                0.5f,            //Twinkle
                0.5f,           //Hobby,
                0.5f,           //Beard,
                0.5f,           //Nose,
                0.2f, //20      //ComeOn,
                0.5f,           //Flick,
                0.5f,           //Darts,
                0.5f,           //Chop,
                0.5f, //24      //ReverseSwipe
            };

        private bool isUnPaused = true;
        private bool streamOpenErrorOccured = false;

        private bool waitForTof = false;
        private RecognizeConfigProperty startConfig = null;
        private bool checkForTof = true;

        private HandLogic logic = null;

        private void SetDefaultRuntimeMode()
        {
            var devcap = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();
            var deviceAttribs = JsonUtility.FromJson<deviceAttributesJson>(devcap.TrimmedDeviceAttributesString);
            if (deviceAttribs != null)
            {
                runtimeMode = GetRuntimeMode(deviceAttribs.defaultHandRuntimeMode);
                runtimeModeAfter = GetRuntimeMode(deviceAttribs.defaultHandRuntimeModeAfter);
            }
            else if (TofArManager.Instance.UsingIos)
            {
                runtimeMode = RuntimeMode.Gpu;
                runtimeModeAfter = RuntimeMode.Gpu;
            }
        }

        /// <summary>
        /// *TODO+ B
        /// 使用してる推定ライブラリーのストリームキーを取得する
        /// </summary>
        /// <returns>*TODO+ C ストリームキー</returns>
        private string GetStreamKey()
        {
            return TofArHandManager.StreamKeyTFLite;
        }

        private bool runtimeModesApplied = false;

        private void OpenHandStreamWithTFLite()
        {
            if (autoSetRuntimeMode && !runtimeModesApplied)
            {
                runtimeModesApplied = true;
                SetDefaultRuntimeMode();
            }

            propertyChanged = true;
            try
            {
                maxStreamStartRetry = 3;

                this.stream = TofArManager.Instance.SensCordCore.OpenStream(GetStreamKey());
                this.stream?.RegisterFrameCallback(StreamCallBack);
            }

            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                HandLibraryLoadFailed?.Invoke(e.Message);
                this.streamOpenErrorOccured = true;
            }
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

                        OpenHandStreamWithTFLite();

                        ApplySettings();

                        TofArTofManager.Instance?.AddManagerDependency(this);
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

        private bool IsStopStreamRequested { get; set; } = false;
        private bool IsStreamPausing { get; set; }


        private List<IPreProcessHandData> preProcessors = new List<IPreProcessHandData>();

        /// <summary>
        /// Handデータ送出前処理を登録する
        /// </summary>
        /// <param name="preProcessHand">IPreProcessHandDataを実装したデータ処理クラス</param>
        public void RegisterHandPreProcessing(IPreProcessHandData preProcessHand)
        {
            preProcessors.Add(preProcessHand);
        }
        /// <summary>
        /// Handデータ送出前の処理を登録解除する
        /// </summary>
        /// <param name="preProcessHand">IPreProcessHandDataを実装したデータ処理クラス</param>
        public void UnregisterHandPreProcessing(IPreProcessHandData preProcessHand)
        {
            preProcessors.Remove(preProcessHand);
        }

        /// <summary>
        /// 最新のHandデータ	
        /// </summary>
        public HandData HandData { get; private set; }

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
        /// ジェスチャー推定完了時デリゲート
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="result">ジェスチャー推定結果</param>
        public delegate void GestureEstimatedEventHandler(object sender, GestureResultProperty result);

        /// <summary>
        /// OnGestureEstimated:ジェスチャー推定が完了した時に GestureResultProperty を通知する。
        /// </summary>
        public static event GestureEstimatedEventHandler OnGestureEstimated;

        /// <summary>
        /// OnGestureEstimatedDefault:ジェスチャー推定の結果、ジェスチャーが検出されなかった時に GestureResultProperty を通知する。
        /// </summary>
        public static event GestureEstimatedEventHandler OnGestureEstimatedDefault;

        /// <summary>
        /// *TODO+ B
        /// OnGestureEstimated:ジェスチャー推定が完了した時に GestureResultProperty を通知する
        /// </summary>
        [Obsolete("GestureEstimated is deprecated, please use OnGestureEstimated")]
        private event GestureEstimatedEventHandler GestureEstimated
        {
            add { OnGestureEstimated += value; }
            remove { OnGestureEstimated -= value; }
        }

        /// <summary>
        /// *TODO+ B
        /// OnGestureEstimatedDefault:ジェスチャー推定の結果、ジェスチャーが検出されなかった時に GestureResultProperty を通知する
        /// </summary>
        [Obsolete("GestureEstimatedDefault is deprecated, please use OnGestureEstimatedDefault")]
        private event GestureEstimatedEventHandler GestureEstimatedDefault
        {
            add { OnGestureEstimatedDefault += value; }
            remove { OnGestureEstimatedDefault -= value; }
        }

        /// <summary>
        /// ジェスチャー推定モデルファイル置換時デリゲート
        /// </summary>
        /// <param name="msg">エラーメッセージ</param>
        public delegate void ModelFileReplacedEventHandler(string msg);
        /// <summary>
        /// ジェスチャー推定モデルファイル置換通知
        /// </summary>
        public static event ModelFileReplacedEventHandler OnReplacedModelFile;

        /// <summary>
        /// *TODO+ B
        /// ジェスチャー推定モデルファイル置換通知
        /// </summary>
        [Obsolete("OnModelFileReplaced is deprecated, please use static OnReplacedModelFile")]
        private event ModelFileReplacedEventHandler OnModelFileReplaced
        {
            add { OnReplacedModelFile += value; }
            remove { OnReplacedModelFile -= value; }
        }

        /// <summary>
        /// 実測FPS
        /// </summary>
        public float FrameRate { get; private set; }
        private int frameCount = 0;
        private float fromFpsMeasured = 0f;

        /// <summary>
        /// ジェスチャー推定のFPS
        /// </summary>
        public int FramesPerSec { get; set; } = 30;
        private bool newDataArrived = false;

        private int lastIndex;

        internal RingBuffer GestureRingBufferLeft { get; set; }
        internal RingBuffer GestureRingBufferRight { get; set; }

        /// <summary>
        /// ジェスチャー推定時に端末の加速度センサーを用いた補正を行う (デフォルト:true)
        /// </summary>
        private bool adjustByAccelerationOnGestureEstimation = true;

        private GestureIndex lastGestureIdLeft, lastGestureIdRight;

        /// <summary>
        /// 手の座標処理を行うオブジェクト
        /// </summary>
        public HandCalc HandCalc = new HandCalc();

        /// <summary>
        /// <para>true: 自動回転を行う</para>
        /// <para>false: 自動回転を行わない</para>
        /// <para>デフォルト値: true</para>
        /// </summary>
        public bool autoRotate = true;
        /// <summary>
        /// <para>true: 手の座標をColor空間に変換する</para>
        /// <para>false: 変換しない</para>
        /// <para>デフォルト値: false</para>
        /// </summary>
        public bool transformToColorSpace = false;


        private bool gestureInitialized = false;

        [System.Serializable]
        private class deviceAttributesJson
        {
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string defaultHandLibrary = "";
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string defaultHandRuntimeMode = "";
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string defaultHandRuntimeModeAfter = "";
            /// <summary>
            /// TODO+ C
            /// </summary>
            public string fallbackHandLibrary = "";
            /// <summary>
            /// TODO+ C
            /// </summary>
            public uint handMgrMaxStreamStartRetry = 10;
        }

        private void Start()
        {
            this.logic = new HandLogic();
            TofArManager.Instance.AddSubManager(this);

            if (this.autoStart)
            {
                this.StartCoroutine(this.StartProcess());
            }
        }

        private void OnEnable()
        {
            Tof.TofArTofManager.OnStreamStarted += OnTofStreamStarted;

            TofArManager.OnDeviceOrientationUpdated += OnDeviceOrientationChanged;
            TofArManager.OnScreenOrientationUpdated += OnScreenOrientationChanged;

            var orientationsProperty = TofArManager.Instance.GetProperty<DeviceOrientationsProperty>();
            if (orientationsProperty != null)
            {
                OnDeviceOrientationChanged(DeviceOrientation.LandscapeLeft, orientationsProperty.deviceOrientation);
                OnScreenOrientationChanged(ScreenOrientation.LandscapeLeft, orientationsProperty.screenOrientation);
            }
        }



        private void OnDisable()
        {
            TofArManager.OnDeviceOrientationUpdated -= OnDeviceOrientationChanged;
            TofArManager.OnScreenOrientationUpdated -= OnScreenOrientationChanged;

            Tof.TofArTofManager.OnStreamStarted -= OnTofStreamStarted;
        }

        private void OnScreenOrientationChanged(ScreenOrientation previousOrientation, ScreenOrientation newOrientation)
        {
            if (!UnityEngine.XR.XRSettings.enabled)
            {
                CameraOrientation cameraOrientation;
                var screenOrientation = newOrientation;

                //if (currentScreenOrientation != screenOrientation)
                {
                    //currentScreenOrientation = screenOrientation;

                    switch (screenOrientation)
                    {
                        case ScreenOrientation.Portrait:
                            TofArManager.Logger.WriteLog(LogLevel.Debug, "Orientation: Portrait");
                            cameraOrientation = CameraOrientation.Portrait;
                            break;
                        case ScreenOrientation.PortraitUpsideDown:
                            TofArManager.Logger.WriteLog(LogLevel.Debug, "Orientation: PortraitUpsideDown");
                            cameraOrientation = CameraOrientation.PortraitUpsideDown;
                            break;
                        case ScreenOrientation.LandscapeLeft:
                            TofArManager.Logger.WriteLog(LogLevel.Debug, "Orientation: LandscapeLeft");
                            cameraOrientation = CameraOrientation.LandscapeLeft;
                            break;
                        case ScreenOrientation.LandscapeRight:
                            TofArManager.Logger.WriteLog(LogLevel.Debug, "Orientation: LandscapeRight");
                            cameraOrientation = CameraOrientation.LandscapeRight;
                            break;
                        default:
                            return;
                    }


                    var cameraOrientationProperty = new CameraOrientationProperty()
                    {
                        cameraOrientation = cameraOrientation
                    };
                    SetProperty(cameraOrientationProperty);

                    if (GestureRingBufferLeft != null)
                    {
                        GestureRingBufferLeft.Reset();
                    }
                    if (GestureRingBufferRight != null)
                    {
                        GestureRingBufferRight.Reset();
                    }
                }

            }
        }

        private void OnDeviceOrientationChanged(DeviceOrientation previousDeviceOrientation, DeviceOrientation newDeviceOrientation)
        {
            CameraOrientation cameraOrientation;

            var deviceOrientation = newDeviceOrientation;

            if (UnityEngine.XR.XRSettings.enabled)
            {
                //if (currentDeviceOrientation != deviceOrientation)
                {
                    //currentDeviceOrientation = deviceOrientation;

                    if ((TofArManager.Instance.EnabledOrientations & (TofArManager.Instance.EnabledOrientations - 1)) != 0)
                    {
                        switch (deviceOrientation)
                        {
                            case DeviceOrientation.LandscapeLeft:
                                cameraOrientation = CameraOrientation.LandscapeLeft;
                                break;
                            case DeviceOrientation.LandscapeRight:
                                cameraOrientation = CameraOrientation.LandscapeRight;
                                break;
                            default:
                                return;
                        }
                    }
                    else
                    {
                        switch (TofArManager.Instance.EnabledOrientations)
                        {
                            case EnabledOrientation.LandscapeLeft:
                                cameraOrientation = CameraOrientation.LandscapeLeft;
                                break;
                            case EnabledOrientation.LandscapeRight:
                                cameraOrientation = CameraOrientation.LandscapeRight;
                                break;
                            default:
                                return;
                        }
                    }

                    var cameraOrientationProperty = new CameraOrientationProperty()
                    {
                        cameraOrientation = cameraOrientation
                    };
                    SetProperty(cameraOrientationProperty);

                    if (GestureRingBufferLeft != null)
                    {
                        GestureRingBufferLeft.Reset();
                    }
                    if (GestureRingBufferRight != null)
                    {
                        GestureRingBufferRight.Reset();
                    }
                }
            }


            if (GestureRingBufferLeft != null && GestureRingBufferLeft.DeviceAngle != deviceOrientation)
            {
                GestureRingBufferLeft.Reset();
                GestureRingBufferLeft.DeviceAngle = deviceOrientation;
            }

            if (GestureRingBufferRight != null && GestureRingBufferRight.DeviceAngle != deviceOrientation)
            {
                GestureRingBufferRight.Reset();
                GestureRingBufferRight.DeviceAngle = deviceOrientation;
            }

        }

        private void OnTofStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, Tof.PointCloudData pointCloudData)
        {
            // restart stream, if hand was already running
            if (this.IsStreamActive)
            {
                this.checkForTof = false;
                var currentConfig = GetProperty<RecognizeConfigProperty>();
                StopStream();
                StartStream(currentConfig);
            }
            else if (this.waitForTof)
            {
                this.waitForTof = false;
                this.checkForTof = false;
                StartStream(startConfig);

            }
        }
        /// <summary>
        /// 関節座標値の変換関数を設定する	
        /// </summary>
        /// <param name="transformFunction">Vector3[]型の座標リストを受け取り、変換後のVector3[]型の座標リストをリターンする関数</param>
        public void SetTransformFunction(Func<Vector3[], Vector3[]> transformFunction)
        {
            this.HandCalc.transformAction = transformFunction;
        }

        private void InitializeGestureBuffers()
        {
            // Lock gesture
            lock (lockGesture)
            {
                if (TofArManager.Instance.UsingIos)
                {
                    var config = Tof.TofArTofManager.Instance.GetProperty<Tof.Camera2ConfigurationProperty>();
                    if (config != null && config.lensFacing == (int)Tof.LensFacing.Front)
                    {
                        this.gestureEstimationFrames = gestureEstimationFrames15FPS;
                        this.FramesPerSec = 15;
                    }
                    else
                    {
                        this.gestureEstimationFrames = gestureEstimationFrames30FPS;
                        this.FramesPerSec = 30;
                    }
                }

                if (gestureResultListLeft == null)
                {
                    gestureResultListLeft = new GestureIndex[this.gestureEstimationFrames];
                }
                else if (gestureResultListLeft.Length != this.gestureEstimationFrames)
                {
                    Array.Resize(ref gestureResultListLeft, this.gestureEstimationFrames);
                }

                if (gestureResultListRight == null)
                {
                    gestureResultListRight = new GestureIndex[this.gestureEstimationFrames];
                }
                else if (gestureResultListRight.Length != this.gestureEstimationFrames)
                {
                    Array.Resize(ref gestureResultListRight, this.gestureEstimationFrames);
                }

                if (handStatusListLeft == null)
                {
                    handStatusListLeft = new HandStatus[this.gestureEstimationFrames];
                }
                else if (handStatusListLeft.Length != this.gestureEstimationFrames)
                {
                    Array.Resize(ref handStatusListLeft, this.gestureEstimationFrames);
                }

                if (handStatusListRight == null)
                {
                    handStatusListRight = new HandStatus[this.gestureEstimationFrames];
                }
                else if (handStatusListRight.Length != this.gestureEstimationFrames)
                {
                    Array.Resize(ref handStatusListRight, this.gestureEstimationFrames);
                }
            }
        }

        private void InitializeGestureRecognition(bool showMsg = true)
        {
            InitializeGestureBuffers();

#if UNITY_IOS
            pathToLocalFile = Application.persistentDataPath + "/" + "hand_gesture.bytes";
#endif

            bool localFileFound = System.IO.File.Exists(pathToLocalFile);
            string nnpFileFullPath = Application.persistentDataPath + "/" + NnpFile;

            // get network
            if (!System.IO.File.Exists(nnpFileFullPath))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "File in persistent path not existing. Creating new one");
                if (localFileFound)
                {
                    GetData(NnpFile, showMsg);
                }
                else
                {
                    GetDataFromResources(NnpFile, showMsg);
                }
            }
            else
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "File in persistent path exists");
                if (localFileFound && !ModelFilesEqual())
                {
                    GetData(NnpFile, showMsg);
                }
                else if (!localFileFound && !ModelFilesEqualResources())
                {
                    GetDataFromResources(NnpFile, showMsg);
                }
            }

            if (System.IO.File.Exists(nnpFileFullPath))
            {
                try
                {
                    tfEstimator = new TFLiteEstimator(nnpFileFullPath);
                }
                catch (Exception e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to initialize TFLite: {e.Message}");
                }
                finally
                {
                    if (nnpFileFullPath != null)
                    {
                        System.IO.File.Delete(nnpFileFullPath);
                    }
                }


                this.GestureRingBufferLeft = new RingBuffer();
                this.GestureRingBufferRight = new RingBuffer();

                GestureRingBufferLeft.GestureEstimationRequested += (RecognizeResultProperty handData, bool wasInterpolated) =>
                {
                    this.GetEstimationResults(handData, GestureHand.LeftHand, wasInterpolated);
                };

                GestureRingBufferRight.GestureEstimationRequested += (RecognizeResultProperty handData, bool wasInterpolated) =>
                {
                    this.GetEstimationResults(handData, GestureHand.RightHand, wasInterpolated);

                };

                this.GestureRingBufferLeft.AdjustByAccelerationOnGestureEstimation = this.adjustByAccelerationOnGestureEstimation;
                this.GestureRingBufferRight.AdjustByAccelerationOnGestureEstimation = this.adjustByAccelerationOnGestureEstimation;

                FramesPerGesture = tfEstimator.DataNum / (this.GestureRingBufferLeft.JointArraySize + 9);

                var orientationsProperty = TofArManager.Instance.GetProperty<DeviceOrientationsProperty>();
                if (orientationsProperty != null)
                {
                    OnDeviceOrientationChanged(DeviceOrientation.LandscapeLeft, orientationsProperty.deviceOrientation);
                    OnScreenOrientationChanged(ScreenOrientation.LandscapeLeft, orientationsProperty.screenOrientation);
                }

                if (this.autoStartGestureEstimation)
                {
                    StartGestureEstimation();
                }
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
            TofArTofManager.Instance?.RemoveManagerDependency(this);

            TofArManager.Instance?.RemoveSubManager(this);
            if (this.GestureRingBufferLeft != null)
            {
                this.GestureRingBufferLeft.Dispose();
            }
            if (this.GestureRingBufferRight != null)
            {
                this.GestureRingBufferRight.Dispose();
            }

            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArHandManager.Dispose()");
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
                    this.streamPlay.Stop();
                }
                this.streamPlay.Dispose();
                this.streamPlay = null;
            }

            if (this.tfEstimator != null)
            {
                this.tfEstimator.Free();
                this.tfEstimator = null;
            }
            this.logic = null;
        }

        private void Update()
        {
            if (!this.gestureInitialized)
            {
                this.gestureInitialized = true;
                InitializeGestureRecognition();
            }

            if (this.stream == null)
            {
                return;
            }
            if (this.propertyChanged)
            {
                this.propertyChanged = false;

                ApplySettings();
            }

            this.fromFpsMeasured += Time.unscaledDeltaTime;
            if (this.fromFpsMeasured >= 1.0f)
            {
                this.FrameRate = this.frameCount / this.fromFpsMeasured;
                this.fromFpsMeasured = 0;
                this.frameCount = 0;
            }

            //UpdateCameraRotationProperty();

            if (Instance != null && Instance.newDataArrived)
            {
                Instance.newDataArrived = false;

                // estimate gesture
                if (Instance.IsGestureEstimating)
                {
                    if (this.GestureRingBufferLeft.AdjustByAccelerationOnGestureEstimation != this.adjustByAccelerationOnGestureEstimation)
                    {
                        this.GestureRingBufferLeft.AdjustByAccelerationOnGestureEstimation = this.adjustByAccelerationOnGestureEstimation;
                    }
                    if (this.GestureRingBufferRight.AdjustByAccelerationOnGestureEstimation != this.adjustByAccelerationOnGestureEstimation)
                    {
                        this.GestureRingBufferRight.AdjustByAccelerationOnGestureEstimation = this.adjustByAccelerationOnGestureEstimation;
                    }
                    Instance.timeElapsedLeft += Time.deltaTime;
                    Instance.timeElapsedRight += Time.deltaTime;

                    // Lock gesture
                    lock (lockGesture)
                    {
                        Instance.EstimateGesture();
                    }
                }
            }

        }

        private void ApplySettings()
        {
            var recognizeConfigProperty = GetProperty<RecognizeConfigProperty>();

            recognizeConfigProperty.processLevel = this.processLevel;
            recognizeConfigProperty.recogMode = this.recogMode;
            recognizeConfigProperty.runtimeMode = this.runtimeMode;
            recognizeConfigProperty.runtimeModeAfter = this.runtimeModeAfter;
            recognizeConfigProperty.intervalFramesNotRecognized = this.intervalFramesNotRecognized;
            recognizeConfigProperty.framesForDetectNoHands = this.framesForDetectNoHands;
            recognizeConfigProperty.regionThreads = nRegionThreads;
            recognizeConfigProperty.trackingMode = this.trackingMode;
            recognizeConfigProperty.temporalRecognitionMode = this.temporalRecognitionMode;
            recognizeConfigProperty.pointThreads = nPointsThreads;
            recognizeConfigProperty.isSetThreads = true;
            recognizeConfigProperty.noiseReductionLevel = this.noiseReductionLevel;
            SetProperty(recognizeConfigProperty);
        }

        /// <summary>
        /// ストリーム開始時の最大リトライ回数
        /// </summary>
        public uint maxStreamStartRetry = 10;

        private IEnumerator StartStreamWithRetry(RecognizeConfigProperty configuration)
        {
            var retryCount = 0;
            this.IsStopStreamRequested = false;

            while (!this.IsStreamActive && !this.IsStopStreamRequested && (retryCount < this.maxStreamStartRetry))
            {
                if (this.streamPlay != null && this.streamPlay.IsStarted)
                {
                    this.StopPlayback();
                }
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofAr] TofArHandManager trying start stream #{retryCount}");


                try
                {
                    if (!this.IsStreamActive && this.isUnPaused)
                    {
                        if (configuration != null)
                        {
                            this.SetProperty(configuration);
                        }
                        else
                        {
                            var recognizeConfigProperty = GetProperty<RecognizeConfigProperty>();
                            if (recognizeConfigProperty != null)
                            {
                                recognizeConfigProperty.processLevel = this.processLevel;
                                recognizeConfigProperty.recogMode = this.recogMode;
                                recognizeConfigProperty.runtimeMode = this.runtimeMode;
                                recognizeConfigProperty.runtimeModeAfter = this.runtimeModeAfter;
                                recognizeConfigProperty.intervalFramesNotRecognized = this.intervalFramesNotRecognized;
                                recognizeConfigProperty.framesForDetectNoHands = this.framesForDetectNoHands;
                                recognizeConfigProperty.regionThreads = nRegionThreads;
                                recognizeConfigProperty.trackingMode = this.trackingMode;
                                recognizeConfigProperty.temporalRecognitionMode = this.temporalRecognitionMode;
                                recognizeConfigProperty.pointThreads = nPointsThreads;
                                recognizeConfigProperty.isSetThreads = true;

                                this.SetProperty(recognizeConfigProperty);
                            }
                        }
                        this.Stream?.Start();

                        this.gestureInitialized = false;

                        if (this.startingPlayback)
                        {
                            this.IsPlaying = true;
                        }

                        if (OnStreamStarted != null)
                        {
                            OnStreamStarted(this);
                        }
                    }
                    break;
                }
                catch (ApiException e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"{e.GetType().Name}:{e.Message}\n{e.StackTrace}");
                }
                retryCount++;
                yield return null;
            }
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        /// <param name="configuration">手認識設定</param>
        public void StartStream(RecognizeConfigProperty configuration)
        {
            if (Array.FindIndex(this.SupportedRuntimeModes, x => x == this.runtimeMode) < 0)
            {
                throw new NotSupportedException($"Can't start hand stream. RuntimeMode {this.runtimeMode} not supported on this platform");
            }

            if (Array.FindIndex(this.SupportedRuntimeModes, x => x == this.runtimeModeAfter) < 0)
            {
                throw new NotSupportedException($"Can't start hand stream. RuntimeMode {this.runtimeModeAfter} not supported on this platform");
            }

            if (Array.FindIndex(this.SupportedRecogModes, x => x == this.recogMode) < 0)
            {
                throw new NotSupportedException($"Can't start hand stream. RecogMode {this.recogMode} not supported on this platform");
            }

            if (this.checkForTof)
            {
                bool isTofStarting = Tof.TofArTofManager.Instance?.IsTofStarting == true;
                if (isTofStarting)
                {
                    startConfig = configuration;
                    waitForTof = true;
                    return;
                }
            }
            this.checkForTof = true;

            this.StartCoroutine(this.StartStreamWithRetry(configuration));
        }

        /// <summary>
        /// ストリーミングを開始する
        /// </summary>
        public void StartStream()
        {
            if (Tof.TofArTofManager.Instance.ProcessTargets?.processDepth == false)
            {
                TofArManager.Logger.WriteLog(LogLevel.Info, "Process depth is disabled. Hand won't be processed.");
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
            }
            this.IsStopStreamRequested = true;
            this.streamOpenErrorOccured = false;
        }

        /// <summary>
        /// ジェスチャー推定処理を開始する
        /// </summary>
        public void StartGestureEstimation()
        {
            Instance.IsGestureEstimating = true;
        }

        /// <summary>
        /// ジェスチャー推定処理を停止する
        /// </summary>
        public void StopGestureEstimation()
        {
            Instance.IsGestureEstimating = false;
        }

        static private void StreamCallBack(Stream stream)
        {
            try
            {
                lock (Instance.stopLock)
                {
                    if (!TofArHandManager.Instantiated)
                    {
                        return;
                    }

                    var instance = Instance;
                    var frame = stream.GetFrame();

                    try
                    {
                        var handChannel = frame.Channels[(int)ChannelIds.Hand];
                        var rawData = handChannel.GetRawData();
                        lock (instance.Lock)
                        {
                            instance.HandData = new HandData(rawData);
                        }
                        foreach (var processor in instance.preProcessors)
                        {
                            instance.HandData.Data = processor.ParseHandData(instance.HandData.Data);
                        }

                        instance.newDataArrived = true;

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

                    instance.HandCalc.ProcessHandPoints(instance.HandData);
                }
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("HandManager Stream callback - \n{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            /*catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("HandManager Stream callback - \n{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            catch (NullReferenceException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("HandManager Stream callback - \n{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
            catch (IndexOutOfRangeException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("HandManager Stream callback - \n{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }*/
        }

        /// <summary>
        /// コンポーネントプロパティを取得する
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>() where T : class, IBaseProperty, new()
        {
            if ((this.isUnPaused || this.IsPlaying) && this.Stream != null)
            {
                T property = new T();
                int bufferSize = 1024;
                //handle types that need a larger buffer

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
        /// <param name="buffersize">シリアライズ用バッファサイズ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value, int buffersize) where T : class, IBaseProperty, new()
        {
            if ((this.isUnPaused || this.IsPlaying) && this.Stream != null)
            {
                var stream = (this.IsPlaying && this.streamPlay?.IsStarted == true) ? this.streamPlay : this.Stream;
                stream?.GetProperty<T>(value.Key, ref value, buffersize);
                return value;
            }
            return null;
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="key">プロパティキー</param>
        /// <param name="value">入力パラメータ</param>
        /// <returns></returns>
        public T GetProperty<T>(string key, T value) where T : class, IBaseProperty
        {
            if ((this.isUnPaused || this.IsPlaying) && this.Stream != null)
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
            if (this.isUnPaused && this.Stream != null)
            {
                var recogConfig = value as RecognizeConfigProperty;
                if (recogConfig != null)
                {
                    recogConfig.isSetThreads = true;
                    if (recogConfig.regionThreads < 1)
                    {
                        recogConfig.regionThreads = 1;
                    }
                    if (recogConfig.regionThreads > 4)
                    {
                        recogConfig.regionThreads = 4;
                    }
                    if (recogConfig.pointThreads < 1)
                    {
                        recogConfig.pointThreads = 1;
                    }
                    if (recogConfig.pointThreads > 4)
                    {
                        recogConfig.pointThreads = 4;
                    }
                    this.Stream?.SetProperty(recogConfig);
                }
                else
                {
                    this.Stream?.SetProperty<T>(value);
                }
            }
        }

        /// <summary>
        /// Propertyリスト取得する
        /// </summary>
        /// <returns>Propertyリスト</returns>
        public string[] GetPropertyList()
        {
            return this.Stream?.GetPropertyList();
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

            if (pause)
            {
                string nnpTmp = Application.persistentDataPath + "/" + NnpFile;
                if (System.IO.File.Exists(nnpTmp))
                {
                    System.IO.File.Delete(nnpTmp);
                }
            }
            else
            {
                if (this.gestureInitialized)
                {
                    InitializeGestureRecognition(false); // don't show message when unpausing
                }
            }
        }

        private void EstimateGesture()
        {
            if ((this.HandData == null) || (this.HandData.Data == null))
            {
                return;
            }
            var instance = TofArHandManager.Instance;
            var handData = new RecognizeResultProperty();
            lock (instance.Lock)
            {
                handData.featurePointsLeft = instance.HandData.Data.featurePointsLeft;
                handData.featurePointsRight = instance.HandData.Data.featurePointsRight;
                handData.poseLevelsLeft = instance.HandData.Data.poseLevelsLeft;
                handData.poseLevelsRight = instance.HandData.Data.poseLevelsRight;
                handData.handStatus = instance.HandData.Data.handStatus;
            }


            bool successLeft = this.GestureRingBufferLeft.Fill(handData, HandStatus.LeftHand);
            bool successRight = this.GestureRingBufferRight.Fill(handData, HandStatus.RightHand);

            if (!successLeft && !successRight)
            {
                if (OnGestureEstimatedDefault != null)
                {
                    OnGestureEstimatedDefault(Instance, new GestureResultProperty()
                    {
                        gestureIndex = GestureIndex.None,
                        gestureHand = GestureHand.Unknown,
                        isCallback = false,
                        wasInterpolated = false
                    });
                }
            }
        }

        private int GetLastIndex()
        {
            return gestureEstimationFrames * FramesPerGesture;
        }

        private void GetEstimationResults(RecognizeResultProperty handData, GestureHand hand, bool wasInterpolated)
        {
            this.lastIndex = GetLastIndex();

            CheckBuffers();

            RingBuffer ringBuffer = hand == GestureHand.LeftHand ? this.GestureRingBufferLeft : this.GestureRingBufferRight;

            int threshold = (this.FramesPerGesture - 1) * this.gestureEstimationFrames + 1;
            if (ringBuffer.DataCount >= threshold)
            {
                ProcessResult(handData.handStatus, hand, wasInterpolated);
            }
            else
            {
                if (OnGestureEstimatedDefault != null)
                {
                    OnGestureEstimatedDefault(Instance, new GestureResultProperty()
                    {
                        gestureIndex = GestureIndex.None,
                        gestureHand = hand,
                        isCallback = false,
                        wasInterpolated = wasInterpolated
                        //gestureHand = GestureHand.Unknown
                    });
                }
            }

        }

        private void ProcessResult(HandStatus handStatus, GestureHand hand, bool wasInterpolated)
        {
            RingBuffer ringBuffer = hand == GestureHand.LeftHand ? this.GestureRingBufferLeft : this.GestureRingBufferRight;
            GestureIndex[] gestureResultList = hand == GestureHand.LeftHand ? this.gestureResultListLeft : this.gestureResultListRight;
            float timeElapsed = hand == GestureHand.LeftHand ? this.timeElapsedLeft : this.timeElapsedRight;
            HandStatus[] handStatusList = hand == GestureHand.LeftHand ? this.handStatusListLeft : this.handStatusListRight;
            GestureIndex lastGestureId = hand == GestureHand.LeftHand ? this.lastGestureIdLeft : this.lastGestureIdRight;

            GestureHand gestureHand = hand;
            var nblaResult = tfEstimator.Forward(ringBuffer.Buffer, ringBuffer.TopIndex, this.FramesPerGesture, this.lastIndex, this.FramesPerSec);

            NotifyGestureDataRead(gestureHand);

            //int maxIdx = 0;
            //float maxResult = nblaResult[0];

            //for (int i = 0; i < nblaResult.Length; i++)
            //{
            //    if (nblaResult[i] > maxResult)
            //    {
            //        maxResult = nblaResult[i];
            //        maxIdx = i;
            //    }
            //}

            //GestureIndex gesture = (GestureIndex)maxIdx;
            GestureIndex gesture = this.logic.GetGesture(nblaResult);
            int gestureIndex = (int)gesture;

            for (int i = 0; i < gestureEstimationFrames - 1; i++)
            {

                gestureResultList[i] = gestureResultList[i + 1];
                handStatusList[i] = handStatusList[i + 1];
            }

            handStatusList[gestureEstimationFrames - 1] = handStatus;
            gestureResultList[gestureEstimationFrames - 1] = gesture;


            int[] lastResults = new int[nblaResult.Length];

            lastResults[gestureIndex]++;


            // if waited long enough OR if current gestureId is different from last valid gestureId
            bool waitFinished = false;

            if (gesture != lastGestureId)
            {
                waitFinished = true;
            }
            else
            {
                waitFinished = (timeElapsed > GestureIndexNotifyInterval[(int)lastGestureId]);
            }

            bool isMasked = gestureIndex < GestureIndexMask.Length ? GestureIndexMask[gestureIndex] : true;

            if (waitFinished && isMasked && gesture != GestureIndex.Others)
            {
                if (GestureOkForCallback(gesture, gestureResultList, lastResults))
                {
                    if (hand == GestureHand.LeftHand)
                    {
                        timeElapsedLeft = 0f;
                        lastGestureIdLeft = gesture;
                    }
                    else
                    {
                        timeElapsedRight = 0f;
                        lastGestureIdRight = gesture;
                    }

                    var gestureResult = new GestureResultProperty()
                    {
                        gestureIndex = gesture,
                        gestureHand = gestureHand,
                        isCallback = true,
                        wasInterpolated = wasInterpolated
                    };
                    this.InvokeGestureEstimatedEvent(gestureResult);
                }
            }

            if (gesture == GestureIndex.Others)
            {
                gesture = GestureIndex.None;
            }
            var gestureResultDefault = new GestureResultProperty()
            {
                gestureIndex = gesture,
                gestureHand = gestureHand,
                isCallback = false,
                wasInterpolated = wasInterpolated
            };
            this.InvokeGestureEstimatedDefaultEvent(gestureResultDefault);
        }

        internal void InvokeGestureEstimatedEvent(GestureResultProperty gestureResult)
        {
            if (OnGestureEstimated != null)
            {
                OnGestureEstimated(Instance, gestureResult);
            }
        }

        internal void InvokeGestureEstimatedDefaultEvent(GestureResultProperty gestureResultDefault)
        {
            if (OnGestureEstimatedDefault != null)
            {
                OnGestureEstimatedDefault(Instance, gestureResultDefault);
            }
        }

        private void NotifyGestureDataRead(GestureHand gestureHand)
        {
            if (gestureHand == GestureHand.BothHands)
            {
                if (OnLeftGestureDataRead != null)
                {
                    OnLeftGestureDataRead.Invoke(this.GestureRingBufferLeft.Buffer, this.FramesPerGesture, this.GestureRingBufferLeft.TopIndex, this.lastIndex);
                }
                if (OnRightGestureDataRead != null)
                {
                    OnRightGestureDataRead.Invoke(this.GestureRingBufferRight.Buffer, this.FramesPerGesture, this.GestureRingBufferRight.TopIndex, this.lastIndex);
                }
            }
            else
            {
                if (gestureHand == GestureHand.LeftHand)
                {
                    if (OnLeftGestureDataRead != null)
                    {
                        OnLeftGestureDataRead.Invoke(this.GestureRingBufferLeft.Buffer, this.FramesPerGesture, this.GestureRingBufferLeft.TopIndex, this.lastIndex);
                    }
                }
                else
                {
                    if (OnRightGestureDataRead != null)
                    {
                        OnRightGestureDataRead.Invoke(this.GestureRingBufferRight.Buffer, this.FramesPerGesture, this.GestureRingBufferRight.TopIndex, this.lastIndex);
                    }
                }
            }
        }

        private bool GestureOkForCallback(GestureIndex gesture, GestureIndex[] gestureResultList, int[] lastResults)
        {
            int gestureIndex = (int)gesture;

            for (int i = 0; i < gestureEstimationFrames - 1; i++)
            {
                int idx = (int)gestureResultList[i];
                lastResults[idx]++;

                // More than X% of last results are equal
                if (lastResults[gestureIndex] >= (gestureRecogThreshold * gestureEstimationFrames))
                {
                    return true;
                }
            }

            return false;
        }

        void CheckBuffers()
        {
            int gestureEstimationFrames = this.gestureEstimationFrames;

            if (gestureResultListLeft.Length != gestureEstimationFrames)
            {
                Array.Resize(ref gestureResultListLeft, gestureEstimationFrames);
            }
            if (gestureResultListRight.Length != gestureEstimationFrames)
            {
                Array.Resize(ref gestureResultListRight, gestureEstimationFrames);
            }
            if (handStatusListLeft.Length != gestureEstimationFrames)
            {
                Array.Resize(ref handStatusListLeft, gestureEstimationFrames);
            }
            if (handStatusListRight.Length != gestureEstimationFrames)
            {
                Array.Resize(ref handStatusListRight, gestureEstimationFrames);
            }
        }

        private byte[] Checksum(System.IO.Stream stream)
        {
            System.Security.Cryptography.SHA256 sha256 = null;
            byte[] checksum = null;
            try
            {
                sha256 = System.Security.Cryptography.SHA256.Create();
                checksum = sha256.ComputeHash(stream);
            }
            finally
            {
                if (sha256 != null)
                {
                    sha256.Clear();
                }
            }
            return checksum;
        }

        private bool ModelFilesEqual()
        {
            if (System.IO.File.Exists(pathToLocalFile) && !Utils.PathIsSymlink(pathToLocalFile))
            {
                byte[] checksum0 = null, checksum1 = null;
                using (System.IO.Stream s = System.IO.File.OpenRead(pathToLocalFile))
                {
                    checksum0 = Checksum(s);
                    s.Dispose();
                    if (checksum0 == null)
                    {
                        return false;
                    }
                }

                using (System.IO.Stream streamPersistent = System.IO.File.OpenRead(Application.persistentDataPath + "/" + NnpFile))
                {
                    checksum1 = Checksum(streamPersistent);
                    streamPersistent.Dispose();
                    if (checksum1 == null)
                    {
                        return false;
                    }
                }

                int nBytes = checksum0.Length;
                if (nBytes != checksum1.Length)
                {
                    return false;
                }
                for (int i = 0; i < nBytes; i++)
                {
                    if (checksum0[i] != checksum1[i])
                    {
                        return false;
                    }
                }

                return true;

            }

            return false;
        }

        private bool ModelFilesEqualResources()
        {
            TextAsset asset = Resources.Load("result") as TextAsset;
            if (asset != null && !Utils.PathIsSymlink(Application.persistentDataPath + "/" + NnpFile))
            {
                System.IO.Stream s = new System.IO.MemoryStream(asset.bytes);
                var checksum0 = Checksum(s);
                s.Close();
                if (checksum0 == null)
                {
                    return false;
                }

                System.IO.Stream streamPersistent = System.IO.File.OpenRead(Application.persistentDataPath + "/" + NnpFile);
                var checksum1 = Checksum(streamPersistent);
                streamPersistent.Close();
                if (checksum1 == null)
                {
                    return false;
                }

                int nBytes = checksum0.Length;

                if (nBytes != checksum1.Length)
                {
                    return false;
                }
                for (int i = 0; i < nBytes; i++)
                {
                    if (checksum0[i] != checksum1[i])
                    {
                        return false;
                    }
                }

                return true;

            }

            return false;
        }

        private string GetData(string file, bool showMsg = false)
        {
            string toPath;

            if (System.IO.File.Exists(pathToLocalFile) && !Utils.PathIsSymlink(pathToLocalFile))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Use local file");
                string creationDate = new System.IO.FileInfo(pathToLocalFile).CreationTime.ToShortDateString();
                string creationTime = new System.IO.FileInfo(pathToLocalFile).CreationTime.ToShortTimeString();
                string msg = string.Format("Hand model file replaced.\nCreated on {0} \nat {1}", creationDate, creationTime);

                using (System.IO.Stream s = new System.IO.FileStream(pathToLocalFile, System.IO.FileMode.Open))
                {
                    System.IO.BinaryReader br = new System.IO.BinaryReader(s);
                    toPath = Application.persistentDataPath + "/" + file;

                    System.IO.File.WriteAllBytes(toPath, br.ReadBytes((int)s.Length));
                    s.Dispose();

                    if (showMsg && OnReplacedModelFile != null)
                    {
                        OnReplacedModelFile.Invoke(msg);
                    }
                    return toPath;
                }
            }

            return null;
        }

        private string GetDataFromResources(string file, bool showMsg = false)
        {
            string toPath;

            string msg = "Hand model file replaced with new file from Resources.";

            TextAsset asset = Resources.Load(System.IO.Path.GetFileNameWithoutExtension(file)) as TextAsset;
            if (asset != null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Use resource file");
                System.IO.Stream s = new System.IO.MemoryStream(asset.bytes);
                System.IO.BinaryReader br = new System.IO.BinaryReader(s);
                toPath = Application.persistentDataPath + "/" + file;

                System.IO.File.WriteAllBytes(toPath, br.ReadBytes(asset.bytes.Length));

                if (showMsg && OnReplacedModelFile != null)
                {
                    OnReplacedModelFile.Invoke(msg);
                }

                return toPath;
            }

            return null;
        }
#if TOFAR_HAND_TEST

        const int featurePointsLength = 25;
        const int poseLevelsLength = 15;
        /// <summary>
        /// TODO+ C
        /// </summary>
        public const int handDataSize = sizeof(float) * (1 + featurePointsLength * 3 * 2 + poseLevelsLength * 2);
        class dummyHandData : HandData
        {
            internal dummyHandData(byte[] data) : base(new RawData())
            {
                this.Data = new RecognizeResultProperty();

                if (data.Length < handDataSize)
                {
                    return;
                }
                var offset = 0;
                var hand = BitConverter.ToInt32(data, 0);
                offset += 4;

                var featurePointsLeft = new Vector3[featurePointsLength];
                var featurePointsRight = new Vector3[featurePointsLength];

                /*float[] featurePointsFloatLeft = new float[featurePointsLength * 3];
                float[] featurePointsFloatRight = new float[featurePointsLength * 3];*/

                int processTime = 0;

                for (var i = 0; i < featurePointsLength; i++)
                {
                    var x = BitConverter.ToSingle(data, offset) / 1000;
                    var y = BitConverter.ToSingle(data, offset + 4) / 1000;
                    var z = BitConverter.ToSingle(data, offset + 8) / 1000;

                    featurePointsLeft[i] = new Vector3(-y, x, z);

                    offset += 12;


                    /*featurePointsFloatLeft[i * 3 + 0] = -x * 1000;
                    featurePointsFloatLeft[i * 3 + 1] = y * 1000;
                    featurePointsFloatLeft[i * 3 + 2] = z * 1000;*/
                }

                for (var i = 0; i < featurePointsLength; i++)
                {
                    var x = BitConverter.ToSingle(data, offset) / 1000;
                    var y = BitConverter.ToSingle(data, offset + 4) / 1000;
                    var z = BitConverter.ToSingle(data, offset + 8) / 1000;

                    featurePointsRight[i] = new Vector3(-y, x, z);

                    offset += 12;


                    /*featurePointsFloatRight[i * 3 + 0] = x * 1000;
                    featurePointsFloatRight[i * 3 + 1] = y * 1000;
                    featurePointsFloatRight[i * 3 + 2] = z * 1000;*/
                }

                processTime = BitConverter.ToInt32(data, offset);

                float[] poseAccuraciesLeft = new float[poseLevelsLength], poseAccuraciesRight = new float[poseLevelsLength];

                float maxVal = 0;
                //  int maxIdx = 0;

                for (var i = 0; i < poseLevelsLength; i++)
                {
                    var x = BitConverter.ToSingle(data, offset);

                    poseAccuraciesLeft[i] = x;

                    if (x > maxVal)
                    {
                        maxVal = x;
                    }

                    offset += 4;
                }

                for (var i = 0; i < poseLevelsLength; i++)
                {
                    var x = BitConverter.ToSingle(data, offset);

                    poseAccuraciesRight[i] = x;

                    if (x > maxVal)
                    {
                        maxVal = x;
                    }

                    offset += 4;

                }

                this.Data = new RecognizeResultProperty()
                {
                    featurePointsLeft = featurePointsLeft,
                    featurePointsRight = featurePointsRight,
                    handStatus = (HandStatus)hand,
                    poseLevelsRight = poseAccuraciesRight,
                    poseLevelsLeft = poseAccuraciesLeft,
                    processTime = processTime
                };
            }
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        /// <param name="data">TODO+ C</param>
        static public void StreamCallBackForTest(byte[] data)
        {
            try
            {
                var instance = TofArHandManager.Instance;

                // inject the test data here!
                instance.HandData = new dummyHandData(data);
                instance.frameCount++;

                if (OnFrameArrived != null)
                {
                    OnFrameArrived(instance);
                }
                instance.HandCalc.ProcessHandPoints(instance.HandData);
            }
            catch (ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("{0} : {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace));
            }
        }
#endif

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

        #region PLAYBACK

        /// <summary>
        /// *TODO+ B
        /// 再生ストリームキー
        /// </summary>
        private const string streamKeyPlay = "player_hand_stream";

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
                        this.streamPlay = TofArManager.Instance.SensCordCore.OpenStream(streamKeyPlay);
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
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default && !Utils.DirectoryContainsNoSymlinks(path))
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"path {path} contained symlink");
                return;
            }
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
                    stream.GetProperty<RecognizeConfigProperty>();
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

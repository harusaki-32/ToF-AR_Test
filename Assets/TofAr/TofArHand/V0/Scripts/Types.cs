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
using MessagePack;
using SensCord;
using UnityEngine;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// *TODO+ B
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        Hand = 0,
    }

    /// <summary>
    /// 認識対象の手
    /// </summary>
    public enum UserHand : int
    {
        /// <summary>
        /// 右手
        /// </summary>
        RightHand,
        /// <summary>
        /// 左手
        /// </summary>
        LeftHand,
        /// <summary>
        /// 自動選択
        /// </summary>
        AutoSelection
    }

    /// <summary>
    /// 手の認識状態
    /// </summary>
    public enum HandStatus : int
    {
        /// <summary>
        /// 右手
        /// </summary>
        RightHand,
        /// <summary>
        /// 左手
        /// </summary>
        LeftHand,
        /// <summary>
        /// 手が無い
        /// </summary>
        NoHand,
        /// <summary>
        /// 指
        /// </summary>
        Tip,
        /// <summary>
        /// 両手
        /// </summary>
        BothHands,
        /// <summary>
        /// 不明
        /// </summary>
        UnknownHand
    }

    /// <summary>
    /// 認識速度と精度
    /// </summary>
    [System.Obsolete("ProcessMode is obsolete and will be removed in a future version")]
    public enum ProcessMode : int
    {
        /// <summary>
        /// 高速低精度
        /// </summary>
        Fast,
        /// <summary>
        /// 速度-精度バランス
        /// </summary>
        Balanced,
        /// <summary>
        /// 低速高精度
        /// </summary>
        Accurate
    }

    /// <summary>
    /// 実行モード
    /// </summary>
    public enum RuntimeMode : int
    {
        /// <summary>
        /// CPUで処理を行う
        /// </summary>
        Cpu,
        /// <summary>
        /// GPUで処理を行う
        /// </summary>
        Gpu,
        /// <summary>
        /// DSPで処理を行う
        /// </summary>
        Dsp,
        /// <summary>
        /// SameAsRegionで処理を行う
        /// </summary>
        SameAsRegion,
        /// <summary>
        /// XNNPACKで処理を行う
        /// </summary>
        XnnPack,
        /// <summary>
        /// CoreMLで処理を行う
        /// </summary>
        CoreML
    }

    /// <summary>
    /// 認識モード
    /// </summary>
    public enum RecogMode : int
    {
        /// <summary>
        /// 片手スマホ持ちで片手認識
        /// </summary>
        OneHandHoldSmapho,
        /// <summary>
        /// フロントカメラで両手重心認識
        /// </summary>
        Face2Face,
        /// <summary>
        /// Cardboard VR用両手認識
        /// </summary>
        HeadMount
    }

    /// <summary>
    /// 手の回転補正有無
    /// </summary>
    [System.Obsolete("RotCorrection is obsolete and will be removed in a future version")]
    public enum RotCorrection : int
    {
        /// <summary>
        /// 手の回転補正なし
        /// </summary>
        Off,
        /// <summary>
        /// 手の回転補正あり
        /// </summary>
        On
    }

    /// <summary>
    /// 認識ステップ
    /// </summary>
    public enum ProcessLevel : int
    {
        /// <summary>
        /// 手検出のみ行う (CameraFacingがFrontの時のみ有効)
        /// </summary>
        HandCenterOnly,
        /// <summary>
        /// 手検出と手特徴点検出を行う
        /// </summary>
        HandPoints,
    }

    /// <summary>
    /// デバイスの向き
    /// </summary>
    public enum CameraOrientation : int
    {
        /// <summary>
        /// 端末上側が上部となる縦
        /// </summary>
        Portrait = 0,
        /// <summary>
        /// 端末下側が上部となる縦
        /// </summary>
        PortraitUpsideDown = 1,
        /// <summary>
        /// 端末右側が上部となる横
        /// </summary>
        LandscapeRight = 2,
        /// <summary>
        /// 端末左側が上部となる横
        /// </summary>
        LandscapeLeft = 3,
    };

    /// <summary>
    /// スムージングモード
    /// </summary>
    public enum NoiseReductionLevel : byte
    {
        /// <summary>
        /// Low
        /// </summary>
        Low,
        /// <summary>
        /// Middle
        /// </summary>
        Middle,
        /// <summary>
        /// High
        /// </summary>
        High
    }

    /// <summary>
    /// 手特徴点インデックス
    /// </summary>
    public enum HandPointIndex : int
    {
        /// <summary>
        /// 小指の指先
        /// </summary>
        PinkyTip,
        /// <summary>
        /// 小指の第２関節
        /// </summary>
        PinkyJoint,
        /// <summary>
        /// 薬指の指先
        /// </summary>
        RingTip,
        /// <summary>
        /// 薬指の第２関節
        /// </summary>
        RingJoint,
        /// <summary>
        /// 中指の指先
        /// </summary>
        MidTip,
        /// <summary>
        /// 中指の第２関節
        /// </summary>
        MidJoint,
        /// <summary>
        /// 人差し指の指先
        /// </summary>
        IndexTip,
        /// <summary>
        /// 人差し指の第２関節
        /// </summary>
        IndexJoint,
        /// <summary>
        /// 親指の指先
        /// </summary>
        ThumbTip,
        /// <summary>
        /// 親指の第１関節
        /// </summary>
        ThumbJoint,
        /// <summary>
        /// 親指の第２関節
        /// </summary>
        ThumbRoot,
        /// <summary>
        /// 手首(小指側)
        /// </summary>
        WristPinkySide,
        /// <summary>
        /// 手首(親指側)
        /// </summary>
        WristThumbSide,
        /// <summary>
        /// 手の中心
        /// </summary>
        HandCenter,
        /// <summary>
        /// 腕の中心
        /// </summary>
        ArmCenter,
        /// <summary>
        /// 小指の第3関節
        /// </summary>
        PinkyRoot,
        /// <summary>
        /// 薬指の第3関節
        /// </summary>
        RingRoot,
        /// <summary>
        /// 中指の根元
        /// </summary>
        MidRoot,
        /// <summary>
        /// 人差し指の根元
        /// </summary>
        IndexRoot,
        /// <summary>
        /// 手首
        /// </summary>
        Wrist,
        /// <summary>
        /// 小指の第１関節
        /// </summary>
        PinkyJoint1st,
        /// <summary>
        /// 薬指の第１関節
        /// </summary>
        RingJoint1st,
        /// <summary>
        /// 中指の第１関節
        /// </summary>
        MidJoint1st,
        /// <summary>
        /// 人差し指の第１関節
        /// </summary>
        IndexJoint1st,
        /// <summary>
        /// 親指の根元
        /// </summary>
        ThumbRootWrist
    }

    /// <summary>
    /// ポーズインデックス
    /// </summary>
    public enum PoseIndex : int
    {
        /// <summary>
        /// 手を握る (0)
        /// </summary>
        Fist,
        /// <summary>
        /// 人差し指だけ伸ばす (1)
        /// </summary>
        Shot,
        /// <summary>
        /// ピース (2)
        /// </summary>
        Peace,
        /// <summary>
        /// 親指だけ折る (3)
        /// </summary>
        ThumbIn,
        /// <summary>
        /// 小指だけ伸ばす (4)
        /// </summary>
        PinkyOut,
        /// <summary>
        /// 手を開く (5)
        /// </summary>
        OpenPalm,
        /// <summary>
        /// ピストル (6)
        /// </summary>
        Pistol,
        /// <summary>
        /// 親指と人差し指だけ曲げる (7)
        /// </summary>
        OK,
        /// <summary>
        /// 中指と薬指を折る (8)
        /// </summary>
        MiddleAndRingIn,
        /// <summary>
        /// 指を3本伸ばす (9)
        /// </summary>
        ThreeFingers,
        /// <summary>
        /// サムアップ (10)
        /// </summary>
        ThumbUp,
        /// <summary>
        /// 親指と小指を伸ばす (11)
        /// </summary>
        Tel,
        /// <summary>
        /// キツネ (12)
        /// </summary>
        Fox,
        /// <summary>
        /// スナップする前(13)
        /// </summary>
        PreSnap,
        /// <summary>
        /// ハート(14)
        /// </summary>
        Heart,

        /// <summary>
        /// ポーズが認識されていない (999)
        /// </summary>
        None = 999,
    }

    /// <summary>
    /// ジェスチャーインデックス
    /// </summary>
    public enum GestureIndex : int
    {
        /// <summary>
        /// ジェスチャーが推定されていない (0)
        /// </summary>
        None,
        /// <summary>
        /// 未定義のジェスチャー (1)
        /// </summary>
        Others,
        /// <summary>
        /// Bloom (2)
        /// </summary>
        Bloom,
        /// <summary>
        /// AirTap (3)
        /// </summary>
        AirTap,
        /// <summary>
        /// SnapFinger (4)
        /// </summary>
        SnapFinger,
        /// <summary>
        /// FingerThrow (5)
        /// </summary>
        FingerThrow,
        /// <summary>
        /// HandThrow (6)
        /// </summary>
        HandThrow,
        /// <summary>
        /// Shoot (7)
        /// </summary>
        Shoot,
        /// <summary>
        /// Punch (8)
        /// </summary>
        Punch,
        /// <summary>
        /// Milk (9)
        /// </summary>
        Milk,
        /// <summary>
        /// Bye (10)
        /// </summary>
        Bye,
        /// <summary>
        /// HandSwipe (11)
        /// </summary>
        HandSwipe,
        /// <summary>
        /// ThumbTap (12)
        /// </summary>
        ThumbTap,
        /// <summary>
        /// TurnKnob (13)
        /// </summary>
        TurnKnob,
        /// <summary>
        /// Finish (14)
        /// </summary>
        Finish,
        /// <summary>
        /// Eat (15)
        /// </summary>
        Eat,
        /// <summary>
        /// Twinkle (16)
        /// </summary>
        Twinkle,
        /// <summary>
        /// Hobby (17)
        /// </summary>
        Hobby,
        /// <summary>
        /// Beard (18)
        /// </summary>
        Beard,
        /// <summary>
        /// Nose (19)
        /// </summary>
        Nose,
        /// <summary>
        /// ComeOn (20)
        /// </summary>
        ComeOn,
        /// <summary>
        /// Flick (21)
        /// </summary>
        Flick,
        /// <summary>
        /// Darts (22)
        /// </summary>
        Darts,
        /// <summary>
        /// Chop (23)
        /// </summary>
        Chop,
        /// <summary>
        /// ReverseSwipe (24)
        /// </summary>
        ReverseSwipe,
    }

    /// <summary>
    /// ジェスチャーが推定された手
    /// </summary>
    public enum GestureHand : int
    {
        /// <summary>
        /// 不明
        /// </summary>
        Unknown,
        /// <summary>
        /// 右手
        /// </summary>
        RightHand,
        /// <summary>
        /// 左手
        /// </summary>
        LeftHand,
        /// <summary>
        /// 両手
        /// </summary>
        BothHands
    }

    /// <summary>
    /// 手認識結果
    /// </summary>
    public class RecognizeResultProperty
    {
        /// <summary>
        /// 手の認識状態
        /// </summary>
        public HandStatus handStatus;
        /// <summary>
        /// 左手特徴点と腕特徴点のカメラ座標系でのXYZ座標
        /// </summary>
        public Vector3[] featurePointsLeft = { };
        /// <summary>
        /// 右手特徴点と腕特徴点のカメラ座標系でのXYZ座標
        /// </summary>
        public Vector3[] featurePointsRight = { };
        /// <summary>
        /// 左手のポーズの認識レベル
        /// </summary>
        public float[] poseLevelsLeft = { };
        /// <summary>
        /// 右手のポーズの認識レベル
        /// </summary>
        public float[] poseLevelsRight = { };
        /// <summary>
        /// 処理時間
        /// </summary>
        public int processTime;
    }


    /// <summary>
    /// 手認識基本設定
    /// </summary>
    [MessagePackObject]
    public class RecognizeConfigProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArHandRecognizeConfigKey";
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 認識速度と精度の指定
        /// <para>デフォルト値: Balanced</para>
        /// </summary>
        [Key("processMode")]
        [System.Obsolete("processMode is obsolete and will be removed in a future version")]
        public ProcessMode processMode;

        /// <summary>
        /// 認識ステップの指定
        /// <para>デフォルト値: HandPoints</para>
        /// </summary>
        [Key("processLevel")]
        public ProcessLevel processLevel;

        /// <summary>
        /// 実行モード(前段処理)の指定
        /// <para>デフォルト値: Gpu</para>
        /// </summary>
        [Key("segmentRuntimeMode")]
        public RuntimeMode runtimeMode;

        /// <summary>
        /// 実行モード(後段処理)の指定
        /// <para>デフォルト値: Gpu</para>
        /// </summary>
        [Key("pointRuntimeMode")]
        public RuntimeMode runtimeModeAfter;

        /// <summary>
        /// Depth画像横解像度
        /// </summary>
        [Key("imageWidth")]
        public int imageWidth;

        /// <summary>
        /// Depth画像縦解像度
        /// </summary>
        [Key("imageHeight")]
        public int imageHeight;

        /// <summary>
        /// 横FOV
        /// </summary>
        [Key("horizontalFovDeg")]
        public double horizontalFovDeg;

        /// <summary>
        /// 縦FOV
        /// </summary>
        [Key("verticalFovDeg")]
        public double verticalFovDeg;

        /// <summary>
        /// 認識モードの指定
        /// <para>デフォルト値: OneHandHoldSmapho</para>
        /// </summary>
        [Key("recogMode")]
        public RecogMode recogMode;

        /// <summary>
        /// 手の回転補正有無
        /// <para>デフォルト値: RotCorrectionOn</para>
        /// </summary>
        [Key("rotCorrection")]
        [System.Obsolete("rotCorrection is obsolete and will be removed in a future version")]
        public RotCorrection rotCorrection;

        /// <summary>
        /// 手が検出されていない時の認識処理間隔フレーム数
        /// <para>デフォルト値: 10</para>
        /// </summary>
        [Key("intervalFramesNotRecognized")]
        public int intervalFramesNotRecognized;

        /// <summary>
        /// このフレーム数のNoHandsが連続して検出されると intervalFramesNotRecognized で指定されたインターバル動作を開始する (デフォルト:3)
        /// </summary>
        [Key("framesForDetectNoHands")]
        public int framesForDetectNoHands;

        /// <summary>
        /// トラッキングモード
        /// </summary>
        [Key("trackingMode")]
        public bool trackingMode;

        /// <summary>
        /// 一時的認識モード
        /// </summary>
        [Key("temporalRecognitionMode")]
        public bool temporalRecognitionMode;

        /// <summary>
        /// <para>true: スレッド化する</para>
        /// <para>false: スレッド化しない</para>
        /// <para>デフォルト値: false</para>
        /// </summary>
        [Key("isSetThreads")]
        public bool isSetThreads = false;

        /// <summary>
        /// Regionスレッド数
        /// </summary>
        [Key("regionThreads")]
        public int regionThreads = 1;

        /// <summary>
        /// Pointスレッド数
        /// </summary>
        [Key("pointThreads")]
        public int pointThreads = 1;

        /// <summary>
        /// スムージングモード
        /// </summary>
        [Key("noiseReductionLevel")]
        public NoiseReductionLevel noiseReductionLevel = NoiseReductionLevel.Low;

    }

    /// <summary>
    /// カメラ向きの指定
    /// <para>※ iOSでは使用できません</para>
    /// </summary>
    [MessagePackObject]
    public class CameraOrientationProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>        
        public static readonly string ConstKey = "kTofArHandCameraOrientationKey";
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// カメラ向きの指定
        /// </summary>
        [Key("cameraOrientation")]
        public CameraOrientation cameraOrientation;

    }

    /// <summary>
    /// ジェスチャー推定結果
    /// </summary>
    public class GestureResultProperty
    {
        /// <summary>
        /// ジェスチャーが推定された手
        /// </summary>
        public GestureHand gestureHand;
        /// <summary>
        /// 推定されたジェスチャーのインデックス
        /// </summary>
        public GestureIndex gestureIndex;
        /// <summary>
        /// <para>true: コールバックする</para>
        /// <para>false: コールバックしない</para>
        /// </summary>
        public bool isCallback;
        /// <summary>
        /// true: 推定時に補間が行われた
        /// </summary>
        public bool wasInterpolated;
    }

}

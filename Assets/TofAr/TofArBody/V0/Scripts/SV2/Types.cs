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

using MessagePack;
using SensCord;

namespace TofAr.V0.Body.SV2
{
    /// <summary>
    /// 認識モード
    /// </summary>
    public enum RecogMode : int
    {
        /// <summary>
        /// フロントカメラ映像で認識する
        /// </summary>
        Face2Face = 0
    };

    /// <summary>
    /// 処理レベル
    /// </summary>
    public enum ProcessLevel : int
    {
        /// <summary>
        /// 人体の特徴点を推定する
        /// </summary>
        HumanPoints = 0,
    };

    /// <summary>
    /// 実行モード 
    /// </summary>
    public enum RuntimeMode : int
    {
        /// <summary>
        /// CPUで処理を行う
        /// </summary>
        Cpu = 0,
        /// <summary>
        /// GPUで処理を行う
        /// </summary>
        Gpu = 1,
        /// <summary>
        /// CoreMLで処理を行う
        /// </summary>
        CoreML = 2,
    };

    /// <summary>
    /// カメラの向き
    /// </summary>
    public enum CameraOrientation : int
    {
        /// <summary>
        /// ホームボタンが下側にある
        /// </summary>
        Portrait = 0,

        /// <summary>
        /// ホームボタンが上側にある
        /// </summary>
        PortraitUpsideDown = 1,

        /// <summary>
        /// ホームボタンが左側にある
        /// </summary>
        LandscapeRight = 2,

        /// <summary>
        /// ホームボタンが右側にある
        /// </summary>
        LandscapeLeft = 3
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
    /// 認識設定
    /// </summary>
    [MessagePackObject]
    public class RecognizeConfigProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>        
        public static readonly string ConstKey = "kTofArBodyRecognizeConfigKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 処理レベル
        /// </summary>
        [Key("processLevel")]
        public ProcessLevel processLevel;
        /// <summary>
        /// 実行モード
        /// </summary>
        [Key("runtimeMode")]
        public RuntimeMode runtimeMode;
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
        /// 認識モード
        /// </summary>
        [Key("recogMode")]
        public RecogMode recogMode;

        /// <summary>
        /// <para>true: スレッド化する</para>
        /// <para>false: スレッド化しない</para>
        /// <para>デフォルト値: false</para>
        /// </summary>
        [Key("isSetThreads")]
        public bool isSetThreads = false;

        /// <summary>
        /// スレッド数
        /// </summary>
        [Key("nThreads")]
        public int nThreads = 1;

        /// <summary>
        /// スムージングモード
        /// </summary>
        [Key("noiseReductionLevel")]
        public NoiseReductionLevel noiseReductionLevel = NoiseReductionLevel.Low;
    }

    /// <summary>
    /// カメラ方向の指定
    /// </summary>
    [MessagePackObject]
    public class CameraOrientationProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>          
        public static readonly string ConstKey = "kTofArBodyCameraOrientationKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>  
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// カメラ方向
        /// </summary>
        [Key("cameraOrientation")]
        public CameraOrientation cameraOrientation;
    }

}

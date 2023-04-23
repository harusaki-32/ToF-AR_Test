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
using System;

namespace TofAr.V0.Color
{
    /// <summary>
    /// ColorRawデータの種類
    /// </summary>
    public class ColorRawDataType
    {
        /// <summary>
        /// BGRAフォーマット
        /// </summary>
        public static string BGRA = "BGRA";
        /// <summary>
        /// NV21フォーマット
        /// </summary>
        public static string NV21 = "NV21";
        /// <summary>
        /// YUV420フォーマット
        /// </summary>
        public static string YUV420 = "YUV420";
        /// <summary>
        /// BGRフォーマット
        /// </summary>
        public static string BGR = "BGR";
        /// <summary>
        /// RGBフォーマット
        /// </summary>
        public static string RGB = "RGB";
        /// <summary>
        /// RGBAフォーマット
        /// </summary>
        public static string RGBA = "RGBA";
    }

    /// <summary>
    /// Colorデータのフォーマット
    /// </summary>
    public enum ColorFormat : byte
    {
        /// <summary>
        /// YUV420フォーマット
        /// </summary>
        YUV420,
        /// <summary>
        /// BGRAフォーマット
        /// </summary>
        BGRA,
        /// <summary>
        /// BGRフォーマット (OpenCV用)
        /// </summary>
        BGR,
        /// <summary>
        /// RGBフォーマット
        /// </summary>
        RGB,
        /// <summary>
        /// RGBAフォーマット
        /// </summary>
        RGBA,
    }
    /// <summary>
    /// *TODO+ B
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        /// <summary>
        /// *TODO+ B
        /// Color
        /// </summary>
        Color = 0,
    }
    /// <summary>
    /// レンス方向
    /// </summary>
    public enum LensFacing
    {
        /// <summary>
        /// フロント
        /// </summary>
        Front = 0,
        /// <summary>
        /// リア
        /// </summary>
        Back = 1,
        /// <summary>
        /// 外部
        /// </summary>
        External = 2
    }

    /// <summary>
    /// TODO+ C
    /// </summary>
    public interface IColorMetadataProperty : IBaseProperty { }

    /// <summary>
    /// 解像度の取得/設定
    /// </summary>
    [MessagePackObject]
    public class ResolutionProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorResolutionKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        virtual public string Key { get; } = ConstKey;

        /// <summary>
        /// 横解像度
        /// </summary>
        [Key("width")]
        public int width { get; set; }

        /// <summary>
        /// 縦解像度
        /// </summary>
        [Key("height")]
        public int height { get; set; }

        /// <summary>
        /// カメラID
        /// </summary>
        [Key("cameraId")]
        public string cameraId { get; set; } = "";

        /// <summary>
        /// カメラ配置面 (0:フロント 1:バック 2:外部)
        /// </summary>
        [Key("lensFacing")]
        public int lensFacing { get; set; }

        /// <summary>
        /// フレームレート
        /// </summary>
        [Key("frameRate")]
        public float frameRate { get; set; }

        /// <summary>
        /// 文字列へ変換する
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return string.Format("{0} : {1}x{2} {3} ({4}FPS)", Key, width, height, (LensFacing)lensFacing, frameRate);
        }
    }

    /// <summary>
    /// 初期解像度の取得/設定
    /// </summary>
    [MessagePackObject]
    public class DefaultResolutionProperty : ResolutionProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public new static readonly string ConstKey = "kTofArColorDefaultResolutionKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        override public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// TODO+ C 内部処理用
    /// </summary>
    public struct YUVLayout
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public int yStride;
        /// <summary>
        /// TODO+ C
        /// </summary>
        public int uvStride;
        /// <summary>
        /// TODO+ C
        /// </summary>
        public int uByteOffset;
        /// <summary>
        /// TODO+ C
        /// </summary>
        public int vByteOffset;
    }

    /// <summary>
    /// 解像度リストの取得
    /// </summary>
    [MessagePackObject]
    public class AvailableResolutionsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorAvailableResolutionsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 解像度リスト
        /// </summary>
        [Key("resolutions")]
        public ResolutionProperty[] resolutions { get; set; } = { };
    }

    /// <summary>
    /// Colorデータフォーマット変換の設定
    /// </summary>
    /// <remarks>
    /// <para>設定はストリーミングを開始する前に行う</para>
    /// <para>ストリーミング中にSetPropertyを行うと例外が発生する</para>
    /// </remarks>
    [MessagePackObject]
    public class FormatConvertProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorFormatConvertKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// Colorデータフォーマット
        /// <para>デフォルト値:YUV420</para>
        /// </summary>
        [Key("format")]
        public ColorFormat format { get; set; }

        /// <summary>
        /// *TODO+ B
        /// true: Colorデータフォーマット変換を行う
        /// false: 変換を行わない
        /// </summary>
        [Obsolete("isEnabled is deprecated, please use format")]
        [IgnoreMember]
        private bool isEnabled
        {
            get
            {
                return this.format != ColorFormat.YUV420;
            }
            set
            {
                this.format = value ? ColorFormat.BGRA : ColorFormat.YUV420;
            }
        }
    }

    /// <summary>
    /// *TODO+ B 一般的なユーザーには不要
    /// Metadataのリストのプロパティ
    /// </summary>
    [MessagePackObject]
    public class MetadataEntriesProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorMetadataEntriesKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// *TODO+ B
        /// Metadataのリスト
        /// </summary>
        [Key("entries")]
        public CameraMetadataProperty[] entries { get; set; } = { };
    }

    /// <summary>
    /// *TODO+ B 一般的なユーザーには不要
    /// カメラのMetadata
    /// </summary>
    [MessagePackObject]
    public class CameraMetadataProperty
    {
        /// <summary>
        /// *TODO+ B
        /// Metadataのタッグ
        /// </summary>
        [Key("tag")]
        public uint tag { get; set; }

        /// <summary>
        /// *TODO+ B
        /// Metadatのタイプ
        /// </summary>
        [Key("type")]
        public byte type { get; set; }

        /// <summary>
        /// *TODO+ B
        /// Metadataの数
        /// </summary>
        [Key("count")]
        public uint count { get; set; }

        /// <summary>
        /// *TODO+ B
        /// Metadataのバッファ
        /// </summary>
        [Key("data")]
        public ulong buffer { get; set; }
    }

    /// <summary>
    /// オートフォーカスの有効性。固定フォーカスとする場合は焦点距離を取得／設定する
    /// </summary>
    [MessagePackObject]
    public class FocusModeProperty : IColorMetadataProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorFocusModeKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 焦点距離最小値
        /// </summary>
        [Key("minDistance")]
        public float minDistance { get; set; }

        /// <summary>
        /// 焦点距離最大値
        /// </summary>
        [Key("maxDistance")]
        public float maxDistance { get; set; }

        /// <summary>
        /// 固定フォーカスとする場合の焦点距離（ミリメートル単位）
        /// </summary>
        [Key("distance")]
        public float distance { get; set; }

        /// <summary>
        /// オートフォーカスの有効性
        /// <para>true: オートフォーカス有効</para>
        /// <para>false: オートフォーカス無効</para>
        /// </summary>
        [Key("autoFocus")]
        public bool autoFocus { get; set; }

    }

    /// <summary>
    /// 自動露出とフラッシュに関する設定を取得／設定する
    /// <para>※ iOSでのSingle指定は無効となります</para>
    /// </summary>
    [MessagePackObject]
    public class ExposureModeProperty : IColorMetadataProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorExposureModeKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 自動露出機能の有効性
        /// <para>true: 自動露出有効</para>
        /// <para>false: 自動露出無効</para>
        /// </summary>
        [Key("autoExposure")]
        public bool autoExposure { get; set; }

        /// <summary>
        /// フラッシュライトの有無
        /// <para>true: フラッシュライト有り</para>
        /// <para>false: フラッシュライト無し</para>
        /// </summary>
        [Key("flashAvailable")]
        public bool flashAvailable { get; set; }

        /// <summary>
        /// <para>“Off”: フラッシュオフ</para>
        /// <para>“Single”: 1フレームのみフラッシュを使用する</para>
        /// <para>“Torch”: フラッシュを連続点灯する</para>
        /// </summary>
        [Key("flash")]
        public FlashMode flash { get; set; }

        /// <summary>
        /// 露光時間最小値
        /// </summary>
        [Key("minExposureTime")]
        public long minExposureTime { get; set; }

        /// <summary>
        /// 露光時間最大値
        /// </summary>
        [Key("maxExposureTime")]
        public long maxExposureTime { get; set; }

        /// <summary>
        /// 露光時間
        /// </summary>
        [Key("exposureTime")]
        public long exposureTime { get; set; }

        /// <summary>
        /// 現在の解像度の最小フレーム間隔
        /// </summary>
        [Key("minFrameDurationForCurrentResolution")]
        public long minFrameDurationForCurrentResolution { get; set; }

        /// <summary>
        /// 最大フレーム間隔
        /// </summary>
        [Key("maxFrameDuration")]
        public long maxFrameDuration { get; set; }

        /// <summary>
        /// フレーム間隔
        /// </summary>
        [Key("frameDuration")]
        public long frameDurarion { get; set; }

        /// <summary>
        /// 分光感度最小値
        /// </summary>
        [Key("minSensitivity")]
        public int minSensitivity { get; set; }

        /// <summary>
        /// 分光感度最大値
        /// </summary>
        [Key("maxSensitivity")]
        public int maxSensitivity { get; set; }

        /// <summary>
        /// 分光感度	
        /// </summary>
        [Key("sensitivity")]
        public int sensitibity { get; set; }

    }

    /// <summary>
    /// フラッシュモード
    /// </summary>
    public enum FlashMode : byte
    {
        /// <summary>
        /// フラッシュオフ
        /// </summary>
        Off = 0,
        /// <summary>
        /// 1フレームのみフラッシュを使用する
        /// </summary>
        Single,
        /// <summary>
        /// フラッシュを連続点灯する
        /// </summary>
        Torch,

    }

    /// <summary>
    /// 自動ホワイトバランスに関する設定を取得／設定する
    /// <para>※ iOSでは使用できません</para>
    /// </summary>
    [MessagePackObject]
    public class WhiteBalanceModeProperty : IColorMetadataProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorColorWhiteBalanceModeKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// ホワイトバランスモード
        /// </summary>
        [Key("mode")]
        public WhiteBalanceMode mode { get; set; }

        /// <summary>
        /// ホワイトバランスモードの設定可否
        /// <para>bool配列の要素0はIncandescent、要素1はFluorescentの設定可否をtrue/falseで示す</para>
        /// </summary>
        [Key("isAvailable")]
        public bool[] isAvailable { get; set; } = new bool[8];

        /// <summary>
        /// 自動ホワイトバランス機能の有効性
        /// <para>true: 自動ホワイトバランス有効</para>
        /// <para>false: 自動ホワイトバランス無効</para>
        /// </summary>
        [Key("autoWhiteBalance")]
        public bool autoWhiteBalance { get; set; }
    }

    /// <summary>
    /// ホワイトバランスモード
    /// </summary>
    public enum WhiteBalanceMode : byte
    {
        /// <summary>
        /// OFF
        /// </summary>
        Off = 0,
        /// <summary>
        /// 白熱灯
        /// </summary>
        //these get +1 to match the android value in the tofAr code
        Incandescent = 1,
        /// <summary>
        /// 蛍光灯
        /// </summary>
        Fluorescent = 2,
        /// <summary>
        /// 暖かい蛍光灯
        /// </summary>
        WarmFluorescent = 3,
        /// <summary>
        /// 昼光
        /// </summary>
        Daylight = 4,
        /// <summary>
        /// 曇りの昼光
        /// </summary>
        CloudyDaylight = 5,
        /// <summary>
        /// 夕暮れ
        /// </summary>
        Twilight = 6,
        /// <summary>
        /// 陰影光
        /// </summary>
        Shade = 7
    }

    /// <summary>
    /// フレームレート設定
    /// <para>本プロパティへ設定された値は次回ストリーミング開始時に適用される。</para>
    /// </summary>
    [MessagePackObject]
    public class FrameRateProperty : IColorMetadataProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorFrameRateKey";
        /// <summary>
        /// FPS値
        /// </summary>
        [Key("desiredFrameRate")]
        public float desiredFrameRate;
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// フレームレート設定可能範囲取得
    /// <para>本プロパティはGetPropertyのみ可能である</para>
    /// <para>SetPropertyを行うと例外が発生する</para>
    /// </summary>
    [MessagePackObject]
    public class FrameRateRangeProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorFrameRateRangeKey";
        /// <summary>
        /// 設定可能な最小FPS
        /// </summary>
        [Key("minimumFrameRate")]
        public float minimumFrameRate;
        /// <summary>
        /// 設定可能な最大FPS
        /// </summary>
        [Key("maximumFrameRate")]
        public float maximumFrameRate;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// 手ブレ補正の有効状態の取得または設定を行う。設定が実際に反映されるかどうかは機種に依存する。
    /// <para>※ iOSでは使用できません</para>
    /// </summary>
    [MessagePackObject]
    public class StabilizationProperty : IColorMetadataProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorStabilizationKey";

        /// <summary>
        /// <para>true: 手ブレ補正を有効とする</para>
        /// <para>false: 手ブレ補正を無効とする</para>
        /// </summary>
        [Key("lensStabilization")]
        public bool lensStabilization;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// *TODO+ B 内部処理用
    /// 共有ストリームの設定
    /// </summary>
    [MessagePackObject]
    public class SharedStreamsProperty : IBaseProperty
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("isRestart")]
        public bool isRestart;
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = "kTofArColorSharedStreamsKey";
    }

    /// <summary>
    /// フレームの送出を指定フレーム数遅延させる
    /// </summary>
    [MessagePackObject]
    public class DelayProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorDelayKey";

        /// <summary>
        /// 遅延フレーム数
        /// </summary>
        [Key("numFramesDelay")]
        public int numFramesDelay;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }


    //external function interface

    /// <summary>
    /// *TODO+ B 内部処理用
    /// 外部Functionのプロパティ
    /// </summary>
    [MessagePackObject]
    public class FunctionStreamParametersProperty : IBaseProperty
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("functionPointer")]
        public long functionPointer = 0;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("useExternalFunctionStream")]
        public bool useExternalFunctionStream;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("useExternalFunctionCallback")]
        public bool useExternalFunctionCallback;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("width")]
        public int width;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("height")]
        public int height;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("uvPixelStride")]
        public int uvPixelStride;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("lensfacing")]
        public int lensfacing;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorFunctionStreamParametersKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// *TODO+ B 内部処理用
    /// 外部Functionのデリゲート
    /// </summary>
    /// <param name="timestamp">TODO+ C</param>
    /// <param name="yArray">TODO+ C</param>
    /// <param name="uArray">TODO+ C</param>
    /// <param name="vArray">TODO+ C</param>
    /// <returns>TODO+ C</returns>
    //delegate type for the function pointer
    [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
    public delegate bool ColorFunctionStreamDelegate(ref System.Int64 timestamp, System.IntPtr yArray, System.IntPtr uArray, System.IntPtr vArray);

    /// <summary>
    /// *TODO+ B 内部処理用
    /// 外部Functionのコールバックプロパティ
    /// </summary>
    //setting this property calls the callback.
    [MessagePackObject]
    public class FunctionStreamCallbackProperty : IBaseProperty
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        //these parameters are not actually used in the current implementation
        [Key("timestamp")]
        public long timestamp;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("Y")]
        public long Y;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("U")]
        public long U;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("V")]
        public long V;
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorFunctionStreamCallbackKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// *TODO+ B 内部処理用
    /// 画面UVのプロパティ
    /// </summary>
    [MessagePackObject]
    public class ScreenUVsProperty : IBaseProperty
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("topLeft_x")]
        public float topLeft_x;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("topLeft_y")]
        public float topLeft_y;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("topRight_x")]
        public float topRight_x;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("topRight_y")]
        public float topRight_y;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("bottomLeft_x")]
        public float bottomLeft_x;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("bottomLeft_y")]
        public float bottomLeft_y;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("bottomRight_x")]
        public float bottomRight_x;
        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("bottomRight_y")]
        public float bottomRight_y;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArColorScreenUVsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }
}

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
using MessagePack;
using SensCord;

namespace TofAr.V0.Tof
{
    /// <summary>
    /// TODO+ C
    /// </summary>
    public interface ITofMetadataProperty : IBaseProperty { }

    /// <summary>
    /// *TODO+ B
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        Depth = 0,
        /// <summary>
        /// TODO+ C
        /// </summary>
        Confidence = 1,
        /// <summary>
        /// TODO+ C
        /// </summary>
        PointCloud = 2,
    }

    /// <summary>
    /// ToFカメラとColorカメラの同期設定
    /// </summary>
    public enum CameraSynchronization
    {
        /// <summary>
        /// Masterモード
        /// <para>ToFカメラとColorカメラの同期は行われない</para>
        /// </summary>
        Master,
        /// <summary>
        /// Slaveモード
        /// <para>ToFカメラとColorカメラの同期が行われる</para>
        /// </summary>
        Slave,
    }

    /// <summary>
    /// レンズ方向
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
    /// 内部パラメータ
    /// </summary>
    [MessagePackObject]
    public struct InternalParameter
    {
        /// <summary>
        /// 内部パラメータ
        /// </summary>
        [Key("fx")]
        public float fx;

        /// <summary>
        /// 内部パラメータ
        /// </summary>
        [Key("fy")]
        public float fy;

        /// <summary>
        /// 内部パラメータ
        /// </summary>
        [Key("cx")]
        public float cx;

        /// <summary>
        /// 内部パラメータ
        /// </summary>
        [Key("cy")]
        public float cy;
    }

    /// <summary>
    /// 3x3行列
    /// </summary>
    [MessagePackObject]
    public struct Matrix
    {
        /// <summary>
        /// 行列の(0,0)成分
        /// </summary>
        [Key("a")]
        public float a;

        /// <summary>
        /// 行列の(0,1)成分
        /// </summary>
        [Key("b")]
        public float b;

        /// <summary>
        /// 行列の(0,2)成分
        /// </summary>
        [Key("c")]
        public float c;

        /// <summary>
        /// 行列の(1,0)成分
        /// </summary>
        [Key("d")]
        public float d;

        /// <summary>
        /// 行列の(1,1)成分
        /// </summary>
        [Key("e")]
        public float e;

        /// <summary>
        /// 行列の(1,2)成分
        /// </summary>
        [Key("f")]
        public float f;

        /// <summary>
        /// 行列の(2,0)成分
        /// </summary>
        [Key("g")]
        public float g;

        /// <summary>
        /// 行列の(2,1)成分
        /// </summary>
        [Key("h")]
        public float h;

        /// <summary>
        /// 行列の(2,2)成分
        /// </summary>
        [Key("i")]
        public float i;

        /// <summary>
        /// 行列を配列に変換する
        /// </summary>
        /// <param name="M">行列</param>
        /// <returns>配列</returns>
        public static float[] MatrixToFloatArray(Matrix M)
        {
            return new float[] { M.a, M.b, M.c, M.d, M.e, M.f, M.g, M.h, M.i };
        }
    }

    /// <summary>
    /// シリアライズ可能なVector
    /// </summary>
    [MessagePackObject]
    public struct Vector
    {
        /// <summary>
        /// x値
        /// </summary>
        [Key("x")]
        public float x;

        /// <summary>
        /// y値
        /// </summary>
        [Key("y")]
        public float y;

        /// <summary>
        /// z値
        /// </summary>
        [Key("z")]
        public float z;
    }

    /// <summary>
    /// カメラキャリブレーション情報
    /// </summary>
    [MessagePackObject]
    public class CalibrationSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofCalibrationSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// Color映像の横ピクセル数
        /// </summary>
        [Key("colorWidth")]
        public int colorWidth;

        /// <summary>
        /// Color映像の縦ピクセル数
        /// </summary>
        [Key("colorHeight")]
        public int colorHeight;

        /// <summary>
        /// Depth映像の横ピクセル数
        /// </summary>
        [Key("depthWidth")]
        public int depthWidth;

        /// <summary>
        /// Depth映像の縦ピクセル数
        /// </summary>
        [Key("depthHeight")]
        public int depthHeight;

        /// <summary>
        /// <para>true: キャリブがAutoCalibから取得された</para>
        /// <para>false: Resourcesから取得された、又はキャリブデータを見つけられなかった</para>
        /// </summary>
        [Key("isCalibrated")]
        public bool isCalibrated;

        /// <summary>
        /// Colorカメラの内部パラメータ
        /// <para>キャリブレーションツールから取得可能</para>
        /// </summary>
        [Key("c")]
        public InternalParameter c;

        /// <summary>
        /// Depthカメラの内部パラメータ
        /// </summary>
        [Key("d")]
        public InternalParameter d;

        /// <summary>
        /// 2つのカメラ間の回転値
        /// <para>キャリブレーションツールから取得可能</para>
        /// </summary>
        [Key("R")]
        public Matrix R;

        /// <summary>
        /// 2つのカメラ間の距離
        /// <para>キャリブレーションツールから取得可能</para>
        /// </summary>
        [Key("T")]
        public Vector T;

        /// <summary>
        /// 文字列変換
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},\n{4},{5},{6},{7},\n{8},{9},{10},{11},{12},{13},{14},{15},{16},\n{17},{18},{19},\n{20},{21}",
                c.fx, c.fy, c.cx, c.cy, d.fx, d.fy, d.cx, d.cy, R.a, R.b, R.c, R.d, R.e, R.f, R.g, R.h, R.i, T.x, T.y, T.z, colorWidth, colorHeight);
        }
    }

    /// <summary>
    /// カメラ設定プロパティ
    /// </summary>
    [MessagePackObject]
    public class CameraConfigurationProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        //same interface for both camera types, so a uniting supertype so we can use polymorphism in the menus
        public static readonly string ConstKey = "kTofArTofCamera2CurrentConfigurationKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        virtual public string Key { get; } = ConstKey;

        /// <summary>
        /// キャリブレーション済みフラグ(Camera2モードでは値はセットされない)
        /// </summary>
        [Key("isCalibrated")]
        public bool isCalibrated;

        /// <summary>
        /// 設定ID
        /// </summary>
        [Key("uid")]
        public int uid;

        /// <summary>
        /// 横解像度
        /// </summary>
        [Key("width")]
        public int width;

        /// <summary>
        /// 縦解像度
        /// </summary>
        [Key("height")]
        public int height;

        /// <summary>
        /// カメラID
        /// </summary>
        [Key("cameraId")]
        public string cameraId = string.Empty;

        /// <summary>
        /// ColorカメラID
        /// </summary>
        [Key("colorCameraId")]
        public string colorCameraId = string.Empty;

        /// <summary>
        /// フレームレート
        /// </summary>
        [Key("frameRate")]
        public float frameRate;

        /// <summary>
        /// カメラ内部パラメータ(Camera2モードでは値はセットされない)
        /// </summary>
        [Key("unambiguousRange")]
        public float unambiguousRange;

        /// <summary>
        /// カメラ配置面 (0:フロント 1:バック 2:外部)
        /// </summary>
        [Key("lensFacing")]
        public int lensFacing;

        /// <summary>
        /// カメラ内部パラメータ
        /// </summary>
        [Key("intrinsics")]
        public Camera2IntrinsicsProperty intrinsics = new Camera2IntrinsicsProperty();

        /// <summary>
        /// 設定名称
        /// </summary>
        [Key("name")]
        public string name = string.Empty;

        /// <summary>
        /// true: private depthフォーマットである
        /// </summary>
        [Key("isDepthPrivate")]
        public bool isDepthPrivate;

        /// <summary>
        /// Fusion機能の有効/無効
        /// </summary>
        [Key("isFusion")]
        public bool isFusion;

        /// <summary>
        /// Fusionデータの横幅
        /// </summary>
        [Key("fusionSourceWidth")]
        public int fusionSourceWidth;

        /// <summary>
        /// Fusionデータの縦幅
        /// </summary>
        [Key("fusionSourceHeight")]
        public int fusionSourceHeight;
    }

    /// <summary>
    /// カメラ設定リストプロパティ
    /// </summary>
    [MessagePackObject]
    public class CameraConfigurationsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofCamera2ConfigurationsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// カメラ設定リスト
        /// </summary>
        [Key("configurations")]
        public CameraConfigurationProperty[] configurations { get; set; } = { };
    }

    /// <summary>
    /// ID指定による設定の適用
    /// </summary>
    [MessagePackObject]
    public class SetConfigurationIdProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofCamera2SetConfigurationIdKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 設定ID
        /// </summary>
        [Key("uid")]
        public int uid;
    }

    /// <summary>
    /// 処理対象チャンネルの指定
    /// <para>アプリケーションで必要としない処理を処理対象外とすることで処理負荷を軽減することができる。</para>
    /// </summary>
    [MessagePackObject]
    public class ProcessTargetsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofProcessTargetsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// <para>true: Depthを処理対象とする</para>
        /// <para>false: Depthを処理対象としない</para>
        /// <para>デフォルト値:true</para>
        /// </summary>
        [Obsolete("isProcessDepth is deprecated, please use processDepth instead")]
        [IgnoreMember]
        public bool isProcessDepth { get => processDepth; set => processDepth = value; }
        /// <summary>
        /// <para>true: Depthを処理対象とする</para>
        /// <para>false: Depthを処理対象としない</para>
        /// <para>デフォルト値:true</para>
        /// </summary>
        [Key("processDepth")]
        public bool processDepth;

        /// <summary>
        /// <para>true: Confidenceを処理対象とする</para>
        /// <para>false: Confidenceを処理対象としない</para>
        /// <para>デフォルト値:true</para>
        /// </summary>
        [Obsolete("isProcessConfidence is deprecated, please use processConfidence instead")]
        [IgnoreMember]
        public bool isProcessConfidence { get => processConfidence; set => processConfidence = value; }
        /// <summary>
        /// <para>true: Confidenceを処理対象とする</para>
        /// <para>false: Confidenceを処理対象としない</para>
        /// <para>デフォルト値:true</para>
        /// </summary>
        [Key("processConfidence")]
        public bool processConfidence;

        /// <summary>
        /// <para>true: PointCloudを処理対象とする</para>
        /// <para>false: PointCloudを処理対象としない</para>
        /// <para>デフォルト値:true</para>
        /// </summary>
        [Obsolete("isProcessPointCloud is deprecated, please use processPointCloud instead")]
        [IgnoreMember]
        public bool isProcessPointCloud { get => processConfidence; set => processConfidence = value; }
        /// <summary>
        /// <para>true: PointCloudを処理対象とする</para>
        /// <para>false: PointCloudを処理対象としない</para>
        /// <para>デフォルト値:true</para>
        /// </summary>
        [Key("processPointCloud")]
        public bool processPointCloud;
    }


    /// <summary>
    /// Confidence処理に対する設定/設定状態取得
    /// <para>※ iOSでは使用できません</para>
    /// </summary>
    [MessagePackObject]
    public class DepthConfidenceProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofDepthConfidenceKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// <para>true: DEPTH16フォーマットのConfidenceに変換処理を行う</para>
        /// <para>false: DEPTH16フォーマットのConfidenceに変換処理を行わない</para>
        /// </summary>
        [Key("depth16ConfidenceConvert")]
        public bool depth16ConfidenceConvert;
    }

    /// <summary>
    /// ポイントクラウド属性情報の取得
    /// </summary>
    [MessagePackObject]
    public class PointCloudProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofPointCloudKey";

        /// <summary>
        /// 横解像度
        /// </summary>
        [Key("width")]
        public int width;

        /// <summary>
        /// 縦解像度
        /// </summary>
        [Key("height")]
        public int height;

        /// <summary>
        /// 固定文字列 "point_cloud_xyz16u"
        /// </summary>
        [Key("pixel_format")]
        public string pixel_format;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// フレームレート設定
    /// <para>本プロパティへ設定された値は次回ストリーミング開始時に適用される。</para>
    /// </summary>
    [MessagePackObject]
    public class FrameRateProperty : ITofMetadataProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofCamera2FrameRateKey";

        /// <summary>
        /// フレームレート値
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
        public static readonly string ConstKey = "kTofArTofCamera2FrameRateRangeKey";

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
    /// フレームの送出を指定フレーム数遅延させる
    /// </summary>
    [MessagePackObject]
    public class DelayProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofDelayKey";

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

    // CAMERA2

    /// <summary>
    /// Tof内部パラメータ
    /// </summary>
    [MessagePackObject]
    public class Camera2IntrinsicsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofCamera2IntrinsicParametersKey";

        /// <summary>
        /// 焦点距離 x
        /// </summary>
        [Key("fx")]
        public float fx;

        /// <summary>
        /// 焦点距離 y
        /// </summary>
        [Key("fy")]
        public float fy;

        /// <summary>
        /// 中心 x
        /// </summary>
        [Key("cx")]
        public float cx;

        /// <summary>
        /// 中心 y
        /// </summary>
        [Key("cy")]
        public float cy;

        /// <summary>
        /// 歪みパラメータ k1
        /// </summary>
        [Key("k1")]
        public float k1;

        /// <summary>
        /// 歪みパラメータ k2
        /// </summary>
        [Key("k2")]
        public float k2;

        /// <summary>
        /// 歪みパラメータ k3
        /// </summary>
        [Key("k3")]
        public float k3;

        /// <summary>
        /// 歪みパラメータ p1
        /// </summary>
        [Key("p1")]
        public float p1;

        /// <summary>
        /// 歪みパラメータ p2
        /// </summary>
        [Key("p2")]
        public float p2;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// カメラ設定プロパティ
    /// </summary>
    [MessagePackObject]
    public class Camera2ConfigurationProperty : CameraConfigurationProperty
    {
    }

    /// <summary>
    /// カメラ設定リストプロパティ
    /// </summary>
    [MessagePackObject]
    public class Camera2ConfigurationsProperty : CameraConfigurationsProperty
    {
    }

    /// <summary>
    /// ID指定による設定の適用
    /// </summary>
    [MessagePackObject]
    public class Camera2SetConfigurationIdProperty : SetConfigurationIdProperty
    {
    }

    /// <summary>
    /// デフォルトのカメラ設定プロパティ
    /// </summary>
    [MessagePackObject]
    public class Camera2DefaultConfigurationProperty : CameraConfigurationProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public new static readonly string ConstKey = "kTofArTofCamera2DefaultConfigurationKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        override public string Key { get; } = ConstKey;
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
        [Key("isDepthSixteen")]
        public bool isDepthSixteen;

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
        [Key("fx")]
        public float fx;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("fy")]
        public float fy;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("cx")]
        public float cx;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("cy")]
        public float cy;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofFunctionStreamParametersKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    //delegate type for the function pointer
    [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]

    /// <summary>
    /// TODO+  B 内部処理用
    /// </summary>
    public delegate bool TofFunctionStreamDelegate(ref System.Int64 timestamp, System.IntPtr shortArray);

    //setting this property calls the callback.
    /// <summary>
    /// TODO+  B 内部処理用
    /// </summary>
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
        [Key("dataArray")]
        public long dataArray;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofFunctionStreamCallbackKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;
    }

    /// <summary>
    /// TODO+  B 内部処理用
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
        public string Key { get; } = "kTofArTofCamera2SharedStreamsKey";
    }

    /// <summary>
    /// 自動露出に関する設定を取得／設定する
    /// </summary>
    [MessagePackObject]
    public class ExposureProperty : ITofMetadataProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArTofCamera2ExposureKey";

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


    }
}

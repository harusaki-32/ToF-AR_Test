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

namespace TofAr.V0.Segmentation
{
    /// <summary>
    /// *TODO+ B
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        Segmentation = 0,
    }

    /// <summary>
    /// データ構造タイプ
    /// </summary>
    public enum DataStructureType : byte
    {
        /// <summary>
        /// byte配列のマスクバッファ
        /// </summary>
        MaskBufferByte = 0,
        /// <summary>
        /// 16bit符号無し整数配列のマスクバッファ
        /// </summary>
        MaskBufferUInt16,
        /// <summary>
        /// 32bit符号無し整数配列のマスクバッファ
        /// </summary>
        MaskBufferUInt32,
        /// <summary>
        /// 64bit符号無し整数配列のマスクバッファ
        /// </summary>
        MaskBufferUInt64,
        /// <summary>
        /// float型配列のマスクバッファ
        /// </summary>
        MaskBufferFloat,
        /// <summary>
        /// ポインタ
        /// </summary>
        RawPointer,
    };

    /// <summary>
    /// Segmentation推定結果
    /// </summary>
    [MessagePackObject]
    public class SegmentationResult
    {
        /// <summary>
        /// Segmentation名称
        /// </summary>
        [Key("name")]
        public string name = string.Empty;

        /// <summary>
        /// データ構造タイプ
        /// </summary>
        [Key("dataStructureType")]
        public DataStructureType dataStructureType = DataStructureType.MaskBufferByte;

        /// <summary>
        /// タイムスタンプ
        /// </summary>
        [Key("timestamp")]
        public UInt64 timestamp = 0;

        /// <summary>
        /// マスクバッファのサイズ
        /// </summary>
        [Key("maskBufferSize")]
        public Int64 maskBufferSize = 0;

        /// <summary>
        /// マスクバッファ幅
        /// </summary>
        [Key("maskBufferWidth")]
        public Int32 maskBufferWidth = 0;

        /// <summary>
        /// マスクバッファ高さ
        /// </summary>
        [Key("maskBufferHeight")]
        public Int32 maskBufferHeight = 0;

        /// <summary>
        /// byte配列のマスクバッファ
        /// </summary>
        [Key("maskBufferByte")]
        public byte[] maskBufferByte = new byte[0];

        /// <summary>
        /// 16bit符号無し整数配列のマスクバッファ
        /// </summary>
        [Key("maskBufferUInt16")]
        public UInt16[] maskBufferUInt16 = new UInt16[0];

        /// <summary>
        /// 32bit符号無し整数配列のマスクバッファ
        /// </summary>
        [Key("maskBufferUInt32")]
        public UInt32[] maskBufferUInt32 = new UInt32[0];

        /// <summary>
        /// 64bit符号無し整数配列のマスクバッファ
        /// </summary>
        [Key("maskBufferUInt64")]
        public UInt64[] maskBufferUInt64 = new UInt64[0];

        /// <summary>
        /// float型配列のマスクバッファ
        /// </summary>
        [Key("maskBufferFloat")]
        public float[] maskBufferFloat = new float[0];

        /// <summary>
        /// ポインタ
        /// </summary>
        [Key("rawPointer")]
        public UInt64 rawPointer = 0;
    }

    /// <summary>
    /// Segmentation推定結果リスト
    /// </summary>
    [MessagePackObject]
    public class SegmentationResults
    {
        /// <summary>
        /// Segmentation推定結果リスト
        /// </summary>
        [Key("results")]
        public SegmentationResult[] results;
    };

    /// <summary>
    /// 内部処理用のProperty TODO C+
    /// </summary>
    [MessagePackObject]
    public class ResultHandlerProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArSegmentationResultHandlerKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("handler")]
        public Int64 handler = 0;
    }

    /// <summary>
    /// SkyDetector設定 Property
    /// </summary>
    [MessagePackObject]
    public class SkyDetectorSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArSegmentationSkyDetectorSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// <para>true: 有効にする</para>
        /// <para>false: 無効にする</para>
        /// </summary>
        [Key("enabled")]
        public bool enabled = false;

        /// <summary>
        /// CopyToInputハンドラへのポインタ値
        /// </summary>
        [Key("copyToInputHandler")]
        public UInt64 copyToInputHandler = 0;

        /// <summary>
        /// Executeハンドラへのポインタ値
        /// </summary>
        [Key("executeHandler")]
        public UInt64 executeHandler = 0;
    }


    /// <summary>
    /// HumanDetector設定 Property
    /// </summary>
    [MessagePackObject]
    public class HumanDetectorSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArSegmentationHumanDetectorSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// <para>true: 有効にする</para>
        /// <para>false: 無効にする</para>
        /// </summary>
        [Key("enabled")]
        public bool enabled = false;

        /// <summary>
        /// CopyToInputハンドラへのポインタ値
        /// </summary>
        [Key("copyToInputHandler")]
        public UInt64 copyToInputHandler = 0;

        /// <summary>
        /// Executeハンドラへのポインタ値
        /// </summary>
        [Key("executeHandler")]
        public UInt64 executeHandler = 0;
    }

    /// <summary>
    /// マスクの情報
    /// </summary>
    [MessagePackObject]
    public class MaskInfo
    {
        /// <summary>
        /// 適用するMask名
        /// </summary>
        [Key("name")]
        public string name;
        /// <summary>
        /// 適用するMaskはColorかTofに合わせるもののフラグ
        /// </summary>
        [Key("isColorSource")]
        public bool isColorSource;
        /// <summary>
        /// Maskの反転
        /// </summary>
        [Key("isInvertMask")]
        public bool isInvertMask;
    }
}

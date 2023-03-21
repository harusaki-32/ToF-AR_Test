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
using TofAr.V0.Tof;
using MessagePack;
using SensCord;

namespace TofAr.V0.Mesh
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
        Mesh = 0,
    }

    /// <summary>
    /// Mesh生成アルゴリズム設定情報
    /// </summary>
    [MessagePackObject]
    public class AlgorithmConfigProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArMeshAlgorithmConfigKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// Meshリダクションレベル
        /// <para>0:リダクションなし</para>
        /// <para>1～:リダクション実施</para>
        /// <para>デフォルト=0</para>
        /// </summary>
        [Key("meshReductionLevel")]
        public int meshReductionLevel;

        /// <summary>
        /// トライアングル計算の間隔フレーム数
        /// <para>デフォルト=4</para>
        /// </summary>
        [Key("resetTrianglePeriod")]
        public int resetTrianglePeriod;
    }

    /// <summary>
    /// Mask設定 Property
    /// </summary>
    [MessagePackObject]
    public class MaskSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArMeshMaskSettingsKey";

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
        /// 適用するMask名リスト
        /// </summary>
        [Key("maskInfo")]
        public TofAr.V0.Segmentation.MaskInfo[] maskInfo;

        /// <summary>
        /// マスクをオフとする閾値
        /// </summary>
        [Key("maskThreshold")]
        public byte maskThreshold = 55;
    }
}

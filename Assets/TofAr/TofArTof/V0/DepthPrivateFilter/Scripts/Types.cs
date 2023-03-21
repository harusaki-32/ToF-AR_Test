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

namespace TofAr.V0.Tof.DepthPrivateFilter
{
    /// <summary>
    /// 反転モード
    /// </summary>
    public enum TransformationMode : byte
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Default = 0,
        /// <summary>
        /// 横反転
        /// </summary>
        MirrorH = 1,
        /// <summary>
        /// 縦反転
        /// </summary>
        MirrorV = 2,
        /// <summary>
        /// 反転
        /// </summary>
        Flip = 3
    }

    /// <summary>
    /// フィルター基本設定
    /// </summary>
    [MessagePackObject]
    public class DepthPrivateFilterSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// ソフトウェアID
        /// </summary>
        [Key("softwareId")]
        public int softwareId;

        /// <summary>
        /// フィルターの有効/無効
        /// </summary>
        [Key("filterEnabled")]
        public bool filterEnabled = true;

        /// <summary>
        /// キャリブレーションデータのパス
        /// </summary>
        [Key("calibrationDataPath")]
        public string calibrationDataPath = string.Empty;

        /// <summary>
        /// 反転モード
        /// </summary>
        [Key("transformationMode")]
        public TransformationMode transformationMode = TransformationMode.Default;

        /// <summary>
        /// フレームレート
        /// </summary>
        [Key("frameRate")]
        public Int64 frameRate = 0;

        /// <summary>
        /// エクスポージャー
        /// </summary>
        [Key("exposure")]
        public Int64 exposure = 0;

        /// <summary>
        /// HCG
        /// </summary>
        [Key("hcg")]
        public bool hcg = false;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = "kTofArTofCamera2DepthPrivateFilterSettingsKey";
    }
}

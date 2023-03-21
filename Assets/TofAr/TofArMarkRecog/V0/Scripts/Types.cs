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
using UnityEngine;

namespace TofAr.V0.MarkRecog
{
    /// <summary>
    /// チャンネルID
    /// </summary>
    public enum ChannelIds
    {
        /// <summary>
        /// 丸
        /// </summary>
        Circle = 0,

        /// <summary>
        /// 三角
        /// </summary>
        Triangle = 1,

        /// <summary>
        /// 上下逆の三角
        /// </summary>
        ReverseTriangle = 2,

        /// <summary>
        /// 八の字
        /// </summary>
        Eight = 3,

        /// <summary>
        /// 星
        /// </summary>
        Star = 4,

        /// <summary>
        /// ハート
        /// </summary>
        Heart = 5,

        /// <summary>
        /// マークが認識されていない
        /// </summary>
        None = 999,
    }

    /// <summary>
    /// マーク認識結果
    /// </summary>
    [MessagePackObject]
    public class ResultProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArMarkRecogResultKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 入力画像。白黒のARGB32フォーマットとする。
        /// </summary>
        [Key("image")]
        public Texture2D image;

        /// <summary>
        /// マークの認識レベル
        /// </summary>
        [Key("levels")]
        public float[] levels;
    }
}

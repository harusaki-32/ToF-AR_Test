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
using SensCord;
using System;

namespace TofAr.V0.Color
{
    /// <summary>
    /// Colorデータクラス
    /// </summary>
    public class ColorData : ChannelData
    {
        /// <summary>
        /// Colorデータ配列
        /// </summary>
        public byte[] Data { get; internal set; }

        internal ColorData(RawData rawData) : base(rawData)
        {
            this.Data = rawData.ToArray();
        }
    }
}

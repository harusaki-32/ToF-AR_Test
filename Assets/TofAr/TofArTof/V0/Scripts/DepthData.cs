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

namespace TofAr.V0.Tof
{
    /// <summary>
    /// Depthデータクラス
    /// </summary>
    public class DepthData : ChannelData
    {
        /// <summary>
        /// Depthデータ配列
        /// </summary>
        public short[] Data { get; internal set; }

        internal DepthData(RawData rawData) : base(rawData)
        {
            var srcBuffer = rawData.ToArray();
            this.Data = new short[rawData.Length / sizeof(short)];
            Buffer.BlockCopy(srcBuffer, 0, this.Data, 0, this.Data.Length * sizeof(short));
        }
    }
}

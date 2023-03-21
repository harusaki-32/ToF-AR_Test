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

namespace TofAr.V0
{
    /// <summary>
    /// TofArストリームのチャネルデータ
    /// </summary>
    public abstract class ChannelData
    {
        /// <summary>
        /// ヘッダーサイズ
        /// </summary>
        protected const int RawDataHeaderSize = 16;

        /// <summary>
        /// データ種類
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// タイムスタンプ
        /// </summary>
        public decimal Timestamp { get; protected set; }

        /// <summary>
        /// チャネルデータ
        /// </summary>
        /// <param name="rawData">データ</param>
        protected ChannelData(RawData rawData)
        {
            this.Type = rawData.Type;
            this.Timestamp = rawData.Timestamp;
        }
    }
}

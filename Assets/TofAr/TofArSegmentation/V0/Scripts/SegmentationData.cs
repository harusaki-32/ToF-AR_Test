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

namespace TofAr.V0.Segmentation
{
    /// <summary>
    /// Segmentationデータクラス
    /// </summary>
    public class SegmentationData : ChannelData
    {
        /// <summary>
        /// Segmentation情報データ
        /// </summary>
        public SegmentationResults Data { get; private set; }

        internal SegmentationData(RawData raw) : base(raw)
        {
            var bytes = raw.ToArray();
            if (bytes != null && raw.Length > 0)
            {
                this.Data = MessagePackSerializer.Deserialize<SegmentationResults>(bytes);
            }
        }

        internal SegmentationData(SegmentationResults results) : base(new RawData())
        {
            this.Data = results;
        }
    }
}

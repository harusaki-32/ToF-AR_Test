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

namespace TofAr.V0.Face
{
    /// <summary>
    /// Faceデータ
    /// </summary>
    public class FaceData : ChannelData
    {
        /// <summary>
        /// Face認識結果
        /// </summary>
        public FaceResults Data { get; protected internal set; }

        internal FaceData(RawData raw) : base(raw)
        {
            var bytes = raw.ToArray();
            if (bytes != null && raw.Length > 0)
            {
                this.Data = MessagePackSerializer.Deserialize<FaceResults>(bytes);
            }
        }

        internal FaceData(FaceResults results) : base(new RawData())
        {
            this.Data = results;

            if (results.results.Length > 0)
            {
                this.Timestamp = results.results[0].timestamp;
            }
        }
    }

}

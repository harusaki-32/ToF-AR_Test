/*
 * Copyright 2018,2019,2020,2021,2022,2023 Sony Semiconductor Solutions Corporation.
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

namespace TofAr.V0.Slam
{
    /// <summary>
    /// TODO+ C Slamはリファレンスに載せる？
    /// </summary>
    public class SlamData : ChannelData
    {
        public CameraPoseProperty Data { get; private set; }
        public SlamStatus Status { get; private set; }

        internal SlamData(RawData raw) : base(raw)
        {
            var data = raw.ToArray();
            if (data != null && raw.Length > 0)
            {

                float[] floatData = new float[(data.Length - 1) / 4];
                Buffer.BlockCopy(data, 0, floatData, 0, data.Length - 1);
                this.Data = new CameraPoseProperty();
                this.Data.position = new TofArVector3(floatData[0], floatData[1], floatData[2]);
                this.Data.rotation = new TofArQuaternion(floatData[3], floatData[4], floatData[5], floatData[6]);
                this.Data.acceleration = new TofArVector3(floatData[7], floatData[8], floatData[9]);
                this.Status = (SlamStatus)data[data.Length - 1];
            }
        }
    }
}

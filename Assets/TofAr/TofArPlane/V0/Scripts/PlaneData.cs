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
using UnityEngine;

namespace TofAr.V0.Plane
{
    /// <summary>
    /// Planeデータクラス
    /// <para>v1.0.0以降 Dataは配列型となります。既存プログラムはData[0]を利用するよう変更が必要です</para>
    /// </summary>
    public class PlaneData : ChannelData
    {
        /// <summary>
        /// 平面情報データリスト
        /// </summary>
        public VariablesProperty[] Data { get; internal set; }

        internal PlaneData(RawData raw) : base(raw)
        {
            var data = raw.ToArray();
            if (data != null)
            {
                float[] floatarr = new float[data.Length / 4];
                int nProperties = floatarr.Length / 7;
                Buffer.BlockCopy(data, 0, floatarr, 0, data.Length);
                Data = new VariablesProperty[nProperties];

                for (int i = 0; i < nProperties; i++)
                {
                    this.Data[i] = new VariablesProperty();

                    this.Data[i].normal = new Vector3(-floatarr[i * 7 + 0], floatarr[i * 7 + 1], -floatarr[i * 7 + 2]);
                    this.Data[i].center = new Vector3(floatarr[i * 7 + 3], -floatarr[i * 7 + 4], floatarr[i * 7 + 5]) / 1000.0f;
                    this.Data[i].radius = floatarr[i * 7 + 6] / 1000.0f;
                }

            }
        }
    }
}

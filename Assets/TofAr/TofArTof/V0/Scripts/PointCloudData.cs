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

namespace TofAr.V0.Tof
{
    /// <summary>
    /// PointCloudデータクラス
    /// </summary>
    public class PointCloudData : ChannelData
    {
        private byte[] srcBuffer;
        /// <summary>
        /// PointCloudデータ配列
        /// </summary>
        public byte[] Data { get { return srcBuffer; } internal set { srcBuffer = value; } }

        private const int BytesPerPoint = 6;
        /// <summary>
        /// Point配列
        /// </summary>
        public Vector3[] Points { get; internal set; } = new Vector3[0];
        /// <summary>
        /// “point_cloud_xyz16u”固定
        /// </summary>
        public string Format { get; private set; }

        internal PointCloudData(RawData rawData) : base(rawData)
        {
            this.Format = "point_cloud_xyz16u";
            UpdateFromRawData(rawData);
        }

        //so we don't have to keep allocating arrays, we'll keep this as it is
        internal void UpdateFromRawData(RawData rawData)
        {
            Timestamp = rawData.Timestamp;
            srcBuffer = rawData.ToArray();

            if (Format == "point_cloud_xyz16u")
            {
                var pointNum = srcBuffer.Length / BytesPerPoint;
                if (Points.Length != pointNum)
                {
                    Points = new Vector3[pointNum];
                }

                for (var i = 0; i < pointNum; i++)
                {
                    var offset = BytesPerPoint * i;
                    Points[i].x = BitConverter.ToInt16(srcBuffer, offset) / 1000f;
                    Points[i].y = -BitConverter.ToInt16(srcBuffer, offset + 2) / 1000f;
                    Points[i].z = BitConverter.ToInt16(srcBuffer, offset + 4) / 1000f;
                }
            }
            else
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Unrecognised Point cloud format {Format}");
            }
        }
    }

}

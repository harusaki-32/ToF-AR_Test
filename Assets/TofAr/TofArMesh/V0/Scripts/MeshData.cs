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
using System.Runtime.InteropServices;
using UnityEngine;

namespace TofAr.V0.Mesh
{
    /// <summary>
    /// Meshデータクラス
    /// </summary>
    public class MeshData : ChannelData
    {
        private float[] verticesBuffer { get; set; } = new float[0];

        /// <summary>
        /// 頂点配列
        /// </summary>
        public Vector3[] vertices { get; internal set; } = new Vector3[0];

        /// <summary>
        /// トライアングル配列
        /// </summary>
        public int[] trianglesBuffer { get; internal set; } = new int[0];

        /// <summary>
        /// 頂点数
        /// </summary>
        public int verticesCount { get; internal set; } = 0;

        /// <summary>
        /// トライアングル数
        /// </summary>
        public int trianglesCount { get; internal set; } = 0;

        /// <summary>
        /// <para>true: トライアングル計算する</para>
        /// <para>false: 計算しない</para>
        /// </summary>
        public bool resetTriangle { get; internal set; } = false;

        private int latestTrianglesCount = 0;

        internal MeshData(RawData rawData) : base(rawData)
        {
            UpdateFromRawData(rawData);
        }

        //so we don't have to keep allocating arrays, we'll keep this as it is
        internal void UpdateFromRawData(RawData rawData)
        {
            Timestamp = rawData.Timestamp;
            var srcBuffer = rawData.ToArray();
            verticesCount = BitConverter.ToInt32(srcBuffer, 0);
            if (verticesBuffer.Length < verticesCount * 3)
            {
                verticesBuffer = new float[verticesCount * 3];
            }

            // Recording needs whole data 
            var recordProperty = TofArMeshManager.Instance.GetProperty<RecordProperty>();

            int verticesSize = 0;
            int trianglesSize = 0;
            IntPtr trianglesPtr = IntPtr.Zero;

            bool useFlatRawData = TofArManager.Instance.RuntimeSettings.runMode == RunMode.MultiNode || (TofArMeshManager.Instance.IsPlaying && !TofArMeshManager.Instance.IsStreamActive) || recordProperty.Enabled;

            if (!useFlatRawData)
            {
                var verticesPtr = (IntPtr)BitConverter.ToUInt64(srcBuffer, 4);
                trianglesCount = BitConverter.ToInt32(srcBuffer, 12);
                trianglesPtr = (IntPtr)BitConverter.ToUInt64(srcBuffer, 16);
                Marshal.Copy(verticesPtr, verticesBuffer, 0, verticesCount * 3);
                this.resetTriangle = BitConverter.ToBoolean(srcBuffer, 24);
            }
            else
            {
                verticesSize = verticesCount * 3 * sizeof(float);
                Buffer.BlockCopy(srcBuffer, 4, verticesBuffer, 0, verticesSize);
                trianglesCount = BitConverter.ToInt32(srcBuffer, 4 + verticesSize);
                trianglesSize = trianglesCount * 3 * sizeof(int);
                this.resetTriangle = BitConverter.ToBoolean(srcBuffer, 4 + verticesSize + 4 + trianglesSize);
            }

            if (trianglesCount != latestTrianglesCount || this.resetTriangle)
            {
                if (trianglesBuffer.Length != trianglesCount * 3)
                {
                    trianglesBuffer = new int[trianglesCount * 3];
                }

                if (!useFlatRawData)
                {
                    Marshal.Copy(trianglesPtr, trianglesBuffer, 0, trianglesCount * 3);
                }
                else
                {
                    Buffer.BlockCopy(srcBuffer, 4 + verticesSize + 4, trianglesBuffer, 0, trianglesSize);
                }
            }
            //populate the vertices
            if (vertices.Length != verticesCount)
            {
                vertices = new Vector3[verticesCount];
            }
            for (var i = 0; i < verticesCount; i++)
            {
                vertices[i].x = verticesBuffer[i * 3];
                vertices[i].y = -verticesBuffer[i * 3 + 1];
                vertices[i].z = verticesBuffer[i * 3 + 2];
            }

            latestTrianglesCount = trianglesCount;
        }
    }
}

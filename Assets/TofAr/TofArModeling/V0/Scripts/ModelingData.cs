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

namespace TofAr.V0.Modeling
{
    /// <summary>
    /// Modelingデータクラス
    /// </summary>
    public class ModelingData : ChannelData
    {
        /// <summary>
        /// Modeling結果データ
        /// </summary>
        public ModelingOutput[] Data { get; private set; }

        internal ModelingData(RawData rawData) : base(rawData)
        {
            var rawBuffer = rawData.ToArray();
            // TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArModeling][ModelingOutput] rawBuffer.Length={rawBuffer.Length}");

            if (rawBuffer.Length > 0)
            {
                var headerBytes = Marshal.SizeOf(typeof(UInt32));
                var elementBytes = Marshal.SizeOf(typeof(RawModelingOutput));
                var elements = Marshal.PtrToStructure<UInt32>(Marshal.UnsafeAddrOfPinnedArrayElement(rawBuffer, 0));

                this.Data = new ModelingOutput[elements];

                var dataIndex = headerBytes + (elementBytes * elements);

                // Recording needs whole data 
                var manager = TofArModelingManager.Instance;
                var recordProperty = manager.GetProperty<RecordProperty>();

                bool useFlatRawData = TofArManager.Instance.RuntimeSettings.runMode == RunMode.MultiNode || (manager.IsPlaying || recordProperty.Enabled);

                //TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArModeling][ModelingOutput] headerBytes={headerBytes} elementBytes={elementBytes} elements={elements} dataIndex={dataIndex}");
                for (var i = 0; i < elements; i++)
                {
                    var output = Marshal.PtrToStructure<RawModelingOutput>(Marshal.UnsafeAddrOfPinnedArrayElement(rawBuffer, headerBytes + (elementBytes * i)));

                    var vertices = new float[output.numVertices * 3];
                    var triangles = new Int32[output.numTriangles * 3];
                    if (useFlatRawData)
                    {
                        var verticeLength = output.numVertices * sizeof(float) * 3;
                        Buffer.BlockCopy(rawBuffer, (int)dataIndex, vertices, 0, (int)verticeLength);
                        dataIndex += verticeLength;
                        var trianglesLength = output.numTriangles * sizeof(Int32) * 3;
                        Buffer.BlockCopy(rawBuffer, (int)dataIndex, triangles, 0, (int)trianglesLength);
                        dataIndex += trianglesLength;
                    }
                    else
                    {
                        var verticeLength = output.numVertices * 3;
                        Marshal.Copy(output.vertices, vertices, 0, (int)verticeLength);
                        var trianglesLength = output.numTriangles * 3;
                        Marshal.Copy(output.triangles, triangles, 0, (int)trianglesLength);
                    }
                    this.Data[i].vertices = vertices;
                    this.Data[i].triangles = triangles;
                    this.Data[i].blockIndex = output.blockIndex;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        private struct RawModelingOutput
        {
            /// <summary>
            /// 頂点配列
            /// </summary>
            public IntPtr vertices;

            /// <summary>
            /// TODO+ C
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 verticesBufferSize;

            /// <summary>
            /// TODO+ C
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 numVertices;

            /// <summary>
            /// トライアングル配列
            /// </summary>
            public IntPtr triangles;

            /// <summary>
            /// TODO+ C
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 trianglesBufferSize;

            /// <summary>
            /// TODO+ C
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 numTriangles;

            /// <summary>
            /// ブロックインデックス
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] blockIndex;
        };
    }
}

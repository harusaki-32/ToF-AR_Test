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

namespace TofAr.V0.Mesh
{
    /// <summary>
    /// Meshデータ
    /// </summary>
    public struct MeshDataSet
    {
        /// <summary>
        /// 頂点配列
        /// </summary>
        public UnityEngine.Vector3[] vertexes;

        /// <summary>
        /// トライアングル配列
        /// </summary>
        public int[] triangles;

        /// <summary>
        /// trueの場合はトライアングルをリセットする
        /// </summary>
        public bool resetTriangle;
    }

    /// <summary>
    /// Meshデータ送出前処理
    /// </summary>
    public interface IPreProcessMeshData
    {
        /// <summary>
        /// Meshデータ変換
        /// </summary>
        /// <param name="meshDataSet">Meshデータ</param>
        MeshDataSet ParseMeshData(MeshDataSet meshDataSet);
    }
}

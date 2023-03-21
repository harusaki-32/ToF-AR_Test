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
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace TofAr.V0.Mesh
{
    /// <summary>
    /// Mesh生成結果を可視化する
    /// <para>同一GameObjectにアタッチされたMeshRendererのMaterialを使用して可視化を行う</para>
    /// </summary>
    public class DynamicMesh : MonoBehaviour
    {
        /// <summary>
        /// 法線ベクトル計算間隔フレーム数
        /// </summary>
        public int RecalculateNormalsPeriod = 30;

        /// <summary>
        /// 描画クリッピング距離。Materialに"_ClippingDistance"パラメータとして入力される。
        /// <para>Assets/TofAr/TofArMesh/V0/Shaders/OcclusionWShadows.shader をMaterialで使用した場合、設定値以遠の描画がクリッピングされる。</para>
        /// <para>デフォルト=1000</para>
        /// </summary>
        public float ClippingDistance = 1000;

        private int recalculateNormalsCounter;
        private UnityEngine.Mesh mesh;
        private Vector3[] vertices = new Vector3[0];
        private int[] triangles = new int[0];
        private bool disabled = true;

        private int verticesCount = 0;
        private int lastVerticesCount = 0;
        private bool meshUpdated = false;

        private MeshRenderer meshRenderer = null;

        void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.indexFormat = IndexFormat.UInt32;

            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            TofArMeshManager.OnFrameArrived += MeshFrameArrived;
            disabled = false;
        }

        private void OnDisable()
        {
            TofArMeshManager.OnFrameArrived -= MeshFrameArrived;
            disabled = true;
        }


        private void ProcessMesh(out Vector3[] verticesOut, out Vector2[] uvsOut, out int[] trianglesOut)
        {
            verticesOut = new Vector3[triangles.Length];
            uvsOut = new Vector2[triangles.Length];
            trianglesOut = new int[triangles.Length];

            int nTriangles = triangles.Length / 3;
            for (int i = 0; i < nTriangles; i++)
            {
                int idx0 = triangles[i * 3 + 0];
                int idx1 = triangles[i * 3 + 1];
                int idx2 = triangles[i * 3 + 2];
                verticesOut[i * 3 + 0] = vertices[idx0];
                verticesOut[i * 3 + 1] = vertices[idx1];
                verticesOut[i * 3 + 2] = vertices[idx2];

                uvsOut[i * 3 + 0] = new Vector2(0.0f, 0.0f);
                uvsOut[i * 3 + 1] = new Vector2(1.0f, 0.0f);
                uvsOut[i * 3 + 2] = new Vector2(0.0f, 1.0f);

                trianglesOut[i * 3 + 0] = i * 3 + 0;
                trianglesOut[i * 3 + 1] = i * 3 + 1;
                trianglesOut[i * 3 + 2] = i * 3 + 2;
            }
        }

        private void Update()
        {
            if (this.meshUpdated)
            {
                this.meshUpdated = false;
                lock (vertices)
                {
                    if (lastVerticesCount != verticesCount)
                    {
                        lastVerticesCount = verticesCount;
                        mesh.Clear();
                        var graphicsDeviceType = SystemInfo.graphicsDeviceType;
                        if (graphicsDeviceType == GraphicsDeviceType.Metal)
                        {
                            Vector3[] newVertices;
                            Vector2[] uvs;
                            int[] newTriangles;

                            ProcessMesh(out newVertices, out uvs, out newTriangles);
                            mesh.vertices = newVertices;
                            mesh.uv = uvs;
                            mesh.triangles = newTriangles;
                        }
                        else
                        {
                            mesh.vertices = vertices;
                            mesh.triangles = triangles;
                        }

                        mesh.RecalculateNormals();
                        recalculateNormalsCounter = 0;
                    }
                    else
                    {
                        var graphicsDeviceType = SystemInfo.graphicsDeviceType;
                        if (graphicsDeviceType == GraphicsDeviceType.Metal)
                        {
                            Vector3[] newVertices;
                            Vector2[] uvs;
                            int[] newTriangles;

                            ProcessMesh(out newVertices, out uvs, out newTriangles);
                            mesh.vertices = newVertices;
                            mesh.uv = uvs;
                            mesh.triangles = newTriangles;
                        }
                        else
                        {
                            mesh.vertices = vertices;
                            mesh.triangles = triangles;
                        }

                        recalculateNormalsCounter += 1;
                        if (recalculateNormalsCounter >= RecalculateNormalsPeriod)
                        {
                            mesh.RecalculateNormals();
                            recalculateNormalsCounter = 0;
                        }
                    }
                }
            }


            meshRenderer.material.SetFloat("_ClippingDistance", ClippingDistance);
        }

        private void MeshFrameArrived(object sender)
        {
            if (disabled)
            {
                return;
            }
            var manager = sender as TofArMeshManager;
            if (manager == null)
            {
                return;
            }

            var meshData = manager.MeshData;

            lock (vertices)
            {
                verticesCount = meshData.verticesCount;
                triangles = meshData.trianglesBuffer;
                vertices = meshData.vertices;
            }

            meshUpdated = true;
        }
    }
}

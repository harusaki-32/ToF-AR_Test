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
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace TofAr.V0.Tof
{
    /// <summary>
    /// PointCloudデータをMeshオブジェクトに変換する
    /// </summary>
    public class PointCloudMeshVisualizer : MonoBehaviour
    {
        private Mesh mesh;

        private int latestPointNum = 0;
        private Vector3[] vertices;
        private object meshLock = new object();
        private int[] indices = new int[0];
        private bool updated = false;
        private bool restarted = false;

        void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.Clear();
        }

        void OnEnable()
        {
            TofArTofManager.OnFrameArrived += OnTofFrameArrived;
            TofArTofManager.OnStreamStarted += OnTofStreamStarted;
        }

        private void OnDisable()
        {
            TofArTofManager.OnFrameArrived -= OnTofFrameArrived;
            TofArTofManager.OnStreamStarted -= OnTofStreamStarted;
        }

        private void OnTofStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, PointCloudData pointCloudData)
        {
            restarted = true;
        }

        private void Update()
        {
            if (restarted)
            {
                restarted = false;
                lock (meshLock)
                {
                    mesh.Clear();
                }
            }
            if (updated)
            {
                updated = false;
                lock (meshLock)
                {
                    
                    mesh.vertices = vertices;
                    mesh.SetIndices(indices, MeshTopology.Points, 0);
                }
            }
        }

        private void OnTofFrameArrived(object stream)
        {
            if (!TofArTofManager.Instantiated)
            {
                return;
            }

            var pointCloudData = TofArTofManager.Instance.PointCloudData;
            if (pointCloudData == null || pointCloudData.Points == null)
            {
                return;
            }
            int pointNum = pointCloudData.Points.Length;

            Array.Resize(ref indices, pointNum);
            lock (meshLock)
            {
                if (pointNum > latestPointNum)
                {
                    for (var i = latestPointNum; i < pointNum; i++)
                    {
                        indices[i] = i;
                    }
                }
                vertices = pointCloudData.Points;
            }
            latestPointNum = pointNum;
            updated = true;
        }

    }
}

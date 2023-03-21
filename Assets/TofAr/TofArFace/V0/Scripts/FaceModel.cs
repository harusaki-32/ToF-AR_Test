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

namespace TofAr.V0.Face
{
    /// <summary>
    /// Faceモデル
    /// </summary>
    public class FaceModel : MonoBehaviour
    {
        private Mesh mesh;

        private Vector3[] vertices;
        private Vector3[] normals;
        private Vector2[] uvs;

        /// <summary>
        /// インデックス
        /// </summary>
        [SerializeField]
        protected uint faceIndex = 0;

        /// <summary>
        /// trueの場合、視線を表示する
        /// </summary>
        [SerializeField]
        private bool showGaze = false;

        /// <summary>
        /// trueの場合、視線を表示する
        /// </summary>
        public bool ShowGaze
        {
            get => this.showGaze;
            set
            {
                if (TofArFaceManager.Instance.DetectorType != FaceDetectorType.Internal_ARKit)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "ShowGaze is available on FaceDetectorType.Internal_ARit only.");
                }
                this.showGaze = TofArFaceManager.Instance.DetectorType == FaceDetectorType.Internal_ARKit && value;
                this.leftEye?.SetActive(this.showGaze);
                this.rightEye?.SetActive(this.showGaze);
            }
        }

        /// <summary>
        /// 目オブジェクトのプレファブ
        /// </summary>
        [SerializeField]
        private GameObject eyePrefab = null;

        /// <summary>
        /// 目オブジェクトのプレファブ
        /// </summary>
        public GameObject EyePrefab
        {
            get => this.eyePrefab;
            set
            {
                if (this.eyePrefab == null)
                {
                    throw new ArgumentException("EyePrefab cannot set to null");
                }
                this.eyePrefab = value;
                if (this.leftEye != null)
                {
                    Destroy(this.leftEye);
                    this.leftEye = null;
                }
                if (this.rightEye != null)
                {
                    Destroy(this.rightEye);
                    this.rightEye = null;
                }

                this.leftEye = Instantiate(this.eyePrefab, this.transform);
                this.rightEye = Instantiate(this.eyePrefab, this.transform);
            }
        }
        private GameObject leftEye = null;
        private GameObject rightEye = null;

        /// <summary>
        /// 視線オブジェクトのプレファブ
        /// </summary>
        [SerializeField]
        private GameObject gazePrefab = null;

        /// <summary>
        /// 視線オブジェクトのプレファブ
        /// </summary>
        public GameObject GazePrefab
        {
            get => this.gazePrefab;
            set
            {
                if (this.gazePrefab == null)
                {
                    throw new ArgumentException("GazePrefab cannot set to null");
                }
                this.gazePrefab = value;
                if (this.leftGaze != null)
                {
                    Destroy(this.leftGaze);
                    this.leftGaze = null;
                }
                if (this.rightGaze != null)
                {
                    Destroy(this.rightGaze);
                    this.rightGaze = null;
                }

                this.leftGaze = Instantiate(this.gazePrefab, this.leftEye.transform);
                this.rightGaze = Instantiate(this.gazePrefab, this.rightEye.transform);
            }
        }
        private GameObject leftGaze = null;
        private GameObject rightGaze = null;

        private MeshRenderer meshRenderer;
        private bool topologyUpdated;

        private void Start()
        {
            this.InitializeEyeAndGaze();
        }

        void SetVisible(bool visible)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                return;
            }
            meshRenderer.enabled = visible;
            this.leftEye?.SetActive(visible && this.showGaze);
            this.rightEye?.SetActive(visible && this.showGaze);
        }

        void SetMeshTopology(FaceResult result)
        {
            if (mesh == null)
            {
                return;
            }

            {
                mesh.Clear();

                if (result != null && result.vertices != null && result.indices != null && result.vertices.Length > 0 && result.indices.Length > 0)
                {
                    Vector3[] vertices = ConvertVertices(result.vertices);

                    if (result.uvs.Length > 0)
                    {
                        mesh.SetVertices(vertices, 0, result.uvs.Length);

                        Vector2[] uvs = ConvertUvs(result.uvs);
                        mesh.SetUVs(0, uvs);
                    } else
                    {
                        mesh.SetVertices(vertices);
                    }

                    mesh.SetIndices(result.indices, MeshTopology.Triangles, 0, false);

                    mesh.RecalculateBounds();
                    mesh.RecalculateNormals();
                }

                var meshFilter = GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshFilter.sharedMesh = mesh;
                }

                var meshCollider = GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    meshCollider.sharedMesh = mesh;
                }

                topologyUpdated = true;
            }
        }

        Vector3[] ConvertVertices(TofArVector3[] vectors)
        {
            if (this.vertices == null)
            {
                this.vertices = new Vector3[vectors.Length];
            }
            else if (this.vertices.Length != vectors.Length)
            {
                Array.Resize(ref this.vertices, vectors.Length);
            }

            for (int i = 0; i < vectors.Length; i++)
            {
                this.vertices[i] = vectors[i].GetVector3();
            }

            return this.vertices;
        }

        Vector3[] ConvertNormals(TofArVector3[] vectors)
        {
            if (this.normals == null)
            {
                this.normals = new Vector3[vectors.Length];
            }
            else if (this.normals.Length != vectors.Length)
            {
                Array.Resize(ref this.normals, vectors.Length);
            }

            for (int i = 0; i < vectors.Length; i++)
            {
                this.normals[i] = vectors[i].GetVector3();
            }

            return this.normals;
        }

        Vector2[] ConvertUvs(TofArVector2[] vectors)
        {
            if (this.uvs == null)
            {
                this.uvs = new Vector2[vectors.Length];
            }
            else if (this.uvs.Length != vectors.Length)
            {
                Array.Resize(ref this.uvs, vectors.Length);
            }

            for (int i = 0; i < vectors.Length; i++)
            {
                this.uvs[i] = vectors[i].GetVector2();
            }

            return this.uvs;
        }

        void UpdateVisibility(FaceResult result)
        {
            var visible = enabled && (result.trackingState != TrackingState.None);

            SetVisible(visible);
        }

        private void Update()
        {
            if (TofArFaceManager.Instance?.IsStreamActive == true || TofArFaceManager.Instance.IsPlaying)
            {
                var results = TofArFaceManager.Instance?.FaceData?.Data?.results;
                if (results == null)
                {
                    SetVisible(false);
                    return;
                }
                if ((0 <= this.faceIndex && this.faceIndex < results.Length))
                {
                    FaceResult faceResult = results[this.faceIndex];
                    UpdateVisibility(faceResult);
                    if (faceResult.pose != null)
                    {
                        this.transform.localPosition = faceResult.pose.position.GetVector3();
                        this.transform.localRotation = faceResult.pose.rotation.GetQuaternion();
                    }
                    if (!topologyUpdated)
                    {
                        this.SetMeshTopology(faceResult);

                    }
                    topologyUpdated = false;

                    if ((TofArFaceManager.Instance?.DetectorType != FaceDetectorType.Internal_ARKit) && this.ShowGaze)
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, "ShowGaze is available on FaceDetectorType.Internal_ARit only.");
                        this.ShowGaze = false;
                    }
                    this.leftEye.transform.localPosition = faceResult.leftEye.position.GetVector3();
                    this.leftEye.transform.localRotation = faceResult.leftEye.rotation.GetQuaternion();
                    this.rightEye.transform.localPosition = faceResult.rightEye.position.GetVector3();
                    this.rightEye.transform.localRotation = faceResult.rightEye.rotation.GetQuaternion();
                }
                else
                {
                    SetVisible(false);
                }
            }

        }

        void Awake()
        {
            mesh = new Mesh();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void OnEnable()
        {
            TofArFaceManager.OnStreamStopped += OnFaceStreamStopped;

            if ((0 <= this.faceIndex && this.faceIndex < TofArFaceManager.Instance?.FaceData?.Data?.results?.Length))
            {
                FaceResult faceResult = TofArFaceManager.Instance?.FaceData?.Data?.results[this.faceIndex];
                UpdateVisibility(faceResult);
            }
        }



        void OnDisable()
        {
            TofArFaceManager.OnStreamStopped -= OnFaceStreamStopped;
        }

        private void OnFaceStreamStopped(object sender)
        {
            SetVisible(false);
        }

        private void InitializeEyeAndGaze()
        {
            this.EyePrefab = this.eyePrefab;
            this.GazePrefab = this.gazePrefab;
            this.ShowGaze = this.showGaze;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                this.InitializeEyeAndGaze();
            }
        }
#endif
    }
}

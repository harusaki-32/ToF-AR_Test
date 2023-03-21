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
using System.Collections.Generic;
using UnityEngine;

namespace TofAr.V0.Body
{
    /// <summary>
    /// BodySkeletonプレファブの実装
    /// </summary>
    public class SkeletonVisualizer : ModelVisualizer
    {
        /// <summary>
        /// 関節の表示半径
        /// </summary>
        [SerializeField]
        protected float jointRadius = 0.1f;

        /// <summary>
        /// 関節の表示半径
        /// </summary>
        public float JointRadius
        {
            get => this.jointRadius;
            set => this.jointRadius = value;
        }

        /// <summary>
        /// 関節の表示メッシュ
        /// </summary>
        [SerializeField]
        protected UnityEngine.Mesh jointMesh;

        /// <summary>
        /// 関節の表示メッシュ
        /// </summary>
        public UnityEngine.Mesh JointMesh
        {
            get => this.jointMesh;
            set => this.jointMesh = value;
        }

        /// <summary>
        /// 関節の表示マテリアル
        /// </summary>
        [SerializeField]
        protected Material jointMaterial;

        /// <summary>
        /// 関節の表示マテリアル
        /// </summary>
        public Material JointMaterial
        {
            get => this.jointMaterial;
            set {
                this.jointMaterial = value;
                this.UpdateJointMaterials();
            }
        }

        /// <summary>
        /// 骨格の表示半径
        /// </summary>
        [SerializeField]
        protected float boneRadius = 0.02f;

        /// <summary>
        /// 骨格の表示半径
        /// </summary>
        public float BoneRadius
        {
            get => this.boneRadius;
            set => this.boneRadius = value;
        }

        /// <summary>
        /// 骨格の表示メッシュ
        /// </summary>
        [SerializeField]
        protected UnityEngine.Mesh boneMesh;

        /// <summary>
        /// 骨格の表示メッシュ
        /// </summary>
        public UnityEngine.Mesh BoneMesh
        {
            get => this.boneMesh;
            set => this.boneMesh = value;
        }

        /// <summary>
        /// 骨格の表示マテリアル
        /// </summary>
        [SerializeField]
        protected Material boneMaterial;

        /// <summary>
        /// 骨格の表示マテリアル
        /// </summary>
        public Material BoneMaterial
        {
            get => this.boneMaterial;
            set => this.boneMaterial = value;
        }

        /// <summary>
        /// Bodyインデックス
        /// </summary>
        [SerializeField]
        protected uint bodyIndex = 0;

        /// <summary>
        /// Bodyインデックス
        /// </summary>
        public uint BodyIndex
        {
            get => this.bodyIndex;
            set => this.bodyIndex = value;
        }

        /// <summary>
        /// 関節色リスト
        /// </summary>
        public UnityEngine.Color[] jointColors = {
            new UnityEngine.Color(1, 0, 0, 0.5f),
            new UnityEngine.Color(1f, 0.33f, 0, 0.5f),
            new UnityEngine.Color(1f, 0.67f, 0, 0.5f),
            new UnityEngine.Color(1f, 1f, 0, 0.5f),
            new UnityEngine.Color(0.67f, 1f, 0, 0.5f),
            new UnityEngine.Color(0.33f, 1f, 0, 0.5f),
            new UnityEngine.Color(0, 1f, 0, 0.5f),
            new UnityEngine.Color(0, 1f, 0.33f, 0.5f),
            new UnityEngine.Color(0, 1f, 0.67f, 0.5f),
            new UnityEngine.Color(0, 1f, 1f, 0.5f),
            new UnityEngine.Color(0, 0.67f, 1f, 0.5f),
            new UnityEngine.Color(0, 0.33f, 1f, 0.5f),
            new UnityEngine.Color(0, 0, 1f, 0.5f),
            new UnityEngine.Color(0.33f, 0, 1f, 0.5f),
            new UnityEngine.Color(0.67f, 0, 1f, 0.5f),
            new UnityEngine.Color(1f, 0, 1f, 0.5f),
            new UnityEngine.Color(1f, 0, 0.67f, 0.5f),
            new UnityEngine.Color(1f, 0, 0.33f, 0.5f),
            new UnityEngine.Color(0.33f, 0.33f, 0.33f, 0.5f),
            new UnityEngine.Color(0.67f, 0.67f, 0.67f, 0.5f),
            new UnityEngine.Color(1f, 1f, 1f, 0.5f),
        };

        private Material[] jointMaterials;

        private void Start()
        {
            this.jointMaterials = new Material[Enum.GetNames(typeof(JointIndices)).Length];
            this.UpdateJointMaterials();
        }

        private void UpdateJointMaterials()
        {
            for (var i = 0; i < this.jointMaterials.Length; i++)
            {
                if (this.jointMaterials[i] != null)
                {
                    Material.Destroy(this.jointMaterials[i]);
                }
                this.jointMaterials[i] = new Material(jointMaterial);
            }
        }

        private void Update()
        {
            if ((TofArBodyManager.Instance.IsStreamActive || TofArBodyManager.Instance.IsPlaying) && (0 <= this.bodyIndex) && (this.bodyIndex < TofArBodyManager.Instance?.BodyData?.Data?.results?.Length))
            {
                this.Apply(TofArBodyManager.Instance?.BodyData?.Data?.results[this.bodyIndex]);
            }
        }

        /// <summary>
        /// Body認識結果を表示する
        /// </summary>
        /// <param name="bodyResult">Body認識結果データ</param>
        public override void Apply(BodyResult bodyResult)
        {
            if (!this.gameObject.activeInHierarchy)
            {
                return;
            }
            if (bodyResult.trackingState == TrackingState.None)
            {
                return;
            }

            var parentsDict = new Dictionary<int, HumanBodyJoint>();
            for (var i = 0; i < bodyResult.joints.Length; i++)
            {
                var joint = bodyResult.joints[i];
                this.DrawJoint(bodyResult.joints[i], this.jointMaterials[i], this.jointColors[i % this.jointColors.Length], this.jointRadius, bodyResult.pose);
                parentsDict[joint.index] = joint;
            }

            foreach (var joint in bodyResult.joints)
            {
                if (joint.tracked)
                {
                    if (joint.parentIndex > 0 && parentsDict.ContainsKey(joint.parentIndex))
                    {
                        var parentJoint = parentsDict[joint.parentIndex];
                        this.DrawBone(joint, parentJoint, this.boneRadius, bodyResult.pose);
                    }
                }
            }
        }

        private void DrawJoint(HumanBodyJoint joint, Material mat, UnityEngine.Color jointColor, float radius, Pose anchor)
        {
            var position = (anchor.rotation.GetQuaternion() * joint.anchorPose.position.GetVector3()) + anchor.position.GetVector3();
            var rotation = anchor.rotation.GetQuaternion() * joint.anchorPose.rotation.GetQuaternion();

            if (joint.tracked)
            {
                var localMatrix = Matrix4x4.TRS(position, rotation, Vector3.one * radius);
                var worldMatrix = transform.localToWorldMatrix * localMatrix;
                mat.color = jointColor;
                Graphics.DrawMesh(jointMesh, worldMatrix, mat, gameObject.layer);
            }
        }

        /// <summary>
        /// 骨格表示
        /// </summary>
        /// <param name="joint">関節データ</param>
        /// <param name="parentJoint">親関節データ</param>
        /// <param name="radius">表示半径</param>
        /// <param name="anchor">アンカー</param>
        protected void DrawBone(HumanBodyJoint joint, HumanBodyJoint parentJoint, float radius, Pose anchor)
        {
            var startPosition = ((joint != null) && joint.tracked) ? (anchor.rotation.GetQuaternion() * joint.anchorPose.position.GetVector3()) + anchor.position.GetVector3() : Vector3.zero;
            var endPosition = ((parentJoint != null) && parentJoint.tracked) ? (anchor.rotation.GetQuaternion() * parentJoint.anchorPose.position.GetVector3()) + anchor.position.GetVector3() : Vector3.zero;
            {
                var startToEnd = endPosition - startPosition;
                var length = startToEnd.magnitude;
                if (Mathf.Approximately(length, 0))
                {
                    startToEnd = Vector3.forward;
                }
                var position = Vector3.Lerp(startPosition, endPosition, 0.5f);
                var localMatrix = Matrix4x4.TRS(position,
                    Quaternion.LookRotation(startToEnd) * Quaternion.AngleAxis(90, Vector3.right),
                    new Vector3(radius * 2.0f, length / 2, radius * 2.0f));
                var worldMatrix = transform.localToWorldMatrix * localMatrix;
                Graphics.DrawMesh(boneMesh, worldMatrix, boneMaterial, gameObject.layer);
            }
        }
    }
}

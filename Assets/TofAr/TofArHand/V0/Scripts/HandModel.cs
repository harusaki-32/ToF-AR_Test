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

namespace TofAr.V0.Hand
{
    /// <summary>
    /// HandModelプレファブの実装
    /// </summary>
    [ExecuteInEditMode]
    public class HandModel : AbstractHandModel
    {
        /// <summary>
        /// 関節の表示半径
        /// </summary>
        [SerializeField]
        protected float jointRadius;

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
            set => this.jointMaterial = value;
        }

        /// <summary>
        /// 骨格の表示半径
        /// </summary>
        [SerializeField]
        protected float boneRadius;

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
        /// 骨格、関節位置の影表示
        /// </summary>
        [SerializeField]
        protected bool castShadows = false;

        /// <summary>
        /// 骨格、関節位置の影表示
        /// </summary>
        public bool CastShadows
        {
            get => castShadows;
            set => castShadows = value;
        }

        /// <summary>
        /// 骨格、関節位置に対する影の影響
        /// </summary>
        [SerializeField]
        protected bool receiveShadows = false;

        /// <summary>
        /// 骨格、関節位置に対する影の影響
        /// </summary>
        public bool RecieveShadows
        {
            get => receiveShadows;
            set => receiveShadows = value;
        }

        //this still isn't set or used - shouldn't we remove it or deprecate it?
        /// <summary>
        /// 認識モード
        /// <para>デフォルト値：OneHandHoldSmapho</para>
        /// </summary>
        public static RecogMode RecogMode { get; set; }

        /// <summary>
        /// オフセットスケール
        /// </summary>
        protected Vector3 scaleOffset = Vector3.zero;

        private void Awake()
        {
            //display for the editor
#if UNITY_EDITOR
            if ((((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.WindowsPlayer)
                        || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.OSXPlayer))
                        && !Application.isPlaying))
            {
                var featurePointsLength = Enum.GetNames(typeof(HandPointIndex)).Length;
                Array.Resize(ref handPoints, featurePointsLength);
                handPoints[(int)HandPointIndex.Wrist] = new Vector3(0, -0.04f, 0.3f);
                handPoints[(int)HandPointIndex.ThumbRootWrist] = new Vector3(-0.03f, 0, 0.3f);
                handPoints[(int)HandPointIndex.ThumbRoot] = new Vector3(-0.048f, 0.02f, 0.3f);
                handPoints[(int)HandPointIndex.ThumbJoint] = new Vector3(-0.057f, 0.04f, 0.3f);
                handPoints[(int)HandPointIndex.ThumbTip] = new Vector3(-0.06f, 0.06f, 0.3f);
                handPoints[(int)HandPointIndex.IndexRoot] = new Vector3(-0.015f, 0.025f, 0.3f);
                handPoints[(int)HandPointIndex.IndexJoint] = new Vector3(-0.02f, 0.05f, 0.3f);
                handPoints[(int)HandPointIndex.IndexJoint1st] = new Vector3(-0.025f, 0.075f, 0.3f);
                handPoints[(int)HandPointIndex.IndexTip] = new Vector3(-0.03f, 0.1f, 0.3f);
                handPoints[(int)HandPointIndex.MidRoot] = new Vector3(0, 0.023f, 0.3f);
                handPoints[(int)HandPointIndex.MidJoint] = new Vector3(0, 0.052f, 0.3f);
                handPoints[(int)HandPointIndex.MidJoint1st] = new Vector3(0, 0.081f, 0.3f);
                handPoints[(int)HandPointIndex.MidTip] = new Vector3(0, 0.11f, 0.3f);
                handPoints[(int)HandPointIndex.RingRoot] = new Vector3(0.015f, 0.025f, 0.3f);
                handPoints[(int)HandPointIndex.RingJoint] = new Vector3(0.02f, 0.05f, 0.3f);
                handPoints[(int)HandPointIndex.RingJoint1st] = new Vector3(0.025f, 0.075f, 0.3f);
                handPoints[(int)HandPointIndex.RingTip] = new Vector3(0.03f, 0.1f, 0.3f);
                handPoints[(int)HandPointIndex.PinkyRoot] = new Vector3(0.035f, 0.032f, 0.3f);
                handPoints[(int)HandPointIndex.PinkyJoint] = new Vector3(0.04f, 0.048f, 0.3f);
                handPoints[(int)HandPointIndex.PinkyJoint1st] = new Vector3(0.045f, 0.064f, 0.3f);
                handPoints[(int)HandPointIndex.PinkyTip] = new Vector3(0.05f, 0.08f, 0.3f);
                handStatus = this.LRHand;
            }
            
#endif
        }

        /// <summary>
        /// 手のモデル表示
        /// </summary>
        override protected void DrawHandModel()
        {
            if (!IsHandDetected)
            {
                return;
            }

            if (handStatus == HandStatus.Tip)
            {
                DrawJoint(this.handPoints[(int)HandPointIndex.IndexJoint]);
                DrawJoint(this.handPoints[(int)HandPointIndex.IndexTip]);
                DrawBone(this.handPoints[(int)HandPointIndex.IndexTip], this.handPoints[(int)HandPointIndex.IndexJoint]);
            }
            else
            {
                if (this.handPoints.Length <= (int)HandPointIndex.Wrist || this.handPoints[(int)HandPointIndex.Wrist].z <= 0f)
                {
                    return;
                }

                if (TofArHandManager.Instance?.ProcessLevel == ProcessLevel.HandCenterOnly)
                {
                    DrawJoint(this.handPoints[(int)HandPointIndex.Wrist]);
                    return;
                }

                float scaleFactor = this.transform.localScale.x;
                Vector3 center = (this.handPoints[(int)HandPointIndex.WristPinkySide] + this.handPoints[(int)HandPointIndex.WristThumbSide]) / 2f;
                this.scaleOffset = (center * (1 - 1 / scaleFactor));

                foreach (HandPointIndex pointIndex in Enum.GetValues(typeof(HandPointIndex)))
                {
                    if (pointIndex == HandPointIndex.WristPinkySide || pointIndex == HandPointIndex.WristThumbSide || pointIndex == HandPointIndex.ArmCenter
                        || pointIndex == HandPointIndex.HandCenter)
                    {
                        continue;
                    }
                    DrawJoint(this.handPoints[(int)pointIndex]);
                }

                DrawBone(this.handPoints[(int)HandPointIndex.ThumbTip], this.handPoints[(int)HandPointIndex.ThumbJoint]);
                DrawBone(this.handPoints[(int)HandPointIndex.ThumbJoint], this.handPoints[(int)HandPointIndex.ThumbRoot]);
                DrawBone(this.handPoints[(int)HandPointIndex.ThumbRoot], this.handPoints[(int)HandPointIndex.ThumbRootWrist]);
                DrawBone(this.handPoints[(int)HandPointIndex.ThumbRootWrist], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(this.handPoints[(int)HandPointIndex.IndexTip], this.handPoints[(int)HandPointIndex.IndexJoint1st]);
                DrawBone(this.handPoints[(int)HandPointIndex.IndexJoint1st], this.handPoints[(int)HandPointIndex.IndexJoint]);
                DrawBone(this.handPoints[(int)HandPointIndex.IndexJoint], this.handPoints[(int)HandPointIndex.IndexRoot]);
                DrawBone(this.handPoints[(int)HandPointIndex.IndexRoot], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(this.handPoints[(int)HandPointIndex.MidTip], this.handPoints[(int)HandPointIndex.MidJoint1st]);
                DrawBone(this.handPoints[(int)HandPointIndex.MidJoint1st], this.handPoints[(int)HandPointIndex.MidJoint]);
                DrawBone(this.handPoints[(int)HandPointIndex.MidJoint], this.handPoints[(int)HandPointIndex.MidRoot]);
                DrawBone(this.handPoints[(int)HandPointIndex.MidRoot], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(this.handPoints[(int)HandPointIndex.RingTip], this.handPoints[(int)HandPointIndex.RingJoint1st]);
                DrawBone(this.handPoints[(int)HandPointIndex.RingJoint1st], this.handPoints[(int)HandPointIndex.RingJoint]);
                DrawBone(this.handPoints[(int)HandPointIndex.RingJoint], this.handPoints[(int)HandPointIndex.RingRoot]);
                DrawBone(this.handPoints[(int)HandPointIndex.RingRoot], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(this.handPoints[(int)HandPointIndex.PinkyTip], this.handPoints[(int)HandPointIndex.PinkyJoint1st]);
                DrawBone(this.handPoints[(int)HandPointIndex.PinkyJoint1st], this.handPoints[(int)HandPointIndex.PinkyJoint]);
                DrawBone(this.handPoints[(int)HandPointIndex.PinkyJoint], this.handPoints[(int)HandPointIndex.PinkyRoot]);
                DrawBone(this.handPoints[(int)HandPointIndex.PinkyRoot], this.handPoints[(int)HandPointIndex.Wrist]);



            }

        }

        /// <summary>
        /// 関節位置の表示
        /// </summary>
        /// <param name="position">位置</param>
        protected void DrawJoint(Vector3 position)
        {
            if (position.z <= 0)
            {
                DrawJoint(position, jointRadius);
            }
            else
            {
                DrawJoint(position - this.scaleOffset, jointRadius);
            }
        }

        /// <summary>
        /// 関節位置の表示
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="radius">表示半径</param>
        protected void DrawJoint(Vector3 position, float radius)
        {
            if (position.z > 0f)
            {
                var localMatrix = Matrix4x4.TRS(position, Quaternion.identity,
                Vector3.one * radius * 2.0f);
                var worldMatrix = transform.localToWorldMatrix * localMatrix;
                Graphics.DrawMesh(jointMesh, worldMatrix, jointMaterial,
                    gameObject.layer, null, 0, null, castShadows, receiveShadows);
            }

        }

        /// <summary>
        /// 骨格表示
        /// </summary>
        /// <param name="startPosition">始点位置</param>
        /// <param name="endPosition">終点位置</param>
        protected void DrawBone(Vector3 startPosition, Vector3 endPosition)
        {
            if (startPosition.z <= 0 || endPosition.z <= 0)
            {
                DrawBone(startPosition, endPosition, boneRadius);
            }
            else
            {
                DrawBone(startPosition - this.scaleOffset, endPosition - this.scaleOffset, boneRadius);
            }
        }

        /// <summary>
        /// 骨格表示
        /// </summary>
        /// <param name="startPosition">始点位置</param>
        /// <param name="endPosition">終点位置</param>
        /// <param name="radius">表示半径</param>
        protected void DrawBone(Vector3 startPosition, Vector3 endPosition, float radius)
        {
            if (startPosition.z > 0f && endPosition.z > 0f)
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
                Graphics.DrawMesh(boneMesh, worldMatrix, boneMaterial,
                    gameObject.layer, null, 0, null, castShadows, receiveShadows);
            }
        }
    }
}

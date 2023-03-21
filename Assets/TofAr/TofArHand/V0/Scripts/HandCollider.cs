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
    /// HandModel に衝突判定を付与するコンポーネント
    /// </summary>
    public class HandCollider : AbstractHandModel
    {
        [SerializeField]
        private Collider JointColliderMaster = null;

        [SerializeField]
        private Collider BoneColliderMaster = null;

        private Collider[] jointColliders;
        private Collider[] boneColliders;

        /// <summary>
        /// 認識モード
        /// </summary>
        public static RecogMode RecogMode
        { get; set; }

        const int nbones = 25;
        int njoints = Enum.GetValues(typeof(HandPointIndex)).Length;
        bool[] boneCollidersActiveStatus;
        bool[] jointCollidersActiveStatus;


        //start is only called once per game object
        private void Start()
        {
            //create the collider arrays
            //this is the parent, so when this object is destroyed they are destroyed
            boneColliders = new Collider[nbones];
            for (int i = 0; i < nbones; i++)
            {
                boneColliders[i] = Instantiate(BoneColliderMaster, this.transform);
                boneColliders[i].enabled = false;
            }

            jointColliders = new Collider[njoints];
            for (int i = 0; i < njoints; i++)
            {
                jointColliders[i] = Instantiate(JointColliderMaster, this.transform);
                jointColliders[i].enabled = false;
            }
            boneCollidersActiveStatus = new bool[nbones];
            jointCollidersActiveStatus = new bool[njoints];
        }

        /// <summary>
        /// オブジェクトが無効になったときに呼び出されます
        /// </summary>
        protected override void OnDisable()

        {
            if (boneColliders != null)
            {
                foreach (var col in boneColliders)
                {
                    col.enabled = false;
                }
            }

            if (jointColliders != null)
            {
                foreach (var col in jointColliders)
                {
                    col.enabled = false;
                }
            }
            base.OnDisable();
        }

        /// <summary>
        ///  Update関数が呼び出された後に実行されます
        /// </summary>
        override protected void LateUpdate()
        {
            //initially tag them all as inactive then set this true when they are used
            for (int i = 0; i < nbones; i++)
            {
                boneCollidersActiveStatus[i] = false;
            }

            for (int i = 0; i < njoints; i++)
            {
                jointCollidersActiveStatus[i] = false;
            }

            base.LateUpdate();

            //turn off all the unused colliders
            for (int i = 0; i < njoints; i++)
            {
                if (jointColliders[i].enabled != jointCollidersActiveStatus[i])
                {
                    jointColliders[i].enabled = (jointCollidersActiveStatus[i]);
                }
            }
            for (int i = 0; i < nbones; i++)
            {
                if (boneColliders[i].enabled != boneCollidersActiveStatus[i])
                {
                    boneColliders[i].enabled = (boneCollidersActiveStatus[i]);
                }
            }
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
                DrawJoint((int)HandPointIndex.IndexJoint, this.handPoints[(int)HandPointIndex.IndexJoint]);
                DrawJoint((int)HandPointIndex.IndexTip, this.handPoints[(int)HandPointIndex.IndexTip]);
                DrawBone(0, this.handPoints[(int)HandPointIndex.IndexTip], this.handPoints[(int)HandPointIndex.IndexJoint]);
            }
            else
            {
                if (nbones > this.handPoints.Length || this.handPoints[(int)HandPointIndex.HandCenter].z <= 0f)
                {
                    return;
                }

                foreach (HandPointIndex pointIndex in Enum.GetValues(typeof(HandPointIndex)))
                {
                    if (pointIndex == HandPointIndex.WristPinkySide || pointIndex == HandPointIndex.WristThumbSide || pointIndex == HandPointIndex.ArmCenter
                        || pointIndex == HandPointIndex.HandCenter)
                    {
                        continue;
                    }
                    DrawJoint((int)pointIndex, this.handPoints[(int)pointIndex]);
                }

                DrawBone(0, this.handPoints[(int)HandPointIndex.ThumbTip], this.handPoints[(int)HandPointIndex.ThumbJoint]);
                DrawBone(1, this.handPoints[(int)HandPointIndex.ThumbJoint], this.handPoints[(int)HandPointIndex.ThumbRoot]);
                DrawBone(2, this.handPoints[(int)HandPointIndex.ThumbRoot], this.handPoints[(int)HandPointIndex.ThumbRootWrist]);
                DrawBone(3, this.handPoints[(int)HandPointIndex.ThumbRootWrist], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(4, this.handPoints[(int)HandPointIndex.IndexTip], this.handPoints[(int)HandPointIndex.IndexJoint1st]);
                DrawBone(5, this.handPoints[(int)HandPointIndex.IndexJoint1st], this.handPoints[(int)HandPointIndex.IndexJoint]);
                DrawBone(6, this.handPoints[(int)HandPointIndex.IndexJoint], this.handPoints[(int)HandPointIndex.IndexRoot]);
                DrawBone(7, this.handPoints[(int)HandPointIndex.IndexRoot], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(8, this.handPoints[(int)HandPointIndex.MidTip], this.handPoints[(int)HandPointIndex.MidJoint1st]);
                DrawBone(9, this.handPoints[(int)HandPointIndex.MidJoint1st], this.handPoints[(int)HandPointIndex.MidJoint]);
                DrawBone(10, this.handPoints[(int)HandPointIndex.MidJoint], this.handPoints[(int)HandPointIndex.MidRoot]);
                DrawBone(11, this.handPoints[(int)HandPointIndex.MidRoot], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(12, this.handPoints[(int)HandPointIndex.RingTip], this.handPoints[(int)HandPointIndex.RingJoint1st]);
                DrawBone(13, this.handPoints[(int)HandPointIndex.RingJoint1st], this.handPoints[(int)HandPointIndex.RingJoint]);
                DrawBone(14, this.handPoints[(int)HandPointIndex.RingJoint], this.handPoints[(int)HandPointIndex.RingRoot]);
                DrawBone(15, this.handPoints[(int)HandPointIndex.RingRoot], this.handPoints[(int)HandPointIndex.Wrist]);

                DrawBone(16, this.handPoints[(int)HandPointIndex.PinkyTip], this.handPoints[(int)HandPointIndex.PinkyJoint1st]);
                DrawBone(17, this.handPoints[(int)HandPointIndex.PinkyJoint1st], this.handPoints[(int)HandPointIndex.PinkyJoint]);
                DrawBone(18, this.handPoints[(int)HandPointIndex.PinkyJoint], this.handPoints[(int)HandPointIndex.PinkyRoot]);
                DrawBone(19, this.handPoints[(int)HandPointIndex.PinkyRoot], this.handPoints[(int)HandPointIndex.Wrist]);

            }
        }

        void DrawJoint(int index, Vector3 location)
        {
            if (location.z > 0f && jointColliders.Length > index)
            {
                jointCollidersActiveStatus[index] = true;
                jointColliders[index].transform.localPosition = location;
            }
        }

        void DrawBone(int index, Vector3 startPosition, Vector3 endPosition)
        {
            if (boneColliders.Length > index && startPosition.z > 0f && endPosition.z > 0f)
            {
                boneCollidersActiveStatus[index] = true;
                var startToEnd = endPosition - startPosition;
                var length = startToEnd.magnitude;
                if (Mathf.Approximately(length, 0))
                {
                    startToEnd = Vector3.forward;
                }
                //assume we are using a capsule collider, with unit lenth in the y direction
                boneColliders[index].transform.localScale = new Vector3(boneColliders[index].transform.localScale.x,
                                                                length, boneColliders[index].transform.localScale.z);
                var position = Vector3.Lerp(startPosition, endPosition, 0.5f);
                boneColliders[index].transform.localPosition = position;

                boneColliders[index].transform.localRotation = Quaternion.FromToRotation(Vector3.up, startToEnd);
            }
        }
    }
}

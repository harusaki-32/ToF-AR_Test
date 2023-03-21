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
    /// 21 点認識結果による Hand Model 制御
    /// </summary>
    public class HandBoneRemapper5 : HandBoneRemapperBase
    {
        /// <summary>
        /// *TODO+ B ?
        /// 回転のオフセット
        /// </summary>        
        [SerializeField]
        private Vector3[] RotOffset;

        /// <summary>
        /// *TODO+ B ?
        /// </summary>        
        protected override void Awake()
        {
            base.Awake();
            initRotation[(int)Joint.Pinky1] = Quaternion.identity;
            initRotation[(int)Joint.Pinky2] = Quaternion.identity;
            initRotation[(int)Joint.Pinky3] = Quaternion.identity;
            initRotation[(int)Joint.Ring1] = Quaternion.identity;
            initRotation[(int)Joint.Ring2] = Quaternion.identity;
            initRotation[(int)Joint.Ring3] = Quaternion.identity;
            initRotation[(int)Joint.Mid1] = Quaternion.identity;
            initRotation[(int)Joint.Mid2] = Quaternion.identity;
            initRotation[(int)Joint.Mid3] = Quaternion.identity;
            initRotation[(int)Joint.Index1] = Quaternion.identity;
            initRotation[(int)Joint.Index2] = Quaternion.identity;
            initRotation[(int)Joint.Index3] = Quaternion.identity;
            reduceXY = 0.2f;
            MaxZ = 90;
            MinZ = -10;
        }

        /// <summary>
        /// 折り曲げた指
        /// </summary>
        protected override void BendFingers()
        {
            Vector3 wrist = JointPos[(int)HandPointIndex.Wrist];
            var center = (JointPos[(int)HandPointIndex.IndexRoot] + JointPos[(int)HandPointIndex.MidRoot] + JointPos[(int)HandPointIndex.RingRoot] + JointPos[(int)HandPointIndex.PinkyRoot]) / 4;

            Vector3 handModelWrist = wrist + (center - wrist).normalized * 0.017f;
            HandRoot.transform.position = handModelWrist;

            var v = wrist - center;
            center += v / 5;
            var wristT = wrist + (JointPos[(int)HandPointIndex.IndexRoot] - JointPos[(int)HandPointIndex.PinkyRoot]) / 4;
            var wristP = wrist + (JointPos[(int)HandPointIndex.PinkyRoot] - JointPos[(int)HandPointIndex.IndexRoot]) / 4;

            Vector3 handAxis = center - wrist;
            if (LRHand == HandStatus.RightHand)
            {
                handAxis = -handAxis;
            }

            Vector3 upVector = Vector3.Cross(handAxis, wristP - wristT);
            HandRoot.transform.LookAt(wrist + handAxis, upVector);

            SetRotation(HandPointIndex.PinkyRoot, HandPointIndex.Wrist, Joint.Pinky0, Joint.Wrist, false, true);
            SetRotation(HandPointIndex.PinkyJoint, HandPointIndex.PinkyRoot, Joint.Pinky1, Joint.Pinky0, false, true);
            SetRotation(HandPointIndex.PinkyJoint1st, HandPointIndex.PinkyJoint, Joint.Pinky2, Joint.Pinky1, true, true);
            SetRotation(HandPointIndex.PinkyTip, HandPointIndex.PinkyJoint1st, Joint.Pinky3, Joint.Pinky2, true, true);

            SetRotation(HandPointIndex.RingRoot, HandPointIndex.Wrist, Joint.Ring0, Joint.Wrist, false, true);
            SetRotation(HandPointIndex.RingJoint, HandPointIndex.RingRoot, Joint.Ring1, Joint.Ring0, false, true);
            SetRotation(HandPointIndex.RingJoint1st, HandPointIndex.RingJoint, Joint.Ring2, Joint.Ring1, true, true);
            SetRotation(HandPointIndex.RingTip, HandPointIndex.RingJoint1st, Joint.Ring3, Joint.Ring2, true, true);

            SetRotation(HandPointIndex.MidRoot, HandPointIndex.Wrist, Joint.Mid0, Joint.Wrist, false, true);
            SetRotation(HandPointIndex.MidJoint, HandPointIndex.MidRoot, Joint.Mid1, Joint.Mid0, false, true);
            SetRotation(HandPointIndex.MidJoint1st, HandPointIndex.MidJoint, Joint.Mid2, Joint.Mid1, true, true);
            SetRotation(HandPointIndex.MidTip, HandPointIndex.MidJoint1st, Joint.Mid3, Joint.Mid2, true, true);

            SetRotation(HandPointIndex.IndexRoot, HandPointIndex.Wrist, Joint.Index0, Joint.Wrist, false, true);
            SetRotation(HandPointIndex.IndexJoint, HandPointIndex.IndexRoot, Joint.Index1, Joint.Index0, false, true);
            SetRotation(HandPointIndex.IndexJoint1st, HandPointIndex.IndexJoint, Joint.Index2, Joint.Index1, true, true);
            SetRotation(HandPointIndex.IndexTip, HandPointIndex.IndexJoint1st, Joint.Index3, Joint.Index2, true, true);

            SetRotation(HandPointIndex.ThumbRoot, HandPointIndex.Wrist, Joint.Thumb0, Joint.Wrist, false, false);
            SetRotation(HandPointIndex.ThumbJoint, HandPointIndex.ThumbRoot, Joint.Thumb2, Joint.Thumb0, false, false);
            SetRotation(HandPointIndex.ThumbTip, HandPointIndex.ThumbJoint, Joint.Thumb3, Joint.Thumb2, false, false);

            for (int i = 0; i < ModelJoints.Length - 1; i++)
            {
                ModelJoints[i].localRotation = Quaternion.Lerp(ModelJoints[i].localRotation, remapTarget_rot[i], lowpassFactor);
            }
        }

        /// <summary>
        /// 回転設定
        /// </summary>
        /// <param name="e"></param>
        /// <param name="eRoot"></param>
        /// <param name="jTargeet">反映対象</param>
        /// <param name="jRoot"></param>
        protected override void SetRotation(HandPointIndex e, HandPointIndex eRoot, Joint jTarget,
                                            Joint jRoot, bool freezeXY, bool fixRotation)
        {
            Vector3 v1 = JointPos[(int)e] - JointPos[(int)eRoot];
            v1 = Quaternion.Inverse(ModelJoints[(int)jRoot].rotation) * v1;

            if (LRHand == HandStatus.LeftHand)
            {
                v1.z = -v1.z;
            }

            Quaternion qt1 = Quaternion.FromToRotation(new Vector3(0, 1, 0), v1);

            Vector3 eas = qt1.eulerAngles;

            if (fixRotation)
            {
                Quaternion qt0 = initRotation[(int)jTarget];
                Vector3 eas0 = qt0.eulerAngles;
                if (eas0.x > 180)
                {
                    eas0.x -= 360;
                }
                if (eas0.y > 180)
                {
                    eas0.y -= 360;
                }
                if (eas0.z > 180)
                {
                    eas0.z -= 360;
                }

                if (freezeXY)
                {
                    if (eas.x > 180 + eas0.x)

                    {
                        eas.x = eas.x - 360;
                    }

                    if (eas.y > 180 + eas0.y)
                    {
                        eas.y = eas.y - 360;
                    }

                    eas.x = (eas.x - eas0.x) * reduceXY + eas0.x;
                    eas.y = (eas.y - eas0.y) * reduceXY + eas0.y;
                }

                if (eas.z > 180 + eas0.z)
                {
                    eas.z = eas.z - 360;
                }

                eas.z = Mathf.Max(eas0.z - MaxZ, Mathf.Min(eas0.z - MinZ, eas.z));


                //eas.z = Mathf.Max(- MaxZ, Mathf.Min(- MinZ, eas.z));


                qt1 = Quaternion.Euler(eas);
            }
            remapTarget_rot[(int)jTarget] = qt1 * Quaternion.Euler(RotOffset[(int)e]);
        }
    }
}

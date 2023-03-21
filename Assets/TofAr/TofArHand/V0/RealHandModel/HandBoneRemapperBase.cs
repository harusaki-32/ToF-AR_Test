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
    /// 21 点認識結果による Hand Model 制御 TODO C 基本的に内部処理が多い、一般ユーザーが直接使うものではない
    /// </summary>
    public class HandBoneRemapperBase : AbstractHandModel, IBoneRemapper
    {
        /// <summary>
        /// 26 の関節。CG モデル割当対象。
        /// </summary>
        public enum Joint
        {
            Pinky0,
            Pinky1,         // 第三関節(拳関節)
            Pinky2,         // 第二関節
            Pinky3,         // 第一関節
            PinkyEnd,       // 先端
            Ring0,
            Ring1,
            Ring2,
            Ring3,
            RingEnd,
            Mid0,
            Mid1,
            Mid2,
            Mid3,
            MidEnd,
            Index0,
            Index1,
            Index2,
            Index3,
            IndexEnd,
            Thumb0,
            Thumb2,
            Thumb3,
            ThumbEnd,
            Wrist,
            Arm,
        }

        /// <summary>
        /// 手の関節の位置
        /// </summary>
        public Vector3[] JointPos;

        /// <summary>
        /// モデルが持っている初期角度
        /// </summary>
        protected Quaternion[] initRotation;

        /// <summary>
        /// 各関節とその根本関節との CG モデルでの距離
        /// </summary>
        public float[] DistanceToRoot;

        protected Quaternion[] remapTarget_rot;

        /// <summary>
        /// TODO+ C 内部処理用？
        /// </summary>
        public float reduceXY = 0.15f;

        /// <summary>
        /// TODO+ C 内部処理用？
        /// </summary>        
        public int MaxZ = 95;

        /// <summary>
        /// TODO+ C 内部処理用？
        /// </summary>
        public int MinZ = -8;

        /// <summary>
        /// TODO+ C 内部処理用？
        /// </summary>
        public float lowpassFactor = 0.35f;


        /// <summary>
        /// TODO+ C 内部処理用？
        /// </summary>
        public Transform HandRoot;

        /// <summary>
        /// CG モデルの関節全部。
        /// <para>予め Hand モデル関節の Transform をセットしておく必要性がある。</para>
        /// </summary>
        [SerializeField]
        internal Transform[] modelJoints;

        /// <summary>
        /// TODO+ C 内部処理用？
        /// </summary>    
        public Transform[] ModelJoints { get => modelJoints; }

        /// <summary>
        /// アーマチュア
        /// </summary>
        public Transform Armature => transform.GetChild(0).GetChild(0);

        /// <summary>
        /// アニメーション状態
        /// </summary>
        public bool isOverridingAnimation;
        protected HandStatus currentHand;

        protected virtual void Awake()
        {
            JointPos = new Vector3[Enum.GetValues(typeof(HandPointIndex)).Length];

            remapTarget_rot = new Quaternion[ModelJoints.Length];
            initRotation = new Quaternion[ModelJoints.Length];

            for (int i = 0; i < ModelJoints.Length; i++)
            {
                remapTarget_rot[i] = ModelJoints[i].localRotation;
                initRotation[i] = ModelJoints[i].localRotation;
            }



            Vector3 localScale = transform.localScale;

            if (transform.parent != null)
            {
                Vector3 parentScale = transform.parent.lossyScale;
                transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);
            }


            // モデルの、先端 <-> 第二関節、第二関節 <-> 第三関節の距離を保存しておく
            DistanceToRoot = new float[24];
            CalcDistance(Joint.PinkyEnd, Joint.PinkyEnd, Joint.Pinky2);
            CalcDistance(Joint.Pinky2, Joint.Pinky2, Joint.Pinky1);
            CalcDistance(Joint.Pinky1, Joint.Pinky1, Joint.Wrist);
            CalcDistance(Joint.RingEnd, Joint.RingEnd, Joint.Ring2);
            CalcDistance(Joint.Ring2, Joint.Ring2, Joint.Ring1);
            CalcDistance(Joint.Ring1, Joint.Ring1, Joint.Wrist);
            CalcDistance(Joint.MidEnd, Joint.MidEnd, Joint.Mid2);
            CalcDistance(Joint.Mid2, Joint.Mid2, Joint.Mid1);
            CalcDistance(Joint.Mid1, Joint.Mid1, Joint.Wrist);
            CalcDistance(Joint.IndexEnd, Joint.IndexEnd, Joint.Index2);
            CalcDistance(Joint.Index2, Joint.Index2, Joint.Index1);
            CalcDistance(Joint.Index1, Joint.Index1, Joint.Wrist);
            CalcDistance(Joint.ThumbEnd, Joint.ThumbEnd, Joint.Thumb3);
            CalcDistance(Joint.Thumb3, Joint.Thumb3, Joint.Thumb2);
            CalcDistance(Joint.Thumb2, Joint.Thumb2, Joint.Wrist);

            transform.localScale = localScale;

            currentHand = HandStatus.UnknownHand;
        }

        protected Vector3 handInitScale;

        void Start()
        {
            handInitScale = this.transform.GetChild(0).GetChild(0).localScale;
        }

        void CalcDistance(Joint target, Joint e1, Joint e2)
        {
            DistanceToRoot[(int)target] = Vector3.Distance(ModelJoints[(int)e1].position, ModelJoints[(int)e2].position);
        }

        public uint recognizeConfigUpdateIntervalMs = 100;
        public uint recognizeConfigUpdateIntervalMsOnEditorDebbug = 500;
        private DateTime prevRecognizeConfigUpdated = DateTime.MinValue;
        private ProcessLevel currentProcessLevel;

        protected override void DrawHandModel()
        {
            if (!IsHandDetected || isOverridingAnimation)
            {
                return;
            }

            if (!(AssignJointPos()))
            {
                return;
            }

            bool streamRunning = TofArHandManager.Instance.IsStreamActive;

            var handScale = this.transform.GetChild(0).GetChild(0).localScale;
            if (this.LRHand == HandStatus.LeftHand)
            {
                if (this.currentHand != this.LRHand)
                {
                    this.currentHand = this.LRHand;
                    handScale.z *= -1;
                }

            }

            this.transform.GetChild(0).GetChild(0).localScale = handScale;

            if (streamRunning)
            {
                var interval = this.recognizeConfigUpdateIntervalMs;
                if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.MultiNode)
                {
                    interval = this.recognizeConfigUpdateIntervalMsOnEditorDebbug;
                }

                bool processLevelChanged = false;

                if ((DateTime.Now - this.prevRecognizeConfigUpdated).Milliseconds > interval)
                {
                    var recognizeConfig = TofArHandManager.Instance.GetProperty<RecognizeConfigProperty>();
                    if (recognizeConfig != null)
                    {
                        processLevelChanged = recognizeConfig.processLevel != this.currentProcessLevel;
                        this.currentProcessLevel = recognizeConfig.processLevel;
                    }
                    this.prevRecognizeConfigUpdated = DateTime.Now;
                }

                if (this.currentProcessLevel == ProcessLevel.HandCenterOnly) // don't bend fingers when using hand center only
                {
                    float screenRotation = 0f;
                    var camProperty = TofArHandManager.Instance.GetProperty<CameraOrientationProperty>();
                    if (camProperty != null)
                    {
                        switch (camProperty.cameraOrientation)
                        {
                            case CameraOrientation.Portrait:
                                screenRotation = 90f;
                                break;
                            case CameraOrientation.PortraitUpsideDown:
                                screenRotation = -90f;
                                break;
                            case CameraOrientation.LandscapeLeft:
                                screenRotation = 0f;
                                break;
                            case CameraOrientation.LandscapeRight:
                                screenRotation = 180f;
                                break;
                        }

                        if (LRHand == HandStatus.LeftHand)
                        {
                            HandRoot.transform.localRotation = Quaternion.Euler(0, 0, -screenRotation) * Quaternion.LookRotation(Vector3.up, -Vector3.forward);
                        }
                        else
                        {
                            HandRoot.transform.localRotation = Quaternion.Euler(0, 0, -screenRotation) * Quaternion.LookRotation(Vector3.up, Vector3.forward);
                        }
                    }

                    HandRoot.transform.position = JointPos[(int)HandPointIndex.Wrist];

                    if (processLevelChanged)
                    {
                        for (int i = 0; i < ModelJoints.Length; i++)
                        {
                            ModelJoints[i].localRotation = initRotation[i];
                        }
                    }
                    return;
                }
            }

            BendFingers();

        }

        protected virtual void BendFingers()
        {
            Vector3 wrist = JointPos[(int)HandPointIndex.Wrist];
            HandRoot.transform.position = wrist;

            var center = (JointPos[(int)HandPointIndex.IndexRoot] + JointPos[(int)HandPointIndex.MidRoot] + JointPos[(int)HandPointIndex.RingRoot] + JointPos[(int)HandPointIndex.PinkyRoot]) / 4;
            var v = wrist - center;
            center += v / 5;
            var wristT = wrist + (JointPos[(int)HandPointIndex.IndexRoot] - JointPos[(int)HandPointIndex.PinkyRoot]) / 4;
            var wristP = wrist + (JointPos[(int)HandPointIndex.PinkyRoot] - JointPos[(int)HandPointIndex.IndexRoot]) / 4;

            Vector3 handAxis = center - wrist;
            Vector3 upVector = Vector3.Cross(handAxis, wristP - wristT);
            HandRoot.transform.LookAt(wrist + handAxis, upVector);


            SetRotation(HandPointIndex.PinkyRoot, HandPointIndex.Wrist, Joint.Pinky0, Joint.Wrist, false);
            SetRotation(HandPointIndex.PinkyJoint, HandPointIndex.PinkyRoot, Joint.Pinky1, Joint.Pinky0, false);
            SetRotation(HandPointIndex.PinkyJoint1st, HandPointIndex.PinkyJoint, Joint.Pinky2, Joint.Pinky1, true);
            SetRotation(HandPointIndex.PinkyTip, HandPointIndex.PinkyJoint1st, Joint.Pinky3, Joint.Pinky2, true);

            SetRotation(HandPointIndex.RingRoot, HandPointIndex.Wrist, Joint.Ring0, Joint.Wrist, false);
            SetRotation(HandPointIndex.RingJoint, HandPointIndex.RingRoot, Joint.Ring1, Joint.Ring0, false);
            SetRotation(HandPointIndex.RingJoint1st, HandPointIndex.RingJoint, Joint.Ring2, Joint.Ring1, true);
            SetRotation(HandPointIndex.RingTip, HandPointIndex.RingJoint1st, Joint.Ring3, Joint.Ring2, true);

            SetRotation(HandPointIndex.MidRoot, HandPointIndex.Wrist, Joint.Mid0, Joint.Wrist, false);
            SetRotation(HandPointIndex.MidJoint, HandPointIndex.MidRoot, Joint.Mid1, Joint.Mid0, false);
            SetRotation(HandPointIndex.MidJoint1st, HandPointIndex.MidJoint, Joint.Mid2, Joint.Mid1, true);
            SetRotation(HandPointIndex.MidTip, HandPointIndex.MidJoint1st, Joint.Mid3, Joint.Mid2, true);

            SetRotation(HandPointIndex.IndexRoot, HandPointIndex.Wrist, Joint.Index0, Joint.Wrist, false);
            SetRotation(HandPointIndex.IndexJoint, HandPointIndex.IndexRoot, Joint.Index1, Joint.Index0, false);
            SetRotation(HandPointIndex.IndexJoint1st, HandPointIndex.IndexJoint, Joint.Index2, Joint.Index1, true);
            SetRotation(HandPointIndex.IndexTip, HandPointIndex.IndexJoint1st, Joint.Index3, Joint.Index2, true);

            SetRotation(HandPointIndex.ThumbRoot, HandPointIndex.Wrist, Joint.Thumb0, Joint.Wrist, false);
            SetRotation(HandPointIndex.ThumbJoint, HandPointIndex.ThumbRoot, Joint.Thumb2, Joint.Thumb0, false);
            SetRotation(HandPointIndex.ThumbTip, HandPointIndex.ThumbJoint, Joint.Thumb3, Joint.Thumb2, false);

            for (int i = 0; i < ModelJoints.Length; i++)
            {
                ModelJoints[i].localRotation = Quaternion.Lerp(ModelJoints[i].localRotation, remapTarget_rot[i], lowpassFactor);
            }
        }

        protected bool AssignJointPos()
        {
            {
                if (handPoints == null)
                {
                    return false;
                }

                // transform.position はハンドモデルを置く基準点
                for (int i = 0; i < Mathf.Min(handPoints.Length, JointPos.Length); i++)
                {
                    JointPos[i] = Vector3.Scale(transform.rotation * handPoints[i], transform.lossyScale) + transform.position;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="eRoot"></param>
        /// <param name="jTargeet">反映対象</param>
        /// <param name="jRoot"></param>
        protected virtual void SetRotation(HandPointIndex e, HandPointIndex eRoot, Joint jTargeet, Joint jRoot, bool freezeXY, bool fixRotation = false)
        {
            Vector3 v1 = JointPos[(int)e] - JointPos[(int)eRoot];
            v1 = Quaternion.Inverse(ModelJoints[(int)jRoot].rotation) * v1;

            if (LRHand == HandStatus.RightHand)
            {
                v1.z = -v1.z;
            }

            Quaternion qt1 = Quaternion.FromToRotation(new Vector3(0, 1, 0), v1);
            {
                Vector3 eas = qt1.eulerAngles;
                if (freezeXY)
                {
                    if (eas.z > 180)

                    {
                        eas.z = eas.z - 360;
                    }

                    if (eas.y > 180)
                    {
                        eas.y = eas.y - 360;
                    }

                    eas.z = eas.z * reduceXY;
                    eas.y = eas.y * reduceXY;
                }

                if (eas.x > 180)
                {
                    eas.x = eas.x - 360;
                }

                eas.x = Mathf.Max(-MaxZ, Mathf.Min(-MinZ, eas.x));

                qt1 = Quaternion.Euler(eas);
            }

            remapTarget_rot[(int)jTargeet] = qt1;
        }

    }
}

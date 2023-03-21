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
using System.Threading;
using UnityEngine;

namespace TofAr.V0.Slam
{
    /// <summary>
    /// TODO+ C Slamはリファレンスに載せる？
    /// </summary>
    public class MainCameraPoseTracker : MonoBehaviour, ITrackTargetFinder
    {
        private SynchronizationContext sc;

        void Start()
        {
            this.sc = SynchronizationContext.Current;
        }

        void OnEnable()
        {
            //if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.MultiNode)
            {
                TofArSlamManager.OnFrameArrived += this.CameraPoseChanged;
            }
        }

        void OnDisable()
        {
            //if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.MultiNode)
            {
                TofArSlamManager.OnFrameArrived -= this.CameraPoseChanged;
            }
        }

        public Transform GetTrackTarget()
        {
            return Camera.main?.transform;
        }

        private void CameraPoseChanged(object sender)
        {
            if (!TofArManager.Instance.UsingIos && 
                TofArManager.Instance.RuntimeSettings.runMode != RunMode.MultiNode && 
                !TofArSlamManager.Instance.IsPlaying)
            {
                return;
            }

            var pose = TofArSlamManager.Instance.SlamData.Data;
            if (pose != null)
            {
                this.sc.Post((s) =>
                {
                    var poseTracker = s as MainCameraPoseTracker;
                    if (poseTracker != null)
                    {
                        var trackTarget = poseTracker.GetTrackTarget();
                        if (trackTarget != null)
                        {
                            var t = trackTarget.transform;
                            t.position = pose.position.GetVector3();
                            t.rotation = pose.rotation.GetQuaternion();
                        }
                    }
                }, this);
            }
        }
    }
}

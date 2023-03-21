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

using System.Collections.Generic;
using UnityEngine;
using TofAr.V0.Body;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using System.Collections;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    public class ARFoundationBodyConnector : MonoBehaviour
    {
        [SerializeField]
        private ARHumanBodyManager humanBodyManager;

        [SerializeField]
        internal bool autoStart = false;

        ulong currentTimeStamp = 0;

        private void OnEnable()
        {
            humanBodyManager.humanBodiesChanged += OnBodiesChanged;
            TofArBodyManager.Instance.DetectorType = BodyPoseDetectorType.External;
            if (autoStart)
            {
                StartCoroutine(AutoStartCoroutine());
            }
        }

        IEnumerator AutoStartCoroutine()
        {
            yield return new WaitForEndOfFrame();
            TofArBodyManager.Instance.StartStream();
        }

        private void OnDisable()
        {
            humanBodyManager.humanBodiesChanged -= OnBodiesChanged;
            TofArBodyManager.Instance.DetectorType = BodyPoseDetectorType.Internal_SV2;
        }

        private void OnBodiesChanged(ARHumanBodiesChangedEventArgs args)
        {
            BodyResults results = new BodyResults();
            List<BodyResult> bodies = new List<BodyResult>();
            currentTimeStamp = (ulong)(Time.unscaledTime * 1e9f);

            foreach (var body in args.updated)
            {
                bodies.Add(ConvertBodyResult(body));
            }
            foreach (var body in args.added)
            {
                bodies.Add(ConvertBodyResult(body));
            }
            results.results = bodies.ToArray();

            TofArBodyManager.Instance.SetEstimatedResults(results, FrameDataSource.ARFoundationBodySkeleton);
        }

        private BodyResult ConvertBodyResult(ARHumanBody body)
        {
            var rval = new BodyResult
            {
                estimatedHeightScaleFactor = body.estimatedHeightScaleFactor,
                joints = ConvertJoints(body.joints),
                trackableId = ConvertTrackableId(body.trackableId),
                timestamp = currentTimeStamp,
                trackingState = ConvertTrackingState(body.trackingState)
            };

            rval.pose.position = body.pose.position;
            rval.pose.rotation = body.pose.rotation;
            //body.Dispose();
            // V0.TofArManager.Logger.WriteLog(V0.LogLevel.Debug, $"body at {body.pose.position} {body.pose.rotation} converted to {rval.pose.position.GetVector3()} {rval.pose.rotation.GetQuaternion()}");
            return rval;
        }

        private V0.Body.TrackableId ConvertTrackableId(UnityEngine.XR.ARSubsystems.TrackableId id)
        {
            return new V0.Body.TrackableId { subId1 = id.subId1, subId2 = id.subId2 };
        }

        private V0.Body.TrackingState ConvertTrackingState(UnityEngine.XR.ARSubsystems.TrackingState state)
        {
            switch (state)
            {
                case UnityEngine.XR.ARSubsystems.TrackingState.None:
                    return V0.Body.TrackingState.None;
                case UnityEngine.XR.ARSubsystems.TrackingState.Limited:
                    return V0.Body.TrackingState.Limited;
                case UnityEngine.XR.ARSubsystems.TrackingState.Tracking:
                    return V0.Body.TrackingState.Tracking;
                default:
                    return V0.Body.TrackingState.None;
            }
        }

        private TofAr.V0.Body.Pose ConvertPose(UnityEngine.Pose p)
        {
            TofAr.V0.Body.Pose rval = new V0.Body.Pose();
            rval.position = p.position;
            rval.rotation = p.rotation;
            return rval;
        }

        private HumanBodyJoint[] ConvertJoints(NativeArray<XRHumanBodyJoint> joints)
        {
            var rval = new HumanBodyJoint[joints.Length];
            for (int i = 0; i < joints.Length; i++)
            {
                rval[i] = new HumanBodyJoint
                {
                    anchorPose = ConvertPose(joints[i].anchorPose),
                    anchorScale = joints[i].anchorScale,
                    index = joints[i].index,
                    localScale = joints[i].localScale,
                    localPose = ConvertPose(joints[i].localPose),
                    parentIndex = joints[i].parentIndex,
                    tracked = joints[i].tracked
                };
            }
            return rval;
        }
    }
}

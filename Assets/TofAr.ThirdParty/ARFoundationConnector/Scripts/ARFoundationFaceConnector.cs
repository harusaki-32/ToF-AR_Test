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
using TofAr.V0.Face;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS
using UnityEngine.XR.ARKit;
#endif
using Unity.Collections;
using System.Collections;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    public class ARFoundationFaceConnector : MonoBehaviour
    {
        [SerializeField]
        private ARFaceManager arFaceManager;

        [SerializeField]
        internal bool autoStart = false;

        ulong currentTimeStamp = 0;

#if UNITY_IOS
        ARKitFaceSubsystem arkitFaceSubsystem;
#endif

        private void OnEnable()
        {
#if UNITY_IOS
            arkitFaceSubsystem = (ARKitFaceSubsystem) arFaceManager.subsystem;
#endif
            arFaceManager.facesChanged += OnFacesChanged;

            if (autoStart)
            {
                StartCoroutine(AutoStartCoroutine());
            }
        }

        IEnumerator AutoStartCoroutine()
        {
            yield return new WaitForEndOfFrame();
            TofArFaceManager.Instance.StartStream();
        }

        private void OnDisable()
        {
            arFaceManager.facesChanged -= OnFacesChanged;

            TofArFaceManager.Instance.SetEstimatedResults(new FaceResults()
            {
                results = new FaceResult[0]
            });
        }

        private void OnFacesChanged(ARFacesChangedEventArgs args)
        {
            FaceResults results = new FaceResults();
            List<FaceResult> faces = new List<FaceResult>();
            currentTimeStamp = (ulong)(Time.unscaledTime * 1e9f);

            foreach (var face in args.updated)
            {
                faces.Add(ConvertFaceResult(face));
            }
            foreach (var face in args.added)
            {
                faces.Add(ConvertFaceResult(face));
            }
            results.results = faces.ToArray();

            TofArFaceManager.Instance.SetEstimatedResults(results);
        }

        private FaceResult ConvertFaceResult(ARFace face)
        {
            float[] blendShapeCoeffs = new float[52];

#if UNITY_IOS
            using (var blendShapes = arkitFaceSubsystem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp)) {
                int i = 0;
                foreach (var featureCoefficient in blendShapes) {
                    blendShapeCoeffs[i++] = featureCoefficient.coefficient;
                }
            }
#endif


            var rval = new FaceResult
            {
                vertices = ConvertVector(face.vertices),
                //normals = ConvertVector(face.normals),
                uvs = ConvertUvs(face.uvs),
                indices = face.indices.ToArray(),
                fixationPoint = face.fixationPoint ?? new V0.TofArTransform(),
                leftEye = face.leftEye ?? new V0.TofArTransform(),
                rightEye = face.rightEye ?? new V0.TofArTransform(),
                pose = face.transform,
                trackableId = ConvertTrackableId(face.trackableId),
                timestamp = currentTimeStamp,
                trackingState = ConvertTrackingState(face.trackingState),
                blendShapes = blendShapeCoeffs
            };

#if UNITY_ANDROID
            TofArFacialExpressionEstimator.Instance.GetMappedBlendshapes(ref rval);
#endif
            return rval;
        }

        private V0.Face.TrackableId ConvertTrackableId(UnityEngine.XR.ARSubsystems.TrackableId id)
        {
            return new V0.Face.TrackableId { subId1 = id.subId1, subId2 = id.subId2 };
        }

        private V0.Face.TrackingState ConvertTrackingState(UnityEngine.XR.ARSubsystems.TrackingState state)
        {
            switch (state)
            {
                case UnityEngine.XR.ARSubsystems.TrackingState.None:
                    return V0.Face.TrackingState.None;
                case UnityEngine.XR.ARSubsystems.TrackingState.Limited:
                    return V0.Face.TrackingState.Limited;
                case UnityEngine.XR.ARSubsystems.TrackingState.Tracking:
                    return V0.Face.TrackingState.Tracking;
                default:
                    return V0.Face.TrackingState.None;
            }
        }

        private V0.TofArVector3[] ConvertVector(NativeArray<Vector3> vectors)
        {
            var rval = new V0.TofArVector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                rval[i] = new V0.TofArVector3(vectors[i].x, vectors[i].y, vectors[i].z);
            }
            return rval;
        }

        private V0.TofArVector2[] ConvertUvs(NativeArray<Vector2> uvs)
        {
            var rval = new V0.TofArVector2[uvs.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                rval[i] = new V0.TofArVector2(uvs[i].x, uvs[i].y);
            }
            return rval;
        }

    }
}

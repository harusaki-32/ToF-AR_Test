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

using UnityEngine;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// TODO+ C 内部処理クラス
    /// </summary>
    public class RingBuffer
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public int JointArraySize = 43;
        /// <summary>
        /// TODO+ C
        /// </summary>
        public const int RingBufferSize = 60;

        /// <summary>
        /// TODO+ C
        /// </summary>
        public FrameData[] Buffer { get; private set; }
        /// <summary>
        /// TODO+ C
        /// </summary>
        public int TopIndex { get; private set; }
        /// <summary>
        /// TODO+ C
        /// </summary>
        public int DataCount { get; private set; }

        private RecognizeResultProperty lastHandData = new RecognizeResultProperty();

        private bool needsInterpolation = false;
        private bool needsCopy = false;
        private bool recognized = false;

        /// <summary>
        /// TODO+ C
        /// </summary>
        public int maxOffFrame = 1;
        private int offFrame = 0;

        private int imageRotation = 0;
        /// <summary>
        /// TODO+ C
        /// </summary>
        /// <param name="handData">TODO+ C</param>
        /// <param name="wasInterpolated">TODO+ C</param>
        public delegate void GestureEstimationRequestedEventHandler(RecognizeResultProperty handData, bool wasInterpolated = false);
        /// <summary>
        /// TODO+ C
        /// </summary>
        public DeviceOrientation DeviceAngle;
        /// <summary>
        /// TODO+ C
        /// </summary>
        public bool AdjustByAccelerationOnGestureEstimation { get; set; }
        /// <summary>
        /// TODO+ C
        /// </summary>
        public event GestureEstimationRequestedEventHandler GestureEstimationRequested;

        private RingBufferInternal logic = null;

        /// <summary>
        /// TODO+ C
        /// </summary>
        public RingBuffer()
        {
            this.logic = new RingBufferInternal();

            this.Buffer = new FrameData[RingBufferSize];
            for (int i = 0; i < RingBufferSize; i++)
            {
                this.Buffer[i].JointData = new float[JointArraySize];
                this.Buffer[i].CopyFlag = false;
            }
            this.Reset();

            TofArManager.OnScreenOrientationUpdated += OnScreenRotationChanged;

            UpdateRotation();
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        public void Dispose()
        {
            TofArManager.OnScreenOrientationUpdated -= OnScreenRotationChanged;
            this.logic = null;
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        public void Reset()
        {
            this.TopIndex = 0;
            this.DataCount = 0;

            for (int i = 0; i < RingBufferSize; i++)
            {
                for (int j = 0; j < JointArraySize; j++)
                {
                    this.Buffer[i].JointData[j] = 0;
                }
            }

            this.needsInterpolation = false;
            this.needsCopy = false;
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        /// <param name="handData">TODO+ C</param>
        /// <param name="hand">TODO+ C</param>
        /// <returns>TODO+ C</returns>
        public bool Fill(RecognizeResultProperty handData, HandStatus hand)
        {
#if __TOFAR_HAND_TEST
            var timestamp = System.DateTime.Now.Ticks;
            {
                var log = string.Format("{0}: \nhandData.handStatus={1}\nlastHandData.handStatus={2}\nneedsInterpolation={3}\ntopIndex={4}\ndataCount={5}\n---------",
                    timestamp, handData.handStatus, this.lastHandData.handStatus, needsInterpolation, this.TopIndex, this.DataCount);
                TofArManager.Logger.WriteLog(LogLevel.Debug, log);
            }
#endif

            bool lostTrack = (handData.handStatus != HandStatus.RightHand) && (handData.handStatus != HandStatus.LeftHand) && (handData.handStatus != HandStatus.BothHands);

            // Left -> BothHands or BothHands -> Left?

            Vector3[] points = null;

            if (!lostTrack)
            {
                points = (hand == HandStatus.RightHand) ? handData.featurePointsRight : handData.featurePointsLeft;

                lostTrack = lostTrack || (points[(int)HandPointIndex.HandCenter].z <= 0);
            }


            if (!lostTrack)
            {
                return TrackNormal(handData, hand, points);
            }
            else
            {
                if (this.needsInterpolation)
                {
                    if (this.offFrame < this.maxOffFrame)
                    {
                        this.offFrame++;
                        return true;
                    }

                    // Hand stays changed -> reset buffer
                    this.needsInterpolation = false;
                    this.recognized = false;
                }
            }

#if __TOFAR_HAND_TEST
            {
                var log = string.Format("{0}: \nlostTrack={1}\nrecognized={2}\n---------",
                    timestamp, lostTrack, this.recognized);
                TofArManager.Logger.WriteLog(LogLevel.Debug, log);
            }
#endif
            //else if (recognized && needsInterpolation)
            if (lostTrack && this.recognized)
            {

                CopyFromLastValidFrame(handData, hand);
            }
            else
            {
                this.Reset();
                this.lastHandData = new RecognizeResultProperty();
                this.lastHandData.handStatus = HandStatus.NoHand;

                return false;
            }

            return true;
        }

        private bool TrackNormal(RecognizeResultProperty handData, HandStatus hand, Vector3[] points)
        {
            // Hand switched
            if ((handData.handStatus != HandStatus.BothHands) && (hand != handData.handStatus) && (this.lastHandData.handStatus != HandStatus.NoHand))
            {

                if (this.needsCopy)
                {
                    CopyFromLastValidFrame(handData, hand);
                    return true;
                }
                else if (!this.needsInterpolation)
                {
                    // Skip frame - 
                    this.needsInterpolation = true;
                    this.offFrame++;
                    return true;
                }
                else
                {
                    if (this.offFrame < this.maxOffFrame)
                    {
                        this.offFrame++;
                        return true;
                    }

                    // Hand stays changed -> reset buffer
                    this.Reset();
                }
            }

            bool wasInterpolated = false;

            if (this.needsInterpolation) // hand has changed for n frames 
            {
                this.needsInterpolation = false;
                wasInterpolated = true;

                // use data from previous frame and current frame for interpolation
                var pointsInterp = new Vector3[points.Length];

                //var pointsLast = (this.lastHandData.handStatus == HandStatus.RightHand) ? this.lastHandData.featurePointsRight : this.lastHandData.featurePointsLeft;
                var pointsLast = (hand == HandStatus.RightHand) ? this.lastHandData.featurePointsRight : this.lastHandData.featurePointsLeft;

                for (int j = 0; j < this.offFrame; j++)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        pointsInterp[i] = Vector3.Lerp(points[i], pointsLast[i], (float)(j + 1) / (this.offFrame + 1));
                    }

                    this.FillInternal(pointsInterp, this.lastHandData, hand);
                }
            }

            this.needsCopy = false;

            this.offFrame = 0;

            this.FillInternal(points, handData, hand);

            if (GestureEstimationRequested != null)
            {
                GestureEstimationRequested(handData, wasInterpolated);
            }
            this.recognized = true;

            this.lastHandData = new RecognizeResultProperty()
            {
                featurePointsLeft = handData.featurePointsLeft,
                featurePointsRight = handData.featurePointsRight,
                poseLevelsLeft = handData.poseLevelsLeft,
                poseLevelsRight = handData.poseLevelsRight,
                handStatus = hand
            };

            return true;
        }

        private void CopyFromLastValidFrame(RecognizeResultProperty handData, HandStatus hand)
        {


            // if hand was recognized in the previous frame, this frame can be skipped (i.e. use result from last frame)
            if (this.offFrame < (this.maxOffFrame - 1))
            {
                this.needsCopy = true;
                this.offFrame++;
            }
            else
            {
                this.needsCopy = false;
                this.recognized = false;
            }


            var pointsLast = hand == HandStatus.RightHand ? this.lastHandData.featurePointsRight : this.lastHandData.featurePointsLeft;

            this.FillInternal(pointsLast, this.lastHandData, hand);

            if (GestureEstimationRequested != null)
            {
                bool wasInterpolated = true;

                GestureEstimationRequested(handData, wasInterpolated);
            }
        }

        private void UpdateRotation()
        {
            imageRotation = TofArManager.Instance.GetScreenOrientation();
        }

        private void OnScreenRotationChanged(ScreenOrientation previousOrientation, ScreenOrientation newOrientation)
        {
            UpdateRotation();
        }


        private void FillInternal(Vector3[] points, RecognizeResultProperty handData, HandStatus hand)
        {
            float[] poseLevels = hand == HandStatus.LeftHand ? handData.poseLevelsLeft : handData.poseLevelsRight;

            PoseIndex poseIndex = (PoseIndex)this.logic.GetMaximum(poseLevels);

            float scale = 1000;

            int nPoints = 14;

            int XTargetIndex = 0;
            int YTargetIndex = 1;
            float[] MultiXY = { 1000, 1000 }; // 0 -> X, 1-> Y

            switch (DeviceAngle)
            {
                case DeviceOrientation.LandscapeLeft: // 0
                    break;
                case DeviceOrientation.PortraitUpsideDown: // 270
                    XTargetIndex = 1;
                    YTargetIndex = 0;
                    MultiXY[0] *= -1f;
                    break;
                case DeviceOrientation.LandscapeRight: // 180
                    MultiXY[0] *= -1f;
                    MultiXY[1] *= -1f;
                    break;
                case DeviceOrientation.Portrait: // 90
                    XTargetIndex = 1;
                    YTargetIndex = 0;
                    MultiXY[1] *= -1f;
                    break;
                default:
                    break;
            }

            if (hand == HandStatus.LeftHand)
            {
                MultiXY[0] *= -1f;
            }
            if (TofArHandManager.Instance.RecogMode == RecogMode.Face2Face)
            {
                MultiXY[0] *= -1f;
                scale *= -1;
            }

            for (int i = 0; i < nPoints; i++)
            {
                Vector3 worldPoint = points[i];
                if (AdjustByAccelerationOnGestureEstimation)
                {
                    worldPoint = this.logic.RotateAccordingToAcceleration(worldPoint, points[(int)HandPointIndex.HandCenter], this.imageRotation);
                }
                this.Buffer[this.TopIndex].JointData[i * 3 + XTargetIndex] = worldPoint.x * MultiXY[XTargetIndex];
                this.Buffer[this.TopIndex].JointData[i * 3 + YTargetIndex] = worldPoint.y * MultiXY[YTargetIndex];
                this.Buffer[this.TopIndex].JointData[i * 3 + 2] = worldPoint.z * scale;
            }


            this.Buffer[this.TopIndex].JointData[nPoints * 3] = (float)poseIndex;

            this.Buffer[this.TopIndex].CopyFlag = false;

            ++this.TopIndex;
            if (this.TopIndex >= RingBufferSize)
            {
                this.TopIndex = 0;
            }
            ++this.DataCount;
            if (this.DataCount > RingBufferSize)
            {
                this.DataCount = RingBufferSize;
            }
        }
    }
}

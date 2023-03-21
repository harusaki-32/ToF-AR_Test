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
using SensCord;
using System;
using System.Linq;
using UnityEngine;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// Handデータクラス
    /// </summary>
    public class HandData : ChannelData
    {
        /// <summary>
        /// ポーズ認識レベルが最大値となっている PoseIndex を取得する
        /// </summary>
        /// <param name="left">左手のPoseIndex</param>
        /// <param name="right">右手のPoseIndex</param>
        public void GetPoseIndex(out PoseIndex left, out PoseIndex right)
        {
            left = right = PoseIndex.None;

            if (this.Data != null)
            {
                if (this.Data.handStatus == HandStatus.NoHand)
                {
                    return;
                }
                if (this.Data.poseLevelsLeft != null && (this.Data.handStatus == HandStatus.LeftHand || this.Data.handStatus == HandStatus.BothHands))
                {
                    float maxVal = this.Data.poseLevelsLeft.ToList().Max();

                    if (!float.IsNaN(maxVal))
                    {
                        int maxIdx = this.Data.poseLevelsLeft.ToList().IndexOf(maxVal);

                        if (maxIdx >= 0 && maxIdx < Enum.GetNames(typeof(PoseIndex)).Length - 1)
                        {
                            left = (PoseIndex)maxIdx;
                        }
                    }


                }

                if (this.Data.poseLevelsRight != null && (this.Data.handStatus == HandStatus.RightHand || this.Data.handStatus == HandStatus.BothHands))
                {
                    float maxVal = this.Data.poseLevelsRight.ToList().Max();

                    if (!float.IsNaN(maxVal))
                    {
                        int maxIdx = this.Data.poseLevelsRight.ToList().IndexOf(maxVal);

                        if (maxIdx >= 0 && maxIdx < Enum.GetNames(typeof(PoseIndex)).Length - 1)
                        {
                            right = (PoseIndex)maxIdx;
                        }
                    }

                }

            }
        }

        /// <summary>
        /// HAND情報データ
        /// </summary>
        public RecognizeResultProperty Data { get; protected internal set; }


        const int featurePointsLength = 25;
        const int poseLevelsLength = 15;
        const int handDataSize = 4 + featurePointsLength * 12 * 2 + poseLevelsLength * 4 * 2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rawData">Rawデータ</param>
        public HandData(RawData rawData) : base(rawData)
        {
            this.Data = new RecognizeResultProperty();
            if (rawData.Length < handDataSize)
            {
                return;
            }
            var data = rawData.ToArray();

            var offset = 0;
            var hand = BitConverter.ToInt32(data, 0);
            offset += 4;

            var featurePointsLeft = new Vector3[featurePointsLength];
            var featurePointsRight = new Vector3[featurePointsLength];

            int processTime = 0;

            bool leftHandRecognized = false;
            bool rightHandRecognized = false;

            for (var i = 0; i < featurePointsLength; i++)
            {
                var x = BitConverter.ToSingle(data, offset) / 1000;
                var y = BitConverter.ToSingle(data, offset + 4) / 1000;
                var z = BitConverter.ToSingle(data, offset + 8) / 1000;

                featurePointsLeft[i] = new Vector3(-y, x, z);

                if (z > 0)
                {
                    leftHandRecognized = true;
                }
                offset += 12;
            }


            for (var i = 0; i < featurePointsLength; i++)
            {
                var x = BitConverter.ToSingle(data, offset) / 1000;
                var y = BitConverter.ToSingle(data, offset + 4) / 1000;
                var z = BitConverter.ToSingle(data, offset + 8) / 1000;

                featurePointsRight[i] = new Vector3(-y, x, z);

                if (z > 0)
                {
                    rightHandRecognized = true;
                }
                offset += 12;
            }

            if (!leftHandRecognized && !rightHandRecognized)
            {
                hand = (int)HandStatus.NoHand;
            }

            float[] poseAccuraciesLeft = new float[poseLevelsLength], poseAccuraciesRight = new float[poseLevelsLength];

            for (var i = 0; i < poseLevelsLength; i++)
            {
                var x = BitConverter.ToSingle(data, offset);

                poseAccuraciesLeft[i] = x;

                offset += 4;
            }

            for (var i = 0; i < poseLevelsLength; i++)
            {
                var x = BitConverter.ToSingle(data, offset);

                poseAccuraciesRight[i] = x;

                offset += 4;

            }

            processTime = BitConverter.ToInt32(data, offset);


            Data.featurePointsLeft = featurePointsLeft;
            Data.featurePointsRight = featurePointsRight;
            Data.handStatus = (HandStatus)hand;
            Data.poseLevelsRight = poseAccuraciesRight;
            Data.poseLevelsLeft = poseAccuraciesLeft;
            Data.processTime = processTime;

        }
    }
}

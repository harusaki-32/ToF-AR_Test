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
using TensorFlowLite.Runtime;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// TODO+ C　内部処理クラス
    /// </summary>
    public class TFLiteEstimator
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public string tfliteFile;

        TFLiteRuntime tflite;

        /// <summary>
        /// TODO+ C
        /// </summary>
        public int DataNum;
        float[] inputData;
        float[] outputData;

        // Use this for initialization
        const int jointNum = 14;
        const int jointNumScalars = jointNum * 3; //42
        const int dataNum = jointNumScalars + 1 + 9;

        /// <summary>
        /// TODO+ C
        /// </summary>
        /// <param name="tFLiteFile">TODO+ C</param>
        public TFLiteEstimator(string tFLiteFile)
        {
            tfliteFile = tFLiteFile;

            tflite = new TFLiteRuntime(tfliteFile, TFLiteRuntime.ExecMode.EXEC_MODE_CPU);

            float[] input = tflite.getInputBuffer()[0];

            // Use a single layer only network
            DataNum = input.Length;

        }

        ~TFLiteEstimator()
        {
            if (this.tflite != null)
            {
                this.Free();
            }
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        /// <param name="positions">TODO+ C</param>
        /// <param name="rB_index">TODO+ C</param>
        /// <param name="frameNum">TODO+ C</param>
        /// <param name="lastIndex">TODO+ C</param>
        /// <param name="framesPerSec">TODO+ C</param>
        /// <returns>TODO+ C</returns>
        public float[] Forward(FrameData[] positions, int rB_index, int frameNum, int lastIndex, int framesPerSec = 30)
        {
            GestureFrameData frameData = new GestureFrameData(framesPerSec);

            inputData = tflite.getInputBuffer()[0];
            if (inputData.Length != frameNum * dataNum)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "input data num does not match");
                return null;
            }

            int dataSet = 0;
            List<Pose> poseList = frameData.getPoseList(dataSet, frameNum, positions, rB_index, lastIndex);
            if (poseList.Count != frameNum)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "poseList length does not match to FrameNum:" + poseList.Count);
                return null;
            }



            for (int i = 0; i < poseList.Count; ++i)
            {
                poseList[i].Normalize(); // normalize hand pose. need to normalize separately

                int offset = i * dataNum;
                for (int j = 0; j < jointNum; ++j)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        inputData[offset + j * 3 + k] = poseList[i].Xyz[j, k];
                    }
                }
                inputData[offset + jointNumScalars] = poseList[i].PoseIndex;

                for (int k = 0; k < 3; k++)
                {
                    inputData[offset + jointNumScalars + 1 + k] = poseList[i].Cxyz[k];
                }

                for (int k = 0; k < 3; k++)
                {
                    inputData[offset + jointNumScalars + 4 + k] = poseList[i].abc[k];
                }

                for (int k = 0; k < 3; k++)
                {
                    inputData[offset + jointNumScalars + 7 + k] = poseList[i].P14[k];
                }
            }

            // Use a single layer only network

            outputData = tflite.forward()[0];

            int outputSize = outputData.Length;
            float[] outputArray = new float[outputSize];

            float maxvalue = float.MinValue;

            for (int i = 0; i < outputSize; ++i)
            {
                outputArray[i] = outputData[i];
                if (outputArray[i] > maxvalue)
                {
                    maxvalue = outputArray[i];
                }
            }
            return outputArray;
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        public void Free()
        {
            UnityEngine.Debug.Log("TFLiteEstimator Free()");
            tflite.Dispose();
            tflite = null;
        }
    }

    /// <summary>
    /// TODO+ C 基本的に内部処理？　一般ユーザーが直接触れることはなさそう？
    /// </summary>
    public struct FrameData
    {
        public float[] JointData;
        public bool CopyFlag;
    }

}

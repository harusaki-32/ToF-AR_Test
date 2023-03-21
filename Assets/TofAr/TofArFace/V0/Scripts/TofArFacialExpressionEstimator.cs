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
using System.Collections.Generic;
using System.IO;
using TensorFlowLite.Runtime;
using UnityEngine;

namespace TofAr.V0.Face
{
    /// <summary>
    /// 「あいうえお」推定で使用される設定
    /// </summary>
    [Serializable]
    public struct AIUEOEntry
    {
        /// <summary>
        /// 「あいうえお」推定モード
        /// </summary>
        public RuleBaseAIUEO.Mode mode;

        /// <summary>
        /// 「あいうえお」の口形状
        /// </summary>
        public AIUEOData vectors;
    }

    /// <summary>
    /// 表情の定数
    /// </summary>
    public enum FacialExpression
    {
        /// <summary>
        /// 「あ」
        /// </summary>
        Japanese_A,

        /// <summary>
        /// 「い」
        /// </summary>
        Japanese_I,

        /// <summary>
        /// 「う」
        /// </summary>
        Japanese_U,

        /// <summary>
        /// 「え」
        /// </summary>
        Japanese_E,

        /// <summary>
        /// 「お」
        /// </summary>
        Japanese_O,

        /// <summary>
        /// 「目」
        /// </summary>
        EyeOpen,

        /// <summary>
        /// 「眉毛」
        /// </summary>
        BrowUp
    }

    /// <summary>
    /// 表情推定
    /// </summary>
    public class TofArFacialExpressionEstimator : Singleton<TofArFacialExpressionEstimator>
    {
        /// <summary>
        /// 表情推定結果をコールバック
        /// </summary>
        /// <param name="result">ジェスチャー認識の結果</param>
        public delegate void FacialExpressionEstimatedEventHandler(float[] result);

        /// <summary>
        /// 表情推定結果通知
        /// </summary>  
        public static event FacialExpressionEstimatedEventHandler OnFacialExpressionEstimated;

        /// <summary>
        /// aiueo認識で使用するTFLiteのExecMode
        /// </summary>      
        [Obsolete("ExecMode is obsolete and will be removed in a future version")]
        public TFLiteRuntime.ExecMode ExecMode { get; set; }

        /// <summary>
        /// aiueo認識で使用するスレッド数
        /// </summary>       
        [Obsolete("ThreadsNum is obsolete and will be removed in a future version")]
        public int ThreadsNum { get; set; }

        private float[][] output;

        private const int facialExpressionsCount = 7;
        // result 
        private float[] facialExpressionsResult = new float[facialExpressionsCount];

        // internal variables
        private float openedEye = 0.009f;
        private float closedEye = 0.006f;

        // internal variables
        private float upBrow = 0.0095f;
        private float downBrow = 0.009f;

        private bool isEstimating = false;

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的に表情推定を開始する
        /// </summary>
        public bool autoStart = true;

        private RuleBaseAIUEO aiueoEstimator = new RuleBaseAIUEO();

        /// <summary>
        /// 表情推定で使用されるデータ
        /// </summary>
        public AIUEOData[] aiueoDataList;
        private List<int[]> mouthIndices = new List<int[]>();

        [SerializeField]
        private bool autoModeSelect = true;
        [HideInInspector]
        [SerializeField]
        private int aiueoDataListIndex;
        private int lastAiueoDataListIndex;


        private RuleBaseAIUEO.Mode currentMode = RuleBaseAIUEO.Mode.ArKit;
        private RuleBaseAIUEO.Mode lastMode;

        /// <summary>
        ///  表情推定する顔のインデックス
        /// </summary>
        public int FacialExpressionDetectionFaceIndex { get; set; } = 0;

        private ulong lastFaceTimestamp;

        private float[] coeffs_a = new float[52] { 0f, 0f, 0.716984335f, 0.045410029f, 0.134937582f, 0.122535494f, 0f, 0f, 0.023791297f, 0.431265206f, 0f, 0.1156161f, 0.160550634f, 0f, 0f, 0.724687291f, 0f, 0.020593494f, 0.141638727f, 0.811743835f, 0f, 0.172078241f, 0.024293945f, 0.168941946f, 0f, 0.007416498f, 0f, 0f, 0f, 0.010384178f, 0.468596056f, 2.06893E-05f, 0.068599427f, 0.069868848f, 0.156436477f, 0f, 0f, 0.285347993f, 0f, 0.115082638f, 0.034222891f, 0.126840573f, 0f, 0.063606792f, 3.86346E-05f, 0f, 0f, 0.288194059f, 0.068342676f, 0f, 0f, 0f };
        private float[] coeffs_i = new float[52] { 0f, 0f, 0.659665259f, 0.020108827f, 0.421334228f, 0.297926192f, 0f, 0f, 0.123297209f, 0.238376694f, 0f, 0.037989803f, 0.412464655f, 0f, 0f, 0.65921616f, 0f, 0.061795684f, 0.438117302f, 0.125754547f, 0f, 0.219568284f, 0.136323753f, 0.408254609f, 0.001771017f, 0.031802607f, 0f, 0f, 0f, 0.014505993f, 0.253996476f, 0.000250742f, 0.042541555f, 0.38816243f, 0.338737572f, 0f, 0f, 0.98785089f, 0f, 0.008080422f, 0.016045604f, 0.307503717f, 0f, 0.010746919f, 0f, 0f, 0f, 0.972329437f, 0.413366192f, 0f, 0f, 0f };
        private float[] coeffs_u = new float[52] { 0f, 0f, 0.155650435f, 0.025036558f, 0.063078221f, 0.054428662f, 0f, 0f, 0.057574672f, 0.170967738f, 0f, 0.525191756f, 0.184090836f, 0f, 0f, 0.166240329f, 0f, 0.003747651f, 0.070433359f, 0.144776355f, 0f, 0.035672136f, 0.064045433f, 0.18283445f, 0f, 0.072420456f, 0f, 0f, 0.013850078f, 0.07108368f, 0.178563725f, 7.98117E-05f, 0.05952798f, 0.112394751f, 0.190033031f, 0f, 0f, 0.044936792f, 0f, 0.143451385f, 0.026298354f, 0.058113106f, 0.016454091f, 0.524185252f, 0.007798679f, 0f, 0f, 0.032807372f, 0.103850152f, 0f, 0f, 0f };
        private float[] coeffs_e = new float[52] { 0f, 0f, 0.590644492f, 0.088076307f, 0.266664269f, 0.231685039f, 0f, 0f, 0.067174385f, 0.467079021f, 0f, 0.354507761f, 0.385064009f, 0f, 0f, 0.604381795f, 0f, 0f, 0.332729522f, 0.117613971f, 0f, 0.310808624f, 0.052554938f, 0.515842784f, 0.024084369f, 0.039622373f, 0f, 0f, 0f, 0.037032875f, 0.456966785f, 0.000534625f, 0.046963859f, 0.627019484f, 0.468208555f, 0f, 0f, 0.502760267f, 0f, 0.086952542f, 0.003171539f, 0.280777853f, 0f, 0.053495587f, 0.030498652f, 0f, 0f, 0.481026234f, 0.550279917f, 0f, 0f, 0f };
        private float[] coeffs_o = new float[52] { 0f, 0f, 0.331421792f, 0.201694139f, 0.087454983f, 0.074916079f, 0f, 0f, 0.012075249f, 0.417560035f, 0f, 0.592829657f, 0.08127094f, 0f, 0f, 0.325128609f, 0f, 0f, 0.105545316f, 0.672644565f, 0f, 0.120194291f, 0.009800821f, 0.109707901f, 0.029621738f, 0.017056767f, 0f, 0f, 0.209186312f, 0.057833044f, 0.416595213f, 0.007259792f, 0.110363696f, 0.100958042f, 0.20470028f, 0f, 0f, 0f, 0f, 0.59909849f, 0f, 0.103092384f, 0.24022439f, 0.443989135f, 0.029227848f, 0f, 0f, 0f, 0.085702503f, 0f, 0f, 0f };

        private MouthShape mouthShape = new MouthShape();
        private MouthShape mouthShapeNeutral = new MouthShape();
        /// <summary>
        /// リセット時に基準の口形状を取得するフレーム数
        /// </summary>
        private const int neutralMouthNum = 3;
        private int resetCount = 0;
        private MouthShape[] nMouthShapes = new MouthShape[neutralMouthNum];

        private void Start()
        {
            if (output == null)
            {
                output = new float[1][];
                output[0] = new float[5];
            }

            foreach (var entry in aiueoDataList)
            {
                Vector2 [] vectors = new Vector2[5];
                vectors[0] = entry.vectorA;
                vectors[1] = entry.vectorI;
                vectors[2] = entry.vectorU;
                vectors[3] = entry.vectorE;
                vectors[4] = entry.vectorO;
                aiueoEstimator.aiueoVectorList.Add(vectors);

                int[] indices = new int[4];
                indices[0] = entry.mouthIndexTop;
                indices[1] = entry.mouthIndexBottom;
                indices[2] = entry.mouthIndexRight;
                indices[3] = entry.mouthIndexLeft;
                mouthIndices.Add(indices);
            }

            lastMode = currentMode;
            if (autoModeSelect)
            {
                aiueoDataListIndex = Array.FindIndex(aiueoDataList, x => x.mode == currentMode);
            }
            if (autoStart)
            {
                StartGestureEstimation();
            }
        }

        /// <summary>
        /// ジェスチャー推定を開始する
        /// </summary>  
        public void StartGestureEstimation()
        {
            this.lastFaceTimestamp = 0;
            this.resetCount = 0;
            this.lastAiueoDataListIndex = -1;
            this.isEstimating = true;
            TofArFaceManager.OnFaceEstimated += TofArFaceManager_OnFaceEstimated;
        }

        /// <summary>
        /// ジェスチャー推定を終了する
        /// </summary>  
        public void StopGestureEstimation()
        {
            TofArFaceManager.OnFaceEstimated -= TofArFaceManager_OnFaceEstimated;
            this.isEstimating = false;
        }

        private void TofArFaceManager_OnFaceEstimated(FaceResults faceResults)
        {
            if (faceResults.results.Length > FacialExpressionDetectionFaceIndex)
            {
                var faceResult = faceResults.results[FacialExpressionDetectionFaceIndex];

                // don't estimate expressions of frame with same timestamp as the last one
                if (lastFaceTimestamp == faceResult.timestamp)
                {
                    return;
                }

                TofArFacialExpressionEstimator.Instance.EstimateExpressions(faceResult);
            }
        }

        /// <summary>
        /// 表情推定のため顔をキャリブレーションする
        /// </summary>
        public void CalibrateFace()
        {
            this.resetCount = 0;
        }

        internal void EstimateExpressions(FaceResult face)
        {
            const int arcoreFaceLength = 478;
            bool usingARKitFace = face.vertices.Length > arcoreFaceLength;

            if (autoModeSelect)
            {
                currentMode = usingARKitFace ? RuleBaseAIUEO.Mode.ArKit : RuleBaseAIUEO.Mode.TFFace;
                if (lastMode != currentMode)
                {
                    lastMode = currentMode;
                    lastAiueoDataListIndex = -1;
                    aiueoDataListIndex = Array.FindIndex(aiueoDataList, x => x.mode == currentMode);
                }
            }
            else
            {
                if (aiueoDataListIndex < 0 || aiueoDataListIndex >= aiueoDataList.Length)
                {
                    return;
                }
                currentMode = aiueoDataList[aiueoDataListIndex].mode;
            }
            if (lastAiueoDataListIndex != aiueoDataListIndex)
            {
                lastAiueoDataListIndex = aiueoDataListIndex;
                if (aiueoDataListIndex >= 0 && aiueoDataListIndex < aiueoEstimator.aiueoVectorList.Count)
                {
                    string msg = $"Current index {aiueoDataListIndex} with mode {currentMode} selected {(autoModeSelect ? "automatically" : "manually")}";
                    TofArManager.Logger.WriteLog(LogLevel.Debug, msg);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    for (int i = 0; i < aiueoEstimator.aiueoVectorList[aiueoDataListIndex].Length; i++)
                    {
                        Vector3 vec = aiueoEstimator.aiueoVectorList[aiueoDataListIndex][i];
                        sb.Append(String.Format("Vector {0}: {1:0.000}, {2:0.000}, {3:0.000}\n", (FacialExpression)i, vec.x, vec.y, vec.z));
                    }
                    TofArManager.Logger.WriteLog(LogLevel.Debug, sb.ToString());
                    sb.Clear();
                    for (int i = 0; i < mouthIndices[aiueoDataListIndex].Length; i++)
                    {
                        int index = mouthIndices[aiueoDataListIndex][i];
                        sb.Append($"Mouth index {i}: {index}\n");
                    }
                    TofArManager.Logger.WriteLog(LogLevel.Debug, sb.ToString());
                } 
                else
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "AIUEO Data List Index is out of bounds");
                }
            }

            FaceLogic.EstimateExpressionsInternal(ref face, ref isEstimating, ref facialExpressionsResult,
                ref openedEye, ref closedEye, ref upBrow, ref downBrow,
                ref OnFacialExpressionEstimated, ref lastFaceTimestamp, PrepareInput, OutputFunc);
        }

        /// <summary>
        /// ARKitに投影した表情データを取得する
        /// </summary>
        /// <param name="face"></param>
        public void GetMappedBlendshapes(ref FaceResult face)
        {
            FaceLogic.GetMappedBlendshapesInternal(ref face, ref isEstimating, ref facialExpressionsResult,
                ref openedEye, ref closedEye, ref upBrow, ref downBrow, ref coeffs_a, ref coeffs_i, ref coeffs_u, ref coeffs_e, ref coeffs_o,
                ref OnFacialExpressionEstimated, ref lastFaceTimestamp, PrepareInput, OutputFunc);
        }

        private void PrepareInput(FaceResult face)
        {
            if (aiueoDataListIndex < 0 || aiueoDataListIndex >= mouthIndices.Count)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "AIUEO Data List Index is out of bounds");
                return;
            }
            int[] mouthIndices_ = mouthIndices[aiueoDataListIndex];
            mouthShape.Set(face, mouthIndices_);

            // Init処理 (基準の口形状を保持する)
            if (resetCount < neutralMouthNum)
            {
                nMouthShapes[resetCount] = mouthShape;
                resetCount++;

                if (resetCount == neutralMouthNum)
                {
                    // 平均を算出して更新する.(1回だけ)
                    float sumH = 0.0f;
                    float sumV = 0.0f;
                    for (int i = 0; i < nMouthShapes.Length; i++)
                    {
                        sumH += nMouthShapes[i].Horizontal;
                        sumV += nMouthShapes[i].Vertical;
                    }

                    float h = sumH / (float)neutralMouthNum;
                    float v = sumV / (float)neutralMouthNum;
                    mouthShapeNeutral.Set(h, v);
                }
            }
        }

        private float[][] OutputFunc()
        {
            aiueoEstimator.Run(mouthShape, mouthShapeNeutral, aiueoDataListIndex, output[0]);
            return output;
        }


        internal void EstimateExpressions(FaceResults faceResults)
        {
            if (!isEstimating)
            {
                return;
            }

            var faceData = faceResults;

            if (faceData == null)
            {
                return;
            }

            if (faceData.results.Length > FacialExpressionDetectionFaceIndex)
            {
                var face = faceData.results[FacialExpressionDetectionFaceIndex];

                EstimateExpressions(face);
            }
        }

    }
}

/*
 * Copyright 2022 Sony Semiconductor Solutions Corporation.
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

namespace TofAr.V0.Face
{
    /// <summary>
    /// 「あいうえお」推定ロジック
    /// </summary>
    public class RuleBaseAIUEO
    {
        /// <summary>
        /// 推定モード
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// ARKit
            /// </summary>
            ArKit,

            /// <summary>
            /// TFLite
            /// </summary>
            TFFace,

            /// <summary>
            /// その他
            /// </summary>
            Other
        }

        enum BlendShape
        {
            A = 0, // (int)TofAr.V0.Face.FacialExpression.Japanese_A
            I = 1, // (int)TofAr.V0.Face.FacialExpression.Japanese_I
            U = 2, // (int)TofAr.V0.Face.FacialExpression.Japanese_U
            E = 3, // (int)TofAr.V0.Face.FacialExpression.Japanese_E
            O = 4, // (int)TofAr.V0.Face.FacialExpression.Japanese_O
        }

        static readonly int numBlendShapes = System.Enum.GetValues(typeof(BlendShape)).Length;

        /// <summary>
        /// BlendShape最小値
        /// </summary>
        private const float blendShapeValueMin = 0.0f;

        /// <summary>
        /// BlendShape最大値
        /// </summary>
        private const float blendShapeValueMax = 1.0f;

        /// <summary>
        /// AIUEOのBlendShapeを算出する.
        /// </summary>
        /// <param name="mouthShape">現在の口形状</param>
        /// <param name="mouthShapeNeutral">定常時の口形状(ん-Nの口を想定)</param>
        /// <param name="output">BlendShape値の書き込み先</param>
        internal void Run(MouthShape mouthShape, MouthShape mouthShapeNeutral, int idx, float[] output)
        {
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = blendShapeValueMin;
            }

            if (output.Length != numBlendShapes)
            {
                Debug.LogWarning("BlendShape array length invalid.");
                return;
            }

            if (mouthShape.IsInvalid || mouthShapeNeutral.IsInvalid)
            {
                Debug.LogWarning("invalid mouth shape.");
                return;
            }

            // 定常時(N)の口形状で正規化.
            float normHorizontal = mouthShape.Horizontal / mouthShapeNeutral.Horizontal;
            float normVertical = mouthShape.Vertical / mouthShapeNeutral.Vertical;

            Vector2[] aiueoVector = GetAiueoVector(idx);
            if (aiueoVector != null)
            {
                CalculateBlendShapes(normHorizontal, normVertical, aiueoVector, output);
            }
        }

        // Logic
        // Nの口で正規化されたAIUEOの口形状をパラメータとして保持しておく.
        // N(1, 1)からAIUEOの各形状を繋いだベクトルのうち近い2つのベクトルを使って現在の口形状のベクトルを表す.
        // ベクトルにかかる係数を使ってブレンドシェイプ値を算出する.
        // horizontal <-> x, vertical <-> y

        /// <summary>
        /// 正規化後のN（ん）の口形状（理論値 1,1）
        /// </summary>
        private readonly Vector2 normNTheoretical = Vector2.one;

        internal List<Vector2[]> aiueoVectorList = new List<Vector2[]>();

        private Vector2[] GetAiueoVector(int idx)
        {
            if (idx < 0 || idx >= aiueoVectorList.Count)
            {
                return null;
            }

            Vector2[] aiueoVector = aiueoVectorList[idx];
            return aiueoVector;   
        }

        /// <summary>
        /// BlendShape値の計算を行う閾値.Nからの変化ベクトルの長さ(magnitude).
        /// </summary>
        private const float nThreshold = 0.05f;

        /// <summary>
        /// BlendShape値を算出する.
        /// </summary>
        /// <param name="normVertical"></param>
        /// <param name="normHorizontal"></param>
        /// <param name="output"></param>
        private void CalculateBlendShapes(float normHorizontal, float normVertical, Vector2[] aiueoVector, float[] output)
        {
            var vecNX = new Vector2(normHorizontal, normVertical) - normNTheoretical;

            if (vecNX.magnitude <= nThreshold)
            {
                // 閾値を超えない限りAIUEO制御をしない.
                return;
            }

            var nearShapes = GetNearShapes(vecNX, aiueoVector);

            int negIdx = (int)IndexNearShapes.Negative;
            int posIdx = (int)IndexNearShapes.Positive;

            if (nearShapes[negIdx] == nearShapes[posIdx])
            {
                // 現在の口の形をAIUEOいずれか1種類のベクトルで表現する.
                var shape = nearShapes[negIdx];
                float value = vecNX.y / aiueoVector[(int)shape].y;

                output[(int)shape] = CeilFloorFloat(value, blendShapeValueMin, blendShapeValueMax);
            }
            else
            {
                // 現在の口の形をAIUEOいずれか2種類のベクトルで表現する.
                // vecNX = a * vecNeg + b * vecPos
                var vecNeg = aiueoVector[(int)nearShapes[negIdx]];
                var vecPos = aiueoVector[(int)nearShapes[posIdx]];
                var coeff = ConvertVectorCoordinates(vecNX, vecNeg, vecPos);

                // 係数に基づいてBlendShape値を決定する.
                float a = CeilFloat(coeff.x, blendShapeValueMin); // a >= 0.0f
                float b = CeilFloat(coeff.y, blendShapeValueMin); // b >= 0.0f

                // 最大でも a+b=1 となるように調整する.
                // 大きな口の時、この処理に入る.
                if (a + b > blendShapeValueMax)
                {
                    if (nearShapes[negIdx] == BlendShape.A)
                    {
                        // Aのパラメータはなるべくそのまま使用する.
                        a = FloorFloat(a, blendShapeValueMax);
                        b = b / (a + b);
                    }
                    else if (nearShapes[posIdx] == BlendShape.A)
                    {
                        // Aのパラメータはなるべくそのまま使用する.
                        b = FloorFloat(b, blendShapeValueMax);
                        a = a / (a + b);
                    }
                    else
                    {
                        a = a / (a + b);
                        b = b / (a + b);
                    }
                }

                output[(int)nearShapes[negIdx]] = a;
                output[(int)nearShapes[posIdx]] = b;
            }
        }

        private float[] angles = new float[numBlendShapes];

        private enum IndexNearShapes
        {
            Negative = 0,
            Positive = 1,
        }

        private BlendShape[] nearShapes = new BlendShape[2];

        /// <summary>
        /// ベクトル同士の角度から、近いAIUEOベクトル2つを選出.
        /// </summary>
        /// <param name="vecNX"></param>
        /// <returns>BlendShape[(int)IndexNearShapes.XXX]</returns>
        private BlendShape[] GetNearShapes(Vector2 vecNX, Vector2[] aiueoVector)
        {
            for (int i = 0; i < numBlendShapes; i++)
            {
                angles[i] = Vector2.SignedAngle(vecNX, aiueoVector[i]);
            }

            float[] nearAngles = { float.MinValue, float.MaxValue };
            int negIdx = (int)IndexNearShapes.Negative;
            int posIdx = (int)IndexNearShapes.Positive;
            for (int i = 0; i < numBlendShapes; i++)
            {
                float angle = angles[i];
                if (angle < 0)
                {
                    if (angle > nearAngles[negIdx])
                    {
                        nearShapes[negIdx] = (BlendShape)i;
                        nearAngles[negIdx] = angle;
                    }
                }
                else if (angle > 0)
                {
                    if (angle < nearAngles[posIdx])
                    {
                        nearShapes[posIdx] = (BlendShape)i;
                        nearAngles[posIdx] = angle;
                    }
                }
                else // angle == 0
                {
                    nearShapes[negIdx] = (BlendShape)i;
                    nearAngles[negIdx] = angle;
                    nearShapes[posIdx] = (BlendShape)i;
                    nearAngles[posIdx] = angle;
                }
            }

            // AIUEOベクトルとの角度が全て正か全て負の場合(AIUEOベクトルに挟まれていない場合).
            if (nearAngles[negIdx] == float.MinValue)
            {
                nearShapes[negIdx] = nearShapes[posIdx];
            }
            else if (nearAngles[posIdx] == float.MaxValue)
            {
                nearShapes[posIdx] = nearShapes[negIdx];
            }

            return nearShapes;
        }

        // Math

        /// <summary>
        /// 1つのベクトルを2つのベクトルで表現する.
        /// vecP = a * vecA + b * vecB
        /// </summary>
        /// <param name="vecP"></param>
        /// <param name="vecA"></param>
        /// <param name="vecB"></param>
        /// <returns>(x, y) <=> (a, b)</returns>
        private Vector2 ConvertVectorCoordinates(Vector2 vecP, Vector2 vecA, Vector2 vecB)
        {
            // vecP = a * vecA + b * vecB
            float a = (vecP.x * vecB.y - vecB.x * vecP.y) / (vecA.x * vecB.y - vecB.x * vecA.y);
            float b = (vecP.x * vecA.y - vecA.x * vecP.y) / (vecB.x * vecA.y - vecA.x * vecB.y);
            return new Vector2(a, b);
        }

        /// <summary>
        /// valueがthresholdよりも小さい場合切り上げを行う.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private float CeilFloat(float value, float threshold)
        {
            if (value < threshold)
            {
                value = threshold;
            }

            return value;
        }

        /// <summary>
        /// valueがthresholdよりも大きい場合切り捨てを行う.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private float FloorFloat(float value, float threshold)
        {
            if (value > threshold)
            {
                value = threshold;
            }

            return value;
        }

        /// <summary>
        /// valueが範囲外の場合切り捨てまたは切り上げを行い、範囲内に収める.
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="lowThreshold">最小値</param>
        /// <param name="highThreshold">最大値</param>
        /// <returns></returns>
        private float CeilFloorFloat(float value, float lowThreshold, float highThreshold)
        {
            return FloorFloat(CeilFloat(value, lowThreshold), highThreshold);
        }
    }
}

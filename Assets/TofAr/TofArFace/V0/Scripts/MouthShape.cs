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
using UnityEngine;

namespace TofAr.V0.Face
{
    internal class MouthShape
    {
        internal float Horizontal { get; private set; }
        internal float Vertical { get; private set; }
        internal bool IsInvalid { get; private set; } = false;

        private const int lipPointNum = 4; // top, bottom, right, left

        private Vector3[] lipPoints = new Vector3[lipPointNum];

        /// <summary>
        /// MouthShapeの値を更新する.
        /// </summary>
        /// <param name="horizontal">口の横方向の長さ[m]</param>
        /// <param name="vertical">口の横方向の長さ[m]</param>
        internal void Set(float horizontal, float vertical)
        {
            IsInvalid = false;
            Horizontal = horizontal;
            Vertical = vertical;
        }

        /// <summary>
        /// MouthShapeの値を更新する.
        /// </summary>
        /// <param name="faceData">顔認識情報</param>
        /// <param name="mouthIndices">特徴点のIndex情報(top, bottom, right, left)</param>
        internal void Set(FaceResult faceData, int[] mouthIndices)
        {
            if (faceData == null)
            {
                IsInvalid = true;
                return;
            }
            if (mouthIndices == null || mouthIndices.Length != lipPointNum)
            {
                IsInvalid = true;
                return;
            }

            for (int i = 0; i < mouthIndices.Length; i++)
            {
                int mouthIdx = mouthIndices[i];
                if (mouthIdx >= faceData.vertices.Length) {
                    continue;
                }
                lipPoints[i] = faceData.vertices[mouthIdx].GetVector3();
            }

            Set(lipPoints);
        }

        /// <summary>
        /// MouthShapeの値を更新する.
        /// </summary>
        /// <param name="points">4点の座標.(top, bottom, right, left)</param>
        internal void Set(Vector3[] points)
        {
            if (points == null || points.Length != lipPointNum)
            {
                IsInvalid = true;
                return;
            }

            IsInvalid = false;
            Horizontal = Vector3.Distance(points[2], points[3]);
            Vertical = Vector3.Distance(points[0], points[1]);
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsInvalid)
            {
                return "invalid result";
            }

            return string.Format(
                "H:{0:f4}, V: {1:f4}", Horizontal, Vertical);
        }
    }
}

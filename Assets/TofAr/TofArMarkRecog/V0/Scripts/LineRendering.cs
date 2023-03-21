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

namespace TofAr.V0.MarkRecog
{
    /// <summary>
    /// ラインのレンダー
    /// </summary>
    public class LineRendering : MonoBehaviour, IMarkRenderer
    {
        private LineRenderer Line;

        private const int initialLineCapacity = 60;
        private int currentPoint = 0;

        /// <summary>
        /// local空間を使用する場合はtrue、World空間の場合はfalse
        /// </summary>
        public bool useLocalSpace = false;

        private void Start()
        {
            Line = GetComponent<LineRenderer>();
            Line.positionCount = initialLineCapacity;
        }

        /// <summary>
        /// 描画を開始する
        /// </summary>
        public void StartDrawing()
        {
            if (isActiveAndEnabled)
            {
                //clears the old drawing
                currentPoint = 0;
                Line.positionCount = initialLineCapacity;
                Line.SetPositions(new Vector3[initialLineCapacity]);
            }
        }

        /// <summary>
        /// 描画を更新する
        /// </summary>
        /// <param name="newPoint">現在の描画位置</param>
        public void UpdateDrawing(Vector3 newPoint)
        {
            if (isActiveAndEnabled)
            {
                if (useLocalSpace)
                {
                    newPoint = this.transform.InverseTransformPoint(newPoint);
                }
                Line.SetPosition(currentPoint, newPoint);
                currentPoint++;
                //extend the capacity if we reach it
                if (currentPoint >= Line.positionCount)
                {
                    Line.positionCount = Line.positionCount + initialLineCapacity;

                }
                //set all the points past the end to the same value as the last point
                for (int i = currentPoint; i < Line.positionCount; i++)
                {
                    Line.SetPosition(i, newPoint);
                }
            }
        }

        /// <summary>
        /// 描画を停止する
        /// </summary>
        public void StopDrawing()
        {
        }
    }
}


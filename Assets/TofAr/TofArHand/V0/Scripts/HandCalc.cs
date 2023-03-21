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
using UnityEngine;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// 手の座標計算を行うクラス
    /// </summary>
    public class HandCalc
    {
        /// <summary>
        /// 手の座標計算処理終了時イベント
        /// </summary>
        /// <param name="points">手の関節座標配列</param>
        /// <param name="handStatus">手の認識状態</param>
        public delegate void OnHandPointsCalculatedEvent(Vector3[] points, HandStatus handStatus);
        /// <summary>
        /// 左手の座標計算処理終了通知
        /// </summary>
        public static event OnHandPointsCalculatedEvent OnLeftHandPointsCalculated;
        /// <summary>
        /// 右手の座標計算処理終了通知
        /// </summary>
        public static event OnHandPointsCalculatedEvent OnRightHandPointsCalculated;

        /// <summary>
        /// *TODO+ B
        /// 左手の関節の処理終了通知
        /// </summary>
        [Obsolete("OnHandPointsCalculatedLeft is deprecated, please use OnLeftHandPointsCalculated")]
        private event OnHandPointsCalculatedEvent OnHandPointsCalculatedLeft
        {
            add { OnLeftHandPointsCalculated += value; }
            remove { OnLeftHandPointsCalculated -= value; }
        }
        /// <summary>
        /// *TODO+ B
        /// 右手の関節の処理終了通知
        /// </summary>
        [Obsolete("OnHandPointsCalculatedRight is deprecated, please use OnRightHandPointsCalculated")]
        private event OnHandPointsCalculatedEvent OnHandPointsCalculatedRight
        {
            add { OnRightHandPointsCalculated += value; }
            remove { OnRightHandPointsCalculated -= value; }
        }

        /// <summary>
        /// 座標処理アクション
        /// </summary>
        public Func<Vector3[], Vector3[]> transformAction { get; set; }

        /// <summary>
        /// 手の座標計算処理を行う
        /// </summary>
        /// <param name="handData">手のデータ</param>
        public void ProcessHandPoints(HandData handData)
        {
            var manager = TofArHandManager.Instance;

            if (manager == null)
            {
                return;
            }
            RecognizeResultProperty data = null;

            if (handData != null)
            {
                data = handData.Data;
            }

            Vector3[] pointsLeft = null, pointsRight = null;
            HandStatus handStatus = HandStatus.NoHand;

            if (data != null)
            {
                pointsLeft = data.featurePointsLeft;
                pointsRight = data.featurePointsRight;

                handStatus = data.handStatus;
            }

            Vector3[] transformedPointsLeft = null, transformedPointsRight = null;

            if (handStatus != HandStatus.NoHand)
            {
                if (pointsLeft != null)
                {
                    if (manager.transformToColorSpace && transformAction != null)
                    {
                        transformedPointsLeft = transformAction.Invoke(pointsLeft);
                    }
                    else
                    {
                        transformedPointsLeft = pointsLeft;
                    }
                }

                if (pointsRight != null)
                {
                    if (manager.transformToColorSpace && transformAction != null)
                    {
                        transformedPointsRight = transformAction.Invoke(pointsRight);
                    }
                    else
                    {
                        transformedPointsRight = pointsRight;
                    }
                }
            }

            // invoke event
            if (OnLeftHandPointsCalculated != null)
            {
                OnLeftHandPointsCalculated.Invoke(transformedPointsLeft, handStatus);
            }
            // invoke event
            if (OnRightHandPointsCalculated != null)
            {
                OnRightHandPointsCalculated.Invoke(transformedPointsRight, handStatus);
            }
        }


    }
}

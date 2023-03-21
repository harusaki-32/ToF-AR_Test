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

namespace TofAr.V0.Body
{
    /// <summary>
    /// 基底ModelVisualizer
    /// </summary>
    public abstract class ModelVisualizer : MonoBehaviour
    {
        /// <summary>
        /// Body認識結果を表示する
        /// </summary>
        /// <param name="bodyResult">Body認識結果データ</param>
        public abstract void Apply(BodyResult bodyResult);
    }
}

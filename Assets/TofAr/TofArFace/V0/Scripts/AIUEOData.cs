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
using System;
using UnityEngine;

namespace TofAr.V0.Face
{
    /// <summary>
    /// 「あいうえお」推定データ
    /// </summary>
    [CreateAssetMenu(fileName = "AIUEOData", menuName = "TofAr/Face/AIUEOData")]
    public class AIUEOData : ScriptableObject
    {
        /// <summary>
        /// 「あいうえお」推定モード
        /// </summary>
        public RuleBaseAIUEO.Mode mode;

        /// <summary>
        /// 「あ」の口形状
        /// </summary>
        public Vector2 vectorA;

        /// <summary>
        /// 「い」の口形状
        /// </summary>
        public Vector2 vectorI;

        /// <summary>
        /// 「う」の口形状
        /// </summary>
        public Vector2 vectorU;

        /// <summary>
        /// 「え」の口形状
        /// </summary>
        public Vector2 vectorE;

        /// <summary>
        /// 「お」の口形状
        /// </summary>
        public Vector2 vectorO;

        /// <summary>
        /// 口の上部
        /// </summary>
        public int mouthIndexTop;

        /// <summary>
        /// 口の下部
        /// </summary>
        public int mouthIndexBottom;

        /// <summary>
        /// 口の右部
        /// </summary>
        public int mouthIndexRight;

        /// <summary>
        /// 口の左部
        /// </summary>
        public int mouthIndexLeft;
    }
}

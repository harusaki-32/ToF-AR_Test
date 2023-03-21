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

namespace TofAr.V0.Segmentation
{
    /// <summary>
    /// Segmentation認識処理インターフェース
    /// </summary>
    public interface ISegmentationDetector
    {
        /// <summary>
        /// アクティブ状態
        /// <para>true: アクティブである</para>
        /// <para>fase: アクティブではない</para>
        /// </summary>
        bool IsActive { get; set; }
    }
}

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

namespace TofAr.V0.Body.SV1
{
    /// <summary>
    /// Body認識モード(SV1BodyEstimatorで使用)
    /// </summary>
    [Obsolete("SV1 Body recognition will remove in future version")]
    public enum BodyShot : byte
    {
        /// <summary>
        /// 全身
        /// </summary>
        FullBody,

        /// <summary>
        /// 上半身
        /// </summary>
        UpperBody,
    };
}

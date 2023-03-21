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

namespace TofAr.V0.Hand
{
    /// <summary>
    /// HandModelインターフェイス
    /// </summary>
    public interface IHandModel
    {
        /// <summary>
        /// 位置
        /// </summary>
        Vector3[] HandPoints { get; }

        /// <summary>
        /// 空間位置
        /// </summary>
        Vector3[] WorldHandPoints { get; }
    }
}

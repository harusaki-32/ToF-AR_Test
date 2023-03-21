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

namespace TofAr.V0.Tof
{

    /// <summary>
    /// Tofデータ送出前の処理
    /// </summary>
    public interface IPreProcessTofData
    {
        /// <summary>
        /// 深度データ
        /// </summary>
        /// <param name="depthtex"></param>
        /// <returns></returns>
        short[] ParseDepthData(short[] depthtex);

        /// <summary>
        /// Confidenceデータ
        /// </summary>
        /// <param name="conftex"></param>
        /// <returns></returns>
        short[] ParseConfidenceData(short[] conftex);

        /// <summary>
        /// ポイントクラウド変換
        /// </summary>
        /// <param name="pointCloud">ポイントクラウドデータ</param>
        UnityEngine.Vector3[] ParsePointCloudData(UnityEngine.Vector3[] pointCloud);
    }
}

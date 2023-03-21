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
using System.Collections;
namespace TofAr.V0.Color
{
    /// <summary>
    /// 外部から入力されるColorストリーム
    /// </summary>
    public interface IExternalColorStream
    {
        /// <summary>
        /// ストリームを開始
        /// </summary>
        void StartStream();

        /// <summary>
        /// ストリームを開始
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitForStreamStart();

        /// <summary>
        /// ストリームを停止
        /// </summary>
        void StopStream();

        /// <summary>
        /// 遅延設定の取得
        /// </summary>
        /// <param name="cameraSettingsJson">Color延長の設定</param>
        int GetStreamDelay(TofArColorManager.deviceCameraSettingsJson cameraSettingsJson);
    }
}

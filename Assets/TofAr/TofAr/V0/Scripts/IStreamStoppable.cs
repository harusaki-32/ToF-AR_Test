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

namespace TofAr.V0
{
    /// <summary>
    /// ストリーム停止/復帰インターフェイス
    /// </summary>
    public interface IStreamStoppable : System.IDisposable, IStreamHolder
    {
        /// <summary>
        /// ストリーム停止
        /// </summary>
        void PauseStream();

        /// <summary>
        /// 復帰インターフェイス
        /// </summary>
        void UnpauseStream();

        /// <summary>
        /// 実測FPS
        /// </summary>
        float FrameRate { get; }
    }
}

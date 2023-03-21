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

namespace TofAr.V0
{
    /// <summary>
    /// 依存Managerインターフェイス
    /// </summary>
    public interface IDependManager
    {
        /// <summary>
        /// trueの場合ストリーミングを行っている
        /// </summary>
        bool IsStreamActive { get; }

        /// <summary>
        /// 依存するManagerから要求されたストリーミング再スタートを開始する
        /// </summary>
        /// <param name="requestSource">要求元</param>
        void RestartStreamByDependManager(object requestSource);

        /// <summary>
        /// 依存するManagerから要求されたストリーミング再スタート後処理
        /// </summary>
        /// <param name="requestSource">要求元</param>
        void FinalizeRestartStreamByDependManager(object requestSource);
    }
}

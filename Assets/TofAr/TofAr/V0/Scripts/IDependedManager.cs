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
    /// 被依存Managerインターフェイス
    /// </summary>
    public interface IDependedManager
    {
        /// <summary>
        /// 依存Managerを追加する
        /// </summary>
        /// <param name="dependManager">依存Manager</param>
        void AddManagerDependency(IDependManager dependManager);

        /// <summary>
        /// 依存Managerを削除する
        /// </summary>
        /// <param name="dependManager">依存Manager</param>
        void RemoveManagerDependency(IDependManager dependManager);
    }
}

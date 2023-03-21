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

namespace TofAr.V0.Color
{
    /// <summary>
    /// Colorデータ送出前の処理
    /// </summary>
    public interface IPreProcessColorData
    {
        /// <summary>
        /// Colorデータ変換
        /// </summary>
        /// <param name="colorData">Colorデータ</param>
        byte[] ParseColorData(byte[] colorData);
    }
}

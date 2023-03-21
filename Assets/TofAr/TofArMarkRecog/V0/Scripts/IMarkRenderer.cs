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

namespace TofAr.V0.MarkRecog
{
    /// <summary>
    /// HandBrush のマーク描画レンダラーインターフェイス
    /// <para>マーク描画を行うクラスはこのインターフェイスを実装する</para>
    /// </summary>
    public interface IMarkRenderer
    {
        /// <summary>
        /// 描画を開始する
        /// </summary>
        void StartDrawing();

        /// <summary>
        /// 描画を終了する
        /// </summary>
        void StopDrawing();

        /// <summary>
        /// 描画を更新する
        /// </summary>
        /// <param name="position">描画中のマークに追加する点の位置</param>
        void UpdateDrawing(Vector3 position);
    }
}

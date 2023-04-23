/*
 * Copyright 2018,2019,2020,2021,2022,2023 Sony Semiconductor Solutions Corporation.
 *
 * This is UNPUBLISHED PROPRIETARY SOURCE CODE of Sony Semiconductor
 * Solutions Corporation.
 * No part of this file may be copied, modified, sold, and distributed in any
 * form or by any means without prior explicit permission in writing from
 * Sony Semiconductor Solutions Corporation.
 *
 */

using UnityEngine;

namespace TofAr.V0.Color
{
    /// <summary>
    /// アスペクト比に合わせたサイズに設定する
    /// </summary>
    public class ColorQuadAspectFitter : QuadAspectFitter
    {
        /// <summary>
        /// オブジェクトが有効になったときに呼び出されます
        /// </summary>
        protected override void OnEnable()
        {
            TofArColorManager.OnStreamStarted += OnColorStreamStart;
            base.OnEnable();
            if (TofArColorManager.Instance.IsStreamActive)
            {
                UpdateAspect();
            }
        }

        /// <summary>
        /// オブジェクトが無効になったときに呼び出されます
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            TofArColorManager.OnStreamStarted -= OnColorStreamStart;
        }

        private void OnColorStreamStart(object sender, Texture2D colorTex)
        {
            UpdateAspect();
        }

        private void UpdateAspect()
        {
            var config = TofArColorManager.Instance.GetProperty<ResolutionProperty>();
            SetAspect(config.height, config.width);
        }
    }
}

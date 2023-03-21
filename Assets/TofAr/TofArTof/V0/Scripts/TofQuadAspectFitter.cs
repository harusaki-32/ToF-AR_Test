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

namespace TofAr.V0.Tof
{
    /// <summary>
    /// アスペクト比に合わせたサイズに設定する
    /// </summary>
    public class TofQuadAspectFitter : QuadAspectFitter
    {
        /// <summary>
        /// オブジェクトが有効になったときに呼び出されます
        /// </summary>
        protected override void OnEnable()
        {
            TofArTofManager.OnStreamStarted += OnTofStreamStart;
            base.OnEnable();
        }

        /// <summary>
        /// オブジェクトが無効になったときに呼び出されます
        /// </summary>
        protected override void OnDisable()
        {
            TofArTofManager.OnStreamStarted -= OnTofStreamStart;
            base.OnDisable();
        }

        private void OnTofStreamStart(object sender, Texture2D depthTex, Texture2D confTex, PointCloudData pcd)
        {
            var config = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
            this.SetAspect(config.height, config.width);
        }
    }
}

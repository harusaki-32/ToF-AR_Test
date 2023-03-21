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
    /// UnityのCameraにアタッチするとTofカメラの内部パラメータに基づいてプロジェクション調整を行う
    /// </summary>
    public class AdjustCameraFOV : MonoBehaviour
    {
        private void OnEnable()
        {
            TofArTofManager.Instance?.CalibrationSettingsLoaded.AddListener(GetIntrinsics);
        }

        private void OnDisable()
        {
            TofArTofManager.Instance?.CalibrationSettingsLoaded.RemoveListener(GetIntrinsics);
        }

        private void GetIntrinsics(CalibrationSettingsProperty calibration)
        {
            var intrinsics = calibration.d;

            var fy = intrinsics.fy;


            if (fy > 0)
            {
                var height = calibration.depthHeight;
                float ratio = (float)calibration.depthWidth / (float)calibration.depthHeight;
                var fov = 2 * Mathf.Atan2(0.5f * height, fy) * Mathf.Rad2Deg;

                var camera = GetComponent<Camera>();
                camera.fieldOfView = fov;
                camera.aspect = ratio;
            }
        }
    }


}

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

using TofAr.V0.Tof;
using UnityEngine;
using UnityEngine.Events;

namespace TofAr.V0.Coordinate
{
    /// <summary>
    /// DepthカメラとColorカメラのキャリブレーション設定値をロードする
    /// </summary>
    public class SettingsLoader : Singleton<SettingsLoader>
    {
        /// <summary>
        /// 設定値ロード完了時デリゲート（外部パラメータのみ）
        /// </summary>
        [System.Serializable]
        public class SettingsLoadedRTEventHandler : UnityEvent<float[], Vector3> { }

        /// <summary>
        /// 設定値ロード完了通知（外部パラメータのみ）。
        /// <para>このハンドラに Apply を設定することで設定値を適用できる。</para>
        /// </summary>
        public SettingsLoadedRTEventHandler SettingsLoaded;

        /// <summary>
        /// 設定値ロード完了時デリゲート（外部パラメータ及び内部パラメータ）
        /// </summary>
        [System.Serializable]
        public class SettingsLoadedEventHandler : UnityEvent<CalibrationSettingsProperty> { }

        /// <summary>
        /// 設定値ロード完了通知（外部パラメータ及び内部パラメータ）
        /// </summary>
        public SettingsLoadedEventHandler FullSettingsLoaded;

        private void OnEnable()
        {
            TofArTofManager.Instance.CalibrationSettingsLoaded.AddListener(LoadSettingsCallback);
        }

        private void OnDisable()
        {
            TofArTofManager.Instance.CalibrationSettingsLoaded.RemoveListener(LoadSettingsCallback);
        }

        private void LoadSettingsCallback(CalibrationSettingsProperty config)
        {

            if (SettingsLoaded != null)
            {
                float[] R = Tof.Matrix.MatrixToFloatArray(config.R);
                Vector3 t = new Vector3(config.T.x, config.T.y, config.T.z);
                SettingsLoaded.Invoke(R, t);
            }
            if (FullSettingsLoaded != null)
            {
                FullSettingsLoaded.Invoke(config);
            }
        }

        /// <summary>
        /// 設定値ロード完了時コールバック
        /// </summary>
        /// <param name="caller">呼び出し元オブジェクト</param>
        /// <param name="depthTexture">Depthテクスチャー</param>
        /// <param name="confidenceTexture">Confidenceテクスチャー</param>
        /// <param name="pointCloudData">ポイントクラウドデータ</param>
        public void LoadSettingsCallback(object caller, Texture2D depthTexture, Texture2D confidenceTexture, Tof.PointCloudData pointCloudData)
        {
            var config = TofArTofManager.Instance.CalibrationSettings;

            LoadSettingsCallback(config);
        }

        /// <summary>
        /// キャリブレーション設定をロードする
        /// </summary>
        /// <returns>キャリブレーション設定</returns>
        public CalibrationSettingsProperty LoadSettings()
        {
            var tofManager = TofArTofManager.Instance;
            var config = tofManager?.LoadSettings();
            return config;
        }
    }

}

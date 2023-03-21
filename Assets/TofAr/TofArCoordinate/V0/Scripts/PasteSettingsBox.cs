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
using UnityEngine.UI;

namespace TofAr.V0.Coordinate
{
    /// <summary>
    /// キャリブレーション設定表示パネル
    /// </summary>
    public class PasteSettingsBox : MonoBehaviour
    {
        /// <summary>
        /// キャリブレーション設定表示Text
        /// </summary>
        public Text calibrationInput;

        /// <summary>
        /// キャリブレーション設定表示パネル
        /// </summary>
        public GameObject calibSettingPanel;

        /// <summary>
        /// Settingsボタンクリック時処理
        /// </summary>
        public void SettingsButtonClicked()
        {
            this.calibSettingPanel.SetActive(!this.calibSettingPanel.activeInHierarchy);
        }

        private void Start()
        {
            var tofMan = TofAr.V0.Tof.TofArTofManager.Instance;
            if (tofMan != null)
            {
                tofMan.CalibrationSettingsLoaded.AddListener(LoadSettingsdelegate);
            }
        }
        void LoadSettingsdelegate(Tof.CalibrationSettingsProperty settings)
        {
            var settingsType = Tof.TofArTofManager.Instance.CalibrationSettingsStatus == Tof.TofArTofManager.CalibrationSettingsStatusType.LoadedWithRequestedColorResolution ?
                                string.Format("D({2},{3}) C({0},{1})", settings.colorWidth, settings.colorHeight, settings.depthWidth, settings.depthHeight) :
                                Tof.TofArTofManager.Instance.CalibrationSettingsStatus.ToString();
            calibrationInput.text = settings.ToString() + "\nApplied value: " + settingsType + "\nCalibrated: " + settings.isCalibrated;
        }

        /// <summary>
        /// 破棄時処理
        /// </summary>
        public void OnDestroy()
        {
            Tof.TofArTofManager.Instance?.CalibrationSettingsLoaded.RemoveListener(LoadSettingsdelegate);
        }
    }

}

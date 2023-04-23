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
using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace TofAr.V0.Tof.DepthPrivateFilter
{
    /// <summary>
    /// フィルター制御
    /// </summary>
    public class FilterController : MonoBehaviour
    {
        internal const string dataFileName = "data.zip";
        internal const string dataPath = "/data/local/tmp/tofar/depthfilter/data.zip";

        /// <summary>
        /// ソフトウェアID
        /// </summary>
        public int softwareId = 0x10401;

        /// <summary>
        /// フィルターの有効/無効
        /// </summary>
        public bool filterEnabled = true;

        /// <summary>
        /// HCG
        /// </summary>
        public bool hcg = false;

        /// <summary>
        /// エクスポージャー
        /// </summary>
        public long exposure = 0;

        /// <summary>
        /// 反転モード
        /// </summary>
        public TransformationMode transformationMode = TransformationMode.Default;

        /// <summary>
        /// キャリブレーションデータのパス
        /// </summary>
        [HideInInspector]
        public string calibrationDataPath = string.Empty;

        /// <summary>
        /// キャリブレーションデータのパス(サーバー)
        /// </summary>
        [HideInInspector]
        public string calibrationDataPathOnServer = "/storage/emulated/0/Android/data/jp.co.sonysemicon.tofar.server";

        /// <summary>
        /// 設定を適用時イベント
        /// </summary>
        public UnityEvent settingsApplied;

#if UNITY_EDITOR

        /// <summary>
        /// 圧縮データ
        /// </summary>
        [HideInInspector]
        public UnityEngine.Object dataZip;
#endif
        [System.Serializable]
        private class cameraSettings
        {
            public string customTagSupported = "";
        }

        [System.Serializable]
        private class deviceAttributesJson
        {
            public cameraSettings[] cameraSettings;
        }

        private void Awake()
        {


            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                this.calibrationDataPath = $"{Application.persistentDataPath}";
            }
            else
            {
                this.calibrationDataPath = this.calibrationDataPathOnServer;
            }
            this.ApplySettings();

            TofArTofManager.OnStreamStarted += (s, dt, ct, pd) =>
            {
                this.ApplySettings();
            };
        }

        private void OnEnable()
        {
            this.StartCoroutine(this.Process());
        }

        private void OnDisable()
        {
            this.StopAllCoroutines();
        }

        /// <summary>
        /// 適用設定
        /// </summary>
        public void ApplySettings()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"ApplySettings calibrationDataPath={this.calibrationDataPath} exposure={this.exposure} ransformationMode={this.transformationMode}");
            try
            {
                TofArTofManager.Instance.SetProperty(new DepthPrivateFilterSettingsProperty()
                {
                    softwareId = this.softwareId,
                    filterEnabled = this.filterEnabled,
                    hcg = this.hcg,
                    calibrationDataPath = this.calibrationDataPath,
                    transformationMode = this.transformationMode,
                    exposure = this.exposure,
                });

                this.settingsApplied?.Invoke();
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
            }
        }

        private IEnumerator Process()
        {
            this.ApplySettings();

            var prevSoftwareId = this.softwareId;
            var prevFilterEnabled = this.filterEnabled;
            var prevHcg = this.hcg;
            var prevTransformationMode = this.transformationMode;
            var prevExposure = this.exposure;
            bool streamIsClosed = false;

            while (true)
            {
                bool reapplySettings = false;

                if (TofArTofManager.Instance?.Stream == null && !streamIsClosed)
                {
                    streamIsClosed = true;
                }

                if (streamIsClosed && TofArTofManager.Instance?.Stream != null)
                {
                    streamIsClosed = false;
                    reapplySettings = true;
                }

                if (!streamIsClosed)
                {
                    reapplySettings |= (prevSoftwareId != this.softwareId) || (prevFilterEnabled != this.filterEnabled) | (prevHcg != this.hcg)
                    || (prevTransformationMode != this.transformationMode) || (prevExposure != this.exposure);

                    if (reapplySettings)
                    {
                        try
                        {
                            this.ApplySettings();

                            prevSoftwareId = this.softwareId;
                            prevFilterEnabled = this.filterEnabled;
                            prevHcg = this.hcg;
                            prevTransformationMode = this.transformationMode;
                            prevExposure = this.exposure;
                        }
                        catch (Exception e)
                        {
                            TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(e));
                        }
                    }
                }

                yield return null;
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                string calibrationFile = Application.persistentDataPath + "/calibration.bin";
                if (System.IO.File.Exists(calibrationFile))
                {
                    System.IO.File.Delete(calibrationFile);
                }
            }
            else
            {
                this.ApplySettings();
            }
        }
    }
}

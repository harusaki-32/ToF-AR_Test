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
using System.Collections;
using UnityEngine;

namespace TofAr.V0.Tof
{
    /// <summary>
    /// カメラ映像を3Dオブジェクトの表面にマッピングする
    /// </summary>
    public class TextureMapper3D : MonoBehaviour
    {
        [System.Serializable]
        private class JsonCamSettings
        {
            public string id = "";
            public string isHidden = "";
        }

        [System.Serializable]
        private class deviceAttributesJson
        {
            public JsonCamSettings[] cameraSettings;
        }

        /// <summary>
        /// DepthのTextureを表示するMaterial
        /// </summary>
        public Material depthViewMaterial;
        /// <summary>
        /// ConfidenceのTextureを表示するMaterial
        /// </summary>
        public Material confidenceViewMaterial;
        /// <summary>
        /// Xperia1のDepthPrivateモードConfidenceのTextureを表示するMaterial
        /// </summary>
        public Material confidenceViewMaterialDepthPrivate;
        private Renderer thisRenderer;

        /// <summary>
        /// 表示対象種別
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Depth映像を表示する
            /// </summary>
            Depth,
            /// <summary>
            /// Confidence映像を表示する
            /// </summary>
            Confidence,
        };
        /// <summary>
        /// 表示対象種別
        /// </summary>
        public Type type;

        void OnEnable()
        {
            thisRenderer = this.GetComponent<Renderer>();
            TofArTofManager.OnStreamStarted += OnStreamStarted;
            TofArTofManager.OnStreamStopped += OnStreamStopped;
            this.StartCoroutine(this.Process());
        }

        void OnDisable()
        {
            TofArTofManager.OnStreamStopped -= OnStreamStopped;
            TofArTofManager.OnStreamStarted -= OnStreamStarted;
            this.StopCoroutine(this.Process());
        }

        private void OnStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, PointCloudData pointCloudData)
        {
            if (thisRenderer != null)
            {
                switch (this.type)
                {
                    case Type.Depth:
                        thisRenderer.material.mainTexture = depthTexture;
                        break;
                    case Type.Confidence:
                        thisRenderer.material.mainTexture = confidenceTexture;
                        break;
                }
            }
        }

        private void OnStreamStopped(object sender)
        {
            if (thisRenderer.material != null)
            {
                thisRenderer.material.mainTexture = null;
            }
        }

        private IEnumerator Process()
        {
            yield return null;
            bool isFirst = true;
            var currentType = this.type;
            while (true)
            {
                if (thisRenderer != null)
                {
                    if ((currentType != this.type) || isFirst)
                    {
                        var config = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();

                        var useDepthPrivateShader = (config?.isDepthPrivate ?? false) || (config?.isFusion ?? false);
                        var capability = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();

                        var depthcorr = JsonUtility.FromJson<deviceAttributesJson>(capability.TrimmedDeviceAttributesString);
                        if (depthcorr != null && depthcorr.cameraSettings != null)
                        {
                            foreach (var setting in depthcorr.cameraSettings)
                            {
                                if (setting != null && setting.id == config.cameraId && setting.isHidden == "true")
                                {
                                    useDepthPrivateShader = true;
                                    break;
                                }
                            }
                        }

                        isFirst = false;
                        switch (this.type)
                        {
                            case Type.Depth:
                                thisRenderer.material = this.depthViewMaterial;
                                thisRenderer.material.mainTexture = TofArTofManager.Instance.DepthTexture;
                                break;
                            case Type.Confidence:
                                thisRenderer.material = useDepthPrivateShader ? this.confidenceViewMaterialDepthPrivate : this.confidenceViewMaterial;
                                thisRenderer.material.mainTexture = TofArTofManager.Instance.ConfidenceTexture;
                                break;
                        }
                    }
                }
                currentType = this.type;
                yield return null;
            }
        }

    }
}

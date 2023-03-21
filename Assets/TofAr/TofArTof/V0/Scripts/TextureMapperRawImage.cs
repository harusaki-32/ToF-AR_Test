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
using UnityEngine.UI;

namespace TofAr.V0.Tof
{
    /// <summary>
    /// カメラ映像をuGUI RawImageオブジェクトに表示する
    /// <para>端末の回転方向に自動追従して映像を回転する。</para>
    /// </summary>
    public class TextureMapperRawImage : MonoBehaviour
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

        /// <summary>
        /// Confidenceブースト
        /// </summary>
        [System.Obsolete]
        [HideInInspector]
        public float ConfidenceBoost = 1f;

        /// <summary>
        /// 表示種別
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Depth映像を表示する
            /// <para>デフォルト値：Depth</para>
            /// </summary>
            Depth,
            /// <summary>
            /// Confidence映像を表示する
            /// </summary>
            Confidence,
        };
        /// <summary>
        /// 表示種別
        /// </summary>
        public Type type;

        /// <summary>
        /// <para>true: 端末の回転方向に応じて表示を回転させる</para>
        /// <para>false: 表示の自動回転を行わない</para>
        /// <para>デフォルト値：true</para>
        /// </summary>
        public bool AutoRotation = true;
        /// <summary>
        /// <para>true: RawImageの親オブジェクト領域内に最大化表示を行う</para>
        /// <para>false: RawImageの親オブジェクト領域内に最大化表示を行わない</para>
        /// <para>デフォルト値：true</para>
        /// </summary>
        public bool Maximize;
        /// <summary>
        /// アスペクト比
        /// <para>デフォルト値：1.333</para>
        /// </summary>
        public float AspectRatio { get => aspectRatio; }

        private float aspectRatio = 1.333f;

        private bool useDepthPrivateShader = false;
        private RawImage rawImage;

        /// <summary>
        /// 親の短形位置情報
        /// </summary>
        protected RectTransform parentRectTransform;

        /// <summary>
        /// 短形位置情報
        /// </summary>
        protected RectTransform rectTransform;
        private Vector2 latestParentSize;
        private int imageRotation = 0;
        private bool aspectRatioUpdated = true;

        /// <summary>
        /// 値が変更されたときに呼び出されます
        /// </summary>
        protected virtual void OnValidate()
        {
            parentRectTransform = transform.parent as RectTransform;
            rectTransform = transform as RectTransform;
        }

        /// <summary>
        /// インスタンスがロードされたときに呼び出されます
        /// </summary>
        protected virtual void Awake()
        {
            parentRectTransform = transform.parent as RectTransform;
            rectTransform = transform as RectTransform;
            rawImage = GetComponent<RawImage>();
        }

        /// <summary>
        /// オブジェクトが有効になったときに呼び出されます
        /// </summary>
        protected virtual void OnEnable()
        {
            TofArManager.OnScreenOrientationUpdated += OnScreenOrientationUpdated;

            UpdateRotation();
            TofArTofManager.OnStreamStarted += OnStreamStarted;
            TofArTofManager.OnStreamStopped += OnStreamStopped;
            isFirst = true;
            currentType = this.type;

            if (TofArTofManager.Instance.IsStreamActive)
            {
                OnStreamStarted(TofArTofManager.Instance, null, null, null);
            }
        }

        

        /// <summary>
        /// オブジェクトが無効になったときに呼び出されます
        /// </summary>
        protected virtual void OnDisable()
        {
            TofArManager.OnScreenOrientationUpdated -= OnScreenOrientationUpdated;
            TofArTofManager.OnStreamStarted -= OnStreamStarted;
            TofArTofManager.OnStreamStopped -= OnStreamStopped;
        }


        bool isFirst = true;
        Type currentType;

        void Update()
        {
            CheckType();
            UpdateImageSize();
        }

        /// <summary>
        /// 回転方向の更新
        /// </summary>
        protected virtual void UpdateRotation()
        {
            if (AutoRotation)
            {
                imageRotation = TofArManager.Instance?.GetScreenOrientation() ?? imageRotation;

                rectTransform.localRotation = Quaternion.Euler(0f, 0f, imageRotation);
            }
            else
            {
                imageRotation = (int)(rectTransform.localRotation.eulerAngles.z);
            }
        }

        private void OnScreenOrientationUpdated(ScreenOrientation previousScreenOrientation, ScreenOrientation newScreenOrientation)
        {
            UpdateRotation();
        }

        /// <summary>
        /// 画像サイズの更新
        /// </summary>
        protected virtual void UpdateImageSize()
        {
            float width, height;
            if (Maximize)
            {
                var parentWidth = parentRectTransform.rect.width;
                var parentHeight = parentRectTransform.rect.height;

                if (imageRotation == 90 || imageRotation == 270)
                {
                    parentWidth = parentRectTransform.rect.height;
                    parentHeight = parentRectTransform.rect.width;
                }

                if (!aspectRatioUpdated &&
                    Mathf.Approximately(latestParentSize.x, parentWidth) &&
                    Mathf.Approximately(latestParentSize.y, parentHeight))
                {
                    return;
                }

                latestParentSize = new Vector2(parentWidth, parentHeight);

                width = parentWidth;
                height = parentHeight;

                var parentAspectRatio = parentWidth / parentHeight;
                if (parentAspectRatio > aspectRatio)
                {
                    width = height * aspectRatio;
                }
                else
                {
                    height = width / aspectRatio;
                }
            }
            else
            {
                if (!aspectRatioUpdated)
                {
                    return;
                }

                height = rectTransform.rect.height;
                width = height * aspectRatio;
            }
            rectTransform.sizeDelta = new Vector2(width, height);
            aspectRatioUpdated = false;
        }

        private void OnStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, PointCloudData pointCloudData)
        {
            try
            {
                var currentConfiguration = TofArTofManager.Instance.GetProperty(new CameraConfigurationProperty());
                if (currentConfiguration != null)
                {
                    aspectRatioUpdated = true;
                    aspectRatio = (float)currentConfiguration.width / currentConfiguration.height;
                }
            }
            catch (SensCord.ApiException)
            {
                // Might be using SKV playback
                var currentConfiguration = TofArTofManager.Instance.GetProperty<SensCord.ImageProperty>();
                if (currentConfiguration != null)
                {
                    aspectRatioUpdated = true;
                    aspectRatio = (float)currentConfiguration.Width / currentConfiguration.Height;
                }
            }

            UpdateRotation();
            CheckMaterial();
        }

        private void OnStreamStopped(object sender)
        {
            rawImage.texture = null;
        }

        private void CheckMaterial()
        {
            try
            {
                var config = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
                useDepthPrivateShader = (config?.isDepthPrivate ?? false) || (config?.isFusion ?? false);

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

            }
            catch (SensCord.ApiException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Unable to get camera config: " + e.Message);
            }
            SetTextureAndMaterial();
        }

        private void CheckType()
        {
            if (currentType != this.type || isFirst)
            {
                var success = SetTextureAndMaterial();
                if (success)
                {
                    isFirst = false;
                }
                currentType = this.type;
            }
        }

        private bool SetTextureAndMaterial()
        {
            if (rawImage == null)
            {
                return false;
            }

            var instance = TofArTofManager.Instance;


            switch (this.type)
            {
                case Type.Depth:
                    rawImage.material = this.depthViewMaterial;
                    rawImage.texture = instance?.DepthTexture;
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "Apply tex and mat");
                    break;
                case Type.Confidence:
                    rawImage.material = useDepthPrivateShader ? this.confidenceViewMaterialDepthPrivate : this.confidenceViewMaterial;
                    rawImage.texture = instance?.ConfidenceTexture;
                    break;
            }
            return true;
        }
    }
}

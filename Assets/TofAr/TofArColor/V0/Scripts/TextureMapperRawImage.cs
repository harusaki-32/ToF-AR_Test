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

namespace TofAr.V0.Color
{
    /// <summary>
    /// カメラ映像をuGUI RawImageオブジェクトに表示する
    /// <para>端末の回転方向に自動追従して映像を回転する。</para>
    /// </summary>
    public class TextureMapperRawImage : MonoBehaviour
    {
        /// <summary>
        /// YUVフォーマットのColorデータ表示用Material
        /// </summary>
        public Material YUVViewMaterial;
        /// <summary>
        /// BGRAフォーマットのColorデータ表示用Material
        /// </summary>
        public Material BGRAViewMaterial;
        /// <summary>
        /// 白色のテクスチャー
        /// </summary>
        public Texture2D whiteTexture;

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
        /// </summary>
        public float AspectRatio { get => aspectRatio; }
        private float aspectRatio = 1.333f;

        private RawImage rawImage;
        private RectTransform parentRectTransform;
        private RectTransform rectTransform;
        private Vector2 latestParentSize;
        private int imageRotation;
        private bool aspectRatioUpdated = true;

#if UNITY_EDITOR
        void OnValidate()
        {
            parentRectTransform = transform.parent as RectTransform;
            if (parentRectTransform != null)
            {
                parentRectTransform.up = parentRectTransform.up;
            }
            rectTransform = transform as RectTransform;
        }
#endif

        void Awake()
        {
            parentRectTransform = transform.parent as RectTransform;
            rectTransform = transform as RectTransform;
            rawImage = GetComponent<RawImage>();
        }

        void OnEnable()
        {
            TofArManager.OnScreenOrientationUpdated += OnScreenOrientationUpdated;
            TofArColorManager.OnStreamStarted += StreamStarted;
            TofArColorManager.OnStreamStopped += StreamStopped;

            UpdateRotation();

            if (TofArColorManager.Instance.IsStreamActive)
            {
                StreamStarted(TofArColorManager.Instance, null);
            }
        }

        private void OnDisable()
        {
            TofArManager.OnScreenOrientationUpdated -= OnScreenOrientationUpdated;
            TofArColorManager.OnStreamStarted -= StreamStarted;
            TofArColorManager.OnStreamStopped -= StreamStopped;

            this.StopAllCoroutines();
        }

        void Update()
        {
            UpdateImageSize();
        }

        private void UpdateImageSize()
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
                if (parentAspectRatio > AspectRatio)
                {
                    width = height * AspectRatio;
                }
                else
                {
                    height = width / AspectRatio;
                }
            }
            else
            {
                if (!aspectRatioUpdated)
                {
                    return;
                }
                height = rectTransform.rect.height;
                width = height * AspectRatio;
            }

            rectTransform.sizeDelta = new Vector2(width, height);
            aspectRatioUpdated = false;
        }

        private void StreamStarted(object sender, Texture2D colorTexture)
        {
            var currentConfiguration = TofArColorManager.Instance.GetProperty<ResolutionProperty>();
            if (currentConfiguration != null)
            {
                aspectRatioUpdated = true;
                aspectRatio = (float)currentConfiguration.width / currentConfiguration.height;
            }
            UpdateRotation();
            //force it to re-maximise
            latestParentSize = Vector2.zero;
            StartCoroutine(SetTextureAndMaterial());
        }

        private void StreamStopped(object sender)
        {
            if (rawImage.material != null)
            {
                rawImage.material.SetTexture("_YTex", null);
                rawImage.material.SetTexture("_UVTex", null);
            }
            else
            {
                rawImage.texture = null;
            }
        }

        private void UpdateRotation()
        {
            if (AutoRotation && TofArManager.Instantiated)
            {
                imageRotation = TofArManager.Instance.GetScreenOrientation();

                if (rectTransform != null)
                {
                    rectTransform.localRotation = Quaternion.Euler(0f, 0f, imageRotation);
                }
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

        private IEnumerator SetTextureAndMaterial()
        {
            var instance = TofArColorManager.Instance;
            while (rawImage == null || instance?.ColorData == null)
            {
                yield return null;
            }

            if ((instance.ColorData.Type == ColorRawDataType.BGRA) || (instance.ColorData.Type == ColorRawDataType.RGB) || (instance.ColorData.Type == ColorRawDataType.RGBA))
            {
                rawImage.texture = instance.ColorTexture;
                rawImage.material = this.BGRAViewMaterial;
                rawImage.color = UnityEngine.Color.white;
            }
            else if (instance.ColorData.Type == ColorRawDataType.NV21)
            {
                rawImage.texture = this.whiteTexture;
                rawImage.material = this.YUVViewMaterial;
                rawImage.color = UnityEngine.Color.white;
                rawImage.material.SetTexture("_YTex", instance.YTexture);
                rawImage.material.SetTexture("_UVTex", instance.UVTexture);
            }
            else if (instance.ColorData.Type == ColorRawDataType.BGR)
            {
                rawImage.texture = null;
                rawImage.material = null;
                rawImage.color = UnityEngine.Color.black;
            }
        }
    }
}

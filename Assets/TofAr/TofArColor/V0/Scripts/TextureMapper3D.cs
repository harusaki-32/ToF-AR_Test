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

namespace TofAr.V0.Color
{
    /// <summary>
    /// カメラ映像を3Dオブジェクトの表面にマッピングする
    /// </summary>
    public class TextureMapper3D : MonoBehaviour
    {
        /// <summary>
        /// YUVフォーマットのColorデータ表示用Material
        /// </summary>
        public Material YUVViewMaterial;
        /// <summary>
        /// BGRAフォーマットのColorデータ表示用Material
        /// </summary>
        public Material BGRAViewMaterial;

        private Renderer thisRenderer;

        void OnEnable()
        {
            thisRenderer = this.GetComponent<Renderer>();
            TofArColorManager.OnStreamStarted += OnStreamStarted;
            TofArColorManager.OnStreamStopped += OnStreamStopped;
            if (TofArColorManager.Instance.IsStreamActive)
            {
                this.StartCoroutine(this.Process());
            }
        }

        void OnDisable()
        {
            TofArColorManager.OnStreamStarted -= OnStreamStarted;
            TofArColorManager.OnStreamStopped -= OnStreamStopped;
            this.StopAllCoroutines();
        }

        private IEnumerator Process()
        {
            yield return null;

            if (thisRenderer != null)
            {
                yield return this.SetTextureAndMaterial();
            }
        }

        void OnStreamStarted(object sender, Texture2D colorTexture)
        {
            if (thisRenderer != null)
            {
                this.StartCoroutine(this.SetTextureAndMaterial());
            }
        }

        void OnStreamStopped(object sender)
        {
            if (thisRenderer.material != null)
            {
                thisRenderer.material.SetTexture("_YTex", null);
                thisRenderer.material.SetTexture("_UVTex", null);
            }
        }

        private IEnumerator SetTextureAndMaterial()
        {
            var instance = TofArColorManager.Instance;
            while (instance?.ColorData == null)
            {
                yield return null;
            }

            if ((instance.ColorData.Type == ColorRawDataType.BGRA) || (instance.ColorData.Type == ColorRawDataType.RGB) || (instance.ColorData.Type == ColorRawDataType.RGBA))
            {
                thisRenderer.material = this.BGRAViewMaterial;
                thisRenderer.material.mainTexture = instance.ColorTexture;
            }
            else if (instance.ColorData.Type == ColorRawDataType.NV21)
            {
                thisRenderer.material = this.YUVViewMaterial;
                thisRenderer.material.SetTexture("_YTex", instance.YTexture);
                thisRenderer.material.SetTexture("_UVTex", instance.UVTexture);
            }
        }
    }
}

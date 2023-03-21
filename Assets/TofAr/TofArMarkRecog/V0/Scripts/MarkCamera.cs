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

namespace TofAr.V0.MarkRecog
{
    /// <summary>
    /// TODO+ C 内部処理用 SDKからサンプルコードに移動しても良いかも
    /// </summary>
    public class MarkCamera : MonoBehaviour
    {
        private Material mat;

        /// <summary>
        /// TODO+ C
        /// </summary>
        public Shader shader;

        private void OnEnable()
        {
            if (mat == null)
            {
                mat = new Material(this.shader);
            }
            TofArManager.OnScreenOrientationUpdated += OnScreenOrientationChanged;

            UpdateRotation();
        }

        private void OnDisable()
        {
            TofArManager.OnScreenOrientationUpdated -= OnScreenOrientationChanged;
        }

        private void UpdateRotation()
        {
            int imageRotation = TofArManager.Instance.GetScreenOrientation();

            mat.SetFloat("_Angle", imageRotation);
        }

        private void OnScreenOrientationChanged(ScreenOrientation previousDeviceOrientation, ScreenOrientation newDeviceOrientation)
        {
            UpdateRotation();
        }

        // Update is called once per frame
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, mat);
        }
    }
}

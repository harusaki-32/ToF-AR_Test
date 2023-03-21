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
    /// UnityのCameraにアタッチするとColorカメラの内部パラメータに基づきプロジェクションを調整する
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class VirtualToFCamera : MonoBehaviour
    {
        /// <summary>
        /// カメラ
        /// </summary>
        protected Camera thisCamera;

        /// <summary>
        /// 縦幅
        /// </summary>
        protected float portraitHeight;

        /// <summary>
        /// 横幅
        /// </summary>
        protected float portraitWidth;

        /// <summary>
        /// 近接
        /// </summary>
        protected float nearClip;

        /// <summary>
        /// 遠景
        /// </summary>
        protected float farClip;

        void Start()
        {
            thisCamera = this.GetComponent<Camera>();
            this.nearClip = thisCamera.nearClipPlane;
            this.farClip = thisCamera.farClipPlane;

            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                this.portraitWidth = Mathf.Min(Screen.height, Screen.width);
                this.portraitHeight = Mathf.Max(Screen.height, Screen.width);
            }
            else
            {
                if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
                {
                    this.portraitWidth = Screen.height;
                    this.portraitHeight = Screen.width;
                }
                else
                {
                    this.portraitWidth = Screen.width;
                    this.portraitHeight = Screen.height;
                }
            }

            TofArTofManager.Instance?.CalibrationSettingsLoaded.AddListener(OnSettingsLoaded);
        }

        void OnDestroy()
        {
            TofArTofManager.Instance?.CalibrationSettingsLoaded.RemoveListener(OnSettingsLoaded);
        }

        private void OnSettingsLoaded(CalibrationSettingsProperty settings)
        {
            this.UpdateProjectionMatrix(this.CreateProjectionMatrix(settings));
        }

        /// <summary>
        /// 行列更新
        /// </summary>
        /// <param name="matrix"></param>
        protected void UpdateProjectionMatrix(Matrix4x4 matrix)
        {
            thisCamera.projectionMatrix = matrix;
        }

        /// <summary>
        /// プロジェクション行列生成
        /// </summary>
        /// <param name="settings">カメラキャリブレーション情報</param>
        protected virtual Matrix4x4 CreateProjectionMatrix(CalibrationSettingsProperty settings)
        {
            float width = settings.colorWidth;
            float height = settings.colorHeight;
            if (width == 0 || height == 0)
            {
                return thisCamera.projectionMatrix;
            }

            float right = width * nearClip / (2 * settings.c.fx);
            float top = height * nearClip / (2 * settings.c.fy);
            float rightOffset = ((settings.c.cx / settings.colorWidth) - 0.5f) * nearClip;
            float topOffset = ((settings.c.cy / settings.colorHeight) - 0.5f) * nearClip;

            return Matrix4x4.Frustum(rightOffset - right, rightOffset + right, topOffset - top, topOffset + top, nearClip, farClip);
        }
    }
}

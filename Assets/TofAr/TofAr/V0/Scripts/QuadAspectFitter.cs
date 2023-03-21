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

namespace TofAr.V0
{
    /// <summary>
    /// アスペクト比に合わせたサイズに設定する
    /// </summary>
    public abstract class QuadAspectFitter : MonoBehaviour
    {
        /// <summary>
        /// if true the height of the Quad is fixed and the width adjusts,
        /// else the width is fixed and the height adjusts to match the aspect ratio
        /// </summary>
        [SerializeField]
        private bool fixHeight = false;

        /// <summary>
        /// 高さ
        /// </summary>
        public bool FixHeight
        {
            get => fixHeight;
            set
            {
                fixHeight = value;
                SetAspect(lastHeight, lastWidth);
            }
        }

        /// <summary>
        /// does the Quad rotate in space when the phone rotates or not
        /// </summary>
        [SerializeField]
        private bool autoRotate = false;

        /// <summary>
        /// 回転
        /// </summary>
        public bool AutoRotate
        {
            get => autoRotate;
            set
            {
                if (autoRotate != value)
                {
                    autoRotate = value;
                    OnScreenRotate(ScreenOrientation.AutoRotation, ScreenOrientation.AutoRotation);
                }
            }
        }


        /// <summary>
        /// the size of the fixed scale parameter (so height if fixHeight, else width)
        /// </summary>
        [SerializeField]
        private float scaleFactor = 1;

        /// <summary>
        /// 倍率
        /// </summary>
        public float ScaleFactor
        {
            get => scaleFactor;
            set
            {
                scaleFactor = value;
                SetAspect(lastHeight, lastWidth);
            }
        }

        /// <summary>
        /// オブジェクトが有効になったときに呼び出されます
        /// </summary>
        protected virtual void OnEnable()
        {
            TofArManager.OnScreenOrientationUpdated += OnScreenRotate;
            OnScreenRotate(Screen.orientation, Screen.orientation);
        }

        /// <summary>
        /// オブジェクトが無効になったときに呼び出されます
        /// </summary>
        protected virtual void OnDisable()
        {
            TofArManager.OnScreenOrientationUpdated -= OnScreenRotate;
        }


        private int lastHeight = 1, lastWidth = 1;

        /// <summary>
        /// 与えられたパラメータのアスペクト比に合わせたサイズに設定する
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        protected void SetAspect(int height, int width)
        {
            if (height > 0 && width > 0)
            {
                lastHeight = height;
                lastWidth = width;
                float aspect = (float)height / (float)width;
                if (fixHeight)
                {
                    this.transform.localScale = new Vector3(scaleFactor / aspect, scaleFactor, 1);
                }
                else
                {
                    this.transform.localScale = new Vector3(scaleFactor, scaleFactor * aspect, 1);
                }
            }
        }

        /// <summary>
        /// handles Autorotation
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="current"></param>
        private void OnScreenRotate(ScreenOrientation prev, ScreenOrientation current)
        {
            if (autoRotate)
            {
                this.transform.localRotation = Quaternion.Euler(0, 0, TofArManager.Instance.GetScreenOrientation());
            }
        }
    }
}

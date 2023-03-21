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

using System;
using UnityEngine;

namespace TofAr.V0.Hand
{
    /// <summary>
    /// Handの表示を管理するクラス
    /// </summary>
    public abstract class AbstractHandModel : MonoBehaviour, IHandModel
    {
        [SerializeField]
        private HandStatus lrHand;

        /// <summary>
        /// 手の認識状態
        /// </summary>
        public HandStatus LRHand
        {
            get { return lrHand; }
            set
            {
                lrHand = value;

                if (isActiveAndEnabled)
                {
                    SetupHandCalcCallbacks();
                }
            }
        }
        [SerializeField]
        private bool _autoRotate = false;

        /// <summary>
        /// *TODO+ B
        /// true: HandModelを端末向きに従って自動回転する
        /// false: 自動回転しない
        /// デフォルト値: false
        /// </summary>
        [Obsolete("please use AutoRotate instead")]
        private bool autoRotate
        {
            get => AutoRotate;
            set => AutoRotate = value;
        }

        /// <summary>
        /// <para>true: HandModelを端末向きに従って自動回転する</para>
        /// <para>false: 自動回転しない</para>
        /// <para>デフォルト値: false</para>
        /// </summary>
        public bool AutoRotate
        {
            get { return _autoRotate; }
            set
            {
                _autoRotate = value;
                if (value)
                {
                    DoAutoRotate();
                }
            }
        }

        /// <summary>
        /// 手の認識状態
        /// </summary>
        [HideInInspector]
        public HandStatus handStatus;

        /// <summary>
        /// <para>true: 自動再生モード</para>
        /// <para>デフォルト値: false</para>
        /// </summary>
        public bool autoMode = false;
        /// <summary>
        /// 自動再生モードで使用する点
        /// </summary>
        public Vector3[] autoPoints;

        /// <summary>
        /// 手の座標
        /// </summary>
        protected Vector3[] handPoints = new Vector3[0];

        private bool isNewHandPoints = false;
        /// <summary>
        /// <para>true: 手が認識された</para>
        /// <para>false: 認識されていない</para>
        /// </summary>
        public bool IsHandDetected { get; private set; }

        /// <summary>
        /// 手の点の配列
        /// </summary>
        public Vector3[] HandPoints
        {
            get { return handPoints; }
            private set
            {
                if (value != null)
                {
                    int pointsLength = Enum.GetValues(typeof(HandPointIndex)).Length;
                    if (value.Length <= pointsLength)
                    {
                        handPoints = value;
                    }
                    else
                    {
                        handPoints = new Vector3[pointsLength];
                        Array.Copy(value, handPoints, pointsLength);
                    }
                    isNewHandPoints = true;
                }
            }
        }

        private Vector3[] worldHandPoints = null;
        /// <summary>
        /// 手の点の配列（world座標）
        /// </summary>
        public Vector3[] WorldHandPoints
        {
            get
            {
                if (handPoints == null)
                {
                    return null;
                }

                if (worldHandPoints == null)
                {
                    worldHandPoints = new Vector3[handPoints.Length];
                }
                else if (worldHandPoints.Length != handPoints.Length)
                {
                    Array.Resize(ref worldHandPoints, handPoints.Length);
                }
                if (isNewHandPoints)
                {
                    for (int i = 0; i < handPoints.Length; i++)
                    {
                        worldHandPoints[i] = transform.TransformPoint(handPoints[i]);
                    }
                    isNewHandPoints = false;
                }

                return worldHandPoints;
            }
        }

        /// <summary>
        /// 手の座標計算コールバック設定
        /// </summary>
        protected virtual void SetupHandCalcCallbacks()
        {
            if (lrHand == HandStatus.LeftHand)
            {
                HandCalc.OnRightHandPointsCalculated -= OnHandsCalculated;
                HandCalc.OnLeftHandPointsCalculated += OnHandsCalculated;
            }
            else if (lrHand == HandStatus.RightHand)
            {
                HandCalc.OnLeftHandPointsCalculated -= OnHandsCalculated;
                HandCalc.OnRightHandPointsCalculated += OnHandsCalculated;
            }
        }

        /// <summary>
        /// オブジェクトが有効になったときに呼び出されます
        /// </summary>
        protected virtual void OnEnable()
        {
            if (LRHand == HandStatus.LeftHand)
            {
                HandCalc.OnLeftHandPointsCalculated += OnHandsCalculated;
            }
            else if (LRHand == HandStatus.RightHand)
            {
                HandCalc.OnRightHandPointsCalculated += OnHandsCalculated;
            }

            TofArManager.OnScreenOrientationUpdated += OnScreenRotationChanged;
            TofArHandManager.OnStreamStopped += OnStreamStopped;

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
            OnScreenRotationChanged(ScreenOrientation.LandscapeLeft, Screen.orientation);
#if UNITY_EDITOR
            }
#endif
        }

        /// <summary>
        /// オブジェクトが無効になったときに呼び出されます
        /// </summary>
        protected virtual void OnDisable()
        {
            if (LRHand == HandStatus.LeftHand)
            {
                HandCalc.OnLeftHandPointsCalculated -= OnHandsCalculated;
            }
            else if (LRHand == HandStatus.RightHand)
            {
                HandCalc.OnRightHandPointsCalculated -= OnHandsCalculated;
            }

            TofArHandManager.OnStreamStopped -= OnStreamStopped;

            TofArManager.OnScreenOrientationUpdated -= OnScreenRotationChanged;
        }

        private void OnStreamStopped(object sender)
        {
            this.handPoints = null;
        }

        /// <summary>
        /// Update関数が呼び出された後に実行されます
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (autoMode)
            {
                if (autoPoints != null && autoPoints.Length > 10)
                {
                    HandPoints = autoPoints;
                    handStatus = LRHand;
                    IsHandDetected = true;
                }
                else
                {
                    handStatus = HandStatus.NoHand;
                    IsHandDetected = false;
                }
            }
            else
            {
                IsHandDetected = !(this.handPoints == null || this.handPoints.Length == 0) && (this.handStatus == this.lrHand || this.handStatus == HandStatus.BothHands || this.handStatus == HandStatus.Tip);
            }

            DrawHandModel();
        }

        /// <summary>
        /// 手のモデル表示
        /// </summary>
        abstract protected void DrawHandModel();

        /// <summary>
        /// 手の座標計算
        /// </summary>
        /// <param name="points">位置</param>
        /// <param name="handStatus">手の認識状態</param>
        protected void OnHandsCalculated(Vector3[] points, HandStatus handStatus)
        {
            if (autoMode)
            {
                return;
            }

            if ((points == null) || (points.Length == 0))
            {
                this.handStatus = HandStatus.NoHand;
                return;
            }
            this.handStatus = handStatus;

            if (this.handPoints == null || this.handPoints.Length != points.Length)
            {
                this.handPoints = new Vector3[points.Length];
            }

            Array.Copy(points, this.handPoints, points.Length);
            this.isNewHandPoints = true;
        }

        /// <summary>
        /// 画面方向変更
        /// </summary>
        /// <param name="previousOrientation">回転前の画面向き</param>
        /// <param name="newOrientation">回転後の画面向き</param>
        protected void OnScreenRotationChanged(ScreenOrientation previousOrientation, ScreenOrientation newOrientation)
        {
            if (AutoRotate)
            {
                DoAutoRotate();
            }
        }

        /// <summary>
        /// 画面方向
        /// </summary>
        protected void DoAutoRotate()
        {
            int imageRotation = 0;

            if (!UnityEngine.XR.XRSettings.enabled)
            {
                imageRotation = TofArManager.Instance.GetScreenOrientation();

                this.transform.localRotation = Quaternion.Euler(0f, 0f, imageRotation);
            }
            else
            {
                if ((TofArManager.Instance.EnabledOrientations & (TofArManager.Instance.EnabledOrientations - 1)) != 0)
                {
                    switch (Input.deviceOrientation)
                    {
                        case DeviceOrientation.LandscapeLeft:
                            imageRotation = 0; break;
                        case DeviceOrientation.LandscapeRight:
                            imageRotation = 180; break;
                        default:
                            return;
                    }

                    this.transform.localRotation = Quaternion.Euler(0f, 0f, imageRotation);
                }
                else
                {
                    switch (TofArManager.Instance.EnabledOrientations)
                    {
                        case EnabledOrientation.LandscapeLeft:
                            imageRotation = 0; break;
                        case EnabledOrientation.LandscapeRight:
                            imageRotation = 180; break;
                        default:
                            return;
                    }

                    this.transform.localRotation = Quaternion.Euler(0f, 0f, imageRotation);
                }
            }
        }
    }
}

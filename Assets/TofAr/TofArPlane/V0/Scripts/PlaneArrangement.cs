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
using System.Collections;
using UnityEngine;

namespace TofAr.V0.Plane
{
    /// <summary>
    /// PlaneObjectプレファブの空間配置処理の実装
    /// </summary>
    public class PlaneArrangement : MonoBehaviour
    {

        [SerializeField]
        private Vector2 centerPosition;

        /// <summary>
        /// 平面検出基準点
        /// </summary>
        public Vector2 CenterPosition
        {
            set
            {
                centerPosition = new Vector2(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y));
                if (isStarted)
                {
                    UpdatePlaneAlgorithmConfigProperty();
                }
            }
            get
            {
                return centerPosition;
            }
        }

        [SerializeField]
        private float minSize = 0;

        /// <summary>
        /// 検出する最小の平面のサイズ。端点までの距離がこれより小さい平面は検出しない。
        /// </summary>
        public float MinSize
        {
            set
            {
                minSize = value;
                if (isStarted)
                {
                    UpdatePlaneAlgorithmConfigProperty();
                }
            }
            get
            {
                return minSize;
            }
        }

        [SerializeField]
        private int interval = 11;

        /// <summary>
        /// 平面推定に求める点の間隔 (デフォルト値:11)
        /// </summary>
        public int Interval
        {
            set
            {
                interval = value;
                if (isStarted)
                {
                    UpdatePlaneAlgorithmConfigProperty();
                }
            }
            get
            {
                return interval;
            }
        }

        [SerializeField]
        private int kSize = 5;

        /// <summary>
        /// 平滑化に使用するフィルタカーネルのサイズ (デフォルト値:5)
        /// </summary>
        public int KSize
        {
            set
            {
                kSize = value;
                if (isStarted)
                {
                    UpdatePlaneAlgorithmConfigProperty();
                }
            }
            get
            {
                return kSize;
            }
        }

        [SerializeField]
        private float planeThreshold = 10f;

        /// <summary>
        /// 求めた平面とPointCloudの距離の閾値 (デフォルト値:10.0)
        /// </summary>
        public float PlaneThreshold
        {
            set
            {
                planeThreshold = value;
                UpdatePlaneAlgorithmConfigProperty();
            }
            get
            {
                return planeThreshold;
            }
        }

        /// <summary>
        /// 表示している平面データ
        /// </summary>
        public VariablesProperty Data
        {
            get; private set;
        }

        /// <summary>
        /// 平面表示用オブジェクト
        /// </summary>
        public GameObject circleImage;

        /// <summary>
        /// 法線表示用オブジェクト
        /// </summary>
        public GameObject normalImage;

        private AlgorithmConfigProperty planeConfig;

        private bool isStarted = false;

        private GameObject GetPlaneRoot()
        {
            var planeRoot = GameObject.Find("PlaneRoot");

            if (planeRoot == null)
            {
                Transform currentRoot = this.transform.parent;

                planeRoot = new GameObject("PlaneRoot");
                planeRoot.transform.SetParent(currentRoot, false);
                planeRoot.transform.localPosition = Vector3.zero;
                planeRoot.transform.localRotation = Quaternion.identity;
            }

            return planeRoot;
        }

        void OnEnable()
        {
            isStarted = true;

            // look for planeRoot
            var planeRoot = GetPlaneRoot();

            this.transform.SetParent(planeRoot.transform);
            TofArPlaneManager.OnStreamStarted += OnStartStream;

            this.StartCoroutine(this.Process());
        }

        void OnDisable()
        {
            TofArPlaneManager.OnStreamStarted -= OnStartStream;

            this.StopCoroutine(this.Process());
        }

        private int GetIndex()
        {
            return this.transform.GetSiblingIndex();
        }

        private void UpdatePlaneAlgorithmConfigProperty()
        {
            var width = 0;
            var height = 0;

            var tofInstance = Tof.TofArTofManager.Instance;
            if (tofInstance != null)
            {
                Tof.CameraConfigurationProperty cameraProp = tofInstance.GetProperty<Tof.CameraConfigurationProperty>();

                if (cameraProp != null)
                {
                    width = cameraProp.width;
                    height = cameraProp.height;
                }
            }

            this.planeConfig = new AlgorithmConfigProperty
            {
                width = width,
                height = height,
                posx = (int)(centerPosition.x * width),
                posy = (int)(centerPosition.y * height),
                min_size = minSize * 1000f,
                interval = interval,
                ksize = kSize,
                plane_threshold = planeThreshold
            };

            int idx = GetIndex();

            var instance = TofArPlaneManager.Instance;

            if (instance != null)
            {
                var algorithmConfigsProperty = instance.GetProperty<AlgorithmConfigsProperty>();

                if (algorithmConfigsProperty != null && algorithmConfigsProperty.configurations != null)
                {
                    if (idx >= 0 && idx < algorithmConfigsProperty.configurations.Length)
                    {

                        algorithmConfigsProperty.configurations[idx] = planeConfig;

                        instance.SetProperty<AlgorithmConfigsProperty>(algorithmConfigsProperty);
                    }
                    else
                    {
                        int nConfigs = algorithmConfigsProperty.configurations.Length + 1;

                        if (nConfigs <= 8)
                        {
                            var configurations = new AlgorithmConfigProperty[nConfigs];

                            algorithmConfigsProperty.configurations.CopyTo(configurations, 0);

                            configurations[nConfigs - 1] = planeConfig;

                            algorithmConfigsProperty.configurations = configurations;

                            instance.SetProperty<AlgorithmConfigsProperty>(algorithmConfigsProperty);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 平面を選択解除する
        /// </summary>
        public void Deselect()
        {
            normalImage.GetComponentInChildren<SpriteRenderer>().color = UnityEngine.Color.red;
        }

        /// <summary>
        /// 平面を選択する
        /// </summary>
        public void Select()
        {
            normalImage.GetComponentInChildren<SpriteRenderer>().color = UnityEngine.Color.yellow;
        }

        private void OnStartStream(object sender, AlgorithmConfigProperty config)
        {
            if (config != null)
            {
                this.planeConfig = config;
            }

            UpdatePlaneAlgorithmConfigProperty();
        }

        private IEnumerator Process()
        {
            var instance = TofArPlaneManager.Instance;

            while (this.isActiveAndEnabled)
            {
                int idx = GetIndex();

                Data = null;

                if (instance.PlaneData != null)
                {
                    if (idx < instance.PlaneData.Data.Length)
                    {
                        Data = instance.PlaneData.Data[idx];
                    }
                }
                if ((Data != null) && (Data.radius > 0))
                {
                    circleImage.SetActive(true);
                    normalImage.SetActive(true);

                    this.transform.localPosition = Data.center;
                    circleImage.transform.localScale = new Vector3(Data.radius * 2, Data.radius * 2, 1);
                    this.transform.localRotation = Quaternion.LookRotation(Data.normal, Vector3.up);
                }
                else
                {
                    circleImage.SetActive(false);
                    normalImage.SetActive(false);
                }
                yield return null;
            }
        }
    }
}

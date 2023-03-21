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
using System.Collections.Generic;
using TofAr.V0.Tof;
using TensorFlowLite.Runtime;
using UnityEngine;
using System.Threading;

namespace TofAr.V0.Body.SV1
{
    /// <summary>
    /// SV1Body認識
    /// </summary>
    [Obsolete("SV1 Body recognition will remove in future version")]
    public class SV1BodyPoseEstimator : MonoBehaviour
    {
        //private Dictionary<BodyShot, string> tflFiles = new Dictionary<BodyShot, string>();

        private const string fullBodyTflPath = BodyModel.FULL_BODY_TFL_PATH;
        private const string upperBodyTflPath = BodyModel.UPPER_BODY_TFL_PATH;
        /*private const string FULL_BODY_DEVEL_TFL_PATH = BodyModel.FULL_BODY_DEVEL_TFL_PATH;
        private const string UPPER_BODY_DEVEL_TFL_PATH = BodyModel.UPPER_BODY_DEVEL_TFL_PATH;*/

        /*private const string fullBody = BodyModel.FULL_BODY;
        private const string upperBody = BodyModel.UPPER_BODY;
        private const string DEVEL = BodyModel.DEVEL;
        private const string FULL_BODY_DEVEL = BodyModel.FULL_BODY_DEVEL;
        private const string UPPER_BODY_DEVEL = BodyModel.UPPER_BODY_DEVEL;*/

        private BodyModel bodyModel;

        private bool runBody = false;

        /// <summary>
        /// Body認識で使用するモード
        /// </summary>        
        public BodyShot bodyShot = BodyShot.FullBody;

        /// <summary>
        /// Body認識で使用するTFLiteのExecMode
        /// </summary>        
        public TFLiteRuntime.ExecMode execMode = TFLiteRuntime.ExecMode.EXEC_MODE_CPU;

        /// <summary>
        /// Body認識で使用するスレッド数
        /// </summary>        
        public int threadsNum = 1;

        private int depthHeight;
        private int depthWidth;
        private float depthFx;
        private float depthFy;
        private int rotation;
        private SynchronizationContext context;

        private void Awake()
        {
            context = SynchronizationContext.Current;
        }

        /// <summary>
        /// 破棄時処理
        /// </summary>
        public void OnDestroy()
        {
            this.bodyModel = null;
            LoadAsset.RemoveLocalFiles();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                LoadAsset.RemoveLocalFiles();
            }
        }

        private IEnumerator SetDetectorType()
        {
            yield return null;
            TofArBodyManager.Instance.DetectorType = BodyPoseDetectorType.External;
            if (TofArBodyManager.Instance.IsStreamActive)
            {
                StreamStarted(null);
            }
            if (TofArTofManager.Instance.IsStreamActive)
            {
                TofFrameStarted(null, null, null, null);
            }
        }

        private void OnEnable()
        {
            TofArBodyManager.OnStreamStarted += this.StreamStarted;
            TofArBodyManager.OnStreamStopped += this.StreamStopped;
            TofArTofManager.OnStreamStarted += this.TofFrameStarted;
            TofArTofManager.OnFrameArrived += this.TofFrameArrived;

            TofArManager.OnDeviceOrientationUpdated += OnDeviceOrientationUpdated;
            OnDeviceOrientationUpdated(DeviceOrientation.Unknown, DeviceOrientation.Unknown);
            StartCoroutine(SetDetectorType());
        }

        private void OnDisable()
        {
            TofArBodyManager.OnStreamStarted -= this.StreamStarted;
            TofArBodyManager.OnStreamStopped -= this.StreamStopped;
            TofArTofManager.OnStreamStarted -= this.TofFrameStarted;
            TofArTofManager.OnFrameArrived -= this.TofFrameArrived;
            TofArManager.OnDeviceOrientationUpdated -= OnDeviceOrientationUpdated;
            TofArBodyManager.Instance.DetectorType = BodyPoseDetectorType.Internal_SV2;
            StreamStopped(null);
        }

        private void Update()
        {
            if (this.runBody && TofArBodyManager.Instance.DetectorType == BodyPoseDetectorType.External)
            {
                try
                {
                    this.RunBodyModel();
                }
                catch (IndexOutOfRangeException ex)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(ex));
                    this.bodyModel = null;
                    this.runBody = false;
                }
                catch (ArgumentException ex)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(ex));
                    this.bodyModel = null;
                    this.runBody = false;
                }
            }
        }

        private void StreamStarted(object sender)
        {
            context.Post((s) =>
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "SV1 starting");
                if (this.bodyModel == null)
                {
                    this.bodyModel = new BodyModel();
                }
                this.InitBodyModel();
                this.runBody = true;
            }, null);
        }

        private void StreamStopped(object sender)
        {
            this.runBody = false;
        }

        private void InitBodyModel()
        {
            int performanceCount = 0;

            var modelFile = this.bodyShot == BodyShot.FullBody ? LoadAsset.LoadFileFromResources(fullBodyTflPath) : LoadAsset.LoadFileFromResources(upperBodyTflPath);
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"Init: model={modelFile}, mode={this.execMode}, shot={this.bodyShot}, performance={performanceCount}, threads={this.threadsNum}");
            bool isFullBody = this.bodyShot == BodyShot.FullBody;

            this.bodyModel?.Init(modelFile, this.execMode, isFullBody, performanceCount, this.threadsNum);
        }

        private void RunBodyModel()
        {
            if (this.bodyModel == null)
            {
                return;
            }
            var result = this.bodyModel.Run();

#pragma warning disable CS0618
            TofArBodyManager.Instance.SetEstimatedResult(result, FrameDataSource.TofArSV1BodySkeleton);
#pragma warning restore CS0618
        }

        private void TofFrameStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, PointCloudData pointCloudData)
        {
            var config = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
            if (config != null)
            {
                this.depthHeight = config.height;
                this.depthWidth = config.width;
                this.depthFx = config.intrinsics.fx;
                this.depthFy = config.intrinsics.fy;
            }
        }

        private void TofFrameArrived(object sender)
        {
            if (this.depthFx < 1 || this.depthFy < 1)
            {
                var config = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
                if (config != null)
                {
                    this.depthHeight = config.height;
                    this.depthWidth = config.width;
                    this.depthFx = config.intrinsics.fx;
                    this.depthFy = config.intrinsics.fy;
                }
            }

            var depth = TofArTofManager.Instance?.DepthData?.Data;
            if (depth == null)
            {
                return;
            }
            this.bodyModel?.SetSrcData(depth, this.depthHeight, this.depthWidth, this.depthFx, this.depthFy, this.rotation);
        }

        private void OnDeviceOrientationUpdated(DeviceOrientation previousDeviceOrientation, DeviceOrientation newDeviceOrientation)
        {
            this.rotation = TofArManager.Instance.GetDeviceOrientation();
        }
    }
}

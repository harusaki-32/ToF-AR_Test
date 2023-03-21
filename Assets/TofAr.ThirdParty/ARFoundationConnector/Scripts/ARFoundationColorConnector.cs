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
using TofAr.V0.Color;
using TofAr.V0;
using System.Runtime.InteropServices;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    public class ARFoundationColorConnector : MonoBehaviour, IExternalColorStream
    {
        private FunctionStreamParametersProperty streamParametersProperty = new FunctionStreamParametersProperty();
        private static ColorFunctionStreamDelegate colorDelegate;

        private static int width = 0, height = 0, uvPixelStride = 0;
        private static int rowStride = 0;
        public bool Started { get; private set; } = false;

        [SerializeField]
        internal bool autoStart = false;

        [SerializeField]
        private ARCameraManager cameraManager;

        private CameraFacingDirection currentFacingDirection;
        private bool setupFinished = false;

        private bool waitUntilRetry = false;

        private void OnEnable()
        {
            cameraManager.frameReceived += CameraManager_frameReceived;
            StartCoroutine(Setup());
        }

        private void OnDisable()
        {
            cameraManager.frameReceived -= CameraManager_frameReceived;
        }

        private void CameraManager_frameReceived(ARCameraFrameEventArgs obj)
        {
            if (this.currentFacingDirection != this.cameraManager.currentFacingDirection && this.setupFinished)
            {
                this.currentFacingDirection = this.cameraManager.currentFacingDirection;
                StartCoroutine(SwitchCameraFacing());
            }
        }

        private IEnumerator Setup()
        {
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == V0.RunMode.MultiNode)
            {
                width = 1920;
                height = 1080;
                yield break;
            }
            this.setupFinished = false;
            colorDelegate = new ColorFunctionStreamDelegate(ColorARFoundationCallback);
            //make sure it doesn't try using the cameras while we wait for the right parameters

            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == TofAr.V0.RunMode.Default)
            {
                SetStreamToExternal();
            }

            yield return WaitForStreamStart();
            StartStream();

            if (!autoStart)
            {
                if (!TofArColorManager.Instance.IsStreamActive)
                {
                    StopStream();
                }
            }
            this.currentFacingDirection = this.cameraManager.currentFacingDirection;
            this.setupFinished = true;
        }

        private IEnumerator SwitchCameraFacing()
        {
            this.setupFinished = false;

            StopStream();
            width = 0;
            height = 0;

            yield return WaitForStreamStart();
            StartStream();
            if (!autoStart)
            {
                if (!TofArColorManager.Instance.IsStreamActive)
                {
                    StopStream();
                }
            }
            this.setupFinished = true;
        }

        private void LateUpdate()
        {
            //TofArManager.Logger.WriteLog(LogLevel.Debug, "connector session update");
            if (Started && !waitUntilRetry)
            {
                //only call the frame callback if we have new framdata from AREngine
                if (LoadFrameData())
                {
                    var cb = new FunctionStreamCallbackProperty();
                    //calls the callback function
                    try
                    {
                        //TofArManager.Logger.WriteLog(LogLevel.Debug, "starting color frame callback");
                        TofArColorManager.Instance?.SetProperty<FunctionStreamCallbackProperty>(cb);
                    }
                    catch (SensCord.ApiException)
                    {
                        waitUntilRetry = true;
                        StartCoroutine(WaitUntilNextTry());
                    }
                }
            }
        }

        private IEnumerator WaitUntilNextTry()
        {
            yield return new WaitForSeconds(1f);
            waitUntilRetry = false;
        }

        private bool LoadFrameData()
        {
            try
            {

                if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage xRCpuImage))
                {
                    if (xRCpuImage != null)
                    {
                        bool copySuccess = CopyToBuffer(xRCpuImage);

                        xRCpuImage.Dispose();

                        if (copySuccess)
                        {
                            return true;
                        }
                    }

                }
                return false;

            }
            catch (IndexOutOfRangeException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message + e.StackTrace);
                return false;
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, e.Message + e.StackTrace);
                return false;
            }
        }

        private bool CopyToBuffer(XRCpuImage xRCpuImage)
        {
            if (xRCpuImage.width == width && xRCpuImage.height == height && xRCpuImage.planeCount > 0)
            {
                long newTimestamp = (long)(xRCpuImage.timestamp * 1e9f);
                if (newTimestamp != lastTimestamp &&
                    (xRCpuImage.format == XRCpuImage.Format.AndroidYuv420_888) ||
                    (xRCpuImage.format == XRCpuImage.Format.IosYpCbCr420_8BiPlanarFullRange))
                {
                    lastTimestamp = newTimestamp;
                    var yplane = xRCpuImage.GetPlane(0);
                    if (yplane != null)
                    {
                        if (copyBufferY == null || copyBufferY.Length != yplane.data.Length)
                        {
                            copyBufferY = new byte[yplane.data.Length];
                        }
                        yplane.data.CopyTo(copyBufferY);
                    }
                    var uplane = xRCpuImage.GetPlane(1);
                    if (uplane != null)
                    {
                        if (copyBufferU == null || copyBufferU.Length != uplane.data.Length)
                        {
                            copyBufferU = new byte[uplane.data.Length];
                        }
                        uplane.data.CopyTo(copyBufferU);

                    }

                    if (!isUVInterlaced)
                    {
                        var vplane = xRCpuImage.GetPlane(2);
                        if (vplane != null)
                        {
                            if (copyBufferV == null || copyBufferV.Length != vplane.data.Length)
                            {
                                copyBufferV = new byte[vplane.data.Length];
                            }
                            vplane.data.CopyTo(copyBufferV);
                        }

                    }

                    return true;
                }
            }
            else
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug,
                    $"expected dimensions {width}x{height} but got image with {xRCpuImage.width}x{xRCpuImage.height}");
            }

            return false;
        }

        private static int uBufferLength;
        private static bool isUVInterlaced;
        private static Int64 lastTimestamp = -1;
        private static byte[] copyBufferY, copyBufferU, copyBufferV;
        [AOT.MonoPInvokeCallback(typeof(ColorFunctionStreamDelegate))]
        public static bool ColorARFoundationCallback(ref Int64 timeStamp, IntPtr y, IntPtr u, IntPtr v)
        {

            if (y == IntPtr.Zero || u == IntPtr.Zero || v == IntPtr.Zero)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"function stream output array was not allocated. y {y.ToInt64()} u {u.ToInt64()} v {v.ToInt64()}");
                return false;
            }
            if (copyBufferY?.Length != width * height && copyBufferY?.Length != rowStride * height)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"wrong length of Y buffer {copyBufferY.Length} instead of {width * height}");
                return false;
            }
            if (copyBufferU?.Length != uBufferLength)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"wrong length of U buffer {copyBufferU.Length} instead of {uBufferLength}");
                return false;
            }

            if (!isUVInterlaced && copyBufferV?.Length != uBufferLength)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"wrong length of V buffer {copyBufferV.Length} instead of {uBufferLength}");
                return false;
            }

            timeStamp = lastTimestamp;
            if (width != rowStride)
            {
                for (int i = 0; i < height; i++)
                {
                    Marshal.Copy(copyBufferY, i * rowStride, y + (i * width), width);
                }
                for (int i = 0; i < height / 2; i++)
                {
                    Marshal.Copy(copyBufferU, i * rowStride, u + (i * width), width);
                }
                if (!isUVInterlaced)
                {
                    for (int i = 0; i < height / 2; i++)
                    {
                        Marshal.Copy(copyBufferV, i * rowStride, v + (i * width), width);
                    }
                }
            }
            else
            {
                Marshal.Copy(copyBufferY, 0, y, width * height);
                Marshal.Copy(copyBufferU, 0, u, uBufferLength);
                if (!isUVInterlaced)
                {
                    Marshal.Copy(copyBufferV, 0, v, uBufferLength);
                }
            }

            return true;

        }

        public void StartStream()
        {
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == TofAr.V0.RunMode.Default)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Starting ARFoundationColorConnector");

                SetStreamToExternal();

                Started = true;
            }
        }

        public void StopStream()
        {
            if (Started)
            {
                Started = false;
            }
        }

        private void SetStreamToExternal()
        {
            if (colorDelegate == null)
            {
                colorDelegate = new ColorFunctionStreamDelegate(ColorARFoundationCallback);
            }

            //set the width and height
            var config = cameraManager.currentConfiguration;
            if (config != null)
            {
                width = config.Value.width;
                height = config.Value.height;
            }

            TofArManager.Logger.WriteLog(LogLevel.Debug, $"setting external stream with {width}x{height}");

            streamParametersProperty.width = width;
            streamParametersProperty.height = height;
            streamParametersProperty.uvPixelStride = uvPixelStride;
            streamParametersProperty.useExternalFunctionStream = true;
            streamParametersProperty.useExternalFunctionCallback = true;
            streamParametersProperty.lensfacing = this.currentFacingDirection == CameraFacingDirection.User ? (int)LensFacing.Front : (int)LensFacing.Back;
            streamParametersProperty.functionPointer = (long)System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(colorDelegate);

            try
            {
                TofArColorManager.Instance.SetProperty(streamParametersProperty);
            }
            catch (SensCord.ApiException)
            {
                TofArColorManager.Instance.StopStream();
                TofArColorManager.Instance.SetProperty(streamParametersProperty);
            }
        }

        private void TryUpdateImageParameters()
        {
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == V0.RunMode.Default)
            {
                if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage xRCpuImage))
                {
                    width = xRCpuImage.width;
                    height = xRCpuImage.height;
                    var yplane = xRCpuImage.GetPlane(0);
                    var uplane = xRCpuImage.GetPlane(1);
                    rowStride = yplane.rowStride;
                    uvPixelStride = uplane.pixelStride;
                    isUVInterlaced = uvPixelStride == 2;
                    uBufferLength = uplane.data.Length;
                    xRCpuImage.Dispose();
                }
            }
        }

        public IEnumerator WaitForStreamStart()
        {
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == V0.RunMode.MultiNode)
            {
                yield break;
            }

            int currentWidth = 0;
            int currentHeight = 0;

            while (width == 0 || height == 0 || width != currentWidth || height != currentHeight)
            {
                var config = cameraManager.currentConfiguration;
                if (config != null && config.HasValue)
                {
                    currentWidth = config.Value.width;
                    currentHeight = config.Value.height;
                }
                yield return new WaitForEndOfFrame();

                TryUpdateImageParameters();
            }

            TofArManager.Logger.WriteLog(LogLevel.Debug, $"update image parameters with {width}x{height}");
        }

        public int GetStreamDelay(TofArColorManager.deviceCameraSettingsJson cameraSettings)
        {
            //check if the ARImage is on
            var arimageManager = FindObjectOfType<ARTrackedImageManager>();
            if (arimageManager?.enabled == true)
            {
                return cameraSettings.colorDelayARFoundationARImage;
            }

            return cameraSettings.colorDelayARFoundation;
        }
    }
}

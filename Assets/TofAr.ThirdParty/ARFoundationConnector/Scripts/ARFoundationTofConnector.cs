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

using TofAr.V0;
using TofAr.V0.Tof;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    public class ARFoundationTofConnector : MonoBehaviour, IExternalStreamSource
    {
        public AROcclusionManager occlusionManager;
        public ARCameraManager cameraManager;

        private FunctionStreamParametersProperty streamParametersProperty = new FunctionStreamParametersProperty();
        private static TofFunctionStreamDelegate tofDelegate;

        private static int width = 0, height = 0;
#if UNITY_IOS
        private static int widthColor = 0, heightColor = 0;
        private static float fx = 0, fy = 0, cx = 0, cy = 0;
#endif
        public bool Started { get; private set; } = false;
        [SerializeField]
        internal bool autoStart = false;


        private void OnEnable()
        {
            StartCoroutine(Setup());
        }

        private IEnumerator Setup()
        {
            //set up so we work if its on the debug server
            yield return null;
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == V0.RunMode.MultiNode)
            {
                width = 240;
                height = 180;
                yield break;
            }
            tofDelegate = new TofFunctionStreamDelegate(TofARFoundationCallback);

            TofArTofManager.Instance.SetProperty(StartStream(null));
            StopStream();
            yield return WaitForStreamStart();
            //set the Tof Manager to use the function stream
            TofArTofManager.Instance.SetProperty(StartStream(null));

            if (!autoStart)
            {
                if (!TofArTofManager.Instance.IsStreamActive)
                {
                    StopStream();
                }
            }
        }


        private void LateUpdate()
        {
            //TofArManager.Logger.WriteLog(LogLevel.Debug, "connector session update");
            if (Started)
            {
                //only call the frame callback if we have new framdata from AREngine
                if (LoadFrameData())
                {
                    var cb = new FunctionStreamCallbackProperty();
                    //calls the callback function
                    TofArTofManager.Instance?.SetProperty<FunctionStreamCallbackProperty>(cb);
                    //TofArManager.Logger.WriteLog(LogLevel.Debug, "Called tof external func callback");
                }
            }
        }

        private bool LoadFrameData()
        {
            try
            {
                if (occlusionManager.TryAcquireEnvironmentDepthCpuImage(out XRCpuImage occlusionImage))
                {
                    if (occlusionImage != null && occlusionImage.width == width && occlusionImage.height == height)
                    {
                        long newTimestamp = (long)(occlusionImage.timestamp * 1e9f);
                        if (newTimestamp != lastTimestamp)
                        {
                            switch (occlusionImage.format)
                            {
                                case XRCpuImage.Format.DepthUint16:
                                    var depthPlane = occlusionImage.GetPlane(0);
                                    if (depthPlane != null)
                                    {
                                        if (copyBuffer == null || depthPlane.data.Length != copyBuffer.Length)
                                        {
                                            copyBuffer = new NativeArray<byte>(depthPlane.data.Length, Allocator.Persistent);
                                        }
                                        copyBuffer.CopyFrom(depthPlane.data);
                                    }
                                    lastTimestamp = newTimestamp;
                                    occlusionImage.Dispose();
                                    return true;

                                case XRCpuImage.Format.DepthFloat32:
                                    int datalength = occlusionImage.width * occlusionImage.height * sizeof(short);
                                    if (copyBuffer == null || datalength != copyBuffer.Length)
                                    {
                                        copyBuffer = new NativeArray<byte>(datalength, Allocator.Persistent);
                                    }
                                    ConvertFloat32(occlusionImage.GetPlane(0).data.Reinterpret<float>(sizeof(byte)), copyBuffer.Reinterpret<short>(sizeof(byte)));

                                    lastTimestamp = newTimestamp;
                                    occlusionImage.Dispose();
                                    return true;
                            }
                        }
                        occlusionImage.Dispose();
                    }
                }
                //TofArManager.Logger.WriteLog(LogLevel.Debug, "Tof Connector no frame");
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

        private static void ConvertFloat32(NativeArray<float> floatArray, NativeArray<short> shortArray)
        {
            Debug.Assert(floatArray.Length == shortArray.Length);

            for (int i = 0; i < shortArray.Length; i++)
            {
                shortArray[i] = (short)(floatArray[i] * 1000);
            }
        }


        private static Int64 lastTimestamp = -1;
        private static NativeArray<byte> copyBuffer;
        [AOT.MonoPInvokeCallback(typeof(TofFunctionStreamDelegate))]
        public static bool TofARFoundationCallback(ref Int64 timeStamp, IntPtr shortArray)
        {

            if (shortArray == IntPtr.Zero)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "function stream output array was not allocated");
                return false;
            }
            if (copyBuffer.Length != width * height * 2)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "wrong length of buffer");
                return false;
            }

            timeStamp = lastTimestamp;

            Marshal.Copy(copyBuffer.ToArray(), 0, shortArray, width * height * 2);
            return true;

        }


        private class deviceAttributesJson
        {
            public string fixedDepthCameraId = "0";
        }

        public CameraConfigurationProperty StartStream(CameraConfigurationProperty selectedConfig)
        {
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == TofAr.V0.RunMode.Default)
            {
                if (tofDelegate == null)
                {
                    tofDelegate = new TofFunctionStreamDelegate(TofARFoundationCallback);
                }
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Starting ARFoundationTofConnector");

                string cameraId = GetCameraId();

#if UNITY_IOS
                float scaleX = (float)widthColor / width;
                float scaleY = (float)heightColor / height;

                CalibrationSettingsProperty settings = new CalibrationSettingsProperty()
                {
                    c = new InternalParameter()
                    {
                        fx = fx,
                        fy = fy,
                        cx = cx,
                        cy = cy
                    },
                    d = new InternalParameter()
                    {
                        fx = fx / scaleX,
                        fy = fy / scaleY,
                        cx = cx / scaleX,
                        cy = cy / scaleY
                    },
                    depthWidth = width,
                    depthHeight = height,
                    colorWidth = widthColor,
                    colorHeight = heightColor,
                    R = new Matrix()
                    {
                        a = 1,
                        b = 0,
                        c = 0,
                        d = 0,
                        e = 1,
                        f = 0,
                        g = 0,
                        h = 0,
                        i = 1
                    }
                };
#else
                CalibrationSettingsProperty settings = TofArTofManager.Instance.LoadSettings(cameraId, height, width, false);
#endif

                TofArTofManager.Instance.CalibrationSettings = settings;

                SetIntrinsics(settings, selectedConfig);

                SetStreamParameters(settings);

                Camera2ConfigurationProperty conf = new Camera2ConfigurationProperty
                {
                    uid = -9997, //special value indicating function stream
                    height = height,
                    width = width,
                    frameRate = 30,
                    isCalibrated = TofArTofManager.Instance.CalibrationSettingsStatus != TofArTofManager.CalibrationSettingsStatusType.BasicCalibration,
                    intrinsics = new Camera2IntrinsicsProperty
                    {
                        k1 = 0,
                        k2 = 0,
                        k3 = 0,
                        fx = settings.d.fx,
                        fy = settings.d.fy,
                        cx = settings.d.cx,
                        cy = settings.d.cy,
                        p1 = 0,
                        p2 = 0,
                    },
                    lensFacing = (int)LensFacing.Back,
                    name = "ARFoundation",
                    cameraId = cameraId,
                    colorCameraId = cameraId,
                    isDepthPrivate = false,
                };

                Started = true;
                return conf;
            }
            else
            {
                return selectedConfig;
            }
        }

        private void SetStreamParameters(CalibrationSettingsProperty settings)
        {
            streamParametersProperty.fx = settings.d.fx;
            streamParametersProperty.fy = settings.d.fy;
            streamParametersProperty.cx = settings.d.cx;
            streamParametersProperty.cy = settings.d.cy;
            streamParametersProperty.width = width;
            streamParametersProperty.height = height;
            streamParametersProperty.isDepthSixteen = true;
            streamParametersProperty.useExternalFunctionStream = true;
            streamParametersProperty.useExternalFunctionCallback = true;
            streamParametersProperty.functionPointer = (long)System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(tofDelegate);
            try
            {
                TofArTofManager.Instance.SetProperty(streamParametersProperty);
            }
            catch (SensCord.ApiException)
            {
                TofArTofManager.Instance.StopStream();
                TofArTofManager.Instance.SetProperty(streamParametersProperty);
            }
        }

        private void SetIntrinsics(CalibrationSettingsProperty settings, CameraConfigurationProperty selectedConfig)
        {
            Camera2IntrinsicsProperty intrinsics = new Camera2IntrinsicsProperty
            {
                cx = settings.d.cx,
                cy = settings.d.cy,
                fx = settings.d.fx,
                fy = settings.d.fy,
                k1 = selectedConfig?.intrinsics?.k1 ?? 0,
                k2 = selectedConfig?.intrinsics?.k2 ?? 0,
                k3 = selectedConfig?.intrinsics?.k3 ?? 0,
                p1 = selectedConfig?.intrinsics?.p1 ?? 0,
                p2 = selectedConfig?.intrinsics?.p2 ?? 0
            };
            TofArTofManager.Instance.SetProperty<Camera2IntrinsicsProperty>(intrinsics);
        }

        private string GetCameraId()
        {
            var deviceAttsProperty = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();

            deviceAttributesJson devAtts = null;
            if (deviceAttsProperty != null)
            {
                devAtts = JsonUtility.FromJson<deviceAttributesJson>(deviceAttsProperty.TrimmedDeviceAttributesString);
            }

            return devAtts?.fixedDepthCameraId ?? "0";
        }

        public void StopStream()
        {
            if (Started)
            {
                Started = false;
            }
        }

        private void TrySetHeightAndWidth()
        {
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == V0.RunMode.Default)
            {
                if (occlusionManager.TryAcquireEnvironmentDepthCpuImage(out XRCpuImage occlusionImage))
                {
                    width = occlusionImage.width;
                    height = occlusionImage.height;
                    occlusionImage.Dispose();
                }
#if UNITY_IOS
                if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage cpuImage))
                {
                    widthColor = cpuImage.width;
                    heightColor = cpuImage.height;
                    cpuImage.Dispose();
                }
                if (cameraManager.TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics))
                {
                    fx = cameraIntrinsics.focalLength.x;
                    fy = cameraIntrinsics.focalLength.y;
                    cx = cameraIntrinsics.principalPoint.x;
                    cy = cameraIntrinsics.principalPoint.y;
                }
#endif
            }
        }


        public IEnumerator WaitForStreamStart()
        {
            TrySetHeightAndWidth();

            var runMode = TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode;

#if UNITY_IOS
            while (width == 0 || height == 0 || (runMode == V0.RunMode.Default && (fx == 0 || fy == 0 || cx == 0 || cy == 0)) )
#else
            while (width == 0 || height == 0)
#endif
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Tof waiting for width {width} height {height} to be set");
                yield return new WaitForEndOfFrame();
                TrySetHeightAndWidth();
            }
        }

        public int GetStreamDelay(TofArTofManager.deviceCameraSettingsJson cameraSettings)
        {
            //check if the ARImage is on
            var arimageManager = FindObjectOfType<ARTrackedImageManager>();
            if (arimageManager?.enabled == true)
            {
                return cameraSettings.depthDelayARFoundationARImage;
            }

            return cameraSettings.depthDelayARFoundation;
        }
    }
}

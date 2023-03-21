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
using TofAr.V0.Tof;
using TofAr.V0;
using System;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    public class DummyTofConnector : MonoBehaviour, IExternalStreamSource
    {
        public bool Started { get; set; } = false;
        private FunctionStreamParametersProperty streamParametersProperty = new FunctionStreamParametersProperty();
        private static TofFunctionStreamDelegate tofDelegate;
        public CameraConfigurationProperty StartStream(CameraConfigurationProperty selectedConfig)
        {
            if (TofAr.V0.TofArManager.Instance.RuntimeSettings.runMode == TofAr.V0.RunMode.Default)
            {
                if (tofDelegate == null)
                {
                    tofDelegate = new TofFunctionStreamDelegate(TofARFoundationCallback);
                }
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Starting ARFoundationTofConnector");
                var deviceAttsProperty = TofArManager.Instance.GetProperty<DeviceCapabilityProperty>();

                string cameraId = "0";

                var settings = TofArTofManager.Instance.LoadSettings(cameraId, 0, 0, false);

                TofArTofManager.Instance.CalibrationSettings = settings;
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

                streamParametersProperty.fx = settings.d.fx;
                streamParametersProperty.fy = settings.d.fy;
                streamParametersProperty.cx = settings.d.cx;
                streamParametersProperty.cy = settings.d.cy;
                streamParametersProperty.width = 0;
                streamParametersProperty.height = 0;
                streamParametersProperty.isDepthSixteen = false;
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

                var conf = new Camera2ConfigurationProperty
                {
                    uid = -9997, //special value indicating function stream
                    height = 0,
                    width = 0,
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
                    name = "Dummy-ARKitBody",
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

        public void StopStream()
        {
            if (Started)
            {
                Started = false;
            }
        }
        public IEnumerator WaitForStreamStart()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            TofArTofManager.Instance.SetProperty(StartStream(null));
        }



        private static Int64 lastTimestamp = 0;
        [AOT.MonoPInvokeCallback(typeof(TofFunctionStreamDelegate))]
        public static bool TofARFoundationCallback(ref Int64 timeStamp, IntPtr shortArray)
        {

            if (shortArray == IntPtr.Zero)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "function stream output array was not allocated");
                return false;
            }

            timeStamp = lastTimestamp++;
            return true;

        }

        public int GetStreamDelay(TofArTofManager.deviceCameraSettingsJson cameraSettings)
        {
            return 0;
        }
    }
}


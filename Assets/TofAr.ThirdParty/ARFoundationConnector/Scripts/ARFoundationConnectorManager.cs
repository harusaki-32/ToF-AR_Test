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
using UnityEngine.XR.ARFoundation;
using TofAr.V0;
using TofAr.V0.Tof;

namespace TofAr.ThirdParty.ARFoundationConnector
{

    public class ARFoundationConnectorManager : MonoBehaviour
    {
        [SerializeField]
        private ARSession arSession = null;

        [SerializeField]
        private ARHumanBodyManager humanBodyManager = null;

        [SerializeField]
        private ARFaceManager faceManager = null;

        [SerializeField]
        private ARCameraManager cameraManager = null;

        [SerializeField]
        private DummyTofConnector dummyTofConnector = null;

        [SerializeField]
        private ARFoundationTofConnector tofConnector = null;

        [SerializeField]
        private ARFoundationBodyConnector bodyConnector = null;

        [SerializeField]
        private ARFoundationFaceConnector faceConnector = null;

        [SerializeField]
        private ARFoundationColorConnector colorConnector = null;

        [SerializeField]
        private ARFoundationSegmentationConnector segmentationConnector = null;

        [SerializeField]
        private bool useARFoundationBody = false;
        public bool UseARFoundationBody
        {
            get => useARFoundationBody;
            set
            {
                useARFoundationBody = value;
                SwitchARFoundationManagerUsage();
            }
        }

        [SerializeField]
        private bool useARFoundationFace = false;
        public bool UseARFoundationFace
        {
            get => useARFoundationFace;
            set
            {
                useARFoundationFace = value;
                SwitchARFoundationManagerUsage();
            }
        }

        [SerializeField]
        private bool autoStart = false;
        public bool AutoStart
        {
            get => autoStart;
            set
            {
                autoStart = value;

            }
        }

        private void SetAutostarts()
        {
            tofConnector.autoStart = autoStart;
            colorConnector.autoStart = autoStart;
            bodyConnector.autoStart = autoStart;
            faceConnector.autoStart = autoStart;
        }

        private void OnEnable()
        {
            
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                var runtimeProp = TofArManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeProp.isUsingArFoundation = true;
                TofAr.V0.TofArManager.Instance.SetProperty(runtimeProp);
            }
            
            SwitchARFoundationManagerUsage();
            SetAutostarts();
        }

        private void OnDisable()
        {
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                var runtimeProp = TofArManager.Instance.GetProperty<RuntimeSettingsProperty>();
                runtimeProp.isUsingArFoundation = false;
                TofAr.V0.TofArManager.Instance.SetProperty(runtimeProp);
            }
        }

        private void SwitchARFoundationManagerUsage()
        {
#if UNITY_IOS
            humanBodyManager.enabled = useARFoundationBody && !useARFoundationFace;
            faceManager.enabled = useARFoundationFace && !useARFoundationBody;
            cameraManager.requestedFacingDirection = (useARFoundationFace && !useARFoundationBody) ? CameraFacingDirection.User : CameraFacingDirection.World;
            bodyConnector.enabled = useARFoundationBody && !useARFoundationFace;
            faceConnector.enabled = useARFoundationFace && !useARFoundationBody;
            tofConnector.occlusionManager.enabled = !useARFoundationBody && !useARFoundationFace;
            tofConnector.enabled = !useARFoundationBody && !useARFoundationFace;
            segmentationConnector.enabled = !useARFoundationBody && !useARFoundationFace;
            dummyTofConnector.enabled = useARFoundationBody || useARFoundationFace;
            if(useARFoundationBody || useARFoundationFace)
            {
                dummyTofConnector.StartStream(TofArTofManager.Instance.GetProperty<Camera2ConfigurationProperty>());
            }
            else
            {
                tofConnector.StartStream(TofArTofManager.Instance.GetProperty<Camera2ConfigurationProperty>());
            }
#else
            humanBodyManager.enabled = false;
            bodyConnector.enabled = false;
            tofConnector.enabled = false;
            dummyTofConnector.enabled = false;
            segmentationConnector.enabled = false;
            faceManager.enabled = useARFoundationFace;
            cameraManager.requestedFacingDirection = useARFoundationFace ? CameraFacingDirection.User : CameraFacingDirection.World;
            faceConnector.enabled = useARFoundationFace;
#endif
            arSession.requestedTrackingMode = useARFoundationFace ? TrackingMode.RotationOnly : TrackingMode.PositionAndRotation;
        }
    }
}

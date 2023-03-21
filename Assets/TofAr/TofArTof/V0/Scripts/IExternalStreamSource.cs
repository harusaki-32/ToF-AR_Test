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

using TofAr.V0.Tof;
using System.Collections;

/// <summary>
/// *TODO+ B
/// 外部のDepthストリームソース
/// </summary>
public interface IExternalStreamSource
{
    //external sources may need to edit the configuration to use the C++ side external stream source
    CameraConfigurationProperty StartStream(CameraConfigurationProperty selectedConfig);

    //any prep that might take longer than a single frame goes here
    IEnumerator WaitForStreamStart();
    void StopStream();

    int GetStreamDelay(TofArTofManager.deviceCameraSettingsJson cameraSettings);
}

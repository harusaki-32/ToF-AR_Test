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
#if UNITY_EDITOR

using System;
using TofAr.V0;
using UnityEditor;

[InitializeOnLoad]

public class SetupDllsAndSysEnvs
{
    static SetupDllsAndSysEnvs()
    {
        // check editor prefs

        int currentTof = EditorPrefs.GetInt("tof", -1);
        if (currentTof < 0 || currentTof > 1)
        {
            EditorPrefs.SetInt("tof", 1); // 1: imx316/camera2
        }

        EditorApplication.playModeStateChanged += PlayModeChanged;

        TcpForward();

#if !UNITY_2019_1_OR_NEWER
        EditorApplication.delayCall = () =>
        {
            EditorApplication.ExecuteMenuItem("Edit/Graphics Emulation/No Emulation");
        };
#endif
    }

    private static void PlayModeChanged(PlayModeStateChange state)
    {
        TofArManager.Logger.WriteLog(LogLevel.Debug, state.ToString());
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            TcpForward();
        }
    }

    [MenuItem("ToF AR / TCP Forward")]
    private static void TcpForward()
    {
        DeviceType deviceType = (DeviceType)EditorPrefs.GetInt("DeviceType");
        if (deviceType == DeviceType.Android)
        {
            var src = "tcp:9999";
            var dst = "tcp:8080";
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Forward, src, dst);
            }
            catch (InvalidOperationException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to tcp forward. {e.Message}");
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to tcp forward. {e.Message}");
            }
        }
    }

    [UnityEditor.InitializeOnLoadMethod]
    static void EditorInitialize()
    {
        //UnityEngine.Debug.Log("EdiorSettings enterPlayModeOptions will change.");
        EditorSettings.enterPlayModeOptionsEnabled = true;
        EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
    }
}

#endif

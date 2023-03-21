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
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TofAr.V0
{
    public static class EditorUtils
    {
        public static string ReadOutput(System.IO.StreamReader reader)
        {
            return Utils.ReadOutput(reader);
        }

        public static string GetAdbPath()
        {
            var adbPath = System.IO.Path.GetFullPath(EditorPrefs.GetString("AndroidSdkRoot"));

            if (!string.IsNullOrEmpty(adbPath))
            {
                adbPath += "/platform-tools/adb" + ((Application.platform == RuntimePlatform.WindowsEditor) ? ".exe" : string.Empty);
            }
            return adbPath;
        }

        public static void InvokeAdbCommand(TofArManager.AdbCommand command, params string[] arguments)
        {
            var adbPath = EditorUtils.GetAdbPath();
#if UNITY_EDITOR_WIN
                if (!adbPath.EndsWith("adb.exe"))
                {
                    throw new InvalidOperationException("Invalid adb file");
                }
#endif
            Utils.InvokeAdbCommand(adbPath, command, arguments);
        }
    }
}
#endif

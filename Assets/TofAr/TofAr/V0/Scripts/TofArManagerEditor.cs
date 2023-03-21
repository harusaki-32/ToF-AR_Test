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
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace TofAr.V0
{
    /// <summary>
    /// TODO+ C UnityEdtorで動作する内部処理用なので記載しなくて良いかも
    /// </summary>
    [InitializeOnLoad]
    public class DebugServerSettings
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public static bool firstTimeStart = false;

        static DebugServerSettings()
        {
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                firstTimeStart = true;
            }
        }
    }

    /// <summary>
    /// TODO+ C UnityEdtorで動作する内部処理用なので記載しなくて良いかも
    /// </summary>
    [CustomEditor(typeof(TofArManager))]
    public class TofArManagerEditor : Editor
    {
        private bool ipChanged = false;
        private bool firstTimeStart = false;
        private LogLevel selectedLogLevel = LogLevel.Silent;

        private void OnEnable()
        {
            string ipAddress = EditorPrefs.GetString("IP");
            if (ipAddress.Equals(""))
            {
                ipAddress = "127.0.0.1";
                EditorPrefs.SetString("IP", ipAddress);
            }
            string port = EditorPrefs.GetString("Port");
            if (port.Equals(""))
            {
                port = "8080";
                EditorPrefs.SetString("Port", port);
            }
            int deviceType = EditorPrefs.GetInt("DeviceType");
            if (deviceType < 0 || deviceType > 1)
            {
                deviceType = 0;
                EditorPrefs.SetInt("DeviceType", deviceType);
            }
            int serverTimeout = EditorPrefs.GetInt("ServerTimeout", 10000);
            if (serverTimeout < 0)
            {
                serverTimeout = 10000;
                EditorPrefs.SetInt("ServerTimeout", 10000);
            }

            bool networkEnabled = EditorPrefs.GetBool("NetworkDebug");

            TofArManager myScript = (TofArManager)target;

            this.serializedObject.Update();

            myScript.enableNetworkDebugging = networkEnabled;
            myScript.debugServerIpAddress = ipAddress;
            myScript.debugServerPort = port;
            myScript.debugServerDevice = (DeviceType)deviceType;
            myScript.serverConnectionTimeout = serverTimeout;
        }

        /// <summary>
        /// TODO+ C
        /// </summary>
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            TofArManager myScript = (TofArManager)target;

            this.serializedObject.Update();

            bool enableNetworkDebugging = EditorGUILayout.Toggle("Enable network debugging", myScript.enableNetworkDebugging);

            if (enableNetworkDebugging != myScript.enableNetworkDebugging)
            {
                ipChanged = true;
                EditorPrefs.SetBool("NetworkDebug", enableNetworkDebugging);
            }

            myScript.enableNetworkDebugging = enableNetworkDebugging;

            if (myScript.enableNetworkDebugging)
            {
                string ipAddress = (string)EditorGUILayout.TextField("Debug Server IP Address", myScript.debugServerIpAddress);
                if (ipAddress != myScript.debugServerIpAddress)
                {
                    ipChanged = true;
                    EditorPrefs.SetString("IP", ipAddress);
                }

                myScript.debugServerIpAddress = ipAddress;

                string port = (string)EditorGUILayout.TextField("Debug Server Port", myScript.debugServerPort);
                if (port != myScript.debugServerPort)
                {
                    ipChanged = true;
                    EditorPrefs.SetString("Port", port);
                }

                myScript.debugServerPort = port;

            }

            DeviceType devType = (DeviceType)EditorGUILayout.EnumPopup("Debug Server Device", myScript.debugServerDevice);
            myScript.debugServerDevice = devType;
            EditorPrefs.SetInt("DeviceType", (int)devType);


            int serverTimeout = EditorGUILayout.IntField("Server Connection Timeout", myScript.serverConnectionTimeout);
            if (serverTimeout != myScript.serverConnectionTimeout)
            {
                ipChanged = true;
                EditorPrefs.SetInt("ServerTimeout", serverTimeout);
            }
            myScript.serverConnectionTimeout = serverTimeout;


            firstTimeStart = DebugServerSettings.firstTimeStart;

            if (ipChanged && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(myScript);
                }
                if (firstTimeStart)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.HelpBox(string.Format("Debug server network settings have been changed. Unity Editor needs to be restarted for changes to take effect."), MessageType.Warning);
                }
                else
                {
                    ipChanged = false;
                }

            }

#if UNITY_IOS
            GUILayout.Space(10);
            GUILayout.Label("-- iOS Platform Settings --");

            var iosFramerate = serializedObject.FindProperty("iOSSessionFramerate");

            int newFramerate = EditorGUILayout.IntField("Session Framerate", iosFramerate.intValue);
            if (newFramerate != iosFramerate.intValue)
            {
                iosFramerate.intValue = newFramerate;
            }
#endif

#if !UNITY_IOS
            GUILayout.Space(16);
            GUILayout.Label("-- Device Profile Update Tool --");

            DrawDeviceProfilePicker(myScript);


            if (GUILayout.Button("Push to Device"))
            {
                this.PushDeviceAttribute(myScript.deviceProfile);
            }
            if (GUILayout.Button("Pull from Device"))
            {
                this.PullDeviceAttribute();
            }
            if (GUILayout.Button("Remove from Device"))
            {
                this.RemoveDeviceAttribute();
            }

            GUILayout.Space(16);
            GUILayout.Label("-- Log Setting Tool --");
            this.selectedLogLevel = (LogLevel)EditorGUILayout.EnumPopup("LogLevel", this.selectedLogLevel);
            if (GUILayout.Button("Apply to Device"))
            {
                this.PushLogSettings(this.selectedLogLevel);
            }
            if (GUILayout.Button("Remove from Device"))
            {
                this.RemoveLogSettings();
            }
#else
            GUILayout.Space(16);
            GUILayout.Label("-- Log Setting Tool --");
            EditorGUILayout.HelpBox(
                $"Not supported on iOS Platform.\n\nYou can change LogLevel following step.\n" +
                "1.Create a text file named 'tofar_log_settings'\n" +
                "2.Specify LogLevel like 'LogLevel=Debug' on 1st line of the file \n" +
                "3.Push that file to root of application's document folder with Finder or iTunes"
                , MessageType.None);
#endif



            this.serializedObject.ApplyModifiedProperties();


            EditorGUILayout.Space();
            EditorGUILayout.HelpBox($"ToF AR Version : v{myScript.Version}", MessageType.None);
        }

        private void DrawDeviceProfilePicker(TofArManager manager)
        {
            {
                var guiContent = EditorGUIUtility.ObjectContent(manager.deviceProfile, typeof(TextAsset));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Device Profile", GUILayout.Width(EditorGUIUtility.labelWidth));
                if (Event.current.commandName == "ObjectSelectorUpdated")
                {
                    var newText = EditorGUIUtility.GetObjectPickerObject();

                    manager.deviceProfile = (TextAsset)newText;
                }

                var style = new GUIStyle("TextField");
                style.fixedHeight = 16;
                style.margin = new RectOffset(0, 21, 0, 0);
                style.imagePosition = manager.deviceProfile ? ImagePosition.ImageLeft : ImagePosition.TextOnly;

                EditorGUILayout.BeginVertical();
                if (GUILayout.Button(guiContent, style, GUILayout.MinWidth(20)) && manager.deviceProfile)
                {
                    EditorGUIUtility.PingObject(manager.deviceProfile);
                }
                Rect dragRect = GUILayoutUtility.GetLastRect();
                dragRect.x += 18;
                if (GUI.Button(dragRect, "", GUI.skin.GetStyle("IN ObjectField")))
                {
                    int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
                    EditorGUIUtility.ShowObjectPicker<TextAsset>(manager.deviceProfile, false, "_v", controlID);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();


                if (DragAndDrop.objectReferences.Length > 0)
                {
                    if (dragRect.Contains(Event.current.mousePosition))
                    {
                        var objRef = DragAndDrop.objectReferences[0];

                        if (objRef.GetType() == typeof(TextAsset))
                        {
                            if (Event.current.type == EventType.DragUpdated)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                Event.current.Use();
                            }
                            else if (Event.current.type == EventType.DragPerform)
                            {
                                manager.deviceProfile = (TextAsset)objRef;
                                Event.current.Use();
                            }
                        }
                    }
                }

            }
        }

        private void PushDeviceAttribute(TextAsset deviceProfile)
        {
            var temporaryPath = System.IO.Path.GetFullPath(FileUtil.GetUniqueTempPathInProject());
            Directory.CreateDirectory(temporaryPath);
            var filePath = $"{temporaryPath}{Path.DirectorySeparatorChar}{TofArManager.DeviceAttributesFileName}";
            File.WriteAllBytes(filePath, deviceProfile.bytes);

            var src = filePath;
            var dst = TofArManager.SpecialDeviceAttributesFilePath;
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Push, src, dst);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"{deviceProfile.name} pushed.");
            }
            catch (InvalidOperationException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }

        private void PullDeviceAttribute()
        {
            var src = TofArManager.SpecialDeviceAttributesFilePath;
            var dst = $"{Application.temporaryCachePath}{Path.DirectorySeparatorChar}TofAr{Path.DirectorySeparatorChar}{TofArManager.DeviceAttributesFileName}";
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Pull, src, dst);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"{dst} pulled.");
            }
            catch (InvalidOperationException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }

        private void RemoveDeviceAttribute()
        {
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Remove, TofArManager.SpecialDeviceAttributesFilePath);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"{TofArManager.SpecialDeviceAttributesFilePath} removed.");
            }
            catch (InvalidOperationException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }

        private void PushLogSettings(LogLevel logLevel)
        {
            var temporaryPath = System.IO.Path.GetFullPath(FileUtil.GetUniqueTempPathInProject());
            Directory.CreateDirectory(temporaryPath);
            var filePath = $"{temporaryPath}{Path.DirectorySeparatorChar}tofar_log_settings";
            File.WriteAllText(filePath, $"LogLevel={logLevel}");

            var src = filePath;
            var dst = Logger.SettingsFilePath;
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Push, src, dst);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Log setting file pushed.");
            }
            catch (InvalidOperationException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }

        private void RemoveLogSettings()
        {
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Remove, Logger.SettingsFilePath);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Log setting file removed.");
            }
            catch (InvalidOperationException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }
    }
}
#endif

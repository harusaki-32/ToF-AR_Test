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


namespace TofAr.V0.Tof.DepthPrivateFilter
{
    [CustomEditor(typeof(FilterController))]
    public class FilterControllerrEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();
            this.serializedObject.Update();

            var controller = (this.target as FilterController);

            GUILayout.Space(16);
            GUILayout.Label("-- Data.zip Update Tool --");

            DrawDeviceProfilePicker(controller);


            if (GUILayout.Button("Push to Device"))
            {
                this.PushDataZip(controller?.dataZip);
            }
            if (GUILayout.Button("Pull from Device"))
            {
                this.PullDataZip();
            }
            if (GUILayout.Button("Remove from Device"))
            {
                this.RemoveDataZip();
            }
        }

        private void DrawDeviceProfilePicker(FilterController controller)
        {
            {
                var guiContent = EditorGUIUtility.ObjectContent(controller.dataZip, typeof(DefaultAsset));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Data.zip", GUILayout.Width(EditorGUIUtility.labelWidth));
                if (Event.current.commandName == "ObjectSelectorUpdated")
                {
                    var newItem = EditorGUIUtility.GetObjectPickerObject();
                    if (newItem == null)
                    {
                        controller.dataZip = null;
                    }
                    else
                    {
                        if (newItem.GetType() == typeof(DefaultAsset))
                        {
                            controller.dataZip = (newItem as DefaultAsset);
                        }
                        else if (newItem.GetType() == typeof(TextAsset))
                        {
                            // check if it's zip file
                            if (IsZipFile(newItem))
                            {
                                controller.dataZip = (newItem as TextAsset);
                            }
                        }
                        else
                        {
                            controller.dataZip = null;
                        }
                    }
                    
                }

                var style = new GUIStyle("TextField");
                style.fixedHeight = 16;
                style.margin = new RectOffset(0, 21, 0, 0);
                style.imagePosition = controller.dataZip ? ImagePosition.ImageLeft : ImagePosition.TextOnly;

                EditorGUILayout.BeginVertical();
                if (GUILayout.Button(guiContent, style, GUILayout.MinWidth(20)) && controller.dataZip)
                {
                    EditorGUIUtility.PingObject(controller.dataZip);
                }

                Rect dragRect = GUILayoutUtility.GetLastRect();
                dragRect.x += 18;
                if (GUI.Button(dragRect, "", GUI.skin.GetStyle("IN ObjectField")))
                {
                    int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
                    EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>(controller.dataZip, false, "data", controlID);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();


                if (DragAndDrop.objectReferences.Length > 0)
                {
                    if (dragRect.Contains(Event.current.mousePosition))
                    {
                        var objRef = DragAndDrop.objectReferences[0];

                        if (objRef.GetType() == typeof(DefaultAsset))
                        {
                            if (Event.current.type == EventType.DragUpdated)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                Event.current.Use();
                            }
                            else if (Event.current.type == EventType.DragPerform)
                            {
                                controller.dataZip = (objRef as DefaultAsset);
                                Event.current.Use();
                            }
                        }
                        else if (objRef.GetType() == typeof(TextAsset))
                        {
                            // check if it's zip file
                            if (!IsZipFile(objRef))
                            {
                                return;
                            }

                            if (Event.current.type == EventType.DragUpdated)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                Event.current.Use();
                            }
                            else if (Event.current.type == EventType.DragPerform)
                            {
                                controller.dataZip = (objRef as TextAsset);
                                Event.current.Use();
                            }
                        }
                    }
                }

            }
        }

        private bool IsZipFile(UnityEngine.Object obj)
        {
            TextAsset textasset = obj as TextAsset;
            if (textasset == null)
            {
                textasset = obj as TextAsset;
                if (textasset == null)
                {
                    return false;
                }
                return false;
            }
            var assetBytes = textasset.bytes;
            if (assetBytes.Length < 4)
            {
                return false;
            }
            // zip format starts with 0x04034b50
            if (assetBytes[0] != 80 || assetBytes[1] != 75 || assetBytes[2] != 3 || assetBytes[3] != 4)
            {
                return false;
            }

            return true;
        }

        private void PushDataZip(UnityEngine.Object dataZip)
        {
            if (dataZip == null)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, "No file selected");
                return;
            }

            var temporaryPath = Path.GetFullPath(FileUtil.GetUniqueTempPathInProject());
            Directory.CreateDirectory(temporaryPath);
            var tmpFilePath = $"{temporaryPath}{Path.DirectorySeparatorChar}{FilterController.dataFileName}";
            var assetFilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{AssetDatabase.GetAssetPath(dataZip.GetInstanceID())}";
            File.WriteAllBytes(tmpFilePath, File.ReadAllBytes(assetFilePath));

            var arguments = new string[] { tmpFilePath, FilterController.dataPath };
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Push, arguments);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"{dataZip.name} pushed.");
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }

        private void PullDataZip()
        {
            var filePath = $"{Application.temporaryCachePath}{Path.DirectorySeparatorChar}TofAr{Path.DirectorySeparatorChar}{FilterController.dataFileName}";
            var arguments = new string[] { FilterController.dataPath, filePath };
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Pull, arguments);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"{filePath} pulled.");
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }

        private void RemoveDataZip()
        {
            var arguments = new string[] { FilterController.dataPath };
            try
            {
                EditorUtils.InvokeAdbCommand(TofArManager.AdbCommand.Remove, arguments);
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"{FilterController.dataPath} removed.");
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Error, e.Message);
            }
        }
    }
}
#endif

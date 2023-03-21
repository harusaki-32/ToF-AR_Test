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
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System;

namespace TofAr.V0.Face
{
    [CustomEditor(typeof(TofArFacialExpressionEstimator))]
    public class TofArFacialExpressionEstimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TofArFacialExpressionEstimator myScript = (TofArFacialExpressionEstimator)target;

            this.serializedObject.Update();

            var autoModeSelect = this.serializedObject.FindProperty("autoModeSelect");

            if (!autoModeSelect.boolValue)
            {
                var aiueoDataListIndex = this.serializedObject.FindProperty("aiueoDataListIndex");
                var newAiueoMode = EditorGUILayout.IntField("AIUEO Data Index", aiueoDataListIndex.intValue);

                if (newAiueoMode != aiueoDataListIndex.intValue)
                {
                    aiueoDataListIndex.intValue = newAiueoMode;
                }
            }

            this.serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(myScript);
                EditorSceneManager.MarkSceneDirty(myScript.gameObject.scene);
            }
        }
    }
}
#endif

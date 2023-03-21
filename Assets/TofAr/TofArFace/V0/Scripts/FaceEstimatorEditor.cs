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

namespace TofAr.V0.Face
{
    [CustomEditor(typeof(FaceEstimator))]
    public class FaceEstimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            FaceEstimator myScript = (FaceEstimator)target;

            this.serializedObject.Update();

            myScript.TargetPlatform = (TargetPlatform) EditorGUILayout.EnumFlagsField("Target Platform", myScript.TargetPlatform);

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

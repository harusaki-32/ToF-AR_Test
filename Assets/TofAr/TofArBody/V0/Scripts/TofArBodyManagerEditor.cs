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


namespace TofAr.V0.Body
{
    /// <summary>
    /// TODO+ C UnityEdtorで動作する内部処理用なので記載しなくて良いかも
    /// </summary>
    [CustomEditor(typeof(TofArBodyManager))]
    public class TofArBodyManagerEditor : Editor
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TofArBodyManager myScript = (TofArBodyManager)target;

            this.serializedObject.Update();

            if (!myScript.RuntimeModeAutoSet)
            {
                var runtimeModeProperty = this.serializedObject.FindProperty("runtimeMode");

                var runtimeValues = (SV2.RuntimeMode[])Enum.GetValues(typeof(SV2.RuntimeMode));
                var runtimeMode = runtimeValues[runtimeModeProperty.enumValueIndex];

                SV2.RuntimeMode newRuntimeMode = (SV2.RuntimeMode)EditorGUILayout.EnumPopup("SV2 Runtime Mode", runtimeMode);
                if (newRuntimeMode != myScript.RuntimeMode)
                {
                    runtimeModeProperty.enumValueIndex = (int)newRuntimeMode;
                }
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(myScript);
            }


            this.serializedObject.ApplyModifiedProperties();
        }

    }
}
#endif

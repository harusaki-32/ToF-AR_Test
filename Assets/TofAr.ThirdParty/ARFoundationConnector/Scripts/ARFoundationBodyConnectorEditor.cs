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

namespace TofAr.ThirdParty.ARFoundationConnector
{
    [CustomEditor(typeof(ARFoundationBodyConnector))]
    public class ARFoundationBodyConnectorEditor : Editor
    {
#if UNITY_ANDROID
        private bool hasMessagedLog = false;
#endif
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

#if UNITY_ANDROID
            EditorGUILayout.HelpBox(
                $"ARFoundation Body detection Not supported on Android Platform.\nPlease remove the connector or switch to iOS.\n"
                , MessageType.Error);
            if (!hasMessagedLog)
            {
                hasMessagedLog = true;
                Debug.LogWarning(
                    $"ARFoundation Body detection Not supported on Android Platform.\nPlease remove the connector or switch to iOS.\n");
            }
#endif

            this.serializedObject.ApplyModifiedProperties();
        }

    }
}
#endif

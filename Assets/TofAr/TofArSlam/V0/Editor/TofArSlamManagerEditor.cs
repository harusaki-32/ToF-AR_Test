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

namespace TofAr.V0.Slam
{
    /// <summary>
    /// TODO+ C UnityEdtorで動作する内部処理
    /// </summary>
    [CustomEditor(typeof(TofArSlamManager))]
    public class TofArSlamManagerEditor : Editor
    {
        private bool isFirst = true;
        private CameraPoseSource prevCameraPoseSource = CameraPoseSource.Gyro;
        private bool prevAutoAssignTracker = false;

        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            var targetComponent = target as TofArSlamManager;
            if (targetComponent == null)
            {
                return;
            }
            this.serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            if (this.isFirst || (this.prevAutoAssignTracker != targetComponent.autoAssignTracker) || (this.prevCameraPoseSource != targetComponent.CameraPoseSource))
            {
                if (targetComponent.autoAssignTracker)
                {
                    var prefab = Resources.Load<GameObject>(targetComponent.presetTrackers[targetComponent.CameraPoseSource]);
                    targetComponent.trackerObject = prefab;
                }
            }

            var trackerObjectProperty = this.serializedObject.FindProperty("trackerObject");
            GUI.enabled = !targetComponent.autoAssignTracker;
            EditorGUILayout.PropertyField(trackerObjectProperty);
            GUI.enabled = true;

            this.serializedObject.ApplyModifiedProperties();

            this.prevAutoAssignTracker = targetComponent.autoAssignTracker;
            this.prevCameraPoseSource = targetComponent.CameraPoseSource;

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(targetComponent);
            }
        }
    }
}

#endif

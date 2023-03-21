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

namespace TofAr.V0.Color
{
    /// <summary>
    /// TofArColorManager Editor Class
    /// </summary>
    [CustomEditor(typeof(TofArColorManager))]
    public class TofArColorManagerEditor : Editor
    {
        private bool prevIsProcessTexture = true;
        private bool prevProcessTexture = true;

        private void OnEnable()
        {
            var script = (this.target as TofArColorManager);
            if (script != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.prevIsProcessTexture = script.IsProcessTexture;
#pragma warning restore CS0618 // Type or member is obsolete
                this.prevProcessTexture = script.ProcessTexture;
            }
        }

        /// <summary>
        /// Draw Inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            var script = (this.target as TofArColorManager);

            

            EditorGUILayout.HelpBox("\"Is Process Texture\" is deprecated, please use \"Process Texture\" instead", MessageType.Warning);
            DrawDefaultInspector();

            var useDefaultStreamDelay = this.serializedObject.FindProperty("useDefaultStreamDelay");
            var streamDelay = this.serializedObject.FindProperty("streamDelay");

            bool autoDelay = EditorGUILayout.Toggle("Use Default Stream Delay", useDefaultStreamDelay.boolValue);

            bool delayChanged = autoDelay != useDefaultStreamDelay.boolValue;
            if (delayChanged)
            {
                useDefaultStreamDelay.boolValue = autoDelay;
            }

            if (!autoDelay && script != null)
            {
                int newStreamDelay = EditorGUILayout.IntSlider("Stream Delay", script.StreamDelay, 0, 10);
                if (newStreamDelay != script.StreamDelay)
                {
                    streamDelay.intValue = newStreamDelay;
                }
            }

            var propIsProcessTexture = this.serializedObject.FindProperty("isProcessTexture");
            var propProcessTexture = this.serializedObject.FindProperty("processTexture");
            var isProcessTextureChanged = this.prevIsProcessTexture != propIsProcessTexture.boolValue;
            var processTextureChanged = this.prevProcessTexture != propProcessTexture.boolValue;
            if (isProcessTextureChanged)
            {
                //Debug.Log($"IsProcessTexture is Changed");
                propProcessTexture.boolValue = propIsProcessTexture.boolValue;
            }

            if (processTextureChanged)
            {
                //Debug.Log($"ProcessTexture is Changed");
                propIsProcessTexture.boolValue = propProcessTexture.boolValue;
            }

            if (script != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.prevIsProcessTexture = script.IsProcessTexture;
#pragma warning restore CS0618 // Type or member is obsolete
                this.prevProcessTexture = script.ProcessTexture;
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif

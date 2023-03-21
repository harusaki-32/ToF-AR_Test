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

namespace TofAr.V0.Tof
{
    /// <summary>
    /// TofArTofManager Editor Class
    /// </summary>
    [CustomEditor(typeof(TofArTofManager))]
    public class TofArTofManagerEditor : Editor
    {
        private bool prevIsProcessDepth = true;
        private bool prevProcessDepth = true;

        private bool prevIsProcessConfidence = true;
        private bool prevProcessConfidence = true;

        private bool prevIsProcessPointCloud = true;
        private bool prevProcessPointCloud = true;

        private bool prevIsProcessTexture = true;
        private bool prevProcessTexture = true;

        private void OnEnable()
        {
            var script = (this.target as TofArTofManager);

            if (script != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.prevIsProcessDepth = script.isProcessDepth;
                this.prevIsProcessConfidence = script.isProcessConfidence;
                this.prevIsProcessPointCloud = script.isProcessPointCloud;
                this.prevIsProcessTexture = script.IsProcessTexture;
#pragma warning restore CS0618 // Type or member is obsolete
                this.prevProcessDepth = script.processDepth;
                this.prevProcessConfidence = script.processConfidence;
                this.prevProcessPointCloud = script.processPointCloud;
                this.prevProcessTexture = script.ProcessTexture;
            }
        }

        /// <summary>
        /// Draw Inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            var script = (this.target as TofArTofManager);


            EditorGUILayout.HelpBox(
                "\"Is Process Depth\" is deprecated, please use \"Process Depth\" instead\n" +
                "\"Is Process Confidence\" is deprecated, please use \"Process Confidence\" instead\n" +
                "\"Is Process PointCloud\" is deprecated, please use \"Process PointCloud\" instead\n" +
                "\"Is Process Texture\" is deprecated, please use \"Process Texture\" instead"
                , MessageType.Warning);
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

            ////

            var propIsProcessDepth = this.serializedObject.FindProperty("isProcessDepth");
            var propProcessDepth = this.serializedObject.FindProperty("processDepth");
            var isProcessDepthChanged = this.prevIsProcessDepth != propIsProcessDepth.boolValue;
            var processDepthChanged = this.prevProcessDepth != propProcessDepth.boolValue;
            if (isProcessDepthChanged)
            {
                //Debug.Log($"IsProcessDepth is Changed");
                propProcessDepth.boolValue = propIsProcessDepth.boolValue;
            }

            if (processDepthChanged)
            {
                //Debug.Log($"ProcessDepth is Changed");
                propIsProcessDepth.boolValue = propProcessDepth.boolValue;
            }
            if (script != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.prevIsProcessDepth = script.isProcessDepth;
#pragma warning restore CS0618 // Type or member is obsolete
                this.prevProcessDepth = script.processDepth;
            }

            ////

            var propIsProcessConfidence = this.serializedObject.FindProperty("isProcessConfidence");
            var propProcessConfidence = this.serializedObject.FindProperty("processConfidence");
            var isProcessConfidenceChanged = this.prevIsProcessConfidence != propIsProcessConfidence.boolValue;
            var processConfidenceChanged = this.prevProcessConfidence != propProcessConfidence.boolValue;
            if (isProcessConfidenceChanged)
            {
                //Debug.Log($"IsProcessConfidence is Changed");
                propProcessConfidence.boolValue = propIsProcessConfidence.boolValue;
            }

            if (processConfidenceChanged)
            {
                //Debug.Log($"ProcessConfidence is Changed");
                propIsProcessConfidence.boolValue = propProcessConfidence.boolValue;
            }
            if (script != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.prevIsProcessConfidence = script.isProcessConfidence;
#pragma warning restore CS0618 // Type or member is obsolete
                this.prevProcessConfidence = script.processConfidence;
            }
            ////

            var propIsProcessPointCloud = this.serializedObject.FindProperty("isProcessPointCloud");
            var propProcessPointCloud = this.serializedObject.FindProperty("processPointCloud");
            var isProcessPointCloudChanged = this.prevIsProcessPointCloud != propIsProcessPointCloud.boolValue;
            var processPointCloudChanged = this.prevProcessPointCloud != propProcessPointCloud.boolValue;
            if (isProcessPointCloudChanged)
            {
                //Debug.Log($"IsProcessPointCloud is Changed");
                propProcessPointCloud.boolValue = propIsProcessPointCloud.boolValue;
            }

            if (processPointCloudChanged)
            {
                //Debug.Log($"ProcessPointCloud is Changed");
                propIsProcessPointCloud.boolValue = propProcessPointCloud.boolValue;
            }
            if (script != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.prevIsProcessPointCloud = script.isProcessPointCloud;
#pragma warning restore CS0618 // Type or member is obsolete
                this.prevProcessPointCloud = script.processPointCloud;
            }
            ////

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

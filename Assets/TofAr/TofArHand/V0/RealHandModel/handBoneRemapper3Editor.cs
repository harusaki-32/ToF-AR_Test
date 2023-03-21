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
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
namespace TofAr.V0.Hand
{
    /// <summary>
    /// TODO+ C Edtor用なので
    /// </summary>
    [CustomEditor(typeof(handBoneRemapper3))]
    public class handBoneRemapper3Editor : Editor
    {
        private static string[] NAMES =
        {
            "pinky0",
            "pinky1",
            "pinky2",
            "pinky3",
            "pinky3_end",
            "ring0",
            "ring1",
            "ring2",
            "ring3",
            "ring3_end",
            "mid0",
            "mid1",
            "mid2",
            "mid3",
            "mid3_end",
            "index0",
            "index1",
            "index2",
            "index3",
            "index3_end",
            "thumb0",
            "thumb2",
            "thumb3",
            "thumb3_end",
            "wrist",
            "arm_end",
        };

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(16);
            GUILayout.Label("-- Auto assignment by name --");

            if (GUILayout.Button("Done"))
            {
                handBoneRemapper3 hbr = (handBoneRemapper3)target;
                hbr.modelJoints = new Transform[NAMES.Length];
                for (int i = 0; i < NAMES.Length; i++)
                {
                    Transform t = FindTransformByName(hbr.transform, NAMES[i]);
                    hbr.ModelJoints[i] = t;
                }
            }
        }

        private Transform FindTransformByName(Transform trans, string name)
        {
            GameObject obj = trans.gameObject;

            foreach (Transform child in obj.GetComponentsInChildren<Transform>())
            {
                if (child == obj.transform)
                {
                    continue;
                }
                string n0 = child.gameObject.name;
                if (n0 == name)
                {
                    return child;
                }
                else
                {
                    Transform t = FindTransformByName(child, name);
                    if (t != null)
                    {
                        return t;
                    }
                }
            }

            return null;
        }
    }
}
#endif

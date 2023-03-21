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

namespace TofAr.V0.MarkRecog
{
    /// <summary>
    /// TODO+ C　内部処理用 SDKからサンプルコードに移動しても良いかも
    /// </summary>
    public class MarkCameraTrackFollow : MonoBehaviour
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public HandBrush brush;

        /// <summary>
        /// TODO+ C
        /// </summary>
        public Transform parentTransform;

        Vector3 initialPositon;
        Quaternion initialRotation;

        // Use this for initialization
        void Start()
        {
            initialPositon = transform.localPosition;
            initialRotation = transform.localRotation;
            brush.DrawStarted += OnDrawStart;
            brush.DrawStopped += OnDrawStop;
        }

        private void OnDestroy()
        {
            if (brush != null)
            {
                brush.DrawStarted -= OnDrawStart;
                brush.DrawStopped -= OnDrawStop;
            }
        }

        private bool cullingMode;
        private void OnPreRender()
        {
            cullingMode = GL.invertCulling;
            GL.invertCulling = false;
        }

        private void OnPostRender()
        {
            GL.invertCulling = cullingMode;
        }

        void OnDrawStart()
        {
            this.transform.parent = parentTransform;
            this.transform.localRotation = initialRotation;
            this.transform.localPosition = initialPositon;
        }

        void OnDrawStop()
        {
            this.transform.parent = null;
        }
    }

}

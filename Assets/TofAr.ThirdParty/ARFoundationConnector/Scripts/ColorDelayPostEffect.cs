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

using System.Collections;
using UnityEngine;
using TofAr.V0.Color;
using TofAr.V0;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    public class ColorDelayPostEffect : MonoBehaviour
    {
        private RenderTexture[] textureBuffer = new RenderTexture[1];
        private int bufferPointer = 0;

        [SerializeField]
        ARFoundationColorConnector colorConnector;

        private ARTrackedImageManager tim;

        private bool? previousImageState;

        private IEnumerator Start()
        {
            tim = FindObjectOfType<ARTrackedImageManager>();
            previousImageState = tim?.enabled;
            yield return new WaitForEndOfFrame();
            TofArManager.Logger.WriteLog(LogLevel.Debug, "setting color delay to default");
            InitialiseStreamDelay();
        }

        void Update()
        {
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                UpdateBufferLengths();
            }
            if (tim?.enabled != previousImageState)
            {
                InitialiseStreamDelay();
                previousImageState = tim?.enabled;
            }
        }

        private void InitialiseStreamDelay()
        {
            TofArColorManager.Instance.SetStreamDelayToDefault(colorConnector);
        }


        private void UpdateBufferLengths()
        {
            int newLength = TofArColorManager.Instance.StreamDelay + 1;
            if (newLength != textureBuffer.Length)
            {
                System.Array.Resize(ref textureBuffer, newLength);
                if (bufferPointer >= newLength)
                {
                    bufferPointer = 0;
                }
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                if (bufferPointer >= textureBuffer.Length)
                {
                    bufferPointer = 0;
                }

                if (textureBuffer[bufferPointer] == null || textureBuffer[bufferPointer].width != source.width || textureBuffer[bufferPointer].height != source.height)
                {
                    textureBuffer[bufferPointer] = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
                }

                Graphics.Blit(source, textureBuffer[bufferPointer]);

                bufferPointer++;
                if (bufferPointer >= textureBuffer.Length)
                {
                    bufferPointer = 0;
                }
                if (textureBuffer[bufferPointer] == null)
                {
                    textureBuffer[bufferPointer] = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
                }
                else
                {
                    Graphics.Blit(textureBuffer[bufferPointer], destination);
                }
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }
    }
}

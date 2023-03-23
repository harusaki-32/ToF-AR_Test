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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TofAr.V0;
using TofAr.V0.Segmentation;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    public class ARFoundationSegmentationConnector : MonoBehaviour
    {
        [SerializeField]
        private AROcclusionManager occlusionManager;

        [SerializeField]
        private bool captureStencil, captureDepth;

        /// <summary>
        /// What level to run the segmentation mode at
        /// </summary>
        [Obsolete("StencilMode will be removed, Please use AROcclusionManager.requestedHumanStencilMode directly")]
        public HumanSegmentationStencilMode StencilMode
        {
            get => occlusionManager.requestedHumanStencilMode;
            set => occlusionManager.requestedHumanStencilMode = value;
        }

        /// <summary>
        /// Whether or not to send the human stencil frame to the segmentation component
        /// </summary>
        public bool CaptureStencil { get => captureStencil; set => captureStencil = value; }

        /// <summary>
        /// Whether or not to send the human depth frame to the segmentation component
        /// </summary>
        public bool CaptureDepth { get => captureDepth; set => captureDepth = value; }

        /// <summary>
        /// name used to identify the segmentation data
        /// </summary>
        public const string HumanDepthName = "HumanDepth";

        private const int arrayBufferLength = 5;

        private float[][] depthImages = new float[arrayBufferLength][];
        private GCHandle[] depthHandles = new GCHandle[arrayBufferLength];
        private byte[][] stencilImages = new byte[arrayBufferLength][];
        private GCHandle[] stencilHandles = new GCHandle[arrayBufferLength];
        private int depthImageIndex = 0, stencilImageIndex = 0;

        private void OnDestroy()
        {
            foreach (var h in depthHandles)
            {
                if (h.IsAllocated)
                {
                    h.Free();
                }
            }
            foreach (var h in stencilHandles)
            {
                if (h.IsAllocated)
                {
                    h.Free();
                }
            }
        }

        private void Update()
        {
            List<SegmentationResult> results = new List<SegmentationResult>();
            if (CaptureDepth)
            {
                if (occlusionManager.TryAcquireHumanDepthCpuImage(out XRCpuImage cpuImage))
                {
                    if (cpuImage.format == XRCpuImage.Format.DepthFloat32)
                    {
                        SegmentationResult depthres = new SegmentationResult();
                        depthres.name = HumanDepthName;
                        if (depthHandles[depthImageIndex].IsAllocated)
                        {
                            depthHandles[depthImageIndex].Free();
                        }
                        depthImages[depthImageIndex] = cpuImage.GetPlane(0).data.Reinterpret<float>(sizeof(byte)).ToArray();
                        depthHandles[depthImageIndex] = GCHandle.Alloc(depthImages[depthImageIndex], GCHandleType.Pinned);
                        depthres.maskBufferWidth = cpuImage.width;
                        depthres.maskBufferHeight = cpuImage.height;
                        if (TofArManager.Instance.RuntimeSettings.isRemoteServer)
                        {
                            depthres.dataStructureType = DataStructureType.MaskBufferFloat;
                            depthres.maskBufferFloat = depthImages[depthImageIndex];
                        }
                        else
                        {
                            depthres.dataStructureType = DataStructureType.RawPointer;
                            depthres.rawPointer = (ulong)depthHandles[depthImageIndex].AddrOfPinnedObject();
                        }
                        results.Add(depthres);
                        depthImageIndex++;
                        if (depthImageIndex >= depthImages.Length)
                        {
                            depthImageIndex = 0;
                        }
                    }
                    else
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, $"human depth image format was {cpuImage.format}");
                    }
                    cpuImage.Dispose();
                }
            }
            if (CaptureStencil)
            {
                if (occlusionManager.TryAcquireHumanStencilCpuImage(out XRCpuImage cpuImage))
                {
                    if (cpuImage.format == XRCpuImage.Format.OneComponent8)
                    {
                        SegmentationResult stencilRes = new SegmentationResult();
                        stencilRes.name = TofAr.V0.Segmentation.Human.HumanSegmentationDetector.ResultName;
                        if (stencilHandles[stencilImageIndex].IsAllocated)
                        {
                            stencilHandles[stencilImageIndex].Free();
                        }
                        stencilImages[stencilImageIndex] = cpuImage.GetPlane(0).data.ToArray();
                        stencilHandles[stencilImageIndex] = GCHandle.Alloc(stencilImages[stencilImageIndex], GCHandleType.Pinned);
                        stencilRes.maskBufferWidth = cpuImage.width;
                        stencilRes.maskBufferHeight = cpuImage.height;
                        if (TofArManager.Instance.RuntimeSettings.isRemoteServer)
                        {
                            stencilRes.dataStructureType = DataStructureType.MaskBufferByte;
                            stencilRes.maskBufferByte = stencilImages[stencilImageIndex];
                        }
                        else
                        {
                            stencilRes.dataStructureType = DataStructureType.RawPointer;
                            stencilRes.rawPointer = (ulong)stencilHandles[stencilImageIndex].AddrOfPinnedObject();
                        }
                        results.Add(stencilRes);
                        stencilImageIndex++;
                        if (stencilImageIndex >= stencilImages.Length)
                        {
                            stencilImageIndex = 0;
                        }
                        cpuImage.Dispose();
                    }
                    else
                    {
                        TofArManager.Logger.WriteLog(LogLevel.Debug, $"human Stencil image format was {cpuImage.format}");
                    }
                }
            }
            TofArSegmentationManager.Instance.SetEstimatedResults(new SegmentationResults { results = results.ToArray()}) ;
        }
    }
}

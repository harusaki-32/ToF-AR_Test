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
using System.Runtime.InteropServices;
using UnityEngine;

namespace TofAr.V0.Segmentation.Sky
{
    /// <summary>
    /// 「空」のSegmentation認識を行うクラス
    /// </summary>
    public class SkySegmentationDetector : MonoBehaviour, ISegmentationDetector
    {
        /// <summary>
        /// Segmentation結果名
        /// </summary>
        public static string ResultName { get; } = "Sky";

        /// <summary>
        /// Segmentation結果名
        /// </summary>
        public static string ResultNameMask { get; } = "SkyMask";

        [SerializeField]
        private bool isActive = true;

        /// <summary>
        /// Swaps it to output a fixed pattern to enable easier debugging
        /// </summary>
        private bool isDebug = false;

        /// <summary>
        /// アクティブ状態
        /// <para>true: アクティブである</para>
        /// <para>false: アクティブではない</para>
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
                if (SkyDetector.IsInitialized)
                {
                    SkyDetector.SetupSkyDetector(this.IsActive);
                }
            }
        }

        [SerializeField]
        private bool enableMaskBufferOutput = false;

        /// <summary>
        /// <para>true: Mask結果をBufferで出力する</para>
        /// <para>false: Mask結果をTextureへのPointerで出力する</para>
        /// </summary>
        public bool EnableMaskBufferOutput
        {
            get => this.enableMaskBufferOutput;
            set => this.enableMaskBufferOutput = value;
        }

        /// <summary>
        /// Mask用画像
        /// </summary>
        public Texture2D MaskTexture { get => texMask; }

        private Texture2D texMask = null;
        private byte[] BMask { get; } = new byte[SkyDetector.Width * SkyDetector.Height];
        private GCHandle bMaskPtr;
        private ulong resultTimestamp = 0;
        private bool skyUpdated = false;

        void Awake()
        {
            this.texMask = new Texture2D(SkyDetector.Height, SkyDetector.Width, TextureFormat.R8, false);
            this.texMask.wrapMode = TextureWrapMode.Repeat;
            //make sure our array doesn't move in memory
            bMaskPtr = GCHandle.Alloc(BMask, GCHandleType.Pinned);
        }
        void OnDestroy()
        {
            if (bMaskPtr.IsAllocated)
            {
                bMaskPtr.Free();
            }
        }

        private void OnEnable()
        {
            SkyDetector.SetupSkyDetector(this.IsActive, isDebug);
            if (!SkyDetector.IsInitialized)
            {
                SkyDetector.Init();
            }
            TofArSegmentationManager.OnFrameArrived += this.FrameArrived;
        }

        private void OnDisable()
        {
            TofArSegmentationManager.OnFrameArrived -= this.FrameArrived;
            SkyDetector.Dispose();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (SkyDetector.IsInitialized)
                {
                    SkyDetector.Dispose();
                }
            }
            else
            {
                if (this.enabled)
                {
                    SkyDetector.SetupSkyDetector(this.IsActive, isDebug);
                    if (!SkyDetector.IsInitialized)
                    {
                        SkyDetector.Init();
                    }
                }
            }
        }

        private void FrameArrived(object sender)
        {
            if (!this.IsActive)
            {
                return;
            }
            var data = TofArSegmentationManager.Instance?.SegmentationData?.Data;
            if (data != null)
            {
                foreach (var result in data.results)
                {
                    if (result.name == ResultNameMask)
                    {
                        lock (this.BMask)
                        {
                            switch (result.dataStructureType)
                            {
                                case DataStructureType.MaskBufferByte:
                                    Buffer.BlockCopy(result.maskBufferByte, 0, BMask, 0, BMask.Length);
                                    this.resultTimestamp = result.timestamp;
                                    this.skyUpdated = true;
                                    break;
                                case DataStructureType.RawPointer:
                                    if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
                                    {
                                        Marshal.Copy(new IntPtr((long)result.rawPointer), this.BMask, 0, this.BMask.Length);
                                        this.resultTimestamp = result.timestamp;
                                        this.skyUpdated = true;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (this.skyUpdated)
            {
                var result = new SegmentationResult();
                lock (this.BMask)
                {
                    this.texMask.LoadRawTextureData(this.BMask);
                    this.texMask.Apply();

                    //アプリケーションが複数のDetectorを使用する場合、nameでデータソースを識別可能とする
                    result.name = ResultName;
                    result.timestamp = this.resultTimestamp;

                    result.dataStructureType = DataStructureType.RawPointer;
                    result.rawPointer = (UInt64)bMaskPtr.AddrOfPinnedObject().ToInt64();
                    result.maskBufferSize = this.texMask.width * this.texMask.height;
                    result.maskBufferHeight = this.texMask.height;
                    result.maskBufferWidth = this.texMask.width;

                    if (this.enableMaskBufferOutput)
                    {
                        result.dataStructureType = DataStructureType.MaskBufferByte;
                        result.maskBufferWidth = SkyDetector.Width;
                        result.maskBufferHeight = SkyDetector.Height;
                        result.maskBufferByte = this.BMask;
                        result.maskBufferSize = result.maskBufferByte.Length;
                    }
                }
                this.skyUpdated = false;
                TofArSegmentationManager.Instance.SetEstimatedResult(result);
            }
        }
    }
}

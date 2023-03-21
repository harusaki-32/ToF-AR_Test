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
using System.Runtime.InteropServices;
using AOT;
using TensorFlowLite.Runtime;
using UnityEngine;
using System.Threading;

namespace TofAr.V0.Segmentation.Human
{
    /// <summary>
    /// 人物検出
    /// </summary>
    public unsafe static class HumanDetector
    {
        private const string networkName = "humanseg.tflite";

        // Human Detecter (DNN)
        private static TFLiteRuntime tflRuntime;

        // DNN input/output size
        /// <summary>
        /// DNNインプットの横解像度
        /// </summary>
        public const int Width = 144;

        /// <summary>
        /// DNNインプットの縦解像度
        /// </summary>
        public const int Height = 256;

        // DNN input buffer (allocated in Runtime side)
        private static float[] input;

        private unsafe delegate bool CopyToInputDelegate(byte* rgb, int rgbSize, int sourceWidth, int sourceHeight);
        private unsafe delegate bool ExecuteDelegate(byte* output, int outputSize);

        internal static bool IsInitialized { get; private set; } = false;
        private static bool threadRunning = false;

        private static string tfliteFilePath = null;

        // for debug
        internal static bool OutputInputedColorImage { get; set; } = false; //default must set false;

        /// <summary>
        /// 入力Color画像データ
        /// </summary>
        public static byte[] inputedColorImage = null;

        private static SynchronizationContext tfliteContext;


        private static Thread tfliteThread;


        private static void InitTFLite()
        {
            try
            {
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                tflRuntime = new TFLiteRuntime(tfliteFilePath, TFLiteRuntime.ExecMode.EXEC_MODE_CPU, 2);
#else
                tflRuntime = new TFLiteRuntime(tfliteFilePath, TFLiteRuntime.ExecMode.EXEC_MODE_GPU);
#endif
            }
            catch (Exception e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"[HumanDetector Init] Failed to initialize TFLite: {e.Message}");

            }
            finally
            {
                if (tfliteFilePath != null && System.IO.File.Exists(tfliteFilePath))
                {
                    System.IO.File.Delete(tfliteFilePath);
                }
            }

            if (tflRuntime == null)
            {
                return;
            }

            input = tflRuntime.getInputBuffer()[0];
            IsInitialized = true;
            threadRunning = true;
            TofArManager.Logger.WriteLog(LogLevel.Debug, "[HumanDetector Init] TFLite initialized");
        }

        private static byte* outputArray;
        private static int outputSize;

        private static bool isExecuteTFLite, tfliteExecuteResult;
        private static bool isInitCalled = false;

        private static object executionLock = new object();
        private static object stopLock = new object();

        private static void ExecuteTFlite()
        {
            //TofArManager.Logger.WriteLog(LogLevel.Debug, "[TofArSegmentation HumanDetector] Execute");
            lock (executionLock)
            {
                try
                {
                    float[][] tflOutput = tflRuntime.forward();
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            float backgroundLikelihood = tflOutput[0][y * Width + x];
                            //for some reason the outputs are appended to each other rather than separate arrays
                            float humanLikelihood = tflOutput[0][(Width * Height) + y * Width + x];
                            //steep slope, but not totally erasing the nuance - this way Modelling can use a harsher threshold to avoid putting points on people
                            float likelihoodDifference = Mathf.Clamp((humanLikelihood - backgroundLikelihood) * 128 + 128, 0, 255);
                            int i = (x) * Height + y;
                            if (i < outputSize)
                            {
                                outputArray[i] = (byte)likelihoodDifference;
                            }
                        }
                    }
                    tfliteExecuteResult = true;
                }
                catch (Exception exception)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(exception));
                    tfliteExecuteResult = false;
                }
                isExecuteTFLite = false;
            }
        }



        private static void TFLiteThreadFunction()
        {
            while (threadRunning)
            {
                lock (stopLock)
                {
                    if (isExecuteTFLite)
                    {
                        ExecuteTFlite();
                    }
                }
                Thread.Sleep(1);
            }
        }


        // Start is called before the first frame update
        internal static void Init()
        {
            if (!isInitCalled)
            {
                try
                {
                    isInitCalled = true;
                    if (tfliteContext == null)
                    {
                        tfliteContext = SynchronizationContext.Current;
                    }
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "[HumanDetector Init] Starting TFLite initialisation");

                    if (tfliteThread != null && tfliteThread.IsAlive)
                    {
                        threadRunning = false;
                        tfliteThread.Join();
                    }

                    InitTFLite();

                    tfliteThread = new Thread(TFLiteThreadFunction);

                    tfliteThread.IsBackground = true;
                    tfliteThread.Start();

                    isInitCalled = true;
                }
                catch (Exception e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "[HumanDetector Init]" + Utils.FormatException(e));
                }
            }
        }

        internal unsafe static void SetupHumanDetector(bool enable, bool isDebug = false)
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArSegmentation HumanDetector SetupHumanDetector] enable:{enable}");

            if (tfliteFilePath == null)
            {
                tfliteFilePath = LoadFileFromResources(networkName);
            }
            var property = TofArSegmentationManager.Instance.GetProperty<HumanDetectorSettingsProperty>();

            property.enabled = enable;

            if (TofArManager.Instance.RuntimeSettings.runMode == RunMode.Default)
            {
                var functionPointer = Marshal.GetFunctionPointerForDelegate(new CopyToInputDelegate(CopyToInput));
                property.copyToInputHandler = (UInt64)functionPointer.ToInt64();

                if (isDebug)
                {
                    functionPointer = Marshal.GetFunctionPointerForDelegate(new ExecuteDelegate(DebugExecute));
                    property.executeHandler = (UInt64)functionPointer.ToInt64();
                }
                else
                {
                    functionPointer = Marshal.GetFunctionPointerForDelegate(new ExecuteDelegate(Execute));
                    property.executeHandler = (UInt64)functionPointer.ToInt64();
                }
            }

            TofArSegmentationManager.Instance.SetProperty(property);
        }

        [MonoPInvokeCallback(typeof(CopyToInputDelegate))]
        private unsafe static bool CopyToInput(byte* rgb, int rgbSize, int sourceWidth, int sourceHeight)
        {
            if (!IsInitialized)
            {
                return false;
            }
            try
            {
                int sw = sourceWidth;
                int sh = sourceHeight;

                double aspectSrc = (double)sh / sw;
                int clipX = 0, clipY = 0, clipW = sw, clipH = sh;

                double aspect = (double)9 / 16;
                if (aspectSrc > aspect)
                {
                    // src が縦長い = 上下をカットする
                    clipH = (int)(clipW * aspect);
                    clipY = (sh - clipH) / 2;
                }
                else
                {
                    // src が横長い = 左右をカットする
                    clipW = (int)(clipH / aspect);
                    clipX = (sw - clipW) / 2;
                }

                if (OutputInputedColorImage)
                {
                    if ((inputedColorImage == null) || (inputedColorImage.Length != rgbSize))
                    {
                        inputedColorImage = new byte[rgbSize];
                    }
                    Marshal.Copy((IntPtr)rgb, inputedColorImage, 0, rgbSize);
                }

                for (int c = 0; c < 3; c++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        int sy = clipX + clipW * y / Height;
                        for (int x = 0; x < Width; x++)
                        {
                            int sx = clipY + clipH * x / Width;
                            int i = y * Width + x;
                            var rgbIndex = (sx * sw + sy) * 3 + c;
                            if (rgbIndex < rgbSize)
                            {
                                input[Width * Height * c + i] = rgb[rgbIndex];
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(exception));
                return false;
            }
        }

        [MonoPInvokeCallback(typeof(ExecuteDelegate))]
        private unsafe static bool Execute(byte* output, int outSize)
        {
            if (!IsInitialized)
            {
                return false;
            }
            lock (executionLock)
            {
                outputArray = output;
                outputSize = outSize;
            }
            isExecuteTFLite = true;

            while (isExecuteTFLite)
            {
                Thread.Sleep(1);
            }

            return tfliteExecuteResult;
        }


        [MonoPInvokeCallback(typeof(ExecuteDelegate))]
        private unsafe static bool DebugExecute(byte* output, int outputSize)
        {
            if (!IsInitialized)
            {
                return false;
            }
            //TofArManager.Logger.WriteLog(LogLevel.Debug, $"[TofArSegmentation HumanDetector Execute] outputSize:{outputSize}");

            try
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        int i = (x) * Height + y;
                        if (i < outputSize)
                        {
                            output[i] = y > Height / 2 ? (byte)0 : x > Width / 2 ? (byte)255 : (byte)x;
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, Utils.FormatException(exception));
                return false;
            }
        }


        private static string LoadFileFromResources(string filePath)
        {
            var asset_path = string.Empty;
            var local_path = string.Empty;

            try
            {
                asset_path = filePath;
                local_path = Application.persistentDataPath + Path.DirectorySeparatorChar + asset_path;

                var file_info = new FileInfo(local_path);
                if (!File.Exists(local_path))
                {
                    file_info.Directory.Create();
                    var asset = Resources.Load(asset_path) as TextAsset;
                    if (asset != null)
                    {
                        var s = new MemoryStream(asset.bytes);
                        var br = new BinaryReader(s);
                        File.WriteAllBytes(local_path, br.ReadBytes(asset.bytes.Length));
                    }
                }

            }
            catch (IOException)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to load: asset_path = {asset_path} local_path = {local_path}");
                throw;
            }
            catch (ArgumentException)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to load: asset_path = {asset_path} local_path = {local_path}");
                throw;
            }
            return local_path;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Dispose()
        {
            var property = TofArSegmentationManager.Instance.GetProperty<HumanDetectorSettingsProperty>();
            if (property.enabled)
            {
                property.enabled = false;
                TofArSegmentationManager.Instance.SetProperty(property);
            }

            lock (stopLock)
            {

                isInitCalled = false;
                isExecuteTFLite = false;
                IsInitialized = false;
                tfliteFilePath = null;

                var pathToModelFile = Application.persistentDataPath + Path.DirectorySeparatorChar + networkName;
                if (System.IO.File.Exists(pathToModelFile))
                {
                    System.IO.File.Delete(pathToModelFile);
                }

                if (tflRuntime != null)
                {
                    tflRuntime.Dispose();
                    tflRuntime = null;
                }

                TofArManager.Logger.WriteLog(LogLevel.Debug, "HumanDetector disposed");
            }
        }
    }
}

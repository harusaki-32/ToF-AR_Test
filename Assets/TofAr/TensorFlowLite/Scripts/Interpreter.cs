/* Copyright 2018 The TensorFlow Authors. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
==============================================================================*/

#define TFLITE_V250

#if TFLITE_V250 && !TFLITE_V230
#define TFLITE_V230
#endif // TFLITE_V250

using System;
using System.IO;
using System.Runtime.InteropServices;

using TfLiteDelegate = System.IntPtr;
using TfLiteInterpreter = System.IntPtr;
using TfLiteInterpreterOptions = System.IntPtr;
using TfLiteModel = System.IntPtr;
using TfLiteTensor = System.IntPtr;
using TfLiteRegistration = System.IntPtr;

using UnityEngine;

namespace TensorFlowLite
{
    public class Interpreter : IDisposable
    {
#if UNITY_IOS && !UNITY_EDITOR
        private const string TensorFlowLibrary = "__Internal";
#elif UNITY_ANDROID && !UNITY_EDITOR
        private const string TensorFlowLibrary = "tensorflowlite_jni";
#else
        private const string TensorFlowLibrary = "tensorflowlite_c";
#endif

#if !UNITY_IOS && !UNITY_STANDALONE_OSX && !UNITY_EDITOR_OSX
        private const string CustomOpLibrary = "customopregister";
#endif

        private GCHandle modelDataHandle;
        private TfLiteModel tfliteModel = IntPtr.Zero;
        private TfLiteInterpreter interpreter = IntPtr.Zero;
        private TfLiteInterpreterOptions interpreterOptions = IntPtr.Zero;

        public enum DataType
        {
            NoType = 0,
            Float32 = 1,
            Int32 = 2,
            UInt8 = 3,
            Int64 = 4,
            String = 5,
            Bool = 6,
            Int16 = 7,
            Complex64 = 8,
            Int8 = 9,
            Float16 = 10,
#if TFLITE_V250
            Float64 = 11,
            Complex128 = 12,
            UInt64 = 13,
            Resource = 14,
            Variant = 15,
            UInt32 = 16,
#endif // TFLITE_V250
        }

        public struct QuantizationParams
        {
            public float scale;
            public int zeroPoint;
        }

        public struct Options
        {
            public int threads { get; private set; }
            public IGpuDelegate gpuDelegate { get; private set; }
            public IXNNPackDelegate xnnpackDelegate { get; private set; }
            public INnApiDelegate nnapiDelegate { get; private set; }
            public ICoreMlDelegate coremlDelegate { get; private set; }

            public void SetNumThreads(int threads)
            {
                this.threads = threads;
            }

            public void AddDelegate(GpuDelegate gpuDelegate)
            {
                this.gpuDelegate = gpuDelegate;
            }

            public void AddDelegate(XNNPackDelegate xnnpackDelegate)
            {
                this.xnnpackDelegate = xnnpackDelegate;
            }

            public void AddDelegate(NnApiDelegate nnapiDelegate)
            {
                this.nnapiDelegate = nnapiDelegate;
            }

            public void AddDelegate(CoreMlDelegate coremlDelegate)
            {
                this.coremlDelegate = coremlDelegate;
            }
        }

        public struct TensorInfo
        {
            public string name { get; internal set; }
            public DataType type { get; internal set; }
            public uint bytes { get; internal set; }
            public int elements { get; internal set; }
            public int dims { get; internal set; }
            public int[] shape { get; internal set; }
            public QuantizationParams quantizationParams { get; internal set; }
        }

        public Interpreter(string modelPath, Options options) : this(File.ReadAllBytes(modelPath), options) { }

        public Interpreter(byte[] modelData, Options options)
        {
            if (modelDataHandle.IsAllocated)
            {
                modelDataHandle.Free();
            }
            modelDataHandle = GCHandle.Alloc(modelData, GCHandleType.Pinned);
            IntPtr modelDataPtr = modelDataHandle.AddrOfPinnedObject();

            tfliteModel = TfLiteModelCreate(modelDataPtr, (uint)modelData.Length);
            if (tfliteModel == IntPtr.Zero)
            {
                throw new Exception("Failed to create TensorFlowLite Model");
            }

            interpreterOptions = TfLiteInterpreterOptionsCreate();
            if (options.threads > 1)
            {
                TfLiteInterpreterOptionsSetNumThreads(interpreterOptions, options.threads);
            }
            if (options.gpuDelegate != null)
            {
                TfLiteInterpreterOptionsAddDelegate(interpreterOptions, options.gpuDelegate.Delegate);
            }
            if (options.xnnpackDelegate != null)
            {
                TfLiteInterpreterOptionsAddDelegate(interpreterOptions, options.xnnpackDelegate.Delegate);
            }
            if (options.nnapiDelegate != null)
            {
#if TFLITE_V230 && UNITY_ANDROID && !UNITY_EDITOR
                TfLiteInterpreterOptionsSetUseNNAPI(interpreterOptions, true);
#else
                TfLiteInterpreterOptionsAddDelegate(interpreterOptions, options.nnapiDelegate.Delegate);
#endif // TFLITE_V230
            }
            if (options.coremlDelegate != null)
            {
                TfLiteInterpreterOptionsAddDelegate(interpreterOptions, options.coremlDelegate.Delegate);
            }

            SetTfLiteCustomOp(interpreterOptions);

            interpreter = TfLiteInterpreterCreate(tfliteModel, interpreterOptions);
            if (interpreter == IntPtr.Zero)
            {
                throw new Exception("Failed to create TensorFlowLite Interpreter");
            }
        }

        public void Dispose()
        {
            if (interpreter != IntPtr.Zero)
            {
                TfLiteInterpreterDelete(interpreter);
            }
            interpreter = IntPtr.Zero;

            if (tfliteModel != IntPtr.Zero)
            {
                TfLiteModelDelete(tfliteModel);
            }
            tfliteModel = IntPtr.Zero;

            if (modelDataHandle.IsAllocated)
            {
                modelDataHandle.Free();
            }

            if (interpreterOptions != IntPtr.Zero)
            {
                TfLiteInterpreterOptionsDelete(interpreterOptions);
            }
            interpreterOptions = IntPtr.Zero;
        }

        public void Invoke()
        {
            ThrowIfError(TfLiteInterpreterInvoke(interpreter));
        }

        public int GetInputTensorCount()
        {
            return TfLiteInterpreterGetInputTensorCount(interpreter);
        }

        public void SetInputTensorData(int inputTensorIndex, Array inputTensorData)
        {
            GCHandle tensorDataHandle = GCHandle.Alloc(inputTensorData, GCHandleType.Pinned);
            IntPtr tensorDataPtr = tensorDataHandle.AddrOfPinnedObject();
            TfLiteTensor tensor = TfLiteInterpreterGetInputTensor(interpreter, inputTensorIndex);
            int result = TfLiteTensorCopyFromBuffer(tensor, tensorDataPtr, (uint)Buffer.ByteLength(inputTensorData));
            tensorDataHandle.Free();
            ThrowIfError(result);
        }

        public void ResizeInputTensor(int inputTensorIndex, int[] inputTensorShape)
        {
            ThrowIfError(TfLiteInterpreterResizeInputTensor(
                interpreter, inputTensorIndex, inputTensorShape, inputTensorShape.Length));
        }

        public void AllocateTensors()
        {
            ThrowIfError(TfLiteInterpreterAllocateTensors(interpreter));
        }

        public int GetOutputTensorCount()
        {
            return TfLiteInterpreterGetOutputTensorCount(interpreter);
        }

        public void GetOutputTensorData(int outputTensorIndex, Array outputTensorData)
        {
            GCHandle tensorDataHandle = GCHandle.Alloc(outputTensorData, GCHandleType.Pinned);
            IntPtr tensorDataPtr = tensorDataHandle.AddrOfPinnedObject();
            TfLiteTensor tensor = TfLiteInterpreterGetOutputTensor(interpreter, outputTensorIndex);
            int result = TfLiteTensorCopyToBuffer(tensor, tensorDataPtr, (uint)Buffer.ByteLength(outputTensorData));
            tensorDataHandle.Free();
            ThrowIfError(result);
        }

        public TensorInfo GetInputTensorInfo(int index)
        {
            TfLiteTensor tensor = TfLiteInterpreterGetInputTensor(interpreter, index);
            return GetTensorInfo(tensor);
        }

        public TensorInfo GetOutputTensorInfo(int index)
        {
            TfLiteTensor tensor = TfLiteInterpreterGetOutputTensor(interpreter, index);
            return GetTensorInfo(tensor);
        }

        public static string GetVersion()
        {
            return Marshal.PtrToStringAnsi(TfLiteVersion());
        }

        private static string GetTensorName(TfLiteTensor tensor)
        {
            return Marshal.PtrToStringAnsi(TfLiteTensorName(tensor));
        }

        private static int GetTypeSize(TfLiteTensor tensor)
        {
            DataType type = TfLiteTensorType(tensor);
            switch (type)
            {
#if TFLITE_V250
                case DataType.Complex128:
                    return 16;
#endif // TFLITE_V250
                case DataType.Complex64:
                case DataType.Int64:
#if TFLITE_V250
                case DataType.UInt64:
                case DataType.Float64:
#endif // TFLITE_V250
                    return 8;
                case DataType.Float32:
                case DataType.Int32:
#if TFLITE_V250
                case DataType.UInt32:
#endif // TFLITE_V250
                    return 4;
                case DataType.Float16:
                case DataType.Int16:
                    return 2;
                case DataType.Bool:
                case DataType.Int8:
                case DataType.UInt8:
                    return 1;
                default:
                    return 1;
            }
        }

        private static TensorInfo GetTensorInfo(TfLiteTensor tensor)
        {
            int numDims = TfLiteTensorNumDims(tensor);
            int[] dimensions = new int[numDims];
            for (int i = 0; i < numDims; i++)
            {
                dimensions[i] = TfLiteTensorDim(tensor, i);
            }
            return new TensorInfo()
            {
                name = GetTensorName(tensor),
                type = TfLiteTensorType(tensor),
                bytes = TfLiteTensorByteSize(tensor),
                elements = (int)TfLiteTensorByteSize(tensor) / GetTypeSize(tensor),
                dims = numDims,
                shape = dimensions,
                quantizationParams = TfLiteTensorQuantizationParams(tensor),
            };
        }

        private static void ThrowIfError(int resultCode)
        {
            if (resultCode != 0)
            {
                throw new Exception("TensorFlowLite operation failed.");
            }
        }

        private static void SetTfLiteCustomOp(TfLiteInterpreterOptions interpreterOptions)
        {
#if !UNITY_IOS && !UNITY_STANDALONE_OSX && !UNITY_EDITOR_OSX
            Debug.Log("Set TfLite Custom Operation");

            int opNumber = GetCustomOpNumber();
            Debug.Log(opNumber);

            IntPtr opNamesPtr = GetCustomOpNames();
            IntPtr registersPtr = GetCustomOpRegisters();
            IntPtr versionsPtr = GetCustomOpVersions();

            int intPtrSize = Marshal.SizeOf(typeof(IntPtr));
            int intSize = Marshal.SizeOf(typeof(int));

            for (int i = 0; i < opNumber; i++)
            {
                IntPtr name = Marshal.ReadIntPtr(opNamesPtr, i * intPtrSize);
                //Debug.Log(Marshal.PtrToStringAnsi(name));

                int version = Marshal.ReadInt32(versionsPtr, i * intSize);
                //Debug.Log(version);

                TfLiteRegistration registration = Marshal.ReadIntPtr(registersPtr, i * intPtrSize);
                //Debug.Log(registration);

                TfLiteInterpreterOptionsAddCustomOp(interpreterOptions, name, registration, 1, version);
            }
#endif // !UNITY_IOS
        }

        #region Externs

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe IntPtr TfLiteVersion();

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe TfLiteModel TfLiteModelCreate(IntPtr model_data, uint model_size);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe void TfLiteModelDelete(TfLiteModel model);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe TfLiteInterpreterOptions TfLiteInterpreterOptionsCreate();

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe void TfLiteInterpreterOptionsDelete(TfLiteInterpreterOptions options);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe void TfLiteInterpreterOptionsSetNumThreads(TfLiteInterpreterOptions options, int num_threads);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe void TfLiteInterpreterOptionsAddDelegate(TfLiteInterpreterOptions options, TfLiteDelegate _delegate);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe TfLiteInterpreter TfLiteInterpreterCreate(TfLiteModel model, TfLiteInterpreterOptions optional_options);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe void TfLiteInterpreterDelete(TfLiteInterpreter interpreter);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteInterpreterGetInputTensorCount(TfLiteInterpreter interpreter);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe TfLiteTensor TfLiteInterpreterGetInputTensor(TfLiteInterpreter interpreter, int input_index);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteInterpreterResizeInputTensor(TfLiteInterpreter interpreter, int input_index, int[] input_dims, int input_dims_size);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteInterpreterAllocateTensors(TfLiteInterpreter interpreter);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteInterpreterInvoke(TfLiteInterpreter interpreter);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteInterpreterGetOutputTensorCount(TfLiteInterpreter interpreter);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe TfLiteTensor TfLiteInterpreterGetOutputTensor(TfLiteInterpreter interpreter, int output_index);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe DataType TfLiteTensorType(TfLiteTensor tensor);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteTensorNumDims(TfLiteTensor tensor);

        [DllImport(TensorFlowLibrary)]
        private static extern int TfLiteTensorDim(TfLiteTensor tensor, int dim_index);

        [DllImport(TensorFlowLibrary)]
        private static extern uint TfLiteTensorByteSize(TfLiteTensor tensor);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe IntPtr TfLiteTensorName(TfLiteTensor tensor);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe QuantizationParams TfLiteTensorQuantizationParams(TfLiteTensor tensor);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteTensorCopyFromBuffer(TfLiteTensor tensor, IntPtr input_data, uint input_data_size);

        [DllImport(TensorFlowLibrary)]
        private static extern unsafe int TfLiteTensorCopyToBuffer(TfLiteTensor tensor, IntPtr output_data, uint output_data_size);

#if TFLITE_V230 && UNITY_ANDROID && !UNITY_EDITOR
        [DllImport(TensorFlowLibrary)]
        private static extern unsafe void TfLiteInterpreterOptionsSetUseNNAPI(TfLiteInterpreterOptions options, bool enable);
#endif // TFLITE_V230

#if !UNITY_IOS && !UNITY_STANDALONE_OSX && !UNITY_EDITOR_OSX
        [DllImport(TensorFlowLibrary)]
        private static extern unsafe void TfLiteInterpreterOptionsAddCustomOp(TfLiteInterpreterOptions options, IntPtr name, TfLiteRegistration registration, int min_version, int max_version);

        [DllImport(CustomOpLibrary)]
        private static extern unsafe int GetCustomOpNumber();

        [DllImport(CustomOpLibrary)]
        private static extern unsafe IntPtr GetCustomOpNames();

        [DllImport(CustomOpLibrary)]
        private static extern unsafe IntPtr GetCustomOpRegisters();

        [DllImport(CustomOpLibrary)]
        private static extern unsafe IntPtr GetCustomOpVersions();
#endif // !UNITY_IOS

        #endregion
    }
}

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

#define TFLITE_V230

#if TFLITE_V230 && !TFLITE_V200
#define TFLITE_V200
#endif // TFLITE_V230

using System.Runtime.InteropServices;

using TfLiteDelegate = System.IntPtr;

namespace TensorFlowLite
{
    public class GpuDelegate : IGpuDelegate
    {
#if UNITY_IOS && !UNITY_EDITOR
        private const string TensorFlowLibraryGpu = "__Internal";
#elif UNITY_ANDROID && !UNITY_EDITOR
        private const string TensorFlowLibraryGpu = "tensorflowlite_gpu_jni";
#else
#if TFLITE_V200
        private const string TensorFlowLibraryGpu = "tensorflowlite_gpu_delegate";
#else
        private const string TensorFlowLibraryGpu = "tensorflowlite_gpu_gl";
#endif // TFLITE_V200
#endif

#if UNITY_IOS && !UNITY_EDITOR
        [StructLayout(LayoutKind.Sequential)]
        public struct Options
        {
            public bool allow_precision_loss;
            public int wait_type;
            public bool enable_quantization;
        };
#else
        [StructLayout(LayoutKind.Sequential)]
        public struct Options
        {
            public int is_precision_loss_allowed;
            public int inference_preference;
            public int inference_priority1;
            public int inference_priority2;
            public int inference_priority3;
#if TFLITE_V230
            public long experimental_flags;
            public int max_delegated_partitions;
#endif // TFLITE_V230
        };
#endif

        public TfLiteDelegate Delegate { get; private set; }

        public GpuDelegate(Options? options = null)
        {
            Options delegateOptions = options ?? TfLiteGpuDelegateOptionsDefault();
            Delegate = TfLiteGpuDelegateCreate(ref delegateOptions);
        }

        public void Dispose()
        {
            TfLiteGpuDelegateDelete(Delegate);
            Delegate = TfLiteDelegate.Zero;
        }

        #region Externs

#if UNITY_IOS && !UNITY_EDITOR
        private static Options TfLiteGpuDelegateOptionsDefault()
        {
            Options options = new Options()
            {
                allow_precision_loss = false,
                wait_type = 0,
                enable_quantization = false,
            };
            return options;
        }
#else
#if TFLITE_V200
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TfLiteGpuDelegateOptionsV2Default")]
#else
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TfLiteGpuDelegateOptionsDefault")]
#endif // TFLITE_V200
        private static extern unsafe Options TfLiteGpuDelegateOptionsDefault();
#endif

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TFLGpuDelegateCreate")]
#else
#if TFLITE_V200
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TfLiteGpuDelegateV2Create")]
#else
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TfLiteGpuDelegateCreate")]
#endif // TFLITE_V200
#endif
        private static extern unsafe TfLiteDelegate TfLiteGpuDelegateCreate(ref Options options);

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TFLGpuDelegateDelete")]
#else
#if TFLITE_V200
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TfLiteGpuDelegateV2Delete")]
#else
        [DllImport(TensorFlowLibraryGpu, EntryPoint = "TfLiteGpuDelegateDelete")]
#endif // TFLITE_V200
#endif
        private static extern unsafe void TfLiteGpuDelegateDelete(TfLiteDelegate _delegate);

        #endregion
    }
}

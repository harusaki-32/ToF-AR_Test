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

// #define TFLITE_JAVA_API

using System.Runtime.InteropServices;

using TfLiteDelegate = System.IntPtr;

namespace TensorFlowLite
{
    public class NnApiDelegate : INnApiDelegate
    {
#if TFLITE_JAVA_API
        private const string TensorFlowLibraryNnApi = "tensorflowlite_jni";
        private const string TensorFlowLibraryNnApiJava = "org.tensorflow.lite.nnapi.NnApiDelegate";

        // This is needed to load the native library before calling the java class.
        private static string TensorFlowLiteVersion = Marshal.PtrToStringAnsi(TfLiteVersion());

#if TFLITE_V230
        private const int ANEURALNETWORKS_PRIORITY_DEFAULT = 100;
#endif // TFLITE_V230
#endif // TFLITE_JAVA_API

        [StructLayout(LayoutKind.Sequential)]
        public struct Options
        {
            public int execution_preference;
            public string accelerator_name;
            public string cache_dir;
            public string model_token;
            public bool disallow_nnapi_cpu;
            public int max_number_delegated_partitions;
#if TFLITE_V230
            public bool allow_fp16;
            public int execution_priority;
            public ulong max_compilation_timeout_duration_ns;
            public ulong max_execution_timeout_duration_ns;
            public ulong max_execution_loop_timeout_duration_ns;
#endif // TFLITE_V230
#if TFLITE_V250
            public bool allow_dynamic_dimensions;
            public bool use_burst_computation;
#endif // TFLITE_V250
        }

        public TfLiteDelegate Delegate { get; private set; }

        public NnApiDelegate(Options? options = null)
        {
#if TFLITE_JAVA_API
            UnityEngine.AndroidJavaClass javaClass = new UnityEngine.AndroidJavaClass(TensorFlowLibraryNnApiJava);
            Options delegateOptions = options ?? TfLiteNnApiDelegateOptionsDefault();
            Delegate = (TfLiteDelegate)javaClass.CallStatic<long>("createDelegate",
                delegateOptions.execution_preference, delegateOptions.accelerator_name, delegateOptions.cache_dir,
                delegateOptions.model_token, delegateOptions.max_number_delegated_partitions);
#endif // TFLITE_JAVA_API
        }

        public void Dispose()
        {
#if TFLITE_JAVA_API
            UnityEngine.AndroidJavaClass javaClass = new UnityEngine.AndroidJavaClass(TensorFlowLibraryNnApiJava);
            javaClass.CallStatic("deleteDelegate", (long)Delegate);
            Delegate = TfLiteDelegate.Zero;
#endif // TFLITE_JAVA_API
        }

        #region Externs

#if TFLITE_JAVA_API
        private static Options TfLiteNnApiDelegateOptionsDefault()
        {
            Options options = new Options()
            {
                execution_preference = -1,
                accelerator_name = null,
                cache_dir = null,
                model_token = null,
#if TFLITE_V250
                disallow_nnapi_cpu = true,
#else
                disallow_nnapi_cpu = false,
#endif // TFLITE_V250
                max_number_delegated_partitions = -1,
#if TFLITE_V230
                allow_fp16 = false,
                execution_priority = ANEURALNETWORKS_PRIORITY_DEFAULT,
                max_compilation_timeout_duration_ns = 0,
                max_execution_timeout_duration_ns = 0,
                max_execution_loop_timeout_duration_ns = 0,
#endif // TFLITE_V230
#if TFLITE_V250
                allow_dynamic_dimensions = false,
                use_burst_computation = false,
#endif // TFLITE_V250
            };
            return options;
        }

        [DllImport(TensorFlowLibraryNnApi)]
        private static extern unsafe System.IntPtr TfLiteVersion();
#endif // TFLITE_JAVA_API

        #endregion
    }
}

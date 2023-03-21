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

// #define TFLITE_V260

using System.Runtime.InteropServices;

using TfLiteDelegate = System.IntPtr;

namespace TensorFlowLite
{
    public class XNNPackDelegate : IXNNPackDelegate
    {
#if UNITY_IOS && !UNITY_EDITOR
        private const string TensorFlowLibraryXNNPack = "__Internal";
#elif UNITY_ANDROID && !UNITY_EDITOR
        private const string TensorFlowLibraryXNNPack = "tensorflowlite_jni";
#else
        private const string TensorFlowLibraryXNNPack = "tensorflowlite_c";
#endif

        [StructLayout(LayoutKind.Sequential)]
        public struct Options
        {
            public int num_threads;
#if TFLITE_V260
            public bool enable_int8_weights_unpacking;
#endif // TFLITE_V260
        };

        public TfLiteDelegate Delegate { get; private set; }

        public XNNPackDelegate(Options? options = null)
        {
            Options delegateOptions = options ?? TfLiteXNNPackDelegateOptionsDefault();
            Delegate = TfLiteXNNPackDelegateCreate(ref delegateOptions);
        }

        public void Dispose()
        {
            TfLiteXNNPackDelegateDelete(Delegate);
            Delegate = TfLiteDelegate.Zero;
        }

        #region Externs

        [DllImport(TensorFlowLibraryXNNPack, EntryPoint = "TfLiteXNNPackDelegateOptionsDefault")]
        private static extern unsafe Options TfLiteXNNPackDelegateOptionsDefault();

        [DllImport(TensorFlowLibraryXNNPack, EntryPoint = "TfLiteXNNPackDelegateCreate")]
        private static extern unsafe TfLiteDelegate TfLiteXNNPackDelegateCreate(ref Options options);

        [DllImport(TensorFlowLibraryXNNPack, EntryPoint = "TfLiteXNNPackDelegateDelete")]
        private static extern unsafe void TfLiteXNNPackDelegateDelete(TfLiteDelegate _delegate);

        #endregion
    }
}

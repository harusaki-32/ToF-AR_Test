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


using System.Runtime.InteropServices;

using TfLiteDelegate = System.IntPtr;

namespace TensorFlowLite
{
    public class CoreMlDelegate : ICoreMlDelegate
    {
#if UNITY_IOS && !UNITY_EDITOR
        private const string TensorFlowLibraryCoreMl = "__Internal";
#else
        private const string TensorFlowLibraryCoreMl = "tensorflowlite_coreml_delegate";
#endif

        [StructLayout(LayoutKind.Sequential)]
        public struct Options
        {
            public int enabled_devices;
            public int coreml_version;
            public int max_delegated_partitions;
            public int min_nodes_per_partition;
        };

        public TfLiteDelegate Delegate { get; private set; }

        public CoreMlDelegate(Options? options = null)
        {
            Options delegateOptions = options ?? TfLiteCoreMlDelegateOptionsDefault();
            Delegate = TfLiteCoreMlDelegateCreate(ref delegateOptions);
        }

        public void Dispose()
        {
            TfLiteCoreMlDelegateDelete(Delegate);
            Delegate = TfLiteDelegate.Zero;
        }

        #region Externs

        private static Options TfLiteCoreMlDelegateOptionsDefault()
        {
            Options options = new Options()
            {
                enabled_devices = 0,
                coreml_version = 0,
                max_delegated_partitions = 0,
                min_nodes_per_partition = 0,
            };
            return options;
        }

        [DllImport(TensorFlowLibraryCoreMl, EntryPoint = "TfLiteCoreMlDelegateCreate")]
        private static extern unsafe TfLiteDelegate TfLiteCoreMlDelegateCreate(ref Options options);

        [DllImport(TensorFlowLibraryCoreMl, EntryPoint = "TfLiteCoreMlDelegateDelete")]
        private static extern unsafe void TfLiteCoreMlDelegateDelete(TfLiteDelegate _delegate);

        #endregion
    }
}

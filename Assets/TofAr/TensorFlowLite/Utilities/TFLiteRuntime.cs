/*
 * Copyright 2020,2021 Sony Semiconductor Solutions Corporation.
 *
 * This is UNPUBLISHED PROPRIETARY SOURCE CODE of Sony Semiconductor
 * Solutions Corporation.
 * No part of this file may be copied, modified, sold, and distributed in any
 * form or by any means without prior explicit permission in writing from
 * Sony Semiconductor Solutions Corporation.
 *
 */

using System;
using System.Diagnostics;
using System.Threading;

namespace TensorFlowLite.Runtime
{
    public class TFLiteRuntime : IDisposable
    {
        public enum ExecMode
        {
            EXEC_MODE_CPU,
            EXEC_MODE_GPU,
            EXEC_MODE_XNNPACK,
            EXEC_MODE_NNAPI,
            EXEC_MODE_COREML,
        }

        private enum Direction
        {
            NCHW_TO_NHWC,
            NHWC_TO_NCHW,
        }

        public struct Config
        {
            public bool isTranspose;
            public int performanceCount;
            public int runFps;
        }

        public struct Performance
        {
            public double average;
            public double minimum;
            public double maximum;
            public double deviation;
        }

        // [TODO: delete in the future] These are for compatibility
        private static bool IS_PERFORMANCE_COUNT_CHANGED = false;
        private static int PERFORMANCE_COUNT = 0;
        private static double AVERAGE_TIME = 0.0;

        private Config runtimeConfig = new Config { isTranspose = true, performanceCount = 0, runFps = 0 };
        private Performance performanceResult;

        private Interpreter interpreter = null;
        private GpuDelegate gpuDelegate = null;
        private XNNPackDelegate xnnpackDelegate = null;
        private NnApiDelegate nnapiDelegate = null;
        private CoreMlDelegate coremlDelegate = null;

        private float[][] input;
        private float[][] output;

        private int[][] inputShape;
        private int[][] outputShape;

        public TFLiteRuntime(string modelPath, ExecMode mode = ExecMode.EXEC_MODE_CPU, int threadsNum = 1, int batchSize = 1)
        {
            Interpreter.Options tflite_options = new Interpreter.Options();

            switch (mode)
            {
                case ExecMode.EXEC_MODE_CPU:
                    break;
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
                case ExecMode.EXEC_MODE_GPU:
                    gpuDelegate = new GpuDelegate();
                    tflite_options.AddDelegate(gpuDelegate);
                    break;
                case ExecMode.EXEC_MODE_XNNPACK:
                    XNNPackDelegate.Options xnnpackOptions = new XNNPackDelegate.Options() { num_threads = threadsNum };
                    xnnpackDelegate = new XNNPackDelegate(xnnpackOptions);
                    tflite_options.AddDelegate(xnnpackDelegate);
                    break;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
                case ExecMode.EXEC_MODE_NNAPI:
                    nnapiDelegate = new NnApiDelegate();
                    tflite_options.AddDelegate(nnapiDelegate);
                    break;
#endif
#if UNITY_IOS && !UNITY_EDITOR
                case ExecMode.EXEC_MODE_COREML:
                    coremlDelegate = new CoreMlDelegate();
                    tflite_options.AddDelegate(coremlDelegate);
                    break;
#endif
                default:
                    string err_msg = "Not supported mode: " + mode;
                    throw new Exception(err_msg);
            }
            tflite_options.SetNumThreads(threadsNum);

            interpreter = new Interpreter(modelPath, tflite_options);

            // Resize by batch size
            for (int index = 0; index < interpreter.GetInputTensorCount(); index++)
            {
                Interpreter.TensorInfo input_tensor = interpreter.GetInputTensorInfo(index);
                int[] input_shapes = input_tensor.shape;
                input_shapes[0] = batchSize;
                interpreter.ResizeInputTensor(index, input_shapes);
                interpreter.AllocateTensors();
            }

            input = new float[interpreter.GetInputTensorCount()][];
            for (int i = 0; i < interpreter.GetInputTensorCount(); i++)
            {
                Interpreter.TensorInfo tensor = interpreter.GetInputTensorInfo(i);
                input[i] = new float[tensor.elements];
            }

            output = new float[interpreter.GetOutputTensorCount()][];
            for (int i = 0; i < interpreter.GetOutputTensorCount(); i++)
            {
                Interpreter.TensorInfo tensor = interpreter.GetOutputTensorInfo(i);
                output[i] = new float[tensor.elements];
            }

            inputShape = new int[interpreter.GetInputTensorCount()][];
            for (int i = 0; i < interpreter.GetInputTensorCount(); i++)
            {
                inputShape[i] = interpreter.GetInputTensorInfo(i).shape;
            }

            outputShape = new int[interpreter.GetOutputTensorCount()][];
            for (int i = 0; i < interpreter.GetOutputTensorCount(); i++)
            {
                outputShape[i] = interpreter.GetOutputTensorInfo(i).shape;
            }
        }

        ~TFLiteRuntime()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (interpreter != null)
            {
                interpreter.Dispose();
                interpreter = null;
            }
            if (gpuDelegate != null)
            {
                gpuDelegate.Dispose();
                gpuDelegate = null;
            }
            if (xnnpackDelegate != null)
            {
                xnnpackDelegate.Dispose();
                xnnpackDelegate = null;
            }
            if (nnapiDelegate != null)
            {
                nnapiDelegate.Dispose();
                nnapiDelegate = null;
            }
            if (coremlDelegate != null)
            {
                coremlDelegate.Dispose();
                coremlDelegate = null;
            }
        }

        public float[][] getInputBuffer()
        {
            return input;
        }

        public float[][] getOutputBuffer()
        {
            return output;
        }

        public int[][] getInputShape()
        {
            return inputShape;
        }

        public int[][] getOutputShape()
        {
            return outputShape;
        }

        public float[][] forward()
        {
            // Transpose input from NCHW to NHWC
            if (runtimeConfig.isTranspose)
            {
                for (int i = 0; i < interpreter.GetInputTensorCount(); i++)
                {
                    Interpreter.TensorInfo tensor = interpreter.GetInputTensorInfo(i);
                    TransposeTensorData(input[i], Direction.NCHW_TO_NHWC, ref tensor);
                }
            }

            for (int i = 0; i < interpreter.GetInputTensorCount(); i++)
            {
                interpreter.SetInputTensorData(i, input[i]);
            }

            // Do forward.
            interpreter.Invoke();

            // [TODO: delete in the future] These are for compatibility
            if (IS_PERFORMANCE_COUNT_CHANGED)
            {
                IS_PERFORMANCE_COUNT_CHANGED = false;
                runtimeConfig.performanceCount = PERFORMANCE_COUNT;
            }

            if (0 < runtimeConfig.performanceCount)
            {
                double[] time_list = new double[runtimeConfig.performanceCount];
#if false
                int fps_ms = (int)(1000 / runtimeConfig.runFps);
#endif
                for (int i = 0; i < time_list.Length; i++)
                {
                    Stopwatch local_stop_watch = new Stopwatch();
                    local_stop_watch.Start();
                    // Do forward.
                    interpreter.Invoke();
                    local_stop_watch.Stop();
                    double elapsed_time = local_stop_watch.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
                    if (0 < runtimeConfig.runFps)
                    {
#if true
                        int fps_ms = (int)(1000 / runtimeConfig.runFps);
#endif
                        int wait_ms = fps_ms - (int)elapsed_time;
                        if (wait_ms > 0) Thread.Sleep(wait_ms);
                    }
                    time_list[i] = elapsed_time;
                }
                double time_sum = 0.0;
                double time_square = 0.0;
                foreach (double time in time_list)
                {
                    time_sum += time;
                    time_square += time * time;
                }
                performanceResult.minimum = time_list[0];
                performanceResult.maximum = time_list[0];
                for (int i = 1; i < time_list.Length; i++)
                {
                    performanceResult.minimum = Math.Min(performanceResult.minimum, time_list[i]);
                    performanceResult.maximum = Math.Max(performanceResult.maximum, time_list[i]);
                }
                performanceResult.average = time_sum / time_list.Length;
                performanceResult.deviation = Math.Sqrt((time_square / time_list.Length) - (performanceResult.average * performanceResult.average));

                // [TODO: delete in the future] These are for compatibility
                AVERAGE_TIME = performanceResult.average;
            }

            for (int i = 0; i < interpreter.GetOutputTensorCount(); i++)
            {
                interpreter.GetOutputTensorData(i, output[i]);
            }

            // Transpose output from NHWC to NCHW
            if (runtimeConfig.isTranspose)
            {
                for (int i = 0; i < interpreter.GetOutputTensorCount(); i++)
                {
                    Interpreter.TensorInfo tensor = interpreter.GetOutputTensorInfo(i);
                    TransposeTensorData(output[i], Direction.NHWC_TO_NCHW, ref tensor);
                }
            }

            return output;
        }

        public Config GetRuntimeConfig()
        {
            // [TODO: delete in the future] These are for compatibility
            if (IS_PERFORMANCE_COUNT_CHANGED)
            {
                IS_PERFORMANCE_COUNT_CHANGED = false;
                runtimeConfig.performanceCount = PERFORMANCE_COUNT;
            }

            return runtimeConfig;
        }

        public void SetRuntimeConfig(Config config)
        {
            runtimeConfig = config;

            // [TODO: delete in the future] These are for compatibility
            IS_PERFORMANCE_COUNT_CHANGED = false;
            PERFORMANCE_COUNT = runtimeConfig.performanceCount;
        }

        public Performance GetPerformanceResult()
        {
            return performanceResult;
        }

        // [TODO: delete in the future] These are for compatibility
        public static void SetPerformanceCount(int value)
        {
            IS_PERFORMANCE_COUNT_CHANGED = true;
            PERFORMANCE_COUNT = value;
        }

        // [TODO: delete in the future] These are for compatibility
        public static int GetPerformanceCount()
        {
            return PERFORMANCE_COUNT;
        }

        // [TODO: delete in the future] These are for compatibility
        public static double GetPerformanceAverage()
        {
            return AVERAGE_TIME;
        }

        private void TransposeTensorData(float[] data, Direction direction, ref Interpreter.TensorInfo tensor)
        {
            if (1 > (tensor.dims - 1))
            {
                // Normally do not pass here
                return;
            }

            int channel_num = tensor.shape[tensor.dims - 1];
            int block_size = 1;

            for (int dim = 1; dim < (tensor.dims - 1); dim++)
            {
                block_size *= tensor.shape[dim];
            }

            if ((1 == block_size) || (1 == channel_num))
            {
                // No need transpose
                return;
            }

            float[] transpose_data = new float[block_size * channel_num];

            for (int ch = 0; ch < channel_num; ch++)
            {
                for (int pos = 0; pos < block_size; pos++)
                {
                    switch (direction)
                    {
                        case Direction.NHWC_TO_NCHW:
                            transpose_data[block_size * ch + pos] = data[pos * channel_num + ch];
                            break;
                        case Direction.NCHW_TO_NHWC:
                            transpose_data[pos * channel_num + ch] = data[block_size * ch + pos];
                            break;
                    }
                }
            }

            Array.Copy(transpose_data, data, data.Length);
        }
    }
}
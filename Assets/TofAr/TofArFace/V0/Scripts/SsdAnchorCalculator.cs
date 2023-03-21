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
using System.Collections.Generic;
using UnityEngine;

namespace TofAr.V0.Face
{

    internal class SsdAnchor
    {
        public float x; // center
        public float y; // center
        public float width;
        public float height;
    }

    internal class Options
    {
        public int inputSizeWidth;
        public int inputSizeHeight;

        public float minScale;
        public float maxScale;

        public float anchorOffsetX;
        public float anchorOffsetY;

        public int numLayers;
        public int[] featureMapWidth;
        public int[] featureMapHeight;
        public int[] strides;

        public float[] aspectRatios;

        public bool reduceBoxesInLowestLayer;
        public float interpolatedScaleAspectRatio;

        public bool fixedAnchorSize;
    }

    internal class SsdAnchorCalculator
    {
        private static float CalculateScale(float minScale, float maxScale, int strideIndex, int numStrides)
        {
            return minScale + (maxScale - minScale) * 1.0f * strideIndex / (numStrides - 1.0f);
        }

        public static SsdAnchor[] Generate(Options options)
        {
            List<SsdAnchor> anchors = new List<SsdAnchor>();

            int layer_id = 0;
            while (layer_id < options.strides.Length)
            {
                List<float> anchor_height = new List<float>();
                List<float> anchor_width = new List<float>();
                List<float> aspect_ratios = new List<float>();
                List<float> scales = new List<float>();

                // For same strides, we merge the anchors in the same order.
                int last_same_stride_layer = layer_id;
                while (last_same_stride_layer < (int)options.strides.Length
                       && options.strides[last_same_stride_layer] == options.strides[layer_id])
                {
                    float scale = CalculateScale(options.minScale,
                                                 options.maxScale,
                                                 last_same_stride_layer,
                                                 options.strides.Length);
                    if (last_same_stride_layer == 0 && options.reduceBoxesInLowestLayer)
                    {
                        // For first layer, it can be specified to use predefined anchors.
                        aspect_ratios.Add(1.0f);
                        aspect_ratios.Add(2.0f);
                        aspect_ratios.Add(0.5f);
                        scales.Add(0.1f);
                        scales.Add(scale);
                        scales.Add(scale);
                    }
                    else
                    {
                        for (int aspect_ratio_id = 0;
                            aspect_ratio_id < (int)options.aspectRatios.Length;
                             ++aspect_ratio_id)
                        {
                            aspect_ratios.Add(options.aspectRatios[aspect_ratio_id]);
                            scales.Add(scale);
                        }
                        if (options.interpolatedScaleAspectRatio > 0.0)
                        {
                            float scale_next = last_same_stride_layer == (int)options.strides.Length - 1
                                    ? 1.0f
                                    : CalculateScale(options.minScale, options.maxScale,
                                                     last_same_stride_layer + 1,
                                                     options.strides.Length);
                            scales.Add((float)Mathf.Sqrt(scale * scale_next));
                            aspect_ratios.Add(options.interpolatedScaleAspectRatio);
                        }
                    }
                    last_same_stride_layer++;
                }

                for (int i = 0; i < (int)aspect_ratios.Count; ++i)
                {
                    float ratio_sqrts = (float)Mathf.Sqrt(aspect_ratios[i]);
                    anchor_height.Add(scales[i] / ratio_sqrts);
                    anchor_width.Add(scales[i] * ratio_sqrts);
                }

                int feature_map_height = 0;
                int feature_map_width = 0;
                if (options.featureMapHeight.Length > 0)
                {
                    feature_map_height = options.featureMapHeight[layer_id];
                    feature_map_width = options.featureMapWidth[layer_id];
                }
                else
                {
                    int stride = options.strides[layer_id];
                    feature_map_height = (int)Mathf.Ceil(1.0f * options.inputSizeHeight / stride);
                    feature_map_width = (int)Mathf.Ceil(1.0f * options.inputSizeWidth / stride);
                }

                for (int y = 0; y < feature_map_height; ++y)
                {
                    for (int x = 0; x < feature_map_width; ++x)
                    {
                        for (int anchor_id = 0; anchor_id < (int)anchor_height.Count; ++anchor_id)
                        {
                            // TODO: Support specifying anchor_offset_x, anchor_offset_y.
                            float x_center = (x + options.anchorOffsetX) * 1.0f / feature_map_width;
                            float y_center = (y + options.anchorOffsetY) * 1.0f / feature_map_height;

                            SsdAnchor new_anchor = new SsdAnchor();
                            new_anchor.x = x_center;
                            new_anchor.y = y_center;

                            if (options.fixedAnchorSize)
                            {
                                new_anchor.width = 1.0f;
                                new_anchor.height = 1.0f;
                            }
                            else
                            {
                                new_anchor.width = anchor_width[anchor_id];
                                new_anchor.height = anchor_height[anchor_id];
                            }
                            anchors.Add(new_anchor);
                        }
                    }
                }
                layer_id = last_same_stride_layer;
            }
            return anchors.ToArray();
        }
    }
}

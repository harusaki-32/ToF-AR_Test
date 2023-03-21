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
using System.Linq;
using UnityEngine;

namespace TofAr.V0.Face
{
    internal class FaceRect
    {
        public float x, y, width, height;
        public float xMin, yMin, xMax, yMax;

        public FaceRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            xMin = Mathf.Min(x, x + width);
            xMax = Mathf.Max(x, x + width);
            yMin = Mathf.Min(y, y + height);
            yMax = Mathf.Max(y, y + height);
        }

        public float IntersectionOverUnion(FaceRect rect1)
        {
            FaceRect rect0 = this;

            float sx0 = rect0.xMin;
            float sy0 = rect0.yMin;
            float ex0 = rect0.xMax;
            float ey0 = rect0.yMax;
            float sx1 = rect1.xMin;
            float sy1 = rect1.yMin;
            float ex1 = rect1.xMax;
            float ey1 = rect1.yMax;

            float xmin0 = Mathf.Min(sx0, ex0);
            float ymin0 = Mathf.Min(sy0, ey0);
            float xmax0 = Mathf.Max(sx0, ex0);
            float ymax0 = Mathf.Max(sy0, ey0);
            float xmin1 = Mathf.Min(sx1, ex1);
            float ymin1 = Mathf.Min(sy1, ey1);
            float xmax1 = Mathf.Max(sx1, ex1);
            float ymax1 = Mathf.Max(sy1, ey1);

            float area0 = (ymax0 - ymin0) * (xmax0 - xmin0);
            float area1 = (ymax1 - ymin1) * (xmax1 - xmin1);
            if (area0 <= 0 || area1 <= 0)
            {
                return 0.0f;
            }

            float intersect_xmin = Mathf.Max(xmin0, xmin1);
            float intersect_ymin = Mathf.Max(ymin0, ymin1);
            float intersect_xmax = Mathf.Min(xmax0, xmax1);
            float intersect_ymax = Mathf.Min(ymax0, ymax1);

            float intersect_area = Mathf.Max(intersect_ymax - intersect_ymin, 0.0f) *
                                   Mathf.Max(intersect_xmax - intersect_xmin, 0.0f);

            return intersect_area / (area0 + area1 - intersect_area);
        }
    }

    internal class FaceDetectorResult
    {
        public float score;
        public FaceRect rect;
        // 0: left
        // 1: right
        public Vector2[] keypoints;

        public FaceDetectorResult(float score, FaceRect rect, Vector2[] vs)
        {
            this.score = score;
            this.rect = rect;
            this.keypoints = vs;
        }
    }

    internal class FaceDetector
    {
        private const int KEY_POINT_SIZE = 6;

        private const int MAX_FACE_NUM = 100;

        private const int OUTPUT_SIZE = 896;

        protected int width = 256;
        protected int height = 256;

        private SsdAnchor[] anchors;
        private List<FaceDetectorResult> results = new List<FaceDetectorResult>();
        private List<FaceDetectorResult> filterdResults = new List<FaceDetectorResult>();

        public FaceDetector()
        {
            Options options = new Options();
            options.inputSizeWidth = 128;
            options.inputSizeHeight = 128;
            options.minScale = 0.1484375f;
            options.maxScale = 0.75f;
            options.anchorOffsetX = 0.5f;
            options.anchorOffsetY = 0.5f;
            options.numLayers = 4;
            options.featureMapWidth = new int[0];
            options.featureMapHeight = new int[0];
            options.strides = new int[] { 8, 16, 16, 16 };
            options.aspectRatios = new float[] { 1.0f };
            options.reduceBoxesInLowestLayer = false;
            options.interpolatedScaleAspectRatio = 1.0f;
            options.fixedAnchorSize = true;

            anchors = SsdAnchorCalculator.Generate(options);
        }

        /*
        public void read(float[][] output)
        {
            for (int i = 0; i < 896; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    output0[i, j] = output[0][j * 896 + i];
                }
                output1[i] = output[1][i];
            }
        }
        //*/

        private static float Sigmoid(float x)
        {
            return (1.0f / (1.0f + Mathf.Exp(-x)));
        }

        internal List<FaceDetectorResult> GetResults(float[][] output, float scoreThreshold, float iouThreshold)
        {
            // regressors / points
            // 0 - 3 are bounding box offset, width and height: dx, dy, w ,h
            // 4 - 15 are 6 keypoint x and y coordinates: x0,y0,x1,y1,x2,y2,x3,y3
            float[] output0 = output[0];

            // classificators / scores
            float[] output1 = output[1];

            results.Clear();



            for (int i = 0; i < anchors.Length; i++)
            {
                float score = Sigmoid(output1[i]);
                if (score < scoreThreshold)
                {
                    continue;
                }
                SsdAnchor anchor = anchors[i];

                float cx = 0;
                float cy = 0;
                float w = 0;
                float h = 0;
                Vector2[] keypoints = new Vector2[KEY_POINT_SIZE];

                FaceLogic.GetResultsInternal(ref output0, ref i, OUTPUT_SIZE, ref anchor.x, ref anchor.y, ref width, ref height, KEY_POINT_SIZE, ref cx, ref cy, ref w, ref h, ref keypoints);

                results.Add(new FaceDetectorResult(score, new FaceRect(cx - w * 0.5f, cy - h * 0.5f, w, h), keypoints));
            }

            return NonMaxSuppression(results, iouThreshold);
        }

        private List<FaceDetectorResult> NonMaxSuppression(List<FaceDetectorResult> results, float iouThreshold)
        {
            filterdResults.Clear();

            foreach (FaceDetectorResult original in results.OrderByDescending(o => o.score))
            {
                bool ignoreCandidate = false;
                foreach (FaceDetectorResult newResult in filterdResults)
                {
                    float iou = original.rect.IntersectionOverUnion(newResult.rect);
                    if (iou >= iouThreshold)
                    {
                        ignoreCandidate = true;
                        break;
                    }
                }

                if (!ignoreCandidate)
                {
                    filterdResults.Add(original);
                    if (filterdResults.Count >= MAX_FACE_NUM)
                    {
                        break;
                    }
                }
            }
            return filterdResults;
        }
    }
}

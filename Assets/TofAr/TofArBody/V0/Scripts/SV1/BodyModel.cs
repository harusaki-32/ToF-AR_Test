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
using System.Collections.Generic;
using System.Linq;
using TensorFlowLite.Runtime;
using UnityEngine;

namespace TofAr.V0.Body.SV1
{
    /// <summary>
    /// 関節ポジション
    /// </summary>
    [Obsolete("SV1 Body recognition will remove in future version")]
    public struct JointPosition
    {
        /// <summary>
        /// 位置
        /// </summary>          
        public Vector3 pos;

        /// <summary>
        /// コンストラクタ
        /// </summary>          
        public JointPosition(float posX, float posY, float posZ)
        {
            pos = new Vector3(posX, posY, posZ);
        }
    }

    /// <summary>
    /// Body認識結果のサイズ情報
    /// </summary>
    [Obsolete("SV1 Body recognition will remove in future version")]
    public struct BodySize
    {
        /// <summary>
        /// Depthデータの幅
        /// </summary>          
        public int srcWidth;

        /// <summary>
        /// Depthデータの高さ
        /// </summary>          
        public int srcHeight;

        /// <summary>
        /// インプットデータの幅
        /// </summary>          
        public int width;

        /// <summary>
        /// インプットデータの高さ
        /// </summary>          
        public int height;

        /// <summary>
        /// Depthデータからインプットデータへのスケール係数
        /// </summary>          
        public float scale;

        /// <summary>
        /// アウトプットデータの幅
        /// </summary>          
        public int wOut;

        /// <summary>
        /// アウトプットデータの高さ
        /// </summary>          
        public int hOut;

        /// <summary>
        /// インプットデータからアウトプットデータへのスケール係数
        /// </summary>          
        public float scaleToOri;

        /// <summary>
        /// Depthデータの最小値
        /// </summary>          
        public float invalidLimitMin;

        /// <summary>
        /// Depthデータの最大値
        /// </summary>          
        public float invalidLimitMax;

        /// <summary>
        /// 関節数
        /// </summary>          
        public int jointNum;

        /// <summary>
        /// コンストラクタ
        /// </summary>          
        public BodySize(int srcWidth, int srcHeight,
                      int width, int height, float scale,
                      int wOut, int hOut, float scaleToOri,
                      float invalidLimitMin, float invalidLimitMax, int jointNum)
        {
            this.srcWidth = srcWidth;
            this.srcHeight = srcHeight;
            this.width = width;
            this.height = height;
            this.scale = scale;
            this.wOut = wOut;
            this.hOut = hOut;
            this.scaleToOri = scaleToOri;
            this.invalidLimitMin = invalidLimitMin;
            this.invalidLimitMax = invalidLimitMax;
            this.jointNum = jointNum;
        }
    }

    /// <summary>
    /// Body 認識結果 (全身、上半身共通）
    /// </summary>
    //public struct BodyResult
    //{
    //    public JointPosition?[] joints;
    //    public float elapseTime;
    //    public int performanceCount;
    //    public double averageTime;
    //    // カメラから身体までの距離
    //    public float bodyDistance;
    //}

    /// <summary>
    /// 全身の関節
    /// </summary>
    [Obsolete("SV1 Body recognition will remove in future version")]
    public enum JointFull
    {
        /// <summary>
        /// 骨盤
        /// </summary>
        Pelvis,

        /// <summary>
        /// 脊椎
        /// </summary>
        Spine,

        /// <summary>
        /// 首
        /// </summary>
        Neck,

        /// <summary>
        /// 右肩
        /// </summary>
        ShoulderRight,

        /// <summary>
        /// 右肘
        /// </summary>
        ElbowRight,

        /// <summary>
        /// 右手首
        /// </summary>
        WristRight,

        /// <summary>
        /// 左肩
        /// </summary>
        ShoulderLeft,

        /// <summary>
        /// 左肘
        /// </summary>
        ElbowLeft,

        /// <summary>
        /// 左手首
        /// </summary>
        WristLeft,

        /// <summary>
        /// 右腰
        /// </summary>
        HipRight,

        /// <summary>
        /// 右膝
        /// </summary>
        KneeRight,

        /// <summary>
        /// 右足首
        /// </summary>
        AnkleRight,

        /// <summary>
        /// 左腰
        /// </summary>
        HipLeft,

        /// <summary>
        /// 左膝
        /// </summary>
        KneeLeft,

        /// <summary>
        /// 左足首
        /// </summary>
        AnkleLeft,

        /// <summary>
        /// 頭
        /// </summary>
        Head,
        //Nose,  // may be unused in full body tracking. because BodySize.jointNum assigned 16 
        //EyeRight,
        //EyeLeft
    }

    /// <summary>
    /// 上半身の関節
    /// </summary>
    [Obsolete("SV1 Body recognition will remove in future version")]
    public enum JointUpper
    {
        /// <summary>
        /// 脊椎
        /// </summary>
        Spine,

        /// <summary>
        /// 首
        /// </summary>
        Neck,

        /// <summary>
        /// 右肩
        /// </summary>
        ShoulderRight,

        /// <summary>
        /// 右肘
        /// </summary>
        ElbowRight,

        /// <summary>
        /// 右手首
        /// </summary>
        WristRight,

        /// <summary>
        /// 左肩
        /// </summary>
        ShoulderLeft,

        /// <summary>
        /// 左肘
        /// </summary>
        ElbowLeft,

        /// <summary>
        /// 左手首
        /// </summary>
        WristLeft,

        /// <summary>
        /// 頭
        /// </summary>
        Head,

        /// <summary>
        /// 鼻
        /// </summary>
        Nose,

        /// <summary>
        /// 右目
        /// </summary>
        EyeRight,

        /// <summary>
        /// 左目
        /// </summary>
        EyeLeft
    }

    /// <summary>
    /// Body認識モデル
    /// </summary>
    [Obsolete("SV1 Body recognition will remove in future version")]
    public class BodyModel
    {
        /// <summary>
        /// *TODO+ B
        /// 全身推定データのファイル名
        /// </summary>          
        internal const string FULL_BODY_TFL_PATH = "tflite/body.tflite";
        /// <summary>
        /// *TODO+ B
        /// 上半身推定データのファイル名
        /// </summary>          
        internal const string UPPER_BODY_TFL_PATH = "tflite/bodyu.tflite";
        // DOI
        /// <summary>
        /// *TODO+ B
        /// 2D->3D変換用データのファイル名
        /// </summary>          
        internal const string FULL_BODY_2D3D_TFL_PATH = "tflite/2d3d.tflite";
        /// <summary>
        /// *TODO+ B
        /// デバイス内の全身推定データのパス
        /// </summary>          
        internal const string FULL_BODY_DEVEL_TFL_PATH = "/data/local/tmp/body/tflite/body.tflite";
        /// <summary>
        /// *TODO+ B
        /// デバイス内の上半身推定データのパス
        /// </summary>          
        internal const string UPPER_BODY_DEVEL_TFL_PATH = "/data/local/tmp/body/tflite/bodyu.tflite";

        /// <summary>
        /// *TODO+ B
        /// full（開発者用）
        /// </summary>          
        internal const string FULL_BODY = "full";
        /// <summary>
        /// *TODO+ B
        /// upper（開発者用）
        /// </summary>          
        internal const string UPPER_BODY = "upper";
        /// <summary>
        /// *TODO+ B
        /// devel（開発者用）
        /// </summary>          
        internal const string DEVEL = "devel";
        /// <summary>
        /// *TODO+ B
        /// devel_full（開発者用）
        /// </summary>          
        internal const string FULL_BODY_DEVEL = DEVEL + "_" + FULL_BODY;
        /// <summary>
        /// *TODO+ B
        /// devel_upper（開発者用）
        /// </summary>          
        internal const string UPPER_BODY_DEVEL = DEVEL + "_" + UPPER_BODY;

        private static readonly BodySize fullBodySize =
            new BodySize(
                srcWidth: 240, srcHeight: 320,
                width: 120, height: 160, scale: 2 /* srcWidth / width */,
                wOut: 15 /* width / 4 */, hOut: 20 /* height / 4 */, scaleToOri: 8 /* width / wOut */,
                invalidLimitMin: 1000, invalidLimitMax: 3500, jointNum: 16);
        //private static readonly BodySize fullBodySize =
        //    new BodySize(
        //        srcWidth: 240, srcHeight: 320,
        //        width: 60, height: 80, scale: 4 /* srcWidth / width */,
        //        wOut: 15 /* width / 4 */, hOut: 20 /* height / 4 */, scaleToOri: 4 /* width / wOut */,
        //        invalidLimitMin: 1000, invalidLimitMax: 3500, jointNum: 16);


        private static readonly BodySize upperBodySize =
            new BodySize(
                srcWidth: 480, srcHeight: 640,
                width: 60, height: 80, scale: 8 /* srcWidth / width */,
                wOut: 15 /* width / 4 */, hOut: 20 /* height / 4 */, scaleToOri: 4 /* width / wOut */,
                invalidLimitMin: 200, invalidLimitMax: 1500, jointNum: 12);

        private static readonly int[][] fullVecPairs = {
            new int[]{0, 1},    //  0: pelvis -> spine
            new int[]{1, 2},    //  1: spine -> neck
            new int[]{1, 3},    //  2: spine -> shoulder_r
            new int[]{3, 4},    //  3: shoulder_r -> elbow_r
            new int[]{4, 5},    //  4: elbow_r -> wrist_r
            new int[]{1, 6},    //  5: spine -> shoulder_l
            new int[]{6, 7},    //  6: shoulder_l -> elbow_l
            new int[]{7, 8},    //  7: elbow_l -> wrist_l
            new int[]{0, 9},    //  8: pelvis -> hip_r
            new int[]{9, 10},   //  9: hip_r -> knee_r
            new int[]{10, 11},  // 10: knee_r -> ankle_r
            new int[]{0, 12},   // 11: pelvis-> hip_l
            new int[]{12, 13},  // 12: hip_l -> knee_l
            new int[]{13, 14},  // 13: knee_l -> ankle_l
            new int[]{2, 15},   // 14: neck -> head
            new int[]{15, 16},  // 15: head -> nose
            new int[]{16, 17},  // 16: nose -> eye_r
            new int[]{16, 18},  // 17: nose -> eye_l
        };

        private static readonly int[][] upperVecPairs = {
            new int[]{0, 1},    //  0: spine -> neck
            new int[]{0, 2},    //  1: spine -> shoulder_r
            new int[]{2, 3},    //  2: shoulder_r -> elbow_r
            new int[]{3, 4},    //  3: elbow_r -> wrist_r
            new int[]{0, 5},    //  4: spine -> shoulder_l
            new int[]{5, 6},    //  5: shoulder_l -> elbow_l
            new int[]{6, 7},    //  6: elbow_l -> wrist_l
            new int[]{1, 8},    //  7: neck -> head
            new int[]{8, 9},    //  8: head -> nose
            new int[]{9, 10},   //  9: nose -> eye_r
            new int[]{9, 11},   // 10: nose -> eye_l
        };

        // 全身推定時の身体への距離算出に使う関節インデックス
        private static readonly int[] fullBodyJoints =
        {
            0, 1
        };

        // 上半身推定時の身体への距離算出に使う関節インデックス
        private static readonly int[] upperBodyJoints =
        {
            0, 1, 2, 5, 8, 9, 10, 11
        };

        private static readonly float threshLevel = 0.3f;

        // Depth -> Joints (2D)
        private TFLiteRuntime tfl;
        // Joints (2D) -> Joints Z (3D) DOI
        private TFLiteRuntime tfl2;

        //private int[][] vecPairs = fullVecPairs;
        private int[] bodyDepthJoints = fullBodyJoints;
        private BodySize bodySize = fullBodySize;

        /// <summary>
        /// trueの場合は全身を認識する, falseの場合は上半身だけ認識する
        /// </summary>          
        public bool isFullbody = true;

        // For buffering
        // TODO: Need to more consideration for buffering
        private object bufLock = new object();
        private const int bufNum = 1;   // Set '1', No buffering
        private int bufIndex = 0;
        private float[][] srcData = new float[bufNum][];
        private BodyResult[] resultData = new BodyResult[bufNum];

        private float[][] joints;
        private SortedDictionary<object, float[]>[] peakMaps;
        private bool[] is_detected;
        private float[] depthList;
        private float[] depthListHand = new float[25];

        private int tofWidth, tofHeight;
        private float tofFx, tofFy;
        private int rotation;

        private int height_ofs;
        private int width_ofs;

        private float bodyDistance;

        private string previousModelFile;
        private TFLiteRuntime.ExecMode previousExecMode;
        private int previousthreadsNum;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(string modelFile, TFLiteRuntime.ExecMode execMode, bool isFullbody, int performanceCount = 0, int threadsNum = 1)
        {
            for (var i = 0; i < this.resultData.Length; i++)
            {
                this.resultData[i] = new BodyResult();
            }
            TFLiteRuntime.SetPerformanceCount(performanceCount);
            if (previousExecMode != execMode || previousModelFile != modelFile || previousthreadsNum != threadsNum)
            {
                tfl?.Dispose();
                try
                {
                    tfl = new TFLiteRuntime(modelFile, execMode, threadsNum, 1);
                }
                catch (Exception e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"[BodyModel Init] Failed to initialize TFLite: {e.Message}");
                }
                finally
                {
                    if (modelFile != null && System.IO.File.Exists(modelFile))
                    {
                        System.IO.File.Delete(modelFile);
                    }
                }
            }

            previousthreadsNum = threadsNum;
            previousModelFile = modelFile;
            previousExecMode = execMode;

            // DOI
            if (tfl2 == null)
            {
                string modelFile2 = LoadAsset.LoadFileFromResources(FULL_BODY_2D3D_TFL_PATH);

                try
                {
                    tfl2 = new TFLiteRuntime(modelFile2, TFLiteRuntime.ExecMode.EXEC_MODE_CPU, 1, 1);
                }
                catch (Exception e)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, $"[BodyModel Init] Failed to initialize TFLite: {e.Message}");
                }
                finally
                {
                    if (modelFile2 != null && System.IO.File.Exists(modelFile2))
                    {
                        System.IO.File.Delete(modelFile2);
                    }
                }
            }

            LoadAsset.RemoveLocalFiles();

            //*/

            this.bodySize = isFullbody ? fullBodySize : upperBodySize;
            //this.vecPairs = isFullbody ? fullVecPairs : upperVecPairs;
            this.bodyDepthJoints = isFullbody ? fullBodyJoints : upperBodyJoints;
            this.isFullbody = isFullbody;
            this.depthList = isFullbody ? new float[27] : new float[72 + (bodySize.width * bodySize.height)];

            for (int i = 0; i < bufNum; i++)
            {
                srcData[i] = new float[bodySize.height * bodySize.width];
            }
        }

        /// <summary>
        /// Body認識の情報を取得する
        /// </summary>
        public BodySize GetBodySize()
        {
            return bodySize;
        }

        /// <summary>
        /// 処理するDepthデータを設定する
        /// </summary>
        public void SetSrcData(short[] data, int height, int width, float fx, float fy, int rotation)
        {
            this.tofWidth = width;
            this.tofHeight = height;
            this.tofFx = fx;
            this.tofFy = fy;
            this.rotation = rotation;

            // Lock >->->->->->->->->->->->->->->->->->->->->->->->->->->->->->-
            lock (bufLock)
            {
                // Cuts the parts that do not match the aspect ratio (height:width=4:3)
                const int height_ratio = 4;
                const int width_ratio = 3;

                //  Swap width and height, because Landscape Left
                int valid_height = height;
                int valid_width = width;

                if (rotation == 90 || rotation == 270)
                {
                    valid_height = width;
                    valid_width = height;
                }

                int omit_height = 0;
                int omit_width = 0;

                if ((valid_width * height_ratio) < (valid_height * width_ratio))
                {
                    omit_width = valid_width % width_ratio;
                    valid_width -= omit_width;
                    omit_height = valid_height - (height_ratio * valid_width) / width_ratio;
                    valid_height -= omit_height;
                }
                else if ((valid_width * height_ratio) > (valid_height * width_ratio))
                {
                    omit_height = valid_height % height_ratio;
                    valid_height -= omit_height;
                    omit_width = valid_width - (width_ratio * valid_height) / height_ratio;
                    valid_width -= omit_width;
                }

                //OnLogOutput?.Invoke($"Valid src size: (H:{width}, W:{height}) -> (H:{valid_height}, W:{valid_width}): omit(H:{omit_height}, W:{omit_width})");


                // Offset for center
                this.height_ofs = omit_height / 2;
                this.width_ofs = omit_width / 2;



                // Update body Size to match as Input
                bodySize.srcHeight = valid_height;
                bodySize.srcWidth = valid_width;
                bodySize.scale = (float)bodySize.srcWidth / bodySize.width;

                // Set to back buffer
                int back_buf = (bufIndex + 1) % bufNum;

                float[] src = srcData[back_buf];

                for (int y = 0; y < bodySize.height; y++)
                {
                    for (int x = 0; x < bodySize.width; x++)
                    {
                        int posX = (int)(x * bodySize.scale + width_ofs);
                        int posY = (int)(y * bodySize.scale + height_ofs);

                        // Transposition and Sampling
                        int i = 0;

                        switch (rotation)
                        {
                            case 0:
                                i = posY * width + posX;
                                break;
                            case 90:
                                i = posX * width + (width - 1 - posY);
                                break;
                            case 180:
                                i = (height - 1 - posY) * width + (width - 1 - posX);
                                break;
                            case 270:
                                i = (height - 1 - posX) * width + posY;
                                break;

                        }

                        int j = y * bodySize.width + x;

                        // Normalize to 0.0-1.0
                        float depth8 = Math.Max(0, Math.Min(255, (data[i] - bodySize.invalidLimitMin) * 255 / (bodySize.invalidLimitMax - bodySize.invalidLimitMin)));
                        src[j] = depth8;
                    }
                }

            }
            // Unlock <-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-
        }

        private void PrepareInput(float[] src)
        {
            // Expected single Input layer
            float[] input = tfl.getInputBuffer()[0];

            // Lock >->->->->->->->->->->->->->->->->->->->->->->->->->->->->->-
            lock (bufLock)
            {
                float div = 1.0f / 255;

                for (int y = 0; y < bodySize.height; y++)
                {
                    for (int x = 0; x < bodySize.width; x++)
                    {
                        int i = y * bodySize.width + x;
                        input[i] = src[i] * div;
                    }
                }

                // Next buffer
                bufIndex = ++bufIndex % bufNum;
            }
            // Unlock <-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-
        }

        private float GetBodyDistance(float[] src)
        {
            Array.Clear(this.depthList, 0, this.depthList.Length);


            int depthListSize = 0;
            int minDepth = (int)Mathf.Max(bodySize.invalidLimitMin, 500);
            for (int i = 0; i < bodyDepthJoints.Length; i++)
            {
                int j = bodyDepthJoints[i];
                if (!is_detected[j])
                {
                    continue;
                }
                int x = (int)(joints[j][0] * bodySize.scaleToOri); //joints[j][0] [0..15] -> x [0..120]
                int y = (int)(joints[j][1] * bodySize.scaleToOri); //joints[j][1] [0..20] -> y [0..160]

                // Check value of the around 3x3 pixel
                for (int dy = -1; dy <= 1; dy++)
                {
                    int yy = y + dy;
                    if (yy < 0 || yy >= bodySize.height)
                    {
                        continue;
                    }
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        int xx = x + dx;
                        if (xx < 0 || xx >= bodySize.width)
                        {
                            continue;
                        }
                        int pos = yy * bodySize.width + xx;

                        float depth8 = src[pos];
                        float depthmm = (depth8 * (bodySize.invalidLimitMax - bodySize.invalidLimitMin) / 255.0f) + bodySize.invalidLimitMin;

                        // Exclude invalid distance
                        if (minDepth >= depthmm || bodySize.invalidLimitMax <= depthmm)
                        {
                            continue;
                        }
                        this.depthList[depthListSize++] = depthmm;
                    }
                }
            }

            // 両肩から Spine の少ししたまでの矩形を全て追加する
            if (!this.isFullbody)
            {
                if (is_detected[0] && is_detected[2] && is_detected[5])
                {
                    int sx = (int)(joints[2][0] * bodySize.scaleToOri);
                    int ex = (int)(joints[5][0] * bodySize.scaleToOri);
                    int sy1 = (int)(joints[2][1] * bodySize.scaleToOri);
                    int sy2 = (int)(joints[5][1] * bodySize.scaleToOri);
                    int sy = Mathf.Min(sy1, sy2);
                    int ey = (int)Mathf.Min(bodySize.height - 1, (joints[0][1] * bodySize.scaleToOri) + 10);

                    for (int y = sy; y <= ey; y++)
                    {
                        for (int x = sx; x <= ex; x++)
                        {
                            int pos = y * bodySize.width + x;

                            float depth8 = src[pos];
                            float depthmm = (depth8 * (bodySize.invalidLimitMax - bodySize.invalidLimitMin) / 255.0f) + bodySize.invalidLimitMin;

                            // Exclude invalid distance
                            if (minDepth >= depthmm || bodySize.invalidLimitMax <= depthmm)
                            {
                                continue;
                            }
                            this.depthList[depthListSize++] = depthmm;
                        }
                    }
                }
            }

            // Get median value
            Array.Sort(this.depthList, 0, depthListSize);
            bodyDistance = (depthListSize > 0) ? this.depthList[depthListSize * 3 / 4] : 0.0f;

            return bodyDistance;
        }

        private float GetHandDepth(int handIdx, float[] src)
        {
            int depthListSize = 0;
            Array.Clear(this.depthListHand, 0, this.depthListHand.Length);

            int x = (int)(joints[handIdx][0] * bodySize.scaleToOri);
            int y = (int)(joints[handIdx][1] * bodySize.scaleToOri);

            // Check value of the around 3x3 pixel
            for (int dy = -2; dy <= 2; dy++)
            {
                int yy = y + dy;
                if (yy < 0 || yy >= bodySize.height)
                {
                    continue;
                }
                for (int dx = -2; dx <= 2; dx++)
                {
                    int xx = x + dx;
                    if (xx < 0 || xx >= bodySize.width)
                    {
                        continue;
                    }
                    int pos = yy * bodySize.width + xx;

                    float depth8 = src[pos];
                    float depthmm = (depth8 * (bodySize.invalidLimitMax - bodySize.invalidLimitMin) / 255.0f) + bodySize.invalidLimitMin;

                    // Exclude invalid distance
                    if ((bodySize.invalidLimitMin >= depthmm || bodySize.invalidLimitMax <= depthmm))
                    {
                        continue;
                    }
                    this.depthListHand[depthListSize++] = depthmm;
                }
            }

            Array.Sort(this.depthListHand, 0, depthListSize);

            return (depthListSize == 0 ? bodyDistance + 10 : Mathf.Min(bodyDistance + 10, this.depthListHand[depthListSize / 3])) * 0.001f;
        }

        private void MapUpperBodyTo3D(float[] src, ref JointPosition?[] tmpJoints)
        {
            int wo = 15;
            int ho = 20;
            float fx = this.tofFx, fy = this.tofFy;
            int w = this.tofWidth, h = this.tofHeight;

            if (this.rotation == 90 || this.rotation == 270)
            {
                w = this.tofHeight;
                h = this.tofWidth;
                fx = this.tofFy;
                fy = this.tofFx;
            }

            // 上半身推定

            // 全ての関節の z を bodyDistance にして result に反映させる
            for (int i = 0; i < bodySize.jointNum; i++)
            {
                if (!is_detected[i] || tmpJoints[i] == null)
                {
                    continue;
                }
                JointPosition jp = (JointPosition)tmpJoints[i];

                float z = bodyDistance * 0.001f;
                float jointX2DNorm = ((joints[i][0] - wo * 0.5f) / wo); // [-0.5 .. +0.5] 
                float jointX2D = jointX2DNorm * (w - 2 * this.width_ofs); // [-67.5 .. +67.5] -> [37.5 .. 172.5]
                float jointY2DNorm = -((joints[i][1] - ho * 0.5f) / ho);// [-0.5 .. +0.5] 
                float jointY2D = jointY2DNorm * (h - 2 * this.height_ofs); // [-67.5 .. +67.5] -> [37.5 .. 172.5]   
                jp.pos.x = jointX2D * z / fx;
                jp.pos.y = jointY2D * z / fy;
                jp.pos.z = z;
                tmpJoints[i] = jp;
            }

            // 手の位置は DepthMap から求めてしまう
            int[] hands = new int[] { 4, 7 };

            for (int i = 0; i < hands.Length; i++)
            {
                int handIdx = hands[i];
                if (!is_detected[handIdx])
                {
                    continue;
                }

                float handDepth = GetHandDepth(handIdx, src);

                // 肩と手首の３次元位置、肘の２次元位置から、肘の z を解く
                // 肩から肘、肘から手首への距離が等しいとする。

                if (tmpJoints[handIdx] != null)
                {
                    JointPosition jp = (JointPosition)tmpJoints[handIdx];

                    float handX2DNorm = ((joints[handIdx][0] - wo * 0.5f) / wo);
                    float handX2D = handX2DNorm * (w - 2 * this.width_ofs);
                    float handY2DNorm = -((joints[handIdx][1] - ho * 0.5f) / ho);
                    float handY2D = handY2DNorm * (h - 2 * this.height_ofs);

                    jp.pos.x = handX2D * handDepth / fx;
                    jp.pos.y = handY2D * handDepth / fy;
                    jp.pos.z = handDepth;

                    tmpJoints[handIdx] = jp;
                }

                //elbow肘位置

                int elbow_n = handIdx - 1;                            //肘のJoint番号（手のJoint番号 - 1）
                int shoulder_n = handIdx - 2;                          //肩のJoint番号（肩のJoint番号 - 2）

                //肩、肘、手の座標すべてが推定されている場合のみ、肘のZを算出
                if (tmpJoints[elbow_n] != null && tmpJoints[handIdx] != null && tmpJoints[shoulder_n] != null)
                {
                    JointPosition jp = (JointPosition)tmpJoints[elbow_n];
                    JointPosition jp_Shoulder = (JointPosition)tmpJoints[shoulder_n];
                    JointPosition jp_Hand = (JointPosition)tmpJoints[handIdx];

                    //肘位置　Elbow?Z　を求める
                    //肩‐肘、肘‐手の3次元距離が等しいと仮定（ Vector3.Distance(肩、肘) = Vector3.Distance(肘、手))を解く
                    float handSquare = jp_Hand.pos.x * jp_Hand.pos.x + jp_Hand.pos.y * jp_Hand.pos.y + jp_Hand.pos.z * jp_Hand.pos.z;
                    float sholderSquare = jp_Shoulder.pos.x * jp_Shoulder.pos.x + jp_Shoulder.pos.y * jp_Shoulder.pos.y + jp_Shoulder.pos.z * jp_Shoulder.pos.z;

                    //肘の2D座標橋推定されているため3D座標（x,y）は　Elbow?Z　を用いて表すことができる。

                    float elbowX2DNorm = (joints[elbow_n][0] - wo * 0.5f) / wo;
                    float elbowX2D = elbowX2DNorm * (w - 2 * this.width_ofs);
                    float elbowY2DNorm = -((joints[elbow_n][1] - ho * 0.5f) / ho);
                    float elbowY2D = elbowY2DNorm * (h - 2 * this.height_ofs);

                    float xToZ = elbowX2D * 1 / fx;
                    float yToZ = elbowY2D * 1 / fy;

                    //連立方程式の解
                    float elbowz = (handSquare - sholderSquare) / (2.0f * (xToZ * (jp_Hand.pos.x - jp_Shoulder.pos.x) + (yToZ * (jp_Hand.pos.y - jp_Shoulder.pos.y)) + (jp_Hand.pos.z - jp_Shoulder.pos.z)));

                    jp.pos.z = elbowz;
                    jp.pos.x = elbowX2D * elbowz / fx;
                    jp.pos.y = elbowY2D * elbowz / fy;
                    //手と肩の中間座標を算出
                    Vector3 armCenter = (jp_Hand.pos + jp_Shoulder.pos) / 2.0f;

                    //カメラの向きと肘ー手ベクトルの内積を求める
                    var cameraLine = jp.pos;
                    var bodyLine = jp_Hand.pos - jp_Shoulder.pos;
                    float val = Vector3.Dot(bodyLine, cameraLine);

                    //肩と手ベクトルとカメラ向きベクトルが直交してる場合、肘-手-(肩＆手の)中間点を三角形として中間点より肘の距離を算出
                    if (Mathf.Abs(val) < 0.13f)
                    {
                        //肘・手・手と肩の中心点で三角形を作成、三平方の定理から肘のベクトルを算出
                        float length = Mathf.Sqrt(armLength * armLength - Vector3.Distance(armCenter, jp_Hand.pos) * Vector3.Distance(armCenter, jp_Hand.pos));
                        float centerSquare = armCenter.x * armCenter.x + armCenter.y * armCenter.y + armCenter.z * armCenter.z;

                        //解の公式                                    
                        float a = 1.0f;
                        float b = -2.0f * (armCenter.z);
                        float c = centerSquare - length * length + jp.pos.x * jp.pos.x + jp.pos.y * jp.pos.y - 2 * (armCenter.x * jp.pos.x + armCenter.y * jp.pos.y);

                        float answer = (-b + Mathf.Sqrt(b * b - 4.0f * a * c)) / (2.0f * a);
                        float answer2 = (-b - Mathf.Sqrt(b * b - 4.0f * a * c)) / (2.0f * a);

                        if (!float.IsNaN(answer) && !float.IsNaN(answer2))
                        {
                            Vector3 ans;
                            ans.z = answer;
                            ans.x = elbowX2D * answer / fx;
                            ans.y = elbowY2D * answer / fy;

                            //要調整・要検討
                            if (Vector3.Distance(jp_Hand.pos, ans) > 0.32f)
                            {
                                elbowz = answer2;
                            }
                            else
                            {
                                elbowz = answer;
                            }
                        }

                        if (float.IsNaN(Mathf.Sqrt(b * b - 4 * a * c) / (2 * a)) || float.IsNaN(length))
                        {
                            elbowz = armCenter.z;
                        }
                    }

                    jp.pos.x = elbowX2D * elbowz / fx;
                    jp.pos.y = elbowY2D * elbowz / fy;
                    jp.pos.z = elbowz;
                    tmpJoints[elbow_n] = jp;
                    //手-肘、肘ー肩間の長さを保存しておく
                    if (Vector3.Distance(jp.pos, jp_Hand.pos) < 0.3f && Vector3.Distance(jp.pos, jp_Hand.pos) > 0.25f)
                    {
                        this.armLength = Vector3.Distance(jp.pos, jp_Hand.pos);
                    }
                }
            }
        }

        private void MapFullBodyTo3D(ref JointPosition?[] tmpJoints)
        {
            int wo = 15;
            int ho = 20;
            float fx = this.tofFx, fy = this.tofFy;
            int w = this.tofWidth, h = this.tofHeight;

            if (this.rotation == 90 || this.rotation == 270)
            {
                w = this.tofHeight;
                h = this.tofWidth;
                fx = this.tofFy;
                fy = this.tofFx;
            }


            // 全身推定

            // DNN による三次元位置推定
            float[] input2 = tfl2.getInputBuffer()[0];

            // Pelvis は原点 O にする。推論の入力データには含めない
            // 各関節は根本関節からの相対座標にして入力データにいれる
            for (int i = 0; i < bodySize.jointNum - 1; i++)
            {
                int r = fullVecPairs[i][0];
                // x
                float boneXNorm = (joints[i + 1][0] - joints[r][0]) / wo; // boneXNorm [0..1]
                float boneX = boneXNorm * (w - 2 * this.width_ofs); // boneX [0..180] X   => [0..135]
                input2[i * 2 + 0] = boneX * bodyDistance / fx;
                // y
                float boneYNorm = (joints[i + 1][1] - joints[r][1]) / ho;
                float boneY = boneYNorm * (h - 2 * this.height_ofs);
                input2[i * 2 + 1] = boneY * bodyDistance / fy;
            }

            // nn2 
            float[] ob2 = tfl2.forward()[0];

            // Pelvis は原点
            joints[0][2] = bodyDistance;
            // 根本座標からの相対値を、絶対値へ変換していく。
            for (int i = 1; i < bodySize.jointNum; i++)
            {
                joints[i][2] = ob2[i - 1];
                int r = fullVecPairs[i - 1][0];
                joints[i][2] += joints[r][2];
            }

            // result に反映させる
            for (int i = 0; i < bodySize.jointNum; i++)
            {
                if (!is_detected[i] && tmpJoints[i] == null)
                {
                    continue;
                }
                JointPosition jp = (JointPosition)tmpJoints[i];

                float z = joints[i][2] * 0.001f;

                float jointX2DNorm = ((joints[i][0] - wo * 0.5f) / wo);
                float jointX2D = jointX2DNorm * (w - 2 * this.width_ofs); // boneX [0..180] X   => [0..135]
                float jointY2DNorm = -((joints[i][1] - ho * 0.5f) / ho);
                float jointY2D = jointY2DNorm * (h - 2 * this.height_ofs); // boneY [0..240] X   => [0..180]

                jp.pos.x = (jointX2D) * z / fx;
                jp.pos.y = (jointY2D) * z / fy;
                jp.pos.z = z;
                tmpJoints[i] = jp;
            }
        }

        private void MapTo3D(float[] src, ref JointPosition?[] tmpJoints)
        {


            if (this.isFullbody)
            {
                MapFullBodyTo3D(ref tmpJoints);
            }
            else
            {
                MapUpperBodyTo3D(src, ref tmpJoints);
            }
        }

        private float armLength = 0.3f;

        /// <summary>
        /// Body認識を行う
        /// </summary>
        public BodyResult Run()
        {
            if (bufIndex > resultData.Length || bufIndex > srcData.Length)
            {
                return null;
            }

            BodyResult result = resultData[bufIndex];
            if (result == null)
            {
                return null;
            }
            result.bodyShot = this.isFullbody ? SV1.BodyShot.FullBody : SV1.BodyShot.UpperBody;
            foreach (var j in result.joints)
            {
                j.tracked = false;
            }

            float[] src = srcData[bufIndex];

            PrepareInput(src);

            float[] output = tfl.forward()[0];

            int jointNum = bodySize.jointNum;
            var tmpJoints = new JointPosition?[jointNum];
            for (int i = 0; i < jointNum; i++)
            {
                tmpJoints[i] = null;
            }

            if (joints == null || joints.Length != jointNum)
            {
                joints = new float[jointNum][];
                for (int i = 0; i < jointNum; i++)
                {
                    joints[i] = new float[3];
                }
            }

            if (peakMaps == null || peakMaps.Length != jointNum)
            {
                peakMaps = new SortedDictionary<object, float[]>[jointNum];
            }

            // joint
            for (int i = 0; i < jointNum; i++)
            {
                // DOI z まで含めるので 3 へ変更
                peakMaps[i] = GetMaxPositions(output, bodySize.wOut * bodySize.hOut * i, threshLevel, 5, bodySize);
            }

            if (is_detected == null || is_detected.Length != jointNum)
            {
                is_detected = new bool[jointNum];
            }


            // check joint
            for (int i = 0; i < jointNum; i++)
            {
                is_detected[i] = (peakMaps[i].Count > 0);
                if (!is_detected[i])
                {
                    continue;
                }
                float[] xy = peakMaps[i].Last().Value;
                joints[i][0] = xy[0];
                joints[i][1] = xy[1];

                tmpJoints[i] = new JointPosition(joints[i][0], joints[i][1], 0.0f);
            }


            // calculate body distance (calculate by pelvis and spine points)


            float bodyDistance = GetBodyDistance(src);

            if (bodyDistance == 0)
            {
                tmpJoints[0] = null;
            }
            else
            {
                MapTo3D(src, ref tmpJoints);
            }

            this.RemapResult(tmpJoints, result);

            return result;
        }

        private static SortedDictionary<object, float[]> PopulatePeakMap(float[] fa, int offset, float levelTh, int maxCount, BodySize bodySize)
        {
            SortedDictionary<object, float[]> peakMap = new SortedDictionary<object, float[]>();

            int[] dxs = { 0, -1, 1, 0 };
            int[] dys = { -1, 0, 0, 1 };

            for (int y = 0; y < bodySize.hOut; y++)
            {
                for (int x = 0; x < bodySize.wOut; x++)
                {
                    float value = fa[offset + y * bodySize.wOut + x];

                    if (value < levelTh)
                    {
                        continue;
                    }
                    // Surrounding check
                    bool isPeak = true;
                    for (int j = 0; j < 4; j++)
                    {
                        int yy = y + dys[j];
                        if (yy < 0 || bodySize.hOut <= yy)
                        {
                            continue;
                        }
                        int xx = x + dxs[j];
                        if (xx < 0 || bodySize.wOut <= xx)
                        {
                            continue;
                        }
                        float nvalue = fa[offset + yy * bodySize.wOut + xx];
                        if (nvalue > value)
                        {
                            isPeak = false;
                            break;
                        }
                    }

                    if (!isPeak)
                    {
                        continue;
                    }
                    float key = value;
                    const int maxTry = 100;
                    int tryCount = 0;
                    while (peakMap.ContainsKey(key) && key < 1f && (tryCount++ < maxTry))
                    {
                        key += 0.01f;
                    }

                    peakMap.Add(key, new float[] { x, y });
                }
            }

            // If it exceeds maxCount, delete from smaller ones
            while (peakMap.Count > maxCount)
            {
                object key = peakMap.First().Key;
                peakMap.Remove(key);
            }

            return peakMap;
        }

        /// <summary>
        /// float[] は、DNN 出力層での画像解像度(15x20)での座標
        /// </summary>
        /// <param name="fa"></param>
        /// <param name="offset"></param>
        /// <param name="levelTh"></param>
        /// <param name="maxCount"></param>
        /// <param name="bodySize"></param>
        /// <returns></returns>
        private static SortedDictionary<object, float[]> GetMaxPositions(float[] fa, int offset, float levelTh, int maxCount, BodySize bodySize)
        {
            SortedDictionary<object, float[]> peakMap = PopulatePeakMap(fa, offset, levelTh, maxCount, bodySize);

            foreach (KeyValuePair<object, float[]> entry in peakMap)
            {
                float[] xy = entry.Value;
                int cx = (int)(xy[0] + 0.5f);
                int cy = (int)(xy[1] + 0.5f);

                float sx = 0, sy = 0, sc = 0;

                int ddx = (cx == 0 || cx == bodySize.wOut - 1 ? 0 : 1);
                int ddy = (cy == 0 || cy == bodySize.hOut - 1 ? 0 : 1);

                for (int dy = -ddy; dy <= ddy; dy++)
                {
                    int y = cy + dy;
                    for (int dx = -ddx; dx <= ddx; dx++)
                    {
                        int x = cx + dx;
                        float value = Math.Max(0, fa[offset + y * bodySize.wOut + x]);

                        sx += dx * value;
                        sy += dy * value;
                        sc += value;
                    }
                }
                sx /= sc;
                sy /= sc;

                xy[0] = cx + sx;
                xy[1] = cy + sy;
            }

            return peakMap;
        }


        private IDictionary<JointFull, JointIndices> remappedJointFull = new Dictionary<JointFull, JointIndices>()
        {
            { JointFull.Pelvis, JointIndices.Spine1 },
            { JointFull.Spine, JointIndices.Spine4 },
            { JointFull.Neck, JointIndices.Neck1 },
            { JointFull.ShoulderRight, JointIndices.RightShoulder1},
            { JointFull.ElbowRight, JointIndices.RightForearm },
            { JointFull.WristRight, JointIndices.RightHand },
            { JointFull.ShoulderLeft, JointIndices.LeftShoulder1 },
            { JointFull.ElbowLeft, JointIndices.LeftForearm },
            { JointFull.WristLeft, JointIndices.LeftHand},
            { JointFull.HipRight, JointIndices.RightUpLeg },
            { JointFull.KneeRight, JointIndices.RightLeg },
            { JointFull.AnkleRight, JointIndices.RightFoot },
            { JointFull.HipLeft, JointIndices.LeftUpLeg },
            { JointFull.KneeLeft, JointIndices.LeftLeg },
            { JointFull.AnkleLeft, JointIndices.LeftFoot },
            { JointFull.Head, JointIndices.Head },
//            { JointFull.Nose, JointIndices.Nose },
//            { JointFull.EyeRight, JointIndices.RightEye },
//            { JointFull.EyeLeft, JointIndices.LeftEye },
        };

        private IDictionary<JointUpper, JointIndices> remappedJointUpper = new Dictionary<JointUpper, JointIndices>()
        {
            { JointUpper.Spine, JointIndices.Spine4 },
            { JointUpper.Neck, JointIndices.Neck1 },
            { JointUpper.ShoulderRight, JointIndices.RightShoulder1 },
            { JointUpper.ElbowRight, JointIndices.RightForearm },
            { JointUpper.WristRight, JointIndices.RightHand },
            { JointUpper.ShoulderLeft, JointIndices.LeftShoulder1 },
            { JointUpper.ElbowLeft, JointIndices.LeftForearm },
            { JointUpper.WristLeft, JointIndices.LeftHand },
            { JointUpper.Head, JointIndices.Head },
            { JointUpper.Nose, JointIndices.Nose },
            { JointUpper.EyeRight, JointIndices.RightEye },
            { JointUpper.EyeLeft, JointIndices.LeftEye },
        };

        private void RemapResult(JointPosition?[] srcJoints, BodyResult result)
        {
            var bodyTracked = false;
            if (this.isFullbody)
            {
                foreach (JointFull target in Enum.GetValues(typeof(JointFull)))
                {
                    var srcIndex = (int)target;
                    if (srcJoints[srcIndex] != null && this.is_detected[srcIndex]
                        && ((JointPosition)srcJoints[srcIndex]).pos.z >= 0)
                    {
                        var index = (int)this.remappedJointFull[target];
                        var joint = new HumanBodyJoint();
                        joint.index = index;
                        joint.anchorPose.position = new TofArVector3(((JointPosition)srcJoints[srcIndex]).pos);
                        joint.tracked = true;
                        bodyTracked = true;
                        if (srcIndex > 0)
                        {
                            var parentSrcIndex = fullVecPairs[srcIndex - 1][0];
                            var parentIndex = (int)this.remappedJointFull[(JointFull)parentSrcIndex];
                            joint.parentIndex = parentIndex;
                        }
                        result.joints[index] = (joint);
                    }
                }
            }
            else
            {
                foreach (JointUpper target in Enum.GetValues(typeof(JointUpper)))
                {
                    var srcIndex = (int)target;
                    if (srcJoints[srcIndex] != null && this.is_detected[srcIndex]
                        && ((JointPosition)srcJoints[srcIndex]).pos.z >= 0)
                    {
                        var index = (int)this.remappedJointUpper[target];
                        var joint = new HumanBodyJoint();
                        joint.index = index;
                        joint.anchorPose.position = new TofArVector3(((JointPosition)srcJoints[srcIndex]).pos);
                        joint.tracked = true;
                        bodyTracked = true;
                        if (srcIndex > 0)
                        {
                            var parentSrcIndex = upperVecPairs[srcIndex - 1][0];
                            var parentIndex = (int)this.remappedJointUpper[(JointUpper)parentSrcIndex];
                            joint.parentIndex = parentIndex;
                        }
                        result.joints[index] = joint;
                    }
                }
            }
            result.trackingState = bodyTracked ? TrackingState.Tracking : TrackingState.None;
            result.timestamp = (ulong)Utils.GetUnixTimestampAsNanoSeconds();
        }
    }
}

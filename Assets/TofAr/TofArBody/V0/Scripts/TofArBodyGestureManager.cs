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
using System.Linq;
using System;
using TensorFlowLite.Runtime;
using System.Threading;

namespace TofAr.V0.Body
{
    /// <summary>
    /// Bodyジェスチャーのインデックス
    /// </summary>  
    public enum BodyGestureJointIndex : int
    {
        /// <summary>
        /// 骨盤
        /// </summary>
        Pelvis = 0,

        /// <summary>
        /// 首
        /// </summary>
        Neck,

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
        /// 頭
        /// </summary>
        Head
    }

    /// <summary>
    /// Body認識ソースのPlatform
    /// </summary>  
    public enum BodyPlatform : int
    {
        /// <summary>
        /// SC2
        /// </summary>
        SV2,

        /// <summary>
        /// AREngine
        /// </summary>
        AREngine,

        /// <summary>
        /// ARKit
        /// </summary>
        ARKit,

        /// <summary>
        /// Others
        /// </summary>
        Others
    }

    /// <summary>
    /// Bodyジェスチャー
    /// </summary>  
    public enum BodyGesture : int
    {
        Others, StraightLeft, HookLeft, UpperLeft, StraightRight, HookRight, UpperRight,
        Iai, Search, Joudan, Furioroshi,
        Hello, Crouch, Golf1, SoccerKick, X, T, Y, I, Life, Sumo1, RugbyKick, A, Tornado,
        Batter1, Batter2, AtEase, Attention, Bow, BothHandsWaist, ForwardDress, FightingPose
    }

    /// <summary>
    /// Bodyジェスチャー管理マネージャー
    /// </summary>  

    public class TofArBodyGestureManager : Singleton<TofArBodyGestureManager>
    {
        /// <summary>
        /// ジェスチャ推定結果をコールバック
        /// </summary>
        /// <param name="result">ジェスチャー認識の結果</param>
        public delegate void GestureEstimatedEventHandler(BodyGesture result);

        /// <summary>
        /// ジェスチャ推定結果通知
        /// </summary>  
        public static event GestureEstimatedEventHandler OnGestureEstimated;

        /// <summary>
        /// 任意のフレーム数の骨格データを保存
        /// </summary>
        private List<BodyGestureData> bodyDataList = new List<BodyGestureData>();

        /// <summary>
        /// 任意のフレーム数のジェスチャ結果を保存
        /// </summary>
        private List<BodyGesture> gestureResultList = new List<BodyGesture>();

        /// <summary>
        /// 現在のポーズ情報
        /// </summary>
        public BodyGesture CurrentPose { get; private set; } = BodyGesture.Others;

        /// <summary>
        /// 直近のジェスチャ情報
        /// </summary>
        private BodyGesture lastGesture;

        /// <summary>
        /// ジェスチャ推定用オプション（基本的にハンドと同じ）
        /// </summary>
        public int gestureEstimationFrames = 4;

        /// <summary>
        /// 最小ジェスチャー推定完了通知間隔秒数
        /// </summary>  
        public float minimumGestureNotifyInterval = 0.5f;

        /// <summary>
        /// ジェスチャー推定完了閾値
        /// </summary>  
        public float gestureRecogThreshold = 0.75f;

        /// <summary>
        /// ジェスチャー推定のFPS
        /// </summary>  
        public int FramesPerSec = 30;

        /// <summary>
        /// trueの場合、アプリケーション開始時に自動的にジェスチャー推定処理を開始する
        /// </summary>  
        public bool autoStartGestureEstimation = false;

        /// <summary>
        /// ジェスチャー推定処理の指標
        /// </summary>
        public int GestureDetectionBodyIndex { get; set; } = 0;

        private const int framesPerGesture = 5;

        private bool isEstimating = false;
        private bool isInitialised = false;

        private TFLiteRuntime estimator;
        private float timeElapsed = 0f;
        private SynchronizationContext context;

        /// <summary>
        /// ジェスチャ一覧（ハンドと違いポーズもジェスチャ認識と同じ方法で推定される）
        /// </summary>
        private readonly List<BodyGesture> bodyGestures = new List<BodyGesture> {
        BodyGesture.StraightLeft, BodyGesture.HookLeft, BodyGesture.UpperLeft,
        BodyGesture.StraightRight, BodyGesture.HookRight, BodyGesture.UpperRight,
        BodyGesture.Furioroshi
    };

        void OnEnable()
        {
            context = SynchronizationContext.Current;
            TofArBodyManager.OnBodyPoseEstimated += OnHumanBodiesChanged;
            if (autoStartGestureEstimation)
            {
                StartGestureEstimation();
            }
        }

        void OnDisable()
        {
            TofArBodyManager.OnBodyPoseEstimated -= OnHumanBodiesChanged;
        }

        private void Update()
        {
            if (isEstimating)
            {
                timeElapsed += Time.deltaTime;
                if (gestureResultList.Count > 0)
                {
                    var gesture = gestureResultList.Last();

                    bool waitFinished = (timeElapsed > minimumGestureNotifyInterval) || (gesture != lastGesture);

                    if (waitFinished && gesture != BodyGesture.Others)
                    {
                        // More than X% of last results are equal
                        var count = gestureResultList.Where((g) => g == gesture).Count();
                        if (count >= (gestureRecogThreshold * gestureEstimationFrames))
                        {
                            // Update body gesture
                            lastGesture = gesture;

                            // Invoke delegate
                            OnGestureEstimated?.Invoke(gesture);
                        }
                    }
                }
            }
        }

        BodyPlatform platform = BodyPlatform.Others;

        private BodyPlatform ParseFrameDataSource(FrameDataSource dataSource)
        {

            switch (dataSource)
            {
#pragma warning disable CS0618 // SV1 Body recognition will remove in future version
                case FrameDataSource.TofArSV1BodySkeleton:
#pragma warning restore CS0618
                case FrameDataSource.TofArSV2BodySkeleton:
                    return BodyPlatform.SV2;
                case FrameDataSource.AREngineBodySkeleton:
                    return BodyPlatform.AREngine;
                case FrameDataSource.ARFoundationBodySkeleton:
                    return BodyPlatform.ARKit;
                default:
                    return BodyPlatform.Others;
            }
        }


        // Callback when human body detected
        void OnHumanBodiesChanged(BodyResults bodies)
        {
            //don't waste processing
            if (!isEstimating)
            {
                return;
            }

            if (ParseFrameDataSource(bodies.frameDataSource) != platform)
            {
                isInitialised = false;
                context.Post((s) => InitializeGestureRecognition(ParseFrameDataSource(bodies.frameDataSource)), null);
            }

            if (isInitialised && bodies.results.Length > GestureDetectionBodyIndex)
            {
                var pos3DForGesture = CreatePostionArrayForGestureFrom(bodies.results[GestureDetectionBodyIndex]);
                var pos3D = pos3DForGesture.Select((point) => point +
                    bodies.results[GestureDetectionBodyIndex].pose.position.GetVector3()).ToArray();

                var bodyData = new BodyGestureData(pos3D, platform);

                // Edit postion for each platform
                bodyData.EditPos3d();

                // Update body gesture
                UpdateGesture(bodyData);
            }
        }

        private void InitializeGestureRecognition(BodyPlatform newPlatform)
        {
            if (newPlatform == platform)
            {
                return;
            }
            string modelFileName;
            string pathToLocalFile;
            if (newPlatform == BodyPlatform.AREngine)
            {
                modelFileName = "body_gesture_arengine.bytes";
                pathToLocalFile = "/data/local/tmp/tofar/config/" + modelFileName;
            }
            else if (newPlatform == BodyPlatform.ARKit)
            {
                modelFileName = "body_gesture_arkit.bytes";
                pathToLocalFile = Application.persistentDataPath + "/" + modelFileName;
            }
            else
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "This device is not supported Body Gesture.");
                return;
            }
            platform = newPlatform;
            var pathToModelFile = Application.persistentDataPath + "/body_gesture.bytes";            // For found local model file
            if (System.IO.File.Exists(pathToLocalFile))
            {
                string creationDate = new System.IO.FileInfo(pathToLocalFile).CreationTime.ToShortDateString();
                string creationTime = new System.IO.FileInfo(pathToLocalFile).CreationTime.ToShortTimeString();
                TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("Body gesture model file loaded.\nCreated on {0} \nat {1}", creationDate, creationTime)); using (System.IO.Stream s = new System.IO.FileStream(pathToLocalFile, System.IO.FileMode.Open))
                {
                    System.IO.BinaryReader br = new System.IO.BinaryReader(s);
                    System.IO.File.WriteAllBytes(pathToModelFile, br.ReadBytes((int)s.Length));
                }
            }
            // For not found local model file
            else
            {
                var model = Resources.Load(System.IO.Path.GetFileNameWithoutExtension(modelFileName)) as TextAsset;
                if (model == null)
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, string.Format("Can't find {0} in Resources.", modelFileName));
                    return;
                }
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Body gesture model file loaded from Resources.");
                //save the model file to the local path
                System.IO.File.WriteAllBytes(pathToModelFile, model.bytes);
            }
            estimator = new TFLiteRuntime(pathToModelFile);
            isInitialised = true;
        }

        private void UpdateGesture(BodyGestureData bodyData)
        {
            var gesture = EstimateGesture(bodyData);

            // Update body pose
            CurrentPose = bodyGestures.Contains(gesture) ? BodyGesture.Others : gesture;

            if (gestureResultList.Count >= gestureEstimationFrames)
            {
                gestureResultList.RemoveAt(0);
            }
            gestureResultList.Add(bodyGestures.Contains(gesture) ? gesture : BodyGesture.Others);

        }

        private BodyGesture EstimateGesture(BodyGestureData bodyData)
        {
            int skip = (FramesPerSec / framesPerGesture) >= 4 ? 4 : 3;
            int maxAdd = skip * (framesPerGesture - 1) + 1;

            if (bodyDataList.Count > maxAdd)
            {
                bodyDataList.RemoveAt(0);
            }
            bodyDataList.Add(bodyData);

            // For not enough data for gesture
            if (bodyDataList.Count < maxAdd)
            {
                return BodyGesture.Others;
            }

            // Prepare index array for gesture
            int[] indexArray = new int[framesPerGesture];
            for (int i = 0; i < framesPerGesture; i++)
            {
                indexArray[i] = i * skip;
            }

            // Gesture detection uses joint data every 4 frames
            List<float> input = new List<float>();
            foreach (var index in indexArray)
            {
                bodyDataList[index].PoseNormalize(bodyDataList[indexArray[0]]);
                bodyDataList[index].GestureNormalize(bodyDataList[indexArray[0]]);
                bodyDataList[index].PoseNormalize();
                foreach (var joint in bodyDataList[index].NormalizedPos3D)
                {
                    input.Add(joint.x);
                    input.Add(joint.y);
                    input.Add(joint.z);
                }
                input.Add(bodyDataList[index].Cxyz.x);
                input.Add(bodyDataList[index].Cxyz.y);
                input.Add(bodyDataList[index].Cxyz.z);
                input.Add(bodyDataList[index].abc3D.x);
                input.Add(bodyDataList[index].abc3D.y);
                input.Add(bodyDataList[index].abc3D.z);
                input.Add(bodyDataList[index].P14.x);
                input.Add(bodyDataList[index].P14.y);
                input.Add(bodyDataList[index].P14.z);
            }
            input.CopyTo(estimator.getInputBuffer()[0]);
            var outputArray = estimator.forward()[0].ToList();

            // get the gesture
            return (BodyGesture)outputArray.IndexOf(outputArray.Max());
        }

        private Vector3[] CreatePostionArrayForGestureFrom(BodyResult humanBody)
        {
            var joints = humanBody.joints;
            var points = new Vector3[Enum.GetNames(typeof(BodyGestureJointIndex)).Length];

            points[(int)BodyGestureJointIndex.Pelvis] = FetchPostionFrom(joints, JointIndices.Spine1);
            points[(int)BodyGestureJointIndex.Neck] = FetchPostionFrom(joints, JointIndices.Neck1);
            points[(int)BodyGestureJointIndex.ShoulderLeft] = FetchPostionFrom(joints, JointIndices.LeftArm);
            points[(int)BodyGestureJointIndex.ElbowLeft] = FetchPostionFrom(joints, JointIndices.LeftForearm);
            points[(int)BodyGestureJointIndex.WristLeft] = FetchPostionFrom(joints, JointIndices.LeftHand);
            points[(int)BodyGestureJointIndex.ShoulderRight] = FetchPostionFrom(joints, JointIndices.RightArm);
            points[(int)BodyGestureJointIndex.ElbowRight] = FetchPostionFrom(joints, JointIndices.RightForearm);
            points[(int)BodyGestureJointIndex.WristRight] = FetchPostionFrom(joints, JointIndices.RightHand);
            points[(int)BodyGestureJointIndex.HipLeft] = FetchPostionFrom(joints, JointIndices.LeftUpLeg);
            points[(int)BodyGestureJointIndex.KneeLeft] = FetchPostionFrom(joints, JointIndices.LeftLeg);
            points[(int)BodyGestureJointIndex.AnkleLeft] = FetchPostionFrom(joints, JointIndices.LeftFoot);
            points[(int)BodyGestureJointIndex.HipRight] = FetchPostionFrom(joints, JointIndices.RightUpLeg);
            points[(int)BodyGestureJointIndex.KneeRight] = FetchPostionFrom(joints, JointIndices.RightLeg);
            points[(int)BodyGestureJointIndex.AnkleRight] = FetchPostionFrom(joints, JointIndices.RightFoot);
            points[(int)BodyGestureJointIndex.Head] = FetchPostionFrom(joints, JointIndices.Head);

            return points;
        }

        private Vector3 FetchPostionFrom(HumanBodyJoint[] joints, JointIndices indices)
        {
            if ((int)indices >= joints.Length)
            {
                return Vector3.zero;
            }
            return joints[(int)indices].anchorPose.position.GetVector3();
        }

        /// <summary>
        /// ジェスチャー推定を開始する
        /// </summary>  
        public void StartGestureEstimation()
        {
            isEstimating = true;
        }

        /// <summary>
        /// ジェスチャー推定を終了する
        /// </summary>  
        public void StopGestureEstimation()
        {
            isEstimating = false;
        }
    }
}

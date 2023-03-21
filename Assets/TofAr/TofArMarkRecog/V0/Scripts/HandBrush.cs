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
using TofAr.V0.Hand;
using UnityEngine;

namespace TofAr.V0.MarkRecog
{
    /// <summary>
    /// Hand認識コンポーネントと連携しマーク画像を作成する
    /// </summary>
    public class HandBrush : MonoBehaviour
    {
        /// <summary>
        /// 描画開始時デリゲート
        /// </summary>
        public delegate void DrawStartedEventHandler();

        /// <summary>
        /// 描画開始通知
        /// </summary>
        public DrawStartedEventHandler DrawStarted;

        /// <summary>
        /// 描画終了時デリゲート
        /// </summary>
        public delegate void DrawStoppedEventHandler();

        /// <summary>
        /// 描画終了通知
        /// </summary>
        public DrawStoppedEventHandler DrawStopped;

        /// <summary>
        /// 描画ポイント追加時デリゲート
        /// </summary>
        /// <param name="point"></param>
        public delegate void DrawPointAddedEventHandler(Vector3 point);

        /// <summary>
        /// 描画ポイント追加通知
        /// </summary>
        public DrawPointAddedEventHandler DrawPointAdded;

        private bool isDrawing = false;
        private bool isPaused = false;

        private PoseIndex poseLeft = PoseIndex.None;
        private PoseIndex poseRight = PoseIndex.None;
        private RecognizeResultProperty handData;

        [SerializeField]
        private GameObject rendererForRecognition;
        private IMarkRenderer markRendererForRecog;
        [SerializeField]
        private GameObject markRenderer;
        private GameObject markRendererInstance;

        /// <summary>
        /// 現在のマークレンダークラス
        /// </summary>
        public GameObject CurrentMarkRenderer { get => markRendererInstance; }

        /// <summary>
        /// 描画終了ポーズ1
        /// </summary>
        public PoseIndex stopPose1 = PoseIndex.OpenPalm;

        /// <summary>
        /// 描画終了ポーズ2
        /// </summary>
        public PoseIndex stopPose2 = PoseIndex.PinkyOut;

        /// <summary>
        /// 描画開始ポーズ1
        /// </summary>
        public PoseIndex startPose1 = PoseIndex.Shot;

        /// <summary>
        /// 描画開始ポーズ2
        /// </summary>
        public PoseIndex startPose2 = PoseIndex.Pistol;

        //some apps want the lines to follow the brush, others want them in space - this allows both to work
        [SerializeField]
        private Transform drawmarkParentTransform;

        /// <summary>
        /// <para>true: 端末の回転方向に応じて描画を回転させる</para>
        /// <para>false: 描画の自動回転を行わない</para>
        /// <para>デフォルト値：false</para>
        /// </summary>
        public bool autoRotation = false;

        /// <summary>
        /// 描画の更新閾値。フレーム間の指の移動距離がこの閾値を超えると描画を更新する。
        /// <para>デフォルト値：0.01</para>
        /// </summary>
        public float significantMotion = 0.01f;
        private Vector3 lastpoint;

        private float idleTime;
        private float confirmTime;

        private bool drawEnabled = true;

        private int imageRotation = 0;

        /// <summary>
        /// trueの場合マーク描画が有効である
        /// </summary>
        public bool DrawEnabled { get { return drawEnabled; } set { drawEnabled = value; } }

        // Use this for initialization
        void Start()
        {
            if (this.drawmarkParentTransform == null)
            {
                drawmarkParentTransform = this.transform;
            }
            this.markRendererForRecog = rendererForRecognition.GetComponent<IMarkRenderer>();
            this.markRendererInstance = GameObject.Instantiate(this.markRenderer, drawmarkParentTransform);
        }

        private void OnEnable()
        {
            TofArManager.OnScreenOrientationUpdated += OnScreenOrientationChanged;

            UpdateRotation();
            TofArHandManager.OnFrameArrived += HandFrameArrived;
        }

        private void OnDisable()
        {
            TofArManager.OnScreenOrientationUpdated -= OnScreenOrientationChanged;

            TofArHandManager.OnFrameArrived -= HandFrameArrived;
        }

        private void UpdateRotation()
        {
            if (autoRotation)
            {
                imageRotation = TofArManager.Instance.GetScreenOrientation();
            }
        }

        private void OnScreenOrientationChanged(ScreenOrientation previousDeviceOrientation, ScreenOrientation newDeviceOrientation)
        {
            UpdateRotation();
        }

        // Update is called once per frame
        void Update()
        {
            if (drawEnabled && handData != null)
            {
                var lrHand = handData.handStatus;
                var handPose = lrHand == HandStatus.LeftHand ? poseLeft : poseRight;

                // if hand is not being recognized for at least 10 frames, stop drawing
                if (lrHand == HandStatus.UnknownHand || lrHand == HandStatus.Tip || lrHand == HandStatus.NoHand ||
                    (lrHand == HandStatus.BothHands && poseLeft == PoseIndex.None && poseRight == PoseIndex.None))
                {
                    idleTime += Time.deltaTime;

                    if (idleTime >= 1f && isDrawing)
                    {
                        StopDrawing();
                    }
                    return;
                }
                else
                {
                    idleTime = 0;
                }

                CheckForStartOrStopPose(handPose);

                if (isDrawing && !this.isPaused)
                {
                    confirmTime = 0f;

                    UpdateDrawing(lrHand);
                }
            }
        }

        private void CheckForStartOrStopPose(PoseIndex handPose)
        {
            // If pinkyOut or openPalm, stop drawing
            // If shot or pistol, start
            if (handPose == stopPose1 || handPose == stopPose2)
            {
                // keep hand like this for a few frames before actually stopping
                if (confirmTime < 0.5f)
                {
                    if (!this.isPaused)
                    {
                        PauseResumeDrawing(true);
                    }
                    confirmTime += Time.deltaTime;
                }
                else
                {
                    StopDrawing();
                }
            }
            else if (handPose == startPose1 || handPose == startPose2)
            {
                if (this.isPaused)
                {
                    PauseResumeDrawing(false);
                }
                StartDrawing();
            }
        }

        // continue drawing in current frame
        void UpdateDrawing(HandStatus lrHand)
        {
            Vector3[] featurePoints = lrHand == HandStatus.LeftHand ? handData.featurePointsLeft : handData.featurePointsRight;

            if (featurePoints != null && featurePoints.Length > (int)HandPointIndex.IndexTip)
            {
                Vector3 newPoint = featurePoints[(int)HandPointIndex.IndexTip];
                DrawNewPoint(newPoint);
            }
        }

        /// <summary>
        /// レンダラーを設定する
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        public void SetRenderer(GameObject renderer)
        {
            if (this.markRendererInstance != null)
            {
                //destroy it if it is not the recognition renderer
                Destroy(this.markRendererInstance.gameObject);
            }
            this.markRendererInstance = GameObject.Instantiate(renderer, drawmarkParentTransform);
        }

        /// <summary>
        /// 描画を開始する
        /// </summary>
        public void StartDrawing()
        {
            //if we are starting, clear up the last one
            if (!this.isDrawing)
            {
                this.confirmTime = 0f;
                this.isPaused = false;

                this.markRendererForRecog.StartDrawing();
                if (this.markRendererInstance != null)
                {
                    this.markRendererInstance.GetComponent<IMarkRenderer>().StartDrawing();
                }

                this.isDrawing = true;

                if (this.DrawStarted != null)
                {
                    this.DrawStarted.Invoke();
                }
            }
        }

        /// <summary>
        /// 描画を終了する
        /// </summary>
        public void StopDrawing()
        {
            if (this.isDrawing)
            {
                this.isDrawing = false;

                this.markRendererForRecog.StopDrawing();
                if (this.markRendererInstance != null)
                {
                    this.markRendererInstance.GetComponent<IMarkRenderer>().StopDrawing();
                }

                if (this.DrawStopped != null)
                {
                    this.DrawStopped.Invoke();
                }
            }

        }

        private void PauseResumeDrawing(bool pause)
        {
            this.isPaused = pause;
        }


        private void DrawNewPoint(Vector3 newPoint)
        {
            //only add the point if the tip has moved significantly
            if ((newPoint - lastpoint).magnitude > significantMotion)
            {
                lastpoint = newPoint;
                newPoint = this.transform.TransformDirection(Quaternion.Euler(0, 0, imageRotation) * newPoint) + this.transform.position;
                this.markRendererForRecog.UpdateDrawing(newPoint);

                if (this.markRendererInstance != null)
                {
                    this.markRendererInstance.GetComponent<IMarkRenderer>().UpdateDrawing(newPoint);
                }
                if (this.DrawPointAdded != null)
                {
                    this.DrawPointAdded.Invoke(newPoint);
                }
            }
        }

        private void HandFrameArrived(object sender)
        {
            var manager = sender as TofArHandManager;
            if (manager == null)
            {
                return;
            }
            handData = manager.HandData.Data;
            manager.HandData.GetPoseIndex(out poseLeft, out poseRight);
        }
    }
}

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

namespace TofAr.V0.Hand
{
    /// <summary>
    /// RealHandModelプレファブの実装
    /// </summary>
    public class RealHandModel : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> handModels = null;
        [SerializeField]
        HandCollider handColliderLeft = null;
        [SerializeField]
        HandCollider handColliderRight = null;

        /// <summary>
        /// <para>true: 端末の回転方向に応じてモデルを回転させる</para>
        /// <para>false: モデルの自動回転を行わない</para>
        /// <para>デフォルト値：true</para>
        /// </summary>
        [SerializeField]
        public bool autoRotate = true;

        GameObject humanHandLeft = null, humanHandRight = null;

        private IHandModel leftHandModel = null;
        /// <summary>
        /// 左手のHandModel(handBoneRemapper3 オブジェクト)
        /// </summary>
        public IHandModel LeftHandModel
        {
            get
            {
                if (this.leftHandModel == null)
                {
                    this.leftHandModel = this.humanHandLeft?.GetComponent<IHandModel>();
                }
                return this.leftHandModel;
            }
        }

        private IHandModel rightHandModel = null;
        /// <summary>
        /// 右手のHandModel(handBoneRemapper3 オブジェクト)
        /// </summary>
        public IHandModel RightHandModel
        {
            get
            {
                if (this.rightHandModel == null)
                {
                    this.rightHandModel = this.humanHandRight?.GetComponent<IHandModel>();
                }
                return this.rightHandModel;
            }
        }

        /// <summary>
        /// 現在使用中の手モデルのhandModels配列内インデックス
        /// </summary>
        public int CurrentHandModelIndex { get; protected set; } = 0;
        /// <summary>
        /// 左手モデル
        /// </summary>
        public Transform handArmatureLeft = null;
        /// <summary>
        /// 右手モデル
        /// </summary>
        public Transform handArmatureRight = null;
        /// <summary>
        /// 左手の関節オブジェクト配列
        /// </summary>
        public Transform[] leftJoints = null;
        /// <summary>
        /// 右手の関節オブジェクト配列
        /// </summary>
        public Transform[] rightJoints = null;

        void Awake()
        {
            ChangeHandMaterial(0);
            setAutoRotates();
        }

        private bool lastAR = true;
        private void Update()
        {
            if (autoRotate != lastAR)
            {
                setAutoRotates();
                lastAR = autoRotate;
            }
        }

        void setAutoRotates()
        {
            if (humanHandLeft != null)
            {
                var handRemapComponent = humanHandLeft.GetComponent<IBoneRemapper>();
                if (handRemapComponent != null)
                {
                    handRemapComponent.AutoRotate = autoRotate;
                }
            }
            if (humanHandRight != null)
            {
                var handRemapComponent = humanHandRight.GetComponent<IBoneRemapper>();
                if (handRemapComponent != null)
                {
                    handRemapComponent.AutoRotate = autoRotate;
                }
            }
            if (handColliderLeft != null)
            {
                handColliderLeft.AutoRotate = autoRotate;
            }
            if (handColliderRight != null)
            {
                handColliderLeft.AutoRotate = autoRotate;
            }
        }

        /// <summary>
        /// 手モデル名称リストを取得する
        /// </summary>
        /// <returns>名称のリスト</returns>
        public List<string> GetObjectNames()
        {
            List<string> nameList = new List<string>();
            foreach (var model in handModels)
            {
                nameList.Add(model.name);
            }

            return nameList;
        }

        /// <summary>
        /// 手モデルの表示を切り替える
        /// </summary>
        /// <param name="enabled">trueの場合手モデルを表示してColliderを有効にする、falseの場合は非表示してColliderを無効にする</param>
        public void ShowRealHandToggleChanged(bool enabled)
        {
            if (humanHandLeft != null)
            {
                humanHandLeft.SetActive(enabled);
            }
            if (humanHandRight != null)
            {
                humanHandRight.SetActive(enabled);
            }

            if (handColliderLeft != null)
            {
                handColliderLeft.enabled = enabled;
            }
            if (handColliderRight != null)
            {
                handColliderRight.enabled = enabled;
            }
        }

        private void setLayersInChildren(Transform parent, int layer)
        {
            parent.gameObject.layer = layer;
            for (int i = 0; i < parent.childCount; i++)
            {
                setLayersInChildren(parent.GetChild(i), layer);
            }
        }

        /// <summary>
        /// 手モデルの変更を行う
        /// </summary>
        /// <param name="materialIndex">適用する手モデルのRealHandModel.handModels配列内インデックス</param>
        public void ChangeHandMaterial(int materialIndex)
        {

            if (materialIndex < handModels.Count)
            {
                bool isVisible = true;

                if (humanHandLeft != null)
                {
                    isVisible = humanHandLeft.activeInHierarchy;
                    Destroy(humanHandLeft);
                }
                if (humanHandRight != null)
                {
                    isVisible = humanHandRight.activeInHierarchy;
                    Destroy(humanHandRight);
                }
                //by parenting the transform, we set the location and rotation relative to be zero by default
                humanHandLeft = Instantiate(handModels[materialIndex], this.transform);
                humanHandRight = Instantiate(handModels[materialIndex], this.transform);
                setLayersInChildren(humanHandLeft.transform, this.gameObject.layer);
                setLayersInChildren(humanHandRight.transform, this.gameObject.layer);

                humanHandLeft.SetActive(isVisible);
                humanHandRight.SetActive(isVisible);

                //TofArManager.Logger.WriteLog(LogLevel.Debug, "Material idx: " + materialIndex);

                var handRemapComponent = humanHandLeft.GetComponent<IBoneRemapper>();
                if (handRemapComponent != null)
                {
                    leftJoints = handRemapComponent.ModelJoints;

                    handRemapComponent.LRHand = HandStatus.LeftHand;

                    handArmatureLeft = handRemapComponent.Armature;
                    this.leftHandModel = this.humanHandLeft?.GetComponent<IHandModel>();
                }


                handRemapComponent = humanHandRight.GetComponent<IBoneRemapper>();
                if (handRemapComponent != null)
                {
                    rightJoints = handRemapComponent.ModelJoints;


                    handRemapComponent.LRHand = HandStatus.RightHand;

                    handArmatureRight = handRemapComponent.Armature;
                    this.rightHandModel = this.humanHandRight?.GetComponent<IHandModel>();
                }

                setAutoRotates();

                this.CurrentHandModelIndex = materialIndex;
            }
        }
    }
}

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

using UnityEngine;


namespace TofAr.V0.Body
{
    /// <summary>
    /// Bodyジェスチャーのデータ
    /// </summary>
    public class BodyGestureData
    {
        /// <summary>
        /// Body認識エンジン種別
        /// </summary>  
        public BodyPlatform platform;

        /// <summary>
        /// Raw3D位置
        /// </summary>  
        public Vector3[] rawPos3D;

        /// <summary>
        /// 3D位置
        /// </summary>  
        public Vector3[] Pos3D;

        /// <summary>
        /// 正規化した3Dデータ
        /// </summary>
        public Vector3[] NormalizedPos3D;

        /// <summary>
        /// 3Dデータの中心座標（pelvis）
        /// </summary>
        public Vector3 center3D;

        /// <summary>
        /// 3Dデータの中心ベクトル
        /// </summary>
        public Vector3 old_pos3D;

        /// <summary>
        /// 3Dデータの中心ベクトル
        /// </summary>
        public Vector3 perp3D;

        /// <summary>
        /// 3Dデータの中心ベクトル
        /// </summary>
        public Quaternion q1;

        /// <summary>
        /// 3Dデータの中心ベクトル
        /// </summary>
        public Quaternion q2;

        /// <summary>
        /// 3Dデータの中心座標（pelvis）
        /// </summary>
        public Vector3 P14;

        /// <summary>
        /// 3Dデータの中心ベクトル
        /// </summary>
        public Vector3 Cxyz;

        /// <summary>
        /// 3Dデータの中心ベクトル
        /// </summary>
        public Vector3 abc3D;

        /// <summary>
        /// コンストラクタ
        /// </summary>  
        public BodyGestureData(Vector3[] pos3D, BodyPlatform platform)
        {
            this.Pos3D = pos3D;
            this.rawPos3D = pos3D;
            this.platform = platform;
        }

        /// <summary>
        /// ポーズを正規化する
        /// </summary>
        /// <param name="baseJoint"></param>
        public void PoseNormalize(BodyGestureData baseJoint = null)
        {
            var target = baseJoint;
            if (baseJoint == null)
            {
                target = this;
            }

            NormalizedPos3D = new Vector3[Pos3D.Length];

            // ターゲットのPelvisを原点にする
            center3D = Pos3D[(int)BodyGestureJointIndex.Pelvis];

            for (var i = 0; i < NormalizedPos3D.Length; i++)
            {
                NormalizedPos3D[i] = Pos3D[i] - target.center3D;
            }

            // Pelvis -> NeckがVector3.upを向くようにする
            old_pos3D = NormalizedPos3D[(int)BodyGestureJointIndex.Neck] - NormalizedPos3D[(int)BodyGestureJointIndex.Pelvis];
            q1 = Quaternion.FromToRotation(old_pos3D, Vector3.up);

            for (var i = 0; i < NormalizedPos3D.Length; i++)
            {
                NormalizedPos3D[i] = target.q1 * NormalizedPos3D[i];
            }

            // ShoulderとPelvisが存在する平面の法線がVector3.forwardを向くようにする
            var a = NormalizedPos3D[(int)BodyGestureJointIndex.ShoulderLeft] - NormalizedPos3D[(int)BodyGestureJointIndex.Pelvis];

            var b = NormalizedPos3D[(int)BodyGestureJointIndex.ShoulderRight] - NormalizedPos3D[(int)BodyGestureJointIndex.Pelvis];
            perp3D = Vector3.Cross(a, b);

            q2 = Quaternion.FromToRotation(perp3D, Vector3.forward);

            for (var i = 0; i < NormalizedPos3D.Length; i++)
            {
                NormalizedPos3D[i] = target.q2 * NormalizedPos3D[i];
            }
        }


        /// <summary>
        /// ジェスチャーを正規化する
        /// </summary>
        /// <param name="baseJoint">ジェスチャーのデータ</param>
        public void GestureNormalize(BodyGestureData baseJoint = null)
        {
            if (baseJoint == this)
            {
                P14 = baseJoint.center3D;
                Cxyz = baseJoint.old_pos3D.normalized;
                abc3D = baseJoint.perp3D.normalized;
            }
            else
            {
                P14 = center3D - baseJoint.center3D;
                Cxyz = baseJoint.q1 * old_pos3D.normalized;
                abc3D = baseJoint.q2 * perp3D.normalized;
            }
        }

        /// <summary>
        /// 3D位置を更新する
        /// </summary>          
        public void EditPos3d()
        {
            if (platform == BodyPlatform.ARKit)
            {
                ResetPos3d();

                var shoulder = rawPos3D[(int)BodyGestureJointIndex.ShoulderLeft];
                var neck = rawPos3D[(int)BodyGestureJointIndex.Neck];

                Pos3D[(int)BodyGestureJointIndex.ShoulderLeft] += (neck - shoulder) / 3;
                Pos3D[(int)BodyGestureJointIndex.ElbowLeft] += (neck - shoulder) / 4;
                Pos3D[(int)BodyGestureJointIndex.WristLeft] += (neck - shoulder) / 4;

                shoulder = rawPos3D[(int)BodyGestureJointIndex.ShoulderRight];
                Pos3D[(int)BodyGestureJointIndex.ShoulderRight] += (neck - shoulder) / 3;
                Pos3D[(int)BodyGestureJointIndex.ElbowRight] += (neck - shoulder) / 4;
                Pos3D[(int)BodyGestureJointIndex.WristRight] += (neck - shoulder) / 4;
            }
            else if (platform == BodyPlatform.AREngine)
            {
                ResetPos3d();

                var head = rawPos3D[(int)BodyGestureJointIndex.Head];
                var neck = rawPos3D[(int)BodyGestureJointIndex.Neck];

                Pos3D[(int)BodyGestureJointIndex.Head] = ((head - neck) / 2.2f) + neck;
            }
        }

        /// <summary>
        /// 3D位置をリゼットする
        /// </summary>  
        public void ResetPos3d()
        {
            rawPos3D.CopyTo(Pos3D, 0);
        }
    }
}

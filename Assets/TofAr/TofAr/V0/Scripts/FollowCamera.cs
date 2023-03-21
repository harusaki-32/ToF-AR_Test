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

namespace TofAr.V0
{
    /// <summary>
    /// targetCameraとして指定されたカメラの位置と回転を使用する
    /// </summary>
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera targetCamera = null;

        void Update()
        {
            this.transform.position = this.targetCamera.transform.position;
            this.transform.rotation = this.targetCamera.transform.rotation;
        }
    }
}

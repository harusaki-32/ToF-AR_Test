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

namespace TofAr.V0.Tof
{
    /// <summary>
    /// Tofのユーティリティクラス
    /// </summary>
    public static class TofUtils
    {
        /// <summary>
        /// Tof内部パラメータに基づいてカメラのProjection matrixを作る
        /// </summary>
        /// <param name="intrinsics">Tofの内部パラメータ</param>
        /// <param name="imageWidth">横解像度</param>
        /// <param name="imageHeight">縦解像度</param>
        /// <param name="nearClip">最小クリップ距離</param>
        /// <param name="farClip">最大クリップ距離</param>
        /// <returns>Projection matrix</returns>
        public static Matrix4x4 CreateProjectionMatrix(Tof.InternalParameter intrinsics, int imageWidth, int imageHeight, float nearClip, float farClip)
        {
            var projectionMatrix = new Matrix4x4();
            projectionMatrix[0, 0] = 2 * intrinsics.fx / imageWidth;
            projectionMatrix[0, 1] = 0;
            projectionMatrix[0, 2] = 1 - 2 * intrinsics.cx / imageWidth;
            projectionMatrix[0, 3] = 0;

            projectionMatrix[1, 0] = 0;
            projectionMatrix[1, 1] = 2 * intrinsics.fy / imageHeight;
            projectionMatrix[1, 2] = -1 + 2 * intrinsics.cy / imageHeight;
            projectionMatrix[1, 3] = 0;

            projectionMatrix[2, 0] = 0;
            projectionMatrix[2, 1] = 0;
            projectionMatrix[2, 2] = -(farClip + nearClip) / (farClip - nearClip);
            projectionMatrix[2, 3] = -2 * farClip * nearClip / (farClip - nearClip);

            projectionMatrix[3, 0] = 0;
            projectionMatrix[3, 1] = 0;
            projectionMatrix[3, 2] = -1;
            projectionMatrix[3, 3] = 0;

            return projectionMatrix;
        }
    }
}

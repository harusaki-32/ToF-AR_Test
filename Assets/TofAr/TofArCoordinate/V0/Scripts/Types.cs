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

namespace TofAr.V0.Coordinate
{
    /// <summary>
    /// Depth座標値
    /// </summary>
    public struct DepthPointProperty
    {
        /// <summary>
        /// Depth座標空間X値
        /// </summary>
        public int x;

        /// <summary>
        /// Depth座標空間Y値
        /// </summary>
        public int y;
    }

    /// <summary>
    /// Color座標値
    /// </summary>
    public struct ColorPointProperty
    {
        /// <summary>
        /// Color座標空間X値
        /// </summary>
        public int x;

        /// <summary>
        /// Color座標空間Y値
        /// </summary>
        public int y;
    }

    /// <summary>
    /// Camera座標値
    /// </summary>
    public struct CameraPointProperty
    {
        /// <summary>
        /// Camera座標空間X値
        /// </summary>
        public float x;

        /// <summary>
        /// Camera座標空間Y値
        /// </summary>
        public float y;

        /// <summary>
        /// Camera座標空間Z値
        /// </summary>
        public float z;
    }

    /// <summary>
    /// Depthデータに対応するColor座標マップを取得する
    /// </summary>
    public class DepthToColorProperty
    {
        /// <summary>
        /// Depthデータ配列
        /// </summary>
        public short[] depthFrame = { };

        /// <summary>
        /// 出力先の ColorPointProperty 型配列
        /// </summary>
        public ColorPointProperty[] colorPoints = { };
    }

    /// <summary>
    /// Depthデータに対応するCamera座標マップを取得する
    /// </summary>
    public class DepthToCameraProperty
    {
        /// <summary>
        /// Depthデータ配列
        /// </summary>
        public short[] depthFrame = { };

        /// <summary>
        /// 出力先の CameraPointProperty 型配列
        /// </summary>
        public CameraPointProperty[] cameraPoints = { };
    }

    /// <summary>
    /// 指定Depth座標をColor座標に変換する
    /// </summary>
    public class DepthPointToColorPointProperty
    {
        /// <summary>
        /// depthPointが示すDepth値
        /// </summary>
        public short depth;

        /// <summary>
        /// Depth座標値
        /// </summary>
        public DepthPointProperty depthPoint;

        /// <summary>
        /// Color座標値
        /// </summary>
        public ColorPointProperty colorPoint;
    }

    /// <summary>
    /// 指定Depth座標をCamera座標に変換する
    /// </summary>
    public class DepthPointToCameraPointProperty
    {
        /// <summary>
        /// depthPointが示すDepth値
        /// </summary>
        public short depth;

        /// <summary>
        /// Depth座標値
        /// </summary>
        public DepthPointProperty depthPoint;

        /// <summary>
        /// Camera座標
        /// </summary>
        public CameraPointProperty cameraPoint;

    }

    /// <summary>
    /// Colorデータに対応するDepth座標マップを取得する
    /// </summary>
    public class ColorToDepthProperty
    {
        /// <summary>
        /// Depthデータ配列
        /// </summary>
        public short[] depthFrame = { };

        /// <summary>
        /// 出力先の DepthPointProperty 型配列
        /// </summary>
        public DepthPointProperty[] depthPoints = { };

    }

    /// <summary>
    /// Colorデータに対応するCamera座標マップを取得する
    /// </summary>
    public class ColorToCameraProperty
    {
        /// <summary>
        /// Depthデータ配列
        /// </summary>
        public short[] depthFrame = { };

        /// <summary>
        /// 出力先の CameraPointProperty 型配列
        /// </summary>
        public CameraPointProperty[] cameraPoints = { };

    }

    /// <summary>
    /// 指定Camera座標をDepth座標に変換する
    /// </summary>
    public class CameraPointToDepthPointProperty
    {
        /// <summary>
        /// Camera座標
        /// </summary>
        public CameraPointProperty cameraPoint;

        /// <summary>
        /// Depth座標
        /// </summary>
        public DepthPointProperty depthPoint;
    }

    /// <summary>
    /// 指定Camera座標をColor座標に変換する
    /// </summary>
    public class CameraPointToColorPointProperty
    {
        /// <summary>
        /// Camera座標
        /// </summary>
        public CameraPointProperty cameraPoint;

        /// <summary>
        /// Color座標
        /// </summary>
        public ColorPointProperty colorPoint;

    }
}

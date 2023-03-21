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
using MessagePack;
using SensCord;
using UnityEngine;

namespace TofAr.V0.Plane
{
    /// <summary>
    /// *TODO+ B
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        Plane = 0,
    }

    /// <summary>
    /// 平面認識アルゴリズム設定情報
    /// </summary>
    [MessagePackObject]
    public class AlgorithmConfigProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArPlaneAlgorithmConfigKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// Depth映像の横解像度  
        /// </summary>
        [Key("width")]
        public int width;

        /// <summary>
        /// Depth映像の縦解像度  
        /// </summary>
        [Key("height")]
        public int height;

        /// <summary>
        /// 平面を求める位置x。Depth Map上の座標値で指定する。
        /// </summary>
        [Key("posx")]
        public int posx;

        /// <summary>
        /// 平面を求める位置y。Depth Map上の座標値で指定する。
        /// </summary>
        [Key("posy")]
        public int posy;

        /// <summary>
        /// 検出する最小の平面のサイズ。端点までの距離がこれより小さい平面は検出しない。
        /// </summary>
        [Key("min_size")]
        public float min_size;

        /// <summary>
        /// 平面推定に求める点の間隔 (デフォルト値:11)
        /// </summary>
        [Key("interval")]
        public int interval;

        /// <summary>
        /// 平滑化に使用するフィルタカーネルのサイズ (デフォルト値:5)
        /// </summary>
        [Key("ksize")]
        public int ksize;

        /// <summary>
        /// 求めた平面とPointCloudの距離の閾値 (デフォルト値:10.0)
        /// </summary>
        [Key("plane_threshold")]
        public float plane_threshold;
    }

    /// <summary>
    /// 平面認識アルゴリズム設定情報リスト
    /// <para>複数の認識設定を一括して指定/取得する</para>
    /// </summary>
    [MessagePackObject]
    public class AlgorithmConfigsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArPlaneAlgorithmConfigsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 設定リスト
        /// </summary>
        [Key("configurations")]
        public AlgorithmConfigProperty[] configurations = { };
    }

    /// <summary>
    /// 平面情報
    /// </summary>
    public class VariablesProperty
    {
        /// <summary>
        /// 平面の法線ベクトル
        /// </summary>
        public Vector3 normal;

        /// <summary>
        /// 平面検出基準点
        /// </summary>
        public Vector3 center;

        /// <summary>
        /// 基準点から最も近い平面端までの距離
        /// </summary>
        public float radius;
    }

    /// <summary>
    /// 平面認識処理実行間隔の設定
    /// </summary>
    [MessagePackObject]
    public class DetectIntervalProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArPlaneDetectIntervalKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 検出処理を実行する間隔フレーム数(デフォルト値:1)
        /// </summary>
        [Key("intervalFrames")]
        public int intervalFrames;
    }
}

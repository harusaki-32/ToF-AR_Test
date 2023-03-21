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
using System;

namespace TofAr.V0.Modeling
{
    /// <summary>
    /// *TODO+ B
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        Modeling = 0,
    }

    /// <summary>
    /// モデリングの出力データ
    /// </summary>
    public struct ModelingOutput
    {
        /// <summary>
        /// 頂点配列
        /// </summary>
        public float[] vertices;
        /// <summary>
        /// トライアングル配列
        /// </summary>
        public Int32[] triangles;
        /// <summary>
        /// ブロックインデックス
        /// </summary>
        public Int32[] blockIndex;
    };

    /// <summary>
    /// *TODO+ B 内部処理用
    /// カメラポーズ
    /// </summary>
    [MessagePackObject]
    public class InternalCameraPoseProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArModelingCameraPoseKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("pose")]
        public float[] pose = new float[12];
    }

    /// <summary>
    /// モデリング基本設定。モデリング実行中は変更不可能。
    /// </summary>
    [MessagePackObject]
    public class ModelingSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArModelingSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// ボクセルサイズ (デフォルト値:0.05)
        /// </summary>
        [Key("voxelSize")]
        public float voxelSize = 0.05f;

        /// <summary>
        /// ボクセル剪定の有無(デフォルト値:false)
        /// </summary>
        [Key("enableVoxelPruning")]
        public bool enableVoxelPruning = false;

        /// <summary>
        /// 最大ブロック数 (デフォルト値:1000)
        /// </summary>
        [Key("numMaxBlocks")]
        public uint numMaxBlocks = 1000;

        /// <summary>
        /// 各ブロック側のボクセル数 (デフォルト値:16)
        /// </summary>
        [Key("numVoxelsPerSide")]
        public uint numVoxelsPerSide = 16;

        /// <summary>
        /// 切り捨て距離の係数 (デフォルト値:2.5)
        /// </summary>
        [Key("muCoeff")]
        public float muCoeff = 2.5f;

        /// <summary>
        /// 切り捨てられた距離の下限 (デフォルト値:0.03)
        /// </summary>
        [Key("minTruncatedDistance")]
        public float minTruncatedDistance = 0.03f;

        /// <summary>
        /// 深度信頼マップの閾値 (デフォルト値:0.5)
        /// </summary>
        [Key("depthConfidenceThresh")]
        public float depthConfidenceThresh = 0.5f;

        /// <summary>
        /// 深度測定の最小距離 (デフォルト値:0.2)
        /// </summary>
        [Key("minDepthMeasurement")]
        public float minDepthMeasurement = 0.2f;

        /// <summary>
        /// 深度測定の最大距離 (デフォルト値:5.0)
        /// </summary>
        [Key("maxDepthMeasurement")]
        public float maxDepthMeasurement = 5.0f;

        /// <summary>
        /// 最大ウェイト値 (デフォルト値:30.0)
        /// </summary>
        [Key("weightMax")]
        public float weightMax = 30.0f;

        /// <summary>
        /// 視錐台カリングの有無 (デフォルト値:true)
        /// </summary>
        [Key("enableFrustumCulling")]
        public bool enableFrustumCulling = true;

        /// <summary>
        /// ボクセル剪定の閾値比率。値が大きいほど剪定が強くなる (デフォルト値:0.3)
        /// </summary>
        [Key("weightThresholdRatio")]
        public float weightThresholdRatio = 0.3f;

        /// <summary>
        /// ボクセル剪定用。フレームステップで測定されたボクセルブロックの最小経過時間 (デフォルト値:50)
        /// </summary>
        [Key("kMinAge")]
        public uint kMinAge = 50;

        /// <summary>
        /// ターゲットスペース定義の有無 (デフォルト値:false)
        /// </summary>
        [Key("defineTargetSpace")]
        public bool defineTargetSpace = false;

        /// <summary>
        /// ターゲットスペース境界ボックス最小値の[X,Y,Z]
        /// </summary>
        [Key("minTargetSpace")]
        public float[] minTargetSpace = new float[3];

        /// <summary>
        /// ターゲットスペース境界ボックス最大値の[X,Y,Z]
        /// </summary>
        [Key("maxTargetSpace")]
        public float[] maxTargetSpace = new float[3];

        /// <summary>
        /// trueの場合はDenseなDepthデータを処理する、falseの場合はSparseなDepthデータを処理する
        /// </summary>
        [Key("isVoxelProjection")]
        public bool isVoxelProjection = true;

        /// <summary>
        /// trueの場合はDenseなDepthデータから擬似的にSparseなDepthデータを生成し使用する
        /// </summary>
        [Key("enableFakeSparseDepth")]
        public bool enableFakeSparseDepth = false;

        /// <summary>
        /// 残りのDenseなDepthデータ数（enableFakeSparseDepthはtrueの時だけ）
        /// </summary>
        [Key("targetFakeSparseDepthPoints")]
        public int targetFakeSparseDepthPoints = 1000;
    }

    /// <summary>
    /// *TODO+ B 内部処理用
    /// カメラ設定
    /// </summary>
    [MessagePackObject]
    public class CameraSettingProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArModelingCameraSettingKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("width")]
        public int width = 0;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("height")]
        public int height = 0;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("stride")]
        public int stride = 0;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("intrinsics")]
        public float[] intrinsics = new float[6];
    }

    /// <summary>
    /// *TODO+ B 内部処理用
    /// カメラ設定のリスト
    /// </summary>
    [MessagePackObject]
    public class CameraSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArModelingCameraSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("settings")]
        public CameraSettingProperty[] settings = new CameraSettingProperty[0];

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("cameraPoseCallbackAddress")]
        public ulong cameraPoseCallbackAddress = 0;
    }

    /// <summary>
    /// モデリング実行時設定。モデリング実行中に変更可能。
    /// </summary>
    [MessagePackObject]
    public class RuntimeSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArModelingRuntimeSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// <para>true: Confidenceデータをモデリングに利用する</para>
        /// <para>false: Confidenceデータをモデリングに利用しない</para>
        /// <para>(デフォルト値:false)</para>
        /// </summary>
        [Key("isProcessConfidence")]
        public bool isProcessConfidence = false;

        /// <summary>
        /// Depthデータをモデリング処理に入力する間隔フレーム数(デフォルト値:3)
        /// </summary>
        [Key("updateInterval")]
        public uint updateInterval = 3;

        /// <summary>
        /// 3DMesh出力を行う間隔Update数 (デフォルト値:3)
        /// <para>updateInterval x estimateInterval フレーム間隔で3DMeshが出力されることになる。</para>
        /// </summary>
        [Key("estimateInterval")]
        public uint estimateInterval = 3;

        /// <summary>
        /// <para>true: 3DMesh出力時に変化があった部分のみ更新する</para>
        /// <para>false: 3DMesh出力時に全てのMeshを更新する</para>
        /// <para>(デフォルト値:false)</para>
        /// </summary>
        [Key("estimateUpdatedSurface")]
        public bool estimateUpdatedSurface = false;

        /// <summary>
        /// この設定値以遠のDepthピクセルをモデリング対象外とする(デフォルト値:3200)
        /// </summary>
        [Key("depthFar")]
        [Obsolete]
        public float depthFar = 3200;

        /// <summary>
        /// カメラポーズ値。TofArModelingManagerが自動設定する。
        /// </summary>
        [Key("depthScale")]
        public float depthScale = 0.001f;

        /// <summary>
        /// カメラポーズ値。TofArModelingManagerが自動設定する。
        /// </summary>
        [Key("transformation")]
        public float[] transformation = new float[12];

        /// <summary>
        /// trueの場合はModeling処理に使用したDepthデータをファイル書き出しする
        /// </summary>
        [Key("recordInputDepth")]
        public bool recordInputDepth = false;

        /// <summary>
        /// trueの場合はConfidence値がconfidenceCorrectionThresholdより小さいピクセルはDepth値をconfidenceCorrectionInvalidValueとする
        /// <para>デフォルト値:true</para>
        /// </summary>
        [Key("enableConfidenceCorrection")]
        public bool enableConfidenceCorrection = true;

        /// <summary>
        /// ConfidenceCorrection処理の閾値
        /// <para>デフォルト値:0</para>
        /// </summary>
        [Key("confidenceCorrectionThreshold")]
        public ushort confidenceCorrectionThreshold = 0;

        /// <summary>
        /// ConfidenceCorrection処理でDepthピクセルに設定する無効値
        /// <para>デフォルト値:32001</para>
        /// </summary>
        [Key("confidenceCorrectionInvalidValue")]
        public ushort confidenceCorrectionInvalidValue = 32001;
    }

    /// <summary>
    /// Mask設定 Property
    /// </summary>
    [MessagePackObject]
    public class MaskSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArModelingMaskSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// <para>true: 有効にする</para>
        /// <para>false: 無効にする</para>
        /// </summary>
        [Key("enabled")]
        public bool enabled = false;

        /// <summary>
        /// 適用するMask名リスト
        /// </summary>
        [Key("maskInfo")]
        public TofAr.V0.Segmentation.MaskInfo[] maskInfo;

        /// <summary>
        /// マスクをオフとする閾値
        /// </summary>
        [Key("maskThreshold")]
        public byte maskThreshold = 20;
    }
}

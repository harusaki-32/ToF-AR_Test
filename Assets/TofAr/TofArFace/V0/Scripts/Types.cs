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
using MessagePack;
using SensCord;

namespace TofAr.V0.Face
{
    /// <summary>
    /// 顔の表情を判断する時に使用する影響箇所
    /// <para>https://docs.unity3d.com/Packages/com.unity.xr.arkit-face-tracking@3.0/api/UnityEngine.XR.ARKit.ARKitBlendShapeLocation.html</para>
    /// </summary>
    public enum BlendshapeLocation
    {
        EyeBlinkRight,
        EyeWideRight,
        MouthLowerDownLeft,
        MouthRollUpper,
        CheekSquintLeft,
        MouthDimpleRight,
        BrowInnerUp,
        EyeLookInLeft,
        MouthPressLeft,
        MouthStretchRight,
        BrowDownLeft,
        MouthFunnel,
        NoseSneerLeft,
        EyeLookOutLeft,
        EyeLookInRight,
        MouthLowerDownRight,
        BrowOuterUpRight,
        MouthLeft,
        CheekSquintRight,
        JawOpen,
        EyeBlinkLeft,
        JawForward,
        MouthPressRight,
        NoseSneerRight,
        JawRight,
        MouthShrugLower,
        EyeSquintLeft,
        EyeLookOutRight,
        MouthFrownLeft,
        CheekPuff,
        MouthStretchLeft,
        TongueOut,
        MouthRollLower,
        MouthUpperUpRight,
        MouthShrugUpper,
        EyeSquintRight,
        EyeLookDownLeft,
        MouthSmileLeft,
        EyeWideLeft,
        MouthClose,
        JawLeft,
        MouthDimpleLeft,
        MouthFrownRight,
        MouthPucker,
        MouthRight,
        EyeLookUpLeft,
        BrowDownRight,
        MouthSmileRight,
        MouthUpperUpLeft,
        BrowOuterUpLeft,
        EyeLookUpRight,
        EyeLookDownRight
    }

    /// <summary>
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        Face = 0
    }

    /// <summary>
    /// トラッキング状態
    /// </summary>
    public enum TrackingState : byte
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 限定的
        /// </summary>
        Limited,

        /// <summary>
        /// トラックしている
        /// </summary>
        Tracking,
    };

    /// <summary>
    /// 平面や特徴点など、実際の環境で追跡可能なもののセッション固有の識別子。
    /// <para>ARFoundationのTrackableIdクラスと同じ定義</para>
    /// <para>https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@4.1/api/UnityEngine.XR.ARSubsystems.TrackableId.html</para>
    /// </summary>
    [MessagePackObject]
    public class TrackableId
    {
        /// <summary>
        /// IDの前半
        /// </summary>
        [Key("subId1")]
        public UInt64 subId1 = 0;

        /// <summary>
        /// IDの後半
        /// </summary>
        [Key("subId2")]
        public UInt64 subId2 = 0;
    }

    /// <summary>
    /// ARFoundationのARFaceクラスと同じ定義
    /// <para>https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.ARFace.html</para>
    /// </summary>
    [MessagePackObject]
    public class FaceResult
    {
        private const int faceVerticesCount = 468;
        private const int faceIndicesCount = 2694;
        private const int blendShapeCount = 52;

        /// <summary>
        /// TrackableのID
        /// </summary>  
        [Key("trackableId")]
        public TrackableId trackableId = new TrackableId();

        /// <summary>
        /// Faceのポーズ
        /// </summary> 
        [Key("pose")]
        public TofAr.V0.TofArTransform pose = new TofAr.V0.TofArTransform();

        /// <summary>
        /// 視点の位置
        /// </summary>
        [Key("fixationPoint")]
        public TofAr.V0.TofArTransform fixationPoint = new TofAr.V0.TofArTransform();

        /// <summary>
        /// 左目
        /// </summary>
        [Key("leftEye")]
        public TofAr.V0.TofArTransform leftEye = new TofAr.V0.TofArTransform();

        /// <summary>
        /// 右目
        /// </summary>
        [Key("rightEye")]
        public TofAr.V0.TofArTransform rightEye = new TofAr.V0.TofArTransform();

        /// <summary>
        /// トラッキング状態
        /// </summary>
        [Key("trackingState")]
        public TrackingState trackingState = TrackingState.None;

        /// <summary>
        /// ネイティブポインター
        /// </summary>
        [Key("nativePtr")]
        public UInt64 nativePtr = 0;

        /// <summary>
        /// 頂点
        /// </summary>
        [Key("vertices")]
        public TofAr.V0.TofArVector3[] vertices = new TofAr.V0.TofArVector3[faceVerticesCount];

        /// <summary>
        /// 法線
        /// </summary>
        [Key("normals")]
        public TofAr.V0.TofArVector3[] normals = new TofAr.V0.TofArVector3[faceVerticesCount];

        /// <summary>
        /// UV座標
        /// </summary>
        [Key("uvs")]
        public TofAr.V0.TofArVector2[] uvs = new TofAr.V0.TofArVector2[faceVerticesCount];

        /// <summary>
        /// 指標
        /// </summary>
        [Key("indices")]
        public int[] indices = new int[faceIndicesCount];


        /// <summary>
        /// blendshape
        /// </summary>
        [Key("blendShapes")]
        public float[] blendShapes = new float[blendShapeCount];

        /// <summary>
        /// タイムスタンプ
        /// </summary>
        [Key("timestamp")]
        public UInt64 timestamp = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FaceResult()
        {
            for (int i = 0; i < faceVerticesCount; i++)
            {
                vertices[i] = new TofAr.V0.TofArVector3();
                normals[i] = new TofAr.V0.TofArVector3();
                uvs[i] = new TofAr.V0.TofArVector2();
            }
        }
    }

    /// <summary>
    /// Face認識結果
    /// </summary>
    [MessagePackObject]
    public class FaceResults
    {
        /// <summary>
        /// ARFoundationのARFaceクラスと同じ定義のデータが格納された配列
        /// </summary>
        [Key("results")]
        public FaceResult[] results;
    };

    /// <summary>
    /// 内部処理用のProperty
    /// </summary>
    [MessagePackObject]
    public class ResultHandlerProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArFaceResultHandlerKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// ハンドラ
        /// </summary>
        [Key("handler")]
        public Int64 handler = 0;
    }

    /// <summary>
    /// 検出方法
    /// </summary>
    [System.Serializable]
    public enum FaceDetectorType : byte
    {
        /// <summary>
        /// 内側
        /// </summary>
        Internal_ARKit,
        /// <summary>
        /// 外側
        /// </summary>
        External
    }

    /// <summary>
    /// 検出方法の指定
    /// </summary>
    [MessagePackObject]
    public class DetectorTypeProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArFaceDetectorTypeKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 検出方法の指定
        /// </summary>
        [Key("detectorType")]
        public FaceDetectorType detectorType = FaceDetectorType.Internal_ARKit;
    }
}

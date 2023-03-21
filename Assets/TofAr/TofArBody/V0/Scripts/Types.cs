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

namespace TofAr.V0.Body
{
    /// <summary>
    /// *TODO+ B
    /// チャネルID
    /// </summary>
    internal enum ChannelIds
    {
        /// <summary>
        /// *TODO+ B
        /// Body
        /// </summary>
        Body = 0,
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
    /// 関節インデックス
    /// <para>ARFoundationの出力と同じ定義</para>
    /// <para>https://github.com/Unity-Technologies/arfoundation-samples/blob/7eb7cda240dd87aa6ee773769ebd7e9e4e2fdef0/Assets/Scripts/BoneController.cs#L11-L106</para>
    /// </summary>
    public enum JointIndices : sbyte
    {
        Invalid = -1,
        Root = 0,
        Hips = 1,
        LeftUpLeg = 2,
        LeftLeg = 3,
        LeftFoot = 4,
        LeftToes = 5,
        LeftToesEnd = 6,
        RightUpLeg = 7,
        RightLeg = 8,
        RightFoot = 9,
        RightToes = 10,
        RightToesEnd = 11,
        Spine1 = 12,
        Spine2 = 13,
        Spine3 = 14,
        Spine4 = 15,
        Spine5 = 16,
        Spine6 = 17,
        Spine7 = 18,
        LeftShoulder1 = 19,
        LeftArm = 20,
        LeftForearm = 21,

        LeftHand = 22,
        LeftHandIndexStart = 23,
        LeftHandIndex1 = 24,
        LeftHandIndex2 = 25,
        LeftHandIndex3 = 26,
        LeftHandIndexEnd = 27,
        LeftHandMidStart = 28,
        LeftHandMid1 = 29,
        LeftHandMid2 = 30,
        LeftHandMid3 = 31,
        LeftHandMidEnd = 32,
        LeftHandPinkyStart = 33,
        LeftHandPinky1 = 34,
        LeftHandPinky2 = 35,
        LeftHandPinky3 = 36,
        LeftHandPinkyEnd = 37,
        LeftHandRingStart = 38,
        LeftHandRing1 = 39,
        LeftHandRing2 = 40,
        LeftHandRing3 = 41,
        LeftHandRingEnd = 42,
        LeftHandThumbStart = 43,
        LeftHandThumb1 = 44,
        LeftHandThumb2 = 45,
        LeftHandThumbEnd = 46,

        Neck1 = 47,
        Neck2 = 48,
        Neck3 = 49,
        Neck4 = 50,
        Head = 51,

        Jaw = 52,
        Chin = 53,
        LeftEye = 54,
        LeftEyeLowerLid = 55,
        LeftEyeUpperLid = 56,
        LeftEyeball = 57,
        Nose = 58,
        RightEye = 59,
        RightEyeLowerLid = 60,
        RightEyeUpperLid = 61,
        RightEyeball = 62,

        RightShoulder1 = 63,
        RightArm = 64,
        RightForearm = 65,

        RightHand = 66,
        RightHandIndexStart = 67,
        RightHandIndex1 = 68,
        RightHandIndex2 = 69,
        RightHandIndex3 = 70,
        RightHandIndexEnd = 71,
        RightHandMidStart = 72,
        RightHandMid1 = 73,
        RightHandMid2 = 74,
        RightHandMid3 = 75,
        RightHandMidEnd = 76,
        RightHandPinkyStart = 77,
        RightHandPinky1 = 78,
        RightHandPinky2 = 79,
        RightHandPinky3 = 80,
        RightHandPinkyEnd = 81,
        RightHandRingStart = 82,
        RightHandRing1 = 83,
        RightHandRing2 = 84,
        RightHandRing3 = 85,
        RightHandRingEnd = 86,
        RightHandThumbStart = 87,
        RightHandThumb1 = 88,
        RightHandThumb2 = 89,
        RightHandThumbEnd = 90,
    };

    /// <summary>
    /// *TODO+ B
    /// Body認識モード(SV1BodyEstimatorで使用) 
    /// </summary>
    [System.Obsolete("Use TofAr.V0.Body.SV1.BodyShot instead")]
    internal enum BodyShot : byte
    {
        /// <summary>
        /// 全身
        /// </summary>
        FullBody,

        /// <summary>
        /// 上半身
        /// </summary>
        UpperBody,
    };

    /// <summary>
    /// データソース
    /// </summary>
    public enum FrameDataSource : byte
    {
        /// <summary>
        /// 不明
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// SV1
        /// </summary>
        [Obsolete("SV1 Body recognition will remove in future version")]
        TofArSV1BodySkeleton,

        /// <summary>
        /// SV2
        /// </summary>
        TofArSV2BodySkeleton,

        /// <summary>
        /// ARFoundation
        /// </summary>
        ARFoundationBodySkeleton,

        /// <summary>
        /// AREngine
        /// </summary>
        AREngineBodySkeleton,
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
    /// 3D空間での位置と回転の表現
    /// <para>UnityのPoseクラスと同じ定義</para>
    /// <para>https://docs.unity3d.com/ScriptReference/Pose.html</para>
    /// </summary>
    [MessagePackObject]
    public class Pose
    {
        /// <summary>
        /// 前方ベクトル
        /// </summary>
        [Key("forward")]
        public TofAr.V0.TofArVector3 forward = new TofArVector3();

        /// <summary>
        /// 位置
        /// </summary>
        [Key("position")]
        public TofAr.V0.TofArVector3 position = new TofArVector3();

        /// <summary>
        /// 右ベクトル
        /// </summary>
        [Key("right")]
        public TofAr.V0.TofArVector3 right = new TofArVector3();

        /// <summary>
        /// 回転
        /// </summary>
        [Key("rotation")]
        public TofAr.V0.TofArQuaternion rotation = new TofArQuaternion();

        /// <summary>
        /// 位置（UnityEngine.Vector3）
        /// </summary>
        [IgnoreMember]
        public UnityEngine.Vector3 Position { get => position.GetVector3(); set => position = value; }

        /// <summary>
        /// 回転（UnityEngine.Quaternion）
        /// </summary>
        [IgnoreMember]
        public UnityEngine.Quaternion Rotation { get => rotation.GetQuaternion(); set => rotation = value; }
    }

    /// <summary>
    /// 関節データのコンテナ
    /// <para>ARFoundationのXRHumanBodyJointクラスと同じ定義</para>
    /// <para>https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@4.1/api/UnityEngine.XR.ARSubsystems.XRHumanBodyJoint.html</para>
    /// </summary>
    [MessagePackObject]
    public class HumanBodyJoint
    {
        /// <summary>
        /// ジョイントのインデックス
        /// </summary>
        [Key("index")]
        public Int32 index = 0;

        /// <summary>
        /// 親ジョイントのインデックス
        /// </summary>
        [Key("parentIndex")]
        public Int32 parentIndex = 0;

        /// <summary>
        /// 親関節に対するスケール
        /// </summary>
        //scale relative to parent
        [Key("localScale")]
        public TofAr.V0.TofArVector3 localScale = new TofArVector3();

        /// <summary>
        /// 親関節に対するポーズ
        /// </summary>
        //pose relative to parent
        [Key("localPose")]
        public Pose localPose = new Pose();

        /// <summary>
        /// Bodyに関連するスケール
        /// </summary>
        //scale relative to anchor
        [Key("anchorScale")]
        public TofAr.V0.TofArVector3 anchorScale = new TofArVector3();

        /// <summary>
        /// Bodyを基準にしたポーズ
        /// </summary>
        //pose relative to anchor
        [Key("anchorPose")]
        public Pose anchorPose = new Pose();

        /// <summary>
        /// 関節が追跡されているかどうか
        /// </summary>
        [Key("tracked")]
        public bool tracked = false;
    }

    /// <summary>
    /// ARFoundationのARHumanBodyクラスと同じ定義
    /// <para>https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.ARHumanBody.html</para>
    /// </summary>
    [MessagePackObject]
    public class BodyResult
    {
        private const int jointIndicesCount = 91;

        /// <summary>
        /// TrackableのID
        /// </summary>  
        // XRHumanBody members
        [Key("trackableId")]
        public TrackableId trackableId = new TrackableId();

        /// <summary>
        /// Bodyのポーズ
        /// </summary>  
        //pose relative to the ARCamera, joints are relative to this
        [Key("pose")]
        public Pose pose = new Pose();

        /// <summary>
        /// 推定身長に対するスケール係数
        /// </summary>  
        [Key("estimatedHeightScaleFactor")]
        public float estimatedHeightScaleFactor = 1.0f;

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
        /// 関節配列
        /// </summary>  
        // ARHumanBody members
        [Key("joints")]
        public HumanBodyJoint[] joints = new HumanBodyJoint[jointIndicesCount];

        /// <summary>
        /// タイムスタンプ
        /// </summary>  
        [Key("timestamp")]
        public UInt64 timestamp = 0;

        /// <summary>
        /// Body認識モード
        /// </summary>  
        [Key("bodyShot")]
        [Obsolete("SV1 Body recognition will remove in future version")]
        public SV1.BodyShot bodyShot = SV1.BodyShot.FullBody;

        /// <summary>
        /// コンストラクタ
        /// </summary>  
        public BodyResult()
        {
            for (int i = 0; i < jointIndicesCount; i++)
            {
                joints[i] = new HumanBodyJoint();
            }
        }
    }

    /// <summary>
    /// Body認識結果
    /// </summary>
    [MessagePackObject]
    public class BodyResults
    {
        /// <summary>
        /// ARFoundationのARHumanBodyクラスと同じ定義のデータが格納された配列
        /// </summary>
        [Key("results")]
        public BodyResult[] results;

        /// <summary>
        /// データソース
        /// </summary>
        [Key("frameDataSource")]
        public FrameDataSource frameDataSource = FrameDataSource.Unknown;
    };

    /// <summary>
    /// 内部処理用のProperty
    /// </summary>
    [MessagePackObject]
    public class ResultHandlerProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArBodyResultHandlerKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// TODO+ C
        /// </summary>
        [Key("handler")]
        public Int64 handler = 0;
    }

    /// <summary>
    /// BodyPose認識タイプ
    /// </summary>
    [System.Serializable]
    public enum BodyPoseDetectorType : byte
    {
        /// <summary>
        /// 内部（SV2）
        /// </summary>
        Internal_SV2,
        /// <summary>
        /// 外部（ARFoundation,AREngine等）
        /// </summary>
        External
    }

    /// <summary>
    /// BodyPose認識タイププロパティ
    /// </summary>
    [MessagePackObject]
    public class DetectorTypeProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArBodyDetectorTypeKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// BodyPose認識タイプ
        /// <para>デフォルト値: Internal_SV2</para>
        /// </summary>
        [Key("detectorType")]
        public BodyPoseDetectorType detectorType = BodyPoseDetectorType.Internal_SV2;
    }
}

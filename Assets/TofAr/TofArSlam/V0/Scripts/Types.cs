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

namespace TofAr.V0.Slam
{
    /// <summary>
    /// TODO+ C Slamはリファレンスに載せる？
    /// </summary>
    internal enum ChannelIds
    {
        Slam = 0,
    }

    public enum CameraPoseSource : byte
    {
        /// <summary>
        /// Estimate by device gyro.
        /// </summary>
        Gyro = 0,

        /// <summary>
        /// Track the main camra in the scene.
        /// </summary>
        [Obsolete("Please use ARKitOrARFoundation instead")]
        MainCamera = 1,

        /// <summary>
        /// Track the main camera moved by ARKit or ARFoundation.
        /// </summary>
        ARKitOrARFoundation = 1,

        /// <summary>
        /// Estimate in the internal slam engine if that is available.
        /// </summary>
        InternalEngine = 2,

        /// <summary>
        /// Estimate using the internal slam engine 02 if that is available.
        /// </summary>
        InternalEngine02 = 3
    }

    public enum SlamStatus : byte
    {
        Idle = 0,
        Initialising,
        Tracking,
        Lost,
        LoadingMap,
        NotStarted = 254
    }


    [MessagePackObject]
    public class CameraPoseSourceProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamCameraPoseSourceKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("cameraPoseSource")]
        public CameraPoseSource cameraPoseSource { get; set; } = CameraPoseSource.Gyro;
    }

    [MessagePackObject]
    public class CameraPoseSourceChangeHandlerProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamCameraPoseSourceChangeHandlerKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("changingHandler")]
        public UInt64 changingHandler = 0;

        [Key("changedHandler")]
        public UInt64 changedHandler = 0;

        [Key("errorHandler")]
        public UInt64 errorHandler = 0;
    }

    [MessagePackObject]
    public class DepthResolutionProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamDepthResolutionKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("depthWidth")]
        public int depthWidth = 640;
        [Key("depthHeight")]
        public int depthHeight = 480;
    }

    [MessagePackObject]
    public class VocabularyFileProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamVocabularyFileKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("filePath")]
        public string filePath = "";
    }

    [MessagePackObject]
    public class VocabularyFile02Property : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamVocabularyFile02Key";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("filePath")]
        public string filePath = "";
    }

    [MessagePackObject]
    public class FeaturePointsProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamFeaturePointsKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("minFeaturePoints")]
        public int minFeaturePoints;
    }

    [MessagePackObject]
    public class SaveMapProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamSaveMapKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("filePath")]
        public string filePath = "";
    }
    [MessagePackObject]
    public class LoadMapProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamLoadMapKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("filePath")]
        public string filePath = "";
    }

    [MessagePackObject]
    public class SaveFrameProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamSaveFrameKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("savePath")]
        public string savePath = "";
    }

    [MessagePackObject]
    public class ProcessTimeProperty : IBaseProperty
    {
        public static readonly string ConstKey = "kTofArSlamProcessingTimeKey";

        [IgnoreMember]
        public string Key { get; } = ConstKey;

        [Key("slamProcessTime")]
        public float slamProcessTime;
        [Key("dataCollectionTime")]
        public float dataCollectionTime;
        [Key("dataProcessingTime")]
        public float dataProcessingTime;
        [Key("tofArWaitTime")]
        public float tofArWaitTime;
    }
}

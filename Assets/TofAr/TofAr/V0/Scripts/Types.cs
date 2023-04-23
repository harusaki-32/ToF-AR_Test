/*
 * Copyright 2018,2019,2020,2021,2022,2023 Sony Semiconductor Solutions Corporation.
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
using System.Collections.Generic;
using UnityEngine;

namespace TofAr.V0
{
    /// <summary>
    /// ログレベル
    /// </summary>
    [MessagePackObject]
    public class LogLevelProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArLogLevelKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// ログレベル
        /// </summary>
        [Key("logLevel")]
        public LogLevel logLevel;
    }

    /// <summary>
    /// デバイス属性
    /// </summary>
    [MessagePackObject]
    public class DeviceCapabilityProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArDeviceCapabilityKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// <para>true: デバイスはQualcommのCPUを搭載している</para>
        /// <para>false: デバイスはQualcommのCPUを搭載していない</para>
        /// </summary>
        [Key("isQualcommSupports")]
        public bool isQualcommSupports;

        /// <summary>
        /// TODO+ C json情報は一般Userには不要かな
        /// </summary>
        [Key("deviceAttributesString")]
        public string deviceAttributesString = ""; //needs to not be null or it breaks the C++

        /// <summary>
        /// 機種名
        /// </summary>
        [Key("modelName")]
        public string modelName = "";

        /// <summary>
        /// 機種グループ名
        /// </summary>
        [Key("modelGroupName")]
        public string modelGroupName = "";

        /// <summary>
        /// デバイス属性詳細情報文字列
        /// </summary>
        [IgnoreMember]
        public string TrimmedDeviceAttributesString
        {
            get { return deviceAttributesString.Trim(' ', '\n', '\r').TrimStart('[').TrimEnd(']'); }
            set { deviceAttributesString = "[\n" + value.Trim(' ', '\n', '\r').TrimStart('[').TrimEnd(']') + "\n]"; }
        }
    }

    /// <summary>
    /// シリアライズ可能なVector2
    /// </summary>
    [MessagePackObject]
    public class TofArVector2
    {
        /// <summary>
        /// X値
        /// </summary>
        [Key("x")]
        public float x = 0;

        /// <summary>
        /// Y値
        /// </summary>
        [Key("y")]
        public float y = 0;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TofArVector2() : this(0f, 0f)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">UnityEngineのVector2</param>
        public TofArVector2(Vector2 value) : this(value.x, value.y)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">Vector2のX値</param>
        /// <param name="y">Vector2のY値</param>
        public TofArVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// UnityEngine.Vector2を取得する
        /// </summary>
        /// <returns>UnityEngine.Vector2</returns>
        public Vector2 GetVector2()
        {
            return new Vector2(this.x, this.y);
        }

        /// <summary>
        /// 比較を行う
        /// </summary>
        /// <param name="other">比較するオブジェクト</param>
        /// <returns>trueの場合、等しい</returns>
        public override bool Equals(object other)
        {
            if (other != null && other.GetType() == typeof(TofArVector2))
            {
                TofArVector2 otherVec = (TofArVector2)other;
                return (((this.x == otherVec.x) && (this.y == otherVec.y)));
            }

            return false;
        }

        /// <summary>
        /// ハッシュコードを取得する
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //quick conversion

        /// <summary>
        /// TofArVector2をUnityEngine.Vector2へ変換する
        /// </summary>
        /// <param name="v">変換されたUnityEngine.Vector2</param>
        public static explicit operator Vector2(TofArVector2 v) => new Vector2(v.x, v.y);
        /// <summary>
        /// UnityEngine.Vector2をTofArVector2へ変換する
        /// </summary>
        /// <param name="v">変換されたTofArVector2</param>
        public static implicit operator TofArVector2(Vector2 v) => new TofArVector2(v);

    }

    /// <summary>
    /// シリアライズ可能なVector3
    /// </summary>
    [MessagePackObject]
    public class TofArVector3
    {
        /// <summary>
        /// X値
        /// </summary>
        [Key("x")]
        public float x = 0;

        /// <summary>
        /// Y値
        /// </summary>
        [Key("y")]
        public float y = 0;

        /// <summary>
        /// Z値
        /// </summary>
        [Key("z")]
        public float z = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TofArVector3() : this(0f, 0f, 0f)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">UnityEngineのVector3</param>
        public TofArVector3(Vector3 value) : this(value.x, value.y, value.z)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">Vector3のX値</param>
        /// <param name="y">Vector3のY値</param>
        /// <param name="z">Vector3のZ値</param>
        public TofArVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// UnityEngine.Vector3を取得する
        /// </summary>
        /// <returns>UnityEngine.Vector3</returns>
        public Vector3 GetVector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }

        /// <summary>
        /// 比較を行う
        /// </summary>
        /// <param name="other">比較するオブジェクト</param>
        /// <returns>trueの場合、等しい</returns>
        public override bool Equals(object other)
        {
            if (other != null && other.GetType() == typeof(TofArVector3))
            {
                TofArVector3 otherVec = (TofArVector3)other;
                return (((this.x == otherVec.x) && (this.y == otherVec.y) && (this.z == otherVec.z)));
            }

            return false;
        }

        /// <summary>
        /// ハッシュコードを取得する
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //quick conversion

        /// <summary>
        /// TofArVector3をUnityEngine.Vector3へ変換する
        /// </summary>
        /// <param name="v">変換されたUnityEngine.Vector3</param>
        public static explicit operator Vector3(TofArVector3 v) => new Vector3(v.x, v.y, v.z);
        /// <summary>
        /// UnityEngine.Vector3をTofArVector3へ変換する
        /// </summary>
        /// <param name="v">変換されたTofArVector3</param>
        public static implicit operator TofArVector3(Vector3 v) => new TofArVector3(v);

    }

    /// <summary>
    /// シリアライズ可能なQuaternion
    /// </summary>
    [MessagePackObject]
    public class TofArQuaternion
    {
        /// <summary>
        /// X値
        /// </summary>
        [Key("x")]
        public float x = 0;

        /// <summary>
        /// Y値
        /// </summary>
        [Key("y")]
        public float y = 0;

        /// <summary>
        /// Z値
        /// </summary>
        [Key("z")]
        public float z = 0;

        /// <summary>
        /// W値
        /// </summary>
        [Key("w")]
        public float w = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TofArQuaternion() : this(0f, 0f, 0f, 1f)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">X値</param>
        /// <param name="y">Y値</param>
        /// <param name="z">Z値</param>
        /// <param name="w">W値</param>
        public TofArQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// UnityEngine.Quaternionを取得する
        /// </summary>
        /// <returns>UnityEngine.Quaternion</returns>
        public Quaternion GetQuaternion()
        {
            return new Quaternion(this.x, this.y, this.z, this.w);
        }

        /// <summary>
        /// TofArQuaternionをUnityEngine.Quaternionへ変換する
        /// </summary>
        /// <param name="q">変換されたUnityEngine.Quaternion</param>
        public static explicit operator Quaternion(TofArQuaternion q) => q.GetQuaternion();
        /// <summary>
        /// UnityEngine.QuaternionをTofArQuaternionへ変換する
        /// </summary>
        /// <param name="q">変換されたTofArQuaternion</param>
        public static implicit operator TofArQuaternion(Quaternion q) => new TofArQuaternion(q.x, q.y, q.z, q.w);

        /// <summary>
        /// 比較する
        /// </summary>
        /// <param name="other">比較するオブジェクト</param>
        /// <returns>trueの場合, 等しい</returns>
        public override bool Equals(object other)
        {
            if (other != null && other.GetType() == typeof(TofArQuaternion))
            {
                TofArQuaternion otherQuat = (TofArQuaternion)other;
                return (((this.x == otherQuat.x) && (this.y == otherQuat.y) && (this.z == otherQuat.z) && (this.w == otherQuat.w)));
            }

            return false;
        }

        /// <summary>
        /// ハッシュコードを取得する
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// シリアライズ可能なTransform
    /// </summary>
    [MessagePackObject]
    public class TofArTransform
    {
        /// <summary>
        /// position値
        /// </summary>
        [Key("position")]
        public TofArVector3 position;

        /// <summary>
        /// rotation値
        /// </summary>
        [Key("rotation")]
        public TofArQuaternion rotation;

        /// <summary>
        /// forward値
        /// </summary>
        [Key("forward")]
        public TofArVector3 forward;

        /// <summary>
        /// right値
        /// </summary>
        [Key("right")]
        public TofArVector3 right;

        /// <summary>
        /// up値
        /// </summary>
        [Key("up")]
        public TofArVector3 up;

        /// <summary>
        /// scale値
        /// </summary>
        [Key("scale")]
        public TofArVector3 scale;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TofArTransform() : this(new TofArVector3(), new TofArQuaternion(), new TofArVector3(1f, 1f, 1f)) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TofArTransform(TofArVector3 position, TofArQuaternion rotation, TofArVector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            var quatTmp = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.z);
            this.forward = (quatTmp * Vector3.forward);
            this.right = (quatTmp * Vector3.right);
            this.up = (quatTmp * Vector3.up);
            this.scale = scale;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TofArTransform(Transform transform)
        {
            this.position = transform.position;
            this.rotation = transform.rotation;
            this.forward = transform.forward;
            this.right = transform.right;
            this.up = transform.up;
            this.scale = transform.localScale;
        }

        /// <summary>
        /// UnityEngine.TransformをTofArTransformへ変換する
        /// </summary>
        public static implicit operator TofArTransform(Transform t) => new TofArTransform(t);

        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="other">比較するオブジェクト</param>
        /// <returns>trueの場合, 等しい</returns>
        public override bool Equals(object other)
        {
            if (other != null && other.GetType() == typeof(TofArTransform))
            {
                TofArTransform otherTrans = (TofArTransform)other;
                return (this.rotation == otherTrans.rotation) && (this.position == otherTrans.position) && (this.scale == otherTrans.scale);
            }

            return false;
        }

        /// <summary>
        /// ハッシュコードを取得する
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// CameraPose
    /// </summary>
    [MessagePackObject]
    public class CameraPoseProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArCameraPoseKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// カメラの位置
        /// </summary>
        [Key("position")]
        public TofArVector3 position = new TofArVector3();
        /// <summary>
        /// 位置
        /// </summary>
        [IgnoreMember]
        public Vector3 Position
        {
            get
            {
                return this.position.GetVector3();
            }
            set
            {
                this.position.x = value.x;
                this.position.y = value.y;
                this.position.z = value.z;
            }
        }

        /// <summary>
        /// カメラの回転
        /// </summary>
        [Key("rotation")]
        public TofArQuaternion rotation = new TofArQuaternion();
        /// <summary>
        /// 回転
        /// </summary>
        [IgnoreMember]
        public Quaternion Rotation
        {
            get
            {
                return this.rotation.GetQuaternion();
            }
            set
            {
                this.rotation.x = value.x;
                this.rotation.y = value.y;
                this.rotation.z = value.z;
                this.rotation.w = value.w;
            }
        }

        /// <summary>
        /// 端末の重力加速度
        /// </summary>
        [Key("acceleration")]
        public TofArVector3 acceleration = new TofArVector3();
        /// <summary>
        /// 重力加速度
        /// </summary>
        [IgnoreMember]
        public Vector3 Acceleration
        {
            get
            {
                return this.acceleration.GetVector3();
            }
            set
            {
                this.acceleration.x = value.x;
                this.acceleration.y = value.y;
                this.acceleration.z = value.z;
            }
        }

        /// <summary>
        /// 比較する
        /// </summary>
        /// <param name="other">比較するオブジェクト</param>
        /// <returns>trueの場合, 等しい</returns>
        public override bool Equals(object other)
        {
            if (other != null && other.GetType() == typeof(CameraPoseProperty))
            {
                CameraPoseProperty otherCamPos = (CameraPoseProperty)other;
                return ((this.position.Equals(otherCamPos.position) && this.rotation.Equals(otherCamPos.rotation)));
            }

            return false;
        }

        /// <summary>
        /// ハッシュコードを取得する
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 実行モード
    /// </summary>
    public enum RunMode : byte
    {
        /// <summary>
        /// 同一ノードで実行されている。
        /// </summary>
        Default = 0,
        /// <summary>
        /// 複数ノード間をネットワーク接続した状態で実行されている。
        /// </summary>
        MultiNode,
    }

    /// <summary>
    /// 実行時設定の取得
    /// </summary>
    [MessagePackObject]
    public class RuntimeSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArRuntimeSettingsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 実行モード
        /// </summary>
        [Key("runMode")]
        public RunMode runMode = RunMode.Default;

        /// <summary>
        /// trueの場合、リモートサーバー上で動作している
        /// </summary>
        [Key("isRemoteServer")]
        public bool isRemoteServer = false;

        /// <summary>
        /// trueの場合、ARFoundationを使用している
        /// </summary>
        [Key("isUsingArFoundation")]
        public bool isUsingArFoundation = false;
    }

    /// <summary>
    /// CameraPose
    /// </summary>
    [MessagePackObject]
    public class DeviceOrientationsProperty : IBaseProperty
    {
        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// プロパティのキー
        /// </summary>
        public static readonly string ConstKey = "kTofArDeviceOrientationsKey";

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = ConstKey;

        /// <summary>
        /// 端末の向き
        /// </summary>
        [Key("deviceOrientation")]
        public int _deviceOrientation;

        /// <summary>
        /// デバイスの物理的な向き
        /// </summary>
        [IgnoreMember]
        public DeviceOrientation deviceOrientation
        {
            get { return (DeviceOrientation)_deviceOrientation; }
            set { _deviceOrientation = (int)value; }
        }

        /// <summary>
        /// 画面の向き
        /// </summary>
        [Key("screenOrientation")]
        public int _screenOrientation;
        /// <summary>
        /// スクリーンの向き
        /// </summary>
        [IgnoreMember]
        public ScreenOrientation screenOrientation
        {
            get { return (ScreenOrientation)_screenOrientation; }
            set { _screenOrientation = (int)value; }
        }
    }

    /// <summary>
    /// 指定パスのディレクトリリストの取得
    /// </summary>
    [MessagePackObject]
    public class DirectoryListProperty : IBaseProperty
    {
        /// <summary>
        /// リスト取得対象のパス
        /// </summary>
        [Key("path")]
        public string path { get; set; } = string.Empty;

        /// <summary>
        /// ディレクトリ名リスト
        /// </summary>
        [Key("directoryList")]
        public List<string> directoryList { get; set; } = new List<string>();

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = "kTofArDirectoryListKey";
    }

    /// <summary>
    /// 内部セッションのAPI
    /// </summary>
    public enum IosCameraApi : byte
    {
        ArKit,
        AvFoundation
    }

    /// <summary>
    /// AvFoundation用深度映像のフィルターモード
    /// </summary>
    public enum AvFoundationDepthFiltering : byte
    {
        Auto,
        Off,
        On
    }

    /// <summary>
    /// iOSプラットフォーム依存設定
    /// </summary>
    [MessagePackObject]
    public class PlatformConfigurationIos
    {
        /// <summary>
        /// 内部セッションのフレームレート
        /// </summary>
        [Key("sessionFramerate")]
        public int sessionFramerate = 60;

        /// <summary>
        /// 内部セッションのAPI
        /// </summary>
        [Key("cameraApi")]
        public IosCameraApi cameraApi = IosCameraApi.ArKit;

        /// <summary>
        /// AvFoundation用深度映像のフィルターモード
        /// </summary>
        [Key("depthFiltering")]
        public AvFoundationDepthFiltering depthFiltering = AvFoundationDepthFiltering.Auto;
    }

    /// <summary>
    /// プラットフォーム依存設定
    /// </summary>
    [MessagePackObject]
    public class PlatformConfigurationProperty : IBaseProperty
    {
        /// <summary>
        /// iOSプラットフォーム依存設定
        /// </summary>
        [Key("platformConfigurationIos")]
        public PlatformConfigurationIos platformConfigurationIos = new PlatformConfigurationIos();

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = "kTofArPlatformConfigurationKey";
    }

    /// <summary>
    /// 左右反転の設定
    /// </summary>
    [MessagePackObject]
    public class MirrorSettingsProperty : IBaseProperty
    {
        /// <summary>
        /// trueの場合、映像を左右反転する
        /// </summary>
        [Key("isMirroring")]
        public bool isMirroring = false;

        /// <summary>
        /// *TODO+ B（MessagePack用のコンパイルで使用されてるのでpublicのままにする）
        /// MessagePack用のキー
        /// </summary>
        [IgnoreMember]
        public string Key { get; } = "kTofArMirrorSettingsKey";
    }
}

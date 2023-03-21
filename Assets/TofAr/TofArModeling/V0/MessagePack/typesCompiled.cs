#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArModelingResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArModelingResolver();

        TofArModelingResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = TofArModelingResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArModelingResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArModelingResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(8)
            {
                {typeof(global::TofAr.V0.Modeling.CameraSettingProperty[]), 0 },
                {typeof(global::TofAr.V0.Segmentation.MaskInfo[]), 1 },
                {typeof(global::TofAr.V0.Modeling.InternalCameraPoseProperty), 2 },
                {typeof(global::TofAr.V0.Modeling.ModelingSettingsProperty), 3 },
                {typeof(global::TofAr.V0.Modeling.CameraSettingProperty), 4 },
                {typeof(global::TofAr.V0.Modeling.CameraSettingsProperty), 5 },
                {typeof(global::TofAr.V0.Modeling.RuntimeSettingsProperty), 6 },
                {typeof(global::TofAr.V0.Modeling.MaskSettingsProperty), 7 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Modeling.CameraSettingProperty>();
                case 1: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Segmentation.MaskInfo>();
                case 2: return new MessagePack.Formatters.TofAr.V0.Modeling.InternalCameraPosePropertyFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Modeling.ModelingSettingsPropertyFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Modeling.CameraSettingPropertyFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Modeling.CameraSettingsPropertyFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Modeling.RuntimeSettingsPropertyFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Modeling.MaskSettingsPropertyFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612



#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.TofAr.V0.Modeling
{
    using System;
    using MessagePack;


    public sealed class InternalCameraPosePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Modeling.InternalCameraPoseProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public InternalCameraPosePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "pose", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pose"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Modeling.InternalCameraPoseProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<float[]>().Serialize(ref bytes, offset, value.pose, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Modeling.InternalCameraPoseProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __pose__ = default(float[]);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __pose__ = formatterResolver.GetFormatterWithVerify<float[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Modeling.InternalCameraPoseProperty();
            ____result.pose = __pose__;
            return ____result;
        }
    }


    public sealed class ModelingSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Modeling.ModelingSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ModelingSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "voxelSize", 0},
                { "enableVoxelPruning", 1},
                { "numMaxBlocks", 2},
                { "numVoxelsPerSide", 3},
                { "muCoeff", 4},
                { "minTruncatedDistance", 5},
                { "depthConfidenceThresh", 6},
                { "minDepthMeasurement", 7},
                { "maxDepthMeasurement", 8},
                { "weightMax", 9},
                { "enableFrustumCulling", 10},
                { "weightThresholdRatio", 11},
                { "kMinAge", 12},
                { "defineTargetSpace", 13},
                { "minTargetSpace", 14},
                { "maxTargetSpace", 15},
                { "isVoxelProjection", 16},
                { "enableFakeSparseDepth", 17},
                { "targetFakeSparseDepthPoints", 18},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("voxelSize"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enableVoxelPruning"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("numMaxBlocks"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("numVoxelsPerSide"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("muCoeff"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minTruncatedDistance"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthConfidenceThresh"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minDepthMeasurement"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maxDepthMeasurement"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("weightMax"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enableFrustumCulling"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("weightThresholdRatio"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("kMinAge"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("defineTargetSpace"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minTargetSpace"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maxTargetSpace"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isVoxelProjection"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enableFakeSparseDepth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("targetFakeSparseDepthPoints"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Modeling.ModelingSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteMapHeader(ref bytes, offset, 19);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.voxelSize);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.enableVoxelPruning);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.numMaxBlocks);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.numVoxelsPerSide);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.muCoeff);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.minTruncatedDistance);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.depthConfidenceThresh);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.minDepthMeasurement);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.maxDepthMeasurement);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.weightMax);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.enableFrustumCulling);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.weightThresholdRatio);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.kMinAge);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[13]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.defineTargetSpace);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[14]);
            offset += formatterResolver.GetFormatterWithVerify<float[]>().Serialize(ref bytes, offset, value.minTargetSpace, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[15]);
            offset += formatterResolver.GetFormatterWithVerify<float[]>().Serialize(ref bytes, offset, value.maxTargetSpace, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[16]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isVoxelProjection);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[17]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.enableFakeSparseDepth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[18]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.targetFakeSparseDepthPoints);
            return offset - startOffset;
        }

        public global::TofAr.V0.Modeling.ModelingSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __voxelSize__ = default(float);
            var __enableVoxelPruning__ = default(bool);
            var __numMaxBlocks__ = default(uint);
            var __numVoxelsPerSide__ = default(uint);
            var __muCoeff__ = default(float);
            var __minTruncatedDistance__ = default(float);
            var __depthConfidenceThresh__ = default(float);
            var __minDepthMeasurement__ = default(float);
            var __maxDepthMeasurement__ = default(float);
            var __weightMax__ = default(float);
            var __enableFrustumCulling__ = default(bool);
            var __weightThresholdRatio__ = default(float);
            var __kMinAge__ = default(uint);
            var __defineTargetSpace__ = default(bool);
            var __minTargetSpace__ = default(float[]);
            var __maxTargetSpace__ = default(float[]);
            var __isVoxelProjection__ = default(bool);
            var __enableFakeSparseDepth__ = default(bool);
            var __targetFakeSparseDepthPoints__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __voxelSize__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __enableVoxelPruning__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __numMaxBlocks__ = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __numVoxelsPerSide__ = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __muCoeff__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __minTruncatedDistance__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 6:
                        __depthConfidenceThresh__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __minDepthMeasurement__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __maxDepthMeasurement__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 9:
                        __weightMax__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 10:
                        __enableFrustumCulling__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 11:
                        __weightThresholdRatio__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 12:
                        __kMinAge__ = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                        break;
                    case 13:
                        __defineTargetSpace__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 14:
                        __minTargetSpace__ = formatterResolver.GetFormatterWithVerify<float[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 15:
                        __maxTargetSpace__ = formatterResolver.GetFormatterWithVerify<float[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 16:
                        __isVoxelProjection__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 17:
                        __enableFakeSparseDepth__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 18:
                        __targetFakeSparseDepthPoints__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Modeling.ModelingSettingsProperty();
            ____result.voxelSize = __voxelSize__;
            ____result.enableVoxelPruning = __enableVoxelPruning__;
            ____result.numMaxBlocks = __numMaxBlocks__;
            ____result.numVoxelsPerSide = __numVoxelsPerSide__;
            ____result.muCoeff = __muCoeff__;
            ____result.minTruncatedDistance = __minTruncatedDistance__;
            ____result.depthConfidenceThresh = __depthConfidenceThresh__;
            ____result.minDepthMeasurement = __minDepthMeasurement__;
            ____result.maxDepthMeasurement = __maxDepthMeasurement__;
            ____result.weightMax = __weightMax__;
            ____result.enableFrustumCulling = __enableFrustumCulling__;
            ____result.weightThresholdRatio = __weightThresholdRatio__;
            ____result.kMinAge = __kMinAge__;
            ____result.defineTargetSpace = __defineTargetSpace__;
            ____result.minTargetSpace = __minTargetSpace__;
            ____result.maxTargetSpace = __maxTargetSpace__;
            ____result.isVoxelProjection = __isVoxelProjection__;
            ____result.enableFakeSparseDepth = __enableFakeSparseDepth__;
            ____result.targetFakeSparseDepthPoints = __targetFakeSparseDepthPoints__;
            return ____result;
        }
    }


    public sealed class CameraSettingPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Modeling.CameraSettingProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraSettingPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "width", 0},
                { "height", 1},
                { "stride", 2},
                { "intrinsics", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("stride"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("intrinsics"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Modeling.CameraSettingProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.stride);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<float[]>().Serialize(ref bytes, offset, value.intrinsics, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Modeling.CameraSettingProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __width__ = default(int);
            var __height__ = default(int);
            var __stride__ = default(int);
            var __intrinsics__ = default(float[]);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __stride__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __intrinsics__ = formatterResolver.GetFormatterWithVerify<float[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Modeling.CameraSettingProperty();
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.stride = __stride__;
            ____result.intrinsics = __intrinsics__;
            return ____result;
        }
    }


    public sealed class CameraSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Modeling.CameraSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "settings", 0},
                { "cameraPoseCallbackAddress", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("settings"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraPoseCallbackAddress"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Modeling.CameraSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Modeling.CameraSettingProperty[]>().Serialize(ref bytes, offset, value.settings, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.cameraPoseCallbackAddress);
            return offset - startOffset;
        }

        public global::TofAr.V0.Modeling.CameraSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __settings__ = default(global::TofAr.V0.Modeling.CameraSettingProperty[]);
            var __cameraPoseCallbackAddress__ = default(ulong);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __settings__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Modeling.CameraSettingProperty[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __cameraPoseCallbackAddress__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Modeling.CameraSettingsProperty();
            ____result.settings = __settings__;
            ____result.cameraPoseCallbackAddress = __cameraPoseCallbackAddress__;
            return ____result;
        }
    }


    public sealed class RuntimeSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Modeling.RuntimeSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RuntimeSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "isProcessConfidence", 0},
                { "updateInterval", 1},
                { "estimateInterval", 2},
                { "estimateUpdatedSurface", 3},
                { "depthFar", 4},
                { "depthScale", 5},
                { "transformation", 6},
                { "recordInputDepth", 7},
                { "enableConfidenceCorrection", 8},
                { "confidenceCorrectionThreshold", 9},
                { "confidenceCorrectionInvalidValue", 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isProcessConfidence"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("updateInterval"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("estimateInterval"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("estimateUpdatedSurface"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthFar"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthScale"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("transformation"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("recordInputDepth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enableConfidenceCorrection"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("confidenceCorrectionThreshold"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("confidenceCorrectionInvalidValue"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Modeling.RuntimeSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 11);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isProcessConfidence);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.updateInterval);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.estimateInterval);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.estimateUpdatedSurface);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.depthFar);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.depthScale);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += formatterResolver.GetFormatterWithVerify<float[]>().Serialize(ref bytes, offset, value.transformation, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.recordInputDepth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.enableConfidenceCorrection);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += MessagePackBinary.WriteUInt16(ref bytes, offset, value.confidenceCorrectionThreshold);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteUInt16(ref bytes, offset, value.confidenceCorrectionInvalidValue);
            return offset - startOffset;
        }

        public global::TofAr.V0.Modeling.RuntimeSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isProcessConfidence__ = default(bool);
            var __updateInterval__ = default(uint);
            var __estimateInterval__ = default(uint);
            var __estimateUpdatedSurface__ = default(bool);
            var __depthFar__ = default(float);
            var __depthScale__ = default(float);
            var __transformation__ = default(float[]);
            var __recordInputDepth__ = default(bool);
            var __enableConfidenceCorrection__ = default(bool);
            var __confidenceCorrectionThreshold__ = default(ushort);
            var __confidenceCorrectionInvalidValue__ = default(ushort);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __isProcessConfidence__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __updateInterval__ = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __estimateInterval__ = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __estimateUpdatedSurface__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 4:
                        __depthFar__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __depthScale__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 6:
                        __transformation__ = formatterResolver.GetFormatterWithVerify<float[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 7:
                        __recordInputDepth__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 8:
                        __enableConfidenceCorrection__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 9:
                        __confidenceCorrectionThreshold__ = MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
                        break;
                    case 10:
                        __confidenceCorrectionInvalidValue__ = MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Modeling.RuntimeSettingsProperty();
            ____result.isProcessConfidence = __isProcessConfidence__;
            ____result.updateInterval = __updateInterval__;
            ____result.estimateInterval = __estimateInterval__;
            ____result.estimateUpdatedSurface = __estimateUpdatedSurface__;
            ____result.depthFar = __depthFar__;
            ____result.depthScale = __depthScale__;
            ____result.transformation = __transformation__;
            ____result.recordInputDepth = __recordInputDepth__;
            ____result.enableConfidenceCorrection = __enableConfidenceCorrection__;
            ____result.confidenceCorrectionThreshold = __confidenceCorrectionThreshold__;
            ____result.confidenceCorrectionInvalidValue = __confidenceCorrectionInvalidValue__;
            return ____result;
        }
    }


    public sealed class MaskSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Modeling.MaskSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MaskSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "enabled", 0},
                { "maskInfo", 1},
                { "maskThreshold", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enabled"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskInfo"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskThreshold"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Modeling.MaskSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.enabled);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Segmentation.MaskInfo[]>().Serialize(ref bytes, offset, value.maskInfo, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.maskThreshold);
            return offset - startOffset;
        }

        public global::TofAr.V0.Modeling.MaskSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __enabled__ = default(bool);
            var __maskInfo__ = default(global::TofAr.V0.Segmentation.MaskInfo[]);
            var __maskThreshold__ = default(byte);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __enabled__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __maskInfo__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Segmentation.MaskInfo[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __maskThreshold__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Modeling.MaskSettingsProperty();
            ____result.enabled = __enabled__;
            ____result.maskInfo = __maskInfo__;
            ____result.maskThreshold = __maskThreshold__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

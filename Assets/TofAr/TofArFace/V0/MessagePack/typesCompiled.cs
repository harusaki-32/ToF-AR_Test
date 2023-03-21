#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArFaceResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArFaceResolver();

        TofArFaceResolver()
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
                var f = TofArFaceResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArFaceResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArFaceResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(10)
            {
                {typeof(global::TofAr.V0.TofArVector3[]), 0 },
                {typeof(global::TofAr.V0.TofArVector2[]), 1 },
                {typeof(global::TofAr.V0.Face.FaceResult[]), 2 },
                {typeof(global::TofAr.V0.Face.TrackingState), 3 },
                {typeof(global::TofAr.V0.Face.FaceDetectorType), 4 },
                {typeof(global::TofAr.V0.Face.TrackableId), 5 },
                {typeof(global::TofAr.V0.Face.FaceResult), 6 },
                {typeof(global::TofAr.V0.Face.FaceResults), 7 },
                {typeof(global::TofAr.V0.Face.ResultHandlerProperty), 8 },
                {typeof(global::TofAr.V0.Face.DetectorTypeProperty), 9 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.TofArVector3>();
                case 1: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.TofArVector2>();
                case 2: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Face.FaceResult>();
                case 3: return new MessagePack.Formatters.TofAr.V0.Face.TrackingStateFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Face.FaceDetectorTypeFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Face.TrackableIdFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Face.FaceResultFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Face.FaceResultsFormatter();
                case 8: return new MessagePack.Formatters.TofAr.V0.Face.ResultHandlerPropertyFormatter();
                case 9: return new MessagePack.Formatters.TofAr.V0.Face.DetectorTypePropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Face
{
    using System;
    using MessagePack;

    public sealed class TrackingStateFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Face.TrackingState>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Face.TrackingState value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }

        public global::TofAr.V0.Face.TrackingState Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Face.TrackingState)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class FaceDetectorTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Face.FaceDetectorType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Face.FaceDetectorType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }

        public global::TofAr.V0.Face.FaceDetectorType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Face.FaceDetectorType)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Face
{
    using System;
    using MessagePack;


    public sealed class TrackableIdFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Face.TrackableId>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TrackableIdFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "subId1", 0},
                { "subId2", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("subId1"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("subId2"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Face.TrackableId value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.subId1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.subId2);
            return offset - startOffset;
        }

        public global::TofAr.V0.Face.TrackableId Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __subId1__ = default(ulong);
            var __subId2__ = default(ulong);

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
                        __subId1__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 1:
                        __subId2__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Face.TrackableId();
            ____result.subId1 = __subId1__;
            ____result.subId2 = __subId2__;
            return ____result;
        }
    }


    public sealed class FaceResultFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Face.FaceResult>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FaceResultFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "trackableId", 0},
                { "pose", 1},
                { "fixationPoint", 2},
                { "leftEye", 3},
                { "rightEye", 4},
                { "trackingState", 5},
                { "nativePtr", 6},
                { "vertices", 7},
                { "normals", 8},
                { "uvs", 9},
                { "indices", 10},
                { "blendShapes", 11},
                { "timestamp", 12},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("trackableId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pose"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fixationPoint"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("leftEye"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rightEye"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("trackingState"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("nativePtr"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("vertices"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("normals"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uvs"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("indices"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("blendShapes"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("timestamp"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Face.FaceResult value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 13);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.TrackableId>().Serialize(ref bytes, offset, value.trackableId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Serialize(ref bytes, offset, value.pose, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Serialize(ref bytes, offset, value.fixationPoint, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Serialize(ref bytes, offset, value.leftEye, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Serialize(ref bytes, offset, value.rightEye, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.TrackingState>().Serialize(ref bytes, offset, value.trackingState, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.nativePtr);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3[]>().Serialize(ref bytes, offset, value.vertices, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3[]>().Serialize(ref bytes, offset, value.normals, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector2[]>().Serialize(ref bytes, offset, value.uvs, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += formatterResolver.GetFormatterWithVerify<int[]>().Serialize(ref bytes, offset, value.indices, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += formatterResolver.GetFormatterWithVerify<float[]>().Serialize(ref bytes, offset, value.blendShapes, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.timestamp);
            return offset - startOffset;
        }

        public global::TofAr.V0.Face.FaceResult Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __trackableId__ = default(global::TofAr.V0.Face.TrackableId);
            var __pose__ = default(global::TofAr.V0.TofArTransform);
            var __fixationPoint__ = default(global::TofAr.V0.TofArTransform);
            var __leftEye__ = default(global::TofAr.V0.TofArTransform);
            var __rightEye__ = default(global::TofAr.V0.TofArTransform);
            var __trackingState__ = default(global::TofAr.V0.Face.TrackingState);
            var __nativePtr__ = default(ulong);
            var __vertices__ = default(global::TofAr.V0.TofArVector3[]);
            var __normals__ = default(global::TofAr.V0.TofArVector3[]);
            var __uvs__ = default(global::TofAr.V0.TofArVector2[]);
            var __indices__ = default(int[]);
            var __blendShapes__ = default(float[]);
            var __timestamp__ = default(ulong);

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
                        __trackableId__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.TrackableId>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __pose__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __fixationPoint__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __leftEye__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __rightEye__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArTransform>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __trackingState__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.TrackingState>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __nativePtr__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 7:
                        __vertices__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 8:
                        __normals__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 9:
                        __uvs__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector2[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __indices__ = formatterResolver.GetFormatterWithVerify<int[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 11:
                        __blendShapes__ = formatterResolver.GetFormatterWithVerify<float[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 12:
                        __timestamp__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Face.FaceResult();
            ____result.trackableId = __trackableId__;
            ____result.pose = __pose__;
            ____result.fixationPoint = __fixationPoint__;
            ____result.leftEye = __leftEye__;
            ____result.rightEye = __rightEye__;
            ____result.trackingState = __trackingState__;
            ____result.nativePtr = __nativePtr__;
            ____result.vertices = __vertices__;
            ____result.normals = __normals__;
            ____result.uvs = __uvs__;
            ____result.indices = __indices__;
            ____result.blendShapes = __blendShapes__;
            ____result.timestamp = __timestamp__;
            return ____result;
        }
    }


    public sealed class FaceResultsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Face.FaceResults>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FaceResultsFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "results", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("results"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Face.FaceResults value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.FaceResult[]>().Serialize(ref bytes, offset, value.results, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Face.FaceResults Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __results__ = default(global::TofAr.V0.Face.FaceResult[]);

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
                        __results__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.FaceResult[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Face.FaceResults();
            ____result.results = __results__;
            return ____result;
        }
    }


    public sealed class ResultHandlerPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Face.ResultHandlerProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ResultHandlerPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "handler", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("handler"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Face.ResultHandlerProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.handler);
            return offset - startOffset;
        }

        public global::TofAr.V0.Face.ResultHandlerProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __handler__ = default(long);

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
                        __handler__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Face.ResultHandlerProperty();
            ____result.handler = __handler__;
            return ____result;
        }
    }


    public sealed class DetectorTypePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Face.DetectorTypeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DetectorTypePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "detectorType", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("detectorType"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Face.DetectorTypeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.FaceDetectorType>().Serialize(ref bytes, offset, value.detectorType, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Face.DetectorTypeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __detectorType__ = default(global::TofAr.V0.Face.FaceDetectorType);

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
                        __detectorType__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Face.FaceDetectorType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Face.DetectorTypeProperty();
            ____result.detectorType = __detectorType__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

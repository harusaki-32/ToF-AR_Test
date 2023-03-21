#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArSegmentationResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArSegmentationResolver();

        TofArSegmentationResolver()
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
                var f = TofArSegmentationResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArSegmentationResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArSegmentationResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(8)
            {
                {typeof(global::TofAr.V0.Segmentation.SegmentationResult[]), 0 },
                {typeof(global::TofAr.V0.Segmentation.DataStructureType), 1 },
                {typeof(global::TofAr.V0.Segmentation.SegmentationResult), 2 },
                {typeof(global::TofAr.V0.Segmentation.SegmentationResults), 3 },
                {typeof(global::TofAr.V0.Segmentation.ResultHandlerProperty), 4 },
                {typeof(global::TofAr.V0.Segmentation.SkyDetectorSettingsProperty), 5 },
                {typeof(global::TofAr.V0.Segmentation.HumanDetectorSettingsProperty), 6 },
                {typeof(global::TofAr.V0.Segmentation.MaskInfo), 7 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Segmentation.SegmentationResult>();
                case 1: return new MessagePack.Formatters.TofAr.V0.Segmentation.DataStructureTypeFormatter();
                case 2: return new MessagePack.Formatters.TofAr.V0.Segmentation.SegmentationResultFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Segmentation.SegmentationResultsFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Segmentation.ResultHandlerPropertyFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Segmentation.SkyDetectorSettingsPropertyFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Segmentation.HumanDetectorSettingsPropertyFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Segmentation.MaskInfoFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Segmentation
{
    using System;
    using MessagePack;

    public sealed class DataStructureTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Segmentation.DataStructureType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Segmentation.DataStructureType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }

        public global::TofAr.V0.Segmentation.DataStructureType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Segmentation.DataStructureType)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Segmentation
{
    using System;
    using MessagePack;


    public sealed class SegmentationResultFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Segmentation.SegmentationResult>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SegmentationResultFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "name", 0},
                { "dataStructureType", 1},
                { "timestamp", 2},
                { "maskBufferSize", 3},
                { "maskBufferWidth", 4},
                { "maskBufferHeight", 5},
                { "maskBufferByte", 6},
                { "maskBufferUInt16", 7},
                { "maskBufferUInt32", 8},
                { "maskBufferUInt64", 9},
                { "maskBufferFloat", 10},
                { "rawPointer", 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("dataStructureType"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("timestamp"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferSize"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferHeight"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferByte"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferUInt16"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferUInt32"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferUInt64"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maskBufferFloat"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rawPointer"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Segmentation.SegmentationResult value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 12);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Segmentation.DataStructureType>().Serialize(ref bytes, offset, value.dataStructureType, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.timestamp);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.maskBufferSize);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.maskBufferWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.maskBufferHeight);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.maskBufferByte, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += formatterResolver.GetFormatterWithVerify<ushort[]>().Serialize(ref bytes, offset, value.maskBufferUInt16, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += formatterResolver.GetFormatterWithVerify<uint[]>().Serialize(ref bytes, offset, value.maskBufferUInt32, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<ulong[]>().Serialize(ref bytes, offset, value.maskBufferUInt64, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += formatterResolver.GetFormatterWithVerify<float[]>().Serialize(ref bytes, offset, value.maskBufferFloat, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.rawPointer);
            return offset - startOffset;
        }

        public global::TofAr.V0.Segmentation.SegmentationResult Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __name__ = default(string);
            var __dataStructureType__ = default(global::TofAr.V0.Segmentation.DataStructureType);
            var __timestamp__ = default(ulong);
            var __maskBufferSize__ = default(long);
            var __maskBufferWidth__ = default(int);
            var __maskBufferHeight__ = default(int);
            var __maskBufferByte__ = default(byte[]);
            var __maskBufferUInt16__ = default(ushort[]);
            var __maskBufferUInt32__ = default(uint[]);
            var __maskBufferUInt64__ = default(ulong[]);
            var __maskBufferFloat__ = default(float[]);
            var __rawPointer__ = default(ulong);

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
                        __name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __dataStructureType__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Segmentation.DataStructureType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __timestamp__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 3:
                        __maskBufferSize__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 4:
                        __maskBufferWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __maskBufferHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 6:
                        __maskBufferByte__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 7:
                        __maskBufferUInt16__ = formatterResolver.GetFormatterWithVerify<ushort[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 8:
                        __maskBufferUInt32__ = formatterResolver.GetFormatterWithVerify<uint[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 9:
                        __maskBufferUInt64__ = formatterResolver.GetFormatterWithVerify<ulong[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __maskBufferFloat__ = formatterResolver.GetFormatterWithVerify<float[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 11:
                        __rawPointer__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Segmentation.SegmentationResult();
            ____result.name = __name__;
            ____result.dataStructureType = __dataStructureType__;
            ____result.timestamp = __timestamp__;
            ____result.maskBufferSize = __maskBufferSize__;
            ____result.maskBufferWidth = __maskBufferWidth__;
            ____result.maskBufferHeight = __maskBufferHeight__;
            ____result.maskBufferByte = __maskBufferByte__;
            ____result.maskBufferUInt16 = __maskBufferUInt16__;
            ____result.maskBufferUInt32 = __maskBufferUInt32__;
            ____result.maskBufferUInt64 = __maskBufferUInt64__;
            ____result.maskBufferFloat = __maskBufferFloat__;
            ____result.rawPointer = __rawPointer__;
            return ____result;
        }
    }


    public sealed class SegmentationResultsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Segmentation.SegmentationResults>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SegmentationResultsFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Segmentation.SegmentationResults value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Segmentation.SegmentationResult[]>().Serialize(ref bytes, offset, value.results, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Segmentation.SegmentationResults Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __results__ = default(global::TofAr.V0.Segmentation.SegmentationResult[]);

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
                        __results__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Segmentation.SegmentationResult[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Segmentation.SegmentationResults();
            ____result.results = __results__;
            return ____result;
        }
    }


    public sealed class ResultHandlerPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Segmentation.ResultHandlerProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Segmentation.ResultHandlerProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Segmentation.ResultHandlerProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Segmentation.ResultHandlerProperty();
            ____result.handler = __handler__;
            return ____result;
        }
    }


    public sealed class SkyDetectorSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Segmentation.SkyDetectorSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SkyDetectorSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "enabled", 0},
                { "copyToInputHandler", 1},
                { "executeHandler", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enabled"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("copyToInputHandler"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("executeHandler"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Segmentation.SkyDetectorSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.copyToInputHandler);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.executeHandler);
            return offset - startOffset;
        }

        public global::TofAr.V0.Segmentation.SkyDetectorSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __copyToInputHandler__ = default(ulong);
            var __executeHandler__ = default(ulong);

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
                        __copyToInputHandler__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 2:
                        __executeHandler__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Segmentation.SkyDetectorSettingsProperty();
            ____result.enabled = __enabled__;
            ____result.copyToInputHandler = __copyToInputHandler__;
            ____result.executeHandler = __executeHandler__;
            return ____result;
        }
    }


    public sealed class HumanDetectorSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Segmentation.HumanDetectorSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public HumanDetectorSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "enabled", 0},
                { "copyToInputHandler", 1},
                { "executeHandler", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enabled"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("copyToInputHandler"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("executeHandler"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Segmentation.HumanDetectorSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.copyToInputHandler);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.executeHandler);
            return offset - startOffset;
        }

        public global::TofAr.V0.Segmentation.HumanDetectorSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __copyToInputHandler__ = default(ulong);
            var __executeHandler__ = default(ulong);

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
                        __copyToInputHandler__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 2:
                        __executeHandler__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Segmentation.HumanDetectorSettingsProperty();
            ____result.enabled = __enabled__;
            ____result.copyToInputHandler = __copyToInputHandler__;
            ____result.executeHandler = __executeHandler__;
            return ____result;
        }
    }


    public sealed class MaskInfoFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Segmentation.MaskInfo>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MaskInfoFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "name", 0},
                { "isColorSource", 1},
                { "isInvertMask", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isColorSource"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isInvertMask"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Segmentation.MaskInfo value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isColorSource);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isInvertMask);
            return offset - startOffset;
        }

        public global::TofAr.V0.Segmentation.MaskInfo Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __name__ = default(string);
            var __isColorSource__ = default(bool);
            var __isInvertMask__ = default(bool);

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
                        __name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __isColorSource__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __isInvertMask__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Segmentation.MaskInfo();
            ____result.name = __name__;
            ____result.isColorSource = __isColorSource__;
            ____result.isInvertMask = __isInvertMask__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

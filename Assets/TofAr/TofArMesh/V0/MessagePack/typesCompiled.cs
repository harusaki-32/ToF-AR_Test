#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArMeshResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArMeshResolver();

        TofArMeshResolver()
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
                var f = TofArMeshResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArMeshResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArMeshResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(3)
            {
                {typeof(global::TofAr.V0.Segmentation.MaskInfo[]), 0 },
                {typeof(global::TofAr.V0.Mesh.AlgorithmConfigProperty), 1 },
                {typeof(global::TofAr.V0.Mesh.MaskSettingsProperty), 2 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Segmentation.MaskInfo>();
                case 1: return new MessagePack.Formatters.TofAr.V0.Mesh.AlgorithmConfigPropertyFormatter();
                case 2: return new MessagePack.Formatters.TofAr.V0.Mesh.MaskSettingsPropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Mesh
{
    using System;
    using MessagePack;


    public sealed class AlgorithmConfigPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Mesh.AlgorithmConfigProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AlgorithmConfigPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "meshReductionLevel", 0},
                { "resetTrianglePeriod", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("meshReductionLevel"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("resetTrianglePeriod"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Mesh.AlgorithmConfigProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.meshReductionLevel);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.resetTrianglePeriod);
            return offset - startOffset;
        }

        public global::TofAr.V0.Mesh.AlgorithmConfigProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __meshReductionLevel__ = default(int);
            var __resetTrianglePeriod__ = default(int);

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
                        __meshReductionLevel__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __resetTrianglePeriod__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Mesh.AlgorithmConfigProperty();
            ____result.meshReductionLevel = __meshReductionLevel__;
            ____result.resetTrianglePeriod = __resetTrianglePeriod__;
            return ____result;
        }
    }


    public sealed class MaskSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Mesh.MaskSettingsProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Mesh.MaskSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Mesh.MaskSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Mesh.MaskSettingsProperty();
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

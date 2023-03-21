#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArPlanesResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArPlanesResolver();

        TofArPlanesResolver()
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
                var f = TofArPlanesResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArPlanesResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArPlanesResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(4)
            {
                {typeof(global::TofAr.V0.Plane.AlgorithmConfigProperty[]), 0 },
                {typeof(global::TofAr.V0.Plane.AlgorithmConfigProperty), 1 },
                {typeof(global::TofAr.V0.Plane.AlgorithmConfigsProperty), 2 },
                {typeof(global::TofAr.V0.Plane.DetectIntervalProperty), 3 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Plane.AlgorithmConfigProperty>();
                case 1: return new MessagePack.Formatters.TofAr.V0.Plane.AlgorithmConfigPropertyFormatter();
                case 2: return new MessagePack.Formatters.TofAr.V0.Plane.AlgorithmConfigsPropertyFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Plane.DetectIntervalPropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Plane
{
    using System;
    using MessagePack;


    public sealed class AlgorithmConfigPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Plane.AlgorithmConfigProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AlgorithmConfigPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "width", 0},
                { "height", 1},
                { "posx", 2},
                { "posy", 3},
                { "min_size", 4},
                { "interval", 5},
                { "ksize", 6},
                { "plane_threshold", 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("posx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("posy"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("min_size"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("interval"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ksize"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("plane_threshold"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Plane.AlgorithmConfigProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 8);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.posx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.posy);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.min_size);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.interval);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ksize);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.plane_threshold);
            return offset - startOffset;
        }

        public global::TofAr.V0.Plane.AlgorithmConfigProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __posx__ = default(int);
            var __posy__ = default(int);
            var __min_size__ = default(float);
            var __interval__ = default(int);
            var __ksize__ = default(int);
            var __plane_threshold__ = default(float);

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
                        __posx__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __posy__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __min_size__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __interval__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 6:
                        __ksize__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 7:
                        __plane_threshold__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Plane.AlgorithmConfigProperty();
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.posx = __posx__;
            ____result.posy = __posy__;
            ____result.min_size = __min_size__;
            ____result.interval = __interval__;
            ____result.ksize = __ksize__;
            ____result.plane_threshold = __plane_threshold__;
            return ____result;
        }
    }


    public sealed class AlgorithmConfigsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Plane.AlgorithmConfigsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AlgorithmConfigsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "configurations", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("configurations"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Plane.AlgorithmConfigsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Plane.AlgorithmConfigProperty[]>().Serialize(ref bytes, offset, value.configurations, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Plane.AlgorithmConfigsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __configurations__ = default(global::TofAr.V0.Plane.AlgorithmConfigProperty[]);

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
                        __configurations__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Plane.AlgorithmConfigProperty[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Plane.AlgorithmConfigsProperty();
            ____result.configurations = __configurations__;
            return ____result;
        }
    }


    public sealed class DetectIntervalPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Plane.DetectIntervalProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DetectIntervalPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "intervalFrames", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("intervalFrames"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Plane.DetectIntervalProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.intervalFrames);
            return offset - startOffset;
        }

        public global::TofAr.V0.Plane.DetectIntervalProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __intervalFrames__ = default(int);

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
                        __intervalFrames__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Plane.DetectIntervalProperty();
            ____result.intervalFrames = __intervalFrames__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

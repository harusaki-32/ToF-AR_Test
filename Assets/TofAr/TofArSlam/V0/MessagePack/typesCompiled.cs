#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArSlamResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArSlamResolver();

        TofArSlamResolver()
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
                var f = TofArSlamResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArSlamResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArSlamResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(11)
            {
                {typeof(global::TofAr.V0.Slam.CameraPoseSource), 0 },
                {typeof(global::TofAr.V0.Slam.CameraPoseSourceProperty), 1 },
                {typeof(global::TofAr.V0.Slam.CameraPoseSourceChangeHandlerProperty), 2 },
                {typeof(global::TofAr.V0.Slam.DepthResolutionProperty), 3 },
                {typeof(global::TofAr.V0.Slam.VocabularyFileProperty), 4 },
                {typeof(global::TofAr.V0.Slam.VocabularyFile02Property), 5 },
                {typeof(global::TofAr.V0.Slam.FeaturePointsProperty), 6 },
                {typeof(global::TofAr.V0.Slam.SaveMapProperty), 7 },
                {typeof(global::TofAr.V0.Slam.LoadMapProperty), 8 },
                {typeof(global::TofAr.V0.Slam.SaveFrameProperty), 9 },
                {typeof(global::TofAr.V0.Slam.ProcessTimeProperty), 10 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.TofAr.V0.Slam.CameraPoseSourceFormatter();
                case 1: return new MessagePack.Formatters.TofAr.V0.Slam.CameraPoseSourcePropertyFormatter();
                case 2: return new MessagePack.Formatters.TofAr.V0.Slam.CameraPoseSourceChangeHandlerPropertyFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Slam.DepthResolutionPropertyFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Slam.VocabularyFilePropertyFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Slam.VocabularyFile02PropertyFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Slam.FeaturePointsPropertyFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Slam.SaveMapPropertyFormatter();
                case 8: return new MessagePack.Formatters.TofAr.V0.Slam.LoadMapPropertyFormatter();
                case 9: return new MessagePack.Formatters.TofAr.V0.Slam.SaveFramePropertyFormatter();
                case 10: return new MessagePack.Formatters.TofAr.V0.Slam.ProcessTimePropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Slam
{
    using System;
    using MessagePack;

    public sealed class CameraPoseSourceFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.CameraPoseSource>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.CameraPoseSource value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Slam.CameraPoseSource Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Slam.CameraPoseSource)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Slam
{
    using System;
    using MessagePack;


    public sealed class CameraPoseSourcePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.CameraPoseSourceProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraPoseSourcePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "cameraPoseSource", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraPoseSource"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.CameraPoseSourceProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Slam.CameraPoseSource>().Serialize(ref bytes, offset, value.cameraPoseSource, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.CameraPoseSourceProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __cameraPoseSource__ = default(global::TofAr.V0.Slam.CameraPoseSource);

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
                        __cameraPoseSource__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Slam.CameraPoseSource>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.CameraPoseSourceProperty();
            ____result.cameraPoseSource = __cameraPoseSource__;
            return ____result;
        }
    }


    public sealed class CameraPoseSourceChangeHandlerPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.CameraPoseSourceChangeHandlerProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraPoseSourceChangeHandlerPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "changingHandler", 0},
                { "changedHandler", 1},
                { "errorHandler", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("changingHandler"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("changedHandler"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("errorHandler"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.CameraPoseSourceChangeHandlerProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.changingHandler);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.changedHandler);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.errorHandler);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.CameraPoseSourceChangeHandlerProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __changingHandler__ = default(ulong);
            var __changedHandler__ = default(ulong);
            var __errorHandler__ = default(ulong);

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
                        __changingHandler__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 1:
                        __changedHandler__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 2:
                        __errorHandler__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.CameraPoseSourceChangeHandlerProperty();
            ____result.changingHandler = __changingHandler__;
            ____result.changedHandler = __changedHandler__;
            ____result.errorHandler = __errorHandler__;
            return ____result;
        }
    }


    public sealed class DepthResolutionPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.DepthResolutionProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DepthResolutionPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "depthWidth", 0},
                { "depthHeight", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthHeight"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.DepthResolutionProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.depthWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.depthHeight);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.DepthResolutionProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __depthWidth__ = default(int);
            var __depthHeight__ = default(int);

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
                        __depthWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __depthHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.DepthResolutionProperty();
            ____result.depthWidth = __depthWidth__;
            ____result.depthHeight = __depthHeight__;
            return ____result;
        }
    }


    public sealed class VocabularyFilePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.VocabularyFileProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public VocabularyFilePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "filePath", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("filePath"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.VocabularyFileProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.filePath, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.VocabularyFileProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __filePath__ = default(string);

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
                        __filePath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.VocabularyFileProperty();
            ____result.filePath = __filePath__;
            return ____result;
        }
    }


    public sealed class VocabularyFile02PropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.VocabularyFile02Property>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public VocabularyFile02PropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "filePath", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("filePath"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.VocabularyFile02Property value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.filePath, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.VocabularyFile02Property Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __filePath__ = default(string);

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
                        __filePath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.VocabularyFile02Property();
            ____result.filePath = __filePath__;
            return ____result;
        }
    }


    public sealed class FeaturePointsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.FeaturePointsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FeaturePointsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "minFeaturePoints", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minFeaturePoints"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.FeaturePointsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.minFeaturePoints);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.FeaturePointsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __minFeaturePoints__ = default(int);

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
                        __minFeaturePoints__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.FeaturePointsProperty();
            ____result.minFeaturePoints = __minFeaturePoints__;
            return ____result;
        }
    }


    public sealed class SaveMapPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.SaveMapProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SaveMapPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "filePath", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("filePath"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.SaveMapProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.filePath, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.SaveMapProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __filePath__ = default(string);

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
                        __filePath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.SaveMapProperty();
            ____result.filePath = __filePath__;
            return ____result;
        }
    }


    public sealed class LoadMapPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.LoadMapProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LoadMapPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "filePath", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("filePath"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.LoadMapProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.filePath, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.LoadMapProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __filePath__ = default(string);

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
                        __filePath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.LoadMapProperty();
            ____result.filePath = __filePath__;
            return ____result;
        }
    }


    public sealed class SaveFramePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.SaveFrameProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SaveFramePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "savePath", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("savePath"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.SaveFrameProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.savePath, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.SaveFrameProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __savePath__ = default(string);

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
                        __savePath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.SaveFrameProperty();
            ____result.savePath = __savePath__;
            return ____result;
        }
    }


    public sealed class ProcessTimePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Slam.ProcessTimeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ProcessTimePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "slamProcessTime", 0},
                { "dataCollectionTime", 1},
                { "dataProcessingTime", 2},
                { "tofArWaitTime", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("slamProcessTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("dataCollectionTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("dataProcessingTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("tofArWaitTime"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Slam.ProcessTimeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.slamProcessTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.dataCollectionTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.dataProcessingTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.tofArWaitTime);
            return offset - startOffset;
        }

        public global::TofAr.V0.Slam.ProcessTimeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __slamProcessTime__ = default(float);
            var __dataCollectionTime__ = default(float);
            var __dataProcessingTime__ = default(float);
            var __tofArWaitTime__ = default(float);

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
                        __slamProcessTime__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __dataCollectionTime__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __dataProcessingTime__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __tofArWaitTime__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Slam.ProcessTimeProperty();
            ____result.slamProcessTime = __slamProcessTime__;
            ____result.dataCollectionTime = __dataCollectionTime__;
            ____result.dataProcessingTime = __dataProcessingTime__;
            ____result.tofArWaitTime = __tofArWaitTime__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

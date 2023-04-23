#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArColorResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArColorResolver();

        TofArColorResolver()
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
                var f = TofArColorResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArColorResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArColorResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(22)
            {
                {typeof(global::TofAr.V0.Color.ResolutionProperty[]), 0 },
                {typeof(global::TofAr.V0.Color.CameraMetadataProperty[]), 1 },
                {typeof(global::TofAr.V0.Color.ColorFormat), 2 },
                {typeof(global::TofAr.V0.Color.FlashMode), 3 },
                {typeof(global::TofAr.V0.Color.WhiteBalanceMode), 4 },
                {typeof(global::TofAr.V0.Color.ResolutionProperty), 5 },
                {typeof(global::TofAr.V0.Color.DefaultResolutionProperty), 6 },
                {typeof(global::TofAr.V0.Color.AvailableResolutionsProperty), 7 },
                {typeof(global::TofAr.V0.Color.FormatConvertProperty), 8 },
                {typeof(global::TofAr.V0.Color.CameraMetadataProperty), 9 },
                {typeof(global::TofAr.V0.Color.MetadataEntriesProperty), 10 },
                {typeof(global::TofAr.V0.Color.FocusModeProperty), 11 },
                {typeof(global::TofAr.V0.Color.ExposureModeProperty), 12 },
                {typeof(global::TofAr.V0.Color.WhiteBalanceModeProperty), 13 },
                {typeof(global::TofAr.V0.Color.FrameRateProperty), 14 },
                {typeof(global::TofAr.V0.Color.FrameRateRangeProperty), 15 },
                {typeof(global::TofAr.V0.Color.StabilizationProperty), 16 },
                {typeof(global::TofAr.V0.Color.SharedStreamsProperty), 17 },
                {typeof(global::TofAr.V0.Color.DelayProperty), 18 },
                {typeof(global::TofAr.V0.Color.FunctionStreamParametersProperty), 19 },
                {typeof(global::TofAr.V0.Color.FunctionStreamCallbackProperty), 20 },
                {typeof(global::TofAr.V0.Color.ScreenUVsProperty), 21 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Color.ResolutionProperty>();
                case 1: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Color.CameraMetadataProperty>();
                case 2: return new MessagePack.Formatters.TofAr.V0.Color.ColorFormatFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Color.FlashModeFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Color.WhiteBalanceModeFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Color.ResolutionPropertyFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Color.DefaultResolutionPropertyFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Color.AvailableResolutionsPropertyFormatter();
                case 8: return new MessagePack.Formatters.TofAr.V0.Color.FormatConvertPropertyFormatter();
                case 9: return new MessagePack.Formatters.TofAr.V0.Color.CameraMetadataPropertyFormatter();
                case 10: return new MessagePack.Formatters.TofAr.V0.Color.MetadataEntriesPropertyFormatter();
                case 11: return new MessagePack.Formatters.TofAr.V0.Color.FocusModePropertyFormatter();
                case 12: return new MessagePack.Formatters.TofAr.V0.Color.ExposureModePropertyFormatter();
                case 13: return new MessagePack.Formatters.TofAr.V0.Color.WhiteBalanceModePropertyFormatter();
                case 14: return new MessagePack.Formatters.TofAr.V0.Color.FrameRatePropertyFormatter();
                case 15: return new MessagePack.Formatters.TofAr.V0.Color.FrameRateRangePropertyFormatter();
                case 16: return new MessagePack.Formatters.TofAr.V0.Color.StabilizationPropertyFormatter();
                case 17: return new MessagePack.Formatters.TofAr.V0.Color.SharedStreamsPropertyFormatter();
                case 18: return new MessagePack.Formatters.TofAr.V0.Color.DelayPropertyFormatter();
                case 19: return new MessagePack.Formatters.TofAr.V0.Color.FunctionStreamParametersPropertyFormatter();
                case 20: return new MessagePack.Formatters.TofAr.V0.Color.FunctionStreamCallbackPropertyFormatter();
                case 21: return new MessagePack.Formatters.TofAr.V0.Color.ScreenUVsPropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Color
{
    using System;
    using MessagePack;

    public sealed class ColorFormatFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.ColorFormat>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.ColorFormat value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Color.ColorFormat Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Color.ColorFormat)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class FlashModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.FlashMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.FlashMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Color.FlashMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Color.FlashMode)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class WhiteBalanceModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.WhiteBalanceMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.WhiteBalanceMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Color.WhiteBalanceMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Color.WhiteBalanceMode)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Color
{
    using System;
    using MessagePack;


    public sealed class ResolutionPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.ResolutionProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ResolutionPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "width", 0},
                { "height", 1},
                { "cameraId", 2},
                { "lensFacing", 3},
                { "frameRate", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("lensFacing"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameRate"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.ResolutionProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.cameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.lensFacing);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.frameRate);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.ResolutionProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __cameraId__ = default(string);
            var __lensFacing__ = default(int);
            var __frameRate__ = default(float);

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
                        __cameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __lensFacing__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __frameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.ResolutionProperty();
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.cameraId = __cameraId__;
            ____result.lensFacing = __lensFacing__;
            ____result.frameRate = __frameRate__;
            return ____result;
        }
    }


    public sealed class DefaultResolutionPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.DefaultResolutionProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DefaultResolutionPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "width", 0},
                { "height", 1},
                { "cameraId", 2},
                { "lensFacing", 3},
                { "frameRate", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("lensFacing"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameRate"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.DefaultResolutionProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.cameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.lensFacing);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.frameRate);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.DefaultResolutionProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __cameraId__ = default(string);
            var __lensFacing__ = default(int);
            var __frameRate__ = default(float);

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
                        __cameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __lensFacing__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __frameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.DefaultResolutionProperty();
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.cameraId = __cameraId__;
            ____result.lensFacing = __lensFacing__;
            ____result.frameRate = __frameRate__;
            return ____result;
        }
    }


    public sealed class AvailableResolutionsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.AvailableResolutionsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AvailableResolutionsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "resolutions", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("resolutions"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.AvailableResolutionsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.ResolutionProperty[]>().Serialize(ref bytes, offset, value.resolutions, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.AvailableResolutionsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __resolutions__ = default(global::TofAr.V0.Color.ResolutionProperty[]);

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
                        __resolutions__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.ResolutionProperty[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.AvailableResolutionsProperty();
            ____result.resolutions = __resolutions__;
            return ____result;
        }
    }


    public sealed class FormatConvertPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.FormatConvertProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FormatConvertPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "format", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("format"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.FormatConvertProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.ColorFormat>().Serialize(ref bytes, offset, value.format, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.FormatConvertProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __format__ = default(global::TofAr.V0.Color.ColorFormat);

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
                        __format__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.ColorFormat>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.FormatConvertProperty();
            ____result.format = __format__;
            return ____result;
        }
    }


    public sealed class CameraMetadataPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.CameraMetadataProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraMetadataPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "tag", 0},
                { "type", 1},
                { "count", 2},
                { "data", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("tag"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("type"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("count"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("data"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.CameraMetadataProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.tag);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.type);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.count);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.buffer);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.CameraMetadataProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __tag__ = default(uint);
            var __type__ = default(byte);
            var __count__ = default(uint);
            var __buffer__ = default(ulong);

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
                        __tag__ = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __type__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 2:
                        __count__ = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __buffer__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.CameraMetadataProperty();
            ____result.tag = __tag__;
            ____result.type = __type__;
            ____result.count = __count__;
            ____result.buffer = __buffer__;
            return ____result;
        }
    }


    public sealed class MetadataEntriesPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.MetadataEntriesProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MetadataEntriesPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "entries", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("entries"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.MetadataEntriesProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.CameraMetadataProperty[]>().Serialize(ref bytes, offset, value.entries, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.MetadataEntriesProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __entries__ = default(global::TofAr.V0.Color.CameraMetadataProperty[]);

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
                        __entries__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.CameraMetadataProperty[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.MetadataEntriesProperty();
            ____result.entries = __entries__;
            return ____result;
        }
    }


    public sealed class FocusModePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.FocusModeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FocusModePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "minDistance", 0},
                { "maxDistance", 1},
                { "distance", 2},
                { "autoFocus", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minDistance"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maxDistance"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("distance"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("autoFocus"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.FocusModeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.minDistance);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.maxDistance);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.distance);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.autoFocus);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.FocusModeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __minDistance__ = default(float);
            var __maxDistance__ = default(float);
            var __distance__ = default(float);
            var __autoFocus__ = default(bool);

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
                        __minDistance__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __maxDistance__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __distance__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __autoFocus__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.FocusModeProperty();
            ____result.minDistance = __minDistance__;
            ____result.maxDistance = __maxDistance__;
            ____result.distance = __distance__;
            ____result.autoFocus = __autoFocus__;
            return ____result;
        }
    }


    public sealed class ExposureModePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.ExposureModeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ExposureModePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "autoExposure", 0},
                { "flashAvailable", 1},
                { "flash", 2},
                { "minExposureTime", 3},
                { "maxExposureTime", 4},
                { "exposureTime", 5},
                { "minFrameDurationForCurrentResolution", 6},
                { "maxFrameDuration", 7},
                { "frameDuration", 8},
                { "minSensitivity", 9},
                { "maxSensitivity", 10},
                { "sensitivity", 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("autoExposure"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("flashAvailable"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("flash"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minExposureTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maxExposureTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposureTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minFrameDurationForCurrentResolution"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maxFrameDuration"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameDuration"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minSensitivity"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maxSensitivity"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("sensitivity"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.ExposureModeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 12);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.autoExposure);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.flashAvailable);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.FlashMode>().Serialize(ref bytes, offset, value.flash, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.minExposureTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.maxExposureTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.exposureTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.minFrameDurationForCurrentResolution);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.maxFrameDuration);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.frameDurarion);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.minSensitivity);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.maxSensitivity);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.sensitibity);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.ExposureModeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __autoExposure__ = default(bool);
            var __flashAvailable__ = default(bool);
            var __flash__ = default(global::TofAr.V0.Color.FlashMode);
            var __minExposureTime__ = default(long);
            var __maxExposureTime__ = default(long);
            var __exposureTime__ = default(long);
            var __minFrameDurationForCurrentResolution__ = default(long);
            var __maxFrameDuration__ = default(long);
            var __frameDurarion__ = default(long);
            var __minSensitivity__ = default(int);
            var __maxSensitivity__ = default(int);
            var __sensitibity__ = default(int);

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
                        __autoExposure__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __flashAvailable__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __flash__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.FlashMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __minExposureTime__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 4:
                        __maxExposureTime__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 5:
                        __exposureTime__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 6:
                        __minFrameDurationForCurrentResolution__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 7:
                        __maxFrameDuration__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 8:
                        __frameDurarion__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 9:
                        __minSensitivity__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 10:
                        __maxSensitivity__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 11:
                        __sensitibity__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.ExposureModeProperty();
            ____result.autoExposure = __autoExposure__;
            ____result.flashAvailable = __flashAvailable__;
            ____result.flash = __flash__;
            ____result.minExposureTime = __minExposureTime__;
            ____result.maxExposureTime = __maxExposureTime__;
            ____result.exposureTime = __exposureTime__;
            ____result.minFrameDurationForCurrentResolution = __minFrameDurationForCurrentResolution__;
            ____result.maxFrameDuration = __maxFrameDuration__;
            ____result.frameDurarion = __frameDurarion__;
            ____result.minSensitivity = __minSensitivity__;
            ____result.maxSensitivity = __maxSensitivity__;
            ____result.sensitibity = __sensitibity__;
            return ____result;
        }
    }


    public sealed class WhiteBalanceModePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.WhiteBalanceModeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public WhiteBalanceModePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "mode", 0},
                { "isAvailable", 1},
                { "autoWhiteBalance", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("mode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isAvailable"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("autoWhiteBalance"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.WhiteBalanceModeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.WhiteBalanceMode>().Serialize(ref bytes, offset, value.mode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<bool[]>().Serialize(ref bytes, offset, value.isAvailable, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.autoWhiteBalance);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.WhiteBalanceModeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __mode__ = default(global::TofAr.V0.Color.WhiteBalanceMode);
            var __isAvailable__ = default(bool[]);
            var __autoWhiteBalance__ = default(bool);

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
                        __mode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Color.WhiteBalanceMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __isAvailable__ = formatterResolver.GetFormatterWithVerify<bool[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __autoWhiteBalance__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.WhiteBalanceModeProperty();
            ____result.mode = __mode__;
            ____result.isAvailable = __isAvailable__;
            ____result.autoWhiteBalance = __autoWhiteBalance__;
            return ____result;
        }
    }


    public sealed class FrameRatePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.FrameRateProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FrameRatePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "desiredFrameRate", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("desiredFrameRate"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.FrameRateProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.desiredFrameRate);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.FrameRateProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __desiredFrameRate__ = default(float);

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
                        __desiredFrameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.FrameRateProperty();
            ____result.desiredFrameRate = __desiredFrameRate__;
            return ____result;
        }
    }


    public sealed class FrameRateRangePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.FrameRateRangeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FrameRateRangePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "minimumFrameRate", 0},
                { "maximumFrameRate", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minimumFrameRate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maximumFrameRate"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.FrameRateRangeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.minimumFrameRate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.maximumFrameRate);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.FrameRateRangeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __minimumFrameRate__ = default(float);
            var __maximumFrameRate__ = default(float);

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
                        __minimumFrameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __maximumFrameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.FrameRateRangeProperty();
            ____result.minimumFrameRate = __minimumFrameRate__;
            ____result.maximumFrameRate = __maximumFrameRate__;
            return ____result;
        }
    }


    public sealed class StabilizationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.StabilizationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StabilizationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "lensStabilization", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("lensStabilization"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.StabilizationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.lensStabilization);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.StabilizationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __lensStabilization__ = default(bool);

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
                        __lensStabilization__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.StabilizationProperty();
            ____result.lensStabilization = __lensStabilization__;
            return ____result;
        }
    }


    public sealed class SharedStreamsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.SharedStreamsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SharedStreamsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "isRestart", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isRestart"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.SharedStreamsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isRestart);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.SharedStreamsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isRestart__ = default(bool);

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
                        __isRestart__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.SharedStreamsProperty();
            ____result.isRestart = __isRestart__;
            return ____result;
        }
    }


    public sealed class DelayPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.DelayProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DelayPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "numFramesDelay", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("numFramesDelay"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.DelayProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.numFramesDelay);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.DelayProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __numFramesDelay__ = default(int);

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
                        __numFramesDelay__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.DelayProperty();
            ____result.numFramesDelay = __numFramesDelay__;
            return ____result;
        }
    }


    public sealed class FunctionStreamParametersPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.FunctionStreamParametersProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FunctionStreamParametersPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "functionPointer", 0},
                { "useExternalFunctionStream", 1},
                { "useExternalFunctionCallback", 2},
                { "width", 3},
                { "height", 4},
                { "uvPixelStride", 5},
                { "lensfacing", 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("functionPointer"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("useExternalFunctionStream"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("useExternalFunctionCallback"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uvPixelStride"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("lensfacing"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.FunctionStreamParametersProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 7);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.functionPointer);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.useExternalFunctionStream);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.useExternalFunctionCallback);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.uvPixelStride);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.lensfacing);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.FunctionStreamParametersProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __functionPointer__ = default(long);
            var __useExternalFunctionStream__ = default(bool);
            var __useExternalFunctionCallback__ = default(bool);
            var __width__ = default(int);
            var __height__ = default(int);
            var __uvPixelStride__ = default(int);
            var __lensfacing__ = default(int);

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
                        __functionPointer__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 1:
                        __useExternalFunctionStream__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __useExternalFunctionCallback__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 3:
                        __width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __uvPixelStride__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 6:
                        __lensfacing__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.FunctionStreamParametersProperty();
            ____result.functionPointer = __functionPointer__;
            ____result.useExternalFunctionStream = __useExternalFunctionStream__;
            ____result.useExternalFunctionCallback = __useExternalFunctionCallback__;
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.uvPixelStride = __uvPixelStride__;
            ____result.lensfacing = __lensfacing__;
            return ____result;
        }
    }


    public sealed class FunctionStreamCallbackPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.FunctionStreamCallbackProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FunctionStreamCallbackPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "timestamp", 0},
                { "Y", 1},
                { "U", 2},
                { "V", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("timestamp"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("U"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("V"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.FunctionStreamCallbackProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.timestamp);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.Y);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.U);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.V);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.FunctionStreamCallbackProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __timestamp__ = default(long);
            var __Y__ = default(long);
            var __U__ = default(long);
            var __V__ = default(long);

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
                        __timestamp__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Y__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 2:
                        __U__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 3:
                        __V__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.FunctionStreamCallbackProperty();
            ____result.timestamp = __timestamp__;
            ____result.Y = __Y__;
            ____result.U = __U__;
            ____result.V = __V__;
            return ____result;
        }
    }


    public sealed class ScreenUVsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Color.ScreenUVsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ScreenUVsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "topLeft_x", 0},
                { "topLeft_y", 1},
                { "topRight_x", 2},
                { "topRight_y", 3},
                { "bottomLeft_x", 4},
                { "bottomLeft_y", 5},
                { "bottomRight_x", 6},
                { "bottomRight_y", 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("topLeft_x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("topLeft_y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("topRight_x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("topRight_y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("bottomLeft_x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("bottomLeft_y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("bottomRight_x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("bottomRight_y"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Color.ScreenUVsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 8);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.topLeft_x);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.topLeft_y);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.topRight_x);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.topRight_y);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.bottomLeft_x);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.bottomLeft_y);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.bottomRight_x);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.bottomRight_y);
            return offset - startOffset;
        }

        public global::TofAr.V0.Color.ScreenUVsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __topLeft_x__ = default(float);
            var __topLeft_y__ = default(float);
            var __topRight_x__ = default(float);
            var __topRight_y__ = default(float);
            var __bottomLeft_x__ = default(float);
            var __bottomLeft_y__ = default(float);
            var __bottomRight_x__ = default(float);
            var __bottomRight_y__ = default(float);

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
                        __topLeft_x__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __topLeft_y__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __topRight_x__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __topRight_y__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 4:
                        __bottomLeft_x__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __bottomLeft_y__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 6:
                        __bottomRight_x__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __bottomRight_y__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Color.ScreenUVsProperty();
            ____result.topLeft_x = __topLeft_x__;
            ____result.topLeft_y = __topLeft_y__;
            ____result.topRight_x = __topRight_x__;
            ____result.topRight_y = __topRight_y__;
            ____result.bottomLeft_x = __bottomLeft_x__;
            ____result.bottomLeft_y = __bottomLeft_y__;
            ____result.bottomRight_x = __bottomRight_x__;
            ____result.bottomRight_y = __bottomRight_y__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

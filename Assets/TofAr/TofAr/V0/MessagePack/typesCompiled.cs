#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArResolver();

        TofArResolver()
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
                var f = TofArResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(18)
            {
                {typeof(global::System.Collections.Generic.List<string>), 0 },
                {typeof(global::TofAr.V0.LogLevel), 1 },
                {typeof(global::TofAr.V0.RunMode), 2 },
                {typeof(global::TofAr.V0.IosCameraApi), 3 },
                {typeof(global::TofAr.V0.AvFoundationDepthFiltering), 4 },
                {typeof(global::TofAr.V0.LogLevelProperty), 5 },
                {typeof(global::TofAr.V0.DeviceCapabilityProperty), 6 },
                {typeof(global::TofAr.V0.TofArVector2), 7 },
                {typeof(global::TofAr.V0.TofArVector3), 8 },
                {typeof(global::TofAr.V0.TofArQuaternion), 9 },
                {typeof(global::TofAr.V0.TofArTransform), 10 },
                {typeof(global::TofAr.V0.CameraPoseProperty), 11 },
                {typeof(global::TofAr.V0.RuntimeSettingsProperty), 12 },
                {typeof(global::TofAr.V0.DeviceOrientationsProperty), 13 },
                {typeof(global::TofAr.V0.DirectoryListProperty), 14 },
                {typeof(global::TofAr.V0.PlatformConfigurationIos), 15 },
                {typeof(global::TofAr.V0.PlatformConfigurationProperty), 16 },
                {typeof(global::TofAr.V0.MirrorSettingsProperty), 17 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ListFormatter<string>();
                case 1: return new MessagePack.Formatters.TofAr.V0.LogLevelFormatter();
                case 2: return new MessagePack.Formatters.TofAr.V0.RunModeFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.IosCameraApiFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.AvFoundationDepthFilteringFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.LogLevelPropertyFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.DeviceCapabilityPropertyFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.TofArVector2Formatter();
                case 8: return new MessagePack.Formatters.TofAr.V0.TofArVector3Formatter();
                case 9: return new MessagePack.Formatters.TofAr.V0.TofArQuaternionFormatter();
                case 10: return new MessagePack.Formatters.TofAr.V0.TofArTransformFormatter();
                case 11: return new MessagePack.Formatters.TofAr.V0.CameraPosePropertyFormatter();
                case 12: return new MessagePack.Formatters.TofAr.V0.RuntimeSettingsPropertyFormatter();
                case 13: return new MessagePack.Formatters.TofAr.V0.DeviceOrientationsPropertyFormatter();
                case 14: return new MessagePack.Formatters.TofAr.V0.DirectoryListPropertyFormatter();
                case 15: return new MessagePack.Formatters.TofAr.V0.PlatformConfigurationIosFormatter();
                case 16: return new MessagePack.Formatters.TofAr.V0.PlatformConfigurationPropertyFormatter();
                case 17: return new MessagePack.Formatters.TofAr.V0.MirrorSettingsPropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0
{
    using System;
    using MessagePack;

    public sealed class LogLevelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.LogLevel>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.LogLevel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.LogLevel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.LogLevel)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class RunModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.RunMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.RunMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.RunMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.RunMode)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class IosCameraApiFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.IosCameraApi>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.IosCameraApi value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.IosCameraApi Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.IosCameraApi)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class AvFoundationDepthFilteringFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.AvFoundationDepthFiltering>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.AvFoundationDepthFiltering value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.AvFoundationDepthFiltering Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.AvFoundationDepthFiltering)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0
{
    using System;
    using MessagePack;


    public sealed class LogLevelPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.LogLevelProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LogLevelPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "logLevel", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("logLevel"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.LogLevelProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.LogLevel>().Serialize(ref bytes, offset, value.logLevel, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.LogLevelProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __logLevel__ = default(global::TofAr.V0.LogLevel);

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
                        __logLevel__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.LogLevel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.LogLevelProperty();
            ____result.logLevel = __logLevel__;
            return ____result;
        }
    }


    public sealed class DeviceCapabilityPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.DeviceCapabilityProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DeviceCapabilityPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "isQualcommSupports", 0},
                { "deviceAttributesString", 1},
                { "modelName", 2},
                { "modelGroupName", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isQualcommSupports"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("deviceAttributesString"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("modelName"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("modelGroupName"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.DeviceCapabilityProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isQualcommSupports);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.deviceAttributesString, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.modelName, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.modelGroupName, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.DeviceCapabilityProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isQualcommSupports__ = default(bool);
            var __deviceAttributesString__ = default(string);
            var __modelName__ = default(string);
            var __modelGroupName__ = default(string);

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
                        __isQualcommSupports__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __deviceAttributesString__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __modelName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __modelGroupName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.DeviceCapabilityProperty();
            ____result.isQualcommSupports = __isQualcommSupports__;
            ____result.deviceAttributesString = __deviceAttributesString__;
            ____result.modelName = __modelName__;
            ____result.modelGroupName = __modelGroupName__;
            return ____result;
        }
    }


    public sealed class TofArVector2Formatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.TofArVector2>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TofArVector2Formatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "x", 0},
                { "y", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("y"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.TofArVector2 value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.x);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.y);
            return offset - startOffset;
        }

        public global::TofAr.V0.TofArVector2 Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __x__ = default(float);
            var __y__ = default(float);

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
                        __x__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __y__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.TofArVector2();
            ____result.x = __x__;
            ____result.y = __y__;
            return ____result;
        }
    }


    public sealed class TofArVector3Formatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.TofArVector3>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TofArVector3Formatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "x", 0},
                { "y", 1},
                { "z", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("z"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.TofArVector3 value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.x);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.y);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.z);
            return offset - startOffset;
        }

        public global::TofAr.V0.TofArVector3 Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __x__ = default(float);
            var __y__ = default(float);
            var __z__ = default(float);

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
                        __x__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __y__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __z__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.TofArVector3();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            return ____result;
        }
    }


    public sealed class TofArQuaternionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.TofArQuaternion>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TofArQuaternionFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "x", 0},
                { "y", 1},
                { "z", 2},
                { "w", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("z"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("w"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.TofArQuaternion value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.x);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.y);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.z);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.w);
            return offset - startOffset;
        }

        public global::TofAr.V0.TofArQuaternion Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __x__ = default(float);
            var __y__ = default(float);
            var __z__ = default(float);
            var __w__ = default(float);

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
                        __x__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __y__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __z__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __w__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.TofArQuaternion();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            ____result.w = __w__;
            return ____result;
        }
    }


    public sealed class TofArTransformFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.TofArTransform>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TofArTransformFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "position", 0},
                { "rotation", 1},
                { "forward", 2},
                { "right", 3},
                { "up", 4},
                { "scale", 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rotation"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("forward"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("right"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("up"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("scale"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.TofArTransform value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 6);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArQuaternion>().Serialize(ref bytes, offset, value.rotation, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.forward, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.right, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.up, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.scale, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.TofArTransform Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __position__ = default(global::TofAr.V0.TofArVector3);
            var __rotation__ = default(global::TofAr.V0.TofArQuaternion);
            var __forward__ = default(global::TofAr.V0.TofArVector3);
            var __right__ = default(global::TofAr.V0.TofArVector3);
            var __up__ = default(global::TofAr.V0.TofArVector3);
            var __scale__ = default(global::TofAr.V0.TofArVector3);

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
                        __position__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __rotation__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArQuaternion>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __forward__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __right__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __up__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __scale__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.TofArTransform();
            ____result.position = __position__;
            ____result.rotation = __rotation__;
            ____result.forward = __forward__;
            ____result.right = __right__;
            ____result.up = __up__;
            ____result.scale = __scale__;
            return ____result;
        }
    }


    public sealed class CameraPosePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.CameraPoseProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraPosePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "position", 0},
                { "rotation", 1},
                { "acceleration", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rotation"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("acceleration"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.CameraPoseProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArQuaternion>().Serialize(ref bytes, offset, value.rotation, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.acceleration, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.CameraPoseProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __position__ = default(global::TofAr.V0.TofArVector3);
            var __rotation__ = default(global::TofAr.V0.TofArQuaternion);
            var __acceleration__ = default(global::TofAr.V0.TofArVector3);

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
                        __position__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __rotation__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArQuaternion>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __acceleration__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.CameraPoseProperty();
            ____result.position = __position__;
            ____result.rotation = __rotation__;
            ____result.acceleration = __acceleration__;
            return ____result;
        }
    }


    public sealed class RuntimeSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.RuntimeSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RuntimeSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "runMode", 0},
                { "isRemoteServer", 1},
                { "isUsingArFoundation", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("runMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isRemoteServer"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isUsingArFoundation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.RuntimeSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.RunMode>().Serialize(ref bytes, offset, value.runMode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isRemoteServer);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isUsingArFoundation);
            return offset - startOffset;
        }

        public global::TofAr.V0.RuntimeSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __runMode__ = default(global::TofAr.V0.RunMode);
            var __isRemoteServer__ = default(bool);
            var __isUsingArFoundation__ = default(bool);

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
                        __runMode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.RunMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __isRemoteServer__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __isUsingArFoundation__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.RuntimeSettingsProperty();
            ____result.runMode = __runMode__;
            ____result.isRemoteServer = __isRemoteServer__;
            ____result.isUsingArFoundation = __isUsingArFoundation__;
            return ____result;
        }
    }


    public sealed class DeviceOrientationsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.DeviceOrientationsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DeviceOrientationsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "deviceOrientation", 0},
                { "screenOrientation", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("deviceOrientation"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("screenOrientation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.DeviceOrientationsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value._deviceOrientation);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value._screenOrientation);
            return offset - startOffset;
        }

        public global::TofAr.V0.DeviceOrientationsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var ___deviceOrientation__ = default(int);
            var ___screenOrientation__ = default(int);

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
                        ___deviceOrientation__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        ___screenOrientation__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.DeviceOrientationsProperty();
            ____result._deviceOrientation = ___deviceOrientation__;
            ____result._screenOrientation = ___screenOrientation__;
            return ____result;
        }
    }


    public sealed class DirectoryListPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.DirectoryListProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DirectoryListPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "path", 0},
                { "directoryList", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("path"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("directoryList"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.DirectoryListProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.path, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Serialize(ref bytes, offset, value.directoryList, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.DirectoryListProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __path__ = default(string);
            var __directoryList__ = default(global::System.Collections.Generic.List<string>);

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
                        __path__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __directoryList__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.DirectoryListProperty();
            ____result.path = __path__;
            ____result.directoryList = __directoryList__;
            return ____result;
        }
    }


    public sealed class PlatformConfigurationIosFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.PlatformConfigurationIos>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PlatformConfigurationIosFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "sessionFramerate", 0},
                { "cameraApi", 1},
                { "depthFiltering", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("sessionFramerate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraApi"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthFiltering"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.PlatformConfigurationIos value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.sessionFramerate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.IosCameraApi>().Serialize(ref bytes, offset, value.cameraApi, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.AvFoundationDepthFiltering>().Serialize(ref bytes, offset, value.depthFiltering, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.PlatformConfigurationIos Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __sessionFramerate__ = default(int);
            var __cameraApi__ = default(global::TofAr.V0.IosCameraApi);
            var __depthFiltering__ = default(global::TofAr.V0.AvFoundationDepthFiltering);

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
                        __sessionFramerate__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __cameraApi__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.IosCameraApi>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __depthFiltering__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.AvFoundationDepthFiltering>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.PlatformConfigurationIos();
            ____result.sessionFramerate = __sessionFramerate__;
            ____result.cameraApi = __cameraApi__;
            ____result.depthFiltering = __depthFiltering__;
            return ____result;
        }
    }


    public sealed class PlatformConfigurationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.PlatformConfigurationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PlatformConfigurationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "platformConfigurationIos", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("platformConfigurationIos"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.PlatformConfigurationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.PlatformConfigurationIos>().Serialize(ref bytes, offset, value.platformConfigurationIos, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.PlatformConfigurationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __platformConfigurationIos__ = default(global::TofAr.V0.PlatformConfigurationIos);

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
                        __platformConfigurationIos__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.PlatformConfigurationIos>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.PlatformConfigurationProperty();
            ____result.platformConfigurationIos = __platformConfigurationIos__;
            return ____result;
        }
    }


    public sealed class MirrorSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.MirrorSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MirrorSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "isMirroring", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isMirroring"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.MirrorSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isMirroring);
            return offset - startOffset;
        }

        public global::TofAr.V0.MirrorSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isMirroring__ = default(bool);

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
                        __isMirroring__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.MirrorSettingsProperty();
            ____result.isMirroring = __isMirroring__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

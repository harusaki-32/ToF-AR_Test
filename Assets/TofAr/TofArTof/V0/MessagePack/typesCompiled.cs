#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArTofResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArTofResolver();

        TofArTofResolver()
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
                var f = TofArTofResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArTofResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArTofResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(25)
            {
                {typeof(global::TofAr.V0.Tof.CameraConfigurationProperty[]), 0 },
                {typeof(global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode), 1 },
                {typeof(global::TofAr.V0.Tof.DepthPrivateFilter.DepthPrivateFilterSettingsProperty), 2 },
                {typeof(global::TofAr.V0.Tof.InternalParameter), 3 },
                {typeof(global::TofAr.V0.Tof.Matrix), 4 },
                {typeof(global::TofAr.V0.Tof.Vector), 5 },
                {typeof(global::TofAr.V0.Tof.CalibrationSettingsProperty), 6 },
                {typeof(global::TofAr.V0.Tof.Camera2IntrinsicsProperty), 7 },
                {typeof(global::TofAr.V0.Tof.CameraConfigurationProperty), 8 },
                {typeof(global::TofAr.V0.Tof.CameraConfigurationsProperty), 9 },
                {typeof(global::TofAr.V0.Tof.SetConfigurationIdProperty), 10 },
                {typeof(global::TofAr.V0.Tof.ProcessTargetsProperty), 11 },
                {typeof(global::TofAr.V0.Tof.DepthConfidenceProperty), 12 },
                {typeof(global::TofAr.V0.Tof.PointCloudProperty), 13 },
                {typeof(global::TofAr.V0.Tof.FrameRateProperty), 14 },
                {typeof(global::TofAr.V0.Tof.FrameRateRangeProperty), 15 },
                {typeof(global::TofAr.V0.Tof.DelayProperty), 16 },
                {typeof(global::TofAr.V0.Tof.Camera2ConfigurationProperty), 17 },
                {typeof(global::TofAr.V0.Tof.Camera2ConfigurationsProperty), 18 },
                {typeof(global::TofAr.V0.Tof.Camera2SetConfigurationIdProperty), 19 },
                {typeof(global::TofAr.V0.Tof.Camera2DefaultConfigurationProperty), 20 },
                {typeof(global::TofAr.V0.Tof.FunctionStreamParametersProperty), 21 },
                {typeof(global::TofAr.V0.Tof.FunctionStreamCallbackProperty), 22 },
                {typeof(global::TofAr.V0.Tof.SharedStreamsProperty), 23 },
                {typeof(global::TofAr.V0.Tof.ExposureProperty), 24 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Tof.CameraConfigurationProperty>();
                case 1: return new MessagePack.Formatters.TofAr.V0.Tof.DepthPrivateFilter.TransformationModeFormatter();
                case 2: return new MessagePack.Formatters.TofAr.V0.Tof.DepthPrivateFilter.DepthPrivateFilterSettingsPropertyFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Tof.InternalParameterFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Tof.MatrixFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Tof.VectorFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Tof.CalibrationSettingsPropertyFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Tof.Camera2IntrinsicsPropertyFormatter();
                case 8: return new MessagePack.Formatters.TofAr.V0.Tof.CameraConfigurationPropertyFormatter();
                case 9: return new MessagePack.Formatters.TofAr.V0.Tof.CameraConfigurationsPropertyFormatter();
                case 10: return new MessagePack.Formatters.TofAr.V0.Tof.SetConfigurationIdPropertyFormatter();
                case 11: return new MessagePack.Formatters.TofAr.V0.Tof.ProcessTargetsPropertyFormatter();
                case 12: return new MessagePack.Formatters.TofAr.V0.Tof.DepthConfidencePropertyFormatter();
                case 13: return new MessagePack.Formatters.TofAr.V0.Tof.PointCloudPropertyFormatter();
                case 14: return new MessagePack.Formatters.TofAr.V0.Tof.FrameRatePropertyFormatter();
                case 15: return new MessagePack.Formatters.TofAr.V0.Tof.FrameRateRangePropertyFormatter();
                case 16: return new MessagePack.Formatters.TofAr.V0.Tof.DelayPropertyFormatter();
                case 17: return new MessagePack.Formatters.TofAr.V0.Tof.Camera2ConfigurationPropertyFormatter();
                case 18: return new MessagePack.Formatters.TofAr.V0.Tof.Camera2ConfigurationsPropertyFormatter();
                case 19: return new MessagePack.Formatters.TofAr.V0.Tof.Camera2SetConfigurationIdPropertyFormatter();
                case 20: return new MessagePack.Formatters.TofAr.V0.Tof.Camera2DefaultConfigurationPropertyFormatter();
                case 21: return new MessagePack.Formatters.TofAr.V0.Tof.FunctionStreamParametersPropertyFormatter();
                case 22: return new MessagePack.Formatters.TofAr.V0.Tof.FunctionStreamCallbackPropertyFormatter();
                case 23: return new MessagePack.Formatters.TofAr.V0.Tof.SharedStreamsPropertyFormatter();
                case 24: return new MessagePack.Formatters.TofAr.V0.Tof.ExposurePropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Tof.DepthPrivateFilter
{
    using System;
    using MessagePack;

    public sealed class TransformationModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Tof.DepthPrivateFilter
{
    using System;
    using MessagePack;


    public sealed class DepthPrivateFilterSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.DepthPrivateFilter.DepthPrivateFilterSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DepthPrivateFilterSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "softwareId", 0},
                { "filterEnabled", 1},
                { "calibrationDataPath", 2},
                { "transformationMode", 3},
                { "frameRate", 4},
                { "exposure", 5},
                { "hcg", 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("softwareId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("filterEnabled"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("calibrationDataPath"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("transformationMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameRate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposure"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("hcg"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.DepthPrivateFilter.DepthPrivateFilterSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 7);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.softwareId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.filterEnabled);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.calibrationDataPath, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode>().Serialize(ref bytes, offset, value.transformationMode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.frameRate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.exposure);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.hcg);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.DepthPrivateFilter.DepthPrivateFilterSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __softwareId__ = default(int);
            var __filterEnabled__ = default(bool);
            var __calibrationDataPath__ = default(string);
            var __transformationMode__ = default(global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode);
            var __frameRate__ = default(long);
            var __exposure__ = default(long);
            var __hcg__ = default(bool);

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
                        __softwareId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __filterEnabled__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __calibrationDataPath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __transformationMode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.DepthPrivateFilter.TransformationMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __frameRate__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 5:
                        __exposure__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 6:
                        __hcg__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.DepthPrivateFilter.DepthPrivateFilterSettingsProperty();
            ____result.softwareId = __softwareId__;
            ____result.filterEnabled = __filterEnabled__;
            ____result.calibrationDataPath = __calibrationDataPath__;
            ____result.transformationMode = __transformationMode__;
            ____result.frameRate = __frameRate__;
            ____result.exposure = __exposure__;
            ____result.hcg = __hcg__;
            return ____result;
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

namespace MessagePack.Formatters.TofAr.V0.Tof
{
    using System;
    using MessagePack;


    public sealed class InternalParameterFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.InternalParameter>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public InternalParameterFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "fx", 0},
                { "fy", 1},
                { "cx", 2},
                { "cy", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fy"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cy"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.InternalParameter value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fy);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cy);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.InternalParameter Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __fx__ = default(float);
            var __fy__ = default(float);
            var __cx__ = default(float);
            var __cy__ = default(float);

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
                        __fx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __fy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __cx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __cy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.InternalParameter();
            ____result.fx = __fx__;
            ____result.fy = __fy__;
            ____result.cx = __cx__;
            ____result.cy = __cy__;
            return ____result;
        }
    }


    public sealed class MatrixFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.Matrix>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatrixFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "a", 0},
                { "b", 1},
                { "c", 2},
                { "d", 3},
                { "e", 4},
                { "f", 5},
                { "g", 6},
                { "h", 7},
                { "i", 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("a"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("b"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("c"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("d"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("e"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("f"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("g"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("h"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("i"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.Matrix value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 9);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.a);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.b);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.c);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.d);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.e);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.f);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.g);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.h);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.i);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.Matrix Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __a__ = default(float);
            var __b__ = default(float);
            var __c__ = default(float);
            var __d__ = default(float);
            var __e__ = default(float);
            var __f__ = default(float);
            var __g__ = default(float);
            var __h__ = default(float);
            var __i__ = default(float);

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
                        __a__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __b__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __c__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __d__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 4:
                        __e__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __f__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 6:
                        __g__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __h__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __i__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.Matrix();
            ____result.a = __a__;
            ____result.b = __b__;
            ____result.c = __c__;
            ____result.d = __d__;
            ____result.e = __e__;
            ____result.f = __f__;
            ____result.g = __g__;
            ____result.h = __h__;
            ____result.i = __i__;
            return ____result;
        }
    }


    public sealed class VectorFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.Vector>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public VectorFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.Vector value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
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

        public global::TofAr.V0.Tof.Vector Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
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

            var ____result = new global::TofAr.V0.Tof.Vector();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            return ____result;
        }
    }


    public sealed class CalibrationSettingsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.CalibrationSettingsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CalibrationSettingsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "colorWidth", 0},
                { "colorHeight", 1},
                { "depthWidth", 2},
                { "depthHeight", 3},
                { "isCalibrated", 4},
                { "c", 5},
                { "d", 6},
                { "R", 7},
                { "T", 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("colorWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("colorHeight"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depthHeight"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isCalibrated"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("c"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("d"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("R"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("T"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.CalibrationSettingsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 9);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.colorWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.colorHeight);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.depthWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.depthHeight);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isCalibrated);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.InternalParameter>().Serialize(ref bytes, offset, value.c, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.InternalParameter>().Serialize(ref bytes, offset, value.d, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Matrix>().Serialize(ref bytes, offset, value.R, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Vector>().Serialize(ref bytes, offset, value.T, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.CalibrationSettingsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __colorWidth__ = default(int);
            var __colorHeight__ = default(int);
            var __depthWidth__ = default(int);
            var __depthHeight__ = default(int);
            var __isCalibrated__ = default(bool);
            var __c__ = default(global::TofAr.V0.Tof.InternalParameter);
            var __d__ = default(global::TofAr.V0.Tof.InternalParameter);
            var __R__ = default(global::TofAr.V0.Tof.Matrix);
            var __T__ = default(global::TofAr.V0.Tof.Vector);

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
                        __colorWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __colorHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __depthWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __depthHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __isCalibrated__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 5:
                        __c__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.InternalParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __d__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.InternalParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 7:
                        __R__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Matrix>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 8:
                        __T__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Vector>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.CalibrationSettingsProperty();
            ____result.colorWidth = __colorWidth__;
            ____result.colorHeight = __colorHeight__;
            ____result.depthWidth = __depthWidth__;
            ____result.depthHeight = __depthHeight__;
            ____result.isCalibrated = __isCalibrated__;
            ____result.c = __c__;
            ____result.d = __d__;
            ____result.R = __R__;
            ____result.T = __T__;
            return ____result;
        }
    }


    public sealed class Camera2IntrinsicsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.Camera2IntrinsicsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public Camera2IntrinsicsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "fx", 0},
                { "fy", 1},
                { "cx", 2},
                { "cy", 3},
                { "k1", 4},
                { "k2", 5},
                { "k3", 6},
                { "p1", 7},
                { "p2", 8},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fy"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cy"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k1"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k2"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k3"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("p1"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("p2"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.Camera2IntrinsicsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 9);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fy);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cy);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.p1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.p2);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.Camera2IntrinsicsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __fx__ = default(float);
            var __fy__ = default(float);
            var __cx__ = default(float);
            var __cy__ = default(float);
            var __k1__ = default(float);
            var __k2__ = default(float);
            var __k3__ = default(float);
            var __p1__ = default(float);
            var __p2__ = default(float);

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
                        __fx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __fy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __cx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __cy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 4:
                        __k1__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __k2__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 6:
                        __k3__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __p1__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __p2__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.Camera2IntrinsicsProperty();
            ____result.fx = __fx__;
            ____result.fy = __fy__;
            ____result.cx = __cx__;
            ____result.cy = __cy__;
            ____result.k1 = __k1__;
            ____result.k2 = __k2__;
            ____result.k3 = __k3__;
            ____result.p1 = __p1__;
            ____result.p2 = __p2__;
            return ____result;
        }
    }


    public sealed class CameraConfigurationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.CameraConfigurationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraConfigurationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "isCalibrated", 0},
                { "uid", 1},
                { "width", 2},
                { "height", 3},
                { "cameraId", 4},
                { "colorCameraId", 5},
                { "frameRate", 6},
                { "unambiguousRange", 7},
                { "lensFacing", 8},
                { "intrinsics", 9},
                { "name", 10},
                { "isDepthPrivate", 11},
                { "isFusion", 12},
                { "fusionSourceWidth", 13},
                { "fusionSourceHeight", 14},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isCalibrated"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uid"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("colorCameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameRate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("unambiguousRange"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("lensFacing"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("intrinsics"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isDepthPrivate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isFusion"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fusionSourceWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fusionSourceHeight"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.CameraConfigurationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 15);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isCalibrated);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.uid);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.cameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.colorCameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.frameRate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.unambiguousRange);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.lensFacing);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Camera2IntrinsicsProperty>().Serialize(ref bytes, offset, value.intrinsics, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isDepthPrivate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isFusion);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[13]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.fusionSourceWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[14]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.fusionSourceHeight);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.CameraConfigurationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isCalibrated__ = default(bool);
            var __uid__ = default(int);
            var __width__ = default(int);
            var __height__ = default(int);
            var __cameraId__ = default(string);
            var __colorCameraId__ = default(string);
            var __frameRate__ = default(float);
            var __unambiguousRange__ = default(float);
            var __lensFacing__ = default(int);
            var __intrinsics__ = default(global::TofAr.V0.Tof.Camera2IntrinsicsProperty);
            var __name__ = default(string);
            var __isDepthPrivate__ = default(bool);
            var __isFusion__ = default(bool);
            var __fusionSourceWidth__ = default(int);
            var __fusionSourceHeight__ = default(int);

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
                        __isCalibrated__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __uid__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __cameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __colorCameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __frameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __unambiguousRange__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __lensFacing__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 9:
                        __intrinsics__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Camera2IntrinsicsProperty>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 11:
                        __isDepthPrivate__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 12:
                        __isFusion__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 13:
                        __fusionSourceWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 14:
                        __fusionSourceHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.CameraConfigurationProperty();
            ____result.isCalibrated = __isCalibrated__;
            ____result.uid = __uid__;
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.cameraId = __cameraId__;
            ____result.colorCameraId = __colorCameraId__;
            ____result.frameRate = __frameRate__;
            ____result.unambiguousRange = __unambiguousRange__;
            ____result.lensFacing = __lensFacing__;
            ____result.intrinsics = __intrinsics__;
            ____result.name = __name__;
            ____result.isDepthPrivate = __isDepthPrivate__;
            ____result.isFusion = __isFusion__;
            ____result.fusionSourceWidth = __fusionSourceWidth__;
            ____result.fusionSourceHeight = __fusionSourceHeight__;
            return ____result;
        }
    }


    public sealed class CameraConfigurationsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.CameraConfigurationsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraConfigurationsPropertyFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.CameraConfigurationsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.CameraConfigurationProperty[]>().Serialize(ref bytes, offset, value.configurations, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.CameraConfigurationsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __configurations__ = default(global::TofAr.V0.Tof.CameraConfigurationProperty[]);

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
                        __configurations__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.CameraConfigurationProperty[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.CameraConfigurationsProperty();
            ____result.configurations = __configurations__;
            return ____result;
        }
    }


    public sealed class SetConfigurationIdPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.SetConfigurationIdProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SetConfigurationIdPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "uid", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uid"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.SetConfigurationIdProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.uid);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.SetConfigurationIdProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __uid__ = default(int);

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
                        __uid__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.SetConfigurationIdProperty();
            ____result.uid = __uid__;
            return ____result;
        }
    }


    public sealed class ProcessTargetsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.ProcessTargetsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ProcessTargetsPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "processDepth", 0},
                { "processConfidence", 1},
                { "processPointCloud", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("processDepth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("processConfidence"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("processPointCloud"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.ProcessTargetsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.processDepth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.processConfidence);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.processPointCloud);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.ProcessTargetsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __processDepth__ = default(bool);
            var __processConfidence__ = default(bool);
            var __processPointCloud__ = default(bool);

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
                        __processDepth__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __processConfidence__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __processPointCloud__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.ProcessTargetsProperty();
            ____result.processDepth = __processDepth__;
            ____result.processConfidence = __processConfidence__;
            ____result.processPointCloud = __processPointCloud__;
            return ____result;
        }
    }


    public sealed class DepthConfidencePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.DepthConfidenceProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DepthConfidencePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "depth16ConfidenceConvert", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depth16ConfidenceConvert"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.DepthConfidenceProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.depth16ConfidenceConvert);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.DepthConfidenceProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __depth16ConfidenceConvert__ = default(bool);

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
                        __depth16ConfidenceConvert__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.DepthConfidenceProperty();
            ____result.depth16ConfidenceConvert = __depth16ConfidenceConvert__;
            return ____result;
        }
    }


    public sealed class PointCloudPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.PointCloudProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PointCloudPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "width", 0},
                { "height", 1},
                { "pixel_format", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pixel_format"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.PointCloudProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.pixel_format, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.PointCloudProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __pixel_format__ = default(string);

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
                        __pixel_format__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.PointCloudProperty();
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.pixel_format = __pixel_format__;
            return ____result;
        }
    }


    public sealed class FrameRatePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.FrameRateProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.FrameRateProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Tof.FrameRateProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Tof.FrameRateProperty();
            ____result.desiredFrameRate = __desiredFrameRate__;
            return ____result;
        }
    }


    public sealed class FrameRateRangePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.FrameRateRangeProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.FrameRateRangeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Tof.FrameRateRangeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Tof.FrameRateRangeProperty();
            ____result.minimumFrameRate = __minimumFrameRate__;
            ____result.maximumFrameRate = __maximumFrameRate__;
            return ____result;
        }
    }


    public sealed class DelayPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.DelayProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.DelayProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Tof.DelayProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Tof.DelayProperty();
            ____result.numFramesDelay = __numFramesDelay__;
            return ____result;
        }
    }


    public sealed class Camera2ConfigurationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.Camera2ConfigurationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public Camera2ConfigurationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "isCalibrated", 0},
                { "uid", 1},
                { "width", 2},
                { "height", 3},
                { "cameraId", 4},
                { "colorCameraId", 5},
                { "frameRate", 6},
                { "unambiguousRange", 7},
                { "lensFacing", 8},
                { "intrinsics", 9},
                { "name", 10},
                { "isDepthPrivate", 11},
                { "isFusion", 12},
                { "fusionSourceWidth", 13},
                { "fusionSourceHeight", 14},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isCalibrated"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uid"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("colorCameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameRate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("unambiguousRange"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("lensFacing"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("intrinsics"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isDepthPrivate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isFusion"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fusionSourceWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fusionSourceHeight"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.Camera2ConfigurationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 15);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isCalibrated);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.uid);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.cameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.colorCameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.frameRate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.unambiguousRange);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.lensFacing);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Camera2IntrinsicsProperty>().Serialize(ref bytes, offset, value.intrinsics, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isDepthPrivate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isFusion);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[13]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.fusionSourceWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[14]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.fusionSourceHeight);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.Camera2ConfigurationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isCalibrated__ = default(bool);
            var __uid__ = default(int);
            var __width__ = default(int);
            var __height__ = default(int);
            var __cameraId__ = default(string);
            var __colorCameraId__ = default(string);
            var __frameRate__ = default(float);
            var __unambiguousRange__ = default(float);
            var __lensFacing__ = default(int);
            var __intrinsics__ = default(global::TofAr.V0.Tof.Camera2IntrinsicsProperty);
            var __name__ = default(string);
            var __isDepthPrivate__ = default(bool);
            var __isFusion__ = default(bool);
            var __fusionSourceWidth__ = default(int);
            var __fusionSourceHeight__ = default(int);

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
                        __isCalibrated__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __uid__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __cameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __colorCameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __frameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __unambiguousRange__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __lensFacing__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 9:
                        __intrinsics__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Camera2IntrinsicsProperty>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 11:
                        __isDepthPrivate__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 12:
                        __isFusion__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 13:
                        __fusionSourceWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 14:
                        __fusionSourceHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.Camera2ConfigurationProperty();
            ____result.isCalibrated = __isCalibrated__;
            ____result.uid = __uid__;
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.cameraId = __cameraId__;
            ____result.colorCameraId = __colorCameraId__;
            ____result.frameRate = __frameRate__;
            ____result.unambiguousRange = __unambiguousRange__;
            ____result.lensFacing = __lensFacing__;
            ____result.intrinsics = __intrinsics__;
            ____result.name = __name__;
            ____result.isDepthPrivate = __isDepthPrivate__;
            ____result.isFusion = __isFusion__;
            ____result.fusionSourceWidth = __fusionSourceWidth__;
            ____result.fusionSourceHeight = __fusionSourceHeight__;
            return ____result;
        }
    }


    public sealed class Camera2ConfigurationsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.Camera2ConfigurationsProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public Camera2ConfigurationsPropertyFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.Camera2ConfigurationsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.CameraConfigurationProperty[]>().Serialize(ref bytes, offset, value.configurations, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.Camera2ConfigurationsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __configurations__ = default(global::TofAr.V0.Tof.CameraConfigurationProperty[]);

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
                        __configurations__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.CameraConfigurationProperty[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.Camera2ConfigurationsProperty();
            ____result.configurations = __configurations__;
            return ____result;
        }
    }


    public sealed class Camera2SetConfigurationIdPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.Camera2SetConfigurationIdProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public Camera2SetConfigurationIdPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "uid", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uid"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.Camera2SetConfigurationIdProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.uid);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.Camera2SetConfigurationIdProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __uid__ = default(int);

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
                        __uid__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.Camera2SetConfigurationIdProperty();
            ____result.uid = __uid__;
            return ____result;
        }
    }


    public sealed class Camera2DefaultConfigurationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.Camera2DefaultConfigurationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public Camera2DefaultConfigurationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "isCalibrated", 0},
                { "uid", 1},
                { "width", 2},
                { "height", 3},
                { "cameraId", 4},
                { "colorCameraId", 5},
                { "frameRate", 6},
                { "unambiguousRange", 7},
                { "lensFacing", 8},
                { "intrinsics", 9},
                { "name", 10},
                { "isDepthPrivate", 11},
                { "isFusion", 12},
                { "fusionSourceWidth", 13},
                { "fusionSourceHeight", 14},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isCalibrated"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uid"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("colorCameraId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameRate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("unambiguousRange"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("lensFacing"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("intrinsics"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isDepthPrivate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isFusion"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fusionSourceWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fusionSourceHeight"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.Camera2DefaultConfigurationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 15);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isCalibrated);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.uid);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.cameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.colorCameraId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.frameRate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.unambiguousRange);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.lensFacing);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Camera2IntrinsicsProperty>().Serialize(ref bytes, offset, value.intrinsics, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isDepthPrivate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isFusion);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[13]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.fusionSourceWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[14]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.fusionSourceHeight);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.Camera2DefaultConfigurationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isCalibrated__ = default(bool);
            var __uid__ = default(int);
            var __width__ = default(int);
            var __height__ = default(int);
            var __cameraId__ = default(string);
            var __colorCameraId__ = default(string);
            var __frameRate__ = default(float);
            var __unambiguousRange__ = default(float);
            var __lensFacing__ = default(int);
            var __intrinsics__ = default(global::TofAr.V0.Tof.Camera2IntrinsicsProperty);
            var __name__ = default(string);
            var __isDepthPrivate__ = default(bool);
            var __isFusion__ = default(bool);
            var __fusionSourceWidth__ = default(int);
            var __fusionSourceHeight__ = default(int);

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
                        __isCalibrated__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __uid__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __cameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __colorCameraId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __frameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __unambiguousRange__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __lensFacing__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 9:
                        __intrinsics__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Tof.Camera2IntrinsicsProperty>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 11:
                        __isDepthPrivate__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 12:
                        __isFusion__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 13:
                        __fusionSourceWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 14:
                        __fusionSourceHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.Camera2DefaultConfigurationProperty();
            ____result.isCalibrated = __isCalibrated__;
            ____result.uid = __uid__;
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.cameraId = __cameraId__;
            ____result.colorCameraId = __colorCameraId__;
            ____result.frameRate = __frameRate__;
            ____result.unambiguousRange = __unambiguousRange__;
            ____result.lensFacing = __lensFacing__;
            ____result.intrinsics = __intrinsics__;
            ____result.name = __name__;
            ____result.isDepthPrivate = __isDepthPrivate__;
            ____result.isFusion = __isFusion__;
            ____result.fusionSourceWidth = __fusionSourceWidth__;
            ____result.fusionSourceHeight = __fusionSourceHeight__;
            return ____result;
        }
    }


    public sealed class FunctionStreamParametersPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.FunctionStreamParametersProperty>
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
                { "isDepthSixteen", 3},
                { "width", 4},
                { "height", 5},
                { "fx", 6},
                { "fy", 7},
                { "cx", 8},
                { "cy", 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("functionPointer"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("useExternalFunctionStream"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("useExternalFunctionCallback"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isDepthSixteen"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fy"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cy"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.FunctionStreamParametersProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 10);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.functionPointer);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.useExternalFunctionStream);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.useExternalFunctionCallback);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isDepthSixteen);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fy);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cy);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.FunctionStreamParametersProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __isDepthSixteen__ = default(bool);
            var __width__ = default(int);
            var __height__ = default(int);
            var __fx__ = default(float);
            var __fy__ = default(float);
            var __cx__ = default(float);
            var __cy__ = default(float);

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
                        __isDepthSixteen__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 4:
                        __width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 6:
                        __fx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __fy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __cx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 9:
                        __cy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.FunctionStreamParametersProperty();
            ____result.functionPointer = __functionPointer__;
            ____result.useExternalFunctionStream = __useExternalFunctionStream__;
            ____result.useExternalFunctionCallback = __useExternalFunctionCallback__;
            ____result.isDepthSixteen = __isDepthSixteen__;
            ____result.width = __width__;
            ____result.height = __height__;
            ____result.fx = __fx__;
            ____result.fy = __fy__;
            ____result.cx = __cx__;
            ____result.cy = __cy__;
            return ____result;
        }
    }


    public sealed class FunctionStreamCallbackPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.FunctionStreamCallbackProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FunctionStreamCallbackPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "timestamp", 0},
                { "dataArray", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("timestamp"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("dataArray"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.FunctionStreamCallbackProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.timestamp);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.dataArray);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.FunctionStreamCallbackProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __dataArray__ = default(long);

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
                        __dataArray__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.FunctionStreamCallbackProperty();
            ____result.timestamp = __timestamp__;
            ____result.dataArray = __dataArray__;
            return ____result;
        }
    }


    public sealed class SharedStreamsPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.SharedStreamsProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.SharedStreamsProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Tof.SharedStreamsProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Tof.SharedStreamsProperty();
            ____result.isRestart = __isRestart__;
            return ____result;
        }
    }


    public sealed class ExposurePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Tof.ExposureProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ExposurePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "autoExposure", 0},
                { "minExposureTime", 1},
                { "maxExposureTime", 2},
                { "exposureTime", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("autoExposure"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minExposureTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("maxExposureTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposureTime"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Tof.ExposureProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.autoExposure);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.minExposureTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.maxExposureTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.exposureTime);
            return offset - startOffset;
        }

        public global::TofAr.V0.Tof.ExposureProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __minExposureTime__ = default(long);
            var __maxExposureTime__ = default(long);
            var __exposureTime__ = default(long);

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
                        __minExposureTime__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 2:
                        __maxExposureTime__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 3:
                        __exposureTime__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Tof.ExposureProperty();
            ____result.autoExposure = __autoExposure__;
            ____result.minExposureTime = __minExposureTime__;
            ____result.maxExposureTime = __maxExposureTime__;
            ____result.exposureTime = __exposureTime__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArHandResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArHandResolver();

        TofArHandResolver()
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
                var f = TofArHandResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArHandResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArHandResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(9)
            {
                {typeof(global::TofAr.V0.Hand.ProcessMode), 0 },
                {typeof(global::TofAr.V0.Hand.ProcessLevel), 1 },
                {typeof(global::TofAr.V0.Hand.RuntimeMode), 2 },
                {typeof(global::TofAr.V0.Hand.RecogMode), 3 },
                {typeof(global::TofAr.V0.Hand.RotCorrection), 4 },
                {typeof(global::TofAr.V0.Hand.NoiseReductionLevel), 5 },
                {typeof(global::TofAr.V0.Hand.CameraOrientation), 6 },
                {typeof(global::TofAr.V0.Hand.RecognizeConfigProperty), 7 },
                {typeof(global::TofAr.V0.Hand.CameraOrientationProperty), 8 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.TofAr.V0.Hand.ProcessModeFormatter();
                case 1: return new MessagePack.Formatters.TofAr.V0.Hand.ProcessLevelFormatter();
                case 2: return new MessagePack.Formatters.TofAr.V0.Hand.RuntimeModeFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Hand.RecogModeFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Hand.RotCorrectionFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Hand.NoiseReductionLevelFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Hand.CameraOrientationFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Hand.RecognizeConfigPropertyFormatter();
                case 8: return new MessagePack.Formatters.TofAr.V0.Hand.CameraOrientationPropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Hand
{
    using System;
    using MessagePack;

    public sealed class ProcessModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.ProcessMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.ProcessMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public global::TofAr.V0.Hand.ProcessMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Hand.ProcessMode)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class ProcessLevelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.ProcessLevel>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.ProcessLevel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public global::TofAr.V0.Hand.ProcessLevel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Hand.ProcessLevel)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class RuntimeModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.RuntimeMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.RuntimeMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public global::TofAr.V0.Hand.RuntimeMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Hand.RuntimeMode)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class RecogModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.RecogMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.RecogMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public global::TofAr.V0.Hand.RecogMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Hand.RecogMode)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class RotCorrectionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.RotCorrection>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.RotCorrection value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public global::TofAr.V0.Hand.RotCorrection Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Hand.RotCorrection)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class NoiseReductionLevelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.NoiseReductionLevel>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.NoiseReductionLevel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }

        public global::TofAr.V0.Hand.NoiseReductionLevel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Hand.NoiseReductionLevel)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class CameraOrientationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.CameraOrientation>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.CameraOrientation value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public global::TofAr.V0.Hand.CameraOrientation Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Hand.CameraOrientation)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Hand
{
    using System;
    using MessagePack;


    public sealed class RecognizeConfigPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.RecognizeConfigProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RecognizeConfigPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "processMode", 0},
                { "processLevel", 1},
                { "segmentRuntimeMode", 2},
                { "pointRuntimeMode", 3},
                { "imageWidth", 4},
                { "imageHeight", 5},
                { "horizontalFovDeg", 6},
                { "verticalFovDeg", 7},
                { "recogMode", 8},
                { "rotCorrection", 9},
                { "intervalFramesNotRecognized", 10},
                { "framesForDetectNoHands", 11},
                { "trackingMode", 12},
                { "temporalRecognitionMode", 13},
                { "isSetThreads", 14},
                { "regionThreads", 15},
                { "pointThreads", 16},
                { "noiseReductionLevel", 17},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("processMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("processLevel"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("segmentRuntimeMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pointRuntimeMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("imageWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("imageHeight"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("horizontalFovDeg"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("verticalFovDeg"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("recogMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rotCorrection"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("intervalFramesNotRecognized"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("framesForDetectNoHands"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("trackingMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("temporalRecognitionMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isSetThreads"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("regionThreads"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pointThreads"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("noiseReductionLevel"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.RecognizeConfigProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteMapHeader(ref bytes, offset, 18);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.ProcessMode>().Serialize(ref bytes, offset, value.processMode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.ProcessLevel>().Serialize(ref bytes, offset, value.processLevel, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RuntimeMode>().Serialize(ref bytes, offset, value.runtimeMode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RuntimeMode>().Serialize(ref bytes, offset, value.runtimeModeAfter, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.imageWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.imageHeight);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.horizontalFovDeg);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.verticalFovDeg);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RecogMode>().Serialize(ref bytes, offset, value.recogMode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RotCorrection>().Serialize(ref bytes, offset, value.rotCorrection, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.intervalFramesNotRecognized);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.framesForDetectNoHands);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.trackingMode);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[13]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.temporalRecognitionMode);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[14]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isSetThreads);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[15]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.regionThreads);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[16]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.pointThreads);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[17]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.NoiseReductionLevel>().Serialize(ref bytes, offset, value.noiseReductionLevel, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Hand.RecognizeConfigProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __processMode__ = default(global::TofAr.V0.Hand.ProcessMode);
            var __processLevel__ = default(global::TofAr.V0.Hand.ProcessLevel);
            var __runtimeMode__ = default(global::TofAr.V0.Hand.RuntimeMode);
            var __runtimeModeAfter__ = default(global::TofAr.V0.Hand.RuntimeMode);
            var __imageWidth__ = default(int);
            var __imageHeight__ = default(int);
            var __horizontalFovDeg__ = default(double);
            var __verticalFovDeg__ = default(double);
            var __recogMode__ = default(global::TofAr.V0.Hand.RecogMode);
            var __rotCorrection__ = default(global::TofAr.V0.Hand.RotCorrection);
            var __intervalFramesNotRecognized__ = default(int);
            var __framesForDetectNoHands__ = default(int);
            var __trackingMode__ = default(bool);
            var __temporalRecognitionMode__ = default(bool);
            var __isSetThreads__ = default(bool);
            var __regionThreads__ = default(int);
            var __pointThreads__ = default(int);
            var __noiseReductionLevel__ = default(global::TofAr.V0.Hand.NoiseReductionLevel);

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
                        __processMode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.ProcessMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __processLevel__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.ProcessLevel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __runtimeMode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RuntimeMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __runtimeModeAfter__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RuntimeMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __imageWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __imageHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 6:
                        __horizontalFovDeg__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    case 7:
                        __verticalFovDeg__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    case 8:
                        __recogMode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RecogMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 9:
                        __rotCorrection__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.RotCorrection>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __intervalFramesNotRecognized__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 11:
                        __framesForDetectNoHands__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 12:
                        __trackingMode__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 13:
                        __temporalRecognitionMode__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 14:
                        __isSetThreads__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 15:
                        __regionThreads__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 16:
                        __pointThreads__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 17:
                        __noiseReductionLevel__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.NoiseReductionLevel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Hand.RecognizeConfigProperty();
            ____result.processMode = __processMode__;
            ____result.processLevel = __processLevel__;
            ____result.runtimeMode = __runtimeMode__;
            ____result.runtimeModeAfter = __runtimeModeAfter__;
            ____result.imageWidth = __imageWidth__;
            ____result.imageHeight = __imageHeight__;
            ____result.horizontalFovDeg = __horizontalFovDeg__;
            ____result.verticalFovDeg = __verticalFovDeg__;
            ____result.recogMode = __recogMode__;
            ____result.rotCorrection = __rotCorrection__;
            ____result.intervalFramesNotRecognized = __intervalFramesNotRecognized__;
            ____result.framesForDetectNoHands = __framesForDetectNoHands__;
            ____result.trackingMode = __trackingMode__;
            ____result.temporalRecognitionMode = __temporalRecognitionMode__;
            ____result.isSetThreads = __isSetThreads__;
            ____result.regionThreads = __regionThreads__;
            ____result.pointThreads = __pointThreads__;
            ____result.noiseReductionLevel = __noiseReductionLevel__;
            return ____result;
        }
    }


    public sealed class CameraOrientationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Hand.CameraOrientationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraOrientationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "cameraOrientation", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cameraOrientation"),

            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Hand.CameraOrientationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.CameraOrientation>().Serialize(ref bytes, offset, value.cameraOrientation, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Hand.CameraOrientationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __cameraOrientation__ = default(global::TofAr.V0.Hand.CameraOrientation);

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
                        __cameraOrientation__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Hand.CameraOrientation>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }

            NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Hand.CameraOrientationProperty();
            ____result.cameraOrientation = __cameraOrientation__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class TofArBodyResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new TofArBodyResolver();

        TofArBodyResolver()
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
                var f = TofArBodyResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class TofArBodyResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static TofArBodyResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(20)
            {
                {typeof(global::TofAr.V0.Body.HumanBodyJoint[]), 0 },
                {typeof(global::TofAr.V0.Body.BodyResult[]), 1 },
                {typeof(global::TofAr.V0.Body.TrackingState), 2 },
                {typeof(global::TofAr.V0.Body.SV1.BodyShot), 3 },
                {typeof(global::TofAr.V0.Body.FrameDataSource), 4 },
                {typeof(global::TofAr.V0.Body.BodyPoseDetectorType), 5 },
                {typeof(global::TofAr.V0.Body.SV2.ProcessLevel), 6 },
                {typeof(global::TofAr.V0.Body.SV2.RuntimeMode), 7 },
                {typeof(global::TofAr.V0.Body.SV2.RecogMode), 8 },
                {typeof(global::TofAr.V0.Body.SV2.NoiseReductionLevel), 9 },
                {typeof(global::TofAr.V0.Body.SV2.CameraOrientation), 10 },
                {typeof(global::TofAr.V0.Body.TrackableId), 11 },
                {typeof(global::TofAr.V0.Body.Pose), 12 },
                {typeof(global::TofAr.V0.Body.HumanBodyJoint), 13 },
                {typeof(global::TofAr.V0.Body.BodyResult), 14 },
                {typeof(global::TofAr.V0.Body.BodyResults), 15 },
                {typeof(global::TofAr.V0.Body.ResultHandlerProperty), 16 },
                {typeof(global::TofAr.V0.Body.DetectorTypeProperty), 17 },
                {typeof(global::TofAr.V0.Body.SV2.RecognizeConfigProperty), 18 },
                {typeof(global::TofAr.V0.Body.SV2.CameraOrientationProperty), 19 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Body.HumanBodyJoint>();
                case 1: return new global::MessagePack.Formatters.ArrayFormatter<global::TofAr.V0.Body.BodyResult>();
                case 2: return new MessagePack.Formatters.TofAr.V0.Body.TrackingStateFormatter();
                case 3: return new MessagePack.Formatters.TofAr.V0.Body.SV1.BodyShotFormatter();
                case 4: return new MessagePack.Formatters.TofAr.V0.Body.FrameDataSourceFormatter();
                case 5: return new MessagePack.Formatters.TofAr.V0.Body.BodyPoseDetectorTypeFormatter();
                case 6: return new MessagePack.Formatters.TofAr.V0.Body.SV2.ProcessLevelFormatter();
                case 7: return new MessagePack.Formatters.TofAr.V0.Body.SV2.RuntimeModeFormatter();
                case 8: return new MessagePack.Formatters.TofAr.V0.Body.SV2.RecogModeFormatter();
                case 9: return new MessagePack.Formatters.TofAr.V0.Body.SV2.NoiseReductionLevelFormatter();
                case 10: return new MessagePack.Formatters.TofAr.V0.Body.SV2.CameraOrientationFormatter();
                case 11: return new MessagePack.Formatters.TofAr.V0.Body.TrackableIdFormatter();
                case 12: return new MessagePack.Formatters.TofAr.V0.Body.PoseFormatter();
                case 13: return new MessagePack.Formatters.TofAr.V0.Body.HumanBodyJointFormatter();
                case 14: return new MessagePack.Formatters.TofAr.V0.Body.BodyResultFormatter();
                case 15: return new MessagePack.Formatters.TofAr.V0.Body.BodyResultsFormatter();
                case 16: return new MessagePack.Formatters.TofAr.V0.Body.ResultHandlerPropertyFormatter();
                case 17: return new MessagePack.Formatters.TofAr.V0.Body.DetectorTypePropertyFormatter();
                case 18: return new MessagePack.Formatters.TofAr.V0.Body.SV2.RecognizeConfigPropertyFormatter();
                case 19: return new MessagePack.Formatters.TofAr.V0.Body.SV2.CameraOrientationPropertyFormatter();
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

namespace MessagePack.Formatters.TofAr.V0.Body
{
    using System;
    using MessagePack;

    public sealed class TrackingStateFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.TrackingState>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.TrackingState value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Body.TrackingState Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.TrackingState)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class FrameDataSourceFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.FrameDataSource>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.FrameDataSource value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Body.FrameDataSource Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.FrameDataSource)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class BodyPoseDetectorTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.BodyPoseDetectorType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.BodyPoseDetectorType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Body.BodyPoseDetectorType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.BodyPoseDetectorType)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Body.SV1
{
    using System;
    using MessagePack;

    public sealed class BodyShotFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV1.BodyShot>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV1.BodyShot value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Body.SV1.BodyShot Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.SV1.BodyShot)MessagePackBinary.ReadByte(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Body.SV2
{
    using System;
    using MessagePack;

    public sealed class ProcessLevelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV2.ProcessLevel>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV2.ProcessLevel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::TofAr.V0.Body.SV2.ProcessLevel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.SV2.ProcessLevel)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class RuntimeModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV2.RuntimeMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV2.RuntimeMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::TofAr.V0.Body.SV2.RuntimeMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.SV2.RuntimeMode)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class RecogModeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV2.RecogMode>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV2.RecogMode value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::TofAr.V0.Body.SV2.RecogMode Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.SV2.RecogMode)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class NoiseReductionLevelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV2.NoiseReductionLevel>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV2.NoiseReductionLevel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::TofAr.V0.Body.SV2.NoiseReductionLevel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.SV2.NoiseReductionLevel)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class CameraOrientationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV2.CameraOrientation>
    {
        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV2.CameraOrientation value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::TofAr.V0.Body.SV2.CameraOrientation Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::TofAr.V0.Body.SV2.CameraOrientation)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.TofAr.V0.Body
{
    using System;
    using MessagePack;


    public sealed class TrackableIdFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.TrackableId>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.TrackableId value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Body.TrackableId Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Body.TrackableId();
            ____result.subId1 = __subId1__;
            ____result.subId2 = __subId2__;
            return ____result;
        }
    }


    public sealed class PoseFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.Pose>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PoseFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "forward", 0},
                { "position", 1},
                { "right", 2},
                { "rotation", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("forward"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("right"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rotation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.Pose value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.forward, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.right, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArQuaternion>().Serialize(ref bytes, offset, value.rotation, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Body.Pose Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __forward__ = default(global::TofAr.V0.TofArVector3);
            var __position__ = default(global::TofAr.V0.TofArVector3);
            var __right__ = default(global::TofAr.V0.TofArVector3);
            var __rotation__ = default(global::TofAr.V0.TofArQuaternion);

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
                        __forward__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __position__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __right__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __rotation__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArQuaternion>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Body.Pose();
            ____result.forward = __forward__;
            ____result.position = __position__;
            ____result.right = __right__;
            ____result.rotation = __rotation__;
            return ____result;
        }
    }


    public sealed class HumanBodyJointFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.HumanBodyJoint>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public HumanBodyJointFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "index", 0},
                { "parentIndex", 1},
                { "localScale", 2},
                { "localPose", 3},
                { "anchorScale", 4},
                { "anchorPose", 5},
                { "tracked", 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("index"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("parentIndex"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("localScale"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("localPose"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("anchorScale"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("anchorPose"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("tracked"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.HumanBodyJoint value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 7);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.index);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.parentIndex);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.localScale, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.Pose>().Serialize(ref bytes, offset, value.localPose, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Serialize(ref bytes, offset, value.anchorScale, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.Pose>().Serialize(ref bytes, offset, value.anchorPose, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.tracked);
            return offset - startOffset;
        }

        public global::TofAr.V0.Body.HumanBodyJoint Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __index__ = default(int);
            var __parentIndex__ = default(int);
            var __localScale__ = default(global::TofAr.V0.TofArVector3);
            var __localPose__ = default(global::TofAr.V0.Body.Pose);
            var __anchorScale__ = default(global::TofAr.V0.TofArVector3);
            var __anchorPose__ = default(global::TofAr.V0.Body.Pose);
            var __tracked__ = default(bool);

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
                        __index__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __parentIndex__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __localScale__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __localPose__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.Pose>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __anchorScale__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.TofArVector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __anchorPose__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.Pose>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __tracked__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Body.HumanBodyJoint();
            ____result.index = __index__;
            ____result.parentIndex = __parentIndex__;
            ____result.localScale = __localScale__;
            ____result.localPose = __localPose__;
            ____result.anchorScale = __anchorScale__;
            ____result.anchorPose = __anchorPose__;
            ____result.tracked = __tracked__;
            return ____result;
        }
    }


    public sealed class BodyResultFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.BodyResult>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BodyResultFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "trackableId", 0},
                { "pose", 1},
                { "estimatedHeightScaleFactor", 2},
                { "trackingState", 3},
                { "nativePtr", 4},
                { "joints", 5},
                { "timestamp", 6},
                { "bodyShot", 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("trackableId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pose"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("estimatedHeightScaleFactor"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("trackingState"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("nativePtr"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("joints"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("timestamp"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("bodyShot"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.BodyResult value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 8);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.TrackableId>().Serialize(ref bytes, offset, value.trackableId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.Pose>().Serialize(ref bytes, offset, value.pose, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.estimatedHeightScaleFactor);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.TrackingState>().Serialize(ref bytes, offset, value.trackingState, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.nativePtr);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.HumanBodyJoint[]>().Serialize(ref bytes, offset, value.joints, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.timestamp);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV1.BodyShot>().Serialize(ref bytes, offset, value.bodyShot, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Body.BodyResult Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __trackableId__ = default(global::TofAr.V0.Body.TrackableId);
            var __pose__ = default(global::TofAr.V0.Body.Pose);
            var __estimatedHeightScaleFactor__ = default(float);
            var __trackingState__ = default(global::TofAr.V0.Body.TrackingState);
            var __nativePtr__ = default(ulong);
            var __joints__ = default(global::TofAr.V0.Body.HumanBodyJoint[]);
            var __timestamp__ = default(ulong);
            var __bodyShot__ = default(global::TofAr.V0.Body.SV1.BodyShot);

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
                        __trackableId__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.TrackableId>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __pose__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.Pose>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __estimatedHeightScaleFactor__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __trackingState__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.TrackingState>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __nativePtr__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 5:
                        __joints__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.HumanBodyJoint[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __timestamp__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 7:
                        __bodyShot__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV1.BodyShot>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Body.BodyResult();
            ____result.trackableId = __trackableId__;
            ____result.pose = __pose__;
            ____result.estimatedHeightScaleFactor = __estimatedHeightScaleFactor__;
            ____result.trackingState = __trackingState__;
            ____result.nativePtr = __nativePtr__;
            ____result.joints = __joints__;
            ____result.timestamp = __timestamp__;
            ____result.bodyShot = __bodyShot__;
            return ____result;
        }
    }


    public sealed class BodyResultsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.BodyResults>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BodyResultsFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "results", 0},
                { "frameDataSource", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("results"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frameDataSource"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.BodyResults value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.BodyResult[]>().Serialize(ref bytes, offset, value.results, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.FrameDataSource>().Serialize(ref bytes, offset, value.frameDataSource, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Body.BodyResults Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __results__ = default(global::TofAr.V0.Body.BodyResult[]);
            var __frameDataSource__ = default(global::TofAr.V0.Body.FrameDataSource);

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
                        __results__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.BodyResult[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __frameDataSource__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.FrameDataSource>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Body.BodyResults();
            ____result.results = __results__;
            ____result.frameDataSource = __frameDataSource__;
            return ____result;
        }
    }


    public sealed class ResultHandlerPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.ResultHandlerProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.ResultHandlerProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::TofAr.V0.Body.ResultHandlerProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::TofAr.V0.Body.ResultHandlerProperty();
            ____result.handler = __handler__;
            return ____result;
        }
    }


    public sealed class DetectorTypePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.DetectorTypeProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.DetectorTypeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.BodyPoseDetectorType>().Serialize(ref bytes, offset, value.detectorType, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Body.DetectorTypeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __detectorType__ = default(global::TofAr.V0.Body.BodyPoseDetectorType);

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
                        __detectorType__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.BodyPoseDetectorType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Body.DetectorTypeProperty();
            ____result.detectorType = __detectorType__;
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

namespace MessagePack.Formatters.TofAr.V0.Body.SV2
{
    using System;
    using MessagePack;


    public sealed class RecognizeConfigPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV2.RecognizeConfigProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RecognizeConfigPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "processLevel", 0},
                { "runtimeMode", 1},
                { "imageWidth", 2},
                { "imageHeight", 3},
                { "horizontalFovDeg", 4},
                { "verticalFovDeg", 5},
                { "recogMode", 6},
                { "isSetThreads", 7},
                { "nThreads", 8},
                { "noiseReductionLevel", 9},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("processLevel"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("runtimeMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("imageWidth"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("imageHeight"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("horizontalFovDeg"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("verticalFovDeg"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("recogMode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("isSetThreads"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("nThreads"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("noiseReductionLevel"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV2.RecognizeConfigProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 10);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.ProcessLevel>().Serialize(ref bytes, offset, value.processLevel, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.RuntimeMode>().Serialize(ref bytes, offset, value.runtimeMode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.imageWidth);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.imageHeight);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.horizontalFovDeg);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.verticalFovDeg);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.RecogMode>().Serialize(ref bytes, offset, value.recogMode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isSetThreads);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.nThreads);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.NoiseReductionLevel>().Serialize(ref bytes, offset, value.noiseReductionLevel, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Body.SV2.RecognizeConfigProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __processLevel__ = default(global::TofAr.V0.Body.SV2.ProcessLevel);
            var __runtimeMode__ = default(global::TofAr.V0.Body.SV2.RuntimeMode);
            var __imageWidth__ = default(int);
            var __imageHeight__ = default(int);
            var __horizontalFovDeg__ = default(double);
            var __verticalFovDeg__ = default(double);
            var __recogMode__ = default(global::TofAr.V0.Body.SV2.RecogMode);
            var __isSetThreads__ = default(bool);
            var __nThreads__ = default(int);
            var __noiseReductionLevel__ = default(global::TofAr.V0.Body.SV2.NoiseReductionLevel);

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
                        __processLevel__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.ProcessLevel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __runtimeMode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.RuntimeMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __imageWidth__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __imageHeight__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __horizontalFovDeg__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    case 5:
                        __verticalFovDeg__ = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                        break;
                    case 6:
                        __recogMode__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.RecogMode>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 7:
                        __isSetThreads__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 8:
                        __nThreads__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 9:
                        __noiseReductionLevel__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.NoiseReductionLevel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Body.SV2.RecognizeConfigProperty();
            ____result.processLevel = __processLevel__;
            ____result.runtimeMode = __runtimeMode__;
            ____result.imageWidth = __imageWidth__;
            ____result.imageHeight = __imageHeight__;
            ____result.horizontalFovDeg = __horizontalFovDeg__;
            ____result.verticalFovDeg = __verticalFovDeg__;
            ____result.recogMode = __recogMode__;
            ____result.isSetThreads = __isSetThreads__;
            ____result.nThreads = __nThreads__;
            ____result.noiseReductionLevel = __noiseReductionLevel__;
            return ____result;
        }
    }


    public sealed class CameraOrientationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::TofAr.V0.Body.SV2.CameraOrientationProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::TofAr.V0.Body.SV2.CameraOrientationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.CameraOrientation>().Serialize(ref bytes, offset, value.cameraOrientation, formatterResolver);
            return offset - startOffset;
        }

        public global::TofAr.V0.Body.SV2.CameraOrientationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __cameraOrientation__ = default(global::TofAr.V0.Body.SV2.CameraOrientation);

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
                        __cameraOrientation__ = formatterResolver.GetFormatterWithVerify<global::TofAr.V0.Body.SV2.CameraOrientation>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::TofAr.V0.Body.SV2.CameraOrientationProperty();
            ____result.cameraOrientation = __cameraOrientation__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

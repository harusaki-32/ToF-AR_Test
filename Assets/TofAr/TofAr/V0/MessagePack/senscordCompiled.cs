#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class SensCordResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new SensCordResolver();

        SensCordResolver()
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
                var f = SensCordResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class SensCordResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static SensCordResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(106)
            {
                {typeof(global::System.Collections.Generic.Dictionary<long, global::SensCord.CameraCalibrationParameters>), 0 },
                {typeof(global::System.Collections.Generic.Dictionary<long, global::SensCord.ChannelInfo>), 1 },
                {typeof(global::System.Collections.Generic.List<long>), 2 },
                {typeof(global::System.Collections.Generic.Dictionary<long, string>), 3 },
                {typeof(global::System.Collections.Generic.List<string>), 4 },
                {typeof(global::System.Collections.Generic.Dictionary<int, global::SensCord.TemperatureInfo>), 5 },
                {typeof(global::System.Collections.Generic.List<global::SensCord.KeyPoint>), 6 },
                {typeof(global::System.Collections.Generic.List<global::SensCord.DetectedKeyPointInformation>), 7 },
                {typeof(global::System.Collections.Generic.List<global::SensCord.DetectedObjectInformation>), 8 },
                {typeof(global::System.Collections.Generic.List<global::SensCord.TrackedObjectInformation>), 9 },
                {typeof(global::SensCord.YCbCrEncoding), 10 },
                {typeof(global::SensCord.YCbCrQuantization), 11 },
                {typeof(global::SensCord.Buffering), 12 },
                {typeof(global::SensCord.BufferingFormat), 13 },
                {typeof(global::SensCord.GridUnit), 14 },
                {typeof(global::SensCord.AccelerationUnit), 15 },
                {typeof(global::SensCord.AngularVelocityUnit), 16 },
                {typeof(global::SensCord.MagneticFieldUnit), 17 },
                {typeof(global::SensCord.OrientationUnit), 18 },
                {typeof(global::SensCord.InterlaceField), 19 },
                {typeof(global::SensCord.InterlaceOrder), 20 },
                {typeof(global::SensCord.CoordinateSystem), 21 },
                {typeof(global::SensCord.PixelPolarityTriggerType), 22 },
                {typeof(global::SensCord.PlaySpeed), 23 },
                {typeof(global::SensCord.StreamState), 24 },
                {typeof(global::SensCord.VelocityUnit), 25 },
                {typeof(global::SensCord.AccelerometerRangeProperty), 26 },
                {typeof(global::SensCord.AccelerationCalibProperty), 27 },
                {typeof(global::SensCord.BaselineLengthProperty), 28 },
                {typeof(global::SensCord.IntrinsicCalibrationParameter), 29 },
                {typeof(global::SensCord.DistortionCalibrationParameter), 30 },
                {typeof(global::SensCord.ExtrinsicCalibrationParameter), 31 },
                {typeof(global::SensCord.CameraCalibrationParameters), 32 },
                {typeof(global::SensCord.CameraCalibrationProperty), 33 },
                {typeof(global::SensCord.ChannelInfo), 34 },
                {typeof(global::SensCord.ChannelInfoProperty), 35 },
                {typeof(global::SensCord.ChannelMaskProperty), 36 },
                {typeof(global::SensCord.ColorSpaceProperty), 37 },
                {typeof(global::SensCord.AxisMisalignment), 38 },
                {typeof(global::SensCord.ConfidenceProperty), 39 },
                {typeof(global::SensCord.CurrentFrameNumProperty), 40 },
                {typeof(global::SensCord.DepthProperty), 41 },
                {typeof(global::SensCord.RectangleRegionParameter), 42 },
                {typeof(global::SensCord.ExposureProperty), 43 },
                {typeof(global::SensCord.FrameBufferingProperty), 44 },
                {typeof(global::SensCord.Itof.TriggerModeOptions), 45 },
                {typeof(global::SensCord.Itof.MetaDataExtension), 46 },
                {typeof(global::SensCord.Itof.FrameExtensionProperty), 47 },
                {typeof(global::SensCord.FrameRateProperty), 48 },
                {typeof(global::SensCord.GridSize), 49 },
                {typeof(global::SensCord.GridMapProperty), 50 },
                {typeof(global::SensCord.GridSizeProperty), 51 },
                {typeof(global::SensCord.GyrometerRangeProperty), 52 },
                {typeof(global::SensCord.AngularVelocityCalibProperty), 53 },
                {typeof(global::SensCord.ImageCropProperty), 54 },
                {typeof(global::SensCord.ImageProperty), 55 },
                {typeof(global::SensCord.ImageSensorFunctionProperty), 56 },
                {typeof(global::SensCord.ImageSensorFunctionSupportedProperty), 57 },
                {typeof(global::SensCord.ImuDataUnitProperty), 58 },
                {typeof(global::SensCord.InitialPoseProperty), 59 },
                {typeof(global::SensCord.InterlaceProperty), 60 },
                {typeof(global::SensCord.InterlaceInfoProperty), 61 },
                {typeof(global::SensCord.LensProperty), 62 },
                {typeof(global::SensCord.MagnetometerRangeProperty), 63 },
                {typeof(global::SensCord.MagnetometerRange3Property), 64 },
                {typeof(global::SensCord.MagneticFieldCalibProperty), 65 },
                {typeof(global::SensCord.MagneticNorthCalibProperty), 66 },
                {typeof(global::SensCord.Itof.ModuleInformationProperty), 67 },
                {typeof(global::SensCord.OdometryDataProperty), 68 },
                {typeof(global::SensCord.PixelPolarityDataProperty), 69 },
                {typeof(global::SensCord.PlayModeProperty), 70 },
                {typeof(global::SensCord.PlayProperty), 71 },
                {typeof(global::SensCord.PointCloudProperty), 72 },
                {typeof(global::SensCord.PoseDataProperty), 73 },
                {typeof(global::SensCord.PresetListProperty), 74 },
                {typeof(global::SensCord.PresetProperty), 75 },
                {typeof(global::SensCord.RecordProperty), 76 },
                {typeof(global::SensCord.RecorderListProperty), 77 },
                {typeof(global::SensCord.RoiProperty), 78 },
                {typeof(global::SensCord.SamplingFrequencyProperty), 79 },
                {typeof(global::SensCord.ScoreThresholdProperty), 80 },
                {typeof(global::SensCord.SkipFrameProperty), 81 },
                {typeof(global::SensCord.SlamDataSupportedProperty), 82 },
                {typeof(global::SensCord.StreamTypeProperty), 83 },
                {typeof(global::SensCord.StreamKeyProperty), 84 },
                {typeof(global::SensCord.StreamStateProperty), 85 },
                {typeof(global::SensCord.TemperatureInfo), 86 },
                {typeof(global::SensCord.TemperatureProperty), 87 },
                {typeof(global::SensCord.UserDataProperty), 88 },
                {typeof(global::SensCord.VelocityDataUnitProperty), 89 },
                {typeof(global::SensCord.VersionProperty), 90 },
                {typeof(global::SensCord.WhiteBalanceProperty), 91 },
                {typeof(global::SensCord.AccelerationData), 92 },
                {typeof(global::SensCord.AngularVelocityData), 93 },
                {typeof(global::SensCord.MagneticFieldData), 94 },
                {typeof(global::SensCord.KeyPoint), 95 },
                {typeof(global::SensCord.DetectedKeyPointInformation), 96 },
                {typeof(global::SensCord.KeyPointData), 97 },
                {typeof(global::SensCord.DetectedObjectInformation), 98 },
                {typeof(global::SensCord.ObjectDetectionData), 99 },
                {typeof(global::SensCord.TrackedObjectInformation), 100 },
                {typeof(global::SensCord.ObjectTrackingData), 101 },
                {typeof(global::SensCord.PoseQuaternionData), 102 },
                {typeof(global::SensCord.PoseData), 103 },
                {typeof(global::SensCord.PoseMatrixData), 104 },
                {typeof(global::SensCord.RotationData), 105 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.DictionaryFormatter<long, global::SensCord.CameraCalibrationParameters>();
                case 1: return new global::MessagePack.Formatters.DictionaryFormatter<long, global::SensCord.ChannelInfo>();
                case 2: return new global::MessagePack.Formatters.ListFormatter<long>();
                case 3: return new global::MessagePack.Formatters.DictionaryFormatter<long, string>();
                case 4: return new global::MessagePack.Formatters.ListFormatter<string>();
                case 5: return new global::MessagePack.Formatters.DictionaryFormatter<int, global::SensCord.TemperatureInfo>();
                case 6: return new global::MessagePack.Formatters.ListFormatter<global::SensCord.KeyPoint>();
                case 7: return new global::MessagePack.Formatters.ListFormatter<global::SensCord.DetectedKeyPointInformation>();
                case 8: return new global::MessagePack.Formatters.ListFormatter<global::SensCord.DetectedObjectInformation>();
                case 9: return new global::MessagePack.Formatters.ListFormatter<global::SensCord.TrackedObjectInformation>();
                case 10: return new MessagePack.Formatters.SensCord.YCbCrEncodingFormatter();
                case 11: return new MessagePack.Formatters.SensCord.YCbCrQuantizationFormatter();
                case 12: return new MessagePack.Formatters.SensCord.BufferingFormatter();
                case 13: return new MessagePack.Formatters.SensCord.BufferingFormatFormatter();
                case 14: return new MessagePack.Formatters.SensCord.GridUnitFormatter();
                case 15: return new MessagePack.Formatters.SensCord.AccelerationUnitFormatter();
                case 16: return new MessagePack.Formatters.SensCord.AngularVelocityUnitFormatter();
                case 17: return new MessagePack.Formatters.SensCord.MagneticFieldUnitFormatter();
                case 18: return new MessagePack.Formatters.SensCord.OrientationUnitFormatter();
                case 19: return new MessagePack.Formatters.SensCord.InterlaceFieldFormatter();
                case 20: return new MessagePack.Formatters.SensCord.InterlaceOrderFormatter();
                case 21: return new MessagePack.Formatters.SensCord.CoordinateSystemFormatter();
                case 22: return new MessagePack.Formatters.SensCord.PixelPolarityTriggerTypeFormatter();
                case 23: return new MessagePack.Formatters.SensCord.PlaySpeedFormatter();
                case 24: return new MessagePack.Formatters.SensCord.StreamStateFormatter();
                case 25: return new MessagePack.Formatters.SensCord.VelocityUnitFormatter();
                case 26: return new MessagePack.Formatters.SensCord.AccelerometerRangePropertyFormatter();
                case 27: return new MessagePack.Formatters.SensCord.AccelerationCalibPropertyFormatter();
                case 28: return new MessagePack.Formatters.SensCord.BaselineLengthPropertyFormatter();
                case 29: return new MessagePack.Formatters.SensCord.IntrinsicCalibrationParameterFormatter();
                case 30: return new MessagePack.Formatters.SensCord.DistortionCalibrationParameterFormatter();
                case 31: return new MessagePack.Formatters.SensCord.ExtrinsicCalibrationParameterFormatter();
                case 32: return new MessagePack.Formatters.SensCord.CameraCalibrationParametersFormatter();
                case 33: return new MessagePack.Formatters.SensCord.CameraCalibrationPropertyFormatter();
                case 34: return new MessagePack.Formatters.SensCord.ChannelInfoFormatter();
                case 35: return new MessagePack.Formatters.SensCord.ChannelInfoPropertyFormatter();
                case 36: return new MessagePack.Formatters.SensCord.ChannelMaskPropertyFormatter();
                case 37: return new MessagePack.Formatters.SensCord.ColorSpacePropertyFormatter();
                case 38: return new MessagePack.Formatters.SensCord.AxisMisalignmentFormatter();
                case 39: return new MessagePack.Formatters.SensCord.ConfidencePropertyFormatter();
                case 40: return new MessagePack.Formatters.SensCord.CurrentFrameNumPropertyFormatter();
                case 41: return new MessagePack.Formatters.SensCord.DepthPropertyFormatter();
                case 42: return new MessagePack.Formatters.SensCord.RectangleRegionParameterFormatter();
                case 43: return new MessagePack.Formatters.SensCord.ExposurePropertyFormatter();
                case 44: return new MessagePack.Formatters.SensCord.FrameBufferingPropertyFormatter();
                case 45: return new MessagePack.Formatters.SensCord.Itof.TriggerModeOptionsFormatter();
                case 46: return new MessagePack.Formatters.SensCord.Itof.MetaDataExtensionFormatter();
                case 47: return new MessagePack.Formatters.SensCord.Itof.FrameExtensionPropertyFormatter();
                case 48: return new MessagePack.Formatters.SensCord.FrameRatePropertyFormatter();
                case 49: return new MessagePack.Formatters.SensCord.GridSizeFormatter();
                case 50: return new MessagePack.Formatters.SensCord.GridMapPropertyFormatter();
                case 51: return new MessagePack.Formatters.SensCord.GridSizePropertyFormatter();
                case 52: return new MessagePack.Formatters.SensCord.GyrometerRangePropertyFormatter();
                case 53: return new MessagePack.Formatters.SensCord.AngularVelocityCalibPropertyFormatter();
                case 54: return new MessagePack.Formatters.SensCord.ImageCropPropertyFormatter();
                case 55: return new MessagePack.Formatters.SensCord.ImagePropertyFormatter();
                case 56: return new MessagePack.Formatters.SensCord.ImageSensorFunctionPropertyFormatter();
                case 57: return new MessagePack.Formatters.SensCord.ImageSensorFunctionSupportedPropertyFormatter();
                case 58: return new MessagePack.Formatters.SensCord.ImuDataUnitPropertyFormatter();
                case 59: return new MessagePack.Formatters.SensCord.InitialPosePropertyFormatter();
                case 60: return new MessagePack.Formatters.SensCord.InterlacePropertyFormatter();
                case 61: return new MessagePack.Formatters.SensCord.InterlaceInfoPropertyFormatter();
                case 62: return new MessagePack.Formatters.SensCord.LensPropertyFormatter();
                case 63: return new MessagePack.Formatters.SensCord.MagnetometerRangePropertyFormatter();
                case 64: return new MessagePack.Formatters.SensCord.MagnetometerRange3PropertyFormatter();
                case 65: return new MessagePack.Formatters.SensCord.MagneticFieldCalibPropertyFormatter();
                case 66: return new MessagePack.Formatters.SensCord.MagneticNorthCalibPropertyFormatter();
                case 67: return new MessagePack.Formatters.SensCord.Itof.ModuleInformationPropertyFormatter();
                case 68: return new MessagePack.Formatters.SensCord.OdometryDataPropertyFormatter();
                case 69: return new MessagePack.Formatters.SensCord.PixelPolarityDataPropertyFormatter();
                case 70: return new MessagePack.Formatters.SensCord.PlayModePropertyFormatter();
                case 71: return new MessagePack.Formatters.SensCord.PlayPropertyFormatter();
                case 72: return new MessagePack.Formatters.SensCord.PointCloudPropertyFormatter();
                case 73: return new MessagePack.Formatters.SensCord.PoseDataPropertyFormatter();
                case 74: return new MessagePack.Formatters.SensCord.PresetListPropertyFormatter();
                case 75: return new MessagePack.Formatters.SensCord.PresetPropertyFormatter();
                case 76: return new MessagePack.Formatters.SensCord.RecordPropertyFormatter();
                case 77: return new MessagePack.Formatters.SensCord.RecorderListPropertyFormatter();
                case 78: return new MessagePack.Formatters.SensCord.RoiPropertyFormatter();
                case 79: return new MessagePack.Formatters.SensCord.SamplingFrequencyPropertyFormatter();
                case 80: return new MessagePack.Formatters.SensCord.ScoreThresholdPropertyFormatter();
                case 81: return new MessagePack.Formatters.SensCord.SkipFramePropertyFormatter();
                case 82: return new MessagePack.Formatters.SensCord.SlamDataSupportedPropertyFormatter();
                case 83: return new MessagePack.Formatters.SensCord.StreamTypePropertyFormatter();
                case 84: return new MessagePack.Formatters.SensCord.StreamKeyPropertyFormatter();
                case 85: return new MessagePack.Formatters.SensCord.StreamStatePropertyFormatter();
                case 86: return new MessagePack.Formatters.SensCord.TemperatureInfoFormatter();
                case 87: return new MessagePack.Formatters.SensCord.TemperaturePropertyFormatter();
                case 88: return new MessagePack.Formatters.SensCord.UserDataPropertyFormatter();
                case 89: return new MessagePack.Formatters.SensCord.VelocityDataUnitPropertyFormatter();
                case 90: return new MessagePack.Formatters.SensCord.VersionPropertyFormatter();
                case 91: return new MessagePack.Formatters.SensCord.WhiteBalancePropertyFormatter();
                case 92: return new MessagePack.Formatters.SensCord.AccelerationDataFormatter();
                case 93: return new MessagePack.Formatters.SensCord.AngularVelocityDataFormatter();
                case 94: return new MessagePack.Formatters.SensCord.MagneticFieldDataFormatter();
                case 95: return new MessagePack.Formatters.SensCord.KeyPointFormatter();
                case 96: return new MessagePack.Formatters.SensCord.DetectedKeyPointInformationFormatter();
                case 97: return new MessagePack.Formatters.SensCord.KeyPointDataFormatter();
                case 98: return new MessagePack.Formatters.SensCord.DetectedObjectInformationFormatter();
                case 99: return new MessagePack.Formatters.SensCord.ObjectDetectionDataFormatter();
                case 100: return new MessagePack.Formatters.SensCord.TrackedObjectInformationFormatter();
                case 101: return new MessagePack.Formatters.SensCord.ObjectTrackingDataFormatter();
                case 102: return new MessagePack.Formatters.SensCord.PoseQuaternionDataFormatter();
                case 103: return new MessagePack.Formatters.SensCord.PoseDataFormatter();
                case 104: return new MessagePack.Formatters.SensCord.PoseMatrixDataFormatter();
                case 105: return new MessagePack.Formatters.SensCord.RotationDataFormatter();
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

namespace MessagePack.Formatters.SensCord
{
    using System;
    using MessagePack;

    public sealed class YCbCrEncodingFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.YCbCrEncoding>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.YCbCrEncoding value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.YCbCrEncoding Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.YCbCrEncoding)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class YCbCrQuantizationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.YCbCrQuantization>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.YCbCrQuantization value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.YCbCrQuantization Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.YCbCrQuantization)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class BufferingFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.Buffering>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.Buffering value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.Buffering Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.Buffering)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class BufferingFormatFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.BufferingFormat>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.BufferingFormat value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.BufferingFormat Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.BufferingFormat)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class GridUnitFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.GridUnit>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.GridUnit value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.GridUnit Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.GridUnit)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class AccelerationUnitFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AccelerationUnit>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AccelerationUnit value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.AccelerationUnit Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.AccelerationUnit)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class AngularVelocityUnitFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AngularVelocityUnit>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AngularVelocityUnit value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.AngularVelocityUnit Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.AngularVelocityUnit)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class MagneticFieldUnitFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.MagneticFieldUnit>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.MagneticFieldUnit value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.MagneticFieldUnit Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.MagneticFieldUnit)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class OrientationUnitFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.OrientationUnit>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.OrientationUnit value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.OrientationUnit Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.OrientationUnit)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class InterlaceFieldFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.InterlaceField>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.InterlaceField value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.InterlaceField Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.InterlaceField)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class InterlaceOrderFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.InterlaceOrder>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.InterlaceOrder value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.InterlaceOrder Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.InterlaceOrder)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class CoordinateSystemFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.CoordinateSystem>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.CoordinateSystem value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.CoordinateSystem Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.CoordinateSystem)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class PixelPolarityTriggerTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PixelPolarityTriggerType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PixelPolarityTriggerType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.PixelPolarityTriggerType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.PixelPolarityTriggerType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class PlaySpeedFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PlaySpeed>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PlaySpeed value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.PlaySpeed Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.PlaySpeed)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class StreamStateFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.StreamState>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.StreamState value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.StreamState Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.StreamState)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class VelocityUnitFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.VelocityUnit>
    {
        public int Serialize(ref byte[] bytes, int offset, global::SensCord.VelocityUnit value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::SensCord.VelocityUnit Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::SensCord.VelocityUnit)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.SensCord
{
    using System;
    using MessagePack;


    public sealed class AccelerometerRangePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AccelerometerRangeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AccelerometerRangePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "value", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("value"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AccelerometerRangeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.value);
            return offset - startOffset;
        }

        public global::SensCord.AccelerometerRangeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __value__ = default(float);

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
                        __value__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.AccelerometerRangeProperty();
            ____result.value = __value__;
            return ____result;
        }
    }


    public sealed class AccelerationCalibPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AccelerationCalibProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AccelerationCalibPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "ms", 0},
                { "offset", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ms"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("offset"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AccelerationCalibProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Serialize(ref bytes, offset, value.MS, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Offset, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.AccelerationCalibProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MS__ = default(global::SensCord.Matrix3x3<float>);
            var __Offset__ = default(global::SensCord.Vector3<float>);

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
                        __MS__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Offset__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.AccelerationCalibProperty();
            ____result.MS = __MS__;
            ____result.Offset = __Offset__;
            return ____result;
        }
    }


    public sealed class BaselineLengthPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.BaselineLengthProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BaselineLengthPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "length_mm", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("length_mm"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.BaselineLengthProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Length);
            return offset - startOffset;
        }

        public global::SensCord.BaselineLengthProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Length__ = default(float);

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
                        __Length__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.BaselineLengthProperty();
            ____result.Length = __Length__;
            return ____result;
        }
    }


    public sealed class IntrinsicCalibrationParameterFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.IntrinsicCalibrationParameter>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public IntrinsicCalibrationParameterFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "cx", 0},
                { "cy", 1},
                { "fx", 2},
                { "fy", 3},
                { "s", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("cy"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fx"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("fy"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("s"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.IntrinsicCalibrationParameter value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.cy);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fx);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.fy);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.s);
            return offset - startOffset;
        }

        public global::SensCord.IntrinsicCalibrationParameter Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __cx__ = default(float);
            var __cy__ = default(float);
            var __fx__ = default(float);
            var __fy__ = default(float);
            var __s__ = default(float);

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
                        __cx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __cy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __fx__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __fy__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 4:
                        __s__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.IntrinsicCalibrationParameter();
            ____result.cx = __cx__;
            ____result.cy = __cy__;
            ____result.fx = __fx__;
            ____result.fy = __fy__;
            ____result.s = __s__;
            return ____result;
        }
    }


    public sealed class DistortionCalibrationParameterFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.DistortionCalibrationParameter>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DistortionCalibrationParameterFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "k1", 0},
                { "k2", 1},
                { "k3", 2},
                { "k4", 3},
                { "k5", 4},
                { "k6", 5},
                { "p1", 6},
                { "p2", 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k1"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k2"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k3"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k4"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k5"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("k6"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("p1"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("p2"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.DistortionCalibrationParameter value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 8);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.k6);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.p1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.p2);
            return offset - startOffset;
        }

        public global::SensCord.DistortionCalibrationParameter Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __k1__ = default(float);
            var __k2__ = default(float);
            var __k3__ = default(float);
            var __k4__ = default(float);
            var __k5__ = default(float);
            var __k6__ = default(float);
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
                        __k1__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __k2__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __k3__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __k4__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 4:
                        __k5__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __k6__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 6:
                        __p1__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
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

            var ____result = new global::SensCord.DistortionCalibrationParameter();
            ____result.k1 = __k1__;
            ____result.k2 = __k2__;
            ____result.k3 = __k3__;
            ____result.k4 = __k4__;
            ____result.k5 = __k5__;
            ____result.k6 = __k6__;
            ____result.p1 = __p1__;
            ____result.p2 = __p2__;
            return ____result;
        }
    }


    public sealed class ExtrinsicCalibrationParameterFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ExtrinsicCalibrationParameter>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ExtrinsicCalibrationParameterFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "p", 0},
                { "r11", 1},
                { "r12", 2},
                { "r13", 3},
                { "r21", 4},
                { "r22", 5},
                { "r23", 6},
                { "r31", 7},
                { "r32", 8},
                { "r33", 9},
                { "t1", 10},
                { "t2", 11},
                { "t3", 12},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("p"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r11"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r12"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r13"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r21"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r22"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r23"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r31"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r32"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("r33"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("t1"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("t2"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("t3"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ExtrinsicCalibrationParameter value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 13);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x4<float>>().Serialize(ref bytes, offset, value.p, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r11);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r12);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r13);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r21);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r22);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r23);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r31);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r32);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.r33);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.t1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.t2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.t3);
            return offset - startOffset;
        }

        public global::SensCord.ExtrinsicCalibrationParameter Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __p__ = default(global::SensCord.Matrix3x4<float>);
            var __r11__ = default(float);
            var __r12__ = default(float);
            var __r13__ = default(float);
            var __r21__ = default(float);
            var __r22__ = default(float);
            var __r23__ = default(float);
            var __r31__ = default(float);
            var __r32__ = default(float);
            var __r33__ = default(float);
            var __t1__ = default(float);
            var __t2__ = default(float);
            var __t3__ = default(float);

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
                        __p__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x4<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __r11__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __r12__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __r13__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 4:
                        __r21__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 5:
                        __r22__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 6:
                        __r23__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __r31__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 8:
                        __r32__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 9:
                        __r33__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 10:
                        __t1__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 11:
                        __t2__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 12:
                        __t3__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ExtrinsicCalibrationParameter();
            ____result.p = __p__;
            ____result.r11 = __r11__;
            ____result.r12 = __r12__;
            ____result.r13 = __r13__;
            ____result.r21 = __r21__;
            ____result.r22 = __r22__;
            ____result.r23 = __r23__;
            ____result.r31 = __r31__;
            ____result.r32 = __r32__;
            ____result.r33 = __r33__;
            ____result.t1 = __t1__;
            ____result.t2 = __t2__;
            ____result.t3 = __t3__;
            return ____result;
        }
    }


    public sealed class CameraCalibrationParametersFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.CameraCalibrationParameters>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraCalibrationParametersFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "intrinsic", 0},
                { "distortion", 1},
                { "extrinsic", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("intrinsic"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("distortion"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("extrinsic"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.CameraCalibrationParameters value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.IntrinsicCalibrationParameter>().Serialize(ref bytes, offset, value.Intrinsic, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.DistortionCalibrationParameter>().Serialize(ref bytes, offset, value.Distortion, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.ExtrinsicCalibrationParameter>().Serialize(ref bytes, offset, value.Extrinsic, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.CameraCalibrationParameters Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Intrinsic__ = default(global::SensCord.IntrinsicCalibrationParameter);
            var __Distortion__ = default(global::SensCord.DistortionCalibrationParameter);
            var __Extrinsic__ = default(global::SensCord.ExtrinsicCalibrationParameter);

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
                        __Intrinsic__ = formatterResolver.GetFormatterWithVerify<global::SensCord.IntrinsicCalibrationParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Distortion__ = formatterResolver.GetFormatterWithVerify<global::SensCord.DistortionCalibrationParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Extrinsic__ = formatterResolver.GetFormatterWithVerify<global::SensCord.ExtrinsicCalibrationParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.CameraCalibrationParameters();
            ____result.Intrinsic = __Intrinsic__;
            ____result.Distortion = __Distortion__;
            ____result.Extrinsic = __Extrinsic__;
            return ____result;
        }
    }


    public sealed class CameraCalibrationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.CameraCalibrationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CameraCalibrationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "parameters", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("parameters"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.CameraCalibrationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, global::SensCord.CameraCalibrationParameters>>().Serialize(ref bytes, offset, value.Parameters, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.CameraCalibrationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Parameters__ = default(global::System.Collections.Generic.Dictionary<long, global::SensCord.CameraCalibrationParameters>);

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
                        __Parameters__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, global::SensCord.CameraCalibrationParameters>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.CameraCalibrationProperty();
            ____result.Parameters = __Parameters__;
            return ____result;
        }
    }


    public sealed class ChannelInfoFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ChannelInfo>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ChannelInfoFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "raw_data_type", 0},
                { "description", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("raw_data_type"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("description"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ChannelInfo value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.RawDataType, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Description, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ChannelInfo Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __RawDataType__ = default(string);
            var __Description__ = default(string);

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
                        __RawDataType__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Description__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ChannelInfo();
            ____result.RawDataType = __RawDataType__;
            ____result.Description = __Description__;
            return ____result;
        }
    }


    public sealed class ChannelInfoPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ChannelInfoProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ChannelInfoPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "channels", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("channels"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ChannelInfoProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, global::SensCord.ChannelInfo>>().Serialize(ref bytes, offset, value.Channels, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ChannelInfoProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Channels__ = default(global::System.Collections.Generic.Dictionary<long, global::SensCord.ChannelInfo>);

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
                        __Channels__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, global::SensCord.ChannelInfo>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ChannelInfoProperty();
            ____result.Channels = __Channels__;
            return ____result;
        }
    }


    public sealed class ChannelMaskPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ChannelMaskProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ChannelMaskPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "channels", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("channels"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ChannelMaskProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<long>>().Serialize(ref bytes, offset, value.Channels, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ChannelMaskProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Channels__ = default(global::System.Collections.Generic.List<long>);

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
                        __Channels__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<long>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ChannelMaskProperty();
            ____result.Channels = __Channels__;
            return ____result;
        }
    }


    public sealed class ColorSpacePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ColorSpaceProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ColorSpacePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "encoding", 0},
                { "quantization", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("encoding"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("quantization"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ColorSpaceProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.YCbCrEncoding>().Serialize(ref bytes, offset, value.Encoding, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.YCbCrQuantization>().Serialize(ref bytes, offset, value.Quantization, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ColorSpaceProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Encoding__ = default(global::SensCord.YCbCrEncoding);
            var __Quantization__ = default(global::SensCord.YCbCrQuantization);

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
                        __Encoding__ = formatterResolver.GetFormatterWithVerify<global::SensCord.YCbCrEncoding>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Quantization__ = formatterResolver.GetFormatterWithVerify<global::SensCord.YCbCrQuantization>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ColorSpaceProperty();
            ____result.Encoding = __Encoding__;
            ____result.Quantization = __Quantization__;
            return ____result;
        }
    }


    public sealed class AxisMisalignmentFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AxisMisalignment>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AxisMisalignmentFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "ms", 0},
                { "offset", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ms"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("offset"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AxisMisalignment value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Serialize(ref bytes, offset, value.MS, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Offset, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.AxisMisalignment Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MS__ = default(global::SensCord.Matrix3x3<float>);
            var __Offset__ = default(global::SensCord.Vector3<float>);

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
                        __MS__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Offset__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.AxisMisalignment();
            ____result.MS = __MS__;
            ____result.Offset = __Offset__;
            return ____result;
        }
    }


    public sealed class ConfidencePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ConfidenceProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ConfidencePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "width", 0},
                { "height", 1},
                { "stride_bytes", 2},
                { "pixel_format", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("stride_bytes"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pixel_format"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ConfidenceProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.StrideBytes);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.PixelFormat, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ConfidenceProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Width__ = default(int);
            var __Height__ = default(int);
            var __StrideBytes__ = default(int);
            var __PixelFormat__ = default(string);

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
                        __Width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __StrideBytes__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __PixelFormat__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ConfidenceProperty();
            ____result.Width = __Width__;
            ____result.Height = __Height__;
            ____result.StrideBytes = __StrideBytes__;
            ____result.PixelFormat = __PixelFormat__;
            return ____result;
        }
    }


    public sealed class CurrentFrameNumPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.CurrentFrameNumProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CurrentFrameNumPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "arrived_number", 0},
                { "received_number", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("arrived_number"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("received_number"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.CurrentFrameNumProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ArrivedNumber);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ReceivedNumber);
            return offset - startOffset;
        }

        public global::SensCord.CurrentFrameNumProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __ArrivedNumber__ = default(int);
            var __ReceivedNumber__ = default(int);

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
                        __ArrivedNumber__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __ReceivedNumber__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.CurrentFrameNumProperty();
            ____result.ArrivedNumber = __ArrivedNumber__;
            ____result.ReceivedNumber = __ReceivedNumber__;
            return ____result;
        }
    }


    public sealed class DepthPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.DepthProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DepthPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "scale", 0},
                { "depth_min_range", 1},
                { "depth_max_range", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("scale"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depth_min_range"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("depth_max_range"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.DepthProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Scale);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.MinRange);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.MaxRange);
            return offset - startOffset;
        }

        public global::SensCord.DepthProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Scale__ = default(float);
            var __MinRange__ = default(float);
            var __MaxRange__ = default(float);

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
                        __Scale__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __MinRange__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __MaxRange__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.DepthProperty();
            ____result.Scale = __Scale__;
            ____result.MinRange = __MinRange__;
            ____result.MaxRange = __MaxRange__;
            return ____result;
        }
    }


    public sealed class RectangleRegionParameterFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.RectangleRegionParameter>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RectangleRegionParameterFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "top", 0},
                { "left", 1},
                { "bottom", 2},
                { "right", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("top"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("left"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("bottom"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("right"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.RectangleRegionParameter value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Top);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Left);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Bottom);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Right);
            return offset - startOffset;
        }

        public global::SensCord.RectangleRegionParameter Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Top__ = default(int);
            var __Left__ = default(int);
            var __Bottom__ = default(int);
            var __Right__ = default(int);

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
                        __Top__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Left__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Bottom__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Right__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.RectangleRegionParameter();
            ____result.Top = __Top__;
            ____result.Left = __Left__;
            ____result.Bottom = __Bottom__;
            ____result.Right = __Right__;
            return ____result;
        }
    }


    public sealed class ExposurePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ExposureProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ExposurePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "mode", 0},
                { "ev_compensation", 1},
                { "exposure_time", 2},
                { "iso_sensitivity", 3},
                { "metering", 4},
                { "target_region", 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("mode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ev_compensation"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposure_time"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("iso_sensitivity"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("metering"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("target_region"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ExposureProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 6);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Mode, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.EvCompensation);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ExposureTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.IsoSensitivity);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Metering, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.RectangleRegionParameter>().Serialize(ref bytes, offset, value.TargetRegion, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ExposureProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Mode__ = default(string);
            var __EvCompensation__ = default(float);
            var __ExposureTime__ = default(int);
            var __IsoSensitivity__ = default(int);
            var __Metering__ = default(string);
            var __TargetRegion__ = default(global::SensCord.RectangleRegionParameter);

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
                        __Mode__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __EvCompensation__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __ExposureTime__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __IsoSensitivity__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __Metering__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __TargetRegion__ = formatterResolver.GetFormatterWithVerify<global::SensCord.RectangleRegionParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ExposureProperty();
            ____result.Mode = __Mode__;
            ____result.EvCompensation = __EvCompensation__;
            ____result.ExposureTime = __ExposureTime__;
            ____result.IsoSensitivity = __IsoSensitivity__;
            ____result.Metering = __Metering__;
            ____result.TargetRegion = __TargetRegion__;
            return ____result;
        }
    }


    public sealed class FrameBufferingPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.FrameBufferingProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FrameBufferingPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "buffering", 0},
                { "num", 1},
                { "format", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("buffering"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("num"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("format"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.FrameBufferingProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Buffering>().Serialize(ref bytes, offset, value.Buffering, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Num);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.BufferingFormat>().Serialize(ref bytes, offset, value.Format, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.FrameBufferingProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Buffering__ = default(global::SensCord.Buffering);
            var __Num__ = default(int);
            var __Format__ = default(global::SensCord.BufferingFormat);

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
                        __Buffering__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Buffering>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Num__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Format__ = formatterResolver.GetFormatterWithVerify<global::SensCord.BufferingFormat>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.FrameBufferingProperty();
            ____result.Buffering = __Buffering__;
            ____result.Num = __Num__;
            ____result.Format = __Format__;
            return ____result;
        }
    }


    public sealed class FrameRatePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.FrameRateProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FrameRatePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "num", 0},
                { "denom", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("num"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("denom"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.FrameRateProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Num);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Denom);
            return offset - startOffset;
        }

        public global::SensCord.FrameRateProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Num__ = default(int);
            var __Denom__ = default(int);

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
                        __Num__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Denom__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.FrameRateProperty();
            ____result.Num = __Num__;
            ____result.Denom = __Denom__;
            return ____result;
        }
    }


    public sealed class GridSizeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.GridSize>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GridSizeFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "x", 0},
                { "y", 1},
                { "z", 2},
                { "unit", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("z"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("unit"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.GridSize value, global::MessagePack.IFormatterResolver formatterResolver)
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
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.GridUnit>().Serialize(ref bytes, offset, value.unit, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.GridSize Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __unit__ = default(global::SensCord.GridUnit);

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
                        __unit__ = formatterResolver.GetFormatterWithVerify<global::SensCord.GridUnit>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.GridSize();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            ____result.unit = __unit__;
            return ____result;
        }
    }


    public sealed class GridMapPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.GridMapProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GridMapPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "grid_num_x", 0},
                { "grid_num_y", 1},
                { "grid_num_z", 2},
                { "pixel_format", 3},
                { "grid_size", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("grid_num_x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("grid_num_y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("grid_num_z"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pixel_format"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("grid_size"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.GridMapProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.GridNumX);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.GridNumY);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.GridNumZ);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.PixelFormat, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.GridSize>().Serialize(ref bytes, offset, value.GridSize, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.GridMapProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __GridNumX__ = default(int);
            var __GridNumY__ = default(int);
            var __GridNumZ__ = default(int);
            var __PixelFormat__ = default(string);
            var __GridSize__ = default(global::SensCord.GridSize);

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
                        __GridNumX__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __GridNumY__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __GridNumZ__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __PixelFormat__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __GridSize__ = formatterResolver.GetFormatterWithVerify<global::SensCord.GridSize>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.GridMapProperty();
            ____result.GridNumX = __GridNumX__;
            ____result.GridNumY = __GridNumY__;
            ____result.GridNumZ = __GridNumZ__;
            ____result.PixelFormat = __PixelFormat__;
            ____result.GridSize = __GridSize__;
            return ____result;
        }
    }


    public sealed class GridSizePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.GridSizeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GridSizePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "x", 0},
                { "y", 1},
                { "z", 2},
                { "unit", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("x"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("y"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("z"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("unit"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.GridSizeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
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
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.GridUnit>().Serialize(ref bytes, offset, value.unit, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.GridSizeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __unit__ = default(global::SensCord.GridUnit);

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
                        __unit__ = formatterResolver.GetFormatterWithVerify<global::SensCord.GridUnit>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.GridSizeProperty();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            ____result.unit = __unit__;
            return ____result;
        }
    }


    public sealed class GyrometerRangePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.GyrometerRangeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public GyrometerRangePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "value", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("value"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.GyrometerRangeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.value);
            return offset - startOffset;
        }

        public global::SensCord.GyrometerRangeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __value__ = default(float);

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
                        __value__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.GyrometerRangeProperty();
            ____result.value = __value__;
            return ____result;
        }
    }


    public sealed class AngularVelocityCalibPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AngularVelocityCalibProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AngularVelocityCalibPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "ms", 0},
                { "offset", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ms"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("offset"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AngularVelocityCalibProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Serialize(ref bytes, offset, value.MS, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Offset, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.AngularVelocityCalibProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MS__ = default(global::SensCord.Matrix3x3<float>);
            var __Offset__ = default(global::SensCord.Vector3<float>);

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
                        __MS__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Offset__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.AngularVelocityCalibProperty();
            ____result.MS = __MS__;
            ____result.Offset = __Offset__;
            return ____result;
        }
    }


    public sealed class ImageCropPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ImageCropProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ImageCropPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "left", 0},
                { "top", 1},
                { "width", 2},
                { "height", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("left"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("top"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ImageCropProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Left);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Top);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Height);
            return offset - startOffset;
        }

        public global::SensCord.ImageCropProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Left__ = default(int);
            var __Top__ = default(int);
            var __Width__ = default(int);
            var __Height__ = default(int);

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
                        __Left__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Top__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ImageCropProperty();
            ____result.Left = __Left__;
            ____result.Top = __Top__;
            ____result.Width = __Width__;
            ____result.Height = __Height__;
            return ____result;
        }
    }


    public sealed class ImagePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ImageProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ImagePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "width", 0},
                { "height", 1},
                { "stride_bytes", 2},
                { "pixel_format", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("stride_bytes"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pixel_format"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ImageProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.StrideBytes);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.PixelFormat, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ImageProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Width__ = default(int);
            var __Height__ = default(int);
            var __StrideBytes__ = default(int);
            var __PixelFormat__ = default(string);

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
                        __Width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __StrideBytes__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __PixelFormat__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ImageProperty();
            ____result.Width = __Width__;
            ____result.Height = __Height__;
            ____result.StrideBytes = __StrideBytes__;
            ____result.PixelFormat = __PixelFormat__;
            return ____result;
        }
    }


    public sealed class ImageSensorFunctionPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ImageSensorFunctionProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ImageSensorFunctionPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "auto_exposure", 0},
                { "auto_white_balance", 1},
                { "brightness", 2},
                { "iso_sensitivity", 3},
                { "exposure_time", 4},
                { "exposure_metering", 5},
                { "gamma_value", 6},
                { "gain_value", 7},
                { "hue", 8},
                { "saturation", 9},
                { "sharpness", 10},
                { "white_balance", 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("auto_exposure"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("auto_white_balance"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("brightness"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("iso_sensitivity"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposure_time"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposure_metering"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("gamma_value"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("gain_value"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("hue"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("saturation"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("sharpness"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("white_balance"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ImageSensorFunctionProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 12);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.AutoExposure);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.AutoWhiteBalance);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Brightness);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.IsoSensitivity);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ExposureTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.ExposureMetering, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.GammaValue);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.GainValue);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Hue);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Saturation);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Sharpness);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.WhiteBalance);
            return offset - startOffset;
        }

        public global::SensCord.ImageSensorFunctionProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __AutoExposure__ = default(bool);
            var __AutoWhiteBalance__ = default(bool);
            var __Brightness__ = default(int);
            var __IsoSensitivity__ = default(int);
            var __ExposureTime__ = default(int);
            var __ExposureMetering__ = default(string);
            var __GammaValue__ = default(float);
            var __GainValue__ = default(int);
            var __Hue__ = default(int);
            var __Saturation__ = default(int);
            var __Sharpness__ = default(int);
            var __WhiteBalance__ = default(int);

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
                        __AutoExposure__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __AutoWhiteBalance__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Brightness__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __IsoSensitivity__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __ExposureTime__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __ExposureMetering__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __GammaValue__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 7:
                        __GainValue__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 8:
                        __Hue__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 9:
                        __Saturation__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 10:
                        __Sharpness__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 11:
                        __WhiteBalance__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ImageSensorFunctionProperty();
            ____result.AutoExposure = __AutoExposure__;
            ____result.AutoWhiteBalance = __AutoWhiteBalance__;
            ____result.Brightness = __Brightness__;
            ____result.IsoSensitivity = __IsoSensitivity__;
            ____result.ExposureTime = __ExposureTime__;
            ____result.ExposureMetering = __ExposureMetering__;
            ____result.GammaValue = __GammaValue__;
            ____result.GainValue = __GainValue__;
            ____result.Hue = __Hue__;
            ____result.Saturation = __Saturation__;
            ____result.Sharpness = __Sharpness__;
            ____result.WhiteBalance = __WhiteBalance__;
            return ____result;
        }
    }


    public sealed class ImageSensorFunctionSupportedPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ImageSensorFunctionSupportedProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ImageSensorFunctionSupportedPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "auto_exposure_supported", 0},
                { "auto_white_balance_supported", 1},
                { "brightness_supported", 2},
                { "iso_sensitivity_supported", 3},
                { "exposure_time_supported", 4},
                { "exposure_metering_supported", 5},
                { "gamma_value_supported", 6},
                { "gain_value_supported", 7},
                { "hue_supported", 8},
                { "saturation_supported", 9},
                { "sharpness_supported", 10},
                { "white_balance_supported", 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("auto_exposure_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("auto_white_balance_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("brightness_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("iso_sensitivity_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposure_time_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("exposure_metering_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("gamma_value_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("gain_value_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("hue_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("saturation_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("sharpness_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("white_balance_supported"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ImageSensorFunctionSupportedProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 12);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.AutoExposureSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.AutoWhiteBalanceSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.BrightnessSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.IsoSensitivitySupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.ExposureTimeSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.ExposureMeteringSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.GammaValueSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.GainValueSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.HueSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.SaturationSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.SharpnessSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.WhiteBalanceSupported);
            return offset - startOffset;
        }

        public global::SensCord.ImageSensorFunctionSupportedProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __AutoExposureSupported__ = default(bool);
            var __AutoWhiteBalanceSupported__ = default(bool);
            var __BrightnessSupported__ = default(bool);
            var __IsoSensitivitySupported__ = default(bool);
            var __ExposureTimeSupported__ = default(bool);
            var __ExposureMeteringSupported__ = default(bool);
            var __GammaValueSupported__ = default(bool);
            var __GainValueSupported__ = default(bool);
            var __HueSupported__ = default(bool);
            var __SaturationSupported__ = default(bool);
            var __SharpnessSupported__ = default(bool);
            var __WhiteBalanceSupported__ = default(bool);

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
                        __AutoExposureSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __AutoWhiteBalanceSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __BrightnessSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 3:
                        __IsoSensitivitySupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 4:
                        __ExposureTimeSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 5:
                        __ExposureMeteringSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 6:
                        __GammaValueSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 7:
                        __GainValueSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 8:
                        __HueSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 9:
                        __SaturationSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 10:
                        __SharpnessSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 11:
                        __WhiteBalanceSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ImageSensorFunctionSupportedProperty();
            ____result.AutoExposureSupported = __AutoExposureSupported__;
            ____result.AutoWhiteBalanceSupported = __AutoWhiteBalanceSupported__;
            ____result.BrightnessSupported = __BrightnessSupported__;
            ____result.IsoSensitivitySupported = __IsoSensitivitySupported__;
            ____result.ExposureTimeSupported = __ExposureTimeSupported__;
            ____result.ExposureMeteringSupported = __ExposureMeteringSupported__;
            ____result.GammaValueSupported = __GammaValueSupported__;
            ____result.GainValueSupported = __GainValueSupported__;
            ____result.HueSupported = __HueSupported__;
            ____result.SaturationSupported = __SaturationSupported__;
            ____result.SharpnessSupported = __SharpnessSupported__;
            ____result.WhiteBalanceSupported = __WhiteBalanceSupported__;
            return ____result;
        }
    }


    public sealed class ImuDataUnitPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ImuDataUnitProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ImuDataUnitPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "acceleration", 0},
                { "angular_velocity", 1},
                { "magnetic_field", 2},
                { "orientation", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("acceleration"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("angular_velocity"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("magnetic_field"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("orientation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ImuDataUnitProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.AccelerationUnit>().Serialize(ref bytes, offset, value.Acceleration, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.AngularVelocityUnit>().Serialize(ref bytes, offset, value.AngularVelocity, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.MagneticFieldUnit>().Serialize(ref bytes, offset, value.MagneticField, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.OrientationUnit>().Serialize(ref bytes, offset, value.Orientation, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ImuDataUnitProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Acceleration__ = default(global::SensCord.AccelerationUnit);
            var __AngularVelocity__ = default(global::SensCord.AngularVelocityUnit);
            var __MagneticField__ = default(global::SensCord.MagneticFieldUnit);
            var __Orientation__ = default(global::SensCord.OrientationUnit);

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
                        __Acceleration__ = formatterResolver.GetFormatterWithVerify<global::SensCord.AccelerationUnit>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __AngularVelocity__ = formatterResolver.GetFormatterWithVerify<global::SensCord.AngularVelocityUnit>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __MagneticField__ = formatterResolver.GetFormatterWithVerify<global::SensCord.MagneticFieldUnit>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __Orientation__ = formatterResolver.GetFormatterWithVerify<global::SensCord.OrientationUnit>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ImuDataUnitProperty();
            ____result.Acceleration = __Acceleration__;
            ____result.AngularVelocity = __AngularVelocity__;
            ____result.MagneticField = __MagneticField__;
            ____result.Orientation = __Orientation__;
            return ____result;
        }
    }


    public sealed class InitialPosePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.InitialPoseProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public InitialPosePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "position", 0},
                { "orientation", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("orientation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.InitialPoseProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Quaternion<float>>().Serialize(ref bytes, offset, value.Orientation, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.InitialPoseProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Position__ = default(global::SensCord.Vector3<float>);
            var __Orientation__ = default(global::SensCord.Quaternion<float>);

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
                        __Position__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Orientation__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Quaternion<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.InitialPoseProperty();
            ____result.Position = __Position__;
            ____result.Orientation = __Orientation__;
            return ____result;
        }
    }


    public sealed class InterlacePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.InterlaceProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public InterlacePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "field", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("field"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.InterlaceProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.InterlaceField>().Serialize(ref bytes, offset, value.Field, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.InterlaceProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Field__ = default(global::SensCord.InterlaceField);

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
                        __Field__ = formatterResolver.GetFormatterWithVerify<global::SensCord.InterlaceField>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.InterlaceProperty();
            ____result.Field = __Field__;
            return ____result;
        }
    }


    public sealed class InterlaceInfoPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.InterlaceInfoProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public InterlaceInfoPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "order", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("order"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.InterlaceInfoProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.InterlaceOrder>().Serialize(ref bytes, offset, value.Order, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.InterlaceInfoProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Order__ = default(global::SensCord.InterlaceOrder);

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
                        __Order__ = formatterResolver.GetFormatterWithVerify<global::SensCord.InterlaceOrder>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.InterlaceInfoProperty();
            ____result.Order = __Order__;
            return ____result;
        }
    }


    public sealed class LensPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.LensProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public LensPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "horizontal_field_of_view", 0},
                { "vertical_field_of_view", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("horizontal_field_of_view"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("vertical_field_of_view"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.LensProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.HorizontalFov);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.VerticalFov);
            return offset - startOffset;
        }

        public global::SensCord.LensProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __HorizontalFov__ = default(float);
            var __VerticalFov__ = default(float);

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
                        __HorizontalFov__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __VerticalFov__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.LensProperty();
            ____result.HorizontalFov = __HorizontalFov__;
            ____result.VerticalFov = __VerticalFov__;
            return ____result;
        }
    }


    public sealed class MagnetometerRangePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.MagnetometerRangeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MagnetometerRangePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "value", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("value"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.MagnetometerRangeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.value);
            return offset - startOffset;
        }

        public global::SensCord.MagnetometerRangeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __value__ = default(float);

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
                        __value__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.MagnetometerRangeProperty();
            ____result.value = __value__;
            return ____result;
        }
    }


    public sealed class MagnetometerRange3PropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.MagnetometerRange3Property>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MagnetometerRange3PropertyFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.MagnetometerRange3Property value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::SensCord.MagnetometerRange3Property Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::SensCord.MagnetometerRange3Property();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            return ____result;
        }
    }


    public sealed class MagneticFieldCalibPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.MagneticFieldCalibProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MagneticFieldCalibPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "ms", 0},
                { "offset", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ms"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("offset"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.MagneticFieldCalibProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Serialize(ref bytes, offset, value.MS, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Offset, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.MagneticFieldCalibProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MS__ = default(global::SensCord.Matrix3x3<float>);
            var __Offset__ = default(global::SensCord.Vector3<float>);

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
                        __MS__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Offset__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.MagneticFieldCalibProperty();
            ____result.MS = __MS__;
            ____result.Offset = __Offset__;
            return ____result;
        }
    }


    public sealed class MagneticNorthCalibPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.MagneticNorthCalibProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MagneticNorthCalibPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "declination", 0},
                { "inclination", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("declination"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("inclination"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.MagneticNorthCalibProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Declination);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Inclination);
            return offset - startOffset;
        }

        public global::SensCord.MagneticNorthCalibProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Declination__ = default(float);
            var __Inclination__ = default(float);

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
                        __Declination__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Inclination__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.MagneticNorthCalibProperty();
            ____result.Declination = __Declination__;
            ____result.Inclination = __Inclination__;
            return ____result;
        }
    }


    public sealed class OdometryDataPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.OdometryDataProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OdometryDataPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "coordinate_system", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("coordinate_system"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.OdometryDataProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.CoordinateSystem>().Serialize(ref bytes, offset, value.CoordinateSystem, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.OdometryDataProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __CoordinateSystem__ = default(global::SensCord.CoordinateSystem);

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
                        __CoordinateSystem__ = formatterResolver.GetFormatterWithVerify<global::SensCord.CoordinateSystem>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.OdometryDataProperty();
            ____result.CoordinateSystem = __CoordinateSystem__;
            return ____result;
        }
    }


    public sealed class PixelPolarityDataPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PixelPolarityDataProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PixelPolarityDataPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "trigger_type", 0},
                { "event_count", 1},
                { "accumulation_time", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("trigger_type"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("event_count"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("accumulation_time"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PixelPolarityDataProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.PixelPolarityTriggerType>().Serialize(ref bytes, offset, value.TriggerType, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.EventCount);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.AccumulationTime);
            return offset - startOffset;
        }

        public global::SensCord.PixelPolarityDataProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __TriggerType__ = default(global::SensCord.PixelPolarityTriggerType);
            var __EventCount__ = default(int);
            var __AccumulationTime__ = default(int);

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
                        __TriggerType__ = formatterResolver.GetFormatterWithVerify<global::SensCord.PixelPolarityTriggerType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __EventCount__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __AccumulationTime__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PixelPolarityDataProperty();
            ____result.TriggerType = __TriggerType__;
            ____result.EventCount = __EventCount__;
            ____result.AccumulationTime = __AccumulationTime__;
            return ____result;
        }
    }


    public sealed class PlayModePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PlayModeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PlayModePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "repeat", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("repeat"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PlayModeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Repeat);
            return offset - startOffset;
        }

        public global::SensCord.PlayModeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Repeat__ = default(bool);

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
                        __Repeat__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PlayModeProperty();
            ____result.Repeat = __Repeat__;
            return ____result;
        }
    }


    public sealed class PlayPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PlayProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PlayPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "target_path", 0},
                { "start_offset", 1},
                { "count", 2},
                { "speed", 3},
                { "mode", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("target_path"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("start_offset"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("count"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("speed"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("mode"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PlayProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.TargetPath, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.StartOffset);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Count);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.PlaySpeed>().Serialize(ref bytes, offset, value.Speed, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.PlayModeProperty>().Serialize(ref bytes, offset, value.Mode, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.PlayProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __TargetPath__ = default(string);
            var __StartOffset__ = default(int);
            var __Count__ = default(int);
            var __Speed__ = default(global::SensCord.PlaySpeed);
            var __Mode__ = default(global::SensCord.PlayModeProperty);

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
                        __TargetPath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __StartOffset__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Count__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Speed__ = formatterResolver.GetFormatterWithVerify<global::SensCord.PlaySpeed>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __Mode__ = formatterResolver.GetFormatterWithVerify<global::SensCord.PlayModeProperty>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PlayProperty();
            ____result.TargetPath = __TargetPath__;
            ____result.StartOffset = __StartOffset__;
            ____result.Count = __Count__;
            ____result.Speed = __Speed__;
            ____result.Mode = __Mode__;
            return ____result;
        }
    }


    public sealed class PointCloudPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PointCloudProperty>
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


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PointCloudProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Height);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.PixelFormat, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.PointCloudProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Width__ = default(int);
            var __Height__ = default(int);
            var __PixelFormat__ = default(string);

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
                        __Width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __PixelFormat__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PointCloudProperty();
            ____result.Width = __Width__;
            ____result.Height = __Height__;
            ____result.PixelFormat = __PixelFormat__;
            return ____result;
        }
    }


    public sealed class PoseDataPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PoseDataProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PoseDataPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "data_format", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("data_format"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PoseDataProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.DataFormat, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.PoseDataProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __DataFormat__ = default(string);

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
                        __DataFormat__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PoseDataProperty();
            ____result.DataFormat = __DataFormat__;
            return ____result;
        }
    }


    public sealed class PresetListPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PresetListProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PresetListPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "presets", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("presets"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PresetListProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, string>>().Serialize(ref bytes, offset, value.Presets, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.PresetListProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Presets__ = default(global::System.Collections.Generic.Dictionary<long, string>);

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
                        __Presets__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, string>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PresetListProperty();
            ____result.Presets = __Presets__;
            return ____result;
        }
    }


    public sealed class PresetPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PresetProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PresetPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "id", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("id"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PresetProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.Id);
            return offset - startOffset;
        }

        public global::SensCord.PresetProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(long);

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
                        __Id__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PresetProperty();
            ____result.Id = __Id__;
            return ____result;
        }
    }


    public sealed class RecordPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.RecordProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RecordPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "enabled", 0},
                { "path", 1},
                { "count", 2},
                { "formats", 3},
                { "buffer_num", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("enabled"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("path"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("count"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("formats"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("buffer_num"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.RecordProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Enabled);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Path, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Count);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, string>>().Serialize(ref bytes, offset, value.Formats, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.BufferNum);
            return offset - startOffset;
        }

        public global::SensCord.RecordProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Enabled__ = default(bool);
            var __Path__ = default(string);
            var __Count__ = default(int);
            var __Formats__ = default(global::System.Collections.Generic.Dictionary<long, string>);
            var __BufferNum__ = default(int);

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
                        __Enabled__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Path__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Count__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Formats__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<long, string>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __BufferNum__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.RecordProperty();
            ____result.Enabled = __Enabled__;
            ____result.Path = __Path__;
            ____result.Count = __Count__;
            ____result.Formats = __Formats__;
            ____result.BufferNum = __BufferNum__;
            return ____result;
        }
    }


    public sealed class RecorderListPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.RecorderListProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RecorderListPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "formats", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("formats"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.RecorderListProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Serialize(ref bytes, offset, value.Formats, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.RecorderListProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Formats__ = default(global::System.Collections.Generic.List<string>);

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
                        __Formats__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.RecorderListProperty();
            ____result.Formats = __Formats__;
            return ____result;
        }
    }


    public sealed class RoiPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.RoiProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RoiPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "left", 0},
                { "top", 1},
                { "width", 2},
                { "height", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("left"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("top"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("width"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("height"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.RoiProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Left);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Top);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Width);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Height);
            return offset - startOffset;
        }

        public global::SensCord.RoiProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Left__ = default(int);
            var __Top__ = default(int);
            var __Width__ = default(int);
            var __Height__ = default(int);

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
                        __Left__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Top__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Width__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Height__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.RoiProperty();
            ____result.Left = __Left__;
            ____result.Top = __Top__;
            ____result.Width = __Width__;
            ____result.Height = __Height__;
            return ____result;
        }
    }


    public sealed class SamplingFrequencyPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.SamplingFrequencyProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SamplingFrequencyPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "value", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("value"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.SamplingFrequencyProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.value);
            return offset - startOffset;
        }

        public global::SensCord.SamplingFrequencyProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __value__ = default(float);

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
                        __value__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.SamplingFrequencyProperty();
            ____result.value = __value__;
            return ____result;
        }
    }


    public sealed class ScoreThresholdPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ScoreThresholdProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ScoreThresholdPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "score_threshold", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("score_threshold"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ScoreThresholdProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.ScoreThreshold);
            return offset - startOffset;
        }

        public global::SensCord.ScoreThresholdProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __ScoreThreshold__ = default(float);

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
                        __ScoreThreshold__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ScoreThresholdProperty();
            ____result.ScoreThreshold = __ScoreThreshold__;
            return ____result;
        }
    }


    public sealed class SkipFramePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.SkipFrameProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SkipFramePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "rate", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rate"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.SkipFrameProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Rate);
            return offset - startOffset;
        }

        public global::SensCord.SkipFrameProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Rate__ = default(int);

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
                        __Rate__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.SkipFrameProperty();
            ____result.Rate = __Rate__;
            return ____result;
        }
    }


    public sealed class SlamDataSupportedPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.SlamDataSupportedProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public SlamDataSupportedPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "odometory_supported", 0},
                { "gridmap_supported", 1},
                { "pointcloud_supported", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("odometory_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("gridmap_supported"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pointcloud_supported"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.SlamDataSupportedProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.OdometorySupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.GridMapSupported);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.PointCloudSupported);
            return offset - startOffset;
        }

        public global::SensCord.SlamDataSupportedProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __OdometorySupported__ = default(bool);
            var __GridMapSupported__ = default(bool);
            var __PointCloudSupported__ = default(bool);

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
                        __OdometorySupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __GridMapSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __PointCloudSupported__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.SlamDataSupportedProperty();
            ____result.OdometorySupported = __OdometorySupported__;
            ____result.GridMapSupported = __GridMapSupported__;
            ____result.PointCloudSupported = __PointCloudSupported__;
            return ____result;
        }
    }


    public sealed class StreamTypePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.StreamTypeProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StreamTypePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "type", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("type"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.StreamTypeProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Type, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.StreamTypeProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Type__ = default(string);

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
                        __Type__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.StreamTypeProperty();
            ____result.Type = __Type__;
            return ____result;
        }
    }


    public sealed class StreamKeyPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.StreamKeyProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StreamKeyPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "stream_key", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("stream_key"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.StreamKeyProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.StreamKey, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.StreamKeyProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __StreamKey__ = default(string);

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
                        __StreamKey__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.StreamKeyProperty();
            ____result.StreamKey = __StreamKey__;
            return ____result;
        }
    }


    public sealed class StreamStatePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.StreamStateProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public StreamStatePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "state", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("state"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.StreamStateProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.StreamState>().Serialize(ref bytes, offset, value.State, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.StreamStateProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __State__ = default(global::SensCord.StreamState);

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
                        __State__ = formatterResolver.GetFormatterWithVerify<global::SensCord.StreamState>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.StreamStateProperty();
            ____result.State = __State__;
            return ____result;
        }
    }


    public sealed class TemperatureInfoFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.TemperatureInfo>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TemperatureInfoFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "temperature", 0},
                { "description", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("temperature"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("description"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.TemperatureInfo value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Temperature);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Description, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.TemperatureInfo Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Temperature__ = default(float);
            var __Description__ = default(string);

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
                        __Temperature__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Description__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.TemperatureInfo();
            ____result.Temperature = __Temperature__;
            ____result.Description = __Description__;
            return ____result;
        }
    }


    public sealed class TemperaturePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.TemperatureProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TemperaturePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "temperatures", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("temperatures"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.TemperatureProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, global::SensCord.TemperatureInfo>>().Serialize(ref bytes, offset, value.Temperatures, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.TemperatureProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Temperatures__ = default(global::System.Collections.Generic.Dictionary<int, global::SensCord.TemperatureInfo>);

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
                        __Temperatures__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, global::SensCord.TemperatureInfo>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.TemperatureProperty();
            ____result.Temperatures = __Temperatures__;
            return ____result;
        }
    }


    public sealed class UserDataPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.UserDataProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserDataPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "data", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("data"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.UserDataProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Data, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.UserDataProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Data__ = default(byte[]);

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
                        __Data__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.UserDataProperty();
            ____result.Data = __Data__;
            return ____result;
        }
    }


    public sealed class VelocityDataUnitPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.VelocityDataUnitProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public VelocityDataUnitPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "velocity", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("velocity"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.VelocityDataUnitProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.VelocityUnit>().Serialize(ref bytes, offset, value.Velocity, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.VelocityDataUnitProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Velocity__ = default(global::SensCord.VelocityUnit);

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
                        __Velocity__ = formatterResolver.GetFormatterWithVerify<global::SensCord.VelocityUnit>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.VelocityDataUnitProperty();
            ____result.Velocity = __Velocity__;
            return ____result;
        }
    }


    public sealed class VersionPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.VersionProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public VersionPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "name", 0},
                { "major", 1},
                { "minor", 2},
                { "patch", 3},
                { "description", 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("major"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("minor"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("patch"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("description"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.VersionProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 5);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Major);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Minor);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Patch);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Description, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.VersionProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Name__ = default(string);
            var __Major__ = default(int);
            var __Minor__ = default(int);
            var __Patch__ = default(int);
            var __Description__ = default(string);

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
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Major__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Minor__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Patch__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __Description__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.VersionProperty();
            ____result.Name = __Name__;
            ____result.Major = __Major__;
            ____result.Minor = __Minor__;
            ____result.Patch = __Patch__;
            ____result.Description = __Description__;
            return ____result;
        }
    }


    public sealed class WhiteBalancePropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.WhiteBalanceProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public WhiteBalancePropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "mode", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("mode"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.WhiteBalanceProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Mode, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.WhiteBalanceProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Mode__ = default(string);

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
                        __Mode__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.WhiteBalanceProperty();
            ____result.Mode = __Mode__;
            return ____result;
        }
    }


    public sealed class AccelerationDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AccelerationData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AccelerationDataFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AccelerationData value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::SensCord.AccelerationData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::SensCord.AccelerationData();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            return ____result;
        }
    }


    public sealed class AngularVelocityDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.AngularVelocityData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public AngularVelocityDataFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.AngularVelocityData value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::SensCord.AngularVelocityData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::SensCord.AngularVelocityData();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            return ____result;
        }
    }


    public sealed class MagneticFieldDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.MagneticFieldData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MagneticFieldDataFormatter()
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


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.MagneticFieldData value, global::MessagePack.IFormatterResolver formatterResolver)
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

        public global::SensCord.MagneticFieldData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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

            var ____result = new global::SensCord.MagneticFieldData();
            ____result.x = __x__;
            ____result.y = __y__;
            ____result.z = __z__;
            return ____result;
        }
    }


    public sealed class KeyPointFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.KeyPoint>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public KeyPointFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "point", 0},
                { "key_point_id", 1},
                { "score", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("point"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("key_point_id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("score"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.KeyPoint value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Point, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.KeyPointId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Score);
            return offset - startOffset;
        }

        public global::SensCord.KeyPoint Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Point__ = default(global::SensCord.Vector3<float>);
            var __KeyPointId__ = default(int);
            var __Score__ = default(float);

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
                        __Point__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __KeyPointId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Score__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.KeyPoint();
            ____result.Point = __Point__;
            ____result.KeyPointId = __KeyPointId__;
            ____result.Score = __Score__;
            return ____result;
        }
    }


    public sealed class DetectedKeyPointInformationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.DetectedKeyPointInformation>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DetectedKeyPointInformationFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "key_points", 0},
                { "class_id", 1},
                { "score", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("key_points"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("class_id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("score"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.DetectedKeyPointInformation value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.KeyPoint>>().Serialize(ref bytes, offset, value.KeyPoints, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ClassId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Score);
            return offset - startOffset;
        }

        public global::SensCord.DetectedKeyPointInformation Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __KeyPoints__ = default(global::System.Collections.Generic.List<global::SensCord.KeyPoint>);
            var __ClassId__ = default(int);
            var __Score__ = default(float);

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
                        __KeyPoints__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.KeyPoint>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __ClassId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Score__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.DetectedKeyPointInformation();
            ____result.KeyPoints = __KeyPoints__;
            ____result.ClassId = __ClassId__;
            ____result.Score = __Score__;
            return ____result;
        }
    }


    public sealed class KeyPointDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.KeyPointData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public KeyPointDataFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "data", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("data"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.KeyPointData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.DetectedKeyPointInformation>>().Serialize(ref bytes, offset, value.Data, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.KeyPointData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Data__ = default(global::System.Collections.Generic.List<global::SensCord.DetectedKeyPointInformation>);

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
                        __Data__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.DetectedKeyPointInformation>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.KeyPointData();
            ____result.Data = __Data__;
            return ____result;
        }
    }


    public sealed class DetectedObjectInformationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.DetectedObjectInformation>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public DetectedObjectInformationFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "box", 0},
                { "class_id", 1},
                { "score", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("box"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("class_id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("score"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.DetectedObjectInformation value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.RectangleRegionParameter>().Serialize(ref bytes, offset, value.Box, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ClassId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Score);
            return offset - startOffset;
        }

        public global::SensCord.DetectedObjectInformation Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Box__ = default(global::SensCord.RectangleRegionParameter);
            var __ClassId__ = default(int);
            var __Score__ = default(float);

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
                        __Box__ = formatterResolver.GetFormatterWithVerify<global::SensCord.RectangleRegionParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __ClassId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Score__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.DetectedObjectInformation();
            ____result.Box = __Box__;
            ____result.ClassId = __ClassId__;
            ____result.Score = __Score__;
            return ____result;
        }
    }


    public sealed class ObjectDetectionDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ObjectDetectionData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ObjectDetectionDataFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "data", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("data"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ObjectDetectionData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.DetectedObjectInformation>>().Serialize(ref bytes, offset, value.Data, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ObjectDetectionData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Data__ = default(global::System.Collections.Generic.List<global::SensCord.DetectedObjectInformation>);

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
                        __Data__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.DetectedObjectInformation>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ObjectDetectionData();
            ____result.Data = __Data__;
            return ____result;
        }
    }


    public sealed class TrackedObjectInformationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.TrackedObjectInformation>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TrackedObjectInformationFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "velocity", 0},
                { "position", 1},
                { "box", 2},
                { "track_id", 3},
                { "class_id", 4},
                { "score", 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("velocity"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("box"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("track_id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("class_id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("score"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.TrackedObjectInformation value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 6);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector2<float>>().Serialize(ref bytes, offset, value.Velocity, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector2<int>>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.RectangleRegionParameter>().Serialize(ref bytes, offset, value.Box, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.TrackId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ClassId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Score);
            return offset - startOffset;
        }

        public global::SensCord.TrackedObjectInformation Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Velocity__ = default(global::SensCord.Vector2<float>);
            var __Position__ = default(global::SensCord.Vector2<int>);
            var __Box__ = default(global::SensCord.RectangleRegionParameter);
            var __TrackId__ = default(int);
            var __ClassId__ = default(int);
            var __Score__ = default(float);

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
                        __Velocity__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector2<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Position__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector2<int>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Box__ = formatterResolver.GetFormatterWithVerify<global::SensCord.RectangleRegionParameter>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __TrackId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __ClassId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __Score__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.TrackedObjectInformation();
            ____result.Velocity = __Velocity__;
            ____result.Position = __Position__;
            ____result.Box = __Box__;
            ____result.TrackId = __TrackId__;
            ____result.ClassId = __ClassId__;
            ____result.Score = __Score__;
            return ____result;
        }
    }


    public sealed class ObjectTrackingDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.ObjectTrackingData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ObjectTrackingDataFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "data", 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("data"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.ObjectTrackingData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 1);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.TrackedObjectInformation>>().Serialize(ref bytes, offset, value.Data, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.ObjectTrackingData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Data__ = default(global::System.Collections.Generic.List<global::SensCord.TrackedObjectInformation>);

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
                        __Data__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SensCord.TrackedObjectInformation>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.ObjectTrackingData();
            ____result.Data = __Data__;
            return ____result;
        }
    }


    public sealed class PoseQuaternionDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PoseQuaternionData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PoseQuaternionDataFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "position", 0},
                { "orientation", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("orientation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PoseQuaternionData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Quaternion<float>>().Serialize(ref bytes, offset, value.Orientation, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.PoseQuaternionData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Position__ = default(global::SensCord.Vector3<float>);
            var __Orientation__ = default(global::SensCord.Quaternion<float>);

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
                        __Position__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Orientation__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Quaternion<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PoseQuaternionData();
            ____result.Position = __Position__;
            ____result.Orientation = __Orientation__;
            return ____result;
        }
    }


    public sealed class PoseDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PoseData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PoseDataFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "position", 0},
                { "orientation", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("orientation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PoseData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Quaternion<float>>().Serialize(ref bytes, offset, value.Orientation, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.PoseData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Position__ = default(global::SensCord.Vector3<float>);
            var __Orientation__ = default(global::SensCord.Quaternion<float>);

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
                        __Position__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Orientation__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Quaternion<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PoseData();
            ____result.Position = __Position__;
            ____result.Orientation = __Orientation__;
            return ____result;
        }
    }


    public sealed class PoseMatrixDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.PoseMatrixData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PoseMatrixDataFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "position", 0},
                { "rotation", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("position"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("rotation"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.PoseMatrixData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Serialize(ref bytes, offset, value.Rotation, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.PoseMatrixData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Position__ = default(global::SensCord.Vector3<float>);
            var __Rotation__ = default(global::SensCord.Matrix3x3<float>);

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
                        __Position__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Vector3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Rotation__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Matrix3x3<float>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.PoseMatrixData();
            ____result.Position = __Position__;
            ____result.Rotation = __Rotation__;
            return ____result;
        }
    }


    public sealed class RotationDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.RotationData>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RotationDataFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "roll", 0},
                { "pitch", 1},
                { "yaw", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("roll"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("pitch"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("yaw"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.RotationData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.roll);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.pitch);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.yaw);
            return offset - startOffset;
        }

        public global::SensCord.RotationData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __roll__ = default(float);
            var __pitch__ = default(float);
            var __yaw__ = default(float);

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
                        __roll__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __pitch__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __yaw__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.RotationData();
            ____result.roll = __roll__;
            ____result.pitch = __pitch__;
            ____result.yaw = __yaw__;
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

namespace MessagePack.Formatters.SensCord.Itof
{
    using System;
    using MessagePack;


    public sealed class TriggerModeOptionsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.Itof.TriggerModeOptions>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TriggerModeOptionsFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "mode", 0},
                { "number_of_frames", 1},
                { "delay", 2},
                { "sampling_mode", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("mode"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("number_of_frames"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("delay"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("sampling_mode"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.Itof.TriggerModeOptions value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Mode);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.NumberOfFrames);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Delay);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.SamplingMode);
            return offset - startOffset;
        }

        public global::SensCord.Itof.TriggerModeOptions Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Mode__ = default(int);
            var __NumberOfFrames__ = default(int);
            var __Delay__ = default(float);
            var __SamplingMode__ = default(int);

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
                        __Mode__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __NumberOfFrames__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Delay__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 3:
                        __SamplingMode__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.Itof.TriggerModeOptions();
            ____result.Mode = __Mode__;
            ____result.NumberOfFrames = __NumberOfFrames__;
            ____result.Delay = __Delay__;
            ____result.SamplingMode = __SamplingMode__;
            return ____result;
        }
    }


    public sealed class MetaDataExtensionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.Itof.MetaDataExtension>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MetaDataExtensionFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "frame_rate", 0},
                { "trigger_mode", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frame_rate"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("trigger_mode"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.Itof.MetaDataExtension value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.FrameRate);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Itof.TriggerModeOptions>().Serialize(ref bytes, offset, value.TriggerMode, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.Itof.MetaDataExtension Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __FrameRate__ = default(float);
            var __TriggerMode__ = default(global::SensCord.Itof.TriggerModeOptions);

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
                        __FrameRate__ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 1:
                        __TriggerMode__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Itof.TriggerModeOptions>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.Itof.MetaDataExtension();
            ____result.FrameRate = __FrameRate__;
            ____result.TriggerMode = __TriggerMode__;
            return ____result;
        }
    }


    public sealed class FrameExtensionPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.Itof.FrameExtensionProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public FrameExtensionPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "frame_id", 0},
                { "host_timestamp", 1},
                { "uid", 2},
                { "error_information_type", 3},
                { "error_information", 4},
                { "raw_laser_temperature", 5},
                { "low_accuracy_data", 6},
                { "extra_data", 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("frame_id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("host_timestamp"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("uid"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("error_information_type"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("error_information"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("raw_laser_temperature"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("low_accuracy_data"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("extra_data"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.Itof.FrameExtensionProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 8);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.FrameId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.HostTimestamp);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Uid);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ErrorInformationType);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ErrorInformation);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.RawLaserTemperature);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.LowAccuracyData);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += formatterResolver.GetFormatterWithVerify<global::SensCord.Itof.MetaDataExtension>().Serialize(ref bytes, offset, value.ExtraData, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.Itof.FrameExtensionProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __FrameId__ = default(int);
            var __HostTimestamp__ = default(long);
            var __Uid__ = default(int);
            var __ErrorInformationType__ = default(int);
            var __ErrorInformation__ = default(int);
            var __RawLaserTemperature__ = default(int);
            var __LowAccuracyData__ = default(bool);
            var __ExtraData__ = default(global::SensCord.Itof.MetaDataExtension);

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
                        __FrameId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __HostTimestamp__ = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Uid__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __ErrorInformationType__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __ErrorInformation__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __RawLaserTemperature__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 6:
                        __LowAccuracyData__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 7:
                        __ExtraData__ = formatterResolver.GetFormatterWithVerify<global::SensCord.Itof.MetaDataExtension>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.Itof.FrameExtensionProperty();
            ____result.FrameId = __FrameId__;
            ____result.HostTimestamp = __HostTimestamp__;
            ____result.Uid = __Uid__;
            ____result.ErrorInformationType = __ErrorInformationType__;
            ____result.ErrorInformation = __ErrorInformation__;
            ____result.RawLaserTemperature = __RawLaserTemperature__;
            ____result.LowAccuracyData = __LowAccuracyData__;
            ____result.ExtraData = __ExtraData__;
            return ____result;
        }
    }


    public sealed class ModuleInformationPropertyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SensCord.Itof.ModuleInformationProperty>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ModuleInformationPropertyFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "software_id", 0},
                { "calibration", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("software_id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("calibration"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::SensCord.Itof.ModuleInformationProperty value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.SoftwareId);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Calibration, formatterResolver);
            return offset - startOffset;
        }

        public global::SensCord.Itof.ModuleInformationProperty Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __SoftwareId__ = default(int);
            var __Calibration__ = default(byte[]);

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
                        __SoftwareId__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Calibration__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SensCord.Itof.ModuleInformationProperty();
            ____result.SoftwareId = __SoftwareId__;
            ____result.Calibration = __Calibration__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

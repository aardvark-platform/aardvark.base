using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Aardvark.Base
{
    #region Col

    /// <summary>
    /// A static container class that provides functionality for dealing with
    /// colors. It is intentionally named Col in order to avoid a collision
    /// with the system provided Color class.
    /// </summary>
    [Serializable]
    public static partial class Col
    {
        #region Space

        [Serializable]
        public enum Space
        {
            None,
            SRGB,
            LinearSRGB,
            XYZ,
        }

        #endregion

        #region Format

        [Serializable]
        public enum Format
        {
            None,
            Alpha,
            BW,
            Gray,
            GrayAlpha,
            RGB,
            BGR,
            RGBA,
            BGRA,

            /// <summary>
            /// RGB with premultiplied alpha.
            /// </summary>
            RGBP,

            /// <summary>
            /// BGR with premultiplied alpha.
            /// </summary>
            BGRP,

            // TODO:

            CieXYZ,
            CieYxy,
            CieLab,
            CieLuv,
            Yuv,

            HSL,
            HSV,

            RG,
        }

        #endregion

        #region Channel

        [Serializable]
        public enum Channel
        {
            BW,
            Gray,
            Red,
            Green,
            Blue,
            Alpha,
            PremultipliedAlpha,

            // TODO:

            CieX,
            CieY,
            CieZ,
            Ciex,
            Ciey,
            Cieu,
            Ciev,
            CieL,
            Ciea,
            Cieb,
            L,
            H,
            S,
            V,
            Y,
            u,
            v,
        }

        #endregion

        #region Maximal Value Constants

        public static class Info<T>
        {
            public static readonly T MaxValue;

            static Info()
            {
                if (typeof(T) == typeof(bool))
                {
                    MaxValue = (T)(object)true;
                }
                else if (typeof(T) == typeof(byte))
                {
                    MaxValue = (T)(object)(byte)255;
                }
                else if (typeof(T) == typeof(ushort))
                {
                    MaxValue = (T)(object)ushort.MaxValue;
                }
                else if (typeof(T) == typeof(uint))
                {
                    MaxValue = (T)(object)uint.MaxValue;
                }
                else if (typeof(T) == typeof(float))
                {
                    MaxValue = (T)(object)1.0f;
                }
                else if (typeof(T) == typeof(double))
                {
                    MaxValue = (T)(object)1.0;
                }
            }
        };

        #endregion

        #region Table-Driven Query Methods

        public static class Name
        {
            public static readonly Symbol BW = "BW";
            public static readonly Symbol Gray = "Gray";
            public static readonly Symbol Alpha = "Alpha";

            public static readonly Symbol GrayAlpha = "GrayAlpha";

            public static readonly Symbol RG = "RG";
            public static readonly Symbol RGB = "RGB";
            public static readonly Symbol BGR = "BGR";
            public static readonly Symbol RGBA = "RGBA";
            public static readonly Symbol BGRA = "BGRA";
            public static readonly Symbol RGBP = "RGBP";
            public static readonly Symbol BGRP = "BGRP";

            // TODO:

            public static readonly Symbol CieXYZ = "CieXYZ";
            public static readonly Symbol CieYxy = "CieYxy";
            public static readonly Symbol CieLab = "CieLab";
            public static readonly Symbol CieLuv = "CieLuv";
            public static readonly Symbol Yuv = "Yuv";
            public static readonly Symbol HSL = "HSL";
            public static readonly Symbol HSV = "HSV";
        };

        public static class Intent
        {
            public static readonly Symbol BW = "BW";
            public static readonly Symbol Gray = "Gray";
            public static readonly Symbol Alpha = "Alpha";

            public static readonly Symbol GrayAlpha = "GrayAlpha";

            public static readonly Symbol RG = "RG";
            public static readonly Symbol RGB = "RGB";
            public static readonly Symbol BGR = "BGR";
            public static readonly Symbol RGBA = "RGBA";
            public static readonly Symbol BGRA = "BGRA";

            // TODO:

            public static readonly Symbol CieXYZ = "CieXYZ";
            public static readonly Symbol CieYxy = "CieYxy";
            public static readonly Symbol CieLab = "CieLab";
            public static readonly Symbol CieLuv = "CieLuv";
            public static readonly Symbol Yuv = "Yuv";
            public static readonly Symbol HSL = "HSL";
            public static readonly Symbol HSV = "HSV";
        };

        private static readonly (Col.Format, Symbol, Symbol, int)[] s_colFormatArray =
            new[]
            {
                (Format.BW,   Name.BW,   Intent.BW,   1),
                (Format.Gray, Name.Gray, Intent.Gray, 1),
                (Format.Alpha, Name.Alpha, Intent.Alpha, 1),

                (Format.GrayAlpha, Name.GrayAlpha, Intent.GrayAlpha, 2),

                (Format.RG,   Name.RG,   Intent.RG,   2),
                (Format.RGB,  Name.RGB,  Intent.RGB,  3),
                (Format.BGR,  Name.BGR,  Intent.BGR,  3),
                (Format.RGBA, Name.RGBA, Intent.RGBA, 4),
                (Format.BGRA, Name.BGRA, Intent.BGRA, 4),
                (Format.RGBP, Name.RGBP, Intent.RGBA, 4),
                (Format.BGRP, Name.BGRP, Intent.BGRA, 4),

                (Format.CieXYZ,  Name.CieXYZ,  Intent.CieXYZ,  3),
                (Format.CieYxy,  Name.CieYxy,  Intent.CieYxy,  3),
                (Format.CieLab,  Name.CieLab,  Intent.CieLab,  3),
                (Format.CieLuv,  Name.CieLuv,  Intent.CieLuv,  3),
                (Format.Yuv,  Name.Yuv,  Intent.Yuv,  3),
                (Format.HSL,  Name.HSL,  Intent.HSL,  3),
                (Format.HSV,  Name.HSV,  Intent.HSV,  3),

            };

        static Col()
        {
            var count = s_colFormatArray.Length;
            s_nameOfFormat = new Dict<Format, Symbol>(count);
            s_formatOfName = new SymbolDict<Format>(count);
            s_intentOfFormat = new Dict<Format, Symbol>(count);
            s_channelCountMap = new Dict<Format, int>(count);
            foreach (var t in s_colFormatArray)
            {
                var format = t.Item1; var name = t.Item2; var intent = t.Item3; var channelCount = t.Item4;
                s_nameOfFormat.Add(format, intent);
                s_formatOfName.Add(name, format);
                s_intentOfFormat.Add(format, intent);
                s_channelCountMap.Add(format, channelCount);
            }
        }

        private static readonly Dict<Format, Symbol> s_nameOfFormat = null;
        private static readonly SymbolDict<Format> s_formatOfName = null;
        private static readonly Dict<Format, Symbol> s_intentOfFormat = null;
        private static readonly Dict<Format, int> s_channelCountMap = null;

        public static Symbol GetName(this Format format)
        {
            return s_nameOfFormat[format];
        }

        public static Format FormatOfName(Symbol name)
        {
            return s_formatOfName[name];
        }

        public static Symbol GetIntent(this Format format)
        {
            return s_intentOfFormat[format];
        }

        public static bool IsPremultiplied(this Format format)
        {
            return format == Format.RGBP || format == Format.BGRP;
        }

        public static int ChannelCount(this Format format)
        {
            return s_channelCountMap[format];
        }

        private static readonly Dict<Format, Channel[]> s_formatChannelsMap =
            new Dict<Format, Channel[]>()
            {
                { Format.BW, new[] { Channel.BW } },
                { Format.Gray, new[] { Channel.Gray } },
                { Format.Alpha, new[] { Channel.Alpha } },

                { Format.GrayAlpha, new[] { Channel.Gray, Channel.Alpha } },

                { Format.RG, new[] { Channel.Red, Channel.Green } },
                { Format.RGB, new[] { Channel.Red, Channel.Green, Channel.Blue } },
                { Format.BGR, new[] { Channel.Blue, Channel.Green, Channel.Red } },
                { Format.RGBA, new[] { Channel.Red, Channel.Green, Channel.Blue, Channel.Alpha } },
                { Format.BGRA, new[] { Channel.Blue, Channel.Green, Channel.Red, Channel.Alpha } },
                { Format.RGBP, new[] { Channel.Red, Channel.Green, Channel.Blue, Channel.PremultipliedAlpha } },
                { Format.BGRP, new[] { Channel.Blue, Channel.Green, Channel.Red, Channel.PremultipliedAlpha } },

                { Format.CieXYZ, new[] { Channel.CieX, Channel.CieY, Channel.CieZ } },
                { Format.CieYxy, new[] { Channel.CieY, Channel.Ciex, Channel.Ciey } },
                { Format.CieLab, new[] { Channel.CieL, Channel.Ciea, Channel.Cieb } },
                { Format.CieLuv, new[] { Channel.CieL, Channel.Cieu, Channel.Ciev } },

                { Format.HSL, new[] { Channel.H, Channel.S, Channel.L } },
                { Format.HSV, new[] { Channel.H, Channel.S, Channel.V } },
                { Format.Yuv, new[] { Channel.Y, Channel.u, Channel.v } },
            };

        public static Channel[] ChannelsOfFormat(this Format format)
        {
            Channel[] channels;
            if (s_formatChannelsMap.TryGetValue(format, out channels))
                return channels;
            throw new ArgumentException($"Channels of format {format} not defined.");
        }

        private static readonly Dict<Format, long[]> s_channelOrderMap =
            new Dict<Format, long[]>()
            {
                { Format.BW, new[] { 0L } },
                { Format.Gray, new[] { 0L } },
                { Format.Alpha, new[] { 0L } },

                { Format.GrayAlpha, new[] { 0L, 1L } },

                { Format.RG, new[] { 0L, 1L } },
                { Format.RGB, new[] { 0L, 1L, 2L } },
                { Format.BGR, new[] { 2L, 1L, 0L } },
                { Format.RGBA, new[] { 0L, 1L, 2L, 3L } },
                { Format.BGRA, new[] { 2L, 1L, 0L, 3L } },
                { Format.RGBP, new[] { 0L, 1L, 2L, 3L } },
                { Format.BGRP, new[] { 2L, 1L, 0L, 3L } },

                { Format.CieXYZ, new[] { 0L, 1L, 2L } },
                { Format.CieYxy, new[] { 0L, 1L, 2L } },
                { Format.CieLab, new[] { 0L, 1L, 2L } },
                { Format.CieLuv, new[] { 0L, 1L, 2L } },

                { Format.HSL, new[] { 0L, 1L, 2L } },
                { Format.HSV, new[] { 0L, 1L, 2L } },
                { Format.Yuv, new[] { 0L, 1L, 2L } },
            };

        public static long[] ChannelOrder(this Format format)
        {
            return s_channelOrderMap[format];
        }

        private static readonly Dict<(Format, Channel), long> s_channelIndexMap =
            new Dict<(Format, Channel), long>()
            {
                { (Format.BW, Channel.BW), 0L },
                { (Format.Gray, Channel.Gray), 0L },
                { (Format.Alpha, Channel.Alpha), 0L },

                { (Format.GrayAlpha, Channel.Gray), 0L },
                { (Format.GrayAlpha, Channel.Alpha), 1L },

                { (Format.RG, Channel.Red), 0L },
                { (Format.RG, Channel.Green), 1L },

                { (Format.RGB, Channel.Red), 0L },
                { (Format.RGB, Channel.Green), 1L },
                { (Format.RGB, Channel.Blue), 2L },

                { (Format.BGR, Channel.Blue), 0L },
                { (Format.BGR, Channel.Green), 1L },
                { (Format.BGR, Channel.Red), 2L },

                { (Format.RGBA, Channel.Red), 0L },
                { (Format.RGBA, Channel.Green), 1L },
                { (Format.RGBA, Channel.Blue), 2L },
                { (Format.RGBA, Channel.Alpha), 3L },

                { (Format.BGRA, Channel.Blue), 0L },
                { (Format.BGRA, Channel.Green), 1L },
                { (Format.BGRA, Channel.Red), 2L },
                { (Format.BGRA, Channel.Alpha), 3L },

                { (Format.RGBP, Channel.Red), 0L },
                { (Format.RGBP, Channel.Green), 1L },
                { (Format.RGBP, Channel.Blue), 2L },
                { (Format.RGBP, Channel.PremultipliedAlpha), 3L },

                { (Format.BGRP, Channel.Blue), 0L },
                { (Format.BGRP, Channel.Green), 1L },
                { (Format.BGRP, Channel.Red), 2L },
                { (Format.BGRP, Channel.PremultipliedAlpha), 3L },

                { (Format.CieXYZ, Channel.CieX), 0L },
                { (Format.CieXYZ, Channel.CieY), 1L },
                { (Format.CieXYZ, Channel.CieZ), 2L },

                { (Format.CieYxy, Channel.CieY), 0L },
                { (Format.CieYxy, Channel.Ciex), 1L },
                { (Format.CieYxy, Channel.Ciey), 2L },

                { (Format.CieLab, Channel.CieL), 0L },
                { (Format.CieLab, Channel.Ciea), 1L },
                { (Format.CieLab, Channel.Cieb), 2L },

                { (Format.CieLuv, Channel.CieL), 0L },
                { (Format.CieLuv, Channel.Cieu), 1L },
                { (Format.CieLuv, Channel.Ciev), 2L },

                { (Format.HSL, Channel.H), 0L },
                { (Format.HSL, Channel.S), 1L },
                { (Format.HSL, Channel.L), 2L },

                { (Format.HSV, Channel.H), 0L },
                { (Format.HSV, Channel.S), 1L },
                { (Format.HSV, Channel.V), 2L },

                { (Format.Yuv, Channel.Y), 0L },
                { (Format.Yuv, Channel.u), 1L },
                { (Format.Yuv, Channel.v), 2L },
            };


        public static long ChannelIndex(this Format format, Channel channel)
        {
            if (s_channelIndexMap.TryGetValue((format, channel),
                                              out long index))
                return index;
            throw new ArgumentException($"Format {format} does not contain channel {channel}.");
        }

        public static long ChannelIndexNoThrow(this Format format, Channel channel)
        {
            long index;
            if (s_channelIndexMap.TryGetValue((format, channel),
                                              out index))
                return index;
            return -1;
        }

        public static Format FormatDefaultOf(this Type type, long channelCount)
        {
            return channelCount switch
            {
                1 => Format.Gray,
                2 => Format.RG,
                3 => (type == typeof(byte)) ? Format.BGR : Format.RGB,
                4 => (type == typeof(byte)) ? Format.BGRA : Format.RGBA,
                _ => throw new ArgumentException($"No default format for {type} with {channelCount} channels"),
            };
        }

        #endregion

        #region Color Channel Conversion Constants

        private const float c_floatToByte = 255.9999f;
        private const float c_byteToFloat = 1.0f / 255.0f;

        private const double c_doubleToByte = 255.9999999999999;
        private const double c_byteToDouble = 1.0 / 255.0;

        private const float c_floatToUShort = 65535.99f;
        private const float c_uShortToFloat = 1.0f / 65535.0f;

        private const double c_doubleToUShort = 65535.99999999999;
        private const double c_uShortToDouble = 1.0 / 65535.0;

        private const double c_floatToUIntAsDouble = 4294967295.999999;
        private const double c_uIntToFloatAsDouble = 1.0 / 4294967295.0;

        private const double c_doubleToUInt = 4294967295.999999;
        private const double c_uIntToDouble = 1.0 / 4294967295.0;

        #endregion

        #region Color Channel Conversions between Byte, UShort, UInt, float, and double

        #region Byte

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ByteToUShort(this byte b) => (ushort)(257 * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ByteToUInt(this byte b) => 0x1010101u * (uint)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ByteToHalf(this byte b) => (Half)(c_byteToFloat * (float)b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ByteToFloat(this byte b) => c_byteToFloat * (float)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ByteToDouble(this byte b) => c_byteToDouble * (double)b;

        #endregion

        #region UShort

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte UShortToByte(this ushort us) => (byte)(us >> 8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UShortToUInt(this ushort us) => 65537u * (uint)us;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half UShortToHalf(this ushort us) => (Half)(c_uShortToFloat * (float)us);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float UShortToFloat(this ushort us) => c_uShortToFloat * (float)us;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UShortToDouble(this ushort us) => c_uShortToDouble * (double)us;

        #endregion

        #region UInt

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte UIntToByte(this uint ui) => (byte)(ui >> 24);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort UIntToUShort(this uint ui) => (ushort)(ui >> 16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half UIntToHalf(this uint ui) => (Half)(c_uIntToFloatAsDouble * ui);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float UIntToFloat(this uint ui) => (float)(c_uIntToFloatAsDouble * ui);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UIntToDouble(this uint ui) => c_uIntToDouble * (double)ui;

        #endregion

        #region Half

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HalfToByte(this Half f) => (byte)(c_floatToByte * f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort HalfToUShort(this Half f) => (ushort)(c_floatToUShort * f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint HalfToUInt(this Half f) => (uint)(c_floatToUIntAsDouble * f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float HalfToFloat(this Half f) => (float)f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double HalfToDouble(this Half f) => (double)f;

        #endregion

        #region Float

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte FloatToByte(this float f) => (byte)(c_floatToByte * f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte FloatToByteClamped(this float f) => (byte)(c_floatToByte * f).Clamp(0, 255);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort FloatToUShort(this float f) => (ushort)(c_floatToUShort * f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort FloatToUShortClamped(this float f) => (ushort)(c_floatToUShort * f).Clamp(0, 65535);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FloatToUInt(this float f) => (uint)(c_floatToUIntAsDouble * f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FloatToUIntClamped(this float f) => (uint)(c_floatToUIntAsDouble * f).Clamp(0, 4294967295);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half FloatToHalf(this float f) => (Half)f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double FloatToDouble(this float f) => (double)f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double FloatToDoubleClamped(this float f) => ((double)f).Clamp(0, 1);

        #endregion

        #region Double

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte DoubleToByte(this double d) => (byte)(c_doubleToByte * d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte DoubleToByteClamped(this double d) => (byte)(c_doubleToByte * d).Clamp(0, 255);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort DoubleToUShort(this double d) => (ushort)(c_doubleToUShort * d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort DoubleToUShortClamped(this double d) => (ushort)(c_doubleToUShort * d).Clamp(0, 65535);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint DoubleToUInt(this double d) => (uint)(c_doubleToUInt * d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint DoubleToUIntClamped(this double d) => (uint)(c_doubleToUInt * d).Clamp(0, 4294967295);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half DoubleToHalf(this double d) => (Half)d;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DoubleToFloat(this double d) => (float)d;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DoubleToFloatClamped(this double d) => ((float)d).Clamp(0, 1);

        #endregion

        #endregion

        #region Special Color Channel Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ByteInIntToFloat(this int i) { return (float)i * c_byteToFloat; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloatToByteInInt(this float f) { return (int)(f * c_floatToByte); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ByteInIntToDouble(this int i) { return (double)i * c_byteToDouble; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DoubleToByteInInt(this double d) { return (int)(d * c_doubleToByte); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UShortInIntToDouble(this int i) { return (double)i * c_uShortToDouble; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DoubleToUShortInInt(this double d) { return (int)(d * c_doubleToUShort); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte BitInByteToByte(this byte b) { return (byte)(b * 255); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ByteToBitInByte(this byte b) { return (byte)(b >> 7); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ByteInFloatToByte(this float f) { return (byte)(f + 0.5f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ByteInDoubleToByte(this double d) { return (byte)(d + 0.5); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort UShortInFloatToUShort(this float f) { return (ushort)(f + 0.5f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort UShortInDoubleToUShort(this double d) { return (ushort)(d + 0.5); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UIntInDoubleToUInt(this double d) { return (uint)(d + 0.5); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloatToByteInIntClamped(this float f) { return (int)(f * c_floatToByte).Clamp(0, 255); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DoubleToUShortInIntClamped(this double d) { return (int)(d * c_doubleToUShort).Clamp(0, 65535); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ByteInFloatToByteClamped(this float f) { return (byte)(f.Clamp(0.0f, 255.0f) + 0.5f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ByteInDoubleToByteClamped(this double d) { return (byte)(d.Clamp(0.0, 255.0) + 0.5); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort UShortInFloatToUShortClamped(this float f) { return (ushort)(f.Clamp(0.0f, 65535.0f) + 0.5f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort UShortInDoubleToUShortClamped(this double d) { return (ushort)(d.Clamp(0.0, 65535.0) + 0.5); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UIntInDoubleToUIntClamped(this double d) { return (uint)(d.Clamp(0.0, (double)uint.MaxValue) + 0.5); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ByteInFloatToFloat(this float f) { return f * c_byteToFloat; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float FloatToByteInFloat(this float f) { return f * c_floatToByte; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ByteInDoubleToDouble(this double d) { return d * c_byteToDouble; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DoubleToByteInDouble(this double d) { return d * c_doubleToByte; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ByteInFloatToFloatClamped(this float f) { return f.ByteInFloatToFloat().Clamp(0, 1); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float FloatToByteInFloatClamped(this float f) { return f.FloatToByteInFloat().Clamp(0, 255); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ByteInDoubleToDoubleClamped(this double d) { return d.ByteInDoubleToDouble().Clamp(0, 1); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DoubleToByteInDoubleClamped(this double d) { return d.DoubleToByteInDouble().Clamp(0, 255); }

        #endregion

        #region Color to Gray Conversion Constants

        private const double c_grayDoubleRed   = 0.2989;
        private const double c_grayDoubleGreen = 0.5870;
        private const double c_grayDoubleBlue  = 0.1140;

        private const float c_grayFloatRed     = (float)c_grayDoubleRed;
        private const float c_grayFloatGreen   = (float)c_grayDoubleGreen;
        private const float c_grayFloatBlue    = (float)c_grayDoubleBlue;

        // 65793 == 65536 * 255.99998/255
        private const int c_grayByteRed   = (int)(65793 * c_grayDoubleRed);
        private const int c_grayByteGreen = (int)(65793 * c_grayDoubleGreen);
        private const int c_grayByteBlue  = (int)(65793 * c_grayDoubleBlue);

        // 4295032833 == 4294967296 * 65535.99999999999/65535
        private const long c_grayUShortRed   = (long)(4295032833L * c_grayDoubleRed);
        private const long c_grayUShortGreen = (long)(4295032833L * c_grayDoubleGreen);
        private const long c_grayUShortBlue  = (long)(4295032833L * c_grayDoubleBlue);

        #endregion

        #region Color to Gray Conversions

        #region Byte

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToGrayByte(byte r, byte g, byte b)
            => (byte)((r * c_grayByteRed + g * c_grayByteGreen + b * c_grayByteBlue) >> 16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToGrayByte(this C3b c) => ToGrayByte(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToGrayByte(this C4b c) => ToGrayByte(c.R, c.G, c.B);

        #endregion

        #region UShort

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToGrayUShort(ushort r, ushort g, ushort b)
            => (ushort)((r * c_grayUShortRed + g * c_grayUShortGreen + b * c_grayUShortBlue) >> 32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToGrayUShort(this C3us c) => ToGrayUShort(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToGrayUShort(this C4us c) => ToGrayUShort(c.R, c.G, c.B);

        #endregion

        #region UInt

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToGrayUInt(uint r, uint g, uint b)
            => DoubleToUInt(ToGrayDoubleClamped(UIntToDouble(r), UIntToDouble(g), UIntToDouble(b)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToGrayUInt(this C3ui c) => ToGrayUInt(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToGrayUInt(this C4ui c) => ToGrayUInt(c.R, c.G, c.B);

        #endregion

        #region Half

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ToGrayHalf(Half r, Half g, Half b)
            => (Half)ToGrayFloat(r, g, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ToGrayHalfClamped(Half r, Half g, Half b)
            => (Half)ToGrayFloatClamped(r, g, b);

        #endregion

        #region Float

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToGrayFloat(float r, float g, float b)
            => r * c_grayFloatRed + g * c_grayFloatGreen + b * c_grayFloatBlue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToGrayFloat(this C3f c) => ToGrayFloat(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToGrayFloat(this C4f c) => ToGrayFloat(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToGrayFloatClamped(float r, float g, float b)
            => ToGrayFloat(r, g, b).Clamp(0, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToGrayFloatClamped(this C3f c) => ToGrayFloatClamped(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToGrayFloatClamped(C4f c) => ToGrayFloatClamped(c.R, c.G, c.B);

        #endregion

        #region Double

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToGrayDouble(double r, double g, double b)
            => r * c_grayDoubleRed + g * c_grayDoubleGreen + b * c_grayDoubleBlue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToGrayDouble(this C3d c) => ToGrayDouble(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToGrayDouble(this C4d c) => ToGrayDouble(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToGrayDoubleClamped(double r, double g, double b)
            => ToGrayDouble(r, g, b).Clamp(0, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToGrayDoubleClamped(this C3d c) => ToGrayDoubleClamped(c.R, c.G, c.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToGrayDoubleClamped(C4d c) => ToGrayDoubleClamped(c.R, c.G, c.B);

        #endregion

        #endregion

        #region Alpha to premultiplied Alpha and back

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b AlphaToPremultipliedAlpha(this C4b c)
        {
            var f = c_byteToDouble * (double)c.A;
            return new C4b((byte)(f * c.R), (byte)(f * c.G), (byte)(f * c.B), c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b PremultipliedAlphaToAlpha(this C4b c)
        {
            if (c.A == 0) return new C4b(0, 0, 0, 0);
            var f = 1.0 / (c_byteToDouble * (double)c.A);
            return new C4b((byte)(f * c.R), (byte)(f * c.G), (byte)(f * c.B), c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us AlphaToPremultipliedAlpha(this C4us c)
        {
            var f = c_uShortToDouble * (double)c.A;
            return new C4us((ushort)(f * c.R), (ushort)(f * c.G), (ushort)(f * c.B), c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us PremultipliedAlphaToAlpha(this C4us c)
        {
            if (c.A == 0) return new C4us(0, 0, 0, 0);
            var f = 1.0 / (c_uShortToDouble * (double)c.A);
            return new C4us((ushort)(f * c.R), (ushort)(f * c.G), (ushort)(f * c.B), c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui AlphaToPremultipliedAlpha(this C4ui c)
        {
            var f = c_uIntToDouble * (double)c.A;
            return new C4ui((uint)(f * c.R), (uint)(f * c.G), (uint)(f * c.B), c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui PremultipliedAlphaToAlpha(this C4ui c)
        {
            if (c.A == 0) return new C4ui(0, 0, 0, 0);
            var f = 1.0 / (c_uIntToDouble * (double)c.A);
            return new C4ui((uint)(f * c.R), (uint)(f * c.G), (uint)(f * c.B), c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f AlphaToPremultipliedAlpha(this C4f c)
        {
            return new C4f(c.R * c.A, c.G * c.A, c.B * c.A, c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f PremultipliedAlphaToAlpha(this C4f c)
        {
            if (c.A == 0.0f) return new C4f(0.0f, 0.0f, 0.0f, 0.0f);
            return new C4f(c.R / c.A, c.G / c.A, c.B / c.A, c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d AlphaToPremultipliedAlpha(this C4d c)
        {
            return new C4d(c.R * c.A, c.G * c.A, c.B * c.A, c.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d PremultipliedAlphaToAlpha(this C4d c)
        {
            if (c.A == 0.0) return new C4d(0.0, 0.0, 0.0, 0.0);
            return new C4d(c.R / c.A, c.G / c.A, c.B / c.A, c.A);
        }

        #endregion

        #region SRGB/Linear SRGB/CieXYZ/CieLab/CieYxy Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LinearSRGBFloatToSRGBFloat(this float c)
            => (float)(c <= 0.0031308 ? c * 12.92 : 1.055 * Fun.Pow(c, 1.0 / 2.4) - 0.055);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SRGBFloatToLinearSRGBFloat(this float c)
            => (float)(c <= 0.04045 ? c / 12.92 : Fun.Pow((c + 0.055) / 1.055, 2.4));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinearSRGBToSRGB(this C3f c)
        {
            return new C3f(c.R <= 0.0031308 ? c.R * 12.92 : 1.055 * Fun.Pow(c.R, 1.0 / 2.4) - 0.055,
                           c.G <= 0.0031308 ? c.G * 12.92 : 1.055 * Fun.Pow(c.G, 1.0 / 2.4) - 0.055,
                           c.B <= 0.0031308 ? c.B * 12.92 : 1.055 * Fun.Pow(c.B, 1.0 / 2.4) - 0.055);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinearSRGBAToSRGBA(this C4f c)
        {
            return new C4f(c.R <= 0.0031308 ? c.R * 12.92 : 1.055 * Fun.Pow(c.R, 1.0 / 2.4) - 0.055,
                           c.G <= 0.0031308 ? c.G * 12.92 : 1.055 * Fun.Pow(c.G, 1.0 / 2.4) - 0.055,
                           c.B <= 0.0031308 ? c.B * 12.92 : 1.055 * Fun.Pow(c.B, 1.0 / 2.4) - 0.055,
                           c.A <= 0.0031308 ? c.A * 12.92 : 1.055 * Fun.Pow(c.A, 1.0 / 2.4) - 0.055);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f SRGBToLinearSRGB(this C3f c)
        {
            return new C3f(c.R <= 0.04045 ? c.R / 12.92 : Fun.Pow((c.R + 0.055) / 1.055, 2.4),
                           c.G <= 0.04045 ? c.G / 12.92 : Fun.Pow((c.G + 0.055) / 1.055, 2.4),
                           c.B <= 0.04045 ? c.B / 12.92 : Fun.Pow((c.B + 0.055) / 1.055, 2.4));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f SRGBAToLinearSRGBA(this C4f c)
        {
            return new C4f(c.R <= 0.04045 ? c.R / 12.92 : Fun.Pow((c.R + 0.055) / 1.055, 2.4),
                           c.G <= 0.04045 ? c.G / 12.92 : Fun.Pow((c.G + 0.055) / 1.055, 2.4),
                           c.B <= 0.04045 ? c.B / 12.92 : Fun.Pow((c.B + 0.055) / 1.055, 2.4),
                           c.A <= 0.04045 ? c.A / 12.92 : Fun.Pow((c.A + 0.055) / 1.055, 2.4));
        }

        /// <summary>
        /// Convert linear SRGB with channel values in the range [0.0, 1.0] to XYZ.
        /// SRGB white (1, 1, 1) is converted to XYZ white (0.9505, 1.0000, 1.0890)
        /// at D65 with unit luminance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinearSRGBToXYZinC3f(this C3f c)
        {
            return new C3f(0.4124 * c.R + 0.3576 * c.G + 0.1805 * c.B,
                           0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B,
                           0.0193 * c.R + 0.1192 * c.G + 0.9505 * c.B);
        }

        /// <summary>
        /// Convert XYZ to linear SRGB with channel values in the range [0.0, 1.0].
        /// XYZ white (0.9505, 1.0000, 1.0890) at D65 with unit luminance is
        /// converted to SRGB white (1, 1, 1)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f XYZinC3fToLinearSRGB(this C3f c)
        {
            return new C3f(3.2406 * c.R - 1.5372 * c.G - 0.4986 * c.B,
                          -0.9689 * c.R + 1.8758 * c.G + 0.0415 * c.B,
                           0.0557 * c.R - 0.2040 * c.G + 1.0570 * c.B);
        }

        /// <summary>
        /// Convert XYZ to CIE RGB with channel values in the range [0.0, 1.0].
        /// XYZ white (1, 1, 1) is converted to (1, 1, 1).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f XYZinC3fToCIERGB(this C3f c)
        {
            return new C3f( 2.3706743 * c.R - 0.9000405 * c.G - 0.4706338 * c.B,
                           -0.5138850 * c.R + 1.4253036 * c.G + 0.0885814 * c.B,
                            0.0052982 * c.R - 0.0146949 * c.G + 1.0093968 * c.B);
        }

        /// <summary>
        /// Convert SRGB with channel values in the range [0.0, 1.0] to XYZ.
        /// SRGB white (1, 1, 1) is converted to XYZ white (0.9505, 1.0000, 1.0890)
        /// at D65 with unit luminance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f SRGBToXYZinC3f(this C3f c) { return c.SRGBToLinearSRGB().LinearSRGBToXYZinC3f(); }

        /// <summary>
        /// Convert XYZ to SRGB with channel values in the range [0.0, 1.0].
        /// XYZ white (0.9505, 1.0000, 1.0890) at D65 with unit luminance is
        /// converted to SRGB white (1, 1, 1)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f XYZinC3fToSRGB(this C3f c) { return c.XYZinC3fToLinearSRGB().LinearSRGBToSRGB(); }

        /// <summary>
        /// Convert linear SRGBA with channel values in the range [0.0, 1.0] to XYZA.
        /// SRGB white (1, 1, 1) is converted to XYZA white (0.9505, 1.0000, 1.0890)
        /// at D65 with unit luminance. The alpha value is simply copied.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinearSRGBAToXYZAinC4f(this C4f c)
        {
            return new C4f(0.4124 * c.R + 0.3576 * c.G + 0.1805 * c.B,
                           0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B,
                           0.0193 * c.R + 0.1192 * c.G + 0.9505 * c.B,
                           c.A);
        }

        /// <summary>
        /// Convert XYZA to linear SRGBA with channel values in the range [0.0, 1.0].
        /// XYZ white (0.9505, 1.0000, 1.0890) at D65 with unit luminance is
        /// converted to SRGB white (1, 1, 1). The alpha value is simply copied.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f XYZAinC4fToLinearSRGBA(this C4f c)
        {
            return new C4f(3.2410 * c.R - 1.5374 * c.G - 0.4986 * c.B,
                          -0.9692 * c.R + 1.876  * c.G + 0.0416 * c.B,
                           0.0556 * c.R - 0.204  * c.G + 1.057  * c.B,
                           c.A);
        }

        /// <summary>
        /// Convert SRGBA with channel values in the range [0.0, 1.0] to XYZA.
        /// SRGBA white (1, 1, 1) is converted to XYZA white (0.9505, 1.0000, 1.0890)
        /// at D65 with unit luminance. The alpha value is subject to sRGB de-gamma
        /// conversion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f SRGBAToXYZAinC4f(this C4f c) { return c.SRGBAToLinearSRGBA().LinearSRGBAToXYZAinC4f(); }

        /// <summary>
        /// Convert XYZA to SRGBA with channel values in the range [0.0, 1.0].
        /// XYZA white (0.9505, 1.0000, 1.0890) at D65 with unit luminance is
        /// converted to SRGBA white (1, 1, 1). The alpha value is subject to
        /// sRGB gamma conversion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f XYZAinC4fToSRGBA(this C4f c) { return c.XYZAinC4fToLinearSRGBA().LinearSRGBAToSRGBA(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf XYZinC3fToCieXYZf(this C3f c) { return new CieXYZf(c.R * 100.0, c.G * 100.0, c.B * 100.0); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf XYZAinC4fToCieXYZf(this C4f c) { return new CieXYZf(c.R * 100.0, c.G * 100.0, c.B * 100.0); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf LinearSRGBToCieXYZf(this C3f c) { return c.LinearSRGBToXYZinC3f().XYZinC3fToCieXYZf(); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf LinearSRGBAToCieXYZf(this C4f c) { return c.LinearSRGBAToXYZAinC4f().XYZAinC4fToCieXYZf(); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf SRGBToCieXYZf(this C3f c) { return c.SRGBToXYZinC3f().XYZinC3fToCieXYZf(); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf SRGBAToCieXYZf(this C4f c) { return c.SRGBAToXYZAinC4f().XYZAinC4fToCieXYZf(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f ToXYZinC3f(this CieXYZf c) { return new C3f(c.X * 0.01f, c.Y * 0.01f, c.Z * 0.01f); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f ToXYZAinC4f(this CieXYZf c) { return new C4f(c.X * 0.01f, c.Y * 0.01f, c.Z * 0.01f); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f ToLinearSRGB(this CieXYZf c) { return c.ToXYZinC3f().XYZinC3fToLinearSRGB(); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f ToLinearSRGBA(this CieXYZf c) { return c.ToXYZAinC4f().XYZAinC4fToLinearSRGBA(); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f ToSRGB(this CieXYZf c) { return c. ToXYZinC3f().XYZinC3fToSRGB(); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f ToSRGBA(this CieXYZf c) { return c.ToXYZAinC4f().XYZAinC4fToSRGBA(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieYxyf ToCieYxyf(this CieXYZf c)
        {
            var s = c.X + c.Y + c.Z; return s > 0.0f ? new CieYxyf(c.Y, c.X / s, c.Y / s) : new CieYxyf(0, 0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf ToCieXYZf(this CieYxyf c)
        {
            var s = c.y > 0.0 ? c.Y / c.y : 0.0; return new CieXYZf(s * c.x, c.Y, s * (1 - c.x - c.y));
        }

        private const double s_labF_t3 = (6 / 29.0) * (6 / 29.0) * (6 / 29.0);
        private const double s_labF_t1 = (1 / 3.0) * (29 / 6.0) * (29 / 6.0);
        private const double s_labF_t0 = 4 / 29.0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double s_labF(double t) { return t > s_labF_t3 ? Math.Pow(t, Constant.OneThird) : s_labF_t1 * t + s_labF_t0; }

        private const double s_labFinv_t3 = 6 / 29.0;
        private const double s_labFinv_t1 = 3.0 * (6 / 29.0) * (6 / 29.0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double s_labFinv(double t) { return t > s_labFinv_t3 ? t * t * t : s_labFinv_t1 * (t - s_labF_t0); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieLabf ToCieLabf(this CieXYZf c, CieXYZf white)
        {
            var fy = s_labF(c.Y / white.Y);
            return new CieLabf( 116.0 * fy - 16.0, 500.0 * (s_labF(c.X / white.X) - fy), 200.0 * (fy - s_labF(c.Z / white.Z)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf ToCieXYZf(this CieLabf c, CieXYZf white)
        {
            var l = (c.L + 16.0) / 116.0;
            return new CieXYZf(white.Y * s_labFinv(l), white.X * s_labFinv(l + c.a * 0.002), white.Z * s_labFinv(l - c.b * 0.005));
        }

        public static readonly Trafo2d ConeResponseDomain_XYZScaling = new Trafo2d(M33d.Identity, M33d.Identity);

        public static readonly Trafo2d ConeResponseDomain_Bradford = new Trafo2d(new M33d(0.8951000, 0.2664000, -0.1614000,
                                                                                         -0.7502000, 1.7135000, 0.0367000,
                                                                                          0.0389000, -0.0685000, 1.0296000),
                                                                                 new M33d(0.9869929, -0.1470543, 0.1599627,
                                                                                          0.4323053, 0.5183603, 0.0492912,
                                                                                         -0.0085287, 0.0400428, 0.9684867));

        public static readonly Trafo2d ConeResponseDomain_VonKries = new Trafo2d(new M33d(0.4002400, 0.7076000, -0.0808100,
                                                                                         -0.2263000, 1.1653200, 0.0457000,
                                                                                          0.0000000, 0.0000000, 0.9182200),
                                                                                 new M33d(1.8599364, -1.1293816, 0.2198974,
                                                                                          0.3611914, 0.6388125, -0.0000064,
                                                                                          0.0000000, 0.0000000, 1.0890636));

        public static readonly CieXYZf CieXYZfAWhite = new CieXYZf(109.85, 100.0, 35.585);
        public static readonly CieXYZf CieXYZfBWhite = new CieXYZf(99.072, 100.0, 85.223);
        public static readonly CieXYZf CieXYZfCWhite = new CieXYZf(98.074, 100.0, 118.232);
        public static readonly CieXYZf CieXYZfD50White = new CieXYZf(96.422, 100.0, 82.521);
        public static readonly CieXYZf CieXYZfD55White = new CieXYZf(95.682, 100.0, 92.149);
        public static readonly CieXYZf CieXYZfD65White = new CieXYZf(95.047, 100.0, 108.883);
        public static readonly CieXYZf CieXYZfD75White = new CieXYZf(94.972, 100.0, 122.638);
        public static readonly CieXYZf CieXYZfEWhite = new CieXYZf(100.0, 100.0, 100.0);
        public static readonly CieXYZf CieXYZfF2White = new CieXYZf(99.186, 100.00, 67.393);
        public static readonly CieXYZf CieXYZfF7White = new CieXYZf(95.041, 100.00, 108.747);
        public static readonly CieXYZf CieXYZfF11White = new CieXYZf(100.962, 100.00, 64.350);

        public static M33d BuildChromaticAdaptationMatrix(CieXYZf whiteFrom, CieXYZf whiteTo, Trafo2d coneResponseDomain)
        {
            // see: http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html

            var responseSource = coneResponseDomain.Forward * new V3d(whiteFrom.X, whiteFrom.Y, whiteFrom.Z);
            var responseTarget = coneResponseDomain.Forward * new V3d(whiteTo.X, whiteTo.Y, whiteTo.Z);
            var det = responseTarget / responseSource;
            var scaleMat = new M33d(det.X, 0, 0,
                                    0, det.Y, 0,
                                    0, 0, det.Z);
            return coneResponseDomain.Backward * scaleMat * coneResponseDomain.Forward;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieLabf ToCieLabfD65(this CieXYZf c) { return ToCieLabf(c, CieXYZfD65White); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CieXYZf ToCieXYZfD65(this CieLabf c) { return ToCieXYZf(c, CieXYZfD65White); }

        #endregion

        #region HSL and HSV Conversions (see http://en.wikipedia.org/wiki/HSL_and_HSV)

        public static HSLf ToHSLf(this C3f c)
        {
            double cr = 0.0, h6 = 0.0, l = 0.0;
            if (c.R > c.G)
            {
                if (c.R > c.B)
                {
                    if (c.G > c.B)  { cr = c.R - c.B; l = 0.5 * (c.R + c.B); }  // R > G > B
                    else            { cr = c.R - c.G; l = 0.5 * (c.R + c.G); }  // R > B >= G
                    h6 = 0 + (c.G - c.B) / cr;
                }
                else                                                            // B >= R > G
                {
                    cr = c.B - c.G; l = 0.5 * (c.B + c.G);
                    h6 = 4 + (c.R - c.G) / cr;
                }
            }
            else                                                                // G >= R
            {
                if (c.G > c.B)
                {
                    if (c.R > c.B)  { cr = c.G - c.B; l = 0.5 * (c.G + c.B); }  // G >= R > B
                    else            { cr = c.G - c.R; l = 0.5 * (c.G + c.R); }  // G > B >= R
                    h6 = 2 + (c.B - c.R) / cr;
                }
                else                                                            // B >= G >= R
                {
                    cr = c.B - c.R; l = 0.5 * (c.B + c.R);
                    if (cr > 0.0) h6 = 4 + (c.R - c.G) / cr;
                }
            }
            return new HSLf(Fun.Frac(h6 * 1.0 / 6.0),
                            cr > Constant<float>.PositiveTinyValue
                                ? cr / (1.0 - Fun.Abs(2.0 * l - 1.0)) : 0.0,
                            l);
        }

        public static HSVf ToHSVf(this C3f c)
        {
            double cr = 0.0, h6 = 0.0, v = 0.0;
            if (c.R > c.G)
            {
                if (c.R > c.B)
                {
                    if (c.G > c.B)  cr = c.R - c.B;     // R > G > B
                    else            cr = c.R - c.G;     // R > B >= G
                    v = c.R; h6 = 0 + (c.G - c.B) / cr;
                }
                else                                    // B >= R > G
                {
                    cr = c.B - c.G; v = c.B;
                    h6 = 4 + (c.R - c.G) / cr;
                }
            }
            else                                        // G >= R
            {
                if (c.G > c.B)
                {
                    if (c.R > c.B)  cr = c.G - c.B;     // G >= R > B
                    else            cr = c.G - c.R;     // G > B >= R
                    v = c.G; h6 = 2 + (c.B - c.R) / cr;
                }
                else                                    // B >= G >= R
                {
                    cr = c.B - c.R; v = c.B;
                    if (cr > 0.0) h6 = 4 + (c.R - c.G) / cr;
                }
            }
            return new HSVf(Fun.Frac(h6 * 1.0 / 6.0),
                            cr > Constant<float>.PositiveTinyValue ? cr / v : 0.0,
                            v);
        }

        public static C3f ToC3f(this HSLf c)
        {
            double h = Fun.Frac(c.H);
            var chr = (1.0 - Fun.Abs(2.0 * c.L - 1.0)) * c.S;
            var x = chr * (1.0 - Fun.Abs(Fun.Frac(h * 3.0) * 2.0 - 1.0));
            var m = c.L - 0.5 * chr;
            switch ((int)(h * 6.0))
            {
                case 0: return new C3f(chr + m, x + m, m);
                case 1: return new C3f(x + m, chr + m, m);
                case 2: return new C3f(m, chr + m, x + m);
                case 3: return new C3f(m, x + m, chr + m);
                case 4: return new C3f(x + m, m, chr + m);
                case 5: return new C3f(chr + m, m, x + m);
                default: throw new ArgumentException("will never occur");
            }
        }

        public static C3f ToC3f(this HSVf c)
        {
            double h = Fun.Frac(c.H);
            var chr = c.V * c.S;
            var x = chr * (1.0 - Fun.Abs(Fun.Frac(h * 3.0) * 2.0 - 1.0));
            var m = c.V - chr;
            switch ((int)(h * 6.0))
            {
                case 0: return new C3f(chr + m, x + m, m);
                case 1: return new C3f(x + m, chr + m, m);
                case 2: return new C3f(m, chr + m, x + m);
                case 3: return new C3f(m, x + m, chr + m);
                case 4: return new C3f(x + m, m, chr + m);
                case 5: return new C3f(chr + m, m, x + m);
                default: throw new ArgumentException("will never occur");
            }
        }

        #endregion

        #region Gamma Correction Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GammaFloatToLinearFloat(float c, double gamma)
        {
            return (float)Fun.Pow(c, gamma);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LinearFloatToGammaFloat(float c, double gamma)
        {
            double inverseGamma = 1.0 / gamma;
            return (float)Fun.Pow(c, inverseGamma);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f GammaToLinear(this C3f c, double gamma)
        {
            return new C3f(Fun.Pow(c.R, gamma), Fun.Pow(c.G, gamma), Fun.Pow(c.B, gamma));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f GammaToLinear(this C4f c, double gamma)
        {
            return new C4f(Fun.Pow(c.R, gamma), Fun.Pow(c.G, gamma),
                           Fun.Pow(c.B, gamma), Fun.Pow(c.A, gamma));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinearToGamma(this C3f c, double gamma)
        {
            double inv = 1.0 / gamma;
            return new C3f(Fun.Pow(c.R, inv), Fun.Pow(c.G, inv), Fun.Pow(c.B, inv));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinearToGamma(this C4f c, double gamma)
        {
            double inv = 1.0 / gamma;
            return new C4f(Fun.Pow(c.R, inv), Fun.Pow(c.G, inv),
                           Fun.Pow(c.B, inv), Fun.Pow(c.A, inv));
        }

        #endregion

        #region Color Temperature

        public static C3f TemperatureToYuvInC3f(this double t)
        {
            // calculation from:
            // http://en.wikipedia.org/wiki/Planckian_locus

            var t2 = t.Square();
            return new C3f(
                100,
                (0.860117757 + 1.54118254e-4 * t + 1.28641212e-7 * t2)
                / (1 + 8.42420235e-4 * t + 7.08145163e-7 * t2),         // u: error within 8e-5 from 1000K < T < 15000K
                (0.317398726 + 4.22806245e-5 * t + 4.20481691e-8 * t2)
                / (1 - 2.89741816e-5 * t + 1.61456053e-7 * t2));        // v: error within 9e-5 from 1000K < T < 15000K
        }

        public static C3f TemperatureToYxyInC3f(this double t)
        {
            // calculation from:
            // http://en.wikipedia.org/wiki/Planckian_locus
            // defined from 1667 <= T <= 25000

            if (t <= 0) return new C3f(0, 0, 0);

            var t2 = t * t;
            var t3 = t2 * t;
            var x = t <= 4000 ? -0.2661239e9/t3 - 0.2343580e6/t2 + 0.8776956e3/t + 0.179910
                              : -3.0258469e9/t3 + 2.1070379e6/t2 + 0.2226347e3/t + 0.240390;
            var x2 = x * x;
            var x3 = x2 * x;
            var y = t <= 2222 ? -1.1063814*x3 - 1.34811020*x2 + 2.18555832*x - 0.20219683
                  : t <= 4000 ? -0.9549476*x3 - 1.37418593*x2 + 2.09137015*x - 0.16748867
                              : 3.0817580*x3 - 5.87338670*x2 + 3.75112997*x - 0.37001483;

            return new C3f(100, x, y);
        }

        #endregion

        #region Operations

        public static C3b Average(this IEnumerable<C3b> items)
        {
            var sum = new C3d(0, 0, 0);
            long count = 0;
            foreach (var c in items) { sum.R += c.R; sum.G += c.G; sum.B += c.B; ++count; }
            var scale = 1.0 / count;
            return new C3b((byte)(sum.R * scale), (byte)(sum.G * scale), (byte)(sum.B * scale));
        }

        public static C4b Average(this IEnumerable<C4b> items)
        {
            var sum = new C4d(0, 0, 0);
            long count = 0;
            foreach (var c in items) { sum.R += c.R; sum.G += c.G; sum.B += c.B; sum.A += c.A; ++count; }
            var scale = 1.0 / count;
            return new C4b((byte)(sum.R * scale), (byte)(sum.G * scale), (byte)(sum.B * scale), (byte)(sum.A * scale));
        }

        public static C3f Average(this IEnumerable<C3f> items)
        {
            var sum = new C3d(0, 0, 0);
            long count = 0;
            foreach (var c in items) { sum.R += c.R; sum.G += c.G; sum.B += c.B; ++count; }
            var scale = 1.0 / count;
            return new C3f(sum.R * scale, sum.G * scale, sum.B * scale);
        }

        public static C4f Average(this IEnumerable<C4f> items)
        {
            var sum = new C4d(0, 0, 0, 0);
            long count = 0;
            foreach (var c in items) { sum.R += c.R; sum.G += c.G; sum.B += c.B; sum.A += c.A; ++count; }
            var scale = 1.0 / count;
            return new C4f(sum.R * scale, sum.B * scale, sum.G * scale, sum.A * scale);
        }

        #endregion

        #region Hex Parsing

        // See: https://developer.mozilla.org/en-US/docs/Web/CSS/hex-color
        private static readonly Regex regexHexSgl = new Regex("^(?:#|0x)?(?<R>[0-9a-fA-F])(?<G>[0-9a-fA-F])(?<B>[0-9a-fA-F])(?<A>[0-9a-fA-F])?$", RegexOptions.Compiled);
        private static readonly Regex regexHexDbl = new Regex("^(?:#|0x)?(?<R>[0-9a-fA-F]{2})(?<G>[0-9a-fA-F]{2})(?<B>[0-9a-fA-F]{2})(?<A>[0-9a-fA-F]{2})?$", RegexOptions.Compiled);

        private static bool TryParseHex(Regex regex, string input, out C4b result)
        {
            var m = regex.Match(input);
            if (m.Success)
            {
                string[] values = new string[4];
                values[0] = m.Groups["R"].Value;
                values[1] = m.Groups["G"].Value;
                values[2] = m.Groups["B"].Value;
                values[3] = m.Groups["A"].Success ? m.Groups["A"].Value : "FF";

                result =
                    new C4b(values.Map((x) => {
                        var value = x.Length == 1 ? new string(x[0], 2) : x;
                        return byte.Parse(value, NumberStyles.AllowHexSpecifier);
                    }));

                return true;
            }

            result = C4b.Zero;
            return false;
        }

        /// <summary>
        /// Parses a hexadecimal color string with format RRGGBBAA or RGBA, where the alpha component is optional.
        /// </summary>
        /// <remarks>
        /// For the single digit RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// The color string may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="input">The string to be parsed.</param>
        /// <param name="result">Contains the parsed color on success, C4b.Zero otherwise.</param>
        /// <returns>True on success, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseHex(Text input, out C4b result)
        {
            var str = input.WhiteSpaceTrimmed.ToString();
            return TryParseHex(regexHexDbl, str, out result) || TryParseHex(regexHexSgl, str, out result);
        }

        /// <summary>
        /// Parses a hexadecimal color string with format RRGGBBAA or RGBA, where the alpha component is optional.
        /// </summary>
        /// <remarks>
        /// For the single digit RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// The color string may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="input">The string to be parsed.</param>
        /// <param name="result">Contains the parsed color on success, C4b.Zero otherwise.</param>
        /// <returns>True on success, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseHex(string input, out C4b result)
            => TryParseHex(new Text(input), out result);

        /// <summary>
        /// Parses a hexadecimal color string with format RRGGBBAA or RGBA, where the alpha component is optional.
        /// </summary>
        /// <remarks>
        /// For the single digit RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// The color string may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="input">The string to be parsed.</param>
        /// <returns>The parsed color.</returns>
        /// <exception cref="FormatException">the input does not represent a valid hexadecimal color.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b ParseHex(Text input)
            => TryParseHex(input, out C4b result) ? result : throw new FormatException($"{input} is not a valid hexadecimal color.");

        /// <summary>
        /// Parses a hexadecimal color string with format RRGGBBAA or RGBA, where the alpha component is optional.
        /// </summary>
        /// <remarks>
        /// For the single digit RGBA format, the components are duplicated (e.g. "F" is interpreted as "FF").
        /// The color string may be prefixed by "#" or "0x".
        /// </remarks>
        /// <param name="input">The string to be parsed.</param>
        /// <returns>The parsed color.</returns>
        /// <exception cref="FormatException">the input does not represent a valid hexadecimal color.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b ParseHex(string input)
            => ParseHex(new Text(input));

        #endregion
    }

    #endregion

    #region PixFormat

    /// <summary>
    /// The PixFormat encodes the channel type and the color format of an image.
    /// </summary>
    [DataContract]
    [Serializable]
    public readonly struct PixFormat : IEquatable<PixFormat>
    {
        [DataMember]
        public readonly Type Type;
        [DataMember]
        public readonly Col.Format Format;

        #region Constructor

        public PixFormat(Type type, Col.Format format)
        {
            Type = type;
            Format = format;
        }

        #endregion

        #region Predefined PixFormats

        public static readonly PixFormat ByteBW = new PixFormat(typeof(byte), Col.Format.BW);
        public static readonly PixFormat ByteGray = new PixFormat(typeof(byte), Col.Format.Gray);
        public static readonly PixFormat ByteGrayAlpha = new PixFormat(typeof(byte), Col.Format.GrayAlpha);
        public static readonly PixFormat ByteRG = new PixFormat(typeof(byte), Col.Format.RG);
        public static readonly PixFormat ByteBGR = new PixFormat(typeof(byte), Col.Format.BGR);
        public static readonly PixFormat ByteRGB = new PixFormat(typeof(byte), Col.Format.RGB);
        public static readonly PixFormat ByteBGRA = new PixFormat(typeof(byte), Col.Format.BGRA);
        public static readonly PixFormat ByteRGBA = new PixFormat(typeof(byte), Col.Format.RGBA);
        public static readonly PixFormat ByteBGRP = new PixFormat(typeof(byte), Col.Format.BGRP);
        public static readonly PixFormat ByteRGBP = new PixFormat(typeof(byte), Col.Format.RGBP);

        public static readonly PixFormat SByteBW = new PixFormat(typeof(sbyte), Col.Format.BW);
        public static readonly PixFormat SByteGray = new PixFormat(typeof(sbyte), Col.Format.Gray);
        public static readonly PixFormat SByteGrayAlpha = new PixFormat(typeof(sbyte), Col.Format.GrayAlpha);
        public static readonly PixFormat SByteRG = new PixFormat(typeof(sbyte), Col.Format.RG);
        public static readonly PixFormat SByteBGR = new PixFormat(typeof(sbyte), Col.Format.BGR);
        public static readonly PixFormat SByteRGB = new PixFormat(typeof(sbyte), Col.Format.RGB);
        public static readonly PixFormat SByteBGRA = new PixFormat(typeof(sbyte), Col.Format.BGRA);
        public static readonly PixFormat SByteRGBA = new PixFormat(typeof(sbyte), Col.Format.RGBA);
        public static readonly PixFormat SByteBGRP = new PixFormat(typeof(sbyte), Col.Format.BGRP);
        public static readonly PixFormat SByteRGBP = new PixFormat(typeof(sbyte), Col.Format.RGBP);

        public static readonly PixFormat UShortGray = new PixFormat(typeof(ushort), Col.Format.Gray);
        public static readonly PixFormat UShortGrayAlpha = new PixFormat(typeof(ushort), Col.Format.GrayAlpha);
        public static readonly PixFormat UShortRG = new PixFormat(typeof(ushort), Col.Format.RG);
        public static readonly PixFormat UShortBGR = new PixFormat(typeof(ushort), Col.Format.BGR);
        public static readonly PixFormat UShortRGB = new PixFormat(typeof(ushort), Col.Format.RGB);
        public static readonly PixFormat UShortBGRA = new PixFormat(typeof(ushort), Col.Format.BGRA);
        public static readonly PixFormat UShortRGBA = new PixFormat(typeof(ushort), Col.Format.RGBA);
        public static readonly PixFormat UShortBGRP = new PixFormat(typeof(ushort), Col.Format.BGRP);
        public static readonly PixFormat UShortRGBP = new PixFormat(typeof(ushort), Col.Format.RGBP);

        public static readonly PixFormat ShortGray = new PixFormat(typeof(short), Col.Format.Gray);
        public static readonly PixFormat ShortGrayAlpha = new PixFormat(typeof(short), Col.Format.GrayAlpha);
        public static readonly PixFormat ShortRG = new PixFormat(typeof(short), Col.Format.RG);
        public static readonly PixFormat ShortBGR = new PixFormat(typeof(short), Col.Format.BGR);
        public static readonly PixFormat ShortRGB = new PixFormat(typeof(short), Col.Format.RGB);
        public static readonly PixFormat ShortBGRA = new PixFormat(typeof(short), Col.Format.BGRA);
        public static readonly PixFormat ShortRGBA = new PixFormat(typeof(short), Col.Format.RGBA);
        public static readonly PixFormat ShortBGRP = new PixFormat(typeof(short), Col.Format.BGRP);
        public static readonly PixFormat ShortRGBP = new PixFormat(typeof(short), Col.Format.RGBP);

        public static readonly PixFormat UIntGray = new PixFormat(typeof(uint), Col.Format.Gray);
        public static readonly PixFormat UIntGrayAlpha = new PixFormat(typeof(uint), Col.Format.GrayAlpha);
        public static readonly PixFormat UIntRG = new PixFormat(typeof(uint), Col.Format.RG);
        public static readonly PixFormat UIntBGR = new PixFormat(typeof(uint), Col.Format.BGR);
        public static readonly PixFormat UIntRGB = new PixFormat(typeof(uint), Col.Format.RGB);
        public static readonly PixFormat UIntBGRA = new PixFormat(typeof(uint), Col.Format.BGRA);
        public static readonly PixFormat UIntRGBA = new PixFormat(typeof(uint), Col.Format.RGBA);
        public static readonly PixFormat UIntBGRP = new PixFormat(typeof(uint), Col.Format.BGRP);
        public static readonly PixFormat UIntRGBP = new PixFormat(typeof(uint), Col.Format.RGBP);

        public static readonly PixFormat IntGray = new PixFormat(typeof(int), Col.Format.Gray);
        public static readonly PixFormat IntGrayAlpha = new PixFormat(typeof(int), Col.Format.GrayAlpha);
        public static readonly PixFormat IntRG = new PixFormat(typeof(int), Col.Format.RG);
        public static readonly PixFormat IntBGR = new PixFormat(typeof(int), Col.Format.BGR);
        public static readonly PixFormat IntRGB = new PixFormat(typeof(int), Col.Format.RGB);
        public static readonly PixFormat IntBGRA = new PixFormat(typeof(int), Col.Format.BGRA);
        public static readonly PixFormat IntRGBA = new PixFormat(typeof(int), Col.Format.RGBA);
        public static readonly PixFormat IntBGRP = new PixFormat(typeof(int), Col.Format.BGRP);
        public static readonly PixFormat IntRGBP = new PixFormat(typeof(int), Col.Format.RGBP);

        public static readonly PixFormat HalfGray = new PixFormat(typeof(Half), Col.Format.Gray);
        public static readonly PixFormat HalfGrayAlpha = new PixFormat(typeof(Half), Col.Format.GrayAlpha);
        public static readonly PixFormat HalfRG = new PixFormat(typeof(Half), Col.Format.RG);
        public static readonly PixFormat HalfBGR = new PixFormat(typeof(Half), Col.Format.BGR);
        public static readonly PixFormat HalfRGB = new PixFormat(typeof(Half), Col.Format.RGB);
        public static readonly PixFormat HalfBGRA = new PixFormat(typeof(Half), Col.Format.BGRA);
        public static readonly PixFormat HalfRGBA = new PixFormat(typeof(Half), Col.Format.RGBA);
        public static readonly PixFormat HalfBGRP = new PixFormat(typeof(Half), Col.Format.BGRP);
        public static readonly PixFormat HalfRGBP = new PixFormat(typeof(Half), Col.Format.RGBP);

        public static readonly PixFormat FloatGray = new PixFormat(typeof(float), Col.Format.Gray);
        public static readonly PixFormat FloatGrayAlpha = new PixFormat(typeof(float), Col.Format.GrayAlpha);
        public static readonly PixFormat FloatRG = new PixFormat(typeof(float), Col.Format.RG);
        public static readonly PixFormat FloatBGR = new PixFormat(typeof(float), Col.Format.BGR);
        public static readonly PixFormat FloatRGB = new PixFormat(typeof(float), Col.Format.RGB);
        public static readonly PixFormat FloatBGRA = new PixFormat(typeof(float), Col.Format.BGRA);
        public static readonly PixFormat FloatRGBA = new PixFormat(typeof(float), Col.Format.RGBA);
        public static readonly PixFormat FloatBGRP = new PixFormat(typeof(float), Col.Format.BGRP);
        public static readonly PixFormat FloatRGBP = new PixFormat(typeof(float), Col.Format.RGBP);

        public static readonly PixFormat DoubleGray = new PixFormat(typeof(double), Col.Format.Gray);
        public static readonly PixFormat DoubleGrayAlpha = new PixFormat(typeof(double), Col.Format.GrayAlpha);
        public static readonly PixFormat DoubleRG = new PixFormat(typeof(double), Col.Format.RG);
        public static readonly PixFormat DoubleBGR = new PixFormat(typeof(double), Col.Format.BGR);
        public static readonly PixFormat DoubleRGB = new PixFormat(typeof(double), Col.Format.RGB);
        public static readonly PixFormat DoubleBGRA = new PixFormat(typeof(double), Col.Format.BGRA);
        public static readonly PixFormat DoubleRGBA = new PixFormat(typeof(double), Col.Format.RGBA);
        public static readonly PixFormat DoubleBGRP = new PixFormat(typeof(double), Col.Format.BGRP);
        public static readonly PixFormat DoubleRGBP = new PixFormat(typeof(double), Col.Format.RGBP);

        #endregion

        #region Properties

        public int ChannelCount { get { return Format.ChannelCount(); } }

        #endregion

        #region Operators

        public static bool operator ==(PixFormat a, PixFormat b)
        {
            return a.Type == b.Type && a.Format == b.Format;
        }

        public static bool operator !=(PixFormat a, PixFormat b)
        {
            return a.Type != b.Type || a.Format != b.Format;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            return obj is PixFormat ? this == (PixFormat)obj : false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type.GetHashCode(), Format.GetHashCode());
        }

        public override string ToString()
            => $"{Type.Name}{Format}";

        #endregion

        #region IEquatable<PixFormat> Members

        public bool Equals(PixFormat other)
        {
            return other.Type == Type && other.Format == Format;
        }

        #endregion
    }

    #endregion

    #region C3b

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C3b
    {
        /// <summary>
        /// Memory layout for intel-based little-endian argb-ints.
        /// (DO NOT CHANGE!)
        /// </summary>
        [DataMember]
        public byte B;
        [DataMember]
        public byte G;
        [DataMember]
        public byte R;
    }

    #endregion

    #region C3us

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C3us
    {
        [DataMember]
        public ushort R;
        [DataMember]
        public ushort G;
        [DataMember]
        public ushort B;
    }

    #endregion

    #region C3ui

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C3ui
    {
        [DataMember]
        public uint R;
        [DataMember]
        public uint G;
        [DataMember]
        public uint B;
    }

    #endregion

    #region C3f

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C3f
    {
        [DataMember]
        public float R;
        [DataMember]
        public float G;
        [DataMember]
        public float B;

        /// <summary>
        /// Creates an RGB representation from hue, saturation and value/brightness.
        /// </summary>
        /// <param name="hue">Hue given in normalized angle [0,1] (may be larger than 1, wraps around), 0=1=red, 1/3=green, 2/3=blue.</param>
        /// <param name="saturation">Saturation [0,1], 0 = gray, 1 = fully saturated.</param>
        /// <param name="value">Value or Brightness [0,1], 0 = black, 1 = white.</param>
        /// <returns>The RGB Color for the HSV values.</returns>
        public static C3f FromHSV(float hue, float saturation, float value)
        {
            return new HSVf(hue, saturation, value).ToC3f();
        }

        /// <summary>
        /// Creates an RGB representation from hue, saturation and lightness.
        /// </summary>
        /// <param name="hue">Hue given in normalized angle [0,1] (may be larger than 1, wraps around), 0=1=red, 1/3=green, 2/3=blue.</param>
        /// <param name="saturation">Saturation [0,1], 0 = gray, 1 = fully saturated.</param>
        /// <param name="lightness">Lightness [0,1], 0 = black, 1 = white.</param>
        /// <returns>The RGB Color for the HSV values.</returns>
        public static C3f FromHSL(float hue, float saturation, float lightness)
        {
            return new HSLf(hue, saturation, lightness).ToC3f();
        }

        public readonly HSLf AsHSLf() { return new HSLf(R, G, B); }

        public readonly HSVf AsHSVf() { return new HSVf(R, G, B); }

    }

    #endregion

    #region C3d

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C3d
    {
        [DataMember]
        public double R;
        [DataMember]
        public double G;
        [DataMember]
        public double B;
    }

    #endregion

    #region C4b

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C4b
    {
        /// <summary>
        /// Memory layout for intel-based little-endian argb-ints.
        /// (DO NOT CHANGE!)
        /// </summary>
        [DataMember]
        public byte B;
        [DataMember]
        public byte G;
        [DataMember]
        public byte R;
        [DataMember]
        public byte A;
    }

    #endregion

    #region C4us

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C4us
    {
        [DataMember]
        public ushort R;
        [DataMember]
        public ushort G;
        [DataMember]
        public ushort B;
        [DataMember]
        public ushort A;
    }

    #endregion

    #region C4ui

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C4ui
    {
        [DataMember]
        public uint R;
        [DataMember]
        public uint G;
        [DataMember]
        public uint B;
        [DataMember]
        public uint A;
    }

    #endregion

    #region C4f

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C4f
    {
        [DataMember]
        public float R;
        [DataMember]
        public float G;
        [DataMember]
        public float B;
        [DataMember]
        public float A;
    }

    #endregion

    #region C4d

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct C4d
    {
        [DataMember]
        public double R;
        [DataMember]
        public double G;
        [DataMember]
        public double B;
        [DataMember]
        public double A;
    }

    #endregion

    #region CieLabf

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CieLabf : IEquatable<CieLabf>
    {
        [DataMember]
        public float L;
        [DataMember]
        public float a;
        [DataMember]
        public float b;

        public CieLabf(float _L, float _a, float _b) { L = _L; a = _a; b = _b; }
        public CieLabf(double _L, double _a, double _b) { L = (float)_L; a = (float)_a; b = (float)_b; }

        public readonly bool Equals(CieLabf other) => L == other.L && a == other.a && b == other.b;
        public override readonly bool Equals(object other) => (other is CieLabf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(L, a, b);
    }

    #endregion

    #region CieLuvf

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CieLuvf : IEquatable<CieLuvf>
    {
        [DataMember]
        public float L;
        [DataMember]
        public float u;
        [DataMember]
        public float v;

        public CieLuvf(float _L, float _u, float _v) { L = _L; u = _u; v = _v; }
        public CieLuvf(double _L, double _u, double _v) { L = (float)_L; u = (float)_u; v = (float)_v; }

        public readonly C3f AsC3f() { return new C3f(L, u, v); }

        public readonly bool Equals(CieLuvf other) => L == other.L && u == other.u && v == other.v;
        public override readonly bool Equals(object other) => (other is CieLuvf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(L, u, v);
    }

    #endregion

    #region CieXYZf

    /// <summary>
    /// This structure holds Cie XYZ colors where Y should be in the range
    /// [0,100] for reflective color values.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CieXYZf : IEquatable<CieXYZf>
    {
        [DataMember]
        public float X;
        [DataMember]
        public float Y;
        [DataMember]
        public float Z;

        public CieXYZf(float _X, float _Y, float _Z) { X = _X; Y = _Y; Z = _Z; }
        public CieXYZf(double _X, double _Y, double _Z) { X = (float)_X; Y = (float)_Y; Z = (float)_Z; }

        public readonly bool Equals(CieXYZf other) => X == other.X && Y == other.Y && Z == other.Z;
        public override readonly bool Equals(object other) => (other is CieXYZf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(X, Y, Z);
    }

    #endregion

    #region CieYxyf

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CieYxyf : IEquatable<CieYxyf>
    {
        [DataMember]
        public float Y;
        [DataMember]
        public float x;
        [DataMember]
        public float y;

        public CieYxyf(float _Y, float _x, float _y) { Y = _Y; x = _x; y = _y; }
        public CieYxyf(double _Y, double _x, double _y) { Y = (float)_Y; x = (float)_x; y = (float)_y; }

        public readonly bool Equals(CieYxyf other) => Y == other.Y && x == other.x && y == other.y;
        public override readonly bool Equals(object other) => (other is CieYxyf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(Y, x, y);
    }

    #endregion

    #region CMYKf

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CMYKf : IEquatable<CMYKf>
    {
        [DataMember]
        public float C;
        [DataMember]
        public float M;
        [DataMember]
        public float Y;
        [DataMember]
        public float K;

        public CMYKf(float _C, float _M, float _Y, float _K) { C = _C; M = _M; Y = _Y; K = _K; }
        public CMYKf(double _C, double _M, double _Y, double _K)
                { C = (float)_C; M = (float)_M; Y = (float)_Y; K = (float)_K; }

        public readonly bool Equals(CMYKf other) => C == other.C && M == other.M && Y == other.Y && K == other.K;
        public override readonly bool Equals(object other) => (other is CMYKf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(C, M, Y, K);
    }

    #endregion

    #region HSLf

    /// <summary>
    /// Hue Saturation Value colors. All three components are defined in the range [0.0, 1.0].
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct HSLf : IEquatable<HSLf>
    {
        [DataMember]
        public float H;
        [DataMember]
        public float S;
        [DataMember]
        public float L;

        #region Constructors

        public HSLf(float hue, float saturation, float lightness)
            { H = hue; S = saturation; L = lightness; }

        public HSLf(double hue, double saturation, double lightness)
            { H = (float)hue; S = (float)saturation; L = (float)lightness; }

        #endregion

        #region Conversions

        public static HSLf FromC3f(C3f col)
        {
            return col.ToHSLf();
        }

        public readonly C3f AsC3f() { return new C3f(H, S, L); }

        #endregion

        public readonly bool Equals(HSLf other) => H == other.H && S == other.S && L == other.L;
        public override readonly bool Equals(object other) => (other is HSLf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(H, S, L);
    }

    #endregion

    #region HSVf

    /// <summary>
    /// Hue Saturation Lightness colors. All three components are defined in the range [0.0, 1.0].
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct HSVf : IEquatable<HSVf>
    {
        [DataMember]
        public float H;
        [DataMember]
        public float S;
        [DataMember]
        public float V;

        #region Constructors

        public HSVf(float hue, float saturation, float value)
        { H = hue; S = saturation; V = value; }

        public HSVf(double hue, double saturation, double value)
        { H = (float)hue; S = (float)saturation; V = (float)value; }

        #endregion

        #region Conversions

        public static HSVf FromC3f(C3f col)
        {
            return col.ToHSVf();
        }

        public readonly C3f AsC3f() { return new C3f(H, S, V); }

        #endregion

        public readonly bool Equals(HSVf other) => H == other.H && S == other.S && V == other.V;
        public override readonly bool Equals(object other) => (other is HSVf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(H, S, V);
    }

    #endregion

    #region Yuvf

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Yuvf : IEquatable<Yuvf>
    {
        [DataMember]
        public float Y;
        [DataMember]
        public float u;
        [DataMember]
        public float v;

        public Yuvf(float _Y, float _u, float _v) { Y = _Y; u = _u; v = _v; }
        public Yuvf(double _Y, double _u, double _v) { Y = (float)_Y; u = (float)_u; v = (float)_v; }

        public readonly C3f AsC3f() { return new C3f(Y, u, v); }

        public readonly bool Equals(Yuvf other) => Y == other.Y && u == other.u && v == other.v;
        public override readonly bool Equals(object other) => (other is Yuvf o) ? Equals(o) : false;
        public override readonly int GetHashCode() => HashCode.GetCombined(Y, u, v);
    }

    #endregion
}

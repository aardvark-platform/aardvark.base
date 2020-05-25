using System;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region C3b

    /// <summary>
    /// Represents an RGB color with each channel stored as a <see cref="byte"/> value within [0, 255].
    /// </summary>
    [Serializable]
    public partial struct C3b : IFormattable, IEquatable<C3b>, IRGB
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="byte"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(byte r, byte g, byte b)
        {
            R = r; G = g; B = b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(int r, int g, int b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(long r, long g, long b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(float r, float g, float b)
        {
            R = Col.ByteFromFloatClamped(r);
            G = Col.ByteFromFloatClamped(g);
            B = Col.ByteFromFloatClamped(b);
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(double r, double g, double b)
        {
            R = Col.ByteFromDoubleClamped(r);
            G = Col.ByteFromDoubleClamped(g);
            B = Col.ByteFromDoubleClamped(b);
        }

        /// <summary>
        /// Creates a color from a single <see cref="byte"/> value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(byte gray)
        {
            R = gray; G = gray; B = gray;
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(float gray)
        {
            var value = Col.ByteFromFloatClamped(gray);
            R = value; G = value; B = value;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(double gray)
        {
            var value = Col.ByteFromDoubleClamped(gray);
            R = value; G = value; B = value;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C3b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C3us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C3ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C3f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C3d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C4b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C4us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C4ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C4f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(C4d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V3i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V3l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V3f vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V3d vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V4i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V4l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V4f vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(V4d vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b(Func<int, byte> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C3b color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C3b color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C3b color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C3b color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C3b color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C3b color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C3b color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C3b color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C3b color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3i(C3b color)
            => new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3l(C3b color)
            => new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C3b color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C3b color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4i(C3b color)
            => new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(255)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4l(C3b color)
            => new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(255)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C3b color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(255)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C3b color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(255)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3us(C3us c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3ui(C3ui c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3f(C3f c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3d(C3d c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4b(C4b c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4us(C4us c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4ui(C4ui c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4f(C4f c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4d(C4d c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3i ToV3i() => (V3i)this;

        /// <summary>
        /// Creates a color from a <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV3i(V3i c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3l ToV3l() => (V3l)this;

        /// <summary>
        /// Creates a color from a <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV3l(V3l c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV3f(V3f c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV3d(V3d c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4i ToV4i() => (V4i)this;

        /// <summary>
        /// Creates a color from a <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV4i(V4i c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4l ToV4l() => (V4l)this;

        /// <summary>
        /// Creates a color from a <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV4l(V4l c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV4f(V4f c) => new C3b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV4d(V4d c) => new C3b(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3b Map(Func<byte, byte> channel_fun)
        {
            return new C3b(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3us Map(Func<byte, ushort> channel_fun)
        {
            return new C3us(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3ui Map(Func<byte, uint> channel_fun)
        {
            return new C3ui(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3f Map(Func<byte, float> channel_fun)
        {
            return new C3f(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3d Map(Func<byte, double> channel_fun)
        {
            return new C3d(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        public void CopyTo<T>(T[] array, int start, Func<byte, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
        }

        public void CopyTo<T>(T[] array, int start, Func<byte, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe byte this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (byte* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (byte* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C3b with all components zero.
        /// </summary>
        public static C3b Zero => new C3b(0, 0, 0);

        public static C3b Black => new C3b(0);

        public static C3b Red => new C3b(255, 0, 0);
        public static C3b Green => new C3b(0, 255, 0);
        public static C3b Blue => new C3b(0, 0, 255);
        public static C3b Cyan => new C3b(0, 255, 255);
        public static C3b Magenta => new C3b(255, 0, 255);
        public static C3b Yellow => new C3b(255, 255, 0);
        public static C3b White => new C3b(255);

        public static C3b DarkRed => new C3b(255 / 2, 0 / 2, 0 / 2);
        public static C3b DarkGreen => new C3b(0 / 2, 255 / 2, 0 / 2);
        public static C3b DarkBlue => new C3b(0 / 2, 0 / 2, 255 / 2);
        public static C3b DarkCyan => new C3b(0 / 2, 255 / 2, 255 / 2);
        public static C3b DarkMagenta => new C3b(255 / 2, 0 / 2, 255 / 2);
        public static C3b DarkYellow => new C3b(255 / 2, 255 / 2, 0 / 2);
        public static C3b Gray => new C3b(255 / 2);

        public static C3b VRVisGreen => new C3b(178, 217, 2);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C3b a, C3b b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C3b a, C3b b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator *(C3b col, float scalar)
        {
            return new C3b(
                (byte)Fun.Round(col.R * scalar), 
                (byte)Fun.Round(col.G * scalar), 
                (byte)Fun.Round(col.B * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator *(float scalar, C3b col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator /(C3b col, float scalar)
        {
            float f = 1 / scalar;
            return new C3b(
                (byte)Fun.Round(col.R * f), 
                (byte)Fun.Round(col.G * f), 
                (byte)Fun.Round(col.B * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator /(float scalar, C3b col)
        {
            return new C3b(
                (byte)Fun.Round(scalar / col.R), 
                (byte)Fun.Round(scalar / col.G), 
                (byte)Fun.Round(scalar / col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator *(C3b col, double scalar)
        {
            return new C3b(
                (byte)Fun.Round(col.R * scalar), 
                (byte)Fun.Round(col.G * scalar), 
                (byte)Fun.Round(col.B * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator *(double scalar, C3b col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator /(C3b col, double scalar)
        {
            double f = 1 / scalar;
            return new C3b(
                (byte)Fun.Round(col.R * f), 
                (byte)Fun.Round(col.G * f), 
                (byte)Fun.Round(col.B * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator /(double scalar, C3b col)
        {
            return new C3b(
                (byte)Fun.Round(scalar / col.R), 
                (byte)Fun.Round(scalar / col.G), 
                (byte)Fun.Round(scalar / col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator +(C3b c0, C3b c1)
        {
            return new C3b(
                (byte)(c0.R + (c1.R)), 
                (byte)(c0.G + (c1.G)), 
                (byte)(c0.B + (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator -(C3b c0, C3b c1)
        {
            return new C3b(
                (byte)(c0.R - (c1.R)), 
                (byte)(c0.G - (c1.G)), 
                (byte)(c0.B - (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator +(C3b c0, C3us c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G + Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B + Col.ByteFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator -(C3b c0, C3us c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G - Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B - Col.ByteFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator +(C3b c0, C3ui c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G + Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B + Col.ByteFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator -(C3b c0, C3ui c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G - Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B - Col.ByteFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator +(C3b c0, C3f c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G + Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B + Col.ByteFromFloat(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator -(C3b c0, C3f c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G - Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B - Col.ByteFromFloat(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator +(C3b c0, C3d c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G + Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B + Col.ByteFromDouble(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator -(C3b c0, C3d c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G - Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B - Col.ByteFromDouble(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator *(C3b c0, C3b c1)
        {
            return new C3b((byte)(c0.R * c1.R), (byte)(c0.G * c1.G), (byte)(c0.B * c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator /(C3b c0, C3b c1)
        {
            return new C3b((byte)(c0.R / c1.R), (byte)(c0.G / c1.G), (byte)(c0.B / c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator +(C3b col, byte scalar)
        {
            return new C3b((byte)(col.R + scalar), (byte)(col.G + scalar), (byte)(col.B + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator +(byte scalar, C3b col)
        {
            return new C3b((byte)(scalar + col.R), (byte)(scalar + col.G), (byte)(scalar + col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator -(C3b col, byte scalar)
        {
            return new C3b((byte)(col.R - scalar), (byte)(col.G - scalar), (byte)(col.B - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b operator -(byte scalar, C3b col)
        {
            return new C3b((byte)(scalar - col.R), (byte)(scalar - col.G), (byte)(scalar - col.B));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(byte min, byte max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b Clamped(byte min, byte max)
        {
            return new C3b(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public int Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return R + G + B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public byte NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public byte NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(R, G, B); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C3b o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C3b, int, byte> Setter =
            (ref C3b color, int i, byte value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b DivideByInt(C3b c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C3b Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C3b Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C3b(
                byte.Parse(x[0], CultureInfo.InvariantCulture), 
                byte.Parse(x[1], CultureInfo.InvariantCulture), 
                byte.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        public static C3b Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<byte>.Parse, C3b.Setter);
        }

        public static C3b Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<byte>.Parse, C3b.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3b> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C3b other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return Col.DoubleFromByte(R); }
            set { R = Col.ByteFromDoubleClamped(value); }
        }

        double IRGB.Green
        {
            get { return Col.DoubleFromByte(G); }
            set { G = Col.ByteFromDoubleClamped(value); }
        }

        double IRGB.Blue
        {
            get { return Col.DoubleFromByte(B); }
            set { B = Col.ByteFromDoubleClamped(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3b Lerp(this float x, C3b a, C3b b)
        {
            return new C3b(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3b Lerp(this double x, C3b a, C3b b)
        {
            return new C3b(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }

        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C3b a, C3b b, byte tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3b a, C3b b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3b col, byte s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(byte s, C3b col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3b a, C3b b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3b col, byte s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(byte s, C3b col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3b a, C3b b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3b col, byte s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(byte s, C3b col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3b a, C3b b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3b col, byte s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(byte s, C3b col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3b a, C3b b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3b col, byte s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(byte s, C3b col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3b a, C3b b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3b col, byte s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(byte s, C3b col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3b a, C3b b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3b col, byte s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(byte s, C3b col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3b a, C3b b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3b col, byte s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(byte s, C3b col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3b a, C3b b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3b col, byte s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(byte s, C3b col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3b a, C3b b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3b col, byte s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(byte s, C3b col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3b a, C3b b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3b col, byte s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(byte s, C3b col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3b a, C3b b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3b col, byte s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(byte s, C3b col)
        {
            return (s != col.R || s != col.G || s != col.B);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, ref Tup4<float> w)
        {
            return new C3b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3b p0, C3b p1, C3b p2, C3b p3, ref Tup4<float> w)
        {
            return new C3f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, ref Tup4<double> w)
        {
            return new C3b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3b p0, C3b p1, C3b p2, C3b p3, ref Tup4<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, C3b p4, C3b p5, ref Tup6<float> w)
        {
            return new C3b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3b p0, C3b p1, C3b p2, C3b p3, C3b p4, C3b p5, ref Tup6<float> w)
        {
            return new C3f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, C3b p4, C3b p5, ref Tup6<double> w)
        {
            return new C3b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3b p0, C3b p1, C3b p2, C3b p3, C3b p4, C3b p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        #endregion
    }

    #endregion

    #region C3us

    /// <summary>
    /// Represents an RGB color with each channel stored as a <see cref="ushort"/> value within [0, 2^16 - 1].
    /// </summary>
    [Serializable]
    public partial struct C3us : IFormattable, IEquatable<C3us>, IRGB
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="ushort"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(ushort r, ushort g, ushort b)
        {
            R = r; G = g; B = b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(int r, int g, int b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(long r, long g, long b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(float r, float g, float b)
        {
            R = Col.UShortFromFloatClamped(r);
            G = Col.UShortFromFloatClamped(g);
            B = Col.UShortFromFloatClamped(b);
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(double r, double g, double b)
        {
            R = Col.UShortFromDoubleClamped(r);
            G = Col.UShortFromDoubleClamped(g);
            B = Col.UShortFromDoubleClamped(b);
        }

        /// <summary>
        /// Creates a color from a single <see cref="ushort"/> value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(ushort gray)
        {
            R = gray; G = gray; B = gray;
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(float gray)
        {
            var value = Col.UShortFromFloatClamped(gray);
            R = value; G = value; B = value;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(double gray)
        {
            var value = Col.UShortFromDoubleClamped(gray);
            R = value; G = value; B = value;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C3b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C3us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C3ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C3f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C3d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C4b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C4us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C4ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C4f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(C4d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V3i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V3l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V3f vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V3d vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V4i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V4l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V4f vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(V4d vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us(Func<int, ushort> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C3us color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C3us color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C3us color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C3us color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C3us color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C3us color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C3us color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C3us color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C3us color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3i(C3us color)
            => new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3l(C3us color)
            => new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C3us color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C3us color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4i(C3us color)
            => new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(65535)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4l(C3us color)
            => new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(65535)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C3us color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(65535)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C3us color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(65535)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3b(C3b c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3ui(C3ui c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3f(C3f c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3d(C3d c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4b(C4b c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4us(C4us c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4ui(C4ui c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4f(C4f c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4d(C4d c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3i ToV3i() => (V3i)this;

        /// <summary>
        /// Creates a color from a <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV3i(V3i c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3l ToV3l() => (V3l)this;

        /// <summary>
        /// Creates a color from a <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV3l(V3l c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV3f(V3f c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV3d(V3d c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4i ToV4i() => (V4i)this;

        /// <summary>
        /// Creates a color from a <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV4i(V4i c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4l ToV4l() => (V4l)this;

        /// <summary>
        /// Creates a color from a <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV4l(V4l c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV4f(V4f c) => new C3us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV4d(V4d c) => new C3us(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3b Map(Func<ushort, byte> channel_fun)
        {
            return new C3b(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3us Map(Func<ushort, ushort> channel_fun)
        {
            return new C3us(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3ui Map(Func<ushort, uint> channel_fun)
        {
            return new C3ui(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3f Map(Func<ushort, float> channel_fun)
        {
            return new C3f(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3d Map(Func<ushort, double> channel_fun)
        {
            return new C3d(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        public void CopyTo<T>(T[] array, int start, Func<ushort, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
        }

        public void CopyTo<T>(T[] array, int start, Func<ushort, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe ushort this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (ushort* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (ushort* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C3us with all components zero.
        /// </summary>
        public static C3us Zero => new C3us(0, 0, 0);

        public static C3us Black => new C3us(0);

        public static C3us Red => new C3us(65535, 0, 0);
        public static C3us Green => new C3us(0, 65535, 0);
        public static C3us Blue => new C3us(0, 0, 65535);
        public static C3us Cyan => new C3us(0, 65535, 65535);
        public static C3us Magenta => new C3us(65535, 0, 65535);
        public static C3us Yellow => new C3us(65535, 65535, 0);
        public static C3us White => new C3us(65535);

        public static C3us DarkRed => new C3us(65535 / 2, 0 / 2, 0 / 2);
        public static C3us DarkGreen => new C3us(0 / 2, 65535 / 2, 0 / 2);
        public static C3us DarkBlue => new C3us(0 / 2, 0 / 2, 65535 / 2);
        public static C3us DarkCyan => new C3us(0 / 2, 65535 / 2, 65535 / 2);
        public static C3us DarkMagenta => new C3us(65535 / 2, 0 / 2, 65535 / 2);
        public static C3us DarkYellow => new C3us(65535 / 2, 65535 / 2, 0 / 2);
        public static C3us Gray => new C3us(65535 / 2);

        public static C3us VRVisGreen => new C3us(45743, 53411, 5243);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C3us a, C3us b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C3us a, C3us b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator *(C3us col, float scalar)
        {
            return new C3us(
                (ushort)Fun.Round(col.R * scalar), 
                (ushort)Fun.Round(col.G * scalar), 
                (ushort)Fun.Round(col.B * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator *(float scalar, C3us col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator /(C3us col, float scalar)
        {
            float f = 1 / scalar;
            return new C3us(
                (ushort)Fun.Round(col.R * f), 
                (ushort)Fun.Round(col.G * f), 
                (ushort)Fun.Round(col.B * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator /(float scalar, C3us col)
        {
            return new C3us(
                (ushort)Fun.Round(scalar / col.R), 
                (ushort)Fun.Round(scalar / col.G), 
                (ushort)Fun.Round(scalar / col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator *(C3us col, double scalar)
        {
            return new C3us(
                (ushort)Fun.Round(col.R * scalar), 
                (ushort)Fun.Round(col.G * scalar), 
                (ushort)Fun.Round(col.B * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator *(double scalar, C3us col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator /(C3us col, double scalar)
        {
            double f = 1 / scalar;
            return new C3us(
                (ushort)Fun.Round(col.R * f), 
                (ushort)Fun.Round(col.G * f), 
                (ushort)Fun.Round(col.B * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator /(double scalar, C3us col)
        {
            return new C3us(
                (ushort)Fun.Round(scalar / col.R), 
                (ushort)Fun.Round(scalar / col.G), 
                (ushort)Fun.Round(scalar / col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator +(C3us c0, C3b c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G + Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B + Col.UShortFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator -(C3us c0, C3b c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G - Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B - Col.UShortFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator +(C3us c0, C3us c1)
        {
            return new C3us(
                (ushort)(c0.R + (c1.R)), 
                (ushort)(c0.G + (c1.G)), 
                (ushort)(c0.B + (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator -(C3us c0, C3us c1)
        {
            return new C3us(
                (ushort)(c0.R - (c1.R)), 
                (ushort)(c0.G - (c1.G)), 
                (ushort)(c0.B - (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator +(C3us c0, C3ui c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G + Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B + Col.UShortFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator -(C3us c0, C3ui c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G - Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B - Col.UShortFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator +(C3us c0, C3f c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G + Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B + Col.UShortFromFloat(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator -(C3us c0, C3f c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G - Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B - Col.UShortFromFloat(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator +(C3us c0, C3d c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G + Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B + Col.UShortFromDouble(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator -(C3us c0, C3d c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G - Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B - Col.UShortFromDouble(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator *(C3us c0, C3us c1)
        {
            return new C3us((ushort)(c0.R * c1.R), (ushort)(c0.G * c1.G), (ushort)(c0.B * c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator /(C3us c0, C3us c1)
        {
            return new C3us((ushort)(c0.R / c1.R), (ushort)(c0.G / c1.G), (ushort)(c0.B / c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator +(C3us col, ushort scalar)
        {
            return new C3us((ushort)(col.R + scalar), (ushort)(col.G + scalar), (ushort)(col.B + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator +(ushort scalar, C3us col)
        {
            return new C3us((ushort)(scalar + col.R), (ushort)(scalar + col.G), (ushort)(scalar + col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator -(C3us col, ushort scalar)
        {
            return new C3us((ushort)(col.R - scalar), (ushort)(col.G - scalar), (ushort)(col.B - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us operator -(ushort scalar, C3us col)
        {
            return new C3us((ushort)(scalar - col.R), (ushort)(scalar - col.G), (ushort)(scalar - col.B));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(ushort min, ushort max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us Clamped(ushort min, ushort max)
        {
            return new C3us(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public int Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return R + G + B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public ushort NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public ushort NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(R, G, B); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C3us o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C3us, int, ushort> Setter =
            (ref C3us color, int i, ushort value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us DivideByInt(C3us c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C3us Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C3us Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C3us(
                ushort.Parse(x[0], CultureInfo.InvariantCulture), 
                ushort.Parse(x[1], CultureInfo.InvariantCulture), 
                ushort.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        public static C3us Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<ushort>.Parse, C3us.Setter);
        }

        public static C3us Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<ushort>.Parse, C3us.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3us> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C3us other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return Col.DoubleFromUShort(R); }
            set { R = Col.UShortFromDoubleClamped(value); }
        }

        double IRGB.Green
        {
            get { return Col.DoubleFromUShort(G); }
            set { G = Col.UShortFromDoubleClamped(value); }
        }

        double IRGB.Blue
        {
            get { return Col.DoubleFromUShort(B); }
            set { B = Col.UShortFromDoubleClamped(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3us Lerp(this float x, C3us a, C3us b)
        {
            return new C3us(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3us Lerp(this double x, C3us a, C3us b)
        {
            return new C3us(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }

        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C3us a, C3us b, ushort tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3us a, C3us b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3us col, ushort s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(ushort s, C3us col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3us a, C3us b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3us col, ushort s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(ushort s, C3us col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3us a, C3us b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3us col, ushort s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(ushort s, C3us col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3us a, C3us b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3us col, ushort s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(ushort s, C3us col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3us a, C3us b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3us col, ushort s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(ushort s, C3us col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3us a, C3us b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3us col, ushort s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(ushort s, C3us col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3us a, C3us b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3us col, ushort s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(ushort s, C3us col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3us a, C3us b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3us col, ushort s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(ushort s, C3us col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3us a, C3us b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3us col, ushort s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(ushort s, C3us col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3us a, C3us b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3us col, ushort s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(ushort s, C3us col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3us a, C3us b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3us col, ushort s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(ushort s, C3us col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3us a, C3us b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3us col, ushort s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(ushort s, C3us col)
        {
            return (s != col.R || s != col.G || s != col.B);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, ref Tup4<float> w)
        {
            return new C3us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3us p0, C3us p1, C3us p2, C3us p3, ref Tup4<float> w)
        {
            return new C3f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, ref Tup4<double> w)
        {
            return new C3us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3us p0, C3us p1, C3us p2, C3us p3, ref Tup4<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, C3us p4, C3us p5, ref Tup6<float> w)
        {
            return new C3us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3us p0, C3us p1, C3us p2, C3us p3, C3us p4, C3us p5, ref Tup6<float> w)
        {
            return new C3f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, C3us p4, C3us p5, ref Tup6<double> w)
        {
            return new C3us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3us p0, C3us p1, C3us p2, C3us p3, C3us p4, C3us p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        #endregion
    }

    #endregion

    #region C3ui

    /// <summary>
    /// Represents an RGB color with each channel stored as a <see cref="uint"/> value within [0, 2^32 - 1].
    /// </summary>
    [Serializable]
    public partial struct C3ui : IFormattable, IEquatable<C3ui>, IRGB
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="uint"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(uint r, uint g, uint b)
        {
            R = r; G = g; B = b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(int r, int g, int b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(long r, long g, long b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(float r, float g, float b)
        {
            R = Col.UIntFromFloatClamped(r);
            G = Col.UIntFromFloatClamped(g);
            B = Col.UIntFromFloatClamped(b);
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(double r, double g, double b)
        {
            R = Col.UIntFromDoubleClamped(r);
            G = Col.UIntFromDoubleClamped(g);
            B = Col.UIntFromDoubleClamped(b);
        }

        /// <summary>
        /// Creates a color from a single <see cref="uint"/> value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(uint gray)
        {
            R = gray; G = gray; B = gray;
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(float gray)
        {
            var value = Col.UIntFromFloatClamped(gray);
            R = value; G = value; B = value;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(double gray)
        {
            var value = Col.UIntFromDoubleClamped(gray);
            R = value; G = value; B = value;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C3b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C3us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C3ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C3f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C3d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C4b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C4us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C4ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C4f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(C4d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(V3l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(V3f vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(V3d vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(V4l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(V4f vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(V4d vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui(Func<int, uint> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C3ui color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C3ui color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C3ui color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C3ui color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C3ui color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C3ui color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C3ui color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C3ui color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C3ui color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3l(C3ui color)
            => new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C3ui color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C3ui color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// W is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4l(C3ui color)
            => new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(UInt32.MaxValue)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C3ui color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(UInt32.MaxValue)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C3ui color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(UInt32.MaxValue)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3b(C3b c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3us(C3us c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3f(C3f c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3d(C3d c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4b(C4b c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4us(C4us c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4ui(C4ui c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4f(C4f c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4d(C4d c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3l ToV3l() => (V3l)this;

        /// <summary>
        /// Creates a color from a <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV3l(V3l c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV3f(V3f c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV3d(V3d c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// W is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4l ToV4l() => (V4l)this;

        /// <summary>
        /// Creates a color from a <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV4l(V4l c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV4f(V4f c) => new C3ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV4d(V4d c) => new C3ui(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3b Map(Func<uint, byte> channel_fun)
        {
            return new C3b(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3us Map(Func<uint, ushort> channel_fun)
        {
            return new C3us(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3ui Map(Func<uint, uint> channel_fun)
        {
            return new C3ui(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3f Map(Func<uint, float> channel_fun)
        {
            return new C3f(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3d Map(Func<uint, double> channel_fun)
        {
            return new C3d(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        public void CopyTo<T>(T[] array, int start, Func<uint, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
        }

        public void CopyTo<T>(T[] array, int start, Func<uint, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe uint this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (uint* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (uint* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C3ui with all components zero.
        /// </summary>
        public static C3ui Zero => new C3ui(0, 0, 0);

        public static C3ui Black => new C3ui(0);

        public static C3ui Red => new C3ui(UInt32.MaxValue, 0, 0);
        public static C3ui Green => new C3ui(0, UInt32.MaxValue, 0);
        public static C3ui Blue => new C3ui(0, 0, UInt32.MaxValue);
        public static C3ui Cyan => new C3ui(0, UInt32.MaxValue, UInt32.MaxValue);
        public static C3ui Magenta => new C3ui(UInt32.MaxValue, 0, UInt32.MaxValue);
        public static C3ui Yellow => new C3ui(UInt32.MaxValue, UInt32.MaxValue, 0);
        public static C3ui White => new C3ui(UInt32.MaxValue);

        public static C3ui DarkRed => new C3ui(UInt32.MaxValue / 2, 0 / 2, 0 / 2);
        public static C3ui DarkGreen => new C3ui(0 / 2, UInt32.MaxValue / 2, 0 / 2);
        public static C3ui DarkBlue => new C3ui(0 / 2, 0 / 2, UInt32.MaxValue / 2);
        public static C3ui DarkCyan => new C3ui(0 / 2, UInt32.MaxValue / 2, UInt32.MaxValue / 2);
        public static C3ui DarkMagenta => new C3ui(UInt32.MaxValue / 2, 0 / 2, UInt32.MaxValue / 2);
        public static C3ui DarkYellow => new C3ui(UInt32.MaxValue / 2, UInt32.MaxValue / 2, 0 / 2);
        public static C3ui Gray => new C3ui(UInt32.MaxValue / 2);


        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C3ui a, C3ui b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C3ui a, C3ui b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator *(C3ui col, float scalar)
        {
            return new C3ui(
                (uint)Fun.Round(col.R * scalar), 
                (uint)Fun.Round(col.G * scalar), 
                (uint)Fun.Round(col.B * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator *(float scalar, C3ui col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator /(C3ui col, float scalar)
        {
            float f = 1 / scalar;
            return new C3ui(
                (uint)Fun.Round(col.R * f), 
                (uint)Fun.Round(col.G * f), 
                (uint)Fun.Round(col.B * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator /(float scalar, C3ui col)
        {
            return new C3ui(
                (uint)Fun.Round(scalar / col.R), 
                (uint)Fun.Round(scalar / col.G), 
                (uint)Fun.Round(scalar / col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator *(C3ui col, double scalar)
        {
            return new C3ui(
                (uint)Fun.Round(col.R * scalar), 
                (uint)Fun.Round(col.G * scalar), 
                (uint)Fun.Round(col.B * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator *(double scalar, C3ui col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator /(C3ui col, double scalar)
        {
            double f = 1 / scalar;
            return new C3ui(
                (uint)Fun.Round(col.R * f), 
                (uint)Fun.Round(col.G * f), 
                (uint)Fun.Round(col.B * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator /(double scalar, C3ui col)
        {
            return new C3ui(
                (uint)Fun.Round(scalar / col.R), 
                (uint)Fun.Round(scalar / col.G), 
                (uint)Fun.Round(scalar / col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator +(C3ui c0, C3b c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromByte(c1.R)), 
                (uint)(c0.G + Col.UIntFromByte(c1.G)), 
                (uint)(c0.B + Col.UIntFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator -(C3ui c0, C3b c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromByte(c1.R)), 
                (uint)(c0.G - Col.UIntFromByte(c1.G)), 
                (uint)(c0.B - Col.UIntFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator +(C3ui c0, C3us c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G + Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B + Col.UIntFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator -(C3ui c0, C3us c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G - Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B - Col.UIntFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator +(C3ui c0, C3ui c1)
        {
            return new C3ui(
                (uint)(c0.R + (c1.R)), 
                (uint)(c0.G + (c1.G)), 
                (uint)(c0.B + (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator -(C3ui c0, C3ui c1)
        {
            return new C3ui(
                (uint)(c0.R - (c1.R)), 
                (uint)(c0.G - (c1.G)), 
                (uint)(c0.B - (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator +(C3ui c0, C3f c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G + Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B + Col.UIntFromFloat(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator -(C3ui c0, C3f c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G - Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B - Col.UIntFromFloat(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator +(C3ui c0, C3d c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G + Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B + Col.UIntFromDouble(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator -(C3ui c0, C3d c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G - Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B - Col.UIntFromDouble(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator *(C3ui c0, C3ui c1)
        {
            return new C3ui((uint)(c0.R * c1.R), (uint)(c0.G * c1.G), (uint)(c0.B * c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator /(C3ui c0, C3ui c1)
        {
            return new C3ui((uint)(c0.R / c1.R), (uint)(c0.G / c1.G), (uint)(c0.B / c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator +(C3ui col, uint scalar)
        {
            return new C3ui((uint)(col.R + scalar), (uint)(col.G + scalar), (uint)(col.B + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator +(uint scalar, C3ui col)
        {
            return new C3ui((uint)(scalar + col.R), (uint)(scalar + col.G), (uint)(scalar + col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator -(C3ui col, uint scalar)
        {
            return new C3ui((uint)(col.R - scalar), (uint)(col.G - scalar), (uint)(col.B - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui operator -(uint scalar, C3ui col)
        {
            return new C3ui((uint)(scalar - col.R), (uint)(scalar - col.G), (uint)(scalar - col.B));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(uint min, uint max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui Clamped(uint min, uint max)
        {
            return new C3ui(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public long Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return R + G + B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public uint NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public uint NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(R, G, B); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C3ui o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C3ui, int, uint> Setter =
            (ref C3ui color, int i, uint value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui DivideByInt(C3ui c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C3ui Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C3ui Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C3ui(
                uint.Parse(x[0], CultureInfo.InvariantCulture), 
                uint.Parse(x[1], CultureInfo.InvariantCulture), 
                uint.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        public static C3ui Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<uint>.Parse, C3ui.Setter);
        }

        public static C3ui Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<uint>.Parse, C3ui.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3ui> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C3ui other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return Col.DoubleFromUInt(R); }
            set { R = Col.UIntFromDoubleClamped(value); }
        }

        double IRGB.Green
        {
            get { return Col.DoubleFromUInt(G); }
            set { G = Col.UIntFromDoubleClamped(value); }
        }

        double IRGB.Blue
        {
            get { return Col.DoubleFromUInt(B); }
            set { B = Col.UIntFromDoubleClamped(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3ui Lerp(this float x, C3ui a, C3ui b)
        {
            return new C3ui(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3ui Lerp(this double x, C3ui a, C3ui b)
        {
            return new C3ui(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }

        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C3ui a, C3ui b, uint tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3ui a, C3ui b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3ui col, uint s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(uint s, C3ui col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3ui a, C3ui b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3ui col, uint s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(uint s, C3ui col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3ui a, C3ui b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3ui col, uint s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(uint s, C3ui col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3ui a, C3ui b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3ui col, uint s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(uint s, C3ui col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3ui a, C3ui b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3ui col, uint s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(uint s, C3ui col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3ui a, C3ui b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3ui col, uint s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(uint s, C3ui col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3ui a, C3ui b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3ui col, uint s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(uint s, C3ui col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3ui a, C3ui b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3ui col, uint s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(uint s, C3ui col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3ui a, C3ui b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3ui col, uint s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(uint s, C3ui col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3ui a, C3ui b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3ui col, uint s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(uint s, C3ui col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3ui a, C3ui b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3ui col, uint s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(uint s, C3ui col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3ui a, C3ui b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3ui col, uint s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(uint s, C3ui col)
        {
            return (s != col.R || s != col.G || s != col.B);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, ref Tup4<float> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, ref Tup4<float> w)
        {
            return new C3f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, ref Tup4<double> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, ref Tup4<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, C3ui p4, C3ui p5, ref Tup6<float> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, C3ui p4, C3ui p5, ref Tup6<float> w)
        {
            return new C3f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, C3ui p4, C3ui p5, ref Tup6<double> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, C3ui p4, C3ui p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        #endregion
    }

    public static class IRandomUniformC3uiExtensions
    {
        #region IRandomUniform extensions for C3ui

        /// <summary>
        /// Uses UniformUInt() to generate the elements of a C3ui color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui UniformC3ui(this IRandomUniform rnd)
        {
            return new C3ui(rnd.UniformUInt(), rnd.UniformUInt(), rnd.UniformUInt());
        }

        #endregion
    }

    #endregion

    #region C3f

    /// <summary>
    /// Represents an RGB color with each channel stored as a <see cref="float"/> value within [0, 1].
    /// </summary>
    [Serializable]
    public partial struct C3f : IFormattable, IEquatable<C3f>, IRGB
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(float r, float g, float b)
        {
            R = r; G = g; B = b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(double r, double g, double b)
        {
            R = (float)(r);
            G = (float)(g);
            B = (float)(b);
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(float gray)
        {
            R = gray; G = gray; B = gray;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C3b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C3us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C3ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C3f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C3d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C4b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C4us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C4ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C4f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(C4d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(V3f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(V3d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(V4f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(V4d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f(Func<int, float> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C3f color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C3f color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C3f color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C3f color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C3f color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C3f color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C3f color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C3f color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C3f color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C3f color)
            => new V3f(
                (color.R), 
                (color.G), 
                (color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C3f color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C3f color)
            => new V4f(
                (color.R), 
                (color.G), 
                (color.B),
                (1.0f)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C3f color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(1.0f)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3b(C3b c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3us(C3us c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3ui(C3ui c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3d(C3d c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4b(C4b c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4us(C4us c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4ui(C4ui c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4f(C4f c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4d(C4d c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV3f(V3f c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV3d(V3d c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV4f(V4f c) => new C3f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV4d(V4d c) => new C3f(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3b Map(Func<float, byte> channel_fun)
        {
            return new C3b(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3us Map(Func<float, ushort> channel_fun)
        {
            return new C3us(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3ui Map(Func<float, uint> channel_fun)
        {
            return new C3ui(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3f Map(Func<float, float> channel_fun)
        {
            return new C3f(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3d Map(Func<float, double> channel_fun)
        {
            return new C3d(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        public void CopyTo<T>(T[] array, int start, Func<float, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
        }

        public void CopyTo<T>(T[] array, int start, Func<float, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe float this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (float* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (float* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C3f with all components zero.
        /// </summary>
        public static C3f Zero => new C3f(0.0f, 0.0f, 0.0f);

        public static C3f Black => new C3f(0.0f);

        public static C3f Red => new C3f(1.0f, 0.0f, 0.0f);
        public static C3f Green => new C3f(0.0f, 1.0f, 0.0f);
        public static C3f Blue => new C3f(0.0f, 0.0f, 1.0f);
        public static C3f Cyan => new C3f(0.0f, 1.0f, 1.0f);
        public static C3f Magenta => new C3f(1.0f, 0.0f, 1.0f);
        public static C3f Yellow => new C3f(1.0f, 1.0f, 0.0f);
        public static C3f White => new C3f(1.0f);

        public static C3f DarkRed => new C3f(1.0f / 2, 0.0f / 2, 0.0f / 2);
        public static C3f DarkGreen => new C3f(0.0f / 2, 1.0f / 2, 0.0f / 2);
        public static C3f DarkBlue => new C3f(0.0f / 2, 0.0f / 2, 1.0f / 2);
        public static C3f DarkCyan => new C3f(0.0f / 2, 1.0f / 2, 1.0f / 2);
        public static C3f DarkMagenta => new C3f(1.0f / 2, 0.0f / 2, 1.0f / 2);
        public static C3f DarkYellow => new C3f(1.0f / 2, 1.0f / 2, 0.0f / 2);
        public static C3f Gray => new C3f(1.0f / 2);

        public static C3f Gray10 => new C3f(0.1f);
        public static C3f Gray20 => new C3f(0.2f);
        public static C3f Gray30 => new C3f(0.3f);
        public static C3f Gray40 => new C3f(0.4f);
        public static C3f Gray50 => new C3f(0.5f);
        public static C3f Gray60 => new C3f(0.6f);
        public static C3f Gray70 => new C3f(0.7f);
        public static C3f Gray80 => new C3f(0.8f);
        public static C3f Gray90 => new C3f(0.9f);
        public static C3f VRVisGreen => new C3f(0.698f, 0.851f, 0.008f);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C3f a, C3f b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C3f a, C3f b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator *(C3f col, float scalar)
        {
            return new C3f(
                col.R * scalar, 
                col.G * scalar, 
                col.B * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator *(float scalar, C3f col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator /(C3f col, float scalar)
        {
            float f = 1 / scalar;
            return new C3f(
                col.R * f, 
                col.G * f, 
                col.B * f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator /(float scalar, C3f col)
        {
            return new C3f(
                scalar / col.R, 
                scalar / col.G, 
                scalar / col.B);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator +(C3f c0, C3b c1)
        {
            return new C3f(
                (float)(c0.R + Col.FloatFromByte(c1.R)), 
                (float)(c0.G + Col.FloatFromByte(c1.G)), 
                (float)(c0.B + Col.FloatFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator -(C3f c0, C3b c1)
        {
            return new C3f(
                (float)(c0.R - Col.FloatFromByte(c1.R)), 
                (float)(c0.G - Col.FloatFromByte(c1.G)), 
                (float)(c0.B - Col.FloatFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator +(C3f c0, C3us c1)
        {
            return new C3f(
                (float)(c0.R + Col.FloatFromUShort(c1.R)), 
                (float)(c0.G + Col.FloatFromUShort(c1.G)), 
                (float)(c0.B + Col.FloatFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator -(C3f c0, C3us c1)
        {
            return new C3f(
                (float)(c0.R - Col.FloatFromUShort(c1.R)), 
                (float)(c0.G - Col.FloatFromUShort(c1.G)), 
                (float)(c0.B - Col.FloatFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator +(C3f c0, C3ui c1)
        {
            return new C3f(
                (float)(c0.R + Col.FloatFromUInt(c1.R)), 
                (float)(c0.G + Col.FloatFromUInt(c1.G)), 
                (float)(c0.B + Col.FloatFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator -(C3f c0, C3ui c1)
        {
            return new C3f(
                (float)(c0.R - Col.FloatFromUInt(c1.R)), 
                (float)(c0.G - Col.FloatFromUInt(c1.G)), 
                (float)(c0.B - Col.FloatFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator +(C3f c0, C3f c1)
        {
            return new C3f(
                (float)(c0.R + (c1.R)), 
                (float)(c0.G + (c1.G)), 
                (float)(c0.B + (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator -(C3f c0, C3f c1)
        {
            return new C3f(
                (float)(c0.R - (c1.R)), 
                (float)(c0.G - (c1.G)), 
                (float)(c0.B - (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator +(C3f c0, C3d c1)
        {
            return new C3f(
                (float)(c0.R + (float)(c1.R)), 
                (float)(c0.G + (float)(c1.G)), 
                (float)(c0.B + (float)(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator -(C3f c0, C3d c1)
        {
            return new C3f(
                (float)(c0.R - (float)(c1.R)), 
                (float)(c0.G - (float)(c1.G)), 
                (float)(c0.B - (float)(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator *(C3f c0, C3f c1)
        {
            return new C3f((float)(c0.R * c1.R), (float)(c0.G * c1.G), (float)(c0.B * c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator /(C3f c0, C3f c1)
        {
            return new C3f((float)(c0.R / c1.R), (float)(c0.G / c1.G), (float)(c0.B / c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator +(C3f col, float scalar)
        {
            return new C3f((float)(col.R + scalar), (float)(col.G + scalar), (float)(col.B + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator +(float scalar, C3f col)
        {
            return new C3f((float)(scalar + col.R), (float)(scalar + col.G), (float)(scalar + col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator -(C3f col, float scalar)
        {
            return new C3f((float)(col.R - scalar), (float)(col.G - scalar), (float)(col.B - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f operator -(float scalar, C3f col)
        {
            return new C3f((float)(scalar - col.R), (float)(scalar - col.G), (float)(scalar - col.B));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(float min, float max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f Clamped(float min, float max)
        {
            return new C3f(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public float Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Abs(R) + Fun.Abs(G) + Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public float Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public float NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public float NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C3f o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C3f, int, float> Setter =
            (ref C3f color, int i, float value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f DivideByInt(C3f c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C3f Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C3f(
                float.Parse(x[0], CultureInfo.InvariantCulture), 
                float.Parse(x[1], CultureInfo.InvariantCulture), 
                float.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        public static C3f Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<float>.Parse, C3f.Setter);
        }

        public static C3f Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<float>.Parse, C3f.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3f> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C3f other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return (double)(R); }
            set { R = (float)(value); }
        }

        double IRGB.Green
        {
            get { return (double)(G); }
            set { G = (float)(value); }
        }

        double IRGB.Blue
        {
            get { return (double)(B); }
            set { B = (float)(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3f Lerp(this float x, C3f a, C3f b)
        {
            return new C3f(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }
        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C3f a, C3f b)
        {
            return ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C3f a, C3f b, float tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3f a, C3f b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3f col, float s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(float s, C3f col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3f a, C3f b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3f col, float s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(float s, C3f col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3f a, C3f b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3f col, float s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(float s, C3f col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3f a, C3f b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3f col, float s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(float s, C3f col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3f a, C3f b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3f col, float s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(float s, C3f col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3f a, C3f b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3f col, float s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(float s, C3f col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3f a, C3f b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3f col, float s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(float s, C3f col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3f a, C3f b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3f col, float s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(float s, C3f col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3f a, C3f b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3f col, float s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(float s, C3f col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3f a, C3f b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3f col, float s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(float s, C3f col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3f a, C3f b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3f col, float s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(float s, C3f col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3f a, C3f b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3f col, float s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(float s, C3f col)
        {
            return (s != col.R || s != col.G || s != col.B);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, ref Tup4<float> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<float> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        #endregion
    }

    public static class IRandomUniformC3fExtensions
    {
        #region IRandomUniform extensions for C3f

        /// <summary>
        /// Uses UniformFloat() to generate the elements of a C3f color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f UniformC3f(this IRandomUniform rnd)
        {
            return new C3f(rnd.UniformFloat(), rnd.UniformFloat(), rnd.UniformFloat());
        }

        /// <summary>
        /// Uses UniformFloatClosed() to generate the elements of a C3f color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f UniformC3fClosed(this IRandomUniform rnd)
        {
            return new C3f(rnd.UniformFloatClosed(), rnd.UniformFloatClosed(), rnd.UniformFloatClosed());
        }

        /// <summary>
        /// Uses UniformFloatOpen() to generate the elements of a C3f color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f UniformC3fOpen(this IRandomUniform rnd)
        {
            return new C3f(rnd.UniformFloatOpen(), rnd.UniformFloatOpen(), rnd.UniformFloatOpen());
        }

        #endregion
    }

    #endregion

    #region C3d

    /// <summary>
    /// Represents an RGB color with each channel stored as a <see cref="double"/> value within [0, 1].
    /// </summary>
    [Serializable]
    public partial struct C3d : IFormattable, IEquatable<C3d>, IRGB
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(double r, double g, double b)
        {
            R = r; G = g; B = b;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(float r, float g, float b)
        {
            R = (double)(r);
            G = (double)(g);
            B = (double)(b);
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(double gray)
        {
            R = gray; G = gray; B = gray;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C3b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C3us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C3ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C3f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C3d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C4b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C4us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C4ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C4f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(C4d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(V3f vec)
        {
            R = (double)(vec.X);
            G = (double)(vec.Y);
            B = (double)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(V3d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(V4f vec)
        {
            R = (double)(vec.X);
            G = (double)(vec.Y);
            B = (double)(vec.Z);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(V4d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d(Func<int, double> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C3d color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C3d color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C3d color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C3d color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C3d color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C3d color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C3d color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C3d color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C3d color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C3d color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C3d color)
            => new V3d(
                (color.R), 
                (color.G), 
                (color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C3d color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(1.0)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C3d color)
            => new V4d(
                (color.R), 
                (color.G), 
                (color.B),
                (1.0)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3b(C3b c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3us(C3us c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3ui(C3ui c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3f(C3f c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4b(C4b c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4us(C4us c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4ui(C4ui c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4f(C4f c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4d(C4d c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromV3f(V3f c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromV3d(V3d c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromV4f(V4f c) => new C3d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// W is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromV4d(V4d c) => new C3d(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3b Map(Func<double, byte> channel_fun)
        {
            return new C3b(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3us Map(Func<double, ushort> channel_fun)
        {
            return new C3us(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3ui Map(Func<double, uint> channel_fun)
        {
            return new C3ui(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3f Map(Func<double, float> channel_fun)
        {
            return new C3f(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C3d Map(Func<double, double> channel_fun)
        {
            return new C3d(channel_fun(R), channel_fun(G), channel_fun(B));
        }

        public void CopyTo<T>(T[] array, int start, Func<double, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
        }

        public void CopyTo<T>(T[] array, int start, Func<double, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe double this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (double* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (double* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C3d with all components zero.
        /// </summary>
        public static C3d Zero => new C3d(0, 0, 0);

        public static C3d Black => new C3d(0);

        public static C3d Red => new C3d(1.0, 0, 0);
        public static C3d Green => new C3d(0, 1.0, 0);
        public static C3d Blue => new C3d(0, 0, 1.0);
        public static C3d Cyan => new C3d(0, 1.0, 1.0);
        public static C3d Magenta => new C3d(1.0, 0, 1.0);
        public static C3d Yellow => new C3d(1.0, 1.0, 0);
        public static C3d White => new C3d(1.0);

        public static C3d DarkRed => new C3d(1.0 / 2, 0 / 2, 0 / 2);
        public static C3d DarkGreen => new C3d(0 / 2, 1.0 / 2, 0 / 2);
        public static C3d DarkBlue => new C3d(0 / 2, 0 / 2, 1.0 / 2);
        public static C3d DarkCyan => new C3d(0 / 2, 1.0 / 2, 1.0 / 2);
        public static C3d DarkMagenta => new C3d(1.0 / 2, 0 / 2, 1.0 / 2);
        public static C3d DarkYellow => new C3d(1.0 / 2, 1.0 / 2, 0 / 2);
        public static C3d Gray => new C3d(1.0 / 2);


        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C3d a, C3d b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C3d a, C3d b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator *(C3d col, double scalar)
        {
            return new C3d(
                col.R * scalar, 
                col.G * scalar, 
                col.B * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator *(double scalar, C3d col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator /(C3d col, double scalar)
        {
            double f = 1 / scalar;
            return new C3d(
                col.R * f, 
                col.G * f, 
                col.B * f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator /(double scalar, C3d col)
        {
            return new C3d(
                scalar / col.R, 
                scalar / col.G, 
                scalar / col.B);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator +(C3d c0, C3b c1)
        {
            return new C3d(
                (double)(c0.R + Col.DoubleFromByte(c1.R)), 
                (double)(c0.G + Col.DoubleFromByte(c1.G)), 
                (double)(c0.B + Col.DoubleFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator -(C3d c0, C3b c1)
        {
            return new C3d(
                (double)(c0.R - Col.DoubleFromByte(c1.R)), 
                (double)(c0.G - Col.DoubleFromByte(c1.G)), 
                (double)(c0.B - Col.DoubleFromByte(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator +(C3d c0, C3us c1)
        {
            return new C3d(
                (double)(c0.R + Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G + Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B + Col.DoubleFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator -(C3d c0, C3us c1)
        {
            return new C3d(
                (double)(c0.R - Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G - Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B - Col.DoubleFromUShort(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator +(C3d c0, C3ui c1)
        {
            return new C3d(
                (double)(c0.R + Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G + Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B + Col.DoubleFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator -(C3d c0, C3ui c1)
        {
            return new C3d(
                (double)(c0.R - Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G - Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B - Col.DoubleFromUInt(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator +(C3d c0, C3f c1)
        {
            return new C3d(
                (double)(c0.R + (double)(c1.R)), 
                (double)(c0.G + (double)(c1.G)), 
                (double)(c0.B + (double)(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator -(C3d c0, C3f c1)
        {
            return new C3d(
                (double)(c0.R - (double)(c1.R)), 
                (double)(c0.G - (double)(c1.G)), 
                (double)(c0.B - (double)(c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator +(C3d c0, C3d c1)
        {
            return new C3d(
                (double)(c0.R + (c1.R)), 
                (double)(c0.G + (c1.G)), 
                (double)(c0.B + (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator -(C3d c0, C3d c1)
        {
            return new C3d(
                (double)(c0.R - (c1.R)), 
                (double)(c0.G - (c1.G)), 
                (double)(c0.B - (c1.B)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator *(C3d c0, C3d c1)
        {
            return new C3d((double)(c0.R * c1.R), (double)(c0.G * c1.G), (double)(c0.B * c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator /(C3d c0, C3d c1)
        {
            return new C3d((double)(c0.R / c1.R), (double)(c0.G / c1.G), (double)(c0.B / c1.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator +(C3d col, double scalar)
        {
            return new C3d((double)(col.R + scalar), (double)(col.G + scalar), (double)(col.B + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator +(double scalar, C3d col)
        {
            return new C3d((double)(scalar + col.R), (double)(scalar + col.G), (double)(scalar + col.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator -(C3d col, double scalar)
        {
            return new C3d((double)(col.R - scalar), (double)(col.G - scalar), (double)(col.B - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d operator -(double scalar, C3d col)
        {
            return new C3d((double)(scalar - col.R), (double)(scalar - col.G), (double)(scalar - col.B));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(double min, double max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d Clamped(double min, double max)
        {
            return new C3d(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public double Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Abs(R) + Fun.Abs(G) + Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public double NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public double NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C3d o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C3d, int, double> Setter =
            (ref C3d color, int i, double value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d DivideByInt(C3d c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C3d Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C3d(
                double.Parse(x[0], CultureInfo.InvariantCulture), 
                double.Parse(x[1], CultureInfo.InvariantCulture), 
                double.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        public static C3d Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<double>.Parse, C3d.Setter);
        }

        public static C3d Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<double>.Parse, C3d.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3d> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C3d other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return (R); }
            set { R = (value); }
        }

        double IRGB.Green
        {
            get { return (G); }
            set { G = (value); }
        }

        double IRGB.Blue
        {
            get { return (B); }
            set { B = (value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3d Lerp(this double x, C3d a, C3d b)
        {
            return new C3d(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B));
        }
        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C3d a, C3d b)
        {
            return ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C3d a, C3d b, double tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3d a, C3d b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C3d col, double s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(double s, C3d col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3d a, C3d b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C3d col, double s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(double s, C3d col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3d a, C3d b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C3d col, double s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(double s, C3d col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3d a, C3d b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C3d col, double s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(double s, C3d col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3d a, C3d b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C3d col, double s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(double s, C3d col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3d a, C3d b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C3d col, double s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(double s, C3d col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3d a, C3d b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C3d col, double s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(double s, C3d col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3d a, C3d b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C3d col, double s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(double s, C3d col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3d a, C3d b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C3d col, double s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(double s, C3d col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3d a, C3d b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C3d col, double s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(double s, C3d col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3d a, C3d b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C3d col, double s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(double s, C3d col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3d a, C3d b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C3d col, double s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(double s, C3d col)
        {
            return (s != col.R || s != col.G || s != col.B);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, ref Tup4<double> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<double> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        #endregion
    }

    public static class IRandomUniformC3dExtensions
    {
        #region IRandomUniform extensions for C3d

        /// <summary>
        /// Uses UniformDouble() to generate the elements of a C3d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d UniformC3d(this IRandomUniform rnd)
        {
            return new C3d(rnd.UniformDouble(), rnd.UniformDouble(), rnd.UniformDouble());
        }

        /// <summary>
        /// Uses UniformDoubleClosed() to generate the elements of a C3d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d UniformC3dClosed(this IRandomUniform rnd)
        {
            return new C3d(rnd.UniformDoubleClosed(), rnd.UniformDoubleClosed(), rnd.UniformDoubleClosed());
        }

        /// <summary>
        /// Uses UniformDoubleOpen() to generate the elements of a C3d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d UniformC3dOpen(this IRandomUniform rnd)
        {
            return new C3d(rnd.UniformDoubleOpen(), rnd.UniformDoubleOpen(), rnd.UniformDoubleOpen());
        }

        /// <summary>
        /// Uses UniformDoubleFull() to generate the elements of a C3d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d UniformC3dFull(this IRandomUniform rnd)
        {
            return new C3d(rnd.UniformDoubleFull(), rnd.UniformDoubleFull(), rnd.UniformDoubleFull());
        }

        /// <summary>
        /// Uses UniformDoubleFullClosed() to generate the elements of a C3d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d UniformC3dFullClosed(this IRandomUniform rnd)
        {
            return new C3d(rnd.UniformDoubleFullClosed(), rnd.UniformDoubleFullClosed(), rnd.UniformDoubleFullClosed());
        }

        /// <summary>
        /// Uses UniformDoubleFullOpen() to generate the elements of a C3d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d UniformC3dFullOpen(this IRandomUniform rnd)
        {
            return new C3d(rnd.UniformDoubleFullOpen(), rnd.UniformDoubleFullOpen(), rnd.UniformDoubleFullOpen());
        }

        #endregion
    }

    #endregion

    #region C4b

    /// <summary>
    /// Represents an RGBA color with each channel stored as a <see cref="byte"/> value within [0, 255].
    /// </summary>
    [Serializable]
    public partial struct C4b : IFormattable, IEquatable<C4b>, IRGB, IOpacity
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="byte"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(byte r, byte g, byte b, byte a)
        {
            R = r; G = g; B = b; A = a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(int r, int g, int b, int a)
        {
            R = (byte)r; G = (byte)g; B = (byte)b; A = (byte)a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(long r, long g, long b, long a)
        {
            R = (byte)r; G = (byte)g; B = (byte)b; A = (byte)a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(float r, float g, float b, float a)
        {
            R = Col.ByteFromFloatClamped(r);
            G = Col.ByteFromFloatClamped(g);
            B = Col.ByteFromFloatClamped(b);
            A = Col.ByteFromFloatClamped(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(double r, double g, double b, double a)
        {
            R = Col.ByteFromDoubleClamped(r);
            G = Col.ByteFromDoubleClamped(g);
            B = Col.ByteFromDoubleClamped(b);
            A = Col.ByteFromDoubleClamped(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="byte"/> RGB values.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(byte r, byte g, byte b)
        {
            R = r; G = g; B = b;
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> RGB values.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(int r, int g, int b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> RGB values.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(long r, long g, long b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> RGB values.
        /// The values are mapped from [0, 1] to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(float r, float g, float b)
        {
            
            R = Col.ByteFromFloatClamped(r); 
            G = Col.ByteFromFloatClamped(g); 
            B = Col.ByteFromFloatClamped(b);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> RGB values.
        /// The values are mapped from [0, 1] to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(double r, double g, double b)
        {
            R = Col.ByteFromDoubleClamped(r); G = Col.ByteFromDoubleClamped(g); B = Col.ByteFromDoubleClamped(b);
            A = 255;
        }

        /// <summary>
        /// Creates a color from a single <see cref="byte"/> value.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(byte gray)
        {
            R = gray; G = gray; B = gray; A = 255;
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(float gray)
        {
            var value = Col.ByteFromFloatClamped(gray);
            R = value; G = value; B = value; A = 255;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(double gray)
        {
            var value = Col.ByteFromDoubleClamped(gray);
            R = value; G = value; B = value; A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3b color, byte alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3us color, byte alpha)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3ui color, byte alpha)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3f color, byte alpha)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C3d color, byte alpha)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C4b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C4us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
            A = Col.ByteFromUShort(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C4ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
            A = Col.ByteFromUInt(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C4f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
            A = Col.ByteFromFloat(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(C4d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
            A = Col.ByteFromDouble(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3f vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3d vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = 255;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V4i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = (byte)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V4l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = (byte)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V4f vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = (byte)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V4d vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = (byte)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3i"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3i vec, byte alpha)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3l vec, byte alpha)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3f vec, byte alpha)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(V3d vec, byte alpha)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b(Func<int, byte> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C4b color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C4b color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C4b color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C4b color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C4b color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C4b color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C4b color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C4b color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C4b color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3i(C4b color)
            => new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3l(C4b color)
            => new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C4b color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C4b color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4i(C4b color)
            => new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4l(C4b color)
            => new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C4b color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C4b color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3b(C3b c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3us(C3us c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3ui(C3ui c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3f(C3f c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3d(C3d c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4us(C4us c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4ui(C4ui c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4f(C4f c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4d(C4d c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3i ToV3i() => (V3i)this;

        /// <summary>
        /// Creates a color from a <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV3i(V3i c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3l ToV3l() => (V3l)this;

        /// <summary>
        /// Creates a color from a <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV3l(V3l c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV3f(V3f c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// The alpha channel is set to 255.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV3d(V3d c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4i ToV4i() => (V4i)this;

        /// <summary>
        /// Creates a color from a <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV4i(V4i c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4l ToV4l() => (V4l)this;

        /// <summary>
        /// Creates a color from a <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV4l(V4l c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV4f(V4f c) => new C4b(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV4d(V4d c) => new C4b(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4b Map(Func<byte, byte> channel_fun)
        {
            return new C4b(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4us Map(Func<byte, ushort> channel_fun)
        {
            return new C4us(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4ui Map(Func<byte, uint> channel_fun)
        {
            return new C4ui(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4f Map(Func<byte, float> channel_fun)
        {
            return new C4f(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4d Map(Func<byte, double> channel_fun)
        {
            return new C4d(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        public void CopyTo<T>(T[] array, int start, Func<byte, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
            array[start + 3] = element_fun(A);
        }

        public void CopyTo<T>(T[] array, int start, Func<byte, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
            array[start + 3] = element_index_fun(A, 3);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe byte this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (byte* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (byte* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C4b with all components zero.
        /// </summary>
        public static C4b Zero => new C4b(0, 0, 0, 0);

        public static C4b Black => new C4b(0);

        public static C4b Red => new C4b(255, 0, 0);
        public static C4b Green => new C4b(0, 255, 0);
        public static C4b Blue => new C4b(0, 0, 255);
        public static C4b Cyan => new C4b(0, 255, 255);
        public static C4b Magenta => new C4b(255, 0, 255);
        public static C4b Yellow => new C4b(255, 255, 0);
        public static C4b White => new C4b(255);

        public static C4b DarkRed => new C4b(255 / 2, 0 / 2, 0 / 2);
        public static C4b DarkGreen => new C4b(0 / 2, 255 / 2, 0 / 2);
        public static C4b DarkBlue => new C4b(0 / 2, 0 / 2, 255 / 2);
        public static C4b DarkCyan => new C4b(0 / 2, 255 / 2, 255 / 2);
        public static C4b DarkMagenta => new C4b(255 / 2, 0 / 2, 255 / 2);
        public static C4b DarkYellow => new C4b(255 / 2, 255 / 2, 0 / 2);
        public static C4b Gray => new C4b(255 / 2);

        public static C4b VRVisGreen => new C4b(178, 217, 2);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C4b a, C4b b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C4b a, C4b b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator *(C4b col, float scalar)
        {
            return new C4b(
                (byte)Fun.Round(col.R * scalar), 
                (byte)Fun.Round(col.G * scalar), 
                (byte)Fun.Round(col.B * scalar), 
                (byte)Fun.Round(col.A * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator *(float scalar, C4b col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator /(C4b col, float scalar)
        {
            float f = 1 / scalar;
            return new C4b(
                (byte)Fun.Round(col.R * f), 
                (byte)Fun.Round(col.G * f), 
                (byte)Fun.Round(col.B * f), 
                (byte)Fun.Round(col.A * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator /(float scalar, C4b col)
        {
            return new C4b(
                (byte)Fun.Round(scalar / col.R), 
                (byte)Fun.Round(scalar / col.G), 
                (byte)Fun.Round(scalar / col.B), 
                (byte)Fun.Round(scalar / col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator *(C4b col, double scalar)
        {
            return new C4b(
                (byte)Fun.Round(col.R * scalar), 
                (byte)Fun.Round(col.G * scalar), 
                (byte)Fun.Round(col.B * scalar), 
                (byte)Fun.Round(col.A * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator *(double scalar, C4b col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator /(C4b col, double scalar)
        {
            double f = 1 / scalar;
            return new C4b(
                (byte)Fun.Round(col.R * f), 
                (byte)Fun.Round(col.G * f), 
                (byte)Fun.Round(col.B * f), 
                (byte)Fun.Round(col.A * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator /(double scalar, C4b col)
        {
            return new C4b(
                (byte)Fun.Round(scalar / col.R), 
                (byte)Fun.Round(scalar / col.G), 
                (byte)Fun.Round(scalar / col.B), 
                (byte)Fun.Round(scalar / col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator +(C4b c0, C4b c1)
        {
            return new C4b(
                (byte)(c0.R + (c1.R)), 
                (byte)(c0.G + (c1.G)), 
                (byte)(c0.B + (c1.B)), 
                (byte)(c0.A + (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator -(C4b c0, C4b c1)
        {
            return new C4b(
                (byte)(c0.R - (c1.R)), 
                (byte)(c0.G - (c1.G)), 
                (byte)(c0.B - (c1.B)), 
                (byte)(c0.A - (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator +(C4b c0, C4us c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G + Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B + Col.ByteFromUShort(c1.B)), 
                (byte)(c0.A + Col.ByteFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator -(C4b c0, C4us c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G - Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B - Col.ByteFromUShort(c1.B)), 
                (byte)(c0.A - Col.ByteFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator +(C4b c0, C4ui c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G + Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B + Col.ByteFromUInt(c1.B)), 
                (byte)(c0.A + Col.ByteFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator -(C4b c0, C4ui c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G - Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B - Col.ByteFromUInt(c1.B)), 
                (byte)(c0.A - Col.ByteFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator +(C4b c0, C4f c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G + Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B + Col.ByteFromFloat(c1.B)), 
                (byte)(c0.A + Col.ByteFromFloat(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator -(C4b c0, C4f c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G - Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B - Col.ByteFromFloat(c1.B)), 
                (byte)(c0.A - Col.ByteFromFloat(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator +(C4b c0, C4d c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G + Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B + Col.ByteFromDouble(c1.B)), 
                (byte)(c0.A + Col.ByteFromDouble(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator -(C4b c0, C4d c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G - Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B - Col.ByteFromDouble(c1.B)), 
                (byte)(c0.A - Col.ByteFromDouble(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator *(C4b c0, C4b c1)
        {
            return new C4b((byte)(c0.R * c1.R), (byte)(c0.G * c1.G), (byte)(c0.B * c1.B), (byte)(c0.A * c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator /(C4b c0, C4b c1)
        {
            return new C4b((byte)(c0.R / c1.R), (byte)(c0.G / c1.G), (byte)(c0.B / c1.B), (byte)(c0.A / c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator +(C4b col, byte scalar)
        {
            return new C4b((byte)(col.R + scalar), (byte)(col.G + scalar), (byte)(col.B + scalar), (byte)(col.A + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator +(byte scalar, C4b col)
        {
            return new C4b((byte)(scalar + col.R), (byte)(scalar + col.G), (byte)(scalar + col.B), (byte)(scalar + col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator -(C4b col, byte scalar)
        {
            return new C4b((byte)(col.R - scalar), (byte)(col.G - scalar), (byte)(col.B - scalar), (byte)(col.A - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b operator -(byte scalar, C4b col)
        {
            return new C4b((byte)(scalar - col.R), (byte)(scalar - col.G), (byte)(scalar - col.B), (byte)(scalar - col.A));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(byte min, byte max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b Clamped(byte min, byte max)
        {
            return new C4b(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public int Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return R + G + B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public byte NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public byte NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(R, G, B); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C4b o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + A.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C4b, int, byte> Setter =
            (ref C4b color, int i, byte value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    case 3: color.A = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b DivideByInt(C4b c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C4b Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C4b Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C4b(
                byte.Parse(x[0], CultureInfo.InvariantCulture), 
                byte.Parse(x[1], CultureInfo.InvariantCulture), 
                byte.Parse(x[2], CultureInfo.InvariantCulture), 
                byte.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        public static C4b Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<byte>.Parse, C4b.Setter);
        }

        public static C4b Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<byte>.Parse, C4b.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4b> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C4b other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return Col.DoubleFromByte(R); }
            set { R = Col.ByteFromDoubleClamped(value); }
        }

        double IRGB.Green
        {
            get { return Col.DoubleFromByte(G); }
            set { G = Col.ByteFromDoubleClamped(value); }
        }

        double IRGB.Blue
        {
            get { return Col.DoubleFromByte(B); }
            set { B = Col.ByteFromDoubleClamped(value); }
        }

        #endregion

        #region IOpacity Members

        public double Opacity
        {
            get { return Col.DoubleFromByte(A); }
            set { A = Col.ByteFromDoubleClamped(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4b Lerp(this float x, C4b a, C4b b)
        {
            return new C4b(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4b Lerp(this double x, C4b a, C4b b)
        {
            return new C4b(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }

        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C4b a, C4b b, byte tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance) && ApproximateEquals(a.A, b.A, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4b a, C4b b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4b col, byte s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(byte s, C4b col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4b a, C4b b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4b col, byte s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(byte s, C4b col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4b a, C4b b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4b col, byte s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(byte s, C4b col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4b a, C4b b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4b col, byte s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(byte s, C4b col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4b a, C4b b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4b col, byte s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(byte s, C4b col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4b a, C4b b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4b col, byte s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(byte s, C4b col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4b a, C4b b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4b col, byte s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(byte s, C4b col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4b a, C4b b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4b col, byte s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(byte s, C4b col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4b a, C4b b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4b col, byte s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(byte s, C4b col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4b a, C4b b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4b col, byte s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(byte s, C4b col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4b a, C4b b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4b col, byte s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(byte s, C4b col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4b a, C4b b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4b col, byte s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(byte s, C4b col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, ref Tup4<float> w)
        {
            return new C4b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4b p0, C4b p1, C4b p2, C4b p3, ref Tup4<float> w)
        {
            return new C4f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, ref Tup4<double> w)
        {
            return new C4b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4b p0, C4b p1, C4b p2, C4b p3, ref Tup4<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, C4b p4, C4b p5, ref Tup6<float> w)
        {
            return new C4b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4b p0, C4b p1, C4b p2, C4b p3, C4b p4, C4b p5, ref Tup6<float> w)
        {
            return new C4f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, C4b p4, C4b p5, ref Tup6<double> w)
        {
            return new C4b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4b p0, C4b p1, C4b p2, C4b p3, C4b p4, C4b p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        #endregion
    }

    #endregion

    #region C4us

    /// <summary>
    /// Represents an RGBA color with each channel stored as a <see cref="ushort"/> value within [0, 2^16 - 1].
    /// </summary>
    [Serializable]
    public partial struct C4us : IFormattable, IEquatable<C4us>, IRGB, IOpacity
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="ushort"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(ushort r, ushort g, ushort b, ushort a)
        {
            R = r; G = g; B = b; A = a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(int r, int g, int b, int a)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b; A = (ushort)a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(long r, long g, long b, long a)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b; A = (ushort)a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(float r, float g, float b, float a)
        {
            R = Col.UShortFromFloatClamped(r);
            G = Col.UShortFromFloatClamped(g);
            B = Col.UShortFromFloatClamped(b);
            A = Col.UShortFromFloatClamped(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(double r, double g, double b, double a)
        {
            R = Col.UShortFromDoubleClamped(r);
            G = Col.UShortFromDoubleClamped(g);
            B = Col.UShortFromDoubleClamped(b);
            A = Col.UShortFromDoubleClamped(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="ushort"/> RGB values.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(ushort r, ushort g, ushort b)
        {
            R = r; G = g; B = b;
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> RGB values.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(int r, int g, int b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> RGB values.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(long r, long g, long b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> RGB values.
        /// The values are mapped from [0, 1] to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(float r, float g, float b)
        {
            
            R = Col.UShortFromFloatClamped(r); 
            G = Col.UShortFromFloatClamped(g); 
            B = Col.UShortFromFloatClamped(b);
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> RGB values.
        /// The values are mapped from [0, 1] to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(double r, double g, double b)
        {
            R = Col.UShortFromDoubleClamped(r); G = Col.UShortFromDoubleClamped(g); B = Col.UShortFromDoubleClamped(b);
            A = 65535;
        }

        /// <summary>
        /// Creates a color from a single <see cref="ushort"/> value.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(ushort gray)
        {
            R = gray; G = gray; B = gray; A = 65535;
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(float gray)
        {
            var value = Col.UShortFromFloatClamped(gray);
            R = value; G = value; B = value; A = 65535;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(double gray)
        {
            var value = Col.UShortFromDoubleClamped(gray);
            R = value; G = value; B = value; A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3b color, ushort alpha)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3us color, ushort alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3ui color, ushort alpha)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3f color, ushort alpha)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
            A = 65535;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C3d color, ushort alpha)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C4b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
            A = Col.UShortFromByte(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C4us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C4ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
            A = Col.UShortFromUInt(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C4f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
            A = Col.UShortFromFloat(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(C4d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
            A = Col.UShortFromDouble(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = 2^16 - 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = 2^16 - 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3f vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = 2^16 - 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3d vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = 2^16 - 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V4i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = (ushort)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V4l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = (ushort)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V4f vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = (ushort)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V4d vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = (ushort)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3i"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3i vec, ushort alpha)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3l vec, ushort alpha)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3f vec, ushort alpha)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(V3d vec, ushort alpha)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us(Func<int, ushort> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C4us color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C4us color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C4us color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C4us color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C4us color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C4us color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C4us color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C4us color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C4us color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3i(C4us color)
            => new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3l(C4us color)
            => new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C4us color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C4us color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4i(C4us color)
            => new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4l(C4us color)
            => new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C4us color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C4us color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3b(C3b c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3us(C3us c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3ui(C3ui c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3f(C3f c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3d(C3d c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4b(C4b c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4ui(C4ui c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4f(C4f c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4d(C4d c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3i ToV3i() => (V3i)this;

        /// <summary>
        /// Creates a color from a <see cref="V3i"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV3i(V3i c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3l ToV3l() => (V3l)this;

        /// <summary>
        /// Creates a color from a <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV3l(V3l c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV3f(V3f c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// The alpha channel is set to 2^16 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV3d(V3d c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4i"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4i ToV4i() => (V4i)this;

        /// <summary>
        /// Creates a color from a <see cref="V4i"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV4i(V4i c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4l ToV4l() => (V4l)this;

        /// <summary>
        /// Creates a color from a <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV4l(V4l c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV4f(V4f c) => new C4us(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV4d(V4d c) => new C4us(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4b Map(Func<ushort, byte> channel_fun)
        {
            return new C4b(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4us Map(Func<ushort, ushort> channel_fun)
        {
            return new C4us(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4ui Map(Func<ushort, uint> channel_fun)
        {
            return new C4ui(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4f Map(Func<ushort, float> channel_fun)
        {
            return new C4f(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4d Map(Func<ushort, double> channel_fun)
        {
            return new C4d(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        public void CopyTo<T>(T[] array, int start, Func<ushort, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
            array[start + 3] = element_fun(A);
        }

        public void CopyTo<T>(T[] array, int start, Func<ushort, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
            array[start + 3] = element_index_fun(A, 3);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe ushort this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (ushort* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (ushort* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C4us with all components zero.
        /// </summary>
        public static C4us Zero => new C4us(0, 0, 0, 0);

        public static C4us Black => new C4us(0);

        public static C4us Red => new C4us(65535, 0, 0);
        public static C4us Green => new C4us(0, 65535, 0);
        public static C4us Blue => new C4us(0, 0, 65535);
        public static C4us Cyan => new C4us(0, 65535, 65535);
        public static C4us Magenta => new C4us(65535, 0, 65535);
        public static C4us Yellow => new C4us(65535, 65535, 0);
        public static C4us White => new C4us(65535);

        public static C4us DarkRed => new C4us(65535 / 2, 0 / 2, 0 / 2);
        public static C4us DarkGreen => new C4us(0 / 2, 65535 / 2, 0 / 2);
        public static C4us DarkBlue => new C4us(0 / 2, 0 / 2, 65535 / 2);
        public static C4us DarkCyan => new C4us(0 / 2, 65535 / 2, 65535 / 2);
        public static C4us DarkMagenta => new C4us(65535 / 2, 0 / 2, 65535 / 2);
        public static C4us DarkYellow => new C4us(65535 / 2, 65535 / 2, 0 / 2);
        public static C4us Gray => new C4us(65535 / 2);

        public static C4us VRVisGreen => new C4us(45743, 53411, 5243);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C4us a, C4us b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C4us a, C4us b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator *(C4us col, float scalar)
        {
            return new C4us(
                (ushort)Fun.Round(col.R * scalar), 
                (ushort)Fun.Round(col.G * scalar), 
                (ushort)Fun.Round(col.B * scalar), 
                (ushort)Fun.Round(col.A * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator *(float scalar, C4us col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator /(C4us col, float scalar)
        {
            float f = 1 / scalar;
            return new C4us(
                (ushort)Fun.Round(col.R * f), 
                (ushort)Fun.Round(col.G * f), 
                (ushort)Fun.Round(col.B * f), 
                (ushort)Fun.Round(col.A * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator /(float scalar, C4us col)
        {
            return new C4us(
                (ushort)Fun.Round(scalar / col.R), 
                (ushort)Fun.Round(scalar / col.G), 
                (ushort)Fun.Round(scalar / col.B), 
                (ushort)Fun.Round(scalar / col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator *(C4us col, double scalar)
        {
            return new C4us(
                (ushort)Fun.Round(col.R * scalar), 
                (ushort)Fun.Round(col.G * scalar), 
                (ushort)Fun.Round(col.B * scalar), 
                (ushort)Fun.Round(col.A * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator *(double scalar, C4us col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator /(C4us col, double scalar)
        {
            double f = 1 / scalar;
            return new C4us(
                (ushort)Fun.Round(col.R * f), 
                (ushort)Fun.Round(col.G * f), 
                (ushort)Fun.Round(col.B * f), 
                (ushort)Fun.Round(col.A * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator /(double scalar, C4us col)
        {
            return new C4us(
                (ushort)Fun.Round(scalar / col.R), 
                (ushort)Fun.Round(scalar / col.G), 
                (ushort)Fun.Round(scalar / col.B), 
                (ushort)Fun.Round(scalar / col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator +(C4us c0, C4b c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G + Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B + Col.UShortFromByte(c1.B)), 
                (ushort)(c0.A + Col.UShortFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator -(C4us c0, C4b c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G - Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B - Col.UShortFromByte(c1.B)), 
                (ushort)(c0.A - Col.UShortFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator +(C4us c0, C4us c1)
        {
            return new C4us(
                (ushort)(c0.R + (c1.R)), 
                (ushort)(c0.G + (c1.G)), 
                (ushort)(c0.B + (c1.B)), 
                (ushort)(c0.A + (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator -(C4us c0, C4us c1)
        {
            return new C4us(
                (ushort)(c0.R - (c1.R)), 
                (ushort)(c0.G - (c1.G)), 
                (ushort)(c0.B - (c1.B)), 
                (ushort)(c0.A - (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator +(C4us c0, C4ui c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G + Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B + Col.UShortFromUInt(c1.B)), 
                (ushort)(c0.A + Col.UShortFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator -(C4us c0, C4ui c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G - Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B - Col.UShortFromUInt(c1.B)), 
                (ushort)(c0.A - Col.UShortFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator +(C4us c0, C4f c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G + Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B + Col.UShortFromFloat(c1.B)), 
                (ushort)(c0.A + Col.UShortFromFloat(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator -(C4us c0, C4f c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G - Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B - Col.UShortFromFloat(c1.B)), 
                (ushort)(c0.A - Col.UShortFromFloat(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator +(C4us c0, C4d c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G + Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B + Col.UShortFromDouble(c1.B)), 
                (ushort)(c0.A + Col.UShortFromDouble(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator -(C4us c0, C4d c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G - Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B - Col.UShortFromDouble(c1.B)), 
                (ushort)(c0.A - Col.UShortFromDouble(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator *(C4us c0, C4us c1)
        {
            return new C4us((ushort)(c0.R * c1.R), (ushort)(c0.G * c1.G), (ushort)(c0.B * c1.B), (ushort)(c0.A * c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator /(C4us c0, C4us c1)
        {
            return new C4us((ushort)(c0.R / c1.R), (ushort)(c0.G / c1.G), (ushort)(c0.B / c1.B), (ushort)(c0.A / c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator +(C4us col, ushort scalar)
        {
            return new C4us((ushort)(col.R + scalar), (ushort)(col.G + scalar), (ushort)(col.B + scalar), (ushort)(col.A + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator +(ushort scalar, C4us col)
        {
            return new C4us((ushort)(scalar + col.R), (ushort)(scalar + col.G), (ushort)(scalar + col.B), (ushort)(scalar + col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator -(C4us col, ushort scalar)
        {
            return new C4us((ushort)(col.R - scalar), (ushort)(col.G - scalar), (ushort)(col.B - scalar), (ushort)(col.A - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us operator -(ushort scalar, C4us col)
        {
            return new C4us((ushort)(scalar - col.R), (ushort)(scalar - col.G), (ushort)(scalar - col.B), (ushort)(scalar - col.A));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(ushort min, ushort max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us Clamped(ushort min, ushort max)
        {
            return new C4us(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public int Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return R + G + B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public ushort NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public ushort NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(R, G, B); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C4us o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + A.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C4us, int, ushort> Setter =
            (ref C4us color, int i, ushort value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    case 3: color.A = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us DivideByInt(C4us c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C4us Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C4us Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C4us(
                ushort.Parse(x[0], CultureInfo.InvariantCulture), 
                ushort.Parse(x[1], CultureInfo.InvariantCulture), 
                ushort.Parse(x[2], CultureInfo.InvariantCulture), 
                ushort.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        public static C4us Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<ushort>.Parse, C4us.Setter);
        }

        public static C4us Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<ushort>.Parse, C4us.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4us> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C4us other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return Col.DoubleFromUShort(R); }
            set { R = Col.UShortFromDoubleClamped(value); }
        }

        double IRGB.Green
        {
            get { return Col.DoubleFromUShort(G); }
            set { G = Col.UShortFromDoubleClamped(value); }
        }

        double IRGB.Blue
        {
            get { return Col.DoubleFromUShort(B); }
            set { B = Col.UShortFromDoubleClamped(value); }
        }

        #endregion

        #region IOpacity Members

        public double Opacity
        {
            get { return Col.DoubleFromUShort(A); }
            set { A = Col.UShortFromDoubleClamped(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4us Lerp(this float x, C4us a, C4us b)
        {
            return new C4us(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4us Lerp(this double x, C4us a, C4us b)
        {
            return new C4us(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }

        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C4us a, C4us b, ushort tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance) && ApproximateEquals(a.A, b.A, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4us a, C4us b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4us col, ushort s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(ushort s, C4us col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4us a, C4us b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4us col, ushort s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(ushort s, C4us col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4us a, C4us b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4us col, ushort s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(ushort s, C4us col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4us a, C4us b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4us col, ushort s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(ushort s, C4us col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4us a, C4us b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4us col, ushort s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(ushort s, C4us col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4us a, C4us b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4us col, ushort s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(ushort s, C4us col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4us a, C4us b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4us col, ushort s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(ushort s, C4us col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4us a, C4us b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4us col, ushort s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(ushort s, C4us col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4us a, C4us b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4us col, ushort s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(ushort s, C4us col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4us a, C4us b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4us col, ushort s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(ushort s, C4us col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4us a, C4us b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4us col, ushort s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(ushort s, C4us col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4us a, C4us b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4us col, ushort s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(ushort s, C4us col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, ref Tup4<float> w)
        {
            return new C4us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4us p0, C4us p1, C4us p2, C4us p3, ref Tup4<float> w)
        {
            return new C4f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, ref Tup4<double> w)
        {
            return new C4us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4us p0, C4us p1, C4us p2, C4us p3, ref Tup4<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, C4us p4, C4us p5, ref Tup6<float> w)
        {
            return new C4us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4us p0, C4us p1, C4us p2, C4us p3, C4us p4, C4us p5, ref Tup6<float> w)
        {
            return new C4f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, C4us p4, C4us p5, ref Tup6<double> w)
        {
            return new C4us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4us p0, C4us p1, C4us p2, C4us p3, C4us p4, C4us p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        #endregion
    }

    #endregion

    #region C4ui

    /// <summary>
    /// Represents an RGBA color with each channel stored as a <see cref="uint"/> value within [0, 2^32 - 1].
    /// </summary>
    [Serializable]
    public partial struct C4ui : IFormattable, IEquatable<C4ui>, IRGB, IOpacity
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="uint"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(uint r, uint g, uint b, uint a)
        {
            R = r; G = g; B = b; A = a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(int r, int g, int b, int a)
        {
            R = (uint)r; G = (uint)g; B = (uint)b; A = (uint)a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(long r, long g, long b, long a)
        {
            R = (uint)r; G = (uint)g; B = (uint)b; A = (uint)a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(float r, float g, float b, float a)
        {
            R = Col.UIntFromFloatClamped(r);
            G = Col.UIntFromFloatClamped(g);
            B = Col.UIntFromFloatClamped(b);
            A = Col.UIntFromFloatClamped(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// The values are mapped from [0, 1] to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(double r, double g, double b, double a)
        {
            R = Col.UIntFromDoubleClamped(r);
            G = Col.UIntFromDoubleClamped(g);
            B = Col.UIntFromDoubleClamped(b);
            A = Col.UIntFromDoubleClamped(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="uint"/> RGB values.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(uint r, uint g, uint b)
        {
            R = r; G = g; B = b;
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="int"/> RGB values.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(int r, int g, int b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> RGB values.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(long r, long g, long b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> RGB values.
        /// The values are mapped from [0, 1] to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(float r, float g, float b)
        {
            
            R = Col.UIntFromFloatClamped(r); 
            G = Col.UIntFromFloatClamped(g); 
            B = Col.UIntFromFloatClamped(b);
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> RGB values.
        /// The values are mapped from [0, 1] to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(double r, double g, double b)
        {
            R = Col.UIntFromDoubleClamped(r); G = Col.UIntFromDoubleClamped(g); B = Col.UIntFromDoubleClamped(b);
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from a single <see cref="uint"/> value.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(uint gray)
        {
            R = gray; G = gray; B = gray; A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(float gray)
        {
            var value = Col.UIntFromFloatClamped(gray);
            R = value; G = value; B = value; A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The value is mapped from [0, 1] to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(double gray)
        {
            var value = Col.UIntFromDoubleClamped(gray);
            R = value; G = value; B = value; A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3b color, uint alpha)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3us color, uint alpha)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3ui color, uint alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3f color, uint alpha)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
            A = UInt32.MaxValue;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C3d color, uint alpha)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C4b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
            A = Col.UIntFromByte(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C4us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
            A = Col.UIntFromUShort(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C4ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C4f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
            A = Col.UIntFromFloat(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(C4d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
            A = Col.UIntFromDouble(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V3l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = 2^32 - 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V3f vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = 2^32 - 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V3d vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = 2^32 - 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V4l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = (uint)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V4f vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = (uint)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V4d vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = (uint)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3l"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V3l vec, uint alpha)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V3f vec, uint alpha)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector and an alpha value.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(V3d vec, uint alpha)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui(Func<int, uint> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C4ui color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C4ui color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C4ui color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C4ui color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C4ui color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C4ui color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C4ui color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C4ui color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C4ui color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3l(C4ui color)
            => new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C4ui color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C4ui color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4l(C4ui color)
            => new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C4ui color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C4ui color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3b(C3b c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3us(C3us c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3ui(C3ui c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C3f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3f(C3f c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C3d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3d(C3d c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4b(C4b c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4us(C4us c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4f(C4f c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4d(C4d c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3l ToV3l() => (V3l)this;

        /// <summary>
        /// Creates a color from a <see cref="V3l"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV3l(V3l c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV3f(V3f c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// The alpha channel is set to 2^32 - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV3d(V3d c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4l"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4l ToV4l() => (V4l)this;

        /// <summary>
        /// Creates a color from a <see cref="V4l"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV4l(V4l c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV4f(V4f c) => new C4ui(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// The values are not mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV4d(V4d c) => new C4ui(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4b Map(Func<uint, byte> channel_fun)
        {
            return new C4b(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4us Map(Func<uint, ushort> channel_fun)
        {
            return new C4us(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4ui Map(Func<uint, uint> channel_fun)
        {
            return new C4ui(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4f Map(Func<uint, float> channel_fun)
        {
            return new C4f(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4d Map(Func<uint, double> channel_fun)
        {
            return new C4d(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        public void CopyTo<T>(T[] array, int start, Func<uint, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
            array[start + 3] = element_fun(A);
        }

        public void CopyTo<T>(T[] array, int start, Func<uint, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
            array[start + 3] = element_index_fun(A, 3);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe uint this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (uint* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (uint* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C4ui with all components zero.
        /// </summary>
        public static C4ui Zero => new C4ui(0, 0, 0, 0);

        public static C4ui Black => new C4ui(0);

        public static C4ui Red => new C4ui(UInt32.MaxValue, 0, 0);
        public static C4ui Green => new C4ui(0, UInt32.MaxValue, 0);
        public static C4ui Blue => new C4ui(0, 0, UInt32.MaxValue);
        public static C4ui Cyan => new C4ui(0, UInt32.MaxValue, UInt32.MaxValue);
        public static C4ui Magenta => new C4ui(UInt32.MaxValue, 0, UInt32.MaxValue);
        public static C4ui Yellow => new C4ui(UInt32.MaxValue, UInt32.MaxValue, 0);
        public static C4ui White => new C4ui(UInt32.MaxValue);

        public static C4ui DarkRed => new C4ui(UInt32.MaxValue / 2, 0 / 2, 0 / 2);
        public static C4ui DarkGreen => new C4ui(0 / 2, UInt32.MaxValue / 2, 0 / 2);
        public static C4ui DarkBlue => new C4ui(0 / 2, 0 / 2, UInt32.MaxValue / 2);
        public static C4ui DarkCyan => new C4ui(0 / 2, UInt32.MaxValue / 2, UInt32.MaxValue / 2);
        public static C4ui DarkMagenta => new C4ui(UInt32.MaxValue / 2, 0 / 2, UInt32.MaxValue / 2);
        public static C4ui DarkYellow => new C4ui(UInt32.MaxValue / 2, UInt32.MaxValue / 2, 0 / 2);
        public static C4ui Gray => new C4ui(UInt32.MaxValue / 2);


        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C4ui a, C4ui b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C4ui a, C4ui b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator *(C4ui col, float scalar)
        {
            return new C4ui(
                (uint)Fun.Round(col.R * scalar), 
                (uint)Fun.Round(col.G * scalar), 
                (uint)Fun.Round(col.B * scalar), 
                (uint)Fun.Round(col.A * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator *(float scalar, C4ui col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator /(C4ui col, float scalar)
        {
            float f = 1 / scalar;
            return new C4ui(
                (uint)Fun.Round(col.R * f), 
                (uint)Fun.Round(col.G * f), 
                (uint)Fun.Round(col.B * f), 
                (uint)Fun.Round(col.A * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator /(float scalar, C4ui col)
        {
            return new C4ui(
                (uint)Fun.Round(scalar / col.R), 
                (uint)Fun.Round(scalar / col.G), 
                (uint)Fun.Round(scalar / col.B), 
                (uint)Fun.Round(scalar / col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator *(C4ui col, double scalar)
        {
            return new C4ui(
                (uint)Fun.Round(col.R * scalar), 
                (uint)Fun.Round(col.G * scalar), 
                (uint)Fun.Round(col.B * scalar), 
                (uint)Fun.Round(col.A * scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator *(double scalar, C4ui col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator /(C4ui col, double scalar)
        {
            double f = 1 / scalar;
            return new C4ui(
                (uint)Fun.Round(col.R * f), 
                (uint)Fun.Round(col.G * f), 
                (uint)Fun.Round(col.B * f), 
                (uint)Fun.Round(col.A * f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator /(double scalar, C4ui col)
        {
            return new C4ui(
                (uint)Fun.Round(scalar / col.R), 
                (uint)Fun.Round(scalar / col.G), 
                (uint)Fun.Round(scalar / col.B), 
                (uint)Fun.Round(scalar / col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator +(C4ui c0, C4b c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromByte(c1.R)), 
                (uint)(c0.G + Col.UIntFromByte(c1.G)), 
                (uint)(c0.B + Col.UIntFromByte(c1.B)), 
                (uint)(c0.A + Col.UIntFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator -(C4ui c0, C4b c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromByte(c1.R)), 
                (uint)(c0.G - Col.UIntFromByte(c1.G)), 
                (uint)(c0.B - Col.UIntFromByte(c1.B)), 
                (uint)(c0.A - Col.UIntFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator +(C4ui c0, C4us c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G + Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B + Col.UIntFromUShort(c1.B)), 
                (uint)(c0.A + Col.UIntFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator -(C4ui c0, C4us c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G - Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B - Col.UIntFromUShort(c1.B)), 
                (uint)(c0.A - Col.UIntFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator +(C4ui c0, C4ui c1)
        {
            return new C4ui(
                (uint)(c0.R + (c1.R)), 
                (uint)(c0.G + (c1.G)), 
                (uint)(c0.B + (c1.B)), 
                (uint)(c0.A + (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator -(C4ui c0, C4ui c1)
        {
            return new C4ui(
                (uint)(c0.R - (c1.R)), 
                (uint)(c0.G - (c1.G)), 
                (uint)(c0.B - (c1.B)), 
                (uint)(c0.A - (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator +(C4ui c0, C4f c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G + Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B + Col.UIntFromFloat(c1.B)), 
                (uint)(c0.A + Col.UIntFromFloat(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator -(C4ui c0, C4f c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G - Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B - Col.UIntFromFloat(c1.B)), 
                (uint)(c0.A - Col.UIntFromFloat(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator +(C4ui c0, C4d c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G + Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B + Col.UIntFromDouble(c1.B)), 
                (uint)(c0.A + Col.UIntFromDouble(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator -(C4ui c0, C4d c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G - Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B - Col.UIntFromDouble(c1.B)), 
                (uint)(c0.A - Col.UIntFromDouble(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator *(C4ui c0, C4ui c1)
        {
            return new C4ui((uint)(c0.R * c1.R), (uint)(c0.G * c1.G), (uint)(c0.B * c1.B), (uint)(c0.A * c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator /(C4ui c0, C4ui c1)
        {
            return new C4ui((uint)(c0.R / c1.R), (uint)(c0.G / c1.G), (uint)(c0.B / c1.B), (uint)(c0.A / c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator +(C4ui col, uint scalar)
        {
            return new C4ui((uint)(col.R + scalar), (uint)(col.G + scalar), (uint)(col.B + scalar), (uint)(col.A + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator +(uint scalar, C4ui col)
        {
            return new C4ui((uint)(scalar + col.R), (uint)(scalar + col.G), (uint)(scalar + col.B), (uint)(scalar + col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator -(C4ui col, uint scalar)
        {
            return new C4ui((uint)(col.R - scalar), (uint)(col.G - scalar), (uint)(col.B - scalar), (uint)(col.A - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui operator -(uint scalar, C4ui col)
        {
            return new C4ui((uint)(scalar - col.R), (uint)(scalar - col.G), (uint)(scalar - col.B), (uint)(scalar - col.A));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(uint min, uint max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui Clamped(uint min, uint max)
        {
            return new C4ui(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public long Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return R + G + B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public uint NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public uint NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(R, G, B); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C4ui o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + A.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C4ui, int, uint> Setter =
            (ref C4ui color, int i, uint value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    case 3: color.A = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui DivideByInt(C4ui c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C4ui Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C4ui Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C4ui(
                uint.Parse(x[0], CultureInfo.InvariantCulture), 
                uint.Parse(x[1], CultureInfo.InvariantCulture), 
                uint.Parse(x[2], CultureInfo.InvariantCulture), 
                uint.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        public static C4ui Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<uint>.Parse, C4ui.Setter);
        }

        public static C4ui Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<uint>.Parse, C4ui.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4ui> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C4ui other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return Col.DoubleFromUInt(R); }
            set { R = Col.UIntFromDoubleClamped(value); }
        }

        double IRGB.Green
        {
            get { return Col.DoubleFromUInt(G); }
            set { G = Col.UIntFromDoubleClamped(value); }
        }

        double IRGB.Blue
        {
            get { return Col.DoubleFromUInt(B); }
            set { B = Col.UIntFromDoubleClamped(value); }
        }

        #endregion

        #region IOpacity Members

        public double Opacity
        {
            get { return Col.DoubleFromUInt(A); }
            set { A = Col.UIntFromDoubleClamped(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4ui Lerp(this float x, C4ui a, C4ui b)
        {
            return new C4ui(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4ui Lerp(this double x, C4ui a, C4ui b)
        {
            return new C4ui(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }

        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C4ui a, C4ui b, uint tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance) && ApproximateEquals(a.A, b.A, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4ui a, C4ui b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4ui col, uint s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(uint s, C4ui col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4ui a, C4ui b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4ui col, uint s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(uint s, C4ui col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4ui a, C4ui b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4ui col, uint s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(uint s, C4ui col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4ui a, C4ui b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4ui col, uint s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(uint s, C4ui col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4ui a, C4ui b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4ui col, uint s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(uint s, C4ui col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4ui a, C4ui b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4ui col, uint s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(uint s, C4ui col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4ui a, C4ui b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4ui col, uint s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(uint s, C4ui col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4ui a, C4ui b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4ui col, uint s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(uint s, C4ui col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4ui a, C4ui b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4ui col, uint s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(uint s, C4ui col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4ui a, C4ui b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4ui col, uint s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(uint s, C4ui col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4ui a, C4ui b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4ui col, uint s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(uint s, C4ui col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4ui a, C4ui b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4ui col, uint s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(uint s, C4ui col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, ref Tup4<float> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, ref Tup4<float> w)
        {
            return new C4f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, ref Tup4<double> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, ref Tup4<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, C4ui p4, C4ui p5, ref Tup6<float> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, C4ui p4, C4ui p5, ref Tup6<float> w)
        {
            return new C4f(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, C4ui p4, C4ui p5, ref Tup6<double> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, C4ui p4, C4ui p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

        #endregion
    }

    public static class IRandomUniformC4uiExtensions
    {
        #region IRandomUniform extensions for C4ui

        /// <summary>
        /// Uses UniformUInt() to generate the elements of a C4ui color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui UniformC4ui(this IRandomUniform rnd)
        {
            return new C4ui(rnd.UniformUInt(), rnd.UniformUInt(), rnd.UniformUInt(), rnd.UniformUInt());
        }

        #endregion
    }

    #endregion

    #region C4f

    /// <summary>
    /// Represents an RGBA color with each channel stored as a <see cref="float"/> value within [0, 1].
    /// </summary>
    [Serializable]
    public partial struct C4f : IFormattable, IEquatable<C4f>, IRGB, IOpacity
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(float r, float g, float b, float a)
        {
            R = r; G = g; B = b; A = a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(double r, double g, double b, double a)
        {
            R = (float)(r);
            G = (float)(g);
            B = (float)(b);
            A = (float)(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> RGB values.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(float r, float g, float b)
        {
            R = r; G = g; B = b;
            A = 1.0f;
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> RGB values.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(double r, double g, double b)
        {
            R = (float)(r); G = (float)(g); B = (float)(b);
            A = 1.0f;
        }

        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(float gray)
        {
            R = gray; G = gray; B = gray; A = 1.0f;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
            A = 1.0f;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3b color, float alpha)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
            A = 1.0f;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3us color, float alpha)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
            A = 1.0f;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3ui color, float alpha)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 1.0f;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3f color, float alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
            A = 1.0f;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C3d color, float alpha)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C4b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
            A = Col.FloatFromByte(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C4us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
            A = Col.FloatFromUShort(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C4ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
            A = Col.FloatFromUInt(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C4f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(C4d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
            A = Col.FloatFromDouble(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(V3f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(V3d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
            A = 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(V4f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = (vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(V4d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
            A = (float)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(V3f vec, float alpha)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(V3d vec, float alpha)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f(Func<int, float> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C4f color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C4f color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C4f color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C4f color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C4f color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C4f color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C4f color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C4f color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4d(C4f color)
            => new C4d(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C4f color)
            => new V3f(
                (color.R), 
                (color.G), 
                (color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C4f color)
            => new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C4f color)
            => new V4f(
                (color.R), 
                (color.G), 
                (color.B),
                (color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C4f color)
            => new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3b(C3b c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3us(C3us c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3ui(C3ui c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3f(C3f c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3d(C3d c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4b(C4b c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4us(C4us c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4f"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4ui(C4ui c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d ToC4d() => (C4d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4d(C4d c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV3f(V3f c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV3d(V3d c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV4f(V4f c) => new C4f(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV4d(V4d c) => new C4f(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4b Map(Func<float, byte> channel_fun)
        {
            return new C4b(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4us Map(Func<float, ushort> channel_fun)
        {
            return new C4us(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4ui Map(Func<float, uint> channel_fun)
        {
            return new C4ui(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4f Map(Func<float, float> channel_fun)
        {
            return new C4f(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4d Map(Func<float, double> channel_fun)
        {
            return new C4d(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        public void CopyTo<T>(T[] array, int start, Func<float, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
            array[start + 3] = element_fun(A);
        }

        public void CopyTo<T>(T[] array, int start, Func<float, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
            array[start + 3] = element_index_fun(A, 3);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe float this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (float* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (float* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C4f with all components zero.
        /// </summary>
        public static C4f Zero => new C4f(0.0f, 0.0f, 0.0f, 0.0f);

        public static C4f Black => new C4f(0.0f);

        public static C4f Red => new C4f(1.0f, 0.0f, 0.0f);
        public static C4f Green => new C4f(0.0f, 1.0f, 0.0f);
        public static C4f Blue => new C4f(0.0f, 0.0f, 1.0f);
        public static C4f Cyan => new C4f(0.0f, 1.0f, 1.0f);
        public static C4f Magenta => new C4f(1.0f, 0.0f, 1.0f);
        public static C4f Yellow => new C4f(1.0f, 1.0f, 0.0f);
        public static C4f White => new C4f(1.0f);

        public static C4f DarkRed => new C4f(1.0f / 2, 0.0f / 2, 0.0f / 2);
        public static C4f DarkGreen => new C4f(0.0f / 2, 1.0f / 2, 0.0f / 2);
        public static C4f DarkBlue => new C4f(0.0f / 2, 0.0f / 2, 1.0f / 2);
        public static C4f DarkCyan => new C4f(0.0f / 2, 1.0f / 2, 1.0f / 2);
        public static C4f DarkMagenta => new C4f(1.0f / 2, 0.0f / 2, 1.0f / 2);
        public static C4f DarkYellow => new C4f(1.0f / 2, 1.0f / 2, 0.0f / 2);
        public static C4f Gray => new C4f(1.0f / 2);

        public static C4f Gray10 => new C4f(0.1f);
        public static C4f Gray20 => new C4f(0.2f);
        public static C4f Gray30 => new C4f(0.3f);
        public static C4f Gray40 => new C4f(0.4f);
        public static C4f Gray50 => new C4f(0.5f);
        public static C4f Gray60 => new C4f(0.6f);
        public static C4f Gray70 => new C4f(0.7f);
        public static C4f Gray80 => new C4f(0.8f);
        public static C4f Gray90 => new C4f(0.9f);
        public static C4f VRVisGreen => new C4f(0.698f, 0.851f, 0.008f);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C4f a, C4f b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C4f a, C4f b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator *(C4f col, float scalar)
        {
            return new C4f(
                col.R * scalar, 
                col.G * scalar, 
                col.B * scalar, 
                col.A * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator *(float scalar, C4f col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator /(C4f col, float scalar)
        {
            float f = 1 / scalar;
            return new C4f(
                col.R * f, 
                col.G * f, 
                col.B * f, 
                col.A * f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator /(float scalar, C4f col)
        {
            return new C4f(
                scalar / col.R, 
                scalar / col.G, 
                scalar / col.B, 
                scalar / col.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator +(C4f c0, C4b c1)
        {
            return new C4f(
                (float)(c0.R + Col.FloatFromByte(c1.R)), 
                (float)(c0.G + Col.FloatFromByte(c1.G)), 
                (float)(c0.B + Col.FloatFromByte(c1.B)), 
                (float)(c0.A + Col.FloatFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator -(C4f c0, C4b c1)
        {
            return new C4f(
                (float)(c0.R - Col.FloatFromByte(c1.R)), 
                (float)(c0.G - Col.FloatFromByte(c1.G)), 
                (float)(c0.B - Col.FloatFromByte(c1.B)), 
                (float)(c0.A - Col.FloatFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator +(C4f c0, C4us c1)
        {
            return new C4f(
                (float)(c0.R + Col.FloatFromUShort(c1.R)), 
                (float)(c0.G + Col.FloatFromUShort(c1.G)), 
                (float)(c0.B + Col.FloatFromUShort(c1.B)), 
                (float)(c0.A + Col.FloatFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator -(C4f c0, C4us c1)
        {
            return new C4f(
                (float)(c0.R - Col.FloatFromUShort(c1.R)), 
                (float)(c0.G - Col.FloatFromUShort(c1.G)), 
                (float)(c0.B - Col.FloatFromUShort(c1.B)), 
                (float)(c0.A - Col.FloatFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator +(C4f c0, C4ui c1)
        {
            return new C4f(
                (float)(c0.R + Col.FloatFromUInt(c1.R)), 
                (float)(c0.G + Col.FloatFromUInt(c1.G)), 
                (float)(c0.B + Col.FloatFromUInt(c1.B)), 
                (float)(c0.A + Col.FloatFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator -(C4f c0, C4ui c1)
        {
            return new C4f(
                (float)(c0.R - Col.FloatFromUInt(c1.R)), 
                (float)(c0.G - Col.FloatFromUInt(c1.G)), 
                (float)(c0.B - Col.FloatFromUInt(c1.B)), 
                (float)(c0.A - Col.FloatFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator +(C4f c0, C4f c1)
        {
            return new C4f(
                (float)(c0.R + (c1.R)), 
                (float)(c0.G + (c1.G)), 
                (float)(c0.B + (c1.B)), 
                (float)(c0.A + (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator -(C4f c0, C4f c1)
        {
            return new C4f(
                (float)(c0.R - (c1.R)), 
                (float)(c0.G - (c1.G)), 
                (float)(c0.B - (c1.B)), 
                (float)(c0.A - (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator +(C4f c0, C4d c1)
        {
            return new C4f(
                (float)(c0.R + (float)(c1.R)), 
                (float)(c0.G + (float)(c1.G)), 
                (float)(c0.B + (float)(c1.B)), 
                (float)(c0.A + (float)(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator -(C4f c0, C4d c1)
        {
            return new C4f(
                (float)(c0.R - (float)(c1.R)), 
                (float)(c0.G - (float)(c1.G)), 
                (float)(c0.B - (float)(c1.B)), 
                (float)(c0.A - (float)(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator *(C4f c0, C4f c1)
        {
            return new C4f((float)(c0.R * c1.R), (float)(c0.G * c1.G), (float)(c0.B * c1.B), (float)(c0.A * c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator /(C4f c0, C4f c1)
        {
            return new C4f((float)(c0.R / c1.R), (float)(c0.G / c1.G), (float)(c0.B / c1.B), (float)(c0.A / c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator +(C4f col, float scalar)
        {
            return new C4f((float)(col.R + scalar), (float)(col.G + scalar), (float)(col.B + scalar), (float)(col.A + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator +(float scalar, C4f col)
        {
            return new C4f((float)(scalar + col.R), (float)(scalar + col.G), (float)(scalar + col.B), (float)(scalar + col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator -(C4f col, float scalar)
        {
            return new C4f((float)(col.R - scalar), (float)(col.G - scalar), (float)(col.B - scalar), (float)(col.A - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f operator -(float scalar, C4f col)
        {
            return new C4f((float)(scalar - col.R), (float)(scalar - col.G), (float)(scalar - col.B), (float)(scalar - col.A));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(float min, float max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f Clamped(float min, float max)
        {
            return new C4f(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public float Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Abs(R) + Fun.Abs(G) + Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public float Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public float NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public float NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C4f o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + A.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C4f, int, float> Setter =
            (ref C4f color, int i, float value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    case 3: color.A = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f DivideByInt(C4f c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C4f Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C4f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C4f(
                float.Parse(x[0], CultureInfo.InvariantCulture), 
                float.Parse(x[1], CultureInfo.InvariantCulture), 
                float.Parse(x[2], CultureInfo.InvariantCulture), 
                float.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        public static C4f Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<float>.Parse, C4f.Setter);
        }

        public static C4f Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<float>.Parse, C4f.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4f> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C4f other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return (double)(R); }
            set { R = (float)(value); }
        }

        double IRGB.Green
        {
            get { return (double)(G); }
            set { G = (float)(value); }
        }

        double IRGB.Blue
        {
            get { return (double)(B); }
            set { B = (float)(value); }
        }

        #endregion

        #region IOpacity Members

        public double Opacity
        {
            get { return (double)(A); }
            set { A = (float)(value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4f Lerp(this float x, C4f a, C4f b)
        {
            return new C4f(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }
        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C4f a, C4f b)
        {
            return ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C4f a, C4f b, float tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance) && ApproximateEquals(a.A, b.A, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4f a, C4f b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4f col, float s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(float s, C4f col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4f a, C4f b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4f col, float s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(float s, C4f col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4f a, C4f b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4f col, float s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(float s, C4f col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4f a, C4f b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4f col, float s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(float s, C4f col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4f a, C4f b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4f col, float s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(float s, C4f col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4f a, C4f b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4f col, float s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(float s, C4f col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4f a, C4f b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4f col, float s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(float s, C4f col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4f a, C4f b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4f col, float s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(float s, C4f col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4f a, C4f b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4f col, float s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(float s, C4f col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4f a, C4f b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4f col, float s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(float s, C4f col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4f a, C4f b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4f col, float s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(float s, C4f col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4f a, C4f b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4f col, float s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(float s, C4f col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, ref Tup4<float> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<float> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        #endregion
    }

    public static class IRandomUniformC4fExtensions
    {
        #region IRandomUniform extensions for C4f

        /// <summary>
        /// Uses UniformFloat() to generate the elements of a C4f color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f UniformC4f(this IRandomUniform rnd)
        {
            return new C4f(rnd.UniformFloat(), rnd.UniformFloat(), rnd.UniformFloat(), rnd.UniformFloat());
        }

        /// <summary>
        /// Uses UniformFloatClosed() to generate the elements of a C4f color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f UniformC4fClosed(this IRandomUniform rnd)
        {
            return new C4f(rnd.UniformFloatClosed(), rnd.UniformFloatClosed(), rnd.UniformFloatClosed(), rnd.UniformFloatClosed());
        }

        /// <summary>
        /// Uses UniformFloatOpen() to generate the elements of a C4f color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f UniformC4fOpen(this IRandomUniform rnd)
        {
            return new C4f(rnd.UniformFloatOpen(), rnd.UniformFloatOpen(), rnd.UniformFloatOpen(), rnd.UniformFloatOpen());
        }

        #endregion
    }

    #endregion

    #region C4d

    /// <summary>
    /// Represents an RGBA color with each channel stored as a <see cref="double"/> value within [0, 1].
    /// </summary>
    [Serializable]
    public partial struct C4d : IFormattable, IEquatable<C4d>, IRGB, IOpacity
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(double r, double g, double b, double a)
        {
            R = r; G = g; B = b; A = a;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(float r, float g, float b, float a)
        {
            R = (double)(r);
            G = (double)(g);
            B = (double)(b);
            A = (double)(a);
        }

        /// <summary>
        /// Creates a color from the given <see cref="double"/> RGB values.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(double r, double g, double b)
        {
            R = r; G = g; B = b;
            A = 1.0;
        }

        /// <summary>
        /// Creates a color from the given <see cref="float"/> RGB values.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(float r, float g, float b)
        {
            
            R = (double)(r); 
            G = (double)(g); 
            B = (double)(b);
            A = 1.0;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(double gray)
        {
            R = gray; G = gray; B = gray; A = 1.0;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
            A = 1.0;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3b color, double alpha)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
            A = 1.0;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3us color, double alpha)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
            A = 1.0;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color and an alpha value.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3ui color, double alpha)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
            A = 1.0;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3f color, double alpha)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 1.0;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C3d color, double alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C4b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
            A = Col.DoubleFromByte(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C4us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
            A = Col.DoubleFromUShort(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C4ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
            A = Col.DoubleFromUInt(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C4f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
            A = Col.DoubleFromFloat(color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="C4d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(C4d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(V3f vec)
        {
            R = (double)(vec.X);
            G = (double)(vec.Y);
            B = (double)(vec.Z);
            A = 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(V3d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = 1;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(V4f vec)
        {
            R = (double)(vec.X);
            G = (double)(vec.Y);
            B = (double)(vec.Z);
            A = (double)(vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(V4d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = (vec.W);
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3f"/> vector and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(V3f vec, double alpha)
        {
            R = (double)(vec.X);
            G = (double)(vec.Y);
            B = (double)(vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the given <see cref="V3d"/> vector and an alpha value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(V3d vec, double alpha)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = alpha;
        }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d(Func<int, double> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3b(C4d color)
            => new C3b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3us(C4d color)
            => new C3us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3ui(C4d color)
            => new C3ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3f(C4d color)
            => new C3f(color);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C3d(C4d color)
            => new C3d(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4b(C4d color)
            => new C4b(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4us(C4d color)
            => new C4us(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4ui(C4d color)
            => new C4ui(color);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator C4f(C4d color)
            => new C4f(color);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3f(C4d color)
            => new V3f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V3d(C4d color)
            => new V3d(
                (color.R), 
                (color.G), 
                (color.B)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4f(C4d color)
            => new V4f(
                (float)(color.R), 
                (float)(color.G), 
                (float)(color.B),
                (float)(color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator V4d(C4d color)
            => new V4d(
                (color.R), 
                (color.G), 
                (color.B),
                (color.A)
                );

        /// <summary>
        /// Converts the given color to a <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C3b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3b ToC3b() => (C3b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3b"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3b(C3b c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C3us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3us ToC3us() => (C3us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3us"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3us(C3us c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C3ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3ui ToC3ui() => (C3ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3ui"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3ui(C3ui c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3f ToC3f() => (C3f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3f"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3f(C3f c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C3d"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C3d ToC3d() => (C3d)this;

        /// <summary>
        /// Creates a color from the given <see cref="C3d"/> color.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3d(C3d c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4b"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4b ToC4b() => (C4b)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4b"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4b(C4b c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4us"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4us ToC4us() => (C4us)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4us"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4us(C4us c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4ui"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4ui ToC4ui() => (C4ui)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4ui"/> color.
        /// The values are mapped to the <see cref="C4d"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4ui(C4ui c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4f ToC4f() => (C4f)this;

        /// <summary>
        /// Creates a color from the given <see cref="C4f"/> color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4f(C4f c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3f ToV3f() => (V3f)this;

        /// <summary>
        /// Creates a color from a <see cref="V3f"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromV3f(V3f c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V3d ToV3d() => (V3d)this;

        /// <summary>
        /// Creates a color from a <see cref="V3d"/> vector.
        /// The alpha channel is set to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromV3d(V3d c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4f ToV4f() => (V4f)this;

        /// <summary>
        /// Creates a color from a <see cref="V4f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromV4f(V4f c) => new C4d(c);

        /// <summary>
        /// Converts the given color to a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V4d ToV4d() => (V4d)this;

        /// <summary>
        /// Creates a color from a <see cref="V4d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromV4d(V4d c) => new C4d(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4b Map(Func<double, byte> channel_fun)
        {
            return new C4b(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4us Map(Func<double, ushort> channel_fun)
        {
            return new C4us(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4ui Map(Func<double, uint> channel_fun)
        {
            return new C4ui(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4f Map(Func<double, float> channel_fun)
        {
            return new C4f(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public C4d Map(Func<double, double> channel_fun)
        {
            return new C4d(channel_fun(R), channel_fun(G), channel_fun(B), channel_fun(A));
        }

        public void CopyTo<T>(T[] array, int start, Func<double, T> element_fun)
        {
            array[start + 0] = element_fun(R);
            array[start + 1] = element_fun(G);
            array[start + 2] = element_fun(B);
            array[start + 3] = element_fun(A);
        }

        public void CopyTo<T>(T[] array, int start, Func<double, int, T> element_index_fun)
        {
            array[start + 0] = element_index_fun(R, 0);
            array[start + 1] = element_index_fun(G, 1);
            array[start + 2] = element_index_fun(B, 2);
            array[start + 3] = element_index_fun(A, 3);
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe double this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (double* ptr = &R) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (double* ptr = &R) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// C4d with all components zero.
        /// </summary>
        public static C4d Zero => new C4d(0, 0, 0, 0);

        public static C4d Black => new C4d(0);

        public static C4d Red => new C4d(1.0, 0, 0);
        public static C4d Green => new C4d(0, 1.0, 0);
        public static C4d Blue => new C4d(0, 0, 1.0);
        public static C4d Cyan => new C4d(0, 1.0, 1.0);
        public static C4d Magenta => new C4d(1.0, 0, 1.0);
        public static C4d Yellow => new C4d(1.0, 1.0, 0);
        public static C4d White => new C4d(1.0);

        public static C4d DarkRed => new C4d(1.0 / 2, 0 / 2, 0 / 2);
        public static C4d DarkGreen => new C4d(0 / 2, 1.0 / 2, 0 / 2);
        public static C4d DarkBlue => new C4d(0 / 2, 0 / 2, 1.0 / 2);
        public static C4d DarkCyan => new C4d(0 / 2, 1.0 / 2, 1.0 / 2);
        public static C4d DarkMagenta => new C4d(1.0 / 2, 0 / 2, 1.0 / 2);
        public static C4d DarkYellow => new C4d(1.0 / 2, 1.0 / 2, 0 / 2);
        public static C4d Gray => new C4d(1.0 / 2);


        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(C4d a, C4d b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(C4d a, C4d b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Color Arithmetic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator *(C4d col, double scalar)
        {
            return new C4d(
                col.R * scalar, 
                col.G * scalar, 
                col.B * scalar, 
                col.A * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator *(double scalar, C4d col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator /(C4d col, double scalar)
        {
            double f = 1 / scalar;
            return new C4d(
                col.R * f, 
                col.G * f, 
                col.B * f, 
                col.A * f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator /(double scalar, C4d col)
        {
            return new C4d(
                scalar / col.R, 
                scalar / col.G, 
                scalar / col.B, 
                scalar / col.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator +(C4d c0, C4b c1)
        {
            return new C4d(
                (double)(c0.R + Col.DoubleFromByte(c1.R)), 
                (double)(c0.G + Col.DoubleFromByte(c1.G)), 
                (double)(c0.B + Col.DoubleFromByte(c1.B)), 
                (double)(c0.A + Col.DoubleFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator -(C4d c0, C4b c1)
        {
            return new C4d(
                (double)(c0.R - Col.DoubleFromByte(c1.R)), 
                (double)(c0.G - Col.DoubleFromByte(c1.G)), 
                (double)(c0.B - Col.DoubleFromByte(c1.B)), 
                (double)(c0.A - Col.DoubleFromByte(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator +(C4d c0, C4us c1)
        {
            return new C4d(
                (double)(c0.R + Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G + Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B + Col.DoubleFromUShort(c1.B)), 
                (double)(c0.A + Col.DoubleFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator -(C4d c0, C4us c1)
        {
            return new C4d(
                (double)(c0.R - Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G - Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B - Col.DoubleFromUShort(c1.B)), 
                (double)(c0.A - Col.DoubleFromUShort(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator +(C4d c0, C4ui c1)
        {
            return new C4d(
                (double)(c0.R + Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G + Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B + Col.DoubleFromUInt(c1.B)), 
                (double)(c0.A + Col.DoubleFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator -(C4d c0, C4ui c1)
        {
            return new C4d(
                (double)(c0.R - Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G - Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B - Col.DoubleFromUInt(c1.B)), 
                (double)(c0.A - Col.DoubleFromUInt(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator +(C4d c0, C4f c1)
        {
            return new C4d(
                (double)(c0.R + (double)(c1.R)), 
                (double)(c0.G + (double)(c1.G)), 
                (double)(c0.B + (double)(c1.B)), 
                (double)(c0.A + (double)(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator -(C4d c0, C4f c1)
        {
            return new C4d(
                (double)(c0.R - (double)(c1.R)), 
                (double)(c0.G - (double)(c1.G)), 
                (double)(c0.B - (double)(c1.B)), 
                (double)(c0.A - (double)(c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator +(C4d c0, C4d c1)
        {
            return new C4d(
                (double)(c0.R + (c1.R)), 
                (double)(c0.G + (c1.G)), 
                (double)(c0.B + (c1.B)), 
                (double)(c0.A + (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator -(C4d c0, C4d c1)
        {
            return new C4d(
                (double)(c0.R - (c1.R)), 
                (double)(c0.G - (c1.G)), 
                (double)(c0.B - (c1.B)), 
                (double)(c0.A - (c1.A)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator *(C4d c0, C4d c1)
        {
            return new C4d((double)(c0.R * c1.R), (double)(c0.G * c1.G), (double)(c0.B * c1.B), (double)(c0.A * c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator /(C4d c0, C4d c1)
        {
            return new C4d((double)(c0.R / c1.R), (double)(c0.G / c1.G), (double)(c0.B / c1.B), (double)(c0.A / c1.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator +(C4d col, double scalar)
        {
            return new C4d((double)(col.R + scalar), (double)(col.G + scalar), (double)(col.B + scalar), (double)(col.A + scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator +(double scalar, C4d col)
        {
            return new C4d((double)(scalar + col.R), (double)(scalar + col.G), (double)(scalar + col.B), (double)(scalar + col.A));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator -(C4d col, double scalar)
        {
            return new C4d((double)(col.R - scalar), (double)(col.G - scalar), (double)(col.B - scalar), (double)(col.A - scalar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d operator -(double scalar, C4d col)
        {
            return new C4d((double)(scalar - col.R), (double)(scalar - col.G), (double)(scalar - col.B), (double)(scalar - col.A));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(double min, double max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public C4d Clamped(double min, double max)
        {
            return new C4d(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public double Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Abs(R) + Fun.Abs(G) + Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(R * R + G * G + B * B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public double NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public double NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is C4d o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + G.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + B.ToString(null, CultureInfo.InvariantCulture)  + ", " 
                + A.ToString(null, CultureInfo.InvariantCulture) 
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<C4d, int, double> Setter =
            (ref C4d color, int i, double value) =>
            {
                switch (i)
                {
                    case 0: color.R = value; return;
                    case 1: color.G = value; return;
                    case 2: color.B = value; return;
                    case 3: color.A = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d DivideByInt(C4d c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static C4d Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static C4d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new C4d(
                double.Parse(x[0], CultureInfo.InvariantCulture), 
                double.Parse(x[1], CultureInfo.InvariantCulture), 
                double.Parse(x[2], CultureInfo.InvariantCulture), 
                double.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        public static C4d Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<double>.Parse, C4d.Setter);
        }

        public static C4d Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<double>.Parse, C4d.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4d> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(C4d other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return (R); }
            set { R = (value); }
        }

        double IRGB.Green
        {
            get { return (G); }
            set { G = (value); }
        }

        double IRGB.Blue
        {
            get { return (B); }
            set { B = (value); }
        }

        #endregion

        #region IOpacity Members

        public double Opacity
        {
            get { return (A); }
            set { A = (value); }
        }

        #endregion

    }

    public static partial class Fun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4d Lerp(this double x, C4d a, C4d b)
        {
            return new C4d(Lerp(x, a.R, b.R), Lerp(x, a.G, b.G), Lerp(x, a.B, b.B), Lerp(x, a.A, b.A));
        }
        #endregion

        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C4d a, C4d b)
        {
            return ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this C4d a, C4d b, double tolerance)
        {
            return ApproximateEquals(a.R, b.R, tolerance) && ApproximateEquals(a.G, b.G, tolerance) && ApproximateEquals(a.B, b.B, tolerance) && ApproximateEquals(a.A, b.A, tolerance);
        }

        #endregion
    }

    public static partial class Col
    {
        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4d a, C4d b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(this C4d col, double s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmaller(double s, C4d col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4d a, C4d b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(this C4d col, double s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmaller(double s, C4d col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4d a, C4d b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(this C4d col, double s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreater(double s, C4d col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4d a, C4d b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(this C4d col, double s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreater(double s, C4d col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4d a, C4d b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(this C4d col, double s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllSmallerOrEqual(double s, C4d col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4d a, C4d b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(this C4d col, double s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnySmallerOrEqual(double s, C4d col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4d a, C4d b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(this C4d col, double s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllGreaterOrEqual(double s, C4d col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4d a, C4d b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(this C4d col, double s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyGreaterOrEqual(double s, C4d col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4d a, C4d b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(this C4d col, double s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllEqual(double s, C4d col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4d a, C4d b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(this C4d col, double s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyEqual(double s, C4d col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4d a, C4d b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(this C4d col, double s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllDifferent(double s, C4d col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4d a, C4d b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(this C4d col, double s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyDifferent(double s, C4d col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
        }

        #endregion

        #region Linear Combination

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, ref Tup4<double> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<double> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        #endregion
    }

    public static class IRandomUniformC4dExtensions
    {
        #region IRandomUniform extensions for C4d

        /// <summary>
        /// Uses UniformDouble() to generate the elements of a C4d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d UniformC4d(this IRandomUniform rnd)
        {
            return new C4d(rnd.UniformDouble(), rnd.UniformDouble(), rnd.UniformDouble(), rnd.UniformDouble());
        }

        /// <summary>
        /// Uses UniformDoubleClosed() to generate the elements of a C4d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d UniformC4dClosed(this IRandomUniform rnd)
        {
            return new C4d(rnd.UniformDoubleClosed(), rnd.UniformDoubleClosed(), rnd.UniformDoubleClosed(), rnd.UniformDoubleClosed());
        }

        /// <summary>
        /// Uses UniformDoubleOpen() to generate the elements of a C4d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d UniformC4dOpen(this IRandomUniform rnd)
        {
            return new C4d(rnd.UniformDoubleOpen(), rnd.UniformDoubleOpen(), rnd.UniformDoubleOpen(), rnd.UniformDoubleOpen());
        }

        /// <summary>
        /// Uses UniformDoubleFull() to generate the elements of a C4d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d UniformC4dFull(this IRandomUniform rnd)
        {
            return new C4d(rnd.UniformDoubleFull(), rnd.UniformDoubleFull(), rnd.UniformDoubleFull(), rnd.UniformDoubleFull());
        }

        /// <summary>
        /// Uses UniformDoubleFullClosed() to generate the elements of a C4d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d UniformC4dFullClosed(this IRandomUniform rnd)
        {
            return new C4d(rnd.UniformDoubleFullClosed(), rnd.UniformDoubleFullClosed(), rnd.UniformDoubleFullClosed(), rnd.UniformDoubleFullClosed());
        }

        /// <summary>
        /// Uses UniformDoubleFullOpen() to generate the elements of a C4d color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d UniformC4dFullOpen(this IRandomUniform rnd)
        {
            return new C4d(rnd.UniformDoubleFullOpen(), rnd.UniformDoubleFullOpen(), rnd.UniformDoubleFullOpen(), rnd.UniformDoubleFullOpen());
        }

        #endregion
    }

    #endregion

}

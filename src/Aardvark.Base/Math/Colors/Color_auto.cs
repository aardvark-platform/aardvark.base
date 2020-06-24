using System;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region C3b

    [Serializable]
    public partial struct C3b : IFormattable, IEquatable<C3b>, IRGB
    {
        #region Constructors

        public C3b(byte r, byte g, byte b)
        {
            R = r; G = g; B = b;
        }

        public C3b(int r, int g, int b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
        }

        public C3b(long r, long g, long b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
        }

        public C3b(double r, double g, double b)
        {
            R = Col.ByteFromDoubleClamped(r);
            G = Col.ByteFromDoubleClamped(g);
            B = Col.ByteFromDoubleClamped(b);
        }

        public C3b(byte gray)
        {
            R = gray; G = gray; B = gray;
        }

        public C3b(double gray)
        {
            var value = Col.ByteFromDoubleClamped(gray);
            R = value; G = value; B = value;
        }

        public C3b(C3b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3b(C3us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
        }

        public C3b(C3ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
        }

        public C3b(C3f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
        }

        public C3b(C3d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
        }

        public C3b(C4b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3b(C4us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
        }

        public C3b(C4ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
        }

        public C3b(C4f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
        }

        public C3b(C4d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
        }

        public C3b(V3i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        public C3b(V3l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        public C3b(V4i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        public C3b(V4l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
        }

        #endregion

        #region Conversions

        public static explicit operator C3us(C3b color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C3b color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C3b color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C3b color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C3b color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C3b color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C3b color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C3b color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C3b color)
        {
            return new C4d(color);
        }

        public static explicit operator V3i(C3b color)
        {
            return new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );
        }

        public static explicit operator V3l(C3b color)
        {
            return new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );
        }

        public static explicit operator V4i(C3b color)
        {
            return new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(255)
                );
        }

        public static explicit operator V4l(C3b color)
        {
            return new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(255)
                );
        }

        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C3b(Func<int, byte> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        public V3i ToV3i() { return (V3i)this; }
        public V3l ToV3l() { return (V3l)this; }
        public V4i ToV4i() { return (V4i)this; }
        public V4l ToV4l() { return (V4l)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3us(C3us c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3ui(C3ui c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3f(C3f c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC3d(C3d c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4b(C4b c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4us(C4us c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4ui(C4ui c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4f(C4f c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromC4d(C4d c)
            => new C3b(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV3i(V3i c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV3l(V3l c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV4i(V4i c)
            => new C3b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3b FromV4l(V4l c)
            => new C3b(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3b Copy(Func<byte, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3us Copy(Func<byte, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3ui Copy(Func<byte, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3f Copy(Func<byte, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3d Copy(Func<byte, double> channel_fun)
        {
            return Map(channel_fun);
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
        public byte this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C3b with all components zero.
        /// </summary>
        public static C3b Zero => new C3b(0, 0, 0);

        // Web colors
        public static C3b AliceBlue => new C3b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(1));
        public static C3b AntiqueWhite => new C3b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.921569), Col.ByteFromDoubleClamped(0.843137));
        public static C3b Aqua => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C3b Aquamarine => new C3b(Col.ByteFromDoubleClamped(0.498039), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.831373));
        public static C3b Azure => new C3b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C3b Beige => new C3b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.862745));
        public static C3b Bisque => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.894118), Col.ByteFromDoubleClamped(0.768627));
        public static C3b Black => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C3b BlanchedAlmond => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.921569), Col.ByteFromDoubleClamped(0.803922));
        public static C3b Blue => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1));
        public static C3b BlueViolet => new C3b(Col.ByteFromDoubleClamped(0.541176), Col.ByteFromDoubleClamped(0.168627), Col.ByteFromDoubleClamped(0.886275));
        public static C3b Brown => new C3b(Col.ByteFromDoubleClamped(0.647059), Col.ByteFromDoubleClamped(0.164706), Col.ByteFromDoubleClamped(0.164706));
        public static C3b BurlyWood => new C3b(Col.ByteFromDoubleClamped(0.870588), Col.ByteFromDoubleClamped(0.721569), Col.ByteFromDoubleClamped(0.529412));
        public static C3b CadetBlue => new C3b(Col.ByteFromDoubleClamped(0.372549), Col.ByteFromDoubleClamped(0.619608), Col.ByteFromDoubleClamped(0.627451));
        public static C3b Chartreuse => new C3b(Col.ByteFromDoubleClamped(0.498039), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0));
        public static C3b Chocolate => new C3b(Col.ByteFromDoubleClamped(0.823529), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.117647));
        public static C3b Coral => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.498039), Col.ByteFromDoubleClamped(0.313725));
        public static C3b CornflowerBlue => new C3b(Col.ByteFromDoubleClamped(0.392157), Col.ByteFromDoubleClamped(0.584314), Col.ByteFromDoubleClamped(0.929412));
        public static C3b Cornsilk => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(0.862745));
        public static C3b Crimson => new C3b(Col.ByteFromDoubleClamped(0.862745), Col.ByteFromDoubleClamped(0.078431), Col.ByteFromDoubleClamped(0.235294));
        public static C3b Cyan => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C3b DarkBlue => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.545098));
        public static C3b DarkCyan => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.545098));
        public static C3b DarkGoldenRod => new C3b(Col.ByteFromDoubleClamped(0.721569), Col.ByteFromDoubleClamped(0.52549), Col.ByteFromDoubleClamped(0.043137));
        public static C3b DarkGray => new C3b(Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745));
        public static C3b DarkGrey => new C3b(Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745));
        public static C3b DarkGreen => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.392157), Col.ByteFromDoubleClamped(0));
        public static C3b DarkKhaki => new C3b(Col.ByteFromDoubleClamped(0.741176), Col.ByteFromDoubleClamped(0.717647), Col.ByteFromDoubleClamped(0.419608));
        public static C3b DarkMagenta => new C3b(Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.545098));
        public static C3b DarkOliveGreen => new C3b(Col.ByteFromDoubleClamped(0.333333), Col.ByteFromDoubleClamped(0.419608), Col.ByteFromDoubleClamped(0.184314));
        public static C3b DarkOrange => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.54902), Col.ByteFromDoubleClamped(0));
        public static C3b DarkOrchid => new C3b(Col.ByteFromDoubleClamped(0.6), Col.ByteFromDoubleClamped(0.196078), Col.ByteFromDoubleClamped(0.8));
        public static C3b DarkRed => new C3b(Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C3b DarkSalmon => new C3b(Col.ByteFromDoubleClamped(0.913725), Col.ByteFromDoubleClamped(0.588235), Col.ByteFromDoubleClamped(0.478431));
        public static C3b DarkSeaGreen => new C3b(Col.ByteFromDoubleClamped(0.560784), Col.ByteFromDoubleClamped(0.737255), Col.ByteFromDoubleClamped(0.560784));
        public static C3b DarkSlateBlue => new C3b(Col.ByteFromDoubleClamped(0.282353), Col.ByteFromDoubleClamped(0.239216), Col.ByteFromDoubleClamped(0.545098));
        public static C3b DarkSlateGray => new C3b(Col.ByteFromDoubleClamped(0.184314), Col.ByteFromDoubleClamped(0.309804), Col.ByteFromDoubleClamped(0.309804));
        public static C3b DarkSlateGrey => new C3b(Col.ByteFromDoubleClamped(0.184314), Col.ByteFromDoubleClamped(0.309804), Col.ByteFromDoubleClamped(0.309804));
        public static C3b DarkTurquoise => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.807843), Col.ByteFromDoubleClamped(0.819608));
        public static C3b DarkViolet => new C3b(Col.ByteFromDoubleClamped(0.580392), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.827451));
        public static C3b DeepPink => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.078431), Col.ByteFromDoubleClamped(0.576471));
        public static C3b DeepSkyBlue => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.74902), Col.ByteFromDoubleClamped(1));
        public static C3b DimGray => new C3b(Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765));
        public static C3b DimGrey => new C3b(Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765));
        public static C3b DodgerBlue => new C3b(Col.ByteFromDoubleClamped(0.117647), Col.ByteFromDoubleClamped(0.564706), Col.ByteFromDoubleClamped(1));
        public static C3b FireBrick => new C3b(Col.ByteFromDoubleClamped(0.698039), Col.ByteFromDoubleClamped(0.133333), Col.ByteFromDoubleClamped(0.133333));
        public static C3b FloralWhite => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.941176));
        public static C3b ForestGreen => new C3b(Col.ByteFromDoubleClamped(0.133333), Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.133333));
        public static C3b Fuchsia => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1));
        public static C3b Gainsboro => new C3b(Col.ByteFromDoubleClamped(0.862745), Col.ByteFromDoubleClamped(0.862745), Col.ByteFromDoubleClamped(0.862745));
        public static C3b GhostWhite => new C3b(Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(1));
        public static C3b Gold => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.843137), Col.ByteFromDoubleClamped(0));
        public static C3b GoldenRod => new C3b(Col.ByteFromDoubleClamped(0.854902), Col.ByteFromDoubleClamped(0.647059), Col.ByteFromDoubleClamped(0.12549));
        public static C3b Gray => new C3b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C3b Grey => new C3b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C3b Green => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0));
        public static C3b GreenYellow => new C3b(Col.ByteFromDoubleClamped(0.678431), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.184314));
        public static C3b HoneyDew => new C3b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.941176));
        public static C3b HotPink => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.705882));
        public static C3b IndianRed  => new C3b(Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.360784), Col.ByteFromDoubleClamped(0.360784));
        public static C3b Indigo  => new C3b(Col.ByteFromDoubleClamped(0.294118), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.509804));
        public static C3b Ivory => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.941176));
        public static C3b Khaki => new C3b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.901961), Col.ByteFromDoubleClamped(0.54902));
        public static C3b Lavender => new C3b(Col.ByteFromDoubleClamped(0.901961), Col.ByteFromDoubleClamped(0.901961), Col.ByteFromDoubleClamped(0.980392));
        public static C3b LavenderBlush => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.960784));
        public static C3b LawnGreen => new C3b(Col.ByteFromDoubleClamped(0.486275), Col.ByteFromDoubleClamped(0.988235), Col.ByteFromDoubleClamped(0));
        public static C3b LemonChiffon => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.803922));
        public static C3b LightBlue => new C3b(Col.ByteFromDoubleClamped(0.678431), Col.ByteFromDoubleClamped(0.847059), Col.ByteFromDoubleClamped(0.901961));
        public static C3b LightCoral => new C3b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C3b LightCyan => new C3b(Col.ByteFromDoubleClamped(0.878431), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C3b LightGoldenRodYellow => new C3b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.823529));
        public static C3b LightGray => new C3b(Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451));
        public static C3b LightGrey => new C3b(Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451));
        public static C3b LightGreen => new C3b(Col.ByteFromDoubleClamped(0.564706), Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.564706));
        public static C3b LightPink => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.713725), Col.ByteFromDoubleClamped(0.756863));
        public static C3b LightSalmon => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.627451), Col.ByteFromDoubleClamped(0.478431));
        public static C3b LightSeaGreen => new C3b(Col.ByteFromDoubleClamped(0.12549), Col.ByteFromDoubleClamped(0.698039), Col.ByteFromDoubleClamped(0.666667));
        public static C3b LightSkyBlue => new C3b(Col.ByteFromDoubleClamped(0.529412), Col.ByteFromDoubleClamped(0.807843), Col.ByteFromDoubleClamped(0.980392));
        public static C3b LightSlateGray => new C3b(Col.ByteFromDoubleClamped(0.466667), Col.ByteFromDoubleClamped(0.533333), Col.ByteFromDoubleClamped(0.6));
        public static C3b LightSlateGrey => new C3b(Col.ByteFromDoubleClamped(0.466667), Col.ByteFromDoubleClamped(0.533333), Col.ByteFromDoubleClamped(0.6));
        public static C3b LightSteelBlue => new C3b(Col.ByteFromDoubleClamped(0.690196), Col.ByteFromDoubleClamped(0.768627), Col.ByteFromDoubleClamped(0.870588));
        public static C3b LightYellow => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.878431));
        public static C3b Lime => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0));
        public static C3b LimeGreen => new C3b(Col.ByteFromDoubleClamped(0.196078), Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.196078));
        public static C3b Linen => new C3b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.901961));
        public static C3b Magenta => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1));
        public static C3b Maroon => new C3b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C3b MediumAquaMarine => new C3b(Col.ByteFromDoubleClamped(0.4), Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.666667));
        public static C3b MediumBlue => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.803922));
        public static C3b MediumOrchid => new C3b(Col.ByteFromDoubleClamped(0.729412), Col.ByteFromDoubleClamped(0.333333), Col.ByteFromDoubleClamped(0.827451));
        public static C3b MediumPurple => new C3b(Col.ByteFromDoubleClamped(0.576471), Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.847059));
        public static C3b MediumSeaGreen => new C3b(Col.ByteFromDoubleClamped(0.235294), Col.ByteFromDoubleClamped(0.701961), Col.ByteFromDoubleClamped(0.443137));
        public static C3b MediumSlateBlue => new C3b(Col.ByteFromDoubleClamped(0.482353), Col.ByteFromDoubleClamped(0.407843), Col.ByteFromDoubleClamped(0.933333));
        public static C3b MediumSpringGreen => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.603922));
        public static C3b MediumTurquoise => new C3b(Col.ByteFromDoubleClamped(0.282353), Col.ByteFromDoubleClamped(0.819608), Col.ByteFromDoubleClamped(0.8));
        public static C3b MediumVioletRed => new C3b(Col.ByteFromDoubleClamped(0.780392), Col.ByteFromDoubleClamped(0.082353), Col.ByteFromDoubleClamped(0.521569));
        public static C3b MidnightBlue => new C3b(Col.ByteFromDoubleClamped(0.098039), Col.ByteFromDoubleClamped(0.098039), Col.ByteFromDoubleClamped(0.439216));
        public static C3b MintCream => new C3b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392));
        public static C3b MistyRose => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.894118), Col.ByteFromDoubleClamped(0.882353));
        public static C3b Moccasin => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.894118), Col.ByteFromDoubleClamped(0.709804));
        public static C3b NavajoWhite => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.870588), Col.ByteFromDoubleClamped(0.678431));
        public static C3b Navy => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961));
        public static C3b OldLace => new C3b(Col.ByteFromDoubleClamped(0.992157), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.901961));
        public static C3b Olive => new C3b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0));
        public static C3b OliveDrab => new C3b(Col.ByteFromDoubleClamped(0.419608), Col.ByteFromDoubleClamped(0.556863), Col.ByteFromDoubleClamped(0.137255));
        public static C3b Orange => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.647059), Col.ByteFromDoubleClamped(0));
        public static C3b OrangeRed => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.270588), Col.ByteFromDoubleClamped(0));
        public static C3b Orchid => new C3b(Col.ByteFromDoubleClamped(0.854902), Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.839216));
        public static C3b PaleGoldenRod => new C3b(Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.909804), Col.ByteFromDoubleClamped(0.666667));
        public static C3b PaleGreen => new C3b(Col.ByteFromDoubleClamped(0.596078), Col.ByteFromDoubleClamped(0.984314), Col.ByteFromDoubleClamped(0.596078));
        public static C3b PaleTurquoise => new C3b(Col.ByteFromDoubleClamped(0.686275), Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.933333));
        public static C3b PaleVioletRed => new C3b(Col.ByteFromDoubleClamped(0.847059), Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.576471));
        public static C3b PapayaWhip => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.937255), Col.ByteFromDoubleClamped(0.835294));
        public static C3b PeachPuff => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.854902), Col.ByteFromDoubleClamped(0.72549));
        public static C3b Peru => new C3b(Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.521569), Col.ByteFromDoubleClamped(0.247059));
        public static C3b Pink => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.752941), Col.ByteFromDoubleClamped(0.796078));
        public static C3b Plum => new C3b(Col.ByteFromDoubleClamped(0.866667), Col.ByteFromDoubleClamped(0.627451), Col.ByteFromDoubleClamped(0.866667));
        public static C3b PowderBlue => new C3b(Col.ByteFromDoubleClamped(0.690196), Col.ByteFromDoubleClamped(0.878431), Col.ByteFromDoubleClamped(0.901961));
        public static C3b Purple => new C3b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961));
        public static C3b Red => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C3b RosyBrown => new C3b(Col.ByteFromDoubleClamped(0.737255), Col.ByteFromDoubleClamped(0.560784), Col.ByteFromDoubleClamped(0.560784));
        public static C3b RoyalBlue => new C3b(Col.ByteFromDoubleClamped(0.254902), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.882353));
        public static C3b SaddleBrown => new C3b(Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.270588), Col.ByteFromDoubleClamped(0.07451));
        public static C3b Salmon => new C3b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.447059));
        public static C3b SandyBrown => new C3b(Col.ByteFromDoubleClamped(0.956863), Col.ByteFromDoubleClamped(0.643137), Col.ByteFromDoubleClamped(0.376471));
        public static C3b SeaGreen => new C3b(Col.ByteFromDoubleClamped(0.180392), Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.341176));
        public static C3b SeaShell => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.933333));
        public static C3b Sienna => new C3b(Col.ByteFromDoubleClamped(0.627451), Col.ByteFromDoubleClamped(0.321569), Col.ByteFromDoubleClamped(0.176471));
        public static C3b Silver => new C3b(Col.ByteFromDoubleClamped(0.752941), Col.ByteFromDoubleClamped(0.752941), Col.ByteFromDoubleClamped(0.752941));
        public static C3b SkyBlue => new C3b(Col.ByteFromDoubleClamped(0.529412), Col.ByteFromDoubleClamped(0.807843), Col.ByteFromDoubleClamped(0.921569));
        public static C3b SlateBlue => new C3b(Col.ByteFromDoubleClamped(0.415686), Col.ByteFromDoubleClamped(0.352941), Col.ByteFromDoubleClamped(0.803922));
        public static C3b SlateGray => new C3b(Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.564706));
        public static C3b SlateGrey => new C3b(Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.564706));
        public static C3b Snow => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.980392));
        public static C3b SpringGreen => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.498039));
        public static C3b SteelBlue => new C3b(Col.ByteFromDoubleClamped(0.27451), Col.ByteFromDoubleClamped(0.509804), Col.ByteFromDoubleClamped(0.705882));
        public static C3b Tan => new C3b(Col.ByteFromDoubleClamped(0.823529), Col.ByteFromDoubleClamped(0.705882), Col.ByteFromDoubleClamped(0.54902));
        public static C3b Teal => new C3b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C3b Thistle => new C3b(Col.ByteFromDoubleClamped(0.847059), Col.ByteFromDoubleClamped(0.74902), Col.ByteFromDoubleClamped(0.847059));
        public static C3b Tomato => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.388235), Col.ByteFromDoubleClamped(0.278431));
        public static C3b Turquoise => new C3b(Col.ByteFromDoubleClamped(0.25098), Col.ByteFromDoubleClamped(0.878431), Col.ByteFromDoubleClamped(0.815686));
        public static C3b Violet => new C3b(Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.509804), Col.ByteFromDoubleClamped(0.933333));
        public static C3b Wheat => new C3b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.870588), Col.ByteFromDoubleClamped(0.701961));
        public static C3b White => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C3b WhiteSmoke => new C3b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.960784));
        public static C3b Yellow => new C3b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0));
        public static C3b YellowGreen => new C3b(Col.ByteFromDoubleClamped(0.603922), Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.196078));

        public static C3b DarkYellow => Olive;

        public static C3b VRVisGreen => new C3b(Col.ByteFromDoubleClamped(0.698), Col.ByteFromDoubleClamped(0.851), Col.ByteFromDoubleClamped(0.008));

        public static C3b Gray10 => new C3b(Col.ByteFromDoubleClamped(0.1));
        public static C3b Gray20 => new C3b(Col.ByteFromDoubleClamped(0.2));
        public static C3b Gray30 => new C3b(Col.ByteFromDoubleClamped(0.3));
        public static C3b Gray40 => new C3b(Col.ByteFromDoubleClamped(0.4));
        public static C3b Gray50 => new C3b(Col.ByteFromDoubleClamped(0.5));
        public static C3b Gray60 => new C3b(Col.ByteFromDoubleClamped(0.6));
        public static C3b Gray70 => new C3b(Col.ByteFromDoubleClamped(0.7));
        public static C3b Gray80 => new C3b(Col.ByteFromDoubleClamped(0.8));
        public static C3b Gray90 => new C3b(Col.ByteFromDoubleClamped(0.9));

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

        public static C3b operator *(C3b col, double scalar)
        {
            return new C3b(
                (byte)(col.R * scalar), 
                (byte)(col.G * scalar), 
                (byte)(col.B * scalar));
        }

        public static C3b operator *(double scalar, C3b col)
        {
            return new C3b(
                (byte)(scalar * col.R), 
                (byte)(scalar * col.G), 
                (byte)(scalar * col.B));
        }

        public static C3b operator /(C3b col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C3b(
                (byte)(col.R * f), 
                (byte)(col.G * f), 
                (byte)(col.B * f));
        }

        public static C3b operator +(C3b c0, C3b c1)
        {
            return new C3b(
                (byte)(c0.R + (c1.R)), 
                (byte)(c0.G + (c1.G)), 
                (byte)(c0.B + (c1.B)));
        }

        public static C3b operator -(C3b c0, C3b c1)
        {
            return new C3b(
                (byte)(c0.R - (c1.R)), 
                (byte)(c0.G - (c1.G)), 
                (byte)(c0.B - (c1.B)));
        }

        public static C3b operator +(C3b c0, C3us c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G + Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B + Col.ByteFromUShort(c1.B)));
        }

        public static C3b operator -(C3b c0, C3us c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G - Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B - Col.ByteFromUShort(c1.B)));
        }

        public static C3b operator +(C3b c0, C3ui c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G + Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B + Col.ByteFromUInt(c1.B)));
        }

        public static C3b operator -(C3b c0, C3ui c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G - Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B - Col.ByteFromUInt(c1.B)));
        }

        public static C3b operator +(C3b c0, C3f c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G + Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B + Col.ByteFromFloat(c1.B)));
        }

        public static C3b operator -(C3b c0, C3f c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G - Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B - Col.ByteFromFloat(c1.B)));
        }

        public static C3b operator +(C3b c0, C3d c1)
        {
            return new C3b(
                (byte)(c0.R + Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G + Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B + Col.ByteFromDouble(c1.B)));
        }

        public static C3b operator -(C3b c0, C3d c1)
        {
            return new C3b(
                (byte)(c0.R - Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G - Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B - Col.ByteFromDouble(c1.B)));
        }

        public static V3i operator + (V3i vec, C3b color)
        {
            return new V3i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B)
                );
        }

        public static V3i operator -(V3i vec, C3b color)
        {
            return new V3i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B)
                );
        }

        public static V3l operator + (V3l vec, C3b color)
        {
            return new V3l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B)
                );
        }

        public static V3l operator -(V3l vec, C3b color)
        {
            return new V3l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B)
                );
        }

        public static V4i operator + (V4i vec, C3b color)
        {
            return new V4i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B),
                vec.W + (int)(255)
                );
        }

        public static V4i operator -(V4i vec, C3b color)
        {
            return new V4i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B),
                vec.W - (int)(255)
                );
        }

        public static V4l operator + (V4l vec, C3b color)
        {
            return new V4l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B),
                vec.W + (long)(255)
                );
        }

        public static V4l operator -(V4l vec, C3b color)
        {
            return new V4l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B),
                vec.W - (long)(255)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(byte min, byte max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3b Clamped(byte min, byte max)
        {
            return new C3b(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp(Col.ByteFromDoubleClamped(min), Col.ByteFromDoubleClamped(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3b Clamped(double min, double max)
        {
            return Clamped(Col.ByteFromDoubleClamped(min), Col.ByteFromDoubleClamped(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public int Norm1
        {
            get { return (int)R + (int)G + (int)B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public byte NormMax
        {
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public byte NormMin
        {
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
        public static bool ApproximateEquals(this C3b a, C3b b)
        {
            return ApproximateEquals(a, b, Constant<byte>.PositiveTinyValue);
        }

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

    [Serializable]
    public partial struct C3us : IFormattable, IEquatable<C3us>, IRGB
    {
        #region Constructors

        public C3us(ushort r, ushort g, ushort b)
        {
            R = r; G = g; B = b;
        }

        public C3us(int r, int g, int b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
        }

        public C3us(long r, long g, long b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
        }

        public C3us(double r, double g, double b)
        {
            R = Col.UShortFromDoubleClamped(r);
            G = Col.UShortFromDoubleClamped(g);
            B = Col.UShortFromDoubleClamped(b);
        }

        public C3us(ushort gray)
        {
            R = gray; G = gray; B = gray;
        }

        public C3us(double gray)
        {
            var value = Col.UShortFromDoubleClamped(gray);
            R = value; G = value; B = value;
        }

        public C3us(C3b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
        }

        public C3us(C3us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3us(C3ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
        }

        public C3us(C3f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
        }

        public C3us(C3d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
        }

        public C3us(C4b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
        }

        public C3us(C4us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3us(C4ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
        }

        public C3us(C4f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
        }

        public C3us(C4d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
        }

        public C3us(V3i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        public C3us(V3l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        public C3us(V4i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        public C3us(V4l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C3us color)
        {
            return new C3b(color);
        }

        public static explicit operator C3ui(C3us color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C3us color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C3us color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C3us color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C3us color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C3us color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C3us color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C3us color)
        {
            return new C4d(color);
        }

        public static explicit operator V3i(C3us color)
        {
            return new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );
        }

        public static explicit operator V3l(C3us color)
        {
            return new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );
        }

        public static explicit operator V4i(C3us color)
        {
            return new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(65535)
                );
        }

        public static explicit operator V4l(C3us color)
        {
            return new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(65535)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C3us(Func<int, ushort> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        public V3i ToV3i() { return (V3i)this; }
        public V3l ToV3l() { return (V3l)this; }
        public V4i ToV4i() { return (V4i)this; }
        public V4l ToV4l() { return (V4l)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3b(C3b c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3ui(C3ui c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3f(C3f c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC3d(C3d c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4b(C4b c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4us(C4us c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4ui(C4ui c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4f(C4f c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromC4d(C4d c)
            => new C3us(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV3i(V3i c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV3l(V3l c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV4i(V4i c)
            => new C3us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3us FromV4l(V4l c)
            => new C3us(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3b Copy(Func<ushort, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3us Copy(Func<ushort, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3ui Copy(Func<ushort, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3f Copy(Func<ushort, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3d Copy(Func<ushort, double> channel_fun)
        {
            return Map(channel_fun);
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
        public ushort this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C3us with all components zero.
        /// </summary>
        public static C3us Zero => new C3us(0, 0, 0);

        // Web colors
        public static C3us AliceBlue => new C3us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(1));
        public static C3us AntiqueWhite => new C3us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.921569), Col.UShortFromDoubleClamped(0.843137));
        public static C3us Aqua => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C3us Aquamarine => new C3us(Col.UShortFromDoubleClamped(0.498039), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.831373));
        public static C3us Azure => new C3us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C3us Beige => new C3us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.862745));
        public static C3us Bisque => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.894118), Col.UShortFromDoubleClamped(0.768627));
        public static C3us Black => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C3us BlanchedAlmond => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.921569), Col.UShortFromDoubleClamped(0.803922));
        public static C3us Blue => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1));
        public static C3us BlueViolet => new C3us(Col.UShortFromDoubleClamped(0.541176), Col.UShortFromDoubleClamped(0.168627), Col.UShortFromDoubleClamped(0.886275));
        public static C3us Brown => new C3us(Col.UShortFromDoubleClamped(0.647059), Col.UShortFromDoubleClamped(0.164706), Col.UShortFromDoubleClamped(0.164706));
        public static C3us BurlyWood => new C3us(Col.UShortFromDoubleClamped(0.870588), Col.UShortFromDoubleClamped(0.721569), Col.UShortFromDoubleClamped(0.529412));
        public static C3us CadetBlue => new C3us(Col.UShortFromDoubleClamped(0.372549), Col.UShortFromDoubleClamped(0.619608), Col.UShortFromDoubleClamped(0.627451));
        public static C3us Chartreuse => new C3us(Col.UShortFromDoubleClamped(0.498039), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0));
        public static C3us Chocolate => new C3us(Col.UShortFromDoubleClamped(0.823529), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.117647));
        public static C3us Coral => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.498039), Col.UShortFromDoubleClamped(0.313725));
        public static C3us CornflowerBlue => new C3us(Col.UShortFromDoubleClamped(0.392157), Col.UShortFromDoubleClamped(0.584314), Col.UShortFromDoubleClamped(0.929412));
        public static C3us Cornsilk => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(0.862745));
        public static C3us Crimson => new C3us(Col.UShortFromDoubleClamped(0.862745), Col.UShortFromDoubleClamped(0.078431), Col.UShortFromDoubleClamped(0.235294));
        public static C3us Cyan => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C3us DarkBlue => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.545098));
        public static C3us DarkCyan => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.545098));
        public static C3us DarkGoldenRod => new C3us(Col.UShortFromDoubleClamped(0.721569), Col.UShortFromDoubleClamped(0.52549), Col.UShortFromDoubleClamped(0.043137));
        public static C3us DarkGray => new C3us(Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745));
        public static C3us DarkGrey => new C3us(Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745));
        public static C3us DarkGreen => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.392157), Col.UShortFromDoubleClamped(0));
        public static C3us DarkKhaki => new C3us(Col.UShortFromDoubleClamped(0.741176), Col.UShortFromDoubleClamped(0.717647), Col.UShortFromDoubleClamped(0.419608));
        public static C3us DarkMagenta => new C3us(Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.545098));
        public static C3us DarkOliveGreen => new C3us(Col.UShortFromDoubleClamped(0.333333), Col.UShortFromDoubleClamped(0.419608), Col.UShortFromDoubleClamped(0.184314));
        public static C3us DarkOrange => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.54902), Col.UShortFromDoubleClamped(0));
        public static C3us DarkOrchid => new C3us(Col.UShortFromDoubleClamped(0.6), Col.UShortFromDoubleClamped(0.196078), Col.UShortFromDoubleClamped(0.8));
        public static C3us DarkRed => new C3us(Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C3us DarkSalmon => new C3us(Col.UShortFromDoubleClamped(0.913725), Col.UShortFromDoubleClamped(0.588235), Col.UShortFromDoubleClamped(0.478431));
        public static C3us DarkSeaGreen => new C3us(Col.UShortFromDoubleClamped(0.560784), Col.UShortFromDoubleClamped(0.737255), Col.UShortFromDoubleClamped(0.560784));
        public static C3us DarkSlateBlue => new C3us(Col.UShortFromDoubleClamped(0.282353), Col.UShortFromDoubleClamped(0.239216), Col.UShortFromDoubleClamped(0.545098));
        public static C3us DarkSlateGray => new C3us(Col.UShortFromDoubleClamped(0.184314), Col.UShortFromDoubleClamped(0.309804), Col.UShortFromDoubleClamped(0.309804));
        public static C3us DarkSlateGrey => new C3us(Col.UShortFromDoubleClamped(0.184314), Col.UShortFromDoubleClamped(0.309804), Col.UShortFromDoubleClamped(0.309804));
        public static C3us DarkTurquoise => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.807843), Col.UShortFromDoubleClamped(0.819608));
        public static C3us DarkViolet => new C3us(Col.UShortFromDoubleClamped(0.580392), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.827451));
        public static C3us DeepPink => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.078431), Col.UShortFromDoubleClamped(0.576471));
        public static C3us DeepSkyBlue => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.74902), Col.UShortFromDoubleClamped(1));
        public static C3us DimGray => new C3us(Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765));
        public static C3us DimGrey => new C3us(Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765));
        public static C3us DodgerBlue => new C3us(Col.UShortFromDoubleClamped(0.117647), Col.UShortFromDoubleClamped(0.564706), Col.UShortFromDoubleClamped(1));
        public static C3us FireBrick => new C3us(Col.UShortFromDoubleClamped(0.698039), Col.UShortFromDoubleClamped(0.133333), Col.UShortFromDoubleClamped(0.133333));
        public static C3us FloralWhite => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.941176));
        public static C3us ForestGreen => new C3us(Col.UShortFromDoubleClamped(0.133333), Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.133333));
        public static C3us Fuchsia => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1));
        public static C3us Gainsboro => new C3us(Col.UShortFromDoubleClamped(0.862745), Col.UShortFromDoubleClamped(0.862745), Col.UShortFromDoubleClamped(0.862745));
        public static C3us GhostWhite => new C3us(Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(1));
        public static C3us Gold => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.843137), Col.UShortFromDoubleClamped(0));
        public static C3us GoldenRod => new C3us(Col.UShortFromDoubleClamped(0.854902), Col.UShortFromDoubleClamped(0.647059), Col.UShortFromDoubleClamped(0.12549));
        public static C3us Gray => new C3us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C3us Grey => new C3us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C3us Green => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0));
        public static C3us GreenYellow => new C3us(Col.UShortFromDoubleClamped(0.678431), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.184314));
        public static C3us HoneyDew => new C3us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.941176));
        public static C3us HotPink => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.705882));
        public static C3us IndianRed  => new C3us(Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.360784), Col.UShortFromDoubleClamped(0.360784));
        public static C3us Indigo  => new C3us(Col.UShortFromDoubleClamped(0.294118), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.509804));
        public static C3us Ivory => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.941176));
        public static C3us Khaki => new C3us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.901961), Col.UShortFromDoubleClamped(0.54902));
        public static C3us Lavender => new C3us(Col.UShortFromDoubleClamped(0.901961), Col.UShortFromDoubleClamped(0.901961), Col.UShortFromDoubleClamped(0.980392));
        public static C3us LavenderBlush => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.960784));
        public static C3us LawnGreen => new C3us(Col.UShortFromDoubleClamped(0.486275), Col.UShortFromDoubleClamped(0.988235), Col.UShortFromDoubleClamped(0));
        public static C3us LemonChiffon => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.803922));
        public static C3us LightBlue => new C3us(Col.UShortFromDoubleClamped(0.678431), Col.UShortFromDoubleClamped(0.847059), Col.UShortFromDoubleClamped(0.901961));
        public static C3us LightCoral => new C3us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C3us LightCyan => new C3us(Col.UShortFromDoubleClamped(0.878431), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C3us LightGoldenRodYellow => new C3us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.823529));
        public static C3us LightGray => new C3us(Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451));
        public static C3us LightGrey => new C3us(Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451));
        public static C3us LightGreen => new C3us(Col.UShortFromDoubleClamped(0.564706), Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.564706));
        public static C3us LightPink => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.713725), Col.UShortFromDoubleClamped(0.756863));
        public static C3us LightSalmon => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.627451), Col.UShortFromDoubleClamped(0.478431));
        public static C3us LightSeaGreen => new C3us(Col.UShortFromDoubleClamped(0.12549), Col.UShortFromDoubleClamped(0.698039), Col.UShortFromDoubleClamped(0.666667));
        public static C3us LightSkyBlue => new C3us(Col.UShortFromDoubleClamped(0.529412), Col.UShortFromDoubleClamped(0.807843), Col.UShortFromDoubleClamped(0.980392));
        public static C3us LightSlateGray => new C3us(Col.UShortFromDoubleClamped(0.466667), Col.UShortFromDoubleClamped(0.533333), Col.UShortFromDoubleClamped(0.6));
        public static C3us LightSlateGrey => new C3us(Col.UShortFromDoubleClamped(0.466667), Col.UShortFromDoubleClamped(0.533333), Col.UShortFromDoubleClamped(0.6));
        public static C3us LightSteelBlue => new C3us(Col.UShortFromDoubleClamped(0.690196), Col.UShortFromDoubleClamped(0.768627), Col.UShortFromDoubleClamped(0.870588));
        public static C3us LightYellow => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.878431));
        public static C3us Lime => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0));
        public static C3us LimeGreen => new C3us(Col.UShortFromDoubleClamped(0.196078), Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.196078));
        public static C3us Linen => new C3us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.901961));
        public static C3us Magenta => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1));
        public static C3us Maroon => new C3us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C3us MediumAquaMarine => new C3us(Col.UShortFromDoubleClamped(0.4), Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.666667));
        public static C3us MediumBlue => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.803922));
        public static C3us MediumOrchid => new C3us(Col.UShortFromDoubleClamped(0.729412), Col.UShortFromDoubleClamped(0.333333), Col.UShortFromDoubleClamped(0.827451));
        public static C3us MediumPurple => new C3us(Col.UShortFromDoubleClamped(0.576471), Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.847059));
        public static C3us MediumSeaGreen => new C3us(Col.UShortFromDoubleClamped(0.235294), Col.UShortFromDoubleClamped(0.701961), Col.UShortFromDoubleClamped(0.443137));
        public static C3us MediumSlateBlue => new C3us(Col.UShortFromDoubleClamped(0.482353), Col.UShortFromDoubleClamped(0.407843), Col.UShortFromDoubleClamped(0.933333));
        public static C3us MediumSpringGreen => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.603922));
        public static C3us MediumTurquoise => new C3us(Col.UShortFromDoubleClamped(0.282353), Col.UShortFromDoubleClamped(0.819608), Col.UShortFromDoubleClamped(0.8));
        public static C3us MediumVioletRed => new C3us(Col.UShortFromDoubleClamped(0.780392), Col.UShortFromDoubleClamped(0.082353), Col.UShortFromDoubleClamped(0.521569));
        public static C3us MidnightBlue => new C3us(Col.UShortFromDoubleClamped(0.098039), Col.UShortFromDoubleClamped(0.098039), Col.UShortFromDoubleClamped(0.439216));
        public static C3us MintCream => new C3us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392));
        public static C3us MistyRose => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.894118), Col.UShortFromDoubleClamped(0.882353));
        public static C3us Moccasin => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.894118), Col.UShortFromDoubleClamped(0.709804));
        public static C3us NavajoWhite => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.870588), Col.UShortFromDoubleClamped(0.678431));
        public static C3us Navy => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961));
        public static C3us OldLace => new C3us(Col.UShortFromDoubleClamped(0.992157), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.901961));
        public static C3us Olive => new C3us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0));
        public static C3us OliveDrab => new C3us(Col.UShortFromDoubleClamped(0.419608), Col.UShortFromDoubleClamped(0.556863), Col.UShortFromDoubleClamped(0.137255));
        public static C3us Orange => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.647059), Col.UShortFromDoubleClamped(0));
        public static C3us OrangeRed => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.270588), Col.UShortFromDoubleClamped(0));
        public static C3us Orchid => new C3us(Col.UShortFromDoubleClamped(0.854902), Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.839216));
        public static C3us PaleGoldenRod => new C3us(Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.909804), Col.UShortFromDoubleClamped(0.666667));
        public static C3us PaleGreen => new C3us(Col.UShortFromDoubleClamped(0.596078), Col.UShortFromDoubleClamped(0.984314), Col.UShortFromDoubleClamped(0.596078));
        public static C3us PaleTurquoise => new C3us(Col.UShortFromDoubleClamped(0.686275), Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.933333));
        public static C3us PaleVioletRed => new C3us(Col.UShortFromDoubleClamped(0.847059), Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.576471));
        public static C3us PapayaWhip => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.937255), Col.UShortFromDoubleClamped(0.835294));
        public static C3us PeachPuff => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.854902), Col.UShortFromDoubleClamped(0.72549));
        public static C3us Peru => new C3us(Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.521569), Col.UShortFromDoubleClamped(0.247059));
        public static C3us Pink => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.752941), Col.UShortFromDoubleClamped(0.796078));
        public static C3us Plum => new C3us(Col.UShortFromDoubleClamped(0.866667), Col.UShortFromDoubleClamped(0.627451), Col.UShortFromDoubleClamped(0.866667));
        public static C3us PowderBlue => new C3us(Col.UShortFromDoubleClamped(0.690196), Col.UShortFromDoubleClamped(0.878431), Col.UShortFromDoubleClamped(0.901961));
        public static C3us Purple => new C3us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961));
        public static C3us Red => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C3us RosyBrown => new C3us(Col.UShortFromDoubleClamped(0.737255), Col.UShortFromDoubleClamped(0.560784), Col.UShortFromDoubleClamped(0.560784));
        public static C3us RoyalBlue => new C3us(Col.UShortFromDoubleClamped(0.254902), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.882353));
        public static C3us SaddleBrown => new C3us(Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.270588), Col.UShortFromDoubleClamped(0.07451));
        public static C3us Salmon => new C3us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.447059));
        public static C3us SandyBrown => new C3us(Col.UShortFromDoubleClamped(0.956863), Col.UShortFromDoubleClamped(0.643137), Col.UShortFromDoubleClamped(0.376471));
        public static C3us SeaGreen => new C3us(Col.UShortFromDoubleClamped(0.180392), Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.341176));
        public static C3us SeaShell => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.933333));
        public static C3us Sienna => new C3us(Col.UShortFromDoubleClamped(0.627451), Col.UShortFromDoubleClamped(0.321569), Col.UShortFromDoubleClamped(0.176471));
        public static C3us Silver => new C3us(Col.UShortFromDoubleClamped(0.752941), Col.UShortFromDoubleClamped(0.752941), Col.UShortFromDoubleClamped(0.752941));
        public static C3us SkyBlue => new C3us(Col.UShortFromDoubleClamped(0.529412), Col.UShortFromDoubleClamped(0.807843), Col.UShortFromDoubleClamped(0.921569));
        public static C3us SlateBlue => new C3us(Col.UShortFromDoubleClamped(0.415686), Col.UShortFromDoubleClamped(0.352941), Col.UShortFromDoubleClamped(0.803922));
        public static C3us SlateGray => new C3us(Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.564706));
        public static C3us SlateGrey => new C3us(Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.564706));
        public static C3us Snow => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.980392));
        public static C3us SpringGreen => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.498039));
        public static C3us SteelBlue => new C3us(Col.UShortFromDoubleClamped(0.27451), Col.UShortFromDoubleClamped(0.509804), Col.UShortFromDoubleClamped(0.705882));
        public static C3us Tan => new C3us(Col.UShortFromDoubleClamped(0.823529), Col.UShortFromDoubleClamped(0.705882), Col.UShortFromDoubleClamped(0.54902));
        public static C3us Teal => new C3us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C3us Thistle => new C3us(Col.UShortFromDoubleClamped(0.847059), Col.UShortFromDoubleClamped(0.74902), Col.UShortFromDoubleClamped(0.847059));
        public static C3us Tomato => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.388235), Col.UShortFromDoubleClamped(0.278431));
        public static C3us Turquoise => new C3us(Col.UShortFromDoubleClamped(0.25098), Col.UShortFromDoubleClamped(0.878431), Col.UShortFromDoubleClamped(0.815686));
        public static C3us Violet => new C3us(Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.509804), Col.UShortFromDoubleClamped(0.933333));
        public static C3us Wheat => new C3us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.870588), Col.UShortFromDoubleClamped(0.701961));
        public static C3us White => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C3us WhiteSmoke => new C3us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.960784));
        public static C3us Yellow => new C3us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0));
        public static C3us YellowGreen => new C3us(Col.UShortFromDoubleClamped(0.603922), Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.196078));

        public static C3us DarkYellow => Olive;

        public static C3us VRVisGreen => new C3us(Col.UShortFromDoubleClamped(0.698), Col.UShortFromDoubleClamped(0.851), Col.UShortFromDoubleClamped(0.008));

        public static C3us Gray10 => new C3us(Col.UShortFromDoubleClamped(0.1));
        public static C3us Gray20 => new C3us(Col.UShortFromDoubleClamped(0.2));
        public static C3us Gray30 => new C3us(Col.UShortFromDoubleClamped(0.3));
        public static C3us Gray40 => new C3us(Col.UShortFromDoubleClamped(0.4));
        public static C3us Gray50 => new C3us(Col.UShortFromDoubleClamped(0.5));
        public static C3us Gray60 => new C3us(Col.UShortFromDoubleClamped(0.6));
        public static C3us Gray70 => new C3us(Col.UShortFromDoubleClamped(0.7));
        public static C3us Gray80 => new C3us(Col.UShortFromDoubleClamped(0.8));
        public static C3us Gray90 => new C3us(Col.UShortFromDoubleClamped(0.9));

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

        public static C3us operator *(C3us col, double scalar)
        {
            return new C3us(
                (ushort)(col.R * scalar), 
                (ushort)(col.G * scalar), 
                (ushort)(col.B * scalar));
        }

        public static C3us operator *(double scalar, C3us col)
        {
            return new C3us(
                (ushort)(scalar * col.R), 
                (ushort)(scalar * col.G), 
                (ushort)(scalar * col.B));
        }

        public static C3us operator /(C3us col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C3us(
                (ushort)(col.R * f), 
                (ushort)(col.G * f), 
                (ushort)(col.B * f));
        }

        public static C3us operator +(C3us c0, C3b c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G + Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B + Col.UShortFromByte(c1.B)));
        }

        public static C3us operator -(C3us c0, C3b c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G - Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B - Col.UShortFromByte(c1.B)));
        }

        public static C3us operator +(C3us c0, C3us c1)
        {
            return new C3us(
                (ushort)(c0.R + (c1.R)), 
                (ushort)(c0.G + (c1.G)), 
                (ushort)(c0.B + (c1.B)));
        }

        public static C3us operator -(C3us c0, C3us c1)
        {
            return new C3us(
                (ushort)(c0.R - (c1.R)), 
                (ushort)(c0.G - (c1.G)), 
                (ushort)(c0.B - (c1.B)));
        }

        public static C3us operator +(C3us c0, C3ui c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G + Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B + Col.UShortFromUInt(c1.B)));
        }

        public static C3us operator -(C3us c0, C3ui c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G - Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B - Col.UShortFromUInt(c1.B)));
        }

        public static C3us operator +(C3us c0, C3f c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G + Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B + Col.UShortFromFloat(c1.B)));
        }

        public static C3us operator -(C3us c0, C3f c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G - Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B - Col.UShortFromFloat(c1.B)));
        }

        public static C3us operator +(C3us c0, C3d c1)
        {
            return new C3us(
                (ushort)(c0.R + Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G + Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B + Col.UShortFromDouble(c1.B)));
        }

        public static C3us operator -(C3us c0, C3d c1)
        {
            return new C3us(
                (ushort)(c0.R - Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G - Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B - Col.UShortFromDouble(c1.B)));
        }

        public static V3i operator + (V3i vec, C3us color)
        {
            return new V3i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B)
                );
        }

        public static V3i operator -(V3i vec, C3us color)
        {
            return new V3i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B)
                );
        }

        public static V3l operator + (V3l vec, C3us color)
        {
            return new V3l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B)
                );
        }

        public static V3l operator -(V3l vec, C3us color)
        {
            return new V3l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B)
                );
        }

        public static V4i operator + (V4i vec, C3us color)
        {
            return new V4i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B),
                vec.W + (int)(65535)
                );
        }

        public static V4i operator -(V4i vec, C3us color)
        {
            return new V4i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B),
                vec.W - (int)(65535)
                );
        }

        public static V4l operator + (V4l vec, C3us color)
        {
            return new V4l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B),
                vec.W + (long)(65535)
                );
        }

        public static V4l operator -(V4l vec, C3us color)
        {
            return new V4l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B),
                vec.W - (long)(65535)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(ushort min, ushort max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3us Clamped(ushort min, ushort max)
        {
            return new C3us(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp(Col.UShortFromDoubleClamped(min), Col.UShortFromDoubleClamped(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3us Clamped(double min, double max)
        {
            return Clamped(Col.UShortFromDoubleClamped(min), Col.UShortFromDoubleClamped(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public int Norm1
        {
            get { return (int)R + (int)G + (int)B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public ushort NormMax
        {
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public ushort NormMin
        {
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
        public static bool ApproximateEquals(this C3us a, C3us b)
        {
            return ApproximateEquals(a, b, Constant<ushort>.PositiveTinyValue);
        }

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

    [Serializable]
    public partial struct C3ui : IFormattable, IEquatable<C3ui>, IRGB
    {
        #region Constructors

        public C3ui(uint r, uint g, uint b)
        {
            R = r; G = g; B = b;
        }

        public C3ui(int r, int g, int b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
        }

        public C3ui(long r, long g, long b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
        }

        public C3ui(double r, double g, double b)
        {
            R = Col.UIntFromDoubleClamped(r);
            G = Col.UIntFromDoubleClamped(g);
            B = Col.UIntFromDoubleClamped(b);
        }

        public C3ui(uint gray)
        {
            R = gray; G = gray; B = gray;
        }

        public C3ui(double gray)
        {
            var value = Col.UIntFromDoubleClamped(gray);
            R = value; G = value; B = value;
        }

        public C3ui(C3b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
        }

        public C3ui(C3us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
        }

        public C3ui(C3ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3ui(C3f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
        }

        public C3ui(C3d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
        }

        public C3ui(C4b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
        }

        public C3ui(C4us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
        }

        public C3ui(C4ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3ui(C4f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
        }

        public C3ui(C4d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
        }

        public C3ui(V3l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        public C3ui(V4l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C3ui color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C3ui color)
        {
            return new C3us(color);
        }

        public static explicit operator C3f(C3ui color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C3ui color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C3ui color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C3ui color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C3ui color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C3ui color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C3ui color)
        {
            return new C4d(color);
        }

        public static explicit operator V3l(C3ui color)
        {
            return new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );
        }

        public static explicit operator V4l(C3ui color)
        {
            return new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(UInt32.MaxValue)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C3ui(Func<int, uint> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        public V3l ToV3l() { return (V3l)this; }
        public V4l ToV4l() { return (V4l)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3b(C3b c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3us(C3us c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3f(C3f c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC3d(C3d c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4b(C4b c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4us(C4us c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4ui(C4ui c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4f(C4f c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromC4d(C4d c)
            => new C3ui(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV3l(V3l c)
            => new C3ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3ui FromV4l(V4l c)
            => new C3ui(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3b Copy(Func<uint, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3us Copy(Func<uint, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3ui Copy(Func<uint, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3f Copy(Func<uint, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3d Copy(Func<uint, double> channel_fun)
        {
            return Map(channel_fun);
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
        public uint this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C3ui with all components zero.
        /// </summary>
        public static C3ui Zero => new C3ui(0, 0, 0);

        // Web colors
        public static C3ui AliceBlue => new C3ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(1));
        public static C3ui AntiqueWhite => new C3ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.921569), Col.UIntFromDoubleClamped(0.843137));
        public static C3ui Aqua => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C3ui Aquamarine => new C3ui(Col.UIntFromDoubleClamped(0.498039), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.831373));
        public static C3ui Azure => new C3ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C3ui Beige => new C3ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.862745));
        public static C3ui Bisque => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.894118), Col.UIntFromDoubleClamped(0.768627));
        public static C3ui Black => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C3ui BlanchedAlmond => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.921569), Col.UIntFromDoubleClamped(0.803922));
        public static C3ui Blue => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1));
        public static C3ui BlueViolet => new C3ui(Col.UIntFromDoubleClamped(0.541176), Col.UIntFromDoubleClamped(0.168627), Col.UIntFromDoubleClamped(0.886275));
        public static C3ui Brown => new C3ui(Col.UIntFromDoubleClamped(0.647059), Col.UIntFromDoubleClamped(0.164706), Col.UIntFromDoubleClamped(0.164706));
        public static C3ui BurlyWood => new C3ui(Col.UIntFromDoubleClamped(0.870588), Col.UIntFromDoubleClamped(0.721569), Col.UIntFromDoubleClamped(0.529412));
        public static C3ui CadetBlue => new C3ui(Col.UIntFromDoubleClamped(0.372549), Col.UIntFromDoubleClamped(0.619608), Col.UIntFromDoubleClamped(0.627451));
        public static C3ui Chartreuse => new C3ui(Col.UIntFromDoubleClamped(0.498039), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0));
        public static C3ui Chocolate => new C3ui(Col.UIntFromDoubleClamped(0.823529), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.117647));
        public static C3ui Coral => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.498039), Col.UIntFromDoubleClamped(0.313725));
        public static C3ui CornflowerBlue => new C3ui(Col.UIntFromDoubleClamped(0.392157), Col.UIntFromDoubleClamped(0.584314), Col.UIntFromDoubleClamped(0.929412));
        public static C3ui Cornsilk => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(0.862745));
        public static C3ui Crimson => new C3ui(Col.UIntFromDoubleClamped(0.862745), Col.UIntFromDoubleClamped(0.078431), Col.UIntFromDoubleClamped(0.235294));
        public static C3ui Cyan => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C3ui DarkBlue => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.545098));
        public static C3ui DarkCyan => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.545098));
        public static C3ui DarkGoldenRod => new C3ui(Col.UIntFromDoubleClamped(0.721569), Col.UIntFromDoubleClamped(0.52549), Col.UIntFromDoubleClamped(0.043137));
        public static C3ui DarkGray => new C3ui(Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745));
        public static C3ui DarkGrey => new C3ui(Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745));
        public static C3ui DarkGreen => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.392157), Col.UIntFromDoubleClamped(0));
        public static C3ui DarkKhaki => new C3ui(Col.UIntFromDoubleClamped(0.741176), Col.UIntFromDoubleClamped(0.717647), Col.UIntFromDoubleClamped(0.419608));
        public static C3ui DarkMagenta => new C3ui(Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.545098));
        public static C3ui DarkOliveGreen => new C3ui(Col.UIntFromDoubleClamped(0.333333), Col.UIntFromDoubleClamped(0.419608), Col.UIntFromDoubleClamped(0.184314));
        public static C3ui DarkOrange => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.54902), Col.UIntFromDoubleClamped(0));
        public static C3ui DarkOrchid => new C3ui(Col.UIntFromDoubleClamped(0.6), Col.UIntFromDoubleClamped(0.196078), Col.UIntFromDoubleClamped(0.8));
        public static C3ui DarkRed => new C3ui(Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C3ui DarkSalmon => new C3ui(Col.UIntFromDoubleClamped(0.913725), Col.UIntFromDoubleClamped(0.588235), Col.UIntFromDoubleClamped(0.478431));
        public static C3ui DarkSeaGreen => new C3ui(Col.UIntFromDoubleClamped(0.560784), Col.UIntFromDoubleClamped(0.737255), Col.UIntFromDoubleClamped(0.560784));
        public static C3ui DarkSlateBlue => new C3ui(Col.UIntFromDoubleClamped(0.282353), Col.UIntFromDoubleClamped(0.239216), Col.UIntFromDoubleClamped(0.545098));
        public static C3ui DarkSlateGray => new C3ui(Col.UIntFromDoubleClamped(0.184314), Col.UIntFromDoubleClamped(0.309804), Col.UIntFromDoubleClamped(0.309804));
        public static C3ui DarkSlateGrey => new C3ui(Col.UIntFromDoubleClamped(0.184314), Col.UIntFromDoubleClamped(0.309804), Col.UIntFromDoubleClamped(0.309804));
        public static C3ui DarkTurquoise => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.807843), Col.UIntFromDoubleClamped(0.819608));
        public static C3ui DarkViolet => new C3ui(Col.UIntFromDoubleClamped(0.580392), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.827451));
        public static C3ui DeepPink => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.078431), Col.UIntFromDoubleClamped(0.576471));
        public static C3ui DeepSkyBlue => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.74902), Col.UIntFromDoubleClamped(1));
        public static C3ui DimGray => new C3ui(Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765));
        public static C3ui DimGrey => new C3ui(Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765));
        public static C3ui DodgerBlue => new C3ui(Col.UIntFromDoubleClamped(0.117647), Col.UIntFromDoubleClamped(0.564706), Col.UIntFromDoubleClamped(1));
        public static C3ui FireBrick => new C3ui(Col.UIntFromDoubleClamped(0.698039), Col.UIntFromDoubleClamped(0.133333), Col.UIntFromDoubleClamped(0.133333));
        public static C3ui FloralWhite => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.941176));
        public static C3ui ForestGreen => new C3ui(Col.UIntFromDoubleClamped(0.133333), Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.133333));
        public static C3ui Fuchsia => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1));
        public static C3ui Gainsboro => new C3ui(Col.UIntFromDoubleClamped(0.862745), Col.UIntFromDoubleClamped(0.862745), Col.UIntFromDoubleClamped(0.862745));
        public static C3ui GhostWhite => new C3ui(Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(1));
        public static C3ui Gold => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.843137), Col.UIntFromDoubleClamped(0));
        public static C3ui GoldenRod => new C3ui(Col.UIntFromDoubleClamped(0.854902), Col.UIntFromDoubleClamped(0.647059), Col.UIntFromDoubleClamped(0.12549));
        public static C3ui Gray => new C3ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C3ui Grey => new C3ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C3ui Green => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0));
        public static C3ui GreenYellow => new C3ui(Col.UIntFromDoubleClamped(0.678431), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.184314));
        public static C3ui HoneyDew => new C3ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.941176));
        public static C3ui HotPink => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.705882));
        public static C3ui IndianRed  => new C3ui(Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.360784), Col.UIntFromDoubleClamped(0.360784));
        public static C3ui Indigo  => new C3ui(Col.UIntFromDoubleClamped(0.294118), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.509804));
        public static C3ui Ivory => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.941176));
        public static C3ui Khaki => new C3ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.901961), Col.UIntFromDoubleClamped(0.54902));
        public static C3ui Lavender => new C3ui(Col.UIntFromDoubleClamped(0.901961), Col.UIntFromDoubleClamped(0.901961), Col.UIntFromDoubleClamped(0.980392));
        public static C3ui LavenderBlush => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.960784));
        public static C3ui LawnGreen => new C3ui(Col.UIntFromDoubleClamped(0.486275), Col.UIntFromDoubleClamped(0.988235), Col.UIntFromDoubleClamped(0));
        public static C3ui LemonChiffon => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.803922));
        public static C3ui LightBlue => new C3ui(Col.UIntFromDoubleClamped(0.678431), Col.UIntFromDoubleClamped(0.847059), Col.UIntFromDoubleClamped(0.901961));
        public static C3ui LightCoral => new C3ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C3ui LightCyan => new C3ui(Col.UIntFromDoubleClamped(0.878431), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C3ui LightGoldenRodYellow => new C3ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.823529));
        public static C3ui LightGray => new C3ui(Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451));
        public static C3ui LightGrey => new C3ui(Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451));
        public static C3ui LightGreen => new C3ui(Col.UIntFromDoubleClamped(0.564706), Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.564706));
        public static C3ui LightPink => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.713725), Col.UIntFromDoubleClamped(0.756863));
        public static C3ui LightSalmon => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.627451), Col.UIntFromDoubleClamped(0.478431));
        public static C3ui LightSeaGreen => new C3ui(Col.UIntFromDoubleClamped(0.12549), Col.UIntFromDoubleClamped(0.698039), Col.UIntFromDoubleClamped(0.666667));
        public static C3ui LightSkyBlue => new C3ui(Col.UIntFromDoubleClamped(0.529412), Col.UIntFromDoubleClamped(0.807843), Col.UIntFromDoubleClamped(0.980392));
        public static C3ui LightSlateGray => new C3ui(Col.UIntFromDoubleClamped(0.466667), Col.UIntFromDoubleClamped(0.533333), Col.UIntFromDoubleClamped(0.6));
        public static C3ui LightSlateGrey => new C3ui(Col.UIntFromDoubleClamped(0.466667), Col.UIntFromDoubleClamped(0.533333), Col.UIntFromDoubleClamped(0.6));
        public static C3ui LightSteelBlue => new C3ui(Col.UIntFromDoubleClamped(0.690196), Col.UIntFromDoubleClamped(0.768627), Col.UIntFromDoubleClamped(0.870588));
        public static C3ui LightYellow => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.878431));
        public static C3ui Lime => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0));
        public static C3ui LimeGreen => new C3ui(Col.UIntFromDoubleClamped(0.196078), Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.196078));
        public static C3ui Linen => new C3ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.901961));
        public static C3ui Magenta => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1));
        public static C3ui Maroon => new C3ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C3ui MediumAquaMarine => new C3ui(Col.UIntFromDoubleClamped(0.4), Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.666667));
        public static C3ui MediumBlue => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.803922));
        public static C3ui MediumOrchid => new C3ui(Col.UIntFromDoubleClamped(0.729412), Col.UIntFromDoubleClamped(0.333333), Col.UIntFromDoubleClamped(0.827451));
        public static C3ui MediumPurple => new C3ui(Col.UIntFromDoubleClamped(0.576471), Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.847059));
        public static C3ui MediumSeaGreen => new C3ui(Col.UIntFromDoubleClamped(0.235294), Col.UIntFromDoubleClamped(0.701961), Col.UIntFromDoubleClamped(0.443137));
        public static C3ui MediumSlateBlue => new C3ui(Col.UIntFromDoubleClamped(0.482353), Col.UIntFromDoubleClamped(0.407843), Col.UIntFromDoubleClamped(0.933333));
        public static C3ui MediumSpringGreen => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.603922));
        public static C3ui MediumTurquoise => new C3ui(Col.UIntFromDoubleClamped(0.282353), Col.UIntFromDoubleClamped(0.819608), Col.UIntFromDoubleClamped(0.8));
        public static C3ui MediumVioletRed => new C3ui(Col.UIntFromDoubleClamped(0.780392), Col.UIntFromDoubleClamped(0.082353), Col.UIntFromDoubleClamped(0.521569));
        public static C3ui MidnightBlue => new C3ui(Col.UIntFromDoubleClamped(0.098039), Col.UIntFromDoubleClamped(0.098039), Col.UIntFromDoubleClamped(0.439216));
        public static C3ui MintCream => new C3ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392));
        public static C3ui MistyRose => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.894118), Col.UIntFromDoubleClamped(0.882353));
        public static C3ui Moccasin => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.894118), Col.UIntFromDoubleClamped(0.709804));
        public static C3ui NavajoWhite => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.870588), Col.UIntFromDoubleClamped(0.678431));
        public static C3ui Navy => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961));
        public static C3ui OldLace => new C3ui(Col.UIntFromDoubleClamped(0.992157), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.901961));
        public static C3ui Olive => new C3ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0));
        public static C3ui OliveDrab => new C3ui(Col.UIntFromDoubleClamped(0.419608), Col.UIntFromDoubleClamped(0.556863), Col.UIntFromDoubleClamped(0.137255));
        public static C3ui Orange => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.647059), Col.UIntFromDoubleClamped(0));
        public static C3ui OrangeRed => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.270588), Col.UIntFromDoubleClamped(0));
        public static C3ui Orchid => new C3ui(Col.UIntFromDoubleClamped(0.854902), Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.839216));
        public static C3ui PaleGoldenRod => new C3ui(Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.909804), Col.UIntFromDoubleClamped(0.666667));
        public static C3ui PaleGreen => new C3ui(Col.UIntFromDoubleClamped(0.596078), Col.UIntFromDoubleClamped(0.984314), Col.UIntFromDoubleClamped(0.596078));
        public static C3ui PaleTurquoise => new C3ui(Col.UIntFromDoubleClamped(0.686275), Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.933333));
        public static C3ui PaleVioletRed => new C3ui(Col.UIntFromDoubleClamped(0.847059), Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.576471));
        public static C3ui PapayaWhip => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.937255), Col.UIntFromDoubleClamped(0.835294));
        public static C3ui PeachPuff => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.854902), Col.UIntFromDoubleClamped(0.72549));
        public static C3ui Peru => new C3ui(Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.521569), Col.UIntFromDoubleClamped(0.247059));
        public static C3ui Pink => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.752941), Col.UIntFromDoubleClamped(0.796078));
        public static C3ui Plum => new C3ui(Col.UIntFromDoubleClamped(0.866667), Col.UIntFromDoubleClamped(0.627451), Col.UIntFromDoubleClamped(0.866667));
        public static C3ui PowderBlue => new C3ui(Col.UIntFromDoubleClamped(0.690196), Col.UIntFromDoubleClamped(0.878431), Col.UIntFromDoubleClamped(0.901961));
        public static C3ui Purple => new C3ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961));
        public static C3ui Red => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C3ui RosyBrown => new C3ui(Col.UIntFromDoubleClamped(0.737255), Col.UIntFromDoubleClamped(0.560784), Col.UIntFromDoubleClamped(0.560784));
        public static C3ui RoyalBlue => new C3ui(Col.UIntFromDoubleClamped(0.254902), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.882353));
        public static C3ui SaddleBrown => new C3ui(Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.270588), Col.UIntFromDoubleClamped(0.07451));
        public static C3ui Salmon => new C3ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.447059));
        public static C3ui SandyBrown => new C3ui(Col.UIntFromDoubleClamped(0.956863), Col.UIntFromDoubleClamped(0.643137), Col.UIntFromDoubleClamped(0.376471));
        public static C3ui SeaGreen => new C3ui(Col.UIntFromDoubleClamped(0.180392), Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.341176));
        public static C3ui SeaShell => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.933333));
        public static C3ui Sienna => new C3ui(Col.UIntFromDoubleClamped(0.627451), Col.UIntFromDoubleClamped(0.321569), Col.UIntFromDoubleClamped(0.176471));
        public static C3ui Silver => new C3ui(Col.UIntFromDoubleClamped(0.752941), Col.UIntFromDoubleClamped(0.752941), Col.UIntFromDoubleClamped(0.752941));
        public static C3ui SkyBlue => new C3ui(Col.UIntFromDoubleClamped(0.529412), Col.UIntFromDoubleClamped(0.807843), Col.UIntFromDoubleClamped(0.921569));
        public static C3ui SlateBlue => new C3ui(Col.UIntFromDoubleClamped(0.415686), Col.UIntFromDoubleClamped(0.352941), Col.UIntFromDoubleClamped(0.803922));
        public static C3ui SlateGray => new C3ui(Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.564706));
        public static C3ui SlateGrey => new C3ui(Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.564706));
        public static C3ui Snow => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.980392));
        public static C3ui SpringGreen => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.498039));
        public static C3ui SteelBlue => new C3ui(Col.UIntFromDoubleClamped(0.27451), Col.UIntFromDoubleClamped(0.509804), Col.UIntFromDoubleClamped(0.705882));
        public static C3ui Tan => new C3ui(Col.UIntFromDoubleClamped(0.823529), Col.UIntFromDoubleClamped(0.705882), Col.UIntFromDoubleClamped(0.54902));
        public static C3ui Teal => new C3ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C3ui Thistle => new C3ui(Col.UIntFromDoubleClamped(0.847059), Col.UIntFromDoubleClamped(0.74902), Col.UIntFromDoubleClamped(0.847059));
        public static C3ui Tomato => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.388235), Col.UIntFromDoubleClamped(0.278431));
        public static C3ui Turquoise => new C3ui(Col.UIntFromDoubleClamped(0.25098), Col.UIntFromDoubleClamped(0.878431), Col.UIntFromDoubleClamped(0.815686));
        public static C3ui Violet => new C3ui(Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.509804), Col.UIntFromDoubleClamped(0.933333));
        public static C3ui Wheat => new C3ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.870588), Col.UIntFromDoubleClamped(0.701961));
        public static C3ui White => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C3ui WhiteSmoke => new C3ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.960784));
        public static C3ui Yellow => new C3ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0));
        public static C3ui YellowGreen => new C3ui(Col.UIntFromDoubleClamped(0.603922), Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.196078));

        public static C3ui DarkYellow => Olive;

        public static C3ui VRVisGreen => new C3ui(Col.UIntFromDoubleClamped(0.698), Col.UIntFromDoubleClamped(0.851), Col.UIntFromDoubleClamped(0.008));

        public static C3ui Gray10 => new C3ui(Col.UIntFromDoubleClamped(0.1));
        public static C3ui Gray20 => new C3ui(Col.UIntFromDoubleClamped(0.2));
        public static C3ui Gray30 => new C3ui(Col.UIntFromDoubleClamped(0.3));
        public static C3ui Gray40 => new C3ui(Col.UIntFromDoubleClamped(0.4));
        public static C3ui Gray50 => new C3ui(Col.UIntFromDoubleClamped(0.5));
        public static C3ui Gray60 => new C3ui(Col.UIntFromDoubleClamped(0.6));
        public static C3ui Gray70 => new C3ui(Col.UIntFromDoubleClamped(0.7));
        public static C3ui Gray80 => new C3ui(Col.UIntFromDoubleClamped(0.8));
        public static C3ui Gray90 => new C3ui(Col.UIntFromDoubleClamped(0.9));

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

        public static C3ui operator *(C3ui col, double scalar)
        {
            return new C3ui(
                (uint)(col.R * scalar), 
                (uint)(col.G * scalar), 
                (uint)(col.B * scalar));
        }

        public static C3ui operator *(double scalar, C3ui col)
        {
            return new C3ui(
                (uint)(scalar * col.R), 
                (uint)(scalar * col.G), 
                (uint)(scalar * col.B));
        }

        public static C3ui operator /(C3ui col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C3ui(
                (uint)(col.R * f), 
                (uint)(col.G * f), 
                (uint)(col.B * f));
        }

        public static C3ui operator +(C3ui c0, C3b c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromByte(c1.R)), 
                (uint)(c0.G + Col.UIntFromByte(c1.G)), 
                (uint)(c0.B + Col.UIntFromByte(c1.B)));
        }

        public static C3ui operator -(C3ui c0, C3b c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromByte(c1.R)), 
                (uint)(c0.G - Col.UIntFromByte(c1.G)), 
                (uint)(c0.B - Col.UIntFromByte(c1.B)));
        }

        public static C3ui operator +(C3ui c0, C3us c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G + Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B + Col.UIntFromUShort(c1.B)));
        }

        public static C3ui operator -(C3ui c0, C3us c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G - Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B - Col.UIntFromUShort(c1.B)));
        }

        public static C3ui operator +(C3ui c0, C3ui c1)
        {
            return new C3ui(
                (uint)(c0.R + (c1.R)), 
                (uint)(c0.G + (c1.G)), 
                (uint)(c0.B + (c1.B)));
        }

        public static C3ui operator -(C3ui c0, C3ui c1)
        {
            return new C3ui(
                (uint)(c0.R - (c1.R)), 
                (uint)(c0.G - (c1.G)), 
                (uint)(c0.B - (c1.B)));
        }

        public static C3ui operator +(C3ui c0, C3f c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G + Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B + Col.UIntFromFloat(c1.B)));
        }

        public static C3ui operator -(C3ui c0, C3f c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G - Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B - Col.UIntFromFloat(c1.B)));
        }

        public static C3ui operator +(C3ui c0, C3d c1)
        {
            return new C3ui(
                (uint)(c0.R + Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G + Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B + Col.UIntFromDouble(c1.B)));
        }

        public static C3ui operator -(C3ui c0, C3d c1)
        {
            return new C3ui(
                (uint)(c0.R - Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G - Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B - Col.UIntFromDouble(c1.B)));
        }

        public static V3l operator + (V3l vec, C3ui color)
        {
            return new V3l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B)
                );
        }

        public static V3l operator -(V3l vec, C3ui color)
        {
            return new V3l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B)
                );
        }

        public static V4l operator + (V4l vec, C3ui color)
        {
            return new V4l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B),
                vec.W + (long)(UInt32.MaxValue)
                );
        }

        public static V4l operator -(V4l vec, C3ui color)
        {
            return new V4l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B),
                vec.W - (long)(UInt32.MaxValue)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(uint min, uint max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3ui Clamped(uint min, uint max)
        {
            return new C3ui(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp(Col.UIntFromDoubleClamped(min), Col.UIntFromDoubleClamped(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3ui Clamped(double min, double max)
        {
            return Clamped(Col.UIntFromDoubleClamped(min), Col.UIntFromDoubleClamped(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public long Norm1
        {
            get { return (long)R + (long)G + (long)B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public uint NormMax
        {
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public uint NormMin
        {
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
        public static bool ApproximateEquals(this C3ui a, C3ui b)
        {
            return ApproximateEquals(a, b, Constant<uint>.PositiveTinyValue);
        }

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

    [Serializable]
    public partial struct C3f : IFormattable, IEquatable<C3f>, IRGB
    {
        #region Constructors

        public C3f(float r, float g, float b)
        {
            R = r; G = g; B = b;
        }

        public C3f(int r, int g, int b)
        {
            R = (float)r; G = (float)g; B = (float)b;
        }

        public C3f(long r, long g, long b)
        {
            R = (float)r; G = (float)g; B = (float)b;
        }

        public C3f(double r, double g, double b)
        {
            R = (float)(r);
            G = (float)(g);
            B = (float)(b);
        }

        public C3f(float gray)
        {
            R = gray; G = gray; B = gray;
        }

        public C3f(double gray)
        {
            var value = (float)(gray);
            R = value; G = value; B = value;
        }

        public C3f(C3b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
        }

        public C3f(C3us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
        }

        public C3f(C3ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
        }

        public C3f(C3f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3f(C3d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
        }

        public C3f(C4b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
        }

        public C3f(C4us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
        }

        public C3f(C4ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
        }

        public C3f(C4f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3f(C4d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
        }

        public C3f(V3f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        public C3f(V3d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
        }

        public C3f(V4f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        public C3f(V4d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C3f color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C3f color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C3f color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3d(C3f color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C3f color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C3f color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C3f color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C3f color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C3f color)
        {
            return new C4d(color);
        }

        public static explicit operator V3f(C3f color)
        {
            return new V3f(
                (color.R), 
                (color.G), 
                (color.B)
                );
        }

        public static explicit operator V3d(C3f color)
        {
            return new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );
        }

        public static explicit operator V4f(C3f color)
        {
            return new V4f(
                (color.R), 
                (color.G), 
                (color.B),
                (1.0f)
                );
        }

        public static explicit operator V4d(C3f color)
        {
            return new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(1.0f)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C3f(Func<int, float> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        public V3f ToV3f() { return (V3f)this; }
        public V3d ToV3d() { return (V3d)this; }
        public V4f ToV4f() { return (V4f)this; }
        public V4d ToV4d() { return (V4d)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3b(C3b c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3us(C3us c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3ui(C3ui c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC3d(C3d c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4b(C4b c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4us(C4us c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4ui(C4ui c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4f(C4f c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromC4d(C4d c)
            => new C3f(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV3f(V3f c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV3d(V3d c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV4f(V4f c)
            => new C3f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f FromV4d(V4d c)
            => new C3f(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3b Copy(Func<float, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3us Copy(Func<float, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3ui Copy(Func<float, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3f Copy(Func<float, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3d Copy(Func<float, double> channel_fun)
        {
            return Map(channel_fun);
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
        public float this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C3f with all components zero.
        /// </summary>
        public static C3f Zero => new C3f(0.0f, 0.0f, 0.0f);

        // Web colors
        public static C3f AliceBlue => new C3f((float)(0.941176), (float)(0.972549), (float)(1));
        public static C3f AntiqueWhite => new C3f((float)(0.980392), (float)(0.921569), (float)(0.843137));
        public static C3f Aqua => new C3f((float)(0), (float)(1), (float)(1));
        public static C3f Aquamarine => new C3f((float)(0.498039), (float)(1), (float)(0.831373));
        public static C3f Azure => new C3f((float)(0.941176), (float)(1), (float)(1));
        public static C3f Beige => new C3f((float)(0.960784), (float)(0.960784), (float)(0.862745));
        public static C3f Bisque => new C3f((float)(1), (float)(0.894118), (float)(0.768627));
        public static C3f Black => new C3f((float)(0), (float)(0), (float)(0));
        public static C3f BlanchedAlmond => new C3f((float)(1), (float)(0.921569), (float)(0.803922));
        public static C3f Blue => new C3f((float)(0), (float)(0), (float)(1));
        public static C3f BlueViolet => new C3f((float)(0.541176), (float)(0.168627), (float)(0.886275));
        public static C3f Brown => new C3f((float)(0.647059), (float)(0.164706), (float)(0.164706));
        public static C3f BurlyWood => new C3f((float)(0.870588), (float)(0.721569), (float)(0.529412));
        public static C3f CadetBlue => new C3f((float)(0.372549), (float)(0.619608), (float)(0.627451));
        public static C3f Chartreuse => new C3f((float)(0.498039), (float)(1), (float)(0));
        public static C3f Chocolate => new C3f((float)(0.823529), (float)(0.411765), (float)(0.117647));
        public static C3f Coral => new C3f((float)(1), (float)(0.498039), (float)(0.313725));
        public static C3f CornflowerBlue => new C3f((float)(0.392157), (float)(0.584314), (float)(0.929412));
        public static C3f Cornsilk => new C3f((float)(1), (float)(0.972549), (float)(0.862745));
        public static C3f Crimson => new C3f((float)(0.862745), (float)(0.078431), (float)(0.235294));
        public static C3f Cyan => new C3f((float)(0), (float)(1), (float)(1));
        public static C3f DarkBlue => new C3f((float)(0), (float)(0), (float)(0.545098));
        public static C3f DarkCyan => new C3f((float)(0), (float)(0.545098), (float)(0.545098));
        public static C3f DarkGoldenRod => new C3f((float)(0.721569), (float)(0.52549), (float)(0.043137));
        public static C3f DarkGray => new C3f((float)(0.662745), (float)(0.662745), (float)(0.662745));
        public static C3f DarkGrey => new C3f((float)(0.662745), (float)(0.662745), (float)(0.662745));
        public static C3f DarkGreen => new C3f((float)(0), (float)(0.392157), (float)(0));
        public static C3f DarkKhaki => new C3f((float)(0.741176), (float)(0.717647), (float)(0.419608));
        public static C3f DarkMagenta => new C3f((float)(0.545098), (float)(0), (float)(0.545098));
        public static C3f DarkOliveGreen => new C3f((float)(0.333333), (float)(0.419608), (float)(0.184314));
        public static C3f DarkOrange => new C3f((float)(1), (float)(0.54902), (float)(0));
        public static C3f DarkOrchid => new C3f((float)(0.6), (float)(0.196078), (float)(0.8));
        public static C3f DarkRed => new C3f((float)(0.545098), (float)(0), (float)(0));
        public static C3f DarkSalmon => new C3f((float)(0.913725), (float)(0.588235), (float)(0.478431));
        public static C3f DarkSeaGreen => new C3f((float)(0.560784), (float)(0.737255), (float)(0.560784));
        public static C3f DarkSlateBlue => new C3f((float)(0.282353), (float)(0.239216), (float)(0.545098));
        public static C3f DarkSlateGray => new C3f((float)(0.184314), (float)(0.309804), (float)(0.309804));
        public static C3f DarkSlateGrey => new C3f((float)(0.184314), (float)(0.309804), (float)(0.309804));
        public static C3f DarkTurquoise => new C3f((float)(0), (float)(0.807843), (float)(0.819608));
        public static C3f DarkViolet => new C3f((float)(0.580392), (float)(0), (float)(0.827451));
        public static C3f DeepPink => new C3f((float)(1), (float)(0.078431), (float)(0.576471));
        public static C3f DeepSkyBlue => new C3f((float)(0), (float)(0.74902), (float)(1));
        public static C3f DimGray => new C3f((float)(0.411765), (float)(0.411765), (float)(0.411765));
        public static C3f DimGrey => new C3f((float)(0.411765), (float)(0.411765), (float)(0.411765));
        public static C3f DodgerBlue => new C3f((float)(0.117647), (float)(0.564706), (float)(1));
        public static C3f FireBrick => new C3f((float)(0.698039), (float)(0.133333), (float)(0.133333));
        public static C3f FloralWhite => new C3f((float)(1), (float)(0.980392), (float)(0.941176));
        public static C3f ForestGreen => new C3f((float)(0.133333), (float)(0.545098), (float)(0.133333));
        public static C3f Fuchsia => new C3f((float)(1), (float)(0), (float)(1));
        public static C3f Gainsboro => new C3f((float)(0.862745), (float)(0.862745), (float)(0.862745));
        public static C3f GhostWhite => new C3f((float)(0.972549), (float)(0.972549), (float)(1));
        public static C3f Gold => new C3f((float)(1), (float)(0.843137), (float)(0));
        public static C3f GoldenRod => new C3f((float)(0.854902), (float)(0.647059), (float)(0.12549));
        public static C3f Gray => new C3f((float)(0.501961), (float)(0.501961), (float)(0.501961));
        public static C3f Grey => new C3f((float)(0.501961), (float)(0.501961), (float)(0.501961));
        public static C3f Green => new C3f((float)(0), (float)(0.501961), (float)(0));
        public static C3f GreenYellow => new C3f((float)(0.678431), (float)(1), (float)(0.184314));
        public static C3f HoneyDew => new C3f((float)(0.941176), (float)(1), (float)(0.941176));
        public static C3f HotPink => new C3f((float)(1), (float)(0.411765), (float)(0.705882));
        public static C3f IndianRed  => new C3f((float)(0.803922), (float)(0.360784), (float)(0.360784));
        public static C3f Indigo  => new C3f((float)(0.294118), (float)(0), (float)(0.509804));
        public static C3f Ivory => new C3f((float)(1), (float)(1), (float)(0.941176));
        public static C3f Khaki => new C3f((float)(0.941176), (float)(0.901961), (float)(0.54902));
        public static C3f Lavender => new C3f((float)(0.901961), (float)(0.901961), (float)(0.980392));
        public static C3f LavenderBlush => new C3f((float)(1), (float)(0.941176), (float)(0.960784));
        public static C3f LawnGreen => new C3f((float)(0.486275), (float)(0.988235), (float)(0));
        public static C3f LemonChiffon => new C3f((float)(1), (float)(0.980392), (float)(0.803922));
        public static C3f LightBlue => new C3f((float)(0.678431), (float)(0.847059), (float)(0.901961));
        public static C3f LightCoral => new C3f((float)(0.941176), (float)(0.501961), (float)(0.501961));
        public static C3f LightCyan => new C3f((float)(0.878431), (float)(1), (float)(1));
        public static C3f LightGoldenRodYellow => new C3f((float)(0.980392), (float)(0.980392), (float)(0.823529));
        public static C3f LightGray => new C3f((float)(0.827451), (float)(0.827451), (float)(0.827451));
        public static C3f LightGrey => new C3f((float)(0.827451), (float)(0.827451), (float)(0.827451));
        public static C3f LightGreen => new C3f((float)(0.564706), (float)(0.933333), (float)(0.564706));
        public static C3f LightPink => new C3f((float)(1), (float)(0.713725), (float)(0.756863));
        public static C3f LightSalmon => new C3f((float)(1), (float)(0.627451), (float)(0.478431));
        public static C3f LightSeaGreen => new C3f((float)(0.12549), (float)(0.698039), (float)(0.666667));
        public static C3f LightSkyBlue => new C3f((float)(0.529412), (float)(0.807843), (float)(0.980392));
        public static C3f LightSlateGray => new C3f((float)(0.466667), (float)(0.533333), (float)(0.6));
        public static C3f LightSlateGrey => new C3f((float)(0.466667), (float)(0.533333), (float)(0.6));
        public static C3f LightSteelBlue => new C3f((float)(0.690196), (float)(0.768627), (float)(0.870588));
        public static C3f LightYellow => new C3f((float)(1), (float)(1), (float)(0.878431));
        public static C3f Lime => new C3f((float)(0), (float)(1), (float)(0));
        public static C3f LimeGreen => new C3f((float)(0.196078), (float)(0.803922), (float)(0.196078));
        public static C3f Linen => new C3f((float)(0.980392), (float)(0.941176), (float)(0.901961));
        public static C3f Magenta => new C3f((float)(1), (float)(0), (float)(1));
        public static C3f Maroon => new C3f((float)(0.501961), (float)(0), (float)(0));
        public static C3f MediumAquaMarine => new C3f((float)(0.4), (float)(0.803922), (float)(0.666667));
        public static C3f MediumBlue => new C3f((float)(0), (float)(0), (float)(0.803922));
        public static C3f MediumOrchid => new C3f((float)(0.729412), (float)(0.333333), (float)(0.827451));
        public static C3f MediumPurple => new C3f((float)(0.576471), (float)(0.439216), (float)(0.847059));
        public static C3f MediumSeaGreen => new C3f((float)(0.235294), (float)(0.701961), (float)(0.443137));
        public static C3f MediumSlateBlue => new C3f((float)(0.482353), (float)(0.407843), (float)(0.933333));
        public static C3f MediumSpringGreen => new C3f((float)(0), (float)(0.980392), (float)(0.603922));
        public static C3f MediumTurquoise => new C3f((float)(0.282353), (float)(0.819608), (float)(0.8));
        public static C3f MediumVioletRed => new C3f((float)(0.780392), (float)(0.082353), (float)(0.521569));
        public static C3f MidnightBlue => new C3f((float)(0.098039), (float)(0.098039), (float)(0.439216));
        public static C3f MintCream => new C3f((float)(0.960784), (float)(1), (float)(0.980392));
        public static C3f MistyRose => new C3f((float)(1), (float)(0.894118), (float)(0.882353));
        public static C3f Moccasin => new C3f((float)(1), (float)(0.894118), (float)(0.709804));
        public static C3f NavajoWhite => new C3f((float)(1), (float)(0.870588), (float)(0.678431));
        public static C3f Navy => new C3f((float)(0), (float)(0), (float)(0.501961));
        public static C3f OldLace => new C3f((float)(0.992157), (float)(0.960784), (float)(0.901961));
        public static C3f Olive => new C3f((float)(0.501961), (float)(0.501961), (float)(0));
        public static C3f OliveDrab => new C3f((float)(0.419608), (float)(0.556863), (float)(0.137255));
        public static C3f Orange => new C3f((float)(1), (float)(0.647059), (float)(0));
        public static C3f OrangeRed => new C3f((float)(1), (float)(0.270588), (float)(0));
        public static C3f Orchid => new C3f((float)(0.854902), (float)(0.439216), (float)(0.839216));
        public static C3f PaleGoldenRod => new C3f((float)(0.933333), (float)(0.909804), (float)(0.666667));
        public static C3f PaleGreen => new C3f((float)(0.596078), (float)(0.984314), (float)(0.596078));
        public static C3f PaleTurquoise => new C3f((float)(0.686275), (float)(0.933333), (float)(0.933333));
        public static C3f PaleVioletRed => new C3f((float)(0.847059), (float)(0.439216), (float)(0.576471));
        public static C3f PapayaWhip => new C3f((float)(1), (float)(0.937255), (float)(0.835294));
        public static C3f PeachPuff => new C3f((float)(1), (float)(0.854902), (float)(0.72549));
        public static C3f Peru => new C3f((float)(0.803922), (float)(0.521569), (float)(0.247059));
        public static C3f Pink => new C3f((float)(1), (float)(0.752941), (float)(0.796078));
        public static C3f Plum => new C3f((float)(0.866667), (float)(0.627451), (float)(0.866667));
        public static C3f PowderBlue => new C3f((float)(0.690196), (float)(0.878431), (float)(0.901961));
        public static C3f Purple => new C3f((float)(0.501961), (float)(0), (float)(0.501961));
        public static C3f Red => new C3f((float)(1), (float)(0), (float)(0));
        public static C3f RosyBrown => new C3f((float)(0.737255), (float)(0.560784), (float)(0.560784));
        public static C3f RoyalBlue => new C3f((float)(0.254902), (float)(0.411765), (float)(0.882353));
        public static C3f SaddleBrown => new C3f((float)(0.545098), (float)(0.270588), (float)(0.07451));
        public static C3f Salmon => new C3f((float)(0.980392), (float)(0.501961), (float)(0.447059));
        public static C3f SandyBrown => new C3f((float)(0.956863), (float)(0.643137), (float)(0.376471));
        public static C3f SeaGreen => new C3f((float)(0.180392), (float)(0.545098), (float)(0.341176));
        public static C3f SeaShell => new C3f((float)(1), (float)(0.960784), (float)(0.933333));
        public static C3f Sienna => new C3f((float)(0.627451), (float)(0.321569), (float)(0.176471));
        public static C3f Silver => new C3f((float)(0.752941), (float)(0.752941), (float)(0.752941));
        public static C3f SkyBlue => new C3f((float)(0.529412), (float)(0.807843), (float)(0.921569));
        public static C3f SlateBlue => new C3f((float)(0.415686), (float)(0.352941), (float)(0.803922));
        public static C3f SlateGray => new C3f((float)(0.439216), (float)(0.501961), (float)(0.564706));
        public static C3f SlateGrey => new C3f((float)(0.439216), (float)(0.501961), (float)(0.564706));
        public static C3f Snow => new C3f((float)(1), (float)(0.980392), (float)(0.980392));
        public static C3f SpringGreen => new C3f((float)(0), (float)(1), (float)(0.498039));
        public static C3f SteelBlue => new C3f((float)(0.27451), (float)(0.509804), (float)(0.705882));
        public static C3f Tan => new C3f((float)(0.823529), (float)(0.705882), (float)(0.54902));
        public static C3f Teal => new C3f((float)(0), (float)(0.501961), (float)(0.501961));
        public static C3f Thistle => new C3f((float)(0.847059), (float)(0.74902), (float)(0.847059));
        public static C3f Tomato => new C3f((float)(1), (float)(0.388235), (float)(0.278431));
        public static C3f Turquoise => new C3f((float)(0.25098), (float)(0.878431), (float)(0.815686));
        public static C3f Violet => new C3f((float)(0.933333), (float)(0.509804), (float)(0.933333));
        public static C3f Wheat => new C3f((float)(0.960784), (float)(0.870588), (float)(0.701961));
        public static C3f White => new C3f((float)(1), (float)(1), (float)(1));
        public static C3f WhiteSmoke => new C3f((float)(0.960784), (float)(0.960784), (float)(0.960784));
        public static C3f Yellow => new C3f((float)(1), (float)(1), (float)(0));
        public static C3f YellowGreen => new C3f((float)(0.603922), (float)(0.803922), (float)(0.196078));

        public static C3f DarkYellow => Olive;

        public static C3f VRVisGreen => new C3f((float)(0.698), (float)(0.851), (float)(0.008));

        public static C3f Gray10 => new C3f((float)(0.1));
        public static C3f Gray20 => new C3f((float)(0.2));
        public static C3f Gray30 => new C3f((float)(0.3));
        public static C3f Gray40 => new C3f((float)(0.4));
        public static C3f Gray50 => new C3f((float)(0.5));
        public static C3f Gray60 => new C3f((float)(0.6));
        public static C3f Gray70 => new C3f((float)(0.7));
        public static C3f Gray80 => new C3f((float)(0.8));
        public static C3f Gray90 => new C3f((float)(0.9));

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

        public static C3f operator *(C3f col, double scalar)
        {
            return new C3f(
                (float)(col.R * scalar), 
                (float)(col.G * scalar), 
                (float)(col.B * scalar));
        }

        public static C3f operator *(double scalar, C3f col)
        {
            return new C3f(
                (float)(scalar * col.R), 
                (float)(scalar * col.G), 
                (float)(scalar * col.B));
        }

        public static C3f operator /(C3f col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C3f(
                (float)(col.R * f), 
                (float)(col.G * f), 
                (float)(col.B * f));
        }

        public static C3f operator +(C3f c0, C3b c1)
        {
            return new C3f(
                (float)(c0.R + Col.FloatFromByte(c1.R)), 
                (float)(c0.G + Col.FloatFromByte(c1.G)), 
                (float)(c0.B + Col.FloatFromByte(c1.B)));
        }

        public static C3f operator -(C3f c0, C3b c1)
        {
            return new C3f(
                (float)(c0.R - Col.FloatFromByte(c1.R)), 
                (float)(c0.G - Col.FloatFromByte(c1.G)), 
                (float)(c0.B - Col.FloatFromByte(c1.B)));
        }

        public static C3f operator +(C3f c0, C3us c1)
        {
            return new C3f(
                (float)(c0.R + Col.FloatFromUShort(c1.R)), 
                (float)(c0.G + Col.FloatFromUShort(c1.G)), 
                (float)(c0.B + Col.FloatFromUShort(c1.B)));
        }

        public static C3f operator -(C3f c0, C3us c1)
        {
            return new C3f(
                (float)(c0.R - Col.FloatFromUShort(c1.R)), 
                (float)(c0.G - Col.FloatFromUShort(c1.G)), 
                (float)(c0.B - Col.FloatFromUShort(c1.B)));
        }

        public static C3f operator +(C3f c0, C3ui c1)
        {
            return new C3f(
                (float)(c0.R + Col.FloatFromUInt(c1.R)), 
                (float)(c0.G + Col.FloatFromUInt(c1.G)), 
                (float)(c0.B + Col.FloatFromUInt(c1.B)));
        }

        public static C3f operator -(C3f c0, C3ui c1)
        {
            return new C3f(
                (float)(c0.R - Col.FloatFromUInt(c1.R)), 
                (float)(c0.G - Col.FloatFromUInt(c1.G)), 
                (float)(c0.B - Col.FloatFromUInt(c1.B)));
        }

        public static C3f operator +(C3f c0, C3f c1)
        {
            return new C3f(
                (float)(c0.R + (c1.R)), 
                (float)(c0.G + (c1.G)), 
                (float)(c0.B + (c1.B)));
        }

        public static C3f operator -(C3f c0, C3f c1)
        {
            return new C3f(
                (float)(c0.R - (c1.R)), 
                (float)(c0.G - (c1.G)), 
                (float)(c0.B - (c1.B)));
        }

        public static C3f operator +(C3f c0, C3d c1)
        {
            return new C3f(
                (float)(c0.R + (float)(c1.R)), 
                (float)(c0.G + (float)(c1.G)), 
                (float)(c0.B + (float)(c1.B)));
        }

        public static C3f operator -(C3f c0, C3d c1)
        {
            return new C3f(
                (float)(c0.R - (float)(c1.R)), 
                (float)(c0.G - (float)(c1.G)), 
                (float)(c0.B - (float)(c1.B)));
        }

        public static C3f operator *(C3f c0, C3f c1)
        {
            return new C3f(
                (float)(c0.R * c1.R), 
                (float)(c0.G * c1.G), 
                (float)(c0.B * c1.B));
        }

        public static C3f operator /(C3f c0, C3f c1)
        {
            return new C3f(
                (float)(c0.R / c1.R), 
                (float)(c0.G / c1.G), 
                (float)(c0.B / c1.B));
        }

        public static C3f operator +(C3f col, double scalar)
        {
            return new C3f(
                (float)(col.R + scalar), 
                (float)(col.G + scalar), 
                (float)(col.B + scalar));
        }

        public static C3f operator +(double scalar, C3f col)
        {
            return new C3f(
                (float)(scalar + col.R), 
                (float)(scalar + col.G), 
                (float)(scalar + col.B));
        }

        public static C3f operator -(C3f col, double scalar)
        {
            return new C3f(
                (float)(col.R - scalar), 
                (float)(col.G - scalar), 
                (float)(col.B - scalar));
        }

        public static C3f operator -(double scalar, C3f col)
        {
            return new C3f(
                (float)(scalar - col.R), 
                (float)(scalar - col.G), 
                (float)(scalar - col.B));
        }

        public static V3f operator + (V3f vec, C3f color)
        {
            return new V3f(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B)
                );
        }

        public static V3f operator -(V3f vec, C3f color)
        {
            return new V3f(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B)
                );
        }

        public static V3d operator + (V3d vec, C3f color)
        {
            return new V3d(
                vec.X + (double)(color.R), 
                vec.Y + (double)(color.G), 
                vec.Z + (double)(color.B)
                );
        }

        public static V3d operator -(V3d vec, C3f color)
        {
            return new V3d(
                vec.X - (double)(color.R), 
                vec.Y - (double)(color.G), 
                vec.Z - (double)(color.B)
                );
        }

        public static V4f operator + (V4f vec, C3f color)
        {
            return new V4f(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B),
                vec.W + (1.0f)
                );
        }

        public static V4f operator -(V4f vec, C3f color)
        {
            return new V4f(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B),
                vec.W - (1.0f)
                );
        }

        public static V4d operator + (V4d vec, C3f color)
        {
            return new V4d(
                vec.X + (double)(color.R), 
                vec.Y + (double)(color.G), 
                vec.Z + (double)(color.B),
                vec.W + (double)(1.0f)
                );
        }

        public static V4d operator -(V4d vec, C3f color)
        {
            return new V4d(
                vec.X - (double)(color.R), 
                vec.Y - (double)(color.G), 
                vec.Z - (double)(color.B),
                vec.W - (double)(1.0f)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(float min, float max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3f Clamped(float min, float max)
        {
            return new C3f(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max));
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp((float)(min), (float)(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C3f Clamped(double min, double max)
        {
            return Clamped((float)(min), (float)(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. 
        /// </summary>
        public double Norm1
        {
            get { return (double)Fun.Abs(R) + (double)Fun.Abs(G) + (double)Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public float NormMax
        {
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public float NormMin
        {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3f p0, C3f p1, C3f p2, C3f p3, ref Tup4<float> w)
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
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, ref Tup4<double> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3f p0, C3f p1, C3f p2, C3f p3, ref Tup4<double> w)
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
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<float> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<float> w)
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
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<double> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
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

    [Serializable]
    public partial struct C3d : IFormattable, IEquatable<C3d>, IRGB
    {
        #region Constructors

        public C3d(double r, double g, double b)
        {
            R = r; G = g; B = b;
        }

        public C3d(int r, int g, int b)
        {
            R = (double)r; G = (double)g; B = (double)b;
        }

        public C3d(long r, long g, long b)
        {
            R = (double)r; G = (double)g; B = (double)b;
        }

        public C3d(double gray)
        {
            R = gray; G = gray; B = gray;
        }

        public C3d(C3b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
        }

        public C3d(C3us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
        }

        public C3d(C3ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
        }

        public C3d(C3f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
        }

        public C3d(C3d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3d(C4b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
        }

        public C3d(C4us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
        }

        public C3d(C4ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
        }

        public C3d(C4f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
        }

        public C3d(C4d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
        }

        public C3d(V3d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        public C3d(V4d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C3d color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C3d color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C3d color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C3d color)
        {
            return new C3f(color);
        }

        public static explicit operator C4b(C3d color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C3d color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C3d color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C3d color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C3d color)
        {
            return new C4d(color);
        }

        public static explicit operator V3d(C3d color)
        {
            return new V3d(
                (color.R), 
                (color.G), 
                (color.B)
                );
        }

        public static explicit operator V4d(C3d color)
        {
            return new V4d(
                (color.R), 
                (color.G), 
                (color.B),
                (1.0)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C3d(Func<int, double> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
        }

        public V3d ToV3d() { return (V3d)this; }
        public V4d ToV4d() { return (V4d)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3b(C3b c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3us(C3us c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3ui(C3ui c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC3f(C3f c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4b(C4b c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4us(C4us c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4ui(C4ui c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4f(C4f c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromC4d(C4d c)
            => new C3d(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromV3d(V3d c)
            => new C3d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d FromV4d(V4d c)
            => new C3d(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3b Copy(Func<double, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3us Copy(Func<double, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3ui Copy(Func<double, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3f Copy(Func<double, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C3d Copy(Func<double, double> channel_fun)
        {
            return Map(channel_fun);
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
        public double this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C3d with all components zero.
        /// </summary>
        public static C3d Zero => new C3d(0, 0, 0);

        // Web colors
        public static C3d AliceBlue => new C3d((0.941176), (0.972549), (1));
        public static C3d AntiqueWhite => new C3d((0.980392), (0.921569), (0.843137));
        public static C3d Aqua => new C3d((0), (1), (1));
        public static C3d Aquamarine => new C3d((0.498039), (1), (0.831373));
        public static C3d Azure => new C3d((0.941176), (1), (1));
        public static C3d Beige => new C3d((0.960784), (0.960784), (0.862745));
        public static C3d Bisque => new C3d((1), (0.894118), (0.768627));
        public static C3d Black => new C3d((0), (0), (0));
        public static C3d BlanchedAlmond => new C3d((1), (0.921569), (0.803922));
        public static C3d Blue => new C3d((0), (0), (1));
        public static C3d BlueViolet => new C3d((0.541176), (0.168627), (0.886275));
        public static C3d Brown => new C3d((0.647059), (0.164706), (0.164706));
        public static C3d BurlyWood => new C3d((0.870588), (0.721569), (0.529412));
        public static C3d CadetBlue => new C3d((0.372549), (0.619608), (0.627451));
        public static C3d Chartreuse => new C3d((0.498039), (1), (0));
        public static C3d Chocolate => new C3d((0.823529), (0.411765), (0.117647));
        public static C3d Coral => new C3d((1), (0.498039), (0.313725));
        public static C3d CornflowerBlue => new C3d((0.392157), (0.584314), (0.929412));
        public static C3d Cornsilk => new C3d((1), (0.972549), (0.862745));
        public static C3d Crimson => new C3d((0.862745), (0.078431), (0.235294));
        public static C3d Cyan => new C3d((0), (1), (1));
        public static C3d DarkBlue => new C3d((0), (0), (0.545098));
        public static C3d DarkCyan => new C3d((0), (0.545098), (0.545098));
        public static C3d DarkGoldenRod => new C3d((0.721569), (0.52549), (0.043137));
        public static C3d DarkGray => new C3d((0.662745), (0.662745), (0.662745));
        public static C3d DarkGrey => new C3d((0.662745), (0.662745), (0.662745));
        public static C3d DarkGreen => new C3d((0), (0.392157), (0));
        public static C3d DarkKhaki => new C3d((0.741176), (0.717647), (0.419608));
        public static C3d DarkMagenta => new C3d((0.545098), (0), (0.545098));
        public static C3d DarkOliveGreen => new C3d((0.333333), (0.419608), (0.184314));
        public static C3d DarkOrange => new C3d((1), (0.54902), (0));
        public static C3d DarkOrchid => new C3d((0.6), (0.196078), (0.8));
        public static C3d DarkRed => new C3d((0.545098), (0), (0));
        public static C3d DarkSalmon => new C3d((0.913725), (0.588235), (0.478431));
        public static C3d DarkSeaGreen => new C3d((0.560784), (0.737255), (0.560784));
        public static C3d DarkSlateBlue => new C3d((0.282353), (0.239216), (0.545098));
        public static C3d DarkSlateGray => new C3d((0.184314), (0.309804), (0.309804));
        public static C3d DarkSlateGrey => new C3d((0.184314), (0.309804), (0.309804));
        public static C3d DarkTurquoise => new C3d((0), (0.807843), (0.819608));
        public static C3d DarkViolet => new C3d((0.580392), (0), (0.827451));
        public static C3d DeepPink => new C3d((1), (0.078431), (0.576471));
        public static C3d DeepSkyBlue => new C3d((0), (0.74902), (1));
        public static C3d DimGray => new C3d((0.411765), (0.411765), (0.411765));
        public static C3d DimGrey => new C3d((0.411765), (0.411765), (0.411765));
        public static C3d DodgerBlue => new C3d((0.117647), (0.564706), (1));
        public static C3d FireBrick => new C3d((0.698039), (0.133333), (0.133333));
        public static C3d FloralWhite => new C3d((1), (0.980392), (0.941176));
        public static C3d ForestGreen => new C3d((0.133333), (0.545098), (0.133333));
        public static C3d Fuchsia => new C3d((1), (0), (1));
        public static C3d Gainsboro => new C3d((0.862745), (0.862745), (0.862745));
        public static C3d GhostWhite => new C3d((0.972549), (0.972549), (1));
        public static C3d Gold => new C3d((1), (0.843137), (0));
        public static C3d GoldenRod => new C3d((0.854902), (0.647059), (0.12549));
        public static C3d Gray => new C3d((0.501961), (0.501961), (0.501961));
        public static C3d Grey => new C3d((0.501961), (0.501961), (0.501961));
        public static C3d Green => new C3d((0), (0.501961), (0));
        public static C3d GreenYellow => new C3d((0.678431), (1), (0.184314));
        public static C3d HoneyDew => new C3d((0.941176), (1), (0.941176));
        public static C3d HotPink => new C3d((1), (0.411765), (0.705882));
        public static C3d IndianRed  => new C3d((0.803922), (0.360784), (0.360784));
        public static C3d Indigo  => new C3d((0.294118), (0), (0.509804));
        public static C3d Ivory => new C3d((1), (1), (0.941176));
        public static C3d Khaki => new C3d((0.941176), (0.901961), (0.54902));
        public static C3d Lavender => new C3d((0.901961), (0.901961), (0.980392));
        public static C3d LavenderBlush => new C3d((1), (0.941176), (0.960784));
        public static C3d LawnGreen => new C3d((0.486275), (0.988235), (0));
        public static C3d LemonChiffon => new C3d((1), (0.980392), (0.803922));
        public static C3d LightBlue => new C3d((0.678431), (0.847059), (0.901961));
        public static C3d LightCoral => new C3d((0.941176), (0.501961), (0.501961));
        public static C3d LightCyan => new C3d((0.878431), (1), (1));
        public static C3d LightGoldenRodYellow => new C3d((0.980392), (0.980392), (0.823529));
        public static C3d LightGray => new C3d((0.827451), (0.827451), (0.827451));
        public static C3d LightGrey => new C3d((0.827451), (0.827451), (0.827451));
        public static C3d LightGreen => new C3d((0.564706), (0.933333), (0.564706));
        public static C3d LightPink => new C3d((1), (0.713725), (0.756863));
        public static C3d LightSalmon => new C3d((1), (0.627451), (0.478431));
        public static C3d LightSeaGreen => new C3d((0.12549), (0.698039), (0.666667));
        public static C3d LightSkyBlue => new C3d((0.529412), (0.807843), (0.980392));
        public static C3d LightSlateGray => new C3d((0.466667), (0.533333), (0.6));
        public static C3d LightSlateGrey => new C3d((0.466667), (0.533333), (0.6));
        public static C3d LightSteelBlue => new C3d((0.690196), (0.768627), (0.870588));
        public static C3d LightYellow => new C3d((1), (1), (0.878431));
        public static C3d Lime => new C3d((0), (1), (0));
        public static C3d LimeGreen => new C3d((0.196078), (0.803922), (0.196078));
        public static C3d Linen => new C3d((0.980392), (0.941176), (0.901961));
        public static C3d Magenta => new C3d((1), (0), (1));
        public static C3d Maroon => new C3d((0.501961), (0), (0));
        public static C3d MediumAquaMarine => new C3d((0.4), (0.803922), (0.666667));
        public static C3d MediumBlue => new C3d((0), (0), (0.803922));
        public static C3d MediumOrchid => new C3d((0.729412), (0.333333), (0.827451));
        public static C3d MediumPurple => new C3d((0.576471), (0.439216), (0.847059));
        public static C3d MediumSeaGreen => new C3d((0.235294), (0.701961), (0.443137));
        public static C3d MediumSlateBlue => new C3d((0.482353), (0.407843), (0.933333));
        public static C3d MediumSpringGreen => new C3d((0), (0.980392), (0.603922));
        public static C3d MediumTurquoise => new C3d((0.282353), (0.819608), (0.8));
        public static C3d MediumVioletRed => new C3d((0.780392), (0.082353), (0.521569));
        public static C3d MidnightBlue => new C3d((0.098039), (0.098039), (0.439216));
        public static C3d MintCream => new C3d((0.960784), (1), (0.980392));
        public static C3d MistyRose => new C3d((1), (0.894118), (0.882353));
        public static C3d Moccasin => new C3d((1), (0.894118), (0.709804));
        public static C3d NavajoWhite => new C3d((1), (0.870588), (0.678431));
        public static C3d Navy => new C3d((0), (0), (0.501961));
        public static C3d OldLace => new C3d((0.992157), (0.960784), (0.901961));
        public static C3d Olive => new C3d((0.501961), (0.501961), (0));
        public static C3d OliveDrab => new C3d((0.419608), (0.556863), (0.137255));
        public static C3d Orange => new C3d((1), (0.647059), (0));
        public static C3d OrangeRed => new C3d((1), (0.270588), (0));
        public static C3d Orchid => new C3d((0.854902), (0.439216), (0.839216));
        public static C3d PaleGoldenRod => new C3d((0.933333), (0.909804), (0.666667));
        public static C3d PaleGreen => new C3d((0.596078), (0.984314), (0.596078));
        public static C3d PaleTurquoise => new C3d((0.686275), (0.933333), (0.933333));
        public static C3d PaleVioletRed => new C3d((0.847059), (0.439216), (0.576471));
        public static C3d PapayaWhip => new C3d((1), (0.937255), (0.835294));
        public static C3d PeachPuff => new C3d((1), (0.854902), (0.72549));
        public static C3d Peru => new C3d((0.803922), (0.521569), (0.247059));
        public static C3d Pink => new C3d((1), (0.752941), (0.796078));
        public static C3d Plum => new C3d((0.866667), (0.627451), (0.866667));
        public static C3d PowderBlue => new C3d((0.690196), (0.878431), (0.901961));
        public static C3d Purple => new C3d((0.501961), (0), (0.501961));
        public static C3d Red => new C3d((1), (0), (0));
        public static C3d RosyBrown => new C3d((0.737255), (0.560784), (0.560784));
        public static C3d RoyalBlue => new C3d((0.254902), (0.411765), (0.882353));
        public static C3d SaddleBrown => new C3d((0.545098), (0.270588), (0.07451));
        public static C3d Salmon => new C3d((0.980392), (0.501961), (0.447059));
        public static C3d SandyBrown => new C3d((0.956863), (0.643137), (0.376471));
        public static C3d SeaGreen => new C3d((0.180392), (0.545098), (0.341176));
        public static C3d SeaShell => new C3d((1), (0.960784), (0.933333));
        public static C3d Sienna => new C3d((0.627451), (0.321569), (0.176471));
        public static C3d Silver => new C3d((0.752941), (0.752941), (0.752941));
        public static C3d SkyBlue => new C3d((0.529412), (0.807843), (0.921569));
        public static C3d SlateBlue => new C3d((0.415686), (0.352941), (0.803922));
        public static C3d SlateGray => new C3d((0.439216), (0.501961), (0.564706));
        public static C3d SlateGrey => new C3d((0.439216), (0.501961), (0.564706));
        public static C3d Snow => new C3d((1), (0.980392), (0.980392));
        public static C3d SpringGreen => new C3d((0), (1), (0.498039));
        public static C3d SteelBlue => new C3d((0.27451), (0.509804), (0.705882));
        public static C3d Tan => new C3d((0.823529), (0.705882), (0.54902));
        public static C3d Teal => new C3d((0), (0.501961), (0.501961));
        public static C3d Thistle => new C3d((0.847059), (0.74902), (0.847059));
        public static C3d Tomato => new C3d((1), (0.388235), (0.278431));
        public static C3d Turquoise => new C3d((0.25098), (0.878431), (0.815686));
        public static C3d Violet => new C3d((0.933333), (0.509804), (0.933333));
        public static C3d Wheat => new C3d((0.960784), (0.870588), (0.701961));
        public static C3d White => new C3d((1), (1), (1));
        public static C3d WhiteSmoke => new C3d((0.960784), (0.960784), (0.960784));
        public static C3d Yellow => new C3d((1), (1), (0));
        public static C3d YellowGreen => new C3d((0.603922), (0.803922), (0.196078));

        public static C3d DarkYellow => Olive;

        public static C3d VRVisGreen => new C3d((0.698), (0.851), (0.008));

        public static C3d Gray10 => new C3d((0.1));
        public static C3d Gray20 => new C3d((0.2));
        public static C3d Gray30 => new C3d((0.3));
        public static C3d Gray40 => new C3d((0.4));
        public static C3d Gray50 => new C3d((0.5));
        public static C3d Gray60 => new C3d((0.6));
        public static C3d Gray70 => new C3d((0.7));
        public static C3d Gray80 => new C3d((0.8));
        public static C3d Gray90 => new C3d((0.9));

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

        public static C3d operator *(C3d col, double scalar)
        {
            return new C3d(
                (double)(col.R * scalar), 
                (double)(col.G * scalar), 
                (double)(col.B * scalar));
        }

        public static C3d operator *(double scalar, C3d col)
        {
            return new C3d(
                (double)(scalar * col.R), 
                (double)(scalar * col.G), 
                (double)(scalar * col.B));
        }

        public static C3d operator /(C3d col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C3d(
                (double)(col.R * f), 
                (double)(col.G * f), 
                (double)(col.B * f));
        }

        public static C3d operator +(C3d c0, C3b c1)
        {
            return new C3d(
                (double)(c0.R + Col.DoubleFromByte(c1.R)), 
                (double)(c0.G + Col.DoubleFromByte(c1.G)), 
                (double)(c0.B + Col.DoubleFromByte(c1.B)));
        }

        public static C3d operator -(C3d c0, C3b c1)
        {
            return new C3d(
                (double)(c0.R - Col.DoubleFromByte(c1.R)), 
                (double)(c0.G - Col.DoubleFromByte(c1.G)), 
                (double)(c0.B - Col.DoubleFromByte(c1.B)));
        }

        public static C3d operator +(C3d c0, C3us c1)
        {
            return new C3d(
                (double)(c0.R + Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G + Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B + Col.DoubleFromUShort(c1.B)));
        }

        public static C3d operator -(C3d c0, C3us c1)
        {
            return new C3d(
                (double)(c0.R - Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G - Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B - Col.DoubleFromUShort(c1.B)));
        }

        public static C3d operator +(C3d c0, C3ui c1)
        {
            return new C3d(
                (double)(c0.R + Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G + Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B + Col.DoubleFromUInt(c1.B)));
        }

        public static C3d operator -(C3d c0, C3ui c1)
        {
            return new C3d(
                (double)(c0.R - Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G - Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B - Col.DoubleFromUInt(c1.B)));
        }

        public static C3d operator +(C3d c0, C3f c1)
        {
            return new C3d(
                (double)(c0.R + (double)(c1.R)), 
                (double)(c0.G + (double)(c1.G)), 
                (double)(c0.B + (double)(c1.B)));
        }

        public static C3d operator -(C3d c0, C3f c1)
        {
            return new C3d(
                (double)(c0.R - (double)(c1.R)), 
                (double)(c0.G - (double)(c1.G)), 
                (double)(c0.B - (double)(c1.B)));
        }

        public static C3d operator +(C3d c0, C3d c1)
        {
            return new C3d(
                (double)(c0.R + (c1.R)), 
                (double)(c0.G + (c1.G)), 
                (double)(c0.B + (c1.B)));
        }

        public static C3d operator -(C3d c0, C3d c1)
        {
            return new C3d(
                (double)(c0.R - (c1.R)), 
                (double)(c0.G - (c1.G)), 
                (double)(c0.B - (c1.B)));
        }

        public static C3d operator *(C3d c0, C3d c1)
        {
            return new C3d(
                (double)(c0.R * c1.R), 
                (double)(c0.G * c1.G), 
                (double)(c0.B * c1.B));
        }

        public static C3d operator /(C3d c0, C3d c1)
        {
            return new C3d(
                (double)(c0.R / c1.R), 
                (double)(c0.G / c1.G), 
                (double)(c0.B / c1.B));
        }

        public static C3d operator +(C3d col, double scalar)
        {
            return new C3d(
                (double)(col.R + scalar), 
                (double)(col.G + scalar), 
                (double)(col.B + scalar));
        }

        public static C3d operator +(double scalar, C3d col)
        {
            return new C3d(
                (double)(scalar + col.R), 
                (double)(scalar + col.G), 
                (double)(scalar + col.B));
        }

        public static C3d operator -(C3d col, double scalar)
        {
            return new C3d(
                (double)(col.R - scalar), 
                (double)(col.G - scalar), 
                (double)(col.B - scalar));
        }

        public static C3d operator -(double scalar, C3d col)
        {
            return new C3d(
                (double)(scalar - col.R), 
                (double)(scalar - col.G), 
                (double)(scalar - col.B));
        }

        public static V3d operator + (V3d vec, C3d color)
        {
            return new V3d(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B)
                );
        }

        public static V3d operator -(V3d vec, C3d color)
        {
            return new V3d(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B)
                );
        }

        public static V4d operator + (V4d vec, C3d color)
        {
            return new V4d(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B),
                vec.W + (1.0)
                );
        }

        public static V4d operator -(V4d vec, C3d color)
        {
            return new V4d(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B),
                vec.W - (1.0)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
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
            get { return (double)Fun.Abs(R) + (double)Fun.Abs(G) + (double)Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). 
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). 
        /// </summary>
        public double NormMax
        {
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). 
        /// </summary>
        public double NormMin
        {
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
            C3d p0, C3d p1, C3d p2, C3d p3, ref Tup4<float> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3d p0, C3d p1, C3d p2, C3d p3, ref Tup4<float> w)
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
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, ref Tup4<double> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3d p0, C3d p1, C3d p2, C3d p3, ref Tup4<double> w)
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
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<float> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3f LinComRawF(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<float> w)
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
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<double> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C3d LinComRawD(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
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

    [Serializable]
    public partial struct C4b : IFormattable, IEquatable<C4b>, IRGB, IOpacity
    {
        #region Constructors

        public C4b(byte r, byte g, byte b, byte a)
        {
            R = r; G = g; B = b; A = a;
        }

        public C4b(int r, int g, int b, int a)
        {
            R = (byte)r; G = (byte)g; B = (byte)b; A = (byte)a;
        }

        public C4b(long r, long g, long b, long a)
        {
            R = (byte)r; G = (byte)g; B = (byte)b; A = (byte)a;
        }

        public C4b(double r, double g, double b, double a)
        {
            R = Col.ByteFromDoubleClamped(r);
            G = Col.ByteFromDoubleClamped(g);
            B = Col.ByteFromDoubleClamped(b);
            A = Col.ByteFromDoubleClamped(a);
        }

        public C4b(byte r, byte g, byte b)
        {
            R = r; G = g; B = b;
            A = 255;
        }

        public C4b(int r, int g, int b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
            A = 255;
        }

        public C4b(long r, long g, long b)
        {
            R = (byte)r; G = (byte)g; B = (byte)b;
            A = 255;
        }

        public C4b(double r, double g, double b)
        {
            R = Col.ByteFromDoubleClamped(r); G = Col.ByteFromDoubleClamped(g); B = Col.ByteFromDoubleClamped(b);
            A = 255;
        }

        public C4b(byte gray)
        {
            R = gray; G = gray; B = gray; A = 255;
        }

        public C4b(double gray)
        {
            var value = Col.ByteFromDoubleClamped(gray);
            R = value; G = value; B = value; A = 255;
        }

        public C4b(C3b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 255;
        }

        public C4b(C3b color, byte alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        public C4b(C3us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
            A = 255;
        }

        public C4b(C3us color, byte alpha)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
            A = alpha;
        }

        public C4b(C3ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
            A = 255;
        }

        public C4b(C3ui color, byte alpha)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
            A = alpha;
        }

        public C4b(C3f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
            A = 255;
        }

        public C4b(C3f color, byte alpha)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
            A = alpha;
        }

        public C4b(C3d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
            A = 255;
        }

        public C4b(C3d color, byte alpha)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
            A = alpha;
        }

        public C4b(C4b color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        public C4b(C4us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
            A = Col.ByteFromUShort(color.A);
        }

        public C4b(C4ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
            A = Col.ByteFromUInt(color.A);
        }

        public C4b(C4f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
            A = Col.ByteFromFloat(color.A);
        }

        public C4b(C4d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
            A = Col.ByteFromDouble(color.A);
        }

        public C4b(V3i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = 255;
        }

        public C4b(V3i vec, byte alpha)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = alpha;
        }

        public C4b(V3l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = 255;
        }

        public C4b(V3l vec, byte alpha)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = alpha;
        }

        public C4b(V4i vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = (byte)(vec.W);
        }

        public C4b(V4l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = (byte)(vec.W);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C4b color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C4b color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C4b color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C4b color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C4b color)
        {
            return new C3d(color);
        }

        public static explicit operator C4us(C4b color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C4b color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C4b color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C4b color)
        {
            return new C4d(color);
        }

        public static explicit operator V3i(C4b color)
        {
            return new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );
        }

        public static explicit operator V3l(C4b color)
        {
            return new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );
        }

        public static explicit operator V4i(C4b color)
        {
            return new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(color.A)
                );
        }

        public static explicit operator V4l(C4b color)
        {
            return new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(color.A)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C4b(Func<int, byte> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        public V3i ToV3i() { return (V3i)this; }
        public V3l ToV3l() { return (V3l)this; }
        public V4i ToV4i() { return (V4i)this; }
        public V4l ToV4l() { return (V4l)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3b(C3b c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3us(C3us c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3ui(C3ui c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3f(C3f c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC3d(C3d c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4us(C4us c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4ui(C4ui c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4f(C4f c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromC4d(C4d c)
            => new C4b(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV3i(V3i c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV3l(V3l c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV4i(V4i c)
            => new C4b(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4b FromV4l(V4l c)
            => new C4b(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4b Copy(Func<byte, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4us Copy(Func<byte, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4ui Copy(Func<byte, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4f Copy(Func<byte, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4d Copy(Func<byte, double> channel_fun)
        {
            return Map(channel_fun);
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
        public byte this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    case 3:
                        A = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    case 3:
                        return A;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C4b with all components zero.
        /// </summary>
        public static C4b Zero => new C4b(0, 0, 0, 0);

        // Web colors
        public static C4b AliceBlue => new C4b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(1));
        public static C4b AntiqueWhite => new C4b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.921569), Col.ByteFromDoubleClamped(0.843137));
        public static C4b Aqua => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C4b Aquamarine => new C4b(Col.ByteFromDoubleClamped(0.498039), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.831373));
        public static C4b Azure => new C4b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C4b Beige => new C4b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.862745));
        public static C4b Bisque => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.894118), Col.ByteFromDoubleClamped(0.768627));
        public static C4b Black => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C4b BlanchedAlmond => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.921569), Col.ByteFromDoubleClamped(0.803922));
        public static C4b Blue => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1));
        public static C4b BlueViolet => new C4b(Col.ByteFromDoubleClamped(0.541176), Col.ByteFromDoubleClamped(0.168627), Col.ByteFromDoubleClamped(0.886275));
        public static C4b Brown => new C4b(Col.ByteFromDoubleClamped(0.647059), Col.ByteFromDoubleClamped(0.164706), Col.ByteFromDoubleClamped(0.164706));
        public static C4b BurlyWood => new C4b(Col.ByteFromDoubleClamped(0.870588), Col.ByteFromDoubleClamped(0.721569), Col.ByteFromDoubleClamped(0.529412));
        public static C4b CadetBlue => new C4b(Col.ByteFromDoubleClamped(0.372549), Col.ByteFromDoubleClamped(0.619608), Col.ByteFromDoubleClamped(0.627451));
        public static C4b Chartreuse => new C4b(Col.ByteFromDoubleClamped(0.498039), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0));
        public static C4b Chocolate => new C4b(Col.ByteFromDoubleClamped(0.823529), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.117647));
        public static C4b Coral => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.498039), Col.ByteFromDoubleClamped(0.313725));
        public static C4b CornflowerBlue => new C4b(Col.ByteFromDoubleClamped(0.392157), Col.ByteFromDoubleClamped(0.584314), Col.ByteFromDoubleClamped(0.929412));
        public static C4b Cornsilk => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(0.862745));
        public static C4b Crimson => new C4b(Col.ByteFromDoubleClamped(0.862745), Col.ByteFromDoubleClamped(0.078431), Col.ByteFromDoubleClamped(0.235294));
        public static C4b Cyan => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C4b DarkBlue => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.545098));
        public static C4b DarkCyan => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.545098));
        public static C4b DarkGoldenRod => new C4b(Col.ByteFromDoubleClamped(0.721569), Col.ByteFromDoubleClamped(0.52549), Col.ByteFromDoubleClamped(0.043137));
        public static C4b DarkGray => new C4b(Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745));
        public static C4b DarkGrey => new C4b(Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745), Col.ByteFromDoubleClamped(0.662745));
        public static C4b DarkGreen => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.392157), Col.ByteFromDoubleClamped(0));
        public static C4b DarkKhaki => new C4b(Col.ByteFromDoubleClamped(0.741176), Col.ByteFromDoubleClamped(0.717647), Col.ByteFromDoubleClamped(0.419608));
        public static C4b DarkMagenta => new C4b(Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.545098));
        public static C4b DarkOliveGreen => new C4b(Col.ByteFromDoubleClamped(0.333333), Col.ByteFromDoubleClamped(0.419608), Col.ByteFromDoubleClamped(0.184314));
        public static C4b DarkOrange => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.54902), Col.ByteFromDoubleClamped(0));
        public static C4b DarkOrchid => new C4b(Col.ByteFromDoubleClamped(0.6), Col.ByteFromDoubleClamped(0.196078), Col.ByteFromDoubleClamped(0.8));
        public static C4b DarkRed => new C4b(Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C4b DarkSalmon => new C4b(Col.ByteFromDoubleClamped(0.913725), Col.ByteFromDoubleClamped(0.588235), Col.ByteFromDoubleClamped(0.478431));
        public static C4b DarkSeaGreen => new C4b(Col.ByteFromDoubleClamped(0.560784), Col.ByteFromDoubleClamped(0.737255), Col.ByteFromDoubleClamped(0.560784));
        public static C4b DarkSlateBlue => new C4b(Col.ByteFromDoubleClamped(0.282353), Col.ByteFromDoubleClamped(0.239216), Col.ByteFromDoubleClamped(0.545098));
        public static C4b DarkSlateGray => new C4b(Col.ByteFromDoubleClamped(0.184314), Col.ByteFromDoubleClamped(0.309804), Col.ByteFromDoubleClamped(0.309804));
        public static C4b DarkSlateGrey => new C4b(Col.ByteFromDoubleClamped(0.184314), Col.ByteFromDoubleClamped(0.309804), Col.ByteFromDoubleClamped(0.309804));
        public static C4b DarkTurquoise => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.807843), Col.ByteFromDoubleClamped(0.819608));
        public static C4b DarkViolet => new C4b(Col.ByteFromDoubleClamped(0.580392), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.827451));
        public static C4b DeepPink => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.078431), Col.ByteFromDoubleClamped(0.576471));
        public static C4b DeepSkyBlue => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.74902), Col.ByteFromDoubleClamped(1));
        public static C4b DimGray => new C4b(Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765));
        public static C4b DimGrey => new C4b(Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.411765));
        public static C4b DodgerBlue => new C4b(Col.ByteFromDoubleClamped(0.117647), Col.ByteFromDoubleClamped(0.564706), Col.ByteFromDoubleClamped(1));
        public static C4b FireBrick => new C4b(Col.ByteFromDoubleClamped(0.698039), Col.ByteFromDoubleClamped(0.133333), Col.ByteFromDoubleClamped(0.133333));
        public static C4b FloralWhite => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.941176));
        public static C4b ForestGreen => new C4b(Col.ByteFromDoubleClamped(0.133333), Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.133333));
        public static C4b Fuchsia => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1));
        public static C4b Gainsboro => new C4b(Col.ByteFromDoubleClamped(0.862745), Col.ByteFromDoubleClamped(0.862745), Col.ByteFromDoubleClamped(0.862745));
        public static C4b GhostWhite => new C4b(Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(0.972549), Col.ByteFromDoubleClamped(1));
        public static C4b Gold => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.843137), Col.ByteFromDoubleClamped(0));
        public static C4b GoldenRod => new C4b(Col.ByteFromDoubleClamped(0.854902), Col.ByteFromDoubleClamped(0.647059), Col.ByteFromDoubleClamped(0.12549));
        public static C4b Gray => new C4b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C4b Grey => new C4b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C4b Green => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0));
        public static C4b GreenYellow => new C4b(Col.ByteFromDoubleClamped(0.678431), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.184314));
        public static C4b HoneyDew => new C4b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.941176));
        public static C4b HotPink => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.705882));
        public static C4b IndianRed  => new C4b(Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.360784), Col.ByteFromDoubleClamped(0.360784));
        public static C4b Indigo  => new C4b(Col.ByteFromDoubleClamped(0.294118), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.509804));
        public static C4b Ivory => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.941176));
        public static C4b Khaki => new C4b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.901961), Col.ByteFromDoubleClamped(0.54902));
        public static C4b Lavender => new C4b(Col.ByteFromDoubleClamped(0.901961), Col.ByteFromDoubleClamped(0.901961), Col.ByteFromDoubleClamped(0.980392));
        public static C4b LavenderBlush => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.960784));
        public static C4b LawnGreen => new C4b(Col.ByteFromDoubleClamped(0.486275), Col.ByteFromDoubleClamped(0.988235), Col.ByteFromDoubleClamped(0));
        public static C4b LemonChiffon => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.803922));
        public static C4b LightBlue => new C4b(Col.ByteFromDoubleClamped(0.678431), Col.ByteFromDoubleClamped(0.847059), Col.ByteFromDoubleClamped(0.901961));
        public static C4b LightCoral => new C4b(Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C4b LightCyan => new C4b(Col.ByteFromDoubleClamped(0.878431), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C4b LightGoldenRodYellow => new C4b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.823529));
        public static C4b LightGray => new C4b(Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451));
        public static C4b LightGrey => new C4b(Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451), Col.ByteFromDoubleClamped(0.827451));
        public static C4b LightGreen => new C4b(Col.ByteFromDoubleClamped(0.564706), Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.564706));
        public static C4b LightPink => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.713725), Col.ByteFromDoubleClamped(0.756863));
        public static C4b LightSalmon => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.627451), Col.ByteFromDoubleClamped(0.478431));
        public static C4b LightSeaGreen => new C4b(Col.ByteFromDoubleClamped(0.12549), Col.ByteFromDoubleClamped(0.698039), Col.ByteFromDoubleClamped(0.666667));
        public static C4b LightSkyBlue => new C4b(Col.ByteFromDoubleClamped(0.529412), Col.ByteFromDoubleClamped(0.807843), Col.ByteFromDoubleClamped(0.980392));
        public static C4b LightSlateGray => new C4b(Col.ByteFromDoubleClamped(0.466667), Col.ByteFromDoubleClamped(0.533333), Col.ByteFromDoubleClamped(0.6));
        public static C4b LightSlateGrey => new C4b(Col.ByteFromDoubleClamped(0.466667), Col.ByteFromDoubleClamped(0.533333), Col.ByteFromDoubleClamped(0.6));
        public static C4b LightSteelBlue => new C4b(Col.ByteFromDoubleClamped(0.690196), Col.ByteFromDoubleClamped(0.768627), Col.ByteFromDoubleClamped(0.870588));
        public static C4b LightYellow => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.878431));
        public static C4b Lime => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0));
        public static C4b LimeGreen => new C4b(Col.ByteFromDoubleClamped(0.196078), Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.196078));
        public static C4b Linen => new C4b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.941176), Col.ByteFromDoubleClamped(0.901961));
        public static C4b Magenta => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1));
        public static C4b Maroon => new C4b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C4b MediumAquaMarine => new C4b(Col.ByteFromDoubleClamped(0.4), Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.666667));
        public static C4b MediumBlue => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.803922));
        public static C4b MediumOrchid => new C4b(Col.ByteFromDoubleClamped(0.729412), Col.ByteFromDoubleClamped(0.333333), Col.ByteFromDoubleClamped(0.827451));
        public static C4b MediumPurple => new C4b(Col.ByteFromDoubleClamped(0.576471), Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.847059));
        public static C4b MediumSeaGreen => new C4b(Col.ByteFromDoubleClamped(0.235294), Col.ByteFromDoubleClamped(0.701961), Col.ByteFromDoubleClamped(0.443137));
        public static C4b MediumSlateBlue => new C4b(Col.ByteFromDoubleClamped(0.482353), Col.ByteFromDoubleClamped(0.407843), Col.ByteFromDoubleClamped(0.933333));
        public static C4b MediumSpringGreen => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.603922));
        public static C4b MediumTurquoise => new C4b(Col.ByteFromDoubleClamped(0.282353), Col.ByteFromDoubleClamped(0.819608), Col.ByteFromDoubleClamped(0.8));
        public static C4b MediumVioletRed => new C4b(Col.ByteFromDoubleClamped(0.780392), Col.ByteFromDoubleClamped(0.082353), Col.ByteFromDoubleClamped(0.521569));
        public static C4b MidnightBlue => new C4b(Col.ByteFromDoubleClamped(0.098039), Col.ByteFromDoubleClamped(0.098039), Col.ByteFromDoubleClamped(0.439216));
        public static C4b MintCream => new C4b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392));
        public static C4b MistyRose => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.894118), Col.ByteFromDoubleClamped(0.882353));
        public static C4b Moccasin => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.894118), Col.ByteFromDoubleClamped(0.709804));
        public static C4b NavajoWhite => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.870588), Col.ByteFromDoubleClamped(0.678431));
        public static C4b Navy => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961));
        public static C4b OldLace => new C4b(Col.ByteFromDoubleClamped(0.992157), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.901961));
        public static C4b Olive => new C4b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0));
        public static C4b OliveDrab => new C4b(Col.ByteFromDoubleClamped(0.419608), Col.ByteFromDoubleClamped(0.556863), Col.ByteFromDoubleClamped(0.137255));
        public static C4b Orange => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.647059), Col.ByteFromDoubleClamped(0));
        public static C4b OrangeRed => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.270588), Col.ByteFromDoubleClamped(0));
        public static C4b Orchid => new C4b(Col.ByteFromDoubleClamped(0.854902), Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.839216));
        public static C4b PaleGoldenRod => new C4b(Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.909804), Col.ByteFromDoubleClamped(0.666667));
        public static C4b PaleGreen => new C4b(Col.ByteFromDoubleClamped(0.596078), Col.ByteFromDoubleClamped(0.984314), Col.ByteFromDoubleClamped(0.596078));
        public static C4b PaleTurquoise => new C4b(Col.ByteFromDoubleClamped(0.686275), Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.933333));
        public static C4b PaleVioletRed => new C4b(Col.ByteFromDoubleClamped(0.847059), Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.576471));
        public static C4b PapayaWhip => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.937255), Col.ByteFromDoubleClamped(0.835294));
        public static C4b PeachPuff => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.854902), Col.ByteFromDoubleClamped(0.72549));
        public static C4b Peru => new C4b(Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.521569), Col.ByteFromDoubleClamped(0.247059));
        public static C4b Pink => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.752941), Col.ByteFromDoubleClamped(0.796078));
        public static C4b Plum => new C4b(Col.ByteFromDoubleClamped(0.866667), Col.ByteFromDoubleClamped(0.627451), Col.ByteFromDoubleClamped(0.866667));
        public static C4b PowderBlue => new C4b(Col.ByteFromDoubleClamped(0.690196), Col.ByteFromDoubleClamped(0.878431), Col.ByteFromDoubleClamped(0.901961));
        public static C4b Purple => new C4b(Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961));
        public static C4b Red => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0));
        public static C4b RosyBrown => new C4b(Col.ByteFromDoubleClamped(0.737255), Col.ByteFromDoubleClamped(0.560784), Col.ByteFromDoubleClamped(0.560784));
        public static C4b RoyalBlue => new C4b(Col.ByteFromDoubleClamped(0.254902), Col.ByteFromDoubleClamped(0.411765), Col.ByteFromDoubleClamped(0.882353));
        public static C4b SaddleBrown => new C4b(Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.270588), Col.ByteFromDoubleClamped(0.07451));
        public static C4b Salmon => new C4b(Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.447059));
        public static C4b SandyBrown => new C4b(Col.ByteFromDoubleClamped(0.956863), Col.ByteFromDoubleClamped(0.643137), Col.ByteFromDoubleClamped(0.376471));
        public static C4b SeaGreen => new C4b(Col.ByteFromDoubleClamped(0.180392), Col.ByteFromDoubleClamped(0.545098), Col.ByteFromDoubleClamped(0.341176));
        public static C4b SeaShell => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.933333));
        public static C4b Sienna => new C4b(Col.ByteFromDoubleClamped(0.627451), Col.ByteFromDoubleClamped(0.321569), Col.ByteFromDoubleClamped(0.176471));
        public static C4b Silver => new C4b(Col.ByteFromDoubleClamped(0.752941), Col.ByteFromDoubleClamped(0.752941), Col.ByteFromDoubleClamped(0.752941));
        public static C4b SkyBlue => new C4b(Col.ByteFromDoubleClamped(0.529412), Col.ByteFromDoubleClamped(0.807843), Col.ByteFromDoubleClamped(0.921569));
        public static C4b SlateBlue => new C4b(Col.ByteFromDoubleClamped(0.415686), Col.ByteFromDoubleClamped(0.352941), Col.ByteFromDoubleClamped(0.803922));
        public static C4b SlateGray => new C4b(Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.564706));
        public static C4b SlateGrey => new C4b(Col.ByteFromDoubleClamped(0.439216), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.564706));
        public static C4b Snow => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.980392), Col.ByteFromDoubleClamped(0.980392));
        public static C4b SpringGreen => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.498039));
        public static C4b SteelBlue => new C4b(Col.ByteFromDoubleClamped(0.27451), Col.ByteFromDoubleClamped(0.509804), Col.ByteFromDoubleClamped(0.705882));
        public static C4b Tan => new C4b(Col.ByteFromDoubleClamped(0.823529), Col.ByteFromDoubleClamped(0.705882), Col.ByteFromDoubleClamped(0.54902));
        public static C4b Teal => new C4b(Col.ByteFromDoubleClamped(0), Col.ByteFromDoubleClamped(0.501961), Col.ByteFromDoubleClamped(0.501961));
        public static C4b Thistle => new C4b(Col.ByteFromDoubleClamped(0.847059), Col.ByteFromDoubleClamped(0.74902), Col.ByteFromDoubleClamped(0.847059));
        public static C4b Tomato => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0.388235), Col.ByteFromDoubleClamped(0.278431));
        public static C4b Turquoise => new C4b(Col.ByteFromDoubleClamped(0.25098), Col.ByteFromDoubleClamped(0.878431), Col.ByteFromDoubleClamped(0.815686));
        public static C4b Violet => new C4b(Col.ByteFromDoubleClamped(0.933333), Col.ByteFromDoubleClamped(0.509804), Col.ByteFromDoubleClamped(0.933333));
        public static C4b Wheat => new C4b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.870588), Col.ByteFromDoubleClamped(0.701961));
        public static C4b White => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1));
        public static C4b WhiteSmoke => new C4b(Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.960784), Col.ByteFromDoubleClamped(0.960784));
        public static C4b Yellow => new C4b(Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(1), Col.ByteFromDoubleClamped(0));
        public static C4b YellowGreen => new C4b(Col.ByteFromDoubleClamped(0.603922), Col.ByteFromDoubleClamped(0.803922), Col.ByteFromDoubleClamped(0.196078));

        public static C4b DarkYellow => Olive;

        public static C4b VRVisGreen => new C4b(Col.ByteFromDoubleClamped(0.698), Col.ByteFromDoubleClamped(0.851), Col.ByteFromDoubleClamped(0.008));

        public static C4b Gray10 => new C4b(Col.ByteFromDoubleClamped(0.1));
        public static C4b Gray20 => new C4b(Col.ByteFromDoubleClamped(0.2));
        public static C4b Gray30 => new C4b(Col.ByteFromDoubleClamped(0.3));
        public static C4b Gray40 => new C4b(Col.ByteFromDoubleClamped(0.4));
        public static C4b Gray50 => new C4b(Col.ByteFromDoubleClamped(0.5));
        public static C4b Gray60 => new C4b(Col.ByteFromDoubleClamped(0.6));
        public static C4b Gray70 => new C4b(Col.ByteFromDoubleClamped(0.7));
        public static C4b Gray80 => new C4b(Col.ByteFromDoubleClamped(0.8));
        public static C4b Gray90 => new C4b(Col.ByteFromDoubleClamped(0.9));

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

        public static C4b operator *(C4b col, double scalar)
        {
            return new C4b(
                (byte)(col.R * scalar), 
                (byte)(col.G * scalar), 
                (byte)(col.B * scalar), 
                (byte)(col.A * scalar));
        }

        public static C4b operator *(double scalar, C4b col)
        {
            return new C4b(
                (byte)(scalar * col.R), 
                (byte)(scalar * col.G), 
                (byte)(scalar * col.B), 
                (byte)(scalar * col.A));
        }

        public static C4b operator /(C4b col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C4b(
                (byte)(col.R * f), 
                (byte)(col.G * f), 
                (byte)(col.B * f), 
                (byte)(col.A * f));
        }

        public static C4b operator +(C4b c0, C4b c1)
        {
            return new C4b(
                (byte)(c0.R + (c1.R)), 
                (byte)(c0.G + (c1.G)), 
                (byte)(c0.B + (c1.B)), 
                (byte)(c0.A + (c1.A)));
        }

        public static C4b operator -(C4b c0, C4b c1)
        {
            return new C4b(
                (byte)(c0.R - (c1.R)), 
                (byte)(c0.G - (c1.G)), 
                (byte)(c0.B - (c1.B)), 
                (byte)(c0.A - (c1.A)));
        }

        public static C4b operator +(C4b c0, C4us c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G + Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B + Col.ByteFromUShort(c1.B)), 
                (byte)(c0.A + Col.ByteFromUShort(c1.A)));
        }

        public static C4b operator -(C4b c0, C4us c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromUShort(c1.R)), 
                (byte)(c0.G - Col.ByteFromUShort(c1.G)), 
                (byte)(c0.B - Col.ByteFromUShort(c1.B)), 
                (byte)(c0.A - Col.ByteFromUShort(c1.A)));
        }

        public static C4b operator +(C4b c0, C4ui c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G + Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B + Col.ByteFromUInt(c1.B)), 
                (byte)(c0.A + Col.ByteFromUInt(c1.A)));
        }

        public static C4b operator -(C4b c0, C4ui c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromUInt(c1.R)), 
                (byte)(c0.G - Col.ByteFromUInt(c1.G)), 
                (byte)(c0.B - Col.ByteFromUInt(c1.B)), 
                (byte)(c0.A - Col.ByteFromUInt(c1.A)));
        }

        public static C4b operator +(C4b c0, C4f c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G + Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B + Col.ByteFromFloat(c1.B)), 
                (byte)(c0.A + Col.ByteFromFloat(c1.A)));
        }

        public static C4b operator -(C4b c0, C4f c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromFloat(c1.R)), 
                (byte)(c0.G - Col.ByteFromFloat(c1.G)), 
                (byte)(c0.B - Col.ByteFromFloat(c1.B)), 
                (byte)(c0.A - Col.ByteFromFloat(c1.A)));
        }

        public static C4b operator +(C4b c0, C4d c1)
        {
            return new C4b(
                (byte)(c0.R + Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G + Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B + Col.ByteFromDouble(c1.B)), 
                (byte)(c0.A + Col.ByteFromDouble(c1.A)));
        }

        public static C4b operator -(C4b c0, C4d c1)
        {
            return new C4b(
                (byte)(c0.R - Col.ByteFromDouble(c1.R)), 
                (byte)(c0.G - Col.ByteFromDouble(c1.G)), 
                (byte)(c0.B - Col.ByteFromDouble(c1.B)), 
                (byte)(c0.A - Col.ByteFromDouble(c1.A)));
        }

        public static V3i operator + (V3i vec, C4b color)
        {
            return new V3i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B)
                );
        }

        public static V3i operator -(V3i vec, C4b color)
        {
            return new V3i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B)
                );
        }

        public static V3l operator + (V3l vec, C4b color)
        {
            return new V3l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B)
                );
        }

        public static V3l operator -(V3l vec, C4b color)
        {
            return new V3l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B)
                );
        }

        public static V4i operator + (V4i vec, C4b color)
        {
            return new V4i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B),
                vec.W + (int)(color.A)
                );
        }

        public static V4i operator -(V4i vec, C4b color)
        {
            return new V4i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B),
                vec.W - (int)(color.A)
                );
        }

        public static V4l operator + (V4l vec, C4b color)
        {
            return new V4l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B),
                vec.W + (long)(color.A)
                );
        }

        public static V4l operator -(V4l vec, C4b color)
        {
            return new V4l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B),
                vec.W - (long)(color.A)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(byte min, byte max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4b Clamped(byte min, byte max)
        {
            return new C4b(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp(Col.ByteFromDoubleClamped(min), Col.ByteFromDoubleClamped(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4b Clamped(double min, double max)
        {
            return Clamped(Col.ByteFromDoubleClamped(min), Col.ByteFromDoubleClamped(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public int Norm1
        {
            get { return (int)R + (int)G + (int)B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public byte NormMax
        {
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public byte NormMin
        {
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
        public static bool ApproximateEquals(this C4b a, C4b b)
        {
            return ApproximateEquals(a, b, Constant<byte>.PositiveTinyValue);
        }

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

    [Serializable]
    public partial struct C4us : IFormattable, IEquatable<C4us>, IRGB, IOpacity
    {
        #region Constructors

        public C4us(ushort r, ushort g, ushort b, ushort a)
        {
            R = r; G = g; B = b; A = a;
        }

        public C4us(int r, int g, int b, int a)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b; A = (ushort)a;
        }

        public C4us(long r, long g, long b, long a)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b; A = (ushort)a;
        }

        public C4us(double r, double g, double b, double a)
        {
            R = Col.UShortFromDoubleClamped(r);
            G = Col.UShortFromDoubleClamped(g);
            B = Col.UShortFromDoubleClamped(b);
            A = Col.UShortFromDoubleClamped(a);
        }

        public C4us(ushort r, ushort g, ushort b)
        {
            R = r; G = g; B = b;
            A = 65535;
        }

        public C4us(int r, int g, int b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
            A = 65535;
        }

        public C4us(long r, long g, long b)
        {
            R = (ushort)r; G = (ushort)g; B = (ushort)b;
            A = 65535;
        }

        public C4us(double r, double g, double b)
        {
            R = Col.UShortFromDoubleClamped(r); G = Col.UShortFromDoubleClamped(g); B = Col.UShortFromDoubleClamped(b);
            A = 65535;
        }

        public C4us(ushort gray)
        {
            R = gray; G = gray; B = gray; A = 65535;
        }

        public C4us(double gray)
        {
            var value = Col.UShortFromDoubleClamped(gray);
            R = value; G = value; B = value; A = 65535;
        }

        public C4us(C3b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
            A = 65535;
        }

        public C4us(C3b color, ushort alpha)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
            A = alpha;
        }

        public C4us(C3us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 65535;
        }

        public C4us(C3us color, ushort alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        public C4us(C3ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
            A = 65535;
        }

        public C4us(C3ui color, ushort alpha)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
            A = alpha;
        }

        public C4us(C3f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
            A = 65535;
        }

        public C4us(C3f color, ushort alpha)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
            A = alpha;
        }

        public C4us(C3d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
            A = 65535;
        }

        public C4us(C3d color, ushort alpha)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
            A = alpha;
        }

        public C4us(C4b color)
        {
            R = Col.UShortFromByte(color.R);
            G = Col.UShortFromByte(color.G);
            B = Col.UShortFromByte(color.B);
            A = Col.UShortFromByte(color.A);
        }

        public C4us(C4us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        public C4us(C4ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
            A = Col.UShortFromUInt(color.A);
        }

        public C4us(C4f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
            A = Col.UShortFromFloat(color.A);
        }

        public C4us(C4d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
            A = Col.UShortFromDouble(color.A);
        }

        public C4us(V3i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = 65535;
        }

        public C4us(V3i vec, ushort alpha)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = alpha;
        }

        public C4us(V3l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = 65535;
        }

        public C4us(V3l vec, ushort alpha)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = alpha;
        }

        public C4us(V4i vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = (ushort)(vec.W);
        }

        public C4us(V4l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = (ushort)(vec.W);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C4us color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C4us color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C4us color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C4us color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C4us color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C4us color)
        {
            return new C4b(color);
        }

        public static explicit operator C4ui(C4us color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C4us color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C4us color)
        {
            return new C4d(color);
        }

        public static explicit operator V3i(C4us color)
        {
            return new V3i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B)
                );
        }

        public static explicit operator V3l(C4us color)
        {
            return new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );
        }

        public static explicit operator V4i(C4us color)
        {
            return new V4i(
                (int)(color.R), 
                (int)(color.G), 
                (int)(color.B),
                (int)(color.A)
                );
        }

        public static explicit operator V4l(C4us color)
        {
            return new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(color.A)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C4us(Func<int, ushort> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        public V3i ToV3i() { return (V3i)this; }
        public V3l ToV3l() { return (V3l)this; }
        public V4i ToV4i() { return (V4i)this; }
        public V4l ToV4l() { return (V4l)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3b(C3b c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3us(C3us c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3ui(C3ui c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3f(C3f c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC3d(C3d c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4b(C4b c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4ui(C4ui c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4f(C4f c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromC4d(C4d c)
            => new C4us(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV3i(V3i c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV3l(V3l c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV4i(V4i c)
            => new C4us(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4us FromV4l(V4l c)
            => new C4us(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4b Copy(Func<ushort, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4us Copy(Func<ushort, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4ui Copy(Func<ushort, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4f Copy(Func<ushort, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4d Copy(Func<ushort, double> channel_fun)
        {
            return Map(channel_fun);
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
        public ushort this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    case 3:
                        A = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    case 3:
                        return A;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C4us with all components zero.
        /// </summary>
        public static C4us Zero => new C4us(0, 0, 0, 0);

        // Web colors
        public static C4us AliceBlue => new C4us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(1));
        public static C4us AntiqueWhite => new C4us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.921569), Col.UShortFromDoubleClamped(0.843137));
        public static C4us Aqua => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C4us Aquamarine => new C4us(Col.UShortFromDoubleClamped(0.498039), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.831373));
        public static C4us Azure => new C4us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C4us Beige => new C4us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.862745));
        public static C4us Bisque => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.894118), Col.UShortFromDoubleClamped(0.768627));
        public static C4us Black => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C4us BlanchedAlmond => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.921569), Col.UShortFromDoubleClamped(0.803922));
        public static C4us Blue => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1));
        public static C4us BlueViolet => new C4us(Col.UShortFromDoubleClamped(0.541176), Col.UShortFromDoubleClamped(0.168627), Col.UShortFromDoubleClamped(0.886275));
        public static C4us Brown => new C4us(Col.UShortFromDoubleClamped(0.647059), Col.UShortFromDoubleClamped(0.164706), Col.UShortFromDoubleClamped(0.164706));
        public static C4us BurlyWood => new C4us(Col.UShortFromDoubleClamped(0.870588), Col.UShortFromDoubleClamped(0.721569), Col.UShortFromDoubleClamped(0.529412));
        public static C4us CadetBlue => new C4us(Col.UShortFromDoubleClamped(0.372549), Col.UShortFromDoubleClamped(0.619608), Col.UShortFromDoubleClamped(0.627451));
        public static C4us Chartreuse => new C4us(Col.UShortFromDoubleClamped(0.498039), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0));
        public static C4us Chocolate => new C4us(Col.UShortFromDoubleClamped(0.823529), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.117647));
        public static C4us Coral => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.498039), Col.UShortFromDoubleClamped(0.313725));
        public static C4us CornflowerBlue => new C4us(Col.UShortFromDoubleClamped(0.392157), Col.UShortFromDoubleClamped(0.584314), Col.UShortFromDoubleClamped(0.929412));
        public static C4us Cornsilk => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(0.862745));
        public static C4us Crimson => new C4us(Col.UShortFromDoubleClamped(0.862745), Col.UShortFromDoubleClamped(0.078431), Col.UShortFromDoubleClamped(0.235294));
        public static C4us Cyan => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C4us DarkBlue => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.545098));
        public static C4us DarkCyan => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.545098));
        public static C4us DarkGoldenRod => new C4us(Col.UShortFromDoubleClamped(0.721569), Col.UShortFromDoubleClamped(0.52549), Col.UShortFromDoubleClamped(0.043137));
        public static C4us DarkGray => new C4us(Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745));
        public static C4us DarkGrey => new C4us(Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745), Col.UShortFromDoubleClamped(0.662745));
        public static C4us DarkGreen => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.392157), Col.UShortFromDoubleClamped(0));
        public static C4us DarkKhaki => new C4us(Col.UShortFromDoubleClamped(0.741176), Col.UShortFromDoubleClamped(0.717647), Col.UShortFromDoubleClamped(0.419608));
        public static C4us DarkMagenta => new C4us(Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.545098));
        public static C4us DarkOliveGreen => new C4us(Col.UShortFromDoubleClamped(0.333333), Col.UShortFromDoubleClamped(0.419608), Col.UShortFromDoubleClamped(0.184314));
        public static C4us DarkOrange => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.54902), Col.UShortFromDoubleClamped(0));
        public static C4us DarkOrchid => new C4us(Col.UShortFromDoubleClamped(0.6), Col.UShortFromDoubleClamped(0.196078), Col.UShortFromDoubleClamped(0.8));
        public static C4us DarkRed => new C4us(Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C4us DarkSalmon => new C4us(Col.UShortFromDoubleClamped(0.913725), Col.UShortFromDoubleClamped(0.588235), Col.UShortFromDoubleClamped(0.478431));
        public static C4us DarkSeaGreen => new C4us(Col.UShortFromDoubleClamped(0.560784), Col.UShortFromDoubleClamped(0.737255), Col.UShortFromDoubleClamped(0.560784));
        public static C4us DarkSlateBlue => new C4us(Col.UShortFromDoubleClamped(0.282353), Col.UShortFromDoubleClamped(0.239216), Col.UShortFromDoubleClamped(0.545098));
        public static C4us DarkSlateGray => new C4us(Col.UShortFromDoubleClamped(0.184314), Col.UShortFromDoubleClamped(0.309804), Col.UShortFromDoubleClamped(0.309804));
        public static C4us DarkSlateGrey => new C4us(Col.UShortFromDoubleClamped(0.184314), Col.UShortFromDoubleClamped(0.309804), Col.UShortFromDoubleClamped(0.309804));
        public static C4us DarkTurquoise => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.807843), Col.UShortFromDoubleClamped(0.819608));
        public static C4us DarkViolet => new C4us(Col.UShortFromDoubleClamped(0.580392), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.827451));
        public static C4us DeepPink => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.078431), Col.UShortFromDoubleClamped(0.576471));
        public static C4us DeepSkyBlue => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.74902), Col.UShortFromDoubleClamped(1));
        public static C4us DimGray => new C4us(Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765));
        public static C4us DimGrey => new C4us(Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.411765));
        public static C4us DodgerBlue => new C4us(Col.UShortFromDoubleClamped(0.117647), Col.UShortFromDoubleClamped(0.564706), Col.UShortFromDoubleClamped(1));
        public static C4us FireBrick => new C4us(Col.UShortFromDoubleClamped(0.698039), Col.UShortFromDoubleClamped(0.133333), Col.UShortFromDoubleClamped(0.133333));
        public static C4us FloralWhite => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.941176));
        public static C4us ForestGreen => new C4us(Col.UShortFromDoubleClamped(0.133333), Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.133333));
        public static C4us Fuchsia => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1));
        public static C4us Gainsboro => new C4us(Col.UShortFromDoubleClamped(0.862745), Col.UShortFromDoubleClamped(0.862745), Col.UShortFromDoubleClamped(0.862745));
        public static C4us GhostWhite => new C4us(Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(0.972549), Col.UShortFromDoubleClamped(1));
        public static C4us Gold => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.843137), Col.UShortFromDoubleClamped(0));
        public static C4us GoldenRod => new C4us(Col.UShortFromDoubleClamped(0.854902), Col.UShortFromDoubleClamped(0.647059), Col.UShortFromDoubleClamped(0.12549));
        public static C4us Gray => new C4us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C4us Grey => new C4us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C4us Green => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0));
        public static C4us GreenYellow => new C4us(Col.UShortFromDoubleClamped(0.678431), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.184314));
        public static C4us HoneyDew => new C4us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.941176));
        public static C4us HotPink => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.705882));
        public static C4us IndianRed  => new C4us(Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.360784), Col.UShortFromDoubleClamped(0.360784));
        public static C4us Indigo  => new C4us(Col.UShortFromDoubleClamped(0.294118), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.509804));
        public static C4us Ivory => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.941176));
        public static C4us Khaki => new C4us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.901961), Col.UShortFromDoubleClamped(0.54902));
        public static C4us Lavender => new C4us(Col.UShortFromDoubleClamped(0.901961), Col.UShortFromDoubleClamped(0.901961), Col.UShortFromDoubleClamped(0.980392));
        public static C4us LavenderBlush => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.960784));
        public static C4us LawnGreen => new C4us(Col.UShortFromDoubleClamped(0.486275), Col.UShortFromDoubleClamped(0.988235), Col.UShortFromDoubleClamped(0));
        public static C4us LemonChiffon => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.803922));
        public static C4us LightBlue => new C4us(Col.UShortFromDoubleClamped(0.678431), Col.UShortFromDoubleClamped(0.847059), Col.UShortFromDoubleClamped(0.901961));
        public static C4us LightCoral => new C4us(Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C4us LightCyan => new C4us(Col.UShortFromDoubleClamped(0.878431), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C4us LightGoldenRodYellow => new C4us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.823529));
        public static C4us LightGray => new C4us(Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451));
        public static C4us LightGrey => new C4us(Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451), Col.UShortFromDoubleClamped(0.827451));
        public static C4us LightGreen => new C4us(Col.UShortFromDoubleClamped(0.564706), Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.564706));
        public static C4us LightPink => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.713725), Col.UShortFromDoubleClamped(0.756863));
        public static C4us LightSalmon => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.627451), Col.UShortFromDoubleClamped(0.478431));
        public static C4us LightSeaGreen => new C4us(Col.UShortFromDoubleClamped(0.12549), Col.UShortFromDoubleClamped(0.698039), Col.UShortFromDoubleClamped(0.666667));
        public static C4us LightSkyBlue => new C4us(Col.UShortFromDoubleClamped(0.529412), Col.UShortFromDoubleClamped(0.807843), Col.UShortFromDoubleClamped(0.980392));
        public static C4us LightSlateGray => new C4us(Col.UShortFromDoubleClamped(0.466667), Col.UShortFromDoubleClamped(0.533333), Col.UShortFromDoubleClamped(0.6));
        public static C4us LightSlateGrey => new C4us(Col.UShortFromDoubleClamped(0.466667), Col.UShortFromDoubleClamped(0.533333), Col.UShortFromDoubleClamped(0.6));
        public static C4us LightSteelBlue => new C4us(Col.UShortFromDoubleClamped(0.690196), Col.UShortFromDoubleClamped(0.768627), Col.UShortFromDoubleClamped(0.870588));
        public static C4us LightYellow => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.878431));
        public static C4us Lime => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0));
        public static C4us LimeGreen => new C4us(Col.UShortFromDoubleClamped(0.196078), Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.196078));
        public static C4us Linen => new C4us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.941176), Col.UShortFromDoubleClamped(0.901961));
        public static C4us Magenta => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1));
        public static C4us Maroon => new C4us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C4us MediumAquaMarine => new C4us(Col.UShortFromDoubleClamped(0.4), Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.666667));
        public static C4us MediumBlue => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.803922));
        public static C4us MediumOrchid => new C4us(Col.UShortFromDoubleClamped(0.729412), Col.UShortFromDoubleClamped(0.333333), Col.UShortFromDoubleClamped(0.827451));
        public static C4us MediumPurple => new C4us(Col.UShortFromDoubleClamped(0.576471), Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.847059));
        public static C4us MediumSeaGreen => new C4us(Col.UShortFromDoubleClamped(0.235294), Col.UShortFromDoubleClamped(0.701961), Col.UShortFromDoubleClamped(0.443137));
        public static C4us MediumSlateBlue => new C4us(Col.UShortFromDoubleClamped(0.482353), Col.UShortFromDoubleClamped(0.407843), Col.UShortFromDoubleClamped(0.933333));
        public static C4us MediumSpringGreen => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.603922));
        public static C4us MediumTurquoise => new C4us(Col.UShortFromDoubleClamped(0.282353), Col.UShortFromDoubleClamped(0.819608), Col.UShortFromDoubleClamped(0.8));
        public static C4us MediumVioletRed => new C4us(Col.UShortFromDoubleClamped(0.780392), Col.UShortFromDoubleClamped(0.082353), Col.UShortFromDoubleClamped(0.521569));
        public static C4us MidnightBlue => new C4us(Col.UShortFromDoubleClamped(0.098039), Col.UShortFromDoubleClamped(0.098039), Col.UShortFromDoubleClamped(0.439216));
        public static C4us MintCream => new C4us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392));
        public static C4us MistyRose => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.894118), Col.UShortFromDoubleClamped(0.882353));
        public static C4us Moccasin => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.894118), Col.UShortFromDoubleClamped(0.709804));
        public static C4us NavajoWhite => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.870588), Col.UShortFromDoubleClamped(0.678431));
        public static C4us Navy => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961));
        public static C4us OldLace => new C4us(Col.UShortFromDoubleClamped(0.992157), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.901961));
        public static C4us Olive => new C4us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0));
        public static C4us OliveDrab => new C4us(Col.UShortFromDoubleClamped(0.419608), Col.UShortFromDoubleClamped(0.556863), Col.UShortFromDoubleClamped(0.137255));
        public static C4us Orange => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.647059), Col.UShortFromDoubleClamped(0));
        public static C4us OrangeRed => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.270588), Col.UShortFromDoubleClamped(0));
        public static C4us Orchid => new C4us(Col.UShortFromDoubleClamped(0.854902), Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.839216));
        public static C4us PaleGoldenRod => new C4us(Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.909804), Col.UShortFromDoubleClamped(0.666667));
        public static C4us PaleGreen => new C4us(Col.UShortFromDoubleClamped(0.596078), Col.UShortFromDoubleClamped(0.984314), Col.UShortFromDoubleClamped(0.596078));
        public static C4us PaleTurquoise => new C4us(Col.UShortFromDoubleClamped(0.686275), Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.933333));
        public static C4us PaleVioletRed => new C4us(Col.UShortFromDoubleClamped(0.847059), Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.576471));
        public static C4us PapayaWhip => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.937255), Col.UShortFromDoubleClamped(0.835294));
        public static C4us PeachPuff => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.854902), Col.UShortFromDoubleClamped(0.72549));
        public static C4us Peru => new C4us(Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.521569), Col.UShortFromDoubleClamped(0.247059));
        public static C4us Pink => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.752941), Col.UShortFromDoubleClamped(0.796078));
        public static C4us Plum => new C4us(Col.UShortFromDoubleClamped(0.866667), Col.UShortFromDoubleClamped(0.627451), Col.UShortFromDoubleClamped(0.866667));
        public static C4us PowderBlue => new C4us(Col.UShortFromDoubleClamped(0.690196), Col.UShortFromDoubleClamped(0.878431), Col.UShortFromDoubleClamped(0.901961));
        public static C4us Purple => new C4us(Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961));
        public static C4us Red => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0));
        public static C4us RosyBrown => new C4us(Col.UShortFromDoubleClamped(0.737255), Col.UShortFromDoubleClamped(0.560784), Col.UShortFromDoubleClamped(0.560784));
        public static C4us RoyalBlue => new C4us(Col.UShortFromDoubleClamped(0.254902), Col.UShortFromDoubleClamped(0.411765), Col.UShortFromDoubleClamped(0.882353));
        public static C4us SaddleBrown => new C4us(Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.270588), Col.UShortFromDoubleClamped(0.07451));
        public static C4us Salmon => new C4us(Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.447059));
        public static C4us SandyBrown => new C4us(Col.UShortFromDoubleClamped(0.956863), Col.UShortFromDoubleClamped(0.643137), Col.UShortFromDoubleClamped(0.376471));
        public static C4us SeaGreen => new C4us(Col.UShortFromDoubleClamped(0.180392), Col.UShortFromDoubleClamped(0.545098), Col.UShortFromDoubleClamped(0.341176));
        public static C4us SeaShell => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.933333));
        public static C4us Sienna => new C4us(Col.UShortFromDoubleClamped(0.627451), Col.UShortFromDoubleClamped(0.321569), Col.UShortFromDoubleClamped(0.176471));
        public static C4us Silver => new C4us(Col.UShortFromDoubleClamped(0.752941), Col.UShortFromDoubleClamped(0.752941), Col.UShortFromDoubleClamped(0.752941));
        public static C4us SkyBlue => new C4us(Col.UShortFromDoubleClamped(0.529412), Col.UShortFromDoubleClamped(0.807843), Col.UShortFromDoubleClamped(0.921569));
        public static C4us SlateBlue => new C4us(Col.UShortFromDoubleClamped(0.415686), Col.UShortFromDoubleClamped(0.352941), Col.UShortFromDoubleClamped(0.803922));
        public static C4us SlateGray => new C4us(Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.564706));
        public static C4us SlateGrey => new C4us(Col.UShortFromDoubleClamped(0.439216), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.564706));
        public static C4us Snow => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.980392), Col.UShortFromDoubleClamped(0.980392));
        public static C4us SpringGreen => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.498039));
        public static C4us SteelBlue => new C4us(Col.UShortFromDoubleClamped(0.27451), Col.UShortFromDoubleClamped(0.509804), Col.UShortFromDoubleClamped(0.705882));
        public static C4us Tan => new C4us(Col.UShortFromDoubleClamped(0.823529), Col.UShortFromDoubleClamped(0.705882), Col.UShortFromDoubleClamped(0.54902));
        public static C4us Teal => new C4us(Col.UShortFromDoubleClamped(0), Col.UShortFromDoubleClamped(0.501961), Col.UShortFromDoubleClamped(0.501961));
        public static C4us Thistle => new C4us(Col.UShortFromDoubleClamped(0.847059), Col.UShortFromDoubleClamped(0.74902), Col.UShortFromDoubleClamped(0.847059));
        public static C4us Tomato => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0.388235), Col.UShortFromDoubleClamped(0.278431));
        public static C4us Turquoise => new C4us(Col.UShortFromDoubleClamped(0.25098), Col.UShortFromDoubleClamped(0.878431), Col.UShortFromDoubleClamped(0.815686));
        public static C4us Violet => new C4us(Col.UShortFromDoubleClamped(0.933333), Col.UShortFromDoubleClamped(0.509804), Col.UShortFromDoubleClamped(0.933333));
        public static C4us Wheat => new C4us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.870588), Col.UShortFromDoubleClamped(0.701961));
        public static C4us White => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1));
        public static C4us WhiteSmoke => new C4us(Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.960784), Col.UShortFromDoubleClamped(0.960784));
        public static C4us Yellow => new C4us(Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(1), Col.UShortFromDoubleClamped(0));
        public static C4us YellowGreen => new C4us(Col.UShortFromDoubleClamped(0.603922), Col.UShortFromDoubleClamped(0.803922), Col.UShortFromDoubleClamped(0.196078));

        public static C4us DarkYellow => Olive;

        public static C4us VRVisGreen => new C4us(Col.UShortFromDoubleClamped(0.698), Col.UShortFromDoubleClamped(0.851), Col.UShortFromDoubleClamped(0.008));

        public static C4us Gray10 => new C4us(Col.UShortFromDoubleClamped(0.1));
        public static C4us Gray20 => new C4us(Col.UShortFromDoubleClamped(0.2));
        public static C4us Gray30 => new C4us(Col.UShortFromDoubleClamped(0.3));
        public static C4us Gray40 => new C4us(Col.UShortFromDoubleClamped(0.4));
        public static C4us Gray50 => new C4us(Col.UShortFromDoubleClamped(0.5));
        public static C4us Gray60 => new C4us(Col.UShortFromDoubleClamped(0.6));
        public static C4us Gray70 => new C4us(Col.UShortFromDoubleClamped(0.7));
        public static C4us Gray80 => new C4us(Col.UShortFromDoubleClamped(0.8));
        public static C4us Gray90 => new C4us(Col.UShortFromDoubleClamped(0.9));

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

        public static C4us operator *(C4us col, double scalar)
        {
            return new C4us(
                (ushort)(col.R * scalar), 
                (ushort)(col.G * scalar), 
                (ushort)(col.B * scalar), 
                (ushort)(col.A * scalar));
        }

        public static C4us operator *(double scalar, C4us col)
        {
            return new C4us(
                (ushort)(scalar * col.R), 
                (ushort)(scalar * col.G), 
                (ushort)(scalar * col.B), 
                (ushort)(scalar * col.A));
        }

        public static C4us operator /(C4us col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C4us(
                (ushort)(col.R * f), 
                (ushort)(col.G * f), 
                (ushort)(col.B * f), 
                (ushort)(col.A * f));
        }

        public static C4us operator +(C4us c0, C4b c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G + Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B + Col.UShortFromByte(c1.B)), 
                (ushort)(c0.A + Col.UShortFromByte(c1.A)));
        }

        public static C4us operator -(C4us c0, C4b c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromByte(c1.R)), 
                (ushort)(c0.G - Col.UShortFromByte(c1.G)), 
                (ushort)(c0.B - Col.UShortFromByte(c1.B)), 
                (ushort)(c0.A - Col.UShortFromByte(c1.A)));
        }

        public static C4us operator +(C4us c0, C4us c1)
        {
            return new C4us(
                (ushort)(c0.R + (c1.R)), 
                (ushort)(c0.G + (c1.G)), 
                (ushort)(c0.B + (c1.B)), 
                (ushort)(c0.A + (c1.A)));
        }

        public static C4us operator -(C4us c0, C4us c1)
        {
            return new C4us(
                (ushort)(c0.R - (c1.R)), 
                (ushort)(c0.G - (c1.G)), 
                (ushort)(c0.B - (c1.B)), 
                (ushort)(c0.A - (c1.A)));
        }

        public static C4us operator +(C4us c0, C4ui c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G + Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B + Col.UShortFromUInt(c1.B)), 
                (ushort)(c0.A + Col.UShortFromUInt(c1.A)));
        }

        public static C4us operator -(C4us c0, C4ui c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromUInt(c1.R)), 
                (ushort)(c0.G - Col.UShortFromUInt(c1.G)), 
                (ushort)(c0.B - Col.UShortFromUInt(c1.B)), 
                (ushort)(c0.A - Col.UShortFromUInt(c1.A)));
        }

        public static C4us operator +(C4us c0, C4f c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G + Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B + Col.UShortFromFloat(c1.B)), 
                (ushort)(c0.A + Col.UShortFromFloat(c1.A)));
        }

        public static C4us operator -(C4us c0, C4f c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromFloat(c1.R)), 
                (ushort)(c0.G - Col.UShortFromFloat(c1.G)), 
                (ushort)(c0.B - Col.UShortFromFloat(c1.B)), 
                (ushort)(c0.A - Col.UShortFromFloat(c1.A)));
        }

        public static C4us operator +(C4us c0, C4d c1)
        {
            return new C4us(
                (ushort)(c0.R + Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G + Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B + Col.UShortFromDouble(c1.B)), 
                (ushort)(c0.A + Col.UShortFromDouble(c1.A)));
        }

        public static C4us operator -(C4us c0, C4d c1)
        {
            return new C4us(
                (ushort)(c0.R - Col.UShortFromDouble(c1.R)), 
                (ushort)(c0.G - Col.UShortFromDouble(c1.G)), 
                (ushort)(c0.B - Col.UShortFromDouble(c1.B)), 
                (ushort)(c0.A - Col.UShortFromDouble(c1.A)));
        }

        public static V3i operator + (V3i vec, C4us color)
        {
            return new V3i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B)
                );
        }

        public static V3i operator -(V3i vec, C4us color)
        {
            return new V3i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B)
                );
        }

        public static V3l operator + (V3l vec, C4us color)
        {
            return new V3l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B)
                );
        }

        public static V3l operator -(V3l vec, C4us color)
        {
            return new V3l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B)
                );
        }

        public static V4i operator + (V4i vec, C4us color)
        {
            return new V4i(
                vec.X + (int)(color.R), 
                vec.Y + (int)(color.G), 
                vec.Z + (int)(color.B),
                vec.W + (int)(color.A)
                );
        }

        public static V4i operator -(V4i vec, C4us color)
        {
            return new V4i(
                vec.X - (int)(color.R), 
                vec.Y - (int)(color.G), 
                vec.Z - (int)(color.B),
                vec.W - (int)(color.A)
                );
        }

        public static V4l operator + (V4l vec, C4us color)
        {
            return new V4l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B),
                vec.W + (long)(color.A)
                );
        }

        public static V4l operator -(V4l vec, C4us color)
        {
            return new V4l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B),
                vec.W - (long)(color.A)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(ushort min, ushort max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4us Clamped(ushort min, ushort max)
        {
            return new C4us(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp(Col.UShortFromDoubleClamped(min), Col.UShortFromDoubleClamped(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4us Clamped(double min, double max)
        {
            return Clamped(Col.UShortFromDoubleClamped(min), Col.UShortFromDoubleClamped(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public int Norm1
        {
            get { return (int)R + (int)G + (int)B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public ushort NormMax
        {
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public ushort NormMin
        {
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
        public static bool ApproximateEquals(this C4us a, C4us b)
        {
            return ApproximateEquals(a, b, Constant<ushort>.PositiveTinyValue);
        }

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

    [Serializable]
    public partial struct C4ui : IFormattable, IEquatable<C4ui>, IRGB, IOpacity
    {
        #region Constructors

        public C4ui(uint r, uint g, uint b, uint a)
        {
            R = r; G = g; B = b; A = a;
        }

        public C4ui(int r, int g, int b, int a)
        {
            R = (uint)r; G = (uint)g; B = (uint)b; A = (uint)a;
        }

        public C4ui(long r, long g, long b, long a)
        {
            R = (uint)r; G = (uint)g; B = (uint)b; A = (uint)a;
        }

        public C4ui(double r, double g, double b, double a)
        {
            R = Col.UIntFromDoubleClamped(r);
            G = Col.UIntFromDoubleClamped(g);
            B = Col.UIntFromDoubleClamped(b);
            A = Col.UIntFromDoubleClamped(a);
        }

        public C4ui(uint r, uint g, uint b)
        {
            R = r; G = g; B = b;
            A = UInt32.MaxValue;
        }

        public C4ui(int r, int g, int b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
            A = UInt32.MaxValue;
        }

        public C4ui(long r, long g, long b)
        {
            R = (uint)r; G = (uint)g; B = (uint)b;
            A = UInt32.MaxValue;
        }

        public C4ui(double r, double g, double b)
        {
            R = Col.UIntFromDoubleClamped(r); G = Col.UIntFromDoubleClamped(g); B = Col.UIntFromDoubleClamped(b);
            A = UInt32.MaxValue;
        }

        public C4ui(uint gray)
        {
            R = gray; G = gray; B = gray; A = UInt32.MaxValue;
        }

        public C4ui(double gray)
        {
            var value = Col.UIntFromDoubleClamped(gray);
            R = value; G = value; B = value; A = UInt32.MaxValue;
        }

        public C4ui(C3b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3b color, uint alpha)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
            A = alpha;
        }

        public C4ui(C3us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3us color, uint alpha)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
            A = alpha;
        }

        public C4ui(C3ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3ui color, uint alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        public C4ui(C3f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3f color, uint alpha)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
            A = alpha;
        }

        public C4ui(C3d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3d color, uint alpha)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
            A = alpha;
        }

        public C4ui(C4b color)
        {
            R = Col.UIntFromByte(color.R);
            G = Col.UIntFromByte(color.G);
            B = Col.UIntFromByte(color.B);
            A = Col.UIntFromByte(color.A);
        }

        public C4ui(C4us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
            A = Col.UIntFromUShort(color.A);
        }

        public C4ui(C4ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        public C4ui(C4f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
            A = Col.UIntFromFloat(color.A);
        }

        public C4ui(C4d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
            A = Col.UIntFromDouble(color.A);
        }

        public C4ui(V3l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = UInt32.MaxValue;
        }

        public C4ui(V3l vec, uint alpha)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = alpha;
        }

        public C4ui(V4l vec)
        {
            R = (uint)(vec.X);
            G = (uint)(vec.Y);
            B = (uint)(vec.Z);
            A = (uint)(vec.W);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C4ui color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C4ui color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C4ui color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C4ui color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C4ui color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C4ui color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C4ui color)
        {
            return new C4us(color);
        }

        public static explicit operator C4f(C4ui color)
        {
            return new C4f(color);
        }

        public static explicit operator C4d(C4ui color)
        {
            return new C4d(color);
        }

        public static explicit operator V3l(C4ui color)
        {
            return new V3l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B)
                );
        }

        public static explicit operator V4l(C4ui color)
        {
            return new V4l(
                (long)(color.R), 
                (long)(color.G), 
                (long)(color.B),
                (long)(color.A)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4f ToC4f() { return (C4f)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C4ui(Func<int, uint> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        public V3l ToV3l() { return (V3l)this; }
        public V4l ToV4l() { return (V4l)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3b(C3b c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3us(C3us c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3ui(C3ui c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3f(C3f c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC3d(C3d c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4b(C4b c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4us(C4us c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4f(C4f c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromC4d(C4d c)
            => new C4ui(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV3l(V3l c)
            => new C4ui(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4ui FromV4l(V4l c)
            => new C4ui(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4b Copy(Func<uint, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4us Copy(Func<uint, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4ui Copy(Func<uint, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4f Copy(Func<uint, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4d Copy(Func<uint, double> channel_fun)
        {
            return Map(channel_fun);
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
        public uint this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    case 3:
                        A = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    case 3:
                        return A;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C4ui with all components zero.
        /// </summary>
        public static C4ui Zero => new C4ui(0, 0, 0, 0);

        // Web colors
        public static C4ui AliceBlue => new C4ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(1));
        public static C4ui AntiqueWhite => new C4ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.921569), Col.UIntFromDoubleClamped(0.843137));
        public static C4ui Aqua => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C4ui Aquamarine => new C4ui(Col.UIntFromDoubleClamped(0.498039), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.831373));
        public static C4ui Azure => new C4ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C4ui Beige => new C4ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.862745));
        public static C4ui Bisque => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.894118), Col.UIntFromDoubleClamped(0.768627));
        public static C4ui Black => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C4ui BlanchedAlmond => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.921569), Col.UIntFromDoubleClamped(0.803922));
        public static C4ui Blue => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1));
        public static C4ui BlueViolet => new C4ui(Col.UIntFromDoubleClamped(0.541176), Col.UIntFromDoubleClamped(0.168627), Col.UIntFromDoubleClamped(0.886275));
        public static C4ui Brown => new C4ui(Col.UIntFromDoubleClamped(0.647059), Col.UIntFromDoubleClamped(0.164706), Col.UIntFromDoubleClamped(0.164706));
        public static C4ui BurlyWood => new C4ui(Col.UIntFromDoubleClamped(0.870588), Col.UIntFromDoubleClamped(0.721569), Col.UIntFromDoubleClamped(0.529412));
        public static C4ui CadetBlue => new C4ui(Col.UIntFromDoubleClamped(0.372549), Col.UIntFromDoubleClamped(0.619608), Col.UIntFromDoubleClamped(0.627451));
        public static C4ui Chartreuse => new C4ui(Col.UIntFromDoubleClamped(0.498039), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0));
        public static C4ui Chocolate => new C4ui(Col.UIntFromDoubleClamped(0.823529), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.117647));
        public static C4ui Coral => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.498039), Col.UIntFromDoubleClamped(0.313725));
        public static C4ui CornflowerBlue => new C4ui(Col.UIntFromDoubleClamped(0.392157), Col.UIntFromDoubleClamped(0.584314), Col.UIntFromDoubleClamped(0.929412));
        public static C4ui Cornsilk => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(0.862745));
        public static C4ui Crimson => new C4ui(Col.UIntFromDoubleClamped(0.862745), Col.UIntFromDoubleClamped(0.078431), Col.UIntFromDoubleClamped(0.235294));
        public static C4ui Cyan => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C4ui DarkBlue => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.545098));
        public static C4ui DarkCyan => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.545098));
        public static C4ui DarkGoldenRod => new C4ui(Col.UIntFromDoubleClamped(0.721569), Col.UIntFromDoubleClamped(0.52549), Col.UIntFromDoubleClamped(0.043137));
        public static C4ui DarkGray => new C4ui(Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745));
        public static C4ui DarkGrey => new C4ui(Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745), Col.UIntFromDoubleClamped(0.662745));
        public static C4ui DarkGreen => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.392157), Col.UIntFromDoubleClamped(0));
        public static C4ui DarkKhaki => new C4ui(Col.UIntFromDoubleClamped(0.741176), Col.UIntFromDoubleClamped(0.717647), Col.UIntFromDoubleClamped(0.419608));
        public static C4ui DarkMagenta => new C4ui(Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.545098));
        public static C4ui DarkOliveGreen => new C4ui(Col.UIntFromDoubleClamped(0.333333), Col.UIntFromDoubleClamped(0.419608), Col.UIntFromDoubleClamped(0.184314));
        public static C4ui DarkOrange => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.54902), Col.UIntFromDoubleClamped(0));
        public static C4ui DarkOrchid => new C4ui(Col.UIntFromDoubleClamped(0.6), Col.UIntFromDoubleClamped(0.196078), Col.UIntFromDoubleClamped(0.8));
        public static C4ui DarkRed => new C4ui(Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C4ui DarkSalmon => new C4ui(Col.UIntFromDoubleClamped(0.913725), Col.UIntFromDoubleClamped(0.588235), Col.UIntFromDoubleClamped(0.478431));
        public static C4ui DarkSeaGreen => new C4ui(Col.UIntFromDoubleClamped(0.560784), Col.UIntFromDoubleClamped(0.737255), Col.UIntFromDoubleClamped(0.560784));
        public static C4ui DarkSlateBlue => new C4ui(Col.UIntFromDoubleClamped(0.282353), Col.UIntFromDoubleClamped(0.239216), Col.UIntFromDoubleClamped(0.545098));
        public static C4ui DarkSlateGray => new C4ui(Col.UIntFromDoubleClamped(0.184314), Col.UIntFromDoubleClamped(0.309804), Col.UIntFromDoubleClamped(0.309804));
        public static C4ui DarkSlateGrey => new C4ui(Col.UIntFromDoubleClamped(0.184314), Col.UIntFromDoubleClamped(0.309804), Col.UIntFromDoubleClamped(0.309804));
        public static C4ui DarkTurquoise => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.807843), Col.UIntFromDoubleClamped(0.819608));
        public static C4ui DarkViolet => new C4ui(Col.UIntFromDoubleClamped(0.580392), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.827451));
        public static C4ui DeepPink => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.078431), Col.UIntFromDoubleClamped(0.576471));
        public static C4ui DeepSkyBlue => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.74902), Col.UIntFromDoubleClamped(1));
        public static C4ui DimGray => new C4ui(Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765));
        public static C4ui DimGrey => new C4ui(Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.411765));
        public static C4ui DodgerBlue => new C4ui(Col.UIntFromDoubleClamped(0.117647), Col.UIntFromDoubleClamped(0.564706), Col.UIntFromDoubleClamped(1));
        public static C4ui FireBrick => new C4ui(Col.UIntFromDoubleClamped(0.698039), Col.UIntFromDoubleClamped(0.133333), Col.UIntFromDoubleClamped(0.133333));
        public static C4ui FloralWhite => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.941176));
        public static C4ui ForestGreen => new C4ui(Col.UIntFromDoubleClamped(0.133333), Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.133333));
        public static C4ui Fuchsia => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1));
        public static C4ui Gainsboro => new C4ui(Col.UIntFromDoubleClamped(0.862745), Col.UIntFromDoubleClamped(0.862745), Col.UIntFromDoubleClamped(0.862745));
        public static C4ui GhostWhite => new C4ui(Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(0.972549), Col.UIntFromDoubleClamped(1));
        public static C4ui Gold => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.843137), Col.UIntFromDoubleClamped(0));
        public static C4ui GoldenRod => new C4ui(Col.UIntFromDoubleClamped(0.854902), Col.UIntFromDoubleClamped(0.647059), Col.UIntFromDoubleClamped(0.12549));
        public static C4ui Gray => new C4ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C4ui Grey => new C4ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C4ui Green => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0));
        public static C4ui GreenYellow => new C4ui(Col.UIntFromDoubleClamped(0.678431), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.184314));
        public static C4ui HoneyDew => new C4ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.941176));
        public static C4ui HotPink => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.705882));
        public static C4ui IndianRed  => new C4ui(Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.360784), Col.UIntFromDoubleClamped(0.360784));
        public static C4ui Indigo  => new C4ui(Col.UIntFromDoubleClamped(0.294118), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.509804));
        public static C4ui Ivory => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.941176));
        public static C4ui Khaki => new C4ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.901961), Col.UIntFromDoubleClamped(0.54902));
        public static C4ui Lavender => new C4ui(Col.UIntFromDoubleClamped(0.901961), Col.UIntFromDoubleClamped(0.901961), Col.UIntFromDoubleClamped(0.980392));
        public static C4ui LavenderBlush => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.960784));
        public static C4ui LawnGreen => new C4ui(Col.UIntFromDoubleClamped(0.486275), Col.UIntFromDoubleClamped(0.988235), Col.UIntFromDoubleClamped(0));
        public static C4ui LemonChiffon => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.803922));
        public static C4ui LightBlue => new C4ui(Col.UIntFromDoubleClamped(0.678431), Col.UIntFromDoubleClamped(0.847059), Col.UIntFromDoubleClamped(0.901961));
        public static C4ui LightCoral => new C4ui(Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C4ui LightCyan => new C4ui(Col.UIntFromDoubleClamped(0.878431), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C4ui LightGoldenRodYellow => new C4ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.823529));
        public static C4ui LightGray => new C4ui(Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451));
        public static C4ui LightGrey => new C4ui(Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451), Col.UIntFromDoubleClamped(0.827451));
        public static C4ui LightGreen => new C4ui(Col.UIntFromDoubleClamped(0.564706), Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.564706));
        public static C4ui LightPink => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.713725), Col.UIntFromDoubleClamped(0.756863));
        public static C4ui LightSalmon => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.627451), Col.UIntFromDoubleClamped(0.478431));
        public static C4ui LightSeaGreen => new C4ui(Col.UIntFromDoubleClamped(0.12549), Col.UIntFromDoubleClamped(0.698039), Col.UIntFromDoubleClamped(0.666667));
        public static C4ui LightSkyBlue => new C4ui(Col.UIntFromDoubleClamped(0.529412), Col.UIntFromDoubleClamped(0.807843), Col.UIntFromDoubleClamped(0.980392));
        public static C4ui LightSlateGray => new C4ui(Col.UIntFromDoubleClamped(0.466667), Col.UIntFromDoubleClamped(0.533333), Col.UIntFromDoubleClamped(0.6));
        public static C4ui LightSlateGrey => new C4ui(Col.UIntFromDoubleClamped(0.466667), Col.UIntFromDoubleClamped(0.533333), Col.UIntFromDoubleClamped(0.6));
        public static C4ui LightSteelBlue => new C4ui(Col.UIntFromDoubleClamped(0.690196), Col.UIntFromDoubleClamped(0.768627), Col.UIntFromDoubleClamped(0.870588));
        public static C4ui LightYellow => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.878431));
        public static C4ui Lime => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0));
        public static C4ui LimeGreen => new C4ui(Col.UIntFromDoubleClamped(0.196078), Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.196078));
        public static C4ui Linen => new C4ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.941176), Col.UIntFromDoubleClamped(0.901961));
        public static C4ui Magenta => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1));
        public static C4ui Maroon => new C4ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C4ui MediumAquaMarine => new C4ui(Col.UIntFromDoubleClamped(0.4), Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.666667));
        public static C4ui MediumBlue => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.803922));
        public static C4ui MediumOrchid => new C4ui(Col.UIntFromDoubleClamped(0.729412), Col.UIntFromDoubleClamped(0.333333), Col.UIntFromDoubleClamped(0.827451));
        public static C4ui MediumPurple => new C4ui(Col.UIntFromDoubleClamped(0.576471), Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.847059));
        public static C4ui MediumSeaGreen => new C4ui(Col.UIntFromDoubleClamped(0.235294), Col.UIntFromDoubleClamped(0.701961), Col.UIntFromDoubleClamped(0.443137));
        public static C4ui MediumSlateBlue => new C4ui(Col.UIntFromDoubleClamped(0.482353), Col.UIntFromDoubleClamped(0.407843), Col.UIntFromDoubleClamped(0.933333));
        public static C4ui MediumSpringGreen => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.603922));
        public static C4ui MediumTurquoise => new C4ui(Col.UIntFromDoubleClamped(0.282353), Col.UIntFromDoubleClamped(0.819608), Col.UIntFromDoubleClamped(0.8));
        public static C4ui MediumVioletRed => new C4ui(Col.UIntFromDoubleClamped(0.780392), Col.UIntFromDoubleClamped(0.082353), Col.UIntFromDoubleClamped(0.521569));
        public static C4ui MidnightBlue => new C4ui(Col.UIntFromDoubleClamped(0.098039), Col.UIntFromDoubleClamped(0.098039), Col.UIntFromDoubleClamped(0.439216));
        public static C4ui MintCream => new C4ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392));
        public static C4ui MistyRose => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.894118), Col.UIntFromDoubleClamped(0.882353));
        public static C4ui Moccasin => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.894118), Col.UIntFromDoubleClamped(0.709804));
        public static C4ui NavajoWhite => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.870588), Col.UIntFromDoubleClamped(0.678431));
        public static C4ui Navy => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961));
        public static C4ui OldLace => new C4ui(Col.UIntFromDoubleClamped(0.992157), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.901961));
        public static C4ui Olive => new C4ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0));
        public static C4ui OliveDrab => new C4ui(Col.UIntFromDoubleClamped(0.419608), Col.UIntFromDoubleClamped(0.556863), Col.UIntFromDoubleClamped(0.137255));
        public static C4ui Orange => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.647059), Col.UIntFromDoubleClamped(0));
        public static C4ui OrangeRed => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.270588), Col.UIntFromDoubleClamped(0));
        public static C4ui Orchid => new C4ui(Col.UIntFromDoubleClamped(0.854902), Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.839216));
        public static C4ui PaleGoldenRod => new C4ui(Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.909804), Col.UIntFromDoubleClamped(0.666667));
        public static C4ui PaleGreen => new C4ui(Col.UIntFromDoubleClamped(0.596078), Col.UIntFromDoubleClamped(0.984314), Col.UIntFromDoubleClamped(0.596078));
        public static C4ui PaleTurquoise => new C4ui(Col.UIntFromDoubleClamped(0.686275), Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.933333));
        public static C4ui PaleVioletRed => new C4ui(Col.UIntFromDoubleClamped(0.847059), Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.576471));
        public static C4ui PapayaWhip => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.937255), Col.UIntFromDoubleClamped(0.835294));
        public static C4ui PeachPuff => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.854902), Col.UIntFromDoubleClamped(0.72549));
        public static C4ui Peru => new C4ui(Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.521569), Col.UIntFromDoubleClamped(0.247059));
        public static C4ui Pink => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.752941), Col.UIntFromDoubleClamped(0.796078));
        public static C4ui Plum => new C4ui(Col.UIntFromDoubleClamped(0.866667), Col.UIntFromDoubleClamped(0.627451), Col.UIntFromDoubleClamped(0.866667));
        public static C4ui PowderBlue => new C4ui(Col.UIntFromDoubleClamped(0.690196), Col.UIntFromDoubleClamped(0.878431), Col.UIntFromDoubleClamped(0.901961));
        public static C4ui Purple => new C4ui(Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961));
        public static C4ui Red => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0));
        public static C4ui RosyBrown => new C4ui(Col.UIntFromDoubleClamped(0.737255), Col.UIntFromDoubleClamped(0.560784), Col.UIntFromDoubleClamped(0.560784));
        public static C4ui RoyalBlue => new C4ui(Col.UIntFromDoubleClamped(0.254902), Col.UIntFromDoubleClamped(0.411765), Col.UIntFromDoubleClamped(0.882353));
        public static C4ui SaddleBrown => new C4ui(Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.270588), Col.UIntFromDoubleClamped(0.07451));
        public static C4ui Salmon => new C4ui(Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.447059));
        public static C4ui SandyBrown => new C4ui(Col.UIntFromDoubleClamped(0.956863), Col.UIntFromDoubleClamped(0.643137), Col.UIntFromDoubleClamped(0.376471));
        public static C4ui SeaGreen => new C4ui(Col.UIntFromDoubleClamped(0.180392), Col.UIntFromDoubleClamped(0.545098), Col.UIntFromDoubleClamped(0.341176));
        public static C4ui SeaShell => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.933333));
        public static C4ui Sienna => new C4ui(Col.UIntFromDoubleClamped(0.627451), Col.UIntFromDoubleClamped(0.321569), Col.UIntFromDoubleClamped(0.176471));
        public static C4ui Silver => new C4ui(Col.UIntFromDoubleClamped(0.752941), Col.UIntFromDoubleClamped(0.752941), Col.UIntFromDoubleClamped(0.752941));
        public static C4ui SkyBlue => new C4ui(Col.UIntFromDoubleClamped(0.529412), Col.UIntFromDoubleClamped(0.807843), Col.UIntFromDoubleClamped(0.921569));
        public static C4ui SlateBlue => new C4ui(Col.UIntFromDoubleClamped(0.415686), Col.UIntFromDoubleClamped(0.352941), Col.UIntFromDoubleClamped(0.803922));
        public static C4ui SlateGray => new C4ui(Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.564706));
        public static C4ui SlateGrey => new C4ui(Col.UIntFromDoubleClamped(0.439216), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.564706));
        public static C4ui Snow => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.980392), Col.UIntFromDoubleClamped(0.980392));
        public static C4ui SpringGreen => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.498039));
        public static C4ui SteelBlue => new C4ui(Col.UIntFromDoubleClamped(0.27451), Col.UIntFromDoubleClamped(0.509804), Col.UIntFromDoubleClamped(0.705882));
        public static C4ui Tan => new C4ui(Col.UIntFromDoubleClamped(0.823529), Col.UIntFromDoubleClamped(0.705882), Col.UIntFromDoubleClamped(0.54902));
        public static C4ui Teal => new C4ui(Col.UIntFromDoubleClamped(0), Col.UIntFromDoubleClamped(0.501961), Col.UIntFromDoubleClamped(0.501961));
        public static C4ui Thistle => new C4ui(Col.UIntFromDoubleClamped(0.847059), Col.UIntFromDoubleClamped(0.74902), Col.UIntFromDoubleClamped(0.847059));
        public static C4ui Tomato => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0.388235), Col.UIntFromDoubleClamped(0.278431));
        public static C4ui Turquoise => new C4ui(Col.UIntFromDoubleClamped(0.25098), Col.UIntFromDoubleClamped(0.878431), Col.UIntFromDoubleClamped(0.815686));
        public static C4ui Violet => new C4ui(Col.UIntFromDoubleClamped(0.933333), Col.UIntFromDoubleClamped(0.509804), Col.UIntFromDoubleClamped(0.933333));
        public static C4ui Wheat => new C4ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.870588), Col.UIntFromDoubleClamped(0.701961));
        public static C4ui White => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1));
        public static C4ui WhiteSmoke => new C4ui(Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.960784), Col.UIntFromDoubleClamped(0.960784));
        public static C4ui Yellow => new C4ui(Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(1), Col.UIntFromDoubleClamped(0));
        public static C4ui YellowGreen => new C4ui(Col.UIntFromDoubleClamped(0.603922), Col.UIntFromDoubleClamped(0.803922), Col.UIntFromDoubleClamped(0.196078));

        public static C4ui DarkYellow => Olive;

        public static C4ui VRVisGreen => new C4ui(Col.UIntFromDoubleClamped(0.698), Col.UIntFromDoubleClamped(0.851), Col.UIntFromDoubleClamped(0.008));

        public static C4ui Gray10 => new C4ui(Col.UIntFromDoubleClamped(0.1));
        public static C4ui Gray20 => new C4ui(Col.UIntFromDoubleClamped(0.2));
        public static C4ui Gray30 => new C4ui(Col.UIntFromDoubleClamped(0.3));
        public static C4ui Gray40 => new C4ui(Col.UIntFromDoubleClamped(0.4));
        public static C4ui Gray50 => new C4ui(Col.UIntFromDoubleClamped(0.5));
        public static C4ui Gray60 => new C4ui(Col.UIntFromDoubleClamped(0.6));
        public static C4ui Gray70 => new C4ui(Col.UIntFromDoubleClamped(0.7));
        public static C4ui Gray80 => new C4ui(Col.UIntFromDoubleClamped(0.8));
        public static C4ui Gray90 => new C4ui(Col.UIntFromDoubleClamped(0.9));

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

        public static C4ui operator *(C4ui col, double scalar)
        {
            return new C4ui(
                (uint)(col.R * scalar), 
                (uint)(col.G * scalar), 
                (uint)(col.B * scalar), 
                (uint)(col.A * scalar));
        }

        public static C4ui operator *(double scalar, C4ui col)
        {
            return new C4ui(
                (uint)(scalar * col.R), 
                (uint)(scalar * col.G), 
                (uint)(scalar * col.B), 
                (uint)(scalar * col.A));
        }

        public static C4ui operator /(C4ui col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C4ui(
                (uint)(col.R * f), 
                (uint)(col.G * f), 
                (uint)(col.B * f), 
                (uint)(col.A * f));
        }

        public static C4ui operator +(C4ui c0, C4b c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromByte(c1.R)), 
                (uint)(c0.G + Col.UIntFromByte(c1.G)), 
                (uint)(c0.B + Col.UIntFromByte(c1.B)), 
                (uint)(c0.A + Col.UIntFromByte(c1.A)));
        }

        public static C4ui operator -(C4ui c0, C4b c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromByte(c1.R)), 
                (uint)(c0.G - Col.UIntFromByte(c1.G)), 
                (uint)(c0.B - Col.UIntFromByte(c1.B)), 
                (uint)(c0.A - Col.UIntFromByte(c1.A)));
        }

        public static C4ui operator +(C4ui c0, C4us c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G + Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B + Col.UIntFromUShort(c1.B)), 
                (uint)(c0.A + Col.UIntFromUShort(c1.A)));
        }

        public static C4ui operator -(C4ui c0, C4us c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromUShort(c1.R)), 
                (uint)(c0.G - Col.UIntFromUShort(c1.G)), 
                (uint)(c0.B - Col.UIntFromUShort(c1.B)), 
                (uint)(c0.A - Col.UIntFromUShort(c1.A)));
        }

        public static C4ui operator +(C4ui c0, C4ui c1)
        {
            return new C4ui(
                (uint)(c0.R + (c1.R)), 
                (uint)(c0.G + (c1.G)), 
                (uint)(c0.B + (c1.B)), 
                (uint)(c0.A + (c1.A)));
        }

        public static C4ui operator -(C4ui c0, C4ui c1)
        {
            return new C4ui(
                (uint)(c0.R - (c1.R)), 
                (uint)(c0.G - (c1.G)), 
                (uint)(c0.B - (c1.B)), 
                (uint)(c0.A - (c1.A)));
        }

        public static C4ui operator +(C4ui c0, C4f c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G + Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B + Col.UIntFromFloat(c1.B)), 
                (uint)(c0.A + Col.UIntFromFloat(c1.A)));
        }

        public static C4ui operator -(C4ui c0, C4f c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromFloat(c1.R)), 
                (uint)(c0.G - Col.UIntFromFloat(c1.G)), 
                (uint)(c0.B - Col.UIntFromFloat(c1.B)), 
                (uint)(c0.A - Col.UIntFromFloat(c1.A)));
        }

        public static C4ui operator +(C4ui c0, C4d c1)
        {
            return new C4ui(
                (uint)(c0.R + Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G + Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B + Col.UIntFromDouble(c1.B)), 
                (uint)(c0.A + Col.UIntFromDouble(c1.A)));
        }

        public static C4ui operator -(C4ui c0, C4d c1)
        {
            return new C4ui(
                (uint)(c0.R - Col.UIntFromDouble(c1.R)), 
                (uint)(c0.G - Col.UIntFromDouble(c1.G)), 
                (uint)(c0.B - Col.UIntFromDouble(c1.B)), 
                (uint)(c0.A - Col.UIntFromDouble(c1.A)));
        }

        public static V3l operator + (V3l vec, C4ui color)
        {
            return new V3l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B)
                );
        }

        public static V3l operator -(V3l vec, C4ui color)
        {
            return new V3l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B)
                );
        }

        public static V4l operator + (V4l vec, C4ui color)
        {
            return new V4l(
                vec.X + (long)(color.R), 
                vec.Y + (long)(color.G), 
                vec.Z + (long)(color.B),
                vec.W + (long)(color.A)
                );
        }

        public static V4l operator -(V4l vec, C4ui color)
        {
            return new V4l(
                vec.X - (long)(color.R), 
                vec.Y - (long)(color.G), 
                vec.Z - (long)(color.B),
                vec.W - (long)(color.A)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(uint min, uint max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4ui Clamped(uint min, uint max)
        {
            return new C4ui(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp(Col.UIntFromDoubleClamped(min), Col.UIntFromDoubleClamped(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4ui Clamped(double min, double max)
        {
            return Clamped(Col.UIntFromDoubleClamped(min), Col.UIntFromDoubleClamped(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public long Norm1
        {
            get { return (long)R + (long)G + (long)B; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public uint NormMax
        {
            get { return Fun.Max(R, G, B); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public uint NormMin
        {
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
        public static bool ApproximateEquals(this C4ui a, C4ui b)
        {
            return ApproximateEquals(a, b, Constant<uint>.PositiveTinyValue);
        }

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

    [Serializable]
    public partial struct C4f : IFormattable, IEquatable<C4f>, IRGB, IOpacity
    {
        #region Constructors

        public C4f(float r, float g, float b, float a)
        {
            R = r; G = g; B = b; A = a;
        }

        public C4f(int r, int g, int b, int a)
        {
            R = (float)r; G = (float)g; B = (float)b; A = (float)a;
        }

        public C4f(long r, long g, long b, long a)
        {
            R = (float)r; G = (float)g; B = (float)b; A = (float)a;
        }

        public C4f(double r, double g, double b, double a)
        {
            R = (float)(r);
            G = (float)(g);
            B = (float)(b);
            A = (float)(a);
        }

        public C4f(float r, float g, float b)
        {
            R = r; G = g; B = b;
            A = 1.0f;
        }

        public C4f(int r, int g, int b)
        {
            R = (float)r; G = (float)g; B = (float)b;
            A = 1.0f;
        }

        public C4f(long r, long g, long b)
        {
            R = (float)r; G = (float)g; B = (float)b;
            A = 1.0f;
        }

        public C4f(double r, double g, double b)
        {
            R = (float)(r); G = (float)(g); B = (float)(b);
            A = 1.0f;
        }

        public C4f(float gray)
        {
            R = gray; G = gray; B = gray; A = 1.0f;
        }

        public C4f(double gray)
        {
            var value = (float)(gray);
            R = value; G = value; B = value; A = 1.0f;
        }

        public C4f(C3b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
            A = 1.0f;
        }

        public C4f(C3b color, float alpha)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
            A = alpha;
        }

        public C4f(C3us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
            A = 1.0f;
        }

        public C4f(C3us color, float alpha)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
            A = alpha;
        }

        public C4f(C3ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
            A = 1.0f;
        }

        public C4f(C3ui color, float alpha)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
            A = alpha;
        }

        public C4f(C3f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 1.0f;
        }

        public C4f(C3f color, float alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        public C4f(C3d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
            A = 1.0f;
        }

        public C4f(C3d color, float alpha)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
            A = alpha;
        }

        public C4f(C4b color)
        {
            R = Col.FloatFromByte(color.R);
            G = Col.FloatFromByte(color.G);
            B = Col.FloatFromByte(color.B);
            A = Col.FloatFromByte(color.A);
        }

        public C4f(C4us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
            A = Col.FloatFromUShort(color.A);
        }

        public C4f(C4ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
            A = Col.FloatFromUInt(color.A);
        }

        public C4f(C4f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        public C4f(C4d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
            A = Col.FloatFromDouble(color.A);
        }

        public C4f(V3f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = 1.0f;
        }

        public C4f(V3f vec, float alpha)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = alpha;
        }

        public C4f(V3d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
            A = 1.0f;
        }

        public C4f(V3d vec, float alpha)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
            A = alpha;
        }

        public C4f(V4f vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = (vec.W);
        }

        public C4f(V4d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
            A = (float)(vec.W);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C4f color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C4f color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C4f color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C4f color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C4f color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C4f color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C4f color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C4f color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4d(C4f color)
        {
            return new C4d(color);
        }

        public static explicit operator V3f(C4f color)
        {
            return new V3f(
                (color.R), 
                (color.G), 
                (color.B)
                );
        }

        public static explicit operator V3d(C4f color)
        {
            return new V3d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B)
                );
        }

        public static explicit operator V4f(C4f color)
        {
            return new V4f(
                (color.R), 
                (color.G), 
                (color.B),
                (color.A)
                );
        }

        public static explicit operator V4d(C4f color)
        {
            return new V4d(
                (double)(color.R), 
                (double)(color.G), 
                (double)(color.B),
                (double)(color.A)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4d ToC4d() { return (C4d)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C4f(Func<int, float> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        public V3f ToV3f() { return (V3f)this; }
        public V3d ToV3d() { return (V3d)this; }
        public V4f ToV4f() { return (V4f)this; }
        public V4d ToV4d() { return (V4d)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3b(C3b c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3us(C3us c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3ui(C3ui c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3f(C3f c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC3d(C3d c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4b(C4b c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4us(C4us c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4ui(C4ui c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromC4d(C4d c)
            => new C4f(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV3f(V3f c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV3d(V3d c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV4f(V4f c)
            => new C4f(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f FromV4d(V4d c)
            => new C4f(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4b Copy(Func<float, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4us Copy(Func<float, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4ui Copy(Func<float, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4f Copy(Func<float, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4d Copy(Func<float, double> channel_fun)
        {
            return Map(channel_fun);
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
        public float this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    case 3:
                        A = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    case 3:
                        return A;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C4f with all components zero.
        /// </summary>
        public static C4f Zero => new C4f(0.0f, 0.0f, 0.0f, 0.0f);

        // Web colors
        public static C4f AliceBlue => new C4f((float)(0.941176), (float)(0.972549), (float)(1));
        public static C4f AntiqueWhite => new C4f((float)(0.980392), (float)(0.921569), (float)(0.843137));
        public static C4f Aqua => new C4f((float)(0), (float)(1), (float)(1));
        public static C4f Aquamarine => new C4f((float)(0.498039), (float)(1), (float)(0.831373));
        public static C4f Azure => new C4f((float)(0.941176), (float)(1), (float)(1));
        public static C4f Beige => new C4f((float)(0.960784), (float)(0.960784), (float)(0.862745));
        public static C4f Bisque => new C4f((float)(1), (float)(0.894118), (float)(0.768627));
        public static C4f Black => new C4f((float)(0), (float)(0), (float)(0));
        public static C4f BlanchedAlmond => new C4f((float)(1), (float)(0.921569), (float)(0.803922));
        public static C4f Blue => new C4f((float)(0), (float)(0), (float)(1));
        public static C4f BlueViolet => new C4f((float)(0.541176), (float)(0.168627), (float)(0.886275));
        public static C4f Brown => new C4f((float)(0.647059), (float)(0.164706), (float)(0.164706));
        public static C4f BurlyWood => new C4f((float)(0.870588), (float)(0.721569), (float)(0.529412));
        public static C4f CadetBlue => new C4f((float)(0.372549), (float)(0.619608), (float)(0.627451));
        public static C4f Chartreuse => new C4f((float)(0.498039), (float)(1), (float)(0));
        public static C4f Chocolate => new C4f((float)(0.823529), (float)(0.411765), (float)(0.117647));
        public static C4f Coral => new C4f((float)(1), (float)(0.498039), (float)(0.313725));
        public static C4f CornflowerBlue => new C4f((float)(0.392157), (float)(0.584314), (float)(0.929412));
        public static C4f Cornsilk => new C4f((float)(1), (float)(0.972549), (float)(0.862745));
        public static C4f Crimson => new C4f((float)(0.862745), (float)(0.078431), (float)(0.235294));
        public static C4f Cyan => new C4f((float)(0), (float)(1), (float)(1));
        public static C4f DarkBlue => new C4f((float)(0), (float)(0), (float)(0.545098));
        public static C4f DarkCyan => new C4f((float)(0), (float)(0.545098), (float)(0.545098));
        public static C4f DarkGoldenRod => new C4f((float)(0.721569), (float)(0.52549), (float)(0.043137));
        public static C4f DarkGray => new C4f((float)(0.662745), (float)(0.662745), (float)(0.662745));
        public static C4f DarkGrey => new C4f((float)(0.662745), (float)(0.662745), (float)(0.662745));
        public static C4f DarkGreen => new C4f((float)(0), (float)(0.392157), (float)(0));
        public static C4f DarkKhaki => new C4f((float)(0.741176), (float)(0.717647), (float)(0.419608));
        public static C4f DarkMagenta => new C4f((float)(0.545098), (float)(0), (float)(0.545098));
        public static C4f DarkOliveGreen => new C4f((float)(0.333333), (float)(0.419608), (float)(0.184314));
        public static C4f DarkOrange => new C4f((float)(1), (float)(0.54902), (float)(0));
        public static C4f DarkOrchid => new C4f((float)(0.6), (float)(0.196078), (float)(0.8));
        public static C4f DarkRed => new C4f((float)(0.545098), (float)(0), (float)(0));
        public static C4f DarkSalmon => new C4f((float)(0.913725), (float)(0.588235), (float)(0.478431));
        public static C4f DarkSeaGreen => new C4f((float)(0.560784), (float)(0.737255), (float)(0.560784));
        public static C4f DarkSlateBlue => new C4f((float)(0.282353), (float)(0.239216), (float)(0.545098));
        public static C4f DarkSlateGray => new C4f((float)(0.184314), (float)(0.309804), (float)(0.309804));
        public static C4f DarkSlateGrey => new C4f((float)(0.184314), (float)(0.309804), (float)(0.309804));
        public static C4f DarkTurquoise => new C4f((float)(0), (float)(0.807843), (float)(0.819608));
        public static C4f DarkViolet => new C4f((float)(0.580392), (float)(0), (float)(0.827451));
        public static C4f DeepPink => new C4f((float)(1), (float)(0.078431), (float)(0.576471));
        public static C4f DeepSkyBlue => new C4f((float)(0), (float)(0.74902), (float)(1));
        public static C4f DimGray => new C4f((float)(0.411765), (float)(0.411765), (float)(0.411765));
        public static C4f DimGrey => new C4f((float)(0.411765), (float)(0.411765), (float)(0.411765));
        public static C4f DodgerBlue => new C4f((float)(0.117647), (float)(0.564706), (float)(1));
        public static C4f FireBrick => new C4f((float)(0.698039), (float)(0.133333), (float)(0.133333));
        public static C4f FloralWhite => new C4f((float)(1), (float)(0.980392), (float)(0.941176));
        public static C4f ForestGreen => new C4f((float)(0.133333), (float)(0.545098), (float)(0.133333));
        public static C4f Fuchsia => new C4f((float)(1), (float)(0), (float)(1));
        public static C4f Gainsboro => new C4f((float)(0.862745), (float)(0.862745), (float)(0.862745));
        public static C4f GhostWhite => new C4f((float)(0.972549), (float)(0.972549), (float)(1));
        public static C4f Gold => new C4f((float)(1), (float)(0.843137), (float)(0));
        public static C4f GoldenRod => new C4f((float)(0.854902), (float)(0.647059), (float)(0.12549));
        public static C4f Gray => new C4f((float)(0.501961), (float)(0.501961), (float)(0.501961));
        public static C4f Grey => new C4f((float)(0.501961), (float)(0.501961), (float)(0.501961));
        public static C4f Green => new C4f((float)(0), (float)(0.501961), (float)(0));
        public static C4f GreenYellow => new C4f((float)(0.678431), (float)(1), (float)(0.184314));
        public static C4f HoneyDew => new C4f((float)(0.941176), (float)(1), (float)(0.941176));
        public static C4f HotPink => new C4f((float)(1), (float)(0.411765), (float)(0.705882));
        public static C4f IndianRed  => new C4f((float)(0.803922), (float)(0.360784), (float)(0.360784));
        public static C4f Indigo  => new C4f((float)(0.294118), (float)(0), (float)(0.509804));
        public static C4f Ivory => new C4f((float)(1), (float)(1), (float)(0.941176));
        public static C4f Khaki => new C4f((float)(0.941176), (float)(0.901961), (float)(0.54902));
        public static C4f Lavender => new C4f((float)(0.901961), (float)(0.901961), (float)(0.980392));
        public static C4f LavenderBlush => new C4f((float)(1), (float)(0.941176), (float)(0.960784));
        public static C4f LawnGreen => new C4f((float)(0.486275), (float)(0.988235), (float)(0));
        public static C4f LemonChiffon => new C4f((float)(1), (float)(0.980392), (float)(0.803922));
        public static C4f LightBlue => new C4f((float)(0.678431), (float)(0.847059), (float)(0.901961));
        public static C4f LightCoral => new C4f((float)(0.941176), (float)(0.501961), (float)(0.501961));
        public static C4f LightCyan => new C4f((float)(0.878431), (float)(1), (float)(1));
        public static C4f LightGoldenRodYellow => new C4f((float)(0.980392), (float)(0.980392), (float)(0.823529));
        public static C4f LightGray => new C4f((float)(0.827451), (float)(0.827451), (float)(0.827451));
        public static C4f LightGrey => new C4f((float)(0.827451), (float)(0.827451), (float)(0.827451));
        public static C4f LightGreen => new C4f((float)(0.564706), (float)(0.933333), (float)(0.564706));
        public static C4f LightPink => new C4f((float)(1), (float)(0.713725), (float)(0.756863));
        public static C4f LightSalmon => new C4f((float)(1), (float)(0.627451), (float)(0.478431));
        public static C4f LightSeaGreen => new C4f((float)(0.12549), (float)(0.698039), (float)(0.666667));
        public static C4f LightSkyBlue => new C4f((float)(0.529412), (float)(0.807843), (float)(0.980392));
        public static C4f LightSlateGray => new C4f((float)(0.466667), (float)(0.533333), (float)(0.6));
        public static C4f LightSlateGrey => new C4f((float)(0.466667), (float)(0.533333), (float)(0.6));
        public static C4f LightSteelBlue => new C4f((float)(0.690196), (float)(0.768627), (float)(0.870588));
        public static C4f LightYellow => new C4f((float)(1), (float)(1), (float)(0.878431));
        public static C4f Lime => new C4f((float)(0), (float)(1), (float)(0));
        public static C4f LimeGreen => new C4f((float)(0.196078), (float)(0.803922), (float)(0.196078));
        public static C4f Linen => new C4f((float)(0.980392), (float)(0.941176), (float)(0.901961));
        public static C4f Magenta => new C4f((float)(1), (float)(0), (float)(1));
        public static C4f Maroon => new C4f((float)(0.501961), (float)(0), (float)(0));
        public static C4f MediumAquaMarine => new C4f((float)(0.4), (float)(0.803922), (float)(0.666667));
        public static C4f MediumBlue => new C4f((float)(0), (float)(0), (float)(0.803922));
        public static C4f MediumOrchid => new C4f((float)(0.729412), (float)(0.333333), (float)(0.827451));
        public static C4f MediumPurple => new C4f((float)(0.576471), (float)(0.439216), (float)(0.847059));
        public static C4f MediumSeaGreen => new C4f((float)(0.235294), (float)(0.701961), (float)(0.443137));
        public static C4f MediumSlateBlue => new C4f((float)(0.482353), (float)(0.407843), (float)(0.933333));
        public static C4f MediumSpringGreen => new C4f((float)(0), (float)(0.980392), (float)(0.603922));
        public static C4f MediumTurquoise => new C4f((float)(0.282353), (float)(0.819608), (float)(0.8));
        public static C4f MediumVioletRed => new C4f((float)(0.780392), (float)(0.082353), (float)(0.521569));
        public static C4f MidnightBlue => new C4f((float)(0.098039), (float)(0.098039), (float)(0.439216));
        public static C4f MintCream => new C4f((float)(0.960784), (float)(1), (float)(0.980392));
        public static C4f MistyRose => new C4f((float)(1), (float)(0.894118), (float)(0.882353));
        public static C4f Moccasin => new C4f((float)(1), (float)(0.894118), (float)(0.709804));
        public static C4f NavajoWhite => new C4f((float)(1), (float)(0.870588), (float)(0.678431));
        public static C4f Navy => new C4f((float)(0), (float)(0), (float)(0.501961));
        public static C4f OldLace => new C4f((float)(0.992157), (float)(0.960784), (float)(0.901961));
        public static C4f Olive => new C4f((float)(0.501961), (float)(0.501961), (float)(0));
        public static C4f OliveDrab => new C4f((float)(0.419608), (float)(0.556863), (float)(0.137255));
        public static C4f Orange => new C4f((float)(1), (float)(0.647059), (float)(0));
        public static C4f OrangeRed => new C4f((float)(1), (float)(0.270588), (float)(0));
        public static C4f Orchid => new C4f((float)(0.854902), (float)(0.439216), (float)(0.839216));
        public static C4f PaleGoldenRod => new C4f((float)(0.933333), (float)(0.909804), (float)(0.666667));
        public static C4f PaleGreen => new C4f((float)(0.596078), (float)(0.984314), (float)(0.596078));
        public static C4f PaleTurquoise => new C4f((float)(0.686275), (float)(0.933333), (float)(0.933333));
        public static C4f PaleVioletRed => new C4f((float)(0.847059), (float)(0.439216), (float)(0.576471));
        public static C4f PapayaWhip => new C4f((float)(1), (float)(0.937255), (float)(0.835294));
        public static C4f PeachPuff => new C4f((float)(1), (float)(0.854902), (float)(0.72549));
        public static C4f Peru => new C4f((float)(0.803922), (float)(0.521569), (float)(0.247059));
        public static C4f Pink => new C4f((float)(1), (float)(0.752941), (float)(0.796078));
        public static C4f Plum => new C4f((float)(0.866667), (float)(0.627451), (float)(0.866667));
        public static C4f PowderBlue => new C4f((float)(0.690196), (float)(0.878431), (float)(0.901961));
        public static C4f Purple => new C4f((float)(0.501961), (float)(0), (float)(0.501961));
        public static C4f Red => new C4f((float)(1), (float)(0), (float)(0));
        public static C4f RosyBrown => new C4f((float)(0.737255), (float)(0.560784), (float)(0.560784));
        public static C4f RoyalBlue => new C4f((float)(0.254902), (float)(0.411765), (float)(0.882353));
        public static C4f SaddleBrown => new C4f((float)(0.545098), (float)(0.270588), (float)(0.07451));
        public static C4f Salmon => new C4f((float)(0.980392), (float)(0.501961), (float)(0.447059));
        public static C4f SandyBrown => new C4f((float)(0.956863), (float)(0.643137), (float)(0.376471));
        public static C4f SeaGreen => new C4f((float)(0.180392), (float)(0.545098), (float)(0.341176));
        public static C4f SeaShell => new C4f((float)(1), (float)(0.960784), (float)(0.933333));
        public static C4f Sienna => new C4f((float)(0.627451), (float)(0.321569), (float)(0.176471));
        public static C4f Silver => new C4f((float)(0.752941), (float)(0.752941), (float)(0.752941));
        public static C4f SkyBlue => new C4f((float)(0.529412), (float)(0.807843), (float)(0.921569));
        public static C4f SlateBlue => new C4f((float)(0.415686), (float)(0.352941), (float)(0.803922));
        public static C4f SlateGray => new C4f((float)(0.439216), (float)(0.501961), (float)(0.564706));
        public static C4f SlateGrey => new C4f((float)(0.439216), (float)(0.501961), (float)(0.564706));
        public static C4f Snow => new C4f((float)(1), (float)(0.980392), (float)(0.980392));
        public static C4f SpringGreen => new C4f((float)(0), (float)(1), (float)(0.498039));
        public static C4f SteelBlue => new C4f((float)(0.27451), (float)(0.509804), (float)(0.705882));
        public static C4f Tan => new C4f((float)(0.823529), (float)(0.705882), (float)(0.54902));
        public static C4f Teal => new C4f((float)(0), (float)(0.501961), (float)(0.501961));
        public static C4f Thistle => new C4f((float)(0.847059), (float)(0.74902), (float)(0.847059));
        public static C4f Tomato => new C4f((float)(1), (float)(0.388235), (float)(0.278431));
        public static C4f Turquoise => new C4f((float)(0.25098), (float)(0.878431), (float)(0.815686));
        public static C4f Violet => new C4f((float)(0.933333), (float)(0.509804), (float)(0.933333));
        public static C4f Wheat => new C4f((float)(0.960784), (float)(0.870588), (float)(0.701961));
        public static C4f White => new C4f((float)(1), (float)(1), (float)(1));
        public static C4f WhiteSmoke => new C4f((float)(0.960784), (float)(0.960784), (float)(0.960784));
        public static C4f Yellow => new C4f((float)(1), (float)(1), (float)(0));
        public static C4f YellowGreen => new C4f((float)(0.603922), (float)(0.803922), (float)(0.196078));

        public static C4f DarkYellow => Olive;

        public static C4f VRVisGreen => new C4f((float)(0.698), (float)(0.851), (float)(0.008));

        public static C4f Gray10 => new C4f((float)(0.1));
        public static C4f Gray20 => new C4f((float)(0.2));
        public static C4f Gray30 => new C4f((float)(0.3));
        public static C4f Gray40 => new C4f((float)(0.4));
        public static C4f Gray50 => new C4f((float)(0.5));
        public static C4f Gray60 => new C4f((float)(0.6));
        public static C4f Gray70 => new C4f((float)(0.7));
        public static C4f Gray80 => new C4f((float)(0.8));
        public static C4f Gray90 => new C4f((float)(0.9));

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

        public static C4f operator *(C4f col, double scalar)
        {
            return new C4f(
                (float)(col.R * scalar), 
                (float)(col.G * scalar), 
                (float)(col.B * scalar), 
                (float)(col.A * scalar));
        }

        public static C4f operator *(double scalar, C4f col)
        {
            return new C4f(
                (float)(scalar * col.R), 
                (float)(scalar * col.G), 
                (float)(scalar * col.B), 
                (float)(scalar * col.A));
        }

        public static C4f operator /(C4f col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C4f(
                (float)(col.R * f), 
                (float)(col.G * f), 
                (float)(col.B * f), 
                (float)(col.A * f));
        }

        public static C4f operator +(C4f c0, C4b c1)
        {
            return new C4f(
                (float)(c0.R + Col.FloatFromByte(c1.R)), 
                (float)(c0.G + Col.FloatFromByte(c1.G)), 
                (float)(c0.B + Col.FloatFromByte(c1.B)), 
                (float)(c0.A + Col.FloatFromByte(c1.A)));
        }

        public static C4f operator -(C4f c0, C4b c1)
        {
            return new C4f(
                (float)(c0.R - Col.FloatFromByte(c1.R)), 
                (float)(c0.G - Col.FloatFromByte(c1.G)), 
                (float)(c0.B - Col.FloatFromByte(c1.B)), 
                (float)(c0.A - Col.FloatFromByte(c1.A)));
        }

        public static C4f operator +(C4f c0, C4us c1)
        {
            return new C4f(
                (float)(c0.R + Col.FloatFromUShort(c1.R)), 
                (float)(c0.G + Col.FloatFromUShort(c1.G)), 
                (float)(c0.B + Col.FloatFromUShort(c1.B)), 
                (float)(c0.A + Col.FloatFromUShort(c1.A)));
        }

        public static C4f operator -(C4f c0, C4us c1)
        {
            return new C4f(
                (float)(c0.R - Col.FloatFromUShort(c1.R)), 
                (float)(c0.G - Col.FloatFromUShort(c1.G)), 
                (float)(c0.B - Col.FloatFromUShort(c1.B)), 
                (float)(c0.A - Col.FloatFromUShort(c1.A)));
        }

        public static C4f operator +(C4f c0, C4ui c1)
        {
            return new C4f(
                (float)(c0.R + Col.FloatFromUInt(c1.R)), 
                (float)(c0.G + Col.FloatFromUInt(c1.G)), 
                (float)(c0.B + Col.FloatFromUInt(c1.B)), 
                (float)(c0.A + Col.FloatFromUInt(c1.A)));
        }

        public static C4f operator -(C4f c0, C4ui c1)
        {
            return new C4f(
                (float)(c0.R - Col.FloatFromUInt(c1.R)), 
                (float)(c0.G - Col.FloatFromUInt(c1.G)), 
                (float)(c0.B - Col.FloatFromUInt(c1.B)), 
                (float)(c0.A - Col.FloatFromUInt(c1.A)));
        }

        public static C4f operator +(C4f c0, C4f c1)
        {
            return new C4f(
                (float)(c0.R + (c1.R)), 
                (float)(c0.G + (c1.G)), 
                (float)(c0.B + (c1.B)), 
                (float)(c0.A + (c1.A)));
        }

        public static C4f operator -(C4f c0, C4f c1)
        {
            return new C4f(
                (float)(c0.R - (c1.R)), 
                (float)(c0.G - (c1.G)), 
                (float)(c0.B - (c1.B)), 
                (float)(c0.A - (c1.A)));
        }

        public static C4f operator +(C4f c0, C4d c1)
        {
            return new C4f(
                (float)(c0.R + (float)(c1.R)), 
                (float)(c0.G + (float)(c1.G)), 
                (float)(c0.B + (float)(c1.B)), 
                (float)(c0.A + (float)(c1.A)));
        }

        public static C4f operator -(C4f c0, C4d c1)
        {
            return new C4f(
                (float)(c0.R - (float)(c1.R)), 
                (float)(c0.G - (float)(c1.G)), 
                (float)(c0.B - (float)(c1.B)), 
                (float)(c0.A - (float)(c1.A)));
        }

        public static C4f operator *(C4f c0, C4f c1)
        {
            return new C4f(
                (float)(c0.R * c1.R), 
                (float)(c0.G * c1.G), 
                (float)(c0.B * c1.B), 
                (float)(c0.A * c1.A));
        }

        public static C4f operator /(C4f c0, C4f c1)
        {
            return new C4f(
                (float)(c0.R / c1.R), 
                (float)(c0.G / c1.G), 
                (float)(c0.B / c1.B), 
                (float)(c0.A / c1.A));
        }

        public static C4f operator +(C4f col, double scalar)
        {
            return new C4f(
                (float)(col.R + scalar), 
                (float)(col.G + scalar), 
                (float)(col.B + scalar), 
                (float)(col.A + scalar));
        }

        public static C4f operator +(double scalar, C4f col)
        {
            return new C4f(
                (float)(scalar + col.R), 
                (float)(scalar + col.G), 
                (float)(scalar + col.B), 
                (float)(scalar + col.A));
        }

        public static C4f operator -(C4f col, double scalar)
        {
            return new C4f(
                (float)(col.R - scalar), 
                (float)(col.G - scalar), 
                (float)(col.B - scalar), 
                (float)(col.A - scalar));
        }

        public static C4f operator -(double scalar, C4f col)
        {
            return new C4f(
                (float)(scalar - col.R), 
                (float)(scalar - col.G), 
                (float)(scalar - col.B), 
                (float)(scalar - col.A));
        }

        public static V3f operator + (V3f vec, C4f color)
        {
            return new V3f(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B)
                );
        }

        public static V3f operator -(V3f vec, C4f color)
        {
            return new V3f(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B)
                );
        }

        public static V3d operator + (V3d vec, C4f color)
        {
            return new V3d(
                vec.X + (double)(color.R), 
                vec.Y + (double)(color.G), 
                vec.Z + (double)(color.B)
                );
        }

        public static V3d operator -(V3d vec, C4f color)
        {
            return new V3d(
                vec.X - (double)(color.R), 
                vec.Y - (double)(color.G), 
                vec.Z - (double)(color.B)
                );
        }

        public static V4f operator + (V4f vec, C4f color)
        {
            return new V4f(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B),
                vec.W + (color.A)
                );
        }

        public static V4f operator -(V4f vec, C4f color)
        {
            return new V4f(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B),
                vec.W - (color.A)
                );
        }

        public static V4d operator + (V4d vec, C4f color)
        {
            return new V4d(
                vec.X + (double)(color.R), 
                vec.Y + (double)(color.G), 
                vec.Z + (double)(color.B),
                vec.W + (double)(color.A)
                );
        }

        public static V4d operator -(V4d vec, C4f color)
        {
            return new V4d(
                vec.X - (double)(color.R), 
                vec.Y - (double)(color.G), 
                vec.Z - (double)(color.B),
                vec.W - (double)(color.A)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(float min, float max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4f Clamped(float min, float max)
        {
            return new C4f(R.Clamp(min, max), G.Clamp(min, max), B.Clamp(min, max), A);
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp((float)(min), (float)(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public C4f Clamped(double min, double max)
        {
            return Clamped((float)(min), (float)(max));
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. The alpha channel is ignored.
        /// </summary>
        public double Norm1
        {
            get { return (double)Fun.Abs(R) + (double)Fun.Abs(G) + (double)Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public float NormMax
        {
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public float NormMin
        {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4f p0, C4f p1, C4f p2, C4f p3, ref Tup4<float> w)
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
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, ref Tup4<double> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4f p0, C4f p1, C4f p2, C4f p3, ref Tup4<double> w)
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
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<float> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<float> w)
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
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<double> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
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

    [Serializable]
    public partial struct C4d : IFormattable, IEquatable<C4d>, IRGB, IOpacity
    {
        #region Constructors

        public C4d(double r, double g, double b, double a)
        {
            R = r; G = g; B = b; A = a;
        }

        public C4d(int r, int g, int b, int a)
        {
            R = (double)r; G = (double)g; B = (double)b; A = (double)a;
        }

        public C4d(long r, long g, long b, long a)
        {
            R = (double)r; G = (double)g; B = (double)b; A = (double)a;
        }

        public C4d(double r, double g, double b)
        {
            R = r; G = g; B = b;
            A = 1.0;
        }

        public C4d(int r, int g, int b)
        {
            R = (double)r; G = (double)g; B = (double)b;
            A = 1.0;
        }

        public C4d(long r, long g, long b)
        {
            R = (double)r; G = (double)g; B = (double)b;
            A = 1.0;
        }

        public C4d(double gray)
        {
            R = gray; G = gray; B = gray; A = 1.0;
        }

        public C4d(C3b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
            A = 1.0;
        }

        public C4d(C3b color, double alpha)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
            A = alpha;
        }

        public C4d(C3us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
            A = 1.0;
        }

        public C4d(C3us color, double alpha)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
            A = alpha;
        }

        public C4d(C3ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
            A = 1.0;
        }

        public C4d(C3ui color, double alpha)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
            A = alpha;
        }

        public C4d(C3f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
            A = 1.0;
        }

        public C4d(C3f color, double alpha)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
            A = alpha;
        }

        public C4d(C3d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 1.0;
        }

        public C4d(C3d color, double alpha)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = alpha;
        }

        public C4d(C4b color)
        {
            R = Col.DoubleFromByte(color.R);
            G = Col.DoubleFromByte(color.G);
            B = Col.DoubleFromByte(color.B);
            A = Col.DoubleFromByte(color.A);
        }

        public C4d(C4us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
            A = Col.DoubleFromUShort(color.A);
        }

        public C4d(C4ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
            A = Col.DoubleFromUInt(color.A);
        }

        public C4d(C4f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
            A = Col.DoubleFromFloat(color.A);
        }

        public C4d(C4d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = (color.A);
        }

        public C4d(V3d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = 1.0;
        }

        public C4d(V3d vec, double alpha)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = alpha;
        }

        public C4d(V4d vec)
        {
            R = (vec.X);
            G = (vec.Y);
            B = (vec.Z);
            A = (vec.W);
        }

        #endregion

        #region Conversions

        public static explicit operator C3b(C4d color)
        {
            return new C3b(color);
        }

        public static explicit operator C3us(C4d color)
        {
            return new C3us(color);
        }

        public static explicit operator C3ui(C4d color)
        {
            return new C3ui(color);
        }

        public static explicit operator C3f(C4d color)
        {
            return new C3f(color);
        }

        public static explicit operator C3d(C4d color)
        {
            return new C3d(color);
        }

        public static explicit operator C4b(C4d color)
        {
            return new C4b(color);
        }

        public static explicit operator C4us(C4d color)
        {
            return new C4us(color);
        }

        public static explicit operator C4ui(C4d color)
        {
            return new C4ui(color);
        }

        public static explicit operator C4f(C4d color)
        {
            return new C4f(color);
        }

        public static explicit operator V3d(C4d color)
        {
            return new V3d(
                (color.R), 
                (color.G), 
                (color.B)
                );
        }

        public static explicit operator V4d(C4d color)
        {
            return new V4d(
                (color.R), 
                (color.G), 
                (color.B),
                (color.A)
                );
        }

        public C3b ToC3b() { return (C3b)this; }
        public C3us ToC3us() { return (C3us)this; }
        public C3ui ToC3ui() { return (C3ui)this; }
        public C3f ToC3f() { return (C3f)this; }
        public C3d ToC3d() { return (C3d)this; }
        public C4b ToC4b() { return (C4b)this; }
        public C4us ToC4us() { return (C4us)this; }
        public C4ui ToC4ui() { return (C4ui)this; }
        public C4f ToC4f() { return (C4f)this; }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public C4d(Func<int, double> index_fun)
        {
            R = index_fun(0);
            G = index_fun(1);
            B = index_fun(2);
            A = index_fun(3);
        }

        public V3d ToV3d() { return (V3d)this; }
        public V4d ToV4d() { return (V4d)this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3b(C3b c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3us(C3us c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3ui(C3ui c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3f(C3f c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC3d(C3d c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4b(C4b c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4us(C4us c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4ui(C4ui c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromC4f(C4f c)
            => new C4d(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromV3d(V3d c)
            => new C4d(c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d FromV4d(V4d c)
            => new C4d(c);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4b Copy(Func<double, byte> channel_fun)
        {
            return Map(channel_fun);
        }

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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4us Copy(Func<double, ushort> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4ui Copy(Func<double, uint> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4f Copy(Func<double, float> channel_fun)
        {
            return Map(channel_fun);
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
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public C4d Copy(Func<double, double> channel_fun)
        {
            return Map(channel_fun);
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
        public double this[int i]
        {
            set
            {
                switch (i)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    case 3:
                        A = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {
                    case 0:
                        return R;
                    case 1:
                        return G;
                    case 2:
                        return B;
                    case 3:
                        return A;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// C4d with all components zero.
        /// </summary>
        public static C4d Zero => new C4d(0, 0, 0, 0);

        // Web colors
        public static C4d AliceBlue => new C4d((0.941176), (0.972549), (1));
        public static C4d AntiqueWhite => new C4d((0.980392), (0.921569), (0.843137));
        public static C4d Aqua => new C4d((0), (1), (1));
        public static C4d Aquamarine => new C4d((0.498039), (1), (0.831373));
        public static C4d Azure => new C4d((0.941176), (1), (1));
        public static C4d Beige => new C4d((0.960784), (0.960784), (0.862745));
        public static C4d Bisque => new C4d((1), (0.894118), (0.768627));
        public static C4d Black => new C4d((0), (0), (0));
        public static C4d BlanchedAlmond => new C4d((1), (0.921569), (0.803922));
        public static C4d Blue => new C4d((0), (0), (1));
        public static C4d BlueViolet => new C4d((0.541176), (0.168627), (0.886275));
        public static C4d Brown => new C4d((0.647059), (0.164706), (0.164706));
        public static C4d BurlyWood => new C4d((0.870588), (0.721569), (0.529412));
        public static C4d CadetBlue => new C4d((0.372549), (0.619608), (0.627451));
        public static C4d Chartreuse => new C4d((0.498039), (1), (0));
        public static C4d Chocolate => new C4d((0.823529), (0.411765), (0.117647));
        public static C4d Coral => new C4d((1), (0.498039), (0.313725));
        public static C4d CornflowerBlue => new C4d((0.392157), (0.584314), (0.929412));
        public static C4d Cornsilk => new C4d((1), (0.972549), (0.862745));
        public static C4d Crimson => new C4d((0.862745), (0.078431), (0.235294));
        public static C4d Cyan => new C4d((0), (1), (1));
        public static C4d DarkBlue => new C4d((0), (0), (0.545098));
        public static C4d DarkCyan => new C4d((0), (0.545098), (0.545098));
        public static C4d DarkGoldenRod => new C4d((0.721569), (0.52549), (0.043137));
        public static C4d DarkGray => new C4d((0.662745), (0.662745), (0.662745));
        public static C4d DarkGrey => new C4d((0.662745), (0.662745), (0.662745));
        public static C4d DarkGreen => new C4d((0), (0.392157), (0));
        public static C4d DarkKhaki => new C4d((0.741176), (0.717647), (0.419608));
        public static C4d DarkMagenta => new C4d((0.545098), (0), (0.545098));
        public static C4d DarkOliveGreen => new C4d((0.333333), (0.419608), (0.184314));
        public static C4d DarkOrange => new C4d((1), (0.54902), (0));
        public static C4d DarkOrchid => new C4d((0.6), (0.196078), (0.8));
        public static C4d DarkRed => new C4d((0.545098), (0), (0));
        public static C4d DarkSalmon => new C4d((0.913725), (0.588235), (0.478431));
        public static C4d DarkSeaGreen => new C4d((0.560784), (0.737255), (0.560784));
        public static C4d DarkSlateBlue => new C4d((0.282353), (0.239216), (0.545098));
        public static C4d DarkSlateGray => new C4d((0.184314), (0.309804), (0.309804));
        public static C4d DarkSlateGrey => new C4d((0.184314), (0.309804), (0.309804));
        public static C4d DarkTurquoise => new C4d((0), (0.807843), (0.819608));
        public static C4d DarkViolet => new C4d((0.580392), (0), (0.827451));
        public static C4d DeepPink => new C4d((1), (0.078431), (0.576471));
        public static C4d DeepSkyBlue => new C4d((0), (0.74902), (1));
        public static C4d DimGray => new C4d((0.411765), (0.411765), (0.411765));
        public static C4d DimGrey => new C4d((0.411765), (0.411765), (0.411765));
        public static C4d DodgerBlue => new C4d((0.117647), (0.564706), (1));
        public static C4d FireBrick => new C4d((0.698039), (0.133333), (0.133333));
        public static C4d FloralWhite => new C4d((1), (0.980392), (0.941176));
        public static C4d ForestGreen => new C4d((0.133333), (0.545098), (0.133333));
        public static C4d Fuchsia => new C4d((1), (0), (1));
        public static C4d Gainsboro => new C4d((0.862745), (0.862745), (0.862745));
        public static C4d GhostWhite => new C4d((0.972549), (0.972549), (1));
        public static C4d Gold => new C4d((1), (0.843137), (0));
        public static C4d GoldenRod => new C4d((0.854902), (0.647059), (0.12549));
        public static C4d Gray => new C4d((0.501961), (0.501961), (0.501961));
        public static C4d Grey => new C4d((0.501961), (0.501961), (0.501961));
        public static C4d Green => new C4d((0), (0.501961), (0));
        public static C4d GreenYellow => new C4d((0.678431), (1), (0.184314));
        public static C4d HoneyDew => new C4d((0.941176), (1), (0.941176));
        public static C4d HotPink => new C4d((1), (0.411765), (0.705882));
        public static C4d IndianRed  => new C4d((0.803922), (0.360784), (0.360784));
        public static C4d Indigo  => new C4d((0.294118), (0), (0.509804));
        public static C4d Ivory => new C4d((1), (1), (0.941176));
        public static C4d Khaki => new C4d((0.941176), (0.901961), (0.54902));
        public static C4d Lavender => new C4d((0.901961), (0.901961), (0.980392));
        public static C4d LavenderBlush => new C4d((1), (0.941176), (0.960784));
        public static C4d LawnGreen => new C4d((0.486275), (0.988235), (0));
        public static C4d LemonChiffon => new C4d((1), (0.980392), (0.803922));
        public static C4d LightBlue => new C4d((0.678431), (0.847059), (0.901961));
        public static C4d LightCoral => new C4d((0.941176), (0.501961), (0.501961));
        public static C4d LightCyan => new C4d((0.878431), (1), (1));
        public static C4d LightGoldenRodYellow => new C4d((0.980392), (0.980392), (0.823529));
        public static C4d LightGray => new C4d((0.827451), (0.827451), (0.827451));
        public static C4d LightGrey => new C4d((0.827451), (0.827451), (0.827451));
        public static C4d LightGreen => new C4d((0.564706), (0.933333), (0.564706));
        public static C4d LightPink => new C4d((1), (0.713725), (0.756863));
        public static C4d LightSalmon => new C4d((1), (0.627451), (0.478431));
        public static C4d LightSeaGreen => new C4d((0.12549), (0.698039), (0.666667));
        public static C4d LightSkyBlue => new C4d((0.529412), (0.807843), (0.980392));
        public static C4d LightSlateGray => new C4d((0.466667), (0.533333), (0.6));
        public static C4d LightSlateGrey => new C4d((0.466667), (0.533333), (0.6));
        public static C4d LightSteelBlue => new C4d((0.690196), (0.768627), (0.870588));
        public static C4d LightYellow => new C4d((1), (1), (0.878431));
        public static C4d Lime => new C4d((0), (1), (0));
        public static C4d LimeGreen => new C4d((0.196078), (0.803922), (0.196078));
        public static C4d Linen => new C4d((0.980392), (0.941176), (0.901961));
        public static C4d Magenta => new C4d((1), (0), (1));
        public static C4d Maroon => new C4d((0.501961), (0), (0));
        public static C4d MediumAquaMarine => new C4d((0.4), (0.803922), (0.666667));
        public static C4d MediumBlue => new C4d((0), (0), (0.803922));
        public static C4d MediumOrchid => new C4d((0.729412), (0.333333), (0.827451));
        public static C4d MediumPurple => new C4d((0.576471), (0.439216), (0.847059));
        public static C4d MediumSeaGreen => new C4d((0.235294), (0.701961), (0.443137));
        public static C4d MediumSlateBlue => new C4d((0.482353), (0.407843), (0.933333));
        public static C4d MediumSpringGreen => new C4d((0), (0.980392), (0.603922));
        public static C4d MediumTurquoise => new C4d((0.282353), (0.819608), (0.8));
        public static C4d MediumVioletRed => new C4d((0.780392), (0.082353), (0.521569));
        public static C4d MidnightBlue => new C4d((0.098039), (0.098039), (0.439216));
        public static C4d MintCream => new C4d((0.960784), (1), (0.980392));
        public static C4d MistyRose => new C4d((1), (0.894118), (0.882353));
        public static C4d Moccasin => new C4d((1), (0.894118), (0.709804));
        public static C4d NavajoWhite => new C4d((1), (0.870588), (0.678431));
        public static C4d Navy => new C4d((0), (0), (0.501961));
        public static C4d OldLace => new C4d((0.992157), (0.960784), (0.901961));
        public static C4d Olive => new C4d((0.501961), (0.501961), (0));
        public static C4d OliveDrab => new C4d((0.419608), (0.556863), (0.137255));
        public static C4d Orange => new C4d((1), (0.647059), (0));
        public static C4d OrangeRed => new C4d((1), (0.270588), (0));
        public static C4d Orchid => new C4d((0.854902), (0.439216), (0.839216));
        public static C4d PaleGoldenRod => new C4d((0.933333), (0.909804), (0.666667));
        public static C4d PaleGreen => new C4d((0.596078), (0.984314), (0.596078));
        public static C4d PaleTurquoise => new C4d((0.686275), (0.933333), (0.933333));
        public static C4d PaleVioletRed => new C4d((0.847059), (0.439216), (0.576471));
        public static C4d PapayaWhip => new C4d((1), (0.937255), (0.835294));
        public static C4d PeachPuff => new C4d((1), (0.854902), (0.72549));
        public static C4d Peru => new C4d((0.803922), (0.521569), (0.247059));
        public static C4d Pink => new C4d((1), (0.752941), (0.796078));
        public static C4d Plum => new C4d((0.866667), (0.627451), (0.866667));
        public static C4d PowderBlue => new C4d((0.690196), (0.878431), (0.901961));
        public static C4d Purple => new C4d((0.501961), (0), (0.501961));
        public static C4d Red => new C4d((1), (0), (0));
        public static C4d RosyBrown => new C4d((0.737255), (0.560784), (0.560784));
        public static C4d RoyalBlue => new C4d((0.254902), (0.411765), (0.882353));
        public static C4d SaddleBrown => new C4d((0.545098), (0.270588), (0.07451));
        public static C4d Salmon => new C4d((0.980392), (0.501961), (0.447059));
        public static C4d SandyBrown => new C4d((0.956863), (0.643137), (0.376471));
        public static C4d SeaGreen => new C4d((0.180392), (0.545098), (0.341176));
        public static C4d SeaShell => new C4d((1), (0.960784), (0.933333));
        public static C4d Sienna => new C4d((0.627451), (0.321569), (0.176471));
        public static C4d Silver => new C4d((0.752941), (0.752941), (0.752941));
        public static C4d SkyBlue => new C4d((0.529412), (0.807843), (0.921569));
        public static C4d SlateBlue => new C4d((0.415686), (0.352941), (0.803922));
        public static C4d SlateGray => new C4d((0.439216), (0.501961), (0.564706));
        public static C4d SlateGrey => new C4d((0.439216), (0.501961), (0.564706));
        public static C4d Snow => new C4d((1), (0.980392), (0.980392));
        public static C4d SpringGreen => new C4d((0), (1), (0.498039));
        public static C4d SteelBlue => new C4d((0.27451), (0.509804), (0.705882));
        public static C4d Tan => new C4d((0.823529), (0.705882), (0.54902));
        public static C4d Teal => new C4d((0), (0.501961), (0.501961));
        public static C4d Thistle => new C4d((0.847059), (0.74902), (0.847059));
        public static C4d Tomato => new C4d((1), (0.388235), (0.278431));
        public static C4d Turquoise => new C4d((0.25098), (0.878431), (0.815686));
        public static C4d Violet => new C4d((0.933333), (0.509804), (0.933333));
        public static C4d Wheat => new C4d((0.960784), (0.870588), (0.701961));
        public static C4d White => new C4d((1), (1), (1));
        public static C4d WhiteSmoke => new C4d((0.960784), (0.960784), (0.960784));
        public static C4d Yellow => new C4d((1), (1), (0));
        public static C4d YellowGreen => new C4d((0.603922), (0.803922), (0.196078));

        public static C4d DarkYellow => Olive;

        public static C4d VRVisGreen => new C4d((0.698), (0.851), (0.008));

        public static C4d Gray10 => new C4d((0.1));
        public static C4d Gray20 => new C4d((0.2));
        public static C4d Gray30 => new C4d((0.3));
        public static C4d Gray40 => new C4d((0.4));
        public static C4d Gray50 => new C4d((0.5));
        public static C4d Gray60 => new C4d((0.6));
        public static C4d Gray70 => new C4d((0.7));
        public static C4d Gray80 => new C4d((0.8));
        public static C4d Gray90 => new C4d((0.9));

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

        public static C4d operator *(C4d col, double scalar)
        {
            return new C4d(
                (double)(col.R * scalar), 
                (double)(col.G * scalar), 
                (double)(col.B * scalar), 
                (double)(col.A * scalar));
        }

        public static C4d operator *(double scalar, C4d col)
        {
            return new C4d(
                (double)(scalar * col.R), 
                (double)(scalar * col.G), 
                (double)(scalar * col.B), 
                (double)(scalar * col.A));
        }

        public static C4d operator /(C4d col, double scalar)
        {
            double f = 1.0 / scalar;
            return new C4d(
                (double)(col.R * f), 
                (double)(col.G * f), 
                (double)(col.B * f), 
                (double)(col.A * f));
        }

        public static C4d operator +(C4d c0, C4b c1)
        {
            return new C4d(
                (double)(c0.R + Col.DoubleFromByte(c1.R)), 
                (double)(c0.G + Col.DoubleFromByte(c1.G)), 
                (double)(c0.B + Col.DoubleFromByte(c1.B)), 
                (double)(c0.A + Col.DoubleFromByte(c1.A)));
        }

        public static C4d operator -(C4d c0, C4b c1)
        {
            return new C4d(
                (double)(c0.R - Col.DoubleFromByte(c1.R)), 
                (double)(c0.G - Col.DoubleFromByte(c1.G)), 
                (double)(c0.B - Col.DoubleFromByte(c1.B)), 
                (double)(c0.A - Col.DoubleFromByte(c1.A)));
        }

        public static C4d operator +(C4d c0, C4us c1)
        {
            return new C4d(
                (double)(c0.R + Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G + Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B + Col.DoubleFromUShort(c1.B)), 
                (double)(c0.A + Col.DoubleFromUShort(c1.A)));
        }

        public static C4d operator -(C4d c0, C4us c1)
        {
            return new C4d(
                (double)(c0.R - Col.DoubleFromUShort(c1.R)), 
                (double)(c0.G - Col.DoubleFromUShort(c1.G)), 
                (double)(c0.B - Col.DoubleFromUShort(c1.B)), 
                (double)(c0.A - Col.DoubleFromUShort(c1.A)));
        }

        public static C4d operator +(C4d c0, C4ui c1)
        {
            return new C4d(
                (double)(c0.R + Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G + Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B + Col.DoubleFromUInt(c1.B)), 
                (double)(c0.A + Col.DoubleFromUInt(c1.A)));
        }

        public static C4d operator -(C4d c0, C4ui c1)
        {
            return new C4d(
                (double)(c0.R - Col.DoubleFromUInt(c1.R)), 
                (double)(c0.G - Col.DoubleFromUInt(c1.G)), 
                (double)(c0.B - Col.DoubleFromUInt(c1.B)), 
                (double)(c0.A - Col.DoubleFromUInt(c1.A)));
        }

        public static C4d operator +(C4d c0, C4f c1)
        {
            return new C4d(
                (double)(c0.R + (double)(c1.R)), 
                (double)(c0.G + (double)(c1.G)), 
                (double)(c0.B + (double)(c1.B)), 
                (double)(c0.A + (double)(c1.A)));
        }

        public static C4d operator -(C4d c0, C4f c1)
        {
            return new C4d(
                (double)(c0.R - (double)(c1.R)), 
                (double)(c0.G - (double)(c1.G)), 
                (double)(c0.B - (double)(c1.B)), 
                (double)(c0.A - (double)(c1.A)));
        }

        public static C4d operator +(C4d c0, C4d c1)
        {
            return new C4d(
                (double)(c0.R + (c1.R)), 
                (double)(c0.G + (c1.G)), 
                (double)(c0.B + (c1.B)), 
                (double)(c0.A + (c1.A)));
        }

        public static C4d operator -(C4d c0, C4d c1)
        {
            return new C4d(
                (double)(c0.R - (c1.R)), 
                (double)(c0.G - (c1.G)), 
                (double)(c0.B - (c1.B)), 
                (double)(c0.A - (c1.A)));
        }

        public static C4d operator *(C4d c0, C4d c1)
        {
            return new C4d(
                (double)(c0.R * c1.R), 
                (double)(c0.G * c1.G), 
                (double)(c0.B * c1.B), 
                (double)(c0.A * c1.A));
        }

        public static C4d operator /(C4d c0, C4d c1)
        {
            return new C4d(
                (double)(c0.R / c1.R), 
                (double)(c0.G / c1.G), 
                (double)(c0.B / c1.B), 
                (double)(c0.A / c1.A));
        }

        public static C4d operator +(C4d col, double scalar)
        {
            return new C4d(
                (double)(col.R + scalar), 
                (double)(col.G + scalar), 
                (double)(col.B + scalar), 
                (double)(col.A + scalar));
        }

        public static C4d operator +(double scalar, C4d col)
        {
            return new C4d(
                (double)(scalar + col.R), 
                (double)(scalar + col.G), 
                (double)(scalar + col.B), 
                (double)(scalar + col.A));
        }

        public static C4d operator -(C4d col, double scalar)
        {
            return new C4d(
                (double)(col.R - scalar), 
                (double)(col.G - scalar), 
                (double)(col.B - scalar), 
                (double)(col.A - scalar));
        }

        public static C4d operator -(double scalar, C4d col)
        {
            return new C4d(
                (double)(scalar - col.R), 
                (double)(scalar - col.G), 
                (double)(scalar - col.B), 
                (double)(scalar - col.A));
        }

        public static V3d operator + (V3d vec, C4d color)
        {
            return new V3d(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B)
                );
        }

        public static V3d operator -(V3d vec, C4d color)
        {
            return new V3d(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B)
                );
        }

        public static V4d operator + (V4d vec, C4d color)
        {
            return new V4d(
                vec.X + (color.R), 
                vec.Y + (color.G), 
                vec.Z + (color.B),
                vec.W + (color.A)
                );
        }

        public static V4d operator -(V4d vec, C4d color)
        {
            return new V4d(
                vec.X - (color.R), 
                vec.Y - (color.G), 
                vec.Z - (color.B),
                vec.W - (color.A)
                );
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            R = R.Clamp(min, max);
            G = G.Clamp(min, max);
            B = B.Clamp(min, max);
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
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
            get { return (double)Fun.Abs(R) + (double)Fun.Abs(G) + (double)Fun.Abs(B); }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). The alpha channel is ignored.
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt((double)R * (double)R + (double)G * (double)G + (double)B * (double)B); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public double NormMax
        {
            get { return Fun.Max(Fun.Abs(R), Fun.Abs(G), Fun.Abs(B)); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). The alpha channel is ignored.
        /// </summary>
        public double NormMin
        {
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
            C4d p0, C4d p1, C4d p2, C4d p3, ref Tup4<float> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4d p0, C4d p1, C4d p2, C4d p3, ref Tup4<float> w)
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
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, ref Tup4<double> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4d p0, C4d p1, C4d p2, C4d p3, ref Tup4<double> w)
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
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<float> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4f LinComRawF(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<float> w)
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
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<double> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static C4d LinComRawD(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
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

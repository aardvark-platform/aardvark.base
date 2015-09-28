using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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

        public static readonly Func<C3us, C3b> FromC3us = c => new C3b(c);
        public static readonly Func<C3ui, C3b> FromC3ui = c => new C3b(c);
        public static readonly Func<C3f, C3b> FromC3f = c => new C3b(c);
        public static readonly Func<C3d, C3b> FromC3d = c => new C3b(c);
        public static readonly Func<C4b, C3b> FromC4b = c => new C3b(c);
        public static readonly Func<C4us, C3b> FromC4us = c => new C3b(c);
        public static readonly Func<C4ui, C3b> FromC4ui = c => new C3b(c);
        public static readonly Func<C4f, C3b> FromC4f = c => new C3b(c);
        public static readonly Func<C4d, C3b> FromC4d = c => new C3b(c);

        public static readonly Func<V3i, C3b> FromV3i = c => new C3b(c);
        public static readonly Func<V3l, C3b> FromV3l = c => new C3b(c);
        public static readonly Func<V4i, C3b> FromV4i = c => new C3b(c);
        public static readonly Func<V4l, C3b> FromV4l = c => new C3b(c);

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

        public static readonly C3b Black = new C3b(0);

        public static readonly C3b Red = new C3b(255, 0, 0);
        public static readonly C3b Green = new C3b(0, 255, 0);
        public static readonly C3b Blue = new C3b(0, 0, 255);
        public static readonly C3b Cyan = new C3b(0, 255, 255);
        public static readonly C3b Magenta = new C3b(255, 0, 255);
        public static readonly C3b Yellow = new C3b(255, 255, 0);
        public static readonly C3b White = new C3b(255);

        public static readonly C3b DarkRed = new C3b(255 / 2, 0 / 2, 0 / 2);
        public static readonly C3b DarkGreen = new C3b(0 / 2, 255 / 2, 0 / 2);
        public static readonly C3b DarkBlue = new C3b(0 / 2, 0 / 2, 255 / 2);
        public static readonly C3b DarkCyan = new C3b(0 / 2, 255 / 2, 255 / 2);
        public static readonly C3b DarkMagenta = new C3b(255 / 2, 0 / 2, 255 / 2);
        public static readonly C3b DarkYellow = new C3b(255 / 2, 255 / 2, 0 / 2);
        public static readonly C3b Gray = new C3b(255 / 2);
        
        public static readonly C3b VRVisGreen = new C3b(178, 217, 2);

        #endregion

        #region Comparison Operators

        public static bool operator ==(C3b a, C3b b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(C3b a, C3b b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3b a, C3b b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C3b col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3b col, byte s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(byte s)
        {
            return (this.R < s && this.G < s && this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(byte s, C3b col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3b a, C3b b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C3b col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3b col, byte s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(byte s)
        {
            return (this.R < s || this.G < s || this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(byte s, C3b col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3b a, C3b b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C3b col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3b col, byte s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(byte s)
        {
            return (this.R > s && this.G > s && this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(byte s, C3b col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3b a, C3b b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C3b col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3b col, byte s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(byte s)
        {
            return (this.R > s || this.G > s || this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(byte s, C3b col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3b a, C3b b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C3b col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3b col, byte s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(byte s)
        {
            return (this.R <= s && this.G <= s && this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(byte s, C3b col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3b a, C3b b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C3b col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3b col, byte s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(byte s)
        {
            return (this.R <= s || this.G <= s || this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(byte s, C3b col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3b a, C3b b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C3b col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3b col, byte s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(byte s)
        {
            return (this.R >= s && this.G >= s && this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(byte s, C3b col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3b a, C3b b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C3b col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3b col, byte s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(byte s)
        {
            return (this.R >= s || this.G >= s || this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(byte s, C3b col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3b a, C3b b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C3b col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3b col, byte s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(byte s)
        {
            return (this.R == s && this.G == s && this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(byte s, C3b col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3b a, C3b b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C3b col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3b col, byte s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(byte s)
        {
            return (this.R == s || this.G == s || this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(byte s, C3b col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3b a, C3b b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C3b col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3b col, byte s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(byte s)
        {
            return (this.R != s && this.G != s && this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(byte s, C3b col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3b a, C3b b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C3b col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3b col, byte s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(byte s)
        {
            return (this.R != s || this.G != s || this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(byte s, C3b col)
        {
            return (s != col.R || s != col.G || s != col.B);
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
        {
            return (other is C3b) ? this == (C3b)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, ref Tup4<float> w)
        {
            return new C3b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3f LinComRawC3f(
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
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, ref Tup4<double> w)
        {
            return new C3b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3d LinComRawC3d(
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
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, C3b p4, C3b p5, ref Tup6<float> w)
        {
            return new C3b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3f LinComRawC3f(
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
        public static C3b LinCom(
            C3b p0, C3b p1, C3b p2, C3b p3, C3b p4, C3b p5, ref Tup6<double> w)
        {
            return new C3b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3d LinComRawC3d(
            C3b p0, C3b p1, C3b p2, C3b p3, C3b p4, C3b p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                byte.Parse(x[0], Localization.FormatEnUS), 
                byte.Parse(x[1], Localization.FormatEnUS), 
                byte.Parse(x[2], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3b> Members

        public bool Equals(C3b other)
        {
            return R == other.R && G == other.G && B == other.B;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3b Lerp(this double x, C3b a, C3b b)
        {
            return new C3b(
                        (byte)((int)a.R + (int)(x * (double)((int)b.R - (int)a.R))), 
                        (byte)((int)a.G + (int)(x * (double)((int)b.G - (int)a.G))), 
                        (byte)((int)a.B + (int)(x * (double)((int)b.B - (int)a.B))));
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

        public static readonly Func<C3b, C3us> FromC3b = c => new C3us(c);
        public static readonly Func<C3ui, C3us> FromC3ui = c => new C3us(c);
        public static readonly Func<C3f, C3us> FromC3f = c => new C3us(c);
        public static readonly Func<C3d, C3us> FromC3d = c => new C3us(c);
        public static readonly Func<C4b, C3us> FromC4b = c => new C3us(c);
        public static readonly Func<C4us, C3us> FromC4us = c => new C3us(c);
        public static readonly Func<C4ui, C3us> FromC4ui = c => new C3us(c);
        public static readonly Func<C4f, C3us> FromC4f = c => new C3us(c);
        public static readonly Func<C4d, C3us> FromC4d = c => new C3us(c);

        public static readonly Func<V3i, C3us> FromV3i = c => new C3us(c);
        public static readonly Func<V3l, C3us> FromV3l = c => new C3us(c);
        public static readonly Func<V4i, C3us> FromV4i = c => new C3us(c);
        public static readonly Func<V4l, C3us> FromV4l = c => new C3us(c);

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

        public static readonly C3us Black = new C3us(0);

        public static readonly C3us Red = new C3us(65535, 0, 0);
        public static readonly C3us Green = new C3us(0, 65535, 0);
        public static readonly C3us Blue = new C3us(0, 0, 65535);
        public static readonly C3us Cyan = new C3us(0, 65535, 65535);
        public static readonly C3us Magenta = new C3us(65535, 0, 65535);
        public static readonly C3us Yellow = new C3us(65535, 65535, 0);
        public static readonly C3us White = new C3us(65535);

        public static readonly C3us DarkRed = new C3us(65535 / 2, 0 / 2, 0 / 2);
        public static readonly C3us DarkGreen = new C3us(0 / 2, 65535 / 2, 0 / 2);
        public static readonly C3us DarkBlue = new C3us(0 / 2, 0 / 2, 65535 / 2);
        public static readonly C3us DarkCyan = new C3us(0 / 2, 65535 / 2, 65535 / 2);
        public static readonly C3us DarkMagenta = new C3us(65535 / 2, 0 / 2, 65535 / 2);
        public static readonly C3us DarkYellow = new C3us(65535 / 2, 65535 / 2, 0 / 2);
        public static readonly C3us Gray = new C3us(65535 / 2);
        
        public static readonly C3us VRVisGreen = new C3us(45743, 53411, 5243);

        #endregion

        #region Comparison Operators

        public static bool operator ==(C3us a, C3us b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(C3us a, C3us b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3us a, C3us b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C3us col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3us col, ushort s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(ushort s)
        {
            return (this.R < s && this.G < s && this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(ushort s, C3us col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3us a, C3us b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C3us col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3us col, ushort s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(ushort s)
        {
            return (this.R < s || this.G < s || this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(ushort s, C3us col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3us a, C3us b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C3us col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3us col, ushort s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(ushort s)
        {
            return (this.R > s && this.G > s && this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(ushort s, C3us col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3us a, C3us b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C3us col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3us col, ushort s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(ushort s)
        {
            return (this.R > s || this.G > s || this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(ushort s, C3us col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3us a, C3us b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C3us col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3us col, ushort s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(ushort s)
        {
            return (this.R <= s && this.G <= s && this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(ushort s, C3us col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3us a, C3us b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C3us col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3us col, ushort s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(ushort s)
        {
            return (this.R <= s || this.G <= s || this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(ushort s, C3us col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3us a, C3us b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C3us col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3us col, ushort s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(ushort s)
        {
            return (this.R >= s && this.G >= s && this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(ushort s, C3us col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3us a, C3us b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C3us col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3us col, ushort s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(ushort s)
        {
            return (this.R >= s || this.G >= s || this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(ushort s, C3us col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3us a, C3us b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C3us col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3us col, ushort s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(ushort s)
        {
            return (this.R == s && this.G == s && this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(ushort s, C3us col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3us a, C3us b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C3us col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3us col, ushort s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(ushort s)
        {
            return (this.R == s || this.G == s || this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(ushort s, C3us col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3us a, C3us b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C3us col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3us col, ushort s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(ushort s)
        {
            return (this.R != s && this.G != s && this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(ushort s, C3us col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3us a, C3us b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C3us col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3us col, ushort s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(ushort s)
        {
            return (this.R != s || this.G != s || this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(ushort s, C3us col)
        {
            return (s != col.R || s != col.G || s != col.B);
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
        {
            return (other is C3us) ? this == (C3us)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, ref Tup4<float> w)
        {
            return new C3us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3f LinComRawC3f(
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
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, ref Tup4<double> w)
        {
            return new C3us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3d LinComRawC3d(
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
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, C3us p4, C3us p5, ref Tup6<float> w)
        {
            return new C3us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3f LinComRawC3f(
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
        public static C3us LinCom(
            C3us p0, C3us p1, C3us p2, C3us p3, C3us p4, C3us p5, ref Tup6<double> w)
        {
            return new C3us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3d LinComRawC3d(
            C3us p0, C3us p1, C3us p2, C3us p3, C3us p4, C3us p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                ushort.Parse(x[0], Localization.FormatEnUS), 
                ushort.Parse(x[1], Localization.FormatEnUS), 
                ushort.Parse(x[2], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3us> Members

        public bool Equals(C3us other)
        {
            return R == other.R && G == other.G && B == other.B;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3us Lerp(this double x, C3us a, C3us b)
        {
            return new C3us(
                        (ushort)((int)a.R + (int)(x * (double)((int)b.R - (int)a.R))), 
                        (ushort)((int)a.G + (int)(x * (double)((int)b.G - (int)a.G))), 
                        (ushort)((int)a.B + (int)(x * (double)((int)b.B - (int)a.B))));
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

        public static readonly Func<C3b, C3ui> FromC3b = c => new C3ui(c);
        public static readonly Func<C3us, C3ui> FromC3us = c => new C3ui(c);
        public static readonly Func<C3f, C3ui> FromC3f = c => new C3ui(c);
        public static readonly Func<C3d, C3ui> FromC3d = c => new C3ui(c);
        public static readonly Func<C4b, C3ui> FromC4b = c => new C3ui(c);
        public static readonly Func<C4us, C3ui> FromC4us = c => new C3ui(c);
        public static readonly Func<C4ui, C3ui> FromC4ui = c => new C3ui(c);
        public static readonly Func<C4f, C3ui> FromC4f = c => new C3ui(c);
        public static readonly Func<C4d, C3ui> FromC4d = c => new C3ui(c);

        public static readonly Func<V3l, C3ui> FromV3l = c => new C3ui(c);
        public static readonly Func<V4l, C3ui> FromV4l = c => new C3ui(c);

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

        public static readonly C3ui Black = new C3ui(0);

        public static readonly C3ui Red = new C3ui(UInt32.MaxValue, 0, 0);
        public static readonly C3ui Green = new C3ui(0, UInt32.MaxValue, 0);
        public static readonly C3ui Blue = new C3ui(0, 0, UInt32.MaxValue);
        public static readonly C3ui Cyan = new C3ui(0, UInt32.MaxValue, UInt32.MaxValue);
        public static readonly C3ui Magenta = new C3ui(UInt32.MaxValue, 0, UInt32.MaxValue);
        public static readonly C3ui Yellow = new C3ui(UInt32.MaxValue, UInt32.MaxValue, 0);
        public static readonly C3ui White = new C3ui(UInt32.MaxValue);

        public static readonly C3ui DarkRed = new C3ui(UInt32.MaxValue / 2, 0 / 2, 0 / 2);
        public static readonly C3ui DarkGreen = new C3ui(0 / 2, UInt32.MaxValue / 2, 0 / 2);
        public static readonly C3ui DarkBlue = new C3ui(0 / 2, 0 / 2, UInt32.MaxValue / 2);
        public static readonly C3ui DarkCyan = new C3ui(0 / 2, UInt32.MaxValue / 2, UInt32.MaxValue / 2);
        public static readonly C3ui DarkMagenta = new C3ui(UInt32.MaxValue / 2, 0 / 2, UInt32.MaxValue / 2);
        public static readonly C3ui DarkYellow = new C3ui(UInt32.MaxValue / 2, UInt32.MaxValue / 2, 0 / 2);
        public static readonly C3ui Gray = new C3ui(UInt32.MaxValue / 2);
        

        #endregion

        #region Comparison Operators

        public static bool operator ==(C3ui a, C3ui b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(C3ui a, C3ui b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3ui a, C3ui b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C3ui col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3ui col, uint s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(uint s)
        {
            return (this.R < s && this.G < s && this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(uint s, C3ui col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3ui a, C3ui b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C3ui col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3ui col, uint s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(uint s)
        {
            return (this.R < s || this.G < s || this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(uint s, C3ui col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3ui a, C3ui b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C3ui col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3ui col, uint s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(uint s)
        {
            return (this.R > s && this.G > s && this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(uint s, C3ui col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3ui a, C3ui b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C3ui col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3ui col, uint s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(uint s)
        {
            return (this.R > s || this.G > s || this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(uint s, C3ui col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3ui a, C3ui b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C3ui col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3ui col, uint s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(uint s)
        {
            return (this.R <= s && this.G <= s && this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(uint s, C3ui col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3ui a, C3ui b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C3ui col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3ui col, uint s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(uint s)
        {
            return (this.R <= s || this.G <= s || this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(uint s, C3ui col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3ui a, C3ui b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C3ui col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3ui col, uint s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(uint s)
        {
            return (this.R >= s && this.G >= s && this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(uint s, C3ui col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3ui a, C3ui b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C3ui col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3ui col, uint s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(uint s)
        {
            return (this.R >= s || this.G >= s || this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(uint s, C3ui col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3ui a, C3ui b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C3ui col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3ui col, uint s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(uint s)
        {
            return (this.R == s && this.G == s && this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(uint s, C3ui col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3ui a, C3ui b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C3ui col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3ui col, uint s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(uint s)
        {
            return (this.R == s || this.G == s || this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(uint s, C3ui col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3ui a, C3ui b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C3ui col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3ui col, uint s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(uint s)
        {
            return (this.R != s && this.G != s && this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(uint s, C3ui col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3ui a, C3ui b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C3ui col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3ui col, uint s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(uint s)
        {
            return (this.R != s || this.G != s || this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(uint s, C3ui col)
        {
            return (s != col.R || s != col.G || s != col.B);
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
        {
            return (other is C3ui) ? this == (C3ui)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, ref Tup4<float> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3f LinComRawC3f(
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
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, ref Tup4<double> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3d LinComRawC3d(
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
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, C3ui p4, C3ui p5, ref Tup6<float> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3f LinComRawC3f(
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
        public static C3ui LinCom(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, C3ui p4, C3ui p5, ref Tup6<double> w)
        {
            return new C3ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3d LinComRawC3d(
            C3ui p0, C3ui p1, C3ui p2, C3ui p3, C3ui p4, C3ui p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                uint.Parse(x[0], Localization.FormatEnUS), 
                uint.Parse(x[1], Localization.FormatEnUS), 
                uint.Parse(x[2], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3ui> Members

        public bool Equals(C3ui other)
        {
            return R == other.R && G == other.G && B == other.B;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3ui Lerp(this double x, C3ui a, C3ui b)
        {
            return new C3ui(
                        (uint)((long)a.R + (long)(x * (double)((long)b.R - (long)a.R))), 
                        (uint)((long)a.G + (long)(x * (double)((long)b.G - (long)a.G))), 
                        (uint)((long)a.B + (long)(x * (double)((long)b.B - (long)a.B))));
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

        public static readonly Func<C3b, C3f> FromC3b = c => new C3f(c);
        public static readonly Func<C3us, C3f> FromC3us = c => new C3f(c);
        public static readonly Func<C3ui, C3f> FromC3ui = c => new C3f(c);
        public static readonly Func<C3d, C3f> FromC3d = c => new C3f(c);
        public static readonly Func<C4b, C3f> FromC4b = c => new C3f(c);
        public static readonly Func<C4us, C3f> FromC4us = c => new C3f(c);
        public static readonly Func<C4ui, C3f> FromC4ui = c => new C3f(c);
        public static readonly Func<C4f, C3f> FromC4f = c => new C3f(c);
        public static readonly Func<C4d, C3f> FromC4d = c => new C3f(c);

        public static readonly Func<V3f, C3f> FromV3f = c => new C3f(c);
        public static readonly Func<V3d, C3f> FromV3d = c => new C3f(c);
        public static readonly Func<V4f, C3f> FromV4f = c => new C3f(c);
        public static readonly Func<V4d, C3f> FromV4d = c => new C3f(c);

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

        public static readonly C3f Black = new C3f(0.0f);

        public static readonly C3f Red = new C3f(1.0f, 0.0f, 0.0f);
        public static readonly C3f Green = new C3f(0.0f, 1.0f, 0.0f);
        public static readonly C3f Blue = new C3f(0.0f, 0.0f, 1.0f);
        public static readonly C3f Cyan = new C3f(0.0f, 1.0f, 1.0f);
        public static readonly C3f Magenta = new C3f(1.0f, 0.0f, 1.0f);
        public static readonly C3f Yellow = new C3f(1.0f, 1.0f, 0.0f);
        public static readonly C3f White = new C3f(1.0f);

        public static readonly C3f DarkRed = new C3f(1.0f / 2, 0.0f / 2, 0.0f / 2);
        public static readonly C3f DarkGreen = new C3f(0.0f / 2, 1.0f / 2, 0.0f / 2);
        public static readonly C3f DarkBlue = new C3f(0.0f / 2, 0.0f / 2, 1.0f / 2);
        public static readonly C3f DarkCyan = new C3f(0.0f / 2, 1.0f / 2, 1.0f / 2);
        public static readonly C3f DarkMagenta = new C3f(1.0f / 2, 0.0f / 2, 1.0f / 2);
        public static readonly C3f DarkYellow = new C3f(1.0f / 2, 1.0f / 2, 0.0f / 2);
        public static readonly C3f Gray = new C3f(1.0f / 2);
        
        public static readonly C3f Gray10 = new C3f(0.1f);
        public static readonly C3f Gray20 = new C3f(0.2f);
        public static readonly C3f Gray30 = new C3f(0.3f);
        public static readonly C3f Gray40 = new C3f(0.4f);
        public static readonly C3f Gray50 = new C3f(0.5f);
        public static readonly C3f Gray60 = new C3f(0.6f);
        public static readonly C3f Gray70 = new C3f(0.7f);
        public static readonly C3f Gray80 = new C3f(0.8f);
        public static readonly C3f Gray90 = new C3f(0.9f);
        public static readonly C3f VRVisGreen = new C3f(0.698f, 0.851f, 0.008f);

        #endregion

        #region Comparison Operators

        public static bool operator ==(C3f a, C3f b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(C3f a, C3f b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3f a, C3f b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C3f col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3f col, float s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(float s)
        {
            return (this.R < s && this.G < s && this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(float s, C3f col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3f a, C3f b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C3f col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3f col, float s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(float s)
        {
            return (this.R < s || this.G < s || this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(float s, C3f col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3f a, C3f b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C3f col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3f col, float s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(float s)
        {
            return (this.R > s && this.G > s && this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(float s, C3f col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3f a, C3f b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C3f col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3f col, float s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(float s)
        {
            return (this.R > s || this.G > s || this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(float s, C3f col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3f a, C3f b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C3f col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3f col, float s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(float s)
        {
            return (this.R <= s && this.G <= s && this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(float s, C3f col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3f a, C3f b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C3f col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3f col, float s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(float s)
        {
            return (this.R <= s || this.G <= s || this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(float s, C3f col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3f a, C3f b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C3f col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3f col, float s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(float s)
        {
            return (this.R >= s && this.G >= s && this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(float s, C3f col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3f a, C3f b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C3f col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3f col, float s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(float s)
        {
            return (this.R >= s || this.G >= s || this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(float s, C3f col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3f a, C3f b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C3f col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3f col, float s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(float s)
        {
            return (this.R == s && this.G == s && this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(float s, C3f col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3f a, C3f b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C3f col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3f col, float s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(float s)
        {
            return (this.R == s || this.G == s || this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(float s, C3f col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3f a, C3f b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C3f col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3f col, float s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(float s)
        {
            return (this.R != s && this.G != s && this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(float s, C3f col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3f a, C3f b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C3f col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3f col, float s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(float s)
        {
            return (this.R != s || this.G != s || this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(float s, C3f col)
        {
            return (s != col.R || s != col.G || s != col.B);
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
        {
            return (other is C3f) ? this == (C3f)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, ref Tup4<float> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3f LinComRawC3f(
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
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, ref Tup4<double> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3d LinComRawC3d(
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
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<float> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3f LinComRawC3f(
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
        public static C3f LinCom(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<double> w)
        {
            return new C3f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3d LinComRawC3d(
            C3f p0, C3f p1, C3f p2, C3f p3, C3f p4, C3f p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                float.Parse(x[0], Localization.FormatEnUS), 
                float.Parse(x[1], Localization.FormatEnUS), 
                float.Parse(x[2], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3f> Members

        public bool Equals(C3f other)
        {
            return R == other.R && G == other.G && B == other.B;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3f Lerp(this double x, C3f a, C3f b)
        {
            return new C3f(
                        (float)((double)a.R + (x * ((double)b.R - (double)a.R))), 
                        (float)((double)a.G + (x * ((double)b.G - (double)a.G))), 
                        (float)((double)a.B + (x * ((double)b.B - (double)a.B))));
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

        public static readonly Func<C3b, C3d> FromC3b = c => new C3d(c);
        public static readonly Func<C3us, C3d> FromC3us = c => new C3d(c);
        public static readonly Func<C3ui, C3d> FromC3ui = c => new C3d(c);
        public static readonly Func<C3f, C3d> FromC3f = c => new C3d(c);
        public static readonly Func<C4b, C3d> FromC4b = c => new C3d(c);
        public static readonly Func<C4us, C3d> FromC4us = c => new C3d(c);
        public static readonly Func<C4ui, C3d> FromC4ui = c => new C3d(c);
        public static readonly Func<C4f, C3d> FromC4f = c => new C3d(c);
        public static readonly Func<C4d, C3d> FromC4d = c => new C3d(c);

        public static readonly Func<V3d, C3d> FromV3d = c => new C3d(c);
        public static readonly Func<V4d, C3d> FromV4d = c => new C3d(c);

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

        public static readonly C3d Black = new C3d(0);

        public static readonly C3d Red = new C3d(1.0, 0, 0);
        public static readonly C3d Green = new C3d(0, 1.0, 0);
        public static readonly C3d Blue = new C3d(0, 0, 1.0);
        public static readonly C3d Cyan = new C3d(0, 1.0, 1.0);
        public static readonly C3d Magenta = new C3d(1.0, 0, 1.0);
        public static readonly C3d Yellow = new C3d(1.0, 1.0, 0);
        public static readonly C3d White = new C3d(1.0);

        public static readonly C3d DarkRed = new C3d(1.0 / 2, 0 / 2, 0 / 2);
        public static readonly C3d DarkGreen = new C3d(0 / 2, 1.0 / 2, 0 / 2);
        public static readonly C3d DarkBlue = new C3d(0 / 2, 0 / 2, 1.0 / 2);
        public static readonly C3d DarkCyan = new C3d(0 / 2, 1.0 / 2, 1.0 / 2);
        public static readonly C3d DarkMagenta = new C3d(1.0 / 2, 0 / 2, 1.0 / 2);
        public static readonly C3d DarkYellow = new C3d(1.0 / 2, 1.0 / 2, 0 / 2);
        public static readonly C3d Gray = new C3d(1.0 / 2);
        

        #endregion

        #region Comparison Operators

        public static bool operator ==(C3d a, C3d b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(C3d a, C3d b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3d a, C3d b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C3d col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C3d col, double s)
        {
            return (col.R < s && col.G < s && col.B < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(double s)
        {
            return (this.R < s && this.G < s && this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(double s, C3d col)
        {
            return (s < col.R && s < col.G && s < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3d a, C3d b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C3d col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C3d col, double s)
        {
            return (col.R < s || col.G < s || col.B < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(double s)
        {
            return (this.R < s || this.G < s || this.B < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(double s, C3d col)
        {
            return (s < col.R || s < col.G || s < col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3d a, C3d b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C3d col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C3d col, double s)
        {
            return (col.R > s && col.G > s && col.B > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(double s)
        {
            return (this.R > s && this.G > s && this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(double s, C3d col)
        {
            return (s > col.R && s > col.G && s > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3d a, C3d b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C3d col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C3d col, double s)
        {
            return (col.R > s || col.G > s || col.B > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(double s)
        {
            return (this.R > s || this.G > s || this.B > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(double s, C3d col)
        {
            return (s > col.R || s > col.G || s > col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3d a, C3d b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C3d col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C3d col, double s)
        {
            return (col.R <= s && col.G <= s && col.B <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(double s)
        {
            return (this.R <= s && this.G <= s && this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(double s, C3d col)
        {
            return (s <= col.R && s <= col.G && s <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3d a, C3d b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C3d col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C3d col, double s)
        {
            return (col.R <= s || col.G <= s || col.B <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(double s)
        {
            return (this.R <= s || this.G <= s || this.B <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(double s, C3d col)
        {
            return (s <= col.R || s <= col.G || s <= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3d a, C3d b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C3d col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C3d col, double s)
        {
            return (col.R >= s && col.G >= s && col.B >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(double s)
        {
            return (this.R >= s && this.G >= s && this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(double s, C3d col)
        {
            return (s >= col.R && s >= col.G && s >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3d a, C3d b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C3d col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C3d col, double s)
        {
            return (col.R >= s || col.G >= s || col.B >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(double s)
        {
            return (this.R >= s || this.G >= s || this.B >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(double s, C3d col)
        {
            return (s >= col.R || s >= col.G || s >= col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3d a, C3d b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C3d col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C3d col, double s)
        {
            return (col.R == s && col.G == s && col.B == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(double s)
        {
            return (this.R == s && this.G == s && this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(double s, C3d col)
        {
            return (s == col.R && s == col.G && s == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3d a, C3d b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C3d col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C3d col, double s)
        {
            return (col.R == s || col.G == s || col.B == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(double s)
        {
            return (this.R == s || this.G == s || this.B == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(double s, C3d col)
        {
            return (s == col.R || s == col.G || s == col.B);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3d a, C3d b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C3d col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C3d col, double s)
        {
            return (col.R != s && col.G != s && col.B != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(double s)
        {
            return (this.R != s && this.G != s && this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(double s, C3d col)
        {
            return (s != col.R && s != col.G && s != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3d a, C3d b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C3d col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C3d col, double s)
        {
            return (col.R != s || col.G != s || col.B != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(double s)
        {
            return (this.R != s || this.G != s || this.B != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(double s, C3d col)
        {
            return (s != col.R || s != col.G || s != col.B);
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
        {
            return (other is C3d) ? this == (C3d)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, ref Tup4<float> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3f LinComRawC3f(
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
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, ref Tup4<double> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C3d LinComRawC3d(
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
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<float> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3f LinComRawC3f(
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
        public static C3d LinCom(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<double> w)
        {
            return new C3d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C3d LinComRawC3d(
            C3d p0, C3d p1, C3d p2, C3d p3, C3d p4, C3d p5, ref Tup6<double> w)
        {
            return new C3d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                double.Parse(x[0], Localization.FormatEnUS), 
                double.Parse(x[1], Localization.FormatEnUS), 
                double.Parse(x[2], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C3d> Members

        public bool Equals(C3d other)
        {
            return R == other.R && G == other.G && B == other.B;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C3d Lerp(this double x, C3d a, C3d b)
        {
            return new C3d(
                        (double)((double)a.R + (x * ((double)b.R - (double)a.R))), 
                        (double)((double)a.G + (x * ((double)b.G - (double)a.G))), 
                        (double)((double)a.B + (x * ((double)b.B - (double)a.B))));
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

        public C4b(C3us color)
        {
            R = Col.ByteFromUShort(color.R);
            G = Col.ByteFromUShort(color.G);
            B = Col.ByteFromUShort(color.B);
            A = 255;
        }

        public C4b(C3ui color)
        {
            R = Col.ByteFromUInt(color.R);
            G = Col.ByteFromUInt(color.G);
            B = Col.ByteFromUInt(color.B);
            A = 255;
        }

        public C4b(C3f color)
        {
            R = Col.ByteFromFloat(color.R);
            G = Col.ByteFromFloat(color.G);
            B = Col.ByteFromFloat(color.B);
            A = 255;
        }

        public C4b(C3d color)
        {
            R = Col.ByteFromDouble(color.R);
            G = Col.ByteFromDouble(color.G);
            B = Col.ByteFromDouble(color.B);
            A = 255;
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

        public C4b(V3l vec)
        {
            R = (byte)(vec.X);
            G = (byte)(vec.Y);
            B = (byte)(vec.Z);
            A = 255;
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

        public static readonly Func<C3b, C4b> FromC3b = c => new C4b(c);
        public static readonly Func<C3us, C4b> FromC3us = c => new C4b(c);
        public static readonly Func<C3ui, C4b> FromC3ui = c => new C4b(c);
        public static readonly Func<C3f, C4b> FromC3f = c => new C4b(c);
        public static readonly Func<C3d, C4b> FromC3d = c => new C4b(c);
        public static readonly Func<C4us, C4b> FromC4us = c => new C4b(c);
        public static readonly Func<C4ui, C4b> FromC4ui = c => new C4b(c);
        public static readonly Func<C4f, C4b> FromC4f = c => new C4b(c);
        public static readonly Func<C4d, C4b> FromC4d = c => new C4b(c);

        public static readonly Func<V3i, C4b> FromV3i = c => new C4b(c);
        public static readonly Func<V3l, C4b> FromV3l = c => new C4b(c);
        public static readonly Func<V4i, C4b> FromV4i = c => new C4b(c);
        public static readonly Func<V4l, C4b> FromV4l = c => new C4b(c);

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

        public static readonly C4b Black = new C4b(0);

        public static readonly C4b Red = new C4b(255, 0, 0);
        public static readonly C4b Green = new C4b(0, 255, 0);
        public static readonly C4b Blue = new C4b(0, 0, 255);
        public static readonly C4b Cyan = new C4b(0, 255, 255);
        public static readonly C4b Magenta = new C4b(255, 0, 255);
        public static readonly C4b Yellow = new C4b(255, 255, 0);
        public static readonly C4b White = new C4b(255);

        public static readonly C4b DarkRed = new C4b(255 / 2, 0 / 2, 0 / 2);
        public static readonly C4b DarkGreen = new C4b(0 / 2, 255 / 2, 0 / 2);
        public static readonly C4b DarkBlue = new C4b(0 / 2, 0 / 2, 255 / 2);
        public static readonly C4b DarkCyan = new C4b(0 / 2, 255 / 2, 255 / 2);
        public static readonly C4b DarkMagenta = new C4b(255 / 2, 0 / 2, 255 / 2);
        public static readonly C4b DarkYellow = new C4b(255 / 2, 255 / 2, 0 / 2);
        public static readonly C4b Gray = new C4b(255 / 2);
        
        public static readonly C4b VRVisGreen = new C4b(178, 217, 2);

        #endregion

        #region Comparison Operators

        public static bool operator ==(C4b a, C4b b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(C4b a, C4b b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4b a, C4b b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C4b col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B && this.A < col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4b col, byte s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(byte s)
        {
            return (this.R < s && this.G < s && this.B < s && this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(byte s, C4b col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4b a, C4b b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C4b col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B || this.A < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4b col, byte s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(byte s)
        {
            return (this.R < s || this.G < s || this.B < s || this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(byte s, C4b col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4b a, C4b b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C4b col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B && this.A > col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4b col, byte s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(byte s)
        {
            return (this.R > s && this.G > s && this.B > s && this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(byte s, C4b col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4b a, C4b b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C4b col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B || this.A > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4b col, byte s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(byte s)
        {
            return (this.R > s || this.G > s || this.B > s || this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(byte s, C4b col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4b a, C4b b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C4b col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B && this.A <= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4b col, byte s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(byte s)
        {
            return (this.R <= s && this.G <= s && this.B <= s && this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(byte s, C4b col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4b a, C4b b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C4b col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B || this.A <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4b col, byte s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(byte s)
        {
            return (this.R <= s || this.G <= s || this.B <= s || this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(byte s, C4b col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4b a, C4b b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C4b col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B && this.A >= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4b col, byte s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(byte s)
        {
            return (this.R >= s && this.G >= s && this.B >= s && this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(byte s, C4b col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4b a, C4b b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C4b col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B || this.A >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4b col, byte s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(byte s)
        {
            return (this.R >= s || this.G >= s || this.B >= s || this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(byte s, C4b col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4b a, C4b b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C4b col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B && this.A == col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4b col, byte s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(byte s)
        {
            return (this.R == s && this.G == s && this.B == s && this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(byte s, C4b col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4b a, C4b b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C4b col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B || this.A == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4b col, byte s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(byte s)
        {
            return (this.R == s || this.G == s || this.B == s || this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(byte s, C4b col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4b a, C4b b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C4b col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B && this.A != col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4b col, byte s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(byte s)
        {
            return (this.R != s && this.G != s && this.B != s && this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(byte s, C4b col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4b a, C4b b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C4b col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B || this.A != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4b col, byte s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(byte s)
        {
            return (this.R != s || this.G != s || this.B != s || this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(byte s, C4b col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
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
        {
            return (other is C4b) ? this == (C4b)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS)  + ", " 
                + A.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, ref Tup4<float> w)
        {
            return new C4b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4f LinComRawC4f(
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
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, ref Tup4<double> w)
        {
            return new C4b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4d LinComRawC4d(
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
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, C4b p4, C4b p5, ref Tup6<float> w)
        {
            return new C4b(
                Col.ByteFromByteInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4f LinComRawC4f(
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
        public static C4b LinCom(
            C4b p0, C4b p1, C4b p2, C4b p3, C4b p4, C4b p5, ref Tup6<double> w)
        {
            return new C4b(
                Col.ByteFromByteInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.ByteFromByteInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4d LinComRawC4d(
            C4b p0, C4b p1, C4b p2, C4b p3, C4b p4, C4b p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                byte.Parse(x[0], Localization.FormatEnUS), 
                byte.Parse(x[1], Localization.FormatEnUS), 
                byte.Parse(x[2], Localization.FormatEnUS), 
                byte.Parse(x[3], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4b> Members

        public bool Equals(C4b other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4b Lerp(this double x, C4b a, C4b b)
        {
            return new C4b(
                        (byte)((int)a.R + (int)(x * (double)((int)b.R - (int)a.R))), 
                        (byte)((int)a.G + (int)(x * (double)((int)b.G - (int)a.G))), 
                        (byte)((int)a.B + (int)(x * (double)((int)b.B - (int)a.B))), 
                        (byte)((int)a.A + (int)(x * (double)((int)b.A - (int)a.A))));
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

        public C4us(C3us color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 65535;
        }

        public C4us(C3ui color)
        {
            R = Col.UShortFromUInt(color.R);
            G = Col.UShortFromUInt(color.G);
            B = Col.UShortFromUInt(color.B);
            A = 65535;
        }

        public C4us(C3f color)
        {
            R = Col.UShortFromFloat(color.R);
            G = Col.UShortFromFloat(color.G);
            B = Col.UShortFromFloat(color.B);
            A = 65535;
        }

        public C4us(C3d color)
        {
            R = Col.UShortFromDouble(color.R);
            G = Col.UShortFromDouble(color.G);
            B = Col.UShortFromDouble(color.B);
            A = 65535;
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

        public C4us(V3l vec)
        {
            R = (ushort)(vec.X);
            G = (ushort)(vec.Y);
            B = (ushort)(vec.Z);
            A = 65535;
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

        public static readonly Func<C3b, C4us> FromC3b = c => new C4us(c);
        public static readonly Func<C3us, C4us> FromC3us = c => new C4us(c);
        public static readonly Func<C3ui, C4us> FromC3ui = c => new C4us(c);
        public static readonly Func<C3f, C4us> FromC3f = c => new C4us(c);
        public static readonly Func<C3d, C4us> FromC3d = c => new C4us(c);
        public static readonly Func<C4b, C4us> FromC4b = c => new C4us(c);
        public static readonly Func<C4ui, C4us> FromC4ui = c => new C4us(c);
        public static readonly Func<C4f, C4us> FromC4f = c => new C4us(c);
        public static readonly Func<C4d, C4us> FromC4d = c => new C4us(c);

        public static readonly Func<V3i, C4us> FromV3i = c => new C4us(c);
        public static readonly Func<V3l, C4us> FromV3l = c => new C4us(c);
        public static readonly Func<V4i, C4us> FromV4i = c => new C4us(c);
        public static readonly Func<V4l, C4us> FromV4l = c => new C4us(c);

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

        public static readonly C4us Black = new C4us(0);

        public static readonly C4us Red = new C4us(65535, 0, 0);
        public static readonly C4us Green = new C4us(0, 65535, 0);
        public static readonly C4us Blue = new C4us(0, 0, 65535);
        public static readonly C4us Cyan = new C4us(0, 65535, 65535);
        public static readonly C4us Magenta = new C4us(65535, 0, 65535);
        public static readonly C4us Yellow = new C4us(65535, 65535, 0);
        public static readonly C4us White = new C4us(65535);

        public static readonly C4us DarkRed = new C4us(65535 / 2, 0 / 2, 0 / 2);
        public static readonly C4us DarkGreen = new C4us(0 / 2, 65535 / 2, 0 / 2);
        public static readonly C4us DarkBlue = new C4us(0 / 2, 0 / 2, 65535 / 2);
        public static readonly C4us DarkCyan = new C4us(0 / 2, 65535 / 2, 65535 / 2);
        public static readonly C4us DarkMagenta = new C4us(65535 / 2, 0 / 2, 65535 / 2);
        public static readonly C4us DarkYellow = new C4us(65535 / 2, 65535 / 2, 0 / 2);
        public static readonly C4us Gray = new C4us(65535 / 2);
        
        public static readonly C4us VRVisGreen = new C4us(45743, 53411, 5243);

        #endregion

        #region Comparison Operators

        public static bool operator ==(C4us a, C4us b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(C4us a, C4us b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4us a, C4us b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C4us col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B && this.A < col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4us col, ushort s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(ushort s)
        {
            return (this.R < s && this.G < s && this.B < s && this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(ushort s, C4us col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4us a, C4us b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C4us col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B || this.A < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4us col, ushort s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(ushort s)
        {
            return (this.R < s || this.G < s || this.B < s || this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(ushort s, C4us col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4us a, C4us b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C4us col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B && this.A > col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4us col, ushort s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(ushort s)
        {
            return (this.R > s && this.G > s && this.B > s && this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(ushort s, C4us col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4us a, C4us b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C4us col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B || this.A > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4us col, ushort s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(ushort s)
        {
            return (this.R > s || this.G > s || this.B > s || this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(ushort s, C4us col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4us a, C4us b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C4us col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B && this.A <= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4us col, ushort s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(ushort s)
        {
            return (this.R <= s && this.G <= s && this.B <= s && this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(ushort s, C4us col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4us a, C4us b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C4us col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B || this.A <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4us col, ushort s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(ushort s)
        {
            return (this.R <= s || this.G <= s || this.B <= s || this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(ushort s, C4us col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4us a, C4us b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C4us col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B && this.A >= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4us col, ushort s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(ushort s)
        {
            return (this.R >= s && this.G >= s && this.B >= s && this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(ushort s, C4us col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4us a, C4us b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C4us col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B || this.A >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4us col, ushort s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(ushort s)
        {
            return (this.R >= s || this.G >= s || this.B >= s || this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(ushort s, C4us col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4us a, C4us b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C4us col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B && this.A == col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4us col, ushort s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(ushort s)
        {
            return (this.R == s && this.G == s && this.B == s && this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(ushort s, C4us col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4us a, C4us b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C4us col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B || this.A == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4us col, ushort s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(ushort s)
        {
            return (this.R == s || this.G == s || this.B == s || this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(ushort s, C4us col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4us a, C4us b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C4us col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B && this.A != col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4us col, ushort s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(ushort s)
        {
            return (this.R != s && this.G != s && this.B != s && this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(ushort s, C4us col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4us a, C4us b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C4us col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B || this.A != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4us col, ushort s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(ushort s)
        {
            return (this.R != s || this.G != s || this.B != s || this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(ushort s, C4us col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
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
        {
            return (other is C4us) ? this == (C4us)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS)  + ", " 
                + A.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, ref Tup4<float> w)
        {
            return new C4us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4f LinComRawC4f(
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
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, ref Tup4<double> w)
        {
            return new C4us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4d LinComRawC4d(
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
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, C4us p4, C4us p5, ref Tup6<float> w)
        {
            return new C4us(
                Col.UShortFromUShortInFloatClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInFloatClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4f LinComRawC4f(
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
        public static C4us LinCom(
            C4us p0, C4us p1, C4us p2, C4us p3, C4us p4, C4us p5, ref Tup6<double> w)
        {
            return new C4us(
                Col.UShortFromUShortInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UShortFromUShortInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4d LinComRawC4d(
            C4us p0, C4us p1, C4us p2, C4us p3, C4us p4, C4us p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                ushort.Parse(x[0], Localization.FormatEnUS), 
                ushort.Parse(x[1], Localization.FormatEnUS), 
                ushort.Parse(x[2], Localization.FormatEnUS), 
                ushort.Parse(x[3], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4us> Members

        public bool Equals(C4us other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4us Lerp(this double x, C4us a, C4us b)
        {
            return new C4us(
                        (ushort)((int)a.R + (int)(x * (double)((int)b.R - (int)a.R))), 
                        (ushort)((int)a.G + (int)(x * (double)((int)b.G - (int)a.G))), 
                        (ushort)((int)a.B + (int)(x * (double)((int)b.B - (int)a.B))), 
                        (ushort)((int)a.A + (int)(x * (double)((int)b.A - (int)a.A))));
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

        public C4ui(C3us color)
        {
            R = Col.UIntFromUShort(color.R);
            G = Col.UIntFromUShort(color.G);
            B = Col.UIntFromUShort(color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3ui color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3f color)
        {
            R = Col.UIntFromFloat(color.R);
            G = Col.UIntFromFloat(color.G);
            B = Col.UIntFromFloat(color.B);
            A = UInt32.MaxValue;
        }

        public C4ui(C3d color)
        {
            R = Col.UIntFromDouble(color.R);
            G = Col.UIntFromDouble(color.G);
            B = Col.UIntFromDouble(color.B);
            A = UInt32.MaxValue;
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

        public static readonly Func<C3b, C4ui> FromC3b = c => new C4ui(c);
        public static readonly Func<C3us, C4ui> FromC3us = c => new C4ui(c);
        public static readonly Func<C3ui, C4ui> FromC3ui = c => new C4ui(c);
        public static readonly Func<C3f, C4ui> FromC3f = c => new C4ui(c);
        public static readonly Func<C3d, C4ui> FromC3d = c => new C4ui(c);
        public static readonly Func<C4b, C4ui> FromC4b = c => new C4ui(c);
        public static readonly Func<C4us, C4ui> FromC4us = c => new C4ui(c);
        public static readonly Func<C4f, C4ui> FromC4f = c => new C4ui(c);
        public static readonly Func<C4d, C4ui> FromC4d = c => new C4ui(c);

        public static readonly Func<V3l, C4ui> FromV3l = c => new C4ui(c);
        public static readonly Func<V4l, C4ui> FromV4l = c => new C4ui(c);

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

        public static readonly C4ui Black = new C4ui(0);

        public static readonly C4ui Red = new C4ui(UInt32.MaxValue, 0, 0);
        public static readonly C4ui Green = new C4ui(0, UInt32.MaxValue, 0);
        public static readonly C4ui Blue = new C4ui(0, 0, UInt32.MaxValue);
        public static readonly C4ui Cyan = new C4ui(0, UInt32.MaxValue, UInt32.MaxValue);
        public static readonly C4ui Magenta = new C4ui(UInt32.MaxValue, 0, UInt32.MaxValue);
        public static readonly C4ui Yellow = new C4ui(UInt32.MaxValue, UInt32.MaxValue, 0);
        public static readonly C4ui White = new C4ui(UInt32.MaxValue);

        public static readonly C4ui DarkRed = new C4ui(UInt32.MaxValue / 2, 0 / 2, 0 / 2);
        public static readonly C4ui DarkGreen = new C4ui(0 / 2, UInt32.MaxValue / 2, 0 / 2);
        public static readonly C4ui DarkBlue = new C4ui(0 / 2, 0 / 2, UInt32.MaxValue / 2);
        public static readonly C4ui DarkCyan = new C4ui(0 / 2, UInt32.MaxValue / 2, UInt32.MaxValue / 2);
        public static readonly C4ui DarkMagenta = new C4ui(UInt32.MaxValue / 2, 0 / 2, UInt32.MaxValue / 2);
        public static readonly C4ui DarkYellow = new C4ui(UInt32.MaxValue / 2, UInt32.MaxValue / 2, 0 / 2);
        public static readonly C4ui Gray = new C4ui(UInt32.MaxValue / 2);
        

        #endregion

        #region Comparison Operators

        public static bool operator ==(C4ui a, C4ui b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(C4ui a, C4ui b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4ui a, C4ui b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C4ui col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B && this.A < col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4ui col, uint s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(uint s)
        {
            return (this.R < s && this.G < s && this.B < s && this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(uint s, C4ui col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4ui a, C4ui b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C4ui col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B || this.A < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4ui col, uint s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(uint s)
        {
            return (this.R < s || this.G < s || this.B < s || this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(uint s, C4ui col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4ui a, C4ui b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C4ui col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B && this.A > col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4ui col, uint s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(uint s)
        {
            return (this.R > s && this.G > s && this.B > s && this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(uint s, C4ui col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4ui a, C4ui b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C4ui col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B || this.A > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4ui col, uint s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(uint s)
        {
            return (this.R > s || this.G > s || this.B > s || this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(uint s, C4ui col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4ui a, C4ui b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C4ui col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B && this.A <= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4ui col, uint s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(uint s)
        {
            return (this.R <= s && this.G <= s && this.B <= s && this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(uint s, C4ui col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4ui a, C4ui b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C4ui col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B || this.A <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4ui col, uint s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(uint s)
        {
            return (this.R <= s || this.G <= s || this.B <= s || this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(uint s, C4ui col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4ui a, C4ui b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C4ui col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B && this.A >= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4ui col, uint s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(uint s)
        {
            return (this.R >= s && this.G >= s && this.B >= s && this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(uint s, C4ui col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4ui a, C4ui b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C4ui col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B || this.A >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4ui col, uint s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(uint s)
        {
            return (this.R >= s || this.G >= s || this.B >= s || this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(uint s, C4ui col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4ui a, C4ui b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C4ui col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B && this.A == col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4ui col, uint s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(uint s)
        {
            return (this.R == s && this.G == s && this.B == s && this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(uint s, C4ui col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4ui a, C4ui b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C4ui col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B || this.A == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4ui col, uint s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(uint s)
        {
            return (this.R == s || this.G == s || this.B == s || this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(uint s, C4ui col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4ui a, C4ui b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C4ui col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B && this.A != col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4ui col, uint s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(uint s)
        {
            return (this.R != s && this.G != s && this.B != s && this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(uint s, C4ui col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4ui a, C4ui b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C4ui col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B || this.A != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4ui col, uint s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(uint s)
        {
            return (this.R != s || this.G != s || this.B != s || this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(uint s, C4ui col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
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
        {
            return (other is C4ui) ? this == (C4ui)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS)  + ", " 
                + A.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, ref Tup4<float> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4f LinComRawC4f(
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
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, ref Tup4<double> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4d LinComRawC4d(
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
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, C4ui p4, C4ui p5, ref Tup6<float> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4f LinComRawC4f(
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
        public static C4ui LinCom(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, C4ui p4, C4ui p5, ref Tup6<double> w)
        {
            return new C4ui(
                Col.UIntFromUIntInDoubleClamped(p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                Col.UIntFromUIntInDoubleClamped(p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4d LinComRawC4d(
            C4ui p0, C4ui p1, C4ui p2, C4ui p3, C4ui p4, C4ui p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                uint.Parse(x[0], Localization.FormatEnUS), 
                uint.Parse(x[1], Localization.FormatEnUS), 
                uint.Parse(x[2], Localization.FormatEnUS), 
                uint.Parse(x[3], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4ui> Members

        public bool Equals(C4ui other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4ui Lerp(this double x, C4ui a, C4ui b)
        {
            return new C4ui(
                        (uint)((long)a.R + (long)(x * (double)((long)b.R - (long)a.R))), 
                        (uint)((long)a.G + (long)(x * (double)((long)b.G - (long)a.G))), 
                        (uint)((long)a.B + (long)(x * (double)((long)b.B - (long)a.B))), 
                        (uint)((long)a.A + (long)(x * (double)((long)b.A - (long)a.A))));
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

        public C4f(C3us color)
        {
            R = Col.FloatFromUShort(color.R);
            G = Col.FloatFromUShort(color.G);
            B = Col.FloatFromUShort(color.B);
            A = 1.0f;
        }

        public C4f(C3ui color)
        {
            R = Col.FloatFromUInt(color.R);
            G = Col.FloatFromUInt(color.G);
            B = Col.FloatFromUInt(color.B);
            A = 1.0f;
        }

        public C4f(C3f color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 1.0f;
        }

        public C4f(C3d color)
        {
            R = Col.FloatFromDouble(color.R);
            G = Col.FloatFromDouble(color.G);
            B = Col.FloatFromDouble(color.B);
            A = 1.0f;
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

        public C4f(V3d vec)
        {
            R = (float)(vec.X);
            G = (float)(vec.Y);
            B = (float)(vec.Z);
            A = 1.0f;
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

        public static readonly Func<C3b, C4f> FromC3b = c => new C4f(c);
        public static readonly Func<C3us, C4f> FromC3us = c => new C4f(c);
        public static readonly Func<C3ui, C4f> FromC3ui = c => new C4f(c);
        public static readonly Func<C3f, C4f> FromC3f = c => new C4f(c);
        public static readonly Func<C3d, C4f> FromC3d = c => new C4f(c);
        public static readonly Func<C4b, C4f> FromC4b = c => new C4f(c);
        public static readonly Func<C4us, C4f> FromC4us = c => new C4f(c);
        public static readonly Func<C4ui, C4f> FromC4ui = c => new C4f(c);
        public static readonly Func<C4d, C4f> FromC4d = c => new C4f(c);

        public static readonly Func<V3f, C4f> FromV3f = c => new C4f(c);
        public static readonly Func<V3d, C4f> FromV3d = c => new C4f(c);
        public static readonly Func<V4f, C4f> FromV4f = c => new C4f(c);
        public static readonly Func<V4d, C4f> FromV4d = c => new C4f(c);

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

        public static readonly C4f Black = new C4f(0.0f);

        public static readonly C4f Red = new C4f(1.0f, 0.0f, 0.0f);
        public static readonly C4f Green = new C4f(0.0f, 1.0f, 0.0f);
        public static readonly C4f Blue = new C4f(0.0f, 0.0f, 1.0f);
        public static readonly C4f Cyan = new C4f(0.0f, 1.0f, 1.0f);
        public static readonly C4f Magenta = new C4f(1.0f, 0.0f, 1.0f);
        public static readonly C4f Yellow = new C4f(1.0f, 1.0f, 0.0f);
        public static readonly C4f White = new C4f(1.0f);

        public static readonly C4f DarkRed = new C4f(1.0f / 2, 0.0f / 2, 0.0f / 2);
        public static readonly C4f DarkGreen = new C4f(0.0f / 2, 1.0f / 2, 0.0f / 2);
        public static readonly C4f DarkBlue = new C4f(0.0f / 2, 0.0f / 2, 1.0f / 2);
        public static readonly C4f DarkCyan = new C4f(0.0f / 2, 1.0f / 2, 1.0f / 2);
        public static readonly C4f DarkMagenta = new C4f(1.0f / 2, 0.0f / 2, 1.0f / 2);
        public static readonly C4f DarkYellow = new C4f(1.0f / 2, 1.0f / 2, 0.0f / 2);
        public static readonly C4f Gray = new C4f(1.0f / 2);
        
        public static readonly C4f Gray10 = new C4f(0.1f);
        public static readonly C4f Gray20 = new C4f(0.2f);
        public static readonly C4f Gray30 = new C4f(0.3f);
        public static readonly C4f Gray40 = new C4f(0.4f);
        public static readonly C4f Gray50 = new C4f(0.5f);
        public static readonly C4f Gray60 = new C4f(0.6f);
        public static readonly C4f Gray70 = new C4f(0.7f);
        public static readonly C4f Gray80 = new C4f(0.8f);
        public static readonly C4f Gray90 = new C4f(0.9f);
        public static readonly C4f VRVisGreen = new C4f(0.698f, 0.851f, 0.008f);

        #endregion

        #region Comparison Operators

        public static bool operator ==(C4f a, C4f b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(C4f a, C4f b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4f a, C4f b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C4f col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B && this.A < col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4f col, float s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(float s)
        {
            return (this.R < s && this.G < s && this.B < s && this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(float s, C4f col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4f a, C4f b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C4f col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B || this.A < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4f col, float s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(float s)
        {
            return (this.R < s || this.G < s || this.B < s || this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(float s, C4f col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4f a, C4f b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C4f col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B && this.A > col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4f col, float s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(float s)
        {
            return (this.R > s && this.G > s && this.B > s && this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(float s, C4f col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4f a, C4f b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C4f col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B || this.A > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4f col, float s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(float s)
        {
            return (this.R > s || this.G > s || this.B > s || this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(float s, C4f col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4f a, C4f b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C4f col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B && this.A <= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4f col, float s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(float s)
        {
            return (this.R <= s && this.G <= s && this.B <= s && this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(float s, C4f col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4f a, C4f b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C4f col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B || this.A <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4f col, float s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(float s)
        {
            return (this.R <= s || this.G <= s || this.B <= s || this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(float s, C4f col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4f a, C4f b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C4f col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B && this.A >= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4f col, float s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(float s)
        {
            return (this.R >= s && this.G >= s && this.B >= s && this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(float s, C4f col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4f a, C4f b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C4f col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B || this.A >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4f col, float s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(float s)
        {
            return (this.R >= s || this.G >= s || this.B >= s || this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(float s, C4f col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4f a, C4f b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C4f col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B && this.A == col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4f col, float s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(float s)
        {
            return (this.R == s && this.G == s && this.B == s && this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(float s, C4f col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4f a, C4f b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C4f col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B || this.A == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4f col, float s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(float s)
        {
            return (this.R == s || this.G == s || this.B == s || this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(float s, C4f col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4f a, C4f b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C4f col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B && this.A != col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4f col, float s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(float s)
        {
            return (this.R != s && this.G != s && this.B != s && this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(float s, C4f col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4f a, C4f b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C4f col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B || this.A != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4f col, float s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(float s)
        {
            return (this.R != s || this.G != s || this.B != s || this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(float s, C4f col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
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
        {
            return (other is C4f) ? this == (C4f)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS)  + ", " 
                + A.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, ref Tup4<float> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4f LinComRawC4f(
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
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, ref Tup4<double> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4d LinComRawC4d(
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
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<float> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4f LinComRawC4f(
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
        public static C4f LinCom(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<double> w)
        {
            return new C4f(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4d LinComRawC4d(
            C4f p0, C4f p1, C4f p2, C4f p3, C4f p4, C4f p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                float.Parse(x[0], Localization.FormatEnUS), 
                float.Parse(x[1], Localization.FormatEnUS), 
                float.Parse(x[2], Localization.FormatEnUS), 
                float.Parse(x[3], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4f> Members

        public bool Equals(C4f other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4f Lerp(this double x, C4f a, C4f b)
        {
            return new C4f(
                        (float)((double)a.R + (x * ((double)b.R - (double)a.R))), 
                        (float)((double)a.G + (x * ((double)b.G - (double)a.G))), 
                        (float)((double)a.B + (x * ((double)b.B - (double)a.B))), 
                        (float)((double)a.A + (x * ((double)b.A - (double)a.A))));
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

        public C4d(C3us color)
        {
            R = Col.DoubleFromUShort(color.R);
            G = Col.DoubleFromUShort(color.G);
            B = Col.DoubleFromUShort(color.B);
            A = 1.0;
        }

        public C4d(C3ui color)
        {
            R = Col.DoubleFromUInt(color.R);
            G = Col.DoubleFromUInt(color.G);
            B = Col.DoubleFromUInt(color.B);
            A = 1.0;
        }

        public C4d(C3f color)
        {
            R = Col.DoubleFromFloat(color.R);
            G = Col.DoubleFromFloat(color.G);
            B = Col.DoubleFromFloat(color.B);
            A = 1.0;
        }

        public C4d(C3d color)
        {
            R = (color.R);
            G = (color.G);
            B = (color.B);
            A = 1.0;
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

        public static readonly Func<C3b, C4d> FromC3b = c => new C4d(c);
        public static readonly Func<C3us, C4d> FromC3us = c => new C4d(c);
        public static readonly Func<C3ui, C4d> FromC3ui = c => new C4d(c);
        public static readonly Func<C3f, C4d> FromC3f = c => new C4d(c);
        public static readonly Func<C3d, C4d> FromC3d = c => new C4d(c);
        public static readonly Func<C4b, C4d> FromC4b = c => new C4d(c);
        public static readonly Func<C4us, C4d> FromC4us = c => new C4d(c);
        public static readonly Func<C4ui, C4d> FromC4ui = c => new C4d(c);
        public static readonly Func<C4f, C4d> FromC4f = c => new C4d(c);

        public static readonly Func<V3d, C4d> FromV3d = c => new C4d(c);
        public static readonly Func<V4d, C4d> FromV4d = c => new C4d(c);

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

        public static readonly C4d Black = new C4d(0);

        public static readonly C4d Red = new C4d(1.0, 0, 0);
        public static readonly C4d Green = new C4d(0, 1.0, 0);
        public static readonly C4d Blue = new C4d(0, 0, 1.0);
        public static readonly C4d Cyan = new C4d(0, 1.0, 1.0);
        public static readonly C4d Magenta = new C4d(1.0, 0, 1.0);
        public static readonly C4d Yellow = new C4d(1.0, 1.0, 0);
        public static readonly C4d White = new C4d(1.0);

        public static readonly C4d DarkRed = new C4d(1.0 / 2, 0 / 2, 0 / 2);
        public static readonly C4d DarkGreen = new C4d(0 / 2, 1.0 / 2, 0 / 2);
        public static readonly C4d DarkBlue = new C4d(0 / 2, 0 / 2, 1.0 / 2);
        public static readonly C4d DarkCyan = new C4d(0 / 2, 1.0 / 2, 1.0 / 2);
        public static readonly C4d DarkMagenta = new C4d(1.0 / 2, 0 / 2, 1.0 / 2);
        public static readonly C4d DarkYellow = new C4d(1.0 / 2, 1.0 / 2, 0 / 2);
        public static readonly C4d Gray = new C4d(1.0 / 2);
        

        #endregion

        #region Comparison Operators

        public static bool operator ==(C4d a, C4d b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(C4d a, C4d b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Returns whether ALL elements of a are Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4d a, C4d b)
        {
            return (a.R < b.R && a.G < b.G && a.B < b.B && a.A < b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Smaller the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(C4d col)
        {
            return (this.R < col.R && this.G < col.G && this.B < col.B && this.A < col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(C4d col, double s)
        {
            return (col.R < s && col.G < s && col.B < s && col.A < s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Smaller s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmaller(double s)
        {
            return (this.R < s && this.G < s && this.B < s && this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmaller(double s, C4d col)
        {
            return (s < col.R && s < col.G && s < col.B && s < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4d a, C4d b)
        {
            return (a.R < b.R || a.G < b.G || a.B < b.B || a.A < b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Smaller the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmaller(C4d col)
        {
            return (this.R < col.R || this.G < col.G || this.B < col.B || this.A < col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(C4d col, double s)
        {
            return (col.R < s || col.G < s || col.B < s || col.A < s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Smaller s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmaller(double s)
        {
            return (this.R < s || this.G < s || this.B < s || this.A < s);
        }

        /// <summary>
        /// Returns whether a is Smaller AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmaller(double s, C4d col)
        {
            return (s < col.R || s < col.G || s < col.B || s < col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4d a, C4d b)
        {
            return (a.R > b.R && a.G > b.G && a.B > b.B && a.A > b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Greater the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(C4d col)
        {
            return (this.R > col.R && this.G > col.G && this.B > col.B && this.A > col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(C4d col, double s)
        {
            return (col.R > s && col.G > s && col.B > s && col.A > s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Greater s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreater(double s)
        {
            return (this.R > s && this.G > s && this.B > s && this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreater(double s, C4d col)
        {
            return (s > col.R && s > col.G && s > col.B && s > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4d a, C4d b)
        {
            return (a.R > b.R || a.G > b.G || a.B > b.B || a.A > b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Greater the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreater(C4d col)
        {
            return (this.R > col.R || this.G > col.G || this.B > col.B || this.A > col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(C4d col, double s)
        {
            return (col.R > s || col.G > s || col.B > s || col.A > s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Greater s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreater(double s)
        {
            return (this.R > s || this.G > s || this.B > s || this.A > s);
        }

        /// <summary>
        /// Returns whether a is Greater AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreater(double s, C4d col)
        {
            return (s > col.R || s > col.G || s > col.B || s > col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4d a, C4d b)
        {
            return (a.R <= b.R && a.G <= b.G && a.B <= b.B && a.A <= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are SmallerOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(C4d col)
        {
            return (this.R <= col.R && this.G <= col.G && this.B <= col.B && this.A <= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(C4d col, double s)
        {
            return (col.R <= s && col.G <= s && col.B <= s && col.A <= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are SmallerOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllSmallerOrEqual(double s)
        {
            return (this.R <= s && this.G <= s && this.B <= s && this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllSmallerOrEqual(double s, C4d col)
        {
            return (s <= col.R && s <= col.G && s <= col.B && s <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4d a, C4d b)
        {
            return (a.R <= b.R || a.G <= b.G || a.B <= b.B || a.A <= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is SmallerOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnySmallerOrEqual(C4d col)
        {
            return (this.R <= col.R || this.G <= col.G || this.B <= col.B || this.A <= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(C4d col, double s)
        {
            return (col.R <= s || col.G <= s || col.B <= s || col.A <= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is SmallerOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnySmallerOrEqual(double s)
        {
            return (this.R <= s || this.G <= s || this.B <= s || this.A <= s);
        }

        /// <summary>
        /// Returns whether a is SmallerOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnySmallerOrEqual(double s, C4d col)
        {
            return (s <= col.R || s <= col.G || s <= col.B || s <= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4d a, C4d b)
        {
            return (a.R >= b.R && a.G >= b.G && a.B >= b.B && a.A >= b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are GreaterOrEqual the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(C4d col)
        {
            return (this.R >= col.R && this.G >= col.G && this.B >= col.B && this.A >= col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(C4d col, double s)
        {
            return (col.R >= s && col.G >= s && col.B >= s && col.A >= s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are GreaterOrEqual s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllGreaterOrEqual(double s)
        {
            return (this.R >= s && this.G >= s && this.B >= s && this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllGreaterOrEqual(double s, C4d col)
        {
            return (s >= col.R && s >= col.G && s >= col.B && s >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4d a, C4d b)
        {
            return (a.R >= b.R || a.G >= b.G || a.B >= b.B || a.A >= b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is GreaterOrEqual the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyGreaterOrEqual(C4d col)
        {
            return (this.R >= col.R || this.G >= col.G || this.B >= col.B || this.A >= col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(C4d col, double s)
        {
            return (col.R >= s || col.G >= s || col.B >= s || col.A >= s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is GreaterOrEqual s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyGreaterOrEqual(double s)
        {
            return (this.R >= s || this.G >= s || this.B >= s || this.A >= s);
        }

        /// <summary>
        /// Returns whether a is GreaterOrEqual AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyGreaterOrEqual(double s, C4d col)
        {
            return (s >= col.R || s >= col.G || s >= col.B || s >= col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4d a, C4d b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Equal the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(C4d col)
        {
            return (this.R == col.R && this.G == col.G && this.B == col.B && this.A == col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(C4d col, double s)
        {
            return (col.R == s && col.G == s && col.B == s && col.A == s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Equal s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllEqual(double s)
        {
            return (this.R == s && this.G == s && this.B == s && this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllEqual(double s, C4d col)
        {
            return (s == col.R && s == col.G && s == col.B && s == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4d a, C4d b)
        {
            return (a.R == b.R || a.G == b.G || a.B == b.B || a.A == b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Equal the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyEqual(C4d col)
        {
            return (this.R == col.R || this.G == col.G || this.B == col.B || this.A == col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(C4d col, double s)
        {
            return (col.R == s || col.G == s || col.B == s || col.A == s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Equal s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyEqual(double s)
        {
            return (this.R == s || this.G == s || this.B == s || this.A == s);
        }

        /// <summary>
        /// Returns whether a is Equal AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyEqual(double s, C4d col)
        {
            return (s == col.R || s == col.G || s == col.B || s == col.A);
        }
        /// <summary>
        /// Returns whether ALL elements of a are Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4d a, C4d b)
        {
            return (a.R != b.R && a.G != b.G && a.B != b.B && a.A != b.A);
        }

        /// <summary>
        /// Returns whether ALL elements of this are Different the corresponding element of col.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(C4d col)
        {
            return (this.R != col.R && this.G != col.G && this.B != col.B && this.A != col.A);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(C4d col, double s)
        {
            return (col.R != s && col.G != s && col.B != s && col.A != s);
        }

        /// <summary>
        /// Returns whether ALL elements of col are Different s.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AllDifferent(double s)
        {
            return (this.R != s && this.G != s && this.B != s && this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different ALL elements of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AllDifferent(double s, C4d col)
        {
            return (s != col.R && s != col.G && s != col.B && s != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4d a, C4d b)
        {
            return (a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is Different the corresponding element of b.
        /// ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).
        /// </summary>
        public bool AnyDifferent(C4d col)
        {
            return (this.R != col.R || this.G != col.G || this.B != col.B || this.A != col.A);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(C4d col, double s)
        {
            return (col.R != s || col.G != s || col.B != s || col.A != s);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is Different s.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public bool AnyDifferent(double s)
        {
            return (this.R != s || this.G != s || this.B != s || this.A != s);
        }

        /// <summary>
        /// Returns whether a is Different AT LEAST ONE element of col.
        /// ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).
        /// </summary>
        public static bool AnyDifferent(double s, C4d col)
        {
            return (s != col.R || s != col.G || s != col.B || s != col.A);
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
        {
            return (other is C4d) ? this == (C4d)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(R, G, B, A);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")
                + R.ToString(null, Localization.FormatEnUS)  + ", " 
                + G.ToString(null, Localization.FormatEnUS)  + ", " 
                + B.ToString(null, Localization.FormatEnUS)  + ", " 
                + A.ToString(null, Localization.FormatEnUS) 
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
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, ref Tup4<float> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4f LinComRawC4f(
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
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, ref Tup4<double> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3));
        }

        public static C4d LinComRawC4d(
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
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<float> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4f LinComRawC4f(
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
        public static C4d LinCom(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<double> w)
        {
            return new C4d(
                (p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5), 
                (p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5), 
                (p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5));
        }

        public static C4d LinComRawC4d(
            C4d p0, C4d p1, C4d p2, C4d p3, C4d p4, C4d p5, ref Tup6<double> w)
        {
            return new C4d(
                p0.R * w.E0 + p1.R * w.E1 + p2.R * w.E2 + p3.R * w.E3 + p4.R * w.E4 + p5.R * w.E5, 
                p0.G * w.E0 + p1.G * w.E1 + p2.G * w.E2 + p3.G * w.E3 + p4.G * w.E4 + p5.G * w.E5, 
                p0.B * w.E0 + p1.B * w.E1 + p2.B * w.E2 + p3.B * w.E3 + p4.B * w.E4 + p5.B * w.E5);
        }

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
                double.Parse(x[0], Localization.FormatEnUS), 
                double.Parse(x[1], Localization.FormatEnUS), 
                double.Parse(x[2], Localization.FormatEnUS), 
                double.Parse(x[3], Localization.FormatEnUS)
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin + R.ToString(format, fp)  + between + G.ToString(format, fp)  + between + B.ToString(format, fp)  + between + A.ToString(format, fp)  + end;
        }

        #endregion

        #region IEquatable<C4d> Members

        public bool Equals(C4d other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static C4d Lerp(this double x, C4d a, C4d b)
        {
            return new C4d(
                        (double)((double)a.R + (x * ((double)b.R - (double)a.R))), 
                        (double)((double)a.G + (x * ((double)b.G - (double)a.G))), 
                        (double)((double)a.B + (x * ((double)b.B - (double)a.B))), 
                        (double)((double)a.A + (x * ((double)b.A - (double)a.A))));
        }

        #endregion
    }

    #endregion

}

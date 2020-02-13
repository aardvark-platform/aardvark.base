using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var single = isDouble ? "double" : "single";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    #region Shift__x3t__

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Shift__x3t__
    {
        [DataMember]
        public V__x3t__ V;

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="Shift__x3t__"/> using three __ft__s.
        /// </summary>
        public Shift__x3t__(__ft__ x, __ft__ y, __ft__ z)
        {
            V = new V__x3t__(x, y, z);
        }

        public Shift__x3t__(V__x3t__ v)
        {
            V = v;
        }

        /// <summary>
        /// Initializes a <see cref="Shift__x3t__"/> class from __ft__-array.
        /// </summary>
        public Shift__x3t__(__ft__[] array)
        {
            V = new V__x3t__(array);
        }

        /// <summary>
        /// Initializes a <see cref="Shift__x3t__"/> class from __ft__-array starting from passed index
        /// </summary>
        public Shift__x3t__(__ft__[] array, int start)
        {
            V = new V__x3t__(array, start);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the X coordinate.
        /// </summary>
        public __ft__ X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.X; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.X = value; }
        }

        /// <summary>
        /// Gets and sets the Y coordinate.
        /// </summary>
        public __ft__ Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Y; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.Y = value; }
        }

        /// <summary>
        /// Gets and sets the Z coordinate.
        /// </summary>
        public __ft__ Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Z; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.Z = value; }
        }

        /// <summary>
        /// Calculates the length of a <see cref="Shift__x3t__"/>.
        /// </summary>
        /// <returns>A __ft__ scalar.</returns>
        public __ft__ Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Length; }
        }

        /// <summary>
        /// Calculates the squared length of a <see cref="Shift__x3t__"/>.
        /// </summary>
        /// <returns>A __ft__ scalar.</returns>
        public __ft__ LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.LengthSquared; }
        }

        public Shift__x3t__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Shift__x3t__(-V); }
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="Shift__x3t__"/>.
        /// </summary>
        public Shift__x3t__ Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Shift__x3t__(1 / X, 1 / Y, 1 / Z);
        }

        #endregion

        #region Constants

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point zero shift vector.
        /// </summary>
        public static Shift__x3t__ Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Shift__x3t__(0, 0, 0);
        }

        public static Shift__x3t__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Shift__x3t__(0, 0, 0);
        }

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point X-Axis shift vector.
        /// </summary>
        public static Shift__x3t__ XAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Shift__x3t__(1, 0, 0);
        }

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point Y-Axis shift vector.
        /// </summary>
        public static Shift__x3t__ YAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Shift__x3t__(0, 1, 0);
        }

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point Z-Axis shift vector.
        /// </summary>
        public static Shift__x3t__ ZAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Shift__x3t__(0, 0, 1);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Negates the values of a <see cref="Shift__x3t__"/> instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shift__x3t__ operator -(Shift__x3t__ shift)
        {
            return new Shift__x3t__(-shift.X, -shift.Y, -shift.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a __ft__ scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shift__x3t__ operator *(Shift__x3t__ shift, __ft__ value)
        {
            return new Shift__x3t__(shift.X * value, shift.Y * value, shift.Z * value);
        }

        /// <summary>
        /// Calculates the multiplacation of a __ft__ scalar with a <see cref="Shift__x3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shift__x3t__ operator *(__ft__ value, Shift__x3t__ shift)
        {
            return new Shift__x3t__(shift.X * value, shift.Y * value, shift.Z * value);
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V__x4t__ operator *(Shift__x3t__ shift, V__x4t__ vec)
        {
            return new V__x4t__(vec.X + shift.X * vec.W,
                            vec.Y + shift.Y * vec.W,
                            vec.Z + shift.Z * vec.W,
                            vec.W);
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(Shift__x3t__ shift, M2__x2t__ mat)
        {
            return new M3__x4t__(mat.M00, mat.M01, 0, shift.X,
                             mat.M10, mat.M11, 0, shift.Y,
                             0, 0, 1, shift.Z);
        }


        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(M2__x2t__ matrix, Shift__x3t__ shift)
        {
            return new M3__x4t__(matrix.M00, matrix.M01, 0, matrix.M00 * shift.X + matrix.M01 * shift.Y,
                            matrix.M10, matrix.M11, 0, matrix.M10 * shift.X + matrix.M11 * shift.Y,
                            0, 0, 1, shift.Z);
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(Shift__x3t__ shift, M3__x3t__ matrix)
        {
            return new M3__x4t__(matrix.M00, matrix.M01, matrix.M02, shift.X,
                             matrix.M10, matrix.M11, matrix.M12, shift.Y,
                             matrix.M20, matrix.M21, matrix.M22, shift.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(M3__x3t__ m, Shift__x3t__ t)
        {
            return new M3__x4t__(
               m.M00, m.M01, m.M02, m.M00 * t.X + m.M01 * t.Y + m.M02 * t.Z,
               m.M10, m.M11, m.M12, m.M10 * t.X + m.M11 * t.Y + m.M12 * t.Z,
               m.M20, m.M21, m.M22, m.M20 * t.X + m.M21 * t.Y + m.M22 * t.Z
               );
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(Shift__x3t__ shift, Rot__x3t__ rot)
        {
            return shift * (M3__x4t__)rot;
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(Shift__x3t__ shift, Rot__x2t__ rot)
        {
            return (M3__x4t__)(shift * (M2__x2t__)rot);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="Shift__x3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shift__x3t__ operator *(Shift__x3t__ shift0, Shift__x3t__ shift1)
        {
            return new Shift__x3t__(shift0.X + shift1.X, shift0.Y + shift1.Y, shift0.Z + shift1.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="Scale__x3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(Shift__x3t__ shift, Scale__x3t__ scale)
        {
            return new M3__x4t__(scale.X, 0, 0, shift.X,
                0, scale.Y, 0, shift.Y,
                0, 0, scale.Z, shift.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="M4__x4t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M4__x4t__ operator *(Shift__x3t__ shift, M4__x4t__ matrix)
        {
            return new M4__x4t__(
                    matrix.M00 + shift.X * matrix.M30,
                    matrix.M01 + shift.X * matrix.M31,
                    matrix.M02 + shift.X * matrix.M32,
                    matrix.M03 + shift.X * matrix.M33,

                    matrix.M10 + shift.Y * matrix.M30,
                    matrix.M11 + shift.Y * matrix.M31,
                    matrix.M12 + shift.Y * matrix.M32,
                    matrix.M13 + shift.Y * matrix.M33,

                    matrix.M20 + shift.Z * matrix.M30,
                    matrix.M21 + shift.Z * matrix.M31,
                    matrix.M22 + shift.Z * matrix.M32,
                    matrix.M23 + shift.Z * matrix.M33,

                    matrix.M30,
                    matrix.M31,
                    matrix.M32,
                    matrix.M33
                    );
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="M3__x4t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__x4t__ operator *(Shift__x3t__ shift, M3__x4t__ matrix)
        {
            return new M3__x4t__(
                    matrix.M00,
                    matrix.M01,
                    matrix.M02,
                    matrix.M03 + shift.X,

                    matrix.M10,
                    matrix.M11,
                    matrix.M12,
                    matrix.M13 + shift.Y,

                    matrix.M20,
                    matrix.M21,
                    matrix.M22,
                    matrix.M23 + shift.Z
                    );
        }

        /// <summary>
        /// Calculates the division of a <see cref="Shift__x3t__"/> with a __ft__ scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shift__x3t__ operator /(Shift__x3t__ shift, __ft__ value)
        {
            return shift * (1 / value);
        }

        /// <summary>
        /// Calculates the division of a __ft__ scalar with a <see cref="Shift__x3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shift__x3t__ operator /(__ft__ value, Shift__x3t__ shift)
        {
            return shift.Reciprocal * value;
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Shift__x3t__"/>s are equal.
        /// </summary>
        public static bool operator ==(Shift__x3t__ s0, Shift__x3t__ s1)
        {
            return s0.X == s1.X && s0.Y == s1.Y && s0.Z == s1.Z;
        }

        /// <summary>
        /// Checks whether two <see cref="Shift__x3t__"/> instances are different.
        /// </summary>
        public static bool operator !=(Shift__x3t__ s0, Shift__x3t__ s1)
        {
            return s0.X != s1.X || s0.Y != s1.Y || s0.Z != s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift__x3t__"/> 
        /// are bigger then the components of the second <see cref="Shift__x3t__"/>.
        /// </summary>
        public static bool operator >(Shift__x3t__ s0, Shift__x3t__ s1)
        {
            return s0.V.X > s1.V.X && s0.V.Y > s1.V.Y && s0.V.Z > s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift__x3t__"/> 
        /// are bigger or equal then the components of the second <see cref="Shift__x3t__"/>.
        /// </summary>
        public static bool operator >=(Shift__x3t__ s0, Shift__x3t__ s1)
        {
            return s0.V.X >= s1.V.X && s0.V.Y >= s1.V.Y && s0.V.Z >= s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift__x3t__"/> 
        /// are smaller then the components of the second <see cref="Shift__x3t__"/>.
        /// </summary>
        public static bool operator <(Shift__x3t__ s0, Shift__x3t__ s1)
        {
            return s0.V.X < s1.V.X && s0.V.Y < s1.V.Y && s0.V.Z < s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift__x3t__"/> 
        /// are smaller or equal then the components of the second <see cref="Shift__x3t__"/>.
        /// </summary>
        public static bool operator <=(Shift__x3t__ s0, Shift__x3t__ s1)
        {
            return s0.V.X <= s1.V.X && s0.V.Y <= s1.V.Y && s0.V.Z <= s1.V.Z;
        }

        #endregion

        #region Conversion Operators

        public static explicit operator M4__x4t__(Shift__x3t__ s)
        {
            return new M4__x4t__(1, 0, 0, s.X,
                            0, 1, 0, s.Y,
                            0, 0, 1, s.Z,
                            0, 0, 0, 1);
        }

        public static explicit operator M3__x4t__(Shift__x3t__ s)
        {
            return new M3__x4t__(1, 0, 0, s.X,
                            0, 1, 0, s.Y,
                            0, 0, 1, s.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine__x3t__(Shift__x3t__ s)
            => new Affine__x3t__(s);

        /// <summary>
        /// Returns all values of a <see cref="Shift__x3t__"/> instance
        /// in a __ft__[] array.
        /// </summary>
        public static explicit operator __ft__[](Shift__x3t__ shift)
        {
            __ft__[] array = new __ft__[3];
            array[0] = shift.X;
            array[1] = shift.Y;
            array[2] = shift.Z;
            return array;
        }

        #endregion

        #region Indexing

        public __ft__ this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return V.X;
                    case 1: return V.Y;
                    case 2: return V.Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: V.X = value; return;
                    case 1: V.Y = value; return;
                    case 2: V.Z = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region Override

        public override int GetHashCode()
        {
            return V.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Shift__x3t__)
            {
                Shift__x3t__ shift = (Shift__x3t__)obj;
                return V.X == shift.X && V.Y == shift.Y && V.Z == shift.Z;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Shift__x3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Shift__x3t__(
                __ft__.Parse(x[0], CultureInfo.InvariantCulture),
                __ft__.Parse(x[1], CultureInfo.InvariantCulture),
                __ft__.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion
    }

    #endregion

    //# }
}

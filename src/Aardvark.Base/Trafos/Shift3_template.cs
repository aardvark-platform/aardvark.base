using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var single = isDouble ? "double" : "single";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Shift__x3t__
    {
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
            get { return V.X; }
            set { V.X = value; }
        }

        /// <summary>
        /// Gets and sets the Y coordinate.
        /// </summary>
        public __ft__ Y
        {
            get { return V.Y; }
            set { V.Y = value; }
        }

        /// <summary>
        /// Gets and sets the Z coordinate.
        /// </summary>
        public __ft__ Z
        {
            get { return V.Z; }
            set { V.Z = value; }
        }

        /// <summary>
        /// Calculates the length of a <see cref="Shift__x3t__"/>.
        /// </summary>
        /// <returns>A __ft__ scalar.</returns>
        public __ft__ Length
        {
            get { return V.Length; }
        }

        /// <summary>
        /// Calculates the squared length of a <see cref="Shift__x3t__"/>.
        /// </summary>
        /// <returns>A __ft__ scalar.</returns>
        public __ft__ LengthSquared
        {
            get { return V.LengthSquared; }
        }

        public Shift__x3t__ Inverse
        {
            get { return new Shift__x3t__(V.Negated); }
        }

        #endregion

        #region Constants

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point zero shift vector.
        /// </summary>
        public static readonly Shift__x3t__ Zero = new Shift__x3t__(0, 0, 0);
        public static readonly Shift__x3t__ Identity = new Shift__x3t__(0, 0, 0);

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point X-Axis shift vector.
        /// </summary>
        public static readonly Shift__x3t__ XAxis = new Shift__x3t__(1, 0, 0);

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point Y-Axis shift vector.
        /// </summary>
        public static readonly Shift__x3t__ YAxis = new Shift__x3t__(0, 1, 0);

        /// <summary>
        /// A <see cref="Shift__x3t__"/> __single__-precision floating point Z-Axis shift vector.
        /// </summary>
        public static readonly Shift__x3t__ ZAxis = new Shift__x3t__(0, 0, 1);

        #endregion

        #region Vector Arithmetics
        //different calculations for shift vectors

        /// <summary>
        /// Multiplacation of a float scalar with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static Shift__x3t__ Multiply(Shift__x3t__ shift, __ft__ value)
        {
            return new Shift__x3t__(shift.X * value,
                               shift.Y * value,
                               shift.Z * value);
        }

        /// <summary>
        /// Multiplication of two <see cref="Shift__x3t__"/>s.
        /// </summary>
        public static Shift__x3t__ Multiply(Shift__x3t__ shift0, Shift__x3t__ shift1)
        {
            return new Shift__x3t__(shift0.X + shift1.X,
                               shift0.Y + shift1.Y,
                               shift0.Z + shift1.Z);
        }

        public static M3__x4t__ Multiply(Shift__x3t__ shift, Scale__x3t__ scale)
        {
            return new M3__x4t__(scale.X, 0, 0, shift.X,
                            0, scale.Y, 0, shift.Y,
                            0, 0, scale.Z, shift.Z);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="M4__x4t__"/>.
        /// </summary>
        public static M4__x4t__ Multiply(Shift__x3t__ shift, M4__x4t__ m)
        {
            return new M4__x4t__(
                    m.M00 + shift.X * m.M30,
                    m.M01 + shift.X * m.M31,
                    m.M02 + shift.X * m.M32,
                    m.M03 + shift.X * m.M33,

                    m.M10 + shift.Y * m.M30,
                    m.M11 + shift.Y * m.M31,
                    m.M12 + shift.Y * m.M32,
                    m.M13 + shift.Y * m.M33,

                    m.M20 + shift.Z * m.M30,
                    m.M21 + shift.Z * m.M31,
                    m.M22 + shift.Z * m.M32,
                    m.M23 + shift.Z * m.M33,

                    m.M30,
                    m.M31,
                    m.M32,
                    m.M33
                    );
        }

        /// <summary>
        /// Multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="M3__x4t__"/>.
        /// </summary>
        public static M3__x4t__ Multiply(Shift__x3t__ shift, M3__x4t__ m)
        {
            return new M3__x4t__(
                    m.M00,
                    m.M01,
                    m.M02,
                    m.M03 + shift.X,

                    m.M10,
                    m.M11,
                    m.M12,
                    m.M13 + shift.Y,

                    m.M20,
                    m.M21,
                    m.M22,
                    m.M23 + shift.Z
                    );
        }

        /// <summary>
        /// Division of a <see cref="Shift__x3t__"/> instance with a __ft__ scalar.
        /// </summary>
        public static Shift__x3t__ Divide(Shift__x3t__ shift, __ft__ val)
        {
            return Multiply(shift, 1 / val);
        }

        /// <summary>
        /// Division of a __ft__ scalar with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static Shift__x3t__ Divide(__ft__ value, Shift__x3t__ shift)
        {
            return Multiply(Reciprocal(shift), value);
        }

        /// <summary>
        /// Negates all values of a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static Shift__x3t__ Negate(Shift__x3t__ shift)
        {
            return new Shift__x3t__(-shift.X, -shift.Y, -shift.Z);
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static Shift__x3t__ Reciprocal(Shift__x3t__ shift)
        {
            return new Shift__x3t__(1 / shift.X, 1 / shift.Y, 1 / shift.Z);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Negates the values of a <see cref="Shift__x3t__"/> instance.
        /// </summary>
        public static Shift__x3t__ operator -(Shift__x3t__ shift)
        {
            return Shift__x3t__.Negate(shift);
        }

        /// <summary>
        /// Calculates the multiplacation of a __ft__ scalar with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static Shift__x3t__ operator *(Shift__x3t__ shift, __ft__ value)
        {
            return Shift__x3t__.Multiply(shift, value);
        }

        /// <summary>
        /// </summary>
        public static V__x4t__ operator *(Shift__x3t__ shift, V__x4t__ vec)
        {
            return new V__x4t__(vec.X + shift.X * vec.W,
                            vec.Y + shift.Y * vec.W,
                            vec.Z + shift.Z * vec.W,
                            vec.W);
        }

        /// <summary>
        /// </summary>
        public static M3__x4t__ operator *(Shift__x3t__ shift, M2__x2t__ mat)
        {
            return new M3__x4t__(mat.M00, mat.M01, 0, shift.X,
                             mat.M10, mat.M11, 0, shift.Y,
                             0, 0, 1, shift.Z);
        }

        /// <summary>
        /// </summary>
        public static M3__x4t__ operator *(Shift__x3t__ shift, M3__x3t__ mat)
        {
            return new M3__x4t__(mat.M00, mat.M01, mat.M02, shift.X,
                             mat.M10, mat.M11, mat.M12, shift.Y,
                             mat.M20, mat.M21, mat.M22, shift.Z);
        }

        /// <summary>
        /// </summary>
        public static M3__x4t__ operator *(Shift__x3t__ shift, Rot__x3t__ rot)
        {
            return Shift__x3t__.Multiply(shift, (M3__x4t__)rot);
        }

        /// <summary>
        /// </summary>
        public static M3__x4t__ operator *(Shift__x3t__ shift, Rot__x2t__ rot)
        {
            return (M3__x4t__)(shift * (M2__x2t__)rot);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a __ft__ scalar.
        /// </summary>
        public static Shift__x3t__ operator *(__ft__ value, Shift__x3t__ shift)
        {
            return Shift__x3t__.Multiply(shift, value);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static Shift__x3t__ operator *(Shift__x3t__ shift0, Shift__x3t__ shift1)
        {
            return Shift__x3t__.Multiply(shift0, shift1);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static M3__x4t__ operator *(Shift__x3t__ shift, Scale__x3t__ scale)
        {
            return Shift__x3t__.Multiply(shift, scale);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="M4__x4t__"/>.
        /// </summary>
        public static M4__x4t__ operator *(Shift__x3t__ shift, M4__x4t__ mat)
        {
            return Shift__x3t__.Multiply(shift, mat);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift__x3t__"/> with a <see cref="M3__x4t__"/>.
        /// </summary>
        public static M3__x4t__ operator *(Shift__x3t__ shift, M3__x4t__ mat)
        {
            return Shift__x3t__.Multiply(shift, mat);
        }

        /// <summary>
        /// Calculates the division of a <see cref="Shift__x3t__"/> with a __ft__ scalar.
        /// </summary>
        public static Shift__x3t__ operator /(Shift__x3t__ shift, __ft__ val)
        {
            return Shift__x3t__.Divide(shift, val);
        }

        /// <summary>
        /// Calculates the division of a __ft__ scalar with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static Shift__x3t__ operator /(__ft__ val, Shift__x3t__ shift)
        {
            return Shift__x3t__.Divide(val, shift);
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
    //# } // isDouble
}

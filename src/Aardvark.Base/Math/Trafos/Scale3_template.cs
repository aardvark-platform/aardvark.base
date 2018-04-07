using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var single = isDouble ? "double" : "single";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    /// <summary>
    /// A three dimensional scaling transform with different scaling values
    /// in each dimension.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Scale__x3t__
    {
        [DataMember]
        public V__x3t__ V;

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="Scale__x3t__"/> using three __ft__s.
        /// </summary>
        public Scale__x3t__(__ft__ x, __ft__ y, __ft__ z)
        {
            V = new V__x3t__(x, y, z);
        }

        public Scale__x3t__(V__x3t__ v)
        {
            V = v;
        }

        /// <summary>
        /// Initializes a <see cref="Scale__x3t__"/> using a uniform __ft__ value.
        /// </summary>
        public Scale__x3t__(__ft__ uniform)
        {
            V = new V__x3t__(uniform, uniform, uniform);
        }

        /// <summary>
        /// Initializes a <see cref="Scale__x3t__"/> class from __ft__-array.
        /// </summary>
        public Scale__x3t__(__ft__[] array)
        {
            V = new V__x3t__(array);
        }

        /// <summary>
        /// Initializes a <see cref="Scale__x3t__"/> class from __ft__-array starting from passed index
        /// </summary>
        public Scale__x3t__(__ft__[] array, int start)
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
        /// Calculates the length of a <see cref="Scale__x3t__"/>.
        /// </summary>
        public __ft__ Length
        {
            get { return V.Length; }
        }

        /// <summary>
        /// Calculates the squared length of a <see cref="Scale__x3t__"/>.
        /// </summary>
        public __ft__ LengthSquared
        {
            get { return V.LengthSquared; }
        }

        public Scale__x3t__ Inverse
        {
            get { return new Scale__x3t__(V.Reciprocal); }
        }

        #endregion

        #region Constants

        public static readonly Scale__x3t__ Identity = new Scale__x3t__(1, 1, 1);

        /// <summary>
        /// A <see cref="Scale3d"/> __single__-precision floating point zero shift vector.
        /// </summary>
        public static readonly Scale__x3t__ Zero = new Scale__x3t__(0, 0, 0);

        /// <summary>
        /// A <see cref="Scale3d"/> __single__-precision floating point X-Axis shift vector.
        /// </summary>
        public static readonly Scale__x3t__ XAxis = new Scale__x3t__(1, 0, 0);

        /// <summary>
        /// A <see cref="Scale3d"/> __single__-precision floating point Y-Axis shift vector.
        /// </summary>
        public static readonly Scale__x3t__ YAxis = new Scale__x3t__(0, 1, 0);

        /// <summary>
        /// A <see cref="Scale3d"/> __single__-precision floating point Z-Axis shift vector.
        /// </summary>
        public static readonly Scale__x3t__ ZAxis = new Scale__x3t__(0, 0, 1);

        #endregion

        #region Vector Arithmetics

        /// <summary>
        /// Multiplacation of a __ft__ scalar with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static Scale__x3t__ Multiply(Scale__x3t__ scale, __ft__ value)
        {
            return new Scale__x3t__(scale.X * value, scale.Y * value, scale.Z * value);
        }

        /// <summary>
        /// Multiplication of two <see cref="Scale__x3t__"/>s.
        /// </summary>
        public static Scale__x3t__ Multiply(Scale__x3t__ scale0, Scale__x3t__ scale1)
        {
            return new Scale__x3t__(scale0.X * scale1.X, scale0.Y * scale1.Y, scale0.Z * scale1.Z);
        }

        public static M3__x3t__ Multiply(Scale__x3t__ scale, Rot__x3t__ rot)
        {
            return Scale__x3t__.Multiply(scale, (M3__x3t__)rot);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="V__x2t__"/>.
        /// </summary>
        public static V__x2t__ Multiply(Scale__x3t__ scale, V__x2t__ v)
        {
            return new V__x2t__(v.X * scale.X, v.Y * scale.Y);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="V__x3t__"/>.
        /// </summary>
        public static V__x3t__ Multiply(Scale__x3t__ scale, V__x3t__ v)
        {
            return new V__x3t__(scale.X * v.X, scale.Y * v.Y, scale.Z * v.Z);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="V__x4t__"/>.
        /// </summary>
        public static V__x4t__ Multiply(Scale__x3t__ scale, V__x4t__ vec)
        {
            return new V__x4t__(scale.X * vec.X,
                            scale.Y * vec.Y,
                            scale.Z * vec.Z,
                            vec.W);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M2__x2t__"/>.
        /// </summary>
        public static M3__x3t__ Multiply(Scale__x3t__ scale, M2__x2t__ mat)
        {
            return new M3__x3t__(scale.X * mat.M00,
                             scale.X * mat.M01,
                             0,

                             scale.Y * mat.M10,
                             scale.Y * mat.M11,
                             0,

                             0,
                             0,
                             scale.Z);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M3__x3t__"/>.
        /// </summary>
        public static M3__x3t__ Multiply(Scale__x3t__ scale, M3__x3t__ mat)
        {
            return new M3__x3t__(scale.X * mat.M00,
                             scale.X * mat.M01,
                             scale.X * mat.M02,

                             scale.Y * mat.M10,
                             scale.Y * mat.M11,
                             scale.Y * mat.M12,

                             scale.Z * mat.M20,
                             scale.Z * mat.M21,
                             scale.Z * mat.M22);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M3__x4t__"/>.
        /// </summary>
        public static M3__x4t__ Multiply(Scale__x3t__ scale, M3__x4t__ mat)
        {
            return new M3__x4t__(
                    scale.X * mat.M00,
                    scale.X * mat.M01,
                    scale.X * mat.M02,
                    scale.X * mat.M03,

                    scale.Y * mat.M10,
                    scale.Y * mat.M11,
                    scale.Y * mat.M12,
                    scale.Y * mat.M13,

                    scale.Z * mat.M20,
                    scale.Z * mat.M21,
                    scale.Z * mat.M22,
                    scale.Z * mat.M23
                    );
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M4__x4t__"/>.
        /// </summary>
        public static M4__x4t__ Multiply(Scale__x3t__ scale, M4__x4t__ mat)
        {
            return new M4__x4t__(
                    scale.X * mat.M00,
                    scale.X * mat.M01,
                    scale.X * mat.M02,
                    scale.X * mat.M03,

                    scale.Y * mat.M10,
                    scale.Y * mat.M11,
                    scale.Y * mat.M12,
                    scale.Y * mat.M13,

                    scale.Z * mat.M20,
                    scale.Z * mat.M21,
                    scale.Z * mat.M22,
                    scale.Z * mat.M23,

                    mat.M30,
                    mat.M31,
                    mat.M32,
                    mat.M33
                    );
        }

        public static M3__x3t__ Multiply(Scale__x3t__ scale, Rot__x2t__ rot)
        {
            return Scale__x3t__.Multiply(scale, (M2__x2t__)rot);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static M3__x4t__ Multiply(Scale__x3t__ scale, Shift__x3t__ shift)
        {
            return new M3__x4t__(
                    scale.X,
                    0,
                    0,
                    scale.X * shift.X,

                    0,
                    scale.Y,
                    0,
                    scale.Y * shift.Y,

                    0,
                    0,
                    scale.Z,
                    scale.Z * shift.Z

                    );
        }

        /// <summary>
        /// Division of a <see cref="Scale__x3t__"/> instance with a __ft__ scalar.
        /// </summary>
        public static Scale__x3t__ Divide(Scale__x3t__ scale, __ft__ val)
        {
            return Multiply(scale, 1 / val);
        }

        /// <summary>
        /// Division of a __ft__ scalar with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static Scale__x3t__ Divide(__ft__ val, Scale__x3t__ scale)
        {
            return Multiply(Reciprocal(scale), val);
        }

        /// <summary>
        /// Negates all values of a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static Scale__x3t__ Negate(Scale__x3t__ scale)
        {
            return new Scale__x3t__(-scale.X, -scale.Y, -scale.Z);
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static Scale__x3t__ Reciprocal(Scale__x3t__ scale)
        {
            return new Scale__x3t__(1 / scale.X, 1 / scale.Y, 1 / scale.Z);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Negates the values of a <see cref="Scale__x3t__"/> instance.
        /// </summary>
        public static Scale__x3t__ operator -(Scale__x3t__ scalingVector)
        {
            return Scale__x3t__.Negate(scalingVector);
        }

        /// <summary>
        /// Calculates the multiplacation of a __ft__ scalar with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static Scale__x3t__ operator *(Scale__x3t__ scale, __ft__ scalar)
        {
            return Scale__x3t__.Multiply(scale, scalar);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="V__x2t__"/>.
        /// </summary>
        public static V__x2t__ operator *(Scale__x3t__ scale, V__x2t__ vector)
        {
            return Scale__x3t__.Multiply(scale, vector);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="V__x3t__"/>.
        /// </summary>
        public static V__x3t__ operator *(Scale__x3t__ scale, V__x3t__ vector)
        {
            return Scale__x3t__.Multiply(scale, vector);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="V__x4t__"/>.
        /// </summary>
        public static V__x4t__ operator *(Scale__x3t__ scale, V__x4t__ vector)
        {
            return Scale__x3t__.Multiply(scale, vector);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M2__x2t__"/>.
        /// </summary>
        public static M3__x3t__ operator *(Scale__x3t__ scale, M2__x2t__ matrix)
        {
            return Scale__x3t__.Multiply(scale, matrix);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M3__x3t__"/>.
        /// </summary>
        public static M3__x3t__ operator *(Scale__x3t__ scale, M3__x3t__ matrix)
        {
            return Scale__x3t__.Multiply(scale, matrix);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M3__x4t__"/>.
        /// </summary>
        public static M3__x4t__ operator *(Scale__x3t__ scale, M3__x4t__ matrix)
        {
            return Scale__x3t__.Multiply(scale, matrix);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="M4__x4t__"/>.
        /// </summary>
        public static M4__x4t__ operator *(Scale__x3t__ scale, M4__x4t__ matrix)
        {
            return Scale__x3t__.Multiply(scale, matrix);
        }

        /// <summary>
        /// </summary>
        public static M3__x3t__ operator *(Scale__x3t__ scale, Rot__x3t__ quaternion)
        {
            return Scale__x3t__.Multiply(scale, quaternion);
        }

        /// <summary>
        /// </summary>
        public static M3__x3t__ operator *(Scale__x3t__ scale, Rot__x2t__ rotation)
        {
            return Scale__x3t__.Multiply(scale, rotation);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a __ft__ scalar.
        /// </summary>
        public static Scale__x3t__ operator *(__ft__ scalar, Scale__x3t__ scale)
        {
            return Scale__x3t__.Multiply(scale, scalar);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static Scale__x3t__ operator *(Scale__x3t__ scale1, Scale__x3t__ scale2)
        {
            return Scale__x3t__.Multiply(scale1, scale2);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="V__x3t__"/> with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static V__x3t__ operator *(V__x3t__ vector, Scale__x3t__ scale)
        {
            return Scale__x3t__.Multiply(scale, vector);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale__x3t__"/> with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static M3__x4t__ operator *(Scale__x3t__ scale, Shift__x3t__ shift)
        {
            return Scale__x3t__.Multiply(scale, shift);
        }

        /// <summary>
        /// Calculates the division of a <see cref="Scale__x3t__"/> with a __ft__ scalar.
        /// </summary>
        public static Scale__x3t__ operator /(Scale__x3t__ scale, __ft__ value)
        {
            return Scale__x3t__.Divide(scale, value);
        }

        /// <summary>
        /// Calculates the division of a __ft__ scalar with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static Scale__x3t__ operator /(__ft__ value, Scale__x3t__ scale)
        {
            return Scale__x3t__.Divide(value, scale);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Scale__x3t__"/>s are equal.
        /// </summary>
        public static bool operator ==(Scale__x3t__ s0, Scale__x3t__ s1)
        {
            return s0.X == s1.X && s0.Y == s1.Y && s0.Z == s1.Z;
        }

        /// <summary>
        /// Checks whether two <see cref="Scale__x3t__"/> instances are different.
        /// </summary>
        public static bool operator !=(Scale__x3t__ s0, Scale__x3t__ s1)
        {
            return s0.X != s1.X || s0.Y != s1.Y || s0.Z != s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale__x3t__"/> 
        /// are bigger then the components of the second <see cref="Scale__x3t__"/>.
        /// </summary>
        public static bool operator >(Scale__x3t__ s0, Scale__x3t__ s1)
        {
            return s0.X > s1.X && s0.Y > s1.Y && s0.Z > s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale__x3t__"/> 
        /// are bigger or equal then the components of the second <see cref="Scale__x3t__"/>.
        /// </summary>
        public static bool operator >=(Scale__x3t__ s0, Scale__x3t__ s1)
        {
            return s0.X >= s1.X && s0.Y >= s1.Y && s0.Z >= s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale__x3t__"/> 
        /// are smaller then the components of the second <see cref="Scale__x3t__"/>.
        /// </summary>
        public static bool operator <(Scale__x3t__ s0, Scale__x3t__ s1)
        {
            return ((s0.X < s1.X) && (s0.Y < s1.Y) && (s0.Z < s1.Z));
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale__x3t__"/> 
        /// are smaller or equal then the components of the second <see cref="Scale__x3t__"/>.
        /// </summary>
        public static bool operator <=(Scale__x3t__ s0, Scale__x3t__ s1)
        {
            return s0.X <= s1.X && s0.Y <= s1.Y && s0.Z <= s1.Z;
        }

        #endregion

        #region Convert to Array

        public static explicit operator M3__x3t__(Scale__x3t__ s)
        {
            return new M3__x3t__(s.X, 0, 0,
                            0, s.Y, 0,
                            0, 0, s.Z);
        }

        public static explicit operator M4__x4t__(Scale__x3t__ s)
        {
            return new M4__x4t__(s.X, 0, 0, 0,
                            0, s.Y, 0, 0,
                            0, 0, s.Z, 0,
                            0, 0, 0, 1);
        }

        public static explicit operator M3__x4t__(Scale__x3t__ s)
        {
            return new M3__x4t__(s.X, 0, 0, 0,
                            0, s.Y, 0, 0,
                            0, 0, s.Z, 0);
        }

        /// <summary>
        /// Returns all values of a <see cref="Scale__x3t__"/> instance
        /// in a __ft__[] array.
        /// </summary>
        public static explicit operator __ft__[](Scale__x3t__ s)
        {
            __ft__[] array = new __ft__[3];
            array[0] = s.X;
            array[1] = s.Y;
            array[2] = s.Z;
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

        #region Overrides

        public override int GetHashCode()
        {
            return V.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Scale__x3t__)
            {
                Scale__x3t__ scalingVector = (Scale__x3t__)obj;
                return (X == scalingVector.X) && (Y == scalingVector.Y) && (Z == scalingVector.Z);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Scale__x3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Scale__x3t__(
                __ft__.Parse(x[0], CultureInfo.InvariantCulture),
                __ft__.Parse(x[1], CultureInfo.InvariantCulture),
                __ft__.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion
    }
    //# } // isDouble
}

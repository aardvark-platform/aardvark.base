using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    #region Scale3f

    /// <summary>
    /// A three dimensional scaling transform with different scaling values
    /// in each dimension.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Scale3f
    {
        [DataMember]
        public V3f V;

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="Scale3f"/> using three floats.
        /// </summary>
        public Scale3f(float x, float y, float z)
        {
            V = new V3f(x, y, z);
        }

        public Scale3f(V3f v)
        {
            V = v;
        }

        /// <summary>
        /// Initializes a <see cref="Scale3f"/> using a uniform float value.
        /// </summary>
        public Scale3f(float uniform)
        {
            V = new V3f(uniform, uniform, uniform);
        }

        /// <summary>
        /// Initializes a <see cref="Scale3f"/> class from float-array.
        /// </summary>
        public Scale3f(float[] array)
        {
            V = new V3f(array);
        }

        /// <summary>
        /// Initializes a <see cref="Scale3f"/> class from float-array starting from passed index
        /// </summary>
        public Scale3f(float[] array, int start)
        {
            V = new V3f(array, start);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the X coordinate.
        /// </summary>
        public float X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.X; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.X = value; }
        }

        /// <summary>
        /// Gets and sets the Y coordinate.
        /// </summary>
        public float Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Y; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.Y = value; }
        }

        /// <summary>
        /// Gets and sets the Z coordinate.
        /// </summary>
        public float Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Z; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.Z = value; }
        }

        /// <summary>
        /// Calculates the length of a <see cref="Scale3f"/>.
        /// </summary>
        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Length; }
        }

        /// <summary>
        /// Calculates the squared length of a <see cref="Scale3f"/>.
        /// </summary>
        public float LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.LengthSquared; }
        }

        public Scale3f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Scale3f(V.Reciprocal); }
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="Scale3f"/>.
        /// </summary>
        public Scale3f Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3f(1 / X, 1 / Y, 1 / Z);
        }

        #endregion

        #region Constants

        public static Scale3f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3f(1, 1, 1);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> single-precision floating point zero shift vector.
        /// </summary>
        public static Scale3f Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3f(0, 0, 0);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> single-precision floating point X-Axis shift vector.
        /// </summary>
        public static Scale3f XAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3f(1, 0, 0);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> single-precision floating point Y-Axis shift vector.
        /// </summary>
        public static Scale3f YAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3f(0, 1, 0);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> single-precision floating point Z-Axis shift vector.
        /// </summary>
        public static Scale3f ZAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3f(0, 0, 1);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Negates the values of a <see cref="Scale3f"/> instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3f operator -(Scale3f scale)
        {
            return new Scale3f(-scale.X, -scale.Y, -scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a float scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3f operator *(Scale3f scale, float scalar)
        {
            return new Scale3f(scale.X * scalar, scale.Y * scalar, scale.Z * scalar);
        }

        /// <summary>
        /// Calculates the multiplacation of a float scalar with a <see cref="Scale3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3f operator *(float scalar, Scale3f scale)
        {
            return new Scale3f(scale.X * scalar, scale.Y * scalar, scale.Z * scalar);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="V2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f operator *(Scale3f scale, V2f vector)
        {
            return new V2f(vector.X * scale.X, vector.Y * scale.Y);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="V2f"/> with a <see cref="Scale3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f operator *(V2f vector, Scale3f scale)
        {
            return new V2f(vector.X * scale.X, vector.Y * scale.Y);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="V3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f operator *(Scale3f scale, V3f vector)
        {
            return new V3f(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="V3f"/> with a <see cref="Scale3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f operator *(V3f vector, Scale3f scale)
        {
            return new V3f(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="V4f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f operator *(Scale3f scale, V4f vector)
        {
            return new V4f(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="V4f"/> with a <see cref="Scale3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f operator *(V4f vector, Scale3f scale)
        {
            return new V4f(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="M22f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Scale3f scale, M22f matrix)
        {
            return new M33f(scale.X * matrix.M00, scale.X * matrix.M01, 0,
                             scale.Y * matrix.M10, scale.Y * matrix.M11, 0,
                             0, 0, scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="M33f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Scale3f scale, M33f matrix)
        {
            return new M33f(scale.X * matrix.M00,
                 scale.X * matrix.M01,
                 scale.X * matrix.M02,

                 scale.Y * matrix.M10,
                 scale.Y * matrix.M11,
                 scale.Y * matrix.M12,

                 scale.Z * matrix.M20,
                 scale.Z * matrix.M21,
                 scale.Z * matrix.M22);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="M34f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Scale3f scale, M34f matrix)
        {
            return new M34f(
                    scale.X * matrix.M00,
                    scale.X * matrix.M01,
                    scale.X * matrix.M02,
                    scale.X * matrix.M03,

                    scale.Y * matrix.M10,
                    scale.Y * matrix.M11,
                    scale.Y * matrix.M12,
                    scale.Y * matrix.M13,

                    scale.Z * matrix.M20,
                    scale.Z * matrix.M21,
                    scale.Z * matrix.M22,
                    scale.Z * matrix.M23
                    );
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="M44f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f operator *(Scale3f scale, M44f matrix)
        {
            return new M44f(
                    scale.X * matrix.M00,
                    scale.X * matrix.M01,
                    scale.X * matrix.M02,
                    scale.X * matrix.M03,

                    scale.Y * matrix.M10,
                    scale.Y * matrix.M11,
                    scale.Y * matrix.M12,
                    scale.Y * matrix.M13,

                    scale.Z * matrix.M20,
                    scale.Z * matrix.M21,
                    scale.Z * matrix.M22,
                    scale.Z * matrix.M23,

                    matrix.M30,
                    matrix.M31,
                    matrix.M32,
                    matrix.M33
                    );
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Scale3f scale, Rot3f rotation)
        {
            return scale * (M33f)rotation;
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Scale3f scale, Rot2f rotation)
        {
            return scale * (M22f)rotation;
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="Scale3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3f operator *(Scale3f scale1, Scale3f scale2)
        {
            return new Scale3f(scale1.X * scale2.X, scale1.Y * scale2.Y, scale1.Z * scale2.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3f"/> with a <see cref="Shift3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Scale3f scale, Shift3f shift)
        {
            return new M34f(scale.X, 0, 0, scale.X * shift.X,
                    0, scale.Y, 0, scale.Y * shift.Y,
                    0, 0, scale.Z, scale.Z * shift.Z);
        }

        /// <summary>
        /// Calculates the division of a <see cref="Scale3f"/> with a float scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3f operator /(Scale3f scale, float value)
        {
            return scale * (1 / value);
        }

        /// <summary>
        /// Calculates the division of a float scalar with a <see cref="Scale3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3f operator /(float value, Scale3f scale)
        {
            return scale.Reciprocal * value;
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Scale3f"/>s are equal.
        /// </summary>
        public static bool operator ==(Scale3f s0, Scale3f s1)
        {
            return s0.X == s1.X && s0.Y == s1.Y && s0.Z == s1.Z;
        }

        /// <summary>
        /// Checks whether two <see cref="Scale3f"/> instances are different.
        /// </summary>
        public static bool operator !=(Scale3f s0, Scale3f s1)
        {
            return s0.X != s1.X || s0.Y != s1.Y || s0.Z != s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3f"/> 
        /// are bigger then the components of the second <see cref="Scale3f"/>.
        /// </summary>
        public static bool operator >(Scale3f s0, Scale3f s1)
        {
            return s0.X > s1.X && s0.Y > s1.Y && s0.Z > s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3f"/> 
        /// are bigger or equal then the components of the second <see cref="Scale3f"/>.
        /// </summary>
        public static bool operator >=(Scale3f s0, Scale3f s1)
        {
            return s0.X >= s1.X && s0.Y >= s1.Y && s0.Z >= s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3f"/> 
        /// are smaller then the components of the second <see cref="Scale3f"/>.
        /// </summary>
        public static bool operator <(Scale3f s0, Scale3f s1)
        {
            return ((s0.X < s1.X) && (s0.Y < s1.Y) && (s0.Z < s1.Z));
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3f"/> 
        /// are smaller or equal then the components of the second <see cref="Scale3f"/>.
        /// </summary>
        public static bool operator <=(Scale3f s0, Scale3f s1)
        {
            return s0.X <= s1.X && s0.Y <= s1.Y && s0.Z <= s1.Z;
        }

        #endregion

        #region Convert to Array

        public static explicit operator M33f(Scale3f s)
        {
            return new M33f(s.X, 0, 0,
                            0, s.Y, 0,
                            0, 0, s.Z);
        }

        public static explicit operator M44f(Scale3f s)
        {
            return new M44f(s.X, 0, 0, 0,
                            0, s.Y, 0, 0,
                            0, 0, s.Z, 0,
                            0, 0, 0, 1);
        }

        public static explicit operator M34f(Scale3f s)
        {
            return new M34f(s.X, 0, 0, 0,
                            0, s.Y, 0, 0,
                            0, 0, s.Z, 0);
        }

        /// <summary>
        /// Returns all values of a <see cref="Scale3f"/> instance
        /// in a float[] array.
        /// </summary>
        public static explicit operator float[](Scale3f s)
        {
            float[] array = new float[3];
            array[0] = s.X;
            array[1] = s.Y;
            array[2] = s.Z;
            return array;
        }

        #endregion

        #region Indexing

        public float this[int index]
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
            if (obj is Scale3f)
            {
                Scale3f scalingVector = (Scale3f)obj;
                return (X == scalingVector.X) && (Y == scalingVector.Y) && (Z == scalingVector.Z);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Scale3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Scale3f(
                float.Parse(x[0], CultureInfo.InvariantCulture),
                float.Parse(x[1], CultureInfo.InvariantCulture),
                float.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion
    }

    #endregion

    #region Scale3d

    /// <summary>
    /// A three dimensional scaling transform with different scaling values
    /// in each dimension.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Scale3d
    {
        [DataMember]
        public V3d V;

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="Scale3d"/> using three doubles.
        /// </summary>
        public Scale3d(double x, double y, double z)
        {
            V = new V3d(x, y, z);
        }

        public Scale3d(V3d v)
        {
            V = v;
        }

        /// <summary>
        /// Initializes a <see cref="Scale3d"/> using a uniform double value.
        /// </summary>
        public Scale3d(double uniform)
        {
            V = new V3d(uniform, uniform, uniform);
        }

        /// <summary>
        /// Initializes a <see cref="Scale3d"/> class from double-array.
        /// </summary>
        public Scale3d(double[] array)
        {
            V = new V3d(array);
        }

        /// <summary>
        /// Initializes a <see cref="Scale3d"/> class from double-array starting from passed index
        /// </summary>
        public Scale3d(double[] array, int start)
        {
            V = new V3d(array, start);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the X coordinate.
        /// </summary>
        public double X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.X; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.X = value; }
        }

        /// <summary>
        /// Gets and sets the Y coordinate.
        /// </summary>
        public double Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Y; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.Y = value; }
        }

        /// <summary>
        /// Gets and sets the Z coordinate.
        /// </summary>
        public double Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Z; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { V.Z = value; }
        }

        /// <summary>
        /// Calculates the length of a <see cref="Scale3d"/>.
        /// </summary>
        public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.Length; }
        }

        /// <summary>
        /// Calculates the squared length of a <see cref="Scale3d"/>.
        /// </summary>
        public double LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return V.LengthSquared; }
        }

        public Scale3d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Scale3d(V.Reciprocal); }
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="Scale3d"/>.
        /// </summary>
        public Scale3d Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3d(1 / X, 1 / Y, 1 / Z);
        }

        #endregion

        #region Constants

        public static Scale3d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3d(1, 1, 1);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> double-precision floating point zero shift vector.
        /// </summary>
        public static Scale3d Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3d(0, 0, 0);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> double-precision floating point X-Axis shift vector.
        /// </summary>
        public static Scale3d XAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3d(1, 0, 0);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> double-precision floating point Y-Axis shift vector.
        /// </summary>
        public static Scale3d YAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3d(0, 1, 0);
        }

        /// <summary>
        /// A <see cref="Scale3d"/> double-precision floating point Z-Axis shift vector.
        /// </summary>
        public static Scale3d ZAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Scale3d(0, 0, 1);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Negates the values of a <see cref="Scale3d"/> instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3d operator -(Scale3d scale)
        {
            return new Scale3d(-scale.X, -scale.Y, -scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a double scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3d operator *(Scale3d scale, double scalar)
        {
            return new Scale3d(scale.X * scalar, scale.Y * scalar, scale.Z * scalar);
        }

        /// <summary>
        /// Calculates the multiplacation of a double scalar with a <see cref="Scale3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3d operator *(double scalar, Scale3d scale)
        {
            return new Scale3d(scale.X * scalar, scale.Y * scalar, scale.Z * scalar);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="V2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d operator *(Scale3d scale, V2d vector)
        {
            return new V2d(vector.X * scale.X, vector.Y * scale.Y);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="V2d"/> with a <see cref="Scale3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d operator *(V2d vector, Scale3d scale)
        {
            return new V2d(vector.X * scale.X, vector.Y * scale.Y);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="V3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d operator *(Scale3d scale, V3d vector)
        {
            return new V3d(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="V3d"/> with a <see cref="Scale3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d operator *(V3d vector, Scale3d scale)
        {
            return new V3d(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="V4d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d operator *(Scale3d scale, V4d vector)
        {
            return new V4d(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="V4d"/> with a <see cref="Scale3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d operator *(V4d vector, Scale3d scale)
        {
            return new V4d(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="M22d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Scale3d scale, M22d matrix)
        {
            return new M33d(scale.X * matrix.M00, scale.X * matrix.M01, 0,
                             scale.Y * matrix.M10, scale.Y * matrix.M11, 0,
                             0, 0, scale.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="M33d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Scale3d scale, M33d matrix)
        {
            return new M33d(scale.X * matrix.M00,
                 scale.X * matrix.M01,
                 scale.X * matrix.M02,

                 scale.Y * matrix.M10,
                 scale.Y * matrix.M11,
                 scale.Y * matrix.M12,

                 scale.Z * matrix.M20,
                 scale.Z * matrix.M21,
                 scale.Z * matrix.M22);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="M34d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Scale3d scale, M34d matrix)
        {
            return new M34d(
                    scale.X * matrix.M00,
                    scale.X * matrix.M01,
                    scale.X * matrix.M02,
                    scale.X * matrix.M03,

                    scale.Y * matrix.M10,
                    scale.Y * matrix.M11,
                    scale.Y * matrix.M12,
                    scale.Y * matrix.M13,

                    scale.Z * matrix.M20,
                    scale.Z * matrix.M21,
                    scale.Z * matrix.M22,
                    scale.Z * matrix.M23
                    );
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="M44d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d operator *(Scale3d scale, M44d matrix)
        {
            return new M44d(
                    scale.X * matrix.M00,
                    scale.X * matrix.M01,
                    scale.X * matrix.M02,
                    scale.X * matrix.M03,

                    scale.Y * matrix.M10,
                    scale.Y * matrix.M11,
                    scale.Y * matrix.M12,
                    scale.Y * matrix.M13,

                    scale.Z * matrix.M20,
                    scale.Z * matrix.M21,
                    scale.Z * matrix.M22,
                    scale.Z * matrix.M23,

                    matrix.M30,
                    matrix.M31,
                    matrix.M32,
                    matrix.M33
                    );
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Scale3d scale, Rot3d rotation)
        {
            return scale * (M33d)rotation;
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Scale3d scale, Rot2d rotation)
        {
            return scale * (M22d)rotation;
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="Scale3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3d operator *(Scale3d scale1, Scale3d scale2)
        {
            return new Scale3d(scale1.X * scale2.X, scale1.Y * scale2.Y, scale1.Z * scale2.Z);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Scale3d"/> with a <see cref="Shift3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Scale3d scale, Shift3d shift)
        {
            return new M34d(scale.X, 0, 0, scale.X * shift.X,
                    0, scale.Y, 0, scale.Y * shift.Y,
                    0, 0, scale.Z, scale.Z * shift.Z);
        }

        /// <summary>
        /// Calculates the division of a <see cref="Scale3d"/> with a double scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3d operator /(Scale3d scale, double value)
        {
            return scale * (1 / value);
        }

        /// <summary>
        /// Calculates the division of a double scalar with a <see cref="Scale3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale3d operator /(double value, Scale3d scale)
        {
            return scale.Reciprocal * value;
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Scale3d"/>s are equal.
        /// </summary>
        public static bool operator ==(Scale3d s0, Scale3d s1)
        {
            return s0.X == s1.X && s0.Y == s1.Y && s0.Z == s1.Z;
        }

        /// <summary>
        /// Checks whether two <see cref="Scale3d"/> instances are different.
        /// </summary>
        public static bool operator !=(Scale3d s0, Scale3d s1)
        {
            return s0.X != s1.X || s0.Y != s1.Y || s0.Z != s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3d"/> 
        /// are bigger then the components of the second <see cref="Scale3d"/>.
        /// </summary>
        public static bool operator >(Scale3d s0, Scale3d s1)
        {
            return s0.X > s1.X && s0.Y > s1.Y && s0.Z > s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3d"/> 
        /// are bigger or equal then the components of the second <see cref="Scale3d"/>.
        /// </summary>
        public static bool operator >=(Scale3d s0, Scale3d s1)
        {
            return s0.X >= s1.X && s0.Y >= s1.Y && s0.Z >= s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3d"/> 
        /// are smaller then the components of the second <see cref="Scale3d"/>.
        /// </summary>
        public static bool operator <(Scale3d s0, Scale3d s1)
        {
            return ((s0.X < s1.X) && (s0.Y < s1.Y) && (s0.Z < s1.Z));
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Scale3d"/> 
        /// are smaller or equal then the components of the second <see cref="Scale3d"/>.
        /// </summary>
        public static bool operator <=(Scale3d s0, Scale3d s1)
        {
            return s0.X <= s1.X && s0.Y <= s1.Y && s0.Z <= s1.Z;
        }

        #endregion

        #region Convert to Array

        public static explicit operator M33d(Scale3d s)
        {
            return new M33d(s.X, 0, 0,
                            0, s.Y, 0,
                            0, 0, s.Z);
        }

        public static explicit operator M44d(Scale3d s)
        {
            return new M44d(s.X, 0, 0, 0,
                            0, s.Y, 0, 0,
                            0, 0, s.Z, 0,
                            0, 0, 0, 1);
        }

        public static explicit operator M34d(Scale3d s)
        {
            return new M34d(s.X, 0, 0, 0,
                            0, s.Y, 0, 0,
                            0, 0, s.Z, 0);
        }

        /// <summary>
        /// Returns all values of a <see cref="Scale3d"/> instance
        /// in a double[] array.
        /// </summary>
        public static explicit operator double[](Scale3d s)
        {
            double[] array = new double[3];
            array[0] = s.X;
            array[1] = s.Y;
            array[2] = s.Z;
            return array;
        }

        #endregion

        #region Indexing

        public double this[int index]
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
            if (obj is Scale3d)
            {
                Scale3d scalingVector = (Scale3d)obj;
                return (X == scalingVector.X) && (Y == scalingVector.Y) && (Z == scalingVector.Z);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Scale3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Scale3d(
                double.Parse(x[0], CultureInfo.InvariantCulture),
                double.Parse(x[1], CultureInfo.InvariantCulture),
                double.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion
    }

    #endregion

}

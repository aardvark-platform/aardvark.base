using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Shift3f
    {
        public V3f V;

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="Shift3f"/> using three floats.
        /// </summary>
        public Shift3f(float x, float y, float z)
        {
            V = new V3f(x, y, z);
        }

        public Shift3f(V3f v)
        {
            V = v;
        }

        /// <summary>
        /// Initializes a <see cref="Shift3f"/> class from float-array.
        /// </summary>
        public Shift3f(float[] array)
        {
            V = new V3f(array);
        }

        /// <summary>
        /// Initializes a <see cref="Shift3f"/> class from float-array starting from passed index
        /// </summary>
        public Shift3f(float[] array, int start)
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
            get { return V.X; }
            set { V.X = value; }
        }

        /// <summary>
        /// Gets and sets the Y coordinate.
        /// </summary>
        public float Y
        {
            get { return V.Y; }
            set { V.Y = value; }
        }

        /// <summary>
        /// Gets and sets the Z coordinate.
        /// </summary>
        public float Z
        {
            get { return V.Z; }
            set { V.Z = value; }
        }

        /// <summary>
        /// Calculates the length of a <see cref="Shift3f"/>.
        /// </summary>
        /// <returns>A float scalar.</returns>
        public float Length
        {
            get { return V.Length; }
        }

        /// <summary>
        /// Calculates the squared length of a <see cref="Shift3f"/>.
        /// </summary>
        /// <returns>A float scalar.</returns>
        public float LengthSquared
        {
            get { return V.LengthSquared; }
        }

        public Shift3f Inverse
        {
            get { return new Shift3f(V.Negated); }
        }

        #endregion

        #region Constants

        /// <summary>
        /// A <see cref="Shift3f"/> single-precision floating point zero shift vector.
        /// </summary>
        public static readonly Shift3f Zero = new Shift3f(0, 0, 0);
        public static readonly Shift3f Identity = new Shift3f(0, 0, 0);

        /// <summary>
        /// A <see cref="Shift3f"/> single-precision floating point X-Axis shift vector.
        /// </summary>
        public static readonly Shift3f XAxis = new Shift3f(1, 0, 0);

        /// <summary>
        /// A <see cref="Shift3f"/> single-precision floating point Y-Axis shift vector.
        /// </summary>
        public static readonly Shift3f YAxis = new Shift3f(0, 1, 0);

        /// <summary>
        /// A <see cref="Shift3f"/> single-precision floating point Z-Axis shift vector.
        /// </summary>
        public static readonly Shift3f ZAxis = new Shift3f(0, 0, 1);

        #endregion

        #region Vector Arithmetics
        //different calculations for shift vectors

        /// <summary>
        /// Multiplacation of a float scalar with a <see cref="Shift3f"/>.
        /// </summary>
        public static Shift3f Multiply(Shift3f shift, float value)
        {
            return new Shift3f(shift.X * value,
                               shift.Y * value,
                               shift.Z * value);
        }

        /// <summary>
        /// Multiplication of two <see cref="Shift3f"/>s.
        /// </summary>
        public static Shift3f Multiply(Shift3f shift0, Shift3f shift1)
        {
            return new Shift3f(shift0.X + shift1.X,
                               shift0.Y + shift1.Y,
                               shift0.Z + shift1.Z);
        }

        public static M34f Multiply(Shift3f shift, Scale3f scale)
        {
            return new M34f(scale.X, 0, 0, shift.X,
                            0, scale.Y, 0, shift.Y,
                            0, 0, scale.Z, shift.Z);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Shift3f"/> with a <see cref="M44f"/>.
        /// </summary>
        public static M44f Multiply(Shift3f shift, M44f m)
        {
            return new M44f(
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
        /// Multiplacation of a <see cref="Shift3f"/> with a <see cref="M34f"/>.
        /// </summary>
        public static M34f Multiply(Shift3f shift, M34f m)
        {
            return new M34f(
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
        /// Division of a <see cref="Shift3f"/> instance with a float scalar.
        /// </summary>
        public static Shift3f Divide(Shift3f shift, float val)
        {
            return Multiply(shift, 1 / val);
        }

        /// <summary>
        /// Division of a float scalar with a <see cref="Shift3f"/>.
        /// </summary>
        public static Shift3f Divide(float value, Shift3f shift)
        {
            return Multiply(Reciprocal(shift), value);
        }

        /// <summary>
        /// Negates all values of a <see cref="Shift3f"/>.
        /// </summary>
        public static Shift3f Negate(Shift3f shift)
        {
            return new Shift3f(-shift.X, -shift.Y, -shift.Z);
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="Shift3f"/>.
        /// </summary>
        public static Shift3f Reciprocal(Shift3f shift)
        {
            return new Shift3f(1 / shift.X, 1 / shift.Y, 1 / shift.Z);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Negates the values of a <see cref="Shift3f"/> instance.
        /// </summary>
        public static Shift3f operator -(Shift3f shift)
        {
            return Shift3f.Negate(shift);
        }

        /// <summary>
        /// Calculates the multiplacation of a float scalar with a <see cref="Shift3f"/>.
        /// </summary>
        public static Shift3f operator *(Shift3f shift, float value)
        {
            return Shift3f.Multiply(shift, value);
        }

        /// <summary>
        /// </summary>
        public static V4f operator *(Shift3f shift, V4f vec)
        {
            return new V4f(vec.X + shift.X * vec.W,
                            vec.Y + shift.Y * vec.W,
                            vec.Z + shift.Z * vec.W,
                            vec.W);
        }

        /// <summary>
        /// </summary>
        public static M34f operator *(Shift3f shift, M22f mat)
        {
            return new M34f(mat.M00, mat.M01, 0, shift.X,
                             mat.M10, mat.M11, 0, shift.Y,
                             0, 0, 1, shift.Z);
        }

        /// <summary>
        /// </summary>
        public static M34f operator *(Shift3f shift, M33f mat)
        {
            return new M34f(mat.M00, mat.M01, mat.M02, shift.X,
                             mat.M10, mat.M11, mat.M12, shift.Y,
                             mat.M20, mat.M21, mat.M22, shift.Z);
        }

        /// <summary>
        /// </summary>
        public static M34f operator *(Shift3f shift, Rot3f rot)
        {
            return Shift3f.Multiply(shift, (M34f)rot);
        }

        /// <summary>
        /// </summary>
        public static M34f operator *(Shift3f shift, Rot2f rot)
        {
            return (M34f)(shift * (M22f)rot);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3f"/> with a float scalar.
        /// </summary>
        public static Shift3f operator *(float value, Shift3f shift)
        {
            return Shift3f.Multiply(shift, value);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3f"/> with a <see cref="Shift3f"/>.
        /// </summary>
        public static Shift3f operator *(Shift3f shift0, Shift3f shift1)
        {
            return Shift3f.Multiply(shift0, shift1);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3f"/> with a <see cref="Scale3f"/>.
        /// </summary>
        public static M34f operator *(Shift3f shift, Scale3f scale)
        {
            return Shift3f.Multiply(shift, scale);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3f"/> with a <see cref="M44f"/>.
        /// </summary>
        public static M44f operator *(Shift3f shift, M44f mat)
        {
            return Shift3f.Multiply(shift, mat);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3f"/> with a <see cref="M34f"/>.
        /// </summary>
        public static M34f operator *(Shift3f shift, M34f mat)
        {
            return Shift3f.Multiply(shift, mat);
        }

        /// <summary>
        /// Calculates the division of a <see cref="Shift3f"/> with a float scalar.
        /// </summary>
        public static Shift3f operator /(Shift3f shift, float val)
        {
            return Shift3f.Divide(shift, val);
        }

        /// <summary>
        /// Calculates the division of a float scalar with a <see cref="Shift3f"/>.
        /// </summary>
        public static Shift3f operator /(float val, Shift3f shift)
        {
            return Shift3f.Divide(val, shift);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Shift3f"/>s are equal.
        /// </summary>
        public static bool operator ==(Shift3f s0, Shift3f s1)
        {
            return s0.X == s1.X && s0.Y == s1.Y && s0.Z == s1.Z;
        }

        /// <summary>
        /// Checks whether two <see cref="Shift3f"/> instances are different.
        /// </summary>
        public static bool operator !=(Shift3f s0, Shift3f s1)
        {
            return s0.X != s1.X || s0.Y != s1.Y || s0.Z != s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3f"/> 
        /// are bigger then the components of the second <see cref="Shift3f"/>.
        /// </summary>
        public static bool operator >(Shift3f s0, Shift3f s1)
        {
            return s0.V.X > s1.V.X && s0.V.Y > s1.V.Y && s0.V.Z > s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3f"/> 
        /// are bigger or equal then the components of the second <see cref="Shift3f"/>.
        /// </summary>
        public static bool operator >=(Shift3f s0, Shift3f s1)
        {
            return s0.V.X >= s1.V.X && s0.V.Y >= s1.V.Y && s0.V.Z >= s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3f"/> 
        /// are smaller then the components of the second <see cref="Shift3f"/>.
        /// </summary>
        public static bool operator <(Shift3f s0, Shift3f s1)
        {
            return s0.V.X < s1.V.X && s0.V.Y < s1.V.Y && s0.V.Z < s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3f"/> 
        /// are smaller or equal then the components of the second <see cref="Shift3f"/>.
        /// </summary>
        public static bool operator <=(Shift3f s0, Shift3f s1)
        {
            return s0.V.X <= s1.V.X && s0.V.Y <= s1.V.Y && s0.V.Z <= s1.V.Z;
        }

        #endregion

        #region Conversion Operators

        public static explicit operator M44f(Shift3f s)
        {
            return new M44f(1, 0, 0, s.X,
                            0, 1, 0, s.Y,
                            0, 0, 1, s.Z,
                            0, 0, 0, 1);
        }

        public static explicit operator M34f(Shift3f s)
        {
            return new M34f(1, 0, 0, s.X,
                            0, 1, 0, s.Y,
                            0, 0, 1, s.Z);
        }

        /// <summary>
        /// Returns all values of a <see cref="Shift3f"/> instance
        /// in a float[] array.
        /// </summary>
        public static explicit operator float[](Shift3f shift)
        {
            float[] array = new float[3];
            array[0] = shift.X;
            array[1] = shift.Y;
            array[2] = shift.Z;
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

        #region Override

        public override int GetHashCode()
        {
            return V.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Shift3f)
            {
                Shift3f shift = (Shift3f)obj;
                return V.X == shift.X && V.Y == shift.Y && V.Z == shift.Z;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Shift3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Shift3f(
                float.Parse(x[0], CultureInfo.InvariantCulture),
                float.Parse(x[1], CultureInfo.InvariantCulture),
                float.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion
    }
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Shift3d
    {
        public V3d V;

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="Shift3d"/> using three doubles.
        /// </summary>
        public Shift3d(double x, double y, double z)
        {
            V = new V3d(x, y, z);
        }

        public Shift3d(V3d v)
        {
            V = v;
        }

        /// <summary>
        /// Initializes a <see cref="Shift3d"/> class from double-array.
        /// </summary>
        public Shift3d(double[] array)
        {
            V = new V3d(array);
        }

        /// <summary>
        /// Initializes a <see cref="Shift3d"/> class from double-array starting from passed index
        /// </summary>
        public Shift3d(double[] array, int start)
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
            get { return V.X; }
            set { V.X = value; }
        }

        /// <summary>
        /// Gets and sets the Y coordinate.
        /// </summary>
        public double Y
        {
            get { return V.Y; }
            set { V.Y = value; }
        }

        /// <summary>
        /// Gets and sets the Z coordinate.
        /// </summary>
        public double Z
        {
            get { return V.Z; }
            set { V.Z = value; }
        }

        /// <summary>
        /// Calculates the length of a <see cref="Shift3d"/>.
        /// </summary>
        /// <returns>A double scalar.</returns>
        public double Length
        {
            get { return V.Length; }
        }

        /// <summary>
        /// Calculates the squared length of a <see cref="Shift3d"/>.
        /// </summary>
        /// <returns>A double scalar.</returns>
        public double LengthSquared
        {
            get { return V.LengthSquared; }
        }

        public Shift3d Inverse
        {
            get { return new Shift3d(V.Negated); }
        }

        #endregion

        #region Constants

        /// <summary>
        /// A <see cref="Shift3d"/> double-precision floating point zero shift vector.
        /// </summary>
        public static readonly Shift3d Zero = new Shift3d(0, 0, 0);
        public static readonly Shift3d Identity = new Shift3d(0, 0, 0);

        /// <summary>
        /// A <see cref="Shift3d"/> double-precision floating point X-Axis shift vector.
        /// </summary>
        public static readonly Shift3d XAxis = new Shift3d(1, 0, 0);

        /// <summary>
        /// A <see cref="Shift3d"/> double-precision floating point Y-Axis shift vector.
        /// </summary>
        public static readonly Shift3d YAxis = new Shift3d(0, 1, 0);

        /// <summary>
        /// A <see cref="Shift3d"/> double-precision floating point Z-Axis shift vector.
        /// </summary>
        public static readonly Shift3d ZAxis = new Shift3d(0, 0, 1);

        #endregion

        #region Vector Arithmetics
        //different calculations for shift vectors

        /// <summary>
        /// Multiplacation of a float scalar with a <see cref="Shift3d"/>.
        /// </summary>
        public static Shift3d Multiply(Shift3d shift, double value)
        {
            return new Shift3d(shift.X * value,
                               shift.Y * value,
                               shift.Z * value);
        }

        /// <summary>
        /// Multiplication of two <see cref="Shift3d"/>s.
        /// </summary>
        public static Shift3d Multiply(Shift3d shift0, Shift3d shift1)
        {
            return new Shift3d(shift0.X + shift1.X,
                               shift0.Y + shift1.Y,
                               shift0.Z + shift1.Z);
        }

        public static M34d Multiply(Shift3d shift, Scale3d scale)
        {
            return new M34d(scale.X, 0, 0, shift.X,
                            0, scale.Y, 0, shift.Y,
                            0, 0, scale.Z, shift.Z);
        }

        /// <summary>
        /// Multiplacation of a <see cref="Shift3d"/> with a <see cref="M44d"/>.
        /// </summary>
        public static M44d Multiply(Shift3d shift, M44d m)
        {
            return new M44d(
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
        /// Multiplacation of a <see cref="Shift3d"/> with a <see cref="M34d"/>.
        /// </summary>
        public static M34d Multiply(Shift3d shift, M34d m)
        {
            return new M34d(
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
        /// Division of a <see cref="Shift3d"/> instance with a double scalar.
        /// </summary>
        public static Shift3d Divide(Shift3d shift, double val)
        {
            return Multiply(shift, 1 / val);
        }

        /// <summary>
        /// Division of a double scalar with a <see cref="Shift3d"/>.
        /// </summary>
        public static Shift3d Divide(double value, Shift3d shift)
        {
            return Multiply(Reciprocal(shift), value);
        }

        /// <summary>
        /// Negates all values of a <see cref="Shift3d"/>.
        /// </summary>
        public static Shift3d Negate(Shift3d shift)
        {
            return new Shift3d(-shift.X, -shift.Y, -shift.Z);
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="Shift3d"/>.
        /// </summary>
        public static Shift3d Reciprocal(Shift3d shift)
        {
            return new Shift3d(1 / shift.X, 1 / shift.Y, 1 / shift.Z);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Negates the values of a <see cref="Shift3d"/> instance.
        /// </summary>
        public static Shift3d operator -(Shift3d shift)
        {
            return Shift3d.Negate(shift);
        }

        /// <summary>
        /// Calculates the multiplacation of a double scalar with a <see cref="Shift3d"/>.
        /// </summary>
        public static Shift3d operator *(Shift3d shift, double value)
        {
            return Shift3d.Multiply(shift, value);
        }

        /// <summary>
        /// </summary>
        public static V4d operator *(Shift3d shift, V4d vec)
        {
            return new V4d(vec.X + shift.X * vec.W,
                            vec.Y + shift.Y * vec.W,
                            vec.Z + shift.Z * vec.W,
                            vec.W);
        }

        /// <summary>
        /// </summary>
        public static M34d operator *(Shift3d shift, M22d mat)
        {
            return new M34d(mat.M00, mat.M01, 0, shift.X,
                             mat.M10, mat.M11, 0, shift.Y,
                             0, 0, 1, shift.Z);
        }

        /// <summary>
        /// </summary>
        public static M34d operator *(Shift3d shift, M33d mat)
        {
            return new M34d(mat.M00, mat.M01, mat.M02, shift.X,
                             mat.M10, mat.M11, mat.M12, shift.Y,
                             mat.M20, mat.M21, mat.M22, shift.Z);
        }

        /// <summary>
        /// </summary>
        public static M34d operator *(Shift3d shift, Rot3d rot)
        {
            return Shift3d.Multiply(shift, (M34d)rot);
        }

        /// <summary>
        /// </summary>
        public static M34d operator *(Shift3d shift, Rot2d rot)
        {
            return (M34d)(shift * (M22d)rot);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3d"/> with a double scalar.
        /// </summary>
        public static Shift3d operator *(double value, Shift3d shift)
        {
            return Shift3d.Multiply(shift, value);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3d"/> with a <see cref="Shift3d"/>.
        /// </summary>
        public static Shift3d operator *(Shift3d shift0, Shift3d shift1)
        {
            return Shift3d.Multiply(shift0, shift1);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3d"/> with a <see cref="Scale3d"/>.
        /// </summary>
        public static M34d operator *(Shift3d shift, Scale3d scale)
        {
            return Shift3d.Multiply(shift, scale);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3d"/> with a <see cref="M44d"/>.
        /// </summary>
        public static M44d operator *(Shift3d shift, M44d mat)
        {
            return Shift3d.Multiply(shift, mat);
        }

        /// <summary>
        /// Calculates the multiplacation of a <see cref="Shift3d"/> with a <see cref="M34d"/>.
        /// </summary>
        public static M34d operator *(Shift3d shift, M34d mat)
        {
            return Shift3d.Multiply(shift, mat);
        }

        /// <summary>
        /// Calculates the division of a <see cref="Shift3d"/> with a double scalar.
        /// </summary>
        public static Shift3d operator /(Shift3d shift, double val)
        {
            return Shift3d.Divide(shift, val);
        }

        /// <summary>
        /// Calculates the division of a double scalar with a <see cref="Shift3d"/>.
        /// </summary>
        public static Shift3d operator /(double val, Shift3d shift)
        {
            return Shift3d.Divide(val, shift);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Shift3d"/>s are equal.
        /// </summary>
        public static bool operator ==(Shift3d s0, Shift3d s1)
        {
            return s0.X == s1.X && s0.Y == s1.Y && s0.Z == s1.Z;
        }

        /// <summary>
        /// Checks whether two <see cref="Shift3d"/> instances are different.
        /// </summary>
        public static bool operator !=(Shift3d s0, Shift3d s1)
        {
            return s0.X != s1.X || s0.Y != s1.Y || s0.Z != s1.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3d"/> 
        /// are bigger then the components of the second <see cref="Shift3d"/>.
        /// </summary>
        public static bool operator >(Shift3d s0, Shift3d s1)
        {
            return s0.V.X > s1.V.X && s0.V.Y > s1.V.Y && s0.V.Z > s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3d"/> 
        /// are bigger or equal then the components of the second <see cref="Shift3d"/>.
        /// </summary>
        public static bool operator >=(Shift3d s0, Shift3d s1)
        {
            return s0.V.X >= s1.V.X && s0.V.Y >= s1.V.Y && s0.V.Z >= s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3d"/> 
        /// are smaller then the components of the second <see cref="Shift3d"/>.
        /// </summary>
        public static bool operator <(Shift3d s0, Shift3d s1)
        {
            return s0.V.X < s1.V.X && s0.V.Y < s1.V.Y && s0.V.Z < s1.V.Z;
        }

        /// <summary>
        /// Checks whether all components of a <see cref="Shift3d"/> 
        /// are smaller or equal then the components of the second <see cref="Shift3d"/>.
        /// </summary>
        public static bool operator <=(Shift3d s0, Shift3d s1)
        {
            return s0.V.X <= s1.V.X && s0.V.Y <= s1.V.Y && s0.V.Z <= s1.V.Z;
        }

        #endregion

        #region Conversion Operators

        public static explicit operator M44d(Shift3d s)
        {
            return new M44d(1, 0, 0, s.X,
                            0, 1, 0, s.Y,
                            0, 0, 1, s.Z,
                            0, 0, 0, 1);
        }

        public static explicit operator M34d(Shift3d s)
        {
            return new M34d(1, 0, 0, s.X,
                            0, 1, 0, s.Y,
                            0, 0, 1, s.Z);
        }

        /// <summary>
        /// Returns all values of a <see cref="Shift3d"/> instance
        /// in a double[] array.
        /// </summary>
        public static explicit operator double[](Shift3d shift)
        {
            double[] array = new double[3];
            array[0] = shift.X;
            array[1] = shift.Y;
            array[2] = shift.Z;
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

        #region Override

        public override int GetHashCode()
        {
            return V.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Shift3d)
            {
                Shift3d shift = (Shift3d)obj;
                return V.X == shift.X && V.Y == shift.Y && V.Z == shift.Z;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Shift3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Shift3d(
                double.Parse(x[0], CultureInfo.InvariantCulture),
                double.Parse(x[1], CultureInfo.InvariantCulture),
                double.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion
    }
}

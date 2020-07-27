using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    #region Rot3f

    /// <summary>
    /// Represents a rotation in three dimensions using a unit quaternion.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rot3f
    {
        /// <summary>
        /// Scalar (real) part of the quaternion.
        /// </summary>
        [DataMember]
        public float W;

        /// <summary>
        /// First component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public float X;

        /// <summary>
        /// Second component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public float Y;

        /// <summary>
        /// Third component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public float Z;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="Rot3f"/> transformation from the quaternion (w, (x, y, z)).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3f(float w, float x, float y, float z)
        {
            W = w;
            X = x; Y = y; Z = z;
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-6f));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3f"/> transformation from the quaternion (w, (v.x, v.y, v.z)).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3f(float w, V3f v)
        {
            W = w;
            X = v.X; Y = v.Y; Z = v.Z;
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-6f));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3f"/> transformation from the quaternion <paramref name="q"/>.
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3f(QuaternionF q)
        {
            W = q.W; X = q.X; Y = q.Y; Z = q.Z; 
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-6f));
        }

        /// <summary>
        /// Constructs a copy of a <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3f(Rot3f r)
        {
            W = r.W; X = r.X; Y = r.Y; Z = r.Z; 
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-6f));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3f"/> transformation from the quaternion (a[0], (a[1], a[2], a[3])).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3f(float[] a)
        {
            W = a[0];
            X = a[1]; Y = a[2]; Z = a[3];
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-6f));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3f"/> transformation from the quaternion (a[start], (a[start + 1], a[start + 2], a[start + 3])).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3f(float[] a, int start)
        {
            W = a[start];
            X = a[start + 1]; Y = a[start + 2]; Z = a[start + 3];
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-6f));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the vector part (x, y, z) of this <see cref="Rot3f"/> unit quaternion.
        /// </summary>
        [XmlIgnore]
        public V3f V
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new V3f(X, Y, Z); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        /// <summary>
        /// Gets the squared norm (or squared length) of this <see cref="Rot3f"/>.
        /// May not be exactly 1, due to numerical inaccuracy.
        /// </summary>
        public float NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => W * W + X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Gets the norm (or length) of this <see cref="Rot3f"/>.
        /// May not be exactly 1, due to numerical inaccuracy. 
        /// </summary>
        public float Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NormSquared.Sqrt();
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this <see cref="Rot3f"/>
        /// </summary>
        public Rot3f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var rs = new Rot3f(this);
                rs.Normalize();
                return rs;
            }
        }

        /// <summary>
        /// Gets the inverse of this <see cref="Rot3f"/> transformation.
        /// </summary>
        public Rot3f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-6f));
                return new Rot3f(W, -X, -Y, -Z);
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity <see cref="Rot3f"/>.
        /// </summary>
        public static Rot3f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Rot3f(1, 0, 0, 0);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Returns the component-wise negation of a <see cref="Rot3f"/> unit quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator -(Rot3f q)
            => new Rot3f(-q.W, -q.X, -q.Y, -q.Z);

        /// <summary>
        /// Multiplies two <see cref="Rot3f"/> transformations.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator *(Rot3f a, Rot3f b)
        {
            return new Rot3f(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
        }

        #region Rot / Vector Multiplication

        /// <summary>
        /// Transforms a <see cref="V3f"/> vector by a <see cref="Rot3f"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f operator *(Rot3f r, V3f v)
        {
            var w = -r.X * v.X - r.Y * v.Y - r.Z * v.Z;
            var x = r.W * v.X + r.Y * v.Z - r.Z * v.Y;
            var y = r.W * v.Y + r.Z * v.X - r.X * v.Z;
            var z = r.W * v.Z + r.X * v.Y - r.Y * v.X;

            return new V3f(
                -w * r.X + x * r.W - y * r.Z + z * r.Y,
                -w * r.Y + y * r.W - z * r.X + x * r.Z,
                -w * r.Z + z * r.W - x * r.Y + y * r.X);
        }

        #endregion

        #region Rot / Matrix Multiplication

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> transformation (as a 3x3 matrix) with a <see cref="M33f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Rot3f rot, M33f m)
        {
            return (M33f)rot * m;
        }

        /// <summary>
        /// Multiplies a <see cref="M33f"/> with a <see cref="Rot3f"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(M33f m, Rot3f rot)
        {
            return m * (M33f)rot;
        }

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> transformation (as a 4x4 matrix) with a <see cref="M44f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f operator *(Rot3f rot, M44f m)
        {
            var r = (M33f)rot;
            return new M44f(
                r.M00 * m.M00 + r.M01 * m.M10 + r.M02 * m.M20, 
                r.M00 * m.M01 + r.M01 * m.M11 + r.M02 * m.M21, 
                r.M00 * m.M02 + r.M01 * m.M12 + r.M02 * m.M22, 
                r.M00 * m.M03 + r.M01 * m.M13 + r.M02 * m.M23,

                r.M10 * m.M00 + r.M11 * m.M10 + r.M12 * m.M20, 
                r.M10 * m.M01 + r.M11 * m.M11 + r.M12 * m.M21, 
                r.M10 * m.M02 + r.M11 * m.M12 + r.M12 * m.M22, 
                r.M10 * m.M03 + r.M11 * m.M13 + r.M12 * m.M23,

                r.M20 * m.M00 + r.M21 * m.M10 + r.M22 * m.M20, 
                r.M20 * m.M01 + r.M21 * m.M11 + r.M22 * m.M21, 
                r.M20 * m.M02 + r.M21 * m.M12 + r.M22 * m.M22, 
                r.M20 * m.M03 + r.M21 * m.M13 + r.M22 * m.M23,

                m.M30, m.M31, m.M32, m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="M44f"/> with a <see cref="Rot3f"/> transformation (as a 4x4 matrix) .
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f operator *(M44f m, Rot3f rot)
        {
            var r = (M33f)rot;
            return new M44f(
                m.M00 * r.M00 + m.M01 * r.M10 + m.M02 * r.M20, 
                m.M00 * r.M01 + m.M01 * r.M11 + m.M02 * r.M21, 
                m.M00 * r.M02 + m.M01 * r.M12 + m.M02 * r.M22,
                m.M03,

                m.M10 * r.M00 + m.M11 * r.M10 + m.M12 * r.M20, 
                m.M10 * r.M01 + m.M11 * r.M11 + m.M12 * r.M21, 
                m.M10 * r.M02 + m.M11 * r.M12 + m.M12 * r.M22,
                m.M13,

                m.M20 * r.M00 + m.M21 * r.M10 + m.M22 * r.M20, 
                m.M20 * r.M01 + m.M21 * r.M11 + m.M22 * r.M21, 
                m.M20 * r.M02 + m.M21 * r.M12 + m.M22 * r.M22,
                m.M23,

                m.M30 * r.M00 + m.M31 * r.M10 + m.M32 * r.M20, 
                m.M30 * r.M01 + m.M31 * r.M11 + m.M32 * r.M21, 
                m.M30 * r.M02 + m.M31 * r.M12 + m.M32 * r.M22,
                m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> transformation (as a 3x3 matrix) with a <see cref="M34f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Rot3f rot, M34f m)
        {
            return (M33f)rot * m;
        }

        /// <summary>
        /// Multiplies a <see cref="M34f"/> with a <see cref="Rot3f"/> transformation (as a 4x4 matrix) .
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(M34f m, Rot3f rot)
        {
            var r = (M33f)rot;
            return new M34f(
                m.M00 * r.M00 + m.M01 * r.M10 + m.M02 * r.M20, 
                m.M00 * r.M01 + m.M01 * r.M11 + m.M02 * r.M21, 
                m.M00 * r.M02 + m.M01 * r.M12 + m.M02 * r.M22,
                m.M03,

                m.M10 * r.M00 + m.M11 * r.M10 + m.M12 * r.M20, 
                m.M10 * r.M01 + m.M11 * r.M11 + m.M12 * r.M21, 
                m.M10 * r.M02 + m.M11 * r.M12 + m.M12 * r.M22,
                m.M13,

                m.M20 * r.M00 + m.M21 * r.M10 + m.M22 * r.M20, 
                m.M20 * r.M01 + m.M21 * r.M11 + m.M22 * r.M21, 
                m.M20 * r.M02 + m.M21 * r.M12 + m.M22 * r.M22,
                m.M23);
        }

        #endregion

        #region Rot / Quaternion arithmetics

        /// <summary>
        /// Returns the sum of a <see cref="Rot3f"/> and a <see cref="QuaternionF"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator +(Rot3f r, QuaternionF q)
            => new QuaternionF(r.W + q.W, r.X + q.X, r.Y + q.Y, r.Z + q.Z);

        /// <summary>
        /// Returns the sum of a <see cref="QuaternionF"/> and a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator +(QuaternionF q, Rot3f r)
            => new QuaternionF(q.W + r.W, q.X + r.X, q.Y + r.Y, q.Z + r.Z);

        /// <summary>
        /// Returns the sum of a <see cref="Rot3f"/> and a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator +(Rot3f r, float s)
            => new QuaternionF(r.W + s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the sum of a real scalar and a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator +(float s, Rot3f r)
            => new QuaternionF(r.W + s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the difference between a <see cref="Rot3f"/> and a <see cref="QuaternionF"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator -(Rot3f r, QuaternionF q)
            => new QuaternionF(r.W - q.W, r.X - q.X, r.Y - q.Y, r.Z - q.Z);

        /// <summary>
        /// Returns the difference between a <see cref="QuaternionF"/> and a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator -(QuaternionF q, Rot3f r)
            => new QuaternionF(q.W - r.W, q.X - r.X, q.Y - r.Y, q.Z - r.Z);

        /// <summary>
        /// Returns the difference between a <see cref="Rot3f"/> and a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator -(Rot3f r, float s)
            => new QuaternionF(r.W - s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the difference between a real scalar and a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator -(float s, Rot3f r)
            => new QuaternionF(s - r.W, -r.X, -r.Y, -r.Z);

        /// <summary>
        /// Returns the product of a <see cref="Rot3f"/> and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator *(Rot3f r, float s)
            => new QuaternionF(r.W * s, r.X * s, r.Y * s, r.Z * s);

        /// <summary>
        /// Returns the product of a scalar and a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator *(float s, Rot3f r)
            => new QuaternionF(r.W * s, r.X * s, r.Y * s, r.Z * s);

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> with a <see cref="QuaternionF"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator *(Rot3f r, QuaternionF q)
        {
            return new QuaternionF(
                r.W * q.W - r.X * q.X - r.Y * q.Y - r.Z * q.Z,
                r.W * q.X + r.X * q.W + r.Y * q.Z - r.Z * q.Y,
                r.W * q.Y + r.Y * q.W + r.Z * q.X - r.X * q.Z,
                r.W * q.Z + r.Z * q.W + r.X * q.Y - r.Y * q.X);
        }

        /// <summary>
        /// Multiplies a <see cref="QuaternionF"/> with a <see cref="Rot3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator *(QuaternionF q, Rot3f r)
        {
            return new QuaternionF(
                q.W * r.W - q.X * r.X - q.Y * r.Y - q.Z * r.Z,
                q.W * r.X + q.X * r.W + q.Y * r.Z - q.Z * r.Y,
                q.W * r.Y + q.Y * r.W + q.Z * r.X - q.X * r.Z,
                q.W * r.Z + q.Z * r.W + q.X * r.Y - q.Y * r.X);
        }

        /// <summary>
        /// Divides a <see cref="Rot3f"/> by a <see cref="QuaternionF"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator /(Rot3f r, QuaternionF q)
            => r * q.Inverse;

        /// <summary>
        /// Divides a <see cref="QuaternionF"/> by a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator /(QuaternionF q, Rot3f r)
            => q * r.Inverse;

        /// <summary>
        /// Divides a <see cref="Rot3f"/> by a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator /(Rot3f r, float s)
            => new QuaternionF(r.W / s, r.X / s, r.Y / s, r.Z / s);

        /// <summary>
        /// Divides a scalar by a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionF operator /(float s, Rot3f r)
            => new QuaternionF(s / r.W, s / r.X, s / r.Y, s / r.Z);

        #endregion

        #region Rot / Shift, Scale Multiplication

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> transformation with a <see cref="Shift3f"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f operator *(Rot3f a, Shift3f b)
            => new Euclidean3f(a, a * b.V);

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> transformation with a <see cref="Scale3f"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3f operator *(Rot3f a, Scale3f b)
            => new Affine3f((M33f)a * (M33f)b);

        #endregion

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Rot3f"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Rot3f r0, Rot3f r1)
            => Rot.Distance(r0, r1) == 0;

        /// <summary>
        /// Checks whether two <see cref="Rot3f"/> transformations are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Rot3f r0, Rot3f r1)
            => !(r0 == r1);

        #endregion

        #region Static Creators
        
        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from an orthonormal basis.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f FromFrame(V3f x, V3f y, V3f z)
        {
            return FromM33f(M33f.FromCols(x, y, z));
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from a Rodrigues axis-angle vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f FromAngleAxis(V3f angleAxis)
        {
            float theta2 = angleAxis.LengthSquared;
            if (theta2 > Constant<float>.PositiveTinyValue)
            {
                var theta = Fun.Sqrt(theta2);
                var thetaHalf = theta / 2;
                var k = Fun.Sin(thetaHalf) / theta;
                return new Rot3f(Fun.Cos(thetaHalf), k * angleAxis);
            }
            else
                return new Rot3f(1, 0, 0, 0);
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from a rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Rot3f FromM33f(M33f m, float epsilon = (float)1e-6)
        {
            if (!m.IsOrthonormal(epsilon)) throw new ArgumentException("Matrix is not orthonormal.");
            var tr = m.M00 + m.M11 + m.M22;

            if (tr > 0)
            {
                float s = (tr + 1).Sqrt() * 2;
                float x = (m.M21 - m.M12) / s;
                float y = (m.M02 - m.M20) / s;
                float z = (m.M10 - m.M01) / s;
                float w = s / 4;
                return new Rot3f(new QuaternionF(w, x, y, z).Normalized);
            }
            else if (m.M00 > m.M11 && m.M00 > m.M22)
            {
                float s = Fun.Sqrt(1 + m.M00 - m.M11 - m.M22) * 2;
                float x = s / 4;
                float y = (m.M01 + m.M10) / s;
                float z = (m.M02 + m.M20) / s;
                float w = (m.M21 - m.M12) / s;
                return new Rot3f(new QuaternionF(w, x, y, z).Normalized);
            }
            else if (m.M11 > m.M22)
            {
                float s = Fun.Sqrt(1 + m.M11 - m.M00 - m.M22) * 2;
                float x = (m.M01 + m.M10) / s;
                float y = s / 4;
                float z = (m.M12 + m.M21) / s;
                float w = (m.M02 - m.M20) / s;
                return new Rot3f(new QuaternionF(w, x, y, z).Normalized);
            }
            else
            {
                float s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                float x = (m.M02 + m.M20) / s;
                float y = (m.M12 + m.M21) / s;
                float z = s / 4;
                float w = (m.M10 - m.M01) / s;
                return new Rot3f(new QuaternionF(w, x, y, z).Normalized);
            }
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from a <see cref="M44f"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f FromM44f(M44f m, float epsilon = 1e-5f)
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");

            if (!m.C3.XYZ.ApproximateEquals(V3f.Zero, epsilon))
                throw new ArgumentException("Matrix contains translational component.");

            if (m.M33.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return FromM33f(((M33f)m) / m.M33);
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from a <see cref="Euclidean3f"/>.
        /// The transformation <paramref name="euclidean"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f FromEuclidean3f(Euclidean3f euclidean, float epsilon = 1e-5f)
        {
            if (!euclidean.Trans.ApproximateEquals(V3f.Zero, epsilon))
                throw new ArgumentException("Euclidean transformation contains translational component");

            return euclidean.Rot;
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from a <see cref="Similarity3f"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f FromSimilarity3f(Similarity3f similarity, float epsilon = 1e-5f)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            if (!similarity.Trans.ApproximateEquals(V3f.Zero, epsilon))
                throw new ArgumentException("Similarity transformation contains translational component");

            return similarity.Rot;
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from an <see cref="Affine3f"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f FromAffine3f(Affine3f affine, float epsilon = 1e-5f)
            => FromM44f((M44f)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation from a <see cref="Trafo3f"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f FromTrafo3f(Trafo3f trafo, float epsilon = 1e-5f)
            => FromM44f(trafo.Forward, epsilon);

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation representing a rotation around 
        /// an axis by an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f Rotation(V3f normalizedAxis, float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            var halfAngleSin = halfAngle.Sin();

            return new Rot3f(halfAngle.Cos(), normalizedAxis * halfAngleSin);
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation representing a rotation around 
        /// an axis by an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationInDegrees(V3f normalizedAxis, float angleInDegrees)
            => Rotation(normalizedAxis, angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation representing a rotation from one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotateInto(V3f from, V3f into)
        {
            var d = Vec.Dot(from, into);

            if (d.ApproximateEquals(-1))
                return new Rot3f(0, from.AxisAlignedNormal());
            else
            {
                QuaternionF q = new QuaternionF(d + 1, Vec.Cross(from, into));
                return new Rot3f(q.Normalized);
            }
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation by <paramref name="angleInRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationX(float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3f(halfAngle.Cos(), new V3f(halfAngle.Sin(), 0, 0));
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation by <paramref name="angleInDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationXInDegrees(float angleInDegrees)
            => RotationX(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation by <paramref name="angleInRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationY(float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3f(halfAngle.Cos(), new V3f(0, halfAngle.Sin(), 0));
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation by <paramref name="angleInDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationYInDegrees(float angleInDegrees)
            => RotationY(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation by <paramref name="angleInRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationZ(float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3f(halfAngle.Cos(), new V3f(0, 0, halfAngle.Sin()));
        }

        /// <summary>
        /// Creates a <see cref="Rot3f"/> transformation by <paramref name="angleInDegrees"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationZInDegrees(float angleInDegrees)
            => RotationZ(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationEuler(float rollInRadians, float pitchInRadians, float yawInRadians)
        {
            float rollHalf = rollInRadians / 2;
            float cr = Fun.Cos(rollHalf);
            float sr = Fun.Sin(rollHalf);
            float pitchHalf = pitchInRadians / 2;
            float cp = Fun.Cos(pitchHalf);
            float sp = Fun.Sin(pitchHalf);
            float yawHalf = yawInRadians / 2;
            float cy = Fun.Cos(yawHalf);
            float sy = Fun.Sin(yawHalf);

            return new Rot3f(
                cy * cp * cr + sy * sp * sr,
                cy * cp * sr - sy * sp * cr,
                sy * cp * sr + cy * sp * cr,
                sy * cp * cr - cy * sp * sr);
        }

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationEulerInDegrees(float rollInDegrees, float pitchInDegrees, float yawInDegrees)
            => RotationEuler(
                rollInDegrees.RadiansFromDegrees(),
                pitchInDegrees.RadiansFromDegrees(),
                yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationEuler(V3f rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f RotationEulerInDegrees(V3f rollPitchYawInDegrees)
            => RotationEulerInDegrees(rollPitchYawInDegrees.X, rollPitchYawInDegrees.Y, rollPitchYawInDegrees.Z);

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33f(Rot3f r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            float xx = r.X * r.X;
            float yy = r.Y * r.Y;
            float zz = r.Z * r.Z;
            float xy = r.X * r.Y;
            float xz = r.X * r.Z;
            float yz = r.Y * r.Z;
            float xw = r.X * r.W;
            float yw = r.Y * r.W;
            float zw = r.Z * r.W;
            return new M33f(
                1 - 2 * (yy + zz),
                2 * (xy - zw),
                2 * (xz + yw),
                
                2 * (xy + zw),
                1 - 2 * (zz + xx),
                2 * (yz - xw),
                
                2 * (xz - yw),
                2 * (yz + xw),
                1 - 2 * (yy + xx));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M34f(Rot3f r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            float xx = r.X * r.X;
            float yy = r.Y * r.Y;
            float zz = r.Z * r.Z;
            float xy = r.X * r.Y;
            float xz = r.X * r.Z;
            float yz = r.Y * r.Z;
            float xw = r.X * r.W;
            float yw = r.Y * r.W;
            float zw = r.Z * r.W;
            return new M34f(
                1 - 2 * (yy + zz),
                2 * (xy - zw),
                2 * (xz + yw),
                0,
                
                2 * (xy + zw),
                1 - 2 * (zz + xx),
                2 * (yz - xw),
                0,
                
                2 * (xz - yw),
                2 * (yz + xw),
                1 - 2 * (yy + xx),
                0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M44f(Rot3f r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            float xx = r.X * r.X;
            float yy = r.Y * r.Y;
            float zz = r.Z * r.Z;
            float xy = r.X * r.Y;
            float xz = r.X * r.Z;
            float yz = r.Y * r.Z;
            float xw = r.X * r.W;
            float yw = r.Y * r.W;
            float zw = r.Z * r.W;
            return new M44f(
                1 - 2 * (yy + zz),
                2 * (xy - zw),
                2 * (xz + yw),
                0,
                
                2 * (xy + zw),
                1 - 2 * (zz + xx),
                2 * (yz - xw),
                0,
                
                2 * (xz - yw),
                2 * (yz + xw),
                1 - 2 * (yy + xx),
                0,

                0, 0, 0, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float[](Rot3f r)
        {
            float[] array = new float[4];
            array[0] = r.W;
            array[1] = r.X;
            array[2] = r.Y;
            array[3] = r.Z;
            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Euclidean3f(Rot3f r)
            => new Euclidean3f(r, V3f.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity3f(Rot3f r)
            => new Similarity3f(1, r, V3f.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine3f(Rot3f r)
            => new Affine3f((M33f)r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3f(Rot3f r)
            => new Trafo3f((M44f)r, (M44f)r.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rot3d(Rot3f r)
            => new Rot3d((double)r.W, (double)r.X, (double)r.Y, (double)r.Z);

        #endregion

        #region Indexing

        /// <summary>
        /// Gets or sets the <paramref name="i"/>-th component of the <see cref="Rot3f"/> unit quaternion with components (W, (X, Y, Z)).
        /// </summary>
        public unsafe float this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (float* ptr = &W) { return ptr[i]; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (float* ptr = &W) { ptr[i] = value; }
            }
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(W, V);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Rot3f other)
            => Rot.Distance(this, other) == 0;

        public override bool Equals(object other)
            => (other is Rot3f o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", W, V);
        }

        public static Rot3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Rot3f(float.Parse(x[0], CultureInfo.InvariantCulture), V3f.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Rot
    {
        #region Dot

        /// <summary> 
        /// Returns the dot product of two <see cref="Rot3f"/> unit quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(this Rot3f a, Rot3f b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        #endregion

        #region Distance

        /// <summary>
        /// Returns the absolute difference in radians between two <see cref="Rot3f"/> rotations.
        /// The result is within the range of [0, Pi].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceFast(this Rot3f r1, Rot3f r2)
        {
            var d = Dot(r1, r2);
            return 2 * Fun.AcosClamped((d < 0) ? -d : d);
        }

        /// <summary>
        /// Returns the absolute difference in radians between two <see cref="Rot3f"/> rotations
        /// using a numerically stable algorithm.
        /// The result is within the range of [0, Pi].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(this Rot3f r1, Rot3f r2)
        {
            var q = r1.Inverse * r2;
            return 2 * Fun.Atan2(q.V.Length, (q.W < 0) ? -q.W : q.W);
        }

        #endregion

        #region Invert, Normalize

        /// <summary>
        /// Returns the inverse of a <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f Inverse(Rot3f r)
            => r.Inverse;

        /// <summary>
        /// Inverts the given <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Rot3f r)
        {
            r.X = -r.X;
            r.Y = -r.Y;
            r.Z = -r.Z;
        }

        /// <summary>
        /// Returns a normalized copy of a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f Normalized(Rot3f r)
            => r.Normalized;

        /// <summary>
        /// Normalizes a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Rot3f r)
        {
            var norm = r.Norm;
            if (norm > 0)
            {
                var scale = 1 / norm;
                
                r.W *= scale;
                r.X *= scale;
                r.Y *= scale;
                r.Z *= scale;
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the Rodrigues angle-axis vector of a <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f ToAngleAxis(this Rot3f r)
        {
            var sinTheta2 = r.V.LengthSquared;
            if (sinTheta2 > Constant<float>.PositiveTinyValue)
            {
                float sinTheta = Fun.Sqrt(sinTheta2);
                float cosTheta = r.W;
                float twoTheta = 2 * (cosTheta < 0 ? Fun.Atan2(-sinTheta, -cosTheta)
                                                    : Fun.Atan2(sinTheta, cosTheta));
                return r.V * (twoTheta / sinTheta);
            }
            else
                return V3f.Zero;
        }

        /// <summary>
        /// Returns the axis-angle representation of a <see cref="Rot3f"/> transformation.
        /// </summary>
        /// <param name="r">A <see cref="Rot3f"/> transformation.</param>
        /// <param name="axis">Output of normalized axis of rotation.</param>
        /// <param name="angleInRadians">Output of angle of rotation in radians about axis (Right Hand Rule).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToAxisAngle(this Rot3f r, ref V3f axis, ref float angleInRadians)
        {
            angleInRadians = 2 * Fun.Acos(r.W);
            var s = Fun.Sqrt(1 - r.W * r.W); // assuming quaternion normalised then w is less than 1, so term always positive.
            if (s < 0.001)
            { // test to avoid divide by zero, s is always positive due to sqrt
                // if s close to zero then direction of axis not important
                axis.X = r.X; // if it is important that axis is normalised then replace with x=1; y=z=0;
                axis.Y = r.Y;
                axis.Z = r.Z;
            }
            else
            {
                axis.X = r.X / s; // normalise axis
                axis.Y = r.Y / s;
                axis.Z = r.Z / s;
            }
        }

        #endregion

        #region Euler Angles

        /// <summary>
        /// Returns the Euler-Angles from the given <see cref="Rot3f"/> as a <see cref="V3f"/> vector.
        /// The vector components represent [roll (X), pitch (Y), yaw (Z)] with rotation order is Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetEulerAngles(this Rot3f r)
        {
            var test = r.W * r.Y - r.X * r.Z;
            if (test > 0.5f - Constant<float>.PositiveTinyValue) // singularity at north pole
            {
                return new V3f(
                    2 * Fun.Atan2(r.X, r.W),
                    (float)Constant.PiHalf,
                    0);
            }
            if (test < -0.5f + Constant<float>.PositiveTinyValue) // singularity at south pole
            {
                return new V3f(
                    2 * Fun.Atan2(r.X, r.W),
                    -(float)Constant.PiHalf,
                    0);
            }
            // From Wikipedia, conversion between quaternions and Euler angles.
            return new V3f(
                        Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                  1 - 2 * (r.X * r.X + r.Y * r.Y)),
                        Fun.AsinClamped(2 * test),
                        Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                  1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="V3f"/> vector by a <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f Transform(this Rot3f r, V3f v)
            => r * v;

        /// <summary>
        /// Transforms a <see cref="V3f"/> vector by the inverse of a <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransform(this Rot3f r, V3f v)
        {
            var w = r.X * v.X + r.Y * v.Y + r.Z * v.Z;
            var x = r.W * v.X - r.Y * v.Z + r.Z * v.Y;
            var y = r.W * v.Y - r.Z * v.X + r.X * v.Z;
            var z = r.W * v.Z - r.X * v.Y + r.Y * v.X;

            return new V3f(
                w * r.X + x * r.W + y * r.Z - z * r.Y,
                w * r.Y + y * r.W + z * r.X - x * r.Z,
                w * r.Z + z * r.W + x * r.Y - y * r.X
                );
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot3f r0, Rot3f r1)
        {
            return ApproximateEquals(r0, r1, Constant<float>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot3f r0, Rot3f r1, float tolerance)
        {
            return Rot.Distance(r0, r1) <= tolerance;
        }

        #endregion
    }

    #endregion

    #region Rot3d

    /// <summary>
    /// Represents a rotation in three dimensions using a unit quaternion.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rot3d
    {
        /// <summary>
        /// Scalar (real) part of the quaternion.
        /// </summary>
        [DataMember]
        public double W;

        /// <summary>
        /// First component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public double X;

        /// <summary>
        /// Second component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public double Y;

        /// <summary>
        /// Third component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public double Z;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="Rot3d"/> transformation from the quaternion (w, (x, y, z)).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3d(double w, double x, double y, double z)
        {
            W = w;
            X = x; Y = y; Z = z;
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-10));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3d"/> transformation from the quaternion (w, (v.x, v.y, v.z)).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3d(double w, V3d v)
        {
            W = w;
            X = v.X; Y = v.Y; Z = v.Z;
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-10));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3d"/> transformation from the quaternion <paramref name="q"/>.
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3d(QuaternionD q)
        {
            W = q.W; X = q.X; Y = q.Y; Z = q.Z; 
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-10));
        }

        /// <summary>
        /// Constructs a copy of a <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3d(Rot3d r)
        {
            W = r.W; X = r.X; Y = r.Y; Z = r.Z; 
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-10));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3d"/> transformation from the quaternion (a[0], (a[1], a[2], a[3])).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3d(double[] a)
        {
            W = a[0];
            X = a[1]; Y = a[2]; Z = a[3];
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-10));
        }

        /// <summary>
        /// Constructs a <see cref="Rot3d"/> transformation from the quaternion (a[start], (a[start + 1], a[start + 2], a[start + 3])).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rot3d(double[] a, int start)
        {
            W = a[start];
            X = a[start + 1]; Y = a[start + 2]; Z = a[start + 3];
            Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-10));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the vector part (x, y, z) of this <see cref="Rot3d"/> unit quaternion.
        /// </summary>
        [XmlIgnore]
        public V3d V
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new V3d(X, Y, Z); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        /// <summary>
        /// Gets the squared norm (or squared length) of this <see cref="Rot3d"/>.
        /// May not be exactly 1, due to numerical inaccuracy.
        /// </summary>
        public double NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => W * W + X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Gets the norm (or length) of this <see cref="Rot3d"/>.
        /// May not be exactly 1, due to numerical inaccuracy. 
        /// </summary>
        public double Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NormSquared.Sqrt();
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this <see cref="Rot3d"/>
        /// </summary>
        public Rot3d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var rs = new Rot3d(this);
                rs.Normalize();
                return rs;
            }
        }

        /// <summary>
        /// Gets the inverse of this <see cref="Rot3d"/> transformation.
        /// </summary>
        public Rot3d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, 1e-10));
                return new Rot3d(W, -X, -Y, -Z);
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity <see cref="Rot3d"/>.
        /// </summary>
        public static Rot3d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Rot3d(1, 0, 0, 0);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Returns the component-wise negation of a <see cref="Rot3d"/> unit quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator -(Rot3d q)
            => new Rot3d(-q.W, -q.X, -q.Y, -q.Z);

        /// <summary>
        /// Multiplies two <see cref="Rot3d"/> transformations.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator *(Rot3d a, Rot3d b)
        {
            return new Rot3d(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
        }

        #region Rot / Vector Multiplication

        /// <summary>
        /// Transforms a <see cref="V3d"/> vector by a <see cref="Rot3d"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d operator *(Rot3d r, V3d v)
        {
            var w = -r.X * v.X - r.Y * v.Y - r.Z * v.Z;
            var x = r.W * v.X + r.Y * v.Z - r.Z * v.Y;
            var y = r.W * v.Y + r.Z * v.X - r.X * v.Z;
            var z = r.W * v.Z + r.X * v.Y - r.Y * v.X;

            return new V3d(
                -w * r.X + x * r.W - y * r.Z + z * r.Y,
                -w * r.Y + y * r.W - z * r.X + x * r.Z,
                -w * r.Z + z * r.W - x * r.Y + y * r.X);
        }

        #endregion

        #region Rot / Matrix Multiplication

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> transformation (as a 3x3 matrix) with a <see cref="M33d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Rot3d rot, M33d m)
        {
            return (M33d)rot * m;
        }

        /// <summary>
        /// Multiplies a <see cref="M33d"/> with a <see cref="Rot3d"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(M33d m, Rot3d rot)
        {
            return m * (M33d)rot;
        }

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> transformation (as a 4x4 matrix) with a <see cref="M44d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d operator *(Rot3d rot, M44d m)
        {
            var r = (M33d)rot;
            return new M44d(
                r.M00 * m.M00 + r.M01 * m.M10 + r.M02 * m.M20, 
                r.M00 * m.M01 + r.M01 * m.M11 + r.M02 * m.M21, 
                r.M00 * m.M02 + r.M01 * m.M12 + r.M02 * m.M22, 
                r.M00 * m.M03 + r.M01 * m.M13 + r.M02 * m.M23,

                r.M10 * m.M00 + r.M11 * m.M10 + r.M12 * m.M20, 
                r.M10 * m.M01 + r.M11 * m.M11 + r.M12 * m.M21, 
                r.M10 * m.M02 + r.M11 * m.M12 + r.M12 * m.M22, 
                r.M10 * m.M03 + r.M11 * m.M13 + r.M12 * m.M23,

                r.M20 * m.M00 + r.M21 * m.M10 + r.M22 * m.M20, 
                r.M20 * m.M01 + r.M21 * m.M11 + r.M22 * m.M21, 
                r.M20 * m.M02 + r.M21 * m.M12 + r.M22 * m.M22, 
                r.M20 * m.M03 + r.M21 * m.M13 + r.M22 * m.M23,

                m.M30, m.M31, m.M32, m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="M44d"/> with a <see cref="Rot3d"/> transformation (as a 4x4 matrix) .
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d operator *(M44d m, Rot3d rot)
        {
            var r = (M33d)rot;
            return new M44d(
                m.M00 * r.M00 + m.M01 * r.M10 + m.M02 * r.M20, 
                m.M00 * r.M01 + m.M01 * r.M11 + m.M02 * r.M21, 
                m.M00 * r.M02 + m.M01 * r.M12 + m.M02 * r.M22,
                m.M03,

                m.M10 * r.M00 + m.M11 * r.M10 + m.M12 * r.M20, 
                m.M10 * r.M01 + m.M11 * r.M11 + m.M12 * r.M21, 
                m.M10 * r.M02 + m.M11 * r.M12 + m.M12 * r.M22,
                m.M13,

                m.M20 * r.M00 + m.M21 * r.M10 + m.M22 * r.M20, 
                m.M20 * r.M01 + m.M21 * r.M11 + m.M22 * r.M21, 
                m.M20 * r.M02 + m.M21 * r.M12 + m.M22 * r.M22,
                m.M23,

                m.M30 * r.M00 + m.M31 * r.M10 + m.M32 * r.M20, 
                m.M30 * r.M01 + m.M31 * r.M11 + m.M32 * r.M21, 
                m.M30 * r.M02 + m.M31 * r.M12 + m.M32 * r.M22,
                m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> transformation (as a 3x3 matrix) with a <see cref="M34d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Rot3d rot, M34d m)
        {
            return (M33d)rot * m;
        }

        /// <summary>
        /// Multiplies a <see cref="M34d"/> with a <see cref="Rot3d"/> transformation (as a 4x4 matrix) .
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(M34d m, Rot3d rot)
        {
            var r = (M33d)rot;
            return new M34d(
                m.M00 * r.M00 + m.M01 * r.M10 + m.M02 * r.M20, 
                m.M00 * r.M01 + m.M01 * r.M11 + m.M02 * r.M21, 
                m.M00 * r.M02 + m.M01 * r.M12 + m.M02 * r.M22,
                m.M03,

                m.M10 * r.M00 + m.M11 * r.M10 + m.M12 * r.M20, 
                m.M10 * r.M01 + m.M11 * r.M11 + m.M12 * r.M21, 
                m.M10 * r.M02 + m.M11 * r.M12 + m.M12 * r.M22,
                m.M13,

                m.M20 * r.M00 + m.M21 * r.M10 + m.M22 * r.M20, 
                m.M20 * r.M01 + m.M21 * r.M11 + m.M22 * r.M21, 
                m.M20 * r.M02 + m.M21 * r.M12 + m.M22 * r.M22,
                m.M23);
        }

        #endregion

        #region Rot / Quaternion arithmetics

        /// <summary>
        /// Returns the sum of a <see cref="Rot3d"/> and a <see cref="QuaternionD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator +(Rot3d r, QuaternionD q)
            => new QuaternionD(r.W + q.W, r.X + q.X, r.Y + q.Y, r.Z + q.Z);

        /// <summary>
        /// Returns the sum of a <see cref="QuaternionD"/> and a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator +(QuaternionD q, Rot3d r)
            => new QuaternionD(q.W + r.W, q.X + r.X, q.Y + r.Y, q.Z + r.Z);

        /// <summary>
        /// Returns the sum of a <see cref="Rot3d"/> and a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator +(Rot3d r, double s)
            => new QuaternionD(r.W + s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the sum of a real scalar and a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator +(double s, Rot3d r)
            => new QuaternionD(r.W + s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the difference between a <see cref="Rot3d"/> and a <see cref="QuaternionD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(Rot3d r, QuaternionD q)
            => new QuaternionD(r.W - q.W, r.X - q.X, r.Y - q.Y, r.Z - q.Z);

        /// <summary>
        /// Returns the difference between a <see cref="QuaternionD"/> and a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(QuaternionD q, Rot3d r)
            => new QuaternionD(q.W - r.W, q.X - r.X, q.Y - r.Y, q.Z - r.Z);

        /// <summary>
        /// Returns the difference between a <see cref="Rot3d"/> and a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(Rot3d r, double s)
            => new QuaternionD(r.W - s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the difference between a real scalar and a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(double s, Rot3d r)
            => new QuaternionD(s - r.W, -r.X, -r.Y, -r.Z);

        /// <summary>
        /// Returns the product of a <see cref="Rot3d"/> and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(Rot3d r, double s)
            => new QuaternionD(r.W * s, r.X * s, r.Y * s, r.Z * s);

        /// <summary>
        /// Returns the product of a scalar and a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(double s, Rot3d r)
            => new QuaternionD(r.W * s, r.X * s, r.Y * s, r.Z * s);

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> with a <see cref="QuaternionD"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(Rot3d r, QuaternionD q)
        {
            return new QuaternionD(
                r.W * q.W - r.X * q.X - r.Y * q.Y - r.Z * q.Z,
                r.W * q.X + r.X * q.W + r.Y * q.Z - r.Z * q.Y,
                r.W * q.Y + r.Y * q.W + r.Z * q.X - r.X * q.Z,
                r.W * q.Z + r.Z * q.W + r.X * q.Y - r.Y * q.X);
        }

        /// <summary>
        /// Multiplies a <see cref="QuaternionD"/> with a <see cref="Rot3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(QuaternionD q, Rot3d r)
        {
            return new QuaternionD(
                q.W * r.W - q.X * r.X - q.Y * r.Y - q.Z * r.Z,
                q.W * r.X + q.X * r.W + q.Y * r.Z - q.Z * r.Y,
                q.W * r.Y + q.Y * r.W + q.Z * r.X - q.X * r.Z,
                q.W * r.Z + q.Z * r.W + q.X * r.Y - q.Y * r.X);
        }

        /// <summary>
        /// Divides a <see cref="Rot3d"/> by a <see cref="QuaternionD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator /(Rot3d r, QuaternionD q)
            => r * q.Inverse;

        /// <summary>
        /// Divides a <see cref="QuaternionD"/> by a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator /(QuaternionD q, Rot3d r)
            => q * r.Inverse;

        /// <summary>
        /// Divides a <see cref="Rot3d"/> by a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator /(Rot3d r, double s)
            => new QuaternionD(r.W / s, r.X / s, r.Y / s, r.Z / s);

        /// <summary>
        /// Divides a scalar by a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator /(double s, Rot3d r)
            => new QuaternionD(s / r.W, s / r.X, s / r.Y, s / r.Z);

        #endregion

        #region Rot / Shift, Scale Multiplication

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> transformation with a <see cref="Shift3d"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d operator *(Rot3d a, Shift3d b)
            => new Euclidean3d(a, a * b.V);

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> transformation with a <see cref="Scale3d"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3d operator *(Rot3d a, Scale3d b)
            => new Affine3d((M33d)a * (M33d)b);

        #endregion

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="Rot3d"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Rot3d r0, Rot3d r1)
            => Rot.Distance(r0, r1) == 0;

        /// <summary>
        /// Checks whether two <see cref="Rot3d"/> transformations are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Rot3d r0, Rot3d r1)
            => !(r0 == r1);

        #endregion

        #region Static Creators
        
        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from an orthonormal basis.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d FromFrame(V3d x, V3d y, V3d z)
        {
            return FromM33d(M33d.FromCols(x, y, z));
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from a Rodrigues axis-angle vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d FromAngleAxis(V3d angleAxis)
        {
            double theta2 = angleAxis.LengthSquared;
            if (theta2 > Constant<double>.PositiveTinyValue)
            {
                var theta = Fun.Sqrt(theta2);
                var thetaHalf = theta / 2;
                var k = Fun.Sin(thetaHalf) / theta;
                return new Rot3d(Fun.Cos(thetaHalf), k * angleAxis);
            }
            else
                return new Rot3d(1, 0, 0, 0);
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from a rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Rot3d FromM33d(M33d m, double epsilon = (double)1e-6)
        {
            if (!m.IsOrthonormal(epsilon)) throw new ArgumentException("Matrix is not orthonormal.");
            var tr = m.M00 + m.M11 + m.M22;

            if (tr > 0)
            {
                double s = (tr + 1).Sqrt() * 2;
                double x = (m.M21 - m.M12) / s;
                double y = (m.M02 - m.M20) / s;
                double z = (m.M10 - m.M01) / s;
                double w = s / 4;
                return new Rot3d(new QuaternionD(w, x, y, z).Normalized);
            }
            else if (m.M00 > m.M11 && m.M00 > m.M22)
            {
                double s = Fun.Sqrt(1 + m.M00 - m.M11 - m.M22) * 2;
                double x = s / 4;
                double y = (m.M01 + m.M10) / s;
                double z = (m.M02 + m.M20) / s;
                double w = (m.M21 - m.M12) / s;
                return new Rot3d(new QuaternionD(w, x, y, z).Normalized);
            }
            else if (m.M11 > m.M22)
            {
                double s = Fun.Sqrt(1 + m.M11 - m.M00 - m.M22) * 2;
                double x = (m.M01 + m.M10) / s;
                double y = s / 4;
                double z = (m.M12 + m.M21) / s;
                double w = (m.M02 - m.M20) / s;
                return new Rot3d(new QuaternionD(w, x, y, z).Normalized);
            }
            else
            {
                double s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                double x = (m.M02 + m.M20) / s;
                double y = (m.M12 + m.M21) / s;
                double z = s / 4;
                double w = (m.M10 - m.M01) / s;
                return new Rot3d(new QuaternionD(w, x, y, z).Normalized);
            }
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from a <see cref="M44d"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d FromM44d(M44d m, double epsilon = 1e-12)
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");

            if (!m.C3.XYZ.ApproximateEquals(V3d.Zero, epsilon))
                throw new ArgumentException("Matrix contains translational component.");

            if (m.M33.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return FromM33d(((M33d)m) / m.M33);
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from a <see cref="Euclidean3d"/>.
        /// The transformation <paramref name="euclidean"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d FromEuclidean3d(Euclidean3d euclidean, double epsilon = 1e-12)
        {
            if (!euclidean.Trans.ApproximateEquals(V3d.Zero, epsilon))
                throw new ArgumentException("Euclidean transformation contains translational component");

            return euclidean.Rot;
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from a <see cref="Similarity3d"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d FromSimilarity3d(Similarity3d similarity, double epsilon = 1e-12)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            if (!similarity.Trans.ApproximateEquals(V3d.Zero, epsilon))
                throw new ArgumentException("Similarity transformation contains translational component");

            return similarity.Rot;
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from an <see cref="Affine3d"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d FromAffine3d(Affine3d affine, double epsilon = 1e-12)
            => FromM44d((M44d)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation from a <see cref="Trafo3d"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d FromTrafo3d(Trafo3d trafo, double epsilon = 1e-12)
            => FromM44d(trafo.Forward, epsilon);

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation representing a rotation around 
        /// an axis by an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d Rotation(V3d normalizedAxis, double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            var halfAngleSin = halfAngle.Sin();

            return new Rot3d(halfAngle.Cos(), normalizedAxis * halfAngleSin);
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation representing a rotation around 
        /// an axis by an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationInDegrees(V3d normalizedAxis, double angleInDegrees)
            => Rotation(normalizedAxis, angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation representing a rotation from one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotateInto(V3d from, V3d into)
        {
            var d = Vec.Dot(from, into);

            if (d.ApproximateEquals(-1))
                return new Rot3d(0, from.AxisAlignedNormal());
            else
            {
                QuaternionD q = new QuaternionD(d + 1, Vec.Cross(from, into));
                return new Rot3d(q.Normalized);
            }
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation by <paramref name="angleInRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationX(double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3d(halfAngle.Cos(), new V3d(halfAngle.Sin(), 0, 0));
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation by <paramref name="angleInDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationXInDegrees(double angleInDegrees)
            => RotationX(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation by <paramref name="angleInRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationY(double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3d(halfAngle.Cos(), new V3d(0, halfAngle.Sin(), 0));
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation by <paramref name="angleInDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationYInDegrees(double angleInDegrees)
            => RotationY(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation by <paramref name="angleInRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationZ(double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3d(halfAngle.Cos(), new V3d(0, 0, halfAngle.Sin()));
        }

        /// <summary>
        /// Creates a <see cref="Rot3d"/> transformation by <paramref name="angleInDegrees"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationZInDegrees(double angleInDegrees)
            => RotationZ(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationEuler(double rollInRadians, double pitchInRadians, double yawInRadians)
        {
            double rollHalf = rollInRadians / 2;
            double cr = Fun.Cos(rollHalf);
            double sr = Fun.Sin(rollHalf);
            double pitchHalf = pitchInRadians / 2;
            double cp = Fun.Cos(pitchHalf);
            double sp = Fun.Sin(pitchHalf);
            double yawHalf = yawInRadians / 2;
            double cy = Fun.Cos(yawHalf);
            double sy = Fun.Sin(yawHalf);

            return new Rot3d(
                cy * cp * cr + sy * sp * sr,
                cy * cp * sr - sy * sp * cr,
                sy * cp * sr + cy * sp * cr,
                sy * cp * cr - cy * sp * sr);
        }

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationEulerInDegrees(double rollInDegrees, double pitchInDegrees, double yawInDegrees)
            => RotationEuler(
                rollInDegrees.RadiansFromDegrees(),
                pitchInDegrees.RadiansFromDegrees(),
                yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationEuler(V3d rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d RotationEulerInDegrees(V3d rollPitchYawInDegrees)
            => RotationEulerInDegrees(rollPitchYawInDegrees.X, rollPitchYawInDegrees.Y, rollPitchYawInDegrees.Z);

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33d(Rot3d r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            double xx = r.X * r.X;
            double yy = r.Y * r.Y;
            double zz = r.Z * r.Z;
            double xy = r.X * r.Y;
            double xz = r.X * r.Z;
            double yz = r.Y * r.Z;
            double xw = r.X * r.W;
            double yw = r.Y * r.W;
            double zw = r.Z * r.W;
            return new M33d(
                1 - 2 * (yy + zz),
                2 * (xy - zw),
                2 * (xz + yw),
                
                2 * (xy + zw),
                1 - 2 * (zz + xx),
                2 * (yz - xw),
                
                2 * (xz - yw),
                2 * (yz + xw),
                1 - 2 * (yy + xx));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M34d(Rot3d r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            double xx = r.X * r.X;
            double yy = r.Y * r.Y;
            double zz = r.Z * r.Z;
            double xy = r.X * r.Y;
            double xz = r.X * r.Z;
            double yz = r.Y * r.Z;
            double xw = r.X * r.W;
            double yw = r.Y * r.W;
            double zw = r.Z * r.W;
            return new M34d(
                1 - 2 * (yy + zz),
                2 * (xy - zw),
                2 * (xz + yw),
                0,
                
                2 * (xy + zw),
                1 - 2 * (zz + xx),
                2 * (yz - xw),
                0,
                
                2 * (xz - yw),
                2 * (yz + xw),
                1 - 2 * (yy + xx),
                0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M44d(Rot3d r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            double xx = r.X * r.X;
            double yy = r.Y * r.Y;
            double zz = r.Z * r.Z;
            double xy = r.X * r.Y;
            double xz = r.X * r.Z;
            double yz = r.Y * r.Z;
            double xw = r.X * r.W;
            double yw = r.Y * r.W;
            double zw = r.Z * r.W;
            return new M44d(
                1 - 2 * (yy + zz),
                2 * (xy - zw),
                2 * (xz + yw),
                0,
                
                2 * (xy + zw),
                1 - 2 * (zz + xx),
                2 * (yz - xw),
                0,
                
                2 * (xz - yw),
                2 * (yz + xw),
                1 - 2 * (yy + xx),
                0,

                0, 0, 0, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double[](Rot3d r)
        {
            double[] array = new double[4];
            array[0] = r.W;
            array[1] = r.X;
            array[2] = r.Y;
            array[3] = r.Z;
            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Euclidean3d(Rot3d r)
            => new Euclidean3d(r, V3d.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity3d(Rot3d r)
            => new Similarity3d(1, r, V3d.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine3d(Rot3d r)
            => new Affine3d((M33d)r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3d(Rot3d r)
            => new Trafo3d((M44d)r, (M44d)r.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rot3f(Rot3d r)
            => new Rot3f((float)r.W, (float)r.X, (float)r.Y, (float)r.Z);

        #endregion

        #region Indexing

        /// <summary>
        /// Gets or sets the <paramref name="i"/>-th component of the <see cref="Rot3d"/> unit quaternion with components (W, (X, Y, Z)).
        /// </summary>
        public unsafe double this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (double* ptr = &W) { return ptr[i]; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (double* ptr = &W) { ptr[i] = value; }
            }
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(W, V);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Rot3d other)
            => Rot.Distance(this, other) == 0;

        public override bool Equals(object other)
            => (other is Rot3d o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", W, V);
        }

        public static Rot3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Rot3d(double.Parse(x[0], CultureInfo.InvariantCulture), V3d.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Rot
    {
        #region Dot

        /// <summary> 
        /// Returns the dot product of two <see cref="Rot3d"/> unit quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(this Rot3d a, Rot3d b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        #endregion

        #region Distance

        /// <summary>
        /// Returns the absolute difference in radians between two <see cref="Rot3d"/> rotations.
        /// The result is within the range of [0, Pi].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceFast(this Rot3d r1, Rot3d r2)
        {
            var d = Dot(r1, r2);
            return 2 * Fun.AcosClamped((d < 0) ? -d : d);
        }

        /// <summary>
        /// Returns the absolute difference in radians between two <see cref="Rot3d"/> rotations
        /// using a numerically stable algorithm.
        /// The result is within the range of [0, Pi].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(this Rot3d r1, Rot3d r2)
        {
            var q = r1.Inverse * r2;
            return 2 * Fun.Atan2(q.V.Length, (q.W < 0) ? -q.W : q.W);
        }

        #endregion

        #region Invert, Normalize

        /// <summary>
        /// Returns the inverse of a <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d Inverse(Rot3d r)
            => r.Inverse;

        /// <summary>
        /// Inverts the given <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Rot3d r)
        {
            r.X = -r.X;
            r.Y = -r.Y;
            r.Z = -r.Z;
        }

        /// <summary>
        /// Returns a normalized copy of a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d Normalized(Rot3d r)
            => r.Normalized;

        /// <summary>
        /// Normalizes a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Rot3d r)
        {
            var norm = r.Norm;
            if (norm > 0)
            {
                var scale = 1 / norm;
                
                r.W *= scale;
                r.X *= scale;
                r.Y *= scale;
                r.Z *= scale;
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the Rodrigues angle-axis vector of a <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d ToAngleAxis(this Rot3d r)
        {
            var sinTheta2 = r.V.LengthSquared;
            if (sinTheta2 > Constant<double>.PositiveTinyValue)
            {
                double sinTheta = Fun.Sqrt(sinTheta2);
                double cosTheta = r.W;
                double twoTheta = 2 * (cosTheta < 0 ? Fun.Atan2(-sinTheta, -cosTheta)
                                                    : Fun.Atan2(sinTheta, cosTheta));
                return r.V * (twoTheta / sinTheta);
            }
            else
                return V3d.Zero;
        }

        /// <summary>
        /// Returns the axis-angle representation of a <see cref="Rot3d"/> transformation.
        /// </summary>
        /// <param name="r">A <see cref="Rot3d"/> transformation.</param>
        /// <param name="axis">Output of normalized axis of rotation.</param>
        /// <param name="angleInRadians">Output of angle of rotation in radians about axis (Right Hand Rule).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToAxisAngle(this Rot3d r, ref V3d axis, ref double angleInRadians)
        {
            angleInRadians = 2 * Fun.Acos(r.W);
            var s = Fun.Sqrt(1 - r.W * r.W); // assuming quaternion normalised then w is less than 1, so term always positive.
            if (s < 0.001)
            { // test to avoid divide by zero, s is always positive due to sqrt
                // if s close to zero then direction of axis not important
                axis.X = r.X; // if it is important that axis is normalised then replace with x=1; y=z=0;
                axis.Y = r.Y;
                axis.Z = r.Z;
            }
            else
            {
                axis.X = r.X / s; // normalise axis
                axis.Y = r.Y / s;
                axis.Z = r.Z / s;
            }
        }

        #endregion

        #region Euler Angles

        /// <summary>
        /// Returns the Euler-Angles from the given <see cref="Rot3d"/> as a <see cref="V3d"/> vector.
        /// The vector components represent [roll (X), pitch (Y), yaw (Z)] with rotation order is Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetEulerAngles(this Rot3d r)
        {
            var test = r.W * r.Y - r.X * r.Z;
            if (test > 0.5 - Constant<double>.PositiveTinyValue) // singularity at north pole
            {
                return new V3d(
                    2 * Fun.Atan2(r.X, r.W),
                    Constant.PiHalf,
                    0);
            }
            if (test < -0.5 + Constant<double>.PositiveTinyValue) // singularity at south pole
            {
                return new V3d(
                    2 * Fun.Atan2(r.X, r.W),
                    -Constant.PiHalf,
                    0);
            }
            // From Wikipedia, conversion between quaternions and Euler angles.
            return new V3d(
                        Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                  1 - 2 * (r.X * r.X + r.Y * r.Y)),
                        Fun.AsinClamped(2 * test),
                        Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                  1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="V3d"/> vector by a <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d Transform(this Rot3d r, V3d v)
            => r * v;

        /// <summary>
        /// Transforms a <see cref="V3d"/> vector by the inverse of a <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransform(this Rot3d r, V3d v)
        {
            var w = r.X * v.X + r.Y * v.Y + r.Z * v.Z;
            var x = r.W * v.X - r.Y * v.Z + r.Z * v.Y;
            var y = r.W * v.Y - r.Z * v.X + r.X * v.Z;
            var z = r.W * v.Z - r.X * v.Y + r.Y * v.X;

            return new V3d(
                w * r.X + x * r.W + y * r.Z - z * r.Y,
                w * r.Y + y * r.W + z * r.X - x * r.Z,
                w * r.Z + z * r.W + x * r.Y - y * r.X
                );
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot3d r0, Rot3d r1)
        {
            return ApproximateEquals(r0, r1, Constant<double>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot3d r0, Rot3d r1, double tolerance)
        {
            return Rot.Distance(r0, r1) <= tolerance;
        }

        #endregion
    }

    #endregion

}

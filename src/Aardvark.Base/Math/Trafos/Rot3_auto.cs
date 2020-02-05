using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    #region Rot3f

    /// <summary>
    /// Type for general quaternions, if normalized it represents an arbritrary rotation in three dimensions.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rot3f
    {
        [DataMember]
        public float W;
        [DataMember]
        public float X;
        [DataMember]
        public float Y;
        [DataMember]
        public float Z;

        #region Constructors

        /// <summary>
        /// Creates quaternion (w, (x, y, z)).
        /// </summary>
        public Rot3f(float w, float x, float y, float z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates quaternion (w, (v.x, v.y, v.z)).
        /// </summary>
        public Rot3f(float w, V3f v)
        {
            W = w;
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Creates quaternion from array.
        /// (w = a[0], (x = a[1], y = a[2], z = a[3])).
        /// </summary>
        public Rot3f(float[] a)
        {
            W = a[0];
            X = a[1];
            Y = a[2];
            Z = a[3];
        }

        /// <summary>
        /// Creates quaternion from array starting at specified index.
        /// (w = a[start], (x = a[start+1], y = a[start+2], z = a[start+3])).
        /// </summary>
        public Rot3f(float[] a, int start)
        {
            W = a[start];
            X = a[start + 1];
            Y = a[start + 2];
            Z = a[start + 3];
        }

        /// <summary>
        /// Creates quaternion representing a rotation around an axis by an angle.
        /// The axis vector has to be normalized.
        /// </summary>
        public Rot3f(V3f normalizedAxis, float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            W = halfAngle.Cos();
            var halfAngleSin = halfAngle.Sin();
            X = normalizedAxis.X * halfAngleSin;
            Y = normalizedAxis.Y * halfAngleSin;
            Z = normalizedAxis.Z * halfAngleSin;
        }

        /// <summary>
        /// Creates quaternion from euler angles [roll, pitch, yaw].
        /// The rotation order is yaw (Z), pitch (Y), roll (X).
        /// </summary>
        /// <param name="rollInRadians">Rotation around X</param>
        /// <param name="pitchInRadians">Rotation around Y</param>
        /// <param name="yawInRadians">Rotation around Z</param>
        public Rot3f(float rollInRadians, float pitchInRadians, float yawInRadians)
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
            W = cy * cp * cr + sy * sp * sr;
            X = cy * cp * sr - sy * sp * cr;
            Y = sy * cp * sr + cy * sp * cr;
            Z = sy * cp * cr - cy * sp * sr;
        }

        /// <summary>
        /// Creates a quaternion representing a rotation from one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        public Rot3f(V3f from, V3f into)
        {
            var angle = from.AngleBetween(into);

            // some vectors do not normalize to 1.0 -> Vec.Dot = -0.99999999999999989 || -0.99999994f
            // acos => 3.1415926386886319 or 3.14124632f -> delta of 1e-7 or 1e-3 -> using AngleBetween allows higher precision again
            if (angle < 1e-6f)
            {
                // axis = a; angle = 0;
                W = 1;
                X = 0;
                Y = 0;
                Z = 0;
            }
            else if (Constant.PiF - angle.Abs() < 1e-6f)
            {
                //axis = a.AxisAlignedNormal(); //angle = PI;
                this = new Rot3f(0, from.AxisAlignedNormal());
            }
            else
            {
                V3f axis = Vec.Cross(from, into).Normalized;
                this = new Rot3f(axis, angle);
            }
        }

        #endregion

        #region Properties

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new ArgumentException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new ArgumentException();
                }
            }
        }

        public V3f V
        {
            get { return new V3f(X, Y, Z); }
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        #endregion

        #region Constants
        /// <summary>
        /// Zero (0, (0,0,0))
        /// </summary>
        public static Rot3f Zero => new Rot3f(0, V3f.Zero);

        /// <summary>
        /// Identity (1, (0,0,0)).
        /// </summary>
        public static Rot3f Identity => new Rot3f(1, 0, 0, 0);

        /// <summary>
        /// X-Axis (0, (1,0,0)).
        /// </summary>
        public static Rot3f XAxis => new Rot3f(0, 1, 0, 0);

        /// <summary>
        /// Y-Axis (0, (0,1,0)).
        /// </summary>
        public static Rot3f YAxis => new Rot3f(0, 0, 1, 0);

        /// <summary>
        /// Z-Axis (0, (0,0,1)).
        /// </summary>
        public static Rot3f ZAxis => new Rot3f(0, 0, 0, 1);

        #endregion

        #region Quaternion Arithmetics

        /// <summary>
        /// Gets squared norm (or squared length) of this quaternion.
        /// </summary>
        public float NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => W * W + V.LengthSquared;
        }

        /// <summary>
        /// Gets norm (or length) of this quaternion.
        /// </summary>
        public float Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NormSquared.Sqrt();
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this quaternion.
        /// </summary>
        public Rot3f Normalized
        {
            get
            {
                var norm = Norm;
                if (norm == 0) return Rot3f.Zero;
                var scale = 1 / norm;
                return new Rot3f(W * scale, V * scale);
            }
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this quaternion.
        /// </summary>
        public Rot3f Inverse
        {
            get
            {
                var norm = NormSquared;
                if (norm == 0) return Rot3f.Zero;
                var scale = 1 / norm;
                return new Rot3f(W * scale, V * (-scale));
            }
        }

        /// <summary>
        /// Gets the conjugate of this quaternion.
        /// For normalized rotation-quaternions this is the same as Inverted().
        /// </summary>
        public Rot3f Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Rot3f(W, -V);
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/W, 1/X, 1/Y, 1/Z).
        /// </summary>
        public Rot3f Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Rot3f(1 / W, 1 / X, 1 / Y, 1 / Z);
        }

        /// <summary>
        /// Returns the Euler-Angles from the quatarnion as vector.
        /// The vector components represent [roll (X), pitch (Y), yaw (Z)] with rotation order is Z, Y, X.
        /// </summary>
        public V3f GetEulerAngles()
        {
            var test = W * Y - X * Z;
            if (test > 0.49999f) // singularity at north pole
            {
                return new V3f(
                    2 * Fun.Atan2(X, W),
                    (float)Constant.PiHalf,
                    0);
            }
            if (test < -0.49999f) // singularity at south pole
            {
                return new V3f(
                    2 * Fun.Atan2(X, W),
                    -(float)Constant.PiHalf,
                    0);
            }
            // From Wikipedia, conversion between quaternions and Euler angles.
            return new V3f(
                        Fun.Atan2(2 * (W * X + Y * Z),
                                  1 - 2 * (X * X + Y * Y)),
                        Fun.AsinClamped(2 * test),
                        Fun.Atan2(2 * (W * Z + X * Y),
                                  1 - 2 * (Y * Y + Z * Z)));
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Returns the component-wise negation (-q.w, -q.v) of quaternion q.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator -(Rot3f q)
        {
            return new Rot3f(-q.W, -q.X, -q.Y, -q.Z);
        }

        /// <summary>
        /// Returns the sum of two quaternions (a.w + b.w, a.v + b.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator +(Rot3f a, Rot3f b)
        {
            return new Rot3f(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator +(Rot3f q, float s)
        {
            return new Rot3f(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator +(float s, Rot3f q)
        {
            return new Rot3f(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (a.w - b.w, a.v - b.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator -(Rot3f a, Rot3f b)
        {
            return new Rot3f(a.W - b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Returns (q.w - s, (q.x - s, q.y - s, q.z - s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator -(Rot3f q, float s)
        {
            return new Rot3f(q.W - s, q.X - s, q.Y - s, q.Z - s);
        }

        /// <summary>
        /// Returns (s - q.w, (s - q.x, s- q.y, s- q.z)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator -(float s, Rot3f q)
        {
            return new Rot3f(s - q.W, s - q.X, s - q.Y, s - q.Z);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator *(Rot3f q, float s)
        {
            return new Rot3f(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator *(float s, Rot3f q)
        {
            return new Rot3f(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator *(Rot3f a, Rot3f b)
        {
            return new Rot3f(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f operator *(Rot3f q, V3f v)
        {
            return q.Transform(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f operator *(V3f v, Rot3f q)
        {
            return q.InvTransform(v);
        }

#if false //// [todo ISSUE 20090421 andi : andi] check if these are really necessary and comment them what they really do.
        /// <summary>
        /// </summary>
        public static V3f operator *(Rot3f rot, V2f v)
        {
            return (M33f)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V3f operator *(Rot3f rot, V3f v)
        {
            return (M33f)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V4f operator *(Rot3f rot, V4f v)
        {
            return (M33f)rot * v;
        }

        /// <summary>
        /// </summary>
        public static M33f operator *(Rot3f r3, Rot2f r2)
        {
            M33f m33 = (M33f)r3;
            M22f m22 = (M22f)r2;
            return new M33f(
                m33.M00 * m22.M00 + m33.M01 * m22.M10,
                m33.M00 * m22.M01 + m33.M01 * m22.M11,
                m33.M02,

                m33.M10 * m22.M00 + m33.M11 * m22.M10,
                m33.M10 * m22.M01 + m33.M11 * m22.M11,
                m33.M12,

                m33.M20 * m22.M00 + m33.M21 * m22.M10,
                m33.M20 * m22.M01 + m33.M21 * m22.M11,
                m33.M22
                );

        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Rot3f rot, Scale3f m)
        {
            return (M33f)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Rot3f rot, Shift3f m)
        {
            return (M33f)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Rot3f rot, M33f m)
        {
            return (M33f)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(M33f m, Rot3f rot)
        {
            return m * (M33f)rot;
        }

        /// <summary>
        /// Divides two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator /(Rot3f a, Rot3f b)
        {
            return a * b.Reciprocal;
        }

        /// <summary>
        /// Returns (q.w / s, q.v / s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator /(Rot3f q, float s)
        {
            return new Rot3f(q.W / s, q.X / s, q.Y / s, q.Z / s);
        }

        /// <summary>
        /// Returns (s / q.w, s / q.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f operator /(float s, Rot3f q)
        {
            return new Rot3f(s / q.W, s / q.X, s / q.Y, s / q.Z);
        }

        #endregion

        #region Comparison Operators

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        public static bool operator ==(Rot3f r0, Rot3f r1)
        {
            return r0.W == r1.W && r0.X == r1.X && r0.Y == r1.Y && r0.Z == r1.Z;
        }

        public static bool operator !=(Rot3f r0, Rot3f r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Creator Functions

        /// <summary>
        /// WARNING: UNTESTED!!!
        /// </summary>
        public static Rot3f FromFrame(V3f x, V3f y, V3f z)
        {
            return FromM33f(M33f.FromCols(x, y, z));
        }

        /// <summary>
        /// Create from Rodrigues axis-angle vactor
        /// </summary>
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
        /// Creates a quaternion from a rotation matrix
        /// </summary>
        /// <param name="m"></param>
        /// <param name="epsilon"></param>
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
                return new Rot3f(w, x, y, z).Normalized;
            }
            else if (m.M00 > m.M11 && m.M00 > m.M22)
            {
                float s = Fun.Sqrt(1 + m.M00 - m.M11 - m.M22) * 2;
                float x = s / 4;
                float y = (m.M01 + m.M10) / s;
                float z = (m.M02 + m.M20) / s;
                float w = (m.M21 - m.M12) / s;
                return new Rot3f(w, x, y, z).Normalized;
            }
            else if (m.M11 > m.M22)
            {
                float s = Fun.Sqrt(1 + m.M11 - m.M00 - m.M22) * 2;
                float x = (m.M01 + m.M10) / s;
                float y = s / 4;
                float z = (m.M12 + m.M21) / s;
                float w = (m.M02 - m.M20) / s;
                return new Rot3f(w, x, y, z).Normalized;
            }
            else
            {
                float s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                float x = (m.M02 + m.M20) / s;
                float y = (m.M12 + m.M21) / s;
                float z = s / 4;
                float w = (m.M10 - m.M01) / s;
                return new Rot3f(w, x, y, z).Normalized;
            }
        }

        /// <summary>
        /// Create rotation around the X-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static Rot3f RotationX(float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3f(halfAngle.Cos(), new V3f(halfAngle.Sin(), 0, 0));
        }

        /// <summary>
        /// Create rotation around the Y-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static Rot3f RotationY(float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3f(halfAngle.Cos(), new V3f(0, halfAngle.Sin(), 0));
        }

        /// <summary>
        /// Create rotation around the Z-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static Rot3f RotationZ(float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3f(halfAngle.Cos(), new V3f(0, 0, halfAngle.Sin()));
        }

        /// <summary>
        /// Create rotation from euler angles as vector [roll, pitch, yaw].
        /// The rotation order is yaw (Z), pitch (Y), roll (X).
        /// <param name="rollPitchYawInRadians">[yaw, pitch, roll] in radians</param>
        /// </summary>
        public static Rot3f FromEulerAngles(V3f rollPitchYawInRadians)
        {
            return new Rot3f(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the Rodrigues angle-axis vector of the quaternion.
        /// </summary>
        public V3f ToAngleAxis()
        {
            var sinTheta2 = V.LengthSquared;
            if (sinTheta2 > Constant<float>.PositiveTinyValue)
            {
                float sinTheta = Fun.Sqrt(sinTheta2);
                float cosTheta = W;
                float twoTheta = 2 * (cosTheta < 0 ? Fun.Atan2(-sinTheta, -cosTheta)
                                                    : Fun.Atan2(sinTheta, cosTheta));
                return V * (twoTheta / sinTheta);
            }
            else
                return V3f.Zero;
        }

        /// <summary>
        /// Converts this Rotation to the axis angle representation.
        /// </summary>
        /// <param name="axis">Output of normalized axis of rotation.</param>
        /// <param name="angleInRadians">Output of angle of rotation about axis (Right Hand Rule).</param>
        public void ToAxisAngle(ref V3f axis, ref float angleInRadians)
        {
            if (!Fun.ApproximateEquals(NormSquared, 1, 0.001))
                throw new ArgumentException("Quaternion needs to be normalized to represent a rotation.");
            angleInRadians = 2 * Fun.Acos(W);
            var s = Fun.Sqrt(1 - W * W); // assuming quaternion normalised then w is less than 1, so term always positive.
            if (s < 0.001)
            { // test to avoid divide by zero, s is always positive due to sqrt
                // if s close to zero then direction of axis not important
                axis.X = X; // if it is important that axis is normalised then replace with x=1; y=z=0;
                axis.Y = Y;
                axis.Z = Z;
            }
            else
            {
                axis.X = X / s; // normalise axis
                axis.Y = Y / s;
                axis.Z = Z / s;
            }
        }

        // [todo ISSUE 20090421 andi> caching of the Matrix would greatly improve performance.
        // Implement Rot3f as a Matrix-backed Quaternion. Quaternion should be its own class with all Quaternion-operations, 
        // and Rot3f only an efficient Rotation (Matrix) that is has its Orthonormalization-Constraint enforced (by a Quaternion).
        //<]
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
                1 - 2 * (yy + xx)
                );
        }

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

                0, 0, 0, 1
                );
        }

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
                0
                );
        }

        public static explicit operator float[](Rot3f r)
        {
            float[] array = new float[4];
            array[0] = r.W;
            array[1] = r.X;
            array[2] = r.Y;
            array[3] = r.Z;
            return array;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(W, V);
        }

        public override bool Equals(object other)
        {
            return (other is Rot3f) ? (this == (Rot3f)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", W, V);
        }

        public static Rot3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Rot3f(float.Parse(x[0]), V3f.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Fun
    {
        #region Log, Exp

        /// <summary>
        /// Calculates the logarithm of the given quaternion.
        /// </summary>
        public static Rot3f Log(this Rot3f a)
        {
            var result = Rot3f.Zero;

            if (a.W.Abs() < 1)
            {
                var angle = a.W.Acos();
                var sin = angle.Sin();
                result.V = (sin.Abs() >= 0) ? (a.V * (angle / sin)) : a.V;
            }

            return result;
        }

        /// <summary>
        /// Calculates exponent of the given quaternion.
        /// </summary>
        public static Rot3f Exp(this Rot3f a)
        {
            var result = Rot3f.Zero;

            var angle = (a.X * a.X + a.Y * a.Y + a.Z * a.Z).Sqrt();
            var sin = angle.Sin();

            if (sin.Abs() > 0)
            {
                var coeff = angle / sin;
                result.V = coeff * a.V;
            }
            else
            {
                result.V = a.V;
            }

            return result;
        }

        #endregion
    }

    public static partial class Rot
    {

        #region Operations

        /// <summary>
        /// Inverts the given quaternion (multiplicative inverse).
        /// </summary>
        public static void Invert(this ref Rot3f r)
        {
            var norm = r.NormSquared;
            if (norm == 0) return;
            var scale = 1 / norm;
            r.W *= scale;
            r.V *= -scale;
        }

        /// <summary>
        /// Conjugates the given quaternion.
        /// For normalized rotation-quaternions this is the same as Invert().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Conjugate(this ref Rot3f r)
        {
            r.V = -r.V;
        }

        /// <summary>
        /// Normalizes the given quaternion.
        /// </summary>
        public static void Normalize(this ref Rot3f r)
        {
            var norm = r.Norm;
            if (norm == 0) return;
            var scale = 1 / norm;
            r.W *= scale;
            r.V *= scale;
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/q.w, 1/q.x, 1/q.y, 1/q.z)
        /// of quaternion q.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f Reciprocal(Rot3f q)
            => q.Reciprocal;

        /// <summary> 
        /// Returns the dot product of two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(this Rot3f a, Rot3f b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a vector with a quaternion.
        /// </summary>
        public static V3f Transform(this Rot3f q, V3f v)
        {
            // q * v * q'

            // step 1: tmp = q * Quaternion(0, v)
            var w = -q.X * v.X - q.Y * v.Y - q.Z * v.Z;
            var x = q.W * v.X + q.Y * v.Z - q.Z * v.Y;
            var y = q.W * v.Y + q.Z * v.X - q.X * v.Z;
            var z = q.W * v.Z + q.X * v.Y - q.Y * v.X;

            // step 2: tmp * q.Conjungated (q.W, -q.V)
            return new V3f(
                -w * q.X + x * q.W - y * q.Z + z * q.Y,
                -w * q.Y + y * q.W - z * q.X + x * q.Z,
                -w * q.Z + z * q.W - x * q.Y + y * q.X
                );
        }

        /// <summary>
        /// Transforms a vector with the inverse of a quaternion.
        /// </summary>
        public static V3f InvTransform(this Rot3f q, V3f v)
        {
            // q' * v * q

            // step 1: tmp = q.Conungated * Rot3d(0, v)
            var w = q.X * v.X + q.Y * v.Y + q.Z * v.Z;
            var x = q.W * v.X - q.Y * v.Z + q.Z * v.Y;
            var y = q.W * v.Y - q.Z * v.X + q.X * v.Z;
            var z = q.W * v.Z - q.X * v.Y + q.Y * v.X;

            // step 2: tmp * q
            return new V3f(
                w * q.X + x * q.W + y * q.Z - z * q.Y,
                w * q.Y + y * q.W + z * q.X - x * q.Z,
                w * q.Z + z * q.W + x * q.Y - y * q.X
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

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add Rot3f.ApproximateEquals(Rot3f other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot3f r0, Rot3f r1, float tolerance)
        {
            return (r0.W - r1.W).Abs() <= tolerance &&
                   (r0.X - r1.X).Abs() <= tolerance &&
                   (r0.Y - r1.Y).Abs() <= tolerance &&
                   (r0.Z - r1.Z).Abs() <= tolerance;
        }

        #endregion
    }

    #endregion

    #region Rot3d

    /// <summary>
    /// Type for general quaternions, if normalized it represents an arbritrary rotation in three dimensions.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rot3d
    {
        [DataMember]
        public double W;
        [DataMember]
        public double X;
        [DataMember]
        public double Y;
        [DataMember]
        public double Z;

        #region Constructors

        /// <summary>
        /// Creates quaternion (w, (x, y, z)).
        /// </summary>
        public Rot3d(double w, double x, double y, double z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates quaternion (w, (v.x, v.y, v.z)).
        /// </summary>
        public Rot3d(double w, V3d v)
        {
            W = w;
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Creates quaternion from array.
        /// (w = a[0], (x = a[1], y = a[2], z = a[3])).
        /// </summary>
        public Rot3d(double[] a)
        {
            W = a[0];
            X = a[1];
            Y = a[2];
            Z = a[3];
        }

        /// <summary>
        /// Creates quaternion from array starting at specified index.
        /// (w = a[start], (x = a[start+1], y = a[start+2], z = a[start+3])).
        /// </summary>
        public Rot3d(double[] a, int start)
        {
            W = a[start];
            X = a[start + 1];
            Y = a[start + 2];
            Z = a[start + 3];
        }

        /// <summary>
        /// Creates quaternion representing a rotation around an axis by an angle.
        /// The axis vector has to be normalized.
        /// </summary>
        public Rot3d(V3d normalizedAxis, double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            W = halfAngle.Cos();
            var halfAngleSin = halfAngle.Sin();
            X = normalizedAxis.X * halfAngleSin;
            Y = normalizedAxis.Y * halfAngleSin;
            Z = normalizedAxis.Z * halfAngleSin;
        }

        /// <summary>
        /// Creates quaternion from euler angles [roll, pitch, yaw].
        /// The rotation order is yaw (Z), pitch (Y), roll (X).
        /// </summary>
        /// <param name="rollInRadians">Rotation around X</param>
        /// <param name="pitchInRadians">Rotation around Y</param>
        /// <param name="yawInRadians">Rotation around Z</param>
        public Rot3d(double rollInRadians, double pitchInRadians, double yawInRadians)
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
            W = cy * cp * cr + sy * sp * sr;
            X = cy * cp * sr - sy * sp * cr;
            Y = sy * cp * sr + cy * sp * cr;
            Z = sy * cp * cr - cy * sp * sr;
        }

        /// <summary>
        /// Creates a quaternion representing a rotation from one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        public Rot3d(V3d from, V3d into)
        {
            var angle = from.AngleBetween(into);

            // some vectors do not normalize to 1.0 -> Vec.Dot = -0.99999999999999989 || -0.99999994f
            // acos => 3.1415926386886319 or 3.14124632f -> delta of 1e-7 or 1e-3 -> using AngleBetween allows higher precision again
            if (angle < 1e-16)
            {
                // axis = a; angle = 0;
                W = 1;
                X = 0;
                Y = 0;
                Z = 0;
            }
            else if (Constant.Pi - angle.Abs() < 1e-16)
            {
                //axis = a.AxisAlignedNormal(); //angle = PI;
                this = new Rot3d(0, from.AxisAlignedNormal());
            }
            else
            {
                V3d axis = Vec.Cross(from, into).Normalized;
                this = new Rot3d(axis, angle);
            }
        }

        #endregion

        #region Properties

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new ArgumentException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new ArgumentException();
                }
            }
        }

        public V3d V
        {
            get { return new V3d(X, Y, Z); }
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        #endregion

        #region Constants
        /// <summary>
        /// Zero (0, (0,0,0))
        /// </summary>
        public static Rot3d Zero => new Rot3d(0, V3d.Zero);

        /// <summary>
        /// Identity (1, (0,0,0)).
        /// </summary>
        public static Rot3d Identity => new Rot3d(1, 0, 0, 0);

        /// <summary>
        /// X-Axis (0, (1,0,0)).
        /// </summary>
        public static Rot3d XAxis => new Rot3d(0, 1, 0, 0);

        /// <summary>
        /// Y-Axis (0, (0,1,0)).
        /// </summary>
        public static Rot3d YAxis => new Rot3d(0, 0, 1, 0);

        /// <summary>
        /// Z-Axis (0, (0,0,1)).
        /// </summary>
        public static Rot3d ZAxis => new Rot3d(0, 0, 0, 1);

        #endregion

        #region Quaternion Arithmetics

        /// <summary>
        /// Gets squared norm (or squared length) of this quaternion.
        /// </summary>
        public double NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => W * W + V.LengthSquared;
        }

        /// <summary>
        /// Gets norm (or length) of this quaternion.
        /// </summary>
        public double Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NormSquared.Sqrt();
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this quaternion.
        /// </summary>
        public Rot3d Normalized
        {
            get
            {
                var norm = Norm;
                if (norm == 0) return Rot3d.Zero;
                var scale = 1 / norm;
                return new Rot3d(W * scale, V * scale);
            }
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this quaternion.
        /// </summary>
        public Rot3d Inverse
        {
            get
            {
                var norm = NormSquared;
                if (norm == 0) return Rot3d.Zero;
                var scale = 1 / norm;
                return new Rot3d(W * scale, V * (-scale));
            }
        }

        /// <summary>
        /// Gets the conjugate of this quaternion.
        /// For normalized rotation-quaternions this is the same as Inverted().
        /// </summary>
        public Rot3d Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Rot3d(W, -V);
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/W, 1/X, 1/Y, 1/Z).
        /// </summary>
        public Rot3d Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Rot3d(1 / W, 1 / X, 1 / Y, 1 / Z);
        }

        /// <summary>
        /// Returns the Euler-Angles from the quatarnion as vector.
        /// The vector components represent [roll (X), pitch (Y), yaw (Z)] with rotation order is Z, Y, X.
        /// </summary>
        public V3d GetEulerAngles()
        {
            var test = W * Y - X * Z;
            if (test > 0.49999999999999) // singularity at north pole
            {
                return new V3d(
                    2 * Fun.Atan2(X, W),
                    Constant.PiHalf,
                    0);
            }
            if (test < -0.49999999999999) // singularity at south pole
            {
                return new V3d(
                    2 * Fun.Atan2(X, W),
                    -Constant.PiHalf,
                    0);
            }
            // From Wikipedia, conversion between quaternions and Euler angles.
            return new V3d(
                        Fun.Atan2(2 * (W * X + Y * Z),
                                  1 - 2 * (X * X + Y * Y)),
                        Fun.AsinClamped(2 * test),
                        Fun.Atan2(2 * (W * Z + X * Y),
                                  1 - 2 * (Y * Y + Z * Z)));
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Returns the component-wise negation (-q.w, -q.v) of quaternion q.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator -(Rot3d q)
        {
            return new Rot3d(-q.W, -q.X, -q.Y, -q.Z);
        }

        /// <summary>
        /// Returns the sum of two quaternions (a.w + b.w, a.v + b.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator +(Rot3d a, Rot3d b)
        {
            return new Rot3d(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator +(Rot3d q, double s)
        {
            return new Rot3d(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator +(double s, Rot3d q)
        {
            return new Rot3d(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (a.w - b.w, a.v - b.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator -(Rot3d a, Rot3d b)
        {
            return new Rot3d(a.W - b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Returns (q.w - s, (q.x - s, q.y - s, q.z - s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator -(Rot3d q, double s)
        {
            return new Rot3d(q.W - s, q.X - s, q.Y - s, q.Z - s);
        }

        /// <summary>
        /// Returns (s - q.w, (s - q.x, s- q.y, s- q.z)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator -(double s, Rot3d q)
        {
            return new Rot3d(s - q.W, s - q.X, s - q.Y, s - q.Z);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator *(Rot3d q, double s)
        {
            return new Rot3d(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator *(double s, Rot3d q)
        {
            return new Rot3d(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator *(Rot3d a, Rot3d b)
        {
            return new Rot3d(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d operator *(Rot3d q, V3d v)
        {
            return q.Transform(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d operator *(V3d v, Rot3d q)
        {
            return q.InvTransform(v);
        }

#if false //// [todo ISSUE 20090421 andi : andi] check if these are really necessary and comment them what they really do.
        /// <summary>
        /// </summary>
        public static V3d operator *(Rot3d rot, V2d v)
        {
            return (M33d)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V3d operator *(Rot3d rot, V3d v)
        {
            return (M33d)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V4d operator *(Rot3d rot, V4d v)
        {
            return (M33d)rot * v;
        }

        /// <summary>
        /// </summary>
        public static M33d operator *(Rot3d r3, Rot2d r2)
        {
            M33d m33 = (M33d)r3;
            M22d m22 = (M22d)r2;
            return new M33d(
                m33.M00 * m22.M00 + m33.M01 * m22.M10,
                m33.M00 * m22.M01 + m33.M01 * m22.M11,
                m33.M02,

                m33.M10 * m22.M00 + m33.M11 * m22.M10,
                m33.M10 * m22.M01 + m33.M11 * m22.M11,
                m33.M12,

                m33.M20 * m22.M00 + m33.M21 * m22.M10,
                m33.M20 * m22.M01 + m33.M21 * m22.M11,
                m33.M22
                );

        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Rot3d rot, Scale3d m)
        {
            return (M33d)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Rot3d rot, Shift3d m)
        {
            return (M33d)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Rot3d rot, M33d m)
        {
            return (M33d)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(M33d m, Rot3d rot)
        {
            return m * (M33d)rot;
        }

        /// <summary>
        /// Divides two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator /(Rot3d a, Rot3d b)
        {
            return a * b.Reciprocal;
        }

        /// <summary>
        /// Returns (q.w / s, q.v / s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator /(Rot3d q, double s)
        {
            return new Rot3d(q.W / s, q.X / s, q.Y / s, q.Z / s);
        }

        /// <summary>
        /// Returns (s / q.w, s / q.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d operator /(double s, Rot3d q)
        {
            return new Rot3d(s / q.W, s / q.X, s / q.Y, s / q.Z);
        }

        #endregion

        #region Comparison Operators

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        public static bool operator ==(Rot3d r0, Rot3d r1)
        {
            return r0.W == r1.W && r0.X == r1.X && r0.Y == r1.Y && r0.Z == r1.Z;
        }

        public static bool operator !=(Rot3d r0, Rot3d r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Creator Functions

        /// <summary>
        /// WARNING: UNTESTED!!!
        /// </summary>
        public static Rot3d FromFrame(V3d x, V3d y, V3d z)
        {
            return FromM33d(M33d.FromCols(x, y, z));
        }

        /// <summary>
        /// Create from Rodrigues axis-angle vactor
        /// </summary>
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
        /// Creates a quaternion from a rotation matrix
        /// </summary>
        /// <param name="m"></param>
        /// <param name="epsilon"></param>
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
                return new Rot3d(w, x, y, z).Normalized;
            }
            else if (m.M00 > m.M11 && m.M00 > m.M22)
            {
                double s = Fun.Sqrt(1 + m.M00 - m.M11 - m.M22) * 2;
                double x = s / 4;
                double y = (m.M01 + m.M10) / s;
                double z = (m.M02 + m.M20) / s;
                double w = (m.M21 - m.M12) / s;
                return new Rot3d(w, x, y, z).Normalized;
            }
            else if (m.M11 > m.M22)
            {
                double s = Fun.Sqrt(1 + m.M11 - m.M00 - m.M22) * 2;
                double x = (m.M01 + m.M10) / s;
                double y = s / 4;
                double z = (m.M12 + m.M21) / s;
                double w = (m.M02 - m.M20) / s;
                return new Rot3d(w, x, y, z).Normalized;
            }
            else
            {
                double s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                double x = (m.M02 + m.M20) / s;
                double y = (m.M12 + m.M21) / s;
                double z = s / 4;
                double w = (m.M10 - m.M01) / s;
                return new Rot3d(w, x, y, z).Normalized;
            }
        }

        /// <summary>
        /// Create rotation around the X-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static Rot3d RotationX(double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3d(halfAngle.Cos(), new V3d(halfAngle.Sin(), 0, 0));
        }

        /// <summary>
        /// Create rotation around the Y-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static Rot3d RotationY(double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3d(halfAngle.Cos(), new V3d(0, halfAngle.Sin(), 0));
        }

        /// <summary>
        /// Create rotation around the Z-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static Rot3d RotationZ(double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new Rot3d(halfAngle.Cos(), new V3d(0, 0, halfAngle.Sin()));
        }

        /// <summary>
        /// Create rotation from euler angles as vector [roll, pitch, yaw].
        /// The rotation order is yaw (Z), pitch (Y), roll (X).
        /// <param name="rollPitchYawInRadians">[yaw, pitch, roll] in radians</param>
        /// </summary>
        public static Rot3d FromEulerAngles(V3d rollPitchYawInRadians)
        {
            return new Rot3d(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the Rodrigues angle-axis vector of the quaternion.
        /// </summary>
        public V3d ToAngleAxis()
        {
            var sinTheta2 = V.LengthSquared;
            if (sinTheta2 > Constant<double>.PositiveTinyValue)
            {
                double sinTheta = Fun.Sqrt(sinTheta2);
                double cosTheta = W;
                double twoTheta = 2 * (cosTheta < 0 ? Fun.Atan2(-sinTheta, -cosTheta)
                                                    : Fun.Atan2(sinTheta, cosTheta));
                return V * (twoTheta / sinTheta);
            }
            else
                return V3d.Zero;
        }

        /// <summary>
        /// Converts this Rotation to the axis angle representation.
        /// </summary>
        /// <param name="axis">Output of normalized axis of rotation.</param>
        /// <param name="angleInRadians">Output of angle of rotation about axis (Right Hand Rule).</param>
        public void ToAxisAngle(ref V3d axis, ref double angleInRadians)
        {
            if (!Fun.ApproximateEquals(NormSquared, 1, 0.001))
                throw new ArgumentException("Quaternion needs to be normalized to represent a rotation.");
            angleInRadians = 2 * Fun.Acos(W);
            var s = Fun.Sqrt(1 - W * W); // assuming quaternion normalised then w is less than 1, so term always positive.
            if (s < 0.001)
            { // test to avoid divide by zero, s is always positive due to sqrt
                // if s close to zero then direction of axis not important
                axis.X = X; // if it is important that axis is normalised then replace with x=1; y=z=0;
                axis.Y = Y;
                axis.Z = Z;
            }
            else
            {
                axis.X = X / s; // normalise axis
                axis.Y = Y / s;
                axis.Z = Z / s;
            }
        }

        // [todo ISSUE 20090421 andi> caching of the Matrix would greatly improve performance.
        // Implement Rot3d as a Matrix-backed Quaternion. Quaternion should be its own class with all Quaternion-operations, 
        // and Rot3d only an efficient Rotation (Matrix) that is has its Orthonormalization-Constraint enforced (by a Quaternion).
        //<]
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
                1 - 2 * (yy + xx)
                );
        }

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

                0, 0, 0, 1
                );
        }

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
                0
                );
        }

        public static explicit operator double[](Rot3d r)
        {
            double[] array = new double[4];
            array[0] = r.W;
            array[1] = r.X;
            array[2] = r.Y;
            array[3] = r.Z;
            return array;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(W, V);
        }

        public override bool Equals(object other)
        {
            return (other is Rot3d) ? (this == (Rot3d)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", W, V);
        }

        public static Rot3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Rot3d(double.Parse(x[0]), V3d.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Fun
    {
        #region Log, Exp

        /// <summary>
        /// Calculates the logarithm of the given quaternion.
        /// </summary>
        public static Rot3d Log(this Rot3d a)
        {
            var result = Rot3d.Zero;

            if (a.W.Abs() < 1)
            {
                var angle = a.W.Acos();
                var sin = angle.Sin();
                result.V = (sin.Abs() >= 0) ? (a.V * (angle / sin)) : a.V;
            }

            return result;
        }

        /// <summary>
        /// Calculates exponent of the given quaternion.
        /// </summary>
        public static Rot3d Exp(this Rot3d a)
        {
            var result = Rot3d.Zero;

            var angle = (a.X * a.X + a.Y * a.Y + a.Z * a.Z).Sqrt();
            var sin = angle.Sin();

            if (sin.Abs() > 0)
            {
                var coeff = angle / sin;
                result.V = coeff * a.V;
            }
            else
            {
                result.V = a.V;
            }

            return result;
        }

        #endregion
    }

    public static partial class Rot
    {

        #region Operations

        /// <summary>
        /// Inverts the given quaternion (multiplicative inverse).
        /// </summary>
        public static void Invert(this ref Rot3d r)
        {
            var norm = r.NormSquared;
            if (norm == 0) return;
            var scale = 1 / norm;
            r.W *= scale;
            r.V *= -scale;
        }

        /// <summary>
        /// Conjugates the given quaternion.
        /// For normalized rotation-quaternions this is the same as Invert().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Conjugate(this ref Rot3d r)
        {
            r.V = -r.V;
        }

        /// <summary>
        /// Normalizes the given quaternion.
        /// </summary>
        public static void Normalize(this ref Rot3d r)
        {
            var norm = r.Norm;
            if (norm == 0) return;
            var scale = 1 / norm;
            r.W *= scale;
            r.V *= scale;
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/q.w, 1/q.x, 1/q.y, 1/q.z)
        /// of quaternion q.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d Reciprocal(Rot3d q)
            => q.Reciprocal;

        /// <summary> 
        /// Returns the dot product of two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(this Rot3d a, Rot3d b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a vector with a quaternion.
        /// </summary>
        public static V3d Transform(this Rot3d q, V3d v)
        {
            // q * v * q'

            // step 1: tmp = q * Quaternion(0, v)
            var w = -q.X * v.X - q.Y * v.Y - q.Z * v.Z;
            var x = q.W * v.X + q.Y * v.Z - q.Z * v.Y;
            var y = q.W * v.Y + q.Z * v.X - q.X * v.Z;
            var z = q.W * v.Z + q.X * v.Y - q.Y * v.X;

            // step 2: tmp * q.Conjungated (q.W, -q.V)
            return new V3d(
                -w * q.X + x * q.W - y * q.Z + z * q.Y,
                -w * q.Y + y * q.W - z * q.X + x * q.Z,
                -w * q.Z + z * q.W - x * q.Y + y * q.X
                );
        }

        /// <summary>
        /// Transforms a vector with the inverse of a quaternion.
        /// </summary>
        public static V3d InvTransform(this Rot3d q, V3d v)
        {
            // q' * v * q

            // step 1: tmp = q.Conungated * Rot3d(0, v)
            var w = q.X * v.X + q.Y * v.Y + q.Z * v.Z;
            var x = q.W * v.X - q.Y * v.Z + q.Z * v.Y;
            var y = q.W * v.Y - q.Z * v.X + q.X * v.Z;
            var z = q.W * v.Z - q.X * v.Y + q.Y * v.X;

            // step 2: tmp * q
            return new V3d(
                w * q.X + x * q.W + y * q.Z - z * q.Y,
                w * q.Y + y * q.W + z * q.X - x * q.Z,
                w * q.Z + z * q.W + x * q.Y - y * q.X
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

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add Rot3d.ApproximateEquals(Rot3d other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot3d r0, Rot3d r1, double tolerance)
        {
            return (r0.W - r1.W).Abs() <= tolerance &&
                   (r0.X - r1.X).Abs() <= tolerance &&
                   (r0.Y - r1.Y).Abs() <= tolerance &&
                   (r0.Z - r1.Z).Abs() <= tolerance;
        }

        #endregion
    }

    #endregion

}

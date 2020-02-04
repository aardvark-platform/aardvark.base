using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var tc = isDouble ? "d" : "f";
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var v4t = "V4" + tc;
    //#   var rot2t = "Rot2" + tc;
    //#   var rot3t = "Rot3" + tc;
    //#   var scale3t = "Scale3" + tc;
    //#   var shift3t = "Shift3" + tc;
    //#   var m22t = "M22" + tc;
    //#   var m23t = "M23" + tc;
    //#   var m33t = "M33" + tc;
    //#   var m34t = "M34" + tc;
    //#   var m44t = "M44" + tc;
    //#   var rotIntoEps = isDouble ? "1e-16" : "1e-6f";
    //#   var eulerAnglesEps = isDouble ? "0.49999999999999" : "0.49999f";
    //#   var pi = isDouble ? "Constant.Pi" : "Constant.PiF";
    //#   var piHalf = isDouble ? "Constant.PiHalf" : "(float)Constant.PiHalf";
    /// <summary>
    /// Type for general quaternions, if normalized it represents an arbritrary rotation in three dimensions.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __rot3t__
    {
        [DataMember]
        public __ft__ W;
        [DataMember]
        public __ft__ X;
        [DataMember]
        public __ft__ Y;
        [DataMember]
        public __ft__ Z;

        #region Constructors

        /// <summary>
        /// Creates quaternion (w, (x, y, z)).
        /// </summary>
        public __rot3t__(__ft__ w, __ft__ x, __ft__ y, __ft__ z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates quaternion (w, (v.x, v.y, v.z)).
        /// </summary>
        public __rot3t__(__ft__ w, __v3t__ v)
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
        public __rot3t__(__ft__[] a)
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
        public __rot3t__(__ft__[] a, int start)
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
        public __rot3t__(__v3t__ normalizedAxis, __ft__ angleInRadians)
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
        public __rot3t__(__ft__ rollInRadians, __ft__ pitchInRadians, __ft__ yawInRadians)
        {
            __ft__ rollHalf = rollInRadians / 2;
            __ft__ cr = Fun.Cos(rollHalf);
            __ft__ sr = Fun.Sin(rollHalf);
            __ft__ pitchHalf = pitchInRadians / 2;
            __ft__ cp = Fun.Cos(pitchHalf);
            __ft__ sp = Fun.Sin(pitchHalf);
            __ft__ yawHalf = yawInRadians / 2;
            __ft__ cy = Fun.Cos(yawHalf);
            __ft__ sy = Fun.Sin(yawHalf);
            W = cy * cp * cr + sy * sp * sr;
            X = cy * cp * sr - sy * sp * cr;
            Y = sy * cp * sr + cy * sp * cr;
            Z = sy * cp * cr - cy * sp * sr;
        }

        /// <summary>
        /// Creates a quaternion representing a rotation from one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        public __rot3t__(__v3t__ from, __v3t__ into)
        {
            var angle = from.AngleBetween(into);

            // some vectors do not normalize to 1.0 -> Vec.Dot = -0.99999999999999989 || -0.99999994f
            // acos => 3.1415926386886319 or 3.14124632f -> delta of 1e-7 or 1e-3 -> using AngleBetween allows higher precision again
            if (angle < __rotIntoEps__)
            {
                // axis = a; angle = 0;
                W = 1;
                X = 0;
                Y = 0;
                Z = 0;
            }
            else if (__pi__ - angle.Abs() < __rotIntoEps__)
            {
                //axis = a.AxisAlignedNormal(); //angle = PI;
                this = new __rot3t__(0, from.AxisAlignedNormal());
            }
            else
            {
                __v3t__ axis = Vec.Cross(from, into).Normalized;
                this = new __rot3t__(axis, angle);
            }
        }

        #endregion

        #region Properties

        public __ft__ this[int index]
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

        public __v3t__ V
        {
            get { return new __v3t__(X, Y, Z); }
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        #endregion

        #region Constants
        /// <summary>
        /// Zero (0, (0,0,0))
        /// </summary>
        public static __rot3t__ Zero => new __rot3t__(0, __v3t__.Zero);

        /// <summary>
        /// Identity (1, (0,0,0)).
        /// </summary>
        public static __rot3t__ Identity => new __rot3t__(1, 0, 0, 0);

        /// <summary>
        /// X-Axis (0, (1,0,0)).
        /// </summary>
        public static __rot3t__ XAxis => new __rot3t__(0, 1, 0, 0);

        /// <summary>
        /// Y-Axis (0, (0,1,0)).
        /// </summary>
        public static __rot3t__ YAxis => new __rot3t__(0, 0, 1, 0);

        /// <summary>
        /// Z-Axis (0, (0,0,1)).
        /// </summary>
        public static __rot3t__ ZAxis => new __rot3t__(0, 0, 0, 1);

        #endregion

        #region Quaternion Arithmetics

        /// <summary>
        /// Gets squared norm (or squared length) of this quaternion.
        /// </summary>
        public __ft__ NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => W * W + V.LengthSquared;
        }

        /// <summary>
        /// Gets norm (or length) of this quaternion.
        /// </summary>
        public __ft__ Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NormSquared.Sqrt();
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this quaternion.
        /// </summary>
        public __rot3t__ Normalized
        {
            get
            {
                var norm = Norm;
                if (norm == 0) return __rot3t__.Zero;
                var scale = 1 / norm;
                return new __rot3t__(W * scale, V * scale);
            }
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this quaternion.
        /// </summary>
        public __rot3t__ Inverse
        {
            get
            {
                var norm = NormSquared;
                if (norm == 0) return __rot3t__.Zero;
                var scale = 1 / norm;
                return new __rot3t__(W * scale, V * (-scale));
            }
        }

        /// <summary>
        /// Gets the conjugate of this quaternion.
        /// For normalized rotation-quaternions this is the same as Inverted().
        /// </summary>
        public __rot3t__ Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __rot3t__(W, -V);
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/W, 1/X, 1/Y, 1/Z).
        /// </summary>
        public __rot3t__ Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __rot3t__(1 / W, 1 / X, 1 / Y, 1 / Z);
        }

        /// <summary>
        /// Returns the Euler-Angles from the quatarnion as vector.
        /// The vector components represent [roll (X), pitch (Y), yaw (Z)] with rotation order is Z, Y, X.
        /// </summary>
        public __v3t__ GetEulerAngles()
        {
            var test = W * Y - X * Z;
            if (test > __eulerAnglesEps__) // singularity at north pole
            {
                return new __v3t__(
                    2 * Fun.Atan2(X, W),
                    __piHalf__,
                    0);
            }
            if (test < -__eulerAnglesEps__) // singularity at south pole
            {
                return new __v3t__(
                    2 * Fun.Atan2(X, W),
                    -__piHalf__,
                    0);
            }
            // From Wikipedia, conversion between quaternions and Euler angles.
            return new __v3t__(
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
        public static __rot3t__ operator -(__rot3t__ q)
        {
            return new __rot3t__(-q.W, -q.X, -q.Y, -q.Z);
        }

        /// <summary>
        /// Returns the sum of two quaternions (a.w + b.w, a.v + b.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator +(__rot3t__ a, __rot3t__ b)
        {
            return new __rot3t__(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator +(__rot3t__ q, __ft__ s)
        {
            return new __rot3t__(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator +(__ft__ s, __rot3t__ q)
        {
            return new __rot3t__(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (a.w - b.w, a.v - b.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator -(__rot3t__ a, __rot3t__ b)
        {
            return new __rot3t__(a.W - b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Returns (q.w - s, (q.x - s, q.y - s, q.z - s)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator -(__rot3t__ q, __ft__ s)
        {
            return new __rot3t__(q.W - s, q.X - s, q.Y - s, q.Z - s);
        }

        /// <summary>
        /// Returns (s - q.w, (s - q.x, s- q.y, s- q.z)).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator -(__ft__ s, __rot3t__ q)
        {
            return new __rot3t__(s - q.W, s - q.X, s - q.Y, s - q.Z);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator *(__rot3t__ q, __ft__ s)
        {
            return new __rot3t__(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator *(__ft__ s, __rot3t__ q)
        {
            return new __rot3t__(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator *(__rot3t__ a, __rot3t__ b)
        {
            return new __rot3t__(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ operator *(__rot3t__ q, __v3t__ v)
        {
            return q.Transform(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ operator *(__v3t__ v, __rot3t__ q)
        {
            return q.InvTransform(v);
        }

#if false //// [todo ISSUE 20090421 andi : andi] check if these are really necessary and comment them what they really do.
        /// <summary>
        /// </summary>
        public static __v3t__ operator *(__rot3t__ rot, __v2t__ v)
        {
            return (__m33t__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static __v3t__ operator *(__rot3t__ rot, __v3t__ v)
        {
            return (__m33t__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static __v4t__ operator *(__rot3t__ rot, __v4t__ v)
        {
            return (__m33t__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static __m33t__ operator *(__rot3t__ r3, __rot2t__ r2)
        {
            __m33t__ m33 = (__m33t__)r3;
            __m22t__ m22 = (__m22t__)r2;
            return new __m33t__(
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
        public static __m33t__ operator *(__rot3t__ rot, __scale3t__ m)
        {
            return (__m33t__)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m34t__ operator *(__rot3t__ rot, __shift3t__ m)
        {
            return (__m33t__)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m33t__ operator *(__rot3t__ rot, __m33t__ m)
        {
            return (__m33t__)rot * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m33t__ operator *(__m33t__ m, __rot3t__ rot)
        {
            return m * (__m33t__)rot;
        }

        /// <summary>
        /// Divides two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator /(__rot3t__ a, __rot3t__ b)
        {
            return a * b.Reciprocal;
        }

        /// <summary>
        /// Returns (q.w / s, q.v / s).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator /(__rot3t__ q, __ft__ s)
        {
            return new __rot3t__(q.W / s, q.X / s, q.Y / s, q.Z / s);
        }

        /// <summary>
        /// Returns (s / q.w, s / q.v).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rot3t__ operator /(__ft__ s, __rot3t__ q)
        {
            return new __rot3t__(s / q.W, s / q.X, s / q.Y, s / q.Z);
        }

        #endregion

        #region Comparison Operators

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        public static bool operator ==(__rot3t__ r0, __rot3t__ r1)
        {
            return r0.W == r1.W && r0.X == r1.X && r0.Y == r1.Y && r0.Z == r1.Z;
        }

        public static bool operator !=(__rot3t__ r0, __rot3t__ r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Creator Functions

        /// <summary>
        /// WARNING: UNTESTED!!!
        /// </summary>
        public static __rot3t__ FromFrame(__v3t__ x, __v3t__ y, __v3t__ z)
        {
            return From__m33t__(__m33t__.FromCols(x, y, z));
        }

        /// <summary>
        /// Create from Rodrigues axis-angle vactor
        /// </summary>
        public static __rot3t__ FromAngleAxis(__v3t__ angleAxis)
        {
            __ft__ theta2 = angleAxis.LengthSquared;
            if (theta2 > Constant<__ft__>.PositiveTinyValue)
            {
                var theta = Fun.Sqrt(theta2);
                var thetaHalf = theta / 2;
                var k = Fun.Sin(thetaHalf) / theta;
                return new __rot3t__(Fun.Cos(thetaHalf), k * angleAxis);
            }
            else
                return new __rot3t__(1, 0, 0, 0);
        }

        /// <summary>
        /// Creates a quaternion from a rotation matrix
        /// </summary>
        /// <param name="m"></param>
        /// <param name="epsilon"></param>
        public static __rot3t__ From__m33t__(__m33t__ m, __ft__ epsilon = (__ft__)1e-6)
        {
            if (!m.IsOrthonormal(epsilon)) throw new ArgumentException("Matrix is not orthonormal.");
            var tr = m.M00 + m.M11 + m.M22;

            if (tr > 0)
            {
                __ft__ s = (tr + 1).Sqrt() * 2;
                __ft__ x = (m.M21 - m.M12) / s;
                __ft__ y = (m.M02 - m.M20) / s;
                __ft__ z = (m.M10 - m.M01) / s;
                __ft__ w = s / 4;
                return new __rot3t__(w, x, y, z).Normalized;
            }
            else if (m.M00 > m.M11 && m.M00 > m.M22)
            {
                __ft__ s = Fun.Sqrt(1 + m.M00 - m.M11 - m.M22) * 2;
                __ft__ x = s / 4;
                __ft__ y = (m.M01 + m.M10) / s;
                __ft__ z = (m.M02 + m.M20) / s;
                __ft__ w = (m.M21 - m.M12) / s;
                return new __rot3t__(w, x, y, z).Normalized;
            }
            else if (m.M11 > m.M22)
            {
                __ft__ s = Fun.Sqrt(1 + m.M11 - m.M00 - m.M22) * 2;
                __ft__ x = (m.M01 + m.M10) / s;
                __ft__ y = s / 4;
                __ft__ z = (m.M12 + m.M21) / s;
                __ft__ w = (m.M02 - m.M20) / s;
                return new __rot3t__(w, x, y, z).Normalized;
            }
            else
            {
                __ft__ s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                __ft__ x = (m.M02 + m.M20) / s;
                __ft__ y = (m.M12 + m.M21) / s;
                __ft__ z = s / 4;
                __ft__ w = (m.M10 - m.M01) / s;
                return new __rot3t__(w, x, y, z).Normalized;
            }
        }

        /// <summary>
        /// Create rotation around the X-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static __rot3t__ RotationX(__ft__ angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new __rot3t__(halfAngle.Cos(), new __v3t__(halfAngle.Sin(), 0, 0));
        }

        /// <summary>
        /// Create rotation around the Y-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static __rot3t__ RotationY(__ft__ angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new __rot3t__(halfAngle.Cos(), new __v3t__(0, halfAngle.Sin(), 0));
        }

        /// <summary>
        /// Create rotation around the Z-axis.
        /// <param name="angleInRadians">Rotation angle in radians</param>
        /// </summary>
        public static __rot3t__ RotationZ(__ft__ angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new __rot3t__(halfAngle.Cos(), new __v3t__(0, 0, halfAngle.Sin()));
        }

        /// <summary>
        /// Create rotation from euler angles as vector [roll, pitch, yaw].
        /// The rotation order is yaw (Z), pitch (Y), roll (X).
        /// <param name="rollPitchYawInRadians">[yaw, pitch, roll] in radians</param>
        /// </summary>
        public static __rot3t__ FromEulerAngles(__v3t__ rollPitchYawInRadians)
        {
            return new __rot3t__(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the Rodrigues angle-axis vector of the quaternion.
        /// </summary>
        public __v3t__ ToAngleAxis()
        {
            var sinTheta2 = V.LengthSquared;
            if (sinTheta2 > Constant<__ft__>.PositiveTinyValue)
            {
                __ft__ sinTheta = Fun.Sqrt(sinTheta2);
                __ft__ cosTheta = W;
                __ft__ twoTheta = 2 * (cosTheta < 0 ? Fun.Atan2(-sinTheta, -cosTheta)
                                                    : Fun.Atan2(sinTheta, cosTheta));
                return V * (twoTheta / sinTheta);
            }
            else
                return __v3t__.Zero;
        }

        /// <summary>
        /// Converts this Rotation to the axis angle representation.
        /// </summary>
        /// <param name="axis">Output of normalized axis of rotation.</param>
        /// <param name="angleInRadians">Output of angle of rotation about axis (Right Hand Rule).</param>
        public void ToAxisAngle(ref __v3t__ axis, ref __ft__ angleInRadians)
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
        // Implement __rot3t__ as a Matrix-backed Quaternion. Quaternion should be its own class with all Quaternion-operations, 
        // and __rot3t__ only an efficient Rotation (Matrix) that is has its Orthonormalization-Constraint enforced (by a Quaternion).
        //<]
        public static explicit operator __m33t__(__rot3t__ r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            __ft__ xx = r.X * r.X;
            __ft__ yy = r.Y * r.Y;
            __ft__ zz = r.Z * r.Z;
            __ft__ xy = r.X * r.Y;
            __ft__ xz = r.X * r.Z;
            __ft__ yz = r.Y * r.Z;
            __ft__ xw = r.X * r.W;
            __ft__ yw = r.Y * r.W;
            __ft__ zw = r.Z * r.W;
            return new __m33t__(
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

        public static explicit operator __m44t__(__rot3t__ r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            __ft__ xx = r.X * r.X;
            __ft__ yy = r.Y * r.Y;
            __ft__ zz = r.Z * r.Z;
            __ft__ xy = r.X * r.Y;
            __ft__ xz = r.X * r.Z;
            __ft__ yz = r.Y * r.Z;
            __ft__ xw = r.X * r.W;
            __ft__ yw = r.Y * r.W;
            __ft__ zw = r.Z * r.W;
            return new __m44t__(
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

        public static explicit operator __m34t__(__rot3t__ r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            __ft__ xx = r.X * r.X;
            __ft__ yy = r.Y * r.Y;
            __ft__ zz = r.Z * r.Z;
            __ft__ xy = r.X * r.Y;
            __ft__ xz = r.X * r.Z;
            __ft__ yz = r.Y * r.Z;
            __ft__ xw = r.X * r.W;
            __ft__ yw = r.Y * r.W;
            __ft__ zw = r.Z * r.W;
            return new __m34t__(
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

        public static explicit operator __ft__[](__rot3t__ r)
        {
            __ft__[] array = new __ft__[4];
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
            return (other is __rot3t__) ? (this == (__rot3t__)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", W, V);
        }

        public static __rot3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __rot3t__(__ft__.Parse(x[0]), __v3t__.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Calculates the logarithm of the given quaternion.
        /// </summary>
        public static __rot3t__ Log(this __rot3t__ a)
        {
            var result = __rot3t__.Zero;

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
        public static __rot3t__ Exp(this __rot3t__ a)
        {
            var result = __rot3t__.Zero;

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
    }

    public static partial class Rot
    {
        /// <summary>
        /// Inverts the given quaternion (multiplicative inverse).
        /// </summary>
        public static void Invert(this ref __rot3t__ r)
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
        public static void Conjugate(this ref __rot3t__ r)
        {
            r.V = -r.V;
        }

        /// <summary>
        /// Normalizes the given quaternion.
        /// </summary>
        public static void Normalize(this ref __rot3t__ r)
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
        public static __rot3t__ Reciprocal(__rot3t__ q)
            => q.Reciprocal;

        /// <summary> 
        /// Returns the dot product of two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ft__ Dot(this __rot3t__ a, __rot3t__ b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Transforms a vector with a quaternion.
        /// </summary>
        public static __v3t__ Transform(this __rot3t__ q, __v3t__ v)
        {
            // q * v * q'

            // step 1: tmp = q * Quaternion(0, v)
            var w = -q.X * v.X - q.Y * v.Y - q.Z * v.Z;
            var x = q.W * v.X + q.Y * v.Z - q.Z * v.Y;
            var y = q.W * v.Y + q.Z * v.X - q.X * v.Z;
            var z = q.W * v.Z + q.X * v.Y - q.Y * v.X;

            // step 2: tmp * q.Conjungated (q.W, -q.V)
            return new __v3t__(
                -w * q.X + x * q.W - y * q.Z + z * q.Y,
                -w * q.Y + y * q.W - z * q.X + x * q.Z,
                -w * q.Z + z * q.W - x * q.Y + y * q.X
                );
        }

        /// <summary>
        /// Transforms a vector with the inverse of a quaternion.
        /// </summary>
        public static __v3t__ InvTransform(this __rot3t__ q, __v3t__ v)
        {
            // q' * v * q

            // step 1: tmp = q.Conungated * Rot3d(0, v)
            var w = q.X * v.X + q.Y * v.Y + q.Z * v.Z;
            var x = q.W * v.X - q.Y * v.Z + q.Z * v.Y;
            var y = q.W * v.Y - q.Z * v.X + q.X * v.Z;
            var z = q.W * v.Z - q.X * v.Y + q.Y * v.X;

            // step 2: tmp * q
            return new __v3t__(
                w * q.X + x * q.W + y * q.Z - z * q.Y,
                w * q.Y + y * q.W + z * q.X - x * q.Z,
                w * q.Z + z * q.W + x * q.Y - y * q.X
                );
        }
    }

    public static partial class Fun
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __rot3t__ r0, __rot3t__ r1)
        {
            return ApproximateEquals(r0, r1, Constant<__ft__>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add __rot3t__.ApproximateEquals(__rot3t__ other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __rot3t__ r0, __rot3t__ r1, __ft__ tolerance)
        {
            return (r0.W - r1.W).Abs() <= tolerance &&
                   (r0.X - r1.X).Abs() <= tolerance &&
                   (r0.Y - r1.Y).Abs() <= tolerance &&
                   (r0.Z - r1.Z).Abs() <= tolerance;
        }
    }

    //# }
}

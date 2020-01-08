using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents an arbitrary rotation in three dimensions. Implemented as
    /// a normalized quaternion.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rot3f
    {
        [DataMember]
        public float W;
        [DataMember]
        public V3f V;

        #region Constructors

        /// <summary>
        /// Creates quaternion (w, (x, y, z)).
        /// </summary>
        // todo ISSUE 20090420 andi : sm, rft: Add asserts for unit-length constraint
        public Rot3f(float w, float x, float y, float z)
        {
            W = w;
            V = new V3f(x, y, z);
        }

        /// <summary>
        /// Creates quaternion (w, (v.x, v.y, v.z)).
        /// </summary>
        public Rot3f(float w, V3f v)
        {
            W = w;
            V = v;
        }

        /// <summary>
        /// Creates quaternion from array.
        /// (w = a[0], (x = a[1], y = a[2], z = a[3])).
        /// </summary>
        public Rot3f(float[] a)
        {
            W = a[0];
            V = new V3f(a[1], a[2], a[3]);
        }

        /// <summary>
        /// Creates quaternion from array starting at specified index.
        /// (w = a[start], (x = a[start+1], y = a[start+2], z = a[start+3])).
        /// </summary>
        public Rot3f(float[] a, int start)
        {
            W = a[start];
            V = new V3f(a[start + 1], a[start + 2], a[start + 3]);
        }

        /// <summary>
        /// Creates quaternion representing a rotation around
        /// an axis by an angle.
        /// </summary>
        // todo ISSUE 20090420 andi : sm, rft: What about adding an AxisAngle struct?.
        public Rot3f(V3f axis, float angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            W = halfAngle.Cos();
            V = axis.Normalized * halfAngle.Sin();
        }

        /// <summary>
        /// Creates quaternion from euler angles [yaw, pitch, roll].
        /// </summary>
        /// <param name="yawInRadians">Rotation around X</param>
        /// <param name="pitchInRadians">Rotation around Y</param>
        /// <param name="rollInRadians">Rotation around Z</param>
        public Rot3f(float yawInRadians, float pitchInRadians, float rollInRadians)
        {
            var qx = new Rot3f(V3f.XAxis, yawInRadians);
            var qy = new Rot3f(V3f.YAxis, pitchInRadians);
            var qz = new Rot3f(V3f.ZAxis, rollInRadians);
            this = qz * qy * qx;
        }

        /// <summary>
        /// Creates a quaternion representing a rotation from one
        /// vector into another.
        /// </summary>
        public Rot3f(V3f from, V3f into)
        {
            var a = from.Normalized;
            var b = into.Normalized;
            var angle = Fun.Clamp(V3f.Dot(a, b), -1, 1).Acos();
            var angleAbs = angle.Abs();
            V3f axis;

            // some vectors do not normalize to 1.0 -> Vec.Dot = -0.99999999999999989 || -0.99999994f
            // acos => 3.1415926386886319 or 3.14124632f -> delta of 1e-7 or 1e-3
            if (angle < 1e-3f)
            {
                axis = a;
                angle = 0;
            }
            else if (Constant.PiF - angleAbs < 1e-3f)
            {
                axis = a.AxisAlignedNormal();
                angle = Constant.PiF;
            }
            else
                axis = V3f.Cross(a, b).Normalized;

            this = new Rot3f(axis, angle);
        }

        #endregion

        #region Properties

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return V.X;
                    case 1: return V.Y;
                    case 2: return V.Z;
                    case 3: return W;
                    default: throw new ArgumentException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0: V.X = value; break;
                    case 1: V.Y = value; break;
                    case 2: V.Z = value; break;
                    case 3: W = value; break;
                    default: throw new ArgumentException();
                }
            }
        }

        public float X
        {
            get { return V.X; }
            set { V.X = value; }
        }

        public float Y
        {
            get { return V.Y; }
            set { V.Y = value; }
        }

        public float Z
        {
            get { return V.Z; }
            set { V.Z = value; }
        }

        #endregion

        #region Constants
#if false
        /// NOTE for developers
        /// A Zero-quaternion does not represent a Rot3, so it should not be implemented
        /// <summary>        /// Zero (0,(0,0,0)).
        /// </summary>
        public static readonly Rot3f Zero = new Rot3f(0, V3f.Zero);
#endif

        /// <summary>
        /// Identity (1,(0,0,0)).
        /// </summary>
        public static readonly Rot3f Identity = new Rot3f(1, V3f.Zero);

        /// <summary>
        /// X-Axis (0, (1,0,0)).
        /// </summary>
        public static readonly Rot3f XAxis = new Rot3f(0, V3f.XAxis);

        /// <summary>
        /// Y-Axis (0, (0,1,0)).
        /// </summary>
        public static readonly Rot3f YAxis = new Rot3f(0, V3f.YAxis);

        /// <summary>
        /// Z-Axis (0, (0,0,1)).
        /// </summary>
        public static readonly Rot3f ZAxis = new Rot3f(0, V3f.ZAxis);

        #endregion

        #region Quaternion Arithmetics

        // [todo ISSUE 20090421 andi : andi>
        // Operations like Add, Subtract and Multiplication with scalar, Divide, Reciprocal
        // should not be defined in a Rot3*.
        // These are perfectly valid for a quaternion, but a rotation is defined on a 
        // NORMALIZED quaternion. This Norm-Constraint would be violated with above operations.
        // <]
        // todo andi {
        /// <summary>
        /// Returns the sum of 2 quaternions (a.w + b.w, a.v + b.v).
        /// </summary>
        public static Rot3f Add(Rot3f a, Rot3f b)
        {
            return new Rot3f(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        public static Rot3f Add(Rot3f q, float s)
        {
            return new Rot3f(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (q.w - s, (q.x - s, q.y - s, q.z - s)).
        /// </summary>
        public static Rot3f Subtract(Rot3f q, float s)
        {
            return Add(q, -s);
        }

        /// <summary>
        /// Returns (s - q.w, (s - q.x, s- q.y, s- q.z)).
        /// </summary>
        public static Rot3f Subtract(float s, Rot3f q)
        {
            return Add(-q, s);
        }

        /// <summary>
        /// Returns (a.w - b.w, a.v - b.v).
        /// </summary>
        public static Rot3f Subtract(Rot3f a, Rot3f b)
        {
            return Add(a, -b);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        public static Rot3f Multiply(Rot3f q, float s)
        {
            return new Rot3f(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }
        // todo andi }

        /// <summary>
        /// Multiplies 2 quaternions.
        /// This concatenates the two rotations into a single one.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static Rot3f Multiply(Rot3f a, Rot3f b)
        {
            return new Rot3f(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
                );
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by quaternion q.
        /// </summary>
        public static V3f TransformDir(Rot3f q, V3f v)
        {
            // first transforming quaternion to M33f is approximately equal in terms of operations ...
            return ((M33f)q).Transform(v);

            // ... than direct multiplication ...
            //QuaternionF r = q.Conjugated() * new QuaternionF(0, v) * q;
            //return new V3f(r.X, r.Y, r.Z);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by quaternion q.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public static V3f TransformPos(Rot3f q, V3f p)
        {
            return TransformDir(q, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of quaternion q.
        /// </summary>
        public static V3f InvTransformDir(Rot3f q, V3f v)
        {
            //for Rotation Matrices R^-1 == R^T:
            return ((M33f)q).TransposedTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the incerse of quaternion q.
        /// For quaternions, this method is equivalent to InvTransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public static V3f InvTransformPos(Rot3f q, V3f p)
        {
            return InvTransformDir(q, p);
        }



        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this quaternion.
        /// </summary>
        public V3f TransformDir(V3f v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this quaternion.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public V3f TransformPos(V3f p)
        {
            return TransformDir(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this quaternion.
        /// </summary>
        public V3f InvTransformDir(V3f v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this quaternion.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public V3f InvTransformPos(V3f p)
        {
            return InvTransformDir(this, p);
        }

        // todo andi {
        public static Rot3f Divide(float s, Rot3f q)
        {
            return new Rot3f(
                s / q.W,
                s / q.X, s / q.Y, s / q.Z
                );
        }

        /// <summary>
        /// Returns (q.w / s, q.v * (1/s)).
        /// </summary>
        public static Rot3f Divide(Rot3f q, float s)
        {
            return Multiply(q, 1 / s);
        }

        /// <summary>
        /// Divides 2 quaternions.
        /// </summary>
        public static Rot3f Divide(Rot3f a, Rot3f b)
        {
            return Multiply(a, Reciprocal(b));
        }
        // todo andi }

        /// <summary>
        /// Returns the component-wise negation (-q.w, -q.v) of quaternion q.
        /// This represents the same rotation.
        /// </summary>
        public static Rot3f Negated(Rot3f q)
        {
            return new Rot3f(-q.W, -q.X, -q.Y, -q.Z);
        }

        // todo andi {
        /// <summary>
        /// Returns the component-wise reciprocal (1/q.w, 1/q.x, 1/q.y, 1/q.z)
        /// of quaternion q.
        /// </summary>
        public static Rot3f Reciprocal(Rot3f q)
        {
            return new Rot3f(1 / q.W, 1 / q.X, 1 / q.Y, 1 / q.Z);
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/w, 1/x, 1/y, 1/z).
        /// </summary>
        public Rot3f OneDividedBy()
        {
            return Reciprocal(this);
        }
        // todo andi }

        /// <summary> 
        /// Returns the dot-product of 2 quaternions.
        /// </summary>
        public static float Dot(Rot3f a, Rot3f b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        // [todo ISSUE 20090225 andi : andi> 
        // Investigate if we can use power to "scale" the rotation (=enlarging the angle of rotation by a factor.)
        // If so, reenable the Log and Exp methods if they are useful then.
        // <]
#if false
        /// <summary>
        /// Calculates the logarithm of the quaternion.
        /// </summary>
        public static Rot3f Log(Rot3f a)
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
        /// Calculates exponent of the quaternion.
        /// </summary>
        public Rot3f Exp(Rot3f a)
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
#endif

        /// <summary>
        /// Gets squared norm (or squared length) of this quaternion.
        /// </summary>
        public float NormSquared
        {
            get
            {
                return W * W + V.LengthSquared;
            }
        }

        /// <summary>
        /// Gets norm (or length) of this quaternion.
        /// </summary>
        public float Norm
        {
            get { return NormSquared.Sqrt(); }
        }

        /// <summary>
        /// Normalizes this quaternion.
        /// </summary>
        /// <returns>This.</returns>
        public void Normalize()
        {
            var norm = Norm;
            if (norm == 0)
                this = Rot3f.Identity;
            else
            {
                var scale = 1 / norm;
                W *= scale;
                V *= scale;
            }
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this quaternion.
        /// </summary>
        public Rot3f Normalized
        {
            get
            {
                var norm = Norm;
                if (norm == 0) return Rot3f.Identity;
                var scale = 1 / norm;
                return new Rot3f(W * scale, V * scale);
            }
        }

        /// <summary>
        /// Inverts this quaternion (multiplicative inverse).
        /// Returns this.
        /// </summary>
        public void Invert()
        {
            var norm = NormSquared;
            if (norm == 0) throw new ArithmeticException("quaternion is not invertible");
            var scale = 1 / norm;
            W *= scale;
            V *= -scale;
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this quaternion.
        /// </summary>
        public Rot3f Inverse
        {
            get
            {
                var norm = NormSquared;
                if (norm == 0) throw new ArithmeticException("quaternion is not invertible");
                var scale = 1 / norm;
                return new Rot3f(W * scale, V * (-scale));
            }
        }

        /// <summary>
        /// Conjugates this quaternion. Returns this.
        /// For normalized rotation-quaternions this is the same as Invert().
        /// </summary>
        public void Conjugate()
        {
            V = -V;
        }

        /// <summary>
        /// Gets the conjugate of this quaternion.
        /// For normalized rotation-quaternions this is the same as Inverted().
        /// </summary>
        public Rot3f Conjugated
        {
            get { return new Rot3f(W, -V); }
        }

        /// <summary>
        /// Returns the Euler-Angles from the quatarnion.
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

        public static bool ApproxEqual(Rot3f r0, Rot3f r1)
        {
            return ApproxEqual(r0, r1, Constant<float>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add Rot3f.ApproxEqual(Rot3f other);
        public static bool ApproxEqual(Rot3f r0, Rot3f r1, float tolerance)
        {
            return (r0.W - r1.W).Abs() <= tolerance &&
                   (r0.X - r1.X).Abs() <= tolerance &&
                   (r0.Y - r1.Y).Abs() <= tolerance &&
                   (r0.Z - r1.Z).Abs() <= tolerance;
        }

        #endregion

        #region Arithmetic Operators

        public static Rot3f operator -(Rot3f rot)
        {
            return Rot3f.Negated(rot);
        }

        // todo andi {
        public static Rot3f operator +(Rot3f a, Rot3f b)
        {
            return Rot3f.Add(a, b);
        }

        public static Rot3f operator +(Rot3f rot, float s)
        {
            return Rot3f.Add(rot, s);
        }

        public static Rot3f operator +(float s, Rot3f rot)
        {
            return Rot3f.Add(rot, s);
        }

        public static Rot3f operator -(Rot3f a, Rot3f b)
        {
            return Rot3f.Subtract(a, b);
        }

        public static Rot3f operator -(Rot3f rot, float s)
        {
            return Rot3f.Subtract(rot, s);
        }

        public static Rot3f operator -(float scalar, Rot3f rot)
        {
            return Rot3f.Subtract(scalar, rot);
        }

        public static Rot3f operator *(Rot3f rot, float s)
        {
            return Rot3f.Multiply(rot, s);
        }

        public static Rot3f operator *(float s, Rot3f rot)
        {
            return Rot3f.Multiply(rot, s);
        }
        // todo andi }

        public static Rot3f operator *(Rot3f a, Rot3f b)
        {
            return Rot3f.Multiply(a, b);
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
        /// <summary>
        /// </summary>
        public static M33f operator *(Rot3f rot, Scale3f m)
        {
            return (M33f)rot * m;
        }

        /// <summary>
        /// </summary>
        public static M34f operator *(Rot3f rot, Shift3f m)
        {
            return (M33f)rot * m;
        }

        public static M33f operator *(Rot3f rot, M33f m)
        {
            return (M33f)rot * m;
        }

        public static M33f operator *(M33f m, Rot3f rot)
        {
            return m * (M33f)rot;
        }

        // todo andi {
        public static Rot3f operator /(Rot3f rot, float s)
        {
            return Rot3f.Divide(rot, s);
        }

        public static Rot3f operator /(float s, Rot3f rot)
        {
            return Rot3f.Divide(s, rot);
        }
        // todo andi }

        #endregion

        #region Comparison Operators

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        public static bool operator ==(Rot3f r0, Rot3f r1)
        {
            return r0.W == r1.W && r0.V == r1.V;
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
            var t = 1 + m.M00 + m.M11 + m.M22;

            if (t > epsilon)
            {
                float s = t.Sqrt() * 2;
                float x = (m.M21 - m.M12) / s;
                float y = (m.M02 - m.M20) / s;
                float z = (m.M10 - m.M01) / s;
                float w = s / 4;
                return new Rot3f(w, x, y, z).Normalized;
            }
            else
            {
                if (m.M00 > m.M11 && m.M00 > m.M22)
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
                    float w = (m.M20 - m.M02) / s;
                    return new Rot3f(w, x, y, z).Normalized;
                }
                else
                {
                    float s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                    float x = (m.M20 + m.M02) / s;
                    float y = (m.M12 + m.M21) / s;
                    float z = s / 4;
                    float w = (m.M01 - m.M10) / s;
                    return new Rot3f(w, x, y, z).Normalized;
                }
            }
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
            angleInRadians = 2 * (float)System.Math.Acos(W);
            var s = (float)System.Math.Sqrt(1 - W * W); // assuming quaternion normalised then w is less than 1, so term always positive.
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
    /// <summary>
    /// Represents an arbitrary rotation in three dimensions. Implemented as
    /// a normalized quaternion.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rot3d
    {
        [DataMember]
        public double W;
        [DataMember]
        public V3d V;

        #region Constructors

        /// <summary>
        /// Creates quaternion (w, (x, y, z)).
        /// </summary>
        // todo ISSUE 20090420 andi : sm, rft: Add asserts for unit-length constraint
        public Rot3d(double w, double x, double y, double z)
        {
            W = w;
            V = new V3d(x, y, z);
        }

        /// <summary>
        /// Creates quaternion (w, (v.x, v.y, v.z)).
        /// </summary>
        public Rot3d(double w, V3d v)
        {
            W = w;
            V = v;
        }

        /// <summary>
        /// Creates quaternion from array.
        /// (w = a[0], (x = a[1], y = a[2], z = a[3])).
        /// </summary>
        public Rot3d(double[] a)
        {
            W = a[0];
            V = new V3d(a[1], a[2], a[3]);
        }

        /// <summary>
        /// Creates quaternion from array starting at specified index.
        /// (w = a[start], (x = a[start+1], y = a[start+2], z = a[start+3])).
        /// </summary>
        public Rot3d(double[] a, int start)
        {
            W = a[start];
            V = new V3d(a[start + 1], a[start + 2], a[start + 3]);
        }

        /// <summary>
        /// Creates quaternion representing a rotation around
        /// an axis by an angle.
        /// </summary>
        // todo ISSUE 20090420 andi : sm, rft: What about adding an AxisAngle struct?.
        public Rot3d(V3d axis, double angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            W = halfAngle.Cos();
            V = axis.Normalized * halfAngle.Sin();
        }

        /// <summary>
        /// Creates quaternion from euler angles [yaw, pitch, roll].
        /// </summary>
        /// <param name="yawInRadians">Rotation around X</param>
        /// <param name="pitchInRadians">Rotation around Y</param>
        /// <param name="rollInRadians">Rotation around Z</param>
        public Rot3d(double yawInRadians, double pitchInRadians, double rollInRadians)
        {
            var qx = new Rot3d(V3d.XAxis, yawInRadians);
            var qy = new Rot3d(V3d.YAxis, pitchInRadians);
            var qz = new Rot3d(V3d.ZAxis, rollInRadians);
            this = qz * qy * qx;
        }

        /// <summary>
        /// Creates a quaternion representing a rotation from one
        /// vector into another.
        /// </summary>
        public Rot3d(V3d from, V3d into)
        {
            var a = from.Normalized;
            var b = into.Normalized;
            var angle = Fun.Clamp(V3d.Dot(a, b), -1, 1).Acos();
            var angleAbs = angle.Abs();
            V3d axis;

            // some vectors do not normalize to 1.0 -> Vec.Dot = -0.99999999999999989 || -0.99999994f
            // acos => 3.1415926386886319 or 3.14124632f -> delta of 1e-7 or 1e-3
            if (angle < 1e-7)
            {
                axis = a;
                angle = 0;
            }
            else if (Constant.Pi - angleAbs < 1e-7)
            {
                axis = a.AxisAlignedNormal();
                angle = Constant.Pi;
            }
            else
                axis = V3d.Cross(a, b).Normalized;

            this = new Rot3d(axis, angle);
        }

        #endregion

        #region Properties

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return V.X;
                    case 1: return V.Y;
                    case 2: return V.Z;
                    case 3: return W;
                    default: throw new ArgumentException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0: V.X = value; break;
                    case 1: V.Y = value; break;
                    case 2: V.Z = value; break;
                    case 3: W = value; break;
                    default: throw new ArgumentException();
                }
            }
        }

        public double X
        {
            get { return V.X; }
            set { V.X = value; }
        }

        public double Y
        {
            get { return V.Y; }
            set { V.Y = value; }
        }

        public double Z
        {
            get { return V.Z; }
            set { V.Z = value; }
        }

        #endregion

        #region Constants
#if false
        /// NOTE for developers
        /// A Zero-quaternion does not represent a Rot3, so it should not be implemented
        /// <summary>        /// Zero (0,(0,0,0)).
        /// </summary>
        public static readonly Rot3d Zero = new Rot3d(0, V3d.Zero);
#endif

        /// <summary>
        /// Identity (1,(0,0,0)).
        /// </summary>
        public static readonly Rot3d Identity = new Rot3d(1, V3d.Zero);

        /// <summary>
        /// X-Axis (0, (1,0,0)).
        /// </summary>
        public static readonly Rot3d XAxis = new Rot3d(0, V3d.XAxis);

        /// <summary>
        /// Y-Axis (0, (0,1,0)).
        /// </summary>
        public static readonly Rot3d YAxis = new Rot3d(0, V3d.YAxis);

        /// <summary>
        /// Z-Axis (0, (0,0,1)).
        /// </summary>
        public static readonly Rot3d ZAxis = new Rot3d(0, V3d.ZAxis);

        #endregion

        #region Quaternion Arithmetics

        // [todo ISSUE 20090421 andi : andi>
        // Operations like Add, Subtract and Multiplication with scalar, Divide, Reciprocal
        // should not be defined in a Rot3*.
        // These are perfectly valid for a quaternion, but a rotation is defined on a 
        // NORMALIZED quaternion. This Norm-Constraint would be violated with above operations.
        // <]
        // todo andi {
        /// <summary>
        /// Returns the sum of 2 quaternions (a.w + b.w, a.v + b.v).
        /// </summary>
        public static Rot3d Add(Rot3d a, Rot3d b)
        {
            return new Rot3d(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        public static Rot3d Add(Rot3d q, double s)
        {
            return new Rot3d(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (q.w - s, (q.x - s, q.y - s, q.z - s)).
        /// </summary>
        public static Rot3d Subtract(Rot3d q, double s)
        {
            return Add(q, -s);
        }

        /// <summary>
        /// Returns (s - q.w, (s - q.x, s- q.y, s- q.z)).
        /// </summary>
        public static Rot3d Subtract(double s, Rot3d q)
        {
            return Add(-q, s);
        }

        /// <summary>
        /// Returns (a.w - b.w, a.v - b.v).
        /// </summary>
        public static Rot3d Subtract(Rot3d a, Rot3d b)
        {
            return Add(a, -b);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        public static Rot3d Multiply(Rot3d q, double s)
        {
            return new Rot3d(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }
        // todo andi }

        /// <summary>
        /// Multiplies 2 quaternions.
        /// This concatenates the two rotations into a single one.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static Rot3d Multiply(Rot3d a, Rot3d b)
        {
            return new Rot3d(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
                );
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by quaternion q.
        /// </summary>
        public static V3d TransformDir(Rot3d q, V3d v)
        {
            // first transforming quaternion to M33d is approximately equal in terms of operations ...
            return ((M33d)q).Transform(v);

            // ... than direct multiplication ...
            //QuaternionF r = q.Conjugated() * new QuaternionF(0, v) * q;
            //return new V3d(r.X, r.Y, r.Z);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by quaternion q.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public static V3d TransformPos(Rot3d q, V3d p)
        {
            return TransformDir(q, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of quaternion q.
        /// </summary>
        public static V3d InvTransformDir(Rot3d q, V3d v)
        {
            //for Rotation Matrices R^-1 == R^T:
            return ((M33d)q).TransposedTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the incerse of quaternion q.
        /// For quaternions, this method is equivalent to InvTransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public static V3d InvTransformPos(Rot3d q, V3d p)
        {
            return InvTransformDir(q, p);
        }



        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this quaternion.
        /// </summary>
        public V3d TransformDir(V3d v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this quaternion.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public V3d TransformPos(V3d p)
        {
            return TransformDir(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this quaternion.
        /// </summary>
        public V3d InvTransformDir(V3d v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this quaternion.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public V3d InvTransformPos(V3d p)
        {
            return InvTransformDir(this, p);
        }

        // todo andi {
        public static Rot3d Divide(double s, Rot3d q)
        {
            return new Rot3d(
                s / q.W,
                s / q.X, s / q.Y, s / q.Z
                );
        }

        /// <summary>
        /// Returns (q.w / s, q.v * (1/s)).
        /// </summary>
        public static Rot3d Divide(Rot3d q, double s)
        {
            return Multiply(q, 1 / s);
        }

        /// <summary>
        /// Divides 2 quaternions.
        /// </summary>
        public static Rot3d Divide(Rot3d a, Rot3d b)
        {
            return Multiply(a, Reciprocal(b));
        }
        // todo andi }

        /// <summary>
        /// Returns the component-wise negation (-q.w, -q.v) of quaternion q.
        /// This represents the same rotation.
        /// </summary>
        public static Rot3d Negated(Rot3d q)
        {
            return new Rot3d(-q.W, -q.X, -q.Y, -q.Z);
        }

        // todo andi {
        /// <summary>
        /// Returns the component-wise reciprocal (1/q.w, 1/q.x, 1/q.y, 1/q.z)
        /// of quaternion q.
        /// </summary>
        public static Rot3d Reciprocal(Rot3d q)
        {
            return new Rot3d(1 / q.W, 1 / q.X, 1 / q.Y, 1 / q.Z);
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/w, 1/x, 1/y, 1/z).
        /// </summary>
        public Rot3d OneDividedBy()
        {
            return Reciprocal(this);
        }
        // todo andi }

        /// <summary> 
        /// Returns the dot-product of 2 quaternions.
        /// </summary>
        public static double Dot(Rot3d a, Rot3d b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        // [todo ISSUE 20090225 andi : andi> 
        // Investigate if we can use power to "scale" the rotation (=enlarging the angle of rotation by a factor.)
        // If so, reenable the Log and Exp methods if they are useful then.
        // <]
#if false
        /// <summary>
        /// Calculates the logarithm of the quaternion.
        /// </summary>
        public static Rot3d Log(Rot3d a)
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
        /// Calculates exponent of the quaternion.
        /// </summary>
        public Rot3d Exp(Rot3d a)
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
#endif

        /// <summary>
        /// Gets squared norm (or squared length) of this quaternion.
        /// </summary>
        public double NormSquared
        {
            get
            {
                return W * W + V.LengthSquared;
            }
        }

        /// <summary>
        /// Gets norm (or length) of this quaternion.
        /// </summary>
        public double Norm
        {
            get { return NormSquared.Sqrt(); }
        }

        /// <summary>
        /// Normalizes this quaternion.
        /// </summary>
        /// <returns>This.</returns>
        public void Normalize()
        {
            var norm = Norm;
            if (norm == 0)
                this = Rot3d.Identity;
            else
            {
                var scale = 1 / norm;
                W *= scale;
                V *= scale;
            }
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this quaternion.
        /// </summary>
        public Rot3d Normalized
        {
            get
            {
                var norm = Norm;
                if (norm == 0) return Rot3d.Identity;
                var scale = 1 / norm;
                return new Rot3d(W * scale, V * scale);
            }
        }

        /// <summary>
        /// Inverts this quaternion (multiplicative inverse).
        /// Returns this.
        /// </summary>
        public void Invert()
        {
            var norm = NormSquared;
            if (norm == 0) throw new ArithmeticException("quaternion is not invertible");
            var scale = 1 / norm;
            W *= scale;
            V *= -scale;
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this quaternion.
        /// </summary>
        public Rot3d Inverse
        {
            get
            {
                var norm = NormSquared;
                if (norm == 0) throw new ArithmeticException("quaternion is not invertible");
                var scale = 1 / norm;
                return new Rot3d(W * scale, V * (-scale));
            }
        }

        /// <summary>
        /// Conjugates this quaternion. Returns this.
        /// For normalized rotation-quaternions this is the same as Invert().
        /// </summary>
        public void Conjugate()
        {
            V = -V;
        }

        /// <summary>
        /// Gets the conjugate of this quaternion.
        /// For normalized rotation-quaternions this is the same as Inverted().
        /// </summary>
        public Rot3d Conjugated
        {
            get { return new Rot3d(W, -V); }
        }

        /// <summary>
        /// Returns the Euler-Angles from the quatarnion.
        /// </summary>
        public V3d GetEulerAngles()
        {
            var test = W * Y - X * Z;
            if (test > 0.49999999) // singularity at north pole
            {
                return new V3d(
                    2 * Fun.Atan2(X, W),
                    Constant.PiHalf,
                    0);
            }
            if (test < -0.49999999) // singularity at south pole
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

        public static bool ApproxEqual(Rot3d r0, Rot3d r1)
        {
            return ApproxEqual(r0, r1, Constant<double>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add Rot3d.ApproxEqual(Rot3d other);
        public static bool ApproxEqual(Rot3d r0, Rot3d r1, double tolerance)
        {
            return (r0.W - r1.W).Abs() <= tolerance &&
                   (r0.X - r1.X).Abs() <= tolerance &&
                   (r0.Y - r1.Y).Abs() <= tolerance &&
                   (r0.Z - r1.Z).Abs() <= tolerance;
        }

        #endregion

        #region Arithmetic Operators

        public static Rot3d operator -(Rot3d rot)
        {
            return Rot3d.Negated(rot);
        }

        // todo andi {
        public static Rot3d operator +(Rot3d a, Rot3d b)
        {
            return Rot3d.Add(a, b);
        }

        public static Rot3d operator +(Rot3d rot, double s)
        {
            return Rot3d.Add(rot, s);
        }

        public static Rot3d operator +(double s, Rot3d rot)
        {
            return Rot3d.Add(rot, s);
        }

        public static Rot3d operator -(Rot3d a, Rot3d b)
        {
            return Rot3d.Subtract(a, b);
        }

        public static Rot3d operator -(Rot3d rot, double s)
        {
            return Rot3d.Subtract(rot, s);
        }

        public static Rot3d operator -(double scalar, Rot3d rot)
        {
            return Rot3d.Subtract(scalar, rot);
        }

        public static Rot3d operator *(Rot3d rot, double s)
        {
            return Rot3d.Multiply(rot, s);
        }

        public static Rot3d operator *(double s, Rot3d rot)
        {
            return Rot3d.Multiply(rot, s);
        }
        // todo andi }

        public static Rot3d operator *(Rot3d a, Rot3d b)
        {
            return Rot3d.Multiply(a, b);
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
        /// <summary>
        /// </summary>
        public static M33d operator *(Rot3d rot, Scale3d m)
        {
            return (M33d)rot * m;
        }

        /// <summary>
        /// </summary>
        public static M34d operator *(Rot3d rot, Shift3d m)
        {
            return (M33d)rot * m;
        }

        public static M33d operator *(Rot3d rot, M33d m)
        {
            return (M33d)rot * m;
        }

        public static M33d operator *(M33d m, Rot3d rot)
        {
            return m * (M33d)rot;
        }

        // todo andi {
        public static Rot3d operator /(Rot3d rot, double s)
        {
            return Rot3d.Divide(rot, s);
        }

        public static Rot3d operator /(double s, Rot3d rot)
        {
            return Rot3d.Divide(s, rot);
        }
        // todo andi }

        #endregion

        #region Comparison Operators

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        public static bool operator ==(Rot3d r0, Rot3d r1)
        {
            return r0.W == r1.W && r0.V == r1.V;
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
            var t = 1 + m.M00 + m.M11 + m.M22;

            if (t > epsilon)
            {
                double s = t.Sqrt() * 2;
                double x = (m.M21 - m.M12) / s;
                double y = (m.M02 - m.M20) / s;
                double z = (m.M10 - m.M01) / s;
                double w = s / 4;
                return new Rot3d(w, x, y, z).Normalized;
            }
            else
            {
                if (m.M00 > m.M11 && m.M00 > m.M22)
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
                    double w = (m.M20 - m.M02) / s;
                    return new Rot3d(w, x, y, z).Normalized;
                }
                else
                {
                    double s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                    double x = (m.M20 + m.M02) / s;
                    double y = (m.M12 + m.M21) / s;
                    double z = s / 4;
                    double w = (m.M01 - m.M10) / s;
                    return new Rot3d(w, x, y, z).Normalized;
                }
            }
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
            angleInRadians = 2 * (double)System.Math.Acos(W);
            var s = (double)System.Math.Sqrt(1 - W * W); // assuming quaternion normalised then w is less than 1, so term always positive.
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
}

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

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
    //#   var rotIntoEps = isDouble ? "1e-7" : "1e-3f";
    //#   var eulerAnglesEps = isDouble ? "0.49999999" : "0.49999f";
    //#   var pi = isDouble ? "Constant.Pi" : "Constant.PiF";
    /// <summary>
    /// Represents an arbitrary rotation in three dimensions. Implemented as
    /// a normalized quaternion.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __rot3t__
    {
        [DataMember]
        public __ft__ W;
        [DataMember]
        public __v3t__ V;

        #region Constructors

        /// <summary>
        /// Creates quaternion (w, (x, y, z)).
        /// </summary>
        // todo ISSUE 20090420 andi : sm, rft: Add asserts for unit-length constraint
        public __rot3t__(__ft__ w, __ft__ x, __ft__ y, __ft__ z)
        {
            W = w;
            V = new __v3t__(x, y, z);
        }

        /// <summary>
        /// Creates quaternion (w, (v.x, v.y, v.z)).
        /// </summary>
        public __rot3t__(__ft__ w, __v3t__ v)
        {
            W = w;
            V = v;
        }

        /// <summary>
        /// Creates quaternion from array.
        /// (w = a[0], (x = a[1], y = a[2], z = a[3])).
        /// </summary>
        public __rot3t__(__ft__[] a)
        {
            W = a[0];
            V = new __v3t__(a[1], a[2], a[3]);
        }

        /// <summary>
        /// Creates quaternion from array starting at specified index.
        /// (w = a[start], (x = a[start+1], y = a[start+2], z = a[start+3])).
        /// </summary>
        public __rot3t__(__ft__[] a, int start)
        {
            W = a[start];
            V = new __v3t__(a[start + 1], a[start + 2], a[start + 3]);
        }

        /// <summary>
        /// Creates quaternion representing a rotation around
        /// an axis by an angle.
        /// </summary>
        // todo ISSUE 20090420 andi : sm, rft: What about adding an AxisAngle struct?.
        public __rot3t__(__v3t__ axis, __ft__ angleInRadians)
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
        public __rot3t__(__ft__ yawInRadians, __ft__ pitchInRadians, __ft__ rollInRadians)
        {
            var qx = new __rot3t__(__v3t__.XAxis, yawInRadians);
            var qy = new __rot3t__(__v3t__.YAxis, pitchInRadians);
            var qz = new __rot3t__(__v3t__.ZAxis, rollInRadians);
            this = qz * qy * qx;
        }

        /// <summary>
        /// Creates a quaternion representing a rotation from one
        /// vector into another.
        /// </summary>
        public __rot3t__(__v3t__ from, __v3t__ into)
        {
            var a = from.Normalized;
            var b = into.Normalized;
            var angle = Fun.Clamp(__v3t__.Dot(a, b), -1, 1).Acos();
            var angleAbs = angle.Abs();
            __v3t__ axis;

            // some vectors do not normalize to 1.0 -> Vec.Dot = -0.99999999999999989 || -0.99999994f
            // acos => 3.1415926386886319 or 3.14124632f -> delta of 1e-7 or 1e-3
            if (angle < __rotIntoEps__)
            {
                axis = a;
                angle = 0;
            }
            else if (__pi__ - angleAbs < __rotIntoEps__)
            {
                axis = a.AxisAlignedNormal();
                angle = __pi__;
            }
            else
                axis = __v3t__.Cross(a, b).Normalized;

            this = new __rot3t__(axis, angle);
        }

        #endregion

        #region Properties

        public __ft__ this[int index]
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

        public __ft__ X
        {
            get { return V.X; }
            set { V.X = value; }
        }

        public __ft__ Y
        {
            get { return V.Y; }
            set { V.Y = value; }
        }

        public __ft__ Z
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
        public static readonly __rot3t__ Zero = new __rot3t__(0, __v3t__.Zero);
#endif

        /// <summary>
        /// Identity (1,(0,0,0)).
        /// </summary>
        public static readonly __rot3t__ Identity = new __rot3t__(1, __v3t__.Zero);

        /// <summary>
        /// X-Axis (0, (1,0,0)).
        /// </summary>
        public static readonly __rot3t__ XAxis = new __rot3t__(0, __v3t__.XAxis);

        /// <summary>
        /// Y-Axis (0, (0,1,0)).
        /// </summary>
        public static readonly __rot3t__ YAxis = new __rot3t__(0, __v3t__.YAxis);

        /// <summary>
        /// Z-Axis (0, (0,0,1)).
        /// </summary>
        public static readonly __rot3t__ ZAxis = new __rot3t__(0, __v3t__.ZAxis);

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
        public static __rot3t__ Add(__rot3t__ a, __rot3t__ b)
        {
            return new __rot3t__(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Returns (q.w + s, (q.x + s, q.y + s, q.z + s)).
        /// </summary>
        public static __rot3t__ Add(__rot3t__ q, __ft__ s)
        {
            return new __rot3t__(q.W + s, q.X + s, q.Y + s, q.Z + s);
        }

        /// <summary>
        /// Returns (q.w - s, (q.x - s, q.y - s, q.z - s)).
        /// </summary>
        public static __rot3t__ Subtract(__rot3t__ q, __ft__ s)
        {
            return Add(q, -s);
        }

        /// <summary>
        /// Returns (s - q.w, (s - q.x, s- q.y, s- q.z)).
        /// </summary>
        public static __rot3t__ Subtract(__ft__ s, __rot3t__ q)
        {
            return Add(-q, s);
        }

        /// <summary>
        /// Returns (a.w - b.w, a.v - b.v).
        /// </summary>
        public static __rot3t__ Subtract(__rot3t__ a, __rot3t__ b)
        {
            return Add(a, -b);
        }

        /// <summary>
        /// Returns (q.w * s, q.v * s).
        /// </summary>
        public static __rot3t__ Multiply(__rot3t__ q, __ft__ s)
        {
            return new __rot3t__(q.W * s, q.X * s, q.Y * s, q.Z * s);
        }
        // todo andi }

        /// <summary>
        /// Multiplies 2 quaternions.
        /// This concatenates the two rotations into a single one.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static __rot3t__ Multiply(__rot3t__ a, __rot3t__ b)
        {
            return new __rot3t__(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
                );
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by quaternion q.
        /// </summary>
        public static __v3t__ TransformDir(__rot3t__ q, __v3t__ v)
        {
            // first transforming quaternion to __m33t__ is approximately equal in terms of operations ...
            return ((__m33t__)q).Transform(v);

            // ... than direct multiplication ...
            //QuaternionF r = q.Conjugated() * new QuaternionF(0, v) * q;
            //return new __v3t__(r.X, r.Y, r.Z);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by quaternion q.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public static __v3t__ TransformPos(__rot3t__ q, __v3t__ p)
        {
            return TransformDir(q, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of quaternion q.
        /// </summary>
        public static __v3t__ InvTransformDir(__rot3t__ q, __v3t__ v)
        {
            //for Rotation Matrices R^-1 == R^T:
            return ((__m33t__)q).TransposedTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the incerse of quaternion q.
        /// For quaternions, this method is equivalent to InvTransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public static __v3t__ InvTransformPos(__rot3t__ q, __v3t__ p)
        {
            return InvTransformDir(q, p);
        }



        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this quaternion.
        /// </summary>
        public __v3t__ TransformDir(__v3t__ v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this quaternion.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public __v3t__ TransformPos(__v3t__ p)
        {
            return TransformDir(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this quaternion.
        /// </summary>
        public __v3t__ InvTransformDir(__v3t__ v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this quaternion.
        /// For quaternions, this method is equivalent to TransformDir, and
        /// is made available only to provide a consistent set of operations
        /// for all transforms.
        /// </summary>
        public __v3t__ InvTransformPos(__v3t__ p)
        {
            return InvTransformDir(this, p);
        }

        // todo andi {
        public static __rot3t__ Divide(__ft__ s, __rot3t__ q)
        {
            return new __rot3t__(
                s / q.W,
                s / q.X, s / q.Y, s / q.Z
                );
        }

        /// <summary>
        /// Returns (q.w / s, q.v * (1/s)).
        /// </summary>
        public static __rot3t__ Divide(__rot3t__ q, __ft__ s)
        {
            return Multiply(q, 1 / s);
        }

        /// <summary>
        /// Divides 2 quaternions.
        /// </summary>
        public static __rot3t__ Divide(__rot3t__ a, __rot3t__ b)
        {
            return Multiply(a, Reciprocal(b));
        }
        // todo andi }

        /// <summary>
        /// Returns the component-wise negation (-q.w, -q.v) of quaternion q.
        /// This represents the same rotation.
        /// </summary>
        public static __rot3t__ Negated(__rot3t__ q)
        {
            return new __rot3t__(-q.W, -q.X, -q.Y, -q.Z);
        }

        // todo andi {
        /// <summary>
        /// Returns the component-wise reciprocal (1/q.w, 1/q.x, 1/q.y, 1/q.z)
        /// of quaternion q.
        /// </summary>
        public static __rot3t__ Reciprocal(__rot3t__ q)
        {
            return new __rot3t__(1 / q.W, 1 / q.X, 1 / q.Y, 1 / q.Z);
        }

        /// <summary>
        /// Returns the component-wise reciprocal (1/w, 1/x, 1/y, 1/z).
        /// </summary>
        public __rot3t__ OneDividedBy()
        {
            return Reciprocal(this);
        }
        // todo andi }

        /// <summary> 
        /// Returns the dot-product of 2 quaternions.
        /// </summary>
        public static __ft__ Dot(__rot3t__ a, __rot3t__ b)
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
        public static __rot3t__ Log(__rot3t__ a)
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
        /// Calculates exponent of the quaternion.
        /// </summary>
        public __rot3t__ Exp(__rot3t__ a)
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
#endif

        /// <summary>
        /// Gets squared norm (or squared length) of this quaternion.
        /// </summary>
        public __ft__ NormSquared
        {
            get
            {
                return W * W + V.LengthSquared;
            }
        }

        /// <summary>
        /// Gets norm (or length) of this quaternion.
        /// </summary>
        public __ft__ Norm
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
                this = __rot3t__.Identity;
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
        public __rot3t__ Normalized
        {
            get
            {
                var norm = Norm;
                if (norm == 0) return __rot3t__.Identity;
                var scale = 1 / norm;
                return new __rot3t__(W * scale, V * scale);
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
        public __rot3t__ Inverse
        {
            get
            {
                var norm = NormSquared;
                if (norm == 0) throw new ArithmeticException("quaternion is not invertible");
                var scale = 1 / norm;
                return new __rot3t__(W * scale, V * (-scale));
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
        public __rot3t__ Conjugated
        {
            get { return new __rot3t__(W, -V); }
        }

        /// <summary>
        /// Returns the Euler-Angles from the quatarnion.
        /// </summary>
        public V3d GetEulerAngles()
<<<<<<< HEAD:src/Aardvark.Base/Math/Trafos/Rot3_template.cs
        {
            var test = W * Y - X * Z;
            if (test > 0.49999) // singularity at north pole
            {
                return new V3d(
                    -2 * Fun.Atan2(X, W),
                    Constant.PiHalf,
                    0);
            }
            if (test < -0.49999) // singularity at south pole
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
                        Fun.AsinC(2 * test),
                        Fun.Atan2(2 * (W * Z + X * Y),
=======
        {
            var test = W * Y - X * Z;
            if (test > __eulerAnglesEps__) // singularity at north pole
            {
                return new V3d(
                    2 * Fun.Atan2(X, W),
                    Constant.PiHalf,
                    0);
            }
            if (test < -__eulerAnglesEps__) // singularity at south pole
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
                        Fun.AsinC(2 * test),
                        Fun.Atan2(2 * (W * Z + X * Y),
>>>>>>> remotes/origin/master:src/Aardvark.Base/Trafos/Rot3_template.cs
                                  1 - 2 * (Y * Y + Z * Z)));
        }

        public static bool ApproxEqual(__rot3t__ r0, __rot3t__ r1)
        {
            return ApproxEqual(r0, r1, Constant<__ft__>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add __rot3t__.ApproxEqual(__rot3t__ other);
        public static bool ApproxEqual(__rot3t__ r0, __rot3t__ r1, __ft__ tolerance)
        {
            return (r0.W - r1.W).Abs() <= tolerance &&
                   (r0.X - r1.X).Abs() <= tolerance &&
                   (r0.Y - r1.Y).Abs() <= tolerance &&
                   (r0.Z - r1.Z).Abs() <= tolerance;
        }

        #endregion

        #region Arithmetic Operators

        public static __rot3t__ operator -(__rot3t__ rot)
        {
            return __rot3t__.Negated(rot);
        }

        // todo andi {
        public static __rot3t__ operator +(__rot3t__ a, __rot3t__ b)
        {
            return __rot3t__.Add(a, b);
        }

        public static __rot3t__ operator +(__rot3t__ rot, __ft__ s)
        {
            return __rot3t__.Add(rot, s);
        }

        public static __rot3t__ operator +(__ft__ s, __rot3t__ rot)
        {
            return __rot3t__.Add(rot, s);
        }

        public static __rot3t__ operator -(__rot3t__ a, __rot3t__ b)
        {
            return __rot3t__.Subtract(a, b);
        }

        public static __rot3t__ operator -(__rot3t__ rot, __ft__ s)
        {
            return __rot3t__.Subtract(rot, s);
        }

        public static __rot3t__ operator -(__ft__ scalar, __rot3t__ rot)
        {
            return __rot3t__.Subtract(scalar, rot);
        }

        public static __rot3t__ operator *(__rot3t__ rot, __ft__ s)
        {
            return __rot3t__.Multiply(rot, s);
        }

        public static __rot3t__ operator *(__ft__ s, __rot3t__ rot)
        {
            return __rot3t__.Multiply(rot, s);
        }
        // todo andi }

        public static __rot3t__ operator *(__rot3t__ a, __rot3t__ b)
        {
            return __rot3t__.Multiply(a, b);
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
        /// <summary>
        /// </summary>
        public static __m33t__ operator *(__rot3t__ rot, __scale3t__ m)
        {
            return (__m33t__)rot * m;
        }

        /// <summary>
        /// </summary>
        public static __m34t__ operator *(__rot3t__ rot, __shift3t__ m)
        {
            return (__m33t__)rot * m;
        }

        public static __m33t__ operator *(__rot3t__ rot, __m33t__ m)
        {
            return (__m33t__)rot * m;
        }

        public static __m33t__ operator *(__m33t__ m, __rot3t__ rot)
        {
            return m * (__m33t__)rot;
        }

        // todo andi {
        public static __rot3t__ operator /(__rot3t__ rot, __ft__ s)
        {
            return __rot3t__.Divide(rot, s);
        }

        public static __rot3t__ operator /(__ft__ s, __rot3t__ rot)
        {
            return __rot3t__.Divide(s, rot);
        }
        // todo andi }

        #endregion

        #region Comparison Operators

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes beruecksichtigen -q == q, weil es die selbe rotation definiert.
        public static bool operator ==(__rot3t__ r0, __rot3t__ r1)
        {
            return r0.W == r1.W && r0.V == r1.V;
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
            var t = 1 + m.M00 + m.M11 + m.M22;

            if (t > epsilon)
            {
                __ft__ s = t.Sqrt() * 2;
                __ft__ x = (m.M21 - m.M12) / s;
                __ft__ y = (m.M02 - m.M20) / s;
                __ft__ z = (m.M10 - m.M01) / s;
                __ft__ w = s / 4;
                return new __rot3t__(w, x, y, z).Normalized;
            }
            else
            {
                if (m.M00 > m.M11 && m.M00 > m.M22)
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
                    __ft__ w = (m.M20 - m.M02) / s;
                    return new __rot3t__(w, x, y, z).Normalized;
                }
                else
                {
                    __ft__ s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                    __ft__ x = (m.M20 + m.M02) / s;
                    __ft__ y = (m.M12 + m.M21) / s;
                    __ft__ z = s / 4;
                    __ft__ w = (m.M01 - m.M10) / s;
                    return new __rot3t__(w, x, y, z).Normalized;
                }
            }
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
            angleInRadians = 2 * (__ft__)System.Math.Acos(W);
            var s = (__ft__)System.Math.Sqrt(1 - W * W); // assuming quaternion normalised then w is less than 1, so term always positive.
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
    //# } // isDouble
}

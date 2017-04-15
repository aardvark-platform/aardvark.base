using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

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
    [StructLayout(LayoutKind.Sequential)]
    public struct __rot2t__
    {
        public __ft__ Angle;

        #region Constructors

        public __rot2t__(__ft__ angleInRadians)
        {
            Angle = angleInRadians;
        }

        #endregion

        #region Constants

        public static readonly __rot2t__ Identity = new __rot2t__(0);

        #endregion

        #region Rotation Arithmetics

        public void Invert()
        {
            Angle = -Angle;
        }

        public __rot2t__ Inverse
        {
            get
            {
                return new __rot2t__(-Angle);
            }
        }

        /// <summary>
        /// Adds 2 rotations.
        /// </summary>
        public static __rot2t__ Add(__rot2t__ r0, __rot2t__ r1)
        {
            return new __rot2t__(r0.Angle + r1.Angle);
        }

        /// <summary>
        /// Adds scalar to a rotation.
        /// </summary>
        public static __rot2t__ Add(__rot2t__ rot, __ft__ val)
        {
            return new __rot2t__(rot.Angle + val);
        }

        /// <summary>
        /// Subtracts 2 rotations.
        /// </summary>
        public static __rot2t__ Subtract(__rot2t__ r0, __rot2t__ r1)
        {
            return new __rot2t__(r0.Angle - r1.Angle);
        }

        /// <summary>
        /// Subtracts scalar from a rotation.
        /// </summary>
        public static __rot2t__ Subtract(__rot2t__ rot, __ft__ angle)
        {
            return new __rot2t__(rot.Angle - angle);
        }

        /// <summary>
        /// Subtracts rotation from a scalar.
        /// </summary>
        public static __rot2t__ Subtract(__ft__ angle, __rot2t__ rot)
        {
            return new __rot2t__(angle - rot.Angle);
        }

        /// <summary>
        /// Multiplies scalar with a rotation.
        /// </summary>
        public static __rot2t__ Multiply(__rot2t__ rot, __ft__ val)
        {
            return new __rot2t__(rot.Angle * val);
        }

        public static __v2t__ Multiply(__rot2t__ rot, __v2t__ vec)
        {

            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new __v2t__(a * vec.X + b * vec.Y,
                              -b * vec.X + a * vec.Y);
        }

        public static __v3t__ Multiply(__rot2t__ rot, __v3t__ vec)
        {
            __ft__ ca = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ sa = (__ft__)System.Math.Sin(rot.Angle);

            return new __v3t__(ca * vec.X + sa * vec.Y,
                           -sa * vec.X + ca * vec.Y,
                           vec.Z);
        }

        public static __v4t__ Multiply(__rot2t__ rot, __v4t__ vec)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new __v4t__(a * vec.X +
                            b * vec.Y,

                           -b * vec.X +
                            a * vec.Y,

                            vec.Z,

                            vec.W);
        }

        public static __m22t__ Multiply(__rot2t__ rot, __m22t__ mat)
        {
            return (__m22t__)rot * mat;
        }

        public static __m33t__ Multiply(__rot2t__ rot, __m33t__ mat)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new __m33t__(a * mat.M00 +
                             b * mat.M10,

                             a * mat.M01 +
                             b * mat.M11,

                             a * mat.M02 +
                             b * mat.M12,

                            -b * mat.M00 +
                             a * mat.M10,

                            -b * mat.M01 +
                             a * mat.M11,

                            -b * mat.M02 +
                             a * mat.M12,

                             mat.M20,

                             mat.M21,

                             mat.M22);
        }

        public static __m34t__ Multiply(__rot2t__ rot, __m34t__ mat)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new __m34t__(a * mat.M00 +
                             b * mat.M10,

                             a * mat.M01 +
                             b * mat.M11,

                             a * mat.M02 +
                             b * mat.M12,

                             a * mat.M03 +
                             b * mat.M13,

                            -b * mat.M00 +
                             a * mat.M10,

                            -b * mat.M01 +
                             a * mat.M11,

                            -b * mat.M02 +
                             a * mat.M12,

                            -b * mat.M03 +
                             a * mat.M13,

                             mat.M20,

                             mat.M21,

                             mat.M22,

                             mat.M23);
        }

        public static __m44t__ Multiply(__rot2t__ rot, __m44t__ mat)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new __m44t__(a * mat.M00 +
                             b * mat.M10,

                             a * mat.M01 +
                             b * mat.M11,

                             a * mat.M02 +
                             b * mat.M12,

                             a * mat.M03 +
                             b * mat.M13,

                            -b * mat.M00 +
                             a * mat.M10,

                            -b * mat.M01 +
                             a * mat.M11,

                            -b * mat.M02 +
                             a * mat.M12,

                            -b * mat.M03 +
                             a * mat.M13,

                             mat.M20,

                             mat.M21,

                             mat.M22,

                             mat.M23,

                             mat.M30,

                             mat.M31,

                             mat.M32,

                             mat.M33);
        }

        public static __m33t__ Multiply(__rot2t__ rot2, __rot3t__ rot3)
        {
            return __rot2t__.Multiply(rot2, (__m33t__)rot3);
        }

        /// <summary>
        /// Multiplies 2 rotations.
        /// </summary>
        public static __rot2t__ Multiply(__rot2t__ r0, __rot2t__ r2)
        {
            return new __rot2t__(r0.Angle * r2.Angle);
        }

        public static __m33t__ Multiply(__rot2t__ rot, __scale3t__ scale)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new __m33t__(a * scale.X,
                             b * scale.Y,
                             0,

                            -b * scale.X,
                             a * scale.Y,
                             0,

                             0,
                             0,
                             scale.Z);
        }

        public static __m34t__ Multiply(__rot2t__ rot, __shift3t__ shift)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new __m34t__(a, b, 0, a * shift.X + b * shift.Y,
                            -b, a, 0, -b * shift.X + a * shift.Y,
                             0, 0, 1, shift.Z);

        }

        /// <summary>
        /// Divides scalar by a rotation.
        /// </summary>
        public static __rot2t__ Divide(__rot2t__ rot, __ft__ val)
        {
            return new __rot2t__(rot.Angle / val);
        }

        /// <summary>
        /// Negates rotation.
        /// </summary>
        public static __rot2t__ Negate(__rot2t__ rot)
        {
            return new __rot2t__(-rot.Angle);
        }

        /// <summary>
        /// Transforms a direction vector.
        /// </summary>
        public static __v2t__ TransformDir(__rot2t__ rot, __v2t__ v)
        {
            return (__m22t__)rot * v;
        }

        /// <summary>
        /// Inverse transforms a direction vector.
        /// </summary>
        public static __v2t__ InvTransformDir(__rot2t__ rot, __v2t__ v)
        {
            return (__m22t__)__rot2t__.Negate(rot) * v;
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public static __v2t__ TransformPos(__rot2t__ rot, __v2t__ v)
        {
            return (__m22t__)rot * v;
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public static __v2t__ InvTransformPos(__rot2t__ rot, __v2t__ v)
        {
            return (__m22t__)__rot2t__.Negate(rot) * v;
        }

        /// <summary>
        /// Transforms a direction vector.
        /// </summary>
        public __v2t__ TransformDir(__v2t__ v)
        {
            return __rot2t__.TransformDir(this, v);
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public __v2t__ TransformPos(__v2t__ v)
        {
            return __rot2t__.TransformPos(this, v);
        }

        /// <summary>
        /// Transforms a direction vector.
        /// </summary>
        public __v2t__ InvTransformDir(__v2t__ v)
        {
            return __rot2t__.InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public __v2t__ InvTransformPos(__v2t__ v)
        {
            return __rot2t__.InvTransformPos(this, v);
        }

        #endregion

        #region Arithmetic operators

        /// <summary>
        /// Negates the rotation.
        /// </summary>
        public static __rot2t__ operator -(__rot2t__ rot)
        {
            return __rot2t__.Negate(rot);
        }

        /// <summary>
        /// Adds 2 rotations.
        /// </summary>
        public static __rot2t__ operator +(__rot2t__ r0, __rot2t__ r1)
        {
            return __rot2t__.Add(r0, r1);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static __rot2t__ operator +(__rot2t__ rot, __ft__ angle)
        {
            return __rot2t__.Add(rot, angle);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static __rot2t__ operator +(__ft__ angle, __rot2t__ rot)
        {
            return __rot2t__.Add(rot, angle);
        }

        /// <summary>
        /// Subtracts 2 rotations.
        /// </summary>
        public static __rot2t__ operator -(__rot2t__ r0, __rot2t__ r1)
        {
            return __rot2t__.Subtract(r0, r1);
        }

        /// <summary>
        /// Subtracts a rotation from a scalar value.
        /// </summary>
        public static __rot2t__ operator -(__rot2t__ rot, __ft__ angle)
        {
            return __rot2t__.Subtract(rot, angle);
        }

        /// <summary>
        /// Subtracts a rotation from a scalar value.
        /// </summary>
        public static __rot2t__ operator -(__ft__ angle, __rot2t__ rot)
        {
            return __rot2t__.Subtract(angle, rot);
        }

        /// <summary>
        /// Multiplies rotation with scalar value.
        /// </summary>
        public static __rot2t__ operator *(__rot2t__ rot, __ft__ val)
        {
            return __rot2t__.Multiply(rot, val);
        }

        /// <summary>
        /// Multiplies rotation with scalar value.
        /// </summary>
        public static __rot2t__ operator *(__ft__ val, __rot2t__ rot)
        {
            return __rot2t__.Multiply(rot, val);
        }

        /// <summary>
        /// </summary>
        public static __v2t__ operator *(__rot2t__ rot, __v2t__ vec)
        {
            return __rot2t__.Multiply(rot, vec);
        }

        /// <summary>
        /// </summary>
        public static __v3t__ operator *(__rot2t__ rot, __v3t__ vec)
        {
            return __rot2t__.Multiply(rot, vec);
        }

        /// <summary>
        /// </summary>
        public static __v4t__ operator *(__rot2t__ rot, __v4t__ vec)
        {
            return __rot2t__.Multiply(rot, vec);
        }

        public static __m22t__ operator *(__rot2t__ rot, __m22t__ mat)
        {
            return __rot2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static __m33t__ operator *(__rot2t__ rot, __m33t__ mat)
        {
            return __rot2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static __m34t__ operator *(__rot2t__ rot, __m34t__ mat)
        {
            return __rot2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static __m44t__ operator *(__rot2t__ rot, __m44t__ mat)
        {
            return __rot2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static __m33t__ operator *(__rot2t__ rot2, __rot3t__ rot3)
        {
            return __rot2t__.Multiply(rot2, rot3);
        }

        /// <summary>
        /// </summary>
        public static __rot2t__ operator *(__rot2t__ r0, __rot2t__ r1)
        {
            return __rot2t__.Multiply(r0, r1);
        }

        /// <summary>
        /// </summary>
        public static __m33t__ operator *(__rot2t__ rot, __scale3t__ scale)
        {
            return __rot2t__.Multiply(rot, scale);
        }

        /// <summary>
        /// </summary>
        public static __m34t__ operator *(__rot2t__ rot, __shift3t__ shift)
        {
            return __rot2t__.Multiply(rot, shift);
        }

        /// <summary>
        /// Divides rotation by scalar value.
        /// </summary>
        public static __rot2t__ operator /(__rot2t__ rot, __ft__ val)
        {
            return __rot2t__.Divide(rot, val);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if 2 rotations are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator ==(__rot2t__ rotation1, __rot2t__ rotation2)
        {
            return (rotation1.Angle == rotation2.Angle);
        }

        /// <summary>
        /// Checks if 2 rotations are not equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator !=(__rot2t__ rotation1, __rot2t__ rotation2)
        {
            return !(rotation1.Angle == rotation2.Angle);
        }


        public static bool ApproxEqual(__rot2t__ r0, __rot2t__ r1)
        {
            return ApproxEqual(r0, r1, Constant<__ft__>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes ber�cksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add __rot3t__.ApproxEqual(__rot3t__ other);
        public static bool ApproxEqual(__rot2t__ r0, __rot2t__ r1, __ft__ tolerance)
        {
            return (r0.Angle - r1.Angle).Abs() <= tolerance;
        }

        #endregion

        #region Creator Function

        //WARNING: untested

        public static __rot2t__ From__m22t__(__m22t__ m)
        {
            // cos(a) sin(a)
            //-sin(a) cos(a)

            if (m.M00 >= -1.0 && m.M00 <= 1.0)
            {
                return new __rot2t__((__ft__)System.Math.Acos(m.M00));
            }
            else throw new ArgumentException("Given __m22t__ is not a Rotation-Matrix");
        }

        #endregion

        #region Conversion Operators

        public static explicit operator __m22t__(__rot2t__ r)
        {
            var ca = (__ft__)System.Math.Cos(r.Angle);
            var sa = (__ft__)System.Math.Sin(r.Angle);

            return new __m22t__(ca, sa, -sa, ca);
        }

        public static explicit operator __m23t__(__rot2t__ r)
        {
            var ca = (__ft__)System.Math.Cos(r.Angle);
            var sa = (__ft__)System.Math.Sin(r.Angle);

            return new __m23t__(ca, sa, 0.0f, -sa, ca, 0.0f);
        }

        public static explicit operator __m33t__(__rot2t__ r)
        {
            var ca = (__ft__)System.Math.Cos(r.Angle);
            var sa = (__ft__)System.Math.Sin(r.Angle);

            return new __m33t__(ca, sa, 0,
                            -sa, ca, 0,
                            0, 0, 1);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Calculates Hash-code of the given rotation.
        /// </summary>
        /// <returns>Hash-code.</returns>
        public override int GetHashCode()
        {
            return Angle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}]", Angle);
        }

        public static __rot2t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __rot2t__(
                __ft__.Parse(x[0], CultureInfo.InvariantCulture)
            );
        }

        /// <summary>
        /// Checks if 2 objects are equal.
        /// </summary>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is __rot2t__)
            {
                __rot2t__ rotation = (__rot2t__)obj;
                return (Angle == rotation.Angle);
            }
            return false;
        }

        #endregion
    }
    //# } // isDouble
}

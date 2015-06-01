using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    [StructLayout(LayoutKind.Sequential)]
    public struct Rot__x2t__
    {
        public __ft__ Angle;

        #region Constructors

        public Rot__x2t__(__ft__ angleInRadians)
        {
            Angle = angleInRadians;
        }

        #endregion

        #region Constants

        public static readonly Rot__x2t__ Identity = new Rot__x2t__(0);

        #endregion

        #region Rotation Arithmetics

        public void Invert()
        {
            Angle = -Angle;
        }

        public Rot__x2t__ Inverse
        {
            get
            {
                return new Rot__x2t__(-Angle);
            }
        }

        /// <summary>
        /// Adds 2 rotations.
        /// </summary>
        public static Rot__x2t__ Add(Rot__x2t__ r0, Rot__x2t__ r1)
        {
            return new Rot__x2t__(r0.Angle + r1.Angle);
        }

        /// <summary>
        /// Adds scalar to a rotation.
        /// </summary>
        public static Rot__x2t__ Add(Rot__x2t__ rot, __ft__ val)
        {
            return new Rot__x2t__(rot.Angle + val);
        }

        /// <summary>
        /// Subtracts 2 rotations.
        /// </summary>
        public static Rot__x2t__ Subtract(Rot__x2t__ r0, Rot__x2t__ r1)
        {
            return new Rot__x2t__(r0.Angle - r1.Angle);
        }

        /// <summary>
        /// Subtracts scalar from a rotation.
        /// </summary>
        public static Rot__x2t__ Subtract(Rot__x2t__ rot, __ft__ angle)
        {
            return new Rot__x2t__(rot.Angle - angle);
        }

        /// <summary>
        /// Subtracts rotation from a scalar.
        /// </summary>
        public static Rot__x2t__ Subtract(__ft__ angle, Rot__x2t__ rot)
        {
            return new Rot__x2t__(angle - rot.Angle);
        }

        /// <summary>
        /// Multiplies scalar with a rotation.
        /// </summary>
        public static Rot__x2t__ Multiply(Rot__x2t__ rot, __ft__ val)
        {
            return new Rot__x2t__(rot.Angle * val);
        }

        public static V__x2t__ Multiply(Rot__x2t__ rot, V__x2t__ vec)
        {

            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new V__x2t__(a * vec.X +
                            b * vec.Y,

                           -b * vec.X +
                            a * vec.Y);
        }

        public static V__x3t__ Multiply(Rot__x2t__ rot, V__x3t__ vec)
        {
            __ft__ ca = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ sa = (__ft__)System.Math.Sin(rot.Angle);

            return new V__x3t__(ca * vec.X + sa * vec.Y,
                           -sa * vec.X + ca * vec.Y,
                           vec.Z);
        }

        public static V__x4t__ Multiply(Rot__x2t__ rot, V__x4t__ vec)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new V__x4t__(a * vec.X +
                            b * vec.Y,

                           -b * vec.X +
                            a * vec.Y,

                            vec.Z,

                            vec.W);
        }

        public static M2__x2t__ Multiply(Rot__x2t__ rot, M2__x2t__ mat)
        {
            return (M2__x2t__)rot * mat;
        }

        public static M3__x3t__ Multiply(Rot__x2t__ rot, M3__x3t__ mat)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new M3__x3t__(a * mat.M00 +
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

        public static M3__x4t__ Multiply(Rot__x2t__ rot, M3__x4t__ mat)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new M3__x4t__(a * mat.M00 +
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

        public static M4__x4t__ Multiply(Rot__x2t__ rot, M4__x4t__ mat)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new M4__x4t__(a * mat.M00 +
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

        public static M3__x3t__ Multiply(Rot__x2t__ rot2, Rot__x3t__ rot3)
        {
            return Rot__x2t__.Multiply(rot2, (M3__x3t__)rot3);
        }

        /// <summary>
        /// Multiplies 2 rotations.
        /// </summary>
        public static Rot__x2t__ Multiply(Rot__x2t__ r0, Rot__x2t__ r2)
        {
            return new Rot__x2t__(r0.Angle * r2.Angle);
        }

        public static M3__x3t__ Multiply(Rot__x2t__ rot, Scale__x3t__ scale)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new M3__x3t__(a * scale.X,
                             b * scale.Y,
                             0,

                            -b * scale.X,
                             a * scale.Y,
                             0,

                             0,
                             0,
                             scale.Z);
        }

        public static M3__x4t__ Multiply(Rot__x2t__ rot, Shift__x3t__ shift)
        {
            __ft__ a = (__ft__)System.Math.Cos(rot.Angle);
            __ft__ b = (__ft__)System.Math.Sin(rot.Angle);

            return new M3__x4t__(a, b, 0, a * shift.X + b * shift.Y,
                            -b, a, 0, -b * shift.X + a * shift.Y,
                             0, 0, 1, shift.Z);

        }

        /// <summary>
        /// Divides scalar by a rotation.
        /// </summary>
        public static Rot__x2t__ Divide(Rot__x2t__ rot, __ft__ val)
        {
            return new Rot__x2t__(rot.Angle / val);
        }

        /// <summary>
        /// Negates rotation.
        /// </summary>
        public static Rot__x2t__ Negate(Rot__x2t__ rot)
        {
            return new Rot__x2t__(-rot.Angle);
        }

        /// <summary>
        /// Transforms a direction vector.
        /// </summary>
        public static V__x2t__ TransformDir(Rot__x2t__ rot, V__x2t__ v)
        {
            return (M2__x2t__)rot * v;
        }

        /// <summary>
        /// Inverse transforms a direction vector.
        /// </summary>
        public static V__x2t__ InvTransformDir(Rot__x2t__ rot, V__x2t__ v)
        {
            return (M2__x2t__)Rot__x2t__.Negate(rot) * v;
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public static V__x2t__ TransformPos(Rot__x2t__ rot, V__x2t__ v)
        {
            return (M2__x2t__)rot * v;
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public static V__x2t__ InvTransformPos(Rot__x2t__ rot, V__x2t__ v)
        {
            return (M2__x2t__)Rot__x2t__.Negate(rot) * v;
        }

        /// <summary>
        /// Transforms a direction vector.
        /// </summary>
        public V__x2t__ TransformDir(V__x2t__ v)
        {
            return Rot__x2t__.TransformDir(this, v);
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public V__x2t__ TransformPos(V__x2t__ v)
        {
            return Rot__x2t__.TransformPos(this, v);
        }

        /// <summary>
        /// Transforms a direction vector.
        /// </summary>
        public V__x2t__ InvTransformDir(V__x2t__ v)
        {
            return Rot__x2t__.InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms a position vector.
        /// </summary>
        public V__x2t__ InvTransformPos(V__x2t__ v)
        {
            return Rot__x2t__.InvTransformPos(this, v);
        }

        #endregion

        #region Arithmetic operators

        /// <summary>
        /// Negates the rotation.
        /// </summary>
        public static Rot__x2t__ operator -(Rot__x2t__ rot)
        {
            return Rot__x2t__.Negate(rot);
        }

        /// <summary>
        /// Adds 2 rotations.
        /// </summary>
        public static Rot__x2t__ operator +(Rot__x2t__ r0, Rot__x2t__ r1)
        {
            return Rot__x2t__.Add(r0, r1);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static Rot__x2t__ operator +(Rot__x2t__ rot, __ft__ angle)
        {
            return Rot__x2t__.Add(rot, angle);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static Rot__x2t__ operator +(__ft__ angle, Rot__x2t__ rot)
        {
            return Rot__x2t__.Add(rot, angle);
        }

        /// <summary>
        /// Subtracts 2 rotations.
        /// </summary>
        public static Rot__x2t__ operator -(Rot__x2t__ r0, Rot__x2t__ r1)
        {
            return Rot__x2t__.Subtract(r0, r1);
        }

        /// <summary>
        /// Subtracts a rotation from a scalar value.
        /// </summary>
        public static Rot__x2t__ operator -(Rot__x2t__ rot, __ft__ angle)
        {
            return Rot__x2t__.Subtract(rot, angle);
        }

        /// <summary>
        /// Subtracts a rotation from a scalar value.
        /// </summary>
        public static Rot__x2t__ operator -(__ft__ angle, Rot__x2t__ rot)
        {
            return Rot__x2t__.Subtract(angle, rot);
        }

        /// <summary>
        /// Multiplies rotation with scalar value.
        /// </summary>
        public static Rot__x2t__ operator *(Rot__x2t__ rot, __ft__ val)
        {
            return Rot__x2t__.Multiply(rot, val);
        }

        /// <summary>
        /// Multiplies rotation with scalar value.
        /// </summary>
        public static Rot__x2t__ operator *(__ft__ val, Rot__x2t__ rot)
        {
            return Rot__x2t__.Multiply(rot, val);
        }

        /// <summary>
        /// </summary>
        public static V__x2t__ operator *(Rot__x2t__ rot, V__x2t__ vec)
        {
            return Rot__x2t__.Multiply(rot, vec);
        }

        /// <summary>
        /// </summary>
        public static V__x3t__ operator *(Rot__x2t__ rot, V__x3t__ vec)
        {
            return Rot__x2t__.Multiply(rot, vec);
        }

        /// <summary>
        /// </summary>
        public static V__x4t__ operator *(Rot__x2t__ rot, V__x4t__ vec)
        {
            return Rot__x2t__.Multiply(rot, vec);
        }

        public static M2__x2t__ operator *(Rot__x2t__ rot, M2__x2t__ mat)
        {
            return Rot__x2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static M3__x3t__ operator *(Rot__x2t__ rot, M3__x3t__ mat)
        {
            return Rot__x2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static M3__x4t__ operator *(Rot__x2t__ rot, M3__x4t__ mat)
        {
            return Rot__x2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static M4__x4t__ operator *(Rot__x2t__ rot, M4__x4t__ mat)
        {
            return Rot__x2t__.Multiply(rot, mat);
        }

        /// <summary>
        /// </summary>
        public static M3__x3t__ operator *(Rot__x2t__ rot2, Rot__x3t__ rot3)
        {
            return Rot__x2t__.Multiply(rot2, rot3);
        }

        /// <summary>
        /// </summary>
        public static Rot__x2t__ operator *(Rot__x2t__ r0, Rot__x2t__ r1)
        {
            return Rot__x2t__.Multiply(r0, r1);
        }

        /// <summary>
        /// </summary>
        public static M3__x3t__ operator *(Rot__x2t__ rot, Scale__x3t__ scale)
        {
            return Rot__x2t__.Multiply(rot, scale);
        }

        /// <summary>
        /// </summary>
        public static M3__x4t__ operator *(Rot__x2t__ rot, Shift__x3t__ shift)
        {
            return Rot__x2t__.Multiply(rot, shift);
        }

        /// <summary>
        /// Divides rotation by scalar value.
        /// </summary>
        public static Rot__x2t__ operator /(Rot__x2t__ rot, __ft__ val)
        {
            return Rot__x2t__.Divide(rot, val);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if 2 rotations are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator ==(Rot__x2t__ rotation1, Rot__x2t__ rotation2)
        {
            return (rotation1.Angle == rotation2.Angle);
        }

        /// <summary>
        /// Checks if 2 rotations are not equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator !=(Rot__x2t__ rotation1, Rot__x2t__ rotation2)
        {
            return !(rotation1.Angle == rotation2.Angle);
        }


        public static bool ApproxEqual(Rot__x2t__ r0, Rot__x2t__ r1)
        {
            return ApproxEqual(r0, r1, Constant<__ft__>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes ber�cksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add Rot__x3t__.ApproxEqual(Rot__x3t__ other);
        public static bool ApproxEqual(Rot__x2t__ r0, Rot__x2t__ r1, __ft__ tolerance)
        {
            return (r0.Angle - r1.Angle).Abs() <= tolerance;
        }

        #endregion

        #region Creator Function

        //WARNING: untested

        public static Rot__x2t__ FromM2__x2t__(M2__x2t__ m)
        {
            // cos(a) sin(a)
            //-sin(a) cos(a)

            if (m.M00 >= -1.0 && m.M00 <= 1.0)
            {
                return new Rot__x2t__((__ft__)System.Math.Acos(m.M00));
            }
            else throw new ArgumentException("Given M2__x2t__ is not a Rotation-Matrix");
        }

        #endregion

        #region Conversion Operators

        public static explicit operator M2__x2t__(Rot__x2t__ r)
        {
            var ca = (__ft__)System.Math.Cos(r.Angle);
            var sa = (__ft__)System.Math.Sin(r.Angle);

            return new M2__x2t__(ca, sa, -sa, ca);
        }

        public static explicit operator M2__x3t__(Rot__x2t__ r)
        {
            var ca = (__ft__)System.Math.Cos(r.Angle);
            var sa = (__ft__)System.Math.Sin(r.Angle);

            return new M2__x3t__(ca, sa, 0.0f, -sa, ca, 0.0f);
        }

        public static explicit operator M3__x3t__(Rot__x2t__ r)
        {
            var ca = (__ft__)System.Math.Cos(r.Angle);
            var sa = (__ft__)System.Math.Sin(r.Angle);

            return new M3__x3t__(ca, sa, 0,
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
            return string.Format(Localization.FormatEnUS, "[{0}]", Angle);
        }

        public static Rot__x2t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Rot__x2t__(
                __ft__.Parse(x[0], Localization.FormatEnUS)
            );
        }

        /// <summary>
        /// Checks if 2 objects are equal.
        /// </summary>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Rot__x2t__)
            {
                Rot__x2t__ rotation = (Rot__x2t__)obj;
                return (Angle == rotation.Angle);
            }
            return false;
        }

        #endregion
    }
    //# } // isDouble
}

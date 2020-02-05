using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    #region Rot2f

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rot2f
    {
        [DataMember]
        public float Angle;

        #region Constructors

        public Rot2f(float angleInRadians)
        {
            Angle = angleInRadians;
        }

        #endregion

        #region Constants

        public static readonly Rot2f Identity = new Rot2f(0);

        #endregion

        #region Properties

        public Rot2f Inverse
        {
            get
            {
                return new Rot2f(-Angle);
            }
        }

        #endregion

        #region Arithmetic operators

        /// <summary>
        /// Negates the rotation.
        /// </summary>
        public static Rot2f operator -(Rot2f rot)
        {
            return new Rot2f(-rot.Angle);
        }

        /// <summary>
        /// Adds two rotations.
        /// </summary>
        public static Rot2f operator +(Rot2f r0, Rot2f r1)
        {
            return new Rot2f(r0.Angle + r1.Angle);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static Rot2f operator +(Rot2f rot, float angle)
        {
            return new Rot2f(rot.Angle + angle);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static Rot2f operator +(float angle, Rot2f rot)
        {
            return new Rot2f(rot.Angle + angle);
        }

        /// <summary>
        /// Subtracts two rotations.
        /// </summary>
        public static Rot2f operator -(Rot2f r0, Rot2f r1)
        {
            return new Rot2f(r0.Angle - r1.Angle);
        }

        /// <summary>
        /// Subtracts a scalar value from a rotation.
        /// </summary>
        public static Rot2f operator -(Rot2f rot, float angle)
        {
            return new Rot2f(rot.Angle - angle);
        }

        /// <summary>
        /// Subtracts a rotation from a scalar value.
        /// </summary>
        public static Rot2f operator -(float angle, Rot2f rot)
        {
            return new Rot2f(angle - rot.Angle);
        }

        /// <summary>
        /// Multiplies a rotation with a scalar value.
        /// </summary>
        public static Rot2f operator *(Rot2f rot, float val)
        {
            return new Rot2f(rot.Angle * val);
        }

        /// <summary>
        /// Multiplies a scalar value with a rotation.
        /// </summary>
        public static Rot2f operator *(float val, Rot2f rot)
        {
            return new Rot2f(rot.Angle * val);
        }

        public static V2f operator *(Rot2f rot, V2f vec)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new V2f(a * vec.X + b * vec.Y,
                              -b * vec.X + a * vec.Y);
        }

        public static V3f operator *(Rot2f rot, V3f vec)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new V3f(a * vec.X + b * vec.Y,
                              -b * vec.X + a * vec.Y,
                               vec.Z);
        }

        public static V4f operator *(Rot2f rot, V4f vec)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new V4f(a * vec.X + b * vec.Y,
                              -b * vec.X + a * vec.Y,
                               vec.Z,
                               vec.W);
        }

        public static M22f operator *(Rot2f rot, M22f mat)
        {
            return (M22f)rot * mat;
        }

        public static M33f operator *(Rot2f rot, M33f mat)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new M33f(a * mat.M00 + b * mat.M10,
                                a * mat.M01 + b * mat.M11,
                                a * mat.M02 + b * mat.M12,
                               -b * mat.M00 + a * mat.M10,
                               -b * mat.M01 + a * mat.M11,
                               -b * mat.M02 + a * mat.M12,
                                mat.M20, mat.M21, mat.M22);
        }

        public static M34f operator *(Rot2f rot, M34f mat)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new M34f(a * mat.M00 + b * mat.M10,
                                a * mat.M01 + b * mat.M11,
                                a * mat.M02 + b * mat.M12,
                                a * mat.M03 + b * mat.M13,
                               -b * mat.M00 + a * mat.M10,
                               -b * mat.M01 + a * mat.M11,
                               -b * mat.M02 + a * mat.M12,
                               -b * mat.M03 + a * mat.M13,
                                mat.M20, mat.M21, mat.M22, mat.M23);
        }

        public static M44f operator *(Rot2f rot, M44f mat)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new M44f(a * mat.M00 + b * mat.M10,
                                a * mat.M01 + b * mat.M11,
                                a * mat.M02 + b * mat.M12,
                                a * mat.M03 + b * mat.M13,
                               -b * mat.M00 + a * mat.M10,
                               -b * mat.M01 + a * mat.M11,
                               -b * mat.M02 + a * mat.M12,
                               -b * mat.M03 + a * mat.M13,
                                mat.M20, mat.M21, mat.M22, mat.M23,
                                mat.M30, mat.M31, mat.M32, mat.M33);
        }

        public static M33f operator *(Rot2f rot2, Rot3f rot3)
        {
            return rot2 * (M33f)rot3;
        }

        public static Rot2f operator *(Rot2f r0, Rot2f r1)
        {
            return new Rot2f(r0.Angle * r1.Angle);
        }

        public static M33f operator *(Rot2f rot, Scale3f scale)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new M33f(a * scale.X, b * scale.Y, 0,
                               -b * scale.X, a * scale.Y, 0,
                                0, 0, scale.Z);
        }

        public static M34f operator *(Rot2f rot, Shift3f shift)
        {
            float a = Fun.Cos(rot.Angle);
            float b = Fun.Sin(rot.Angle);

            return new M34f(a, b, 0, a * shift.X + b * shift.Y,
                               -b, a, 0, -b * shift.X + a * shift.Y,
                                0, 0, 1, shift.Z);
        }

        /// <summary>
        /// Divides rotation by scalar value.
        /// </summary>
        public static Rot2f operator /(Rot2f rot, float val)
        {
            return new Rot2f(rot.Angle / val);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if 2 rotations are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator ==(Rot2f rotation1, Rot2f rotation2)
        {
            return (rotation1.Angle == rotation2.Angle);
        }

        /// <summary>
        /// Checks if 2 rotations are not equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator !=(Rot2f rotation1, Rot2f rotation2)
        {
            return !(rotation1.Angle == rotation2.Angle);
        }

        #endregion

        #region Creator Function

        //WARNING: untested

        public static Rot2f FromM22f(M22f m)
        {
            // cos(a) sin(a)
            //-sin(a) cos(a)

            if (m.M00 >= -1.0 && m.M00 <= 1.0)
            {
                return new Rot2f(Fun.Acos(m.M00));
            }
            else throw new ArgumentException("Given M22f is not a Rotation-Matrix");
        }

        #endregion

        #region Conversion Operators

        public static explicit operator M22f(Rot2f r)
        {
            var ca = Fun.Cos(r.Angle);
            var sa = Fun.Sin(r.Angle);

            return new M22f(ca, sa, -sa, ca);
        }

        public static explicit operator M23f(Rot2f r)
        {
            var ca = Fun.Cos(r.Angle);
            var sa = Fun.Sin(r.Angle);

            return new M23f(ca, sa, 0.0f, -sa, ca, 0.0f);
        }

        public static explicit operator M33f(Rot2f r)
        {
            var ca = Fun.Cos(r.Angle);
            var sa = Fun.Sin(r.Angle);

            return new M33f(ca, sa, 0,
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

        public static Rot2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Rot2f(
                float.Parse(x[0], CultureInfo.InvariantCulture)
            );
        }

        /// <summary>
        /// Checks if 2 objects are equal.
        /// </summary>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Rot2f)
            {
                Rot2f rotation = (Rot2f)obj;
                return (Angle == rotation.Angle);
            }
            return false;
        }

        #endregion
    }

    public static partial class Rot
    {
        #region Invert

        /// <summary>
        /// Inverts a rotation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Rot2f rot)
        {
            rot.Angle = -rot.Angle;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f Transform(this Rot2f rot, V2f v)
        {
            return rot * v;
        }

        /// <summary>
        /// Inverse transforms a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransform(this Rot2f rot, V2f v)
        {
            return -rot * v;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot2f r0, Rot2f r1)
        {
            return ApproximateEquals(r0, r1, Constant<float>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes ber�cksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add Rot3f.ApproximateEquals(Rot3f other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot2f r0, Rot2f r1, float tolerance)
        {
            return (r0.Angle - r1.Angle).Abs() <= tolerance;
        }

        #endregion
    }

    #endregion

    #region Rot2d

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rot2d
    {
        [DataMember]
        public double Angle;

        #region Constructors

        public Rot2d(double angleInRadians)
        {
            Angle = angleInRadians;
        }

        #endregion

        #region Constants

        public static readonly Rot2d Identity = new Rot2d(0);

        #endregion

        #region Properties

        public Rot2d Inverse
        {
            get
            {
                return new Rot2d(-Angle);
            }
        }

        #endregion

        #region Arithmetic operators

        /// <summary>
        /// Negates the rotation.
        /// </summary>
        public static Rot2d operator -(Rot2d rot)
        {
            return new Rot2d(-rot.Angle);
        }

        /// <summary>
        /// Adds two rotations.
        /// </summary>
        public static Rot2d operator +(Rot2d r0, Rot2d r1)
        {
            return new Rot2d(r0.Angle + r1.Angle);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static Rot2d operator +(Rot2d rot, double angle)
        {
            return new Rot2d(rot.Angle + angle);
        }

        /// <summary>
        /// Adds a rotation and a scalar value.
        /// </summary>
        public static Rot2d operator +(double angle, Rot2d rot)
        {
            return new Rot2d(rot.Angle + angle);
        }

        /// <summary>
        /// Subtracts two rotations.
        /// </summary>
        public static Rot2d operator -(Rot2d r0, Rot2d r1)
        {
            return new Rot2d(r0.Angle - r1.Angle);
        }

        /// <summary>
        /// Subtracts a scalar value from a rotation.
        /// </summary>
        public static Rot2d operator -(Rot2d rot, double angle)
        {
            return new Rot2d(rot.Angle - angle);
        }

        /// <summary>
        /// Subtracts a rotation from a scalar value.
        /// </summary>
        public static Rot2d operator -(double angle, Rot2d rot)
        {
            return new Rot2d(angle - rot.Angle);
        }

        /// <summary>
        /// Multiplies a rotation with a scalar value.
        /// </summary>
        public static Rot2d operator *(Rot2d rot, double val)
        {
            return new Rot2d(rot.Angle * val);
        }

        /// <summary>
        /// Multiplies a scalar value with a rotation.
        /// </summary>
        public static Rot2d operator *(double val, Rot2d rot)
        {
            return new Rot2d(rot.Angle * val);
        }

        public static V2d operator *(Rot2d rot, V2d vec)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new V2d(a * vec.X + b * vec.Y,
                              -b * vec.X + a * vec.Y);
        }

        public static V3d operator *(Rot2d rot, V3d vec)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new V3d(a * vec.X + b * vec.Y,
                              -b * vec.X + a * vec.Y,
                               vec.Z);
        }

        public static V4d operator *(Rot2d rot, V4d vec)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new V4d(a * vec.X + b * vec.Y,
                              -b * vec.X + a * vec.Y,
                               vec.Z,
                               vec.W);
        }

        public static M22d operator *(Rot2d rot, M22d mat)
        {
            return (M22d)rot * mat;
        }

        public static M33d operator *(Rot2d rot, M33d mat)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new M33d(a * mat.M00 + b * mat.M10,
                                a * mat.M01 + b * mat.M11,
                                a * mat.M02 + b * mat.M12,
                               -b * mat.M00 + a * mat.M10,
                               -b * mat.M01 + a * mat.M11,
                               -b * mat.M02 + a * mat.M12,
                                mat.M20, mat.M21, mat.M22);
        }

        public static M34d operator *(Rot2d rot, M34d mat)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new M34d(a * mat.M00 + b * mat.M10,
                                a * mat.M01 + b * mat.M11,
                                a * mat.M02 + b * mat.M12,
                                a * mat.M03 + b * mat.M13,
                               -b * mat.M00 + a * mat.M10,
                               -b * mat.M01 + a * mat.M11,
                               -b * mat.M02 + a * mat.M12,
                               -b * mat.M03 + a * mat.M13,
                                mat.M20, mat.M21, mat.M22, mat.M23);
        }

        public static M44d operator *(Rot2d rot, M44d mat)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new M44d(a * mat.M00 + b * mat.M10,
                                a * mat.M01 + b * mat.M11,
                                a * mat.M02 + b * mat.M12,
                                a * mat.M03 + b * mat.M13,
                               -b * mat.M00 + a * mat.M10,
                               -b * mat.M01 + a * mat.M11,
                               -b * mat.M02 + a * mat.M12,
                               -b * mat.M03 + a * mat.M13,
                                mat.M20, mat.M21, mat.M22, mat.M23,
                                mat.M30, mat.M31, mat.M32, mat.M33);
        }

        public static M33d operator *(Rot2d rot2, Rot3d rot3)
        {
            return rot2 * (M33d)rot3;
        }

        public static Rot2d operator *(Rot2d r0, Rot2d r1)
        {
            return new Rot2d(r0.Angle * r1.Angle);
        }

        public static M33d operator *(Rot2d rot, Scale3d scale)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new M33d(a * scale.X, b * scale.Y, 0,
                               -b * scale.X, a * scale.Y, 0,
                                0, 0, scale.Z);
        }

        public static M34d operator *(Rot2d rot, Shift3d shift)
        {
            double a = Fun.Cos(rot.Angle);
            double b = Fun.Sin(rot.Angle);

            return new M34d(a, b, 0, a * shift.X + b * shift.Y,
                               -b, a, 0, -b * shift.X + a * shift.Y,
                                0, 0, 1, shift.Z);
        }

        /// <summary>
        /// Divides rotation by scalar value.
        /// </summary>
        public static Rot2d operator /(Rot2d rot, double val)
        {
            return new Rot2d(rot.Angle / val);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if 2 rotations are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator ==(Rot2d rotation1, Rot2d rotation2)
        {
            return (rotation1.Angle == rotation2.Angle);
        }

        /// <summary>
        /// Checks if 2 rotations are not equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public static bool operator !=(Rot2d rotation1, Rot2d rotation2)
        {
            return !(rotation1.Angle == rotation2.Angle);
        }

        #endregion

        #region Creator Function

        //WARNING: untested

        public static Rot2d FromM22d(M22d m)
        {
            // cos(a) sin(a)
            //-sin(a) cos(a)

            if (m.M00 >= -1.0 && m.M00 <= 1.0)
            {
                return new Rot2d(Fun.Acos(m.M00));
            }
            else throw new ArgumentException("Given M22d is not a Rotation-Matrix");
        }

        #endregion

        #region Conversion Operators

        public static explicit operator M22d(Rot2d r)
        {
            var ca = Fun.Cos(r.Angle);
            var sa = Fun.Sin(r.Angle);

            return new M22d(ca, sa, -sa, ca);
        }

        public static explicit operator M23d(Rot2d r)
        {
            var ca = Fun.Cos(r.Angle);
            var sa = Fun.Sin(r.Angle);

            return new M23d(ca, sa, 0.0f, -sa, ca, 0.0f);
        }

        public static explicit operator M33d(Rot2d r)
        {
            var ca = Fun.Cos(r.Angle);
            var sa = Fun.Sin(r.Angle);

            return new M33d(ca, sa, 0,
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

        public static Rot2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Rot2d(
                double.Parse(x[0], CultureInfo.InvariantCulture)
            );
        }

        /// <summary>
        /// Checks if 2 objects are equal.
        /// </summary>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Rot2d)
            {
                Rot2d rotation = (Rot2d)obj;
                return (Angle == rotation.Angle);
            }
            return false;
        }

        #endregion
    }

    public static partial class Rot
    {
        #region Invert

        /// <summary>
        /// Inverts a rotation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Rot2d rot)
        {
            rot.Angle = -rot.Angle;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d Transform(this Rot2d rot, V2d v)
        {
            return rot * v;
        }

        /// <summary>
        /// Inverse transforms a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransform(this Rot2d rot, V2d v)
        {
            return -rot * v;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot2d r0, Rot2d r1)
        {
            return ApproximateEquals(r0, r1, Constant<double>.PositiveTinyValue);
        }

        // [todo ISSUE 20090225 andi : andi] Wir sollten auch folgendes ber�cksichtigen -q == q, weil es die selbe rotation definiert.
        // [todo ISSUE 20090427 andi : andi] use an angle-tolerance
        // [todo ISSUE 20090427 andi : andi] add Rot3d.ApproximateEquals(Rot3d other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Rot2d r0, Rot2d r1, double tolerance)
        {
            return (r0.Angle - r1.Angle).Abs() <= tolerance;
        }

        #endregion
    }

    #endregion

}

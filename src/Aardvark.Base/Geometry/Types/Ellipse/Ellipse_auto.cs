using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Ellipse2f

    /// <summary>
    /// A 2d ellipse is defined by its center
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ellipse2f
    {
        public V2f Center;
        public V2f Axis0;
        public V2f Axis1;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse2f(V2f center, V2f axis0, V2f axis1)
        {
            Center = center;
            Axis0 = axis0;
            Axis1 = axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse2f(Ellipse2f o)
        {
            Center = o.Center;
            Axis0 = o.Axis0;
            Axis1 = o.Axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse2f(Ellipse2d o)
        {
            Center = (V2f)o.Center;
            Axis0 = (V2f)o.Axis0;
            Axis1 = (V2f)o.Axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static Ellipse2f FromConjugateDiameters(V2f center, V2f a, V2f b)
        {
            var ab = Vec.Dot(a, b);
            float a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) return new Ellipse2f(center, a, b);
                else return new Ellipse2f(center, b, a);
            }
            else
            {
                var t = 0.5f * Fun.Atan2(2 * ab, a2 - b2);
                float ct = Fun.Cos(t), st = Fun.Sin(t);
                V2f v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared) return new Ellipse2f(center, v0, v1);
                else return new Ellipse2f(center, v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static Ellipse2f FromConjugateDiameters(V2f center, V2f a, V2f b,
                out float major2, out float minor2)
        {
            var ab = Vec.Dot(a, b);
            float a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse2f(center, a, b); }
                else { major2 = b2; minor2 = a2; return new Ellipse2f(center, b, a); }
            }
            else
            {
                var t = 0.5f * Fun.Atan2(2 * ab, a2 - b2);
                float ct = Fun.Cos(t), st = Fun.Sin(t);
                V2f v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse2f(center, v0, v1); }
                else { major2 = b2; minor2 = a2; return new Ellipse2f(center, v1, v0); }
            }
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ellipse2f(Ellipse2d o)
            => new Ellipse2f(o);

        #endregion

        #region Constants

        public static Ellipse2f Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ellipse2f(V2f.Zero, V2f.Zero, V2f.Zero);
        }

        #endregion

        #region Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2f GetVector(float alpha)
            => Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2f GetPoint(float alpha)
            => Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference.
        /// </summary>
        public readonly void ForEachVector(int count, Action<int, V2f> index_vector_act)
        {
            float d = ConstantF.PiTimesTwo / count;
            float a = Fun.Sin(d * 0.5f).Square() * 2, b = Fun.Sin(d); // init trig. recurrence
            float ct = 1, st = 0;

            index_vector_act(0, Axis0);
            for (int i = 1; i < count; i++)
            {
                float dct = a * ct + b * st, dst = a * st - b * ct; ;  // trig. recurrence
                ct -= dct; st -= dst;                                   // cos (t + d), sin (t + d)
                index_vector_act(i, Axis0 * ct + Axis1 * st);
            }
        }

        /// <summary>
        /// Get count points on the circumference of the ellipse.
        /// </summary>
        public readonly V2f[] GetPoints(int count)
        {
            var array = new V2f[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ellipse2f a, Ellipse2f b) =>
            (a.Center == b.Center) &&
            (a.Axis0 == b.Axis0) &&
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ellipse2f a, Ellipse2f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
            => HashCode.GetCombined(Center, Axis0, Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Ellipse2f other) =>
            Center.Equals(other.Center) &&
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
             => (other is Ellipse2f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Axis0, Axis1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ellipse2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ellipse2f(
                V2f.Parse(x[0]),
                V2f.Parse(x[1]),
                V2f.Parse(x[2])
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ellipse2f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse2f a, Ellipse2f b, float tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ellipse2f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse2f a, Ellipse2f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Ellipse3f

    /// <summary>
    /// A 3d ellipse is defined by its center, its plane normal,
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ellipse3f
    {
        public V3f Center;
        public V3f Normal;
        public V3f Axis0;
        public V3f Axis1;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse3f(V3f center, V3f normal, V3f axis0, V3f axis1)
        {
            Center = center;
            Normal = normal;
            Axis0 = axis0;
            Axis1 = axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse3f(Ellipse3f o)
        {
            Center = o.Center;
            Normal = o.Normal;
            Axis0 = o.Axis0;
            Axis1 = o.Axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse3f(Ellipse3d o)
        {
            Center = (V3f)o.Center;
            Normal = (V3f)o.Normal;
            Axis0 = (V3f)o.Axis0;
            Axis1 = (V3f)o.Axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static Ellipse3f FromConjugateDiameters(V3f center, V3f normal, V3f a, V3f b)
        {
            var ab = Vec.Dot(a, b);
            float a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) return new Ellipse3f(center, normal, a, b);
                else return new Ellipse3f(center, normal, b, a);
            }
            else
            {
                var t = 0.5f * Fun.Atan2(2 * ab, a2 - b2);
                float ct = Fun.Cos(t), st = Fun.Sin(t);
                V3f v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared) return new Ellipse3f(center, normal, v0, v1);
                else return new Ellipse3f(center, normal, v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static Ellipse3f FromConjugateDiameters(V3f center, V3f normal, V3f a, V3f b,
                out float major2, out float minor2)
        {
            var ab = Vec.Dot(a, b);
            float a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse3f(center, normal, a, b); }
                else { major2 = b2; minor2 = a2; return new Ellipse3f(center, normal, b, a); }
            }
            else
            {
                var t = 0.5f * Fun.Atan2(2 * ab, a2 - b2);
                float ct = Fun.Cos(t), st = Fun.Sin(t);
                V3f v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse3f(center, normal, v0, v1); }
                else { major2 = b2; minor2 = a2; return new Ellipse3f(center, normal, v1, v0); }
            }
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ellipse3f(Ellipse3d o)
            => new Ellipse3f(o);

        #endregion

        #region Constants

        public static Ellipse3f Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ellipse3f(V3f.Zero, V3f.Zero, V3f.Zero, V3f.Zero);
        }

        #endregion

        #region Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V3f GetVector(float alpha)
            => Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V3f GetPoint(float alpha)
            => Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference.
        /// </summary>
        public readonly void ForEachVector(int count, Action<int, V3f> index_vector_act)
        {
            float d = ConstantF.PiTimesTwo / count;
            float a = Fun.Sin(d * 0.5f).Square() * 2, b = Fun.Sin(d); // init trig. recurrence
            float ct = 1, st = 0;

            index_vector_act(0, Axis0);
            for (int i = 1; i < count; i++)
            {
                float dct = a * ct + b * st, dst = a * st - b * ct; ;  // trig. recurrence
                ct -= dct; st -= dst;                                   // cos (t + d), sin (t + d)
                index_vector_act(i, Axis0 * ct + Axis1 * st);
            }
        }

        /// <summary>
        /// Get count points on the circumference of the ellipse.
        /// </summary>
        public readonly V3f[] GetPoints(int count)
        {
            var array = new V3f[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ellipse3f a, Ellipse3f b) =>
            (a.Center == b.Center) &&
            (a.Normal == b.Normal) &&
            (a.Axis0 == b.Axis0) &&
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ellipse3f a, Ellipse3f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
            => HashCode.GetCombined(Center, Normal, Axis0, Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Ellipse3f other) =>
            Center.Equals(other.Center) &&
            Normal.Equals(other.Normal) &&
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
             => (other is Ellipse3f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Center, Normal, Axis0, Axis1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ellipse3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ellipse3f(
                V3f.Parse(x[0]),
                V3f.Parse(x[1]),
                V3f.Parse(x[2]),
                V3f.Parse(x[3])
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ellipse3f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse3f a, Ellipse3f b, float tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ellipse3f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse3f a, Ellipse3f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Ellipse2d

    /// <summary>
    /// A 2d ellipse is defined by its center
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ellipse2d
    {
        public V2d Center;
        public V2d Axis0;
        public V2d Axis1;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse2d(V2d center, V2d axis0, V2d axis1)
        {
            Center = center;
            Axis0 = axis0;
            Axis1 = axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse2d(Ellipse2d o)
        {
            Center = o.Center;
            Axis0 = o.Axis0;
            Axis1 = o.Axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse2d(Ellipse2f o)
        {
            Center = (V2d)o.Center;
            Axis0 = (V2d)o.Axis0;
            Axis1 = (V2d)o.Axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static Ellipse2d FromConjugateDiameters(V2d center, V2d a, V2d b)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) return new Ellipse2d(center, a, b);
                else return new Ellipse2d(center, b, a);
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V2d v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared) return new Ellipse2d(center, v0, v1);
                else return new Ellipse2d(center, v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static Ellipse2d FromConjugateDiameters(V2d center, V2d a, V2d b,
                out double major2, out double minor2)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse2d(center, a, b); }
                else { major2 = b2; minor2 = a2; return new Ellipse2d(center, b, a); }
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V2d v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse2d(center, v0, v1); }
                else { major2 = b2; minor2 = a2; return new Ellipse2d(center, v1, v0); }
            }
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ellipse2d(Ellipse2f o)
            => new Ellipse2d(o);

        #endregion

        #region Constants

        public static Ellipse2d Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero);
        }

        #endregion

        #region Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2d GetVector(double alpha)
            => Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2d GetPoint(double alpha)
            => Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference.
        /// </summary>
        public readonly void ForEachVector(int count, Action<int, V2d> index_vector_act)
        {
            double d = Constant.PiTimesTwo / count;
            double a = Fun.Sin(d * 0.5).Square() * 2, b = Fun.Sin(d); // init trig. recurrence
            double ct = 1, st = 0;

            index_vector_act(0, Axis0);
            for (int i = 1; i < count; i++)
            {
                double dct = a * ct + b * st, dst = a * st - b * ct; ;  // trig. recurrence
                ct -= dct; st -= dst;                                   // cos (t + d), sin (t + d)
                index_vector_act(i, Axis0 * ct + Axis1 * st);
            }
        }

        /// <summary>
        /// Get count points on the circumference of the ellipse.
        /// </summary>
        public readonly V2d[] GetPoints(int count)
        {
            var array = new V2d[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ellipse2d a, Ellipse2d b) =>
            (a.Center == b.Center) &&
            (a.Axis0 == b.Axis0) &&
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ellipse2d a, Ellipse2d b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
            => HashCode.GetCombined(Center, Axis0, Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Ellipse2d other) =>
            Center.Equals(other.Center) &&
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
             => (other is Ellipse2d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Axis0, Axis1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ellipse2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ellipse2d(
                V2d.Parse(x[0]),
                V2d.Parse(x[1]),
                V2d.Parse(x[2])
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ellipse2d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse2d a, Ellipse2d b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ellipse2d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse2d a, Ellipse2d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

    #region Ellipse3d

    /// <summary>
    /// A 3d ellipse is defined by its center, its plane normal,
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ellipse3d
    {
        public V3d Center;
        public V3d Normal;
        public V3d Axis0;
        public V3d Axis1;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse3d(V3d center, V3d normal, V3d axis0, V3d axis1)
        {
            Center = center;
            Normal = normal;
            Axis0 = axis0;
            Axis1 = axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse3d(Ellipse3d o)
        {
            Center = o.Center;
            Normal = o.Normal;
            Axis0 = o.Axis0;
            Axis1 = o.Axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ellipse3d(Ellipse3f o)
        {
            Center = (V3d)o.Center;
            Normal = (V3d)o.Normal;
            Axis0 = (V3d)o.Axis0;
            Axis1 = (V3d)o.Axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static Ellipse3d FromConjugateDiameters(V3d center, V3d normal, V3d a, V3d b)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) return new Ellipse3d(center, normal, a, b);
                else return new Ellipse3d(center, normal, b, a);
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V3d v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared) return new Ellipse3d(center, normal, v0, v1);
                else return new Ellipse3d(center, normal, v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static Ellipse3d FromConjugateDiameters(V3d center, V3d normal, V3d a, V3d b,
                out double major2, out double minor2)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse3d(center, normal, a, b); }
                else { major2 = b2; minor2 = a2; return new Ellipse3d(center, normal, b, a); }
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V3d v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new Ellipse3d(center, normal, v0, v1); }
                else { major2 = b2; minor2 = a2; return new Ellipse3d(center, normal, v1, v0); }
            }
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ellipse3d(Ellipse3f o)
            => new Ellipse3d(o);

        #endregion

        #region Constants

        public static Ellipse3d Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ellipse3d(V3d.Zero, V3d.Zero, V3d.Zero, V3d.Zero);
        }

        #endregion

        #region Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V3d GetVector(double alpha)
            => Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V3d GetPoint(double alpha)
            => Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference.
        /// </summary>
        public readonly void ForEachVector(int count, Action<int, V3d> index_vector_act)
        {
            double d = Constant.PiTimesTwo / count;
            double a = Fun.Sin(d * 0.5).Square() * 2, b = Fun.Sin(d); // init trig. recurrence
            double ct = 1, st = 0;

            index_vector_act(0, Axis0);
            for (int i = 1; i < count; i++)
            {
                double dct = a * ct + b * st, dst = a * st - b * ct; ;  // trig. recurrence
                ct -= dct; st -= dst;                                   // cos (t + d), sin (t + d)
                index_vector_act(i, Axis0 * ct + Axis1 * st);
            }
        }

        /// <summary>
        /// Get count points on the circumference of the ellipse.
        /// </summary>
        public readonly V3d[] GetPoints(int count)
        {
            var array = new V3d[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ellipse3d a, Ellipse3d b) =>
            (a.Center == b.Center) &&
            (a.Normal == b.Normal) &&
            (a.Axis0 == b.Axis0) &&
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ellipse3d a, Ellipse3d b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
            => HashCode.GetCombined(Center, Normal, Axis0, Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Ellipse3d other) =>
            Center.Equals(other.Center) &&
            Normal.Equals(other.Normal) &&
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
             => (other is Ellipse3d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Center, Normal, Axis0, Axis1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ellipse3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ellipse3d(
                V3d.Parse(x[0]),
                V3d.Parse(x[1]),
                V3d.Parse(x[2]),
                V3d.Parse(x[3])
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ellipse3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse3d a, Ellipse3d b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ellipse3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse3d a, Ellipse3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

}

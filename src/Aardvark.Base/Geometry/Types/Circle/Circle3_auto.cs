using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Circle3f

    /// <summary>
    /// A circle in 3-space represented by its center, normal (normalized), and a radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Circle3f : IEquatable<Circle3f>, IBoundingBox3f, IValidity
    {
        [DataMember]
        public V3f Center;

        [DataMember]
        public V3f Normal;

        [DataMember]
        public float Radius;

        #region Constructors

        /// <summary>
        /// Creates a circle from it's center, a normal vector (normalized) and a radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle3f(V3f center, V3f normal, float radius)
        {
            Center = center;
            Normal = normal;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle3f(Circle3f circle)
		{
            Center = circle.Center;
            Normal = circle.Normal;
            Radius = circle.Radius;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle3f(Circle3d circle)
        {
            Center = (V3f)circle.Center;
            Normal = (V3f)circle.Normal;
            Radius = (float)circle.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Circle3f(Circle3d c)
            => new Circle3f(c);

        #endregion

        #region Constants

        public static Circle3f Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3f(V3f.Zero, V3f.ZAxis, 0);
        }

        public static Circle3f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3f(V3f.NaN, V3f.NaN, -1);
        }

        #endregion

        #region Properties

        public readonly float RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius.Square();
        }

        public readonly float Circumference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2 * Radius * ConstantF.Pi;
        }

        public readonly float Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => RadiusSquared * ConstantF.Pi;
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0;
        }

        /// <summary>
        /// Returns a point on the circumference (AxisU).
        /// </summary>
        public readonly V3f Point
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Center + AxisU * Radius;
        }

        /// <summary>
        /// Returns an axis aligned vector pointing from the center to the circumference.
        /// </summary>
        public readonly V3f AxisU
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                V3f dir = Normal.Dot(V3f.XAxis).Abs() > 0.9f ? V3f.YAxis : V3f.XAxis;
                var pdir = Normal.Cross(dir);
                return pdir.Normalized * Radius;
            }
        }

        /// <summary>
        /// Returns an axis aligned vector pointing from the center to the circumference.
        /// </summary>
        public readonly V3f AxisV
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var axisU = AxisU;
                return AxisU.Cross(Normal).Normalized * Radius;
            }
        }

        /// <summary>
        /// Return an IEnumerable of points on the circle's circumference, optionally repeating the first point as the last.
        /// </summary>
        /// <param name="tesselation">number of distinct points to generate. the actual number of points returned depends on the <para>duplicateClosePoint</para> parameter. must be 3 or larger.</param>
        /// <param name="duplicateClosePoint">if true, the first point is repeated as the last</param>
        /// <returns>IEnumerable of points on the circle's circumference. if diplicateClosePoint is true, <para>tesselation</para>+1 points are returned.</returns>
        public readonly IEnumerable<V3f> Points(int tesselation, bool duplicateClosePoint = false)
		{
			if (tesselation < 3)
				throw new ArgumentOutOfRangeException("tesselation", "tesselation must be at least 3.");

			var off = 0;
			if (duplicateClosePoint)
				off = 1;
			for (int i = 0; i < tesselation + off; i++)
			{
				var angle = ((float)i) / tesselation * ConstantF.PiTimesTwo;
				yield return Center + AxisU * Fun.Cos(angle) + AxisV * Fun.Sin(angle);
			}
		}

        public readonly Plane3f Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3f(Normal, Center);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V3f GetPoint(float angle)
            => Center + AxisU * angle.Cos() + AxisV * angle.Sin();

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Circle3f a, Circle3f b)
            => (a.Center == b.Center) && (a.Normal == b.Normal) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Circle3f a, Circle3f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Circle3f other)
            => Center.Equals(other.Center) && Normal.Equals(other.Normal) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Circle3f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Normal, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Circle3f(V3f.Parse(x[0]), V3f.Parse(x[1]), float.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox3f Members

        public readonly Box3f BoundingBox3f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var u = AxisU; var v = AxisV;
                return new Box3f(Center + u, Center - u, Center + v, Center - v);
            }
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Circle3f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle3f a, Circle3f b, float tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Circle3f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle3f a, Circle3f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Circle3d

    /// <summary>
    /// A circle in 3-space represented by its center, normal (normalized), and a radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Circle3d : IEquatable<Circle3d>, IBoundingBox3d, IValidity
    {
        [DataMember]
        public V3d Center;

        [DataMember]
        public V3d Normal;

        [DataMember]
        public double Radius;

        #region Constructors

        /// <summary>
        /// Creates a circle from it's center, a normal vector (normalized) and a radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle3d(V3d center, V3d normal, double radius)
        {
            Center = center;
            Normal = normal;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle3d(Circle3d circle)
		{
            Center = circle.Center;
            Normal = circle.Normal;
            Radius = circle.Radius;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle3d(Circle3f circle)
        {
            Center = (V3d)circle.Center;
            Normal = (V3d)circle.Normal;
            Radius = (double)circle.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Circle3d(Circle3f c)
            => new Circle3d(c);

        #endregion

        #region Constants

        public static Circle3d Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3d(V3d.Zero, V3d.ZAxis, 0);
        }

        public static Circle3d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3d(V3d.NaN, V3d.NaN, -1);
        }

        #endregion

        #region Properties

        public readonly double RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius.Square();
        }

        public readonly double Circumference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2 * Radius * Constant.Pi;
        }

        public readonly double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => RadiusSquared * Constant.Pi;
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0;
        }

        /// <summary>
        /// Returns a point on the circumference (AxisU).
        /// </summary>
        public readonly V3d Point
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Center + AxisU * Radius;
        }

        /// <summary>
        /// Returns an axis aligned vector pointing from the center to the circumference.
        /// </summary>
        public readonly V3d AxisU
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                V3d dir = Normal.Dot(V3d.XAxis).Abs() > 0.9 ? V3d.YAxis : V3d.XAxis;
                var pdir = Normal.Cross(dir);
                return pdir.Normalized * Radius;
            }
        }

        /// <summary>
        /// Returns an axis aligned vector pointing from the center to the circumference.
        /// </summary>
        public readonly V3d AxisV
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var axisU = AxisU;
                return AxisU.Cross(Normal).Normalized * Radius;
            }
        }

        /// <summary>
        /// Return an IEnumerable of points on the circle's circumference, optionally repeating the first point as the last.
        /// </summary>
        /// <param name="tesselation">number of distinct points to generate. the actual number of points returned depends on the <para>duplicateClosePoint</para> parameter. must be 3 or larger.</param>
        /// <param name="duplicateClosePoint">if true, the first point is repeated as the last</param>
        /// <returns>IEnumerable of points on the circle's circumference. if diplicateClosePoint is true, <para>tesselation</para>+1 points are returned.</returns>
        public readonly IEnumerable<V3d> Points(int tesselation, bool duplicateClosePoint = false)
		{
			if (tesselation < 3)
				throw new ArgumentOutOfRangeException("tesselation", "tesselation must be at least 3.");

			var off = 0;
			if (duplicateClosePoint)
				off = 1;
			for (int i = 0; i < tesselation + off; i++)
			{
				var angle = ((double)i) / tesselation * Constant.PiTimesTwo;
				yield return Center + AxisU * Fun.Cos(angle) + AxisV * Fun.Sin(angle);
			}
		}

        public readonly Plane3d Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3d(Normal, Center);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V3d GetPoint(double angle)
            => Center + AxisU * angle.Cos() + AxisV * angle.Sin();

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Circle3d a, Circle3d b)
            => (a.Center == b.Center) && (a.Normal == b.Normal) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Circle3d a, Circle3d b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Circle3d other)
            => Center.Equals(other.Center) && Normal.Equals(other.Normal) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Circle3d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Normal, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Circle3d(V3d.Parse(x[0]), V3d.Parse(x[1]), double.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox3d Members

        public readonly Box3d BoundingBox3d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var u = AxisU; var v = AxisV;
                return new Box3d(Center + u, Center - u, Center + v, Center - v);
            }
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Circle3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle3d a, Circle3d b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Circle3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle3d a, Circle3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

}
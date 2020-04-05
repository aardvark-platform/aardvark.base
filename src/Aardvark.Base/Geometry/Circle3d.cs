using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// A circle in 3-space represented by its center, normal (normalized), and a radius.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle3d : IBoundingBox3d, IValidity
    {
        public V3d Center;
        public V3d Normal;
        public double Radius;

        #region Constructors
        
        /// <summary>
        /// Creates a circle from it's center, a normal vector (normalized) and a radius.
        /// </summary>
        public Circle3d(V3d center, V3d normal, double radius)
        {
            Center = center;
            Normal = normal;
            Radius = radius;
        }

        public Circle3d(Circle3d circle)
		{
            Center = circle.Center;
            Normal = circle.Normal;
            Radius = circle.Radius;
		}

        #endregion

        #region Constants

        public static readonly Circle3d Zero = new Circle3d(V3d.Zero, V3d.ZAxis, 0);
        public static readonly Circle3d Invalid = new Circle3d(V3d.NaN, V3d.NaN, -1.0);

        #endregion

        #region Properties

        public double RadiusSquared => Radius.Square();

        public double Circumference => 2.0 * Radius * Constant.Pi;

        public double Area => RadiusSquared * Constant.Pi;

        public bool IsValid => Radius >= 0.0;

        public bool IsInvalid => Radius < 0.0;

        /// <summary>
        /// Returns a point on the circumference (AxisU).
        /// </summary>
        public V3d Point => Center + AxisU * Radius;

        /// <summary>
        /// Returns an axis aligned vector pointing from the center to the circumference.
        /// </summary>
        public V3d AxisU
        {
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
        public V3d AxisV
        {
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
		public IEnumerable<V3d> Points(int tesselation, bool duplicateClosePoint = false)
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

        public Plane3d Plane => new Plane3d(Normal, Center);

        public V3d GetPoint(double angle) => Center + AxisU * angle.Cos() + AxisV * angle.Sin();

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

        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Circle3d other)
            => Center.Equals(other.Center) && Normal.Equals(other.Normal) && Radius.Equals(other.Radius);

        public override bool Equals(object other)
            => (other is Circle3d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Normal, Radius);

        public static Circle3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Circle3d(V3d.Parse(x[0]), V3d.Parse(x[1]), double.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d
        {
            get
            {
                var u = AxisU;
                var v = AxisV;
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
}
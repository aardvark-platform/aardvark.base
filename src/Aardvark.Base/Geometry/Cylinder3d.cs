using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Cylinder3d : IBoundingBox3d, IValidity
    {
        public V3d P0;
        public V3d P1;
        public double Radius;
        public double DistanceScale;

        #region Constructors

        public Cylinder3d(V3d p0, V3d p1, double radius, double distanceScale = 0)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
            DistanceScale = distanceScale;
        }

        public Cylinder3d(Line3d axis, double radius, double distanceScale = 0)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
            DistanceScale = distanceScale;
        }

        #endregion

        #region Constants

        public static readonly Cylinder3d Invalid = new Cylinder3d(V3d.NaN, V3d.NaN, -1.0);

        #endregion

        #region Properties

        public double Height => (P0 - P1).Length;

        public V3d Center => (P0 + P1) * 0.5;

        public Line3d Axis => new Line3d(P0, P1);

        public bool IsValid => Radius >= 0.0;

        public bool IsInvalid => Radius < 0.0;

        public Circle3d Circle0 => new Circle3d(P0, (P0 - P1).Normalized, Radius);

        public Circle3d Circle1 => new Circle3d(P1, (P1 - P0).Normalized, Radius);

        public double Area => Radius * Constant.PiTimesTwo * (Radius + Height);

        public double Volume => Radius * Radius * Constant.Pi * Height;

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cylinder3d a, Cylinder3d b)
            => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.Radius == b.Radius) && (a.DistanceScale == b.DistanceScale);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cylinder3d a, Cylinder3d b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(P0, P1, Radius, DistanceScale);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Cylinder3d other) =>
            P0.Equals(other.P0) &&
            P1.Equals(other.P1) &&
            Radius.Equals(other.Radius) &&
            DistanceScale.Equals(other.DistanceScale);

        public override bool Equals(object other)
            => (other is Cylinder3d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", P0, P1, Radius, DistanceScale);

        public static Cylinder3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Cylinder3d(
                V3d.Parse(x[0]),
                V3d.Parse(x[1]),
                double.Parse(x[2], CultureInfo.InvariantCulture),
                double.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        #endregion

        #region Operations

        /// <summary>
        /// P0 has height 0.0, P1 has height 1.0
        /// </summary>
        public double GetHeight(V3d p)
        {
            var dir = (P1 - P0).Normalized;
            var pp = p.GetClosestPointOn(new Ray3d(P0, dir));
            return (pp - P0).Dot(dir);
        }
        /// <summary>
        /// Get circle at a specific height
        /// </summary>
        public Circle3d GetCircle(double height)
        {
            var dir = (P1 - P0).Normalized;
            return new Circle3d(P0 + height * dir, dir, Radius);
        }

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d => new Box3d(Circle0.BoundingBox3d, Circle1.BoundingBox3d);

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Cylinder3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cylinder3d a, Cylinder3d b, double tolerance) =>
            ApproximateEquals(a.P0, b.P0, tolerance) &&
            ApproximateEquals(a.P1, b.P1, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance) &&
            ApproximateEquals(a.DistanceScale, b.DistanceScale, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Cylinder3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cylinder3d a, Cylinder3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }
}

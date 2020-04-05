using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    #region ObliqueCone3d

    [StructLayout(LayoutKind.Sequential)]
    public struct ObliqueCone3d
    {
        public readonly V3d Origin;
        public readonly Circle3d Circle;

        #region Constructor

        public ObliqueCone3d(V3d o, Circle3d c)
        {
            Origin = o;
            Circle = c;
        }

        #endregion

        #region Constants

        public static readonly ObliqueCone3d Invalid = new ObliqueCone3d(V3d.NaN, Circle3d.Invalid);

        #endregion

        #region Properties

        public bool IsValid => Circle.Radius >= 0.0;
        public bool IsInvalid => Circle.Radius < 0.0;

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ObliqueCone3d a, ObliqueCone3d b)
            => (a.Origin == b.Origin) && (a.Circle == b.Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ObliqueCone3d a, ObliqueCone3d b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Origin, Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ObliqueCone3d other)
            => Origin.Equals(other.Origin) && Circle.Equals(other.Circle);

        public override bool Equals(object other)
            => (other is ObliqueCone3d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Circle);

        public static ObliqueCone3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new ObliqueCone3d(V3d.Parse(x[0]), Circle3d.Parse(x[1]));
        }

        #endregion

        #region Operations

        /// <summary>
        /// get circle of oblique cone where distance between cone origin and circle origin equals distance
        /// </summary>
        public Circle3d GetCircle(double distance)
        {
            var dir = Circle.Center - Origin;
            var pDistance = dir.Length;
            dir.Normalize();
            var newCenter = Origin + dir * distance;
            var newRadius = distance / pDistance * Circle.Radius;

            return new Circle3d(newCenter, Circle.Normal, newRadius);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="ObliqueCone3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ObliqueCone3d a, ObliqueCone3d b, double tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Circle, b.Circle, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="ObliqueCone3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ObliqueCone3d a, ObliqueCone3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

    #region Cone3d

    [StructLayout(LayoutKind.Sequential)]
    public struct Cone3d : IValidity
    {
        public V3d Origin;
        public V3d Direction; // axis of revolution
        public double Angle; // angle between axis and outer edge

        #region Constructor

        public Cone3d(V3d origin, V3d dir, double angle)
        {
            Origin = origin;
            Direction = dir;
            Angle = angle;
        }

        #endregion

        #region Constants

        public static readonly Cone3d Invalid = new Cone3d(V3d.NaN, V3d.Zero, 0.0);

        #endregion

        #region Properties

        public bool IsValid => Direction != V3d.Zero;
        public bool IsInvalid => Direction == V3d.Zero;

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cone3d a, Cone3d b)
            => (a.Origin == b.Origin) && (a.Direction == b.Direction) && (a.Angle == b.Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cone3d a, Cone3d b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Origin, Direction, Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Cone3d other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction) && Angle.Equals(other.Angle);

        public override bool Equals(object other)
            => (other is Cone3d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Origin, Direction, Angle);

        public static Cone3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Cone3d(V3d.Parse(x[0]), V3d.Parse(x[1]), double.Parse(x[2], CultureInfo.InvariantCulture));
        }            

        #endregion

        #region Operations

        /// <summary>
        /// if zero, point is located on cone
        /// </summary>
        public double GetDistance(V3d point) => Vec.Distance(point, GetClosestPoint(point));

        public Circle3d GetCircle(double height)
        {
            //circle along axis
            var dirLength = height;
            var radius = GetRadius(height);
            var dir = Direction.Normalized * dirLength;
            return new Circle3d(Origin + dir, dir.Normalized, radius);
        }

        public Ray3d GetAxis() => new Ray3d(Origin, Direction);

        public double GetHeight(V3d position)
            => new Ray3d(Origin, Direction).GetTOfProjectedPoint(position);

        public double GetRadius(double height) => height * Angle.Sin() / Angle.Cos();

        public V3d GetClosestPoint(V3d point)
        {
            var ray = new Ray3d(Origin, Direction);
            var cp = point.GetClosestPointOn(ray);
            var radius = GetRadius(GetHeight(point));
            var dir = (point - cp).Normalized * radius;

            var p0 = cp + dir;
            var p1 = point.GetClosestPointOn(new Ray3d(Origin, (p0 - Origin).Normalized));

            return (Vec.Distance(point, p1) < Vec.Distance(point, p0)) ? p1 : p0;
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Cone3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cone3d a, Cone3d b, double tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance) &&
            ApproximateEquals(a.Angle, b.Angle, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Cone3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cone3d a, Cone3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion
}

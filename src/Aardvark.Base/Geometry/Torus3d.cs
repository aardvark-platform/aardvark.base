using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Torus3d : IEquatable<Torus3d>, IBoundingBox3d
    {
        [DataMember]
        public V3d Position;
        [DataMember]
        public V3d Direction;
        [DataMember]
        public double MajorRadius;
        [DataMember]
        public double MinorRadius;

        #region Constructor

        public Torus3d(V3d position, V3d direction, double majorRadius, double minorRadius)
        {
            Position = position;
            Direction = direction;
            MinorRadius = minorRadius;
            MajorRadius = majorRadius;
        }

        #endregion

        #region Properties

        public Circle3d MajorCircle => GetMajorCircle();
        public double Area => 4 * Constant.PiSquared * MajorRadius * MinorRadius;
        public double Volume => 2 * Constant.PiSquared * MajorRadius * MinorRadius * MinorRadius;

        #endregion

        #region Operations

        public Circle3d GetMajorCircle() => new Circle3d(Position, Direction, MajorRadius); 

        public Circle3d GetMinorCircle(double angle)
        {
            var c = GetMajorCircle();
            var p = c.GetPoint(angle);
            var dir = (p - Position).Normalized.Cross(Direction).Normalized;
            return new Circle3d(p, dir, MinorRadius);
        }

        public double GetMinimalDistance(V3d p) => GetMinimalDistance(p, Position, Direction, MajorRadius, MinorRadius);

        public static double GetMinimalDistance(V3d p, V3d position, V3d direction, double majorRadius, double minorRadius)
        {
            var plane = new Plane3d(direction, position);
            var planePoint = p.GetClosestPointOn(plane);
            var distanceOnPlane = (Vec.Distance(planePoint, position) - majorRadius).Abs();
            var distanceToCircle = (Vec.DistanceSquared(planePoint, p) + distanceOnPlane.Square()).Sqrt();
            return (distanceToCircle - minorRadius).Abs();
        }

        #endregion

        #region Comparison operators

        /// <summary>
        /// Tests whether two specified spheres are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Torus3d a, Torus3d b) =>
            (a.Position == b.Position) &&
            (a.Direction == b.Direction) &&
            (a.MajorRadius == b.MajorRadius) &&
            (a.MinorRadius == b.MinorRadius);

        /// <summary>
        /// Tests whether two specified spheres are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Torus3d a, Torus3d b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Position, Direction, MajorRadius, MinorRadius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Torus3d other) =>
            Position.Equals(other.Position) &&
            Direction.Equals(other.Direction) &&
            MajorRadius.Equals(other.MajorRadius) &&
            MinorRadius.Equals(other.MinorRadius);

        public override bool Equals(object other)
            => (other is Torus3d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Position, Direction, MajorRadius, MinorRadius);

        public static Torus3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Torus3d(
                V3d.Parse(x[0]),
                V3d.Parse(x[1]),
                double.Parse(x[2], CultureInfo.InvariantCulture),
                double.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        #endregion

        #region IBoundingBox3d Members

        Box3d IBoundingBox3d.BoundingBox3d => GetMajorCircle().BoundingBox3d.EnlargedBy(MinorRadius);

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Torus3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Torus3d a, Torus3d b, double tolerance) =>
            ApproximateEquals(a.Position, b.Position, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance) &&
            ApproximateEquals(a.MajorRadius, b.MajorRadius, tolerance) &&
            ApproximateEquals(a.MinorRadius, b.MinorRadius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Torus3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Torus3d a, Torus3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }
}

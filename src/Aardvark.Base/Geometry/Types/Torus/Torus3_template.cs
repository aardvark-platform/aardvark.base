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
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Torus3" + tc;
    //#   var type2 = "Torus3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var circle3t = "Circle3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var trafo3t = "Trafo3" + tc;
    //#   var iboundingbox3t = "IBoundingBox3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var dotnine = isDouble ? "0.9" : "0.9f";
    //#   var constant = isDouble ? "Constant" : "ConstantF";
    #region __type__

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>, __iboundingbox3t__
    {
        [DataMember]
        public __v3t__ Position;
        [DataMember]
        public __v3t__ Direction;
        [DataMember]
        public __ftype__ MajorRadius;
        [DataMember]
        public __ftype__ MinorRadius;

        #region Constructor

        public __type__(__v3t__ position, __v3t__ direction, __ftype__ majorRadius, __ftype__ minorRadius)
        {
            Position = position;
            Direction = direction;
            MinorRadius = minorRadius;
            MajorRadius = majorRadius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ o)
        {
            Position = o.Position;
            Direction = o.Direction;
            MinorRadius = o.MinorRadius;
            MajorRadius = o.MajorRadius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ o)
        {
            Position = (__v3t__)o.Position;
            Direction = (__v3t__)o.Direction;
            MinorRadius = (__ftype__)o.MinorRadius;
            MajorRadius = (__ftype__)o.MajorRadius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type2__ o)
            => new __type__(o);

        #endregion

        #region Properties

        public __circle3t__ MajorCircle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetMajorCircle();
        }

        public __ftype__ Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 4 * __constant__.PiSquared * MajorRadius * MinorRadius;
        }

        public __ftype__ Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2 * __constant__.PiSquared * MajorRadius * MinorRadius * MinorRadius;
        }

        #endregion

        #region Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __circle3t__ GetMajorCircle() => new __circle3t__(Position, Direction, MajorRadius);

        public __circle3t__ GetMinorCircle(__ftype__ angle)
        {
            var c = GetMajorCircle();
            var p = c.GetPoint(angle);
            var dir = (p - Position).Normalized.Cross(Direction).Normalized;
            return new __circle3t__(p, dir, MinorRadius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ftype__ GetMinimalDistance(__v3t__ p) => GetMinimalDistance(p, Position, Direction, MajorRadius, MinorRadius);

        public static __ftype__ GetMinimalDistance(__v3t__ p, __v3t__ position, __v3t__ direction, __ftype__ majorRadius, __ftype__ minorRadius)
        {
            var plane = new __plane3t__(direction, position);
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
        public static bool operator ==(__type__ a, __type__ b) =>
            (a.Position == b.Position) &&
            (a.Direction == b.Direction) &&
            (a.MajorRadius == b.MajorRadius) &&
            (a.MinorRadius == b.MinorRadius);

        /// <summary>
        /// Tests whether two specified spheres are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Position, Direction, MajorRadius, MinorRadius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other) =>
            Position.Equals(other.Position) &&
            Direction.Equals(other.Direction) &&
            MajorRadius.Equals(other.MajorRadius) &&
            MinorRadius.Equals(other.MinorRadius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Position, Direction, MajorRadius, MinorRadius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(
                __v3t__.Parse(x[0]),
                __v3t__.Parse(x[1]),
                __ftype__.Parse(x[2], CultureInfo.InvariantCulture),
                __ftype__.Parse(x[3], CultureInfo.InvariantCulture)
            );
        }

        #endregion

        #region __iboundingbox3t__ Members

        __box3t__ __iboundingbox3t__.BoundingBox3__tc__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetMajorCircle().BoundingBox3__tc__.EnlargedBy(MinorRadius);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Position, b.Position, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance) &&
            ApproximateEquals(a.MajorRadius, b.MajorRadius, tolerance) &&
            ApproximateEquals(a.MinorRadius, b.MinorRadius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    //# }
}

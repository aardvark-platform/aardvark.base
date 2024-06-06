using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Cone3" + tc;
    //#   var type2 = "Cone3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var ray3t = "Ray3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var circle3t = "Circle3" + tc;
    //#   var sphere3t = "Sphere3" + tc;
    //#   var iboundingsphere3t = "IBoundingSphere3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#   var eps = isDouble ? "1e-9" : "1e-5f";
    #region Oblique__type__

    /// <summary>
    /// An oblique cone in 3-space represented by its origin (apex) and base circle.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Oblique__type__ : IEquatable<Oblique__type__>
    {
        [DataMember]
        public readonly __v3t__ Origin;

        [DataMember]
        public readonly __circle3t__ Circle;

        #region Constructor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Oblique__type__(__v3t__ o, __circle3t__ c)
        {
            Origin = o;
            Circle = c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Oblique__type__(Oblique__type__ o)
        {
            Origin = o.Origin;
            Circle = o.Circle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Oblique__type__(Oblique__type2__ o)
        {
            Origin = (__v3t__)o.Origin;
            Circle = (__circle3t__)o.Circle;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Oblique__type__(Oblique__type2__ c)
            => new Oblique__type__(c);

        #endregion

        #region Constants

        public static Oblique__type__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Oblique__type__(__v3t__.NaN, __circle3t__.Invalid);
        }

        #endregion

        #region Properties

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Circle.Radius >= 0;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Circle.Radius < 0;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Oblique__type__ a, Oblique__type__ b)
            => (a.Origin == b.Origin) && (a.Circle == b.Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Oblique__type__ a, Oblique__type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Origin, Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Oblique__type__ other)
            => Origin.Equals(other.Origin) && Circle.Equals(other.Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Oblique__type__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Oblique__type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Oblique__type__(__v3t__.Parse(x[0]), __circle3t__.Parse(x[1]));
        }

        #endregion

        #region Operations

        /// <summary>
        /// get circle of oblique cone where distance between cone origin and circle origin equals distance
        /// </summary>
        public __circle3t__ GetCircle(__ftype__ distance)
        {
            var dir = Circle.Center - Origin;
            var pDistance = dir.Length;
            dir.Normalize();
            var newCenter = Origin + dir * distance;
            var newRadius = distance / pDistance * Circle.Radius;

            return new __circle3t__(newCenter, Circle.Normal, newRadius);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Oblique__type__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Oblique__type__ a, Oblique__type__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Circle, b.Circle, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Oblique__type__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Oblique__type__ a, Oblique__type__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    #region __type__

    /// <summary>
    /// A cone in 3-space represented by its origin, axis of revolution (Direction), and the angle between axis and outer edge.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>, IValidity
    {
        [DataMember]
        public __v3t__ Origin;

        /// <summary>
        /// Axis of revolution.
        /// </summary>
        [DataMember]
        public __v3t__ Direction;

        /// <summary>
        /// Angle between axis and outer edge (in radians).
        /// </summary>
        [DataMember]
        public __ftype__ Angle;

        #region Constructor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v3t__ origin, __v3t__ dir, __ftype__ angle)
        {
            Origin = origin;
            Direction = dir;
            Angle = angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ o)
        {
            Origin = o.Origin;
            Direction = o.Direction;
            Angle = o.Angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ o)
        {
            Origin = (__v3t__)o.Origin;
            Direction = (__v3t__)o.Direction;
            Angle = (__ftype__)o.Angle;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type2__ c)
            => new __type__(c);

        #endregion

        #region Constants

        public static __type__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v3t__.NaN, __v3t__.Zero, 0);
        }

        #endregion

        #region Properties

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction != __v3t__.Zero;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction == __v3t__.Zero;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
            => (a.Origin == b.Origin) && (a.Direction == b.Direction) && (a.Angle == b.Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.GetCombined(Origin, Direction, Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(__type__ other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction) && Angle.Equals(other.Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Origin, Direction, Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__v3t__.Parse(x[0]), __v3t__.Parse(x[1]), __ftype__.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

        #region Operations

        /// <summary>
        /// if zero, point is located on cone
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __ftype__ GetDistance(__v3t__ point) => Vec.Distance(point, GetClosestPoint(point));

        public readonly __circle3t__ GetCircle(__ftype__ height)
        {
            //circle along axis
            var dirLength = height;
            var radius = GetRadius(height);
            var dir = Direction.Normalized * dirLength;
            return new __circle3t__(Origin + dir, dir.Normalized, radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __ray3t__ GetAxis() => new __ray3t__(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __ftype__ GetHeight(__v3t__ position)
            => new __ray3t__(Origin, Direction).GetTOfProjectedPoint(position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __ftype__ GetRadius(__ftype__ height) => height * Angle.Sin() / Angle.Cos();

        public readonly __v3t__ GetClosestPoint(__v3t__ point)
        {
            var ray = new __ray3t__(Origin, Direction);
            var cp = point.GetClosestPointOn(ray);
            var radius = GetRadius(GetHeight(point));
            var dir = (point - cp).Normalized * radius;

            var p0 = cp + dir;
            var p1 = point.GetClosestPointOn(new __ray3t__(Origin, (p0 - Origin).Normalized));

            return (Vec.Distance(point, p1) < Vec.Distance(point, p0)) ? p1 : p0;
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
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance) &&
            ApproximateEquals(a.Angle, b.Angle, tolerance);

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

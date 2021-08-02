using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region ObliqueCone3f

    /// <summary>
    /// An oblique cone in 3-space represented by its origin (apex) and base circle.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ObliqueCone3f : IEquatable<ObliqueCone3f>
    {
        [DataMember]
        public readonly V3f Origin;

        [DataMember]
        public readonly Circle3f Circle;

        #region Constructor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObliqueCone3f(V3f o, Circle3f c)
        {
            Origin = o;
            Circle = c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObliqueCone3f(ObliqueCone3f o)
        {
            Origin = o.Origin;
            Circle = o.Circle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObliqueCone3f(ObliqueCone3d o)
        {
            Origin = (V3f)o.Origin;
            Circle = (Circle3f)o.Circle;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ObliqueCone3f(ObliqueCone3d c)
            => new ObliqueCone3f(c);

        #endregion

        #region Constants

        public static ObliqueCone3f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ObliqueCone3f(V3f.NaN, Circle3f.Invalid);
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
        public static bool operator ==(ObliqueCone3f a, ObliqueCone3f b)
            => (a.Origin == b.Origin) && (a.Circle == b.Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ObliqueCone3f a, ObliqueCone3f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Origin, Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ObliqueCone3f other)
            => Origin.Equals(other.Origin) && Circle.Equals(other.Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is ObliqueCone3f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ObliqueCone3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new ObliqueCone3f(V3f.Parse(x[0]), Circle3f.Parse(x[1]));
        }

        #endregion

        #region Operations

        /// <summary>
        /// get circle of oblique cone where distance between cone origin and circle origin equals distance
        /// </summary>
        public Circle3f GetCircle(float distance)
        {
            var dir = Circle.Center - Origin;
            var pDistance = dir.Length;
            dir.Normalize();
            var newCenter = Origin + dir * distance;
            var newRadius = distance / pDistance * Circle.Radius;

            return new Circle3f(newCenter, Circle.Normal, newRadius);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="ObliqueCone3f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ObliqueCone3f a, ObliqueCone3f b, float tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Circle, b.Circle, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="ObliqueCone3f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ObliqueCone3f a, ObliqueCone3f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Cone3f

    /// <summary>
    /// A cone in 3-space represented by its origin, axis of revolution (Direction), and the angle between axis and outer edge.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Cone3f : IEquatable<Cone3f>, IValidity
    {
        [DataMember]
        public V3f Origin;

        /// <summary>
        /// Axis of revolution.
        /// </summary>
        [DataMember]
        public V3f Direction;

        /// <summary>
        /// Angle between axis and outer edge (in radians).
        /// </summary>
        [DataMember]
        public float Angle;

        #region Constructor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cone3f(V3f origin, V3f dir, float angle)
        {
            Origin = origin;
            Direction = dir;
            Angle = angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cone3f(Cone3f o)
        {
            Origin = o.Origin;
            Direction = o.Direction;
            Angle = o.Angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cone3f(Cone3d o)
        {
            Origin = (V3f)o.Origin;
            Direction = (V3f)o.Direction;
            Angle = (float)o.Angle;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Cone3f(Cone3d c)
            => new Cone3f(c);

        #endregion

        #region Constants

        public static Cone3f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Cone3f(V3f.NaN, V3f.Zero, 0);
        }

        #endregion

        #region Properties

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction != V3f.Zero;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction == V3f.Zero;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cone3f a, Cone3f b)
            => (a.Origin == b.Origin) && (a.Direction == b.Direction) && (a.Angle == b.Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cone3f a, Cone3f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Origin, Direction, Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Cone3f other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction) && Angle.Equals(other.Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Cone3f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Origin, Direction, Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cone3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Cone3f(V3f.Parse(x[0]), V3f.Parse(x[1]), float.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

        #region Operations

        /// <summary>
        /// if zero, point is located on cone
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetDistance(V3f point) => Vec.Distance(point, GetClosestPoint(point));

        public Circle3f GetCircle(float height)
        {
            //circle along axis
            var dirLength = height;
            var radius = GetRadius(height);
            var dir = Direction.Normalized * dirLength;
            return new Circle3f(Origin + dir, dir.Normalized, radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray3f GetAxis() => new Ray3f(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetHeight(V3f position)
            => new Ray3f(Origin, Direction).GetTOfProjectedPoint(position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetRadius(float height) => height * Angle.Sin() / Angle.Cos();

        public V3f GetClosestPoint(V3f point)
        {
            var ray = new Ray3f(Origin, Direction);
            var cp = point.GetClosestPointOn(ray);
            var radius = GetRadius(GetHeight(point));
            var dir = (point - cp).Normalized * radius;

            var p0 = cp + dir;
            var p1 = point.GetClosestPointOn(new Ray3f(Origin, (p0 - Origin).Normalized));

            return (Vec.Distance(point, p1) < Vec.Distance(point, p0)) ? p1 : p0;
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Cone3f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cone3f a, Cone3f b, float tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance) &&
            ApproximateEquals(a.Angle, b.Angle, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Cone3f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cone3f a, Cone3f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region ObliqueCone3d

    /// <summary>
    /// An oblique cone in 3-space represented by its origin (apex) and base circle.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ObliqueCone3d : IEquatable<ObliqueCone3d>
    {
        [DataMember]
        public readonly V3d Origin;

        [DataMember]
        public readonly Circle3d Circle;

        #region Constructor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObliqueCone3d(V3d o, Circle3d c)
        {
            Origin = o;
            Circle = c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObliqueCone3d(ObliqueCone3d o)
        {
            Origin = o.Origin;
            Circle = o.Circle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObliqueCone3d(ObliqueCone3f o)
        {
            Origin = (V3d)o.Origin;
            Circle = (Circle3d)o.Circle;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ObliqueCone3d(ObliqueCone3f c)
            => new ObliqueCone3d(c);

        #endregion

        #region Constants

        public static ObliqueCone3d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ObliqueCone3d(V3d.NaN, Circle3d.Invalid);
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
        public static bool operator ==(ObliqueCone3d a, ObliqueCone3d b)
            => (a.Origin == b.Origin) && (a.Circle == b.Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ObliqueCone3d a, ObliqueCone3d b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Origin, Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ObliqueCone3d other)
            => Origin.Equals(other.Origin) && Circle.Equals(other.Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is ObliqueCone3d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Circle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    /// <summary>
    /// A cone in 3-space represented by its origin, axis of revolution (Direction), and the angle between axis and outer edge.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Cone3d : IEquatable<Cone3d>, IValidity
    {
        [DataMember]
        public V3d Origin;

        /// <summary>
        /// Axis of revolution.
        /// </summary>
        [DataMember]
        public V3d Direction;

        /// <summary>
        /// Angle between axis and outer edge (in radians).
        /// </summary>
        [DataMember]
        public double Angle;

        #region Constructor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cone3d(V3d origin, V3d dir, double angle)
        {
            Origin = origin;
            Direction = dir;
            Angle = angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cone3d(Cone3d o)
        {
            Origin = o.Origin;
            Direction = o.Direction;
            Angle = o.Angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cone3d(Cone3f o)
        {
            Origin = (V3d)o.Origin;
            Direction = (V3d)o.Direction;
            Angle = (double)o.Angle;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Cone3d(Cone3f c)
            => new Cone3d(c);

        #endregion

        #region Constants

        public static Cone3d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Cone3d(V3d.NaN, V3d.Zero, 0);
        }

        #endregion

        #region Properties

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction != V3d.Zero;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction == V3d.Zero;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Origin, Direction, Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Cone3d other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction) && Angle.Equals(other.Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Cone3d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Origin, Direction, Angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetDistance(V3d point) => Vec.Distance(point, GetClosestPoint(point));

        public Circle3d GetCircle(double height)
        {
            //circle along axis
            var dirLength = height;
            var radius = GetRadius(height);
            var dir = Direction.Normalized * dirLength;
            return new Circle3d(Origin + dir, dir.Normalized, radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray3d GetAxis() => new Ray3d(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetHeight(V3d position)
            => new Ray3d(Origin, Direction).GetTOfProjectedPoint(position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

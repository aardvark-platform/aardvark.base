using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Ray2f

    /// <summary>
    /// A two-dimensional ray with an origin and a direction.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ray2f : IEquatable<Ray2f>, IValidity, IBoundingBox2f
    {
        [DataMember]
        public V2f Origin;
        [DataMember]
        public V2f Direction;

        #region Constructors

        /// <summary>
        /// Creates Ray from origin point and directional vector
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2f(V2f origin, V2f direction)
        {
            Origin = origin;
            Direction = direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2f(Ray2f o)
        {
            Origin = o.Origin;
            Direction = o.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2f(Ray2d o)
        {
            Origin = (V2f)o.Origin;
            Direction = (V2f)o.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ray2f FromEndPoints(V2f origin, V2f target)
            => new Ray2f(origin, target - origin);

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ray2f(Ray2d c)
            => new Ray2f(c);

        #endregion

        #region Constants

        /// <summary>
        /// An invalid ray has a zero direction.
        /// </summary>
        public static Ray2f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ray2f(V2f.NaN, V2f.Zero);
        }

        #endregion

        #region Properties

        /// <summary>
        /// A ray is valid if its direction is non-zero.
        /// </summary>
        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction != V2f.Zero;
        }

        /// <summary>
        /// A ray is invalid if its direction is zero.
        /// </summary>
        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction == V2f.Zero;
        }

        /// <summary>
        /// Returns true if either the origin or the direction contains any NaN value.
        /// </summary>
        public readonly bool AnyNaN
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Origin.AnyNaN || Direction.AnyNaN;
        }

        public readonly Line2f Line2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Line2f(Origin, Origin + Direction);
        }

        public readonly Plane2f Plane2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2f(new V2f(-Direction.Y, Direction.X), Origin); // Direction.Rot90
        }

        public readonly Ray2f Reversed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ray2f(Origin, -Direction);
        }

        #endregion

        #region Ray Arithmetics

        /// <summary>
        /// Gets the point on the ray that is t * direction from origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2f GetPointOnRay(float t) => (Origin + Direction * t);

        /// <summary>
        /// Gets segment on the ray starting at range.Min * direction from origin
        /// and ending at range.Max * direction from origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Line2f GetLine2fOnRay(Range1f range)
            => new Line2f(Origin + Direction * range.Min, Origin + Direction * range.Max);

        /// <summary>
        /// Gets segment on the ray starting at tMin * direction from origin
        /// and ending at tMax * direction from origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Line2f GetLine2fOnRay(float tMin, float tMax)
            => new Line2f(Origin + Direction * tMin, Origin + Direction * tMax);

        /// <summary>
        /// Gets the t for a point p on this ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetT(V2f p)
        {
            var v = p - Origin;
            return (Direction.X.Abs() > Direction.Y.Abs())
                ? (v.X / Direction.X)
                : (v.Y / Direction.Y);
        }

        /// <summary>
        /// Gets the point on the ray that is closest to the given point.
        /// Ray direction must be normalized (length 1).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2f GetClosestPointOnRay(V2f p)
            => Origin + Direction * Direction.Dot(p - Origin);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetDistanceToRay(V2f p)
        {
            var f = GetClosestPointOnRay(p);
            return (f - p).Length;
        }

        public readonly V2f Intersect(Ray2f r)
        {
            V2f a = r.Origin - Origin;
            if (a.Abs().AllSmaller(Constant<float>.PositiveTinyValue))
                return Origin; // Early exit when rays have same origin

            float cross = Direction.Dot270(r.Direction);
            if (!Fun.IsTiny(cross)) // Rays not parallel
                return Origin + Direction * r.Direction.Dot90(a) / cross;
            else // Rays are parallel
                return V2f.NaN;
        }

        public readonly V2f Intersect(V2f dirVector)
        {
            if (Origin.Abs().AllSmaller(Constant<float>.PositiveTinyValue))
                return Origin; // Early exit when rays have same origin

            float cross = Direction.Dot270(dirVector);
            if (!Fun.IsTiny(cross)) // Rays not parallel
                return Origin + Direction * dirVector.Dot270(Origin) / cross;
            else // Rays are parallel
                return V2f.NaN;
        }

        /// <summary>
        /// Returns the angle between this and the given <see cref="Ray2f"/> in radians.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float AngleBetweenFast(Ray2f r)
            => Direction.AngleBetweenFast(r.Direction);

        /// <summary>
        /// Returns the angle between this and the given <see cref="Ray2f"/> in radians using a numerically stable algorithm.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float AngleBetween(Ray2f r)
            => Direction.AngleBetween(r.Direction);

        /// <summary>
        /// Returns a signed value where left is negative and right positive.
        /// The magnitude is equal to the float size of the triangle the ray + direction and p.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetPointSide(V2f p) => Direction.Dot90(p - Origin);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ray2f a, Ray2f b)
            => (a.Origin == b.Origin) && (a.Direction == b.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ray2f a, Ray2f b)
            => !((a.Origin == b.Origin) && (a.Direction == b.Direction));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LexicalCompare(Ray2f other)
        {
            var cmp = Origin.LexicalCompare(other.Origin);
            if (cmp != 0) return cmp;
            return Direction.LexicalCompare(other.Direction);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Calculates Hash-code of the given ray.
        /// </summary>
        /// <returns>Hash-code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.GetCombined(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Ray2f other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Ray2f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ray2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ray2f(V2f.Parse(x[0]), V2f.Parse(x[1]));
        }

        #endregion

        #region IBoundingBox2f

        public readonly Box2f BoundingBox2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Box2f.FromPoints(Origin, Direction + Origin);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ray2f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ray2f a, Ray2f b, float tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ray2f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ray2f a, Ray2f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region FastRay2f

    /// <summary>
    /// A fast ray contains a ray and a number of precomputed flags and
    /// fields for fast intersection computation with bounding boxes and
    /// other axis-aligned sturctures such as kd-Trees.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct FastRay2f
    {
        [DataMember]
        public readonly Ray2f Ray;
        [DataMember]
        public readonly DirFlags DirFlags;
        [DataMember]
        public readonly V2f InvDir;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2f(Ray2f ray)
        {
            Ray = ray;
            DirFlags = ray.Direction.DirFlags();
            InvDir = 1 / ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2f(V2f origin, V2f direction)
            : this(new Ray2f(origin, direction))
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2f(FastRay2f o)
        {
            Ray = o.Ray;
            DirFlags = o.DirFlags;
            InvDir = o.InvDir;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2f(FastRay2d o)
        {
            Ray = (Ray2f)o.Ray;
            DirFlags = o.DirFlags;
            InvDir = (V2f)o.InvDir;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator FastRay2f(FastRay2d r)
            => new FastRay2f(r);

        #endregion

        #region Ray Arithmetics

        public bool Intersects(
            Box2f box,
            ref float tmin,
            ref float tmax
        )
        {
            var dirFlags = DirFlags;

            if ((dirFlags & DirFlags.PositiveX) != 0)
            {
                {
                    float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else    // ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                {
                    float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else    // ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        /// <summary>
        /// This variant of the intersection method returns the affected
        /// planes of the box if the box was hit.
        /// </summary>
        public bool Intersects(
            Box2f box,
            ref float tmin,
            ref float tmax,
            out Box.Flags tminFlags,
            out Box.Flags tmaxFlags
        )
        {
            var dirFlags = DirFlags;
            tminFlags = Box.Flags.None;
            tmaxFlags = Box.Flags.None;

            if ((dirFlags & DirFlags.PositiveX) != 0)
            {
                {
                    float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
                }
                {
                    float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
                }
                {
                    float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
                }
            }
            else    // ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                {
                    float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
                }
                {
                    float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
                }
                {
                    float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
                }
            }
            else    // ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        #endregion
    }

    #endregion

    #region Ray2d

    /// <summary>
    /// A two-dimensional ray with an origin and a direction.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ray2d : IEquatable<Ray2d>, IValidity, IBoundingBox2d
    {
        [DataMember]
        public V2d Origin;
        [DataMember]
        public V2d Direction;

        #region Constructors

        /// <summary>
        /// Creates Ray from origin point and directional vector
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2d(V2d origin, V2d direction)
        {
            Origin = origin;
            Direction = direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2d(Ray2d o)
        {
            Origin = o.Origin;
            Direction = o.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ray2d(Ray2f o)
        {
            Origin = (V2d)o.Origin;
            Direction = (V2d)o.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ray2d FromEndPoints(V2d origin, V2d target)
            => new Ray2d(origin, target - origin);

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ray2d(Ray2f c)
            => new Ray2d(c);

        #endregion

        #region Constants

        /// <summary>
        /// An invalid ray has a zero direction.
        /// </summary>
        public static Ray2d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ray2d(V2d.NaN, V2d.Zero);
        }

        #endregion

        #region Properties

        /// <summary>
        /// A ray is valid if its direction is non-zero.
        /// </summary>
        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction != V2d.Zero;
        }

        /// <summary>
        /// A ray is invalid if its direction is zero.
        /// </summary>
        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Direction == V2d.Zero;
        }

        /// <summary>
        /// Returns true if either the origin or the direction contains any NaN value.
        /// </summary>
        public readonly bool AnyNaN
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Origin.AnyNaN || Direction.AnyNaN;
        }

        public readonly Line2d Line2d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Line2d(Origin, Origin + Direction);
        }

        public readonly Plane2d Plane2d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2d(new V2d(-Direction.Y, Direction.X), Origin); // Direction.Rot90
        }

        public readonly Ray2d Reversed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ray2d(Origin, -Direction);
        }

        #endregion

        #region Ray Arithmetics

        /// <summary>
        /// Gets the point on the ray that is t * direction from origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2d GetPointOnRay(double t) => (Origin + Direction * t);

        /// <summary>
        /// Gets segment on the ray starting at range.Min * direction from origin
        /// and ending at range.Max * direction from origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Line2d GetLine2dOnRay(Range1d range)
            => new Line2d(Origin + Direction * range.Min, Origin + Direction * range.Max);

        /// <summary>
        /// Gets segment on the ray starting at tMin * direction from origin
        /// and ending at tMax * direction from origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Line2d GetLine2dOnRay(double tMin, double tMax)
            => new Line2d(Origin + Direction * tMin, Origin + Direction * tMax);

        /// <summary>
        /// Gets the t for a point p on this ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double GetT(V2d p)
        {
            var v = p - Origin;
            return (Direction.X.Abs() > Direction.Y.Abs())
                ? (v.X / Direction.X)
                : (v.Y / Direction.Y);
        }

        /// <summary>
        /// Gets the point on the ray that is closest to the given point.
        /// Ray direction must be normalized (length 1).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V2d GetClosestPointOnRay(V2d p)
            => Origin + Direction * Direction.Dot(p - Origin);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double GetDistanceToRay(V2d p)
        {
            var f = GetClosestPointOnRay(p);
            return (f - p).Length;
        }

        public readonly V2d Intersect(Ray2d r)
        {
            V2d a = r.Origin - Origin;
            if (a.Abs().AllSmaller(Constant<double>.PositiveTinyValue))
                return Origin; // Early exit when rays have same origin

            double cross = Direction.Dot270(r.Direction);
            if (!Fun.IsTiny(cross)) // Rays not parallel
                return Origin + Direction * r.Direction.Dot90(a) / cross;
            else // Rays are parallel
                return V2d.NaN;
        }

        public readonly V2d Intersect(V2d dirVector)
        {
            if (Origin.Abs().AllSmaller(Constant<double>.PositiveTinyValue))
                return Origin; // Early exit when rays have same origin

            double cross = Direction.Dot270(dirVector);
            if (!Fun.IsTiny(cross)) // Rays not parallel
                return Origin + Direction * dirVector.Dot270(Origin) / cross;
            else // Rays are parallel
                return V2d.NaN;
        }

        /// <summary>
        /// Returns the angle between this and the given <see cref="Ray2d"/> in radians.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double AngleBetweenFast(Ray2d r)
            => Direction.AngleBetweenFast(r.Direction);

        /// <summary>
        /// Returns the angle between this and the given <see cref="Ray2d"/> in radians using a numerically stable algorithm.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double AngleBetween(Ray2d r)
            => Direction.AngleBetween(r.Direction);

        /// <summary>
        /// Returns a signed value where left is negative and right positive.
        /// The magnitude is equal to the double size of the triangle the ray + direction and p.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double GetPointSide(V2d p) => Direction.Dot90(p - Origin);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ray2d a, Ray2d b)
            => (a.Origin == b.Origin) && (a.Direction == b.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ray2d a, Ray2d b)
            => !((a.Origin == b.Origin) && (a.Direction == b.Direction));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LexicalCompare(Ray2d other)
        {
            var cmp = Origin.LexicalCompare(other.Origin);
            if (cmp != 0) return cmp;
            return Direction.LexicalCompare(other.Direction);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Calculates Hash-code of the given ray.
        /// </summary>
        /// <returns>Hash-code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.GetCombined(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Ray2d other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Ray2d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ray2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ray2d(V2d.Parse(x[0]), V2d.Parse(x[1]));
        }

        #endregion

        #region IBoundingBox2d

        public readonly Box2d BoundingBox2d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Box2d.FromPoints(Origin, Direction + Origin);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ray2d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ray2d a, Ray2d b, double tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ray2d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ray2d a, Ray2d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

    #region FastRay2d

    /// <summary>
    /// A fast ray contains a ray and a number of precomputed flags and
    /// fields for fast intersection computation with bounding boxes and
    /// other axis-aligned sturctures such as kd-Trees.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct FastRay2d
    {
        [DataMember]
        public readonly Ray2d Ray;
        [DataMember]
        public readonly DirFlags DirFlags;
        [DataMember]
        public readonly V2d InvDir;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2d(Ray2d ray)
        {
            Ray = ray;
            DirFlags = ray.Direction.DirFlags();
            InvDir = 1 / ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2d(V2d origin, V2d direction)
            : this(new Ray2d(origin, direction))
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2d(FastRay2d o)
        {
            Ray = o.Ray;
            DirFlags = o.DirFlags;
            InvDir = o.InvDir;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastRay2d(FastRay2f o)
        {
            Ray = (Ray2d)o.Ray;
            DirFlags = o.DirFlags;
            InvDir = (V2d)o.InvDir;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator FastRay2d(FastRay2f r)
            => new FastRay2d(r);

        #endregion

        #region Ray Arithmetics

        public bool Intersects(
            Box2d box,
            ref double tmin,
            ref double tmax
        )
        {
            var dirFlags = DirFlags;

            if ((dirFlags & DirFlags.PositiveX) != 0)
            {
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else    // ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else    // ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        /// <summary>
        /// This variant of the intersection method returns the affected
        /// planes of the box if the box was hit.
        /// </summary>
        public bool Intersects(
            Box2d box,
            ref double tmin,
            ref double tmax,
            out Box.Flags tminFlags,
            out Box.Flags tmaxFlags
        )
        {
            var dirFlags = DirFlags;
            tminFlags = Box.Flags.None;
            tmaxFlags = Box.Flags.None;

            if ((dirFlags & DirFlags.PositiveX) != 0)
            {
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
                }
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
                }
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
                }
            }
            else    // ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
                }
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
                }
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
                }
            }
            else    // ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        #endregion
    }

    #endregion

}

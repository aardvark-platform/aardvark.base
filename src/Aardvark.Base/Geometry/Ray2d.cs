using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// A two-dimensional ray with an origin and a direction.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray2d : IValidity, IBoundingBox2d
    {
        public V2d Origin;
        public V2d Direction;

        #region Constructors

        /// <summary>
        /// Creates Ray from origin point and directional vector
        /// </summary>
        public Ray2d(V2d origin, V2d direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public static Ray2d FromEndPoints(V2d origin, V2d target)
            => new Ray2d(origin, target - origin);

        #endregion

        #region Constants

        /// <summary>
        /// An invalid ray has a zero direction.
        /// </summary>
        public static readonly Ray2d Invalid = new Ray2d(V2d.NaN, V2d.Zero);

        #endregion

        #region Properties

        /// <summary>
        /// A ray is valid if its direction is non-zero.
        /// </summary>
        public bool IsValid => Direction != V2d.Zero;

        /// <summary>
        /// A ray is invalid if its direction is zero.
        /// </summary>
        public bool IsInvalid => Direction == V2d.Zero;

        /// <summary>
        /// Returns true if either the origin or the direction contains any NaN value.
        /// </summary>
        public bool AnyNaN => Origin.AnyNaN || Direction.AnyNaN;

        public Line2d Line2d => new Line2d(Origin, Origin + Direction);

        public Plane2d Plane2d => new Plane2d(new V2d(-Direction.Y, Direction.X), Origin); // Direction.Rot90

        public Ray2d Reversed => new Ray2d(Origin, -Direction);

        #endregion

        #region Ray Arithmetics

        /// <summary>
        /// Gets the point on the ray that is t * direction from origin.
        /// </summary>
        public V2d GetPointOnRay(double t) => (Origin + Direction * t);

        /// <summary>
        /// Gets segment on the ray starting at range.Min * direction from origin
        /// and ending at range.Max * direction from origin.
        /// </summary>
        public Line2d GetLine2dOnRay(Range1d range)
            => new Line2d(Origin + Direction * range.Min, Origin + Direction * range.Max);

        /// <summary>
        /// Gets segment on the ray starting at tMin * direction from origin
        /// and ending at tMax * direction from origin.
        /// </summary>
        public Line2d GetLine2dOnRay(double tMin, double tMax)
            => new Line2d(Origin + Direction * tMin, Origin + Direction * tMax);

        /// <summary>
        /// Gets the t for a point p on this ray. 
        /// </summary>
        public double GetT(V2d p)
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
        public V2d GetClosestPointOnRay(V2d p)
            => Origin + Direction * Direction.Dot(p - Origin);

        public double GetDistanceToRay(V2d p)
        {
            var f = GetClosestPointOnRay(p);
            return (f - p).Length;
        }

        public V2d Intersect(Ray2d r)
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

        public V2d Intersect(V2d dirVector)
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
        public double AngleBetweenFast(Ray2d r)
            => Direction.AngleBetweenFast(r.Direction);

        /// <summary>
        /// Returns the angle between this and the given <see cref="Ray2d"/> in radians using a numerically stable algorithm.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double AngleBetween(Ray2d r)
            => Direction.AngleBetween(r.Direction);

        /// <summary>
        /// Returns a signed value where left is negative and right positive.
        /// The magnitude is equal to the double size of the triangle the ray + direction and p.
        /// </summary>
        public double GetPointSide(V2d p) => Direction.Dot90(p - Origin);

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ray2d a, Ray2d b)
            => (a.Origin == b.Origin) && (a.Direction == b.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ray2d a, Ray2d b)
            => !((a.Origin == b.Origin) && (a.Direction == b.Direction));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int LexicalCompare(Ray2d other)
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
        public override int GetHashCode() => HashCode.GetCombined(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Ray2d other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

        public override bool Equals(object other)
            => (other is Ray2d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

        public static Ray2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ray2d(V2d.Parse(x[0]), V2d.Parse(x[1]));
        }

        #endregion

        #region IBoundingBox2d

        public Box2d BoundingBox2d => Box2d.FromPoints(Origin, Direction + Origin);

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

    /// <summary>
    /// A fast ray contains a ray and a number of precomputed flags and
    /// fields for fast intersection computation with bounding boxes and
    /// other axis-aligned sturctures such as kd-Trees.
    /// </summary>
    public struct FastRay2d
    {
        public readonly Ray2d Ray;
        public readonly DirFlags DirFlags;
        public readonly V2d InvDir;

        #region Constructors

        public FastRay2d(Ray2d ray)
        {
            Ray = ray;
            DirFlags = ray.Direction.DirFlags();
            InvDir = 1.0 / ray.Direction;
        }

        public FastRay2d(V2d origin, V2d direction)
            : this(new Ray2d(origin, direction))
        { }

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
}

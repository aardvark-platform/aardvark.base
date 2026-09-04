using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var ray3t = "Ray3" + tc;
    //#   var ray3t2 = "Ray3" + tc2;
    //#   var rayhit3t = "RayHit3" + tc;
    //#   var rayhit3t2 = "RayHit3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var m33t = "M33" + tc;
    //#   var m44t = "M44" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var line3t = "Line3" + tc;
    //#   var triangle3t = "Triangle3" + tc;
    //#   var quad3t = "Quad3" + tc;
    //#   var sphere3t = "Sphere3" + tc;
    //#   var circle3t = "Circle3" + tc;
    //#   var cylinder3t = "Cylinder3" + tc;
    //#   var iboundingbox = "IBoundingBox3" + tc;
    //#   var rot3t = "Rot3" + tc;
    //#   var scale3t = "Scale3" + tc;
    //#   var shift3t = "Shift3" + tc;
    //#   var euclidean3t = "Euclidean3" + tc;
    //#   var similarity3t = "Similarity3" + tc;
    //#   var affine3t = "Affine3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var eps = isDouble ? "1e-7" : "1e-4f";
    #region __ray3t__

    /// <summary>
    /// A three-dimensional ray with an origin and a direction.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __ray3t__ : IEquatable<__ray3t__>, IValidity, __iboundingbox__
    {
        [DataMember]
        public __v3t__ Origin;
        [DataMember]
        public __v3t__ Direction;

        #region Constructors

        /// <summary>
        /// Creates Ray from origin point and directional vector
        /// </summary>
        public __ray3t__(__v3t__ origin, __v3t__ direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public static __ray3t__ FromEndPoints(__v3t__ origin, __v3t__ target) => new __ray3t__(origin, target - origin);

        #endregion

        #region Constants

        /// <summary>
        /// An invalid ray has a zero direction.
        /// </summary>
        public static readonly __ray3t__ Invalid = new __ray3t__(__v3t__.NaN, __v3t__.Zero);

        #endregion

        #region Properties

        /// <summary>
        /// A ray is valid if its direction is non-zero.
        /// </summary>
        public readonly bool IsValid { get { return Direction != __v3t__.Zero; } }

        /// <summary>
        /// A ray is invalid if its direction is zero.
        /// </summary>
        public readonly bool IsInvalid { get { return Direction == __v3t__.Zero; } }

        /// <summary>
        /// Returns true if either the origin or the direction contains any NaN value.
        /// </summary>
        public readonly bool AnyNaN { get { return Origin.AnyNaN || Direction.AnyNaN; } }

        /// <summary>
        /// Line segment from origin to origin + direction.
        /// </summary>
        public readonly __line3t__ __line3t__ => new __line3t__(Origin, Origin + Direction);

        /// <summary>
        /// Returns new ray with flipped direction.
        /// </summary>
        public readonly __ray3t__ Reversed => new __ray3t__(Origin, -Direction);

        /// <summary>
        /// Returns the ray with its directional normalized.
        /// </summary>
        public readonly __ray3t__ Normalized => new(Origin, Direction.Normalized);

        #endregion

        #region Ray Arithmetics

        /// <summary>
        /// Gets the point on the ray that is t * Direction from Origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __v3t__ GetPointOnRay(__ftype__ t) => Origin + Direction * t;

        /// <summary>
        /// Gets the t for a point p on this ray.
        /// </summary>
        public readonly __ftype__ GetT(__v3t__ p)
        {
            var v = p - Origin;
            var d = Direction.Abs();

            if (d.X > d.Y)
                return (d.X > d.Z) ? (v.X / Direction.X) : (v.Z / Direction.Z);
            else
                return (d.Y > d.Z) ? (v.Y / Direction.Y) : (v.Z / Direction.Z);
        }

        /// <summary>
        /// Gets the t of the closest point on the ray for any point p.
        /// </summary>
        public readonly __ftype__ GetTOfProjectedPoint(__v3t__ p)
        {
            var v = p - Origin;
            return v.Dot(Direction) / Direction.LengthSquared;
        }

        /// <summary>
        /// Returns the ray transformed with the given matrix.
        /// This method is only valid for similarity transformations (uniform scale).
        /// </summary>
        public readonly __ray3t__ Transformed(__m44t__ mat)
            => new(mat.TransformPos(Origin), mat.TransformDir(Direction));

        /// <summary>
        /// Returns the ray transformed with the given <see cref="__rot3t__"/> transformation.
        /// </summary>
        public readonly __ray3t__ Transformed(__rot3t__ transform)
            => new(transform.Transform(Origin), transform.Transform(Direction));

        /// <summary>
        /// Returns the ray transformed with the given <see cref="__scale3t__"/> transformation.
        /// </summary>
        public readonly __ray3t__ Transformed(__scale3t__ transform)
            => new(transform.Transform(Origin), transform.Transform(Direction));

        /// <summary>
        /// Returns the ray transformed with the given <see cref="__shift3t__"/> transformation.
        /// </summary>
        public readonly __ray3t__ Transformed(__shift3t__ transform)
            => new(transform.Transform(Origin), Direction);

        /// <summary>
        /// Returns the ray transformed with the given <see cref="__euclidean3t__"/> transformation.
        /// </summary>
        public readonly __ray3t__ Transformed(__euclidean3t__ transform)
            => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

        /// <summary>
        /// Returns the ray transformed with the given <see cref="__similarity3t__"/> transformation.
        /// </summary>
        public readonly __ray3t__ Transformed(__similarity3t__ transform)
            => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

        /// <summary>
        /// Returns the ray transformed with the given <see cref="__affine3t__"/> transformation.
        /// </summary>
        public readonly __ray3t__ Transformed(__affine3t__ transform)
            => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

        /// <summary>
        /// Returns the angle between this and the given <see cref="__ray3t__"/> in radians.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __ftype__ AngleBetweenFast(__ray3t__ r)
            => Direction.AngleBetweenFast(r.Direction);

        /// <summary>
        /// Returns the angle between this and the given <see cref="__ray3t__"/> in radians using a numerically stable algorithm.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __ftype__ AngleBetween(__ray3t__ r)
            => Direction.AngleBetween(r.Direction);

        #endregion

        #region Ray hit intersections

        #region Private functions

        private readonly bool ComputeHit(
              __ftype__ t,
              __ftype__ tmin, __ftype__ tmax,
              ref __rayhit3t__ hit)
        {
            if (t >= tmin)
            {
                if (t < tmax && t < hit.T)
                {
                    hit.T = t;
                    hit.Point = GetPointOnRay(t);
                    hit.Coord = V2d.NaN;
                    hit.BackSide = false;
                    return true;
                }
                return false;
            }
            return false;
        }

        private readonly bool GetClosestHit(
                __ftype__ t1, __ftype__ t2,
                __ftype__ tmin, __ftype__ tmax,
                ref __rayhit3t__ hit)
        {
            return t1 < t2
                  ? ProcessHits(t1, t2, tmin, tmax, ref hit)
                  : ProcessHits(t2, t1, tmin, tmax, ref hit);
        }


        private readonly bool ProcessHits(
                __ftype__ t1, __ftype__ t2,
                __ftype__ tmin, __ftype__ tmax,
                ref __rayhit3t__ hit)
        {
            if (t1 >= tmin)
            {
                if (t1 < tmax && t1 < hit.T)
                {
                    hit.T = t1;
                    hit.Point = GetPointOnRay(t1);
                    hit.Coord = V2d.NaN;
                    hit.BackSide = false;
                    return true;
                }
                return false;
            }
            if (t2 >= tmin)
            {
                if (t2 < tmax && t2 < hit.T)
                {
                    hit.T = t2;
                    hit.Point = GetPointOnRay(t2);
                    hit.Coord = V2d.NaN;
                    hit.BackSide = true;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Ray-Ray hit intersection

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        public readonly bool Hits(__ray3t__ ray, ref __rayhit3t__ hit)
            => HitsRay(ray, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public readonly bool Hits(__ray3t__ ray, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => HitsRay(ray, tmin, tmax, ref hit);

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public readonly bool HitsRay(__ray3t__ ray, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
        {
            __v3t__ d = Origin - ray.Origin;
            __v3t__ u = Direction;
            __v3t__ v = ray.Direction;
            __v3t__ n = u.Cross(v);

            if (Fun.IsTiny(d.Length)) return true;
            else if (Fun.IsTiny(u.Cross(v).Length)) return false;
            else
            {
                //-t0*u + t1*v + t2*n == d
                //M = {-u,v,n}
                //M*{t0,t1,t2}T == d
                //{t0,t1,t2}T == M^-1 * d

                __m33t__ M = new __m33t__
                {
                    C0 = -u,
                    C1 = v,
                    C2 = n
                };

                if (M.Invertible)
                {
                    __v3t__ t = M.Inverse * d;
                    if (Fun.IsTiny(t.Z))
                    {
                        ProcessHits(t.X, __ftype__.MaxValue, tmin, tmax, ref hit);
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
        }

        #endregion

        #region Ray-Triangle hit intersection

        /// <summary>
        /// Returns true if the ray hits the triangle before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__triangle3t__ triangle, ref __rayhit3t__ hit)
            => HitsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the triangle. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__triangle3t__ triangle, out __ftype__ t)
            => HitsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with __rayhit3t__.MaxRange.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__triangle3t__ triangle, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => HitsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                tmin, tmax, ref hit);

        /// <summary>
        /// Returns true if the ray hits the triangle within [0, __ftype__.MaxValue]
        /// and before the parameter value contained in the supplied hit. Detailed
        /// information about the hit is returned in the supplied hit. In order to
        /// obtain all potential hits, the supplied hit can be initialized with
        /// __rayhit3t__.MaxRange. Degenerated triangles will not result in an intersection
        /// even if any edge is hit exactly.
        /// </summary>
        public readonly bool HitsTriangle(__v3t__ p0, __v3t__ p1, __v3t__ p2, ref __rayhit3t__ hit)
            => HitsTriangle(p0, p1, p2, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with __rayhit3t__.MaxRange.
        /// Degenerated triangles will not result in an intersection even if
        /// any edge is hit exactly.
        /// </summary>
        public readonly bool HitsTriangle(
            __v3t__ p0, __v3t__ p1, __v3t__ p2,
            __ftype__ tmin, __ftype__ tmax,
            ref __rayhit3t__ hit
            )
        {
            __v3t__ edge01 = p1 - p0;
            __v3t__ edge02 = p2 - p0;
            __v3t__ plane = Vec.Cross(Direction, edge02);
            __ftype__ det = Vec.Dot(edge01, plane);
            if (det > -__eps__ && det < __eps__) return false;
            // ray ~= paralell / Triangle
            __v3t__ tv = Origin - p0;
            det = 1 / det;  // det is now inverse det
            __ftype__ u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) return false;
            plane = Vec.Cross(tv, edge01); // plane is now qv
            __ftype__ v = Vec.Dot(Direction, plane) * det;
            if (v < 0 || u + v > 1) return false;
            __ftype__ t = Vec.Dot(edge02, plane) * det;
            if (t < tmin || t >= tmax || t >= hit.T) return false;
            hit.T = t;
            hit.Point = Origin + t * Direction;
            hit.Coord.X = u; hit.Coord.Y = v;
            hit.BackSide = (det < 0);
            return true;
        }

        /// <summary>
        /// Returns true if the ray hits the triangle. Degenerated triangles
        /// will not result in an intersection even if any edge is hit exactly.
        /// A hit with this overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsTriangle(__v3t__ p0, __v3t__ p1, __v3t__ p2, out __ftype__ t)
            => HitsTriangle(p0, p1, p2, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval. Degenerated triangles will not result in an
        /// intersection even if any edge is hit exactly.
        /// </summary>
        public readonly bool HitsTriangle(
            __v3t__ p0, __v3t__ p1, __v3t__ p2,
            __ftype__ tmin, __ftype__ tmax,
            out __ftype__ t
            )
        {
            __v3t__ edge01 = p1 - p0;
            __v3t__ edge02 = p2 - p0;
            __v3t__ plane = Vec.Cross(Direction, edge02);
            __ftype__ det = Vec.Dot(edge01, plane);
            t = __ftype__.NaN;
            if (det > -__eps__ && det < __eps__) return false;
            // ray ~= paralell / Triangle
            __v3t__ tv = Origin - p0;
            det = 1 / det;  // det is now inverse det
            __ftype__ u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) return false;
            plane = Vec.Cross(tv, edge01); // plane is now qv
            __ftype__ v = Vec.Dot(Direction, plane) * det;
            if (v < 0 || u + v > 1) return false;
            t = Vec.Dot(edge02, plane) * det;
            return t >= tmin && t <= tmax;
        }

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with __rayhit3t__.MaxRange.
        /// </summary>
        public readonly bool HitsTrianglePointAndEdges(
            __v3t__ p0, __v3t__ edge01, __v3t__ edge02,
            __ftype__ tmin, __ftype__ tmax,
            ref __rayhit3t__ hit
            )
        {
            __v3t__ plane = Vec.Cross(Direction, edge02);
            __ftype__ det = Vec.Dot(edge01, plane);
            if (det > -__eps__ && det < __eps__) return false;
            // ray ~= paralell / Triangle
            __v3t__ tv = Origin - p0;
            det = 1 / det;  // det is now inverse det
            __ftype__ u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) return false;
            plane = Vec.Cross(tv, edge01); // plane is now qv
            __ftype__ v = Vec.Dot(Direction, plane) * det;
            if (v < 0 || u + v > 1) return false;
            __ftype__ t = Vec.Dot(edge02, plane) * det;
            if (t < tmin || t >= tmax || t >= hit.T) return false;
            hit.T = t;
            hit.Point = Origin + t * Direction;
            hit.Coord.X = u; hit.Coord.Y = v;
            hit.BackSide = (det < 0);
            return true;
        }

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with __rayhit3t__.MaxRange.
        /// </summary>
        public readonly bool HitsTrianglePointAndEdges(
            __v3t__ p0, __v3t__ edge01, __v3t__ edge02,
            __ftype__ tmin, __ftype__ tmax,
            out __ftype__ t
            )
        {
            __v3t__ plane = Vec.Cross(Direction, edge02);
            __ftype__ det = Vec.Dot(edge01, plane);
            t = __ftype__.NaN;
            if (det > -__eps__ && det < __eps__) return false;
            // ray ~= paralell / Triangle
            __v3t__ tv = Origin - p0;
            det = 1 / det;  // det is now inverse det
            __ftype__ u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) return false;
            plane = Vec.Cross(tv, edge01); // plane is now qv
            __ftype__ v = Vec.Dot(Direction, plane) * det;
            if (v < 0 || u + v > 1) return false;
            t = Vec.Dot(edge02, plane) * det;
            return t >= tmin && t <= tmax;
        }

        #endregion

        #region Ray-Quad hit intersection

        /// <summary>
        /// Returns true if the ray hits the quad before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit. In order to obtain all
        /// potential hits, the supplied hit can be initialized with
        /// __rayhit3t__.MaxRange. A hit with this overload is considered
        /// for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__quad3t__ quad, ref __rayhit3t__ hit) => HitsQuad(
            quad.P0, quad.P1, quad.P2, quad.P3,
            0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with __rayhit3t__.MaxRange.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__quad3t__ quad, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit) => HitsQuad(
            quad.P0, quad.P1, quad.P2, quad.P3,
            tmin, tmax, ref hit);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. The quad is considered to consist of the
        /// two triangles [p0,p1,p2] and [p0,p2,p3]. Detailed information
        /// about the hit is returned in the supplied hit. In order to obtain
        /// all potential hits, the supplied hit can be initialized with
        /// __rayhit3t__.MaxRange.
        /// </summary>
        public readonly bool HitsQuad(
            __v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3,
            __ftype__ tmin, __ftype__ tmax,
            ref __rayhit3t__ hit
            )
        {
            __v3t__ e02 = p2 - p0;
            bool result = false;
            if (HitsTrianglePointAndEdges(p0, p1 - p0, e02, tmin, tmax,
                                          ref hit))
            {
                hit.Coord.X += hit.Coord.Y;
                result = true;
            }
            if (HitsTrianglePointAndEdges(p0, e02, p3 - p0, tmin, tmax,
                                          ref hit))
            {
                hit.Coord.Y += hit.Coord.X;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied parameter interval.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__quad3t__ quad, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
            => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied parameter interval.
        /// A hit with this overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__quad3t__ quad, out __ftype__ t)
            => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied parameter interval.
        /// A hit with this overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsQuad(__v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3, out __ftype__ t)
            => HitsQuad(p0, p1, p2, p3, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied parameter interval.
        /// </summary>
        public readonly bool HitsQuad(__v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
        {
            __v3t__ e02 = p2 - p0;
            return HitsTrianglePointAndEdges(p0, p1 - p0, e02, tmin, tmax, out t)
                || HitsTrianglePointAndEdges(p0, e02, p3 - p0, tmin, tmax, out t);
        }

        #endregion

        #region Ray-Sphere hit intersection

        /// <summary>
        /// Returns true if the ray hits the sphere given by center and
        /// radius within the supplied parameter interval and before the
        /// parameter value contained in the supplied hit. Note that a
        /// hit is only registered if the front or the backsurface is
        /// encountered within the interval. If there are two valid solutions, the
        /// closest will be returned.
        /// </summary>
        public readonly bool HitsSphere(
                __v3t__ center, __ftype__ radius,
                __ftype__ tmin, __ftype__ tmax,
                ref __rayhit3t__ hit)
        {
            __v3t__ originSubCenter = Origin - center;
            __ftype__ a = Direction.LengthSquared;
            __ftype__ b = Direction.Dot(originSubCenter);
            __ftype__ c = originSubCenter.LengthSquared - radius * radius;

            // --------------------- quadric equation : a t^2  + 2b t + c = 0
            __ftype__ d = b * b - a * c;           // factor 2 was eliminated

            if (d < __ftype__.Epsilon)             // no root ?
                return false;                   // then exit

            if (b > 0)                        // stable way to calculate
                d = -Fun.Sqrt(d) - b;           // the roots of a quadratic
            else                                // equation
                d = Fun.Sqrt(d) - b;

            __ftype__ t1 = d / a;
            __ftype__ t2 = c / d;  // Vieta : t1 * t2 == c/a

            // typically two solutions, either both positive, both negative or mixed
            // -> take closest (if valid) first
            return t1.Abs() < t2.Abs()
                    ? ProcessHits(t1, t2, tmin, tmax, ref hit)
                    : ProcessHits(t2, t1, tmin, tmax, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the
        /// supplied parameter interval and before the parameter value
        /// contained in the supplied hit. Note that a hit is only
        /// registered if the front or the backsurface is encountered
        /// within the interval. If there are two valid solutions, the
        /// closest will be returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__sphere3t__ sphere, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, ref hit);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the
        /// supplied parameter interval and before the parameter value
        /// contained in the supplied hit. Note that a hit is only
        /// registered if the front or the backsurface is encountered
        /// within the interval. If there are two valid solutions, the
        /// closest will be returned. A hit with this overload is
        /// considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__sphere3t__ sphere, ref __rayhit3t__ hit)
            => HitsSphere(sphere.Center, sphere.Radius, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the
        /// supplied parameter interval. Note that a hit is only
        /// registered if the front or the backsurface is encountered
        /// within the interval. If there are two valid solutions, the
        /// closest will be returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__sphere3t__ sphere, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
            => HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere. Note that a hit is
        /// registered if the front or the backsurface is encountered. If there
        /// are two valid solutions, the closest will be returned. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__sphere3t__ sphere, out __ftype__ t)
            => HitsSphere(sphere.Center, sphere.Radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the supplied parameter interval.
        /// Note that a hit is registered if the front or the backsurface is encountered within the
        /// interval. If there are two valid solutions, the closest will be returned. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        public readonly bool HitsSphere(__v3t__ center, __ftype__ radius, out __ftype__ t)
            => HitsSphere(center, radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the supplied parameter interval.
        /// Note that a hit is registered if the front or the backsurface is encountered within the
        /// interval. If there are two valid solutions, the closest will be returned.
        /// </summary>
        public readonly bool HitsSphere(__v3t__ center, __ftype__ radius, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
        {
            var originSubCenter = Origin - center;
            var a = Direction.LengthSquared;
            var b = Direction.Dot(originSubCenter);
            var c = originSubCenter.LengthSquared - radius * radius;

            // --------------------- quadric equation : a t^2  + 2b t + c = 0
            var d = b * b - a * c;              // factor 2 was eliminated

            if (d >= __ftype__.Epsilon)            // no root ? -> exit
            {
                if (b > 0)                    // stable way to calculate
                    d = -Fun.Sqrt(d) - b;       // the roots of a quadratic
                else                            // equation
                    d = Fun.Sqrt(d) - b;

                var t1 = d / a;
                var t2 = c / d;  // Vieta : t1 * t2 == c/a

                // typically two solutions, either both positive, both negative or mixed
                // -> take closest (if valid) first
                if (t2.Abs() < t1.Abs())
                    Fun.Swap(ref t1, ref t2);

                if (t1 >= tmin)
                {
                    if (t1 < tmax)
                    {
                        t = t1;
                        return true;
                    }
                    // return false
                }
                else if (t2 >= tmin)
                {
                    if (t2 < tmax)
                    {
                        t = t2;
                        return true;
                    }
                    // return false
                }
            }

            t = __ftype__.NaN;
            return false;
        }

        #endregion

        #region Ray-Plane hit intersection

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsPlane(__plane3t__ plane, ref __rayhit3t__ hit)
            => HitsPlane(plane, 0, __ftype__.MaxValue, ref hit);

        public readonly bool HitsPlane(__plane3t__ plane, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
        {
            var dc = plane.Normal.Dot(Direction);

            // If parallel to plane
            if (dc == 0)
                return false;

            var dw = plane.Distance - plane.Normal.Dot(Origin);
            var t = dw / dc;
            return ComputeHit(t, tmin, tmax, ref hit);
        }

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsPlane(__plane3t__ plane, out __ftype__ t)
            => HitsPlane(plane, 0, __ftype__.MaxValue, out t);

        public readonly bool HitsPlane(__plane3t__ plane, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
        {
            var dc = plane.Normal.Dot(Direction);

            // If parallel to plane
            if (dc == 0)
            {
                t = __ftype__.NaN;
                return false;
            }

            var dw = plane.Distance - plane.Normal.Dot(Origin);
            t = dw / dc;
            return t >= tmin && t <= tmax;
        }

        #endregion

        #region Ray-Circle hit intersection

        /// <summary>
        /// Returns true if the ray intersects with the primitive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__circle3t__ circle, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, ref hit);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__circle3t__ circle, ref __rayhit3t__ hit)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, ref __rayhit3t__ hit)
            => HitsCircle(center, normal, radius, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray intersects with the primitive.
        /// </summary>
        public readonly bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
        {
            var dc = normal.Dot(Direction);
            var dw = normal.Dot(center - Origin);

            // If parallel to plane
            if (dc == 0)
                return false;

            var t = dw / dc;
            if (!ComputeHit(t, tmin, tmax, ref hit))
                return false;

            if (Vec.DistanceSquared(hit.Point, center) > radius * radius)
            {
                hit.Point = __v3t__.NaN;
                hit.T = tmax;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the ray intersects with the primitive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__circle3t__ circle, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__circle3t__ circle, out __ftype__ t)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, out __ftype__ t)
            => HitsCircle(center, normal, radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive.
        /// </summary>
        public readonly bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
        {
            var dc = normal.Dot(Direction);
            var dw = normal.Dot(center - Origin);

            // If parallel to plane
            if (dc == 0)
            {
                t = __ftype__.NaN;
                return false;
            }

            t = dw / dc;
            if (t < tmin || t > tmax)
                return false;

            var point = GetPointOnRay(t); // add point as out parameter?
            return Vec.DistanceSquared(point, center) <= radius * radius;
        }

        #endregion

        #region Ray-Cylinder hit intersection

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsFiniteCylinderCandidate(__ftype__ t, __ftype__ tmin, __ftype__ best)
            => t >= tmin && t < best && t.IsFinite();

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsInsideScaledCylinderCap(__v3t__ radial, __ftype__ radius)
        {
            var scale = Fun.Max(radial.NormMax, radius);
            if (!(scale > 0)) return scale == 0;
            if (!scale.IsFinite()) return false;

            radial /= scale;
            var scaledRadius = radius / scale;
            return radial.LengthSquared <= scaledRadius * scaledRadius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsInsideCylinderCap(
            __v3t__ originPerpendicular, __v3t__ directionPerpendicular,
            __ftype__ t, __ftype__ radius)
        {
            var radial = originPerpendicular + directionPerpendicular * t;
            var radialSquared = radial.LengthSquared;
            var radiusSquared = radius * radius;
            return radialSquared.IsFinite() && radiusSquared.IsFinite()
                ? radialSquared <= radiusSquared
                : IsInsideScaledCylinderCap(radial, radius);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetScaledCylinderBarrelRoots(
            __v3t__ directionPerpendicular, __v3t__ originPerpendicular,
            __ftype__ radius, out __ftype__ root0, out __ftype__ root1)
        {
            root0 = __ftype__.NaN;
            root1 = __ftype__.NaN;

            var directionScale = directionPerpendicular.NormMax;
            if (!(directionScale > 0) || !directionScale.IsFinite()) return;

            var originScale = Fun.Max(originPerpendicular.NormMax, radius);
            if (!(originScale > 0))
            {
                root0 = 0;
                root1 = 0;
                return;
            }
            if (!originScale.IsFinite()) return;

            var scaledDirection = directionPerpendicular / directionScale;
            var scaledOrigin = originPerpendicular / originScale;
            var scaledRadius = radius / originScale;
            var a = scaledDirection.LengthSquared;
            var b = scaledDirection.Dot(scaledOrigin);
            var c = scaledOrigin.LengthSquared - scaledRadius * scaledRadius;
            var discriminant = b * b - a * c;
            var rootScale = originScale / directionScale;

            if (!(a > 0) || !discriminant.IsFinite() || !rootScale.IsFinite() || discriminant < 0)
                return;

            var sqrtDiscriminant = Fun.Sqrt(discriminant);
            if (sqrtDiscriminant == 0)
            {
                root0 = (-b / a) * rootScale;
                root1 = root0;
                return;
            }

            var q = b > 0 ? -b - sqrtDiscriminant : -b + sqrtDiscriminant;
            if (b > 0)
            {
                root0 = (q / a) * rootScale;
                root1 = (c / q) * rootScale;
            }
            else
            {
                root0 = (c / q) * rootScale;
                root1 = (q / a) * rootScale;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetCylinderBarrelRoots(
            __v3t__ directionPerpendicular, __v3t__ originPerpendicular,
            __ftype__ radius, out __ftype__ root0, out __ftype__ root1)
        {
            var a = directionPerpendicular.LengthSquared;
            var b = directionPerpendicular.Dot(originPerpendicular);
            var c = originPerpendicular.LengthSquared - radius * radius;
            var discriminant = b * b - a * c;

            if (!(a > 0) || !discriminant.IsFinite())
            {
                GetScaledCylinderBarrelRoots(
                    directionPerpendicular, originPerpendicular, radius, out root0, out root1);
                return;
            }

            if (discriminant < 0)
            {
                root0 = __ftype__.NaN;
                root1 = __ftype__.NaN;
                return;
            }

            var sqrtDiscriminant = Fun.Sqrt(discriminant);
            if (sqrtDiscriminant == 0)
            {
                root0 = -b / a;
                root1 = root0;
                return;
            }

            var q = b > 0 ? -b - sqrtDiscriminant : -b + sqrtDiscriminant;
            if (b > 0)
            {
                root0 = q / a;
                root1 = c / q;
            }
            else
            {
                root0 = c / q;
                root1 = q / a;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly bool TryGetCylinderHit(
            __v3t__ p0, __v3t__ p1, __ftype__ radius,
            __ftype__ tmin, __ftype__ tmax, __ftype__ distanceScale,
            out __ftype__ t)
        {
            if (distanceScale != 0)
                return TryGetCylinderHitRobust(p0, p1, radius, tmin, tmax, distanceScale, out t);

            return TryGetCylinderHitFast(p0, p1, radius, tmin, tmax, out t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly bool TryGetCylinderHitFast(
            __v3t__ p0, __v3t__ p1, __ftype__ radius,
            __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
        {
            t = __ftype__.NaN;
            if (!(radius >= 0) || radius > __ftype__.MaxValue || !(tmin < tmax))
                return false;

            var axis = p1 - p0;
            var axisLengthSquared = axis.LengthSquared;
            if (!(axisLengthSquared > 0) || !axisLengthSquared.IsFinite())
                return TryGetCylinderHitRobust(p0, p1, radius, tmin, tmax, 0, out t);

            var axisLength = Fun.Sqrt(axisLengthSquared);
            var axisDirection = axis * (1 / axisLength);
            var originOffset = Origin - p0;
            var directionAlongAxis = Direction.Dot(axisDirection);
            var originAlongAxis = originOffset.Dot(axisDirection);
            var directionPerpendicular = Direction - directionAlongAxis * axisDirection;
            var originPerpendicular = originOffset - originAlongAxis * axisDirection;
            var radiusSquared = radius * radius;

            var best = tmax;
            var found = false;
            if (directionPerpendicular != __v3t__.Zero)
            {
                var a = directionPerpendicular.LengthSquared;
                var b = directionPerpendicular.Dot(originPerpendicular);
                var c = originPerpendicular.LengthSquared - radiusSquared;
                var discriminant = b * b - a * c;
                if (!(a > 0) || !discriminant.IsFinite() || !radiusSquared.IsFinite())
                    return TryGetCylinderHitRobust(p0, p1, radius, tmin, tmax, 0, out t);

                if (discriminant >= 0)
                {
                    var sqrtDiscriminant = Fun.Sqrt(discriminant);
                    __ftype__ root0;
                    __ftype__ root1;
                    if (sqrtDiscriminant == 0)
                    {
                        root0 = -b / a;
                        root1 = root0;
                    }
                    else
                    {
                        var q = b > 0 ? -b - sqrtDiscriminant : -b + sqrtDiscriminant;
                        if (b > 0)
                        {
                            root0 = q / a;
                            root1 = c / q;
                        }
                        else
                        {
                            root0 = c / q;
                            root1 = q / a;
                        }
                    }

                    if (root0 >= tmin && root0 < best)
                    {
                        var axial = originAlongAxis + root0 * directionAlongAxis;
                        if (axial >= 0 && axial <= axisLength)
                        {
                            best = root0;
                            found = true;
                        }
                    }
                    if (root1 >= tmin && root1 < best)
                    {
                        var axial = originAlongAxis + root1 * directionAlongAxis;
                        if (axial >= 0 && axial <= axisLength)
                        {
                            best = root1;
                            found = true;
                        }
                    }
                }
            }

            if (directionAlongAxis != 0)
            {
                var cap0 = -originAlongAxis / directionAlongAxis;
                if (cap0 >= tmin && cap0 < best)
                {
                    var radial = originPerpendicular + directionPerpendicular * cap0;
                    var radialSquared = radial.LengthSquared;
                    if (!radialSquared.IsFinite() || !radiusSquared.IsFinite())
                        return TryGetCylinderHitRobust(p0, p1, radius, tmin, tmax, 0, out t);
                    if (radialSquared <= radiusSquared)
                    {
                        best = cap0;
                        found = true;
                    }
                }

                var cap1 = (axisLength - originAlongAxis) / directionAlongAxis;
                if (cap1 >= tmin && cap1 < best)
                {
                    var radial = originPerpendicular + directionPerpendicular * cap1;
                    var radialSquared = radial.LengthSquared;
                    if (!radialSquared.IsFinite() || !radiusSquared.IsFinite())
                        return TryGetCylinderHitRobust(p0, p1, radius, tmin, tmax, 0, out t);
                    if (radialSquared <= radiusSquared)
                    {
                        best = cap1;
                        found = true;
                    }
                }
            }

            if (!found || !best.IsFinite()) return false;
            if (!(Origin + Direction * best).IsFinite)
                return TryGetCylinderHitRobust(p0, p1, radius, tmin, tmax, 0, out t);

            t = best;
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private readonly bool TryGetCylinderHitRobust(
            __v3t__ p0, __v3t__ p1, __ftype__ radius,
            __ftype__ tmin, __ftype__ tmax, __ftype__ distanceScale,
            out __ftype__ t)
        {
            t = __ftype__.NaN;
            if (!(radius >= 0) || radius > __ftype__.MaxValue || !(tmin < tmax))
                return false;

            var axis = p1 - p0;

            __v3t__ axisDirection;
            var axisLengthSquared = axis.LengthSquared;
            __ftype__ axisLength;
            if (axisLengthSquared > 0 && axisLengthSquared.IsFinite())
            {
                axisLength = Fun.Sqrt(axisLengthSquared);
                axisDirection = axis * (1 / axisLength);
            }
            else
            {
                var axisScale = axis.NormMax;
                if (!(axisScale > 0) || !axisScale.IsFinite()) return false;

                var scaledAxis = axis / axisScale;
                var scaledLength = scaledAxis.Length;
                axisLength = axisScale * scaledLength;
                axisDirection = scaledAxis * (1 / scaledLength);
                if (!axisLength.IsFinite() || !axisDirection.IsFinite) return false;
            }

            var originOffset = Origin - p0;
            var directionAlongAxis = Direction.Dot(axisDirection);
            var originAlongAxis = originOffset.Dot(axisDirection);
            var directionPerpendicular = Direction - directionAlongAxis * axisDirection;
            var originPerpendicular = originOffset - originAlongAxis * axisDirection;

            if (distanceScale != 0)
            {
                if (!distanceScale.IsFinite()) return false;
                __ftype__ closestParameter;
                var perpendicularLengthSquared = directionPerpendicular.LengthSquared;
                var perpendicularDot = directionPerpendicular.Dot(originPerpendicular);
                if (perpendicularLengthSquared > 0 && perpendicularLengthSquared.IsFinite() && perpendicularDot.IsFinite())
                {
                    closestParameter = -perpendicularDot / perpendicularLengthSquared;
                }
                else if (directionPerpendicular != __v3t__.Zero)
                {
                    var directionScale = directionPerpendicular.NormMax;
                    var originScale = originPerpendicular.NormMax;
                    if (!(directionScale > 0) || !directionScale.IsFinite() || !originScale.IsFinite())
                        return false;

                    if (originScale > 0)
                    {
                        var scaledDirection = directionPerpendicular / directionScale;
                        var scaledOrigin = originPerpendicular / originScale;
                        closestParameter = -(originScale / directionScale)
                            * scaledDirection.Dot(scaledOrigin) / scaledDirection.LengthSquared;
                    }
                    else
                    {
                        closestParameter = 0;
                    }
                }
                else
                {
                    closestParameter = 0;
                }

                var directionLength = Direction.Length;
                if (!(directionLength > 0) || !directionLength.IsFinite())
                {
                    var directionScale = Direction.NormMax;
                    var scaledDirection = Direction / directionScale;
                    directionLength = directionScale * scaledDirection.Length;
                }

                var distance = Fun.Abs(closestParameter) * directionLength;
                radius = ((radius / distanceScale) * distance) * 2;
                if (!radius.IsFinite() || radius < 0) return false;
            }

            var best = tmax;
            var found = false;

            if (directionPerpendicular != __v3t__.Zero)
            {
                GetCylinderBarrelRoots(directionPerpendicular, originPerpendicular, radius, out var root0, out var root1);
                if (IsFiniteCylinderCandidate(root0, tmin, best))
                {
                    var axial = originAlongAxis + root0 * directionAlongAxis;
                    if (axial >= 0 && axial <= axisLength)
                    {
                        best = root0;
                        found = true;
                    }
                }
                if (IsFiniteCylinderCandidate(root1, tmin, best))
                {
                    var axial = originAlongAxis + root1 * directionAlongAxis;
                    if (axial >= 0 && axial <= axisLength)
                    {
                        best = root1;
                        found = true;
                    }
                }
            }

            if (directionAlongAxis != 0)
            {
                var cap0 = -originAlongAxis / directionAlongAxis;
                if (IsFiniteCylinderCandidate(cap0, tmin, best)
                    && IsInsideCylinderCap(originPerpendicular, directionPerpendicular, cap0, radius))
                {
                    best = cap0;
                    found = true;
                }

                var cap1 = (axisLength - originAlongAxis) / directionAlongAxis;
                if (IsFiniteCylinderCandidate(cap1, tmin, best)
                    && IsInsideCylinderCap(originPerpendicular, directionPerpendicular, cap1, radius))
                {
                    best = cap1;
                    found = true;
                }
            }

            if (!found || !(Origin + Direction * best).IsFinite) return false;
            t = best;
            return true;
        }

        /// <summary>
        /// Returns true if the ray hits the finite capped cylinder within the supplied parameter
        /// interval and before the parameter value already stored in <paramref name="hit"/>.
        /// </summary>
        public readonly bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius,
                __ftype__ tmin, __ftype__ tmax,
                ref __rayhit3t__ hit)
        {
            if (!TryGetCylinderHit(p0, p1, radius, tmin, tmax, 0, out var t) || !(t < hit.T))
                return false;

            hit.T = t;
            hit.Point = GetPointOnRay(t);
            hit.Coord = V2d.NaN;
            hit.BackSide = false;
            return true;
        }

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius, ref __rayhit3t__ hit)
            => HitsCylinder(p0, p1, radius, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the finite capped cylinder within the half-open parameter
        /// interval [<paramref name="tmin"/>, <paramref name="tmax"/>). On failure,
        /// <paramref name="t"/> is NaN.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius,
                __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
            => TryGetCylinderHit(p0, p1, radius, tmin, tmax, 0, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius, out __ftype__ t)
            => HitsCylinder(p0, p1, radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__cylinder3t__ cylinder, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => Hits(cylinder, tmin, tmax, 0, ref hit);

        /// <summary>
        /// Returns true if the ray hits the finite capped cylinder within the supplied parameter
        /// interval and before the parameter value already stored in <paramref name="hit"/>.
        /// A nonzero <paramref name="distanceScale"/> grows the effective radius with distance.
        /// </summary>
        public readonly bool Hits(__cylinder3t__ cylinder, __ftype__ tmin, __ftype__ tmax, __ftype__ distanceScale, ref __rayhit3t__ hit)
        {
            if (!TryGetCylinderHit(
                    cylinder.P0, cylinder.P1, cylinder.Radius,
                    tmin, tmax, distanceScale, out var t)
                || !(t < hit.T))
                return false;

            hit.T = t;
            hit.Point = GetPointOnRay(t);
            hit.Coord = V2d.NaN;
            hit.BackSide = false;
            return true;
        }

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Hits(__cylinder3t__ cylinder, ref __rayhit3t__ hit)
            => Hits(cylinder, 0, __ftype__.MaxValue, ref hit);

        #endregion

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__ray3t__ a, __ray3t__ b)
            => (a.Origin == b.Origin) && (a.Direction == b.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__ray3t__ a, __ray3t__ b)
            => !((a.Origin == b.Origin) && (a.Direction == b.Direction));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LexicalCompare(__ray3t__ other)
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
        public override readonly int GetHashCode() => HashCode.GetCombined(Origin, Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(__ray3t__ other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

        public override readonly bool Equals(object other)
            => (other is __ray3t__ o) ? Equals(o) : false;

        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

        public static __ray3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __ray3t__(__v3t__.Parse(x[0]), __v3t__.Parse(x[1]));
        }

        #endregion

        #region __iboundingbox__

        public readonly __box3t__ BoundingBox3__tc__ => __box3t__.FromPoints(Origin, Direction + Origin);

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__ray3t__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __ray3t__ a, __ray3t__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Origin, b.Origin, tolerance) &&
            ApproximateEquals(a.Direction, b.Direction, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__ray3t__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __ray3t__ a, __ray3t__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    #region __rayhit3t__

    /// <summary>
    /// A ray hit represents the hit of a ray on a primitive object such as
    /// a triangle. It stores the ray parameter of the hit, the hit point,
    /// the hit point's coordinates, and a flag indicating if the backside
    /// of the primitive was hit. Optionally the part field can be used to
    /// store which part of a multi-part object was hit. If no multi-part
    /// objects are used, this field remains 0.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __rayhit3t__
    {
        [DataMember]
        public __ftype__ T;
        [DataMember]
        public __v3t__ Point;
        [DataMember]
        public V2d Coord;
        [DataMember]
        public bool BackSide;
        [DataMember]
        public int Part;

        #region Constructor

        public __rayhit3t__(__ftype__ tMax)
        {
            T = tMax;
            Point = __v3t__.NaN;
            Coord = V2d.NaN;
            BackSide = false;
            Part = 0;
        }

        #endregion

        #region Constants

        public static readonly __rayhit3t__ MaxRange = new __rayhit3t__(__ftype__.MaxValue);

        #endregion
    }

    #endregion

    #region Fast__ray3t__

    /// <summary>
    /// A fast ray contains a ray and a number of precomputed flags and
    /// fields for fast intersection computation with bounding boxes and
    /// other axis-aligned sturctures such as kd-Trees.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Fast__ray3t__
    {
        [DataMember]
        public readonly __ray3t__ Ray;
        [DataMember]
        public readonly DirFlags DirFlags;
        [DataMember]
        public readonly __v3t__ InvDir;

        #region Constructors

        public Fast__ray3t__(__ray3t__ ray)
        {
            Ray = ray;
            DirFlags = ray.Direction.DirFlags();
            InvDir = 1 / ray.Direction;
        }

        public Fast__ray3t__(__v3t__ origin, __v3t__ direction)
            : this(new __ray3t__(origin, direction))
        { }

        #endregion

        #region Ray Arithmetics

        public readonly bool Intersects(
            __box3t__ box,
            ref __ftype__ tmin,
            ref __ftype__ tmax
            )
        {
            var dirFlags = DirFlags;

            if ((dirFlags & DirFlags.PositiveX) != 0)
            {
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveZ) != 0)
            {
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to Z-plane
            {
                if (Ray.Origin.Z < box.Min.Z || Ray.Origin.Z > box.Max.Z)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        /// <summary>
        /// This variant of the intersection method only tests with the
        /// faces of the box indicated by the supplied boxFlags.
        /// </summary>
        public readonly bool Intersects(
            __box3t__ box,
            Box.Flags boxFlags,
            ref __ftype__ tmin,
            ref __ftype__ tmax
            )
        {
            var dirFlags = DirFlags;

            if ((dirFlags & DirFlags.PositiveX) != 0)
            {
                if ((boxFlags & Box.Flags.MaxX) != 0)
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MaxX) != 0)
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to X-plane
            {
                if ((boxFlags & Box.Flags.MinX) != 0 && (Ray.Origin.X < box.Min.X) ||
                    (boxFlags & Box.Flags.MaxX) != 0 && (Ray.Origin.X > box.Max.X))
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                if ((boxFlags & Box.Flags.MaxY) != 0)
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MaxY) != 0)
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to Y-plane
            {
                if ((boxFlags & Box.Flags.MinY) != 0 && (Ray.Origin.Y < box.Min.Y) ||
                    (boxFlags & Box.Flags.MaxY) != 0 && (Ray.Origin.Y > box.Max.Y))
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveZ) != 0)
            {
                if ((boxFlags & Box.Flags.MaxZ) != 0)
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MaxZ) != 0)
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to Z-plane
            {
                if ((boxFlags & Box.Flags.MinZ) != 0 && (Ray.Origin.Z < box.Min.Z) ||
                    (boxFlags & Box.Flags.MaxZ) != 0 && (Ray.Origin.Z > box.Max.Z))
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        /// <summary>
        /// This variant of the intersection method returns the affected
        /// planes of the box if the box was hit. Flags are combined when
        /// multiple planes produce the same interval bound.
        /// </summary>
        public readonly bool Intersects(
            __box3t__ box,
            ref __ftype__ tmin,
            ref __ftype__ tmax,
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
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MaxX; }
                }
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MinX; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MinX; }
                }
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MaxX; }
                }
            }
            else	// ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MaxY; }
                }
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MinY; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MinY; }
                }
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MaxY; }
                }
            }
            else	// ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveZ) != 0)
            {
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MaxZ; }
                }
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MinZ; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MinZ; }
                }
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxZ; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MaxZ; }
                }
            }
            else	// ray parallel to Z-plane
            {
                if (Ray.Origin.Z < box.Min.Z || Ray.Origin.Z > box.Max.Z)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        /// <summary>
        /// This variant of the intersection method only tests with the
        /// faces of the box indicated by the supplied boxFlags and
        /// returns the affected planes of the box if the box was hit.
        /// Selected planes producing the same interval bound are combined.
        /// </summary>
        public readonly bool Intersects(
            __box3t__ box,
            Box.Flags boxFlags,
            ref __ftype__ tmin,
            ref __ftype__ tmax,
            out Box.Flags tminFlags,
            out Box.Flags tmaxFlags
            )
        {
            var dirFlags = DirFlags;
            tminFlags = Box.Flags.None;
            tmaxFlags = Box.Flags.None;

            if ((dirFlags & DirFlags.PositiveX) != 0)
            {
                if ((boxFlags & Box.Flags.MaxX) != 0)
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MaxX; }
                }
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MinX; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MinX; }
                }
                if ((boxFlags & Box.Flags.MaxX) != 0)
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MaxX; }
                }
            }
            else	// ray parallel to X-plane
            {
                if ((boxFlags & Box.Flags.MinX) != 0 && (Ray.Origin.X < box.Min.X) ||
                    (boxFlags & Box.Flags.MaxX) != 0 && (Ray.Origin.X > box.Max.X))
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveY) != 0)
            {
                if ((boxFlags & Box.Flags.MaxY) != 0)
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MaxY; }
                }
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MinY; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MinY; }
                }
                if ((boxFlags & Box.Flags.MaxY) != 0)
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MaxY; }
                }
            }
            else	// ray parallel to Y-plane
            {
                if ((boxFlags & Box.Flags.MinY) != 0 && (Ray.Origin.Y < box.Min.Y) ||
                    (boxFlags & Box.Flags.MaxY) != 0 && (Ray.Origin.Y > box.Max.Y))
                    return false;
            }

            if ((dirFlags & DirFlags.PositiveZ) != 0)
            {
                if ((boxFlags & Box.Flags.MaxZ) != 0)
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MaxZ; }
                }
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MinZ; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t < tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
                    else if (t == tmax) { tmaxFlags |= Box.Flags.MinZ; }
                }
                if ((boxFlags & Box.Flags.MaxZ) != 0)
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t > tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxZ; }
                    else if (t == tmin) { tminFlags |= Box.Flags.MaxZ; }
                }
            }
            else	// ray parallel to Z-plane
            {
                if ((boxFlags & Box.Flags.MinZ) != 0 && (Ray.Origin.Z < box.Min.Z) ||
                    (boxFlags & Box.Flags.MaxZ) != 0 && (Ray.Origin.Z > box.Max.Z))
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        #endregion
    }

    #endregion

    //# }
}

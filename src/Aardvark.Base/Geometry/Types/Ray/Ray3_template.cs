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
        public bool IsValid { get { return Direction != __v3t__.Zero; } }

        /// <summary>
        /// A ray is invalid if its direction is zero.
        /// </summary>
        public bool IsInvalid { get { return Direction == __v3t__.Zero; } }

        /// <summary>
        /// Returns true if either the origin or the direction contains any NaN value.
        /// </summary>
        public bool AnyNaN { get { return Origin.AnyNaN || Direction.AnyNaN; } }

        /// <summary>
        /// Line segment from origin to origin + direction.
        /// </summary>
        public __line3t__ __line3t__ => new __line3t__(Origin, Origin + Direction);

        /// <summary>
        /// Returns new ray with flipped direction.
        /// </summary>
        public __ray3t__ Reversed => new __ray3t__(Origin, -Direction);

        #endregion

        #region Ray Arithmetics

        /// <summary>
        /// Gets the point on the ray that is t * Direction from Origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __v3t__ GetPointOnRay(__ftype__ t) => Origin + Direction * t;

        /// <summary>
        /// Gets the t for a point p on this ray. 
        /// </summary>
        public __ftype__ GetT(__v3t__ p)
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
        public __ftype__ GetTOfProjectedPoint(__v3t__ p)
        {
            var v = p - Origin;
            return v.Dot(Direction) / Direction.LengthSquared;
        }

        /// <summary>
        /// Returns the ray transformed with the given matrix.
        /// This method is only valid for similarity transformations (uniform scale).
        /// </summary>
        public __ray3t__ Transformed(__m44t__ mat) => new __ray3t__(
            mat.TransformPos(Origin), mat.TransformDir(Direction)
            );

        /// <summary>
        /// Returns the angle between this and the given <see cref="__ray3t__"/> in radians.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ftype__ AngleBetweenFast(__ray3t__ r)
            => Direction.AngleBetweenFast(r.Direction);

        /// <summary>
        /// Returns the angle between this and the given <see cref="__ray3t__"/> in radians using a numerically stable algorithm.
        /// The direction vectors of the input rays have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ftype__ AngleBetween(__ray3t__ r)
            => Direction.AngleBetween(r.Direction);

        #endregion

        #region Ray hit intersections

        #region Private functions

        private bool ComputeHit(
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

        private bool GetClosestHit(
                __ftype__ t1, __ftype__ t2,
                __ftype__ tmin, __ftype__ tmax,
                ref __rayhit3t__ hit)
        {
            return t1 < t2
                  ? ProcessHits(t1, t2, tmin, tmax, ref hit)
                  : ProcessHits(t2, t1, tmin, tmax, ref hit);
        }


        private bool ProcessHits(
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
        public bool Hits(__ray3t__ ray, ref __rayhit3t__ hit)
            => HitsRay(ray, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public bool Hits(__ray3t__ ray, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => HitsRay(ray, tmin, tmax, ref hit);

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public bool HitsRay(__ray3t__ ray, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
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
        public bool Hits(__triangle3t__ triangle, ref __rayhit3t__ hit)
            => HitsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the triangle. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__triangle3t__ triangle, out __ftype__ t)
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
        public bool Hits(__triangle3t__ triangle, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
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
        public bool HitsTriangle(__v3t__ p0, __v3t__ p1, __v3t__ p2, ref __rayhit3t__ hit)
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
        public bool HitsTriangle(
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
        public bool HitsTriangle(__v3t__ p0, __v3t__ p1, __v3t__ p2, out __ftype__ t)
            => HitsTriangle(p0, p1, p2, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval. Degenerated triangles will not result in an 
        /// intersection even if any edge is hit exactly.
        /// </summary>
        public bool HitsTriangle(
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
        public bool HitsTrianglePointAndEdges(
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
        public bool HitsTrianglePointAndEdges(
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
        public bool Hits(__quad3t__ quad, ref __rayhit3t__ hit) => HitsQuad(
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
        public bool Hits(__quad3t__ quad, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit) => HitsQuad(
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
        public bool HitsQuad(
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
        public bool Hits(__quad3t__ quad, __ftype__ tmin, __ftype__ tmax, out __ftype__ t) 
            => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied parameter interval.
        /// A hit with this overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__quad3t__ quad, out __ftype__ t)
            => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied parameter interval.
        /// A hit with this overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HitsQuad(__v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3, out __ftype__ t)
            => HitsQuad(p0, p1, p2, p3, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied parameter interval.
        /// </summary>
        public bool HitsQuad(__v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
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
        public bool HitsSphere(
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
        public bool Hits(__sphere3t__ sphere, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
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
        public bool Hits(__sphere3t__ sphere, ref __rayhit3t__ hit)
            => HitsSphere(sphere.Center, sphere.Radius, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the
        /// supplied parameter interval. Note that a hit is only
        /// registered if the front or the backsurface is encountered
        /// within the interval. If there are two valid solutions, the 
        /// closest will be returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__sphere3t__ sphere, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
            => HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere. Note that a hit is
        /// registered if the front or the backsurface is encountered. If there 
        /// are two valid solutions, the closest will be returned. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__sphere3t__ sphere, out __ftype__ t)
            => HitsSphere(sphere.Center, sphere.Radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the supplied parameter interval. 
        /// Note that a hit is registered if the front or the backsurface is encountered within the 
        /// interval. If there are two valid solutions, the closest will be returned. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        public bool HitsSphere(__v3t__ center, __ftype__ radius, out __ftype__ t)
            => HitsSphere(center, radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the supplied parameter interval. 
        /// Note that a hit is registered if the front or the backsurface is encountered within the 
        /// interval. If there are two valid solutions, the closest will be returned.
        /// </summary>
        public bool HitsSphere(__v3t__ center, __ftype__ radius, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
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
        public bool HitsPlane(__plane3t__ plane, ref __rayhit3t__ hit)
            => HitsPlane(plane, 0, __ftype__.MaxValue, ref hit);

        public bool HitsPlane(__plane3t__ plane, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
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
        public bool HitsPlane(__plane3t__ plane, out __ftype__ t)
            => HitsPlane(plane, 0, __ftype__.MaxValue, out t);

        public bool HitsPlane(__plane3t__ plane, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
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
        public bool Hits(__circle3t__ circle, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, ref hit);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__circle3t__ circle, ref __rayhit3t__ hit)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, ref __rayhit3t__ hit)
            => HitsCircle(center, normal, radius, 0, __ftype__.MaxValue, ref hit);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. 
        /// </summary>
        public bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
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
        public bool Hits(__circle3t__ circle, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__circle3t__ circle, out __ftype__ t)
            => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, out __ftype__ t)
            => HitsCircle(center, normal, radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. 
        /// </summary>
        public bool HitsCircle(__v3t__ center, __v3t__ normal, __ftype__ radius, __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
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

        /// <summary>
        /// Returns true if the ray intersects with the primitive. 
        /// </summary>
        public bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius,
                __ftype__ tmin, __ftype__ tmax,
                ref __rayhit3t__ hit)
        {
            var axis = new __line3t__(p0, p1);
            var axisDir = axis.Direction.Normalized;

            // Vector Cyl.P0 -> Ray.Origin
            var op = Origin - p0;

            // normal RayDirection - CylinderAxis
            var normal = Direction.Cross(axisDir);
            var unitNormal = normal.Normalized;

            // normal (Vec Cyl.P0 -> Ray.Origin) - CylinderAxis
            var normal2 = op.Cross(axisDir);
            var t = -normal2.Dot(unitNormal) / normal.Length;

            // between enitre rays (caps are ignored)
            var shortestDistance = Fun.Abs(op.Dot(unitNormal));
            if (shortestDistance <= radius)
            {
                var s = Fun.Abs(Fun.Sqrt(radius.Square() - shortestDistance.Square()) / Direction.Length);

                var t1 = t - s; // first hit of Cylinder shell
                var t2 = t + s; // second hit of Cylinder shell

                if (t1 > tmin && t1 < tmax) tmin = t1;
                if (t2 < tmax && t2 > tmin) tmax = t2;

                hit.T = t1;
                hit.Point = GetPointOnRay(t1);

                // check if found point is outside of Cylinder Caps
                var bottomPlane = new __plane3t__(-axisDir, p0);
                var topPlane = new __plane3t__(axisDir, p1);
                var heightBottom = bottomPlane.Height(hit.Point);
                var heightTop = topPlane.Height(hit.Point);
                // t1 lies outside of caps => find closest cap hit
                if (heightBottom > 0 || heightTop > 0)
                {
                    hit.T = tmax;
                    // intersect with bottom Cylinder Cap
                    var bottomHit = HitsPlane(bottomPlane, tmin, tmax, ref hit);
                    // intersect with top Cylinder Cap
                    var topHit = HitsPlane(topPlane, tmin, tmax, ref hit);
                    
                    // hit still close enough to cylinder axis?
                    var distance = axis.__ray3t__.GetMinimalDistanceTo(hit.Point);

                    if (distance <= radius && (bottomHit || topHit))
                        return true;
                }
                else
                    return true;
            }

            hit.T = tmax;
            hit.Point = __v3t__.NaN;
            return false;
        }

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius, ref __rayhit3t__ hit)
            => HitsCylinder(p0, p1, radius, 0, __ftype__.MaxValue, ref hit);

        public bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius,
                __ftype__ tmin, __ftype__ tmax, out __ftype__ t)
        {
            var axis = new __line3t__(p0, p1);
            var axisDir = axis.Direction.Normalized;

            // Vector Cyl.P0 -> Ray.Origin
            var op = Origin - p0;

            // normal RayDirection - CylinderAxis
            var normal = Direction.Cross(axisDir);
            var unitNormal = normal.Normalized;

            // normal (Vec Cyl.P0 -> Ray.Origin) - CylinderAxis
            var normal2 = op.Cross(axisDir);
            t = -normal2.Dot(unitNormal) / normal.Length;

            // between entire rays (caps are ignored)
            var shortestDistance = Fun.Abs(op.Dot(unitNormal));
            if (shortestDistance <= radius)
            {
                var s = Fun.Abs(Fun.Sqrt(radius.Square() - shortestDistance.Square()) / Direction.Length);

                var t1 = t - s; // first hit of Cylinder shell
                var t2 = t + s; // second hit of Cylinder shell

                if (t1 > tmin && t1 < tmax) tmin = t1;
                if (t2 < tmax && t2 > tmin) tmax = t2;

                t = t1;
                var point = GetPointOnRay(t1);

                // check if found point is outside of Cylinder Caps
                var bottomPlane = new __plane3t__(-axisDir, p0);
                var topPlane = new __plane3t__(axisDir, p1);
                var heightBottom = bottomPlane.Height(point);
                var heightTop = topPlane.Height(point);
                // t1 lies outside of caps => find closest cap hit
                if (heightBottom > 0 || heightTop > 0)
                {
                    // intersect with bottom Cylinder Cap
                    var bottomHit = HitsCircle(p0, -axisDir, radius, tmin, tmax, out t);
                    // intersect with top Cylinder Cap
                    var topHit = HitsCircle(p1, axisDir, radius, tmin, tmax, out __ftype__ ttop);

                    if (topHit)
                    {
                        if (bottomHit)
                        {
                            if (ttop.Abs() < t)
                                t = ttop;
                        }
                        else
                            t = ttop;
                    }

                    return topHit || bottomHit;
                }
                else
                    return true;
            }

            t = __ftype__.NaN;
            return false;
        }

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HitsCylinder(__v3t__ p0, __v3t__ p1, __ftype__ radius, out __ftype__ t)
            => HitsCylinder(p0, p1, radius, 0, __ftype__.MaxValue, out t);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__cylinder3t__ cylinder, __ftype__ tmin, __ftype__ tmax, ref __rayhit3t__ hit)
            => Hits(cylinder, tmin, tmax, 0, ref hit);

        /// <summary>
        /// Returns true if the ray intersects with the primitive. 
        /// </summary>
        public bool Hits(__cylinder3t__ cylinder, __ftype__ tmin, __ftype__ tmax, __ftype__ distanceScale, ref __rayhit3t__ hit)
        {
            var axisDir = cylinder.Axis.Direction.Normalized;

            // Vector Cyl.P0 -> Ray.Origin
            var op = Origin - cylinder.P0;

            // normal RayDirection - CylinderAxis
            var normal = Direction.Cross(axisDir);
            var unitNormal = normal.Normalized;

            // normal (Vec Cyl.P0 -> Ray.Origin) - CylinderAxis
            var normal2 = op.Cross(axisDir);
            var t = -normal2.Dot(unitNormal) / normal.Length;

            var radius = cylinder.Radius;
            if (distanceScale != 0)
            {   // cylinder gets bigger, the further away it is
                var pnt = GetPointOnRay(t);

                var dis = Vec.Distance(pnt, this.Origin);
                radius = ((cylinder.Radius / distanceScale) * dis) * 2;
            }

            // between enitre rays (caps are ignored)
            var shortestDistance = Fun.Abs(op.Dot(unitNormal));
            if (shortestDistance <= radius)
            {
                var s = Fun.Abs(Fun.Sqrt(radius.Square() - shortestDistance.Square()) / Direction.Length);

                var t1 = t - s; // first hit of Cylinder shell
                var t2 = t + s; // second hit of Cylinder shell

                if (t1 > tmin && t1 < tmax) tmin = t1;
                if (t2 < tmax && t2 > tmin) tmax = t2;

                hit.T = t1;
                hit.Point = GetPointOnRay(t1);

                // check if found point is outside of Cylinder Caps
                var bottomPlane = new __plane3t__(cylinder.Circle0.Normal, cylinder.Circle0.Center);
                var topPlane = new __plane3t__(cylinder.Circle1.Normal, cylinder.Circle1.Center);
                var heightBottom = bottomPlane.Height(hit.Point);
                var heightTop = topPlane.Height(hit.Point);
                // t1 lies outside of caps => find closest cap hit
                if (heightBottom > 0 || heightTop > 0)
                {
                    hit.T = tmax;
                    // intersect with bottom Cylinder Cap
                    var bottomHit = HitsPlane(bottomPlane, tmin, tmax, ref hit);
                    // intersect with top Cylinder Cap
                    var topHit = HitsPlane(topPlane, tmin, tmax, ref hit);

                    // hit still close enough to cylinder axis?
                    var distance = cylinder.Axis.__ray3t__.GetMinimalDistanceTo(hit.Point);

                    if (distance <= radius && (bottomHit || topHit))
                        return true;
                }
                else
                    return true;
            }

            hit.T = tmax;
            hit.Point = __v3t__.NaN;
            return false;
        }

        /// <summary>
        /// Returns true if the ray intersects with the primitive. A hit with this 
        /// overload is considered for t in [0, __ftype__.MaxValue].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Hits(__cylinder3t__ cylinder, ref __rayhit3t__ hit)
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
        public int LexicalCompare(__ray3t__ other)
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
        public bool Equals(__ray3t__ other)
            => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

        public override bool Equals(object other)
            => (other is __ray3t__ o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

        public static __ray3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __ray3t__(__v3t__.Parse(x[0]), __v3t__.Parse(x[1]));
        }

        #endregion

        #region __iboundingbox__

        public __box3t__ BoundingBox3__tc__ => __box3t__.FromPoints(Origin, Direction + Origin);

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
    public struct Fast__ray3t__
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

        public bool Intersects(
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
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
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
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
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
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
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
        public bool Intersects(
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
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MaxX) != 0)
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
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
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MaxY) != 0)
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
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
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                if ((boxFlags & Box.Flags.MaxZ) != 0)
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
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
        /// planes of the box if the box was hit.
        /// </summary>
        public bool Intersects(
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
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
                }
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
                }
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
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
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
                }
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
                }
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
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
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
                }
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
                }
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxZ; }
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
        /// </summary>
        public bool Intersects(
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
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
                }
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeX) != 0)
            {
                if ((boxFlags & Box.Flags.MinX) != 0)
                {
                    __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
                }
                if ((boxFlags & Box.Flags.MaxX) != 0)
                {
                    __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
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
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
                }
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeY) != 0)
            {
                if ((boxFlags & Box.Flags.MinY) != 0)
                {
                    __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
                }
                if ((boxFlags & Box.Flags.MaxY) != 0)
                {
                    __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
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
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
                }
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
                }
            }
            else if ((dirFlags & DirFlags.NegativeZ) != 0)
            {
                if ((boxFlags & Box.Flags.MinZ) != 0)
                {
                    __ftype__ t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
                }
                if ((boxFlags & Box.Flags.MaxZ) != 0)
                {
                    __ftype__ t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxZ; }
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

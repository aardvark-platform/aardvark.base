/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

#region Ray3f

/// <summary>
/// A three-dimensional ray with an origin and a direction.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Ray3f : IEquatable<Ray3f>, IValidity, IBoundingBox3f
{
    [DataMember]
    public V3f Origin;
    [DataMember]
    public V3f Direction;

    #region Constructors

    /// <summary>
    /// Creates Ray from origin point and directional vector
    /// </summary>
    public Ray3f(V3f origin, V3f direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public static Ray3f FromEndPoints(V3f origin, V3f target) => new Ray3f(origin, target - origin);

    #endregion

    #region Constants

    /// <summary>
    /// An invalid ray has a zero direction.
    /// </summary>
    public static readonly Ray3f Invalid = new Ray3f(V3f.NaN, V3f.Zero);

    #endregion

    #region Properties

    /// <summary>
    /// A ray is valid if its direction is non-zero.
    /// </summary>
    public readonly bool IsValid { get { return Direction != V3f.Zero; } }

    /// <summary>
    /// A ray is invalid if its direction is zero.
    /// </summary>
    public readonly bool IsInvalid { get { return Direction == V3f.Zero; } }

    /// <summary>
    /// Returns true if either the origin or the direction contains any NaN value.
    /// </summary>
    public readonly bool AnyNaN { get { return Origin.AnyNaN || Direction.AnyNaN; } }

    /// <summary>
    /// Line segment from origin to origin + direction.
    /// </summary>
    public readonly Line3f Line3f => new Line3f(Origin, Origin + Direction);

    /// <summary>
    /// Returns new ray with flipped direction.
    /// </summary>
    public readonly Ray3f Reversed => new Ray3f(Origin, -Direction);

    /// <summary>
    /// Returns the ray with its directional normalized.
    /// </summary>
    public readonly Ray3f Normalized => new(Origin, Direction.Normalized);

    #endregion

    #region Ray Arithmetics

    /// <summary>
    /// Gets the point on the ray that is t * Direction from Origin.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly V3f GetPointOnRay(float t) => Origin + Direction * t;

    /// <summary>
    /// Gets the t for a point p on this ray.
    /// </summary>
    public readonly float GetT(V3f p)
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
    public readonly float GetTOfProjectedPoint(V3f p)
    {
        var v = p - Origin;
        return v.Dot(Direction) / Direction.LengthSquared;
    }

    /// <summary>
    /// Returns the ray transformed with the given matrix.
    /// This method is only valid for similarity transformations (uniform scale).
    /// </summary>
    public readonly Ray3f Transformed(M44f mat)
        => new(mat.TransformPos(Origin), mat.TransformDir(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Rot3f"/> transformation.
    /// </summary>
    public readonly Ray3f Transformed(Rot3f transform)
        => new(transform.Transform(Origin), transform.Transform(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Scale3f"/> transformation.
    /// </summary>
    public readonly Ray3f Transformed(Scale3f transform)
        => new(transform.Transform(Origin), transform.Transform(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Shift3f"/> transformation.
    /// </summary>
    public readonly Ray3f Transformed(Shift3f transform)
        => new(transform.Transform(Origin), Direction);

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    public readonly Ray3f Transformed(Euclidean3f transform)
        => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    public readonly Ray3f Transformed(Similarity3f transform)
        => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Affine3f"/> transformation.
    /// </summary>
    public readonly Ray3f Transformed(Affine3f transform)
        => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

    /// <summary>
    /// Returns the angle between this and the given <see cref="Ray3f"/> in radians.
    /// The direction vectors of the input rays have to be normalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float AngleBetweenFast(Ray3f r)
        => Direction.AngleBetweenFast(r.Direction);

    /// <summary>
    /// Returns the angle between this and the given <see cref="Ray3f"/> in radians using a numerically stable algorithm.
    /// The direction vectors of the input rays have to be normalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float AngleBetween(Ray3f r)
        => Direction.AngleBetween(r.Direction);

    #endregion

    #region Ray hit intersections

    #region Private functions

    private readonly bool ComputeHit(
          float t,
          float tmin, float tmax,
          ref RayHit3f hit)
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
            float t1, float t2,
            float tmin, float tmax,
            ref RayHit3f hit)
    {
        return t1 < t2
              ? ProcessHits(t1, t2, tmin, tmax, ref hit)
              : ProcessHits(t2, t1, tmin, tmax, ref hit);
    }


    private readonly bool ProcessHits(
            float t1, float t2,
            float tmin, float tmax,
            ref RayHit3f hit)
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
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    public readonly bool Hits(Ray3f ray, ref RayHit3f hit)
        => HitsRay(ray, 0, float.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the other ray before the parameter
    /// value contained in the supplied hit. Detailed information about
    /// the hit is returned in the supplied hit.
    /// </summary>
    public readonly bool Hits(Ray3f ray, float tmin, float tmax, ref RayHit3f hit)
        => HitsRay(ray, tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the other ray before the parameter
    /// value contained in the supplied hit. Detailed information about
    /// the hit is returned in the supplied hit.
    /// </summary>
    public readonly bool HitsRay(Ray3f ray, float tmin, float tmax, ref RayHit3f hit)
    {
        V3f d = Origin - ray.Origin;
        V3f u = Direction;
        V3f v = ray.Direction;
        V3f n = u.Cross(v);

        if (Fun.IsTiny(d.Length)) return true;
        else if (Fun.IsTiny(u.Cross(v).Length)) return false;
        else
        {
            //-t0*u + t1*v + t2*n == d
            //M = {-u,v,n}
            //M*{t0,t1,t2}T == d
            //{t0,t1,t2}T == M^-1 * d

            M33f M = new M33f
            {
                C0 = -u,
                C1 = v,
                C2 = n
            };

            if (M.Invertible)
            {
                V3f t = M.Inverse * d;
                if (Fun.IsTiny(t.Z))
                {
                    ProcessHits(t.X, float.MaxValue, tmin, tmax, ref hit);
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
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Triangle3f triangle, ref RayHit3f hit)
        => HitsTrianglePointAndEdges(
            triangle.P0, triangle.Edge01, triangle.Edge02,
            0, float.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the triangle. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Triangle3f triangle, out float t)
        => HitsTrianglePointAndEdges(
            triangle.P0, triangle.Edge01, triangle.Edge02,
            0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3f.MaxRange.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Triangle3f triangle, float tmin, float tmax, ref RayHit3f hit)
        => HitsTrianglePointAndEdges(
            triangle.P0, triangle.Edge01, triangle.Edge02,
            tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the triangle within [0, float.MaxValue]
    /// and before the parameter value contained in the supplied hit. Detailed
    /// information about the hit is returned in the supplied hit. In order to
    /// obtain all potential hits, the supplied hit can be initialized with
    /// RayHit3f.MaxRange. Degenerated triangles will not result in an intersection
    /// even if any edge is hit exactly.
    /// </summary>
    public readonly bool HitsTriangle(V3f p0, V3f p1, V3f p2, ref RayHit3f hit)
        => HitsTriangle(p0, p1, p2, 0, float.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3f.MaxRange.
    /// Degenerated triangles will not result in an intersection even if
    /// any edge is hit exactly.
    /// </summary>
    public readonly bool HitsTriangle(
        V3f p0, V3f p1, V3f p2,
        float tmin, float tmax,
        ref RayHit3f hit
        )
    {
        V3f edge01 = p1 - p0;
        V3f edge02 = p2 - p0;
        V3f plane = Vec.Cross(Direction, edge02);
        float det = Vec.Dot(edge01, plane);
        if (det > -1e-4f && det < 1e-4f) return false;
        // ray ~= paralell / Triangle
        V3f tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        float u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        float v = Vec.Dot(Direction, plane) * det;
        if (v < 0 || u + v > 1) return false;
        float t = Vec.Dot(edge02, plane) * det;
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
    /// A hit with this overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsTriangle(V3f p0, V3f p1, V3f p2, out float t)
        => HitsTriangle(p0, p1, p2, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval. Degenerated triangles will not result in an
    /// intersection even if any edge is hit exactly.
    /// </summary>
    public readonly bool HitsTriangle(
        V3f p0, V3f p1, V3f p2,
        float tmin, float tmax,
        out float t
        )
    {
        V3f edge01 = p1 - p0;
        V3f edge02 = p2 - p0;
        V3f plane = Vec.Cross(Direction, edge02);
        float det = Vec.Dot(edge01, plane);
        t = float.NaN;
        if (det > -1e-4f && det < 1e-4f) return false;
        // ray ~= paralell / Triangle
        V3f tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        float u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        float v = Vec.Dot(Direction, plane) * det;
        if (v < 0 || u + v > 1) return false;
        t = Vec.Dot(edge02, plane) * det;
        return t >= tmin && t <= tmax;
    }

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3f.MaxRange.
    /// </summary>
    public readonly bool HitsTrianglePointAndEdges(
        V3f p0, V3f edge01, V3f edge02,
        float tmin, float tmax,
        ref RayHit3f hit
        )
    {
        V3f plane = Vec.Cross(Direction, edge02);
        float det = Vec.Dot(edge01, plane);
        if (det > -1e-4f && det < 1e-4f) return false;
        // ray ~= paralell / Triangle
        V3f tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        float u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        float v = Vec.Dot(Direction, plane) * det;
        if (v < 0 || u + v > 1) return false;
        float t = Vec.Dot(edge02, plane) * det;
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
    /// hits, the supplied hit can be initialized with RayHit3f.MaxRange.
    /// </summary>
    public readonly bool HitsTrianglePointAndEdges(
        V3f p0, V3f edge01, V3f edge02,
        float tmin, float tmax,
        out float t
        )
    {
        V3f plane = Vec.Cross(Direction, edge02);
        float det = Vec.Dot(edge01, plane);
        t = float.NaN;
        if (det > -1e-4f && det < 1e-4f) return false;
        // ray ~= paralell / Triangle
        V3f tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        float u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        float v = Vec.Dot(Direction, plane) * det;
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
    /// RayHit3f.MaxRange. A hit with this overload is considered
    /// for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Quad3f quad, ref RayHit3f hit) => HitsQuad(
        quad.P0, quad.P1, quad.P2, quad.P3,
        0, float.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3f.MaxRange.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Quad3f quad, float tmin, float tmax, ref RayHit3f hit) => HitsQuad(
        quad.P0, quad.P1, quad.P2, quad.P3,
        tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. The quad is considered to consist of the
    /// two triangles [p0,p1,p2] and [p0,p2,p3]. Detailed information
    /// about the hit is returned in the supplied hit. In order to obtain
    /// all potential hits, the supplied hit can be initialized with
    /// RayHit3f.MaxRange.
    /// </summary>
    public readonly bool HitsQuad(
        V3f p0, V3f p1, V3f p2, V3f p3,
        float tmin, float tmax,
        ref RayHit3f hit
        )
    {
        V3f e02 = p2 - p0;
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
    public readonly bool Hits(Quad3f quad, float tmin, float tmax, out float t)
        => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, tmin, tmax, out t);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied parameter interval.
    /// A hit with this overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Quad3f quad, out float t)
        => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied parameter interval.
    /// A hit with this overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsQuad(V3f p0, V3f p1, V3f p2, V3f p3, out float t)
        => HitsQuad(p0, p1, p2, p3, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied parameter interval.
    /// </summary>
    public readonly bool HitsQuad(V3f p0, V3f p1, V3f p2, V3f p3, float tmin, float tmax, out float t)
    {
        V3f e02 = p2 - p0;
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
            V3f center, float radius,
            float tmin, float tmax,
            ref RayHit3f hit)
    {
        V3f originSubCenter = Origin - center;
        float a = Direction.LengthSquared;
        float b = Direction.Dot(originSubCenter);
        float c = originSubCenter.LengthSquared - radius * radius;

        // --------------------- quadric equation : a t^2  + 2b t + c = 0
        float d = b * b - a * c;           // factor 2 was eliminated

        if (d < float.Epsilon)             // no root ?
            return false;                   // then exit

        if (b > 0)                        // stable way to calculate
            d = -Fun.Sqrt(d) - b;           // the roots of a quadratic
        else                                // equation
            d = Fun.Sqrt(d) - b;

        float t1 = d / a;
        float t2 = c / d;  // Vieta : t1 * t2 == c/a

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
    public readonly bool Hits(Sphere3f sphere, float tmin, float tmax, ref RayHit3f hit)
        => HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the
    /// supplied parameter interval and before the parameter value
    /// contained in the supplied hit. Note that a hit is only
    /// registered if the front or the backsurface is encountered
    /// within the interval. If there are two valid solutions, the
    /// closest will be returned. A hit with this overload is
    /// considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Sphere3f sphere, ref RayHit3f hit)
        => HitsSphere(sphere.Center, sphere.Radius, 0, float.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the
    /// supplied parameter interval. Note that a hit is only
    /// registered if the front or the backsurface is encountered
    /// within the interval. If there are two valid solutions, the
    /// closest will be returned.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Sphere3f sphere, float tmin, float tmax, out float t)
        => HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, out t);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere. Note that a hit is
    /// registered if the front or the backsurface is encountered. If there
    /// are two valid solutions, the closest will be returned. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Sphere3f sphere, out float t)
        => HitsSphere(sphere.Center, sphere.Radius, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the supplied parameter interval.
    /// Note that a hit is registered if the front or the backsurface is encountered within the
    /// interval. If there are two valid solutions, the closest will be returned. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    public readonly bool HitsSphere(V3f center, float radius, out float t)
        => HitsSphere(center, radius, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the supplied parameter interval.
    /// Note that a hit is registered if the front or the backsurface is encountered within the
    /// interval. If there are two valid solutions, the closest will be returned.
    /// </summary>
    public readonly bool HitsSphere(V3f center, float radius, float tmin, float tmax, out float t)
    {
        var originSubCenter = Origin - center;
        var a = Direction.LengthSquared;
        var b = Direction.Dot(originSubCenter);
        var c = originSubCenter.LengthSquared - radius * radius;

        // --------------------- quadric equation : a t^2  + 2b t + c = 0
        var d = b * b - a * c;              // factor 2 was eliminated

        if (d >= float.Epsilon)            // no root ? -> exit
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

        t = float.NaN;
        return false;
    }

    #endregion

    #region Ray-Plane hit intersection

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsPlane(Plane3f plane, ref RayHit3f hit)
        => HitsPlane(plane, 0, float.MaxValue, ref hit);

    public readonly bool HitsPlane(Plane3f plane, float tmin, float tmax, ref RayHit3f hit)
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
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsPlane(Plane3f plane, out float t)
        => HitsPlane(plane, 0, float.MaxValue, out t);

    public readonly bool HitsPlane(Plane3f plane, float tmin, float tmax, out float t)
    {
        var dc = plane.Normal.Dot(Direction);

        // If parallel to plane
        if (dc == 0)
        {
            t = float.NaN;
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
    public readonly bool Hits(Circle3f circle, float tmin, float tmax, ref RayHit3f hit)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Circle3f circle, ref RayHit3f hit)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, float.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCircle(V3f center, V3f normal, float radius, ref RayHit3f hit)
        => HitsCircle(center, normal, radius, 0, float.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    public readonly bool HitsCircle(V3f center, V3f normal, float radius, float tmin, float tmax, ref RayHit3f hit)
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
            hit.Point = V3f.NaN;
            hit.T = tmax;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Circle3f circle, float tmin, float tmax, out float t)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Circle3f circle, out float t)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCircle(V3f center, V3f normal, float radius, out float t)
        => HitsCircle(center, normal, radius, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    public readonly bool HitsCircle(V3f center, V3f normal, float radius, float tmin, float tmax, out float t)
    {
        var dc = normal.Dot(Direction);
        var dw = normal.Dot(center - Origin);

        // If parallel to plane
        if (dc == 0)
        {
            t = float.NaN;
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
    public readonly bool HitsCylinder(V3f p0, V3f p1, float radius,
            float tmin, float tmax,
            ref RayHit3f hit)
    {
        var axis = new Line3f(p0, p1);
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
            var bottomPlane = new Plane3f(-axisDir, p0);
            var topPlane = new Plane3f(axisDir, p1);
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
                var distance = axis.Ray3f.GetMinimalDistanceTo(hit.Point);

                if (distance <= radius && (bottomHit || topHit))
                    return true;
            }
            else
                return true;
        }

        hit.T = tmax;
        hit.Point = V3f.NaN;
        return false;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCylinder(V3f p0, V3f p1, float radius, ref RayHit3f hit)
        => HitsCylinder(p0, p1, radius, 0, float.MaxValue, ref hit);

    public readonly bool HitsCylinder(V3f p0, V3f p1, float radius,
            float tmin, float tmax, out float t)
    {
        var axis = new Line3f(p0, p1);
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
            var bottomPlane = new Plane3f(-axisDir, p0);
            var topPlane = new Plane3f(axisDir, p1);
            var heightBottom = bottomPlane.Height(point);
            var heightTop = topPlane.Height(point);
            // t1 lies outside of caps => find closest cap hit
            if (heightBottom > 0 || heightTop > 0)
            {
                // intersect with bottom Cylinder Cap
                var bottomHit = HitsCircle(p0, -axisDir, radius, tmin, tmax, out t);
                // intersect with top Cylinder Cap
                var topHit = HitsCircle(p1, axisDir, radius, tmin, tmax, out float ttop);

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

        t = float.NaN;
        return false;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCylinder(V3f p0, V3f p1, float radius, out float t)
        => HitsCylinder(p0, p1, radius, 0, float.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Cylinder3f cylinder, float tmin, float tmax, ref RayHit3f hit)
        => Hits(cylinder, tmin, tmax, 0, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    public readonly bool Hits(Cylinder3f cylinder, float tmin, float tmax, float distanceScale, ref RayHit3f hit)
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
            var bottomPlane = new Plane3f(cylinder.Circle0.Normal, cylinder.Circle0.Center);
            var topPlane = new Plane3f(cylinder.Circle1.Normal, cylinder.Circle1.Center);
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
                var distance = cylinder.Axis.Ray3f.GetMinimalDistanceTo(hit.Point);

                if (distance <= radius && (bottomHit || topHit))
                    return true;
            }
            else
                return true;
        }

        hit.T = tmax;
        hit.Point = V3f.NaN;
        return false;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, float.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Cylinder3f cylinder, ref RayHit3f hit)
        => Hits(cylinder, 0, float.MaxValue, ref hit);

    #endregion

    #endregion

    #region Comparison Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Ray3f a, Ray3f b)
        => (a.Origin == b.Origin) && (a.Direction == b.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Ray3f a, Ray3f b)
        => !((a.Origin == b.Origin) && (a.Direction == b.Direction));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int LexicalCompare(Ray3f other)
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
    public readonly bool Equals(Ray3f other)
        => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    public override readonly bool Equals(object other)
        => (other is Ray3f o) ? Equals(o) : false;

    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

    public static Ray3f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Ray3f(V3f.Parse(x[0]), V3f.Parse(x[1]));
    }

    #endregion

    #region IBoundingBox3f

    public readonly Box3f BoundingBox3f => Box3f.FromPoints(Origin, Direction + Origin);

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Ray3f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Ray3f a, Ray3f b, float tolerance) =>
        ApproximateEquals(a.Origin, b.Origin, tolerance) &&
        ApproximateEquals(a.Direction, b.Direction, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Ray3f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Ray3f a, Ray3f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region RayHit3f

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
public struct RayHit3f
{
    [DataMember]
    public float T;
    [DataMember]
    public V3f Point;
    [DataMember]
    public V2d Coord;
    [DataMember]
    public bool BackSide;
    [DataMember]
    public int Part;

    #region Constructor

    public RayHit3f(float tMax)
    {
        T = tMax;
        Point = V3f.NaN;
        Coord = V2d.NaN;
        BackSide = false;
        Part = 0;
    }

    #endregion

    #region Constants

    public static readonly RayHit3f MaxRange = new RayHit3f(float.MaxValue);

    #endregion
}

#endregion

#region FastRay3f

/// <summary>
/// A fast ray contains a ray and a number of precomputed flags and
/// fields for fast intersection computation with bounding boxes and
/// other axis-aligned sturctures such as kd-Trees.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public readonly struct FastRay3f
{
    [DataMember]
    public readonly Ray3f Ray;
    [DataMember]
    public readonly DirFlags DirFlags;
    [DataMember]
    public readonly V3f InvDir;

    #region Constructors

    public FastRay3f(Ray3f ray)
    {
        Ray = ray;
        DirFlags = ray.Direction.DirFlags();
        InvDir = 1 / ray.Direction;
    }

    public FastRay3f(V3f origin, V3f direction)
        : this(new Ray3f(origin, direction))
    { }

    #endregion

    #region Ray Arithmetics

    public readonly bool Intersects(
        Box3f box,
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
        else	// ray parallel to X-plane
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
        else	// ray parallel to Y-plane
        {
            if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                return false;
        }

        if ((dirFlags & DirFlags.PositiveZ) != 0)
        {
            {
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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
    public readonly bool Intersects(
        Box3f box,
        Box.Flags boxFlags,
        ref float tmin,
        ref float tmax
        )
    {
        var dirFlags = DirFlags;

        if ((dirFlags & DirFlags.PositiveX) != 0)
        {
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeX) != 0)
        {
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
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
                float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeY) != 0)
        {
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MaxY) != 0)
            {
                float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
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
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MaxZ) != 0)
            {
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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
    public readonly bool Intersects(
        Box3f box,
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
        else	// ray parallel to X-plane
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
        else	// ray parallel to Y-plane
        {
            if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                return false;
        }

        if ((dirFlags & DirFlags.PositiveZ) != 0)
        {
            {
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
            }
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
            }
            {
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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
    public readonly bool Intersects(
        Box3f box,
        Box.Flags boxFlags,
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
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
            }
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeX) != 0)
        {
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                float t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
            }
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                float t = (box.Max.X - Ray.Origin.X) * InvDir.X;
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
                float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
            }
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeY) != 0)
        {
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                float t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
            }
            if ((boxFlags & Box.Flags.MaxY) != 0)
            {
                float t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
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
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
            }
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                float t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
            }
            if ((boxFlags & Box.Flags.MaxZ) != 0)
            {
                float t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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

#region Ray3d

/// <summary>
/// A three-dimensional ray with an origin and a direction.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Ray3d : IEquatable<Ray3d>, IValidity, IBoundingBox3d
{
    [DataMember]
    public V3d Origin;
    [DataMember]
    public V3d Direction;

    #region Constructors

    /// <summary>
    /// Creates Ray from origin point and directional vector
    /// </summary>
    public Ray3d(V3d origin, V3d direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public static Ray3d FromEndPoints(V3d origin, V3d target) => new Ray3d(origin, target - origin);

    #endregion

    #region Constants

    /// <summary>
    /// An invalid ray has a zero direction.
    /// </summary>
    public static readonly Ray3d Invalid = new Ray3d(V3d.NaN, V3d.Zero);

    #endregion

    #region Properties

    /// <summary>
    /// A ray is valid if its direction is non-zero.
    /// </summary>
    public readonly bool IsValid { get { return Direction != V3d.Zero; } }

    /// <summary>
    /// A ray is invalid if its direction is zero.
    /// </summary>
    public readonly bool IsInvalid { get { return Direction == V3d.Zero; } }

    /// <summary>
    /// Returns true if either the origin or the direction contains any NaN value.
    /// </summary>
    public readonly bool AnyNaN { get { return Origin.AnyNaN || Direction.AnyNaN; } }

    /// <summary>
    /// Line segment from origin to origin + direction.
    /// </summary>
    public readonly Line3d Line3d => new Line3d(Origin, Origin + Direction);

    /// <summary>
    /// Returns new ray with flipped direction.
    /// </summary>
    public readonly Ray3d Reversed => new Ray3d(Origin, -Direction);

    /// <summary>
    /// Returns the ray with its directional normalized.
    /// </summary>
    public readonly Ray3d Normalized => new(Origin, Direction.Normalized);

    #endregion

    #region Ray Arithmetics

    /// <summary>
    /// Gets the point on the ray that is t * Direction from Origin.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly V3d GetPointOnRay(double t) => Origin + Direction * t;

    /// <summary>
    /// Gets the t for a point p on this ray.
    /// </summary>
    public readonly double GetT(V3d p)
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
    public readonly double GetTOfProjectedPoint(V3d p)
    {
        var v = p - Origin;
        return v.Dot(Direction) / Direction.LengthSquared;
    }

    /// <summary>
    /// Returns the ray transformed with the given matrix.
    /// This method is only valid for similarity transformations (uniform scale).
    /// </summary>
    public readonly Ray3d Transformed(M44d mat)
        => new(mat.TransformPos(Origin), mat.TransformDir(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Rot3d"/> transformation.
    /// </summary>
    public readonly Ray3d Transformed(Rot3d transform)
        => new(transform.Transform(Origin), transform.Transform(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Scale3d"/> transformation.
    /// </summary>
    public readonly Ray3d Transformed(Scale3d transform)
        => new(transform.Transform(Origin), transform.Transform(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Shift3d"/> transformation.
    /// </summary>
    public readonly Ray3d Transformed(Shift3d transform)
        => new(transform.Transform(Origin), Direction);

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    public readonly Ray3d Transformed(Euclidean3d transform)
        => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    public readonly Ray3d Transformed(Similarity3d transform)
        => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

    /// <summary>
    /// Returns the ray transformed with the given <see cref="Affine3d"/> transformation.
    /// </summary>
    public readonly Ray3d Transformed(Affine3d transform)
        => new(transform.TransformPos(Origin), transform.TransformDir(Direction));

    /// <summary>
    /// Returns the angle between this and the given <see cref="Ray3d"/> in radians.
    /// The direction vectors of the input rays have to be normalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double AngleBetweenFast(Ray3d r)
        => Direction.AngleBetweenFast(r.Direction);

    /// <summary>
    /// Returns the angle between this and the given <see cref="Ray3d"/> in radians using a numerically stable algorithm.
    /// The direction vectors of the input rays have to be normalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double AngleBetween(Ray3d r)
        => Direction.AngleBetween(r.Direction);

    #endregion

    #region Ray hit intersections

    #region Private functions

    private readonly bool ComputeHit(
          double t,
          double tmin, double tmax,
          ref RayHit3d hit)
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
            double t1, double t2,
            double tmin, double tmax,
            ref RayHit3d hit)
    {
        return t1 < t2
              ? ProcessHits(t1, t2, tmin, tmax, ref hit)
              : ProcessHits(t2, t1, tmin, tmax, ref hit);
    }


    private readonly bool ProcessHits(
            double t1, double t2,
            double tmin, double tmax,
            ref RayHit3d hit)
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
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    public readonly bool Hits(Ray3d ray, ref RayHit3d hit)
        => HitsRay(ray, 0, double.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the other ray before the parameter
    /// value contained in the supplied hit. Detailed information about
    /// the hit is returned in the supplied hit.
    /// </summary>
    public readonly bool Hits(Ray3d ray, double tmin, double tmax, ref RayHit3d hit)
        => HitsRay(ray, tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the other ray before the parameter
    /// value contained in the supplied hit. Detailed information about
    /// the hit is returned in the supplied hit.
    /// </summary>
    public readonly bool HitsRay(Ray3d ray, double tmin, double tmax, ref RayHit3d hit)
    {
        V3d d = Origin - ray.Origin;
        V3d u = Direction;
        V3d v = ray.Direction;
        V3d n = u.Cross(v);

        if (Fun.IsTiny(d.Length)) return true;
        else if (Fun.IsTiny(u.Cross(v).Length)) return false;
        else
        {
            //-t0*u + t1*v + t2*n == d
            //M = {-u,v,n}
            //M*{t0,t1,t2}T == d
            //{t0,t1,t2}T == M^-1 * d

            M33d M = new M33d
            {
                C0 = -u,
                C1 = v,
                C2 = n
            };

            if (M.Invertible)
            {
                V3d t = M.Inverse * d;
                if (Fun.IsTiny(t.Z))
                {
                    ProcessHits(t.X, double.MaxValue, tmin, tmax, ref hit);
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
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Triangle3d triangle, ref RayHit3d hit)
        => HitsTrianglePointAndEdges(
            triangle.P0, triangle.Edge01, triangle.Edge02,
            0, double.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the triangle. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Triangle3d triangle, out double t)
        => HitsTrianglePointAndEdges(
            triangle.P0, triangle.Edge01, triangle.Edge02,
            0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Triangle3d triangle, double tmin, double tmax, ref RayHit3d hit)
        => HitsTrianglePointAndEdges(
            triangle.P0, triangle.Edge01, triangle.Edge02,
            tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the triangle within [0, double.MaxValue]
    /// and before the parameter value contained in the supplied hit. Detailed
    /// information about the hit is returned in the supplied hit. In order to
    /// obtain all potential hits, the supplied hit can be initialized with
    /// RayHit3d.MaxRange. Degenerated triangles will not result in an intersection
    /// even if any edge is hit exactly.
    /// </summary>
    public readonly bool HitsTriangle(V3d p0, V3d p1, V3d p2, ref RayHit3d hit)
        => HitsTriangle(p0, p1, p2, 0, double.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
    /// Degenerated triangles will not result in an intersection even if
    /// any edge is hit exactly.
    /// </summary>
    public readonly bool HitsTriangle(
        V3d p0, V3d p1, V3d p2,
        double tmin, double tmax,
        ref RayHit3d hit
        )
    {
        V3d edge01 = p1 - p0;
        V3d edge02 = p2 - p0;
        V3d plane = Vec.Cross(Direction, edge02);
        double det = Vec.Dot(edge01, plane);
        if (det > -1e-7 && det < 1e-7) return false;
        // ray ~= paralell / Triangle
        V3d tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        double u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        double v = Vec.Dot(Direction, plane) * det;
        if (v < 0 || u + v > 1) return false;
        double t = Vec.Dot(edge02, plane) * det;
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
    /// A hit with this overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsTriangle(V3d p0, V3d p1, V3d p2, out double t)
        => HitsTriangle(p0, p1, p2, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval. Degenerated triangles will not result in an
    /// intersection even if any edge is hit exactly.
    /// </summary>
    public readonly bool HitsTriangle(
        V3d p0, V3d p1, V3d p2,
        double tmin, double tmax,
        out double t
        )
    {
        V3d edge01 = p1 - p0;
        V3d edge02 = p2 - p0;
        V3d plane = Vec.Cross(Direction, edge02);
        double det = Vec.Dot(edge01, plane);
        t = double.NaN;
        if (det > -1e-7 && det < 1e-7) return false;
        // ray ~= paralell / Triangle
        V3d tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        double u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        double v = Vec.Dot(Direction, plane) * det;
        if (v < 0 || u + v > 1) return false;
        t = Vec.Dot(edge02, plane) * det;
        return t >= tmin && t <= tmax;
    }

    /// <summary>
    /// Returns true if the ray hits the triangle within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
    /// </summary>
    public readonly bool HitsTrianglePointAndEdges(
        V3d p0, V3d edge01, V3d edge02,
        double tmin, double tmax,
        ref RayHit3d hit
        )
    {
        V3d plane = Vec.Cross(Direction, edge02);
        double det = Vec.Dot(edge01, plane);
        if (det > -1e-7 && det < 1e-7) return false;
        // ray ~= paralell / Triangle
        V3d tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        double u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        double v = Vec.Dot(Direction, plane) * det;
        if (v < 0 || u + v > 1) return false;
        double t = Vec.Dot(edge02, plane) * det;
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
    /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
    /// </summary>
    public readonly bool HitsTrianglePointAndEdges(
        V3d p0, V3d edge01, V3d edge02,
        double tmin, double tmax,
        out double t
        )
    {
        V3d plane = Vec.Cross(Direction, edge02);
        double det = Vec.Dot(edge01, plane);
        t = double.NaN;
        if (det > -1e-7 && det < 1e-7) return false;
        // ray ~= paralell / Triangle
        V3d tv = Origin - p0;
        det = 1 / det;  // det is now inverse det
        double u = Vec.Dot(tv, plane) * det;
        if (u < 0 || u > 1) return false;
        plane = Vec.Cross(tv, edge01); // plane is now qv
        double v = Vec.Dot(Direction, plane) * det;
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
    /// RayHit3d.MaxRange. A hit with this overload is considered
    /// for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Quad3d quad, ref RayHit3d hit) => HitsQuad(
        quad.P0, quad.P1, quad.P2, quad.P3,
        0, double.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. Detailed information about the hit is
    /// returned in the supplied hit. In order to obtain all potential
    /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Quad3d quad, double tmin, double tmax, ref RayHit3d hit) => HitsQuad(
        quad.P0, quad.P1, quad.P2, quad.P3,
        tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied
    /// parameter interval and before the parameter value contained
    /// in the supplied hit. The quad is considered to consist of the
    /// two triangles [p0,p1,p2] and [p0,p2,p3]. Detailed information
    /// about the hit is returned in the supplied hit. In order to obtain
    /// all potential hits, the supplied hit can be initialized with
    /// RayHit3d.MaxRange.
    /// </summary>
    public readonly bool HitsQuad(
        V3d p0, V3d p1, V3d p2, V3d p3,
        double tmin, double tmax,
        ref RayHit3d hit
        )
    {
        V3d e02 = p2 - p0;
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
    public readonly bool Hits(Quad3d quad, double tmin, double tmax, out double t)
        => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, tmin, tmax, out t);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied parameter interval.
    /// A hit with this overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Quad3d quad, out double t)
        => HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied parameter interval.
    /// A hit with this overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsQuad(V3d p0, V3d p1, V3d p2, V3d p3, out double t)
        => HitsQuad(p0, p1, p2, p3, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the quad within the supplied parameter interval.
    /// </summary>
    public readonly bool HitsQuad(V3d p0, V3d p1, V3d p2, V3d p3, double tmin, double tmax, out double t)
    {
        V3d e02 = p2 - p0;
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
            V3d center, double radius,
            double tmin, double tmax,
            ref RayHit3d hit)
    {
        V3d originSubCenter = Origin - center;
        double a = Direction.LengthSquared;
        double b = Direction.Dot(originSubCenter);
        double c = originSubCenter.LengthSquared - radius * radius;

        // --------------------- quadric equation : a t^2  + 2b t + c = 0
        double d = b * b - a * c;           // factor 2 was eliminated

        if (d < double.Epsilon)             // no root ?
            return false;                   // then exit

        if (b > 0)                        // stable way to calculate
            d = -Fun.Sqrt(d) - b;           // the roots of a quadratic
        else                                // equation
            d = Fun.Sqrt(d) - b;

        double t1 = d / a;
        double t2 = c / d;  // Vieta : t1 * t2 == c/a

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
    public readonly bool Hits(Sphere3d sphere, double tmin, double tmax, ref RayHit3d hit)
        => HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the
    /// supplied parameter interval and before the parameter value
    /// contained in the supplied hit. Note that a hit is only
    /// registered if the front or the backsurface is encountered
    /// within the interval. If there are two valid solutions, the
    /// closest will be returned. A hit with this overload is
    /// considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Sphere3d sphere, ref RayHit3d hit)
        => HitsSphere(sphere.Center, sphere.Radius, 0, double.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the
    /// supplied parameter interval. Note that a hit is only
    /// registered if the front or the backsurface is encountered
    /// within the interval. If there are two valid solutions, the
    /// closest will be returned.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Sphere3d sphere, double tmin, double tmax, out double t)
        => HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, out t);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere. Note that a hit is
    /// registered if the front or the backsurface is encountered. If there
    /// are two valid solutions, the closest will be returned. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Sphere3d sphere, out double t)
        => HitsSphere(sphere.Center, sphere.Radius, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the supplied parameter interval.
    /// Note that a hit is registered if the front or the backsurface is encountered within the
    /// interval. If there are two valid solutions, the closest will be returned. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    public readonly bool HitsSphere(V3d center, double radius, out double t)
        => HitsSphere(center, radius, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray hits the supplied sphere within the supplied parameter interval.
    /// Note that a hit is registered if the front or the backsurface is encountered within the
    /// interval. If there are two valid solutions, the closest will be returned.
    /// </summary>
    public readonly bool HitsSphere(V3d center, double radius, double tmin, double tmax, out double t)
    {
        var originSubCenter = Origin - center;
        var a = Direction.LengthSquared;
        var b = Direction.Dot(originSubCenter);
        var c = originSubCenter.LengthSquared - radius * radius;

        // --------------------- quadric equation : a t^2  + 2b t + c = 0
        var d = b * b - a * c;              // factor 2 was eliminated

        if (d >= double.Epsilon)            // no root ? -> exit
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

        t = double.NaN;
        return false;
    }

    #endregion

    #region Ray-Plane hit intersection

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsPlane(Plane3d plane, ref RayHit3d hit)
        => HitsPlane(plane, 0, double.MaxValue, ref hit);

    public readonly bool HitsPlane(Plane3d plane, double tmin, double tmax, ref RayHit3d hit)
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
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsPlane(Plane3d plane, out double t)
        => HitsPlane(plane, 0, double.MaxValue, out t);

    public readonly bool HitsPlane(Plane3d plane, double tmin, double tmax, out double t)
    {
        var dc = plane.Normal.Dot(Direction);

        // If parallel to plane
        if (dc == 0)
        {
            t = double.NaN;
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
    public readonly bool Hits(Circle3d circle, double tmin, double tmax, ref RayHit3d hit)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Circle3d circle, ref RayHit3d hit)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, double.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCircle(V3d center, V3d normal, double radius, ref RayHit3d hit)
        => HitsCircle(center, normal, radius, 0, double.MaxValue, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    public readonly bool HitsCircle(V3d center, V3d normal, double radius, double tmin, double tmax, ref RayHit3d hit)
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
            hit.Point = V3d.NaN;
            hit.T = tmax;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Circle3d circle, double tmin, double tmax, out double t)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, tmin, tmax, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Circle3d circle, out double t)
        => HitsCircle(circle.Center, circle.Normal, circle.Radius, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCircle(V3d center, V3d normal, double radius, out double t)
        => HitsCircle(center, normal, radius, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    public readonly bool HitsCircle(V3d center, V3d normal, double radius, double tmin, double tmax, out double t)
    {
        var dc = normal.Dot(Direction);
        var dw = normal.Dot(center - Origin);

        // If parallel to plane
        if (dc == 0)
        {
            t = double.NaN;
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
    public readonly bool HitsCylinder(V3d p0, V3d p1, double radius,
            double tmin, double tmax,
            ref RayHit3d hit)
    {
        var axis = new Line3d(p0, p1);
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
            var bottomPlane = new Plane3d(-axisDir, p0);
            var topPlane = new Plane3d(axisDir, p1);
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
                var distance = axis.Ray3d.GetMinimalDistanceTo(hit.Point);

                if (distance <= radius && (bottomHit || topHit))
                    return true;
            }
            else
                return true;
        }

        hit.T = tmax;
        hit.Point = V3d.NaN;
        return false;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCylinder(V3d p0, V3d p1, double radius, ref RayHit3d hit)
        => HitsCylinder(p0, p1, radius, 0, double.MaxValue, ref hit);

    public readonly bool HitsCylinder(V3d p0, V3d p1, double radius,
            double tmin, double tmax, out double t)
    {
        var axis = new Line3d(p0, p1);
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
            var bottomPlane = new Plane3d(-axisDir, p0);
            var topPlane = new Plane3d(axisDir, p1);
            var heightBottom = bottomPlane.Height(point);
            var heightTop = topPlane.Height(point);
            // t1 lies outside of caps => find closest cap hit
            if (heightBottom > 0 || heightTop > 0)
            {
                // intersect with bottom Cylinder Cap
                var bottomHit = HitsCircle(p0, -axisDir, radius, tmin, tmax, out t);
                // intersect with top Cylinder Cap
                var topHit = HitsCircle(p1, axisDir, radius, tmin, tmax, out double ttop);

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

        t = double.NaN;
        return false;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool HitsCylinder(V3d p0, V3d p1, double radius, out double t)
        => HitsCylinder(p0, p1, radius, 0, double.MaxValue, out t);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Cylinder3d cylinder, double tmin, double tmax, ref RayHit3d hit)
        => Hits(cylinder, tmin, tmax, 0, ref hit);

    /// <summary>
    /// Returns true if the ray intersects with the primitive.
    /// </summary>
    public readonly bool Hits(Cylinder3d cylinder, double tmin, double tmax, double distanceScale, ref RayHit3d hit)
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
            var bottomPlane = new Plane3d(cylinder.Circle0.Normal, cylinder.Circle0.Center);
            var topPlane = new Plane3d(cylinder.Circle1.Normal, cylinder.Circle1.Center);
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
                var distance = cylinder.Axis.Ray3d.GetMinimalDistanceTo(hit.Point);

                if (distance <= radius && (bottomHit || topHit))
                    return true;
            }
            else
                return true;
        }

        hit.T = tmax;
        hit.Point = V3d.NaN;
        return false;
    }

    /// <summary>
    /// Returns true if the ray intersects with the primitive. A hit with this
    /// overload is considered for t in [0, double.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Hits(Cylinder3d cylinder, ref RayHit3d hit)
        => Hits(cylinder, 0, double.MaxValue, ref hit);

    #endregion

    #endregion

    #region Comparison Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Ray3d a, Ray3d b)
        => (a.Origin == b.Origin) && (a.Direction == b.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Ray3d a, Ray3d b)
        => !((a.Origin == b.Origin) && (a.Direction == b.Direction));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int LexicalCompare(Ray3d other)
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
    public readonly bool Equals(Ray3d other)
        => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    public override readonly bool Equals(object other)
        => (other is Ray3d o) ? Equals(o) : false;

    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

    public static Ray3d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Ray3d(V3d.Parse(x[0]), V3d.Parse(x[1]));
    }

    #endregion

    #region IBoundingBox3d

    public readonly Box3d BoundingBox3d => Box3d.FromPoints(Origin, Direction + Origin);

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Ray3d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Ray3d a, Ray3d b, double tolerance) =>
        ApproximateEquals(a.Origin, b.Origin, tolerance) &&
        ApproximateEquals(a.Direction, b.Direction, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Ray3d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Ray3d a, Ray3d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region RayHit3d

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
public struct RayHit3d
{
    [DataMember]
    public double T;
    [DataMember]
    public V3d Point;
    [DataMember]
    public V2d Coord;
    [DataMember]
    public bool BackSide;
    [DataMember]
    public int Part;

    #region Constructor

    public RayHit3d(double tMax)
    {
        T = tMax;
        Point = V3d.NaN;
        Coord = V2d.NaN;
        BackSide = false;
        Part = 0;
    }

    #endregion

    #region Constants

    public static readonly RayHit3d MaxRange = new RayHit3d(double.MaxValue);

    #endregion
}

#endregion

#region FastRay3d

/// <summary>
/// A fast ray contains a ray and a number of precomputed flags and
/// fields for fast intersection computation with bounding boxes and
/// other axis-aligned sturctures such as kd-Trees.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public readonly struct FastRay3d
{
    [DataMember]
    public readonly Ray3d Ray;
    [DataMember]
    public readonly DirFlags DirFlags;
    [DataMember]
    public readonly V3d InvDir;

    #region Constructors

    public FastRay3d(Ray3d ray)
    {
        Ray = ray;
        DirFlags = ray.Direction.DirFlags();
        InvDir = 1 / ray.Direction;
    }

    public FastRay3d(V3d origin, V3d direction)
        : this(new Ray3d(origin, direction))
    { }

    #endregion

    #region Ray Arithmetics

    public readonly bool Intersects(
        Box3d box,
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
        else	// ray parallel to X-plane
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
        else	// ray parallel to Y-plane
        {
            if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                return false;
        }

        if ((dirFlags & DirFlags.PositiveZ) != 0)
        {
            {
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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
    public readonly bool Intersects(
        Box3d box,
        Box.Flags boxFlags,
        ref double tmin,
        ref double tmax
        )
    {
        var dirFlags = DirFlags;

        if ((dirFlags & DirFlags.PositiveX) != 0)
        {
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeX) != 0)
        {
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
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
                double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeY) != 0)
        {
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MaxY) != 0)
            {
                double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
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
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            if ((boxFlags & Box.Flags.MaxZ) != 0)
            {
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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
    public readonly bool Intersects(
        Box3d box,
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
        else	// ray parallel to X-plane
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
        else	// ray parallel to Y-plane
        {
            if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                return false;
        }

        if ((dirFlags & DirFlags.PositiveZ) != 0)
        {
            {
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
            }
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
            }
            {
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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
    public readonly bool Intersects(
        Box3d box,
        Box.Flags boxFlags,
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
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
            }
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeX) != 0)
        {
            if ((boxFlags & Box.Flags.MinX) != 0)
            {
                double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
            }
            if ((boxFlags & Box.Flags.MaxX) != 0)
            {
                double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
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
                double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
            }
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeY) != 0)
        {
            if ((boxFlags & Box.Flags.MinY) != 0)
            {
                double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
            }
            if ((boxFlags & Box.Flags.MaxY) != 0)
            {
                double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
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
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
            }
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeZ) != 0)
        {
            if ((boxFlags & Box.Flags.MinZ) != 0)
            {
                double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
            }
            if ((boxFlags & Box.Flags.MaxZ) != 0)
            {
                double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
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


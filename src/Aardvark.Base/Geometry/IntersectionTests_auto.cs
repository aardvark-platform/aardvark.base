using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    public static partial class GeometryFun
    {
        private static readonly int[] c_cubeEdgeVertex0 =
            new int[] { 0, 2, 4, 6, 0, 1, 4, 5, 0, 1, 2, 3 };

        private static readonly int[] c_cubeEdgeVertex1 =
            new int[] { 1, 3, 5, 7, 2, 3, 6, 7, 4, 5, 6, 7 };

        // Contains-tests should return true if the contained object is
        // either entirely inside the containing object or lies on the
        // boundary of the containing object.

        #region Triangle2f contains V2f

        public static bool Contains(
            this Triangle2f triangle, V2f point
            )
        {
            var v0p = point - triangle.P0;
            return triangle.Line01.LeftValueOfDir(v0p) >= 0
                    && triangle.Line02.RightValueOfDir(v0p) >= 0
                    && triangle.Line12.LeftValueOfPos(point) >= 0;
        }

        #endregion

        #region Triangle2f contains Line2f (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Triangle2f triangle, Line2f linesegment)
            => triangle.Contains(linesegment.P0) && triangle.Contains(linesegment.P1);

        #endregion

        #region Triangle2f contains Triangle2f (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Triangle2f triangle, Triangle2f other)
            => triangle.Contains(other.P0) && triangle.Contains(other.P1) && triangle.Contains(other.P2);

        #endregion

        #region Triangle2f contains Quad2f (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Triangle2f triangle, Quad2f q)
            => triangle.Contains(q.P0) && triangle.Contains(q.P1) && triangle.Contains(q.P2) && triangle.Contains(q.P3);

        #endregion

        #region Triangle2f contains Circle2f - TODO

        public static bool Contains(this Triangle2f triangle, Circle2f circle)
            => throw new NotImplementedException();

        #endregion


        #region Circle2f contains V2f (sm)

        /// <summary>
        /// True if point p is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2f circle, V2f p)
            => (p - circle.Center).LengthSquared <= circle.RadiusSquared;

        #endregion

        #region Circle2f contains Line2f (sm)

        /// <summary>
        /// True if line segment is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2f circle, Line2f l)
            => circle.Contains(l.P0) && circle.Contains(l.P1);

        #endregion

        #region Circle2f contains Triangle2f (sm)

        /// <summary>
        /// True if triangle is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2f circle, Triangle2f t)
            => circle.Contains(t.P0) && circle.Contains(t.P1) && circle.Contains(t.P2);

        #endregion

        #region Circle2f contains Quad2f (sm)

        /// <summary>
        /// True if quad is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2f circle, Quad2f q)
            => circle.Contains(q.P0) && circle.Contains(q.P1) && circle.Contains(q.P2) && circle.Contains(q.P3);

        #endregion

        #region Circle2f contains Circle2f (sm)

        /// <summary>
        /// True if other circle is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2f circle, Circle2f other)
            => (other.Center - circle.Center).Length + other.Radius <= circle.Radius;

        #endregion


        #region Quad2f contains V2f (haaser)

        /// <summary>
        /// True if point is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2f quad, V2f point)
        {
            return LeftValOfPos(0, 1, ref point) >= 0 &&
                   LeftValOfPos(1, 2, ref point) >= 0 &&
                   LeftValOfPos(2, 3, ref point) >= 0 &&
                   LeftValOfPos(3, 0, ref point) >= 0;

            float LeftValOfPos(int i0, int i1, ref V2f p)
                => (p.X - quad[i0].X) * (quad[i0].Y - quad[i1].Y) + (p.Y - quad[i0].Y) * (quad[i1].X - quad[i0].X);
        }

        #endregion

        #region Quad2f contains Line2f - TODO

        /// <summary>
        /// True if line segment is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2f quad, Line2f l)
            => throw new NotImplementedException();

        #endregion

        #region Quad2f contains Triangle2f - TODO

        /// <summary>
        /// True if triangle is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2f quad, Triangle2f t)
            => throw new NotImplementedException();

        #endregion

        #region Quad2f contains Quad2f - TODO

        /// <summary>
        /// True if other quad is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2f quad, Quad2f q)
            => throw new NotImplementedException();

        #endregion

        #region Quad2f contains Circle2f - TODO

        /// <summary>
        /// True if circle is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2f quad, Circle2f other)
            => throw new NotImplementedException();

        #endregion


        #region Box3f contains Quad3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
            this Box3f box, Quad3f quad
            )
        {
            return box.Contains(quad.P0)
                    && box.Contains(quad.P1)
                    && box.Contains(quad.P2)
                    && box.Contains(quad.P3);
        }

        #endregion

        #region Box3f contains Triangle3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
             this Box3f box, Triangle3f triangle
            )
        {
            return box.Contains(triangle.P0)
                    && box.Contains(triangle.P1)
                    && box.Contains(triangle.P2);
        }

        #endregion

        #region Box3f contains Sphere3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
             this Box3f box, Sphere3f sphere
            )
        {
            return box.Contains(sphere.Center)
                    && !box.Intersects(sphere);
        }

        #endregion

        #region Box3f contains Cylinder3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
            this Box3f box, Cylinder3f cylinder
           )
        {
            return box.Contains(cylinder.Center)
                    && !box.Intersects(cylinder);
        }

        #endregion


        #region Hull3f contains V3f

        /// <summary>
        /// Hull normals are expected to point outside.
        /// </summary>
        public static bool Contains(this Hull3f hull, V3f point)
        {
            var planes = hull.PlaneArray;
            for (var i = 0; i < planes.Length; i++)
            {
                if (planes[i].Height(point) > 0)
                    return false;
            }
            return true;
        }

        #endregion

        #region Hull3f contains Sphere3f (sm)

        /// <summary>
        /// Hull normals are expected to point outside.
        /// </summary>
        public static bool Contains(this Hull3f hull, Sphere3f sphere)
        {
            var planes = hull.PlaneArray;
            var negativeRadius = -sphere.Radius;
            for (var i = 0; i < planes.Length; i++)
            {
                if (planes[i].Height(sphere.Center) > negativeRadius)
                    return false;
            }
            return true;
        }

        #endregion


        #region Polygon2f contains V2f (haaser)

        internal static V3i InsideTriangleFlags(ref V2f p0, ref V2f p1, ref V2f p2, ref V2f point)
        {
            V2f n0 = new V2f(p0.Y - p1.Y, p1.X - p0.X);
            V2f n1 = new V2f(p1.Y - p2.Y, p2.X - p1.X);
            V2f n2 = new V2f(p2.Y - p0.Y, p0.X - p2.X);

            int t0 = Fun.Sign(n0.Dot(point - p0));
            int t1 = Fun.Sign(n1.Dot(point - p1));
            int t2 = Fun.Sign(n2.Dot(point - p2));

            if (t0 == 0) t1 = 1;
            if (t1 == 0) t1 = 1;
            if (t2 == 0) t2 = 1;

            return new V3i(t0, t1, t2);
        }

        internal static V3i InsideTriangleFlags(ref V2f p0, ref V2f p1, ref V2f p2, ref V2f point, int t0)
        {
            V2f n1 = new V2f(p1.Y - p2.Y, p2.X - p1.X);
            V2f n2 = new V2f(p2.Y - p0.Y, p0.X - p2.X);

            int t1 = Fun.Sign(n1.Dot(point - p1));
            int t2 = Fun.Sign(n2.Dot(point - p2));

            if (t1 == 0) t1 = 1;
            if (t2 == 0) t2 = 1;

            return new V3i(t0, t1, t2);
        }

        /// <summary>
        /// Returns true if the Polygon2f contains the given point.
        /// Works with all (convex and non-convex) Polygons.
        /// Assumes that the Vertices of the Polygon are sorted counter clockwise
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon2f poly, V2f point)
        {
            return poly.Contains(point, true);
        }

        /// <summary>
        /// Returns true if the Polygon2f contains the given point.
        /// Works with all (convex and non-convex) Polygons.
        /// CCW represents the sorting order of the Polygon-Vertices (true -> CCW, false -> CW)
        /// </summary>
        public static bool Contains(this Polygon2f poly, V2f point, bool CCW)
        {
            int pc = poly.PointCount;
            if (pc < 3)
                return false;
            int counter = 0;
            V2f p0 = poly[0], p1 = poly[1], p2 = poly[2];
            V3i temp = InsideTriangleFlags(ref p0, ref p1, ref p2, ref point);
            int t2_cache = temp.Z;
            if (temp.X == temp.Y && temp.Y == temp.Z) counter += temp.X;
            for (int pi = 3; pi < pc; pi++)
            {
                p1 = p2; p2 = poly[pi];
                temp = InsideTriangleFlags(ref p0, ref p1, ref p2, ref point, -t2_cache);
                t2_cache = temp.Z;
                if (temp.X == temp.Y && temp.Y == temp.Z) counter += temp.X;
            }
            if (CCW) return counter > 0;
            else return counter < 0;
        }

        #endregion


        #region Plane3f +- eps contains V3f (sm)

        /// <summary>
        /// Returns true if point is within given eps to plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Plane3f plane, float eps, V3f point)
        {
            var d = plane.Height(point);
            return d >= -eps && d <= eps;
        }

        #endregion

        #region Plane3f +- eps contains Box3f (sm)

        /// <summary>
        /// Returns true if the space within eps to a plane fully contains the given box.
        /// </summary>
        public static bool Contains(this Plane3f plane, float eps, Box3f box)
        {
            var corners = box.ComputeCorners();
            for (var i = 0; i < 8; i++)
            {
                var d = plane.Height(corners[i]);
                if (d < -eps || d > eps) return false;
            }
            return true;
        }

        #endregion

        #region Polygon3f +- eps contains V3f (sm)

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon3f polygon, float eps, V3f point, out float distance)
        {
            var plane = polygon.GetPlane3f();
            distance = plane.Height(point);
            if (distance < -eps || distance > eps) return false;
            var w2p = plane.GetWorldToPlane();
            var poly2d = new Polygon2f(polygon.GetPointArray().Map(p => w2p.TransformPos(p).XY));
            return poly2d.Contains(w2p.TransformPos(point).XY);
        }

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon3f polygon, Plane3f supportingPlane, Euclidean3f world2plane, Polygon2f poly2d, float eps, V3f point, out float distance)
        {
            distance = supportingPlane.Height(point);
            if (distance < -eps || distance > eps) return false;
            return poly2d.Contains(world2plane.TransformPos(point).XY);
        }

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon3f polygon, Plane3f supportingPlane, M44f world2plane, Polygon2f poly2d, float eps, V3f point, out float distance)
        {
            distance = supportingPlane.Height(point);
            if (distance < -eps || distance > eps) return false;
            return poly2d.Contains(world2plane.TransformPos(point).XY);
        }

        #endregion


        // Intersection tests

        #region Line2f intersects Line2f

        /// <summary>
        /// Returns true if the two line segments intersect.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Line2f l0, Line2f l1)
            => l0.IntersectsLine(l1.P0, l1.P1, out V2f _);

        /// <summary>
        /// Returns true if the two line segments intersect.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Line2f l0, Line2f l1, out V2f p)
            => l0.IntersectsLine(l1.P0, l1.P1, out p);

        /// <summary>
        /// Returns true if the two line segments intersect within given tolerance.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Line2f l0, Line2f l1, float absoluteEpsilon, out V2f p)
            => l0.IntersectsLine(l1.P0, l1.P1, absoluteEpsilon, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this Line2f line, V2f p0, V2f p1)
            => line.IntersectsLine(p0, p1, out _);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this Line2f line, V2f p0, V2f p1, out V2f p)
            => line.IntersectsLine(p0, p1, false, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// The intersection point is returned in p.
        /// If overlapping is true and the segments are parallel, then
        /// the closest point to p0 of the intersection range is returned in p.
        /// </summary>
        public static bool IntersectsLine(
            this Line2f line,
            V2f p0, V2f p1,
            bool overlapping,
            out V2f p
            )
        {
            var a = line.P0 - p0;

            if (Fun.IsTiny(a.LengthSquared))
            {
                p = p0;
                return true;
            }

            var u = line.P1 - line.P0;
            var v = p1 - p0;
            var lu = u.Length;

            var cross = u.X * v.Y - u.Y * v.X;
            if (!Fun.IsTiny(cross))
            {
                // non-parallel lines
                cross = 1 / cross;
                var t0 = (a.Y * v.X - a.X * v.Y) * cross;
                if (t0 > 1 || t0 < 0)
                {
                    p = V2f.NaN;
                    return false;
                }

                float t1 = (a.Y * u.X - a.X * u.Y) * cross;
                if (t1 > 1 || t1 < 0)
                {
                    p = V2f.NaN;
                    return false;
                }

                p = line.P0 + u * t0;
                return true;
            }
            else
            {
                // parallel lines
                if (!overlapping)
                {
                    p = V2f.NaN;
                    return false;
                }

                var normalizedDirection = u / lu;

                var r0 = new Range1f(0, lu);
                var r1 = new Range1f((p0 - line.P0).Dot(normalizedDirection), (p1 - line.P0).Dot(normalizedDirection));
                r1.Repair();

                if (r0.Intersects(r1, 0, out Range1f result))
                {
                    p = line.P0 + normalizedDirection * result.Min;
                    return true;
                }

                p = V2f.NaN;
                return false;
            }
        }

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1 within given tolerance.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this Line2f line, V2f p0, V2f p1, float absoluteEpsilon, out V2f p)
            => line.IntersectsLine(p0, p1, absoluteEpsilon, false, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1 within given tolerance.
        /// The intersection point is returned in p.
        /// If overlapping is true and the segments are parallel, then
        /// the closest point to p0 of the intersection range is returned in p.
        /// </summary>
        public static bool IntersectsLine(
            this Line2f line,
            V2f p0, V2f p1,
            float absoluteEpsilon,
            bool overlapping,
            out V2f p
            )
        {
            var a = line.P0 - p0;

            if (Fun.IsTiny(a.LengthSquared))
            {
                p = p0;
                return true;
            }

            var u = line.P1 - line.P0;
            var v = p1 - p0;

            var lu = u.Length;
            var lv = v.Length;
            var relativeEpsilonU = absoluteEpsilon / lu;
            var RelativeEpsilonV = absoluteEpsilon / lv;

            var cross = u.X * v.Y - u.Y * v.X;
            if (!Fun.IsTiny(cross))
            {
                // non-parallel lines
                cross = 1 / cross;
                var t0 = (a.Y * v.X - a.X * v.Y) * cross;
                if (t0 > 1 + relativeEpsilonU || t0 < -relativeEpsilonU)
                {
                    p = V2f.NaN;
                    return false;
                }

                var t1 = (a.Y * u.X - a.X * u.Y) * cross;
                if (t1 > 1 + RelativeEpsilonV || t1 < -RelativeEpsilonV)
                {
                    p = V2f.NaN;
                    return false;
                }

                p = line.P0 + u * t0;
                return true;
            }
            else
            {
                // parallel lines
                if (!overlapping)
                {
                    p = V2f.NaN;
                    return false;
                }

                var normalizedDirection = u / lu;

                var r0 = new Range1f(0, lu);
                var r1 = new Range1f((p0 - line.P0).Dot(normalizedDirection), (p1 - line.P0).Dot(normalizedDirection));
                r1.Repair();

                if (r0.Intersects(r1, absoluteEpsilon, out Range1f result))
                {
                    p = line.P0 + normalizedDirection * result.Min;
                    return true;
                }

                p = V2f.NaN;
                return false;
            }
        }

        #endregion

        #region Ray2f intersects Line2f

        /// <summary>
        /// Returns true if the Ray and the Line intersect.
        /// ATTENTION: Both-Sided Ray
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray2f ray,
            Line2f line
            )
        {
            return ray.IntersectsLine(line.P0, line.P1, out _);
        }

        /// <summary>
        /// returns true if the Ray and the Line intersect.
        /// t holds the smallest Intersection-Parameter for the Ray
        /// ATTENTION: t can be negative
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray2f ray,
            Line2f line,
            out float t
            )
        {
            return ray.IntersectsLine(line.P0, line.P1, out t);
        }


        /// <summary>
        /// returns true if the Ray and the Line(p0,p1) intersect.
        /// ATTENTION: Both-Sided Ray
        /// </summary>
        public static bool IntersectsLine(
            this Ray2f ray,
            V2f p0, V2f p1
            )
        {
            V2f n = new V2f(-ray.Direction.Y, ray.Direction.X);

            float d0 = n.Dot(p0 - ray.Origin);
            float d1 = n.Dot(p1 - ray.Origin);

            if (d0.Sign() != d1.Sign()) return true;
            else if (Fun.IsTiny(d0) && Fun.IsTiny(d1)) return true;
            else return false;
        }


        /// <summary>
        /// returns true if the Ray and the Line(p0,p1) intersect.
        /// t holds the Intersection-Parameter for the Ray
        /// If both Line-Points are on the Ray no Intersection is returned
        /// ATTENTION: t can be negative
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this Ray2f ray,
            V2f p0, V2f p1,
            out float t
            )
        {
            return ray.IntersectsLine(p0, p1, false, out t);
        }

        /// <summary>
        /// returns true if the Ray and the Line(p0,p1) intersect.
        /// if overlapping is true t holds the smallest Intersection-Parameter for the Ray
        /// if overlagging is false and both Line-Points are on the Ray no Intersection is returned
        /// ATTENTION: t can be negative
        /// </summary>
        public static bool IntersectsLine(
            this Ray2f ray,
            V2f p0, V2f p1,
            bool overlapping,
            out float t
            )
        {
            V2f a = p0 - ray.Origin;
            V2f u = p1 - p0;
            V2f v = ray.Direction;
            float lv2 = v.LengthSquared;


            float cross = u.X * v.Y - u.Y * v.X;
            float n = a.Y * u.X - a.X * u.Y;

            if (!Fun.IsTiny(cross))
            {
                cross = 1 / cross;

                float t0 = (a.Y * v.X - a.X * v.Y) * cross;
                if (t0 >= 0 && t0 <= 1)
                {
                    t = n * cross;
                    return true;
                }

                t = float.NaN;
                return false;
            }

            if (Fun.IsTiny(n) && overlapping)
            {
                float ta = v.Dot(a) / lv2;
                float tb = v.Dot(p1 - ray.Origin) / lv2;

                if ((ta < 0 && tb > 0) || (ta > 0 && tb < 0))
                {
                    t = 0;
                    return true;
                }
                else
                {
                    if (ta >= 0) t = Fun.Min(ta, tb);
                    else t = Fun.Max(ta, tb);

                    return true;
                }
            }

            t = float.NaN;
            return false;
        }



        #endregion

        #region Ray2f intersects Ray2f

        /// <summary>
        /// Returns true if the Rays intersect
        /// ATTENTION: Both-Sided Rays
        /// </summary>
        public static bool Intersects(
            this Ray2f r0,
            Ray2f r1
            )
        {
            if (!r0.Direction.IsParallelTo(r1.Direction)) return true;
            else
            {
                V2f n0 = new V2f(-r0.Direction.Y, r0.Direction.X);

                if (Fun.IsTiny(n0.Dot(r1.Origin - r0.Origin))) return true;
                else return false;
            }
        }

        /// <summary>
        /// Returns true if the Rays intersect
        /// t0 and t1 are the corresponding Ray-Parameters for the Intersection
        /// ATTENTION: Both-Sided Rays
        /// </summary>
        public static bool Intersects(
            this Ray2f r0, Ray2f r1,
            out float t0, out float t1
            )
        {
            V2f a = r0.Origin - r1.Origin;

            if (r0.Origin.ApproximateEquals(r1.Origin, Constant<float>.PositiveTinyValue))
            {
                t0 = 0;
                t1 = 0;
                return true;
            }

            V2f u = r0.Direction;
            V2f v = r1.Direction;

            float cross = u.X * v.Y - u.Y * v.X;

            if (!Fun.IsTiny(cross))
            {
                //Rays not parallel
                cross = 1 / cross;

                t0 = (a.Y * v.X - a.X * v.Y) * cross;
                t1 = (a.Y * u.X - a.X * u.Y) * cross;
                return true;
            }
            else
            {
                t0 = float.NaN;
                t1 = float.NaN;
                //Rays are parallel
                if (Fun.IsTiny(a.Y * u.X - a.X * u.Y)) return true;
                else return false;
            }
        }

        /// <summary>
        /// Returns true if the Rays intersect.
        /// </summary>
        public static bool Intersects(this Ray2f r0, Ray2f r1, out float t)
        {
            V2f a = r1.Origin - r0.Origin;
            if (a.Abs().AllSmaller(Constant<float>.PositiveTinyValue))
            {
                t = 0;
                return true; // Early exit when rays have same origin
            }

            float cross = r0.Direction.Dot270(r1.Direction);
            if (!Fun.IsTiny(cross)) // Rays not parallel
            {
                t = r1.Direction.Dot90(a) / cross;
                return true;
            }
            else // Rays are parallel
            {
                t = float.NaN;
                return false;
            }
        }

        #endregion


        #region Plane2f intersects Line2f

        /// <summary>
        /// Returns true if the Plane2f and the Line2f intersect or the Line2f
        /// lies completely in the Plane's Epsilon-Range
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        public static bool Intersects(
            this Plane2f plane,
            Line2f line,
            float absoluteEpsilon
            )
        {
            float lengthOfNormal2 = plane.Normal.LengthSquared;
            float d0 = plane.Height(line.P0);
            float d1 = plane.Height(line.P1);

            return d0 * d1 < absoluteEpsilon * absoluteEpsilon * lengthOfNormal2;
        }

        /// <summary>
        /// Returns true if the Plane2f and the Line2f intersect
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Plane2f plane,
            Line2f line
            )
        {
            return plane.IntersectsLine(line.P0, line.P1);
        }


        /// <summary>
        /// Returns true if the Plane2f and the line between p0 and p1 intersect
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this Plane2f plane,
            V2f p0, V2f p1
            )
        {
            float d0 = plane.Height(p0);
            float d1 = plane.Height(p1);

            return d0 * d1 <= 0;
        }

        /// <summary>
        /// Returns true if the Plane2f and the Line2f intersect
        /// point holds the Intersection-Point. If no Intersection is found point is V2f.NaN
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Plane2f plane,
            Line2f line,
            out V2f point
            )
        {
            return plane.Intersects(line, 0, out point);
        }

        /// <summary>
        /// Returns true if the Plane2f and the Line2f intersect or the Line2f
        /// lies completely in the Plane's Epsilon-Range
        /// point holds the Intersection-Point. If the Line2f is inside Epsilon point holds the centroid of the Line2f
        /// If no Intersection is found point is V2f.NaN
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        public static bool Intersects(
            this Plane2f plane,
            Line2f line,
            float absoluteEpsilon,
            out V2f point
            )
        {
            float h0 = plane.Height(line.P0);
            float h1 = plane.Height(line.P1);

            int s0 = (h0 > -absoluteEpsilon ? (h0 < absoluteEpsilon ? 0 : 1) : -1);
            int s1 = (h1 > -absoluteEpsilon ? (h1 < absoluteEpsilon ? 0 : 1) : -1);

            if (s0 == s1)
            {
                if (s0 != 0)
                {
                    point = V2f.NaN;
                    return false;
                }
                else
                {
                    point = (line.P0 + line.P1) * 0.5f;
                    return true;
                }
            }
            else
            {
                if (s0 == 0)
                {
                    point = line.P0;
                    return true;
                }
                else if (s1 == 0)
                {
                    point = line.P1;
                    return true;
                }
                else
                {
                    V2f dir = line.Direction;
                    float no = plane.Normal.Dot(line.P0);
                    float nd = plane.Normal.Dot(dir);
                    float t = (plane.Distance - no) / nd;

                    point = line.P0 + t * dir;
                    return true;
                }
            }
        }



        #endregion

        #region Plane2f intersects Ray2f

        /// <summary>
        /// Returns true if the Plane2f and the Ray2f intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2f plane, Ray2f ray)
            => !plane.Normal.IsOrthogonalTo(ray.Direction);

        /// <summary>
        /// Returns true if the Plane2f and the Ray2f intersect.
        /// t holds the ray paramater of the intersection point if the intersection is found (else Double.PositiveInfinity).
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2f plane, Ray2f ray, out float t)
        {
            float dot = Vec.Dot(plane.Normal, ray.Direction);
            if (Fun.IsTiny(dot))
            {
                t = float.PositiveInfinity;
                return false;
            }
            t = -plane.Height(ray.Origin) / dot;
            return true;
        }

        /// <summary>
        /// Returns true if the Plane2f and the Ray2f intersect.
        /// t and p hold the ray paramater and point of the intersection if one is found (else Double.PositiveInfinity and V2f.PositiveInfinity).
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2f plane, Ray2f ray, out float t, out V2f p)
        {
            bool result = Intersects(plane, ray, out t);
            p = ray.Origin + t * ray.Direction;
            return result;
        }

        /// <summary>
        /// Returns the intersection point of the given Plane2f and Ray2f, or V2f.PositiveInfinity if ray is parallel to plane.
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f Intersect(this Plane2f plane, Ray2f ray)
        {
            float dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot)) return V2f.PositiveInfinity;
            return ray.GetPointOnRay(-plane.Height(ray.Origin) / dot);
        }

        #endregion

        #region Plane2f intersects Plane2f

        /// <summary>
        /// Returns true if the two Plane2ds intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2f p0, Plane2f p1)
        {
            var hit = p0.Coefficients.Cross(p1.Coefficients);
            return !hit.Z.IsTiny();
        }

        /// <summary>
        /// Returns true if the two Plane2ds intersect.
        /// Point holds the intersection point if an intersection is found.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2f p0, Plane2f p1, out V2f point)
        {
            var hit = p0.Coefficients.Cross(p1.Coefficients);
            point = hit.XY / hit.Z;

            return !hit.Z.IsTiny();
        }

        #endregion

        #region Plane2f intersects IEnumerable<V2f>

        /// <summary>
        /// returns true if the Plane2f divides the Point-Cloud
        /// </summary>
        public static bool Divides(
            this Plane2f plane,
            IEnumerable<V2f> data
            )
        {
            int first = int.MinValue;
            foreach (var p in data)
            {
                if (first == int.MinValue)
                {
                    first = plane.Height(p).Sign();
                }
                else
                {
                    if (plane.Height(p).Sign() != first) return true;
                }
            }

            return false;
        }

        #endregion


        #region Circle2f intersects Circle2f (sm)

        /// <summary>
        /// Returns true if the circles intersect, or one contains the other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Circle2f c0, Circle2f c1)
            => (c0.Center - c1.Center).LengthSquared <= (c0.Radius + c1.Radius).Square();

        #endregion


        #region Triangle2f intersects Line2f

        /// <summary>
        /// Returns true if the triangle and the line intersect or the triangle contains the line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle2f triangle,
            Line2f line
            )
        {
            return triangle.IntersectsLine(line.P0, line.P1);
        }

        /// <summary>
        /// Returns true if the triangle and the line between p0 and p1 intersect or the triangle contains the line
        /// </summary>
        public static bool IntersectsLine(
            this Triangle2f triangle,
            V2f p0, V2f p1
            )
        {
            if(triangle.Contains(p0))return true;
            if(triangle.Contains(p1))return true;

            if(triangle.Line01.IntersectsLine(p0,p1))return true;
            if(triangle.Line12.IntersectsLine(p0,p1))return true;
            if(triangle.Line20.IntersectsLine(p0,p1))return true;

            return false;
        }

        #endregion

        #region Triangle2f intersects Ray2f

        /// <summary>
        /// Returns true if the Triangle and the Ray intersect
        /// </summary>
        public static bool Intersects(
            this Triangle2f triangle,
            Ray2f ray
            )
        {
            if (triangle.Contains(ray.Origin)) return true;

            if (ray.IntersectsLine(triangle.P0, triangle.P1)) return true;
            if (ray.IntersectsLine(triangle.P1, triangle.P2)) return true;
            if (ray.IntersectsLine(triangle.P2, triangle.P0)) return true;

            return false;
        }

        #endregion

        #region Triangle2f intersects Plane2f

        /// <summary>
        /// returns true if the Triangle2f and the Plane2f intersect
        /// </summary>
        public static bool Intersects(
            this Triangle2f triangle,
            Plane2f plane
            )
        {
            if (plane.Intersects(triangle.Line01)) return true;
            if (plane.Intersects(triangle.Line12)) return true;
            if (plane.Intersects(triangle.Line20)) return true;

            return false;
        }

        #endregion

        #region Triangle2f intersects Triangle2f

        /// <summary>
        /// Returns true if the triangles intersect or one contains the other.
        /// </summary>
        public static bool Intersects(this Triangle2f t0, Triangle2f t1)
        {
            var l = t0.Line01;
            if (l.IntersectsLine(t1.P0, t1.P1)) return true;
            if (l.IntersectsLine(t1.P1, t1.P2)) return true;
            if (l.IntersectsLine(t1.P2, t1.P0)) return true;

            l = t0.Line12;
            if (l.IntersectsLine(t1.P0, t1.P1)) return true;
            if (l.IntersectsLine(t1.P1, t1.P2)) return true;
            if (l.IntersectsLine(t1.P2, t1.P0)) return true;

            l = t0.Line20;
            if (l.IntersectsLine(t1.P0, t1.P1)) return true;
            if (l.IntersectsLine(t1.P1, t1.P2)) return true;
            if (l.IntersectsLine(t1.P2, t1.P0)) return true;

            if (t0.Contains(t1.P0)) return true;
            if (t1.Contains(t0.P0)) return true;

            return false;
        }

        #endregion


        #region Box2f intersects Line2f

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool Intersects(this Box2f box, Line2f line)
        {
            var out0 = box.OutsideFlags(line.P0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(line.P1); if (out1 == 0) return true;
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
                this Box2f box, Line2f line, Box.Flags out0, Box.Flags out1)
        {
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        private static bool IntersectsLine(
                this Box2f box, V2f p0, V2f p1)
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            return box.IntersectsLine(p0, p1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        private static bool IntersectsLine(
                this Box2f box, V2f p0, V2f p1,
                Box.Flags out0, Box.Flags out1)
        {
            if ((out0 & out1) != 0) return false;

            V2f min = box.Min;
            V2f max = box.Max;
            var bf = out0 | out1;

            if ((bf & Box.Flags.X) != 0)
            {
                float dx = p1.X - p0.X;
                if ((bf & Box.Flags.MinX) != 0)
                {
                    if (dx == 0 && p0.X < min.X) return false;
                    float t = (min.X - p0.X) / dx;
                    V2f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinX) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxX) != 0)
                {
                    if (dx == 0 && p0.X > max.X) return false;
                    float t = (max.X - p0.X) / dx;
                    V2f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxX) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Y) != 0)
            {
                float dy = p1.Y - p0.Y;
                if ((bf & Box.Flags.MinY) != 0)
                {
                    if (dy == 0 && p0.Y < min.Y) return false;
                    float t = (min.Y - p0.Y) / dy;
                    V2f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinY) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxY) != 0)
                {
                    if (dy == 0 && p0.Y > max.Y) return false;
                    float t = (max.Y - p0.Y) / dy;
                    V2f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxY) == 0)
                        return true;
                }
            }
            return false;
        }


        #endregion

        #region Box2f intersects Ray2f

        /// <summary>
        /// Returns true if the box and the ray intersect
        /// </summary>
        public static bool Intersects(
            this Box2f box,
            Ray2f ray
            )
        {
            /*
             * Getting a Normal-Vector for the Ray and calculating
             * the Normal Distances for every Box-Point:
             */
            V2f n = new V2f(-ray.Direction.Y, ray.Direction.X);

            float d0 = n.Dot(box.Min - ray.Origin);                                            //n.Dot(box.p0 - ray.Origin)
            float d1 = n.X * (box.Max.X - ray.Origin.X) + n.Y * (box.Min.Y - ray.Origin.Y);    //n.Dot(box.p1 - ray.Origin)
            float d2 = n.Dot(box.Max - ray.Origin);                                            //n.Dot(box.p2 - ray.Origin)
            float d3 = n.X * (box.Min.X - ray.Origin.X) + n.Y * (box.Max.Y - ray.Origin.Y);    //n.Dot(box.p3 - ray.Origin)

            /*
             * If Zero lies in the Range of the Distances there
             * have to be Points on both sides of the Ray.
             * This means the Box and the Ray have an Intersection
             */

            Range1f r = new Range1f(d0, d1, d2, d3);
            return r.Contains(0);
        }

        #endregion

        #region Box2f intersects Plane2f

        /// <summary>
        /// returns true if the box and the plane intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box2f box,
            Plane2f plane
            )
        {
            //UNTESTED
            return plane.Divides(box.ComputeCorners());
        }


        /// <summary>
        /// NOT TESTED YET.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box2f box,
            Plane2f plane,
            out Line2f line)
        {
            return Intersects(
                    plane.Normal.X, plane.Normal.Y, plane.Distance,
                    box.Min.X, box.Min.Y, box.Max.X, box.Max.Y,
                    out line);
        }

        /// <summary>
        /// Intersects an infinite line given by its normal vector [nx, ny]
        /// and its distance to the origin d, with an axis aligned box given
        /// by it minimal point [xmin, ymin] and its maximal point
        /// [xmax, ymax]. Returns true if there is an intersection and computes
        /// the actual intersection line.
        /// NOT TESTED YET.
        /// </summary>
        public static bool Intersects(
                float nx, float ny, float d,
                float xmin, float ymin, float xmax, float ymax,
                out Line2f line)
        {
            if (nx.IsTiny()) // horizontal
            {
                if (d <= ymin || d >= ymax) { line = default; return false; }
                line = new Line2f(new V2f(xmin, d), new V2f(xmax, d));
                return true;
            }

            if (ny.IsTiny()) // vertical
            {
                if (d <= xmin || d >= xmax) { line = default; return false; }
                line = new Line2f(new V2f(d, ymin), new V2f(d, ymax));
                return true;
            }

            if (nx.Sign() != ny.Sign())
            {
                float x0 = (d - ny * ymin) / nx;
                if (x0 >= xmax) { line = default; return false; }
                if (x0 > xmin) xmin = x0;
                float x1 = (d - ny * ymax) / nx;
                if (x1 <= xmin) { line = default; return false; }
                if (x1 < xmax) xmax = x1;

                float y0 = (d - nx * xmin) / ny;
                if (y0 >= ymax) { line = default; return false; }
                if (y0 > ymin) ymin = y0;
                float y1 = (d - nx * xmax) / ny;
                if (y1 <= ymin) { line = default; return false; }
                if (y1 < ymax) ymax = y1;

                line = new Line2f(new V2f(xmin, ymin), new V2f(xmax, ymax));
            }
            else
            {
                float x0 = (d - ny * ymax) / nx;
                if (x0 >= xmax) { line = default; return false; }
                if (x0 > xmin) xmin = x0;
                float x1 = (d - ny * ymin) / nx;
                if (x1 <= xmin) { line = default; return false; }
                if (x1 < xmax) xmax = x1;
                float y0 = (d - nx * xmax) / ny;
                if (y0 >= ymax) { line = default; return false; }
                if (y0 > ymin) ymin = y0;
                float y1 = (d - nx * xmin) / ny;
                if (y1 <= ymin) { line = default; return false; }
                if (y1 < ymax) ymax = y1;

                line = new Line2f(new V2f(xmax, ymin), new V2f(xmin, ymax));
            }
            return true;
        }

        #endregion

        #region Box2f intersects Triangle2f

        /// <summary>
        /// Returns true if the Box and the Triangle intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box2f box,
            Triangle2f triangle
            )
        {
            return box.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2);
        }

        /// <summary>
        /// Returns true if the Box and the Triangle intersect
        /// </summary>
        public static bool IntersectsTriangle(
            this Box2f box,
            V2f p0, V2f p1, V2f p2
            )
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            var out2 = box.OutsideFlags(p2); if (out2 == 0) return true;

            return box.IntersectsTriangle(p0, p1, p2, out0, out1, out2);
        }

        /// <summary>
        /// Returns true if the Box and the Triangle intersect. The outside flags
        /// of the end points of the Triangle with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        private static bool IntersectsTriangle(
            this Box2f box, V2f p0, V2f p1, V2f p2,
            Box.Flags out0, Box.Flags out1, Box.Flags out2
            )
        {

            /* ---------------------------------------------------------------
                If all of the points of the triangle are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2) != 0) return false;


            /* ---------------------------------------------------------------
               If two points of the triangle are not on the same side
               outside the box, it is possible that the edge between them
               intersects the box. The outside flags we computed are also
               used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(p0, p1, out0, out1)) return true;
            if (box.IntersectsLine(p1, p2, out1, out2)) return true;
            if (box.IntersectsLine(p2, p0, out2, out0)) return true;


            /* ---------------------------------------------------------------
               The only case left: The triangle contains the the whole box
               i.e. every point. When no triangle-line intersects the box and one
               box point is inside the triangle the triangle must contain the box
            --------------------------------------------------------------- */
            V2f a = box.Min - p0;
            V2f u = p1 - p0;
            V2f v = p2 - p0;


            float cross = u.X * v.Y - u.Y * v.X;
            if (Fun.IsTiny(cross)) return false;
            cross = 1 / cross;

            float t0 = (a.Y * v.X - a.X * v.Y) * cross; if (t0 < 0 || t0 > 1) return false;
            float t1 = (a.Y * u.X - a.X * u.Y) * cross; if (t1 < 0 || t1 > 1) return false;

            return (t0 + t1 < 1);
        }

        #endregion

        #region Box2f intersects Box2f (Box2f-Implementation)

        //Directly in Box-Implementation

        #endregion


        #region Quad2f intersects Line2f

        /// <summary>
        /// returns true if the Quad and the line intersect or the quad contains the line
        /// </summary>
        public static bool Intersects(
            this Quad2f quad,
            Line2f line
            )
        {
            if (quad.Contains(line.P0)) return true;
            if (quad.Contains(line.P1)) return true;

            if (line.IntersectsLine(quad.P0, quad.P1)) return true;
            if (line.IntersectsLine(quad.P1, quad.P2)) return true;
            if (line.IntersectsLine(quad.P2, quad.P3)) return true;
            if (line.IntersectsLine(quad.P3, quad.P0)) return true;

            return false;
        }

        /// <summary>
        /// returns true if the Quad and the line between p0 and p1 intersect or the quad contains the line
        /// </summary>
        public static bool IntersectsLine(
            this Quad2f quad,
            V2f p0, V2f p1
            )
        {
            if (quad.Contains(p0)) return true;
            if (quad.Contains(p1)) return true;

            Line2f line = new Line2f(p0, p1);
            if (line.IntersectsLine(quad.P0, quad.P1)) return true;
            if (line.IntersectsLine(quad.P1, quad.P2)) return true;
            if (line.IntersectsLine(quad.P2, quad.P3)) return true;
            if (line.IntersectsLine(quad.P3, quad.P0)) return true;

            return false;
        }

        #endregion

        #region Quad2f intersects Ray2f

        /// <summary>
        /// returns true if the quad and the ray intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad2f quad,
            Ray2f ray
            )
        {
            return ray.Plane2f.Divides(quad.Points);
        }

        #endregion

        #region Quad2f intersects Plane2f

        /// <summary>
        /// returns true if the Quad2f and the Plane2f intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad2f quad,
            Plane2f plane
            )
        {
            //UNTESTED
            if (plane.Divides(quad.Points)) return true;
            else return false;
        }

        #endregion

        #region Quad2f intersects Triangle2f

        /// <summary>
        /// returns true if the Quad2f and the Triangle2f intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Quad2f quad,
            Triangle2f triangle
            )
        {
            if (quad.Intersects(triangle.Line01)) return true;
            if (quad.Intersects(triangle.Line12)) return true;
            if (quad.Intersects(triangle.Line20)) return true;

            if (quad.Contains(triangle.P0)) return true;
            if (triangle.Contains(quad.P0)) return true;

            return false;
        }

        #endregion

        #region Quad2f intersects Box2f

        /// <summary>
        /// Returns true if the box and the Quad intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Box2f box,
            Quad2f quad
            )
        {
            Box.Flags out0 = box.OutsideFlags(quad.P0); if (out0 == 0) return true;
            Box.Flags out1 = box.OutsideFlags(quad.P1); if (out1 == 0) return true;
            Box.Flags out2 = box.OutsideFlags(quad.P2); if (out2 == 0) return true;
            Box.Flags out3 = box.OutsideFlags(quad.P3); if (out3 == 0) return true;


            /* ---------------------------------------------------------------
                If all of the points of the Quad are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2 & out3) != 0) return false;


            /* ---------------------------------------------------------------
               If two points of the Quad are not on the same side
               outside the box, it is possible that the edge between them
               intersects the box. The outside flags we computed are also
               used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(quad.P0, quad.P1, out0, out1)) return true;
            if (box.IntersectsLine(quad.P1, quad.P2, out1, out2)) return true;
            if (box.IntersectsLine(quad.P2, quad.P3, out2, out3)) return true;
            if (box.IntersectsLine(quad.P3, quad.P0, out3, out0)) return true;


            /* ---------------------------------------------------------------
               The only case left: The Quad contains the the whole box
               i.e. every point. When no triangle-line intersects the box and one
               box point is inside the Quad the Quad must contain the box
            --------------------------------------------------------------- */

            return quad.Contains(box.Min);
        }

        #endregion

        #region Quad2f intersects Quad2f

        /// <summary>
        /// returns true if the Quad2ds intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Quad2f q0,
            Quad2f quad
            )
        {
            if (q0.IntersectsLine(quad.P0, quad.P1)) return true;
            if (q0.IntersectsLine(quad.P1, quad.P2)) return true;
            if (q0.IntersectsLine(quad.P2, quad.P3)) return true;
            if (q0.IntersectsLine(quad.P3, quad.P0)) return true;

            if (q0.Contains(quad.P0)) return true;
            if (quad.Contains(q0.P0)) return true;

            return false;
        }

        #endregion


        #region Polygon2f intersects Line2f

        /// <summary>
        /// returns true if the Polygon2f and the Line2f intersect or the Polygon contains the Line
        /// </summary>
        public static bool Intersects(
            this Polygon2f poly,
            Line2f line
            )
        {
            foreach (var l in poly.EdgeLines)
            {
                if (l.Intersects(line)) return true;
            }

            if (poly.Contains(line.P0)) return true;

            return false;
        }

        #endregion

        #region Polygon2f intersects Ray2f

        /// <summary>
        /// returns true if the Polygon2f and the Ray2f intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Polygon2f poly,
            Ray2f ray
            )
        {
            //UNTESTED
            return ray.Plane2f.Divides(poly.Points);
        }


        #endregion

        #region Polygon2f intersects Plane2f

        /// <summary>
        /// returns true if the Polygon2f and the Plane2f itnersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Polygon2f poly,
            Plane2f plane
            )
        {
            //UNTESTED
            return plane.Divides(poly.Points);
        }

        #endregion

        #region Polygon2f intersects Triangle2f

        /// <summary>
        /// returns true if the Polygon2f and the Triangle2f intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2f poly,
            Triangle2f triangle
            )
        {
            foreach (var line in poly.EdgeLines)
            {
                if (triangle.Intersects(line)) return true;
            }

            if (triangle.Contains(poly[0])) return true;
            if (poly.Contains(triangle.P0)) return true;

            return false;
        }

        #endregion

        #region Polygon2f intersects Box2f

        /// <summary>
        /// returns true if the Polygon2f and the Box2f intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2f poly,
            Box2f box
            )
        {
            //UNTESTED
            int count = poly.PointCount;
            Box.Flags[] outFlags = new Box.Flags[count];

            int i0 = 0;
            foreach (var p in poly.Points) outFlags[i0++] = box.OutsideFlags(p);

            i0 = 0;
            int i1 = 1;
            foreach (var l in poly.EdgeLines)
            {
                if (box.Intersects(l, outFlags[i0], outFlags[i1])) return true;

                i0++;
                i1 = (i1 + 1) % count;
            }

            if (box.Contains(poly[0])) return true;
            if (poly.Contains(box.Min)) return true;

            return false;
        }

        #endregion

        #region Polygon2f intersects Quad2f

        /// <summary>
        /// returns true if the Polygon2f and the Quad2f interset or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2f poly,
            Quad2f quad
            )
        {
            foreach (var l in poly.EdgeLines)
            {
                if (quad.Intersects(l)) return true;
            }

            if (quad.Contains(poly[0])) return true;
            if (poly.Contains(quad.P0)) return true;

            return false;
        }

        #endregion

        #region Polygon2f intersects Polygon2f

        /// <summary>
        /// returns true if the Polygon2ds intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2f poly0,
            Polygon2f poly1
            )
        {
            //check if projected ranges intersect for all possible normals


            V2f[] allnormals = new V2f[poly0.PointCount + poly1.PointCount];
            int c = 0;

            foreach (var d in poly0.Edges)
            {
                allnormals[c] = new V2f(-d.Y, d.X);
                c++;
            }
            foreach (var d in poly1.Edges)
            {
                allnormals[c] = new V2f(-d.Y, d.X);
                c++;
            }



            foreach (var n in allnormals)
            {
                var r0 = poly0.ProjectTo(n);
                var r1 = poly1.ProjectTo(n);

                if (!r0.Intersects(r1)) return false;
            }

            return true;
        }

        private static Range1f ProjectTo(this Polygon2f poly, V2f dir)
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            foreach (var p in poly.Points)
            {
                float dotproduct = p.Dot(dir);

                if (dotproduct < min) min = dotproduct;
                if (dotproduct > max) max = dotproduct;
            }

            return new Range1f(min, max);
        }

        #endregion


        #region Line3f intersects Line3f (haaser)

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3f l0,
            Line3f l1
            )
        {
            return l0.Intersects(l1, Constant<float>.PositiveTinyValue);
        }

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than absoluteEpsilon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3f l0,
            Line3f l1,
            float absoluteEpsilon
            )
        {
            if (l0.GetMinimalDistanceTo(l1) < absoluteEpsilon) return true;
            else return false;
        }

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than absoluteEpsilon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3f l0,
            Line3f l1,
            float absoluteEpsilon,
            out V3f point
            )
        {
            if (l0.GetMinimalDistanceTo(l1, out point) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region Line3f intersects Special (inconsistent argument order)

        #region Line3f intersects Plane3f

        /// <summary>
        /// Returns true if the line and the plane intersect.
        /// </summary>
        public static bool Intersects(
             this Line3f line, Plane3f plane, out float t
             )
        {
            if (!line.Ray3f.Intersects(plane, out t)) return false;
            if (t >= 0 && t <= 1) return true;
            t = float.PositiveInfinity;
            return false;

        }

        /// <summary>
        /// Returns true if the line and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Line3f line, Plane3f plane, out float t, out V3f p
             )
        {
            bool result = line.Intersects(plane, out t);
            p = line.Origin + t * line.Direction;
            return result;
        }

        #endregion

        #region Line3f intersects Triangle3f

        /// <summary>
        /// Returns true if the line and the triangle intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3f line,
            Triangle3f triangle
            )
        {
            return line.Ray3f.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2, 0, 1, out _);
        }

        /// <summary>
        /// Returns true if the line and the triangle intersect.
        /// point holds the intersection point.
        /// </summary>
        public static bool Intersects(
            this Line3f line,
            Triangle3f triangle,
            out V3f point
            )
        {
            Ray3f ray = line.Ray3f;

            if (ray.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2, 0, 1, out float temp))
            {
                point = ray.GetPointOnRay(temp);
                return true;
            }
            else
            {
                point = V3f.NaN;
                return false;
            }
        }

        #endregion

        #endregion


        #region Ray3f intersects Line3f (haaser)

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than Constant&lt;float&gt;.PositiveTinyValue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3f ray, Line3f line
            )
        {
            return ray.Intersects(line, Constant<float>.PositiveTinyValue);
        }

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than absoluteEpsilon
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3f ray, Line3f line,
            float absoluteEpsilon
            )
        {
            if (ray.GetMinimalDistanceTo(line) < absoluteEpsilon) return true;
            else return false;
        }

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than absoluteEpsilon
        /// t holds the corresponding ray parameter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3f ray, Line3f line,
            float absoluteEpsilon,
            out float t
            )
        {
            if (ray.GetMinimalDistanceTo(line, out t) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region Ray3f intersects Ray3f (haaser)

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than Constant&lt;float&gt;.PositiveTinyValue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3f r0,
            Ray3f r1
            )
        {
            return r0.Intersects(r1, out _, out _, Constant<float>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than Constant&lt;float&gt;.PositiveTinyValue
        /// t0 and t1 hold the ray-parameters for the intersection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3f r0,
            Ray3f r1,
            out float t0,
            out float t1
            )
        {
            return r0.Intersects(r1, out t0, out t1, Constant<float>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than absoluteEpsilon
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3f r0,
            Ray3f r1,
            float absoluteEpsilon
            )
        {
            return r0.Intersects(r1, out _, out _, absoluteEpsilon);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than absoluteEpsilon
        /// t0 and t1 hold the ray-parameters for the intersection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3f r0,
            Ray3f r1,
            out float t0,
            out float t1,
            float absoluteEpsilon
            )
        {
            if (r0.GetMinimalDistanceTo(r1, out t0, out t1) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region Ray3f intersects Special (inconsistent argument order)

        #region Ray3f intersects Triangle3f

        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the Ray3f.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3f ray, Triangle3f triangle)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                float.MinValue, float.MaxValue);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3f ray, Triangle3f triangle, float tmin, float tmax)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                tmin, tmax);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3f ray, Triangle3f triangle, float tmin, float tmax, out float t)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTriangle(this Ray3f ray, V3f p0, V3f p1, V3f p2, float tmin, float tmax)
            => ray.IntersectsTriangle(p0, p1, p2, tmin, tmax, out float _);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        public static bool IntersectsTriangle(this Ray3f ray, V3f p0, V3f p1, V3f p2, float tmin, float tmax, out float t)
        {
            var edge01 = p1 - p0;
            var edge02 = p2 - p0;
            var plane = Vec.Cross(ray.Direction, edge02);
            var det = Vec.Dot(edge01, plane);
            if (det > -1E-7f && det < 1E-7f) { t = float.NaN; return false; }
            //ray ~= parallel / Triangle
            var tv = ray.Origin - p0;
            det = 1 / det;  // det is now inverse det
            var u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) { t = float.NaN; return false; }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            var v = Vec.Dot(ray.Direction, plane) * det;
            if (v < 0 || u + v > 1) { t = float.NaN; return false; }
            var temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < tmin || temp_t >= tmax) { t = float.NaN; return false; }

            t = temp_t;
            return true;
        }


        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTrianglePointAndEdges(this Ray3f ray, V3f p0, V3f edge01, V3f edge02, float tmin, float tmax)
            => ray.IntersectsTrianglePointAndEdges(p0, edge01, edge02, tmin, tmax, out _);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        public static bool IntersectsTrianglePointAndEdges(this Ray3f ray, V3f p0, V3f edge01, V3f edge02, float tmin, float tmax, out float t)
        {
            var plane = Vec.Cross(ray.Direction, edge02);
            var det = Vec.Dot(edge01, plane);
            if (det > -1E-7f && det < 1E-7f) { t = float.NaN; return false; }
            //ray ~= parallel / Triangle
            var tv = ray.Origin - p0;
            det = 1 / det;  // det is now inverse det
            var u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) { t = float.NaN; return false; }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            var v = Vec.Dot(ray.Direction, plane) * det;
            if (v < 0 || u + v > 1) { t = float.NaN; return false; }
            var temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < tmin || temp_t >= tmax) { t = float.NaN; return false; }

            t = temp_t;
            return true;
        }

        #endregion

        #region Ray3f intersects Quad3f


        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the Ray3f.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3f ray, Quad3f quad)
            => ray.Intersects(quad, float.MinValue, float.MaxValue);

        /// <summary>
        /// Returns true if the ray and the quad intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// </summary>
        public static bool Intersects(this Ray3f ray, Quad3f quad, float tmin, float tmax)
        {
            var edge02 = quad.P2 - quad.P0;
            return ray.IntersectsTrianglePointAndEdges(quad.P0, quad.Edge01, edge02, tmin, tmax)
                || ray.IntersectsTrianglePointAndEdges(quad.P0, edge02, quad.Edge03, tmin, tmax);
        }

        /// <summary>
        /// Returns true if the ray and the quad intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// </summary>
        public static bool IntersectsQuad(this Ray3f ray, V3f p0, V3f p1, V3f p2, V3f p3, float tmin, float tmax)
        {
            var edge02 = p2 - p0;
            return ray.IntersectsTrianglePointAndEdges(p0, p1 - p0, edge02, tmin, tmax)
                || ray.IntersectsTrianglePointAndEdges(p0, edge02, p3 - p0, tmin, tmax);
        }

        #endregion

        #region Ray3f intersects Polygon3f (haaser)

        /// <summary>
        /// Returns true if the ray and the polygon intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3f ray, Polygon3f poly, float tmin, float tmax)
            => ray.Intersects(poly, tmin, tmax, out _);

        /// <summary>
        /// Returns true if the ray and the polygon intersect within the
        /// supplied parameter interval of the ray.
        /// t holds the correspoinding paramter.
        /// </summary>
        public static bool Intersects(this Ray3f ray, Polygon3f poly, float tmin, float tmax, out float t)
        {
            var tris = poly.ComputeTriangulationOfConcavePolygon(1E-5f);
            var count = tris.Length;

            for (var i = 0; i < count; i += 3)
            {
                if (ray.IntersectsTriangle(poly[tris[i + 0]], poly[tris[i + 1]], poly[tris[i + 2]],
                                           tmin, tmax, out t))
                {
                    return true;
                }
            }

            t = float.NaN;
            return false;
        }

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices, intersect within the
        /// supplied parameter interval of the ray.
        /// (The Method triangulates the polygon)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsPolygon(this Ray3f ray, V3f[] vertices, float tmin, float tmax)
            => ray.Intersects(new Polygon3f(vertices), tmin, tmax);

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices and triangulation, intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        public static bool IntersectsPolygon(
            this Ray3f ray,
            V3f[] vertices,
            int[] triangulation,
            float tmin, float tmax
            )
        {
            for (var i = 0; i < triangulation.Length; i += 3)
            {
                if (ray.IntersectsTriangle(vertices[triangulation[i + 0]],
                                           vertices[triangulation[i + 1]],
                                           vertices[triangulation[i + 2]], tmin, tmax)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices and triangulation, intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        public static bool Intersects(
            this Ray3f ray,
            Polygon3f polygon,
            int[] triangulation,
            float tmin, float tmax
            )
        {
            for (var i = 0; i < triangulation.Length; i += 3)
            {
                if (ray.IntersectsTriangle(polygon[triangulation[i + 0]],
                                           polygon[triangulation[i + 1]],
                                           polygon[triangulation[i + 2]], tmin, tmax)) return true;
            }
            return false;
        }

        #endregion

        #region Ray3f intersects Sphere3f

        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the Ray3f.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3f ray, Sphere3f sphere)
            => ray.Intersects(sphere, float.MinValue, float.MaxValue);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3f.Hits method.
        /// </summary>
        public static bool Intersects(this Ray3f ray, Sphere3f sphere, float tmin, float tmax)
        {
            // calculate closest point
            var t = ray.Direction.Dot(sphere.Center - ray.Origin) / ray.Direction.LengthSquared;
            if (t < 0) t = 0;
            if (t < tmin) t = tmin;
            if (t > tmax) t = tmax;
            V3f p = ray.Origin + t * ray.Direction;

            // distance to sphere?
            var d = (p - sphere.Center).LengthSquared;
            if (d <= sphere.RadiusSquared)
                return true;

            return false;
        }

        #endregion

        #region Sphere3f intersects Triangle3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Sphere3f sphere, Triangle3f triangle
             )
        {
            V3f v = sphere.Center.GetClosestPointOn(triangle) - sphere.Center;
            return sphere.RadiusSquared >= v.LengthSquared;
        }

        #endregion

        #endregion


        #region Triangle3f intersects Line3f (haaser)

        /// <summary>
        /// Returns true if the triangle and the line intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3f tri,
            Line3f line
            )
        {
            return tri.IntersectsLine(line.P0, line.P1, out _);
        }

        /// <summary>
        /// Returns true if the triangle and the line intersect.
        /// point holds the intersection point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3f tri,
            Line3f line,
            out V3f point
            )
        {
            return tri.IntersectsLine(line.P0, line.P1, out point);
        }

        /// <summary>
        /// returns true if the triangle and the line, given by p0 and p1, intersect.
        /// </summary>
        public static bool IntersectsLine(
            this Triangle3f tri,
            V3f p0, V3f p1
            )
        {
            V3f edge01 = tri.Edge01;
            V3f edge02 = tri.Edge02;
            V3f dir = p1 - p0;

            V3f plane = Vec.Cross(dir, edge02);
            float det = Vec.Dot(edge01, plane);
            if (det > -1E-7f && det < 1E-7f)return false;
            //ray ~= parallel / Triangle
            V3f tv = p0 - tri.P0;
            det = 1 / det;  // det is now inverse det
            float u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1)
            {
                return false;
            }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            float v = Vec.Dot(dir, plane) * det;
            if (v < 0 || u + v > 1)
            {
                return false;
            }
            float temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < 0 || temp_t >= 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// returns true if the triangle and the line, given by p0 and p1, intersect.
        /// point holds the intersection point.
        /// </summary>
        public static bool IntersectsLine(
            this Triangle3f tri,
            V3f p0, V3f p1,
            out V3f point
            )
        {
            V3f edge01 = tri.Edge01;
            V3f edge02 = tri.Edge02;
            V3f dir = p1 - p0;

            V3f plane = Vec.Cross(dir, edge02);
            float det = Vec.Dot(edge01, plane);
            if (det > -1E-7f && det < 1E-7f) { point = V3f.NaN; return false; }
            //ray ~= parallel / Triangle
            V3f tv = p0 - tri.P0;
            det = 1 / det;  // det is now inverse det
            float u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1)
            {
                point = V3f.NaN;
                return false;
            }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            float v = Vec.Dot(dir, plane) * det;
            if (v < 0 || u + v > 1)
            {
                point = V3f.NaN;
                return false;
            }
            float temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < 0 || temp_t >= 1)
            {
                point = V3f.NaN;
                return false;
            }

            point = p0 + temp_t * dir;
            return true;
        }


        #endregion

        #region Triangle3f intersects Ray3f (haaser)

        /// <summary>
        /// Returns true if the triangle and the ray intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3f tri,
            Ray3f ray
            )
        {
            return tri.Intersects(ray, float.MinValue, float.MaxValue, out _);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect
        /// within the given parameter interval
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3f tri,
            Ray3f ray,
            float tmin, float tmax
            )
        {
            return tri.Intersects(ray, tmin, tmax, out _);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect.
        /// t holds the intersection paramter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3f tri,
            Ray3f ray,
            out float t
            )
        {
            return tri.Intersects(ray, float.MinValue, float.MaxValue, out t);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect
        /// within the given parameter interval
        /// t holds the intersection paramter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3f tri,
            Ray3f ray,
            float tmin, float tmax,
            out float t
            )
        {
            return ray.Intersects(tri, tmin, tmax, out t);
        }

        #endregion

        #region Triangle3f intersects Triangle3f (haaser)

        /// <summary>
        /// Returns true if the triangles intersect.
        /// </summary>
        public static bool Intersects(
            this Triangle3f t0,
            Triangle3f t1
            )
        {
            if (t0.IntersectsLine(t1.P0, t1.P1, out _)) return true;
            if (t0.IntersectsLine(t1.P1, t1.P2, out _)) return true;
            if (t0.IntersectsLine(t1.P2, t1.P0, out _)) return true;

            if (t1.IntersectsLine(t0.P0, t0.P1, out _)) return true;
            if (t1.IntersectsLine(t0.P1, t0.P2, out _)) return true;
            if (t1.IntersectsLine(t0.P2, t0.P0, out _)) return true;

            return false;
        }

        /// <summary>
        /// Returns true if the triangles intersect.
        /// line holds the cutting-line of the two triangles.
        /// </summary>
        public static bool Intersects(
            this Triangle3f t0,
            Triangle3f t1,
            out Line3f line
            )
        {
            List<V3f> points = new List<V3f>();

            if (t0.IntersectsLine(t1.P0, t1.P1, out V3f temp)) points.Add(temp);
            if (t0.IntersectsLine(t1.P1, t1.P2, out temp)) points.Add(temp);
            if (t0.IntersectsLine(t1.P2, t1.P0, out temp)) points.Add(temp);

            if (points.Count == 2)
            {
                line = new Line3f(points[0], points[1]);
                return true;
            }

            if (t1.IntersectsLine(t0.P0, t0.P1, out temp)) points.Add(temp);
            if (t1.IntersectsLine(t0.P1, t0.P2, out temp)) points.Add(temp);
            if (t1.IntersectsLine(t0.P2, t0.P0, out temp)) points.Add(temp);

            if (points.Count == 2)
            {
                line = new Line3f(points[0], points[1]);
                return true;
            }

            line = new Line3f(V3f.NaN, V3f.NaN);
            return false;
        }

        #endregion


        #region Quad3f intersects Line3f (haaser)

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3f quad,
            Line3f line)
        {
            return quad.IntersectsLine(line.P0, line.P1, out _);
        }

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// point holds the intersection point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3f quad,
            Line3f line,
            out V3f point)
        {
            return quad.IntersectsLine(line.P0, line.P1, out point);
        }


        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// </summary>
        public static bool IntersectsLine(
            this Quad3f quad,
            V3f p0, V3f p1)
        {
            Ray3f ray = new Ray3f(p0, p1 - p0);
            if (quad.Intersects(ray, 0, 1, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// point holds the intersection point.
        /// </summary>
        public static bool IntersectsLine(
            this Quad3f quad,
            V3f p0, V3f p1,
            out V3f point)
        {
            Ray3f ray = new Ray3f(p0, p1 - p0);
            if (quad.Intersects(ray, 0, 1, out float t))
            {
                point = ray.GetPointOnRay(t);
                return true;
            }
            else
            {
                point = V3f.NaN;
                return false;
            }
        }

        #endregion

        #region Quad3f intersects Ray3f (haaser)


        /// <summary>
        /// Returns true if the quad and the ray intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3f quad,
            Ray3f ray
            )
        {
            return quad.Intersects(ray, float.MinValue, float.MaxValue, out _);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect.
        /// t holds the intersection parameter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3f quad,
            Ray3f ray,
            out float t
            )
        {
            return quad.Intersects(ray, float.MinValue, float.MaxValue, out t);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect
        /// within the given paramter range
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3f quad,
            Ray3f ray,
            float tmin, float tmax
            )
        {
            return quad.Intersects(ray, tmin, tmax, out _);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect
        /// within the given paramter range
        /// t holds the intersection parameter.
        /// </summary>
        public static bool Intersects(
            this Quad3f quad,
            Ray3f ray,
            float tmin, float tmax,
            out float t
            )
        {
            V3f edge02 = quad.P2 - quad.P0;
            if (ray.IntersectsTrianglePointAndEdges(quad.P0, quad.Edge01, edge02, tmin, tmax, out t)) return true;
            if (ray.IntersectsTrianglePointAndEdges(quad.P0, edge02, quad.Edge03, tmin, tmax, out t)) return true;

            t = float.NaN;
            return false;
        }

        #endregion

        #region Quad3f intersects Triangle3f (haaser)

        /// <summary>
        /// Returns true if the quad and the triangle intersect.
        /// </summary>
        public static bool Intersects(
            this Quad3f quad,
            Triangle3f tri
            )
        {
            if (quad.IntersectsLine(tri.P0, tri.P1)) return true;
            if (quad.IntersectsLine(tri.P1, tri.P2)) return true;
            if (quad.IntersectsLine(tri.P2, tri.P0)) return true;

            if (tri.IntersectsLine(quad.P0, quad.P1)) return true;
            if (tri.IntersectsLine(quad.P1, quad.P2)) return true;
            if (tri.IntersectsLine(quad.P2, quad.P3)) return true;
            if (tri.IntersectsLine(quad.P3, quad.P0)) return true;

            return false;
        }


        /// <summary>
        /// Returns true if the quad and the triangle, given by p0/p1/p1, intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTriangle(
            this Quad3f quad,
            V3f p0, V3f p1, V3f p2
            )
        {
            Triangle3f tri = new Triangle3f(p0, p1, p2);
            return quad.Intersects(tri);
        }

        #endregion

        #region Quad3f intersects Quad3f (haaser)

        /// <summary>
        /// Returns true if the given quads intersect.
        /// </summary>
        public static bool Intersects(
            this Quad3f q0,
            Quad3f q1
            )
        {
            if (q0.IntersectsTriangle(q1.P0, q1.P1, q1.P2)) return true;
            if (q0.IntersectsTriangle(q1.P2, q1.P3, q1.P0)) return true;

            if (q1.IntersectsTriangle(q0.P0, q0.P1, q0.P2)) return true;
            if (q1.IntersectsTriangle(q0.P2, q0.P3, q0.P0)) return true;

            return false;
        }

        #endregion


        #region Plane3f intersects Line3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane3f plane, Line3f line)
        {
            return plane.IntersectsLine(line.P0, line.P1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane3f plane, Line3f line, float absoluteEpsilon)
        {
            return plane.IntersectsLine(line.P0, line.P1, absoluteEpsilon);
        }

        public static bool IntersectsLine(this Plane3f plane, V3f p0, V3f p1, float absoluteEpsilon)
        {
            float h0 = plane.Height(p0);
            int s0 = (h0 > absoluteEpsilon ? 1 :(h0 < -absoluteEpsilon ? -1 : 0));
            if (s0 == 0) return true;

            float h1 = plane.Height(p1);
            int s1 = (h1 > absoluteEpsilon ? 1 : (h1 < -absoluteEpsilon ? -1 : 0));
            if (s1 == 0) return true;


            if (s0 == s1) return false;
            else return true;
        }

        public static bool IntersectsLine(this Plane3f plane, V3f p0, V3f p1, float absoluteEpsilon, out V3f point)
        {
            //<n|origin + t0*dir> == d
            //<n|or> + t0*<n|dir> == d
            //t0 == (d - <n|or>) / <n|dir>;

            V3f dir = p1 - p0;
            float ld = dir.Length;
            dir /= ld;

            float nDotd = plane.Normal.Dot(dir);


            if (!Fun.IsTiny(nDotd))
            {
                float t0 = (plane.Distance - plane.Normal.Dot(p0)) / nDotd;

                if (t0 >= -absoluteEpsilon && t0 <= ld + absoluteEpsilon)
                {
                    point = p0 + dir * t0;
                    return true;
                }
                else
                {
                    point = V3f.NaN;
                    return false;
                }
            }
            else
            {
                point = V3f.NaN;
                return false;
            }

        }

        #endregion

        #region Plane3f intersects Ray3f

        /// <summary>
        /// Returns true if the Ray3f and the Plane3f intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3f ray, Plane3f plane)
            => !plane.Normal.IsOrthogonalTo(ray.Direction);

        /// <summary>
        /// Returns true if the ray and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Ray3f ray, Plane3f plane, out float t
             )
        {
            float dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot))
            {
                t = float.PositiveInfinity;
                return false;
            }
            t = -plane.Height(ray.Origin) / dot;
            return true;
        }

        /// <summary>
        /// Returns the intersection point with the given plane, or V3f.PositiveInfinity if ray is parallel to plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f Intersect(
             this Ray3f ray, Plane3f plane
             )
        {
            float dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot)) return V3f.PositiveInfinity;
            return ray.GetPointOnRay(-plane.Height(ray.Origin) / dot);
        }

        /// <summary>
        /// Returns true if the ray and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Ray3f ray, Plane3f plane, out float t, out V3f p
             )
        {
            bool result = Intersects(ray, plane, out t);
            p = ray.Origin + t * ray.Direction;
            return result;
        }

        #endregion

        #region Plane3f intersects Plane3f

        public static bool Intersects(this Plane3f p0, Plane3f p1)
        {
            bool parallel = p0.Normal.IsParallelTo(p1.Normal);

            if (parallel) return Fun.IsTiny(p0.Distance - p1.Distance);
            else return true;
        }

        public static bool Intersects(this Plane3f p0, Plane3f p1, out Ray3f ray)
        {
            V3f dir = p0.Normal.Cross(p1.Normal);
            float len = dir.Length;

            if (Fun.IsTiny(len))
            {
                if (Fun.IsTiny(p0.Distance - p1.Distance))
                {
                    ray = new Ray3f(p0.Normal * p0.Distance, V3f.Zero);
                    return true;
                }
                else
                {
                    ray = Ray3f.Invalid;
                    return false;
                }
            }

            dir *= 1 / len;

            var alu = new float[,]
                { { p0.Normal.X, p0.Normal.Y, p0.Normal.Z },
                  { p1.Normal.X, p1.Normal.Y, p1.Normal.Z },
                  { dir.X, dir.Y, dir.Z } };

            int[] p = alu.LuFactorize();

            var b = new float[] { p0.Distance, p1.Distance, 0 };

            var x = alu.LuSolve(p, b);

            ray = new Ray3f(new V3f(x), dir);
            return true;
        }

        #endregion

        #region Plane3f intersects Plane3f intersects Plane3f

        public static bool Intersects(this Plane3f p0, Plane3f p1, Plane3f p2, out V3f point)
        {
            var alu = new float[,]
                { { p0.Normal.X, p0.Normal.Y, p0.Normal.Z },
                  { p1.Normal.X, p1.Normal.Y, p1.Normal.Z },
                  { p2.Normal.X, p2.Normal.Y, p2.Normal.Z } };

            var p = new int[3];
            if (!alu.LuFactorize(p)) { point = V3f.NaN; return false; }
            var b = new float[] { p0.Distance, p1.Distance, p2.Distance };
            var x = alu.LuSolve(p, b);
            point = new V3f(x);
            return true;
        }

        #endregion

        #region Plane3f intersects Triangle3f

        /// <summary>
        /// Returns whether the given plane and triangle intersect.
        /// </summary>
        public static bool Intersects(
             this Plane3f plane, Triangle3f triangle
             )
        {
            int sign = plane.Sign(triangle.P0);
            if (sign == 0) return true;
            if (sign != plane.Sign(triangle.P1)) return true;
            if (sign != plane.Sign(triangle.P2)) return true;
            return false;
        }

        #endregion

        #region Plane3f intersects Sphere3f

        /// <summary>
        /// Returns whether the given sphere and plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Plane3f plane, Sphere3f sphere
             )
        {
            return sphere.Radius >= plane.Height(sphere.Center).Abs();
        }

        #endregion

        #region Plane3f intersects Polygon3f

        /// <summary>
        /// returns true if the Plane3f and the Polygon3f intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Plane3f plane,
            Polygon3f poly
            )
        {
            return plane.Intersects(poly, Constant<float>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the Plane3f and the polygon, intersect
        /// within a tolerance of absoluteEpsilon
        /// </summary>
        public static bool Intersects(
            this Plane3f plane,
            Polygon3f polygon,
            float absoluteEpsilon
            )
        {
            float height = plane.Height(polygon[0]);
            int sign0 = height > -absoluteEpsilon ? (height < absoluteEpsilon ? 0 : 1) : -1; if (sign0 == 0) return true;
            int pc = polygon.PointCount;
            for (int i = 1; i < pc; i++)
            {
                height = plane.Height(polygon[i]);
                int sign = height > -absoluteEpsilon ? (height < absoluteEpsilon ? 0 : 1) : -1;
                if (sign != sign0) return true;
            }

            return false;
        }


        /// <summary>
        /// returns true if the Plane3f and the Polygon3f intersect.
        /// line holds the intersection line
        /// ATTENTION: works only with non-concave polygons!
        /// </summary>
        public static bool IntersectsConvex(
            this Plane3f plane,
            Polygon3f poly,
            out Line3f line
            )
        {
            return plane.IntersectsConvex(poly, Constant<float>.PositiveTinyValue, out line);
        }

        /// <summary>
        /// Returns true if the Plane3f and the polygon, given by points, intersect
        /// within a tolerance of absoluteEpsilon.
        /// Line holds the intersection line.
        /// ATTENTION: works only with non-concave polygons!
        /// </summary>
        public static bool IntersectsConvex(
            this Plane3f plane,
            Polygon3f polygon,
            float absoluteEpsilon,
            out Line3f line
            )
        {
            int count = polygon.PointCount;
            int[] signs = new int[count];
            int pc = 0, nc = 0, zc = 0;
            for (int pi = 0; pi < count; pi++)
            {
                float h = plane.Height(polygon[pi]);
                if (h < -absoluteEpsilon) { nc++; signs[pi] = -1; continue; }
                if (h > absoluteEpsilon) { pc++; signs[pi] = 1; continue;  }
                zc++; signs[pi] = 0;
            }

            if (zc == count)
            {
                line = new Line3f(polygon[0], polygon[0]);
                return false;
            }
            else if (pc == 0 && zc == 0)
            {
                line = new Line3f(V3f.NaN, V3f.NaN);
                return false;
            }
            else if (nc == 0 && zc == 0)
            {
                line = new Line3f(V3f.NaN, V3f.NaN);
                return false;
            }
            else
            {
                int pointcount = 0;
                V3f[] linePoints = new V3f[2];
                for (int i = 0; i < count; i++)
                {
                    int u = (i + 1) % count;

                    if (signs[i] != signs[u] || signs[i] == 0 || signs[u] == 0)
                    {
                        if (plane.IntersectsLine(polygon[i], polygon[u], absoluteEpsilon, out V3f point))
                        {
                            linePoints[pointcount++] = point;
                            //If Endpoint is on Plane => Next startpoint is on plane => same intersection point
                            // => skip all following lines which start within absoluteEpsilon (whic have a zero sign)
                            while (signs[(i + 1) % count] == 0) i++;
                        }
                    }
                    if (pointcount == 2)
                    {
                        //
                        line = new Line3f(linePoints[0], linePoints[1]);
                        return true;
                    }
                }
                line = new Line3f(V3f.NaN, V3f.NaN);
                return false;
            }
        }

        #endregion

        #region Plane3f intersects Cylinder3f

        /// <summary>
        /// Returns whether the given sphere and cylinder intersect.
        /// </summary>
        public static bool Intersects(this Plane3f plane, Cylinder3f cylinder)
        {
            if (plane.IsParallelToAxis(cylinder))
            {
                var distance = cylinder.P0.GetMinimalDistanceTo(plane);
                return distance < cylinder.Radius;
            }
            return true;
        }

        /// <summary>
        /// Tests if the given plane is parallel to the cylinder axis (i.e. the plane's normal is orthogonal to the axis).
        /// The plane will intersect the cylinder in two rays or in one tangent line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelToAxis(this Plane3f plane, Cylinder3f cylinder)
            => plane.Normal.IsOrthogonalTo(cylinder.Axis.Direction.Normalized);

        /// <summary>
        /// Tests if the given plane is orthogonal to the cylinder axis (i.e. the plane's normal is parallel to the axis).
        /// The plane will intersect the cylinder in a circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOrthogonalToAxis(this Plane3f plane, Cylinder3f cylinder)
            => plane.Normal.IsParallelTo(cylinder.Axis.Direction.Normalized);

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in an ellipse.
        /// This is only true if the plane is neither orthogonal nor parallel to the cylinder axis. Otherwise the intersection methods returning a circle or rays have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="ellipse"></param>
        public static bool Intersects(this Plane3f plane, Cylinder3f cylinder, out Ellipse3f ellipse)
        {
            if (plane.IsParallelToAxis(cylinder) || plane.IsOrthogonalToAxis(cylinder))
            {
                ellipse = Ellipse3f.Zero;
                return false;
            }

            var dir = cylinder.Axis.Direction.Normalized;
            cylinder.Axis.Ray3f.Intersects(plane, out _, out V3f center);
            var cosTheta = dir.Dot(plane.Normal);

            var eNormal = plane.Normal;
            var eCenter = center;
            var eMajor = (dir - cosTheta * eNormal).Normalized;
            var eMinor = (eNormal.Cross(eMajor)).Normalized;
            eMajor = eNormal.Cross(eMinor).Normalized; //to be sure - if ellipse is nearly a circle
            eMajor = eMajor * cylinder.Radius / cosTheta.Abs();
            eMinor *= cylinder.Radius;
            ellipse = new Ellipse3f(eCenter, eNormal, eMajor, eMinor);
            return true;
        }

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in a circle.
        /// This is only true if the plane is orthogonal to the cylinder axis. Otherwise the intersection methods returning an ellipse or rays have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="circle"></param>
        public static bool Intersects(this Plane3f plane, Cylinder3f cylinder, out Circle3f circle)
        {
            if (plane.IsOrthogonalToAxis(cylinder))
            {
                circle = cylinder.GetCircle(cylinder.GetHeight(plane.Point));
                return true;
            }

            circle = Circle3f.Zero;
            return false;
        }

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in one or two rays.
        /// This is only true if the plane is parallel to the cylinder axis. Otherwise the intersection methods returning an ellipse or a circle have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="rays">Output of intersection rays. The array contains two rays (intersection), one ray (plane is tangent to cylinder) or no ray (no intersection).</param>
        public static bool Intersects(this Plane3f plane, Cylinder3f cylinder, out Ray3f[] rays)
        {
            if (plane.IsParallelToAxis(cylinder))
            {
                var distance = cylinder.P0.GetMinimalDistanceTo(plane);
                var center = cylinder.P0 - distance * plane.Normal;
                var axis = cylinder.Axis.Direction.Normalized;

                if (distance == cylinder.Radius) //one tangent line
                {
                    rays = new[] { new Ray3f(center, axis) };
                    return true;
                }
                else //two intersection lines
                {
                    var offset = axis.Cross(plane.Normal);
                    var extent = (cylinder.Radius.Square() - distance.Square()).Sqrt();
                    rays = new[]
                    {
                        new Ray3f(center - extent * offset, axis),
                        new Ray3f(center + extent * offset, axis)
                    };
                    return true;
                }
            }
            rays = new Ray3f[0];
            return false;
        }

        #endregion


        #region Sphere3f intersects Sphere3f (sm)

        /// <summary>
        /// Returns true if the spheres intersect, or one contains the other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Sphere3f s0, Sphere3f s1)
            => (s0.Center - s1.Center).LengthSquared <= (s0.Radius + s1.Radius).Square();

        #endregion


        #region Box3f intersects Line3f

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool Intersects(this Box3f box, Line3f line)
        {
            var out0 = box.OutsideFlags(line.P0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(line.P1); if (out1 == 0) return true;
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool IntersectsLine(
                this Box3f box, V3f p0, V3f p1)
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            return box.IntersectsLine(p0, p1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied, and have to be already individually tested against
        /// intersection with the box.
        /// </summary>
        public static bool IntersectsLine(
                this Box3f box, V3f p0, V3f p1, Box.Flags out0, Box.Flags out1)
        {
            if ((out0 & out1) != 0) return false;

            V3f min = box.Min;
            V3f max = box.Max;
            var bf = out0 | out1;

            if ((bf & Box.Flags.X) != 0)
            {
                float dx = p1.X - p0.X;
                if ((bf & Box.Flags.MinX) != 0)
                {
                    if (dx == 0 && p0.X < min.X) return false;
                    float t = (min.X - p0.X) / dx;
                    V3f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinX) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxX) != 0)
                {
                    if (dx == 0 && p0.X > max.X) return false;
                    float t = (max.X - p0.X) / dx;
                    V3f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxX) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Y) != 0)
            {
                float dy = p1.Y - p0.Y;
                if ((bf & Box.Flags.MinY) != 0)
                {
                    if (dy == 0 && p0.Y < min.Y) return false;
                    float t = (min.Y - p0.Y) / dy;
                    V3f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinY) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxY) != 0)
                {
                    if (dy == 0 && p0.Y > max.Y) return false;
                    float t = (max.Y - p0.Y) / dy;
                    V3f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxY) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Z) != 0)
            {
                float dz = p1.Z - p0.Z;
                if ((bf & Box.Flags.MinZ) != 0)
                {
                    if (dz == 0 && p0.Z < min.Z) return false;
                    float t = (min.Z - p0.Z) / dz;
                    V3f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinZ) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxZ) != 0)
                {
                    if (dz == 0 && p0.Z > max.Z) return false;
                    float t = (max.Z - p0.Z) / dz;
                    V3f p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxZ) == 0)
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region Box3f intersects Ray3f (haaser)

        public static bool Intersects(this Box3f box, Ray3f ray, out float t)
        {
            Box.Flags out0 = box.OutsideFlags(ray.Origin);

            if (out0 == 0)
            {
                t = 0;
                return true;
            }

            Box3f largeBox = box.EnlargedByRelativeEps(1E-5f);
            float tmin = float.PositiveInfinity;
            float ttemp;
            if ((out0 & Box.Flags.X) != 0)
            {
                if (!Fun.IsTiny(ray.Direction.X))
                {
                    if ((out0 & Box.Flags.MinX) != 0)
                    {
                        ttemp = (box.Min.X - ray.Origin.X) / ray.Direction.X;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                    else if ((out0 & Box.Flags.MaxX) != 0)
                    {
                        ttemp = (box.Max.X - ray.Origin.X) / ray.Direction.X;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                }
            }


            if ((out0 & Box.Flags.Y) != 0)
            {
                if (!Fun.IsTiny(ray.Direction.Y))
                {
                    if ((out0 & Box.Flags.MinY) != 0)
                    {
                        ttemp = (box.Min.Y - ray.Origin.Y) / ray.Direction.Y;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                    else if ((out0 & Box.Flags.MaxY) != 0)
                    {
                        ttemp = (box.Max.Y - ray.Origin.Y) / ray.Direction.Y;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                }
            }


            if ((out0 & Box.Flags.Z) != 0)
            {
                if (!Fun.IsTiny(ray.Direction.Z))
                {
                    if ((out0 & Box.Flags.MinZ) != 0)
                    {
                        ttemp = (box.Min.Z - ray.Origin.Z) / ray.Direction.Z;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                    else if ((out0 & Box.Flags.MaxZ) != 0)
                    {
                        ttemp = (box.Max.Z - ray.Origin.Z) / ray.Direction.Z;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                }
            }


            if (tmin < float.PositiveInfinity)
            {
                t = tmin;
                return true;
            }

            t = float.NaN;
            return false;
        }


        #endregion

        #region Box3f intersects Plane3f

        /// <summary>
        /// Returns true if the box and the plane intersect or touch with a
        /// supplied epsilon tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box3f box, Plane3f plane, float eps)
        {
            var signs = box.GetIntersectionSignsWithPlane(plane, eps);
            return signs != Signs.Negative && signs != Signs.Positive;
        }

        /// <summary>
        /// Classify the position of all the eight vertices of a box with
        /// respect to a supplied plane.
        /// </summary>
        public static Signs GetIntersectionSignsWithPlane(
            this Box3f box, Plane3f plane, float eps)
        {
            var normal = plane.Normal;
            var distance = plane.Distance;

            float npMinX = normal.X * box.Min.X;
            float npMaxX = normal.X * box.Max.X;
            float npMinY = normal.Y * box.Min.Y;
            float npMaxY = normal.Y * box.Max.Y;
            float npMinZ = normal.Z * box.Min.Z;
            float npMaxZ = normal.Z * box.Max.Z;

            float hMinZ = npMinZ - distance;
            float hMaxZ = npMaxZ - distance;

            float hMinYMinZ = npMinY + hMinZ;
            float hMaxYMinZ = npMaxY + hMinZ;
            float hMinYMaxZ = npMinY + hMaxZ;
            float hMaxYMaxZ = npMaxY + hMaxZ;

            return (npMinX + hMinYMinZ).GetSigns(eps)
                 | (npMaxX + hMinYMinZ).GetSigns(eps)
                 | (npMinX + hMaxYMinZ).GetSigns(eps)
                 | (npMaxX + hMaxYMinZ).GetSigns(eps)
                 | (npMinX + hMinYMaxZ).GetSigns(eps)
                 | (npMaxX + hMinYMaxZ).GetSigns(eps)
                 | (npMinX + hMaxYMaxZ).GetSigns(eps)
                 | (npMaxX + hMaxYMaxZ).GetSigns(eps);
        }

        /// <summary>
        /// Returns true if the box intersects the supplied plane. The
        /// bounding boxes of the resulting parts are returned in the out
        /// parameters.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Signs GetIntersectionSigns(
                this Box3f box, Plane3f plane, float eps,
                out Box3f negBox, out Box3f zeroBox, out Box3f posBox)
        {
            return box.GetIntersectionSigns(plane.Normal, plane.Distance, eps,
                                       out negBox, out zeroBox, out posBox);
        }

        /// <summary>
        /// Returns true if the box intersects the supplied plane. The
        /// bounding boxes of the resulting parts are returned in the out
        /// parameters.
        /// </summary>
        public static Signs GetIntersectionSigns(
                this Box3f box, V3f normal, float distance, float eps,
                out Box3f negBox, out Box3f zeroBox, out Box3f posBox)
        {
            float npMinX = normal.X * box.Min.X;
            float npMaxX = normal.X * box.Max.X;
            float npMinY = normal.Y * box.Min.Y;
            float npMaxY = normal.Y * box.Max.Y;
            float npMinZ = normal.Z * box.Min.Z;
            float npMaxZ = normal.Z * box.Max.Z;

            var ha = new float[8];

            float hMinZ = npMinZ - distance;
            float hMaxZ = npMaxZ - distance;

            float hMinYMinZ = npMinY + hMinZ;
            ha[0] = npMinX + hMinYMinZ;
            ha[1] = npMaxX + hMinYMinZ;

            float hMaxYMinZ = npMaxY + hMinZ;
            ha[2] = npMinX + hMaxYMinZ;
            ha[3] = npMaxX + hMaxYMinZ;

            float hMinYMaxZ = npMinY + hMaxZ;
            ha[4] = npMinX + hMinYMaxZ;
            ha[5] = npMaxX + hMinYMaxZ;

            float hMaxYMaxZ = npMaxY + hMaxZ;
            ha[6] = npMinX + hMaxYMaxZ;
            ha[7] = npMaxX + hMaxYMaxZ;

            Signs all = Signs.None;
            var sa = new Signs[8];
            for (int i = 0; i < 8; i++) { sa[i] = ha[i].GetSigns(eps); all |= sa[i]; }

            negBox = Box3f.Invalid;
            zeroBox = Box3f.Invalid;
            posBox = Box3f.Invalid;

            if (all == Signs.Zero) { zeroBox = box; return all; }
            if (all == Signs.Positive) { posBox = box; return all; }
            if (all == Signs.Negative) { negBox = box; return all; }

            var pa = box.ComputeCorners();

            for (int i = 0; i < 8; i++)
            {
                if (sa[i] == Signs.Negative)
                    negBox.ExtendBy(pa[i]);
                else if (sa[i] == Signs.Positive)
                    posBox.ExtendBy(pa[i]);
                else
                {
                    negBox.ExtendBy(pa[i]);
                    zeroBox.ExtendBy(pa[i]);
                    posBox.ExtendBy(pa[i]);
                }
            }

            if (all == Signs.NonPositive) { posBox = Box3f.Invalid; return all; }
            if (all == Signs.NonNegative) { negBox = Box3f.Invalid; return all; }

            for (int ei = 0; ei < 12; ei++)
            {
                int i0 = c_cubeEdgeVertex0[ei], i1 = c_cubeEdgeVertex1[ei];

                if ((sa[i0] == Signs.Negative && sa[i1] == Signs.Positive)
                    || (sa[i0] == Signs.Positive && sa[i1] == Signs.Negative))
                {
                    float h0 = ha[i0];
                    float t = h0 / (h0 - ha[i1]);
                    V3f p0 = pa[i0];
                    V3f sp = p0 + t * (pa[i1] - p0);
                    negBox.ExtendBy(sp);
                    zeroBox.ExtendBy(sp);
                    posBox.ExtendBy(sp);
                }
            }

            return all;
        }

        #endregion

        #region Box3f intersects Sphere3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Box3f box, Sphere3f sphere
             )
        {
            V3f v = sphere.Center.GetClosestPointOn(box) - sphere.Center;
            return sphere.RadiusSquared >= v.LengthSquared;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Box3f box, Cylinder3f cylinder
             )
        {

            return box.Intersects(cylinder.BoundingBox3f);

            //throw new NotImplementedException();
        }

        #endregion

        #region Box3f intersects Triangle3f

        /// <summary>
        /// Returns true if the box and the triangle intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Box3f box, Triangle3f triangle
             )
        {
            return box.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2);
        }

        /// <summary>
        /// Returns true if the box and the triangle intersect.
        /// </summary>
        public static bool IntersectsTriangle(
             this Box3f box, V3f p0, V3f p1, V3f p2
             )
        {
            /* ---------------------------------------------------------------
               If one of the points of the triangle is inside the box, it
               intersects, of course.
            --------------------------------------------------------------- */
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            var out2 = box.OutsideFlags(p2); if (out2 == 0) return true;
            return box.IntersectsTriangle(p0, p1, p2, out0, out1, out2);
        }


        /// <summary>
        /// Returns true if the box and the triangle intersect. The outside
        /// flags of the triangle vertices with respect to the box have to be
        /// supplied, and already be individually tested for intersection with
        /// the box.
        /// </summary>
        public static bool IntersectsTriangle(
             this Box3f box, V3f p0, V3f p1, V3f p2,
             Box.Flags out0, Box.Flags out1, Box.Flags out2
             )
        {
            /* ---------------------------------------------------------------
                If all of the points of the triangle are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2) != 0) return false;

            /* ---------------------------------------------------------------
                If two points of the triangle are not on the same side
                outside the box, it is possible that the edge between them
                intersects the box. The outside flags we computed are also
                used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(p0, p1, out0, out1)) return true;
            if (box.IntersectsLine(p1, p2, out1, out2)) return true;
            if (box.IntersectsLine(p2, p0, out2, out0)) return true;

            /* ---------------------------------------------------------------
                The only case left: the edges of the triangle go outside
                around the box. Intersect the four space diagonals of the box
                with the triangle to test for intersection.
            --------------------------------------------------------------- */
            Ray3f ray = new Ray3f(box.Min, box.Size);
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;

            ray.Origin.X = box.Max.X;
            ray.Direction.X = -ray.Direction.X;
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;
            ray.Direction.X = -ray.Direction.X;
            ray.Origin.X = box.Min.X;

            ray.Origin.Y = box.Max.Y;
            ray.Direction.Y = -ray.Direction.Y;
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;
            ray.Direction.Y = -ray.Direction.Y;
            ray.Origin.Y = box.Min.Y;

            ray.Origin.Z = box.Max.Z;
            ray.Direction.Z = -ray.Direction.Z;
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;

            return false;
        }

        #endregion

        #region Box3f intersects Quad3f (haaser)

        public static bool Intersects(
            this Box3f box, Quad3f quad
            )
        {
            Box.Flags out0 = box.OutsideFlags(quad.P0); if (out0 == 0) return true;
            Box.Flags out1 = box.OutsideFlags(quad.P1); if (out1 == 0) return true;
            Box.Flags out2 = box.OutsideFlags(quad.P2); if (out2 == 0) return true;
            Box.Flags out3 = box.OutsideFlags(quad.P3); if (out3 == 0) return true;

            return box.IntersectsQuad(quad.P0, quad.P1, quad.P2, quad.P3, out0, out1, out2, out3);
        }

        public static bool IntersectsQuad(
             this Box3f box, V3f p0, V3f p1, V3f p2, V3f p3
             )
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            var out2 = box.OutsideFlags(p2); if (out2 == 0) return true;
            var out3 = box.OutsideFlags(p3); if (out3 == 0) return true;

            return box.IntersectsQuad(p0, p1, p2, p3, out0, out1, out2, out3);
        }

        public static bool IntersectsQuad(
            this Box3f box, V3f p0, V3f p1, V3f p2, V3f p3,
            Box.Flags out0, Box.Flags out1, Box.Flags out2, Box.Flags out3
            )
        {
            /* ---------------------------------------------------------------
                If all of the points of the quad are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2 & out3) != 0) return false;


            /* ---------------------------------------------------------------
                If two points of the quad are not on the same side
                outside the box, it is possible that the edge between them
                intersects the box. The outside flags we computed are also
                used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(p0, p1, out0, out1)) return true;
            if (box.IntersectsLine(p1, p2, out1, out2)) return true;
            if (box.IntersectsLine(p2, p3, out2, out3)) return true;
            if (box.IntersectsLine(p3, p0, out3, out0)) return true;


            /* ---------------------------------------------------------------
                The only case left: the edges of the quad go outside
                around the box. Intersect the four space diagonals of the box
                with the triangle to test for intersection.
            --------------------------------------------------------------- */
            Ray3f ray = new Ray3f(box.Min, box.Size);
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;

            ray.Origin.X = box.Max.X;
            ray.Direction.X = -ray.Direction.X;
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;
            ray.Direction.X = -ray.Direction.X;
            ray.Origin.X = box.Min.X;

            ray.Origin.Y = box.Max.Y;
            ray.Direction.Y = -ray.Direction.Y;
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;
            ray.Direction.Y = -ray.Direction.Y;
            ray.Origin.Y = box.Min.Y;

            ray.Origin.Z = box.Max.Z;
            ray.Direction.Z = -ray.Direction.Z;
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;

            return false;
        }

        #endregion

        #region Box3f intersects Polygon3f (haaser)

        public static bool Intersects(this Box3f box, Polygon3f poly)
        {
            int edges = poly.PointCount;
            Box.Flags[] outside = new Box.Flags[edges];
            for (int i = 0; i < edges; i++)
            {
                outside[i] = box.OutsideFlags(poly[i]); if (outside[i] == 0) return true;
            }

            /* ---------------------------------------------------------------
                If all of the points of the polygon are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            Box.Flags sum = outside[0];
            for (int i = 1; i < edges; i++) sum &= outside[i];
            if (sum != 0) return false;

            /* ---------------------------------------------------------------
                If two points of the polygon are not on the same side
                outside the box, it is possible that the edge between them
                intersects the box. The outside flags we computed are also
                used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            int u;
            for (int i = 0; i < edges; i++)
            {
                u = (i + 1) % edges;
                if (box.IntersectsLine(poly[i], poly[u], outside[i], outside[u])) return true;
            }

            /* ---------------------------------------------------------------
                The only case left: the edges of the polygon go outside
                around the box. Intersect the four space diagonals of the box
                with the triangle to test for intersection.
                The polygon needs to be triangulated for this check
            --------------------------------------------------------------- */

            int[] tris = poly.ComputeTriangulationOfConcavePolygon(1E-5f);

            Ray3f ray = new Ray3f(box.Min, box.Size);
            if (ray.Intersects(poly, tris, 0, 1)) return true;

            ray.Origin.X = box.Max.X;
            ray.Direction.X = -ray.Direction.X;
            if (ray.Intersects(poly, tris, 0, 1)) return true;
            ray.Direction.X = -ray.Direction.X;
            ray.Origin.X = box.Min.X;

            ray.Origin.Y = box.Max.Y;
            ray.Direction.Y = -ray.Direction.Y;
            if (ray.Intersects(poly, tris, 0, 1)) return true;
            ray.Direction.Y = -ray.Direction.Y;
            ray.Origin.Y = box.Min.Y;

            ray.Origin.Z = box.Max.Z;
            ray.Direction.Z = -ray.Direction.Z;
            if (ray.Intersects(poly, tris, 0, 1)) return true;

            return false;
        }

        #endregion

        #region Box3f intersects Projection-Trafo (haaser)

        /// <summary>
        /// returns true if the Box3f and the frustum described by the M44f intersect or the frustum contains the Box3f
        /// Assumes DirectX clip-space:
        ///     -w &lt; x &lt; w
        ///     -w &lt; y &lt; w
        ///      0 &lt; z &lt; w
        /// </summary>
        public static bool IntersectsFrustum(this Box3f box, M44f projection)
        {
            //Let's look at the left clip-plane
            //which corresponds to:
            //-w < x
            //  # which can easily be transformed to:
            //0 < x + w
            //  # for a given vector v this means (* is a dot product here)
            //0 < proj.R0 * v + proj.R3 * v
            //  # or in other words:
            //0 < (proj.R0 + proj.R3) * v

            //therefore (proj.R0 + proj.R3) is the plane describing the left clip-plane.
            //The other planes can be derived in a similar way (only the near plane is a little different)
            //see http://fgiesen.wordpress.com/2012/08/31/frustum-planes-from-the-projection-matrix/ for a full explanation

            var r0 = projection.R0;
            var r1 = projection.R1;
            var r2 = projection.R2;
            var r3 = projection.R3;


            V4f plane;
            V3f n;

            //left
            plane = r3 + r0;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out V3f min, out V3f max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //right
            plane = r3 - r0;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //top
            plane = r3 + r1;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //bottom
            plane = r3 - r1;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //near
            plane = r2;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //far
            plane = r3 - r2;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;


            return true;
        }



        #endregion


        #region Hull3f intersects Line3f

        /// <summary>
        /// returns true if the Hull3f and the Line3f intersect or the Hull3f contains the Line3f
        /// [Hull3f-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Hull3f hull,
            Line3f line
            )
        {
            if (hull.Contains(line.P0)) return true;
            if (hull.Contains(line.P1)) return true;

            return hull.Intersects(line.Ray3f, 0, 1, out _);
        }

        /// <summary>
        /// returns true if the Hull3f and the Line between p0 and p1 intersect or the Hull3f contains the Line
        /// [Hull3f-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this Hull3f hull,
            V3f p0, V3f p1
            )
        {
            return hull.Intersects(new Line3f(p0, p1));
        }

        #endregion

        #region Hull3f intersects Ray3f

        /// <summary>
        /// returns true if the Hull3f and the Ray3f intersect
        /// [Hull3f-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Hull3f hull,
            Ray3f ray
            )
        {
            return hull.Intersects(ray, float.NegativeInfinity, float.PositiveInfinity, out _);
        }

        /// <summary>
        /// returns true if the Hull3f and the Ray3f intersect
        /// the out parameter t holds the ray-parameter where an intersection was found
        /// [Hull3f-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Hull3f hull,
            Ray3f ray,
            out float t
            )
        {
            return hull.Intersects(ray, float.NegativeInfinity, float.PositiveInfinity, out t);
        }

        /// <summary>
        /// returns true if the Hull3f and the Ray3f intersect and the
        /// ray-parameter for the intersection is between t_min and t_max
        /// the out parameter t holds the ray-parameter where an intersection was found
        /// [Hull3f-Normals must point outside]
        /// </summary>
        public static bool Intersects(
            this Hull3f hull,
            Ray3f ray,
            float t_min, float t_max,
            out float t
            )
        {
            if (!float.IsInfinity(t_min) && hull.Contains(ray.GetPointOnRay(t_min))) { t = t_min; return true; }
            if (!float.IsInfinity(t_max) && hull.Contains(ray.GetPointOnRay(t_max))) { t = t_max; return true; }

            var planes = hull.PlaneArray;
            for (int i = 0; i < planes.Length; i++)
            {
                if (!Fun.IsTiny(planes[i].Normal.Dot(ray.Direction)) &&
                    ray.Intersects(planes[i], out float temp_t) &&
                    temp_t >= t_min && temp_t <= t_max)
                {
                    V3f candidatePoint = ray.GetPointOnRay(temp_t);
                    bool contained = true;

                    for (int u = 0; u < planes.Length; u++)
                    {
                        if (u != i && planes[u].Height(candidatePoint) > Constant<float>.PositiveTinyValue)
                        {
                            contained = false;
                            break;
                        }
                    }

                    if (contained)
                    {
                        t = temp_t;
                        return true;
                    }
                }
            }

            t = float.NaN;
            return false;
        }

        #endregion

        #region Hull3f intersects Plane3f

        /// <summary>
        /// returns true if the Hull3f and the Plane3f intersect
        /// [Hull3f-Normals must point outside]
        /// </summary>
        public static bool Intersects(
            this Hull3f hull,
            Plane3f plane
            )
        {
            foreach (var p in hull.PlaneArray)
            {
                if (!p.Normal.IsParallelTo(plane.Normal) && p.Intersects(plane, out Ray3f ray))
                {
                    if (hull.Intersects(ray)) return true;
                }
            }

            return false;
        }

        #endregion

        #region Hull3f intersects Box3f

        /// <summary>
        /// Returns true if the hull and the box intersect.
        /// </summary>
        public static bool Intersects(
            this Hull3f hull, Box3f box
            )
        {
            if (box.IsInvalid) return false;
            bool intersecting = false;
            foreach (Plane3f p in hull.PlaneArray)
            {
                box.GetMinMaxInDirection(p.Normal, out V3f min, out V3f max);
                if (p.Height(min) > 0) return false; // outside
                if (p.Height(max) >= 0) intersecting = true;
            }
            if (intersecting) return true; // intersecting
            return true; // inside
        }

        /// Test hull against intersection of the supplied bounding box.
        /// Note that this is a conservative test, since in some cases
        /// around the edges of the hull it may return true although the
        /// hull does not intersect the box.
        public static bool Intersects(
                this FastHull3f fastHull,
                Box3f box)
        {
            var planes = fastHull.Hull.PlaneArray;
            int count = planes.Length;
            bool intersecting = false;
            for (int pi = 0; pi < count; pi++)
            {
                int minCornerIndex = fastHull.MinCornerIndexArray[pi];
                if (planes[pi].Height(box.Corner(minCornerIndex)) > 0)
                    return false;
                if (planes[pi].Height(box.Corner(minCornerIndex ^ 7)) >= 0)
                    intersecting = true;
            }
            if (intersecting) return true;
            return true;
        }

        #endregion

        #region Hull3f intersects Sphere3f

        /// <summary>
        /// Returns true if the hull and the sphere intersect.
        /// </summary>
        public static bool Intersects(
            this Hull3f hull, Sphere3f sphere
            )
        {
            if (sphere.IsInvalid) return false;
            bool intersecting = false;
            foreach (Plane3f p in hull.PlaneArray)
            {
                float height = p.Height(sphere.Center);
                if (height > sphere.Radius) return false; // outside
                if (height.Abs() < sphere.Radius) intersecting = true;
            }
            if (intersecting) return true; // intersecting
            return true; // inside
        }

        #endregion


        #region Plane3f intersects Box3f

        /// <summary>
        /// Returns true if the box and the plane intersect or touch with a
        /// supplied epsilon tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane3f plane, float eps, Box3f box)
            => box.Intersects(plane, eps);

        #endregion

        // Contains-tests should return true if the contained object is
        // either entirely inside the containing object or lies on the
        // boundary of the containing object.

        #region Triangle2d contains V2d

        public static bool Contains(
            this Triangle2d triangle, V2d point
            )
        {
            var v0p = point - triangle.P0;
            return triangle.Line01.LeftValueOfDir(v0p) >= 0
                    && triangle.Line02.RightValueOfDir(v0p) >= 0
                    && triangle.Line12.LeftValueOfPos(point) >= 0;
        }

        #endregion

        #region Triangle2d contains Line2d (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Triangle2d triangle, Line2d linesegment)
            => triangle.Contains(linesegment.P0) && triangle.Contains(linesegment.P1);

        #endregion

        #region Triangle2d contains Triangle2d (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Triangle2d triangle, Triangle2d other)
            => triangle.Contains(other.P0) && triangle.Contains(other.P1) && triangle.Contains(other.P2);

        #endregion

        #region Triangle2d contains Quad2d (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Triangle2d triangle, Quad2d q)
            => triangle.Contains(q.P0) && triangle.Contains(q.P1) && triangle.Contains(q.P2) && triangle.Contains(q.P3);

        #endregion

        #region Triangle2d contains Circle2d - TODO

        public static bool Contains(this Triangle2d triangle, Circle2d circle)
            => throw new NotImplementedException();

        #endregion


        #region Circle2d contains V2d (sm)

        /// <summary>
        /// True if point p is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2d circle, V2d p)
            => (p - circle.Center).LengthSquared <= circle.RadiusSquared;

        #endregion

        #region Circle2d contains Line2d (sm)

        /// <summary>
        /// True if line segment is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2d circle, Line2d l)
            => circle.Contains(l.P0) && circle.Contains(l.P1);

        #endregion

        #region Circle2d contains Triangle2d (sm)

        /// <summary>
        /// True if triangle is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2d circle, Triangle2d t)
            => circle.Contains(t.P0) && circle.Contains(t.P1) && circle.Contains(t.P2);

        #endregion

        #region Circle2d contains Quad2d (sm)

        /// <summary>
        /// True if quad is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2d circle, Quad2d q)
            => circle.Contains(q.P0) && circle.Contains(q.P1) && circle.Contains(q.P2) && circle.Contains(q.P3);

        #endregion

        #region Circle2d contains Circle2d (sm)

        /// <summary>
        /// True if other circle is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Circle2d circle, Circle2d other)
            => (other.Center - circle.Center).Length + other.Radius <= circle.Radius;

        #endregion


        #region Quad2d contains V2d (haaser)

        /// <summary>
        /// True if point is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2d quad, V2d point)
        {
            return LeftValOfPos(0, 1, ref point) >= 0 &&
                   LeftValOfPos(1, 2, ref point) >= 0 &&
                   LeftValOfPos(2, 3, ref point) >= 0 &&
                   LeftValOfPos(3, 0, ref point) >= 0;

            double LeftValOfPos(int i0, int i1, ref V2d p)
                => (p.X - quad[i0].X) * (quad[i0].Y - quad[i1].Y) + (p.Y - quad[i0].Y) * (quad[i1].X - quad[i0].X);
        }

        #endregion

        #region Quad2d contains Line2d - TODO

        /// <summary>
        /// True if line segment is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2d quad, Line2d l)
            => throw new NotImplementedException();

        #endregion

        #region Quad2d contains Triangle2d - TODO

        /// <summary>
        /// True if triangle is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2d quad, Triangle2d t)
            => throw new NotImplementedException();

        #endregion

        #region Quad2d contains Quad2d - TODO

        /// <summary>
        /// True if other quad is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2d quad, Quad2d q)
            => throw new NotImplementedException();

        #endregion

        #region Quad2d contains Circle2d - TODO

        /// <summary>
        /// True if circle is contained in this quad.
        /// </summary>
        public static bool Contains(this Quad2d quad, Circle2d other)
            => throw new NotImplementedException();

        #endregion


        #region Box3d contains Quad3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
            this Box3d box, Quad3d quad
            )
        {
            return box.Contains(quad.P0)
                    && box.Contains(quad.P1)
                    && box.Contains(quad.P2)
                    && box.Contains(quad.P3);
        }

        #endregion

        #region Box3d contains Triangle3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
             this Box3d box, Triangle3d triangle
            )
        {
            return box.Contains(triangle.P0)
                    && box.Contains(triangle.P1)
                    && box.Contains(triangle.P2);
        }

        #endregion

        #region Box3d contains Sphere3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
             this Box3d box, Sphere3d sphere
            )
        {
            return box.Contains(sphere.Center)
                    && !box.Intersects(sphere);
        }

        #endregion

        #region Box3d contains Cylinder3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
            this Box3d box, Cylinder3d cylinder
           )
        {
            return box.Contains(cylinder.Center)
                    && !box.Intersects(cylinder);
        }

        #endregion


        #region Hull3d contains V3d

        /// <summary>
        /// Hull normals are expected to point outside.
        /// </summary>
        public static bool Contains(this Hull3d hull, V3d point)
        {
            var planes = hull.PlaneArray;
            for (var i = 0; i < planes.Length; i++)
            {
                if (planes[i].Height(point) > 0)
                    return false;
            }
            return true;
        }

        #endregion

        #region Hull3d contains Sphere3d (sm)

        /// <summary>
        /// Hull normals are expected to point outside.
        /// </summary>
        public static bool Contains(this Hull3d hull, Sphere3d sphere)
        {
            var planes = hull.PlaneArray;
            var negativeRadius = -sphere.Radius;
            for (var i = 0; i < planes.Length; i++)
            {
                if (planes[i].Height(sphere.Center) > negativeRadius)
                    return false;
            }
            return true;
        }

        #endregion


        #region Polygon2d contains V2d (haaser)

        internal static V3i InsideTriangleFlags(ref V2d p0, ref V2d p1, ref V2d p2, ref V2d point)
        {
            V2d n0 = new V2d(p0.Y - p1.Y, p1.X - p0.X);
            V2d n1 = new V2d(p1.Y - p2.Y, p2.X - p1.X);
            V2d n2 = new V2d(p2.Y - p0.Y, p0.X - p2.X);

            int t0 = Fun.Sign(n0.Dot(point - p0));
            int t1 = Fun.Sign(n1.Dot(point - p1));
            int t2 = Fun.Sign(n2.Dot(point - p2));

            if (t0 == 0) t1 = 1;
            if (t1 == 0) t1 = 1;
            if (t2 == 0) t2 = 1;

            return new V3i(t0, t1, t2);
        }

        internal static V3i InsideTriangleFlags(ref V2d p0, ref V2d p1, ref V2d p2, ref V2d point, int t0)
        {
            V2d n1 = new V2d(p1.Y - p2.Y, p2.X - p1.X);
            V2d n2 = new V2d(p2.Y - p0.Y, p0.X - p2.X);

            int t1 = Fun.Sign(n1.Dot(point - p1));
            int t2 = Fun.Sign(n2.Dot(point - p2));

            if (t1 == 0) t1 = 1;
            if (t2 == 0) t2 = 1;

            return new V3i(t0, t1, t2);
        }

        /// <summary>
        /// Returns true if the Polygon2d contains the given point.
        /// Works with all (convex and non-convex) Polygons.
        /// Assumes that the Vertices of the Polygon are sorted counter clockwise
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon2d poly, V2d point)
        {
            return poly.Contains(point, true);
        }

        /// <summary>
        /// Returns true if the Polygon2d contains the given point.
        /// Works with all (convex and non-convex) Polygons.
        /// CCW represents the sorting order of the Polygon-Vertices (true -> CCW, false -> CW)
        /// </summary>
        public static bool Contains(this Polygon2d poly, V2d point, bool CCW)
        {
            int pc = poly.PointCount;
            if (pc < 3)
                return false;
            int counter = 0;
            V2d p0 = poly[0], p1 = poly[1], p2 = poly[2];
            V3i temp = InsideTriangleFlags(ref p0, ref p1, ref p2, ref point);
            int t2_cache = temp.Z;
            if (temp.X == temp.Y && temp.Y == temp.Z) counter += temp.X;
            for (int pi = 3; pi < pc; pi++)
            {
                p1 = p2; p2 = poly[pi];
                temp = InsideTriangleFlags(ref p0, ref p1, ref p2, ref point, -t2_cache);
                t2_cache = temp.Z;
                if (temp.X == temp.Y && temp.Y == temp.Z) counter += temp.X;
            }
            if (CCW) return counter > 0;
            else return counter < 0;
        }

        #endregion


        #region Plane3d +- eps contains V3d (sm)

        /// <summary>
        /// Returns true if point is within given eps to plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Plane3d plane, double eps, V3d point)
        {
            var d = plane.Height(point);
            return d >= -eps && d <= eps;
        }

        #endregion

        #region Plane3d +- eps contains Box3d (sm)

        /// <summary>
        /// Returns true if the space within eps to a plane fully contains the given box.
        /// </summary>
        public static bool Contains(this Plane3d plane, double eps, Box3d box)
        {
            var corners = box.ComputeCorners();
            for (var i = 0; i < 8; i++)
            {
                var d = plane.Height(corners[i]);
                if (d < -eps || d > eps) return false;
            }
            return true;
        }

        #endregion

        #region Polygon3d +- eps contains V3d (sm)

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon3d polygon, double eps, V3d point, out double distance)
        {
            var plane = polygon.GetPlane3d();
            distance = plane.Height(point);
            if (distance < -eps || distance > eps) return false;
            var w2p = plane.GetWorldToPlane();
            var poly2d = new Polygon2d(polygon.GetPointArray().Map(p => w2p.TransformPos(p).XY));
            return poly2d.Contains(w2p.TransformPos(point).XY);
        }

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon3d polygon, Plane3d supportingPlane, Euclidean3d world2plane, Polygon2d poly2d, double eps, V3d point, out double distance)
        {
            distance = supportingPlane.Height(point);
            if (distance < -eps || distance > eps) return false;
            return poly2d.Contains(world2plane.TransformPos(point).XY);
        }

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this Polygon3d polygon, Plane3d supportingPlane, M44d world2plane, Polygon2d poly2d, double eps, V3d point, out double distance)
        {
            distance = supportingPlane.Height(point);
            if (distance < -eps || distance > eps) return false;
            return poly2d.Contains(world2plane.TransformPos(point).XY);
        }

        #endregion


        // Intersection tests

        #region Line2d intersects Line2d

        /// <summary>
        /// Returns true if the two line segments intersect.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Line2d l0, Line2d l1)
            => l0.IntersectsLine(l1.P0, l1.P1, out V2d _);

        /// <summary>
        /// Returns true if the two line segments intersect.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Line2d l0, Line2d l1, out V2d p)
            => l0.IntersectsLine(l1.P0, l1.P1, out p);

        /// <summary>
        /// Returns true if the two line segments intersect within given tolerance.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Line2d l0, Line2d l1, double absoluteEpsilon, out V2d p)
            => l0.IntersectsLine(l1.P0, l1.P1, absoluteEpsilon, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this Line2d line, V2d p0, V2d p1)
            => line.IntersectsLine(p0, p1, out _);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this Line2d line, V2d p0, V2d p1, out V2d p)
            => line.IntersectsLine(p0, p1, false, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// The intersection point is returned in p.
        /// If overlapping is true and the segments are parallel, then
        /// the closest point to p0 of the intersection range is returned in p.
        /// </summary>
        public static bool IntersectsLine(
            this Line2d line,
            V2d p0, V2d p1,
            bool overlapping,
            out V2d p
            )
        {
            var a = line.P0 - p0;

            if (Fun.IsTiny(a.LengthSquared))
            {
                p = p0;
                return true;
            }

            var u = line.P1 - line.P0;
            var v = p1 - p0;
            var lu = u.Length;

            var cross = u.X * v.Y - u.Y * v.X;
            if (!Fun.IsTiny(cross))
            {
                // non-parallel lines
                cross = 1 / cross;
                var t0 = (a.Y * v.X - a.X * v.Y) * cross;
                if (t0 > 1 || t0 < 0)
                {
                    p = V2d.NaN;
                    return false;
                }

                double t1 = (a.Y * u.X - a.X * u.Y) * cross;
                if (t1 > 1 || t1 < 0)
                {
                    p = V2d.NaN;
                    return false;
                }

                p = line.P0 + u * t0;
                return true;
            }
            else
            {
                // parallel lines
                if (!overlapping)
                {
                    p = V2d.NaN;
                    return false;
                }

                var normalizedDirection = u / lu;

                var r0 = new Range1d(0, lu);
                var r1 = new Range1d((p0 - line.P0).Dot(normalizedDirection), (p1 - line.P0).Dot(normalizedDirection));
                r1.Repair();

                if (r0.Intersects(r1, 0, out Range1d result))
                {
                    p = line.P0 + normalizedDirection * result.Min;
                    return true;
                }

                p = V2d.NaN;
                return false;
            }
        }

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1 within given tolerance.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this Line2d line, V2d p0, V2d p1, double absoluteEpsilon, out V2d p)
            => line.IntersectsLine(p0, p1, absoluteEpsilon, false, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1 within given tolerance.
        /// The intersection point is returned in p.
        /// If overlapping is true and the segments are parallel, then
        /// the closest point to p0 of the intersection range is returned in p.
        /// </summary>
        public static bool IntersectsLine(
            this Line2d line,
            V2d p0, V2d p1,
            double absoluteEpsilon,
            bool overlapping,
            out V2d p
            )
        {
            var a = line.P0 - p0;

            if (Fun.IsTiny(a.LengthSquared))
            {
                p = p0;
                return true;
            }

            var u = line.P1 - line.P0;
            var v = p1 - p0;

            var lu = u.Length;
            var lv = v.Length;
            var relativeEpsilonU = absoluteEpsilon / lu;
            var RelativeEpsilonV = absoluteEpsilon / lv;

            var cross = u.X * v.Y - u.Y * v.X;
            if (!Fun.IsTiny(cross))
            {
                // non-parallel lines
                cross = 1 / cross;
                var t0 = (a.Y * v.X - a.X * v.Y) * cross;
                if (t0 > 1 + relativeEpsilonU || t0 < -relativeEpsilonU)
                {
                    p = V2d.NaN;
                    return false;
                }

                var t1 = (a.Y * u.X - a.X * u.Y) * cross;
                if (t1 > 1 + RelativeEpsilonV || t1 < -RelativeEpsilonV)
                {
                    p = V2d.NaN;
                    return false;
                }

                p = line.P0 + u * t0;
                return true;
            }
            else
            {
                // parallel lines
                if (!overlapping)
                {
                    p = V2d.NaN;
                    return false;
                }

                var normalizedDirection = u / lu;

                var r0 = new Range1d(0, lu);
                var r1 = new Range1d((p0 - line.P0).Dot(normalizedDirection), (p1 - line.P0).Dot(normalizedDirection));
                r1.Repair();

                if (r0.Intersects(r1, absoluteEpsilon, out Range1d result))
                {
                    p = line.P0 + normalizedDirection * result.Min;
                    return true;
                }

                p = V2d.NaN;
                return false;
            }
        }

        #endregion

        #region Ray2d intersects Line2d

        /// <summary>
        /// Returns true if the Ray and the Line intersect.
        /// ATTENTION: Both-Sided Ray
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray2d ray,
            Line2d line
            )
        {
            return ray.IntersectsLine(line.P0, line.P1, out _);
        }

        /// <summary>
        /// returns true if the Ray and the Line intersect.
        /// t holds the smallest Intersection-Parameter for the Ray
        /// ATTENTION: t can be negative
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray2d ray,
            Line2d line,
            out double t
            )
        {
            return ray.IntersectsLine(line.P0, line.P1, out t);
        }


        /// <summary>
        /// returns true if the Ray and the Line(p0,p1) intersect.
        /// ATTENTION: Both-Sided Ray
        /// </summary>
        public static bool IntersectsLine(
            this Ray2d ray,
            V2d p0, V2d p1
            )
        {
            V2d n = new V2d(-ray.Direction.Y, ray.Direction.X);

            double d0 = n.Dot(p0 - ray.Origin);
            double d1 = n.Dot(p1 - ray.Origin);

            if (d0.Sign() != d1.Sign()) return true;
            else if (Fun.IsTiny(d0) && Fun.IsTiny(d1)) return true;
            else return false;
        }


        /// <summary>
        /// returns true if the Ray and the Line(p0,p1) intersect.
        /// t holds the Intersection-Parameter for the Ray
        /// If both Line-Points are on the Ray no Intersection is returned
        /// ATTENTION: t can be negative
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this Ray2d ray,
            V2d p0, V2d p1,
            out double t
            )
        {
            return ray.IntersectsLine(p0, p1, false, out t);
        }

        /// <summary>
        /// returns true if the Ray and the Line(p0,p1) intersect.
        /// if overlapping is true t holds the smallest Intersection-Parameter for the Ray
        /// if overlagging is false and both Line-Points are on the Ray no Intersection is returned
        /// ATTENTION: t can be negative
        /// </summary>
        public static bool IntersectsLine(
            this Ray2d ray,
            V2d p0, V2d p1,
            bool overlapping,
            out double t
            )
        {
            V2d a = p0 - ray.Origin;
            V2d u = p1 - p0;
            V2d v = ray.Direction;
            double lv2 = v.LengthSquared;


            double cross = u.X * v.Y - u.Y * v.X;
            double n = a.Y * u.X - a.X * u.Y;

            if (!Fun.IsTiny(cross))
            {
                cross = 1 / cross;

                double t0 = (a.Y * v.X - a.X * v.Y) * cross;
                if (t0 >= 0 && t0 <= 1)
                {
                    t = n * cross;
                    return true;
                }

                t = double.NaN;
                return false;
            }

            if (Fun.IsTiny(n) && overlapping)
            {
                double ta = v.Dot(a) / lv2;
                double tb = v.Dot(p1 - ray.Origin) / lv2;

                if ((ta < 0 && tb > 0) || (ta > 0 && tb < 0))
                {
                    t = 0;
                    return true;
                }
                else
                {
                    if (ta >= 0) t = Fun.Min(ta, tb);
                    else t = Fun.Max(ta, tb);

                    return true;
                }
            }

            t = double.NaN;
            return false;
        }



        #endregion

        #region Ray2d intersects Ray2d

        /// <summary>
        /// Returns true if the Rays intersect
        /// ATTENTION: Both-Sided Rays
        /// </summary>
        public static bool Intersects(
            this Ray2d r0,
            Ray2d r1
            )
        {
            if (!r0.Direction.IsParallelTo(r1.Direction)) return true;
            else
            {
                V2d n0 = new V2d(-r0.Direction.Y, r0.Direction.X);

                if (Fun.IsTiny(n0.Dot(r1.Origin - r0.Origin))) return true;
                else return false;
            }
        }

        /// <summary>
        /// Returns true if the Rays intersect
        /// t0 and t1 are the corresponding Ray-Parameters for the Intersection
        /// ATTENTION: Both-Sided Rays
        /// </summary>
        public static bool Intersects(
            this Ray2d r0, Ray2d r1,
            out double t0, out double t1
            )
        {
            V2d a = r0.Origin - r1.Origin;

            if (r0.Origin.ApproximateEquals(r1.Origin, Constant<double>.PositiveTinyValue))
            {
                t0 = 0;
                t1 = 0;
                return true;
            }

            V2d u = r0.Direction;
            V2d v = r1.Direction;

            double cross = u.X * v.Y - u.Y * v.X;

            if (!Fun.IsTiny(cross))
            {
                //Rays not parallel
                cross = 1 / cross;

                t0 = (a.Y * v.X - a.X * v.Y) * cross;
                t1 = (a.Y * u.X - a.X * u.Y) * cross;
                return true;
            }
            else
            {
                t0 = double.NaN;
                t1 = double.NaN;
                //Rays are parallel
                if (Fun.IsTiny(a.Y * u.X - a.X * u.Y)) return true;
                else return false;
            }
        }

        /// <summary>
        /// Returns true if the Rays intersect.
        /// </summary>
        public static bool Intersects(this Ray2d r0, Ray2d r1, out double t)
        {
            V2d a = r1.Origin - r0.Origin;
            if (a.Abs().AllSmaller(Constant<double>.PositiveTinyValue))
            {
                t = 0;
                return true; // Early exit when rays have same origin
            }

            double cross = r0.Direction.Dot270(r1.Direction);
            if (!Fun.IsTiny(cross)) // Rays not parallel
            {
                t = r1.Direction.Dot90(a) / cross;
                return true;
            }
            else // Rays are parallel
            {
                t = double.NaN;
                return false;
            }
        }

        #endregion


        #region Plane2d intersects Line2d

        /// <summary>
        /// Returns true if the Plane2d and the Line2d intersect or the Line2d
        /// lies completely in the Plane's Epsilon-Range
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        public static bool Intersects(
            this Plane2d plane,
            Line2d line,
            double absoluteEpsilon
            )
        {
            double lengthOfNormal2 = plane.Normal.LengthSquared;
            double d0 = plane.Height(line.P0);
            double d1 = plane.Height(line.P1);

            return d0 * d1 < absoluteEpsilon * absoluteEpsilon * lengthOfNormal2;
        }

        /// <summary>
        /// Returns true if the Plane2d and the Line2d intersect
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Plane2d plane,
            Line2d line
            )
        {
            return plane.IntersectsLine(line.P0, line.P1);
        }


        /// <summary>
        /// Returns true if the Plane2d and the line between p0 and p1 intersect
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this Plane2d plane,
            V2d p0, V2d p1
            )
        {
            double d0 = plane.Height(p0);
            double d1 = plane.Height(p1);

            return d0 * d1 <= 0;
        }

        /// <summary>
        /// Returns true if the Plane2d and the Line2d intersect
        /// point holds the Intersection-Point. If no Intersection is found point is V2d.NaN
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Plane2d plane,
            Line2d line,
            out V2d point
            )
        {
            return plane.Intersects(line, 0, out point);
        }

        /// <summary>
        /// Returns true if the Plane2d and the Line2d intersect or the Line2d
        /// lies completely in the Plane's Epsilon-Range
        /// point holds the Intersection-Point. If the Line2d is inside Epsilon point holds the centroid of the Line2d
        /// If no Intersection is found point is V2d.NaN
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        public static bool Intersects(
            this Plane2d plane,
            Line2d line,
            double absoluteEpsilon,
            out V2d point
            )
        {
            double h0 = plane.Height(line.P0);
            double h1 = plane.Height(line.P1);

            int s0 = (h0 > -absoluteEpsilon ? (h0 < absoluteEpsilon ? 0 : 1) : -1);
            int s1 = (h1 > -absoluteEpsilon ? (h1 < absoluteEpsilon ? 0 : 1) : -1);

            if (s0 == s1)
            {
                if (s0 != 0)
                {
                    point = V2d.NaN;
                    return false;
                }
                else
                {
                    point = (line.P0 + line.P1) * 0.5;
                    return true;
                }
            }
            else
            {
                if (s0 == 0)
                {
                    point = line.P0;
                    return true;
                }
                else if (s1 == 0)
                {
                    point = line.P1;
                    return true;
                }
                else
                {
                    V2d dir = line.Direction;
                    double no = plane.Normal.Dot(line.P0);
                    double nd = plane.Normal.Dot(dir);
                    double t = (plane.Distance - no) / nd;

                    point = line.P0 + t * dir;
                    return true;
                }
            }
        }



        #endregion

        #region Plane2d intersects Ray2d

        /// <summary>
        /// Returns true if the Plane2d and the Ray2d intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2d plane, Ray2d ray)
            => !plane.Normal.IsOrthogonalTo(ray.Direction);

        /// <summary>
        /// Returns true if the Plane2d and the Ray2d intersect.
        /// t holds the ray paramater of the intersection point if the intersection is found (else Double.PositiveInfinity).
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2d plane, Ray2d ray, out double t)
        {
            double dot = Vec.Dot(plane.Normal, ray.Direction);
            if (Fun.IsTiny(dot))
            {
                t = double.PositiveInfinity;
                return false;
            }
            t = -plane.Height(ray.Origin) / dot;
            return true;
        }

        /// <summary>
        /// Returns true if the Plane2d and the Ray2d intersect.
        /// t and p hold the ray paramater and point of the intersection if one is found (else Double.PositiveInfinity and V2d.PositiveInfinity).
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2d plane, Ray2d ray, out double t, out V2d p)
        {
            bool result = Intersects(plane, ray, out t);
            p = ray.Origin + t * ray.Direction;
            return result;
        }

        /// <summary>
        /// Returns the intersection point of the given Plane2d and Ray2d, or V2d.PositiveInfinity if ray is parallel to plane.
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d Intersect(this Plane2d plane, Ray2d ray)
        {
            double dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot)) return V2d.PositiveInfinity;
            return ray.GetPointOnRay(-plane.Height(ray.Origin) / dot);
        }

        #endregion

        #region Plane2d intersects Plane2d

        /// <summary>
        /// Returns true if the two Plane2ds intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2d p0, Plane2d p1)
        {
            var hit = p0.Coefficients.Cross(p1.Coefficients);
            return !hit.Z.IsTiny();
        }

        /// <summary>
        /// Returns true if the two Plane2ds intersect.
        /// Point holds the intersection point if an intersection is found.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane2d p0, Plane2d p1, out V2d point)
        {
            var hit = p0.Coefficients.Cross(p1.Coefficients);
            point = hit.XY / hit.Z;

            return !hit.Z.IsTiny();
        }

        #endregion

        #region Plane2d intersects IEnumerable<V2d>

        /// <summary>
        /// returns true if the Plane2d divides the Point-Cloud
        /// </summary>
        public static bool Divides(
            this Plane2d plane,
            IEnumerable<V2d> data
            )
        {
            int first = int.MinValue;
            foreach (var p in data)
            {
                if (first == int.MinValue)
                {
                    first = plane.Height(p).Sign();
                }
                else
                {
                    if (plane.Height(p).Sign() != first) return true;
                }
            }

            return false;
        }

        #endregion


        #region Circle2d intersects Circle2d (sm)

        /// <summary>
        /// Returns true if the circles intersect, or one contains the other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Circle2d c0, Circle2d c1)
            => (c0.Center - c1.Center).LengthSquared <= (c0.Radius + c1.Radius).Square();

        #endregion


        #region Triangle2d intersects Line2d

        /// <summary>
        /// Returns true if the triangle and the line intersect or the triangle contains the line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle2d triangle,
            Line2d line
            )
        {
            return triangle.IntersectsLine(line.P0, line.P1);
        }

        /// <summary>
        /// Returns true if the triangle and the line between p0 and p1 intersect or the triangle contains the line
        /// </summary>
        public static bool IntersectsLine(
            this Triangle2d triangle,
            V2d p0, V2d p1
            )
        {
            if(triangle.Contains(p0))return true;
            if(triangle.Contains(p1))return true;

            if(triangle.Line01.IntersectsLine(p0,p1))return true;
            if(triangle.Line12.IntersectsLine(p0,p1))return true;
            if(triangle.Line20.IntersectsLine(p0,p1))return true;

            return false;
        }

        #endregion

        #region Triangle2d intersects Ray2d

        /// <summary>
        /// Returns true if the Triangle and the Ray intersect
        /// </summary>
        public static bool Intersects(
            this Triangle2d triangle,
            Ray2d ray
            )
        {
            if (triangle.Contains(ray.Origin)) return true;

            if (ray.IntersectsLine(triangle.P0, triangle.P1)) return true;
            if (ray.IntersectsLine(triangle.P1, triangle.P2)) return true;
            if (ray.IntersectsLine(triangle.P2, triangle.P0)) return true;

            return false;
        }

        #endregion

        #region Triangle2d intersects Plane2d

        /// <summary>
        /// returns true if the Triangle2d and the Plane2d intersect
        /// </summary>
        public static bool Intersects(
            this Triangle2d triangle,
            Plane2d plane
            )
        {
            if (plane.Intersects(triangle.Line01)) return true;
            if (plane.Intersects(triangle.Line12)) return true;
            if (plane.Intersects(triangle.Line20)) return true;

            return false;
        }

        #endregion

        #region Triangle2d intersects Triangle2d

        /// <summary>
        /// Returns true if the triangles intersect or one contains the other.
        /// </summary>
        public static bool Intersects(this Triangle2d t0, Triangle2d t1)
        {
            var l = t0.Line01;
            if (l.IntersectsLine(t1.P0, t1.P1)) return true;
            if (l.IntersectsLine(t1.P1, t1.P2)) return true;
            if (l.IntersectsLine(t1.P2, t1.P0)) return true;

            l = t0.Line12;
            if (l.IntersectsLine(t1.P0, t1.P1)) return true;
            if (l.IntersectsLine(t1.P1, t1.P2)) return true;
            if (l.IntersectsLine(t1.P2, t1.P0)) return true;

            l = t0.Line20;
            if (l.IntersectsLine(t1.P0, t1.P1)) return true;
            if (l.IntersectsLine(t1.P1, t1.P2)) return true;
            if (l.IntersectsLine(t1.P2, t1.P0)) return true;

            if (t0.Contains(t1.P0)) return true;
            if (t1.Contains(t0.P0)) return true;

            return false;
        }

        #endregion


        #region Box2d intersects Line2d

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool Intersects(this Box2d box, Line2d line)
        {
            var out0 = box.OutsideFlags(line.P0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(line.P1); if (out1 == 0) return true;
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
                this Box2d box, Line2d line, Box.Flags out0, Box.Flags out1)
        {
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        private static bool IntersectsLine(
                this Box2d box, V2d p0, V2d p1)
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            return box.IntersectsLine(p0, p1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        private static bool IntersectsLine(
                this Box2d box, V2d p0, V2d p1,
                Box.Flags out0, Box.Flags out1)
        {
            if ((out0 & out1) != 0) return false;

            V2d min = box.Min;
            V2d max = box.Max;
            var bf = out0 | out1;

            if ((bf & Box.Flags.X) != 0)
            {
                double dx = p1.X - p0.X;
                if ((bf & Box.Flags.MinX) != 0)
                {
                    if (dx == 0 && p0.X < min.X) return false;
                    double t = (min.X - p0.X) / dx;
                    V2d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinX) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxX) != 0)
                {
                    if (dx == 0 && p0.X > max.X) return false;
                    double t = (max.X - p0.X) / dx;
                    V2d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxX) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Y) != 0)
            {
                double dy = p1.Y - p0.Y;
                if ((bf & Box.Flags.MinY) != 0)
                {
                    if (dy == 0 && p0.Y < min.Y) return false;
                    double t = (min.Y - p0.Y) / dy;
                    V2d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinY) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxY) != 0)
                {
                    if (dy == 0 && p0.Y > max.Y) return false;
                    double t = (max.Y - p0.Y) / dy;
                    V2d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxY) == 0)
                        return true;
                }
            }
            return false;
        }


        #endregion

        #region Box2d intersects Ray2d

        /// <summary>
        /// Returns true if the box and the ray intersect
        /// </summary>
        public static bool Intersects(
            this Box2d box,
            Ray2d ray
            )
        {
            /*
             * Getting a Normal-Vector for the Ray and calculating
             * the Normal Distances for every Box-Point:
             */
            V2d n = new V2d(-ray.Direction.Y, ray.Direction.X);

            double d0 = n.Dot(box.Min - ray.Origin);                                            //n.Dot(box.p0 - ray.Origin)
            double d1 = n.X * (box.Max.X - ray.Origin.X) + n.Y * (box.Min.Y - ray.Origin.Y);    //n.Dot(box.p1 - ray.Origin)
            double d2 = n.Dot(box.Max - ray.Origin);                                            //n.Dot(box.p2 - ray.Origin)
            double d3 = n.X * (box.Min.X - ray.Origin.X) + n.Y * (box.Max.Y - ray.Origin.Y);    //n.Dot(box.p3 - ray.Origin)

            /*
             * If Zero lies in the Range of the Distances there
             * have to be Points on both sides of the Ray.
             * This means the Box and the Ray have an Intersection
             */

            Range1d r = new Range1d(d0, d1, d2, d3);
            return r.Contains(0);
        }

        #endregion

        #region Box2d intersects Plane2d

        /// <summary>
        /// returns true if the box and the plane intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box2d box,
            Plane2d plane
            )
        {
            //UNTESTED
            return plane.Divides(box.ComputeCorners());
        }


        /// <summary>
        /// NOT TESTED YET.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box2d box,
            Plane2d plane,
            out Line2d line)
        {
            return Intersects(
                    plane.Normal.X, plane.Normal.Y, plane.Distance,
                    box.Min.X, box.Min.Y, box.Max.X, box.Max.Y,
                    out line);
        }

        /// <summary>
        /// Intersects an infinite line given by its normal vector [nx, ny]
        /// and its distance to the origin d, with an axis aligned box given
        /// by it minimal point [xmin, ymin] and its maximal point
        /// [xmax, ymax]. Returns true if there is an intersection and computes
        /// the actual intersection line.
        /// NOT TESTED YET.
        /// </summary>
        public static bool Intersects(
                double nx, double ny, double d,
                double xmin, double ymin, double xmax, double ymax,
                out Line2d line)
        {
            if (nx.IsTiny()) // horizontal
            {
                if (d <= ymin || d >= ymax) { line = default; return false; }
                line = new Line2d(new V2d(xmin, d), new V2d(xmax, d));
                return true;
            }

            if (ny.IsTiny()) // vertical
            {
                if (d <= xmin || d >= xmax) { line = default; return false; }
                line = new Line2d(new V2d(d, ymin), new V2d(d, ymax));
                return true;
            }

            if (nx.Sign() != ny.Sign())
            {
                double x0 = (d - ny * ymin) / nx;
                if (x0 >= xmax) { line = default; return false; }
                if (x0 > xmin) xmin = x0;
                double x1 = (d - ny * ymax) / nx;
                if (x1 <= xmin) { line = default; return false; }
                if (x1 < xmax) xmax = x1;

                double y0 = (d - nx * xmin) / ny;
                if (y0 >= ymax) { line = default; return false; }
                if (y0 > ymin) ymin = y0;
                double y1 = (d - nx * xmax) / ny;
                if (y1 <= ymin) { line = default; return false; }
                if (y1 < ymax) ymax = y1;

                line = new Line2d(new V2d(xmin, ymin), new V2d(xmax, ymax));
            }
            else
            {
                double x0 = (d - ny * ymax) / nx;
                if (x0 >= xmax) { line = default; return false; }
                if (x0 > xmin) xmin = x0;
                double x1 = (d - ny * ymin) / nx;
                if (x1 <= xmin) { line = default; return false; }
                if (x1 < xmax) xmax = x1;
                double y0 = (d - nx * xmax) / ny;
                if (y0 >= ymax) { line = default; return false; }
                if (y0 > ymin) ymin = y0;
                double y1 = (d - nx * xmin) / ny;
                if (y1 <= ymin) { line = default; return false; }
                if (y1 < ymax) ymax = y1;

                line = new Line2d(new V2d(xmax, ymin), new V2d(xmin, ymax));
            }
            return true;
        }

        #endregion

        #region Box2d intersects Triangle2d

        /// <summary>
        /// Returns true if the Box and the Triangle intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box2d box,
            Triangle2d triangle
            )
        {
            return box.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2);
        }

        /// <summary>
        /// Returns true if the Box and the Triangle intersect
        /// </summary>
        public static bool IntersectsTriangle(
            this Box2d box,
            V2d p0, V2d p1, V2d p2
            )
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            var out2 = box.OutsideFlags(p2); if (out2 == 0) return true;

            return box.IntersectsTriangle(p0, p1, p2, out0, out1, out2);
        }

        /// <summary>
        /// Returns true if the Box and the Triangle intersect. The outside flags
        /// of the end points of the Triangle with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        private static bool IntersectsTriangle(
            this Box2d box, V2d p0, V2d p1, V2d p2,
            Box.Flags out0, Box.Flags out1, Box.Flags out2
            )
        {

            /* ---------------------------------------------------------------
                If all of the points of the triangle are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2) != 0) return false;


            /* ---------------------------------------------------------------
               If two points of the triangle are not on the same side
               outside the box, it is possible that the edge between them
               intersects the box. The outside flags we computed are also
               used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(p0, p1, out0, out1)) return true;
            if (box.IntersectsLine(p1, p2, out1, out2)) return true;
            if (box.IntersectsLine(p2, p0, out2, out0)) return true;


            /* ---------------------------------------------------------------
               The only case left: The triangle contains the the whole box
               i.e. every point. When no triangle-line intersects the box and one
               box point is inside the triangle the triangle must contain the box
            --------------------------------------------------------------- */
            V2d a = box.Min - p0;
            V2d u = p1 - p0;
            V2d v = p2 - p0;


            double cross = u.X * v.Y - u.Y * v.X;
            if (Fun.IsTiny(cross)) return false;
            cross = 1 / cross;

            double t0 = (a.Y * v.X - a.X * v.Y) * cross; if (t0 < 0 || t0 > 1) return false;
            double t1 = (a.Y * u.X - a.X * u.Y) * cross; if (t1 < 0 || t1 > 1) return false;

            return (t0 + t1 < 1);
        }

        #endregion

        #region Box2d intersects Box2d (Box2d-Implementation)

        //Directly in Box-Implementation

        #endregion


        #region Quad2d intersects Line2d

        /// <summary>
        /// returns true if the Quad and the line intersect or the quad contains the line
        /// </summary>
        public static bool Intersects(
            this Quad2d quad,
            Line2d line
            )
        {
            if (quad.Contains(line.P0)) return true;
            if (quad.Contains(line.P1)) return true;

            if (line.IntersectsLine(quad.P0, quad.P1)) return true;
            if (line.IntersectsLine(quad.P1, quad.P2)) return true;
            if (line.IntersectsLine(quad.P2, quad.P3)) return true;
            if (line.IntersectsLine(quad.P3, quad.P0)) return true;

            return false;
        }

        /// <summary>
        /// returns true if the Quad and the line between p0 and p1 intersect or the quad contains the line
        /// </summary>
        public static bool IntersectsLine(
            this Quad2d quad,
            V2d p0, V2d p1
            )
        {
            if (quad.Contains(p0)) return true;
            if (quad.Contains(p1)) return true;

            Line2d line = new Line2d(p0, p1);
            if (line.IntersectsLine(quad.P0, quad.P1)) return true;
            if (line.IntersectsLine(quad.P1, quad.P2)) return true;
            if (line.IntersectsLine(quad.P2, quad.P3)) return true;
            if (line.IntersectsLine(quad.P3, quad.P0)) return true;

            return false;
        }

        #endregion

        #region Quad2d intersects Ray2d

        /// <summary>
        /// returns true if the quad and the ray intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad2d quad,
            Ray2d ray
            )
        {
            return ray.Plane2d.Divides(quad.Points);
        }

        #endregion

        #region Quad2d intersects Plane2d

        /// <summary>
        /// returns true if the Quad2d and the Plane2d intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad2d quad,
            Plane2d plane
            )
        {
            //UNTESTED
            if (plane.Divides(quad.Points)) return true;
            else return false;
        }

        #endregion

        #region Quad2d intersects Triangle2d

        /// <summary>
        /// returns true if the Quad2d and the Triangle2d intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Quad2d quad,
            Triangle2d triangle
            )
        {
            if (quad.Intersects(triangle.Line01)) return true;
            if (quad.Intersects(triangle.Line12)) return true;
            if (quad.Intersects(triangle.Line20)) return true;

            if (quad.Contains(triangle.P0)) return true;
            if (triangle.Contains(quad.P0)) return true;

            return false;
        }

        #endregion

        #region Quad2d intersects Box2d

        /// <summary>
        /// Returns true if the box and the Quad intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Box2d box,
            Quad2d quad
            )
        {
            Box.Flags out0 = box.OutsideFlags(quad.P0); if (out0 == 0) return true;
            Box.Flags out1 = box.OutsideFlags(quad.P1); if (out1 == 0) return true;
            Box.Flags out2 = box.OutsideFlags(quad.P2); if (out2 == 0) return true;
            Box.Flags out3 = box.OutsideFlags(quad.P3); if (out3 == 0) return true;


            /* ---------------------------------------------------------------
                If all of the points of the Quad are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2 & out3) != 0) return false;


            /* ---------------------------------------------------------------
               If two points of the Quad are not on the same side
               outside the box, it is possible that the edge between them
               intersects the box. The outside flags we computed are also
               used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(quad.P0, quad.P1, out0, out1)) return true;
            if (box.IntersectsLine(quad.P1, quad.P2, out1, out2)) return true;
            if (box.IntersectsLine(quad.P2, quad.P3, out2, out3)) return true;
            if (box.IntersectsLine(quad.P3, quad.P0, out3, out0)) return true;


            /* ---------------------------------------------------------------
               The only case left: The Quad contains the the whole box
               i.e. every point. When no triangle-line intersects the box and one
               box point is inside the Quad the Quad must contain the box
            --------------------------------------------------------------- */

            return quad.Contains(box.Min);
        }

        #endregion

        #region Quad2d intersects Quad2d

        /// <summary>
        /// returns true if the Quad2ds intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Quad2d q0,
            Quad2d quad
            )
        {
            if (q0.IntersectsLine(quad.P0, quad.P1)) return true;
            if (q0.IntersectsLine(quad.P1, quad.P2)) return true;
            if (q0.IntersectsLine(quad.P2, quad.P3)) return true;
            if (q0.IntersectsLine(quad.P3, quad.P0)) return true;

            if (q0.Contains(quad.P0)) return true;
            if (quad.Contains(q0.P0)) return true;

            return false;
        }

        #endregion


        #region Polygon2d intersects Line2d

        /// <summary>
        /// returns true if the Polygon2d and the Line2d intersect or the Polygon contains the Line
        /// </summary>
        public static bool Intersects(
            this Polygon2d poly,
            Line2d line
            )
        {
            foreach (var l in poly.EdgeLines)
            {
                if (l.Intersects(line)) return true;
            }

            if (poly.Contains(line.P0)) return true;

            return false;
        }

        #endregion

        #region Polygon2d intersects Ray2d

        /// <summary>
        /// returns true if the Polygon2d and the Ray2d intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Polygon2d poly,
            Ray2d ray
            )
        {
            //UNTESTED
            return ray.Plane2d.Divides(poly.Points);
        }


        #endregion

        #region Polygon2d intersects Plane2d

        /// <summary>
        /// returns true if the Polygon2d and the Plane2d itnersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Polygon2d poly,
            Plane2d plane
            )
        {
            //UNTESTED
            return plane.Divides(poly.Points);
        }

        #endregion

        #region Polygon2d intersects Triangle2d

        /// <summary>
        /// returns true if the Polygon2d and the Triangle2d intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2d poly,
            Triangle2d triangle
            )
        {
            foreach (var line in poly.EdgeLines)
            {
                if (triangle.Intersects(line)) return true;
            }

            if (triangle.Contains(poly[0])) return true;
            if (poly.Contains(triangle.P0)) return true;

            return false;
        }

        #endregion

        #region Polygon2d intersects Box2d

        /// <summary>
        /// returns true if the Polygon2d and the Box2d intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2d poly,
            Box2d box
            )
        {
            //UNTESTED
            int count = poly.PointCount;
            Box.Flags[] outFlags = new Box.Flags[count];

            int i0 = 0;
            foreach (var p in poly.Points) outFlags[i0++] = box.OutsideFlags(p);

            i0 = 0;
            int i1 = 1;
            foreach (var l in poly.EdgeLines)
            {
                if (box.Intersects(l, outFlags[i0], outFlags[i1])) return true;

                i0++;
                i1 = (i1 + 1) % count;
            }

            if (box.Contains(poly[0])) return true;
            if (poly.Contains(box.Min)) return true;

            return false;
        }

        #endregion

        #region Polygon2d intersects Quad2d

        /// <summary>
        /// returns true if the Polygon2d and the Quad2d interset or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2d poly,
            Quad2d quad
            )
        {
            foreach (var l in poly.EdgeLines)
            {
                if (quad.Intersects(l)) return true;
            }

            if (quad.Contains(poly[0])) return true;
            if (poly.Contains(quad.P0)) return true;

            return false;
        }

        #endregion

        #region Polygon2d intersects Polygon2d

        /// <summary>
        /// returns true if the Polygon2ds intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this Polygon2d poly0,
            Polygon2d poly1
            )
        {
            //check if projected ranges intersect for all possible normals


            V2d[] allnormals = new V2d[poly0.PointCount + poly1.PointCount];
            int c = 0;

            foreach (var d in poly0.Edges)
            {
                allnormals[c] = new V2d(-d.Y, d.X);
                c++;
            }
            foreach (var d in poly1.Edges)
            {
                allnormals[c] = new V2d(-d.Y, d.X);
                c++;
            }



            foreach (var n in allnormals)
            {
                var r0 = poly0.ProjectTo(n);
                var r1 = poly1.ProjectTo(n);

                if (!r0.Intersects(r1)) return false;
            }

            return true;
        }

        private static Range1d ProjectTo(this Polygon2d poly, V2d dir)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (var p in poly.Points)
            {
                double dotproduct = p.Dot(dir);

                if (dotproduct < min) min = dotproduct;
                if (dotproduct > max) max = dotproduct;
            }

            return new Range1d(min, max);
        }

        #endregion


        #region Line3d intersects Line3d (haaser)

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3d l0,
            Line3d l1
            )
        {
            return l0.Intersects(l1, Constant<double>.PositiveTinyValue);
        }

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than absoluteEpsilon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3d l0,
            Line3d l1,
            double absoluteEpsilon
            )
        {
            if (l0.GetMinimalDistanceTo(l1) < absoluteEpsilon) return true;
            else return false;
        }

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than absoluteEpsilon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3d l0,
            Line3d l1,
            double absoluteEpsilon,
            out V3d point
            )
        {
            if (l0.GetMinimalDistanceTo(l1, out point) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region Line3d intersects Special (inconsistent argument order)

        #region Line3d intersects Plane3d

        /// <summary>
        /// Returns true if the line and the plane intersect.
        /// </summary>
        public static bool Intersects(
             this Line3d line, Plane3d plane, out double t
             )
        {
            if (!line.Ray3d.Intersects(plane, out t)) return false;
            if (t >= 0 && t <= 1) return true;
            t = double.PositiveInfinity;
            return false;

        }

        /// <summary>
        /// Returns true if the line and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Line3d line, Plane3d plane, out double t, out V3d p
             )
        {
            bool result = line.Intersects(plane, out t);
            p = line.Origin + t * line.Direction;
            return result;
        }

        #endregion

        #region Line3d intersects Triangle3d

        /// <summary>
        /// Returns true if the line and the triangle intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Line3d line,
            Triangle3d triangle
            )
        {
            return line.Ray3d.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2, 0, 1, out _);
        }

        /// <summary>
        /// Returns true if the line and the triangle intersect.
        /// point holds the intersection point.
        /// </summary>
        public static bool Intersects(
            this Line3d line,
            Triangle3d triangle,
            out V3d point
            )
        {
            Ray3d ray = line.Ray3d;

            if (ray.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2, 0, 1, out double temp))
            {
                point = ray.GetPointOnRay(temp);
                return true;
            }
            else
            {
                point = V3d.NaN;
                return false;
            }
        }

        #endregion

        #endregion


        #region Ray3d intersects Line3d (haaser)

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than Constant&lt;double&gt;.PositiveTinyValue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3d ray, Line3d line
            )
        {
            return ray.Intersects(line, Constant<double>.PositiveTinyValue);
        }

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than absoluteEpsilon
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3d ray, Line3d line,
            double absoluteEpsilon
            )
        {
            if (ray.GetMinimalDistanceTo(line) < absoluteEpsilon) return true;
            else return false;
        }

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than absoluteEpsilon
        /// t holds the corresponding ray parameter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3d ray, Line3d line,
            double absoluteEpsilon,
            out double t
            )
        {
            if (ray.GetMinimalDistanceTo(line, out t) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region Ray3d intersects Ray3d (haaser)

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than Constant&lt;double&gt;.PositiveTinyValue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3d r0,
            Ray3d r1
            )
        {
            return r0.Intersects(r1, out _, out _, Constant<double>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than Constant&lt;double&gt;.PositiveTinyValue
        /// t0 and t1 hold the ray-parameters for the intersection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3d r0,
            Ray3d r1,
            out double t0,
            out double t1
            )
        {
            return r0.Intersects(r1, out t0, out t1, Constant<double>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than absoluteEpsilon
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3d r0,
            Ray3d r1,
            double absoluteEpsilon
            )
        {
            return r0.Intersects(r1, out _, out _, absoluteEpsilon);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than absoluteEpsilon
        /// t0 and t1 hold the ray-parameters for the intersection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Ray3d r0,
            Ray3d r1,
            out double t0,
            out double t1,
            double absoluteEpsilon
            )
        {
            if (r0.GetMinimalDistanceTo(r1, out t0, out t1) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region Ray3d intersects Special (inconsistent argument order)

        #region Ray3d intersects Triangle3d

        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the Ray3d.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3d ray, Triangle3d triangle)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                double.MinValue, double.MaxValue);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3d ray, Triangle3d triangle, double tmin, double tmax)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                tmin, tmax);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3d ray, Triangle3d triangle, double tmin, double tmax, out double t)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTriangle(this Ray3d ray, V3d p0, V3d p1, V3d p2, double tmin, double tmax)
            => ray.IntersectsTriangle(p0, p1, p2, tmin, tmax, out double _);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        public static bool IntersectsTriangle(this Ray3d ray, V3d p0, V3d p1, V3d p2, double tmin, double tmax, out double t)
        {
            var edge01 = p1 - p0;
            var edge02 = p2 - p0;
            var plane = Vec.Cross(ray.Direction, edge02);
            var det = Vec.Dot(edge01, plane);
            if (det > -1E-7 && det < 1E-7) { t = double.NaN; return false; }
            //ray ~= parallel / Triangle
            var tv = ray.Origin - p0;
            det = 1 / det;  // det is now inverse det
            var u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) { t = double.NaN; return false; }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            var v = Vec.Dot(ray.Direction, plane) * det;
            if (v < 0 || u + v > 1) { t = double.NaN; return false; }
            var temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < tmin || temp_t >= tmax) { t = double.NaN; return false; }

            t = temp_t;
            return true;
        }


        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTrianglePointAndEdges(this Ray3d ray, V3d p0, V3d edge01, V3d edge02, double tmin, double tmax)
            => ray.IntersectsTrianglePointAndEdges(p0, edge01, edge02, tmin, tmax, out _);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        public static bool IntersectsTrianglePointAndEdges(this Ray3d ray, V3d p0, V3d edge01, V3d edge02, double tmin, double tmax, out double t)
        {
            var plane = Vec.Cross(ray.Direction, edge02);
            var det = Vec.Dot(edge01, plane);
            if (det > -1E-7 && det < 1E-7) { t = double.NaN; return false; }
            //ray ~= parallel / Triangle
            var tv = ray.Origin - p0;
            det = 1 / det;  // det is now inverse det
            var u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) { t = double.NaN; return false; }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            var v = Vec.Dot(ray.Direction, plane) * det;
            if (v < 0 || u + v > 1) { t = double.NaN; return false; }
            var temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < tmin || temp_t >= tmax) { t = double.NaN; return false; }

            t = temp_t;
            return true;
        }

        #endregion

        #region Ray3d intersects Quad3d


        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the Ray3d.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3d ray, Quad3d quad)
            => ray.Intersects(quad, double.MinValue, double.MaxValue);

        /// <summary>
        /// Returns true if the ray and the quad intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// </summary>
        public static bool Intersects(this Ray3d ray, Quad3d quad, double tmin, double tmax)
        {
            var edge02 = quad.P2 - quad.P0;
            return ray.IntersectsTrianglePointAndEdges(quad.P0, quad.Edge01, edge02, tmin, tmax)
                || ray.IntersectsTrianglePointAndEdges(quad.P0, edge02, quad.Edge03, tmin, tmax);
        }

        /// <summary>
        /// Returns true if the ray and the quad intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// </summary>
        public static bool IntersectsQuad(this Ray3d ray, V3d p0, V3d p1, V3d p2, V3d p3, double tmin, double tmax)
        {
            var edge02 = p2 - p0;
            return ray.IntersectsTrianglePointAndEdges(p0, p1 - p0, edge02, tmin, tmax)
                || ray.IntersectsTrianglePointAndEdges(p0, edge02, p3 - p0, tmin, tmax);
        }

        #endregion

        #region Ray3d intersects Polygon3d (haaser)

        /// <summary>
        /// Returns true if the ray and the polygon intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3d ray, Polygon3d poly, double tmin, double tmax)
            => ray.Intersects(poly, tmin, tmax, out _);

        /// <summary>
        /// Returns true if the ray and the polygon intersect within the
        /// supplied parameter interval of the ray.
        /// t holds the correspoinding paramter.
        /// </summary>
        public static bool Intersects(this Ray3d ray, Polygon3d poly, double tmin, double tmax, out double t)
        {
            var tris = poly.ComputeTriangulationOfConcavePolygon(1E-5);
            var count = tris.Length;

            for (var i = 0; i < count; i += 3)
            {
                if (ray.IntersectsTriangle(poly[tris[i + 0]], poly[tris[i + 1]], poly[tris[i + 2]],
                                           tmin, tmax, out t))
                {
                    return true;
                }
            }

            t = double.NaN;
            return false;
        }

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices, intersect within the
        /// supplied parameter interval of the ray.
        /// (The Method triangulates the polygon)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsPolygon(this Ray3d ray, V3d[] vertices, double tmin, double tmax)
            => ray.Intersects(new Polygon3d(vertices), tmin, tmax);

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices and triangulation, intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        public static bool IntersectsPolygon(
            this Ray3d ray,
            V3d[] vertices,
            int[] triangulation,
            double tmin, double tmax
            )
        {
            for (var i = 0; i < triangulation.Length; i += 3)
            {
                if (ray.IntersectsTriangle(vertices[triangulation[i + 0]],
                                           vertices[triangulation[i + 1]],
                                           vertices[triangulation[i + 2]], tmin, tmax)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices and triangulation, intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        public static bool Intersects(
            this Ray3d ray,
            Polygon3d polygon,
            int[] triangulation,
            double tmin, double tmax
            )
        {
            for (var i = 0; i < triangulation.Length; i += 3)
            {
                if (ray.IntersectsTriangle(polygon[triangulation[i + 0]],
                                           polygon[triangulation[i + 1]],
                                           polygon[triangulation[i + 2]], tmin, tmax)) return true;
            }
            return false;
        }

        #endregion

        #region Ray3d intersects Sphere3d

        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the Ray3d.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3d ray, Sphere3d sphere)
            => ray.Intersects(sphere, double.MinValue, double.MaxValue);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the Ray3d.Hits method.
        /// </summary>
        public static bool Intersects(this Ray3d ray, Sphere3d sphere, double tmin, double tmax)
        {
            // calculate closest point
            var t = ray.Direction.Dot(sphere.Center - ray.Origin) / ray.Direction.LengthSquared;
            if (t < 0) t = 0;
            if (t < tmin) t = tmin;
            if (t > tmax) t = tmax;
            V3d p = ray.Origin + t * ray.Direction;

            // distance to sphere?
            var d = (p - sphere.Center).LengthSquared;
            if (d <= sphere.RadiusSquared)
                return true;

            return false;
        }

        #endregion

        #region Sphere3d intersects Triangle3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Sphere3d sphere, Triangle3d triangle
             )
        {
            V3d v = sphere.Center.GetClosestPointOn(triangle) - sphere.Center;
            return sphere.RadiusSquared >= v.LengthSquared;
        }

        #endregion

        #endregion


        #region Triangle3d intersects Line3d (haaser)

        /// <summary>
        /// Returns true if the triangle and the line intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3d tri,
            Line3d line
            )
        {
            return tri.IntersectsLine(line.P0, line.P1, out _);
        }

        /// <summary>
        /// Returns true if the triangle and the line intersect.
        /// point holds the intersection point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3d tri,
            Line3d line,
            out V3d point
            )
        {
            return tri.IntersectsLine(line.P0, line.P1, out point);
        }

        /// <summary>
        /// returns true if the triangle and the line, given by p0 and p1, intersect.
        /// </summary>
        public static bool IntersectsLine(
            this Triangle3d tri,
            V3d p0, V3d p1
            )
        {
            V3d edge01 = tri.Edge01;
            V3d edge02 = tri.Edge02;
            V3d dir = p1 - p0;

            V3d plane = Vec.Cross(dir, edge02);
            double det = Vec.Dot(edge01, plane);
            if (det > -1E-7 && det < 1E-7)return false;
            //ray ~= parallel / Triangle
            V3d tv = p0 - tri.P0;
            det = 1 / det;  // det is now inverse det
            double u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1)
            {
                return false;
            }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            double v = Vec.Dot(dir, plane) * det;
            if (v < 0 || u + v > 1)
            {
                return false;
            }
            double temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < 0 || temp_t >= 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// returns true if the triangle and the line, given by p0 and p1, intersect.
        /// point holds the intersection point.
        /// </summary>
        public static bool IntersectsLine(
            this Triangle3d tri,
            V3d p0, V3d p1,
            out V3d point
            )
        {
            V3d edge01 = tri.Edge01;
            V3d edge02 = tri.Edge02;
            V3d dir = p1 - p0;

            V3d plane = Vec.Cross(dir, edge02);
            double det = Vec.Dot(edge01, plane);
            if (det > -1E-7 && det < 1E-7) { point = V3d.NaN; return false; }
            //ray ~= parallel / Triangle
            V3d tv = p0 - tri.P0;
            det = 1 / det;  // det is now inverse det
            double u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1)
            {
                point = V3d.NaN;
                return false;
            }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            double v = Vec.Dot(dir, plane) * det;
            if (v < 0 || u + v > 1)
            {
                point = V3d.NaN;
                return false;
            }
            double temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < 0 || temp_t >= 1)
            {
                point = V3d.NaN;
                return false;
            }

            point = p0 + temp_t * dir;
            return true;
        }


        #endregion

        #region Triangle3d intersects Ray3d (haaser)

        /// <summary>
        /// Returns true if the triangle and the ray intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3d tri,
            Ray3d ray
            )
        {
            return tri.Intersects(ray, double.MinValue, double.MaxValue, out _);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect
        /// within the given parameter interval
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3d tri,
            Ray3d ray,
            double tmin, double tmax
            )
        {
            return tri.Intersects(ray, tmin, tmax, out _);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect.
        /// t holds the intersection paramter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3d tri,
            Ray3d ray,
            out double t
            )
        {
            return tri.Intersects(ray, double.MinValue, double.MaxValue, out t);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect
        /// within the given parameter interval
        /// t holds the intersection paramter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Triangle3d tri,
            Ray3d ray,
            double tmin, double tmax,
            out double t
            )
        {
            return ray.Intersects(tri, tmin, tmax, out t);
        }

        #endregion

        #region Triangle3d intersects Triangle3d (haaser)

        /// <summary>
        /// Returns true if the triangles intersect.
        /// </summary>
        public static bool Intersects(
            this Triangle3d t0,
            Triangle3d t1
            )
        {
            if (t0.IntersectsLine(t1.P0, t1.P1, out _)) return true;
            if (t0.IntersectsLine(t1.P1, t1.P2, out _)) return true;
            if (t0.IntersectsLine(t1.P2, t1.P0, out _)) return true;

            if (t1.IntersectsLine(t0.P0, t0.P1, out _)) return true;
            if (t1.IntersectsLine(t0.P1, t0.P2, out _)) return true;
            if (t1.IntersectsLine(t0.P2, t0.P0, out _)) return true;

            return false;
        }

        /// <summary>
        /// Returns true if the triangles intersect.
        /// line holds the cutting-line of the two triangles.
        /// </summary>
        public static bool Intersects(
            this Triangle3d t0,
            Triangle3d t1,
            out Line3d line
            )
        {
            List<V3d> points = new List<V3d>();

            if (t0.IntersectsLine(t1.P0, t1.P1, out V3d temp)) points.Add(temp);
            if (t0.IntersectsLine(t1.P1, t1.P2, out temp)) points.Add(temp);
            if (t0.IntersectsLine(t1.P2, t1.P0, out temp)) points.Add(temp);

            if (points.Count == 2)
            {
                line = new Line3d(points[0], points[1]);
                return true;
            }

            if (t1.IntersectsLine(t0.P0, t0.P1, out temp)) points.Add(temp);
            if (t1.IntersectsLine(t0.P1, t0.P2, out temp)) points.Add(temp);
            if (t1.IntersectsLine(t0.P2, t0.P0, out temp)) points.Add(temp);

            if (points.Count == 2)
            {
                line = new Line3d(points[0], points[1]);
                return true;
            }

            line = new Line3d(V3d.NaN, V3d.NaN);
            return false;
        }

        #endregion


        #region Quad3d intersects Line3d (haaser)

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3d quad,
            Line3d line)
        {
            return quad.IntersectsLine(line.P0, line.P1, out _);
        }

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// point holds the intersection point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3d quad,
            Line3d line,
            out V3d point)
        {
            return quad.IntersectsLine(line.P0, line.P1, out point);
        }


        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// </summary>
        public static bool IntersectsLine(
            this Quad3d quad,
            V3d p0, V3d p1)
        {
            Ray3d ray = new Ray3d(p0, p1 - p0);
            if (quad.Intersects(ray, 0, 1, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// point holds the intersection point.
        /// </summary>
        public static bool IntersectsLine(
            this Quad3d quad,
            V3d p0, V3d p1,
            out V3d point)
        {
            Ray3d ray = new Ray3d(p0, p1 - p0);
            if (quad.Intersects(ray, 0, 1, out double t))
            {
                point = ray.GetPointOnRay(t);
                return true;
            }
            else
            {
                point = V3d.NaN;
                return false;
            }
        }

        #endregion

        #region Quad3d intersects Ray3d (haaser)


        /// <summary>
        /// Returns true if the quad and the ray intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3d quad,
            Ray3d ray
            )
        {
            return quad.Intersects(ray, double.MinValue, double.MaxValue, out _);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect.
        /// t holds the intersection parameter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3d quad,
            Ray3d ray,
            out double t
            )
        {
            return quad.Intersects(ray, double.MinValue, double.MaxValue, out t);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect
        /// within the given paramter range
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Quad3d quad,
            Ray3d ray,
            double tmin, double tmax
            )
        {
            return quad.Intersects(ray, tmin, tmax, out _);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect
        /// within the given paramter range
        /// t holds the intersection parameter.
        /// </summary>
        public static bool Intersects(
            this Quad3d quad,
            Ray3d ray,
            double tmin, double tmax,
            out double t
            )
        {
            V3d edge02 = quad.P2 - quad.P0;
            if (ray.IntersectsTrianglePointAndEdges(quad.P0, quad.Edge01, edge02, tmin, tmax, out t)) return true;
            if (ray.IntersectsTrianglePointAndEdges(quad.P0, edge02, quad.Edge03, tmin, tmax, out t)) return true;

            t = double.NaN;
            return false;
        }

        #endregion

        #region Quad3d intersects Triangle3d (haaser)

        /// <summary>
        /// Returns true if the quad and the triangle intersect.
        /// </summary>
        public static bool Intersects(
            this Quad3d quad,
            Triangle3d tri
            )
        {
            if (quad.IntersectsLine(tri.P0, tri.P1)) return true;
            if (quad.IntersectsLine(tri.P1, tri.P2)) return true;
            if (quad.IntersectsLine(tri.P2, tri.P0)) return true;

            if (tri.IntersectsLine(quad.P0, quad.P1)) return true;
            if (tri.IntersectsLine(quad.P1, quad.P2)) return true;
            if (tri.IntersectsLine(quad.P2, quad.P3)) return true;
            if (tri.IntersectsLine(quad.P3, quad.P0)) return true;

            return false;
        }


        /// <summary>
        /// Returns true if the quad and the triangle, given by p0/p1/p1, intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTriangle(
            this Quad3d quad,
            V3d p0, V3d p1, V3d p2
            )
        {
            Triangle3d tri = new Triangle3d(p0, p1, p2);
            return quad.Intersects(tri);
        }

        #endregion

        #region Quad3d intersects Quad3d (haaser)

        /// <summary>
        /// Returns true if the given quads intersect.
        /// </summary>
        public static bool Intersects(
            this Quad3d q0,
            Quad3d q1
            )
        {
            if (q0.IntersectsTriangle(q1.P0, q1.P1, q1.P2)) return true;
            if (q0.IntersectsTriangle(q1.P2, q1.P3, q1.P0)) return true;

            if (q1.IntersectsTriangle(q0.P0, q0.P1, q0.P2)) return true;
            if (q1.IntersectsTriangle(q0.P2, q0.P3, q0.P0)) return true;

            return false;
        }

        #endregion


        #region Plane3d intersects Line3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane3d plane, Line3d line)
        {
            return plane.IntersectsLine(line.P0, line.P1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane3d plane, Line3d line, double absoluteEpsilon)
        {
            return plane.IntersectsLine(line.P0, line.P1, absoluteEpsilon);
        }

        public static bool IntersectsLine(this Plane3d plane, V3d p0, V3d p1, double absoluteEpsilon)
        {
            double h0 = plane.Height(p0);
            int s0 = (h0 > absoluteEpsilon ? 1 :(h0 < -absoluteEpsilon ? -1 : 0));
            if (s0 == 0) return true;

            double h1 = plane.Height(p1);
            int s1 = (h1 > absoluteEpsilon ? 1 : (h1 < -absoluteEpsilon ? -1 : 0));
            if (s1 == 0) return true;


            if (s0 == s1) return false;
            else return true;
        }

        public static bool IntersectsLine(this Plane3d plane, V3d p0, V3d p1, double absoluteEpsilon, out V3d point)
        {
            //<n|origin + t0*dir> == d
            //<n|or> + t0*<n|dir> == d
            //t0 == (d - <n|or>) / <n|dir>;

            V3d dir = p1 - p0;
            double ld = dir.Length;
            dir /= ld;

            double nDotd = plane.Normal.Dot(dir);


            if (!Fun.IsTiny(nDotd))
            {
                double t0 = (plane.Distance - plane.Normal.Dot(p0)) / nDotd;

                if (t0 >= -absoluteEpsilon && t0 <= ld + absoluteEpsilon)
                {
                    point = p0 + dir * t0;
                    return true;
                }
                else
                {
                    point = V3d.NaN;
                    return false;
                }
            }
            else
            {
                point = V3d.NaN;
                return false;
            }

        }

        #endregion

        #region Plane3d intersects Ray3d

        /// <summary>
        /// Returns true if the Ray3d and the Plane3d intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Ray3d ray, Plane3d plane)
            => !plane.Normal.IsOrthogonalTo(ray.Direction);

        /// <summary>
        /// Returns true if the ray and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Ray3d ray, Plane3d plane, out double t
             )
        {
            double dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot))
            {
                t = double.PositiveInfinity;
                return false;
            }
            t = -plane.Height(ray.Origin) / dot;
            return true;
        }

        /// <summary>
        /// Returns the intersection point with the given plane, or V3d.PositiveInfinity if ray is parallel to plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d Intersect(
             this Ray3d ray, Plane3d plane
             )
        {
            double dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot)) return V3d.PositiveInfinity;
            return ray.GetPointOnRay(-plane.Height(ray.Origin) / dot);
        }

        /// <summary>
        /// Returns true if the ray and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Ray3d ray, Plane3d plane, out double t, out V3d p
             )
        {
            bool result = Intersects(ray, plane, out t);
            p = ray.Origin + t * ray.Direction;
            return result;
        }

        #endregion

        #region Plane3d intersects Plane3d

        public static bool Intersects(this Plane3d p0, Plane3d p1)
        {
            bool parallel = p0.Normal.IsParallelTo(p1.Normal);

            if (parallel) return Fun.IsTiny(p0.Distance - p1.Distance);
            else return true;
        }

        public static bool Intersects(this Plane3d p0, Plane3d p1, out Ray3d ray)
        {
            V3d dir = p0.Normal.Cross(p1.Normal);
            double len = dir.Length;

            if (Fun.IsTiny(len))
            {
                if (Fun.IsTiny(p0.Distance - p1.Distance))
                {
                    ray = new Ray3d(p0.Normal * p0.Distance, V3d.Zero);
                    return true;
                }
                else
                {
                    ray = Ray3d.Invalid;
                    return false;
                }
            }

            dir *= 1 / len;

            var alu = new double[,]
                { { p0.Normal.X, p0.Normal.Y, p0.Normal.Z },
                  { p1.Normal.X, p1.Normal.Y, p1.Normal.Z },
                  { dir.X, dir.Y, dir.Z } };

            int[] p = alu.LuFactorize();

            var b = new double[] { p0.Distance, p1.Distance, 0 };

            var x = alu.LuSolve(p, b);

            ray = new Ray3d(new V3d(x), dir);
            return true;
        }

        #endregion

        #region Plane3d intersects Plane3d intersects Plane3d

        public static bool Intersects(this Plane3d p0, Plane3d p1, Plane3d p2, out V3d point)
        {
            var alu = new double[,]
                { { p0.Normal.X, p0.Normal.Y, p0.Normal.Z },
                  { p1.Normal.X, p1.Normal.Y, p1.Normal.Z },
                  { p2.Normal.X, p2.Normal.Y, p2.Normal.Z } };

            var p = new int[3];
            if (!alu.LuFactorize(p)) { point = V3d.NaN; return false; }
            var b = new double[] { p0.Distance, p1.Distance, p2.Distance };
            var x = alu.LuSolve(p, b);
            point = new V3d(x);
            return true;
        }

        #endregion

        #region Plane3d intersects Triangle3d

        /// <summary>
        /// Returns whether the given plane and triangle intersect.
        /// </summary>
        public static bool Intersects(
             this Plane3d plane, Triangle3d triangle
             )
        {
            int sign = plane.Sign(triangle.P0);
            if (sign == 0) return true;
            if (sign != plane.Sign(triangle.P1)) return true;
            if (sign != plane.Sign(triangle.P2)) return true;
            return false;
        }

        #endregion

        #region Plane3d intersects Sphere3d

        /// <summary>
        /// Returns whether the given sphere and plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Plane3d plane, Sphere3d sphere
             )
        {
            return sphere.Radius >= plane.Height(sphere.Center).Abs();
        }

        #endregion

        #region Plane3d intersects Polygon3d

        /// <summary>
        /// returns true if the Plane3d and the Polygon3d intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Plane3d plane,
            Polygon3d poly
            )
        {
            return plane.Intersects(poly, Constant<double>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the Plane3d and the polygon, intersect
        /// within a tolerance of absoluteEpsilon
        /// </summary>
        public static bool Intersects(
            this Plane3d plane,
            Polygon3d polygon,
            double absoluteEpsilon
            )
        {
            double height = plane.Height(polygon[0]);
            int sign0 = height > -absoluteEpsilon ? (height < absoluteEpsilon ? 0 : 1) : -1; if (sign0 == 0) return true;
            int pc = polygon.PointCount;
            for (int i = 1; i < pc; i++)
            {
                height = plane.Height(polygon[i]);
                int sign = height > -absoluteEpsilon ? (height < absoluteEpsilon ? 0 : 1) : -1;
                if (sign != sign0) return true;
            }

            return false;
        }


        /// <summary>
        /// returns true if the Plane3d and the Polygon3d intersect.
        /// line holds the intersection line
        /// ATTENTION: works only with non-concave polygons!
        /// </summary>
        public static bool IntersectsConvex(
            this Plane3d plane,
            Polygon3d poly,
            out Line3d line
            )
        {
            return plane.IntersectsConvex(poly, Constant<double>.PositiveTinyValue, out line);
        }

        /// <summary>
        /// Returns true if the Plane3d and the polygon, given by points, intersect
        /// within a tolerance of absoluteEpsilon.
        /// Line holds the intersection line.
        /// ATTENTION: works only with non-concave polygons!
        /// </summary>
        public static bool IntersectsConvex(
            this Plane3d plane,
            Polygon3d polygon,
            double absoluteEpsilon,
            out Line3d line
            )
        {
            int count = polygon.PointCount;
            int[] signs = new int[count];
            int pc = 0, nc = 0, zc = 0;
            for (int pi = 0; pi < count; pi++)
            {
                double h = plane.Height(polygon[pi]);
                if (h < -absoluteEpsilon) { nc++; signs[pi] = -1; continue; }
                if (h > absoluteEpsilon) { pc++; signs[pi] = 1; continue;  }
                zc++; signs[pi] = 0;
            }

            if (zc == count)
            {
                line = new Line3d(polygon[0], polygon[0]);
                return false;
            }
            else if (pc == 0 && zc == 0)
            {
                line = new Line3d(V3d.NaN, V3d.NaN);
                return false;
            }
            else if (nc == 0 && zc == 0)
            {
                line = new Line3d(V3d.NaN, V3d.NaN);
                return false;
            }
            else
            {
                int pointcount = 0;
                V3d[] linePoints = new V3d[2];
                for (int i = 0; i < count; i++)
                {
                    int u = (i + 1) % count;

                    if (signs[i] != signs[u] || signs[i] == 0 || signs[u] == 0)
                    {
                        if (plane.IntersectsLine(polygon[i], polygon[u], absoluteEpsilon, out V3d point))
                        {
                            linePoints[pointcount++] = point;
                            //If Endpoint is on Plane => Next startpoint is on plane => same intersection point
                            // => skip all following lines which start within absoluteEpsilon (whic have a zero sign)
                            while (signs[(i + 1) % count] == 0) i++;
                        }
                    }
                    if (pointcount == 2)
                    {
                        //
                        line = new Line3d(linePoints[0], linePoints[1]);
                        return true;
                    }
                }
                line = new Line3d(V3d.NaN, V3d.NaN);
                return false;
            }
        }

        #endregion

        #region Plane3d intersects Cylinder3d

        /// <summary>
        /// Returns whether the given sphere and cylinder intersect.
        /// </summary>
        public static bool Intersects(this Plane3d plane, Cylinder3d cylinder)
        {
            if (plane.IsParallelToAxis(cylinder))
            {
                var distance = cylinder.P0.GetMinimalDistanceTo(plane);
                return distance < cylinder.Radius;
            }
            return true;
        }

        /// <summary>
        /// Tests if the given plane is parallel to the cylinder axis (i.e. the plane's normal is orthogonal to the axis).
        /// The plane will intersect the cylinder in two rays or in one tangent line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelToAxis(this Plane3d plane, Cylinder3d cylinder)
            => plane.Normal.IsOrthogonalTo(cylinder.Axis.Direction.Normalized);

        /// <summary>
        /// Tests if the given plane is orthogonal to the cylinder axis (i.e. the plane's normal is parallel to the axis).
        /// The plane will intersect the cylinder in a circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOrthogonalToAxis(this Plane3d plane, Cylinder3d cylinder)
            => plane.Normal.IsParallelTo(cylinder.Axis.Direction.Normalized);

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in an ellipse.
        /// This is only true if the plane is neither orthogonal nor parallel to the cylinder axis. Otherwise the intersection methods returning a circle or rays have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="ellipse"></param>
        public static bool Intersects(this Plane3d plane, Cylinder3d cylinder, out Ellipse3d ellipse)
        {
            if (plane.IsParallelToAxis(cylinder) || plane.IsOrthogonalToAxis(cylinder))
            {
                ellipse = Ellipse3d.Zero;
                return false;
            }

            var dir = cylinder.Axis.Direction.Normalized;
            cylinder.Axis.Ray3d.Intersects(plane, out _, out V3d center);
            var cosTheta = dir.Dot(plane.Normal);

            var eNormal = plane.Normal;
            var eCenter = center;
            var eMajor = (dir - cosTheta * eNormal).Normalized;
            var eMinor = (eNormal.Cross(eMajor)).Normalized;
            eMajor = eNormal.Cross(eMinor).Normalized; //to be sure - if ellipse is nearly a circle
            eMajor = eMajor * cylinder.Radius / cosTheta.Abs();
            eMinor *= cylinder.Radius;
            ellipse = new Ellipse3d(eCenter, eNormal, eMajor, eMinor);
            return true;
        }

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in a circle.
        /// This is only true if the plane is orthogonal to the cylinder axis. Otherwise the intersection methods returning an ellipse or rays have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="circle"></param>
        public static bool Intersects(this Plane3d plane, Cylinder3d cylinder, out Circle3d circle)
        {
            if (plane.IsOrthogonalToAxis(cylinder))
            {
                circle = cylinder.GetCircle(cylinder.GetHeight(plane.Point));
                return true;
            }

            circle = Circle3d.Zero;
            return false;
        }

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in one or two rays.
        /// This is only true if the plane is parallel to the cylinder axis. Otherwise the intersection methods returning an ellipse or a circle have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="rays">Output of intersection rays. The array contains two rays (intersection), one ray (plane is tangent to cylinder) or no ray (no intersection).</param>
        public static bool Intersects(this Plane3d plane, Cylinder3d cylinder, out Ray3d[] rays)
        {
            if (plane.IsParallelToAxis(cylinder))
            {
                var distance = cylinder.P0.GetMinimalDistanceTo(plane);
                var center = cylinder.P0 - distance * plane.Normal;
                var axis = cylinder.Axis.Direction.Normalized;

                if (distance == cylinder.Radius) //one tangent line
                {
                    rays = new[] { new Ray3d(center, axis) };
                    return true;
                }
                else //two intersection lines
                {
                    var offset = axis.Cross(plane.Normal);
                    var extent = (cylinder.Radius.Square() - distance.Square()).Sqrt();
                    rays = new[]
                    {
                        new Ray3d(center - extent * offset, axis),
                        new Ray3d(center + extent * offset, axis)
                    };
                    return true;
                }
            }
            rays = new Ray3d[0];
            return false;
        }

        #endregion


        #region Sphere3d intersects Sphere3d (sm)

        /// <summary>
        /// Returns true if the spheres intersect, or one contains the other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Sphere3d s0, Sphere3d s1)
            => (s0.Center - s1.Center).LengthSquared <= (s0.Radius + s1.Radius).Square();

        #endregion


        #region Box3d intersects Line3d

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool Intersects(this Box3d box, Line3d line)
        {
            var out0 = box.OutsideFlags(line.P0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(line.P1); if (out1 == 0) return true;
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool IntersectsLine(
                this Box3d box, V3d p0, V3d p1)
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            return box.IntersectsLine(p0, p1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied, and have to be already individually tested against
        /// intersection with the box.
        /// </summary>
        public static bool IntersectsLine(
                this Box3d box, V3d p0, V3d p1, Box.Flags out0, Box.Flags out1)
        {
            if ((out0 & out1) != 0) return false;

            V3d min = box.Min;
            V3d max = box.Max;
            var bf = out0 | out1;

            if ((bf & Box.Flags.X) != 0)
            {
                double dx = p1.X - p0.X;
                if ((bf & Box.Flags.MinX) != 0)
                {
                    if (dx == 0 && p0.X < min.X) return false;
                    double t = (min.X - p0.X) / dx;
                    V3d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinX) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxX) != 0)
                {
                    if (dx == 0 && p0.X > max.X) return false;
                    double t = (max.X - p0.X) / dx;
                    V3d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxX) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Y) != 0)
            {
                double dy = p1.Y - p0.Y;
                if ((bf & Box.Flags.MinY) != 0)
                {
                    if (dy == 0 && p0.Y < min.Y) return false;
                    double t = (min.Y - p0.Y) / dy;
                    V3d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinY) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxY) != 0)
                {
                    if (dy == 0 && p0.Y > max.Y) return false;
                    double t = (max.Y - p0.Y) / dy;
                    V3d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxY) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Z) != 0)
            {
                double dz = p1.Z - p0.Z;
                if ((bf & Box.Flags.MinZ) != 0)
                {
                    if (dz == 0 && p0.Z < min.Z) return false;
                    double t = (min.Z - p0.Z) / dz;
                    V3d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinZ) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxZ) != 0)
                {
                    if (dz == 0 && p0.Z > max.Z) return false;
                    double t = (max.Z - p0.Z) / dz;
                    V3d p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxZ) == 0)
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region Box3d intersects Ray3d (haaser)

        public static bool Intersects(this Box3d box, Ray3d ray, out double t)
        {
            Box.Flags out0 = box.OutsideFlags(ray.Origin);

            if (out0 == 0)
            {
                t = 0;
                return true;
            }

            Box3d largeBox = box.EnlargedByRelativeEps(1E-5);
            double tmin = double.PositiveInfinity;
            double ttemp;
            if ((out0 & Box.Flags.X) != 0)
            {
                if (!Fun.IsTiny(ray.Direction.X))
                {
                    if ((out0 & Box.Flags.MinX) != 0)
                    {
                        ttemp = (box.Min.X - ray.Origin.X) / ray.Direction.X;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                    else if ((out0 & Box.Flags.MaxX) != 0)
                    {
                        ttemp = (box.Max.X - ray.Origin.X) / ray.Direction.X;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                }
            }


            if ((out0 & Box.Flags.Y) != 0)
            {
                if (!Fun.IsTiny(ray.Direction.Y))
                {
                    if ((out0 & Box.Flags.MinY) != 0)
                    {
                        ttemp = (box.Min.Y - ray.Origin.Y) / ray.Direction.Y;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                    else if ((out0 & Box.Flags.MaxY) != 0)
                    {
                        ttemp = (box.Max.Y - ray.Origin.Y) / ray.Direction.Y;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                }
            }


            if ((out0 & Box.Flags.Z) != 0)
            {
                if (!Fun.IsTiny(ray.Direction.Z))
                {
                    if ((out0 & Box.Flags.MinZ) != 0)
                    {
                        ttemp = (box.Min.Z - ray.Origin.Z) / ray.Direction.Z;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                    else if ((out0 & Box.Flags.MaxZ) != 0)
                    {
                        ttemp = (box.Max.Z - ray.Origin.Z) / ray.Direction.Z;
                        if (ttemp.Abs() < tmin.Abs() && largeBox.Contains(ray.Origin + ttemp * ray.Direction)) tmin = ttemp;
                    }
                }
            }


            if (tmin < double.PositiveInfinity)
            {
                t = tmin;
                return true;
            }

            t = double.NaN;
            return false;
        }


        #endregion

        #region Box3d intersects Plane3d

        /// <summary>
        /// Returns true if the box and the plane intersect or touch with a
        /// supplied epsilon tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Box3d box, Plane3d plane, double eps)
        {
            var signs = box.GetIntersectionSignsWithPlane(plane, eps);
            return signs != Signs.Negative && signs != Signs.Positive;
        }

        /// <summary>
        /// Classify the position of all the eight vertices of a box with
        /// respect to a supplied plane.
        /// </summary>
        public static Signs GetIntersectionSignsWithPlane(
            this Box3d box, Plane3d plane, double eps)
        {
            var normal = plane.Normal;
            var distance = plane.Distance;

            double npMinX = normal.X * box.Min.X;
            double npMaxX = normal.X * box.Max.X;
            double npMinY = normal.Y * box.Min.Y;
            double npMaxY = normal.Y * box.Max.Y;
            double npMinZ = normal.Z * box.Min.Z;
            double npMaxZ = normal.Z * box.Max.Z;

            double hMinZ = npMinZ - distance;
            double hMaxZ = npMaxZ - distance;

            double hMinYMinZ = npMinY + hMinZ;
            double hMaxYMinZ = npMaxY + hMinZ;
            double hMinYMaxZ = npMinY + hMaxZ;
            double hMaxYMaxZ = npMaxY + hMaxZ;

            return (npMinX + hMinYMinZ).GetSigns(eps)
                 | (npMaxX + hMinYMinZ).GetSigns(eps)
                 | (npMinX + hMaxYMinZ).GetSigns(eps)
                 | (npMaxX + hMaxYMinZ).GetSigns(eps)
                 | (npMinX + hMinYMaxZ).GetSigns(eps)
                 | (npMaxX + hMinYMaxZ).GetSigns(eps)
                 | (npMinX + hMaxYMaxZ).GetSigns(eps)
                 | (npMaxX + hMaxYMaxZ).GetSigns(eps);
        }

        /// <summary>
        /// Returns true if the box intersects the supplied plane. The
        /// bounding boxes of the resulting parts are returned in the out
        /// parameters.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Signs GetIntersectionSigns(
                this Box3d box, Plane3d plane, double eps,
                out Box3d negBox, out Box3d zeroBox, out Box3d posBox)
        {
            return box.GetIntersectionSigns(plane.Normal, plane.Distance, eps,
                                       out negBox, out zeroBox, out posBox);
        }

        /// <summary>
        /// Returns true if the box intersects the supplied plane. The
        /// bounding boxes of the resulting parts are returned in the out
        /// parameters.
        /// </summary>
        public static Signs GetIntersectionSigns(
                this Box3d box, V3d normal, double distance, double eps,
                out Box3d negBox, out Box3d zeroBox, out Box3d posBox)
        {
            double npMinX = normal.X * box.Min.X;
            double npMaxX = normal.X * box.Max.X;
            double npMinY = normal.Y * box.Min.Y;
            double npMaxY = normal.Y * box.Max.Y;
            double npMinZ = normal.Z * box.Min.Z;
            double npMaxZ = normal.Z * box.Max.Z;

            var ha = new double[8];

            double hMinZ = npMinZ - distance;
            double hMaxZ = npMaxZ - distance;

            double hMinYMinZ = npMinY + hMinZ;
            ha[0] = npMinX + hMinYMinZ;
            ha[1] = npMaxX + hMinYMinZ;

            double hMaxYMinZ = npMaxY + hMinZ;
            ha[2] = npMinX + hMaxYMinZ;
            ha[3] = npMaxX + hMaxYMinZ;

            double hMinYMaxZ = npMinY + hMaxZ;
            ha[4] = npMinX + hMinYMaxZ;
            ha[5] = npMaxX + hMinYMaxZ;

            double hMaxYMaxZ = npMaxY + hMaxZ;
            ha[6] = npMinX + hMaxYMaxZ;
            ha[7] = npMaxX + hMaxYMaxZ;

            Signs all = Signs.None;
            var sa = new Signs[8];
            for (int i = 0; i < 8; i++) { sa[i] = ha[i].GetSigns(eps); all |= sa[i]; }

            negBox = Box3d.Invalid;
            zeroBox = Box3d.Invalid;
            posBox = Box3d.Invalid;

            if (all == Signs.Zero) { zeroBox = box; return all; }
            if (all == Signs.Positive) { posBox = box; return all; }
            if (all == Signs.Negative) { negBox = box; return all; }

            var pa = box.ComputeCorners();

            for (int i = 0; i < 8; i++)
            {
                if (sa[i] == Signs.Negative)
                    negBox.ExtendBy(pa[i]);
                else if (sa[i] == Signs.Positive)
                    posBox.ExtendBy(pa[i]);
                else
                {
                    negBox.ExtendBy(pa[i]);
                    zeroBox.ExtendBy(pa[i]);
                    posBox.ExtendBy(pa[i]);
                }
            }

            if (all == Signs.NonPositive) { posBox = Box3d.Invalid; return all; }
            if (all == Signs.NonNegative) { negBox = Box3d.Invalid; return all; }

            for (int ei = 0; ei < 12; ei++)
            {
                int i0 = c_cubeEdgeVertex0[ei], i1 = c_cubeEdgeVertex1[ei];

                if ((sa[i0] == Signs.Negative && sa[i1] == Signs.Positive)
                    || (sa[i0] == Signs.Positive && sa[i1] == Signs.Negative))
                {
                    double h0 = ha[i0];
                    double t = h0 / (h0 - ha[i1]);
                    V3d p0 = pa[i0];
                    V3d sp = p0 + t * (pa[i1] - p0);
                    negBox.ExtendBy(sp);
                    zeroBox.ExtendBy(sp);
                    posBox.ExtendBy(sp);
                }
            }

            return all;
        }

        #endregion

        #region Box3d intersects Sphere3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Box3d box, Sphere3d sphere
             )
        {
            V3d v = sphere.Center.GetClosestPointOn(box) - sphere.Center;
            return sphere.RadiusSquared >= v.LengthSquared;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Box3d box, Cylinder3d cylinder
             )
        {

            return box.Intersects(cylinder.BoundingBox3d);

            //throw new NotImplementedException();
        }

        #endregion

        #region Box3d intersects Triangle3d

        /// <summary>
        /// Returns true if the box and the triangle intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this Box3d box, Triangle3d triangle
             )
        {
            return box.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2);
        }

        /// <summary>
        /// Returns true if the box and the triangle intersect.
        /// </summary>
        public static bool IntersectsTriangle(
             this Box3d box, V3d p0, V3d p1, V3d p2
             )
        {
            /* ---------------------------------------------------------------
               If one of the points of the triangle is inside the box, it
               intersects, of course.
            --------------------------------------------------------------- */
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            var out2 = box.OutsideFlags(p2); if (out2 == 0) return true;
            return box.IntersectsTriangle(p0, p1, p2, out0, out1, out2);
        }


        /// <summary>
        /// Returns true if the box and the triangle intersect. The outside
        /// flags of the triangle vertices with respect to the box have to be
        /// supplied, and already be individually tested for intersection with
        /// the box.
        /// </summary>
        public static bool IntersectsTriangle(
             this Box3d box, V3d p0, V3d p1, V3d p2,
             Box.Flags out0, Box.Flags out1, Box.Flags out2
             )
        {
            /* ---------------------------------------------------------------
                If all of the points of the triangle are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2) != 0) return false;

            /* ---------------------------------------------------------------
                If two points of the triangle are not on the same side
                outside the box, it is possible that the edge between them
                intersects the box. The outside flags we computed are also
                used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(p0, p1, out0, out1)) return true;
            if (box.IntersectsLine(p1, p2, out1, out2)) return true;
            if (box.IntersectsLine(p2, p0, out2, out0)) return true;

            /* ---------------------------------------------------------------
                The only case left: the edges of the triangle go outside
                around the box. Intersect the four space diagonals of the box
                with the triangle to test for intersection.
            --------------------------------------------------------------- */
            Ray3d ray = new Ray3d(box.Min, box.Size);
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;

            ray.Origin.X = box.Max.X;
            ray.Direction.X = -ray.Direction.X;
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;
            ray.Direction.X = -ray.Direction.X;
            ray.Origin.X = box.Min.X;

            ray.Origin.Y = box.Max.Y;
            ray.Direction.Y = -ray.Direction.Y;
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;
            ray.Direction.Y = -ray.Direction.Y;
            ray.Origin.Y = box.Min.Y;

            ray.Origin.Z = box.Max.Z;
            ray.Direction.Z = -ray.Direction.Z;
            if (ray.IntersectsTriangle(p0, p1, p2, 0, 1)) return true;

            return false;
        }

        #endregion

        #region Box3d intersects Quad3d (haaser)

        public static bool Intersects(
            this Box3d box, Quad3d quad
            )
        {
            Box.Flags out0 = box.OutsideFlags(quad.P0); if (out0 == 0) return true;
            Box.Flags out1 = box.OutsideFlags(quad.P1); if (out1 == 0) return true;
            Box.Flags out2 = box.OutsideFlags(quad.P2); if (out2 == 0) return true;
            Box.Flags out3 = box.OutsideFlags(quad.P3); if (out3 == 0) return true;

            return box.IntersectsQuad(quad.P0, quad.P1, quad.P2, quad.P3, out0, out1, out2, out3);
        }

        public static bool IntersectsQuad(
             this Box3d box, V3d p0, V3d p1, V3d p2, V3d p3
             )
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            var out2 = box.OutsideFlags(p2); if (out2 == 0) return true;
            var out3 = box.OutsideFlags(p3); if (out3 == 0) return true;

            return box.IntersectsQuad(p0, p1, p2, p3, out0, out1, out2, out3);
        }

        public static bool IntersectsQuad(
            this Box3d box, V3d p0, V3d p1, V3d p2, V3d p3,
            Box.Flags out0, Box.Flags out1, Box.Flags out2, Box.Flags out3
            )
        {
            /* ---------------------------------------------------------------
                If all of the points of the quad are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            if ((out0 & out1 & out2 & out3) != 0) return false;


            /* ---------------------------------------------------------------
                If two points of the quad are not on the same side
                outside the box, it is possible that the edge between them
                intersects the box. The outside flags we computed are also
                used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            if (box.IntersectsLine(p0, p1, out0, out1)) return true;
            if (box.IntersectsLine(p1, p2, out1, out2)) return true;
            if (box.IntersectsLine(p2, p3, out2, out3)) return true;
            if (box.IntersectsLine(p3, p0, out3, out0)) return true;


            /* ---------------------------------------------------------------
                The only case left: the edges of the quad go outside
                around the box. Intersect the four space diagonals of the box
                with the triangle to test for intersection.
            --------------------------------------------------------------- */
            Ray3d ray = new Ray3d(box.Min, box.Size);
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;

            ray.Origin.X = box.Max.X;
            ray.Direction.X = -ray.Direction.X;
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;
            ray.Direction.X = -ray.Direction.X;
            ray.Origin.X = box.Min.X;

            ray.Origin.Y = box.Max.Y;
            ray.Direction.Y = -ray.Direction.Y;
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;
            ray.Direction.Y = -ray.Direction.Y;
            ray.Origin.Y = box.Min.Y;

            ray.Origin.Z = box.Max.Z;
            ray.Direction.Z = -ray.Direction.Z;
            if (ray.IntersectsQuad(p0, p1, p2, p3, 0, 1)) return true;

            return false;
        }

        #endregion

        #region Box3d intersects Polygon3d (haaser)

        public static bool Intersects(this Box3d box, Polygon3d poly)
        {
            int edges = poly.PointCount;
            Box.Flags[] outside = new Box.Flags[edges];
            for (int i = 0; i < edges; i++)
            {
                outside[i] = box.OutsideFlags(poly[i]); if (outside[i] == 0) return true;
            }

            /* ---------------------------------------------------------------
                If all of the points of the polygon are on the same side
                outside the box, no intersection is possible.
            --------------------------------------------------------------- */
            Box.Flags sum = outside[0];
            for (int i = 1; i < edges; i++) sum &= outside[i];
            if (sum != 0) return false;

            /* ---------------------------------------------------------------
                If two points of the polygon are not on the same side
                outside the box, it is possible that the edge between them
                intersects the box. The outside flags we computed are also
                used to optimize the intersection routine with the edge.
            --------------------------------------------------------------- */
            int u;
            for (int i = 0; i < edges; i++)
            {
                u = (i + 1) % edges;
                if (box.IntersectsLine(poly[i], poly[u], outside[i], outside[u])) return true;
            }

            /* ---------------------------------------------------------------
                The only case left: the edges of the polygon go outside
                around the box. Intersect the four space diagonals of the box
                with the triangle to test for intersection.
                The polygon needs to be triangulated for this check
            --------------------------------------------------------------- */

            int[] tris = poly.ComputeTriangulationOfConcavePolygon(1E-5);

            Ray3d ray = new Ray3d(box.Min, box.Size);
            if (ray.Intersects(poly, tris, 0, 1)) return true;

            ray.Origin.X = box.Max.X;
            ray.Direction.X = -ray.Direction.X;
            if (ray.Intersects(poly, tris, 0, 1)) return true;
            ray.Direction.X = -ray.Direction.X;
            ray.Origin.X = box.Min.X;

            ray.Origin.Y = box.Max.Y;
            ray.Direction.Y = -ray.Direction.Y;
            if (ray.Intersects(poly, tris, 0, 1)) return true;
            ray.Direction.Y = -ray.Direction.Y;
            ray.Origin.Y = box.Min.Y;

            ray.Origin.Z = box.Max.Z;
            ray.Direction.Z = -ray.Direction.Z;
            if (ray.Intersects(poly, tris, 0, 1)) return true;

            return false;
        }

        #endregion

        #region Box3d intersects Projection-Trafo (haaser)

        /// <summary>
        /// returns true if the Box3d and the frustum described by the M44d intersect or the frustum contains the Box3d
        /// Assumes DirectX clip-space:
        ///     -w &lt; x &lt; w
        ///     -w &lt; y &lt; w
        ///      0 &lt; z &lt; w
        /// </summary>
        public static bool IntersectsFrustum(this Box3d box, M44d projection)
        {
            //Let's look at the left clip-plane
            //which corresponds to:
            //-w < x
            //  # which can easily be transformed to:
            //0 < x + w
            //  # for a given vector v this means (* is a dot product here)
            //0 < proj.R0 * v + proj.R3 * v
            //  # or in other words:
            //0 < (proj.R0 + proj.R3) * v

            //therefore (proj.R0 + proj.R3) is the plane describing the left clip-plane.
            //The other planes can be derived in a similar way (only the near plane is a little different)
            //see http://fgiesen.wordpress.com/2012/08/31/frustum-planes-from-the-projection-matrix/ for a full explanation

            var r0 = projection.R0;
            var r1 = projection.R1;
            var r2 = projection.R2;
            var r3 = projection.R3;


            V4d plane;
            V3d n;

            //left
            plane = r3 + r0;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out V3d min, out V3d max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //right
            plane = r3 - r0;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //top
            plane = r3 + r1;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //bottom
            plane = r3 - r1;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //near
            plane = r2;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;

            //far
            plane = r3 - r2;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out min, out max);
            if (min.Dot(n) + plane.W < 0 && max.Dot(n) + plane.W < 0) return false;


            return true;
        }



        #endregion


        #region Hull3d intersects Line3d

        /// <summary>
        /// returns true if the Hull3d and the Line3d intersect or the Hull3d contains the Line3d
        /// [Hull3d-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Hull3d hull,
            Line3d line
            )
        {
            if (hull.Contains(line.P0)) return true;
            if (hull.Contains(line.P1)) return true;

            return hull.Intersects(line.Ray3d, 0, 1, out _);
        }

        /// <summary>
        /// returns true if the Hull3d and the Line between p0 and p1 intersect or the Hull3d contains the Line
        /// [Hull3d-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this Hull3d hull,
            V3d p0, V3d p1
            )
        {
            return hull.Intersects(new Line3d(p0, p1));
        }

        #endregion

        #region Hull3d intersects Ray3d

        /// <summary>
        /// returns true if the Hull3d and the Ray3d intersect
        /// [Hull3d-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Hull3d hull,
            Ray3d ray
            )
        {
            return hull.Intersects(ray, double.NegativeInfinity, double.PositiveInfinity, out _);
        }

        /// <summary>
        /// returns true if the Hull3d and the Ray3d intersect
        /// the out parameter t holds the ray-parameter where an intersection was found
        /// [Hull3d-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this Hull3d hull,
            Ray3d ray,
            out double t
            )
        {
            return hull.Intersects(ray, double.NegativeInfinity, double.PositiveInfinity, out t);
        }

        /// <summary>
        /// returns true if the Hull3d and the Ray3d intersect and the
        /// ray-parameter for the intersection is between t_min and t_max
        /// the out parameter t holds the ray-parameter where an intersection was found
        /// [Hull3d-Normals must point outside]
        /// </summary>
        public static bool Intersects(
            this Hull3d hull,
            Ray3d ray,
            double t_min, double t_max,
            out double t
            )
        {
            if (!double.IsInfinity(t_min) && hull.Contains(ray.GetPointOnRay(t_min))) { t = t_min; return true; }
            if (!double.IsInfinity(t_max) && hull.Contains(ray.GetPointOnRay(t_max))) { t = t_max; return true; }

            var planes = hull.PlaneArray;
            for (int i = 0; i < planes.Length; i++)
            {
                if (!Fun.IsTiny(planes[i].Normal.Dot(ray.Direction)) &&
                    ray.Intersects(planes[i], out double temp_t) &&
                    temp_t >= t_min && temp_t <= t_max)
                {
                    V3d candidatePoint = ray.GetPointOnRay(temp_t);
                    bool contained = true;

                    for (int u = 0; u < planes.Length; u++)
                    {
                        if (u != i && planes[u].Height(candidatePoint) > Constant<double>.PositiveTinyValue)
                        {
                            contained = false;
                            break;
                        }
                    }

                    if (contained)
                    {
                        t = temp_t;
                        return true;
                    }
                }
            }

            t = double.NaN;
            return false;
        }

        #endregion

        #region Hull3d intersects Plane3d

        /// <summary>
        /// returns true if the Hull3d and the Plane3d intersect
        /// [Hull3d-Normals must point outside]
        /// </summary>
        public static bool Intersects(
            this Hull3d hull,
            Plane3d plane
            )
        {
            foreach (var p in hull.PlaneArray)
            {
                if (!p.Normal.IsParallelTo(plane.Normal) && p.Intersects(plane, out Ray3d ray))
                {
                    if (hull.Intersects(ray)) return true;
                }
            }

            return false;
        }

        #endregion

        #region Hull3d intersects Box3d

        /// <summary>
        /// Returns true if the hull and the box intersect.
        /// </summary>
        public static bool Intersects(
            this Hull3d hull, Box3d box
            )
        {
            if (box.IsInvalid) return false;
            bool intersecting = false;
            foreach (Plane3d p in hull.PlaneArray)
            {
                box.GetMinMaxInDirection(p.Normal, out V3d min, out V3d max);
                if (p.Height(min) > 0) return false; // outside
                if (p.Height(max) >= 0) intersecting = true;
            }
            if (intersecting) return true; // intersecting
            return true; // inside
        }

        /// Test hull against intersection of the supplied bounding box.
        /// Note that this is a conservative test, since in some cases
        /// around the edges of the hull it may return true although the
        /// hull does not intersect the box.
        public static bool Intersects(
                this FastHull3d fastHull,
                Box3d box)
        {
            var planes = fastHull.Hull.PlaneArray;
            int count = planes.Length;
            bool intersecting = false;
            for (int pi = 0; pi < count; pi++)
            {
                int minCornerIndex = fastHull.MinCornerIndexArray[pi];
                if (planes[pi].Height(box.Corner(minCornerIndex)) > 0)
                    return false;
                if (planes[pi].Height(box.Corner(minCornerIndex ^ 7)) >= 0)
                    intersecting = true;
            }
            if (intersecting) return true;
            return true;
        }

        #endregion

        #region Hull3d intersects Sphere3d

        /// <summary>
        /// Returns true if the hull and the sphere intersect.
        /// </summary>
        public static bool Intersects(
            this Hull3d hull, Sphere3d sphere
            )
        {
            if (sphere.IsInvalid) return false;
            bool intersecting = false;
            foreach (Plane3d p in hull.PlaneArray)
            {
                double height = p.Height(sphere.Center);
                if (height > sphere.Radius) return false; // outside
                if (height.Abs() < sphere.Radius) intersecting = true;
            }
            if (intersecting) return true; // intersecting
            return true; // inside
        }

        #endregion


        #region Plane3d intersects Box3d

        /// <summary>
        /// Returns true if the box and the plane intersect or touch with a
        /// supplied epsilon tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this Plane3d plane, double eps, Box3d box)
            => box.Intersects(plane, eps);

        #endregion

    }
}

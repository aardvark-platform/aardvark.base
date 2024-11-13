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

        //# foreach (var isDouble in new[] { false, true }) {
        //#   var rtype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var v2t = "V2" + tc;
        //#   var v3t = "V3" + tc;
        //#   var v4t = "V4" + tc;
        //#   var m44t = "M44" + tc;
        //#   var line2t = "Line2" + tc;
        //#   var line3t = "Line3" + tc;
        //#   var plane2t = "Plane2" + tc;
        //#   var plane3t = "Plane3" + tc;
        //#   var ray2t = "Ray2" + tc;
        //#   var ray3t = "Ray3" + tc;
        //#   var circle2t = "Circle2" + tc;
        //#   var circle3t = "Circle3" + tc;
        //#   var quad2t = "Quad2" + tc;
        //#   var quad3t = "Quad3" + tc;
        //#   var range1t = "Range1" + tc;
        //#   var box2t = "Box2" + tc;
        //#   var box3t = "Box3" + tc;
        //#   var hull2t = "Hull2" + tc;
        //#   var hull3t = "Hull3" + tc;
        //#   var sphere3t = "Sphere3" + tc;
        //#   var cylinder3t = "Cylinder3" + tc;
        //#   var ellipse3t = "Ellipse3" + tc;
        //#   var triangle2t = "Triangle2" + tc;
        //#   var triangle3t = "Triangle3" + tc;
        //#   var polygon2t = "Polygon2" + tc;
        //#   var polygon3t = "Polygon3" + tc;
        //#   var euclidean3t = "Euclidean3" + tc;
        //#   var half = isDouble ? "0.5" : "0.5f";
        //#   var eminus5 = isDouble ? "1E-5" : "1E-5f";
        //#   var epsdot = isDouble ? "1E-7" : "1E-4f";
        // Contains-tests should return true if the contained object is
        // either entirely inside the containing object or lies on the
        // boundary of the containing object.

        #region __triangle2t__ contains __v2t__

        public static bool Contains(
            this __triangle2t__ triangle, __v2t__ point
            )
        {
            var v0p = point - triangle.P0;
            return triangle.Line01.LeftValueOfDir(v0p) >= 0
                    && triangle.Line02.RightValueOfDir(v0p) >= 0
                    && triangle.Line12.LeftValueOfPos(point) >= 0;
        }

        #endregion

        #region __triangle2t__ contains __line2t__ (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __triangle2t__ triangle, __line2t__ linesegment)
            => triangle.Contains(linesegment.P0) && triangle.Contains(linesegment.P1);

        #endregion

        #region __triangle2t__ contains __triangle2t__ (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __triangle2t__ triangle, __triangle2t__ other)
            => triangle.Contains(other.P0) && triangle.Contains(other.P1) && triangle.Contains(other.P2);

        #endregion

        #region __triangle2t__ contains __quad2t__ (sm)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __triangle2t__ triangle, __quad2t__ q)
            => triangle.Contains(q.P0) && triangle.Contains(q.P1) && triangle.Contains(q.P2) && triangle.Contains(q.P3);

        #endregion

        #region __triangle2t__ contains __circle2t__ - TODO

        public static bool Contains(this __triangle2t__ triangle, __circle2t__ circle)
            => throw new NotImplementedException();

        #endregion


        #region __circle2t__ contains __v2t__ (sm)

        /// <summary>
        /// True if point p is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __circle2t__ circle, __v2t__ p)
            => (p - circle.Center).LengthSquared <= circle.RadiusSquared;

        #endregion

        #region __circle2t__ contains __line2t__ (sm)

        /// <summary>
        /// True if line segment is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __circle2t__ circle, __line2t__ l)
            => circle.Contains(l.P0) && circle.Contains(l.P1);

        #endregion

        #region __circle2t__ contains __triangle2t__ (sm)

        /// <summary>
        /// True if triangle is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __circle2t__ circle, __triangle2t__ t)
            => circle.Contains(t.P0) && circle.Contains(t.P1) && circle.Contains(t.P2);

        #endregion

        #region __circle2t__ contains __quad2t__ (sm)

        /// <summary>
        /// True if quad is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __circle2t__ circle, __quad2t__ q)
            => circle.Contains(q.P0) && circle.Contains(q.P1) && circle.Contains(q.P2) && circle.Contains(q.P3);

        #endregion

        #region __circle2t__ contains __circle2t__ (sm)

        /// <summary>
        /// True if other circle is contained in this circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __circle2t__ circle, __circle2t__ other)
            => (other.Center - circle.Center).Length + other.Radius <= circle.Radius;

        #endregion


        #region __quad2t__ contains __v2t__ (haaser)

        /// <summary>
        /// True if point is contained in this quad.
        /// </summary>
        public static bool Contains(this __quad2t__ quad, __v2t__ point)
        {
            return LeftValOfPos(0, 1, ref point) >= 0 &&
                   LeftValOfPos(1, 2, ref point) >= 0 &&
                   LeftValOfPos(2, 3, ref point) >= 0 &&
                   LeftValOfPos(3, 0, ref point) >= 0;

            __rtype__ LeftValOfPos(int i0, int i1, ref __v2t__ p)
                => (p.X - quad[i0].X) * (quad[i0].Y - quad[i1].Y) + (p.Y - quad[i0].Y) * (quad[i1].X - quad[i0].X);
        }

        #endregion

        #region __quad2t__ contains __line2t__ - TODO

        /// <summary>
        /// True if line segment is contained in this quad.
        /// </summary>
        public static bool Contains(this __quad2t__ quad, __line2t__ l)
            => throw new NotImplementedException();

        #endregion

        #region __quad2t__ contains __triangle2t__ - TODO

        /// <summary>
        /// True if triangle is contained in this quad.
        /// </summary>
        public static bool Contains(this __quad2t__ quad, __triangle2t__ t)
            => throw new NotImplementedException();

        #endregion

        #region __quad2t__ contains __quad2t__ - TODO

        /// <summary>
        /// True if other quad is contained in this quad.
        /// </summary>
        public static bool Contains(this __quad2t__ quad, __quad2t__ q)
            => throw new NotImplementedException();

        #endregion

        #region __quad2t__ contains __circle2t__ - TODO

        /// <summary>
        /// True if circle is contained in this quad.
        /// </summary>
        public static bool Contains(this __quad2t__ quad, __circle2t__ other)
            => throw new NotImplementedException();

        #endregion


        #region __box3t__ contains __quad3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
            this __box3t__ box, __quad3t__ quad
            )
        {
            return box.Contains(quad.P0)
                    && box.Contains(quad.P1)
                    && box.Contains(quad.P2)
                    && box.Contains(quad.P3);
        }

        #endregion

        #region __box3t__ contains __triangle3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
             this __box3t__ box, __triangle3t__ triangle
            )
        {
            return box.Contains(triangle.P0)
                    && box.Contains(triangle.P1)
                    && box.Contains(triangle.P2);
        }

        #endregion

        #region __box3t__ contains __sphere3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
             this __box3t__ box, __sphere3t__ sphere
            )
        {
            return box.Contains(sphere.Center)
                    && !box.Intersects(sphere);
        }

        #endregion

        #region __box3t__ contains __cylinder3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(
            this __box3t__ box, __cylinder3t__ cylinder
           )
        {
            return box.Contains(cylinder.Center)
                    && !box.Intersects(cylinder);
        }

        #endregion


        #region __hull3t__ contains __v3t__

        /// <summary>
        /// Hull normals are expected to point outside.
        /// </summary>
        public static bool Contains(this __hull3t__ hull, __v3t__ point)
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

        #region __hull3t__ contains __sphere3t__ (sm)

        /// <summary>
        /// Hull normals are expected to point outside.
        /// </summary>
        public static bool Contains(this __hull3t__ hull, __sphere3t__ sphere)
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


        #region __polygon2t__ contains __v2t__ (haaser)

        internal static V3i InsideTriangleFlags(ref __v2t__ p0, ref __v2t__ p1, ref __v2t__ p2, ref __v2t__ point)
        {
            __v2t__ n0 = new __v2t__(p0.Y - p1.Y, p1.X - p0.X);
            __v2t__ n1 = new __v2t__(p1.Y - p2.Y, p2.X - p1.X);
            __v2t__ n2 = new __v2t__(p2.Y - p0.Y, p0.X - p2.X);

            int t0 = Fun.Sign(n0.Dot(point - p0));
            int t1 = Fun.Sign(n1.Dot(point - p1));
            int t2 = Fun.Sign(n2.Dot(point - p2));

            if (t0 == 0) t1 = 1;
            if (t1 == 0) t1 = 1;
            if (t2 == 0) t2 = 1;

            return new V3i(t0, t1, t2);
        }

        internal static V3i InsideTriangleFlags(ref __v2t__ p0, ref __v2t__ p1, ref __v2t__ p2, ref __v2t__ point, int t0)
        {
            __v2t__ n1 = new __v2t__(p1.Y - p2.Y, p2.X - p1.X);
            __v2t__ n2 = new __v2t__(p2.Y - p0.Y, p0.X - p2.X);

            int t1 = Fun.Sign(n1.Dot(point - p1));
            int t2 = Fun.Sign(n2.Dot(point - p2));

            if (t1 == 0) t1 = 1;
            if (t2 == 0) t2 = 1;

            return new V3i(t0, t1, t2);
        }

        /// <summary>
        /// Returns true if the __polygon2t__ contains the given point.
        /// Works with all (convex and non-convex) Polygons.
        /// Assumes that the Vertices of the Polygon are sorted counter clockwise
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __polygon2t__ poly, __v2t__ point)
        {
            return poly.Contains(point, true);
        }

        /// <summary>
        /// Returns true if the __polygon2t__ contains the given point.
        /// Works with all (convex and non-convex) Polygons.
        /// CCW represents the sorting order of the Polygon-Vertices (true -> CCW, false -> CW)
        /// </summary>
        public static bool Contains(this __polygon2t__ poly, __v2t__ point, bool CCW)
        {
            int pc = poly.PointCount;
            if (pc < 3)
                return false;
            int counter = 0;
            __v2t__ p0 = poly[0], p1 = poly[1], p2 = poly[2];
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


        #region __plane3t__ +- eps contains __v3t__ (sm)

        /// <summary>
        /// Returns true if point is within given eps to plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __plane3t__ plane, __rtype__ eps, __v3t__ point)
        {
            var d = plane.Height(point);
            return d >= -eps && d <= eps;
        }

        #endregion

        #region __plane3t__ +- eps contains __box3t__ (sm)

        /// <summary>
        /// Returns true if the space within eps to a plane fully contains the given box.
        /// </summary>
        public static bool Contains(this __plane3t__ plane, __rtype__ eps, __box3t__ box)
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

        #region __polygon3t__ +- eps contains __v3t__ (sm)

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __polygon3t__ polygon, __rtype__ eps, __v3t__ point, out __rtype__ distance)
        {
            var plane = polygon.Get__plane3t__();
            distance = plane.Height(point);
            if (distance < -eps || distance > eps) return false;
            var w2p = plane.GetWorldToPlane();
            var poly2d = new __polygon2t__(polygon.GetPointArray().Map(p => w2p.TransformPos(p).XY));
            return poly2d.Contains(w2p.TransformPos(point).XY);
        }

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __polygon3t__ polygon, __plane3t__ supportingPlane, __euclidean3t__ world2plane, __polygon2t__ poly2d, __rtype__ eps, __v3t__ point, out __rtype__ distance)
        {
            distance = supportingPlane.Height(point);
            if (distance < -eps || distance > eps) return false;
            return poly2d.Contains(world2plane.TransformPos(point).XY);
        }

        /// <summary>
        /// Returns true if point is within given eps to polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this __polygon3t__ polygon, __plane3t__ supportingPlane, __m44t__ world2plane, __polygon2t__ poly2d, __rtype__ eps, __v3t__ point, out __rtype__ distance)
        {
            distance = supportingPlane.Height(point);
            if (distance < -eps || distance > eps) return false;
            return poly2d.Contains(world2plane.TransformPos(point).XY);
        }

        #endregion


        // Intersection tests

        #region __line2t__ intersects __line2t__

        /// <summary>
        /// Returns true if the two line segments intersect.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __line2t__ l0, __line2t__ l1)
            => l0.IntersectsLine(l1.P0, l1.P1, out __v2t__ _);

        /// <summary>
        /// Returns true if the two line segments intersect.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __line2t__ l0, __line2t__ l1, out __v2t__ p)
            => l0.IntersectsLine(l1.P0, l1.P1, out p);

        /// <summary>
        /// Returns true if the two line segments intersect within given tolerance.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __line2t__ l0, __line2t__ l1, __rtype__ absoluteEpsilon, out __v2t__ p)
            => l0.IntersectsLine(l1.P0, l1.P1, absoluteEpsilon, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this __line2t__ line, __v2t__ p0, __v2t__ p1)
            => line.IntersectsLine(p0, p1, out _);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this __line2t__ line, __v2t__ p0, __v2t__ p1, out __v2t__ p)
            => line.IntersectsLine(p0, p1, false, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1.
        /// The intersection point is returned in p.
        /// If overlapping is true and the segments are parallel, then
        /// the closest point to p0 of the intersection range is returned in p.
        /// </summary>
        public static bool IntersectsLine(
            this __line2t__ line,
            __v2t__ p0, __v2t__ p1,
            bool overlapping,
            out __v2t__ p
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
                    p = __v2t__.NaN;
                    return false;
                }

                __rtype__ t1 = (a.Y * u.X - a.X * u.Y) * cross;
                if (t1 > 1 || t1 < 0)
                {
                    p = __v2t__.NaN;
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
                    p = __v2t__.NaN;
                    return false;
                }

                var normalizedDirection = u / lu;

                var r0 = new __range1t__(0, lu);
                var r1 = new __range1t__((p0 - line.P0).Dot(normalizedDirection), (p1 - line.P0).Dot(normalizedDirection));
                r1.Repair();

                if (r0.Intersects(r1, 0, out __range1t__ result))
                {
                    p = line.P0 + normalizedDirection * result.Min;
                    return true;
                }

                p = __v2t__.NaN;
                return false;
            }
        }

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1 within given tolerance.
        /// The intersection point is returned in p.
        /// If the segments are parallel and overlap, then no intersection is returned.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(this __line2t__ line, __v2t__ p0, __v2t__ p1, __rtype__ absoluteEpsilon, out __v2t__ p)
            => line.IntersectsLine(p0, p1, absoluteEpsilon, false, out p);

        /// <summary>
        /// Returns true if the line segment intersects the line segment between p0 and p1 within given tolerance.
        /// The intersection point is returned in p.
        /// If overlapping is true and the segments are parallel, then
        /// the closest point to p0 of the intersection range is returned in p.
        /// </summary>
        public static bool IntersectsLine(
            this __line2t__ line,
            __v2t__ p0, __v2t__ p1,
            __rtype__ absoluteEpsilon,
            bool overlapping,
            out __v2t__ p
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
                    p = __v2t__.NaN;
                    return false;
                }

                var t1 = (a.Y * u.X - a.X * u.Y) * cross;
                if (t1 > 1 + RelativeEpsilonV || t1 < -RelativeEpsilonV)
                {
                    p = __v2t__.NaN;
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
                    p = __v2t__.NaN;
                    return false;
                }

                var normalizedDirection = u / lu;

                var r0 = new __range1t__(0, lu);
                var r1 = new __range1t__((p0 - line.P0).Dot(normalizedDirection), (p1 - line.P0).Dot(normalizedDirection));
                r1.Repair();

                if (r0.Intersects(r1, absoluteEpsilon, out __range1t__ result))
                {
                    p = line.P0 + normalizedDirection * result.Min;
                    return true;
                }

                p = __v2t__.NaN;
                return false;
            }
        }

        #endregion

        #region __ray2t__ intersects __line2t__

        /// <summary>
        /// Returns true if the Ray and the Line intersect.
        /// ATTENTION: Both-Sided Ray
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __ray2t__ ray,
            __line2t__ line
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
            this __ray2t__ ray,
            __line2t__ line,
            out __rtype__ t
            )
        {
            return ray.IntersectsLine(line.P0, line.P1, out t);
        }


        /// <summary>
        /// returns true if the Ray and the Line(p0,p1) intersect.
        /// ATTENTION: Both-Sided Ray
        /// </summary>
        public static bool IntersectsLine(
            this __ray2t__ ray,
            __v2t__ p0, __v2t__ p1
            )
        {
            __v2t__ n = new __v2t__(-ray.Direction.Y, ray.Direction.X);

            __rtype__ d0 = n.Dot(p0 - ray.Origin);
            __rtype__ d1 = n.Dot(p1 - ray.Origin);

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
            this __ray2t__ ray,
            __v2t__ p0, __v2t__ p1,
            out __rtype__ t
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
            this __ray2t__ ray,
            __v2t__ p0, __v2t__ p1,
            bool overlapping,
            out __rtype__ t
            )
        {
            __v2t__ a = p0 - ray.Origin;
            __v2t__ u = p1 - p0;
            __v2t__ v = ray.Direction;
            __rtype__ lv2 = v.LengthSquared;


            __rtype__ cross = u.X * v.Y - u.Y * v.X;
            __rtype__ n = a.Y * u.X - a.X * u.Y;

            if (!Fun.IsTiny(cross))
            {
                cross = 1 / cross;

                __rtype__ t0 = (a.Y * v.X - a.X * v.Y) * cross;
                if (t0 >= 0 && t0 <= 1)
                {
                    t = n * cross;
                    return true;
                }

                t = __rtype__.NaN;
                return false;
            }

            if (Fun.IsTiny(n) && overlapping)
            {
                __rtype__ ta = v.Dot(a) / lv2;
                __rtype__ tb = v.Dot(p1 - ray.Origin) / lv2;

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

            t = __rtype__.NaN;
            return false;
        }



        #endregion

        #region __ray2t__ intersects __ray2t__

        /// <summary>
        /// Returns true if the Rays intersect
        /// ATTENTION: Both-Sided Rays
        /// </summary>
        public static bool Intersects(
            this __ray2t__ r0,
            __ray2t__ r1
            )
        {
            if (!r0.Direction.IsParallelTo(r1.Direction)) return true;
            else
            {
                __v2t__ n0 = new __v2t__(-r0.Direction.Y, r0.Direction.X);

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
            this __ray2t__ r0, __ray2t__ r1,
            out __rtype__ t0, out __rtype__ t1
            )
        {
            __v2t__ a = r0.Origin - r1.Origin;

            if (r0.Origin.ApproximateEquals(r1.Origin, Constant<__rtype__>.PositiveTinyValue))
            {
                t0 = 0;
                t1 = 0;
                return true;
            }

            __v2t__ u = r0.Direction;
            __v2t__ v = r1.Direction;

            __rtype__ cross = u.X * v.Y - u.Y * v.X;

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
                t0 = __rtype__.NaN;
                t1 = __rtype__.NaN;
                //Rays are parallel
                if (Fun.IsTiny(a.Y * u.X - a.X * u.Y)) return true;
                else return false;
            }
        }

        /// <summary>
        /// Returns true if the Rays intersect.
        /// </summary>
        public static bool Intersects(this __ray2t__ r0, __ray2t__ r1, out __rtype__ t)
        {
            __v2t__ a = r1.Origin - r0.Origin;
            if (a.Abs().AllSmaller(Constant<__rtype__>.PositiveTinyValue))
            {
                t = 0;
                return true; // Early exit when rays have same origin
            }

            __rtype__ cross = r0.Direction.Dot270(r1.Direction);
            if (!Fun.IsTiny(cross)) // Rays not parallel
            {
                t = r1.Direction.Dot90(a) / cross;
                return true;
            }
            else // Rays are parallel
            {
                t = __rtype__.NaN;
                return false;
            }
        }

        #endregion

        #region __ray2t__ intersects __circle2t__

        /// <summary>
        /// Computes the intersection between the given <see cref="__ray2t__"/> and <see cref="__circle2t__"/>.
        /// </summary>
        /// <param name="ray">The ray to intersect.</param>
        /// <param name="circle">The circle to intersect the ray with.</param>
        /// <param name="t0">The ray parameter of the first intersection. Set to infinity if there is no intersection.</param>
        /// <param name="t1">The ray parameter of the second intersection. Set to infinity if there is no intersection.</param>
        /// <returns>True if there is an intersection, false otherwise.</returns>
        public static bool Intersects(this __ray2t__ ray, __circle2t__ circle, out __rtype__ t0, out __rtype__ t1)
        {
            var p = ray.Origin - circle.Center;
            var a = ray.Direction.X.Square() + ray.Direction.Y.Square();
            var b = 2 * ray.Direction.X * p.X + 2 * ray.Direction.Y * p.Y;
            var c = p.X.Square() + p.Y.Square() - circle.Radius.Square();
            var d = b.Square() - 4 * a * c;

            if (d < 0)
            {
                t0 = __rtype__.PositiveInfinity;
                t1 = __rtype__.PositiveInfinity;
                return false;
            }
            else
            {
                var div = 1 / (a + a);
                d = d.Sqrt();
                t0 = (-b + d) * div;
                t1 = (-b - d) * div;
                return true;
            }
        }

        #endregion


        #region __plane2t__ intersects __line2t__

        /// <summary>
        /// Returns true if the __plane2t__ and the __line2t__ intersect or the __line2t__
        /// lies completely in the Plane's Epsilon-Range
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        public static bool Intersects(
            this __plane2t__ plane,
            __line2t__ line,
            __rtype__ absoluteEpsilon
            )
        {
            __rtype__ lengthOfNormal2 = plane.Normal.LengthSquared;
            __rtype__ d0 = plane.Height(line.P0);
            __rtype__ d1 = plane.Height(line.P1);

            return d0 * d1 < absoluteEpsilon * absoluteEpsilon * lengthOfNormal2;
        }

        /// <summary>
        /// Returns true if the __plane2t__ and the __line2t__ intersect
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __plane2t__ plane,
            __line2t__ line
            )
        {
            return plane.IntersectsLine(line.P0, line.P1);
        }


        /// <summary>
        /// Returns true if the __plane2t__ and the line between p0 and p1 intersect
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this __plane2t__ plane,
            __v2t__ p0, __v2t__ p1
            )
        {
            __rtype__ d0 = plane.Height(p0);
            __rtype__ d1 = plane.Height(p1);

            return d0 * d1 <= 0;
        }

        /// <summary>
        /// Returns true if the __plane2t__ and the __line2t__ intersect
        /// point holds the Intersection-Point. If no Intersection is found point is __v2t__.NaN
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __plane2t__ plane,
            __line2t__ line,
            out __v2t__ point
            )
        {
            return plane.Intersects(line, 0, out point);
        }

        /// <summary>
        /// Returns true if the __plane2t__ and the __line2t__ intersect or the __line2t__
        /// lies completely in the Plane's Epsilon-Range
        /// point holds the Intersection-Point. If the __line2t__ is inside Epsilon point holds the centroid of the __line2t__
        /// If no Intersection is found point is __v2t__.NaN
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        public static bool Intersects(
            this __plane2t__ plane,
            __line2t__ line,
            __rtype__ absoluteEpsilon,
            out __v2t__ point
            )
        {
            __rtype__ h0 = plane.Height(line.P0);
            __rtype__ h1 = plane.Height(line.P1);

            int s0 = (h0 > -absoluteEpsilon ? (h0 < absoluteEpsilon ? 0 : 1) : -1);
            int s1 = (h1 > -absoluteEpsilon ? (h1 < absoluteEpsilon ? 0 : 1) : -1);

            if (s0 == s1)
            {
                if (s0 != 0)
                {
                    point = __v2t__.NaN;
                    return false;
                }
                else
                {
                    point = (line.P0 + line.P1) * __half__;
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
                    __v2t__ dir = line.Direction;
                    __rtype__ no = plane.Normal.Dot(line.P0);
                    __rtype__ nd = plane.Normal.Dot(dir);
                    __rtype__ t = (plane.Distance - no) / nd;

                    point = line.P0 + t * dir;
                    return true;
                }
            }
        }



        #endregion

        #region __plane2t__ intersects __ray2t__

        /// <summary>
        /// Returns true if the __plane2t__ and the __ray2t__ intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane2t__ plane, __ray2t__ ray)
            => !plane.Normal.IsOrthogonalTo(ray.Direction);

        /// <summary>
        /// Returns true if the Plane2d and the Ray2d intersect.
        /// point holds the Intersection-Point if an intersection is found (else V2d.NaN)
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane2t__ plane, __ray2t__ ray, out __v2t__ point)
            => plane.Intersects(ray, out __rtype__ t, out point);

        /// <summary>
        /// Returns true if the __plane2t__ and the __ray2t__ intersect.
        /// t holds the ray paramater of the intersection point if the intersection is found (else Double.PositiveInfinity).
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane2t__ plane, __ray2t__ ray, out __rtype__ t)
        {
            __rtype__ dot = Vec.Dot(plane.Normal, ray.Direction);
            if (Fun.IsTiny(dot))
            {
                t = __rtype__.PositiveInfinity;
                return false;
            }
            t = -plane.Height(ray.Origin) / dot;
            return true;
        }

        /// <summary>
        /// Returns true if the __plane2t__ and the __ray2t__ intersect.
        /// t and p hold the ray paramater and point of the intersection if one is found (else Double.PositiveInfinity and __v2t__.PositiveInfinity).
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane2t__ plane, __ray2t__ ray, out __rtype__ t, out __v2t__ p)
        {
            bool result = Intersects(plane, ray, out t);
            p = ray.Origin + t * ray.Direction;
            return result;
        }

        /// <summary>
        /// Returns the intersection point of the given __plane2t__ and __ray2t__, or __v2t__.PositiveInfinity if ray is parallel to plane.
        /// ATTENTION: Works only with Normalized Plane2ds
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ Intersect(this __plane2t__ plane, __ray2t__ ray)
        {
            __rtype__ dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot)) return __v2t__.PositiveInfinity;
            return ray.GetPointOnRay(-plane.Height(ray.Origin) / dot);
        }

        #endregion

        #region __plane2t__ intersects __plane2t__

        /// <summary>
        /// Returns true if the two Plane2ds intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane2t__ p0, __plane2t__ p1)
        {
            var hit = p0.Coefficients.Cross(p1.Coefficients);
            return !hit.Z.IsTiny();
        }

        /// <summary>
        /// Returns true if the two Plane2ds intersect.
        /// Point holds the intersection point if an intersection is found.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane2t__ p0, __plane2t__ p1, out __v2t__ point)
        {
            var hit = p0.Coefficients.Cross(p1.Coefficients);
            point = hit.XY / hit.Z;

            return !hit.Z.IsTiny();
        }

        #endregion

        #region __plane2t__ intersects IEnumerable<__v2t__>

        /// <summary>
        /// returns true if the __plane2t__ divides the Point-Cloud
        /// </summary>
        public static bool Divides(
            this __plane2t__ plane,
            IEnumerable<__v2t__> data
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


        #region __circle2t__ intersects __circle2t__ (sm)

        /// <summary>
        /// Returns true if the circles intersect, or one contains the other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __circle2t__ c0, __circle2t__ c1)
            => (c0.Center - c1.Center).LengthSquared <= (c0.Radius + c1.Radius).Square();

        #endregion


        #region __triangle2t__ intersects __line2t__

        /// <summary>
        /// Returns true if the triangle and the line intersect or the triangle contains the line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __triangle2t__ triangle,
            __line2t__ line
            )
        {
            return triangle.IntersectsLine(line.P0, line.P1);
        }

        /// <summary>
        /// Returns true if the triangle and the line between p0 and p1 intersect or the triangle contains the line
        /// </summary>
        public static bool IntersectsLine(
            this __triangle2t__ triangle,
            __v2t__ p0, __v2t__ p1
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

        #region __triangle2t__ intersects __ray2t__

        /// <summary>
        /// Returns true if the Triangle and the Ray intersect
        /// </summary>
        public static bool Intersects(
            this __triangle2t__ triangle,
            __ray2t__ ray
            )
        {
            if (triangle.Contains(ray.Origin)) return true;

            if (ray.IntersectsLine(triangle.P0, triangle.P1)) return true;
            if (ray.IntersectsLine(triangle.P1, triangle.P2)) return true;
            if (ray.IntersectsLine(triangle.P2, triangle.P0)) return true;

            return false;
        }

        #endregion

        #region __triangle2t__ intersects __plane2t__

        /// <summary>
        /// returns true if the __triangle2t__ and the __plane2t__ intersect
        /// </summary>
        public static bool Intersects(
            this __triangle2t__ triangle,
            __plane2t__ plane
            )
        {
            if (plane.Intersects(triangle.Line01)) return true;
            if (plane.Intersects(triangle.Line12)) return true;
            if (plane.Intersects(triangle.Line20)) return true;

            return false;
        }

        #endregion

        #region __triangle2t__ intersects __triangle2t__

        /// <summary>
        /// Returns true if the triangles intersect or one contains the other.
        /// </summary>
        public static bool Intersects(this __triangle2t__ t0, __triangle2t__ t1)
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


        #region __box2t__ intersects __line2t__

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool Intersects(this __box2t__ box, __line2t__ line)
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
                this __box2t__ box, __line2t__ line, Box.Flags out0, Box.Flags out1)
        {
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect. The outside flags
        /// of the end points of the line with respect to the box have to be
        /// supplied as parameters.
        /// </summary>
        private static bool IntersectsLine(
                this __box2t__ box, __v2t__ p0, __v2t__ p1)
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
                this __box2t__ box, __v2t__ p0, __v2t__ p1,
                Box.Flags out0, Box.Flags out1)
        {
            if ((out0 & out1) != 0) return false;

            __v2t__ min = box.Min;
            __v2t__ max = box.Max;
            var bf = out0 | out1;

            if ((bf & Box.Flags.X) != 0)
            {
                __rtype__ dx = p1.X - p0.X;
                if ((bf & Box.Flags.MinX) != 0)
                {
                    if (dx == 0 && p0.X < min.X) return false;
                    __rtype__ t = (min.X - p0.X) / dx;
                    __v2t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinX) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxX) != 0)
                {
                    if (dx == 0 && p0.X > max.X) return false;
                    __rtype__ t = (max.X - p0.X) / dx;
                    __v2t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxX) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Y) != 0)
            {
                __rtype__ dy = p1.Y - p0.Y;
                if ((bf & Box.Flags.MinY) != 0)
                {
                    if (dy == 0 && p0.Y < min.Y) return false;
                    __rtype__ t = (min.Y - p0.Y) / dy;
                    __v2t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinY) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxY) != 0)
                {
                    if (dy == 0 && p0.Y > max.Y) return false;
                    __rtype__ t = (max.Y - p0.Y) / dy;
                    __v2t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxY) == 0)
                        return true;
                }
            }
            return false;
        }


        #endregion

        #region __box2t__ intersects __ray2t__

        /// <summary>
        /// Returns true if the box and the ray intersect
        /// </summary>
        public static bool Intersects(
            this __box2t__ box,
            __ray2t__ ray
            )
        {
            /*
             * Getting a Normal-Vector for the Ray and calculating
             * the Normal Distances for every Box-Point:
             */
            __v2t__ n = new __v2t__(-ray.Direction.Y, ray.Direction.X);

            __rtype__ d0 = n.Dot(box.Min - ray.Origin);                                            //n.Dot(box.p0 - ray.Origin)
            __rtype__ d1 = n.X * (box.Max.X - ray.Origin.X) + n.Y * (box.Min.Y - ray.Origin.Y);    //n.Dot(box.p1 - ray.Origin)
            __rtype__ d2 = n.Dot(box.Max - ray.Origin);                                            //n.Dot(box.p2 - ray.Origin)
            __rtype__ d3 = n.X * (box.Min.X - ray.Origin.X) + n.Y * (box.Max.Y - ray.Origin.Y);    //n.Dot(box.p3 - ray.Origin)

            /*
             * If Zero lies in the Range of the Distances there
             * have to be Points on both sides of the Ray.
             * This means the Box and the Ray have an Intersection
             */

            __range1t__ r = new __range1t__(d0, d1, d2, d3);
            return r.Contains(0);
        }

        #endregion

        #region __box2t__ intersects __plane2t__

        /// <summary>
        /// returns true if the box and the plane intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __box2t__ box,
            __plane2t__ plane
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
            this __box2t__ box,
            __plane2t__ plane,
            out __line2t__ line)
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
                __rtype__ nx, __rtype__ ny, __rtype__ d,
                __rtype__ xmin, __rtype__ ymin, __rtype__ xmax, __rtype__ ymax,
                out __line2t__ line)
        {
            if (nx.IsTiny()) // horizontal
            {
                if (d <= ymin || d >= ymax) { line = default; return false; }
                line = new __line2t__(new __v2t__(xmin, d), new __v2t__(xmax, d));
                return true;
            }

            if (ny.IsTiny()) // vertical
            {
                if (d <= xmin || d >= xmax) { line = default; return false; }
                line = new __line2t__(new __v2t__(d, ymin), new __v2t__(d, ymax));
                return true;
            }

            if (nx.Sign() != ny.Sign())
            {
                __rtype__ x0 = (d - ny * ymin) / nx;
                if (x0 >= xmax) { line = default; return false; }
                if (x0 > xmin) xmin = x0;
                __rtype__ x1 = (d - ny * ymax) / nx;
                if (x1 <= xmin) { line = default; return false; }
                if (x1 < xmax) xmax = x1;

                __rtype__ y0 = (d - nx * xmin) / ny;
                if (y0 >= ymax) { line = default; return false; }
                if (y0 > ymin) ymin = y0;
                __rtype__ y1 = (d - nx * xmax) / ny;
                if (y1 <= ymin) { line = default; return false; }
                if (y1 < ymax) ymax = y1;

                line = new __line2t__(new __v2t__(xmin, ymin), new __v2t__(xmax, ymax));
            }
            else
            {
                __rtype__ x0 = (d - ny * ymax) / nx;
                if (x0 >= xmax) { line = default; return false; }
                if (x0 > xmin) xmin = x0;
                __rtype__ x1 = (d - ny * ymin) / nx;
                if (x1 <= xmin) { line = default; return false; }
                if (x1 < xmax) xmax = x1;
                __rtype__ y0 = (d - nx * xmax) / ny;
                if (y0 >= ymax) { line = default; return false; }
                if (y0 > ymin) ymin = y0;
                __rtype__ y1 = (d - nx * xmin) / ny;
                if (y1 <= ymin) { line = default; return false; }
                if (y1 < ymax) ymax = y1;

                line = new __line2t__(new __v2t__(xmax, ymin), new __v2t__(xmin, ymax));
            }
            return true;
        }

        #endregion

        #region __box2t__ intersects __triangle2t__

        /// <summary>
        /// Returns true if the Box and the Triangle intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __box2t__ box,
            __triangle2t__ triangle
            )
        {
            return box.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2);
        }

        /// <summary>
        /// Returns true if the Box and the Triangle intersect
        /// </summary>
        public static bool IntersectsTriangle(
            this __box2t__ box,
            __v2t__ p0, __v2t__ p1, __v2t__ p2
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
            this __box2t__ box, __v2t__ p0, __v2t__ p1, __v2t__ p2,
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
            __v2t__ a = box.Min - p0;
            __v2t__ u = p1 - p0;
            __v2t__ v = p2 - p0;


            __rtype__ cross = u.X * v.Y - u.Y * v.X;
            if (Fun.IsTiny(cross)) return false;
            cross = 1 / cross;

            __rtype__ t0 = (a.Y * v.X - a.X * v.Y) * cross; if (t0 < 0 || t0 > 1) return false;
            __rtype__ t1 = (a.Y * u.X - a.X * u.Y) * cross; if (t1 < 0 || t1 > 1) return false;

            return (t0 + t1 < 1);
        }

        #endregion

        #region __box2t__ intersects __box2t__ (__box2t__-Implementation)

        //Directly in Box-Implementation

        #endregion


        #region __quad2t__ intersects __line2t__

        /// <summary>
        /// returns true if the Quad and the line intersect or the quad contains the line
        /// </summary>
        public static bool Intersects(
            this __quad2t__ quad,
            __line2t__ line
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
            this __quad2t__ quad,
            __v2t__ p0, __v2t__ p1
            )
        {
            if (quad.Contains(p0)) return true;
            if (quad.Contains(p1)) return true;

            __line2t__ line = new __line2t__(p0, p1);
            if (line.IntersectsLine(quad.P0, quad.P1)) return true;
            if (line.IntersectsLine(quad.P1, quad.P2)) return true;
            if (line.IntersectsLine(quad.P2, quad.P3)) return true;
            if (line.IntersectsLine(quad.P3, quad.P0)) return true;

            return false;
        }

        #endregion

        #region __quad2t__ intersects __ray2t__

        /// <summary>
        /// returns true if the quad and the ray intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __quad2t__ quad,
            __ray2t__ ray
            )
        {
            return ray.__plane2t__.Divides(quad.Points);
        }

        #endregion

        #region __quad2t__ intersects __plane2t__

        /// <summary>
        /// returns true if the __quad2t__ and the __plane2t__ intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __quad2t__ quad,
            __plane2t__ plane
            )
        {
            //UNTESTED
            if (plane.Divides(quad.Points)) return true;
            else return false;
        }

        #endregion

        #region __quad2t__ intersects __triangle2t__

        /// <summary>
        /// returns true if the __quad2t__ and the __triangle2t__ intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this __quad2t__ quad,
            __triangle2t__ triangle
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

        #region __quad2t__ intersects __box2t__

        /// <summary>
        /// Returns true if the box and the Quad intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this __box2t__ box,
            __quad2t__ quad
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

        #region __quad2t__ intersects __quad2t__

        /// <summary>
        /// returns true if the Quad2ds intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this __quad2t__ q0,
            __quad2t__ quad
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


        #region __polygon2t__ intersects __line2t__

        /// <summary>
        /// returns true if the __polygon2t__ and the __line2t__ intersect or the Polygon contains the Line
        /// </summary>
        public static bool Intersects(
            this __polygon2t__ poly,
            __line2t__ line
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

        #region __polygon2t__ intersects __ray2t__

        /// <summary>
        /// returns true if the __polygon2t__ and the __ray2t__ intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __polygon2t__ poly,
            __ray2t__ ray
            )
        {
            //UNTESTED
            return ray.__plane2t__.Divides(poly.Points);
        }


        #endregion

        #region __polygon2t__ intersects __plane2t__

        /// <summary>
        /// returns true if the __polygon2t__ and the __plane2t__ itnersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __polygon2t__ poly,
            __plane2t__ plane
            )
        {
            //UNTESTED
            return plane.Divides(poly.Points);
        }

        #endregion

        #region __polygon2t__ intersects __triangle2t__

        /// <summary>
        /// returns true if the __polygon2t__ and the __triangle2t__ intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this __polygon2t__ poly,
            __triangle2t__ triangle
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

        #region __polygon2t__ intersects __box2t__

        /// <summary>
        /// returns true if the __polygon2t__ and the __box2t__ intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this __polygon2t__ poly,
            __box2t__ box
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

        #region __polygon2t__ intersects __quad2t__

        /// <summary>
        /// returns true if the __polygon2t__ and the __quad2t__ interset or one contains the other
        /// </summary>
        public static bool Intersects(
            this __polygon2t__ poly,
            __quad2t__ quad
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

        #region __polygon2t__ intersects __polygon2t__

        /// <summary>
        /// returns true if the Polygon2ds intersect or one contains the other
        /// </summary>
        public static bool Intersects(
            this __polygon2t__ poly0,
            __polygon2t__ poly1
            )
        {
            //check if projected ranges intersect for all possible normals


            __v2t__[] allnormals = new __v2t__[poly0.PointCount + poly1.PointCount];
            int c = 0;

            foreach (var d in poly0.Edges)
            {
                allnormals[c] = new __v2t__(-d.Y, d.X);
                c++;
            }
            foreach (var d in poly1.Edges)
            {
                allnormals[c] = new __v2t__(-d.Y, d.X);
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

        private static __range1t__ ProjectTo(this __polygon2t__ poly, __v2t__ dir)
        {
            __rtype__ min = __rtype__.MaxValue;
            __rtype__ max = __rtype__.MinValue;
            foreach (var p in poly.Points)
            {
                __rtype__ dotproduct = p.Dot(dir);

                if (dotproduct < min) min = dotproduct;
                if (dotproduct > max) max = dotproduct;
            }

            return new __range1t__(min, max);
        }

        #endregion


        #region __line3t__ intersects __line3t__ (haaser)

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than Constant&lt;__rtype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __line3t__ l0,
            __line3t__ l1
            )
        {
            return l0.Intersects(l1, Constant<__rtype__>.PositiveTinyValue);
        }

        /// <summary>
        /// Returns true if the minimal distance between the given lines is smaller than absoluteEpsilon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __line3t__ l0,
            __line3t__ l1,
            __rtype__ absoluteEpsilon
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
            this __line3t__ l0,
            __line3t__ l1,
            __rtype__ absoluteEpsilon,
            out __v3t__ point
            )
        {
            if (l0.GetMinimalDistanceTo(l1, out point) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region __line3t__ intersects Special (inconsistent argument order)

        #region __line3t__ intersects __plane3t__

        /// <summary>
        /// Returns true if the line and the plane intersect.
        /// </summary>
        public static bool Intersects(
             this __line3t__ line, __plane3t__ plane, out __rtype__ t
             )
        {
            if (!line.__ray3t__.Intersects(plane, out t)) return false;
            if (t >= 0 && t <= 1) return true;
            t = __rtype__.PositiveInfinity;
            return false;

        }

        /// <summary>
        /// Returns true if the line and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __line3t__ line, __plane3t__ plane, out __rtype__ t, out __v3t__ p
             )
        {
            bool result = line.Intersects(plane, out t);
            p = line.Origin + t * line.Direction;
            return result;
        }

        #endregion

        #region __line3t__ intersects __triangle3t__

        /// <summary>
        /// Returns true if the line and the triangle intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __line3t__ line,
            __triangle3t__ triangle
            )
        {
            return line.__ray3t__.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2, 0, 1, out _);
        }

        /// <summary>
        /// Returns true if the line and the triangle intersect.
        /// point holds the intersection point.
        /// </summary>
        public static bool Intersects(
            this __line3t__ line,
            __triangle3t__ triangle,
            out __v3t__ point
            )
        {
            __ray3t__ ray = line.__ray3t__;

            if (ray.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2, 0, 1, out __rtype__ temp))
            {
                point = ray.GetPointOnRay(temp);
                return true;
            }
            else
            {
                point = __v3t__.NaN;
                return false;
            }
        }

        #endregion

        #endregion


        #region __ray3t__ intersects __line3t__ (haaser)

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than Constant&lt;__rtype__&gt;.PositiveTinyValue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __ray3t__ ray, __line3t__ line
            )
        {
            return ray.Intersects(line, Constant<__rtype__>.PositiveTinyValue);
        }

        /// <summary>
        /// Returns true if the minimal distance between the line and the ray is smaller than absoluteEpsilon
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __ray3t__ ray, __line3t__ line,
            __rtype__ absoluteEpsilon
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
            this __ray3t__ ray, __line3t__ line,
            __rtype__ absoluteEpsilon,
            out __rtype__ t
            )
        {
            if (ray.GetMinimalDistanceTo(line, out t) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region __ray3t__ intersects __ray3t__ (haaser)

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than Constant&lt;__rtype__&gt;.PositiveTinyValue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __ray3t__ r0,
            __ray3t__ r1
            )
        {
            return r0.Intersects(r1, out _, out _, Constant<__rtype__>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than Constant&lt;__rtype__&gt;.PositiveTinyValue
        /// t0 and t1 hold the ray-parameters for the intersection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __ray3t__ r0,
            __ray3t__ r1,
            out __rtype__ t0,
            out __rtype__ t1
            )
        {
            return r0.Intersects(r1, out t0, out t1, Constant<__rtype__>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the minimal distance between the rays is smaller than absoluteEpsilon
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __ray3t__ r0,
            __ray3t__ r1,
            __rtype__ absoluteEpsilon
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
            this __ray3t__ r0,
            __ray3t__ r1,
            out __rtype__ t0,
            out __rtype__ t1,
            __rtype__ absoluteEpsilon
            )
        {
            if (r0.GetMinimalDistanceTo(r1, out t0, out t1) < absoluteEpsilon) return true;
            else return false;
        }

        #endregion

        #region __ray3t__ intersects Special (inconsistent argument order)

        #region __ray3t__ intersects __triangle3t__

        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the __ray3t__.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __ray3t__ ray, __triangle3t__ triangle)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                __rtype__.MinValue, __rtype__.MaxValue);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __ray3t__ ray, __triangle3t__ triangle, __rtype__ tmin, __rtype__ tmax)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                tmin, tmax);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __ray3t__ ray, __triangle3t__ triangle, __rtype__ tmin, __rtype__ tmax, out __rtype__ t)
            => ray.IntersectsTrianglePointAndEdges(
                triangle.P0, triangle.Edge01, triangle.Edge02,
                tmin, tmax, out t);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTriangle(this __ray3t__ ray, __v3t__ p0, __v3t__ p1, __v3t__ p2, __rtype__ tmin, __rtype__ tmax)
            => ray.IntersectsTriangle(p0, p1, p2, tmin, tmax, out __rtype__ _);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        public static bool IntersectsTriangle(this __ray3t__ ray, __v3t__ p0, __v3t__ p1, __v3t__ p2, __rtype__ tmin, __rtype__ tmax, out __rtype__ t)
        {
            var edge01 = p1 - p0;
            var edge02 = p2 - p0;
            var plane = Vec.Cross(ray.Direction, edge02);
            var det = Vec.Dot(edge01, plane);
            if (det > -__epsdot__ && det < __epsdot__) { t = __rtype__.NaN; return false; }
            //ray ~= parallel / Triangle
            var tv = ray.Origin - p0;
            det = 1 / det;  // det is now inverse det
            var u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) { t = __rtype__.NaN; return false; }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            var v = Vec.Dot(ray.Direction, plane) * det;
            if (v < 0 || u + v > 1) { t = __rtype__.NaN; return false; }
            var temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < tmin || temp_t >= tmax) { t = __rtype__.NaN; return false; }

            t = temp_t;
            return true;
        }


        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsTrianglePointAndEdges(this __ray3t__ ray, __v3t__ p0, __v3t__ edge01, __v3t__ edge02, __rtype__ tmin, __rtype__ tmax)
            => ray.IntersectsTrianglePointAndEdges(p0, edge01, edge02, tmin, tmax, out _);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// t holds the corresponding ray-parameter
        /// </summary>
        public static bool IntersectsTrianglePointAndEdges(this __ray3t__ ray, __v3t__ p0, __v3t__ edge01, __v3t__ edge02, __rtype__ tmin, __rtype__ tmax, out __rtype__ t)
        {
            var plane = Vec.Cross(ray.Direction, edge02);
            var det = Vec.Dot(edge01, plane);
            if (det > -__epsdot__ && det < __epsdot__) { t = __rtype__.NaN; return false; }
            //ray ~= parallel / Triangle
            var tv = ray.Origin - p0;
            det = 1 / det;  // det is now inverse det
            var u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1) { t = __rtype__.NaN; return false; }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            var v = Vec.Dot(ray.Direction, plane) * det;
            if (v < 0 || u + v > 1) { t = __rtype__.NaN; return false; }
            var temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < tmin || temp_t >= tmax) { t = __rtype__.NaN; return false; }

            t = temp_t;
            return true;
        }

        #endregion

        #region __ray3t__ intersects __quad3t__


        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the __ray3t__.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __ray3t__ ray, __quad3t__ quad)
            => ray.Intersects(quad, __rtype__.MinValue, __rtype__.MaxValue);

        /// <summary>
        /// Returns true if the ray and the quad intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// </summary>
        public static bool Intersects(this __ray3t__ ray, __quad3t__ quad, __rtype__ tmin, __rtype__ tmax)
        {
            var edge02 = quad.P2 - quad.P0;
            return ray.IntersectsTrianglePointAndEdges(quad.P0, quad.Edge01, edge02, tmin, tmax)
                || ray.IntersectsTrianglePointAndEdges(quad.P0, edge02, quad.Edge03, tmin, tmax);
        }

        /// <summary>
        /// Returns true if the ray and the quad intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// </summary>
        public static bool IntersectsQuad(this __ray3t__ ray, __v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3, __rtype__ tmin, __rtype__ tmax)
        {
            var edge02 = p2 - p0;
            return ray.IntersectsTrianglePointAndEdges(p0, p1 - p0, edge02, tmin, tmax)
                || ray.IntersectsTrianglePointAndEdges(p0, edge02, p3 - p0, tmin, tmax);
        }

        #endregion

        #region __ray3t__ intersects __polygon3t__ (haaser)

        /// <summary>
        /// Returns true if the ray and the polygon intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __ray3t__ ray, __polygon3t__ poly, __rtype__ tmin, __rtype__ tmax)
            => ray.Intersects(poly, tmin, tmax, out _);

        /// <summary>
        /// Returns true if the ray and the polygon intersect within the
        /// supplied parameter interval of the ray.
        /// t holds the correspoinding paramter.
        /// </summary>
        public static bool Intersects(this __ray3t__ ray, __polygon3t__ poly, __rtype__ tmin, __rtype__ tmax, out __rtype__ t)
        {
            var tris = poly.ComputeTriangulationOfConcavePolygon(__eminus5__);
            var count = tris.Length;

            for (var i = 0; i < count; i += 3)
            {
                if (ray.IntersectsTriangle(poly[tris[i + 0]], poly[tris[i + 1]], poly[tris[i + 2]],
                                           tmin, tmax, out t))
                {
                    return true;
                }
            }

            t = __rtype__.NaN;
            return false;
        }

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices, intersect within the
        /// supplied parameter interval of the ray.
        /// (The Method triangulates the polygon)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsPolygon(this __ray3t__ ray, __v3t__[] vertices, __rtype__ tmin, __rtype__ tmax)
            => ray.Intersects(new __polygon3t__(vertices), tmin, tmax);

        /// <summary>
        /// Returns true if the ray and the polygon, which is given by vertices and triangulation, intersect within the
        /// supplied parameter interval of the ray.
        /// </summary>
        public static bool IntersectsPolygon(
            this __ray3t__ ray,
            __v3t__[] vertices,
            int[] triangulation,
            __rtype__ tmin, __rtype__ tmax
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
            this __ray3t__ ray,
            __polygon3t__ polygon,
            int[] triangulation,
            __rtype__ tmin, __rtype__ tmax
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

        #region __ray3t__ intersects __sphere3t__

        /// <summary>
        /// Returns true if the ray and the triangle intersect. If you need
        /// information about the hit point see the __ray3t__.Hits method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __ray3t__ ray, __sphere3t__ sphere)
            => ray.Intersects(sphere, __rtype__.MinValue, __rtype__.MaxValue);

        /// <summary>
        /// Returns true if the ray and the triangle intersect within the
        /// supplied parameter interval of the ray. If you need information
        /// about the hit point see the __ray3t__.Hits method.
        /// </summary>
        public static bool Intersects(this __ray3t__ ray, __sphere3t__ sphere, __rtype__ tmin, __rtype__ tmax)
        {
            // calculate closest point
            var t = ray.Direction.Dot(sphere.Center - ray.Origin) / ray.Direction.LengthSquared;
            if (t < 0) t = 0;
            if (t < tmin) t = tmin;
            if (t > tmax) t = tmax;
            __v3t__ p = ray.Origin + t * ray.Direction;

            // distance to sphere?
            var d = (p - sphere.Center).LengthSquared;
            if (d <= sphere.RadiusSquared)
                return true;

            return false;
        }

        #endregion

        #region __sphere3t__ intersects __triangle3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __sphere3t__ sphere, __triangle3t__ triangle
             )
        {
            __v3t__ v = sphere.Center.GetClosestPointOn(triangle) - sphere.Center;
            return sphere.RadiusSquared >= v.LengthSquared;
        }

        #endregion

        #endregion


        #region __triangle3t__ intersects __line3t__ (haaser)

        /// <summary>
        /// Returns true if the triangle and the line intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __triangle3t__ tri,
            __line3t__ line
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
            this __triangle3t__ tri,
            __line3t__ line,
            out __v3t__ point
            )
        {
            return tri.IntersectsLine(line.P0, line.P1, out point);
        }

        /// <summary>
        /// returns true if the triangle and the line, given by p0 and p1, intersect.
        /// </summary>
        public static bool IntersectsLine(
            this __triangle3t__ tri,
            __v3t__ p0, __v3t__ p1
            )
        {
            __v3t__ edge01 = tri.Edge01;
            __v3t__ edge02 = tri.Edge02;
            __v3t__ dir = p1 - p0;

            __v3t__ plane = Vec.Cross(dir, edge02);
            __rtype__ det = Vec.Dot(edge01, plane);
            if (det > -__epsdot__ && det < __epsdot__)return false;
            //ray ~= parallel / Triangle
            __v3t__ tv = p0 - tri.P0;
            det = 1 / det;  // det is now inverse det
            __rtype__ u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1)
            {
                return false;
            }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            __rtype__ v = Vec.Dot(dir, plane) * det;
            if (v < 0 || u + v > 1)
            {
                return false;
            }
            __rtype__ temp_t = Vec.Dot(edge02, plane) * det;
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
            this __triangle3t__ tri,
            __v3t__ p0, __v3t__ p1,
            out __v3t__ point
            )
        {
            __v3t__ edge01 = tri.Edge01;
            __v3t__ edge02 = tri.Edge02;
            __v3t__ dir = p1 - p0;

            __v3t__ plane = Vec.Cross(dir, edge02);
            __rtype__ det = Vec.Dot(edge01, plane);
            if (det > -__epsdot__ && det < __epsdot__) { point = __v3t__.NaN; return false; }
            //ray ~= parallel / Triangle
            __v3t__ tv = p0 - tri.P0;
            det = 1 / det;  // det is now inverse det
            __rtype__ u = Vec.Dot(tv, plane) * det;
            if (u < 0 || u > 1)
            {
                point = __v3t__.NaN;
                return false;
            }
            plane = Vec.Cross(tv, edge01); // plane is now qv
            __rtype__ v = Vec.Dot(dir, plane) * det;
            if (v < 0 || u + v > 1)
            {
                point = __v3t__.NaN;
                return false;
            }
            __rtype__ temp_t = Vec.Dot(edge02, plane) * det;
            if (temp_t < 0 || temp_t >= 1)
            {
                point = __v3t__.NaN;
                return false;
            }

            point = p0 + temp_t * dir;
            return true;
        }


        #endregion

        #region __triangle3t__ intersects __ray3t__ (haaser)

        /// <summary>
        /// Returns true if the triangle and the ray intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __triangle3t__ tri,
            __ray3t__ ray
            )
        {
            return tri.Intersects(ray, __rtype__.MinValue, __rtype__.MaxValue, out _);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect
        /// within the given parameter interval
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __triangle3t__ tri,
            __ray3t__ ray,
            __rtype__ tmin, __rtype__ tmax
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
            this __triangle3t__ tri,
            __ray3t__ ray,
            out __rtype__ t
            )
        {
            return tri.Intersects(ray, __rtype__.MinValue, __rtype__.MaxValue, out t);
        }

        /// <summary>
        /// Returns true if the triangle and the ray intersect
        /// within the given parameter interval
        /// t holds the intersection paramter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __triangle3t__ tri,
            __ray3t__ ray,
            __rtype__ tmin, __rtype__ tmax,
            out __rtype__ t
            )
        {
            return ray.Intersects(tri, tmin, tmax, out t);
        }

        #endregion

        #region __triangle3t__ intersects __triangle3t__ (haaser)

        /// <summary>
        /// Returns true if the triangles intersect.
        /// </summary>
        public static bool Intersects(
            this __triangle3t__ t0,
            __triangle3t__ t1
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
            this __triangle3t__ t0,
            __triangle3t__ t1,
            out __line3t__ line
            )
        {
            List<__v3t__> points = new List<__v3t__>();

            if (t0.IntersectsLine(t1.P0, t1.P1, out __v3t__ temp)) points.Add(temp);
            if (t0.IntersectsLine(t1.P1, t1.P2, out temp)) points.Add(temp);
            if (t0.IntersectsLine(t1.P2, t1.P0, out temp)) points.Add(temp);

            if (points.Count == 2)
            {
                line = new __line3t__(points[0], points[1]);
                return true;
            }

            if (t1.IntersectsLine(t0.P0, t0.P1, out temp)) points.Add(temp);
            if (t1.IntersectsLine(t0.P1, t0.P2, out temp)) points.Add(temp);
            if (t1.IntersectsLine(t0.P2, t0.P0, out temp)) points.Add(temp);

            if (points.Count == 2)
            {
                line = new __line3t__(points[0], points[1]);
                return true;
            }

            line = new __line3t__(__v3t__.NaN, __v3t__.NaN);
            return false;
        }

        #endregion


        #region __quad3t__ intersects __line3t__ (haaser)

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __quad3t__ quad,
            __line3t__ line)
        {
            return quad.IntersectsLine(line.P0, line.P1, out _);
        }

        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// point holds the intersection point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __quad3t__ quad,
            __line3t__ line,
            out __v3t__ point)
        {
            return quad.IntersectsLine(line.P0, line.P1, out point);
        }


        /// <summary>
        /// Returns true if the quad and the line, given by p0 and p1, intersect.
        /// </summary>
        public static bool IntersectsLine(
            this __quad3t__ quad,
            __v3t__ p0, __v3t__ p1)
        {
            __ray3t__ ray = new __ray3t__(p0, p1 - p0);
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
            this __quad3t__ quad,
            __v3t__ p0, __v3t__ p1,
            out __v3t__ point)
        {
            __ray3t__ ray = new __ray3t__(p0, p1 - p0);
            if (quad.Intersects(ray, 0, 1, out __rtype__ t))
            {
                point = ray.GetPointOnRay(t);
                return true;
            }
            else
            {
                point = __v3t__.NaN;
                return false;
            }
        }

        #endregion

        #region __quad3t__ intersects __ray3t__ (haaser)


        /// <summary>
        /// Returns true if the quad and the ray intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __quad3t__ quad,
            __ray3t__ ray
            )
        {
            return quad.Intersects(ray, __rtype__.MinValue, __rtype__.MaxValue, out _);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect.
        /// t holds the intersection parameter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __quad3t__ quad,
            __ray3t__ ray,
            out __rtype__ t
            )
        {
            return quad.Intersects(ray, __rtype__.MinValue, __rtype__.MaxValue, out t);
        }

        /// <summary>
        /// Returns true if the quad and the ray intersect
        /// within the given paramter range
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __quad3t__ quad,
            __ray3t__ ray,
            __rtype__ tmin, __rtype__ tmax
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
            this __quad3t__ quad,
            __ray3t__ ray,
            __rtype__ tmin, __rtype__ tmax,
            out __rtype__ t
            )
        {
            __v3t__ edge02 = quad.P2 - quad.P0;
            if (ray.IntersectsTrianglePointAndEdges(quad.P0, quad.Edge01, edge02, tmin, tmax, out t)) return true;
            if (ray.IntersectsTrianglePointAndEdges(quad.P0, edge02, quad.Edge03, tmin, tmax, out t)) return true;

            t = __rtype__.NaN;
            return false;
        }

        #endregion

        #region __quad3t__ intersects __triangle3t__ (haaser)

        /// <summary>
        /// Returns true if the quad and the triangle intersect.
        /// </summary>
        public static bool Intersects(
            this __quad3t__ quad,
            __triangle3t__ tri
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
            this __quad3t__ quad,
            __v3t__ p0, __v3t__ p1, __v3t__ p2
            )
        {
            __triangle3t__ tri = new __triangle3t__(p0, p1, p2);
            return quad.Intersects(tri);
        }

        #endregion

        #region __quad3t__ intersects __quad3t__ (haaser)

        /// <summary>
        /// Returns true if the given quads intersect.
        /// </summary>
        public static bool Intersects(
            this __quad3t__ q0,
            __quad3t__ q1
            )
        {
            if (q0.IntersectsTriangle(q1.P0, q1.P1, q1.P2)) return true;
            if (q0.IntersectsTriangle(q1.P2, q1.P3, q1.P0)) return true;

            if (q1.IntersectsTriangle(q0.P0, q0.P1, q0.P2)) return true;
            if (q1.IntersectsTriangle(q0.P2, q0.P3, q0.P0)) return true;

            return false;
        }

        #endregion


        #region __plane3t__ intersects __line3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane3t__ plane, __line3t__ line)
        {
            return plane.IntersectsLine(line.P0, line.P1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane3t__ plane, __line3t__ line, __rtype__ absoluteEpsilon)
        {
            return plane.IntersectsLine(line.P0, line.P1, absoluteEpsilon);
        }

        public static bool IntersectsLine(this __plane3t__ plane, __v3t__ p0, __v3t__ p1, __rtype__ absoluteEpsilon)
        {
            __rtype__ h0 = plane.Height(p0);
            int s0 = (h0 > absoluteEpsilon ? 1 :(h0 < -absoluteEpsilon ? -1 : 0));
            if (s0 == 0) return true;

            __rtype__ h1 = plane.Height(p1);
            int s1 = (h1 > absoluteEpsilon ? 1 : (h1 < -absoluteEpsilon ? -1 : 0));
            if (s1 == 0) return true;


            if (s0 == s1) return false;
            else return true;
        }

        public static bool IntersectsLine(this __plane3t__ plane, __v3t__ p0, __v3t__ p1, __rtype__ absoluteEpsilon, out __v3t__ point)
        {
            //<n|origin + t0*dir> == d
            //<n|or> + t0*<n|dir> == d
            //t0 == (d - <n|or>) / <n|dir>;

            __v3t__ dir = p1 - p0;
            __rtype__ ld = dir.Length;
            dir /= ld;

            __rtype__ nDotd = plane.Normal.Dot(dir);


            if (!Fun.IsTiny(nDotd))
            {
                __rtype__ t0 = (plane.Distance - plane.Normal.Dot(p0)) / nDotd;

                if (t0 >= -absoluteEpsilon && t0 <= ld + absoluteEpsilon)
                {
                    point = p0 + dir * t0;
                    return true;
                }
                else
                {
                    point = __v3t__.NaN;
                    return false;
                }
            }
            else
            {
                point = __v3t__.NaN;
                return false;
            }

        }

        #endregion

        #region __plane3t__ intersects __ray3t__

        /// <summary>
        /// Returns true if the __ray3t__ and the __plane3t__ intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __ray3t__ ray, __plane3t__ plane)
            => !plane.Normal.IsOrthogonalTo(ray.Direction);

        /// <summary>
        /// Returns true if the ray and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __ray3t__ ray, __plane3t__ plane, out __rtype__ t
             )
        {
            __rtype__ dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot))
            {
                t = __rtype__.PositiveInfinity;
                return false;
            }
            t = -plane.Height(ray.Origin) / dot;
            return true;
        }

        /// <summary>
        /// Returns the intersection point with the given plane, or __v3t__.PositiveInfinity if ray is parallel to plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ Intersect(
             this __ray3t__ ray, __plane3t__ plane
             )
        {
            __rtype__ dot = Vec.Dot(ray.Direction, plane.Normal);
            if (Fun.IsTiny(dot)) return __v3t__.PositiveInfinity;
            return ray.GetPointOnRay(-plane.Height(ray.Origin) / dot);
        }

        /// <summary>
        /// Returns true if the ray and the plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __ray3t__ ray, __plane3t__ plane, out __rtype__ t, out __v3t__ p
             )
        {
            bool result = Intersects(ray, plane, out t);
            p = ray.Origin + t * ray.Direction;
            return result;
        }

        #endregion

        #region __plane3t__ intersects __plane3t__

        public static bool Intersects(this __plane3t__ p0, __plane3t__ p1)
        {
            bool parallel = p0.Normal.IsParallelTo(p1.Normal);

            if (parallel) return Fun.IsTiny(p0.Distance - p1.Distance);
            else return true;
        }

        public static bool Intersects(this __plane3t__ p0, __plane3t__ p1, out __ray3t__ ray)
        {
            __v3t__ dir = p0.Normal.Cross(p1.Normal);
            __rtype__ len = dir.Length;

            if (Fun.IsTiny(len))
            {
                if (Fun.IsTiny(p0.Distance - p1.Distance))
                {
                    ray = new __ray3t__(p0.Normal * p0.Distance, __v3t__.Zero);
                    return true;
                }
                else
                {
                    ray = __ray3t__.Invalid;
                    return false;
                }
            }

            dir *= 1 / len;

            var alu = new __rtype__[,]
                { { p0.Normal.X, p0.Normal.Y, p0.Normal.Z },
                  { p1.Normal.X, p1.Normal.Y, p1.Normal.Z },
                  { dir.X, dir.Y, dir.Z } };

            int[] p = alu.LuFactorize();

            var b = new __rtype__[] { p0.Distance, p1.Distance, 0 };

            var x = alu.LuSolve(p, b);

            ray = new __ray3t__(new __v3t__(x), dir);
            return true;
        }

        #endregion

        #region __plane3t__ intersects __plane3t__ intersects __plane3t__

        public static bool Intersects(this __plane3t__ p0, __plane3t__ p1, __plane3t__ p2, out __v3t__ point)
        {
            var alu = new __rtype__[,]
                { { p0.Normal.X, p0.Normal.Y, p0.Normal.Z },
                  { p1.Normal.X, p1.Normal.Y, p1.Normal.Z },
                  { p2.Normal.X, p2.Normal.Y, p2.Normal.Z } };

            var p = new int[3];
            if (!alu.LuFactorize(p)) { point = __v3t__.NaN; return false; }
            var b = new __rtype__[] { p0.Distance, p1.Distance, p2.Distance };
            var x = alu.LuSolve(p, b);
            point = new __v3t__(x);
            return true;
        }

        #endregion

        #region __plane3t__ intersects __triangle3t__

        /// <summary>
        /// Returns whether the given plane and triangle intersect.
        /// </summary>
        public static bool Intersects(
             this __plane3t__ plane, __triangle3t__ triangle
             )
        {
            int sign = plane.Sign(triangle.P0);
            if (sign == 0) return true;
            if (sign != plane.Sign(triangle.P1)) return true;
            if (sign != plane.Sign(triangle.P2)) return true;
            return false;
        }

        #endregion

        #region __plane3t__ intersects __sphere3t__

        /// <summary>
        /// Returns whether the given sphere and plane intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __plane3t__ plane, __sphere3t__ sphere
             )
        {
            return sphere.Radius >= plane.Height(sphere.Center).Abs();
        }

        #endregion

        #region __plane3t__ intersects __polygon3t__

        /// <summary>
        /// returns true if the __plane3t__ and the __polygon3t__ intersect
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __plane3t__ plane,
            __polygon3t__ poly
            )
        {
            return plane.Intersects(poly, Constant<__rtype__>.PositiveTinyValue);
        }

        /// <summary>
        /// returns true if the __plane3t__ and the polygon, intersect
        /// within a tolerance of absoluteEpsilon
        /// </summary>
        public static bool Intersects(
            this __plane3t__ plane,
            __polygon3t__ polygon,
            __rtype__ absoluteEpsilon
            )
        {
            __rtype__ height = plane.Height(polygon[0]);
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
        /// returns true if the __plane3t__ and the __polygon3t__ intersect.
        /// line holds the intersection line
        /// ATTENTION: works only with non-concave polygons!
        /// </summary>
        public static bool IntersectsConvex(
            this __plane3t__ plane,
            __polygon3t__ poly,
            out __line3t__ line
            )
        {
            return plane.IntersectsConvex(poly, Constant<__rtype__>.PositiveTinyValue, out line);
        }

        /// <summary>
        /// Returns true if the __plane3t__ and the polygon, given by points, intersect
        /// within a tolerance of absoluteEpsilon.
        /// Line holds the intersection line.
        /// ATTENTION: works only with non-concave polygons!
        /// </summary>
        public static bool IntersectsConvex(
            this __plane3t__ plane,
            __polygon3t__ polygon,
            __rtype__ absoluteEpsilon,
            out __line3t__ line
            )
        {
            int count = polygon.PointCount;
            int[] signs = new int[count];
            int pc = 0, nc = 0, zc = 0;
            for (int pi = 0; pi < count; pi++)
            {
                __rtype__ h = plane.Height(polygon[pi]);
                if (h < -absoluteEpsilon) { nc++; signs[pi] = -1; continue; }
                if (h > absoluteEpsilon) { pc++; signs[pi] = 1; continue;  }
                zc++; signs[pi] = 0;
            }

            if (zc == count)
            {
                line = new __line3t__(polygon[0], polygon[0]);
                return false;
            }
            else if (pc == 0 && zc == 0)
            {
                line = new __line3t__(__v3t__.NaN, __v3t__.NaN);
                return false;
            }
            else if (nc == 0 && zc == 0)
            {
                line = new __line3t__(__v3t__.NaN, __v3t__.NaN);
                return false;
            }
            else
            {
                int pointcount = 0;
                __v3t__[] linePoints = new __v3t__[2];
                for (int i = 0; i < count; i++)
                {
                    int u = (i + 1) % count;

                    if (signs[i] != signs[u] || signs[i] == 0 || signs[u] == 0)
                    {
                        if (plane.IntersectsLine(polygon[i], polygon[u], absoluteEpsilon, out __v3t__ point))
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
                        line = new __line3t__(linePoints[0], linePoints[1]);
                        return true;
                    }
                }
                line = new __line3t__(__v3t__.NaN, __v3t__.NaN);
                return false;
            }
        }

        #endregion

        #region __plane3t__ intersects __cylinder3t__

        /// <summary>
        /// Returns whether the given sphere and cylinder intersect.
        /// </summary>
        public static bool Intersects(this __plane3t__ plane, __cylinder3t__ cylinder)
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
        public static bool IsParallelToAxis(this __plane3t__ plane, __cylinder3t__ cylinder)
            => plane.Normal.IsOrthogonalTo(cylinder.Axis.Direction.Normalized);

        /// <summary>
        /// Tests if the given plane is orthogonal to the cylinder axis (i.e. the plane's normal is parallel to the axis).
        /// The plane will intersect the cylinder in a circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOrthogonalToAxis(this __plane3t__ plane, __cylinder3t__ cylinder)
            => plane.Normal.IsParallelTo(cylinder.Axis.Direction.Normalized);

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in an ellipse.
        /// This is only true if the plane is neither orthogonal nor parallel to the cylinder axis. Otherwise the intersection methods returning a circle or rays have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="ellipse"></param>
        public static bool Intersects(this __plane3t__ plane, __cylinder3t__ cylinder, out __ellipse3t__ ellipse)
        {
            if (plane.IsParallelToAxis(cylinder) || plane.IsOrthogonalToAxis(cylinder))
            {
                ellipse = __ellipse3t__.Zero;
                return false;
            }

            var dir = cylinder.Axis.Direction.Normalized;
            cylinder.Axis.__ray3t__.Intersects(plane, out _, out __v3t__ center);
            var cosTheta = dir.Dot(plane.Normal);

            var eNormal = plane.Normal;
            var eCenter = center;
            var eMajor = (dir - cosTheta * eNormal).Normalized;
            var eMinor = (eNormal.Cross(eMajor)).Normalized;
            eMajor = eNormal.Cross(eMinor).Normalized; //to be sure - if ellipse is nearly a circle
            eMajor = eMajor * cylinder.Radius / cosTheta.Abs();
            eMinor *= cylinder.Radius;
            ellipse = new __ellipse3t__(eCenter, eNormal, eMajor, eMinor);
            return true;
        }

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in a circle.
        /// This is only true if the plane is orthogonal to the cylinder axis. Otherwise the intersection methods returning an ellipse or rays have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="circle"></param>
        public static bool Intersects(this __plane3t__ plane, __cylinder3t__ cylinder, out __circle3t__ circle)
        {
            if (plane.IsOrthogonalToAxis(cylinder))
            {
                circle = cylinder.GetCircle(cylinder.GetHeight(plane.Point));
                return true;
            }

            circle = __circle3t__.Zero;
            return false;
        }

        /// <summary>
        /// Returns true if the given plane and cylinder intersect in one or two rays.
        /// This is only true if the plane is parallel to the cylinder axis. Otherwise the intersection methods returning an ellipse or a circle have to be used.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="cylinder">The cylinder is assumed to have infinite extent along its axis.</param>
        /// <param name="rays">Output of intersection rays. The array contains two rays (intersection), one ray (plane is tangent to cylinder) or no ray (no intersection).</param>
        public static bool Intersects(this __plane3t__ plane, __cylinder3t__ cylinder, out __ray3t__[] rays)
        {
            if (plane.IsParallelToAxis(cylinder))
            {
                var distance = cylinder.P0.GetMinimalDistanceTo(plane);
                var center = cylinder.P0 - distance * plane.Normal;
                var axis = cylinder.Axis.Direction.Normalized;

                if (distance == cylinder.Radius) //one tangent line
                {
                    rays = new[] { new __ray3t__(center, axis) };
                    return true;
                }
                else //two intersection lines
                {
                    var offset = axis.Cross(plane.Normal);
                    var extent = (cylinder.Radius.Square() - distance.Square()).Sqrt();
                    rays = new[]
                    {
                        new __ray3t__(center - extent * offset, axis),
                        new __ray3t__(center + extent * offset, axis)
                    };
                    return true;
                }
            }
            rays = Array.Empty<__ray3t__>();
            return false;
        }

        #endregion


        #region __sphere3t__ intersects __sphere3t__ (sm)

        /// <summary>
        /// Returns true if the spheres intersect, or one contains the other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __sphere3t__ s0, __sphere3t__ s1)
            => (s0.Center - s1.Center).LengthSquared <= (s0.Radius + s1.Radius).Square();

        #endregion


        #region __box3t__ intersects __line3t__

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool Intersects(this __box3t__ box, __line3t__ line)
        {
            var out0 = box.OutsideFlags(line.P0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(line.P1); if (out1 == 0) return true;
            return box.IntersectsLine(line.P0, line.P1, out0, out1);
        }

        /// <summary>
        /// Returns true if the box and the line intersect.
        /// </summary>
        public static bool IntersectsLine(
                this __box3t__ box, __v3t__ p0, __v3t__ p1)
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
                this __box3t__ box, __v3t__ p0, __v3t__ p1, Box.Flags out0, Box.Flags out1)
        {
            if ((out0 & out1) != 0) return false;

            __v3t__ min = box.Min;
            __v3t__ max = box.Max;
            var bf = out0 | out1;

            if ((bf & Box.Flags.X) != 0)
            {
                __rtype__ dx = p1.X - p0.X;
                if ((bf & Box.Flags.MinX) != 0)
                {
                    if (dx == 0 && p0.X < min.X) return false;
                    __rtype__ t = (min.X - p0.X) / dx;
                    __v3t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinX) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxX) != 0)
                {
                    if (dx == 0 && p0.X > max.X) return false;
                    __rtype__ t = (max.X - p0.X) / dx;
                    __v3t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxX) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Y) != 0)
            {
                __rtype__ dy = p1.Y - p0.Y;
                if ((bf & Box.Flags.MinY) != 0)
                {
                    if (dy == 0 && p0.Y < min.Y) return false;
                    __rtype__ t = (min.Y - p0.Y) / dy;
                    __v3t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinY) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxY) != 0)
                {
                    if (dy == 0 && p0.Y > max.Y) return false;
                    __rtype__ t = (max.Y - p0.Y) / dy;
                    __v3t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxY) == 0)
                        return true;
                }
            }
            if ((bf & Box.Flags.Z) != 0)
            {
                __rtype__ dz = p1.Z - p0.Z;
                if ((bf & Box.Flags.MinZ) != 0)
                {
                    if (dz == 0 && p0.Z < min.Z) return false;
                    __rtype__ t = (min.Z - p0.Z) / dz;
                    __v3t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MinZ) == 0)
                        return true;
                }
                if ((bf & Box.Flags.MaxZ) != 0)
                {
                    if (dz == 0 && p0.Z > max.Z) return false;
                    __rtype__ t = (max.Z - p0.Z) / dz;
                    __v3t__ p = p0 + t * (p1 - p0);
                    if ((box.OutsideFlags(p) & ~Box.Flags.MaxZ) == 0)
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region __box3t__ intersects __ray3t__ (haaser)

        public static bool Intersects(this __box3t__ box, __ray3t__ ray, out __rtype__ t)
        {
            Box.Flags out0 = box.OutsideFlags(ray.Origin);

            if (out0 == 0)
            {
                t = 0;
                return true;
            }

            __box3t__ largeBox = box.EnlargedByRelativeEps(__eminus5__);
            __rtype__ tmin = __rtype__.PositiveInfinity;
            __rtype__ ttemp;
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


            if (tmin < __rtype__.PositiveInfinity)
            {
                t = tmin;
                return true;
            }

            t = __rtype__.NaN;
            return false;
        }


        #endregion

        #region __box3t__ intersects __plane3t__

        /// <summary>
        /// Returns true if the box and the plane intersect or touch with a
        /// supplied epsilon tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __box3t__ box, __plane3t__ plane, __rtype__ eps)
        {
            var signs = box.GetIntersectionSignsWithPlane(plane, eps);
            return signs != Signs.Negative && signs != Signs.Positive;
        }

        /// <summary>
        /// Classify the position of all the eight vertices of a box with
        /// respect to a supplied plane.
        /// </summary>
        public static Signs GetIntersectionSignsWithPlane(
            this __box3t__ box, __plane3t__ plane, __rtype__ eps)
        {
            var normal = plane.Normal;
            var distance = plane.Distance;

            __rtype__ npMinX = normal.X * box.Min.X;
            __rtype__ npMaxX = normal.X * box.Max.X;
            __rtype__ npMinY = normal.Y * box.Min.Y;
            __rtype__ npMaxY = normal.Y * box.Max.Y;
            __rtype__ npMinZ = normal.Z * box.Min.Z;
            __rtype__ npMaxZ = normal.Z * box.Max.Z;

            __rtype__ hMinZ = npMinZ - distance;
            __rtype__ hMaxZ = npMaxZ - distance;

            __rtype__ hMinYMinZ = npMinY + hMinZ;
            __rtype__ hMaxYMinZ = npMaxY + hMinZ;
            __rtype__ hMinYMaxZ = npMinY + hMaxZ;
            __rtype__ hMaxYMaxZ = npMaxY + hMaxZ;

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
                this __box3t__ box, __plane3t__ plane, __rtype__ eps,
                out __box3t__ negBox, out __box3t__ zeroBox, out __box3t__ posBox)
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
                this __box3t__ box, __v3t__ normal, __rtype__ distance, __rtype__ eps,
                out __box3t__ negBox, out __box3t__ zeroBox, out __box3t__ posBox)
        {
            __rtype__ npMinX = normal.X * box.Min.X;
            __rtype__ npMaxX = normal.X * box.Max.X;
            __rtype__ npMinY = normal.Y * box.Min.Y;
            __rtype__ npMaxY = normal.Y * box.Max.Y;
            __rtype__ npMinZ = normal.Z * box.Min.Z;
            __rtype__ npMaxZ = normal.Z * box.Max.Z;

            var ha = new __rtype__[8];

            __rtype__ hMinZ = npMinZ - distance;
            __rtype__ hMaxZ = npMaxZ - distance;

            __rtype__ hMinYMinZ = npMinY + hMinZ;
            ha[0] = npMinX + hMinYMinZ;
            ha[1] = npMaxX + hMinYMinZ;

            __rtype__ hMaxYMinZ = npMaxY + hMinZ;
            ha[2] = npMinX + hMaxYMinZ;
            ha[3] = npMaxX + hMaxYMinZ;

            __rtype__ hMinYMaxZ = npMinY + hMaxZ;
            ha[4] = npMinX + hMinYMaxZ;
            ha[5] = npMaxX + hMinYMaxZ;

            __rtype__ hMaxYMaxZ = npMaxY + hMaxZ;
            ha[6] = npMinX + hMaxYMaxZ;
            ha[7] = npMaxX + hMaxYMaxZ;

            Signs all = Signs.None;
            var sa = new Signs[8];
            for (int i = 0; i < 8; i++) { sa[i] = ha[i].GetSigns(eps); all |= sa[i]; }

            negBox = __box3t__.Invalid;
            zeroBox = __box3t__.Invalid;
            posBox = __box3t__.Invalid;

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

            if (all == Signs.NonPositive) { posBox = __box3t__.Invalid; return all; }
            if (all == Signs.NonNegative) { negBox = __box3t__.Invalid; return all; }

            for (int ei = 0; ei < 12; ei++)
            {
                int i0 = c_cubeEdgeVertex0[ei], i1 = c_cubeEdgeVertex1[ei];

                if ((sa[i0] == Signs.Negative && sa[i1] == Signs.Positive)
                    || (sa[i0] == Signs.Positive && sa[i1] == Signs.Negative))
                {
                    __rtype__ h0 = ha[i0];
                    __rtype__ t = h0 / (h0 - ha[i1]);
                    __v3t__ p0 = pa[i0];
                    __v3t__ sp = p0 + t * (pa[i1] - p0);
                    negBox.ExtendBy(sp);
                    zeroBox.ExtendBy(sp);
                    posBox.ExtendBy(sp);
                }
            }

            return all;
        }

        #endregion

        #region __box3t__ intersects __sphere3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __box3t__ box, __sphere3t__ sphere
             )
        {
            __v3t__ v = sphere.Center.GetClosestPointOn(box) - sphere.Center;
            return sphere.RadiusSquared >= v.LengthSquared;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __box3t__ box, __cylinder3t__ cylinder
             )
        {

            return box.Intersects(cylinder.BoundingBox3__tc__);

            //throw new NotImplementedException();
        }

        #endregion

        #region __box3t__ intersects __triangle3t__

        /// <summary>
        /// Returns true if the box and the triangle intersect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
             this __box3t__ box, __triangle3t__ triangle
             )
        {
            return box.IntersectsTriangle(triangle.P0, triangle.P1, triangle.P2);
        }

        /// <summary>
        /// Returns true if the box and the triangle intersect.
        /// </summary>
        public static bool IntersectsTriangle(
             this __box3t__ box, __v3t__ p0, __v3t__ p1, __v3t__ p2
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
             this __box3t__ box, __v3t__ p0, __v3t__ p1, __v3t__ p2,
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
            __ray3t__ ray = new __ray3t__(box.Min, box.Size);
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

        #region __box3t__ intersects __quad3t__ (haaser)

        public static bool Intersects(
            this __box3t__ box, __quad3t__ quad
            )
        {
            Box.Flags out0 = box.OutsideFlags(quad.P0); if (out0 == 0) return true;
            Box.Flags out1 = box.OutsideFlags(quad.P1); if (out1 == 0) return true;
            Box.Flags out2 = box.OutsideFlags(quad.P2); if (out2 == 0) return true;
            Box.Flags out3 = box.OutsideFlags(quad.P3); if (out3 == 0) return true;

            return box.IntersectsQuad(quad.P0, quad.P1, quad.P2, quad.P3, out0, out1, out2, out3);
        }

        public static bool IntersectsQuad(
             this __box3t__ box, __v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3
             )
        {
            var out0 = box.OutsideFlags(p0); if (out0 == 0) return true;
            var out1 = box.OutsideFlags(p1); if (out1 == 0) return true;
            var out2 = box.OutsideFlags(p2); if (out2 == 0) return true;
            var out3 = box.OutsideFlags(p3); if (out3 == 0) return true;

            return box.IntersectsQuad(p0, p1, p2, p3, out0, out1, out2, out3);
        }

        public static bool IntersectsQuad(
            this __box3t__ box, __v3t__ p0, __v3t__ p1, __v3t__ p2, __v3t__ p3,
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
            __ray3t__ ray = new __ray3t__(box.Min, box.Size);
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

        #region __box3t__ intersects __polygon3t__ (haaser)

        public static bool Intersects(this __box3t__ box, __polygon3t__ poly)
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

            int[] tris = poly.ComputeTriangulationOfConcavePolygon(__eminus5__);

            __ray3t__ ray = new __ray3t__(box.Min, box.Size);
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

        #region __box3t__ intersects Projection-Trafo (haaser)

        /// <summary>
        /// returns true if the __box3t__ and the frustum described by the __m44t__ intersect or the frustum contains the __box3t__
        /// Assumes DirectX clip-space:
        ///     -w &lt; x &lt; w
        ///     -w &lt; y &lt; w
        ///      0 &lt; z &lt; w
        /// </summary>
        public static bool IntersectsFrustum(this __box3t__ box, __m44t__ projection)
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


            __v4t__ plane;
            __v3t__ n;

            //left
            plane = r3 + r0;
            n = plane.XYZ; box.GetMinMaxInDirection(n, out __v3t__ min, out __v3t__ max);
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


        #region __hull3t__ intersects __line3t__

        /// <summary>
        /// returns true if the __hull3t__ and the __line3t__ intersect or the __hull3t__ contains the __line3t__
        /// [__hull3t__-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __hull3t__ hull,
            __line3t__ line
            )
        {
            if (hull.Contains(line.P0)) return true;
            if (hull.Contains(line.P1)) return true;

            return hull.Intersects(line.__ray3t__, 0, 1, out _);
        }

        /// <summary>
        /// returns true if the __hull3t__ and the Line between p0 and p1 intersect or the __hull3t__ contains the Line
        /// [__hull3t__-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsLine(
            this __hull3t__ hull,
            __v3t__ p0, __v3t__ p1
            )
        {
            return hull.Intersects(new __line3t__(p0, p1));
        }

        #endregion

        #region __hull3t__ intersects __ray3t__

        /// <summary>
        /// returns true if the __hull3t__ and the __ray3t__ intersect
        /// [__hull3t__-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __hull3t__ hull,
            __ray3t__ ray
            )
        {
            return hull.Intersects(ray, __rtype__.NegativeInfinity, __rtype__.PositiveInfinity, out _);
        }

        /// <summary>
        /// returns true if the __hull3t__ and the __ray3t__ intersect
        /// the out parameter t holds the ray-parameter where an intersection was found
        /// [__hull3t__-Normals must point outside]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(
            this __hull3t__ hull,
            __ray3t__ ray,
            out __rtype__ t
            )
        {
            return hull.Intersects(ray, __rtype__.NegativeInfinity, __rtype__.PositiveInfinity, out t);
        }

        /// <summary>
        /// returns true if the __hull3t__ and the __ray3t__ intersect and the
        /// ray-parameter for the intersection is between t_min and t_max
        /// the out parameter t holds the ray-parameter where an intersection was found
        /// [__hull3t__-Normals must point outside]
        /// </summary>
        public static bool Intersects(
            this __hull3t__ hull,
            __ray3t__ ray,
            __rtype__ t_min, __rtype__ t_max,
            out __rtype__ t
            )
        {
            if (!__rtype__.IsInfinity(t_min) && hull.Contains(ray.GetPointOnRay(t_min))) { t = t_min; return true; }
            if (!__rtype__.IsInfinity(t_max) && hull.Contains(ray.GetPointOnRay(t_max))) { t = t_max; return true; }

            var planes = hull.PlaneArray;
            for (int i = 0; i < planes.Length; i++)
            {
                if (!Fun.IsTiny(planes[i].Normal.Dot(ray.Direction)) &&
                    ray.Intersects(planes[i], out __rtype__ temp_t) &&
                    temp_t >= t_min && temp_t <= t_max)
                {
                    __v3t__ candidatePoint = ray.GetPointOnRay(temp_t);
                    bool contained = true;

                    for (int u = 0; u < planes.Length; u++)
                    {
                        if (u != i && planes[u].Height(candidatePoint) > Constant<__rtype__>.PositiveTinyValue)
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

            t = __rtype__.NaN;
            return false;
        }

        #endregion

        #region __hull3t__ intersects __plane3t__

        /// <summary>
        /// returns true if the __hull3t__ and the __plane3t__ intersect
        /// [__hull3t__-Normals must point outside]
        /// </summary>
        public static bool Intersects(
            this __hull3t__ hull,
            __plane3t__ plane
            )
        {
            foreach (var p in hull.PlaneArray)
            {
                if (!p.Normal.IsParallelTo(plane.Normal) && p.Intersects(plane, out __ray3t__ ray))
                {
                    if (hull.Intersects(ray)) return true;
                }
            }

            return false;
        }

        #endregion

        #region __hull3t__ intersects __box3t__

        /// <summary>
        /// Returns true if the hull and the box intersect.
        /// </summary>
        public static bool Intersects(
            this __hull3t__ hull, __box3t__ box
            )
        {
            if (box.IsInvalid) return false;
            bool intersecting = false;
            foreach (__plane3t__ p in hull.PlaneArray)
            {
                box.GetMinMaxInDirection(p.Normal, out __v3t__ min, out __v3t__ max);
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
                this Fast__hull3t__ fastHull,
                __box3t__ box)
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

        #region __hull3t__ intersects __sphere3t__

        /// <summary>
        /// Returns true if the hull and the sphere intersect.
        /// </summary>
        public static bool Intersects(
            this __hull3t__ hull, __sphere3t__ sphere
            )
        {
            if (sphere.IsInvalid) return false;
            bool intersecting = false;
            foreach (__plane3t__ p in hull.PlaneArray)
            {
                __rtype__ height = p.Height(sphere.Center);
                if (height > sphere.Radius) return false; // outside
                if (height.Abs() < sphere.Radius) intersecting = true;
            }
            if (intersecting) return true; // intersecting
            return true; // inside
        }

        #endregion


        #region __plane3t__ intersects __box3t__

        /// <summary>
        /// Returns true if the box and the plane intersect or touch with a
        /// supplied epsilon tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this __plane3t__ plane, __rtype__ eps, __box3t__ box)
            => box.Intersects(plane, eps);

        #endregion

        //# }
    }
}

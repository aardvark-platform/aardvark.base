using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var polygon2t = "Polygon2" + tc;
    //#   var polygon3t = "Polygon3" + tc;
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var m22t = "M22" + tc;
    //#   var m44t = "M44" + tc;
    //#   var box2t = "Box2" + tc;
    //#   var plane2t = "Plane2" + tc;
    //#   var line2t = "Line2" + tc;
    //#   var iboundingbox = "IBoundingBox2" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var constant = isDouble ? "Constant" : "ConstantF";
    //#   var eps = isDouble ? "1e-6" : "1e-4f";
    #region __polygon2t__Extensions

    public static partial class __polygon2t__Extensions
    {
        #region Conversions

        public static __polygon2t__ To__polygon2t__CCW(this __box2t__ self)
            => new __polygon2t__(self.Min, new __v2t__(self.Max.X, self.Min.Y), self.Max, new __v2t__(self.Min.X, self.Max.Y));

        public static __polygon3t__ To__polygon3t__(this __polygon2t__ polygon, Func<__v2t__, __v3t__> point_copyFun)
            => new __polygon3t__(polygon.GetPointArray(point_copyFun));

        public static __polygon3t__ To__polygon3t__(this __polygon2t__ polygon, Func<__v2t__, int, __v3t__> point_index_copyFun)
            => new __polygon3t__(polygon.GetPointArray(point_index_copyFun));

        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a 2d polygon into 3d.
        /// The z-coordinate is assumed to be zero.
        /// </summary>
        public static __polygon3t__ Transformed(this __polygon2t__ polygon, __m44t__ transform)
        {
            return new __polygon3t__(polygon.GetPointArray(p => transform.TransformPos(p.XYO)));
        }

        #endregion

        #region Geometric Properties

        /// <summary>
        /// The geometric center of the polygon.
        /// </summary>
        public static __v2t__ ComputeCentroid(this __polygon2t__ polygon)
        {
            var pc = polygon.PointCount;
            __ftype__ area = 0;
            var centroid = __v2t__.Zero;

            // signed area as weight for center of edge line
            var p0 = polygon[pc - 1];
            for (int i = 0; i < pc; i++)
            {
                var p1 = polygon[i];
                var a = p0.X * p1.Y - p0.Y * p1.X;
                area += a;
                centroid += (p0 + p1) * a; // center point would be /2
                p0 = p1;
            }

            area *= __half__; // /2 moved outside loop
            return area > 0 ? centroid / (area * 6) : __v2t__.Zero; // normalization by area/6
        }

        /// <summary>
        /// Computes the area of the polygon according to
        /// "Fast Polygon Area and Newell Normal Computation"
        ///  journal of graphics tools, 7(2):9-13, 2002
        /// </summary>
        public static __ftype__ ComputeSignedArea(this __polygon2t__ polygon)
        {
            var pc = polygon.PointCount;
            if (pc < 3) return 0;
            __ftype__ area = polygon[pc - 1].X * (polygon[0].Y - polygon[pc - 2].Y);
            area += polygon[0].X * (polygon[1].Y - polygon[pc - 1].Y);
            for (int pi = 2; pi < pc; pi++)
                area += polygon[pi - 1].X * (polygon[pi].Y - polygon[pi - 2].Y);
            return area / 2;
        }

        /// <summary>
        /// Computes the area of the polygon according to
        /// "Fast Polygon Area and Newell Normal Computation"
        ///  journal of graphics tools, 7(2):9-13, 2002. The
        ///  absolute value is returned (i.e. area >= 0.0).
        /// </summary>
        public static __ftype__ ComputeArea(this __polygon2t__ polygon)
            => polygon.ComputeSignedArea().Abs();

        /// <summary>
        /// Returns the rotation of the supplied counter clockwise enumerated
        /// convex polygon that results in the minimum area enclosing box.
        /// If multiple rotations are within epsilon in their area, the one
        /// that is closest to an axis-aligned rotation (0, 90, 180, 270) is
        /// returned. O(n).
        /// </summary>
        public static __m22t__ ComputeMinAreaEnclosingBoxRotation(
                this __polygon2t__ polygon, __ftype__ epsilon = __eps__)
        {
            polygon = polygon.WithoutMultiplePoints(epsilon);
            var pc = polygon.PointCount;

            if (pc < 2) return __m22t__.Identity;
            var ea = polygon.GetEdgeArray();
            ea.Apply(v => v.Normalized);

            int i0 = 0, i1 = 0;
            int i2 = 0, i3 = 0;
            var min = polygon[0]; var max = polygon[0];
            for (int pi = 1; pi < pc; pi++)
            {
                var p = polygon[pi];
                if (p.Y < min.Y) { i0 = pi; min.Y = p.Y; }
                else if (p.Y > max.Y) { i2 = pi; max.Y = p.Y; }
                if (p.X > max.X) { i1 = pi; max.X = p.X; }
                else if (p.X < min.X) { i3 = pi; min.X = p.X; }
            }

            __v2t__ p0 = polygon[i0], e0 = ea[i0], p1 = polygon[i1], e1 = ea[i1];
            __v2t__ p2 = polygon[i2], e2 = ea[i2], p3 = polygon[i3], e3 = ea[i3];

            int end0 = (i0 + 1) % pc, end1 = (i1 + 1) % pc;
            int end2 = (i2 + 1) % pc, end3 = (i3 + 1) % pc;
            var dir = __v2t__.XAxis;

            var best = dir;
            var bestArea = __ftype__.MaxValue;
            var bestValue = __ftype__.MaxValue;
            while (true)
            {
                var s0 = Fun.FastAtan2(e0.Dot90(dir), e0.Dot(dir));
                var s1 = Fun.FastAtan2(e1.Dot180(dir), e1.Dot90(dir));
                var s2 = Fun.FastAtan2(e2.Dot270(dir), e2.Dot180(dir));
                var s3 = Fun.FastAtan2(e3.Dot(dir), e3.Dot270(dir));

                int si, si01, si23; __ftype__ s01, s23;
                if (s0 < s1) { s01 = s0; si01 = 0; } else { s01 = s1; si01 = 1; }
                if (s2 < s3) { s23 = s2; si23 = 2; } else { s23 = s3; si23 = 3; }
                if (s01 < s23) { si = si01; } else { si = si23; }

                if (si == 0) dir = ea[i0];
                else if (si == 1) dir = ea[i1].Rot270;
                else if (si == 2) dir = ea[i2].Rot180;
                else dir = ea[i3].Rot90;

                __ftype__ sx = (p2 - p0).Dot90(dir), sy = (p1 - p3).Dot(dir);
                __ftype__ area = sx * sy;
                __ftype__ value = Fun.Min(Fun.Abs(dir.X), Fun.Abs(dir.Y));

                if (area < bestArea - epsilon
                    || (area < bestArea + epsilon && value < bestValue))
                {
                    bestArea = area; bestValue = value; best = dir;
                }

                if (si == 0)
                {
                    if (++i0 >= pc) i0 -= pc;
                    if (i0 == end1) break;
                    p0 = polygon[i0]; e0 = ea[i0];
                }
                else if (si == 1)
                {
                    if (++i1 >= pc) i1 -= pc;
                    if (i1 == end2) break;
                    p1 = polygon[i1]; e1 = ea[i1];
                }
                else if (si == 2)
                {
                    if (++i2 >= pc) i2 -= pc;
                    if (i2 == end3) break;
                    p2 = polygon[i2]; e2 = ea[i2];
                }
                else
                {
                    if (++i3 >= pc) i3 -= pc;
                    if (i3 == end0) break;
                    p3 = polygon[i3]; e3 = ea[i3];
                }
            }
            return new __m22t__(best.X, best.Y, -best.Y, best.X);
        }

        /// <summary>
        /// Gets oriented bounding box of this polygon
        /// </summary>
        public static __polygon2t__ ComputeOrientedBoundingBox(this __polygon2t__ polygon)
        {
            var rot = polygon.ComputeMinAreaEnclosingBoxRotation();
            var rotInv = rot.Transposed;
            var bbGlobal = new __box2t__(polygon.Points.Select(p => rot * p));
            return new __polygon2t__(bbGlobal.ComputeCornersCCW().Apply(p => rotInv * p));
        }
        
        /// <summary>
        /// Computes the winding number of the polyon.
        /// The winding number is positive for counter-
        /// clockwise polygons, negative for clockwise polygons.
        /// </summary>
        public static int ComputeWindingNumber(this __polygon2t__ polygon)
        {
            int pc = polygon.PointCount;
            __v2t__ e = polygon[0] - polygon[pc - 1];
            var a = Fun.Atan2(e.Y, e.X);
            var a0 = a;
            var radians = 0.0;
            for (int pi = 0; pi < pc - 1; pi++)
            {
                __v2t__ e1 = polygon[pi + 1] - polygon[pi];
                var a1 = Fun.Atan2(e1.Y, e1.X);
                var alpha = a1 - a0;
                if (alpha >= __constant__.Pi) alpha -= __constant__.PiTimesTwo;
                else if (alpha < -__constant__.Pi) alpha += __constant__.PiTimesTwo;
                radians += alpha;
                a0 = a1;
            }
            var alpha0 = a - a0;
            if (alpha0 >= __constant__.Pi) alpha0 -= __constant__.PiTimesTwo;
            else if (alpha0 < -__constant__.Pi) alpha0 += __constant__.PiTimesTwo;
            radians += alpha0;

            var winding = radians >= 0
                ? (int)((__constant__.Pi + radians) / __constant__.PiTimesTwo)
                : -(int)((__constant__.Pi - radians) / __constant__.PiTimesTwo);

            return winding;
        }

        private const int V = 1;
        private static readonly Func<__v2t__, __v2t__, bool> c_ccwFun = (a, b) => (a.X * b.Y - a.Y * b.X) >= 0;
        private static readonly Func<__v2t__, __v2t__, bool> c_cwFun = (a, b) => (a.X * b.Y - a.Y * b.X) <= 0;

        /// <summary>
        /// Check if a polygon has a specified winding.
        /// </summary>
        public static bool HasWinding(
                this __polygon2t__ polygon, Winding winding)
        {
            var winFun = winding == Winding.CCW ? c_ccwFun : c_cwFun;
            var ef = polygon.Edge(0);
            var e0 = polygon.Edge(1);
            var orientation = winFun(ef, e0);
            for (int i = 2; i < polygon.PointCount; i++)
            {
                var e1 = polygon.Edge(i);
                if (winFun(e0, e1) != orientation) return false;
                e0 = e1;
            }
            return winFun(e0, ef) == orientation;
        }

        /// <summary>
        /// Returns true if polygon points are oriented in counter-clockwise order.
        /// </summary>
        public static bool IsCcw(this __polygon2t__ polygon) => polygon.ComputeWindingNumber() >= 0;

        public static bool IsConcave(this __polygon2t__ polygon) => polygon.HasWinding(Winding.CW);

        public static bool IsConvex(this __polygon2t__ polygon) => polygon.HasWinding(Winding.CCW);

        #endregion

        #region Convex Hull

        /// <summary>
        /// Returns convex hull of this polygon.
        /// </summary>
        public static Index__polygon2t__ ComputeConvexHullIndexPolygon(this __polygon2t__ polygon)
            => polygon.m_pointArray.ConvexHullIndexPolygon(polygon.m_pointCount);

        #endregion

        #region Intersection

        /// <summary>
        /// Returns:
        ///      1 if the Polygon created by poly has no self-intersections
        ///      0 if one point of the polygon lies close to a line (absoluteEpsilon)
        ///     -1 if the Polygon created by poly has a real self-intersection
        /// </summary>
        public static int HasSelfIntersections(
                this __polygon2t__ poly, __ftype__ absoluteEpsilon)
        {
            var pointCount = poly.PointCount;
            if (absoluteEpsilon < 0) throw new ArgumentOutOfRangeException();
            int worst = V;
            __v2t__ n0;

            //Triangles cannot have self-intersections
            if (pointCount == 3) return 1;

            __line2t__ line;
            int i;
            for (i = 0; i < pointCount - 1; i++)
            {
                //line between i and i+1
                line = new __line2t__(poly[i], poly[i + 1]);
                n0 = line.Direction.Normalized;
                n0 = new __v2t__(-n0.Y, n0.X);


                for (int u = i + 2; u < pointCount && (u + 1) % pointCount != i; u++)
                {
                    //Polygon not degenerated -> Line cannot intersect with line directly before and directly after
                    //All lines prior to (u,u1) have already been tested with (i,i+1)
                    int u1 = (u + 1) % pointCount;

                    if (line.IntersectsLine(poly[u], poly[u1]))
                    {
                        //One Point of (u,u1) lies on line (within absoluteEpsilon)
                        if (n0.Dot(poly[u1] - line.P0).Abs() < absoluteEpsilon || n0.Dot(poly[u] - line.P0).Abs() < absoluteEpsilon)
                        {
                            worst = 0;
                        }
                        else return -1;
                    }
                }
            }

            return worst;
        }

        /// <summary>
        /// Returns true if this polygon is fully contained inside the 'other' polygon.
        /// </summary>
        public static bool IsFullyContainedInside(this __polygon2t__ self, __polygon2t__ other)
        {
            // check if all my vertices are inside the other polygon
            foreach (var v in self.Points)
            {
                if (!other.Contains(v)) return false;
            }

            // check if all my edges do NOT intersect with edges of the other polygon
            foreach (var e in self.EdgeLines)
            {
                foreach (var x in other.EdgeLines)
                {
                    if (x.Intersects(e)) return false;
                }
            }

            // :-)
            return true;
        }

        #endregion

        #region Relations

        /// <summary>
        /// Returns the minimal distance between the polygon and the
        /// other supplied polygon. O(n).
        /// </summary>
        public static __ftype__ MinDistanceTo(this __polygon2t__ polygon, __polygon2t__ polygon1)
            => polygon.MinDistanceTo(polygon1, out _, out _, out _);

        /// <summary>
        /// Returns the minimal distance between the polygon and the non-
        /// overlapping other supplied polygon. The minimal distance is
        /// always computed as the distance between a line segment and a
        /// point. The indices of the minimal distance configuration are
        /// returned in the out parameter, as the indices of points on the
        /// two polygons, and wether the line segement was on this or the
        /// other polygon. O(n). The returned index of the line segment is
        /// the lower point index (except in case of wraparound).
        /// </summary>
        public static __ftype__ MinDistanceTo(
                this __polygon2t__ polygon, __polygon2t__ polygon1,
                out int pi0, out int pi1, out bool lineOnThis)
        {
            var p0a = polygon.m_pointArray;
            var p1a = polygon1.m_pointArray;
            var p0c = polygon.m_pointCount;
            var p1c = polygon1.m_pointCount;
            var e0a = polygon.GetEdgeArray();
            var e1a = polygon1.GetEdgeArray();
            int i0 = p0a.IndexOfMinY(p0c), i1 = p1a.IndexOfMaxY(p1c);
            __v2t__ p0 = p0a[i0], e0 = e0a[i0], p1 = p1a[i1], e1 = e1a[i1];

            int start0 = i0, start1 = i1;
            var dir = __v2t__.XAxis;
            var d = Vec.Distance(p0, p1);

            var bestValue = __ftype__.MaxValue;
            int bpi0 = -1, bpi1 = -1;
            var bLineOnThis = true;

            do
            {
                var s0 = Fun.Atan2(e0.Dot90(dir), e0.Dot(dir));
                var s1 = Fun.Atan2(e1.Dot270(dir), e1.Dot180(dir));
                if (s0 <= s1)
                {
                    dir = e0a[i0];
                    int i0n = (i0 + 1) % p0c; var p0n = p0a[i0];
                    var dn = Vec.Distance(p0n, p1);
                    var dist = DistanceToLine(p1, p0, p0n, d, dn);
                    if (dist < bestValue)
                    {
                        bestValue = dist;
                        bLineOnThis = true; bpi0 = i0; bpi1 = i1;
                    }
                    i0 = i0n; p0 = p0n; e0 = e0a[i0]; d = dn;
                }
                else
                {
                    dir = e0a[i1].Rot180;
                    int i1n = (i1 + 1) % p1c; var p1n = p1a[i1];
                    var dn = Vec.Distance(p0, p1n);
                    var dist = DistanceToLine(p0, p1, p1n, d, dn);
                    if (dist < bestValue)
                    {
                        bestValue = dist;
                        bLineOnThis = false; bpi0 = i0; bpi1 = i1;
                    }
                    i1 = i1n; p1 = p1n; e1 = e1a[i1]; d = dn;
                }
            }
            while (i0 != start0 || i1 != start1);
            lineOnThis = bLineOnThis; pi0 = bpi0; pi1 = bpi1;
            return bestValue;
        }

        private static __ftype__ DistanceToLine(__v2t__ query, __v2t__ p0, __v2t__ p1, __ftype__ d0, __ftype__ d1)
        {
            var p0p1 = p1 - p0;
            var p0q = query - p0;
            var t = Vec.Dot(p0q, p0p1);
            if (t <= 0) { return d0; }
            var denom = p0p1.LengthSquared;
            if (t >= denom) { return d1; }
            t /= denom;
            return Vec.Distance(query, p0 + t * p0p1);
        }

        #endregion

        #region Degenerated parts of a __polygon2t__

        #region __polygon2t__-Extensions

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool HasDegeneratedPart(this __polygon2t__ poly)
            => poly.PolygonHasDegeneratedPart();

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool HasDegeneratedPart(this __polygon2t__ poly, __ftype__ absoluteEpsilon)
            => poly.PolygonHasDegeneratedPart(absoluteEpsilon);

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool HasDegeneratedPart(this __polygon2t__ poly, out __polygon2t__ NonDegenerated)
        {
            bool result = poly.PolygonHasDegeneratedPart(out int[] temp);
            NonDegenerated = new __polygon2t__((from i in temp select poly[i]).ToArray<__v2t__>());
            return result;
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool HasDegeneratedPart(this __polygon2t__ poly, out int[] NonDegenerated)
            => poly.PolygonHasDegeneratedPart(out NonDegenerated);

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool HasDegeneratedPart(this __polygon2t__ poly, __ftype__ absoluteEpsilon, out int[] NonDegenerated)
            => poly.PolygonHasDegeneratedPart(absoluteEpsilon, out NonDegenerated);

        #endregion

        #region __v2t__[]-Extension

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this __polygon2t__ poly)
            => poly.PolygonHasDegeneratedPart(4 * __ftype__.Epsilon);

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this __polygon2t__ poly, __ftype__ absoluteEpsilon)
        {
            if (absoluteEpsilon < 0) throw new ArgumentOutOfRangeException();

            int i = 0;
            int count = poly.PointCount;

            __v2t__ e0;
            __v2t__ e1;

            __ftype__ l0 = 0;
            __ftype__ l1 = 0;

            while (i < count - 1)
            {
                if ((poly[i + 1] - poly[i]).Length < absoluteEpsilon) return true;

                e0 = (poly[i + 1] - poly[i]);
                e1 = (poly[(i + 2) % count] - poly[i + 1]);

                l0 = e0.Length;
                l1 = e1.Length;

                e0 = e0 / l0;
                //e1 = e1 / l1;

                if ((e0.X * e1.Y - e0.Y * e1.X).Abs() > absoluteEpsilon) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this __polygon2t__ poly, out int[] NonDegenerated)
            => poly.PolygonHasDegeneratedPart(4 * __ftype__.Epsilon, out NonDegenerated);

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this __polygon2t__ poly, __ftype__ absoluteEpsilon, out int[] NonDegenerated)
        {
            if (absoluteEpsilon < 0) throw new ArgumentOutOfRangeException();

            int i = 0;
            int u = 1;
            int start = 0;
            __v2t__ e0;
            __v2t__ e1;
            bool found = false;
            int count = poly.PointCount;

            List<int> newPoints = new List<int>();
            newPoints.Add(0);

            __ftype__ l0 = 0;

            while (i < count - 1)
            {
                e0 = (poly[i + 1] - poly[i]);
                l0 = e0.Length;

                e0 = e0 / l0;

                u = i + 1;
                __v2t__ e = poly[(u + 1) % count] - poly[u];
                while (
                    (((e0.X * e.Y - e0.Y * e.X).Abs() < absoluteEpsilon && e.Dot(e0) > 0) ||
                    (poly[(u + 1) % count] - poly[i + 1]).Length < absoluteEpsilon)
                    && u < count - 1)
                {
                    if ((poly[(u + 1) % count] - poly[i + 1]).Length < absoluteEpsilon) found = true;

                    u++;
                    e0 = (poly[u] - poly[i]).Normalized;
                    e = poly[(u + 1) % count] - poly[u];
                }
                u--;

                start = u;

                do
                {
                    u++;
                    e1 = (poly[(u + 1) % count] - poly[u]);
                }
                while (u < count && ((e0.X * e1.Y - e0.Y * e1.X).Abs() < absoluteEpsilon && e.Dot(e0) < 0));

                if (u != start + 1) found = true;

                newPoints.Add(u);

                i = u;
            }

            NonDegenerated = newPoints.ToArray();
            return found;
        }

        #endregion

        #endregion
    }

    public static partial class Index__polygon2t__Extensions
    {
        #region Conversions

        public static __polygon3t__ To__polygon3t__(this Index__polygon2t__ polygon, __v3t__[] pointArray)
            => new __polygon3t__(polygon.GetPointArray(pointArray));

        public static __polygon3t__ To__polygon3t__(this Index__polygon2t__ polygon, List<__v3t__> pointList)
            => new __polygon3t__(polygon.GetPointArray(pointList));

        #endregion
    }

    #endregion

    //# }
}

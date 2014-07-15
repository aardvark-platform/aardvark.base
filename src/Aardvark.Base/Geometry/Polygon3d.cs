using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static partial class Polygon3dExtensions
    {
        #region Conversions

        public static Polygon2d ToPolygon2d(
                this Polygon3d polygon, Func<V3d, V2d> point_copyFun)
        {
            return new Polygon2d(polygon.GetPointArray(point_copyFun));
        }

        public static Polygon2d ToPolygon2d(
                this Polygon3d polygon, Func<V3d, int, V2d> point_index_copyFun)
        {
            return new Polygon2d(polygon.GetPointArray(point_index_copyFun));
        }

        #endregion

        #region Geometric Properties

        /// <summary>
        /// Computes the area as the length of the sum of  the simple
        /// triangulation cross-product normals.
        /// NOTE: This has been tested to be slightly faster and slightly more
        /// accurate than the computation via the 3d Newell normal
        /// (see Math.Tests/GeometryTests, rft 2013-05-04).
        /// </summary>
        public static double ComputeArea(this Polygon3d polygon)
        {
            return 0.5 * polygon.ComputeDoubleAreaNormal().Length;
        }

        /// <summary>
        /// Computes the normalized normal as the sum of  the simple
        /// triangulation cross-product normals.
        /// NOTE: This has been tested to be slightly faster and slightly more
        /// accurate than the computation via the 3d Newell normal
        /// (see Math.Tests/GeometryTests, rft 2013-05-04).
        /// </summary>
        public static V3d ComputeNormal(this Polygon3d polygon)
        {
            return polygon.ComputeDoubleAreaNormal().Normalized;
        }

        /// <summary>
        /// The geometric center of a 3-dimensional, flat polygon.
        /// WARNING: UNTESTED!
        /// </summary>
        public static V3d ComputeCentroid(this Polygon3d polygon)
        {
            var pc = polygon.PointCount;
            if (pc < 3) return V3d.Zero;
            V3d p0 = polygon[0], p1 = polygon[1];
            V3d e0 = p1 - p0;
            var p2 = polygon[2];
            var e1 = p2 - p0;
            var normal = e0.Cross(e1);
            var area2 = normal.Length;
            var centroid = area2 * (p0 + p1 + p2);
            p1 = p2; e0 = e1;
            for (int pi = 3; pi < pc; pi++)
            {
                p2 = polygon[pi]; e1 = p2 - p0;
                var n = e0.Cross(e1);
                var a2 = Fun.Sign(normal.Dot(n)) * n.Length;
                area2 += a2;
                centroid += a2 * (p0 + p1 + p2);
                p1 = p2; e0 = e1;
            }
            if (area2 > Constant<double>.PositiveTinyValue)
                return centroid * (Constant.OneThird / area2);
            else if (area2 < Constant<double>.NegativeTinyValue)
                return centroid * (-Constant.OneThird / area2);
            else
                return V3d.Zero;
        }

        /// <summary>
        /// Computes the normal with the length of twice the polygon area as
        /// the sum of  the simple triangulation cross-product normals.
        /// NOTE: This has been tested to be slightly faster and slightly more
        /// accurate than the computation via the 3d Newell normal
        /// (see Math.Tests/GeometryTests, rft 2013-05-04).
        /// </summary>
        public static V3d ComputeDoubleAreaNormal(this Polygon3d polygon)
        {
            var pc = polygon.PointCount;
            if (pc < 3) return V3d.Zero;
            V3d p0 = polygon[0];
            V3d e0 = polygon[1] - p0;
            V3d normal = V3d.Zero;
            for (int pi = 2; pi < pc; pi++)
            {
                var e1 = polygon[pi] - p0;
                normal += e0.Cross(e1);
                e0 = e1;
            }
            return normal;
        }

        /// <summary>
        /// Computes the supporting plane of the polygon via the vertex centroid
        /// and the sum of  the simple triangulation cross-product normals.
        /// NOTE: This has been tested to be slightly faster and slightly more
        /// accurate than the computation via the 3d Newell normal
        /// (see Math.Tests/GeometryTests, rft 2013-05-04).
        /// </summary>
        public static Plane3d ComputePlane3d(this Polygon3d polygon)
        {
            return new Plane3d(polygon.ComputeNormal(), polygon.ComputeVertexCentroid());
        }

        /// <summary>
        /// Returns the plane through the first 3 points of the polygon.
        /// </summary>
        public static Plane3d GetPlane3d(this Polygon3d polygon)
        {
            return new Plane3d(polygon[0], polygon[1], polygon[2]);
        }

        #endregion

        #region FormFactor

        public static double ComputeToPointFormFactor(
                this Polygon3d sourcePolygon, double polygonArea,
                V3d targetPoint, V3d targetNormal,
                double eps = 1e-6)
        {
            return sourcePolygon.ComputeUnscaledFormFactor(
                                        targetPoint, targetNormal, eps)
                    / (Constant.PiTimesTwo * polygonArea);
        }

        public static double ComputeFromPointFormFactor(
                this Polygon3d targetPolygon,
                V3d sourcePoint, V3d sourceNormal,
                double eps = 1e-6)
        {
            return targetPolygon.ComputeUnscaledFormFactor(
                                        sourcePoint, sourceNormal, eps)
                    / Constant.PiTimesTwo;
        }

        public static double ComputeUnscaledFormFactor(
                this Polygon3d polygon,
                V3d p, V3d n,
                double eps = 1e-6)
        {
            var vc = polygon.PointCount;

            V3d[] cpa = new V3d[vc + 1];

            var cc = 0;
            var pb = polygon[0] - p;
            var hb = V3d.Dot(pb, n); bool hbp = hb > eps, hbn = hb < -eps;
            if (hb >= -eps) cpa[cc++] = pb;
            var p0 = pb; var h0 = hb; var h0p = hbp; var h0n = hbn;
            for (int vi = 1; vi < vc; vi++)
            {
                var p1 = polygon[vi] - p; var h1 = V3d.Dot(p1, n);
                bool h1p = h1 > eps, h1n = h1 < -eps;
                if (h0p && h1n || h0n && h1p)
                    cpa[cc++] = p0 + (p1 - p0) * (h0 / (h0 - h1));
                if (h1 >= -eps) cpa[cc++] = p1;
                p0 = p1; h0 = h1; h0p = h1p; h0n = h1n;
            }
            if (h0p && hbn || h0n && hbp)
                cpa[cc++] = p0 + (pb - p0) * (h0 / (h0 - hb));

            var cpr = cpa.Copy(cc, v => v.Length);

            var cv = V3d.Cross(cpa[0], cpa[cc - 1]);
            double ff = V3d.Dot(n, cv)
                        * Fun.Acos(V3d.Dot(cpa[0], cpa[cc - 1])
                                    / (cpr[0] * cpr[cc - 1]))
                        / cv.Length;

            for (int ci = 0; ci < cc - 1; ci++)
            {
                cv = V3d.Cross(cpa[ci + 1], cpa[ci]);
                ff += V3d.Dot(n, cv)
                       * Fun.Acos(V3d.Dot(cpa[ci + 1], cpa[ci])
                                    / (cpr[ci + 1] * cpr[ci]))
                        / cv.Length;
            }
            return ff;
        }

        #endregion

        #region Shape Analysis

        /// <summary>
        /// Enumerates all pairs of coincident vertices (as pairs of vertex indices).
        /// </summary>
        public static IEnumerable<Pair<int>> GetCoincidentPoints(
                this Polygon3d polygon, double toleranceAbsolute)
        {
            if (toleranceAbsolute < 0.0) throw new ArgumentOutOfRangeException();
            var pc = polygon.PointCount;
            for (int pi = 0; pi < pc; pi++)
            {
                for (int pj = pi + 1; pj < pc; pj++)
                {
                    if (polygon[pi].ApproxEqual(polygon[pj], toleranceAbsolute))
                        yield return Pair.Create(pi, pj);
                }
            }
        }

        /// <summary>
        /// Returns true if at least two vertices are coincident.
        /// </summary>
        public static bool HasCoincidentPoints(
                this Polygon3d polygon, double toleranceAbsolute)
        {
            return GetCoincidentPoints(polygon, toleranceAbsolute).Any();
        }

        /// <summary>
        /// Enumerates all pairs of edges that intersect (as pairs of edge indices).
        /// </summary>
        public static IEnumerable<Pair<int>> GetSelfIntersections(
                this Polygon3d polygon, double toleranceAbsolute)
        {
            if (toleranceAbsolute < 0.0) throw new ArgumentOutOfRangeException();
            var pc = polygon.PointCount;
            var la = polygon.GetEdgeLineArray();
            for (int i = 0; i < pc; i++)
            {
                int jmax = (i > 0) ? pc : pc - 1;
                for (int j = i + 2; j < jmax; j++)
                {
                    if (la[i].Intersects(la[j], toleranceAbsolute)) yield return Pair.Create(i, j);
                }
            }
        }

        /// <summary>
        /// Returns true if at least two edges intersect.
        /// </summary>
        public static bool IsSelfIntersecting(
                this Polygon3d self, double toleranceAbsolute = 1e-10)
        {
            return GetSelfIntersections(self, toleranceAbsolute).Any();
        }

        /// <summary>
        /// Enumerates all vertex indexes at which
        /// both outgoing edges meet at an angle that
        /// is less than the given threshold.
        /// </summary>
        public static IEnumerable<int> GetSpikes(
                this Polygon3d self, double toleranceInDegrees = 0.1)
        {
            if (toleranceInDegrees < 0.0) throw new ArgumentOutOfRangeException();
            var threshold = Conversion.RadiansFromDegrees(toleranceInDegrees).Cos();
            var edges = self.GetEdgeArray();
            edges.Apply(e => e.Normalized);
            var ec = edges.Length;
            if (V3d.Dot(-edges[ec - 1], edges[0]) > threshold) yield return 0;
            for (int i = 1; i < ec; i++)
            {
                if (V3d.Dot(-edges[i - 1], edges[i]) > threshold) yield return i;
            }
        }

        /// <summary>
        /// Returns true if at least one vertex
        /// both outgoing edges meet at an angle that
        /// is less than the given threshold.
        /// </summary>
        public static bool HasSpikes(
                this Polygon3d self, double toleranceInDegrees = 0.1)
        {
            return GetSpikes(self, toleranceInDegrees).Any();
        }

        #endregion
    }

    public static partial class IndexPolygon3dExtensions
    {
        #region Conversions

        public static Polygon2d ToPolygon3d(this IndexPolygon3d polygon, V2d[] pointArray)
        {
            return new Polygon2d(polygon.GetPointArray(pointArray));
        }

        public static Polygon2d ToPolygon3d(this IndexPolygon3d polygon, List<V2d> pointList)
        {
            return new Polygon2d(polygon.GetPointArray(pointList));
        }

        #endregion
    }

}

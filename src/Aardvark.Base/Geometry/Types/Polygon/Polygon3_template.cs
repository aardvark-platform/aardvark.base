using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
    //#   var plane3t = "Plane3" + tc;
    //#   var line2t = "Line2" + tc;
    //#   var iboundingbox = "IBoundingBox2" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var zerodotone = isDouble ? "0.1" : "0.1f";
    //#   var constant = isDouble ? "Constant" : "ConstantF";
    //#   var eps = isDouble ? "1e-6" : "1e-4f";
    #region __polygon3t__Extensions

    public static partial class __polygon3t__Extensions
    {
        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __polygon2t__ To__polygon2t__(this __polygon3t__ polygon, Func<__v3t__, __v2t__> point_copyFun)
            => new __polygon2t__(polygon.GetPointArray(point_copyFun));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __polygon2t__ To__polygon2t__(
            this __polygon3t__ polygon, Func<__v3t__, int, __v2t__> point_index_copyFun
            )
            => new __polygon2t__(polygon.GetPointArray(point_index_copyFun));

        #endregion

        #region Geometric Properties

        /// <summary>
        /// Computes the area as the length of the sum of  the simple
        /// triangulation cross-product normals.
        /// NOTE: This has been tested to be slightly faster and slightly more
        /// accurate than the computation via the 3d Newell normal
        /// (see Math.Tests/GeometryTests, rft 2013-05-04).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ ComputeArea(this __polygon3t__ polygon)
            => __half__ * polygon.ComputeDoubleAreaNormal().Length;

        /// <summary>
        /// Computes the normalized normal as the sum of  the simple
        /// triangulation cross-product normals.
        /// NOTE: This has been tested to be slightly faster and slightly more
        /// accurate than the computation via the 3d Newell normal
        /// (see Math.Tests/GeometryTests, rft 2013-05-04).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ ComputeNormal(this __polygon3t__ polygon)
            => polygon.ComputeDoubleAreaNormal().Normalized;

        /// <summary>
        /// The geometric center of a 3-dimensional, flat polygon.
        /// WARNING: UNTESTED!
        /// </summary>
        public static __v3t__ ComputeCentroid(this __polygon3t__ polygon)
        {
            var pc = polygon.PointCount;
            if (pc < 3) return __v3t__.Zero;
            __v3t__ p0 = polygon[0], p1 = polygon[1];
            __v3t__ e0 = p1 - p0;
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
            if (area2 > Constant<__ftype__>.PositiveTinyValue)
                return centroid * (__constant__.OneThird / area2);
            else if (area2 < Constant<__ftype__>.NegativeTinyValue)
                return centroid * (-__constant__.OneThird / area2);
            else
                return __v3t__.Zero;
        }

        /// <summary>
        /// Computes the normal with the length of twice the polygon area as
        /// the sum of  the simple triangulation cross-product normals.
        /// NOTE: This has been tested to be slightly faster and slightly more
        /// accurate than the computation via the 3d Newell normal
        /// (see Math.Tests/GeometryTests, rft 2013-05-04).
        /// </summary>
        public static __v3t__ ComputeDoubleAreaNormal(this __polygon3t__ polygon)
        {
            var pc = polygon.PointCount;
            if (pc < 3) return __v3t__.Zero;
            __v3t__ p0 = polygon[0];
            __v3t__ e0 = polygon[1] - p0;
            __v3t__ normal = __v3t__.Zero;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __plane3t__ Compute__plane3t__(this __polygon3t__ polygon)
            => new __plane3t__(polygon.ComputeNormal(), polygon.ComputeVertexCentroid());

        /// <summary>
        /// Returns the plane through the first 3 points of the polygon.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __plane3t__ Get__plane3t__(this __polygon3t__ polygon)
            => new __plane3t__(polygon[0], polygon[1], polygon[2]);

        #endregion

        #region FormFactor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ ComputeToPointFormFactor(
                this __polygon3t__ sourcePolygon, __ftype__ polygonArea,
                __v3t__ targetPoint, __v3t__ targetNormal,
                __ftype__ eps = __eps__)
        {
            return sourcePolygon.ComputeUnscaledFormFactor(
                                        targetPoint, targetNormal, eps)
                    / (__constant__.PiTimesTwo * polygonArea);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ ComputeFromPointFormFactor(
                this __polygon3t__ targetPolygon,
                __v3t__ sourcePoint, __v3t__ sourceNormal,
                __ftype__ eps = __eps__)
        {
            return targetPolygon.ComputeUnscaledFormFactor(
                                        sourcePoint, sourceNormal, eps)
                    / __constant__.PiTimesTwo;
        }

        public static __ftype__ ComputeUnscaledFormFactor(
                this __polygon3t__ polygon,
                __v3t__ p, __v3t__ n,
                __ftype__ eps = __eps__)
        {
            var vc = polygon.PointCount;

            __v3t__[] cpa = new __v3t__[vc + 1];

            var cc = 0;
            var pb = polygon[0] - p;
            var hb = Vec.Dot(pb, n); bool hbp = hb > eps, hbn = hb < -eps;
            if (hb >= -eps) cpa[cc++] = pb;
            var p0 = pb; var h0 = hb; var h0p = hbp; var h0n = hbn;
            for (int vi = 1; vi < vc; vi++)
            {
                var p1 = polygon[vi] - p; var h1 = Vec.Dot(p1, n);
                bool h1p = h1 > eps, h1n = h1 < -eps;
                if (h0p && h1n || h0n && h1p)
                    cpa[cc++] = p0 + (p1 - p0) * (h0 / (h0 - h1));
                if (h1 >= -eps) cpa[cc++] = p1;
                p0 = p1; h0 = h1; h0p = h1p; h0n = h1n;
            }
            if (h0p && hbn || h0n && hbp)
                cpa[cc++] = p0 + (pb - p0) * (h0 / (h0 - hb));

            var cpr = cpa.Map(cc, v => v.Length);

            var cv = Vec.Cross(cpa[0], cpa[cc - 1]);
            __ftype__ ff = Vec.Dot(n, cv)
                        * Fun.AcosClamped(Vec.Dot(cpa[0], cpa[cc - 1])
                                    / (cpr[0] * cpr[cc - 1]))
                        / cv.Length;

            for (int ci = 0; ci < cc - 1; ci++)
            {
                cv = Vec.Cross(cpa[ci + 1], cpa[ci]);
                ff += Vec.Dot(n, cv)
                       * Fun.AcosClamped(Vec.Dot(cpa[ci + 1], cpa[ci])
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
        public static IEnumerable<(int, int)> GetCoincidentPoints(
                this __polygon3t__ polygon, __ftype__ toleranceAbsolute)
        {
            if (toleranceAbsolute < 0) throw new ArgumentOutOfRangeException();
            var pc = polygon.PointCount;
            for (int pi = 0; pi < pc; pi++)
            {
                for (int pj = pi + 1; pj < pc; pj++)
                {
                    if (polygon[pi].ApproximateEquals(polygon[pj], toleranceAbsolute))
                        yield return (pi, pj);
                }
            }
        }

        /// <summary>
        /// Returns true if at least two vertices are coincident.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasCoincidentPoints(
                this __polygon3t__ polygon, __ftype__ toleranceAbsolute)
        {
            return GetCoincidentPoints(polygon, toleranceAbsolute).Any();
        }

        /// <summary>
        /// Enumerates all pairs of edges that intersect (as pairs of edge indices).
        /// </summary>
        public static IEnumerable<(int, int)> GetSelfIntersections(
                this __polygon3t__ polygon, __ftype__ toleranceAbsolute)
        {
            if (toleranceAbsolute < 0) throw new ArgumentOutOfRangeException();
            var pc = polygon.PointCount;
            var la = polygon.GetEdgeLineArray();
            for (int i = 0; i < pc; i++)
            {
                int jmax = (i > 0) ? pc : pc - 1;
                for (int j = i + 2; j < jmax; j++)
                {
                    if (la[i].Intersects(la[j], toleranceAbsolute)) yield return (i, j);
                }
            }
        }

        /// <summary>
        /// Returns true if at least two edges intersect.
        /// </summary>
        //# var toleranceAbsolute = isDouble ? "1e-10" : "1e-5f";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSelfIntersecting(
                this __polygon3t__ self, __ftype__ toleranceAbsolute = __toleranceAbsolute__)
        {
            return GetSelfIntersections(self, toleranceAbsolute).Any();
        }

        /// <summary>
        /// Enumerates all vertex indexes at which
        /// both outgoing edges meet at an angle that
        /// is less than the given threshold.
        /// </summary>
        public static IEnumerable<int> GetSpikes(
                this __polygon3t__ self, __ftype__ toleranceInDegrees = __zerodotone__)
        {
            if (toleranceInDegrees < 0) throw new ArgumentOutOfRangeException();
            var threshold = Conversion.RadiansFromDegrees(toleranceInDegrees).Cos();
            var edges = self.GetEdgeArray();
            edges.Apply(e => e.Normalized);
            var ec = edges.Length;
            if (Vec.Dot(-edges[ec - 1], edges[0]) > threshold) yield return 0;
            for (int i = 1; i < ec; i++)
            {
                if (Vec.Dot(-edges[i - 1], edges[i]) > threshold) yield return i;
            }
        }

        /// <summary>
        /// Returns true if at least one vertex
        /// both outgoing edges meet at an angle that
        /// is less than the given threshold.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSpikes(
                this __polygon3t__ self, __ftype__ toleranceInDegrees = __zerodotone__)
        {
            return GetSpikes(self, toleranceInDegrees).Any();
        }

        #endregion
    }

    #endregion

    #region Index__polygon3t__Extensions

    public static partial class Index__polygon3t__Extensions
    {
        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __polygon2t__ To__polygon3t__(this Index__polygon3t__ polygon, __v2t__[] pointArray)
            => new __polygon2t__(polygon.GetPointArray(pointArray));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __polygon2t__ To__polygon3t__(this Index__polygon3t__ polygon, List<__v2t__> pointList)
            => new __polygon2t__(polygon.GetPointArray(pointList));

        #endregion
    }

    #endregion

    //# }
}

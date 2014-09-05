using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides various methods determining parallelism
    /// </summary>
    public static class Parallelism
    {
        // 2-Dimensional

        #region V2d - V2d

        public static bool IsParallelTo(this V2d u, V2d v)
        {
            return Fun.IsTiny(u.X * v.Y - u.Y * v.X);
        }

        public static bool IsParallelTo(this V2d u, V2d v, double epsilon = 0.01)
        {
            var un = u.Normalized;
            var vn = v.Normalized;

            return (un - vn).Length < epsilon || (un + vn).Length < epsilon;
        }

        #endregion

        #region Ray2d - V2d

        public static bool IsParallelTo(this Ray2d ray, V2d v)
        {
            return ray.Direction.IsParallelTo(v);
        }

        public static bool IsParallelTo(this Ray2d ray, V2d v, double epsilon = 0.01)
        {
            return ray.Direction.IsParallelTo(v, epsilon);
        }

        #endregion

        #region Ray2d - Ray2d

        public static bool IsParallelTo(this Ray2d r0, Ray2d r1)
        {
            return r0.Direction.IsParallelTo(r1.Direction);
        }

        public static bool IsParallelTo(this Ray2d r0, Ray2d r1, double epsilon = 0.01)
        {
            return r0.Direction.IsParallelTo(r1.Direction, epsilon);
        }

        #endregion

        #region Line2d - Line2d

        public static bool IsParallelTo(this Line2d l0, Line2d l1)
        {
            return l0.Direction.IsParallelTo(l1.Direction);
        }

        public static bool IsParallelTo(this Line2d l0, Line2d l1, double epsilon = 0.01)
        {
            return l0.Direction.IsParallelTo(l1.Direction, epsilon);
        }

        #endregion

        // 3-Dimensional

        #region V3d - V3d

        public static bool IsParallelTo(this V3d u, V3d v)
        {
            return Fun.IsTiny(u.Cross(v).Norm1);
        }

        #endregion

        #region Ray3d - V3d

        public static bool IsParallelTo(this Ray3d ray, V3d vec)
        {
            return ray.Direction.IsParallelTo(vec);
        }

        #endregion

        #region Ray3d - Ray3d

        public static bool IsParallelTo(this Ray3d r0, Ray3d r1)
        {
            return r0.Direction.IsParallelTo(r1.Direction);
        }

        #endregion

        #region Plane3d - Plane3d

        public static bool IsParallelTo(this Plane3d p0, Plane3d p1)
        {
            return p0.Normal.IsParallelTo(p1.Normal);
        }

        #endregion

        #region Ray3d - Plane3d

        public static bool IsParallelTo(this Ray3d ray, Plane3d plane)
        {
            return ray.Direction.IsOrthogonalTo(plane.Normal);
        }

        public static bool IsParallelTo(this Plane3d plane , Ray3d ray)
        {
            return ray.Direction.IsOrthogonalTo(plane.Normal);
        }

        #endregion

        #region Line3d - Line3d

        public static bool IsParallelTo(this Line3d l0, Line3d l1)
        {
            return l0.Direction.IsParallelTo(l1.Direction);
        }

        #endregion
    }

    /// <summary>
    /// Provides various methods determining normalism
    /// </summary>
    public static class Otrthogonalism
    {
        // 2-Dimensional

        #region V2d - V2d

        public static bool IsOrthogonalTo(this V2d u, V2d v)
        {
            return Fun.IsTiny(u.Dot(v));
        }

        #endregion

        #region Ray2d - V2d

        public static bool IsOrthogonalTo(this Ray2d ray, V2d v)
        {
            return ray.Direction.IsOrthogonalTo(v);
        }

        #endregion

        #region Ray2d - Ray2d

        public static bool IsOrthogonalTo(this Ray2d r0, Ray2d r1)
        {
            return r0.Direction.IsOrthogonalTo(r1.Direction);
        }

        #endregion

        // 3-Dimensional

        #region V3d - V3d

        public static bool IsOrthogonalTo(this V3d u, V3d v)
        {
            return Fun.IsTiny(u.Dot(v));
        }

        #endregion

        #region Ray3d - V3d

        public static bool IsOrthogonalTo(this Ray3d ray, V3d vec)
        {
            return ray.Direction.IsOrthogonalTo(vec);
        }

        #endregion

        #region Ray3d - Ray3d

        public static bool IsOrthogonalTo(this Ray3d r0, Ray3d r1)
        {
            return r0.Direction.IsOrthogonalTo(r1.Direction);
        }

        #endregion

        #region Plane3d - Plane3d

        public static bool IsOrthogonalTo(this Plane3d p0, Plane3d p1)
        {
            return p0.Normal.IsOrthogonalTo(p1.Normal);
        }

        #endregion

        #region Ray3d - Plane3d

        public static bool IsOrthogonalTo(this Ray3d ray, Plane3d plane)
        {
            return ray.Direction.IsParallelTo(plane.Normal);
        }

        public static bool IsNormalTo(this Plane3d plane, Ray3d ray)
        {
            return ray.Direction.IsParallelTo(plane.Normal);
        }

        #endregion
    }

    public static class LinearCombination
    {
        #region V3d - V3d

        public static bool IsLinearCombinationOf(this V3d x, V3d u, V3d v)
        {
            V3d n = u.Cross(v);
            return n.IsOrthogonalTo(x);
        }

        public static bool IsLinearCombinationOf(this V3d x, V3d u)
        {
            return x.IsParallelTo(u);
        }

        public static bool IsLinearCombinationOf(this V3d x, V3d u, V3d v, out double t0, out double t1)
        { 
            //x == t2*u + t1*v
            V3d n = u.Cross(v);

            double[,] mat = new double[3, 3] 
            { 
                {u.X,v.X,n.X},
                {u.Y,v.Y,n.Y},
                {u.Z,v.Z,n.Z}
            };

            double[] result = new double[3]{x.X,x.Y,x.Z};

            int[] perm = mat.LuFactorize();
            V3d t = new V3d(mat.LuSolve(perm,result));

            if (Fun.IsTiny(t.Z))
            {
                t0 = t.X;
                t1 = t.Y;

                return true;
            }
            else
            {
                t0 = double.NaN;
                t1 = double.NaN;

                return false;
            }

            //x == 
        }

        #endregion
    }

    public static class GeometricProperties
    {
        #region Degenerated parts of a Polygon2d

        #region Polygon2d-Extensions
        
        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool HasDegeneratedPart(this Polygon2d poly)
        {
            return poly.PolygonHasDegeneratedPart();
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool HasDegeneratedPart(this Polygon2d poly, double absoluteEpsilon)
        {
            return poly.PolygonHasDegeneratedPart(absoluteEpsilon);
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool HasDegeneratedPart(this Polygon2d poly, out Polygon2d NonDegenerated)
        {
            int[] temp;
            bool result = poly.PolygonHasDegeneratedPart(out temp);

            NonDegenerated = new Polygon2d((from i in temp select poly[i]).ToArray<V2d>());
            return result;
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool HasDegeneratedPart(this Polygon2d poly, out int[] NonDegenerated)
        {
            return poly.PolygonHasDegeneratedPart(out NonDegenerated);
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool HasDegeneratedPart(this Polygon2d poly, double absoluteEpsilon, out int[] NonDegenerated)
        {
            return poly.PolygonHasDegeneratedPart(absoluteEpsilon, out NonDegenerated);
        }

        #endregion

        #region V2d[]-Extension

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this Polygon2d poly)
        {
            return poly.PolygonHasDegeneratedPart(4.0 * double.Epsilon);
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this Polygon2d poly, double absoluteEpsilon)
        {
            if (absoluteEpsilon < 0.0) throw new ArgumentOutOfRangeException();

            int i = 0;
            int count = poly.PointCount;

            V2d e0;
            V2d e1;

            double l0 = 0.0;
            double l1 = 0.0;

            while (i < count - 1)
            {
                if ((poly[i + 1] - poly[i]).Length < absoluteEpsilon) return true;

                e0 = (poly[i + 1] - poly[i]);
                e1 = (poly[(i + 2) % count] - poly[i + 1]);

                l0 = e0.Length;
                l1 = e1.Length;

                e0 = e0 / l0;
                //e1 = e1 / l1;

                if ((e0.X*e1.Y - e0.Y*e1.X).Abs() > absoluteEpsilon) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this Polygon2d poly, out int[] NonDegenerated)
        {
            return poly.PolygonHasDegeneratedPart(4.0 * double.Epsilon, out NonDegenerated);
        }

        /// <summary>
        /// Returns true if the Polygon contains a degenerated part
        /// NonDegenerated holds the Non-Degenerated part of the polygon
        /// </summary>
        public static bool PolygonHasDegeneratedPart(this Polygon2d poly, double absoluteEpsilon, out int[] NonDegenerated)
        {
            if (absoluteEpsilon < 0.0) throw new ArgumentOutOfRangeException();

            int i = 0;
            int u = 1;
            int start = 0;
            V2d e0;
            V2d e1;
            bool found = false;
            int count = poly.PointCount;

            List<int> newPoints = new List<int>();
            newPoints.Add(0);

            double l0 = 0.0;


            while (i < count - 1)
            {
                e0 = (poly[i + 1] - poly[i]);
                l0 = e0.Length;

                e0 = e0 / l0;


                
                u = i + 1;
                V2d e = poly[(u + 1) % count] - poly[u];
                while (
                    (((e0.X*e.Y - e0.Y*e.X).Abs() < absoluteEpsilon && e.Dot(e0) > 0.0) ||
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
                while (u < count && ((e0.X * e1.Y - e0.Y * e1.X).Abs() < absoluteEpsilon && e.Dot(e0) < 0.0));

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

    public static class SubPrimitives
    {
        #region Triangulation of Convex Polygons

        /// <summary>
        /// Compute a simple triangulation starting from the first point of
        /// the polygon. Note that the indices are twice indirect (once over
        /// pia, once over via). Returns the number of triangles (ie. two
        /// less than the length of the array pia).
        /// </summary>
        public static int ComputeSimpleTriangulation(
                this int[] pia, int[] via, int tvi, int[] tia, int[] pvibm, int[] tibm)
        {
            int fi = pia[0], fvi = via[fi];
            int i1 = pia[1], vi1 = via[i1];
            int count = pia.Length;
            for (int ii = 2; ii < count; ii++)
            {
                int vi0 = vi1, i0 = i1;
                vi1 = via[i1 = pia[ii]];
                tia[tvi] = fvi;
                tia[tvi + 1] = vi0;
                tia[tvi + 2] = vi1;
                if (tibm != null)
                {
                    tibm[tvi] = pvibm[fi];
                    tibm[tvi + 1] = pvibm[i0];
                    tibm[tvi + 2] = pvibm[i1];
                }
                tvi += 3;
            }
            return count - 2;
        }

        #endregion

        #region Non-Concave Sub-Polygons of a Polygon2d

        private static int[] ComputeNonConcaveSubPolygon(
                this Polygon2d poly, ref int[] indices, double absoluteEpsilon)
        {
            int count = indices.Length;

            if (count <= 3)
            {
                var result = indices;
                indices = new int[0];
                return result;
            }

            Func<int, int> addFun = i => (i + 1) % count;
            Func<int, int> subFun = i => (i + count - 1) % count;

            for (int vi = 0; vi < count; vi++) // we possibly have to try all vertices
            {
                int nowMinIndex = vi, nowMaxIndex = vi;
                var nowMinPoint = poly[indices[nowMinIndex]];
                var nowMaxPoint = poly[indices[nowMaxIndex]];
                var oldMinPoint = poly[indices[addFun(nowMinIndex)]];
                var oldMaxPoint = poly[indices[subFun(nowMinIndex)]];
                var subCount = 1;

                bool add = true;

                for (int pass = 0, goodPass = 0; pass - goodPass < 3; pass++, add = !add)
                {
                    // alternate between adding a point at the two ends
                    var nextFun = add ? addFun : subFun;
                    int lastIndex = add ? nowMinIndex : nowMaxIndex;

                    int newIndex = add ? addFun(nowMaxIndex) : subFun(nowMinIndex);
                    if (newIndex == lastIndex)
                    {
                        var finalIndices = indices;
                        indices = new int[0];
                        return finalIndices;
                    }

                    var newPoint = poly[indices[newIndex]];
                    var pMax = new Line2d(oldMaxPoint, nowMaxPoint).Plane2d;
                    var pMin = new Line2d(oldMinPoint, nowMinPoint).Plane2d;

                    var npMax = pMax.Normalized;
                    var npMin = pMin.Normalized;

                    var hMax = new Line2d(oldMaxPoint, nowMaxPoint).Plane2d.Normalized.Height(newPoint);
                    var hMin = new Line2d(oldMinPoint, nowMinPoint).Plane2d.Normalized.Height(newPoint);

                    if (hMax >= -absoluteEpsilon && hMin <= absoluteEpsilon)
                    {
                        bool good = true;
                        if (nowMinIndex != nowMaxIndex)
                        {
                            var plane0 = new Line2d(nowMinPoint, nowMaxPoint).Plane2d.Normalized;
                            var plane1 = new Line2d(nowMaxPoint, newPoint).Plane2d.Normalized;
                            var plane2 = new Line2d(newPoint, nowMinPoint).Plane2d.Normalized;

                            // check if new triangle does not contain any other points of the polygon
                            for (int i = nextFun(newIndex); i != lastIndex; i = nextFun(i))
                            {
                                var point = poly[indices[i]];
                                if (plane0.Height(point) > -absoluteEpsilon
                                    && plane1.Height(point) >= absoluteEpsilon
                                    && plane2.Height(point) >= absoluteEpsilon)
                                { good = false; break; }
                            }
                        }
                        if (good)
                        {
                            if (add)
                            {
                                nowMaxIndex = newIndex; oldMaxPoint = nowMaxPoint; nowMaxPoint = newPoint;
                            }
                            else
                            {
                                nowMinIndex = newIndex; oldMinPoint = nowMinPoint; nowMinPoint = newPoint;
                            }
                            goodPass = pass;
                            ++subCount;
                        }
                    }
                }
                if (subCount < 3) continue;

                // build new arrays here

                var subIndices = new int[subCount];
                for (int i = 0, ni = nowMinIndex; i < subCount; i++, ni = addFun(ni))
                    subIndices[i] = indices[ni];
                int newCount = 2 + count - subCount;
                var newIndices = new int[newCount];
                for (int i = 0, ni = nowMaxIndex; i < newCount; i++, ni = addFun(ni))
                    newIndices[i] = indices[ni];

                indices = newIndices;
                return subIndices;
            }
            return null;
        }

        public static void AddNonConcaveSubPolygons(
                this List<int[]> polygonList, Polygon2d poly,
                double absoluteEpsilon)
        {
            var indices = new int[poly.PointCount].SetByIndex(i => i);
            while (indices.Length > 0)
            {
                var subPoly = poly.ComputeNonConcaveSubPolygon(
                                    ref indices, absoluteEpsilon);
                if (subPoly.Length < 3)
                {
                    Console.WriteLine("encountered invalid subpolygon");
                    continue;
                }
                polygonList.Add(subPoly);
            }
        }

        public static List<int[]> ComputeNonConcaveSubPolygons(
                this Polygon2d polygon, double absoluteEpsilon)
        {
            var polygonList = new List<int[]>();
            polygonList.AddNonConcaveSubPolygons(polygon, absoluteEpsilon);
            return polygonList;
        }

        #endregion

        #region Non-Concave Sub-Polygons of Polygons in 3d

        public static List<int[]> ComputeNonConcaveSubPolygons(
                this Polygon3d polygon, double absoluteEpsilon)
        {
            V3d normal = polygon.ComputeDoubleAreaNormal();
            double len2 = normal.LengthSquared;

            if (len2 < absoluteEpsilon * absoluteEpsilon)
                return new int[polygon.PointCount].SetByIndex(i => i).IntoList();

            M44d local2global, global2local;
            M44d.NormalFrame(V3d.Zero, normal * (1.0/Fun.Sqrt(len2)), out local2global, out global2local);
            var polygon2d = polygon.ToPolygon2d(p => global2local.TransformPos(p).XY);
            return polygon2d.ComputeNonConcaveSubPolygons(absoluteEpsilon);
        }

        public static List<int[]> ComputeNonConcaveSubPolygons(
                this V3d[] vertexArray, int polyCount,
                V3d[] normalArray, int[] firstIndices, int[] vertexIndices,
                List<int> faceBackMap,
                double absoluteEpsilon)
        {
            var polyList = new List<int[]>();
            var eps2 = absoluteEpsilon * absoluteEpsilon;

            for (int fvi = 0, fi = 0; fi < polyCount; fi++)
            {
                int fve = firstIndices[fi+1], fvc = fve - fvi;
                var n = normalArray[fi];
                var l2 = n.LengthSquared;
                if (l2 < eps2)
                {
                    polyList.Add(new int[fvc].SetByIndex(i => vertexIndices[fvi + i]));
                    continue;
                }
                M44d local2global, global2local;
                M44d.NormalFrame(V3d.Zero, n, out local2global, out global2local);
                var polygon = new Polygon2d(fvc,
                        i => global2local.TransformPos(vertexArray[vertexIndices[fvi + i]]).XY);
                var subPolyList = polygon.ComputeNonConcaveSubPolygons(absoluteEpsilon);
                foreach (var poly in subPolyList)
                    polyList.Add(poly.Copy(i => fvi + i));
                if (faceBackMap != null)
                    for (int i = 0; i < subPolyList.Count; i++)
                        faceBackMap.Add(fi);
                fvi = fve;
            }

            return polyList;
        }

        #endregion

        #region Triangulation of Non-Concave Polygons in 3d

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Triangle count.</returns>
        public static int ComputeTriangulationOfNonConcavePolygon(
                this int[] ia, int[] via, V3d[] va, V3d[] edgeArray, bool[] straightArray,
                int tvi, int[] tia, int[] pvibm, int[] tibm)
        {
            int count = ia.Length;

            if (count < 3) throw new ArgumentOutOfRangeException();

            if (count == 3)
            {
                tia[tvi] = via[ia[0]];
                tia[tvi + 1] = via[ia[1]];
                tia[tvi + 2] = via[ia[2]];
                if (tibm != null)
                {
                    tibm[tvi] = pvibm[ia[0]];
                    tibm[tvi + 1] = pvibm[ia[1]];
                    tibm[tvi + 2] = pvibm[ia[2]];
                }
                return 1;
            }

            int ii = 0;

            // search for the first non-straight corner
            while (ii < ia.Length && straightArray[ia[ii]]) ii++;

            int bii = ii; ii = (ii + 1) % count;

            // search for first straight corner afterwards
            while (ii != bii && !straightArray[ia[ii]]) ii = (ii + 1) % count;

            if (ii == bii) // if there is no straight corner
                return ia.ComputeSimpleTriangulation(via, tvi, tia, pvibm, tibm);
            bii = ii; // first
            int eii = ii;
            int straightCount = 0;
            while (straightArray[ia[ii]])
            {
                eii = ii; ii = (ii + 1) % count;
                ++straightCount;
            }

            if (count - straightCount < 3) // numeric problem, cop out
                return ia.ComputeSimpleTriangulation(via, tvi, tia, pvibm, tibm);

            var hii = ii; double height = 0.0;
            var li = ia[eii];
            var edge = edgeArray[li];
            var vertex = va[via[li]];

            int rightCount = 2;
            int rc = 2;
            ii = (ii + 1) % count;
            while (ii != bii)
            {
                int i = ia[ii];
                var h = edge.Cross(va[via[i]] - vertex).LengthSquared;
                ++rc;
                if (h > height)
                {
                    height = h;
                    hii = ii;
                    rightCount = rc;
                }
                ii = (ii + 1) % count;
            }

            straightArray[ia[bii]] = false;
            straightArray[ia[eii]] = false;
            straightArray[ia[hii]] = false;

            int leftCount = count - straightCount - rightCount + 3;

            var lia = new int[leftCount];
            ii = hii;
            for (int lii = 0; lii < leftCount; lii++, ii = (ii + 1) % count)
                lia[lii] = ia[ii];

            if (leftCount > 2)
                lia.ComputeTriangulationOfNonConcavePolygon(via, va, edgeArray, straightArray,
                        tvi, tia, pvibm, tibm);

            tvi += 3 * (leftCount - 2);

            if (straightCount > 1)
            {
                int hi = ia[hii], hvi = via[hi];
                int i1 = ia[bii], vi1 = via[i1];
                ii = bii;
                for (int sii = 0; sii < straightCount - 1; sii++)
                {
                    ii = (ii + 1) % count;
                    int vi0 = vi1, i0 = i1;
                    vi1 = via[i1 = ia[ii]];
                    tia[tvi] = hvi;
                    tia[tvi + 1] = vi0;
                    tia[tvi + 2] = vi1;

                    if (tibm != null)
                    {
                        tibm[tvi] = pvibm[hi];
                        tibm[tvi + 1] = pvibm[i0];
                        tibm[tvi + 2] = pvibm[i1];
                    }
                    tvi += 3;
                }
            }

            var ria = new int[rightCount];
            ii = eii;
            for (int rii = 0; rii < rightCount; rii++, ii = (ii + 1) % count)
                ria[rii] = ia[ii];

            if (rightCount > 2)
                ria.ComputeTriangulationOfNonConcavePolygon(via, va, edgeArray, straightArray,
                        tvi, tia, pvibm, tibm);

            return count - 2;
        }

        public static void ComputeStraightVertices(
                this int[] via, V3d[] va, double absoluteEpsilon,
                V3d[] normalizedEdgeArray, bool[] straightArray)
        {
            int count = via.Length;
            if (straightArray == null) throw new ArgumentNullException();
            if (count <= 2) throw new ArgumentException();
            if (straightArray.Length != count) throw new ArgumentException();
            if (normalizedEdgeArray != null && normalizedEdgeArray.Length != count) throw new ArgumentException();

            double eps2 = absoluteEpsilon * absoluteEpsilon;

            V3d[] ea = new V3d[count];
            int oi = count - 1;
            var oldVertex = va[via[oi]];

            for (int i = 0; i < count; i++)
            {
                var vertex = va[via[i]];
                ea[oi] = vertex - oldVertex;
                oi = i; oldVertex = vertex;
            }

            oi = count - 1;
            var oldEdge = ea[oi];

            for (int i = 0; i < count; i++)
            {
                var edge = ea[i];
                var normalizedEdge = oldEdge.Normalized;
                if (normalizedEdgeArray != null)
                    normalizedEdgeArray[oi] = normalizedEdge;
                straightArray[i] = normalizedEdge.Cross(edge).LengthSquared < eps2;
                oi = i; oldEdge = edge;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Triangle count.</returns>
        public static int ComputeTriangulationOfNonConcavePolygon(
                this V3d[] va, int[] via, double absoluteEpsilon,
                int ftvi, int[] tia, int[] pvibm = null, int[] tibm = null)
        {
            int count = via.Length;
            var straightArray = new bool[count];
            var normalizedEdgeArray = new V3d[count];

            via.ComputeStraightVertices(va, absoluteEpsilon,
                    normalizedEdgeArray, straightArray);

            var ia = new int[count].SetByIndex(i => i);

            return ia.ComputeTriangulationOfNonConcavePolygon(
                    via, va, normalizedEdgeArray, straightArray,
                    ftvi, tia, pvibm, tibm);
        }

        public static int[] ComputeTriangulationOfNonConcavePolygons(
                this V3d[] vertexArray, List<int[]> nonConcavePolyList,
                double absoluteEpsilon)
        {
            int tc = nonConcavePolyList.Sum(p => p.Length - 2);
            var tia = new int[tc];
            tc = 0;
            foreach (var pia in nonConcavePolyList)
            {
                tc += 3 * vertexArray.ComputeTriangulationOfNonConcavePolygon(
                                        pia, absoluteEpsilon, tc, tia);
            }
            return tia;
        }

        #endregion

        #region Triangulation of Concave Polygons

        /// <summary>
        /// Compute the triangulation of the supplied concave polygon and put
        /// the triangle vertex indices into the supplied array starting at
        /// the supplied triangle vertex index.
        /// </summary>
        /// <returns>the number of triangles in the triangulation</returns>        
        private static int ComputeTriangulationOfConcavePolygon(
                this Polygon3d polygon, double absoluteEpsilon,
                int triangleVertexIndex, int[] triangleIndexArray)
        {
            var polyList = polygon.ComputeNonConcaveSubPolygons(absoluteEpsilon);

            foreach (var pia in polyList)
            {
                int tc = polygon.GetPointArray().ComputeTriangulationOfNonConcavePolygon(
                                            pia, absoluteEpsilon, triangleVertexIndex,
                    triangleIndexArray);
                triangleVertexIndex += 3 * tc;
            }
            return polygon.PointCount - 2;
        }

        /// <summary>
        /// Compute the triangulation of the supplied concave polygon and
        /// return the resulting triangle index array.
        /// </summary>
        public static int[] ComputeTriangulationOfConcavePolygon(
                this Polygon3d polygon, double absoluteEpsilon)
        {
            int[] triangleIndexArray = new int[3 * (polygon.PointCount - 2)];
            polygon.ComputeTriangulationOfConcavePolygon(
                        absoluteEpsilon, 0, triangleIndexArray);
            return triangleIndexArray;
        }

        /// <summary>
        /// Compute the triangulation of the concave polygons supplied in
        /// the arrays with PolyMesh layout, and return the resulting
        /// triangle index array.
        /// </summary>
        public static int[] ComputeTriangulationOfConcavePolygons(
                this V3d[] vertexArray, int polyCount,
                V3d[] normalArray, int[] firstIndices, int[] vertexIndices,
                double absoluteEpsilon)
        {
            var polyList = vertexArray.ComputeNonConcaveSubPolygons(
                                polyCount, normalArray, firstIndices,
                                vertexIndices, null, absoluteEpsilon);
            return vertexArray.ComputeTriangulationOfNonConcavePolygons(
                                polyList, absoluteEpsilon);
        }

        #endregion
    }

}

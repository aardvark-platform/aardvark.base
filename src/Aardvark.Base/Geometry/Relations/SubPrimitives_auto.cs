using System;
using System.Linq;
using System.Collections.Generic;

namespace Aardvark.Base
{
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

        #region Non-Concave Sub-Polygons of a Polygon2f

        private static int[] ComputeNonConcaveSubPolygon(
                this Polygon2f poly, ref int[] indices, float absoluteEpsilon)
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
                    var pMax = new Line2f(oldMaxPoint, nowMaxPoint).Plane2f;
                    var pMin = new Line2f(oldMinPoint, nowMinPoint).Plane2f;

                    var npMax = pMax.Normalized;
                    var npMin = pMin.Normalized;

                    var hMax = new Line2f(oldMaxPoint, nowMaxPoint).Plane2f.Normalized.Height(newPoint);
                    var hMin = new Line2f(oldMinPoint, nowMinPoint).Plane2f.Normalized.Height(newPoint);

                    if (hMax >= -absoluteEpsilon && hMin <= absoluteEpsilon)
                    {
                        bool good = true;
                        if (nowMinIndex != nowMaxIndex)
                        {
                            var plane0 = new Line2f(nowMinPoint, nowMaxPoint).Plane2f.Normalized;
                            var plane1 = new Line2f(nowMaxPoint, newPoint).Plane2f.Normalized;
                            var plane2 = new Line2f(newPoint, nowMinPoint).Plane2f.Normalized;

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
                this List<int[]> polygonList, Polygon2f poly,
                float absoluteEpsilon)
        {
            var indices = new int[poly.PointCount].SetByIndex(i => i);
            while (indices.Length > 0)
            {
                var subPoly = poly.ComputeNonConcaveSubPolygon(
                                    ref indices, absoluteEpsilon);

                if (subPoly == null)
                {
                    Console.WriteLine("encountered degenerated polygon that cannot be easily triangulated");
                    break;
                }

                if (subPoly.Length < 3)
                {
                    Console.WriteLine("encountered invalid subpolygon");
                    continue;
                }
                polygonList.Add(subPoly);
            }
        }

        public static List<int[]> ComputeNonConcaveSubPolygons(
                this Polygon2f polygon, float absoluteEpsilon)
        {
            var polygonList = new List<int[]>();
            polygonList.AddNonConcaveSubPolygons(polygon, absoluteEpsilon);
            return polygonList;
        }

        #endregion

        #region Non-Concave Sub-Polygons of Polygons in 3d

        public static List<int[]> ComputeNonConcaveSubPolygons(
                this Polygon3f polygon, float absoluteEpsilon)
        {
            V3f normal = polygon.ComputeDoubleAreaNormal();
            float len2 = normal.LengthSquared;

            if (len2 < absoluteEpsilon * absoluteEpsilon)
                return new int[polygon.PointCount].SetByIndex(i => i).IntoList();

            M44f.NormalFrame(V3f.Zero, normal * (1 / Fun.Sqrt(len2)), out M44f local2global, out M44f global2local);
            var polygon2d = polygon.ToPolygon2f(p => global2local.TransformPos(p).XY);
            return polygon2d.ComputeNonConcaveSubPolygons(absoluteEpsilon);
        }

        public static List<int[]> ComputeNonConcaveSubPolygons(
                this V3f[] vertexArray, int polyCount,
                V3f[] normalArray, int[] firstIndices, int[] vertexIndices,
                List<int> faceBackMap,
                float absoluteEpsilon)
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
                    if (faceBackMap != null)
                        faceBackMap.Add(fi);
                    fvi = fve;
                    continue;
                }
                M44f.NormalFrame(V3f.Zero, n, out M44f local2global, out M44f global2local);
                var polygon = new Polygon2f(fvc,
                        i => global2local.TransformPos(vertexArray[vertexIndices[fvi + i]]).XY);
                var subPolyList = polygon.ComputeNonConcaveSubPolygons(absoluteEpsilon);
                foreach (var poly in subPolyList)
                    polyList.Add(poly.Map(i => fvi + i));
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
                this int[] ia, int[] via, V3f[] va, V3f[] edgeArray, bool[] straightArray,
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

            var hii = ii; float height = 0;
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
                this int[] via, V3f[] va, float absoluteEpsilon,
                V3f[] normalizedEdgeArray, bool[] straightArray)
        {
            int count = via.Length;
            if (straightArray == null) throw new ArgumentNullException();
            if (count <= 2) throw new ArgumentException();
            if (straightArray.Length != count) throw new ArgumentException();
            if (normalizedEdgeArray != null && normalizedEdgeArray.Length != count) throw new ArgumentException();

            float eps2 = absoluteEpsilon * absoluteEpsilon;

            V3f[] ea = new V3f[count];
            int oi = count - 1;
            var oldVertex = va[via[oi]];

            for (int i = 0; i < count; i++)
            {
                var vertex = va[via[i]];
                ea[oi] = vertex - oldVertex;
                oi = i; oldVertex = vertex;
            }

            oi = count - 1;
            var oldEdge = ea[oi].Normalized;

            for (int i = 0; i < count; i++)
            {
                var edge = ea[i].Normalized;
                if (normalizedEdgeArray != null)
                    normalizedEdgeArray[oi] = oldEdge;
                straightArray[i] = (edge - oldEdge).LengthSquared < eps2;
                oi = i; oldEdge = edge;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>Triangle count.</returns>
        public static int ComputeTriangulationOfNonConcavePolygon(
                this V3f[] va, int[] via, float absoluteEpsilon,
                int ftvi, int[] tia, int[] pvibm = null, int[] tibm = null)
        {
            int count = via.Length;
            var straightArray = new bool[count];
            var normalizedEdgeArray = new V3f[count];

            via.ComputeStraightVertices(va, absoluteEpsilon,
                    normalizedEdgeArray, straightArray);

            var ia = new int[count].SetByIndex(i => i);

            return ia.ComputeTriangulationOfNonConcavePolygon(
                    via, va, normalizedEdgeArray, straightArray,
                    ftvi, tia, pvibm, tibm);
        }

        public static int[] ComputeTriangulationOfNonConcavePolygons(
                this V3f[] vertexArray, List<int[]> nonConcavePolyList,
                float absoluteEpsilon)
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
                this Polygon3f polygon, float absoluteEpsilon,
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
                this Polygon3f polygon, float absoluteEpsilon)
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
                this V3f[] vertexArray, int polyCount,
                V3f[] normalArray, int[] firstIndices, int[] vertexIndices,
                float absoluteEpsilon)
        {
            var polyList = vertexArray.ComputeNonConcaveSubPolygons(
                                polyCount, normalArray, firstIndices,
                                vertexIndices, null, absoluteEpsilon);
            return vertexArray.ComputeTriangulationOfNonConcavePolygons(
                                polyList, absoluteEpsilon);
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

                if (subPoly == null)
                {
                    Console.WriteLine("encountered degenerated polygon that cannot be easily triangulated");
                    break;
                }

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

            M44d.NormalFrame(V3d.Zero, normal * (1 / Fun.Sqrt(len2)), out M44d local2global, out M44d global2local);
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
                    if (faceBackMap != null)
                        faceBackMap.Add(fi);
                    fvi = fve;
                    continue;
                }
                M44d.NormalFrame(V3d.Zero, n, out M44d local2global, out M44d global2local);
                var polygon = new Polygon2d(fvc,
                        i => global2local.TransformPos(vertexArray[vertexIndices[fvi + i]]).XY);
                var subPolyList = polygon.ComputeNonConcaveSubPolygons(absoluteEpsilon);
                foreach (var poly in subPolyList)
                    polyList.Add(poly.Map(i => fvi + i));
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

            var hii = ii; double height = 0;
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
            var oldEdge = ea[oi].Normalized;

            for (int i = 0; i < count; i++)
            {
                var edge = ea[i].Normalized;
                if (normalizedEdgeArray != null)
                    normalizedEdgeArray[oi] = oldEdge;
                straightArray[i] = (edge - oldEdge).LengthSquared < eps2;
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

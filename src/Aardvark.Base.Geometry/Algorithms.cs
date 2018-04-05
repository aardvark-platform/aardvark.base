using Aardvark.Base.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static partial class GeometryFun
    {
        #region Bresenham

        /// <summary>
        /// Iterates along Bresenham Discrete Line Raster from P0 to P1. 
        /// Yields each integer position V2i.
        /// </summary>
        public static IEnumerable<V2i> BresenhamLineIterator(V2i p0, V2i p1)
        {
            int x0 = p0.X, y0 = p0.Y, x1 = p1.X, y1 = p1.Y;
            int dx, dy;
            int incx, incy;
            int balance;

            if (x1 >= x0)
            {
                dx = x1 - x0;
                incx = 1;
            }
            else
            {
                dx = x0 - x1;
                incx = -1;
            }

            if (y1 >= y0)
            {
                dy = y1 - y0;
                incy = 1;
            }
            else
            {
                dy = y0 - y1;
                incy = -1;
            }

            if (dx >= dy)
            {
                dy <<= 1;
                balance = dy - dx;
                dx <<= 1;

                while (x0 != x1)
                {
                    yield return new V2i(x0, y0);
                    if (balance >= 0)
                    {
                        y0 += incy;
                        balance -= dx;
                    }
                    balance += dy;
                    x0 += incx;
                }

                yield return new V2i(x0, y0);
            }
            else
            {
                dx <<= 1;
                balance = dx - dy;
                dy <<= 1;

                while (y0 != y1)
                {
                    yield return new V2i(x0, y0);
                    if (balance >= 0)
                    {
                        x0 += incx;
                        balance -= dy;
                    }
                    balance += dx;
                    y0 += incy;
                }

                yield return new V2i(x0, y0);
            }
        }

        /// <summary>
        /// Iterates along Bresenham discrete line raster from p0 to p1. 
        /// Yields each integer position V2l.
        /// </summary>
        public static IEnumerable<V2l> BresenhamLineIterator(V2l p0, V2l p1)
        {
            long x0 = p0.X, y0 = p0.Y, x1 = p1.X, y1 = p1.Y;
            long dx, dy;
            long incx, incy;
            long balance;

            if (x1 >= x0)
            {
                dx = x1 - x0;
                incx = 1;
            }
            else
            {
                dx = x0 - x1;
                incx = -1;
            }

            if (y1 >= y0)
            {
                dy = y1 - y0;
                incy = 1;
            }
            else
            {
                dy = y0 - y1;
                incy = -1;
            }

            if (dx >= dy)
            {
                dy <<= 1;
                balance = dy - dx;
                dx <<= 1;

                while (x0 != x1)
                {
                    yield return new V2l(x0, y0);
                    if (balance >= 0)
                    {
                        y0 += incy;
                        balance -= dx;
                    }
                    balance += dy;
                    x0 += incx;
                }

                yield return new V2l(x0, y0);
            }
            else
            {
                dx <<= 1;
                balance = dx - dy;
                dy <<= 1;

                while (y0 != y1)
                {
                    yield return new V2l(x0, y0);
                    if (balance >= 0)
                    {
                        x0 += incx;
                        balance -= dy;
                    }
                    balance += dx;
                    y0 += incy;
                }

                yield return new V2l(x0, y0);
            }
        }
    
        #endregion

        #region Convex Hull in 2D

        /// <summary>
        /// Returns those indices of the supplied index array to the supplied
        /// points set that constitute the convex hull. The returned indices
        /// emumerate the convex hull in counter-clockwise fashion.
        /// Implemented via Andrew's monotone chain 2d convex hull algorithm.
        /// </summary>
        public static IndexPolygon2d ConvexHullIndexPolygon(
                this int[] indexArray, V2d[] pointArray)
        {
            var sortedIndex = indexArray.Copy();
            sortedIndex.PermutationQuickSort(pointArray, V2d.LexicalCompare);
            return ConvexHullIndexPolygonOfSortedIndexArray(sortedIndex, pointArray);
        }

        /// <summary>
        /// Returns the indices of the points of the supplied point set that
        /// constitute the convex hull. The returned indices emumerate the
        /// convex hull in counter-clockwise fashion. Implemented via Andrew's
        /// monotone chain 2d convex hull algorithm.
        /// </summary>
        public static IndexPolygon2d ConvexHullIndexPolygon(this V2d[] pointArray, int pointCount = 0)
        {
            if (pointCount == 0) pointCount = pointArray.Length;
            var sortedIndex = new int[pointCount].SetByIndex(i => i);
            sortedIndex.PermutationQuickSort(pointArray, V2d.LexicalCompare);
            return ConvexHullIndexPolygonOfSortedIndexArray(sortedIndex, pointArray);
        }

        private static IndexPolygon2d ConvexHullIndexPolygonOfSortedIndexArray(int[] idx, V2d[] p)
        {
            // Copyright 2001, softSurfer (www.softsurfer.com)
            // This code may be freely used and modified for any purpose
            // providing that this copyright notice is included with it.
            // SoftSurfer makes no warranty for this code, and cannot be held
            // liable for any real or imagined damage resulting from its use.
            // Users of this code must verify correctness for their application.

            int n = idx.Length;
            var hull = new int[n + 1];
            // the output array hull[] will be used as the stack
            int bot = 0, top = -1;  // indices for bottom and top of the stack
            int i;                // array scan index

            // Get the indices of points with min x-coord and min|max y-coord
            int minmin = 0, minmax;
            double xmin = p[idx[0]].X;
            for (i = 1; i < n; i++)
                if (p[idx[i]].X != xmin) break;
            minmax = i - 1;
            if (minmax == n - 1)
            {       // degenerate case: all x-coords == xmin
                hull[++top] = idx[minmin];
                if (p[idx[minmax]].Y != p[idx[minmin]].Y) // a nontrivial segment
                    hull[++top] = idx[minmax];
                return new IndexPolygon2d(hull, 0, top + 1, p);
            }

            // get the indices of points with max x-coord and min|max y-coord
            int maxmin, maxmax = n - 1;
            double xmax = p[idx[maxmax]].X;
            for (i = n - 2; i >= 0; i--)
                if (p[idx[i]].X != xmax) break;
            maxmin = i + 1;

            // compute the lower hull on the stack hull
            hull[++top] = idx[minmin]; // push minmin point onto stack
            i = minmax;
            var pimaxmin = p[idx[maxmin]];
            var phtop = p[hull[top]];
            while (++i <= maxmin)
            {
                int ii = idx[i]; var pii = p[ii];
                // ignore points above or on line p[idx[minmin], p[idx[maxmin]]
                if (pii.PosLeftOfLineValue(phtop, pimaxmin) >= 0 && i < maxmin) continue;
                while (top > 0)         // at least 2 points on the stack
                {
                    // test if p[ii] is left of the line at the stack top
                    if (pii.PosLeftOfLineValue(p[hull[top - 1]], p[hull[top]]) > 0)
                        break;          // p[ii] is a new hull vertex
                    top--;              // pop top point off stack
                }
                hull[++top] = ii;       // push p[ii] onto stack
                phtop = pii;
            }

            // compute the upper hull on the stack hull above the bottom hull
            if (maxmax != maxmin)      // if distinct xmax points
                hull[++top] = idx[maxmax]; // push maxmax point onto stack
            bot = top;                  // the bottom point of the upper hull stack
            i = maxmin;
            var piminmax = p[idx[minmax]];
            phtop = p[hull[top]];
            while (--i >= minmax)
            {
                int ii = idx[i]; var pii = p[ii];
                // ignore points below the line p[idx[maxmax]], p[idx[minmax]]
                if (pii.PosLeftOfLineValue(phtop, piminmax) >= 0 && i > minmax) continue;
                while (top > bot)       // at least 2 points on the upper stack
                {
                    // test if p[ii] is left of the line at the stack top
                    if (pii.PosLeftOfLineValue(p[hull[top - 1]], p[hull[top]]) > 0)
                        break;          // p[ii] is a new hull vertex
                    top--;              // pop top point off stack
                }
                hull[++top] = ii;       // push p[ii] onto stack
                phtop = pii;
            }
            if (minmax != minmin)
                ++top;

            return new IndexPolygon2d(hull, 0, top, p);
        }

        #endregion

        #region Ramer–Douglas–Peucker

        /// <summary>
        /// Returns those indices of the supplied polyline that constitute
        /// the simplified polyline. A larger epsilon results in a simpler polyline.
        /// Implemented via the Ramer–Douglas–Peucker algorithm.
        /// http://en.wikipedia.org/wiki/Ramer-Douglas-Peucker_algorithm
        /// </summary>
        public static int[] Simplify(this V2d[] polyline, double epsilon)
        {
            return Simplify(polyline, epsilon, 0, polyline.Length - 1);
        }
        private static int[] Simplify(V2d[] polyline, double eps, int indexFirst, int indexLast)
        {
            if (indexFirst == indexLast - 1) return new[] { indexFirst, indexLast };

            var pFirst = polyline[indexFirst]; var pLast = polyline[indexLast];
            var line = new Line2d(pFirst, pLast);

            var distMax = 0.0; var indexMax = 0;
            for (var i = indexFirst + 1; i < indexLast; i++)
            {
                var d = line.GetDistanceToLine(polyline[i]);
                if (d > distMax) { distMax = d; indexMax = i; }
            }

            if (distMax < eps) return new[] { indexFirst, indexLast };

            var left = Simplify(polyline, eps, indexFirst, indexMax);
            var right = Simplify(polyline, eps, indexMax, indexLast);

            var result = new int[left.Length + right.Length - 1];
            left.CopyTo(result, 0);
            right.CopyTo(1, right.Length - 1, result, left.Length);
            return result;
        }

        #endregion

        #region Delaunay Triangulation O(n^2/3)

        /// <summary>
        /// Compute the Delaunay triangulation of the supplied x-sorted point
        /// array in the XY-plane and return a triangle index array with
        /// counter-clockwise triangles.
        /// </summary>
        public static int[] TriangulateXY(this V3d[] xSortedPoints)
        {
            int vc = xSortedPoints.Length;
            V2d[] pxy = new V2d[vc + 4].SetByIndexLong(0, vc, i => xSortedPoints[i].XY);
            return Triangulate2d(pxy);
        }

        /// <summary>
        /// Compute the Delaunay triangulation of the supplied x-sorted point
        /// array and return a triangle index array with counter-clockwise
        /// triangles.
        /// </summary>
        public static int[] Triangulate(this V2d[] xSortedPoints)
        {
            int vc = xSortedPoints.Length;
            V2d[] pxy = new V2d[vc + 4].SetByIndexLong(0, vc, i => xSortedPoints[i]);
            return Triangulate2d(pxy);
        }

        /// <summary>
        /// Internal wrapper around the Delaunay triangulation that constructs
        /// the triangle index array.
        /// </summary>
        private static int[] Triangulate2d(V2d[] pa)
        {
            Triangle1i[] tri;
            int tc;
            Triangulate2d(pa, out tri, out tc);
            int[] ia = new int[3 * tc];
            int i = 0;
            for (int ti = 0; ti < tc; ti++)
            {
                var tr = tri[ti];
                ia[i++] = tr.I0; ia[i++] = tr.I1; ia[i++] = tr.I2;
            }
            return ia;
        }

        /// <summary>
        /// Compute the Delaunay triangluation of the supplied points. Note that
        /// the supplied point array must be by 3 larger than the actual number of
        /// points.
        /// </summary>
        private static void Triangulate2d(V2d[] pa,
            out Triangle1i[] ta, out int triangleCount)
        {
            int vc = pa.Length - 4;
            int tcMax = 2 * vc + 2; // sharp upper bound with no degenerates
            int ecMax = 6 * vc - 3; // sharp bound: last step replaces all tris

            ta = new Triangle1i[tcMax];
            var ra = new double[tcMax];
            var ca = new V2d[tcMax];
            var ea = new Line1i[ecMax];

            // -------------------------------------- set up the supertriangle
            ta[0] = new Triangle1i(vc, vc + 2, vc + 1);
            ra[0] = -1.0;
            ta[1] = new Triangle1i(vc, vc + 3, vc + 2);
            ra[1] = -1.0;
            int tc = 2, cc = 0; // triangle count, complete count

            // ------------- superquad vertices at the end of vertex array
            var box = new Box2d(pa.Take(vc)).EnlargedBy(0.1);
            V2d center = box.Center, size = box.Size;
            pa[vc + 0] = box.Min;
            pa[vc + 1] = new V2d(box.Min.X, box.Max.Y);
            pa[vc + 2] = box.Max;
            pa[vc + 3] = new V2d(box.Max.X, box.Min.Y); 

            // ------------------------------ include the points one at a time
            for (int i = 0; i < vc; i++)
            {
                V2d p = pa[i];
                int ec = 0;
                /*
                   if the point lies inside the circumcircle then the triangle
                   is removed and its edges are added to the edge array
                */
                for (int ti = cc; ti < tc; ti++)
                {
                    var tr = ta[ti];
                    double r2 = ra[ti];
                    if (r2 < 0.0)
                    {
                        Triangle2d.ComputeCircumCircleSquared(
                                pa[tr.I0], pa[tr.I1], pa[tr.I2],
                                out center, out r2);
                        ra[ti] = r2; ca[ti] = center;
                    }
                    else
                        center = ca[ti];

                    // ---------------- include this if points are sorted by X
                    if (center.X < p.X && (p.X - center.X).Square() > r2)
                    {
                        Fun.Swap(ref ta[ti], ref ta[cc]);
                        ra[ti] = ra[cc]; ca[ti] = ca[cc];
                        ++cc; continue;
                    }
                    if (V2d.DistanceSquared(p, center) <= r2)
                    {
                        int nec = ec + 3;
                        if (nec >= ecMax)
                        {
                            ecMax = Fun.Max(nec, (int)(1.1 * (double)ecMax));
                            Array.Resize(ref ea, ecMax);
                        }
                        ea[ec] = tr.Line01; ea[ec + 1] = tr.Line12; ea[ec + 2] = tr.Line20;
                        --tc; ec = nec;
                        ta[ti] = ta[tc]; ra[ti] = ra[tc]; ca[ti] = ca[tc];
                        --ti;
                    }
                }

                // ---------------------------------------- tag multiple edges
                for (int ei = 0; ei < ec - 1; ei++)
                    for (int ej = ei + 1; ej < ec; ej++)
                        if (ea[ei].I0 == ea[ej].I1 && ea[ei].I1 == ea[ej].I0)
                        {
                            ea[ei] = Line1i.Invalid; ea[ej] = Line1i.Invalid;
                        }

                // ------------------ form new triangles for the current point
                for (int ei = 0; ei < ec; ei++)
                {
                    var e = ea[ei];
                    if (e.I0 < 0 || e.I1 < 0) continue; // skip tagged edges
                    if (tc >= tcMax) // necessary for degenerate cases
                    {
                        tcMax = Fun.Max(tcMax + 1, (int)(1.1 * (double)tcMax));
                        Array.Resize(ref ta, tcMax);
                        Array.Resize(ref ra, tcMax);
                        Array.Resize(ref ca, tcMax);
                    }
                    ta[tc] = new Triangle1i(e.I0, e.I1, i);
                    ra[tc++] = -1.0;
                }
            }

            // ------------------ remove triangles with supertriangle vertices
            for (int ti = 0; ti < tc; ti++)
                if (ta[ti].I0 >= vc || ta[ti].I1 >= vc || ta[ti].I2 >= vc)
                    ta[ti--] = ta[--tc];

            triangleCount = tc;
        }

        #endregion

        #region Box containing Percentile of points

        /// <summary>
        /// Compute a box containing approximately the supplied percentile
        /// of points, by performing a separate sorting operation on each
        /// coordinate.
        /// </summary>
        public static Box3d ComputeBoxContaining(
                this IEnumerable<V3d> points, double percentile)
        {
            if (percentile < 0 || percentile > 1) throw new ArgumentOutOfRangeException();
            var pa = points.ToArray();
            long offset = (long)(pa.Length * percentile) / 2;
            long mid = pa.Length / 2;
            long lo = mid - offset, hi = mid + offset;

            var xa = pa.Map(v => v.X); xa.QuickSortAscending();
            var ya = pa.Map(v => v.Y); ya.QuickSortAscending();
            var za = pa.Map(v => v.Z); za.QuickSortAscending();

            return new Box3d(new V3d(xa[lo], ya[lo], za[lo]),
                             new V3d(xa[hi], ya[hi], za[hi]));
        }

        #endregion
    }
}

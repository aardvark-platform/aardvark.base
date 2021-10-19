using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static partial class GeometryFun
    {
        #region Helpers (float)

        public static bool ClosestIntersection(this Polygon2f poly, V2f start, V2f end, out bool enter, out int otherIndex, out V2f p)
        {
            float min = float.MaxValue;

            bool intersects = false;

            V2f point = V2f.NaN;

            bool tempenter = false;
            int tempindex = -1;
            V2f temppoint = V2f.NaN;
            float templength = float.MaxValue;

            Line2f line = new Line2f(start, end);

            int i = 0;
            foreach (var l in poly.EdgeLines)
            {
                if (line.Intersects(l, 4 * float.Epsilon, out temppoint))
                {
                    templength = (temppoint - line.P0).Length;
                    if (templength < min)
                    {
                        tempenter = l.LeftValueOfDir(line.Direction) >= 0;
                        tempindex = i;
                        min = templength;
                        point = temppoint;
                    }

                    intersects = true;
                }
                i++;
            }

            enter = tempenter;
            otherIndex = tempindex;
            p = point;
            return intersects;
        }

        internal static float Closest(this List<V2f> points, V2f p)
        {
            float temp;
            float min = float.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                temp = (p - points[i]).Length;
                if (temp < min) min = temp;
            }

            return min;
        }

        internal static bool Intersects(V2f[] poly, V2f p0, V2f p1, ref List<V2f> known, out bool enter, out int index0, out V2f p)
        {
            int index = 0;

            Line2f l;
            int i0 = 0;
            int i1 = 1;
            for (int i = 1; i <= poly.Length; i++)
            {
                if (i < poly.Length)
                {
                    i0 = i - 1;
                    i1 = i;
                }
                else
                {
                    i0 = poly.Length - 1;
                    i1 = 0;
                }

                l = new Line2f(poly[i0], poly[i1]);
                if (l.Intersects(new Line2f(p0, p1), 4 *float.Epsilon, out p))
                {
                    if (!Fun.IsTiny(known.Closest(p)))
                    {
                        enter = (l.LeftValueOfPos(p1) >= -4 *float.Epsilon);
                        index0 = index;

                        return true;
                    }
                }

                index++;
            }

            p = V2f.NaN;
            index0 = -1;
            enter = false;
            return false;
        }

        internal static IEnumerable<V2f> AllVertices(Polygon2f poly)
        {
            int pc = poly.PointCount;
            for (int pi = 0; pi < pc; pi++) yield return poly[pi];
            yield return poly[0];
        }

        #endregion

        #region Intersections (float)

        #region Line2f - Polygon2f

        /// <summary>
        /// Returns the Line-Segments of line inside the Polygon (CCW ordered).
        /// Works with all (convex and non-convex) Polygons
        /// </summary>
        public static IEnumerable<Line2f> ClipWith(this Line2f line, Polygon2f poly)
        {
            bool i0, i1;

            i0 = poly.Contains(line.P0);
            i1 = poly.Contains(line.P1);

            var resulting = new List<V2f>();
            var enter = new List<bool>();

            if (i0)
            {
                resulting.Add(line.P0);
                enter.Add(true);
            }
            if (i1)
            {
                resulting.Add(line.P1);
                enter.Add(false);
            }

            var p = V2f.NaN;
            var direction = line.Direction;

            foreach (var l in poly.EdgeLines)
            {
                if (line.Intersects(l, out p))
                {
                    var d = l.Direction;
                    var n = new V2f(-d.Y, d.X);

                    if (!p.IsNaN)
                    {
                        bool addflag = true;
                        bool flag = direction.Dot(n) > 0;

                        for (int i = 0; i < resulting.Count; i++)
                        {
                            if (Fun.IsTiny((resulting[i] - p).Length))
                            {
                                if (flag != enter[i])
                                {
                                    resulting.RemoveAt(i);
                                    enter.RemoveAt(i);
                                }

                                addflag = false;
                                break;
                            }
                        }

                        if (addflag)
                        {
                            resulting.Add(p);
                            enter.Add(flag);
                        }
                    }
                }
            }

            var dir = line.P1 - line.P0;
            resulting = (from r in resulting select r).OrderBy(x => x.Dot(dir)).ToList();

            var counter = resulting.Count;
            var lines = new List<Line2f>();
            for (int i = 0; i < counter - 1; i += 2)
            {
                lines.Add(new Line2f(resulting[i], resulting[i + 1]));
            }
            return lines;
        }

        /// <summary>
        /// Returns the Line-Segments of line inside the Polygon (CCW ordered).
        /// Works only with Convex-Polygons
        /// If the line is clipped entirely, the points of the returned Line2f are NaN.
        /// </summary>
        public static Line2f ClipWithConvex(this Line2f line, Polygon2f poly)
        {
            var p = V2f.NaN;
            bool i0, i1;

            i0 = poly.Contains(line.P0);
            i1 = poly.Contains(line.P1);

            if (i0 && i1) return line;
            else if ((!i0 && i1) || (i0 && !i1))
            {
                foreach (var l in poly.EdgeLines)
                {
                    if (line.Intersects(l, out p)) break;
                }

                if (i0) return new Line2f(line.P0, p);
                else return new Line2f(p, line.P1);
            }
            else
            {
                V2f p0 = V2f.NaN;
                V2f p1 = V2f.NaN;
                int c = 0;

                foreach (var l in poly.EdgeLines)
                {
                    if (line.Intersects(l, out p))
                    {
                        if (c == 0) p0 = p;
                        else p1 = p;
                        c++;
                    }
                }

                if (c == 2)
                {
                    V2f u = p1 - p0;

                    if (u.Dot(line.Direction) > 0) return new Line2f(p0, p1);
                    else return new Line2f(p1, p0);
                }
                else return new Line2f(V2f.NaN, V2f.NaN);
            }
        }

        #endregion

        #region Polygon2f - Polygon2f

        /// <summary>
        /// Clips one Polygon with an other. Returns a Polygon representing the Intersection of the two.
        /// Works only with Convex Polygons. (For non-convex Polygons use Polygon2f.ClipWith(Polygon2f))
        /// !!!! UNTESTED !!!!
        /// </summary>
        public static Polygon2f ClippedByConvex(this Polygon2f poly, Polygon2f second)
        {
            V2f[][] arr = new V2f[2][];
            arr[0] = AllVertices(poly).ToArray();
            arr[1] = AllVertices(second).ToArray();

            Polygon2f[] polygon = new Polygon2f[2];
            polygon[0] = poly;
            polygon[1] = second;

            bool current = false;
            int currentIndex = 0;
            int otherIndex = 1;

            bool enter = false;
            int index = -1;
            V2f point = V2f.NaN;

            List<V2f> points = new List<V2f>();

            for (int i = 1; i < arr[currentIndex].Length; i = (i + 1) % arr[currentIndex].Length)
            {
                if (i < 1) i = 1;
                currentIndex = (current ? 1 : 0);
                otherIndex = (current ? 0 : 1);

                V2f start = arr[currentIndex][i - 1];
                V2f end = arr[currentIndex][i];

                if (Intersects(arr[otherIndex], start, end, ref points, out enter, out index, out point))
                {
                    if (enter)
                    {
                        points.Add(point);
                        int u = i;
                        int count = 0;
                        while (polygon[otherIndex].Contains(arr[currentIndex][u]))
                        {
                            points.Add(arr[currentIndex][u]);
                            u = (u + arr[currentIndex].Length - 1) % arr[currentIndex].Length;
                            count++;
                        }
                        if (count == 0) i--;
                        else
                        {
                            i += count - 1;
                        }
                    }
                    else
                    {
                        points.Add(point);
                        i = index;
                        current = !current;
                        currentIndex = (current ? 1 : 0);
                        otherIndex = (current ? 0 : 1);
                    }
                }
                else
                {

                    if (i < arr[currentIndex].Length && polygon[otherIndex].Contains(arr[currentIndex][i]))
                    {
                        if (Closest(points, arr[currentIndex][i]) < 0.001f) break;
                        points.Add(arr[currentIndex][i]);
                    }
                }
            }


            return new Polygon2f(points);
        }

        /// <summary>
        /// Clips one Polygon with an other. Returns a Polygon-Set representing the Intersections of the two.
        /// Works only with all (convex/concave) Polygons
        /// !!!! UNTESTED !!!!
        /// </summary>
        public static IEnumerable<Polygon2f> ClippedBy(this Polygon2f p0, Polygon2f p1, float relativeEpsilon)
        {
            List<int[]> i0 = SubPrimitives.ComputeNonConcaveSubPolygons(p0, relativeEpsilon);
            List<int[]> i1 = SubPrimitives.ComputeNonConcaveSubPolygons(p1, relativeEpsilon);

            List<Polygon2f> polys = new List<Polygon2f>();
            for (int i = 0; i < i0.Count; i++)
            {
                for (int u = 0; u < i1.Count; u++)
                {
                    Polygon2f q0 = new Polygon2f(from o in i0[i] select p0[o]);
                    Polygon2f q1 = new Polygon2f(from o in i1[u] select p1[o]);

                    Polygon2f r = q0.ClippedByConvex(q1);
                    if (r.PointCount > 0) polys.Add(r);
                }
            }

            return Union(polys);

        }

        #endregion

        #endregion

        #region Unions (float)

        #region Polygon2f - Polygon2f


        public static IEnumerable<Polygon2f> Union(List<Polygon2f> p)
        {
            List<Polygon2f> polys = new List<Polygon2f>();
            polys.Add(p[0]);
            List<Polygon2f> temp;

            for (int i = 1; i < p.Count; i++)
            {
                bool found = false;
                for (int u = 0; u < polys.Count; u++)
                {
                    temp = polys[u].Union(p[i]).ToList();

                    if (temp.Count == 1)
                    {
                        polys[u] = temp[0];
                        found = true;
                    }
                }

                if (!found)
                {
                    polys.Add(p[i]);
                }
            }

            return polys;
        }

        /// <summary>
        /// Returns the unified Polygon of the two given ones.
        /// Works only with convex-Polygons
        /// Returns an empty Polygon if the two do not intersect
        /// </summary>
        public static IEnumerable<Polygon2f> Union(this Polygon2f p0, Polygon2f p1)
        {
            Polygon2f[] arr = new Polygon2f[2];
            arr[0] = p0;
            arr[1] = p1;

            int current = 0;
            int other = 1;

            List<V2f> points = new List<V2f>();
            bool enter = false;
            int index = -1;
            V2f point = V2f.NaN;

            bool firstIntersectionFound = false;
            V2f firstIntersectionPoint = V2f.NaN;

            for (int i = 0; i >= 0; i = (i + 1) % arr[current].PointCount)
            {
                V2f start = arr[current][(i + arr[current].PointCount - 1) % arr[current].PointCount];
                V2f end = arr[current][i];

                if (arr[other].ClosestIntersection(start, end, out enter, out index, out point))
                {
                    if (!firstIntersectionFound)
                    {
                        firstIntersectionFound = true;
                        firstIntersectionPoint = point;
                    }
                    else
                    {
                        if ((point - firstIntersectionPoint).Length < 0.001f) break;
                    }

                    if (enter)
                    {
                        points.Add(point);

                        while ((arr[other][(index + 1) % arr[other].PointCount] - point).Length < 0.001f) index = (index + 1) % arr[other].PointCount;

                        if (!arr[current].Contains(arr[other][(index + 1) % arr[other].PointCount]))
                        {
                            points.Add(arr[other][(index + 1) % arr[other].PointCount]);
                            index = (index + 1) % arr[other].PointCount;
                        }

                        i = index;
                        current = (current == 1 ? 0 : 1);
                        other = (current == 1 ? 0 : 1);
                    }
                    else
                    {
                        points.Add(point);
                        if (!arr[other].Contains(arr[current][i]))
                        {
                            points.Add(arr[current][i]);
                        }
                    }
                }
                else
                {
                    if (i == arr[current].PointCount - 1 && !firstIntersectionFound) break;

                    if (!arr[other].Contains(end) && firstIntersectionFound)
                    {
                        points.Add(end);
                    }
                }
            }

            if (!firstIntersectionFound)
            {
                if (p0.Contains(p1[0])) yield return p0;
                else if (p1.Contains(p0[0])) yield return p1;
                else
                {
                    yield return p0;
                    yield return p1;
                }
            }
            else yield return new Polygon2f(points);
        }

        #endregion

        public static IEnumerable<Polygon2f> Union(this Box2f b0, Box2f b1)
        {
            Polygon2f p0 = new Polygon2f(b0.ComputeCornersCCW());
            Polygon2f p1 = new Polygon2f(b1.ComputeCornersCCW());

            return p0.Union(p1);
        }

        #endregion

        #region Helpers (double)

        public static bool ClosestIntersection(this Polygon2d poly, V2d start, V2d end, out bool enter, out int otherIndex, out V2d p)
        {
            double min = double.MaxValue;

            bool intersects = false;

            V2d point = V2d.NaN;

            bool tempenter = false;
            int tempindex = -1;
            V2d temppoint = V2d.NaN;
            double templength = double.MaxValue;

            Line2d line = new Line2d(start, end);

            int i = 0;
            foreach (var l in poly.EdgeLines)
            {
                if (line.Intersects(l, 4 * double.Epsilon, out temppoint))
                {
                    templength = (temppoint - line.P0).Length;
                    if (templength < min)
                    {
                        tempenter = l.LeftValueOfDir(line.Direction) >= 0;
                        tempindex = i;
                        min = templength;
                        point = temppoint;
                    }

                    intersects = true;
                }
                i++;
            }

            enter = tempenter;
            otherIndex = tempindex;
            p = point;
            return intersects;
        }

        internal static double Closest(this List<V2d> points, V2d p)
        {
            double temp;
            double min = double.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                temp = (p - points[i]).Length;
                if (temp < min) min = temp;
            }

            return min;
        }

        internal static bool Intersects(V2d[] poly, V2d p0, V2d p1, ref List<V2d> known, out bool enter, out int index0, out V2d p)
        {
            int index = 0;

            Line2d l;
            int i0 = 0;
            int i1 = 1;
            for (int i = 1; i <= poly.Length; i++)
            {
                if (i < poly.Length)
                {
                    i0 = i - 1;
                    i1 = i;
                }
                else
                {
                    i0 = poly.Length - 1;
                    i1 = 0;
                }

                l = new Line2d(poly[i0], poly[i1]);
                if (l.Intersects(new Line2d(p0, p1), 4 *double.Epsilon, out p))
                {
                    if (!Fun.IsTiny(known.Closest(p)))
                    {
                        enter = (l.LeftValueOfPos(p1) >= -4 *double.Epsilon);
                        index0 = index;

                        return true;
                    }
                }

                index++;
            }

            p = V2d.NaN;
            index0 = -1;
            enter = false;
            return false;
        }

        internal static IEnumerable<V2d> AllVertices(Polygon2d poly)
        {
            int pc = poly.PointCount;
            for (int pi = 0; pi < pc; pi++) yield return poly[pi];
            yield return poly[0];
        }

        #endregion

        #region Intersections (double)

        #region Line2d - Polygon2d

        /// <summary>
        /// Returns the Line-Segments of line inside the Polygon (CCW ordered).
        /// Works with all (convex and non-convex) Polygons
        /// </summary>
        public static IEnumerable<Line2d> ClipWith(this Line2d line, Polygon2d poly)
        {
            bool i0, i1;

            i0 = poly.Contains(line.P0);
            i1 = poly.Contains(line.P1);

            var resulting = new List<V2d>();
            var enter = new List<bool>();

            if (i0)
            {
                resulting.Add(line.P0);
                enter.Add(true);
            }
            if (i1)
            {
                resulting.Add(line.P1);
                enter.Add(false);
            }

            var p = V2d.NaN;
            var direction = line.Direction;

            foreach (var l in poly.EdgeLines)
            {
                if (line.Intersects(l, out p))
                {
                    var d = l.Direction;
                    var n = new V2d(-d.Y, d.X);

                    if (!p.IsNaN)
                    {
                        bool addflag = true;
                        bool flag = direction.Dot(n) > 0;

                        for (int i = 0; i < resulting.Count; i++)
                        {
                            if (Fun.IsTiny((resulting[i] - p).Length))
                            {
                                if (flag != enter[i])
                                {
                                    resulting.RemoveAt(i);
                                    enter.RemoveAt(i);
                                }

                                addflag = false;
                                break;
                            }
                        }

                        if (addflag)
                        {
                            resulting.Add(p);
                            enter.Add(flag);
                        }
                    }
                }
            }

            var dir = line.P1 - line.P0;
            resulting = (from r in resulting select r).OrderBy(x => x.Dot(dir)).ToList();

            var counter = resulting.Count;
            var lines = new List<Line2d>();
            for (int i = 0; i < counter - 1; i += 2)
            {
                lines.Add(new Line2d(resulting[i], resulting[i + 1]));
            }
            return lines;
        }

        /// <summary>
        /// Returns the Line-Segments of line inside the Polygon (CCW ordered).
        /// Works only with Convex-Polygons
        /// If the line is clipped entirely, the points of the returned Line2d are NaN.
        /// </summary>
        public static Line2d ClipWithConvex(this Line2d line, Polygon2d poly)
        {
            var p = V2d.NaN;
            bool i0, i1;

            i0 = poly.Contains(line.P0);
            i1 = poly.Contains(line.P1);

            if (i0 && i1) return line;
            else if ((!i0 && i1) || (i0 && !i1))
            {
                foreach (var l in poly.EdgeLines)
                {
                    if (line.Intersects(l, out p)) break;
                }

                if (i0) return new Line2d(line.P0, p);
                else return new Line2d(p, line.P1);
            }
            else
            {
                V2d p0 = V2d.NaN;
                V2d p1 = V2d.NaN;
                int c = 0;

                foreach (var l in poly.EdgeLines)
                {
                    if (line.Intersects(l, out p))
                    {
                        if (c == 0) p0 = p;
                        else p1 = p;
                        c++;
                    }
                }

                if (c == 2)
                {
                    V2d u = p1 - p0;

                    if (u.Dot(line.Direction) > 0) return new Line2d(p0, p1);
                    else return new Line2d(p1, p0);
                }
                else return new Line2d(V2d.NaN, V2d.NaN);
            }
        }

        #endregion

        #region Polygon2d - Polygon2d

        /// <summary>
        /// Clips one Polygon with an other. Returns a Polygon representing the Intersection of the two.
        /// Works only with Convex Polygons. (For non-convex Polygons use Polygon2d.ClipWith(Polygon2d))
        /// !!!! UNTESTED !!!!
        /// </summary>
        public static Polygon2d ClippedByConvex(this Polygon2d poly, Polygon2d second)
        {
            V2d[][] arr = new V2d[2][];
            arr[0] = AllVertices(poly).ToArray();
            arr[1] = AllVertices(second).ToArray();

            Polygon2d[] polygon = new Polygon2d[2];
            polygon[0] = poly;
            polygon[1] = second;

            bool current = false;
            int currentIndex = 0;
            int otherIndex = 1;

            bool enter = false;
            int index = -1;
            V2d point = V2d.NaN;

            List<V2d> points = new List<V2d>();

            for (int i = 1; i < arr[currentIndex].Length; i = (i + 1) % arr[currentIndex].Length)
            {
                if (i < 1) i = 1;
                currentIndex = (current ? 1 : 0);
                otherIndex = (current ? 0 : 1);

                V2d start = arr[currentIndex][i - 1];
                V2d end = arr[currentIndex][i];

                if (Intersects(arr[otherIndex], start, end, ref points, out enter, out index, out point))
                {
                    if (enter)
                    {
                        points.Add(point);
                        int u = i;
                        int count = 0;
                        while (polygon[otherIndex].Contains(arr[currentIndex][u]))
                        {
                            points.Add(arr[currentIndex][u]);
                            u = (u + arr[currentIndex].Length - 1) % arr[currentIndex].Length;
                            count++;
                        }
                        if (count == 0) i--;
                        else
                        {
                            i += count - 1;
                        }
                    }
                    else
                    {
                        points.Add(point);
                        i = index;
                        current = !current;
                        currentIndex = (current ? 1 : 0);
                        otherIndex = (current ? 0 : 1);
                    }
                }
                else
                {

                    if (i < arr[currentIndex].Length && polygon[otherIndex].Contains(arr[currentIndex][i]))
                    {
                        if (Closest(points, arr[currentIndex][i]) < 0.001) break;
                        points.Add(arr[currentIndex][i]);
                    }
                }
            }


            return new Polygon2d(points);
        }

        /// <summary>
        /// Clips one Polygon with an other. Returns a Polygon-Set representing the Intersections of the two.
        /// Works only with all (convex/concave) Polygons
        /// !!!! UNTESTED !!!!
        /// </summary>
        public static IEnumerable<Polygon2d> ClippedBy(this Polygon2d p0, Polygon2d p1, double relativeEpsilon)
        {
            List<int[]> i0 = SubPrimitives.ComputeNonConcaveSubPolygons(p0, relativeEpsilon);
            List<int[]> i1 = SubPrimitives.ComputeNonConcaveSubPolygons(p1, relativeEpsilon);

            List<Polygon2d> polys = new List<Polygon2d>();
            for (int i = 0; i < i0.Count; i++)
            {
                for (int u = 0; u < i1.Count; u++)
                {
                    Polygon2d q0 = new Polygon2d(from o in i0[i] select p0[o]);
                    Polygon2d q1 = new Polygon2d(from o in i1[u] select p1[o]);

                    Polygon2d r = q0.ClippedByConvex(q1);
                    if (r.PointCount > 0) polys.Add(r);
                }
            }

            return Union(polys);

        }

        #endregion

        #endregion

        #region Unions (double)

        #region Polygon2d - Polygon2d


        public static IEnumerable<Polygon2d> Union(List<Polygon2d> p)
        {
            List<Polygon2d> polys = new List<Polygon2d>();
            polys.Add(p[0]);
            List<Polygon2d> temp;

            for (int i = 1; i < p.Count; i++)
            {
                bool found = false;
                for (int u = 0; u < polys.Count; u++)
                {
                    temp = polys[u].Union(p[i]).ToList();

                    if (temp.Count == 1)
                    {
                        polys[u] = temp[0];
                        found = true;
                    }
                }

                if (!found)
                {
                    polys.Add(p[i]);
                }
            }

            return polys;
        }

        /// <summary>
        /// Returns the unified Polygon of the two given ones.
        /// Works only with convex-Polygons
        /// Returns an empty Polygon if the two do not intersect
        /// </summary>
        public static IEnumerable<Polygon2d> Union(this Polygon2d p0, Polygon2d p1)
        {
            Polygon2d[] arr = new Polygon2d[2];
            arr[0] = p0;
            arr[1] = p1;

            int current = 0;
            int other = 1;

            List<V2d> points = new List<V2d>();
            bool enter = false;
            int index = -1;
            V2d point = V2d.NaN;

            bool firstIntersectionFound = false;
            V2d firstIntersectionPoint = V2d.NaN;

            for (int i = 0; i >= 0; i = (i + 1) % arr[current].PointCount)
            {
                V2d start = arr[current][(i + arr[current].PointCount - 1) % arr[current].PointCount];
                V2d end = arr[current][i];

                if (arr[other].ClosestIntersection(start, end, out enter, out index, out point))
                {
                    if (!firstIntersectionFound)
                    {
                        firstIntersectionFound = true;
                        firstIntersectionPoint = point;
                    }
                    else
                    {
                        if ((point - firstIntersectionPoint).Length < 0.001) break;
                    }

                    if (enter)
                    {
                        points.Add(point);

                        while ((arr[other][(index + 1) % arr[other].PointCount] - point).Length < 0.001) index = (index + 1) % arr[other].PointCount;

                        if (!arr[current].Contains(arr[other][(index + 1) % arr[other].PointCount]))
                        {
                            points.Add(arr[other][(index + 1) % arr[other].PointCount]);
                            index = (index + 1) % arr[other].PointCount;
                        }

                        i = index;
                        current = (current == 1 ? 0 : 1);
                        other = (current == 1 ? 0 : 1);
                    }
                    else
                    {
                        points.Add(point);
                        if (!arr[other].Contains(arr[current][i]))
                        {
                            points.Add(arr[current][i]);
                        }
                    }
                }
                else
                {
                    if (i == arr[current].PointCount - 1 && !firstIntersectionFound) break;

                    if (!arr[other].Contains(end) && firstIntersectionFound)
                    {
                        points.Add(end);
                    }
                }
            }

            if (!firstIntersectionFound)
            {
                if (p0.Contains(p1[0])) yield return p0;
                else if (p1.Contains(p0[0])) yield return p1;
                else
                {
                    yield return p0;
                    yield return p1;
                }
            }
            else yield return new Polygon2d(points);
        }

        #endregion

        public static IEnumerable<Polygon2d> Union(this Box2d b0, Box2d b1)
        {
            Polygon2d p0 = new Polygon2d(b0.ComputeCornersCCW());
            Polygon2d p1 = new Polygon2d(b1.ComputeCornersCCW());

            return p0.Union(p1);
        }

        #endregion

    }
}

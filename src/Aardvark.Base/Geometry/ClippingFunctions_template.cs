using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static partial class GeometryFun
    {
        //# foreach (var isDouble in new[] { false, true }) {
        //#   var rtype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var v2t = "V2" + tc;
        //#   var v3t = "V3" + tc;
        //#   var box2t = "Box2" + tc;
        //#   var line2t = "Line2" + tc;
        //#   var box3t = "Box3" + tc;
        //#   var triangle2t = "Triangle2" + tc;
        //#   var polygon2t = "Polygon2" + tc;
        //#   var eps = isDouble ? "0.001" : "0.001f";
        #region Helpers (__rtype__)

        public static bool ClosestIntersection(this __polygon2t__ poly, __v2t__ start, __v2t__ end, out bool enter, out int otherIndex, out __v2t__ p)
        {
            __rtype__ min = __rtype__.MaxValue;

            bool intersects = false;

            __v2t__ point = __v2t__.NaN;

            bool tempenter = false;
            int tempindex = -1;
            __v2t__ temppoint = __v2t__.NaN;
            __rtype__ templength = __rtype__.MaxValue;

            __line2t__ line = new __line2t__(start, end);

            int i = 0;
            foreach (var l in poly.EdgeLines)
            {
                if (line.Intersects(l, 4 * __rtype__.Epsilon, out temppoint))
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

        internal static __rtype__ Closest(this List<__v2t__> points, __v2t__ p)
        {
            __rtype__ temp;
            __rtype__ min = __rtype__.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                temp = (p - points[i]).Length;
                if (temp < min) min = temp;
            }

            return min;
        }

        internal static bool Intersects(__v2t__[] poly, __v2t__ p0, __v2t__ p1, ref List<__v2t__> known, out bool enter, out int index0, out __v2t__ p)
        {
            int index = 0;

            __line2t__ l;
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

                l = new __line2t__(poly[i0], poly[i1]);
                if (l.Intersects(new __line2t__(p0, p1), 4 *__rtype__.Epsilon, out p))
                {
                    if (!Fun.IsTiny(known.Closest(p)))
                    {
                        enter = (l.LeftValueOfPos(p1) >= -4 *__rtype__.Epsilon);
                        index0 = index;

                        return true;
                    }
                }

                index++;
            }

            p = __v2t__.NaN;
            index0 = -1;
            enter = false;
            return false;
        }

        internal static IEnumerable<__v2t__> AllVertices(__polygon2t__ poly)
        {
            int pc = poly.PointCount;
            for (int pi = 0; pi < pc; pi++) yield return poly[pi];
            yield return poly[0];
        }

        #endregion

        #region Intersections (__rtype__)

        #region __line2t__ - __polygon2t__

        /// <summary>
        /// Returns the Line-Segments of line inside the Polygon (CCW ordered).
        /// Works with all (convex and non-convex) Polygons
        /// </summary>
        public static IEnumerable<__line2t__> ClipWith(this __line2t__ line, __polygon2t__ poly)
        {
            bool i0, i1;

            i0 = poly.Contains(line.P0);
            i1 = poly.Contains(line.P1);

            var resulting = new List<__v2t__>();
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

            var p = __v2t__.NaN;
            var direction = line.Direction;

            foreach (var l in poly.EdgeLines)
            {
                if (line.Intersects(l, out p))
                {
                    var d = l.Direction;
                    var n = new __v2t__(-d.Y, d.X);

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
            var lines = new List<__line2t__>();
            for (int i = 0; i < counter - 1; i += 2)
            {
                lines.Add(new __line2t__(resulting[i], resulting[i + 1]));
            }
            return lines;
        }

        /// <summary>
        /// Returns the Line-Segments of line inside the Polygon (CCW ordered).
        /// Works only with Convex-Polygons
        /// If the line is clipped entirely, the points of the returned __line2t__ are NaN.
        /// </summary>
        public static __line2t__ ClipWithConvex(this __line2t__ line, __polygon2t__ poly)
        {
            var p = __v2t__.NaN;
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

                if (i0) return new __line2t__(line.P0, p);
                else return new __line2t__(p, line.P1);
            }
            else
            {
                __v2t__ p0 = __v2t__.NaN;
                __v2t__ p1 = __v2t__.NaN;
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
                    __v2t__ u = p1 - p0;

                    if (u.Dot(line.Direction) > 0) return new __line2t__(p0, p1);
                    else return new __line2t__(p1, p0);
                }
                else return new __line2t__(__v2t__.NaN, __v2t__.NaN);
            }
        }

        #endregion

        #region __polygon2t__ - __polygon2t__

        /// <summary>
        /// Clips one Polygon with an other. Returns a Polygon representing the Intersection of the two.
        /// Works only with Convex Polygons. (For non-convex Polygons use __polygon2t__.ClipWith(__polygon2t__))
        /// !!!! UNTESTED !!!!
        /// </summary>
        public static __polygon2t__ ClippedByConvex(this __polygon2t__ poly, __polygon2t__ second)
        {
            __v2t__[][] arr = new __v2t__[2][];
            arr[0] = AllVertices(poly).ToArray();
            arr[1] = AllVertices(second).ToArray();

            __polygon2t__[] polygon = new __polygon2t__[2];
            polygon[0] = poly;
            polygon[1] = second;

            bool current = false;
            int currentIndex = 0;
            int otherIndex = 1;

            bool enter = false;
            int index = -1;
            __v2t__ point = __v2t__.NaN;

            List<__v2t__> points = new List<__v2t__>();

            for (int i = 1; i < arr[currentIndex].Length; i = (i + 1) % arr[currentIndex].Length)
            {
                if (i < 1) i = 1;
                currentIndex = (current ? 1 : 0);
                otherIndex = (current ? 0 : 1);

                __v2t__ start = arr[currentIndex][i - 1];
                __v2t__ end = arr[currentIndex][i];

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
                        if (Closest(points, arr[currentIndex][i]) < __eps__) break;
                        points.Add(arr[currentIndex][i]);
                    }
                }
            }


            return new __polygon2t__(points);
        }

        /// <summary>
        /// Clips one Polygon with an other. Returns a Polygon-Set representing the Intersections of the two.
        /// Works only with all (convex/concave) Polygons
        /// !!!! UNTESTED !!!!
        /// </summary>
        public static IEnumerable<__polygon2t__> ClippedBy(this __polygon2t__ p0, __polygon2t__ p1, __rtype__ relativeEpsilon)
        {
            List<int[]> i0 = SubPrimitives.ComputeNonConcaveSubPolygons(p0, relativeEpsilon);
            List<int[]> i1 = SubPrimitives.ComputeNonConcaveSubPolygons(p1, relativeEpsilon);

            List<__polygon2t__> polys = new List<__polygon2t__>();
            for (int i = 0; i < i0.Count; i++)
            {
                for (int u = 0; u < i1.Count; u++)
                {
                    __polygon2t__ q0 = new __polygon2t__(from o in i0[i] select p0[o]);
                    __polygon2t__ q1 = new __polygon2t__(from o in i1[u] select p1[o]);

                    __polygon2t__ r = q0.ClippedByConvex(q1);
                    if (r.PointCount > 0) polys.Add(r);
                }
            }

            return Union(polys);

        }

        #endregion

        #endregion

        #region Unions (__rtype__)

        #region __polygon2t__ - __polygon2t__


        public static IEnumerable<__polygon2t__> Union(List<__polygon2t__> p)
        {
            List<__polygon2t__> polys = new List<__polygon2t__>();
            polys.Add(p[0]);
            List<__polygon2t__> temp;

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
        public static IEnumerable<__polygon2t__> Union(this __polygon2t__ p0, __polygon2t__ p1)
        {
            __polygon2t__[] arr = new __polygon2t__[2];
            arr[0] = p0;
            arr[1] = p1;

            int current = 0;
            int other = 1;

            List<__v2t__> points = new List<__v2t__>();
            bool enter = false;
            int index = -1;
            __v2t__ point = __v2t__.NaN;

            bool firstIntersectionFound = false;
            __v2t__ firstIntersectionPoint = __v2t__.NaN;

            for (int i = 0; i >= 0; i = (i + 1) % arr[current].PointCount)
            {
                __v2t__ start = arr[current][(i + arr[current].PointCount - 1) % arr[current].PointCount];
                __v2t__ end = arr[current][i];

                if (arr[other].ClosestIntersection(start, end, out enter, out index, out point))
                {
                    if (!firstIntersectionFound)
                    {
                        firstIntersectionFound = true;
                        firstIntersectionPoint = point;
                    }
                    else
                    {
                        if ((point - firstIntersectionPoint).Length < __eps__) break;
                    }

                    if (enter)
                    {
                        points.Add(point);

                        while ((arr[other][(index + 1) % arr[other].PointCount] - point).Length < __eps__) index = (index + 1) % arr[other].PointCount;

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
            else yield return new __polygon2t__(points);
        }

        #endregion

        public static IEnumerable<__polygon2t__> Union(this __box2t__ b0, __box2t__ b1)
        {
            __polygon2t__ p0 = new __polygon2t__(b0.ComputeCornersCCW());
            __polygon2t__ p1 = new __polygon2t__(b1.ComputeCornersCCW());

            return p0.Union(p1);
        }

        #endregion

    //# }
    }
}

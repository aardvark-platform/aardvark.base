using System.Collections.Generic;
using static System.Math;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides various methods for middle point computations.
    /// </summary>
    public static partial class GeometryFun
    {
        /* Ray3d */

        #region Ray3d - Ray3d

        public static V3d GetMiddlePoint(this Ray3d ray0, Ray3d ray1)
        {
            /*RayHit3d hit = new RayHit3d(double.MaxValue);
            if (ray0.Hits(ray1, ref hit))
            {
                return hit.Point;
            }
            else
            {*/
                var a = ray0.Origin - ray1.Origin;
                var u = ray0.Direction.Normalized;
                var v = ray1.Direction.Normalized;

                var uDotv = u.Dot(v);
                var my = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1.0);
                var la = (my * uDotv - a.Dot(u));

                var p0 = ray0.Origin + la * u;
                var p1 = ray1.Origin + my * v;

                //double d = (p1 - p0).Length;

                return 0.5*(p0 + p1);
            //}
        }

        #endregion

        #region IEnumerable<Ray3d>

        public static V3d GetMiddlePoint(this IEnumerable<Ray3d> rays)
        {
            var center = V3d.Zero;
            var count = 0;

            foreach(var r0 in rays)
                foreach (var r1 in rays)
                    if (r0.LexicalCompare(r1) < 0)
                    {
                        center += r0.GetMiddlePoint(r1);
                        ++count;
                    }

            return center / (double)count;
        }

        #endregion
    }

    /// <summary>
    /// Provides various methods for closest point computations.
    /// If the query object is an surface or volume the function
    /// return to closest point on the surface.
    /// </summary>
    public static partial class GeometryFun
    {
        /* V2d */

        #region V2d - Line2d

        public static V2d GetClosestPointOn(this V2d query, Line2d line)
            => GetClosestPointOn(query, line, out double t);

        public static V2d GetClosestPointOn(this Line2d line, V2d query)
            => query.GetClosestPointOn(line);

        public static V2d GetClosestPointOn(this V2d query, Line2d line, out double t)
        {
            var p0q = query - line.P0;
            t = V2d.Dot(p0q, line.Direction);
            if (t <= 0.0) { t = 0.0; return line.P0; }
            var denom = line.Direction.LengthSquared;
            if (t >= denom) { t = 1.0; return line.P1; }
            t /= denom;
            return line.P0 + t * line.Direction;
        }

        public static V2d GetClosestPointOn(this Line2d line, V2d query, out double t)
            => query.GetClosestPointOn(line, out t);

        #endregion

        #region V2d - Ray2d

        public static V2d GetClosestPointOn(this V2d query, Ray2d ray)
            => GetClosestPointOn(query, ray, out double t);

        public static V2d GetClosestPointOn(this Ray2d ray, V2d query)
            => query.GetClosestPointOn(ray);

        /// <summary>
        /// Returns the t-parameter along <paramref name="ray"/> at which the closest point to <paramref name="query"/> is.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        public static double GetClosestPointTOn(this V2d query, Ray2d ray)
            => V2d.Dot(query - ray.Origin, ray.Direction) / ray.Direction.LengthSquared;

        public static V2d GetClosestPointOn(this V2d query, Ray2d ray, out double t)
        {
            t = GetClosestPointTOn(query, ray);
            return ray.Origin + t * ray.Direction;
        }

        public static V2d GetClosestPointOn(this Ray2d ray, V2d query, out double t)
            => query.GetClosestPointOn(ray, out t);

        /// <summary>
        /// Returns the t-parameter along <paramref name="ray"/> at which the closest point to <paramref name="query"/> is.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        public static double GetClosestPointTOn(this Ray2d ray, V2d query)
            => query.GetClosestPointTOn(ray);

        #endregion

        #region V2d - Plane2d

        public static V2d GetClosestPointOn(this V2d point, Plane2d line)
        {
            var lengthOfNormal2 = line.Normal.LengthSquared;
            return (point - (line.Normal.Dot(point - line.Point) / lengthOfNormal2) * line.Normal);
        }

        public static V2d GetClosestPointOn(this Plane2d line, V2d point)
            => point.GetClosestPointOn(line);

        #endregion

        #region V2d - Box2d

        public static V2d GetClosestPointOn(this V2d query, Box2d box)
        {
            var closest = query;
            for (var i = 0; i < 2; i++)
            {
                if (query[i] < box.Min[i]) closest[i] = box.Min[i];
                if (query[i] > box.Max[i]) closest[i] = box.Max[i];
            }
            return closest;
        }

        public static V2d GetClosestPointOn(this Box2d box, V2d query)
            => query.GetClosestPointOn(box);

        #endregion

        #region V2d - Quad2d

        public static V2d GetClosestPointOn(this V2d vec, Quad2d quad)
        {
            V2d closestPoint;
            var temp = vec.GetClosestPointOn(new Line2d(quad.P0,quad.P1));
            closestPoint = temp;

            temp = vec.GetClosestPointOn(new Line2d(quad.P1, quad.P2));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            temp = vec.GetClosestPointOn(new Line2d(quad.P2, quad.P3));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            temp = vec.GetClosestPointOn(new Line2d(quad.P3, quad.P0));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            return closestPoint;
        }

        public static V2d GetClosestPointOn(this Quad2d quad, V2d vec)
            => vec.GetClosestPointOn(quad);

        #endregion

        #region V2d - Polygon2d

        public static V2d GetClosestPointOn(this V2d vec, Polygon2d poly)
        {
            var closestPoint = V2d.PositiveInfinity;
            var i = 0;
            foreach (var line in poly.EdgeLines)
            {
                var temp = vec.GetClosestPointOn(line);
                if (i++ == 0) closestPoint = temp;
                else if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;
            }

            return closestPoint;
        }

        public static V2d GetClosestPointOn(this Polygon2d poly, V2d vec)
            => vec.GetClosestPointOn(poly);

        #endregion

        #region V2d - Circle2d

        public static V2d GetClosestPointOn(this V2d query, Circle2d circle)
            => circle.Center + circle.Radius * (query - circle.Center).Normalized;

        public static V2d GetClosestPointOn(this Circle2d circle, V2d query)
            => query.GetClosestPointOn(circle);

        #endregion

        #region V2d - Triangle2d

        public static V2d GetClosestPointOn(this V2d query, Triangle2d triangle)
        {
            var e01 = triangle.Edge01;
            var e02 = triangle.Edge02;

            var p0q = query - triangle.P0;
            var d1 = V2d.Dot(e01, p0q);
            var d2 = V2d.Dot(e02, p0q);
            if (d1 <= 0.0 && d2 <= 0.0) return triangle.P0; // bary (1,0,0)

            var p1q = query - triangle.P1;
            var d3 = V2d.Dot(e01, p1q);
            var d4 = V2d.Dot(e02, p1q);
            if (d3 >= 0.0 && d4 <= d3) return triangle.P1; // bary (0,1,0)

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0.0 && d1 >= 0.0 && d3 <= 0.0)
            {
                var t = d1 / (d1 - d3);
                return triangle.P0 + t * e01;   // bary (1-t,t,0) 
            }

            var p2q = query - triangle.P2;
            var d5 = V2d.Dot(e01, p2q);
            var d6 = V2d.Dot(e02, p2q);
            if (d6 >= 0.0 && d5 <= d6) return triangle.P2; // bary (0,0,1)

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0 && d2 >= 0.0 && d6 <= 0.0)
            {
                var t = d2 / (d2 - d6);
                return triangle.P0 + t * e02; // bary (1-t,0,t)
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0.0 && (d4 - d3) >= 0.0 && (d5 - d6) >= 0.0)
            {
                var t = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return triangle.P1 + t * triangle.Edge12; // bary (0, 1-t, t)
            }

            // var denom = 1.0 / (va + vb + vc);
            // var v = vb * denom;
            // var w = vc * denom;
            // return triangle.P0 + v * e01 + w * e02; // bary (1-v-w, v, w)
            return query;
        }

        public static V2d GetClosestPointOn(this Triangle2d triangle, V2d query)
            => query.GetClosestPointOn(triangle);

        #endregion

        /* V3d */

        #region V3d - Box3d

        public static double GetDistanceSquared(this V3d q, Box3d b)
            => (q.X < b.Min.X ? Fun.Square(b.Min.X - q.X) : q.X > b.Max.X ? Fun.Square(q.X - b.Max.X) : 0.0)
             + (q.Y < b.Min.Y ? Fun.Square(b.Min.Y - q.Y) : q.Y > b.Max.Y ? Fun.Square(q.Y - b.Max.Y) : 0.0)
             + (q.Z < b.Min.Z ? Fun.Square(b.Min.Z - q.Z) : q.Z > b.Max.Z ? Fun.Square(q.Z - b.Max.Z) : 0.0);

        public static (double, double) GetDistanceSquared(
            this V3d q, Box3dAndFlags boxAndFlags, Box.Flags d2Flags, V3d d2v,
            out Box.Flags d2Flags0, out V3d d2v0,
            out Box.Flags d2Flags1, out V3d d2v1)
        {
            d2Flags0 = d2Flags; d2Flags1 = d2Flags;
            d2v0 = d2v; d2v1 = d2v;

            double d0 = 0.0, d1 = 0.0;

            if ((d2Flags & Box.Flags.MinX) != 0)
            {
                if ((boxAndFlags.BFlags & Box.Flags.MinX0) != 0)
                {
                    d2v0.X = Fun.Square(boxAndFlags.BBox.Min.X - q.X); d2Flags0 |= Box.Flags.MinX;
                }
                else if ((boxAndFlags.BFlags & Box.Flags.MinX1) != 0)
                {
                    d2v1.X = Fun.Square(boxAndFlags.BBox.Min.X - q.X); d2Flags1 |= Box.Flags.MinX;
                }
                d0 += d2v0.X; d1 += d2v1.X;
            }
            else if ((d2Flags & Box.Flags.MaxX) != 0)
            {
                if ((boxAndFlags.BFlags & Box.Flags.MaxX0) != 0)
                {
                    d2v0.X = Fun.Square(boxAndFlags.BBox.Max.X - q.X); d2Flags0 |= Box.Flags.MaxX;
                }
                else if ((boxAndFlags.BFlags & Box.Flags.MaxX1) != 0)
                {
                    d2v1.X = Fun.Square(boxAndFlags.BBox.Max.X - q.X); d2Flags1 |= Box.Flags.MaxX;
                }
                d0 += d2v0.X; d1 += d2v1.X;
            }
            else
            {
                if ((boxAndFlags.BFlags & Box.Flags.MinX0) != 0)
                {
                    if (q.X < boxAndFlags.BBox.Min.X)
                    {
                        d2v0.X = Fun.Square(boxAndFlags.BBox.Min.X - q.X); d2Flags0 |= Box.Flags.MinX; d0 += d2v0.X;
                    }
                }
                else if ((boxAndFlags.BFlags & Box.Flags.MinX1) != 0)
                {
                    if (q.X < boxAndFlags.BBox.Min.X)
                    {
                        d2v1.X = Fun.Square(boxAndFlags.BBox.Min.X - q.X); d2Flags1 |= Box.Flags.MinX; d1 += d2v1.X;
                    }
                }
                if ((boxAndFlags.BFlags & Box.Flags.MaxX0) != 0)
                {
                    if (q.X > boxAndFlags.BBox.Max.X)
                    {
                        d2v0.X = Fun.Square(boxAndFlags.BBox.Max.X - q.X); d2Flags0 |= Box.Flags.MaxX; d0 += d2v0.X;
                    }
                }
                else if ((boxAndFlags.BFlags & Box.Flags.MaxX1) != 0)
                {
                    if (q.X > boxAndFlags.BBox.Max.X)
                    {
                        d2v1.X = Fun.Square(boxAndFlags.BBox.Max.X - q.X); d2Flags1 |= Box.Flags.MaxX; d1 += d2v1.X;
                    }
                }
            }
            
            return (d0, d1);
        }
        
        public static V3d GetClosestPointOn(this V3d query, Box3d box)
        {
            var closest = query;
            for (int i = 0; i < 3; i++)
            {
                if (query[i] < box.Min[i]) closest[i] = box.Min[i];
                if (query[i] > box.Max[i]) closest[i] = box.Max[i];
            }
            return closest;
        }

        public static V3d GetClosestPointOn(this Box3d box, V3d query)
            => query.GetClosestPointOn(box);

        #endregion

        #region V3d - Line3d

        public static V3d GetClosestPointOn(this V3d query, Line3d line)
            => query.GetClosestPointOnLine(line.P0, line.P1, out double t);

        public static V3d GetClosestPointOn(this Line3d line, V3d query)
            => query.GetClosestPointOn(line);

        public static V3d GetClosestPointOn(this V3d query, Line3d line, out double t)
            => query.GetClosestPointOnLine(line.P0, line.P1, out t);

        public static V3d GetClosestPointOn(this Line3d line, V3d query, out double t)
            => query.GetClosestPointOn(line, out t);

        public static V3d GetClosestPointOnLine(this V3d query, V3d p0, V3d p1)
            => query.GetClosestPointOnLine(p0, p1, out double t);

        public static V3d GetClosestPointOnLine(this V3d query, V3d p0, V3d p1, out double t)
        {
            var dir = p1 - p0;
            var p0q = query - p0;
            t = V3d.Dot(p0q, dir);
            if (t <= 0.0) { t = 0.0; return p0; }
            var denom = dir.LengthSquared;
            if (t >= denom) { t = 1.0; return p1; }
            t /= denom;
            return p0 + t * dir;
        }

        #endregion

        #region V3d - Plane3d

        public static V3d GetClosestPointOn(this V3d query, Plane3d plane)
            => query - plane.Height(query) * plane.Normal;

        public static V3d GetClosestPointOn(this Plane3d plane, V3d query)
            => query.GetClosestPointOn(plane);

        #endregion

        #region V3d - Ray3d

        public static V3d GetClosestPointOn(this V3d query, Ray3d ray)
            => GetClosestPointOn(query, ray, out double t);

        public static V3d GetClosestPointOn(this Ray3d ray, V3d query)
            => query.GetClosestPointOn(ray);

        public static V3d GetClosestPointOn(this V3d query, Ray3d ray, out double t)
        {
            t = V3d.Dot(query - ray.Origin, ray.Direction)
                        / ray.Direction.LengthSquared;
            return ray.Origin + t * ray.Direction;
        }

        public static V3d GetClosestPointOn(this Ray3d ray, V3d query, out double t)
            => query.GetClosestPointOn(ray, out t);

        #endregion

        #region V3d - Sphere3d

        public static V3d GetClosestPointOn(this V3d query, Sphere3d sphere)
            => sphere.Center + sphere.Radius * (query - sphere.Center).Normalized;

        public static V3d GetClosestPointOn(this Sphere3d sphere, V3d query)
            => query.GetClosestPointOn(sphere);

        #endregion

        #region V3d - Cylinder3d

        /// <summary>
        /// get closest point on cylinder with infinite length
        /// </summary>
        public static V3d GetClosestPointOn(this V3d query, Cylinder3d cylinder)
        {
            var p = query.GetClosestPointOn(cylinder.Axis.Ray3d);
            var dir = (query - p).Normalized;
            return p + cylinder.Radius * dir;
        }

        /// <summary>
        /// get closest point on cylinder with infinite length
        /// </summary>
        public static V3d GetClosestPointOn(this Cylinder3d cylinder, V3d query)
            => query.GetClosestPointOn(cylinder);

        #endregion

        #region V3d - Triangle3d

        /// <summary>
        /// Tested by rft on 2008-08-06.
        /// </summary>
        public static V3d GetClosestPointOn(this V3d query, Triangle3d triangle)
            => GetClosestPointOnTriangle(query, triangle.P0, triangle.P1, triangle.P2);

        public static V3d GetClosestPointOn(this Triangle3d triangle, V3d query)
            => query.GetClosestPointOn(triangle);

        /// <summary>
        /// Tested by rft on 2008-08-06.
        /// </summary>
        public static V3d GetClosestPointOnTriangle(this V3d query, V3d p0, V3d p1, V3d p2)
        {
            var e01 = p1 - p0;
            var e02 = p2 - p0;

            var p0q = query - p0;
            var d1 = V3d.Dot(e01, p0q);
            var d2 = V3d.Dot(e02, p0q);
            if (d1 <= 0.0 && d2 <= 0.0) return p0; // bary (1,0,0)

            var p1q = query - p1;
            var d3 = V3d.Dot(e01, p1q);
            var d4 = V3d.Dot(e02, p1q);
            if (d3 >= 0.0 && d4 <= d3) return p1; // bary (0,1,0)

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0.0 && d1 >= 0.0 && d3 <= 0.0)
            {
                var t = d1 / (d1 - d3);
                return p0 + t * e01;   // bary (1-t,t,0) 
            }

            var p2q = query - p2;
            var d5 = V3d.Dot(e01, p2q);
            var d6 = V3d.Dot(e02, p2q);
            if (d6 >= 0.0 && d5 <= d6) return p2; // bary (0,0,1)

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0 && d2 >= 0.0 && d6 <= 0.0)
            {
                var t = d2 / (d2 - d6);
                return p0 + t * e02; // bary (1-t,0,t)
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0.0 && (d4 - d3) >= 0.0 && (d5 - d6) >= 0.0)
            {
                var t = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return p1 + t * (p2 - p1); // bary (0, 1-t, t)
            }

            var denom = 1.0 / (va + vb + vc);
            var v = vb * denom;
            var w = vc * denom;
            return p0 + v * e01 + w * e02; // bary (1-v-w, v, w)
        }

        #endregion

        /* Ray3d */

        #region Ray3d - Ray3d

        public static V3d GetClosestPointOn(this Ray3d ray0, Ray3d ray1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            var n = (uDotv * uDotv - 1.0);
            // make sure rays are not parallel
            if (!n.Abs().IsTiny())
            {
                var my = (a.Dot(u) * uDotv - a.Dot(v)) / n;
                return ray1.Origin + my * v;
            }

            return ray1.Origin;
        }

        #endregion

        /* Line3d */

        #region Line3d - Line3d

        /// <summary>
        /// Returns the point on line1 which is closest to line0.
        /// </summary>
        public static V3d GetClosestPointOn(this Line3d line0, Line3d line1)
        {
            var r0 = line0.Ray3d;
            var r1 = line1.Ray3d;

            if (r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                if (line0.P0.GetMinimalDistanceTo(line1) < line0.P1.GetMinimalDistanceTo(line1))
                    return line0.P0.GetClosestPointOn(line1);
                else
                    return line0.P1.GetClosestPointOn(line1);
            }

            /* var distance = */
            r0.GetMinimalDistanceTo(r1, out double t0, out double t1);
            if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
                return r1.GetPointOnRay(t1);

            var d0 = line0.P0.GetMinimalDistanceTo(line1);
            var d1 = line0.P1.GetMinimalDistanceTo(line1);
            var d2 = line1.P0.GetMinimalDistanceTo(line0);
            var d3 = line1.P1.GetMinimalDistanceTo(line0);

            if (d0 < d1)
            {
                if (d0 < d2)
                    return d0 < d3 ? line0.P0.GetClosestPointOn(line1) : line1.P1;
                else
                    return d2 < d3 ? line1.P0 : line1.P1;
            }
            else
            {
                if (d1 < d2)
                    return d1 < d3 ? line0.P1.GetClosestPointOn(line1) : line1.P1;
                else
                    return d2 < d3 ? line1.P0 : line1.P1;
            }
        }

        #endregion

        #region Line3d - Plane3d

        /// <summary>
        /// Returns the closest point from a line to a plane (point on line).
        /// </summary>
        public static V3d GetClosestPointOn(this Plane3d plane, Line3d line)
        {
            var ray = line.Ray3d;
            if (ray.Intersects(plane, out double t))
            {
                var tClamped = t.Clamp(0, 1); // clamp point to line range
                return ray.GetPointOnRay(tClamped);
            }
            else // plane and line are parallel
            {
                return line.P0;
            }
        }
        //note: reverse method is not the same (closest point on line)

        #endregion

        #region Line3d - Triangle3d

        /// <summary>
        /// Returns the closest point from a triangle to a line (point on triangle).
        /// Note: untested
        /// </summary>
        public static V3d GetClosestPointOn(this Line3d line, Triangle3d triangle)
        {
            // test if closest point on triangle plane is inside the triangle (we have the point), 
            // otherwiese find closest edge and take closest point to that edge

            var plane = triangle.Plane;
            var planePoint = plane.GetClosestPointOn(line);
            
            // switch to 2d:
            var trafo = Trafo3d.FromNormalFrame(plane.Point, plane.Normal);
            var point2d = trafo.Backward.TransformPos(planePoint).XY;
            var triangle2d = new Triangle2d(trafo.Backward.TransformPos(triangle.P0).XY,
                                            trafo.Backward.TransformPos(triangle.P1).XY,
                                            trafo.Backward.TransformPos(triangle.P2).XY);

            if (triangle2d.Contains(point2d))
                return trafo.Forward.TransformPos(point2d.XYO);

            var edgeDistances = new [] { 
                line.GetMinimalDistanceTo(triangle.Line01),
                line.GetMinimalDistanceTo(triangle.Line12),
                line.GetMinimalDistanceTo(triangle.Line20)
            };

            var closestEdge = triangle.GetEdgeLine(edgeDistances.IndexOfMin());
            return line.GetClosestPointOn(closestEdge);
        }
        //note: reverse method is not the same (closest point on line)

        #endregion
    }


    public static partial class GeometryFun
    {
        #region Range1d - Range1d

        public static double GetMinimalDistanceTo(this Range1d range0, Range1d range1)
        {
            if (range0.Min > range1.Max) return range0.Min - range1.Max;
            if (range0.Max < range1.Min) return range1.Min - range0.Max;
            return 0;
        }

        #endregion

        #region Box2d - Box2d

        public static double GetMinimalDistanceTo(this Box2d box0, Box2d box1)
        {
            var distX = box0.RangeX.GetMinimalDistanceTo(box1.RangeX);
            var distY = box0.RangeY.GetMinimalDistanceTo(box1.RangeY);

            if (distX == 0 && distY == 0)
                return 0;
            else if (distX == 0)
                return distY;
            else if (distY == 0)
                return distX;

            if (box0.Min.X > box1.Max.X) // 1 is left
            {
                if (box0.Min.Y > box1.Max.Y) // 1 is top left
                    return (box0.Corner(0) - box1.Corner(3)).Length;
                else // 1 is bottom left
                    return (box0.Corner(2) - box1.Corner(1)).Length;
            }
            else // 1 is right
            {
                if (box0.Min.Y > box1.Max.Y) // 1 is top right
                    return (box0.Corner(1) - box1.Corner(2)).Length;
                else // 1 is bottom right
                    return (box0.Corner(3) - box1.Corner(0)).Length;
            }
        }

        #endregion

        #region V3d - Line3d
        
        /*  Performance Test .........................................................................
                Faster Method:                                      V3d.MinimalDistanceTo(Line3d)
                V3d.MinimalDistanceTo(Line3d):                      2,418s
                (V3d - V3d.GetClosestPointOn(Line3d)).Length:       4,789s
                Total Executions:                                   10000000
                Errors outside tolerance(1E-7):                     0
                Average Squared-Distance of Results:                1,13089959414307E-31
                Speedup-factor:                                     1,98
         */

        /// <summary>
        /// returns the minimal distance between the point and the Line
        /// </summary>
        public static double GetMinimalDistanceTo(this V3d point, Line3d line)
        {
            var a = point - line.P0;
            var u = line.P1 - line.P0;

            var lu2 = u.LengthSquared;
            var adu = V3d.Dot(a, u);

            if (adu > lu2)
            {
                var acu2 = V3d.Cross(a, u).LengthSquared;
                var s1 = (adu * adu - 2.0 * adu * lu2 + lu2 * lu2);

                return Sqrt((acu2 + s1) / lu2);
            }
            else if (adu >= 0.0)
            {
                var acu2 = V3d.Cross(a, u).LengthSquared;
                return Sqrt(acu2 / lu2);
            }
            else
            {
                return a.Length;
            }
        }

        /// <summary>
        /// Returns the minimal distance between the point and the Line.
        /// </summary>
        public static double GetMinimalDistanceTo(this Line3d line, V3d point)
            => point.GetMinimalDistanceTo(line);

        #endregion

        #region V3d - Ray3d
        
        /*  Performance Test .........................................................................
                Faster Method:                                      V3d.MinimalDistanceTo(Ray3d)
                V3d.MinimalDistanceTo(Ray3d):                       1,841s
                (V3d - V3d.GetClosestPointOn(Ray3d)).Length:        3,058s
                Total Executions:                                   10000000
                Errors outside tolerance(1E-7):                     0
                Average Squared-Distance of Results:                2,24116481902135E-32
                Speedup-factor:                                     1,66
         */

        /// <summary>
        /// Returns the minimal distance between the point and the ray.
        /// </summary>
        public static double GetMinimalDistanceTo(this V3d point, Ray3d ray)
        {
            var a = point - ray.Origin;
            double lu2 = ray.Direction.LengthSquared;
            double acu2 = V3d.Cross(a, ray.Direction).LengthSquared;

            return Sqrt(acu2 / lu2);
        }

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// </summary>
        public static double GetMinimalDistanceTo(this Ray3d ray, V3d point)
            => point.GetMinimalDistanceTo(ray);

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// t holds the ray parameter for the closest point.
        /// </summary>
        public static double GetMinimalDistanceTo(this V3d point, Ray3d ray, out double t)
        {
            var a = point - ray.Origin;
            var lu2 = ray.Direction.LengthSquared;
            var acu2 = V3d.Cross(a, ray.Direction).LengthSquared;

            var NormalPart2 = acu2 / lu2;
            var ParallelPart2 = lu2 - NormalPart2;

            t = Sqrt(ParallelPart2 / lu2);
            return Sqrt(NormalPart2);
        }

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// t holds the ray parameter for the closest point.
        /// </summary>
        public static double GetMinimalDistanceTo(this Ray3d ray, V3d point, out double t)
            => point.GetMinimalDistanceTo(ray, out t);

        #endregion

        #region V3d - Plane3d

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use Plane3d.Height to compute the signed height of the point over the plane.
        /// </summary>
        public static double GetMinimalDistanceTo(this V3d point, Plane3d plane)
            => plane.Height(point).Abs();

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use Plane3d.Height to compute the signed height of the point over the plane.
        /// </summary>
        public static double GetMinimalDistanceTo(this Plane3d plane, V3d point)
            => point.GetMinimalDistanceTo(plane);

        #endregion

        #region V3d - Box3d

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        public static double GetMinimalDistanceTo(this V3d point, Box3d box)
        {
            var outside = box.OutsideFlags(point); if (outside == 0) return 0.0;
            
            var d = V3d.Zero;

            if ((outside & Box.Flags.X) != 0)
            {
                if ((outside & Box.Flags.MaxX) != 0) d.X = point.X - box.Max.X;
                else if ((outside & Box.Flags.MinX) != 0) d.X = point.X - box.Min.X;
            }

            if ((outside & Box.Flags.Y) != 0)
            {
                if ((outside & Box.Flags.MaxY) != 0) d.Y = point.Y - box.Max.Y;
                else if ((outside & Box.Flags.MinY) != 0) d.Y = point.Y - box.Min.Y;
            }

            if ((outside & Box.Flags.Z) != 0)
            {
                if ((outside & Box.Flags.MaxZ) != 0) d.Z = point.Z - box.Max.Z;
                else if ((outside & Box.Flags.MinZ) != 0) d.Z = point.Z - box.Min.Z;
            }

            return d.Length;
        }

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        public static double GetMinimalDistanceTo(this Box3d box, V3d point)
            => point.GetMinimalDistanceTo(box);

        #endregion

        #region Line3d - Line3d

        /// <summary>
        /// returns the minimal distance between the given lines.
        /// </summary>
        public static double GetMinimalDistanceTo(this Line3d l0, Line3d l1)
        {
            var r0 = l0.Ray3d;
            var r1 = l1.Ray3d;

            if (!r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                var distance = r0.GetMinimalDistanceTo(r1, out double t0, out double t1);

                if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
                    return distance;
            }

            // t0 and t1 are outside of line
            return Fun.Min(l0.P0.GetMinimalDistanceTo(l1), l0.P1.GetMinimalDistanceTo(l1),
                l1.P0.GetMinimalDistanceTo(l0), l1.P1.GetMinimalDistanceTo(l0));
        }

        /// <summary>
        /// returns the minimal distance between the given lines.
        /// points holds the centroid of the shortest connection between the lines
        /// </summary>
        public static double GetMinimalDistanceTo(this Line3d l0, Line3d l1, out V3d point)
        {
            var r0 = l0.Ray3d;
            var r1 = l1.Ray3d;

            var distance = r0.GetMinimalDistanceTo(r1, out double t0, out double t1);

            if (t0 >= 0.0 && t0 <= 1.0 && t1 >= 0.0 && t1 <= 1.0)
            {
                point = 0.5 * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t1));
                return distance;
            }
            else
            {
                double t;
                if (t0 < 0.0 || t0 > 1.0)
                {
                    if (t0 < 0.0) t0 = 0.0;
                    else t0 = 1.0;

                    distance = r0.GetPointOnRay(t0).GetMinimalDistanceTo(r1, out t);
                    if (t >= 0.0 && t <= 1.0)
                    {
                        point = 0.5 * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t));
                        return distance;
                    }
                }
                
                if (t1 < 0.0 || t1 > 1.0)
                {
                    if (t1 < 0.0) t1 = 0.0;
                    else t1 = 1.0;

                    distance = r1.GetPointOnRay(t1).GetMinimalDistanceTo(r0, out t);
                    if (t >= 0.0 && t <= 1.0)
                    {
                        point = 0.5 * (r0.GetPointOnRay(t) + r1.GetPointOnRay(t1));
                        return distance;
                    }
                }

            }
                
            point = 0.5 * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t1));
            return (r0.GetPointOnRay(t0) - r1.GetPointOnRay(t1)).Length;
        }
  
        #endregion

        #region Line3d - Plane3d

        public static double GetMinimalDistanceTo(this Line3d line, Plane3d plane)
            => plane.Height(GeometryFun.GetClosestPointOn(plane, line)).Abs();

        public static double GetMinimalDistanceTo(this Plane3d plane, Line3d line)
            => line.GetMinimalDistanceTo(plane);

        #endregion

        #region Ray3d - Line3d

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// </summary>
        public static double GetMinimalDistanceTo(this Ray3d ray, Line3d line)
        {
            var r1 = line.Ray3d;

            var distance = ray.GetMinimalDistanceTo(r1, out double t0, out double t1);

            if (t1 >= 0.0 && t1 <= 1.0) return distance;
            else
            {
                if (t1 < 0.0) t1 = 0.0;
                if (t1 > 1.0) t1 = 1.0;

                var p1 = r1.GetPointOnRay(t1);
                return p1.GetMinimalDistanceTo(ray);
            }
        }

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// </summary>
        public static double GetMinimalDistanceTo(this Line3d line, Ray3d ray)
            => ray.GetMinimalDistanceTo(line);

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// t holds the correspoinding ray parameter.
        /// </summary>
        public static double GetMinimalDistanceTo(this Ray3d ray, Line3d line, out double t)
        {
            var r1 = line.Ray3d;

            var distance = ray.GetMinimalDistanceTo(r1, out double t0, out double t1);

            if (t1 >= 0.0 && t1 <= 1.0)
            {
                t = t0;
                return distance;
            }
            else
            {
                if (t1 < 0.0) t1 = 0.0;
                if (t1 > 1.0) t1 = 1.0;

                var p1 = r1.GetPointOnRay(t1);
                return p1.GetMinimalDistanceTo(ray, out t);
            }
        }

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// t holds the correspoinding ray parameter.
        /// </summary>
        public static double GetMinimalDistanceTo(this Line3d line, Ray3d ray, out double t)
            => ray.GetMinimalDistanceTo(line, out t);

        #endregion

        #region Ray3d - Ray3d

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// </summary>
        public static double GetMinimalDistanceTo(this Ray3d ray0, Ray3d ray1)
            => ray0.GetMinimalDistanceTo(ray1, out double t0, out double t1);

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// t0 and t1 hold the correspunding ray parameters.
        /// if both rays are parallel the t0 and t1 are from the origin of ray1
        /// </summary>
        public static double GetMinimalDistanceTo(this Ray3d ray0, Ray3d ray1, out double t0, out double t1)
        {
            // TODO: computation probalby possible without Direction.Normalized and Direction.Length

            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            if (uDotv.Abs().ApproximateEquals(1, Constant<double>.PositiveTinyValue)) // rays are parallel
            {
                // return origin of ray 1
                t1 = 0;
                t0 = ray0.GetTOfProjectedPoint(ray1.Origin);
            }
            else
            {
                // change by lui: added normalization (ortherwise in case the directions are not normalized t0 and t1 are wrong)
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1.0);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 = t1 / ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        #endregion
    }

#if NEVERMORE
    
        /// <summary>
        /// Determines the closest points on a frustum to the query point.
        /// Calculated point is passed as out parameter.
        /// </summary>
        /// <returns>Distance point to frustum.</returns>
        public static double GetClosestPointOn(
            this V3d queryPoint, IFrustum3d frustum, out V3d Point
            )
        {
            V3d closest_p1 = V3d.Zero;
            V3d closest_p2 = V3d.Zero;
            V3d closest_p3 = V3d.Zero;
            double dist_1 = double.MaxValue;
            double dist_2 = double.MaxValue;
            double dist_3 = double.MaxValue;
            double check;
            for (int i = 0; i < 8; i++)
            {
                check = V3d.DistanceSquared(frustum.GetCorners()[i], queryPoint);
                if (check < dist_3)
                {
                    dist_3 = check;
                    closest_p3 = frustum.GetCorners()[i];
                }
                if (dist_3 < dist_2)
                {
                    Fun.Swap(ref dist_3, ref dist_2);
                    closest_p3 = closest_p2;
                    closest_p2 = frustum.GetCorners()[i];
                }
                if (dist_2 < dist_1)
                {
                    Fun.Swap(ref dist_2, ref dist_1);
                    closest_p2 = closest_p1;
                    closest_p1 = frustum.GetCorners()[i];
                }
            }
            double t12, t13;
            Plane3d compare = new Plane3d(closest_p1, closest_p2, closest_p3);
            compare.Normalize();
            dist_1 = DistancePointToPlane(queryPoint, compare);
            dist_2 = V3d.Distance(queryPoint, queryPoint.GetClosestPointOn((new Ray3d(closest_p2, (closest_p2 - closest_p1))), out t12));
            dist_3 = V3d.Distance(queryPoint, queryPoint.GetClosestPointOn((new Ray3d(closest_p3, (closest_p3 - closest_p1))), out t13));
            if (t12 == 0 && t13 == 0)
            {
                Point = closest_p1;
                return V3d.Distance(queryPoint, closest_p1);
            }
            if (t12 == 0)
            {
                Point = closest_p1 + (closest_p3 - closest_p1) * t13;
                return dist_3;
            }
            if (t13 == 0)
            {
                Point = closest_p1 + (closest_p2 - closest_p1) * t12;
                return dist_2;
            }
            Point = queryPoint + (compare.Normal * -dist_1);
            return dist_1;
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between two lines.
        /// r1 = origin1 + t_line1 * direction1 and r2 = origin2 + t_line2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Ray2d line1, Ray2d line2,
            out double t_line1, out double t_line2
            )
        {
            V2d r = line1.Origin - line2.Origin;

            double a = V2d.Dot(line1.Direction, line1.Direction);
            double b = V2d.Dot(line1.Direction, line2.Direction);
            double c = V2d.Dot(line1.Direction, r);
            double e = V2d.Dot(line2.Direction, line2.Direction);
            double f = V2d.Dot(line2.Direction, r);

            double d = (a * e - b * b);

            if (d >= 0 - double.Epsilon && d <= 0 + double.Epsilon)
            {
                t_line1 = 0.0;
                line1.Origin.GetClosestPointOn(line2, out t_line2);
            }

            else
            {
                t_line1 = (b * f - c * e) / d;
                t_line2 = (a * f - b * c) / d;
            }
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between two lines.
        /// r1 = origin1 + t_line1 * direction1 and r2 = origin2 + t_line2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Ray3d line1, Ray3d line2,
            out double t_line1, out double t_line2
            )
        {
            V3d r = line1.Origin - line2.Origin;

            double a = V3d.Dot(line1.Direction, line1.Direction);
            double b = V3d.Dot(line1.Direction, line2.Direction);
            double c = V3d.Dot(line1.Direction, r);
            double e = V3d.Dot(line2.Direction, line2.Direction);
            double f = V3d.Dot(line2.Direction, r);

            double d = (a * e - b * b);

            if (d >= 0 - double.Epsilon && d <= 0 + double.Epsilon)
            {
                t_line1 = 0.0;
                line1.Origin.GetClosestPointOn(line2, out t_line2);
            }

            else
            {
                t_line1 = (b * f - c * e) / d;
                t_line2 = (a * f - b * c) / d;
            }
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between line and line segment.
        /// r1 = origin1 + t1 * direction1 and r2 = origin2 + t2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Ray2d line, Line2d segment,
            out double t_line, out double t_segment
            )
        {
            V2d r = line.Origin - segment.Origin;

            double a = V2d.Dot(line.Direction, line.Direction);
            double b = V2d.Dot(line.Direction, segment.Direction);
            double c = V2d.Dot(line.Direction, r);
            double e = V2d.Dot(segment.Direction, segment.Direction);
            double f = V2d.Dot(segment.Direction, r);

            double d = (a * e - b * b);

            if (d >= 0 - double.Epsilon && d <= 0 + double.Epsilon)
            {
                t_line = t_segment = double.PositiveInfinity;
            }

            else
            {
                t_segment = (a * f - b * c) / d;

                if (t_segment <= 0)
                {
                    t_segment = 0;
                    segment.P0.GetClosestPointOn(line, out t_line);
                }

                if (t_segment >= 1)
                {
                    t_segment = 1;
                    segment.P1.GetClosestPointOn(line, out t_line);
                }

                else
                {
                    t_line = (b * f - c * e) / d;
                }
            }
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between line and line segment.
        /// r1 = origin1 + t1 * direction1 and r2 = origin2 + t2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Ray3d line, Line3d segment,
            out double t_line, out double t_segment
            )
        {
            V3d r = line.Origin - segment.Origin;

            double a = V3d.Dot(line.Direction, line.Direction);
            double b = V3d.Dot(line.Direction, segment.Direction);
            double c = V3d.Dot(line.Direction, r);
            double e = V3d.Dot(segment.Direction, segment.Direction);
            double f = V3d.Dot(segment.Direction, r);

            double d = (a * e - b * b);

            if (d >= 0 - double.Epsilon && d <= 0 + double.Epsilon)
            {
                t_line = t_segment = double.PositiveInfinity;
            }

            else
            {
                t_segment = (a * f - b * c) / d;

                if (t_segment <= 0)
                {
                    t_segment = 0;
                    segment.P0.GetClosestPointOn(line, out t_line);
                }

                if (t_segment >= 1)
                {
                    t_segment = 1;
                    segment.P1.GetClosestPointOn(line, out t_line);
                }

                else
                {
                    t_line = (b * f - c * e) / d;
                }
            }
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between line segment and line.
        /// r1 = origin1 + t1 * direction1 and r2 = origin2 + t2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Line2d segment, Ray2d line,
            out double t_segment, out double t_line
            )
        {
            line.GetClosestPoints(segment, out t_line, out t_segment);
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between line segment and line.
        /// r1 = origin1 + t1 * direction1 and r2 = origin2 + t2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Line3d segment,
            Ray3d line, out double t_segment, out double t_line
            )
        {
            line.GetClosestPoints(segment, out t_line, out t_segment);
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between two line segments.
        /// r1 = origin1 + t1 * direction1 and r2 = origin2 + t2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Line2d segment1, Line2d segment2,
            out double t_segment1, out double t_segment2
            )
        {
            /*-----------------------------------------------------------------
            Compute vector between two startingpoints of the linesegments onto r
            -----------------------------------------------------------------*/
            V2d r = segment1.P0 - segment2.P0;

            /*-----------------------------------------------------------------
            Compute squared length of the two line segments with dotproduct
            -----------------------------------------------------------------*/
            double a = V2d.Dot(segment1.Direction, segment1.Direction);
            double e = V2d.Dot(segment2.Direction, segment2.Direction);

            /*-----------------------------------------------------------------
            Compute dotproduct of segment2 and r onto f
            -----------------------------------------------------------------*/
            double f = V2d.Dot(segment2.Direction, r);

            /*-----------------------------------------------------------------
            Check if either or both segments degenerate into points
            -----------------------------------------------------------------*/
            if (a <= double.Epsilon && e <= double.Epsilon)
            {
                /*-----------------------------------------------------------------
                Both segments degenerate into points
                -----------------------------------------------------------------*/
                t_segment1 = t_segment2 = 0.0f;
            }

            if (a <= double.Epsilon)
            {
                /*-----------------------------------------------------------------
                First segment degenerate into a point
                since t1 = 0.0f -> t2 = (b * t1 + f) / e 
                simple: f / e
                clamp t2 if necessary
                -----------------------------------------------------------------*/
                t_segment1 = 0.0f;
                t_segment2 = (f / e).Clamp(0.0f, 1.0f);
            }

            else
            {
                double c = V2d.Dot(segment1.Direction, r);

                if (e <= double.Epsilon)
                {
                    /*-----------------------------------------------------------------
                    Second segment degenerate into a point
                    since t2 = 0.0f -> t1 = (b * t2 - c) / a 
                    simple: -c / a
                    clamp t1 if necessary
                    -----------------------------------------------------------------*/
                    t_segment2 = 0.0f;
                    t_segment1 = (-c / a).Clamp(0.0f, 1.0f);
                }

                else
                {
                    /*-----------------------------------------------------------------
                    General nondegenerate case starts here
                    -----------------------------------------------------------------*/
                    double b = V2d.Dot(segment1.Direction, segment2.Direction);
                    double denom = a * e - b * b;       //must be positive

                    /*-----------------------------------------------------------------
                    If segments aren't parallel, compute closest point on segment1 to
                    segment2 and clamp to segment1 (compute t1)
                    -----------------------------------------------------------------*/
                    if (denom != 0.0f)
                    {
                        t_segment1 = ((b * f - c * e) / denom).Clamp(0.0f, 1.0);
                    }
                    /*-----------------------------------------------------------------
                    Else pick pick arbitary t1 (in this case 0.0f)
                    -----------------------------------------------------------------*/
                    else
                    {
                        t_segment1 = 0.0f;
                    }
                    /*-----------------------------------------------------------------
                    Compute point on segment2 closest to point on segment1 using
                    t2 = Dot((segment1.A + t1 * D1) - segment2.A, D2) / Dot(D2, D2) 
                    simple: (b * t1 + f) / e
                    -----------------------------------------------------------------*/
                    t_segment2 = (b * t_segment1 + f) / e;
                    /*-----------------------------------------------------------------
                    If t2 is in [0, 1] intervall then we are done.
                    Else clamp t2 and recompute t1 for the new value using
                    t1 = Dot((segment2.A + t2 * D2) - segment1.A, D1) / Dot(D1, D1)  
                    simple: (b * t2 - c) / a
                    and clamp t1 finally to [0, 1]
                    -----------------------------------------------------------------*/
                    if (t_segment2 < 0.0f)
                    {
                        t_segment2 = 0.0f;
                        t_segment1 = (-c / a).Clamp(0.0f, 1.0f);
                    }

                    else if (t_segment2 > 1.0f)
                    {
                        t_segment2 = 1.0f;
                        t_segment1 = ((b - c) / a).Clamp(0.0f, 1.0f);
                    }
                }
            }
        }

        /// <summary>
        /// Determines t(1)/t(2) representing closest points between two line segments.
        /// r1 = origin1 + t1 * direction1 and r2 = origin2 + t2 * direction2.
        /// </summary>
        public static void GetClosestPoints(
            this Line3d segment1, Line3d segment2,
            out double t_segment1, out double t_segment2
            )
        {
            /*-----------------------------------------------------------------
            Compute vector between two startingpoints of the linesegments onto r
            -----------------------------------------------------------------*/
            V3d r = segment1.P0 - segment2.P0;

            /*-----------------------------------------------------------------
            Compute squared length of the two line segments with dotproduct
            -----------------------------------------------------------------*/
            double a = V3d.Dot(segment1.Direction, segment1.Direction);
            double e = V3d.Dot(segment2.Direction, segment2.Direction);

            /*-----------------------------------------------------------------
            Compute dotproduct of segment2 and r onto f
            -----------------------------------------------------------------*/
            double f = V3d.Dot(segment2.Direction, r);

            /*-----------------------------------------------------------------
            Check if either or both segments degenerate into points
            -----------------------------------------------------------------*/
            if (a <= double.Epsilon && e <= double.Epsilon)
            {
                /*-----------------------------------------------------------------
                Both segments degenerate into points
                -----------------------------------------------------------------*/
                t_segment1 = t_segment2 = 0.0f;
            }

            if (a <= double.Epsilon)
            {
                /*-----------------------------------------------------------------
                First segment degenerate into a point
                since t1 = 0.0f -> t2 = (b * t1 + f) / e 
                simple: f / e
                clamp t2 if necessary
                -----------------------------------------------------------------*/
                t_segment1 = 0.0f;
                t_segment2 = (f / e).Clamp(0.0f, 1.0f);
            }

            else
            {
                double c = V3d.Dot(segment1.Direction, r);

                if (e <= double.Epsilon)
                {
                    /*-----------------------------------------------------------------
                    Second segment degenerate into a point
                    since t2 = 0.0f -> t1 = (b * t2 - c) / a 
                    simple: -c / a
                    clamp t1 if necessary
                    -----------------------------------------------------------------*/
                    t_segment2 = 0.0f;
                    t_segment1 = (-c / a).Clamp(0.0f, 1.0f);
                }

                else
                {
                    /*-----------------------------------------------------------------
                    General nondegenerate case starts here
                    -----------------------------------------------------------------*/
                    double b = V3d.Dot(segment1.Direction, segment2.Direction);
                    double denom = a * e - b * b;       //must be positive
                    /*-----------------------------------------------------------------
                    If segments aren't parallel, compute closest point on segment1 to
                    segment2 and clamp to segment1 (compute t1)
                    -----------------------------------------------------------------*/
                    if (denom != 0.0f)
                    {
                        t_segment1 = ((b * f - c * e) / denom).Clamp(0.0f, 1.0);
                    }
                    /*-----------------------------------------------------------------
                    Else pick pick arbitary t1 (in this case 0.0f)
                    -----------------------------------------------------------------*/
                    else
                    {
                        t_segment1 = 0.0f;
                    }
                    /*-----------------------------------------------------------------
                    Compute point on segment2 closest to point on segment1 using
                    t2 = Dot((segment1.A + t1 * D1) - segment2.A, D2) / Dot(D2, D2) 
                    simple: (b * t1 + f) / e
                    -----------------------------------------------------------------*/
                    t_segment2 = (b * t_segment1 + f) / e;
                    /*-----------------------------------------------------------------
                    If t2 is in [0, 1] intervall then we are done.
                    Else clamp t2 and recompute t1 for the new value using
                    t1 = Dot((segment2.A + t2 * D2) - segment1.A, D1) / Dot(D1, D1)  
                    simple: (b * t2 - c) / a
                    and clamp t1 finally to [0, 1]
                    -----------------------------------------------------------------*/
                    if (t_segment2 < 0.0f)
                    {
                        t_segment2 = 0.0f;
                        t_segment1 = (-c / a).Clamp(0.0f, 1.0f);
                    }

                    else if (t_segment2 > 1.0f)
                    {
                        t_segment2 = 1.0f;
                        t_segment1 = ((b - c) / a).Clamp(0.0f, 1.0f);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates closest points between line and box.
        /// t represents the point on the line (p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on box.</returns>
        public static V2d GetClosestPoints(
            this Ray2d line, Box2d box, out double t
            )
        {
            if (line.Direction.X * line.Direction.Y > 0)
            {
                double xdim = (box.Min.X - line.Origin.X) / line.Direction.X;
                if (xdim * line.Direction.Y + line.Origin.Y > box.Max.Y)
                {
                    V2d ret = new V2d(box.Min.X, box.Max.Y);
                    ret.GetClosestPointOn(line, out t);
                    return ret;
                }
                else
                {
                    V2d ret = new V2d(box.Max.X, box.Min.Y);
                    ret.GetClosestPointOn(line, out t);
                    return ret;
                }
            }
            else if (line.Direction.X * line.Direction.Y < 0)
            {
                double xdim = (box.Min.X - line.Origin.X) / line.Direction.X;
                if (xdim * line.Direction.Y + line.Origin.Y < box.Min.Y)
                {
                    V2d ret = new V2d(box.Min.X, box.Min.Y);
                    ret.GetClosestPointOn(line, out t);
                    return ret;
                }
                else
                {
                    V2d ret = new V2d(box.Max.X, box.Max.Y);
                    ret.GetClosestPointOn(line, out t);
                    return ret;
                }
            }
            else
            {
                if (line.Direction.X == 0)
                {
                    if (line.Origin.Y > box.Max.Y)
                    {
                        t = box.Max.Y - line.Origin.Y;
                        return new V2d(line.Origin.X, box.Max.Y);
                    }
                    else if (line.Origin.Y < box.Min.Y)
                    {
                        t = box.Min.Y - line.Origin.Y;
                        return new V2d(line.Origin.X, box.Min.Y);
                    }
                    else
                    {
                        if (line.Origin.X > box.Max.X || line.Origin.X < box.Max.X)
                        {
                            t = 0;
                            return line.Origin;
                        }
                        else
                        {
                            // TODO if new interface gets implemented replace ((box.Min + box.Max) * 0.5) by CalculateCenter
                            if (line.Origin.Y > ((box.Min + box.Max) * 0.5).Y)
                            {
                                t = box.Max.Y - line.Origin.Y;
                                return new V2d(line.Origin.X, box.Max.Y);
                            }
                            else
                            {
                                t = box.Min.Y - line.Origin.Y;
                                return new V2d(line.Origin.X, box.Min.Y);
                            }
                        }
                    }
                }
                else
                {
                    if (line.Origin.X > box.Max.X)
                    {
                        t = box.Max.X - line.Origin.X;
                        return new V2d(box.Max.X, line.Origin.Y);
                    }
                    else if (line.Origin.X < box.Min.X)
                    {
                        t = box.Min.X - line.Origin.X;
                        return new V2d(box.Min.X, line.Origin.Y);
                    }
                    else
                    {
                        if (line.Origin.Y > box.Max.Y || line.Origin.Y < box.Max.Y)
                        {
                            t = 0;
                            return line.Origin;
                        }
                        else
                        {
                            if (line.Origin.X > ((box.Min + box.Max) * 0.5).X)
                            {
                                t = box.Max.X - line.Origin.X;
                                return new V2d(box.Max.X, line.Origin.Y);
                            }
                            else
                            {
                                t = box.Min.X - line.Origin.X;
                                return new V2d(box.Min.X, line.Origin.Y);
                            }
                        }
                    }
                }
            }
        }

        // TODO exact check for this method needed - what did dietmar do in this one?
        // TODO should this method also use the Box3d interface?

        /// <summary>
        /// Calculates closest points between line and box.
        /// t represents the point on the line (p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on box.</returns>
        public static V3d GetClosestPoints(
            this Ray3d line, Box3f box, out double t
            )
        {
            t = 0.0f;
            /*-----------------------------------------------------------------
            Method for x,y,z != 0.
            -----------------------------------------------------------------*/
            if ((line.Direction.X * line.Direction.Y * line.Direction.Z) != 0)
            {
            /*-----------------------------------------------------------------
            Get closest corner of box to origin to start with.
            -----------------------------------------------------------------*/
                V3d calc = line.Origin.GetClosestPointOn((Box3d)box);
                for (int i = 0; i < 3; i++)
                {
                    /*-----------------------------------------------------------------
                    Get closest corner if closest point is not corner point (if box is intersected).
                    -----------------------------------------------------------------*/
                    if (calc[i] > box.Min[i] + (box.Max[i] - box.Min[i]) / 2)
                    {
                        calc[i] = box.Max[i];
                    }
                    else
                    {
                        calc[i] = box.Min[i];
                    }
                }
                V3d close = V3d.Zero;
                V3d close2 = V3d.Zero;
                double dist_comp = double.MaxValue;
                double dist_comp2 = double.MaxValue;
                int outside = 0;
                V3d t1 = V3d.Zero;
                V3d t2 = V3d.Zero;

                /*-----------------------------------------------------------------
                Process all sides originatig in closest corner point.
                This is done by intesecting line with a plane (planes represent sides).
                Finally distance of intersection point and boxes vertices are compared
                and saved if a lesser distance was found.
                -----------------------------------------------------------------*/
                for (int j = 0; j < 3; j++)
                {
                    V3d dir = V3d.Zero;
                    dir[j] = 1;
                    Plane3d distance = new Plane3d(dir, calc);
                    V3d holder = V3d.Zero;
                    bool a = IntersectionTests.Intersect(line, distance, out dist_comp, out holder);
                    dist_comp = dist_comp * dist_comp;
                    double ho_ca = V3d.DistanceSquared(holder, calc);
                    if (holder[j] >= box.Max[j] || holder[j] <= box.Min[j])
                    {
                        outside = outside++;
                    }
                    if (ho_ca <= dist_comp)
                    {
                        close2 = close;
                        close = holder;
                        dist_comp2 = ho_ca = dist_comp;
                    }
                    else if (ho_ca < dist_comp2)
                    {
                        close2 = holder;
                        dist_comp2 = ho_ca;
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    if ((t1[j] - t2[j]).Abs() < dist_comp)
                    {
                        dist_comp = (t1[j] - t2[j]).Abs();
                    }
                }
                if (outside > 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (calc[i] <= box.Min[i])
                        {
                            calc[i] = box.Min[i];
                        }
                        else if (calc[i] >= box.Max[i])
                        {
                            calc[i] = box.Max[i];
                        }
                    }
                    return calc;
                }
                V3d b_a = V3d.Zero;
                V3d b_b = V3d.Zero;
                int x = 0;
                for (int j = 0; j < 3; j++)
                {
                    if(close[j] == box.Max[j] || close[j] == box.Min[j])
                    {
                        b_a[j] = close[j];
                        b_b[j] = close[j];
                    }
                    else
                    {
                        b_a[j] = box.Min[j];
                        b_b[j] = box.Max[j];
                        x = j;
                    }
                }
                var seg = new Line3d(b_a, (b_b - b_a));
                seg.GetClosestPoints(line, out dist_comp, out t);
                b_a[x] = b_a[x] + ((b_b[x] - b_a[x]) * dist_comp);
                return b_a;
            }
            else
            /*-----------------------------------------------------------------
            Method for x*y*z == 0 (at least one value must be zero).
            -----------------------------------------------------------------*/
            {
                double i = 0;
                double j = 0;
                int k, co = 0;
                /*-----------------------------------------------------------------
                If one dimension is zero convert to V2d (missing the 0 coordinate)
                and run Line2AABB in 2d space since distance must be equal.
                Return point is then extended again by the missin 0 coordinate.
                -----------------------------------------------------------------*/
                for (k = 0; k < 3; k++)
                {
                    if (line.Direction[k] != 0 && i == 0) i = line.Direction[k];
                    else if (line.Direction[k] != 0) j = line.Direction[k];
                    else co = k;
                }
                V2d dir2 = new V2d(i, j);
                i = j = 0;
                for (k = 0; k < 3; k++)
                {
                    if (k == co){}
                    else if (i == 0) i = line.Origin[k];
                    else { j = line.Origin[k]; }
                }
                V2d or2 = new V2d(i, j);
                i = j = 0;
                // TODO why create a new Ray2d and use the Ray2d constructor?
                Ray2d line_2 = new Ray2d(or2, dir2);
                for (k = 0; k < 3; k++)
                {
                    if (k == co) { }
                    else if (i == 0) i = box.Min[k];
                    else { j = box.Min[k]; }
                }
                dir2 = new V2d(i, j);
                i = j = 0;
                for (k = 0; k < 3; k++)
                {
                    if (k == co) { }
                    else if (i == 0) i = box.Max[k];
                    else { j = box.Max[k]; }
                }
                V2d dir3 = new V2d(i, j);
                Box2d box_2 = new Box2d((V2d)dir2, (V2d)dir3);
                V2d res = line_2.GetClosestPoints(box_2, out t);
                V3d ret = V3d.Zero;
                bool first = true;
                for (k = 0; k < 3; k++)
                {
                    if (k == co)
                    {
                        ret[k] = line.Origin[k];
                    }
                    else if (first)
                    {
                        ret[k] = res.X;
                        first = false;
                    }
                    else ret[k] = res.Y;
                }
                return ret;
            }
        }

        /// <summary>
        /// Calculates closest points between ray and box.
        /// t represents the point on the ray (p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on box.</returns>
        public static V2d GetClosestPoints(
            this Ray2d ray, Box2f box, out double t
            )
        {
            if (ray.Direction.X * ray.Direction.Y > 0)
            {
                double xdim = (box.Min.X - ray.Origin.X) / ray.Direction.X;
                if (xdim * ray.Direction.Y + ray.Origin.Y > box.Max.Y)
                {
                    V2d query = new V2d(box.Min.X, box.Max.Y).GetClosestPointOn(ray, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (ray.Origin[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (ray.Origin[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = ray.Origin[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Min.X, box.Max.Y);
                    }
                }
                else
                {
                    V2d query = new V2d(box.Max.X, box.Min.Y).GetClosestPointOn(ray, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (ray.Origin[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (ray.Origin[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = ray.Origin[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Max.X, box.Min.Y);
                    }
                }
            }
            else if (ray.Direction.X * ray.Direction.Y < 0)
            {
                double xdim = (box.Min.X - ray.Origin.X) / ray.Direction.X;
                if (xdim * ray.Direction.Y + ray.Origin.Y < box.Min.Y)
                {
                    V2d query = new V2d(box.Min.X, box.Min.Y).GetClosestPointOn(ray, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (ray.Origin[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (ray.Origin[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = ray.Origin[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Min.X, box.Min.Y);
                    }
                }
                else
                {
                    V2d query = new V2d(box.Max.X, box.Max.Y).GetClosestPointOn(ray, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (ray.Origin[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (ray.Origin[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = ray.Origin[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Max.X, box.Max.Y);
                    }
                }
            }
            else
            {
                if (ray.Direction.X == 0)
                {
                    if (ray.Origin.Y > box.Max.Y)
                    {
                        t = (box.Max.Y - ray.Origin.Y) / ray.Direction.Y;
                        return new V2d(ray.Origin.X, box.Max.Y);
                    }
                    else if (ray.Origin.Y < box.Min.Y)
                    {
                        t = (box.Min.Y - ray.Origin.Y) / ray.Direction.Y;;
                        return new V2d(ray.Origin.X, box.Min.Y);
                    }
                    else
                    {
                        if (ray.Origin.X > box.Max.X) 
                        {
                            t = 0;
                            return new V2d(box.Max.X, ray.Origin.Y);
                        }
                        else if (ray.Origin.X < box.Min.X)
                        {
                            t = 0;
                            return new V2d(box.Min.X, ray.Origin.Y);
                        }
                        else
                        {
                            if (ray.Origin.Y > box.Center.Y)
                            {
                                t = (box.Max.Y - ray.Origin.Y) / ray.Direction.Y;
                                return new V2d(ray.Origin.X, box.Max.Y);
                            }
                            else
                            {
                                t = (box.Min.Y - ray.Origin.Y) / ray.Direction.Y;
                                return new V2d(ray.Origin.X, box.Min.Y);
                            }
                        }
                    }
                }
                else
                {
                    if (ray.Origin.X > box.Max.X)
                    {
                        t = (box.Max.X - ray.Origin.X) / ray.Direction.X;
                        return new V2d(box.Max.X, ray.Origin.Y);
                    }
                    else if (ray.Origin.X < box.Min.X)
                    {
                        t = (box.Min.X - ray.Origin.X) / ray.Direction.X;
                        return new V2d(box.Min.X, ray.Origin.Y);
                    }
                    else
                    {
                        if (ray.Origin.Y > box.Max.Y) 
                        {
                            t = 0;
                            return new V2d(ray.Origin.X, box.Max.Y);
                        }
                        else if(ray.Origin.Y < box.Min.Y)
                        {
                            t = 0;
                            return new V2d(ray.Origin.X, box.Min.Y);
                        }
                        else
                        {
                            if (ray.Origin.X > box.Center.X)
                            {
                                t = box.Max.X - ray.Origin.X;
                                return new V2d(box.Max.X, ray.Origin.Y);
                            }
                            else
                            {
                                t = box.Min.X - ray.Origin.X;
                                return new V2d(box.Min.X, ray.Origin.Y);
                            }
                        }
                    }
                }
            }
        }

        public static V3d GetClosestPoints(
            this Ray3d ray, Box3d box, out double t
            )
        {
            {
                t = 0.0f;
                /*-----------------------------------------------------------------
                Method for x,y,z != 0.
                -----------------------------------------------------------------*/
                if (ray.Direction.X != 0 && ray.Direction.Y != 0 && ray.Direction.Z != 0)
                {
                    /*-----------------------------------------------------------------
                    Get closest corner of box to origin to start with.
                    -----------------------------------------------------------------*/
                    V3d calc = ray.Origin.GetClosestPointOn((Box3d)box);
                    for (int i = 0; i < 3; i++)
                    {
                        /*-----------------------------------------------------------------
                        Get closest corner if closest point is not corner point (if box is intersected).
                        -----------------------------------------------------------------*/
                        if (calc[i] > box.Min[i] + (box.Max[i] - box.Min[i]) / 2)
                        {
                            calc[i] = box.Max[i];
                        }
                        else
                        {
                            calc[i] = box.Min[i];
                        }
                    }
                    V3d close = V3d.Zero;
                    V3d close2 = V3d.Zero;
                    double dist_comp = double.MaxValue;
                    double dist_comp2 = double.MaxValue;
                    int outside = 0;
                    /*-----------------------------------------------------------------
                    Process all sides originatig in closest corner point.
                    This is done by intesecting ray with a plane (planes represent sides).
                    Finally distance of intersection point and boxes vertices are compared
                    and saved if a lesser distance was found.
                    -----------------------------------------------------------------*/
                    for (int j = 0; j < 3; j++)
                    {
                        V3d dir = V3d.Zero;
                        dir[j] = 1;
                        Plane3d distance = new Plane3d(dir, calc);
                        V3d holder = V3d.Zero;
                        bool a = IntersectionTests.Intersect(ray, distance, out dist_comp, out holder);
                        double ho_ca = V3d.DistanceSquared(holder, calc);
                        dist_comp = dist_comp * dist_comp;
                        if (holder[j] > box.Max[j] || holder[j] < box.Min[j])
                        {
                            outside = outside++;
                        }

                        if (ho_ca <= dist_comp)
                        {
                            close2 = close;
                            close = holder;
                            dist_comp2 = t = ho_ca = dist_comp;
                        }
                        else if (ho_ca < dist_comp2)
                        {
                            close2 = holder;
                            dist_comp2 = ho_ca;
                        }
                    }
                    if (t < 0.0f) 
                    { 
                        t = 0.0f; 
                    }
                    if (outside > 2)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (calc[i] <= box.Min[i])
                            {
                                calc[i] = box.Min[i];
                            }
                            else if (calc[i] >= box.Max[i])
                            {
                                calc[i] = box.Max[i];
                            }
                        }
                        return calc;
                    }
                    V3d b_a = V3d.Zero;
                    V3d b_b = V3d.Zero;
                    int x = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        if (close[j] == box.Max[j] || close[j] == box.Min[j])
                        {
                            b_a[j] = close[j];
                            b_b[j] = close[j];
                        }
                        else
                        {
                            b_a[j] = box.Min[j];
                            b_b[j] = box.Max[j];
                            x = j;
                        }
                    }
                    // TODO why create a new LineSegment3d and use the Ray3d constructor?
                    var seg = new Line3d(b_a, (b_b - b_a));
                    seg.GetClosestPoints(ray, out dist_comp, out t);
                    b_a[x] = b_a[x] + ((b_b[x] - b_a[x]) * dist_comp);
                    return b_a;
                }
                else
                /*-----------------------------------------------------------------
                Method for x*y*z == 0 (at least one value must be zero).
                -----------------------------------------------------------------*/
                {
                    double i = 0;
                    double j = 0;
                    int k, co = 0;
                    /*-----------------------------------------------------------------
                    If one dimension is zero convert to V2d (missing the 0 coordinate)
                    and run ray2AABB in 2d space since distance (t) must be equal.
                    Return point is then extended again by the missing 0 coordinate.
                    -----------------------------------------------------------------*/
                    for (k = 0; k < 3; k++)
                    {
                        if (ray.Direction[k] != 0 && i == 0) i = ray.Direction[k];
                        else if (ray.Direction[k] != 0) j = ray.Direction[k];
                        else co = k;
                    }
                    V2d dir2 = new V2d(i, j);
                    i = j = 0;
                    for (k = 0; k < 3; k++)
                    {
                        if (k == co) { }
                        else if (i == 0) i = ray.Origin[k];
                        else { j = ray.Origin[k]; }
                    }
                    V2d or2 = new V2d(i, j);
                    i = j = 0;
                    Ray2d line_2 = new Ray2d(or2, dir2);
                    for (k = 0; k < 3; k++)
                    {
                        if (k == co) { }
                        else if (i == 0) i = box.Min[k];
                        else { j = box.Min[k]; }
                    }
                    dir2 = new V2d(i, j);
                    i = j = 0;
                    for (k = 0; k < 3; k++)
                    {
                        if (k == co) { }
                        else if (i == 0) i = box.Max[k];
                        else { j = box.Max[k]; }
                    }
                    V2d dir3 = new V2d(i, j);
                    Box2d box_2 = new Box2d((V2d)dir2, (V2d)dir3);
                    V2d res = line_2.GetClosestPoints(box_2, out t);
                    V3d ret = V3d.Zero;
                    bool first = true;
                    for (k = 0; k < 3; k++)
                    {                 
                        if (k == co) 
                        {
                            ret[k] = ray.Origin[k];
                        }
                        else if (first)
                        {
                            ret[k] = res.X;
                            first = false;
                        }
                        else ret[k] = res.Y;
                    }
                    return ret;
                }
            }
        }

        /// <summary>
        /// Calculates closest points between line segment and box.
        /// t represents the point on the line segment(p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on box.</returns>
        public static V2d GetClosestPoints(
            this Line2d segment, Box2d box, out double t
            )
        {
            if (segment.Direction.X * segment.Direction.Y > 0)
            {
                double xdim = (box.Min.X - segment.Origin.X) / segment.Direction.X;
                if (xdim * segment.Direction.Y + segment.Origin.Y > box.Max.Y)
                {
                    V2d query = new V2d(box.Min.X, box.Max.Y).GetClosestPointOn(segment, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P0[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P0[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P0[i];
                            }
                        }
                        return query;
                    }
                    else if (t > 1)
                    {
                        t = 1;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P1[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P1[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P1[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Min.X, box.Max.Y);
                    }
                }
                else
                {
                    V2d query = new V2d(box.Max.X, box.Min.Y).GetClosestPointOn(segment, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P0[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P0[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P0[i];
                            }
                        }
                        return query;
                    }
                    else if (t > 1)
                    {
                        t = 1;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P1[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P1[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P1[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Max.X, box.Min.Y);
                    }
                }
            }
            else if (segment.Direction.X * segment.Direction.Y < 0)
            {
                double xdim = (box.Min.X - segment.Origin.X) / segment.Direction.X;
                if (xdim * segment.Direction.Y + segment.Origin.Y < box.Min.Y)
                {
                    V2d query = new V2d(box.Min.X, box.Min.Y).GetClosestPointOn(segment, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P0[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P0[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P0[i];
                            }
                        }
                        return query;
                    }
                    else if (t > 1)
                    {
                        t = 1;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P1[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P1[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P1[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Min.X, box.Min.Y);
                    }
                }
                else
                {
                    V2d query = new V2d(box.Max.X, box.Max.Y).GetClosestPointOn(segment, out t);
                    if (t < 0)
                    {
                        t = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P0[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P0[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P0[i];
                            }
                        }
                        return query;
                    }
                    else if (t > 1)
                    {
                        t = 1;
                        for (int i = 0; i < 2; i++)
                        {
                            if (segment.P1[i] > box.Max[i])
                            {
                                query[i] = box.Max[i];
                            }
                            else if (segment.P1[i] < box.Min[i])
                            {
                                query[i] = box.Min[i];
                            }
                            else
                            {
                                query[i] = segment.P1[i];
                            }
                        }
                        return query;
                    }
                    else
                    {
                        return new V2d(box.Max.X, box.Max.Y);
                    }
                }
            }
            else
            {
                if (segment.Direction.X == 0)
                {
                    if (segment.Origin.Y > box.Max.Y)
                    {
                        t = (box.Max.Y - segment.Origin.Y) / segment.Direction.Y;
                        if (t < 0) { t = 0; }
                        else if (t > 1) { t = 1; }
                        return new V2d(segment.Origin.X, box.Max.Y);
                    }
                    else if (segment.Origin.Y < box.Min.Y)
                    {
                        t = (box.Min.Y - segment.Origin.Y) / segment.Direction.Y;
                        if (t < 0) { t = 0; }
                        else if (t > 1) { t = 1; }
                        return new V2d(segment.Origin.X, box.Min.Y);
                    }
                    else
                    {
                        if (segment.Origin.X > box.Max.X || segment.Origin.X < box.Max.X)
                        {
                            t = 0;
                            return segment.Origin;
                        }
                        else
                        {
                            // TODO replace this with CalculateCenter when interface implements it
                            if (segment.Origin.Y > ((box.Min + box.Max) * 0.5).Y)
                            {
                                t = (box.Max.Y - segment.Origin.Y) / segment.Direction.Y;
                                if (t < 0) { t = 0; }
                                else if (t > 1) { t = 1; }
                                return new V2d(segment.Origin.X, box.Max.Y);
                            }
                            else
                            {
                                t = (box.Min.Y - segment.Origin.Y) / segment.Direction.Y;
                                if (t < 0) { t = 0; }
                                else if (t > 1) { t = 1; }
                                return new V2d(segment.Origin.X, box.Min.Y);
                            }
                        }
                    }
                }
                else
                {
                    if (segment.Origin.X > box.Max.X)
                    {
                        t = (box.Max.X - segment.Origin.X) / segment.Direction.X;
                        if (t < 0) { t = 0; }
                        else if (t > 1) { t = 1; }
                        return new V2d(box.Max.X, segment.Origin.Y);
                    }
                    else if (segment.Origin.X < box.Min.X)
                    {
                        t = (box.Min.X - segment.Origin.X) / segment.Direction.X;
                        if (t < 0) { t = 0; }
                        else if (t > 1) { t = 1; }
                        return new V2d(box.Min.X, segment.Origin.Y);
                    }
                    else
                    {
                        if (segment.Origin.Y > box.Max.Y)
                        {
                            t = 0;
                            return new V2d(segment.Origin.X, box.Max.Y);
                        }
                        else if (segment.Origin.Y < box.Min.Y)
                        {
                            t = 0;
                            return new V2d(segment.Origin.X, box.Min.Y);
                        }
                        else
                        {
                            // TODO replace this with CalculateCenter when interface implements it
                            if (segment.Origin.X > ((box.Min + box.Max) * 0.5).X)
                            {
                                t = (box.Max.X - segment.Origin.X) / segment.Direction.X;
                                if (t < 0) { t = 0; }
                                else if (t > 1) { t = 1; }
                                return new V2d(box.Max.X, segment.Origin.Y);
                            }
                            else
                            {
                                t = (box.Min.X - segment.Origin.X) / segment.Direction.X;
                                if (t < 0) { t = 0; }
                                else if (t > 1) { t = 1; }
                                return new V2d(box.Min.X, segment.Origin.Y);
                            }
                        }
                    }
                }
            }
        }

        public static V3d GetClosestPoints(
            this Line3d segment, Box3d box, out double t
            )
        {
            t = 0.0f;
            V3d calca = V3d.Zero;
            V3d calcb = V3d.Zero;
            V3d outside = V3d.Zero;
            int outs = 0;
            int i;
            
            /*-----------------------------------------------------------------
            Loop to determine if and where both endpoints of the line segment 
            are outside the box. 
            -----------------------------------------------------------------*/
            for (i = 0; i < 3; i++)
            {
                if (segment.P0[i] > box.Max[i])
                {
                    calca[i] = segment.P0[i] - box.Max[i];
                }
                else if (segment.P0[i] < box.Min[i])
                {
                    calca[i] = segment.P0[i] - box.Min[i];
                }
                if (segment.P1[i] > box.Max[i])
                {
                    calcb[i] = segment.P1[i] - box.Max[i];
                }
                else if (segment.P0[i] < box.Min[i])
                {
                    calcb[i] = segment.P1[i] - box.Min[i];
                }
                if (calca[i] > 0 && calcb[i] > 0)
                {
                    outside[i] = 1.0f;
                    outs++;
                }
                else if (calca[i] < 0 && calcb[i] < 0)
                {
                    outside[i] = -1.0f;
                    outs++;
                }
            }
            /*-----------------------------------------------------------------
            Switch to determine futher processing depending on how much dividing 
            planes there are between both points and the box.
            -----------------------------------------------------------------*/
            switch (outs)
            {
                case (0):
                    {
                        if (((calca[0] + calca[1] + calca[2]) == 0) || ((calcb[0] + calcb[1] + calcb[2]) == 0))
                        {
                            segment.Intersect(box, out t);
                            return (segment.Direction * t + segment.P0);             
                        }
                        int compare = 0;
                        int pla = 0;
                        /*-----------------------------------------------------------------
                        Compares the 2 closest corner points from each segment endpoint.
                        -----------------------------------------------------------------*/
                        for (i = 0; i < 3; i++)
                        {
                            if (calca[i] != 0)
                            {
                                if (segment.P0[i] + calca[i] == segment.P1[i] + calcb[i])
                                {
                                    compare++;
                                    pla = i;
                                }
                            }
                        }
                        /*-----------------------------------------------------------------
                        Closest point is on boxes vertice between both closest corner 
                        points.
                        -----------------------------------------------------------------*/
                        if (compare == 2)
                        {
                            var vertice = new Line3d(calca, (calcb - calca));
                            double vl;
                            vertice.GetClosestPoints(segment, out t, out vl);
                            return (vertice.Origin + (vertice.Direction * vl));
                        }
                        /*-----------------------------------------------------------------
                        Segment points are on different sides of the box.
                        -----------------------------------------------------------------*/
                        else if (compare == 0)
                        {
                            segment.Intersect(box, out t);
                            return (segment.Direction * t + segment.P0); 
                        }
                        /*-----------------------------------------------------------------
                        Closest corner point on box is in both cases the same closest corner 
                        and therefore closest point.
                        -----------------------------------------------------------------*/
                        else if (compare == 3)
                        {
                            calca = segment.P1 + calcb;
                            t = (calca[0] - segment.P0[0]) / segment.Direction[0];
                            return calca;
                        }
                        else
                        {
                            V3d ret = new V3d(V3d.Zero);
                            if (segment.P1[pla] < box.Min[pla] || segment.P0[pla] < box.Min[pla])
                            {
                                if (segment.P0[pla] > segment.P1[pla])
                                {
                                    outside = segment.P0 + calca;
                                    double t_comp = 0.0f;
                                    for(i = 0; i < 3; i++)
                                    {
                                        t_comp = (outside[i] - segment.P0[i]) / segment.Direction[i];
                                        if(t_comp < t) { t = t_comp; }
                                    }
                                    ret = segment.Direction * t + segment.P0;
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (ret[j] > box.Max[j])
                                        {
                                            ret[j] = box.Max[j];
                                        }
                                        else if (ret[j] < box.Min[j])
                                        {
                                            ret[j] = box.Min[j];
                                        }
                                    }
                                    return ret;
                                }
                                else
                                {
                                    outside = segment.P1 + calcb;
                                    double t_comp = 0.0f;
                                    for (i = 0; i < 3; i++)
                                    {
                                        t_comp = (outside[i] - segment.P1[i]) / segment.Direction[i];
                                        if (t_comp > t) { t = t_comp; }
                                    }
                                    t = 1 - t.Abs();
                                    ret = segment.Direction * t + segment.P0;
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (ret[j] > box.Max[j])
                                        {
                                            ret[j] = box.Max[j];
                                        }
                                        else if (ret[j] < box.Min[j])
                                        {
                                            ret[j] = box.Min[j];
                                        }
                                    }
                                    return ret;
                                }
                            }
                            else
                            {
                                if (segment.P0[pla] < segment.P1[pla])
                                {
                                    outside = segment.P0 + calca;
                                    double t_comp = 0.0f;
                                    for (i = 0; i < 3; i++)
                                    {
                                        t_comp = (outside[i] - segment.P0[i]) / segment.Direction[i];
                                        if (t_comp < t) { t = t_comp; }
                                    }
                                    ret = segment.Direction * t + segment.P0;
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (ret[j] > box.Max[j])
                                        {
                                            ret[j] = box.Max[j];
                                        }
                                        else if (ret[j] < box.Min[j])
                                        {
                                            ret[j] = box.Min[j];
                                        }
                                    }
                                    return ret;
                                }
                                else
                                {
                                    outside = segment.P1 + calcb;
                                    double t_comp = 0.0f;
                                    for (i = 0; i < 3; i++)
                                    {
                                        t_comp = (outside[i] - segment.P1[i]) / segment.Direction[i];
                                        if (t_comp > t) { t = t_comp; }
                                    }
                                    t = 1 - t.Abs();
                                    ret = segment.Direction * t + segment.P0;
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (ret[j] > box.Max[j])
                                        {
                                            ret[j] = box.Max[j];
                                        }
                                        else if (ret[j] < box.Min[j])
                                        {
                                            ret[j] = box.Min[j];
                                        }
                                    }
                                    return ret;
                                }
                            }
                        }
                    }

                /*-----------------------------------------------------------------
                Exactly 1 seperating plane between both points and the box.
                -----------------------------------------------------------------*/
                case (1):
                    {
                        int dim = 0;
                        int pla = 0;
                        /*-----------------------------------------------------------------
                        Determine on which axis both points and the box are seperated and if
                        one of the points values lies inside the interval of the box.
                        -----------------------------------------------------------------*/
                        for (i = 0; i < 3; i++)
                        {
                            if ((calca[i] == 0 && calcb[i] != 0) || (calcb[i] == 0 && calca[i] != 0))
                            {
                                pla = i;
                            }
                            if (outside[i] != 0)
                            {
                                dim = i;
                            }
                        }
                        /*-----------------------------------------------------------------
                        This case happens if both points inside interval or both outside 
                        (for the 2 remaining coordinates).
                        -----------------------------------------------------------------*/
                        if (pla == 0)
                        {
                            double tmin = 0.0f;
                            double tmax = 1.0f;
                            double var;
                            /*-----------------------------------------------------------------
                            Loop for calculating the t to get to both points on the edges of the
                            box interval, since one of them has to be closest.
                            -----------------------------------------------------------------*/
                            for (i = 0; i < 3; i++)
                            {
                                if (i != dim)
                                {
                                    var = box.Max[i] - segment.P0[i] / segment.Direction[i];
                                    if (var > 1.0f || var > tmax || var < tmin) { }
                                    else { tmax = var; }
                                    var = box.Min[i] - segment.P0[i] / segment.Direction[i];
                                    if (var < 0.0f || var < tmax || var > tmax) { }
                                    else { tmin = var; }
                                }
                            }
                            /*-----------------------------------------------------------------
                            Looking for point with minimum distance to box and calculating t 
                            for segement and according point on box.
                            -----------------------------------------------------------------*/
                            if (tmin > tmax)
                            {
                                if (segment.P0[dim] > box.Max[dim])
                                {
                                    t = tmax;
                                    calca = t * segment.Direction + segment.P0;
                                    calca[dim] = box.Max[dim];
                                }
                                else
                                {
                                    t = tmin;
                                    calca = t * segment.Direction + segment.P0;
                                    calca[dim] = box.Min[dim];
                                }
                                return calca;
                            }
                            else if (tmin < tmax)
                            {
                                if (segment.P0[dim] > box.Max[dim])
                                {
                                    t = tmin;
                                    calca = t * segment.Direction + segment.P0;
                                    calca[dim] = box.Max[dim];
                                }
                                else
                                {
                                    t = tmax;
                                    calca = t * segment.Direction + segment.P0;
                                    calca[dim] = box.Min[dim];
                                }
                                return calca;
                            }
                        }
                        else
                        {
                            if (calca[pla] == 0)
                            {
                                if (((segment.P0[pla] < segment.P1[pla]) && (segment.P1[pla] > box.Max[pla])) || ((segment.P0[pla] > segment.P1[pla]) && (segment.P1[pla] < box.Min[pla])))
                                {
                                    for (i = 0; i < 3; i++)
                                    {
                                        if (segment.P0[i] > box.Max[i])
                                        {
                                            calca[i] = box.Max[i];
                                        }
                                        else if (segment.P0[i] < box.Min[i])
                                        {
                                            calca[i] = box.Min[i];
                                        }
                                        else { calca[i] = segment.P0[i]; }
                                    }
                                    return calca;
                                }
                            }
                            else if (calcb[pla] == 0)
                            {
                                if (((segment.P1[pla] < segment.P0[pla]) && (segment.P0[pla] > box.Max[pla])) || ((segment.P1[pla] > segment.P0[pla]) && (segment.P0[pla] < box.Min[pla])))
                                {
                                    for (i = 0; i < 3; i++)
                                    {
                                        if (segment.P1[i] > box.Max[i])
                                        {
                                            calcb[i] = box.Max[i];
                                        }
                                        else if (segment.P1[i] < box.Min[i])
                                        {
                                            calcb[i] = box.Min[i];
                                        }
                                        else { calcb[i] = segment.P1[i]; }
                                    }
                                    t = 1.0f;
                                    return calcb;
                                }
                                else
                                {
                                    for (i = 0; i < 3; i++)
                                    {
                                        if(i == dim)
                                        {
                                            if(segment.P0[i] > box.Max[i])
                                            {
                                                calca[i] = box.Max[i];
                                                calcb[i] = box.Max[i];
                                            }
                                            else
                                            {
                                                calca[i] = box.Min[i];
                                                calcb[i] = box.Min[i];
                                            }
                                        }
                                        else if (i == pla)
                                        {
                                            if (calca[i] > box.Max[i])
                                            {
                                                calca[i] = box.Max[i];
                                            }
                                            else
                                            {
                                                calca[i] = box.Min[i];
                                            }
                                            calcb[i] = segment.P1[i];
                                        }
                                        else
                                        {
                                            if (segment.P0[i] > box.Max[i])
                                            {

                                            }
                                            else if (segment.P0[i] > box.Min[i])
                                            {

                                            }
                                            else
                                            {
                                                calca[i] = segment.P0[i];
                                            }
                                        }
                                    }
                                    var vertice = new Line3d(calca, (calcb - calca));
                                    double vl;
                                    segment.GetClosestPoints(vertice, out t, out vl);
                                    return (vertice.Origin + (vertice.Direction * vl));
                                }
                            }
                        }
                        return calca;
                    }

                case (2):
                    {
                        for (i = 0; i < 3; i++)
                        {
                            if (outside[i] == 1.0f)
                            {
                                calca[i] = box.Max[i];
                                calcb[i] = box.Max[i];
                            }
                            else if (outside[i] == -1.0f)
                            {
                                calca[i] = box.Min[i];
                                calcb[i] = box.Min[i];
                            }
                            else
                            {
                                calca[i] = box.Max[i];
                                calcb[i] = box.Min[i];
                            }
                        }
                        var vertice = new Line3d(calca, (calcb - calca));
                        double vl;
                        segment.GetClosestPoints(vertice, out t, out vl);
                        return (vertice.Origin + (vertice.Direction * vl));
                    }
                default:
                    {
                        for (i = 0; i < 3; i++)
                        {
                            if (outside[i] == 1.0f)
                            {
                                calca[i] = box.Max[i];
                            }
                            else if (outside[i] == -1.0f)
                            {
                                calca[i] = box.Min[i];
                            }
                        }
                        if ((V3d.DistanceSquared(calca, segment.P0)) > (V3d.DistanceSquared(calca, segment.P1)))
                        {
                            t = 1.0f;
                        }
                        return calca;
                    }
            }
        }

        public static V2d GetClosestPoints(
            this Ray2d line, Triangle2d triangle, out double t_line
            )
        {
            throw new NotImplementedException();
        }

        public static V3d GetClosestPoints(
            this Ray3d line, Triangle3d triangle, out double t_line
            )
        {
            V3d point_last = V3d.Zero;

            if (line.Intersect(triangle, out t_line, out point_last) == true)
            {
                return point_last;
            }

            var segment_AB = new Line3d(triangle.P0, triangle.P1);
            var segment_BC = new Line3d(triangle.P1, triangle.P2);
            var segment_CA = new Line3d(triangle.P2, triangle.P0);

            V3d point_new = V3d.Zero;

            double dist_last;
            double dist_new;
            double t_temp;
            double t_AB;
            double t_BC;
            double t_CA;

            line.GetClosestPoints(segment_AB, out t_line, out t_AB);
            point_last = segment_AB.P0 + t_AB * segment_AB.Direction;
            dist_last = V3d.DistanceSquared((line.Origin + t_line * line.Direction), point_last);

            line.GetClosestPoints(segment_BC, out t_temp, out t_BC);
            point_new = segment_BC.P0 + t_BC * segment_BC.Direction;
            dist_new = V3d.DistanceSquared((line.Origin + t_temp * line.Direction), point_new);

            if (dist_last > dist_new)
            {
                t_line = t_temp;
                dist_last = dist_new;
                point_last = point_new;
            }

            line.GetClosestPoints(segment_CA, out t_temp, out t_CA);
            point_new = segment_CA.P0 + t_CA * segment_CA.Direction;
            dist_new = V3d.DistanceSquared((line.Origin + t_temp * line.Direction), point_new);

            if (dist_last > dist_new)
            {
                t_line = t_temp;
                dist_last = dist_new;
                point_last = point_new;
            }

            return point_last;
        }

        public static V2d GetClosestPoints(
            this Line2d segment, Triangle2d triangle, out double t_segment
            )
        {
            throw new NotImplementedException();
        }

        public static V3d GetClosestPoints(
            this Line3d segment, Triangle3d triangle, out double t_segment
            )
        {
            V3d point_last = V3d.Zero;

            if (segment.Intersect(triangle, out t_segment, out point_last) == true)
            {
                return point_last;
            }

            var segment_AB = new Line3d(triangle.P0, triangle.P1);
            var segment_BC = new Line3d(triangle.P1, triangle.P2);
            var segment_CA = new Line3d(triangle.P2, triangle.P0);


            V3d point_new = V3d.Zero;

            double dist_last;
            double dist_new;
            double t_temp;
            double t_AB;
            double t_BC;
            double t_CA;

            segment.GetClosestPoints(segment_AB, out t_segment, out t_AB);
            point_last = segment_AB.P0 + t_AB * segment_AB.Direction;
            dist_last = V3d.DistanceSquared((segment.P0 + t_segment * segment.Direction), point_last);

            segment.GetClosestPoints(segment_BC, out t_temp, out t_BC);
            point_new = segment_BC.P0 + t_BC * segment_BC.Direction;
            dist_new = V3d.DistanceSquared((segment.P0 + t_temp * segment.Direction), point_new);

            if (dist_last > dist_new)
            {
                t_segment = t_temp;
                dist_last = dist_new;
                point_last = point_new;
            }

            segment.GetClosestPoints(segment_CA, out t_temp, out t_CA);
            point_new = segment_CA.P0 + t_CA * segment_CA.Direction;
            dist_new = V3d.DistanceSquared((segment.P0 + t_temp * segment.Direction), point_new);

            if (dist_last > dist_new)
            {
                t_segment = t_temp;
                dist_last = dist_new;
                point_last = point_new;
            }

            point_new = segment.P0.GetClosestPointOn(triangle);
            dist_new = V3d.DistanceSquared(point_new, segment.P0);

            if (dist_last > dist_new)
            {
                t_segment = 0.0f;
                dist_last = dist_new;
                point_last = point_new;
            }

            point_new = segment.P1.GetClosestPointOn(triangle);
            dist_new = V3d.DistanceSquared(point_new, segment.P1);

            if (dist_last > dist_new)
            {
                t_segment = 1.0f;
                dist_last = dist_new;
                point_last = point_new;
            }

            return point_last;
        }

        /// <summary>
        /// Calculates closest points between line and circle.
        /// t represents the point on the line(p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on circle.</returns>
        public static V2d GetClosestPoints(
            this Ray2d line, Circle2d circle, out double t_line
            )
        {
            V2d temp_point = circle.Center.GetClosestPointOn(line, out t_line);

            if (V2d.DistanceSquared(temp_point, circle.Center) <= circle.RadiusSquared)
            {
                double t2;
                line.Intersect(circle, out t_line, out t2);
                return line.Origin + t_line * line.Direction;
            }

            return circle.Center + circle.Radius * new V2d(temp_point - circle.Center).Normalized;

        }

        /// <summary>
        /// Calculates closest points between line segment and circle.
        /// t represents the point on the line(p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on circle.</returns>
        public static V2d GetClosestPoints(
            this Line2d segment, Circle2d circle, out double t_segment
            )
        {
            V2d temp_point = circle.Center.GetClosestPointOn(segment, out t_segment);

            if (V2d.DistanceSquared(temp_point, circle.Center) <= (circle.Radius * circle.Radius))
            {
                segment.Intersect(circle, out t_segment);
                return segment.P0 + t_segment * segment.Direction;
            }

            return circle.Center + circle.Radius * new V2d(temp_point - circle.Center).Normalized;

        }

        /// <summary>
        /// Calculates closest points between ray and sphere.
        /// t represents the point on the ray(p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on sphere.</returns>
        public static V3d GetClosestPoints(
            this Ray3d ray, Sphere3d sphere, out double t_ray
            )
        {
            V3d temp_point = sphere.Center.GetClosestPointOn(ray, out t_ray);

            if (V3d.DistanceSquared(temp_point, sphere.Center) <= (sphere.Radius * sphere.Radius))
            {
                ray.Intersect(sphere, out t_ray);
                return ray.Origin + t_ray * ray.Direction;
            }

            return sphere.Center + sphere.Radius * new V3d(temp_point - sphere.Center).Normalized;
        }

        /// <summary>
        /// Calculates closest points between line segment and sphere.
        /// t represents the point on the line segment(p = origin + t * direction).
        /// </summary>
        /// <returns>Closest Point on sphere.</returns>
        public static V3d GetClosestPoints(
            this Line3d segment, Sphere3d sphere, out double t_segment
            )
        {
            V3d temp_point = sphere.Center.GetClosestPointOn(segment, out t_segment);

            if (V3d.DistanceSquared(temp_point, sphere.Center) <= (sphere.Radius * sphere.Radius))
            {
                segment.Intersect(sphere, out t_segment);
                return segment.P0 + t_segment * segment.Direction;
            }

            return sphere.Center + sphere.Radius * new V3d(temp_point - sphere.Center).Normalized;
        }

        public static V2d GetClosestPointTo(
            this Box2d box, V2d queryPoint
            )
        {
            return queryPoint.GetClosestPointOn(box);
        }

        public static V3d GetClosestPointTo(
            this Box3d box, V3d queryPoint
            )
        {
            return queryPoint.GetClosestPointOn(box);
        }

        /// <summary>
        /// Returns point on AABB which is nearest to the triangle.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Box2d aabb, Triangle2d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on AABB which is nearest to the triangle.
        /// The AABB is assumed to lie in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Box2d aabb, Triangle3d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on AABB which is nearest to the triangle.
        /// The triangle is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Box3d aabb, Triangle2d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on AABB which is nearest to the triangle.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Box3d aabb, Triangle3d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on AABB which is nearest to circle.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Box2d aabb, Circle2d circle
            )
        {
            int i;
            bool corner = true;
            V2d outside = new V2d(0,0);
            for( i = 0; i < 2; i++)
            {
                if (circle.Center[i] > aabb.Max[i] + circle.Radius)
                {
                    outside[i] = aabb.Max[i];
                }
                else if (circle.Center[i] < aabb.Min[i] - circle.Radius)
                {
                    outside[i] = aabb.Min[i];
                }
                else
                {
                    corner = false;
                }
            }
            if (corner) { return outside; }
            else if (outside[0] == 0 && outside[1] == 0)
            {
                // TODO add intersection part here
                return outside;
            }
            else
            {
                for (i = 0; i < 2; i++)
                {
                    if (outside[i] == 0)
                    {
                        outside[i] = circle.Center[i];
                    }
                }
                return outside;
            }
        }

        /// <summary>
        /// Returns point on AABB which is nearest to circle.
        /// The circle is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Box3d aabb, Circle2d circle
            )
        {
            int i;
            bool corner = true;
            V3d outside = new V3d(0, 0, 0);
            for (i = 0; i < 2; i++)
            {
                if (circle.Center[i] > aabb.Max[i] + circle.Radius)
                {
                    outside[i] = aabb.Max[i];
                }
                else if (circle.Center[i] < aabb.Min[i] - circle.Radius)
                {
                    outside[i] = aabb.Min[i];
                }
                else
                {
                    corner = false;
                }
            }
            if (corner) { return outside; }
            else if (outside[0] == 0 && outside[1] == 0)
            {
                // TODO add intersection part here II
                return outside;
            }
            else
            {
                for (i = 0; i < 2; i++)
                {
                    if (outside[i] == 0)
                    {
                        outside[i] = circle.Center[i];
                    }
                }
                return outside;
            }
        }

        /// <summary>
        /// Returns point on AABB which is nearest to sphere.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Box3d aabb, Sphere3d sphere
            )
        {
            int i;
            bool corner = true;
            V3d outside = new V3d(0, 0, 0);
            for (i = 0; i < 3; i++)
            {
                if (sphere.Center[i] > aabb.Max[i] + sphere.Radius)
                {
                    outside[i] = aabb.Max[i];
                }
                else if (sphere.Center[i] < aabb.Min[i] - sphere.Radius)
                {
                    outside[i] = aabb.Min[i];
                }
                else
                {
                    corner = false;
                }
            }
            if (corner) { return outside; }
            else if (outside[0] == 0 && outside[1] == 0 && outside[2] == 0)
            {
                // TODO add intersection part here III
                return outside;
            }
            else
            {
                for (i = 0; i < 3; i++)
                {
                    if (outside[i] == 0)
                    {
                        outside[i] = sphere.Center[i];
                    }
                }
                return outside;
            }
        }

        /// <summary>
        /// Returns point on AABB which is nearest to sphere.
        /// The AABB is assumed to lie in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Box2d aabb, Sphere3d sphere
            )
        {
            int i;
            bool corner = true;
            V2d outside = new V2d(0, 0);
            for (i = 0; i < 2; i++)
            {
                if (sphere.Center[i] > aabb.Max[i] + sphere.Radius)
                {
                    outside[i] = aabb.Max[i];
                }
                else if (sphere.Center[i] < aabb.Min[i] - sphere.Radius)
                {
                    outside[i] = aabb.Min[i];
                }
                else
                {
                    corner = false;
                }
            }
            if (corner) { return outside; }
            else if (outside[0] == 0 && outside[1] == 0 && outside[2] == 0)
            {
                // TODO add intersection part here IV
                return outside;
            }
            else
            {
                for (i = 0; i < 3; i++)
                {
                    if (outside[i] == 0)
                    {
                        outside[i] = sphere.Center[i];
                    }
                }
                return outside;
            }
        }

        /// <summary>
        /// Returns point on AABB which is nearest to AABB.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Box2d aabb, Box2d aabb2
            )
        {
            V2d ret = new V2d(V2d.Zero);
            int i;
            for (i = 0; i < 2; i++)
            {
                if (aabb.Max[i] < aabb2.Min[i])
                {
                    ret[i] = aabb.Max[i];
                }
                else
                {
                    ret[i] = aabb.Min[i];
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns point on AABB which is nearest to AABB.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Box3d aabb, Box3d aabb2
            )
        {
            V3d ret = new V3d(V3d.Zero);
            int i;
            for (i = 0; i < 3; i++)
            {
                if (aabb.Max[i] < aabb2.Min[i])
                {
                    ret[i] = aabb.Max[i];
                }
                else
                {
                    ret[i] = aabb.Min[i];
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns point on triangle which is nearest to the AABB.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Triangle2d triangle, Box2d aabb
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on triangle which is nearest to the AABB.
        /// The triangle is assumed to lie in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Triangle2d triangle, Box3d aabb
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on triangle which is nearest to the AABB.
        /// The AABB is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Triangle3d triangle, Box2d aabb
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on triangle which is nearest to the AABB.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Triangle3d triangle, Box3d aabb
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on first triangle which is nearest to the second triangle.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Triangle2d triangle1, Triangle2d triangle2
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on first triangle which is nearest to the second triangle.
        /// The first triangle is assumed to lie in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Triangle2d triangle1, Triangle3d triangle2
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on first triangle which is nearest to the second triangle.
        /// The second triangle is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Triangle3d triangle1, Triangle2d triangle2
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on first triangle which is nearest to the second triangle.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Triangle3d triangle1, Triangle3d triangle2
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on triangle which is nearest to circle.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Triangle2d triangle, Circle2d circle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on triangle which is nearest to circle.
        /// The circle is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Triangle3d triangle, Circle2d circle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on triangle which is nearest to sphere.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Triangle3d triangle, Sphere3d sphere
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on triangle which is nearest to sphere.
        /// The triangle is assumed to lie in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Triangle2d triangle, Sphere3d sphere
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on circle which is nearest to AABB.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Circle2d circle, Box2d aabb
            )
        {
            V2d p_aabb = aabb.GetClosestPointTo(circle);
            p_aabb = p_aabb - circle.Center;
            p_aabb.Normalize();
            p_aabb = p_aabb * circle.Radius;
            return p_aabb;
        }

        /// <summary>
        /// Returns point on circle which is nearest to AABB.
        /// The circle is assumed to lie in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Circle2d circle, Box3d aabb
            )
        {
            V2d min = new V2d(aabb.Min.X, aabb.Min.Y);
            V2d max = new V2d(aabb.Max.X, aabb.Max.Y);
            Box2d box = new Box2d(min, max);
            V2d p_aabb = box.GetClosestPointTo(circle);
            p_aabb = p_aabb - circle.Center;
            p_aabb.Normalize();
            p_aabb = p_aabb * circle.Radius;
            return p_aabb;
        }

        /// <summary>
        /// Returns point on circle which is nearest to triangle.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Circle2d circle, Triangle2d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on circle which is nearest to triangle.
        /// The circle is assumed to lie in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Circle2d circle, Triangle3d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on first circle which is nearest to second circle.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Circle2d circle1, Circle2d circle2
            )
        {
            V2d circ_circ = new V2d((circle2.Center - circle1.Center));
            circ_circ.Normalize();
            circ_circ = circ_circ * circle1.Radius;
            return circle1.Center + circ_circ;
        }

        /// <summary>
        /// Returns point on circle which is nearest to sphere.
        /// It is assumed that the circle lies in the xy-plane.
        /// </summary>
        public static V2d GetClosestPointTo(
            this Circle2d circle, Sphere3d sphere
            )
        {
            V2d sphe_circ = new V2d((sphere.Center.X - circle.Center.X), (sphere.Center.Y - circle.Center.Y));
            sphe_circ.Normalize();
            sphe_circ = sphe_circ * circle.Radius;
            return sphe_circ;
        }

        /// <summary>
        /// Returns point on sphere which is nearest to AABB.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Sphere3d sphere, Box3d aabb
            )
        {
            V3d p_aabb = aabb.GetClosestPointTo(sphere);
            p_aabb = p_aabb - sphere.Center;
            p_aabb.Normalize();
            p_aabb = p_aabb * sphere.Radius;
            return p_aabb;
        }

        /// <summary>
        /// Returns point on sphere which is nearest to AABB.
        /// The AABB is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Sphere3d sphere, Box2d aabb
            )
        {
            V2d cc = new V2d(sphere.Center.X, sphere.Center.Y);
            Circle2d ci2 = new Circle2d(cc, sphere.Radius);
            cc = aabb.GetClosestPointTo(ci2);
            cc = cc - ci2.Center;
            cc.Normalize();
            cc = cc * sphere.Radius;
            return new V3d(cc.X, cc.Y, sphere.Center.Z);
        }

        /// <summary>
        /// Returns point on sphere which is nearest to triangle.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Sphere3d sphere, Triangle3d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on sphere which is nearest to triangle.
        /// The triangle is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Sphere3d sphere, Triangle2d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on sphere which is nearest to circle.
        /// It is assumed that the circle lies in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Sphere3d sphere, Circle2d circle
            )
        {
            V3d sphe_circ = new V3d((circle.Center.X - sphere.Center.X), (circle.Center.Y - sphere.Center.Y), 0);
            sphe_circ.Normalize();
            sphe_circ = sphe_circ * sphere.Radius;
            return sphe_circ;
        }

        /// <summary>
        /// Returns point on first sphere which is nearest to second sphere.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Sphere3d sphere1, Sphere3d sphere2
            )
        {
            V3d sphe_sphe = new V3d((sphere2.Center - sphere1.Center));
            sphe_sphe.Normalize();
            sphe_sphe = sphe_sphe * sphere1.Radius;
            return sphe_sphe;
        }

        /// <summary>
        /// Returns point on plane which is nearest to point.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, V2d point
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to point.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, V3d point
            )
        {
            return point.GetClosestPointOn(plane);
        }

        /// <summary>
        /// Returns point on plane which is nearest to line.
        /// If parallel, the lines origin is returned.
        /// The line is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Ray2d line
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to line.
        /// If parallel, the lines origin is returned.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Ray3d line
            )
        {
            double t;
            bool result = IntersectionTests.Intersect(line, plane, out t); 
            
            if(result == false)
            {
                return line.Origin;
            }

            return line.Origin + t * line.Direction;
        }

        /// <summary>
        /// Returns point on plane which is nearest to segment.
        /// If parallel, the segments origin is returned.
        /// The segment is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Line2d segment
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to segment.
        /// If parallel, the segments origin is returned.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Line3d segment
            )
        {
            double t;
            bool result = segment.Intersect(plane, out t);

            if (result == false)
            {
                return segment.Origin;
            }

            return segment.Origin + t * segment.Direction;
        }

        /// <summary>
        /// Returns point on plane which is nearest to AABB.
        /// If parallel, one of the corners is returned.
        /// The AABB is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Box2d aabb
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to AABB.
        /// If parallel, one of the corners is returned.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Box3d aabb
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to triangle.
        /// If parallel, one of the corners is returned.
        /// The triangle is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Triangle2d triangle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to triangle.
        /// If parallel, one of the corners is returned.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Triangle3d triangle
            )
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns point on plane which is nearest to circle.
        /// If parallel, the center is returned.
        /// The circle is assumed to lie in the xy-plane.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Circle2d circle
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to sphere.
        /// If intersecting, any point from the intersection is returned.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, Sphere3d sphere
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns point on plane which is nearest to frustum.
        /// If parallel, a corner is returned.
        /// If intersecting, any point from the intersection is returned.
        /// </summary>
        public static V3d GetClosestPointTo(
            this Plane3d plane, IFrustum3d frustum
            )
        {
            throw new NotImplementedException();
        }

#endif
}

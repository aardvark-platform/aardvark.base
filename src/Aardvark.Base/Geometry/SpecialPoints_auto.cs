using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides various methods for middle point computations.
    /// </summary>
    public static partial class GeometryFun
    {
        /* Ray3f */

        #region Ray3f - Ray3f

        public static V3f GetMiddlePoint(this Ray3f ray0, Ray3f ray1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            var my = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1);
            var la = (my * uDotv - a.Dot(u));

            var p0 = ray0.Origin + la * u;
            var p1 = ray1.Origin + my * v;

            return 0.5f*(p0 + p1);
        }

        #endregion

        #region IEnumerable<Ray3f>

        public static V3f GetMiddlePoint(this IEnumerable<Ray3f> rays)
        {
            var center = V3f.Zero;
            var count = 0;

            foreach(var r0 in rays)
                foreach (var r1 in rays)
                    if (r0.LexicalCompare(r1) < 0)
                    {
                        center += r0.GetMiddlePoint(r1);
                        ++count;
                    }

            return center / (float)count;
        }

        #endregion

        /* Ray3d */

        #region Ray3d - Ray3d

        public static V3d GetMiddlePoint(this Ray3d ray0, Ray3d ray1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            var my = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1);
            var la = (my * uDotv - a.Dot(u));

            var p0 = ray0.Origin + la * u;
            var p1 = ray1.Origin + my * v;

            return 0.5*(p0 + p1);
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
        /* V2f */

        #region V2f - Line2f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this V2f query, Line2f line)
            => GetClosestPointOn(query, line, out float t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Line2f line, V2f query)
            => query.GetClosestPointOn(line);

        public static V2f GetClosestPointOn(this V2f query, Line2f line, out float t)
        {
            var p0q = query - line.P0;
            t = Vec.Dot(p0q, line.Direction);
            if (t <= 0) { t = 0; return line.P0; }
            var denom = line.Direction.LengthSquared;
            if (t >= denom) { t = 1; return line.P1; }
            t /= denom;
            return line.P0 + t * line.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Line2f line, V2f query, out float t)
            => query.GetClosestPointOn(line, out t);

        #endregion

        #region V2f - Ray2f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this V2f query, Ray2f ray)
            => GetClosestPointOn(query, ray, out float t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Ray2f ray, V2f query)
            => query.GetClosestPointOn(ray);

        /// <summary>
        /// Returns the t-parameter along <paramref name="ray"/> at which the closest point to <paramref name="query"/> is.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetClosestPointTOn(this V2f query, Ray2f ray)
            => Vec.Dot(query - ray.Origin, ray.Direction) / ray.Direction.LengthSquared;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this V2f query, Ray2f ray, out float t)
        {
            t = GetClosestPointTOn(query, ray);
            return ray.Origin + t * ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Ray2f ray, V2f query, out float t)
            => query.GetClosestPointOn(ray, out t);

        /// <summary>
        /// Returns the t-parameter along <paramref name="ray"/> at which the closest point to <paramref name="query"/> is.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetClosestPointTOn(this Ray2f ray, V2f query)
            => query.GetClosestPointTOn(ray);

        #endregion

        #region V2f - Plane2f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this V2f point, Plane2f line)
        {
            var lengthOfNormal2 = line.Normal.LengthSquared;
            return (point - (line.Normal.Dot(point - line.Point) / lengthOfNormal2) * line.Normal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Plane2f line, V2f point)
            => point.GetClosestPointOn(line);

        #endregion

        #region V2f - Box2f

        public static V2f GetClosestPointOn(this V2f query, Box2f box)
        {
            var closest = query;
            for (var i = 0; i < 2; i++)
            {
                if (query[i] < box.Min[i]) closest[i] = box.Min[i];
                if (query[i] > box.Max[i]) closest[i] = box.Max[i];
            }
            return closest;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Box2f box, V2f query)
            => query.GetClosestPointOn(box);

        #endregion

        #region V2f - Quad2f

        public static V2f GetClosestPointOn(this V2f vec, Quad2f quad)
        {
            V2f closestPoint;
            var temp = vec.GetClosestPointOn(new Line2f(quad.P0,quad.P1));
            closestPoint = temp;

            temp = vec.GetClosestPointOn(new Line2f(quad.P1, quad.P2));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            temp = vec.GetClosestPointOn(new Line2f(quad.P2, quad.P3));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            temp = vec.GetClosestPointOn(new Line2f(quad.P3, quad.P0));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            return closestPoint;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Quad2f quad, V2f vec)
            => vec.GetClosestPointOn(quad);

        #endregion

        #region V2f - Polygon2f

        public static V2f GetClosestPointOn(this V2f vec, Polygon2f poly)
        {
            var closestPoint = V2f.PositiveInfinity;
            var i = 0;
            foreach (var line in poly.EdgeLines)
            {
                var temp = vec.GetClosestPointOn(line);
                if (i++ == 0) closestPoint = temp;
                else if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;
            }

            return closestPoint;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Polygon2f poly, V2f vec)
            => vec.GetClosestPointOn(poly);

        #endregion

        #region V2f - Circle2f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this V2f query, Circle2f circle)
            => circle.Center + circle.Radius * (query - circle.Center).Normalized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Circle2f circle, V2f query)
            => query.GetClosestPointOn(circle);

        #endregion

        #region V2f - Triangle2f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this V2f query, Triangle2f triangle)
            => query.GetClosestPointOnTriangle(triangle.P0, triangle.P1, triangle.P2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetClosestPointOn(this Triangle2f triangle, V2f query)
            => query.GetClosestPointOn(triangle);

        public static V2f GetClosestPointOnTriangle(this V2f query, V2f a, V2f b, V2f c)
        {
            var e01 = b - a;
            var e02 = c - a;

            var p0q = query - a;
            var d1 = Vec.Dot(e01, p0q);
            var d2 = Vec.Dot(e02, p0q);
            if (d1 <= 0 && d2 <= 0) return a; // bary (1,0,0)

            var p1q = query - b;
            var d3 = Vec.Dot(e01, p1q);
            var d4 = Vec.Dot(e02, p1q);
            if (d3 >= 0 && d4 <= d3) return b; // bary (0,1,0)

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0 && d1 >= 0 && d3 <= 0)
            {
                var t = d1 / (d1 - d3);
                return a + t * e01;   // bary (1-t,t,0)
            }

            var p2q = query - c;
            var d5 = Vec.Dot(e01, p2q);
            var d6 = Vec.Dot(e02, p2q);
            if (d6 >= 0 && d5 <= d6) return c; // bary (0,0,1)

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0 && d2 >= 0 && d6 <= 0)
            {
                var t = d2 / (d2 - d6);
                return a + t * e02; // bary (1-t,0,t)
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0 && (d4 - d3) >= 0 && (d5 - d6) >= 0)
            {
                var t = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return b + t * (c - b); // bary (0, 1-t, t)
            }

            // var denom = 1.0 / (va + vb + vc);
            // var v = vb * denom;
            // var w = vc * denom;
            // return triangle.P0 + v * e01 + w * e02; // bary (1-v-w, v, w)
            return query;
        }

        #endregion

        /* V3f */

        #region V3f - Box3f

        public static float GetDistanceSquared(this V3f q, Box3f b)
            => (q.X < b.Min.X ? Fun.Square(b.Min.X - q.X) : q.X > b.Max.X ? Fun.Square(q.X - b.Max.X) : 0)
             + (q.Y < b.Min.Y ? Fun.Square(b.Min.Y - q.Y) : q.Y > b.Max.Y ? Fun.Square(q.Y - b.Max.Y) : 0)
             + (q.Z < b.Min.Z ? Fun.Square(b.Min.Z - q.Z) : q.Z > b.Max.Z ? Fun.Square(q.Z - b.Max.Z) : 0);

        public static (float, float) GetDistanceSquared(
            this V3f q, Box3fAndFlags boxAndFlags, Box.Flags d2Flags, V3f d2v,
            out Box.Flags d2Flags0, out V3f d2v0,
            out Box.Flags d2Flags1, out V3f d2v1)
        {
            d2Flags0 = d2Flags; d2Flags1 = d2Flags;
            d2v0 = d2v; d2v1 = d2v;

            float d0 = 0, d1 = 0;

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

        public static V3f GetClosestPointOn(this V3f query, Box3f box)
        {
            var closest = query;
            for (int i = 0; i < 3; i++)
            {
                if (query[i] < box.Min[i]) closest[i] = box.Min[i];
                if (query[i] > box.Max[i]) closest[i] = box.Max[i];
            }
            return closest;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Box3f box, V3f query)
            => query.GetClosestPointOn(box);

        #endregion

        #region V3f - Line3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this V3f query, Line3f line)
            => query.GetClosestPointOnLine(line.P0, line.P1, out float t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Line3f line, V3f query)
            => query.GetClosestPointOn(line);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this V3f query, Line3f line, out float t)
            => query.GetClosestPointOnLine(line.P0, line.P1, out t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Line3f line, V3f query, out float t)
            => query.GetClosestPointOn(line, out t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOnLine(this V3f query, V3f p0, V3f p1)
            => query.GetClosestPointOnLine(p0, p1, out float t);

        public static V3f GetClosestPointOnLine(this V3f query, V3f p0, V3f p1, out float t)
        {
            var dir = p1 - p0;
            var p0q = query - p0;
            t = Vec.Dot(p0q, dir);
            if (t <= 0) { t = 0; return p0; }
            var denom = dir.LengthSquared;
            if (t >= denom) { t = 1; return p1; }
            t /= denom;
            return p0 + t * dir;
        }

        #endregion

        #region V3f - Plane3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this V3f query, Plane3f plane)
            => query - plane.Height(query) * plane.Normal;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Plane3f plane, V3f query)
            => query.GetClosestPointOn(plane);

        #endregion

        #region V3f - Ray3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this V3f query, Ray3f ray)
            => GetClosestPointOn(query, ray, out float t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Ray3f ray, V3f query)
            => query.GetClosestPointOn(ray);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this V3f query, Ray3f ray, out float t)
        {
            t = Vec.Dot(query - ray.Origin, ray.Direction)
                        / ray.Direction.LengthSquared;
            return ray.Origin + t * ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Ray3f ray, V3f query, out float t)
            => query.GetClosestPointOn(ray, out t);

        #endregion

        #region V3f - Sphere3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this V3f query, Sphere3f sphere)
            => sphere.Center + sphere.Radius * (query - sphere.Center).Normalized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Sphere3f sphere, V3f query)
            => query.GetClosestPointOn(sphere);

        #endregion

        #region V3f - Cylinder3f

        /// <summary>
        /// get closest point on cylinder with infinite length
        /// </summary>
        public static V3f GetClosestPointOn(this V3f query, Cylinder3f cylinder)
        {
            var p = query.GetClosestPointOn(cylinder.Axis.Ray3f);
            var dir = (query - p).Normalized;
            return p + cylinder.Radius * dir;
        }

        /// <summary>
        /// get closest point on cylinder with infinite length
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Cylinder3f cylinder, V3f query)
            => query.GetClosestPointOn(cylinder);

        #endregion

        #region V3f - Triangle3f

        /// <summary>
        /// Tested by rft on 2008-08-06.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this V3f query, Triangle3f triangle)
            => GetClosestPointOnTriangle(query, triangle.P0, triangle.P1, triangle.P2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetClosestPointOn(this Triangle3f triangle, V3f query)
            => query.GetClosestPointOn(triangle);

        /// <summary>
        /// Tested by rft on 2008-08-06.
        /// </summary>
        public static V3f GetClosestPointOnTriangle(this V3f query, V3f p0, V3f p1, V3f p2)
        {
            var e01 = p1 - p0;
            var e02 = p2 - p0;

            var p0q = query - p0;
            var d1 = Vec.Dot(e01, p0q);
            var d2 = Vec.Dot(e02, p0q);
            if (d1 <= 0 && d2 <= 0) return p0; // bary (1,0,0)

            var p1q = query - p1;
            var d3 = Vec.Dot(e01, p1q);
            var d4 = Vec.Dot(e02, p1q);
            if (d3 >= 0 && d4 <= d3) return p1; // bary (0,1,0)

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0 && d1 >= 0 && d3 <= 0)
            {
                var t = d1 / (d1 - d3);
                return p0 + t * e01;   // bary (1-t,t,0)
            }

            var p2q = query - p2;
            var d5 = Vec.Dot(e01, p2q);
            var d6 = Vec.Dot(e02, p2q);
            if (d6 >= 0 && d5 <= d6) return p2; // bary (0,0,1)

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0 && d2 >= 0 && d6 <= 0)
            {
                var t = d2 / (d2 - d6);
                return p0 + t * e02; // bary (1-t,0,t)
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0 && (d4 - d3) >= 0 && (d5 - d6) >= 0)
            {
                var t = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return p1 + t * (p2 - p1); // bary (0, 1-t, t)
            }

            float denom = 1 / (va + vb + vc);
            var v = vb * denom;
            var w = vc * denom;
            return p0 + v * e01 + w * e02; // bary (1-v-w, v, w)
        }

        #endregion

        /* Ray3f */

        #region Ray3f - Ray3f

        public static V3f GetClosestPointOn(this Ray3f ray0, Ray3f ray1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            var n = (uDotv * uDotv - 1);
            // make sure rays are not parallel
            if (!n.Abs().IsTiny())
            {
                var my = (a.Dot(u) * uDotv - a.Dot(v)) / n;
                return ray1.Origin + my * v;
            }

            return ray1.Origin;
        }

        #endregion

        /* Line3f */

        #region Line3f - Line3f

        /// <summary>
        /// Returns the point on line1 which is closest to line0.
        /// </summary>
        public static V3f GetClosestPointOn(this Line3f line0, Line3f line1)
        {
            var r0 = line0.Ray3f;
            var r1 = line1.Ray3f;

            if (r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                if (line0.P0.GetMinimalDistanceTo(line1) < line0.P1.GetMinimalDistanceTo(line1))
                    return line0.P0.GetClosestPointOn(line1);
                else
                    return line0.P1.GetClosestPointOn(line1);
            }

            /* var distance = */
            r0.GetMinimalDistanceTo(r1, out float t0, out float t1);
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

        #region Line3f - Plane3f

        /// <summary>
        /// Returns the closest point from a line to a plane (point on line).
        /// </summary>
        public static V3f GetClosestPointOn(this Plane3f plane, Line3f line)
        {
            var ray = line.Ray3f;
            if (ray.Intersects(plane, out float t))
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

        #region Line3f - Triangle3f

        /// <summary>
        /// Returns the closest point from a triangle to a line (point on triangle).
        /// Note: untested
        /// </summary>
        public static V3f GetClosestPointOn(this Line3f line, Triangle3f triangle)
        {
            // test if closest point on triangle plane is inside the triangle (we have the point),
            // otherwiese find closest edge and take closest point to that edge

            var plane = triangle.Plane;
            var planePoint = plane.GetClosestPointOn(line);

            // switch to 2d:
            var trafo = Trafo3f.FromNormalFrame(plane.Point, plane.Normal);
            var point2d = trafo.Backward.TransformPos(planePoint).XY;
            var triangle2d = new Triangle2f(trafo.Backward.TransformPos(triangle.P0).XY,
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

        /* V2d */

        #region V2d - Line2d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this V2d query, Line2d line)
            => GetClosestPointOn(query, line, out double t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this Line2d line, V2d query)
            => query.GetClosestPointOn(line);

        public static V2d GetClosestPointOn(this V2d query, Line2d line, out double t)
        {
            var p0q = query - line.P0;
            t = Vec.Dot(p0q, line.Direction);
            if (t <= 0) { t = 0; return line.P0; }
            var denom = line.Direction.LengthSquared;
            if (t >= denom) { t = 1; return line.P1; }
            t /= denom;
            return line.P0 + t * line.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this Line2d line, V2d query, out double t)
            => query.GetClosestPointOn(line, out t);

        #endregion

        #region V2d - Ray2d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this V2d query, Ray2d ray)
            => GetClosestPointOn(query, ray, out double t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this Ray2d ray, V2d query)
            => query.GetClosestPointOn(ray);

        /// <summary>
        /// Returns the t-parameter along <paramref name="ray"/> at which the closest point to <paramref name="query"/> is.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetClosestPointTOn(this V2d query, Ray2d ray)
            => Vec.Dot(query - ray.Origin, ray.Direction) / ray.Direction.LengthSquared;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this V2d query, Ray2d ray, out double t)
        {
            t = GetClosestPointTOn(query, ray);
            return ray.Origin + t * ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this Ray2d ray, V2d query, out double t)
            => query.GetClosestPointOn(ray, out t);

        /// <summary>
        /// Returns the t-parameter along <paramref name="ray"/> at which the closest point to <paramref name="query"/> is.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetClosestPointTOn(this Ray2d ray, V2d query)
            => query.GetClosestPointTOn(ray);

        #endregion

        #region V2d - Plane2d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this V2d point, Plane2d line)
        {
            var lengthOfNormal2 = line.Normal.LengthSquared;
            return (point - (line.Normal.Dot(point - line.Point) / lengthOfNormal2) * line.Normal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this Polygon2d poly, V2d vec)
            => vec.GetClosestPointOn(poly);

        #endregion

        #region V2d - Circle2d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this V2d query, Circle2d circle)
            => circle.Center + circle.Radius * (query - circle.Center).Normalized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this Circle2d circle, V2d query)
            => query.GetClosestPointOn(circle);

        #endregion

        #region V2d - Triangle2d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this V2d query, Triangle2d triangle)
            => query.GetClosestPointOnTriangle(triangle.P0, triangle.P1, triangle.P2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetClosestPointOn(this Triangle2d triangle, V2d query)
            => query.GetClosestPointOn(triangle);

        public static V2d GetClosestPointOnTriangle(this V2d query, V2d a, V2d b, V2d c)
        {
            var e01 = b - a;
            var e02 = c - a;

            var p0q = query - a;
            var d1 = Vec.Dot(e01, p0q);
            var d2 = Vec.Dot(e02, p0q);
            if (d1 <= 0 && d2 <= 0) return a; // bary (1,0,0)

            var p1q = query - b;
            var d3 = Vec.Dot(e01, p1q);
            var d4 = Vec.Dot(e02, p1q);
            if (d3 >= 0 && d4 <= d3) return b; // bary (0,1,0)

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0 && d1 >= 0 && d3 <= 0)
            {
                var t = d1 / (d1 - d3);
                return a + t * e01;   // bary (1-t,t,0)
            }

            var p2q = query - c;
            var d5 = Vec.Dot(e01, p2q);
            var d6 = Vec.Dot(e02, p2q);
            if (d6 >= 0 && d5 <= d6) return c; // bary (0,0,1)

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0 && d2 >= 0 && d6 <= 0)
            {
                var t = d2 / (d2 - d6);
                return a + t * e02; // bary (1-t,0,t)
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0 && (d4 - d3) >= 0 && (d5 - d6) >= 0)
            {
                var t = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return b + t * (c - b); // bary (0, 1-t, t)
            }

            // var denom = 1.0 / (va + vb + vc);
            // var v = vb * denom;
            // var w = vc * denom;
            // return triangle.P0 + v * e01 + w * e02; // bary (1-v-w, v, w)
            return query;
        }

        #endregion

        /* V3d */

        #region V3d - Box3d

        public static double GetDistanceSquared(this V3d q, Box3d b)
            => (q.X < b.Min.X ? Fun.Square(b.Min.X - q.X) : q.X > b.Max.X ? Fun.Square(q.X - b.Max.X) : 0)
             + (q.Y < b.Min.Y ? Fun.Square(b.Min.Y - q.Y) : q.Y > b.Max.Y ? Fun.Square(q.Y - b.Max.Y) : 0)
             + (q.Z < b.Min.Z ? Fun.Square(b.Min.Z - q.Z) : q.Z > b.Max.Z ? Fun.Square(q.Z - b.Max.Z) : 0);

        public static (double, double) GetDistanceSquared(
            this V3d q, Box3dAndFlags boxAndFlags, Box.Flags d2Flags, V3d d2v,
            out Box.Flags d2Flags0, out V3d d2v0,
            out Box.Flags d2Flags1, out V3d d2v1)
        {
            d2Flags0 = d2Flags; d2Flags1 = d2Flags;
            d2v0 = d2v; d2v1 = d2v;

            double d0 = 0, d1 = 0;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this Box3d box, V3d query)
            => query.GetClosestPointOn(box);

        #endregion

        #region V3d - Line3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this V3d query, Line3d line)
            => query.GetClosestPointOnLine(line.P0, line.P1, out double t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this Line3d line, V3d query)
            => query.GetClosestPointOn(line);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this V3d query, Line3d line, out double t)
            => query.GetClosestPointOnLine(line.P0, line.P1, out t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this Line3d line, V3d query, out double t)
            => query.GetClosestPointOn(line, out t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOnLine(this V3d query, V3d p0, V3d p1)
            => query.GetClosestPointOnLine(p0, p1, out double t);

        public static V3d GetClosestPointOnLine(this V3d query, V3d p0, V3d p1, out double t)
        {
            var dir = p1 - p0;
            var p0q = query - p0;
            t = Vec.Dot(p0q, dir);
            if (t <= 0) { t = 0; return p0; }
            var denom = dir.LengthSquared;
            if (t >= denom) { t = 1; return p1; }
            t /= denom;
            return p0 + t * dir;
        }

        #endregion

        #region V3d - Plane3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this V3d query, Plane3d plane)
            => query - plane.Height(query) * plane.Normal;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this Plane3d plane, V3d query)
            => query.GetClosestPointOn(plane);

        #endregion

        #region V3d - Ray3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this V3d query, Ray3d ray)
            => GetClosestPointOn(query, ray, out double t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this Ray3d ray, V3d query)
            => query.GetClosestPointOn(ray);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this V3d query, Ray3d ray, out double t)
        {
            t = Vec.Dot(query - ray.Origin, ray.Direction)
                        / ray.Direction.LengthSquared;
            return ray.Origin + t * ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this Ray3d ray, V3d query, out double t)
            => query.GetClosestPointOn(ray, out t);

        #endregion

        #region V3d - Sphere3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this V3d query, Sphere3d sphere)
            => sphere.Center + sphere.Radius * (query - sphere.Center).Normalized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this Cylinder3d cylinder, V3d query)
            => query.GetClosestPointOn(cylinder);

        #endregion

        #region V3d - Triangle3d

        /// <summary>
        /// Tested by rft on 2008-08-06.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetClosestPointOn(this V3d query, Triangle3d triangle)
            => GetClosestPointOnTriangle(query, triangle.P0, triangle.P1, triangle.P2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            var d1 = Vec.Dot(e01, p0q);
            var d2 = Vec.Dot(e02, p0q);
            if (d1 <= 0 && d2 <= 0) return p0; // bary (1,0,0)

            var p1q = query - p1;
            var d3 = Vec.Dot(e01, p1q);
            var d4 = Vec.Dot(e02, p1q);
            if (d3 >= 0 && d4 <= d3) return p1; // bary (0,1,0)

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0 && d1 >= 0 && d3 <= 0)
            {
                var t = d1 / (d1 - d3);
                return p0 + t * e01;   // bary (1-t,t,0)
            }

            var p2q = query - p2;
            var d5 = Vec.Dot(e01, p2q);
            var d6 = Vec.Dot(e02, p2q);
            if (d6 >= 0 && d5 <= d6) return p2; // bary (0,0,1)

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0 && d2 >= 0 && d6 <= 0)
            {
                var t = d2 / (d2 - d6);
                return p0 + t * e02; // bary (1-t,0,t)
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0 && (d4 - d3) >= 0 && (d5 - d6) >= 0)
            {
                var t = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return p1 + t * (p2 - p1); // bary (0, 1-t, t)
            }

            double denom = 1 / (va + vb + vc);
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
            var n = (uDotv * uDotv - 1);
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
        #region Range1f - Range1f

        public static float GetMinimalDistanceTo(this Range1f range0, Range1f range1)
        {
            if (range0.Min > range1.Max) return range0.Min - range1.Max;
            if (range0.Max < range1.Min) return range1.Min - range0.Max;
            return 0;
        }

        #endregion

        #region Ray2f - Ray2f

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Ray2f ray0, Ray2f ray1)
            => ray0.GetMinimalDistanceTo(ray1, out float t0, out float t1);

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// t0 and t1 hold the correspunding ray parameters.
        /// if both rays are parallel the t0 and t1 are from the origin of ray1
        /// </summary>
        public static float GetMinimalDistanceTo(this Ray2f ray0, Ray2f ray1, out float t0, out float t1)
        {
            // NOTE: copy of Ray3f to Ray3f distance

            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            if (uDotv.Abs().ApproximateEquals(1, Constant<float>.PositiveTinyValue)) // rays are parallel
            {
                // return origin of ray 1
                t1 = 0;
                t0 = ray0.GetT(ray1.Origin);
            }
            else
            {
                // change by lui: added normalization (ortherwise in case the directions are not normalized t0 and t1 are wrong)
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 = t1 / ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        #endregion

        #region Box2f - Box2f

        public static float GetMinimalDistanceTo(this Box2f box0, Box2f box1)
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

        #region V2f - Line2f

        /// <summary>
        /// returns the minimal distance between the point and the Line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this V2f point, Line2f line)
            => point.DistanceToLine(line.P0, line.P1);

        #endregion

        #region V2f - Triangle2f

        /// <summary>
        /// returns the minimal distance between the point and the triangle
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this V2f point, Triangle2f t)
            => Triangle.Distance(t.P0, t.P1, t.P2, point);

        #endregion

        #region Line2f - Line2f
        /// <summary>
        /// returns the minimal distance between the given lines.
        /// </summary>
        public static float GetMinimalDistanceTo(this Line2f l0, Line2f l1)
        {
            var r0 = l0.Ray2f;
            var r1 = l1.Ray2f;

            if (!r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                var distance = r0.GetMinimalDistanceTo(r1, out float t0, out float t1);

                if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
                    return distance;
            }

            // lines are parallel or t0 and t1 are outside of the line
            return Fun.Min(l0.P0.GetMinimalDistanceTo(l1), l0.P1.GetMinimalDistanceTo(l1),
                l1.P0.GetMinimalDistanceTo(l0), l1.P1.GetMinimalDistanceTo(l0));
        }
        #endregion

        #region V3f - Line3f

        /// <summary>
        /// returns the minimal distance between the point and the Line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this V3f point, Line3f line)
            => point.DistanceToLine(line.P0, line.P1);

        /// <summary>
        /// Returns the minimal distance between the point and the Line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Line3f line, V3f point)
            => point.GetMinimalDistanceTo(line);

        #endregion

        #region V3f - Ray3f

        /*  Performance Test .........................................................................
                Faster Method:                                      V3f.MinimalDistanceTo(Ray3f)
                V3f.MinimalDistanceTo(Ray3f):                       1,841s
                (V3f - V3f.GetClosestPointOn(Ray3f)).Length:        3,058s
                Total Executions:                                   10000000
                Errors outside tolerance(1E-7):                     0
                Average Squared-Distance of Results:                2,24116481902135E-32
                Speedup-factor:                                     1,66
         */

        /// <summary>
        /// Returns the minimal distance between the point and the ray.
        /// </summary>
        public static float GetMinimalDistanceTo(this V3f point, Ray3f ray)
        {
            var a = point - ray.Origin;
            float lu2 = ray.Direction.LengthSquared;
            float acu2 = Vec.Cross(a, ray.Direction).LengthSquared;

            return Fun.Sqrt(acu2 / lu2);
        }

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Ray3f ray, V3f point)
            => point.GetMinimalDistanceTo(ray);

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// t holds the ray parameter for the closest point.
        /// </summary>
        public static float GetMinimalDistanceTo(this V3f point, Ray3f ray, out float t)
        {
            var a = point - ray.Origin;
            var lu2 = ray.Direction.LengthSquared;
            var acu2 = Vec.Cross(a, ray.Direction).LengthSquared;

            var NormalPart2 = acu2 / lu2;
            var ParallelPart2 = lu2 - NormalPart2;

            t = Fun.Sqrt(ParallelPart2 / lu2);
            return Fun.Sqrt(NormalPart2);
        }

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// t holds the ray parameter for the closest point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Ray3f ray, V3f point, out float t)
            => point.GetMinimalDistanceTo(ray, out t);

        #endregion

        #region V3f - Plane3f

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use Plane3f.Height to compute the signed height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this V3f point, Plane3f plane)
            => plane.Height(point).Abs();

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use Plane3f.Height to compute the signed height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Plane3f plane, V3f point)
            => point.GetMinimalDistanceTo(plane);

        #endregion

        #region V3f - Box3f

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        public static float GetMinimalDistanceTo(this V3f point, Box3f box)
        {
            var outside = box.OutsideFlags(point); if (outside == 0) return 0;

            var d = V3f.Zero;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Box3f box, V3f point)
            => point.GetMinimalDistanceTo(box);

        #endregion

        #region V3f - Triangle3f

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this V3f point, Triangle3f triangle)
            => Triangle.Distance(triangle.P0, triangle.P1, triangle.P2, point);

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Triangle3f triangle, V3f point)
            => point.GetMinimalDistanceTo(triangle);

        #endregion

        #region Line3f - Line3f

        /// <summary>
        /// returns the minimal distance between the given lines.
        /// </summary>
        public static float GetMinimalDistanceTo(this Line3f l0, Line3f l1)
        {
            var r0 = l0.Ray3f;
            var r1 = l1.Ray3f;

            if (!r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                var distance = r0.GetMinimalDistanceTo(r1, out float t0, out float t1);

                if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
                    return distance;
            }

            // lines are parallel or t0 and t1 are outside of the line
            return Fun.Min(l0.P0.GetMinimalDistanceTo(l1), l0.P1.GetMinimalDistanceTo(l1),
                l1.P0.GetMinimalDistanceTo(l0), l1.P1.GetMinimalDistanceTo(l0));
        }

        /// <summary>
        /// returns the minimal distance between the given lines.
        /// points holds the centroid of the shortest connection between the lines
        /// </summary>
        public static float GetMinimalDistanceTo(this Line3f l0, Line3f l1, out V3f point)
        {
            var r0 = l0.Ray3f;
            var r1 = l1.Ray3f;

            var distance = r0.GetMinimalDistanceTo(r1, out float t0, out float t1);

            if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
            {
                point = 0.5f * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t1));
                return distance;
            }
            else
            {
                float t;
                if (t0 < 0 || t0 > 1)
                {
                    if (t0 < 0) t0 = 0;
                    else t0 = 1;

                    distance = r0.GetPointOnRay(t0).GetMinimalDistanceTo(r1, out t);
                    if (t >= 0 && t <= 1)
                    {
                        point = 0.5f * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t));
                        return distance;
                    }
                }

                if (t1 < 0 || t1 > 1)
                {
                    if (t1 < 0) t1 = 0;
                    else t1 = 1;

                    distance = r1.GetPointOnRay(t1).GetMinimalDistanceTo(r0, out t);
                    if (t >= 0 && t <= 1)
                    {
                        point = 0.5f * (r0.GetPointOnRay(t) + r1.GetPointOnRay(t1));
                        return distance;
                    }
                }

            }

            point = 0.5f * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t1));
            return (r0.GetPointOnRay(t0) - r1.GetPointOnRay(t1)).Length;
        }

        #endregion

        #region Line3f - Plane3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Line3f line, Plane3f plane)
            => plane.Height(GeometryFun.GetClosestPointOn(plane, line)).Abs();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Plane3f plane, Line3f line)
            => line.GetMinimalDistanceTo(plane);

        #endregion

        #region Ray3f - Line3f

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// </summary>
        public static float GetMinimalDistanceTo(this Ray3f ray, Line3f line)
        {
            var r1 = line.Ray3f;

            var distance = ray.GetMinimalDistanceTo(r1, out float t0, out float t1);

            if (t1 >= 0 && t1 <= 1) return distance;
            else
            {
                if (t1 < 0) t1 = 0;
                if (t1 > 1) t1 = 1;

                var p1 = r1.GetPointOnRay(t1);
                return p1.GetMinimalDistanceTo(ray);
            }
        }

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Line3f line, Ray3f ray)
            => ray.GetMinimalDistanceTo(line);

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// t holds the correspoinding ray parameter.
        /// </summary>
        public static float GetMinimalDistanceTo(this Ray3f ray, Line3f line, out float t)
        {
            var r1 = line.Ray3f;

            var distance = ray.GetMinimalDistanceTo(r1, out float t0, out float t1);

            if (t1 >= 0 && t1 <= 1)
            {
                t = t0;
                return distance;
            }
            else
            {
                if (t1 < 0) t1 = 0;
                if (t1 > 1) t1 = 1;

                var p1 = r1.GetPointOnRay(t1);
                return p1.GetMinimalDistanceTo(ray, out t);
            }
        }

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// t holds the correspoinding ray parameter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Line3f line, Ray3f ray, out float t)
            => ray.GetMinimalDistanceTo(line, out t);

        #endregion

        #region Ray3f - Ray3f

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMinimalDistanceTo(this Ray3f ray0, Ray3f ray1)
            => ray0.GetMinimalDistanceTo(ray1, out float t0, out float t1);

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// t0 and t1 hold the correspunding ray parameters.
        /// if both rays are parallel the t0 and t1 are from the origin of ray1
        /// </summary>
        public static float GetMinimalDistanceTo(this Ray3f ray0, Ray3f ray1, out float t0, out float t1)
        {
            // TODO: computation probalby possible without Direction.Normalized and Direction.Length

            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            if (uDotv.Abs().ApproximateEquals(1, Constant<float>.PositiveTinyValue)) // rays are parallel
            {
                // return origin of ray 1
                t1 = 0;
                t0 = ray0.GetTOfProjectedPoint(ray1.Origin);
            }
            else
            {
                // change by lui: added normalization (ortherwise in case the directions are not normalized t0 and t1 are wrong)
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 = t1 / ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        #endregion

        #region Range1d - Range1d

        public static double GetMinimalDistanceTo(this Range1d range0, Range1d range1)
        {
            if (range0.Min > range1.Max) return range0.Min - range1.Max;
            if (range0.Max < range1.Min) return range1.Min - range0.Max;
            return 0;
        }

        #endregion

        #region Ray2d - Ray2d

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this Ray2d ray0, Ray2d ray1)
            => ray0.GetMinimalDistanceTo(ray1, out double t0, out double t1);

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// t0 and t1 hold the correspunding ray parameters.
        /// if both rays are parallel the t0 and t1 are from the origin of ray1
        /// </summary>
        public static double GetMinimalDistanceTo(this Ray2d ray0, Ray2d ray1, out double t0, out double t1)
        {
            // NOTE: copy of Ray3d to Ray3d distance

            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            if (uDotv.Abs().ApproximateEquals(1, Constant<double>.PositiveTinyValue)) // rays are parallel
            {
                // return origin of ray 1
                t1 = 0;
                t0 = ray0.GetT(ray1.Origin);
            }
            else
            {
                // change by lui: added normalization (ortherwise in case the directions are not normalized t0 and t1 are wrong)
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 = t1 / ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
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

        #region V2d - Line2d

        /// <summary>
        /// returns the minimal distance between the point and the Line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this V2d point, Line2d line)
            => point.DistanceToLine(line.P0, line.P1);

        #endregion

        #region V2d - Triangle2d

        /// <summary>
        /// returns the minimal distance between the point and the triangle
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this V2d point, Triangle2d t)
            => Triangle.Distance(t.P0, t.P1, t.P2, point);

        #endregion

        #region Line2d - Line2d
        /// <summary>
        /// returns the minimal distance between the given lines.
        /// </summary>
        public static double GetMinimalDistanceTo(this Line2d l0, Line2d l1)
        {
            var r0 = l0.Ray2d;
            var r1 = l1.Ray2d;

            if (!r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                var distance = r0.GetMinimalDistanceTo(r1, out double t0, out double t1);

                if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
                    return distance;
            }

            // lines are parallel or t0 and t1 are outside of the line
            return Fun.Min(l0.P0.GetMinimalDistanceTo(l1), l0.P1.GetMinimalDistanceTo(l1),
                l1.P0.GetMinimalDistanceTo(l0), l1.P1.GetMinimalDistanceTo(l0));
        }
        #endregion

        #region V3d - Line3d

        /// <summary>
        /// returns the minimal distance between the point and the Line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this V3d point, Line3d line)
            => point.DistanceToLine(line.P0, line.P1);

        /// <summary>
        /// Returns the minimal distance between the point and the Line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            double acu2 = Vec.Cross(a, ray.Direction).LengthSquared;

            return Fun.Sqrt(acu2 / lu2);
        }

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            var acu2 = Vec.Cross(a, ray.Direction).LengthSquared;

            var NormalPart2 = acu2 / lu2;
            var ParallelPart2 = lu2 - NormalPart2;

            t = Fun.Sqrt(ParallelPart2 / lu2);
            return Fun.Sqrt(NormalPart2);
        }

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// t holds the ray parameter for the closest point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this Ray3d ray, V3d point, out double t)
            => point.GetMinimalDistanceTo(ray, out t);

        #endregion

        #region V3d - Plane3d

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use Plane3d.Height to compute the signed height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this V3d point, Plane3d plane)
            => plane.Height(point).Abs();

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use Plane3d.Height to compute the signed height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this Plane3d plane, V3d point)
            => point.GetMinimalDistanceTo(plane);

        #endregion

        #region V3d - Box3d

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        public static double GetMinimalDistanceTo(this V3d point, Box3d box)
        {
            var outside = box.OutsideFlags(point); if (outside == 0) return 0;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this Box3d box, V3d point)
            => point.GetMinimalDistanceTo(box);

        #endregion

        #region V3d - Triangle3d

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this V3d point, Triangle3d triangle)
            => Triangle.Distance(triangle.P0, triangle.P1, triangle.P2, point);

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this Triangle3d triangle, V3d point)
            => point.GetMinimalDistanceTo(triangle);

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

            // lines are parallel or t0 and t1 are outside of the line
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

            if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
            {
                point = 0.5 * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t1));
                return distance;
            }
            else
            {
                double t;
                if (t0 < 0 || t0 > 1)
                {
                    if (t0 < 0) t0 = 0;
                    else t0 = 1;

                    distance = r0.GetPointOnRay(t0).GetMinimalDistanceTo(r1, out t);
                    if (t >= 0 && t <= 1)
                    {
                        point = 0.5 * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t));
                        return distance;
                    }
                }

                if (t1 < 0 || t1 > 1)
                {
                    if (t1 < 0) t1 = 0;
                    else t1 = 1;

                    distance = r1.GetPointOnRay(t1).GetMinimalDistanceTo(r0, out t);
                    if (t >= 0 && t <= 1)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this Line3d line, Plane3d plane)
            => plane.Height(GeometryFun.GetClosestPointOn(plane, line)).Abs();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

            if (t1 >= 0 && t1 <= 1) return distance;
            else
            {
                if (t1 < 0) t1 = 0;
                if (t1 > 1) t1 = 1;

                var p1 = r1.GetPointOnRay(t1);
                return p1.GetMinimalDistanceTo(ray);
            }
        }

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

            if (t1 >= 0 && t1 <= 1)
            {
                t = t0;
                return distance;
            }
            else
            {
                if (t1 < 0) t1 = 0;
                if (t1 > 1) t1 = 1;

                var p1 = r1.GetPointOnRay(t1);
                return p1.GetMinimalDistanceTo(ray, out t);
            }
        }

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// t holds the correspoinding ray parameter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetMinimalDistanceTo(this Line3d line, Ray3d ray, out double t)
            => ray.GetMinimalDistanceTo(line, out t);

        #endregion

        #region Ray3d - Ray3d

        /// <summary>
        /// returns the minimal distance between the given rays.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 = t1 / ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        #endregion

    }
}
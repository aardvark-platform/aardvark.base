using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides various methods for middle point computations.
    /// </summary>
    public static partial class GeometryFun
    {
        //# foreach (var isDouble in new[] { false, true }) {
        //#   var rtype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var v3t = "V3" + tc;
        //#   var ray3t = "Ray3" + tc;
        //#   var half = isDouble ? "0.5" : "0.5f";
        /* __ray3t__ */

        #region __ray3t__ - __ray3t__

        public static __v3t__ GetMiddlePoint(this __ray3t__ ray0, __ray3t__ ray1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;

            var uDotv = u.Dot(v);
            var my = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1);
            var la = (my * uDotv - a.Dot(u));

            var p0 = ray0.Origin + la * u;
            var p1 = ray1.Origin + my * v;

            return __half__*(p0 + p1);
        }

        #endregion

        #region IEnumerable<__ray3t__>

        public static __v3t__ GetMiddlePoint(this IEnumerable<__ray3t__> rays)
        {
            var center = __v3t__.Zero;
            var count = 0;

            foreach(var r0 in rays)
                foreach (var r1 in rays)
                    if (r0.LexicalCompare(r1) < 0)
                    {
                        center += r0.GetMiddlePoint(r1);
                        ++count;
                    }

            return center / (__rtype__)count;
        }

        #endregion

        //# }
    }

    /// <summary>
    /// Provides various methods for closest point computations.
    /// If the query object is an surface or volume the function
    /// return to closest point on the surface.
    /// </summary>
    public static partial class GeometryFun
    {
        //# foreach (var isDouble in new[] { false, true }) {
        //#   var rtype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var v2t = "V2" + tc;
        //#   var v3t = "V3" + tc;
        //#   var line2t = "Line2" + tc;
        //#   var line3t = "Line3" + tc;
        //#   var plane2t = "Plane2" + tc;
        //#   var plane3t = "Plane3" + tc;
        //#   var ray2t = "Ray2" + tc;
        //#   var ray3t = "Ray3" + tc;
        //#   var circle2t = "Circle2" + tc;
        //#   var circle3t = "Circle3" + tc;
        //#   var quad2t = "Quad2" + tc;
        //#   var quad3t = "Quad3" + tc;
        //#   var range1t = "Range1" + tc;
        //#   var box2t = "Box2" + tc;
        //#   var box3t = "Box3" + tc;
        //#   var hull2t = "Hull2" + tc;
        //#   var hull3t = "Hull3" + tc;
        //#   var sphere3t = "Sphere3" + tc;
        //#   var cylinder3t = "Cylinder3" + tc;
        //#   var ellipse3t = "Ellipse3" + tc;
        //#   var triangle2t = "Triangle2" + tc;
        //#   var triangle3t = "Triangle3" + tc;
        //#   var polygon2t = "Polygon2" + tc;
        //#   var polygon3t = "Polygon3" + tc;
        //#   var trafo3t = "Trafo3" + tc;
        //#   var half = isDouble ? "0.5" : "0.5f";
        /* __v2t__ */

        #region __v2t__ - __line2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __v2t__ query, __line2t__ line)
            => GetClosestPointOn(query, line, out __rtype__ t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __line2t__ line, __v2t__ query)
            => query.GetClosestPointOn(line);

        public static __v2t__ GetClosestPointOn(this __v2t__ query, __line2t__ line, out __rtype__ t)
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
        public static __v2t__ GetClosestPointOn(this __line2t__ line, __v2t__ query, out __rtype__ t)
            => query.GetClosestPointOn(line, out t);

        #endregion

        #region __v2t__ - __ray2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __v2t__ query, __ray2t__ ray)
            => GetClosestPointOn(query, ray, out __rtype__ t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __ray2t__ ray, __v2t__ query)
            => query.GetClosestPointOn(ray);

        /// <summary>
        /// Returns the signed parameter on the supporting line of <paramref name="ray"/> at which the closest point to
        /// <paramref name="query"/> lies. The parameter is not clamped; a zero direction returns zero.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetClosestPointTOn(this __v2t__ query, __ray2t__ ray)
        {
            var lengthSquared = ray.Direction.LengthSquared;
            return lengthSquared == 0
                ? 0
                : Vec.Dot(query - ray.Origin, ray.Direction) / lengthSquared;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __v2t__ query, __ray2t__ ray, out __rtype__ t)
        {
            t = GetClosestPointTOn(query, ray);
            return ray.Origin + t * ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __ray2t__ ray, __v2t__ query, out __rtype__ t)
            => query.GetClosestPointOn(ray, out t);

        /// <summary>
        /// Returns the t-parameter along <paramref name="ray"/> at which the closest point to <paramref name="query"/> is.
        /// </summary>
        /// <param name="query">Find the closest point on the ray to this point.</param>
        /// <param name="ray">Find the closest point on this ray.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetClosestPointTOn(this __ray2t__ ray, __v2t__ query)
            => query.GetClosestPointTOn(ray);

        #endregion

        #region __v2t__ - __plane2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __v2t__ point, __plane2t__ line)
        {
            var lengthOfNormal2 = line.Normal.LengthSquared;
            return (point - (line.Normal.Dot(point - line.Point) / lengthOfNormal2) * line.Normal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __plane2t__ line, __v2t__ point)
            => point.GetClosestPointOn(line);

        #endregion

        #region __v2t__ - __box2t__

        public static __v2t__ GetClosestPointOn(this __v2t__ query, __box2t__ box)
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
        public static __v2t__ GetClosestPointOn(this __box2t__ box, __v2t__ query)
            => query.GetClosestPointOn(box);

        #endregion

        #region __v2t__ - __quad2t__

        public static __v2t__ GetClosestPointOn(this __v2t__ vec, __quad2t__ quad)
        {
            __v2t__ closestPoint;
            var temp = vec.GetClosestPointOn(new __line2t__(quad.P0,quad.P1));
            closestPoint = temp;

            temp = vec.GetClosestPointOn(new __line2t__(quad.P1, quad.P2));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            temp = vec.GetClosestPointOn(new __line2t__(quad.P2, quad.P3));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            temp = vec.GetClosestPointOn(new __line2t__(quad.P3, quad.P0));
            if ((temp - vec).LengthSquared < (closestPoint - vec).LengthSquared) closestPoint = temp;

            return closestPoint;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __quad2t__ quad, __v2t__ vec)
            => vec.GetClosestPointOn(quad);

        #endregion

        #region __v2t__ - __polygon2t__

        public static __v2t__ GetClosestPointOn(this __v2t__ vec, __polygon2t__ poly)
        {
            var closestPoint = __v2t__.PositiveInfinity;
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
        public static __v2t__ GetClosestPointOn(this __polygon2t__ poly, __v2t__ vec)
            => vec.GetClosestPointOn(poly);

        #endregion

        #region __v2t__ - __circle2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __v2t__ query, __circle2t__ circle)
            => circle.Center + circle.Radius * (query - circle.Center).Normalized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __circle2t__ circle, __v2t__ query)
            => query.GetClosestPointOn(circle);

        #endregion

        #region __v2t__ - __triangle2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __v2t__ query, __triangle2t__ triangle)
            => query.GetClosestPointOnTriangle(triangle.P0, triangle.P1, triangle.P2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ GetClosestPointOn(this __triangle2t__ triangle, __v2t__ query)
            => query.GetClosestPointOn(triangle);

        public static __v2t__ GetClosestPointOnTriangle(this __v2t__ query, __v2t__ a, __v2t__ b, __v2t__ c)
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

        /* __v3t__ */

        #region __v3t__ - __box3t__

        public static __rtype__ GetDistanceSquared(this __v3t__ q, __box3t__ b)
            => (q.X < b.Min.X ? Fun.Square(b.Min.X - q.X) : q.X > b.Max.X ? Fun.Square(q.X - b.Max.X) : 0)
             + (q.Y < b.Min.Y ? Fun.Square(b.Min.Y - q.Y) : q.Y > b.Max.Y ? Fun.Square(q.Y - b.Max.Y) : 0)
             + (q.Z < b.Min.Z ? Fun.Square(b.Min.Z - q.Z) : q.Z > b.Max.Z ? Fun.Square(q.Z - b.Max.Z) : 0);

        public static (__rtype__, __rtype__) GetDistanceSquared(
            this __v3t__ q, __box3t__AndFlags boxAndFlags, Box.Flags d2Flags, __v3t__ d2v,
            out Box.Flags d2Flags0, out __v3t__ d2v0,
            out Box.Flags d2Flags1, out __v3t__ d2v1)
        {
            d2Flags0 = d2Flags; d2Flags1 = d2Flags;
            d2v0 = d2v; d2v1 = d2v;

            __rtype__ d0 = 0, d1 = 0;

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

        public static __v3t__ GetClosestPointOn(this __v3t__ query, __box3t__ box)
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
        public static __v3t__ GetClosestPointOn(this __box3t__ box, __v3t__ query)
            => query.GetClosestPointOn(box);

        #endregion

        #region __v3t__ - __line3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __line3t__ line)
            => query.GetClosestPointOnLine(line.P0, line.P1, out __rtype__ t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __line3t__ line, __v3t__ query)
            => query.GetClosestPointOn(line);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __line3t__ line, out __rtype__ t)
            => query.GetClosestPointOnLine(line.P0, line.P1, out t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __line3t__ line, __v3t__ query, out __rtype__ t)
            => query.GetClosestPointOn(line, out t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOnLine(this __v3t__ query, __v3t__ p0, __v3t__ p1)
            => query.GetClosestPointOnLine(p0, p1, out __rtype__ t);

        public static __v3t__ GetClosestPointOnLine(this __v3t__ query, __v3t__ p0, __v3t__ p1, out __rtype__ t)
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

        #region __v3t__ - __plane3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __plane3t__ plane)
            => query - plane.Height(query) * plane.Normal;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __plane3t__ plane, __v3t__ query)
            => query.GetClosestPointOn(plane);

        #endregion

        #region __v3t__ - __ray3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __ray3t__ ray)
            => GetClosestPointOn(query, ray, out __rtype__ t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __ray3t__ ray, __v3t__ query)
            => query.GetClosestPointOn(ray);

        /// <summary>
        /// Returns the closest point on the supporting line of <paramref name="ray"/> and its signed, unclamped parameter.
        /// A ray with a zero direction is treated as its origin and returns parameter zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __ray3t__ ray, out __rtype__ t)
        {
            var lengthSquared = ray.Direction.LengthSquared;
            t = lengthSquared == 0
                ? 0
                : Vec.Dot(query - ray.Origin, ray.Direction) / lengthSquared;
            return ray.Origin + t * ray.Direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __ray3t__ ray, __v3t__ query, out __rtype__ t)
            => query.GetClosestPointOn(ray, out t);

        #endregion

        #region __v3t__ - __sphere3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __sphere3t__ sphere)
            => sphere.Center + sphere.Radius * (query - sphere.Center).Normalized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __sphere3t__ sphere, __v3t__ query)
            => query.GetClosestPointOn(sphere);

        #endregion

        #region __v3t__ - __cylinder3t__

        /// <summary>
        /// get closest point on cylinder with infinite length
        /// </summary>
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __cylinder3t__ cylinder)
        {
            var p = query.GetClosestPointOn(cylinder.Axis.__ray3t__);
            var dir = (query - p).Normalized;
            return p + cylinder.Radius * dir;
        }

        /// <summary>
        /// get closest point on cylinder with infinite length
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __cylinder3t__ cylinder, __v3t__ query)
            => query.GetClosestPointOn(cylinder);

        #endregion

        #region __v3t__ - __triangle3t__

        /// <summary>
        /// Tested by rft on 2008-08-06.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __v3t__ query, __triangle3t__ triangle)
            => GetClosestPointOnTriangle(query, triangle.P0, triangle.P1, triangle.P2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetClosestPointOn(this __triangle3t__ triangle, __v3t__ query)
            => query.GetClosestPointOn(triangle);

        /// <summary>
        /// Tested by rft on 2008-08-06.
        /// </summary>
        public static __v3t__ GetClosestPointOnTriangle(this __v3t__ query, __v3t__ p0, __v3t__ p1, __v3t__ p2)
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

            __rtype__ denom = 1 / (va + vb + vc);
            var v = vb * denom;
            var w = vc * denom;
            return p0 + v * e01 + w * e02; // bary (1-v-w, v, w)
        }

        #endregion

        /* __ray3t__ */

        #region __ray3t__ - __ray3t__

        /// <summary>
        /// Returns the point on the supporting line of <paramref name="ray1"/> closest to the supporting line of
        /// <paramref name="ray0"/>. Zero directions are treated as points.
        /// </summary>
        public static __v3t__ GetClosestPointOn(this __ray3t__ ray0, __ray3t__ ray1)
        {
            GetClosestRayParameters(ray0, ray1, out __rtype__ t0, out __rtype__ t1);
            return ray1.Origin + t1 * ray1.Direction;
        }

        #endregion

        /* __line3t__ */

        #region __line3t__ - __line3t__

        /// <summary>
        /// Returns the point on line1 which is closest to line0.
        /// </summary>
        public static __v3t__ GetClosestPointOn(this __line3t__ line0, __line3t__ line1)
        {
            var r0 = line0.__ray3t__;
            var r1 = line1.__ray3t__;

            if (r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                if (line0.P0.GetMinimalDistanceTo(line1) < line0.P1.GetMinimalDistanceTo(line1))
                    return line0.P0.GetClosestPointOn(line1);
                else
                    return line0.P1.GetClosestPointOn(line1);
            }

            /* var distance = */
            r0.GetMinimalDistanceTo(r1, out __rtype__ t0, out __rtype__ t1);
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

        #region __line3t__ - __plane3t__

        /// <summary>
        /// Returns the closest point from a line to a plane (point on line).
        /// </summary>
        public static __v3t__ GetClosestPointOn(this __plane3t__ plane, __line3t__ line)
        {
            var ray = line.__ray3t__;
            if (ray.Intersects(plane, out __rtype__ t))
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

        #region __line3t__ - __triangle3t__

        /// <summary>
        /// Returns the closest point from a triangle to a line (point on triangle).
        /// Note: untested
        /// </summary>
        public static __v3t__ GetClosestPointOn(this __line3t__ line, __triangle3t__ triangle)
        {
            // test if closest point on triangle plane is inside the triangle (we have the point),
            // otherwiese find closest edge and take closest point to that edge

            var plane = triangle.Plane;
            var planePoint = plane.GetClosestPointOn(line);

            // switch to 2d:
            var trafo = __trafo3t__.FromNormalFrame(plane.Point, plane.Normal);
            var point2d = trafo.Backward.TransformPos(planePoint).XY;
            var triangle2d = new __triangle2t__(trafo.Backward.TransformPos(triangle.P0).XY,
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

        //# }
    }

    public static partial class GeometryFun
    {
        //# foreach (var isDouble in new[] { false, true }) {
        //#   var rtype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var v2t = "V2" + tc;
        //#   var v3t = "V3" + tc;
        //#   var line2t = "Line2" + tc;
        //#   var line3t = "Line3" + tc;
        //#   var plane2t = "Plane2" + tc;
        //#   var plane3t = "Plane3" + tc;
        //#   var ray2t = "Ray2" + tc;
        //#   var ray3t = "Ray3" + tc;
        //#   var circle2t = "Circle2" + tc;
        //#   var circle3t = "Circle3" + tc;
        //#   var quad2t = "Quad2" + tc;
        //#   var quad3t = "Quad3" + tc;
        //#   var range1t = "Range1" + tc;
        //#   var box2t = "Box2" + tc;
        //#   var box3t = "Box3" + tc;
        //#   var hull2t = "Hull2" + tc;
        //#   var hull3t = "Hull3" + tc;
        //#   var sphere3t = "Sphere3" + tc;
        //#   var cylinder3t = "Cylinder3" + tc;
        //#   var ellipse3t = "Ellipse3" + tc;
        //#   var triangle2t = "Triangle2" + tc;
        //#   var triangle3t = "Triangle3" + tc;
        //#   var polygon2t = "Polygon2" + tc;
        //#   var polygon3t = "Polygon3" + tc;
        //#   var half = isDouble ? "0.5" : "0.5f";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetClosestRayParameters(__ray2t__ ray0, __ray2t__ ray1, out __rtype__ t0, out __rtype__ t1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction;
            var v = ray1.Direction;
            var uu = Vec.Dot(u, u);
            var vv = Vec.Dot(v, v);

            if (uu == 0)
            {
                t0 = 0;
                t1 = vv == 0 ? 0 : Vec.Dot(a, v) / vv;
                return;
            }

            if (vv == 0)
            {
                t0 = -Vec.Dot(a, u) / uu;
                t1 = 0;
                return;
            }

            var uv = Vec.Dot(u, v);
            var scale = uu * vv;
            var determinant = scale - uv * uv;
            var epsilon = Constant<__rtype__>.PositiveTinyValue;

            if (determinant <= epsilon * (2 - epsilon) * scale)
            {
                t0 = -Vec.Dot(a, u) / uu;
                t1 = 0;
                return;
            }

            var au = Vec.Dot(a, u);
            var av = Vec.Dot(a, v);
            t0 = (uv * av - vv * au) / determinant;
            t1 = (uu * av - uv * au) / determinant;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetClosestRayParameters(__ray3t__ ray0, __ray3t__ ray1, out __rtype__ t0, out __rtype__ t1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction;
            var v = ray1.Direction;
            var uu = Vec.Dot(u, u);
            var vv = Vec.Dot(v, v);

            if (uu == 0)
            {
                t0 = 0;
                t1 = vv == 0 ? 0 : Vec.Dot(a, v) / vv;
                return;
            }

            if (vv == 0)
            {
                t0 = -Vec.Dot(a, u) / uu;
                t1 = 0;
                return;
            }

            var uv = Vec.Dot(u, v);
            var scale = uu * vv;
            var determinant = scale - uv * uv;
            var epsilon = Constant<__rtype__>.PositiveTinyValue;

            if (determinant <= epsilon * (2 - epsilon) * scale)
            {
                t0 = -Vec.Dot(a, u) / uu;
                t1 = 0;
                return;
            }

            var au = Vec.Dot(a, u);
            var av = Vec.Dot(a, v);
            t0 = (uv * av - vv * au) / determinant;
            t1 = (uu * av - uv * au) / determinant;
        }

        #region __range1t__ - __range1t__

        public static __rtype__ GetMinimalDistanceTo(this __range1t__ range0, __range1t__ range1)
        {
            if (range0.Min > range1.Max) return range0.Min - range1.Max;
            if (range0.Max < range1.Min) return range1.Min - range0.Max;
            return 0;
        }

        #endregion

        #region __ray2t__ - __ray2t__

        /// <summary>
        /// Returns the minimal distance between the supporting lines represented by the given rays.
        /// Zero directions are treated as points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __ray2t__ ray0, __ray2t__ ray1)
            => ray0.GetMinimalDistanceTo(ray1, out __rtype__ t0, out __rtype__ t1);

        /// <summary>
        /// Returns the minimal distance between the supporting lines represented by the given rays.
        /// <paramref name="t0"/> and <paramref name="t1"/> are signed parameters in the original directions.
        /// For parallel directions, <paramref name="t1"/> is zero and <paramref name="t0"/> projects the second origin
        /// onto the first line. Zero directions are treated as points and use parameter zero.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __ray2t__ ray0, __ray2t__ ray1, out __rtype__ t0, out __rtype__ t1)
        {
            var a = ray0.Origin - ray1.Origin;
            GetClosestRayParameters(ray0, ray1, out t0, out t1);
            return (a + t0 * ray0.Direction - t1 * ray1.Direction).Length;
        }

        #endregion

        #region __box2t__ - __box2t__

        public static __rtype__ GetMinimalDistanceTo(this __box2t__ box0, __box2t__ box1)
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

        #region __v2t__ - __line2t__

        /// <summary>
        /// returns the minimal distance between the point and the Line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __v2t__ point, __line2t__ line)
            => point.DistanceToLine(line.P0, line.P1);

        #endregion

        #region __v2t__ - __triangle2t__

        /// <summary>
        /// returns the minimal distance between the point and the triangle
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __v2t__ point, __triangle2t__ t)
            => Triangle.Distance(t.P0, t.P1, t.P2, point);

        #endregion

        #region __line2t__ - __line2t__
        /// <summary>
        /// returns the minimal distance between the given lines.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __line2t__ l0, __line2t__ l1)
        {
            var r0 = l0.__ray2t__;
            var r1 = l1.__ray2t__;

            if (!r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                var distance = r0.GetMinimalDistanceTo(r1, out __rtype__ t0, out __rtype__ t1);

                if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
                    return distance;
            }

            // lines are parallel or t0 and t1 are outside of the line
            return Fun.Min(l0.P0.GetMinimalDistanceTo(l1), l0.P1.GetMinimalDistanceTo(l1),
                l1.P0.GetMinimalDistanceTo(l0), l1.P1.GetMinimalDistanceTo(l0));
        }
        #endregion

        #region __v3t__ - __line3t__

        /// <summary>
        /// returns the minimal distance between the point and the Line
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __v3t__ point, __line3t__ line)
            => point.DistanceToLine(line.P0, line.P1);

        /// <summary>
        /// Returns the minimal distance between the point and the Line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __line3t__ line, __v3t__ point)
            => point.GetMinimalDistanceTo(line);

        #endregion

        #region __v3t__ - __ray3t__

        /// <summary>
        /// Returns the minimal distance between the point and the supporting line represented by the ray.
        /// A ray with a zero direction is treated as its origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __v3t__ point, __ray3t__ ray)
            => point.GetMinimalDistanceTo(ray, out __rtype__ t);

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __ray3t__ ray, __v3t__ point)
            => point.GetMinimalDistanceTo(ray);

        /// <summary>
        /// Returns the minimal distance between the point and the supporting line represented by the ray.
        /// <paramref name="t"/> receives the signed, unclamped ray parameter. A zero direction is treated as the ray
        /// origin and returns parameter zero.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __v3t__ point, __ray3t__ ray, out __rtype__ t)
        {
            var a = point - ray.Origin;
            var lengthSquared = ray.Direction.LengthSquared;
            if (lengthSquared == 0)
            {
                t = 0;
                return a.Length;
            }

            t = Vec.Dot(a, ray.Direction) / lengthSquared;
            return (a - t * ray.Direction).Length;
        }

        /// <summary>
        /// returns the minimal distance between the point and the Ray.
        /// t holds the ray parameter for the closest point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __ray3t__ ray, __v3t__ point, out __rtype__ t)
            => point.GetMinimalDistanceTo(ray, out t);

        #endregion

        #region __v3t__ - __plane3t__

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use __plane3t__.Height to compute the signed height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __v3t__ point, __plane3t__ plane)
            => plane.Height(point).Abs();

        /// <summary>
        /// Returns the minimal distance (unsigned) between the point and the plane.
        /// Use __plane3t__.Height to compute the signed height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __plane3t__ plane, __v3t__ point)
            => point.GetMinimalDistanceTo(plane);

        #endregion

        #region __v3t__ - __box3t__

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __v3t__ point, __box3t__ box)
        {
            var outside = box.OutsideFlags(point); if (outside == 0) return 0;

            var d = __v3t__.Zero;

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
        public static __rtype__ GetMinimalDistanceTo(this __box3t__ box, __v3t__ point)
            => point.GetMinimalDistanceTo(box);

        #endregion

        #region __v3t__ - __triangle3t__

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __v3t__ point, __triangle3t__ triangle)
            => Triangle.Distance(triangle.P0, triangle.P1, triangle.P2, point);

        /// <summary>
        /// returns the minimal distance between the point and the box.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __triangle3t__ triangle, __v3t__ point)
            => point.GetMinimalDistanceTo(triangle);

        #endregion

        #region __line3t__ - __line3t__

        /// <summary>
        /// returns the minimal distance between the given lines.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __line3t__ l0, __line3t__ l1)
        {
            var r0 = l0.__ray3t__;
            var r1 = l1.__ray3t__;

            if (!r0.IsParallelTo(r1)) // t1 and t0 of following computation are invalid if lines are parallel
            {
                var distance = r0.GetMinimalDistanceTo(r1, out __rtype__ t0, out __rtype__ t1);

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
        public static __rtype__ GetMinimalDistanceTo(this __line3t__ l0, __line3t__ l1, out __v3t__ point)
        {
            var r0 = l0.__ray3t__;
            var r1 = l1.__ray3t__;

            var distance = r0.GetMinimalDistanceTo(r1, out __rtype__ t0, out __rtype__ t1);

            if (t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
            {
                point = __half__ * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t1));
                return distance;
            }
            else
            {
                __rtype__ t;
                if (t0 < 0 || t0 > 1)
                {
                    if (t0 < 0) t0 = 0;
                    else t0 = 1;

                    distance = r0.GetPointOnRay(t0).GetMinimalDistanceTo(r1, out t);
                    if (t >= 0 && t <= 1)
                    {
                        point = __half__ * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t));
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
                        point = __half__ * (r0.GetPointOnRay(t) + r1.GetPointOnRay(t1));
                        return distance;
                    }
                }

            }

            point = __half__ * (r0.GetPointOnRay(t0) + r1.GetPointOnRay(t1));
            return (r0.GetPointOnRay(t0) - r1.GetPointOnRay(t1)).Length;
        }

        #endregion

        #region __line3t__ - __plane3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __line3t__ line, __plane3t__ plane)
            => plane.Height(GeometryFun.GetClosestPointOn(plane, line)).Abs();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __plane3t__ plane, __line3t__ line)
            => line.GetMinimalDistanceTo(plane);

        #endregion

        #region __ray3t__ - __line3t__

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __ray3t__ ray, __line3t__ line)
        {
            var r1 = line.__ray3t__;

            var distance = ray.GetMinimalDistanceTo(r1, out __rtype__ t0, out __rtype__ t1);

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
        public static __rtype__ GetMinimalDistanceTo(this __line3t__ line, __ray3t__ ray)
            => ray.GetMinimalDistanceTo(line);

        /// <summary>
        /// returns the minimal distance between the ray and the line.
        /// t holds the correspoinding ray parameter.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __ray3t__ ray, __line3t__ line, out __rtype__ t)
        {
            var r1 = line.__ray3t__;

            var distance = ray.GetMinimalDistanceTo(r1, out __rtype__ t0, out __rtype__ t1);

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
        public static __rtype__ GetMinimalDistanceTo(this __line3t__ line, __ray3t__ ray, out __rtype__ t)
            => ray.GetMinimalDistanceTo(line, out t);

        #endregion

        #region __ray3t__ - __ray3t__

        /// <summary>
        /// Returns the minimal distance between the supporting lines represented by the given rays.
        /// Zero directions are treated as points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetMinimalDistanceTo(this __ray3t__ ray0, __ray3t__ ray1)
            => ray0.GetMinimalDistanceTo(ray1, out __rtype__ t0, out __rtype__ t1);

        /// <summary>
        /// Returns the minimal distance between the supporting lines represented by the given rays.
        /// <paramref name="t0"/> and <paramref name="t1"/> are signed parameters in the original directions.
        /// For parallel directions, <paramref name="t1"/> is zero and <paramref name="t0"/> projects the second origin
        /// onto the first line. Zero directions are treated as points and use parameter zero.
        /// </summary>
        public static __rtype__ GetMinimalDistanceTo(this __ray3t__ ray0, __ray3t__ ray1, out __rtype__ t0, out __rtype__ t1)
        {
            var a = ray0.Origin - ray1.Origin;
            GetClosestRayParameters(ray0, ray1, out t0, out t1);
            return (a + t0 * ray0.Direction - t1 * ray1.Direction).Length;
        }

        #endregion

        //# }
    }
}

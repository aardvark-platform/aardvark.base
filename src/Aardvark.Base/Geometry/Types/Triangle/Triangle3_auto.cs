using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Triangle3f

    /// <summary>
    /// A three-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct Triangle3f : IBoundingSphere3f
    {
        #region Geometric Properties

        /// <summary>
        /// Returns the area of the triangle.
        /// </summary>
        public readonly float Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Area(this);
        }

        /// <summary>
        /// Returns whether the triangle is degenerated, i.e. its area is zero.
        /// </summary>
        public readonly bool IsDegenerated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.IsDegenerated(this);
        }

        /// <summary>
        /// Returns the normal of the triangle.
        /// </summary>
        public readonly V3f Normal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Normal(this);
        }

        /// <summary>
        /// Returns the plane that contains the points of the triangle.
        /// </summary>
        public readonly Plane3f Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Plane(this);
        }

        #endregion

        #region IBoundingSphere3f Members

        /// <summary>
        /// Returns the smallest enclosing sphere, using a longest-edge diameter for right,
        /// obtuse or collinear triangles and zero radius for coincident points.
        /// Non-finite points return Invalid. The radius accounts for rounding of the center.
        /// </summary>
        public readonly Sphere3f BoundingSphere3f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = P1 - P0; var b = P2 - P0; var c = P2 - P1;
                var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
                var sum = aa + bb + cc;
                if (!(sum >= 1e-10f && sum <= 1e10f)) return BoundingSphereScaled();
                var origin = P0; var d = a; var e = b; var f = c;
                bool useE = bb <= cc;
                if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
                else if (cc > aa && cc > bb) { origin = P1; d = c; e = -a; f = -b; useE = aa <= bb; }
                var offset = 0.5f * d;
                var dot = e.Dot(f);
                if (dot > 0)
                {
                    // The shorter leg stays at least 45 degrees from the longest-edge line.
                    var relative = useE ? e : f;
                    var perpendicular = relative - d * (relative.Dot(d) / d.LengthSquared);
                    var pp = perpendicular.LengthSquared;
                    if (pp == 0) return BoundingSphereScaled();
                    offset += perpendicular * (dot / (2 * pp));
                }
                var center = origin + offset;
                var r2 = Fun.Max((center - P0).LengthSquared, (center - P1).LengthSquared, (center - P2).LengthSquared);
                return new Sphere3f(center, r2.Sqrt());
            }
        }

        private readonly Sphere3f BoundingSphereScaled()
        {
            var p0 = (V3d)P0; var p1 = (V3d)P1; var p2 = (V3d)P2;
            if (!p0.IsFinite || !p1.IsFinite || !p2.IsFinite) return Sphere3f.Invalid;
            var a = p1 - p0; var b = p2 - p0; var c = p2 - p1;
            bool scaledPoints = !a.IsFinite || !b.IsFinite || !c.IsFinite;
            double scale;
            if (scaledPoints)
            {
                scale = Fun.Max(p0.NormMax, p1.NormMax, p2.NormMax);
                p0 = DivideForBounds(p0, scale); p1 = DivideForBounds(p1, scale); p2 = DivideForBounds(p2, scale);
                a = p1 - p0; b = p2 - p0; c = p2 - p1;
            }
            else
            {
                scale = Fun.Max(a.NormMax, b.NormMax, c.NormMax);
                if (scale == 0) return new Sphere3f(P0, 0);
                a = DivideForBounds(a, scale); b = DivideForBounds(b, scale); c = DivideForBounds(c, scale);
            }
            var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
            var origin = p0; var d = a; var e = b; var f = c;
            bool useE = bb <= cc;
            if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
            else if (cc > aa && cc > bb) { origin = p1; d = c; e = -a; f = -b; useE = aa <= bb; }
            var offset = 0.5 * d;
            double dot = e.Dot(f);
            if (dot > 0)
            {
                var relative = useE ? e : f;
                var n = d.Cross(relative);
                double nm = n.NormMax;
                if (nm > 0)
                {
                    // Normalize before the second cross product, avoiding a squared tiny normal.
                    var perpendicular = DivideForBounds(n, nm).Cross(d);
                    double denominator = 2 * perpendicular.Dot(relative);
                    if (denominator != 0) offset += perpendicular * (dot / denominator);
                }
            }
            var center = scaledPoints ? (origin + offset) * scale : origin + offset * scale;
            // The minimum center lies in the convex hull; clamp only reconstruction roundoff.
            center.X = Fun.Clamp(center.X, Fun.Min(P0.X, P1.X, P2.X), Fun.Max(P0.X, P1.X, P2.X));
            center.Y = Fun.Clamp(center.Y, Fun.Min(P0.Y, P1.Y, P2.Y), Fun.Max(P0.Y, P1.Y, P2.Y));
            center.Z = Fun.Clamp(center.Z, Fun.Min(P0.Z, P1.Z, P2.Z), Fun.Max(P0.Z, P1.Z, P2.Z));
            var rounded = (V3f)center;
            double radius = Fun.Max(BoundsDistance((V3d)rounded, (V3d)P0), BoundsDistance((V3d)rounded, (V3d)P1), BoundsDistance((V3d)rounded, (V3d)P2));
            return new Sphere3f(rounded, (float)radius);
        }

        private static V3d DivideForBounds(V3d p, double scale)
            => new V3d(p.X / scale, p.Y / scale, p.Z / scale);

        private static double BoundsDistance(V3d a, V3d b)
        {
            var d = a - b;
            double scale = d.NormMax;
            if (scale == 0 || double.IsPositiveInfinity(scale)) return scale;
            d = DivideForBounds(d, scale);
            return scale * d.Length;
        }

        #endregion
    }

    /// <summary>
    /// Contains static methods for triangles.
    /// </summary>
    public static partial class Triangle
    {
        #region Area

        /// <summary>
        /// Returns the area of the triangle defined by the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Area(V3f p0, V3f p1, V3f p2)
            => (p1 - p0).Cross(p2 - p0).Length * 0.5f;

        /// <summary>
        /// Returns the area of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Area(Triangle3f t)
            => Area(t.P0, t.P1, t.P2);

        #endregion

        #region IsDegenerated

        /// <summary>
        /// Returns whether the triangle defined by the give points is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(V3f p0, V3f p1, V3f p2)
            => (p1 - p0).Cross(p2 - p0).AllTiny;

        /// <summary>
        /// Returns whether the given triangle is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(Triangle3f t)
            => IsDegenerated(t.P0, t.P1, t.P2);

        #endregion

        #region Normal

        /// <summary>
        /// Returns the normal of the triangle defined by the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f Normal(V3f p0, V3f p1, V3f p2)
            => (p1 - p0).Cross(p2 - p0).Normalized;

        /// <summary>
        /// Returns the normal of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f Normal(Triangle3f t)
            => Normal(t.P0, t.P1, t.P2);

        #endregion

        #region Plane

        /// <summary>
        /// Returns the plane that contains the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane3f Plane(V3f p0, V3f p1, V3f p2)
            => new Plane3f(Normal(p0, p1, p2), p0);

        /// <summary>
        /// Returns the plane that contains the points of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane3f Plane(Triangle3f t)
            => Plane(t.P0, t.P1, t.P2);

        #endregion

        #region SolidAngle

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SolidAngle(Triangle3f t)
            => SolidAngle(t.P0, t.P1, t.P2);

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the origin.
        ///
        /// Van Oosterom, A., and Strackee, J. (1983). 
        /// The solid angle of a plane triangle.
        /// IEEE transactions on Biomedical Engineering, (2), 125-126.
        /// 
        /// https://en.wikipedia.org/wiki/Solid_angle#Tetrahedron
        /// </summary>
        public static float SolidAngle(V3f va, V3f vb, V3f vc)
        {
            var ma = va.Length;
            var mb = vb.Length;
            var mc = vc.Length;

            var numerator = Fun.Abs(Vec.Dot(va, (Vec.Cross(vb, vc))));

            var denom = ma * mb * mc + Vec.Dot(va, vb) * mc + Vec.Dot(va, vc) * mb + Vec.Dot(vb, vc) * ma;

            var halfSA = Fun.Atan2(numerator, denom);

            return 2 * (halfSA >= 0 ? halfSA : halfSA + ConstantF.Pi);
        }

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the given point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SolidAngle(V3f va, V3f vb, V3f vc, V3f p)
            => SolidAngle(va - p, vb - p, vc - p);

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the given point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SolidAngle(Triangle3f t, V3f p)
            => SolidAngle(t.P0 - p, t.P1 - p, t.P2 - p);

        #endregion

        #region Distance

        /// <summary>
        /// Gets the distance between the closest point on the triangle [a, b, c] and the given query point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(V3f a, V3f b, V3f c, V3f query)
        {
            var cp = query.GetClosestPointOnTriangle(a, b, c);
            return (cp - query).Length;
        }

        #endregion
    }

    #endregion

    #region Triangle3d

    /// <summary>
    /// A three-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct Triangle3d : IBoundingSphere3d
    {
        #region Geometric Properties

        /// <summary>
        /// Returns the area of the triangle.
        /// </summary>
        public readonly double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Area(this);
        }

        /// <summary>
        /// Returns whether the triangle is degenerated, i.e. its area is zero.
        /// </summary>
        public readonly bool IsDegenerated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.IsDegenerated(this);
        }

        /// <summary>
        /// Returns the normal of the triangle.
        /// </summary>
        public readonly V3d Normal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Normal(this);
        }

        /// <summary>
        /// Returns the plane that contains the points of the triangle.
        /// </summary>
        public readonly Plane3d Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Plane(this);
        }

        #endregion

        #region IBoundingSphere3d Members

        /// <summary>
        /// Returns the smallest enclosing sphere, using a longest-edge diameter for right,
        /// obtuse or collinear triangles and zero radius for coincident points.
        /// Non-finite points return Invalid. The radius accounts for rounding of the center.
        /// </summary>
        public readonly Sphere3d BoundingSphere3d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = P1 - P0; var b = P2 - P0; var c = P2 - P1;
                var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
                var sum = aa + bb + cc;
                if (!(sum >= 1e-100 && sum <= 1e100)) return BoundingSphereScaled();
                var origin = P0; var d = a; var e = b; var f = c;
                bool useE = bb <= cc;
                if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
                else if (cc > aa && cc > bb) { origin = P1; d = c; e = -a; f = -b; useE = aa <= bb; }
                var offset = 0.5 * d;
                var dot = e.Dot(f);
                if (dot > 0)
                {
                    // The shorter leg stays at least 45 degrees from the longest-edge line.
                    var relative = useE ? e : f;
                    var perpendicular = relative - d * (relative.Dot(d) / d.LengthSquared);
                    var pp = perpendicular.LengthSquared;
                    if (pp == 0) return BoundingSphereScaled();
                    offset += perpendicular * (dot / (2 * pp));
                }
                var center = origin + offset;
                var r2 = Fun.Max((center - P0).LengthSquared, (center - P1).LengthSquared, (center - P2).LengthSquared);
                return new Sphere3d(center, r2.Sqrt());
            }
        }

        private readonly Sphere3d BoundingSphereScaled()
        {
            var p0 = (V3d)P0; var p1 = (V3d)P1; var p2 = (V3d)P2;
            if (!p0.IsFinite || !p1.IsFinite || !p2.IsFinite) return Sphere3d.Invalid;
            var a = p1 - p0; var b = p2 - p0; var c = p2 - p1;
            bool scaledPoints = !a.IsFinite || !b.IsFinite || !c.IsFinite;
            double scale;
            if (scaledPoints)
            {
                scale = Fun.Max(p0.NormMax, p1.NormMax, p2.NormMax);
                p0 = DivideForBounds(p0, scale); p1 = DivideForBounds(p1, scale); p2 = DivideForBounds(p2, scale);
                a = p1 - p0; b = p2 - p0; c = p2 - p1;
            }
            else
            {
                scale = Fun.Max(a.NormMax, b.NormMax, c.NormMax);
                if (scale == 0) return new Sphere3d(P0, 0);
                a = DivideForBounds(a, scale); b = DivideForBounds(b, scale); c = DivideForBounds(c, scale);
            }
            var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
            var origin = p0; var d = a; var e = b; var f = c;
            bool useE = bb <= cc;
            if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
            else if (cc > aa && cc > bb) { origin = p1; d = c; e = -a; f = -b; useE = aa <= bb; }
            var offset = 0.5 * d;
            double dot = e.Dot(f);
            if (dot > 0)
            {
                var relative = useE ? e : f;
                var n = d.Cross(relative);
                double nm = n.NormMax;
                if (nm > 0)
                {
                    // Normalize before the second cross product, avoiding a squared tiny normal.
                    var perpendicular = DivideForBounds(n, nm).Cross(d);
                    double denominator = 2 * perpendicular.Dot(relative);
                    if (denominator != 0) offset += perpendicular * (dot / denominator);
                }
            }
            var center = scaledPoints ? (origin + offset) * scale : origin + offset * scale;
            // The minimum center lies in the convex hull; clamp only reconstruction roundoff.
            center.X = Fun.Clamp(center.X, Fun.Min(P0.X, P1.X, P2.X), Fun.Max(P0.X, P1.X, P2.X));
            center.Y = Fun.Clamp(center.Y, Fun.Min(P0.Y, P1.Y, P2.Y), Fun.Max(P0.Y, P1.Y, P2.Y));
            center.Z = Fun.Clamp(center.Z, Fun.Min(P0.Z, P1.Z, P2.Z), Fun.Max(P0.Z, P1.Z, P2.Z));
            var rounded = (V3d)center;
            double radius = Fun.Max(BoundsDistance((V3d)rounded, (V3d)P0), BoundsDistance((V3d)rounded, (V3d)P1), BoundsDistance((V3d)rounded, (V3d)P2));
            return new Sphere3d(rounded, (double)radius);
        }

        private static V3d DivideForBounds(V3d p, double scale)
            => new V3d(p.X / scale, p.Y / scale, p.Z / scale);

        private static double BoundsDistance(V3d a, V3d b)
        {
            var d = a - b;
            double scale = d.NormMax;
            if (scale == 0 || double.IsPositiveInfinity(scale)) return scale;
            d = DivideForBounds(d, scale);
            return scale * d.Length;
        }

        #endregion
    }

    /// <summary>
    /// Contains static methods for triangles.
    /// </summary>
    public static partial class Triangle
    {
        #region Area

        /// <summary>
        /// Returns the area of the triangle defined by the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Area(V3d p0, V3d p1, V3d p2)
            => (p1 - p0).Cross(p2 - p0).Length * 0.5;

        /// <summary>
        /// Returns the area of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Area(Triangle3d t)
            => Area(t.P0, t.P1, t.P2);

        #endregion

        #region IsDegenerated

        /// <summary>
        /// Returns whether the triangle defined by the give points is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(V3d p0, V3d p1, V3d p2)
            => (p1 - p0).Cross(p2 - p0).AllTiny;

        /// <summary>
        /// Returns whether the given triangle is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(Triangle3d t)
            => IsDegenerated(t.P0, t.P1, t.P2);

        #endregion

        #region Normal

        /// <summary>
        /// Returns the normal of the triangle defined by the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d Normal(V3d p0, V3d p1, V3d p2)
            => (p1 - p0).Cross(p2 - p0).Normalized;

        /// <summary>
        /// Returns the normal of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d Normal(Triangle3d t)
            => Normal(t.P0, t.P1, t.P2);

        #endregion

        #region Plane

        /// <summary>
        /// Returns the plane that contains the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane3d Plane(V3d p0, V3d p1, V3d p2)
            => new Plane3d(Normal(p0, p1, p2), p0);

        /// <summary>
        /// Returns the plane that contains the points of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane3d Plane(Triangle3d t)
            => Plane(t.P0, t.P1, t.P2);

        #endregion

        #region SolidAngle

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SolidAngle(Triangle3d t)
            => SolidAngle(t.P0, t.P1, t.P2);

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the origin.
        ///
        /// Van Oosterom, A., and Strackee, J. (1983). 
        /// The solid angle of a plane triangle.
        /// IEEE transactions on Biomedical Engineering, (2), 125-126.
        /// 
        /// https://en.wikipedia.org/wiki/Solid_angle#Tetrahedron
        /// </summary>
        public static double SolidAngle(V3d va, V3d vb, V3d vc)
        {
            var ma = va.Length;
            var mb = vb.Length;
            var mc = vc.Length;

            var numerator = Fun.Abs(Vec.Dot(va, (Vec.Cross(vb, vc))));

            var denom = ma * mb * mc + Vec.Dot(va, vb) * mc + Vec.Dot(va, vc) * mb + Vec.Dot(vb, vc) * ma;

            var halfSA = Fun.Atan2(numerator, denom);

            return 2 * (halfSA >= 0 ? halfSA : halfSA + Constant.Pi);
        }

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the given point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SolidAngle(V3d va, V3d vb, V3d vc, V3d p)
            => SolidAngle(va - p, vb - p, vc - p);

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the given point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SolidAngle(Triangle3d t, V3d p)
            => SolidAngle(t.P0 - p, t.P1 - p, t.P2 - p);

        #endregion

        #region Distance

        /// <summary>
        /// Gets the distance between the closest point on the triangle [a, b, c] and the given query point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(V3d a, V3d b, V3d c, V3d query)
        {
            var cp = query.GetClosestPointOnTriangle(a, b, c);
            return (cp - query).Length;
        }

        #endregion
    }

    #endregion

}

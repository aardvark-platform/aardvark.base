using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Triangle2f

    /// <summary>
    /// A two-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct Triangle2f : IBoundingCircle2f
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
        /// Returns a negative value if the triangle has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        public readonly float WindingOrder
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.WindingOrder(this);
        }

        #endregion

        #region CircumCircle

        public readonly Circle2f CircumCircle
        {
            get
            {
                ComputeCircumCircleSquared(P0, P1, P2, out V2f center, out float radiusSquared);
                return new Circle2f(center, radiusSquared.Sqrt());
            }
        }

        public readonly Circle2f CircumCircleSquared
        {
            get
            {
                ComputeCircumCircleSquared(P0, P1, P2, out V2f center, out float radiusSquared);
                return new Circle2f(center, radiusSquared);
            }
        }

        public static void ComputeCircumCircleSquared(
            V2f p0, V2f p1, V2f p2,
            out V2f center, out float radiusSquared)
        {
            float y01abs = Fun.Abs(p0.Y - p1.Y);
            float y12abs = Fun.Abs(p1.Y - p2.Y);

            if (y01abs < Constant<float>.PositiveTinyValue
                && y12abs < Constant<float>.PositiveTinyValue)
            {
                center = V2f.NaN; radiusSquared = -1; return;
            }

            float xc, yc;

            if (y01abs < Constant<float>.PositiveTinyValue)
            {
                float m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                float m12x = 0.5f * (p1.X + p2.X);
                float m12y = 0.5f * (p1.Y + p2.Y);
                xc = 0.5f * (p1.X + p0.X);
                yc = m2 * (xc - m12x) + m12y;
            }
            else if (y12abs < Constant<float>.PositiveTinyValue)
            {
                float m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                float m01x = 0.5f * (p0.X + p1.X);
                float m01y = 0.5f * (p0.Y + p1.Y);
                xc = 0.5f * (p2.X + p1.X);
                yc = m1 * (xc - m01x) + m01y;
            }
            else
            {
                float m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                float m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                float m01x = 0.5f * (p0.X + p1.X);
                float m01y = 0.5f * (p0.Y + p1.Y);
                float m12x = 0.5f * (p1.X + p2.X);
                float m12y = 0.5f * (p1.Y + p2.Y);
                float m12 = m1 - m2;
                if (Fun.Abs(m12) < Constant<float>.PositiveTinyValue)
                {
                    center = V2f.NaN; radiusSquared = -1; return;
                }
                xc = (m1 * m01x - m2 * m12x + m12y - m01y) / m12;
                if (y01abs > y12abs)
                {
                    yc = m1 * (xc - m01x) + m01y;
                }
                else
                {
                    yc = m2 * (xc - m12x) + m12y;
                }
            }
            center = new V2f(xc, yc);
            radiusSquared = Vec.DistanceSquared(p0, center);
        }

        #endregion

        #region IBoundingCircle2f Members

        /// <summary>
        /// Returns the smallest enclosing circle, using a longest-edge diameter for right,
        /// obtuse or collinear triangles and zero radius for coincident points.
        /// Non-finite points return Invalid. The radius accounts for rounding of the center.
        /// </summary>
        public readonly Circle2f BoundingCircle2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = P1 - P0; var b = P2 - P0; var c = P2 - P1;
                var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
                var sum = aa + bb + cc;
                if (!(sum >= 1e-10f && sum <= 1e10f)) return BoundingCircleScaled();
                var origin = P0; var d = a; var e = b; var f = c;
                bool useE = bb <= cc;
                if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
                else if (cc > aa && cc > bb) { origin = P1; d = c; e = -a; f = -b; useE = aa <= bb; }
                var offset = 0.5f * d;
                var dot = e.Dot(f);
                if (dot > 0)
                {
                    var relative = useE ? e : f;
                    var area = d.X * relative.Y - d.Y * relative.X;
                    var t = dot / (2 * area);
                    offset += new V2f(-d.Y * t, d.X * t);
                }
                var center = origin + offset;
                var r2 = Fun.Max((center - P0).LengthSquared, (center - P1).LengthSquared, (center - P2).LengthSquared);
                return new Circle2f(center, r2.Sqrt());
            }
        }

        private readonly Circle2f BoundingCircleScaled()
        {
            var p0 = (V2d)P0; var p1 = (V2d)P1; var p2 = (V2d)P2;
            if (!p0.IsFinite || !p1.IsFinite || !p2.IsFinite) return Circle2f.Invalid;
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
                if (scale == 0) return new Circle2f(P0, 0);
                a = DivideForBounds(a, scale); b = DivideForBounds(b, scale); c = DivideForBounds(c, scale);
            }
            var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
            var origin = p0; var d = a; var e = b; var f = c;
            bool useE = bb <= cc;
            if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
            else if (cc > aa && cc > bb) { origin = p1; d = c; e = -a; f = -b; useE = aa <= bb; }
            var offset = 0.5 * d;
            double dot = e.Dot(f);
            var relative = useE ? e : f;
            double area = d.X * relative.Y - d.Y * relative.X;
            if (dot > 0 && area != 0)
            {
                double t = dot / (2 * area);
                offset += new V2d(-d.Y * t, d.X * t);
            }
            var center = scaledPoints ? (origin + offset) * scale : origin + offset * scale;
            // The minimum center lies in the convex hull; clamp only reconstruction roundoff.
            center.X = Fun.Clamp(center.X, Fun.Min(P0.X, P1.X, P2.X), Fun.Max(P0.X, P1.X, P2.X));
            center.Y = Fun.Clamp(center.Y, Fun.Min(P0.Y, P1.Y, P2.Y), Fun.Max(P0.Y, P1.Y, P2.Y));
            var rounded = (V2f)center;
            double radius = Fun.Max(BoundsDistance((V2d)rounded, (V2d)P0), BoundsDistance((V2d)rounded, (V2d)P1), BoundsDistance((V2d)rounded, (V2d)P2));
            return new Circle2f(rounded, (float)radius);
        }

        private static V2d DivideForBounds(V2d p, double scale)
            => new V2d(p.X / scale, p.Y / scale);

        private static double BoundsDistance(V2d a, V2d b)
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
        public static float Area(V2f p0, V2f p1, V2f p2)
            => WindingOrder(p0, p1, p2).Abs() * 0.5f;

        /// <summary>
        /// Returns the area of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Area(Triangle2f t)
            => Area(t.P0, t.P1, t.P2);

        #endregion

        #region IsDegenerated

        /// <summary>
        /// Returns whether the triangle defined by the give points is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(V2f p0, V2f p1, V2f p2)
            => WindingOrder(p0, p1, p2).IsTiny();

        /// <summary>
        /// Returns whether the given triangle is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(Triangle2f t)
            => WindingOrder(t).IsTiny();

        #endregion

        #region WindingOrder

        /// <summary>
        /// Returns a negative value if the triangle defined by the given points has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float WindingOrder(V2f p0, V2f p1, V2f p2)
            => (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);

        /// <summary>
        /// Returns a negative value if the given triangle has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float WindingOrder(Triangle2f t)
            => WindingOrder(t.P0, t.P1, t.P2);

        #endregion

        #region Distance

        /// <summary>
        /// Gets the distance between the closest point on the triangle [a, b, c] and the given query point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(V2f a, V2f b, V2f c, V2f query)
        {
            var cp = query.GetClosestPointOnTriangle(a, b, c);
            return (cp - query).Length;
        }

        #endregion
    }

    #endregion

    #region Triangle2d

    /// <summary>
    /// A two-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct Triangle2d : IBoundingCircle2d
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
        /// Returns a negative value if the triangle has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        public readonly double WindingOrder
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.WindingOrder(this);
        }

        #endregion

        #region CircumCircle

        public readonly Circle2d CircumCircle
        {
            get
            {
                ComputeCircumCircleSquared(P0, P1, P2, out V2d center, out double radiusSquared);
                return new Circle2d(center, radiusSquared.Sqrt());
            }
        }

        public readonly Circle2d CircumCircleSquared
        {
            get
            {
                ComputeCircumCircleSquared(P0, P1, P2, out V2d center, out double radiusSquared);
                return new Circle2d(center, radiusSquared);
            }
        }

        public static void ComputeCircumCircleSquared(
            V2d p0, V2d p1, V2d p2,
            out V2d center, out double radiusSquared)
        {
            double y01abs = Fun.Abs(p0.Y - p1.Y);
            double y12abs = Fun.Abs(p1.Y - p2.Y);

            if (y01abs < Constant<double>.PositiveTinyValue
                && y12abs < Constant<double>.PositiveTinyValue)
            {
                center = V2d.NaN; radiusSquared = -1; return;
            }

            double xc, yc;

            if (y01abs < Constant<double>.PositiveTinyValue)
            {
                double m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                double m12x = 0.5 * (p1.X + p2.X);
                double m12y = 0.5 * (p1.Y + p2.Y);
                xc = 0.5 * (p1.X + p0.X);
                yc = m2 * (xc - m12x) + m12y;
            }
            else if (y12abs < Constant<double>.PositiveTinyValue)
            {
                double m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                double m01x = 0.5 * (p0.X + p1.X);
                double m01y = 0.5 * (p0.Y + p1.Y);
                xc = 0.5 * (p2.X + p1.X);
                yc = m1 * (xc - m01x) + m01y;
            }
            else
            {
                double m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                double m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                double m01x = 0.5 * (p0.X + p1.X);
                double m01y = 0.5 * (p0.Y + p1.Y);
                double m12x = 0.5 * (p1.X + p2.X);
                double m12y = 0.5 * (p1.Y + p2.Y);
                double m12 = m1 - m2;
                if (Fun.Abs(m12) < Constant<double>.PositiveTinyValue)
                {
                    center = V2d.NaN; radiusSquared = -1; return;
                }
                xc = (m1 * m01x - m2 * m12x + m12y - m01y) / m12;
                if (y01abs > y12abs)
                {
                    yc = m1 * (xc - m01x) + m01y;
                }
                else
                {
                    yc = m2 * (xc - m12x) + m12y;
                }
            }
            center = new V2d(xc, yc);
            radiusSquared = Vec.DistanceSquared(p0, center);
        }

        #endregion

        #region IBoundingCircle2d Members

        /// <summary>
        /// Returns the smallest enclosing circle, using a longest-edge diameter for right,
        /// obtuse or collinear triangles and zero radius for coincident points.
        /// Non-finite points return Invalid. The radius accounts for rounding of the center.
        /// </summary>
        public readonly Circle2d BoundingCircle2d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = P1 - P0; var b = P2 - P0; var c = P2 - P1;
                var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
                var sum = aa + bb + cc;
                if (!(sum >= 1e-100 && sum <= 1e100)) return BoundingCircleScaled();
                var origin = P0; var d = a; var e = b; var f = c;
                bool useE = bb <= cc;
                if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
                else if (cc > aa && cc > bb) { origin = P1; d = c; e = -a; f = -b; useE = aa <= bb; }
                var offset = 0.5 * d;
                var dot = e.Dot(f);
                if (dot > 0)
                {
                    var relative = useE ? e : f;
                    var area = d.X * relative.Y - d.Y * relative.X;
                    var t = dot / (2 * area);
                    offset += new V2d(-d.Y * t, d.X * t);
                }
                var center = origin + offset;
                var r2 = Fun.Max((center - P0).LengthSquared, (center - P1).LengthSquared, (center - P2).LengthSquared);
                return new Circle2d(center, r2.Sqrt());
            }
        }

        private readonly Circle2d BoundingCircleScaled()
        {
            var p0 = (V2d)P0; var p1 = (V2d)P1; var p2 = (V2d)P2;
            if (!p0.IsFinite || !p1.IsFinite || !p2.IsFinite) return Circle2d.Invalid;
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
                if (scale == 0) return new Circle2d(P0, 0);
                a = DivideForBounds(a, scale); b = DivideForBounds(b, scale); c = DivideForBounds(c, scale);
            }
            var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
            var origin = p0; var d = a; var e = b; var f = c;
            bool useE = bb <= cc;
            if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
            else if (cc > aa && cc > bb) { origin = p1; d = c; e = -a; f = -b; useE = aa <= bb; }
            var offset = 0.5 * d;
            double dot = e.Dot(f);
            var relative = useE ? e : f;
            double area = d.X * relative.Y - d.Y * relative.X;
            if (dot > 0 && area != 0)
            {
                double t = dot / (2 * area);
                offset += new V2d(-d.Y * t, d.X * t);
            }
            var center = scaledPoints ? (origin + offset) * scale : origin + offset * scale;
            // The minimum center lies in the convex hull; clamp only reconstruction roundoff.
            center.X = Fun.Clamp(center.X, Fun.Min(P0.X, P1.X, P2.X), Fun.Max(P0.X, P1.X, P2.X));
            center.Y = Fun.Clamp(center.Y, Fun.Min(P0.Y, P1.Y, P2.Y), Fun.Max(P0.Y, P1.Y, P2.Y));
            var rounded = (V2d)center;
            double radius = Fun.Max(BoundsDistance((V2d)rounded, (V2d)P0), BoundsDistance((V2d)rounded, (V2d)P1), BoundsDistance((V2d)rounded, (V2d)P2));
            return new Circle2d(rounded, (double)radius);
        }

        private static V2d DivideForBounds(V2d p, double scale)
            => new V2d(p.X / scale, p.Y / scale);

        private static double BoundsDistance(V2d a, V2d b)
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
        public static double Area(V2d p0, V2d p1, V2d p2)
            => WindingOrder(p0, p1, p2).Abs() * 0.5;

        /// <summary>
        /// Returns the area of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Area(Triangle2d t)
            => Area(t.P0, t.P1, t.P2);

        #endregion

        #region IsDegenerated

        /// <summary>
        /// Returns whether the triangle defined by the give points is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(V2d p0, V2d p1, V2d p2)
            => WindingOrder(p0, p1, p2).IsTiny();

        /// <summary>
        /// Returns whether the given triangle is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(Triangle2d t)
            => WindingOrder(t).IsTiny();

        #endregion

        #region WindingOrder

        /// <summary>
        /// Returns a negative value if the triangle defined by the given points has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double WindingOrder(V2d p0, V2d p1, V2d p2)
            => (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);

        /// <summary>
        /// Returns a negative value if the given triangle has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double WindingOrder(Triangle2d t)
            => WindingOrder(t.P0, t.P1, t.P2);

        #endregion

        #region Distance

        /// <summary>
        /// Gets the distance between the closest point on the triangle [a, b, c] and the given query point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(V2d a, V2d b, V2d c, V2d query)
        {
            var cp = query.GetClosestPointOnTriangle(a, b, c);
            return (cp - query).Length;
        }

        #endregion
    }

    #endregion

}

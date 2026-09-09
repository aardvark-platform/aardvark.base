using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Triangle2" + tc;
    //#   var type2 = "Triangle2" + tc2;
    //#   var v2t = "V2" + tc;
    //#   var box2t = "Box2" + tc;
    //#   var plane2t = "Plane2" + tc;
    //#   var circle2t = "Circle2" + tc;
    //#   var iboundingcircle2t = "IBoundingCircle2" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#   var boundsLow = isDouble ? "1e-100" : "1e-10f";
    //#   var boundsHigh = isDouble ? "1e100" : "1e10f";
    #region __type__

    /// <summary>
    /// A two-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct __type__ : __iboundingcircle2t__
    {
        #region Geometric Properties

        /// <summary>
        /// Returns the area of the triangle.
        /// </summary>
        public readonly __ftype__ Area
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
        public readonly __ftype__ WindingOrder
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.WindingOrder(this);
        }

        #endregion

        #region CircumCircle

        public readonly __circle2t__ CircumCircle
        {
            get
            {
                ComputeCircumCircleSquared(P0, P1, P2, out __v2t__ center, out __ftype__ radiusSquared);
                return new __circle2t__(center, radiusSquared.Sqrt());
            }
        }

        public readonly __circle2t__ CircumCircleSquared
        {
            get
            {
                ComputeCircumCircleSquared(P0, P1, P2, out __v2t__ center, out __ftype__ radiusSquared);
                return new __circle2t__(center, radiusSquared);
            }
        }

        public static void ComputeCircumCircleSquared(
            __v2t__ p0, __v2t__ p1, __v2t__ p2,
            out __v2t__ center, out __ftype__ radiusSquared)
        {
            __ftype__ y01abs = Fun.Abs(p0.Y - p1.Y);
            __ftype__ y12abs = Fun.Abs(p1.Y - p2.Y);

            if (y01abs < Constant<__ftype__>.PositiveTinyValue
                && y12abs < Constant<__ftype__>.PositiveTinyValue)
            {
                center = __v2t__.NaN; radiusSquared = -1; return;
            }

            __ftype__ xc, yc;

            if (y01abs < Constant<__ftype__>.PositiveTinyValue)
            {
                __ftype__ m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                __ftype__ m12x = __half__ * (p1.X + p2.X);
                __ftype__ m12y = __half__ * (p1.Y + p2.Y);
                xc = __half__ * (p1.X + p0.X);
                yc = m2 * (xc - m12x) + m12y;
            }
            else if (y12abs < Constant<__ftype__>.PositiveTinyValue)
            {
                __ftype__ m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                __ftype__ m01x = __half__ * (p0.X + p1.X);
                __ftype__ m01y = __half__ * (p0.Y + p1.Y);
                xc = __half__ * (p2.X + p1.X);
                yc = m1 * (xc - m01x) + m01y;
            }
            else
            {
                __ftype__ m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                __ftype__ m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                __ftype__ m01x = __half__ * (p0.X + p1.X);
                __ftype__ m01y = __half__ * (p0.Y + p1.Y);
                __ftype__ m12x = __half__ * (p1.X + p2.X);
                __ftype__ m12y = __half__ * (p1.Y + p2.Y);
                __ftype__ m12 = m1 - m2;
                if (Fun.Abs(m12) < Constant<__ftype__>.PositiveTinyValue)
                {
                    center = __v2t__.NaN; radiusSquared = -1; return;
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
            center = new __v2t__(xc, yc);
            radiusSquared = Vec.DistanceSquared(p0, center);
        }

        #endregion

        #region __iboundingcircle2t__ Members

        /// <summary>
        /// Returns the smallest enclosing circle, using a longest-edge diameter for right,
        /// obtuse or collinear triangles and zero radius for coincident points.
        /// Non-finite points return Invalid. The radius accounts for rounding of the center.
        /// </summary>
        public readonly __circle2t__ BoundingCircle2__tc__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = P1 - P0; var b = P2 - P0; var c = P2 - P1;
                var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
                var sum = aa + bb + cc;
                if (!(sum >= __boundsLow__ && sum <= __boundsHigh__)) return BoundingCircleScaled();
                var origin = P0; var d = a; var e = b; var f = c;
                bool useE = bb <= cc;
                if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
                else if (cc > aa && cc > bb) { origin = P1; d = c; e = -a; f = -b; useE = aa <= bb; }
                var offset = __half__ * d;
                var dot = e.Dot(f);
                if (dot > 0)
                {
                    var relative = useE ? e : f;
                    var area = d.X * relative.Y - d.Y * relative.X;
                    var t = dot / (2 * area);
                    offset += new __v2t__(-d.Y * t, d.X * t);
                }
                var center = origin + offset;
                var r2 = Fun.Max((center - P0).LengthSquared, (center - P1).LengthSquared, (center - P2).LengthSquared);
                return new __circle2t__(center, r2.Sqrt());
            }
        }

        private readonly __circle2t__ BoundingCircleScaled()
        {
            var p0 = (V2d)P0; var p1 = (V2d)P1; var p2 = (V2d)P2;
            if (!p0.IsFinite || !p1.IsFinite || !p2.IsFinite) return __circle2t__.Invalid;
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
                if (scale == 0) return new __circle2t__(P0, 0);
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
            var rounded = (__v2t__)center;
            double radius = Fun.Max(BoundsDistance((V2d)rounded, (V2d)P0), BoundsDistance((V2d)rounded, (V2d)P1), BoundsDistance((V2d)rounded, (V2d)P2));
            return new __circle2t__(rounded, (__ftype__)radius);
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
        public static __ftype__ Area(__v2t__ p0, __v2t__ p1, __v2t__ p2)
            => WindingOrder(p0, p1, p2).Abs() * __half__;

        /// <summary>
        /// Returns the area of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Area(__type__ t)
            => Area(t.P0, t.P1, t.P2);

        #endregion

        #region IsDegenerated

        /// <summary>
        /// Returns whether the triangle defined by the give points is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(__v2t__ p0, __v2t__ p1, __v2t__ p2)
            => WindingOrder(p0, p1, p2).IsTiny();

        /// <summary>
        /// Returns whether the given triangle is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(__type__ t)
            => WindingOrder(t).IsTiny();

        #endregion

        #region WindingOrder

        /// <summary>
        /// Returns a negative value if the triangle defined by the given points has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ WindingOrder(__v2t__ p0, __v2t__ p1, __v2t__ p2)
            => (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);

        /// <summary>
        /// Returns a negative value if the given triangle has a
        /// counter-clockwise winding order, and a positive value if it has a clockwise winding-order.
        /// The magnitude is twice the area of the triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ WindingOrder(__type__ t)
            => WindingOrder(t.P0, t.P1, t.P2);

        #endregion

        #region Distance

        /// <summary>
        /// Gets the distance between the closest point on the triangle [a, b, c] and the given query point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Distance(__v2t__ a, __v2t__ b, __v2t__ c, __v2t__ query)
        {
            var cp = query.GetClosestPointOnTriangle(a, b, c);
            return (cp - query).Length;
        }

        #endregion
    }

    #endregion

    //# }
}

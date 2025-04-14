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
    //#   var eps = isDouble ? "1e-6" : "1e-4f";
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

        public readonly __circle2t__ BoundingCircle2__tc__
        {
            get
            {
                var edge01 = P1 - P0;
                var edge02 = P2 - P0;
                __ftype__ dot0101 = Vec.Dot(edge01, edge01);
                __ftype__ dot0102 = Vec.Dot(edge01, edge02);
                __ftype__ dot0202 = Vec.Dot(edge02, edge02);
                __ftype__ d = 2 * (dot0101 * dot0202 - dot0102 * dot0102);
                if (d.Abs() <= __eps__) return __circle2t__.Invalid;
                __ftype__ s = (dot0101 * dot0202 - dot0202 * dot0102) / d;
                __ftype__ t = (dot0202 * dot0101 - dot0101 * dot0102) / d;
                var p = P0;
                var cir = new __circle2t__();
                if (s <= 0)
                    cir.Center = __half__ * (P0 + P2);
                else if (t <= 0)
                    cir.Center = __half__ * (P0 + P1);
                else if (s + t >= 1)
                {
                    cir.Center = __half__ * (P1 + P2);
                    p = P1;
                }
                else
                    cir.Center = P0 + s * edge01 + t * edge02;
                cir.Radius = (cir.Center - p).Length;
                return cir;
            }
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

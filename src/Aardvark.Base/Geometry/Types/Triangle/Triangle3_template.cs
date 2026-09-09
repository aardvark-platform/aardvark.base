using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Triangle3" + tc;
    //#   var type2 = "Triangle3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var sphere3t = "Sphere3" + tc;
    //#   var iboundingsphere3t = "IBoundingSphere3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#   var boundsLow = isDouble ? "1e-100" : "1e-10f";
    //#   var boundsHigh = isDouble ? "1e100" : "1e10f";
    #region __type__

    /// <summary>
    /// A three-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct __type__ : __iboundingsphere3t__
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
        /// Returns the normal of the triangle.
        /// </summary>
        public readonly __v3t__ Normal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Normal(this);
        }

        /// <summary>
        /// Returns the plane that contains the points of the triangle.
        /// </summary>
        public readonly __plane3t__ Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Plane(this);
        }

        #endregion

        #region __iboundingsphere3t__ Members

        /// <summary>
        /// Returns the smallest enclosing sphere, using a longest-edge diameter for right,
        /// obtuse or collinear triangles and zero radius for coincident points.
        /// Non-finite points return Invalid. The radius accounts for rounding of the center.
        /// </summary>
        public readonly __sphere3t__ BoundingSphere3__tc__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = P1 - P0; var b = P2 - P0; var c = P2 - P1;
                var aa = a.LengthSquared; var bb = b.LengthSquared; var cc = c.LengthSquared;
                var sum = aa + bb + cc;
                if (!(sum >= __boundsLow__ && sum <= __boundsHigh__)) return BoundingSphereScaled();
                var origin = P0; var d = a; var e = b; var f = c;
                bool useE = bb <= cc;
                if (bb > aa && bb >= cc) { d = b; e = a; f = -c; useE = aa <= cc; }
                else if (cc > aa && cc > bb) { origin = P1; d = c; e = -a; f = -b; useE = aa <= bb; }
                var offset = __half__ * d;
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
                return new __sphere3t__(center, r2.Sqrt());
            }
        }

        private readonly __sphere3t__ BoundingSphereScaled()
        {
            var p0 = (V3d)P0; var p1 = (V3d)P1; var p2 = (V3d)P2;
            if (!p0.IsFinite || !p1.IsFinite || !p2.IsFinite) return __sphere3t__.Invalid;
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
                if (scale == 0) return new __sphere3t__(P0, 0);
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
            var rounded = (__v3t__)center;
            double radius = Fun.Max(BoundsDistance((V3d)rounded, (V3d)P0), BoundsDistance((V3d)rounded, (V3d)P1), BoundsDistance((V3d)rounded, (V3d)P2));
            return new __sphere3t__(rounded, (__ftype__)radius);
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
        public static __ftype__ Area(__v3t__ p0, __v3t__ p1, __v3t__ p2)
            => (p1 - p0).Cross(p2 - p0).Length * __half__;

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
        public static bool IsDegenerated(__v3t__ p0, __v3t__ p1, __v3t__ p2)
            => (p1 - p0).Cross(p2 - p0).AllTiny;

        /// <summary>
        /// Returns whether the given triangle is degenerated, i.e. its area is zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDegenerated(__type__ t)
            => IsDegenerated(t.P0, t.P1, t.P2);

        #endregion

        #region Normal

        /// <summary>
        /// Returns the normal of the triangle defined by the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ Normal(__v3t__ p0, __v3t__ p1, __v3t__ p2)
            => (p1 - p0).Cross(p2 - p0).Normalized;

        /// <summary>
        /// Returns the normal of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ Normal(__type__ t)
            => Normal(t.P0, t.P1, t.P2);

        #endregion

        #region Plane

        /// <summary>
        /// Returns the plane that contains the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __plane3t__ Plane(__v3t__ p0, __v3t__ p1, __v3t__ p2)
            => new __plane3t__(Normal(p0, p1, p2), p0);

        /// <summary>
        /// Returns the plane that contains the points of the given triangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __plane3t__ Plane(__type__ t)
            => Plane(t.P0, t.P1, t.P2);

        #endregion

        #region SolidAngle

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ SolidAngle(__type__ t)
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
        public static __ftype__ SolidAngle(__v3t__ va, __v3t__ vb, __v3t__ vc)
        {
            var ma = va.Length;
            var mb = vb.Length;
            var mc = vc.Length;

            var numerator = Fun.Abs(Vec.Dot(va, (Vec.Cross(vb, vc))));

            var denom = ma * mb * mc + Vec.Dot(va, vb) * mc + Vec.Dot(va, vc) * mb + Vec.Dot(vb, vc) * ma;

            var halfSA = Fun.Atan2(numerator, denom);

            return 2 * (halfSA >= 0 ? halfSA : halfSA + __pi__);
        }

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the given point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ SolidAngle(__v3t__ va, __v3t__ vb, __v3t__ vc, __v3t__ p)
            => SolidAngle(va - p, vb - p, vc - p);

        /// <summary>
        /// Computes the solid angle for a planar triangle as seen from the given point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ SolidAngle(__type__ t, __v3t__ p)
            => SolidAngle(t.P0 - p, t.P1 - p, t.P2 - p);

        #endregion

        #region Distance

        /// <summary>
        /// Gets the distance between the closest point on the triangle [a, b, c] and the given query point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Distance(__v3t__ a, __v3t__ b, __v3t__ c, __v3t__ query)
        {
            var cp = query.GetClosestPointOnTriangle(a, b, c);
            return (cp - query).Length;
        }

        #endregion
    }

    #endregion

    //# }
}

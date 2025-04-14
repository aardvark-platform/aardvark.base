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
        /// Returns the bounding sphere of the triangle.
        /// </summary>
        public readonly Sphere3f BoundingSphere3f
        {
            get
            {
                var edge01 = Edge01;
                var edge02 = Edge02;
                float dot0101 = Vec.Dot(edge01, edge01);
                float dot0102 = Vec.Dot(edge01, edge02);
                float dot0202 = Vec.Dot(edge02, edge02);
                float d = 2 * (dot0101 * dot0202 - dot0102 * dot0102);
                if (d.Abs() <= 1e-5f) return Sphere3f.Invalid;
                float s = (dot0101 * dot0202 - dot0202 * dot0102) / d;
                float t = (dot0202 * dot0101 - dot0101 * dot0102) / d;
                var p = P0;
                var sph = new Sphere3f();
                if (s <= 0)
                    sph.Center = 0.5f * (P0 + P2);
                else if (t <= 0)
                    sph.Center = 0.5f * (P0 + P1);
                else if (s + t >= 1)
                {
                    sph.Center = 0.5f * (P1 + P2);
                    p = P1;
                }
                else
                    sph.Center = P0 + s * edge01 + t * edge02;
                sph.Radius = (sph.Center - p).Length;
                return sph;
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
        /// Returns the bounding sphere of the triangle.
        /// </summary>
        public readonly Sphere3d BoundingSphere3d
        {
            get
            {
                var edge01 = Edge01;
                var edge02 = Edge02;
                double dot0101 = Vec.Dot(edge01, edge01);
                double dot0102 = Vec.Dot(edge01, edge02);
                double dot0202 = Vec.Dot(edge02, edge02);
                double d = 2 * (dot0101 * dot0202 - dot0102 * dot0102);
                if (d.Abs() <= 1e-9) return Sphere3d.Invalid;
                double s = (dot0101 * dot0202 - dot0202 * dot0102) / d;
                double t = (dot0202 * dot0101 - dot0101 * dot0102) / d;
                var p = P0;
                var sph = new Sphere3d();
                if (s <= 0)
                    sph.Center = 0.5 * (P0 + P2);
                else if (t <= 0)
                    sph.Center = 0.5 * (P0 + P1);
                else if (s + t >= 1)
                {
                    sph.Center = 0.5 * (P1 + P2);
                    p = P1;
                }
                else
                    sph.Center = P0 + s * edge01 + t * edge02;
                sph.Radius = (sph.Center - p).Length;
                return sph;
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
    }

    #endregion

}

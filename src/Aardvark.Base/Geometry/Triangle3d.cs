using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// A three-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct Triangle3d : IBoundingSphere3d
    {
        #region Geometric Properties

        /// <summary>
        /// Returns the area of the triangle.
        /// </summary>
        public double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Area(this);
        }

        /// <summary>
        /// Returns whether the triangle is degenerated, i.e. its area is zero.
        /// </summary>
        public bool IsDegenerated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.IsDegenerated(this);
        }

        /// <summary>
        /// Returns the normal of the triangle.
        /// </summary>
        public V3d Normal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Normal(this);
        }

        /// <summary>
        /// Returns the plane that contains the points of the triangle.
        /// </summary>
        public Plane3d Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Triangle.Plane(this);
        }

        #endregion

        #region IBoundingSphere3d Members

        /// <summary>
        /// Returns the bounding sphere of the triangle.
        /// </summary>
        public Sphere3d BoundingSphere3d
        {
            get
            {
                var edge01 = Edge01;
                var edge02 = Edge02;
                double dot0101 = Vec.Dot(edge01, edge01);
                double dot0102 = Vec.Dot(edge01, edge02);
                double dot0202 = Vec.Dot(edge02, edge02);
                double d = 2.0 * (dot0101 * dot0202 - dot0102 * dot0102);
                if (d.Abs() <= 0.000000001) return Sphere3d.Invalid;
                double s = (dot0101 * dot0202 - dot0202 * dot0102) / d;
                double t = (dot0202 * dot0101 - dot0101 * dot0102) / d;
                var p = P0;
                var sph = new Sphere3d();
                if (s <= 0.0)
                    sph.Center = 0.5 * (P0 + P2);
                else if (t <= 0.0)
                    sph.Center = 0.5 * (P0 + P1);
                else if (s + t >= 1.0)
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
    }
}

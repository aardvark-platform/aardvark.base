namespace Aardvark.Base
{
    /// <summary>
    /// A three-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct Triangle3d : IBoundingSphere3d
    {
        #region Geometric Properties

        public double Area => (P1 - P0).Cross(P2 - P0).Length * 0.5;

        public bool IsDegenerated => (P1 - P0).Cross(P2 - P0).AllTiny; // area == 0 => degenerated 

        public V3d Normal => (P1 - P0).Cross(P2 - P0).Normalized;

        public Plane3d Plane => new Plane3d(Normal, P0);

        #endregion

        #region IBoundingSphere3d Members

        public Sphere3d BoundingSphere3d
        {
            get
            {
                var edge01 = Edge01;
                var edge02 = Edge02;
                double dot0101 = V3d.Dot(edge01, edge01);
                double dot0102 = V3d.Dot(edge01, edge02);
                double dot0202 = V3d.Dot(edge02, edge02);
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
}

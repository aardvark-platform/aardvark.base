
namespace Aardvark.Base
{
    /// <summary>
    /// A three-dimensional quadrangle specified by its four points.
    /// The points are not required to lie on the same plane.
    /// </summary>
    public partial struct Quad3d
    {
        #region Geometric Properties

        /// <summary>
        /// Returns the area when projected onto a plane normal to the area
        /// weighted average normal of the triangles (P0,P1,P2) and (P0,P2,P3).
        /// </summary>
        public double Area
        {
            get
            {
                var v02 = P2 - P0;
                return 0.5 * ((P1 - P0).Cross(v02) + v02.Cross(P3 - P0)).Length;
            }
        }

        public bool IsDegenerated
        {
            get
            {
                V3d v01 = P1 - P0; if (v01.AllTiny) return true;
                V3d v03 = P3 - P0; if (v03.AllTiny) return true;
                if ((P2 - P1).AllTiny || (P3 - P2).AllTiny) return true;
                var v02 = P2 - P0;
                return (v01.Cross(v02) + v02.Cross(v03)).AllTiny;
            }
        }

        public V3d Normal
        {
            get
            {
                var v02 = P2 - P0;
                return ((P1 - P0).Cross(v02) + v02.Cross(P3 - P0)).Normalized;
            }
        }

        #endregion
    }
}

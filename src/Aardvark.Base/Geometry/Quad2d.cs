
namespace Aardvark.Base
{
    /// <summary>
    /// A two-dimensional quadrangle specified by its four points.
    /// </summary>
    public partial struct Quad2d
    {
        #region Geometric Properties

        public double Area
        {
            get
            {
                return 0.5 * ((P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y)).Abs();
            }
        }

        public bool IsDegenerated
        {
            get
            {
                return (P1 - P0).AllTiny || (P2 - P1).AllTiny
                    || (P3 - P2).AllTiny || (P0 - P3).AllTiny
                    || WindingOrder.IsTiny();
            }
        }

        /// <summary>
        /// Returns a value less than zero for ccw and greater than zero for cw.
        /// The magnitude magnitude is twice the area.
        /// </summary>
        public double WindingOrder
        {
            get
            {
                return (P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y);
            }
        }

        #endregion
    }
}

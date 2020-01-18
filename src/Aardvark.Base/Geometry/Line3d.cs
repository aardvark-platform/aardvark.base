using System;

namespace Aardvark.Base
{
    /// <summary>
    /// A three-dimensional line with specified start and end points.
    /// </summary>
    [Serializable]
    public partial struct Line3d : IBoundingSphere3d
    {
        #region Geometric Properties

        /// <summary>
        /// P0
        /// </summary>
        public V3d Origin
        {
            get { return P0; }
            set { P0 = value; }
        }

        /// <summary>
        /// P1 - P0
        /// </summary>
        public V3d Direction
        {
            get { return P1 - P0; }
            set { P1 = P0 + value; }
        }

        public Ray3d Ray3d => new Ray3d(P0, P1 - P0);

        public bool IsDegenerated => !Direction.Abs().AnyGreater(Constant<double>.PositiveTinyValue);

        #endregion

        #region IBoundingSphere3d Members

        public Sphere3d BoundingSphere3d => new Sphere3d(this.ComputeCentroid(), 0.5 * Direction.Length);

        #endregion

        public Line3d Flipped => new Line3d(P1, P0);
    }
}

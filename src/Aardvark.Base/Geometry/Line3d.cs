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

        public V3d Origin
        {
            get { return P0; }
            set { P0 = value; }
        }

        public V3d Direction
        {
            get { return P1 - P0; }
            set { P1 = P0 + value; }
        }

        public Ray3d Ray3d
        {
            get { return new Ray3d(P0, P1 - P0); }
        }

        public bool IsDegenerated
        {
            get { return !Direction.Abs.AnyGreater(Constant<double>.PositiveTinyValue); }
        }

        #endregion

        #region IBoundingSphere3d Members

        public Sphere3d BoundingSphere3d
        {
            get { return new Sphere3d(this.ComputeCentroid(), 0.5 * Direction.Length); }
        }

        #endregion
    }
}

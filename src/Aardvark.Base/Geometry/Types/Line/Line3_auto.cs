using System;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Line3f

    /// <summary>
    /// A three-dimensional line with specified start and end points.
    /// </summary>
    [Serializable]
    public partial struct Line3f : IBoundingSphere3f
    {
        #region Geometric Properties

        /// <summary>
        /// P0
        /// </summary>
        [XmlIgnore]
        public V3f Origin
        {
            readonly get { return P0; }
            set { P0 = value; }
        }

        /// <summary>
        /// P1 - P0
        /// </summary>
        [XmlIgnore]
        public V3f Direction
        {
            readonly get { return P1 - P0; }
            set { P1 = P0 + value; }
        }

        public readonly Ray3f Ray3f => new Ray3f(P0, P1 - P0);

        public readonly bool IsDegenerated => !Direction.Abs().AnyGreater(Constant<float>.PositiveTinyValue);

        #endregion

        #region IBoundingSphere3f Members

        public readonly Sphere3f BoundingSphere3f => new Sphere3f(this.ComputeCentroid(), 0.5f * Direction.Length);

        #endregion

        public readonly Line3f Flipped => new Line3f(P1, P0);
    }

    #endregion

    #region Line3d

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
        [XmlIgnore]
        public V3d Origin
        {
            readonly get { return P0; }
            set { P0 = value; }
        }

        /// <summary>
        /// P1 - P0
        /// </summary>
        [XmlIgnore]
        public V3d Direction
        {
            readonly get { return P1 - P0; }
            set { P1 = P0 + value; }
        }

        public readonly Ray3d Ray3d => new Ray3d(P0, P1 - P0);

        public readonly bool IsDegenerated => !Direction.Abs().AnyGreater(Constant<double>.PositiveTinyValue);

        #endregion

        #region IBoundingSphere3d Members

        public readonly Sphere3d BoundingSphere3d => new Sphere3d(this.ComputeCentroid(), 0.5 * Direction.Length);

        #endregion

        public readonly Line3d Flipped => new Line3d(P1, P0);
    }

    #endregion

}

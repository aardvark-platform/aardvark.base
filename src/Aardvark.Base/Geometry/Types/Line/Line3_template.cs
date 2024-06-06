using System;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Line3" + tc;
    //#   var type2 = "Line3" + tc2;
    //#   var v2t = "V2" + tc;
    //#   var ray3t = "Ray3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var sphere3t = "Sphere3" + tc;
    //#   var v3t = "V3" + tc;
    //#   var boundingbox3t = "BoundingBox3" + tc;
    //#   var iboundingsphere = "IBoundingSphere3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var eps = isDouble ? "1e-9" : "1e-5f";
    #region __type__

    /// <summary>
    /// A three-dimensional line with specified start and end points.
    /// </summary>
    [Serializable]
    public partial struct __type__ : __iboundingsphere__
    {
        #region Geometric Properties

        /// <summary>
        /// P0
        /// </summary>
        [XmlIgnore]
        public __v3t__ Origin
        {
            readonly get { return P0; }
            set { P0 = value; }
        }

        /// <summary>
        /// P1 - P0
        /// </summary>
        [XmlIgnore]
        public __v3t__ Direction
        {
            readonly get { return P1 - P0; }
            set { P1 = P0 + value; }
        }

        public readonly __ray3t__ __ray3t__ => new __ray3t__(P0, P1 - P0);

        public readonly bool IsDegenerated => !Direction.Abs().AnyGreater(Constant<__ftype__>.PositiveTinyValue);

        #endregion

        #region __iboundingsphere__ Members

        public readonly __sphere3t__ BoundingSphere3__tc__ => new __sphere3t__(this.ComputeCentroid(), __half__ * Direction.Length);

        #endregion

        public readonly __type__ Flipped => new __type__(P1, P0);
    }

    #endregion

    //# }
}

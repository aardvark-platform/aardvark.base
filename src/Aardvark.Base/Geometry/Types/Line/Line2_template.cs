using System.Xml.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Line2" + tc;
    //#   var type2 = "Line2" + tc2;
    //#   var v2t = "V2" + tc;
    //#   var ray2t = "Ray2" + tc;
    //#   var plane2t = "Plane2" + tc;
    //#   var circle2t = "Circle2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var boundingbox2t = "BoundingBox2" + tc;
    //#   var iboundingcircle = "IBoundingCircle2" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var eps = isDouble ? "1e-9" : "1e-5f";
    #region __type__

    /// <summary>
    /// A two dimensional line with specified start and end points.
    /// </summary>
    public partial struct __type__ : __iboundingcircle__
    {
        #region Geometric Properties

        public readonly __v2t__ Center => (P0 + P1) * __half__;

        /// <summary>
        /// P0
        /// </summary>
        [XmlIgnore]
        public __v2t__ Origin
        {
            readonly get { return P0; }
            set { P0 = value; }
        }

        /// <summary>
        /// P1 - P0
        /// </summary>
        [XmlIgnore]
        public __v2t__ Direction
        {
            readonly get { return P1 - P0; }
            set { P1 = P0 + value; }
        }

        public readonly __ray2t__ __ray2t__ => new __ray2t__(P0, P1 - P0);

        public readonly __plane2t__ __plane2t__ => __ray2t__.__plane2t__;

        public readonly __ftype__ LeftValueOfDir(__v2t__ v) => v.X * (P0.Y - P1.Y) + v.Y * (P1.X - P0.X);

        public readonly __ftype__ RightValueOfDir(__v2t__ v) => v.X * (P1.Y - P0.Y) + v.Y * (P0.X - P1.X);

        public readonly __ftype__ LeftValueOfPos(__v2t__ p)
            => (p.X - P0.X) * (P0.Y - P1.Y) + (p.Y - P0.Y) * (P1.X - P0.X);

        public readonly __ftype__ RightValueOfPos(__v2t__ p)
            => (p.X - P0.X) * (P1.Y - P0.Y) + (p.Y - P0.Y) * (P0.X - P1.X);

        public readonly bool IsDegenerated => Direction.AllTiny;

        #endregion

        #region __iboundingcircle__ Members

        public readonly __circle2t__ BoundingCircle2__tc__ => new __circle2t__(Center, __half__ * Direction.Length);

        #endregion

        #region Geometric Computations

        public readonly __v2t__ GetClosestPointOnLine(__v2t__ p)
        {
            var d = P0 - P1;
            var l = d.LengthSquared;
            if(Fun.IsTiny(l))
                return P0; //it does not matter which of the two points we choose
            var t = (P0.Dot(d) - p.Dot(d)) / l; //parametric distance from P0 to P1, where closest point to p is
            if (t <= 0)
                return P0;
            if (t >= 1)
                return P1;
            return P0 - t * d;
        }

        public readonly __ftype__ GetDistanceToLine(__v2t__ p)
        {
            var f = GetClosestPointOnLine(p);
            return (f - p).Length;
        }

        public readonly bool IsDistanceToPointSmallerThan(__v2t__ p, __ftype__ maxDist)
        {
            //speed-up by first checking the bounding box
            var box = __boundingbox2t__;
            box.EnlargeBy(maxDist);
            if (!box.Contains(p))
                return false;
            return GetDistanceToLine(p) <= maxDist;
        }

        public readonly __type__ Flipped => new __type__(P1, P0);

        #endregion

        /// <summary>
        /// Returns true if points a, b and c are exactly collinear.
        /// </summary>
        public static bool IsCollinear(__v2t__ a, __v2t__ b, __v2t__ c)
            => (b.Y - a.Y) * (c.X - b.X) == (c.Y - b.Y) * (b.X - a.X);

        /// <summary>
        /// Returns true if points a, b and c are collinear within eps.
        /// </summary>
        public static bool IsCollinear(__v2t__ a, __v2t__ b, __v2t__ c, __ftype__ eps = __eps__)
            => Fun.ApproximateEquals((b.Y - a.Y) * (c.X - b.X), (c.Y - b.Y) * (b.X - a.X), eps);
    }

    #endregion

    //# }
}

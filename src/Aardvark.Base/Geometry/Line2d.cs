
namespace Aardvark.Base
{
    /// <summary>
    /// A two dimensional line with specified start and end points.
    /// </summary>
    public partial struct Line2d : IBoundingCircle2d
    {
        #region Geometric Properties

        public V2d Center
        {
            get { return (P0 + P1) * 0.5; }
        }

        public V2d Origin
        {
            get { return P0; }
            set { P0 = value; }
        }

        public V2d Direction
        {
            get { return P1 - P0; }
            set { P1 = P0 + value; }
        }

        public Ray2d Ray2d
        {
            get { return new Ray2d(P0, P1 - P0); }
        }

        public Plane2d Plane2d
        {
            get { return Ray2d.Plane2d; }
        }

        public double LeftValueOfDir(V2d v)
        {
            return v.X * (P0.Y - P1.Y) + v.Y * (P1.X - P0.X);
        }

        public double RightValueOfDir(V2d v)
        {
            return v.X * (P1.Y - P0.Y) + v.Y * (P0.X - P1.X);
        }

        public double LeftValueOfPos(V2d p)
        {
            return (p.X - P0.X) * (P0.Y - P1.Y) + (p.Y - P0.Y) * (P1.X - P0.X);
        }

        public double RightValueOfPos(V2d p)
        {
            return (p.X - P0.X) * (P1.Y - P0.Y) + (p.Y - P0.Y) * (P0.X - P1.X);
        }

        public bool IsDegenerated
        {
            get { return Direction.AllTiny; }
        }

        #endregion

        #region IBoundingCircle2d Members

        public Circle2d BoundingCircle2d
        {
            get { return new Circle2d(Center, 0.5 * Direction.Length); }
        }

        #endregion

        #region Geometric Computations

        public V2d GetClosestPointOnLine(V2d p)
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

        public double GetDistanceToLine(V2d p)
        {
            var f = GetClosestPointOnLine(p);
            return (f - p).Length;
        }

        public bool IsDistanceToPointSmallerThan(V2d p, double maxDist)
        {
            //speed-up by first checking the bounding box
            var box = BoundingBox2d;
            box.EnlargeBy(maxDist);
            if (!box.Contains(p))
                return false;
            return GetDistanceToLine(p) <= maxDist;
        }

        #endregion
    }

}

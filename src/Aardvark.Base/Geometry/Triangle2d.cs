
namespace Aardvark.Base
{
    /// <summary>
    /// A two-dimensional triangle represented by its three points.
    /// </summary>
    public partial struct Triangle2d : IBoundingCircle2d
    {
        #region Geometric Properties

        public double Area
        {
            get
            {
                return 0.5 * ((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y)).Abs();
            }
        }

        public bool IsDegenerated
        {
            get { return WindingOrder.IsTiny(); }
        }

        /// <summary>
        /// Returns a value less than zero for ccw and greater than zero for cw.
        /// The magnitude is twice the area.
        /// </summary>
        public double WindingOrder
        {
            get { return (P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y); }
        }

        #endregion

        #region CircumCircle

        public Circle2d CircumCircle
        {
            get
            {
                V2d center;
                double radiusSquared;
                ComputeCircumCircleSquared(P0, P1, P2,
                                    out center, out radiusSquared);
                return new Circle2d(center, radiusSquared.Sqrt());
            }
        }

        public Circle2d CircumCircleSquared
        {
            get
            {
                V2d center;
                double radiusSquared;
                ComputeCircumCircleSquared(P0, P1, P2,
                                    out center, out radiusSquared);
                return new Circle2d(center, radiusSquared);
            }
        }

        public static void ComputeCircumCircleSquared(
            V2d p0, V2d p1, V2d p2,
            out V2d center, out double radiusSquared)
        {
            double y01abs = System.Math.Abs(p0.Y - p1.Y);
            double y12abs = System.Math.Abs(p1.Y - p2.Y);

            if (y01abs < Constant<double>.PositiveTinyValue
                && y12abs < Constant<double>.PositiveTinyValue)
            {
                center = V2d.NaN; radiusSquared = -1.0; return;
            }

            double xc, yc;

            if (y01abs < Constant<double>.PositiveTinyValue)
            {
                double m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                double m12x = 0.5 * (p1.X + p2.X);
                double m12y = 0.5 * (p1.Y + p2.Y);
                xc = 0.5 * (p1.X + p0.X);
                yc = m2 * (xc - m12x) + m12y;
            }
            else if (y12abs < Constant<double>.PositiveTinyValue)
            {
                double m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                double m01x = 0.5 * (p0.X + p1.X);
                double m01y = 0.5 * (p0.Y + p1.Y);
                xc = 0.5 * (p2.X + p1.X);
                yc = m1 * (xc - m01x) + m01y;
            }
            else
            {
                double m1 = (p0.X - p1.X) / (p1.Y - p0.Y);
                double m2 = (p1.X - p2.X) / (p2.Y - p1.Y);
                double m01x = 0.5 * (p0.X + p1.X);
                double m01y = 0.5 * (p0.Y + p1.Y);
                double m12x = 0.5 * (p1.X + p2.X);
                double m12y = 0.5 * (p1.Y + p2.Y);
                double m12 = m1 - m2;
                if (System.Math.Abs(m12) < Constant<double>.PositiveTinyValue)
                {
                    center = V2d.NaN; radiusSquared = -1.0; return;
                }
                xc = (m1 * m01x - m2 * m12x + m12y - m01y) / m12;
                if (y01abs > y12abs)
                {
                    yc = m1 * (xc - m01x) + m01y;
                }
                else
                {
                    yc = m2 * (xc - m12x) + m12y;
                }
            }
            center = new V2d(xc, yc);
            radiusSquared = V2d.DistanceSquared(p0, center);
        }

        #endregion

        #region IBoundingCircle2d Members

        public Circle2d BoundingCircle2d
        {
            get
            {
                var edge01 = P1 - P0;
                var edge02 = P2 - P0;
                double dot0101 = V2d.Dot(edge01, edge01);
                double dot0102 = V2d.Dot(edge01, edge02);
                double dot0202 = V2d.Dot(edge02, edge02);
                double d = 2.0 * (dot0101 * dot0202 - dot0102 * dot0102);
                if (d.Abs() <= 0.000001) return Circle2d.Invalid;
                double s = (dot0101 * dot0202 - dot0202 * dot0102) / d;
                double t = (dot0202 * dot0101 - dot0101 * dot0102) / d;
                var p = P0;
                var cir = new Circle2d();
                if (s <= 0.0)
                    cir.Center = 0.5 * (P0 + P2);
                else if (t <= 0.0)
                    cir.Center = 0.5 * (P0 + P1);
                else if (s + t >= 1.0)
                {
                    cir.Center = 0.5 * (P1 + P2);
                    p = P1;
                }
                else
                    cir.Center = P0 + s * edge01 + t * edge02;
                cir.Radius = (cir.Center - p).Length;
                return cir;
            }
        }

        #endregion
    }
}

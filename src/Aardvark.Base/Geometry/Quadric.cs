using System;

namespace Aardvark.Base
{
    public struct Quadric
    {
        V3d m_normal;
        M44d m_errorQuadric;

        #region Properties

        public V3d Normal
        {
            get { return m_normal.Normalized; }
            set { m_normal = value; }
        }
        
        public M44d ErrorQuadric
        {
            get { return m_errorQuadric; }
            set { m_errorQuadric = value; }
        }

        public double ErrorOffset => ErrorQuadric.M33;
        
        public M44d ErrorHeuristic { get; set; }

        #endregion

        public void Create(Plane3d plane)
        {
            CreateQuadric(plane);
            CreateHeuristic();
        }

        #region Create Quadric/Heuristic

        public void CreateQuadric(Plane3d plane)
        {
            Normal = plane.Normal;
            double a = Normal.X;
            double b = Normal.Y;
            double c = Normal.Z;

            // Garland uses "ax + by + cz + d = 0"
            // Aardvark uses "ax + by + cz - d = 0"
            double d = -plane.Distance;

            m_errorQuadric.M00 = a * a;
            m_errorQuadric.M11 = b * b;
            m_errorQuadric.M22 = c * c;
            m_errorQuadric.M33 = d * d;

            m_errorQuadric.M01 = m_errorQuadric.M10 = a * b;
            m_errorQuadric.M02 = m_errorQuadric.M20 = a * c;
            m_errorQuadric.M03 = m_errorQuadric.M30 = a * d;
            m_errorQuadric.M12 = m_errorQuadric.M21 = b * c;
            m_errorQuadric.M13 = m_errorQuadric.M31 = b * d;
            m_errorQuadric.M23 = m_errorQuadric.M32 = c * d;
        }

        public void CreateHeuristic()
        {
            if (m_errorQuadric == M44d.Zero)
            {
                throw new InvalidOperationException("Must call CreateQuadric(...) first");
            }

            ErrorHeuristic = ToHeuristic(ErrorQuadric);
        }

        #endregion

        #region Operator Overload

        public static Quadric operator +(Quadric lhs, Quadric rhs)
        {
            Quadric result = new Quadric
            {
                ErrorQuadric = lhs.ErrorQuadric + rhs.ErrorQuadric,
                Normal = lhs.Normal + rhs.Normal
            };

            result.ErrorHeuristic = ToHeuristic(result.ErrorQuadric);

            return result;
        }

        //public static Quadric operator -(Quadric lhs, Quadric rhs)
        //{
        //    Quadric result = new Quadric();

        //    result.ErrorQuadric = lhs.ErrorQuadric - rhs.ErrorQuadric;

        //    result.ErrorHeuristic = Quadric.ToHeuristic(result.ErrorQuadric);

        //    return result;
        //}

        #endregion

        #region Static Methods 

        static M44d ToHeuristic(M44d quadric)
        {
            var result = new M44d();

            result = quadric;
            result.R3 = new V4d(0, 0, 0, 1);

            return result;
        }

        #endregion
    }
}

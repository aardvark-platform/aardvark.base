using System;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region QuadricF

    public struct QuadricF
    {
        V3f m_normal;
        M44f m_errorQuadric;

        #region Properties

        public V3f Normal
        {
            readonly get { return m_normal.Normalized; }
            set { m_normal = value; }
        }

        public M44f ErrorQuadric
        {
            readonly get { return m_errorQuadric; }
            set { m_errorQuadric = value; }
        }

        public readonly float ErrorOffset => ErrorQuadric.M33;

        public M44f ErrorHeuristic { readonly get; set; }

        #endregion

        public void Create(Plane3f plane)
        {
            CreateQuadric(plane);
            CreateHeuristic();
        }

        #region Create QuadricF/Heuristic

        public void CreateQuadric(Plane3f plane)
        {
            Normal = plane.Normal;
            float a = Normal.X;
            float b = Normal.Y;
            float c = Normal.Z;

            // Garland uses "ax + by + cz + d = 0"
            // Aardvark uses "ax + by + cz - d = 0"
            float d = -plane.Distance;

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
            if (m_errorQuadric == M44f.Zero)
            {
                throw new InvalidOperationException("Must call CreateQuadric(...) first");
            }

            ErrorHeuristic = ToHeuristic(ErrorQuadric);
        }

        #endregion

        #region Operator Overload

        public static QuadricF operator +(QuadricF lhs, QuadricF rhs)
        {
            var result = new QuadricF
            {
                ErrorQuadric = lhs.ErrorQuadric + rhs.ErrorQuadric,
                Normal = lhs.Normal + rhs.Normal
            };

            result.ErrorHeuristic = ToHeuristic(result.ErrorQuadric);

            return result;
        }

        //public static QuadricF operator -(QuadricF lhs, QuadricF rhs)
        //{
        //    QuadricF result = new QuadricF();

        //    result.ErrorQuadric = lhs.ErrorQuadric - rhs.ErrorQuadric;

        //    result.ErrorHeuristic = QuadricF.ToHeuristic(result.ErrorQuadric);

        //    return result;
        //}

        #endregion

        #region Static Methods

        static M44f ToHeuristic(M44f quadric)
        {
            var result = new M44f();

            result = quadric;
            result.R3 = new V4f(0, 0, 0, 1);

            return result;
        }

        #endregion
    }

    #endregion

    #region QuadricD

    public struct QuadricD
    {
        V3d m_normal;
        M44d m_errorQuadric;

        #region Properties

        public V3d Normal
        {
            readonly get { return m_normal.Normalized; }
            set { m_normal = value; }
        }

        public M44d ErrorQuadric
        {
            readonly get { return m_errorQuadric; }
            set { m_errorQuadric = value; }
        }

        public readonly double ErrorOffset => ErrorQuadric.M33;

        public M44d ErrorHeuristic { readonly get; set; }

        #endregion

        public void Create(Plane3d plane)
        {
            CreateQuadric(plane);
            CreateHeuristic();
        }

        #region Create QuadricD/Heuristic

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

        public static QuadricD operator +(QuadricD lhs, QuadricD rhs)
        {
            var result = new QuadricD
            {
                ErrorQuadric = lhs.ErrorQuadric + rhs.ErrorQuadric,
                Normal = lhs.Normal + rhs.Normal
            };

            result.ErrorHeuristic = ToHeuristic(result.ErrorQuadric);

            return result;
        }

        //public static QuadricD operator -(QuadricD lhs, QuadricD rhs)
        //{
        //    QuadricD result = new QuadricD();

        //    result.ErrorQuadric = lhs.ErrorQuadric - rhs.ErrorQuadric;

        //    result.ErrorHeuristic = QuadricD.ToHeuristic(result.ErrorQuadric);

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

    #endregion

}

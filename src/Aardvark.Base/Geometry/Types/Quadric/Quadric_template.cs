using System;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var tccaps = tc.ToUpper();
    //#   var tccaps2 = tc2.ToUpper();
    //#   var type = "Quadric" + tccaps;
    //#   var type2 = "Quadric" + tccaps2;
    //#   var v3t = "V3" + tc;
    //#   var v4t = "V4" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var m44t = "M44" + tc;
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#   var half = isDouble ? "0.5" : "0.5f";
    #region __type__

    public struct __type__
    {
        __v3t__ m_normal;
        __m44t__ m_errorQuadric;

        #region Properties

        public __v3t__ Normal
        {
            get { return m_normal.Normalized; }
            set { m_normal = value; }
        }

        public __m44t__ ErrorQuadric
        {
            get { return m_errorQuadric; }
            set { m_errorQuadric = value; }
        }

        public __ftype__ ErrorOffset => ErrorQuadric.M33;

        public __m44t__ ErrorHeuristic { get; set; }

        #endregion

        public void Create(__plane3t__ plane)
        {
            CreateQuadric(plane);
            CreateHeuristic();
        }

        #region Create __type__/Heuristic

        public void CreateQuadric(__plane3t__ plane)
        {
            Normal = plane.Normal;
            __ftype__ a = Normal.X;
            __ftype__ b = Normal.Y;
            __ftype__ c = Normal.Z;

            // Garland uses "ax + by + cz + d = 0"
            // Aardvark uses "ax + by + cz - d = 0"
            __ftype__ d = -plane.Distance;

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
            if (m_errorQuadric == __m44t__.Zero)
            {
                throw new InvalidOperationException("Must call CreateQuadric(...) first");
            }

            ErrorHeuristic = ToHeuristic(ErrorQuadric);
        }

        #endregion

        #region Operator Overload

        public static __type__ operator +(__type__ lhs, __type__ rhs)
        {
            var result = new __type__
            {
                ErrorQuadric = lhs.ErrorQuadric + rhs.ErrorQuadric,
                Normal = lhs.Normal + rhs.Normal
            };

            result.ErrorHeuristic = ToHeuristic(result.ErrorQuadric);

            return result;
        }

        //public static __type__ operator -(__type__ lhs, __type__ rhs)
        //{
        //    __type__ result = new __type__();

        //    result.ErrorQuadric = lhs.ErrorQuadric - rhs.ErrorQuadric;

        //    result.ErrorHeuristic = __type__.ToHeuristic(result.ErrorQuadric);

        //    return result;
        //}

        #endregion

        #region Static Methods

        static __m44t__ ToHeuristic(__m44t__ quadric)
        {
            var result = new __m44t__();

            result = quadric;
            result.R3 = new __v4t__(0, 0, 0, 1);

            return result;
        }

        #endregion
    }

    #endregion

    //# }
}

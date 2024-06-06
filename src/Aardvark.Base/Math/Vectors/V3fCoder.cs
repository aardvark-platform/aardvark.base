// The decode table would grow to large for 64bit codes, and should not
// be used in this case. However it is very likely that the 64 bit version
// does not work as of yet.
// The actual savings by using a table are small enough, that it is
// normally not worth the associated initialization cost.
// #define V3FCODER_DECODE_TABLE
// The following option is mostly for debugging:
// #define V3FCODER_NO_WARP
using System;

namespace Aardvark.Base
{
    /// <summary>
    /// A V3fCoder can be used to encode direction vectors in unsigned
    /// integers.
    /// </summary>
    public class V3fCoder
    {
        // private uint m_raster;
        private readonly uint m_r2Sub1;
        private readonly double m_doubleRaster;
        private readonly double m_invDoubleRaster;
        private readonly uint m_edgeBasis;
        private readonly uint m_cornerBasis;
        #if V3FCODER_DECODE_TABLE
        private double[] m_unWarpTable;
        #endif
        private readonly double m_dRp05;

        #region Constructor

        /// <summary>
        /// Create a V3fCoder with supplied raster. The raster defines the
        /// number of discretized directons along an octant of one of the
        /// major circumferences of the sphere. Thus on each of the 3 major
        /// circumferences of the sphere 8 * raster evenly spaced different
        /// directions are encoded.
        /// </summary>
        /// <param name="raster"></param>
        public V3fCoder(uint raster)
        {
            // m_raster = raster;
            m_r2Sub1 = 2 * raster - 1;
            m_doubleRaster = (double)raster;
            m_invDoubleRaster = 1.0/m_doubleRaster;
            m_edgeBasis = 6 * m_r2Sub1 * m_r2Sub1;
            m_cornerBasis = m_edgeBasis + 12  * m_r2Sub1;
            
            m_dRp05 = m_doubleRaster + 0.5;


            #if V3FCODER_DECODE_TABLE
            // build a table for slightly faster decoding
            m_unWarpTable = new double[m_r2Sub1];
            
            for (int i = 0; i < m_r2Sub1; i++)
            {
                double u = (i+1) * m_invDoubleRaster - 1.0;
                #if (!V3FCODER_NO_WARP)
                u = SphericalOfBox(u);
                #endif
                m_unWarpTable[i] = u;
            }
            #endif
        }

        #endregion

        #region Static Creator

        public static V3fCoder ForBits(int bits)
        {
            if (bits < 5) return null;
            if (bits > 32) bits = 32;
            return s_coderForBits[bits];
        }

        #endregion

        #region Constants

        static readonly uint[] s_rasterForBitsTable =
        {
            // bits       raster,    used bits
            /*    0 */         0,
            /*    1 */         0,
            /*    2 */         0,
            /*    3 */         0,
            /*    4 */         0,
            /*    5 */         1, //  4.7004397181
            /*    6 */         1, //  4.7004397181
            /*    7 */         2, //  6.6147098441
            /*    8 */         3, //  7.7681843248
            /*    9 */         4, //  8.5924570373
            /*   10 */         6, //  9.7582232147
            /*   11 */         9, // 10.9262959948
            /*   12 */        13, // 11.9865531498
            /*   13 */        18, // 12.9251835194
            /*   14 */        26, // 13.9860197731
            /*   15 */        36, // 14.9249052665
            /*   16 */        52, // 15.9858863980
            /*   17 */        73, // 16.9646341787
            /*   18 */       104, // 17.9858530524
            /*   19 */       147, // 18.9843127540
            /*   20 */       209, // 19.9996835172
            /*   21 */       295, // 20.9941061707
            /*   22 */       418, // 21.9996814530
            /*   23 */       591, // 22.9989914853
            /*   24 */       836, // 23.9996809369
            /*   25 */      1182, // 24.9989912271
            /*   26 */      1672, // 25.9996808079
            /*   27 */      2364, // 26.9989911626
            /*   28 */      3344, // 27.9996807756
            /*   29 */      4729, // 28.9996013590
            /*   30 */      6688, // 29.9996807676
            /*   31 */      9459, // 30.9999064129
            /*   32 */     13377, // 31.9998964715
            
            /*
                For the following codes to work, the algorithm
                has to be changed to 64-bit ints.
            */
            
            /*   33 */     18918, // 32.9999064119
            /*   34 */     26754, // 33.9998964710
            /*   35 */     37837, // 34.9999826710
            /*   36 */     53509, // 35.9999503948
            /*   37 */     75674, // 36.9999826710
            /*   38 */    107019, // 37.9999773564
            /*   39 */    151348, // 38.9999826710
            /*   40 */    214039, // 39.9999908371
            /*   41 */    302697, // 40.9999922033
            /*   42 */    428079, // 41.9999975774
            /*   43 */    605395, // 42.9999969694
            /*   44 */    856158, // 43.9999975774
            /*   45 */   1210791, // 44.9999993524
            /*   46 */   1712317, // 45.9999992625
            /*   47 */   2421582, // 46.9999993524
            /*   48 */   3424634, // 47.9999992625
            /*   49 */   4843165, // 48.9999999482
            /*   50 */   6849269, // 49.9999996837
            /*   51 */   9686330, // 50.9999999482
            /*   52 */  13698539, // 51.9999998944
            /*   53 */  19372660, // 52.9999999482
            /*   54 */  27397079, // 53.9999999997
            /*   55 */  38745320, // 54.9999999482
            /*   56 */  54794158, // 55.9999999997
            /*   57 */  77490641, // 56.9999999854
            /*   58 */ 109588316, // 57.9999999997
            /*   59 */ 154981282, // 58.9999999854
            /*   60 */ 219176632, // 59.9999999997
            /*   61 */ 309962565, // 60.9999999948
            /*   62 */ 438353264, // 61.9999999997
            /*   63 */ 619925131, // 62.9999999994
            /*   64 */ 876706528, // 63.9999999997
        };

        const double c_piOver4 = Constant.Pi * 0.25;
        const double c_4OverPi = 4.0 / Constant.Pi;

        private static readonly V3fCoder[] s_coderForBits =
            new V3fCoder[33];

        static V3fCoder()
        {
            for (int bits = 5; bits < 33; bits++)
                s_coderForBits[bits] = new V3fCoder(RasterForBits(bits));
        }

        #endregion

        #region Encoding Helpers

        public static double SphericalOfBox(double x)
        { return System.Math.Tan(x * c_piOver4); }
        public static double BoxOfSpherical(double x)
        { return System.Math.Atan(x) * c_4OverPi; }

        #if (!V3FCODER_DECODE_TABLE)
        #if (V3FCODER_NO_WARP)
        private double Unwarp(uint u)
        { return (u+1) * m_invDoubleRaster - 1.0; }
        #else
        private double Unwarp(uint u)
        { return System.Math.Tan(((u + 1) * m_invDoubleRaster - 1.0) * c_piOver4); }
        #endif
        #else
        private double Unwarp(uint u)  { return m_unWarpTable[u]; }
        #endif

        /*
            The directions are coded based on the faces, edges, and corners
            of a cube of size 2, centered at the origin. Within an uint the
            first codes are for the faces, then the edges and finally the
            corners. The coding within these three groups is performed
            according to the following table:
        
                faces:
                 0: -1  u  v
                 1:  v -1  u
                 2:  u  v -1
                 3: +1  u  v
                 4:  v +1  u
                 5:  u  v +1

                 edges:
                 0:  u -1 -1    faces 1,-,u  2,u,-      corners 0, 1
                 1:  u +1 -1    faces 4,-,u  2,u,+      corners 2, 3
                 2:  u -1 +1    faces 1,+,u  5,u,-      corners 4, 5
                 3:  u +1 +1    faces 4,+,u  5,u,+      corners 6, 7
                 4: -1  u -1    faces 2,-,u  0,u,-      corners 0, 2
                 5: +1  u -1    faces 2,+,u  3,u,-      corners 1, 3
                 6: -1  u +1    faces 5,-,u  0,u,+      corners 4, 6
                 7: +1  u +1    faces 5,+,u  3,u,+      corners 5, 7
                 8: -1 -1  u    faces 0,-,u  1,u,-      corners 0, 4
                 9: +1 -1  u    faces 3,-,u  1,u,+      corners 1, 5
                10: -1 +1  u    faces 0,+,u  4,u,-      corners 2, 6
                11: +1 +1  u    faces 3,+,u  4,u,+      corners 3, 7
                
                corners:
                 0: -1 -1 -1    edges  0,  4,  8    faces 0, 1, 2
                 1: +1 -1 -1    edges  0,  5,  9
                 2: -1 +1 -1    edges  1,  4, 10
                 3: +1 +1 -1    edges  1,  5, 11
                 4: -1 -1 +1    edges  2,  6,  8
                 5: +1 -1 +1    edges  2,  7,  9
                 6: -1 +1 +1    edges  3,  6, 10
                 7: +1 +1 +1    edges  3,  7, 11
            
        */

        uint EncodeCornerIndex(uint corner)
        {
            return m_cornerBasis + corner;
        }

        static readonly uint[] s_edgeNegCorner = new uint[]
                    { 0, 2, 4, 6, 0, 1, 4, 5, 0, 1, 2, 3 };
        static readonly uint[] s_edgePosCorner = new uint[]
                    { 1, 3, 5, 7, 2, 3, 6, 7, 4, 5, 6, 7 };

        uint EncodeEdgeIndex(uint edge, int iu)
        {
            if (iu < 0) return m_cornerBasis + s_edgeNegCorner[edge];
            if (iu >= m_r2Sub1) return m_cornerBasis + s_edgePosCorner[edge];
            return m_edgeBasis + (uint)(edge  * m_r2Sub1 + iu);
        }

        uint RawEncodeEdgeIndex(uint edge, int iu)
        {
            return m_edgeBasis + (uint)(edge * m_r2Sub1 + iu);
        } 

        static readonly uint[] s_faceNegUedge = new uint[] { 8, 0, 4, 9, 1, 6 };
        static readonly uint[] s_facePosUedge = new uint[] { 10, 2, 5, 11, 3, 7 };
        static readonly uint[] s_faceNegVedge = new uint[] { 4, 8, 0, 5, 10, 2 };
        static readonly uint[] s_facePosVedge = new uint[] { 6, 9, 1, 7, 11, 3 };

        uint EncodeFaceIndex(uint face, int iu, int iv)
        {
            if (iu < 0) return EncodeEdgeIndex(s_faceNegUedge[face], iv);
            if (iu >= m_r2Sub1) return EncodeEdgeIndex(s_facePosUedge[face], iv);
            if (iv < 0) return EncodeEdgeIndex(s_faceNegVedge[face], iu);
            if (iv >= m_r2Sub1) return EncodeEdgeIndex(s_facePosVedge[face], iu);
            return (uint)((face * m_r2Sub1 + iv ) * m_r2Sub1 + iu);
        }

        uint RawEncodeFaceIndex(uint face, int iu, int iv)
        {
            return (uint)((face * m_r2Sub1 + iv) * m_r2Sub1 + iu);
        }

        #endregion

        #region Encoding

        /// <summary>
        /// Encode the given direction in an unsigned integer. The direction
        /// does not need to be normalized!
        /// </summary>
        /// <param name="dir">The direction to encode.</param>
        /// <returns></returns>
        public uint Encode(V3f dir)
        {
            int mAxis;
            double aX = System.Math.Abs(dir.X);
            double aY = System.Math.Abs(dir.Y);
            double aZ = System.Math.Abs(dir.Z);
            double u, v;
            if (aX > aY)
            {
                if (aX > aZ)
                {
                    double invSize = 1.0/aX; mAxis = 0;
                    u = dir.Y * invSize; v = dir.Z * invSize;
                }
                else
                {
                    double invSize = 1.0/aZ; mAxis = 2;
                    u = dir.X * invSize; v = dir.Y * invSize;
                }
            }
            else
            {
                if (aY > aZ)
                {
                    double invSize = 1.0/aY; mAxis = 1;
                    u = dir.Z * invSize; v = dir.X * invSize;
                }
                else
                {
                    double invSize = 1.0/aZ; mAxis = 2;
                    u = dir.X * invSize; v = dir.Y * invSize;
                }
            }
            #if (!V3FCODER_NO_WARP)
            u = BoxOfSpherical(u);
            v = BoxOfSpherical(v);
            #endif
            uint face = (uint)(mAxis + (dir[mAxis] < 0 ? 0 : 3));
            return EncodeFaceIndex(face,
                                   (int)(u * m_doubleRaster + m_dRp05) - 1,
                                   (int)(v * m_doubleRaster + m_dRp05) - 1);
        }

        #endregion

        #region Decoding Helpers

        static readonly V3f[] s_vecToCornerTableNonNormalized =
        {
            new V3f( -1.0f, -1.0f, -1.0f ),
            new V3f( +1.0f, -1.0f, -1.0f ),
            new V3f( -1.0f, +1.0f, -1.0f ),
            new V3f( +1.0f, +1.0f, -1.0f ),
            new V3f( -1.0f, -1.0f, +1.0f ),
            new V3f( +1.0f, -1.0f, +1.0f ),
            new V3f( -1.0f, +1.0f, +1.0f ),
            new V3f( +1.0f, +1.0f, +1.0f )
        };

        const float c_oneOverSqrt3 = 0.57735026918962584f;
        static readonly V3f[] s_vecToCornerTable =
        {
            new V3f( -c_oneOverSqrt3, -c_oneOverSqrt3, -c_oneOverSqrt3 ),
            new V3f( +c_oneOverSqrt3, -c_oneOverSqrt3, -c_oneOverSqrt3 ),
            new V3f( -c_oneOverSqrt3, +c_oneOverSqrt3, -c_oneOverSqrt3 ),
            new V3f( +c_oneOverSqrt3, +c_oneOverSqrt3, -c_oneOverSqrt3 ),
            new V3f( -c_oneOverSqrt3, -c_oneOverSqrt3, +c_oneOverSqrt3 ),
            new V3f( +c_oneOverSqrt3, -c_oneOverSqrt3, +c_oneOverSqrt3 ),
            new V3f( -c_oneOverSqrt3, +c_oneOverSqrt3, +c_oneOverSqrt3 ),
            new V3f( +c_oneOverSqrt3, +c_oneOverSqrt3, +c_oneOverSqrt3 )
        };

        static readonly double[] s_uSignOfEdge = { -1, +1, -1, +1 };
        static readonly double[] s_vSignOfEdge = { -1, -1, +1, +1 };
        static readonly int[] s_uAxis = { 1, 0, 0 };
        static readonly int[] s_vAxis = { 2, 2, 1 };

        #endregion

        #region Decoding

        /// <summary>
        /// Decode the unsigned integer direction code. 
        /// </summary>
        /// <param name="code"></param>
        /// <returns>A normalized direction.</returns>
        public V3f Decode(uint code)
        {
            if (code < m_edgeBasis) // face number is code
            {
                double u = Unwarp(code % m_r2Sub1);
                code /= m_r2Sub1;
                double v = Unwarp(code % m_r2Sub1);
                code /= m_r2Sub1;
                double scale = 1.0 / System.Math.Sqrt(1.0 + u * u + v * v);
                switch (code)
                {
                    case 0: return new V3f(-scale, u * scale, v * scale);
                    case 1: return new V3f(v * scale, -scale, u * scale);
                    case 2: return new V3f(u * scale, v * scale, -scale);
                    case 3: return new V3f(+scale, u * scale, v * scale);
                    case 4: return new V3f(v * scale, +scale, u * scale);
                    case 5: return new V3f(u * scale, v * scale, +scale);
                    default: throw new ArgumentException();
                }
            }
            else if (code < m_cornerBasis)
            {
                code -= m_edgeBasis;
                double u = Unwarp(code % m_r2Sub1);
                code /= m_r2Sub1;      // edge number in code
                double scale = 1.0 / System.Math.Sqrt(2.0 + u * u);
                uint edge = code & 3; code >>= 2;
                V3f dir = new V3f(0, 0, 0); // init to make compiler happy
                dir[(int)code] = (float)(u * scale);
                dir[s_uAxis[code]] = (float)(s_uSignOfEdge[edge] * scale);
                dir[s_vAxis[code]] = (float)(s_vSignOfEdge[edge] * scale);
                return dir;
#if NEVERMORE
                float edgeOne = (code & 1) == 0 ? -scale : scale;
                float edgeTwo = (code & 2) == 0 ? -scale : scale;
                switch (code >> 2)
                {
                    case 0: return new V3f((float)(u * scale), edgeOne, edgeTwo);
                    case 1: return new V3f(edgeOne, (float)(u * scale), edgeTwo);
                    case 2: return new V3f(edgeOne, edgeTwo, (float)(u * scale));
                    default: throw new ArgumentException();
                }
#endif
            }
            else
                return s_vecToCornerTable[code - m_cornerBasis];
        }

        public V3f DecodeOnCube(uint code, bool warped)
        {
            if (code < m_edgeBasis) // face number is code
            {

                double u = warped
                    ? Unwarp(code % m_r2Sub1)
                    : (code % m_r2Sub1 + 1) * m_invDoubleRaster - 1.0;

                code /= m_r2Sub1;
                double v = warped
                    ? Unwarp(code % m_r2Sub1)
                    : (code % m_r2Sub1 + 1) * m_invDoubleRaster - 1.0;
                code /= m_r2Sub1;

                switch (code)
                {
                    case 0: return new V3f(-1, u, v);
                    case 1: return new V3f(v, -1, u);
                    case 2: return new V3f(u, v, -1);
                    case 3: return new V3f(+1, u, v);
                    case 4: return new V3f(v, +1, u);
                    case 5: return new V3f(u, v, +1);
                    default: throw new ArgumentException();
                }
            }
            else if (code < m_cornerBasis)
            {
                code -= m_edgeBasis;
                double u = warped
                    ? Unwarp(code % m_r2Sub1)
                    : (code % m_r2Sub1+1) * m_invDoubleRaster - 1.0;
                code /= m_r2Sub1;      // edge number in code
                uint edge = code & 3; code >>= 2;
                V3f dir = new V3f(0, 0, 0); // init to make compiler happy
                dir[(int)code] = (float)u;
                dir[s_uAxis[code]] = (float)(s_uSignOfEdge[edge]);
                dir[s_vAxis[code]] = (float)(s_vSignOfEdge[edge]);
                return dir;
#if NEVERMORE
                float edgeOne = (code & 1) == 0 ? -1.0f : 1.0f;
                float edgeTwo = (code & 2) == 0 ? -1.0f : 1.0f;
                switch (code >> 2)
                {
                    case 0: return new V3f((float)u, edgeOne, edgeTwo);
                    case 1: return new V3f(edgeOne, (float)u, edgeTwo);
                    case 2: return new V3f(edgeOne, edgeTwo, (float)u);
                    default: throw new ArgumentException();
                }
#endif
            }
            else
                return s_vecToCornerTableNonNormalized[code - m_cornerBasis];
        }

        #endregion

        #region Neighbours Helpers

        static readonly uint[] s_edgeFace0
                = new uint[] { 1, 4, 1, 4, 2, 2, 5, 5, 0, 3, 0, 3 };
        static readonly int[] s_edgeFace0u
                = new int[] { 0, 0, 1, 1, 0, 1, 0, 1, 0 ,0, 1, 1 };
        static readonly uint[] s_edgeFace1
                = new uint[] { 2, 2, 5, 5, 0, 3, 0, 3, 1, 1, 4, 4 };
        static readonly int[] s_edgeFace1v
                = new int[] { 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1 };

        #endregion

        #region Neigbours

        /// <summary>
        /// Fills the array neighbourCodes with the codes of all neigbouring
        /// cells of the code discretization.
        /// </summary>
        /// <param name="code">The code for which to calculate all
        /// neighbours.</param>
        /// <param name="neighbourCodes">The array that is filled with
        /// the neighbourcodes</param>
        /// <returns>The number of neigbouring cells (maximal 8).</returns>
        public uint NeighbourCodes(uint code, uint[] neighbourCodes)
        {
            if (code < m_edgeBasis) // face number is code
            {
                int iu = (int)(code % m_r2Sub1);
                code /= m_r2Sub1;
                int iv = (int)(code % m_r2Sub1);
                code /= m_r2Sub1;

                if (iu > 0 && iu < m_r2Sub1 - 1 && iv > 0 && iv < m_r2Sub1 - 1)
                {
                    neighbourCodes[0] = RawEncodeFaceIndex(code, iu - 1, iv - 1);
                    neighbourCodes[1] = RawEncodeFaceIndex(code, iu - 1, iv);
                    neighbourCodes[2] = RawEncodeFaceIndex(code, iu - 1, iv + 1);
                    neighbourCodes[3] = RawEncodeFaceIndex(code, iu, iv - 1);
                    neighbourCodes[4] = RawEncodeFaceIndex(code, iu, iv + 1);
                    neighbourCodes[5] = RawEncodeFaceIndex(code, iu + 1, iv - 1);
                    neighbourCodes[6] = RawEncodeFaceIndex(code, iu + 1, iv);
                    neighbourCodes[7] = RawEncodeFaceIndex(code, iu + 1, iv + 1);
                }
                else
                {
                    neighbourCodes[0] = EncodeFaceIndex(code, iu - 1, iv - 1);
                    neighbourCodes[1] = EncodeFaceIndex(code, iu - 1, iv);
                    neighbourCodes[2] = EncodeFaceIndex(code, iu - 1, iv + 1);
                    neighbourCodes[3] = EncodeFaceIndex(code, iu, iv - 1);
                    neighbourCodes[4] = EncodeFaceIndex(code, iu, iv + 1);
                    neighbourCodes[5] = EncodeFaceIndex(code, iu + 1, iv - 1);
                    neighbourCodes[6] = EncodeFaceIndex(code, iu + 1, iv);
                    neighbourCodes[7] = EncodeFaceIndex(code, iu + 1, iv + 1);
                }
              
                return 8;
            }
            if (code < m_cornerBasis)
            {
                int nc = 0;
                code -= m_edgeBasis;
                int iu = (int)(code % m_r2Sub1);
                code /= m_r2Sub1;      // edge number in code
                neighbourCodes[nc++] = EncodeEdgeIndex(code, iu - 1);
                neighbourCodes[nc++] = EncodeEdgeIndex(code, iu + 1);

                int m = ((int)m_r2Sub1 - 1);

                for (int i = -1; i < 2; i++)
                {
                    neighbourCodes[nc++] = EncodeFaceIndex(
                        s_edgeFace0[code], s_edgeFace0u[code] * m, iu + i);
                    neighbourCodes[nc++] = EncodeFaceIndex(
                        s_edgeFace1[code], iu + i, s_edgeFace1v[code] * m);
                }
                return 8;
            }
            else
            {
                code -= m_cornerBasis;
                int m = (int)m_r2Sub1 - 1;
                switch (code)
                {
                    case 0:
                        neighbourCodes[0] = RawEncodeFaceIndex(0, 0, 0);
                        neighbourCodes[1] = RawEncodeFaceIndex(1, 0, 0);
                        neighbourCodes[2] = RawEncodeFaceIndex(2, 0, 0);
                        neighbourCodes[3] = RawEncodeEdgeIndex(0, 0);
                        neighbourCodes[4] = RawEncodeEdgeIndex(4, 0);
                        neighbourCodes[5] = RawEncodeEdgeIndex(8, 0);
                        break;
                    case 1:
                        neighbourCodes[0] = RawEncodeEdgeIndex(0, m);
                        neighbourCodes[1] = RawEncodeFaceIndex(1, 0, m);
                        neighbourCodes[2] = RawEncodeFaceIndex(2, m, 0);
                        neighbourCodes[3] = RawEncodeFaceIndex(3, 0, 0);
                        neighbourCodes[4] = RawEncodeEdgeIndex(5, 0);
                        neighbourCodes[5] = RawEncodeEdgeIndex(9, 0);
                        break;
                    case 2:
                        neighbourCodes[0] = RawEncodeFaceIndex(0, m, 0);
                        neighbourCodes[1] = RawEncodeEdgeIndex(4, m);
                        neighbourCodes[2] = RawEncodeFaceIndex(2, 0, m);
                        neighbourCodes[3] = RawEncodeEdgeIndex(1, 0);
                        neighbourCodes[4] = RawEncodeFaceIndex(4, 0, 0);
                        neighbourCodes[5] = RawEncodeEdgeIndex(10, 0);
                        break;
                    case 3:
                        neighbourCodes[0] = RawEncodeEdgeIndex(1, m);
                        neighbourCodes[1] = RawEncodeEdgeIndex(5, m);
                        neighbourCodes[2] = RawEncodeFaceIndex(2, m, m);
                        neighbourCodes[3] = RawEncodeFaceIndex(3, m, 0);
                        neighbourCodes[4] = RawEncodeFaceIndex(4, 0, m);
                        neighbourCodes[5] = RawEncodeEdgeIndex(11, 0);
                        break;
                    case 4:
                        neighbourCodes[0] = RawEncodeFaceIndex(0, 0, m);
                        neighbourCodes[1] = RawEncodeFaceIndex(1, m, 0);
                        neighbourCodes[2] = RawEncodeEdgeIndex(8, m);
                        neighbourCodes[3] = RawEncodeEdgeIndex(2, 0);
                        neighbourCodes[4] = RawEncodeEdgeIndex(6, 0);
                        neighbourCodes[5] = RawEncodeFaceIndex(5, 0, 0);
                        break;
                    case 5:
                        neighbourCodes[0] = RawEncodeEdgeIndex(2, m);
                        neighbourCodes[1] = RawEncodeFaceIndex(1, m, m);
                        neighbourCodes[2] = RawEncodeEdgeIndex(9, m);
                        neighbourCodes[3] = RawEncodeFaceIndex(3, 0, m);
                        neighbourCodes[4] = RawEncodeEdgeIndex(7, 0);
                        neighbourCodes[5] = RawEncodeFaceIndex(5, m, 0);
                        break;
                    case 6:
                        neighbourCodes[0] = RawEncodeFaceIndex(0, m, m);
                        neighbourCodes[1] = RawEncodeEdgeIndex(6, m);
                        neighbourCodes[2] = RawEncodeEdgeIndex(10, m);
                        neighbourCodes[3] = RawEncodeEdgeIndex(3, 0);
                        neighbourCodes[4] = RawEncodeFaceIndex(4, m, 0);
                        neighbourCodes[5] = RawEncodeFaceIndex(5, 0, m);
                        break;
                    case 7:
                        neighbourCodes[0] = RawEncodeEdgeIndex(3, m);
                        neighbourCodes[1] = RawEncodeEdgeIndex(7, m);
                        neighbourCodes[2] = RawEncodeEdgeIndex(11, m);
                        neighbourCodes[3] = RawEncodeFaceIndex(3, m, m);
                        neighbourCodes[4] = RawEncodeFaceIndex(4, m, m);
                        neighbourCodes[5] = RawEncodeFaceIndex(5, m, m);
                        break;
                }
                return 6;
            }
        }

        #endregion

        #region Info Methods

        /// <summary>
        /// Calculate the maximal raster for a given number of bits.
        /// Currently maximal 32 bits are supported.
        /// </summary>
        /// <param name="bits">The number of bits.</param>
        /// <returns>The maximal possible raster value.</returns>
        public static uint RasterForBits(int bits)
        {
            if (bits < 5) return 0;
            if (bits > 32) bits = 32;
            return s_rasterForBitsTable[bits];
        }

        /// <summary>
        /// Calculate the number of different codes that are available.
        /// </summary>
        /// <returns></returns>
        public uint Count
        {
            get { return m_cornerBasis + 8; }
        }
        
        /// <summary>
        /// Calculate all directions that are exactly encoded.
        /// </summary>
        /// <returns>An array of directions.</returns>
        public V3f[] GenerateTable()
        {
            uint dirCount = Count;
            V3f[] directionTable = new V3f[dirCount];
            for (uint dc = 0; dc < dirCount; dc++)
            {
                directionTable[dc] = Decode(dc);
            }
            return directionTable;
        }

        #endregion

        #region Code Generator

        /// <summary>
        /// Code generation.
        /// </summary>
        public void WriteCode(uint code)
        {
            if (code < m_edgeBasis) // face number is code
            {
                uint iu = code % m_r2Sub1;
                code /= m_r2Sub1;
                uint iv = code % m_r2Sub1;
                code /= m_r2Sub1;

                string u = iu == 0 ? "0" : (iu == m_r2Sub1 - 1 ? "m" : "u");
                string v = iv == 0 ? "0" : (iv == m_r2Sub1 - 1 ? "m" : "v");

                Console.WriteLine("EncodeFaceIndex({0}, {1}, {2});", code, u, v);
            }
            else if (code < m_cornerBasis)
            {
                code -= m_edgeBasis;
                uint iu = code % m_r2Sub1;
                code /= m_r2Sub1;      // edge number in code
                string u = iu == 0 ? "0" : (iu == m_r2Sub1 - 1 ? "m" : "u");
                Console.WriteLine("EncodeEdgeIndex({0}, {1});", code, u);
            }
            else
                Console.WriteLine("EncodeCornerIndex({0});", code - m_cornerBasis);
        }

        /// <summary>
        /// Code generation.
        /// </summary>
        public void WriteCornerNeighbours(uint corner)
        {
            V3f v = s_vecToCornerTableNonNormalized[corner];

            string f = "    neighbourCodes[{0}] = ";
            float s = (float)m_invDoubleRaster;
            Console.Write(f, 0); WriteCode(Encode(v + s * new V3f(-1, 0, 0)));
            Console.Write(f, 1); WriteCode(Encode(v + s * new V3f(0, -1, 0)));
            Console.Write(f, 2); WriteCode(Encode(v + s * new V3f(0, 0, -1)));
            Console.Write(f, 3); WriteCode(Encode(v + s * new V3f(1, 0, 0)));
            Console.Write(f, 4); WriteCode(Encode(v + s * new V3f(0, 1, 0)));
            Console.Write(f, 5); WriteCode(Encode(v + s * new V3f(0, 0, 1)));
        }

        /// <summary>
        /// Code generation.
        /// </summary>
        public void WriteAllCornerNeighbours()
        {
            for (uint c = 0; c < 8; c++)
            {
                Console.WriteLine("case {0}:", c);
                WriteCornerNeighbours(c);
                Console.WriteLine("    break;");
            }
        }

        #endregion
    }
}

using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class V3fCoderTests : TestSuite
    {
        public V3fCoderTests() : base() { }
        public V3fCoderTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestV3fCoder()
        {
            foreach (var bits in new[] { 5, 8, 12, 16, 20, 24, 32 })
                RasterTest(V3fCoder.RasterForBits(bits));
        }

        public void RasterTest(uint raster)
        {
            int iraster = (int)raster;

            V3fCoder coder = new V3fCoder(raster);

            uint step = 1;
            bool large = false;

            if (iraster > 52) { step = 7; large = true; }
            if (iraster > 591) step = 63;
            if (iraster > 6688) step = 2039;

            int bits = (int)Fun.Ceiling(Fun.Log2(coder.Count));

            Test.Begin("normal coder raster {0} ({1} bits)", iraster, bits);
            /*
            Console.WriteLine("  raster = {0}", m_raster);
            Console.WriteLine("  rasterMul2Sub1 = {0}", m_r2Sub1);
            Console.WriteLine("  doubleRaster = {0}", m_doubleRaster);
            Console.WriteLine("  invDoubleRaster = {0}", m_invDoubleRaster);
            Console.WriteLine("  edgeBasis = {0}", m_edgeBasis);
            Console.WriteLine("  cornerBasis = {0}", m_cornerBasis);
            */
            Test.Begin("testing {0} of {1} codes", 1 + ((long)coder.Count - 1) / (long)step, coder.Count);
            for (uint code = 0; code < coder.Count; code += step)
            {
                V3f dir = coder.Decode(code);
                uint newCode = coder.Encode(dir);
                Test.IsTrue(code == newCode);
            }
            Test.End();

            double minDot = 1.0;

            float eps = Constant<float>.PositiveTinyValue;

            float[] factorTable = { 1.0f - eps, 1.0f, 1.0f + eps };

            for (int sign = -1; sign < 2; sign += 2)
            {
                for (int axis = 0; axis < 3; axis++)
                {
                    float factor = factorTable[axis];

                    for (int xi = -2 * iraster; xi <= 2 * iraster; xi++)
                    {
                        if (large && (xi > 3 - 2 * iraster) && (xi < -3))
                            continue;
                        if (large && (xi < 2 * iraster - 3) && (xi > +3))
                            continue;
                        double x = (double)xi * factor / (2 * iraster);
                        #if (!V3FCODER_NO_WARP)
                        x = V3fCoder.SphericalOfBox(x);
                        #endif
                        for (int yi = -2 * iraster; yi <= 2 * iraster; yi++)
                        {
                            if (large && (yi > 3 - 2 * iraster) && (yi < -3))
                                continue;
                            if (large && (yi < 2 * iraster - 3) && (yi > +3))
                                continue;
                            double y = (double)yi * factor / (2 * iraster);

                            #if (!V3FCODER_NO_WARP)
                            y = V3fCoder.SphericalOfBox(y);
                            #endif
                            V3f n = new V3f(0, 0, 0); // init to make compiler h.
                            n[axis] = sign;
                            n[(axis + 1) % 3] = (float)x;
                            n[(axis + 2) % 3] = (float)y;

                            n.Normalize();

                            uint code = coder.Encode(n);
                            V3f newN = coder.Decode(code);

                            double newDot = V3f.Dot(n, newN);
                            if (newDot < minDot) minDot = newDot;
                        }
                    }
                }
            }
            double maxErr = System.Math.Acos(minDot) * 180.0 / System.Math.PI;
            Report.Line("maximal error {0:g4} degrees", maxErr);
            Test.End();
        }


        public void NeigbourTest(uint raster)
        {
            var coder = new V3fCoder(raster);
            for (uint code = 0; code < coder.Count; code++)
            {
                uint[] neighbours = new uint[8];
                V3f n = coder.DecodeOnCube(code, false);
                uint nCount = coder.NeighbourCodes(code, neighbours);

                float min = float.MaxValue;
                float max = float.MinValue;
                for (uint nc = 0; nc < nCount; nc++)
                {
                    V3f nv = coder.DecodeOnCube(neighbours[nc], false);
                    float diff = (nv - n).Length;
                    if (diff < min) min = diff;
                    if (diff > max) max = diff;
                }

                if (min < 0.000001) Console.WriteLine("min too small");
                if (max > 2.0 * min) Console.WriteLine("max too large");

                if (raster < 3)
                {
                    Console.WriteLine("code {0} min {1} max {2}",
                                      code, min, max);
                }

                min *= 0.99f;

                for (uint nc0 = 0; nc0 < nCount; nc0++)
                {
                    V3f nv0 = coder.DecodeOnCube(neighbours[nc0], false);
                    for (uint nc1 = nc0 + 1; nc1 < nCount; nc1++)
                    {
                        V3f nv1 = coder.DecodeOnCube(neighbours[nc1], false);
                        float diff = (nv1 - nv0).Length;
                        if (diff < min) Console.Write("neighbours too close");
                    }
                }
            }
        }

    }
}

using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.Geometry
{
    [InProcess]
    public class SolidAngle
    {
        public IRandomUniform _rnd = new RandomSystem();

        public int Count { get; set; } = 100000;

        private V3d[] a, b, c, large, small;
        private V3f[] af, bf, cf, largef, smallf;

        [GlobalSetup]
        public void Setup()
        {
            a = new V3d[Count]; b = new V3d[Count]; c = new V3d[Count];
            large = new V3d[Count]; small = new V3d[Count];
            af = new V3f[Count]; bf = new V3f[Count]; cf = new V3f[Count];
            largef = new V3f[Count]; smallf = new V3f[Count];
            _rnd.ReSeed(123);
            for (int j = 0; j < Count; j++)
            {
                a[j] = _rnd.UniformV3d(); b[j] = _rnd.UniformV3d(); c[j] = _rnd.UniformV3d();
                af[j] = (V3f)a[j]; bf[j] = (V3f)b[j]; cf[j] = (V3f)c[j];
                large[j] = a[j] * 1e200; small[j] = b[j] * 1e-200;
                largef[j] = af[j] * 1e20f; smallf[j] = bf[j] * 1e-20f;
            }
        }

        [Benchmark]
        public double Triangle_SolidAngle()
        {
            var sum = 0.0;
            for (int j = 0; j < Count; j++)
            {
                sum += Triangle.SolidAngle(a[j], b[j], c[j]);
            }
            return sum;

        }

        [Benchmark]
        public double Triangle_SolidAngle_Method2()
        {
            var sum = 0.0;
            for (int j = 0; j < Count; j++)
            {
                sum += CalcSolidAngle(a[j], b[j], c[j]);
            }
            return sum;
        }

        [Benchmark]
        public double Triangle_SolidAngle_Float()
        {
            double sum = 0;
            for (int j = 0; j < Count; j++) sum += Triangle.SolidAngle(af[j], bf[j], cf[j]);
            return sum;
        }

        [Benchmark]
        public double Triangle_SolidAngle_Extreme()
        {
            double sum = 0;
            for (int j = 0; j < Count; j++) sum += Triangle.SolidAngle(large[j], small[j], c[j]);
            return sum;
        }

        [Benchmark]
        public double Triangle_SolidAngle_Float_Extreme()
        {
            double sum = 0;
            for (int j = 0; j < Count; j++) sum += Triangle.SolidAngle(largef[j], smallf[j], cf[j]);
            return sum;
        }

        /// <summary>
        /// Numerically less stable version to calculate the solid angle of a triangle
        /// https://en.wikipedia.org/wiki/Solid_angle#Tetrahedron
        /// </summary>
        public static double CalcSolidAngle(V3d va, V3d vb, V3d vc)
        {
            var crossVaVc = Vec.Cross(va, vc).Normalized;
            var crossVaVb = Vec.Cross(va, vb).Normalized;
            var crossVbVc = Vec.Cross(vb, vc).Normalized;

            var alpha = Vec.Dot(crossVaVc, crossVaVb).AcosClamped();
            var betha = (-Vec.Dot(crossVaVb, crossVbVc)).AcosClamped();
            var gamma = Vec.Dot(crossVbVc, crossVaVc).AcosClamped();

            return alpha + betha + gamma - Constant.Pi;
        }
    }
}

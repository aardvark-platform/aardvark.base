using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.Geometry
{
    //BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.5737/22H2/2022Update)
    //AMD Ryzen 9 7900, 1 CPU, 24 logical and 12 physical cores
    //.NET SDK 8.0.408
    //  [Host] : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

    //Job = InProcess  Toolchain=InProcessEmitToolchain

    //| Method                      | Mean      | Error     | StdDev    |
    //|---------------------------- |----------:|----------:|----------:|
    //| Triangle_SolidAngle         |  6.148 ms | 0.0372 ms | 0.0329 ms |
    //| Triangle_SolidAngle_Method2 | 10.277 ms | 0.0184 ms | 0.0163 ms |

    [InProcess]
    public class SolidAngle
    {
        public IRandomUniform _rnd = new RandomSystem();

        public int Count { get; set; } = 100000;

        [Benchmark]
        public double Triangle_SolidAngle()
        {
            var sum = 0.0;
            _rnd.ReSeed(123);
            for (int j = 0; j < Count; j++)
            {
                var va = _rnd.UniformV3d();
                var vb = _rnd.UniformV3d();
                var vc = _rnd.UniformV3d();
                sum += Triangle.SolidAngle(va, vb, vc);
            }
            return sum;

        }

        [Benchmark]
        public double Triangle_SolidAngle_Method2()
        {
            var sum = 0.0;
            _rnd.ReSeed(123);
            for (int j = 0; j < Count; j++)
            {
                var va = _rnd.UniformV3d();
                var vb = _rnd.UniformV3d();
                var vc = _rnd.UniformV3d();
                sum += CalcSolidAngle(va, vb, vc);
            }
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

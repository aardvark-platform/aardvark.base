using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    //|                Method |     Mean |   Error |  StdDev | Code Size |
    //|---------------------- |---------:|--------:|--------:|----------:|
    //|  ApproximateEqDefault | 214.2 μs | 0.75 μs | 0.66 μs |     279 B |
    //|   ApproximateEqInline | 145.1 μs | 1.58 μs | 1.32 μs |     190 B |

    // After simplifying computation of machine epsilon:

    //| Method               | Mean     | Error    | StdDev   |
    //|--------------------- |---------:|---------:|---------:|
    //| ApproximateEqDefault | 84.80 us | 1.080 us | 1.010 us |
    //| ApproximateEqInline  | 83.66 us | 1.240 us | 1.160 us |

    [InProcess]
    public class ConstantsBenchmark
    {
        public Ray3d[] rays;

        public ConstantsBenchmark()
        {
            var rnd = new RandomSystem(1);
            rays = new Ray3d[100000].SetByIndex(i => new Ray3d(rnd.UniformV3d() * 2 - 1, rnd.UniformV3dDirection()));
        }

        [Benchmark]
        public int ApproximateEqDefault()
        {
            var a = rays;
            var cnt = 0;
            for (int i = 0; i < a.Length; i++)
                if (a[i].Origin.ApproximateEquals(a[i].Direction))
                    cnt++;
            return cnt;
        }

        [Benchmark]
        public int ApproximateEqInline()
        {
            var a = rays;
            var cnt = 0;
            for (int i = 0; i < a.Length; i++)
                if (a[i].Origin.ApproximateEquals(a[i].Direction, 1e-16))
                    cnt++;
            return cnt;
        }
    }
}

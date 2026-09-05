using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*CubicCurveBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class CubicCurveBenchmark
    {
        private const int EvaluationCount = 1024;

        private static readonly V3d s_p0 = new V3d(-2.0, 1.5, 0.25);
        private static readonly V3d s_p1 = new V3d(0.5, -1.0, 2.0);
        private static readonly V3d s_p2 = new V3d(3.0, 2.5, -0.75);
        private static readonly V3d s_p3 = new V3d(4.5, -2.0, 1.25);

        [Benchmark]
        public double CubicHermiteScalar()
        {
            double sum = 0.0;
            for (int i = 0; i < EvaluationCount; i++)
            {
                double t = i * (1.0 / (EvaluationCount - 1));
                sum += Ipol.CubicHermite.Eval(t, 1.25, -2.5, 0.75, -1.125);
            }
            return sum;
        }

        [Benchmark]
        public V3d CubicHermiteVector()
        {
            V3d sum = V3d.Zero;
            for (int i = 0; i < EvaluationCount; i++)
            {
                double t = i * (1.0 / (EvaluationCount - 1));
                sum += Ipol.CubicHermite.Eval(t, s_p1, s_p2, s_p0, s_p3);
            }
            return sum;
        }

        [Benchmark]
        public V3d CatmullRomVector()
        {
            V3d sum = V3d.Zero;
            for (int i = 0; i < EvaluationCount; i++)
            {
                double t = i * (1.0 / (EvaluationCount - 1));
                sum += Ipol.CatmullRom.Eval(t, s_p0, s_p1, s_p2, s_p3);
            }
            return sum;
        }

        [Benchmark]
        public V3d KochanekBartelsVector()
        {
            V3d sum = V3d.Zero;
            for (int i = 0; i < EvaluationCount; i++)
            {
                double t = i * (1.0 / (EvaluationCount - 1));
                sum += Ipol.KochanekBartels.Eval(t, s_p0, s_p1, s_p2, s_p3, 0.2, -0.35);
            }
            return sum;
        }
    }
}

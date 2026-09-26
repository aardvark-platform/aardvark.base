using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*PolynomialCubicRootsBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class PolynomialCubicRootsBenchmark
    {
        [Benchmark]
        public (double, double, double) PositiveDiscriminant()
            => Polynomial.RealRootsOfDepressed(1.0, 1.0);

        [Benchmark]
        public (double, double, double) ZeroDiscriminant()
            => Polynomial.RealRootsOfDepressed(-3.0, 2.0);

        [Benchmark]
        public (double, double, double) NegativeDiscriminant()
            => Polynomial.RealRootsOfDepressed(-1.0, 0.0);

        [Benchmark]
        public (double, double, double) NormedPositiveDiscriminant()
            => Polynomial.RealRootsOfNormed(0.0, 1.0, 1.0);

        [Benchmark]
        public (double, double, double) NormedZeroDiscriminant()
            => Polynomial.RealRootsOfNormed(0.0, -3.0, 2.0);

        [Benchmark]
        public (double, double, double) NormedNegativeDiscriminant()
            => Polynomial.RealRootsOfNormed(0.0, -1.0, 0.0);
    }
}

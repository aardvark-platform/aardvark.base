using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*LuFactorizationBenchmark*'
    [MemoryDiagnoser]
    public class LuFactorizationBenchmark
    {
        private static readonly M22d s_m22 = new M22d(
            4.0, 7.0,
            2.0, 6.0
        );

        private static readonly M33d s_m33 = new M33d(
            4.0, 1.0, 2.0,
            0.5, 5.0, 1.0,
            1.0, 0.25, 6.0
        );

        private static readonly M44d s_m44 = new M44d(
            5.0, 1.0, 0.5, 0.25,
            0.5, 6.0, 1.0, 0.5,
            1.0, 0.25, 7.0, 1.0,
            0.25, 0.5, 1.0, 8.0
        );

        private readonly double[,] m_managed =
        {
            { 4.0, 1.0, 2.0 },
            { 0.0, 5.0, 1.0 },
            { 0.0, 0.0, 6.0 }
        };

        private readonly double[] m_strided = new double[32];
        private readonly int[] m_managedPermutation = new int[3];
        private readonly int[] m_stridedPermutation = new int[3];

        [GlobalSetup]
        public void Setup()
        {
            const long offset = 3;
            const long columnStride = 2;
            const long rowStride = 11;

            m_strided[offset + 0 * rowStride + 0 * columnStride] = 4.0;
            m_strided[offset + 0 * rowStride + 1 * columnStride] = 1.0;
            m_strided[offset + 0 * rowStride + 2 * columnStride] = 2.0;
            m_strided[offset + 1 * rowStride + 0 * columnStride] = 0.0;
            m_strided[offset + 1 * rowStride + 1 * columnStride] = 5.0;
            m_strided[offset + 1 * rowStride + 2 * columnStride] = 1.0;
            m_strided[offset + 2 * rowStride + 0 * columnStride] = 0.0;
            m_strided[offset + 2 * rowStride + 1 * columnStride] = 0.0;
            m_strided[offset + 2 * rowStride + 2 * columnStride] = 6.0;
        }

        [Benchmark]
        public M22d FixedM22dInverse()
            => s_m22.LuInverse();

        [Benchmark]
        public M33d FixedM33dInverse()
            => s_m33.LuInverse();

        [Benchmark]
        public M44d FixedM44dInverse()
            => s_m44.LuInverse();

        [Benchmark]
        public bool ManagedFactorization()
            => m_managed.LuFactorize(m_managedPermutation);

        [Benchmark]
        public bool StridedFactorization()
            => m_strided.LuFactorize(3, 2, 11, m_stridedPermutation);
    }
}

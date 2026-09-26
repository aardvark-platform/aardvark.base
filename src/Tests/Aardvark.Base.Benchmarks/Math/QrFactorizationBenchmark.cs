using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*QrFactorizationBenchmark*'
    [MemoryDiagnoser]
    public class QrFactorizationBenchmark
    {
        private const long Offset = 3;
        private const long ColumnStride = 2;
        private const long RowStride = 11;

        private readonly double[,] m_managed = new double[3, 3];
        private readonly double[,] m_managedZeroPivot = new double[1, 2];
        private readonly double[] m_strided = new double[32];
        private readonly double[] m_stridedDiagonal = new double[3];

        [Benchmark]
        public double ManagedOrdinary()
        {
            SetOrdinary(m_managed);
            return m_managed.QrFactorize()[2];
        }

        [Benchmark]
        public double ManagedZeroPivot()
        {
            SetZeroPivot(m_managedZeroPivot);
            return m_managedZeroPivot.QrFactorize()[0];
        }

        [Benchmark]
        public double StridedOrdinary()
        {
            SetOrdinary(m_strided);
            m_strided.QrFactorize(Offset, ColumnStride, RowStride, 3, 3, m_stridedDiagonal);
            return m_stridedDiagonal[2];
        }

        [Benchmark]
        public double StridedZeroPivot()
        {
            SetZeroPivot(m_strided);
            m_strided.QrFactorize(Offset, ColumnStride, RowStride, 2, 1, m_stridedDiagonal);
            return m_stridedDiagonal[0];
        }

        private static void SetOrdinary(double[,] matrix)
        {
            matrix[0, 0] = 4.0; matrix[0, 1] = 1.0; matrix[0, 2] = 2.0;
            matrix[1, 0] = 0.5; matrix[1, 1] = 5.0; matrix[1, 2] = 1.0;
            matrix[2, 0] = 1.0; matrix[2, 1] = 0.25; matrix[2, 2] = 6.0;
        }

        private static void SetZeroPivot(double[,] matrix)
        {
            matrix[0, 0] = 0.0;
            matrix[0, 1] = 1.0;
        }

        private static void SetOrdinary(double[] matrix)
        {
            Set(matrix, 0, 0, 4.0); Set(matrix, 0, 1, 1.0); Set(matrix, 0, 2, 2.0);
            Set(matrix, 1, 0, 0.5); Set(matrix, 1, 1, 5.0); Set(matrix, 1, 2, 1.0);
            Set(matrix, 2, 0, 1.0); Set(matrix, 2, 1, 0.25); Set(matrix, 2, 2, 6.0);
        }

        private static void SetZeroPivot(double[] matrix)
        {
            Set(matrix, 0, 0, 0.0);
            Set(matrix, 0, 1, 1.0);
        }

        private static void Set(double[] matrix, long row, long col, double value)
            => matrix[Offset + row * RowStride + col * ColumnStride] = value;
    }
}

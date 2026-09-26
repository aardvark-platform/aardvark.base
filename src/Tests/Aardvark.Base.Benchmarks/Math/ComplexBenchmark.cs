using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*ComplexBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class ComplexBenchmark
    {
        private const int Count = 1024;

        private ComplexD[] m_leftD;
        private ComplexD[] m_rightD;
        private ComplexF[] m_leftF;
        private ComplexF[] m_rightF;

        [GlobalSetup]
        public void Setup()
        {
            m_leftD = new ComplexD[Count];
            m_rightD = new ComplexD[Count];
            m_leftF = new ComplexF[Count];
            m_rightF = new ComplexF[Count];

            for (int i = 0; i < Count; i++)
            {
                double ar = 0.25 + (i % 29) * 0.125;
                double ai = -1.75 + (i % 31) * 0.1;
                double br = 1.5 + (i % 23) * 0.075;
                double bi = -0.9 + (i % 19) * 0.08;
                m_leftD[i] = new ComplexD(ar, ai);
                m_rightD[i] = new ComplexD(br, bi);
                m_leftF[i] = new ComplexF((float)ar, (float)ai);
                m_rightF[i] = new ComplexF((float)br, (float)bi);
            }
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public double NormDouble()
        {
            double sum = 0.0;
            for (int i = 0; i < Count; i++) sum += m_leftD[i].Norm;
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float NormFloat()
        {
            float sum = 0.0f;
            for (int i = 0; i < Count; i++) sum += m_leftF[i].Norm;
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexD ReciprocalDouble()
        {
            ComplexD sum = ComplexD.Zero;
            for (int i = 0; i < Count; i++) sum += m_leftD[i].Reciprocal;
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexF ReciprocalFloat()
        {
            ComplexF sum = ComplexF.Zero;
            for (int i = 0; i < Count; i++) sum += m_leftF[i].Reciprocal;
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexD DivisionDouble()
        {
            ComplexD sum = ComplexD.Zero;
            for (int i = 0; i < Count; i++) sum += m_leftD[i] / m_rightD[i];
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexF DivisionFloat()
        {
            ComplexF sum = ComplexF.Zero;
            for (int i = 0; i < Count; i++) sum += m_leftF[i] / m_rightF[i];
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexD ScalarDivisionDouble()
        {
            ComplexD sum = ComplexD.Zero;
            for (int i = 0; i < Count; i++) sum += (i + 1.0) / m_rightD[i];
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexF ScalarDivisionFloat()
        {
            ComplexF sum = ComplexF.Zero;
            for (int i = 0; i < Count; i++) sum += (i + 1.0f) / m_rightF[i];
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexD SquareRootDouble()
        {
            ComplexD sum = ComplexD.Zero;
            for (int i = 0; i < Count; i++) sum += m_leftD[i].Sqrt();
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public ComplexF SquareRootFloat()
        {
            ComplexF sum = ComplexF.Zero;
            for (int i = 0; i < Count; i++) sum += m_leftF[i].Sqrt();
            return sum;
        }
    }
}

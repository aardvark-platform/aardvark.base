using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*MedianWindowBenchmark*'
    [MemoryDiagnoser]
    public class MedianWindowBenchmark
    {
        private const int ValuesPerInvoke = 4096;

        private MedianWindow m_window;
        private double[] m_values;

        [Params(3, 31, 127, 511)]
        public int Capacity { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            m_values = new double[ValuesPerInvoke];
            uint state = 0x9e3779b9U;
            for (int i = 0; i < m_values.Length; i++)
            {
                state = state * 1664525U + 1013904223U;
                m_values[i] = ((int)(state >> 8) - 8388608) * (1.0 / 128.0);
            }

            m_window = new MedianWindow(Capacity);
            for (int i = 0; i < Capacity; i++)
                m_window.Insert(m_values[i & (ValuesPerInvoke - 1)]);
        }

        [Benchmark(OperationsPerInvoke = ValuesPerInvoke)]
        public double Insert()
        {
            var window = m_window;
            var values = m_values;
            double result = 0.0;
            for (int i = 0; i < values.Length; i++)
                result += window.Insert(values[i]);
            return result;
        }
    }
}

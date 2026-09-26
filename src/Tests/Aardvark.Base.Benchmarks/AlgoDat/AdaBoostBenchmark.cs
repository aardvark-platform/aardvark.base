using BenchmarkDotNet.Attributes;
using System;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*AdaBoostBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class AdaBoostBenchmark
    {
        private const int QueryCount = 1024;
        private Func<int, bool> m_classifier;

        [GlobalSetup]
        public void Setup()
        {
            var items = new[] { 0, 1, 2, 3 };
            var groundTruth = new[] { false, false, true, true };
            Func<int, bool>[] learners =
            {
                value => value >= 1,
                value => value == 2,
                value => value == 3,
            };
            int index = 0;

            m_classifier = AdaBoost.Train(
                items,
                groundTruth,
                (_, _, _) => learners[index++],
                learners.Length);
        }

        [Benchmark]
        public int Classify()
        {
            int result = 0;
            for (int i = 0; i < QueryCount; i++)
                if (m_classifier(i & 3)) result++;
            return result;
        }
    }
}

using BenchmarkDotNet.Attributes;
using Half = Aardvark.Base.Half;

namespace Aardvark.Base.Benchmarks
{
    public enum HalfComparisonWorkload
    {
        Normal,
        Exceptional
    }

    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*HalfComparisonBenchmark*'
    [MemoryDiagnoser]
    public class HalfComparisonBenchmark
    {
        private const int Count = 1024;
        private readonly Half[] m_left = new Half[Count];
        private readonly Half[] m_right = new Half[Count];

        [Params(HalfComparisonWorkload.Normal, HalfComparisonWorkload.Exceptional)]
        public HalfComparisonWorkload Workload { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            if (Workload == HalfComparisonWorkload.Normal)
            {
                for (int i = 0; i < Count; i++)
                {
                    m_left[i] = (Half)((i * 37 % 2000 - 1000) * 0.03125f);
                    m_right[i] = (Half)((i * 53 % 2000 - 1000) * 0.03125f);
                }
            }
            else
            {
                ushort[] bits =
                {
                    0x0000, 0x8000, 0x7c00, 0xfc00,
                    0x7c01, 0x7e00, 0xfc01, 0xfe00
                };

                for (int i = 0; i < Count; i++)
                {
                    m_left[i] = Half.ToHalf(bits[i & 7]);
                    m_right[i] = Half.ToHalf(bits[(i * 5 + 3) & 7]);
                }
            }
        }

        [Benchmark]
        public int CompareBatch()
        {
            int result = 0;
            for (int i = 0; i < Count; i++)
            {
                Half left = m_left[i];
                Half right = m_right[i];
                if (left == right) result += 1;
                if (left != right) result += 2;
                if (left < right) result += 4;
                if (left > right) result += 8;
                if (left <= right) result += 16;
                if (left >= right) result += 32;
                if (left.Equals(right)) result += 64;
                result += left.CompareTo(right);
            }

            return result;
        }
    }
}

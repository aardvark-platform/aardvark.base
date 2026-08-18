using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks
{
    public enum CommonDivisorMultipleWorkload
    {
        Positive,
        MixedSign,
        Zero
    }

    internal static class CommonDivisorMultipleBenchmarkData
    {
        public const int Count = 1024;

        public static void Fill(int[] a, int[] b, CommonDivisorMultipleWorkload workload)
        {
            for (int i = 0; i < Count; i++)
            {
                int x = 1_000 + i * 37 % 20_000;
                int y = 1_000 + i * 53 % 20_000;

                switch (workload)
                {
                    case CommonDivisorMultipleWorkload.Positive:
                        a[i] = x;
                        b[i] = y;
                        break;
                    case CommonDivisorMultipleWorkload.MixedSign:
                        a[i] = (i & 1) == 0 ? -x : x;
                        b[i] = i % 3 == 0 ? -y : y;
                        break;
                    case CommonDivisorMultipleWorkload.Zero:
                        a[i] = (i & 1) == 0 ? 0 : x;
                        b[i] = (i & 1) == 0 ? y : 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(workload));
                }
            }
        }

        public static void Fill(long[] a, long[] b, CommonDivisorMultipleWorkload workload)
        {
            for (int i = 0; i < Count; i++)
            {
                long x = 1_000_000L + i * 37_003L % 20_000_000L;
                long y = 1_000_000L + i * 53_009L % 20_000_000L;

                switch (workload)
                {
                    case CommonDivisorMultipleWorkload.Positive:
                        a[i] = x;
                        b[i] = y;
                        break;
                    case CommonDivisorMultipleWorkload.MixedSign:
                        a[i] = (i & 1) == 0 ? -x : x;
                        b[i] = i % 3 == 0 ? -y : y;
                        break;
                    case CommonDivisorMultipleWorkload.Zero:
                        a[i] = (i & 1) == 0 ? 0 : x;
                        b[i] = (i & 1) == 0 ? y : 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(workload));
                }
            }
        }
    }

    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*CommonDivisorMultiple*'
    [MemoryDiagnoser]
    public class CommonDivisorMultipleIntBenchmark
    {
        private readonly int[] m_a = new int[CommonDivisorMultipleBenchmarkData.Count];
        private readonly int[] m_b = new int[CommonDivisorMultipleBenchmarkData.Count];

        [Params(CommonDivisorMultipleWorkload.Positive, CommonDivisorMultipleWorkload.MixedSign, CommonDivisorMultipleWorkload.Zero)]
        public CommonDivisorMultipleWorkload Workload { get; set; }

        [GlobalSetup]
        public void Setup()
            => CommonDivisorMultipleBenchmarkData.Fill(m_a, m_b, Workload);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int PreviousGreatestCommonDivisor(int a, int b)
            => b == 0 ? a : PreviousGreatestCommonDivisor(b, a % b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int PreviousLeastCommonMultiple(int a, int b)
            => a * b / PreviousGreatestCommonDivisor(a, b);

        [Benchmark(Baseline = true)]
        public int Previous()
        {
            int result = 0;
            for (int i = 0; i < m_a.Length; i++)
            {
                int a = m_a[i];
                int b = m_b[i];
                result += PreviousGreatestCommonDivisor(a, b);
                result += PreviousLeastCommonMultiple(a, b);
            }
            return result;
        }

        [Benchmark]
        public int Corrected()
        {
            int result = 0;
            for (int i = 0; i < m_a.Length; i++)
            {
                int a = m_a[i];
                int b = m_b[i];
                result += Fun.GreatestCommonDivisor(a, b);
                result += Fun.LeastCommonMultiple(a, b);
            }
            return result;
        }
    }

    [MemoryDiagnoser]
    public class CommonDivisorMultipleLongBenchmark
    {
        private readonly long[] m_a = new long[CommonDivisorMultipleBenchmarkData.Count];
        private readonly long[] m_b = new long[CommonDivisorMultipleBenchmarkData.Count];

        [Params(CommonDivisorMultipleWorkload.Positive, CommonDivisorMultipleWorkload.MixedSign, CommonDivisorMultipleWorkload.Zero)]
        public CommonDivisorMultipleWorkload Workload { get; set; }

        [GlobalSetup]
        public void Setup()
            => CommonDivisorMultipleBenchmarkData.Fill(m_a, m_b, Workload);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long PreviousGreatestCommonDivisor(long a, long b)
            => b == 0 ? a : PreviousGreatestCommonDivisor(b, a % b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long PreviousLeastCommonMultiple(long a, long b)
            => a * b / PreviousGreatestCommonDivisor(a, b);

        [Benchmark(Baseline = true)]
        public long Previous()
        {
            long result = 0;
            for (int i = 0; i < m_a.Length; i++)
            {
                long a = m_a[i];
                long b = m_b[i];
                result += PreviousGreatestCommonDivisor(a, b);
                result += PreviousLeastCommonMultiple(a, b);
            }
            return result;
        }

        [Benchmark]
        public long Corrected()
        {
            long result = 0;
            for (int i = 0; i < m_a.Length; i++)
            {
                long a = m_a[i];
                long b = m_b[i];
                result += Fun.GreatestCommonDivisor(a, b);
                result += Fun.LeastCommonMultiple(a, b);
            }
            return result;
        }
    }
}

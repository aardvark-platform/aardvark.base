using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*EnumerableVarianceBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class EnumerableVarianceBenchmark
    {
        private const int SmallCount = 16;
        private const int LargeCount = 4096;

        private readonly struct Sample
        {
            public readonly double Value;

            public Sample(double value)
            {
                Value = value;
            }
        }

        private sealed class Sequence<T> : IEnumerable<T>
        {
            private readonly T[] m_values;
            private readonly Func<T, T> m_project;

            public Sequence(T[] values, Func<T, T> project)
            {
                m_values = values;
                m_project = project;
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0; i < m_values.Length; i++)
                    yield return m_project(m_values[i]);
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static double Project(double value) => value;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static long Project(long value) => value;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Sample Project(Sample value) => value;

        private static readonly Func<double, double> s_doubleIdentity = Project;
        private static readonly Func<long, long> s_longIdentity = Project;
        private static readonly Func<Sample, Sample> s_sampleIdentity = Project;
        private static readonly Func<Sample, double> s_selector = sample => sample.Value;

        private Sequence<double> m_smallDoubles;
        private Sequence<double> m_largeDoubles;
        private Sequence<long> m_smallLongs;
        private Sequence<long> m_largeLongs;
        private Sequence<Sample> m_smallSamples;
        private Sequence<Sample> m_largeSamples;

        [GlobalSetup]
        public void Setup()
        {
            m_smallDoubles = new Sequence<double>(CreateDoubles(SmallCount), s_doubleIdentity);
            m_largeDoubles = new Sequence<double>(CreateDoubles(LargeCount), s_doubleIdentity);
            m_smallLongs = new Sequence<long>(CreateLongs(SmallCount), s_longIdentity);
            m_largeLongs = new Sequence<long>(CreateLongs(LargeCount), s_longIdentity);
            m_smallSamples = new Sequence<Sample>(CreateSamples(SmallCount), s_sampleIdentity);
            m_largeSamples = new Sequence<Sample>(CreateSamples(LargeCount), s_sampleIdentity);
        }

        private static double[] CreateDoubles(int count)
        {
            var result = new double[count];
            for (int i = 0; i < count; i++)
                result[i] = 1.0e12 + ((i * 17) % 101) * 0.125;
            return result;
        }

        private static long[] CreateLongs(int count)
        {
            var result = new long[count];
            for (int i = 0; i < count; i++)
                result[i] = long.MaxValue - ((i * 37L) % 1009L);
            return result;
        }

        private static Sample[] CreateSamples(int count)
        {
            var result = new Sample[count];
            for (int i = 0; i < count; i++)
                result[i] = new Sample(-1.0e12 + ((i * 29) % 127) * 0.0625);
            return result;
        }

        [Benchmark]
        public double DoubleSmall() => m_smallDoubles.Variance();

        [Benchmark]
        public double DoubleLarge() => m_largeDoubles.Variance();

        [Benchmark]
        public double LongSmall() => m_smallLongs.Variance();

        [Benchmark]
        public double LongLarge() => m_largeLongs.Variance();

        [Benchmark]
        public double SelectorSmall() => m_smallSamples.Variance(s_selector);

        [Benchmark]
        public double SelectorLarge() => m_largeSamples.Variance(s_selector);
    }
}

using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*FractionBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class FractionBenchmark
    {
        private const int Count = 1024;

        private Fraction[] m_ordinaryLeft;
        private Fraction[] m_ordinaryRight;
        private Fraction[] m_equivalentLeft;
        private Fraction[] m_equivalentRight;
        private Fraction[] m_tiedLeft;
        private Fraction[] m_tiedRight;
        private Fraction[] m_specialLeft;
        private Fraction[] m_specialRight;
        private Fraction[] m_addLeft;
        private Fraction[] m_addRight;

        [GlobalSetup]
        public void Setup()
        {
            m_ordinaryLeft = new Fraction[Count];
            m_ordinaryRight = new Fraction[Count];
            m_equivalentLeft = new Fraction[Count];
            m_equivalentRight = new Fraction[Count];
            m_tiedLeft = new Fraction[Count];
            m_tiedRight = new Fraction[Count];
            m_specialLeft = new Fraction[Count];
            m_specialRight = new Fraction[Count];
            m_addLeft = new Fraction[Count];
            m_addRight = new Fraction[Count];

            for (int i = 0; i < Count; i++)
            {
                m_ordinaryLeft[i] = new Fraction(i + 1, i + 17);
                m_ordinaryRight[i] = new Fraction(i + 5, i + 11);

                long numerator = i + 1;
                long denominator = i + 2;
                m_equivalentLeft[i] = new Fraction(numerator, denominator);
                m_equivalentRight[i] = new Fraction(numerator * 3, denominator * 3);

                long extreme = long.MaxValue - 4L * i;
                m_tiedLeft[i] = new Fraction(extreme, extreme - 1);
                m_tiedRight[i] = new Fraction(extreme - 1, extreme - 2);

                switch (i & 3)
                {
                    case 0:
                        m_specialLeft[i] = Fraction.NaN;
                        m_specialRight[i] = Fraction.NaN;
                        break;
                    case 1:
                        m_specialLeft[i] = Fraction.PositiveInfinity;
                        m_specialRight[i] = new Fraction(7, 0);
                        break;
                    case 2:
                        m_specialLeft[i] = Fraction.NegativeInfinity;
                        m_specialRight[i] = Fraction.PositiveInfinity;
                        break;
                    default:
                        m_specialLeft[i] = Fraction.PositiveInfinity;
                        m_specialRight[i] = new Fraction(i + 1, i + 2);
                        break;
                }

                m_addLeft[i] = new Fraction(i % 17 + 1, i % 23 + 2);
                m_addRight[i] = new Fraction(-(i % 13 + 1), i % 19 + 2);
            }
        }

        [Benchmark]
        public int OrdinaryOrdering()
        {
            int result = 0;
            for (int i = 0; i < Count; i++)
                if (m_ordinaryLeft[i] < m_ordinaryRight[i]) result++;
            return result;
        }

        [Benchmark]
        public int OrdinaryEquality()
        {
            int result = 0;
            for (int i = 0; i < Count; i++)
                if (m_ordinaryLeft[i] == m_ordinaryRight[i]) result++;
            return result;
        }

        [Benchmark]
        public int EquivalentComparisons()
        {
            int result = 0;
            for (int i = 0; i < Count; i++)
                if (m_equivalentLeft[i] == m_equivalentRight[i]) result++;
            return result;
        }

        [Benchmark]
        public int RoundedTieComparisons()
        {
            int result = 0;
            for (int i = 0; i < Count; i++)
            {
                if (m_tiedLeft[i] < m_tiedRight[i]) result++;
                if (m_tiedLeft[i] == m_tiedRight[i]) result += 3;
                if (m_tiedLeft[i] > m_tiedRight[i]) result += 7;
            }
            return result;
        }

        [Benchmark]
        public int SpecialValueOperators()
        {
            int result = 0;
            for (int i = 0; i < Count; i++)
            {
                if (m_specialLeft[i] == m_specialRight[i]) result++;
                if (m_specialLeft[i] != m_specialRight[i]) result += 3;
                if (m_specialLeft[i] < m_specialRight[i]) result += 7;
            }
            return result;
        }

        [Benchmark]
        public long NormalAddition()
        {
            long result = 0;
            for (int i = 0; i < Count; i++)
            {
                var sum = m_addLeft[i] + m_addRight[i];
                result += sum.Numerator + sum.Denominator;
            }
            return result;
        }
    }
}

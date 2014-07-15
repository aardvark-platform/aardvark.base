using System;
using System.Diagnostics;
using System.Linq;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        public class UtilizationView : IProbe<double>
        {
            private Stopwatch m_stopwatch = new Stopwatch();
            private Func<TimeSpan> m_baseValue;

            private UtilizationView(Func<TimeSpan> baseValue)
            {
                m_baseValue = baseValue;
                m_stopwatch.Start();

                Telemetry.OnReset += (s, e) =>
                {
                    m_stopwatch.Restart();
                };
            }

            public UtilizationView(IProbe<TimeSpan> time)
                : this(() => time.Value)
            {
            }

            public double Value
            {
                get
                {
                    return m_baseValue().Ticks / m_stopwatch.Elapsed.Ticks;
                }
            }
            public double ValueDouble { get { return Value; } }
        }

        public class RatePerSecondView : IProbe<double>
        {
            private IProbe m_counter;
            private Stopwatch m_stopwatch = new Stopwatch();
            private double m_lastValue;
            private long m_lastTicks;
            private double m_result;

            public RatePerSecondView(Telemetry.IProbe counter)
            {
                Requires.NotNull(counter);
                m_counter = counter;
                m_stopwatch.Start();
                m_lastValue = counter.ValueDouble;

                Telemetry.OnReset += (s, e) =>
                {
                    m_stopwatch.Restart();
                    m_lastValue = 0;
                    m_lastTicks = 0;
                    m_result = 0;
                };
            }

            /// <summary>
            /// Rate per second.
            /// </summary>
            public double Value
            {
                get
                {
                    var t = m_stopwatch.ElapsedTicks;
                    var dt = t - m_lastTicks;

                    if (dt > Stopwatch.Frequency)
                    {
                        var x = m_counter.ValueDouble;
                        var dx = x - m_lastValue;
                        m_lastTicks = t;
                        m_lastValue = x;
                        m_result = (dx * Stopwatch.Frequency) / (double)dt;
                    }

                    return m_result;
                }
            }
            public double ValueDouble { get { return Value; } }
        }

        public class RatioView : IProbe<double>
        {
            private Func<double> m_f;

            public RatioView(IProbe nominator, IProbe denominator)
            {
                Requires.NotNull(nominator, denominator);
                m_f = () => nominator.ValueDouble / denominator.ValueDouble;
            }

            public RatioView(IProbe nominator, double denominator)
            {
                Requires.NotNull(nominator);
                Requires.That(denominator != 0.0);
                m_f = () => nominator.ValueDouble / denominator;
            }

            /// <summary>
            /// Ratio.
            /// </summary>
            public double Value { get { return m_f(); } }
            public double ValueDouble { get { return Value; } }
        }

        public class SumView : IProbe<double>
        {
            private Func<double> m_f;

            public SumView(IProbe one, params IProbe[] others)
            {
                Requires.NotNull(one, others);
                m_f = () => one.ValueDouble + others.Sum(x => x.ValueDouble);
            }

            public SumView(IProbe one, params double[] others)
            {
                Requires.NotNull(one, others);
                var x = others.Sum();
                m_f = () => one.ValueDouble + x;
            }

            /// <summary>
            /// Ratio.
            /// </summary>
            public double Value { get { return m_f(); } }
            public double ValueDouble { get { return Value; } }
        }

        public class SubtractView : IProbe<double>
        {
            private Func<double> m_f;

            public SubtractView(IProbe one, params IProbe[] others)
            {
                Requires.NotNull(one, others);
                m_f = () => one.ValueDouble - others.Sum(x => x.ValueDouble);
            }

            public SubtractView(IProbe one, params double[] others)
            {
                Requires.NotNull(one, others);
                var x = others.Sum();
                m_f = () => one.ValueDouble - x;
            }

            /// <summary>
            /// Ratio.
            /// </summary>
            public double Value { get { return m_f(); } }
            public double ValueDouble { get { return Value; } }
        }

        public class MultiplyView : IProbe<double>
        {
            private Func<double> m_f;

            public MultiplyView(IProbe one, IProbe other)
            {
                Requires.NotNull(one, other);
                m_f = () => one.ValueDouble * other.ValueDouble;
            }

            public MultiplyView(IProbe one, double other)
            {
                Requires.NotNull(one);
                m_f = () => one.ValueDouble * other;
            }

            /// <summary>
            /// Ratio.
            /// </summary>
            public double Value { get { return m_f(); } }
            public double ValueDouble { get { return Value; } }
        }

        public class MinView : IProbe<double>
        {
            private Func<double> m_f;

            public MinView(IProbe one, params IProbe[] others)
            {
                Requires.NotNull(one, others);
                m_f = () => Math.Min(one.ValueDouble, others.Min(x => x.ValueDouble));
            }

            public MinView(IProbe one, params double[] others)
            {
                Requires.NotNull(one, others);
                var x = others.Min();
                m_f = () => Math.Min(one.ValueDouble, x);
            }

            /// <summary>
            /// Ratio.
            /// </summary>
            public double Value { get { return m_f(); } }
            public double ValueDouble { get { return Value; } }
        }

        public class MaxView : IProbe<double>
        {
            private Func<double> m_f;

            public MaxView(IProbe one, params IProbe[] others)
            {
                Requires.NotNull(one, others);
                m_f = () => Math.Max(one.ValueDouble, others.Max(x => x.ValueDouble));
            }

            public MaxView(IProbe one, params double[] others)
            {
                Requires.NotNull(one, others);
                var x = others.Max();
                m_f = () => Math.Max(one.ValueDouble, x);
            }

            /// <summary>
            /// Ratio.
            /// </summary>
            public double Value { get { return m_f(); } }
            public double ValueDouble { get { return Value; } }
        }

        public class AvgView : IProbe<double>
        {
            private Func<double> m_f;

            public AvgView(IProbe one, params IProbe[] others)
            {
                Requires.NotNull(one, others);
                double d = (1 + others.Length);
                m_f = () => (one.ValueDouble + others.Sum(x => x.ValueDouble)) / d;
            }

            public AvgView(IProbe one, params double[] others)
            {
                Requires.NotNull(one, others);
                var x = others.Sum();
                double d = (1 + others.Length);
                m_f = () => (one.ValueDouble + x) / d;
            }

            /// <summary>
            /// Ratio.
            /// </summary>
            public double Value { get { return m_f(); } }
            public double ValueDouble { get { return Value; } }
        }
    }
}

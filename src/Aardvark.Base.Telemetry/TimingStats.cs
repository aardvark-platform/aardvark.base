using System;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        public class TimingStats
        {
            private Type m_type;
            private long m_count;
            private TimeSpan m_current;
            private TimeSpan m_total;
            private TimeSpan m_min = TimeSpan.MaxValue;
            private TimeSpan m_max = TimeSpan.MinValue;
            private TimeSpan m_avg;

            public Type Type { get { return m_type; } }
            public long Count { get { return m_count; } }
            public TimeSpan Current { get { return m_current; } }
            public TimeSpan Total { get { return m_total; } }
            public TimeSpan Min { get { return m_min; } }
            public TimeSpan Max { get { return m_max; } }
            public TimeSpan Avg { get { return m_avg; } }

            public TimingStats(Type type)
            {
                if (type is null) throw new ArgumentNullException(nameof(type));
                m_type = type;
            }

            public void AddSample(TimeSpan x)
            {
                if (x < m_min) m_min = x;
                if (x > m_max) m_max = x;
                m_count++;
                m_current = x;
                m_total += x;
                double w = 1.0 / m_count;
                double a = m_avg.Ticks * (1.0 - w);
                double b = x.Ticks * w;
                m_avg = TimeSpan.FromTicks((long)(a + b));
            }
        }
    }
}

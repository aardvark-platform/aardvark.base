using System;

namespace Aardvark.Base
{
    public static partial class Telemetry
    {
        public class TimingStats
        {
            public Type Type { get; }
            public long Count { get; private set; }
            public TimeSpan Current { get; private set; }
            public TimeSpan Total { get; private set; }
            public TimeSpan Min { get; private set; } = TimeSpan.MaxValue;
            public TimeSpan Max { get; private set; } = TimeSpan.MinValue;
            public TimeSpan Avg { get; private set; }

            public TimingStats(Type type)
            {
                if (type is null) throw new ArgumentNullException(nameof(type));
                Type = type;
            }

            public void AddSample(TimeSpan x)
            {
                if (x < Min) Min = x;
                if (x > Max) Max = x;
                Count++;
                Current = x;
                Total += x;
                double w = 1.0 / Count;
                double a = Avg.Ticks * (1.0 - w);
                double b = x.Ticks * w;
                Avg = TimeSpan.FromTicks((long)(a + b));
            }
        }
    }
}

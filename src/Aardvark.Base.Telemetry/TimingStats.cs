/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;

namespace Aardvark.Base;

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

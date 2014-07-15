using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static class TelemetryExtensions
    {
        public static Telemetry.RatePerSecondView RatePerSecond(this Telemetry.IProbe self)
        {
            return new Telemetry.RatePerSecondView(self);
        }

        public static Telemetry.UtilizationView Utilization(this Telemetry.IProbe<TimeSpan> self)
        {
            return new Telemetry.UtilizationView(self);
        }

        public static Telemetry.RatioView Per(this Telemetry.IProbe self, Telemetry.IProbe denominator)
        {
            return new Telemetry.RatioView(self, denominator);
        }
        public static Telemetry.RatioView Per(this Telemetry.IProbe self, double denominator)
        {
            return new Telemetry.RatioView(self, denominator);
        }
        public static Telemetry.SumView Sum(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        {
            return new Telemetry.SumView(self, others);
        }
        public static Telemetry.SumView Sum(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        {
            return new Telemetry.SumView(self, others.ToArray());
        }
        public static Telemetry.SumView Sum(this Telemetry.IProbe self, Telemetry.IProbe value)
        {
            return new Telemetry.SumView(self, value);
        }
        public static Telemetry.SumView Sum(this Telemetry.IProbe self, double value)
        {
            return new Telemetry.SumView(self, value);
        }
        public static Telemetry.SumView Sum(this Telemetry.IProbe self, params double[] values)
        {
            return new Telemetry.SumView(self, values);
        }
        public static Telemetry.SumView Add(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        {
            return new Telemetry.SumView(self, others);
        }
        public static Telemetry.SumView Add(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        {
            return new Telemetry.SumView(self, others.ToArray());
        }
        public static Telemetry.SumView Add(this Telemetry.IProbe self, Telemetry.IProbe value)
        {
            return new Telemetry.SumView(self, value);
        }
        public static Telemetry.SumView Add(this Telemetry.IProbe self, double value)
        {
            return new Telemetry.SumView(self, value);
        }
        public static Telemetry.SumView Add(this Telemetry.IProbe self, params double[] values)
        {
            return new Telemetry.SumView(self, values);
        }
        public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        {
            return new Telemetry.SubtractView(self, others);
        }
        public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        {
            return new Telemetry.SubtractView(self, others.ToArray());
        }
        public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, Telemetry.IProbe value)
        {
            return new Telemetry.SubtractView(self, value);
        }
        public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, double value)
        {
            return new Telemetry.SubtractView(self, value);
        }
        public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, params double[] values)
        {
            return new Telemetry.SubtractView(self, values);
        }
        public static Telemetry.MultiplyView Multiply(this Telemetry.IProbe self, Telemetry.IProbe value)
        {
            return new Telemetry.MultiplyView(self, value);
        }
        public static Telemetry.MultiplyView Multiply(this Telemetry.IProbe self, double value)
        {
            return new Telemetry.MultiplyView(self, value);
        }
        public static Telemetry.MinView Min(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        {
            return new Telemetry.MinView(self, others);
        }
        public static Telemetry.MinView Min(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        {
            return new Telemetry.MinView(self, others.ToArray());
        }
        public static Telemetry.MinView Min(this Telemetry.IProbe self, Telemetry.IProbe value)
        {
            return new Telemetry.MinView(self, value);
        }
        public static Telemetry.MinView Min(this Telemetry.IProbe self, double value)
        {
            return new Telemetry.MinView(self, value);
        }
        public static Telemetry.MinView Min(this Telemetry.IProbe self, params double[] values)
        {
            return new Telemetry.MinView(self, values);
        }
        public static Telemetry.MaxView Max(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        {
            return new Telemetry.MaxView(self, others);
        }
        public static Telemetry.MaxView Max(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        {
            return new Telemetry.MaxView(self, others.ToArray());
        }
        public static Telemetry.MaxView Max(this Telemetry.IProbe self, Telemetry.IProbe value)
        {
            return new Telemetry.MaxView(self, value);
        }
        public static Telemetry.MaxView Max(this Telemetry.IProbe self, double value)
        {
            return new Telemetry.MaxView(self, value);
        }
        public static Telemetry.MaxView Max(this Telemetry.IProbe self, params double[] values)
        {
            return new Telemetry.MaxView(self, values);
        }
        public static Telemetry.AvgView Avg(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        {
            return new Telemetry.AvgView(self, others);
        }
        public static Telemetry.AvgView Avg(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        {
            return new Telemetry.AvgView(self, others.ToArray());
        }
        public static Telemetry.AvgView Avg(this Telemetry.IProbe self, Telemetry.IProbe value)
        {
            return new Telemetry.AvgView(self, value);
        }
        public static Telemetry.AvgView Avg(this Telemetry.IProbe self, double value)
        {
            return new Telemetry.AvgView(self, value);
        }
        public static Telemetry.AvgView Avg(this Telemetry.IProbe self, params double[] values)
        {
            return new Telemetry.AvgView(self, values);
        }

        public static Telemetry.IProbe<T> Snapshot<T>(this Telemetry.IProbe<T> probe)
        {
            if (typeof(T) == typeof(long)) return (Telemetry.IProbe<T>)
                new Telemetry.SnapshotProbeLong((Telemetry.IProbe<long>)probe);

            if (typeof(T) == typeof(double)) return (Telemetry.IProbe<T>)
                new Telemetry.SnapshotProbeDouble((Telemetry.IProbe<double>)probe);

            if (typeof(T) == typeof(TimeSpan)) return (Telemetry.IProbe<T>)
                new Telemetry.SnapshotProbeTimeSpan((Telemetry.IProbe<TimeSpan>)probe);

            throw new NotImplementedException(string.Format(
                "Telemetry.IProbe<{0}>.Snapshot() not implemented.", typeof(T).Name
                ));
        }
    }
}

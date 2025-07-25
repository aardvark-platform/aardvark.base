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
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base;

public static class TelemetryExtensions
{
    public static Telemetry.RatePerSecondView RatePerSecond(this Telemetry.IProbe self)
        => new(self);

    public static Telemetry.UtilizationView Utilization(this Telemetry.IProbe<TimeSpan> self)
        => new(self);

    public static Telemetry.RatioView Per(this Telemetry.IProbe self, Telemetry.IProbe denominator)
        => new(self, denominator);

    public static Telemetry.RatioView Per(this Telemetry.IProbe self, double denominator)
        => new(self, denominator);
    
    public static Telemetry.SumView Sum(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        => new(self, others);

    public static Telemetry.SumView Sum(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        => new(self, others.ToArray());

    public static Telemetry.SumView Sum(this Telemetry.IProbe self, Telemetry.IProbe value)
        => new(self, value);

    public static Telemetry.SumView Sum(this Telemetry.IProbe self, double value)
        => new(self, value);

    public static Telemetry.SumView Sum(this Telemetry.IProbe self, params double[] values)
        => new(self, values);
            
    public static Telemetry.SumView Add(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        => new(self, others);

    public static Telemetry.SumView Add(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        => new(self, others.ToArray());

    public static Telemetry.SumView Add(this Telemetry.IProbe self, Telemetry.IProbe value)
        => new(self, value);

    public static Telemetry.SumView Add(this Telemetry.IProbe self, double value)
        => new(self, value);

    public static Telemetry.SumView Add(this Telemetry.IProbe self, params double[] values)
        => new(self, values);
            
    public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        => new(self, others);

    public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        => new(self, others.ToArray());

    public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, Telemetry.IProbe value)
        => new(self, value);

    public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, double value)
        => new(self, value);

    public static Telemetry.SubtractView Subtract(this Telemetry.IProbe self, params double[] values)
        => new(self, values);
    
    public static Telemetry.MultiplyView Multiply(this Telemetry.IProbe self, Telemetry.IProbe value)
        => new(self, value);

    public static Telemetry.MultiplyView Multiply(this Telemetry.IProbe self, double value)
        => new(self, value);
            
    public static Telemetry.MinView Min(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        => new(self, others);

    public static Telemetry.MinView Min(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        => new(self, others.ToArray());

    public static Telemetry.MinView Min(this Telemetry.IProbe self, Telemetry.IProbe value)
        => new(self, value);

    public static Telemetry.MinView Min(this Telemetry.IProbe self, double value)
        => new(self, value);

    public static Telemetry.MinView Min(this Telemetry.IProbe self, params double[] values)
        => new(self, values);
    
    public static Telemetry.MaxView Max(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        => new(self, others);

    public static Telemetry.MaxView Max(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        => new(self, others.ToArray());

    public static Telemetry.MaxView Max(this Telemetry.IProbe self, Telemetry.IProbe value)
        => new(self, value);

    public static Telemetry.MaxView Max(this Telemetry.IProbe self, double value)
        => new(self, value);

    public static Telemetry.MaxView Max(this Telemetry.IProbe self, params double[] values)
        => new(self, values);
    
    public static Telemetry.AvgView Avg(this Telemetry.IProbe self, params Telemetry.IProbe[] others)
        => new(self, others);

    public static Telemetry.AvgView Avg(this Telemetry.IProbe self, IEnumerable<Telemetry.IProbe> others)
        => new(self, others.ToArray());

    public static Telemetry.AvgView Avg(this Telemetry.IProbe self, Telemetry.IProbe value)
        => new(self, value);

    public static Telemetry.AvgView Avg(this Telemetry.IProbe self, double value)
        => new(self, value);

    public static Telemetry.AvgView Avg(this Telemetry.IProbe self, params double[] values)
        => new(self, values);
    
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

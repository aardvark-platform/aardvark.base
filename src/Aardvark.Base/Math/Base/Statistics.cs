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

namespace Aardvark.Base;

#region Extremum<T>

/// <summary>
/// A structure that maintains the extreme Value and an associated Data field
/// as each value/data pair is added.
/// </summary>
public struct Extremum<T>(double value, T data)
{
    private double m_value = value;
    private T m_data = data;

    #region Constructor

    #endregion

    #region Constants

    public static readonly Extremum<T> EmptyMin
        = new(Constant<double>.ParseableMaxValue, default);
    public static readonly Extremum<T> EmptyMax
        = new(Constant<double>.ParseableMinValue, default);

    #endregion

    #region Properties

    public readonly double Value { get { return m_value; } }
    public readonly T Data { get { return m_data; } }

    #endregion

    #region Methods

    public void AddMin(double value)
    {
        if (value < m_value) { m_value = value; }
    }

    public void AddMax(double value)
    {
        if (value > m_value) { m_value = value; }
    }

    public void AddMin(double value, T data)
    {
        if (value < m_value) { m_value = value; m_data = data; }
    }

    public void AddMax(double value, T data)
    {
        if (value > m_value) { m_value = value; m_data = data; }
    }

    public static Extremum<T> Max(Extremum<T> e0, Extremum<T> e1)
    {
        return e0.m_value >= e1.m_value ? e0 : e1;
    }

    public static Extremum<T> Min(Extremum<T> e0, Extremum<T> e1)
    {
        return e0.m_value < e1.m_value ? e0 : e1;
    }

    #endregion
}

#endregion

#region StatsOptions

[Flags]
public enum StatsOptions
{
    Count = 0x0001,
    Sum = 0x0002,
    Mean = 0x0004,
    Min = 0x0010,
    Max = 0x0020,
    Range = 0x0030,
    Variance = 0x0100,
    StandardDeviation = 000200,
    NeedsSumOfSquares = 0x0300,
    All = 0x3fffffff,
    MaxMean = Max | Mean,
    MinMean = Min | Mean,
    RangeMean = Range | Mean,
    CountRange = Count | Range,
}

#endregion

#region Stats<T>

/// <summary>
/// A data structure for accumulating statistical moments. Currently only
/// Count/Sum/Min/Max/Mean/Variance is implemented. Extend as necessary.
/// </summary>
public struct Stats<T>(StatsOptions options) : IReportable
{
    long m_count = 0;
    KahanSum m_sum = KahanSum.Zero;
    readonly StatsOptions m_options = options;
    Extremum<T> m_min = Extremum<T>.EmptyMin;
    Extremum<T> m_max = Extremum<T>.EmptyMax;
    KahanSum m_sumOfSquares = KahanSum.Zero;

    #region Constructor

    #endregion

    #region Constants

    public static readonly Stats<T> ComputeCountRange
            = new(StatsOptions.Count | StatsOptions.Range);
    public static readonly Stats<T> ComputeRange
            = new(StatsOptions.Range);
    public static readonly Stats<T> ComputeMinMean
            = new(StatsOptions.Mean | StatsOptions.Min);
    public static readonly Stats<T> ComputeMaxMean
            = new(StatsOptions.Mean | StatsOptions.Max);
    public static readonly Stats<T> ComputeRangeMean
            = new(StatsOptions.Mean | StatsOptions.Range);
    public static readonly Stats<T> ComputeCountMaxMean
            = new(StatsOptions.Count | StatsOptions.Mean | StatsOptions.Max);
    public static readonly Stats<T> ComputeCountRangeMean
            = new(StatsOptions.Count | StatsOptions.Mean | StatsOptions.Range);
    public static readonly Stats<T> ComputeAll
            = new(StatsOptions.All);

    #endregion

    #region Properties

    public readonly long Count { get { return m_count; } }

    public readonly double Min { get { return m_min.Value; } }
    public readonly double Max { get { return m_max.Value; } }
    public readonly T MinData { get { return m_min.Data; } }
    public readonly T MaxData { get { return m_max.Data; } }

    public readonly double Sum { get { return m_sum.Value; } }
    public readonly double SumOfSquares { get { return m_sumOfSquares.Value; } }
    public readonly double Mean { get { return Sum / m_count; } }
    public readonly double Variance { get { return (SumOfSquares - Sum * Mean)/ m_count; } }
    public readonly double SampleVariance { get { return (SumOfSquares - Sum * Mean) / (m_count - 1); } }

    #endregion

    #region Methods

    public void Add(double value)
    {
        Add(value, default);
    }

    public void Add(double value, T data)
    {
        ++m_count;
        m_sum.Add(value);
        if ((m_options & StatsOptions.Min) != 0) m_min.AddMin(value, data);
        if ((m_options & StatsOptions.Max) != 0) m_max.AddMax(value, data);
        if ((m_options & StatsOptions.NeedsSumOfSquares) != 0)
            m_sumOfSquares.Add(Fun.Square(value));
    }

    public void Add(Stats<T> s)
    {
        if (s.m_options != m_options)
            throw new ArgumentException("need matching options for adding");

        m_count += s.m_count;
        m_sum.Add(s.m_sum);
        m_min = Extremum<T>.Min(m_min, s.m_min);
        m_max = Extremum<T>.Min(m_max, s.m_max);
        m_sumOfSquares.Add(s.m_sumOfSquares);
    }

    public static Stats<T> operator+(Stats<T> s0, Stats<T> s1)
    {
        if (s0.m_options != s1.m_options)
            throw new ArgumentException("need matching options for adding");

        return new Stats<T>(s0.m_options)
        {
            m_count = s0.m_count + s1.m_count,
            m_sum = s0.m_sum + s1.m_sum,
            m_min = Extremum<T>.Min(s0.m_min, s1.m_min),
            m_max = Extremum<T>.Min(s0.m_max, s1.m_max),
            m_sumOfSquares = s0.m_sumOfSquares + s1.m_sumOfSquares,
        };
    }

    #endregion

    #region IReportable Members

    public readonly void ReportValue(int verbosity, string name)
    {
        using (Report.Job(verbosity, name))
        {
            if ((m_options & StatsOptions.Count) != 0)
                Report.Value(verbosity, "count", Count);
            if ((m_options & StatsOptions.Sum) != 0)
                Report.Value(verbosity, "sum", Sum);
            if ((m_options & StatsOptions.Min) != 0)
                Report.Value(verbosity, "minimum", Min);
            if ((m_options & StatsOptions.Max) != 0)
                Report.Value(verbosity, "maximum", Max);
            if ((m_options & StatsOptions.Mean) != 0)
            {
                Report.Value(verbosity, "mean", Mean);
            }
            if ((m_options & StatsOptions.NeedsSumOfSquares) != 0)
            {
                double variance = Variance;
                if ((m_options & StatsOptions.Variance) != 0)
                    Report.Value(verbosity, "variance", variance);
                if ((m_options & StatsOptions.Variance) != 0)
                    Report.Value(verbosity, "standard deviation", Fun.Sqrt(variance));
            }
        }
    }

    #endregion

}

public class StatsClass<T>
{
    public Stats<T> Value;
}

#endregion

#region Histogram

/// <summary>
/// A data structure that maintains a histogram with a selectable number
/// of slots.
/// </summary>
public struct Histogram(Range1d range, int slotCount) : IReportable
{
    private Range1d m_slotRange = range;
    private readonly double m_scale = slotCount / (range.Max - range.Min);
    private long m_small = 0;
    private long m_large = 0;
    private Range1d m_dataRange = Range1d.Invalid;
    private readonly long[] m_histo = new long[slotCount];

    #region Constructor

    public Histogram(double min, double max, int slotCount)
        : this(new Range1d(min, max), slotCount)
    {
    }

    #endregion

    #region Properties

    public readonly Range1d DataRange { get { return m_dataRange; } }
    public readonly Range1d SlotRange { get { return m_slotRange; } }
    public readonly long SmallCount { get { return m_small; } }
    public readonly long LargeCount { get { return m_large; } }
    public readonly int SlotCount { get { return m_histo.Length; } }
    public readonly long this[int slot] { get { return m_histo[slot]; } }
    public readonly long[] Slots { get { return m_histo; } }

    #endregion

    #region Methods

    public void AddLog10(double value) { Add(Fun.Log10(value)); }

    public void AddLog2(double value) { Add(Fun.Log2(value)); }

    public void Add(double value)
    {
        m_dataRange.ExtendBy(value);
        if (value >= m_slotRange.Max) { ++m_large; return; }
        value -= m_slotRange.Min;
        if (value < 0.0) { ++m_small; return; }
        int slot = (int)(value * m_scale);
        if (slot >= m_histo.Length) slot = m_histo.Length - 1;
        m_histo[slot] += 1;
    }

    public void AddRange(IEnumerable<double> values)
    {
        foreach (var v in values)
            Add(v);
    }

    public void Clear()
    {
        m_small = 0;
        m_large = 0;
        m_dataRange = Range1d.Invalid;
        m_histo.Set(0);
    }

    public void Add(Histogram h)
    {
        if (m_dataRange != h.m_dataRange)
            throw new ArgumentException("adding histograms requires identical data ranges");
        if (m_scale != h.m_scale || m_histo.Length != h.m_histo.Length)
            throw new ArgumentException("adding histograms reuqires identical scales and slots");

        m_small += h.m_small;
        m_large += h.m_large;
        for (int i = 0; i < m_histo.Length; i++)
            m_histo[i] += h.m_histo[i];
    }

    public static Histogram operator +(Histogram h0, Histogram h1)
    {
        if (h0.m_dataRange != h1.m_dataRange)
            throw new ArgumentException("adding histograms requires identical data ranges");
        if (h0.m_scale != h1.m_scale || h0.m_histo.Length != h1.m_histo.Length)
            throw new ArgumentException("adding histograms reuqires identical scales and slots");

        var h = new Histogram(h0.m_dataRange, h0.SlotCount)
        {
            m_small = h0.m_small + h1.m_small,
            m_large = h0.m_large + h1.m_large
        };
        for (int i = 0; i < h.m_histo.Length; i++)
            h.m_histo[i] = h0.m_histo[i] + h1.m_histo[i];
        return h;
    }

    #endregion

    #region IReportable Members

    private static void ReportRange(
            int v, double min, double max, long count, bool outside)
    {
        string range = String.Format("[{0,9:g6} <= x < {1,-9:g6}]", min, max);
        if (outside)
            Report.Values(v, range, " ", count, "***");
        else
            Report.Value(v, range, count);
    }

    public readonly void ReportValue(int v, string name)
    {
        using (Report.Job(v, "{0}:", name))
        {
            if (m_small > 0) ReportRange(v, m_dataRange.Min, m_slotRange.Min, m_small, true);
            double min = m_slotRange.Min;
            double scale = (m_slotRange.Max - m_slotRange.Min) / (double)SlotCount;
            for (int s = 0; s < SlotCount; s++)
            {
                double max = m_slotRange.Min + (s + 1.0) * scale;
                ReportRange(v, min, max, m_histo[s], false);
                min = max;
            }
            if (m_large > 0) ReportRange(v, m_slotRange.Max, m_dataRange.Max, m_large, true);
        }
    }

    #endregion

}

public class HistogramClass
{
    public Histogram Value;
}

#endregion

#region HistogramAndStats<T>

public class HistogramAndStats<T>(Histogram histogram, Stats<T> stats)
{
    public Histogram Histogram = histogram;
    public Stats<T> Stats = stats;

    #region Constructors

    public HistogramAndStats(double min, double max, int steps, StatsOptions options)
        : this(new Histogram(min, max, steps), new Stats<T>(options))
    { }

    #endregion

    #region Operations

    public void AddLog10Hist(double value, T data)
    {
        Histogram.AddLog10(value);
        Stats.Add(value, data);
    }

    public void AddLog2Hist(double value, T data)
    {
        Histogram.AddLog2(value);
        Stats.Add(value, data);
    }

    public void Add(double value, T data)
    {
        Histogram.Add(value);
        Stats.Add(value, data);
    }

    public void Add(HistogramAndStats<T> hs)
    {
        Histogram.Add(hs.Histogram);
        Stats.Add(hs.Stats);
    }

    public static HistogramAndStats<T> operator +(HistogramAndStats<T> hs0, HistogramAndStats<T> hs1)
    {
        return new HistogramAndStats<T>(hs0.Histogram + hs1.Histogram, hs0.Stats + hs1.Stats);
    }

    #endregion
}

#endregion

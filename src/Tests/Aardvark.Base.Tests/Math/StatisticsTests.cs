using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public class StatisticsTests : TestSuite
    {
        private const StatsOptions MergeOptions =
            StatsOptions.Count |
            StatsOptions.Sum |
            StatsOptions.Mean |
            StatsOptions.Range |
            StatsOptions.Variance |
            StatsOptions.StandardDeviation;

        private static Stats<string> CreateStats(params (double Value, string Data)[] values)
        {
            var stats = new Stats<string>(MergeOptions);
            foreach (var value in values)
                stats.Add(value.Value, value.Data);
            return stats;
        }

        private static void AssertStatsEqual(Stats<string> expected, Stats<string> actual)
        {
            Assert.That(actual.Count, Is.EqualTo(expected.Count));
            Assert.That(actual.Sum, Is.EqualTo(expected.Sum).Within(1e-12));
            Assert.That(actual.SumOfSquares, Is.EqualTo(expected.SumOfSquares).Within(1e-12));
            Assert.That(actual.Mean, Is.EqualTo(expected.Mean).Within(1e-12));
            Assert.That(actual.Variance, Is.EqualTo(expected.Variance).Within(1e-12));
            Assert.That(actual.Min, Is.EqualTo(expected.Min));
            Assert.That(actual.MinData, Is.EqualTo(expected.MinData));
            Assert.That(actual.Max, Is.EqualTo(expected.Max));
            Assert.That(actual.MaxData, Is.EqualTo(expected.MaxData));
        }

        private static string CaptureReport<T>(Stats<T> stats)
        {
            var previousRootTarget = Report.RootTarget;
            var output = new StringBuilder();
            var target = new TextLogTarget((threadIndex, type, level, message) => output.Append(message))
            {
                Verbosity = int.MaxValue,
                LogCompleteLinesOnly = true,
            };
            target.PrefixFun = _ => "";

            try
            {
                Report.RootTarget = target;
                stats.ReportValue(0, "stats");
            }
            finally
            {
                target.Dispose();
                Report.RootTarget = previousRootTarget;
            }

            return output.ToString();
        }

        private static Histogram CreateLeftHistogram()
        {
            var histogram = new Histogram(0.0, 4.0, 4);
            histogram.Add(-2.0);
            histogram.Add(0.5);
            histogram.Add(1.5);
            return histogram;
        }

        private static Histogram CreateRightHistogram()
        {
            var histogram = new Histogram(0.0, 4.0, 4);
            histogram.Add(2.5);
            histogram.Add(3.5);
            histogram.Add(6.0);
            return histogram;
        }

        private static void AssertMergedHistogram(Histogram histogram)
        {
            Assert.That(histogram.SlotRange, Is.EqualTo(new Range1d(0.0, 4.0)));
            Assert.That(histogram.SlotCount, Is.EqualTo(4));
            Assert.That(histogram.DataRange, Is.EqualTo(new Range1d(-2.0, 6.0)));
            Assert.That(histogram.SmallCount, Is.EqualTo(1));
            Assert.That(histogram.LargeCount, Is.EqualTo(1));
            Assert.That(histogram.Slots, Is.EqualTo(new long[] { 1, 1, 1, 1 }));
        }

        [Test]
        public void StatsMergeApisMatchSequentialAccumulation()
        {
            var leftValues = new[]
            {
                (Value: 2.0, Data: "left-two"),
                (Value: -1.0, Data: "left-min"),
                (Value: 7.0, Data: "left-max"),
            };
            var rightValues = new[]
            {
                (Value: 4.0, Data: "right-four"),
                (Value: -3.0, Data: "right-min"),
                (Value: 11.0, Data: "right-max"),
            };

            var expected = CreateStats(
                leftValues[0], leftValues[1], leftValues[2],
                rightValues[0], rightValues[1], rightValues[2]
            );
            var left = CreateStats(leftValues);
            var right = CreateStats(rightValues);

            var added = left;
            added.Add(right);
            var summed = left + right;

            AssertStatsEqual(expected, added);
            AssertStatsEqual(expected, summed);
        }

        [Test]
        public void StatsMergeApisPreserveLeftDataForTiedExtrema()
        {
            var left = CreateStats((1.0, "left-min"), (9.0, "left-max"));
            var right = CreateStats((1.0, "right-min"), (9.0, "right-max"));

            var added = left;
            added.Add(right);
            var summed = left + right;

            Assert.That(added.MinData, Is.EqualTo("left-min"));
            Assert.That(added.MaxData, Is.EqualTo("left-max"));
            Assert.That(summed.MinData, Is.EqualTo("left-min"));
            Assert.That(summed.MaxData, Is.EqualTo("left-max"));
        }

        [Test]
        [NonParallelizable]
        public void VarianceAndStandardDeviationOptionsCollectAndReportIndependently()
        {
            Assert.That((int)StatsOptions.StandardDeviation, Is.EqualTo(0x0200));
            Assert.That(
                StatsOptions.NeedsSumOfSquares,
                Is.EqualTo(StatsOptions.Variance | StatsOptions.StandardDeviation)
            );

            var variance = new Stats<object>(StatsOptions.Variance);
            variance.Add(1.0);
            variance.Add(3.0);

            var standardDeviation = new Stats<object>(StatsOptions.StandardDeviation);
            standardDeviation.Add(1.0);
            standardDeviation.Add(3.0);

            Assert.That(variance.SumOfSquares, Is.EqualTo(10.0));
            Assert.That(standardDeviation.SumOfSquares, Is.EqualTo(10.0));

            var varianceReport = CaptureReport(variance);
            Assert.That(varianceReport, Does.Contain("variance"));
            Assert.That(varianceReport, Does.Not.Contain("standard deviation"));

            var standardDeviationReport = CaptureReport(standardDeviation);
            Assert.That(standardDeviationReport, Does.Not.Contain("variance"));
            Assert.That(standardDeviationReport, Does.Contain("standard deviation"));
        }

        [Test]
        public void HistogramMergeApisUnionObservedRangesAndPreserveBins()
        {
            var summed = CreateLeftHistogram() + CreateRightHistogram();

            var added = CreateLeftHistogram();
            added.Add(CreateRightHistogram());

            AssertMergedHistogram(summed);
            AssertMergedHistogram(added);
        }

        [Test]
        public void HistogramAddRejectsIncompatibleBinsWithoutMutation()
        {
            var histogram = new Histogram(0.0, 4.0, 4);
            histogram.Add(-1.0);
            histogram.Add(2.0);
            histogram.Add(5.0);

            var shifted = new Histogram(1.0, 5.0, 4);
            shifted.Add(-1.0);
            shifted.Add(2.0);
            shifted.Add(5.0);

            var differentCount = new Histogram(0.0, 4.0, 5);
            differentCount.Add(-1.0);
            differentCount.Add(2.0);
            differentCount.Add(5.0);

            var slotRange = histogram.SlotRange;
            var dataRange = histogram.DataRange;
            var smallCount = histogram.SmallCount;
            var largeCount = histogram.LargeCount;
            var slots = (long[])histogram.Slots.Clone();

            Assert.Throws<ArgumentException>(() => histogram.Add(shifted));
            Assert.Throws<ArgumentException>(() => histogram.Add(differentCount));
            Assert.Throws<ArgumentException>(() => { _ = histogram + shifted; });

            Assert.That(histogram.SlotRange, Is.EqualTo(slotRange));
            Assert.That(histogram.DataRange, Is.EqualTo(dataRange));
            Assert.That(histogram.SmallCount, Is.EqualTo(smallCount));
            Assert.That(histogram.LargeCount, Is.EqualTo(largeCount));
            Assert.That(histogram.Slots, Is.EqualTo(slots));
        }
    }
}

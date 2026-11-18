using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.Versioning;
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

        private sealed class HistogramApi
        {
            internal readonly string Name;
            internal readonly Type Type, Combined;
            internal readonly ConstructorInfo Constructor, CombinedConstructor;
            internal readonly MethodInfo Add, AddRange, CombinedAdd;
            internal readonly object Options;

            internal HistogramApi(string target)
            {
                Name = target;
                string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required actual core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                Type = assembly.GetType("Aardvark.Base.Histogram", true);
                Combined = assembly.GetType("Aardvark.Base.HistogramAndStats`1", true).MakeGenericType(typeof(int));
                var options = assembly.GetType("Aardvark.Base.StatsOptions", true);
                Options = Enum.Parse(options, "CountRange");
                Constructor = Type.GetConstructor(new[] { typeof(double), typeof(double), typeof(int) });
                CombinedConstructor = Combined.GetConstructor(new[] { typeof(double), typeof(double), typeof(int), options });
                Add = Type.GetMethod("Add", new[] { typeof(double) });
                AddRange = Type.GetMethod("AddRange", new[] { typeof(IEnumerable<double>) });
                CombinedAdd = Combined.GetMethod("Add", new[] { typeof(double), typeof(int) });
                Assert.That(Constructor, Is.Not.Null, Name);
                Assert.That(CombinedConstructor, Is.Not.Null, Name);
                foreach (var method in new[] { Add, AddRange, CombinedAdd })
                {
                    Assert.That(method, Is.Not.Null, Name);
                    Assert.That(method.ReturnType, Is.EqualTo(typeof(void)), Name);
                    Assert.That(method.DeclaringType.Assembly, Is.SameAs(assembly), Name);
                }
                Assert.That(Type.GetProperty("Slots").PropertyType, Is.EqualTo(typeof(long[])), Name);
                Assert.That(Type.GetProperty("DataRange").PropertyType.Assembly, Is.SameAs(assembly), Name);
                Assert.That(Combined.GetField("Histogram").FieldType, Is.EqualTo(Type), Name);
            }
            internal object Property(object value, string name) => value.GetType().GetProperty(name).GetValue(value);
        }

        // Exact integers in units of the minimum subnormal: no reciprocal or floating-point bin formula.
        private static BigInteger DyadicUnits(double value)
        {
            long bits = BitConverter.DoubleToInt64Bits(value);
            int exponent = (int)((bits >> 52) & 2047);
            var significand = new BigInteger((bits & 0x000fffffffffffffL) | (exponent == 0 ? 0 : 0x0010000000000000L));
            var units = significand << Math.Max(0, exponent - 1);
            return bits < 0 ? -units : units;
        }

        private static void CheckHistogram(HistogramApi api, double min, double max, int count, double[] values, bool exact)
        {
            var bins = new long[count];
            long small = 0, large = 0;
            var observed = Range1d.Invalid;
            var statsRange = Range1d.Invalid;
            int completed = 0;
            Type expectedException = null;
            try
            {
                foreach (double input in values)
                {
                    observed.ExtendBy(input);
                    if (input >= max) ++large;
                    else if (input - min < 0) ++small;
                    else
                    {
                        int bin = exact ? (int)((DyadicUnits(input) - DyadicUnits(min)) * count / (DyadicUnits(max) - DyadicUnits(min)))
                            : unchecked((int)((input - min) * (count / (max - min))));
                        if (bin >= count) bin = count - 1;
                        bins[bin]++;
                    }
                    statsRange.ExtendBy(input);
                    ++completed;
                }
            }
            catch (IndexOutOfRangeException e) { expectedException = e.GetType(); }

            foreach (int mode in new[] { 0, 1, 2 })
            {
                string context = $"{api.Name}: [{min:R},{max:R}), slots={count}, mode={mode}, samples={values.Length}, exact={exact}";
                object combined = mode == 2 ? api.CombinedConstructor.Invoke(new[] { (object)min, max, count, api.Options }) : null;
                object histogram = mode == 2 ? null : api.Constructor.Invoke(new object[] { min, max, count });
                Type actualException = null;
                try
                {
                    if (mode == 1) api.AddRange.Invoke(histogram, new object[] { values });
                    else for (int i = 0; i < values.Length; i++)
                        if (mode == 2) api.CombinedAdd.Invoke(combined, new object[] { values[i], i });
                        else api.Add.Invoke(histogram, new object[] { values[i] });
                }
                catch (TargetInvocationException e) { actualException = e.InnerException.GetType(); }
                Assert.That(actualException, Is.EqualTo(expectedException), context + " exception");
                if (mode == 2)
                {
                    histogram = api.Combined.GetField("Histogram").GetValue(combined);
                    var stats = api.Combined.GetField("Stats").GetValue(combined);
                    Assert.That(api.Property(stats, "Count"), Is.EqualTo((long)completed), context + " forwarded count");
                    Assert.That(api.Property(stats, "Min"), Is.EqualTo(statsRange.Min), context + " forwarded minimum");
                    Assert.That(api.Property(stats, "Max"), Is.EqualTo(statsRange.Max), context + " forwarded maximum");
                }
                Assert.That(api.Property(histogram, "Slots"), Is.EqualTo(bins), context + " bins");
                Assert.That(api.Property(histogram, "SlotCount"), Is.EqualTo(count), context);
                Assert.That(api.Property(histogram, "SmallCount"), Is.EqualTo(small), context + " below range");
                Assert.That(api.Property(histogram, "LargeCount"), Is.EqualTo(large), context + " half-open upper endpoint");
                var range = api.Property(histogram, "DataRange");
                Assert.That(range.GetType().GetField("Min").GetValue(range), Is.EqualTo(observed.Min), context + " observed minimum");
                Assert.That(range.GetType().GetField("Max").GetValue(range), Is.EqualTo(observed.Max), context + " observed maximum");
                range = api.Property(histogram, "SlotRange");
                Assert.That(range.GetType().GetField("Min").GetValue(range), Is.EqualTo(min), context + " slot minimum");
                Assert.That(range.GetType().GetField("Max").GetValue(range), Is.EqualTo(max), context + " slot maximum");
            }
        }

        private static double[] HistogramSamples(double min, double max, int count, bool grid)
        {
            double width = max - min;
            var values = new List<double> { Math.BitDecrement(min), min, Math.BitIncrement(min) };
            if (grid) for (int i = 1; i < 128; i++) values.Add(min + width * (i / 128.0));
            else for (int i = 0; i < count; i++) values.Add(min + width * ((i + 0.5) / count));
            values.Add(Math.BitDecrement(max)); values.Add(max); values.Add(Math.BitIncrement(max));
            return values.ToArray();
        }

        [Test]
        public void HistogramTinyDyadicRangesKeepRepresentableBins()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                var api = new HistogramApi(target);
                double requiredWidth = Math.ScaleB(1, -1030);
                var midpoints = new double[8];
                for (int i = 0; i < midpoints.Length; i++) midpoints[i] = requiredWidth * ((i + 0.5) / 8);
                CheckHistogram(api, 0, requiredWidth, 8, midpoints, true);
                foreach (int exponent in new[] { -1074, -1073, -1072, -1070, -1060, -1040, -1030, -1023, -1022, -1021, -1019, -1015, -1000 })
                foreach (int count in new[] { 1, 2, 3, 8, 17, 64 })
                {
                    double width = Math.ScaleB(1, exponent);
                    foreach (double min in new[] { 0.0, -width, Math.ScaleB(1, -1022), -Math.ScaleB(1, -1022) })
                        if (double.IsPositiveInfinity(count / width))
                            CheckHistogram(api, min, min + width, count, HistogramSamples(min, min + width, count, true), true);
                }
            }
        }

        [Test]
        public void HistogramReciprocalOverflowNeighborsKeepEndpointsAndForwarders()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                var api = new HistogramApi(target);
                foreach (int count in new[] { 1, 2, 3, 8, 17, 64 })
                {
                    double edge = count / double.MaxValue;
                    foreach (double width in new[] { Math.BitDecrement(Math.BitDecrement(edge)), Math.BitDecrement(edge), edge,
                        Math.BitIncrement(edge), Math.BitIncrement(Math.BitIncrement(edge)) })
                    foreach (double min in new[] { 0.0, -width, Math.ScaleB(1, -1022), -Math.ScaleB(1, -1022) })
                        CheckHistogram(api, min, min + width, count, HistogramSamples(min, min + width, count, false), true);
                }
            }
        }

        [Test]
        public void HistogramFiniteScaleAndInvalidRangePathsStayLiteral()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                var api = new HistogramApi(target);
                foreach (var range in new[] { (0.0, 1.0, 8), (-5.0, 11.0, 17), (0.0, Math.ScaleB(1, -1000), 64),
                    (-double.MaxValue, double.MaxValue, 8), (0.0, double.MaxValue, 8) })
                {
                    var random = new Random(203173);
                    var values = new double[1024];
                    for (int i = 0; i < values.Length; i++) values[i] = range.Item1 * (1 - i / 1024.0) + range.Item2 * (i / 1024.0);
                    CheckHistogram(api, range.Item1, range.Item2, range.Item3, values, false);
                    CheckHistogram(api, range.Item1, range.Item2, range.Item3,
                        HistogramSamples(range.Item1, range.Item2, range.Item3, false), false);
                    for (int i = 0; i < values.Length; i++) values[i] = (random.NextDouble() - 0.25) * 16;
                    CheckHistogram(api, range.Item1, range.Item2, range.Item3, values, false);
                }
                foreach (double value in new[] { double.NegativeInfinity, -0.0, 0.0, double.PositiveInfinity, double.NaN })
                    CheckHistogram(api, 0, Math.ScaleB(1, -1030), 8, new[] { value }, false);
                foreach (var range in new[] { (0.0, 0.0, 8), (2.0, 1.0, 8), (double.NaN, 1.0, 8), (0.0, double.NaN, 8),
                    (double.NegativeInfinity, double.PositiveInfinity, 8), (0.0, double.PositiveInfinity, 8),
                    (double.NegativeInfinity, 0.0, 8), (1.0, 1.0, 0), (0.0, 1.0, 0) })
                foreach (double value in new[] { double.NegativeInfinity, -1, -0.0, 0.0, double.Epsilon, 0.5, 1, 2, double.PositiveInfinity, double.NaN })
                    CheckHistogram(api, range.Item1, range.Item2, range.Item3, new[] { value }, false);
            }
        }
    }
}

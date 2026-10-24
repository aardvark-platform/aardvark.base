using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FunTests : TestSuite
    {
        public FunTests() : base() { }
        public FunTests(TestSuite.Options options) : base(options) { }

        private sealed class SingleUseEnumerable<T> : IEnumerable<T>
        {
            private readonly T[] m_values;
            private bool m_wasEnumerated;

            public int EnumerationCount { get; private set; }

            public SingleUseEnumerable(params T[] values)
            {
                m_values = values;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (m_wasEnumerated)
                    throw new InvalidOperationException("The sequence was enumerated more than once.");

                m_wasEnumerated = true;
                EnumerationCount++;
                return ((IEnumerable<T>)m_values).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private static double ExpectedBinaryEntropy(double positiveWeight, double negativeWeight)
        {
            var total = positiveWeight + negativeWeight;
            var positive = positiveWeight / total;
            var negative = negativeWeight / total;
            return -positive * Math.Log(positive, 2.0) - negative * Math.Log(negative, 2.0);
        }

        [Test]
        public static void AngleDistanceTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 1000; i++)
            {
                var alpha = rnd.UniformDouble() * Constant.PiTimesTwo;
                var delta = (rnd.UniformDouble() - 0.5) * 2.0 * Constant.Pi;
                var beta = alpha + (delta + Constant.PiTimesTwo * rnd.UniformInt(10));

                var dist = alpha.AngleDistance(beta);
                Assert.AreEqual(dist, Fun.Abs(delta), 1e-8);
            }
        }

        [Test]
        public static void AngleDifferenceTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 1000; i++)
            {
                var alpha = rnd.UniformDouble() * Constant.PiTimesTwo * rnd.UniformInt(10);
                var delta = (rnd.UniformDouble() - 0.5) * Constant.Pi;
                if (delta == Constant.Pi)
                    delta = -Constant.Pi;

                var beta = alpha + (delta + Constant.PiTimesTwo * rnd.UniformInt(10));

                var dist = alpha.AngleDifference(beta);
                Assert.AreEqual(dist, delta, 1e-8);
            }
        }

        [Test]
        public void MinMaxVarArg()
        {
            var rnd = new RandomSystem(1);
            var a = rnd.UniformInt();
            var b = rnd.UniformInt();
            var c = rnd.UniformInt();
            var d = rnd.UniformInt();
            var e = rnd.UniformInt();

            var min = Fun.Min(a, b, c, d, e);
            var min_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(min, min_ref, "Min not equal to reference");

            var max = Fun.Max(a, b, c, d, e);
            var max_ref = Fun.Max(Fun.Max(Fun.Max(Fun.Max(a, b), c), d), e);
            Assert.AreEqual(max, max_ref, "Max not equal to reference");
        }

        [Test]
        public void MinMaxVecVarArg()
        {
            var rnd = new RandomSystem(1);
            var a = rnd.UniformV4i();
            var b = rnd.UniformV4i();
            var c = rnd.UniformV4i();
            var d = rnd.UniformV4i();
            var e = rnd.UniformV4i();

            var min = Fun.Min(a, b, c, d, e);
            var min_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(min, min_ref, "Min not equal to reference");

            var max = Fun.Max(a, b, c, d, e);
            var max_ref = Fun.Max(Fun.Max(Fun.Max(Fun.Max(a, b), c), d), e);
            Assert.AreEqual(max, max_ref, "Max not equal to reference");
        }

        [Test]
        public void EntropyEnumeratesInputOnce()
        {
            var values = new SingleUseEnumerable<int>(1, 1, 2, 2);

            Assert.AreEqual(1.0, values.Entropy(), 1e-12);
            Assert.AreEqual(1, values.EnumerationCount);
        }

        [Test]
        public void EntropyHandlesEmptyAllEqualNullAndMixedDistributions()
        {
            Assert.AreEqual(0.0, Array.Empty<int>().Entropy(), 0.0);
            Assert.AreEqual(0.0, new[] { 7, 7, 7 }.Entropy(), 1e-12);
            Assert.AreEqual(1.0, new[] { "a", "a", "b", "b" }.Entropy(), 1e-12);
            Assert.AreEqual(1.5, new string[] { null, null, "x", "y" }.Entropy(), 1e-12);
        }

        [Test]
        public void WeightedBipartiteEntropyNormalizesByTotalWeight()
        {
            var classes = new[] { true, false };

            Assert.AreEqual(1.0, classes.Entropy(new[] { 2.0, 2.0 }), 1e-12);
            Assert.AreEqual(ExpectedBinaryEntropy(3.0, 1.0), classes.Entropy(new[] { 3.0, 1.0 }), 1e-12);
        }

        [Test]
        public void WeightedBipartiteEntropyIsScaleInvariant()
        {
            var classes = new[] { true, false };

            var entropy = classes.Entropy(new[] { 3.0, 1.0 });
            var scaledEntropy = classes.Entropy(new[] { 30.0, 10.0 });

            Assert.AreEqual(entropy, scaledEntropy, 1e-12);
        }

        [Test]
        public void WeightedBipartiteEntropyReturnsZeroForZeroClassTotals()
        {
            Assert.AreEqual(0.0, new[] { true, true }.Entropy(new[] { 2.0, 5.0 }), 0.0);
            Assert.AreEqual(0.0, new[] { false, false }.Entropy(new[] { 2.0, 5.0 }), 0.0);
            Assert.AreEqual(0.0, new[] { true, false }.Entropy(new[] { 0.0, 5.0 }), 0.0);
            Assert.AreEqual(0.0, new[] { true, false }.Entropy(new[] { 5.0, 0.0 }), 0.0);
        }

        private static IEnumerable<Assembly> EntropyCores()
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            yield return typeof(Fun).Assembly;
            yield return Assembly.LoadFile(path);
        }

        private static Func<bool[], double[], double> WeightedEntropy(Assembly core)
            => (Func<bool[], double[], double>)core.GetType("Aardvark.Base.Fun", true)!
                .GetMethod("Entropy", new[] { typeof(bool[]), typeof(double[]) })!
                .CreateDelegate(typeof(Func<bool[], double[], double>));

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static double LegacyWeightedEntropy(bool[] xs, double[] weights)
        {
            if (weights == null) throw new ArgumentNullException("weights");
            if (xs == null) throw new ArgumentNullException("xs");
            if (weights.Length != xs.Length) throw new ArgumentException("weights.Length != xs.Length");
            double positive = 0, negative = 0;
            for (int i = 0; i < xs.Length; i++) { if (xs[i]) positive += weights[i]; else negative += weights[i]; }
            if (positive == 0 || negative == 0) return 0;
            double total = positive + negative, p = positive / total, n = negative / total;
            double hp = -Math.Log(p, 2), hn = -Math.Log(n, 2);
            return p * hp + n * hn;
        }

        private static double SmallProportionEntropy(bool[] classes, int[] units)
        {
            long positive = 0, negative = 0;
            for (int i = 0; i < classes.Length; i++) { if (classes[i]) positive += units[i]; else negative += units[i]; }
            return positive == 0 || negative == 0 ? 0 : ExpectedBinaryEntropy(positive, negative);
        }

        [Test]
        public void WeightedEntropyRescalesClassAndCombinedOverflow()
        {
            foreach (var core in EntropyCores())
            {
                var entropy = WeightedEntropy(core);
                void Check(bool[] classes, int[] units, string context)
                {
                    double expected = SmallProportionEntropy(classes, units);
                    int upperExponent = 1023 - Math.ILogB(units.Max());
                    foreach (int exponent in new[] { upperExponent - 2, upperExponent - 1, upperExponent })
                    for (int order = 0; order < 3; order++)
                    for (int complement = 0; complement < 2; complement++)
                    {
                        var indices = Enumerable.Range(0, classes.Length).Select(i => order == 0 ? i : order == 1 ? classes.Length - i - 1 : (i + 1) % classes.Length).ToArray();
                        var labels = indices.Select(i => classes[i] ^ (complement != 0)).ToArray();
                        var weights = indices.Select(i => Math.ScaleB(units[i], exponent)).ToArray();
                        Assert.That(weights.All(double.IsFinite), Is.True, context);
                        Assert.That(entropy(labels, weights), Is.EqualTo(expected).Within(2e-14),
                            $"{core.Location}: {context}, exponent={exponent}, order={order}, complement={complement}, proportions={string.Join(",", units)}");
                    }
                }
                Check(new[] { true, false }, new[] { 1, 1 }, "combined balanced");
                Check(new[] { true, false }, new[] { 3, 1 }, "combined 3:1");
                Check(new[] { true, false }, new[] { 31, 1 }, "combined 31:1");
                Check(new[] { true, true, false }, new[] { 1, 1, 1 }, "within one class");
                Check(new[] { true, true, false, false }, new[] { 1, 1, 1, 1 }, "within both classes");
                Check(new[] { true, false, true, false, true, false }, new[] { 0, 1, 3, 0, 1, 7 }, "zero weights");
                Check(new[] { true, true, true }, new[] { 1, 2, 3 }, "single class early return");
                for (int n = 2; n <= 8; n++)
                {
                    var labels = Enumerable.Range(0, n).Select(i => i < n - 1).ToArray();
                    var weights = Enumerable.Repeat(double.MaxValue, n).ToArray();
                    Assert.That(entropy(labels, weights), Is.EqualTo(ExpectedBinaryEntropy(n - 1, 1)).Within(2e-14), $"{core.Location}: repeated maximum n={n}");
                }
                const int seed = 84179;
                var random = new Random(seed);
                for (int sample = 0; sample < 512; sample++)
                {
                    int n = random.Next(2, 66);
                    var labels = Enumerable.Range(0, n).Select(i => random.Next(2) == 0).ToArray();
                    var units = Enumerable.Range(0, n).Select(i => random.Next(32)).ToArray();
                    labels[0] = true; labels[1] = false; units[0] = random.Next(1, 32); units[1] = random.Next(1, 32);
                    Check(labels, units, $"seed={seed}, sample={sample}, n={n}");
                }
            }
        }

        [Test]
        public void WeightedEntropySkipsRoundedZeroFallbackContributions()
        {
            foreach (var core in EntropyCores())
            {
                var entropy = WeightedEntropy(core);
                var rows = new[]
                {
                    (small: double.Epsilon, large: double.MaxValue, expected: 0.0),
                    (small: Math.ScaleB(1, -51), large: Math.ScaleB(1, 1023), expected: 0.0),
                    (small: Math.ScaleB(1, -50), large: Math.ScaleB(1, 1023), expected: ExpectedBinaryEntropy(2 * double.Epsilon, 2))
                };
                foreach (var row in rows)
                for (int tinyPosition = 0; tinyPosition < 3; tinyPosition++)
                for (int complement = 0; complement < 2; complement++)
                {
                    var labels = new[] { false, false, false, true }; labels[tinyPosition] = true;
                    if (complement != 0) labels = labels.Select(x => !x).ToArray();
                    var weights = new[] { row.large, row.large, row.large, -0.0 }; weights[tinyPosition] = row.small;
                    double actual = entropy(labels, weights);
                    Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(row.expected)),
                        $"{core.Location}: small={row.small:R}, large={row.large:R}, tiny position={tinyPosition}, complement={complement}");
                }
            }
        }

        [Test]
        public void WeightedEntropyKeepsValidationAndLegacyWeightPolicies()
        {
            foreach (var core in EntropyCores())
            {
                var entropy = WeightedEntropy(core);
                Assert.That(Assert.Throws<ArgumentNullException>(() => entropy(null!, null!))!.ParamName, Is.EqualTo("weights"), core.Location);
                Assert.That(Assert.Throws<ArgumentNullException>(() => entropy(Array.Empty<bool>(), null!))!.ParamName, Is.EqualTo("weights"), core.Location);
                Assert.That(Assert.Throws<ArgumentNullException>(() => entropy(null!, Array.Empty<double>()))!.ParamName, Is.EqualTo("xs"), core.Location);
                foreach (var lengths in new[] { (0, 1), (1, 0), (2, 3), (3, 2) })
                    Assert.That(Assert.Throws<ArgumentException>(() => entropy(new bool[lengths.Item1], new double[lengths.Item2]))!.Message,
                        Is.EqualTo("weights.Length != xs.Length"), core.Location);
                void Check(bool[] labels, double[] weights, string context)
                {
                    var savedLabels = (bool[])labels.Clone(); var savedWeights = weights.Select(BitConverter.DoubleToInt64Bits).ToArray();
                    double expected = LegacyWeightedEntropy(labels, weights), actual = entropy(labels, weights);
                    if (double.IsNaN(expected)) Assert.That(double.IsNaN(actual), Is.True, $"{core.Location}: {context}");
                    else Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), $"{core.Location}: {context}");
                    Assert.That(labels, Is.EqualTo(savedLabels), context);
                    Assert.That(weights.Select(BitConverter.DoubleToInt64Bits), Is.EqualTo(savedWeights), context);
                }
                Check(Array.Empty<bool>(), Array.Empty<double>(), "empty");
                Check(new[] { true, false }, new[] { double.Epsilon, double.MaxValue }, "ordinary rounded probability remains legacy NaN");
                Check(new[] { true, false }, new[] { -1.0, -3.0 }, "negative class totals");
                Check(new[] { true, true, false }, new[] { -1.0, 1.0, double.NaN }, "zero class before invalid input");
                const int seed = 84203;
                var random = new Random(seed);
                for (int sample = 0; sample < 2048; sample++)
                {
                    int n = random.Next(66), exponent = random.Next(-100, 101);
                    var labels = Enumerable.Range(0, n).Select(i => random.Next(2) == 0).ToArray();
                    var weights = Enumerable.Range(0, n).Select(i => Math.ScaleB(random.Next(32), exponent)).ToArray();
                    Check(labels, weights, $"ordinary seed={seed}, sample={sample}");
                }
                var invalid = new[] { -1.0, -double.MaxValue, double.PositiveInfinity, double.NegativeInfinity,
                    BitConverter.Int64BitsToDouble(0x7ff8000000001234), BitConverter.Int64BitsToDouble(unchecked((long)0xfff8000000005678)) };
                foreach (double bad in invalid)
                for (int position = 0; position < 4; position++)
                for (int mask = 0; mask < 16; mask++)
                foreach (bool overflow in new[] { false, true })
                {
                    var labels = Enumerable.Range(0, 4).Select(i => (mask & (1 << i)) != 0).ToArray();
                    var weights = overflow ? new[] { double.MaxValue, double.MaxValue, double.MaxValue, 1.0 } : new[] { 0.0, -0.0, 1.0, 2.0 };
                    weights[position] = bad;
                    Check(labels, weights, $"invalid={bad:R}, position={position}, mask={mask}, overflow={overflow}");
                }
                var fun = core.GetType("Aardvark.Base.Fun", true)!;
                var binary = (Func<bool[], double>)fun.GetMethods().Single(m => m.Name == "Entropy" && m.IsGenericMethodDefinition && m.GetParameters()[0].ParameterType == typeof(bool[]))
                    .MakeGenericMethod(typeof(int)).CreateDelegate(typeof(Func<bool[], double>));
                var sequence = (Func<IEnumerable<int>, double>)fun.GetMethods().Single(m => m.Name == "Entropy" && m.IsGenericMethodDefinition && m.GetParameters()[0].ParameterType.IsGenericType)
                    .MakeGenericMethod(typeof(int)).CreateDelegate(typeof(Func<IEnumerable<int>, double>));
                Assert.That(binary(new[] { true, false, true, false }), Is.EqualTo(1), core.Location);
                Assert.That(binary(Array.Empty<bool>()), Is.Zero, core.Location);
                Assert.That(sequence(new[] { 1, 1, 2, 2 }), Is.EqualTo(1), core.Location);
                Assert.That(sequence(Array.Empty<int>()), Is.Zero, core.Location);
            }
        }

        [Test]
        public void VarianceEnumeratesDirectAndSelectedInputOnce()
        {
            var direct = new SingleUseEnumerable<double>(1.0, 2.0, 3.0, 4.0);
            Assert.AreEqual(1.25, direct.Variance(), 1e-12);
            Assert.AreEqual(1, direct.EnumerationCount);

            var selected = new SingleUseEnumerable<int>(1, 2, 3, 4);
            int selectorCalls = 0;
            Assert.AreEqual(1.25, selected.Variance(x => { selectorCalls++; return x; }), 1e-12);
            Assert.AreEqual(1, selected.EnumerationCount);
            Assert.AreEqual(4, selectorCalls);

            var selectedDeviation = new SingleUseEnumerable<int>(1, 2, 3, 4);
            selectorCalls = 0;
            Assert.AreEqual(Math.Sqrt(1.25), selectedDeviation.StandardDeviation(x => { selectorCalls++; return x; }), 1e-12);
            Assert.AreEqual(1, selectedDeviation.EnumerationCount);
            Assert.AreEqual(4, selectorCalls);
        }

        [Test]
        public void GeneratedNumericVarianceOverloadsReturnPopulationMoments()
        {
            const double expectedVariance = 1.25;
            double expectedDeviation = Math.Sqrt(expectedVariance);

            Assert.AreEqual(expectedVariance, new[] { 1, 2, 3, 4 }.Variance(), 1e-12);
            Assert.AreEqual(expectedDeviation, new[] { 1, 2, 3, 4 }.StandardDeviation(), 1e-12);

            Assert.AreEqual(expectedVariance, new long[] { 1, 2, 3, 4 }.Variance(), 1e-12);
            Assert.AreEqual(expectedDeviation, new long[] { 1, 2, 3, 4 }.StandardDeviation(), 1e-12);

            Assert.AreEqual(expectedVariance, new[] { 1.0f, 2.0f, 3.0f, 4.0f }.Variance(), 1e-12);
            Assert.AreEqual(expectedDeviation, new[] { 1.0f, 2.0f, 3.0f, 4.0f }.StandardDeviation(), 1e-12);

            Assert.AreEqual(expectedVariance, new[] { 1.0, 2.0, 3.0, 4.0 }.Variance(), 1e-12);
            Assert.AreEqual(expectedDeviation, new[] { 1.0, 2.0, 3.0, 4.0 }.StandardDeviation(), 1e-12);
        }

        [Test]
        public void VarianceRetainsSmallMomentsAtLargeDoubleOffsets()
        {
            var values = new[] { 1.0e16, 1.0e16, 1.0e16 + 2.0, 1.0e16 + 2.0 };

            Assert.AreEqual(1.0, values.Variance(), 0.0);
            Assert.AreEqual(1.0, values.StandardDeviation(), 0.0);
        }

        [Test]
        public void LongVarianceHandlesEqualAdjacentAndOppositeExtremes()
        {
            Assert.AreEqual(0.0, new[] { long.MaxValue, long.MaxValue }.Variance(), 0.0);
            Assert.AreEqual(0.0, new[] { long.MinValue, long.MinValue }.Variance(), 0.0);
            Assert.AreEqual(0.25, new[] { long.MaxValue - 1, long.MaxValue }.Variance(), 0.0);
            Assert.AreEqual(0.25, new[] { long.MinValue, long.MinValue + 1 }.Variance(), 0.0);

            double oppositeExtremeVariance = Math.Pow(2.0, 126.0);
            Assert.AreEqual(oppositeExtremeVariance, new[] { long.MinValue, long.MaxValue }.Variance(), 0.0);
            Assert.AreEqual(Math.Pow(2.0, 63.0), new[] { long.MinValue, long.MaxValue }.StandardDeviation(), 0.0);
        }

        [Test]
        public void VarianceDefinesEmptySingletonAndNonFiniteInputs()
        {
            Assert.IsTrue(double.IsNaN(Array.Empty<int>().Variance()));
            Assert.IsTrue(double.IsNaN(Array.Empty<long>().Variance()));
            Assert.IsTrue(double.IsNaN(Array.Empty<float>().Variance()));
            Assert.IsTrue(double.IsNaN(Array.Empty<double>().Variance()));
            Assert.IsTrue(double.IsNaN(Array.Empty<double>().StandardDeviation()));

            int selectorCalls = 0;
            Assert.IsTrue(double.IsNaN(Array.Empty<int>().Variance(x => { selectorCalls++; return x; })));
            Assert.AreEqual(0, selectorCalls);

            Assert.AreEqual(0.0, new[] { 7 }.Variance(), 0.0);
            Assert.AreEqual(0.0, new[] { long.MinValue }.Variance(), 0.0);
            Assert.AreEqual(0.0, new[] { 7.0f }.Variance(), 0.0);
            Assert.AreEqual(0.0, new[] { 7.0 }.Variance(), 0.0);
            Assert.AreEqual(0.0, new[] { 7.0 }.StandardDeviation(), 0.0);

            selectorCalls = 0;
            Assert.AreEqual(0.0, new[] { 7 }.StandardDeviation(x => { selectorCalls++; return x; }), 0.0);
            Assert.AreEqual(1, selectorCalls);

            Assert.IsTrue(double.IsNaN(new[] { double.NaN }.Variance()));
            Assert.IsTrue(double.IsNaN(new[] { double.PositiveInfinity }.Variance()));
            Assert.IsTrue(double.IsNaN(new[] { 1.0, double.NegativeInfinity, 2.0 }.Variance()));
            Assert.IsTrue(double.IsNaN(new[] { double.PositiveInfinity }.StandardDeviation()));

            selectorCalls = 0;
            Assert.IsTrue(double.IsNaN(new[] { 1, 2, 3 }.Variance(x =>
            {
                selectorCalls++;
                return x == 2 ? double.PositiveInfinity : x;
            })));
            Assert.AreEqual(3, selectorCalls);
        }

        private static double NextAfter(double input, int dir)
            => BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(input) + dir);

        private static float NextAfter(float input, int dir)
            => BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(input) + dir);

        private static float SincFloatImpl(float x)
        {
            return MathF.Sin(ConstantF.Pi * x) / (ConstantF.Pi * x);
        }

        private static double SincDoubleImpl(double x)
        {
            return Math.Sin(Constant.Pi * x) / (Constant.Pi * x);
        }

        private static string FormatBits(float x)
        {
            return $"0x{BitConverter.SingleToInt32Bits(x):X8}";
        }

        private static string FormatBits(double x)
        {
            return $"0x{BitConverter.DoubleToInt64Bits(x):X16}";
        }

        private static float FindFirstFloatSincNotOne(float startInclusive, int maxUlps)
        {
            var x = startInclusive;

            for (var i = 0; i <= maxUlps; i++)
            {
                if (SincFloatImpl(x) != 1.0f)
                    return x;

                x = NextAfter(x, 1);
            }

            Assert.Fail(
                "Could not find float x with raw sinc(x) != 1 within {0} ULPs from x = {1:R} ({2})",
                maxUlps,
                startInclusive,
                FormatBits(startInclusive)
            );

            return default;
        }

        private static double FindFirstDoubleSincNotOne(double startInclusive, int maxUlps)
        {
            var x = startInclusive;

            for (var i = 0; i <= maxUlps; i++)
            {
                if (SincDoubleImpl(x) != 1.0)
                    return x;

                x = NextAfter(x, 1);
            }

            Assert.Fail(
                "Could not find double x with raw sinc(x) != 1 within {0} ULPs from x = {1:R} ({2})",
                maxUlps,
                startInclusive,
                FormatBits(startInclusive)
            );

            return default;
        }

        [Test]
        public void SincTest()
        {
            const int searchUlps = 4096;
            const int verifyUlps = 256;

            // Keep the current implementation cutoffs in sync with Fun.Sinc.
            {
                const float cutoff = 0.00017791694f;
                float[] thresholds = { cutoff, -cutoff };

                foreach (var threshold in thresholds)
                {
                    var insideCutoff = NextAfter(threshold, -1);
                    Assert.AreEqual(
                        1.0f,
                        Fun.Sinc(insideCutoff),
                        "Expected Fun.Sinc(x) = 1 one ULP inside the float cutoff, but got x = {0:R} ({1})",
                        insideCutoff,
                        FormatBits(insideCutoff)
                    );

                    var firstNonOne = FindFirstFloatSincNotOne(threshold, searchUlps);
                    Assert.AreNotEqual(
                        1.0f,
                        Fun.Sinc(firstNonOne),
                        "Expected Fun.Sinc(x) != 1 at or above the float cutoff, but got x = {0:R} ({1})",
                        firstNonOne,
                        FormatBits(firstNonOne)
                    );

                    var x = firstNonOne;
                    for (var i = 0; i < verifyUlps; i++)
                    {
                        Assert.AreNotEqual(
                            1.0f,
                            Fun.Sinc(x),
                            "Expected Fun.Sinc(x) != 1 in the verified float window above the cutoff, but got x = {0:R} ({1})",
                            x,
                            FormatBits(x)
                        );

                        x = NextAfter(x, 1);
                    }
                }
            }

            {
                const double cutoff = 6.840859302478615E-09;
                double[] thresholds = { cutoff, -cutoff };

                foreach (var threshold in thresholds)
                {
                    var insideCutoff = NextAfter(threshold, -1);
                    Assert.AreEqual(
                        1.0,
                        Fun.Sinc(insideCutoff),
                        "Expected Fun.Sinc(x) = 1 one ULP inside the double cutoff, but got x = {0:R} ({1})",
                        insideCutoff,
                        FormatBits(insideCutoff)
                    );

                    var firstNonOne = FindFirstDoubleSincNotOne(threshold, searchUlps);
                    Assert.AreNotEqual(
                        1.0,
                        Fun.Sinc(firstNonOne),
                        "Expected Fun.Sinc(x) != 1 at or above the double cutoff, but got x = {0:R} ({1})",
                        firstNonOne,
                        FormatBits(firstNonOne)
                    );

                    var x = firstNonOne;
                    for (var i = 0; i < verifyUlps; i++)
                    {
                        Assert.AreNotEqual(
                            1.0,
                            Fun.Sinc(x),
                            "Expected Fun.Sinc(x) != 1 in the verified double window above the cutoff, but got x = {0:R} ({1})",
                            x,
                            FormatBits(x)
                        );

                        x = NextAfter(x, 1);
                    }
                }
            }
        }
    }
}

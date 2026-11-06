using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using BigInteger = System.Numerics.BigInteger;

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
        public static void SignedRootsRestoreNegativeOddPerfectPowers()
        {
            foreach (var core in RootCores())
            {
                if (!core.Signed) continue;
                foreach (var basis in new[] { 1, 2, 3, 5, 7, 9 })
                foreach (var degree in new[] { 1, 3, 5, 7, 9, 15, 31 })
                {
                    var power = BigInteger.Pow(basis, degree);
                    if (power > core.ExactIntegerLimit) continue;
                    var value = core.Integer(-power);
                    core.Odd(value, degree, -basis, "exact negative integer power");
                    core.Odd(value, -degree, -1.0 / basis, "reciprocal exact negative integer power");
                }
            }
        }

        [Test]
        public static void SignedRootsHandleMinimaAndExtremeOddDegrees()
        {
            foreach (var core in RootCores())
            {
                if (!core.Signed) continue;
                foreach (var value in core.NegativeValues())
                foreach (var degree in new[] { int.MinValue + 1, -16777217, -65537, -63, -31, -7, -3, -1,
                                               1, 3, 7, 31, 63, 65537, 16777217, int.MaxValue })
                    core.Odd(value, degree, null, "signed minimum/extreme magnitude and degree");
                if (!core.Floating)
                {
                    // The magnitude of every signed integer minimum is 2^(bits-1).
                    core.Odd(core.Minimum, core.Bits - 1, -2, "signed-minimum analytic root");
                    core.Odd(core.Minimum, 1 - core.Bits, -.5, "signed-minimum analytic reciprocal root");
                }
            }
        }

        [Test]
        public static void ScalarRootsPreserveExistingDomains()
        {
            foreach (var core in RootCores())
            {
                var degrees = new[] { int.MinValue, int.MinValue + 1, -31, -8, -3, -2, -1, 0, 1, 2, 3, 8, 31, int.MaxValue };
                foreach (var value in core.NonnegativeValues())
                foreach (var degree in degrees) core.Legacy(value, degree, "nonnegative/unsigned domain");
                if (core.Signed)
                {
                    foreach (var value in core.NegativeValues())
                    foreach (var degree in new[] { int.MinValue, -32, -8, -2, 0, 2, 8, 32, int.MaxValue - 1 })
                        core.Legacy(value, degree, "negative operand/even or zero degree");
                }
                if (core.Floating)
                {
                    foreach (var value in new[] { -0.0, 0.0, double.NaN, double.PositiveInfinity })
                    foreach (var degree in degrees) core.Legacy(core.FloatingValue(value), degree, "signed zero/NaN/infinity");
                }
                var random = new Random(175129);
                var limit = (int)Math.Min(1048576, Convert.ToDouble(core.Maximum));
                for (int i = 0; i < 1024; i++)
                {
                    var integer = new BigInteger(random.Next(limit + 1));
                    int degree = degrees[i % degrees.Length];
                    if (core.Signed && (i & 1) == 0) { integer = -integer; degree &= ~1; }
                    core.Legacy(core.Integer(integer), degree, "ordinary seed=175129, case=" + i);
                }
            }
        }

        private sealed class RootCore
        {
            private readonly MethodInfo root;
            private readonly Type type;
            private readonly bool native;
            private readonly string name;
            public bool Floating => type == typeof(float) || type == typeof(double);
            public bool Single => type == typeof(float);
            public bool Signed => Floating || type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long);
            public int Bits => type == typeof(sbyte) || type == typeof(byte) ? 8 : type == typeof(short) || type == typeof(ushort) ? 16 : type == typeof(int) || type == typeof(uint) ? 32 : 64;
            public object Minimum => type.GetField("MinValue").GetValue(null);
            public object Maximum => type.GetField("MaxValue").GetValue(null);
            public BigInteger ExactIntegerLimit => Floating ? BigInteger.One << (Single ? 24 : 53) : BigInteger.Abs(new BigInteger(Convert.ToInt64(Minimum)));
            public RootCore(Assembly assembly, Type type, string target)
            {
                this.type = type; native = target == "net10.0"; name = target + "/" + type.Name;
                var fun = assembly.GetType("Aardvark.Base.Fun", true); Assert.AreSame(assembly, fun.Assembly, name);
                root = fun.GetMethod("Root", new[] { type, typeof(int) });
                Assert.IsNotNull(root, name); Assert.AreSame(assembly, root.DeclaringType.Assembly, name);
                Assert.AreEqual(Single ? typeof(float) : typeof(double), root.ReturnType, name);
            }
            public object Integer(BigInteger x)
            {
                if (type == typeof(sbyte)) return (sbyte)x; if (type == typeof(byte)) return (byte)x;
                if (type == typeof(short)) return (short)x; if (type == typeof(ushort)) return (ushort)x;
                if (type == typeof(int)) return (int)x; if (type == typeof(uint)) return (uint)x;
                if (type == typeof(long)) return (long)x; if (type == typeof(ulong)) return (ulong)x;
                return FloatingValue((double)x);
            }
            public object FloatingValue(double x) => Single ? (object)(float)x : x;
            private double Pow(double x, int n) => Single ? native ? MathF.Pow((float)x, 1.0f / n) : (float)Math.Pow((float)x, 1.0 / n) : Math.Pow(x, 1.0 / n);
            private double Call(object x, int n) => Convert.ToDouble(root.Invoke(null, new[] { x, (object)n }));
            private void Same(double expected, double actual, string context)
            {
                context += $", expected={expected:R}, actual={actual:R}";
                if (double.IsNaN(expected)) Assert.IsTrue(double.IsNaN(actual), context + " NaN");
                else Assert.AreEqual(Single ? BitConverter.SingleToInt32Bits((float)expected) : BitConverter.DoubleToInt64Bits(expected),
                    Single ? BitConverter.SingleToInt32Bits((float)actual) : BitConverter.DoubleToInt64Bits(actual), context + " exact result/zero sign");
            }
            public void Odd(object value, int degree, double? ideal, string label)
            {
                var x = Convert.ToDouble(value); var context = $"{name}: {label}, x={value}, converted={x:R}, degree={degree}";
                Assert.IsTrue(x < 0 && (degree & 1) != 0, context + " repaired domain");
                var actual = Call(value, degree);
                Same(-Pow(-x, degree), actual, context + " positive-magnitude calculation and sign");
                if (ideal.HasValue)
                {
                    double exponent = Single && native ? 1.0f / degree : 1.0 / degree;
                    double exponentUlp = Single && native ? Math.Abs((double)float.BitIncrement((float)exponent) - exponent) : Math.Abs(Math.BitIncrement(exponent) - exponent);
                    double rounded = Single ? (float)ideal.Value : ideal.Value;
                    double ulp = Single ? Math.Abs((double)float.BitIncrement((float)rounded) - rounded) : Math.Abs(Math.BitIncrement(rounded) - rounded);
                    // The reciprocal exponent is intentionally rounded as before. Bound its
                    // propagated effect separately from a small power-evaluation allowance.
                    double bound = Math.Abs(ideal.Value) * (Math.Exp(Math.Abs(Math.Log(-x)) * exponentUlp) - 1) + 8 * ulp;
                    Assert.That(Math.Abs(actual - ideal.Value), Is.LessThanOrEqualTo(bound), context + " independent analytic root=" + ideal.Value.ToString("R"));
                }
            }
            public void Legacy(object value, int degree, string label)
            {
                double x = Convert.ToDouble(value);
                Assert.IsFalse(x < 0 && (degree & 1) != 0, name + " unchanged domain");
                Same(Pow(x, degree), Call(value, degree), $"{name}: {label}, x={value}, converted={x:R}, degree={degree}");
            }
            public IEnumerable<object> NegativeValues()
            {
                yield return Minimum; yield return Integer(-1); yield return Integer(-2); yield return Integer(-8);
                if (!Floating) { yield return Integer(new BigInteger(Convert.ToInt64(Minimum)) + 1); yield break; }
                yield return FloatingValue(Single ? -float.Epsilon : -double.Epsilon);
                yield return FloatingValue(-Math.ScaleB(1, Single ? -126 : -1022));
                yield return FloatingValue(double.NegativeInfinity);
            }
            public IEnumerable<object> NonnegativeValues()
            {
                foreach (var n in new[] { 0, 1, 2, 3, 4, 8, 9, 16, 27, 32, 64, 81, 125, 128, 243, 1024 })
                    if (n <= Convert.ToDouble(Maximum)) yield return Integer(n);
                yield return Maximum;
            }
        }
        private static IEnumerable<RootCore> RootCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path); var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                foreach (var type in new[] { typeof(sbyte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) })
                    yield return new RootCore(assembly, type, target);
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using BigInteger = System.Numerics.BigInteger;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PolynomialTests : TestSuite
    {
        public PolynomialTests() : base() { }
        public PolynomialTests(Options options) : base(options) { }

#if !TRAVIS_CI

        [Test]
        public void NormedQuadraticsKeepFiniteRootsWhenSquaresOverflow()
        {
            var boundary = Math.ScaleB(1, 513);
            var magnitudes = new[] { Math.ScaleB(1, 510), Math.ScaleB(1, 511), Math.ScaleB(1, 512),
                Math.BitDecrement(boundary), boundary, Math.BitIncrement(boundary), Math.ScaleB(1, 514),
                Math.ScaleB(1, 600), 1e200, Math.ScaleB(1, 768), Math.ScaleB(1, 1023), double.MaxValue };
            foreach (var core in QuadraticCores())
            foreach (var magnitude in magnitudes)
            foreach (var sign in new[] { -1, 1 })
            foreach (var q in new[] { -1.0, 1.0, -0.0, 0.0, -double.Epsilon, double.Epsilon,
                                      -Math.ScaleB(1, -1022), Math.ScaleB(1, -1022) })
                core.Exact(sign * magnitude, q, "square overflow/boundary");
        }

        [Test]
        public void NormedQuadraticsHandleOverflowingDiscriminantSums()
        {
            var boundary = Math.ScaleB(1, 513);
            foreach (var core in QuadraticCores())
            {
                // Roots 2^513 and -2^510: p^2/4 is finite but adding -q overflows.
                double p = -7 * Math.ScaleB(1, 510), q = -Math.ScaleB(1, 1023);
                Assert.IsTrue(double.IsFinite((p / 2) * (p / 2)), core.Name + " finite square");
                Assert.IsTrue(double.IsPositiveInfinity((p / 2) * (p / 2) - q), core.Name + " overflowing sum");
                core.Exact(p, q, "known opposite-sign power-of-two roots");
                var known = ExactQuadraticRoots(p, q);
                Assert.AreEqual(-Math.ScaleB(1, 510), known[0]);
                Assert.AreEqual(Math.ScaleB(1, 513), known[1]);
                foreach (var magnitude in new[] { 0.0, Math.ScaleB(1, 512), -p, Math.BitDecrement(boundary),
                                                  boundary, Math.BitIncrement(boundary), 1e155, 1e200 })
                foreach (var sign in new[] { -1, 1 })
                foreach (var constant in new[] { -double.MaxValue, -Math.BitDecrement(double.MaxValue),
                    -Math.ScaleB(1, 1023), Math.ScaleB(1, 1023), Math.BitDecrement(double.MaxValue), double.MaxValue })
                    core.Exact(sign * magnitude, constant, "negative-q sum overflow/large positive-q control");
            }
        }

        [Test]
        public void NormedQuadraticsPreserveOrdinaryAndIntegratedContracts()
        {
            foreach (var core in QuadraticCores())
            {
                for (int p = -16; p <= 16; p++)
                for (int q = -16; q <= 16; q++) core.Legacy(p * .25, q * .25, "ordinary discriminant signs");
                var special = new[] { -0.0, 0.0, -1.0, 1.0, -double.MaxValue, double.MaxValue,
                                      double.NegativeInfinity, double.PositiveInfinity, double.NaN };
                foreach (var p in special)
                foreach (var q in special)
                    if (!(double.IsFinite(p) && double.IsFinite(q) && double.IsPositiveInfinity((p / 2) * (p / 2) - q)))
                        core.Legacy(p, q, "signed-zero/nonfinite compatibility");
                var random = new Random(179131);
                for (int i = 0; i < 1024; i++)
                    core.Legacy(random.Next(-4096, 4097) / 32.0, random.Next(-4096, 4097) / 32.0, "seed=179131 case=" + i);
                core.Integrations();
            }
        }

        // Exact IEEE dyadics share a 2^-1074 unit. Integer square root with
        // 128 guard bits provides an independent reference, without floating
        // discriminants, scaled division, or the production q/r pairing.
        private static BigInteger QuadraticUnits(double value)
        {
            long bits = BitConverter.DoubleToInt64Bits(value);
            int exponent = (int)((bits >> 52) & 2047);
            var significand = new BigInteger((ulong)bits & 0xfffffffffffffUL);
            if (exponent != 0) significand = (significand + (BigInteger.One << 52)) << (exponent - 1);
            return bits < 0 ? -significand : significand;
        }
        private static BigInteger QuadraticIntegerSqrt(BigInteger value)
        {
            if (value.IsZero) return value;
            var x = BigInteger.One << (int)((value.GetBitLength() + 1) / 2);
            while (true) { var next = (x + value / x) >> 1; if (next >= x) return x; x = next; }
        }
        private static double QuadraticRoundedDyadic(BigInteger value, int exponent)
        {
            if (value.IsZero) return 0;
            bool negative = value.Sign < 0; value = BigInteger.Abs(value);
            int shift = Math.Max((int)value.GetBitLength() - 53, -1074 - exponent);
            if (shift > 0)
            {
                var rounded = value >> shift; var remainder = value - (rounded << shift);
                var half = BigInteger.One << (shift - 1);
                if (remainder > half || (remainder == half && !rounded.IsEven)) rounded++;
                value = rounded;
            }
            else value <<= -shift;
            var result = Math.ScaleB((double)value, exponent + shift);
            return negative ? -result : result;
        }
        private static double[] ExactQuadraticRoots(double p, double q)
        {
            var ip = QuadraticUnits(p); var iq = QuadraticUnits(q);
            var discriminant = ip * ip - (iq << 1076);
            if (discriminant.Sign < 0) return Array.Empty<double>();
            var squareRoot = QuadraticIntegerSqrt(discriminant << 256);
            return new[] { QuadraticRoundedDyadic(-(ip << 128) - squareRoot, -1203),
                           QuadraticRoundedDyadic(-(ip << 128) + squareRoot, -1203) };
        }
        private static double QuadraticUlp(double x)
        {
            x = Math.Abs(x);
            return x == double.MaxValue ? x - Math.BitDecrement(x) : Math.BitIncrement(x) - x;
        }
        private sealed class QuadraticCore
        {
            private readonly MethodInfo direct, array;
            private readonly Type polynomial;
            public string Name { get; }
            public QuadraticCore(Assembly assembly, string name)
            {
                Name = name; polynomial = assembly.GetType("Aardvark.Base.Polynomial", true);
                Assert.AreSame(assembly, polynomial.Assembly, Name);
                direct = polynomial.GetMethod("RealRootsOfNormed", new[] { typeof(double), typeof(double) });
                array = polynomial.GetMethod("RealRootsNormed", new[] { typeof(double[]) });
                foreach (var method in new[] { direct, array })
                { Assert.IsNotNull(method, Name); Assert.AreSame(assembly, method.DeclaringType.Assembly, Name); }
                Assert.AreEqual(typeof(ValueTuple<double, double>), direct.ReturnType, Name);
                Assert.AreEqual(typeof(double[]), array.ReturnType, Name);
            }
            private (double, double) Pair(double p, double q) => ((double, double))direct.Invoke(null, new object[] { p, q });
            private double[] Array(double p, double q) => (double[])array.Invoke(null, new object[] { new[] { q, p } });
            private static void Bits(double expected, double actual, string context)
            {
                if (double.IsNaN(expected)) Assert.IsTrue(double.IsNaN(actual), context);
                else Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context);
            }
            public void Exact(double p, double q, string label)
            {
                var context = $"{Name}: {label}, p={p:R}, q={q:R}";
                var expected = ExactQuadraticRoots(p, q); var pair = Pair(p, q); var forwarded = Array(p, q);
                Assert.AreEqual(expected.Length, forwarded.Length, context + " array root count");
                if (expected.Length == 0)
                { Assert.IsTrue(double.IsNaN(pair.Item1) && double.IsNaN(pair.Item2), context + " no real roots"); return; }
                var roots = new[] { pair.Item1, pair.Item2 };
                Assert.That(roots[0], Is.LessThanOrEqualTo(roots[1]), context + " ascending/multiplicity");
                var ip = QuadraticUnits(p); var iq = QuadraticUnits(q);
                for (int i = 0; i < 2; i++)
                {
                    var x = roots[i]; var message = context + $", root[{i}]={x:R}, expected={expected[i]:R}";
                    Assert.IsTrue(double.IsFinite(x), message + " finite root");
                    if (expected[i] == 0) Assert.AreEqual(0.0, x, message + " rounded zero");
                    else Assert.That(Math.Abs(x - expected[i]), Is.LessThanOrEqualTo(8 * QuadraticUlp(expected[i])), message + " eight ULP reference");
                    Bits(x, forwarded[i], message + " array forwarding/zero sign");
                    var ix = QuadraticUnits(x); var u = QuadraticUnits(QuadraticUlp(x)) * 16;
                    var residual = ix * ix + ip * ix + (iq << 1074);
                    Assert.IsTrue(BigInteger.Abs(residual) <= u * (2 * BigInteger.Abs(ix) + BigInteger.Abs(ip) + u), message + " exact dyadic residual/rounding bound");
                }
                var x0 = QuadraticUnits(roots[0]); var x1 = QuadraticUnits(roots[1]);
                var u0 = 16 * QuadraticUnits(QuadraticUlp(roots[0])); var u1 = 16 * QuadraticUnits(QuadraticUlp(roots[1]));
                Assert.IsTrue(BigInteger.Abs(x0 * x1 - (iq << 1074)) <= BigInteger.Abs(x0) * u1 + BigInteger.Abs(x1) * u0 + u0 * u1, context + " exact dyadic root product/rounding bound");
            }
            public void Legacy(double p, double q, string label)
            {
                double half = p / 2, d = half * half - q; (double, double) expected;
                if (d < 0) expected = (double.NaN, double.NaN);
                else if (d == 0) expected = (-half, -half);
                else if (half > 0) { var r = -(half + Math.Sqrt(d)); expected = (r, q / r); }
                else { var r = Math.Sqrt(d) - half; expected = (q / r, r); }
                var actual = Pair(p, q); var context = $"{Name}: {label}, p={p:R}, q={q:R}";
                Bits(expected.Item1, actual.Item1, context + " first"); Bits(expected.Item2, actual.Item2, context + " second");
                var values = new[] { expected.Item1, expected.Item2 }.Where(x => !double.IsNaN(x)).ToArray();
                var forwarded = Array(p, q); Assert.AreEqual(values.Length, forwarded.Length, context + " array length");
                for (int i = 0; i < values.Length; i++) Bits(values[i], forwarded[i], context + " array index=" + i);
            }
            public void Integrations()
            {
                var general = polynomial.GetMethod("RealRootsOf", new[] { typeof(double), typeof(double), typeof(double) });
                Assert.AreSame(polynomial.Assembly, general.DeclaringType.Assembly, Name);
                var pair = ((double, double))general.Invoke(null, new object[] { 2.0, -2.0, -12.0 });
                Assert.AreEqual(-2.0, pair.Item1, Name + " unchanged general quadratic"); Assert.AreEqual(3.0, pair.Item2, Name);
                foreach (var coefficients in new[] { new[] { 0.0, -1.0, 0.0 }, new[] { 2.0, -3.0, 0.0 }, new[] { 6.0, 1.0, -7.0, -1.0 } })
                {
                    var roots = (double[])array.Invoke(null, new object[] { coefficients });
                    var expected = coefficients.Length == 4 ? new[] { -2.0, -1.0, 1.0, 3.0 } : coefficients[0] == 0 ? new[] { -1.0, 0.0, 1.0 } : new[] { -2.0, 1.0, 1.0 };
                    Assert.AreEqual(expected.Length, roots.Length, Name + " cubic/quartic multiplicity");
                    for (int i = 0; i < expected.Length; i++) Assert.That(roots[i], Is.EqualTo(expected[i]).Within(1e-12), Name + ", integration coefficients=" + string.Join(",", coefficients) + ", index=" + i);
                }
            }
        }
        private static IEnumerable<QuadraticCore> QuadraticCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Polynomial).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new QuadraticCore(assembly, target);
            }
        }

        [Test]
        public void TestPolynomial()
        {
            RootsTest3(0.04);
            RootsTest4(0.125);
        }

        [Test]
        public void RealRoots_LinearAscendingCoefficients()
        {
            var roots = new[] { -6.0, 2.0 }.RealRoots();

            Assert.That(roots, Has.Length.EqualTo(1));
            Assert.That(roots[0], Is.EqualTo(3.0).Within(1e-12));
        }

        [Test]
        public void RealRoots_ConstantPolynomialHasNoRoots()
        {
            var roots = new[] { -6.0, 0.0 }.RealRoots();

            Assert.That(roots, Is.Empty);
        }

        [Test]
        public void RealRootsOf_ZeroDoubleRootReturnsRepeatedZero()
        {
            var roots = Polynomial.RealRootsOf(1.0, 0.0, 0.0);

            Assert.That(roots.Item1, Is.EqualTo(0.0));
            Assert.That(roots.Item2, Is.EqualTo(0.0));
        }

        [Test]
        public void RealRootsOfNormed_ZeroDoubleRootReturnsRepeatedZero()
        {
            var roots = Polynomial.RealRootsOfNormed(0.0, 0.0);

            Assert.That(roots.Item1, Is.EqualTo(0.0));
            Assert.That(roots.Item2, Is.EqualTo(0.0));
        }

        [Test]
        public void RealRoots_ZeroDoubleRootReturnsRepeatedZero()
        {
            var roots = new[] { 0.0, 0.0, 1.0 }.RealRoots();

            Assert.That(roots, Has.Length.EqualTo(2));
            Assert.That(roots[0], Is.EqualTo(0.0));
            Assert.That(roots[1], Is.EqualTo(0.0));
        }

        [Test]
        public void RealRoots_QuarticIgnoresTrailingZeroCoefficients()
        {
            var roots = new[] { 6.0, 1.0, -7.0, -1.0, 1.0, 0.0, 0.0 }.RealRoots();

            AssertRoots(roots, -2.0, -1.0, 1.0, 3.0);
        }

        [Test]
        public void RealRoots_LowerDegreePolynomialIgnoresTrailingZeroCoefficients()
        {
            var roots = new[] { -1.0, 0.0, 1.0, 0.0, 0.0, 0.0 }.RealRoots();

            AssertRoots(roots, -1.0, 1.0);
        }

        [Test]
        public void RealRoots_AllZeroPaddedPolynomialHasNoRoots()
        {
            var roots = new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }.RealRoots();

            Assert.That(roots, Is.Empty);
        }

        [TestCase(0.0)]
        [TestCase(1.0)]
        public void RealRootsOfNormed_PositiveDiscriminantReturnsOneRealRoot(double constant)
        {
            var roots = Polynomial.RealRootsOfNormed(0.0, 1.0, constant);

            AssertSingleRealCubicRoot(roots, 1.0, 0.0, 1.0, constant);
        }

        [TestCase(0.0)]
        [TestCase(1.0)]
        public void RealRootsOfDepressed_PositiveDiscriminantReturnsOneRealRoot(double constant)
        {
            var roots = Polynomial.RealRootsOfDepressed(1.0, constant);

            AssertSingleRealCubicRoot(roots, 1.0, 0.0, 1.0, constant);
        }

        [TestCase(0.0)]
        [TestCase(1.0)]
        public void RealRootsOf_NonNormedPositiveDiscriminantReturnsOneRealRoot(double constant)
        {
            var roots = Polynomial.RealRootsOf(2.0, 0.0, 2.0, 2.0 * constant);

            AssertSingleRealCubicRoot(roots, 2.0, 0.0, 2.0, 2.0 * constant);
        }

        [TestCase(0.0)]
        [TestCase(1.0)]
        public void RealRoots_CoefficientArraysContainOnlyRealRoots(double constant)
        {
            var coefficients = new[] { constant, 1.0, 0.0, 1.0 };
            var normedCoefficients = new[] { constant, 1.0, 0.0 };

            AssertSingleRealCubicRoot(coefficients.RealRoots(), coefficients);
            AssertSingleRealCubicRoot(normedCoefficients.RealRootsNormed(), coefficients);
        }

        [Test]
        public void RealRootsOfDepressed_NegativeDiscriminantReturnsThreeAscendingRoots()
        {
            var roots = Polynomial.RealRootsOfDepressed(-1.0, 0.0);

            Assert.That(roots.Item1, Is.EqualTo(-1.0).Within(1e-12));
            Assert.That(roots.Item2, Is.EqualTo(0.0).Within(1e-12));
            Assert.That(roots.Item3, Is.EqualTo(1.0).Within(1e-12));
        }

        [Test]
        public void RealRootsOfDepressed_ZeroDiscriminantPreservesMultiplicity()
        {
            var doubleRoot = Polynomial.RealRootsOfDepressed(-3.0, 2.0);
            Assert.That(doubleRoot.Item1, Is.EqualTo(-2.0).Within(1e-12));
            Assert.That(doubleRoot.Item2, Is.EqualTo(1.0).Within(1e-12));
            Assert.That(doubleRoot.Item3, Is.EqualTo(1.0).Within(1e-12));

            var tripleRoot = Polynomial.RealRootsOfDepressed(0.0, 0.0);
            Assert.That(tripleRoot.Item1, Is.EqualTo(0.0));
            Assert.That(tripleRoot.Item2, Is.EqualTo(0.0));
            Assert.That(tripleRoot.Item3, Is.EqualTo(0.0));
        }

        [Test]
        public void RealRootsOfNormed_RoundoffNearRepeatedRootPreservesThreeRoots()
        {
            // (x + 1)^2 (x + 0.96), whose depressed discriminant rounds positive.
            var roots = Polynomial.RealRootsOfNormed(2.96, 2.92, 0.96);

            Assert.That(roots.Item1, Is.EqualTo(-1.0).Within(1e-9));
            Assert.That(roots.Item2, Is.EqualTo(-1.0).Within(1e-9));
            Assert.That(roots.Item3, Is.EqualTo(-0.96).Within(1e-9));
        }

        [Test]
        public void CubicRootSolvers_NaNDiscriminantsDoNotProduceFiniteRoots()
        {
            var normed = Polynomial.RealRootsOfNormed(double.NaN, 0.0, 0.0);
            var depressed = Polynomial.RealRootsOfDepressed(double.NaN, 0.0);

            Assert.That(double.IsNaN(normed.Item1), Is.True);
            Assert.That(double.IsNaN(normed.Item2), Is.True);
            Assert.That(double.IsNaN(normed.Item3), Is.True);
            Assert.That(double.IsNaN(depressed.Item1), Is.True);
            Assert.That(double.IsNaN(depressed.Item2), Is.True);
            Assert.That(double.IsNaN(depressed.Item3), Is.True);
        }

        [Test]
        public void RealRoots_QuarticWithPositiveDiscriminantCubicFactorContainsOnlyRealRoots()
        {
            // x (x^3 + x + 1) = x^4 + x^2 + x
            var coefficients = new[] { 0.0, 1.0, 1.0, 0.0, 1.0 };
            var roots = coefficients.RealRoots();

            Assert.That(roots, Has.Length.EqualTo(2));
            Assert.That(roots[0], Is.LessThan(0.0));
            Assert.That(roots[1], Is.EqualTo(0.0).Within(1e-12));
            foreach (double root in roots)
                Assert.That(Math.Abs(coefficients.Evaluate(root)), Is.LessThan(1e-12));
        }

        public void Run()
        {
            RootsTest3(0.01);
            RootsTest4(0.05);
        }

        public void RootsTest3(double step)
        {
            Test.Begin("cubic roots tests");
            var range = new Range1d(-1.0, 1.0);
            double half = 0.5 * step;
            double epsilon = Fun.Cbrt(Constant<double>.PositiveTinyValue);
            Report.Value("epsilon", epsilon);
            var stats = Stats<bool>.ComputeMaxMean;
            var uniqueStats = Stats<bool>.ComputeMaxMean;
            var multipleStats = Stats<bool>.ComputeMaxMean;
            var residualStats = Stats<bool>.ComputeMaxMean;
            var uniqueHisto = new Histogram(-16, -8, 8);
            var multipleHisto = new Histogram(-16, 0, 16);
            var uniqueResidualStats = Stats<bool>.ComputeMaxMean;
            var multipleResidualStats = Stats<bool>.ComputeMaxMean;
            long multipleCount = 0;
            for (double x0 = range.Min; x0 < range.Max + half; x0 += step)
            {
                var p0 = new double[] { -x0, 1.0 };
                for (double x1 = range.Min; x1 < range.Max + half; x1 += step)
                {
                    var p1 = new double[] { -x1, 1.0 };
                    var p01 = Polynomial.Multiply(p0, p1);
                    for (double x2 = range.Min; x2 < range.Max + half; x2 += step)
                    {
                        var p2 = new double[] { -x2, 1.0 };
                        var p012 = Polynomial.Multiply(p01, p2);

                        var t = Aardvark.Base.TupleExtensions.CreateAscending(x0, x1, x2);
                        var exact = new double[] { t.Item1, t.Item2, t.Item3 };
                        var roots = p012.RealRoots();

                        var multiple = CountDoubles(exact, 0.0001);
                        if (multiple > 0) ++multipleCount;

                        var rootCountsOk = exact.Length == roots.Length;
                        Test.IsTrue(rootCountsOk, "wrong number of roots found");
                        if (!rootCountsOk)
                        {
                            using (Report.Job("problematic roots:"))
                            {
                                Report.Line("exact: [" + exact.Select(x => x.ToString()).Join(",") + "]");
                                Report.Line("roots: [" + roots.Select(x => x.ToString()).Join(",") + "]");
                            }
                            continue;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            var err = (exact[i] - roots[i]).Abs();
                            Test.IsTrue(err < epsilon, "root differs significantly from exact value {0}", err);
                            double res = p012.Evaluate(roots[i]).Abs();
                            stats.Add(err);
                            residualStats.Add(res);
                            if (multiple == 0)
                            {
                                uniqueStats.Add(err);
                                uniqueHisto.AddLog10(err);
                                uniqueResidualStats.Add(res);
                            }
                            else
                            {
                                multipleStats.Add(err);
                                multipleHisto.AddLog10(err);
                                multipleResidualStats.Add(res);
                            }
                        }
                    }
                }
            }
            Report.Value("roots error", stats);
            Report.Value("unique roots error", uniqueStats);
            Report.Value("unique roots log error histogram", uniqueHisto);
            Report.Value("multiple roots error", multipleStats);
            Report.Value("multiple roots log error histogram", multipleHisto);
            Report.Value("residual error", residualStats);
            Report.Value("unique roots residual error", uniqueResidualStats);
            Report.Value("multiple residual error", multipleResidualStats);
            Test.End();
        }

        public void RootsTest4(double step)
        {
            Test.Begin("quartic roots tests");
            var range = new Range1d(-1.0, 1.0);
            double half = 0.5 * step;
            double epsilon = 4 * Fun.Cbrt(Constant<double>.PositiveTinyValue);
            Report.Value("epsilon", epsilon);
            var stats = Stats<bool>.ComputeMaxMean;
            var uniqueStats = Stats<bool>.ComputeMaxMean;
            var multipleStats = Stats<bool>.ComputeMaxMean;
            var uniqueHisto = new Histogram(-16, -8, 8);
            var multipleHisto = new Histogram(-16, 0, 16);
            var residualStats = Stats<bool>.ComputeMaxMean;
            var uniqueResidualStats = Stats<bool>.ComputeMaxMean;
            var multipleResidualStats = Stats<bool>.ComputeMaxMean;
            long multipleCount = 0;
            for (double x0 = range.Min; x0 < range.Max + half; x0 += step)
            {
                var p0 = new double[] { -x0, 1.0 };
                for (double x1 = range.Min; x1 < range.Max + half; x1 += step)
                {
                    var p1 = new double[] { -x1, 1.0 };
                    var p01 = Polynomial.Multiply(p0, p1);
                    for (double x2 = range.Min; x2 < range.Max + half; x2 += step)
                    {
                        var p2 = new double[] { -x2, 1.0 };
                        var p012 = p01.Multiply(p2);  
                        var t = Aardvark.Base.TupleExtensions.CreateAscending(x0, x1, x2);

                        for (double x3 = range.Min; x3 < range.Max + half; x3 += step)
                        {
                            var p3 = new double[] { -x3, 1.0 };
                            var p0123 = Polynomial.Multiply(p012, p3);
                            var exact = x3.IntoArray().MergeAscending(
                                    new double[] { t.Item1, t.Item2, t.Item3 });
                            var roots = p0123.RealRoots();

                            var multiple = CountDoubles(exact, epsilon);
                            if (multiple > 0) ++multipleCount;

                            if (roots.Length != exact.Length)
                            {
                                exact = exact.WithoutDoubleRoots(epsilon);
                                roots = roots.WithoutDoubleRoots(epsilon);

                                var rootCountsOk = exact.Length == roots.Length;
                                Test.IsTrue(rootCountsOk, "wrong number of roots found");
                                if (!rootCountsOk)
                                {
                                    using (Report.Job("problematic roots:"))
                                    {
                                        Report.Line("exact: [" + exact.Select(x => x.ToString()).Join(",") + "]");
                                        Report.Line("roots: [" + roots.Select(x => x.ToString()).Join(",") + "]");
                                    }
                                    continue;
                                }
                            }

                            for (int i = 0; i < exact.Length; i++)
                            {
                                var err = (exact[i] - roots[i]).Abs();
                                Test.IsTrue(err < epsilon, "root differs significantly from exact value {0}", err);
                                double res = p0123.Evaluate(roots[i]).Abs();
                                stats.Add(err);
                                residualStats.Add(res);
                                if (multiple == 0)
                                {
                                    uniqueStats.Add(err);
                                    uniqueHisto.AddLog10(err);
                                    uniqueResidualStats.Add(res);
                                }
                                else
                                {
                                    multipleStats.Add(err);
                                    multipleHisto.AddLog10(err);
                                    multipleResidualStats.Add(res);
                                }
                            }
                        }
                    }
                }
            }
            Report.Value("roots error", stats);
            Report.Value("unique roots error", uniqueStats);
            Report.Value("unique roots log error histogram", uniqueHisto);
            Report.Value("multiple roots error", multipleStats);
            Report.Value("multiple roots log error histogram", multipleHisto);
            Report.Value("residual error", residualStats);
            Report.Value("unique roots residual error", uniqueResidualStats);
            Report.Value("multiple residual error", multipleResidualStats);
            Test.End();
        }

        private static void AssertSingleRealCubicRoot(
            (double, double, double) roots,
            double c3, double c2, double c1, double c0)
        {
            Assert.That(double.IsFinite(roots.Item1), Is.True);
            Assert.That(double.IsNaN(roots.Item2), Is.True);
            Assert.That(double.IsNaN(roots.Item3), Is.True);

            double residual = ((c3 * roots.Item1 + c2) * roots.Item1 + c1) * roots.Item1 + c0;
            Assert.That(Math.Abs(residual), Is.LessThan(1e-12));
        }

        private static void AssertSingleRealCubicRoot(double[] roots, double[] coefficients)
        {
            Assert.That(roots, Has.Length.EqualTo(1));
            Assert.That(double.IsFinite(roots[0]), Is.True);
            Assert.That(Math.Abs(coefficients.Evaluate(roots[0])), Is.LessThan(1e-12));
        }

#endif

        private static void AssertRoots(double[] roots, params double[] expected)
        {
            Array.Sort(roots);

            Assert.That(roots, Has.Length.EqualTo(expected.Length));
            for (var i = 0; i < expected.Length; i++)
                Assert.That(roots[i], Is.EqualTo(expected[i]).Within(1e-10));
        }

        static int CountDoubles(double[] a, double eps)
        {
            int len = a.Length;
            if (len < 2) return 0;
            int count = 0;
            if ((a[0] - a[1]).Abs() < eps) count++;
            if (len < 3) return count;
            if ((a[1] - a[2]).Abs() < eps) count++;
            if (len < 4) return count;
            if ((a[2] - a[3]).Abs() < eps) count++;
            return count;
        }
    }
}

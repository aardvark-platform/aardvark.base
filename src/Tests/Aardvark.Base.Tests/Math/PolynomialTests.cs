using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
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

        [Test]
        public void CubicTinyRootsKeepSignsAndRelativeAccuracy()
        {
            foreach (var core in CubicCores())
            foreach (var magnitude in new[] { 1e-8, 1e-12, 1e-16, 1e-20, 1e-40, 1e-80, 1e-160, 1e-280 }
                .Concat(new[] { 32, 40, 52, 60, 100, 200, 400, 800, 1000 }.Select(k => Math.ScaleB(1, -k))))
            foreach (var sign in new[] { -1, 1 })
            {
                var q = sign * magnitude;
                CheckSingleCubic(core, 1, q, 0, 1, q, -q, -q, Math.Abs(q), "signed tiny root");
            }
        }

        [Test]
        public void ConstructedCubicRootsStayAccurateAcrossScales()
        {
            foreach (var core in CubicCores())
            foreach (var exponent in new[] { -100, -50, -10, 0, 10, 50, 100 })
            foreach (var mantissa in new[] { .5, 1, 1.5, 3 })
            foreach (var sign in new[] { -1, 1 })
            foreach (var k in new[] { -.5, -.25, 0, Math.ScaleB(1, -40), Math.ScaleB(1, -20), .25, 1, 4, 16, Math.ScaleB(1, 20), Math.ScaleB(1, 40) })
            {
                var root = sign * Math.ScaleB(mantissa, exponent);
                var p = k * root * root;
                var q = -root * ((1 + k) * root * root);
                // (x-root)(x^2+root*x+(1+k)*root^2); k>-.75 leaves one real root.
                CheckSingleCubic(core, p, q, 0, p, q, root, root, Math.Abs(root), $"constructed exponent={exponent}, mantissa={mantissa}, sign={sign}, k={k}");
                if (k == 1)
                foreach (var h in new[] { -2 * root, 2 * root })
                {
                    var c2 = -3 * h; var c1 = 3 * h * h + p; var c0 = q - p * h - h * h * h;
                    CheckSingleCubic(core, p, q, c2, c1, c0, root, root + h, Math.Abs(root) + Math.Abs(h), $"shifted constructed exponent={exponent}, h={h}");
                }
            }
        }

        [Test]
        public void CubicBranchesAndQuarticConsumersKeepTheirContracts()
        {
            foreach (var core in CubicCores())
            {
                foreach (var q in new[] { 0.0, BitConverter.Int64BitsToDouble(long.MinValue) })
                foreach (var result in core.Solve(1, q, 0, 1, q))
                {
                    CheckSingleSlots(result.Api, result.Roots, core.Name + " zero constant");
                    Assert.AreEqual(0L, BitConverter.DoubleToInt64Bits(result.Roots[0]), core.Name + "/" + result.Api + " legacy positive zero");
                }
                foreach (var scale in new[] { .5, 1, 2, 8 })
                foreach (var sign in new[] { -1, 1 })
                {
                    var p = -3 * scale * scale; var q = sign * 2 * scale * scale * scale;
                    var expected = sign > 0 ? new[] { -2 * scale, scale, scale } : new[] { -scale, -scale, 2 * scale };
                    foreach (var result in core.Solve(p, q, 0, p, q))
                        CheckControlRoots(result.Api, result.Roots, expected, -sign * 2 * scale, 1e-12 * scale, core.Name + " repeated roots");
                    p = -scale * scale;
                    foreach (var result in core.Solve(p, 0, 0, p, 0))
                        CheckControlRoots(result.Api, result.Roots, new[] { -scale, 0, scale }, scale, 1e-12 * scale, core.Name + " negative discriminant");
                }
                foreach (var pair in new[] { (double.NaN, 0.0), (1.0, double.NaN), (double.PositiveInfinity, 0.0), (1.0, double.PositiveInfinity), (1.0, double.NegativeInfinity), (double.MaxValue, 1.0), (1.0, double.MaxValue) })
                foreach (var result in core.Solve(pair.Item1, pair.Item2, 0, pair.Item1, pair.Item2))
                {
                    var context = $"{core.Name}/{result.Api}, exceptional p={pair.Item1}, q={pair.Item2}";
                    if (result.Api.Contains("array")) Assert.IsEmpty(result.Roots, context);
                    else Assert.That(result.Roots.All(double.IsNaN), Is.True, context);
                }
                foreach (var entry in new[]
                {
                    (Coefficients: new[] { 6.0, 1.0, -7.0, -1.0 }, Roots: new[] { -2.0, -1.0, 1.0, 3.0 }, Legacy: false),
                    (Coefficients: new[] { 4.0, 4.0, -3.0, -2.0 }, Roots: new[] { -1.0, -1.0, 2.0, 2.0 }, Legacy: true)
                })
                foreach (var result in core.Quartics(entry.Coefficients))
                {
                    var context = core.Name + "/" + result.Api + " quartic coefficients=" + string.Join(",", entry.Coefficients);
                    if (entry.Legacy)
                    {
                        // Native cube-root rounding can already lose repeated quartic
                        // roots here. Preserve that unchanged zero-resolvent branch,
                        // rather than promising a new quartic accuracy/range repair.
                        var expected = core.LegacyRepeatedQuartic();
                        if (result.Api.Contains("array")) expected = expected.Where(v => !double.IsNaN(v)).ToArray();
                        Assert.AreEqual(expected.Length, result.Roots.Length, context + " legacy root count");
                        for (var i = 0; i < expected.Length; i++)
                            if (double.IsNaN(expected[i])) Assert.That(double.IsNaN(result.Roots[i]), Is.True, context + " legacy NaN=" + i);
                            else Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected[i]), BitConverter.DoubleToInt64Bits(result.Roots[i]), context + " exact legacy root=" + i);
                    }
                    else
                    {
                        Assert.AreEqual(4, result.Roots.Length, context);
                        for (var i = 0; i < 4; i++) Assert.That(result.Roots[i], Is.EqualTo(entry.Roots[i]).Within(1e-10), context + " root=" + i);
                    }
                }
                foreach (var q in new[] { -1e-20, 1e-20 })
                foreach (var result in core.Quartics(new[] { 0.0, q, 1.0, 0.0 }))
                {
                    var roots = result.Roots.Where(v => !double.IsNaN(v)).ToArray();
                    var context = core.Name + "/" + result.Api + " quartic x(x^3+x+q), q=" + q;
                    Assert.AreEqual(2, roots.Length, context);
                    var nonzero = q > 0 ? roots[0] : roots[1]; var zero = q > 0 ? roots[1] : roots[0];
                    Assert.AreEqual(0, zero, context);
                    Assert.That(Math.Abs((nonzero + q) / q), Is.LessThanOrEqualTo(CubicError), context);
                }
            }
        }

        private static readonly double CubicError = 128 * Math.ScaleB(1, -53);

        private static void CheckSingleCubic(CubicCore core, double p, double q, double c2, double c1, double c0, double depressedRoot, double shiftedRoot, double rootScale, string label)
        {
            foreach (var result in core.Solve(p, q, c2, c1, c0))
            {
                var expected = result.Depressed ? depressedRoot : shiftedRoot;
                var context = $"{core.Name}/{result.Api}: {label}, p={p}, q={q}, c2={c2}, c1={c1}, c0={c0}, expected={expected}";
                CheckSingleSlots(result.Api, result.Roots, context);
                var root = result.Roots[0];
                Assert.That(double.IsFinite(root), Is.True, context);
                Assert.That(root != 0 && Math.Sign(root) == Math.Sign(expected), Is.True, context + " nonzero sign");
                Assert.That(Math.Abs(root - expected) / rootScale, Is.LessThanOrEqualTo(CubicError), context + " scale-aware root error");
                var a = result.Depressed ? 0 : c2; var b = result.Depressed ? p : c1; var c = result.Depressed ? q : c0;
                var residual = ((root + a) * root + b) * root + c;
                var magnitude = ((Math.Abs(root) + Math.Abs(a)) * Math.Abs(root) + Math.Abs(b)) * Math.Abs(root) + Math.Abs(c);
                Assert.That(magnitude > 0 && double.IsFinite(magnitude), Is.True, context + " residual scale");
                Assert.That(Math.Abs(residual) / magnitude, Is.LessThanOrEqualTo(CubicError), context + " scaled Horner residual");
            }
        }

        private static void CheckSingleSlots(string api, double[] roots, string context)
        {
            if (api.EndsWith("tuple"))
            {
                Assert.AreEqual(3, roots.Length, context);
                Assert.That(double.IsNaN(roots[1]) && double.IsNaN(roots[2]), Is.True, context + " trailing NaNs");
            }
            else Assert.AreEqual(1, roots.Length, context + " root count");
        }

        private static void CheckControlRoots(string api, double[] actual, double[] expected, double one, double tolerance, string context)
        {
            context += "/" + api + ", expected=" + string.Join(",", expected);
            if (api == "one")
            {
                Assert.That(actual[0], Is.EqualTo(one).Within(tolerance), context);
                return;
            }
            Assert.AreEqual(3, actual.Length, context);
            for (var i = 0; i < 3; i++) Assert.That(actual[i], Is.EqualTo(expected[i]).Within(tolerance), context + " root=" + i);
        }

        private sealed class CubicCore
        {
            public readonly string Name;
            private readonly Type polynomial, fun;
            public CubicCore(Assembly assembly, string name)
            {
                polynomial = assembly.GetType("Aardvark.Base.Polynomial", true); fun = assembly.GetType("Aardvark.Base.Fun", true); Name = name;
            }
            private double ScalarFun(string name, double value) => (double)fun.GetMethod(name, new[] { typeof(double) }).Invoke(null, new object[] { value });
            public double[] LegacyRepeatedQuartic()
            {
                var z = ScalarFun("Cbrt", 3.375) - ScalarFun("Cbrt", -3.375) - .75;
                var u = z * z - 5.0625; var v = 2 * z + 4.5;
                u = u < Constant<double>.PositiveTinyValue ? 0 : ScalarFun("Sqrt", u);
                v = v < Constant<double>.PositiveTinyValue ? 0 : ScalarFun("Sqrt", v);
                return Call("RealRootsOfNormed", v, z - u).Concat(Call("RealRootsOfNormed", -v, z + u))
                    .Where(x => !double.IsNaN(x)).OrderBy(x => x).Select(x => x + .5).Concat(Enumerable.Repeat(double.NaN, 4)).Take(4).ToArray();
            }
            private double[] Call(string name, params object[] arguments)
            {
                var value = polynomial.GetMethod(name, arguments.Select(a => a.GetType()).ToArray()).Invoke(null, arguments);
                if (value is double[] array) return array;
                if (value is double scalar) return new[] { scalar };
                var tuple = (ITuple)value;
                return Enumerable.Range(0, tuple.Length).Select(i => (double)tuple[i]).ToArray();
            }
            public IEnumerable<(string Api, double[] Roots, bool Depressed)> Solve(double p, double q, double c2, double c1, double c0)
            {
                yield return ("depressed tuple", Call("RealRootsOfDepressed", p, q), true);
                yield return ("normed tuple", Call("RealRootsOfNormed", c2, c1, c0), false);
                yield return ("one", Call("OneRealRootOfNormed", c2, c1, c0), false);
                yield return ("nonnormed tuple", Call("RealRootsOf", 2.0, 2 * c2, 2 * c1, 2 * c0), false);
                yield return ("coefficient array", Call("RealRoots", (object)new[] { 2 * c0, 2 * c1, 2 * c2, 2.0 }), false);
                yield return ("normed array", Call("RealRootsNormed", (object)new[] { c0, c1, c2 }), false);
            }
            public IEnumerable<(string Api, double[] Roots)> Quartics(double[] c)
            {
                yield return ("quartic tuple", Call("RealRootsOfNormed", c[3], c[2], c[1], c[0]));
                yield return ("quartic nonnormed tuple", Call("RealRootsOf", 2.0, 2 * c[3], 2 * c[2], 2 * c[1], 2 * c[0]));
                yield return ("quartic array", Call("RealRoots", (object)c.Concat(new[] { 1.0, 0.0, 0.0 }).ToArray()));
                yield return ("quartic normed array", Call("RealRootsNormed", (object)c));
            }
        }

        private static IEnumerable<CubicCore> CubicCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Polynomial).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new CubicCore(assembly, target);
            }
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

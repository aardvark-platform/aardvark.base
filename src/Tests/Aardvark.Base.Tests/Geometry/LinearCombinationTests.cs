using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class LinearCombinationTests
    {
        private static IEnumerable<TestCaseData> SpanCases
        {
            get
            {
                yield return Case("IndependentHit", new V3d(2, -3, 0), V3d.XAxis, V3d.YAxis, true);
                yield return Case("IndependentMiss", V3d.ZAxis, V3d.XAxis, V3d.YAxis, false);
                yield return Case("ZeroTarget", V3d.Zero, new V3d(1, 2, 3), new V3d(3, -2, 1), true);
                yield return Case("ParallelHit", new V3d(3, 6, 9), new V3d(1, 2, 3), new V3d(2, 4, 6), true);
                yield return Case("ParallelMiss", V3d.XAxis, new V3d(1, 2, 3), new V3d(2, 4, 6), false);
                yield return Case("Antiparallel", new V3d(3, 6, 9), new V3d(1, 2, 3), new V3d(-2, -4, -6), true);
                yield return Case("Repeated", new V3d(-2, -4, -6), new V3d(1, 2, 3), new V3d(1, 2, 3), true);
                yield return Case("OneZeroHit", new V3d(0, -6, 0), V3d.Zero, new V3d(0, 2, 0), true);
                yield return Case("OneZeroMiss", V3d.XAxis, V3d.Zero, V3d.YAxis, false);
                yield return Case("BothZeroHit", V3d.Zero, V3d.Zero, V3d.Zero, true);
                yield return Case("BothZeroMiss", V3d.One, V3d.Zero, V3d.Zero, false);
                yield return Case("SmallNonzeroTarget", new V3d(1e-20, 0, 0), V3d.Zero, V3d.Zero, false);
            }
        }

        private static TestCaseData Case(string name, V3d x, V3d u, V3d v, bool hit)
            => new TestCaseData(x, u, v, hit).SetName("LinearCombination_" + name);

        [TestCaseSource(nameof(SpanCases))]
        public void RanksAndBasisOrder(V3d x, V3d u, V3d v, bool hit)
        {
            Check(x, u, v, hit);
            Check(x, v, u, hit);
        }

        [Test]
        public void ExhaustiveIntegerSpans()
        {
            var vectors = new List<V3d>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++) vectors.Add(new V3d(x, y, z));
            int count = 0;
            foreach (var u in vectors)
                foreach (var v in vectors)
                {
                    int rank = Rank(u, v);
                    foreach (var x in vectors)
                    {
                        Check(x, u, v, Rank(u, v, x) == rank);
                        count++;
                    }
                }
            Assert.That(count, Is.EqualTo(19683));
        }

        [Test]
        public void ExhaustiveSingleIntegerSpans()
        {
            for (int i = 0; i < 27; i++)
                for (int j = 0; j < 27; j++)
                {
                    var u = new V3d(i % 3 - 1, i / 3 % 3 - 1, i / 9 - 1);
                    var x = new V3d(j % 3 - 1, j / 3 % 3 - 1, j / 9 - 1);
                    bool expected = Rank(u, x) == Rank(u);
                    Assert.That(x.IsLinearCombinationOf(u), Is.EqualTo(expected));
                    Assert.That(((V3f)x).IsLinearCombinationOf((V3f)u), Is.EqualTo(expected));
                }
        }

        // Exact, fraction-free row elimination: independent of vector cross/dot and library LU.
        private static int Rank(params V3d[] columns)
        {
            var a = new BigInteger[3, columns.Length];
            for (int j = 0; j < columns.Length; j++)
                for (int i = 0; i < 3; i++) a[i, j] = (int)columns[j][i];
            int rank = 0;
            for (int j = 0; j < columns.Length && rank < 3; j++)
            {
                int pivot = rank;
                while (pivot < 3 && a[pivot, j].IsZero) pivot++;
                if (pivot == 3) continue;
                for (int k = j; k < columns.Length; k++)
                    (a[rank, k], a[pivot, k]) = (a[pivot, k], a[rank, k]);
                for (int i = rank + 1; i < 3; i++)
                {
                    var factor = a[i, j];
                    for (int k = j; k < columns.Length; k++)
                        a[i, k] = a[i, k] * a[rank, j] - a[rank, k] * factor;
                }
                rank++;
            }
            return rank;
        }

        private static void Check(V3d x, V3d u, V3d v, bool expected)
        {
            string context = $"x={x}, u={u}, v={v}";
            Assert.That(x.IsLinearCombinationOf(u, v), Is.EqualTo(expected), context);
            Assert.That(x.IsLinearCombinationOf(u, v, out var a, out var b), Is.EqualTo(expected), context);
            var xf = (V3f)x; var uf = (V3f)u; var vf = (V3f)v;
            Assert.That(xf.IsLinearCombinationOf(uf, vf), Is.EqualTo(expected), context);
            Assert.That(xf.IsLinearCombinationOf(uf, vf, out var af, out var bf), Is.EqualTo(expected), context);
            if (expected)
            {
                Assert.That(double.IsFinite(a) && double.IsFinite(b), Is.True, context);
                Assert.That(float.IsFinite(af) && float.IsFinite(bf), Is.True, context);
                Assert.That((a * u + b * v - x).NormMax, Is.LessThanOrEqualTo(1e-12), context);
                Assert.That((af * uf + bf * vf - xf).NormMax, Is.LessThanOrEqualTo(2e-5f), context);
                x.IsLinearCombinationOf(u, v, out var againA, out var againB);
                Assert.That((againA, againB), Is.EqualTo((a, b)), "Deterministic coefficients");
                if (u.Cross(v) == V3d.Zero)
                {
                    Assert.That(a == 0 || b == 0, Is.True, context);
                    Assert.That(af == 0 || bf == 0, Is.True, context);
                }
            }
            else
            {
                Assert.That(double.IsNaN(a) && double.IsNaN(b), Is.True, context);
                Assert.That(float.IsNaN(af) && float.IsNaN(bf), Is.True, context);
            }
        }

        [Test]
        public void RankOneUsesLargestBasisAndFirstOnTies()
        {
            var u = new V3d(1, -2, 3);
            Assert.That((6 * u).IsLinearCombinationOf(u, -2 * u, out var a, out var b), Is.True);
            Assert.That((a, b), Is.EqualTo((0.0, -3.0)));
            Assert.That((6 * u).IsLinearCombinationOf(u, u, out a, out b), Is.True);
            Assert.That((a, b), Is.EqualTo((6.0, 0.0)));
            var uf = (V3f)u;
            Assert.That((6 * uf).IsLinearCombinationOf(uf, -2 * uf, out var af, out var bf), Is.True);
            Assert.That((af, bf), Is.EqualTo((0.0f, -3.0f)));
        }

        [Test]
        public void TinyNonzeroBasisIsNotRankZero()
        {
            Assert.That(V3d.XAxis.IsLinearCombinationOf(new V3d(1e-200, 0, 0), V3d.Zero, out var a, out var b), Is.True);
            Assert.That((a, b), Is.EqualTo((1e200, 0.0)));
            Assert.That(V3f.XAxis.IsLinearCombinationOf(new V3f(1e-20f, 0, 0), V3f.Zero, out var af, out var bf), Is.True);
            Assert.That(af, Is.EqualTo(1e20f));
            Assert.That(bf, Is.Zero);
        }

        [Test]
        public void NonFiniteCoefficientsAreMisses()
        {
            Assert.That(V3d.XAxis.IsLinearCombinationOf(new V3d(double.Epsilon, 0, 0), V3d.Zero, out var a, out var b), Is.False);
            Assert.That(double.IsNaN(a) && double.IsNaN(b), Is.True);
            Assert.That(V3f.XAxis.IsLinearCombinationOf(new V3f(float.Epsilon, 0, 0), V3f.Zero, out var af, out var bf), Is.False);
            Assert.That(float.IsNaN(af) && float.IsNaN(bf), Is.True);
        }

        [Test]
        public void NearDependentNonzeroNormalsRetainRankTwo()
        {
            foreach (double h in new[] { 1e-7, 1e-10, 1e-17 })
            {
                var u = V3d.XAxis; var v = new V3d(1, h, 0); var x = new V3d(0, h, 0);
                Assert.That(x.IsLinearCombinationOf(u, v, out var a, out var b), Is.True);
                Assert.That((a, b), Is.EqualTo((-1.0, 1.0)));
                Assert.That((a * u + b * v - x).NormMax, Is.Zero);
                var uf = (V3f)u; var vf = (V3f)v; var xf = (V3f)x;
                Assert.That(xf.IsLinearCombinationOf(uf, vf, out var af, out var bf), Is.True);
                Assert.That((af, bf), Is.EqualTo((-1.0f, 1.0f)));
                Assert.That(V3d.ZAxis.IsLinearCombinationOf(u, v, out a, out b), Is.False);
                Assert.That(double.IsNaN(a) && double.IsNaN(b), Is.True);
                Assert.That(V3f.ZAxis.IsLinearCombinationOf(uf, vf, out af, out bf), Is.False);
                Assert.That(float.IsNaN(af) && float.IsNaN(bf), Is.True);
            }
        }

        [Test]
        public void PredicateAndNormalCoefficientTolerancesArePreserved()
        {
            double eps = Constant<double>.PositiveTinyValue;
            float epsf = Constant<float>.PositiveTinyValue;
            foreach (double scale in new[] { 0.25, 1.0, 4.0 })
                foreach (double multiplier in new[] { -2.0, -1.0, -0.5, 0.0, 0.5, 1.0, 2.0 })
                {
                    var u = new V3d(scale, 0, 0); var v = V3d.YAxis;
                    var x = new V3d(2 * scale, 3, eps * multiplier);
                    Assert.That(x.IsLinearCombinationOf(u, v), Is.EqualTo(u.Cross(v).IsOrthogonalTo(x)));
                    Assert.That(x.IsLinearCombinationOf(u, v, out var a, out var b), Is.EqualTo(Fun.IsTiny(x.Z / scale)));
                    if (double.IsFinite(a)) Assert.That((a, b), Is.EqualTo((2.0, 3.0)));
                    else Assert.That(double.IsNaN(a) && double.IsNaN(b), Is.True);
                    var lineTarget = new V3d(2 * scale, eps * multiplier, 0);
                    Assert.That(lineTarget.IsLinearCombinationOf(u), Is.EqualTo(lineTarget.IsParallelTo(u)));
                    Assert.That(lineTarget.IsLinearCombinationOf(u, 2 * u), Is.EqualTo(lineTarget.IsParallelTo(2 * u)));
                    var uf = (V3f)u; var vf = (V3f)v;
                    var xf = new V3f(2 * (float)scale, 3, epsf * (float)multiplier);
                    Assert.That(xf.IsLinearCombinationOf(uf, vf), Is.EqualTo(uf.Cross(vf).IsOrthogonalTo(xf)));
                    Assert.That(xf.IsLinearCombinationOf(uf, vf, out var af, out var bf), Is.EqualTo(Fun.IsTiny(xf.Z / (float)scale)));
                    if (float.IsFinite(af)) Assert.That((af, bf), Is.EqualTo((2.0f, 3.0f)));
                    else Assert.That(float.IsNaN(af) && float.IsNaN(bf), Is.True);
                    var lineTargetf = new V3f(2 * (float)scale, epsf * (float)multiplier, 0);
                    Assert.That(lineTargetf.IsLinearCombinationOf(uf), Is.EqualTo(lineTargetf.IsParallelTo(uf)));
                    Assert.That(lineTargetf.IsLinearCombinationOf(uf, 2 * uf), Is.EqualTo(lineTargetf.IsParallelTo(2 * uf)));
                }
        }

        [Test]
        public void OrdinaryCoefficientsMatchPreviousSolve()
        {
            var random = new Random(731);
            for (int i = 0; i < 1000; i++)
            {
                var u = new V3d(random.Next(-8, 9), random.Next(-8, 9), random.Next(-8, 9));
                var v = new V3d(random.Next(-8, 9), random.Next(-8, 9), random.Next(-8, 9));
                var n = u.Cross(v);
                if (n == V3d.Zero) continue;
                var x = 2 * u - 3 * v;
                var mat = new[,] { { u.X, v.X, n.X }, { u.Y, v.Y, n.Y }, { u.Z, v.Z, n.Z } };
                var t = mat.LuSolve(mat.LuFactorize(), new[] { x.X, x.Y, x.Z });
                bool hit = x.IsLinearCombinationOf(u, v, out var a, out var b);
                Assert.That(hit, Is.EqualTo(Fun.IsTiny(t[2])));
                if (hit)
                {
                    Assert.That(a, Is.EqualTo(t[0]).Within(2e-13));
                    Assert.That(b, Is.EqualTo(t[1]).Within(2e-13));
                }
                var uf = (V3f)u; var vf = (V3f)v; var nf = uf.Cross(vf); var xf = (V3f)x;
                var matf = new[,] { { uf.X, vf.X, nf.X }, { uf.Y, vf.Y, nf.Y }, { uf.Z, vf.Z, nf.Z } };
                var tf = matf.LuSolve(matf.LuFactorize(), new[] { xf.X, xf.Y, xf.Z });
                bool hitf = xf.IsLinearCombinationOf(uf, vf, out var af, out var bf);
                Assert.That(hitf, Is.EqualTo(Fun.IsTiny(tf[2])));
                if (hitf)
                {
                    Assert.That(af, Is.EqualTo(tf[0]).Within(2e-5f));
                    Assert.That(bf, Is.EqualTo(tf[1]).Within(2e-5f));
                }
            }
        }

        [Test]
        public void WarmedCoefficientQueriesAllocateNothing()
        {
            RunQueries(20000);
            long before = GC.GetAllocatedBytesForCurrentThread();
            double checksum = RunQueries(20000);
            long bytes = GC.GetAllocatedBytesForCurrentThread() - before;
            Assert.That(checksum, Is.GreaterThan(0));
            Assert.That(bytes, Is.Zero);
        }

        private static readonly V3d[] Bases = { V3d.Zero, V3d.XAxis, V3d.YAxis, new V3d(1, 2, 3) };

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static double RunQueries(int count)
        {
            double checksum = 0;
            for (int i = 0; i < count; i++)
            {
                var u = Bases[i % 4]; var v = Bases[i / 4 % 4];
                var x = i % 2 == 0 ? 2 * u + v : V3d.ZAxis;
                if (x.IsLinearCombinationOf(u, v, out var a, out var b)) checksum += a + b + 1;
                if (((V3f)x).IsLinearCombinationOf((V3f)u, (V3f)v, out var af, out var bf)) checksum += af + bf + 1;
            }
            return checksum;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PolynomialTests : TestSuite
    {
        public PolynomialTests() : base() { }
        public PolynomialTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestPolynomial()
        {
            RootsTest3(0.04);
            RootsTest4(0.125);
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

using NUnit.Framework;
using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Aardvark.Tests
{
    public static class Distributions
    {
        [Test]
        public static void CDFSampling()
        {
            var cdfSampleTestCount = 2000000;
            var pdfSizes = new[] { 5, 10, 25, 50, 100, 250 };

            var rnd = new RandomSystem(0);

            foreach (var pz in pdfSizes)
            {
                var pdf = new double[pz].SetByIndex(i => rnd.UniformDouble());
                var sum = pdf.Sum();

                var cdf = new DistributionFunction(pdf);
                Assert.True(cdf.Norm.ApproximateEquals(sum, 1e-10));

                rnd.ReSeed(0);
                var sc1 = new int[pdf.Length];
                Report.BeginTimed("Sample Binary Search");
                for (int i = 0; i < cdfSampleTestCount; i++)
                {
                    var ind = cdf.Sample(rnd.UniformDouble());
                    sc1[ind]++;
                }
                Report.End();

                rnd.ReSeed(0);
                var sc2 = new int[pdf.Length];
                Report.BeginTimed("Sample Linear Search");
                for (int i = 0; i < cdfSampleTestCount; i++)
                {
                    var ind = SamplePDFLinear(pdf, sum, rnd.UniformDouble());
                    sc2[ind]++;
                }
                Report.End();

                for (int i = 0; i < pdf.Length; i++)
                    Assert.True(sc1[i] == sc2[i]);
            }
        }

        static int SamplePDFLinear(double[] pdf, double sum, double rnd)
        {
            var valueToFind = rnd * sum;
            int index = 0;
            double aggregate = 0;
            // Linear Search
            for (; index < pdf.Length - 1; index++)
            {
                aggregate += pdf[index];
                if (valueToFind <= aggregate)
                    break;
            }
            return index;
        }

        private static IEnumerable<(string Target, Func<double, double, double> Gauss,
            Func<double, double, double, double, double> Gauss2d)> GaussianFunctions()
        {
            yield return (typeof(Fun).Assembly.Location, Fun.Gauss, Fun.Gauss2d);

            var directory = Path.GetDirectoryName(typeof(Distributions).Assembly.Location);
            var path = Path.GetFullPath(Path.Combine(directory!, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Expected sibling core assembly at '{path}'.");
            var fun = Assembly.LoadFile(path).GetType(typeof(Fun).FullName!);
            var gauss = fun!.GetMethod(nameof(Fun.Gauss), new[] { typeof(double), typeof(double) })!
                .CreateDelegate<Func<double, double, double>>();
            var gauss2d = fun.GetMethod(nameof(Fun.Gauss2d),
                new[] { typeof(double), typeof(double), typeof(double), typeof(double) })!
                .CreateDelegate<Func<double, double, double, double, double>>();
            yield return (path, gauss, gauss2d);
        }

        [Test]
        public static void GaussianDensitiesMatchStandardizedCoordinates()
        {
            Assert.Multiple(() =>
            {
                foreach (var (target, gauss, _) in GaussianFunctions())
                {
                    foreach (var sigma in new[] { 0.125, 0.3, 0.5, 0.75, 1.0, 1.5, 2.0, 3.0, 10.0 })
                    {
                        var center = 1.0 / (sigma * Math.Sqrt(2.0 * Math.PI));
                        Assert.AreEqual(center, gauss(0.0, sigma), center * 2e-15, $"{target}: center, sigma={sigma:R}");
                        for (int i = -32; i <= 32; i++)
                        {
                            var x = sigma * (i / 4.0);
                            var z = x / sigma;
                            var expected = Math.Exp(-(z * z) / 2.0) / (sigma * Math.Sqrt(2.0 * Math.PI));
                            var actual = gauss(x, sigma);
                            var context = $"{target}: x={x:R}, sigma={sigma:R}";
                            Assert.AreEqual(expected, actual, expected * 2e-14, context);
                            Assert.AreEqual(actual, gauss(-x, sigma), context + ", symmetry");
                        }
                    }
                    Assert.AreEqual(0.12098536225957168, gauss(2.0, 2.0), 2e-16, target);
                    Assert.AreEqual(0.48394144903828673, gauss(0.5, 0.5), 2e-16, target);
                }
            });
        }

        [Test]
        public static void GaussTest()
        {
            Assert.Multiple(() =>
            {
                foreach (var (target, gauss, _) in GaussianFunctions())
                foreach (var sigma in new[] { 0.5, 1.0, 2.0 })
                {
                    // Trapezoidal integration over +/-10 standard deviations.
                    const int intervals = 2000;
                    var sum = 0.5 * (gauss(-10.0 * sigma, sigma) + gauss(10.0 * sigma, sigma));
                    for (int i = 1; i < intervals; i++)
                        sum += gauss((20.0 * i / intervals - 10.0) * sigma, sigma);
                    var integral = sum * (20.0 * sigma / intervals);
                    Assert.AreEqual(1.0, integral, 1e-12, $"{target}: sigma={sigma:R}");
                }
            });
        }

        [Test]
        public static void EllipticalGaussianFactorizesIntoAxisDensities()
        {
            Assert.Multiple(() =>
            {
                foreach (var (target, gauss, gauss2d) in GaussianFunctions())
                foreach (var sx in new[] { 0.5, 1.0, 2.0 })
                foreach (var sy in new[] { 0.3, 1.0, 3.0 })
                foreach (var zx in new[] { -4.0, -1.0, 0.0, 0.5, 3.0 })
                foreach (var zy in new[] { -4.0, -1.0, 0.0, 0.5, 3.0 })
                {
                    var x = sx * zx;
                    var y = sy * zy;
                    var expected = gauss2d(x, y, sx, sy);
                    Assert.AreEqual(expected, gauss(x, sx) * gauss(y, sy), expected * 2e-14,
                        $"{target}: x={x:R}, y={y:R}, sx={sx:R}, sy={sy:R}");
                }
            });
        }

        [Test]
        public static void Gauss2dTest()
        {
            // sample 2d normal distribution function from -10 to 10 with 201x201 samples
            var range = 10;
            var area = new Box2d(-range, -range, range, range);
            var sampleCount = 201; // uneven values with give symmetrical pdf
            var pdf = new Matrix<double>(sampleCount, sampleCount);
            pdf.SetByCoord((x,y) =>
            {
                var px = x / (sampleCount - 1.0);
                var py = y / (sampleCount - 1.0);
                var p = area.Lerp(px, py);
                return Fun.Gauss2d(p.X, p.Y, 1.0);
            });

            // integrate sampled gauss distribution
            var stepSize = range * 2.0 / (sampleCount - 1.0);
            var integral = pdf.Data.Sum() * stepSize * stepSize;
            // Gauss must integrate to 1.0
            Assert.True(integral.ApproximateEquals(1.0, 1e-10));
        }

        [Test]
        public static void Gauss2dEllipticalTest()
        {
            // sample 2d normal distribution function from -10 to 10 with 201x201 samples
            var range = 10;
            var area = new Box2d(-range, -range, range, range);
            var sampleCount = 201; // uneven values with give symmetrical pdf
            var pdf = new Matrix<double>(sampleCount, sampleCount);
            pdf.SetByCoord((x, y) =>
            {
                var px = x / (sampleCount - 1.0);
                var py = y / (sampleCount - 1.0);
                var p = area.Lerp(px, py);
                return Fun.Gauss2d(p.X, p.Y, 1.0, 1.5);
            });

            // integrate sampled gauss distribution
            var stepSize = range * 2.0 / (sampleCount - 1.0);
            var integral = pdf.Data.Sum() * stepSize * stepSize;
            // Gauss must integrate to 1.0
            Assert.True(integral.ApproximateEquals(1.0, 1e-10));
        }

        [Test]
        public static void ErfTest()
        {
            // reference solution
            var erf = new[]
            {
                0,
                0.84270079294971486934122063508261, // 1
                0.99532226501895273416206925636725, // 2
                0.99997790950300141455862722387042, // 3
                0.99999998458274209971998114784033, // 4
                0.99999999999846254020557196514981, // 5
            };

            for (int i = -5; i < 5; i++)
            {
                var f1 = Fun.Erf(i);
                var f2 = Fun.Erf2(i);
                var should = erf[i.Abs()] * i.Sign();
                var e1 = (f1 - should).Abs();
                var e2 = (f2 - should).Abs();

                Report.Line("Erf1 error {0}", e1);
                Report.Line("Erf2 error {0}", e2);

                if (e1 < e2)
                    Report.Line("Erf1");
                else
                    Report.Line("Erf2");

                var test1 = e1.ApproximateEquals(0, 1e-6);
                Report.Line("Erf1 valid={0}", test1);
                Assert.True(test1);
                var test2 = e2.ApproximateEquals(0, 1e-6);
                Report.Line("Erf2 valid={0}", test2);
                Assert.True(test2);
            }

            // reference solution
            var erfClose = new[]
            {
                0,
                0.1124629160182848922032750717439683832216962991597025, // 0.1
                0.2227025892104784541401390068001438163882690384302276, // 0.2
                0.3286267594591274276389140478667565511699180962626758, // 0.3
                0.4283923550466684551036038453201724441218629285225903, // 0.4
                0.5204998778130465376827466538919645287364515757579637, // 0.5
                0.6038560908479259225626224360567232065642733648000979, // 0.6
                0.6778011938374184729756288092441513967162881743348702, // 0.7
                0.7421009647076604861671105865029458773176895799147087, // 0.8
                0.7969082124228321285187247851418859375486580415858037, // 0.9
            };

            for (int i = 1; i < 10; i++)
            {
                var v = i / 10.0;
                var f1 = Fun.Erf(v);
                var f2 = Fun.Erf2(v);
                var should = erfClose[i.Abs()] * i.Sign();
                var e1 = (f1 - should).Abs();
                var e2 = (f2 - should).Abs();

                Report.Line("Erf1 error {0}", e1);
                Report.Line("Erf2 error {0}", e2);

                if (e1 < e2)
                    Report.Line("Erf1");
                else
                    Report.Line("Erf2");

                var test1 = e1.ApproximateEquals(0, 1e-6);
                Report.Line("Erf1 valid={0}", test1);
                Assert.True(test1);
                var test2 = e2.ApproximateEquals(0, 1e-6);
                Report.Line("Erf2 valid={0}", test2);
                Assert.True(test2);
            }
        }
    }
}

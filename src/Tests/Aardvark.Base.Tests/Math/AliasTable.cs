using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Tests
{
    [TestFixture]
    public class AliasTable : TestSuite
    {
        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var ex = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, ex.ParamName);
        }

        static void PrintHistogram(double[] pdf, int[] hist)
        {
            var colWidth = 8;
            var row0 = new StringBuilder();
            var row1 = new StringBuilder();
            var row2 = new StringBuilder();
            var pdfSum = pdf.Sum();
            var histSum = hist.Sum();
            for (int i = 0; i < pdf.Length; i++)
            {
                row0.Append(pdf[i].ToString("0.000").PadLeft(colWidth));
                row1.Append(hist[i].ToString().PadLeft(colWidth));
                var target = pdf[i] / pdfSum;
                var real = hist[i] / (double)histSum;
                var ratio = real / target;
                row2.Append(ratio.ToString("0.000").PadLeft(colWidth));
                if (i < pdf.Length - 1)
                {
                    row0.Append(" | ");
                    row1.Append(" | ");
                    row2.Append(" | ");
                }
            }

            Report.Line(row0.ToString());
            Report.Line(row1.ToString());
            Report.Line(new String('-', pdf.Length * (colWidth + 3) - 3));
            Report.Line(row2.ToString());
        }

        static void EvaluateHistogram(double[] pdf, int[] hist)
        {
            var pdfSum = pdf.Sum();
            var histSum = hist.Sum();
            var rmin = double.MaxValue;
            var rmax = 0.0;
            var ravg = 0.0;
            for (int i = 0; i < pdf.Length; i++)
            {
                var target = pdf[i] / pdfSum;
                var real = hist[i] / (double)histSum;
                var ratio = real / target;
                ravg += ratio;
                if (ratio < rmin) rmin = ratio;
                if (ratio > rmax) rmax = ratio;
            }
            Report.Line("Min: {0:0.000} Max: {1:0.000} Avg: {2:0.000}", rmin, rmax, ravg / pdf.Length);

            Assert.IsTrue(rmin.ApproximateEquals(1, 0.05));
            Assert.IsTrue(rmax.ApproximateEquals(1, 0.05));
        }

        [Test]
        public void AliasTableFRejectsInvalidPdfArguments()
        {
            AssertParamName<ArgumentNullException>("pdf", () => new AliasTableF(null, 1f));
            AssertParamName<ArgumentException>("pdf", () => new AliasTableF(new float[0], 1f));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => new AliasTableF(new[] { -1f, 1f }, 1f));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => new AliasTableF(new[] { float.NaN, 1f }, 1f));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => new AliasTableF(new[] { float.PositiveInfinity, 1f }, 1f));
            AssertParamName<ArgumentException>("pdf", () => new AliasTableF(new[] { 0f, 0f }, 1f));

            var table = new AliasTableF(new[] { 1f }, 1f);
            AssertParamName<ArgumentNullException>("pdf", () => table.FromPdf(null));
            AssertParamName<ArgumentException>("pdf", () => table.FromPdf(new float[0]));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => table.FromPdf(new[] { float.NegativeInfinity, 1f }));
            AssertParamName<ArgumentException>("pdf", () => table.FromPdf(new[] { 0f, 0f }));
        }

        [Test]
        public void AliasTableDRejectsInvalidPdfArguments()
        {
            AssertParamName<ArgumentNullException>("pdf", () => new AliasTableD(null, 1.0));
            AssertParamName<ArgumentException>("pdf", () => new AliasTableD(new double[0], 1.0));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => new AliasTableD(new[] { -1.0, 1.0 }, 1.0));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => new AliasTableD(new[] { double.NaN, 1.0 }, 1.0));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => new AliasTableD(new[] { double.PositiveInfinity, 1.0 }, 1.0));
            AssertParamName<ArgumentException>("pdf", () => new AliasTableD(new[] { 0.0, 0.0 }, 1.0));

            var table = new AliasTableD(new[] { 1.0 }, 1.0);
            AssertParamName<ArgumentNullException>("pdf", () => table.FromPdf(null));
            AssertParamName<ArgumentException>("pdf", () => table.FromPdf(new double[0]));
            AssertParamName<ArgumentOutOfRangeException>("pdf", () => table.FromPdf(new[] { double.NegativeInfinity, 1.0 }));
            AssertParamName<ArgumentException>("pdf", () => table.FromPdf(new[] { 0.0, 0.0 }));
        }

        [Test]
        public void AliasTableFRejectsInvalidPdfNorm()
        {
            var pdf = new[] { 1f };
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableF(pdf, 0f));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableF(pdf, -1f));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableF(pdf, float.NaN));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableF(pdf, float.PositiveInfinity));
        }

        [Test]
        public void AliasTableDRejectsInvalidPdfNorm()
        {
            var pdf = new[] { 1.0 };
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableD(pdf, 0.0));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableD(pdf, -1.0));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableD(pdf, double.NaN));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => new AliasTableD(pdf, double.PositiveInfinity));
        }

        [Test]
        public void AliasTableFUpdateValidatesInputs()
        {
            var table = new AliasTableF(new[] { 1f, 0f }, 1f);
            AssertParamName<ArgumentNullException>("pdf", () => table.Update(null, 1f));
            AssertParamName<ArgumentException>("pdf", () => table.Update(new float[0], 1f));
            AssertParamName<ArgumentException>("pdf", () => table.Update(new[] { 1f }, 1f));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => table.Update(new[] { 1f, 0f }, 0f));
        }

        [Test]
        public void AliasTableDUpdateValidatesInputs()
        {
            var table = new AliasTableD(new[] { 1.0, 0.0 }, 1.0);
            AssertParamName<ArgumentNullException>("pdf", () => table.Update(null, 1.0));
            AssertParamName<ArgumentException>("pdf", () => table.Update(new double[0], 1.0));
            AssertParamName<ArgumentException>("pdf", () => table.Update(new[] { 1.0 }, 1.0));
            AssertParamName<ArgumentOutOfRangeException>("pdfNorm", () => table.Update(new[] { 1.0, 0.0 }, 0.0));
        }

        [Test]
        public void AliasTableFValidSparseDistributionSamplesOnlyPositiveBucket()
        {
            var table = new AliasTableF(new[] { 1f }, 1f).FromNormalizedPdf(new[] { 0f, 5f, 0f });

            Assert.AreEqual(1, table.Sample(0f));
            Assert.AreEqual(1, table.Sample(0.2f));
            Assert.AreEqual(1, table.Sample(0.5f));
            Assert.AreEqual(1, table.Sample(0.999f));
        }

        [Test]
        public void AliasTableDValidSparseDistributionSamplesOnlyPositiveBucket()
        {
            var table = new AliasTableD(new[] { 1.0 }, 1.0).FromNormalizedPdf(new[] { 0.0, 5.0, 0.0 });

            Assert.AreEqual(1, table.Sample(0.0));
            Assert.AreEqual(1, table.Sample(0.2));
            Assert.AreEqual(1, table.Sample(0.5));
            Assert.AreEqual(1, table.Sample(0.999));
        }

        [Test]
        public void SampleTest()
        {
            int sampleCount = 100000000;
            var pdfLength = 1000;

            var rnd = new RandomSystem(0);
            //var pdf = new[] { 0.1, 0.2, 0.5, 0.012, 0.99, 1.4, 0.33 };
            var pdf = new double[pdfLength].SetByIndex(_ => (0.1 + rnd.UniformDouble() * 0.9).Square());
            Report.BeginTimed("Generate CDF");
            var df = new DistributionFunction(pdf);
            Report.End();

            Report.BeginTimed("CDF sampling with binary search:");
            var bin = new int[pdf.Length];
            for (int i = 0; i < sampleCount; i++)
            {
                var s = df.Sample(rnd);
                bin[s]++;
            }
            Report.End();

            Report.Line();
            PrintHistogram(pdf, bin);
            EvaluateHistogram(pdf, bin);
            Report.Line();
            Report.Line();
            Report.Line();

            Report.BeginTimed("Generate alias table:");
            var at = new AliasTableD(pdf, 1 / pdf.Sum());
            Report.End();

            bin.Set(0);
            Report.BeginTimed("Alias table sampling:");
            for (int i = 0; i < sampleCount; i++)
            {
                var s = at.Sample(rnd.UniformDouble());
                bin[s]++;
            }
            Report.End();

            Report.Line();
            PrintHistogram(pdf, bin);
            EvaluateHistogram(pdf, bin);
        }
    }
}

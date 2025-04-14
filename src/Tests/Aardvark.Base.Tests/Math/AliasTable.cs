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

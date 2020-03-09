using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class MatrixTests : TestSuite
    {
        public MatrixTests() : base() { }
        public MatrixTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void InPlaceTransposeTest()
        {
            var rand = new RandomSystem();

            var m = new M44d(rand.CreateUniformDoubleArray(16));
            var transposed = m.Transposed;
            Mat.Transpose(ref m);

            Assert.IsTrue(Fun.ApproximateEquals(m, transposed, 0.0001));
        }

        [Test]
        public void InverseTest()
        {
            MatrixInverseTest(1, 1 << 16);
        }

        [Test]
        public void MultiplicationTest()
        {
            MatrixMultiplicationTest();
        }

        public void Run()
        {
            MatrixMultiplicationTest();
            MatrixInverseTest(16, 1 << 20);
        }

        public void MatrixMultiplicationTest()
        {
            using (Report.JobTimed("Matrix multiplication tests"))
            {
                var rand = new RandomSystem();

                Test.Begin("Row vector with matrix");
                var m = new M44d(rand.CreateUniformDoubleArray(16));
                var v = new V4d(rand.CreateUniformDoubleArray(4));

                Test.IsTrue(v * m == m.Transposed * v);
                Test.End();
            }
        }

        public void MatrixInverseTest(int rounds, int count,
                                      bool doLuM = true, bool doLuV = true, bool doLu2 = true,
                                      bool doGj2 = false,
                                      bool doQrI = true, bool doQr2 = true)
        {
            bool doMul = true;
            double luEpsilon = 1e-5;
            double qrEpsilon = 4e-5;
            Test.Begin("matrix inverse tests");
            Report.Line("epsilon for lu tests: {0:e0}", luEpsilon);
            Report.Line("epsilon for qr tests: {0:e0}", qrEpsilon);
            bool showWorst = true;
            bool showTypes = true;
            var rnd = new RandomSystem(19680713);

            var tc = 4;
            var typeStatsOpt = Stats<M44d>.ComputeCountMaxMean;
            Stats<M44d>[] luiTypeStats = new Stats<M44d>[tc].Set(typeStatsOpt);
            Stats<M44d>[] lumTypeStats = new Stats<M44d>[tc].Set(typeStatsOpt);
            Stats<M44d>[] luvTypeStats = new Stats<M44d>[tc].Set(typeStatsOpt);
            Stats<M44d>[] lu2TypeStats = new Stats<M44d>[tc].Set(typeStatsOpt);
            Stats<M44d>[] gj2TypeStats = new Stats<M44d>[tc].Set(typeStatsOpt);
            Stats<M44d>[] qriTypeStats = new Stats<M44d>[tc].Set(typeStatsOpt);
            Stats<M44d>[] qr2TypeStats = new Stats<M44d>[tc].Set(typeStatsOpt);
            string[] typenames = new string[] { "Rotation", "Scale", "Translation", "Mixed" };

            var histoStatsOpt = StatsOptions.MaxMean;
            var luiHistoStats = new HistogramAndStats<M44d>(-16, 0, 16, histoStatsOpt);
            var lumHistoStats = new HistogramAndStats<M44d>(-16, 0, 16, histoStatsOpt);
            var luvHistoStats = new HistogramAndStats<M44d>(-16, 0, 16, histoStatsOpt);
            var lu2HistoStats = new HistogramAndStats<M44d>(-16, 0, 16, histoStatsOpt);
            var gj2HistoStats = new HistogramAndStats<M44d>(-16, 0, 16, histoStatsOpt);
            var qriHistoStats = new HistogramAndStats<M44d>(-16, 0, 16, histoStatsOpt);
            var qr2HistoStats = new HistogramAndStats<M44d>(-16, 0, 16, histoStatsOpt);

            M44d[] mats = new M44d[count];
            M44d[] luis = new M44d[count];
            M44d[] lums = new M44d[count];
            M44d[] luvs = new M44d[count];
            M44d[] lu2s = new M44d[count];
            M44d[] gj2s = new M44d[count];
            M44d[] qris = new M44d[count];
            M44d[] qr2s = new M44d[count];
            int[] types = new int[count].SetByIndex(i => -1);

            bool failedLuM = false, failedLuV = false, failedLu2 = false;
            bool failedQr2 = false;
            var chainStats = new Stats<bool>(StatsOptions.MaxMean);

            for (int j = 0; j < rounds; j++)
            {
                using (Report.JobTimed("creating {0} matrices round {1} of {2}", count, j + 1, rounds))
                    for (int i = 0; i < count; i++)
                    {
                        M44d mat = M44d.Identity;
                        var chainLength = 0;
                        do
                        {
                            int type = rnd.UniformInt(3);
                            if (types[i] == -1) types[i] = type;
                            else if (types[i] != type)
                                types[i] = 3; //mixed type
                            switch (type)
                            {
                                case 0:
                                    V3d axis = V3d.Zero;
                                    double squaredLength = 0.0;
                                    do
                                    {
                                        axis = new V3d(2 * rnd.UniformDoubleFullClosed() - 1,
                                                       2 * rnd.UniformDoubleFullClosed() - 1,
                                                       2 * rnd.UniformDoubleFullClosed() - 1);
                                        squaredLength = axis.LengthSquared;
                                    }
                                    while (squaredLength == 0);
                                    axis *= 1.0 / Fun.Sqrt(squaredLength);
                                    double alpha = 2 * Constant.Pi * rnd.UniformDoubleFull()
                                                   - Constant.Pi;
                                    M44d rot = M44d.Rotation(axis, alpha);
                                    mat = rot * mat;
                                    chainLength++;
                                    break;
                                case 1:
                                    var s0 = rnd.UniformDouble() < 0.5 ? -4.0 : 4.0;
                                    var s1 = rnd.UniformDouble() < 0.5 ? -4.0 : 4.0;
                                    var s2 = rnd.UniformDouble() < 0.5 ? -4.0 : 4.0;
                                    M44d scale = M44d.Scale(
                                                    s0 * (1.0 - rnd.UniformDoubleFull()),
                                                    s1 * (1.0 - rnd.UniformDoubleFull()),
                                                    s2 * (1.0 - rnd.UniformDoubleFull()));
                                    mat = scale * mat;
                                    chainLength++;
                                    break;
                                case 2:
                                    M44d shift = M44d.Translation(
                                                    32 * rnd.UniformDoubleFullClosed() - 16,
                                                    32 * rnd.UniformDoubleFullClosed() - 16,
                                                    32 * rnd.UniformDoubleFullClosed() - 16);
                                    mat = shift * mat;
                                    chainLength++;
                                    break;
                                default:
                                    break;
                            }
                        }
                        while (rnd.UniformDouble() > 0.25);
                        mats[i] = mat;
                        chainStats.Add(chainLength);
                    }

                if (doQr2)
                    using (Report.JobTimed("qr [,] factorization"))
                        for (int i = 0; i < count; i++)
                            qr2s[i] = mats[i].QrInverse2();

                if (doQrI)
                    using (Report.JobTimed("qr factorization"))
                        for (int i = 0; i < count; i++)
                            qris[i] = mats[i].QrInverse();

                if (doGj2)
                    using (Report.JobTimed("gauss jordan"))
                        for (int i = 0; i < count; i++)
                            gj2s[i] = mats[i].NumericallyInstableGjInverse2();

                if (doLu2)
                    using (Report.JobTimed("lu [,] factorization"))
                        for (int i = 0; i < count; i++)
                            lu2s[i] = mats[i].LuInverse2();

                if (doLuV)
                    using (Report.JobTimed("lu vector solve factorization"))
                        for (int i = 0; i < count; i++)
                            luvs[i] = mats[i].LuInverseV();

                if (doLuM)
                    using (Report.JobTimed("lu matrix solve factorization"))
                        for (int i = 0; i < count; i++)
                            lums[i] = mats[i].LuInverseM();

                using (Report.JobTimed("lu factorization"))
                    for (int i = 0; i < count; i++)
                        luis[i] = mats[i].LuInverse();

                Test.Begin("analysis");
                for (int i = 0; i < count; i++)
                {
                    M44d luid = mats[i] * luis[i];
                    double plErr = Mat.DistanceMax(luid, M44d.Identity);
                    Test.IsTrue(plErr < luEpsilon);
                    luiHistoStats.AddLog10Hist(plErr, mats[i]);
                    luiTypeStats[types[i]].Add(plErr);
                    if (doMul)
                    {
                        var m0 = new Matrix<double>((double[])mats[i], 4, 4);
                        var m1 = new Matrix<double>((double[])luis[i], 4, 4);
                        var id = m0.Multiply(m1);
                        double deltaErr = Mat.Distance1(luid, new M44d(id.Data));
                        Test.IsTrue(deltaErr == 0.0);
                    }


                    if (doLuM)
                    {
                        M44d msid = mats[i] * lums[i];
                        double error = Mat.DistanceMax(msid, M44d.Identity);
                        lumHistoStats.AddLog10Hist(error, mats[i]);
                        lumTypeStats[types[i]].Add(error);
                        double deltaErr = Mat.Distance1(luis[i], lums[i]);
                        if (!Test.IsTrue(deltaErr == 0.0)) failedLuM = true;
                    }
                    if (doLuV)
                    {
                        M44d vsid = mats[i] * luvs[i];
                        double error = Mat.DistanceMax(vsid, M44d.Identity);
                        luvHistoStats.AddLog10Hist(error, mats[i]);
                        luvTypeStats[types[i]].Add(error);
                        double deltaErr = Mat.Distance1(luis[i], luvs[i]);
                        if (!Test.IsTrue(deltaErr == 0.0)) failedLuV = true;
                    }
                    if (doLu2)
                    {
                        M44d a2id = mats[i] * lu2s[i];
                        double error = Mat.DistanceMax(a2id, M44d.Identity);
                        lu2HistoStats.AddLog10Hist(error, mats[i]);
                        lu2TypeStats[types[i]].Add(error);
                        double deltaErr = Mat.Distance1(luis[i], lu2s[i]);
                        if (!Test.IsTrue(deltaErr == 0.0)) failedLu2 = true;
                    }
                    if (doGj2)
                    {
                        M44d gjid = mats[i] * gj2s[i];
                        double error = Mat.DistanceMax(gjid, M44d.Identity);
                        gj2HistoStats.AddLog10Hist(error, mats[i]);
                        gj2TypeStats[types[i]].Add(error);
                    }
                    if (doQrI)
                    {
                        M44d qrid = mats[i] * qris[i];
                        double error = Mat.DistanceMax(qrid, M44d.Identity);
                        qriHistoStats.AddLog10Hist(error, mats[i]);
                        qriTypeStats[types[i]].Add(error);
                        Test.IsTrue(error < qrEpsilon);
                    }
                    if (doQr2)
                    {
                        M44d qrid = mats[i] * qr2s[i];
                        double error = Mat.DistanceMax(qrid, M44d.Identity);
                        qr2HistoStats.AddLog10Hist(error, mats[i]);
                        qr2TypeStats[types[i]].Add(error);
                        double deltaErr = Mat.Distance1(qris[i], qr2s[i]);
                        if (!Test.IsTrue(deltaErr == 0.0)) failedQr2 = true;
                    }
                }
                Test.End();
            }

            Report.Value("matrix chain lengths", chainStats);
            Report.Value("lu factorization error", luiHistoStats.Stats);
            if (showTypes)
                for (int ti = 0; ti < tc; ti++)
                    Report.Value("lu " + typenames[ti] + " matrix factorization error",
                        luiTypeStats[ti]);
            Report.Value("lu factorization log error histogram", luiHistoStats.Histogram);

            if (showWorst)
            {
                using (Report.Job("worst matrix")) WriteMat(luiHistoStats.Stats.MaxData);
                using (Report.Job("worst inverse")) WriteMat(luiHistoStats.Stats.MaxData.LuInverse());
                using (Report.Job("worst result")) WriteMat(luiHistoStats.Stats.MaxData
                                                            * luiHistoStats.Stats.MaxData.LuInverse());
            }

            if (failedLuM)
            {
                Report.Value("lu matrix solve factorization error", lumHistoStats.Stats);
                if (showTypes)
                    for (int ti = 0; ti < tc; ti++)
                        Report.Value("lu matrix solve " + typenames[ti] + " matrix factorization error",
                            lumTypeStats[ti]);
                Report.Value("lu matrix solve factorization log error histogram", lumHistoStats.Histogram);
                if (showWorst)
                {
                    using (Report.Job("worst matrix")) WriteMat(lumHistoStats.Stats.MaxData);
                    using (Report.Job("worst inverse")) WriteMat(lumHistoStats.Stats.MaxData.LuInverseM());
                    using (Report.Job("worst result")) WriteMat(lumHistoStats.Stats.MaxData
                                                                * lumHistoStats.Stats.MaxData.LuInverseM());
                }
            }

            if (failedLuV)
            {
                Report.Value("lu vector solve factorization error", luvHistoStats.Stats);
                if (showTypes)
                    for (int ti = 0; ti < tc; ti++)
                        Report.Value("lu vector solve " + typenames[ti] + " matrix factorization error",
                            luvTypeStats[ti]);
                Report.Value("lu vector solve factorization log error histogram", luvHistoStats.Histogram);
                if (showWorst)
                {
                    using (Report.Job("worst matrix")) WriteMat(luvHistoStats.Stats.MaxData);
                    using (Report.Job("worst inverse")) WriteMat(luvHistoStats.Stats.MaxData.LuInverseV());
                    using (Report.Job("worst result")) WriteMat(luvHistoStats.Stats.MaxData
                                                                * luvHistoStats.Stats.MaxData.LuInverseV());
                }
            }

            if (failedLu2)
            {
                Report.Value("lu [,] factorization error", lu2HistoStats.Stats);
                if (showTypes)
                    for (int ti = 0; ti < tc; ti++)
                        Report.Value("lu [,] " + typenames[ti] + " matrix factorization error",
                            lu2TypeStats[ti]);
                Report.Value("lu [,] factorization log error histogram", lu2HistoStats.Histogram);
                if (showWorst)
                {
                    using (Report.Job("worst matrix")) WriteMat(lu2HistoStats.Stats.MaxData);
                    using (Report.Job("worst inverse")) WriteMat(lu2HistoStats.Stats.MaxData.LuInverse2());
                    using (Report.Job("worst result")) WriteMat(lu2HistoStats.Stats.MaxData
                                                                * lu2HistoStats.Stats.MaxData.LuInverse2());
                }
            }

            if (doGj2)
            {
                Report.Value("gauss jordan error", gj2HistoStats.Stats);
                if (showTypes)
                    for (int ti = 0; ti < tc; ti++)
                        Report.Value("gj " + typenames[ti] + " matrix factorization error",
                            gj2TypeStats[ti]);
                Report.Value("gauss jordan log error histogram", gj2HistoStats.Histogram);
                if (showWorst)
                {
                    using (Report.Job("worst matrix")) WriteMat(gj2HistoStats.Stats.MaxData);
                    using (Report.Job("worst inverse")) WriteMat(gj2HistoStats.Stats.MaxData.NumericallyInstableGjInverse2());
                    using (Report.Job("worst result")) WriteMat(gj2HistoStats.Stats.MaxData
                                                                * gj2HistoStats.Stats.MaxData.NumericallyInstableGjInverse2());
                }
            }

            if (doQrI)
            {
                Report.Value("qr factorization error", qr2HistoStats.Stats);
                if (showTypes)
                    for (int ti = 0; ti < tc; ti++)
                        Report.Value("qr " + typenames[ti] + " matrix factorization error",
                            qriTypeStats[ti]);
                Report.Value("qr factorization log error histogram", qriHistoStats.Histogram);
                if (showWorst)
                {
                    using (Report.Job("worst matrix")) WriteMat(qriHistoStats.Stats.MaxData);
                    using (Report.Job("worst inverse")) WriteMat(qriHistoStats.Stats.MaxData.QrInverse());
                    using (Report.Job("worst result")) WriteMat(qriHistoStats.Stats.MaxData
                                                                * qriHistoStats.Stats.MaxData.QrInverse());
                }
            }
            if (failedQr2)
            {
                Report.Value("qr [,] factorization error", qr2HistoStats.Stats);
                if (showTypes)
                    for (int ti = 0; ti < tc; ti++)
                        Report.Value("qr [,] " + typenames[ti] + " matrix factorization error",
                            qr2TypeStats[ti]);
                Report.Value("qr [,] factorization log error histogram", qr2HistoStats.Histogram);
                if (showWorst)
                {
                    using (Report.Job("worst matrix")) WriteMat(qr2HistoStats.Stats.MaxData);
                    using (Report.Job("worst inverse")) WriteMat(qr2HistoStats.Stats.MaxData.QrInverse2());
                    using (Report.Job("worst result")) WriteMat(qr2HistoStats.Stats.MaxData
                                                                * qr2HistoStats.Stats.MaxData.QrInverse2());
                }
            }
            Test.End();
        }

        public static void WriteMat(M44d mat)
        {
            Report.Line("[ {0},", mat.R0.ToString("g4"));
            Report.Line("  {0},", mat.R1.ToString("g4"));
            Report.Line("  {0},", mat.R2.ToString("g4"));
            Report.Line("  {0} ]", mat.R3.ToString("g4"));
        }
    }

    public static class MatrixTestExtensions
    {
        public static M44d LuInverse2(this M44d m)
        {
            var lu = (double[,])m;
            return (M44d)lu.LuInverse(lu.LuFactorize());
        }

        /// <summary>
        /// Calculates the inverse Matrix to A using Householder-Transformations
        /// </summary>
        public static M44d QrInverse2(this M44d mat)
        {
            double[,] qr = (double[,])mat;
            double[] diag = qr.QrFactorize();
            double[,] inv = new double[4, 4];
            qr.QrInverse(diag, inv);
            return (M44d)inv;
        }

        public static readonly V2l s_luSize = new V2l(4, 4);
        public static readonly V2l s_luDelta = new V2l(1, 4);
        public static readonly Matrix<double> s_unit = new Matrix<double>(
                new double[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 }, 0, s_luSize, s_luDelta);

        public static M44d LuInverseM(this M44d m)
        {
            var lu = new Matrix<double>((double[])m, 0, s_luSize, s_luDelta);
            return (M44d)(lu.LuSolve(lu.LuFactorize(), s_unit).Data);
        }

        public static readonly Vector<double> s_c0 = new Vector<double>(new double[] { 1, 0, 0, 0 });
        public static readonly Vector<double> s_c1 = new Vector<double>(new double[] { 0, 1, 0, 0 });
        public static readonly Vector<double> s_c2 = new Vector<double>(new double[] { 0, 0, 1, 0 });
        public static readonly Vector<double> s_c3 = new Vector<double>(new double[] { 0, 0, 0, 1 });

        public static M44d LuInverseV(this M44d m)
        {
            var lu = new Matrix<double>((double[])m, 0, s_luSize, s_luDelta);
            var p = lu.LuFactorize();
            var inv = new M44d();
            var c0 = lu.LuSolve(p, s_c0); inv.M00 = c0.Data[0]; inv.M10 = c0.Data[1]; inv.M20 = c0.Data[2]; inv.M30 = c0.Data[3];
            var c1 = lu.LuSolve(p, s_c1); inv.M01 = c1.Data[0]; inv.M11 = c1.Data[1]; inv.M21 = c1.Data[2]; inv.M31 = c1.Data[3];
            var c2 = lu.LuSolve(p, s_c2); inv.M02 = c2.Data[0]; inv.M12 = c2.Data[1]; inv.M22 = c2.Data[2]; inv.M32 = c2.Data[3];
            var c3 = lu.LuSolve(p, s_c3); inv.M03 = c3.Data[0]; inv.M13 = c3.Data[1]; inv.M23 = c3.Data[2]; inv.M33 = c3.Data[3];
            return inv;
        }

        /// <summary>
        /// Calculates the inverse using gauss elemination.
        /// This is a more accurate calculation of the inverse (but slower).
        /// This method returns the inverse of the matrix to a new object.
        /// </summary>
        /// <returns>Returns the inverse of the matrix.</returns>
        public static M44d NumericallyInstableGjInverse2(this M44d mat)
        {
            int i, j, k;

            var work = (double[,])mat;
            var result = (double[,])M44d.Identity;

            for (i = 0; i < 3; i++)
            {
                int pivot = i;
                double pivotsize = System.Math.Abs(work[i, i]);

                for (j = i + 1; j < 4; j++)
                {
                    double r = work[j, i];

                    if (r < 0) r = -r;

                    if (r > pivotsize)
                    {
                        pivot = j;
                        pivotsize = r;
                    }
                }

                if (pivotsize == 0.0)
                    throw new ArgumentException(
                                "cannot invert singular matrix");

                if (pivot != i)
                {
                    for (j = 0; j < 4; j++)
                    {
                        double r;

                        r = work[i, j];
                        work[i, j] = work[pivot, j];
                        work[pivot, j] = r;

                        r = result[i, j];
                        result[i, j] = result[pivot, j];
                        result[pivot, j] = r;
                    }
                }

                for (j = i + 1; j < 4; j++)
                {
                    double f = work[j, i] / work[i, i];

                    for (k = 0; k < 4; k++)
                    {
                        work[j, k] -= f * work[i, k];
                        result[j, k] -= f * result[i, k];
                    }
                }
            }

            //backward substitution
            for (i = 3; i >= 0; --i)
            {
                double f;

                if ((f = work[i, i]) == 0)
                    throw new ArgumentException("cannot invert singular matrix");

                for (j = 0; j < 4; j++)
                {
                    work[i, j] /= f;
                    result[i, j] /= f;
                }

                for (j = 0; j < i; j++)
                {
                    f = work[j, i];

                    for (k = 0; k < 4; k++)
                    {
                        work[j, k] -= f * work[i, k];
                        result[j, k] -= f * result[i, k];
                    }
                }
            }
            return (M44d)result;
        }

    }

    public class HartleyTests : TestSuite
    {
        public HartleyTests() : base() { }
        public HartleyTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestHartley()
        {
            BitReversedIndexTest(20);
        }

        public static string BitString(int value, int bits)
        {
            StringBuilder s = new StringBuilder(bits);
            for (int mask = 1 << (bits - 1); mask > 0; mask >>= 1)
                s.Append((value & mask) != 0 ? 'I' : 'O');
            return s.ToString();
        }

        public void BitReversedIndexTest(int maxBits, bool report = false)
        {
            Test.Begin("BitReversedIndex tests");
            for (var bits = 1; bits <= maxBits; bits++)
            {
                int n = 1 << bits;
                Test.Begin("int[{0}]", n);
                var original = new int[n].SetByIndex(i => i);
                var reversed = original.BitReversedIndexCopy(0, n);
                var revTwice = reversed.Copy();
                revTwice.BitReverseIndex(0, n, 1);
                for (int i = 0; i < n; i++)
                    Test.IsTrue(original[i] == revTwice[i]);
                if (report && n < 32)
                    for (int i = 0; i < n; i++)
                        Report.Line("{0} -> {1}", BitString(original[i], bits), BitString(reversed[i], bits));
                Test.End();
            }
            Test.End();
        }

    }
}
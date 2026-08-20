using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class QrFactorizationTests
    {
        private const double Tolerance = 2e-12;

        [Test]
        public void SignedZeroPivotsChooseDeterministicFiniteReflectors()
        {
            double negativeZero = BitConverter.Int64BitsToDouble(long.MinValue);
            foreach (double zero in new[] { 0.0, negativeZero })
            {
                var managed = new double[,] { { zero, 1.0 } };
                double[] managedDiagonal = managed.QrFactorize();
                Assert.That(managedDiagonal[0], Is.EqualTo(-1.0));
                AssertFinite(managed, managedDiagonal);

                const long offset = 5;
                const long columnStride = 3;
                const long rowStride = 17;
                var strided = new double[32];
                strided[offset] = zero;
                strided[offset + columnStride] = 1.0;
                var stridedDiagonal = new double[1];
                strided.QrFactorize(offset, columnStride, rowStride, 2, 1, stridedDiagonal);
                Assert.That(stridedDiagonal[0], Is.EqualTo(-1.0));
                AssertFinite(strided, offset, columnStride, rowStride, 1, 2, stridedDiagonal);
            }
        }

        [Test]
        public void SquarePermutationMatricesFactorSolveAndInvert()
        {
            double negativeZero = BitConverter.Int64BitsToDouble(long.MinValue);
            foreach (double zero in new[] { 0.0, negativeZero })
            {
                var initialZero = new double[,]
                {
                    { zero, 1.0 },
                    { 1.0, 0.0 }
                };
                VerifyManaged(initialZero, new[] { 2.0, -3.0 }, verifyInverse: true);
                VerifyStrided(initialZero, new[] { 2.0, -3.0 });
            }

            var laterZero = new double[,]
            {
                { 1.0, 0.0, 0.0 },
                { 0.0, 0.0, 1.0 },
                { 0.0, 1.0, 0.0 }
            };
            VerifyManaged(laterZero, new[] { 2.0, -1.0, 3.0 }, verifyInverse: true);
            VerifyStrided(laterZero, new[] { 2.0, -1.0, 3.0 });
        }

        [Test]
        public void TallFullRankMatricesHandleInitialAndLaterZeroPivots()
        {
            var initialZero = new double[,]
            {
                { 0.0, 1.0 },
                { 1.0, 0.0 },
                { 0.0, 1.0 }
            };
            VerifyManaged(initialZero, new[] { 2.0, -1.0 }, verifyInverse: false);
            VerifyStrided(initialZero, new[] { 2.0, -1.0 });

            var laterZero = new double[,]
            {
                { 1.0, 0.0 },
                { 0.0, 0.0 },
                { 0.0, 1.0 }
            };
            VerifyManaged(laterZero, new[] { 2.0, -1.0 }, verifyInverse: false);
            VerifyStrided(laterZero, new[] { 2.0, -1.0 });
        }

        [Test]
        public void WideFullRankMatricesHandleInitialAndLaterZeroPivots()
        {
            var initialZero = new double[,]
            {
                { 0.0, 1.0, 0.0 },
                { 1.0, 0.0, 0.0 }
            };
            VerifyManaged(initialZero, new[] { 2.0, -1.0, 0.0 }, verifyInverse: false);
            VerifyStrided(initialZero, new[] { 2.0, -1.0, 0.0 });

            var laterZero = new double[,]
            {
                { 1.0, 0.0, 0.0 },
                { 0.0, 0.0, 1.0 }
            };
            VerifyManaged(laterZero, new[] { 2.0, 0.0, -1.0 }, verifyInverse: false);
            VerifyStrided(laterZero, new[] { 2.0, 0.0, -1.0 });
        }

        private static void VerifyManaged(double[,] matrix, double[] expectedSolution, bool verifyInverse)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[] rightHandSide = Multiply(matrix, expectedSolution);
            var qr = (double[,])matrix.Clone();

            double[] diagonal = qr.QrFactorize();
            AssertFinite(qr, diagonal);

            double[] solution = qr.QrSolve(diagonal, rightHandSide, out double residual);
            Assert.That(residual, Is.EqualTo(0.0).Within(Tolerance));
            AssertVector(expectedSolution, solution);
            AssertVector(rightHandSide, Multiply(matrix, solution));

            if (verifyInverse)
            {
                var inverse = new double[cols, rows];
                qr.QrInverse(diagonal, inverse);
                AssertFinite(inverse, Array.Empty<double>());
                AssertIdentity(Multiply(matrix, inverse));

                double[,] convenienceInverse = matrix.QrInverse();
                AssertFinite(convenienceInverse, Array.Empty<double>());
                AssertIdentity(Multiply(matrix, convenienceInverse));
            }
        }

        private static void VerifyStrided(double[,] matrix, double[] expectedSolution)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            const long offset = 5;
            const long columnStride = 3;
            const long rowStride = 17;
            int matrixLength = checked((int)(offset + (rows - 1) * rowStride + (cols - 1) * columnStride + 1));
            var qr = new double[matrixLength + 4];
            Array.Fill(qr, -999.0);
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    qr[offset + row * rowStride + col * columnStride] = matrix[row, col];

            var diagonal = new double[Math.Min(rows, cols)];
            qr.QrFactorize(offset, columnStride, rowStride, cols, rows, diagonal);
            AssertFinite(qr, offset, columnStride, rowStride, rows, cols, diagonal);

            double[] rightHandSide = Multiply(matrix, expectedSolution);
            const long rightHandSideOffset = 2;
            const long rightHandSideStride = 3;
            var rightHandSideStorage = new double[rightHandSideOffset + (rows - 1) * rightHandSideStride + 1];
            for (int i = 0; i < rows; i++)
                rightHandSideStorage[rightHandSideOffset + i * rightHandSideStride] = rightHandSide[i];

            const long solutionOffset = 1;
            const long solutionStride = 4;
            var solutionStorage = new double[solutionOffset + (cols - 1) * solutionStride + 1];
            qr.QrSolve(
                offset, columnStride, rowStride, cols, rows, diagonal,
                rightHandSideStorage, rightHandSideOffset, rightHandSideStride,
                solutionStorage, solutionOffset, solutionStride,
                out double residual);

            var solution = new double[cols];
            for (int i = 0; i < cols; i++) solution[i] = solutionStorage[solutionOffset + i * solutionStride];

            Assert.That(residual, Is.EqualTo(0.0).Within(Tolerance));
            AssertVector(expectedSolution, solution);
            AssertVector(rightHandSide, Multiply(matrix, solution));
        }

        private static double[] Multiply(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            var result = new double[rows];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    result[row] += matrix[row, col] * vector[col];
            return result;
        }

        private static double[,] Multiply(double[,] left, double[,] right)
        {
            int rows = left.GetLength(0);
            int inner = left.GetLength(1);
            int cols = right.GetLength(1);
            var result = new double[rows, cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    for (int i = 0; i < inner; i++)
                        result[row, col] += left[row, i] * right[i, col];
            return result;
        }

        private static void AssertFinite(double[,] matrix, double[] diagonal)
        {
            foreach (double value in matrix) Assert.That(double.IsFinite(value), Is.True);
            foreach (double value in diagonal) Assert.That(double.IsFinite(value), Is.True);
        }

        private static void AssertFinite(
            double[] matrix, long offset, long columnStride, long rowStride,
            int rows, int cols, double[] diagonal)
        {
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    Assert.That(double.IsFinite(matrix[offset + row * rowStride + col * columnStride]), Is.True);
            foreach (double value in diagonal) Assert.That(double.IsFinite(value), Is.True);
        }

        private static void AssertVector(double[] expected, double[] actual)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length));
            for (int i = 0; i < expected.Length; i++)
                Assert.That(actual[i], Is.EqualTo(expected[i]).Within(Tolerance), $"element {i}");
        }

        private static void AssertIdentity(double[,] matrix)
        {
            Assert.That(matrix.GetLength(0), Is.EqualTo(matrix.GetLength(1)));
            for (int row = 0; row < matrix.GetLength(0); row++)
                for (int col = 0; col < matrix.GetLength(1); col++)
                    Assert.That(matrix[row, col], Is.EqualTo(row == col ? 1.0 : 0.0).Within(Tolerance), $"[{row}, {col}]");
        }
    }
}

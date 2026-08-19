using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class LuFactorizationTests
    {
        private static unsafe void AssertFloatVariants(
            float[] values, int order, bool expected, int expectedFirstPermutation = -1)
        {
            var managed = new float[order, order];
            for (int row = 0; row < order; row++)
                for (int column = 0; column < order; column++)
                    managed[row, column] = values[row * order + column];

            var managedPermutation = new int[order];
            Assert.That(managed.LuFactorize(managedPermutation), Is.EqualTo(expected), "float[,]");

            const long offset = 2;
            const long columnStride = 2;
            long rowStride = 2L * order + 3;
            int stridedLength = order == 0
                ? (int)offset + 1
                : (int)(offset + (order - 1L) * (rowStride + columnStride) + 1);
            var strided = new float[stridedLength];
            for (int row = 0; row < order; row++)
                for (int column = 0; column < order; column++)
                    strided[offset + row * rowStride + column * columnStride] = values[row * order + column];

            var pointerValues = (float[])strided.Clone();
            var stridedPermutation = new int[order];
            Assert.That(
                strided.LuFactorize(offset, columnStride, rowStride, stridedPermutation),
                Is.EqualTo(expected),
                "strided float[]"
            );

            var pointerPermutation = new int[order];
            bool pointerResult;
            fixed (float* matrix = pointerValues)
            fixed (int* permutation = pointerPermutation)
                pointerResult = NumericExtensions.LuFactorize(
                    matrix, offset, columnStride, rowStride, permutation, order
                );
            Assert.That(pointerResult, Is.EqualTo(expected), "float*");

            if (expectedFirstPermutation >= 0)
            {
                Assert.That(managedPermutation[0], Is.EqualTo(expectedFirstPermutation), "float[,] permutation");
                Assert.That(stridedPermutation[0], Is.EqualTo(expectedFirstPermutation), "strided float[] permutation");
                Assert.That(pointerPermutation[0], Is.EqualTo(expectedFirstPermutation), "float* permutation");
            }
        }

        private static unsafe void AssertDoubleVariants(
            double[] values, int order, bool expected, int expectedFirstPermutation = -1)
        {
            var managed = new double[order, order];
            for (int row = 0; row < order; row++)
                for (int column = 0; column < order; column++)
                    managed[row, column] = values[row * order + column];

            var managedPermutation = new int[order];
            Assert.That(managed.LuFactorize(managedPermutation), Is.EqualTo(expected), "double[,]");

            const long offset = 2;
            const long columnStride = 2;
            long rowStride = 2L * order + 3;
            int stridedLength = order == 0
                ? (int)offset + 1
                : (int)(offset + (order - 1L) * (rowStride + columnStride) + 1);
            var strided = new double[stridedLength];
            for (int row = 0; row < order; row++)
                for (int column = 0; column < order; column++)
                    strided[offset + row * rowStride + column * columnStride] = values[row * order + column];

            var pointerValues = (double[])strided.Clone();
            var stridedPermutation = new int[order];
            Assert.That(
                strided.LuFactorize(offset, columnStride, rowStride, stridedPermutation),
                Is.EqualTo(expected),
                "strided double[]"
            );

            var pointerPermutation = new int[order];
            bool pointerResult;
            fixed (double* matrix = pointerValues)
            fixed (int* permutation = pointerPermutation)
                pointerResult = NumericExtensions.LuFactorize(
                    matrix, offset, columnStride, rowStride, permutation, order
                );
            Assert.That(pointerResult, Is.EqualTo(expected), "double*");

            if (expectedFirstPermutation >= 0)
            {
                Assert.That(managedPermutation[0], Is.EqualTo(expectedFirstPermutation), "double[,] permutation");
                Assert.That(stridedPermutation[0], Is.EqualTo(expectedFirstPermutation), "strided double[] permutation");
                Assert.That(pointerPermutation[0], Is.EqualTo(expectedFirstPermutation), "double* permutation");
            }
        }

        private static unsafe void AssertComplexVariants(
            ComplexD[] values, int order, bool expected, int expectedFirstPermutation = -1)
        {
            var managed = new ComplexD[order, order];
            for (int row = 0; row < order; row++)
                for (int column = 0; column < order; column++)
                    managed[row, column] = values[row * order + column];

            var managedPermutation = new int[order];
            Assert.That(managed.LuFactorize(managedPermutation), Is.EqualTo(expected), "ComplexD[,]");

            const long offset = 2;
            const long columnStride = 2;
            long rowStride = 2L * order + 3;
            int stridedLength = order == 0
                ? (int)offset + 1
                : (int)(offset + (order - 1L) * (rowStride + columnStride) + 1);
            var strided = new ComplexD[stridedLength];
            for (int row = 0; row < order; row++)
                for (int column = 0; column < order; column++)
                    strided[offset + row * rowStride + column * columnStride] = values[row * order + column];

            var pointerValues = (ComplexD[])strided.Clone();
            var stridedPermutation = new int[order];
            Assert.That(
                strided.LuFactorize(offset, columnStride, rowStride, stridedPermutation),
                Is.EqualTo(expected),
                "strided ComplexD[]"
            );

            var pointerPermutation = new int[order];
            bool pointerResult;
            fixed (ComplexD* matrix = pointerValues)
            fixed (int* permutation = pointerPermutation)
                pointerResult = NumericExtensions.LuFactorize(
                    matrix, offset, columnStride, rowStride, permutation, order
                );
            Assert.That(pointerResult, Is.EqualTo(expected), "ComplexD*");

            if (expectedFirstPermutation >= 0)
            {
                Assert.That(managedPermutation[0], Is.EqualTo(expectedFirstPermutation), "ComplexD[,] permutation");
                Assert.That(stridedPermutation[0], Is.EqualTo(expectedFirstPermutation), "strided ComplexD[] permutation");
                Assert.That(pointerPermutation[0], Is.EqualTo(expectedFirstPermutation), "ComplexD* permutation");
            }
        }

        [Test]
        public void ZeroOrderFactorizationSucceedsForAllStorageAndTypes()
        {
            AssertFloatVariants(Array.Empty<float>(), 0, true);
            AssertDoubleVariants(Array.Empty<double>(), 0, true);
            AssertComplexVariants(Array.Empty<ComplexD>(), 0, true);

            Assert.That(new float[0, 0].LuFactorize(), Is.Empty);
            Assert.That(new double[0, 0].LuFactorize(), Is.Empty);
            Assert.That(new ComplexD[0, 0].LuFactorize(), Is.Empty);
        }

        [Test]
        public void SingularOneByOneFactorizationFailsForAllStorageAndTypes()
        {
            AssertFloatVariants(new[] { 0.0f }, 1, false);
            AssertDoubleVariants(new[] { 0.0 }, 1, false);
            AssertComplexVariants(new[] { ComplexD.Zero }, 1, false);
        }

        [Test]
        public void FinalPivotRankDeficiencyFailsForAllStorageAndTypes()
        {
            AssertFloatVariants(new[] { 1.0f, 2.0f, 2.0f, 4.0f }, 2, false);
            AssertDoubleVariants(new[] { 1.0, 2.0, 2.0, 4.0 }, 2, false);
            AssertComplexVariants(new ComplexD[] { 1.0, 2.0, 2.0, 4.0 }, 2, false);
        }

        [Test]
        public void TinyFinalPivotsUseTheExistingTolerance()
        {
            float tinyFloat = Constant<float>.PositiveTinyValue * 0.5f;
            double tinyDouble = Constant<double>.PositiveTinyValue * 0.5;

            AssertFloatVariants(new[] { 1.0f, 0.0f, 0.0f, tinyFloat }, 2, false);
            AssertDoubleVariants(new[] { 1.0, 0.0, 0.0, tinyDouble }, 2, false);
            AssertComplexVariants(
                new ComplexD[] { 1.0, 0.0, 0.0, new ComplexD(tinyDouble, tinyDouble) },
                2,
                false
            );
            AssertComplexVariants(
                new ComplexD[] { 1.0, 0.0, 0.0, new ComplexD(0.0, 2.0 * Constant<double>.PositiveTinyValue) },
                2,
                true
            );
        }

        [Test]
        public void InitialRowPivotingStillSucceedsForAllStorageAndTypes()
        {
            AssertFloatVariants(new[] { 0.0f, 1.0f, 2.0f, 3.0f }, 2, true, 1);
            AssertDoubleVariants(new[] { 0.0, 1.0, 2.0, 3.0 }, 2, true, 1);
            AssertComplexVariants(new ComplexD[] { 0.0, 1.0, 2.0, 3.0 }, 2, true, 1);
        }

        [Test]
        public void ThrowingOverloadsRejectFinalPivotRankDeficiency()
        {
            Assert.Throws<ArgumentException>(() => new float[,] { { 1.0f, 2.0f }, { 2.0f, 4.0f } }.LuFactorize());
            Assert.Throws<ArgumentException>(() => new double[,] { { 1.0, 2.0 }, { 2.0, 4.0 } }.LuFactorize());
            Assert.Throws<ArgumentException>(() => new ComplexD[,] { { 1.0, 2.0 }, { 2.0, 4.0 } }.LuFactorize());
        }

        [Test]
        public void FixedSizeInversionReportsFailureWithoutMutatingInputs()
        {
            var singular22 = new M22d(
                1.0, 2.0,
                2.0, 4.0
            );
            var inPlace22 = singular22;
            Assert.That(inPlace22.LuInvert(), Is.False);
            Assert.That(inPlace22, Is.EqualTo(singular22));
            Assert.That(singular22.LuInverse(), Is.EqualTo(M22d.Zero));

            var singular33 = new M33d(
                1.0, 0.0, 1.0,
                0.0, 1.0, 1.0,
                1.0, 1.0, 2.0
            );
            var inPlace33 = singular33;
            Assert.That(inPlace33.LuInvert(), Is.False);
            Assert.That(inPlace33, Is.EqualTo(singular33));
            Assert.That(singular33.LuInverse(), Is.EqualTo(M33d.Zero));

            var singular44 = new M44d(
                1.0, 0.0, 0.0, 1.0,
                0.0, 1.0, 0.0, 1.0,
                0.0, 0.0, 1.0, 1.0,
                1.0, 1.0, 1.0, 3.0
            );
            var inPlace44 = singular44;
            Assert.That(inPlace44.LuInvert(), Is.False);
            Assert.That(inPlace44, Is.EqualTo(singular44));
            Assert.That(singular44.LuInverse(), Is.EqualTo(M44d.Zero));
        }
    }
}

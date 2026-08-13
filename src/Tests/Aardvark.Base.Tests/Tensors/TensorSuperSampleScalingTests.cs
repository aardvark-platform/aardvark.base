using System;
using Aardvark.Base;
using NUnit.Framework;
using Half = Aardvark.Base.Half;

namespace Aardvark.Tests
{
    [TestFixture]
    [NonParallelizable]
    public class TensorSuperSampleScalingTests
    {
        [Test]
        public void IntegerFootprintsUseExactSourceArea()
        {
            var source = new Matrix<double>(new[]
            {
                 0.0,  2.0,  4.0,  6.0,
                10.0, 12.0, 14.0, 16.0,
                20.0, 22.0, 24.0, 26.0,
                30.0, 32.0, 34.0, 36.0
            }, 4, 4);
            var target = new Matrix<double>(2, 2);

            target.SetScaledSuperSample(source);

            Assert.That(target.Data, Is.EqualTo(new[] { 6.0, 10.0, 26.0, 30.0 }));
        }

        [Test]
        public void FractionalFootprintsUseExactSourceArea()
        {
            var source = new Matrix<double>(5, 3);
            for (long y = 0; y < source.SY; y++)
                for (long x = 0; x < source.SX; x++)
                    source[x, y] = x + 10.0 * y;

            var target = new Matrix<double>(3, 2);
            target.SetScaledSuperSample(source);

            var expected = new[]
            {
                56.0 / 15.0, 16.0 / 3.0, 104.0 / 15.0,
                256.0 / 15.0, 56.0 / 3.0, 304.0 / 15.0
            };
            for (var i = 0; i < expected.Length; i++)
                Assert.That(target.Data[i], Is.EqualTo(expected[i]).Within(1e-12), $"index {i}");
        }

        [Test]
        public void ConstantsAnisotropicAndOnePixelOutputsArePreserved()
        {
            var constant = new Matrix<float>(7, 5, 17.25f);
            var reduced = new Matrix<float>(4, 3);
            reduced.SetScaledSuperSample(constant);
            Assert.That(reduced.Data, Is.All.EqualTo(17.25f));

            var source = new Matrix<double>(new[]
            {
                 0.0,  2.0,  4.0,  6.0,
                10.0, 12.0, 14.0, 16.0
            }, 4, 2);
            var anisotropic = new Matrix<double>(2, 2);
            anisotropic.SetScaledSuperSample(source);
            Assert.That(anisotropic.Data, Is.EqualTo(new[] { 1.0, 5.0, 11.0, 15.0 }));

            var onePixel = new Matrix<double>(1, 1);
            onePixel.SetScaledSuperSample(source);
            Assert.That(onePixel[0, 0], Is.EqualTo(8.0));
        }

        [Test]
        public void EverySupportedComponentTypeUsesExpectedConversion()
        {
            var byteTarget = new Matrix<byte>(1, 1);
            byteTarget.SetScaledSuperSample(new Matrix<byte>(new byte[] { 0, 1 }, 2, 1));
            Assert.That(byteTarget[0, 0], Is.EqualTo(1));

            var ushortTarget = new Matrix<ushort>(1, 1);
            ushortTarget.SetScaledSuperSample(new Matrix<ushort>(new ushort[] { 0, 1 }, 2, 1));
            Assert.That(ushortTarget[0, 0], Is.EqualTo(1));

            var uintTarget = new Matrix<uint>(1, 1);
            uintTarget.SetScaledSuperSample(new Matrix<uint>(new uint[] { 0, 1 }, 2, 1));
            Assert.That(uintTarget[0, 0], Is.EqualTo(1));

            var halfTarget = new Matrix<Half>(1, 1);
            halfTarget.SetScaledSuperSample(new Matrix<Half>(new[] { (Half)0.0f, (Half)1.0f }, 2, 1));
            Assert.That((float)halfTarget[0, 0], Is.EqualTo(0.5f));

            var floatTarget = new Matrix<float>(1, 1);
            floatTarget.SetScaledSuperSample(new Matrix<float>(new[] { 0.0f, 1.0f }, 2, 1));
            Assert.That(floatTarget[0, 0], Is.EqualTo(0.5f));

            var doubleTarget = new Matrix<double>(1, 1);
            doubleTarget.SetScaledSuperSample(new Matrix<double>(new[] { 0.0, 1.0 }, 2, 1));
            Assert.That(doubleTarget[0, 0], Is.EqualTo(0.5));
        }

        [Test]
        public void NonCanonicalPositiveStridesStayInsideTheirWindows()
        {
            var sourceData = new double[128];
            Array.Fill(sourceData, -1000.0);
            var source = new Matrix<double>(sourceData, 7, new V2l(4, 4), new V2l(3, 23));
            for (long y = 0; y < source.SY; y++)
                for (long x = 0; x < source.SX; x++)
                    source[x, y] = x + 10.0 * y;

            var targetData = new double[96];
            Array.Fill(targetData, -1.0);
            var target = new Matrix<double>(targetData, 5, new V2l(2, 2), new V2l(4, 31));

            target.SetScaledSuperSample(source);

            Assert.That(target[0, 0], Is.EqualTo(5.5));
            Assert.That(target[1, 0], Is.EqualTo(7.5));
            Assert.That(target[0, 1], Is.EqualTo(25.5));
            Assert.That(target[1, 1], Is.EqualTo(27.5));

            var written = new[] { 5, 9, 36, 40 };
            for (var i = 0; i < targetData.Length; i++)
            {
                if (Array.IndexOf(written, i) < 0)
                    Assert.That(targetData[i], Is.EqualTo(-1.0), $"guard index {i}");
            }
        }

        [Test]
        public void MultichannelScalingKeepsChannelsIsolated()
        {
            var data = new double[80];
            Array.Fill(data, -1.0);
            var source = new Volume<double>(data, 5, new V3l(4, 2, 3), new V3l(5, 31, 1));
            for (long y = 0; y < source.SY; y++)
            {
                for (long x = 0; x < source.SX; x++)
                {
                    source[x, y, 0] = 10.0;
                    source[x, y, 1] = 2.0 * x;
                    source[x, y, 2] = 10.0 * y;
                }
            }

            var target = source.Scaled(new V2d(0.5, 0.5), ImageInterpolation.SuperSample);

            Assert.That(target.Size, Is.EqualTo(new V3l(2, 1, 3)));
            Assert.That(target[0, 0, 0], Is.EqualTo(10.0));
            Assert.That(target[1, 0, 0], Is.EqualTo(10.0));
            Assert.That(target[0, 0, 1], Is.EqualTo(1.0));
            Assert.That(target[1, 0, 1], Is.EqualTo(5.0));
            Assert.That(target[0, 0, 2], Is.EqualTo(5.0));
            Assert.That(target[1, 0, 2], Is.EqualTo(5.0));
        }

        [Test]
        public void PixImageAndTensorEntryPointsAgree()
        {
            var source = ImageTensors.CreateImageVolume<double>(new V3l(4, 2, 1));
            for (long y = 0; y < source.SY; y++)
                for (long x = 0; x < source.SX; x++)
                    source[x, y, 0] = x + 10.0 * y;

            var direct = source.Scaled(new V2d(0.5, 0.5), ImageInterpolation.SuperSample);
            var image = new PixImage<double>(Col.Format.Gray, source);
            var viaPixImage = image.Scaled(new V2d(0.5, 0.5), ImageInterpolation.SuperSample);

            Assert.That(viaPixImage.Volume.Size, Is.EqualTo(direct.Size));
            Assert.That(viaPixImage.Volume.Data, Is.EqualTo(direct.Data));
        }

        [Test]
        public void EnlargingEitherAxisFallsBackToCubic()
        {
            var source = ImageTensors.CreateImageVolume<double>(new V3l(3, 4, 1));
            for (long y = 0; y < source.SY; y++)
                for (long x = 0; x < source.SX; x++)
                    source[x, y, 0] = x * x + y * 7.0;

            var factor = new V2d(2.0, 0.5);
            var superSample = source.Scaled(factor, ImageInterpolation.SuperSample);
            var cubic = source.Scaled(factor, ImageInterpolation.Cubic);
            Assert.That(superSample.Data, Is.EqualTo(cubic.Data));

            var image = new PixImage<double>(Col.Format.Gray, source);
            var viaPixImage = image.Scaled(factor, ImageInterpolation.SuperSample);
            Assert.That(viaPixImage.Volume.Data, Is.EqualTo(cubic.Data));

            var matrixSource = source.SubXYMatrixWindow(0);
            var matrixSuperSample = new Matrix<double>(6, 2);
            var matrixCubic = new Matrix<double>(6, 2);
            matrixSuperSample.SetScaledSuperSample(matrixSource);
            matrixCubic.SetScaledCubic(matrixSource);
            Assert.That(matrixSuperSample.Data, Is.EqualTo(matrixCubic.Data));
        }
    }
}

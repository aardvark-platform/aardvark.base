using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Aardvark.Base;

namespace Aardvark.Tests.Images
{
    [TestFixture]
    class PixImageTests
    {
        static readonly RandomSystem rnd = new(0);

        [Test]
        public void MipMapCreate1x1()
        {
            var pix = new PixImage<byte>(1, 1, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 1);
        }

        [Test]
        public void MipMapCreate2x2()
        {
            var pix = new PixImage<byte>(2, 2, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 2);
        }

        [Test]
        public void MipMapCreate3x3()
        {
            var pix = new PixImage<byte>(3, 3, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 2);
        }

        [Test]
        public void MipMapCreate256()
        {
            var pix = new PixImage<byte>(256, 256, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 9);
        }

        [Test]
        public void MipMapCreate255()
        {
            var pix = new PixImage<byte>(255, 255, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 8);
        }

        [Test]
        public void MipMapCreate257()
        {
            var pix = new PixImage<byte>(257, 257, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 9);
        }

        [Test]
        public void MipMapCreate57x43()
        {
            var pix = new PixImage<byte>(57, 43, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 6);
            //level 0: 57x43
            //level 1: 28x21
            //level 2: 14x10
            //level 3: 7x5
            //level 4: 3x2
            //level 5: 1x1
        }

        [Test]
        public void MipMapCreate57x11()
        {
            var pix = new PixImage<byte>(57, 11, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 6);
            //level 0: 57x11
            //level 1: 28x5
            //level 2: 14x2
            //level 3: 7x1
            //level 4: 3x1
            //level 5: 1x1
        }

        private static void FormatConversion<T>(Col.Format sourceFormat, Col.Format targetFormat, Func<T, byte[]> expectedConversion)
        {
            var src = new PixImage<byte>(sourceFormat, 43, 81);
            src.Volume.Data.SetByIndex((_) => (byte)rnd.UniformInt(256));
            var srcMatrix = src.GetMatrix<T>();

            var dst = src.ToFormat(targetFormat);

            srcMatrix.ForeachCoord((coord) =>
            {
                var expected = expectedConversion(srcMatrix[coord.XY]);

                var actual = new byte[dst.ChannelCount];
                actual.SetByIndex(i => dst.Volume[new V3l(coord, i)]);

                Assert.AreEqual(expected, actual);
            });
        }

        private static void FormatConversion<T1, T2>(Col.Format sourceFormat, Col.Format targetFormat, Func<T1, T2> expectedConversion)
        {
            var src = new PixImage<byte>(sourceFormat, 43, 81);
            src.Volume.Data.SetByIndex((_) => (byte)rnd.UniformInt(256));
            var srcMatrix = src.GetMatrix<T1>();

            var dst = src.ToFormat(targetFormat);
            var dstMatrix = dst.GetMatrix<T2>();

            dstMatrix.ForeachCoord((coord) =>
            {
                var expected = expectedConversion(srcMatrix[coord]);
                Assert.AreEqual(expected, dstMatrix[coord]);
            });
        }

        #region From Gray

        [Test]
        public void FormatConversionGrayToGrayAlpha()
            => FormatConversion<byte>(Col.Format.Gray, Col.Format.GrayAlpha, gray => new byte[] { gray, 255 });

        [Test]
        public void FormatConversionGrayToAlpha()
            => FormatConversion<byte, byte>(Col.Format.Gray, Col.Format.Alpha, gray => 255);

        [Test]
        public void FormatConversionGrayToRGB()
            => FormatConversion<byte, C3b>(Col.Format.Gray, Col.Format.RGB, gray => new C3b(gray));

        [Test]
        public void FormatConversionGrayToBGR()
            => FormatConversion<byte, C3b>(Col.Format.Gray, Col.Format.BGR, gray => new C3b(gray));

        [Test]
        public void FormatConversionGrayToRGBA()
            => FormatConversion<byte, C4b>(Col.Format.Gray, Col.Format.RGBA, gray => new C4b(gray));

        [Test]
        public void FormatConversionGrayToBGRA()
            => FormatConversion<byte, C4b>(Col.Format.Gray, Col.Format.BGRA, gray => new C4b(gray));

        #endregion

        #region From RGBA

        [Test]
        public void FormatConversionRGBAToBGRA()
            => FormatConversion<C4b, C4b>(Col.Format.RGBA, Col.Format.BGRA, color => color);

        [Test]
        public void FormatConversionRGBAToRGB()
            => FormatConversion<C4b, C3b>(Col.Format.RGBA, Col.Format.RGB, color => color.RGB);

        [Test]
        public void FormatConversionRGBAToBGR()
            => FormatConversion<C4b, C3b>(Col.Format.RGBA, Col.Format.BGR, color => color.RGB);

        [Test]
        public void FormatConversionRGBAToGray()
            => FormatConversion<C4b, byte>(Col.Format.RGBA, Col.Format.Gray, color => color.ToGrayByte());

        [Test]
        public void FormatConversionRGBAToGrayAlpha()
            => FormatConversion<C4b>(Col.Format.RGBA, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), color.A });

        #endregion

        #region From BGRA

        [Test]
        public void FormatConversionBGRAToRGBA()
            => FormatConversion<C4b, C4b>(Col.Format.BGRA, Col.Format.RGBA, color => color);

        [Test]
        public void FormatConversionBGRAToRGB()
            => FormatConversion<C4b, C3b>(Col.Format.BGRA, Col.Format.RGB, color => color.RGB);

        [Test]
        public void FormatConversionBGRAToBGR()
            => FormatConversion<C4b, C3b>(Col.Format.BGRA, Col.Format.BGR, color => color.RGB);

        [Test]
        public void FormatConversionBGRAToGray()
            => FormatConversion<C4b, byte>(Col.Format.BGRA, Col.Format.Gray, color => color.ToGrayByte());

        [Test]
        public void FormatConversionBGRAToGrayAlpha()
            => FormatConversion<C4b>(Col.Format.BGRA, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), color.A });

        #endregion

        #region From RGB

        [Test]
        public void FormatConversionRGBToBGRA()
            => FormatConversion<C3b, C4b>(Col.Format.RGB, Col.Format.BGRA, color => color.ToC4b());

        [Test]
        public void FormatConversionRGBToRGBA()
            => FormatConversion<C3b, C4b>(Col.Format.RGB, Col.Format.RGBA, color => color.ToC4b());

        [Test]
        public void FormatConversionRGBToBGR()
            => FormatConversion<C3b, C3b>(Col.Format.RGB, Col.Format.BGR, color => color);

        [Test]
        public void FormatConversionRGBToGray()
            => FormatConversion<C3b, byte>(Col.Format.RGB, Col.Format.Gray, color => color.ToGrayByte());

        [Test]
        public void FormatConversionRGBToGrayAlpha()
            => FormatConversion<C3b>(Col.Format.RGB, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), 255 });

        #endregion

        #region From BGR

        [Test]
        public void FormatConversionBGRToBGRA()
            => FormatConversion<C3b, C4b>(Col.Format.BGR, Col.Format.BGRA, color => color.ToC4b());

        [Test]
        public void FormatConversionBGRToRGBA()
            => FormatConversion<C3b, C4b>(Col.Format.BGR, Col.Format.RGBA, color => color.ToC4b());

        [Test]
        public void FormatConversionBGRToRGB()
            => FormatConversion<C3b, C3b>(Col.Format.BGR, Col.Format.RGB, color => color);

        [Test]
        public void FormatConversionBGRToGray()
            => FormatConversion<C3b, byte>(Col.Format.BGR, Col.Format.Gray, color => color.ToGrayByte());

        [Test]
        public void FormatConversionBGRToGrayAlpha()
            => FormatConversion<C3b>(Col.Format.BGR, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), 255 });

        #endregion
    }
}

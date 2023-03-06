using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Aardvark.Base;
using System.Runtime.InteropServices;

namespace Aardvark.Tests.Images
{
    [TestFixture]
    class PixImageTests
    {
        static readonly RandomSystem rnd = new(0);

        #region Mipmap

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

        #endregion

        #region Format Conversions

        private static byte[] GetArray(PixImage<byte> pi, V2l coord)
        {
            var arr = new byte[pi.ChannelCount];
            arr.SetByIndex(i => pi.Volume[new V3l(coord, i)]);
            return arr;
        }

        private static T GetColor<T>(PixImage<byte> pi, V2l coord)
            => pi.GetMatrix<T>()[coord];

        private static void FormatConversion<T1, T2>(Col.Format sourceFormat, Col.Format targetFormat,
                                                     Func<PixImage<byte>, V2l, T1> getInput,
                                                     Func<PixImage<byte>, V2l, T2> getActual,
                                                     Func<T1, T2> expectedConversion)
        {
            var src = new PixImage<byte>(sourceFormat, 43, 81);
            src.Volume.Data.SetByIndex((_) => (byte)rnd.UniformInt(256));

            var dst = src.ToFormat(targetFormat);

            src.GetChannel(0L).ForeachCoord((coord) =>
            {
                var expected = expectedConversion(getInput(src, coord));
                var actual = getActual(dst, coord);
                Assert.AreEqual(expected, actual);
            });
        }

        private static void FormatConversionArrays(Col.Format sourceFormat, Col.Format targetFormat, Func<byte[], byte[]> expectedConversion)
            => FormatConversion(sourceFormat, targetFormat, GetArray, GetArray, expectedConversion);

        private static void FormatConversionFromArray<T>(Col.Format sourceFormat, Col.Format targetFormat, Func<byte[], T> expectedConversion)
            => FormatConversion(sourceFormat, targetFormat, GetArray, GetColor<T>, expectedConversion);

        private static void FormatConversionToArray<T>(Col.Format sourceFormat, Col.Format targetFormat, Func<T, byte[]> expectedConversion)
            => FormatConversion(sourceFormat, targetFormat, GetColor<T>, GetArray, expectedConversion);

        private static void FormatConversion<T1, T2>(Col.Format sourceFormat, Col.Format targetFormat, Func<T1, T2> expectedConversion)
            => FormatConversion(sourceFormat, targetFormat, GetColor<T1>, GetColor<T2>, expectedConversion);

        #region From Gray

        [Test]
        public void FormatConversionGrayToGrayAlpha()
            => FormatConversionToArray<byte>(Col.Format.Gray, Col.Format.GrayAlpha, gray => new byte[] { gray, 255 });

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

        [Test]
        public void FormatConversionGrayToRG()
            => FormatConversionToArray<byte>(Col.Format.Gray, Col.Format.RG, gray => new byte[] { gray, gray });

        #endregion

        #region From GrayAlpha

        [Test]
        public void FormatConversionGrayAlphaToGray()
            => FormatConversionFromArray<byte>(Col.Format.GrayAlpha, Col.Format.Gray, gray => gray[0]);

        [Test]
        public void FormatConversionGrayAlphaToAlpha()
            => FormatConversionFromArray<byte>(Col.Format.GrayAlpha, Col.Format.Alpha, gray => gray[1]);

        [Test]
        public void FormatConversionGrayAlphaToRGB()
            => FormatConversionFromArray<C3b>(Col.Format.GrayAlpha, Col.Format.RGB, gray => new C3b(gray[0]));

        [Test]
        public void FormatConversionGrayAlphaToBGR()
            => FormatConversionFromArray<C3b>(Col.Format.GrayAlpha, Col.Format.BGR, gray => new C3b(gray[0]));

        [Test]
        public void FormatConversionGrayAlphaToRGBA()
            => FormatConversionFromArray<C4b>(Col.Format.GrayAlpha, Col.Format.RGBA, gray => new C4b(gray[0], gray[0], gray[0], gray[1]));

        [Test]
        public void FormatConversionGrayAlphaToBGRA()
            => FormatConversionFromArray<C4b>(Col.Format.GrayAlpha, Col.Format.BGRA, gray => new C4b(gray[0], gray[0], gray[0], gray[1]));

        [Test]
        public void FormatConversionGrayAlphaToRG()
            => FormatConversionArrays(Col.Format.GrayAlpha, Col.Format.RG, gray => new byte[] { gray[0], gray[0] });

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
            => FormatConversionToArray<C4b>(Col.Format.RGBA, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), color.A });

        [Test]
        public void FormatConversionRGBAToRG()
            => FormatConversionToArray<C4b>(Col.Format.RGBA, Col.Format.RG, color => new byte[] { color.R, color.G });

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
            => FormatConversionToArray<C4b>(Col.Format.BGRA, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), color.A });

        [Test]
        public void FormatConversionBGRAToRG()
            => FormatConversionToArray<C4b>(Col.Format.BGRA, Col.Format.RG, color => new byte[] { color.R, color.G });

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
            => FormatConversionToArray<C3b>(Col.Format.RGB, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), 255 });

        [Test]
        public void FormatConversionRGBToRG()
            => FormatConversionToArray<C3b>(Col.Format.RGB, Col.Format.RG, color => new byte[] { color.R, color.G });

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
            => FormatConversionToArray<C3b>(Col.Format.BGR, Col.Format.GrayAlpha, color => new byte[] { color.ToGrayByte(), 255 });

        [Test]
        public void FormatConversionBGRToRG()
            => FormatConversionToArray<C3b>(Col.Format.BGR, Col.Format.RG, color => new byte[] { color.R, color.G });

        #endregion

        #region From RG

        [Test]
        public void FormatConversionRGToBGRA()
            => FormatConversionFromArray<C4b>(Col.Format.RG, Col.Format.BGRA, color => new C4b(color[0], color[1], (byte)0, (byte)255));

        [Test]
        public void FormatConversionRGToRGBA()
            => FormatConversionFromArray<C4b>(Col.Format.RG, Col.Format.RGBA, color => new C4b(color[0], color[1], (byte)0, (byte)255));

        [Test]
        public void FormatConversionRGToRGB()
            => FormatConversionFromArray<C3b>(Col.Format.RG, Col.Format.RGB, color => new C3b(color[0], color[1], (byte)0));

        [Test]
        public void FormatConversionRGToBGR()
            => FormatConversionFromArray<C3b>(Col.Format.RG, Col.Format.BGR, color => new C3b(color[0], color[1], (byte)0));

        #endregion

        #endregion

        #region Creation

        struct Foo
        {
            public string Hugo;
            public int Bar;
        }

        [Test]
        public void CreateWithCustomFormat()
        {
            var format = new PixFormat(typeof(Foo), Col.Format.RG);
            var pi = PixImage.Create(format, 4, 4).AsPixImage<Foo>();

            pi.Volume[3, 2, 0] = new Foo() { Hugo = "Hey", Bar = 43 };

            var result = pi.Volume[3, 2, 0];
            Assert.AreEqual("Hey", result.Hugo);
            Assert.AreEqual(43, result.Bar);
        }

        [Test]
        public void CreateArrayWithCustomFormat()
        {
            var data = new Foo[]
            {
                new Foo() { Hugo = "A", Bar = 1 },
                new Foo() { Hugo = "B", Bar = 2 },
                new Foo() { Hugo = "C", Bar = 3 },
                new Foo() { Hugo = "D", Bar = 4 },
            };

            var pi = PixImage.Create(data, Col.Format.Gray, 2, 2).AsPixImage<Foo>();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], pi.Matrix[i % 2, i / 2]);
            }
        }

        #endregion
    }
}

using NUnit.Framework;
using System;
using Aardvark.Base;

namespace Aardvark.Tests.Images
{
    [TestFixture]
    class PixVolumeTests
    {
        static readonly RandomSystem rnd = new(0);

        #region Format Conversions

        private static byte[] GetArray(PixVolume<byte> pi, V3l coord)
        {
            var arr = new byte[pi.ChannelCount];
            arr.SetByIndex(i => pi.Tensor4[new V4l(coord, i)]);
            return arr;
        }

        private static T GetColor<T>(PixVolume<byte> pi, V3l coord)
            => pi.GetVolume<T>()[coord];

        private static void FormatConversion<T1, T2>(Col.Format sourceFormat, Col.Format targetFormat,
                                                     Func<PixVolume<byte>, V3l, T1> getInput,
                                                     Func<PixVolume<byte>, V3l, T2> getActual,
                                                     Func<T1, T2> expectedConversion,
                                                     bool subImageWindow)
        {
            var src = new PixVolume<byte>(sourceFormat, 43, 31, 23);
            src.Tensor4.Data.SetByIndex((_) => (byte)rnd.UniformInt(256));

            if (subImageWindow)
                src = new PixVolume<byte>(sourceFormat, src.Tensor4.SubImageWindow(11, 7, 3, 27, 13, 8));

            var dst = src.ToFormat(targetFormat);

            src.GetChannel(0L).ForeachCoord((coord) =>
            {
                var expected = expectedConversion(getInput(src, coord));
                var actual = getActual(dst, coord);
                Assert.AreEqual(expected, actual);
            });
        }

        private static void FormatConversionArrays(Col.Format sourceFormat, Col.Format targetFormat, Func<byte[], byte[]> expectedConversion, bool subImageWindow = false)
            => FormatConversion(sourceFormat, targetFormat, GetArray, GetArray, expectedConversion, subImageWindow);

        private static void FormatConversionFromArray<T>(Col.Format sourceFormat, Col.Format targetFormat, Func<byte[], T> expectedConversion, bool subImageWindow = false)
            => FormatConversion(sourceFormat, targetFormat, GetArray, GetColor<T>, expectedConversion, subImageWindow);

        private static void FormatConversionToArray<T>(Col.Format sourceFormat, Col.Format targetFormat, Func<T, byte[]> expectedConversion, bool subImageWindow = false)
            => FormatConversion(sourceFormat, targetFormat, GetColor<T>, GetArray, expectedConversion, subImageWindow);

        private static void FormatConversion<T1, T2>(Col.Format sourceFormat, Col.Format targetFormat, Func<T1, T2> expectedConversion, bool subImageWindow = false)
            => FormatConversion(sourceFormat, targetFormat, GetColor<T1>, GetColor<T2>, expectedConversion, subImageWindow);

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
        public void FormatConversionRGBAToRGBA()
            => FormatConversion<C4b, C4b>(Col.Format.RGBA, Col.Format.RGBA, color => color);

        [Test]
        public void FormatConversionRGBAToRGBAWindow()
            => FormatConversion<C4b, C4b>(Col.Format.RGBA, Col.Format.RGBA, color => color, true);

        [Test]
        public void FormatConversionRGBAToBGRA()
            => FormatConversion<C4b, C4b>(Col.Format.RGBA, Col.Format.BGRA, color => color);

        [Test]
        public void FormatConversionRGBAToBGRAWindow()
            => FormatConversion<C4b, C4b>(Col.Format.RGBA, Col.Format.BGRA, color => color, true);

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
    }
}

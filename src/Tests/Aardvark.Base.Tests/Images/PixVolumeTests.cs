using NUnit.Framework;
using System;
using Aardvark.Base;
using float16 = Aardvark.Base.Half;

namespace Aardvark.Tests.Images
{
    [TestFixture]
    class PixVolumeTests
    {
        static readonly RandomSystem rnd = new(0);

        #region Format Conversions

        private static T[] GetArray<T>(PixVolume<T> pi, V3l coord)
        {
            var arr = new T[pi.ChannelCount];
            arr.SetByIndex(i => pi.Tensor4[new V4l(coord, i)]);
            return arr;
        }

        private static Tc GetColor<T, Tc>(PixVolume<T> pi, V3l coord)
            => pi.GetVolume<Tc>()[coord];

        private static void FormatConversion<T1, T2, Tc1, Tc2>(Col.Format sourceFormat, Col.Format targetFormat,
                                                               Func<PixVolume<T1>, V3l, Tc1> getInput,
                                                               Func<PixVolume<T2>, V3l, Tc2> getActual,
                                                               Func<Tc1, Tc2> expectedConversion,
                                                               bool subImageWindow)
        {
            var getRandom = PixImageTests.s_randomValues[typeof(T1)];
            var src = new PixVolume<T1>(sourceFormat, 43, 31, 23);
            src.Tensor4.Data.SetByIndex((_) => (T1)getRandom(rnd));

            if (subImageWindow)
                src = new PixVolume<T1>(sourceFormat, src.Tensor4.SubImageWindow(11, 7, 3, 27, 13, 8));

            var dst = new PixVolume<T2>(targetFormat, src);

            src.GetChannel(0L).ForeachCoord((coord) =>
            {
                var expected = expectedConversion(getInput(src, coord));
                var actual = getActual(dst, coord);
                Assert.AreEqual(expected, actual);
            });
        }

        private static void FormatConversionArrays<T1, T2>(Col.Format sourceFormat, Col.Format targetFormat, Func<T1[], T2[]> expectedConversion, bool subImageWindow = false)
            => FormatConversion<T1, T2, T1[], T2[]>(sourceFormat, targetFormat, GetArray, GetArray, expectedConversion, subImageWindow);

        private static void FormatConversionFromArray<T1, T2, Tc2>(Col.Format sourceFormat, Col.Format targetFormat, Func<T1[], Tc2> expectedConversion, bool subImageWindow = false)
            => FormatConversion<T1, T2, T1[], Tc2>(sourceFormat, targetFormat, GetArray, GetColor<T2, Tc2>, expectedConversion, subImageWindow);

        private static void FormatConversionToArray<T1, T2, Tc1>(Col.Format sourceFormat, Col.Format targetFormat, Func<Tc1, T2[]> expectedConversion, bool subImageWindow = false)
            => FormatConversion<T1, T2, Tc1, T2[]>(sourceFormat, targetFormat, GetColor<T1, Tc1>, GetArray, expectedConversion, subImageWindow);

        private static void FormatConversion<T1, T2, Tc1, Tc2>(Col.Format sourceFormat, Col.Format targetFormat, Func<Tc1, Tc2> expectedConversion, bool subImageWindow = false)
            => FormatConversion<T1, T2, Tc1, Tc2>(sourceFormat, targetFormat, GetColor<T1, Tc1>, GetColor<T2, Tc2>, expectedConversion, subImageWindow);

        #region From Gray

        [Test]
        public void FormatConversionGrayToGrayAlpha()
            => FormatConversionToArray<byte, byte, byte>(Col.Format.Gray, Col.Format.GrayAlpha, gray => new byte[] { gray, 255 });

        [Test]
        public void FormatConversionGrayToAlpha()
            => FormatConversion<byte, byte, byte, byte>(Col.Format.Gray, Col.Format.Alpha, gray => 255);

        [Test]
        public void FormatConversionGrayToRGB()
            => FormatConversion<byte, byte, byte, C3b>(Col.Format.Gray, Col.Format.RGB, gray => new C3b(gray));

        [Test]
        public void FormatConversionGrayToBGR()
            => FormatConversion<byte, byte, byte, C3b>(Col.Format.Gray, Col.Format.BGR, gray => new C3b(gray));

        [Test]
        public void FormatConversionGrayToRGBA()
            => FormatConversion<byte, byte, byte, C4b>(Col.Format.Gray, Col.Format.RGBA, gray => new C4b(gray));

        [Test]
        public void FormatConversionGrayToBGRA()
            => FormatConversion<byte, byte, byte, C4b>(Col.Format.Gray, Col.Format.BGRA, gray => new C4b(gray));

        [Test]
        public void FormatConversionGrayToRG()
            => FormatConversionToArray<byte, byte, byte>(Col.Format.Gray, Col.Format.RG, gray => new byte[] { gray, gray });

        #endregion

        #region From GrayAlpha

        [Test]
        public void FormatConversionGrayAlphaToGray()
            => FormatConversionFromArray<byte, byte, byte>(Col.Format.GrayAlpha, Col.Format.Gray, gray => gray[0]);

        [Test]
        public void FormatConversionGrayAlphaToAlpha()
            => FormatConversionFromArray<byte, byte, byte>(Col.Format.GrayAlpha, Col.Format.Alpha, gray => gray[1]);

        [Test]
        public void FormatConversionGrayAlphaToRGB()
            => FormatConversionFromArray<byte, byte, C3b>(Col.Format.GrayAlpha, Col.Format.RGB, gray => new C3b(gray[0]));

        [Test]
        public void FormatConversionGrayAlphaToBGR()
            => FormatConversionFromArray<byte, byte, C3b>(Col.Format.GrayAlpha, Col.Format.BGR, gray => new C3b(gray[0]));

        [Test]
        public void FormatConversionGrayAlphaToRGBA()
            => FormatConversionFromArray<byte, byte, C4b>(Col.Format.GrayAlpha, Col.Format.RGBA, gray => new C4b(gray[0], gray[0], gray[0], gray[1]));

        [Test]
        public void FormatConversionGrayAlphaToBGRA()
            => FormatConversionFromArray<byte, byte, C4b>(Col.Format.GrayAlpha, Col.Format.BGRA, gray => new C4b(gray[0], gray[0], gray[0], gray[1]));

        [Test]
        public void FormatConversionGrayAlphaToRG()
            => FormatConversionArrays<byte, byte>(Col.Format.GrayAlpha, Col.Format.RG, gray => new byte[] { gray[0], gray[0] });

        #endregion

        #region From RGBA

        [Test]
        public void FormatConversionRGBAToRGBA()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.RGBA, c => c);

        [Test]
        public void FormatConversionRGBAToRGBAWindow()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.RGBA, c => c, true);

        [Test]
        public void FormatConversionRGBAToBGRA()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.BGRA, c => c);

        [Test]
        public void FormatConversionRGBAToBGRAWindow()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.BGRA, c => c, true);

        [Test]
        public void FormatConversionRGBAToRGB()
            => FormatConversion<byte, byte, C4b, C3b>(Col.Format.RGBA, Col.Format.RGB, c => c.RGB);

        [Test]
        public void FormatConversionRGBAToBGR()
            => FormatConversion<byte, byte, C4b, C3b>(Col.Format.RGBA, Col.Format.BGR, c => c.RGB);

        [Test]
        public void FormatConversionRGBAToGray()
            => FormatConversion<byte, byte, C4b, byte>(Col.Format.RGBA, Col.Format.Gray, c => c.ToGrayByte());

        [Test]
        public void FormatConversionRGBAToGrayAlpha()
            => FormatConversionToArray<byte, byte, C4b>(Col.Format.RGBA, Col.Format.GrayAlpha, c => new byte[] { c.ToGrayByte(), c.A });

        [Test]
        public void FormatConversionRGBAToRG()
            => FormatConversionToArray<byte, byte, C4b>(Col.Format.RGBA, Col.Format.RG, c => new byte[] { c.R, c.G });

        #endregion

        #region From BGRA

        [Test]
        public void FormatConversionBGRAToRGBA()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.BGRA, Col.Format.RGBA, c => c);

        [Test]
        public void FormatConversionBGRAToRGB()
            => FormatConversion<byte, byte, C4b, C3b>(Col.Format.BGRA, Col.Format.RGB, c => c.RGB);

        [Test]
        public void FormatConversionBGRAToBGR()
            => FormatConversion<byte, byte, C4b, C3b>(Col.Format.BGRA, Col.Format.BGR, c => c.RGB);

        [Test]
        public void FormatConversionBGRAToGray()
            => FormatConversion<byte, byte, C4b, byte>(Col.Format.BGRA, Col.Format.Gray, c => c.ToGrayByte());

        [Test]
        public void FormatConversionBGRAToGrayAlpha()
            => FormatConversionToArray<byte, byte, C4b>(Col.Format.BGRA, Col.Format.GrayAlpha, c => new byte[] { c.ToGrayByte(), c.A });

        [Test]
        public void FormatConversionBGRAToRG()
            => FormatConversionToArray<byte, byte, C4b>(Col.Format.BGRA, Col.Format.RG, c => new byte[] { c.R, c.G });

        #region Byte to BGRA (other types)

        [Test]
        public void TypeConversionByteToUShort()
            => FormatConversion<byte, ushort, C4b, C4us>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4us());

        [Test]
        public void TypeConversionByteToUInt()
            => FormatConversion<byte, uint, C4b, C4ui>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4ui());

        [Test]
        public void TypeConversionByteToHalf()
            => FormatConversionToArray<byte, float16, C4b>(Col.Format.RGBA, Col.Format.BGRA,
                c => new float16[] { c.B.ByteToHalf(), c.G.ByteToHalf(), c.R.ByteToHalf(), c.A.ByteToHalf() }
            );

        [Test]
        public void TypeConversionByteToFloat()
            => FormatConversion<byte, float, C4b, C4f>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4f());

        [Test]
        public void TypeConversionByteToDouble()
            => FormatConversion<byte, double, C4b, C4d>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4d());

        #endregion

        #region UShort to BGRA (other types)

        [Test]
        public void TypeConversionUShortToByte()
            => FormatConversion<ushort, byte, C4us, C4b>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4b());

        [Test]
        public void TypeConversionUShortToUInt()
            => FormatConversion<ushort, uint, C4us, C4ui>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4ui());

        [Test]
        public void TypeConversionUShortToHalf()
            => FormatConversionToArray<ushort, float16, C4us>(Col.Format.RGBA, Col.Format.BGRA,
                c => new float16[] { c.B.UShortToHalf(), c.G.UShortToHalf(), c.R.UShortToHalf(), c.A.UShortToHalf() }
            );

        [Test]
        public void TypeConversionUShortToFloat()
            => FormatConversion<ushort, float, C4us, C4f>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4f());

        [Test]
        public void TypeConversionUShortToDouble()
            => FormatConversion<ushort, double, C4us, C4d>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4d());

        #endregion

        #region UInt to BGRA (other types)

        [Test]
        public void TypeConversionUIntToByte()
            => FormatConversion<uint, byte, C4ui, C4b>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4b());

        [Test]
        public void TypeConversionUIntToUShort()
            => FormatConversion<uint, ushort, C4ui, C4us>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4us());

        [Test]
        public void TypeConversionUIntToHalf()
            => FormatConversionToArray<uint, float16, C4ui>(Col.Format.RGBA, Col.Format.BGRA,
                c => new float16[] { c.B.UIntToHalf(), c.G.UIntToHalf(), c.R.UIntToHalf(), c.A.UIntToHalf() }
            );

        [Test]
        public void TypeConversionUIntToFloat()
            => FormatConversion<uint, float, C4ui, C4f>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4f());

        [Test]
        public void TypeConversionUIntToDouble()
            => FormatConversion<uint, double, C4ui, C4d>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4d());

        #endregion

        #region Half to BGRA (other types)

        [Test]
        public void TypeConversionHalfToByte()
            => FormatConversionFromArray<float16, byte, C4b>(Col.Format.RGBA, Col.Format.BGRA, c =>
                new C4b(c[0].HalfToByte(), c[1].HalfToByte(), c[2].HalfToByte(), c[3].HalfToByte())
            );

        [Test]
        public void TypeConversionHalfToUShort()
            => FormatConversionFromArray<float16, ushort, C4us>(Col.Format.RGBA, Col.Format.BGRA, c =>
                new C4us(c[0].HalfToUShort(), c[1].HalfToUShort(), c[2].HalfToUShort(), c[3].HalfToUShort())
            );

        [Test]
        public void TypeConversionHalfToUInt()
            => FormatConversionFromArray<float16, uint, C4ui>(Col.Format.RGBA, Col.Format.BGRA, c =>
                new C4ui(c[0].HalfToUInt(), c[1].HalfToUInt(), c[2].HalfToUInt(), c[3].HalfToUInt())
            );

        [Test]
        public void TypeConversionHalfToFloat()
            => FormatConversionFromArray<float16, float, C4f>(Col.Format.RGBA, Col.Format.BGRA, c =>
                new C4f(c[0], c[1], c[2], c[3])
            );

        [Test]
        public void TypeConversionHalfToDouble()
            => FormatConversionFromArray<float16, double, C4d>(Col.Format.RGBA, Col.Format.BGRA, c =>
                new C4d(c[0], c[1], c[2], c[3])
            );

        #endregion

        #region Float to BGRA (other types)

        [Test]
        public void TypeConversionFloatToByte()
            => FormatConversion<float, byte, C4f, C4b>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4b());

        [Test]
        public void TypeConversionFloatToUShort()
            => FormatConversion<float, ushort, C4f, C4us>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4us());

        [Test]
        public void TypeConversionFloatToUInt()
            => FormatConversion<float, uint, C4f, C4ui>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4ui());

        [Test]
        public void TypeConversionFloatToHalf()
            => FormatConversionToArray<float, float16, C4f>(Col.Format.RGBA, Col.Format.BGRA,
                c => new float16[] { c.B.FloatToHalf(), c.G.FloatToHalf(), c.R.FloatToHalf(), c.A.FloatToHalf() }
            );


        [Test]
        public void TypeConversionFloatToDouble()
            => FormatConversion<float, double, C4f, C4d>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4d());

        #endregion

        #region Double to BGRA (other types)

        [Test]
        public void TypeConversionDoubleToByte()
            => FormatConversion<double, byte, C4d, C4b>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4b());

        [Test]
        public void TypeConversionDoubleToUShort()
            => FormatConversion<double, ushort, C4d, C4us>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4us());

        [Test]
        public void TypeConversionDoubleToUInt()
            => FormatConversion<double, uint, C4d, C4ui>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4ui());

        [Test]
        public void TypeConversionDoubleToHalf()
            => FormatConversionToArray<double, float16, C4d>(Col.Format.RGBA, Col.Format.BGRA,
                c => new float16[] { c.B.DoubleToHalf(), c.G.DoubleToHalf(), c.R.DoubleToHalf(), c.A.DoubleToHalf() }
            );

        [Test]
        public void TypeConversionDoubleToFloat()
            => FormatConversion<double, float, C4f, C4d>(Col.Format.RGBA, Col.Format.BGRA, c => c.ToC4d());

        #endregion

        #endregion

        #region From RGB

        [Test]
        public void FormatConversionRGBToBGRA()
            => FormatConversion<byte, byte, C3b, C4b>(Col.Format.RGB, Col.Format.BGRA, c => c.ToC4b());

        [Test]
        public void FormatConversionRGBToRGBA()
            => FormatConversion<byte, byte, C3b, C4b>(Col.Format.RGB, Col.Format.RGBA, c => c.ToC4b());

        [Test]
        public void FormatConversionRGBToBGR()
            => FormatConversion<byte, byte, C3b, C3b>(Col.Format.RGB, Col.Format.BGR, c => c);

        [Test]
        public void FormatConversionRGBToGray()
            => FormatConversion<byte, byte, C3b, byte>(Col.Format.RGB, Col.Format.Gray, c => c.ToGrayByte());

        [Test]
        public void FormatConversionRGBToGrayAlpha()
            => FormatConversionToArray<byte, byte, C3b>(Col.Format.RGB, Col.Format.GrayAlpha, c => new byte[] { c.ToGrayByte(), 255 });

        [Test]
        public void FormatConversionRGBToRG()
            => FormatConversionToArray<byte, byte, C3b>(Col.Format.RGB, Col.Format.RG, c => new byte[] { c.R, c.G });

        #endregion

        #region From BGR

        [Test]
        public void FormatConversionBGRToBGRA()
            => FormatConversion<byte, byte, C3b, C4b>(Col.Format.BGR, Col.Format.BGRA, c => c.ToC4b());

        [Test]
        public void FormatConversionBGRToRGBA()
            => FormatConversion<byte, byte, C3b, C4b>(Col.Format.BGR, Col.Format.RGBA, c => c.ToC4b());

        [Test]
        public void FormatConversionBGRToRGB()
            => FormatConversion<byte, byte, C3b, C3b>(Col.Format.BGR, Col.Format.RGB, c => c);

        [Test]
        public void FormatConversionBGRToGray()
            => FormatConversion<byte, byte, C3b, byte>(Col.Format.BGR, Col.Format.Gray, c => c.ToGrayByte());

        [Test]
        public void FormatConversionBGRToGrayAlpha()
            => FormatConversionToArray<byte, byte, C3b>(Col.Format.BGR, Col.Format.GrayAlpha, c => new byte[] { c.ToGrayByte(), 255 });

        [Test]
        public void FormatConversionBGRToRG()
            => FormatConversionToArray<byte, byte, C3b>(Col.Format.BGR, Col.Format.RG, c => new byte[] { c.R, c.G });

        #endregion

        #region From RG

        [Test]
        public void FormatConversionRGToBGRA()
            => FormatConversionFromArray<byte, byte, C4b>(Col.Format.RG, Col.Format.BGRA, c => new C4b(c[0], c[1], (byte)0, (byte)255));

        [Test]
        public void FormatConversionRGToRGBA()
            => FormatConversionFromArray<byte, byte, C4b>(Col.Format.RG, Col.Format.RGBA, c => new C4b(c[0], c[1], (byte)0, (byte)255));

        [Test]
        public void FormatConversionRGToRGB()
            => FormatConversionFromArray<byte, byte, C3b>(Col.Format.RG, Col.Format.RGB, c => new C3b(c[0], c[1], (byte)0));

        [Test]
        public void FormatConversionRGToBGR()
            => FormatConversionFromArray<byte, byte, C3b>(Col.Format.RG, Col.Format.BGR, c => new C3b(c[0], c[1], (byte)0));

        #endregion

        #endregion
    }
}

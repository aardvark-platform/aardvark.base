using NUnit.Framework;
using System;
using System.Collections.Generic;
using Aardvark.Base;
using float16 = Aardvark.Base.Half;

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

        internal static readonly Dictionary<Type, Func<RandomSystem, object>> s_randomValues = new ()
        {
            { typeof(byte), rnd => (byte)rnd.UniformInt(256) },
            { typeof(ushort), rnd => (ushort)rnd.UniformInt(Col.Info<ushort>.MaxValue + 1) },
            { typeof(uint), rnd => rnd.UniformUInt() },
            { typeof(float16), rnd => (float16)rnd.UniformFloat() },
            { typeof(float), rnd => rnd.UniformFloat() },
            { typeof(double), rnd => rnd.UniformDouble() },
        };

        private static T[] GetArray<T>(PixImage<T> pi, V2l coord)
        {
            var arr = new T[pi.ChannelCount];
            arr.SetByIndex(i => pi.Volume[new V3l(coord, i)]);
            return arr;
        }

        private static Tc GetColor<T, Tc>(PixImage<T> pi, V2l coord)
            => pi.GetMatrix<Tc>()[coord];

        private static void FormatConversion<T1, T2, Tc1, Tc2>(Col.Format sourceFormat, Col.Format targetFormat,
                                                               Func<PixImage<T1>, V2l, Tc1> getInput,
                                                               Func<PixImage<T2>, V2l, Tc2> getActual,
                                                               Func<Tc1, Tc2> expectedConversion,
                                                               bool subImageWindow)
        {

            var getRandom = s_randomValues[typeof(T1)];
            var src = new PixImage<T1>(sourceFormat, 43, 81);
            src.Volume.Data.SetByIndex((_) => (T1)getRandom(rnd));

            if (subImageWindow)
                src = new PixImage<T1>(sourceFormat, src.Volume.SubImageWindow(2, 3, 33, 67));

            var dst = new PixImage<T2>(targetFormat, src);

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
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.RGBA, (c => c));

        [Test]
        public void FormatConversionRGBAToRGBAWindow()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.RGBA, (c => c), true);

        [Test]
        public void FormatConversionRGBAToBGRA()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.BGRA, c => c);

        [Test]
        public void FormatConversionRGBAToBGRAWindow()
            => FormatConversion<byte, byte, C4b, C4b>(Col.Format.RGBA, Col.Format.BGRA, (c => c), true);

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

        #endregion

        #region From RGB

        [Test]
        public void FormatConversionRGBToRGBWithAdditionalChannel()
        {
            var getRandom = s_randomValues[typeof(byte)];
            var volume = ImageTensors.CreateImageVolume<byte>(new V3l(54, 23, 4));
            volume.Data.SetByIndex((_) => (byte)getRandom(rnd));
            var src = new PixImage<byte>(Col.Format.RGB, volume);
            var dst = new PixImage<byte>(Col.Format.RGB, src);

            src.GetChannel(0L).ForeachCoord((coord) =>
            {
                var expected = GetColor<byte, C3b>(src, coord);
                var actual = GetColor<byte, C3b>(dst, coord);
                Assert.AreEqual(expected, actual);
            });
        }

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

        #region  Stitch

        [Test]
        public void Stitch()
        {
            // Arrange tiles with varying sizes and null at [0,0]; ensure tiles do not align perfectly
            // Grid layout (rows x columns):
            // . G(2x2)
            // B(3x1) R(3x2)
            var red = new PixImage<byte>(Col.Format.RGB, new V2i(3, 2));
            red.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(255, 0, 0));

            var green = new PixImage<byte>(Col.Format.RGB, new V2i(2, 2));
            green.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(0, 255, 0));

            var blue = new PixImage<byte>(Col.Format.RGB, new V2i(3, 1));
            blue.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(0, 0, 255));

            // Note: null at [0,0], sizes cause gaps (non-perfect alignment)
            var grid = new[]
            {
                new[] { null,  green },
                new[] { blue,  red }
            };

            // Act (generic overload)
            var stitched = grid.Stitch();

            // Assert size:
            // sizesX = [max(0,3)=3, max(2,3)=3] -> 6; sizesY = [max(0,2)=2, max(1,2)=2] -> 4
            Assert.AreEqual(new V2i(6, 4), stitched.Size);

            var m = stitched.GetMatrix<C3b>();

            // Top-left 3x2 should be black (null tile)
            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 3; x++)
                    Assert.AreEqual(new C3b(0, 0, 0), m[x, y], $"Expected black at ({x},{y})");

            // Top-right area width is 3 but green is only 2 wide -> last column should be black
            for (int y = 0; y < 2; y++)
            {
                for (int x = 3; x < 5; x++) Assert.AreEqual(new C3b(0, 255, 0), m[x, y], $"Expected green at ({x},{y})");
                Assert.AreEqual(new C3b(0, 0, 0), m[5, y], $"Expected black gap at (5,{y})");
            }

            // Bottom-left row height is 2 but blue is only 1 high -> y=2 blue, y=3 black
            for (int x = 0; x < 3; x++) Assert.AreEqual(new C3b(0, 0, 255), m[x, 2], $"Expected blue at ({x},2)");
            for (int x = 0; x < 3; x++) Assert.AreEqual(new C3b(0, 0, 0), m[x, 3], $"Expected black at ({x},3)");

            // Bottom-right 3x2 should be red
            for (int y = 2; y < 4; y++)
                for (int x = 3; x < 6; x++)
                    Assert.AreEqual(new C3b(255, 0, 0), m[x, y], $"Expected red at ({x},{y})");

            // Also test non-generic overload with base PixImage[][] dispatch and compare size and a sample pixel
            PixImage[][] baseGrid =
            [
                [null, green],
                [blue, red]
            ];
            var stitchedBase = baseGrid.Stitch();
            Assert.IsNotNull(stitchedBase);
            Assert.AreEqual(stitched.Size, stitchedBase.Size);
            // Check a few sample pixels
            var mb = stitchedBase.AsPixImage<byte>().GetMatrix<C3b>();
            Assert.AreEqual(new C3b(0, 0, 0), mb[0, 0]);    // null tile area
            Assert.AreEqual(new C3b(0, 255, 0), mb[3, 0]);  // green start
            Assert.AreEqual(new C3b(0, 0, 0), mb[5, 0]);    // green gap column
            Assert.AreEqual(new C3b(0, 0, 255), mb[0, 2]);  // blue row
            Assert.AreEqual(new C3b(255, 0, 0), mb[3, 2]);  // red area

            // Mismatching formats should throw
            var gray = new PixImage<byte>(Col.Format.Gray, new V2i(1, 1));
            var badGrid = new PixImage<byte>[][] { [red, gray] };
            Assert.Throws<ArgumentException>(() => badGrid.Stitch());

            // Null and empty inputs should return null (extension methods accept null receivers)
            Assert.IsNull(PixImageExtensions.Stitch(null));
            Assert.IsNull(Array.Empty<PixImage<byte>[]>().Stitch());
        }

        [Test]
        public void StitchSquare()
        {
            // Arrange: 5 images with varying sizes to force gaps when arranged in a 3x3 near-square grid
            // Index mapping (row-major into 3x3):
            // [0]=R(2x1) [1]=G(1x2) [2]=B(3x1)
            // [3]=Y(1x1) [4]=M(2x2) [5]=null (implicit)
            // Row2 is entirely null
            var red = new PixImage<byte>(Col.Format.RGB, new V2i(2, 1));
            red.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(255, 0, 0));

            var green = new PixImage<byte>(Col.Format.RGB, new V2i(1, 2));
            green.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(0, 255, 0));

            var blue = new PixImage<byte>(Col.Format.RGB, new V2i(3, 1));
            blue.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(0, 0, 255));

            var yellow = new PixImage<byte>(Col.Format.RGB, new V2i(1, 1));
            yellow.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(255, 255, 0));

            var magenta = new PixImage<byte>(Col.Format.RGB, new V2i(2, 2));
            magenta.GetMatrix<C3b>().SetByCoord((x, y) => new C3b(255, 0, 255));

            var list = new PixImage<byte>[] { red, green, blue, yellow, magenta };

            // Act (generic overload)
            var stitched = list.StitchSquare();

            // Compute expected stitched size:
            // squareSize = ceil(sqrt(5)) = 3
            // Column widths = max per column across rows:
            // col0: max(2,1,0)=2; col1: max(1,2,0)=2; col2: max(3,0,0)=3 -> width = 7
            // Row heights = max per row across columns:
            // row0: max(1,2,1)=2; row1: max(1,2,0)=2; row2: 0 -> height = 4
            Assert.AreEqual(new V2i(7, 4), stitched.Size);

            var m = stitched.GetMatrix<C3b>();

            // Validate row 0 placements and gaps
            // Red (2x1) at x=[0..1], y=0; gap at y=1
            Assert.AreEqual(new C3b(255, 0, 0), m[0, 0]);
            Assert.AreEqual(new C3b(255, 0, 0), m[1, 0]);
            Assert.AreEqual(new C3b(0, 0, 0), m[0, 1]);
            Assert.AreEqual(new C3b(0, 0, 0), m[1, 1]);

            // Green (1x2) at x=2, y=[0..1]; gap column x=3 should be black (since column width=2)
            Assert.AreEqual(new C3b(0, 255, 0), m[2, 0]);
            Assert.AreEqual(new C3b(0, 255, 0), m[2, 1]);
            Assert.AreEqual(new C3b(0, 0, 0), m[3, 0]);
            Assert.AreEqual(new C3b(0, 0, 0), m[3, 1]);

            // Blue (3x1) at x=[4..6], y=0; gap at y=1 for the whole band
            Assert.AreEqual(new C3b(0, 0, 255), m[4, 0]);
            Assert.AreEqual(new C3b(0, 0, 255), m[5, 0]);
            Assert.AreEqual(new C3b(0, 0, 255), m[6, 0]);
            Assert.AreEqual(new C3b(0, 0, 0), m[4, 1]);
            Assert.AreEqual(new C3b(0, 0, 0), m[5, 1]);
            Assert.AreEqual(new C3b(0, 0, 0), m[6, 1]);

            // Validate row 1 placements and gaps (y=2..3)
            // Yellow (1x1) at (0,2); rest of its cell should be black
            Assert.AreEqual(new C3b(255, 255, 0), m[0, 2]);
            Assert.AreEqual(new C3b(0, 0, 0), m[1, 2]);
            Assert.AreEqual(new C3b(0, 0, 0), m[0, 3]);

            // Magenta (2x2) at x=[2..3], y=[2..3]
            Assert.AreEqual(new C3b(255, 0, 255), m[2, 2]);
            Assert.AreEqual(new C3b(255, 0, 255), m[3, 2]);
            Assert.AreEqual(new C3b(255, 0, 255), m[2, 3]);
            Assert.AreEqual(new C3b(255, 0, 255), m[3, 3]);

            // Right-most band in row 1 (x=4..6) should be black (no image in that cell)
            Assert.AreEqual(new C3b(0, 0, 0), m[4, 2]);
            Assert.AreEqual(new C3b(0, 0, 0), m[5, 2]);
            Assert.AreEqual(new C3b(0, 0, 0), m[6, 2]);
            Assert.AreEqual(new C3b(0, 0, 0), m[4, 3]);
            Assert.AreEqual(new C3b(0, 0, 0), m[5, 3]);
            Assert.AreEqual(new C3b(0, 0, 0), m[6, 3]);

            // Also test non-generic overload with base PixImage[] dispatch
            PixImage[] baseList = [red, green, blue, yellow, magenta];
            var stitchedBase = baseList.StitchSquare();
            Assert.IsNotNull(stitchedBase);
            Assert.AreEqual(stitched.Size, stitchedBase.Size);
            var mb = stitchedBase.AsPixImage<byte>().GetMatrix<C3b>();
            Assert.AreEqual(new C3b(255, 0, 0), mb[0, 0]);   // red
            Assert.AreEqual(new C3b(0, 255, 0), mb[2, 1]);   // green column
            Assert.AreEqual(new C3b(0, 0, 255), mb[6, 0]);   // blue
            Assert.AreEqual(new C3b(255, 255, 0), mb[0, 2]); // yellow
            Assert.AreEqual(new C3b(255, 0, 255), mb[3, 3]); // magenta

            // Mismatching formats should throw
            var gray = new PixImage<byte>(Col.Format.Gray, new V2i(1, 1));
            Assert.Throws<ArgumentException>(() => new PixImage<byte>[] { red, gray }.StitchSquare());

            // Null and empty inputs should return null (extension methods accept null receivers)
            Assert.IsNull(PixImageExtensions.StitchSquare<byte>(null));
            Assert.IsNull(Array.Empty<PixImage<byte>>().StitchSquare());
        }

        #endregion
    }
}

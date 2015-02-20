//#define USE_STORAGESERVICE
#if !__MonoCS__
#if !__ANDROID__
#define USE_SYSTEMIMAGE
#endif
#endif
#if USE_SYSTEMIMAGE
using System;
using System.Collections.Generic;
using System.IO;

using SdiPixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aardvark.Base
{
    public abstract partial class PixImage
    {

        #region Static Tables and Methods

        protected static Dictionary<PixFileFormat, Func<BitmapEncoder>> s_FormatEncoder =
            new Dictionary<PixFileFormat, Func<BitmapEncoder>>()
        {
            { PixFileFormat.Png, () => new PngBitmapEncoder() },
            { PixFileFormat.Bmp, () => new BmpBitmapEncoder() },
            { PixFileFormat.Jpeg, () => new JpegBitmapEncoder() },
            { PixFileFormat.Tiff, () => new TiffBitmapEncoder() },
            { PixFileFormat.Gif, () => new GifBitmapEncoder() },
            { PixFileFormat.Wmp, () => new WmpBitmapEncoder() }
        };

        protected static BitmapEncoder GetEncoder(PixFileFormat format)
        {
            Func<BitmapEncoder> encCreator;

            if (s_FormatEncoder.TryGetValue(format, out encCreator))
                return encCreator();

            return null;
        }

        protected static Dictionary<PixFormat, Tup<PixelFormat, Col.Format>> s_pixelFormatOfFormat =
            new Dictionary<PixFormat, Tup<PixelFormat, Col.Format>>()
            {
                { new PixFormat(typeof(byte), Col.Format.BW), Tup.Create(PixelFormats.BlackWhite, Col.Format.BW) },
                { new PixFormat(typeof(byte), Col.Format.Gray), Tup.Create(PixelFormats.Gray8, Col.Format.Gray) },
                { new PixFormat(typeof(byte), Col.Format.RGB), Tup.Create(PixelFormats.Bgr24, Col.Format.BGR) },
                { new PixFormat(typeof(byte), Col.Format.BGR), Tup.Create(PixelFormats.Bgr24, Col.Format.BGR) },
                { new PixFormat(typeof(byte), Col.Format.RGBA), Tup.Create(PixelFormats.Bgra32, Col.Format.BGRA) },
                { new PixFormat(typeof(byte), Col.Format.BGRA), Tup.Create(PixelFormats.Bgra32, Col.Format.BGRA) },
                { new PixFormat(typeof(byte), Col.Format.RGBP), Tup.Create(PixelFormats.Pbgra32, Col.Format.BGRP) },
                { new PixFormat(typeof(byte), Col.Format.BGRP), Tup.Create(PixelFormats.Pbgra32, Col.Format.BGRP) },

                { new PixFormat(typeof(ushort), Col.Format.Gray), Tup.Create(PixelFormats.Gray16, Col.Format.Gray) },
                { new PixFormat(typeof(ushort), Col.Format.RGB), Tup.Create(PixelFormats.Rgb48, Col.Format.RGB) },
                { new PixFormat(typeof(ushort), Col.Format.BGR), Tup.Create(PixelFormats.Rgb48, Col.Format.RGB) },
                { new PixFormat(typeof(ushort), Col.Format.RGBA), Tup.Create(PixelFormats.Rgba64, Col.Format.RGBA) },
                { new PixFormat(typeof(ushort), Col.Format.BGRA), Tup.Create(PixelFormats.Rgba64, Col.Format.RGBA) },
                { new PixFormat(typeof(ushort), Col.Format.RGBP), Tup.Create(PixelFormats.Prgba64, Col.Format.RGBP) },
                { new PixFormat(typeof(ushort), Col.Format.BGRP), Tup.Create(PixelFormats.Prgba64, Col.Format.RGBP) },

                { new PixFormat(typeof(float), Col.Format.Gray), Tup.Create(PixelFormats.Gray32Float, Col.Format.Gray) },
                { new PixFormat(typeof(float), Col.Format.RGB), Tup.Create(PixelFormats.Rgb128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.BGR), Tup.Create(PixelFormats.Rgb128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.RGBA), Tup.Create(PixelFormats.Rgba128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.BGRA), Tup.Create(PixelFormats.Rgba128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.RGBP), Tup.Create(PixelFormats.Prgba128Float, Col.Format.RGBP) },
                { new PixFormat(typeof(float), Col.Format.BGRP), Tup.Create(PixelFormats.Prgba128Float, Col.Format.RGBP) },
            };

        protected static Tup<PixelFormat, Col.Format> GetStoreFormats(Type type, Col.Format format)
        {
            Tup<PixelFormat, Col.Format> formatPair;
            if (s_pixelFormatOfFormat.TryGetValue(new PixFormat(type, format),
                                             out formatPair))
                return formatPair;
            throw new ArgumentException("unsupported pixel type");
        }

        protected static Dictionary<Tup<Type, int>, Tup<PixelFormat, Col.Format>> s_pixelFormatOfChannelCount =
            new Dictionary<Tup<Type, int>, Tup<PixelFormat, Col.Format>>()
            {
                { Tup.Create(typeof(byte), 1), Tup.Create(PixelFormats.Gray8, Col.Format.Gray) },
                { Tup.Create(typeof(byte), 3), Tup.Create(PixelFormats.Bgr24, Col.Format.BGR) },
                { Tup.Create(typeof(byte), 4), Tup.Create(PixelFormats.Bgra32, Col.Format.BGRA) },

                { Tup.Create(typeof(ushort), 1), Tup.Create(PixelFormats.Gray16, Col.Format.Gray) },
                { Tup.Create(typeof(ushort), 3), Tup.Create(PixelFormats.Rgb48, Col.Format.RGB) },
                { Tup.Create(typeof(ushort), 4), Tup.Create(PixelFormats.Rgba64, Col.Format.RGBA) },

                { Tup.Create(typeof(float), 1), Tup.Create(PixelFormats.Gray32Float, Col.Format.Gray) },
                { Tup.Create(typeof(float), 3), Tup.Create(PixelFormats.Rgb128Float, Col.Format.RGBA) },
                { Tup.Create(typeof(float), 4), Tup.Create(PixelFormats.Rgba128Float, Col.Format.RGBA) },
            };

        protected static Tup<PixelFormat, Col.Format> GetStoreFormats(Type type, int channelCount)
        {
            Tup<PixelFormat, Col.Format> format;
            if (s_pixelFormatOfChannelCount.TryGetValue(Tup.Create(type, channelCount), out format))
                return format;
            throw new ArgumentException("unsupported pixel type");
        }

        protected static Dictionary<SdiPixelFormat, PixelFormat> s_pixelFormatOfSdiPixelFormat =
            new Dictionary<SdiPixelFormat, PixelFormat>()
            {
                { SdiPixelFormat.Format1bppIndexed, PixelFormats.BlackWhite },
                { SdiPixelFormat.Format16bppGrayScale, PixelFormats.Gray16 },
 
                { SdiPixelFormat.Format24bppRgb, PixelFormats.Bgr24 },
                { SdiPixelFormat.Format32bppRgb, PixelFormats.Bgr32 },
                { SdiPixelFormat.Format32bppArgb, PixelFormats.Bgra32 },
                { SdiPixelFormat.Format32bppPArgb, PixelFormats.Pbgra32 },

                { SdiPixelFormat.Format48bppRgb, PixelFormats.Rgb48 },
                { SdiPixelFormat.Format64bppArgb, PixelFormats.Rgba64 },
                { SdiPixelFormat.Format64bppPArgb, PixelFormats.Prgba64 },
            };

        protected static Dictionary<SdiPixelFormat, Tup<PixFormat, int>> s_pixFormatAndCountOfSdiPixelFormat =
            new Dictionary<SdiPixelFormat, Tup<PixFormat, int>>()
            {
                { SdiPixelFormat.Format1bppIndexed, Tup.Create(PixFormat.ByteBW, 1) },

                { SdiPixelFormat.Format16bppGrayScale, Tup.Create(PixFormat.UShortGray, 1) },
 
                { SdiPixelFormat.Format24bppRgb, Tup.Create(PixFormat.ByteBGR, 3) },
                { SdiPixelFormat.Format32bppRgb, Tup.Create(PixFormat.ByteBGR, 4) },
                { SdiPixelFormat.Format32bppArgb, Tup.Create(PixFormat.ByteBGRA, 4) },
                { SdiPixelFormat.Format32bppPArgb, Tup.Create(PixFormat.ByteBGRP, 4) },

                { SdiPixelFormat.Format48bppRgb, Tup.Create(PixFormat.UShortBGR, 3) },
                { SdiPixelFormat.Format64bppArgb, Tup.Create(PixFormat.UShortBGRA, 4) },
                { SdiPixelFormat.Format64bppPArgb, Tup.Create(PixFormat.UShortBGRP, 4) },
            };

        protected static Dictionary<PixelFormat, Tup<PixFormat, int>> s_pixFormatAndCountOfPixelFormat =
            new Dictionary<PixelFormat, Tup<PixFormat, int>>()
            {
                { PixelFormats.BlackWhite, Tup.Create(PixFormat.ByteBW, 1) },

                { PixelFormats.Gray8, Tup.Create(PixFormat.ByteGray, 1) },
                { PixelFormats.Bgr24, Tup.Create(PixFormat.ByteBGR, 3) },
                { PixelFormats.Bgr32, Tup.Create(PixFormat.ByteBGR, 4) },
                { PixelFormats.Bgra32, Tup.Create(PixFormat.ByteBGRA, 4) },
                { PixelFormats.Pbgra32, Tup.Create(PixFormat.ByteBGRP, 4) },

                { PixelFormats.Rgb24, Tup.Create(PixFormat.ByteRGB, 3) },

                { PixelFormats.Gray16, Tup.Create(PixFormat.UShortGray, 1) },
                { PixelFormats.Rgb48, Tup.Create(PixFormat.UShortRGB, 3) },
                { PixelFormats.Rgba64, Tup.Create(PixFormat.UShortRGBA, 4) },
                { PixelFormats.Prgba64, Tup.Create(PixFormat.UShortRGBP, 4) },

                { PixelFormats.Gray32Float, Tup.Create(PixFormat.FloatGray, 1) },
                { PixelFormats.Rgb128Float, Tup.Create(PixFormat.FloatRGB, 4) },
                { PixelFormats.Rgba128Float, Tup.Create(PixFormat.FloatRGBA, 4) },
                { PixelFormats.Prgba128Float, Tup.Create(PixFormat.FloatRGBP, 4) }, 
            };

        public static PixFormat PixFormatOfPixelFormat(PixelFormat pixelFormat)
        {
            return s_pixFormatAndCountOfPixelFormat[pixelFormat].E0;
        }

        /// <summary>
        /// Creates a new image from the file. Note that in this raw image the channel
        /// count of the volume and the channel count of the format may be different.
        /// </summary>
        protected static PixImage CreateRawSystem(string filename, PixLoadOptions options = PixLoadOptions.Default)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.None
                                 | BitmapCreateOptions.PreservePixelFormat;
            bitmapImage.EndInit();
            return CreateRaw((BitmapSource)bitmapImage);
        }

        protected static PixImage CreateRaw(BitmapSource bitmapSource)
        {
            var pixelFormat = bitmapSource.Format;
            var fcBitmap = new FormatConvertedBitmap(bitmapSource, pixelFormat, null, 0);
            var pfc = s_pixFormatAndCountOfPixelFormat[pixelFormat];
            return CreateRaw(fcBitmap, pfc.E0, pfc.E1);
        }

        protected static PixImage CreateRaw(FormatConvertedBitmap fcBitmap, PixFormat pixFormat, int channels)
        {
            var sx = fcBitmap.PixelWidth;
            var sy = fcBitmap.PixelHeight;
            if (pixFormat.Format == Col.Format.BW)
            {
                var bitImage = new PixImage<byte>(Col.Format.BW, 1 + (sx - 1) / 8, sy, channels);
                fcBitmap.CopyPixels(bitImage.Array, bitImage.IntStride, 0);
                var pixImage = new PixImage<byte>(Col.Format.BW, sx, sy, 1);
                ExpandPixels(bitImage, pixImage);
                return pixImage;
            }
            else
            {
                var pixImage = Create(pixFormat, sx, sy, channels);
                fcBitmap.CopyPixels(pixImage.Array, pixImage.IntStride, 0);
                return pixImage;
            }
        }

        protected static PixImage CreateRawSystem(Stream stream, PixLoadOptions options)
        {
            var decoder = BitmapDecoder.Create(stream,
                                BitmapCreateOptions.None, BitmapCacheOption.Default);
            return CreateRaw((BitmapSource)decoder.Frames[0]);
        }

        /// <summary>
        /// Gets info about a PixImage without loading the entire image into
        /// memory.
        /// </summary>
        protected static PixImageInfo InfoFromFileNameSystem(
                string fileName, PixLoadOptions options)
        {
            try
            {
                var src0 = new BitmapImage();
                src0.BeginInit();
                src0.UriSource = new Uri(fileName, UriKind.RelativeOrAbsolute);
                src0.CacheOption = BitmapCacheOption.None;
                src0.CreateOptions = BitmapCreateOptions.DelayCreation;
                src0.EndInit();
                var pixFormat = s_pixFormatAndCountOfPixelFormat[src0.Format].E0;
                return new PixImageInfo(pixFormat, new V2i(src0.PixelWidth, src0.PixelHeight));
            }
            catch
            {
                return null;
            }
        }

        protected static PixImage CreateRawSystem(System.Drawing.Bitmap bitmap)
        {
            var sdipf = bitmap.PixelFormat;
            var pfc = s_pixFormatAndCountOfSdiPixelFormat[sdipf];

            var sx = bitmap.Width;
            var sy = bitmap.Height;
            var ch = pfc.E1;

            var pixImage = Create(pfc.E0, sx, sy, ch);
            var array = pixImage.Array;

            if (pfc.E0.Format == Col.Format.BW)
            {
                var bitImage = new PixImage<byte>(Col.Format.BW, 1 + (sx - 1) / 8, sy, 1);

                System.Drawing.Imaging.BitmapData bdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, sx, sy),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, sdipf);

                var bmp = BitmapSource.Create(
                    sx, sy,
                    (double)bitmap.VerticalResolution, (double)bitmap.HorizontalResolution,
                    s_pixelFormatOfSdiPixelFormat[sdipf],
                    null, bdata.Scan0, bitImage.Volume.Data.Length, bitImage.IntStride);
                bmp.CopyPixels(bitImage.Array, bitImage.IntStride, 0);
                bitmap.UnlockBits(bdata);
                ExpandPixels(bitImage, pixImage.ToPixImage<byte>());
            }
            else
            {
                System.Drawing.Imaging.BitmapData bdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, sx, sy),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, sdipf);
                var bmp = BitmapSource.Create(
                    sx, sy,
                    (double)bitmap.VerticalResolution, (double)bitmap.HorizontalResolution,
                    s_pixelFormatOfSdiPixelFormat[sdipf],
                    null, bdata.Scan0, ch * sx * sy, ch * sx);
                bmp.CopyPixels(pixImage.Array, pixImage.IntStride, 0);
                bitmap.UnlockBits(bdata);
            }
            return pixImage;
        }

        #endregion

        #region Conversions

        public abstract BitmapSource ToBitmapSource();

        protected BitmapFrame ToBitmapFrame()
        {
            return BitmapFrame.Create(ToBitmapSource());
        }

        protected MemoryStream ToMemoryStream(BitmapEncoder encoder)
        {
            encoder.Frames.Add(ToBitmapFrame());
            var stream = new MemoryStream();
            encoder.Save(stream);
            stream.Flush();
            return stream;
        }

        #endregion

        #region Save as Image

        protected bool SaveAsImageSystem(
                Stream stream, PixFileFormat fileFormat,
                PixSaveOptions options, int qualityLevel)
        {
            if (Format == Col.Format.BW) return false;
            BitmapEncoder encoder = null;
            switch (fileFormat)
            {
                case PixFileFormat.Jpeg:
                    encoder = qualityLevel > 0
                                ? new JpegBitmapEncoder { QualityLevel = qualityLevel }
                                : new JpegBitmapEncoder();
                    break;
                default:
                    encoder = GetEncoder(fileFormat);
                    if (encoder == null) return false;
                    break;
            }
            encoder.Frames.Add(ToBitmapFrame());
            encoder.Save(stream);
            stream.Flush();
            return true;
        }

        #endregion

    }

    public partial class PixImage<T>
    {
        #region Conversions

        public override BitmapSource ToBitmapSource()
        {
            return ToBitmapSource(96.0);
        }

        public BitmapSource ToBitmapSource(double dpi)
        {
            var storeFormats = GetStoreFormats(typeof(T), Format);
            var vol = Volume;

            if (Format == Col.Format.BW)
            {
                long sx = vol.SX, sy = vol.SY;

                var bitImage = new PixImage<byte>(Format, 1 + (Size.X - 1) / 8, Size.Y, 1);
                var bitData = bitImage.Volume.Data;
                var pixData = ToPixImage<byte>().Volume.Data;

                CompressPixels(ToPixImage<byte>(), bitImage);

                return BitmapSource.Create(
                    (int)sx, (int)sy, dpi, dpi,
                    storeFormats.E0, null, bitData, bitImage.IntStride);
            }
            else if (Format != storeFormats.E1)
            {
                var saveImage = new PixImage<T>(storeFormats.E1, this);
                vol = saveImage.Volume;
            }

            return BitmapSource.Create(
                (int)vol.SX, (int)vol.SY, 96, 96,
                storeFormats.E0, null, vol.Data, IntStride);
        }

        #endregion
    }

}
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SdiPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Aardvark.Base
{
    public static class PixImageWindowsMedia
    {
        [OnAardvarkInit]
        public static void Init()
        {
            PixImage.AddLoader(Loader);
        }

        #region PixelFormat extensions

        private static readonly Dictionary<PixelFormat, (PixFormat, int)> s_pixFormatAndCountOfPixelFormat =
            new Dictionary<PixelFormat, (PixFormat, int)>()
            {
                { PixelFormats.Indexed1, (PixFormat.ByteBGRA, 4) },
                { PixelFormats.Indexed2, (PixFormat.ByteBGRA, 4) },
                { PixelFormats.Indexed4, (PixFormat.ByteBGRA, 4) },
                { PixelFormats.Indexed8, (PixFormat.ByteBGRA, 4) },

                { PixelFormats.BlackWhite, (PixFormat.ByteBW, 1) },

                { PixelFormats.Gray8, (PixFormat.ByteGray, 1) },
                { PixelFormats.Bgr24, (PixFormat.ByteBGR, 3) },
                { PixelFormats.Bgr32, (PixFormat.ByteBGR, 4) },
                { PixelFormats.Bgra32, (PixFormat.ByteBGRA, 4) },
                { PixelFormats.Pbgra32, (PixFormat.ByteBGRP, 4) },

                { PixelFormats.Rgb24, (PixFormat.ByteRGB, 3) },

                { PixelFormats.Gray16, (PixFormat.UShortGray, 1) },
                { PixelFormats.Rgb48, (PixFormat.UShortRGB, 3) },
                { PixelFormats.Rgba64, (PixFormat.UShortRGBA, 4) },
                { PixelFormats.Prgba64, (PixFormat.UShortRGBP, 4) },

                { PixelFormats.Gray32Float, (PixFormat.FloatGray, 1) },
                { PixelFormats.Rgb128Float, (PixFormat.FloatRGB, 4) },
                { PixelFormats.Rgba128Float, (PixFormat.FloatRGBA, 4) },
                { PixelFormats.Prgba128Float, (PixFormat.FloatRGBP, 4) },
            };

        private static (PixFormat, int) ToPixFormatAndCount(this PixelFormat pixelFormat)
        {
            if (s_pixFormatAndCountOfPixelFormat.TryGetValue(pixelFormat, out var result))
                return result;

            throw new ArgumentException($"Unsupported pixel format {pixelFormat}");
        }

        private static PixelFormat ToUnindexed(this PixelFormat pixelFormat)
        {
            if (pixelFormat == PixelFormats.Indexed1 ||
                pixelFormat == PixelFormats.Indexed2 ||
                pixelFormat == PixelFormats.Indexed4 ||
                pixelFormat == PixelFormats.Indexed8)
            {
                return PixelFormats.Bgra32;
            }
            else
            {
                return pixelFormat;
            }
        }

        public static PixFormat ToPixFormat(this PixelFormat pixelFormat)
            => pixelFormat.ToPixFormatAndCount().Item1;

        #endregion

        #region System.Drawing.Bitmap extensions

        private static readonly Dictionary<SdiPixelFormat, PixelFormat> s_pixelFormatOfSdiPixelFormat =
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

        public static PixImage ToPixImageViaWindowsMedia(this System.Drawing.Bitmap bitmap)
        {
            if (!s_pixelFormatOfSdiPixelFormat.TryGetValue(bitmap.PixelFormat, out var pixelFormat))
                throw new ArgumentException($"Unsupported pixel format {bitmap.PixelFormat}");

            var (pixFormat, ch) = pixelFormat.ToPixFormatAndCount();
            var sx = bitmap.Width;
            var sy = bitmap.Height;

            var pixImage = PixImage.Create(pixFormat, sx, sy, ch);

            if (pixFormat.Format == Col.Format.BW)
            {
                var bitImage = new PixImage<byte>(Col.Format.BW, 1 + (sx - 1) / 8, sy, 1);

                var bdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, sx, sy),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

                var bmp = BitmapSource.Create(
                    sx, sy,
                    (double)bitmap.VerticalResolution, (double)bitmap.HorizontalResolution, pixelFormat,
                    null, bdata.Scan0, bitImage.Volume.Data.Length, bitImage.IntStride);
                bmp.CopyPixels(bitImage.Array, bitImage.IntStride, 0);
                bitmap.UnlockBits(bdata);
                PixImage.ExpandPixels(bitImage, pixImage.ToPixImage<byte>());
            }
            else
            {
                var bdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, sx, sy),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
                var bmp = BitmapSource.Create(
                    sx, sy,
                    (double)bitmap.VerticalResolution, (double)bitmap.HorizontalResolution,
                    pixelFormat,
                    null, bdata.Scan0, ch * sx * sy, ch * sx);
                bmp.CopyPixels(pixImage.Array, pixImage.IntStride, 0);
                bitmap.UnlockBits(bdata);
            }

            return pixImage;
        }

        #endregion

        #region Static Tables and Methods

        private static readonly Dictionary<PixFileFormat, Func<BitmapEncoder>> s_FormatEncoder =
            new Dictionary<PixFileFormat, Func<BitmapEncoder>>()
        {
            { PixFileFormat.Png, () => new PngBitmapEncoder() },
            { PixFileFormat.Bmp, () => new BmpBitmapEncoder() },
            { PixFileFormat.Jpeg, () => new JpegBitmapEncoder() },
            { PixFileFormat.Tiff, () => new TiffBitmapEncoder() },
            { PixFileFormat.Gif, () => new GifBitmapEncoder() },
            { PixFileFormat.Wmp, () => new WmpBitmapEncoder() }
        };

        private static BitmapEncoder GetEncoder(PixFileFormat format)
        {
            if (s_FormatEncoder.TryGetValue(format, out Func<BitmapEncoder> encCreator))
                return encCreator();

            throw new ArgumentException($"Unsupported image file format {format}");
        }

        private static readonly Dictionary<PixFormat, (PixelFormat, Col.Format)> s_pixelFormatOfFormat =
            new Dictionary<PixFormat, (PixelFormat, Col.Format)>()
            {
                { new PixFormat(typeof(byte), Col.Format.BW), (PixelFormats.BlackWhite, Col.Format.BW) },
                { new PixFormat(typeof(byte), Col.Format.Gray), (PixelFormats.Gray8, Col.Format.Gray) },
                { new PixFormat(typeof(byte), Col.Format.RGB), (PixelFormats.Bgr24, Col.Format.BGR) },
                { new PixFormat(typeof(byte), Col.Format.BGR), (PixelFormats.Bgr24, Col.Format.BGR) },
                { new PixFormat(typeof(byte), Col.Format.RGBA), (PixelFormats.Bgra32, Col.Format.BGRA) },
                { new PixFormat(typeof(byte), Col.Format.BGRA), (PixelFormats.Bgra32, Col.Format.BGRA) },
                { new PixFormat(typeof(byte), Col.Format.RGBP), (PixelFormats.Pbgra32, Col.Format.BGRP) },
                { new PixFormat(typeof(byte), Col.Format.BGRP), (PixelFormats.Pbgra32, Col.Format.BGRP) },

                { new PixFormat(typeof(ushort), Col.Format.Gray), (PixelFormats.Gray16, Col.Format.Gray) },
                { new PixFormat(typeof(ushort), Col.Format.RGB), (PixelFormats.Rgb48, Col.Format.RGB) },
                { new PixFormat(typeof(ushort), Col.Format.BGR), (PixelFormats.Rgb48, Col.Format.RGB) },
                { new PixFormat(typeof(ushort), Col.Format.RGBA), (PixelFormats.Rgba64, Col.Format.RGBA) },
                { new PixFormat(typeof(ushort), Col.Format.BGRA), (PixelFormats.Rgba64, Col.Format.RGBA) },
                { new PixFormat(typeof(ushort), Col.Format.RGBP), (PixelFormats.Prgba64, Col.Format.RGBP) },
                { new PixFormat(typeof(ushort), Col.Format.BGRP), (PixelFormats.Prgba64, Col.Format.RGBP) },

                { new PixFormat(typeof(float), Col.Format.Gray), (PixelFormats.Gray32Float, Col.Format.Gray) },
                { new PixFormat(typeof(float), Col.Format.RGB), (PixelFormats.Rgb128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.BGR), (PixelFormats.Rgb128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.RGBA), (PixelFormats.Rgba128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.BGRA), (PixelFormats.Rgba128Float, Col.Format.RGBA) },
                { new PixFormat(typeof(float), Col.Format.RGBP), (PixelFormats.Prgba128Float, Col.Format.RGBP) },
                { new PixFormat(typeof(float), Col.Format.BGRP), (PixelFormats.Prgba128Float, Col.Format.RGBP) },
            };

        private static (PixelFormat, Col.Format) GetStoreFormats(Type type, Col.Format format)
        {
            if (s_pixelFormatOfFormat.TryGetValue(new PixFormat(type, format), out (PixelFormat, Col.Format) formatPair))
                return formatPair;

            throw new ArgumentException($"Unsupported pixel format ({format}, {type})");
        }

        private static PixImage CreateFromBitmapSource(BitmapSource bitmapSource)
        {
            var pixelFormat = bitmapSource.Format.ToUnindexed();
            var fcBitmap = new FormatConvertedBitmap(bitmapSource, pixelFormat, null, 0);
            var (pixFormat, channels) = pixelFormat.ToPixFormatAndCount();
            return CreateFromFormatConvertedBitmap(fcBitmap, pixFormat, channels);
        }

        private static PixImage CreateFromFormatConvertedBitmap(FormatConvertedBitmap fcBitmap, PixFormat pixFormat, int channels)
        {
            var sx = fcBitmap.PixelWidth;
            var sy = fcBitmap.PixelHeight;
            if (pixFormat.Format == Col.Format.BW)
            {
                var bitImage = new PixImage<byte>(Col.Format.BW, 1 + (sx - 1) / 8, sy, channels);
                fcBitmap.CopyPixels(bitImage.Array, bitImage.IntStride, 0);
                var pixImage = new PixImage<byte>(Col.Format.BW, sx, sy, 1);
                PixImage.ExpandPixels(bitImage, pixImage);
                return pixImage;
            }
            else
            {
                var pixImage = PixImage.Create(pixFormat, sx, sy, channels);
                fcBitmap.CopyPixels(pixImage.Array, pixImage.IntStride, 0);
                return pixImage;
            }
        }

        #endregion

        #region Loader

        private class PixLoader : IPixLoader
        {
            public string Name => "Windows Media";

            #region Load from File / Stream

            /// <summary>
            /// Creates a new image from the file. Note that in this raw image the channel
            /// count of the volume and the channel count of the format may be different.
            /// </summary>
            public PixImage LoadFromFile(string filename)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.None
                                     | BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.EndInit();
                return CreateFromBitmapSource(bitmapImage);
            }

            public PixImage LoadFromStream(Stream stream)
            {
                var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                return CreateFromBitmapSource(decoder.Frames[0]);
            }

            #endregion

            #region Save to File / Stream

            public void SaveToFile (string filename, PixImage pi, PixSaveParams saveParams)
            {
                using var stream = File.OpenWrite(filename);
                SaveToStream(stream, pi, saveParams);
            }

            public void SaveToStream(Stream stream, PixImage pi, PixSaveParams saveParams)
            {
                if (pi.Format == Col.Format.BW)
                    throw new ArgumentException($"Unsupported color format {pi.Format}");

                BitmapEncoder encoder =
                    saveParams switch
                    {
                        PixJpegSaveParams jpeg => new JpegBitmapEncoder { QualityLevel = jpeg.Quality },
                        _ => GetEncoder(saveParams.Format)
                    };

                encoder.Frames.Add(pi.ToBitmapFrame());
                encoder.Save(stream);
                stream.Flush();
            }

            #endregion

            #region Get info from File / Stream

            public PixImageInfo GetInfoFromFile(string fileName)
            {
                using var stream = File.OpenRead(fileName);
                return GetInfoFromStream(stream);
            }

            public PixImageInfo GetInfoFromStream(Stream stream)
            {
                var src0 = new BitmapImage();
                src0.BeginInit();
                src0.StreamSource = stream;
                src0.CacheOption = BitmapCacheOption.None;
                src0.CreateOptions = BitmapCreateOptions.DelayCreation;
                src0.EndInit();
                var pixFormat = src0.Format.ToPixFormat();
                return new PixImageInfo(pixFormat, new V2i(src0.PixelWidth, src0.PixelHeight));
            }

            #endregion
        }

        public static readonly IPixLoader Loader = new PixLoader();

        #endregion

        #region Conversions

        private static BitmapFrame ToBitmapFrame(this PixImage pi)
        {
            return BitmapFrame.Create(pi.ToBitmapSource());
        }

        public static BitmapSource ToBitmapSource(this PixImage pi, double dpi = 96.0)
        {
            var t = pi.PixFormat.Type;
            var mi = typeof(PixImageWindowsMedia).GetMethod("ToBitmapSourceTyped", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).MakeGenericMethod(new[] { t });
            return (BitmapSource)mi.Invoke(null, new object[] { pi, dpi });
        }

        public static BitmapSource ToBitmapSource<T>(this PixImage<T> pi, double dpi = 96.0)
        {
            return ToBitmapSourceTyped(pi, dpi);
        }

        private static BitmapSource ToBitmapSourceTyped<T>(this PixImage<T> pi, double dpi)
        {
            var storeFormats = GetStoreFormats(typeof(T), pi.Format);
            var vol = pi.Volume;

            if (pi.Format == Col.Format.BW)
            {
                long sx = vol.SX, sy = vol.SY;

                var bitImage = new PixImage<byte>(pi.Format, 1 + (pi.Size.X - 1) / 8, pi.Size.Y, 1);
                var bitData = bitImage.Volume.Data;

                PixImage.CompressPixels(pi.ToPixImage<byte>(), bitImage);

                return BitmapSource.Create(
                    (int)sx, (int)sy, dpi, dpi,
                    storeFormats.Item1, null, bitData, bitImage.IntStride);
            }
            else if (pi.Format != storeFormats.Item2)
            {
                var saveImage = new PixImage<T>(storeFormats.Item2, pi);
                vol = saveImage.Volume;
            }

            return BitmapSource.Create(
                (int)vol.SX, (int)vol.SY, 96, 96,
                storeFormats.Item1, null, vol.Data, pi.IntStride);
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Contains Windows-only extensions for converting to and from bitmaps, as well as saving images to files and streams.
    /// </summary>
    public static class PixImageSystemDrawing
    {
        #region Private

        private static readonly Dictionary<PixelFormat, (PixFormat, int)> s_pixFormatAndCountOfPixelFormatBitmap =
            new Dictionary<PixelFormat, (PixFormat, int)>()
        {
            { PixelFormat.Format1bppIndexed, (PixFormat.ByteBW, 1) },
            { PixelFormat.Format8bppIndexed, (PixFormat.ByteBW, 1) },

            { PixelFormat.Format16bppGrayScale, (PixFormat.UShortGray, 1) },

            { PixelFormat.Format24bppRgb, (PixFormat.ByteBGR, 3) },
            { PixelFormat.Format32bppRgb, (PixFormat.ByteBGR, 4) },
            { PixelFormat.Format32bppArgb, (PixFormat.ByteBGRA, 4) },
            { PixelFormat.Format32bppPArgb, (PixFormat.ByteBGRP, 4) },

            { PixelFormat.Format48bppRgb, (PixFormat.UShortBGR, 3) },
            { PixelFormat.Format64bppArgb, (PixFormat.UShortBGRA, 4) },
            { PixelFormat.Format64bppPArgb, (PixFormat.UShortBGRP, 4) },

        };

        private static readonly Dictionary<PixelFormat, PixelFormat> s_bitmapLockFormats = new Dictionary<PixelFormat, PixelFormat>()
        {
            { PixelFormat.Format16bppArgb1555, PixelFormat.Format32bppArgb },
            { PixelFormat.Format16bppRgb555, PixelFormat.Format24bppRgb },
            { PixelFormat.Format16bppRgb565, PixelFormat.Format24bppRgb },
            { PixelFormat.Format4bppIndexed, PixelFormat.Format8bppIndexed }
        };

        private static readonly Dictionary<PixFormat, PixelFormat> s_pixelFormats =
            new Dictionary<PixFormat, PixelFormat>()
        {
            { PixFormat.ByteRGB, PixelFormat.Format24bppRgb },
            { PixFormat.ByteBGR, PixelFormat.Format24bppRgb },
            { PixFormat.ByteBGRA, PixelFormat.Format32bppArgb },
            { PixFormat.ByteBW, PixelFormat.Format8bppIndexed },
            { PixFormat.ByteRGBP, PixelFormat.Format32bppPArgb },
            { PixFormat.UShortGray, PixelFormat.Format16bppGrayScale },
            { PixFormat.UShortBGR, PixelFormat.Format48bppRgb },
            { PixFormat.UShortBGRA, PixelFormat.Format64bppArgb },
            { PixFormat.UShortBGRP, PixelFormat.Format64bppPArgb },
        };

        private static readonly Dictionary<PixFileFormat, ImageFormat> s_imageFormats =
            new Dictionary<PixFileFormat, ImageFormat>()
        {
            {PixFileFormat.Bmp, ImageFormat.Bmp},
            {PixFileFormat.Gif, ImageFormat.Gif},
            {PixFileFormat.Jpeg, ImageFormat.Jpeg},
            {PixFileFormat.Png, ImageFormat.Png},
            {PixFileFormat.Tiff, ImageFormat.Tiff},
            {PixFileFormat.Wmp, ImageFormat.Wmf},
        };

        private static readonly Dictionary<Guid, ImageCodecInfo> s_imageCodecInfos =
            ImageCodecInfo.GetImageEncoders().ToDictionary(c => c.FormatID);

        private static PixelFormat GetLockFormat(PixelFormat format)
            => s_pixFormatAndCountOfPixelFormatBitmap.ContainsKey(format) ? format : s_bitmapLockFormats[format];

        private static PixImage CreateRawBitmap(Bitmap bitmap)
        {
            var sdipf = GetLockFormat(bitmap.PixelFormat);
            var pfc = s_pixFormatAndCountOfPixelFormatBitmap[sdipf];

            var sx = bitmap.Width;
            var sy = bitmap.Height;
            var ch = pfc.Item2;

            var pixImage = PixImage.Create(pfc.Item1, sx, sy, ch);
            var array = pixImage.Array;

            if (pfc.Item1.Format == Col.Format.BW)
            {
                var bitImage = new PixImage<byte>(Col.Format.BW, 1 + (sx - 1) / 8, sy, 1);

                var bdata = bitmap.LockBits(
                    new Rectangle(0, 0, sx, sy),
                    ImageLockMode.ReadOnly, sdipf);

                bdata.Scan0.CopyTo(bitImage.Volume.Data);

                bitmap.UnlockBits(bdata);
                PixImage.ExpandPixels(bitImage, pixImage.ToPixImage<byte>());
            }
            else
            {
                var bdata = bitmap.LockBits(
                    new Rectangle(0, 0, sx, sy),
                    ImageLockMode.ReadOnly, sdipf);

                bdata.Scan0.CopyTo(array);
                bitmap.UnlockBits(bdata);
            }
            return pixImage;
        }

        /// <summary>
        /// Creates PixImage from System.Drawing.Bitmap.
        /// </summary>
        private static PixImage Create(Bitmap bitmap)
        {
            var loadImage = CreateRawBitmap(bitmap);
            return loadImage.ToPixImage(loadImage.Format);
        }

        /// <summary>
        /// Creates PixImage from System.Drawing.Image.
        /// </summary>
        private static PixImage Create(Image image) => Create((Bitmap)image);

        #endregion

        /// <summary>
        /// Creates PixImage from System.Drawing.Bitmap.
        /// </summary>
        public static PixImage ToPixImage(this Bitmap bitmap) => Create(bitmap);

        /// <summary>
        /// Creates PixImage from System.Drawing.Image.
        /// </summary>
        public static PixImage ToPixImage(this Image image) => Create(image);

        /// <summary>
        /// Converts image to System.Drawing.Image.
        /// </summary>
        public static Bitmap ToSystemDrawingBitmap(this PixImage self)
            => new Bitmap(self.ToMemoryStream(PixFileFormat.Png), false);

        /// <summary>
        /// Saves image to stream via System.Drawing.
        /// </summary>
        /// <param name="self">The image to save.</param>
        /// <param name="stream">The stream to save the image to.</param>
        /// <param name="saveParams">The save parameters to use.</param>
        /// <returns>true on success, false otherwise.</returns>
        public static bool SaveViaSystemDrawing(this PixImage self, Stream stream, PixSaveParams saveParams)
        {
            try
            {
                self = self.ToCanonicalDenseLayout();
                if (!s_pixelFormats.TryGetValue(self.PixFormat, out PixelFormat pixelFormat))
                    Report.Error($"Unknown PixelFormat {self.PixFormat.Format}.");

                var imageFormat = s_imageFormats[saveParams.Format];

                var size = self.Size;
                using (var bmp = new Bitmap(size.X, size.Y, pixelFormat))
                {
                    var bdata = bmp.LockBits(new Rectangle(0, 0, size.X, size.Y), ImageLockMode.ReadOnly, pixelFormat);
                    self.Data.CopyTo(bdata.Scan0);
                    bmp.UnlockBits(bdata);

                    int quality =
                        saveParams switch
                        {
                            PixJpegSaveParams jpeg => jpeg.Quality,
                            _ => -1
                        };

                    if (quality >= 0)
                    {
                        var codec = s_imageCodecInfos[imageFormat.Guid];
                        var parameters = new EncoderParameters(1);
                        parameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                        bmp.Save(stream, codec, parameters);
                        parameters.Dispose();
                    }
                    else
                    {
                        bmp.Save(stream, imageFormat);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Saves image to stream via System.Drawing.
        /// </summary>
        /// <param name="self">The image to save.</param>
        /// <param name="stream">The stream to save the image to.</param>
        /// <param name="fileFormat">The image format of the stream.</param>
        /// <returns>true on success, false otherwise.</returns>
        public static bool SaveViaSystemDrawing(this PixImage self, Stream stream, PixFileFormat fileFormat)
            => self.SaveViaSystemDrawing(stream, new PixSaveParams(fileFormat));

        /// <summary>
        ///  Saves image to stream via System.Drawing as JPEG.
        /// </summary>
        /// <param name="self">The image to save.</param>
        /// <param name="stream">The stream to save the image to.</param>
        /// <param name="quality">The quality of the JPEG file. Must be within 0 - 100.</param>
        /// <returns>true on success, false otherwise.</returns>
        public static bool SaveViaSystemDrawingAsJpeg(this PixImage self, Stream stream, int quality = PixJpegSaveParams.DefaultQuality)
            => self.SaveViaSystemDrawing(stream, new PixJpegSaveParams(quality));

        /// <summary>
        /// Saves image to file via System.Drawing.
        /// </summary>
        /// <param name="self">The image to save.</param>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="saveParams">The save parameters to use.</param>
        /// <returns>true on success, false otherwise.</returns>
        public static bool SaveViaSystemDrawing(this PixImage self, string filename, PixSaveParams saveParams)
        {
            using var stream = File.OpenWrite(filename);
            return SaveViaSystemDrawing(self, stream, saveParams);
        }

        /// <summary>
        /// Saves image to file via System.Drawing.
        /// </summary>
        /// <param name="self">The image to save.</param>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="fileFormat">The image format of the stream.</param>
        /// <returns>true on success, false otherwise.</returns>
        public static bool SaveViaSystemDrawing(this PixImage self, string filename, PixFileFormat fileFormat)
            => self.SaveViaSystemDrawing(filename, new PixSaveParams(fileFormat));

        /// <summary>
        ///  Saves image to file via System.Drawing as JPEG.
        /// </summary>
        /// <param name="self">The image to save.</param>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="quality">The quality of the JPEG file. Must be within 0 - 100.</param>
        /// <returns>true on success, false otherwise.</returns>
        public static bool SaveViaSystemDrawingAsJpeg(this PixImage self, string filename, int quality = PixJpegSaveParams.DefaultQuality)
            => self.SaveViaSystemDrawing(filename, new PixJpegSaveParams(quality));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    public abstract partial class PixImage
    {
		protected static Dictionary<PixelFormat, Tup<PixFormat, int>> s_pixFormatAndCountOfPixelFormat =
			new Dictionary<PixelFormat, Tup<PixFormat, int>>()
		{
			{ PixelFormat.Format1bppIndexed, Tup.Create(PixFormat.ByteBW, 1) },

			{ PixelFormat.Format16bppGrayScale, Tup.Create(PixFormat.UShortGray, 1) },

			{ PixelFormat.Format24bppRgb, Tup.Create(PixFormat.ByteBGR, 3) },
			{ PixelFormat.Format32bppRgb, Tup.Create(PixFormat.ByteBGR, 4) },
			{ PixelFormat.Format32bppArgb, Tup.Create(PixFormat.ByteBGRA, 4) },
			{ PixelFormat.Format32bppPArgb, Tup.Create(PixFormat.ByteBGRP, 4) },

			{ PixelFormat.Format48bppRgb, Tup.Create(PixFormat.UShortBGR, 3) },
			{ PixelFormat.Format64bppArgb, Tup.Create(PixFormat.UShortBGRA, 4) },
			{ PixelFormat.Format64bppPArgb, Tup.Create(PixFormat.UShortBGRP, 4) },
		};

        public static Dictionary<PixelFormat, PixFormat> s_formats = new Dictionary<PixelFormat, PixFormat>()
        {
            { PixelFormat.Format24bppRgb, PixFormat.ByteBGR },
            { PixelFormat.Format32bppArgb, PixFormat.ByteBGRA },
        };

        protected static PixImage CreateRawBitmap(System.Drawing.Bitmap bitmap)
        {
            var sdipf = bitmap.PixelFormat;
			var pfc = s_pixFormatAndCountOfPixelFormat[sdipf];

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


                var ptr = bdata.Scan0;
                Marshal.Copy(ptr, bitImage.Volume.Data, 0, bitImage.Volume.Data.Length);

                bitmap.UnlockBits(bdata);
                ExpandPixels(bitImage, pixImage.ToPixImage<byte>());
            }
            else
            {
                System.Drawing.Imaging.BitmapData bdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, sx, sy),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, sdipf);

                var ptr = bdata.Scan0;
                pixImage.Data.UnsafeCoercedApply((byte[] arr) =>
                {
                    Marshal.Copy(ptr, arr, 0, arr.Length);
                });


                bitmap.UnlockBits(bdata);
            }
            return pixImage;
        }



        /// <summary>
        /// Load image from stream via devil.
        /// </summary>
        /// <returns>If file could not be read, returns null, otherwise a Piximage.</returns>
        private static PixImage CreateRawBitmap(
                Stream stream,
                PixLoadOptions loadFlags = PixLoadOptions.Default)
        {
            var bmp = (Bitmap)Bitmap.FromStream(stream);

            var result = CreateRawBitmap(bmp);
            bmp.Dispose();

            return result;
        }

        /// <summary>
        /// Save image to stream via devil.
        /// </summary>
        /// <returns>True if the file was successfully saved.</returns>
        private bool SaveAsImageBitmap(
                Stream stream, PixFileFormat format,
                PixSaveOptions options, int qualityLevel)
        {
            return false;
        }

        /// <summary>
        /// Gets info about a PixImage without loading the entire image into memory.
        /// </summary>
        /// <returns>null if the file info could not be loaded.</returns>
        public static PixImageInfo InfoFromFileNameBitmap(
                string fileName, PixLoadOptions options)
        {
            // TODO: implement
            return null;
        }
    }
}

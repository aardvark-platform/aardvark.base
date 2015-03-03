#if USE_FREEIMAGE
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


using FreeImageAPI;

namespace Aardvark.Base
{
    public abstract partial class PixImage
    {

        #region Static Tables and Methods

        private static Dictionary<PixFileFormat, FREE_IMAGE_FORMAT> s_fifImageFormatMap =
            new Dictionary<PixFileFormat, FREE_IMAGE_FORMAT>
        {
            { PixFileFormat.Unknown,    FREE_IMAGE_FORMAT.FIF_UNKNOWN   },
            { PixFileFormat.Bmp,        FREE_IMAGE_FORMAT.FIF_BMP       },
            { PixFileFormat.Ico,        FREE_IMAGE_FORMAT.FIF_ICO       },
            { PixFileFormat.Jpeg,       FREE_IMAGE_FORMAT.FIF_JPEG      },
            { PixFileFormat.Jng,        FREE_IMAGE_FORMAT.FIF_JNG       },
            { PixFileFormat.Koala,      FREE_IMAGE_FORMAT.FIF_KOALA     },
            { PixFileFormat.Lbm,        FREE_IMAGE_FORMAT.FIF_LBM       },
            { PixFileFormat.Iff,        FREE_IMAGE_FORMAT.FIF_IFF       },
            { PixFileFormat.Mng,        FREE_IMAGE_FORMAT.FIF_MNG       },
            { PixFileFormat.Pbm,        FREE_IMAGE_FORMAT.FIF_PBM       },
            { PixFileFormat.PbmRaw,     FREE_IMAGE_FORMAT.FIF_PBMRAW    },
            { PixFileFormat.Pcd,        FREE_IMAGE_FORMAT.FIF_PCD       },
            { PixFileFormat.Pcx,        FREE_IMAGE_FORMAT.FIF_PCX       },
            { PixFileFormat.Pgm,        FREE_IMAGE_FORMAT.FIF_PGM       },
            { PixFileFormat.PgmRaw,     FREE_IMAGE_FORMAT.FIF_PGMRAW    },
            { PixFileFormat.Png,        FREE_IMAGE_FORMAT.FIF_PNG       },
            { PixFileFormat.Ppm,        FREE_IMAGE_FORMAT.FIF_PPM       },
            { PixFileFormat.PpmRaw,     FREE_IMAGE_FORMAT.FIF_PPMRAW    },
            { PixFileFormat.Ras,        FREE_IMAGE_FORMAT.FIF_RAS       },
            { PixFileFormat.Targa,      FREE_IMAGE_FORMAT.FIF_TARGA     },
            { PixFileFormat.Tiff,       FREE_IMAGE_FORMAT.FIF_TIFF      },
            { PixFileFormat.Wbmp,       FREE_IMAGE_FORMAT.FIF_WBMP      },
            { PixFileFormat.Psd,        FREE_IMAGE_FORMAT.FIF_PSD       },
            { PixFileFormat.Cut,        FREE_IMAGE_FORMAT.FIF_CUT       },
            { PixFileFormat.Xbm,        FREE_IMAGE_FORMAT.FIF_XBM       },
            { PixFileFormat.Xpm,        FREE_IMAGE_FORMAT.FIF_XPM       },
            { PixFileFormat.Dds,        FREE_IMAGE_FORMAT.FIF_DDS       },
            { PixFileFormat.Gif,        FREE_IMAGE_FORMAT.FIF_GIF       },
            { PixFileFormat.Hdr,        FREE_IMAGE_FORMAT.FIF_HDR       },
            { PixFileFormat.Faxg3,      FREE_IMAGE_FORMAT.FIF_FAXG3     },
            { PixFileFormat.Sgi,        FREE_IMAGE_FORMAT.FIF_SGI       },
            { PixFileFormat.Exr,        FREE_IMAGE_FORMAT.FIF_EXR       },
            { PixFileFormat.J2k,        FREE_IMAGE_FORMAT.FIF_J2K       },
            { PixFileFormat.Jp2,        FREE_IMAGE_FORMAT.FIF_JP2       },
            { PixFileFormat.Pfm,        FREE_IMAGE_FORMAT.FIF_PFM       },
            { PixFileFormat.Pict,       FREE_IMAGE_FORMAT.FIF_PICT      },
            { PixFileFormat.Raw,        FREE_IMAGE_FORMAT.FIF_RAW       },
        };

        #endregion

        private static PixImage CreateRawFreeImage(
                Stream stream,
                PixLoadOptions loadFlags = PixLoadOptions.Default)
        {
            var dib = FreeImage.LoadFromStream(stream);
            var pi = CreateFromFiBitMap(dib);
            if (pi == null) return null;
            FreeImage.Unload(dib);
            return pi;
        }

        private static PixImage CreateFromFiBitMap(FIBITMAP dib)
        {
            var sx = (int)FreeImage.GetWidth(dib);
            var sy = (int)FreeImage.GetHeight(dib);
            var delta = (int)FreeImage.GetPitch(dib);
            var ftype = FreeImage.GetImageType(dib);
            var bpp = FreeImage.GetBPP(dib);

            var bits = FreeImage.GetBits(dib) + sy * delta;

            switch (ftype)
            {
                case FREE_IMAGE_TYPE.FIT_BITMAP:
                    switch (bpp)
                    {
                        case 1:
                            {
                                var palette = FreeImage.GetPaletteEx(dib);
                                var pi = new PixImage<byte>(Col.Format.BW, sx, sy, 1);
                                var data = pi.Volume.Data;
                                int i = 0;
                                if (palette != null &&
                                    palette[0].rgbRed + palette[0].rgbGreen + palette[0].rgbBlue >= 384)
                                {
                                    for (var y = 0; y < sy; y++)
                                    {
                                        bits = bits - delta;
                                        byte bit = 0x80; int bi = 0;
                                        unsafe
                                        {
                                            byte* pixel = (byte*)bits;
                                            for (var x = 0; x < sx; x++)
                                            {
                                                data[i++] = ((pixel[bi] & bit) == 0) ? (byte)255 : (byte)0;
                                                bit >>= 1; if (bit == 0) { bit = 0x80; bi++; }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (var y = 0; y < sy; y++)
                                    {
                                        bits = bits - delta;
                                        byte bit = 0x80; int bi = 0;
                                        unsafe
                                        {
                                            byte* pixel = (byte*)bits;
                                            for (var x = 0; x < sx; x++)
                                            {
                                                data[i++] = ((pixel[bi] & bit) != 0) ? (byte)255 : (byte)0;
                                                bit >>= 1; if (bit == 0) { bit = 0x80; bi++; }
                                            }
                                        }
                                    }
                                }
                                return pi;
                            }
                        case 8:
                            {
                                var pi = new PixImage<byte>(sx, sy, 1);
                                var data = pi.Volume.Data;
                                long i = 0;
                                for (var y = 0; y < sy; y++)
                                {
                                    bits = bits - delta;
                                    unsafe
                                    {
                                        Byte* pixel = (Byte*)bits;
                                        for (var x = 0; x < sx; x++)
                                            data[i++] = pixel[x];
                                    }
                                }
                                return pi;
                            }
                        case 24:
                            {
                                var pi = new PixImage<byte>(sx, sy, 3);
                                var data = pi.Volume.Data;
                                long i = 0;
                                for (var y = 0; y < sy; y++)
                                {
                                    bits = bits - delta;
                                    unsafe
                                    {
                                        Byte* pixel = (Byte*)bits;
                                        for (var x = 0; x < sx; x++)
                                        {
                                            data[i++] = pixel[FreeImage.FI_RGBA_BLUE];
                                            data[i++] = pixel[FreeImage.FI_RGBA_GREEN];
                                            data[i++] = pixel[FreeImage.FI_RGBA_RED];
                                            pixel += 3;
                                        }
                                    }
                                }
                                return pi;
                            }
                        case 32:
                            {
                                var pi = new PixImage<byte>(sx, sy, 4);
                                var data = pi.Volume.Data;
                                long i = 0;
                                for (var y = 0; y < sy; y++)
                                {
                                    bits = bits - delta;
                                    unsafe
                                    {
                                        Byte* pixel = (Byte*)bits;
                                        for (var x = 0; x < sx; x++)
                                        {
                                            data[i++] = pixel[FreeImage.FI_RGBA_BLUE];
                                            data[i++] = pixel[FreeImage.FI_RGBA_GREEN];
                                            data[i++] = pixel[FreeImage.FI_RGBA_RED];
                                            data[i++] = pixel[FreeImage.FI_RGBA_ALPHA];
                                            pixel += 4;
                                        }
                                    }
                                }
                                return pi;
                            }
                    }
                    break;
                case FREE_IMAGE_TYPE.FIT_UINT16:
                    {
                        var pi = new PixImage<ushort>(sx, sy, 1);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                ushort* pixel = (ushort*)bits;
                                for (var x = 0; x < sx; x++)
                                    data[i++] = pixel[x];
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_INT16:
                    {
                        var pi = new PixImage<short>(sx, sy, 1);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                short* pixel = (short*)bits;
                                for (var x = 0; x < sx; x++)
                                    data[i++] = pixel[x];
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_UINT32:
                    {
                        var pi = new PixImage<uint>(sx, sy, 1);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                uint* pixel = (uint*)bits;
                                for (var x = 0; x < sx; x++)
                                    data[i++] = pixel[x];
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_INT32:
                    {
                        var pi = new PixImage<int>(sx, sy, 1);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                int* pixel = (int*)bits;
                                for (var x = 0; x < sx; x++)
                                    data[i++] = pixel[x];
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_FLOAT:
                    {
                        var pi = new PixImage<float>(sx, sy, 1);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                float* pixel = (float*)bits;
                                for (var x = 0; x < sx; x++)
                                    data[i++] = pixel[x];
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_DOUBLE:
                    {
                        var pi = new PixImage<double>(sx, sy, 1);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                double* pixel = (double*)bits;
                                for (var x = 0; x < sx; x++)
                                    data[i++] = pixel[x];
                            }
                        }
                    }
                    break;
                case FREE_IMAGE_TYPE.FIT_COMPLEX:
                    {
                        var pi = new PixImage<double>(sx, sy, 2);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FICOMPLEX* pixel = (FICOMPLEX*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    data[i++] = pixel[x].real;
                                    data[i++] = pixel[x].imag;
                                }
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_RGB16:
                    {
                        var pi = new PixImage<ushort>(sx, sy, 3);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGB16* pixel = (FIRGB16*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    data[i++] = pixel[x].red;
                                    data[i++] = pixel[x].green;
                                    data[i++] = pixel[x].blue;
                                }
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_RGBF:
                    {
                        var pi = new PixImage<float>(sx, sy, 3);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGBF* pixel = (FIRGBF*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    data[i++] = pixel[x].red;
                                    data[i++] = pixel[x].green;
                                    data[i++] = pixel[x].blue;
                                }
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_RGBA16:
                    {
                        var pi = new PixImage<ushort>(sx, sy, 4);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGBA16* pixel = (FIRGBA16*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    data[i++] = pixel[x].red;
                                    data[i++] = pixel[x].green;
                                    data[i++] = pixel[x].blue;
                                    data[i++] = pixel[x].alpha;
                                }
                            }
                        }
                        return pi;
                    }
                case FREE_IMAGE_TYPE.FIT_RGBAF:
                    {
                        var pi = new PixImage<float>(sx, sy, 4);
                        var data = pi.Volume.Data;
                        long i = 0;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGBAF* pixel = (FIRGBAF*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    data[i++] = pixel[x].red;
                                    data[i++] = pixel[x].green;
                                    data[i++] = pixel[x].blue;
                                    data[i++] = pixel[x].alpha;
                                }
                            }
                        }
                        return pi;
                    }
            }
            return null;
        }

        private bool SaveAsImageFreeImage(
                Stream stream, PixFileFormat format,
                PixSaveOptions options, int qualityLevel)
        {
            var dib = ToFiBitMap();

            var flags = FREE_IMAGE_SAVE_FLAGS.DEFAULT;

            switch (format)
            {
                case PixFileFormat.Jpeg: if (qualityLevel > 0) flags = (FREE_IMAGE_SAVE_FLAGS)qualityLevel; break;
                case PixFileFormat.Exr:  if (PixFormat.Type == typeof(float)) flags |= FREE_IMAGE_SAVE_FLAGS.EXR_FLOAT; break;
                default: break;
            }

            var result = FreeImage.SaveToStream(dib, stream, s_fifImageFormatMap[format], flags);
            FreeImage.UnloadEx(ref dib);
            return result;
        }

        private static void FreeImageCheck(VolumeInfo vi)
        {
            Requires.That(vi.DZ == 1L && vi.SZ == vi.DX);
        }

        private static FIBITMAP NewFiBitMap(PixImage<byte> pi)
        {
            FreeImageCheck(pi.Volume.Info);
            var sx = pi.Size.X;
            var sy = pi.Size.Y;
            var bpp = pi.ChannelCount == 1 && pi.Format == Col.Format.BW ? 1 : pi.ChannelCount * 8;
            var dib = FreeImage.Allocate(sx, sy, bpp);
            var delta = (int)FreeImage.GetPitch(dib);
            var data = pi.Volume.Data;
            long i = pi.Volume.FirstIndex;
            long j = pi.Volume.JY;
            var bits = FreeImage.GetBits(dib) + sy * delta;

            switch (bpp)
            {
                case 1:
                    {
                        var palette = FreeImage.GetPaletteEx(dib);
                        if (palette != null) // should alway be != null
                        {
                            palette[0] = new RGBQUAD { rgbRed = 0, rgbGreen = 0, rgbBlue = 0 };
                            palette[1] = new RGBQUAD { rgbRed = 255, rgbGreen = 255, rgbBlue = 255 };
                        }
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            byte bit = 0x80;
                            int bi = 0;
                            unsafe
                            {
                                byte* pixel = (byte*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    if ((data[i++] & 0x80) != 0) pixel[bi] |= bit;
                                    bit >>= 1; if (bit == 0) { bit = 0x80; bi++; }
                                }
                            }
                            i += j;
                        }
                    }
                    break;
                case 8:
                    {
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                byte* pixel = (byte*)bits;
                                for (var x = 0; x < sx; x++)
                                    pixel[x] = data[i++];
                            }
                            i += j;
                        }
                    }
                    break;
                case 24:
                    {
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                Byte* pixel = (Byte*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    pixel[FreeImage.FI_RGBA_BLUE] = data[i++];
                                    pixel[FreeImage.FI_RGBA_GREEN] = data[i++];
                                    pixel[FreeImage.FI_RGBA_RED] = data[i++];
                                    pixel += 3;
                                }
                            }
                            i += j;
                        }
                    }
                    break;
                case 32:
                    {
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                Byte* pixel = (Byte*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    pixel[FreeImage.FI_RGBA_BLUE] = data[i++];
                                    pixel[FreeImage.FI_RGBA_GREEN] = data[i++];
                                    pixel[FreeImage.FI_RGBA_RED] = data[i++];
                                    pixel[FreeImage.FI_RGBA_ALPHA] = data[i++];
                                    pixel += 4;
                                }
                            }
                            i += j;
                        }
                    }
                    break;
            }
            return dib;
        }

        private static FIBITMAP NewFiBitMap(PixImage<ushort> pi)
        {
            FreeImageCheck(pi.Volume.Info);
            var sx = pi.Size.X;
            var sy = pi.Size.Y;
            var data = pi.Volume.Data;
            long i = pi.Volume.FirstIndex;
            long j = pi.Volume.JY;

            switch (pi.ChannelCount)
            {
                case 1:
                    {
                        var dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, sx, sy, 16);
                        var delta = (int)FreeImage.GetPitch(dib);
                        var bits = FreeImage.GetBits(dib) + sy * delta;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                ushort* pixel = (ushort*)bits;
                                for (var x = 0; x < sx; x++)
                                    pixel[x] = data[i++];
                            }
                            i += j;
                        }
                        return dib;
                    }
                case 3:
                    {
                        var dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_RGB16, sx, sy, 48);
                        var delta = (int)FreeImage.GetPitch(dib);
                        var bits = FreeImage.GetBits(dib) + sy * delta;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGB16* pixel = (FIRGB16*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    pixel[x].red = data[i++];
                                    pixel[x].green = data[i++];
                                    pixel[x].blue = data[i++];
                                }
                            }
                            i += j;
                        }
                        return dib;
                    }
                case 4:
                    {
                        var dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_RGBA16, sx, sy, 64);
                        var delta = (int)FreeImage.GetPitch(dib);
                        var bits = FreeImage.GetBits(dib) + sy * delta;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGBA16* pixel = (FIRGBA16*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    pixel[x].red = data[i++];
                                    pixel[x].green = data[i++];
                                    pixel[x].blue = data[i++];
                                    pixel[x].alpha = data[i++];
                                }
                            }
                            i += j;
                        }
                        return dib;
                    }
                default:
                    break;
            }
            throw new ArgumentException("cannot save PixImage");
        }

        private static FIBITMAP NewFiBitMap(PixImage<float> pi)
        {
            FreeImageCheck(pi.Volume.Info);
            var sx = pi.Size.X;
            var sy = pi.Size.Y;
            var data = pi.Volume.Data;
            long i = pi.Volume.FirstIndex;
            long j = pi.Volume.JY;

            switch (pi.ChannelCount)
            {
                case 1:
                    {
                        var dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_FLOAT, sx, sy, 32);
                        var delta = (int)FreeImage.GetPitch(dib);
                        var bits = FreeImage.GetBits(dib) + sy * delta;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                float* pixel = (float*)bits;
                                for (var x = 0; x < sx; x++)
                                    pixel[x] = data[i++];
                            }
                            i += j;
                        }
                        return dib;
                    }
                case 3:
                    {
                        var dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_RGBF, sx, sy, 96);
                        var delta = (int)FreeImage.GetPitch(dib);
                        var bits = FreeImage.GetBits(dib) + sy * delta;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGBF* pixel = (FIRGBF*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    pixel[x].red = data[i++];
                                    pixel[x].green = data[i++];
                                    pixel[x].blue = data[i++];
                                }
                            }
                            i += j;
                        }
                        return dib;
                    }
                case 4:
                    {
                        var dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_RGBAF, sx, sy, 128);
                        var delta = (int)FreeImage.GetPitch(dib);
                        var bits = FreeImage.GetBits(dib) + sy * delta;
                        for (var y = 0; y < sy; y++)
                        {
                            bits = bits - delta;
                            unsafe
                            {
                                FIRGBAF* pixel = (FIRGBAF*)bits;
                                for (var x = 0; x < sx; x++)
                                {
                                    pixel[x].red = data[i++];
                                    pixel[x].green = data[i++];
                                    pixel[x].blue = data[i++];
                                    pixel[x].alpha = data[i++];
                                }
                            }
                            i += j;
                        }
                        return dib;
                    }
                default:
                    break;
            }
            throw new ArgumentException("cannot save PixImage");
        }

        protected Dictionary<Type, Func<PixImage, FIBITMAP>> s_createFiBitMap =
            new Dictionary<Type, Func<PixImage, FIBITMAP>>
            {
                { typeof(byte), pi => NewFiBitMap((PixImage<byte>)pi) },
                { typeof(ushort), pi => NewFiBitMap((PixImage<ushort>)pi) },
                { typeof(float), pi => NewFiBitMap((PixImage<float>)pi) },
            };

        protected abstract FIBITMAP ToFiBitMap();

        private static PixImageInfo InfoFromFileNameFreeImage(
                string fileName, PixLoadOptions options)
        {
            // TODO: return something if its possible
            return null;
        }

    }

    public partial class PixImage<T>
    {
        protected override FIBITMAP ToFiBitMap()
        {
            return s_createFiBitMap[typeof(T)](this);
        }
    }

}
#endif

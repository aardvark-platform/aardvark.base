//#define USE_STORAGESERVICE
#if !__MonoCS__ && !__ANDROID__
#define USE_BITMAP
#define USE_SYSTEMIMAGE
#define USE_FREEIMAGE
#define USE_DEVIL
#define USE_LIBTIFF
#endif

#if __ANDROID__
#define USE_ANDROID
#endif

#if __MonoCS__
#define USE_BITMAP
#define USE_FREEIMAGE
#define USE_DEVIL
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace Aardvark.Base
{

    public enum PixFileFormat
    {
        Unknown,
        Bmp,
        Ico,
        Jpeg,
        Jng,
        Koala,
        Lbm,
        Iff,
        Mng,
        Pbm,
        PbmRaw,
        Pcd,
        Pcx,
        Pgm,
        PgmRaw,
        Png,
        Ppm,
        PpmRaw,
        Ras,
        Targa,
        Tiff,
        Wbmp,
        Psd,
        Cut,
        Xbm,
        Xpm,
        Dds,
        Gif,
        Hdr,
        Faxg3,
        Sgi,
        Exr,
        J2k,
        Jp2,
        Pfm,
        Pict,
        Raw,

        Wmp, // not in FreeImage ?
    }

    [Flags]
    public enum PixLoadOptions
    {
		Default             = UseSystemImage | UseFreeImage,
        UseSystemImage      = 0x01000000,
        UseFreeImage        = 0x02000000,
        UseStorageService   = 0x08000000,
        UseDevil            = 0x10000000,
        UseBitmap           = 0x20000000,
        UseLibTiff          = 0x40000000,
    }

    [Flags]
    public enum PixSaveOptions
    {
		Default             = UseSystemImage | UseFreeImage | NormalizeFilename,
        NormalizeFilename   = 0x00010000,
        UseSystemImage      = 0x01000000,
        UseFreeImage        = 0x02000000,
        UseChunkedStream    = 0x04000000,
        UseStorageService   = 0x08000000,
        UseDevil            = 0x10000000,
        UseLibTiff          = 0x20000000,
    }

    /// <summary>
    /// TODO: Design this
    /// </summary>
    public class PixImageMetaData
    {
    }

    [Serializable]
    public abstract partial class PixImage : IPix, IPixOld //, IPixImage2d
    {
        public Col.Format Format;
        // TODO: public PixImageMetaData MetaData;

        #region Constructors

        //static PixImage()
        //{ 
        //    PixImageExtensions.Init();
        //}

        public PixImage()
            : this(Col.Format.None)
        { }

        public PixImage(Col.Format format)
        {
            Format = format;
        }

        #endregion

        #region Constants

        public const long c_writeMemoryChunkSize = 1 << 24;

        #endregion

        #region Properties

        public PixImageInfo Info
        {
            get
            {
                return new PixImageInfo(PixFormat, Size);
            }
        }

        public PixInfoOld PixInfo
        {
            get
            {
                return new PixInfoOld { { PixInfoOld.Property.ColFormat, Format } };
            }
        }

        /// <summary>
        /// Size.X * Size.Y.
        /// </summary>
        public int NumberOfPixels { get { return Size.X * Size.Y; } }

        /// <summary>
        /// Width/height.
        /// </summary>
        public double AspectRatio { get { return Size.X / (double)Size.Y; } }

        // public IEnumerable<INode> SubNodes { get { return Enumerable.Empty<INode>(); } }

        #endregion

        #region Static Tables and Methods

        protected static Dictionary<string, PixFileFormat> s_formatOfExtension =
            new Dictionary<string, PixFileFormat>()
        {
            { ".png", PixFileFormat.Png },
            { ".bmp", PixFileFormat.Bmp },
            { ".jpg", PixFileFormat.Jpeg },
            { ".jpeg", PixFileFormat.Jpeg },
            { ".tif", PixFileFormat.Tiff },
            { ".tiff", PixFileFormat.Tiff },
            { ".gif", PixFileFormat.Gif },
            { ".wmp", PixFileFormat.Wmp },
            { ".pgm", PixFileFormat.Pgm },
            { ".exr", PixFileFormat.Exr },
        };

        protected static Lazy<Dictionary<PixFileFormat, string>> s_preferredExtensionOfFormat =
            new Lazy<Dictionary<PixFileFormat, string>>(() =>
            {
                var result = new Dictionary<PixFileFormat, string>();
                foreach (var pext in from kvp in s_formatOfExtension
                                        group kvp by kvp.Value into g
                                        select new
                                        {
                                            Key = g.Key,
                                            Value = g.OrderBy(x => x.Key.Length).Select(x => x.Key).First()
                                        })
                {
                    result[pext.Key] = pext.Value;
                }
                return result;
            },
            LazyThreadSafetyMode.PublicationOnly
            );

        /// <summary>
        /// Throws exception if file name has no extension,
        /// or extension is unknown format.
        /// </summary>
        protected static PixFileFormat GetFormatOfExtension(string filename)
        {
            if (!Path.HasExtension(filename))
                throw new FileFormatException("File name has no extension: " + filename);

            var ext = Path.GetExtension(filename).ToLowerInvariant();
            return s_formatOfExtension[ext];
        }

        public static string GetPreferredExtensionOfFormat(PixFileFormat format)
        {
            return s_preferredExtensionOfFormat.Value[format];
        }

        private static Dictionary<PixFormat, Func<int, int, int, PixImage>> s_pixImageCreatorMap =
            new Dictionary<PixFormat, Func<int, int, int, PixImage>>()
            {
                { PixFormat.ByteBW, (x, y, c) => new PixImage<byte>(Col.Format.BW, x, y, c) },

                { PixFormat.ByteGray, (x, y, c) => new PixImage<byte>(Col.Format.Gray, x, y, c) },
                { PixFormat.ByteBGR, (x, y, c) => new PixImage<byte>(Col.Format.BGR, x, y, c) },
                { PixFormat.ByteBGRA, (x, y, c) => new PixImage<byte>(Col.Format.BGRA, x, y, c) },
                { PixFormat.ByteRGBA, (x, y, c) => new PixImage<byte>(Col.Format.RGBA, x, y, c) },
                { PixFormat.ByteRGB, (x, y, c) => new PixImage<byte>(Col.Format.RGB, x, y, c) },
                { PixFormat.ByteBGRP, (x, y, c) => new PixImage<byte>(Col.Format.BGRP, x, y, c) },

                { PixFormat.UShortGray, (x, y, c) => new PixImage<ushort>(Col.Format.Gray, x, y, c) },
                { PixFormat.UShortRGB, (x, y, c) => new PixImage<ushort>(Col.Format.RGB, x, y, c) },
                { PixFormat.UShortRGBA, (x, y, c) => new PixImage<ushort>(Col.Format.RGBA, x, y, c) },
                { PixFormat.UShortRGBP, (x, y, c) => new PixImage<ushort>(Col.Format.RGBP, x, y, c) },

                { PixFormat.UIntGray, (x, y, c) => new PixImage<uint>(Col.Format.Gray, x, y, c) },
                { PixFormat.UIntRGB, (x, y, c) => new PixImage<uint>(Col.Format.RGB, x, y, c) },
                { PixFormat.UIntRGBA, (x, y, c) => new PixImage<uint>(Col.Format.RGBA, x, y, c) },
                { PixFormat.UIntRGBP, (x, y, c) => new PixImage<uint>(Col.Format.RGBP, x, y, c) },

                { PixFormat.FloatGray, (x, y, c) => new PixImage<float>(Col.Format.Gray, x, y, c) },
                { PixFormat.FloatRGB, (x, y, c) => new PixImage<float>(Col.Format.RGB, x, y, c) },
                { PixFormat.FloatRGBA, (x, y, c) => new PixImage<float>(Col.Format.RGBA, x, y, c) },
                { PixFormat.FloatRGBP, (x, y, c) => new PixImage<float>(Col.Format.RGBP, x, y, c) },

                { PixFormat.DoubleGray, (x, y, c) => new PixImage<double>(Col.Format.Gray, x, y, c) },
                { PixFormat.DoubleRGB, (x, y, c) => new PixImage<double>(Col.Format.RGB, x, y, c) },
                { PixFormat.DoubleRGBA, (x, y, c) => new PixImage<double>(Col.Format.RGBA, x, y, c) },
                { PixFormat.DoubleRGBP, (x, y, c) => new PixImage<double>(Col.Format.RGBP, x, y, c) },
            };

        private static Dictionary<Type, Func<Array, Col.Format, int, int, int, PixImage>> s_pixImageArrayCreatorMap =
            new Dictionary<Type, Func<Array, Col.Format, int, int, int, PixImage>>()
            {
                { typeof(byte[]), (a, f, x, y, c) =>
                                    new PixImage<byte>(f, ((byte[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(ushort[]), (a, f, x, y, c) =>
                                    new PixImage<ushort>(f, ((ushort[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(uint[]), (a, f, x, y, c) =>
                                    new PixImage<uint>(f, ((uint[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(float[]), (a, f, x, y, c) =>
                                    new PixImage<float>(f, ((float[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(double[]), (a, f, x, y, c) =>
                                    new PixImage<double>(f, ((double[])a).CreateImageVolume(new V3l(x, y, c))) },
            };

            
        #endregion

        #region Static Creator Functions

        public static PixImage Create(PixFormat pixFormat, int sx, int sy, int ch)
        {
            return s_pixImageCreatorMap[pixFormat](sx, sy, ch);
        }

        public static PixImage Create(PixFormat pixFormat, int sx, int sy)
        {
            return s_pixImageCreatorMap[pixFormat](sx, sy, pixFormat.Format.ChannelCount());
        }

        public static PixImage Create(Array array, Col.Format format, int sx, int sy, int ch)
        {
            return s_pixImageArrayCreatorMap[array.GetType()](array, format, sx, sy, ch);
        }

        public static PixImage Create(Array array, Col.Format format, int sx, int sy)
        {
            return s_pixImageArrayCreatorMap[array.GetType()](array, format, sx, sy, format.ChannelCount());
        }

        /// <summary>
        /// Create a new image from the file. This is the standard way of
        /// loading images if they do not need to be converted to a specific
        /// format.
        /// </summary>
        public static PixImage Create(string filename, PixLoadOptions options = PixLoadOptions.Default)
        {
            var loadImage = CreateRaw(filename, options);
            return loadImage.ToPixImage(loadImage.Format);
        }

        public static IEnumerable<PixImage> Create(IEnumerable<string> filenames)
        {
            return from file in filenames
                   select PixImage.Create(file);
        }

        public static PixImage CreateRaw(string filename, PixLoadOptions options = PixLoadOptions.Default)
        {
            #if USE_STORAGESERVICE
            if ((options & PixLoadOptions.UseStorageService) != 0)
                return CreateRaw(Load.AsMemoryStream(filename), options);
            #endif

            #if USE_SYSTEMIMAGE
            if ((options & PixLoadOptions.UseSystemImage) != 0)
            {
                var img = CreateRawSystem(filename, options);
                if (img != null) return img;
            }
            #endif

            #if USE_FREEIMAGE
            if ((options & PixLoadOptions.UseFreeImage) != 0)
            {
                var img = CreateRawFreeImage(new FileStream(filename, FileMode.Open, FileAccess.Read), options);
                if (img != null) return img;
            }
            #endif

            #if USE_DEVIL
            if ((options & PixLoadOptions.UseDevil) != 0)
            {
                var img = CreateRawDevil(new FileStream(filename, FileMode.Open, FileAccess.Read), options);
                if (img != null) return img;
            }
            #endif

            #if USE_LIBTIFF
            if ((options & PixLoadOptions.UseLibTiff) != 0)
            {
                var img = CreateRawLibTiff(filename, options);
                if (img != null) return img;
            }
            #endif

            #if USE_BITMAP
            if ((options & PixLoadOptions.UseBitmap) != 0)
            {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    var img = CreateRawBitmap(stream, options);
                    if (img != null) return img;
                }
            }
            #endif

			#if USE_ANDROID
			if ((options & PixLoadOptions.UseSystemImage) != 0)
			{
				var img = CreateRawAndroid(new FileStream(filename, FileMode.Open, FileAccess.Read), options);
				if (img != null) return img;
			}
			#endif

            throw new FileFormatException("could not load PixImage");
        }

        /// <summary>
        /// Compress BW images from 1 bit per byte to 8 bits per byte.
        /// </summary>
        internal static void CompressPixels(PixImage<byte> pixImage, PixImage<byte> bitImage)
        {
            var bitData = bitImage.Volume.Data; int bi = 0; byte bit = 0x80;
            var pixData = pixImage.Volume.Data;

            // compress pixels
            pixImage.Volume.Info.ForeachYXIndex(
                () => { if (bit != 0x80) { bit = 0x80; bi++; } },
                i =>
                {
                    if ((pixData[i] & 0x80) != 0) bitData[bi] |= bit;
                    bit >>= 1; if (bit == 0) { bit = 0x80; bi++; }
                }
            );

        }

        /// <summary>
        /// Expand BW images from 8 bit per byte to 1 bit per byte.
        /// </summary>
        internal static void ExpandPixels(PixImage<byte> bitImage, PixImage<byte> pixImage)
        {
            var bitData = bitImage.Volume.Data; int bi = 0; byte bit = 0x80;
            var pixData = pixImage.Volume.Data;

            // expand pixels
            pixImage.Volume.Info.ForeachYXIndex(
                () => { if (bit != 0x80) { bit = 0x80; bi++; } },
                i =>
                {
                    pixData[i] = ((bitData[bi] & bit) != 0) ? (byte)255 : (byte)0;
                    bit >>= 1; if (bit == 0) { bit = 0x80; bi++; }
                }
            );
        }

        public static PixImage Create(Stream stream, PixLoadOptions options = PixLoadOptions.Default)
        {
            var loadImage = CreateRaw(stream, options);
            return loadImage.ToPixImage(loadImage.Format);
        }

        public static PixImage CreateRaw(Stream stream, PixLoadOptions options = PixLoadOptions.Default)
        {
            Exception exception = null;
            #if USE_SYSTEMIMAGE
            if ((options & PixLoadOptions.UseSystemImage) != 0)
            {
                try
                {
                    var img = CreateRawSystem(stream, options);
                    if (img != null) return img;
                }
                catch (Exception ex) { if (exception == null) exception = ex; }
            }
            #endif

            #if USE_FREEIMAGE
            if ((options & PixLoadOptions.UseFreeImage) != 0)
            {
                try
                {
                    var img = CreateRawFreeImage(stream, options);
                    if (img != null) return img;
                }
                catch (Exception ex) { if (exception == null) exception = ex; }
            }
            #endif

            #if USE_DEVIL
            if ((options & PixLoadOptions.UseDevil) != 0)
            {
                try
                {
                    var img = CreateRawDevil(stream, options);
                    if (img != null) return img;
                }
                catch (Exception ex) { if (exception == null) exception = ex; }
            }
            #endif

			#if USE_ANDROID
			if ((options & PixLoadOptions.UseSystemImage) != 0)
			{
				try
				{
					var img = CreateRawAndroid(stream, options);
					if (img != null) return img;
				}
				catch (Exception ex) { if (exception == null) exception = ex; }
			}
			#endif

            throw new FileFormatException("could not load image", exception);
        }

        public static PixImage Create(System.Drawing.Bitmap bitmap)
        {
            #if USE_SYSTEMIMAGE
            var loadImage = CreateRawBitmap(bitmap);
            return loadImage.ToPixImage(loadImage.Format);
            #else
            throw new NotSupportedException("WPF is not supported");
            #endif
        }

        public static Volume<T> CreateVolume<T>(V3l size)
        {
            return size.CreateImageVolume<T>();
        }

        public static Volume<T> CreateVolume<T>(long sizeX, long sizeY, long channelCount)
        {
            return new V3l(sizeX, sizeY, channelCount).CreateImageVolume<T>();
        }

        #endregion

        #region Conversions

        protected static Dictionary<Tup<Type, Type>, Func<object, object>>
            s_copyFunMap =
            new Dictionary<Tup<Type, Type>, Func<object, object>>()
            {
                { Tup.Create(typeof(byte), typeof(byte)), v => ((Volume<byte>)v).CopyWindow() },
                { Tup.Create(typeof(byte), typeof(ushort)), v => ((Volume<byte>)v).ToUShortColor() },
                { Tup.Create(typeof(byte), typeof(uint)), v => ((Volume<byte>)v).ToUIntColor() },
                { Tup.Create(typeof(byte), typeof(float)), v => ((Volume<byte>)v).ToFloatColor() },
                { Tup.Create(typeof(byte), typeof(double)), v => ((Volume<byte>)v).ToDoubleColor() },

                { Tup.Create(typeof(ushort), typeof(byte)), v => ((Volume<ushort>)v).ToByteColor() },
                { Tup.Create(typeof(ushort), typeof(ushort)), v => ((Volume<ushort>)v).CopyWindow() },
                { Tup.Create(typeof(ushort), typeof(uint)), v => ((Volume<ushort>)v).ToUIntColor() },
                { Tup.Create(typeof(ushort), typeof(float)), v => ((Volume<ushort>)v).ToFloatColor() },
                { Tup.Create(typeof(ushort), typeof(double)), v => ((Volume<ushort>)v).ToDoubleColor() },
            
                { Tup.Create(typeof(uint), typeof(byte)), v => ((Volume<uint>)v).ToByteColor() },
                { Tup.Create(typeof(uint), typeof(ushort)), v => ((Volume<uint>)v).ToUShortColor() },
                { Tup.Create(typeof(uint), typeof(uint)), v => ((Volume<uint>)v).CopyWindow() },
                { Tup.Create(typeof(uint), typeof(float)), v => ((Volume<uint>)v).ToFloatColor() },
                { Tup.Create(typeof(uint), typeof(double)), v => ((Volume<uint>)v).ToDoubleColor() },

                { Tup.Create(typeof(float), typeof(byte)), v => ((Volume<float>)v).ToByteColor() },
                { Tup.Create(typeof(float), typeof(ushort)), v => ((Volume<float>)v).ToUShortColor() },
                { Tup.Create(typeof(float), typeof(uint)), v => ((Volume<float>)v).ToUIntColor() },
                { Tup.Create(typeof(float), typeof(float)), v => ((Volume<float>)v).CopyWindow() },
                { Tup.Create(typeof(float), typeof(double)), v => ((Volume<float>)v).ToDoubleColor() },

                { Tup.Create(typeof(double), typeof(byte)), v => ((Volume<double>)v).ToByteColor() },
                { Tup.Create(typeof(double), typeof(ushort)), v => ((Volume<double>)v).ToUShortColor() },
                { Tup.Create(typeof(double), typeof(uint)), v => ((Volume<double>)v).ToUIntColor() },
                { Tup.Create(typeof(double), typeof(float)), v => ((Volume<double>)v).ToFloatColor() },
                { Tup.Create(typeof(double), typeof(double)), v => ((Volume<double>)v).CopyWindow() },
            };

        public abstract PixImage<T1> ToPixImage<T1>();

        public PixImage<T> AsPixImage<T>()
        {
            return this as PixImage<T>;
        }

        public PixImage<T> ToPixImage<T>(Col.Format format)
        {
            var castImage = this as PixImage<T>;
            if (castImage != null
                && castImage.Format == format
                && castImage.ChannelCount == format.ChannelCount())
                return castImage;
            return new PixImage<T>(format, this);
        }

        public System.Drawing.Bitmap ToBitmap()
        {
            return new System.Drawing.Bitmap(ToMemoryStream(PixFileFormat.Png), false);
        }

        #endregion

        #region Abstract Methods

        public abstract PixFormat PixFormat { get; }

        public abstract V2i Size { get; }
        public abstract V2l SizeL { get; }

        public abstract int ChannelCount { get; }
        public abstract long ChannelCountL { get; }

        public abstract Array Array { get; }

        public abstract int IntStride { get; }

        public abstract void CopyChannelTo<Tv>(long channelIndex, Matrix<Tv> target);

        public abstract PixImage ToPixImage(Col.Format format);

        public abstract PixImage CopyToPixImage();

        public abstract PixImage CopyToPixImageWithCanonicalDenseLayout();

        public abstract PixImage ToCanonicalDenseLayout();

        public abstract Array Data { get; }

        #endregion

        #region Save as Image

        /// <summary>
        /// Native implementation of PGM image writer, due to dismal
        /// performance of FreeImage version. Currently only 8-bit
        /// gray images are supported, otherwise we still use FreeImage.
        /// </summary>
        private bool SaveAsImagePgm(Stream stream, PixSaveOptions options)
        {
            if (PixFormat != PixFormat.ByteGray) return false;
            var image = ToPixImage<byte>().ToImageLayout();

            var sw = new StreamWriter(stream, Encoding.ASCII);
            sw.NewLine = "\n";
            sw.WriteLine("P5");
            sw.WriteLine("# Created by Aardvark");
            sw.WriteLine("{0} {1}", image.Size.X, image.Size.Y);
            sw.WriteLine("255");
            sw.Flush();
            stream.Write(image.Volume.Data, 0, image.Volume.Data.Length);
            return true;
        }

        /// <summary>
        /// This method handles all saving that can be done natively without
        /// the use of either System.Drawing or FreeImage. Note, that the bool
        /// return value indicates if the image was actually saved, so that
        /// the other libraries can be used as fall backs.
        /// </summary>
        private bool SaveAsImagePixImage(Stream stream, PixFileFormat fileFormat,
                PixSaveOptions options, int qualityLevel)
        {
            switch (fileFormat)
            {
                case PixFileFormat.Pgm: return SaveAsImagePgm(stream, options);
                default: return false;
            }
        }

        public void SaveAsImage(
                Stream stream, PixFileFormat fileFormat,
                PixSaveOptions options = PixSaveOptions.Default, int qualityLevel = -1)
        {
            if (SaveAsImagePixImage(stream, fileFormat, options, qualityLevel)) return;

            #if USE_SYSTEMIMAGE
            if ((options & PixSaveOptions.UseSystemImage) != 0
                && SaveAsImageSystem(stream, fileFormat, options, qualityLevel)) return;
            #endif
            #if USE_FREEIMAGE
            if ((options & PixSaveOptions.UseFreeImage) != 0
                && SaveAsImageFreeImage(stream, fileFormat, options, qualityLevel)) return;
            #endif
            #if USE_DEVIL
            if ((options & PixSaveOptions.UseDevil) != 0
                && SaveAsImageDevil(stream, fileFormat, options, qualityLevel)) return;
            #endif
            #if USE_LIBTIFF
            if ((options & PixSaveOptions.UseDevil) != 0
                && SaveAsImageLibTiff(stream, fileFormat, options, qualityLevel)) return;
            #endif
            
            throw new FileFormatException("could not save PixImage");
        }

        /// <summary>
        /// Makes the file name valid for the given format.
        /// E.g. for PixFileFormat.Png:
        /// "foo.png" -> "foo.png",
        /// "foo" -> "foo.png",
        /// "foo.jpg" -> "foo.jpg.png",
        /// "foo.2011" -> "foo.2011.png",
        /// </summary>
        public static string NormalizedFileName(string fileName, PixFileFormat format)
        {
            bool appendExtension = false;
            if (Path.HasExtension(fileName))
            {
                var ext = Path.GetExtension(fileName);
                if (s_formatOfExtension.ContainsKey(ext))
                {
                    if (s_formatOfExtension[ext] != format)
                    {
                        // conflicting extension
                        // e.g. NormalizedFileName("foo.jpg", PixFileFormat.Png)
                        appendExtension = true;
                    }
                }
                else
                {
                    // unknown extension
                    // e.g. NormalizedFileName("foo.123.2011", PixFileFormat.Png)
                    appendExtension = true;
                }
            }
            else
            {
                // no extension
                // e.g. NormalizedFileName("foo", PixFileFormat.Png)
                appendExtension = true;
            }

            if (appendExtension)
            {
                fileName += GetPreferredExtensionOfFormat(format);
            }

            return fileName;
        }

        public void SaveAsImage(
                string filename, PixFileFormat fileFormat,
                PixSaveOptions options = PixSaveOptions.Default, int qualityLevel = -1)
        {
            if ((options & PixSaveOptions.NormalizeFilename) != 0)
                filename = NormalizedFileName(filename, fileFormat);
            #if USE_STORAGESERVICE
            if ((options & PixSaveOptions.UseStorageService) != 0)
            {
                if ((options & PixSaveOptions.UseChunkedStream) != 0)
                {
                    var mstream = new ChunkedMemoryStream(c_writeMemoryChunkSize);
                    SaveAsImage(mstream, fileFormat, options, qualityLevel);
                    mstream.ChunkList.Save(mstream.Length, fileName);
                }
                else
                {
                    var mstream = new MemoryStream();
                    SaveAsImage(mstream, fileFormat, options, qualityLevel);
                    mstream.Save(fileName);
                }
                return;
            }
            #endif
            var stream = new FileStream(filename, FileMode.Create);
            SaveAsImage(stream, fileFormat, options, qualityLevel);
            stream.Close();
        }

        /// <summary>
        /// Automatically detects file format from filename extension. 
        /// Throws exception if file name does not end in known format extension.
        /// </summary>
        public void SaveAsImage(string filename)
        {
            SaveAsImage(filename, GetFormatOfExtension(filename));
        }

        public MemoryStream ToMemoryStream(
                PixFileFormat fileFormat,
                PixSaveOptions options = PixSaveOptions.Default, int qualityLevel = -1)
        {
            var stream = new MemoryStream();
            SaveAsImage(stream, fileFormat, options, qualityLevel);
            return stream;
        }

        public MemoryStream ToMemoryStream(int qualityLevel)
        {
            return ToMemoryStream(PixFileFormat.Jpeg, PixSaveOptions.Default, qualityLevel);
        }

        #endregion

        #region Image Manipulation (abstract)

        public abstract PixImage RemappedPixImage(
                Matrix<float> xMap, Matrix<float> yMap,
                ImageInterpolation ip = ImageInterpolation.Cubic);

        public abstract PixImage ResizedPixImage(
                V2i size, ImageInterpolation ip = ImageInterpolation.Cubic);

        public abstract PixImage RotatedPixImage(
                double angleInRadiansCCW, bool resize = true,
                ImageInterpolation ip = ImageInterpolation.Cubic);

        public abstract PixImage ScaledPixImage(
                V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic);

        #endregion

        #region Static Utility Methods

        /// <summary>
        /// Gets info about a PixImage without loading the entire image into memory.
        /// </summary>
        public static PixImageInfo InfoFromFileName(
                string fileName, PixLoadOptions options = PixLoadOptions.Default)
        {
            #if USE_SYSTEMIMAGE
            if ((options & PixLoadOptions.UseSystemImage) != 0)
            {
                var info = InfoFromFileNameSystem(fileName, options);
                if (info != null) return info;
            }
            #endif

            #if USE_FREEIMAGE
            if ((options & PixLoadOptions.UseFreeImage) != 0)
            {
                var info = InfoFromFileNameFreeImage(fileName, options);
                if (info != null) return info;
            }
            #endif

            #if USE_DEVIL
            if ((options & PixLoadOptions.UseDevil) != 0)
            {
                var info = InfoFromFileNameDevil(fileName, options);
                if (info != null) return info;
            }
            #endif

            #if USE_LIBTIFF
            if ((options & PixLoadOptions.UseLibTiff) != 0)
            {
                var info = InfoFromFileNameLibTiff(fileName, options);
                if (info != null) return info;
            }
            #endif

            throw new FileFormatException("could not load info of image");
        }

        #endregion

        #region IPix

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixImage(this); }

        #endregion

        //#region IFieldCodeable Members

        //virtual public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    return
        //         new[]
        //    {
        //        new FieldCoder(0, "Format",
        //            (c,o) => c.CodeT(ref ((PixImage)o).Format) ),
        //    };
        //}

        //#endregion

    }

    /// <summary>
    /// The generic PixImage stores an image with a specific channel type that
    /// is specified as type parameter.
    /// </summary>
    [Serializable]
    public partial class PixImage<T> : PixImage, IPixOld<T> //, IPixImage2d
    {
        public Volume<T> Volume;

        #region Constructors

        public PixImage(Col.Format format, Volume<T> volume)
            : base(format)
        {
            Volume = volume;
            //TypeInfo.Add(GetType());
        }

        public PixImage() { }

        public PixImage(Volume<T> volume)
            : this(Col.FormatDefaultOf(typeof(T), (int)(volume.SZ)), volume)
        { }

        public PixImage(V2i size, int channelCount)
            : this(Col.FormatDefaultOf(typeof(T), channelCount),
                   CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(V2l size, long channelCount)
            : this(Col.FormatDefaultOf(typeof(T), (int)channelCount),
                   CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(int sizeX, int sizeY, int channelCount)
            : this(Col.FormatDefaultOf(typeof(T), channelCount),
                   CreateVolume<T>(sizeX, sizeY, channelCount))
        { }

        public PixImage(Col.Format format, V2i size)
            : this(format, CreateVolume<T>(size.X, size.Y, format.ChannelCount()))
        { }

        public PixImage(Col.Format format, V2l size)
            : this(format, CreateVolume<T>(size.X, size.Y, format.ChannelCount()))
        { }

        public PixImage(Col.Format format, int sizeX, int sizeY)
            : this(format, CreateVolume<T>(sizeX, sizeY, format.ChannelCount()))
        { }

        public PixImage(Col.Format format, V2i size, int channelCount)
            : this(format, CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(Col.Format format, V2l size, long channelCount)
            : this(format, CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(Col.Format format, int sizeX, int sizeY, int channelCount)
            : this(format, CreateVolume<T>(sizeX, sizeY, channelCount))
        { }

        public PixImage(PixImageInfo info)
            : this(info.Format.Format, info.Size)
        {
            System.Diagnostics.Trace.Assert(info.Format.Type == typeof(T), "Attempt to create Piximage from PixImageInfo with different Type T.");
        }

        /// <summary>
        /// Create a pixel image from all the given channels in the default format.
        /// Note, that the channels have to be supplied in the canonical order:
        /// red, green, blue, (alpha).
        /// </summary>
        public PixImage(IEnumerable<Matrix<T>> channels)
            : this(channels.ToArray())
        { }

        /// <summary>
        /// Create a pixel image from all the given channels in the default format.
        /// Note, that the channels have to be supplied in the canonical order:
        /// red, green, blue, (alpha).
        /// </summary>
        public PixImage(params Matrix<T>[] channels)
            : this(Col.FormatDefaultOf(typeof(T), channels.Length), channels)
        { }

        /// <summary>
        /// Create a pixel image from all the given channels in the specified format.
        /// Note, that the channels have to be supplied in the canonical order: red,
        /// green, blue, (alpha).
        /// </summary>
        public PixImage(Col.Format format, IEnumerable<Matrix<T>> channels)
            : this(format, channels.ToArray())
        { }

        /// <summary>
        /// Create a pixel image from all the given channels in the specified format.
        /// Note, that the channels have to be supplied in the canonical order: red,
        /// green, blue, (alpha).
        /// </summary>
        public PixImage(Col.Format format, params Matrix<T>[] channels)
            : base(format)
        {
            int ch = channels.Length;
            var ch0 = channels[0];

            var sx = ch0.SX;
            var sy = ch0.SY;

            var volume = CreateVolume<T>(sx, sy, ch);
            var order = format.ChannelOrder();
            if (ch != format.ChannelCount())
                throw new ArgumentException("the specified format needs a different number of channels");
            for (int i = 0; i < ch; i++)
            {
                var mat = volume.SubXYMatrix(order[i]);
                var channel = channels[i];
                if (channel.IsValid) mat.Set(channel);
            }

            Volume = volume;
        }

        /// <summary>
        /// Copy constructor: ALWAYS creates a copy of the data!
        /// </summary>
        public PixImage(PixImage pixImage)
            : this(Col.FormatDefaultOf(typeof(T), pixImage.Format.ChannelCount()), pixImage)
        { }

        /// <summary>
        /// Copy constructor: ALWAYS creates a copy of the data!
        /// </summary>
        public PixImage(Col.Format format, PixImage pixImage)
        {
            var size = pixImage.Size;
            var channelCount = format.ChannelCount();
            var volume = CreateVolume<T>(size.X, size.Y, channelCount);
            var order = format.ChannelOrder();
            var typedPixImage = pixImage as PixImage<T>;
            if (channelCount != pixImage.ChannelCount
                && !(channelCount == 3 && pixImage.ChannelCount == 4))
            {
                if (channelCount == 4 && pixImage.ChannelCount == 3)
                {
                    channelCount = 3;   // only copy three channels, and set channel 4 (implied alpha) to 'opaque'
                    volume.SubXYMatrix(3).Set(Col.Info<T>.MaxValue);
                }
                else if (channelCount > 1 && pixImage.ChannelCount == 1)
                {
                    if (typedPixImage != null)
                    {
                        var mat = typedPixImage.Matrix;
                        for (int i = 0; i < channelCount; i++)
                            volume.SubXYMatrix(i).Set(mat);
                    }
                    else
                    {
                        for (int i = 0; i < channelCount; i++)
                            pixImage.CopyChannelTo(0, volume.SubXYMatrix(i));
                    }
                    Volume = volume;
                    Format = format;
                    return;
                }
                else
                    throw new ArgumentException("cannot perform color space conversion");
            }

            if (format.IsPremultiplied() != pixImage.Format.IsPremultiplied())
            {
                throw new NotImplementedException(
                        "conversion between alpha and premultiplied alpha not implemented yet");
            }

            if (typedPixImage != null)
            {
                var channelArray = typedPixImage.ChannelArray;
                for (int i = 0; i < channelCount; i++)
                    volume.SubXYMatrix(order[i]).Set(channelArray[i]);
            }
            else
            {
                for (int i = 0; i < channelCount; i++)
                    pixImage.CopyChannelTo(i, volume.SubXYMatrix(order[i]));
            }

            Volume = volume;
            Format = format;
        }

        public PixImage(System.Drawing.Bitmap bitmap)
        {
            var loadImage = Create(bitmap);
            var loadPixImage = loadImage as PixImage<T>;
            if (loadPixImage == null)
                loadPixImage = new PixImage<T>(loadImage);
            Format = loadPixImage.Format;
            Volume = loadPixImage.Volume;
        }

        public PixImage<T> CopyToImageLayout()
        {
            if (Volume.HasImageLayout())
                return new PixImage<T>(Format, Volume.CopyImage());
            return new PixImage<T>(this);
        }

        public PixImage<T> Copy()
        {
            return new PixImage<T>(Format, Volume.CopyImageWindow());
        }

        public override PixImage CopyToPixImage()
        {
            return Copy();
        }

        public override PixImage CopyToPixImageWithCanonicalDenseLayout()
        {
            return CopyToImageLayout();
        }

        public override PixImage ToCanonicalDenseLayout()
        {
            return this.ToImageLayout();
        }

        /// <summary>
        /// Copy function for color conversions.
        /// </summary>
        /// <typeparam name="Tv"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> fun)
        {
            return Copy<Tv>(fun, Format);
        }

        /// <summary>
        /// Copy function for color conversions. Note that the
        /// new color format must have the same number of channels
        /// as the old one, and the result of the supplied conversion
        /// function is reinterpreted as a color in the new format.
        /// </summary>
        /// <typeparam name="Tv"></typeparam>
        /// <param name="fun"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> fun, Col.Format format)
        {
            var mat = GetMatrix<Tv>().CopyWindow(fun);
            var vol = new Volume<T>(mat.Data, Volume.Info);
            return new PixImage<T>(format, vol);
        }

        #endregion

        #region Constructors from File

        public PixImage(string filename, PixLoadOptions options = PixLoadOptions.Default)
        {
            var loadImage = PixImage.CreateRaw(filename, options);
            var channelCount = loadImage.Format.ChannelCount();
            var image = loadImage as PixImage<T>;

            if (image == null || image.ChannelCount != channelCount)
                image = new PixImage<T>(loadImage);

            Volume = image.Volume;
            Format = image.Format;
        }
        
        #endregion

        #region Static Creator Functions

        public static PixImage<T> Create(params Matrix<T>[] channels)
        {
            return new PixImage<T>(channels);
        }

        /// <summary>
        /// Create a pixel image from all the given channels in the specified format.
        /// Note, that the channels have to be supplied in the canonical order: red,
        /// green, blue, (alpha).
        /// <param name="format">the color format</param>
        /// <param name="channels">channel matrices</param>
        /// </summary>
        public static PixImage<T> Create(Col.Format format, params Matrix<T>[] channels)
        {
            return new PixImage<T>(format, channels);
        }

        /// <summary>
        /// Create a pixel image from all the given channels in the specified format.
        /// Note, that the channels have to be supplied in the canonical order: red,
        /// green, blue, (alpha).
        /// </summary>
        public static PixImage<T> Create<Td>(Col.Format format, params Matrix<Td, T>[] channels)
        {
            long ch = channels.Length;
            var ch0 = channels[0];

            var sx = ch0.SX;
            var sy = ch0.SY;

            var volume = CreateVolume<T>(sx, sy, ch);
            var order = format.ChannelOrder();

            if (ch != format.ChannelCount())
                throw new ArgumentException("the specified format needs a different number of channels");            
            channels.ForEach((channel, ci) =>
                volume.SubXYMatrix(order[ci]).Set(channel));

            return new PixImage<T>(format, volume);
        }

        #endregion

        #region Properties

        public override V2i Size
        {
            get { return (V2i)Volume.Info.Size.XY; }
        }

        public override V2l SizeL
        {
            get { return Volume.Info.Size.XY; }
        }

        public override int ChannelCount
        {
            get { return (int)Volume.Info.Size.Z; }
        }

        public override long ChannelCountL
        {
            get { return Volume.Info.Size.Z; }
        }

        /// <summary>
        /// Returns the channels of the image in canonical order: red, green,
        /// blue, (alpha).
        /// </summary>
        public IEnumerable<Matrix<T>> Channels
        {
            get
            {
                long[] order = Format.ChannelOrder();
                for (long i = 0; i < order.Length; i++)
                    yield return GetChannelInFormatOrder(order[i]);
            }
        }

        /// <summary>
        /// Returns the array containing the cahnnels in canonical order: red,
        /// green, blue, (alpha).
        /// </summary>
        public Matrix<T>[] ChannelArray
        {
            get { return Channels.ToArray(); }
        }

        public int BytesPerChannel
        {
            get { return System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)); }
        }

        public override Array Array
        {
            get { return Volume.Data; }
        }

        public override int IntStride
        {
            get { return BytesPerChannel * (int)Volume.DY; }
        }

        public new PixInfoOld PixInfo
        {
            get
            {
                return new PixInfoOld
                {
                    { PixInfoOld.Property.Type, typeof(T) },
                    { PixInfoOld.Property.ColFormat, Format },
                };
            }
        }

        /// <summary>
        /// Returns the matrix representation of the volume if there is only
        /// one channel. Fails if there are multiple channels.
        /// </summary>
        public Matrix<T> Matrix
        {
            get { return Volume.AsMatrixWindow(); }
        }

        #endregion

        #region Image Manipulation

        public override PixImage RemappedPixImage(
                Matrix<float> xMap, Matrix<float> yMap,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Remapped(xMap, xMap, ip);
        }

        public PixImage<T> Remapped(
                Matrix<float> xMap, Matrix<float> yMap,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return new PixImage<T>(Format, s_remappedFun(Volume, xMap, yMap, ip));
        }

        private static Func<Volume<T>, Matrix<float>, Matrix<float>, ImageInterpolation, Volume<T>> s_remappedFun = null;

        public static void SetRemappedFun(
                Func<Volume<T>, Matrix<float>, Matrix<float>, ImageInterpolation, Volume<T>> remappedFun)
        {
            s_remappedFun = remappedFun;
        }

        public override PixImage ResizedPixImage(
                V2i newSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Scaled((V2d)newSize / (V2d)Size, ip);
        }

        public PixImage<T> Resized(
                V2i newSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Scaled((V2d)newSize / (V2d)Size, ip);
        }

        public PixImage<T> Resized(
                int xSize, int ySize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Scaled(new V2d(xSize, ySize) / (V2d)Size, ip);
        }

        public override PixImage RotatedPixImage(
                double angleInRadiansCCW, bool resize = true,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Rotated(angleInRadiansCCW, resize, ip);
        }

        public PixImage<T> Rotated(
                double angleInRadiansCCW, bool resize = true,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return new PixImage<T>(Format, s_rotatedFun(Volume, angleInRadiansCCW, resize, ip));
        }

        private static Func<Volume<T>, double, bool, ImageInterpolation, Volume<T>> s_rotatedFun = null;

        public static void SetRotatedFun(
                Func<Volume<T>, double, bool, ImageInterpolation, Volume<T>> rotatedFun)
        {
            s_rotatedFun = rotatedFun;
        }

        public override PixImage ScaledPixImage(
                V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Scaled(scaleFactor, ip);
        }

        public PixImage<T> Scaled(
                V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            Requires.That(scaleFactor.X > 0.0 && scaleFactor.Y > 0.0);

            // SuperSample is only available for scale factors < 1; fall back to Cubic
            if ((scaleFactor.X >= 1.0 || scaleFactor.Y >= 1.0) && ip == ImageInterpolation.SuperSample)
                ip = ImageInterpolation.Cubic;

            return new PixImage<T>(Format, s_scaledFun(Volume, scaleFactor, ip));
        }

        private static Func<Volume<T>, V2d, ImageInterpolation, Volume<T>> s_scaledFun = null;

        public static void SetScaledFun(Func<Volume<T>, V2d, ImageInterpolation, Volume<T>> scaledFun)
        {
            s_scaledFun = scaledFun;
        }


        public PixImage<T> Scaled(
                double scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Scaled(new V2d(scaleFactor, scaleFactor), ip);
        }

        public PixImage<T> Scaled(
                double xScaleFactor, double yScaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return Scaled(new V2d(xScaleFactor, yScaleFactor), ip);
        }

        #endregion

        #region SubImages

        public PixImage<T> SubImage(long x, long y, long xSize, long ySize)
        {
            if (x < 0 || y < 0 || x + xSize > Size.X || y + ySize > Size.Y)
                throw new ArgumentOutOfRangeException(
                                        "subregion out of image boundary");
            return new PixImage<T>(Format,
                    Volume.SubVolume(x, y, 0, xSize, ySize, ChannelCount));
        }

        /// <summary>
        /// Returns a specified region as new PixImage.
        /// </summary>
        public PixImage<T> SubImage(V2i pos, V2i size)
        {
            return SubImage(pos.X, pos.Y, size.X, size.Y);
        }

        public PixImage<T> SubImage(V2l pos, V2l size)
        {
            return SubImage(pos.X, pos.Y, size.X, size.Y);
        }

        /// <summary>
        /// Returns a specified region as new PixImage.
        /// </summary>
        public PixImage<T> SubImage(Box2i box)
        {
            return SubImage(box.Min, box.Size);
        }

        /// <summary>
        /// Returns a square region around the center as new PixImage.
        /// SquareRadius is the given radus around the center.
        /// </summary>
        public PixImage<T> SubImage(V2i center, int squareRadius)
        {
            return SubImage(center - squareRadius, V2i.II * squareRadius * 2 + 1);
        }

        /// <summary>
        /// Returns a specified region as a new PixImage.
        /// The supplied pos and size are rounded to the nearest integer.
        /// Note that this may be different from SubImage(Box2d box), since
        /// rounding the size and rounding the Max of the box may
        /// result in different integer sizes.
        /// </summary>
        public PixImage<T> SubImage(V2d pos, V2d size)
        {
            return SubImage(new V2i(pos + 0.5), new V2i(size + 0.5));
        }

        /// <summary>
        /// Returns a specified region as a new PixImage.
        /// Min and Max of the supplied box are rounded to the nearest integer.
        /// Note that this may be different from SubImage(V2d pos, V2d size),
        /// since rounding the Max of the box and rounding the size may
        /// result in different integer sizes.
        /// </summary>
        public PixImage<T> SubImage(Box2d box)
        {
            var min = new V2i(box.Min + 0.5);
            return SubImage(min, new V2i(box.Max + 0.5) - min);
        }

        /// <summary>
        /// Coords in normalized [0, 1] coords of 'this'.
        /// The supplied pos and size are converted to pixel coordinates and
        /// rounded to the nearest integer. Note that this may be different
        /// from SubImage01(Box2d box) since rounding the size and rounding
        /// the Max of the box may result in different integer sizes.
        /// </summary>
        public PixImage<T> SubImage01(V2d pos, V2d size)
        {
            var iPos = (V2i)(pos * (V2d)Size + 0.5);
            var iSize = (V2i)(size * (V2d)Size + 0.5);
            return SubImage(iPos, iSize);
        }

        /// <summary>
        /// Coords in normalized [0, 1] coords of 'this'.
        /// Min and Max of the supplied box are converted to pixel coordinates
        /// and rounded to the nearest integer. Note that this may be different
        /// from SubImage01(V2d pos, V2d size), since rounding the Max of the
        /// box and rounding the size may result in different integer sizes.
        /// </summary>
        public PixImage<T> SubImage01(Box2d bounds)
        {
            var iMin = (V2i)(bounds.Min * (V2d)Size + 0.5);
            var iMax = (V2i)(bounds.Max * (V2d)Size + 0.5);
            return SubImage(iMin, iMax - iMin);
        }

        #endregion

        #region Setting

        /// <summary>
        /// Set a subregion of the image to the contents of another image.
        /// The size of the subregion is determined by the other image.
        /// </summary>
        public void Set(int x, int y, PixImage<T> img)
        {
            Set(new V2i(x, y), img);
        }

        /// <summary>
        /// Set a subregion of the image to the contents of another image.
        /// The size of the subregion is dtermined by the other image.
        /// </summary>
        public void Set(V2i pos, PixImage<T> image)
        {
            SubImage(pos, image.Size).Set(image);
        }

        /// <summary>
        /// Set the image contents of the image to the contents of another
        /// image. The sizes of the image and the other image have to match.
        /// </summary>
        public void Set(PixImage<T> image)
        {
            if (Format != image.Format)
                throw new ArgumentException("wrong PixImage.Format");
            if (Size != image.Size)
                throw new ArgumentException("size mismatch");
            Volume.Set(image.Volume);
        }

        /// <summary>
        /// Set the image contents of the image to the contents of another
        /// image. The contents of the other image are resized to match.
        /// </summary>
        public void SetResized(
                PixImage<T> image,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            if (Format != image.Format)
                throw new ArgumentException("wrong PixImage.Format");

            if (Size == image.Size)
                Volume.Set(image.Volume);
            else
                Volume.Set(image.Resized(Size, ip).Volume);
        }

        #endregion

        #region Conversions

        public PixImage<T> ToImageLayout()
        {
            if (!Volume.HasImageLayout())
                return new PixImage<T>(Format, this);
            else return this;
        }

        public PixImage<T> ToFormat(Col.Format format)
        {
            return Format == format ? this : new PixImage<T>(format, this);
        }

        public override PixImage<T1> ToPixImage<T1>()
        {
            var castImage = this as PixImage<T1>;
            if (castImage != null) return castImage;
            var format = typeof(T1).FormatDefaultOf(ChannelCount);
            if (Format == format)
            {
                var copy = s_copyFunMap[Tup.Create(typeof(T), typeof(T1))];
                return new PixImage<T1>(format, (Volume<T1>)copy(Volume));
            }
            return new PixImage<T1>(this);
        }

        #endregion

        #region Obtaining Matrices

        /// <summary>
        /// Returns the specified channel, based on the canonical channel
        /// order: red, green, blue, (alpha).
        /// </summary>
        public Matrix<T> GetChannel(long channelIndex)
        {
            var order = Format.ChannelOrder();
            return GetChannelInFormatOrder(order[channelIndex]);
        }

        /// <summary>
        /// Returns the specified channel.
        /// </summary>
        public Matrix<T> GetChannel(Col.Channel channel)
        {
            return GetChannelInFormatOrder(Format.ChannelIndex(channel));
        }

        /// <summary>
        /// Returns the specified channel (based on the canonical order) with
        /// a different view type.
        /// </summary>
        public Matrix<T, Tv> GetChannel<Tv>(long channelIndex)
        {
            return GetChannelInFormatOrder<Tv>(Format.ChannelOrder()[channelIndex]);
        }

        /// <summary>
        /// Returns the specified channel based on the order of the image's
        /// color format.
        /// </summary>
        public Matrix<T> GetChannelInFormatOrder(long formatChannelIndex)
        {
            return Volume.SubXYMatrixWindow(formatChannelIndex);
        }

        /// <summary>
        /// Returns the specified channel based on the order of the image's
        /// color format with a different view type.
        /// <param name="formatChannelIndex">formatChannelIndex</param>
        /// </summary>
        public Matrix<T, Tv> GetChannelInFormatOrder<Tv>(long formatChannelIndex)
        {
            var matrix = Volume.SubXYMatrix<Tv>(formatChannelIndex);
            matrix.Accessors = TensorAccessors.Get<T, Tv>(
                    typeof(T), typeof(Tv), TensorAccessors.Intent.ColorChannel, Volume.DeltaArray);
            return matrix;
        }
            
        public Matrix<T, Tv> GetMatrix<Tv>()
        {
            return Volume.GetMatrix<T, Tv>(Format.GetIntent());
        }

        #endregion

        #region Concrete Implementation Of Abstract Functions

        public override PixFormat PixFormat
        {
            get { return new PixFormat(typeof(T), Format); }
        }

        public override void CopyChannelTo<Tv>(long channelIndex, Matrix<Tv> target)
        {
            var subMatrix = GetChannel<Tv>(channelIndex);
            target.Set(subMatrix);
        }

        public override PixImage ToPixImage(Col.Format format)
        {
            if (Format == format && ChannelCount == format.ChannelCount())
                return this;
            return new PixImage<T>(format, this);
        }

        public override Array Data
        {
            get
            {
                Requires.That(Volume.HasImageLayout());
                return Volume.Data;
            }
        }

        #endregion

        //#region IFieldCodeable Members

        //override public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    return base.GetFieldCoders(coderVersion).Base().Concat(
        //        new[]
        //        {
        //            new FieldCoder(0, "Volume",
        //                (c,o) => c.CodeT(ref ((PixImage<T>)o).Volume) ),
        //        });
        //}

        //#endregion

    }

}

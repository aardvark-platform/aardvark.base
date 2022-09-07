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
    [Obsolete]
    public enum PixLoadOptions
    {
        Default             = UseSystemImage | UseDevil | UseFreeImage,
        UseSystemImage      = 0x01000000,
        UseFreeImage        = 0x02000000,
        UseStorageService   = 0x08000000,   // deprecated
        UseDevil            = 0x10000000,
        UseBitmap           = 0x20000000,
        UseLibTiff          = 0x40000000,
    }

    [Flags]
    [Obsolete]
    public enum PixSaveOptions
    {
        Default             = UseSystemImage | UseDevil | UseFreeImage | NormalizeFilename,
        NormalizeFilename   = 0x00010000,
        UseSystemImage      = 0x01000000,
        UseFreeImage        = 0x02000000,
        UseChunkedStream    = 0x04000000,
        UseStorageService   = 0x08000000, // deprecated
        UseDevil            = 0x10000000,
        UseLibTiff          = 0x20000000,
        UseBitmap           = 0x40000000,
    }

    public interface IPixImageVisitor<T>
    {
        T Visit<TData>(PixImage<TData> image);
    }

    [Serializable]
    public abstract partial class PixImage : IPix, IPixImage2d
    {
        public Col.Format Format;

        #region Loaders

        #region LegacyPixLoader

        // Obsolete: Loader implementation for legacy API
        [Obsolete]
        private class LegacyPixLoader : IPixLoader
        {
            private readonly Func<string, PixLoadOptions, PixImage> loadFromFile;
            private readonly Func<Stream, PixLoadOptions, PixImage> loadFromStream;
            private readonly Func<string, PixSaveOptions, PixFileFormat, int, PixImage, bool> saveToFile;
            private readonly Func<Stream, PixSaveOptions, PixFileFormat, int, PixImage, bool> saveToStream;
            private readonly Func<string, PixImageInfo> getInfo;

            public string Name { get; }

            public LegacyPixLoader(
                        string name,
                        Func<string, PixLoadOptions, PixImage> loadFromFile,
                        Func<Stream, PixLoadOptions, PixImage> loadFromStream,
                        Func<string, PixSaveOptions, PixFileFormat, int, PixImage, bool> saveToFile,
                        Func<Stream, PixSaveOptions, PixFileFormat, int, PixImage, bool> saveToStream,
                        Func<string, PixImageInfo> getInfo)
            {
                Name = name;
                this.loadFromFile = loadFromFile;
                this.loadFromStream = loadFromStream;
                this.saveToFile = saveToFile;
                this.saveToStream = saveToStream;
                this.getInfo = getInfo;
            }

            public PixImage LoadFromFile(string filename)
            {
                if (loadFromFile != null)
                    return loadFromFile(filename, PixLoadOptions.Default);
                else
                {
                    if (loadFromStream != null)
                    {
                        using (var fs = File.OpenRead(filename))
                        {
                            return LoadFromStream(fs);
                        }
                    }
                    else
                        throw new ImageLoadException($"{Name} loader does not support loading from files and streams.");
                }
            }

            public PixImage LoadFromStream(Stream stream)
            {
                if (loadFromStream != null)
                    return loadFromStream(stream, PixLoadOptions.Default);
                else
                    throw new ImageLoadException($"{Name} loader does not support loading from streams.");
            }

            public void SaveToFile(string filename, PixImage image, PixSaveParams saveParams)
            {
                if (saveToFile != null)
                {
                    int quality = (saveParams is PixJpegSaveParams jpg) ? jpg.Quality : PixJpegSaveParams.DefaultQuality;
                    if (!saveToFile(filename, PixSaveOptions.Default, saveParams.Format, quality, image))
                        throw new ImageLoadException("Internal error.");
                }
                else
                {
                    if (saveToStream != null)
                    {
                        using (var fs = File.OpenWrite(filename))
                        {
                            SaveToStream(fs, image, saveParams);
                        }
                    }
                    else
                        throw new NotSupportedException($"{Name} loader does not support saving to files and streams.");
                }
            }

            public void SaveToStream(Stream stream, PixImage image, PixSaveParams saveParams)
            {
                if (saveToStream != null)
                {
                    int quality = (saveParams is PixJpegSaveParams jpg) ? jpg.Quality : PixJpegSaveParams.DefaultQuality;
                    if (!saveToStream(stream, PixSaveOptions.Default, saveParams.Format, quality, image))
                        throw new ImageLoadException("Internal error.");
                }
                else
                {
                    throw new NotSupportedException($"{Name} loader does not support saving to streams.");
                }
            }

            public PixImageInfo GetInfoFromFile(string filename)
            {
                if (getInfo != null)
                {
                    var info = getInfo(filename);
                    if (info != null)
                        return info;
                    else
                        throw new ImageLoadException($"Could not get info from file '{filename}'");
                }
                else
                    throw new ImageLoadException($"{Name} loader does not support retrieving info from files.");
            }

            public PixImageInfo GetInfoFromStream(Stream stream)
                => throw new ImageLoadException($"{Name} loader does not support retrieving info from streams.");
        }

        #endregion

        #region PgmPixLoader

        private class PgmPixLoader : IPixLoader
        {
            public string Name { get; }

            public PgmPixLoader()
            {
                Name = "Aardvark PGM";
            }

            public PixImage LoadFromFile(string filename)
                => throw new ImageLoadException($"{Name} loader does not support loading from files.");

            public PixImage LoadFromStream(Stream stream)
                => throw new ImageLoadException($"{Name} loader does not support loading from streams.");

            public void SaveToFile(string filename, PixImage image, PixSaveParams saveParams)
            {
                using (var stream = File.OpenWrite(filename))
                {
                    SaveToStream(stream, image, saveParams);
                }
            }

            public void SaveToStream(Stream stream, PixImage image, PixSaveParams saveParams)
            {
                if (saveParams.Format != PixFileFormat.Pgm || image.PixFormat != PixFormat.ByteGray)
                    throw new NotSupportedException($"{Name} loader only supports PixFormat.ByteGray and PixFileFormat.Pgm");

                var img = image.ToPixImage<byte>().ToImageLayout();

                var sw = new StreamWriter(stream, Encoding.ASCII);
                sw.NewLine = "\n";
                sw.WriteLine("P5");
                sw.WriteLine("# Created by Aardvark");
                sw.WriteLine("{0} {1}", img.Size.X, img.Size.Y);
                sw.WriteLine("255");
                sw.Flush();
                stream.Write(img.Volume.Data, 0, img.Volume.Data.Length);
            }

            public PixImageInfo GetInfoFromFile(string filename)
                => throw new ImageLoadException($"{Name} loader does not support retrieving info from files.");

            public PixImageInfo GetInfoFromStream(Stream stream)
                => throw new ImageLoadException($"{Name} loader does not support retrieving info from streams.");
        }

        #endregion

        private static readonly Dictionary<IPixLoader, int> s_loaders = new Dictionary<IPixLoader, int>();

        /// <summary>
        /// Sets the priority of a PixImage loader.
        /// The priority determines the order in which loaders are invoked to load or save an image.
        /// Loaders with higher priority are invoked first.
        /// If the loader does not exist, it is added with the given priority.
        /// </summary>
        /// <param name="loader">The loader to modify.</param>
        /// <param name="priority">The priority to set.</param>
        public static void SetLoader(IPixLoader loader, int priority)
        {
            lock (s_loaders)
            {
                s_loaders[loader] = priority;
            }
        }

        /// <summary>
        /// Adds a PixImage loader.
        /// Assigns a priority that is greater than the highest priority among existing loaders, resulting in a LIFO order.
        /// If the loader already exists, the priority is not modified.
        /// </summary>
        /// <param name="loader">The loader to add.</param>
        public static void AddLoader(IPixLoader loader)
        {
            lock (s_loaders)
            {
                var loaders = s_loaders.ToList();
                loaders.Sort((x, y) => y.Value - x.Value);
                SetLoader(loader, loaders.Map(x => x.Value).FirstOrDefault(-1) + 1);
            }
        }

        /// <summary>
        /// Removes a PixImage loader.
        /// </summary>
        /// <param name="loader">The loader to remove.</param>
        public static void RemoveLoader(IPixLoader loader)
        {
            lock (s_loaders)
            {
                s_loaders.Remove(loader);
            }
        }

        /// <summary>
        /// Gets a dictionary of registered loaders with their associated priority.
        /// </summary>
        /// <returns>A dictionary of registered loaders.</returns>
        public static Dictionary<IPixLoader, int> GetLoadersWithPriority()
        {
            lock (s_loaders)
            {
                return new Dictionary<IPixLoader, int>(s_loaders);
            }
        }

        /// <summary>
        /// Gets a list of registered loaders sorted by priority in descending order.
        /// </summary>
        /// <returns>A list of registered loaders.</returns>
        public static List<IPixLoader> GetLoaders()
        {
            lock (s_loaders)
            {
                var list =  s_loaders.ToList();
                list.Sort((x, y) => y.Value - x.Value);
                return list.Map(x => x.Key);
            }
        }

        [Obsolete("Use AddLoader with IPixLoader interface")]
        public static void RegisterLoadingLib(
            Func<string, PixLoadOptions, PixImage> fileLoad = null,
            Func<Stream, PixLoadOptions, PixImage> streamLoad = null,
            Func<string, PixSaveOptions, PixFileFormat, int, PixImage, bool> fileSave = null,
            Func<Stream, PixSaveOptions, PixFileFormat, int, PixImage, bool> streamSave = null,
            Func<string, PixImageInfo> imageInfo = null)
        {
            lock (s_loaders)
            {
                var names = s_loaders.ToList().Map(x => x.Key.Name);
                int index = 2;
                string name = "Unnamed";

                while (names.Contains(name))
                {
                    name = $"Unnamed ({index++})";
                }

                var loader = new LegacyPixLoader(name, fileLoad, streamLoad, fileSave, streamSave, imageInfo);
                AddLoader(loader);
            }
        }

        private static Result TryInvokeLoader<Result>(
                        IPixLoader loader, Func<IPixLoader, Result> invoke,
                        Func<Result, bool> isValid,
                        string operationDescription)
        {
            Report.BeginTimed(3, $"Trying to {operationDescription} with {loader.Name} loader");

            try
            {
                var result = invoke(loader);
                if (isValid(result))
                {
                    Report.EndTimed(3, ": success in");
                    return result;
                }
                else
                {
                    Report.EndTimed(3, ": failed in");
                }
            }
            catch (Exception e)
            {
                Report.EndTimed(3, ": failed in");
                Report.Line(3, $"Failed to {operationDescription} with {loader.Name} loader: {e.Message}");
            }

            return default;
        }

        private static Result InvokeLoaders<Input, Result>(
                                IPixLoader loader, Input input,
                                Func<IPixLoader, Input, Result> tryInvoke,
                                Action<Input> resetInput,
                                Func<Result, bool> isValid,
                                string errorMessage)
        {
            if (loader != null)
            {
                var result = tryInvoke(loader, input);
                if (isValid(result)) { return result; }
            }
            else
            {
                var loaders = GetLoaders();

                for (int i = 0; i < loaders.Count; i++)
                {
                    if (i != 0) resetInput(input);

                    var result = tryInvoke(loaders[i], input);
                    if (isValid(result)) return result;
                }
            }

            throw new ImageLoadException(errorMessage);
        }

        private static Result InvokeLoadersWithStream<Result>(
                        IPixLoader loader, Stream stream,
                        Func<IPixLoader, Stream, Result> tryInvoke,
                        Func<Result, bool> isValid,
                        string errorMessage)
        {
            var initialPosition = stream.Position;

            return InvokeLoaders(
                loader, stream, tryInvoke,
                s => s.Seek(initialPosition, SeekOrigin.Begin),
                isValid, errorMessage
            );
        }

        private static void Ignore<T>(T _) { }

        private static bool NotNull<T>(T x) => x != null;

        private static bool Identity(bool x) => x;

        #endregion

        #region Constructors

        static PixImage()
        {
            AddLoader(new PgmPixLoader());
        }

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

        public PixImageInfo Info {  get { return new PixImageInfo(PixFormat, Size); } }

        /// <summary>
        /// Size.X * Size.Y.
        /// </summary>
        public int NumberOfPixels { get { return Size.X * Size.Y; } }

        /// <summary>
        /// Width/height.
        /// </summary>
        public double AspectRatio { get { return Size.X / (double)Size.Y; } }

        #endregion

        #region Static Tables and Methods

        protected static Dictionary<string, PixFileFormat> s_formatOfExtension =
            new Dictionary<string, PixFileFormat>()
        {
            { ".tga", PixFileFormat.Targa },
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
                                            g.Key,
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
        /// Gets the image file format from a file path.
        /// Throws exception if file name has no extension, or extension is unknown format.
        /// </summary>
        protected static PixFileFormat GetFormatOfExtension(string filename)
        {
            if (!Path.HasExtension(filename))
                throw new ArgumentException("File name has no extension: " + filename);

            var ext = Path.GetExtension(filename).ToLowerInvariant();
            if (s_formatOfExtension.TryGetValue(ext, out var format))
                return format;
            else
                throw new ArgumentException("File name has unknown extension: " + ext);
        }

        public static string GetPreferredExtensionOfFormat(PixFileFormat format)
        {
            return s_preferredExtensionOfFormat.Value[format];
        }

        private static Dictionary<PixFormat, Func<long, long, long, PixImage>> s_pixImageCreatorMap =
            new Dictionary<PixFormat, Func<long, long, long, PixImage>>()
            {
                { PixFormat.ByteBW, (x, y, c) => new PixImage<byte>(Col.Format.BW, x, y, c) },
                { PixFormat.ByteGray, (x, y, c) => new PixImage<byte>(Col.Format.Gray, x, y, c) },
                { PixFormat.ByteBGR, (x, y, c) => new PixImage<byte>(Col.Format.BGR, x, y, c) },
                { PixFormat.ByteBGRA, (x, y, c) => new PixImage<byte>(Col.Format.BGRA, x, y, c) },
                { PixFormat.ByteRGBA, (x, y, c) => new PixImage<byte>(Col.Format.RGBA, x, y, c) },
                { PixFormat.ByteRGB, (x, y, c) => new PixImage<byte>(Col.Format.RGB, x, y, c) },
                { PixFormat.ByteBGRP, (x, y, c) => new PixImage<byte>(Col.Format.BGRP, x, y, c) },

                { PixFormat.SByteBW, (x, y, c) => new PixImage<sbyte>(Col.Format.BW, x, y, c) },
                { PixFormat.SByteGray, (x, y, c) => new PixImage<sbyte>(Col.Format.Gray, x, y, c) },
                { PixFormat.SByteBGR, (x, y, c) => new PixImage<sbyte>(Col.Format.BGR, x, y, c) },
                { PixFormat.SByteBGRA, (x, y, c) => new PixImage<sbyte>(Col.Format.BGRA, x, y, c) },
                { PixFormat.SByteRGBA, (x, y, c) => new PixImage<sbyte>(Col.Format.RGBA, x, y, c) },
                { PixFormat.SByteRGB, (x, y, c) => new PixImage<sbyte>(Col.Format.RGB, x, y, c) },
                { PixFormat.SByteBGRP, (x, y, c) => new PixImage<sbyte>(Col.Format.BGRP, x, y, c) },

                { PixFormat.UShortGray, (x, y, c) => new PixImage<ushort>(Col.Format.Gray, x, y, c) },
                { PixFormat.UShortRGB, (x, y, c) => new PixImage<ushort>(Col.Format.RGB, x, y, c) },
                { PixFormat.UShortRGBA, (x, y, c) => new PixImage<ushort>(Col.Format.RGBA, x, y, c) },
                { PixFormat.UShortRGBP, (x, y, c) => new PixImage<ushort>(Col.Format.RGBP, x, y, c) },

                { PixFormat.ShortGray, (x, y, c) => new PixImage<short>(Col.Format.Gray, x, y, c) },
                { PixFormat.ShortRGB, (x, y, c) => new PixImage<short>(Col.Format.RGB, x, y, c) },
                { PixFormat.ShortRGBA, (x, y, c) => new PixImage<short>(Col.Format.RGBA, x, y, c) },
                { PixFormat.ShortRGBP, (x, y, c) => new PixImage<short>(Col.Format.RGBP, x, y, c) },

                { PixFormat.UIntGray, (x, y, c) => new PixImage<uint>(Col.Format.Gray, x, y, c) },
                { PixFormat.UIntRGB, (x, y, c) => new PixImage<uint>(Col.Format.RGB, x, y, c) },
                { PixFormat.UIntRGBA, (x, y, c) => new PixImage<uint>(Col.Format.RGBA, x, y, c) },
                { PixFormat.UIntRGBP, (x, y, c) => new PixImage<uint>(Col.Format.RGBP, x, y, c) },

                { PixFormat.IntGray, (x, y, c) => new PixImage<int>(Col.Format.Gray, x, y, c) },
                { PixFormat.IntRGB, (x, y, c) => new PixImage<int>(Col.Format.RGB, x, y, c) },
                { PixFormat.IntRGBA, (x, y, c) => new PixImage<int>(Col.Format.RGBA, x, y, c) },
                { PixFormat.IntRGBP, (x, y, c) => new PixImage<int>(Col.Format.RGBP, x, y, c) },

                { PixFormat.HalfGray, (x, y, c) => new PixImage<Half>(Col.Format.Gray, x, y, c) },
                { PixFormat.HalfRGB, (x, y, c) => new PixImage<Half>(Col.Format.RGB, x, y, c) },
                { PixFormat.HalfRGBA, (x, y, c) => new PixImage<Half>(Col.Format.RGBA, x, y, c) },
                { PixFormat.HalfRGBP, (x, y, c) => new PixImage<Half>(Col.Format.RGBP, x, y, c) },

                { PixFormat.FloatGray, (x, y, c) => new PixImage<float>(Col.Format.Gray, x, y, c) },
                { PixFormat.FloatRGB, (x, y, c) => new PixImage<float>(Col.Format.RGB, x, y, c) },
                { PixFormat.FloatRGBA, (x, y, c) => new PixImage<float>(Col.Format.RGBA, x, y, c) },
                { PixFormat.FloatRGBP, (x, y, c) => new PixImage<float>(Col.Format.RGBP, x, y, c) },

                { PixFormat.DoubleGray, (x, y, c) => new PixImage<double>(Col.Format.Gray, x, y, c) },
                { PixFormat.DoubleRGB, (x, y, c) => new PixImage<double>(Col.Format.RGB, x, y, c) },
                { PixFormat.DoubleRGBA, (x, y, c) => new PixImage<double>(Col.Format.RGBA, x, y, c) },
                { PixFormat.DoubleRGBP, (x, y, c) => new PixImage<double>(Col.Format.RGBP, x, y, c) },
            };

        private static Dictionary<Type, Func<Array, Col.Format, long, long, long, PixImage>> s_pixImageArrayCreatorMap =
            new Dictionary<Type, Func<Array, Col.Format, long, long, long, PixImage>>()
            {
                { typeof(byte[]), (a, f, x, y, c) =>
                                    new PixImage<byte>(f, ((byte[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(sbyte[]), (a, f, x, y, c) =>
                                    new PixImage<sbyte>(f, ((sbyte[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(ushort[]), (a, f, x, y, c) =>
                                    new PixImage<ushort>(f, ((ushort[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(short[]), (a, f, x, y, c) =>
                                    new PixImage<short>(f, ((short[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(uint[]), (a, f, x, y, c) =>
                                    new PixImage<uint>(f, ((uint[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(int[]), (a, f, x, y, c) =>
                                    new PixImage<int>(f, ((int[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(Half[]), (a, f, x, y, c) =>
                                    new PixImage<Half>(f, ((Half[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(float[]), (a, f, x, y, c) =>
                                    new PixImage<float>(f, ((float[])a).CreateImageVolume(new V3l(x, y, c))) },
                { typeof(double[]), (a, f, x, y, c) =>
                                    new PixImage<double>(f, ((double[])a).CreateImageVolume(new V3l(x, y, c))) },
            };


        #endregion

        #region Static Creator Functions

        public static PixImage Create(PixFormat pixFormat, long sx, long sy, long ch)
            => s_pixImageCreatorMap[pixFormat](sx, sy, ch);

        public static PixImage Create(PixFormat pixFormat, long sx, long sy)
            => s_pixImageCreatorMap[pixFormat](sx, sy, pixFormat.Format.ChannelCount());

        public static PixImage Create(Array array, Col.Format format, long sx, long sy, long ch)
            => s_pixImageArrayCreatorMap[array.GetType()](array, format, sx, sy, ch);

        public static PixImage Create(Array array, Col.Format format, long sx, long sy)
            => s_pixImageArrayCreatorMap[array.GetType()](array, format, sx, sy, format.ChannelCount());

        public static Volume<T> CreateVolume<T>(V3i size) => size.ToV3l().CreateImageVolume<T>();

        public static Volume<T> CreateVolume<T>(V3l size) => size.CreateImageVolume<T>();

        public static Volume<T> CreateVolume<T>(long sizeX, long sizeY, long channelCount)
            => new V3l(sizeX, sizeY, channelCount).CreateImageVolume<T>();

        #endregion

        #region Load from file

        private static PixImage TryLoadFromFileWithLoader(IPixLoader loader, string filename)
            => TryInvokeLoader(
                    loader, l => l.LoadFromFile(filename), NotNull,
                    $"load image from file '{filename}'"
            );

        /// <summary>
        /// Loads an image from the given file without doing any conversions.
        /// </summary>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public static PixImage LoadRaw(string filename, IPixLoader loader = null)
            => InvokeLoaders(
                    loader, filename, (l, f) => TryLoadFromFileWithLoader(l, f), Ignore, NotNull,
                    $"Could not load image from file '{filename}'"
            );

        /// <summary>
        /// Loads an image from the given file.
        /// </summary>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public static PixImage Load(string filename, IPixLoader loader = null)
        {
            var loadImage = LoadRaw(filename, loader);
            return loadImage.ToPixImage(loadImage.Format);
        }

        /// <summary>
        /// Loads images from the given files.
        /// </summary>
        /// <param name="filenames">The image files to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>The loaded images.</returns>
        /// <exception cref="ImageLoadException">if an image could not be loaded.</exception>
        public static IEnumerable<PixImage> Load(IEnumerable<string> filenames, IPixLoader loader = null)
            => from file in filenames select Load(file, loader);

        [Obsolete("Use LoadRaw()")]
        public static PixImage CreateRaw(string filename, PixLoadOptions options = PixLoadOptions.Default)
            => LoadRaw(filename);

        [Obsolete("Use Load()")]
        public static PixImage Create(string filename, PixLoadOptions options = PixLoadOptions.Default)
            => Load(filename);

        [Obsolete("Use Load()")]
        public static IEnumerable<PixImage> Create(IEnumerable<string> filenames)
            => from file in filenames select Load(file);

        #endregion

        #region Load from stream

        private static PixImage TryLoadFromStreamWithLoader(IPixLoader loader, Stream stream)
            => TryInvokeLoader(
                    loader, l => l.LoadFromStream(stream), NotNull,
                    "load image from stream"
            );

        /// <summary>
        /// Loads an image from the given stream without doing any conversions.
        /// </summary>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public static PixImage LoadRaw(Stream stream, IPixLoader loader = null)
            => InvokeLoadersWithStream(
                loader, stream, (l, s) => TryLoadFromStreamWithLoader(l, s), NotNull,
                "Could not load image from stream"
            );

        /// <summary>
        /// Loads an image from the given stream.
        /// </summary>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public static PixImage Load(Stream stream, IPixLoader loader = null)
        {
            var loadImage = LoadRaw(stream, loader);
            return loadImage.ToPixImage(loadImage.Format);
        }

        [Obsolete("Use LoadRaw()")]
        public static PixImage CreateRaw(Stream stream, PixLoadOptions options = PixLoadOptions.Default)
            => LoadRaw(stream);

        [Obsolete("Use Load()")]
        public static PixImage Create(Stream stream, PixLoadOptions options = PixLoadOptions.Default)
            => Load(stream);

        #endregion

        #region Save to file

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
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
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

        private bool TrySaveToFileWithLoader(IPixLoader loader, string filename, PixSaveParams saveParams)
            => TryInvokeLoader(
                    loader, l => { l.SaveToFile(filename, this, saveParams); return true; }, Identity,
                    $"save image to file '{filename}'"
            );

        /// <summary>
        /// Saves the image to the given file.
        /// </summary>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="saveParams">The save parameters to use.</param>
        /// <param name="normalizeFilename">Indicates if the filename is to be normalized according to the image file format.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public void Save(string filename, PixSaveParams saveParams, bool normalizeFilename = true, IPixLoader loader = null)
        {
            if (normalizeFilename)
                filename = NormalizedFileName(filename, saveParams.Format);

            InvokeLoaders(
                loader, filename, (l, f) => TrySaveToFileWithLoader(l, f, saveParams), Ignore, Identity,
                $"Could not save image to file '{filename}'"
            );
        }

        /// <summary>
        /// Saves the image to the given file.
        /// </summary>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="fileFormat">The image format of the file.</param>
        /// <param name="normalizeFilename">Indicates if the filename is to be normalized according to the image file format.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public void Save(string filename, PixFileFormat fileFormat, bool normalizeFilename = true, IPixLoader loader = null)
            => Save(filename, new PixSaveParams(fileFormat), normalizeFilename, loader);

        /// <summary>
        /// Saves the image to the given file.
        /// The image file format is determined by the extension of the filename.
        /// </summary>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        /// <exception cref="ArgumentException">if the filename extension is missing or unknown.</exception>
        public void Save(string filename, IPixLoader loader = null)
            => Save(filename, GetFormatOfExtension(filename), false, loader);

        /// <summary>
        /// Saves the image to the given JPEG file.
        /// </summary>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="quality">The quality of the JPEG file. Must be within 0 - 100.</param>
        /// <param name="normalizeFilename">Indicates if the filename is to be normalized according to the image file format.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public void SaveAsJpeg(string filename, int quality = PixJpegSaveParams.DefaultQuality,
                               bool normalizeFilename = true, IPixLoader loader = null)
            => Save(filename, new PixJpegSaveParams(quality), normalizeFilename, loader);

        /// <summary>
        /// Saves the image to the given PNG file.
        /// </summary>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="compressionLevel">The compression level of the PNG file. Must be within 0 - 9.</param>
        /// <param name="normalizeFilename">Indicates if the filename is to be normalized according to the image file format.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public void SaveAsPng(string filename, int compressionLevel = PixPngSaveParams.DefaultCompressionLevel,
                              bool normalizeFilename = true,  IPixLoader loader = null)
            => Save(filename, new PixPngSaveParams(compressionLevel), normalizeFilename, loader);

        [Obsolete("Use Save() or SaveAsJpeg()")]
        public void SaveAsImage(string filename, PixFileFormat fileFormat, PixSaveOptions options = PixSaveOptions.Default, int qualityLevel = PixJpegSaveParams.DefaultQuality)
        {
            var normalizeFilename = ((options & PixSaveOptions.NormalizeFilename) != 0);

            if (fileFormat == PixFileFormat.Jpeg)
                SaveAsJpeg(filename, (qualityLevel == -1) ? PixJpegSaveParams.DefaultQuality : qualityLevel, normalizeFilename);
            else
                Save(filename, fileFormat, normalizeFilename);
        }

        /// <summary>
        /// Automatically detects file format from filename extension. 
        /// Throws exception if file name does not end in known format extension.
        /// </summary>
        [Obsolete("Use Save() or SaveAsJpeg()")]
        public void SaveAsImage(string filename)
            => Save(filename);

        #endregion

        #region Save to stream

        private bool TrySaveToStreamWithLoader(IPixLoader loader, Stream stream, PixSaveParams saveParams)
            => TryInvokeLoader(
                    loader, l => { l.SaveToStream(stream, this, saveParams); return true; }, Identity,
                    "save image to stream"
            );

        /// <summary>
        /// Saves the image to the given stream.
        /// </summary>
        /// <param name="stream">The stream to save the image to.</param>
        /// <param name="saveParams">The save parameters to use.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public void Save(Stream stream, PixSaveParams saveParams, IPixLoader loader = null)
            => InvokeLoadersWithStream(
                loader, stream, (l, s) => TrySaveToStreamWithLoader(l, s, saveParams), Identity,
                "Could not save image to stream"
            );

        /// <summary>
        /// Saves the image to the given stream.
        /// </summary>
        /// <param name="stream">The stream to save the image to.</param>
        /// <param name="fileFormat">The image format of the stream.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public void Save(Stream stream, PixFileFormat fileFormat, IPixLoader loader = null)
            => Save(stream, new PixSaveParams(fileFormat), loader);

        /// <summary>
        /// Saves the image to the given JPEG stream.
        /// </summary>
        /// <param name="stream">The stream to save the image to.</param>
        /// <param name="quality">The quality of the JPEG file. Must be within 0 - 100.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public void SaveAsJpeg(Stream stream, int quality = PixJpegSaveParams.DefaultQuality, IPixLoader loader = null)
            => Save(stream, new PixJpegSaveParams(quality), loader);

        /// <summary>
        /// Saves the image to the given PNG stream.
        /// </summary>
        /// <param name="stream">The file to save the image to.</param>
        /// <param name="compressionLevel">The compression level of the PNG file. Must be within 0 - 9.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public void SaveAsPng(Stream stream, int compressionLevel = PixPngSaveParams.DefaultCompressionLevel, IPixLoader loader = null)
            => Save(stream, new PixPngSaveParams(compressionLevel), loader);

        [Obsolete("Use overloads without PixSaveOptions")]
        public void SaveAsImage(Stream stream, PixFileFormat fileFormat, PixSaveOptions options = PixSaveOptions.Default, int qualityLevel = PixJpegSaveParams.DefaultQuality)
        {
            if (fileFormat == PixFileFormat.Jpeg)
                SaveAsJpeg(stream, qualityLevel);
            else
                Save(stream, fileFormat);
        }

        /// <summary>
        /// Writes the image to a memory stream.
        /// </summary>
        /// <param name="saveParams">The save parameters to use.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A memory stream containing the image data.</returns>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public MemoryStream ToMemoryStream(PixSaveParams saveParams, IPixLoader loader = null)
        {
            var stream = new MemoryStream();
            Save(stream, saveParams, loader);
            return stream;
        }

        /// <summary>
        /// Writes the image to a memory stream.
        /// </summary>
        /// <param name="fileFormat">The image format of the stream.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A memory stream containing the image data.</returns>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public MemoryStream ToMemoryStream(PixFileFormat fileFormat, IPixLoader loader = null)
            => ToMemoryStream(new PixSaveParams(fileFormat), loader);

        /// <summary>
        /// Writes the image to a JPEG memory stream.
        /// </summary>
        /// <param name="quality">The quality of the JPEG file. Must be within 0 - 100.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A memory stream containing the image data.</returns>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public MemoryStream ToMemoryStreamAsJpeg(int quality = PixJpegSaveParams.DefaultQuality, IPixLoader loader = null)
            => ToMemoryStream(new PixJpegSaveParams(quality), loader);

        /// <summary>
        /// Writes the image to a PNG memory stream.
        /// </summary>
        /// <param name="compressionLevel">The compression level of the PNG file. Must be within 0 - 9.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A memory stream containing the image data.</returns>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public MemoryStream ToMemoryStreamAsPng(int compressionLevel = PixPngSaveParams.DefaultCompressionLevel, IPixLoader loader = null)
            => ToMemoryStream(new PixJpegSaveParams(compressionLevel), loader);

        [Obsolete("Use overloads without PixSaveOptions")]
        public MemoryStream ToMemoryStream(PixFileFormat fileFormat, PixSaveOptions options, int qualityLevel)
        {
            if (fileFormat == PixFileFormat.Jpeg)
                return ToMemoryStreamAsJpeg(qualityLevel);
            else
                return ToMemoryStream(fileFormat);
        }

        [Obsolete("Use ToMemoryStreamAsJpeg")]
        public MemoryStream ToMemoryStream(int qualityLevel)
            => ToMemoryStreamAsJpeg(qualityLevel);

        #endregion

        #region Query info from file

        private static PixImageInfo TryGetInfoFromFileWithLoader(IPixLoader loader, string filename)
            => TryInvokeLoader(
                    loader, l => l.GetInfoFromFile(filename), NotNull,
                    $"get image info from file '{filename}'"
            );

        /// <summary>
        /// Loads basic information about an image from a file without loading its content.
        /// </summary>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A PixImageInfo instance containing basic information about the image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public static PixImageInfo GetInfoFromFile(string filename, IPixLoader loader = null)
            => InvokeLoaders(
                    loader, filename, (l, f) => TryGetInfoFromFileWithLoader(l, f), Ignore, NotNull,
                    $"Could not get image info from file '{filename}'"
            );

        [Obsolete("Use GetInfoFromFile()")]
        public static PixImageInfo InfoFromFileName(string fileName, PixLoadOptions options = PixLoadOptions.Default)
            => GetInfoFromFile(fileName);

        #endregion

        #region Query info from stream

        private static PixImageInfo TryGetInfoFromStreamWithLoader(IPixLoader loader, Stream stream)
            => TryInvokeLoader(
                    loader, l => l.GetInfoFromStream(stream), NotNull,
                    "get image info from stream"
            );

        /// <summary>
        /// Loads basic information about an image from a stream without loading its content.
        /// </summary>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A PixImageInfo instance containing basic information about the image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public static PixImageInfo GetInfoFromStream(Stream stream, IPixLoader loader = null)
            => InvokeLoadersWithStream(
                    loader, stream, (l, s) => TryGetInfoFromStreamWithLoader(l, s), NotNull,
                    $"Could not get image info from stream"
            );

        #endregion

        #region Conversions

        protected static Dictionary<(Type, Type), Func<object, object>> 
            s_copyFunMap =
            new Dictionary<(Type, Type), Func<object, object>>()
            {
                { (typeof(byte), typeof(byte)), v => ((Volume<byte>)v).CopyWindow() },
                { (typeof(byte), typeof(ushort)), v => ((Volume<byte>)v).ToUShortColor() },
                { (typeof(byte), typeof(uint)), v => ((Volume<byte>)v).ToUIntColor() },
                { (typeof(byte), typeof(float)), v => ((Volume<byte>)v).ToFloatColor() },
                { (typeof(byte), typeof(double)), v => ((Volume<byte>)v).ToDoubleColor() },

                { (typeof(ushort), typeof(byte)), v => ((Volume<ushort>)v).ToByteColor() },
                { (typeof(ushort), typeof(ushort)), v => ((Volume<ushort>)v).CopyWindow() },
                { (typeof(ushort), typeof(uint)), v => ((Volume<ushort>)v).ToUIntColor() },
                { (typeof(ushort), typeof(float)), v => ((Volume<ushort>)v).ToFloatColor() },
                { (typeof(ushort), typeof(double)), v => ((Volume<ushort>)v).ToDoubleColor() },

                { (typeof(uint), typeof(byte)), v => ((Volume<uint>)v).ToByteColor() },
                { (typeof(uint), typeof(ushort)), v => ((Volume<uint>)v).ToUShortColor() },
                { (typeof(uint), typeof(uint)), v => ((Volume<uint>)v).CopyWindow() },
                { (typeof(uint), typeof(float)), v => ((Volume<uint>)v).ToFloatColor() },
                { (typeof(uint), typeof(double)), v => ((Volume<uint>)v).ToDoubleColor() },

                { (typeof(float), typeof(byte)), v => ((Volume<float>)v).ToByteColor() },
                { (typeof(float), typeof(ushort)), v => ((Volume<float>)v).ToUShortColor() },
                { (typeof(float), typeof(uint)), v => ((Volume<float>)v).ToUIntColor() },
                { (typeof(float), typeof(float)), v => ((Volume<float>)v).CopyWindow() },
                { (typeof(float), typeof(double)), v => ((Volume<float>)v).ToDoubleColor() },

                { (typeof(double), typeof(byte)), v => ((Volume<double>)v).ToByteColor() },
                { (typeof(double), typeof(ushort)), v => ((Volume<double>)v).ToUShortColor() },
                { (typeof(double), typeof(uint)), v => ((Volume<double>)v).ToUIntColor() },
                { (typeof(double), typeof(float)), v => ((Volume<double>)v).ToFloatColor() },
                { (typeof(double), typeof(double)), v => ((Volume<double>)v).CopyWindow() },
            };

        public abstract PixImage<T1> ToPixImage<T1>();
        public abstract PixImage Transformed(ImageTrafo trafo);

        public PixImage<T> AsPixImage<T>() => this as PixImage<T>;

        public PixImage<T> ToPixImage<T>(Col.Format format)
        {
            if (this is PixImage<T> castImage && castImage.Format == format && castImage.ChannelCount == format.ChannelCount())
                return castImage;
            return new PixImage<T>(format, this);
        }

        #endregion

        #region Abstract Methods

        public abstract PixFormat PixFormat { get; }

        public abstract VolumeInfo VolumeInfo { get; }
        
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

        public abstract T Visit<T>(IPixImageVisitor<T> visitor);
        
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
        /// Compress BW images from 8 bits per byte to 1 bit per byte.
        /// </summary>
        public static void CompressPixels(PixImage<byte> pixImage, PixImage<byte> bitImage)
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
        /// Expand BW images from 1 bit per byte to 8 bits per byte.
        /// </summary>
        public static void ExpandPixels(PixImage<byte> bitImage, PixImage<byte> pixImage)
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

        #endregion

        #region IPix

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixImage(this); }

        #endregion
    }

    /// <summary>
    /// The generic PixImage stores an image with a specific channel type that
    /// is specified as type parameter.
    /// </summary>
    [Serializable]
    public partial class PixImage<T> : PixImage //, IPixImage2d
    {
        public Volume<T> Volume;

        #region Constructors

        public PixImage(Col.Format format, Volume<T> volume)
            : base(format)
        {
            Volume = volume;
        }

        public PixImage() { }

        public PixImage(Volume<T> volume)
            : this(Col.FormatDefaultOf(typeof(T), volume.SZ), volume)
        { }

        public PixImage(V2i size, long channelCount)
            : this(Col.FormatDefaultOf(typeof(T), channelCount),
                   CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(V2l size, long channelCount)
            : this(Col.FormatDefaultOf(typeof(T), channelCount),
                   CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(long sizeX, long sizeY, long channelCount)
            : this(Col.FormatDefaultOf(typeof(T), channelCount),
                   CreateVolume<T>(sizeX, sizeY, channelCount))
        { }

        public PixImage(Col.Format format, V2i size)
            : this(format, CreateVolume<T>(size.X, size.Y, format.ChannelCount()))
        { }

        public PixImage(Col.Format format, V2l size)
            : this(format, CreateVolume<T>(size.X, size.Y, format.ChannelCount()))
        { }

        public PixImage(Col.Format format, long sizeX, long sizeY)
            : this(format, CreateVolume<T>(sizeX, sizeY, format.ChannelCount()))
        { }

        public PixImage(Col.Format format, V2i size, long channelCount)
            : this(format, CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(Col.Format format, V2l size, long channelCount)
            : this(format, CreateVolume<T>(size.X, size.Y, channelCount))
        { }

        public PixImage(Col.Format format, long sizeX, long sizeY, long channelCount)
            : this(format, CreateVolume<T>(sizeX, sizeY, channelCount))
        { }

        public PixImage(PixImageInfo info)
            : this(info.Format.Format, info.Size)
        {
            if (info.Format.Type != typeof(T)) throw new Exception("Attempt to create PixImage from PixImageInfo with different Type T.");
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
        
        public PixImage<T> CopyToImageLayout()
        {
            if (Volume.HasImageLayout())
                return new PixImage<T>(Format, Volume.CopyToImage());
            return new PixImage<T>(this);
        }

        public PixImage<T> Copy() => new PixImage<T>(Format, Volume.CopyToImageWindow());

        public override PixImage CopyToPixImage() => Copy();

        public override PixImage CopyToPixImageWithCanonicalDenseLayout() => CopyToImageLayout();

        public override PixImage ToCanonicalDenseLayout() => ToImageLayout();

        /// <summary>
        /// Copy function for color conversions.
        /// </summary>
        /// <typeparam name="Tv"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> fun) => Copy<Tv>(fun, Format);

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
            var mat = GetMatrix<Tv>().MapWindow(fun);
            var vol = new Volume<T>(mat.Data, Volume.Info);
            return new PixImage<T>(format, vol);
        }

        #endregion

        #region Constructors from File / Stream

        /// <summary>
        /// Loads an image from the given file.
        /// </summary>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public PixImage(string filename, IPixLoader loader = null)
        {
            var loadImage = LoadRaw(filename, loader);
            var channelCount = loadImage.Format.ChannelCount();

            if (!(loadImage is PixImage<T> image) || image.ChannelCount != channelCount)
                image = new PixImage<T>(loadImage);

            Volume = image.Volume;
            Format = image.Format;
        }

        /// <summary>
        /// Loads an image from the given stream.
        /// </summary>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public PixImage(Stream stream, IPixLoader loader = null)
        {
            var loadImage = LoadRaw(stream, loader);
            var channelCount = loadImage.Format.ChannelCount();

            if (!(loadImage is PixImage<T> image) || image.ChannelCount != channelCount)
                image = new PixImage<T>(loadImage);

            Volume = image.Volume;
            Format = image.Format;
        }

        [Obsolete("Use overload without PixLoadOptions")]
        public PixImage(string filename, PixLoadOptions options) : this(filename) { }

        [Obsolete("Use overload without PixLoadOptions")]
        public PixImage(Stream stream, PixLoadOptions options) : this(stream) { }

        #endregion

        #region Static Creator Functions

        public static PixImage<T> Create(params Matrix<T>[] channels)
            => new PixImage<T>(channels);

        /// <summary>
        /// Create a pixel image from all the given channels in the specified format.
        /// Note, that the channels have to be supplied in the canonical order: red,
        /// green, blue, (alpha).
        /// <param name="format">the color format</param>
        /// <param name="channels">channel matrices</param>
        /// </summary>
        public static PixImage<T> Create(Col.Format format, params Matrix<T>[] channels)
            => new PixImage<T>(format, channels);

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

        public override VolumeInfo VolumeInfo => Volume.Info;
        
        public override V2i Size => (V2i)Volume.Info.Size.XY;

        public override V2l SizeL => Volume.Info.Size.XY;

        public override int ChannelCount => (int)Volume.Info.Size.Z;

        public override long ChannelCountL => Volume.Info.Size.Z;

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
        public Matrix<T>[] ChannelArray => Channels.ToArray();

        public int BytesPerChannel => System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

        public override Array Array => Volume.Data;

        public override int IntStride => BytesPerChannel * (int)Volume.DY;

        /// <summary>
        /// Returns the matrix representation of the volume if there is only
        /// one channel. Fails if there are multiple channels.
        /// </summary>
        public Matrix<T> Matrix => Volume.AsMatrixWindow();

        #endregion

        #region Image Manipulation

        public override PixImage RemappedPixImage(
            Matrix<float> xMap, Matrix<float> yMap,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Remapped(xMap, xMap, ip);

        public PixImage<T> Remapped(
            Matrix<float> xMap, Matrix<float> yMap,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => new PixImage<T>(Format, s_remappedFun(Volume, xMap, yMap, ip));

        private static Func<Volume<T>, Matrix<float>, Matrix<float>, ImageInterpolation, Volume<T>> s_remappedFun = null;

        public static void SetRemappedFun(
            Func<Volume<T>, Matrix<float>, Matrix<float>, ImageInterpolation, Volume<T>> remappedFun
            )
        {
            s_remappedFun = remappedFun;
        }

        public override PixImage ResizedPixImage(
            V2i newSize,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Scaled((V2d)newSize / (V2d)Size, ip);

        public PixImage<T> Resized(
            V2i newSize,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Scaled((V2d)newSize / (V2d)Size, ip);

        public PixImage<T> Resized(
            int xSize, int ySize,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Scaled(new V2d(xSize, ySize) / (V2d)Size, ip);

        public override PixImage RotatedPixImage(
            double angleInRadiansCCW, bool resize = true,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Rotated(angleInRadiansCCW, resize, ip);

        public PixImage<T> Rotated(
            double angleInRadiansCCW, bool resize = true,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => new PixImage<T>(Format, s_rotatedFun(Volume, angleInRadiansCCW, resize, ip));

        private static Func<Volume<T>, double, bool, ImageInterpolation, Volume<T>> s_rotatedFun = null;

        public static void SetRotatedFun(
            Func<Volume<T>, double, bool, ImageInterpolation, Volume<T>> rotatedFun)
        {
            s_rotatedFun = rotatedFun;
        }

        public override PixImage ScaledPixImage(
            V2d scaleFactor,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Scaled(scaleFactor, ip);

        public PixImage<T> Scaled(
            V2d scaleFactor,
            ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            if (!(scaleFactor.X > 0.0 && scaleFactor.Y > 0.0)) throw new ArgumentOutOfRangeException(nameof(scaleFactor));

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
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Scaled(new V2d(scaleFactor, scaleFactor), ip);

        public PixImage<T> Scaled(
            double xScaleFactor, double yScaleFactor,
            ImageInterpolation ip = ImageInterpolation.Cubic
            ) => Scaled(new V2d(xScaleFactor, yScaleFactor), ip);

        #endregion

        #region SubImages

        /// <summary>
        /// Returns a specified region as new PixImage.
        /// No data is copied, the internal Volumue is only a view on the original one.
        /// </summary>
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
        /// No data is copied, the internal Volumue is only a view on the original one.
        /// </summary>
        public PixImage<T> SubImage(V2i pos, V2i size) => SubImage(pos.X, pos.Y, size.X, size.Y);

        /// <summary>
        /// Returns a specified region as new PixImage.
        /// No data is copied, the internal Volumue is only a view on the original one.
        /// </summary>
        public PixImage<T> SubImage(V2l pos, V2l size) => SubImage(pos.X, pos.Y, size.X, size.Y);

        /// <summary>
        /// Returns a specified region as new PixImage.
        /// No data is copied, the internal Volumue is only a view on the original one.
        /// </summary>
        public PixImage<T> SubImage(Box2i box) => SubImage(box.Min, box.Size);

        /// <summary>
        /// Returns a square region around the center as new PixImage.
        /// SquareRadius is the given radius around the center.
        /// No data is copied, the internal Volumue is only a view on the original one.
        /// </summary>
        public PixImage<T> SubImage(V2i center, int squareRadius)
            => SubImage(center - squareRadius, V2i.II * squareRadius * 2 + 1);

        /// <summary>
        /// Returns a specified region as a new PixImage.
        /// The supplied pos and size are rounded to the nearest integer.
        /// Note that this may be different from SubImage(Box2d box), since
        /// rounding the size and rounding the Max of the box may
        /// result in different integer sizes.
        /// No data is copied, the internal Volumue is only a view on the original one.
        /// </summary>
        public PixImage<T> SubImage(V2d pos, V2d size) => SubImage(new V2i(pos + 0.5), new V2i(size + 0.5));

        /// <summary>
        /// Returns a specified region as a new PixImage.
        /// Min and Max of the supplied box are rounded to the nearest integer.
        /// Note that this may be different from SubImage(V2d pos, V2d size),
        /// since rounding the Max of the box and rounding the size may
        /// result in different integer sizes.
        /// No data is copied, the internal Volumue is only a view on the original one.
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
        /// No data is copied, the internal Volumue is only a view on the original one.
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
        /// No data is copied, the internal Volumue is only a view on the original one.
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
        public void Set(int x, int y, PixImage<T> img) => Set(new V2i(x, y), img);

        /// <summary>
        /// Set a subregion of the image to the contents of another image.
        /// The size of the subregion is dtermined by the other image.
        /// </summary>
        public void Set(V2i pos, PixImage<T> image) => SubImage(pos, image.Size).Set(image);

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

        public PixImage<T> ToImageLayout() => !Volume.HasImageLayout() ? new PixImage<T>(Format, this) : this;

        public PixImage<T> ToFormat(Col.Format format) => Format == format ? this : new PixImage<T>(format, this);

        public override PixImage<T1> ToPixImage<T1>()
        {
            var castImage = this as PixImage<T1>;
            if (castImage != null) return castImage;
            var format = typeof(T1).FormatDefaultOf(ChannelCount);
            if (Format == format)
            {
                var copy = s_copyFunMap[(typeof(T), typeof(T1))];
                return new PixImage<T1>(format, (Volume<T1>)copy(Volume));
            }
            return new PixImage<T1>(this);
        }

        public override PixImage Transformed(ImageTrafo trafo)
            => new PixImage<T>(Format, Volume.Transformed(trafo));

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
            => GetChannelInFormatOrder(Format.ChannelIndex(channel));

        /// <summary>
        /// Returns the specified channel (based on the canonical order) with
        /// a different view type.
        /// </summary>
        public Matrix<T, Tv> GetChannel<Tv>(long channelIndex)
            => GetChannelInFormatOrder<Tv>(Format.ChannelOrder()[channelIndex]);

        /// <summary>
        /// Returns the specified channel based on the order of the image's
        /// color format.
        /// </summary>
        public Matrix<T> GetChannelInFormatOrder(long formatChannelIndex)
            => Volume.SubXYMatrixWindow(formatChannelIndex);

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
            
        public Matrix<T, Tv> GetMatrix<Tv>() => Volume.GetMatrix<T, Tv>(Format.GetIntent());

        #endregion

        #region Concrete Implementation Of Abstract Functions

        public override PixFormat PixFormat => new PixFormat(typeof(T), Format);

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

        public override Array Data => Volume.HasImageLayout() ? Volume.Data : throw new ArgumentException(nameof(Volume));

        public override TResult Visit<TResult>(IPixImageVisitor<TResult> visitor) => visitor.Visit<T>(this);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
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

    public interface IPixImageVisitor<T>
    {
        T Visit<TData>(PixImage<TData> image);
    }

    public record PixImageInfo(PixFormat Format, V2i Size);

    [Serializable]
    public abstract partial class PixImage : IPix
    {
        public Col.Format Format;

        #region Loaders

        #region PgmPixLoader

        /// <summary>
        /// Loader for saving PGM images. Reading is not supported.
        /// </summary>
        private class PgmPixLoader : IPixLoader
        {
            public string Name { get; }

            public bool CanEncode => true;

            public bool CanDecode => false;

            public PgmPixLoader()
            {
                Name = "Aardvark PGM";
            }

            public PixImage LoadFromFile(string filename) => null;

            public PixImage LoadFromStream(Stream stream) => null;

            public void SaveToFile(string filename, PixImage image, PixSaveParams saveParams)
            {
                using var stream = File.OpenWrite(filename);
                SaveToStream(stream, image, saveParams);
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

            public PixImageInfo GetInfoFromFile(string filename) => null;

            public PixImageInfo GetInfoFromStream(Stream stream) => null;
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
        /// If the loader already exists, the priority is modified.
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
                var list = s_loaders.ToList();
                list.Sort((x, y) => y.Value - x.Value);
                return list.Map(x => x.Key);
            }
        }

        /// <summary>
        /// Gets a list of registered loaders with encoding support sorted by priority in descending order.
        /// </summary>
        /// <returns>A list of registered loaders that support encoding.</returns>
        public static List<IPixLoader> GetEncoders()
        {
            var list = GetLoaders();
            list.RemoveAll(loader => !loader.CanEncode);
            return list;
        }

        /// <summary>
        /// Gets a list of registered loaders with decoding support sorted by priority in descending order.
        /// </summary>
        /// <returns>A list of registered loaders that support decoding.</returns>
        public static List<IPixLoader> GetDecoders()
        {
            var list = GetLoaders();
            list.RemoveAll(loader => !loader.CanDecode);
            return list;
        }

        internal static Result TryInvokeLoader<Result>(
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

        internal enum LoaderType { Encoder, Decoder, Any };

        internal static Result InvokeLoaders<Input, Result>(
                                LoaderType loaderType, IPixLoader loader, Input input,
                                Func<IPixLoader, Input, Result> tryInvoke,
                                Action<Input> resetInput,
                                Func<Result, bool> isValid,
                                string errorMessage)
        {
            if (loader != null)
            {
                if (loaderType == LoaderType.Encoder && !loader.CanEncode)
                    throw new ImageLoadException(errorMessage + " - Encoding not supported.");

                if (loaderType == LoaderType.Decoder && !loader.CanDecode)
                    throw new ImageLoadException(errorMessage + " - Decoding not supported.");

                var result = tryInvoke(loader, input);
                if (isValid(result)) { return result; }
            }
            else
            {
                var loaders =
                    loaderType switch
                    {
                        LoaderType.Encoder => GetEncoders(),
                        LoaderType.Decoder => GetDecoders(),
                        _ => GetLoaders()
                    };

                for (int i = 0; i < loaders.Count; i++)
                {
                    if (i != 0) resetInput(input);

                    var result = tryInvoke(loaders[i], input);
                    if (isValid(result)) return result;
                }

                var loaderDesc =
                    loaderType switch
                    {
                        LoaderType.Encoder => "encoder",
                        LoaderType.Decoder => "decoder",
                        _ => "loader"
                    };

                if (loaders.Count == 0)
                {
                    errorMessage += $" - No {loaderDesc}s available!";
                }
                else
                {
                    errorMessage += $" - Available {loaderDesc}s:" + Environment.NewLine;

                    foreach (var l in loaders)
                    {
                        errorMessage += "    - " + l.Name + Environment.NewLine;
                    }
                }
            }

            throw new ImageLoadException(errorMessage);
        }

        internal static Result InvokeLoadersWithStream<Result>(
                        LoaderType loaderType, IPixLoader loader, Stream stream,
                        Func<IPixLoader, Stream, Result> tryInvoke,
                        Func<Result, bool> isValid,
                        string errorMessage)
        {
            var initialPosition = stream.Position;

            return InvokeLoaders(
                loaderType, loader, stream, tryInvoke,
                s => s.Seek(initialPosition, SeekOrigin.Begin),
                isValid, errorMessage
            );
        }

        internal static void Ignore<T>(T _) { }

        internal static bool NotNull<T>(T x) => x != null;

        internal static bool Identity(bool x) => x;

        #endregion

        #region Processors

        private static readonly Dictionary<IPixProcessor, int> s_processors = new()
        {
            { PixProcessor.Instance, 1 }
        };

        /// <summary>
        /// Sets the priority of a PixImage processor.
        /// The priority determines the order in which proocessors are invoked to scale, rotate, or remap an image.
        /// Processors with higher priority are invoked first.
        /// If the processor does not exist, it is added with the given priority.
        /// </summary>
        /// <param name="processor">The processor to modify.</param>
        /// <param name="priority">The priority to set.</param>
        public static void SetProcessor(IPixProcessor processor, int priority)
        {
            lock (s_processors)
            {
                s_processors[processor] = priority;
            }
        }

        /// <summary>
        /// Adds a PixImage processor.
        /// Assigns a priority that is greater than the highest priority among existing processors, resulting in a LIFO order.
        /// If the processor already exists, the priority is modified.
        /// </summary>
        /// <param name="processor">The processor to add.</param>
        public static void AddProcessor(IPixProcessor processor)
        {
            lock (s_processors)
            {
                var maxPriority = s_processors.Values.Max(-1);
                s_processors[processor] = maxPriority + 1;
            }
        }

        /// <summary>
        /// Removes a PixImage processor.
        /// </summary>
        /// <param name="processor">The processor to remove.</param>
        public static void RemoveProcessor(IPixProcessor processor)
        {
            lock (s_processors) { s_processors.Remove(processor); }
        }

        /// <summary>
        /// Gets a dictionary of registered processors with their associated priority.
        /// Only returns processors that have at least the given minimum capabilities.
        /// </summary>
        /// <param name="minCapabilities">The minimum capabilities for a processor to be considered.</param>
        /// <returns>A dictionary of registered processors.</returns>
        public static Dictionary<IPixProcessor, int> GetProcessorsWithPriority(PixProcessorCaps minCapabilities = PixProcessorCaps.None)
        {
            lock (s_processors)
            {
                var result = new Dictionary<IPixProcessor, int>();

                foreach (var p in s_processors)
                {
                    if (p.Key.Capabilities.HasFlag(minCapabilities))
                        result[p.Key] = p.Value;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets a list of registered processors sorted by priority in descending order.
        /// Only returns processors that have at least the given minimum capabilities.
        /// </summary>
        /// <param name="minCapabilities">The minimum capabilities for a processor to be considered.</param>
        /// <returns>A list of registered processors.</returns>
        public static List<IPixProcessor> GetProcessors(PixProcessorCaps minCapabilities = PixProcessorCaps.None)
        {
            lock (s_processors)
            {
                var list = new List<KeyValuePair<IPixProcessor, int>>();

                foreach (var p in s_processors)
                {
                    if (p.Key.Capabilities.HasFlag(minCapabilities))
                        list.Add(p);
                }

                list.Sort((x, y) => y.Value - x.Value);
                return list.Map(x => x.Key);
            }
        }

        internal static PixImage<T> InvokeProcessors<T>(
                        Func<IPixProcessor, PixImage<T>> invoke,
                        PixProcessorCaps minCapabilities,
                        string operationDescription)
        {
            PixImage<T> result;

            foreach (var p in GetProcessors(minCapabilities))
            {
                try
                {
                    result = invoke(p);
                    if (result != null) return result;
                }
                catch (Exception e)
                {
                    Report.Warn($"Failed to {operationDescription} with {p.Name} image processor: {e.Message}");
                }
            }

            var processors = GetProcessors(PixProcessorCaps.None);
            var errorMessage = $"Cannot {operationDescription}";

            if (processors.Count == 0)
            {
                errorMessage += ", no image processors available!";
            }
            else
            {
                errorMessage += ", available image processors:" + Environment.NewLine;

                foreach (var p in processors)
                {
                    errorMessage += $"    - {p.Name}: {p.Capabilities}" + Environment.NewLine;
                }
            }

            throw new NotSupportedException(errorMessage);
        }

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

        #region Properties

        public abstract Array Array { get; }

        public abstract PixFormat PixFormat { get; }

        public abstract VolumeInfo VolumeInfo { get; }

        public abstract int BytesPerChannel { get; }

        public PixImageInfo Info => new PixImageInfo(PixFormat, Size);

        /// <summary>
        /// Size.X * Size.Y.
        /// </summary>
        public int NumberOfPixels => Size.X * Size.Y;

        /// <summary>
        /// Width/height.
        /// </summary>
        public double AspectRatio => Size.X / (double)Size.Y;

        public V2i Size => (V2i)VolumeInfo.Size.XY;

        public V2l SizeL => VolumeInfo.Size.XY;

        public int Width => Size.X;

        public long WidthL => SizeL.X;

        public int Height => Size.Y;

        public long HeightL => SizeL.Y;

        public int ChannelCount => (int)VolumeInfo.Size.Z;

        public long ChannelCountL => VolumeInfo.Size.Z;

        public int Stride => BytesPerChannel * (int)VolumeInfo.DY;

        public long StrideL => BytesPerChannel * VolumeInfo.DY;

        #region Obsolete

        [Obsolete("Use Stride instead.")]
        public int IntStride => Stride;

        #endregion

        #endregion

        #region Static Tables and Methods

        #region Format file extensions

        private static readonly Dictionary<string, PixFileFormat> s_formatOfExtension = new()
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

        private static readonly Lazy<Dictionary<PixFileFormat, string>> s_preferredExtensionOfFormat = new(() =>
            {
                var result = new Dictionary<PixFileFormat, string>();
                foreach (var kvp in s_formatOfExtension)
                {
                    result[kvp.Value] = kvp.Key;
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

        #endregion

        #region Grayscale conversion

        private static void ToGray<T, Tv, R>(PixImage src, object dst, Func<Tv, R> toGray)
        {
            ((Matrix<R>)dst).SetMap(src.AsPixImage<T>().GetMatrix<Tv>(), toGray);
        }

        protected static readonly Dictionary<(Type, Type), Action<PixImage, object>> s_rgbToGrayMap = new()
            {
                { (typeof(byte), typeof(byte)),     (src, dst) => ToGray<byte, C3b, byte>(src, dst, Col.ToGrayByte) },
                { (typeof(ushort), typeof(ushort)), (src, dst) => ToGray<ushort, C3us, ushort>(src, dst, Col.ToGrayUShort) },
                { (typeof(uint), typeof(uint)),     (src, dst) => ToGray<uint, C3ui, uint>(src, dst, Col.ToGrayUInt) },
                { (typeof(float), typeof(float)),   (src, dst) => ToGray<float, C3f, float>(src, dst, Col.ToGrayFloat) },
                { (typeof(double), typeof(double)), (src, dst) => ToGray<double, C3d, double>(src, dst, Col.ToGrayDouble) },
            };

        #endregion

        #region Create dispatcher

        // Helper class to create PixImage from given Type
        private static class Dispatch
        {
            private delegate PixImage CreateDelegate(Col.Format format, long sizeX, long sizeY, long channels);
            private delegate PixImage CreateArrayDelegate(Array data, Col.Format format, long sizeX, long sizeY, long channels);

            private static class CreateDispatcher
            {
                public static PixImage Create<T>(Col.Format format, long sizeX, long sizeY, long channels)
                    => new PixImage<T>(format, sizeX, sizeY, channels);

                public static PixImage CreateArray<T>(Array data, Col.Format format, long sizeX, long sizeY, long channels)
                    => new PixImage<T>(format, ((T[])data).CreateImageVolume(new V3l(sizeX, sizeY, channels)));
            }

            private const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            private static readonly MethodInfo s_createMethod = typeof(CreateDispatcher).GetMethod(nameof(CreateDispatcher.Create), flags);
            private static readonly ConcurrentDictionary<Type, CreateDelegate> s_createDelegates = new();

            private static readonly MethodInfo s_createArrayMethod = typeof(CreateDispatcher).GetMethod(nameof(CreateDispatcher.CreateArray), flags);
            private static readonly ConcurrentDictionary<Type, CreateArrayDelegate> s_createArrayDelegates = new();

            public static PixImage Create(PixFormat format, long sizeX, long sizeY, long channels)
            {
                var create = s_createDelegates.GetOrAdd(format.Type, t => {
                    var mi = s_createMethod.MakeGenericMethod(t);
                    return (CreateDelegate)Delegate.CreateDelegate(typeof(CreateDelegate), mi);
                });

                return create(format.Format, sizeX, sizeY, channels);
            }

            public static PixImage Create(Array array, Col.Format format, long sizeX, long sizeY, long channels)
            {
                var create = s_createArrayDelegates.GetOrAdd(array.GetType().GetElementType(), t => {
                    var mi = s_createArrayMethod.MakeGenericMethod(t);
                    return (CreateArrayDelegate)Delegate.CreateDelegate(typeof(CreateArrayDelegate), mi);
                });

                return create(array, format, sizeX, sizeY, channels);
            }
        }

        #endregion

        #endregion

        #region Static Creator Functions

        public static PixImage Create(PixFormat format, long sizeX, long sizeY, long channelCount)
            => Dispatch.Create(format, sizeX, sizeY, channelCount);

        public static PixImage Create(PixFormat format, long sizeX, long sizeY)
            => Dispatch.Create(format, sizeX, sizeY, format.ChannelCount);

        public static PixImage Create(Array array, Col.Format format, long sizeX, long sizeY, long channelCount)
            => Dispatch.Create(array, format, sizeX, sizeY, channelCount);

        public static PixImage Create(Array array, Col.Format format, long sizeX, long sizeY)
            => Dispatch.Create(array, format, sizeX, sizeY, format.ChannelCount());

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
                    LoaderType.Decoder, loader, filename, (l, f) => TryLoadFromFileWithLoader(l, f), Ignore, NotNull,
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

        #endregion

        #region Load from stream

        internal static string GetStreamDescription(Stream stream)
            => (stream is FileStream fs) ? $"file stream '{fs.Name}'" : "stream";

        private static PixImage TryLoadFromStreamWithLoader(IPixLoader loader, Stream stream)
            => TryInvokeLoader(
                    loader, l => l.LoadFromStream(stream), NotNull,
                    $"load image from {GetStreamDescription(stream)}"
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
                LoaderType.Decoder, loader, stream, (l, s) => TryLoadFromStreamWithLoader(l, s), NotNull,
                $"Could not load image from {GetStreamDescription(stream)}"
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
                LoaderType.Encoder, loader, filename, (l, f) => TrySaveToFileWithLoader(l, f, saveParams), Ignore, Identity,
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
                              bool normalizeFilename = true, IPixLoader loader = null)
            => Save(filename, new PixPngSaveParams(compressionLevel), normalizeFilename, loader);

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
                LoaderType.Encoder, loader, stream, (l, s) => TrySaveToStreamWithLoader(l, s, saveParams), Identity,
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
                    LoaderType.Any, loader, filename, (l, f) => TryGetInfoFromFileWithLoader(l, f), Ignore, NotNull,
                    $"Could not get image info from file '{filename}'"
            );

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
                    LoaderType.Any, loader, stream, (l, s) => TryGetInfoFromStreamWithLoader(l, s), NotNull,
                    $"Could not get image info from stream"
            );

        #endregion

        #region Conversions

        public PixImage<T> AsPixImage<T>() => this as PixImage<T>;

        public PixImage<T1> ToPixImage<T1>() => AsPixImage<T1>() ?? new PixImage<T1>(this);

        public abstract PixImage ToPixImage(Col.Format format);

        public PixImage<T> ToPixImage<T>(Col.Format format)
        {
            if (this is PixImage<T> castImage && castImage.Format == format && castImage.ChannelCount == format.ChannelCount())
                return castImage;
            return new PixImage<T>(format, this);
        }

        public abstract PixImage ToCanonicalDenseLayout();

        #endregion

        #region Copy

        public abstract void CopyChannelTo<Tv>(long channelIndex, Matrix<Tv> target);

        public abstract void CopyVolumeTo<Tv>(Volume<Tv> target);

        public abstract PixImage CopyToPixImage();

        public abstract PixImage CopyToPixImageWithCanonicalDenseLayout();


        #endregion

        #region Image Manipulation

        public abstract PixImage TransformedPixImage(ImageTrafo trafo);

        public abstract PixImage RemappedPixImage(Matrix<float> xMap, Matrix<float> yMap, ImageInterpolation ip = ImageInterpolation.Cubic);

        public abstract PixImage ResizedPixImage(V2i size, ImageInterpolation ip = ImageInterpolation.Cubic);

        public abstract PixImage RotatedPixImage(double angleInRadiansCCW, bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic);

        public abstract PixImage ScaledPixImage(V2d scaleFactor, ImageInterpolation ip = ImageInterpolation.Cubic);

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

        #region IPixImageVisitor

        public abstract T Visit<T>(IPixImageVisitor<T> visitor);

        #endregion
    }

    /// <summary>
    /// The generic PixImage stores an image with a specific channel type that
    /// is specified as type parameter.
    /// </summary>
    [Serializable]
    public partial class PixImage<T> : PixImage
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
            if (format.IsPremultiplied() != pixImage.Format.IsPremultiplied())
            {
                throw new NotImplementedException(
                    "Conversion between alpha and premultiplied alpha formats not implemented yet."
                );
            }

            var srcInfo = pixImage.VolumeInfo;
            var dstChannels = Col.ChannelsOfFormat(format);
            var volume = CreateVolume<T>(srcInfo.Size.X, srcInfo.Size.Y, dstChannels.Length);
            volume.F = srcInfo.F;

            if (format == pixImage.Format && srcInfo.Size == volume.Size)
            {
                pixImage.CopyVolumeTo(volume);
            }
            else
            {
                var srcChannels = Col.ChannelsOfFormat(pixImage.Format);

                for (int dstIndex = 0; dstIndex < dstChannels.Length; dstIndex++)
                {
                    var channel = dstChannels[dstIndex];
                    var matrix = volume.SubXYMatrix(dstIndex);
                    var srcIndex = srcChannels.IndexOf(channel);

                    // If we have an RGB channel, we may also just copy a Gray or BW channel
                    if (srcIndex == -1 && (channel == Col.Channel.Red || channel == Col.Channel.Green || channel == Col.Channel.Blue))
                    {
                        var bw = srcChannels.IndexOf(Col.Channel.BW);
                        var gray = srcChannels.IndexOf(Col.Channel.Gray);
                        srcIndex = Fun.Max(bw, gray);
                    }

                    if (srcIndex > -1)
                    {
                        // Channel exists in source image, just copy
                        if (pixImage is PixImage<T> pi)
                        {
                            matrix.Set(pi.GetChannelInFormatOrder(srcIndex));
                        }
                        else
                        {
                            var order = pixImage.Format.ChannelOrder();
                            pixImage.CopyChannelTo(order[srcIndex], matrix); // CopyChannelTo uses canonical order
                        }
                    }
                    else if (channel == Col.Channel.Alpha || channel == Col.Channel.PremultipliedAlpha)
                    {
                        // Alpha channel does not exist in source image, fill with max value
                        matrix.Set(Col.Info<T>.MaxValue);
                    }
                    else if (channel == Col.Channel.Gray &&
                             srcChannels.Contains(Col.Channel.Red) &&
                             srcChannels.Contains(Col.Channel.Green) &&
                             srcChannels.Contains(Col.Channel.Blue))
                    {
                        var t1 = pixImage.PixFormat.Type;
                        var t2 = typeof(T);

                        if (s_rgbToGrayMap.TryGetValue((t1, t2), out var toGray))
                        {
                            toGray(pixImage, matrix);
                        }
                        else
                        {
                            throw new NotImplementedException(
                                $"Conversion from {t1} image with format {pixImage.Format} to {t2} grayscale not implemented."
                            );
                        }
                    }
                    else if (channel == Col.Channel.Blue &&
                             pixImage.Format == Col.Format.RG &&
                             dstChannels.Contains(Col.Channel.Red) &&
                             dstChannels.Contains(Col.Channel.Green))
                    {
                        // Allow expanding from RG to RGB formats, blue channel is set to zero
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"Conversion from format {pixImage.Format} to format {format} is not supported."
                        );
                    }
                }
            }

            Volume = volume;
            Format = format;
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

        public T[] Data => Volume.Data;

        public override Array Array => Volume.Array;

        public override PixFormat PixFormat => new PixFormat(typeof(T), Format);

        public override VolumeInfo VolumeInfo => Volume.Info;

        public override int BytesPerChannel => typeof(T).GetCLRSize();

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

        /// <summary>
        /// Returns the matrix representation of the volume if there is only
        /// one channel. Fails if there are multiple channels.
        /// </summary>
        public Matrix<T> Matrix => Volume.AsMatrixWindow();

        #endregion

        #region Image Manipulation

        #region Transformed

        public override PixImage TransformedPixImage(ImageTrafo trafo)
            => Transformed(trafo);

        public PixImage<T> Transformed(ImageTrafo trafo)
            => new PixImage<T>(Format, Volume.Transformed(trafo));

        #endregion

        #region Remapped

        public override PixImage RemappedPixImage(Matrix<float> xMap, Matrix<float> yMap, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Remapped(xMap, xMap, ip);

        public PixImage<T> Remapped(Matrix<float> xMap, Matrix<float> yMap, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return InvokeProcessors(
                (p) => p.Remap(this, xMap, yMap, ip, default),
                PixProcessorCaps.Remap, "remap image"
            );
        }

        [Obsolete("Use the PixImage processor API instead.")]
        public static void SetRemappedFun(Func<Volume<T>, Matrix<float>, Matrix<float>, ImageInterpolation, Volume<T>> remappedFun)
        {
            LegacyPixProcessor.Instance.SetRemapFun<T>(
                (remappedFun == null) ? null : (pi, xMap, yMap, ip) => new (pi.Format, remappedFun(pi.Volume, xMap, yMap, ip))
            );

            if (LegacyPixProcessor.Instance.Capabilities != PixProcessorCaps.None)
                SetProcessor(LegacyPixProcessor.Instance, 0);
            else
                RemoveProcessor(LegacyPixProcessor.Instance);
        }

        #endregion

        #region Resized

        public override PixImage ResizedPixImage(V2i newSize, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Scaled((V2d)newSize / (V2d)Size, ip);

        public PixImage<T> Resized(V2i newSize, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Scaled((V2d)newSize / (V2d)Size, ip);

        public PixImage<T> Resized(int xSize, int ySize, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Scaled(new V2d(xSize, ySize) / (V2d)Size, ip);

        #endregion

        #region Rotated

        public override PixImage RotatedPixImage(double angleInRadiansCCW, bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Rotated(angleInRadiansCCW, resize, ip);

        public PixImage<T> Rotated(double angleInRadiansCCW, bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return InvokeProcessors(
                (p) => p.Rotate(this, angleInRadiansCCW, resize, ip, default),
                PixProcessorCaps.Rotate, "rotate image"
            );
        }

        [Obsolete("Use the PixImage processor API instead.")]
        public static void SetRotatedFun(Func<Volume<T>, double, bool, ImageInterpolation, Volume<T>> rotatedFun)
        {
            LegacyPixProcessor.Instance.SetRotateFun<T>(
                (rotatedFun == null) ? null : (pi, angle, resize, ip) => new (pi.Format, rotatedFun(pi.Volume, angle, resize, ip))
            );

            if (LegacyPixProcessor.Instance.Capabilities != PixProcessorCaps.None)
                SetProcessor(LegacyPixProcessor.Instance, 0);
            else
                RemoveProcessor(LegacyPixProcessor.Instance);
        }

        #endregion

        #region Scaled

        public override PixImage ScaledPixImage(V2d scaleFactor, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Scaled(scaleFactor, ip);

        public PixImage<T> Scaled(V2d scaleFactor, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            if (scaleFactor.AnySmallerOrEqual(0))
                throw new ArgumentOutOfRangeException($"Scale factor must be positive ({scaleFactor}).");

            // SuperSample is only available for scale factors < 1; fall back to Cubic
            if (scaleFactor.AnyGreater(1.0) && ip == ImageInterpolation.SuperSample)
                ip = ImageInterpolation.Cubic;

            return InvokeProcessors(
                (p) => p.Scale(this, scaleFactor, ip),
                PixProcessorCaps.Scale, "scale image"
            );
        }

        [Obsolete("Use the PixImage processor API instead.")]
        public static void SetScaledFun(Func<Volume<T>, V2d, ImageInterpolation, Volume<T>> scaledFun)
        {
            LegacyPixProcessor.Instance.SetScaleFun<T>(
                (scaledFun == null) ? null : (pi, scaleFactor, ip) => new (pi.Format, scaledFun(pi.Volume, scaleFactor, ip))
            );

            if (LegacyPixProcessor.Instance.Capabilities != PixProcessorCaps.None)
                SetProcessor(LegacyPixProcessor.Instance, 0);
            else
                RemoveProcessor(LegacyPixProcessor.Instance);
        }

        public PixImage<T> Scaled(double scaleFactor, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Scaled(new V2d(scaleFactor, scaleFactor), ip);

        public PixImage<T> Scaled(double xScaleFactor, double yScaleFactor, ImageInterpolation ip = ImageInterpolation.Cubic)
            => Scaled(new V2d(xScaleFactor, yScaleFactor), ip);

        #endregion

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

        public override PixImage ToPixImage(Col.Format format)
        {
            if (Format == format && ChannelCount == format.ChannelCount())
                return this;
            return new PixImage<T>(format, this);
        }

        public PixImage<T> ToFormat(Col.Format format) => Format == format ? this : new PixImage<T>(format, this);

        public PixImage<T> ToImageLayout() => !Volume.HasImageLayout() ? new PixImage<T>(Format, this) : this;

        public override PixImage ToCanonicalDenseLayout() => ToImageLayout();

        #endregion

        #region Copy

        public override void CopyChannelTo<Tv>(long channelIndex, Matrix<Tv> target)
        {
            var subMatrix = GetChannel<Tv>(channelIndex);
            target.Set(subMatrix);
        }

        public override void CopyVolumeTo<Tv>(Volume<Tv> target)
        {
            if (Volume is Volume<Tv> source)
                target.Set(source);
            else
                target.Set(Volume.AsVolume<T, Tv>());
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
            matrix.Accessors = TensorAccessors.Get<T, Tv>(TensorAccessors.Intent.ColorChannel, Volume.DeltaArray);
            return matrix;
        }

        public Matrix<T, Tv> GetMatrix<Tv>() => Volume.GetMatrix<T, Tv>(Format.GetIntent());

        #endregion

        #region IPixImageVisitor

        public override TResult Visit<TResult>(IPixImageVisitor<TResult> visitor) => visitor.Visit<T>(this);

        #endregion
    }
}

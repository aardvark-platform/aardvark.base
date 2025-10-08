using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aardvark.Base
{
    /// <summary>
    /// Supported file formats for loading and saving images.
    /// </summary>
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
        Webp
    }

    /// <summary>
    /// Defines a visitor for strongly typed PixImage instances.
    /// Use this to dispatch operations based on the underlying pixel data type without reflection.
    /// </summary>
    /// <typeparam name="TResult">The result type produced by the visitor.</typeparam>
    public interface IPixImageVisitor<TResult>
    {
        /// <summary>
        /// Visits the given PixImage with element type <typeparamref name="TData"/>.
        /// </summary>
        /// <typeparam name="TData">The pixel element type (e.g. byte, float, etc.).</typeparam>
        /// <param name="image">The typed PixImage instance to visit.</param>
        /// <returns>The visitor-specific result.</returns>
        TResult Visit<TData>(PixImage<TData> image);
    }

    /// <summary>
    /// Lightweight description of a <see cref="PixImage"/>, containing its pixel format and dimensions.
    /// </summary>
    /// <param name="Format">The pixel format (channel type and color format).</param>
    /// <param name="Size">The 2D size in pixels (X = width, Y = height).</param>
    public record PixImageInfo(PixFormat Format, V2i Size);

    /// <summary>
    /// Base (non-generic) 2D pixel container abstraction. Provides format, size and conversion utilities
    /// for images independent of the underlying element type.
    /// </summary>
    [Serializable]
    public abstract partial class PixImage : IPix
    {
        /// <summary>
        /// Color format describing the channel layout and semantics.
        /// </summary>
        public Col.Format Format;

        #region Loaders

        #region PgmPixLoader

        /// <summary>
        /// Loader for saving PGM images. Reading is not supported.
        /// </summary>
        private class PgmPixLoader : IPixLoader
        {
            public string Name => "Aardvark PGM";

            public bool CanEncode => true;

            public bool CanDecode => false;

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

        private static readonly Dictionary<IPixLoader, int> s_loaders = new();

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

        internal static Result InvokeLoader<Result>(
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
                throw;
            }

            return default;
        }

        internal enum LoaderType { Encoder, Decoder, Any };

        internal static Result InvokeLoaders<Input, Result>(
                                LoaderType loaderType, IPixLoader loader, Input input,
                                Func<IPixLoader, Input, Result> invoke,
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

                try
                {
                    var result = invoke(loader, input);
                    if (isValid(result)) { return result; }
                }
                catch (Exception e)
                {
                    errorMessage += $" with {loader.Name} - {e.Message}";
                }
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

                var loaderErrors = new List<string>();

                for (int i = 0; i < loaders.Count; i++)
                {
                    if (i != 0) resetInput(input);

                    try
                    {
                        var result = invoke(loaders[i], input);
                        if (isValid(result)) return result;
                        loaderErrors.Add($"{i + 1}. {loaders[i].Name}: Failed");
                    }
                    catch (Exception e)
                    {
                        var message = Regex.Replace(e.Message, @"(\r\n?|\n)\z", ""); // Remove last line break in multi-line message
                        loaderErrors.Add($"{i + 1}. {loaders[i].Name}: {message}");
                    }
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
                    errorMessage += $" - No {loaderDesc}s available. Install {loaderDesc}s by referencing Aardvark.PixImage.* packages.";
                }
                else
                {
                    var errors = string.Join(Environment.NewLine, loaderErrors);
                    errorMessage += $" - All available {loaderDesc}s failed:{Environment.NewLine}{errors}";
                }
            }

            throw new ImageLoadException(errorMessage);
        }

        internal static Result InvokeLoadersWithStream<Result>(
                        LoaderType loaderType, IPixLoader loader, Stream stream,
                        Func<IPixLoader, Stream, Result> invoke,
                        Func<Result, bool> isValid,
                        string errorMessage)
        {
            var initialPosition = stream.Position;

            return InvokeLoaders(
                loaderType, loader, stream, invoke,
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
            foreach (var p in GetProcessors(minCapabilities))
            {
                try
                {
                    var result = invoke(p);
                    if (result != null) return result;
                }
                catch (Exception e)
                {
                    Report.Warn($"Failed to {operationDescription} with {p.Name} image processor: {e.Message}");
                }
            }

            var processors = GetProcessors();
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

        public PixImage() : this(Col.Format.None) { }

        public PixImage(Col.Format format)
        {
            Format = format;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying storage as a System.Array of the element type.
        /// </summary>
        public abstract Array Array { get; }

        /// <summary>
        /// Gets the pixel format of the image (channel type and color format).
        /// </summary>
        public abstract PixFormat PixFormat { get; }

        /// <summary>
        /// Gets structural information about the underlying 3D tensor including size and layout.
        /// </summary>
        public abstract VolumeInfo VolumeInfo { get; }

        /// <summary>
        /// Gets the size in bytes of a single channel element (e.g. 1 for byte, 4 for float).
        /// </summary>
        public abstract int BytesPerChannel { get; }

        /// <summary>
        /// Gets a lightweight summary of this image's format and size.
        /// </summary>
        public PixImageInfo Info => new(PixFormat, Size);

        /// <summary>
        /// Gets the total number of pixels (Width * Height).
        /// </summary>
        public int NumberOfPixels => Size.X * Size.Y;

        /// <summary>
        /// Gets the aspect ratio defined as Width / Height.
        /// </summary>
        public double AspectRatio => Size.X / (double)Size.Y;

        /// <summary>
        /// Gets the 2D size of the image in pixels.
        /// </summary>
        public V2i Size => (V2i)VolumeInfo.Size.XY;

        /// <summary>
        /// Gets the 2D size of the image in pixels as 64-bit integers.
        /// </summary>
        public V2l SizeL => VolumeInfo.Size.XY;

        /// <summary>
        /// Gets the image width in pixels.
        /// </summary>
        public int Width => Size.X;

        /// <summary>
        /// Gets the image width in pixels as a 64-bit integer.
        /// </summary>
        public long WidthL => SizeL.X;

        /// <summary>
        /// Gets the image height in pixels.
        /// </summary>
        public int Height => Size.Y;

        /// <summary>
        /// Gets the image height in pixels as a 64-bit integer.
        /// </summary>
        public long HeightL => SizeL.Y;

        /// <summary>
        /// Gets the number of channels per pixel.
        /// </summary>
        public int ChannelCount => (int)VolumeInfo.Size.Z;

        /// <summary>
        /// Gets the number of channels per pixel as a 64-bit integer.
        /// </summary>
        public long ChannelCountL => VolumeInfo.Size.Z;

        /// <summary>
        /// Gets the stride (number of bytes between two vertically adjacent pixels).
        /// </summary>
        public int Stride => BytesPerChannel * (int)VolumeInfo.DY;

        /// <summary>
        /// Gets the stride (number of bytes between two vertically adjacent pixels) as a 64-bit integer.
        /// </summary>
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
            { ".bmp", PixFileFormat.Bmp },
            { ".ico", PixFileFormat.Ico },
            { ".jpg", PixFileFormat.Jpeg },
            { ".jpeg", PixFileFormat.Jpeg },
            { ".jng", PixFileFormat.Jng },
            { ".koa", PixFileFormat.Koala },
            { ".lbm", PixFileFormat.Lbm },
            { ".iff", PixFileFormat.Iff },
            { ".mng", PixFileFormat.Mng },
            { ".pbm", PixFileFormat.Pbm },
            { ".pcd", PixFileFormat.Pcd },
            { ".pcx", PixFileFormat.Pcx },
            { ".pgm", PixFileFormat.Pgm },
            { ".png", PixFileFormat.Png },
            { ".ppm", PixFileFormat.Ppm },
            { ".ras", PixFileFormat.Ras },
            { ".targa", PixFileFormat.Targa },
            { ".tga", PixFileFormat.Targa },
            { ".tif", PixFileFormat.Tiff },
            { ".tiff", PixFileFormat.Tiff },
            { ".wap", PixFileFormat.Wbmp },
            { ".wbm", PixFileFormat.Wbmp },
            { ".wbmp", PixFileFormat.Wbmp },
            { ".psd", PixFileFormat.Psd },
            { ".cut", PixFileFormat.Cut },
            { ".xbm", PixFileFormat.Xbm },
            { ".xpm", PixFileFormat.Xpm },
            { ".dds", PixFileFormat.Dds },
            { ".gif", PixFileFormat.Gif },
            { ".hdr", PixFileFormat.Hdr },
            { ".g3", PixFileFormat.Faxg3 },
            { ".sgi", PixFileFormat.Sgi },
            { ".exr", PixFileFormat.Exr },
            { ".j2c", PixFileFormat.J2k },
            { ".j2k", PixFileFormat.J2k },
            { ".jp2", PixFileFormat.Jp2 },
            { ".pfm", PixFileFormat.Pfm },
            { ".pic", PixFileFormat.Pict },
            { ".pict", PixFileFormat.Pict },
            { ".pct", PixFileFormat.Pict },
            { ".raw", PixFileFormat.Raw },
            { ".wmp", PixFileFormat.Wmp },
            { ".webp", PixFileFormat.Webp },
        };

        private static readonly Lazy<Dictionary<PixFileFormat, string>> s_preferredExtensionOfFormat = new(() =>
            {
                var result = new Dictionary<PixFileFormat, string>();
                foreach (var kvp in s_formatOfExtension)
                {
                    result[kvp.Value] = kvp.Key;
                }
                result[PixFileFormat.PbmRaw] = result[PixFileFormat.Pbm];
                result[PixFileFormat.PgmRaw] = result[PixFileFormat.Pgm];
                result[PixFileFormat.PpmRaw] = result[PixFileFormat.Ppm];
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
            private delegate PixImage CreateDelegate(Col.Format format, long width, long height, long channels);
            private delegate PixImage CreateArrayDelegate(Array data, Col.Format format, long width, long height, long channels);

            private static class CreateDispatcher
            {
                public static PixImage Create<T>(Col.Format format, long width, long height, long channels)
                    => new PixImage<T>(format, width, height, channels);

                public static PixImage CreateArray<T>(Array data, Col.Format format, long width, long height, long channels)
                    => new PixImage<T>(format, (T[])data, width, height, channels);
            }

            private const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            private static readonly MethodInfo s_createMethod = typeof(CreateDispatcher).GetMethod(nameof(CreateDispatcher.Create), flags);
            private static readonly ConcurrentDictionary<Type, CreateDelegate> s_createDelegates = new();

            private static readonly MethodInfo s_createArrayMethod = typeof(CreateDispatcher).GetMethod(nameof(CreateDispatcher.CreateArray), flags);
            private static readonly ConcurrentDictionary<Type, CreateArrayDelegate> s_createArrayDelegates = new();

            public static PixImage Create(PixFormat format, long width, long height, long channels)
            {
                var create = s_createDelegates.GetOrAdd(format.Type, t => {
                    var mi = s_createMethod.MakeGenericMethod(t);
                    return (CreateDelegate)Delegate.CreateDelegate(typeof(CreateDelegate), mi);
                });

                return create(format.Format, width, height, channels);
            }

            public static PixImage Create(Array array, Col.Format format, long width, long height, long channels)
            {
                var create = s_createArrayDelegates.GetOrAdd(array.GetType().GetElementType(), t => {
                    var mi = s_createArrayMethod.MakeGenericMethod(t);
                    return (CreateArrayDelegate)Delegate.CreateDelegate(typeof(CreateArrayDelegate), mi);
                });

                return create(array, format, width, height, channels);
            }
        }

        #endregion

        #endregion

        #region Static Creator Functions

        /// <summary>
        /// Creates a new PixImage with the specified pixel format and dimensions.
        /// </summary>
        /// <param name="format">The pixel format (defines channel type and color format).</param>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        /// <param name="channels">The number of channels per pixel.</param>
        /// <returns>A new PixImage instance.</returns>
        public static PixImage Create(PixFormat format, long width, long height, long channels)
            => Dispatch.Create(format, width, height, channels);

        /// <summary>
        /// Creates a new PixImage with the specified format and dimensions using the format's default channel count.
        /// </summary>
        /// <param name="format">The pixel format (defines channel type and color format).</param>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        /// <returns>A new PixImage instance.</returns>
        public static PixImage Create(PixFormat format, long width, long height)
            => Dispatch.Create(format, width, height, format.ChannelCount);

        /// <summary>
        /// Wraps the given array as a PixImage using the provided color format and dimensions.
        /// </summary>
        /// <param name="array">The underlying data array.</param>
        /// <param name="format">The color format describing the channel layout and semantics.</param>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        /// <param name="channels">The number of channels per pixel.</param>
        /// <returns>A new PixImage instance referencing the provided array.</returns>
        public static PixImage Create(Array array, Col.Format format, long width, long height, long channels)
            => Dispatch.Create(array, format, width, height, channels);

        /// <summary>
        /// Wraps the given array as a PixImage using the format's default channel count.
        /// </summary>
        /// <param name="array">The underlying data array.</param>
        /// <param name="format">The color format describing the channel layout and semantics.</param>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        /// <returns>A new PixImage instance referencing the provided array.</returns>
        public static PixImage Create(Array array, Col.Format format, long width, long height)
            => Dispatch.Create(array, format, width, height, format.ChannelCount());

        /// <summary>
        /// Creates a 3D volume with image-friendly memory layout for the given size.
        /// </summary>
        /// <typeparam name="T">Element type of the volume.</typeparam>
        /// <param name="size">The volume size as (width, height, channels).</param>
        /// <returns>A new volume with the requested size.</returns>
        public static Volume<T> CreateVolume<T>(V3i size) => size.ToV3l().CreateImageVolume<T>();

        /// <inheritdoc cref="CreateVolume{T}(V3i)"/>
        public static Volume<T> CreateVolume<T>(V3l size) => size.CreateImageVolume<T>();

        /// <summary>
        /// Creates a 3D volume with image-friendly memory layout for the given dimensions.
        /// </summary>
        /// <typeparam name="T">Element type of the volume.</typeparam>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        /// <returns>A new volume with the requested size.</returns>
        public static Volume<T> CreateVolume<T>(long width, long height, long channels)
            => new V3l(width, height, channels).CreateImageVolume<T>();

        #endregion

        #region Load from file

        private static PixImage LoadFromFileWithLoader(IPixLoader loader, string filename)
            => InvokeLoader(
                    loader, l => l.LoadFromFile(filename), NotNull,
                    $"load image from file '{filename}'"
            );

        /// <summary>
        /// Loads an image from the given file without doing any conversions.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public static PixImage LoadRaw(string filename, IPixLoader loader = null)
            => InvokeLoaders(
                    LoaderType.Decoder, loader, filename, LoadFromFileWithLoader, Ignore, NotNull,
                    $"Could not load image from file '{filename}'"
            );

        /// <summary>
        /// Loads an image from the given file.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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

        private static PixImage LoadFromStreamWithLoader(IPixLoader loader, Stream stream)
            => InvokeLoader(
                    loader, l => l.LoadFromStream(stream), NotNull,
                    $"load image from {GetStreamDescription(stream)}"
            );

        /// <summary>
        /// Loads an image from the given stream without doing any conversions.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public static PixImage LoadRaw(Stream stream, IPixLoader loader = null)
            => InvokeLoadersWithStream(
                LoaderType.Decoder, loader, stream, LoadFromStreamWithLoader, NotNull,
                $"Could not load image from {GetStreamDescription(stream)}"
            );

        /// <summary>
        /// Loads an image from the given stream.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
        /// </summary>
        /// <example>
        /// E.g. for <see cref="PixFileFormat.Png"/>:
        /// <code>
        /// "foo.png" -> "foo.png"
        /// "foo" -> "foo.png"
        /// "foo.jpg" -> "foo.jpg.png"
        /// "foo.2011" -> "foo.2011.png"
        /// </code>
        /// </example>
        public static string NormalizedFileName(string fileName, PixFileFormat format)
        {
            bool appendExtension = false;
            if (Path.HasExtension(fileName))
            {
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (s_formatOfExtension.TryGetValue(ext, out var value))
                {
                    if (value != format)
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

        private bool SaveToFileWithLoader(IPixLoader loader, string filename, PixSaveParams saveParams)
            => InvokeLoader(
                    loader, l => { l.SaveToFile(filename, this, saveParams); return true; }, Identity,
                    $"save image to file '{filename}'"
            );

        /// <summary>
        /// Saves the image to the given file.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
                LoaderType.Encoder, loader, filename, (l, f) => SaveToFileWithLoader(l, f, saveParams), Ignore, Identity,
                $"Could not save image to file '{filename}'"
            );
        }

        /// <summary>
        /// Saves the image to the given file.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="filename">The file to save the image to.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        /// <exception cref="ArgumentException">if the filename extension is missing or unknown.</exception>
        public void Save(string filename, IPixLoader loader = null)
            => Save(filename, GetFormatOfExtension(filename), false, loader);

        /// <summary>
        /// Saves the image to the given JPEG file.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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

        private bool SaveToStreamWithLoader(IPixLoader loader, Stream stream, PixSaveParams saveParams)
            => InvokeLoader(
                    loader, l => { l.SaveToStream(stream, this, saveParams); return true; }, Identity,
                    "save image to stream"
            );

        /// <summary>
        /// Saves the image to the given stream.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="stream">The stream to save the image to.</param>
        /// <param name="saveParams">The save parameters to use.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public void Save(Stream stream, PixSaveParams saveParams, IPixLoader loader = null)
            => InvokeLoadersWithStream(
                LoaderType.Encoder, loader, stream, (l, s) => SaveToStreamWithLoader(l, s, saveParams), Identity,
                "Could not save image to stream"
            );

        /// <summary>
        /// Saves the image to the given stream.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
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
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="fileFormat">The image format of the stream.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A memory stream containing the image data.</returns>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public MemoryStream ToMemoryStream(PixFileFormat fileFormat, IPixLoader loader = null)
            => ToMemoryStream(new PixSaveParams(fileFormat), loader);

        /// <summary>
        /// Writes the image to a JPEG memory stream.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="quality">The quality of the JPEG file. Must be within 0 - 100.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A memory stream containing the image data.</returns>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public MemoryStream ToMemoryStreamAsJpeg(int quality = PixJpegSaveParams.DefaultQuality, IPixLoader loader = null)
            => ToMemoryStream(new PixJpegSaveParams(quality), loader);

        /// <summary>
        /// Writes the image to a PNG memory stream.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="compressionLevel">The compression level of the PNG file. Must be within 0 - 9.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A memory stream containing the image data.</returns>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        public MemoryStream ToMemoryStreamAsPng(int compressionLevel = PixPngSaveParams.DefaultCompressionLevel, IPixLoader loader = null)
            => ToMemoryStream(new PixJpegSaveParams(compressionLevel), loader);

        #endregion

        #region Query info from file

        private static PixImageInfo GetInfoFromFileWithLoader(IPixLoader loader, string filename)
            => InvokeLoader(
                    loader, l => l.GetInfoFromFile(filename), NotNull,
                    $"get image info from file '{filename}'"
            );

        /// <summary>
        /// Loads basic information about an image from a file without loading its content.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A PixImageInfo instance containing basic information about the image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public static PixImageInfo GetInfoFromFile(string filename, IPixLoader loader = null)
            => InvokeLoaders(
                    LoaderType.Any, loader, filename, GetInfoFromFileWithLoader, Ignore, NotNull,
                    $"Could not get image info from file '{filename}'"
            );

        #endregion

        #region Query info from stream

        private static PixImageInfo GetInfoFromStreamWithLoader(IPixLoader loader, Stream stream)
            => InvokeLoader(
                    loader, l => l.GetInfoFromStream(stream), NotNull,
                    "get image info from stream"
            );

        /// <summary>
        /// Loads basic information about an image from a stream without loading its content.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <returns>A PixImageInfo instance containing basic information about the image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public static PixImageInfo GetInfoFromStream(Stream stream, IPixLoader loader = null)
            => InvokeLoadersWithStream(
                    LoaderType.Any, loader, stream, GetInfoFromStreamWithLoader, NotNull,
                    $"Could not get image info from stream"
            );

        #endregion

        #region Conversions

        /// <summary>
        /// Attempts to cast this instance to <see cref="PixImage{T}"/>. Returns null if the element type differs.
        /// </summary>
        /// <typeparam name="T">Target element type.</typeparam>
        /// <returns>The typed image or null.</returns>
        public PixImage<T> AsPixImage<T>() => this as PixImage<T>;

        /// <summary>
        /// Returns a typed view of this image. If the element type differs, a new image with converted element type is created by copying.
        /// </summary>
        /// <typeparam name="T">Target element type.</typeparam>
        /// <returns>A typed image of <typeparamref name="T"/>.</returns>
        public PixImage<T> ToPixImage<T>() => AsPixImage<T>() ?? new PixImage<T>(this);

        /// <summary>
        /// Converts this image to the specified color format while keeping the underlying element type.
        /// If this image already has the requested format, returns the same instance;
        /// in that case no data conversion or additional allocation is performed.
        /// </summary>
        /// <param name="format">Target color format.</param>
        /// <returns>An image in the requested format; this instance if no conversion is required.</returns>
        public abstract PixImage ToPixImage(Col.Format format);

        /// <summary>
        /// Converts this image to the specified color format and element type.
        /// If this image already has the requested format and element type, returns the same instance;
        /// in that case no data conversion or additional allocation is performed.
        /// </summary>
        /// <param name="format">Target color format.</param>
        /// <typeparam name="T">Target element type.</typeparam>
        /// <returns>A typed image in the requested format; this instance if no conversion is required.</returns>
        public PixImage<T> ToPixImage<T>(Col.Format format)
        {
            if (this is PixImage<T> image && image.Format == format && image.ChannelCount == format.ChannelCount()) return image;
            return new PixImage<T>(format, this);
        }

        /// <summary>
        /// Returns a representation with canonical, densely packed memory layout.
        /// If this image is already in the correct layout, returns the same instance;
        /// in that case no data conversion or additional allocation is performed.
        /// </summary>
        public abstract PixImage ToCanonicalDenseLayout();

        #endregion

        #region Copy

        /// <summary>
        /// Copies a single channel to the target matrix (must match in size).
        /// If the element type differs, the data are reinterpreted.
        /// </summary>
        /// <typeparam name="Tv">Element type of the destination matrix.</typeparam>
        /// <param name="channelIndex">Index of the channel in this matrix.</param>
        /// <param name="target">Destination matrix receiving the channel data.</param>
        /// <exception cref="NotSupportedException">if the element type <typeparamref name="Tv"/> is invalid.</exception>
        public abstract void CopyChannelTo<Tv>(long channelIndex, Matrix<Tv> target);

        /// <summary>
        /// Copies the entire underlying volume to the given target volume (must match in size).
        /// If the element type differs, the data are reinterpreted.
        /// </summary>
        /// <typeparam name="Tv">Element type of the destination volume.</typeparam>
        /// <param name="target">Destination volume.</param>
        /// <exception cref="NotSupportedException">if the element type <typeparamref name="Tv"/> is invalid.</exception>
        public abstract void CopyVolumeTo<Tv>(Volume<Tv> target);

        /// <summary>
        /// Creates a deep copy of this image.
        /// </summary>
        public abstract PixImage CopyToPixImage();

        /// <summary>
        /// Creates a deep copy of this image using the canonical dense memory layout.
        /// </summary>
        public abstract PixImage CopyToPixImageWithCanonicalDenseLayout();

        #endregion

        #region Image Manipulation

        /// <summary>
        /// Returns a transformed copy of this image using the specified image transformation.
        /// </summary>
        /// <param name="trafo">The image transformation to apply.</param>
        /// <returns>A new PixImage with the transformation applied.</returns>
        public abstract PixImage TransformedPixImage(ImageTrafo trafo);

        /// <summary>
        /// Returns a remapped copy of this image using the provided per-pixel coordinate maps.
        /// </summary>
        /// <param name="mapX">Matrix of X-coordinate samples mapping destination pixels to source X.</param>
        /// <param name="mapY">Matrix of Y-coordinate samples mapping destination pixels to source Y.</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <returns>A new PixImage generated by sampling the source with the given maps.</returns>
        public abstract PixImage RemappedPixImage(Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation = ImageInterpolation.Cubic);

        /// <summary>
        /// Returns a resized copy of this image.
        /// </summary>
        /// <param name="size">Requested output size in pixels.</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <returns>A new PixImage with the given size.</returns>
        public abstract PixImage ResizedPixImage(V2i size, ImageInterpolation interpolation = ImageInterpolation.Cubic);

        /// <summary>
        /// Returns a rotated copy of this image around its center.
        /// </summary>
        /// <param name="angleInRadians">Rotation angle in radians, counter-clockwise.</param>
        /// <param name="resize">When <c>true</c>, the output image is resized to fully contain the rotated content; otherwise it keeps the original size.</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <returns>A new PixImage containing the rotated image.</returns>
        public abstract PixImage RotatedPixImage(double angleInRadians, bool resize = true, ImageInterpolation interpolation = ImageInterpolation.Cubic);

        /// <summary>
        /// Returns a scaled copy of this image by the given factors.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply in X and Y (1.0 keeps the size).</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <returns>A new PixImage scaled by the given factors.</returns>
        public abstract PixImage ScaledPixImage(V2d scaleFactor, ImageInterpolation interpolation = ImageInterpolation.Cubic);

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

        /// <summary>
        /// Dispatches the current instance to a visitor based on its element type.
        /// </summary>
        /// <typeparam name="TResult">The result type produced by the visitor.</typeparam>
        /// <param name="visitor">The visitor implementation.</param>
        /// <returns>The visitor-specific result.</returns>
        public abstract TResult Visit<TResult>(IPixImageVisitor<TResult> visitor);

        #endregion
    }

    /// <summary>
    /// 2D pixel container with element type <typeparamref name="T"/>.
    /// Provides access to the underlying 3D volume.
    /// </summary>
    /// <typeparam name="T">Per-channel element type.</typeparam>
    [Serializable]
    public class PixImage<T> : PixImage
    {
        /// <summary>
        /// The underlying 3D volume storing the image data.
        /// </summary>
        public Volume<T> Volume;

        #region Constructors

        #region  Default

        /// <summary>
        /// Initializes a new empty PixImage instance without allocating storage.
        /// Intended for serializers or deferred initialization scenarios. The
        /// <see cref="Volume"/> field must be assigned before use.
        /// </summary>
        public PixImage() { }


        #endregion

        #region From Volume

        /// <summary>
        /// Creates a new PixImage backed by the given volume and using the specified color format.
        /// No data is copied; the instance takes a reference to <paramref name="volume"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="volume">Backing volume in image layout. Not copied.</param>
        public PixImage(Col.Format format, Volume<T> volume)
            : base(format)
        {
            Volume = volume;
        }

        /// <summary>
        /// Creates a new PixImage backed by the given volume and using the default color format for the element type.
        /// No data is copied; the instance takes a reference to <paramref name="volume"/>.
        /// </summary>
        /// <param name="volume">Backing volume in image layout. Not copied.</param>
        public PixImage(Volume<T> volume)
            : this(Col.FormatDefaultOf(typeof(T), volume.SZ), volume)
        { }

        #endregion

        #region  From Dimensions

        /// <summary>
        /// Allocates a new image with the given 2D size and channel count using the default color format for the element type.
        /// </summary>
        /// <param name="size">Image size in pixels (width, height).</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(V2i size, int channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateVolume<T>(size.X, size.Y, channels))
        { }

        /// <inheritdoc cref="PixImage{T}(V2i, int)"/>
        public PixImage(V2l size, long channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateVolume<T>(size.X, size.Y, channels))
        { }

        /// <summary>
        /// Allocates a new image with the given dimensions and channel count using the default color format for the element type.
        /// </summary>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(int width, int height, int channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateVolume<T>(width, height, channels))
        { }

        /// <inheritdoc cref="PixImage{T}(int, int, int)"/>
        public PixImage(long width, long height, long channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateVolume<T>(width, height, channels))
        { }

        /// <summary>
        /// Allocates a new image with the given size and channel format. The number of channels is
        /// derived from <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="size">Image size in pixels (width, height).</param>
        public PixImage(Col.Format format, V2i size)
            : this(format, CreateVolume<T>(size.X, size.Y, format.ChannelCount()))
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, V2i)"/>
        public PixImage(Col.Format format, V2l size)
            : this(format, CreateVolume<T>(size.X, size.Y, format.ChannelCount()))
        { }

        /// <summary>
        /// Allocates a new image with the given dimensions and channel format. The number of channels is
        /// derived from <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        public PixImage(Col.Format format, int width, int height)
            : this(format, CreateVolume<T>(width, height, format.ChannelCount()))
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, int, int)"/>
        public PixImage(Col.Format format, long width, long height)
            : this(format, CreateVolume<T>(width, height, format.ChannelCount()))
        { }

        /// <summary>
        /// Allocates a new image with the given size, explicit channel count and format.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="size">Image size in pixels (width, height).</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(Col.Format format, V2i size, int channels)
            : this(format, CreateVolume<T>(size.X, size.Y, channels))
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, V2i, int)"/>
        public PixImage(Col.Format format, V2l size, long channels)
            : this(format, CreateVolume<T>(size.X, size.Y, channels))
        { }

        [Obsolete("Use PixImage<T>(Col.Format, V2i, int) or PixImage<T>(Col.Format, V2l, long) instead.")]
        public PixImage(Col.Format format, V2i size, long channels)
            : this(format, CreateVolume<T>(size.X, size.Y, channels))
        { }

        /// <summary>
        /// Allocates a new image with the given dimensions, explicit channel count and format.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(Col.Format format, int width, int height, int channels)
            : this(format, CreateVolume<T>(width, height, channels))
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, int, int, int)"/>
        public PixImage(Col.Format format, long width, long height, long channels)
            : this(format, CreateVolume<T>(width, height, channels))
        { }

        #endregion

        #region From PixImageInfo

        /// <summary>
        /// Initializes a new image from meta information (without loading pixel data).
        /// </summary>
        /// <param name="info">Image metadata (size, format, type).</param>
        /// <exception cref="ArgumentException">if the element type <typeparamref name="T"/> does not match <c>info.Format.Type</c>.</exception>
        public PixImage(PixImageInfo info)
            : this(info.Format.Format, info.Size)
        {
            if (info.Format.Type != typeof(T)) throw new ArgumentException($"Expected element type {typeof(T)} but format has type {info.Format.Type}.");
        }

        #endregion

        #region From Channel Matrices

        /// <summary>
        /// Creates an image from the given channel matrices with the default color format.
        /// The channel data are copied to a newly allocated volume.
        /// </summary>
        /// <param name="channels">Sequence of channel matrices in canonical order (red, green, blue, alpha).</param>
        /// <exception cref="ArgumentException">if <paramref name="channels"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if fewer channels are provided than expected by the default color format.</exception>
        /// <exception cref="ArgumentException">if the channel matrices differ in size.</exception>
        public PixImage(IEnumerable<Matrix<T>> channels)
            : this(channels.ToArray())
        { }

        /// <summary>
        /// Creates an image from the given channel matrices with the default color format.
        /// The channel data are copied to a newly allocated volume.
        /// </summary>
        /// <param name="channels">Array of channel matrices in canonical order (red, green, blue, alpha).</param>
        /// <exception cref="ArgumentException">if <paramref name="channels"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if fewer channels are provided than expected by the default color format.</exception>
        /// <exception cref="ArgumentException">if the channel matrices differ in size.</exception>
        public PixImage(params Matrix<T>[] channels)
            : this(Col.FormatDefaultOf(typeof(T), channels.Length), channels)
        { }

        /// <summary>
        /// Creates an image from the given channel matrices and color format.
        /// The channel data are copied to a newly allocated volume.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="channels">Sequence of channel matrices in canonical order (red, green, blue, alpha).</param>
        /// <exception cref="ArgumentException">if <paramref name="channels"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if fewer channels are provided than expected by <paramref name="format"/>.</exception>
        /// <exception cref="ArgumentException">if the channel matrices differ in size.</exception>
        public PixImage(Col.Format format, IEnumerable<Matrix<T>> channels)
            : this(format, channels.ToArray())
        { }

        /// <summary>
        /// Creates an image from the given channel matrices and color format.
        /// The channel data are copied to a newly allocated volume.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="channels">Array of channel matrices in canonical order (red, green, blue, alpha).</param>
        /// <exception cref="ArgumentException">if <paramref name="channels"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if fewer channels are provided than expected by <paramref name="format"/>.</exception>
        /// <exception cref="ArgumentException">if the channel matrices differ in size.</exception>
        public PixImage(Col.Format format, params Matrix<T>[] channels)
            : base(format)
        {
            int channelCount = format.ChannelCount();
            if (channels.IsEmptyOrNull()) throw new ArgumentException("Channels cannot be null or empty.");
            if (channels.Length < channelCount) throw new ArgumentException($"Color format expects {channelCount} but got only {channels.Length}.");

            var ch0 = channels[0];

            var sx = ch0.SX;
            var sy = ch0.SY;

            var volume = CreateVolume<T>(sx, sy, channelCount);
            var order = format.ChannelOrder();

            for (int i = 0; i < channelCount; i++)
            {
                var mat = volume.SubXYMatrix(order[i]);
                var channel = channels[i];
                if (channel.IsValid) mat.Set(channel);
            }

            Volume = volume;
        }

        #endregion

        #region From Array

        /// <summary>
        /// Creates a new PixImage backed by the given array and using the specified color format.
        /// No data is copied; the instance takes a reference to <paramref name="data"/>.
        /// The data array is interpreted as an image volume with the specified dimensions.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="data">Backing array in image layout. Not copied.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(Col.Format format, T[] data, int width, int height, int channels)
            : this(format, data.CreateImageVolume(new V3l(width, height, channels)))
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, T[], int, int, int)"/>
        public PixImage(Col.Format format, T[] data, long width, long height, long channels)
            : this(format, data.CreateImageVolume(new V3l(width, height, channels)))
        { }

        /// <summary>
        /// Creates a new PixImage backed by the given array and using the specified color format.
        /// No data is copied; the instance takes a reference to <paramref name="data"/>.
        /// The data array is interpreted as an image volume with the specified dimensions.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="data">Backing array in image layout. Not copied.</param>
        /// <param name="size">Image size in pixels (width, height).</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(Col.Format format, T[] data, V2i size, int channels)
            : this(format, data, size.X, size.Y, channels)
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, T[], V2i, int)"/>
        public PixImage(Col.Format format, T[] data, V2l size, long channels)
            : this(format, data, size.X, size.Y, channels)
        { }

        /// <summary>
        /// Creates a new PixImage backed by the given array and using the specified color format.
        /// No data is copied; the instance takes a reference to <paramref name="data"/>.
        /// The data array is interpreted as an image volume with the specified dimensions.
        /// The number of channels is derived from <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="data">Backing array in image layout. Not copied.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        public PixImage(Col.Format format, T[] data, int width, int height)
            : this(format, data, width, height, format.ChannelCount())
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, T[], int, int)"/>
        public PixImage(Col.Format format, T[] data, long width, long height)
            : this(format, data, width, height, format.ChannelCount())
        { }

        /// <summary>
        /// Creates a new PixImage backed by the given array and using the specified color format.
        /// No data is copied; the instance takes a reference to <paramref name="data"/>.
        /// The data array is interpreted as an image volume with the specified dimensions.
        /// The number of channels is derived from <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="data">Backing array in image layout. Not copied.</param>
        /// <param name="size">Image size in pixels (width, height).</param>
        public PixImage(Col.Format format, T[] data, V2i size)
            : this(format, data, size.X, size.Y)
        { }

        /// <inheritdoc cref="PixImage{T}(Col.Format, T[], V2i)"/>
        public PixImage(Col.Format format, T[] data, V2l size)
            : this(format, data, size.X, size.Y)
        { }

        /// <summary>
        /// Creates a new PixImage backed by the given array and using the default color format for the element type.
        /// No data is copied; the instance takes a reference to <paramref name="data"/>.
        /// The data array is interpreted as an image volume with the specified dimensions.
        /// </summary>
        /// <param name="data">Backing array in image layout. Not copied.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(T[] data, int width, int height, int channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), data, width, height, channels)
        { }

        /// <inheritdoc cref="PixImage{T}(T[], int, int, int)"/>
        public PixImage(T[] data, long width, long height, long channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), data, width, height, channels)
        { }

        /// <summary>
        /// Creates a new PixImage backed by the given array and using the default color format for the element type.
        /// No data is copied; the instance takes a reference to <paramref name="data"/>.
        /// The data array is interpreted as an image volume with the specified dimensions.
        /// </summary>
        /// <param name="data">Backing array in image layout. Not copied.</param>
        /// <param name="size">Image size in pixels (width, height).</param>
        /// <param name="channels">Number of channels.</param>
        public PixImage(T[] data, V2i size, int channels)
            : this(data, size.X, size.Y, channels)
        { }

        /// <inheritdoc cref="PixImage{T}(T[], V2i, int)"/>
        public PixImage(T[] data, V2l size, long channels)
            : this(data, size.X, size.Y, channels)
        { }

        #endregion

        #region From Copy

        /// <summary>
        /// Creates a typed copy of the given image. Always allocates new storage and copies data.
        /// The channel count is taken from <paramref name="source"/>, the element type becomes
        /// <typeparamref name="T"/> (conversion may occur).
        /// </summary>
        /// <param name="source">Source image to copy from.</param>
        /// <remarks>
        /// <inheritdoc cref="PixImage{T}(Col.Format, PixImage)" path="/remarks"/>
        /// </remarks>
        public PixImage(PixImage source)
            : this(Col.FormatDefaultOf(typeof(T), source.Format.ChannelCount()), source)
        { }

        /// <summary>
        /// Creates a typed copy of the given image in the requested color format. Always allocates new storage
        /// and copies or converts channel data as needed.
        /// </summary>
        /// <param name="format">Target color format for the new image.</param>
        /// <param name="source">Source image to copy from.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// Premultiplied state must match between <paramref name="format"/> and <paramref name="source"/>;
        /// otherwise a <see cref="NotImplementedException"/> is thrown.
        /// </item>
        /// <item>
        /// If the formats match and sizes are equal, a straight copy is performed.
        /// </item>
        /// <item>
        /// Missing alpha is filled with the maximum value of <typeparamref name="T"/>.
        /// </item>
        /// <item>
        /// Gray can be computed from RGB for a few common element types; unsupported combinations throw.
        /// </item>
        /// <item>
        /// Expanding RG to RGB sets blue to zero.
        /// </item>
        /// <item>
        /// Other unmapped conversions throw <see cref="NotSupportedException"/>.
        /// </item>
        /// </list>
        /// </remarks>
        public PixImage(Col.Format format, PixImage source)
        {
            if (format.IsPremultiplied() != source.Format.IsPremultiplied())
            {
                throw new NotImplementedException(
                    "Conversion between alpha and premultiplied alpha formats not implemented yet."
                );
            }

            var srcInfo = source.VolumeInfo;
            var dstChannels = Col.ChannelsOfFormat(format);
            var volume = CreateVolume<T>(srcInfo.Size.X, srcInfo.Size.Y, dstChannels.Length);
            volume.F = srcInfo.F;

            if (format == source.Format && srcInfo.Size == volume.Size)
            {
                source.CopyVolumeTo(volume);
            }
            else
            {
                var srcChannels = Col.ChannelsOfFormat(source.Format);

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
                        if (source is PixImage<T> pi)
                        {
                            matrix.Set(pi.GetChannelInFormatOrder(srcIndex));
                        }
                        else
                        {
                            var order = source.Format.ChannelOrder();
                            source.CopyChannelTo(order[srcIndex], matrix); // CopyChannelTo uses canonical order
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
                        var t1 = source.PixFormat.Type;
                        var t2 = typeof(T);

                        if (s_rgbToGrayMap.TryGetValue((t1, t2), out var toGray))
                        {
                            toGray(source, matrix);
                        }
                        else
                        {
                            throw new NotImplementedException(
                                $"Conversion from {t1} image with format {source.Format} to {t2} grayscale not implemented."
                            );
                        }
                    }
                    else if (channel == Col.Channel.Blue &&
                             source.Format == Col.Format.RG &&
                             dstChannels.Contains(Col.Channel.Red) &&
                             dstChannels.Contains(Col.Channel.Green))
                    {
                        // Allow expanding from RG to RGB formats, blue channel is set to zero
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"Conversion from format {source.Format} to format {format} is not supported."
                        );
                    }
                }
            }

            Volume = volume;
            Format = format;
        }

        #endregion

        #region From File / Stream

        /// <summary>
        /// Loads an image from the given file.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public PixImage(string filename, IPixLoader loader = null)
        {
            var loadImage = LoadRaw(filename, loader);
            var channels = loadImage.Format.ChannelCount();

            if (loadImage is not PixImage<T> image || image.ChannelCount != channels)
                image = new PixImage<T>(loadImage);

            Volume = image.Volume;
            Format = image.Format;
        }

        /// <summary>
        /// Loads an image from the given stream.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use, or null if no specific loader is to be used.</param>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public PixImage(Stream stream, IPixLoader loader = null)
        {
            var loadImage = LoadRaw(stream, loader);
            var channels = loadImage.Format.ChannelCount();

            if (loadImage is not PixImage<T> image || image.ChannelCount != channels)
                image = new PixImage<T>(loadImage);

            Volume = image.Volume;
            Format = image.Format;
        }

        #endregion

        #endregion

        #region Static Creator Functions

        /// <inheritdoc cref="PixImage{T}(Matrix{T}[])"/>
        public static PixImage<T> Create(params Matrix<T>[] channels)
            => new(channels);

        /// <inheritdoc cref="PixImage{T}(Col.Format, Matrix{T}[])"/>
        public static PixImage<T> Create(Col.Format format, params Matrix<T>[] channels)
            => new(format, channels);

        /// <inheritdoc cref="PixImage{T}(Col.Format, Matrix{T}[])"/>
        public static PixImage<T> Create<Td>(Col.Format format, params Matrix<Td, T>[] channels)
        {
            int channelCount = format.ChannelCount();
            if (channels.IsEmptyOrNull()) throw new ArgumentException("Channels cannot be null or empty.");
            if (channels.Length < channelCount) throw new ArgumentException($"Color format expects {channelCount} but got only {channels.Length}.");

            var ch0 = channels[0];

            var sx = ch0.SX;
            var sy = ch0.SY;

            var volume = CreateVolume<T>(sx, sy, channelCount);
            var order = format.ChannelOrder();

            for (int i = 0; i < channelCount; i++)
            {
                var mat = volume.SubXYMatrix(order[i]);
                var channel = channels[i];
                if (channel.IsValid) mat.Set(channel);
            }

            return new PixImage<T>(format, volume);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying storage as a <typeparamref name="T"/> array.
        /// </summary>
        public T[] Data => Volume.Data;

        /// <inheritdoc />
        public override Array Array => Volume.Array;

        /// <inheritdoc />
        public override PixFormat PixFormat => new PixFormat(typeof(T), Format);

        /// <inheritdoc />
        public override VolumeInfo VolumeInfo => Volume.Info;

        /// <inheritdoc />
        public override int BytesPerChannel => typeof(T).GetCLRSize();

        /// <summary>
        /// Gets the channel matrices of the image in canonical order (red, green, blue, alpha).
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
        /// Gets the channel matrices of the image as array in canonical order (red, green, blue, alpha).
        /// </summary>
        public Matrix<T>[] ChannelArray => Channels.ToArray();

        /// <summary>
        /// Returns the matrix representation of the volume if there is only one channel.
        /// </summary>
        /// <exception cref="InvalidOperationException">if there are multiple channels.</exception>
        public Matrix<T> Matrix => Volume.AsMatrixWindow();

        #endregion

        #region Image Manipulation

        #region Transformed

        /// <inheritdoc />
        public override PixImage TransformedPixImage(ImageTrafo trafo)
            => Transformed(trafo);

        /// <inheritdoc cref="TransformedPixImage" />
        public PixImage<T> Transformed(ImageTrafo trafo) => new(Format, Volume.Transformed(trafo));

        #endregion

        #region Remapped

        /// <inheritdoc />
        public override PixImage RemappedPixImage(Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Remapped(mapX, mapX, interpolation);

        /// <inheritdoc cref="RemappedPixImage" />
        public PixImage<T> Remapped(Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation = ImageInterpolation.Cubic)
        {
            return InvokeProcessors(
                (p) => p.Remap(this, mapX, mapY, interpolation),
                PixProcessorCaps.Remap, "remap image"
            );
        }

        [Obsolete("Use the PixImage processor API instead.")]
        public static void SetRemappedFun(Func<Volume<T>, Matrix<float>, Matrix<float>, ImageInterpolation, Volume<T>> remappedFun)
        {
            LegacyPixProcessor.Instance.SetRemapFun<T>(
                (remappedFun == null) ? null : (pi, xMap, yMap, ip) => new PixImage<T>(pi.Format, remappedFun(pi.Volume, xMap, yMap, ip))
            );

            if (LegacyPixProcessor.Instance.Capabilities != PixProcessorCaps.None)
                SetProcessor(LegacyPixProcessor.Instance, 0);
            else
                RemoveProcessor(LegacyPixProcessor.Instance);
        }

        #endregion

        #region Resized

        /// <inheritdoc />
        public override PixImage ResizedPixImage(V2i size, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Scaled((V2d)size / (V2d)Size, interpolation);

        /// <inheritdoc cref="ResizedPixImage" />
        public PixImage<T> Resized(V2i size, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Scaled((V2d)size / (V2d)Size, interpolation);

        /// <summary>
        /// Returns a resized copy of this image.
        /// </summary>
        /// <param name="width">Requested output width in pixels.</param>
        /// <param name="height">Requested output height in pixels.</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <returns>A new PixImage with the given size.</returns>
        public PixImage<T> Resized(int width, int height, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Scaled(new V2d(width, height) / (V2d)Size, interpolation);

        #endregion

        #region Rotated

        /// <inheritdoc />
        public override PixImage RotatedPixImage(double angleInRadians, bool resize = true, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Rotated(angleInRadians, resize, interpolation);

        /// <inheritdoc cref="RotatedPixImage" />
        public PixImage<T> Rotated(double angleInRadians, bool resize = true, ImageInterpolation interpolation = ImageInterpolation.Cubic)
        {
            return InvokeProcessors(
                (p) => p.Rotate(this, angleInRadians, resize, interpolation),
                PixProcessorCaps.Rotate, "rotate image"
            );
        }

        [Obsolete("Use the PixImage processor API instead.")]
        public static void SetRotatedFun(Func<Volume<T>, double, bool, ImageInterpolation, Volume<T>> rotatedFun)
        {
            LegacyPixProcessor.Instance.SetRotateFun<T>(
                (rotatedFun == null) ? null : (pi, angle, resize, ip) => new PixImage<T>(pi.Format, rotatedFun(pi.Volume, angle, resize, ip))
            );

            if (LegacyPixProcessor.Instance.Capabilities != PixProcessorCaps.None)
                SetProcessor(LegacyPixProcessor.Instance, 0);
            else
                RemoveProcessor(LegacyPixProcessor.Instance);
        }

        #endregion

        #region Scaled

        /// <inheritdoc />
        public override PixImage ScaledPixImage(V2d scaleFactor, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Scaled(scaleFactor, interpolation);

        /// <inheritdoc cref="ScaledPixImage" />
        public PixImage<T> Scaled(V2d scaleFactor, ImageInterpolation interpolation = ImageInterpolation.Cubic)
        {
            if (scaleFactor.AnySmallerOrEqual(0))
                throw new ArgumentOutOfRangeException($"Scale factor must be positive ({scaleFactor}).");

            // SuperSample is only available for scale factors < 1; fall back to Cubic
            if (scaleFactor.AnyGreater(1.0) && interpolation == ImageInterpolation.SuperSample)
                interpolation = ImageInterpolation.Cubic;

            return InvokeProcessors(
                (p) => p.Scale(this, scaleFactor, interpolation),
                PixProcessorCaps.Scale, "scale image"
            );
        }

        [Obsolete("Use the PixImage processor API instead.")]
        public static void SetScaledFun(Func<Volume<T>, V2d, ImageInterpolation, Volume<T>> scaledFun)
        {
            LegacyPixProcessor.Instance.SetScaleFun<T>(
                (scaledFun == null) ? null : (pi, scaleFactor, ip) => new PixImage<T>(pi.Format, scaledFun(pi.Volume, scaleFactor, ip))
            );

            if (LegacyPixProcessor.Instance.Capabilities != PixProcessorCaps.None)
                SetProcessor(LegacyPixProcessor.Instance, 0);
            else
                RemoveProcessor(LegacyPixProcessor.Instance);
        }


        /// <inheritdoc cref="ScaledPixImage" />
        public PixImage<T> Scaled(double scaleFactor, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Scaled(new V2d(scaleFactor, scaleFactor), interpolation);

        /// <summary>
        /// Returns a scaled copy of this image by the given factors.
        /// </summary>
        /// <param name="scaleFactorX">The scale factor to apply in X (1.0 keeps the width).</param>
        /// <param name="scaleFactorY">The scale factor to apply in Y (1.0 keeps the height).</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <returns>A new PixImage scaled by the given factors.</returns>
        public PixImage<T> Scaled(double scaleFactorX, double scaleFactorY, ImageInterpolation interpolation = ImageInterpolation.Cubic)
            => Scaled(new V2d(scaleFactorX, scaleFactorY), interpolation);

        #endregion

        #endregion

        #region SubImages

        /// <summary>
        /// Returns the specified region as new PixImage.
        /// No data is copied, the internal <see cref="Volume{T}"/> is only a view of the original one.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if the region is out of bounds.</exception>
        public PixImage<T> SubImage(long x, long y, long width, long height)
        {
            if (x < 0 || y < 0 || x + width > Size.X || y + height > Size.Y)
                throw new ArgumentOutOfRangeException(null, "Subregion out of image boundary.");
            return new PixImage<T>(Format, Volume.SubVolume(x, y, 0, width, height, ChannelCount));
        }

        /// <inheritdoc cref="SubImage(long, long, long, long)"/>
        public PixImage<T> SubImage(V2i offset, V2i size) => SubImage(offset.X, offset.Y, size.X, size.Y);

        /// <inheritdoc cref="SubImage(long, long, long, long)"/>
        public PixImage<T> SubImage(V2l offset, V2l size) => SubImage(offset.X, offset.Y, size.X, size.Y);

        /// <inheritdoc cref="SubImage(long, long, long, long)"/>
        public PixImage<T> SubImage(Box2i box) => SubImage(box.Min, box.Size);

        /// <summary>
        /// Returns a square region around the center as new PixImage.
        /// <paramref name="squareRadius"/> is the given radius around the center.
        /// No data is copied, the internal <see cref="Volume{T}"/> is only a view of the original one.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if the region is out of bounds.</exception>
        public PixImage<T> SubImage(V2i center, int squareRadius)
            => SubImage(center - squareRadius, V2i.II * squareRadius * 2 + 1);

        /// <summary>
        /// Returns a specified region as a new PixImage.
        /// The supplied offset and size are rounded to the nearest integer.
        /// Note that this may be different from <see cref="SubImage(Box2d)"/>, since
        /// rounding the size and rounding the Max of the box may
        /// result in different integer sizes.
        /// No data is copied, the internal <see cref="Volume{T}"/> is only a view onf the original one.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if the region is out of bounds.</exception>
        public PixImage<T> SubImage(V2d offset, V2d size) => SubImage(new V2i(offset + 0.5), new V2i(size + 0.5));

        /// <summary>
        /// Returns a specified region as a new PixImage.
        /// Min and Max of the supplied box are rounded to the nearest integer.
        /// Note that this may be different from <see cref="SubImage(V2d, V2d)"/>,
        /// since rounding the Max of the box and rounding the size may
        /// result in different integer sizes.
        /// No data is copied, the internal <see cref="Volume{T}"/> is only a view onf the original one.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if the region is out of bounds.</exception>
        public PixImage<T> SubImage(Box2d box)
        {
            var min = new V2i(box.Min + 0.5);
            return SubImage(min, new V2i(box.Max + 0.5) - min);
        }

        /// <summary>
        /// Coords in normalized [0, 1] coords of 'this'.
        /// The supplied pos and size are converted to pixel coordinates and
        /// rounded to the nearest integer. Note that this may be different
        /// from <see cref="SubImage01(Box2d)"/> since rounding the size and rounding
        /// the Max of the box may result in different integer sizes.
        /// No data is copied, the internal <see cref="Volume{T}"/> is only a view onf the original one.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if the region is out of bounds.</exception>
        public PixImage<T> SubImage01(V2d offset, V2d size)
        {
            var iPos = (V2i)(offset * (V2d)Size + 0.5);
            var iSize = (V2i)(size * (V2d)Size + 0.5);
            return SubImage(iPos, iSize);
        }

        /// <summary>
        /// Coords in normalized [0, 1] coords of 'this'.
        /// Min and Max of the supplied box are converted to pixel coordinates
        /// and rounded to the nearest integer. Note that this may be different
        /// from <see cref="SubImage01(V2d, V2d)"/>, since rounding the Max of the
        /// box and rounding the size may result in different integer sizes.
        /// No data is copied, the internal <see cref="Volume{T}"/> is only a view onf the original one.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if the region is out of bounds.</exception>
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
        /// <exception cref="ArgumentException">if the format of <paramref name="image"/> does not match the format of this instance.</exception>
        /// <exception cref="ArgumentOutOfRangeException">if the region is out of bounds.</exception>
        public void Set(int x, int y, PixImage<T> image) => Set(new V2i(x, y), image);

        /// <inheritdoc cref="Set(int, int, PixImage{T})"/>
        public void Set(V2i offset, PixImage<T> image) => SubImage(offset, image.Size).Set(image);

        /// <summary>
        /// Set the image contents of the image to the contents of another image.
        /// </summary>
        /// <exception cref="ArgumentException">if the format of <paramref name="image"/> does not match the format of this instance.</exception>
        /// <exception cref="ArgumentException">if the size of <paramref name="image"/> does not match the size of this instance.</exception>
        public void Set(PixImage<T> image)
        {
            if (Format != image.Format) throw new ArgumentException($"Format mismatch. Expected {Format} but got {image.Format}.");
            if (Size != image.Size) throw new ArgumentException($"Size mismatch. Expected  {Size} but got {image.Size}.");
            Volume.Set(image.Volume);
        }

        /// <summary>
        /// Set the image contents of the image to the contents of another image.
        /// The contents of the other image are resized to match.
        /// </summary>
        /// <exception cref="ArgumentException">if the format of <paramref name="image"/> does not match the format of this instance.</exception>
        public void SetResized(PixImage<T> image, ImageInterpolation interpolation = ImageInterpolation.Cubic)
        {
            if (Format != image.Format) throw new ArgumentException($"Format mismatch. Expected {Format} but got {image.Format}.");
            Volume.Set(Size == image.Size ? image.Volume : image.Resized(Size, interpolation).Volume);
        }

        #endregion

        #region Conversions

        /// <inheritdoc cref="ToFormat"/>
        public override PixImage ToPixImage(Col.Format format) => ToFormat(format);

        /// <summary>
        /// Converts this image to the specified color format. If the current format already matches
        /// and the channel counts are equal, the current instance is returned; otherwise a new instance
        /// with converted channels is created.
        /// </summary>
        /// <param name="format">Target color format.</param>
        /// <returns>An image in the requested format.</returns>
        public PixImage<T> ToFormat(Col.Format format)
        {
            if (Format == format && ChannelCount == format.ChannelCount()) return this;
            return new PixImage<T>(format, this);
        }

        /// <inheritdoc cref="ToCanonicalDenseLayout" />
        public PixImage<T> ToImageLayout() => !Volume.HasImageLayout() ? new PixImage<T>(Format, this) : this;

        /// <inheritdoc />
        public override PixImage ToCanonicalDenseLayout() => ToImageLayout();

        #endregion

        #region Copy

        /// <inheritdoc />
        public override void CopyChannelTo<Tv>(long channelIndex, Matrix<Tv> target)
        {
            var subMatrix = GetChannel<Tv>(channelIndex);
            target.Set(subMatrix);
        }

        /// <inheritdoc />
        public override void CopyVolumeTo<Tv>(Volume<Tv> target)
        {
            if (Volume is Volume<Tv> source)
                target.Set(source);
            else
                target.Set(Volume.AsVolume<T, Tv>());
        }

        /// <summary>
        /// Creates a deep copy using canonical image layout regardless of the current layout.
        /// If already in image layout, the data is copied; otherwise, data is transformed to image layout.
        /// </summary>
        public PixImage<T> CopyToImageLayout()
        {
            return Volume.HasImageLayout() ? new PixImage<T>(Format, Volume.CopyToImage()) : new PixImage<T>(this);
        }

        /// <inheritdoc cref="CopyToPixImage" />
        public PixImage<T> Copy() => new(Format, Volume.CopyToImageWindow());

        /// <inheritdoc />
        public override PixImage CopyToPixImage() => Copy();

        /// <inheritdoc />
        public override PixImage CopyToPixImageWithCanonicalDenseLayout() => CopyToImageLayout();

        /// <summary>
        /// Applies a per-pixel conversion function and returns a new image in the same color format.
        /// </summary>
        /// <typeparam name="Tv">View type used for the conversion (e.g. a color type such as <see cref="C3f"/> or <see cref="C4ui"/>).</typeparam>
        /// <param name="mapping">Unary transformation applied to each pixel value.</param>
        /// <exception cref="NotSupportedException">if the view type is invalid for the format and element type.</exception>
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> mapping) => Copy(mapping, Format);

        /// <summary>
        /// Applies a per-pixel conversion function and returns a new image with the specified color format.
        /// </summary>
        /// <typeparam name="Tv">View type used for the conversion (e.g. a color type such as <see cref="C3f"/> or <see cref="C4ui"/>).</typeparam>
        /// <param name="mapping">Unary transformation applied to each pixel value.</param>
        /// <param name="format">Target color format.</param>
        /// <exception cref="NotSupportedException">if the view type is invalid for the format and element type.</exception>
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> mapping, Col.Format format)
        {
            var mat = GetMatrix<Tv>().MapWindow(mapping);
            var vol = new Volume<T>(mat.Data, Volume.Info);
            return new PixImage<T>(format, vol);
        }

        #endregion

        #region Obtaining Matrices

        /// <summary>
        /// Returns the specified channel using the canonical order (red, green, blue, alpha).
        /// </summary>
        /// <param name="channelIndex">Index within the canonical order of channels.</param>
        /// <returns>A matrix window over the selected channel.</returns>
        public Matrix<T> GetChannel(long channelIndex)
        {
            var order = Format.ChannelOrder();
            return GetChannelInFormatOrder(order[channelIndex]);
        }

        /// <summary>
        /// Returns the specified channel by semantic name according to the current format.
        /// </summary>
        /// <param name="channel">The channel to access (e.g. Red, Green, Blue, Alpha, Gray).</param>
        /// <returns>A matrix window over the selected channel.</returns>
        public Matrix<T> GetChannel(Col.Channel channel)
            => GetChannelInFormatOrder(Format.ChannelIndex(channel));

        /// <summary>
        /// Returns the specified channel (using the canonical order) with a different view type.
        /// </summary>
        /// <typeparam name="Tv">Element view type of the returned matrix.</typeparam>
        /// <param name="channelIndex">Index within the canonical order of channels.</param>
        /// <returns>A typed matrix window over the selected channel.</returns>
        /// <exception cref="NotSupportedException">if the element type and view type are incompatible.</exception>
        public Matrix<T, Tv> GetChannel<Tv>(long channelIndex)
            => GetChannelInFormatOrder<Tv>(Format.ChannelOrder()[channelIndex]);

        /// <summary>
        /// Returns the specified channel based on the order of the color format.
        /// </summary>
        /// <param name="formatChannelIndex">Index of the channel in the current format's order.</param>
        /// <returns>A matrix window over the selected channel.</returns>
        public Matrix<T> GetChannelInFormatOrder(long formatChannelIndex)
            => Volume.SubXYMatrixWindow(formatChannelIndex);

        /// <summary>
        /// Returns the specified channel based on the order of the color format with a different view type.
        /// </summary>
        /// <typeparam name="Tv">Element view type of the returned matrix.</typeparam>
        /// <param name="formatChannelIndex">Index of the channel in the current format's order.</param>
        /// <returns>A typed matrix window over the selected channel.</returns>
        /// <exception cref="NotSupportedException">if the element type and view type are incompatible.</exception>
        public Matrix<T, Tv> GetChannelInFormatOrder<Tv>(long formatChannelIndex)
        {
            var matrix = Volume.SubXYMatrix<Tv>(formatChannelIndex);
            matrix.Accessors = TensorAccessors.Get<T, Tv>(TensorAccessors.Intent.ColorChannel, Volume.DeltaArray);
            return matrix;
        }

        /// <summary>
        /// Reinterprets the underlying 3D image volume as a matrix of the specified type.
        /// </summary>
        /// <typeparam name="Tv">Element view type of the returned matrix (e.g. a color type such as <see cref="C3f"/> or <see cref="C4ui"/>).</typeparam>
        /// <returns>A typed matrix window over the whole 3D image volume.</returns>
        /// <exception cref="NotSupportedException">if the view type is invalid for the format and element type.</exception>
        public Matrix<T, Tv> GetMatrix<Tv>() => Volume.GetMatrix<T, Tv>(Format.GetIntent());

        #endregion

        #region IPixImageVisitor

        /// <inheritdoc/>
        public override TResult Visit<TResult>(IPixImageVisitor<TResult> visitor) => visitor.Visit(this);

        #endregion
    }
}

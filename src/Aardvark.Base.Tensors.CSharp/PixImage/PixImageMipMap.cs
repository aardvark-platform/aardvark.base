using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Aardvark.Base
{
    public class PixImageMipMap : IPix, IPixMipMap2d
    {
        public PixImage[] ImageArray;

        #region MipMapOptions

        public class MipMapOptions
        {
            public int Count;
            public bool PowerOfTwo;
            public ImageInterpolation Interpolation;

            public static MipMapOptions Default = new MipMapOptions()
            {
                Count = 0,
                Interpolation = ImageInterpolation.Cubic,
                PowerOfTwo = false,
            };
        }

        #endregion

        #region Constructors

        public PixImageMipMap() { }

        public PixImageMipMap(params PixImage[] images) { ImageArray = images; }

        #endregion

        #region Properties

        public int ImageCount { get { return ImageArray.Length; } }

        public IEnumerable<PixImage> Images { get { return ImageArray; } }

        #endregion

        #region Static Creators

        // Helper class to create PixImageMipMap from untyped PixImage
        private static class Dispatch
        {
            private static class CreateDispatcher
            {
                public static PixImageMipMap<T> Create<T>(PixImage<T> image, MipMapOptions options)
                    => new PixImageMipMap<T>(image, options);
            }

            private const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            private static readonly MethodInfo s_createMethod = typeof(CreateDispatcher).GetMethod("Create", flags);

            public static PixImageMipMap Create(PixImage image, MipMapOptions options)
            {
                var mi = s_createMethod.MakeGenericMethod(image.PixFormat.Type);
                return (PixImageMipMap)mi.Invoke(null, new object[] { image, options });
            }
        }

        public static IEnumerable<PixImageMipMap> Create(IEnumerable<PixImage> pixies, MipMapOptions options)
            => pixies.Select(pix => Dispatch.Create(pix, options));

        public static IEnumerable<PixImageMipMap> Create(IEnumerable<PixImage> pixies)
            => Create(pixies, MipMapOptions.Default);

        public static PixImageMipMap Create(PixImage pix)
            => Dispatch.Create(pix, MipMapOptions.Default);

        public static PixImageMipMap Create(PixImage pix, MipMapOptions options)
            => Dispatch.Create(pix, options);

        #region Load from file

        private static PixImageMipMap LoadMipmapFromFileWithLoader(IPixLoader loader, string filename)
        {
            if (loader is IPixMipmapLoader mipLoader)
            {
                return mipLoader.LoadMipmapFromFile(filename);
            }
            else
            {
                return new PixImageMipMap(loader.LoadFromFile(filename));
            }
        }

        private static PixImageMipMap TryLoadFromFileWithLoader(IPixLoader loader, string filename)
            => PixImage.TryInvokeLoader(
                    loader, l => LoadMipmapFromFileWithLoader(loader, filename), PixImage.NotNull,
                    $"load image mipmap from file '{filename}'"
            );

        /// <summary>
        /// Loads an image from the given file.
        /// </summary>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use first, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public static PixImageMipMap Load(string filename, IPixLoader loader = null)
            => PixImage.InvokeLoaders(
                    loader, filename, (l, f) => TryLoadFromFileWithLoader(l, f), PixImage.Ignore, PixImage.NotNull,
                    $"Could not load image mipmap from file '{filename}'"
            );

        /// <summary>
        /// Loads images from the given files.
        /// </summary>
        /// <param name="filenames">The image files to load.</param>
        /// <param name="loader">The loader to use first, or null if no specific loader is to be used.</param>
        /// <returns>The loaded images.</returns>
        /// <exception cref="ImageLoadException">if an image could not be loaded.</exception>
        [Obsolete]
        public static IEnumerable<PixImageMipMap> Load(IEnumerable<string> filenames, IPixLoader loader = null)
            => Load(filenames, MipMapOptions.Default, loader);

        /// <summary>
        /// Loads images from the given files.
        /// </summary>
        /// <param name="filenames">The image files to load.</param>
        /// <param name="options">Options determining if and how mip maps are generated.</param>
        /// <param name="loader">The loader to use first, or null if no specific loader is to be used.</param>
        /// <returns>The loaded images.</returns>
        /// <exception cref="ImageLoadException">if an image could not be loaded.</exception>
        [Obsolete]
        public static IEnumerable<PixImageMipMap> Load(IEnumerable<string> filenames, MipMapOptions options, IPixLoader loader = null)
            => Create(PixImage.Load(filenames, loader), options);

        [Obsolete("Use Load()")]
        public static PixImageMipMap Create(string filename)
        {
            return new PixImageMipMap(PixImage.Create(filename, PixLoadOptions.Default));
        }

        [Obsolete("Use Load()")]
        public static PixImageMipMap Create(string filename, PixLoadOptions options)
        {
            return new PixImageMipMap(PixImage.Create(filename, options));
        }

        [Obsolete("Use Load()")]
        public static IEnumerable<PixImageMipMap> Create(IEnumerable<string> filenames)
        {
            return Create(filenames, MipMapOptions.Default);
        }

        [Obsolete("Use Load()")]
        public static IEnumerable<PixImageMipMap> Create(IEnumerable<string> filenames, MipMapOptions options)
        {
            return Create(from filename in filenames select PixImage.Create(filename), options);
        }

        #endregion

        #region Load from stream

        private static PixImageMipMap LoadMipmapFromStreamWithLoader(IPixLoader loader, Stream stream)
        {
            if (loader is IPixMipmapLoader mipLoader)
            {
                return mipLoader.LoadMipmapFromStream(stream);
            }
            else
            {
                return new PixImageMipMap(loader.LoadFromStream(stream));
            }
        }

        private static PixImageMipMap TryLoadFromStreamWithLoader(IPixLoader loader, Stream stream)
            => PixImage.TryInvokeLoader(
                    loader, l => LoadMipmapFromStreamWithLoader(l, stream), PixImage.NotNull,
                    $"load image mipmap from {PixImage.GetStreamDescription(stream)}"
            );

        /// <summary>
        /// Loads an image from the given stream.
        /// </summary>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use first, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public static PixImageMipMap Load(Stream stream, IPixLoader loader = null)
            => PixImage.InvokeLoadersWithStream(
                loader, stream, (l, s) => TryLoadFromStreamWithLoader(l, s), PixImage.NotNull,
                $"Could not load image mipmap from {PixImage.GetStreamDescription(stream)}"
            );

        #endregion

        #endregion

        #region IPix

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixImageMipMap(this); }

        #endregion

        #region IPixMipMap2d

        public PixFormat PixFormat
        {
            get { if (ImageArray.IsEmptyOrNull()) new ArgumentException("PixMipMap is empty"); return ImageArray[0].PixFormat; }
        }

        public int LevelCount
        {
            get { return ImageArray.Length; }
        }

        IPixImage2d IPixMipMap2d.this[int level]
        {
            get
            {
                return ImageArray[level];
            }
        }

        public PixImage this[int level]
        {
            get { return ImageArray[level]; }
        }

        #endregion
    }

    public class PixImageMipMap<T> : PixImageMipMap
    {
        #region Properties

        new public IEnumerable<PixImage<T>> Images { get { return (PixImage<T>[])ImageArray; } }
        public IEnumerable<V2i> ImageSizes { get { return from image in ImageArray select image.Size; } }
        public V2i BaseImageSize { get { return ImageArray[0].Size; } }

        #endregion

        #region Constructors

        public PixImageMipMap(PixImage<T> singleImage)
        {
            ImageArray = new PixImage<T>[] { singleImage };
            BuildMipMaps(MipMapOptions.Default);
        }

        public PixImageMipMap(PixImage<T> singleImage, MipMapOptions options)
        {
            ImageArray = new PixImage<T>[] { singleImage };
            BuildMipMaps(options);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds mipmap pyramid using supplied MipMapOptions.
        /// Throws an exception if mips are already present (this function ensures correct MipMapOptions)
        /// </summary>
        private void BuildMipMaps(MipMapOptions options)
        {
            if (ImageArray == null || ImageArray.Length == 0)
                throw new ArgumentException("No valid PixImage available.");

            if (ImageArray.Length > 1)
                throw new ArgumentException("PixImageMipMap already contains image pyramid");

            var size = ImageArray[0].Size;

            if (options.PowerOfTwo && (!Fun.IsPowerOfTwo(size.X) || !Fun.IsPowerOfTwo(size.Y)))
            {
                var powerOfTwoSize = new V2i(size.X.NextPowerOfTwo(), size.Y.NextPowerOfTwo());
                ImageArray[0] = ((PixImage<T>)ImageArray[0]).Resized(powerOfTwoSize);
                Report.Line(2, "Resizing PixImage to size of power of two");
                size = powerOfTwoSize;
                //throw new ArgumentException("Can only build mip maps for PixImages with size of power of two");
            }

            var count = options.Count == 0
                ? Fun.MipmapLevels(size)
                : options.Count;

            var mipMaps = new PixImage<T>[count];
            mipMaps[0] = (PixImage<T>)ImageArray[0];

            for (int i = 1; i < count; i++)
            {
                size = Fun.MipmapLevelSize(size, 1);
                mipMaps[i] = mipMaps[i - 1].Resized(size, options.Interpolation);
            }

            ImageArray = mipMaps;
            //ImagesContainer.FromPixImageArray(m_images);
            //InfoOfSet = new Info(Kind.MipMapPyramid, m_images.Length, m_images.Select(x => x.Size).ToArray());
        }

        #endregion
    }
}

using System;
using System.IO;

namespace Aardvark.Base
{
    public class PixImageMipMap
    {
        /// <summary>
        /// Array of images representing the mipmap levels.
        /// </summary>
        public PixImage[] ImageArray;

        #region Constructors

        /// <summary>
        /// Creates an empty mipmap.
        /// </summary>
        public PixImageMipMap() => ImageArray = Array.Empty<PixImage>();

        /// <summary>
        /// Creates a mipmap from the given images.
        /// </summary>
        /// <param name="images">An array of images representing the mipmap levels.</param>
        public PixImageMipMap(params PixImage[] images) => ImageArray = images;

        /// <summary>
        /// Creates a mipmap consisting of a single image.
        /// </summary>
        /// <param name="image">The single image of the mipmap.</param>
        public PixImageMipMap(PixImage image) => ImageArray = new[] { image };

        #endregion

        #region Properties

        /// <summary>
        /// Number of images in the mipmap array.
        /// </summary>
        public int Count => ImageArray?.Length ?? 0;

        /// <summary>
        /// Returns the base image of the mipmap array or null if it is empty.
        /// </summary>
        public PixImage BaseImage => Count > 0 ? ImageArray[0] : null;

        /// <summary>
        /// Returns the size of the base image, or V2i.Zero if the mipmap array is empty.
        /// </summary>
        public V2i BaseSize => BaseImage?.Size ?? V2i.Zero;

        /// <summary>
        /// Returns the format of the mipmap.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the mipmap is empty.</exception>
        public PixFormat PixFormat => BaseImage?.PixFormat ?? throw new InvalidOperationException("Cannot determine format of empty PixImageMipMap.");

        /// <summary>
        /// Gets or sets an image of the mipmap array.
        /// </summary>
        public PixImage this[int index]
        {
            get => ImageArray[index];
            set => ImageArray[index] = value;
        }

        #region Obsolete

        [Obsolete("Use Count instead.")]
        public int LevelCount => Count;

        [Obsolete("Use Count instead.")]
        public int ImageCount => Count;

        [Obsolete("Use ImageArray instead.")]
        public PixImage[] Images => ImageArray;

        [Obsolete("Use BaseSize instead.")]
        public V2i BaseImageSize => BaseSize;

        #endregion

        #endregion

        #region Load from file

        private static PixImageMipMap LoadMipmapFromFileWithLoader(IPixLoader loader, string filename)
        {
            if (loader is IPixMipmapLoader mipLoader)
            {
                return mipLoader.LoadMipmapFromFile(filename);
            }
            else
            {
                var pi = loader.LoadFromFile(filename);
                return (pi != null) ? new PixImageMipMap(pi) : null;
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
                var pi = loader.LoadFromStream(stream);
                return (pi != null) ? new PixImageMipMap(pi) : null;
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

        #region Generation

        /// <summary>
        /// Builds a mipmap for the given image.
        /// </summary>
        /// <param name="image">The base image of the mipmap.</param>
        /// <param name="maxCount">The maximum number of levels to generate. Ignored if non-positive.</param>
        /// <param name="interpolation">The used interpolation method.</param>
        /// <param name="powerOfTwo">If true, the base image is resized to have power-of-two dimensions.</param>
        /// <returns></returns>
        public static PixImageMipMap Create(PixImage image, ImageInterpolation interpolation = ImageInterpolation.Cubic, int maxCount = 0, bool powerOfTwo = false)
        {
            var baseSize = image.Size;
            var baseImage = image;

            if (powerOfTwo && (!Fun.IsPowerOfTwo(baseSize.X) || !Fun.IsPowerOfTwo(baseSize.Y)))
            {
                baseSize = Fun.NextPowerOfTwo(baseSize);
                baseImage = image.ResizedPixImage(baseSize, interpolation);
            }

            var count = Fun.MipmapLevels(baseSize);
            if (maxCount > 0 && count > maxCount) count = maxCount;

            var images = new PixImage[count];
            images[0] = baseImage;

            for (var i = 1; i < count; i++)
            {
                var size = Fun.MipmapLevelSize(baseSize, i);
                images[i] = images[i - 1].ResizedPixImage(size, interpolation);
            }

            return new PixImageMipMap(images);
        }

        #endregion
    }
}

using System;
using System.IO;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents a collection of downsampled images (mipmap levels) derived from a base image.
    /// A mipmap typically contains the original image at level 0 and successively smaller images
    /// at higher levels, which can be used for efficient rendering, filtering, and sampling.
    /// </summary>
    public class PixImageMipMap : IPix
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
        public int LevelCount => ImageArray?.Length ?? 0;

        /// <summary>
        /// Returns the base image of the mipmap array or null if it is empty.
        /// </summary>
        public PixImage BaseImage => LevelCount > 0 ? ImageArray[0] : null;

        /// <summary>
        /// Returns the size of the base image, or V2i.Zero if the mipmap array is empty.
        /// </summary>
        public V2i BaseSize => BaseImage?.Size ?? V2i.Zero;

        /// <summary>
        /// Gets the pixel format of the base image.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the mipmap is empty.</exception>
        public PixFormat PixFormat => BaseImage?.PixFormat ?? throw new InvalidOperationException("Cannot determine format of empty PixImageMipMap.");

        /// <summary>
        /// Gets or sets an image at the specified mipmap level.
        /// </summary>
        /// <param name="index">Zero-based mipmap level index, where 0 is the base image.</param>
        /// <returns>The image at the requested mipmap level.</returns>
        /// <exception cref="IndexOutOfRangeException">if <paramref name="index"/> is outside the valid range [0, <see cref="LevelCount"/>-1].</exception>
        public PixImage this[int index]
        {
            get => ImageArray[index];
            set => ImageArray[index] = value;
        }

        #region Obsolete

        [Obsolete("Use LevelCount instead.")]
        public int ImageCount => LevelCount;

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

            var pi = loader.LoadFromFile(filename);
            return (pi != null) ? new PixImageMipMap(pi) : null;
        }

        private static PixImageMipMap LoadFromFileWithLoader(IPixLoader loader, string filename)
            => PixImage.InvokeLoader(
                    loader, l => LoadMipmapFromFileWithLoader(l, filename), PixImage.NotNull,
                    $"load image mipmap from file '{filename}'"
            );

        /// <summary>
        /// Loads an image mipmap from the given file.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="filename">The image file to load.</param>
        /// <param name="loader">The loader to use first, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image mipmap.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        public static PixImageMipMap Load(string filename, IPixLoader loader = null)
            => PixImage.InvokeLoaders(
                    PixImage.LoaderType.Decoder, loader, filename, LoadFromFileWithLoader, PixImage.Ignore, PixImage.NotNull,
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

        private static PixImageMipMap LoadFromStreamWithLoader(IPixLoader loader, Stream stream)
            => PixImage.InvokeLoader(
                    loader, l => LoadMipmapFromStreamWithLoader(l, stream), PixImage.NotNull,
                    $"load image mipmap from {PixImage.GetStreamDescription(stream)}"
            );

        /// <summary>
        /// Loads an image mipmap from the given stream.
        /// </summary>
        /// <remarks>
        /// Install image loaders by referencing Aardvark.PixImage.* packages or by
        /// manually calling <see cref="PixImage.AddLoader"/> or <see cref="PixImage.SetLoader"/>.
        /// </remarks>
        /// <param name="stream">The image stream to load.</param>
        /// <param name="loader">The loader to use first, or null if no specific loader is to be used.</param>
        /// <returns>The loaded image mipmap.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        /// <exception cref="NotSupportedException">if the stream is not seekable and multiple loaders are invoked.</exception>
        public static PixImageMipMap Load(Stream stream, IPixLoader loader = null)
            => PixImage.InvokeLoadersWithStream(
                PixImage.LoaderType.Decoder, loader, stream, LoadFromStreamWithLoader, PixImage.NotNull,
                $"Could not load image mipmap from {PixImage.GetStreamDescription(stream)}"
            );

        #endregion

        #region Generation

        /// <summary>
        /// Builds a full mipmap chain for the given image, optionally resizing to power-of-two dimensions
        /// and/or limiting the number of levels.
        /// </summary>
        /// <param name="image">The base image of the mipmap (level 0).</param>
        /// <param name="interpolation">The interpolation method used when downsampling between levels.</param>
        /// <param name="maxCount">Optional maximum number of mipmap levels to generate. If less than or equal to 0, all valid levels are generated.</param>
        /// <param name="powerOfTwo">If true and the base image size is not already a power of two, the image is resized to the next power-of-two size before building the chain.</param>
        /// <returns>A new <see cref="PixImageMipMap"/> where index 0 contains the (possibly resized) base image and higher indices contain successively downsampled images until the minimum size is reached or <paramref name="maxCount"/> is satisfied.</returns>
        public static PixImageMipMap Create(PixImage image, ImageInterpolation interpolation = ImageInterpolation.Linear, int maxCount = 0, bool powerOfTwo = false)
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

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Aardvark.Base
{
    /// <summary>
    /// Extension methods for common PixImage conversions and utilities.
    /// </summary>
    public static class PixImageExtensions
    {
        #region Black and White Conversions

        /// <summary>
        /// Converts a single-channel byte image to a black and white matrix using the specified threshold.
        /// </summary>
        /// <param name="pixImage">The source image (must have one channel).</param>
        /// <param name="threshold">Threshold in [0,255]; below yields 0, above or equal yields 255.</param>
        /// <returns>A thresholded matrix with values 0 or 255.</returns>
        public static Matrix<byte> ToBlackAndWhiteMatrix(this PixImage<byte> pixImage, int threshold)
        {
            if (pixImage.ChannelCount != 1) throw new ArgumentOutOfRangeException(nameof(pixImage.ChannelCount));
            return pixImage.GetChannel(Col.Channel.Gray).MapWindow(b => (byte)(b < threshold ? 0 : 255));
        }

        /// <summary>
        /// Converts a single-channel byte image to a black and white matrix using a default threshold of 128.
        /// </summary>
        /// <param name="pixImage">The source image (must have one channel).</param>
        /// <returns>A thresholded matrix with values 0 or 255.</returns>
        public static Matrix<byte> ToBlackAndWhiteMatrix(this PixImage<byte> pixImage)
        {
            return pixImage.ToBlackAndWhiteMatrix(128);
        }

        /// <summary>
        /// Converts a single-channel byte image to a black and white PixImage using the specified threshold.
        /// </summary>
        /// <param name="pixImage">The source image (must have one channel).</param>
        /// <param name="threshold">Threshold in [0,255]; below yields 0, above or equal yields 255.</param>
        /// <returns>A new black and white PixImage.</returns>
        public static PixImage<byte> ToBlackAndWhitePixImage(this PixImage<byte> pixImage, int threshold)
        {
            return new PixImage<byte>(Col.Format.BW, pixImage.ToBlackAndWhiteMatrix(threshold));
        }

        /// <summary>
        /// Converts a single-channel byte image to a black and white PixImage using a default threshold of 128.
        /// </summary>
        /// <param name="pixImage">The source image (must have one channel).</param>
        /// <returns>A new black and white PixImage.</returns>
        public static PixImage<byte> ToBlackAndWhitePixImage(this PixImage<byte> pixImage)
        {
            return new PixImage<byte>(Col.Format.BW, pixImage.ToBlackAndWhiteMatrix());
        }

        #endregion

        #region Grayscale Conversions

        /// <summary>
        /// Converts a byte PixImage to a single-channel grayscale matrix.
        /// If the image already has a single channel in BW or Gray format, the channel is returned as-is.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale matrix of bytes.</returns>
        public static Matrix<byte> ToGrayscaleMatrix(this PixImage<byte> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format is Col.Format.BW or Col.Format.Gray) return pixImage.Volume.AsMatrixWindow();
            return pixImage.GetMatrix<C3b>().MapWindow(Col.ToGrayByte);
        }

        /// <summary>
        /// Converts a byte PixImage to a single-channel grayscale PixImage.
        /// If the image already has a single channel in Gray format, it is returned unmodified.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale PixImage of bytes.</returns>
        public static PixImage<byte> ToGrayscalePixImage(this PixImage<byte> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray) return pixImage;
            return new PixImage<byte>(pixImage.ToGrayscaleMatrix());
        }

        /// <summary>
        /// Converts an ushort PixImage to a single-channel grayscale matrix.
        /// If the image already has a single channel in BW or Gray format, the channel is returned as-is.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale matrix of ushorts.</returns>
        public static Matrix<ushort> ToGrayscaleMatrix(this PixImage<ushort> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format is Col.Format.BW or Col.Format.Gray) return pixImage.Volume.AsMatrixWindow();
            return pixImage.GetMatrix<C3us>().MapWindow(Col.ToGrayUShort);
        }

        /// <summary>
        /// Converts an ushort PixImage to a single-channel grayscale PixImage.
        /// If the image already has a single channel in Gray format, it is returned unmodified.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale PixImage of ushorts.</returns>
        public static PixImage<ushort> ToGrayscalePixImage(this PixImage<ushort> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray) return pixImage;
            return new PixImage<ushort>(pixImage.ToGrayscaleMatrix());
        }

        /// <summary>
        /// Converts a float PixImage to a single-channel grayscale matrix.
        /// If the image already has a single channel in BW or Gray format, the channel is returned as-is.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale matrix of floats.</returns>
        public static Matrix<float> ToGrayscaleMatrix(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format is Col.Format.BW or Col.Format.Gray) return pixImage.Volume.AsMatrixWindow();
            return pixImage.GetMatrix<C3f>().MapWindow(Col.ToGrayFloat);
        }

        /// <summary>
        /// Converts a float PixImage to a single-channel grayscale PixImage.
        /// If the image already has a single channel in Gray format, it is returned unmodified.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale PixImage of floats.</returns>
        public static PixImage<float> ToGrayscalePixImage(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray) return pixImage;
            return new PixImage<float>(pixImage.ToGrayscaleMatrix());
        }

        /// <summary>
        /// Converts a float PixImage to a single-channel grayscale matrix and clamps the result to [0,1].
        /// If the image already has a single channel in BW or Gray format, the channel is returned as-is.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale matrix of floats in [0,1].</returns>
        public static Matrix<float> ToClampedGrayscaleMatrix(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format is Col.Format.BW or Col.Format.Gray) return pixImage.Volume.AsMatrixWindow();
            return pixImage.GetMatrix<C3f>().MapWindow(Col.ToGrayFloatClamped);
        }

        /// <summary>
        /// Converts a float PixImage to a single-channel grayscale PixImage with values clamped to [0,1].
        /// If the image already has a single channel in Gray format, it is returned unmodified.
        /// </summary>
        /// <param name="pixImage">The source image.</param>
        /// <returns>A grayscale PixImage of floats in [0,1].</returns>
        public static PixImage<float> ToClampedGrayscalePixImage(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray) return pixImage;
            return new PixImage<float>(pixImage.ToClampedGrayscaleMatrix());
        }

        #endregion

        #region Inversion

        /// <summary>
        /// Creates a new image with each pixel value inverted as <c>255 - value</c>.
        /// </summary>
        /// <param name="pixImage">The source image; it is not modified.</param>
        /// <returns>A new PixImage containing the inverted pixel values.</returns>
        public static PixImage<byte> Inverted(this PixImage<byte> pixImage)
        {
            return new PixImage<byte>(pixImage.Volume.MapToImageWindow(b => (byte)(255 - b)));
        }

        /// <summary>
        /// Creates a new image with each pixel value inverted as <c>0xFFFF - value</c>.
        /// </summary>
        /// <param name="pixImage">The source image; it is not modified.</param>
        /// <returns>A new PixImage containing the inverted pixel values.</returns>
        public static PixImage<ushort> Inverted(this PixImage<ushort> pixImage)
        {
            return new PixImage<ushort>(pixImage.Volume.MapToImageWindow(us => (ushort)(65535 - us)));
        }

        /// <summary>
        /// Creates a new image with each pixel value inverted as <c>0xFFFFFFFF - value</c>.
        /// </summary>
        /// <param name="pixImage">The source image; it is not modified.</param>
        /// <returns>A new PixImage containing the inverted pixel values.</returns>
        public static PixImage<uint> Inverted(this PixImage<uint> pixImage)
        {
            return new PixImage<uint>(pixImage.Volume.MapToImageWindow(ui => 0xffffffffu - ui));
        }

        /// <summary>
        /// Creates a new image with each pixel value inverted as <c>1 - value</c>.
        /// </summary>
        /// <remarks>
        /// Values are assumed to be normalized to [0,1]; this method does not clamp out-of-range values.
        /// </remarks>
        /// <param name="pixImage">The source image; it is not modified.</param>
        /// <returns>A new PixImage containing the inverted pixel values.</returns>
        public static PixImage<float> Inverted(this PixImage<float> pixImage)
        {
            return new PixImage<float>(pixImage.Volume.MapToImageWindow(f => 1.0f - f));
        }

        /// <summary>
        /// Creates a new image with each pixel value inverted as <c>1 - value</c>.
        /// </summary>
        /// <remarks>
        /// Values are assumed to be normalized to [0,1]; this method does not clamp out-of-range values.
        /// </remarks>
        /// <param name="pixImage">The source image; it is not modified.</param>
        /// <returns>A new PixImage containing the inverted pixel values.</returns>
        public static PixImage<double> Inverted(this PixImage<double> pixImage)
        {
            return new PixImage<double>(pixImage.Volume.MapToImageWindow(d => 1.0 - d));
        }

        /// <summary>
        /// Inverts pixel values in place as <c>255 - value</c>. No copy is created.
        /// </summary>
        /// <param name="pixImage">The image to invert; the same instance is returned.</param>
        /// <returns>The same instance after in-place inversion.</returns>
        public static PixImage<byte> Invert(this PixImage<byte> pixImage)
        {
            pixImage.Volume.Apply(b => (byte)(255 - b));
            return pixImage;
        }

        /// <summary>
        /// Inverts pixel values in place as <c>0xFFFF - value</c>. No copy is created.
        /// </summary>
        /// <param name="pixImage">The image to invert; the same instance is returned.</param>
        /// <returns>The same instance after in-place inversion.</returns>
        public static PixImage<ushort> Invert(this PixImage<ushort> pixImage)
        {
            pixImage.Volume.Apply(us => (ushort)(65535 - us));
            return pixImage;
        }

        /// <summary>
        /// Inverts pixel values in place as <c>0xFFFFFFFF - value</c>. No copy is created.
        /// </summary>
        /// <param name="pixImage">The image to invert; the same instance is returned.</param>
        /// <returns>The same instance after in-place inversion.</returns>
        public static PixImage<uint> Invert(this PixImage<uint> pixImage)
        {
            pixImage.Volume.Apply(ui => 0xffffffffu - ui);
            return pixImage;
        }

        /// <summary>
        /// Inverts pixel values in place as <c>1 - value</c>. No copy is created.
        /// </summary>
        /// <remarks>
        /// Values are assumed to be normalized to [0,1]; this method does not clamp out-of-range values.
        /// </remarks>
        /// <param name="pixImage">The image to invert; the same instance is returned.</param>
        /// <returns>The same instance after in-place inversion.</returns>
        public static PixImage<float> Invert(this PixImage<float> pixImage)
        {
            pixImage.Volume.Apply(f => 1.0f - f);
            return pixImage;
        }

        /// <summary>
        /// Inverts pixel values in place as <c>1 - value</c>. No copy is created.
        /// </summary>
        /// <remarks>
        /// Values are assumed to be normalized to [0,1]; this method does not clamp out-of-range values.
        /// </remarks>
        /// <param name="pixImage">The image to invert; the same instance is returned.</param>
        /// <returns>The same instance after in-place inversion.</returns>
        public static PixImage<double> Invert(this PixImage<double> pixImage)
        {
            pixImage.Volume.Apply(d => 1.0 - d);
            return pixImage;
        }

        #endregion

        #region Stitching

        #region  Dispatch

        private static class Dispatch
        {
            private delegate PixImage StitchDelegate(PixImage[][] images, Col.Format format);
            private delegate PixImage StitchSquareDelegate(PixImage[] images, Col.Format format);

            private static class StitchDispatcher
            {
                public static PixImage Stitch<T>(PixImage[][] images, Col.Format format)
                    => images.Map(ia => ia.Map(pi => pi?.ToPixImage<T>(format))).Stitch();

                public static PixImage StitchSquare<T>(PixImage[] images, Col.Format format)
                    => images.Map(pi => pi?.ToPixImage<T>(format)).StitchSquare();
            }

            private const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            private static readonly MethodInfo s_stitchMethod = typeof(StitchDispatcher).GetMethod(nameof(StitchDispatcher.Stitch), flags);
            private static readonly ConcurrentDictionary<Type, StitchDelegate> s_stitchDelegates = new();

            private static readonly MethodInfo s_stitchSquareMethod = typeof(StitchDispatcher).GetMethod(nameof(StitchDispatcher.StitchSquare), flags);
            private static readonly ConcurrentDictionary<Type, StitchSquareDelegate> s_stitchSquareDelegates = new();

            public static PixImage Stitch(PixImage[][] images)
            {
                var first = images.SelectMany(ia => ia).WhereNotNull().First();

                var stitch = s_stitchDelegates.GetOrAdd(first.PixFormat.Type, t => {
                    var mi = s_stitchMethod.MakeGenericMethod(t);
                    return (StitchDelegate)Delegate.CreateDelegate(typeof(StitchDelegate), mi);
                });

                return stitch(images, first.PixFormat.Format);
            }

            public static PixImage StitchSquare(PixImage[] images)
            {
                var first = images.WhereNotNull().First();

                var stitch = s_stitchSquareDelegates.GetOrAdd(first.PixFormat.Type, t => {
                    var mi = s_stitchSquareMethod.MakeGenericMethod(t);
                    return (StitchSquareDelegate)Delegate.CreateDelegate(typeof(StitchSquareDelegate), mi);
                });

                return stitch(images, first.PixFormat.Format);
            }
        }

        #endregion

        /// <summary>
        /// Stitches the images provided in the 2D jagged array into one large image according to their positions.
        /// The outer array enumerates rows (Y), the inner arrays enumerate columns (X).
        /// For example, the array<br/>
        /// {{1, 2, 3}}<br/>
        /// {{4, 5, 6}}<br/>
        /// produces the image<br/>
        /// 1 2 3<br/>
        /// 4 5 6<br/>
        /// Null entries are allowed and result in black gaps at the corresponding positions.
        /// All images must share the same color format.
        /// </summary>
        /// <param name="images">The 2D image array that should be stitched.</param>
        /// <returns>The stitched image, or null if the input is null or empty.</returns>
        public static PixImage<T> Stitch<T>(this PixImage<T>[][] images)
        {
            if (images.IsEmptyOrNull()) return null;

            // Find largest extensions
            var numRows = images.Length;
            var numColumns = images.Max(img => img.Length);

            // Find largest X Y
            var sizesX = new int[numColumns].SetByIndex(c => images.Max(ic => (ic[c] == null) ? 0 : ic[c].Size.X));
            var sizesY = images.Map(img => img.Max(img2 => (img2 == null) ? 0 : img2.Size.Y));
            var width = sizesX.Sum();
            var height = sizesY.Sum();

            // The first image in the array, used to determine the color format
            var fst = images.First(img => img.Length > 0).First(img => img != null);

            // Assure that the color format fits
            if (images.TrueForAny(imgs => imgs.Where(i => i != null).TrueForAny(img => img.Format != fst.Format)))
            {
                throw new ArgumentException("Mismatching color formats.");
            }

            // Allocate new image
            var target = new PixImage<T>(fst.Format, new V2i(width, height));

            for (int r = 0, y = 0; r < numRows; r++)
            {
                for (int c = 0, x = 0; c < Fun.Max(numColumns, images[r].Length); c++)
                {
                    var source = images[r][c];
                    if (source != null)
                        target.Set(x, y, source);
                    x += sizesX[c];
                }
                y += sizesY[r];
            }
            return target;
        }

        /// <summary>
        /// Stitches the images provided in the 2D jagged array into one large image according to their positions.
        /// The outer array enumerates rows (Y), the inner arrays enumerate columns (X).
        /// For example, the array<br/>
        /// {{1, 2, 3}}<br/>
        /// {{4, 5, 6}}<br/>
        /// produces the image<br/>
        /// 1 2 3<br/>
        /// 4 5 6<br/>
        /// Null entries are allowed and result in black gaps at the corresponding positions.
        /// All images must share the same color format. The pixel type will be inferred from the first non-null image.
        /// </summary>
        /// <param name="images">The 2D image array that should be stitched.</param>
        /// <returns>The stitched image.</returns>
        public static PixImage Stitch(this PixImage[][] images)
            => images.IsEmptyOrNull() ? null : Dispatch.Stitch(images);

        /// <summary>
        /// Stitches the provided images into a new image arranged in a near-square grid.
        /// All images must share the same color format. If the last row is incomplete, missing cells are left black.
        /// </summary>
        /// <example>
        /// {1, 2} -> {1, 2}<br/>
        /// {1, 2, 3} -> {1, 2}, {3}<br/>
        /// {1, 2, 3, 4, 5} -> {1, 2, 3}, {4, 5}<br/>
        /// </example>
        /// <param name="images">The image array that should be stitched.</param>
        /// <returns>The stitched image, or null if the input is empty or null.</returns>
        public static PixImage<T> StitchSquare<T>(this PixImage<T>[] images)
        {
            if (images.IsEmptyOrNull()) return null;

            var squareSize = (int)Fun.Ceiling(Fun.Sqrt(images.Length));
            var array =
                new PixImage<T>[squareSize][].SetByIndex(
                    row => new PixImage<T>[squareSize].SetByIndex(
                        col => { var ii = squareSize * row + col; return ii < images.Length ? images[ii] : null; }
                    )
                );
            return array.Stitch();
        }

        /// <summary>
        /// Stitches the provided images into a new image arranged in a near-square grid.
        /// All images must share the same color format. If the last row is incomplete, missing cells are left black.
        /// The pixel type will be inferred from the first non-null image.
        /// </summary>
        /// <example>
        /// {1, 2} -> {1, 2}<br/>
        /// {1, 2, 3} -> {1, 2}, {3}<br/>
        /// {1, 2, 3, 4, 5} -> {1, 2, 3}, {4, 5}<br/>
        /// </example>
        /// <param name="images">The image array that should be stitched.</param>
        /// <returns>The stitched image, or null if the input is empty or null.</returns>
        public static PixImage StitchSquare(this PixImage[] images)
            => images.IsEmptyOrNull() ? null : Dispatch.StitchSquare(images);

        #endregion
    }
}

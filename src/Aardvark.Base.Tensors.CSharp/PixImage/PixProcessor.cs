using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// Describes the feature set supported by an <see cref="IPixProcessor"/> implementation.
    /// </summary>
    [Flags]
    public enum PixProcessorCaps
    {
        /// <summary>
        /// No capabilities are supported.
        /// </summary>
        None = 0,

        /// <summary>
        /// Supports scaling operations.
        /// </summary>
        Scale = 1,

        /// <summary>
        /// Supports rotation operations.
        /// </summary>
        Rotate = 2,

        /// <summary>
        /// Supports generic geometric remapping operations.
        /// </summary>
        Remap = 4,

        /// <summary>
        /// Convenience value for enabling all capabilities (<see cref="Scale"/>, <see cref="Rotate"/>, <see cref="Remap"/>).
        /// </summary>
        All = Scale | Rotate | Remap,
    }

    /// <summary>
    /// Defines a pluggable image processor that can perform common operations
    /// such as scaling, rotating, and remapping pixel images.
    /// </summary>
    /// <remarks>
    /// Implementations may not support all operations for all pixel formats. In such cases, methods
    /// are expected to return <c>null</c> to indicate that the specific operation is not available.
    /// Check <see cref="Capabilities"/> before invoking an operation to avoid unnecessary work.
    /// </remarks>
    /// <seealso cref="PixProcessor"/>
    /// <seealso cref="LegacyPixProcessor"/>
    public interface IPixProcessor
    {
        /// <summary>
        /// Gets the human-readable name of the processor implementation.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the set of capabilities supported by this processor.
        /// </summary>
        PixProcessorCaps Capabilities { get; }

        /// <summary>
        /// Scales the given image by the provided factors.
        /// </summary>
        /// <typeparam name="T">The pixel component type of the image.</typeparam>
        /// <param name="image">The image to scale. Must not be <c>null</c>.</param>
        /// <param name="scaleFactor">The scale factor to apply in X and Y (1.0 keeps the size).</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <returns>
        /// A new <see cref="PixImage{T}"/> containing the scaled image, or <c>null</c> if scaling is not supported.
        /// </returns>
        PixImage<T> Scale<T>(PixImage<T> image, V2d scaleFactor, ImageInterpolation interpolation);

        /// <summary>
        /// Rotates an image counter-clockwise by the given angle in radians.
        /// </summary>
        /// <typeparam name="T">The pixel component type of the image.</typeparam>
        /// <param name="image">The image to rotate. Must not be <c>null</c>.</param>
        /// <param name="angleInRadians">The rotation angle in radians (counter-clockwise).</param>
        /// <param name="resize">When <c>true</c>, the output image is resized to fully contain the rotated content; otherwise it keeps the original size.</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <param name="borderType">How to evaluate pixels that sample outside the source image.</param>
        /// <param name="border">Border value used when <paramref name="borderType"/> is <see cref="ImageBorderType.Const"/>.</param>
        /// <returns>
        /// A new <see cref="PixImage{T}"/> containing the rotated image, or <c>null</c> if rotation is not supported.
        /// </returns>
        PixImage<T> Rotate<T>(PixImage<T> image, double angleInRadians, bool resize, ImageInterpolation interpolation,
                              ImageBorderType borderType = ImageBorderType.Const,
                              T border = default);

        /// <summary>
        /// Applies a generic geometric transformation to the given image by remapping destination
        /// pixel coordinates to source coordinates.
        /// </summary>
        /// <remarks>
        /// The resulting image is computed as f(x, y) = <paramref name="image"/>[<paramref name="mapX"/>(x, y), <paramref name="mapY"/>(x, y)].
        /// </remarks>
        /// <typeparam name="T">The pixel component type of the image.</typeparam>
        /// <param name="image">The image to transform. Must not be <c>null</c>.</param>
        /// <param name="mapX">Matrix of X-coordinate samples mapping destination pixels to source X.</param>
        /// <param name="mapY">Matrix of Y-coordinate samples mapping destination pixels to source Y.</param>
        /// <param name="interpolation">The interpolation method to use during resampling.</param>
        /// <param name="borderType">How to evaluate pixels that sample outside the source image.</param>
        /// <param name="border">Border value used when <paramref name="borderType"/> is <see cref="ImageBorderType.Const"/>.</param>
        /// <returns>
        /// A new <see cref="PixImage{T}"/> containing the remapped image, or <c>null</c> if remapping is not supported.
        /// </returns>
        PixImage<T> Remap<T>(PixImage<T> image, Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation,
                             ImageBorderType borderType = ImageBorderType.Const,
                             T border = default);
    }

    /// <summary>
    /// Basic image processor using built-in Aardvark algorithms.
    /// </summary>
    /// <remarks>
    /// Currently only scaling is implemented; rotation and remapping return <c>null</c>.
    /// </remarks>
    /// <seealso cref="IPixProcessor"/>
    public sealed class PixProcessor : IPixProcessor
    {
        /// <inheritdoc/>
        public string Name => "Aardvark";

        /// <inheritdoc/>
        public PixProcessorCaps Capabilities => PixProcessorCaps.Scale;

        /// <inheritdoc/>
        public PixImage<T> Scale<T>(PixImage<T> image, V2d scaleFactor, ImageInterpolation interpolation)
            => new(image.Format, TensorExtensions.Scaled(image.Volume, scaleFactor, interpolation));

        /// <inheritdoc/>
        public PixImage<T> Rotate<T>(PixImage<T> image, double angleInRadians, bool resize, ImageInterpolation interpolation,
                                     ImageBorderType borderType = ImageBorderType.Const,
                                     T border = default)
            => null;

        /// <inheritdoc/>
        public PixImage<T> Remap<T>(PixImage<T> image, Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation,
                                    ImageBorderType borderType = ImageBorderType.Const,
                                    T border = default)
            => null;

        private PixProcessor() { }

        private static readonly Lazy<PixProcessor> _instance = new(() => new PixProcessor());

        /// <summary>
        /// Gets the singleton instance of the default <see cref="PixProcessor"/>.
        /// </summary>
        public static PixProcessor Instance { get; } = _instance.Value;
    }

    /// <summary>
    /// Image processor that maintains compatibility with a legacy API by allowing
    /// clients to register per-type delegate implementations for the various operations.
    /// </summary>
    /// <remarks>
    /// If no delegate is registered for a given pixel type and operation, the corresponding method
    /// returns <c>null</c> to signal that the operation is unsupported.
    /// </remarks>
    /// <seealso cref="IPixProcessor"/>
    public sealed class LegacyPixProcessor : IPixProcessor
    {
        private readonly Dictionary<Type, Func<PixImage, V2d, ImageInterpolation, PixImage>> scaleFuns = new();
        private readonly Dictionary<Type, Func<PixImage, double, bool, ImageInterpolation, PixImage>> rotateFuns = new();
        private readonly Dictionary<Type, Func<PixImage, Matrix<float>, Matrix<float>, ImageInterpolation, PixImage>> remapFuns = new();

        /// <inheritdoc/>
        public string Name => "Aardvark (Legacy)";

        /// <summary>
        /// Gets the computed set of capabilities based on the registered delegates.
        /// </summary>
        public PixProcessorCaps Capabilities
        {
            get
            {
                var result = PixProcessorCaps.None;
                if (scaleFuns.Count > 0) result |= PixProcessorCaps.Scale;
                if (rotateFuns.Count > 0) result |= PixProcessorCaps.Rotate;
                if (remapFuns.Count > 0) result |= PixProcessorCaps.Remap;
                return result;
            }
        }

        /// <summary>
        /// Registers or unregisters the scaling function for pixel type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The pixel component type.</typeparam>
        /// <param name="scaleFun">The scaling function to register; pass <c>null</c> to unregister.</param>
        public void SetScaleFun<T>(Func<PixImage<T>, V2d, ImageInterpolation, PixImage<T>> scaleFun)
        {
            if (scaleFun != null)
                scaleFuns[typeof(T)] = (p, s, i) => scaleFun((PixImage<T>)p, s, i);
            else
                scaleFuns.Remove(typeof(T));
        }

        /// <inheritdoc/>
        public PixImage<T> Scale<T>(PixImage<T> image, V2d scaleFactor, ImageInterpolation interpolation)
        {
            if (scaleFuns.TryGetValue(typeof(T), out var fun))
                return (PixImage<T>)fun(image, scaleFactor, interpolation);
            else
                return null;
        }

        /// <summary>
        /// Registers or unregisters the rotation function for pixel type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The pixel component type.</typeparam>
        /// <param name="rotateFun">The rotation function to register; pass <c>null</c> to unregister.</param>
        public void SetRotateFun<T>(Func<PixImage<T>, double, bool, ImageInterpolation, PixImage<T>> rotateFun)
        {
            if (rotateFun != null)
                rotateFuns[typeof(T)] = (p, a, r, i) => rotateFun((PixImage<T>)p, a, r, i);
            else
                rotateFuns.Remove(typeof(T));
        }

        /// <inheritdoc/>
        public PixImage<T> Rotate<T>(PixImage<T> image, double angleInRadians, bool resize, ImageInterpolation interpolation,
                                     ImageBorderType borderType = ImageBorderType.Const,
                                     T border = default)
        {
            if (rotateFuns.TryGetValue(typeof(T), out var fun))
                return (PixImage<T>)fun(image, angleInRadians, resize, interpolation);
            else
                return null;
        }

        /// <summary>
        /// Registers or unregisters the remapping function for pixel type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The pixel component type.</typeparam>
        /// <param name="remapFun">The remapping function to register; pass <c>null</c> to unregister.</param>
        public void SetRemapFun<T>(Func<PixImage<T>, Matrix<float>, Matrix<float>, ImageInterpolation, PixImage<T>> remapFun)
        {
            if (remapFun != null)
                remapFuns[typeof(T)] = (p, x, y, i) => remapFun((PixImage<T>)p, x, y, i);
            else
                remapFuns.Remove(typeof(T));
        }

        /// <inheritdoc/>
        public PixImage<T> Remap<T>(PixImage<T> image, Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation,
                                    ImageBorderType borderType = ImageBorderType.Const,
                                    T border = default)
        {
            if (remapFuns.TryGetValue(typeof(T), out var fun))
                return (PixImage<T>)fun(image, mapX, mapY, interpolation);
            else
                return null;
        }

        private LegacyPixProcessor() { }

        private static readonly Lazy<LegacyPixProcessor> _instance = new(() => new LegacyPixProcessor());

        /// <summary>
        /// Gets the singleton instance of the <see cref="LegacyPixProcessor"/>.
        /// </summary>
        public static LegacyPixProcessor Instance { get; } = _instance.Value;
    }
}
using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    [Flags]
    public enum PixProcessorCaps
    {
        None   = 0,
        Scale  = 1,
        Rotate = 2,
        Remap  = 4,
        All    = Scale | Rotate | Remap,
    }

    /// <summary>
    /// Interface for plugins providing various image processing methods.
    /// </summary>
    public interface IPixProcessor
    {
        /// <summary>
        /// Name of the processor.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Capabilities of the processor.
        /// </summary>
        PixProcessorCaps Capabilities { get; }

        /// <summary>
        /// Scales the given image.
        /// Returns null if the operation is not supported.
        /// </summary>
        /// <param name="image">The image to scale.</param>
        /// <param name="scaleFactor">The scale factor to apply in each dimension.</param>
        /// <param name="interpolation">The interpolation method to use.</param>
        PixImage<T> Scale<T>(PixImage<T> image, V2d scaleFactor, ImageInterpolation interpolation);

        /// <summary>
        /// Rotates an image counter-clockwise by the given angle in radians.
        /// Returns null if the operation is not supported.
        /// </summary>
        /// <param name="image">The image to rotate.</param>
        /// <param name="angleInRadians">The angle in radians by which to rotate the image.</param>
        /// <param name="resize">True if the resulting image is resized accordingly, false otherwise.</param>
        /// <param name="interpolation">The interpolation method to use.</param>
        /// <param name="borderType">Determines how pixels outside the source image are computed.</param>
        /// <param name="border">The value to use for border pixels if <paramref name="borderType"/> is <see cref="ImageBorderType.Const"/>.</param>
        PixImage<T> Rotate<T>(PixImage<T> image, double angleInRadians, bool resize, ImageInterpolation interpolation,
                              ImageBorderType borderType = ImageBorderType.Const,
                              T border = default);

        /// <summary>
        /// Applies a generic geometric transformation to the given image.
        /// Computes the values of the resulting image as f(x, y) = <paramref name="image"/>[<paramref name="mapX"/>(x, y), <paramref name="mapY"/>(x, y)].
        /// Returns null if the operation is not supported.
        /// </summary>
        /// <param name="image">The image to transform.</param>
        /// <param name="mapX">The mapping for X coordinates.</param>
        /// <param name="mapY">The mapping for Y coordinates.</param>
        /// <param name="interpolation">The interpolation method to use.</param>
        /// <param name="borderType">Determines how pixels outside the source image are computed.</param>
        /// <param name="border">The value to use for border pixels if <paramref name="borderType"/> is <see cref="ImageBorderType.Const"/>.</param>
        PixImage<T> Remap<T>(PixImage<T> image, Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation,
                             ImageBorderType borderType = ImageBorderType.Const,
                             T border = default);
    }

    /// <summary>
    /// Basic image processor using built-in Aardvark algorithms and methods.
    /// </summary>
    public sealed class PixProcessor : IPixProcessor
    {
        public string Name => "Aardvark";

        public PixProcessorCaps Capabilities => PixProcessorCaps.Scale;

        public PixImage<T> Scale<T>(PixImage<T> image, V2d scaleFactor, ImageInterpolation interpolation)
            => new (image.Format, TensorExtensions.Scaled(image.Volume, scaleFactor, interpolation));

        public PixImage<T> Rotate<T>(PixImage<T> image, double angleInRadians, bool resize,ImageInterpolation interpolation,
                                     ImageBorderType borderType = ImageBorderType.Const,
                                     T border = default)
            => null;

        public PixImage<T> Remap<T>(PixImage<T> image, Matrix<float> mapX, Matrix<float> mapY, ImageInterpolation interpolation,
                                    ImageBorderType borderType = ImageBorderType.Const,
                                    T border = default)
            => null;

        private PixProcessor() { }

        private static readonly Lazy<PixProcessor> _instance = new (() => new PixProcessor());

        public static PixProcessor Instance { get; } = _instance.Value;
    }

    /// <summary>
    /// Processor for compatibility with legacy API.
    /// </summary>
    public sealed class LegacyPixProcessor : IPixProcessor
    {
        private readonly Dictionary<Type, Func<PixImage, V2d, ImageInterpolation, PixImage>> scaleFuns = new();
        private readonly Dictionary<Type, Func<PixImage, double, bool, ImageInterpolation, PixImage>> rotateFuns = new();
        private readonly Dictionary<Type, Func<PixImage, Matrix<float>, Matrix<float>, ImageInterpolation, PixImage>> remapFuns = new();

        public string Name => "Aardvark (Legacy)";

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

        public void SetScaleFun<T>(Func<PixImage<T>, V2d, ImageInterpolation, PixImage<T>> scaleFun)
        {
            if (scaleFun != null)
                scaleFuns[typeof(T)] = (p, s, i) => scaleFun((PixImage<T>)p, s, i);
            else
                scaleFuns.Remove(typeof(T));
        }

        public PixImage<T> Scale<T>(PixImage<T> image, V2d scaleFactor, ImageInterpolation interpolation)
        {
            if (scaleFuns.TryGetValue(typeof(T), out var fun))
                return (PixImage<T>)fun(image, scaleFactor, interpolation);
            else
                return null;
        }

        public void SetRotateFun<T>(Func<PixImage<T>, double, bool, ImageInterpolation, PixImage<T>> rotateFun)
        {
            if (rotateFun != null)
                rotateFuns[typeof(T)] = (p, a, r, i) => rotateFun((PixImage<T>)p, a, r, i);
            else
                rotateFuns.Remove(typeof(T));
        }

        public PixImage<T> Rotate<T>(PixImage<T> image, double angleInRadians, bool resize, ImageInterpolation interpolation,
                                     ImageBorderType borderType = ImageBorderType.Const,
                                     T border = default)
        {
            if (rotateFuns.TryGetValue(typeof(T), out var fun))
                return (PixImage<T>)fun(image, angleInRadians, resize, interpolation);
            else
                return null;
        }

        public void SetRemapFun<T>(Func<PixImage<T>, Matrix<float>, Matrix<float>, ImageInterpolation, PixImage<T>> remapFun)
        {
            if (remapFun != null)
                remapFuns[typeof(T)] = (p, x, y, i) => remapFun((PixImage<T>)p, x, y, i);
            else
                remapFuns.Remove(typeof(T));
        }

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

        public static LegacyPixProcessor Instance { get; } = _instance.Value;
    }
}
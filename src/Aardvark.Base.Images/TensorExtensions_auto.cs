using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;

namespace Aardvark.Base
{
    /// <summary>
    /// Wrappers for the best (fastest) available implementation of the respective tensor operation.
    /// </summary>
    public static partial class TensorExtensions
    {
        #region Image Scaling

        public static void SetScaledNearest(this Matrix<byte> targetMat, Matrix<byte> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<byte> targetMat, Matrix<byte> sourceMat,
                                           Func<double, byte, byte, T1> xinterpolator,
                                           Func<double, T1, T1, byte> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte> targetMat, Matrix<byte> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<byte> targetMat, Matrix<byte> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte> targetMat, Matrix<byte> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinComRawF, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  Col.ByteFromByteInFloatClamped);
        }

        public static void SetScaledBSpline5(this Matrix<byte> targetMat, Matrix<byte> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<byte> targetMat, Matrix<byte> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<byte> targetMat, Matrix<byte> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Fun.LinComRawF, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  Col.ByteFromByteInFloatClamped);
        }

        public static void SetScaledNearest(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat,
                                           Func<double, ushort, ushort, T1> xinterpolator,
                                           Func<double, T1, T1, ushort> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinComRawF, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  Col.UShortFromUShortInFloatClamped);
        }

        public static void SetScaledBSpline5(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Fun.LinComRawF, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  Col.UShortFromUShortInFloatClamped);
        }

        public static void SetScaledNearest(this Matrix<float> targetMat, Matrix<float> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<float> targetMat, Matrix<float> sourceMat,
                                           Func<double, float, float, T1> xinterpolator,
                                           Func<double, T1, T1, float> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float> targetMat, Matrix<float> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<float> targetMat, Matrix<float> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float> targetMat, Matrix<float> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinCom, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<float> targetMat, Matrix<float> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<float> targetMat, Matrix<float> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<float> targetMat, Matrix<float> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Fun.LinCom, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        public static void SetScaledNearest(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat,
                                           Func<double, C3b, C3b, T1> xinterpolator,
                                           Func<double, T1, T1, C3b> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C3b.LinComRawC3f, C3f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.ByteFromByteInFloatClamped));
        }

        public static void SetScaledBSpline5(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, C3b.LinComRawC3f, C3f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.ByteFromByteInFloatClamped));
        }

        public static void SetScaledNearest(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat,
                                           Func<double, C3us, C3us, T1> xinterpolator,
                                           Func<double, T1, T1, C3us> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C3us.LinComRawC3f, C3f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.UShortFromUShortInFloatClamped));
        }

        public static void SetScaledBSpline5(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, C3us.LinComRawC3f, C3f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.UShortFromUShortInFloatClamped));
        }

        public static void SetScaledNearest(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat,
                                           Func<double, C3f, C3f, T1> xinterpolator,
                                           Func<double, T1, T1, C3f> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C3f.LinCom, C3f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, C3f.LinCom, C3f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        public static void SetScaledNearest(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                           Func<double, C4b, C4b, T1> xinterpolator,
                                           Func<double, T1, T1, C4b> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C4b.LinComRawC4f, C4f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.ByteFromByteInFloatClamped));
        }

        public static void SetScaledBSpline5(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, C4b.LinComRawC4f, C4f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.ByteFromByteInFloatClamped));
        }

        public static void SetScaledNearest(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat,
                                           Func<double, C4us, C4us, T1> xinterpolator,
                                           Func<double, T1, T1, C4us> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C4us.LinComRawC4f, C4f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.UShortFromUShortInFloatClamped));
        }

        public static void SetScaledBSpline5(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, C4us.LinComRawC4f, C4f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.UShortFromUShortInFloatClamped));
        }

        public static void SetScaledNearest(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }
        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat,
                                           Func<double, C4f, C4f, T1> xinterpolator,
                                           Func<double, T1, T1, C4f> yinterpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled4(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 xinterpolator, yinterpolator,
                                 Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3f);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C4f.LinCom, C4f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3f);
        }

        public static void SetScaledOrder5(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat,
                                           Func<double, Tup6<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, C4f.LinCom, C4f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        #endregion
    }
}
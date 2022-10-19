using System;

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
                                  Col.ByteInFloatToByteClamped);
        }

        public static void SetScaledBSpline5(this Matrix<byte> targetMat, Matrix<byte> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  Col.ByteInFloatToByteClamped);
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
                                  Col.UShortInFloatToUShortClamped);
        }

        public static void SetScaledBSpline5(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  Col.UShortInFloatToUShortClamped);
        }

        public static void SetScaledNearest(this Matrix<uint> targetMat, Matrix<uint> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<uint> targetMat, Matrix<uint> sourceMat,
                                           Func<double, uint, uint, T1> xinterpolator,
                                           Func<double, T1, T1, uint> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<uint> targetMat, Matrix<uint> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4d(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<uint> targetMat, Matrix<uint> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3d);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<uint> targetMat, Matrix<uint> sourceMat,
                                          Func<double, Tup4<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinComRawD, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  Col.UIntInDoubleToUIntClamped);
        }

        public static void SetScaledBSpline5(this Matrix<uint> targetMat, Matrix<uint> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5d);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<uint> targetMat, Matrix<uint> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3d);
        }

        public static void SetScaledOrder5(this Matrix<uint> targetMat, Matrix<uint> sourceMat,
                                           Func<double, Tup6<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Fun.LinComRawD, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  Col.UIntInDoubleToUIntClamped);
        }

        public static void SetScaledNearest(this Matrix<Half> targetMat, Matrix<Half> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<Half> targetMat, Matrix<Half> sourceMat,
                                           Func<double, Half, Half, T1> xinterpolator,
                                           Func<double, T1, T1, Half> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<Half> targetMat, Matrix<Half> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4h(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<Half> targetMat, Matrix<Half> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3h);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<Half> targetMat, Matrix<Half> sourceMat,
                                          Func<double, Tup4<Half>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinCom, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<Half> targetMat, Matrix<Half> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5h);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<Half> targetMat, Matrix<Half> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3h);
        }

        public static void SetScaledOrder5(this Matrix<Half> targetMat, Matrix<Half> sourceMat,
                                           Func<double, Tup6<Half>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Fun.LinCom, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
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
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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

        public static void SetScaledNearest(this Matrix<double> targetMat, Matrix<double> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<double> targetMat, Matrix<double> sourceMat,
                                           Func<double, double, double, T1> xinterpolator,
                                           Func<double, T1, T1, double> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<double> targetMat, Matrix<double> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4d(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<double> targetMat, Matrix<double> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3d);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<double> targetMat, Matrix<double> sourceMat,
                                          Func<double, Tup4<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinCom, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<double> targetMat, Matrix<double> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5d);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<double> targetMat, Matrix<double> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3d);
        }

        public static void SetScaledOrder5(this Matrix<double> targetMat, Matrix<double> sourceMat,
                                           Func<double, Tup6<double>> interpolator)
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
                                 interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.ByteInFloatToByteClamped));
        }

        public static void SetScaledBSpline5(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.ByteInFloatToByteClamped));
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
                                 interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.UShortInFloatToUShortClamped));
        }

        public static void SetScaledBSpline5(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.UShortInFloatToUShortClamped));
        }

        public static void SetScaledNearest(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat,
                                           Func<double, C3ui, C3ui, T1> xinterpolator,
                                           Func<double, T1, T1, C3ui> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4d(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3d);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat,
                                          Func<double, Tup4<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Col.LinComRawD, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.UIntInDoubleToUIntClamped));
        }

        public static void SetScaledBSpline5(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5d);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3d);
        }

        public static void SetScaledOrder5(this Matrix<uint, C3ui> targetMat, Matrix<uint, C3ui> sourceMat,
                                           Func<double, Tup6<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Col.LinComRawD, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.UIntInDoubleToUIntClamped));
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
                                 interpolator, interpolator, Col.LinCom, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  interpolator, interpolator, Col.LinCom, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        public static void SetScaledNearest(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat,
                                           Func<double, C3d, C3d, T1> xinterpolator,
                                           Func<double, T1, T1, C3d> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4d(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3d);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat,
                                          Func<double, Tup4<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Col.LinCom, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5d);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3d);
        }

        public static void SetScaledOrder5(this Matrix<double, C3d> targetMat, Matrix<double, C3d> sourceMat,
                                           Func<double, Tup6<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Col.LinCom, Col.LinCom,
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
                                 interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.ByteInFloatToByteClamped));
        }

        public static void SetScaledBSpline5(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.ByteInFloatToByteClamped));
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
                                 interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.UShortInFloatToUShortClamped));
        }

        public static void SetScaledBSpline5(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  interpolator, interpolator, Col.LinComRawF, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.UShortInFloatToUShortClamped));
        }

        public static void SetScaledNearest(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat,
                                           Func<double, C4ui, C4ui, T1> xinterpolator,
                                           Func<double, T1, T1, C4ui> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4d(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3d);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat,
                                          Func<double, Tup4<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Col.LinComRawD, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Map(Col.UIntInDoubleToUIntClamped));
        }

        public static void SetScaledBSpline5(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5d);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3d);
        }

        public static void SetScaledOrder5(this Matrix<uint, C4ui> targetMat, Matrix<uint, C4ui> sourceMat,
                                           Func<double, Tup6<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Col.LinComRawD, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Map(Col.UIntInDoubleToUIntClamped));
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
                                 interpolator, interpolator, Col.LinCom, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5f);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
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
                                  interpolator, interpolator, Col.LinCom, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        public static void SetScaledNearest(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat,
                                           Func<double, C4d, C4d, T1> xinterpolator,
                                           Func<double, T1, T1, C4d> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4d(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3d);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat,
                                          Func<double, Tup4<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Col.LinCom, Col.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public static void SetScaledBSpline5(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5d);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3d);
        }

        public static void SetScaledOrder5(this Matrix<double, C4d> targetMat, Matrix<double, C4d> sourceMat,
                                           Func<double, Tup6<double>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, Col.LinCom, Col.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        #endregion
    }
}
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


        /// <summary>
        /// Scales the source into the target using exact area-weighted supersampling when both target axes are
        /// unchanged or smaller. If either target axis is larger, this method uses cubic interpolation instead.
        /// Tensor windows and positive, non-canonical strides are supported.
        /// </summary>
        public static void SetScaledSuperSample(this Matrix<byte> targetMat, Matrix<byte> sourceMat)
        {
            if (targetMat.SX == 0 || targetMat.SY == 0)
                return;

            if (targetMat.SX > sourceMat.SX || targetMat.SY > sourceMat.SY)
            {
                targetMat.SetScaledCubic(sourceMat);
                return;
            }

            if (targetMat.Size == sourceMat.Size)
            {
                targetMat.Set(sourceMat);
                return;
            }

            var xSpans = targetMat.SX < sourceMat.SX ? CreateAreaSpans(sourceMat.SX, targetMat.SX) : null;
            var ySpans = targetMat.SY < sourceMat.SY ? CreateAreaSpans(sourceMat.SY, targetMat.SY) : null;
            var workspace = targetMat.SX < sourceMat.SX && targetMat.SY < sourceMat.SY
                ? new double[checked(targetMat.SX * sourceMat.SY)]
                : null;
            targetMat.SetScaledSuperSample(sourceMat, xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Volume<byte> target, Volume<byte> source,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            for (long c = 0; c < source.SZ; c++)
                target.SubXYMatrixWindow(c).SetScaledSuperSample(source.SubXYMatrixWindow(c), xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Matrix<byte> targetMat, Matrix<byte> sourceMat,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            var source = sourceMat.Data;
            var target = targetMat.Data;
            long sourceFirst = sourceMat.FirstIndex;
            long targetFirst = targetMat.FirstIndex;
            long sourceDx = sourceMat.DX;
            long sourceDy = sourceMat.DY;
            long targetDx = targetMat.DX;
            long targetDy = targetMat.DY;
            long sourceSx = sourceMat.SX;
            long sourceSy = sourceMat.SY;
            long targetSx = targetMat.SX;
            long targetSy = targetMat.SY;
            bool scaleX = sourceSx != targetSx;
            bool scaleY = sourceSy != targetSy;

            if (!scaleX && !scaleY)
            {
                targetMat.Set(sourceMat);
                return;
            }

            if (scaleX && !scaleY)
            {
                double normalization = (double)targetSx / sourceSx;
                for (long y = 0; y < targetSy; y++)
                {
                    long sourceRow = sourceFirst + y * sourceDy;
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        var span = xSpans[x];
                        double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                        for (long sx = span.First + 1; sx < span.Last; sx++)
                            sum += source[sourceRow + sx * sourceDx];
                        sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = Col.ByteInDoubleToByteClamped(value);
                    }
                }
                return;
            }

            if (!scaleX)
            {
                double normalization = (double)targetSy / sourceSy;
                for (long y = 0; y < targetSy; y++)
                {
                    var span = ySpans[y];
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        long sourceColumn = sourceFirst + x * sourceDx;
                        double sum = source[sourceColumn + span.First * sourceDy] * span.FirstWeight;
                        for (long sy = span.First + 1; sy < span.Last; sy++)
                            sum += source[sourceColumn + sy * sourceDy];
                        sum += source[sourceColumn + span.Last * sourceDy] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = Col.ByteInDoubleToByteClamped(value);
                    }
                }
                return;
            }

            double xNormalization = (double)targetSx / sourceSx;
            long workspaceIndex = 0;
            for (long y = 0; y < sourceSy; y++)
            {
                long sourceRow = sourceFirst + y * sourceDy;
                for (long x = 0; x < targetSx; x++)
                {
                    var span = xSpans[x];
                    double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                    for (long sx = span.First + 1; sx < span.Last; sx++)
                        sum += source[sourceRow + sx * sourceDx];
                    sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                    workspace[workspaceIndex++] = sum * xNormalization;
                }
            }

            double yNormalization = (double)targetSy / sourceSy;
            for (long y = 0; y < targetSy; y++)
            {
                var span = ySpans[y];
                long targetRow = targetFirst + y * targetDy;
                for (long x = 0; x < targetSx; x++)
                {
                    double sum = workspace[span.First * targetSx + x] * span.FirstWeight;
                    for (long sy = span.First + 1; sy < span.Last; sy++)
                        sum += workspace[sy * targetSx + x];
                    sum += workspace[span.Last * targetSx + x] * span.LastWeight;
                    double value = sum * yNormalization;
                    target[targetRow + x * targetDx] = Col.ByteInDoubleToByteClamped(value);
                }
            }
        }

        /// <summary>
        /// Scales the source into the target using exact area-weighted supersampling when both target axes are
        /// unchanged or smaller. If either target axis is larger, this method uses cubic interpolation instead.
        /// Tensor windows and positive, non-canonical strides are supported.
        /// </summary>
        public static void SetScaledSuperSample(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat)
        {
            if (targetMat.SX == 0 || targetMat.SY == 0)
                return;

            if (targetMat.SX > sourceMat.SX || targetMat.SY > sourceMat.SY)
            {
                targetMat.SetScaledCubic(sourceMat);
                return;
            }

            if (targetMat.Size == sourceMat.Size)
            {
                targetMat.Set(sourceMat);
                return;
            }

            var xSpans = targetMat.SX < sourceMat.SX ? CreateAreaSpans(sourceMat.SX, targetMat.SX) : null;
            var ySpans = targetMat.SY < sourceMat.SY ? CreateAreaSpans(sourceMat.SY, targetMat.SY) : null;
            var workspace = targetMat.SX < sourceMat.SX && targetMat.SY < sourceMat.SY
                ? new double[checked(targetMat.SX * sourceMat.SY)]
                : null;
            targetMat.SetScaledSuperSample(sourceMat, xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Volume<ushort> target, Volume<ushort> source,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            for (long c = 0; c < source.SZ; c++)
                target.SubXYMatrixWindow(c).SetScaledSuperSample(source.SubXYMatrixWindow(c), xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Matrix<ushort> targetMat, Matrix<ushort> sourceMat,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            var source = sourceMat.Data;
            var target = targetMat.Data;
            long sourceFirst = sourceMat.FirstIndex;
            long targetFirst = targetMat.FirstIndex;
            long sourceDx = sourceMat.DX;
            long sourceDy = sourceMat.DY;
            long targetDx = targetMat.DX;
            long targetDy = targetMat.DY;
            long sourceSx = sourceMat.SX;
            long sourceSy = sourceMat.SY;
            long targetSx = targetMat.SX;
            long targetSy = targetMat.SY;
            bool scaleX = sourceSx != targetSx;
            bool scaleY = sourceSy != targetSy;

            if (!scaleX && !scaleY)
            {
                targetMat.Set(sourceMat);
                return;
            }

            if (scaleX && !scaleY)
            {
                double normalization = (double)targetSx / sourceSx;
                for (long y = 0; y < targetSy; y++)
                {
                    long sourceRow = sourceFirst + y * sourceDy;
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        var span = xSpans[x];
                        double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                        for (long sx = span.First + 1; sx < span.Last; sx++)
                            sum += source[sourceRow + sx * sourceDx];
                        sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = Col.UShortInDoubleToUShortClamped(value);
                    }
                }
                return;
            }

            if (!scaleX)
            {
                double normalization = (double)targetSy / sourceSy;
                for (long y = 0; y < targetSy; y++)
                {
                    var span = ySpans[y];
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        long sourceColumn = sourceFirst + x * sourceDx;
                        double sum = source[sourceColumn + span.First * sourceDy] * span.FirstWeight;
                        for (long sy = span.First + 1; sy < span.Last; sy++)
                            sum += source[sourceColumn + sy * sourceDy];
                        sum += source[sourceColumn + span.Last * sourceDy] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = Col.UShortInDoubleToUShortClamped(value);
                    }
                }
                return;
            }

            double xNormalization = (double)targetSx / sourceSx;
            long workspaceIndex = 0;
            for (long y = 0; y < sourceSy; y++)
            {
                long sourceRow = sourceFirst + y * sourceDy;
                for (long x = 0; x < targetSx; x++)
                {
                    var span = xSpans[x];
                    double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                    for (long sx = span.First + 1; sx < span.Last; sx++)
                        sum += source[sourceRow + sx * sourceDx];
                    sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                    workspace[workspaceIndex++] = sum * xNormalization;
                }
            }

            double yNormalization = (double)targetSy / sourceSy;
            for (long y = 0; y < targetSy; y++)
            {
                var span = ySpans[y];
                long targetRow = targetFirst + y * targetDy;
                for (long x = 0; x < targetSx; x++)
                {
                    double sum = workspace[span.First * targetSx + x] * span.FirstWeight;
                    for (long sy = span.First + 1; sy < span.Last; sy++)
                        sum += workspace[sy * targetSx + x];
                    sum += workspace[span.Last * targetSx + x] * span.LastWeight;
                    double value = sum * yNormalization;
                    target[targetRow + x * targetDx] = Col.UShortInDoubleToUShortClamped(value);
                }
            }
        }

        /// <summary>
        /// Scales the source into the target using exact area-weighted supersampling when both target axes are
        /// unchanged or smaller. If either target axis is larger, this method uses cubic interpolation instead.
        /// Tensor windows and positive, non-canonical strides are supported.
        /// </summary>
        public static void SetScaledSuperSample(this Matrix<uint> targetMat, Matrix<uint> sourceMat)
        {
            if (targetMat.SX == 0 || targetMat.SY == 0)
                return;

            if (targetMat.SX > sourceMat.SX || targetMat.SY > sourceMat.SY)
            {
                targetMat.SetScaledCubic(sourceMat);
                return;
            }

            if (targetMat.Size == sourceMat.Size)
            {
                targetMat.Set(sourceMat);
                return;
            }

            var xSpans = targetMat.SX < sourceMat.SX ? CreateAreaSpans(sourceMat.SX, targetMat.SX) : null;
            var ySpans = targetMat.SY < sourceMat.SY ? CreateAreaSpans(sourceMat.SY, targetMat.SY) : null;
            var workspace = targetMat.SX < sourceMat.SX && targetMat.SY < sourceMat.SY
                ? new double[checked(targetMat.SX * sourceMat.SY)]
                : null;
            targetMat.SetScaledSuperSample(sourceMat, xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Volume<uint> target, Volume<uint> source,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            for (long c = 0; c < source.SZ; c++)
                target.SubXYMatrixWindow(c).SetScaledSuperSample(source.SubXYMatrixWindow(c), xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Matrix<uint> targetMat, Matrix<uint> sourceMat,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            var source = sourceMat.Data;
            var target = targetMat.Data;
            long sourceFirst = sourceMat.FirstIndex;
            long targetFirst = targetMat.FirstIndex;
            long sourceDx = sourceMat.DX;
            long sourceDy = sourceMat.DY;
            long targetDx = targetMat.DX;
            long targetDy = targetMat.DY;
            long sourceSx = sourceMat.SX;
            long sourceSy = sourceMat.SY;
            long targetSx = targetMat.SX;
            long targetSy = targetMat.SY;
            bool scaleX = sourceSx != targetSx;
            bool scaleY = sourceSy != targetSy;

            if (!scaleX && !scaleY)
            {
                targetMat.Set(sourceMat);
                return;
            }

            if (scaleX && !scaleY)
            {
                double normalization = (double)targetSx / sourceSx;
                for (long y = 0; y < targetSy; y++)
                {
                    long sourceRow = sourceFirst + y * sourceDy;
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        var span = xSpans[x];
                        double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                        for (long sx = span.First + 1; sx < span.Last; sx++)
                            sum += source[sourceRow + sx * sourceDx];
                        sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = Col.UIntInDoubleToUIntClamped(value);
                    }
                }
                return;
            }

            if (!scaleX)
            {
                double normalization = (double)targetSy / sourceSy;
                for (long y = 0; y < targetSy; y++)
                {
                    var span = ySpans[y];
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        long sourceColumn = sourceFirst + x * sourceDx;
                        double sum = source[sourceColumn + span.First * sourceDy] * span.FirstWeight;
                        for (long sy = span.First + 1; sy < span.Last; sy++)
                            sum += source[sourceColumn + sy * sourceDy];
                        sum += source[sourceColumn + span.Last * sourceDy] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = Col.UIntInDoubleToUIntClamped(value);
                    }
                }
                return;
            }

            double xNormalization = (double)targetSx / sourceSx;
            long workspaceIndex = 0;
            for (long y = 0; y < sourceSy; y++)
            {
                long sourceRow = sourceFirst + y * sourceDy;
                for (long x = 0; x < targetSx; x++)
                {
                    var span = xSpans[x];
                    double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                    for (long sx = span.First + 1; sx < span.Last; sx++)
                        sum += source[sourceRow + sx * sourceDx];
                    sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                    workspace[workspaceIndex++] = sum * xNormalization;
                }
            }

            double yNormalization = (double)targetSy / sourceSy;
            for (long y = 0; y < targetSy; y++)
            {
                var span = ySpans[y];
                long targetRow = targetFirst + y * targetDy;
                for (long x = 0; x < targetSx; x++)
                {
                    double sum = workspace[span.First * targetSx + x] * span.FirstWeight;
                    for (long sy = span.First + 1; sy < span.Last; sy++)
                        sum += workspace[sy * targetSx + x];
                    sum += workspace[span.Last * targetSx + x] * span.LastWeight;
                    double value = sum * yNormalization;
                    target[targetRow + x * targetDx] = Col.UIntInDoubleToUIntClamped(value);
                }
            }
        }

        /// <summary>
        /// Scales the source into the target using exact area-weighted supersampling when both target axes are
        /// unchanged or smaller. If either target axis is larger, this method uses cubic interpolation instead.
        /// Tensor windows and positive, non-canonical strides are supported.
        /// </summary>
        public static void SetScaledSuperSample(this Matrix<Half> targetMat, Matrix<Half> sourceMat)
        {
            if (targetMat.SX == 0 || targetMat.SY == 0)
                return;

            if (targetMat.SX > sourceMat.SX || targetMat.SY > sourceMat.SY)
            {
                targetMat.SetScaledCubic(sourceMat);
                return;
            }

            if (targetMat.Size == sourceMat.Size)
            {
                targetMat.Set(sourceMat);
                return;
            }

            var xSpans = targetMat.SX < sourceMat.SX ? CreateAreaSpans(sourceMat.SX, targetMat.SX) : null;
            var ySpans = targetMat.SY < sourceMat.SY ? CreateAreaSpans(sourceMat.SY, targetMat.SY) : null;
            var workspace = targetMat.SX < sourceMat.SX && targetMat.SY < sourceMat.SY
                ? new double[checked(targetMat.SX * sourceMat.SY)]
                : null;
            targetMat.SetScaledSuperSample(sourceMat, xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Volume<Half> target, Volume<Half> source,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            for (long c = 0; c < source.SZ; c++)
                target.SubXYMatrixWindow(c).SetScaledSuperSample(source.SubXYMatrixWindow(c), xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Matrix<Half> targetMat, Matrix<Half> sourceMat,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            var source = sourceMat.Data;
            var target = targetMat.Data;
            long sourceFirst = sourceMat.FirstIndex;
            long targetFirst = targetMat.FirstIndex;
            long sourceDx = sourceMat.DX;
            long sourceDy = sourceMat.DY;
            long targetDx = targetMat.DX;
            long targetDy = targetMat.DY;
            long sourceSx = sourceMat.SX;
            long sourceSy = sourceMat.SY;
            long targetSx = targetMat.SX;
            long targetSy = targetMat.SY;
            bool scaleX = sourceSx != targetSx;
            bool scaleY = sourceSy != targetSy;

            if (!scaleX && !scaleY)
            {
                targetMat.Set(sourceMat);
                return;
            }

            if (scaleX && !scaleY)
            {
                double normalization = (double)targetSx / sourceSx;
                for (long y = 0; y < targetSy; y++)
                {
                    long sourceRow = sourceFirst + y * sourceDy;
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        var span = xSpans[x];
                        double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                        for (long sx = span.First + 1; sx < span.Last; sx++)
                            sum += source[sourceRow + sx * sourceDx];
                        sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = (Half)value;
                    }
                }
                return;
            }

            if (!scaleX)
            {
                double normalization = (double)targetSy / sourceSy;
                for (long y = 0; y < targetSy; y++)
                {
                    var span = ySpans[y];
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        long sourceColumn = sourceFirst + x * sourceDx;
                        double sum = source[sourceColumn + span.First * sourceDy] * span.FirstWeight;
                        for (long sy = span.First + 1; sy < span.Last; sy++)
                            sum += source[sourceColumn + sy * sourceDy];
                        sum += source[sourceColumn + span.Last * sourceDy] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = (Half)value;
                    }
                }
                return;
            }

            double xNormalization = (double)targetSx / sourceSx;
            long workspaceIndex = 0;
            for (long y = 0; y < sourceSy; y++)
            {
                long sourceRow = sourceFirst + y * sourceDy;
                for (long x = 0; x < targetSx; x++)
                {
                    var span = xSpans[x];
                    double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                    for (long sx = span.First + 1; sx < span.Last; sx++)
                        sum += source[sourceRow + sx * sourceDx];
                    sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                    workspace[workspaceIndex++] = sum * xNormalization;
                }
            }

            double yNormalization = (double)targetSy / sourceSy;
            for (long y = 0; y < targetSy; y++)
            {
                var span = ySpans[y];
                long targetRow = targetFirst + y * targetDy;
                for (long x = 0; x < targetSx; x++)
                {
                    double sum = workspace[span.First * targetSx + x] * span.FirstWeight;
                    for (long sy = span.First + 1; sy < span.Last; sy++)
                        sum += workspace[sy * targetSx + x];
                    sum += workspace[span.Last * targetSx + x] * span.LastWeight;
                    double value = sum * yNormalization;
                    target[targetRow + x * targetDx] = (Half)value;
                }
            }
        }

        /// <summary>
        /// Scales the source into the target using exact area-weighted supersampling when both target axes are
        /// unchanged or smaller. If either target axis is larger, this method uses cubic interpolation instead.
        /// Tensor windows and positive, non-canonical strides are supported.
        /// </summary>
        public static void SetScaledSuperSample(this Matrix<float> targetMat, Matrix<float> sourceMat)
        {
            if (targetMat.SX == 0 || targetMat.SY == 0)
                return;

            if (targetMat.SX > sourceMat.SX || targetMat.SY > sourceMat.SY)
            {
                targetMat.SetScaledCubic(sourceMat);
                return;
            }

            if (targetMat.Size == sourceMat.Size)
            {
                targetMat.Set(sourceMat);
                return;
            }

            var xSpans = targetMat.SX < sourceMat.SX ? CreateAreaSpans(sourceMat.SX, targetMat.SX) : null;
            var ySpans = targetMat.SY < sourceMat.SY ? CreateAreaSpans(sourceMat.SY, targetMat.SY) : null;
            var workspace = targetMat.SX < sourceMat.SX && targetMat.SY < sourceMat.SY
                ? new double[checked(targetMat.SX * sourceMat.SY)]
                : null;
            targetMat.SetScaledSuperSample(sourceMat, xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Volume<float> target, Volume<float> source,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            for (long c = 0; c < source.SZ; c++)
                target.SubXYMatrixWindow(c).SetScaledSuperSample(source.SubXYMatrixWindow(c), xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Matrix<float> targetMat, Matrix<float> sourceMat,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            var source = sourceMat.Data;
            var target = targetMat.Data;
            long sourceFirst = sourceMat.FirstIndex;
            long targetFirst = targetMat.FirstIndex;
            long sourceDx = sourceMat.DX;
            long sourceDy = sourceMat.DY;
            long targetDx = targetMat.DX;
            long targetDy = targetMat.DY;
            long sourceSx = sourceMat.SX;
            long sourceSy = sourceMat.SY;
            long targetSx = targetMat.SX;
            long targetSy = targetMat.SY;
            bool scaleX = sourceSx != targetSx;
            bool scaleY = sourceSy != targetSy;

            if (!scaleX && !scaleY)
            {
                targetMat.Set(sourceMat);
                return;
            }

            if (scaleX && !scaleY)
            {
                double normalization = (double)targetSx / sourceSx;
                for (long y = 0; y < targetSy; y++)
                {
                    long sourceRow = sourceFirst + y * sourceDy;
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        var span = xSpans[x];
                        double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                        for (long sx = span.First + 1; sx < span.Last; sx++)
                            sum += source[sourceRow + sx * sourceDx];
                        sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = (float)value;
                    }
                }
                return;
            }

            if (!scaleX)
            {
                double normalization = (double)targetSy / sourceSy;
                for (long y = 0; y < targetSy; y++)
                {
                    var span = ySpans[y];
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        long sourceColumn = sourceFirst + x * sourceDx;
                        double sum = source[sourceColumn + span.First * sourceDy] * span.FirstWeight;
                        for (long sy = span.First + 1; sy < span.Last; sy++)
                            sum += source[sourceColumn + sy * sourceDy];
                        sum += source[sourceColumn + span.Last * sourceDy] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = (float)value;
                    }
                }
                return;
            }

            double xNormalization = (double)targetSx / sourceSx;
            long workspaceIndex = 0;
            for (long y = 0; y < sourceSy; y++)
            {
                long sourceRow = sourceFirst + y * sourceDy;
                for (long x = 0; x < targetSx; x++)
                {
                    var span = xSpans[x];
                    double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                    for (long sx = span.First + 1; sx < span.Last; sx++)
                        sum += source[sourceRow + sx * sourceDx];
                    sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                    workspace[workspaceIndex++] = sum * xNormalization;
                }
            }

            double yNormalization = (double)targetSy / sourceSy;
            for (long y = 0; y < targetSy; y++)
            {
                var span = ySpans[y];
                long targetRow = targetFirst + y * targetDy;
                for (long x = 0; x < targetSx; x++)
                {
                    double sum = workspace[span.First * targetSx + x] * span.FirstWeight;
                    for (long sy = span.First + 1; sy < span.Last; sy++)
                        sum += workspace[sy * targetSx + x];
                    sum += workspace[span.Last * targetSx + x] * span.LastWeight;
                    double value = sum * yNormalization;
                    target[targetRow + x * targetDx] = (float)value;
                }
            }
        }

        /// <summary>
        /// Scales the source into the target using exact area-weighted supersampling when both target axes are
        /// unchanged or smaller. If either target axis is larger, this method uses cubic interpolation instead.
        /// Tensor windows and positive, non-canonical strides are supported.
        /// </summary>
        public static void SetScaledSuperSample(this Matrix<double> targetMat, Matrix<double> sourceMat)
        {
            if (targetMat.SX == 0 || targetMat.SY == 0)
                return;

            if (targetMat.SX > sourceMat.SX || targetMat.SY > sourceMat.SY)
            {
                targetMat.SetScaledCubic(sourceMat);
                return;
            }

            if (targetMat.Size == sourceMat.Size)
            {
                targetMat.Set(sourceMat);
                return;
            }

            var xSpans = targetMat.SX < sourceMat.SX ? CreateAreaSpans(sourceMat.SX, targetMat.SX) : null;
            var ySpans = targetMat.SY < sourceMat.SY ? CreateAreaSpans(sourceMat.SY, targetMat.SY) : null;
            var workspace = targetMat.SX < sourceMat.SX && targetMat.SY < sourceMat.SY
                ? new double[checked(targetMat.SX * sourceMat.SY)]
                : null;
            targetMat.SetScaledSuperSample(sourceMat, xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Volume<double> target, Volume<double> source,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            for (long c = 0; c < source.SZ; c++)
                target.SubXYMatrixWindow(c).SetScaledSuperSample(source.SubXYMatrixWindow(c), xSpans, ySpans, workspace);
        }

        internal static void SetScaledSuperSample(
            this Matrix<double> targetMat, Matrix<double> sourceMat,
            AreaSpan[] xSpans, AreaSpan[] ySpans, double[] workspace)
        {
            var source = sourceMat.Data;
            var target = targetMat.Data;
            long sourceFirst = sourceMat.FirstIndex;
            long targetFirst = targetMat.FirstIndex;
            long sourceDx = sourceMat.DX;
            long sourceDy = sourceMat.DY;
            long targetDx = targetMat.DX;
            long targetDy = targetMat.DY;
            long sourceSx = sourceMat.SX;
            long sourceSy = sourceMat.SY;
            long targetSx = targetMat.SX;
            long targetSy = targetMat.SY;
            bool scaleX = sourceSx != targetSx;
            bool scaleY = sourceSy != targetSy;

            if (!scaleX && !scaleY)
            {
                targetMat.Set(sourceMat);
                return;
            }

            if (scaleX && !scaleY)
            {
                double normalization = (double)targetSx / sourceSx;
                for (long y = 0; y < targetSy; y++)
                {
                    long sourceRow = sourceFirst + y * sourceDy;
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        var span = xSpans[x];
                        double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                        for (long sx = span.First + 1; sx < span.Last; sx++)
                            sum += source[sourceRow + sx * sourceDx];
                        sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = value;
                    }
                }
                return;
            }

            if (!scaleX)
            {
                double normalization = (double)targetSy / sourceSy;
                for (long y = 0; y < targetSy; y++)
                {
                    var span = ySpans[y];
                    long targetRow = targetFirst + y * targetDy;
                    for (long x = 0; x < targetSx; x++)
                    {
                        long sourceColumn = sourceFirst + x * sourceDx;
                        double sum = source[sourceColumn + span.First * sourceDy] * span.FirstWeight;
                        for (long sy = span.First + 1; sy < span.Last; sy++)
                            sum += source[sourceColumn + sy * sourceDy];
                        sum += source[sourceColumn + span.Last * sourceDy] * span.LastWeight;
                        double value = sum * normalization;
                        target[targetRow + x * targetDx] = value;
                    }
                }
                return;
            }

            double xNormalization = (double)targetSx / sourceSx;
            long workspaceIndex = 0;
            for (long y = 0; y < sourceSy; y++)
            {
                long sourceRow = sourceFirst + y * sourceDy;
                for (long x = 0; x < targetSx; x++)
                {
                    var span = xSpans[x];
                    double sum = source[sourceRow + span.First * sourceDx] * span.FirstWeight;
                    for (long sx = span.First + 1; sx < span.Last; sx++)
                        sum += source[sourceRow + sx * sourceDx];
                    sum += source[sourceRow + span.Last * sourceDx] * span.LastWeight;
                    workspace[workspaceIndex++] = sum * xNormalization;
                }
            }

            double yNormalization = (double)targetSy / sourceSy;
            for (long y = 0; y < targetSy; y++)
            {
                var span = ySpans[y];
                long targetRow = targetFirst + y * targetDy;
                for (long x = 0; x < targetSx; x++)
                {
                    double sum = workspace[span.First * targetSx + x] * span.FirstWeight;
                    for (long sy = span.First + 1; sy < span.Last; sy++)
                        sum += workspace[sy * targetSx + x];
                    sum += workspace[span.Last * targetSx + x] * span.LastWeight;
                    double value = sum * yNormalization;
                    target[targetRow + x * targetDx] = value;
                }
            }
        }

        #endregion
    }
}

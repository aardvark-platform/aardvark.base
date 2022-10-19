using System;

namespace Aardvark.Base
{
    /// <summary>
    /// Wrappers for the best (fastest) available implementation of the respective tensor operation.
    /// </summary>
    public static partial class TensorExtensions
    {
        #region Image Scaling

        //# var intConfigs = new []
        //# {
        //#     Tup.Create(Meta.ByteType,    Meta.FloatType,  "Fun",  "Fun"),
        //#     Tup.Create(Meta.UShortType,  Meta.FloatType,  "Fun",  "Fun"),
        //#     Tup.Create(Meta.UIntType,    Meta.DoubleType, "Fun",  "Fun"),
        //#     Tup.Create(Meta.HalfType,    Meta.HalfType,   "Fun",  "Fun"),
        //#     Tup.Create(Meta.FloatType,   Meta.FloatType,  "Fun",  "Fun"),
        //#     Tup.Create(Meta.DoubleType,  Meta.DoubleType, "Fun",  "Fun"),
        //#
        //#     Tup.Create(Meta.ByteType,    Meta.FloatType,  "C3b",  "C3f"),
        //#     Tup.Create(Meta.UShortType,  Meta.FloatType,  "C3us", "C3f"),
        //#     Tup.Create(Meta.UIntType,    Meta.DoubleType, "C3ui", "C3d"),
        //#     Tup.Create(Meta.FloatType,   Meta.FloatType,  "C3f",  "C3f"),
        //#     Tup.Create(Meta.DoubleType,  Meta.DoubleType, "C3d",  "C3d"),
        //#
        //#     Tup.Create(Meta.ByteType,    Meta.FloatType,  "C4b",  "C4f"),
        //#     Tup.Create(Meta.UShortType,  Meta.FloatType,  "C4us", "C4f"),
        //#     Tup.Create(Meta.UIntType,    Meta.DoubleType, "C4ui", "C4d"),
        //#     Tup.Create(Meta.FloatType,   Meta.FloatType,  "C4f",  "C4f"),
        //#     Tup.Create(Meta.DoubleType,  Meta.DoubleType, "C4d",  "C4d"),
        //# };
        //# intConfigs.ForEach((dtype, ftype, ct, fct) => {
        //#     var isReal = dtype.IsReal;
        //#     var fun = ct == "Fun" ? ct : "Col";
        //#     var clampVal = !isReal && ct == "Fun";
        //#     var clampMap = !isReal && ct != "Fun";
        //#     var dt = dtype.Name;
        //#     var dtn = dtype.Caps;
        //#     var ft = ftype.Name;
        //#     var ftn = ftype.Caps;
        //#     var ftc = ftype.Char;
        //#     var rfct = isReal ? "" : "Raw" + ftc.ToUpper();
        //#     var dtct = ct == "Fun" ? dt : dt + ", " + ct;
        //#     var it = ct == "Fun" ? dt : ct;
        public static void SetScaledNearest(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat)
        {
            targetMat.SetScaledLinear(sourceMat, (x, a, b) => x < 0.5 ? a : b,
                                                 (x, a, b) => x < 0.5 ? a : b);
        }

        /// <summary>
        /// Use supplied linear interpolators in x and y to scale the source matrix into the target
        /// matrix.
        /// </summary>
        public static void SetScaledLinear<T1>(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat,
                                           Func<double, __it__, __it__, T1> xinterpolator,
                                           Func<double, T1, T1, __it__> yinterpolator)
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
        public static void SetScaledCubic(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4__ftc__(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledBSpline3(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat)
        {
            targetMat.SetScaledCubic(sourceMat, Fun.BSpline3__ftc__);
        }

        /// <summary>
        /// Use a supplied cubic interpolator to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledCubic(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat,
                                          Func<double, Tup4<__ft__>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, __fun__.LinCom__rfct__, __fun__.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped/*#
                                  if (clampVal) { */,
                                  Col.__dtn__In__ftn__To__dtn__Clamped/*#
                                  } else if (clampMap) { */,
                                  col => col.Map(Col.__dtn__In__ftn__To__dtn__Clamped)/*# } */);
        }

        public static void SetScaledBSpline5(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.BSpline5__ftc__);
        }

        /// <summary>
        /// Use Lanczos Interpolation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat)
        {
            targetMat.SetScaledOrder5(sourceMat, Fun.Lanczos3__ftc__);
        }

        public static void SetScaledOrder5(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat,
                                           Func<double, Tup6<__ft__>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  interpolator, interpolator, __fun__.LinCom__rfct__, __fun__.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped/*#
                                  if (clampVal) { */,
                                  Col.__dtn__In__ftn__To__dtn__Clamped/*#
                                  } else if (clampMap) { */,
                                  col => col.Map(Col.__dtn__In__ftn__To__dtn__Clamped)/*# } */);
        }

        //# }); // configs
        #endregion
    }
}
using System;

namespace Aardvark.Base
{
    public partial class PixImage
    {
        private static readonly V3d[] s_v0 =
            { new(-1, -1, 1), new(-1,  1, 1), new(1,  1,  1),
              new( 1, -1, 1), new(-1, -1, 1), new(-1, 1, -1) };

        private static readonly V3d[] s_dx =
            { new( 0, 2, 0), new(2, 0, 0), new(0, -2, 0),
              new(-2, 0, 0), new(2, 0, 0), new(2,  0, 0) };

        private static readonly V3d[] s_dy =
            { new(0, 0, -2), new(0, 0, -2), new(0,  0, -2),
              new(0, 0, -2), new(0, 2,  0), new(0, -2,  0) };

        /// <summary>
        /// Creates a single cube-map side image by evaluating a function on the unit cube directions.
        /// The cube faces are arranged in a right-handed layout (XN YP XP YN ZP ZN) as illustrated below.
        /// <code>
        /// UV ->
        /// |       | 4 |
        /// v   | 0 | 1 | 2 | 3 |
        ///         | 5 |
        /// </code>
        /// </summary>
        /// <typeparam name="T">The pixel channel storage type (e.g. byte, float).</typeparam>
        /// <typeparam name="Tcol">The color vector type used to write into the matrix (e.g. C3b, C4f).</typeparam>
        /// <param name="cubeSide">Index of the cube side in the range [0..5]. See <see cref="CubeSide"/>.</param>
        /// <param name="size">The width and height of the square side in pixels.</param>
        /// <param name="channelCount">Number of channels in the created image.</param>
        /// <param name="vec_colFun">Function that maps a normalized 3D direction vector to a color value.</param>
        /// <returns>A newly created image containing the requested cube side.</returns>
        public static PixImage<T> CreateCubeMapSide<T, Tcol>(
                int cubeSide, int size, int channelCount, Func<V3d, Tcol> vec_colFun)
        {
            var image = new PixImage<T>(size, size, channelCount);
            var mat = image.GetMatrix<Tcol>();
            var d = 1.0 / size;
            V3d dx = d * s_dx[cubeSide], dy = d * s_dy[cubeSide], v0 = s_v0[cubeSide] + 0.5 * dx + 0.5 * dy;
            mat.SetByCoordParallelY((x, y) => { var v = v0 + x * dx + y * dy; return vec_colFun(v.Normalized); });
            return image;
        }

        /// <summary>
        /// Creates a single monochrome cube-map side image by evaluating a function on the unit cube directions.
        /// The cube faces are arranged in a right-handed layout (XN YP XP YN ZP ZN) as illustrated below.
        /// <code>
        /// UV ->
        /// |       | 4 |
        /// v   | 0 | 1 | 2 | 3 |
        ///         | 5 |
        /// </code>
        /// </summary>
        /// <typeparam name="T">The pixel storage type (e.g. byte, float).</typeparam>
        /// <param name="cubeSide">Index of the cube side in the range [0..5]. See <see cref="CubeSide"/>.</param>
        /// <param name="size">The width and height of the square side in pixels.</param>
        /// <param name="vec_valueFun">Function that maps a normalized 3D direction vector to a channel value.</param>
        /// <returns>A newly created single-channel image containing the requested cube side.</returns>
        public static PixImage<T> CreateCubeMapSide<T>(
                int cubeSide, int size, Func<V3d, T> vec_valueFun)
        {
            var image = new PixImage<T>(size, size, 1);
            var mat = image.Matrix;
            var d = 1.0 / size;
            V3d dx = d * s_dx[cubeSide], dy = d * s_dy[cubeSide], v0 = s_v0[cubeSide] + 0.5 * dx + 0.5 * dy;
            mat.SetByCoordParallelY((x, y) => { var v = v0 + x * dx + y * dy; return vec_valueFun(v.Normalized); });
            return image;
        }

        /// <summary>
        /// Creates an image representing a cylindrical environment parameterized by (phi, theta).
        /// </summary>
        /// <typeparam name="T">The pixel channel storage type.</typeparam>
        /// <typeparam name="Tcol">The color vector type used to write into the matrix.</typeparam>
        /// <param name="width">Width of the generated image in pixels.</param>
        /// <param name="height">Height of the generated image in pixels.</param>
        /// <param name="channelCount">Number of channels in the created image.</param>
        /// <param name="phi_theta_colFun">Function mapping (phi, theta) to a color value, where phi is in [0, 2pi) and theta in [0, pi/2].</param>
        /// <returns>The generated image.</returns>
        public static PixImage<T> CreateCylinder<T, Tcol>(
                int width, int height, int channelCount,
                Func<double, double, Tcol> phi_theta_colFun)
        {
            var image = new PixImage<T>(width, height, channelCount);
            var mat = image.GetMatrix<Tcol>();
            double dx = Constant.PiTimesTwo / width, dy = 1.0 / height;
            double x0 = 0.5 * dx, y0 = 1.0 - 0.5 * dy;

            mat.SetByCoordParallelY((x, y) => phi_theta_colFun(x0 + x * dx, Fun.Acos(y0 - y * dy)));

            return image;
        }

        /// <summary>
        /// Creates an image representing a hemispherical dome parameterized by (phi, theta).
        /// </summary>
        /// <typeparam name="T">The pixel channel storage type.</typeparam>
        /// <typeparam name="Tcol">The color vector type used to write into the matrix.</typeparam>
        /// <param name="width">Width of the generated image in pixels.</param>
        /// <param name="height">Height of the generated image in pixels.</param>
        /// <param name="channelCount">Number of channels in the created image.</param>
        /// <param name="phi_theta_colFun">Function mapping (phi, theta) to a color value, where phi is in [0, 2pi) and theta in [0, pi/2].</param>
        /// <returns>The generated image.</returns>
        public static PixImage<T> CreateDome<T, Tcol>(
                int width, int height, int channelCount,
                Func<double, double, Tcol> phi_theta_colFun)
        {
            var image = new PixImage<T>(width, height, channelCount);
            var mat = image.GetMatrix<Tcol>();
            double dx = Constant.PiTimesTwo / width, dy = Constant.PiHalf / height;
            double x0 = 0.5 * dx, y0 = 0.5 * dy;

            mat.SetByCoordParallelY((x, y) => phi_theta_colFun(x0 + x * dx, y0 + y * dy));

            return image;
        }
    }
}

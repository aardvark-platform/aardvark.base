using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;

namespace Aardvark.Base
{
    public partial class PixImage
    {
        private static readonly V3d[] s_v0 =
            new V3d[] { new V3d(-1, 1, 1), new V3d(-1, -1, 1), new V3d(1, -1, 1),
                        new V3d(1, 1, 1), new V3d(-1, 1, 1), new V3d(-1, -1, -1) };
        private static V3d[] s_dx =
            new V3d[] { new V3d(0, -2, 0), new V3d(2, 0, 0), new V3d(0, 2, 0),
                        new V3d(-2, 0, 0), new V3d(2, 0, 0), new V3d(2, 0, 0) };
        private static V3d[] s_dy =
            new V3d[] { new V3d(0, 0, -2), new V3d(0, 0, -2), new V3d(0, 0, -2),
                        new V3d(0, 0, -2), new V3d(0, -2, 0), new V3d(0, 2, 0) };

        /// <summary>
        /// Returns 6 square PixImages of the supplied size containing the
        /// textures of a cube map in the following arrangement:
        ///         | 4 |
        ///     | 0 | 1 | 2 | 3 |
        ///         | 5 |
        /// </summary>
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

        //public static PixImage<byte> CreateSkyCubeMapSideC4b(
        //        int cubeSide, int size, IPhysicalSky sky, C3f groundColor)
        //{
        //    var gc = groundColor.ToC4b();
        //    if (cubeSide < 4)
        //        return CreateCubeMapSide<byte, C4b>(
        //                    cubeSide, size, 4, v => v.Z < 0.0 ? gc : sky.GetRadiance(v).ToC4b());
        //    if (cubeSide == 4)
        //        return CreateCubeMapSide<byte, C4b>(
        //                    cubeSide, size, 4, v => sky.GetRadiance(v).ToC4b());
        //    else
        //        return CreateCubeMapSide<byte, C4b>(cubeSide, size, 4, v => gc);
        //}

        //public static PixImage<byte>[] CreateSkyCubeMapC4b(
        //        int size, IPhysicalSky sky, C3f groundColor)
        //{
        //    return new PixImage<byte>[6].SetByIndex(i => CreateSkyCubeMapSideC4b(i, size, sky, groundColor));
        //}

        //public static PixImage<byte>[] CreateSkyCubeMapC4b(int size, IPhysicalSky sky)
        //{
        //    return new PixImage<byte>[6].SetByIndex(i => CreateCubeMapSide<byte, C4b>(i, size, 4,
        //                                                        v => sky.GetRadiance(v).ToC4b()));
        //}

        //public static PixImage<float>[] CreateSkyCubeMapC3f(int size, IPhysicalSky sky)
        //{
        //    return new PixImage<float>[6].SetByIndex(i => CreateCubeMapSide<float, C3f>(i, size, 3,
        //                                                        v => sky.GetRadiance(v)));
        //}

        //public static PixImage<float>[] CreateSkyCubeMapC4f(int size, IPhysicalSky sky)
        //{
        //    return new PixImage<float>[6].SetByIndex(i => CreateCubeMapSide<float, C4f>(i, size, 4,
        //                                                        v => sky.GetRadiance(v).ToC4f()));
        //}


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

        //public static PixImage<byte> CreateSkyCylinderC4b(int width, int height, IPhysicalSky sky)
        //{
        //    return CreateCylinder<byte, C4b>(width, height, 4, (phi, theta) => sky.GetRadiance(phi, theta).ToC4b());
        //}

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

        //public static PixImage<byte> CreateSkyDomeC4b(int width, int height, IPhysicalSky sky)
        //{
        //    return CreateDome<byte, C4b>(width, height, 4, (phi, theta) => sky.GetRadiance(phi, theta).ToC4b());
        //}

    }
}

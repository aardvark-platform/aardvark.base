using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static partial class Tensor
    {
        public static Matrix<Td> SetByCoordParallelX<Td>(
                this Matrix<Td> mat, Func<long, long, Td> x_y_valueFun)
        {
            Parallel.For(mat.FX, mat.EX, x => mat.SubYVectorWindow(x).SetByCoord(y => x_y_valueFun(x, y)));
            return mat;
        }

        public static Matrix<Td, Tv> SetByCoordParallelX<Td, Tv>(
                this Matrix<Td, Tv> mat, Func<long, long, Tv> x_y_valueFun)
        {
            Parallel.For(mat.FX, mat.EX, x => mat.SubYVectorWindow(x).SetByCoord(y => x_y_valueFun(x, y)));
            return mat;
        }

        public static Matrix<Td> SetByCoordParallelY<Td>(
                this Matrix<Td> mat, Func<long, long, Td> x_y_valueFun)
        {
            Parallel.For(mat.FY, mat.EY, y => mat.SubXVectorWindow(y).SetByCoord(x => x_y_valueFun(x, y)));
            return mat;
        }

        public static Matrix<Td, Tv> SetByCoordParallelY<Td, Tv>(
                this Matrix<Td, Tv> mat, Func<long, long, Tv> x_y_valueFun)
        {
            Parallel.For(mat.FY, mat.EY, y => mat.SubXVectorWindow(y).SetByCoord(x => x_y_valueFun(x, y)));
            return mat;
        }

        public static Volume<Td> SetByCoordParallelX<Td>(
                this Volume<Td> vol, Func<long, long, long, Td> x_y_z_valueFun)
        {
            Parallel.For(vol.FX, vol.EX, x => vol.SubYZMatrixWindow(x).SetByCoord((y, z) => x_y_z_valueFun(x, y, z)));
            return vol;
        }

        public static Volume<Td, Tv> SetByCoordParallelX<Td, Tv>(
                this Volume<Td, Tv> vol, Func<long, long, long, Tv> x_y_z_valueFun)
        {
            Parallel.For(vol.FX, vol.EX, x => vol.SubYZMatrixWindow(x).SetByCoord((y, z) => x_y_z_valueFun(x, y, z)));
            return vol;
        }

        public static Volume<Td> SetByCoordParallelY<Td>(
                this Volume<Td> vol, Func<long, long, long, Td> x_y_z_valueFun)
        {
            Parallel.For(vol.FY, vol.EY, y => vol.SubXZMatrixWindow(y).SetByCoord((x, z) => x_y_z_valueFun(x, y, z)));
            return vol;
        }

        public static Volume<Td, Tv> SetByCoordParallelY<Td, Tv>(
                this Volume<Td, Tv> vol, Func<long, long, long, Tv> x_y_z_valueFun)
        {
            Parallel.For(vol.FY, vol.EY, y => vol.SubXZMatrixWindow(y).SetByCoord((x, z) => x_y_z_valueFun(x, y, z)));
            return vol;
        }

        public static Volume<Td> SetByCoordParallelZ<Td>(
                this Volume<Td> vol, Func<long, long, long, Td> x_y_z_valueFun)
        {
            Parallel.For(vol.FZ, vol.EZ, z => vol.SubXYMatrixWindow(z).SetByCoord((x, y) => x_y_z_valueFun(x, y, z)));
            return vol;
        }

        public static Volume<Td, Tv> SetByCoordParallelZ<Td, Tv>(
                this Volume<Td, Tv> vol, Func<long, long, long, Tv> x_y_z_valueFun)
        {
            Parallel.For(vol.FZ, vol.EZ, z => vol.SubXYMatrixWindow(z).SetByCoord((x, y) => x_y_z_valueFun(x, y, z)));
            return vol;
        }

    }
}

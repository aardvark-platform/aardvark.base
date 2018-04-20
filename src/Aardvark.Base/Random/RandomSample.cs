namespace Aardvark.Base
{
    public static class RandomSample
    {
        /// <summary>
        /// Uses the 2 random series (seriesIndex, seriesIndex+1)
        /// </summary>
        public static V3d Spherical(
                IRandomSeries rnds, int seriesIndex)
        {
            double phi = Constant.PiTimesTwo * rnds.UniformDouble(seriesIndex);
            double z = 1.0 - 2.0 * rnds.UniformDouble(seriesIndex + 1);
            double r = Fun.Max(1.0 - z * z, 0.0).Sqrt();
            return new V3d(r * phi.Cos(), r * phi.Sin(), z);
        }

        /// <summary>
        /// Supplied normal MUST be normalized, uses the 2 random series
        /// (seriesIndex, seriesIndex+1).
        /// </summary>
        public static V3d Lambertian(
                V3d normal, IRandomSeries rnds, int seriesIndex)
        {
            V3d vec;
            double squareLen;

            do
            {
                double phi = Constant.PiTimesTwo * rnds.UniformDouble(seriesIndex);
                double z = 1.0 - 2.0 * rnds.UniformDouble(seriesIndex + 1);
                double r = Fun.Max(1.0 - z * z, 0.0).Sqrt();
                vec = new V3d(r * phi.Cos(), r * phi.Sin(), z) + normal;
                squareLen = vec.LengthSquared;
            }
            while (squareLen < 0.000001);

            vec *= 1.0 / squareLen.Sqrt();
            return vec;
        }
    }
}

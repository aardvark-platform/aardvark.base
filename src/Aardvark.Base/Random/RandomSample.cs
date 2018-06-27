namespace Aardvark.Base
{
    public static class RandomSample
    {
        /// <summary>
        /// Uses the 2 random series (seriesIndex, seriesIndex+1) to generate a random point on a sphere.
        /// </summary>
        public static V3d Spherical(IRandomSeries rnds, int seriesIndex)
        {
            return Spherical(rnds.UniformDouble(seriesIndex), rnds.UniformDouble(seriesIndex + 1));
        }

        /// <summary>
        /// Uses the 2 random variables x1 and x2 to generate a random point on a sphere.
        /// </summary>
        public static V3d Spherical(double x1, double x2)
        {
            double phi = Constant.PiTimesTwo * x1;
            double z = 1.0 - 2.0 * x2;
            double r = Fun.Max(1.0 - z * z, 0.0).Sqrt();
            return new V3d(r * phi.Cos(), r * phi.Sin(), z);
        }

        /// <summary>
        /// Supplied normal MUST be normalized, uses the 2 random series
        /// (seriesIndex, seriesIndex+1).
        /// </summary>
        public static V3d Lambertian(V3d normal, IRandomSeries rnds, int seriesIndex)
        {
            return Lambertian(normal, 
                rnds.UniformDouble(seriesIndex),
                rnds.UniformDouble(seriesIndex + 1));
        }

        /// <summary>
        /// Supplied normal MUST be normalized, uses the 2 random variables x1 and x2.
        /// </summary>
        public static V3d Lambertian(V3d normal, double x1, double x2)
        {
            // random point on cylinder barrel
            var phi = Constant.PiTimesTwo * x1;
            var z = 1.0 - 2.0 * x2;
            // project to sphere
            var r = Fun.Max(1.0 - z * z, 0.0).Sqrt();
            var vec = new V3d(r * phi.Cos(), r * phi.Sin(), z) + normal;
            var squareLen = vec.LengthSquared;

            // check if random sphere point was perfectly opposite of the normal direction (in case x2 ~= 1)
            if (squareLen < 1e-9)
                return normal;

            var norm = 1.0 / squareLen.Sqrt();
            return vec * norm;
        }
        
        /// <summary>
        /// Generates a uniform distributed 2d random sample on a disk with radius 1.
        /// It uses two random series (seriesIndex, seriesIndex+1).
        /// </summary>
        public static V2d Disk(IRandomSeries rnd, int seriesIndex)
        {
            return Disk(rnd.UniformDouble(seriesIndex), rnd.UniformDouble(seriesIndex + 1));
        }

        /// <summary>
        /// Generates a uniform distributed 2d random sample on a disk with radius 1 
        /// using the 2 random variables x1 and x2.
        /// </summary>
        public static V2d Disk(double x1, double x2)
        {
            // random direction
            var phi = x1 * Constant.PiTimesTwo;
            // random radius transformed by sqrt to result in area equivalent samples distribution
            var r = x2.Sqrt();
            return new V2d(r * Fun.Cos(phi),
                           r * Fun.Sin(phi));
        }
    }
}

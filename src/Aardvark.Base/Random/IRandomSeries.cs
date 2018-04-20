namespace Aardvark.Base
{
    /// <summary>
    /// Maintain multiple independent series of random or quasi-random
    /// variables. For two-dimensional sampling the random variables of two
    /// series need to be combined (e.g. a random variable of series k and
    /// a random variable of series k+1). For quasi-random sampling, the
    /// lower series are assumed to achieve a faster coverage of the domain,
    /// and should therefore be used for the sampling dimensions with a 
    /// larger influence on the result.
    /// </summary>
    public interface IRandomSeries
    {
        double UniformDouble(int seriesIndex);
    }

    public static class RandomSeriesExtensions
    {
        /// <summary>
        /// Returns a uniformly distributed vector (corresponds to a
        /// uniformly distributed point on the unit sphere) by using
        /// 2 random values from the series with the supplied indices.
        /// Note, that the returned vector will never be equal to [0, 0, -1].
        /// </summary>
        public static V3d UniformV3dDirection(this IRandomSeries rnd, int si0, int si1)
        {
            double phi = rnd.UniformDouble(si0) * Constant.PiTimesTwo;
            double z = 1.0 - rnd.UniformDouble(si1) * 2.0;
            double s = System.Math.Sqrt(1.0 - z * z);
            return new V3d(System.Math.Cos(phi) * s, System.Math.Sin(phi) * s, z);
        }
    }
}

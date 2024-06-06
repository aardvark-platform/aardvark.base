namespace Aardvark.Base
{
    /// <summary>
    /// A halton series generator, that uses a normal random generator for
    /// higher order series, in order to avoid the slow coverage of higher
    /// order halton series.
    /// </summary>
    public class HaltonRandomSeries : IRandomSeries
    {
	    public HaltonRandomSeries(
	            int haltonCount, IRandomUniform randomUniform)
        {
            m_haltonStateArray = new double[haltonCount].SetByIndex(
                                        i => randomUniform.UniformDouble());
            m_randomUniform = randomUniform;
        }

	    public double UniformDouble(int seriesIndex)
        {
            if (seriesIndex < m_haltonStateArray.Length)
	        {
                double value = Quasi.QuasiHaltonWithIndex(
                                seriesIndex, m_haltonStateArray[seriesIndex]);
	            m_haltonStateArray[seriesIndex] = value;
	            return value;
	        }
	        else
                return m_randomUniform.UniformDouble();
        }

	    private readonly double [] m_haltonStateArray;
        private readonly IRandomUniform m_randomUniform;
    }
}

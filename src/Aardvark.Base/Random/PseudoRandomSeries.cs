using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    /// <summary>
    /// Uses an IRandomUniform generator to create an IRandomSeries.
    /// The series that are generated will not correlated.
    /// </summary>
    public class PseudoRandomSeries : IRandomSeries
    {
        IRandomUniform m_rnd;

        public PseudoRandomSeries(IRandomUniform rnd)
        {
            m_rnd = rnd;
        }

        public double UniformDouble(int seriesIndex)
        {
            return m_rnd.UniformDouble();
        }
    }
}

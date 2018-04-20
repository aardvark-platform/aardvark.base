using System;

namespace Aardvark.Base
{
    public class RandomGaussian : IRandomDistribution
    {
        private IRandomUniform m_rndUniform;
        private double m_cachedValue;

        public RandomGaussian(IRandomUniform rndUniform)
        {
            m_rndUniform = rndUniform;
            m_cachedValue = Double.NaN;
        }

        #region IRandomDistribution Members

        /// <summary>
        /// Returns a gaussian distributed double.
        /// </summary>
        public double GetDouble()
        {
            double value;

            if (!Double.IsNaN(m_cachedValue))
            {
                value = m_cachedValue;
                m_cachedValue = Double.NaN;
            }
            else
            {
                // using the polar form of the Box-Muller transformation to
                // transform two random uniform values to two gaussian
                // distributed values
                double x1, x2, w;
                do
                {
                    x1 = 2.0 * m_rndUniform.UniformDouble() - 1.0;
                    x2 = 2.0 * m_rndUniform.UniformDouble() - 1.0;
                    w = x1 * x1 + x2 * x2;
                } while (w >= 1.0);

                w = Math.Sqrt((-2.0 * Math.Log(w)) / w);
                value = x1 * w;
                m_cachedValue = x2 * w;
            }

            return value;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Returns a gaussian distributed double given a mean and standard deviation.
        /// </summary>
        public double GetDouble(double mean, double standardDeviation)
            => GetDouble() * standardDeviation + mean;

        #endregion
    }
}

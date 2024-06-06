using System;

namespace Aardvark.Base
{
    /// <summary>
    /// Generator of normal distributed random values.
    /// The generator uses the Box-Muller transformation to generate two samples at 
    /// once and thereby is superior to <see cref="IRandomUniformExtensions.Gaussian" /> if multiple samples are required.
    /// </summary>
    public class RandomGaussian : IRandomDistribution
    {
        private readonly IRandomUniform m_rndUniform;
        private double m_cachedValue;

        public RandomGaussian(IRandomUniform rndUniform)
        {
            m_rndUniform = rndUniform;
            m_cachedValue = Double.NaN;
        }

        #region IRandomDistribution Members

        /// <summary>
        /// Returns a Gaussian distributed double.
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
                // transform two random uniform values to two Gaussian
                // distributed values
                double x1, x2, w;
                do
                {
                    x1 = 2.0 * m_rndUniform.UniformDouble() - 1.0;
                    x2 = 2.0 * m_rndUniform.UniformDouble() - 1.0;
                    w = x1 * x1 + x2 * x2;
                } while (w >= 1.0);

                w = Fun.Sqrt((-2.0 * Fun.Log(w)) / w);
                value = x1 * w;
                m_cachedValue = x2 * w;
            }

            return value;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Returns a Gaussian distributed double given a mean and standard deviation.
        /// </summary>
        public double GetDouble(double mean, double standardDeviation)
            => GetDouble() * standardDeviation + mean;

        #endregion
    }
}

using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents a discrete function that is represented by a probability density function (PDF)
    /// and a cumulative density function (CDF).
    /// </summary>
    public class DistributionFunction
    {
        double[] m_pdf;
        double[] m_cdf;

        /// <summary>
        /// Gets the input probability density function.
        /// It is not necessarily normalized.
        /// </summary>
        public double[] PDF { get { return m_pdf; } }

        /// <summary>
        /// Gets the calculated cumulative density function with +1 elements than the PDF.
        /// It is not necessarily normalized.
        /// </summary>
        public double[] CDF { get { return m_cdf; } }

        /// <summary>
        /// Returns the normalization factor of the PDF and CDF.
        /// In case the supplied PDF is not normalized this factor will be != 1.0 and
        /// needs to be considered when interpreting the raw PDF or CDF values.
        /// </summary>
        public double Norm { get { return m_cdf[m_cdf.Length - 1]; } }

        /// <summary>
        /// Create distribution from discrete probability distribution function (PDF).
        /// The PDF does not need to be normalized.
        /// </summary>
        public DistributionFunction(double[] pdf)
        {
            m_pdf = pdf;
            m_cdf = m_pdf.Integrated();
        }

        /// <summary>
        /// Retrieve random index distributed proportional to probability density function.
        /// Uses binary search O(log n).
        /// </summary>
        public int Sample(IRandomUniform rnd)
        {
            return Sample(rnd.UniformDouble());
        }

        /// <summary>
        /// Retrieve index to the discrete PDF where its cumulative distribution matches the random variable x1 [0, 1].
        /// Uses binary search O(log n).
        /// </summary>
        public int Sample(double x1)
        {
            var range = m_cdf.Length;
            var valueToFind = x1 * m_cdf[range - 1]; // x1 * sum -> CDF threshold to find

            var i0 = 0;
            // binary search
            while (range > 0)
            {
                var halfRange = range >> 1;
                var center = i0 + halfRange;

                // check if value is left or right
                if (m_cdf[center] <= valueToFind)
                {
                    // right half
                    i0 = center + 1;
                    range -= halfRange + 1;
                }
                else
                {
                    // left half
                    range = halfRange;
                }
            }
            return Fun.Clamp(i0 - 1, 0, m_cdf.Length - 2);
        }

        /// <summary>
        /// Gets the normalized probability density function value at the supplied index.
        /// The function is normalized to integrate to a value of 1.
        /// </summary>
        public double PDFValue(int index)
        {
            return m_pdf[index] / this.Norm;
        }

        /// <summary>
        /// Gets the normalized cumulative distribution function value at the supplied index.
        /// The function is normalized to integrate to a value of 1.
        /// </summary>
        public double CDFValue(int index)
        {
            return m_cdf[index] / this.Norm;
        }
    }
}

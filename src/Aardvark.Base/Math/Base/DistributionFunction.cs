using System.Linq;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents a discrete function that is represented by a probability density function (PDF)
    /// and a cumulative distribution function (CDF).
    /// </summary>
    public class DistributionFunction
    {
        readonly double[] m_pdf;
        readonly double[] m_cdf;

        /// <summary>
        /// Gets the input probability density function.
        /// It is not necessarily normalized.
        /// </summary>
        public double[] PDF { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return m_pdf; } }

        /// <summary>
        /// Gets the calculated cumulative distribution function with +1 elements than the PDF.
        /// It is not necessarily normalized.
        /// </summary>
        public double[] CDF { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return m_cdf; } }

        /// <summary>
        /// Returns the normalization factor of the PDF and CDF.
        /// In case the supplied PDF is not normalized this factor will be != 1.0 and
        /// needs to be considered when interpreting the raw PDF or CDF values.
        /// </summary>
        public double Norm { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return m_cdf[m_cdf.Length - 1]; } }

        /// <summary>
        /// Create distribution from a discrete probability distribution function (PDF).
        /// The PDF does not need to be normalized.
        /// </summary>
        public DistributionFunction(double[] pdf)
        {
            m_pdf = pdf;
            m_cdf = m_pdf.Integrated();
        }

        /// <summary>
        /// Create distribution from a discrete probability distribution function (PDF) 
        /// and its cumulative distribution function (CDF).
        /// The PDF does not need to be normalized.
        /// </summary>
        public DistributionFunction(double[] pdf, double[] cdf)
        {
            m_pdf = pdf;
            m_cdf = cdf;
        }

        /// <summary>
        /// Retrieve random index distributed proportional to probability density function.
        /// For a valid CDF with finite positive total, a threshold rounded to the total
        /// selects the final positive-mass interval, skipping trailing zero-weight bins.
        /// Uses binary search O(log n).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sample(IRandomUniform rnd)
        {
            return Sample(rnd.UniformDouble());
        }

        /// <summary>
        /// Retrieve index to the discrete PDF where its cumulative distribution matches the random variable x1 [0, 1].
        /// For a valid CDF with finite positive total, a threshold equal to the total
        /// selects the final positive-mass interval, including x1 == 1 and rounded interior draws.
        /// Uses binary search O(log n).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sample(double x1)
        {
            return SampleCDF(m_cdf, x1);
        }

        /// <summary>
        /// Retrieve random index of a discretized PDF given its cumulative distribution function (CDF).
        /// For a valid CDF with finite positive total, a threshold rounded to the total
        /// selects the final positive-mass interval, skipping trailing zero-weight bins.
        /// Uses binary search O(log n).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SampleCDF(double[] cdf, IRandomUniform rnd)
        {
            return SampleCDF(cdf, rnd.UniformDouble());
        }

        /// <summary>
        /// Retrieve index of a discrete PDF given its cumulative distribution function (CDF) and a random variable x1 [0, 1].
        /// For a valid CDF with finite positive total, a threshold equal to the total
        /// selects the final positive-mass interval, including x1 == 1 and rounded interior draws.
        /// Uses binary search O(log n).
        /// </summary>
        public static int SampleCDF(double[] cdf, double x1)
        {
            var range = cdf.Length;
            var total = cdf[range - 1];
            var valueToFind = x1 * total; // x1 * sum -> CDF threshold to find

            var i0 = 0;
            if (valueToFind == total && total > 0 && total < double.PositiveInfinity && x1 <= 1)
            {
                // Find the start of the terminal plateau, not its last zero-weight bin.
                var end = range - 1;
                while (i0 < end)
                {
                    var center = i0 + ((end - i0) >> 1);
                    if (cdf[center] < total) i0 = center + 1;
                    else end = center;
                }
                return Fun.Clamp(i0 - 1, 0, cdf.Length - 2);
            }

            // binary search
            while (range > 0)
            {
                var halfRange = range >> 1;
                var center = i0 + halfRange;

                // check if value is left or right
                if (cdf[center] <= valueToFind)
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
            return Fun.Clamp(i0 - 1, 0, cdf.Length - 2);
        }

        /// <summary>
        /// Gets the normalized probability density function value at the supplied index.
        /// The function is normalized to integrate to a value of 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double PDFValue(int index)
        {
            return m_pdf[index] / this.Norm;
        }

        /// <summary>
        /// Gets the normalized cumulative distribution function value at the supplied index.
        /// The function is normalized to integrate to a value of 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CDFValue(int index)
        {
            return m_cdf[index] / this.Norm;
        }

        /// <summary>
        /// Inplace update of the CDF by re-integerating the PDF.
        /// </summary>
        public void UpdateCDF()
        {
            var cdf = m_cdf;
            var pdf = m_pdf;
            cdf[0] = 0;
            double acc = 0;
            for (int i = 0; i < pdf.Length;)
            {
                acc += pdf[i++];
                cdf[i] = acc;
            }
        }
    }
}

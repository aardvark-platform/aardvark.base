using System.IO;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides an IRandomSeries view of a pre-computed 2d random sequence.
    /// </summary>
    public class ForcedRandomSeries : IRandomSeries
    {
        V2i m_index = V2i.OO;
        V2i[] m_series;
        double m_norm;
        V2i m_seed;

        /// <summary>
        /// Reads a file that contains a raw V2i binary array as V2i[].
        /// The binary data is supposed to contain NxN points as (int, int) in random order.
        /// </summary>
        public static V2i[] ReadSeries(string frsSqFile)
        {
            var bytes = File.ReadAllBytes(frsSqFile);
            var matSize = (int)(bytes.Length / 8).Sqrt();

            // V2i[] encoded in byte array must be NxN and a multiple of 8.
            if (matSize * matSize * 8 != bytes.Length)
                throw new InvalidDataException("Forced Random series data has invalid length.");

            return bytes.UnsafeCoerce<V2i>();
        }

        /// <summary>
        /// Create a ForcedRandomSeries with the given sample sequence and the seed as offset.
        /// The sequence is supposed to contain NxN points in random order.
        /// </summary>
        public ForcedRandomSeries(V2i[] series, V2i seed)
        {
            m_series = series;
            m_norm = 1.0 / m_series.Length.Sqrt(); // [0, 1) 
            m_seed = seed;
        }

        /// <summary>
        /// Create a ForcedRandomSeries from the given sequence data file 
        /// and the seed as offset.
        /// </summary>
        public ForcedRandomSeries(string frsSqFile, V2i seed)
            : this(ReadSeries(frsSqFile), seed)
        {
        }

        /// <summary>
        /// Create a ForcedRandomSeries from the given sample sequence
        /// and a random generator that is used to generate a seed.
        /// The sequence is supposed to contain NxN points in random order.
        /// </summary>
        public ForcedRandomSeries(V2i[] series, IRandomUniform rndSeed)
            : this(series, new V2i(rndSeed.UniformInt(), rndSeed.UniformInt()))
        {
        }

        /// <summary>
        /// Create a ForcedRandomSeries from the given sequence data file
        /// and a random generator that is used to generate a seed.
        /// </summary>
        public ForcedRandomSeries(string frsSqFile, IRandomUniform rndSeed)
            : this(ReadSeries(frsSqFile), rndSeed)
        {
        }

        public double UniformDouble(int seriesIndex)
        {
            var ind = m_index[seriesIndex]++;
            var rnd = m_series[ind++ % m_series.Length][seriesIndex];
            rnd += m_seed[seriesIndex];
            return (rnd * m_norm).Frac();
        }
    }
}

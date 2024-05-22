using System.IO;
using System.Runtime.InteropServices;
using System;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides an IRandomSeries view of a pre-computed 2d random sequence.
    /// </summary>
    public class ForcedRandomSeries : IRandomSeries
    {
        V2i m_index = V2i.OO;
        V2i[] m_series;
        int m_matrixSize; 
        double m_norm;
        IRandomUniform m_rnd;
        V2i m_seed;
        bool m_jitter;

        /// <summary>
        /// Reads a file that contains a raw V2i binary array as V2i[].
        /// The binary data is supposed to contain NxN points as (int, int) in random order.
        /// </summary>
        public static V2i[] ReadSeries(string frsSqFile)
        {
            var bytes = File.ReadAllBytes(frsSqFile);
            var matrixSize = (int)(bytes.Length / 8).Sqrt();

            // V2i[] encoded in byte array must be NxN and a multiple of 8.
            if (matrixSize * matrixSize * 8 != bytes.Length)
                throw new InvalidDataException("Forced Random series data has invalid length.");

            var dst = new V2i[bytes.Length / 8];
            bytes.AsSpan().CopyTo(MemoryMarshal.AsBytes(dst.AsSpan()));
            return dst;
        }

        /// <summary>
        /// Create a ForcedRandomSeries with the given sample sequence and the seed as offset.
        /// The sequence is supposed to contain NxN points in random order.
        /// </summary>
        public ForcedRandomSeries(V2i[] series, int matrixSize, IRandomUniform rnd, bool jitter = true)
        {
            m_series = series;
            m_matrixSize = matrixSize;
            m_norm = 1.0 / m_matrixSize; // [0, 1) 
            m_rnd = rnd;
            m_jitter = jitter;
            // random offset of sample pattern
            m_seed = new V2i(rnd.UniformInt(m_matrixSize), rnd.UniformInt(m_matrixSize)); 
        }

        /// <summary>
        /// Create a ForcedRandomSeries with the given sample sequence and the seed as offset.
        /// The sequence is supposed to contain NxN points in random order.
        /// </summary>
        public ForcedRandomSeries(V2i[] series, IRandomUniform rnd, bool jitter = true)
            : this(series, (int)series.Length.Sqrt(), rnd, jitter)
        {
        }

        /// <summary>
        /// Create a ForcedRandomSeries from the given sequence data file
        /// and a random generator that is used to generate a seed.
        /// </summary>
        public ForcedRandomSeries(string frsSqFile, IRandomUniform rndSeed, bool jitter = true)
            : this(ReadSeries(frsSqFile), rndSeed, jitter)
        {
        }

        public double UniformDouble(int seriesIndex)
        {
            var ind = m_index[seriesIndex]++;
            var rnd = m_series[ind++ % m_series.Length][seriesIndex];
            rnd += m_seed[seriesIndex]; // shift complete pattern by seed
            return ((rnd + (m_jitter ? m_rnd.UniformDouble() : 0.0)) * m_norm).Frac(); // optionally add sub-pixel jittering
        }
    }
}

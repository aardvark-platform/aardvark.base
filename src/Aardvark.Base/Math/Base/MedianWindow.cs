using Aardvark.Base.Sorting;
using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents a moving median window of a sequence.
    /// It builds the median of the last N inserted values.
    /// </summary>
    public class MedianWindow
    {
        double m_median = 0;
        int m_write = -1;
        int m_count = 0; // for case when buffer is not fully filled
        readonly double[] m_buffer;
        readonly int[] m_indices;

        public MedianWindow(int count)
        {
            m_buffer = new double[count];
            m_indices = new int[count].SetByIndex(i => i);
        }

        public double Insert(double value)
        {
            if (m_count < m_buffer.Length)
                m_count++;

            if (m_write > m_buffer.Length - 2)
                m_write = 0;
            else
                m_write++;

            m_buffer[m_write] = value;

            // NOTE: indices are still sorted from last insert
            m_indices.PermutationQuickSortAscending(m_buffer, 0, m_count);
            m_median = m_buffer[m_indices[m_count >> 1]];

            return m_median;
        }

        /// <summary>
        /// Returns the history buffer. Contains zeroes if less elements than the window size have been inserted.
        /// </summary>
        public IReadOnlyList<double> History { get { return m_buffer; } }

        public double Value { get { return m_median; } }

        /// <summary>
        /// Returns the last inserted valued.
        /// In case no value has been inserted yet, 0 is returned.
        /// </summary>
        public double Last
        {
            get
            {
                // in the case that last is queried before any insert return 0
                if (m_write >= m_count)
                    return 0;

                return m_buffer[m_write];
            }
        }

        /// <summary>
        /// Resets the median window.
        /// </summary>
        public void Reset()
        {
            m_median = 0;
            m_write = -1;
            m_count = 0;
            m_indices.SetByIndex(i => i);
        }
    }
}

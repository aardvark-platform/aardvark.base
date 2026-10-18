using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents a moving median window of a sequence.
    /// It builds the median of the last N inserted values, ordering NaNs below
    /// numeric values and equal to other NaNs.
    /// </summary>
    public class MedianWindow
    {
        private double m_median;
        private int m_write = -1;
        private int m_count;
        private readonly double[] m_buffer;
        private readonly int[] m_indices;

        public MedianWindow(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            m_buffer = new double[count];
            m_indices = new int[count];
        }

        /// <summary>
        /// Inserts a value and returns the median of the active window. For an
        /// even number of active values, the upper of the two middle values is returned.
        /// NaNs sort below numeric values and compare equal to other NaNs.
        /// </summary>
        public double Insert(double value)
        {
            int slot = m_write + 1;
            if (slot == m_buffer.Length)
                slot = 0;
            m_write = slot;

            int sortedIndex;
            if (m_count < m_buffer.Length)
            {
                sortedIndex = m_count;
                m_count++;
            }
            else
            {
                sortedIndex = 0;
                while (m_indices[sortedIndex] != slot)
                    sortedIndex++;
            }

            m_buffer[slot] = value;

            if (sortedIndex > 0 && value.CompareTo(m_buffer[m_indices[sortedIndex - 1]]) < 0)
            {
                do
                {
                    m_indices[sortedIndex] = m_indices[sortedIndex - 1];
                    sortedIndex--;
                }
                while (sortedIndex > 0 && value.CompareTo(m_buffer[m_indices[sortedIndex - 1]]) < 0);
            }
            else
            {
                int last = m_count - 1;
                while (sortedIndex < last && value.CompareTo(m_buffer[m_indices[sortedIndex + 1]]) > 0)
                {
                    m_indices[sortedIndex] = m_indices[sortedIndex + 1];
                    sortedIndex++;
                }
            }

            m_indices[sortedIndex] = slot;
            m_median = m_buffer[m_indices[m_count >> 1]];
            return m_median;
        }

        /// <summary>
        /// Returns the fixed-size ring history buffer. Unwritten slots are initially
        /// zero and retain their previous contents across <see cref="Reset"/>.
        /// </summary>
        public IReadOnlyList<double> History { get { return m_buffer; } }

        /// <summary>
        /// Returns the current median, or 0.0 when the window is empty. For an
        /// even number of active values, this is the upper of the two middle values.
        /// NaNs sort below numeric values and compare equal to other NaNs.
        /// </summary>
        public double Value { get { return m_median; } }

        /// <summary>
        /// Returns the last inserted value, or 0.0 before the first insertion
        /// and after <see cref="Reset"/>.
        /// </summary>
        public double Last { get { return m_count == 0 ? 0.0 : m_buffer[m_write]; } }

        /// <summary>
        /// Resets the active median window in constant time without clearing
        /// the <see cref="History"/> buffer.
        /// </summary>
        public void Reset()
        {
            m_median = 0.0;
            m_write = -1;
            m_count = 0;
        }
    }
}

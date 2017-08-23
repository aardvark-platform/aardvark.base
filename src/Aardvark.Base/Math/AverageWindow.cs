namespace Aardvark.Base
{
    /// <summary>
    /// Represents a moving average window of a sequence.
    /// It builds the average of the last N inserted values.
    /// </summary>
    public class AverageWindow
    {
        double m_sum = 0;
        int m_count = 0; // if buffer is not fully filled
        int m_write = 0;
        double[] m_buffer;
        double m_avg = 0;

        /// <summary>
        /// Creates a new AverageWindow for the given count. 
        /// </summary>
        public AverageWindow(int count)
        {
            m_buffer = new double[count];
        }

        /// <summary>
        /// Insert a new value to the sequence and returns the average of the last N values.
        /// </summary>
        public double Insert(double value)
        {
            m_sum += value; // ISSUE: using this sum might bias the actual sum of m_buffer since its always accumulated by +/-
            m_sum -= m_buffer[m_write];
            m_buffer[m_write] = value;

            if (m_count < m_buffer.Length)
                m_count++;

            if (m_write > m_buffer.Length - 2)
                m_write = 0;
            else
                m_write++;

            m_avg = m_sum / m_count;
            return m_avg;
        }

        /// <summary>
        /// Returns the average of the last N inserted values.
        /// </summary>
        public double Value { get { return m_avg; } }
    }
}

/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using Aardvark.Base.Sorting;
using System.Collections.Generic;

namespace Aardvark.Base;

/// <summary>
/// Represents a moving median window of a sequence.
/// It builds the median of the last N inserted values.
/// </summary>
public class MedianWindow(int count)
{
    double m_median = 0;
    int m_write = -1;
    int m_count = 0; // for case when buffer is not fully filled
    readonly double[] m_buffer = new double[count];
    readonly int[] m_indices = new int[count].SetByIndex(i => i);

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

using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// A SubRange is an IList that servers as a window into other ILists.
    /// </summary>
    public class SubRange<T> : IList<T>
    {
        private readonly IList<T> m_base;
        private readonly int m_start;
        private readonly int m_count;
        private readonly int m_stop;

        #region Constructor

        public SubRange(IList<T> of, int index, int count)
        {
            if (of == null) throw new ArgumentNullException(nameof(of));
            if (index < 0 || index > of.Count) throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0 || count > of.Count - index) throw new ArgumentOutOfRangeException(nameof(count));

            m_base = of;
            m_start = index;
            m_count = count;
            m_stop = m_start + m_count;
        }

        #endregion

        #region IList<T> Members

        public int IndexOf(T item)
        {
            for (int i = m_start; i < m_stop; i++)
            {
                if (EqualityComparer<T>.Default.Equals(m_base[i], item)) return i - m_start;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }

        public void RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= m_count)
                    throw new IndexOutOfRangeException();
                return m_base[m_start + index];
            }
            set
            {
                if (index < 0 || index >= m_count)
                    throw new IndexOutOfRangeException();
                m_base[m_start + index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Copies this range into the destination array. Overlap with an array source,
        /// including through ordinary nested SubRange views, is handled as if the source
        /// elements were read before any destination writes. Other IList sources retain
        /// forward indexer reads; aliasing hidden by custom list implementations is not detected.
        /// Reference elements are copied shallowly.
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (m_count > array.Length - arrayIndex)
                throw new ArgumentException("The destination array has insufficient capacity.", nameof(array));

            if (m_count == 0) return;

            var source = m_base;
            var sourceIndex = m_start;
            // A derived SubRange can reimplement IList<T>; do not bypass its indexer.
            while (source.GetType() == typeof(SubRange<T>))
            {
                var range = (SubRange<T>)source;
                sourceIndex += range.m_start;
                source = range.m_base;
            }

            if (source is Array sourceArray &&
                (array.GetType() == typeof(T[]) || array.GetType() == sourceArray.GetType()))
            {
                Array.Copy(sourceArray, sourceIndex, array, arrayIndex, m_count);
                return;
            }

            // Preserve element-wise compatibility checks for narrower covariant destinations.
            for (int i = 0; i < m_count; i++)
            {
                array[arrayIndex + i] = m_base[m_start + i];
            }
        }

        public int Count
        {
            get { return m_count; }
        }

        public bool IsReadOnly
        {
            get { return m_base.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = m_start; i < m_stop; i++) yield return m_base[i];
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = m_start; i < m_stop; i++) yield return m_base[i];
        }

        #endregion
    }
}

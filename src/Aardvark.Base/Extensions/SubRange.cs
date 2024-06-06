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
                if (m_base[i].Equals(item)) return i - m_start;
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
                if (index < 0 || index > m_count)
                    throw new IndexOutOfRangeException();
                return m_base[m_start + index];
            }
            set
            {
                if (index < 0 || index > m_count)
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

        public void CopyTo(T[] array, int arrayIndex)
        {
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

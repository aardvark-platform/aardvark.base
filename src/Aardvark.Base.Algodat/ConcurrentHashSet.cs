using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace System.Collections.Concurrent
{
    /// <summary>
    /// Represents a thread-safe collection that can be accessed by multiple threads concurrently.
    /// </summary>
    public class ConcurrentHashSet<T> : IEnumerable<T>, ICollection<T>
    {
		//int does not waste too much memory and might be used for reference-counting 
		//or similar features. TODO: investigate if this is faster using reference types.
        private ConcurrentDictionary<T, int> m_entries;

        #region Constructors

        public ConcurrentHashSet()
        {
            m_entries = new ConcurrentDictionary<T, int>();
        }

        public ConcurrentHashSet(int concurrencyLevel, int capacity)
        {
            m_entries = new ConcurrentDictionary<T, int>(concurrencyLevel, capacity);
        }

        public ConcurrentHashSet(IEqualityComparer<T> comparer)
        {
            m_entries = new ConcurrentDictionary<T, int>(comparer);
        }

        public ConcurrentHashSet(IEnumerable<T> collection)
        {
            m_entries = new ConcurrentDictionary<T, int>(collection.Select(e => new KeyValuePair<T, int>(e, 1)));
        }

        public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            m_entries = new ConcurrentDictionary<T, int>(collection.Select(e => new KeyValuePair<T, int>(e, 1)), comparer);
        }

        #endregion

        #region Properties

        public int Count
        {
            get { return m_entries.Count; }
        }

        #endregion

        #region Methods

        public void Clear()
        {
            m_entries.Clear();
        }

        public bool Add(T item)
        {
            return m_entries.TryAdd(item, 1);
        }

        public bool Remove(T item)
        {
            int dummy;
            return m_entries.TryRemove(item, out dummy);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (var e in other)
                Add(e);
        }

        #endregion

        #region IEnumerable Members

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region ICollection Members

        public bool Contains(T item)
        {
            return m_entries.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_entries.Keys.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        #endregion

        public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private IEnumerator<KeyValuePair<T, int>> m_enumerator;

            internal Enumerator(ConcurrentHashSet<T> set)
            {
                m_enumerator = set.m_entries.GetEnumerator();
            }

            public T Current
            {
                get { return m_enumerator.Current.Key; }
            }

            public void Dispose()
            {
                m_enumerator.Dispose();
            }

            object IEnumerator.Current
            {
                get { return ((IEnumerator)m_enumerator).Current; }
            }

            public bool MoveNext()
            {
                return m_enumerator.MoveNext();
            }

            public void Reset()
            {
                m_enumerator.Reset();
            }
        }



        
    }
}

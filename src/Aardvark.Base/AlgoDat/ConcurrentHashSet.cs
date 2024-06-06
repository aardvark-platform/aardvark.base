using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Concurrent
{
    /// <summary>
    /// Represents a thread-safe collection that can be accessed by multiple threads concurrently.
    /// </summary>
    public class ConcurrentHashSet<T> : IEnumerable<T>, ICollection<T>
    {
		//int does not waste too much memory and might be used for reference-counting 
		//or similar features. TODO: investigate if this is faster using reference types.
        private readonly ConcurrentDictionary<T, int> m_entries;

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

        public int Count => m_entries.Count;

        #endregion

        #region Methods

        public void Clear() => m_entries.Clear();

        public bool Add(T item) => m_entries.TryAdd(item, 1);

        public bool Remove(T item) => m_entries.TryRemove(item, out int dummy);

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (var e in other)
                Add(e);
        }

        #endregion

        #region IEnumerable Members

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        #endregion

        #region ICollection Members

        public bool Contains(T item) => m_entries.ContainsKey(item);

        public void CopyTo(T[] array, int arrayIndex) => m_entries.Keys.CopyTo(array, arrayIndex);

        public bool IsReadOnly => false;

        void ICollection<T>.Add(T item) => Add(item);

        #endregion

        public readonly struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private readonly IEnumerator<KeyValuePair<T, int>> m_enumerator;

            internal Enumerator(ConcurrentHashSet<T> set)
            {
                m_enumerator = set.m_entries.GetEnumerator();
            }

            public T Current => m_enumerator.Current.Key;

            public void Dispose() => m_enumerator.Dispose();

            object IEnumerator.Current => ((IEnumerator)m_enumerator).Current;

            public bool MoveNext() => m_enumerator.MoveNext();

            public void Reset() => m_enumerator.Reset();
        }
    }
}

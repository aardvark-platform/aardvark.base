using System;
using System.IO;
using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// A least-recently-used cache, with specifyable capacity and per-item size-, read-, and delete
    /// function. The indexer is used to access items. Capacity can be changed on the fly. All
    /// operations are synchronized for use in multi-threaded applications.
    /// </summary>
    public class LruCache<TKey, TValue>
    {
        private class Entry
        {
            public long Time;
            public long Size;
            public int Index;
            public TKey Key;
            public TValue Value;
            public Action DeleteAct;
        }

        private object m_lock;
        private Dict<TKey, Entry> m_cache;
        private List<Entry> m_heap;
        private Func<TKey, long> m_sizeFun;
        private Func<TKey,TValue> m_readFun;
        private Action<TKey,TValue> m_deleteAct;
        private long m_capacity;
        private long m_time;
        private long m_size;

        /// <summary>
        /// Creates an LruCache with the specified capacity, per-item (Key) size function,
        /// per-item (Key) read function, and per-item (key/value) delete action.
        /// The indexer is used to access items. Capacity can be changed on the fly. All
        /// operations are synchronized for use in multi-threaded applications.
        /// </summary>
        public LruCache(
            long capacity,
            Func<TKey, long> sizeFun,
            Func<TKey,TValue> readFun,
            Action<TKey, TValue> deleteAct = null
        )
        {
            m_lock = new object();
            m_cache = new Dict<TKey, Entry>();
            m_heap = new List<Entry>();
            m_sizeFun = sizeFun;
            m_readFun = readFun;
            m_deleteAct = deleteAct;
            m_capacity = capacity;
            m_time = 0;
            m_size = 0;
        }

        public LruCache(
            long capacity
        )
        {
            m_lock = new object();
            m_cache = new Dict<TKey, Entry>();
            m_heap = new List<Entry>();
            m_sizeFun = null;
            m_readFun = null;
            m_deleteAct = null;
            m_capacity = capacity;
            m_time = 0;
            m_size = 0;
        }




        public long Capacity
        {
            get
            {
                return m_capacity;
            }
            set
            {
                lock (m_lock)
                {
                    m_capacity = value;
                    Shrink(m_size);
                }
            }
        }

        private void Shrink(long size)
        {
            while (size > m_capacity)
            {
                Entry entry;
                var removeKey = Dequeue(m_heap).Key;
                if (m_cache.TryRemove(removeKey, out entry))
                {
                    if (m_deleteAct != null)
                        m_deleteAct(removeKey, entry.Value);
                    if (entry.DeleteAct != null)
                        entry.DeleteAct();
                    size -= entry.Size;
                }
                else
                    Report.Warn("tried to remove an item that is not in the cache");  
                    // this should never ever happen!
            }
            m_size = size;
        }

        /// <summary>
        /// Accessing items in the cache. If an item is not encountred in the cache,
        /// it is read using the read function that was specified on cache creation.
        /// </summary>
        public TValue this [TKey key]
        {
            get
            {
                Entry entry;
                lock (m_lock)
                {
                    if (m_cache.TryGetValue(key, out entry))
                    {
                        entry.Time = ++m_time;
                        Sink(m_heap, entry.Index);
                    }
                    else
                    {
                        var size = m_sizeFun(key);
                        Shrink(m_size + size);
                        entry = new Entry
                        {
                            Time = ++m_time, Size = size, Key = key, Value = m_readFun(key)
                        };
                        m_cache[key] = entry;
                        Enqueue(m_heap, entry);
                    }
                }
                return entry.Value;
            }
        }

        public TValue GetOrAdd(TKey key, long size, Func<TValue> valueFun, Action deleteAct = null)
        {
            Entry entry;
            lock (m_lock)
            {
                if (m_cache.TryGetValue(key, out entry))
                {
                    entry.Time = ++m_time;
                    Sink(m_heap, entry.Index);
                }
                else
                {
                    Shrink(m_size + size);
                    entry = new Entry
                    {
                        Time = ++m_time,
                        Size = size,
                        Key = key,
                        Value = valueFun(),
                        DeleteAct = deleteAct,
                    };
                    m_cache[key] = entry;
                    Enqueue(m_heap, entry);
                }
                return entry.Value;
            }
        }

        /// <summary>
        /// Remove the entry with the supplied key from the hash.
        /// Returns true on success and puts the value of the
        /// entry into the out parameter.
        /// </summary>
        public bool TryRemove(TKey key, out TValue value)
        {
            lock (m_lock)
            {
                Entry entry;
                if (m_cache.TryRemove(key, out entry))
                {
                    m_size -= entry.Size;
                    RemoveAt(m_heap, entry.Index);
                    if (m_deleteAct != null)
                        m_deleteAct(key, entry.Value);
                    value = entry.Value;
                    return true;
                }
                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Remove the entry with the supplied key from the hash.
        /// Returns true on success.
        /// </summary>
        public bool Remove(TKey key)
        {
            lock (m_lock)
            {
                Entry entry;
                if (m_cache.TryRemove(key, out entry))
                {
                    m_size -= entry.Size;
                    RemoveAt(m_heap, entry.Index);
                    if (m_deleteAct != null)
                        m_deleteAct(key, entry.Value);
                    if (entry.DeleteAct != null)
                        entry.DeleteAct();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Reomves an arbitrary element from the heap, and maintains the heap
        /// conditions.
        /// </summary>
        private static void RemoveAt(
            List<Entry> heap, int index)
        {
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return; }
            var element = heap[--count];
            heap.RemoveAt(count);
            if (index == count) return;

            int i = index;
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (element.Time > heap[i2].Time) break;
                heap[i] = heap[i2];
                heap[i].Index = i;
                i = i2;
            }
            if (i == index)
            {
                int i1 = 2 * i + 1;
                while (i1 < count) // at least one child
                {
                    int i2 = i1 + 1;
                    int ni = (i2 < count // two children?
                        && heap[i1].Time > heap[i2].Time)
                        ? i2 : i1; // smaller child
                    if (heap[ni].Time > element.Time) break;
                    heap[i] = heap[ni];
                    heap[i].Index = i;
                    i = ni; i1 = 2 * i + 1;
                }
            }
            heap[i] = element;
            heap[i].Index = i;
        }

        private static void Enqueue(
            List<Entry> heap, Entry entry)
        {
            int i = heap.Count;
            heap.Add(entry);
            entry.Index = i;
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (entry.Time > heap[i2].Time) break;
                heap[i] = heap[i2];
                heap[i].Index = i;
                i = i2;
            }
            heap[i] = entry;
            heap[i].Index = i;
        }

        /// <summary>
        /// Removes and returns the item at the top of the heap (i.e. the
        /// 0th position of the list).
        /// </summary>
        private static Entry Dequeue(
            List<Entry> heap)
        {
            var result = heap[0];
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return result; }
            var entry = heap[--count];
            heap.RemoveAt(count);
            int i = 0, i1 = 1;
            while (i1 < count) // at least one child
            {
                int i2 = i1 + 1;
                int ni = (i2 < count // two children?
                    && heap[i1].Time > heap[i2].Time)
                    ? i2 : i1; // smaller child
                if (heap[ni].Time > entry.Time) break;
                heap[i] = heap[ni];
                heap[i].Index = i; // track index
                i = ni; i1 = 2 * i + 1;
            }
            heap[i] = entry;
            heap[i].Index = i; // track index
            return result;
        }

        /// <summary>
        /// Sinks an item.
        /// </summary>
        private static void Sink(
            List<Entry> heap, int i)
        {
            var count = heap.Count;
            var entry = heap[i];
            int i1 = 2 * i + 1;
            while (i1 < count) // at least one child
            {
                int i2 = i1 + 1;
                int ni = (i2 < count // two children?
                    && heap[i1].Time > heap[i2].Time)
                    ? i2 : i1; // smaller child
                if (heap[ni].Time > entry.Time) break;
                heap[i] = heap[ni];
                heap[i].Index = i; // track index
                i = ni; i1 = 2 * i + 1;
            }
            heap[i] = entry;
            heap[i].Index = i; // track index
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region Special Dicts

    public class SingleEntryDict<TKey, TValue> : IDict<TKey, TValue>
    {
        TKey m_key;
        TValue m_value;

        #region Constructors

        public SingleEntryDict(TKey key, TValue value)
        {
            m_key = key;
            m_value = value;
        }

        #endregion

        #region IDict<TKey, TValue>

        public IEnumerable<TKey> Keys
        {
            get { yield return m_key; }
        }

        public IEnumerable<TValue> Values
        {
            get { yield return m_value; }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get { yield return new KeyValuePair<TKey, TValue>(m_key, m_value); }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key.Equals(m_key)) return m_value;
                throw new KeyNotFoundException();
            }
            set
            {
                if (key.Equals(m_key)) m_value = value;
                throw new ArgumentException();
            }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            return key.Equals(m_key);
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// An IDict where all contained keys return the same specified value.
    /// </summary>
    public class SingleValueDict<TKey, TValue> : IDict<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private IDictSet<TKey> m_keys;
        private TValue m_value;

        #region Constructors

        public SingleValueDict(TValue value)
        {
            m_value = value;
        }

        public SingleValueDict(IDictSet<TKey> keySet, TValue value)
        {
            m_keys = keySet;
            m_value = value;
        }

        #endregion

        #region IDict<TKey,TValue> Members

        public IEnumerable<TKey> Keys
        {
            get { return m_keys.Items; }
        }

        public IEnumerable<TValue> Values
        {
            get { yield return m_value; }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get { return m_keys.Items.Select(k => new KeyValuePair<TKey, TValue>(k, m_value)); }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (m_keys.Contains(key)) return m_value;
                throw new KeyNotFoundException();
            }
            set
            {
                if (value.Equals(m_value)) m_keys.Add(key);
                throw new ArgumentException();
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (value.Equals(m_value)) m_keys.Add(key);
            throw new ArgumentException();
        }

        public bool ContainsKey(TKey key)
        {
            return m_keys.Contains(key);
        }

        public bool Remove(TKey key)
        {
            return m_keys.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_keys.Contains(key))
            {
                value = m_value; return true;
            }
            else
            {
                value = default(TValue); return false;
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// An IDict where all possible keys return the same specified value.
    /// </summary>
    public class UniversalDict<TKey, TValue> : IDict<TKey, TValue>
    {
        private TValue m_value;

        #region Constructor

        public UniversalDict(TValue value)
        {
            m_value = value;
        }

        #endregion

        #region IDict<TKey,TValue> Members

        public IEnumerable<TKey> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<TValue> Values
        {
            get { yield return m_value; }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get { throw new NotImplementedException(); }
        }

        public TValue this[TKey key]
        {
            get
            {
                return m_value;
            }
            set
            {
                if (!value.Equals(m_value))
                    throw new ArgumentException();
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (!value.Equals(m_value))
                throw new ArgumentException();
        }

        public bool ContainsKey(TKey key)
        {
            return true;
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = m_value;
            return true;
        }

        #endregion
    }

    /// <summary>
    /// A union of IDicts with left priority.
    /// </summary>
    public class UnionDict<TKey, TValue> : IDict<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        List<IDict<TKey, TValue>> m_dictList;

        #region Constructors

        public UnionDict()
        {
            m_dictList = new List<IDict<TKey, TValue>>();
        }

        public UnionDict(IEnumerable<IDict<TKey, TValue>> dicts)
        {
            m_dictList = dicts.ToList();
        }

        public UnionDict(params IDict<TKey, TValue>[] dictArray)
            : this((IEnumerable<IDict<TKey, TValue>>)dictArray)
        { }

        #endregion

        #region Properties

        public IEnumerable<IDict<TKey, TValue>> Dicts { get { return m_dictList; } }

        #endregion

        #region Manipulation Methods

        public void Add(IDict<TKey, TValue> dict)
        {
            m_dictList.Add(dict);
        }

        #endregion

        #region IDict<TKey,TValue> Members

        public IEnumerable<TKey> Keys
        {
            get { return (from d in m_dictList from k in d.Keys select k)
                            .GroupBy(k => k).Select(g => g.Key); }
        }

        public IEnumerable<TValue> Values
        {
            get { return (from d in m_dictList from k in d.Keys select k)
                            .GroupBy(k => k).Select(g => this[g.Key]); }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get { return (from d in m_dictList from k in d.Keys select k)
                            .GroupBy(k => k).Select(g => new KeyValuePair<TKey, TValue>(g.Key, this[g.Key])); }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                foreach (var d in m_dictList)
                    if (d.TryGetValue(key, out value))
                        return value;
                throw new KeyNotFoundException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            foreach (var d in m_dictList)
                if (d.ContainsKey(key))
                    return true;
            return false;
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var d in m_dictList)
                if (d.TryGetValue(key, out value))
                    return true;
            value = default(TValue);
            return false;
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// A Dict that overrides a single value of a supplied
    /// base Dict.
    /// </summary>
    public class SingleDeltaDict<TKey, TValue> : IDict<TKey, TValue>, IDictDepth, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        internal IDict<TKey, TValue> m_baseDict;
        internal TKey m_key;
        internal TValue m_value;
        internal int m_depth;

        #region Constructors

        public SingleDeltaDict(IDict<TKey, TValue> baseDict, TKey key, TValue value)
        {
            m_baseDict = baseDict;
            m_key = key;
            m_value = value;
            var dictDepth = baseDict as IDictDepth;
            m_depth = dictDepth != null ? dictDepth.Depth + 1 : 1;
        }

        #endregion

        #region IDict<TKey,TValue> Members

        public IEnumerable<TKey> Keys
        {
            get
            {
                yield return m_key;
                foreach (var k in m_baseDict.Keys)
                    if (!k.Equals(m_key))
                        yield return k;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                yield return m_value;
                foreach (var kvp in m_baseDict.KeyValuePairs)
                    if (!kvp.Key.Equals(m_key))
                        yield return kvp.Value;
            }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get
            {
                yield return new KeyValuePair<TKey, TValue>(m_key, m_value);
                foreach (var kvp in m_baseDict.KeyValuePairs)
                    if (!kvp.Key.Equals(m_key))
                        yield return kvp;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key.Equals(m_key)) return m_value;
                return m_baseDict[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            if (key.Equals(m_key)) return true;
            return m_baseDict.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key.Equals(m_key))
            {
                value = m_value;
                return true;
            }
            return m_baseDict.TryGetValue(key, out value);
        }

        #endregion

        #region IDictDepth

        public int Depth { get { return m_depth; } }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

    }

    /// <summary>
    /// A Dict that overrides all values contained in a delta
    /// Dict with respect to a supplied base Dict.
    /// </summary>
    public class DeltaDict<TKey, TValue> : IDict<TKey, TValue>, IDictDepth, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        IDict<TKey, TValue> m_baseDict;
        Dict<TKey, TValue> m_deltaDict;
        int m_depth;

        #region Constructors

        public DeltaDict(IDict<TKey, TValue> baseDict, Dict<TKey, TValue> deltaDict)
        {
            m_baseDict = baseDict;
            m_deltaDict = deltaDict;
            var dictDepth = baseDict as IDictDepth;
            m_depth = dictDepth != null ? dictDepth.Depth + 1 : 1;
        }

        #endregion

        #region IDict<TKey,TValue> Members

        public IEnumerable<TKey> Keys
        {
            get
            {
                return m_deltaDict.Keys.Concat(m_baseDict.Keys.Where(k => !m_deltaDict.ContainsKey(k)));
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                return m_deltaDict.Values.Concat(m_baseDict.KeyValuePairs.Where(kvp => !m_deltaDict.ContainsKey(kvp.Key))
                                                                         .Select(kvp => kvp.Value));
            }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get
            {
                return m_deltaDict.KeyValuePairs.Concat(m_baseDict.KeyValuePairs.Where(kvp => !m_deltaDict.ContainsKey(kvp.Key)));
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (m_deltaDict.TryGetValue(key, out value)) return value;
                return m_baseDict[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            if (m_deltaDict.ContainsKey(key)) return true;
            return m_baseDict.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_deltaDict.TryGetValue(key, out value))
                return true;
            return m_baseDict.TryGetValue(key, out value);
        }

        #endregion

        #region IDictDepth

        public int Depth { get { return m_depth; } }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

    }

    /// <summary>
    /// An IDict with Symbol keys where all contained keys return the same
    /// specified value.
    /// </summary>
    public class SingleValueSymbolDict<TValue> : IDict<Symbol, TValue>, IEnumerable<KeyValuePair<Symbol, TValue>>
    {
        private IDictSet<Symbol> m_keys;
        private TValue m_value;

        #region Constructors

        public SingleValueSymbolDict(IDictSet<Symbol> keySet, TValue value)
        {
            m_keys = keySet;
            m_value = value;
        }

        public SingleValueSymbolDict(TValue value)
        {
            m_value = value;
        }

        #endregion

        #region IDict<Symbol,TValue> Members

        public IEnumerable<Symbol> Keys
        {
            get { return m_keys.Items; }
        }

        public IEnumerable<TValue> Values
        {
            get { yield return m_value; }
        }

        public IEnumerable<KeyValuePair<Symbol, TValue>> KeyValuePairs
        {
            get { return m_keys.Items.Select(k => new KeyValuePair<Symbol, TValue>(k, m_value)); }
        }

        public TValue this[Symbol key]
        {
            get
            {
                if (m_keys.Contains(key)) return m_value;
                throw new KeyNotFoundException();
            }
            set
            {
                if (value.Equals(m_value)) m_keys.Add(key);
                throw new ArgumentException();
            }
        }

        public void Add(Symbol key, TValue value)
        {
            if (value.Equals(m_value)) m_keys.Add(key);
            throw new ArgumentException();
        }

        public bool ContainsKey(Symbol key)
        {
            return m_keys.Contains(key);
        }

        public bool Remove(Symbol key)
        {
            return m_keys.Remove(key);
        }

        public bool TryGetValue(Symbol key, out TValue value)
        {
            if (m_keys.Contains(key))
            {
                value = m_value; return true;
            }
            else
            {
                value = default(TValue); return false;
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<Symbol,TValue>> Members

        public IEnumerator<KeyValuePair<Symbol, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// An IDict with Symbol keys where all possible keys return the same
    /// specified value.
    /// </summary>
    public class UniversalSymbolDict<TValue> : IDict<Symbol, TValue>
    {
        private TValue m_value;

        #region Constructor

        public UniversalSymbolDict(TValue value)
        {
            m_value = value;
        }

        #endregion

        #region IDict<Symbol,TValue> Members

        public IEnumerable<Symbol> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<TValue> Values
        {
            get { yield return m_value; }
        }

        public IEnumerable<KeyValuePair<Symbol, TValue>> KeyValuePairs
        {
            get { throw new NotImplementedException(); }
        }

        public TValue this[Symbol key]
        {
            get
            {
                return m_value;
            }
            set
            {
                if (!value.Equals(m_value))
                    throw new ArgumentException();
            }
        }

        public void Add(Symbol key, TValue value)
        {
            if (!value.Equals(m_value))
                throw new ArgumentException();
        }

        public bool ContainsKey(Symbol key)
        {
            return true;
        }

        public bool Remove(Symbol key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(Symbol key, out TValue value)
        {
            value = m_value;
            return true;
        }

        #endregion
    }

    /// <summary>
    /// A union of IDicts with Symbol keys with left priority.
    /// </summary>
    public class UnionSymbolDict<TValue> : IDict<Symbol, TValue>, IEnumerable<KeyValuePair<Symbol, TValue>>
    {
        List<IDict<Symbol, TValue>> m_dictList;

        #region Constructors

        public UnionSymbolDict()
        {
            m_dictList = new List<IDict<Symbol, TValue>>();
        }

        public UnionSymbolDict(IEnumerable<IDict<Symbol, TValue>> dicts)
        {
            m_dictList = dicts.ToList();
        }

        public UnionSymbolDict(params IDict<Symbol, TValue>[] dictArray)
            : this((IEnumerable<IDict<Symbol, TValue>>)dictArray)
        { }

        #endregion

        #region Properties

        public IEnumerable<IDict<Symbol, TValue>> Dicts { get { return m_dictList; } }

        #endregion

        #region Manipulation Methods

        public void Add(IDict<Symbol, TValue> dict)
        {
            m_dictList.Add(dict);
        }

        #endregion

        #region IDict<Symbol,TValue> Members

        public IEnumerable<Symbol> Keys
        {
            get
            {
                return (from d in m_dictList from k in d.Keys select k)
                          .GroupBy(k => k).Select(g => g.Key);
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                return (from d in m_dictList from k in d.Keys select k)
                          .GroupBy(k => k).Select(g => this[g.Key]);
            }
        }

        public IEnumerable<KeyValuePair<Symbol, TValue>> KeyValuePairs
        {
            get
            {
                return (from d in m_dictList from k in d.Keys select k)
                          .GroupBy(k => k).Select(g => new KeyValuePair<Symbol, TValue>(g.Key, this[g.Key]));
            }
        }
        public TValue this[Symbol key]
        {
            get
            {
                TValue value;
                foreach (var d in m_dictList)
                    if (d.TryGetValue(key, out value))
                        return value;
                throw new KeyNotFoundException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(Symbol key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(Symbol key)
        {
            foreach (var d in m_dictList)
                if (d.ContainsKey(key))
                    return true;
            return false;
        }

        public bool Remove(Symbol key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(Symbol key, out TValue value)
        {
            foreach (var d in m_dictList)
                if (d.TryGetValue(key, out value))
                    return true;
            value = default(TValue);
            return false;
        }

        #endregion

        #region IEnumerable<KeyValuePair<Symbol,TValue>> Members

        public IEnumerator<KeyValuePair<Symbol, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// A SymbolDict that overrides a single value of a supplied
    /// base SymbolDict.
    /// </summary>
    public class SingleDeltaSymbolDict<TValue> : IDict<Symbol, TValue>, IEnumerable<KeyValuePair<Symbol, TValue>>
    {
        internal IDict<Symbol, TValue> m_baseDict;
        internal Symbol m_key;
        internal TValue m_value;

        #region Constructors

        public SingleDeltaSymbolDict(IDict<Symbol, TValue> baseDict, Symbol key, TValue value)
        {
            m_baseDict = baseDict;
            m_key = key;
            m_value = value;
        }

        #endregion

        #region IDict<TKey,TValue> Members

        public IEnumerable<Symbol> Keys
        {
            get
            {
                yield return m_key;
                foreach (var k in m_baseDict.Keys)
                    if (k != m_key)
                        yield return k;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                yield return m_value;
                foreach (var kvp in m_baseDict.KeyValuePairs)
                    if (kvp.Key != m_key)
                        yield return kvp.Value;
            }
        }

        public IEnumerable<KeyValuePair<Symbol, TValue>> KeyValuePairs
        {
            get
            {
                yield return new KeyValuePair<Symbol, TValue>(m_key, m_value);
                foreach (var kvp in m_baseDict.KeyValuePairs)
                    if (kvp.Key != m_key)
                        yield return kvp;
            }
        }

        public TValue this[Symbol key]
        {
            get
            {
                if (key == m_key) return m_value;
                return m_baseDict[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(Symbol key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(Symbol key)
        {
            if (key == m_key) return true;
            return m_baseDict.ContainsKey(key);
        }

        public bool Remove(Symbol key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(Symbol key, out TValue value)
        {
            if (key == m_key)
            {
                value = m_value;
                return true;
            }
            return m_baseDict.TryGetValue(key, out value);
        }

        #endregion

        #region IEnumerable<KeyValuePair<Symbol,TValue>> Members

        public IEnumerator<KeyValuePair<Symbol, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// A SymbolDict that overrides all values contained in a delta
    /// SymbolDict with respect to a supplied base SymbolDict.
    /// </summary>
    public class DeltaSymbolDict<TValue> : IDict<Symbol, TValue>, IEnumerable<KeyValuePair<Symbol, TValue>>
    {
        IDict<Symbol, TValue> m_baseDict;
        IDict<Symbol, TValue> m_deltaDict;

        #region Constructors

        public DeltaSymbolDict(IDict<Symbol, TValue> baseDict, IDict<Symbol, TValue> deltaDict)
        {
            m_baseDict = baseDict;
            m_deltaDict = deltaDict;
        }

        #endregion

        #region IDict<Symbol,TValue> Members

        public IEnumerable<Symbol> Keys
        {
            get
            {
                return m_deltaDict.Keys.Concat(m_baseDict.Keys.Where(k => !m_deltaDict.ContainsKey(k)));
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                return m_deltaDict.Values.Concat(m_baseDict.KeyValuePairs.Where(kvp => !m_deltaDict.ContainsKey(kvp.Key))
                                                                         .Select(kvp => kvp.Value));
            }
        }

        public IEnumerable<KeyValuePair<Symbol, TValue>> KeyValuePairs
        {
            get
            {
                return m_deltaDict.KeyValuePairs.Concat(m_baseDict.KeyValuePairs.Where(kvp => !m_deltaDict.ContainsKey(kvp.Key)));
            }
        }

        public TValue this[Symbol key]
        {
            get
            {
                TValue value;
                if (m_deltaDict.TryGetValue(key, out value)) return value;
                return m_baseDict[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(Symbol key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(Symbol key)
        {
            if (m_deltaDict.ContainsKey(key)) return true;
            return m_baseDict.ContainsKey(key);
        }

        public bool Remove(Symbol key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(Symbol key, out TValue value)
        {
            if (m_deltaDict.TryGetValue(key, out value))
                return true;
            return m_baseDict.TryGetValue(key, out value);
        }

        #endregion

        #region IEnumerable<KeyValuePair<Symbol,TValue>> Members

        public IEnumerator<KeyValuePair<Symbol, TValue>> GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return KeyValuePairs.GetEnumerator();
        }

        #endregion
    }

    public static class IDictExtensions
    {

        /// <summary>
        /// Returns an IDict with the supplied key/value pair added. Internally
        /// the supplied dict is not modified, but referenced, and a suitable
        /// DeltaDict is built that contains only the changes.
        /// </summary>
        public static IDict<TKey, TValue> WithAdded<TKey, TValue>(
                this IDict<TKey, TValue> dict,
                TKey key, TValue value)
        {
            var sd = dict as SingleDeltaDict<TKey, TValue>;
            if (sd != null && key.Equals(sd.m_key))
                return new SingleDeltaDict<TKey, TValue>(sd.m_baseDict, key, value);
            return new SingleDeltaDict<TKey, TValue>(dict, key, value);
        }

        /// <summary>
        /// Return an IDict with the supplied Dict added as deltas. Internally
        /// the supplied dicts are not modified, but referenced, and a suitable
        /// DeltaDict is built.
        /// </summary>
        public static IDict<TKey, TValue> WithAdded<TKey, TValue>(
                this IDict<TKey, TValue> dict,
                Dict<TKey, TValue> deltaDict)
        {
            return new DeltaDict<TKey, TValue>(dict, deltaDict);
        }

        /// <summary>
        /// Returns an IDict with the supplied key/value pair added. Internally
        /// the supplied dict is not modified, but referenced, and a suitable
        /// DeltaDict is built that contains only the changes.
        /// </summary>
        public static IDict<Symbol, TValue> WithAdded<TValue>(
                this IDict<Symbol, TValue> dict,
                Symbol key, TValue value)
        {
            var sd = dict as SingleDeltaSymbolDict<TValue>;
            if (sd != null && key == sd.m_key)
                return new SingleDeltaSymbolDict<TValue>(sd.m_baseDict, key, value);
            return new SingleDeltaSymbolDict<TValue>(dict, key, value);
        }

        /// <summary>
        /// Return an IDict with the supplied Dict added as deltas. Internally
        /// the supplied dicts are not modified, but referenced, and a suitable
        /// DeltaDict is built.
        /// </summary>
        public static IDict<Symbol, TValue> WithAdded<TKey, TValue>(
                this IDict<Symbol, TValue> dict,
                SymbolDict<TValue> deltaDict)
        {
            return new DeltaSymbolDict<TValue>(dict, deltaDict);
        }

        public static bool ContainsKey<TType, TValue>(
                this IDict<Symbol, TValue> dict, TypedSymbol<TType> key)
            where TType : TValue
        {
            return dict.ContainsKey(key.Symbol);
        }

        public static TType Get<TType, TValue>(
                this IDict<Symbol, TValue> dict, TypedSymbol<TType> key)
            where TType : TValue
        {
            return (TType)dict[key.Symbol];
        }

        public static TType Get<TType, TValue>(
                this IDict<Symbol, TValue> dict, TypedSymbol<TType> key, TType defaultValue)
            where TType : TValue
        {
            TValue value;
            if (dict.TryGetValue(key.Symbol, out value)) return (TType)value;
            return defaultValue;
        }

        public static TType GetOrDefault<TType, TValue>(
                this IDict<Symbol, TValue> dict, TypedSymbol<TType> key)
            where TType : TValue
        {
            TValue value;
            if (dict.TryGetValue(key.Symbol, out value)) return (TType)value;
            return default(TType);
        }

        public static bool TryGetValue<TType, TValue>(
                this IDict<Symbol, TValue> dict, TypedSymbol<TType> key, out TType typedValue)
            where TType : TValue
        {
            TValue value;
            if (dict.TryGetValue(key.Symbol, out value))
            {
                typedValue = (TType)value;
                return true;
            }
            typedValue = default(TType);
            return false;
        }
    }

    #endregion

    #region Dict Extensions

    public static class DictFun
    {
        public static T[] GetArray<T>(this SymbolDict<Array> dict, Symbol name)
        {
            Array array;
            if (dict.TryGetValue(name, out array)) return (T[])array;
            throw new ArgumentException(
                    String.Format("symbol \"{0}\" not found in dictionary", name));
        }

        public static T[] GetArray<T>(this SymbolDict<Array> dict, Symbol name, T[] defaultArray)
        {
            Array array;
            if (dict.TryGetValue(name, out array)) return (T[])array;
            return defaultArray;
        }

        public static SymbolDict<Tv> Copy<Tv>(
            this SymbolDict<Tv> self)
        {
            var r = new SymbolDict<Tv>(self.Count);
            foreach (var kvp in self)
                r[kvp.Key] = kvp.Value;
            return r;
        }

        public static SymbolDict<Tr> Copy<T, Tr>(
            this SymbolDict<T> self,
            Func<Symbol, T, Tr> fun)
        {
            var r = new SymbolDict<Tr>(self.Count);
            foreach (var kvp in self)
                r[kvp.Key] = fun(kvp.Key, kvp.Value);
            return r;
        }

        public static SymbolDict<Tr> Copy<T, Tr>(
            this SymbolDict<T> self,
            Func<KeyValuePair<Symbol, T>, KeyValuePair<Symbol, Tr>> fun)
        {
            var r = new SymbolDict<Tr>(self.Count);
            foreach (var kvp in self)
            {
                var nkvp = fun(kvp);
                r[nkvp.Key] = nkvp.Value;
            }
            return r;
        }

        public static SymbolDict<T1v> Copy<Tv, T1v>(
            this SymbolDict<Tv> self,
            SymbolDict<Func<Tv, T1v>> funMap,
            Func<Tv, T1v> defaultFun)
        {
            var r = new SymbolDict<T1v>(self.Count);
            foreach (var kvp in self)
            {
                Func<Tv, T1v> fun;
                if (funMap.TryGetValue(kvp.Key, out fun))
                    r[kvp.Key] = fun(kvp.Value);
                else if (defaultFun != null)
                    r[kvp.Key] = defaultFun(kvp.Value);
            }
            return r;
        }

        public static IEnumerable<KeyValuePair<Tk, Tv>> KeyValueParisWith<Tk, Tv>(this Dict<Tk, Tv> dict, Tk key)
            where Tk : IEquatable<Tk>
        {
            return dict.ValuesWithKey(key).Select(v => KeyValuePairs.Create(key, v));
        }

        public static void RemoveAll<Tk, Tv>(this Dict<Tk, Tv> dict, Tk key)
        {
            while (dict.Remove(key)) ;
        }

        public static void RemoveWithValue<Tk, Tv>(this Dict<Tk, Tv> dict, Tk key, Func<Tv, bool> remove)
            where Tk : IEquatable<Tk>
        {
            var values = dict.ValuesWithKey(key).ToArray();
            dict.RemoveAll(key);
            dict.AddRange(values.Where(v => !remove(v)).Select(v => new KeyValuePair<Tk, Tv>(key, v)));
        }

        public static IEnumerable<Tv> PopWithValue<Tk, Tv>(this Dict<Tk, Tv> dict, Tk key, Func<Tv, bool> remove)
            where Tk : IEquatable<Tk>
        {
            var values = dict.ValuesWithKey(key).ToArray();
            dict.RemoveAll(key);
            foreach (var v in values)
            {
                if (remove(v))
                    yield return v;
                else
                    dict.Add(KeyValuePairs.Create(key, v));
            }
        }

        public static IEnumerable<Tv> PopAll<Tk, Tv>(this Dict<Tk, Tv> dict, Tk key)
            where Tk : IEquatable<Tk>
        {
            while (true)
            {
                Tv value;
                if (dict.TryGetValue(key, out value))
                    yield return value;
                else
                    yield break;
                dict.Remove(key);
            }
        }

        public static int Pop(this IntSet self)
        {
            if (self.Count == 0)
                throw new InvalidOperationException();
            int result = self.First();
            self.Remove(result);
            return result;
        }

        public static T TryPop<T>(this SymbolDict<T> self, Symbol key)
        {
            T value = default(T);
            if (self.TryGetValue(key, out value))
                self.Remove(key);
            return value;
        }
    }

    #endregion
}

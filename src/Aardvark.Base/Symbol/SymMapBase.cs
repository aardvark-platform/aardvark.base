using System.Collections.Generic;

namespace Aardvark.Base
{
    public class SymMapBase
    {
        protected SymbolDict<object> m_ht;
        protected Symbol m_typeName;

        #region Constructors

        public SymMapBase(Symbol typeName, SymbolDict<object> ht)
        {
            m_typeName = typeName;
            m_ht = ht;
        }

        public SymMapBase()
            : this(Symbol.Empty, new SymbolDict<object>())
        { }

        public SymMapBase(Symbol typeName)
            : this(typeName, new SymbolDict<object>())
        { }

        public SymMapBase(SymMapBase map)
            : this(map.m_typeName, map.m_ht.Copy())
        { }

        /// <summary>
        /// Creates a shallow copy of the supplied map, but uses entries
        /// in the supplied override dictionary instead of map entries
        /// where they exist.
        /// </summary>
        public SymMapBase(SymMapBase map, SymbolDict<object> overrides)
            : this(map.m_typeName,
                   overrides != null ? overrides.Copy() : new SymbolDict<object>())
        {
            if (overrides != null)
            {
                foreach (var item in map.m_ht)
                {
                    if (overrides.Contains(item.Key)) continue;
                    m_ht[item.Key] = item.Value;
                }
            }
            else
            {
                foreach (var item in map.m_ht)
                    m_ht[item.Key] = item.Value;
            }
        }

        #endregion

        #region Indexer

        public object this[string key]
        {
            get
            {
                return m_ht[Symbol.Create(key)];
            }
            set
            {
                m_ht[Symbol.Create(key)] = value;
            }
        }

        public object this[Symbol key]
        {
            get
            {
                return m_ht[key];
            }
            set
            {
                m_ht[key] = value;
            }
        }

        #endregion

        #region Properties

        public Symbol TypeName
        {
            get { return m_typeName; }
            set { m_typeName = value; }
        }

        public IEnumerable<KeyValuePair<Symbol, object>> MapItems
        {
            get { foreach (var kvp in m_ht) yield return kvp; }
        }

        #endregion

        #region Typed Access

        public T Get<T>(TypedSymbol<T> key) => m_ht.Get(key);

        public T GetOrDefault<T>(TypedSymbol<T> key) => m_ht.GetOrDefault(key);

        public T Get<T>(TypedSymbol<T> key, T defaultValue) => m_ht.Get(key, defaultValue);

        public T Get<T>(Symbol key) => m_ht.TryGetValue(key, out object r) ? (T)r : default;

        public T Get<T>(Symbol key, T defaultValue) => m_ht.TryGetValue(key, out object r) ? (T)r : defaultValue;

        public void Set<T>(TypedSymbol<T> key, T value) => m_ht.Set(key, value);

        #endregion

        #region Queries

        /// <summary>
        /// Checks if key exists.
        /// </summary>
        public bool Contains(Symbol key) => m_ht.Contains(key);

        #endregion
    }
}

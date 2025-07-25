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
using System.Collections.Generic;

namespace Aardvark.Base;

public class SymMapBase(Symbol typeName, SymbolDict<object> ht)
{
    protected SymbolDict<object> m_ht = ht;
    protected Symbol m_typeName = typeName;

    #region Constructors

    public SymMapBase()
        : this(Symbol.Empty, [])
    { }

    public SymMapBase(Symbol typeName)
        : this(typeName, [])
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
               overrides != null ? overrides.Copy() : [])
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

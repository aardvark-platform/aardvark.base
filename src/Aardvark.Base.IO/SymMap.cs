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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aardvark.Base.Coder;

[RegisterTypeInfo]

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
public class SymMap : SymMapBase, IFieldCodeable, ITypedMap, IEquatable<SymMap>
#pragma warning restore CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
#pragma warning restore IDE0079 // Remove unnecessary suppression
{
    protected Symbol m_guidSymbol;

    #region Constructors

    public SymMap() : base(Symbol.Empty, [])
    // guidSymbol is Empty here, in order to avoid polluting the
    // symbol table with Guids that are never used.
    {
        m_guidSymbol = Symbol.Empty;
    }

    public SymMap(Symbol typeName, SymbolDict<object> ht)
        : base(typeName, ht)
    {
        m_guidSymbol = Symbol.CreateNewGuid();
    }

    /// <summary>
    /// Creates a shallow copy of the supplied map, but uses entries
    /// in the supplied override dictionary instead of map entries
    /// where they exist.
    /// </summary>
    public SymMap(SymMap map, SymbolDict<object> overrides)
        : base(map, overrides)
    {
        m_guidSymbol = Symbol.CreateNewGuid();
    }

    /// <summary>
    /// Creates a shallow copy of the supplied map, but uses entries
    /// in the supplied override dictionary instead of map entries
    /// where they exist.
    /// </summary>
    public SymMap(SymMap map, SymbolDict<object> overrides, Symbol guidSymbol)
        : base(map, overrides)
    {
        m_guidSymbol = guidSymbol;
    }

    public SymMap(Symbol typeName)
        : base(typeName)
    {
        m_guidSymbol = Symbol.CreateNewGuid();
    }

    public SymMap(Symbol typeName, Symbol guidSymbol)
        : base(typeName)
    {
        m_guidSymbol = guidSymbol;
    }

    #endregion

    #region Properties

    /// <summary>
    /// A unique identifier for each SymMap that can be conveniently used
    /// as key in a dictionary.
    /// </summary>
    public Symbol GuidSymbol
    {
        get { return m_guidSymbol; }
    }

    public void CreateNewGuidSymbolIfEmpty()
    {
        if (m_guidSymbol == Symbol.Empty) m_guidSymbol = Symbol.CreateNewGuid();
    }

    #endregion

    #region IFieldCodeable Members

    /// <summary>
    /// Note that all types that are in this set need to implement IAwakeable
    /// and call CreateNewGuidSymbol in the Awake method.
    /// </summary>
    private static readonly SymbolDict<int> s_lastMapCodedVersionMap = new()
    {
        { "KdTreeSet", 3 },
        { "SphereSet", 3 },
        { "TriangleSet", 3 },
    };

    public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
    {
        if (coderVersion > 2)
        {
            if (coderVersion < 4) // change to current version
            {
                // legacy files that were previously derived from Map

                if (s_lastMapCodedVersionMap.TryGetValue(m_typeName, out int lastMapCodedVersion)
                    && lastMapCodedVersion >= coderVersion)
                {
                    if (m_typeName == GetType().GetCanonicalName()) return [];
                    return
                    [
                       new(0, "TypeName", (c, o) => c.CodePositiveSymbol(ref ((SymMap)o).m_typeName)),
                    ];
                }
            }

            if (m_typeName == GetType().GetCanonicalName())
            {
                return
                [
                    new(0, "Guid", (c, o) => c.CodeGuidSymbol(ref ((SymMap)o).m_guidSymbol)),
                ];
            }
            else
            {
                return
                [
                    new FieldCoder(0, "TypeName", (c, o) => c.CodePositiveSymbol(ref ((SymMap)o).m_typeName)),
                    new FieldCoder(1, "Guid", (c, o) => c.CodeGuidSymbol(ref ((SymMap)o).m_guidSymbol)),
                ];
            }
        }

        if (m_typeName == GetType().GetCanonicalName()) return [];

        return
        [
            new FieldCoder(0, "TypeName", (c, o) =>
                {
                    if (c.IsReading)
                    {
                        string typeName = null;
                        c.CodeString(ref typeName);
                        (((SymMap)o).m_typeName) = Symbol.Create(typeName);
                    }
                    else
                    {
                        string typeName = (((SymMap)o).m_typeName).ToString();
                        c.CodeString(ref typeName);
                    }
                }),
            new FieldCoder(0, "Guid", (c, o) =>
                {
                    if (c.IsReading)
                    {
                        string guid = null;
                        c.CodeString(ref guid);
                        (((SymMap)o).m_guidSymbol) = Symbol.Create(guid);
                    }
                    else
                    {
                        string guid = (((SymMap)o).m_guidSymbol).ToString();
                        c.CodeString(ref guid);
                    }
                }),
        ];
    }

    #endregion

    #region ITypedMap Members

    public IEnumerable<FieldType> FieldTypes
    {
        get
        {
            Type type = this.GetType();

            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (PropertyInfo pi in propertyInfos)
            {
                string name = pi.Name;
                Type propertyType = pi.PropertyType;
                yield return new FieldType(name, propertyType);
            }
        }
    }

    public int FieldCount { get { return m_ht.Count; } }

    private IEnumerable<string> FieldNamesUnsorted
    {
        get { foreach (var k in m_ht.Keys) yield return k.ToString(); }
    }

    public IEnumerable<string> FieldNames
    {
        get { return FieldNamesUnsorted.OrderBy(k => k); }
    }

    #endregion

    #region IEquatable<SymMap> Members

    public bool Equals(SymMap other)
    {
        return this == other;
    }

    #endregion
}

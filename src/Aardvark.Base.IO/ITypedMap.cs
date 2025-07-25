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

namespace Aardvark.Base.Coder;

public struct FieldType(string name, Type type)
{
    private string m_name = name;
    private Type m_type = type;

    #region Constructor

    #endregion

    #region Properties

    public string Name { readonly get { return m_name; } set { m_name = value; } }
    public Type Type { readonly get { return m_type; } set { m_type = value; } }

    #endregion
}

public interface ITypedMap
{
    /// <summary>
    /// Returns the types of all fields of the ITypedMap. Note that it is
    /// possible that this method does not report all fields of the
    /// concrete object it was called for. It just returns fields that are
    /// known in all instances of this class. Also, not all of the fields
    /// for which types are known, are present in all instances of this
    /// class.
    /// </summary>
    IEnumerable<FieldType> FieldTypes { get; }

    /// <summary>
    /// Returns the number of actual fields of the concrete object.
    /// </summary>
    int FieldCount { get; }

    /// <summary>
    /// Returns the names of all fields of the concrete object. Note that
    /// this can return names of fields that are not reported in
    /// <see cref="FieldTypes"/>
    /// </summary>
    IEnumerable<string> FieldNames { get; }

    /// <summary>
    /// This provides access to all the fields by their name.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    object this[string key] { get; set; }
}

#region FieldTypeMap

public class TypeOfString : Dictionary<string, Type> { }
public static class FieldTypeMap
{
    static readonly object s_lock = new();
    static readonly Dictionary<Type, TypeOfString> s_fieldTypeTypeMap = [];

    public static TypeOfString Get(Type type, ITypedMap typedMap)
    {
        TypeOfString fieldTypeMap;
        lock (s_lock)
        {
            if (!s_fieldTypeTypeMap.TryGetValue(type, out fieldTypeMap))
            {
                fieldTypeMap = [];
                foreach (FieldType ft in typedMap.FieldTypes)
                    fieldTypeMap[ft.Name] = ft.Type;
                s_fieldTypeTypeMap[type] = fieldTypeMap;
            }
        }
        return fieldTypeMap;
    }
}

#endregion

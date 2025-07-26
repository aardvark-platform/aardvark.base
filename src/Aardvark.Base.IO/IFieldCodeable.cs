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
using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base.Coder;

/// <summary>
/// A field coder defines the name of a field and an associated coding
/// function which is used for both reading and writing the field.
/// </summary>
/// <remarks>
/// Defines a FieldCoder that is only used for objects with version
/// numbers inside the inclusive interval
/// [minVersionInclusive, maxVersionInclusive].
/// </remarks>
public struct FieldCoder(int order, string name, int minVersionInclusive, int maxVersionInclusive,
                  Action<ICoder, object> code)
{
    readonly int m_order = order;
    string m_name = name;
    readonly int m_minVersion = minVersionInclusive;
    readonly int m_maxVersion = maxVersionInclusive;
    Action<ICoder, object> m_code = code;

    #region Constructors

    public FieldCoder(int order, string name,
                      Action<ICoder, object> code)
        : this(order, name, 0, int.MaxValue, code)
    { }

    #endregion

    #region Properties

    public readonly int Order { get { return m_order; } }
    public string Name { readonly get { return m_name; } set { m_name = value; } }
    public readonly int MinVersion { get { return m_minVersion; } }
    public readonly int MaxVersion { get { return m_maxVersion; } }
    public Action<ICoder, object> Code { readonly get { return m_code; } set { m_code = value; } }

    #endregion
}

/// <summary>
/// A field-codeable needs to supply an array of <see cref="FieldCoder"/>s.
/// </summary>
public interface IFieldCodeable
{
    /// <summary>
    /// Supply a <see cref="FieldCoder"/>s for each field that needs to
    /// be coded. Note that this function is normally only called once
    /// for each type, when the first instance of this type is encountered.
    /// </summary>
    IEnumerable<FieldCoder> GetFieldCoders(int coderVersion);
}

#region FieldCoderExtensions

public static class IEnumerableOfFieldCoderExtensions
{
    /// <summary>
    /// This extension is necessary to renumber the relative order
    /// values of the base class, if a derived class adds additional
    /// field coders. The standard usages is:
    /// <code>
    /// base.GetFieldCoders().Base().Concat(additionalFieldCoders)
    /// </code>
    /// </summary>
    public static IEnumerable<FieldCoder> Base(
        this IEnumerable<FieldCoder> fieldCoders)
    {
        if (!fieldCoders.Any()) return fieldCoders;
        int max = fieldCoders.Max(fc => fc.Order) + 1;
        return from fc in fieldCoders
               select new FieldCoder(fc.Order - max, fc.Name, fc.MinVersion, fc.MaxVersion, fc.Code);
    }
}

#endregion

#region FieldCoderArray

internal static class FieldCoderArray
{
    private static readonly object s_lock = new();
    private static readonly Dictionary<(int, Type, int), FieldCoder[]> s_fieldCoderArrayMap = [];

    public static FieldCoder[] Get(int coderVersion, Type type, int version, IFieldCodeable fieldCodeAble)
    {
        var key = (coderVersion, type, version);
        FieldCoder[] fieldCoderArray;
        lock (s_lock)
        {
            if (!s_fieldCoderArrayMap.TryGetValue(key, out fieldCoderArray))
            {
                fieldCoderArray = [.. fieldCodeAble.GetFieldCoders(coderVersion)
                    .OrderBy(fc => fc.Order)
                    .Where(fc => fc.MinVersion <= version && version <= fc.MaxVersion)];
                s_fieldCoderArrayMap[key] = fieldCoderArray;
            }
        }
        return fieldCoderArray;
    }

}

#endregion

#region FieldCoderMap

internal class FieldCoderMap : Dictionary<string, Action<ICoder, object>>
{
    private static readonly object s_lock = new();
    private static readonly Dictionary<(int, Type, int), FieldCoderMap> s_fieldCoderMapMap = [];

    public static FieldCoderMap Get(int coderVersion, Type type, int version, IFieldCodeable fieldCodeAble)
    {
        var key = (coderVersion, type, version);
        FieldCoderMap fieldCoderMap;
        lock (s_lock)
        {
            if (!s_fieldCoderMapMap.TryGetValue(key, out fieldCoderMap))
            {
                fieldCoderMap = [];
                var fieldCoders = fieldCodeAble.GetFieldCoders(coderVersion)
                          .Where(fc => fc.MinVersion <= version && version <= fc.MaxVersion);
                foreach (var fc in fieldCoders)
                    fieldCoderMap[fc.Name] = fc.Code;
                s_fieldCoderMapMap[key] = fieldCoderMap;
            }
        }
        return fieldCoderMap;
    }
}

#endregion

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

namespace Aardvark.Base;

public static class EnumHelpers
{
    static readonly Dict<Type, Dict<long, (object, object)>> s_neighbourValuesDicts = [];

    static Dict<long, (object, object)> GetNeighbourValuesDict<T>(Type enumType)
    {
        if (!enumType.IsEnum) throw new ArgumentException($"{enumType.Name} is not an enumeration type.");

        if (!s_neighbourValuesDicts.TryGetValue(enumType, out Dict<long, (object, object)> neighbourValuesDict))
        {
            var values = (T[])Enum.GetValues(enumType);

            neighbourValuesDict = [];
            for (int i = 0; i < values.Length; i++)
            {
                neighbourValuesDict[Convert.ToInt64(values[i])] = 
                    (
                        values[(i > 0 ? i : values.Length) - 1], 
                        values[i < values.Length - 1 ? i + 1 : 0]
                    ); // previous and next value
            }

            s_neighbourValuesDicts[enumType] = neighbourValuesDict;
        }

        return neighbourValuesDict;
    }

    /// <summary>
    /// Returns the previous value of the enumeration or the last when the current value is the first one.
    /// </summary>
    public static T GetPrevValue<T>(T enumValue)
    // NOTE: where T: enum
    {
        var neighbourValuesDict = GetNeighbourValuesDict<T>(typeof(T));

        var intValue = Convert.ToInt64(enumValue);
        neighbourValuesDict.TryGetValue(intValue, out (object, object) result);
        return (T)result.Item1;
    }

    /// <summary>
    /// Returns the next value of the enumeration or the first when the current value is the last one.
    /// </summary>
    public static T GetNextValue<T>(T enumValue)
    // NOTE: where T: enum
    {
        var neighbourValuesDict = GetNeighbourValuesDict<T>(typeof(T));

        var intValue = Convert.ToInt64(enumValue);
        neighbourValuesDict.TryGetValue(intValue, out (object, object) result);
        return (T)result.Item2;
    }

    static readonly Dictionary<Type, (Array, Dictionary<long, int>)> s_valueIndexMapping = []; // long = worst case enum value

    static (Array, Dictionary<long, int>) GetValueIndexMapping(Type enumType)
    {
        if (!enumType.IsEnum) throw new ArgumentException($"{enumType.Name} is not an enumeration type.");

        return s_valueIndexMapping.GetCreate(enumType, et =>
        {
            var enumValues = Enum.GetValues(et);
            var enumIndices = new Dictionary<long, int>();
            for (int i = 0; i < enumValues.Length; i++)
                enumIndices.Add(Convert.ToInt64(enumValues.GetValue(i)), i);
            return (enumValues, enumIndices);
        });
    }

    /// <summary>
    /// Return the index of the enumeration value
    /// </summary>
    public static int GetIndex<T>(T enumValue)
    {
        var mapping = GetValueIndexMapping(typeof(T));
        return mapping.Item2[Convert.ToInt64(enumValue)];
    }

    /// <summary>
    /// Return the index of the enumeration value
    /// </summary>
    public static int GetIndex(Type enumType, object enumValue)
    {
        var mapping = GetValueIndexMapping(enumType);
        return mapping.Item2[Convert.ToInt64(enumValue)]; 
    }

    /// <summary>
    /// Returns the enumeartion value of a certain index
    /// </summary>
    public static int GetValue(Type enumType, int index)
    {
        var mapping = GetValueIndexMapping(enumType);
        return (int)mapping.Item1.GetValue(index);
    }

    /// <summary>
    /// Returns the enumeartion value of a certain index
    /// </summary>
    public static T GetValue<T>(int index)
    {
        var mapping = GetValueIndexMapping(typeof(T));
        return (T)mapping.Item1.GetValue(index);
    }
}

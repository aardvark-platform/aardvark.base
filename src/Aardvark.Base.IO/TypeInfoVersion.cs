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

public static class TypeInfoVersion
{
    public static void Add(Type type, int version)
    {
        Add(type, version, null, null, null);
    }

    public static void Add(
            Type type, int version,
            Type targetType, Type newestType,
            Action<Convertible, Convertible> converter)
    {
        lock (s_lock)
        {
            TypeInfo.Add(type, version);
            TypeInfo typeInfo = TypeInfo.OfType[type];

            if (s_oldTypeListOfType.TryGetValue(typeInfo.Type, out List<Type> oldTypes))
            {
                foreach (var oldType in oldTypes)
                    typeInfo.AddVersion(TypeInfo.OfType[oldType]);
                s_oldTypeListOfType.Remove(typeInfo.Type);
            }

            if (s_conversionOfTargetType.TryGetValue(typeInfo.Type, out (string, Action<Convertible, Convertible>) conversion))
            {
                Converter.Global.Register(conversion.Item1, typeInfo.Name, conversion.Item2);
                s_conversionOfTargetType.Remove(typeInfo.Type);
            }

            if (newestType == null) return;

            if (!TypeInfo.OfType.TryGetValue(newestType, out TypeInfo newestTypeInfo))
            {
                if (!s_oldTypeListOfType.TryGetValue(newestType, out List<Type> oldTypeList))
                {
                    oldTypeList = [];
                    s_oldTypeListOfType.Add(newestType, oldTypeList);
                }
                oldTypeList.Add(type);
            }
            else
                newestTypeInfo.AddVersion(typeInfo);

            if (!TypeInfo.OfType.TryGetValue(targetType, out TypeInfo targetTypeInfo))
                s_conversionOfTargetType.Add(targetType, (typeInfo.Name, converter));
            else
                Converter.Global.Register(typeInfo.Name, targetTypeInfo.Name, converter);
        }
    }

    private static readonly Dictionary<Type, List<Type>> s_oldTypeListOfType = [];

    private static readonly Dictionary<Type, (string, Action<Convertible, Convertible>)> s_conversionOfTargetType = [];

    private static readonly object s_lock = new();
}

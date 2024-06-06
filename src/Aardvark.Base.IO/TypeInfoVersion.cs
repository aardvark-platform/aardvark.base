using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Coder
{
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

                List<Type> oldTypes;
                if (s_oldTypeListOfType.TryGetValue(typeInfo.Type,
                                                    out oldTypes))
                {
                    foreach (var oldType in oldTypes)
                        typeInfo.AddVersion(TypeInfo.OfType[oldType]);
                    s_oldTypeListOfType.Remove(typeInfo.Type);
                }

                (string, Action<Convertible, Convertible>) conversion;
                if (s_conversionOfTargetType.TryGetValue(typeInfo.Type,
                                                         out conversion))
                {
                    Converter.Global.Register(conversion.Item1, typeInfo.Name, conversion.Item2);
                    s_conversionOfTargetType.Remove(typeInfo.Type);
                }

                if (newestType == null) return;

                TypeInfo newestTypeInfo;
                if (!TypeInfo.OfType.TryGetValue(newestType, out newestTypeInfo))
                {
                    List<Type> oldTypeList;
                    if (!s_oldTypeListOfType.TryGetValue(newestType, out oldTypeList))
                    {
                        oldTypeList = new List<Type>();
                        s_oldTypeListOfType.Add(newestType, oldTypeList);
                    }
                    oldTypeList.Add(type);
                }
                else
                    newestTypeInfo.AddVersion(typeInfo);

                TypeInfo targetTypeInfo;
                if (!TypeInfo.OfType.TryGetValue(targetType, out targetTypeInfo))
                    s_conversionOfTargetType.Add(targetType, (typeInfo.Name, converter));
                else
                    Converter.Global.Register(typeInfo.Name, targetTypeInfo.Name, converter);
            }
        }

        private static readonly Dictionary<Type, List<Type>> s_oldTypeListOfType =
            new Dictionary<Type, List<Type>>();

        private static readonly Dictionary<Type, (string, Action<Convertible, Convertible>)>
            s_conversionOfTargetType
            = new Dictionary<Type, (string, Action<Convertible, Convertible>)>();

        private static readonly object s_lock = new object();
    }
}

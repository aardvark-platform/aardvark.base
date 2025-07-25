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
using System.Collections;
using System.Collections.Generic;

namespace Aardvark.Base.Coder;

/// <summary>
/// This associates a type with a reading function and a writing
/// action for the type.
/// </summary>
public readonly struct TypeReaderAndWriter(Type type,
    Func<IReadingCoder, object> reader,
    Action<IWritingCoder, object> writer
    )
{
    public readonly Type Type = type;
    public readonly Func<IReadingCoder, object> Reader = reader;
    public readonly Action<IWritingCoder, object> Writer = writer;
}

/// <summary>
/// This class provides coding of primitive types. Reading and Witing of
/// additional primitive types can be registered via the static
/// <see cref="Add"/> methods.
/// </summary>
public static partial class TypeCoder
{
    static readonly Dictionary<Type, Action<IWritingCoder, Type, object>> s_genericWriterMap = new()
    {
        { typeof(List<>),       (c,t,o) => { var v = (IList)o; c.Code(t, ref v); } },
        { typeof(Dictionary<,>),(c,t,o) => { var v = (IDictionary)o; c.Code(t, ref v); } },
        { typeof(Dict<,>),      (c,t,o) => { var v = (ICountableDict)o; c.Code(t, ref v); } },
        { typeof(IntDict<>),    (c,t,o) => { var v = (ICountableDict)o; c.Code(t, ref v); } },
        { typeof(SymbolDict<>), (c,t,o) => { var v = (ICountableDict)o; c.Code(t, ref v); } },
        { typeof(DictSet<>),    (c,t,o) => { var v = (ICountableDictSet)o; c.Code(t, ref v); } },
        { typeof(Vector<>),     (c,t,o) => { var v = (IArrayVector)o; c.Code(t, ref v); } },
        { typeof(Matrix<>),     (c,t,o) => { var v = (IArrayMatrix)o; c.Code(t, ref v); } },
        { typeof(Volume<>),     (c,t,o) => { var v = (IArrayVolume)o; c.Code(t, ref v); } },
        { typeof(Tensor<>),     (c,t,o) => { var v = (IArrayTensorN)o; c.Code(t, ref v); } },
        { typeof(HashSet<>),    (c,t,o) => { c.CodeHashSet(t, ref o); } },
    };

    static readonly Dictionary<Type, Func<IReadingCoder, Type, object>> s_genericReaderMap = new()
    {
        { typeof(List<>),       (c,t) => { var v = default(IList); c.Code(t, ref v); return v; } },
        { typeof(Dictionary<,>),(c,t) => { var v = default(IDictionary); c.Code(t, ref v); return v; } },
        { typeof(Dict<,>),      (c,t) => { var v = default(ICountableDict); c.Code(t, ref v); return v; } },
        { typeof(IntDict<>),    (c,t) => { var v = default(ICountableDict); c.Code(t, ref v); return v; } },
        { typeof(SymbolDict<>), (c,t) => { var v = default(ICountableDict); c.Code(t, ref v); return v; } },
        { typeof(DictSet<>),    (c,t) => { var v = default(ICountableDictSet); c.Code(t, ref v); return v; } },
        { typeof(Vector<>),     (c,t) => { var v = default(IArrayVector); c.Code(t, ref v); return v; } },
        { typeof(Matrix<>),     (c,t) => { var v = default(IArrayMatrix); c.Code(t, ref v); return v; } },
        { typeof(Volume<>),     (c,t) => { var v = default(IArrayVolume); c.Code(t, ref v); return v; } },
        { typeof(Tensor<>),     (c,t) => { var v = default(IArrayTensorN); c.Code(t, ref v); return v; } },
        { typeof(HashSet<>),    (c,t) => { object o = null; c.CodeHashSet(t, ref o); return o; } },
    };

    public static bool WritePrimitive(IWritingCoder coder, Type type, ref object obj)
    {
        if (TypeWriterMap.TryGetValue(type, out Action<IWritingCoder, object> writer))
        {
            writer(coder, obj);
            return true;
        }
        else if (type.IsEnum)
        {
            coder.CodeEnum(type, ref obj);
            return true;
        }
        else if (type.IsArray)
        {
            Array a = (Array)obj;
            coder.Code(type, ref a);
            return true;
        }
        else if (type.IsGenericType)
        {
            if (s_genericWriterMap.TryGetValue(
                    type.GetGenericTypeDefinition(),
                    out Action<IWritingCoder, Type, object> genericWriter))
            {
                genericWriter(coder, type, obj);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Write an object based on the supplied type.
    /// </summary>
    public static void Write(IWritingCoder coder, Type type, ref object obj)
    {
        if (!WritePrimitive(coder, type, ref obj))
            coder.Code(ref obj);
    }

    /// <summary>
    /// Read an object based on the supplied type.
    /// </summary>
    public static bool ReadPrimitive(IReadingCoder coder, Type type, ref object obj)
    {
        if (TypeReaderMap.TryGetValue(type, out Func<IReadingCoder, object> reader))
        {
            obj = reader(coder);
            return true;
        }
        else if (type.IsEnum)
        {
            coder.CodeEnum(type, ref obj);
            return true;
        }
        else if (type.IsArray)
        {
            Array a = null;
            coder.Code(type, ref a);
            obj = a;
            return true;
        }
        else if (type.IsGenericType)
        {
            Type genericType = type.GetGenericTypeDefinition();
            if (s_genericReaderMap.TryGetValue(genericType, out Func<IReadingCoder, Type, object> genericReader))
            {
                obj = genericReader(coder, type);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Read an object based on the supplied type.
    /// </summary>
    public static void Read(IReadingCoder coder, Type type, ref object obj)
    {
        if (!ReadPrimitive(coder, type, ref obj))
            coder.Code(ref obj);
    }

    public static bool IsDirectlyCodeable(Type type)
    {
        if (TypeWriterMap.ContainsKey(type)) return true;
        if (type.IsEnum) return true;
        if (type.IsArray) return true;
        if (type.IsGenericType)
        {
            Type genericType = type.GetGenericTypeDefinition();
            if (s_genericWriterMap.ContainsKey(genericType)) return true;
        }
        return false;
    }

    /// <summary>
    /// Register an array of additional read functions and write
    /// procedures for the data types.
    /// </summary>
    public static void Add(IEnumerable<TypeReaderAndWriter> typeReaderAndWriters)
    {
        foreach (var trw in typeReaderAndWriters)
        {
            TypeReaderMap[trw.Type] = trw.Reader;
            TypeWriterMap[trw.Type] = trw.Writer;
        }
    }

    /// <summary>
    /// Marker class signifying coding of null pointers.
    /// </summary>
    public static class Null
    { }

    /// <summary>
    /// Marker class signifying coding of references.
    /// </summary>
    public static class Reference
    { }

    public static partial class Default
    {
        private static bool s_initialized = false;

        public static void Init()
        {
            if (s_initialized) return;
            s_initialized = true;

            TypeInfo.Add(TypeCoder.Default.Reference);
            TypeInfo.Add(TypeCoder.Default.Null);
            TypeInfo.Add(TypeCoder.Default.Basic);
        }

        /// <summary>
        /// The default way of encoding null pointers.
        /// </summary>
        public static readonly TypeInfo[] Null =
        [
            new TypeInfo("null", typeof(TypeCoder.Null), TypeInfo.Option.Active),
        ];

        public static readonly TypeInfo[] Reference =
        [
            new TypeInfo("ref", typeof(TypeCoder.Reference), TypeInfo.Option.Active),
        ];

        public static readonly TypeInfo[] NoNull =
        [
            new TypeInfo("null", typeof(TypeCoder.Null)),
        ];

        public static readonly TypeInfo[] NoReference =
        [
            new TypeInfo("ref", typeof(TypeCoder.Reference)),
        ];
    }
}

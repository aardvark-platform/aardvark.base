using Aardvark.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aardvark.Base.Coder
{
    /// <summary>
    /// This associates a type with a reading function and a writing
    /// action for the type.
    /// </summary>
    public readonly struct TypeReaderAndWriter
    {
        public readonly Type Type;
        public readonly Func<IReadingCoder, object> Reader;
        public readonly Action<IWritingCoder, object> Writer;

        public TypeReaderAndWriter(Type type,
                Func<IReadingCoder, object> reader,
                Action<IWritingCoder, object> writer
                )
        {
            Type = type; Reader = reader;  Writer = writer;
        }
    }

    /// <summary>
    /// This class provides coding of primitive types. Reading and Witing of
    /// additional primitive types can be registered via the static
    /// <see cref="Add"/> methods.
    /// </summary>
    public static partial class TypeCoder
    {
        static readonly Dictionary<Type, Action<IWritingCoder, Type, object>> s_genericWriterMap =
                                new Dictionary<Type, Action<IWritingCoder, Type, object>>
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

        static readonly Dictionary<Type, Func<IReadingCoder, Type, object>> s_genericReaderMap =
                                new Dictionary<Type, Func<IReadingCoder, Type, object>>
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
            Action<IWritingCoder, object> writer;

            if (TypeWriterMap.TryGetValue(type, out writer))
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
                Action<IWritingCoder, Type, object> genericWriter;
                if (s_genericWriterMap.TryGetValue(
                        type.GetGenericTypeDefinition(),
                        out genericWriter))
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
            Func<IReadingCoder, object> reader;

            if (TypeReaderMap.TryGetValue(type, out reader))
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
                Func<IReadingCoder, Type, object> genericReader;
                Type genericType = type.GetGenericTypeDefinition();
                if (s_genericReaderMap.TryGetValue(genericType, out genericReader))
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
            public static TypeInfo[] Null = new[]
            {
                new TypeInfo("null", typeof(TypeCoder.Null),
                             TypeInfo.Option.Active),
            };

            public static TypeInfo[] Reference = new[]
            {
                new TypeInfo("ref", typeof(TypeCoder.Reference),
                             TypeInfo.Option.Active),
            };

            public static TypeInfo[] NoNull = new[]
            {
                new TypeInfo("null", typeof(TypeCoder.Null)),
            };

            public static TypeInfo[] NoReference = new[]
            {
                new TypeInfo("ref", typeof(TypeCoder.Reference)),
            };
        }
    }
}

using Aardvark.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aardvark.Base.Coder
{
    /// <summary>
    /// A class for reading binary files in an Aardvark specific format.
    /// </summary>
    public partial class BinaryReadingCoder
        : BaseReadingCoder, ICoder, IDisposable, IReadingCoder
    {
        private StreamCodeReader m_reader;
        private string m_fileName;
        private readonly int m_coderVersion;
        private bool m_disposeStream;

        #region Constructors

        public BinaryReadingCoder(Stream stream) : base()
        {
            m_reader = new StreamCodeReader(stream, Encoding.UTF8);
            m_fileName = null;
            var identifier = m_reader.ReadString();
            if (identifier != "Aardvark")
                throw new InvalidOperationException(
                        "attempted read on a file of a format unknown to "
                        + "Aardvark");
            m_coderVersion = m_reader.ReadInt32();
            if (m_coderVersion < 0
                || m_coderVersion > c_coderVersion)
                throw new InvalidOperationException(
                        "attempted read on an Aardvark file with an "
                        + "unsupported coder version");
            /* var size = */m_reader.ReadInt64();
            m_disposeStream = false;
        }

        public BinaryReadingCoder(string fileName)
            : this(File.OpenRead(fileName))
        {
            m_fileName = fileName;
            m_disposeStream = true;
        }

        #endregion

        #region Static Convenience Functions

        public static object ReadFirstObject(string fileName)
        {
            object first = null;
            using (var coder = new BinaryReadingCoder(fileName))
                coder.Code(ref first);
            return first;
        }

        public static object ReadFirstObject(Stream stream)
        {
            object first = null;
            using (var coder = new BinaryReadingCoder(stream))
                coder.Code(ref first);
            return first;
        }

        public static T ReadFirst<T>(string fileName)
        {
            T first = default(T);
            using (var coder = new BinaryReadingCoder(fileName))
                coder.CodeT(ref first);
            return first;
        }

        public static T ReadFirst<T>(Stream stream)
        {
            T first = default(T);
            using (var coder = new BinaryReadingCoder(stream))
                coder.CodeT(ref first);
            return first;
        }

        #endregion

        #region ICoder Members

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public int CoderVersion { get { return m_coderVersion; } }

        public int StreamVersion { get { return m_version; } }

        public int MemoryVersion { get { return m_typeInfoStack.Peek().Version; } }

        internal bool TryGetTypeInfo(string name, out TypeInfo ti)
        {
            if (m_typeInfoOfName.TryGetValue(name, out ti)) return true;
            if (TypeInfo.OfName.TryGetValue(name, out ti)) return true;
            return TypeInfo.TryGetOfFullName(name, out ti);
        }

        private int AddRef(object obj)
        {
            int count = m_refs.Count;
            m_refs.Add(obj);
            if ((m_debugMode & CoderDebugMode.ReportReferences) != 0)
            {
                Report.Line("{0,32}   0x{1:x}", count, obj.GetHashCode());
            }
            return count;
        }

        private object UseRef(int refNum)
        {
            object obj = m_refs[refNum];
            if ((m_debugMode & CoderDebugMode.ReportReferences) != 0)
            {
                Report.Line("{0,32} * 0x{1:x}", refNum, obj.GetHashCode());
            }
            return obj;
        }

        private object UseRef(Guid guid)
        {
            // TODO

            return null;
        }


        public void Code(ref object obj)
        {
            var typeName = m_reader.ReadString();
            TypeInfo newTypeInfo = null;
            TypeInfo typeInfo;
            int objRef = -1;

            if (TryGetTypeInfo(typeName, out typeInfo))
            {
                if (typeInfo.Type == typeof(TypeCoder.Null))
                {
                    if ((typeInfo.Options & TypeInfo.Option.Active) != 0)
                    {
                        obj = null;
                        return;
                    }
                    else
                        throw new Exception("cannot decode null object "
                            + "- change by configuring coder with "
                            + "\"coder.Add(TypeCoder.Default.Null);\"");
                }
                if (typeInfo.Type == typeof(TypeCoder.Reference))
                {
                    if ((typeInfo.Options & TypeInfo.Option.Active) != 0)
                    {
                        if (CoderVersion < 5)
                            obj = UseRef(m_reader.ReadInt32());
                        else
                        {
                            obj = UseRef(m_reader.ReadGuid());
                        }
                        return;
                    }
                    else
                        throw new Exception(
                            "cannot decode multiply referenced object "
                            + "- change by configuring coder with "
                            + "\"coder.Add(TypeCoder.Default.Reference);\"");
                }
            }
            else
            {
                typeInfo = new TypeInfo(typeName, Type.GetType(typeName),
                            TypeInfo.Option.Size | TypeInfo.Option.Version);
                if ((m_debugMode & CoderDebugMode.ReportQualifiedNames) != 0)
                    Report.Line("qualified name \"{0}\"", typeName);
            }

            if ((typeInfo.Options & TypeInfo.Option.Version) != 0)
            {
                m_versionStack.Push(m_version);
                m_version = m_reader.ReadInt32();
                if (m_version < typeInfo.Version)
                {
                    TypeInfo oldTypeInfo;
                    if (typeInfo.VersionMap.TryGetValue(m_version, out oldTypeInfo))
                    {
                        newTypeInfo = typeInfo; typeInfo = oldTypeInfo;
                    }
                }
            }

            long end = 0;
            if ((typeInfo.Options & TypeInfo.Option.Size) != 0)
                end = m_reader.ReadInt64() + m_reader.BaseStream.Position;

            if (typeInfo.Type != null)
            {
                if (! TypeCoder.ReadPrimitive(this, typeInfo.Type, ref obj))
                {
                    if ((typeInfo.Options & TypeInfo.Option.Ignore) != 0)
                    {
                        m_reader.BaseStream.Position = end;
                        obj = null;
                    }
                    else
                    {
                        var codedVersion = m_version;

                        m_typeInfoStack.Push(typeInfo);

                        if (typeInfo.Creator != null)
                            obj = typeInfo.Creator();
                        else
                            obj = FastObjectFactory.ObjectFactory(typeInfo.Type)();

                        if ((m_debugMode & CoderDebugMode.ReportObjects) != 0)
                        {
                            Report.Line("{0,-34} 0x{1:x}", typeName, obj.GetHashCode());
                        }

                        if (m_doRefs) objRef = AddRef(obj);

                        #region code fields based on supported interface

                        var fcobj = obj as IFieldCodeable;
                        if (fcobj != null)
                        {
                            CodeFields(typeInfo.Type, codedVersion, fcobj);
                            if (typeInfo.ProxyType != null)
                            {
                                obj = typeInfo.Proxy2ObjectFun(fcobj);
                                if (m_doRefs) m_refs[objRef] = obj; // fix reference after proxy conversion -> Issue when sub-object (in CodeFields) codes this object! (see #77)
                            }
                            else
                            {
                                var tmobj = obj as ITypedMap;
                                if (tmobj != null) CodeFields(typeInfo.Type, tmobj);
                            }
                            if ((typeInfo.Options & TypeInfo.Option.Size) != 0)
                                m_reader.BaseStream.Position = end;
                        }
                        else
                        {
                            if ((typeInfo.Options & TypeInfo.Option.Size) != 0)
                            {
                                m_reader.BaseStream.Position = end;
                                Report.Warn(
                                    "skipping object of uncodeable type \"{0}\"",
                                    typeName);
                                obj = null;
                            }
                            else
                                throw new Exception(
                                        "cannot skip uncodeable object of type \""
                                        + typeName + '"');
                        }

                        var aobj = obj as IAwakeable;
                        if (aobj != null) aobj.Awake(codedVersion); // codedVersion
                        #endregion

                        m_typeInfoStack.Pop();
                    }
                }
            }
            else
            {
                if ((typeInfo.Options & TypeInfo.Option.Size) != 0)
                {
                    m_reader.BaseStream.Position = end;
                    Report.Warn("skipping object of unknown type " + typeName);
                    obj = null;
                }
                else
                    throw new Exception(
                            "cannot skip object of unknown type \""
                            + typeName + '"');
            }

            if ((typeInfo.Options & TypeInfo.Option.Version) != 0)
            {
                m_version = m_versionStack.Pop();
                if (obj != null && newTypeInfo != null)
                {
                    var source = new Convertible(typeInfo.Name, obj);
                    var target = new Convertible(newTypeInfo.Name, null);
                    source.ConvertInto(target);
                    obj = target.Data;
                    if (m_doRefs) m_refs[objRef] = obj; // fix reference after conversion -> Issue when sub-object (in CodeFields) codes this object! (see #77)
                }
            }
        }

        public void CodeFields(Type type, int version, IFieldCodeable obj)
        {
            FieldCoder[] fca = FieldCoderArray.Get(m_coderVersion, type, version, obj);

            if ((m_debugMode & CoderDebugMode.ReportFields) == 0)
            {
                foreach (var fc in fca)
                    fc.Code(this, obj);
            }
            else
            {
                foreach (var fc in fca)
                {
                    Report.Line("  {0}", fc.Name);
                    fc.Code(this, obj);
                }
            }
        }

        public void CodeFields(Type type, ITypedMap obj)
        {
            TypeOfString fieldTypeMap = FieldTypeMap.Get(type, obj);

            int fieldCount = m_reader.ReadInt32();

            while (fieldCount-- > 0)
            {
                string fieldName = m_reader.ReadString();
                if ((m_debugMode & CoderDebugMode.ReportFields) != 0)
                    Report.Line("  {0}", fieldName);
                object value = null;
                bool directlyCodeable = m_reader.ReadBoolean();
                if (directlyCodeable)
                {
                    Type fieldType;
                    if (!fieldTypeMap.TryGetValue(fieldName, out fieldType))
                        throw new InvalidOperationException(String.Format("Could not get type of field \"{0}\"", fieldName));
                        // this can happen if a SymMap is used and a field has been removed
                    TypeCoder.Read(this, fieldType, ref value);
                }
                else
                    Code(ref value);
                obj[fieldName] = value;
            }
        }

        public void CodeT<T>(ref T obj)
        {
            object o = default(T);
            var type = typeof(T);
            if (type == typeof(Array))
                Code(ref o);
            else
                TypeCoder.Read(this, type, ref o);
            obj = (T)o;
        }

        public object ReferenceObject(int refNum)
        {
            return null;
        }

        private bool CodeNull<T>(ref T value) where T: class
        {
            bool isNonNull = m_reader.ReadBoolean();
            if (isNonNull) return false;
            value = null;
            return true;
        }

        public int CodeCount<T>(ref T value, Func<int, T> creator) where T : class
        {
            int count = m_reader.ReadInt32();
            if (count < 0)
            {
                switch (count)
                {
                    case -1:
                        value = null; return -1;
                    case -2:
                        int refNum = m_reader.ReadInt32();
                        if (refNum >= m_refs.Count)
                        {
                            Report.Warn("skipping invalid reference to " + refNum);
                            value = null; return -1;
                        }
                        value = (T)UseRef(refNum);
                        return -2;
                    default:
                        Report.Warn("encountered invalid count of " + count);
                        value = null; return -1;
                }
            }
            value = creator(count);
            if (m_doRefs) AddRef(value); 
            return count;
        }

        private long CodeLong(long intValue)
        {
            return ((intValue & (long)int.MaxValue) << 32) + (long)m_reader.ReadUInt32();
        }

        /// <summary>
        /// Backward compatible extension for coding collections with 64-bit lengths.
        /// </summary>
        public long CodeCountLong<T>(ref T value, Func<long, T> creator) where T : class
        {
            long count = m_reader.ReadInt32();
            if (count < 0)
            {
                switch (count)
                {
                    case -1:
                        value = null; return -1L;
                    case -2:
                        int refNum = m_reader.ReadInt32();
                        if (refNum >= m_refs.Count)
                        {
                            Report.Warn("skipping invalid reference to " + refNum);
                            value = null;
                        }
                        else
                            value = (T)UseRef(refNum);
                        return -2L;
                    default:
                        count = CodeLong(count);
                        break;
                }
            }
            value = creator(count);
            if (m_doRefs) AddRef(value);
            return count;
        }

        /// <summary>
        /// Backward compatible extension for coding collections with 64-bit lengths.
        /// </summary>
        public long[] CodeCountArray<T>(ref T value, Func<long[], T> creator) where T : class
        {
            long intCount = m_reader.ReadInt32();
            long[] countArray = null;
            if (intCount >= 0)
                countArray = intCount.IntoArray();
            if (intCount < 0)
            {
                switch (intCount)
                {
                    case -1:
                        value = null; return (-1L).IntoArray();
                    case -2:
                        int refNum = m_reader.ReadInt32();
                        if (refNum >= m_refs.Count)
                        {
                            Report.Warn("skipping invalid reference to " + refNum);
                            value = null;
                        }
                        else
                            value = (T)UseRef(refNum);
                        return (-2L).IntoArray();
                    case -3:
                    case -4:
                        var dim = -intCount - 1;
                        countArray = new long[dim];
                        for (int d = 0; d < dim; d++)
                        {
                            long ci = m_reader.ReadInt32();
                            countArray[d] = ci >= 0 ? ci : CodeLong(ci);
                        }
                        break;
                    default:
                        countArray = CodeLong(intCount).IntoArray();
                        break;
                }
            }
            value = creator(countArray);
            if (m_doRefs) AddRef(value);
            return countArray;
        }

        private long CodeCountLong<T>(ref T[] array)
        {
            return CodeCountLong(ref array, n => new T[n]);
        }

        private long[] CodeCountLong<T>(ref T[,] array)
        {
            return CodeCountArray(ref array, ca => new T[ca[0], ca[1]]);
        }

        private long[] CodeCountLong<T>(ref T[,,] array)
        {
            return CodeCountArray(ref array, ca => new T[ca[0], ca[1], ca[2]]);
        }

        private long CodeCountLong<T>(ref T[][] array)
        {
            return CodeCountLong(ref array, c => new T[c][]);
        }

        private long CodeCountLong<T>(ref T[][][] array)
        {
            return CodeCountLong(ref array, c => new T[c][][]);
        }

        private int CodeCount<T>(ref List<T> list)
        {
            return CodeCount(ref list, n => new List<T>(n));
        }

        public void CodeTArray<T>(ref T[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeT(ref value[i]);
        }

        public void CodeList_of_T_<T>(ref List<T> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            while (count-- > 0)
            {
                object o = default(T); Code(ref o); value.Add((T)o);
            }
        }

        public void CodeHashSet_of_T_<T>(ref HashSet<T> value)
        {
            int count = CodeCount(ref value, c => new HashSet<T>());
            if (count < 1) return;
            while (count-- > 0)
            {
                object o = default(T); Code(ref o); value.Add((T)o);
            }
        }

        public void Code(Type t, ref object o)
        {
            TypeCoder.Read(this, t, ref o);
        }

        private static Array CreateArray(Type t, long[] countArray)
        {
            t = t.GetElementType();
            switch (countArray.Length)
            {
                case 1: return Array.CreateInstance(t, countArray[0]);
                case 2: return Array.CreateInstance(t, countArray[0], countArray[1]);
                case 3: return Array.CreateInstance(t, countArray[0], countArray[1], countArray[2]);
            }
            throw new ArgumentException("cannot decode array with more than 3 dimensions");
        }

        public void Code(Type t, ref Array array)
        {
            long[] countArray = CodeCountArray(ref array, ca => CreateArray(t, ca));
            long c0 = countArray[0];  if (c0 < 1) return;
            if (countArray.Length == 1)
            {
                for (int i = 0; i < c0; i++)
                {
                    object o = null; Code(ref o); array.SetValue(o, i);
                }
                return;
            }
            var c1 = countArray[1]; if (c1 < 1) return;
            if (countArray.Length == 2)
            {
                for (long i = 0; i < c0; i++)
                for (long j = 0; j < c1; j++)
                {
                    object o = null; Code(ref o); array.SetValue(o, i, j);
                }
                return;
            }
            var c2 = countArray[3]; if (c2 < 1) return;
            if (countArray.Length == 3)
            {
                for (long i = 0; i < c0; i++)
                for (long j = 0; j < c1; j++)
                for (long k = 0; k < c2; k++)
                {
                    object o = null; Code(ref o); array.SetValue(o, i, j, k);
                }
                return;
            }
            throw new ArgumentException("cannot decode array with more than 3 dimensions");
        }

        public void CodeOld(Type t, ref Array array)
        {
            int count = CodeCount(ref array,
                                  n => (Array)Activator.CreateInstance(t, n));
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                object o = null;
                Code(ref o);
                array.SetValue(o, i);
            }
        }

        public void Code(Type t, ref IList list)
        {
            int count = CodeCount(ref list,
                                  n => (IList)Activator.CreateInstance(t, n));
            if (count < 1) return;
            while (count-- > 0)
            {
                object o = null;
                Code(ref o);
                list.Add(o);
            }
        }

        public void Code(Type t, ref IDictionary dict)
        {
            int count = CodeCount(ref dict,
                                  n => (IDictionary)Activator.CreateInstance(t, n));
            if (count < 1) return;

            Type[] subTypeArray = t.GetGenericArguments();

            while (count-- > 0)
            {
                object key = null;
                TypeCoder.Read(this, subTypeArray[0], ref key);
                object val = null;
                TypeCoder.Read(this, subTypeArray[1], ref val);
                dict[key] = val;
            }
        }

        public void Code(Type t, ref ICountableDict dict)
        {
            int count = CodeCount(ref dict,
                                  n => (ICountableDict)Activator.CreateInstance(t, n));
            if (count < 1) return;

            var keyType = dict.KeyType;
            var valueType = dict.ValueType;

            while (count-- > 0)
            {
                object key = null;
                TypeCoder.Read(this, keyType, ref key);
                object val = null;
                TypeCoder.Read(this, valueType, ref val);
                dict.AddObject(key, val);
            }
        }

        public void Code(Type t, ref ICountableDictSet dictSet)
        {
            int count = CodeCount(ref dictSet,
                                  n => (ICountableDictSet)Activator.CreateInstance(t, n));
            if (count < 1) return;

            var keyType = dictSet.KeyType;

            while (count-- > 0)
            {
                object key = null;
                TypeCoder.Read(this, keyType, ref key);
                dictSet.AddObject(key);
            }
        }

        public void Code(Type t, ref IArrayVector value)
        {
            value = (IArrayVector)FastObjectFactory.ObjectFactory(t)();
            Array array = null; Code(value.ArrayType, ref array); value.Array = array;
            long origin = 0L; CodeLong(ref origin); value.Origin = origin;
            long length = 0L; CodeLong(ref length); value.Size = length;
            long delta = 0L; CodeLong(ref delta); value.Delta = delta;
        }

        public void Code(Type t, ref IArrayMatrix value)
        {
            value = (IArrayMatrix)FastObjectFactory.ObjectFactory(t)();
            Array array = null; Code(value.ArrayType, ref array); value.Array = array;
            long origin = 0L; CodeLong(ref origin); value.Origin = origin;
            V2l length = default(V2l); CodeV2l(ref length); value.Size = length;
            V2l delta = default(V2l); CodeV2l(ref delta); value.Delta = delta;
        }

        public void Code(Type t, ref IArrayVolume value)
        {
            value = (IArrayVolume)FastObjectFactory.ObjectFactory(t)();
            Array array = null; Code(value.ArrayType, ref array); value.Array = array;
            long origin = 0L; CodeLong(ref origin); value.Origin = origin;
            V3l length = default(V3l); CodeV3l(ref length); value.Size = length;
            V3l delta = default(V3l); CodeV3l(ref delta); value.Delta = delta;
        }

        public void Code(Type t, ref IArrayTensor4 value)
        {
            value = (IArrayTensor4)FastObjectFactory.ObjectFactory(t)();
            Array array = null; Code(value.ArrayType, ref array); value.Array = array;
            long origin = 0L; CodeLong(ref origin); value.Origin = origin;
            V4l length = default(V4l); CodeV4l(ref length); value.Size = length;
            V4l delta = default(V4l); CodeV4l(ref delta); value.Delta = delta;
        }

        public void Code(Type t, ref IArrayTensorN value)
        {
            value = (IArrayTensorN)FastObjectFactory.ObjectFactory(t)();
            Array array = null; Code(value.ArrayType, ref array); value.Array = array;
            long origin = 0L; CodeLong(ref origin); value.Origin = origin;
            long[] length = null; CodeLongArray(ref length); value.Size = length;
            long[] delta = null; CodeLongArray(ref delta); value.Delta = delta;
        }

        public void CodeHashSet(Type t, ref object obj)
        {
            throw new NotImplementedException("only possible whith a non-generic ISet interface)");
        }

        public void CodeEnum(Type type, ref object value)
        {
            Type systemType = Enum.GetUnderlyingType(type);
            var reader = TypeCoder.TypeReaderMap[systemType];
            object obj = reader(this);
            value = Enum.ToObject(type, obj);
        }

        #endregion

        #region Code Primitives

        public void CodeBool(ref bool value)
        {
            value = m_reader.ReadBoolean();
        }

        public void CodeByte(ref byte value)
        {
            value = m_reader.ReadByte();
        }

        public void CodeSByte(ref sbyte value)
        {
            value = m_reader.ReadSByte();
        }

        public void CodeShort(ref short value)
        {
            value = m_reader.ReadInt16();
        }

        public void CodeUShort(ref ushort value)
        {
            value = m_reader.ReadUInt16();
        }

        public void CodeInt(ref int value)
        {
            value = m_reader.ReadInt32();
        }

        public void CodeUInt(ref uint value)
        {
            value = m_reader.ReadUInt32();
        }

        public void CodeLong(ref long value)
        {
            value = m_reader.ReadInt64();
        }

        public void CodeULong(ref ulong value)
        {
            value = m_reader.ReadUInt64();
        }

        public void CodeFloat(ref float value)
        {
            value = m_reader.ReadSingle();
        }

        public void CodeDouble(ref double value)
        {
            value = m_reader.ReadDouble();
        }

        public void CodeChar(ref char value)
        {
            value = m_reader.ReadChar();
        }

        public void CodeString(ref string value)
        {
            if (m_coderVersion == 0)
                value = m_reader.ReadString();
            else
            {
                if (CodeNull(ref value)) return;
                value = m_reader.ReadString();
            }
        }

        public void CodeType(ref Type value)
        {
            var typeName = m_reader.ReadString();
            TypeInfo ti;

            if (TryGetTypeInfo(typeName, out ti))
                value = ti.Type;
            else
                value = Type.GetType(typeName);
        }

        public void CodeGuid(ref Guid value) { value = m_reader.ReadGuid(); }

        public void CodeSymbol(ref Symbol value) { value = m_reader.ReadSymbol(); }

        public void CodeGuidSymbol(ref Symbol value) { value = m_reader.ReadGuidSymbol(); }

        public void CodePositiveSymbol(ref Symbol value) { value = m_reader.ReadPositiveSymbol(); }

        public void CodeIntSet(ref IntSet set)
        {
            int count = CodeCount(ref set, n => new IntSet(n));
            if (count < 1) return;
            while (count-- > 0) set.Add(m_reader.ReadInt32());
        }

        public void CodeSymbolSet(ref SymbolSet set)
        {
            int count = CodeCount(ref set, n => new SymbolSet(n));
            if (count < 1) return;
            while (count-- > 0) set.Add(m_reader.ReadSymbol());
        }

        public void CodeFraction(ref Fraction value)
        {
            long numerator = m_reader.ReadInt64();
            long denominator = m_reader.ReadInt64();
            value = new Fraction(numerator, denominator);
        }

        public void CodeCameraExtrinsics(ref CameraExtrinsics v)
        {
            var rotation = default(M33d); var translation = default(V3d);
            CodeM33d(ref rotation); CodeV3d(ref translation);
            v = new CameraExtrinsics(rotation, translation);
        }

        public void CodeCameraIntrinsics(ref CameraIntrinsics v)
        {
            var focalLength = default(V2d); var principalPoint = default(V2d);
            double skew = 0.0, k1 = 0.0, k2 = 0.0, k3 = 0.0, p1 = 0.0, p2 = 0.0;
            CodeV2d(ref focalLength); CodeV2d(ref principalPoint);
            CodeDouble(ref skew);
            CodeDouble(ref k1); CodeDouble(ref k2); CodeDouble(ref k3);
            CodeDouble(ref p1); CodeDouble(ref p1);
            v = new CameraIntrinsics(focalLength, principalPoint, skew, k1, k2, k3, p1, p2);
        }

        #endregion

        #region Code Arrays

        public void CodeStructArray<T>(ref T[] value)
            where T : struct
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBoolArray(ref bool[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) value[i] = m_reader.ReadBoolean();
        }

        public void CodeByteArray(ref byte[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeSByteArray(ref sbyte[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeShortArray(ref short[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeUShortArray(ref ushort[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeIntArray(ref int[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeUIntArray(ref uint[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeLongArray(ref long[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeULongArray(ref ulong[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeFloatArray(ref float[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeDoubleArray(ref double[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeCharArray(ref char[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) value[i] = m_reader.ReadChar();
        }

        public void CodeStringArray(ref string[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) value[i] = m_reader.ReadString();
        }

        public void CodeTypeArray(ref Type[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;

            for (long i = 0; i < count; i++)
            {
                var typeName = m_reader.ReadString();
                TypeInfo ti;

                if (TryGetTypeInfo(typeName, out ti))
                    value[i] = ti.Type;
                else
                    value[i] = Type.GetType(typeName);
            }
        }

        public void CodeGuidArray(ref Guid[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeGuid(ref value[i]);
        }

        public void CodeSymbolArray(ref Symbol[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeSymbol(ref value[i]);
        }

        public void CodeFractionArray(ref Fraction[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        #endregion

        #region Code Lists

        public void CodeStructList<T>(ref List<T> value)
            where T : struct
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Bool_(ref List<bool> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            while (count-- > 0) value.Add(m_reader.ReadBoolean());
        }

        public void CodeList_of_Byte_(ref List<byte> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            while (count-- > 0) value.Add(m_reader.ReadByte());
        }

        public void CodeList_of_SByte_(ref List<sbyte> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Short_(ref List<short> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_UShort_(ref List<ushort> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Int_(ref List<int> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_UInt_(ref List<uint> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Long_(ref List<long> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_ULong_(ref List<ulong> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Float_(ref List<float> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Double_(ref List<double> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Char_(ref List<char> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            while (count-- > 0) value.Add(m_reader.ReadChar());
        }

        public void CodeList_of_String_(ref List<string> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            while (count-- > 0) value.Add(m_reader.ReadString());
            //for (int i = 0; i < count; i++) value[i] = m_reader.ReadString();
        }

        public void CodeList_of_Type_(ref List<Type> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;

            for (int i = 0; i < count; i++)
            {
                var typeName = m_reader.ReadString();
                TypeInfo ti;

                if (TryGetTypeInfo(typeName, out ti))
                    value.Add(ti.Type);
                else
                    value.Add(Type.GetType(typeName));
            }
        }

        public void CodeList_of_Guid_(ref List<Guid> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            //while (count-- > 0) value.Add(new Guid(m_reader.ReadString()));
            for (int i = 0; i < count; i++)
            {
                var m = new Guid();
                CodeGuid(ref m);
                value.Add(m);
                //value[i] = new Guid(m_reader.ReadString());
            }
        }

        public void CodeList_of_Symbol_(ref List<Symbol> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Symbol);
                CodeSymbol(ref m);
                value.Add(m);
            }
        }

        public void CodeList_of_Fraction_(ref List<Fraction> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // dispose reader
            if (m_disposeStream)
                m_reader.BaseStream.Dispose();
        }

        #endregion

    }
}

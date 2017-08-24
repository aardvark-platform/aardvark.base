using Aardvark.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aardvark.VRVis
{

    /// <summary>
    /// A class for writing binary files in an Aardvark specific format.
    /// </summary>
    public partial class BinaryWritingCoder
        : BaseWritingCoder, IWritingCoder, IDisposable
    {
        private StreamCodeWriter m_writer;
        private string m_fileName;
        private long m_size = 0;
        private long m_sizePosition = 0;
        private long m_startPosition = 0;
        private bool m_disposeStream;

        #region Constructors

        public BinaryWritingCoder(Stream stream) : base()
        {
            m_writer = new StreamCodeWriter(stream, Encoding.UTF8);
            m_fileName = null;
            m_writer.Write("Aardvark");
            m_writer.Write(c_coderVersion);
            m_sizePosition = m_writer.BaseStream.Position;
            m_writer.Write(m_size);
            m_startPosition = m_writer.BaseStream.Position;
            m_disposeStream = false;
        }

        public BinaryWritingCoder(string fileName)
            : this(File.Open(fileName, FileMode.Create))
        {
            m_fileName = fileName;
            m_disposeStream = true;
        }

        #endregion

        #region Static Convenience Methods

        public static void WriteObject(string fileName, object obj)
        {
            using (var coder = new BinaryWritingCoder(fileName))
                coder.Code(ref obj);
        }

        public static void WriteObject(Stream stream, object obj)
        {
            using (var coder = new BinaryWritingCoder(stream))
                coder.Code(ref obj);
        }

        #endregion

        #region ICoder Members

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public int CoderVersion { get { return c_coderVersion; } }

        public int StreamVersion { get { return m_version; } }

        public int MemoryVersion { get { return m_version; } }

        private void AddRef(object obj)
        {
            int count = m_refs.Count;
            m_refs[obj] = count;
            if ((m_debugMode & CoderDebugMode.ReportReferences) != 0)
            {
                Report.Line("{0,32}   0x{1:x}", count, obj.GetHashCode());
            }
        }

        private bool HandleRef(ref object obj)
        {
            if (m_doRefs)
            {
                if (c_coderVersion < 5)
                {
                    int index;
                    if (m_refs.TryGetValue(obj, out index))
                    {
                        TypeInfo typeInfo;
                        if (TryGetTypeInfo(typeof(TypeCoder.Reference), out typeInfo))
                        {
                            m_writer.Write(typeInfo.Name);
                            m_writer.Write(index);
                            if ((m_debugMode & CoderDebugMode.ReportReferences) != 0)
                            {
                                Report.Line("{0,32} * 0x{1:x}", index, obj.GetHashCode());
                            }
                            return true;
                        }
                        else
                            throw new Exception(
                                    "cannot encode multiply referenced object "
                                    + "- change by configuring coder with "
                                    + "\"coder.Add(TypeCoder.Default.Reference);\"");
                    }
                    AddRef(obj);
                }
                else
                {
                    // TODO
                }
            }
            return false;
        }

        public void Code(ref object obj)
        {
            TypeInfo typeInfo;

            #region handle null objects

            if (obj == null)
            {
                if (TryGetTypeInfo(typeof(TypeCoder.Null), out typeInfo)
                    && ((typeInfo.Options & TypeInfo.Option.Active) != 0))
                {
                    m_writer.Write(typeInfo.Name);
                    return;
                }
                else
                    throw new Exception("cannot encode null object "
                            + "- change by configuring coder with "
                            + "\"coder.Add(TypeCoder.Default.Null);\"");
            }

            #endregion

            Type type = obj.GetType();
            string typeName;
            TypeInfo.Option typeOptions = TypeInfo.Option.Size
                                            | TypeInfo.Option.Version;

            if (!TypeCoder.IsDirectlyCodeable(type))
                if (HandleRef(ref obj)) return;

            #region handle modified type names

            if (TryGetTypeInfo(type, out typeInfo))
            {
                typeName = typeInfo.Name;
                typeOptions = typeInfo.Options;
            }
            else
            {
                typeName = type.AssemblyQualifiedName;
                if ((m_debugMode & CoderDebugMode.ReportQualifiedNames) != 0)
                    Report.Line("qualified name \"{0}\"", typeName); 
            }

            #endregion

            m_writer.Write(typeName);

            #region code version placeholder

            if ((typeOptions & TypeInfo.Option.Version) != 0)
            {
                m_versionStack.Push(m_version);
                m_version = typeInfo != null ? typeInfo.Version : 0;
                m_writer.Write(m_version);
            }

            #endregion

            #region code size placeholder

            long size = 0;
            long sizePosition = 0;
            long startPosition = 0;
            if ((typeOptions & TypeInfo.Option.Size) != 0)
            {
                sizePosition = m_writer.BaseStream.Position;
                m_writer.Write(size);
                startPosition = m_writer.BaseStream.Position;
            }

            #endregion

            #region code fields based on supported interface

            if (!TypeCoder.WritePrimitive(this, type, ref obj))
            {
                m_typeInfoStack.Push(typeInfo);

                if ((m_debugMode & CoderDebugMode.ReportObjects) != 0)
                {
                    Report.Line("{0,-34} 0x{1:x}", typeName, obj.GetHashCode());
                }

                var fcobj = obj as IFieldCodeable;
                if (fcobj != null)
                {
                    CodeFields(type, m_version, fcobj);
                    var tmobj = obj as ITypedMap;
                    if (tmobj != null) CodeFields(type, tmobj);
                }
                else if (typeInfo.ProxyType != null)
                {
                    CodeFields(type, m_version, typeInfo.Object2ProxyFun(obj));
                }
                else
                {
                    throw new Exception("uncodeable object of type \""
                                        + typeName + '\"');
                }

                m_typeInfoStack.Pop();
            }

            #endregion

            #region backpatch size into stream

            if ((typeOptions & TypeInfo.Option.Size) != 0)
            {
                var position = m_writer.BaseStream.Position;

                size = position - startPosition;
                m_writer.BaseStream.Position = sizePosition;
                m_writer.Write(size);
                m_writer.BaseStream.Position = position;
            }
            #endregion

            #region pop version stack

            if ((typeOptions & TypeInfo.Option.Version) != 0)
                m_version = m_versionStack.Pop();

            #endregion
        }

        public void CodeFields(Type type, int version, IFieldCodeable obj)
        {
            FieldCoder[] fca = FieldCoderArray.Get(c_coderVersion, type, version, obj);
            if ((m_debugMode & CoderDebugMode.ReportFields) == 0)
            {
                foreach (var fc in fca) fc.Code(this, obj);
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

            m_writer.Write(obj.FieldCount);

            foreach (string fieldName in obj.FieldNames)
            {
                m_writer.Write(fieldName);
                object value = obj[fieldName];
                bool directlyCodeable = true;
                Type fieldType = null;

                if ((m_debugMode & CoderDebugMode.ReportFields) != 0)
                    Report.Line("  {0}", fieldName);

                // A field is directly codeable, if its type information can
                // be computed by the writer and the reader without
                // inspecting the actual field value.

                if (!fieldTypeMap.TryGetValue(fieldName, out fieldType)
                    || (value != null && fieldType != value.GetType())
                    || !TypeCoder.IsDirectlyCodeable(fieldType))
                    directlyCodeable = false;

                // We store the directly codeable flag, so that a stored file
                // can be read, even if the class has been modified, and the
                // value of directly codeable has been changed.

                m_writer.Write(directlyCodeable);
                if (directlyCodeable)
                    TypeCoder.Write(this, fieldType, ref value);
                else
                    Code(ref value);
            }
        }

        public void CodeT<T>(ref T obj)
        {
            object o = obj;
            if (typeof(T) == typeof(Array) || obj == null)
                Code(ref o);
            else
                TypeCoder.Write(this, obj.GetType(), ref o);
        }

        public void Code(Type t, ref object o)
        {
            TypeCoder.Write(this, t, ref o);
        }

        public bool CodeNull<T>(ref T value) where T : class
        {
            bool isNonNull = value != null;
            m_writer.Write(isNonNull);
            return !isNonNull;
        }

        public int CodeCount<T>(ref T value, Func<T, int> counter) where T : class
        {
            if (value == null) { m_writer.Write(-1); return -1; }
            int refNum;
            if (m_refs.TryGetValue(value, out refNum))
            {
                m_writer.Write(-2); m_writer.Write(refNum);
                if ((m_debugMode & CoderDebugMode.ReportReferences) != 0)
                {
                    Report.Line("{0,32} * 0x{1:x}", refNum, value.GetHashCode());
                }
                return -2;
            }
            int count = counter(value);
            m_writer.Write(count);
            if (m_doRefs) AddRef(value);
            return count;
        }

        private void CodeLong(long value)
        {
            if (value > (long)int.MaxValue)
            {
                m_writer.Write(int.MinValue | (int)(value >> 32));
                m_writer.Write((uint)value);
            }
            else
                m_writer.Write((uint)value);
        }

        /// <summary>
        /// Backward compatible extension for coding collections with 64-bit lengths.
        /// </summary>
        public long CodeCountLong<T>(ref T value, Func<T, long> counter) where T : class
        {
            if (value == null) { m_writer.Write(-1); return -1L; }
            int refNum;
            if (m_refs.TryGetValue(value, out refNum))
            {
                m_writer.Write(-2); m_writer.Write(refNum);
                if ((m_debugMode & CoderDebugMode.ReportReferences) != 0)
                {
                    Report.Line("{0,32} * 0x{1:x}", refNum, value.GetHashCode());
                }
                return -2L;
            }
            long count = counter(value);
            CodeLong(count);
            if (m_doRefs) AddRef(value);
            return count;
        }

        /// <summary>
        /// Backward compatible extension for coding collections with 64-bit lengths.
        /// </summary>
        public long[] CodeCountArray<T>(ref T value, Func<T, long[]> counter) where T : class
        {
            if (value == null) { m_writer.Write(-1); return (-1L).IntoArray(); }
            int refNum;
            if (m_refs.TryGetValue(value, out refNum))
            {
                m_writer.Write(-2); m_writer.Write(refNum);
                if ((m_debugMode & CoderDebugMode.ReportReferences) != 0)
                {
                    Report.Line("{0,32} * 0x{1:x}", refNum, value.GetHashCode());
                }
                return (-2L).IntoArray();
            }
            long[] countArray = counter(value);
            if (countArray.Length == 1)
            {
                long count = countArray[0];
                CodeLong(count);
                if (m_doRefs) AddRef(value);
                return count.IntoArray();
            }
            if (countArray.Length < 4)
            {
                m_writer.Write(-1 - countArray.Length);
                foreach (long ci in countArray) CodeLong(ci);
                if (m_doRefs) AddRef(value);
                return countArray;
            }
            throw new ArgumentException("cannot encode array with more than 3 dimensions");
        }

        private long CodeCountLong<T>(ref T[] array)
        {
            return CodeCountLong(ref array, v => v.LongLength);
        }

        private long[] CodeCountLong<T>(ref T[,] array)
        {
            return CodeCountArray(ref array, v => new[] {
                                  v.GetLongLength(0), v.GetLongLength(1) });
        }

        private long[] CodeCountLong<T>(ref T[,,] array)
        {
            return CodeCountArray(ref array, v => new[] {
                                  v.GetLongLength(0), v.GetLongLength(1), v.GetLongLength(2) });
        }

        private long CodeCountLong<T>(ref T[][] array)
        {
            return CodeCountLong(ref array, v => v.GetLongLength(0) );
        }

        private long CodeCountLong<T>(ref T[][][] array)
        {
            return CodeCountLong(ref array, v => v.GetLongLength(0) );
        }

        private int CodeCount<T>(ref List<T> list)
        {
            return CodeCount(ref list, v => v.Count);
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
            for (int i = 0; i < count; i++)
            {
                object o = value[i];
                Code(ref o);
            }
        }

        public void CodeHashSet_of_T_<T>(ref HashSet<T> value)
        {
            int count = CodeCount(ref value, v => v.Count);
            if (count < 1) return;
            foreach (var e in value)
            {
                object o = e;
                Code(ref o);
            }
        }

        private static long[] LengthArray(Array array)
        {
            switch (array.Rank)
            {
                case 1: return array.LongLength.IntoArray();
                case 2: return new[] { array.GetLongLength(0), array.GetLongLength(1) };
                case 3: return new[] { array.GetLongLength(0), array.GetLongLength(1), array.GetLongLength(2) };
            }
            throw new ArgumentException("cannot code arrays with more than 3 dimensions");
        }


        public void Code(Type t, ref Array value)
        {
            long[] countArray = CodeCountArray(ref value, v => LengthArray(v));
            var c0 = countArray[0]; if (c0 < 1) return;
            if (countArray.Length == 1)
            {
                for (long i = 0; i < c0; i++)
                {
                    object o = value.GetValue(i); Code(ref o);
                }
                return;
            }
            var c1 = countArray[1]; if (c1 < 1) return;
            if (countArray.Length == 2)
            {
                for (long i = 0; i < c0; i++)
                for (long j = 0; j < c1; j++)
                {
                    object o = value.GetValue(i, j); Code(ref o);
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
                    object o = value.GetValue(i, j, k); Code(ref o);
                }
                return;
            }
            throw new ArgumentException("cannot code arrays with more than 3 dimensions");
        }

        public void CodeOld(Type t, ref Array value)
        {
            int count = CodeCount(ref value, v => v.Length);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                object o = value.GetValue(i);
                Code(ref o);
            }
        }

        public void Code(Type t, ref IList list)
        {
            int count = CodeCount(ref list, v => v.Count);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                object o = list[i];
                Code(ref o);
            }
        }

        public void Code(Type t, ref IDictionary dict)
        {
            int count = CodeCount(ref dict, v => v.Count);
            if (count < 1) return;
            Type[] subTypeArray = t.GetGenericArguments();
            foreach (object k in dict.Keys)
            {
                var key = k;
                TypeCoder.Write(this, subTypeArray[0], ref key);
                var val = dict[key];
                TypeCoder.Write(this, subTypeArray[1], ref val);
            }
        }

        public void Code(Type t, ref ICountableDict dict)
        {
            long count = CodeCountLong(ref dict, v => v.LongCount);
            if (count < 1L) return;
            var keyType = dict.KeyType;
            var valueType = dict.ValueType;
            foreach (var pair in dict.ObjectPairs)
            {
                var key = pair.E0;
                TypeCoder.Write(this, keyType, ref key);
                var val = pair.E1;
                TypeCoder.Write(this, valueType, ref val);
            }
        }

        public void Code(Type t, ref ICountableDictSet dictSet)
        {
            long count = CodeCountLong(ref dictSet, v => v.LongCount);
            if (count < 1L) return;
            var keyType = dictSet.KeyType;
            foreach (var obj in dictSet.Objects)
            {
                var key = obj;
                TypeCoder.Write(this, keyType, ref key);
            }
        }

        public void Code(Type t, ref IArrayVector value)
        {
            var array = value.Array; Code(value.ArrayType, ref array);
            var origin = value.Origin; CodeLong(ref origin);
            var length = value.Size; CodeLong(ref length);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void Code(Type t, ref IArrayMatrix value)
        {
            var array = value.Array; Code(value.ArrayType, ref array);
            var origin = value.Origin; CodeLong(ref origin);
            var length = value.Size; CodeV2l(ref length);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void Code(Type t, ref IArrayVolume value)
        {
            var array = value.Array; Code(value.ArrayType, ref array);
            var origin = value.Origin; CodeLong(ref origin);
            var length = value.Size; CodeV3l(ref length);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void Code(Type t, ref IArrayTensor4 value)
        {
            var array = value.Array; Code(value.ArrayType, ref array);
            var origin = value.Origin; CodeLong(ref origin);
            var length = value.Size; CodeV4l(ref length);
            var delta = value.Delta; CodeV4l(ref delta);
        }

        public void Code(Type t, ref IArrayTensorN value)
        {
            var array = value.Array; Code(value.ArrayType, ref array);
            var origin = value.Origin; CodeLong(ref origin);
            var length = value.Size; CodeLongArray(ref length);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeHashSet(Type t, ref object obj)
        {
            throw new NotImplementedException("only possible whith a non-generic ISet interface)");
        }

        public void CodeEnum(Type type, ref object value)
        {
            Type systemType = Enum.GetUnderlyingType(type);
            var writer = TypeCoder.TypeWriterMap[systemType];
            writer(this, value);
        }

        #endregion

        #region Code Primitives

        public void CodeBool(ref bool value) { m_writer.Write(value); }
        public void CodeByte(ref byte value) { m_writer.Write(value); }
        public void CodeSByte(ref sbyte value) { m_writer.Write(value); }
        public void CodeShort(ref short value) { m_writer.Write(value); }
        public void CodeUShort(ref ushort value) { m_writer.Write(value); }
        public void CodeInt(ref int value) { m_writer.Write(value); }
        public void CodeUInt(ref uint value) { m_writer.Write(value); }
        public void CodeLong(ref long value) { m_writer.Write(value); }
        public void CodeULong(ref ulong value)  { m_writer.Write(value); }
        public void CodeFloat(ref float value) { m_writer.Write(value); }
        public void CodeDouble(ref double value) { m_writer.Write(value); }
        public void CodeChar(ref char value) { m_writer.Write(value); }

        public void CodeString(ref string value)
        {
            if (CodeNull(ref value)) return;
            m_writer.Write(value);
        }

        public void CodeType(ref Type value)
        {
            string typeName;
            TypeInfo ti;

            if (TryGetTypeInfo(value, out ti))
                typeName = ti.Name;
            else
                typeName = value.AssemblyQualifiedName;

            m_writer.Write(typeName);
        }

        public void CodeGuid(ref Guid value) { m_writer.Write(value); }

        public void CodeSymbol(ref Symbol value) { m_writer.Write(value); }

        public void CodeGuidSymbol(ref Symbol value) { m_writer.WriteGuidSymbol(value); }

        public void CodePositiveSymbol(ref Symbol value) { m_writer.WritePositiveSymbol(value); }

        public void CodeIntSet(ref IntSet set)
        {
            int count = CodeCount(ref set, v => v.Count);
            if (count < 1) return;
            foreach (var item in set.Keys) m_writer.Write(item);
        }

        public void CodeSymbolSet(ref SymbolSet set)
        {
            int count = CodeCount(ref set, v => v.Count);
            if (count < 1) return;
            foreach (var item in set.Keys) m_writer.Write(item);
        }

        public void CodeFraction(ref Fraction value)
        {
            m_writer.Write(value.Numerator);
            m_writer.Write(value.Denominator);
        }

        public void CodeCircle2d(ref Circle2d v)
        {
            CodeV2d(ref v.Center); CodeDouble(ref v.Radius);
        }

        public void CodeLine2d(ref Line2d v)
        {
            CodeV2d(ref v.P0); CodeV2d(ref v.P1);
        }

        public void CodeLine3d(ref Line3d v)
        {
            CodeV3d(ref v.P0); CodeV3d(ref v.P1);
        }

        public void CodePlane2d(ref Plane2d v)
        {
            CodeV2d(ref v.Normal); CodeDouble(ref v.Distance);
        }

        public void CodePlane3d(ref Plane3d v)
        {
            CodeV3d(ref v.Normal); CodeDouble(ref v.Distance);
        }

        public void CodePlaneWithPoint3d(ref PlaneWithPoint3d v)
        {
            CodeV3d(ref v.Normal); CodeV3d(ref v.Point);
        }

        public void CodeQuad2d(ref Quad2d v)
        {
            CodeV2d(ref v.P0); CodeV2d(ref v.P1); CodeV2d(ref v.P2); CodeV2d(ref v.P3);
        }

        public void CodeQuad3d(ref Quad3d v)
        {
            CodeV3d(ref v.P0); CodeV3d(ref v.P1); CodeV3d(ref v.P2); CodeV3d(ref v.P3);
        }

        public void CodeRay2d(ref Ray2d v)
        {
            CodeV2d(ref v.Origin); CodeV2d(ref v.Direction);
        }

        public void CodeRay3d(ref Ray3d v)
        {
            CodeV3d(ref v.Origin); CodeV3d(ref v.Direction);
        }

        public void CodeSphere3d(ref Sphere3d v)
        {
            CodeV3d(ref v.Center); CodeDouble(ref v.Radius);
        }

        public void CodeTriangle2d(ref Triangle2d v)
        {
            CodeV2d(ref v.P0); CodeV2d(ref v.P1); CodeV2d(ref v.P2);
        }

        public void CodeTriangle3d(ref Triangle3d v)
        {
            CodeV3d(ref v.P0); CodeV3d(ref v.P1); CodeV3d(ref v.P2);
        }

        public void CodeCameraExtrinsics(ref CameraExtrinsics v)
        {
            var rotation = v.Rotation; var translation = v.Translation;
            CodeM33d(ref rotation); CodeV3d(ref translation);
        }

        public void CodeCameraIntrinsics(ref CameraIntrinsics v)
        {
            var focalLength = v.FocalLength; var principalPoint = v.PrincipalPoint;
            double skew = v.Skew, k1 = v.K1, k2 = v.K2, k3 = v.K3, p1 = v.P1, p2 = v.P2;
            CodeV2d(ref focalLength); CodeV2d(ref principalPoint);
            CodeDouble(ref skew);
            CodeDouble(ref k1); CodeDouble(ref k2); CodeDouble(ref k3);
            CodeDouble(ref p1); CodeDouble(ref p1);
        }

        #endregion

        #region Code Arrays

        public void CodeStructArray<T>(ref T[] value)
            where T: struct
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBoolArray(ref bool[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) m_writer.Write(value[i]);
        }

        public void CodeByteArray(ref byte[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeSByteArray(ref sbyte[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeShortArray(ref short[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeUShortArray(ref ushort[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeIntArray(ref int[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeUIntArray(ref uint[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeLongArray(ref long[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeULongArray(ref ulong[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeFloatArray(ref float[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeDoubleArray(ref double[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeCharArray(ref char[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) m_writer.Write(value[i]);
        }

        public void CodeStringArray(ref string[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) m_writer.Write(value[i]);
        }

        public void CodeTypeArray(ref Type[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;

            for (long i = 0; i < count; i++)
            {
                string typeName;
                TypeInfo ti;

                if (TryGetTypeInfo(value[i], out ti))
                    typeName = ti.Name;
                else
                    typeName = value[i].AssemblyQualifiedName;

                m_writer.Write(typeName);
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
            m_writer.WriteArray(value, 0, count);
        }

        #endregion

        #region Code Lists

        public void CodeStructList<T>(ref List<T> value)
            where T : struct
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Bool_(ref List<bool> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) m_writer.Write(value[i]);
        }

        public void CodeList_of_Byte_(ref List<byte> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) m_writer.Write(value[i]);
        }

        public void CodeList_of_SByte_(ref List<sbyte> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Short_(ref List<short> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_UShort_(ref List<ushort> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Int_(ref List<int> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_UInt_(ref List<uint> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Long_(ref List<long> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_ULong_(ref List<ulong> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Float_(ref List<float> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Double_(ref List<double> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Char_(ref List<char> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) m_writer.Write(value[i]);
        }

        public void CodeList_of_String_(ref List<string> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) m_writer.Write(value[i]);
        }

        public void CodeList_of_Type_(ref List<Type> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;

            for (int i = 0; i < count; i++)
            {
                string typeName;
                TypeInfo ti;

                if (TryGetTypeInfo(value[i], out ti))
                    typeName = ti.Name;
                else
                    typeName = value[i].AssemblyQualifiedName;

                m_writer.Write(typeName);
            }
        }

        public void CodeList_of_Guid_(ref List<Guid> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeGuid(ref x); }
        }

        public void CodeList_of_Symbol_(ref List<Symbol> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeSymbol(ref x); }
        }

        public void CodeList_of_Fraction_(ref List<Fraction> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            var position = m_writer.BaseStream.Position;

            m_size = position - m_startPosition;
            m_writer.BaseStream.Position = m_sizePosition;
            m_writer.Write(m_size);
            m_writer.BaseStream.Position = position;

            if (m_disposeStream)
                m_writer.BaseStream.Dispose();
            else
                m_writer.BaseStream.Flush();
        }

        #endregion    

    }

}

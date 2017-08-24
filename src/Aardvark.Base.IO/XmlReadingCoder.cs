using Aardvark.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Aardvark.VRVis
{

    public partial class NewXmlReadingCoder
        : BaseReadingCoder, IReadingCoder, IDisposable
    {
        private Stack<State> m_stateStack;
        private State m_state;
        private int m_coderVersion;
        private Stream m_stream;
        private bool m_disposeStream;
        private string m_fileName;

        #region Constructors

        public NewXmlReadingCoder(XmlDoc doc)
            : base()
        {
            m_stateStack = new Stack<State>();
            m_state.Item = doc.FirstSub<XmlItem>();
            if (m_state.Item == null || m_state.Item.Name != AardvarkSymbol)
                throw new InvalidOperationException("could not read file as xml");
            m_state.Index = 0;
            m_coderVersion = m_state.Item.Attribute(VersionSymbol).ParseInt();
            if (m_coderVersion < 0
                || m_coderVersion > c_coderVersion)
                throw new InvalidOperationException(
                        "attempted read on an Aardvark file with an "
                        + "unsupported coder version");
            m_disposeStream = false;
        }

        private static XmlDoc XmlFromStream(Stream stream)
        {
            XmlDoc root = new XmlDoc();
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                new XmlParser().Parse(reader.ReadToEnd(), root);
            }
            return root;
        }

        public NewXmlReadingCoder(Stream stream)
            : this(XmlFromStream(stream))
        {
            m_stream = stream;
        }

        public NewXmlReadingCoder(string fileName)
            : this(File.Open(fileName, FileMode.Open))
        {
            m_fileName = fileName;
            m_disposeStream = true;
        }

        #endregion

        private struct State
        {
            public XmlItem Item;
            public int Index;

            public State(XmlItem node, int index)
            {
                Item = node; Index = index;
            }

            public bool Valid { get { return Index >= 0; } }

            public void Advance()
            {
                Index = Item.NextSubIndex<XmlItem>(Index);
            }

            public XmlItem Current
            {
                get { return Item[Index] as XmlItem; }
            }
        }

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
            return TypeInfo.OfXmlName.TryGetValue(name, out ti);
        }

        public void Code(ref object obj)
        {
            // if (m_state.Enumerator.Current == null) return;

            var typeName = m_state.Current.Name.ToString();
            TypeInfo newTypeInfo = null;
            TypeInfo typeInfo;

            if (TryGetTypeInfo(typeName, out typeInfo))
            {
                if (typeInfo.Type == typeof(TypeCoder.Null))
                {
                    obj = null;
                    m_state.Advance();
                    return;
                }
                if (typeInfo.Type == typeof(TypeCoder.Reference))
                {
                    if ((typeInfo.Options & TypeInfo.Option.Active) != 0)
                    {
                        int num = m_state.Current.Attribute(NumSymbol).ParseInt();
                        obj = m_refs[num];
                        m_state.Advance();
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
                if (typeName == "object")
                {
                    typeName = m_state.Current.Attribute(TypeSymbol).ToString();
                    if (!TypeInfo.TryGetOfFullName(typeName, out typeInfo))
                        typeInfo = new TypeInfo(typeName, Type.GetType(typeName),
                                    TypeInfo.Option.Size | TypeInfo.Option.Version);
                    if (typeInfo.Type == null)
                    {
                        Report.Warn("skipping unkown object of type \""
                                    + typeName + '"');
                    }
                    else
                    {
                        if ((m_debugMode & CoderDebugMode.ReportQualifiedNames) != 0)
                            Report.Line("qualified name \"{0}\"", typeName);
                    }
                }
                else
                {
                    if (typeName == "null")
                        throw new Exception("cannot decode null object "
                            + "- change by configuring coder with "
                            + "\"coder.Add(TypeCoder.Default.Null);\"");
                    Report.Warn("skipping unknown object \"" + typeName + '"');
                    typeInfo = new TypeInfo(typeName, null);
                }
            }

            if ((typeInfo.Options & TypeInfo.Option.Version) != 0)
            {
                m_versionStack.Push(m_version);
                var attribute = m_state.Current.Attribute(VersionSymbol);
                m_version = attribute != default(Text) ? attribute.ParseInt() : 0;
                if (m_version < typeInfo.Version)
                {
                    TypeInfo oldTypeInfo;
                    if (typeInfo.VersionMap.TryGetValue(m_version, out oldTypeInfo))
                    {
                        newTypeInfo = typeInfo; typeInfo = oldTypeInfo;
                    }
                }
            }

            m_stateStack.Push(m_state);
            m_state = new State(m_state.Current, 0);

            if (typeInfo.Type != null)
            {
                if (!TypeCoder.ReadPrimitive(this, typeInfo.Type, ref obj))
                {
                    if ((typeInfo.Options & TypeInfo.Option.Ignore) != 0)
                    {
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

                        if (m_doRefs) m_refs.Add(obj);

                        var fcobj = obj as IFieldCodeable;
                        if (fcobj != null)
                        {
                            CodeFields(typeInfo.Type, codedVersion, fcobj);
                            var tmobj = obj as ITypedMap;
                            if (tmobj != null) CodeFields(typeInfo.Type, tmobj);
                        }
                        else
                        {
                            Report.Warn("skipping uncodeable object of type \""
                                        + typeInfo.Name + '"');
                        }

                        var aobj = obj as IAwakeable;
                        if (aobj != null) aobj.Awake(codedVersion);

                        m_typeInfoStack.Pop();
                    }
                }
            }

            m_state = m_stateStack.Pop();

            if ((typeInfo.Options & TypeInfo.Option.Version) != 0)
            {
                m_version = m_versionStack.Pop();
                if (obj != null && newTypeInfo != null)
                {
                    var source = new Convertible(typeInfo.Name, obj);
                    var target = new Convertible(newTypeInfo.Name, null);
                    source.ConvertInto(target);
                    obj = target.Data;
                }
            }
            m_state.Advance();
        }

        private void CodeFields(Type type, int version, IFieldCodeable obj)
        {
            FieldCoderMap fcm = FieldCoderMap.Get(m_coderVersion, type, version, obj);
            while (m_state.Valid)
            {
                string fieldName = m_state.Current.Name.ToString();

                Action<ICoder, object> code;

                if (fcm.TryGetValue(fieldName, out code))
                {
                    m_stateStack.Push(m_state);
                    m_state = new State(m_state.Current, 0);
                    code(this, obj);
                    m_state = m_stateStack.Pop();
                }
                else
                {
                    if (obj is ITypedMap) return;
                    Report.Warn("skipping unknown field \""
                                + fieldName + '"');
                }
                m_state.Advance();
            }
        }

        private void CodeFields(Type type, ITypedMap obj)
        {
            TypeOfString fieldTypeMap = FieldTypeMap.Get(type, obj);

            while (m_state.Valid)
            {
                string fieldName = m_state.Current.Name.ToString();
                Type fieldType;
                string fieldTypeName = null;
                TypeInfo typeInfo;

                if (fieldTypeMap.TryGetValue(fieldName, out fieldType))
                {
                    if (TryGetTypeInfo(fieldName, out typeInfo))
                        fieldTypeName = typeInfo.Name;
                }
                else
                {
                    fieldTypeName = m_state.Current.Attribute(TypeSymbol).ToString();
                    if (TryGetTypeInfo(fieldTypeName, out typeInfo))
                        fieldType = typeInfo.Type;
                    else
                    {
                        fieldType = Type.GetType(fieldTypeName);
                        if (fieldName == "object")
                        {
                            fieldTypeName = fieldType.Name;
                            fieldName = fieldType.Name;
                        }
                    }
                }

                if (fieldTypeName != fieldName || TypeCoder.IsDirectlyCodeable(fieldType))
                {
                    m_stateStack.Push(m_state);
                    m_state = new State(m_state.Current, 0);
                }
                object value = null;
                TypeCoder.Read(this, fieldType, ref value);
                obj[fieldName] = value;
                if (fieldTypeName != fieldName || TypeCoder.IsDirectlyCodeable(fieldType))
                {
                    m_state = m_stateStack.Pop();
                    m_state.Advance();
                }
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

        private bool CodeNull<T>(ref T value) where T : class
        {
            if (m_coderVersion > 0)
            {
                XmlItem item;
                if (m_state.Current.SubList.Count == 1
                    && (item = (m_state.Current[0] as XmlItem)) != null
                    && item.Name == NullSymbol)
                {
                    value = null;
                    return true;
                }
            }
            return false;
        }

        public int CodeCount<T>(ref T value, Func<int, T> creator) where T : class
        {
            if (m_coderVersion < 2)
            {
                var text = m_state.Current.FirstSub<XmlDirectText>();
                if (text != null && text.Text == "null")
                {
                    value = null; return -1;
                }
            }
            var item = m_state.Current.FirstSub<XmlItem>();
            if (item != null)
            {
                if (item.Name == NullSymbol)
                {
                    value = null; return -1;
                }
                if (item.Name == RefSymbol)
                {
                    int refNum = item.Attribute(NumSymbol).ParseInt();
                    if (refNum >= m_refs.Count)
                    {
                        Report.Warn("skipping invalid reference to " + refNum);
                        value = null;
                    }
                    else
                        value = (T)m_refs[refNum];
                    return -2;
                }
            }

            int count = 0;
            Text attribute = m_state.Current.Attribute(CountSymbol);
            if (attribute != default(Text))
                count = attribute.ParseInt();
            value = creator(count);
            if (m_doRefs) m_refs.Add(value);
            return count;
        }

        private int CodeCount<T>(ref T[] array)
        {
            return CodeCount(ref array, n => new T[n]);
        }

        private int CodeCount<T>(ref List<T> list)
        {
            return CodeCount(ref list, n => new List<T>(n));
        }

        public void CodeTArray<T>(ref T[] value)
        {
            int count = CodeCount(ref value, n => new T[n]);
            if (count < 1) return;
            int i = 0;
            while (m_state.Valid)
            {
                object o = default(T); Code(ref o); value[i++] = (T)o;
            }
        }

        public void CodeList_of_T_<T>(ref List<T> value)
        {
            int count = CodeCount(ref value, n => new List<T>(n));
            if (count < 1) return;
            while (m_state.Valid)
            {
                object o = default(T); Code(ref o); value.Add((T)o);
            }
        }

        public void CodeHashSet_of_T_<T>(ref HashSet<T> value)
        {
            int count = CodeCount(ref value, n => new HashSet<T>());
            if (count < 1) return;
            while (m_state.Valid)
            {
                object o = default(T); Code(ref o); value.Add((T)o);
            }
        }

        public void Code(Type t, ref object o)
        {
            TypeCoder.Read(this, t, ref o);
        }

        public void Code(Type t, ref Array value)
        {
            int count = CodeCount(ref value, n => Array.CreateInstance(t, n));
            if (count < 1) return;
            int i = 0;
            while (m_state.Valid)
            {
                object o = null; Code(ref o); value.SetValue(o, i++);
            }
        }

        public void Code(Type t, ref IList value)
        {
            int count = CodeCount(ref value,
                                  n => (IList)Activator.CreateInstance(t, n));
            if (count < 1) return;
            while (m_state.Valid)
            {
                object o = null; Code(ref o); value.Add(o);
            }
        }

        public void Code(Type t, ref IDictionary dict)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref ICountableDict dict)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref ICountableDictSet dict)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref IArrayVector value)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref IArrayMatrix value)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref IArrayVolume value)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref IArrayTensor4 value)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref IArrayTensorN value)
        {
            throw new NotImplementedException();
        }

        public void CodeHashSet(Type t, ref object o)
        {
            throw new NotImplementedException("only possible whith a non-generic ISet interface)");
        }

        public void CodeEnum(Type t, ref object value)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region Code Primitives

        private Text CurrentText()
        {
            return m_state.Current.FirstSub<XmlDirectText>().Text;
        }

        public void CodeBool(ref bool v) { throw new NotImplementedException(); }
        public void CodeByte(ref byte v) { v = CurrentText().ParseByte(); }
        public void CodeSByte(ref sbyte v) { v = CurrentText().ParseSByte(); }
        public void CodeShort(ref short v) { v = CurrentText().ParseShort(); }
        public void CodeUShort(ref ushort v) { v = CurrentText().ParseUShort(); }
        public void CodeInt(ref int v) { v = CurrentText().ParseInt(); }
        public void CodeUInt(ref uint v) { v = CurrentText().ParseUInt(); }
        public void CodeLong(ref long v) { v = CurrentText().ParseLong(); }
        public void CodeULong(ref ulong v) { v = CurrentText().ParseULong(); }
        public void CodeFloat(ref float v) { v = CurrentText().ParseFloat(); }
        public void CodeDouble(ref double v) { v = CurrentText().ParseDouble(); }

        public void CodeChar(ref char v) { throw new NotImplementedException(); }

        public void CodeString(ref string v) { v = CurrentText().ToString(); }

        public void CodeType(ref Type value)
        {
            throw new NotImplementedException();
        }

        public void CodeGuid(ref Guid v) { throw new NotImplementedException(); }

        public void CodeSymbol(ref Symbol v) { throw new NotImplementedException(); }

        public void CodeGuidSymbol(ref Symbol v) { throw new NotImplementedException(); }

        public void CodePositiveSymbol(ref Symbol v) { throw new NotImplementedException(); }

        public void CodeIntSet(ref IntSet set) { throw new NotImplementedException(); }

        public void CodeSymbolSet(ref SymbolSet set) { throw new NotImplementedException(); }

        public void CodeFraction(ref Fraction v) { throw new NotImplementedException(); }

        public void CodeStructArray<T>(ref T[] a) where T : struct
        {
            throw new NotImplementedException();
        }

        public void CodeStructList<T>(ref List<T> l) where T : struct
        {
            throw new NotImplementedException();
        }

        internal void CodeArray<T>(
                ref T[] value, Func<string, IEnumerable<string>> splitter,
                Func<string, T> creator)
        {
            throw new NotImplementedException();
        }

        internal void CodeList<T>(ref List<T> value,
                Func<string, IEnumerable<string>> splitter, Func<string, T> creator)
        {
            throw new NotImplementedException();
        }

        internal void CodeArray<T>(ref T[] v, Func<string, T> fromString)
        {
            throw new NotImplementedException();
        }

        internal void CodeList<T>(ref List<T> v, Func<string, T> fromString)
        {
            throw new NotImplementedException();
        }

        public void CodeBoolArray(ref bool[] v) { throw new NotImplementedException(); }
        public void CodeByteArray(ref byte[] v) { throw new NotImplementedException(); }
        public void CodeSByteArray(ref sbyte[] v) { throw new NotImplementedException(); }
        public void CodeShortArray(ref short[] v) { throw new NotImplementedException(); }
        public void CodeUShortArray(ref ushort[] v) { throw new NotImplementedException(); }
        public void CodeIntArray(ref int[] v) { throw new NotImplementedException(); }
        public void CodeUIntArray(ref uint[] v) { throw new NotImplementedException(); }
        public void CodeLongArray(ref long[] v) { throw new NotImplementedException(); }
        public void CodeULongArray(ref ulong[] v) { throw new NotImplementedException(); }

        public void CodeFloatArray(ref float[] v)
        {
            throw new NotImplementedException();
        }
        public void CodeDoubleArray(ref double[] v)
        {
            throw new NotImplementedException();
        }

        // private static readonly char[] s_charSeparators = new[] { ',', ' ' };

        public void CodeCharArray(ref char[] v)
        {
            throw new NotImplementedException();
        }

        public void CodeStringArray(ref string[] value)
        {
            throw new NotImplementedException();
        }

        public void CodeTypeArray(ref Type[] value)
        {
            throw new NotImplementedException();
        }

        public void CodeGuidArray(ref Guid[] v) { throw new NotImplementedException(); }

        public void CodeSymbolArray(ref Symbol[] v) { throw new NotImplementedException(); }

        public void CodeFractionArray(ref Fraction[] v) { throw new NotImplementedException(); }

        public void Code(ref List<object> value)
        {
            throw new NotImplementedException();
        }

        public void CodeList_of_Bool_(ref List<bool> v) { throw new NotImplementedException(); }
        public void CodeList_of_Byte_(ref List<byte> v) { throw new NotImplementedException(); }
        public void CodeList_of_SByte_(ref List<sbyte> v) { throw new NotImplementedException(); }
        public void CodeList_of_Short_(ref List<short> v) { throw new NotImplementedException(); }
        public void CodeList_of_UShort_(ref List<ushort> v) { throw new NotImplementedException(); }
        public void CodeList_of_Int_(ref List<int> v) { throw new NotImplementedException(); }
        public void CodeList_of_UInt_(ref List<uint> v) { throw new NotImplementedException(); }
        public void CodeList_of_Long_(ref List<long> v) { throw new NotImplementedException(); }
        public void CodeList_of_ULong_(ref List<ulong> v) { throw new NotImplementedException(); }

        public void CodeList_of_Float_(ref List<float> v)
        {
            throw new NotImplementedException();
        }

        public void CodeList_of_Double_(ref List<double> v)
        {
            throw new NotImplementedException();
        }

        public void CodeList_of_Char_(ref List<char> v)
        {
            throw new NotImplementedException();
        }

        public void CodeList_of_String_(ref List<string> value)
        {
            throw new NotImplementedException();
        }

        public void CodeList_of_Type_(ref List<Type> value)
        {
            throw new NotImplementedException();
        }

        public void CodeList_of_Guid_(ref List<Guid> v) { throw new NotImplementedException(); }
        public void CodeList_of_Symbol_(ref List<Symbol> v) { throw new NotImplementedException(); }
        public void CodeList_of_Fraction_(ref List<Fraction> v) { throw new NotImplementedException(); }

        public void CodeCircle2d(ref Circle2d v) { throw new NotImplementedException(); }
        public void CodeLine2d(ref Line2d v) { throw new NotImplementedException(); }
        public void CodeLine3d(ref Line3d v) { throw new NotImplementedException(); }
        public void CodePlane2d(ref Plane2d v) { throw new NotImplementedException(); }
        public void CodePlane3d(ref Plane3d v) { throw new NotImplementedException(); }
        public void CodePlaneWithPoint3d(ref PlaneWithPoint3d v) { throw new NotImplementedException(); }
        public void CodeQuad2d(ref Quad2d v) { throw new NotImplementedException(); }
        public void CodeQuad3d(ref Quad3d v) { throw new NotImplementedException(); }
        public void CodeRay2d(ref Ray2d v) { throw new NotImplementedException(); }
        public void CodeRay3d(ref Ray3d v) { throw new NotImplementedException(); }
        public void CodeSphere3d(ref Sphere3d v) { throw new NotImplementedException(); }
        public void CodeTriangle2d(ref Triangle2d v) { throw new NotImplementedException(); }
        public void CodeTriangle3d(ref Triangle3d v) { throw new NotImplementedException(); }

        public void CodeCameraExtrinsics(ref CameraExtrinsics v)
        {
            throw new NotSupportedException("cannot serialize single camera");
        }

        public void CodeCameraIntrinsics(ref CameraIntrinsics v)
        {
            throw new NotSupportedException("cannot serialize single camera");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (m_disposeStream)
                m_stream.Dispose();
        }

        #endregion    
    }

    public partial class XmlReadingCoder
        : BaseReadingCoder, IReadingCoder, IDisposable
    {
        private Stack<State> m_stateStack;
        private State m_state;
        private Stream m_stream;
        private string m_fileName;
        private readonly int m_coderVersion;
        private bool m_disposeStream = false;

        #region Constructors

        public XmlReadingCoder(XElement container) : base()
        {
            m_stateStack = new Stack<State>();
            m_state.Element = container;
            m_state.Enumerator = container.Elements().GetEnumerator();
            m_fileName = null;
            m_stream = null;
            XAttribute attribute = container.Attribute("version");
            m_coderVersion = attribute != null ? int.Parse(attribute.Value) : 0;
            if (m_coderVersion < 0
                || m_coderVersion > c_coderVersion)
                throw new InvalidOperationException(
                        "attempted read on an Aardvark file with an "
                        + "unsupported coder version");
        }

        private static XElement XElementFromStream(Stream stream)
        {
            XElement root;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                root = XElement.Load(reader, LoadOptions.PreserveWhitespace);
            return root;
        }

        public XmlReadingCoder(Stream stream)
            : this(XElementFromStream(stream))
        {
            m_stream = stream;
        }

        public XmlReadingCoder(string fileName)
            : this(File.Open(fileName, FileMode.Open))
        {
            m_fileName = fileName;
            m_disposeStream = true;
        }

        #endregion

        #region Private Data Structure

        struct State
        {
            IEnumerator<XElement> m_enumerator;
            XElement m_element;
            bool m_valid;

            public bool Valid
            {
                get { return m_valid; }
                set { m_valid = value; }
            }
            public XElement Element
            {
                get { return m_element; }
                set { m_element = value; }
            }
            public IEnumerator<XElement> Enumerator
            {
                get { return m_enumerator; }
                set
                {
                    m_enumerator = value;
                    m_valid = m_enumerator.MoveNext();
                }
            }
            public void Advance()
            {
                m_valid = m_enumerator.MoveNext();
            }
        }

        #endregion

        #region Properties

        public XElement ContainerElement
        {
            get { return m_state.Element; }
        }

        #endregion

        #region Static Convenience Functions

        public static object ReadFirstObject(string fileName)
        {
            object first = null;
            using (var coder = new XmlReadingCoder(fileName))
                coder.Code(ref first);
            return first;
        }

        public static object ReadFirstObject(Stream stream)
        {
            object first = null;
            using (var coder = new XmlReadingCoder(stream))
                coder.Code(ref first);
            return first;
        }

        public static T ReadFirst<T>(string fileName)
        {
            T first = default(T);
            using (var coder = new XmlReadingCoder(fileName))
                coder.CodeT(ref first);
            return first;
        }

        public static T ReadFirst<T>(Stream stream)
        {
            T first = default(T);
            using (var coder = new XmlReadingCoder(stream))
                coder.CodeT(ref first);
            return first;
        }

        #endregion

        #region Private Convenience Functions

        private string CurrentValue()
        {
            return m_state.Element.Value;
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
            return TypeInfo.OfXmlName.TryGetValue(name, out ti);
        }

        public void Code(ref object obj)
        {
            if (m_state.Enumerator.Current == null) return;

            m_state.Element = m_state.Enumerator.Current;
            var typeName = m_state.Element.Name.ToString();
            TypeInfo newTypeInfo = null;
            TypeInfo typeInfo;

            if (TryGetTypeInfo(typeName, out typeInfo))
            {
                if (typeInfo.Type == typeof(TypeCoder.Null))
                {
                    obj = null;
                    m_state.Advance();
                    return;
                }
                if (typeInfo.Type == typeof(TypeCoder.Reference))
                {
                    if ((typeInfo.Options & TypeInfo.Option.Active) != 0)
                    {
                        int num = m_state.Element.Attribute("num").Value.ToInt();
                        obj = m_refs[num];
                        m_state.Advance();
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
                if (typeName == "object")
                {
                    typeName = m_state.Element.Attribute("type").Value;
                    if (!TypeInfo.TryGetOfFullName(typeName, out typeInfo))
                        typeInfo = new TypeInfo(typeName, Type.GetType(typeName),
                                    TypeInfo.Option.Size | TypeInfo.Option.Version);
                    if (typeInfo.Type == null)
                    {
                        Report.Warn("skipping unkown object of type \""
                                    + typeName + '"');
                    }
                    else
                    {
                        if ((m_debugMode & CoderDebugMode.ReportQualifiedNames) != 0)
                            Report.Line("qualified name \"{0}\"", typeName);
                    }
                }
                else
                {
                    if (typeName == "null")
                        throw new Exception("cannot decode null object "
                            + "- change by configuring coder with "
                            + "\"coder.Add(TypeCoder.Default.Null);\"");
                    Report.Warn("skipping unknown object \"" + typeName + '"');
                    typeInfo = new TypeInfo(typeName, null);
                }
            }

            if ((typeInfo.Options & TypeInfo.Option.Version) != 0)
            {
                m_versionStack.Push(m_version);
                XAttribute attribute = m_state.Element.Attribute("version");
                m_version = attribute != null ? int.Parse(attribute.Value) : 0;
                if (m_version < typeInfo.Version)
                {
                    TypeInfo oldTypeInfo;
                    if (typeInfo.VersionMap.TryGetValue(m_version, out oldTypeInfo))
                    {
                        newTypeInfo = typeInfo; typeInfo = oldTypeInfo;
                    }
                }
            }

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();

            if (typeInfo.Type != null)
            {
                if (! TypeCoder.ReadPrimitive(this, typeInfo.Type, ref obj))
                {
                    if ((typeInfo.Options & TypeInfo.Option.Ignore) != 0)
                    {
                        obj = null;
                    }
                    else
                    {
                        int codedVersion = m_version;

                        m_typeInfoStack.Push(typeInfo);

                        if (typeInfo.Creator != null)
                            obj = typeInfo.Creator();
                        else
                            obj = FastObjectFactory.ObjectFactory(typeInfo.Type)();

                        if (m_doRefs) m_refs.Add(obj);

                        var fcobj = obj as IFieldCodeable;
                        if (fcobj != null)
                        {
                            CodeFields(typeInfo.Type, m_version, fcobj);
                            var tmobj = obj as ITypedMap;
                            if (tmobj != null) CodeFields(typeInfo.Type, tmobj);
                        }
                        else
                        {
                            Report.Warn("skipping uncodeable object of type \""
                                        + typeInfo.Name + '"');
                        }

                        var aobj = obj as IAwakeable;
                        if (aobj != null) aobj.Awake(codedVersion);

                        m_typeInfoStack.Pop();
                    }
                }
            }

            m_state = m_stateStack.Pop();

            if ((typeInfo.Options & TypeInfo.Option.Version) != 0)
            {
                m_version = m_versionStack.Pop();
                if (obj != null && newTypeInfo != null)
                {
                    var source = new Convertible(typeInfo.Name, obj);
                    var target = new Convertible(newTypeInfo.Name, null);
                    source.ConvertInto(target);
                    obj = target.Data;
                }
            }
            m_state.Advance();
        }

        private void CodeFields(Type type, int version, IFieldCodeable obj)
        {
            FieldCoderMap fcm = FieldCoderMap.Get(m_coderVersion, type, version, obj);
            while (m_state.Valid)
            {
                m_state.Element = m_state.Enumerator.Current;
                string fieldName = m_state.Element.Name.ToString();

                Action<ICoder, object> code;

                if (fcm.TryGetValue(fieldName, out code))
                {
                    m_stateStack.Push(m_state);
                    m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
                    code(this, obj);
                    m_state = m_stateStack.Pop();
                }
                else
                {
                    if (obj is ITypedMap) return;
                    Report.Warn("skipping unknown field \""
                                + fieldName + '"');
                }
                m_state.Advance();
            }
        }

        private void CodeFields(Type type, ITypedMap obj)
        {
            TypeOfString fieldTypeMap = FieldTypeMap.Get(type, obj);

            while (m_state.Valid)
            {
                m_state.Element = m_state.Enumerator.Current;
                string fieldName = m_state.Element.Name.ToString();
                Type fieldType;
                string fieldTypeName = null;
                TypeInfo typeInfo;

                if (fieldTypeMap.TryGetValue(fieldName, out fieldType))
                {
                    if (TryGetTypeInfo(fieldName, out typeInfo))
                        fieldTypeName = typeInfo.Name;
                }
                else
                {
                    fieldTypeName = m_state.Element.Attribute("type").Value.ToString();
                    if (TryGetTypeInfo(fieldTypeName, out typeInfo))
                        fieldType = typeInfo.Type;
                    else
                    {
                        fieldType = Type.GetType(fieldTypeName);
                        if (fieldName == "object")
                        {
                            fieldTypeName = fieldType.Name;
                            fieldName = fieldType.Name;
                        }
                    }
                }

                if (fieldTypeName != fieldName || TypeCoder.IsDirectlyCodeable(fieldType))
                {
                    m_stateStack.Push(m_state);
                    m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
                }
                object value = null;
                TypeCoder.Read(this, fieldType, ref value);
                obj[fieldName] = value;
                if (fieldTypeName != fieldName || TypeCoder.IsDirectlyCodeable(fieldType))
                {
                    m_state = m_stateStack.Pop();
                    m_state.Advance();
                }
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

        private bool CodeNull<T>(ref T value) where T : class
        {
            if (m_coderVersion > 0)
            {
                var subElements = m_state.Element.Elements();
                if (subElements.Count() == 1 && subElements.First().Name == "null")
                {
                    value = null;
                    return true;
                }
            }
            return false;
        }

        public int CodeCount<T>(ref T value, Func<int, T> creator) where T : class
        {
            if (m_coderVersion < 2)
            {
                if (CurrentValue() == "null") { value = null; return -1; }
            }
            var subElements = m_state.Element.Elements();
            if (subElements.Count() == 1)
            {
                var firstSubElement = subElements.First();
                var name = firstSubElement.Name;
                if (name == "null")
                {
                    value = null;
                    return -1;
                }
                if (name == "ref")
                {
                    int refNum = firstSubElement.Attribute("num").Value.ToInt();
                    if (refNum >= m_refs.Count)
                    {
                        Report.Warn("skipping invalid reference to " + refNum);
                        value = null;
                    }
                    else
                        value = (T)m_refs[refNum];
                    return -2;
                }
            }
            int count = 0;
            XAttribute attribute = m_state.Element.Attribute("count");
            if (attribute != null)
                count = int.Parse(attribute.Value);

            value = creator(count);
            if (m_doRefs) m_refs.Add(value);
            return count;
        }

        private int CodeCount<T>(ref T[] array)
        {
            return CodeCount(ref array, n => new T[n]);
        }

        private int CodeCount<T>(ref List<T> list)
        {
            return CodeCount(ref list, n => new List<T>(n));
        }

        public void CodeTArray<T>(ref T[] value)
        {
            int count = CodeCount(ref value, n => new T[n]);
            if (count < 1) return;
            int i = 0;
            while (m_state.Valid)
            {
                object o = default(T); Code(ref o); value[i++] = (T)o;
            }
        }

        public void CodeList_of_T_<T>(ref List<T> value)
        {
            int count = CodeCount(ref value, n => new List<T>(n));
            if (count < 1) return;
            while (m_state.Valid)
            {
                object o = default(T); Code(ref o); value.Add((T)o);
            }
        }

        public void CodeHashSet_of_T_<T>(ref HashSet<T> value)
        {
            int count = CodeCount(ref value, n => new HashSet<T>());
            if (count < 1) return;
            while (m_state.Valid)
            {
                object o = default(T); Code(ref o); value.Add((T)o);
            }
        }

        public void Code(Type t, ref object o)
        {
            TypeCoder.Read(this, t, ref o);
        }

        public void Code(Type t, ref Array value)
        {
            int count = CodeCount(ref value, n => Array.CreateInstance(t, n));
            if (count < 1) return;
            int i = 0;
            while (m_state.Valid)
            {
                object o = null; Code(ref o); value.SetValue(o, i++);
            }
        }

        public void Code(Type t, ref IList value)
        {
            int count = CodeCount(ref value,
                                  n => (IList)Activator.CreateInstance(t, n));
            if (count < 1) return;
            while (m_state.Valid)
            {
                object o = null; Code(ref o);  value.Add(o);
            }
        }

        public void Code(Type t, ref IDictionary dict)
        {
            int count = CodeCount(ref dict, n => (IDictionary)Activator.CreateInstance(t));
            if (count < 1) return;

            Type[] subTypeArray = t.GetGenericArguments();

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();

            while (m_state.Valid)
            {
                m_state.Element = m_state.Enumerator.Current;

                m_stateStack.Push(m_state);
                m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
                m_state.Element = m_state.Enumerator.Current;

                object key = null;
                TypeCoder.Read(this, subTypeArray[0], ref key);
                m_state.Advance();

                m_state.Element = m_state.Enumerator.Current;
                object val = null;
                if (!TypeCoder.ReadPrimitive(this, subTypeArray[1], ref val))
                {
                    m_stateStack.Push(m_state);
                    m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
                    m_state.Element = m_state.Enumerator.Current;
                    Code(ref val);
                    m_state = m_stateStack.Pop();
                }
                m_state = m_stateStack.Pop();

                m_state.Advance();

                dict[key] = val;
            }
            m_state = m_stateStack.Pop();
        }

        public void Code(Type t, ref ICountableDict dict)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref ICountableDictSet dict)
        {
            throw new NotImplementedException();
        }

        private T CodeTensorDim<T>()
        {
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            T dim = default(T);
            CodeT(ref dim);

            m_state.Advance();
            m_state.Element = m_state.Enumerator.Current;
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();

            return dim;
        }

        public void Code(Type t, ref IArrayVector value)
        {
            value = (IArrayVector)Activator.CreateInstance(t);

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            Array array = null; Code(value.ArrayType, ref array); value.Array = array;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long origin = 0L; CodeLong(ref origin); value.Origin = origin;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long length = 0L; CodeLong(ref length); value.Size = length;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long delta = 0L; CodeLong(ref delta); value.Delta = delta;

            m_state = m_stateStack.Pop();
            m_state = m_stateStack.Pop();
        }

        public void Code(Type t, ref IArrayMatrix value)
        {
            value = (IArrayMatrix)Activator.CreateInstance(t);

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            Array array = null; Code(value.ArrayType, ref array); value.Array = array;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long origin = 0L; CodeLong(ref origin); value.Origin = origin;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            V2l length = default(V2l); CodeV2l(ref length); value.Size = length;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            V2l delta = default(V2l); CodeV2l(ref delta); value.Delta = delta;

            m_state = m_stateStack.Pop();
            m_state = m_stateStack.Pop();
        }

        public void Code(Type t, ref IArrayVolume value)
        {
            value = (IArrayVolume)Activator.CreateInstance(t);

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            Array array = null; Code(value.ArrayType, ref array); value.Array = array;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long origin = 0L; CodeLong(ref origin); value.Origin = origin;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            V3l length = default(V3l); CodeV3l(ref length); value.Size = length;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            V3l delta = default(V3l); CodeV3l(ref delta); value.Delta = delta;

            m_state = m_stateStack.Pop();
            m_state = m_stateStack.Pop();
        }

        public void Code(Type t, ref IArrayTensor4 value)
        {
            throw new NotImplementedException();
        }

        public void Code(Type t, ref IArrayTensorN value)
        {
            value = (IArrayTensorN)Activator.CreateInstance(t);

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            Array array = null; Code(value.ArrayType, ref array); value.Array = array;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long origin = 0L; CodeLong(ref origin); value.Origin = origin;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long[] length = null; CodeLongArray(ref length); value.Size = length;

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long[] delta = null; CodeLongArray(ref delta); value.Delta = delta;

            m_state = m_stateStack.Pop();
            m_state = m_stateStack.Pop();
        }

        public void CodeHashSet(Type t, ref object o)
        {
            throw new NotImplementedException("only possible whith a non-generic ISet interface)");
        }

        public void CodeEnum(Type t, ref object value)
        {
            value = Enum.Parse(t, CurrentValue(), true);
        }

        #endregion

        #region Code Primitives

        public void CodeBool(ref bool v) { v = bool.Parse(CurrentValue()); }
        public void CodeByte(ref byte v) { v = byte.Parse(CurrentValue()); }
        public void CodeSByte(ref sbyte v) { v = sbyte.Parse(CurrentValue()); }
        public void CodeShort(ref short v) { v = short.Parse(CurrentValue()); }
        public void CodeUShort(ref ushort v) { v = ushort.Parse(CurrentValue()); }
        public void CodeInt(ref int v) { v = int.Parse(CurrentValue()); }
        public void CodeUInt(ref uint v) { v = uint.Parse(CurrentValue()); }
        public void CodeLong(ref long v) { v = long.Parse(CurrentValue()); }
        public void CodeULong(ref ulong v) { v = ulong.Parse(CurrentValue()); }

        public void CodeFloat(ref float value)
        {
            value = float.Parse(CurrentValue(), CultureInfo.InvariantCulture);
        }

        public void CodeDouble(ref double value)
        {
            value = double.Parse(CurrentValue(), CultureInfo.InvariantCulture);
        }

        public void CodeChar(ref char v) { v = Char.Parse(CurrentValue()); }

        public void CodeString(ref string value)
        {
            if (CodeNull(ref value)) return;
            value = CurrentValue();
        }

        public void CodeType(ref Type value)
        {
            var typeName = CurrentValue();
            TypeInfo ti;
            if (TryGetTypeInfo(typeName, out ti))
                value = ti.Type;
            else
                value = Type.GetType(typeName);
        }

        public void CodeGuid(ref Guid v) { v = new Guid(CurrentValue()); }

        public void CodeSymbol(ref Symbol v) { throw new NotImplementedException(); }

        public void CodeGuidSymbol(ref Symbol v) { throw new NotImplementedException(); }

        public void CodePositiveSymbol(ref Symbol v) { throw new NotImplementedException(); }

        public void CodeIntSet(ref IntSet set) { throw new NotImplementedException(); }

        public void CodeSymbolSet(ref SymbolSet set) { throw new NotImplementedException(); }

        public void CodeFraction(ref Fraction v) { v = Fraction.Parse(CurrentValue()); }

        public void CodeStructArray<T>(ref T[] a) where T : struct
        {
            throw new NotImplementedException();
        }

        public void CodeStructList<T>(ref List<T> l) where T : struct
        {
            throw new NotImplementedException();
        }

        private readonly Func<string, IEnumerable<string>> s_nestedBracketSplitter =
            s => s.NestedBracketSplit(0);

        internal void CodeArray<T>(
                ref T[] value, Func<string, IEnumerable<string>> splitter,
                Func<string, T> creator)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            var items = splitter(CurrentValue());
            var v = value;
            items.ForEach((s, i) => { v[i] = creator(s); });
        }

        internal void CodeList<T>(ref List<T> value,
                Func<string, IEnumerable<string>> splitter, Func<string, T> creator)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            var items = splitter(CurrentValue());
            var v = value;
            items.ForEach(s => { v.Add(creator(s)); });
        }

        static char[] s_itemSeparator = new[] { ',' };

        internal void CodeArray<T>(ref T[] v, Func<string, T> fromString)
        {
            CodeArray(ref v, s => s.Split(s_itemSeparator), fromString);
        }

        internal void CodeList<T>(ref List<T> v, Func<string, T> fromString)
        {
            CodeList(ref v, s => s.Split(s_itemSeparator), fromString);
        }

        public void CodeBoolArray(ref bool[] v) { CodeArray(ref v, s => bool.Parse(s)); }
        public void CodeByteArray(ref byte[] v) { CodeArray(ref v, s => byte.Parse(s)); }
        public void CodeSByteArray(ref sbyte[] v) { CodeArray(ref v, s => sbyte.Parse(s)); }
        public void CodeShortArray(ref short[] v) { CodeArray(ref v, s => short.Parse(s)); }
        public void CodeUShortArray(ref ushort[] v) { CodeArray(ref v, s => ushort.Parse(s)); }
        public void CodeIntArray(ref int[] v) { CodeArray(ref v, s => int.Parse(s)); }
        public void CodeUIntArray(ref uint[] v) { CodeArray(ref v, s => uint.Parse(s)); }
        public void CodeLongArray(ref long[] v) { CodeArray(ref v, s => long.Parse(s)); }
        public void CodeULongArray(ref ulong[] v) { CodeArray(ref v, s => ulong.Parse(s)); }
        public void CodeFloatArray(ref float[] v)
        {
            CodeArray(ref v, s => float.Parse(s, CultureInfo.InvariantCulture));
        }
        public void CodeDoubleArray(ref double[] v)
        {
            CodeArray(ref v, s => double.Parse(s, CultureInfo.InvariantCulture));
        }

        private static readonly char[] s_charSeparators = new[] { ',', ' ' };

        public void CodeCharArray(ref char[] v)
        {
            CodeArray(ref v,
                 s => s.Split(s_charSeparators,
                              StringSplitOptions.RemoveEmptyEntries),
                 s => char.Parse(s));
        }

        public void CodeStringArray(ref string[] value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();

            int i = 0;
            while (m_state.Valid)
            {
                value[i++] = m_state.Enumerator.Current.Value;
                m_state.Advance();
            }
            m_state = m_stateStack.Pop();
        }

        public void CodeTypeArray(ref Type[] value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            string[] items = CurrentValue().Split(s_itemSeparator);
            for (int i = 0; i < items.Length; i++)
            {
                var typeName = items[i];
                TypeInfo ti;

                if (TryGetTypeInfo(typeName, out ti))
                    value[i] = ti.Type;
                else
                    value[i] = Type.GetType(typeName);
            }
        }

        public void CodeGuidArray(ref Guid[] v) { CodeArray(ref v, s => new Guid(s)); }

        public void CodeSymbolArray(ref Symbol[] v)
        {
            throw new NotImplementedException();
        }        
        
        public void CodeFractionArray(ref Fraction[] v) { CodeArray(ref v, s => Fraction.Parse(s)); }

        public void Code(ref List<object> value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CodeList_of_Bool_(ref List<bool> v) { CodeList(ref v, s => bool.Parse(s)); }
        public void CodeList_of_Byte_(ref List<byte> v) { CodeList(ref v, s => byte.Parse(s)); }
        public void CodeList_of_SByte_(ref List<sbyte> v) { CodeList(ref v, s => sbyte.Parse(s)); }
        public void CodeList_of_Short_(ref List<short> v) { CodeList(ref v, s => short.Parse(s)); }
        public void CodeList_of_UShort_(ref List<ushort> v) { CodeList(ref v, s => ushort.Parse(s)); }
        public void CodeList_of_Int_(ref List<int> v) { CodeList(ref v, s => int.Parse(s)); }
        public void CodeList_of_UInt_(ref List<uint> v) { CodeList(ref v, s => uint.Parse(s)); }
        public void CodeList_of_Long_(ref List<long> v) { CodeList(ref v, s => long.Parse(s)); }
        public void CodeList_of_ULong_(ref List<ulong> v) { CodeList(ref v, s => ulong.Parse(s)); }
        public void CodeList_of_Float_(ref List<float> v)
        {
            CodeList(ref v, s => float.Parse(s, CultureInfo.InvariantCulture)); 
        }
        public void CodeList_of_Double_(ref List<double> v)
        {
            CodeList(ref v, s => double.Parse(s, CultureInfo.InvariantCulture));
        }

        public void CodeList_of_Char_(ref List<char> v)
        {
            CodeList(ref v,
                 s => s.Split(s_charSeparators,
                              StringSplitOptions.RemoveEmptyEntries),
                 s => char.Parse(s));
        }

        public void CodeList_of_String_(ref List<string> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;

            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();

            while (m_state.Valid)
            {
                value.Add(m_state.Enumerator.Current.Value);
                m_state.Advance();
            }
            m_state = m_stateStack.Pop();
        }

        public void CodeList_of_Type_(ref List<Type> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            string[] items = CurrentValue().Split(s_itemSeparator);

            for (int i = 0; i < items.Length; i++)
            {
                var typeName = items[i];
                TypeInfo ti;

                if (TryGetTypeInfo(typeName, out ti))
                    value.Add(ti.Type);
                else
                    value.Add(Type.GetType(typeName));
            }
        }

        public void CodeList_of_Guid_(ref List<Guid> v) { CodeList(ref v, s => new Guid(s)); }

        public void CodeList_of_Symbol_(ref List<Symbol> v)
        {
            throw new NotImplementedException();
        }        
        
        public void CodeList_of_Fraction_(ref List<Fraction> v) { CodeList(ref v, s => Fraction.Parse(s)); }

        public void CodeCircle2d(ref Circle2d v) { v = Circle2d.Parse(CurrentValue()); }
        public void CodeLine2d(ref Line2d v) { v = Line2d.Parse(CurrentValue()); }
        public void CodeLine3d(ref Line3d v) { v = Line3d.Parse(CurrentValue()); }
        public void CodePlane2d(ref Plane2d v) { v = Plane2d.Parse(CurrentValue()); }
        public void CodePlane3d(ref Plane3d v) { v = Plane3d.Parse(CurrentValue()); }
        public void CodePlaneWithPoint3d(ref PlaneWithPoint3d v) { v = PlaneWithPoint3d.Parse(CurrentValue()); }
        public void CodeQuad2d(ref Quad2d v) { v = Quad2d.Parse(CurrentValue()); }
        public void CodeQuad3d(ref Quad3d v) { v = Quad3d.Parse(CurrentValue()); }
        public void CodeRay2d(ref Ray2d v) { v = Ray2d.Parse(CurrentValue()); }
        public void CodeRay3d(ref Ray3d v) { v = Ray3d.Parse(CurrentValue()); }
        public void CodeSphere3d(ref Sphere3d v) { v = Sphere3d.Parse(CurrentValue()); }
        public void CodeTriangle2d(ref Triangle2d v) { v = Triangle2d.Parse(CurrentValue()); }
        public void CodeTriangle3d(ref Triangle3d v) { v = Triangle3d.Parse(CurrentValue()); }

        public void CodeCameraExtrinsics(ref CameraExtrinsics v)
        {
            throw new NotSupportedException("cannot serialize single camera");
        }

        public void CodeCameraIntrinsics(ref CameraIntrinsics v)
        {
            throw new NotSupportedException("cannot serialize single camera");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (m_disposeStream)
                m_stream.Dispose();
        }

        #endregion

    }
}

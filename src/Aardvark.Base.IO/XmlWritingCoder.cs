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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Aardvark.Base.Coder;

#pragma warning disable CS9113 // Parameter is unread.
public partial class NewXmlWritingCoder(XmlDoc doc)
#pragma warning restore CS9113 // Parameter is unread.
        : BaseWritingCoder(), IWritingCoder, IDisposable
{
    public Stack<XmlItem> m_itemStack = new();
    private readonly XmlItem m_item = new(AardvarkSymbol);
    private readonly Stream m_stream = null;
    readonly bool m_disposeStream = false;
    private string m_fileName = null;

    #region Constructors

    public NewXmlWritingCoder(Stream stream)
        : this(new XmlDoc())
    {
        m_stream = stream;
    }

    public NewXmlWritingCoder(string fileName)
        : this(File.Open(fileName, FileMode.Create))
    {
        m_fileName = fileName;
        m_disposeStream = true;
    }

    #endregion

    #region ICoder Members

    public int CoderVersion { get { return c_coderVersion; } }

    public int StreamVersion { get { throw new NotImplementedException(); } }

    public int MemoryVersion { get { throw new NotImplementedException(); } }

    public string FileName
    {
        get { return m_fileName; }
        set { m_fileName = value; }
    }

    public void Code(ref object obj)
    {
        throw new NotImplementedException();
    }

    public void CodeFields(Type type, int version, IFieldCodeable obj)
    {
        throw new NotImplementedException();
    }

    public void CodeFields(Type type, ITypedMap obj)
    {
        throw new NotImplementedException();
    }

    public void CodeT<T>(ref T obj)
    {
        throw new NotImplementedException();
    }

    private bool CodeNull<T>(ref T value)
    {
        throw new NotImplementedException();
    }

    public int CodeCount<T>(ref T value, Func<T, int> counter) where T : class
    {
        throw new NotImplementedException();
    }

    private int CodeCount<T>(ref T[] array)
    {
        throw new NotImplementedException();
    }

    private int CodeCount<T>(ref List<T> list)
    {
        throw new NotImplementedException();
    }

    public void CodeTArray<T>(ref T[] value)
    {
        throw new NotImplementedException();
    }

    public void CodeList_of_T_<T>(ref List<T> value)
    {
        throw new NotImplementedException();
    }

    public void CodeHashSet_of_T_<T>(ref HashSet<T> value)
    {
        throw new NotImplementedException();
    }

    public void Code(Type t, ref object o)
    {
        throw new NotImplementedException();
    }

    public void Code(Type t, ref Array value)
    {
        throw new NotImplementedException();
    }

    public void Code(Type t, ref IList value)
    {
        throw new NotImplementedException();
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

    public void CodeBool(ref bool v) { throw new NotImplementedException(); }
    public void CodeByte(ref byte v) { throw new NotImplementedException(); }
    public void CodeSByte(ref sbyte v) { throw new NotImplementedException(); }
    public void CodeShort(ref short v) { throw new NotImplementedException(); }
    public void CodeUShort(ref ushort v) { throw new NotImplementedException(); }
    public void CodeInt(ref int v) { throw new NotImplementedException(); }
    public void CodeUInt(ref uint v) { throw new NotImplementedException(); }
    public void CodeLong(ref long v) { throw new NotImplementedException(); }
    public void CodeULong(ref ulong v) { throw new NotImplementedException(); }

    public void CodeFloat(ref float value)
    {
        throw new NotImplementedException();
    }

    public void CodeDouble(ref double value)
    {
        throw new NotImplementedException();
    }

    public void CodeChar(ref char v) { throw new NotImplementedException(); }
    public void CodeString(ref string value) { throw new NotImplementedException(); }

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

    public void CodeBoolArray(ref bool[] value) { throw new NotImplementedException(); }
    public void CodeByteArray(ref byte[] value) { throw new NotImplementedException(); }
    public void CodeSByteArray(ref sbyte[] value) { throw new NotImplementedException(); }
    public void CodeShortArray(ref short[] value) { throw new NotImplementedException(); }
    public void CodeUShortArray(ref ushort[] value) { throw new NotImplementedException(); }
    public void CodeIntArray(ref int[] value) { throw new NotImplementedException(); }
    public void CodeUIntArray(ref uint[] value) { throw new NotImplementedException(); }
    public void CodeLongArray(ref long[] value) { throw new NotImplementedException(); }
    public void CodeULongArray(ref ulong[] value) { throw new NotImplementedException(); }
    public void CodeFloatArray(ref float[] value) { throw new NotImplementedException(); }
    public void CodeDoubleArray(ref double[] value) { throw new NotImplementedException(); }

    public void CodeCharArray(ref char[] value) { throw new NotImplementedException(); }
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

    public void CodeList_of_Bool_(ref List<bool> value) { throw new NotImplementedException(); }
    public void CodeList_of_Byte_(ref List<byte> value) { throw new NotImplementedException(); }
    public void CodeList_of_SByte_(ref List<sbyte> value) { throw new NotImplementedException(); }
    public void CodeList_of_Short_(ref List<short> value) { throw new NotImplementedException(); }
    public void CodeList_of_UShort_(ref List<ushort> value) { throw new NotImplementedException(); }
    public void CodeList_of_Int_(ref List<int> value) { throw new NotImplementedException(); }
    public void CodeList_of_UInt_(ref List<uint> value) { throw new NotImplementedException(); }
    public void CodeList_of_Long_(ref List<long> value) { throw new NotImplementedException(); }
    public void CodeList_of_ULong_(ref List<ulong> value) { throw new NotImplementedException(); }
    public void CodeList_of_Float_(ref List<float> value) { throw new NotImplementedException(); }
    public void CodeList_of_Double_(ref List<double> value) { throw new NotImplementedException(); }

    public void CodeList_of_Char_(ref List<char> value) { throw new NotImplementedException(); }
    public void CodeList_of_String_(ref List<string> value)
    {
        throw new NotImplementedException();
    }

    public void CodeList_of_Type_(ref List<Type> value)
    {
        throw new NotImplementedException();
    }

    public void CodeList_of_Fraction_(ref List<Fraction> v) { throw new NotImplementedException(); }
    public void CodeList_of_Symbol_(ref List<Symbol> v) { throw new NotImplementedException(); }
    public void CodeList_of_Guid_(ref List<Guid> v) { throw new NotImplementedException(); }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        try
        {
            if (m_stream == null)
                return;

            var writer = new StreamWriter(m_stream, Encoding.UTF8);
            m_item.WriteTo(writer);

            if (m_disposeStream)
                writer.Close();
            else
                writer.Flush();
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }

    #endregion
}

public partial class XmlWritingCoder
    : BaseWritingCoder, IWritingCoder, IDisposable
{
    private readonly Stack<XElement> m_elementStack;
    protected XElement m_element;
    private readonly XElement m_container;
    private readonly Stream m_stream;
    private string m_fileName;
    bool m_writeRefNumbers;
    readonly bool m_disposeStream = false;

    #region Constructors

    public XmlWritingCoder(XElement container) : base()
    {
        m_container = container;
        m_elementStack = new Stack<XElement>();
        m_element = container;
        m_stream = null;
        m_fileName = null;
        m_writeRefNumbers = true;
        m_container.Add(new XAttribute("version", c_coderVersion));
    }

    public XmlWritingCoder(Stream stream)
        : this(new XElement("Aardvark"))
    {
        m_stream = stream;
    }

    public XmlWritingCoder(string fileName)
        : this(File.Open(fileName, FileMode.Create))
    {
        m_fileName = fileName;
        m_disposeStream = true;
    }

    #endregion

    #region Properties

    public XElement ContainerElement
    {
        set { m_element = value; }
    }

    /// <summary>
    /// By setting this debugging option to true all objects are written
    /// to the Xml file with their reference numbers as additional
    /// Xml attribute.
    /// </summary>
    public bool WriteRefNumbers
    {
        get { return m_writeRefNumbers; }
        set { m_writeRefNumbers = value; }
    }

    #endregion

    #region Static Convenience Methods

    public static void WriteObject(string fileName, object obj)
    {
        using var coder = new XmlWritingCoder(fileName);
        coder.Code(ref obj);
    }

    public static void WriteObject(Stream stream, object obj)
    {
        using var coder = new XmlWritingCoder(stream);
        coder.Code(ref obj);
    }

    #endregion

    #region Private Convenience Methods

    private void AddValue(object value)
    {
        m_element.Add(value);
    }

    #endregion

    #region ICoder Members

    public int CoderVersion { get { return c_coderVersion; } }

    public int StreamVersion { get { return m_version; } }

    public int MemoryVersion { get { return m_version; } }

    public string FileName
    {
        get { return m_fileName; }
        set { m_fileName = value; }
    }

    private bool HandleRef(ref object obj)
    {
        if (m_doRefs)
        {
            if (m_refs.TryGetValue(obj, out _))
            {
                if (TryGetTypeInfo(typeof(TypeCoder.Reference), out TypeInfo typeInfo))
                {
                    XElement refElement = new(typeInfo.XmlName);
                    refElement.Add(new XAttribute("num", m_refs[obj]));
                    AddValue(refElement);
                    return true;
                }
                else
                    throw new Exception(
                            "cannot encode multiply referenced object "
                            + "- change by configuring coder with "
                            + "\"coder.Add(TypeCoder.Default.Reference);\"");
            }

            m_refs[obj] = m_refs.Count;
        }
        return false;
    }

    public void Code(ref object obj)
    {
        TypeInfo typeInfo;
        if (obj == null)
        {
            if (TryGetTypeInfo(typeof(TypeCoder.Null), out typeInfo)
                && ((typeInfo.Options & TypeInfo.Option.Active) != 0))
            {
                AddValue(new XElement(typeInfo.XmlName));
                return;
            }
            else
            {
                throw new Exception("cannot encode null object "
                        + "- change by configuring coder with "
                        + "\"coder.Add(TypeCoder.Default.Null);\"");
            }
        }

        Type type = obj.GetType();

        if (!TypeCoder.IsDirectlyCodeable(type))
            if (HandleRef(ref obj)) return;

        string elementName;
        string typeName = null;
        TypeInfo.Option typeOptions = TypeInfo.Option.Size
                                        | TypeInfo.Option.Version;

        if (TryGetTypeInfo(type, out typeInfo))
        {
            elementName = typeInfo.XmlName;
            typeOptions = typeInfo.Options;
        }
        else
        {
            typeName = type.AssemblyQualifiedName;
            elementName = "object";
            if ((m_debugMode & CoderDebugMode.ReportQualifiedNames) != 0)
                Report.Line("qualified name \"{0}\"", typeName);
        }

        XElement element = new(elementName);
        if (typeName != null)
            element.Add(new XAttribute("type", typeName));

        if ((typeOptions & TypeInfo.Option.Version) != 0)
        {
            m_versionStack.Push(m_version);
            m_version = typeInfo != null ? typeInfo.Version : 0;
            element.Add(new XAttribute("version", m_version));
        }

        if (!TypeCoder.IsDirectlyCodeable(type))
            if (m_doRefs && m_writeRefNumbers)
                element.Add(new XAttribute("num", m_refs.Count - 1));

        m_elementStack.Push(m_element);
        m_element = element;

        #region code fields based on supported interface

        if (!TypeCoder.WritePrimitive(this, type, ref obj))
        {
            m_typeInfoStack.Push(typeInfo);

            if (obj is IFieldCodeable fcobj)
            {
                CodeFields(type, m_version, fcobj);
                if (obj is ITypedMap tmobj) CodeFields(type, tmobj);
            }
            else
            {
                throw new Exception(string.Format("uncodeable object: {0}", obj.GetType()));
            }

            m_typeInfoStack.Pop();
        }

        #endregion

        m_element = m_elementStack.Pop();
        AddValue(element);

        if ((typeOptions & TypeInfo.Option.Version) != 0)
            m_version = m_versionStack.Pop();
    }

    public void CodeFields(Type type, int version, IFieldCodeable obj)
    {
        FieldCoder[] fca = FieldCoderArray.Get(c_coderVersion, type, version, obj);
        foreach (var fc in fca)
        {
            var element = new XElement(fc.Name);
            m_elementStack.Push(m_element);
            m_element = element;
            fc.Code(this, obj);
            m_element = m_elementStack.Pop();
            AddValue(element);
        }
    }

    public void CodeFields(Type type, ITypedMap obj)
    {
        TypeOfString fieldTypeMap = FieldTypeMap.Get(type, obj);

        foreach (string fieldName in obj.FieldNames)
        {
            XElement element = null;
            XAttribute attribute = null;
            object value = obj[fieldName];
            string fieldTypeName;
            if (!fieldTypeMap.TryGetValue(fieldName, out Type fieldType))
            {
                fieldType = value.GetType();
                if (TryGetTypeInfo(fieldType, out TypeInfo typeInfo))
                    fieldTypeName = typeInfo.XmlName;
                else
                    fieldTypeName = fieldType.AssemblyQualifiedName;
                attribute = new XAttribute("type", fieldTypeName);
            }
            else
            {
                fieldTypeName = fieldType.Name;
            }

            if (fieldTypeName != fieldName || TypeCoder.IsDirectlyCodeable(fieldType))
            {
                element = new XElement(fieldName);
                if (attribute != null) element.Add(attribute);
                m_elementStack.Push(m_element);
                m_element = element;
            }
            TypeCoder.Write(this, fieldType, ref value);
            if (fieldTypeName != fieldName || TypeCoder.IsDirectlyCodeable(fieldType))
            {
                m_element = m_elementStack.Pop();
                AddValue(element);
            }
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

    private bool CodeNull<T>(ref T value)
    {
        if (value != null) return false;
        AddValue(new XElement("null"));
        return true;
    }

    public int CodeCount<T>(ref T value, Func<T, int> counter) where T : class
    {
        if (value == null)
        {
            AddValue(new XElement("null")); return -1;
        }
        object obj = value;

        if (m_doRefs)
        {
            if (m_refs.TryGetValue(obj, out _))
            {
                if (TryGetTypeInfo(typeof(TypeCoder.Reference), out TypeInfo typeInfo))
                {
                    var refElement = new XElement(typeInfo.XmlName);
                    refElement.Add(new XAttribute("num", m_refs[obj]));
                    AddValue(refElement);
                    return -2;
                }
                throw new Exception(
                        "cannot encode multiply referenced object "
                        + "- change by configuring coder with "
                        + "\"coder.Add(TypeCoder.Default.Reference);\"");
            }
        }            
        int count = counter(value);
        if (m_doRefs)
        {
            int refNum = m_refs.Count;
            m_refs[obj] = refNum;
            if (m_writeRefNumbers)
                AddValue(new XAttribute("num", refNum));
        }
        AddValue(new XAttribute("count", count));
        return count;
    }

    private int CodeCount<T>(ref T[] array)
    {
        return CodeCount(ref array, v => v.Length);
    }

    private int CodeCount<T>(ref List<T> list)
    {
        return CodeCount(ref list, v => v.Count);
    }

    public void CodeTArray<T>(ref T[] value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        for (int i = 0; i < count; i++)
            CodeT(ref value[i]);
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
        int count = CodeCount(ref value, s => s.Count);
        if (count < 1) return;
        foreach (var k in value)
        {
            object o = k;
            Code(ref o);
        }
    }

    public void Code(Type t, ref object o)
    {
        TypeCoder.Write(this, t, ref o);
    }

    public void Code(Type t, ref Array value)
    {
        int count = CodeCount(ref value, a => a.Length);
        if (count < 1) return;
        for (int i = 0; i < count; i++)
        {
            object o = value.GetValue(i);
            Code(ref o);
        }
    }
    public void Code(Type t, ref IList value)
    {
        int count = CodeCount(ref value, l => l.Count);
        if (count < 1) return;
        for (int i = 0; i < count; i++)
        {
            object o = value[i];
            Code(ref o);
        }
    }

    public void Code(Type t, ref IDictionary dict)
    {
        int count = CodeCount(ref dict, d => d.Count);
        if (count < 1) return;
        XElement element;
        Type[] subTypeArray = t.GetGenericArguments();
        var keys = dict.Keys;
        foreach (object k in keys)
        {
            var item = new XElement("item");
            m_elementStack.Push(m_element);
            m_element = item;

            object key = k;
            element = new XElement("key");
            m_elementStack.Push(m_element);
            m_element = element;
            TypeCoder.Write(this, subTypeArray[0], ref key);
            m_element = m_elementStack.Pop();
            AddValue(element);
            object val = dict[key];
            element = new XElement("val");
            m_elementStack.Push(m_element);
            m_element = element;
            TypeCoder.Write(this, subTypeArray[1], ref val);
            m_element = m_elementStack.Pop();
            AddValue(element);

            m_element = m_elementStack.Pop();
            AddValue(item);
        }
    }

    public void Code(Type t, ref ICountableDict dict)
    {
        throw new NotImplementedException();
    }

    public void Code(Type t, ref ICountableDictSet dict)
    {
        throw new NotImplementedException();
    }

    private void CodeTensor<T>(T dim, Action dataWriter)
    {
        var element = new XElement("dim");
        m_elementStack.Push(m_element);
        m_element = element;

        CodeT(ref dim);

        m_element = m_elementStack.Pop();
        AddValue(element);

        element = new XElement("data");
        m_elementStack.Push(m_element);
        m_element = element;

        dataWriter();

        m_element = m_elementStack.Pop();
        AddValue(element);
    }

    public void Code(Type t, ref IArrayVector value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var array = value.Array; Code(value.ArrayType, ref array);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var length = value.Size; CodeLong(ref length);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void Code(Type t, ref IArrayMatrix value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var array = value.Array; Code(value.ArrayType, ref array);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var length = value.Size; CodeV2l(ref length);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void Code(Type t, ref IArrayVolume value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var array = value.Array; Code(value.ArrayType, ref array);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var length = value.Size; CodeV3l(ref length);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void Code(Type t, ref IArrayTensor4 value)
    {
        throw new NotImplementedException();
    }

    public void Code(Type t, ref IArrayTensorN value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var array = value.Array; Code(value.ArrayType, ref array);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var length = value.Size; CodeLongArray(ref length);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeHashSet(Type t, ref object o)
    {
        throw new NotImplementedException("only possible whith a non-generic ISet interface)");
    }

    public void CodeEnum(Type t, ref object value)
    {
        var name = value.ToString();
        AddValue(name);
    }

    #endregion

    #region Code Primitives

    public void CodeBool(ref bool v) { AddValue(v.ToString()); }
    public void CodeByte(ref byte v) { AddValue(v.ToString()); }
    public void CodeSByte(ref sbyte v) { AddValue(v.ToString()); }
    public void CodeShort(ref short v) { AddValue(v.ToString()); }
    public void CodeUShort(ref ushort v) { AddValue(v.ToString()); }
    public void CodeInt(ref int v) { AddValue(v.ToString()); }
    public void CodeUInt(ref uint v) { AddValue(v.ToString()); }
    public void CodeLong(ref long v) { AddValue(v.ToString()); }
    public void CodeULong(ref ulong v) { AddValue(v.ToString()); }

    public void CodeFloat(ref float value)
    {
        AddValue(value.ToString(CultureInfo.InvariantCulture));
    }

    public void CodeDouble(ref double value)
    {
        AddValue(value.ToString(CultureInfo.InvariantCulture));
    }

    private static readonly char[] s_stringSplitChars =
    [
        '\0',
        Convert.ToChar(0x19),
        Convert.ToChar(0x01),
        Convert.ToChar(0x1e),
    ];

    private void CodeStringHelper(string value)
    {
        if (CodeNull(ref value)) return;
        foreach (string s in value.Split(s_stringSplitChars))
            AddValue(s);
    }

    public void CodeChar(ref char v) { AddValue(v.ToString()); }
    public void CodeString(ref string value) { CodeStringHelper(value); }

    public void CodeType(ref Type value)
    {
        string typeName;
        if (TryGetTypeInfo(value, out TypeInfo ti))
            typeName = ti.Name;
        else
            typeName = value.AssemblyQualifiedName;
        AddValue(typeName);
    }

    public void CodeGuid(ref Guid v) { AddValue(v.ToString()); }

    public void CodeSymbol(ref Symbol v) { throw new NotImplementedException(); }

    public void CodeGuidSymbol(ref Symbol v) { throw new NotImplementedException(); }

    public void CodePositiveSymbol(ref Symbol v) { throw new NotImplementedException(); }

    public void CodeIntSet(ref IntSet set) { throw new NotImplementedException(); }

    public void CodeSymbolSet(ref SymbolSet set) { throw new NotImplementedException(); }

    public void CodeFraction(ref Fraction v) { AddValue(v.ToString()); }

    public void CodeStructArray<T>(ref T[] a) where T : struct
    {
        throw new NotImplementedException();
    }

    public void CodeStructList<T>(ref List<T> l) where T : struct
    {
        throw new NotImplementedException();
    }

    private void CodeFormattableArray<T>(T[] value) where T : IFormattable
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        var text = new StringBuilder();
        for (int i = 0; i < count - 1; i++)
        {
            text.Append(value[i].ToString(null, CultureInfo.InvariantCulture));
            text.Append(", ");
        }
        text.Append(value[count - 1].ToString(null, CultureInfo.InvariantCulture));
        AddValue(text.ToString());
    }

    private void CodeFormattableList<T>(List<T> value) where T : IFormattable
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        var text = new StringBuilder();
        for (int i = 0; i < count - 1; i++)
        {
            text.Append(value[i].ToString(null, CultureInfo.InvariantCulture));
            text.Append(", ");
        }
        text.Append(value[count - 1].ToString(null, CultureInfo.InvariantCulture));
        AddValue(text.ToString());
    }

    private void CodeArrayOfStruct<T>(T[] value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        var text = new StringBuilder();
        for (int i = 0; i < count - 1; i++)
        {
            text.Append(value[i].ToString());
            text.Append(", ");
        }
        text.Append(value[count - 1].ToString());
        AddValue(text.ToString());
    }

    private void CodeListOfStruct<T>(List<T> value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        var text = new StringBuilder();
        for (int i = 0; i < count - 1; i++)
        {
            text.Append(value[i].ToString());
            text.Append(", ");
        }
        text.Append(value[count - 1].ToString());
        AddValue(text.ToString());
    }

    private void CodeArrayOf<T>(T[] value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        for (int i = 0; i < count; i++)
        {
            var item = value[i]; CodeT(ref item);
        }
    }

    private void CodeListOf<T>(List<T> value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        for (int i = 0; i < count; i++)
        {
            var item = value[i]; CodeT(ref item);
        }
    }

    public void CodeBoolArray(ref bool[] value) { CodeArrayOfStruct(value); }
    public void CodeByteArray(ref byte[] value) { CodeArrayOfStruct(value); }
    public void CodeSByteArray(ref sbyte[] value) { CodeArrayOfStruct(value); }
    public void CodeShortArray(ref short[] value) { CodeArrayOfStruct(value); }
    public void CodeUShortArray(ref ushort[] value) { CodeArrayOfStruct(value); }
    public void CodeIntArray(ref int[] value) { CodeArrayOfStruct(value); }
    public void CodeUIntArray(ref uint[] value) { CodeArrayOfStruct(value); }
    public void CodeLongArray(ref long[] value) { CodeArrayOfStruct(value); }
    public void CodeULongArray(ref ulong[] value) { CodeArrayOfStruct(value); }
    public void CodeFloatArray(ref float[] value) { CodeFormattableArray(value); }
    public void CodeDoubleArray(ref double[] value) { CodeFormattableArray(value); }

    public void CodeCharArray(ref char[] value) { CodeArrayOfStruct(value); }
    public void CodeStringArray(ref string[] value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        XElement element;
        foreach (string s in value)
        {
            element = new XElement("str");
            m_elementStack.Push(m_element);
            m_element = element;
            CodeStringHelper(s);
            m_element = m_elementStack.Pop();
            AddValue(element);
        }
    }

    public void CodeTypeArray(ref Type[] value) 
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        var text = new StringBuilder();

        for (int i = 0; i < count; i++)
        {
            string typeName;
            if (TryGetTypeInfo(value[i], out TypeInfo ti))
                typeName = ti.Name;
            else
                typeName = value[i].AssemblyQualifiedName;

            text.Append(typeName);

            if (i < count - 1)
                text.Append(", ");
        }
        AddValue(text.ToString());
    }

    public void CodeGuidArray(ref Guid[] v) { CodeArrayOfStruct(v); }

    public void CodeSymbolArray(ref Symbol[] v) { throw new NotImplementedException(); }
    
    public void CodeFractionArray(ref Fraction[] v) { CodeArrayOfStruct(v); }

    public void CodeList_of_Bool_(ref List<bool> value) { CodeListOfStruct(value); }
    public void CodeList_of_Byte_(ref List<byte> value) { CodeListOfStruct(value); }
    public void CodeList_of_SByte_(ref List<sbyte> value) { CodeListOfStruct(value); }
    public void CodeList_of_Short_(ref List<short> value) { CodeListOfStruct(value); }
    public void CodeList_of_UShort_(ref List<ushort> value) { CodeListOfStruct(value); }
    public void CodeList_of_Int_(ref List<int> value) { CodeListOfStruct(value); }
    public void CodeList_of_UInt_(ref List<uint> value) { CodeListOfStruct(value); }
    public void CodeList_of_Long_(ref List<long> value) { CodeListOfStruct(value); }
    public void CodeList_of_ULong_(ref List<ulong> value) { CodeListOfStruct(value); }
    public void CodeList_of_Float_(ref List<float> value) { CodeFormattableList(value); }
    public void CodeList_of_Double_(ref List<double> value) { CodeFormattableList(value); }

    public void CodeList_of_Char_(ref List<char> value) { CodeListOfStruct(value); }
    public void CodeList_of_String_(ref List<string> value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        XElement element;
        foreach (string s in value)
        {
            element = new XElement("string");
            m_elementStack.Push(m_element);
            m_element = element;
            CodeStringHelper(s);
            m_element = m_elementStack.Pop();
            AddValue(element);
        }
    }

    public void CodeList_of_Type_(ref List<Type> value)
    {
        int count = CodeCount(ref value);
        if (count < 1) return;
        var text = new StringBuilder();

        for (int i = 0; i < count; i++)
        {
            string typeName;
            if (TryGetTypeInfo(value[i], out TypeInfo ti))
                typeName = ti.Name;
            else
                typeName = value[i].AssemblyQualifiedName;

            text.Append(typeName);

            if (i < count - 1)
                text.Append(", ");
        }
        AddValue(text.ToString());
    }

    public void CodeList_of_Fraction_(ref List<Fraction> v) { CodeListOfStruct(v); }
    public void CodeList_of_Guid_(ref List<Guid> v) { CodeListOfStruct(v); }
    public void CodeList_of_Symbol_(ref List<Symbol> v) { throw new NotImplementedException(); }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        try
        {
            if (m_stream == null)
                return;

            var writer = new StreamWriter(m_stream, Encoding.UTF8);
            m_element.Save(writer);

            if (m_disposeStream)
                writer.Close();
            else
                writer.Flush();
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }

    #endregion
}

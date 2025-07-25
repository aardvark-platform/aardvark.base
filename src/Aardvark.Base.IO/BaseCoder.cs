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

[Flags]
public enum CoderDebugMode
{
    None = 0x0000,
    ReportObjects = 0x0001,
    ReportFields = 0x0002,
    ReportReferences = 0x0004,
    ReportQualifiedNames = 0x008,
    ReportAll = 0xffff,
}

/// <summary>
/// Root class for all coders, currently handles only debug mode.
/// </summary>
public class BaseCoder
{
    protected bool m_doRefs;
    protected CoderDebugMode m_debugMode;
    protected Stack<TypeInfo> m_typeInfoStack;
    protected Stack<int> m_versionStack;
    protected int m_version;
    internal const int c_coderVersion = 4;
                            // 3: KdTreeSet, SphereSet, TriangleSet are still Maps (later SymMaps)
                            // 2: old SymMap coding
                            // 1: wrong coding of SceneGraph Attribute<T>
                            // 0: wrong coding of null strings

    #region Contructor

    public BaseCoder()
    {
        if (!s_initTypeInfosCalled) InitTypeInfos();
        TypeCoder.Default.Init();
        m_doRefs = true;
        m_debugMode = CoderDebugMode.None;
        m_typeInfoStack = new Stack<TypeInfo>();
        m_versionStack = new Stack<int>();
        m_version = 0;
    }

    private static bool s_initTypeInfosCalled = false;
    private static readonly object s_initTypeInfosLock = new();

    private static void InitTypeInfos()
    {
        lock (s_initTypeInfosLock)
        {
            if (!s_initTypeInfosCalled)
            {
                s_initTypeInfosCalled = true;
                foreach (var assembly in Introspection.AllAssemblies)
                {
                    var typeInfos = Introspection.GetAllTypesWithAttribute<RegisterTypeInfoAttribute>(assembly).ToArray();
                    foreach (var x in typeInfos)
                    {
                        Report.Line(4, "[RegisterTypeInfo] {0}", x.Item1.FullName);
                        try
                        {
                            foreach (var a in x.Item2)
                                TypeInfo.Add(x.Item1, a.Version);
                        }
                        catch (Exception e)
                        {
                            Report.Warn("Error registering TypeInfo:\n{0}", e.ToString());
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Properties

    public CoderDebugMode DebugMode
    {
        get { return m_debugMode; }
        set { m_debugMode = value; }
    }

    public TypeInfo TypeInfo
    {
        get { return m_typeInfoStack.Peek(); }
    }

    #endregion

    #region Constants

    internal static readonly Symbol AardvarkSymbol = Symbol.Create("Aardvark");
    internal static readonly Symbol VersionSymbol = Symbol.Create("version");
    internal static readonly Symbol NumSymbol = Symbol.Create("num");
    internal static readonly Symbol CountSymbol = Symbol.Create("count");
    internal static readonly Symbol TypeSymbol = Symbol.Create("type");
    internal static readonly Symbol NullSymbol = Symbol.Create("null");
    internal static readonly Symbol RefSymbol = Symbol.Create("ref");

    #endregion
}

/// <summary>
/// This class handles type information for reading coders.
/// </summary>
public class BaseReadingCoder : BaseCoder
{
    protected Dictionary<string, TypeInfo> m_typeInfoOfName;
    private readonly Stack<List<TypeInfo>> m_typeInfoArrayStack;
    protected List<object> m_refs;

    #region Constructor

    public BaseReadingCoder() : base()
    {
        m_typeInfoOfName = [];
        m_typeInfoArrayStack = new Stack<List<TypeInfo>>();
        m_refs = [];
    }

    #endregion

    #region Properties

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
    public bool IsReading => true;
    public bool IsWriting => false;
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression
    public List<object> Refs { get { return m_refs; } set { m_refs = value; } }

    #endregion

    #region TypeInfo Management

    public void Add(TypeInfo[] tia)
    {
        var oldTypeInfoList = new List<TypeInfo>();
        foreach (var ti in tia)
        {
            if (m_typeInfoOfName.TryGetValue(ti.Name, out TypeInfo old))
                oldTypeInfoList.Add(old);
            m_typeInfoOfName.Add(ti.Name, ti);
            if (ti.Type == typeof(TypeCoder.Reference)
                && ((ti.Options & TypeInfo.Option.Active) == 0))
                m_doRefs = false;
        }
        m_typeInfoArrayStack.Push(oldTypeInfoList);
    }

    public void Del(TypeInfo[] tia)
    {
        foreach (var ti in tia)
        {
            m_typeInfoOfName.Remove(ti.Name);
            if (ti.Type == typeof(TypeCoder.Reference)) m_doRefs = true;
        }
        var oldTypeInfoList = m_typeInfoArrayStack.Pop();
        foreach (var ti in oldTypeInfoList)
        {
            m_typeInfoOfName.Add(ti.Name, ti);
            if (ti.Type == typeof(TypeCoder.Reference)
                && ((ti.Options & TypeInfo.Option.Active) == 0))
                m_doRefs = false;
        }
    }

    #endregion
}

/// <summary>
/// This class handles type information for writing coders.
/// </summary>
public class BaseWritingCoder : BaseCoder
{
    protected Dictionary<Type, TypeInfo> m_typeInfoOfType;
    private readonly Stack<List<TypeInfo>> m_typeInfoArrayStack;
    protected Dictionary<object, int> m_refs;

    #region Constructor

    public BaseWritingCoder() : base()
    {
        m_typeInfoOfType = [];
        m_typeInfoArrayStack = new Stack<List<TypeInfo>>();
        m_refs = [];
    }

    #endregion

    #region Properties

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
    public bool IsReading => false;
    public bool IsWriting => true;
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression

    public Dictionary<object, int> Refs { get { return m_refs; } set { m_refs = value; } }

    #endregion

    #region TypeInfo Management

    internal bool TryGetTypeInfo(Type type, out TypeInfo ti)
    {
        if (m_typeInfoOfType.TryGetValue(type, out ti)) return true;
        return TypeInfo.OfType.TryGetValue(type, out ti);
    }

    public void Add(TypeInfo[] tia)
    {
        var oldTypeInfoList = new List<TypeInfo>();
        foreach (var ti in tia)
        {
            if (m_typeInfoOfType.TryGetValue(ti.Type, out TypeInfo old))
                oldTypeInfoList.Add(old);
            m_typeInfoOfType.Add(ti.Type, ti);
            if (ti.Type == typeof(TypeCoder.Reference)
                && ((ti.Options & TypeInfo.Option.Active) == 0))
                m_doRefs = false;
        }
        m_typeInfoArrayStack.Push(oldTypeInfoList);
    }

    public void Del(TypeInfo[] tia)
    {
        foreach (var ti in tia)
        {
            m_typeInfoOfType.Remove(ti.Type);
            if (ti.Type == typeof(TypeCoder.Reference)) m_doRefs = true;
        }
        var oldTypeInfoList = m_typeInfoArrayStack.Pop();
        foreach (var ti in oldTypeInfoList)
        {
            m_typeInfoOfType.Add(ti.Type, ti);
            if (ti.Type == typeof(TypeCoder.Reference)
                && ((ti.Options & TypeInfo.Option.Active) == 0))
                m_doRefs = false;
        }
    }

    #endregion
}

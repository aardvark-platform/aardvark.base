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
using System.Text.RegularExpressions;

namespace Aardvark.Base.Coder;

/// <summary>
/// This represents the association between a type, its short name,
/// and its XML-name. Options can be specified for more control of
/// these associations.
/// </summary>
public partial class TypeInfo
{
    private Type m_type;
    private string m_name;
    private readonly string m_xmlName;
    private readonly int m_version;
    private Option m_options;
    private Dictionary<int, TypeInfo> m_versionMap;
    private readonly Func<object> m_creator;
    private object m_typeOptions;

    public Type ProxyType;  // if this != null m_creator must create a proxy object
    public Func<object, IFieldCodeable> Object2ProxyFun;
    public Func<IFieldCodeable, object> Proxy2ObjectFun;


    #region Constructors

    private TypeInfo(Type type, string name, string xmlName, int version,
                    Option options, Dictionary<int, TypeInfo> versionMap,
                    Func<object> creator, object typeOptions)
    {
        m_type = type; m_name = name; m_xmlName = xmlName;
        m_version = version; m_options = options;
        m_versionMap = versionMap;
        m_creator = creator;
        m_typeOptions = typeOptions;
    }

    public TypeInfo(Type type, string name, string xmlName, int version,
                    Option options, Func<object> creator)
    {
        m_type = type; m_name = name; m_xmlName = xmlName;
        m_version = version; m_options = options;
        m_versionMap = null;
        m_creator = creator;
    }

    public TypeInfo(TypeInfo ti)
        : this(ti.m_type, ti.m_name, ti.m_xmlName, ti.m_version, ti.m_options,
               ti.m_versionMap, ti.m_creator, ti.m_typeOptions)
    { }

    public TypeInfo(TypeInfo ti, int version)
        : this(ti.m_type, ti.m_name, ti.m_xmlName, version, ti.m_options,
               ti.m_versionMap, ti.m_creator, ti.m_typeOptions)
    { }

    public TypeInfo(string name, Type type, int version = 0)
        : this(type, name, name, version, Option.Version | Option.Size, null)
    { }

    public TypeInfo(Type type, int version = 0)
        : this(type, type.GetCanonicalName(), type.GetCanonicalXmlName(),
               version, Option.Version|Option.Size, null)
    { }

    public TypeInfo(string name, Type type, Option codeInfo)
        : this(type, name, name, 0, codeInfo, null)
    { }

    public TypeInfo(string name, string xmlName, Type type, Option codeInfo)
        : this(type, name, xmlName, 0, codeInfo, null)
    { }

    public TypeInfo(Type type, Option codeInfo)
        : this(type, type.GetCanonicalName(), type.GetCanonicalXmlName(),
               0, codeInfo, null)
    { }

    public TypeInfo(
            string name, Type type, Option codeInfo,
            Func<object> creator)
        : this(type, name, name, 0, codeInfo, creator)
    { }

    public TypeInfo(
            Type type, Option codeInfo,
            Func<object> creator)
        : this(type, type.GetCanonicalName(), type.GetCanonicalXmlName(),
               0, codeInfo, creator)
    { }

    public TypeInfo(
            Type type,
            Func<object> creator)
        : this (type, type.GetCanonicalName(), type.GetCanonicalXmlName(),
               0, Option.Version | Option.Size, creator)
    { }


    /// <summary>
    /// Creates a type info for a type that uses the IFieldCodeable
    /// implementation of a proxy type for coding.
    /// </summary>
    /// <param name="type">the that needs to be coded</param>
    /// <param name="proxyType">the type of the proxy object</param>
    /// <param name="proxyCreator">creates a proxy object for reading</param>
    /// <param name="proxyToObjectFun">converts the proxy object that was used for reading
    /// into the actual object</param>
    /// <param name="objectToProxyFun">converts the actual object into a proxy object that
    /// is used for writing</param>
    public TypeInfo(
            Type type,
            Type proxyType,
            Func<object> proxyCreator,
            Func<IFieldCodeable, object> proxyToObjectFun, // used for reading
            Func<object, IFieldCodeable> objectToProxyFun // used for writing
            )
            : this (type, proxyCreator)
    {
        ProxyType = proxyType;
        Proxy2ObjectFun = proxyToObjectFun;
        Object2ProxyFun = objectToProxyFun;
    }

    #endregion

    #region Properties

    public Type Type { get { return m_type; } set { m_type = value; } }
    public string Name { get { return m_name; } set { m_name = value; } }
    public string XmlName { get { return m_xmlName; } }
    public int Version { get { return m_version; } }
    public object TypeOptions { get { return m_typeOptions; } set { m_typeOptions = value; } }

    public Dictionary<int, TypeInfo> VersionMap
    {
        get { return m_versionMap ?? s_emptyVersionMap; }
    }

    public Option Options
    {
        get { return m_options; }
        set { m_options = value; }
    }
    public Func<object> Creator { get { return m_creator; } }

    #endregion

    #region Option

    [Flags]
    public enum Option
    {
        None = 0x00,
        Active = 0x01, // currently only used for nulls and refs
        Version = 0x02,
        Size = 0x04,
        Ignore = 0x08,
    }

    #endregion

    #region Private Methods

    public void AddVersion(TypeInfo oldTypeInfo)
    {
        m_versionMap ??= [];
        m_versionMap.Add(oldTypeInfo.m_version, oldTypeInfo);
    }

    #endregion

    #region Private Static Members

    private static readonly object s_lock = new();

    private static readonly Dictionary<int, TypeInfo> s_emptyVersionMap = [];

    private static readonly Dictionary<Type, TypeInfo> s_ofType = [];
    private static readonly Dictionary<string, TypeInfo> s_ofName = [];
    private static readonly Dictionary<string, TypeInfo> s_ofXmlName = [];

    #endregion

    #region Public Static Members

    public static Dictionary<Type, TypeInfo> OfType
    {
        get { return s_ofType; }
    }
    public static Dictionary<string, TypeInfo> OfName
    {
        get { return s_ofName; }
    }
    public static Dictionary<string, TypeInfo> OfXmlName
    {
        get { return s_ofXmlName; }
    }

#if NETSTANDARD2_0
    private static readonly Regex s_symbolRegex = new(@"([A-Za-z_][0-9A-Za-z_]*)(\.[A-Za-z_][0-9A-Za-z_]*)*");
#else
    [GeneratedRegex(@"([A-Za-z_][0-9A-Za-z_]*)(\.[A-Za-z_][0-9A-Za-z_]*)*")]
    private static partial Regex GenerateSymbolRegex();
    private static readonly Regex s_symbolRegex = GenerateSymbolRegex();
#endif


    public static bool TryGetOfFullName(string fullName, out TypeInfo typeInfo)
    {
        var isArray = false;
        var match = s_symbolRegex.Match(fullName);
        if (!match.Success) { typeInfo = default; return false; }
#if NETSTANDARD2_0
        if (fullName.Substring(match.Index + match.Length).StartsWith("[]")) isArray = true;
#else
        if (fullName[(match.Index + match.Length)..].StartsWith("[]")) isArray = true;
#endif
        var dotName = match.Value;
#if NETSTANDARD2_0
        var name = dotName.Substring(dotName.LastIndexOf('.') + 1);
#else
        var name = dotName[(dotName.LastIndexOf('.') + 1)..];
#endif
        lock (s_lock)
        {
            if (OfName.TryGetValue(name, out typeInfo))
            {
                if (!isArray) { OfName[fullName] = typeInfo; return true; }
                var newFullName = typeInfo.Type.AssemblyQualifiedName;
                var newMatch = s_symbolRegex.Match(newFullName);
                if (!newMatch.Success) { typeInfo = default; return false; }
                var newShortName = newMatch.Value; var pos = newMatch.Index + newMatch.Length;
#if NETSTANDARD2_0
                var newName = newFullName.Substring(0, pos) + "[]" + newFullName.Substring(pos);
#else
                var newName = string.Concat(newFullName.AsSpan()[..pos], "[]", newFullName.AsSpan(pos));
#endif
                typeInfo = new TypeInfo(typeInfo.Name + "[]", Type.GetType(newName), TypeInfo.Option.Size | TypeInfo.Option.Version);
                return true;
            }
            return false;
        }
    }

#if FUTURE
    public static TypeInfo OfType(Type type)
    {
        TypeInfo typeInfo;
        lock (s_lock)
        {
            if (!s_ofType.TryGetValue(type, out typeInfo))
                typeInfo = null;
        }
        return typeInfo;
    }

    public static TypeInfo OfName(string name)
    {
        TypeInfo typeInfo;
        lock (s_lock)
        {
            if (!s_ofName.TryGetValue(name, out typeInfo))
                typeInfo = null;
        }
        return typeInfo;
    }

    public static TypeInfo OfXmlName(string xmlName)
    {
        TypeInfo typeInfo;
        lock (s_lock)
        {
            if (!s_ofXmlName.TryGetValue(xmlName, out typeInfo))
                typeInfo = null;
        }
        return typeInfo;
    }
#endif

#endregion

    #region Private Static Methods

    private static void AddName(string name, TypeInfo typeInfo)
    {
        if (s_ofName.TryGetValue(name, out TypeInfo used))
        {
            throw new ArgumentException(
                String.Format("type name \"{0}\" cannot be added for"
                              + " \"{1}\" since it is already in use by"
                              + " \"{2}\"",
                              name, typeInfo.Type.AssemblyQualifiedName,
                              used.Type.AssemblyQualifiedName));
        }
        s_ofName.Add(name, typeInfo);
    }

    private static void AddXmlName(string xmlName, TypeInfo typeInfo)
    {
        if (s_ofXmlName.TryGetValue(xmlName, out TypeInfo used))
        {
            throw new ArgumentException(
                String.Format("xml type name \"{0}\" cannot be added for"
                              + " \"{1}\" since it is already in use by"
                              + " \"{2}\"",
                              xmlName, typeInfo.Type.AssemblyQualifiedName,
                              used.Type.AssemblyQualifiedName));
        }
        s_ofXmlName.Add(xmlName, typeInfo);
    }

    #endregion

    #region Public Static Methods

    public static void Add(TypeInfo typeInfo)
    {
        lock (s_lock)
        {
#if NETSTANDARD2_0
            if (!s_ofType.ContainsKey(typeInfo.Type))
            {
                AddName(typeInfo.Name, typeInfo);
                AddXmlName(typeInfo.XmlName, typeInfo);
                s_ofType.Add(typeInfo.Type, typeInfo);
            }
#else
            if (s_ofType.TryAdd(typeInfo.Type, typeInfo))
            {
                AddName(typeInfo.Name, typeInfo);
                AddXmlName(typeInfo.XmlName, typeInfo);
            }
#endif
        }
    }

    public static void Add(IEnumerable<TypeInfo> typeInfos)
    {
        lock (s_lock)
        {
            foreach (var ti in typeInfos) Add(ti);
        }
    }

    public static void Add(Type type, int version = 0)
    {
        lock (s_lock)
        {
            if (!s_ofType.ContainsKey(type))
                Add(new TypeInfo(type, version));
        }
    }

    public static void Add(IEnumerable<Type> types)
    {
        lock (s_lock)
        {
            foreach (var type in types)
                if (!s_ofType.ContainsKey(type))
                    Add(new TypeInfo(type, 0));
        }
    }


    public static void Add(TypeInfo typeInfo, params string[] alternateNames)
    {
        lock (s_lock)
        {
            if (!s_ofType.ContainsKey(typeInfo.Type))
            {
                Add(typeInfo);
                foreach (string name in alternateNames)
                    AddName(name, typeInfo);
            }
        }
    }

    #endregion

}

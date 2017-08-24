using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.VRVis
{
    /// <summary>
    /// A field coder defines the name of a field and an associated coding
    /// function which is used for both reading and writing the field.
    /// </summary>
    public struct FieldCoder
    {
        int m_order;
        string m_name;
        int m_minVersion;
        int m_maxVersion;
        Action<ICoder, object> m_code;

        #region Constructors

        public FieldCoder(int order, string name,
                          Action<ICoder, object> code)
            : this(order, name, 0, int.MaxValue, code)
        { }


        /// <summary>
        /// Defines a FieldCoder that is only used for objects with version
        /// numbers inside the inclusive interval
        /// [minVersionInclusive, maxVersionInclusive].
        /// </summary>
        public FieldCoder(int order, string name, int minVersionInclusive, int maxVersionInclusive,
                          Action<ICoder, object> code)
        {
            m_order = order; m_name = name;
            m_minVersion = minVersionInclusive; m_maxVersion = maxVersionInclusive;
            m_code = code;
        }

        #endregion

        #region Properties

        public int Order { get { return m_order; } }
        public string Name { get { return m_name; } set { m_name = value; } }
        public int MinVersion { get { return m_minVersion; } }
        public int MaxVersion { get { return m_maxVersion; } }
        public Action<ICoder, object> Code { get { return m_code; } set { m_code = value; } }

        #endregion
    }

    /// <summary>
    /// A field-codeable needs to supply an array of <see cref="FieldCoder"/>s.
    /// </summary>
    public interface IFieldCodeable
    {
        /// <summary>
        /// Supply a <see cref="FieldCoder"/>s for each field that needs to
        /// be coded. Note that this function is normally only called once
        /// for each type, when the first instance of this type is encountered.
        /// </summary>
        IEnumerable<FieldCoder> GetFieldCoders(int coderVersion);
    }

    #region FieldCoderExtensions

    public static class IEnumerableOfFieldCoderExtensions
    {
        /// <summary>
        /// This extension is necessary to renumber the relative order
        /// values of the base class, if a derived class adds additional
        /// field coders. The standard usages is:
        /// <code>
        /// base.GetFieldCoders().Base().Concat(additionalFieldCoders)
        /// </code>
        /// </summary>
        public static IEnumerable<FieldCoder> Base(
            this IEnumerable<FieldCoder> fieldCoders)
        {
            if (fieldCoders.Count() == 0) return fieldCoders;
            int max = fieldCoders.Max(fc => fc.Order) + 1;
            return from fc in fieldCoders
                   select new FieldCoder(fc.Order - max, fc.Name, fc.MinVersion, fc.MaxVersion, fc.Code);
        }
    }

    #endregion

    #region FieldCoderArray

    internal static class FieldCoderArray
    {
        private static object s_lock = new object();
        private static Dictionary<Tup<int, Type, int>, FieldCoder[]> s_fieldCoderArrayMap
                = new Dictionary<Tup<int, Type, int>, FieldCoder[]>();

        public static FieldCoder[] Get(int coderVersion, Type type, int version, IFieldCodeable fieldCodeAble)
        {
            var key = Tup.Create(coderVersion, type, version);
            FieldCoder[] fieldCoderArray;
            lock (s_lock)
            {
                if (!s_fieldCoderArrayMap.TryGetValue(key, out fieldCoderArray))
                {
                    fieldCoderArray = fieldCodeAble.GetFieldCoders(coderVersion)
                        .OrderBy(fc => fc.Order)
                        .Where(fc => fc.MinVersion <= version && version <= fc.MaxVersion)
                        .ToArray();
                    s_fieldCoderArrayMap[key] = fieldCoderArray;
                }
            }
            return fieldCoderArray;
        }

    }

    #endregion

    #region FieldCoderMap

    internal class FieldCoderMap : Dictionary<string, Action<ICoder, object>>
    {
        private static object s_lock = new object();
        private static Dictionary<Tup<int, Type, int>, FieldCoderMap> s_fieldCoderMapMap
                = new Dictionary<Tup<int, Type, int>, FieldCoderMap>();

        public static FieldCoderMap Get(int coderVersion, Type type, int version, IFieldCodeable fieldCodeAble)
        {
            var key = Tup.Create(coderVersion, type, version);
            FieldCoderMap fieldCoderMap;
            lock (s_lock)
            {
                if (!s_fieldCoderMapMap.TryGetValue(key, out fieldCoderMap))
                {
                    fieldCoderMap = new FieldCoderMap();
                    var fieldCoders = fieldCodeAble.GetFieldCoders(coderVersion)
                              .Where(fc => fc.MinVersion <= version && version <= fc.MaxVersion);
                    foreach (var fc in fieldCoders)
                        fieldCoderMap[fc.Name] = fc.Code;
                    s_fieldCoderMapMap[key] = fieldCoderMap;
                }
            }
            return fieldCoderMap;
        }
    }

    #endregion
}

using System;
using System.Collections.Generic;

namespace Aardvark.Base.Coder
{
    public struct FieldType
    {
        private string m_name;
        private Type m_type;

        #region Constructor

        public FieldType(string name, Type type)
        {
            m_name = name; m_type = type;
        }

        #endregion

        #region Properties

        public string Name { get { return m_name; } set { m_name = value; } }
        public Type Type { get { return m_type; } set { m_type = value; } }

        #endregion
    }

    public interface ITypedMap
    {
        /// <summary>
        /// Returns the types of all fields of the ITypedMap. Note that it is
        /// possible that this method does not report all fields of the
        /// concrete object it was called for. It just returns fields that are
        /// known in all instances of this class. Also, not all of the fields
        /// for which types are known, are present in all instances of this
        /// class.
        /// </summary>
        IEnumerable<FieldType> FieldTypes { get; }

        /// <summary>
        /// Returns the number of actual fields of the concrete object.
        /// </summary>
        int FieldCount { get; }

        /// <summary>
        /// Returns the names of all fields of the concrete object. Note that
        /// this can return names of fields that are not reported in
        /// <see cref="FieldTypes"/>
        /// </summary>
        IEnumerable<string> FieldNames { get; }

        /// <summary>
        /// This provides access to all the fields by their name.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }
    }

    #region FieldTypeMap

    public class TypeOfString : Dictionary<string, Type> { }
    public static class FieldTypeMap
    {
        static object s_lock = new object();
        static Dictionary<Type, TypeOfString> s_fieldTypeTypeMap
                = new Dictionary<Type, TypeOfString>();

        public static TypeOfString Get(Type type, ITypedMap typedMap)
        {
            TypeOfString fieldTypeMap;
            lock (s_lock)
            {
                if (!s_fieldTypeTypeMap.TryGetValue(type, out fieldTypeMap))
                {
                    fieldTypeMap = new TypeOfString();
                    foreach (FieldType ft in typedMap.FieldTypes)
                        fieldTypeMap[ft.Name] = ft.Type;
                    s_fieldTypeTypeMap[type] = fieldTypeMap;
                }
            }
            return fieldTypeMap;
        }
    }

    #endregion
}

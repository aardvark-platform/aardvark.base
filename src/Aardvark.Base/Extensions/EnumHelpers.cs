using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class EnumHelpers
    {
        private sealed class EnumMetadata
        {
            private readonly TypeCode m_underlyingTypeCode;
            private readonly object[] m_values;
            private readonly Dictionary<ulong, int> m_indices;

            public EnumMetadata(Type enumType)
            {
                m_underlyingTypeCode = Type.GetTypeCode(Enum.GetUnderlyingType(enumType));

                var enumValues = Enum.GetValues(enumType);
                var values = new List<object>(enumValues.Length);
                m_indices = new Dictionary<ulong, int>(enumValues.Length);
                for (var i = 0; i < enumValues.Length; i++)
                {
                    var value = enumValues.GetValue(i);
                    var key = GetKey(value);
                    if (m_indices.ContainsKey(key)) continue;

                    m_indices.Add(key, values.Count);
                    values.Add(value);
                }

                m_values = values.ToArray();
            }

            private ulong GetKey(object value)
            {
                switch (m_underlyingTypeCode)
                {
                    case TypeCode.Byte: return Convert.ToByte(value);
                    case TypeCode.SByte: return unchecked((byte)Convert.ToSByte(value));
                    case TypeCode.Int16: return unchecked((ushort)Convert.ToInt16(value));
                    case TypeCode.UInt16: return Convert.ToUInt16(value);
                    case TypeCode.Int32: return unchecked((uint)Convert.ToInt32(value));
                    case TypeCode.UInt32: return Convert.ToUInt32(value);
                    case TypeCode.Int64: return unchecked((ulong)Convert.ToInt64(value));
                    case TypeCode.UInt64: return Convert.ToUInt64(value);
                    default: throw new InvalidOperationException("Unsupported enumeration underlying type.");
                }
            }

            public int GetIndex(object value) => m_indices[GetKey(value)];

            public object GetValue(int index) => m_values[index];

            public object GetPreviousValue(object value)
            {
                var index = GetIndex(value);
                return m_values[index > 0 ? index - 1 : m_values.Length - 1];
            }

            public object GetNextValue(object value)
            {
                var index = GetIndex(value);
                return m_values[index < m_values.Length - 1 ? index + 1 : 0];
            }
        }

        private static readonly ConcurrentDictionary<Type, EnumMetadata> s_metadata =
            new ConcurrentDictionary<Type, EnumMetadata>();

        private static EnumMetadata GetMetadata(Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException($"{enumType.Name} is not an enumeration type.");
            return s_metadata.GetOrAdd(enumType, type => new EnumMetadata(type));
        }

        /// <summary>
        /// Returns the previous distinct underlying value in <see cref="Enum.GetValues(Type)"/> order,
        /// wrapping to the last value when the current value is first. Aliases share the same position.
        /// </summary>
        public static T GetPrevValue<T>(T enumValue)
        // NOTE: where T: enum
        {
            return (T)GetMetadata(typeof(T)).GetPreviousValue(enumValue);
        }

        /// <summary>
        /// Returns the next distinct underlying value in <see cref="Enum.GetValues(Type)"/> order,
        /// wrapping to the first value when the current value is last. Aliases share the same position.
        /// </summary>
        public static T GetNextValue<T>(T enumValue)
        // NOTE: where T: enum
        {
            return (T)GetMetadata(typeof(T)).GetNextValue(enumValue);
        }

        /// <summary>
        /// Returns the zero-based index of the distinct underlying enumeration value in
        /// <see cref="Enum.GetValues(Type)"/> order. Aliases share the same index.
        /// </summary>
        public static int GetIndex<T>(T enumValue)
        {
            return GetMetadata(typeof(T)).GetIndex(enumValue);
        }

        /// <summary>
        /// Returns the zero-based index of the distinct underlying enumeration value in
        /// <see cref="Enum.GetValues(Type)"/> order. Aliases share the same index.
        /// </summary>
        public static int GetIndex(Type enumType, object enumValue)
        {
            return GetMetadata(enumType).GetIndex(enumValue);
        }

        /// <summary>
        /// Returns the <see cref="int"/> representation of the distinct enumeration value at the
        /// specified index in <see cref="Enum.GetValues(Type)"/> order.
        /// </summary>
        public static int GetValue(Type enumType, int index)
        {
            return Convert.ToInt32(GetMetadata(enumType).GetValue(index));
        }

        /// <summary>
        /// Returns the distinct enumeration value at the specified index in
        /// <see cref="Enum.GetValues(Type)"/> order. Alias values occupy a single index.
        /// </summary>
        public static T GetValue<T>(int index)
        {
            return (T)GetMetadata(typeof(T)).GetValue(index);
        }
    }
}

using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class EnumHelpers
    {
        static Dict<Type, Dict<long, (object, object)>> s_neighbourValuesDicts = new Dict<Type, Dict<long, (object, object)>>();

        static Dict<long, (object, object)> GetNeighbourValuesDict<T>(Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException(nameof(enumType));

            if (!s_neighbourValuesDicts.TryGetValue(enumType, out Dict<long, (object, object)> neighbourValuesDict))
            {
                var values = (T[])Enum.GetValues(enumType);

                neighbourValuesDict = new Dict<long, (object, object)>();
                for (int i = 0; i < values.Length; i++)
                {
                    neighbourValuesDict[Convert.ToInt64(values[i])] = 
                        (
                            values[(i > 0 ? i : values.Length) - 1], 
                            values[i < values.Length - 1 ? i + 1 : 0]
                        ); // previous and next value
                }

                s_neighbourValuesDicts[enumType] = neighbourValuesDict;
            }

            return neighbourValuesDict;
        }

        /// <summary>
        /// Returns the previous value of the enumeration or the last when the current value is the first one.
        /// </summary>
        public static T GetPrevValue<T>(T enumValue)
        // NOTE: where T: enum
        {
            var neighbourValuesDict = GetNeighbourValuesDict<T>(typeof(T));

            var intValue = Convert.ToInt64(enumValue);
            neighbourValuesDict.TryGetValue(intValue, out (object, object) result);
            return (T)result.Item1;
        }

        /// <summary>
        /// Returns the next value of the enumeration or the first when the current value is the last one.
        /// </summary>
        public static T GetNextValue<T>(T enumValue)
        // NOTE: where T: enum
        {
            var neighbourValuesDict = GetNeighbourValuesDict<T>(typeof(T));

            var intValue = Convert.ToInt64(enumValue);
            neighbourValuesDict.TryGetValue(intValue, out (object, object) result);
            return (T)result.Item2;
        }

        static Dictionary<Type, Tup<Array, Dictionary<long, int>>> s_valueIndexMapping = new Dictionary<Type, Tup<Array, Dictionary<long, int>>>(); // long = worst case enum value

        static Tup<Array, Dictionary<long, int>> GetValueIndexMapping(Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException(nameof(enumType));

            return s_valueIndexMapping.GetCreate(enumType, et =>
            {
                var enumValues = Enum.GetValues(et);
                var enumIndices = new Dictionary<long, int>();
                for (int i = 0; i < enumValues.Length; i++)
                    enumIndices.Add(Convert.ToInt64(enumValues.GetValue(i)), i);
                return Tup.Create(enumValues, enumIndices);
            });
        }

        /// <summary>
        /// Return the index of the enumeration value
        /// </summary>
        public static int GetIndex<T>(T enumValue)
        {
            var mapping = GetValueIndexMapping(typeof(T));
            return mapping.E1[Convert.ToInt64(enumValue)];
        }

        /// <summary>
        /// Return the index of the enumeration value
        /// </summary>
        public static int GetIndex(Type enumType, object enumValue)
        {
            var mapping = GetValueIndexMapping(enumType);
            return mapping.E1[Convert.ToInt64(enumValue)]; 
        }

        /// <summary>
        /// Returns the enumeartion value of a certain index
        /// </summary>
        public static int GetValue(Type enumType, int index)
        {
            var mapping = GetValueIndexMapping(enumType);
            return (int)mapping.E0.GetValue(index);
        }

        /// <summary>
        /// Returns the enumeartion value of a certain index
        /// </summary>
        public static T GetValue<T>(int index)
        {
            var mapping = GetValueIndexMapping(typeof(T));
            return (T)mapping.E0.GetValue(index);
        }
    }
}

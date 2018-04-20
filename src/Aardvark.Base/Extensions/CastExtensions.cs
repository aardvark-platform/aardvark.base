using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static class CastExtensions
    {
        public static T[] ToArrayOfT<T>(this IEnumerable s)
        {
            return (from T o in s select (T)o).ToArray();
        }

        public static List<T> ToListOfT<T>(this IEnumerable s)
        {
            return (from T o in s select o).ToList();
        }

        public static IEnumerable<T> ToIEnumerableOfT<T>(this IEnumerable s)
        {
            return from T o in s select o;
        }

        public static List<T> IntoList<T>(this T item)
        {
            return new List<T>() { item };
        }

        public static T[] IntoArray<T>(this T item)
        {
            return new T[] { item };
        }

        public static IEnumerable<T> IntoIEnumerable<T>(this T item)
        {
            return new T[] { item };
            // yield return item; // this generates a more code
        }

        /// <summary>
        /// Returns the item as a T.
        /// Or throws an exception if not convertible.
        /// </summary>
        public static T To<T>(this object item) where T : class
        {
            if (item is T) return item as T;
            throw new ArgumentException(string.Format("expected {0} instead of {1}", typeof(T), item.GetType()));
        }
    }
}

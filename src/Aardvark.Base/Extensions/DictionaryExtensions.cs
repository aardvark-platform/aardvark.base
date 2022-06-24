using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region Dictionary Extensions

    public static class DictionaryFun
    {
        public static Dictionary<Tk, Tv> Copy<Tk, Tv>(
            this Dictionary<Tk, Tv> self)
        {
            var r = new Dictionary<Tk, Tv>(self.Count);
            foreach (var kvp in self)
                r[kvp.Key] = kvp.Value;
            return r;
        }

        public static Dictionary<Tk, T1v> Copy<Tk, Tv, T1v>(
            this Dictionary<Tk, Tv> self,
            Func<Tv, T1v> fun)
        {
            var r = new Dictionary<Tk, T1v>(self.Count);
            foreach (var kvp in self)
                r[kvp.Key] = fun(kvp.Value);
            return r;
        }

        public static Dictionary<Tk, T1v> Copy<Tk, Tv, T1v>(
            this Dictionary<Tk, Tv> self,
            Dictionary<Tk, Func<Tv, T1v>> funMap,
            Func<Tv, T1v> defaultFun)
        {
            var r = new Dictionary<Tk, T1v>(self.Count);
            foreach (var kvp in self)
            {
                if (funMap.TryGetValue(kvp.Key, out Func<Tv, T1v> fun))
                    r[kvp.Key] = fun(kvp.Value);
                else if (defaultFun != null)
                    r[kvp.Key] = defaultFun(kvp.Value);
            }
            return r;
        }

        /// <summary>
        /// Combines the dictionary with another one. Duplicate keys
        /// are overwritten.
        /// </summary>
        public static Dictionary<Tk, Tv> Combine<Tk, Tv>(
            this Dictionary<Tk, Tv> self,
            Dictionary<Tk, Tv> second)
        {
            var result = self.Copy();
            foreach (var kvp in second)
                result[kvp.Key] = kvp.Value;
            return result;
        }
    }


    #endregion

    #region IDictionaryExt

    public static class IDictionaryExtensions
    { 
        /// <summary>
        /// Tries to get a value from the dictionary.
        /// If the key is not found, default(T) will be returned.
        /// </summary>
        public static Tv Get<Tk, Tv>(
            this IDictionary<Tk, Tv> self, Tk key)
        {
            if (self.TryGetValue(key, out Tv value)) return value;
            return default(Tv);
        }

        /// <summary>
        /// Tries to get a value from the dictionary. 
        /// If the key is not found, the specified defaultValue will be returned.
        /// </summary>
        public static Tv Get<Tk, Tv>(
            this IDictionary<Tk, Tv> self, Tk key, Tv defaultValue)
        {
            if (self.TryGetValue(key, out Tv value)) return value;
            return defaultValue;
        }

        /// <summary>
        /// Gets the value with the given key. If not found, a new value for the key is created.
        /// </summary>
        public static TV GetCreate<TK, TV>(this IDictionary<TK, TV> self, TK key, Func<TK, TV> creator = null)
        {
            if (!self.TryGetValue(key, out TV value))
            {
                value = (creator != null) ? creator(key) : default(TV);
                self[key] = value;
            }
            return value;
        }

        /// <summary>
        /// Removes the value with the given key from the dictionary and passes it as return argument.
        /// </summary>
        /// <return>The value removed from the dictionary</return>
        /// <exception cref="KeyNotFoundException">if key not found</exception>
        public static TV Pop<TK, TV>(this IDictionary<TK, TV> self, TK key)
        {
            if (self.TryGetValue(key, out TV value))
            {
                self.Remove(key);
                return value;
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Removes the value with the given key from the dictionary and passes it as return argument.
        /// In case the value is not found default(T) will be return.
        /// </summary>
        /// <return>The value removed from the dictionary or default(T)</return>
        public static TV TryPop<TK, TV>(this IDictionary<TK, TV> self, TK key)
        {
            if (self.TryGetValue(key, out TV value))
                self.Remove(key);
            return value;
        }
        
        public static T1v[] CopyToArray<Tk, Tv, T1v>(this IDictionary<Tk, Tv> self, Func<KeyValuePair<Tk, Tv>, T1v> fun)
        {
            var r = new T1v[self.Count];
            var i = 0L;
            foreach (var kvp in self)
                r[i++] = fun(kvp);
            return r;
        }

        public static T1v[] CopyToArray<Tk, Tv, T1v>(this IDictionary<Tk, Tv> self, Func<Tk, Tv, T1v> fun)
        {
            var r = new T1v[self.Count];
            var i = 0L;
            foreach (var kvp in self)
                r[i++] = fun(kvp.Key, kvp.Value);
            return r;
        }

        /// <summary>
        /// Adds a range of KeyValuePairs, will throw DuplicateKeyException
        /// </summary>
        public static Td AddRange<Td, Tk, Tv>(this Td self, IEnumerable<KeyValuePair<Tk, Tv>> keyValuePairs)
            where Td: IDictionary<Tk, Tv>
        {
            foreach (var kvp in keyValuePairs)
                self.Add(kvp.Key, kvp.Value);
            return self;
        }

        /// <summary>
        /// Adds a range of KeyValuePairs as Tup, will throw DuplicateKeyException
        /// </summary>
        public static Td AddRange<Td, Tk, Tv>(this Td self, IEnumerable<Tup<Tk, Tv>> keyValueTuples)
            where Td : IDictionary<Tk, Tv>
        {
            foreach (var kvp in keyValueTuples)
                self.Add(kvp.E0, kvp.E1);
            return self;
        }

        /// <summary>
        /// Adds a range of KeyValuePairs, duplicate keys will be overwritten
        /// </summary>
        public static Td SetRange<Td, Tk, Tv>(this Td self, IEnumerable<KeyValuePair<Tk, Tv>> keyValuePairs)
            where Td : IDictionary<Tk, Tv>
        {
            foreach (var kvp in keyValuePairs)
                self[kvp.Key] = kvp.Value;
            return self;
        }

        /// <summary>
        /// Adds a range of KeyValuePairs, duplicate keys will be overwritten
        /// </summary>
        public static Td SetRange<Td, Tk, Tv>(this Td self, IEnumerable<Tup<Tk, Tv>> keyValueTuples)
            where Td : IDictionary<Tk, Tv>
        {
            foreach (var kvp in keyValueTuples)
                self[kvp.E0] = kvp.E1;
            return self;
        }

        /// <summary>
        /// Compares the key value pairs of two dictionaries using Equals to compare the values.
        /// </summary>
        public static bool DictionaryEquals<Tk, Tv>(this IDictionary<Tk, Tv> self, IDictionary<Tk, Tv> other)
        {
            return DictionaryEquals(self, other, (valueA, valueB) => valueA.Equals(valueB));
        }

        /// <summary>
        /// Compares the key value pairs of two dictionaries using a custom valueComparisonFunc
        /// (which returns true if both are equal).
        /// </summary>
        public static bool DictionaryEquals<Tk, Tv>(this IDictionary<Tk, Tv> self, IDictionary<Tk, Tv> other, Func<Tv, Tv, bool> valueComparisonFunc)
        {
            if (self.Count != other.Count)
                return false;
            else
            {
                foreach (var item in self)
                {
                    Tv valueA = item.Value;

                    if (!other.TryGetValue(item.Key, out Tv valueB) || !valueComparisonFunc(valueA, valueB))
                        return false;

                }
            }
            return true;
        }

// obsolete in netcore -> replaced by System.Collections.Generic.CollectionExtensions
#if !NETCOREAPP3_1_OR_GREATER
        /// <summary>
        /// Returns the value stored with the supplied key or the specified default value if not found.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue default_value)
        {
            return self.TryGetValue(key, out var result) ? result : default_value;
        }
#endif
    }

    #endregion

    #region KeyValuePair Extensions

    public static class KeyValuePairs
    {
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(
                TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        public static KeyValuePair<TKey, TValue2> Copy<TKey, TValue1, TValue2>(
                this KeyValuePair<TKey, TValue1> kvp, Func<TValue1, TValue2> func)
        {
            return new KeyValuePair<TKey, TValue2>(kvp.Key, func(kvp.Value));
        }
    }

    #endregion
}

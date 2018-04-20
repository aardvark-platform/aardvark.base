using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public interface IIntCountable
    {
        int Count { get; }
    }

    public interface ICountable
    {
        long LongCount { get; }
    }

    public interface IDict
    {
        Type KeyType { get; }
        Type ValueType { get; }

        IEnumerable<(object, object)> ObjectPairs { get; }

        void AddObject(object key, object value);
        void Clear();
    }

    public interface ICountableDict : ICountable, IDict
    {
    }

    public interface IDict<TKey, TValue>
    {
        IEnumerable<TKey> Keys { get; }
        IEnumerable<TValue> Values { get; }
        IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs { get; }
        TValue this[TKey key] { get; set; }
        void Add(TKey key, TValue value);
        bool ContainsKey(TKey key);
        bool Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }

    public interface IDictDepth
    {
        int Depth { get; }
    }


    public interface IDictSet
    {
        Type KeyType { get; }

        IEnumerable<object> Objects { get; }

        bool AddObject(object obj);
        void Clear();
    }

    public interface ICountableDictSet : ICountable, IDictSet
    {
    }

    public interface IDictSet<TKey>
    {
        IEnumerable<TKey> Items { get; }
        bool Add(TKey item);
        bool Contains(TKey item);
        bool Remove(TKey item);
    }
}

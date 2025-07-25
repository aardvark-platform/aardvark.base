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

namespace Aardvark.Base;

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

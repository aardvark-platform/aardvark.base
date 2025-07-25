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
using System.Collections;
using System.Collections.Generic;

namespace Aardvark.Base.Coder;

/// <summary>
/// An ICoder can either read or write objects. It declares its purpose
/// with the methods <see cref="IsReading"/> and <see cref="IsWriting"/>
/// one of which must return true the other one false. Depending on its
/// declared purpose all its <see cref="o:Code"/> methods either read or
/// write objects via the supplied reference.
/// </summary>
public partial interface ICoder
{
    int CoderVersion { get; }
    
    /// <summary>
    /// The version of the current object in memory.
    /// </summary>
    int MemoryVersion { get; }

    /// <summary>
    /// For writing coders this is the same as the memory version, for
    /// reading coders, an object of the stream version has to be
    /// converted to the memory version.
    /// </summary>
    int StreamVersion { get; }

    /// <summary>
    /// If true all the <see cref="o:Code"/> methods read objects.
    /// </summary>
    bool IsReading { get; }

    /// <summary>
    /// If true all the <see cref="o:Code"/> methods write objects.
    /// </summary>
    bool IsWriting { get; }

    string FileName { get; }
    
    void Add(TypeInfo[] typeInfoArray);
    void Del(TypeInfo[] typeInfoArray);

    void Code(ref object obj);

    void CodeT<T>(ref T obj);
    void CodeTArray<T>(ref T[] array);
    void CodeList_of_T_<T>(ref List<T> list);
    void CodeHashSet_of_T_<T>(ref HashSet<T> set);

    void Code(Type t, ref object o);

    void Code(Type t, ref Array array);
    void Code(Type t, ref IList list);
    void Code(Type t, ref IDictionary dict);

    void Code(Type t, ref ICountableDict dict);
    void Code(Type t, ref ICountableDictSet dictSet);

    void Code(Type t, ref IArrayVector vector);
    void Code(Type t, ref IArrayMatrix matrix);
    void Code(Type t, ref IArrayVolume volume);
    void Code(Type t, ref IArrayTensor4 tensor4);
    void Code(Type t, ref IArrayTensorN tensor);

    void CodeHashSet(Type t, ref object o);

    void CodeEnum(Type t, ref object value);

    /// <summary>
    /// Code a symbol that is known to be a Guid.
    /// </summary>
    void CodeGuidSymbol(ref Symbol v);

    /// <summary>
    /// Code a Symbol that is known to be positive.
    /// </summary>
    void CodePositiveSymbol(ref Symbol v);

    void CodeIntSet(ref IntSet v);
    void CodeSymbolSet(ref SymbolSet v);

    void CodeStructArray<T>(ref T[] a) where T : struct;
    void CodeStructList<T>(ref List<T> l) where T : struct;
}

/// <summary>
/// This interface supplies additional functionality that is provided
/// by reading coders.
/// </summary>
public interface IReadingCoder : ICoder
{
    int CodeCount<T>(ref T value, Func<int, T> creator) where T : class;
}

/// <summary>
/// This interface supplies additional functionality that is provided
/// by writing coders.
/// </summary>
public interface IWritingCoder : ICoder
{
    int CodeCount<T>(ref T value, Func<T, int> counter) where T : class;
}

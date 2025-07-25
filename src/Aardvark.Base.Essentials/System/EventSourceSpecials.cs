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

namespace Aardvark.Base;

/// <summary>
/// </summary>
public class ConstEventSource
{
    /// <summary>
    /// Creates new ConstEventSource with given value.
    /// </summary>
    public static ConstEventSource<T> Create<T>(T value)
    {
        return new ConstEventSource<T>(value);
    }

    /// <summary>
    /// Creates new ConstEventSource with tuple of given values.
    /// </summary>
    public static ConstEventSource<Tuple<T0, T1>> Create<T0, T1>(T0 initialValue0, T1 initialValue1)
    {
        return new ConstEventSource<Tuple<T0, T1>>(Tuple.Create(initialValue0, initialValue1));
    }

    /// <summary>
    /// Creates new ConstEventSource with tuple of given values.
    /// </summary>
    public static ConstEventSource<Tuple<T0, T1, T2>> Create<T0, T1, T2>(T0 initialValue0, T1 initialValue1, T2 initialValue2)
    {
        return new ConstEventSource<Tuple<T0, T1, T2>>(Tuple.Create(initialValue0, initialValue1, initialValue2));
    }

    /// <summary>
    /// Creates new ConstEventSource with tuple of given values.
    /// </summary>
    public static ConstEventSource<Tuple<T0, T1, T2, T3>> Create<T0, T1, T2, T3>(T0 initialValue0, T1 initialValue1, T2 initialValue2, T3 initialValue3)
    {
        return new ConstEventSource<Tuple<T0, T1, T2, T3>>(Tuple.Create(initialValue0, initialValue1, initialValue2, initialValue3));
    }

    /// <summary>
    /// Creates new ConstEventSource with tuple of given values.
    /// </summary>
    public static ConstEventSource<Tuple<T0, T1, T2, T3, T4>> Create<T0, T1, T2, T3, T4>(T0 initialValue0, T1 initialValue1, T2 initialValue2, T3 initialValue3, T4 initialValue4)
    {
        return new ConstEventSource<Tuple<T0, T1, T2, T3, T4>>(Tuple.Create(initialValue0, initialValue1, initialValue2, initialValue3, initialValue4));
    }
}

/// <summary>
/// A ConstEventSource has its initial value and will never emit new values.
/// </summary>
public class ConstEventSource<T>(T constValue) : IEvent<T>
{
    private static readonly Awaitable s_awaitableNonGeneric = new();
    private static readonly Awaitable<T> s_awaitable = new();
    private readonly T m_value = constValue;

    /// <summary>
    /// </summary>
    public T Latest
    {
        get { return m_value; }
    }

    /// <summary>
    /// </summary>
    public IAwaitable<T> Next
    {
        get { return s_awaitable; }
    }

    /// <summary>
    /// </summary>
    public IObservable<T> Values
    {
        get { return new NeverObservable<T>(); }
    }

    IAwaitable IEvent.Next
    {
        get { return s_awaitableNonGeneric; }
    }

    IObservable<Unit> IEvent.Values
    {
        get { return new NeverObservable<Unit>(); }
    }
}

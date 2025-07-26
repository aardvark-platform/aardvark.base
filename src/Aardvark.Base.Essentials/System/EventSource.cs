﻿/*
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
using System.Threading;

namespace Aardvark.Base;

public class EventSourceSlim<T> : IEvent<T>
{
    private readonly Subject<T> m_subject;
    private T m_latest;
    public EventSourceSlim(T defaultValue)
    {
        m_subject = new Subject<T>();
        m_subject.OnNext(defaultValue);
        m_latest = defaultValue;
    }

    public void Emit(T v)
    {
        lock(this)
        {
            m_latest = v;
            m_subject.OnNext(v);
        }
    }

    public T Latest
    {
        get
        {
            lock(this)
            {
                return m_latest;
            }
        }
    }

    public IAwaitable<T> Next
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IObservable<T> Values
    {
        get
        {
            return m_subject;
        }
    }

    IAwaitable IEvent.Next
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    IObservable<Unit> IEvent.Values
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}

/// <summary>
/// </summary>
public static class EventSource
{
    /// <summary>
    /// The type used for the Values property in non-generic event sources.
    /// </summary>
    public static readonly Type UnitEventType = typeof(Unit);

    /// <summary>
    /// Creates new EventSource with given initial value.
    /// </summary>
    public static EventSource<T> Create<T>(T initialValue)
    {
        return new EventSource<T>(initialValue);
    }

    /// <summary>
    /// Creates new EventSource with tuple of given initial values.
    /// </summary>
    public static EventSource<Tuple<T0, T1>> Create<T0, T1>(T0 initialValue0, T1 initialValue1)
    {
        return new EventSource<Tuple<T0, T1>>(Tuple.Create(initialValue0, initialValue1));
    }

    /// <summary>
    /// Creates new EventSource with tuple of given initial values.
    /// </summary>
    public static EventSource<Tuple<T0, T1, T2>> Create<T0, T1, T2>(T0 initialValue0, T1 initialValue1, T2 initialValue2)
    {
        return new EventSource<Tuple<T0, T1, T2>>(Tuple.Create(initialValue0, initialValue1, initialValue2));
    }

    /// <summary>
    /// Creates new EventSource with tuple of given initial values.
    /// </summary>
    public static EventSource<Tuple<T0, T1, T2, T3>> Create<T0, T1, T2, T3>(T0 initialValue0, T1 initialValue1, T2 initialValue2, T3 initialValue3)
    {
        return new EventSource<Tuple<T0, T1, T2, T3>>(Tuple.Create(initialValue0, initialValue1, initialValue2, initialValue3));
    }

    /// <summary>
    /// Creates new EventSource with tuple of given initial values.
    /// </summary>
    public static EventSource<Tuple<T0, T1, T2, T3, T4>> Create<T0, T1, T2, T3, T4>(T0 initialValue0, T1 initialValue1, T2 initialValue2, T3 initialValue3, T4 initialValue4)
    {
        return new EventSource<Tuple<T0, T1, T2, T3, T4>>(Tuple.Create(initialValue0, initialValue1, initialValue2, initialValue3, initialValue4));
    }
}

/// <summary>
/// </summary>
public class EventSource<T> : IEvent<T>, IEventEmitter<T>
{
    private readonly ThreadLocal<bool> m_currentThreadDoesNotOwnLock = new(() => true);
    private SpinLock m_lock = new(true);
    private T m_latest;
    private Awaitable<T> m_awaitable = new();
    private readonly Lazy<Subject<T>> m_eventStream = new();

    /// <summary>
    /// Creates an EventSource with default initial value.
    /// </summary>
    public EventSource()
    {
        EventSourceTelemetry.CountConstructorDefault.Increment();
    }

    /// <summary>
    /// Creates a new EventSource with specified initial value.
    /// </summary>
    public EventSource(T initialValue)
    {
        EventSourceTelemetry.CountConstructorInitialValue.Increment();
        m_latest = initialValue;
    }

    /// <summary>
    /// Wraps an observable as an event source.
    /// </summary>
    public EventSource(IObservable<T> fromObservable)
    {
        EventSourceTelemetry.CountConstructorFromObservable.Increment();
        fromObservable.Subscribe(new LambdaObserver<T>(x => Emit(x)));
    }

    /// <summary>
    /// Wraps an observable as an event source and sets its initial value.
    /// </summary>
    public EventSource(T initialValue, IObservable<T> fromObservable)
    {
        m_latest = initialValue;
        EventSourceTelemetry.CountConstructorInitialValue.Increment();
        EventSourceTelemetry.CountConstructorFromObservable.Increment();
        fromObservable.Subscribe(new LambdaObserver<T>(x => Emit(x)));
    }

    /// <summary>
    /// Gets latest value emitted by this event source,
    /// or default(T) if no value has been emitted so far.
    /// Do not set this value! Unless you have multiple
    /// related event sources you want to 'update'
    /// simultanously. In this case first set Latest for
    /// all related event sources to their respective new
    /// value, and then Emit the same values. A subscriber
    /// of such an event source which accesses the Latest
    /// property of a related event source then sees the
    /// correct value (independent of emit order).
    /// </summary>
    public T Latest
    {
        get
        {
            EventSourceTelemetry.CountLatestGet.Increment();
            return m_latest;
        }
        set
        {
            m_latest = value;
        }
    }

    /// <summary>
    /// Gets next value that will be emitted by this event source.
    /// </summary>
    public IAwaitable<T> Next
    {
        get
        {
            EventSourceTelemetry.CountNextGet.Increment();

            bool lockTaken = false;
            try
            {
                if (m_currentThreadDoesNotOwnLock.Value)
                {
                    m_lock.Enter(ref lockTaken);
                    m_currentThreadDoesNotOwnLock.Value = false;
                }
                return m_awaitable;
            }
            finally
            {
                if (lockTaken)
                {
                    m_currentThreadDoesNotOwnLock.Value = true;
                    m_lock.Exit(true);
                }
            }
        }
    }

    /// <summary>
    /// Gets observable stream of values emitted by this event source.
    /// </summary>
    public IObservable<T> Values
    {
        get
        {
            EventSourceTelemetry.CountValuesGet.Increment();
            return m_eventStream.Value;
        }
    }

    /// <summary>
    /// Emits given value from this event source.
    /// </summary>
    public virtual void Emit(T value)
    {
        EventSourceTelemetry.CountEmit.Increment();

        bool lockTaken = false;
        try
        {
            if (m_currentThreadDoesNotOwnLock.Value)
            {
                m_currentThreadDoesNotOwnLock.Value = false;
                m_lock.Enter(ref lockTaken);
            }

            var currentAwaitable = m_awaitable;

            // update Latest and Next
            // (before emitting, such that a listener immediately coming back to us already sees the updated state)
            m_awaitable = new Awaitable<T>();
            m_latest = value;

            // emit value
            currentAwaitable.Emit(value);
            if (m_eventStream.IsValueCreated) m_eventStream.Value.OnNext(value);
        }
        finally
        {
            if (lockTaken)
            {
                m_currentThreadDoesNotOwnLock.Value = true;
                m_lock.Exit(true);
            }
        }
    }

    #region IEvent

    /// <summary>
    /// Observable notifications for all values that are emitted.
    /// </summary>
    IObservable<Unit> IEvent.Values
    {
        get { return new MapObservable<T, Unit>(Values, _ => Unit.Default); }
    }

    IAwaitable IEvent.Next
    {
        get { return Next; }
    }

    #endregion

    #region IEventEmitter

    /// <summary>
    /// Emits default T.
    /// </summary>
    public void Emit()
    {
        Emit(default);
    }

    #endregion
}

internal static class EventSourceTelemetry
{
    public static readonly Telemetry.Counter CountConstructorDefault = new();
    public static readonly Telemetry.Counter CountConstructorInitialValue = new();
    public static readonly Telemetry.Counter CountConstructorFromObservable = new();
    public static readonly Telemetry.Counter CountEmit = new();
    public static readonly Telemetry.Counter CountNextGet = new();
    public static readonly Telemetry.Counter CountLatestGet = new();
    public static readonly Telemetry.Counter CountValuesGet = new();
}

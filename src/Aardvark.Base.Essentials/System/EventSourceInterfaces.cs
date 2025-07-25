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
/// The receiving side of an event source.
/// </summary>
public interface IEvent<T> : IEvent
{
    /// <summary>
    /// The latest value that has been emitted.
    /// This will return default(T) if no value has been emitted yet. 
    /// </summary>
    T Latest { get; }

    /// <summary>
    /// The next value that will be emitted.
    /// </summary>
    new IAwaitable<T> Next { get; }

    /// <summary>
    /// Observable sequence of emitted values.
    /// </summary>
    new IObservable<T> Values { get; }
}

/// <summary>
/// The sending side of an event source.
/// </summary>
public interface IEventEmitter<T> : IEventEmitter
{
    /// <summary>
    /// Pushes next event value.
    /// </summary>
    void Emit(T value);
}

/// <summary>
/// The receiving side of an event source.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// The next value that will be emitted.
    /// </summary>
    IAwaitable Next { get; }

    /// <summary>
    /// Observable notifications for all values that are emitted.
    /// </summary>
    IObservable<Unit> Values { get; }
}

/// <summary>
/// The sending side of an event source.
/// </summary>
public interface IEventEmitter
{
    /// <summary>
    /// Pushes next event.
    /// </summary>
    void Emit();
}

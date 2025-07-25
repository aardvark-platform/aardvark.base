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
using System.Runtime.CompilerServices;

namespace Aardvark.Base;

//These are the interfaces needed to be awaitable (which are not exposed by the .NET)
//Here's a little example to make it easier to understand its behaviour.
//
//  Consider the follwing function:
//
//    async void Foo()
//    {
//        await x;
//        Bar();
//    }
//
//  Which will be translated to something like (it's actually more complicated but this will give you the basic idea)
//  Basically all the continuations call Foo itself and Foo contains something like a jump table (at least using the current .NET framework)
//
//    async void Foo()
//    {
//        var xAwaiter = x.GetAwaiter();
//        if(xAwaiter.IsCompleted) Bar();
//        else
//        {
//            xAwaiter.OnCompleted(() =>
//            {
//                xAwaiter.GetResult();
//                Bar();
//            }
//        }
//    }

/// <summary>
/// Represents the required interface for using the async/await syntax.
/// every class implementing IAwaitable can be 'awaited' like tasks
/// for an awaitable returning a value see IAwaitable[T].
/// </summary>
public interface IAwaitable
{
    /// <summary>
    /// </summary>
    IAwaiter GetAwaiter();

    /// <summary>
    /// </summary>
    bool IsCompleted { get; }
}

/// <summary>
/// </summary>
public interface IAwaiter : INotifyCompletion
{
    /// <summary>
    /// When IsCompleted returns true the the awaiting code will simply keep running without subscribing itself
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// GetResult is always the first function called in the continuation (even for void) 
    /// and is therefore the only point where one can throw exceptions which will occur in user-code
    /// </summary>
    void GetResult();
}

/// <summary>
/// represents the required interface for using the async/await syntax
/// every class implementing IAwaitable can be 'awaited' like tasks
/// for an awaitable returning no value see IAwaitable
/// </summary>
public interface IAwaitable<T> : IAwaitable
{
    /// <summary>
    /// </summary>
    new IAwaiter<T> GetAwaiter();

    /// <summary>
    /// </summary>
    T Result { get; }
}

/// <summary>
/// </summary>
public interface IAwaiter<T> : INotifyCompletion, IAwaiter
{
    /// <summary>
    /// GetResult is always the first function called in the continuation (even for void) 
    /// and is therefore the only point where one can throw exceptions which will occur in user-code
    /// </summary>
    new T GetResult();
}

/// <summary>
/// </summary>
public static class IAwaitableExtensions
{
    /// <summary>
    /// </summary>
    public static void Subscribe<T>(this IAwaitable<T> awaitable, Action<T> action)
    {
        var awaiter = awaitable.GetAwaiter();
        awaiter.OnCompleted(() => { try { action(awaiter.GetResult()); } catch (OperationCanceledException) { } });
    }

    /// <summary>
    /// </summary>
    public static void Subscribe(this IAwaitable awaitable, Action action)
    {
        var awaiter = awaitable.GetAwaiter();
        awaiter.OnCompleted(() => { try { awaiter.GetResult(); action(); } catch (OperationCanceledException) { } });
    }
}

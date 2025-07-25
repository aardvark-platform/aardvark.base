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
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Base;

/// <summary>
/// Implementation based on
/// http://blogs.msdn.com/b/dotnet/archive/2013/04/04/net-memory-allocation-profiling-with-visual-studio-2012.aspx
/// http://blogs.msdn.com/b/pfxteam/archive/2012/10/05/how-do-i-cancel-non-cancelable-async-operations.aspx
/// </summary>
public static class TaskWithCancellationExtensions
{
    /// <summary>
    /// Creates a task that completes (or cancels) when either the input task completes or the cancellation token is signalled.
    /// On cancellation, the original task still runs to completion because there is no way to preemptively cancel it.
    /// </summary>
    public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken ct)
    {
        if (task.IsCompleted || !ct.CanBeCanceled)
            return task;
        else if (ct.IsCancellationRequested)
            return new Task<T>(() => default, ct);
        else
            return task.WithCancellationInternal(ct);
    }

    /// <summary>
    /// Creates a task that completes (or cancels) when either the input task completes or the cancellation token is signalled.
    /// On cancellation, the original task still runs to completion because there is no way to preemptively cancel it.
    /// </summary>
    public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken? ct)
    {
        if (task.IsCompleted || !ct.HasValue || !ct.Value.CanBeCanceled)
            return task;
        else if (ct.Value.IsCancellationRequested)
            return new Task<T>(() => default, ct.Value);
        else
            return task.WithCancellationInternal(ct.Value);
    }


    private static readonly Action<object> s_cancellationRegistration =
        s => ((TaskCompletionSource<bool>)s).TrySetResult(true);

    private static async Task<T> WithCancellationInternal<T>(this Task<T> task, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<bool>();
        using (ct.Register(s_cancellationRegistration, tcs))
        {
            if (task != await Task.WhenAny(task, tcs.Task))
            {
                //TODO: fixed by haaser (check SM)
                throw new TaskCanceledException(task);

                //prior: (wrong since all Controllers check for TaskCanceledException)
                //throw new OperationCanceledException(ct);
            }
        }
        return await task;
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Base
{
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
                return new Task<T>(() => default(T), ct);
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
                return new Task<T>(() => default(T), ct.Value);
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
}

using System;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    /// <summary>
    /// </summary>
    public static class EventSourceExtensions
    {
        /// <summary>
        /// Wraps an observable as an event source.
        /// </summary>
        public static EventSource<T> ToEventSource<T>(this IObservable<T> self)
        {
            return new EventSource<T>(self);
        }

        /// <summary>
        /// Executes action when next value arrives.
        /// </summary>
        public static void ExecuteOnNextValue<T>(this IEvent<T> self, Action<T> action)
        {
            self.Next.ContinueWith(t => action(t));
        }

        /// <summary>
        /// Executes action when next value arrives.
        /// </summary>
        public static void ExecuteOnNextValue<T>(this IEvent<T> self, Action action)
        {
            self.Next.ContinueWith(t => action());
        }

        /// <summary>
        /// Repeats given action until the next value of this event source arrives.
        /// The optional final action gets called with this newly arrived value. 
        /// </summary>
        public static async Task RepeatUntilNext<T>(this IEvent<T> self, Func<Task> action, Func<T, Task> finallyAction = null)
        {
            var next = self.Next;
            while (!next.IsCompleted)
            {
                await action();
            }

            if (finallyAction != null)
                await finallyAction(next.Result);
        }

        /// <summary>
        /// Repeats given action until the next value of this event source arrives.
        /// The optional final action gets called with this newly arrived value. 
        /// </summary>
        public static async Task RepeatUntilNext<T>(this IEvent<T> self, Func<Task> action, Action<T> finallyAction)
        {
            var next = self.Next;
            while (!next.IsCompleted)
            {
                await action();
            }

            if (finallyAction != null)
                finallyAction(next.Result);
        }

        /// <summary>
        /// Repeats given action until the given task/awaitable completes.
        /// The optional final action gets called with the task's result. 
        /// </summary>
        public static async Task RepeatUntilCompleted<T>(this IAwaitable<T> self, Func<Task> action, Func<T, Task> finallyAction = null)
        {
            while (!self.IsCompleted)
            {
                await action();
            }

            if (finallyAction != null)
                await finallyAction(self.Result);
        }

        /// <summary>
        /// Repeats given action until the given task/awaitable completes.
        /// The optional final action gets called with the task's result. 
        /// </summary>
        public static async Task RepeatUntilCompleted<T>(this IAwaitable<T> self, Func<Task> action, Action<T> finallyAction)
        {
            while (!self.IsCompleted)
            {
                await action();
            }

            if (finallyAction != null)
                finallyAction(self.Result);
        }

        /// <summary>
        /// Repeats given action until the given task/awaitable completes.
        /// </summary>
        public static async Task RepeatUntilCompleted(this IAwaitable self, Func<Task> action, Func<Task> finallyAction = null)
        {
            while (!self.IsCompleted)
            {
                await action();
            }

            if (finallyAction != null)
                await finallyAction();
        }

        /// <summary>
        /// Repeats given action until the given task/awaitable completes.
        /// </summary>
        public static async Task RepeatUntilCompleted(this IAwaitable self, Func<Task> action, Action finallyAction)
        {
            while (!self.IsCompleted)
            {
                await action();
            }

            if (finallyAction != null)
                finallyAction();
        }
    }
}

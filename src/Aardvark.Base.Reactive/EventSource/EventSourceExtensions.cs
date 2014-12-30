using System;

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
    }
}

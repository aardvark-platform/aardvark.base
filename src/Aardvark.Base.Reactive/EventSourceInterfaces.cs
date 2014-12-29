using System;

namespace Aardvark.Base
{
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
        IObservable<UnitEvent> Values { get; }
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

    /// <summary>
    /// See System.Reactive.Unit.
    /// </summary>
    public struct UnitEvent : IEquatable<UnitEvent>
    {
        /// <summary>
        /// </summary>
        public static UnitEvent Default { get { return default(UnitEvent); } }

        /// <summary>
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is UnitEvent;
        }

        /// <summary>
        /// </summary>
        public bool Equals(UnitEvent other)
        {
            return true;
        }

        /// <summary>
        /// </summary>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// </summary>
        public static bool operator ==(UnitEvent first, UnitEvent second)
        {
            return true;
        }

        /// <summary>
        /// </summary>
        public static bool operator !=(UnitEvent first, UnitEvent second)
        {
            return false;
        }
    }
}

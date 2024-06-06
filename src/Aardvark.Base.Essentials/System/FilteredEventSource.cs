using System;

namespace Aardvark.Base
{
    /// <summary>
    /// </summary>
    public class FilteredEventSource<T> : EventSource<T>
    {
        private readonly Func<T, bool> m_predicate;

        /// <summary>
        /// </summary>
        public FilteredEventSource(Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException();
            m_predicate = predicate;
        }

        /// <summary>
        /// </summary>
        public override void Emit(T value)
        {
            if (m_predicate(value)) base.Emit(value);
        }
    }
}

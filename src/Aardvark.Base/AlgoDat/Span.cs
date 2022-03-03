using System;

namespace Aardvark.Base
{
    public static class SpanPinning
    {
        /// <summary>
        /// Pins a span and performs the given action with the resulting pointer.
        /// This method is required due to missing support for pinning spans in F#.
        /// </summary>
        /// <param name="span">The span to pin.</param>
        /// <param name="action">The action to perform.</param>
        /// <returns>The result of the action.</returns>
        public unsafe static Result Pin<T, Result>(Span<T> span, Func<IntPtr, Result> action) where T : unmanaged
        {
            fixed (T* ptr = span)
            {
                return action((IntPtr)ptr);
            }
        }
    }
}
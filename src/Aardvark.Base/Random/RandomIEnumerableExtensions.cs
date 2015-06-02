using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class RandomIEnumerableExtensions
    {
        /// <summary>
        /// Yields each element with propability p.
        /// </summary>
        public static IEnumerable<T> TakeRandomly<T>(this IEnumerable<T> self, double p,
                                                     IRandomUniform random = null)
        {
            if (self == null) throw new ArgumentNullException();
            if (p < 0 || p > 1) throw new ArgumentOutOfRangeException();

            if (random == null) random = new RandomSystem();
            foreach (var s in self) if (random.UniformDouble() <= p) yield return s;
        }

        /// <summary>
        /// Yields each element with propability p.
        /// </summary>
        public static IEnumerable<R> TakeRandomly<T, R>(this IEnumerable<T> self, Func<T, R> selector, double p,
                                                        IRandomUniform random = null)
        {
            if (self == null) throw new ArgumentNullException();
            if (p < 0 || p > 1) throw new ArgumentOutOfRangeException();

            if (random == null) random = new RandomSystem();
            foreach (var s in self) if (random.UniformDouble() <= p) yield return selector(s);
        }
    }
}

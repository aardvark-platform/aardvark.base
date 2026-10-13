using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class RandomIEnumerableExtensions
    {
        /// <summary>
        /// Yields each element when its uniform draw in [0, 1) is strictly less
        /// than p. Draws once per visited element, including at p = 0 and p = 1.
        /// </summary>
        public static IEnumerable<T> TakeRandomly<T>(this IEnumerable<T> self, double p,
                                                     IRandomUniform random = null)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (double.IsNaN(p) || p < 0 || p > 1) throw new ArgumentOutOfRangeException(nameof(p));

            if (random == null) random = new RandomSystem();
            foreach (var s in self) if (random.UniformDouble() < p) yield return s;
        }

        /// <summary>
        /// Yields selector results only for elements whose uniform draw in
        /// [0, 1) is strictly less than p. Draws once per visited element,
        /// including at p = 0 and p = 1, and preserves source order.
        /// </summary>
        public static IEnumerable<R> TakeRandomly<T, R>(this IEnumerable<T> self, Func<T, R> selector, double p,
                                                        IRandomUniform random = null)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (double.IsNaN(p) || p < 0 || p > 1) throw new ArgumentOutOfRangeException(nameof(p));

            if (random == null) random = new RandomSystem();
            foreach (var s in self) if (random.UniformDouble() < p) yield return selector(s);
        }
    }
}

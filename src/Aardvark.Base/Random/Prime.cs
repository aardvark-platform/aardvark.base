using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides primality testing and a lazily expanded table of indexed prime numbers.
    /// </summary>
    public static class Prime
    {
        const int c_initialPrimeCount = 64;
        const int c_maxPrimeCount = 1 << 24;

        /// <summary>
        /// Tests whether <paramref name="value"/> is a prime number.
        /// </summary>
        /// <returns><c>true</c> exactly for prime values; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrueFor(long value) => Fun.IsPrime(value);

        /// <summary>
        /// Returns the prime number with the supplied zero-based index. The first
        /// prime number is 2, so <c>Prime.WithIndex(0) == 2</c>. Cached lookups are
        /// lock-free and table growth is thread-safe.
        /// </summary>
        /// <param name="primeIndex">The zero-based prime index.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WithIndex(int primeIndex)
        {
            if (primeIndex >= Volatile.Read(ref primeCount))
                CalculateUpToIndex(primeIndex);
            return primeArray[primeIndex];
        }

        /// <summary>
        /// Returns the inverse of the prime number with the supplied zero-based
        /// index. The first prime number is 2, so
        /// <c>Prime.InverseWithIndex(0) == 0.5</c>. Cached lookups are lock-free and
        /// observe the same published table prefix as indexed prime lookups.
        /// </summary>
        /// <param name="primeIndex">The zero-based prime index.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double InverseWithIndex(int primeIndex)
        {
            if (primeIndex >= Volatile.Read(ref primeCount))
                CalculateUpToIndex(primeIndex);
            return primeInverseArray[primeIndex];
        }

        private static int candidate = 5;
        private static int root = 3;
        private static int square = 9;
        private static int step = 2;

        private static void CalculateUpToIndex(int last)
        {
            lock (growthLock)
            {
                int count = Volatile.Read(ref primeCount);
                if (last < count) return;

                if (last >= c_maxPrimeCount)
                    throw new ArgumentException("exceeded prime table size limit");

                int capacity = primeArray.Length;
                int[] primes = primeArray;
                double[] inverses = primeInverseArray;
                if (last >= capacity)
                {
                    while (last >= capacity) capacity *= 2;
                    primes = new int[capacity];
                    inverses = new double[capacity];
                    Array.Copy(primeArray, primes, count);
                    Array.Copy(primeInverseArray, inverses, count);
                }

                while (count <= last)
                {
                    for (bool found = false; !found; )
                    {
                        candidate += step;
                        step = 6 - step;
                        if (candidate > square)
                        {
                            ++root;
                            square = root * root;
                        }

                        found = true;
                        for (int pi = 2; primes[pi] <= root; pi++)
                        {
                            if (candidate % primes[pi] == 0)
                            {
                                found = false;
                                break;
                            }
                        }
                    }

                    primes[count] = candidate;
                    inverses[count] = 1.0 / candidate;
                    ++count;
                }

                primeArray = primes;
                primeInverseArray = inverses;
                Volatile.Write(ref primeCount, count);
            }
        }

        static Prime()
        {
            primeArray[0] = 2;
            primeArray[1] = 3;
            primeArray[2] = 5;
            primeInverseArray[0] = 1.0 / 2.0;
            primeInverseArray[1] = 1.0 / 3.0;
            primeInverseArray[2] = 1.0 / 5.0;

            primeCount = 3;
            CalculateUpToIndex(c_initialPrimeCount - 1);
        }

        private static readonly object growthLock = new object();
        private static int primeCount;
        private static int[] primeArray = new int[c_initialPrimeCount];
        private static double[] primeInverseArray = new double[c_initialPrimeCount];
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    public static class Prime
    {
        const int c_initialPrimeCount = 64;
        const int c_maxPrimeCount = 1 << 24;

        public static bool IsTrueFor(long value)
        {
            if (value < 0) return false;
            if (value < isPrimeArray.Length) return isPrimeArray[value];

            int root = (int)Fun.Sqrt(value);
            int directMaxPrime = Fun.Min(primeArray[primeCount - 1], root);
            int pi;
            for (pi = 0; primeArray[pi] <= directMaxPrime; pi++)
                if (value % primeArray[pi] == 0) return false;

            if (directMaxPrime == root) return true;

            while (true)
            {
                int p = Prime.WithIndex(pi);
                if (p > root) break;
                if (value % p == 0) return false;
                pi++;
            }
            return true;
        }

        /// <summary>
        /// Returns the prime number with the supplied index. Indices start at
        /// zero and the first prime number is 2. Thus
        /// 'Prime.WithIndex(0) == 2'.
        /// </summary>
        public static int WithIndex(int primeIndex)
        {
            if (primeIndex >= primeCount) CalculateUpToIndex(primeIndex);
            return primeArray[primeIndex];
        }

        /// <summary>
        /// Returns the inverse of the prime number with the supplied index.
        /// Indices start at zero and the first prime number is 2. Thus
        /// 'Prime.InverseWithIndex(0) == 0.5'.
        /// </summary>
        public static double InverseWithIndex(int primeIndex)
        {
            if (primeIndex >= primeCount) CalculateUpToIndex(primeIndex);
            return primeInverseArray[primeIndex];
        }

        /// <summary>
        /// The number of primes that have already been calculated.
        /// </summary>
        private static int primeCount;

        private static int candidate = 5;
        private static int root = 3;
        private static int square = 9;
        private static int step = 2;

        private static int logIndexCount = 3;
        private static int logIndexTrigger = 8;

        private static void CalculateUpToIndex(int last)
        {
            if (last >= c_maxPrimeCount)
                throw new ArgumentException("exceeded prime table size limit");
            if (last >= primeCapacity)
            {
                while (last >= primeCapacity) primeCapacity *= 2;
                Array.Resize(ref primeArray, primeCapacity);
                Array.Resize(ref primeInverseArray, primeCapacity);
            }

            while (primeCount <= last)
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
	                for (int pi = 2; primeArray[pi] <= root; pi++)
		                if (candidate % primeArray[pi] == 0)
		                {
		                    found = false; break;
		                }
	            }
                if (candidate > logIndexTrigger)
                {
                    primeLogIndexArray[logIndexCount++] = primeCount;
                    logIndexTrigger *= 2;
                }
	            primeArray[primeCount] = candidate;
	            primeInverseArray[primeCount] = 1.0/(double)candidate;
	            ++primeCount;
            }
        }

        static Prime()
        {
            primeCapacity = c_initialPrimeCount;

            primeArray[0] = 2;
            primeArray[1] = 3;
            primeArray[2] = 5;
            primeInverseArray[0] = 1/2.0;
            primeInverseArray[1] = 1/3.0;
            primeInverseArray[2] = 1/5.0;

            primeLogIndexArray[0] = -1;
            primeLogIndexArray[1] = 0;
            primeLogIndexArray[2] = 2;

            primeCount = 3;

            CalculateUpToIndex(c_initialPrimeCount - 1);

            int isPrimeCount = primeArray[c_initialPrimeCount - 1] + 1;
            isPrimeArray = new bool[isPrimeCount];
            for (int pi = 0; pi < primeCount; pi++)
                isPrimeArray[primeArray[pi]] = true;
        }

        private static int primeCapacity;
        private static int[] primeArray = new int[c_initialPrimeCount];
        private static double[] primeInverseArray = new double[c_initialPrimeCount];
        private static bool[] isPrimeArray;
        private static int[] primeLogIndexArray = new int[64];
    }
}

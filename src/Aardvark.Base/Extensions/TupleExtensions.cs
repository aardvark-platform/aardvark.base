using System;

namespace Aardvark.Base
{
    public static class TupleExtensions
    {
        /// <summary>
        /// Number of NaNs in tuple.
        /// </summary>
        public static int CountNonNaNs(this (double, double) p)
        {
            int count = 2;
            if (double.IsNaN(p.Item1)) --count;
            if (double.IsNaN(p.Item2)) --count;
            return count;
        }

        /// <summary>
        /// Number of NaNs in tuple.
        /// </summary>
        public static int CountNonNaNs(this (double, double, double) p)
        {
            int count = 3;
            if (double.IsNaN(p.Item1)) --count;
            if (double.IsNaN(p.Item2)) --count;
            if (double.IsNaN(p.Item3)) --count;
            return count;
        }

        /// <summary>
        /// Number of NaNs in tuple.
        /// </summary>
        public static int CountNonNaNs(this (double, double, double, double) p)
        {
            int count = 4;
            if (double.IsNaN(p.Item1)) --count;
            if (double.IsNaN(p.Item2)) --count;
            if (double.IsNaN(p.Item3)) --count;
            if (double.IsNaN(p.Item4)) --count;
            return count;
        }

        /// <summary>
        /// Gets i-th value of tuple.
        /// </summary>
        public static double Get(this (double, double) p, int i)
        {
            switch (i)
            {
                case 0: return p.Item1;
                case 1: return p.Item2;
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets i-th value of tuple.
        /// </summary>
        public static double Get(this (double, double, double) p, int i)
        {
            switch (i)
            {
                case 0: return p.Item1;
                case 1: return p.Item2;
                case 2: return p.Item3;
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets i-th value of tuple.
        /// </summary>
        public static double Get(this (double, double, double, double) p, int i)
        {
            switch (i)
            {
                case 0: return p.Item1;
                case 1: return p.Item2;
                case 2: return p.Item3;
                case 3: return p.Item4;
                default: throw new IndexOutOfRangeException();
            }
        }

#if !TRAVIS_CI

        /// <summary>
        /// Sets i-th value in tuple (in-place).
        /// </summary>
        public static void Set(this ref (double, double) p, int i, double value)
        {
            switch (i)
            {
                case 0: p.Item1 = value; break;
                case 1: p.Item2 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Sets i-th value in tuple (in-place).
        /// </summary>
        public static void Set(this ref (double, double, double) p, int i, double value)
        {
            switch (i)
            {
                case 0: p.Item1 = value; break;
                case 1: p.Item2 = value; break;
                case 2: p.Item3 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Sets i-th value in tuple (in-place).
        /// </summary>
        public static void Set(this ref (double, double, double, double) p, int i, double value)
        {
            switch (i)
            {
                case 0: p.Item1 = value; break;
                case 1: p.Item2 = value; break;
                case 2: p.Item3 = value; break;
                case 3: p.Item4 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

#endif

        /// <summary>
        /// Creates tuple from given values with values in ascending order. 
        /// </summary>
        public static (double, double) CreateAscending(double d0, double d1)
            => d0 < d1 ? (d0, d1) : (d1, d0);

        /// <summary>
        /// Creates tuple from given values with values in ascending order. 
        /// </summary>
        public static (double, double, double) CreateAscending(double d0, double d1, double d2)
        {
            if (d0 < d1)
            {
                if (d1 < d2)
                    return (d0, d1, d2);
                else
                    return (d0 < d2) ? (d0, d2, d1) : (d2, d0, d1);
            }
            else
            {
                if (d2 < d1)
                    return (d2, d1, d0);
                else
                    return (d0 < d2) ? (d1, d0, d2) : (d1, d2, d0);
            }
        }
    }
}

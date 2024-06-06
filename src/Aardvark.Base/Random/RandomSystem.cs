using System;
using System.Threading;

namespace Aardvark.Base
{
    /// <summary>
    /// System Random Generator.
    /// </summary>
    public class RandomSystem : IRandomUniform
    {
        public Random Generator;

        #region Constructors

        /// <summary>
        /// Initialize using the time of day in milliseconds as seed.
        /// </summary>
        public RandomSystem() => Generator = new Random();

        /// <summary>
        /// Initialize using custom seed.
        /// </summary>
        public RandomSystem(int seed) => Generator = new Random(seed);

        #endregion

        #region IRandomUniform Members

        public int RandomBits => 31;

        public bool GeneratesFullDoubles => false;

        public void ReSeed(int seed) => Generator = new Random(seed);

        // FIXME: System.Random.Next() returns an integer in the interval of [0, 2^31 - 1)
        // but the IRandomSystem.UniformInt() comment specifies that the result
        // is in the interval [0, 2^31-1]
        public int UniformInt() => Generator.Next();

        private static readonly ThreadLocal<byte[]> s_buffer4 = new ThreadLocal<byte[]>(() => new byte[4]);
        /// <summary>
        /// Returns a uniformly distributed uint in the interval [0, 2^63-1].
        /// Constructed using 16 bits of each of two random integers.
        /// </summary>
        public uint UniformUInt()
        {
            var array = s_buffer4.Value;
            Generator.NextBytes(array);
            return BitConverter.ToUInt32(array, 0);
        }

        private static readonly ThreadLocal<byte[]> s_buffer8 = new ThreadLocal<byte[]>(() => new byte[8]);
        /// <summary>
        /// Returns a uniformly distributed long in the interval [0, 2^63-1].
        /// </summary>
        public long UniformLong()
        {
            var array = s_buffer8.Value;
            Generator.NextBytes(array);
            return (long)(BitConverter.ToUInt64(array, 0) & 0x7ffffffffffffffful);

        }

        /// <summary>
        /// Returns a uniformly distributed long in the interval [0, 2^64-1].
        /// </summary>
        public ulong UniformULong()
        {
            var array = s_buffer8.Value;
            Generator.NextBytes(array);
            return BitConverter.ToUInt64(array, 0);
        }

        public float UniformFloat() => (UniformInt() >> 7) * (float)(1.0 / 16777216.0);

        public float UniformFloatClosed() => (float)((UniformInt() - 1) * (1.0 / 2147483645.0));

        public float UniformFloatOpen()
        {
            int r; do { r = UniformInt() >> 7; } while (r == 0);
            return r * (float)(1.0 / 16777216.0);
        }

        public double UniformDouble() => Generator.NextDouble();

        public double UniformDoubleClosed() => (UniformInt() - 1) * (1.0 / 2147483645.0);

        public double UniformDoubleOpen() => UniformInt() * (1.0 / 2147483647.0);

        #endregion
    }
}

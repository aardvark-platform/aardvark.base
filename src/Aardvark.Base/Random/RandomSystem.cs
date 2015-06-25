using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public RandomSystem()
        {
            Generator = new Random();
        }

        public RandomSystem(int seed)
        {
            Generator = new Random(seed);
        }

        #endregion

        #region IRandomUniform Members

        public int RandomBits { get { return 31; } }

        public bool GeneratesFullDoubles { get { return false; } }

        public void ReSeed(int seed)
        {
            Generator = new Random(seed);
        }

        public int UniformInt()
        {
            return Generator.Next();
        }


        public static ThreadLocal<byte[]> s_buffer4 = new ThreadLocal<byte[]>(() => new byte[4]);
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

        public static ThreadLocal<byte[]> s_buffer8 = new ThreadLocal<byte[]>(() => new byte[8]);
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

        public float UniformFloat()
        {
            return (UniformInt() >> 7) * (float)(1.0 / 16777216.0);
        }

        public float UniformFloatClosed()
        {
            return (float)((UniformInt() - 1) * (1.0 / 2147483645.0));
        }

        public float UniformFloatOpen()
        {
            int r; do { r = UniformInt() >> 7; } while (r == 0);
            return r * (float)(1.0 / 16777216.0);
        }

        public double UniformDouble()
        {
            return Generator.NextDouble();
        }

        public double UniformDoubleClosed()
        {
            return (UniformInt() - 1) * (1.0 / 2147483645.0);
        }

        public double UniformDoubleOpen()
        {
            return UniformInt() * (1.0 / 2147483647.0);
        }

        #endregion

    }
}

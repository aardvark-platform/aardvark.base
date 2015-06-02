using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    /// <summary>
    /// Numerical Recipes Random Generator 0. This generator has only 32-bits
    /// of state for minimal memory overhead. The state is publically accessible
    /// in order to quickly access single bits.
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

        /// <summary>
        /// Returns a uniformly distributed uint in the interval [0, 2^63-1].
        /// Constructed using 16 bits of each of two random integers.
        /// </summary>
        public uint UniformUInt()
        {
            return ((~0x7fffu & (uint)UniformInt()) << 1) // bits 31-16
                   | ((uint)UniformInt() >> 15); // bits 15-0
        }

        /// <summary>
        /// Returns a uniformly distributed long in the interval [0, 2^63-1].
        /// Constructed using 21 bits of each of three random integers.
        /// </summary>
        public long UniformLong()
        {
            return ((~0x3ff & (long)UniformInt()) << 32) // bits 62-42
                   | ((~0x3ff & (long)UniformInt()) << 11) // bits 41-21  
                   | ((long)UniformInt() >> 10); // bits 20-0
        }

        /// <summary>
        /// Returns a uniformly distributed long in the interval [0, 2^64-1].
        /// Constructed using 21/22/21 bits of three random integers.
        /// </summary>
        public ulong UniformULong()
        {
            return ((~0x3fful & (ulong)UniformInt()) << 33) // bits 63-43
                   | ((~0x1fful & (ulong)UniformInt()) << 12) // bits 42-21  
                   | ((ulong)UniformInt() >> 10); // bits 20-0
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

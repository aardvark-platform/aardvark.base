using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Aardvark.Tests
{
    [TestFixture]
    public class Log2CeilingIntTests
    {
        private static Func<T, int> NetStandardCeiling<T>()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(directory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Expected sibling netstandard2.0 assembly at '{path}'.");
            var fun = Assembly.LoadFile(path).GetType(typeof(Fun).FullName);
            return (Func<T, int>)fun.GetMethod(nameof(Fun.Log2CeilingInt), new[] { typeof(T) })
                .CreateDelegate(typeof(Func<T, int>));
        }

        private static int IntegerCeiling(ulong value)
        {
            int result = 0;
            for (ulong remaining = value - 1; remaining != 0; remaining >>= 1) result++;
            return result;
        }

        private static IEnumerable<ulong> PositiveValues(int width)
        {
            ulong maximum = width == 32 ? uint.MaxValue : ulong.MaxValue;
            for (ulong value = 1; value <= 1024; value++) yield return value;
            for (int bit = 0; bit < width; bit++)
            {
                ulong power = 1UL << bit;
                if (power > 2) yield return power - 2;
                if (power > 1) yield return power - 1;
                yield return power;
                yield return power + 1;
                yield return power + 2;
            }
            // Explore rounding neighborhoods as well as the literal precision thresholds.
            foreach (int bit in new[] { 24, 31, 32, 53, 63 })
            {
                if (bit >= width) continue;
                ulong power = 1UL << bit;
                for (ulong offset = 1; offset <= 2048; offset++)
                {
                    yield return power - offset;
                    yield return power + offset;
                }
            }
            yield return maximum - 1;
            yield return maximum;
            var random = new Random(73189 + width);
            var bytes = new byte[8];
            for (int i = 0; i < 4096; i++)
            {
                random.NextBytes(bytes);
                ulong value = BitConverter.ToUInt64(bytes, 0) & maximum;
                if (value != 0) yield return value;
            }
        }

        [Test]
        public void Positive32BitCeilingsMatchIntegerBitLengths()
        {
            foreach (bool standard in new[] { false, true })
            {
                Func<int, int> signed = standard ? NetStandardCeiling<int>() : Fun.Log2CeilingInt;
                Func<uint, int> unsigned = standard ? NetStandardCeiling<uint>() : Fun.Log2CeilingInt;
                string target = standard ? "netstandard2.0" : "net8.0";
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(25, signed(16777217), $"{target}, int precision boundary");
                    Assert.AreEqual(25, unsigned(16777217U), $"{target}, uint precision boundary");
                });
                foreach (ulong value in PositiveValues(32))
                {
                    int expected = IntegerCeiling(value);
                    Assert.AreEqual(expected, unsigned((uint)value), $"{target}, uint={value}");
                    if (value <= int.MaxValue)
                        Assert.AreEqual(expected, signed((int)value), $"{target}, int={value}");
                }
            }
        }

        [Test]
        public void Positive64BitCeilingsMatchIntegerBitLengths()
        {
            foreach (bool standard in new[] { false, true })
            {
                Func<long, int> signed = standard ? NetStandardCeiling<long>() : Fun.Log2CeilingInt;
                Func<ulong, int> unsigned = standard ? NetStandardCeiling<ulong>() : Fun.Log2CeilingInt;
                string target = standard ? "netstandard2.0" : "net8.0";
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(54, signed(9007199254740993L), $"{target}, long precision boundary");
                    Assert.AreEqual(54, unsigned(9007199254740993UL), $"{target}, ulong precision boundary");
                });
                foreach (ulong value in PositiveValues(64))
                {
                    int expected = IntegerCeiling(value);
                    Assert.AreEqual(expected, unsigned(value), $"{target}, ulong={value}");
                    if (value <= long.MaxValue)
                        Assert.AreEqual(expected, signed((long)value), $"{target}, long={value}");
                }
            }
        }

        [Test]
        public void ZeroAndNegativeInputsKeepTheirFloatingConversionResults()
        {
            var negatives32 = new List<int> { -1, -2, -3, -16777217, int.MinValue, int.MinValue + 1 };
            var negatives64 = new List<long> { -1, -2, -3, -9007199254740993L, long.MinValue, long.MinValue + 1 };
            var random = new Random(37019);
            var bytes = new byte[8];
            for (int i = 0; i < 128; i++)
            {
                random.NextBytes(bytes);
                negatives32.Add(BitConverter.ToInt32(bytes, 0) | int.MinValue);
                negatives64.Add(BitConverter.ToInt64(bytes, 0) | long.MinValue);
            }
            foreach (bool standard in new[] { false, true })
            {
                Func<int, int> i32 = standard ? NetStandardCeiling<int>() : Fun.Log2CeilingInt;
                Func<uint, int> u32 = standard ? NetStandardCeiling<uint>() : Fun.Log2CeilingInt;
                Func<long, int> i64 = standard ? NetStandardCeiling<long>() : Fun.Log2CeilingInt;
                Func<ulong, int> u64 = standard ? NetStandardCeiling<ulong>() : Fun.Log2CeilingInt;
                Func<float, int> f32 = standard ? NetStandardCeiling<float>() : Fun.Log2CeilingInt;
                Func<double, int> f64 = standard ? NetStandardCeiling<double>() : Fun.Log2CeilingInt;
                string target = standard ? "netstandard2.0" : "net8.0";
                Assert.AreEqual(-127, i32(0), target + ", int zero");
                Assert.AreEqual(-127, u32(0), target + ", uint zero");
                Assert.AreEqual(-1023, i64(0), target + ", long zero");
                Assert.AreEqual(-1023, u64(0), target + ", ulong zero");
                foreach (int value in negatives32)
                    Assert.AreEqual(f32((float)value), i32(value), $"{target}, int={value}");
                foreach (long value in negatives64)
                    Assert.AreEqual(f64((double)value), i64(value), $"{target}, long={value}");
            }
        }
    }
}

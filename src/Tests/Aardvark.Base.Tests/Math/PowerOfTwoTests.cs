using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PowerOfTwoTests
    {
        private static Assembly[] CoreAssemblies()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(directory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Expected sibling netstandard2.0 assembly at '{path}'.");
            return new[] { typeof(Fun).Assembly, Assembly.LoadFile(path) };
        }

        private static Func<T, T> Next<T>(Assembly assembly)
            => (Func<T, T>)assembly.GetType(typeof(Fun).FullName)
                .GetMethod(nameof(Fun.NextPowerOfTwo), new[] { typeof(T) })
                .CreateDelegate(typeof(Func<T, T>));

        private static BigInteger Oracle(BigInteger value, int width, bool signed)
        {
            if (value <= 0) return BigInteger.Zero;
            BigInteger result = BigInteger.One;
            while (result < value) result *= 2;
            if (result >= (BigInteger.One << (signed ? width - 1 : width)))
                return signed ? -(BigInteger.One << (width - 1)) : BigInteger.Zero;
            return result;
        }

        private static IEnumerable<ulong> UnsignedInputs(int width)
        {
            ulong maximum = width == 32 ? uint.MaxValue : ulong.MaxValue;
            for (ulong value = 0; value <= 1024; value++) yield return value;
            for (int bit = 0; bit < width; bit++)
            {
                ulong power = 1UL << bit;
                yield return power - 1;
                yield return power;
                yield return power + 1;
            }
            yield return maximum - 1;
            yield return maximum;
            var random = new Random(51389 + width);
            var bytes = new byte[8];
            for (int i = 0; i < 4096; i++)
            {
                random.NextBytes(bytes);
                yield return BitConverter.ToUInt64(bytes, 0) & maximum;
            }
        }

        [Test]
        public void SignedNonpositiveInputsReturnZeroIncludingMinima()
        {
            foreach (var assembly in CoreAssemblies())
            {
                var i32 = Next<int>(assembly);
                var i64 = Next<long>(assembly);
                string target = assembly.Location;
                Assert.Multiple(() =>
                {
                    foreach (int value in new[] { int.MinValue, int.MinValue + 1, int.MinValue + 2, -1024, -3, -2, -1, 0 })
                        Assert.AreEqual(0, i32(value), $"{target}, int={value}");
                    foreach (long value in new[] { long.MinValue, long.MinValue + 1, long.MinValue + 2, -1024L, -3, -2, -1, 0 })
                        Assert.AreEqual(0L, i64(value), $"{target}, long={value}");
                });
            }
        }

        [Test]
        public void ScalarBoundariesAndSeededValuesMatchBigIntegerRounding()
        {
            foreach (var assembly in CoreAssemblies())
            {
                var i32 = Next<int>(assembly);var u32 = Next<uint>(assembly);
                var i64 = Next<long>(assembly);var u64 = Next<ulong>(assembly);
                string target = assembly.Location;
                foreach (ulong bits in UnsignedInputs(32))
                {
                    uint unsigned = (uint)bits;int signed = unchecked((int)unsigned);
                    Assert.AreEqual((uint)Oracle(unsigned, 32, false), u32(unsigned), $"{target}, uint={unsigned}");
                    Assert.AreEqual((int)Oracle(signed, 32, true), i32(signed), $"{target}, int={signed}");
                }
                foreach (ulong unsigned in UnsignedInputs(64))
                {
                    long signed = unchecked((long)unsigned);
                    Assert.AreEqual((ulong)Oracle(unsigned, 64, false), u64(unsigned), $"{target}, ulong={unsigned}");
                    Assert.AreEqual((long)Oracle(signed, 64, true), i64(signed), $"{target}, long={signed}");
                }
            }
        }

        [Test]
        public void SignedVectorsRoundEachMixedComponentIndependently()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in CoreAssemblies())
                foreach (int width in new[] { 32, 64 })
                foreach (int dimension in new[] { 2, 3, 4 })
                {
                    Type scalar = width == 32 ? typeof(int) : typeof(long);
                    var vector = assembly.GetType($"Aardvark.Base.V{dimension}{(width == 32 ? "i" : "l")}");
                    var parameters = new Type[dimension];Array.Fill(parameters, scalar);
                    var constructor = vector.GetConstructor(parameters);
                    var method = assembly.GetType(typeof(Fun).FullName).GetMethod(nameof(Fun.NextPowerOfTwo), new[] { vector });
                    var minimum = -(BigInteger.One << (width - 1));
                    var maximum = -minimum - 1;
                    var values = new[] { minimum, minimum + 1, minimum + 2, new BigInteger(-1), BigInteger.Zero,
                        BigInteger.One, new BigInteger(3), (BigInteger.One << (width - 2)),
                        (BigInteger.One << (width - 2)) + 1, maximum };
                    foreach (var value in values)
                    for (int position = 0; position < dimension; position++)
                    {
                        var input = new object[dimension];var expected = new object[dimension];
                        for (int component = 0; component < dimension; component++)
                        {
                            var element = component == position ? value : values[(position + component + 3) % values.Length];
                            var rounded = Oracle(element, width, true);
                            if (width == 32) { input[component] = (int)element;expected[component] = (int)rounded; }
                            else { input[component] = (long)element;expected[component] = (long)rounded; }
                        }
                        var result = method.Invoke(null, new[] { constructor.Invoke(input) });
                        for (int component = 0; component < dimension; component++)
                            Assert.AreEqual(expected[component], vector.GetField("XYZW"[component].ToString()).GetValue(result),
                                $"{assembly.Location}, {vector.Name}, input=[{string.Join(",", input)}], component={component}");
                    }
                }
            });
        }
    }
}

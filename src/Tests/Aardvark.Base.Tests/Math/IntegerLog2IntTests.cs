using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace Aardvark.Tests
{
    public class IntegerLog2IntTests
    {
        [Test]
        public void PositiveIntegerFloorsMatchShiftOracle()
        {
            foreach (var core in Cores())
            foreach (var value in PositiveCases(core))
                Assert.AreEqual(ShiftFloor(value), core.Positive(value), $"{core.Name}: positive value={value}");
        }

        [Test]
        public void MipmapCountsUseExactLargestDimension()
        {
            foreach (var core in Cores().Where(c => c.Scalar == typeof(int)))
            {
                var values = new HashSet<int> { 1, 2, 3, 7, 8, 15, 16, 17, int.MaxValue };
                for (var exponent = 24; exponent <= 31; exponent++)
                for (var offset = -257; offset <= 2; offset++)
                {
                    var value = (1L << exponent) + offset;
                    if (value > 0 && value <= int.MaxValue) values.Add((int)value);
                }
                foreach (var size in values.OrderBy(x => x))
                {
                    var expected = 1 + ShiftFloor((ulong)size);
                    Assert.AreEqual(expected, core.Mipmap(new[] { size }), core.Name + $" scalar mipmap size={size}");
                    foreach (var dimension in new[] { 2, 3 })
                    for (var largest = 0; largest < dimension; largest++)
                    {
                        var dimensions = Enumerable.Repeat(Math.Max(1, size / 2), dimension).ToArray();
                        dimensions[largest] = size;
                        Assert.AreEqual(expected, core.Mipmap(dimensions), core.Name + $" mipmap size=({string.Join(",", dimensions)})");
                    }
                }
            }
        }

        [Test]
        public void OrdinaryAndNonpositiveInputsKeepTheirResults()
        {
            foreach (var core in Cores())
            {
                for (ulong value = 1; value <= 10000; value++)
                    Assert.AreEqual(ShiftFloor(value), core.Positive(value), $"{core.Name}: ordinary value={value}");
                Assert.AreEqual(core.Portable ? (core.Width == 32 ? -127 : -1023) : 0, core.Positive(0), core.Name + " zero policy");
                if (!core.Signed) continue;
                var negatives = new HashSet<long> { -1, -2, -3, -127, core.Width == 32 ? int.MinValue : long.MinValue };
                var minimum = -(BigInteger.One << (core.Width - 1));
                for (var exponent = 0; exponent < core.Width; exponent++)
                for (var offset = -2; offset <= 2; offset++)
                {
                    var value = -(BigInteger.One << exponent) + offset;
                    if (value >= minimum && value < 0) negatives.Add((long)value);
                }
                var random = new Random(63197 + core.Width); var bytes = new byte[8];
                for (var i = 0; i < 4096; i++)
                {
                    random.NextBytes(bytes);
                    negatives.Add(core.Width == 32 ? unchecked((int)(BitConverter.ToUInt32(bytes, 0) | 0x80000000u))
                        : unchecked((long)(BitConverter.ToUInt64(bytes, 0) | 0x8000000000000000ul)));
                }
                foreach (var value in negatives)
                {
                    // Characterize the old target-specific policy rather than assigning
                    // a new meaning to a logarithm of a nonpositive signed integer.
                    var expected = !core.Portable ? core.Width - 1 : core.Width == 32
                        ? ((BitConverter.SingleToInt32Bits((float)value) >> 23) & 255) - 127
                        : (int)((BitConverter.DoubleToInt64Bits((double)value) >> 52) & 2047) - 1023;
                    Assert.AreEqual(expected, core.Negative(value), $"{core.Name}: negative value={value}");
                }
                if (core.Scalar == typeof(int))
                foreach (var dimensions in new[] { new[] { 0 }, new[] { -1 }, new[] { 0, 0 }, new[] { -3, -1 },
                    new[] { 0, 0, 0 }, new[] { int.MinValue, -3, -1 } })
                {
                    var maximum = dimensions.Max();
                    var expected = 1 + (maximum == 0 ? core.Positive(0) : core.Negative(maximum));
                    Assert.AreEqual(expected, core.Mipmap(dimensions), core.Name + $" legacy mipmap=({string.Join(",", dimensions)})");
                }
            }
        }

        private static int ShiftFloor(ulong value)
        {
            var exponent = -1;
            while (value != 0) { value >>= 1; exponent++; }
            return exponent;
        }

        private static IEnumerable<ulong> PositiveCases(Core core)
        {
            var values = new HashSet<ulong> { 1, core.Maximum, core.Maximum - 1, core.Maximum - 2 };
            for (var exponent = 0; exponent <= core.Width; exponent++)
            {
                var power = BigInteger.One << exponent;
                // The below-power floating spacing is 2^(exponent-precision).
                // Dense neighborhoods include its midpoint/tie and adjacent integers,
                // including estimates32/64 at the unsigned upper endpoint.
                var radius = 1 << Math.Max(0, exponent - (core.Width == 32 ? 24 : 53) + 1);
                for (var offset = -Math.Max(4, radius); offset <= Math.Max(4, radius); offset++)
                {
                    var value = power + offset;
                    if (value > 0 && value <= core.Maximum) values.Add((ulong)value);
                }
            }
            var random = new Random(63191 + core.Width + (core.Signed ? 1 : 0)); var bytes = new byte[8];
            for (var i = 0; i < 20000; i++)
            {
                random.NextBytes(bytes); var value = BitConverter.ToUInt64(bytes, 0) & core.Maximum;
                values.Add(value == 0 ? 1 : value);
            }
            return values.OrderBy(x => x);
        }

        private sealed class Core
        {
            public readonly string Name;
            public readonly bool Portable, Signed;
            public readonly int Width;
            public readonly ulong Maximum;
            public readonly Type Scalar;
            public readonly Func<ulong, int> Positive;
            public readonly Func<long, int> Negative;
            private readonly Type fun;
            private readonly Assembly assembly;
            public Core(Assembly assembly, string target, Type scalar, int width, bool signed, ulong maximum)
            {
                this.assembly = assembly; Scalar = scalar; Width = width; Signed = signed; Maximum = maximum;
                Portable = target == "netstandard2.0"; Name = target + "/" + scalar.Name;
                fun = assembly.GetType("Aardvark.Base.Fun", true);
                var method = fun.GetMethod("Log2Int", new[] { scalar });
                if (scalar == typeof(int)) { var f = (Func<int, int>)method.CreateDelegate(typeof(Func<int, int>)); Positive = x => f((int)x); Negative = x => f((int)x); }
                else if (scalar == typeof(uint)) { var f = (Func<uint, int>)method.CreateDelegate(typeof(Func<uint, int>)); Positive = x => f((uint)x); }
                else if (scalar == typeof(long)) { var f = (Func<long, int>)method.CreateDelegate(typeof(Func<long, int>)); Positive = x => f((long)x); Negative = f; }
                else { var f = (Func<ulong, int>)method.CreateDelegate(typeof(Func<ulong, int>)); Positive = f; }
            }
            public int Mipmap(int[] dimensions)
            {
                if (dimensions.Length == 1)
                    return (int)fun.GetMethod("MipmapLevels", new[] { typeof(int) }).Invoke(null, new object[] { dimensions[0] });
                var vector = assembly.GetType("Aardvark.Base.V" + dimensions.Length + "i", true);
                var value = vector.GetConstructor(Enumerable.Repeat(typeof(int), dimensions.Length).ToArray())
                    .Invoke(dimensions.Select(x => (object)x).ToArray());
                return (int)fun.GetMethod("MipmapLevels", new[] { vector }).Invoke(null, new[] { value });
            }
        }

        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path);
                var assembly = Assembly.LoadFile(path);
                yield return new Core(assembly, target, typeof(int), 32, true, int.MaxValue);
                yield return new Core(assembly, target, typeof(uint), 32, false, uint.MaxValue);
                yield return new Core(assembly, target, typeof(long), 64, true, long.MaxValue);
                yield return new Core(assembly, target, typeof(ulong), 64, false, ulong.MaxValue);
            }
        }
    }
}

using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FloatingLog2IntTests
    {
        private static IEnumerable<Assembly> Assemblies()
        {
            yield return typeof(Fun).Assembly;
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location)!,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core assembly: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static Func<T, int> Bind<T>(Assembly assembly, string name)
            => assembly.GetType(typeof(Fun).FullName!, true)!.GetMethod(name, new[] { typeof(T) })!
                .CreateDelegate<Func<T, int>>();

        private static Func<double, int> Scalar(Assembly assembly, bool single, bool ceiling)
        {
            string name = ceiling ? nameof(Fun.Log2CeilingInt) : nameof(Fun.Log2Int);
            if (!single) return Bind<double>(assembly, name);
            var call = Bind<float>(assembly, name);
            return x => call((float)x);
        }

        private static int BitLength(ulong value)
        {
            int length = 0;
            while (value != 0) { value >>= 1; length++; }
            return length;
        }

        private static double Value(ulong bits, bool single) => single
            ? BitConverter.Int32BitsToSingle(unchecked((int)bits))
            : BitConverter.Int64BitsToDouble(unchecked((long)bits));

        private static ulong Bits(double value, bool single) => single
            ? unchecked((uint)BitConverter.SingleToInt32Bits((float)value))
            : unchecked((ulong)BitConverter.DoubleToInt64Bits(value));

        private static int Expected(ulong bits, bool single, bool ceiling)
        {
            int mantissaBits = single ? 23 : 52, bias = single ? 127 : 1023;
            ulong mantissa = bits & ((1UL << mantissaBits) - 1);
            int exponent = (int)((bits >> mantissaBits) & (ulong)(2 * bias + 1));
            bool negative = (bits & (1UL << (single ? 31 : 63))) != 0;
            if (!negative && exponent != 2 * bias + 1 && (exponent != 0 || mantissa != 0))
            {
                ulong significand = exponent == 0 ? mantissa : (1UL << mantissaBits) | mantissa;
                int scale = exponent == 0 ? 1 - bias - mantissaBits : exponent - bias - mantissaBits;
                return scale + (ceiling ? BitLength(significand - 1) : BitLength(significand) - 1);
            }
            // Retain the legacy exponent-field convention outside the positive finite domain.
            return exponent - bias + (ceiling && mantissa != 0 ? 1 : 0);
        }

        private static void Check(Func<double, int> floor, Func<double, int> ceiling, ulong bits, bool single, string context)
        {
            double value = Value(bits, single);
            int f = floor(value), c = ceiling(value);
            int expectedFloor = Expected(bits, single, false), expectedCeiling = Expected(bits, single, true);
            if (f != expectedFloor || c != expectedCeiling)
                Assert.Fail($"{context}, {(single ? "float" : "double")}, bits 0x{bits:X}: floor {f}/{expectedFloor}, ceiling {c}/{expectedCeiling}");
        }

        private static ulong[] Significands(int width, int samples)
        {
            ulong mask = (1UL << width) - 1;
            var values = new HashSet<ulong>();
            for (ulong i = 1; i <= 4096; i++) { values.Add(i); values.Add(mask - i + 1); }
            for (int bit = 0; bit < width; bit++)
            for (int delta = -2; delta <= 2; delta++)
            {
                long value = (1L << bit) + delta;
                if (value > 0 && (ulong)value <= mask) values.Add((ulong)value);
            }
            var random = new Random(71831 + width);
            var bytes = new byte[8];
            for (int i = 0; i < samples; i++)
            {
                random.NextBytes(bytes);
                ulong value = BitConverter.ToUInt64(bytes) & mask;
                if (value != 0) values.Add(value);
            }
            return values.ToArray();
        }

        [Test]
        public void PositiveSubnormalScalarsHaveExactFloorAndCeilingExponents()
        {
            foreach (var assembly in Assemblies())
            {
                var floor = Scalar(assembly, true, false); var ceiling = Scalar(assembly, true, true);
                // Exhaust all float significands; the leading-bit threshold is an integer oracle.
                int exponent = -149; uint nextPower = 2, visited = 0;
                for (uint bits = 1; bits <= 0x007fffff; bits++)
                {
                    if (bits == nextPower) { exponent++; nextPower <<= 1; }
                    float value = BitConverter.Int32BitsToSingle((int)bits);
                    int f = floor(value), c = ceiling(value);
                    int expectedCeiling = exponent + ((bits & (bits - 1)) == 0 ? 0 : 1);
                    if (f != exponent || c != expectedCeiling)
                        Assert.Fail($"{assembly.Location}, float bits 0x{bits:X}: floor {f}/{exponent}, ceiling {c}/{expectedCeiling}");
                    visited++;
                }
                Assert.That(visited, Is.EqualTo(0x007fffff));
                floor = Scalar(assembly, false, false); ceiling = Scalar(assembly, false, true);
                foreach (ulong bits in Significands(52, 100000)) Check(floor, ceiling, bits, false, assembly.Location);
            }
        }

        private static IEnumerable<ulong> NormalAndExceptionalBits(bool single)
        {
            int mantissaBits = single ? 23 : 52, bias = single ? 127 : 1023;
            ulong mask = (1UL << mantissaBits) - 1, sign = 1UL << (single ? 31 : 63);
            foreach (ulong bits in new[] { 0UL, 1UL, mask, mask + 1, mask + 2,
                (ulong)(2 * bias + 1) << mantissaBits, ((ulong)(2 * bias + 1) << mantissaBits) | 1,
                ((ulong)(2 * bias + 1) << mantissaBits) | mask })
            { if (bits == 0 || bits > mask) yield return bits; yield return bits | sign; }
            for (int exponent = 1; exponent <= 2 * bias; exponent++)
            foreach (ulong mantissa in new[] { 0UL, 1UL, mask / 2, mask - 1, mask })
            {
                ulong bits = ((ulong)exponent << mantissaBits) | mantissa;
                yield return bits; yield return bits | sign;
            }
            foreach (ulong bits in Significands(mantissaBits, 4096)) yield return bits | sign;
        }

        private static void IntegerControl<T>(Assembly assembly, T[] values, Func<T, double> converted, Func<T, ulong> unsigned, bool single)
        {
            bool portable = assembly.GetCustomAttribute<TargetFrameworkAttribute>()!.FrameworkName.Contains("NETStandard");
            var floor = Bind<T>(assembly, nameof(Fun.Log2Int)); var ceiling = Bind<T>(assembly, nameof(Fun.Log2CeilingInt));
            foreach (T value in values)
            {
                ulong bits = Bits(converted(value), single);
                int expectedFloor = portable ? Expected(bits, single, false) : Math.Max(0, BitLength(unsigned(value)) - 1);
                Assert.That(floor(value), Is.EqualTo(expectedFloor), $"{assembly.Location}, {typeof(T).Name} floor {value}");
                Assert.That(ceiling(value), Is.EqualTo(Expected(bits, single, true)), $"{assembly.Location}, {typeof(T).Name} ceiling {value}");
            }
        }

        [Test]
        public void NormalExceptionalAndIntegerInputsKeepTheirResults()
        {
            foreach (var assembly in Assemblies())
            {
                foreach (bool single in new[] { true, false })
                {
                    var floor = Scalar(assembly, single, false); var ceiling = Scalar(assembly, single, true);
                    foreach (ulong bits in NormalAndExceptionalBits(single)) Check(floor, ceiling, bits, single, assembly.Location);
                }
                IntegerControl(assembly, new[] { int.MinValue, int.MinValue + 1, -1, 0, 1, 2, 3, 16777215, 16777216, 16777217, int.MaxValue }, x => (float)x, x => unchecked((uint)x), true);
                IntegerControl(assembly, new uint[] { 0, 1, 2, 3, 16777215, 16777216, 16777217, uint.MaxValue }, x => (float)x, x => x, true);
                IntegerControl(assembly, new[] { long.MinValue, long.MinValue + 1, -1, 0, 1, 2, 3, 9007199254740991, 9007199254740992, 9007199254740993, long.MaxValue }, x => (double)x, x => unchecked((ulong)x), false);
                IntegerControl(assembly, new ulong[] { 0, 1, 2, 3, 9007199254740991, 9007199254740992, 9007199254740993, ulong.MaxValue }, x => (double)x, x => x, false);
            }
        }

        private static Func<double[], int[]> Vector(Assembly assembly, int dimension, bool single)
        {
            var scalar = single ? typeof(float) : typeof(double);
            var vector = assembly.GetType($"Aardvark.Base.V{dimension}{(single ? "f" : "d")}", true)!;
            Assert.That(vector.Assembly, Is.SameAs(assembly));
            var values = Expression.Parameter(typeof(double[]));
            var input = Expression.New(vector.GetConstructor(Enumerable.Repeat(scalar, dimension).ToArray())!,
                Enumerable.Range(0, dimension).Select(i => Expression.Convert(Expression.ArrayIndex(values, Expression.Constant(i)), scalar)));
            var method = assembly.GetType(typeof(Fun).FullName!, true)!.GetMethod(nameof(Fun.Log2Int), new[] { vector })!;
            Assert.That(method.ReturnType.Assembly, Is.SameAs(assembly));
            var result = Expression.Variable(method.ReturnType);
            return Expression.Lambda<Func<double[], int[]>>(Expression.Block(new[] { result },
                Expression.Assign(result, Expression.Call(method, input)),
                Expression.NewArrayInit(typeof(int), "XYZW".Take(dimension).Select(c => Expression.Field(result, c.ToString())))), values).Compile();
        }

        [Test]
        public void FloatingVectorsApplyExactExponentsToEveryComponent()
        {
            foreach (var assembly in Assemblies())
            foreach (bool single in new[] { true, false })
            {
                var corpus = Significands(single ? 23 : 52, 4096).Concat(NormalAndExceptionalBits(single)).Distinct().ToArray();
                for (int dimension = 2; dimension <= 4; dimension++)
                {
                    var call = Vector(assembly, dimension, single); var values = new double[dimension];
                    for (int i = 0; i < corpus.Length; i++)
                    {
                        for (int axis = 0; axis < dimension; axis++) values[axis] = Value(corpus[(i + 17 * axis) % corpus.Length], single);
                        var actual = call(values);
                        for (int axis = 0; axis < dimension; axis++)
                        {
                            ulong bits = corpus[(i + 17 * axis) % corpus.Length];
                            int expected = Expected(bits, single, false);
                            if (actual[axis] != expected)
                                Assert.Fail($"{assembly.Location}, V{dimension}{(single ? "f" : "d")}, axis {axis}, bits 0x{bits:X}: {actual[axis]}/{expected}");
                        }
                    }
                }
            }
        }
    }
}

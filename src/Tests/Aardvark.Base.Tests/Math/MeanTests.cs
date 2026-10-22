using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class MeanTests
    {
        private static IEnumerable<Assembly> Cores()
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            yield return typeof(Fun).Assembly;
            yield return Assembly.LoadFile(path);
        }

        private static Func<IEnumerable<T>, double> Mean<T>(Assembly core)
            => (Func<IEnumerable<T>, double>)core.GetType("Aardvark.Base.Fun", true)!
                .GetMethod("Mean", new[] { typeof(IEnumerable<T>) })!
                .CreateDelegate(typeof(Func<IEnumerable<T>, double>));

        // Explicit round-to-nearest, ties-to-even; BigInteger's double cast is not the oracle.
        private static double RoundedDouble(BigInteger value)
        {
            var n = BigInteger.Abs(value);
            int shift = Math.Max(0, (int)n.GetBitLength() - 53);
            var head = n >> shift;
            if (shift > 0)
            {
                var tail = n - (head << shift);
                var half = BigInteger.One << (shift - 1);
                if (tail > half || (tail == half && !head.IsEven)) head++;
            }
            double result = Math.ScaleB((double)(ulong)head, shift);
            return value.Sign < 0 ? -result : result;
        }

        [Test]
        public void IntegralMeansAccumulateExactSumsAcrossStorageLimits()
        {
            Assert.That(RoundedDouble((BigInteger.One << 53) + 1), Is.EqualTo(9007199254740992.0));
            Assert.That(RoundedDouble((BigInteger.One << 53) + 3), Is.EqualTo(9007199254740996.0));
            Assert.That(RoundedDouble(-((BigInteger.One << 53) + 3)), Is.EqualTo(-9007199254740996.0));
            foreach (var core in Cores())
            {
                CheckValues<byte>(core, byte.MinValue, byte.MaxValue, x => (byte)x, 53117);
                CheckValues<sbyte>(core, sbyte.MinValue, sbyte.MaxValue, x => (sbyte)x, 53118);
                CheckValues<long>(core, long.MinValue, long.MaxValue, x => (long)x, 53119);
                CheckValues<ulong>(core, ulong.MinValue, ulong.MaxValue, x => (ulong)x, 53120);
            }
        }

        private static void CheckValues<T>(Assembly core, BigInteger min, BigInteger max, Func<BigInteger, T> convert, int seed)
        {
            var mean = Mean<T>(core);
            void Check(BigInteger[] values, string context)
            {
                BigInteger sum = 0;
                foreach (var value in values) sum += value;
                double expected = RoundedDouble(sum) / values.Length;
                double actual = mean(values.Select(convert).ToArray());
                string message = $"{core.Location}: {typeof(T).Name}; {context}; n={values.Length}, sum={sum}, expected={expected:R}, actual={actual:R}";
                if (values.Length == 0) Assert.That(double.IsNaN(actual), Is.True, message);
                else if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) || (sum >= min && sum <= max))
                    Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), message);
                else
                {
                    // Decimal converts via finite-precision limbs; allow two ULPs after division,
                    // but keep originally representable sums and byte means bit-exact above.
                    double magnitude = Math.Abs(expected);
                    double ulp = Math.BitIncrement(magnitude) - magnitude;
                    Assert.That(actual, Is.EqualTo(expected).Within(2 * ulp), message);
                }
            }

            var anchors = new[] { min, min + 1, max / 2, max / 2 + 1, max - 1, max, BigInteger.Zero, BigInteger.One, -BigInteger.One,
                (BigInteger.One << 53) - 1, BigInteger.One << 53, (BigInteger.One << 53) + 1,
                -((BigInteger.One << 53) + 1) }.Where(x => x >= min && x <= max).Distinct().ToArray();
            foreach (var value in anchors)
            foreach (int count in new[] { 1, 2, 3, 7, 64, 4096 })
                Check(Enumerable.Repeat(value, count).ToArray(), $"repeat {value}");
            foreach (var a in anchors)
            foreach (var b in anchors)
                Check(new[] { a, b }, $"pair {a},{b}");
            foreach (var values in new[] { new[] { max, max, min }, new[] { min, min, max }, new[] { max, min, BigInteger.One } })
            foreach (var order in new[] { new[] { 0, 1, 2 }, new[] { 0, 2, 1 }, new[] { 1, 0, 2 }, new[] { 1, 2, 0 }, new[] { 2, 0, 1 }, new[] { 2, 1, 0 } })
                Check(order.Select(i => values[i]).ToArray(), $"permutation {string.Join(",", order)}");
            Check(new[] { max, min, max, min }, "mixed extrema cancellation");
            if (typeof(T) == typeof(long)) Check(new[] { max, max, max, new BigInteger(2052) }, "conversion midpoint above UInt64");
            if (typeof(T) == typeof(ulong)) Check(new[] { max, BigInteger.One, (BigInteger.One << 63) + 2049 }, "conversion midpoint above UInt64");

            var random = new Random(seed);
            var bytes = new byte[typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) ? 1 : 8];
            for (int sample = 0; sample < 1024; sample++)
            {
                var values = new BigInteger[random.Next(65)];
                for (int i = 0; i < values.Length; i++)
                {
                    random.NextBytes(bytes);
                    values[i] = new BigInteger(bytes, isUnsigned: min >= 0, isBigEndian: false);
                }
                Check(values, $"seed={seed}, sample={sample}");
                Check(values.Reverse().ToArray(), $"seed={seed}, reversed sample={sample}");
                if (values.Length > 1) Check(values.Skip(1).Concat(values.Take(1)).ToArray(), $"seed={seed}, rotated sample={sample}");
            }
        }

        [Test]
        public void LazyByteMeansCrossInt32SumBoundariesWithoutLargeArrays()
        {
            foreach (var core in Cores())
            {
                var unsigned = Mean<byte>(core);
                foreach (int count in new[] { 8421503, 8421504, 8421505 })
                {
                    double expected = RoundedDouble(new BigInteger(255) * count) / count;
                    Assert.That(unsigned(Enumerable.Repeat((byte)255, count)), Is.EqualTo(expected), $"{core.Location}: byte 255, count={count}");
                }
                var signed = Mean<sbyte>(core);
                foreach (int count in new[] { 16777215, 16777216, 16777217 })
                {
                    double expected = RoundedDouble(new BigInteger(-128) * count) / count;
                    Assert.That(signed(Enumerable.Repeat((sbyte)-128, count)), Is.EqualTo(expected), $"{core.Location}: sbyte -128, count={count}");
                }
            }
        }

        [Test]
        public void IntegralMeansKeepEmptyAndEnumerationContracts()
        {
            foreach (var core in Cores())
            {
                CheckEnumeration<byte>(core, x => (byte)x);
                CheckEnumeration<sbyte>(core, x => (sbyte)x);
                CheckEnumeration<long>(core, x => x);
                CheckEnumeration<ulong>(core, x => (ulong)x);
            }
        }

        private static void CheckEnumeration<T>(Assembly core, Func<int, T> convert)
        {
            var mean = Mean<T>(core);
            Assert.That(double.IsNaN(mean(Array.Empty<T>())), Is.True, $"{core.Location}: empty {typeof(T).Name}");
            Assert.Throws<NullReferenceException>(() => mean(null!), $"{core.Location}: null {typeof(T).Name}");
            var empty = new MeanSequence<T>(Array.Empty<T>(), 0);
            Assert.That(double.IsNaN(mean(empty)), Is.True, core.Location);
            Assert.That(string.Join(",", empty.Events), Is.EqualTo("get,move0,dispose"), core.Location);
            for (int fault = 0; fault <= 5; fault++)
            {
                var source = new MeanSequence<T>(new[] { convert(1), convert(3) }, fault);
                string context = $"{core.Location}: {typeof(T).Name}, fault={fault}";
                Assert.That(source.Events, Is.Empty, context);
                if (fault == 0) Assert.That(mean(source), Is.EqualTo(2.0), context);
                else Assert.That(Assert.Throws<InvalidOperationException>(() => mean(source)),
                    Is.SameAs(fault >= 4 ? source.DisposeFailure : source.Failure), context);
                string expected = fault switch
                {
                    1 => "get",
                    2 or 5 => "get,move0,current0,move1,dispose",
                    3 => "get,move0,current0,move1,current1,dispose",
                    _ => "get,move0,current0,move1,current1,move2,dispose"
                };
                Assert.That(string.Join(",", source.Events), Is.EqualTo(expected), context);
            }
        }

        [Test]
        public void OtherMeanOverloadsKeepTheirResults()
        {
            foreach (var core in Cores())
            {
                Assert.That(Mean<short>(core)(new[] { short.MinValue, short.MaxValue }), Is.EqualTo(-0.5), core.Location);
                Assert.That(Mean<ushort>(core)(new[] { ushort.MaxValue, ushort.MaxValue }), Is.EqualTo((double)ushort.MaxValue), core.Location);
                Assert.That(Mean<int>(core)(new[] { int.MinValue, int.MaxValue }), Is.EqualTo(-0.5), core.Location);
                Assert.That(Mean<uint>(core)(new[] { uint.MaxValue, uint.MaxValue }), Is.EqualTo((double)uint.MaxValue), core.Location);
                Assert.That(Mean<float>(core)(new[] { 1f, 3f, 6f }), Is.EqualTo(10.0 / 3), core.Location);
                Assert.That(Mean<double>(core)(new[] { 1.0, 3.0, 6.0 }), Is.EqualTo(10.0 / 3), core.Location);
                var fun = core.GetType("Aardvark.Base.Fun", true)!;
                var dec = (Func<IEnumerable<decimal>, decimal>)fun.GetMethod("Mean", new[] { typeof(IEnumerable<decimal>) })!
                    .CreateDelegate(typeof(Func<IEnumerable<decimal>, decimal>));
                Assert.That(dec(new[] { 1m, 3m, 6m }), Is.EqualTo(10m / 3), core.Location);
                Assert.Throws<DivideByZeroException>(() => dec(Array.Empty<decimal>()), core.Location);
                var selected = (Func<IEnumerable<int>, Func<int, double>, double>)fun.GetMethods()
                    .Single(m => m.Name == "Mean" && m.IsGenericMethodDefinition).MakeGenericMethod(typeof(int))
                    .CreateDelegate(typeof(Func<IEnumerable<int>, Func<int, double>, double>));
                int calls = 0;
                Assert.That(selected(new[] { 1, 3, 6 }, x => { calls++; return x; }), Is.EqualTo(10.0 / 3), core.Location);
                Assert.That(calls, Is.EqualTo(3), core.Location);
                Assert.That(double.IsNaN(selected(Array.Empty<int>(), x => x)), Is.True, core.Location);
            }
        }

        private sealed class MeanSequence<T> : IEnumerable<T>, IEnumerator<T>
        {
            private readonly T[] values;
            private readonly int fault;
            private int index = -1;
            public readonly List<string> Events = new();
            public readonly InvalidOperationException Failure = new("source failure");
            public readonly InvalidOperationException DisposeFailure = new("dispose failure");
            public MeanSequence(T[] values, int fault) { this.values = values; this.fault = fault; }
            public IEnumerator<T> GetEnumerator()
            {
                if (Events.Count != 0) throw new InvalidOperationException("Sequence acquired twice.");
                Events.Add("get");
                if (fault == 1) throw Failure;
                return this;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public bool MoveNext()
            {
                Events.Add($"move{++index}");
                if ((fault == 2 || fault == 5) && index == 1) throw Failure;
                return index < values.Length;
            }
            public T Current
            {
                get { Events.Add($"current{index}"); if (fault == 3 && index == 1) throw Failure; return values[index]; }
            }
            object IEnumerator.Current => Current!;
            public void Reset() => throw new NotSupportedException();
            public void Dispose() { Events.Add("dispose"); if (fault >= 4) throw DisposeFailure; }
        }
    }
}

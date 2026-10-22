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
    public class AbsSumTests
    {
        private static IEnumerable<Assembly> Cores()
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            yield return typeof(Fun).Assembly;
            yield return Assembly.LoadFile(path);
        }

        private static Func<IEnumerable<T>, R> Sum<T, R>(Assembly core)
            => (Func<IEnumerable<T>, R>)core.GetType("Aardvark.Base.Fun", true)!
                .GetMethod("AbsSum", new[] { typeof(IEnumerable<T>) })!
                .CreateDelegate(typeof(Func<IEnumerable<T>, R>));

        private static IEnumerable<T> Lazy<T>(T[] values)
        {
            foreach (var value in values) yield return value;
        }

        private static IEnumerable<IEnumerable<T>> Shapes<T>(T[] values)
        {
            yield return values;
            yield return values.ToList();
            yield return Lazy(values);
        }

        [Test]
        public void AbsoluteSumsIncludeMinimaAcrossSequenceShapes()
        {
            foreach (var core in Cores())
            {
                CheckValues<sbyte, int>(core, sbyte.MinValue, sbyte.MaxValue, x => (sbyte)x, x => x, 47131);
                CheckValues<short, long>(core, short.MinValue, short.MaxValue, x => (short)x, x => x, 47132);
                CheckValues<int, long>(core, int.MinValue, int.MaxValue, x => (int)x, x => x, 47133);
            }
        }

        private static void CheckValues<T, R>(Assembly core, BigInteger min, BigInteger max,
            Func<BigInteger, T> convert, Func<R, BigInteger> exact, int seed)
        {
            var sum = Sum<T, R>(core);
            void Check(BigInteger[] values, string context)
            {
                BigInteger expected = 0;
                foreach (var value in values) expected += BigInteger.Abs(value);
                var input = values.Select(convert).ToArray();
                int shape = 0;
                foreach (var source in Shapes(input))
                    Assert.That(exact(sum(source)), Is.EqualTo(expected),
                        $"{core.Location}: {typeof(T).Name}, {context}, n={values.Length}, shape={shape++}, expected={expected}");
            }

            Check(Array.Empty<BigInteger>(), "empty");
            var anchors = new[] { min, min + 1, new BigInteger(-2), -BigInteger.One, BigInteger.Zero, BigInteger.One, new BigInteger(2), max - 1, max };
            foreach (var value in anchors) Check(new[] { value }, $"singleton {value}");
            foreach (int n in new[] { 2, 3, 8, 64, 4096 }) Check(Enumerable.Repeat(min, n).ToArray(), $"repeated minimum {min}");
            foreach (var a in anchors)
            foreach (var b in anchors) Check(new[] { a, b }, $"pair {a},{b}");
            foreach (int n in new[] { 1, 2, 3, 8, 31, 64 })
            for (int position = 0; position < n; position++)
            {
                var values = Enumerable.Range(0, n).Select(i => new BigInteger((i & 1) == 0 ? 1 : -2)).ToArray();
                values[position] = min;
                Check(values, $"minimum at {position}");
                values[(position + 1) % n] = min;
                Check(values, $"mixed repeated minima at {position}");
            }
            if (typeof(T) != typeof(int))
                for (int value = (int)min; value <= (int)max; value++) Check(new[] { new BigInteger(value) }, $"exhaustive singleton {value}");
            var random = new Random(seed);
            var bytes = new byte[typeof(T) == typeof(sbyte) ? 1 : typeof(T) == typeof(short) ? 2 : 4];
            for (int sample = 0; sample < 1024; sample++)
            {
                var values = new BigInteger[random.Next(65)];
                for (int i = 0; i < values.Length; i++) { random.NextBytes(bytes); values[i] = new BigInteger(bytes, isUnsigned: false, isBigEndian: false); }
                if (values.Length > 0 && sample % 3 == 0) values[sample % values.Length] = min;
                Check(values, $"seed={seed}, sample={sample}");
            }
        }

        [Test]
        public void AbsoluteSumsKeepEnumerationAndSourceExceptions()
        {
            foreach (var core in Cores())
            {
                CheckEnumeration<sbyte, int>(core, x => (sbyte)x, x => x);
                CheckEnumeration<short, long>(core, x => (short)x, x => x);
                CheckEnumeration<int, long>(core, x => x, x => x);
            }
        }

        private static void CheckEnumeration<T, R>(Assembly core, Func<int, T> convert, Func<R, BigInteger> exact)
        {
            var sum = Sum<T, R>(core);
            var empty = new SumSequence<T>(Array.Empty<T>(), 0);
            Assert.That(exact(sum(empty)), Is.EqualTo(BigInteger.Zero), core.Location);
            Assert.That(string.Join(",", empty.Events), Is.EqualTo("get,move0,dispose"), core.Location);
            Assert.Throws<NullReferenceException>(() => sum(null!), $"{core.Location}: null {typeof(T).Name}");
            for (int fault = 0; fault <= 5; fault++)
            {
                var source = new SumSequence<T>(new[] { convert(-1), convert(3) }, fault);
                string context = $"{core.Location}: {typeof(T).Name}, fault={fault}";
                Assert.That(source.Events, Is.Empty, context);
                if (fault == 0) Assert.That(exact(sum(source)), Is.EqualTo(new BigInteger(4)), context);
                else Assert.That(Assert.Throws<InvalidOperationException>(() => sum(source)),
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
        public void ScalarAbsOtherSumsAndAccumulatorPoliciesStayUnchanged()
        {
            foreach (var core in Cores())
            {
                var fun = core.GetType("Aardvark.Base.Fun", true)!;
                void Scalar<T>(T min)
                {
                    var abs = (Func<T, T>)fun.GetMethod("Abs", new[] { typeof(T) })!.CreateDelegate(typeof(Func<T, T>));
                    Assert.Throws<OverflowException>(() => abs(min), $"{core.Location}: scalar Abs {typeof(T).Name}");
                }
                Scalar(sbyte.MinValue); Scalar(short.MinValue); Scalar(int.MinValue); Scalar(long.MinValue);
                var integral = Sum<long, long>(core);
                Assert.That(integral(Array.Empty<long>()), Is.Zero, core.Location);
                Assert.That(integral(new[] { -1L, 2L, -3L }), Is.EqualTo(6L), core.Location);
                Assert.That(integral(new[] { long.MaxValue, long.MaxValue }), Is.EqualTo(-2L), $"{core.Location}: legacy accumulator wrap");
                for (int position = 0; position < 3; position++)
                {
                    var input = new[] { -1L, 2L, 3L }; input[position] = long.MinValue;
                    foreach (var shape in Shapes(input)) Assert.Throws<OverflowException>(() => integral(shape), $"{core.Location}: long minimum at {position}");
                }
                var minimum = new SumSequence<long>(new[] { long.MinValue }, 0);
                Assert.Throws<OverflowException>(() => integral(minimum), core.Location);
                Assert.That(string.Join(",", minimum.Events), Is.EqualTo("get,move0,current0,dispose"), core.Location);
                const int count = 16909321;
                var total = new BigInteger(127) * count;
                int wrapped = (int)(total - (BigInteger.One << 32));
                Assert.That(Sum<sbyte, int>(core)(Enumerable.Repeat((sbyte)-127, count)), Is.EqualTo(wrapped), $"{core.Location}: sbyte accumulator wrap count={count}");
                var single = Sum<float, double>(core); var real = Sum<double, double>(core);
                Assert.That(single(new[] { -1.25f, 2.5f, -0.0f }), Is.EqualTo(3.75), core.Location);
                Assert.That(real(new[] { -1.25, 2.5, -0.0 }), Is.EqualTo(3.75), core.Location);
                Assert.That(single(Array.Empty<float>()), Is.Zero, core.Location);
                Assert.That(real(Array.Empty<double>()), Is.Zero, core.Location);
                Assert.That(double.IsNaN(single(new[] { float.NaN })), Is.True, core.Location);
                Assert.That(double.IsNaN(real(new[] { double.NaN })), Is.True, core.Location);
                Assert.That(single(new[] { float.NegativeInfinity }), Is.EqualTo(double.PositiveInfinity), core.Location);
                Assert.That(real(new[] { double.NegativeInfinity }), Is.EqualTo(double.PositiveInfinity), core.Location);
                var dec = Sum<decimal, decimal>(core);
                Assert.That(dec(new[] { -1.25m, 2.5m }), Is.EqualTo(3.75m), core.Location);
                Assert.That(dec(Array.Empty<decimal>()), Is.Zero, core.Location);
                Assert.Throws<OverflowException>(() => dec(new[] { decimal.MaxValue, 1m }), core.Location);
            }
        }

        private sealed class SumSequence<T> : IEnumerable<T>, IEnumerator<T>
        {
            private readonly T[] values;
            private readonly int fault;
            private int index = -1;
            public readonly List<string> Events = new();
            public readonly InvalidOperationException Failure = new("source failure");
            public readonly InvalidOperationException DisposeFailure = new("dispose failure");
            public SumSequence(T[] values, int fault) { this.values = values; this.fault = fault; }
            public IEnumerator<T> GetEnumerator()
            {
                if (Events.Count != 0) throw new InvalidOperationException("Sequence acquired twice.");
                Events.Add("get"); if (fault == 1) throw Failure; return this;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public bool MoveNext()
            {
                Events.Add($"move{++index}");
                if ((fault == 2 || fault == 5) && index == 1) throw Failure;
                return index < values.Length;
            }
            public T Current { get { Events.Add($"current{index}"); if (fault == 3 && index == 1) throw Failure; return values[index]; } }
            object IEnumerator.Current => Current!;
            public void Reset() => throw new NotSupportedException();
            public void Dispose() { Events.Add("dispose"); if (fault >= 4) throw DisposeFailure; }
        }
    }
}

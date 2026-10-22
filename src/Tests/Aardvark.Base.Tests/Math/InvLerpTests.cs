using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class InvLerpTests
    {
        private static IEnumerable<Assembly> Cores()
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            yield return typeof(Fun).Assembly;
            yield return Assembly.LoadFile(path);
        }

        private static Func<T, T, T, R> Bind<T, R>(Assembly core)
            => (Func<T, T, T, R>)core.GetType("Aardvark.Base.Fun", true)!
                .GetMethod("InvLerp", new[] { typeof(T), typeof(T), typeof(T) })!
                .CreateDelegate(typeof(Func<T, T, T, R>));

        // Convert an exact integer with explicit nearest/ties-even rounding, not BigInteger's double cast.
        private static double Rounded(BigInteger value)
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

        private static double Legacy(BigInteger y, BigInteger a, BigInteger b)
            => (Rounded(a) - Rounded(y)) / (Rounded(a) - Rounded(b));

        private static double Expected(BigInteger y, BigInteger a, BigInteger b)
            => a == b ? Legacy(y, a, b) : Rounded(a - y) / Rounded(a - b);

        private static void Same(double actual, double expected, string context)
        {
            if (double.IsNaN(expected)) Assert.That(double.IsNaN(actual), Is.True, context);
            else Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), context);
        }

        private static void Check(double actual, BigInteger y, BigInteger a, BigInteger b, string context)
        {
            double expected = Expected(y, a, b);
            string message = $"{context}: y={y}, a={a}, b={b}; expected={expected:R}, actual={actual:R}";
            if (a == b || expected == 0 || expected == 1) Same(actual, expected, message);
            else
            {
                Assert.That(double.IsFinite(actual), Is.True, message);
                double magnitude = Math.Abs(expected);
                double ulp = Math.BitIncrement(magnitude) - magnitude;
                Assert.That(actual, Is.EqualTo(expected).Within(2 * ulp), message);
            }
        }

        private static BigInteger[] Anchors(BigInteger min, BigInteger max)
        {
            var values = new List<BigInteger> { min, max, 0, 1, -1, 1024, -1024 };
            foreach (var center in new[] { min, max, BigInteger.One << 53, -(BigInteger.One << 53), BigInteger.One << 63 })
            foreach (int offset in new[] { -2049, -2048, -1024, -4, -3, -2, -1, 0, 1, 2, 3, 4, 1024, 2048, 2049 })
                values.Add(center + offset);
            return values.Where(v => v >= min && v <= max).Distinct().OrderBy(v => v).ToArray();
        }

        private static IEnumerable<(BigInteger Y, BigInteger A, BigInteger B)> Seeded(BigInteger min, BigInteger max, int seed, int count)
        {
            var random = new Random(seed);
            var bytes = new byte[8];
            BigInteger Next() { random.NextBytes(bytes); return new BigInteger(bytes, isUnsigned: min >= 0, isBigEndian: false); }
            BigInteger Clip(BigInteger x) => BigInteger.Min(max, BigInteger.Max(min, x));
            for (int i = 0; i < count; i++)
            {
                var a = Next();
                var b = (i & 1) == 0 ? Next() : Clip(a + random.Next(-4096, 4097));
                var y = (i & 1) == 0 ? Next() : Clip(a + random.Next(-8192, 8193));
                yield return (y, a, b);
            }
        }

        [Test]
        public void DistinctInt64EndpointsMatchExactDifferenceOracle()
        {
            foreach (var core in Cores())
            {
                CheckScalars<long>(core, long.MinValue, long.MaxValue, x => (long)x, 81371);
                CheckScalars<ulong>(core, ulong.MinValue, ulong.MaxValue, x => (ulong)x, 81372);
            }
        }

        private static void CheckScalars<T>(Assembly core, BigInteger min, BigInteger max, Func<BigInteger, T> convert, int seed)
        {
            var inv = Bind<T, double>(core);
            var anchors = Anchors(min, max);
            string context = $"{core.Location}: {typeof(T).Name}";
            foreach (var a in anchors)
            foreach (var b in anchors)
            {
                if (a == b) continue;
                foreach (var y in anchors) Check(inv(convert(y), convert(a), convert(b)), y, a, b, context);
            }
            foreach (var c in Seeded(min, max, seed, 4096))
                if (c.A != c.B) Check(inv(convert(c.Y), convert(c.A), convert(c.B)), c.Y, c.A, c.B, $"{context}; seed={seed}");
        }

        private static Func<T, T, T, double> Range<T>(Assembly core, string name)
        {
            var type = core.GetType($"Aardvark.Base.{name}", true)!;
            Assert.That(type.Assembly, Is.SameAs(core));
            var y = Expression.Parameter(typeof(T)); var a = Expression.Parameter(typeof(T)); var b = Expression.Parameter(typeof(T));
            var range = Expression.New(type.GetConstructor(new[] { typeof(T), typeof(T) })!, a, b);
            return Expression.Lambda<Func<T, T, T, double>>(Expression.Call(range, type.GetMethod("InvLerp", new[] { typeof(T) })!, y), y, a, b).Compile();
        }

        private static Func<long[], long[], long[], double[]> VectorOrBox(Assembly core, int dimension, bool box)
        {
            var input = core.GetType($"Aardvark.Base.V{dimension}l", true)!;
            var output = core.GetType($"Aardvark.Base.V{dimension}d", true)!;
            Assert.That(input.Assembly, Is.SameAs(core)); Assert.That(output.Assembly, Is.SameAs(core));
            var y = Expression.Parameter(typeof(long[])); var a = Expression.Parameter(typeof(long[])); var b = Expression.Parameter(typeof(long[]));
            var ctor = input.GetConstructor(Enumerable.Repeat(typeof(long), dimension).ToArray())!;
            NewExpression Make(ParameterExpression p) => Expression.New(ctor, Enumerable.Range(0, dimension).Select(i => Expression.ArrayIndex(p, Expression.Constant(i))));
            Expression call;
            if (box)
            {
                var type = core.GetType($"Aardvark.Base.Box{dimension}l", true)!;
                Assert.That(type.Assembly, Is.SameAs(core));
                var instance = Expression.New(type.GetConstructor(new[] { input, input })!, Make(a), Make(b));
                call = Expression.Call(instance, type.GetMethod("InvLerp", new[] { input })!, Make(y));
            }
            else call = Expression.Call(core.GetType("Aardvark.Base.Fun", true)!.GetMethod("InvLerp", new[] { input, input, input })!, Make(y), Make(a), Make(b));
            var result = Expression.Variable(output);
            var components = new[] { "X", "Y", "Z", "W" }.Take(dimension).Select(c => Expression.Field(result, c));
            return Expression.Lambda<Func<long[], long[], long[], double[]>>(Expression.Block(new[] { result },
                Expression.Assign(result, call), Expression.NewArrayInit(typeof(double), components)), y, a, b).Compile();
        }

        private static (BigInteger Y, BigInteger A, BigInteger B)[] WrapperCases(BigInteger min, BigInteger max, int seed)
        {
            var p = BigInteger.One << 53;
            var fixedCases = new[]
            {
                (max - 1, max - 2, max), (min + 1, min, min + 2),
                (p + 1, p - 1, p + 3), (p + 1, p + 2, p),
                (BigInteger.Zero, min, max), (max, min, BigInteger.Zero), (min, max, BigInteger.Zero),
                (max - 1, max, max), (BigInteger.Zero, max, max), (min, min, min + 2),
                (max, max, min), (max - 4, max - 2, max), (min + 4, min + 2, min)
            };
            return fixedCases.Concat(Seeded(min, max, seed, 256)).ToArray();
        }

        [Test]
        public void Int64RangesVectorsAndBoxesForwardEveryComponent()
        {
            foreach (var core in Cores())
            {
                var signed = WrapperCases(long.MinValue, long.MaxValue, 91371);
                var unsigned = WrapperCases(ulong.MinValue, ulong.MaxValue, 91372);
                var rangeL = Range<long>(core, "Range1l"); var rangeUL = Range<ulong>(core, "Range1ul");
                foreach (var c in signed) Check(rangeL((long)c.Y, (long)c.A, (long)c.B), c.Y, c.A, c.B, $"{core.Location}: Range1l");
                foreach (var c in unsigned) Check(rangeUL((ulong)c.Y, (ulong)c.A, (ulong)c.B), c.Y, c.A, c.B, $"{core.Location}: Range1ul");
                foreach (int dimension in new[] { 2, 3, 4 })
                foreach (bool box in dimension < 4 ? new[] { false, true } : new[] { false })
                {
                    var inv = VectorOrBox(core, dimension, box);
                    for (int sample = 0; sample < signed.Length; sample++)
                    {
                        var cases = Enumerable.Range(0, dimension).Select(i => signed[(sample + 17 * i) % signed.Length]).ToArray();
                        var actual = inv(cases.Select(c => (long)c.Y).ToArray(), cases.Select(c => (long)c.A).ToArray(), cases.Select(c => (long)c.B).ToArray());
                        for (int i = 0; i < dimension; i++) Check(actual[i], cases[i].Y, cases[i].A, cases[i].B,
                            $"{core.Location}: {(box ? "Box" : "V")}{dimension}l; sample={sample}, component={i}");
                    }
                }
            }
        }

        [Test]
        public void OrdinaryDegenerateAndOtherOverloadsKeepResultsAndZeroSigns()
        {
            foreach (var core in Cores())
            {
                CheckLegacy<long>(core, long.MinValue, long.MaxValue, x => (long)x);
                CheckLegacy<ulong>(core, ulong.MinValue, ulong.MaxValue, x => (ulong)x);
                CheckLegacy<byte>(core, byte.MinValue, byte.MaxValue, x => (byte)x);
                CheckLegacy<sbyte>(core, sbyte.MinValue, sbyte.MaxValue, x => (sbyte)x);
                CheckLegacy<short>(core, short.MinValue, short.MaxValue, x => (short)x);
                CheckLegacy<ushort>(core, ushort.MinValue, ushort.MaxValue, x => (ushort)x);
                CheckLegacy<int>(core, int.MinValue, int.MaxValue, x => (int)x);
                CheckLegacy<uint>(core, uint.MinValue, uint.MaxValue, x => (uint)x);
                var single = Bind<float, float>(core); var real = Bind<double, double>(core);
                foreach (double y in new[] { -8.0, -0.0, 0.0, 0.25, 1.0, 8.0, double.PositiveInfinity, double.NaN })
                foreach (double a in new[] { -2.0, -0.0, 0.0, 2.0 })
                foreach (double b in new[] { -2.0, 0.0, 2.0 })
                {
                    Same(real(y, a, b), (a - y) / (a - b), $"{core.Location}: double {y},{a},{b}");
                    Same(single((float)y, (float)a, (float)b), ((float)a - (float)y) / ((float)a - (float)b), $"{core.Location}: float {y},{a},{b}");
                }
                var dec = Bind<decimal, decimal>(core);
                Assert.That(dec(2m, 1m, 5m), Is.EqualTo(0.25m), core.Location);
                Assert.Throws<DivideByZeroException>(() => dec(1m, 0m, 0m), core.Location);
            }
        }

        private static void CheckLegacy<T>(Assembly core, BigInteger min, BigInteger max, Func<BigInteger, T> convert)
        {
            var inv = Bind<T, double>(core);
            var ordinary = new BigInteger[] { -4096, -7, -1, 0, 1, 7, 4096 }.Where(v => v >= min && v <= max).ToArray();
            foreach (var y in ordinary)
            foreach (var a in ordinary)
            foreach (var b in ordinary) Same(inv(convert(y), convert(a), convert(b)), Legacy(y, a, b), $"{core.Location}: {typeof(T).Name} ordinary {y},{a},{b}");
            var anchors = Anchors(min, max);
            foreach (var a in anchors)
            foreach (var y in anchors) Same(inv(convert(y), convert(a), convert(a)), Legacy(y, a, a), $"{core.Location}: {typeof(T).Name} degenerate y={y}, a=b={a}");
        }
    }
}

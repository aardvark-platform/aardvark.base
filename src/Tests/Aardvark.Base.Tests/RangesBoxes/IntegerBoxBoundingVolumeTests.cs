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
    public class IntegerBoxBoundingVolumeTests
    {
        private static IEnumerable<Assembly> Cores()
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            yield return typeof(Box2i).Assembly;
            yield return Assembly.LoadFile(path);
        }

        private static NewExpression IntegerBox(Assembly core, int dim, bool wide, ParameterExpression low, ParameterExpression high)
        {
            var scalar = wide ? typeof(long) : typeof(int);
            var type = core.GetType($"Aardvark.Base.Box{dim}{(wide ? "l" : "i")}", true)!;
            var values = new[] { low, high }.SelectMany(a => Enumerable.Range(0, dim).Select(i => Expression.Convert(Expression.ArrayIndex(a, Expression.Constant(i)), scalar)));
            return Expression.New(type.GetConstructor(Enumerable.Repeat(scalar, 2 * dim).ToArray())!, values);
        }

        // All constructed boxes, vectors and bounds belong to the selected core.
        // Only primitive arrays cross assembly identities.
        private static Func<long[], long[], double[]> Bound(Assembly core, int dim, bool wide, bool floating = false)
        {
            var low = Expression.Parameter(typeof(long[])); var high = Expression.Parameter(typeof(long[]));
            Expression box = IntegerBox(core, dim, wide, low, high);
            if (floating)
            {
                var type = core.GetType($"Aardvark.Base.Box{dim}d", true)!;
                box = Expression.New(type.GetConstructor(new[] { box.Type })!, box);
            }
            var property = Expression.Property(box, dim == 2 ? "BoundingCircle2d" : "BoundingSphere3d");
            var result = Expression.Variable(property.Type);
            var fields = Enumerable.Range(0, dim).Select(i => (Expression)Expression.Field(Expression.Field(result, "Center"), new[] { "X", "Y", "Z" }[i])).Append(Expression.Field(result, "Radius"));
            return Expression.Lambda<Func<long[], long[], double[]>>(Expression.Block(new[] { result }, Expression.Assign(result, property), Expression.NewArrayInit(typeof(double), fields)), low, high).Compile();
        }

        private static Func<long[], long[], long[]> IntegerProperties(Assembly core, int dim, bool wide)
        {
            var low = Expression.Parameter(typeof(long[])); var high = Expression.Parameter(typeof(long[]));
            var box = IntegerBox(core, dim, wide, low, high);
            var fields = new[] { "Center", "Size" }.SelectMany(p => Enumerable.Range(0, dim).Select(i => (Expression)Expression.Convert(Expression.Field(Expression.Property(box, p), new[] { "X", "Y", "Z" }[i]), typeof(long))));
            return Expression.Lambda<Func<long[], long[], long[]>>(Expression.NewArrayInit(typeof(long), fields), low, high).Compile();
        }

        private static double Length(double[] values)
        {
            double max = values.Max(Math.Abs);
            return max == 0 ? 0 : max * Math.Sqrt(values.Sum(x => (x / max) * (x / max)));
        }

        private static void CheckRepresentable(long[] low, long[] high, double[] bound, string context)
        {
            int dim = low.Length;
            for (int i = 0; i < dim; i++)
            {
                Assert.That(new BigInteger((double)low[i]), Is.EqualTo(new BigInteger(low[i])), $"{context}, representable min axis={i}");
                Assert.That(new BigInteger((double)high[i]), Is.EqualTo(new BigInteger(high[i])), $"{context}, representable max axis={i}");
                BigInteger sum = (BigInteger)low[i] + high[i];
                double center = (double)sum * 0.5;
                Assert.That(new BigInteger(center * 2), Is.EqualTo(sum), $"{context}, representable midpoint axis={i}");
                Assert.That(bound[i], Is.EqualTo(center), $"{context}, center axis={i}");
            }
            var diagonal = Enumerable.Range(0, dim).Select(i => (double)((BigInteger)high[i] - low[i])).ToArray();
            double radius = 0.5 * Length(diagonal), tolerance = 8 * 1.1102230246251565e-16 * Math.Max(1, radius);
            Assert.That(bound[dim], Is.EqualTo(radius).Within(tolerance), $"{context}, radius");
            for (int corner = 0; corner < (1 << dim); corner++)
            {
                var delta = Enumerable.Range(0, dim).Select(i => (double)(((corner >> i) & 1) == 0 ? low[i] : high[i]) - bound[i]).ToArray();
                Assert.That(Length(delta), Is.LessThanOrEqualTo(bound[dim] + tolerance), $"{context}, corner={corner}");
            }
            if (radius == 0) Assert.That(bound[dim], Is.Zero, context);
        }

        [Test]
        public void IntegerBoundsUseFloatingMidpointsAndDiagonals()
        {
            foreach (var core in Cores())
            foreach (int dim in new[] { 2, 3 })
            foreach (bool wide in new[] { false, true })
            {
                var get = Bound(core, dim, wide);
                void Check(long[] min, long[] max, string label)
                {
                    var low = min.Take(dim).ToArray(); var high = max.Take(dim).ToArray();
                    CheckRepresentable(low, high, get(low, high), $"{core.Location}: Box{dim}{(wide ? "l" : "i")}, {label}, min=({string.Join(",", low)}), max=({string.Join(",", high)})");
                }
                Check(new long[] { 0, 0, 0 }, new long[] { 1, 1, 1 }, "unit");
                Check(new long[] { 10, -21, 31 }, new long[] { 11, -18, 36 }, "translated odd extents");
                Check(new long[] { -11, -9, -7 }, new long[] { -2, -4, -2 }, "negative odd extents");
                Check(new long[] { -3, -1, -9 }, new long[] { 4, 2, 8 }, "mixed signs");
                Check(new long[] { -8, 12, -20 }, new long[] { -2, 18, -14 }, "even extents");
                Check(new long[] { 17, -29, 41 }, new long[] { 17, -29, 41 }, "point");
                for (int axis = 0; axis < dim; axis++)
                {
                    var low = new long[dim]; var high = Enumerable.Repeat(3L, dim).ToArray(); high[axis] = 0;
                    Check(low, high, $"degenerate axis={axis}");
                    if (!wide) { low[axis] = int.MinValue; high[axis] = int.MaxValue; Check(low, high, $"full signed span axis={axis}"); }
                }
                long positiveLow = wide ? long.MaxValue - 4095 : int.MaxValue - 100;
                long positiveHigh = wide ? long.MaxValue - 2047 : int.MaxValue - 2;
                long negativeLow = wide ? long.MinValue + 2048 : int.MinValue + 2;
                long negativeHigh = wide ? long.MinValue + 4096 : int.MinValue + 100;
                Check(Enumerable.Repeat(positiveLow, dim).ToArray(), Enumerable.Repeat(positiveHigh, dim).ToArray(), "overflowing positive midpoint sum");
                Check(Enumerable.Repeat(negativeLow, dim).ToArray(), Enumerable.Repeat(negativeHigh, dim).ToArray(), "overflowing negative midpoint sum");
                if (!wide) Check(Enumerable.Repeat((long)int.MinValue, dim).ToArray(), Enumerable.Repeat((long)int.MaxValue, dim).ToArray(), "all full signed spans");
                const int seed = 51329;
                var random = new Random(seed + dim + (wide ? 10 : 0));
                for (int sample = 0; sample < 2048; sample++)
                {
                    long limit = wide ? 1L << 40 : 1000000;
                    var low = Enumerable.Range(0, dim).Select(i => random.NextInt64(-limit, limit)).ToArray();
                    var high = low.Select(x => x + (sample % 2 == 0 ? 2 * random.Next(513) : 2 * random.Next(512) + 1)).ToArray();
                    Check(low, high, $"seed={seed + dim + (wide ? 10 : 0)}, sample={sample}");
                }
            }
        }

        private static long Wrapped(BigInteger value, bool wide)
        {
            BigInteger modulus = BigInteger.One << (wide ? 64 : 32);
            value %= modulus; if (value < 0) value += modulus;
            if (value >= modulus / 2) value -= modulus;
            return (long)value;
        }

        [Test]
        public void IntegerBoundsKeepInvalidChecksAndPublicIntegerProperties()
        {
            foreach (var core in Cores())
            foreach (int dim in new[] { 2, 3 })
            foreach (bool wide in new[] { false, true })
            {
                var get = Bound(core, dim, wide); var properties = IntegerProperties(core, dim, wide);
                long minimum = wide ? long.MinValue : int.MinValue, maximum = wide ? long.MaxValue : int.MaxValue;
                var cases = new List<(long[] low, long[] high)>
                {
                    (new long[dim], Enumerable.Repeat(1L, dim).ToArray()),
                    (Enumerable.Repeat(-7L, dim).ToArray(), Enumerable.Repeat(4L, dim).ToArray()),
                    (Enumerable.Repeat(minimum, dim).ToArray(), Enumerable.Repeat(maximum, dim).ToArray()),
                    (Enumerable.Repeat(maximum - 2, dim).ToArray(), Enumerable.Repeat(maximum, dim).ToArray()),
                    (Enumerable.Repeat(maximum, dim).ToArray(), Enumerable.Repeat(minimum, dim).ToArray())
                };
                for (int axis = 0; axis < dim; axis++)
                {
                    var low = new long[dim]; var high = new long[dim]; low[axis] = 1;
                    cases.Add((low, high));
                    if (wide)
                    {
                        low = new long[dim]; high = new long[dim]; low[axis] = (1L << 53) + 1; high[axis] = 1L << 53;
                        cases.Add((low, high));
                        low = new long[dim]; high = new long[dim]; low[axis] = long.MaxValue; high[axis] = long.MaxValue - 1;
                        cases.Add((low, high));
                    }
                }
                foreach (var row in cases)
                {
                    string context = $"{core.Location}: Box{dim}{(wide ? "l" : "i")}, min=({string.Join(",", row.low)}), max=({string.Join(",", row.high)})";
                    var actual = properties(row.low, row.high);
                    for (int i = 0; i < dim; i++)
                    {
                        Assert.That(actual[i], Is.EqualTo(Wrapped((BigInteger)row.low[i] + row.high[i], wide) / 2), $"{context}, legacy Center axis={i}");
                        Assert.That(actual[dim + i], Is.EqualTo(Wrapped((BigInteger)row.high[i] - row.low[i], wide)), $"{context}, legacy Size axis={i}");
                    }
                    if (Enumerable.Range(0, dim).Any(i => row.low[i] > row.high[i]))
                    {
                        var bound = get(row.low, row.high);
                        Assert.That(bound[dim], Is.EqualTo(-1), $"{context}, invalid radius");
                        for (int i = 0; i < dim; i++) Assert.That(double.IsNaN(bound[i]), Is.True, $"{context}, invalid center axis={i}");
                    }
                }
            }
        }

        [Test]
        public void LongBoundsExposeDoubleRoundingWithoutLosingIntegerInvalidity()
        {
            foreach (var core in Cores())
            foreach (int dim in new[] { 2, 3 })
            {
                var get = Bound(core, dim, true); var floating = Bound(core, dim, true, true);
                double[] Check(long[] low, long[] high, string label)
                {
                    string context = $"{core.Location}: Box{dim}l, {label}, min=({string.Join(",", low)}), max=({string.Join(",", high)})";
                    var actual = get(low, high); var reference = floating(low, high);
                    for (int i = 0; i <= dim; i++) Assert.That(BitConverter.DoubleToInt64Bits(actual[i]), Is.EqualTo(BitConverter.DoubleToInt64Bits(reference[i])), $"{context}, double-box component={i}");
                    for (int i = 0; i < dim; i++) Assert.That(actual[i], Is.EqualTo((double)low[i] * 0.5 + (double)high[i] * 0.5), $"{context}, rounded midpoint axis={i}");
                    double radius = 0.5 * Length(Enumerable.Range(0, dim).Select(i => (double)high[i] - (double)low[i]).ToArray());
                    Assert.That(actual[dim], Is.EqualTo(radius).Within(8 * 1.1102230246251565e-16 * Math.Max(1, radius)), $"{context}, rounded radius");
                    return actual;
                }
                var min = new long[dim]; var max = new long[dim]; min[0] = 1L << 53; max[0] = min[0] + 1;
                var collapsed = Check(min, max, "distinct integer endpoints collapse");
                Assert.That(collapsed[dim], Is.Zero, core.Location);
                Assert.That(new BigInteger(collapsed[0] * 2), Is.Not.EqualTo((BigInteger)min[0] + max[0]), "No exact Int64 midpoint guarantee");
                max[0] = min[0] + 2;
                var shifted = Check(min, max, "endpoints exact but midpoint unrepresentable");
                Assert.That((double)max[0] - shifted[0], Is.GreaterThan(shifted[dim]), "Double midpoint rounding can prevent exact corner enclosure");
                min[0] = long.MinValue; max[0] = long.MaxValue;
                var full = Check(min, max, "full signed span");
                Assert.That(full[0], Is.Zero, core.Location);
                Assert.That(full[dim], Is.EqualTo(Math.ScaleB(1, 63)), core.Location);
                var point = Enumerable.Repeat(long.MaxValue, dim).ToArray();
                var roundedPoint = Check(point, point, "maximum point rounds to 2^63");
                Assert.That(roundedPoint[dim], Is.Zero, core.Location);
                Assert.That(new BigInteger(roundedPoint[0]), Is.Not.EqualTo(new BigInteger(long.MaxValue)), core.Location);
                min = new long[dim]; max = new long[dim]; min[0] = (1L << 53) + 1; max[0] = 1L << 53;
                Assert.That(floating(min, max)[dim], Is.Zero, "Floating conversion alone loses this invalidity");
                Assert.That(get(min, max)[dim], Is.EqualTo(-1), "Integer invalidity must be checked before conversion");
                const int seed = 71843;
                var random = new Random(seed + dim); var bytes = new byte[8];
                for (int sample = 0; sample < 1024; sample++)
                {
                    min = new long[dim]; max = new long[dim];
                    for (int i = 0; i < dim; i++)
                    {
                        random.NextBytes(bytes); long a = BitConverter.ToInt64(bytes); random.NextBytes(bytes); long b = BitConverter.ToInt64(bytes);
                        min[i] = Math.Min(a, b); max[i] = Math.Max(a, b);
                    }
                    Check(min, max, $"full-width seed={seed + dim}, sample={sample}");
                }
            }
        }
    }
}

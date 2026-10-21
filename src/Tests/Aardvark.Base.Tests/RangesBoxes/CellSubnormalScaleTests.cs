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
    [TestFixture]
    public class CellSubnormalScaleTests
    {
        private sealed record Snapshot(long[] Index, int Exponent, double[] Min, double[] Max);

        private static IEnumerable<Assembly> Assemblies()
        {
            yield return typeof(Cell).Assembly;
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Cell).Assembly.Location)!, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static Snapshot Construct(Assembly assembly, int dimension, double[] min, double[] max, bool point)
        {
            var vector = assembly.GetType($"Aardvark.Base.V{dimension}d", true)!;
            var box = assembly.GetType($"Aardvark.Base.Box{dimension}d", true)!;
            var cell = assembly.GetType(dimension == 2 ? typeof(Cell2d).FullName! : typeof(Cell).FullName!, true)!;
            Assert.That(vector.Assembly, Is.SameAs(assembly));
            Assert.That(box.Assembly, Is.SameAs(assembly));
            var vectorCtor = vector.GetConstructor(Enumerable.Repeat(typeof(double), dimension).ToArray())!;
            object a = vectorCtor.Invoke(min.Take(dimension).Cast<object>().ToArray());
            object result = point ? cell.GetConstructor(new[] { vector })!.Invoke(new[] { a })
                : cell.GetConstructor(new[] { box })!.Invoke(new[] { box.GetConstructor(new[] { vector, vector })!.Invoke(
                    new[] { a, vectorCtor.Invoke(max.Take(dimension).Cast<object>().ToArray()) }) });
            object bounds = cell.GetProperty(nameof(Cell.BoundingBox))!.GetValue(result)!;
            Assert.That(bounds.GetType().Assembly, Is.SameAs(assembly));
            object lower = box.GetField("Min")!.GetValue(bounds)!, upper = box.GetField("Max")!.GetValue(bounds)!;
            var fields = "XYZ".Take(dimension).Select(c => c.ToString()).ToArray();
            return new Snapshot(fields.Select(f => (long)cell.GetField(f)!.GetValue(result)!).ToArray(),
                (int)cell.GetField(nameof(Cell.Exponent))!.GetValue(result)!,
                fields.Select(f => (double)vector.GetField(f)!.GetValue(lower)!).ToArray(),
                fields.Select(f => (double)vector.GetField(f)!.GetValue(upper)!).ToArray());
        }

        private static double Power(int exponent) => exponent < -1022
            ? BitConverter.Int64BitsToDouble(1L << (exponent + 1074))
            : BitConverter.Int64BitsToDouble((long)(exponent + 1023) << 52);

        private static int CompareToGrid(double value, BigInteger index, int exponent)
        {
            ulong bits = unchecked((ulong)BitConverter.DoubleToInt64Bits(value));
            int stored = (int)((bits >> 52) & 2047);
            BigInteger significand = bits & 0x000fffffffffffff;
            if (stored != 0) significand += BigInteger.One << 52;
            if ((bits >> 63) != 0) significand = -significand;
            int scale = stored == 0 ? -1074 : stored - 1023 - 52;
            int common = Math.Min(scale, exponent);
            return (significand << (scale - common)).CompareTo(index << (exponent - common));
        }

        private static void Contains(Snapshot cell, double[] min, double[] max, string context)
        {
            bool centered = cell.Index.All(x => x == long.MaxValue);
            for (int axis = 0; axis < cell.Index.Length; axis++)
            {
                int scale = centered ? cell.Exponent - 1 : cell.Exponent;
                BigInteger lower = centered ? -BigInteger.One : new BigInteger(cell.Index[axis]);
                BigInteger upper = centered ? BigInteger.One : lower + 1;
                Assert.That(CompareToGrid(min[axis], lower, scale), Is.GreaterThanOrEqualTo(0), context + $", exact lower axis {axis}");
                Assert.That(CompareToGrid(max[axis], upper, scale), Is.LessThan(0), context + $", exact exclusive upper axis {axis}");
                Assert.That(double.IsFinite(cell.Min[axis]) && double.IsFinite(cell.Max[axis]), Is.True, context);
                Assert.That(cell.Min[axis], Is.LessThanOrEqualTo(min[axis]), context + $", bounds lower axis {axis}");
                Assert.That(cell.Max[axis], Is.GreaterThanOrEqualTo(max[axis]), context + $", bounds upper axis {axis}");
            }
        }

        [Test]
        public void TinyCellsContainPointsAndInclusiveBoxes()
        {
            foreach (var assembly in Assemblies())
            for (int dimension = 2; dimension <= 3; dimension++)
            {
                for (int exponent = -1074; exponent <= -900; exponent++)
                foreach (double p in new[] { Power(exponent), Math.BitIncrement(Power(exponent)), Math.BitDecrement(Power(exponent)) })
                foreach (var coordinates in new[] { new[] { p, p, p }, new[] { -p, -p, -p }, new[] { p, 0, -p } })
                foreach (bool point in new[] { false, true })
                {
                    string context = $"{assembly.Location}, dimension {dimension}, point {point}, exponent {exponent}, [{string.Join(",", coordinates)}]";
                    Contains(Construct(assembly, dimension, coordinates, coordinates, point), coordinates, coordinates, context);
                }
                for (int exponent = -1074; exponent <= -1022; exponent++)
                foreach (int offset in new[] { 0, 2, 10, 30, 50 })
                {
                    double size = Power(exponent), origin = Power(exponent + offset);
                    foreach (var pair in new[]
                    {
                        (Min: new[] { origin, 0, -3 * size }, Max: new[] { origin + size, size, -2 * size }),
                        (Min: new[] { -size, -size, -size }, Max: new[] { size, size, size })
                    })
                        Contains(Construct(assembly, dimension, pair.Min, pair.Max, false), pair.Min, pair.Max,
                            $"{assembly.Location}, dimension {dimension}, box exponent {exponent}, offset {offset}");
                }
            }
        }

        private static int LegacyCeiling(double value)
        {
            ulong bits = unchecked((ulong)BitConverter.DoubleToInt64Bits(value));
            return (int)((bits >> 52) & 2047) - ((bits & 0x000fffffffffffff) != 0 ? 1022 : 1023);
        }

        private static Snapshot Legacy(double[] min, double[] max, int dimension)
        {
            double norm = Enumerable.Range(0, dimension).Max(i => Math.Abs(min[i]));
            bool centered = Enumerable.Range(0, dimension).Any(i => min[i] < 0 && max[i] >= 0);
            int exponent; var indices = new long[dimension];
            if (centered)
            {
                exponent = LegacyCeiling(Math.Max(norm, Enumerable.Range(0, dimension).Max(i => Math.Abs(max[i])))) + 1;
                double half = Math.Pow(2, exponent - 1);
                while (max.Take(dimension).Any(x => x >= half)) { exponent++; half *= 2; }
                Array.Fill(indices, long.MaxValue);
            }
            else
            {
                exponent = min.Take(dimension).SequenceEqual(max.Take(dimension))
                    ? LegacyCeiling(norm / (long.MaxValue >> 1))
                    : LegacyCeiling(Enumerable.Range(0, dimension).Max(i => Math.Abs(max[i] - min[i])));
                double size = Math.Pow(2, exponent), inverse = 1.0 / size;
                Assert.That(double.IsFinite(inverse), Is.True, "Finite reciprocal control corpus");
                for (int i = 0; i < dimension; i++) indices[i] = (long)Math.Floor(min[i] * inverse);
                int budget = 4096;
                while (Enumerable.Range(0, dimension).Any(i => max[i] >= indices[i] * size + size))
                {
                    Assert.That(--budget, Is.GreaterThan(0), "Legacy constructor control budget");
                    for (int i = 0; i < dimension; i++) indices[i] >>= 1;
                    exponent++; size *= 2;
                }
            }
            double scale = Math.Pow(2, exponent);
            var lower = indices.Select(i => centered ? -0.5 * scale : i * scale).ToArray();
            return new Snapshot(indices, exponent, lower, lower.Select(x => x + scale).ToArray());
        }

        [Test]
        public void FiniteReciprocalCellsKeepLegacyIndicesAndBounds()
        {
            foreach (var assembly in Assemblies())
            for (int dimension = 2; dimension <= 3; dimension++)
            {
                var random = new Random(41971 + dimension);
                for (int i = 0; i < 1024; i++)
                {
                    var min = Enumerable.Range(0, 3).Select(_ => random.Next(-256, 257) / 8.0).ToArray();
                    bool point = (i & 3) == 0;
                    var max = min.Select(x => point ? x : x + random.Next(0, 65) / 8.0).ToArray();
                    var expected = Legacy(min, max, dimension); var actual = Construct(assembly, dimension, min, max, point);
                    string context = $"{assembly.Location}, dimension {dimension}, case {i}, point {point}";
                    Assert.That(actual.Index, Is.EqualTo(expected.Index), context);
                    Assert.That(actual.Exponent, Is.EqualTo(expected.Exponent), context);
                    Assert.That(actual.Min.Select(BitConverter.DoubleToInt64Bits), Is.EqualTo(expected.Min.Select(BitConverter.DoubleToInt64Bits)), context);
                    Assert.That(actual.Max.Select(BitConverter.DoubleToInt64Bits), Is.EqualTo(expected.Max.Select(BitConverter.DoubleToInt64Bits)), context);
                }
            }
        }
    }
}

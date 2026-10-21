using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class BresenhamLineIteratorTests
    {
        private static IEnumerable<V2l> Raster(bool wide, V2l start, V2l end) => wide
            ? GeometryFun.BresenhamLineIterator(start, end)
            : GeometryFun.BresenhamLineIterator(new V2i((int)start.X, (int)start.Y),
                new V2i((int)end.X, (int)end.Y)).Select(p => new V2l(p.X, p.Y));

        // Independent rational rounding, not an incremental error recurrence.
        // Half-step ties advance the minor coordinate toward the directed endpoint.
        private static V2l Oracle(V2l start, V2l end, int step)
        {
            BigInteger dx = BigInteger.Abs((BigInteger)end.X - start.X);
            BigInteger dy = BigInteger.Abs((BigInteger)end.Y - start.Y);
            BigInteger major = BigInteger.Max(dx, dy), minor = BigInteger.Min(dx, dy);
            if (major.IsZero) return start;
            BigInteger rounded = BigInteger.DivRem(step * minor, major, out BigInteger remainder);
            if (2 * remainder >= major) rounded++;
            BigInteger x = (BigInteger)start.X + (end.X >= start.X ? 1 : -1) * (dx >= dy ? step : rounded);
            BigInteger y = (BigInteger)start.Y + (end.Y >= start.Y ? 1 : -1) * (dx >= dy ? rounded : step);
            return new V2l((long)x, (long)y);
        }

        private static void Check(bool wide, V2l start, V2l end, int limit = 64)
        {
            BigInteger count = BigInteger.Max(BigInteger.Abs((BigInteger)end.X - start.X),
                BigInteger.Abs((BigInteger)end.Y - start.Y)) + 1;
            int consumed = (int)BigInteger.Min(count, limit);
            using var iterator = Raster(wide, start, end).GetEnumerator();
            V2l previous = start;
            for (int i = 0; i < consumed; i++)
            {
                if (!iterator.MoveNext())
                    Assert.Fail($"{start} -> {end}: stopped before point {i} (wide={wide})");
                V2l actual = iterator.Current, expected = Oracle(start, end, i);
                if (actual != expected)
                    Assert.Fail($"{start} -> {end}, point {i}: {actual} != {expected} (wide={wide})");
                if (i > 0)
                {
                    BigInteger dx = BigInteger.Abs((BigInteger)actual.X - previous.X);
                    BigInteger dy = BigInteger.Abs((BigInteger)actual.Y - previous.Y);
                    if (BigInteger.Max(dx, dy) != 1)
                        Assert.Fail($"Disconnected or repeated points: {previous}, {actual}");
                }
                previous = actual;
            }
            if (count <= limit)
            {
                Assert.That(previous, Is.EqualTo(end), "Endpoint inclusion");
                Assert.That(iterator.MoveNext(), Is.False, "Exact cardinality");
                Assert.That(iterator.MoveNext(), Is.False, "Completed iterator stays completed");
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void FullSignedSpanDoesNotTerminateEarly(bool wide)
        {
            long min = wide ? long.MinValue : int.MinValue;
            long max = wide ? long.MaxValue : int.MaxValue;
            foreach (long minor in new long[] { 0, 1, -1, 100, -100 })
            {
                Check(wide, new V2l(min, 0), new V2l(max, minor));
                Check(wide, new V2l(max, minor), new V2l(min, 0));
                Check(wide, new V2l(0, min), new V2l(minor, max));
                Check(wide, new V2l(minor, max), new V2l(0, min));
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void LargePositiveSpanHasCorrectPrefix(bool wide)
        {
            long major = wide ? 6_000_000_000_000_000_000L : 1_500_000_000;
            long minor = major / 15;
            foreach (int sx in new[] { -1, 1 })
            foreach (int sy in new[] { -1, 1 })
            {
                V2l end = new V2l(sx * major, sy * minor);
                Check(wide, V2l.Zero, end);
                Check(wide, end, V2l.Zero);
                Check(wide, V2l.Zero, new V2l(end.Y, end.X));
                Check(wide, new V2l(end.Y, end.X), V2l.Zero);
            }
            Assert.That(Raster(wide, V2l.Zero, new V2l(major, minor)).Skip(9).First(),
                Is.EqualTo(new V2l(9, 1)));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExtremeSlopeClassesAndDistanceBoundaries(bool wide)
        {
            long min = wide ? long.MinValue : int.MinValue;
            long max = wide ? long.MaxValue : int.MaxValue;
            long[] values = { min, min + 1, min + 2, -1, 0, 1, max - 2, max - 1, max };
            foreach (long a in values)
            foreach (long b in values)
            {
                Check(wide, new V2l(a, a), new V2l(b, b));
                Check(wide, new V2l(a, b), new V2l(b, a));
                Check(wide, new V2l(a, 0), new V2l(b, max));
                Check(wide, new V2l(0, a), new V2l(min, b));
            }
            // Even full-width major distance and exact half-slope, beyond signed magnitudes.
            Check(wide, new V2l(min, 0), new V2l(max - 1, max));
            Check(wide, new V2l(max - 1, max), new V2l(min, 0));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void DirectedHalfStepTies(bool wide)
        {
            foreach (int sx in new[] { -1, 1 })
            foreach (int sy in new[] { -1, 1 })
            {
                V2l end = new V2l(2 * sx, sy);
                Assert.That(Raster(wide, V2l.Zero, end).ToArray(),
                    Is.EqualTo(new[] { V2l.Zero, new V2l(sx, sy), end }));
                Assert.That(Raster(wide, end, V2l.Zero).ToArray(),
                    Is.EqualTo(new[] { end, new V2l(sx, 0), V2l.Zero }));
                V2l swapped = new V2l(sy, 2 * sx);
                Assert.That(Raster(wide, V2l.Zero, swapped).ToArray(),
                    Is.EqualTo(new[] { V2l.Zero, new V2l(sy, sx), swapped }));
                Assert.That(Raster(wide, swapped, V2l.Zero).ToArray(),
                    Is.EqualTo(new[] { swapped, new V2l(0, sx), V2l.Zero }));
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExhaustiveSmallCoordinates(bool wide)
        {
            for (int x0 = -4; x0 <= 4; x0++)
            for (int y0 = -4; y0 <= 4; y0++)
            for (int x1 = -4; x1 <= 4; x1++)
            for (int y1 = -4; y1 <= 4; y1++)
                Check(wide, new V2l(x0, y0), new V2l(x1, y1));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ShortLinesTranslatedNearLimits(bool wide)
        {
            long min = wide ? long.MinValue : int.MinValue;
            long max = wide ? long.MaxValue : int.MaxValue;
            foreach (long x in new[] { min + 4, max - 4 })
            foreach (long y in new[] { min + 4, max - 4 })
            for (int dx = -4; dx <= 4; dx++)
            for (int dy = -4; dy <= 4; dy++)
            {
                Check(wide, new V2l(x, y), new V2l(x + dx, y + dy));
                Check(wide, new V2l(x + dx, y + dy), new V2l(x, y));
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void SeededFullDomainPrefixes(bool wide)
        {
            var random = new Random(517239);
            var bytes = new byte[8];
            long Coordinate()
            {
                random.NextBytes(bytes);
                return wide ? BitConverter.ToInt64(bytes, 0) : BitConverter.ToInt32(bytes, 0);
            }
            for (int i = 0; i < 2000; i++)
            {
                var start = new V2l(Coordinate(), Coordinate());
                var end = new V2l(Coordinate(), Coordinate());
                Check(wide, start, end);
                Check(wide, end, start);
                Check(wide, new V2l(start.Y, start.X), new V2l(end.Y, end.X));
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void LazyRepeatableAndIndependentEnumeration(bool wide)
        {
            long min = wide ? long.MinValue : int.MinValue;
            long max = wide ? long.MaxValue : int.MaxValue;
            var start = new V2l(min, max);
            var sequence = Raster(wide, start, new V2l(max, min));
            using (var a = sequence.GetEnumerator())
            using (var b = sequence.GetEnumerator())
            {
                Assert.That(a.MoveNext(), Is.True);
                Assert.That(a.Current, Is.EqualTo(start));
                Assert.That(a.MoveNext(), Is.True);
                Assert.That(b.MoveNext(), Is.True);
                Assert.That(b.Current, Is.EqualTo(start));
            }
            using (var restarted = sequence.GetEnumerator())
            {
                Assert.That(restarted.MoveNext(), Is.True);
                Assert.That(restarted.Current, Is.EqualTo(start));
            }
            var shortLine = Raster(wide, new V2l(2, -3), new V2l(-3, 1));
            Assert.That(shortLine.ToArray(), Is.EqualTo(shortLine.ToArray()));
            Check(wide, new V2l(min, max), new V2l(min, max));
            Check(wide, new V2l(max, min), new V2l(max, min));
        }

        private static long s_checksum;

        private static long AllocateAndTraverse(bool wide, int length, int count)
        {
            long before = GC.GetAllocatedBytesForCurrentThread(), checksum = 0;
            for (int i = 0; i < count; i++)
            {
                if (wide)
                {
                    foreach (var p in GeometryFun.BresenhamLineIterator(V2l.Zero, new V2l(length, length / 3)))
                        checksum = unchecked(checksum + p.X + p.Y);
                }
                else
                {
                    foreach (var p in GeometryFun.BresenhamLineIterator(V2i.Zero, new V2i(length, length / 3)))
                        checksum = unchecked(checksum + p.X + p.Y);
                }
            }
            long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
            s_checksum = checksum;
            return allocated;
        }

        [TestCase(false)]
        [TestCase(true)]
        public void OneIteratorAllocationAndNoPerPointAllocation(bool wide)
        {
            for (int i = 0; i < 8; i++) AllocateAndTraverse(wide, 128, 32);
            long singleton = AllocateAndTraverse(wide, 0, 256);
            long longer = AllocateAndTraverse(wide, 128, 256);
            Assert.That(longer, Is.EqualTo(singleton));
            Assert.That(longer, Is.LessThanOrEqualTo((wide ? 176L : 104L) * 256));
            Assert.That(singleton, Is.GreaterThan(0));
        }
    }
}

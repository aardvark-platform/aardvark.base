using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Edge = System.Action<long, long, long, long, double, double>;
using Center = System.Action<long, long, long, long, long, long, double, double, double, double>;
using DepthEdge = System.Action<int, long, long, long, long, double, double>;
using DepthCenter = System.Action<int, long, long, long, long, long, long, double, double, double, double>;
using Similar = System.Func<long, long, long, long, double, double, double, double, bool>;
using Point = System.Action<long, long, double, double>;
using Region = System.Action<long, long, long, long, double, double, double, double>;

namespace Aardvark.Tests
{
    [TestFixture]
    public class SampleGrid2dTests
    {
        private const double Tolerance = 1e-12;

        private delegate void Fill(Edge x, Edge y, Center xy, long a, long b, long c, long d);
        private delegate void DepthFill(DepthEdge x, DepthEdge y, DepthCenter xy, int depth, long a, long b, long c, long d);
        private delegate void AdaptiveSample(Similar similar, Point sample, Region region, Region super,
            long a, long b, long c, long d, double p, double q, double r, double s);
        private record Event(string Key, double[] Values);
        private record Cell(long A, long B, long C, long D);

        private static IEnumerable<Type> GridTypes()
        {
            yield return typeof(SampleGrid2d);
            string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, path);
            yield return Assembly.LoadFile(path).GetType(typeof(SampleGrid2d).FullName, true);
        }

        private static T Bind<T>(Type type, string name, int parameters) where T : Delegate
            => (T)type.GetMethods().Single(m => m.Name == name && m.GetParameters().Length == parameters)
                .CreateDelegate(typeof(T));

        private static IEnumerable<Cell> Cells()
        {
            long[] origins = { 0, -9, -3, -1, (1L << 62) - 3, (1L << 62) + 1,
                -(1L << 62) - 7, long.MaxValue - 7, long.MinValue, long.MinValue + 1 };
            for (int i = 0; i < origins.Length; i++)
            foreach (var size in new[] { (5, 1), (1, 7), (5, 7), (2, 3), (3, 2), (0, 0), (1, 1) })
                yield return new Cell(origins[i], origins[i] + size.Item1,
                    origins[(i + 3) % origins.Length], origins[(i + 3) % origins.Length] + size.Item2);
        }

        private static long Average(long a, long b) => (long)(((BigInteger)a + b) / 2);
        private static double Fraction(long a, long m, long b) => (double)((BigInteger)m - a) / (double)((BigInteger)b - a);
        private static bool Stop(Cell cell, int mode) => mode == 1 || (mode == 2 && cell.B - cell.A <= 3 && cell.D - cell.C <= 3);
        private static Event E(string kind, int depth, Cell cell, long x = 0, long y = 0, params double[] values)
            => new Event($"{kind}:{depth}:{cell.A},{cell.B},{cell.C},{cell.D}:{x},{y}", values);

        private static List<Event> Oracle(Cell root, bool sampling, bool depth, int initial, int mode)
        {
            var events = new List<Event>();
            void Visit(Cell cell, int level, double p, double q, double r, double s)
            {
                long a = cell.A, b = cell.B, c = cell.C, d = cell.D;
                bool splitX = b - a > 1, splitY = d - c > 1;
                if (!splitX && !splitY)
                {
                    if (sampling) events.Add(E("S", 0, cell, 0, 0, p, q, r, s));
                    return;
                }
                if (sampling)
                {
                    events.Add(E("Q", 0, cell, 0, 0, p, q, r, s));
                    if (Stop(cell, mode)) { events.Add(E("R", 0, cell, 0, 0, p, q, r, s)); return; }
                }
                long x = splitX ? Average(a, b) : a, y = splitY ? Average(c, d) : c;
                double fx = splitX ? Fraction(a, x, b) : 0, fy = splitY ? Fraction(c, y, d) : 0;
                double px = p + (q - p) * fx, py = r + (s - r) * fy;
                if (splitX)
                {
                    if (sampling)
                    {
                        events.Add(E("P", 0, new Cell(0, 0, 0, 0), x, c, px, r));
                        events.Add(E("P", 0, new Cell(0, 0, 0, 0), x, d, px, s));
                    }
                    else foreach (long edge in new[] { c, d })
                        events.Add(E("X", level, new Cell(a, b, edge, edge), x, edge, 1 - fx, fx));
                }
                if (splitY)
                {
                    if (sampling)
                    {
                        foreach (long edge in splitX ? new[] { a, x, b } : new[] { a, b })
                        {
                            double physicalX = edge == a ? p : edge == b ? q : px;
                            events.Add(E("P", 0, new Cell(0, 0, 0, 0), edge, y, physicalX, py));
                        }
                    }
                    else foreach (long edge in new[] { a, b })
                        events.Add(E("Y", level, new Cell(edge, edge, c, d), edge, y, 1 - fy, fy));
                }
                if (!sampling && splitX && splitY)
                    events.Add(E("XY", level, cell, x, y, 1 - fx, fx, 1 - fy, fy));
                int next = depth ? unchecked(level + 1) : 0;
                if (splitX && splitY)
                {
                    Visit(new Cell(a, x, c, y), next, p, px, r, py);
                    Visit(new Cell(x, b, c, y), next, px, q, r, py);
                    Visit(new Cell(a, x, y, d), next, p, px, py, s);
                    Visit(new Cell(x, b, y, d), next, px, q, py, s);
                }
                else if (splitX)
                {
                    Visit(new Cell(a, x, c, d), next, p, px, r, s);
                    Visit(new Cell(x, b, c, d), next, px, q, r, s);
                }
                else
                {
                    Visit(new Cell(a, b, c, y), next, p, q, r, py);
                    Visit(new Cell(a, b, y, d), next, p, q, py, s);
                }
            }
            Visit(root, initial, -3.5, 12.75, 7.25, -2.5);
            return events;
        }

        private static List<Event> Run(Type type, Cell cell, bool sampling, bool depth, int initial, int mode,
            int failAt = 0, Exception failure = null, List<Event> events = null)
        {
            events ??= new List<Event>();
            void Add(Event item)
            {
                if (events.Count >= 512) throw new InvalidOperationException("Adaptive callback budget exceeded.");
                events.Add(item);
                if (events.Count == failAt) throw failure;
            }
            void Interior(long a, long b, long m)
            {
                if (!(a < m && m < b)) throw new InvalidOperationException($"Midpoint {m} outside ({a},{b}).");
            }
            void X(int level, long a, long b, long y, long m, double p, double q)
            { Interior(a, b, m); Add(E("X", level, new Cell(a, b, y, y), m, y, p, q)); }
            void Y(int level, long x, long a, long b, long m, double p, double q)
            { Interior(a, b, m); Add(E("Y", level, new Cell(x, x, a, b), x, m, p, q)); }
            void XY(int level, long a, long b, long c, long d, long x, long y, double p, double q, double r, double s)
            { Interior(a, b, x); Interior(c, d, y); Add(E("XY", level, new Cell(a, b, c, d), x, y, p, q, r, s)); }
            if (!sampling)
            {
                if (depth) Bind<DepthFill>(type, "AdaptivelyFill", 8)(X, Y, XY, initial, cell.A, cell.B, cell.C, cell.D);
                else Bind<Fill>(type, "AdaptivelyFill", 7)(
                    (a, b, y, m, p, q) => X(0, a, b, y, m, p, q),
                    (x, a, b, m, p, q) => Y(0, x, a, b, m, p, q),
                    (a, b, c, d, x, y, p, q, r, s) => XY(0, a, b, c, d, x, y, p, q, r, s),
                    cell.A, cell.B, cell.C, cell.D);
            }
            else
            {
                Cell current = cell;
                Bind<AdaptiveSample>(type, "AdaptivelySample", 12)(
                    (a, b, c, d, p, q, r, s) =>
                    {
                        current = new Cell(a, b, c, d);
                        if (a < cell.A || b > cell.B || c < cell.C || d > cell.D || a > b || c > d)
                            throw new InvalidOperationException("Subdivision escaped its root.");
                        Add(E("Q", 0, current, 0, 0, p, q, r, s)); return Stop(current, mode);
                    },
                    (x, y, p, q) =>
                    {
                        if (x < current.A || x > current.B || y < current.C || y > current.D ||
                            !(x > current.A && x < current.B || y > current.C && y < current.D))
                            throw new InvalidOperationException("Sample is not an interior subdivision point.");
                        Add(E("P", 0, new Cell(0, 0, 0, 0), x, y, p, q));
                        if (Math.Abs(p - (-3.5 + 16.25 * Fraction(cell.A, x, cell.B))) > Tolerance ||
                            Math.Abs(q - (7.25 - 9.75 * Fraction(cell.C, y, cell.D))) > Tolerance)
                            throw new InvalidOperationException("Physical sample coordinates changed.");
                    },
                    (a, b, c, d, p, q, r, s) => Add(E("R", 0, new Cell(a, b, c, d), 0, 0, p, q, r, s)),
                    (a, b, c, d, p, q, r, s) => Add(E("S", 0, new Cell(a, b, c, d), 0, 0, p, q, r, s)),
                    cell.A, cell.B, cell.C, cell.D, -3.5, 12.75, 7.25, -2.5);
            }
            return events;
        }

        private static void AssertEvents(List<Event> actual, List<Event> expected, string context)
        {
            Assert.That(actual.Select(e => e.Key), Is.EqualTo(expected.Select(e => e.Key)), context + ", order/indices/depth");
            if (actual.Count != expected.Count) return;
            for (int i = 0; i < actual.Count; i++)
                Assert.That(actual[i].Values, Is.EqualTo(expected[i].Values).Within(Tolerance), context + $", event={i}, {expected[i].Key}");
        }

        [Test]
        public void AdaptiveFillMatchesBigIntegerMidpointsWeightsAndDepth()
        {
            Assert.Multiple(() =>
            {
                foreach (var type in GridTypes())
                foreach (var cell in Cells())
                foreach (bool depth in new[] { false, true })
                foreach (int initial in depth ? new[] { 7, -3, int.MaxValue - 1 } : new[] { 0 })
                {
                    string context = $"{type.Assembly.Location}, {cell}, depth={depth}, initial={initial}";
                    Assert.DoesNotThrow(() => AssertEvents(Run(type, cell, false, depth, initial, 0),
                        Oracle(cell, false, depth, initial, 0), context), context);
                }
            });
        }

        [Test]
        public void AdaptiveSamplingMatchesBigIntegerMidpointsCoordinatesAndExits()
        {
            Assert.Multiple(() =>
            {
                foreach (var type in GridTypes())
                foreach (var cell in Cells())
                foreach (int mode in new[] { 0, 1, 2 })
                {
                    string context = $"{type.Assembly.Location}, {cell}, similarity mode={mode}";
                    Assert.DoesNotThrow(() => AssertEvents(Run(type, cell, true, false, 0, mode),
                        Oracle(cell, true, false, 0, mode), context), context);
                }
            });
        }

        [Test]
        public void AdaptiveCallbackFailuresPreserveIdentityAndStopImmediately()
        {
            var cell = new Cell(-3, 4, -2, 3);
            foreach (var type in GridTypes())
            foreach (int api in new[] { 0, 1, 2 })
            foreach (int mode in api == 2 ? new[] { 0, 1 } : new[] { 0 })
            {
                var expected = Oracle(cell, api == 2, api == 1, api == 1 ? 7 : 0, mode);
                // Include the first occurrence of every callback kind, including terminal callbacks.
                foreach (int position in expected.Select((e, i) => (e.Key.Split(':')[0], i + 1)).GroupBy(e => e.Item1).Select(g => g.First().Item2))
                {
                    var failure = new InvalidOperationException("callback failure");
                    var actual = new List<Event>();
                    string context = $"{type.Assembly.Location}, api={api}, mode={mode}, callback={position}";
                    Assert.That(Assert.Throws<InvalidOperationException>(() => Run(type, cell, api == 2, api == 1,
                        api == 1 ? 7 : 0, mode, position, failure, actual)), Is.SameAs(failure), context);
                    AssertEvents(actual, expected.Take(position).ToList(), context);
                }
            }
        }

        private static void AssertSharedCoordinate(
            Dictionary<long, double> coordinates, long index, double coordinate)
        {
            if (coordinates.TryGetValue(index, out double existing))
                Assert.That(coordinate, Is.EqualTo(existing));
            else
                coordinates.Add(index, coordinate);
        }

        private static List<(long MinX, long MaxX, long MinY, long MaxY)> CollectAndValidate(
            V2l gridSize, Box2d box, V2l step)
        {
            var last = gridSize - new V2l(1, 1);
            var delta = box.Size / (V2d)last;
            var coverage = new int[(int)last.X, (int)last.Y];
            var regions = new List<(long, long, long, long)>();
            var xCoordinates = new Dictionary<long, double>();
            var yCoordinates = new Dictionary<long, double>();
            var grid = new SampleGrid2d(gridSize, box);

            grid.SampleRegular(step, (minXi, maxXi, minYi, maxYi, minX, maxX, minY, maxY) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(minXi, Is.GreaterThanOrEqualTo(0));
                    Assert.That(minYi, Is.GreaterThanOrEqualTo(0));
                    Assert.That(maxXi, Is.LessThanOrEqualTo(last.X));
                    Assert.That(maxYi, Is.LessThanOrEqualTo(last.Y));
                    Assert.That(minXi, Is.LessThan(maxXi));
                    Assert.That(minYi, Is.LessThan(maxYi));

                    Assert.That(minX, Is.GreaterThanOrEqualTo(box.Min.X - Tolerance));
                    Assert.That(minY, Is.GreaterThanOrEqualTo(box.Min.Y - Tolerance));
                    Assert.That(maxX, Is.LessThanOrEqualTo(box.Max.X + Tolerance));
                    Assert.That(maxY, Is.LessThanOrEqualTo(box.Max.Y + Tolerance));
                    Assert.That(minX, Is.LessThan(maxX));
                    Assert.That(minY, Is.LessThan(maxY));

                    Assert.That(
                        minX,
                        Is.EqualTo(box.Min.X + minXi * delta.X).Within(Tolerance));
                    Assert.That(
                        maxX,
                        Is.EqualTo(box.Min.X + maxXi * delta.X).Within(Tolerance));
                    Assert.That(
                        minY,
                        Is.EqualTo(box.Min.Y + minYi * delta.Y).Within(Tolerance));
                    Assert.That(
                        maxY,
                        Is.EqualTo(box.Min.Y + maxYi * delta.Y).Within(Tolerance));
                });

                AssertSharedCoordinate(xCoordinates, minXi, minX);
                AssertSharedCoordinate(xCoordinates, maxXi, maxX);
                AssertSharedCoordinate(yCoordinates, minYi, minY);
                AssertSharedCoordinate(yCoordinates, maxYi, maxY);

                regions.Add((minXi, maxXi, minYi, maxYi));
                for (long y = minYi; y < maxYi; y++)
                    for (long x = minXi; x < maxXi; x++)
                        coverage[(int)x, (int)y]++;
            });

            for (int y = 0; y < coverage.GetLength(1); y++)
                for (int x = 0; x < coverage.GetLength(0); x++)
                    Assert.That(
                        coverage[x, y],
                        Is.EqualTo(1),
                        $"Grid cell ({x}, {y}) was covered {coverage[x, y]} times.");

            return regions;
        }

        private static void AssertRegions(
            IReadOnlyList<(long MinX, long MaxX, long MinY, long MaxY)> actual,
            long[] xs,
            long[] ys)
        {
            var expected = new List<(long, long, long, long)>();
            for (int y = 0; y + 1 < ys.Length; y++)
                for (int x = 0; x + 1 < xs.Length; x++)
                    expected.Add((xs[x], xs[x + 1], ys[y], ys[y + 1]));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void RegionSampleRegularExactFitUsesExpectedCells()
        {
            var regions = CollectAndValidate(
                new V2l(9, 7),
                new Box2d(new V2d(-2.0, 3.0), new V2d(14.0, 15.0)),
                new V2l(4, 3));

            AssertRegions(regions, new long[] { 0, 4, 8 }, new long[] { 0, 3, 6 });
        }

        [Test]
        public void RegionSampleRegularDistributesUnevenBorders()
        {
            var regions = CollectAndValidate(
                new V2l(12, 11),
                new Box2d(new V2d(1.25, -4.5), new V2d(23.25, 25.5)),
                new V2l(4, 4));

            AssertRegions(
                regions,
                new long[] { 0, 1, 5, 9, 11 },
                new long[] { 0, 1, 5, 9, 10 });
        }

        [Test]
        public void RegionSampleRegularHandlesOversizedStep()
        {
            var regions = CollectAndValidate(
                new V2l(6, 5),
                new Box2d(new V2d(-8.0, -3.0), new V2d(7.0, 9.0)),
                new V2l(10, 9));

            AssertRegions(regions, new long[] { 0, 2, 5 }, new long[] { 0, 2, 4 });
        }

        [Test]
        public void RegionSampleRegularSweepPreservesEndpointAndCoverageContract()
        {
            var box = new Box2d(new V2d(-13.25, 2.75), new V2d(17.5, 31.125));

            for (long sizeY = 2; sizeY <= 9; sizeY++)
                for (long sizeX = 2; sizeX <= 9; sizeX++)
                    for (long stepY = 1; stepY <= sizeY + 1; stepY++)
                        for (long stepX = 1; stepX <= sizeX + 1; stepX++)
                            CollectAndValidate(
                                new V2l(sizeX, sizeY),
                                box,
                                new V2l(stepX, stepY));
        }
    }
}

using Aardvark.Base;
using NUnit.Framework;
using System.Collections.Generic;

namespace Aardvark.Tests
{
    [TestFixture]
    public class SampleGrid2dTests
    {
        private const double Tolerance = 1e-12;

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

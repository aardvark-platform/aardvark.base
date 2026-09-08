using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Aardvark.Tests
{
    internal static class CellIntersectionTestData
    {
        public static IEnumerable<TestCaseData> Cases(int dimensions)
        {
            foreach (var (name, a, b, expected) in Pairs(dimensions))
            {
                yield return dimensions == 3
                    ? new TestCaseData(a, b, expected).SetName("IntersectsExactly_" + name)
                    : new TestCaseData(Project(a), Project(b), expected).SetName("IntersectsExactly_" + name);
            }
        }

        private static IEnumerable<(string Name, Cell A, Cell B, bool Expected)> Pairs(int dimensions)
        {
            yield return ("BeyondDoublePrecision", new Cell(1L << 53, 0, 0, 0), new Cell(1L << 54, 0, 0, -1), true);
            yield return ("UnderflowParentChild", new Cell(0, 0, 0, -1100), new Cell(0, 0, 0, -1101), true);
            yield return ("OverflowDisjoint", new Cell(0, 0, 0, 1100), new Cell(2, 0, 0, 1100), false);

            foreach (int gap in new[] { 0, 1, 62, 63, 64, 65, 127, int.MaxValue })
            {
                // A shift by 63 or more is sign extension, not C#'s masked shift count.
                long positiveParent = long.MaxValue >> Math.Min(gap, 63);
                long negativeParent = long.MinValue >> Math.Min(gap, 63);
                var finePositive = new Cell(long.MaxValue, 0, 0, -1);
                var fineNegative = new Cell(long.MinValue, 0, 0, -1);
                yield return ($"PositiveGap{gap}", new Cell(positiveParent, 0, 0, gap - 1), finePositive, true);
                yield return ($"NegativeGap{gap}", new Cell(negativeParent, 0, 0, gap - 1), fineNegative, true);
                yield return ($"DisjointPositiveGap{gap}", new Cell(-1, 0, 0, gap - 1), finePositive, false);
                yield return ($"DisjointNegativeGap{gap}", new Cell(0, 0, 0, gap - 1), fineNegative, false);
            }

            yield return ("FullExponentRangePositive", new Cell(0, 0, 0, int.MaxValue), new Cell(long.MaxValue, 0, 0, int.MinValue), true);
            yield return ("FullExponentRangeNegative", new Cell(-1, -1, -1, int.MaxValue), new Cell(long.MinValue, -1, -1, int.MinValue), true);
            yield return ("FullExponentRangeDisjoint", new Cell(1, 0, 0, int.MaxValue), new Cell(long.MaxValue, 0, 0, int.MinValue), false);
            yield return ("FullExponentRangeAcrossOrigin", new Cell(0, 0, 0, int.MaxValue), new Cell(long.MinValue, 0, 0, int.MinValue), false);
            yield return ("SameCoordinatesDifferentScale", new Cell(1, 0, 0, 0), new Cell(1, 0, 0, -1), false);
            yield return ("LongMaxEndpoint", new Cell(long.MaxValue, 0, 0, 0), new Cell(long.MaxValue >> 1, 0, 0, 1), true);
            yield return ("LongMinEndpoint", new Cell(long.MinValue, 0, 0, 0), new Cell(long.MinValue >> 1, 0, 0, 1), true);

            var smallestCentered = new Cell(long.MaxValue, long.MaxValue, long.MaxValue, int.MinValue);
            yield return ("CenteredFullExponentRange", smallestCentered, new Cell(int.MaxValue), true);
            yield return ("CenteredNegativeExponentGapPositiveOutside", smallestCentered, new Cell(1, 0, 0, 0), false);
            yield return ("CenteredNegativeExponentGapNegativeOutside", smallestCentered, new Cell(-2, 0, 0, 0), false);
            yield return ("CenteredUnderflow", new Cell(-1100), new Cell(-1101), true);
            yield return ("CenteredOverflow", new Cell(1100), new Cell(1101), true);
            yield return ("CenteredContainsAllFiniteCoordinates", new Cell(int.MaxValue), new Cell(long.MinValue, long.MaxValue, 0, int.MinValue), true);

            foreach (int centeredExponent in new[] { int.MinValue, -1100, -1, 0, 1, 62, 63, 64, 65, 1100, int.MaxValue })
            {
                var centered = new Cell(long.MaxValue, long.MaxValue, long.MaxValue, centeredExponent);
                // Tiny centered cells still partially overlap every origin-adjacent ordinary cell.
                for (int signs = 0; signs < (1 << dimensions); signs++)
                {
                    var ordinary = new Cell((signs & 1) == 0 ? -1 : 0, (signs & 2) == 0 ? -1 : 0,
                        (signs & 4) == 0 ? -1 : 0, int.MaxValue);
                    yield return ($"CenteredPartial{centeredExponent}_{signs}", centered, ordinary, true);
                }
            }

            foreach (int gap in new[] { 0, 1, 62, 63, 64, 65 })
            {
                // C's radius is 2^(gap-1), while the ordinary cell has unit size.
                var centered = new Cell(gap);
                long insidePositive = gap == 0 ? 0 : gap >= 64 ? long.MaxValue : (1L << (gap - 1)) - 1;
                long insideNegative = gap == 0 ? -1 : gap >= 64 ? long.MinValue : -(1L << (gap - 1));
                yield return ($"CenteredPositiveGap{gap}", centered, new Cell(insidePositive, 0, 0, 0), true);
                yield return ($"CenteredNegativeGap{gap}", centered, new Cell(insideNegative, 0, 0, 0), true);
                if (gap < 64)
                {
                    long outsidePositive = gap == 0 ? 1 : 1L << (gap - 1);
                    long outsideNegative = gap == 0 ? -2 : -(1L << (gap - 1)) - 1;
                    for (int axis = 0; axis < dimensions; axis++)
                    {
                        yield return ($"CenteredPositiveBoundary{gap}_{axis}", centered, OnAxis(axis, outsidePositive, 0), false);
                        yield return ($"CenteredNegativeBoundary{gap}_{axis}", centered, OnAxis(axis, outsideNegative, 0), false);
                    }
                }
            }

            foreach (int exponent in new[] { int.MinValue, -1100, 0, 1100, int.MaxValue })
            {
                // Every nonempty subset of axes: faces, edges, and corners.
                for (int axes = 1; axes < (1 << dimensions); axes++)
                {
                    var neighbor = new Cell(axes & 1, (axes >> 1) & 1, (axes >> 2) & 1, exponent);
                    yield return ($"BoundaryOnly{exponent}_{axes}", new Cell(0, 0, 0, exponent), neighbor, false);
                }
                for (int axis = 0; axis < dimensions; axis++)
                {
                    yield return ($"LongMaxNeighbors{exponent}_{axis}", OnAxis(axis, long.MaxValue, exponent), OnAxis(axis, long.MaxValue - 1, exponent), false);
                    yield return ($"LongMinNeighbors{exponent}_{axis}", OnAxis(axis, long.MinValue, exponent), OnAxis(axis, long.MinValue + 1, exponent), false);
                }
            }
        }

        private static Cell OnAxis(int axis, long coordinate, int exponent)
            => new Cell(axis == 0 ? coordinate : 0, axis == 1 ? coordinate : 0, axis == 2 ? coordinate : 0, exponent);

        private static Cell2d Project(Cell cell) => new Cell2d(cell.X, cell.Y, cell.Exponent);

        public static void CompareRandomPairs(int dimensions, int seed)
        {
            var random = new Random(seed);
            for (int i = 0; i < 20000; i++)
            {
                var a = RandomCell(random);
                Cell b;
                switch (i % 4)
                {
                    case 0:
                        b = RandomCell(random);
                        break;
                    case 1:
                    case 2:
                        int exponent = Math.Min(1200, a.Exponent + random.Next(131));
                        int shift = Math.Min(63, exponent - a.Exponent);
                        b = a.IsCenteredAtOrigin ? new Cell(exponent) :
                            new Cell(a.X >> shift, a.Y >> shift, a.Z >> shift, exponent);
                        if (i % 4 == 2 && !b.IsCenteredAtOrigin)
                            b = new Cell(unchecked(b.X + 1), b.Y, b.Z, b.Exponent);
                        break;
                    default:
                        a = new Cell(random.Next(-1200, 1201));
                        b = RandomCell(random);
                        break;
                }

                bool ac = dimensions == 2 ? Project(a).IsCenteredAtOrigin : a.IsCenteredAtOrigin;
                bool bc = dimensions == 2 ? Project(b).IsCenteredAtOrigin : b.IsCenteredAtOrigin;
                bool expected = AxisOverlap(a.X, a.Exponent, ac, b.X, b.Exponent, bc) &&
                    AxisOverlap(a.Y, a.Exponent, ac, b.Y, b.Exponent, bc) &&
                    (dimensions == 2 || AxisOverlap(a.Z, a.Exponent, ac, b.Z, b.Exponent, bc));
                if (dimensions == 3)
                {
                    Assert.That(a.Intersects(b), Is.EqualTo(expected), $"{a} versus {b}");
                    Assert.That(b.Intersects(a), Is.EqualTo(expected), $"{b} versus {a}");
                    Assert.That(a.Intersects(a), Is.True);
                }
                else
                {
                    var a2 = Project(a);
                    var b2 = Project(b);
                    Assert.That(a2.Intersects(b2), Is.EqualTo(expected), $"{a2} versus {b2}");
                    Assert.That(b2.Intersects(a2), Is.EqualTo(expected), $"{b2} versus {a2}");
                    Assert.That(a2.Intersects(a2), Is.True);
                }
            }
        }

        // BigInteger endpoints provide an independent interval oracle.
        private static bool AxisOverlap(long a, int ae, bool ac, long b, int be, bool bc)
        {
            int unitExponent = Math.Min(ae, be) - 1; // Random test exponents are bounded to [-1200, 1200].
            var (aMin, aMax) = Interval(a, ae, ac, unitExponent);
            var (bMin, bMax) = Interval(b, be, bc, unitExponent);
            return aMin < bMax && bMin < aMax;
        }

        private static (BigInteger Min, BigInteger Max) Interval(long coordinate, int exponent, bool centered, int unitExponent)
        {
            if (centered)
            {
                var radius = BigInteger.One << (exponent - unitExponent - 1);
                return (-radius, radius);
            }
            var size = BigInteger.One << (exponent - unitExponent);
            return (coordinate * size, ((BigInteger)coordinate + 1) * size);
        }

        private static Cell RandomCell(Random random)
        {
            int exponent = random.Next(-1200, 1201);
            if (random.Next(4) == 0) return new Cell(exponent);
            long x = Coordinate(random), y = Coordinate(random), z = Coordinate(random);
            return new Cell(x, y, z, exponent);
        }

        private static long Coordinate(Random random)
        {
            switch (random.Next(8))
            {
                case 0: return long.MinValue;
                case 1: return long.MaxValue;
                case 2: return (1L << 53) + random.Next(-2, 3);
                case 3: return -(1L << 53) + random.Next(-2, 3);
                case 4: return random.NextInt64(long.MinValue, long.MaxValue);
                default: return random.Next(-3, 4);
            }
        }

        public static IEnumerable<TestCaseData> InvalidCases(int dimensions)
        {
            var pairs = new (Cell Other, bool Expected)[]
            {
                (Cell.Invalid, true), (Cell.Unit, false), (new Cell(-1, -1, -1, 0), false),
                (new Cell(0), true), (new Cell(-1073), true), (new Cell(-1074), false),
                (new Cell(-1100), false), (new Cell(1100), true),
                (new Cell(0, 0, 0, 1100), true), (new Cell(2, 0, 0, 1100), false),
                (new Cell(-1, -1, -1, 1100), true),
                (new Cell(long.MaxValue, long.MaxValue, long.MaxValue, int.MinValue), false),
                (new Cell(long.MinValue, long.MinValue, long.MinValue, int.MinValue + 1), false)
            };
            for (int i = 0; i < pairs.Length; i++)
            {
                var (other, expected) = pairs[i];
                yield return dimensions == 3
                    ? new TestCaseData(other, expected).SetName("IntersectsInvalidCompatibility_" + i)
                    : new TestCaseData(Project(other), expected).SetName("IntersectsInvalidCompatibility_" + i);
            }
        }
    }
}

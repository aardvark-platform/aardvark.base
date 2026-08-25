using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PolylineSimplificationTests
    {
        [Test]
        public void CardinalityBoundariesHaveCanonicalIndices()
        {
            Assert.That(Array.Empty<V2f>().Simplify(0.0f), Is.Empty);
            Assert.That(new[] { new V2f(1, 2) }.Simplify(0.0f), Is.EqualTo(new[] { 0 }));
            Assert.That(new[] { new V2f(1, 2), new V2f(3, 4) }.Simplify(0.0f), Is.EqualTo(new[] { 0, 1 }));

            Assert.That(Array.Empty<V2d>().Simplify(0.0), Is.Empty);
            Assert.That(new[] { new V2d(1, 2) }.Simplify(0.0), Is.EqualTo(new[] { 0 }));
            Assert.That(new[] { new V2d(1, 2), new V2d(3, 4) }.Simplify(0.0), Is.EqualTo(new[] { 0, 1 }));
        }

        [Test]
        public void NullAndInvalidToleranceAreRejected()
        {
            var nullFloat = Assert.Throws<ArgumentNullException>(() => GeometryFun.Simplify((V2f[])null, 0.0f));
            var nullDouble = Assert.Throws<ArgumentNullException>(() => GeometryFun.Simplify((V2d[])null, 0.0));
            Assert.That(nullFloat.ParamName, Is.EqualTo("polyline"));
            Assert.That(nullDouble.ParamName, Is.EqualTo("polyline"));

            var pointsF = new[] { V2f.Zero };
            var pointsD = new[] { V2d.Zero };
            foreach (float epsilon in new[] { -1.0f, float.NegativeInfinity, float.NaN })
                Assert.That(Assert.Throws<ArgumentOutOfRangeException>(() => pointsF.Simplify(epsilon)).ParamName,
                    Is.EqualTo("epsilon"));
            foreach (double epsilon in new[] { -1.0, double.NegativeInfinity, double.NaN })
                Assert.That(Assert.Throws<ArgumentOutOfRangeException>(() => pointsD.Simplify(epsilon)).ParamName,
                    Is.EqualTo("epsilon"));
        }

        [Test]
        public void PositiveInfinityIsAllowedAndKeepsEndpoints()
        {
            var pointsF = new[] { new V2f(0, 0), new V2f(1, 100), new V2f(2, -100), new V2f(3, 0) };
            var pointsD = new[] { new V2d(0, 0), new V2d(1, 100), new V2d(2, -100), new V2d(3, 0) };

            Assert.That(pointsF.Simplify(float.PositiveInfinity), Is.EqualTo(new[] { 0, 3 }));
            Assert.That(pointsD.Simplify(double.PositiveInfinity), Is.EqualTo(new[] { 0, 3 }));
        }

        [Test]
        public void RepeatedAndCollinearPointsTerminateAtZeroTolerance()
        {
            var repeatedF = new V2f[128];
            var repeatedD = new V2d[128];
            Array.Fill(repeatedF, new V2f(2, 3));
            Array.Fill(repeatedD, new V2d(2, 3));
            Assert.That(repeatedF.Simplify(0.0f), Is.EqualTo(new[] { 0, 127 }));
            Assert.That(repeatedD.Simplify(0.0), Is.EqualTo(new[] { 0, 127 }));

            var collinearF = new V2f[128];
            var collinearD = new V2d[128];
            for (int i = 0; i < collinearF.Length; i++)
            {
                collinearF[i] = new V2f(i, 0);
                collinearD[i] = new V2d(i, 0);
            }
            Assert.That(collinearF.Simplify(0.0f), Is.EqualTo(new[] { 0, 127 }));
            Assert.That(collinearD.Simplify(0.0), Is.EqualTo(new[] { 0, 127 }));

            Assert.That(new[] { V2f.Zero, V2f.II, V2f.Zero }.Simplify(0.0f), Is.EqualTo(new[] { 0, 1, 2 }));
            Assert.That(new[] { V2d.Zero, V2d.II, V2d.Zero }.Simplify(0.0), Is.EqualTo(new[] { 0, 1, 2 }));
        }

        [Test]
        public void DistanceEqualToToleranceIsWithinTolerance()
        {
            var pointsF = new[] { new V2f(0, 0), new V2f(1, 1), new V2f(2, 0) };
            var pointsD = new[] { new V2d(0, 0), new V2d(1, 1), new V2d(2, 0) };

            Assert.That(pointsF.Simplify(1.0f), Is.EqualTo(new[] { 0, 2 }));
            Assert.That(pointsF.Simplify(MathF.BitDecrement(1.0f)), Is.EqualTo(new[] { 0, 1, 2 }));
            Assert.That(pointsD.Simplify(1.0), Is.EqualTo(new[] { 0, 2 }));
            Assert.That(pointsD.Simplify(Math.BitDecrement(1.0)), Is.EqualTo(new[] { 0, 1, 2 }));
        }

        [Test]
        public void FarthestTiesSelectFirstIndexAndOutputIsOrdered()
        {
            var pointsF = new[] { new V2f(0, 0), new V2f(1, 1), new V2f(2, 1), new V2f(3, 0) };
            var pointsD = new[] { new V2d(0, 0), new V2d(1, 1), new V2d(2, 1), new V2d(3, 0) };

            Assert.That(pointsF.Simplify(0.9f), Is.EqualTo(new[] { 0, 1, 3 }));
            Assert.That(pointsD.Simplify(0.9), Is.EqualTo(new[] { 0, 1, 3 }));

            var zigzagF = new[] { new V2f(0, 0), new V2f(1, 2), new V2f(2, -2), new V2f(3, 2), new V2f(4, 0) };
            var zigzagD = new[] { new V2d(0, 0), new V2d(1, 2), new V2d(2, -2), new V2d(3, 2), new V2d(4, 0) };
            Assert.That(zigzagF.Simplify(0.0f), Is.EqualTo(new[] { 0, 1, 2, 3, 4 }));
            Assert.That(zigzagD.Simplify(0.0), Is.EqualTo(new[] { 0, 1, 2, 3, 4 }));
        }

        [Test]
        public void RandomizedResultsMatchIndependentReference()
        {
            var random = new Random(1);
            double[] tolerances = { 0.0, 0.2, 0.75, 2.0, double.PositiveInfinity };

            for (int iteration = 0; iteration < 300; iteration++)
            {
                int count = random.Next(0, 33);
                var pointsD = new V2d[count];
                var pointsF = new V2f[count];
                for (int i = 0; i < count; i++)
                {
                    if (i > 0 && random.Next(8) == 0)
                        pointsD[i] = pointsD[i - 1];
                    else
                        pointsD[i] = new V2d(i, random.Next(-32, 33) * 0.25);
                    pointsF[i] = (V2f)pointsD[i];
                }

                double epsilon = tolerances[random.Next(tolerances.Length)];
                int[] expectedD = ReferenceSimplify(pointsD, epsilon);
                int[] expectedF = ReferenceSimplify(pointsF, (float)epsilon);
                Assert.That(pointsD.Simplify(epsilon), Is.EqualTo(expectedD), $"double iteration {iteration}");
                Assert.That(pointsF.Simplify((float)epsilon), Is.EqualTo(expectedF), $"float iteration {iteration}");
            }
        }

        [Test]
        public void EveryOutputSegmentSatisfiesErrorTolerance()
        {
            const double epsilon = 0.075;
            var pointsD = new V2d[4096];
            var pointsF = new V2f[4096];
            for (int i = 0; i < pointsD.Length; i++)
            {
                pointsD[i] = new V2d(i * 0.01, Math.Sin(i * 0.031) + 0.15 * Math.Sin(i * 0.113));
                pointsF[i] = (V2f)pointsD[i];
            }

            AssertSegmentError(pointsD, pointsD.Simplify(epsilon), epsilon);
            AssertSegmentError(pointsF, pointsF.Simplify((float)epsilon), (float)epsilon);
        }

        [Test]
        public void LargeCollinearInputUsesBoundedStack()
        {
            const int count = 200_000;
            var pointsF = new V2f[count];
            var pointsD = new V2d[count];
            for (int i = 0; i < count; i++)
            {
                pointsF[i] = new V2f((float)i / (count - 1), 0);
                pointsD[i] = new V2d((double)i / (count - 1), 0);
            }

            Assert.That(pointsF.Simplify(0.0f), Is.EqualTo(new[] { 0, count - 1 }));
            Assert.That(pointsD.Simplify(0.0), Is.EqualTo(new[] { 0, count - 1 }));
        }

        private static int[] ReferenceSimplify(V2f[] points, float epsilon)
        {
            if (points.Length == 0) return Array.Empty<int>();
            if (points.Length == 1) return new[] { 0 };

            var retained = new bool[points.Length];
            retained[0] = true;
            retained[points.Length - 1] = true;
            ReferenceSplit(points, epsilon, 0, points.Length - 1, retained);

            var result = new List<int>();
            for (int i = 0; i < retained.Length; i++)
                if (retained[i]) result.Add(i);
            return result.ToArray();
        }

        private static void ReferenceSplit(V2f[] points, float epsilon, int first, int last, bool[] retained)
        {
            float distanceMax = 0.0f;
            int indexMax = first;
            for (int i = first + 1; i < last; i++)
            {
                float distance = ReferenceDistanceToSegment(points[i], points[first], points[last]);
                if (distance > distanceMax)
                {
                    distanceMax = distance;
                    indexMax = i;
                }
            }

            if (distanceMax <= epsilon) return;
            retained[indexMax] = true;
            ReferenceSplit(points, epsilon, first, indexMax, retained);
            ReferenceSplit(points, epsilon, indexMax, last, retained);
        }

        private static float ReferenceDistanceToSegment(V2f point, V2f p0, V2f p1)
        {
            V2f direction = p0 - p1;
            float lengthSquared = direction.LengthSquared;
            if (lengthSquared == 0.0f) return (point - p0).Length;

            float t = (p0.Dot(direction) - point.Dot(direction)) / lengthSquared;
            if (t <= 0.0f) return (point - p0).Length;
            if (t >= 1.0f) return (point - p1).Length;
            return (point - (p0 - t * direction)).Length;
        }

        private static int[] ReferenceSimplify(V2d[] points, double epsilon)
        {
            if (points.Length == 0) return Array.Empty<int>();
            if (points.Length == 1) return new[] { 0 };

            var retained = new bool[points.Length];
            retained[0] = true;
            retained[points.Length - 1] = true;
            ReferenceSplit(points, epsilon, 0, points.Length - 1, retained);

            var result = new List<int>();
            for (int i = 0; i < retained.Length; i++)
                if (retained[i]) result.Add(i);
            return result.ToArray();
        }

        private static void ReferenceSplit(V2d[] points, double epsilon, int first, int last, bool[] retained)
        {
            double distanceMax = 0.0;
            int indexMax = first;
            for (int i = first + 1; i < last; i++)
            {
                double distance = ReferenceDistanceToSegment(points[i], points[first], points[last]);
                if (distance > distanceMax)
                {
                    distanceMax = distance;
                    indexMax = i;
                }
            }

            if (distanceMax <= epsilon) return;
            retained[indexMax] = true;
            ReferenceSplit(points, epsilon, first, indexMax, retained);
            ReferenceSplit(points, epsilon, indexMax, last, retained);
        }

        private static double ReferenceDistanceToSegment(V2d point, V2d p0, V2d p1)
        {
            V2d direction = p0 - p1;
            double lengthSquared = direction.LengthSquared;
            if (lengthSquared == 0.0) return (point - p0).Length;

            double t = (p0.Dot(direction) - point.Dot(direction)) / lengthSquared;
            if (t <= 0.0) return (point - p0).Length;
            if (t >= 1.0) return (point - p1).Length;
            return (point - (p0 - t * direction)).Length;
        }

        private static void AssertSegmentError(V2d[] points, int[] indices, double epsilon)
        {
            AssertOrderedEndpoints(points.Length, indices);
            for (int segment = 0; segment + 1 < indices.Length; segment++)
            {
                int first = indices[segment];
                int last = indices[segment + 1];
                var line = new Line2d(points[first], points[last]);
                for (int i = first + 1; i < last; i++)
                    Assert.That(line.GetDistanceToLine(points[i]), Is.LessThanOrEqualTo(epsilon + 1e-12));
            }
        }

        private static void AssertSegmentError(V2f[] points, int[] indices, float epsilon)
        {
            AssertOrderedEndpoints(points.Length, indices);
            for (int segment = 0; segment + 1 < indices.Length; segment++)
            {
                int first = indices[segment];
                int last = indices[segment + 1];
                var line = new Line2f(points[first], points[last]);
                for (int i = first + 1; i < last; i++)
                    Assert.That(line.GetDistanceToLine(points[i]), Is.LessThanOrEqualTo(epsilon + 1e-5f));
            }
        }

        private static void AssertOrderedEndpoints(int pointCount, int[] indices)
        {
            Assert.That(indices[0], Is.EqualTo(0));
            Assert.That(indices[indices.Length - 1], Is.EqualTo(pointCount - 1));
            for (int i = 1; i < indices.Length; i++)
                Assert.That(indices[i], Is.GreaterThan(indices[i - 1]));
        }
    }
}

using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class PolygonCentroidTests
    {
        // Union of a 5 x 1 rectangle and a 2 x 3 rectangle. This gives an
        // independent area-decomposition reference for the asymmetric L shape.
        private static readonly V2d[] s_concave =
        {
            new V2d(0, 0),
            new V2d(5, 0),
            new V2d(5, 1),
            new V2d(2, 1),
            new V2d(2, 4),
            new V2d(0, 4),
        };

        private static readonly V2d s_concaveCentroid = new V2d(18.5 / 11.0, 17.5 / 11.0);
        private static readonly V3d s_origin = new V3d(3, -4, 2);
        private static readonly V3d s_axisU = new V3d(2, 1, -1);
        private static readonly V3d s_axisV = new V3d(-1, 3, 2);

        [Test]
        public void Polygon2dCentroidIsIndependentOfWindingAndCyclicShift()
        {
            ForEveryCyclicShiftAndWinding(s_concave, points =>
            {
                var actual = new Polygon2d(points).ComputeCentroid();
                AssertVector(actual, s_concaveCentroid, 2e-15);
            });
        }

        [Test]
        public void Polygon2fCentroidIsIndependentOfWindingAndCyclicShift()
        {
            var expected = (V2f)s_concaveCentroid;
            var source = Array.ConvertAll(s_concave, p => (V2f)p);
            ForEveryCyclicShiftAndWinding(source, points =>
            {
                var actual = new Polygon2f(points).ComputeCentroid();
                AssertVector(actual, expected, 2e-6f);
            });
        }

        [Test]
        public void Polygon3dCentroidIsIndependentOfWindingAndCyclicShiftOnObliquePlane()
        {
            var source = Array.ConvertAll(s_concave, ToOblique);
            var expected = ToOblique(s_concaveCentroid);
            ForEveryCyclicShiftAndWinding(source, points =>
            {
                var actual = new Polygon3d(points).ComputeCentroid();
                AssertVector(actual, expected, 4e-15);
            });
        }

        [Test]
        public void Polygon3fCentroidIsIndependentOfWindingAndCyclicShiftOnObliquePlane()
        {
            var source = Array.ConvertAll(s_concave, p => (V3f)ToOblique(p));
            var expected = (V3f)ToOblique(s_concaveCentroid);
            ForEveryCyclicShiftAndWinding(source, points =>
            {
                var actual = new Polygon3f(points).ComputeCentroid();
                AssertVector(actual, expected, 4e-6f);
            });
        }

        [Test]
        public void CentroidsAcceptLeadingCollinearTriangles()
        {
            var points = new[]
            {
                new V2d(0, 0),
                new V2d(1, 0),
                new V2d(2, 0),
                new V2d(2, 2),
                new V2d(0, 2),
            };

            AssertVector(new Polygon2d(points).ComputeCentroid(), new V2d(1, 1), 1e-15);
            AssertVector(new Polygon2f(Array.ConvertAll(points, p => (V2f)p)).ComputeCentroid(), new V2f(1, 1), 1e-6f);

            var points3d = Array.ConvertAll(points, ToOblique);
            var expected3d = ToOblique(new V2d(1, 1));
            AssertVector(new Polygon3d(points3d).ComputeCentroid(), expected3d, 2e-15);
            AssertVector(new Polygon3f(Array.ConvertAll(points3d, p => (V3f)p)).ComputeCentroid(), (V3f)expected3d, 2e-6f);
        }

        [Test]
        public void Polygon2CentroidsRemainStableUnderLargeRepresentableTranslations()
        {
            var doubleOffset = new V2d(1e12, -2e12);
            var doublePoints = Array.ConvertAll(s_concave, p => p + doubleOffset);
            AssertVector(new Polygon2d(doublePoints).ComputeCentroid(), doubleOffset + s_concaveCentroid, 3e-4);

            const float scale = 64.0f;
            var floatOffset = new V2f(1048576.0f, -2097152.0f);
            var floatPoints = Array.ConvertAll(s_concave, p => floatOffset + (V2f)p * scale);
            var expected = floatOffset + (V2f)s_concaveCentroid * scale;
            AssertVector(new Polygon2f(floatPoints).ComputeCentroid(), expected, 0.15f);
        }

        [Test]
        public void MediumPolygon2CentroidsPreserveTranslationAndOrderingInvariants()
        {
            var outline = new[]
            {
                new V2d(0, 0), new V2d(0.5, 0), new V2d(1, 0), new V2d(2, 0), new V2d(3, 0),
                new V2d(4, 0), new V2d(5, 0), new V2d(5, 1), new V2d(4, 1), new V2d(3, 1),
                new V2d(2, 1), new V2d(2, 2), new V2d(2, 3), new V2d(2, 4), new V2d(1, 4),
                new V2d(0, 4), new V2d(0, 3), new V2d(0, 2), new V2d(0, 1),
            };
            var offsetD = new V2d(1e12, -2e12);
            var pointsD = Array.ConvertAll(outline, p => offsetD + p);
            var expectedD = offsetD + s_concaveCentroid;
            ForEveryCyclicShiftAndWinding(pointsD, points =>
                AssertVector(new Polygon2d(points).ComputeCentroid(), expectedD, 3e-4));

            const float scale = 64.0f;
            var offsetF = new V2f(1048576.0f, -2097152.0f);
            var pointsF = Array.ConvertAll(outline, p => offsetF + (V2f)p * scale);
            var expectedF = offsetF + (V2f)s_concaveCentroid * scale;
            ForEveryCyclicShiftAndWinding(pointsF, points =>
                AssertVector(new Polygon2f(points).ComputeCentroid(), expectedF, 0.15f));
        }

        [Test]
        public void Polygon3CentroidsRemainStableUnderLargeRepresentableTranslations()
        {
            var doubleOffset = new V3d(1e12, -2e12, 3e12);
            var doublePoints = Array.ConvertAll(s_concave, p => doubleOffset + ToOblique(p));
            AssertVector(new Polygon3d(doublePoints).ComputeCentroid(), doubleOffset + ToOblique(s_concaveCentroid), 8e-4);

            const float scale = 64.0f;
            var floatOffset = new V3f(1048576.0f, -2097152.0f, 3145728.0f);
            var floatPoints = Array.ConvertAll(s_concave, p => floatOffset + (V3f)ToOblique(p) * scale);
            var expected = floatOffset + (V3f)ToOblique(s_concaveCentroid) * scale;
            AssertVector(new Polygon3f(floatPoints).ComputeCentroid(), expected, 0.35f);
        }

        [Test]
        public void FewerThanThreeAndDegeneratePolygonsHaveZeroCentroid()
        {
            Assert.That(default(Polygon2d).ComputeCentroid(), Is.EqualTo(V2d.Zero));
            Assert.That(new Polygon2d(new V2d(1, 2)).ComputeCentroid(), Is.EqualTo(V2d.Zero));
            Assert.That(new Polygon2d(new V2d(1, 2), new V2d(3, 4)).ComputeCentroid(), Is.EqualTo(V2d.Zero));
            Assert.That(new Polygon2d(new V2d(0, 0), new V2d(1, 1), new V2d(2, 2), new V2d(3, 3)).ComputeCentroid(), Is.EqualTo(V2d.Zero));

            Assert.That(default(Polygon2f).ComputeCentroid(), Is.EqualTo(V2f.Zero));
            Assert.That(new Polygon2f(new V2f(1, 2)).ComputeCentroid(), Is.EqualTo(V2f.Zero));
            Assert.That(new Polygon2f(new V2f(1, 2), new V2f(3, 4)).ComputeCentroid(), Is.EqualTo(V2f.Zero));
            Assert.That(new Polygon2f(new V2f(0, 0), new V2f(1, 1), new V2f(2, 2)).ComputeCentroid(), Is.EqualTo(V2f.Zero));

            Assert.That(default(Polygon3d).ComputeCentroid(), Is.EqualTo(V3d.Zero));
            Assert.That(new Polygon3d(new V3d(1, 2, 3)).ComputeCentroid(), Is.EqualTo(V3d.Zero));
            Assert.That(new Polygon3d(new V3d(1, 2, 3), new V3d(4, 5, 6)).ComputeCentroid(), Is.EqualTo(V3d.Zero));
            Assert.That(new Polygon3d(new V3d(0, 0, 0), new V3d(1, 1, 1), new V3d(2, 2, 2), new V3d(3, 3, 3)).ComputeCentroid(), Is.EqualTo(V3d.Zero));

            Assert.That(default(Polygon3f).ComputeCentroid(), Is.EqualTo(V3f.Zero));
            Assert.That(new Polygon3f(new V3f(1, 2, 3)).ComputeCentroid(), Is.EqualTo(V3f.Zero));
            Assert.That(new Polygon3f(new V3f(1, 2, 3), new V3f(4, 5, 6)).ComputeCentroid(), Is.EqualTo(V3f.Zero));
            Assert.That(new Polygon3f(new V3f(0, 0, 0), new V3f(1, 1, 1), new V3f(2, 2, 2)).ComputeCentroid(), Is.EqualTo(V3f.Zero));
        }

        private static V3d ToOblique(V2d point)
            => s_origin + point.X * s_axisU + point.Y * s_axisV;

        private static void ForEveryCyclicShiftAndWinding<T>(T[] source, Action<T[]> test)
        {
            for (int reversed = 0; reversed < 2; reversed++)
            {
                for (int shift = 0; shift < source.Length; shift++)
                {
                    var points = new T[source.Length];
                    for (int i = 0; i < points.Length; i++)
                    {
                        var index = reversed == 0
                            ? (shift + i) % source.Length
                            : (shift - i + source.Length) % source.Length;
                        points[i] = source[index];
                    }
                    test(points);
                }
            }
        }

        private static void AssertVector(V2d actual, V2d expected, double tolerance)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(tolerance));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(tolerance));
        }

        private static void AssertVector(V2f actual, V2f expected, float tolerance)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(tolerance));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(tolerance));
        }

        private static void AssertVector(V3d actual, V3d expected, double tolerance)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(tolerance));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(tolerance));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(tolerance));
        }

        private static void AssertVector(V3f actual, V3f expected, float tolerance)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(tolerance));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(tolerance));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(tolerance));
        }
    }
}

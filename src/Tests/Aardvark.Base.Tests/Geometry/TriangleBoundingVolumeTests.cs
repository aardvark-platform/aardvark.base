using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class TriangleBoundingVolumeTests
    {
        private static readonly int[][] Permutations =
        {
            new[] { 0, 1, 2 }, new[] { 0, 2, 1 }, new[] { 1, 0, 2 },
            new[] { 1, 2, 0 }, new[] { 2, 0, 1 }, new[] { 2, 1, 0 }
        };

        [TestCase(false, false), TestCase(false, true), TestCase(true, false), TestCase(true, true)]
        public void AnalyticBoundsAcrossScalesAndPermutations(bool three, bool single)
        {
            var shapes = new (V3d A, V3d B, V3d C, V3d Center, double Radius)[]
            {
                (V3d.Zero, V3d.XAxis, V3d.YAxis, new V3d(0.5, 0.5, 0), Math.Sqrt(0.5)),
                (V3d.Zero, new V3d(0.01, 0, 0), new V3d(0, 0.01, 0), new V3d(0.005, 0.005, 0), 0.01 * Math.Sqrt(0.5)),
                (V3d.Zero, new V3d(0.001, 0, 0), new V3d(0, 0.001, 0), new V3d(0.0005, 0.0005, 0), 0.001 * Math.Sqrt(0.5)),
                (V3d.Zero, V3d.XAxis, new V3d(0.5, 1e-8, 0), new V3d(0.5, 0, 0), 0.5),
                (V3d.Zero, new V3d(2, 0, 0), new V3d(1, 2, 0), new V3d(1, 0.75, 0), 1.25),
                (V3d.Zero, new V3d(2, 0, 0), new V3d(0.5, 0.25, 0), V3d.XAxis, 1),
                (new V3d(-2, -2, 0), new V3d(1, 1, 0), new V3d(4, 4, 0), new V3d(1, 1, 0), 3 * Math.Sqrt(2)),
                (V3d.Zero, V3d.Zero, V3d.XAxis, new V3d(0.5, 0, 0), 0.5),
                (V3d.One, V3d.One, V3d.One, V3d.One, 0)
            };
            int[] powers = single ? new[] { -130, -50, -10, 0, 10, 50, 120 } : new[] { -1040, -500, -40, 0, 40, 500, 1000 };
            foreach (var shape in shapes)
                foreach (int power in powers)
                    foreach (bool translated in new[] { false, true })
                        foreach (bool rotated in three ? new[] { false, true } : new[] { false })
                        {
                            double scale = Math.Pow(2, power);
                            var shift = translated ? new V3d(4, -8, three ? 16 : 0) : V3d.Zero;
                            V3d Map(V3d p)
                            {
                                if (!three) p.Z = 0;
                                if (rotated) p = p.X * new V3d(0.6, 0.8, 0) + p.Y * new V3d(-0.48, 0.36, 0.8) + p.Z * new V3d(0.64, -0.48, 0.6);
                                return (p + shift) * scale;
                            }
                            var points = new[] { Map(shape.A), Map(shape.B), Map(shape.C) };
                            foreach (var order in Permutations)
                                Check(points[order[0]], points[order[1]], points[order[2]], Map(shape.Center), shape.Radius * scale,
                                    scale, three, single);
                        }
        }

        [TestCase(false, false), TestCase(false, true), TestCase(true, false), TestCase(true, true)]
        public void ThinAcuteTrianglesUseStableLocalCenters(bool three, bool single)
        {
            double h = single ? 1e-4 : 1e-8;
            double delta = 0.5 * h * h;
            double y = (delta * delta + 0.5 * h * h) / (2 * h);
            var p = new[] { V3d.Zero, V3d.XAxis, new V3d(delta, h, 0) };
            foreach (var order in Permutations)
                Check(p[order[0]], p[order[1]], p[order[2]], new V3d(0.5, y, 0), Math.Sqrt(0.25 + y * y), 1, three, single);
        }

        [TestCase(false, false), TestCase(false, true), TestCase(true, false), TestCase(true, true)]
        public void OverflowingDifferencesStillHaveRepresentableBounds(bool three, bool single)
        {
            double max = single ? float.MaxValue : double.MaxValue;
            var points = new[] { new V3d(-max, 0, 0), new V3d(max, 0, 0), new V3d(0, 0.5 * max, 0) };
            foreach (var order in Permutations)
                Check(points[order[0]], points[order[1]], points[order[2]], V3d.Zero, max, max, three, single);
        }

        [TestCase(false, false), TestCase(false, true), TestCase(true, false), TestCase(true, true)]
        public void LargeTranslationsRetainLocalExtents(bool three, bool single)
        {
            double origin = Math.Pow(2, single ? 100 : 900);
            double step = Math.Pow(2, single ? 77 : 848);
            var a = new V3d(origin, origin, three ? origin : 0);
            var b = a + new V3d(2 * step, 0, 0);
            var c = a + new V3d(step, 0, 0);
            foreach (var order in Permutations)
            {
                var p = new[] { a, b, c };
                Check(p[order[0]], p[order[1]], p[order[2]], c, step, step, three, single);
            }

            // When the ideal midpoint is not representable, the rounded center must still enclose both ends.
            var bound = Get(a, a + new V3d(step, 0, 0), a, three, single);
            Assert.That(bound.Valid && bound.Center.IsFinite && double.IsFinite(bound.Radius), Is.True);
            Contains(bound, a, step, 0);
            Contains(bound, a + new V3d(step, 0, 0), step, 0);
        }

        [TestCase(false, false), TestCase(false, true), TestCase(true, false), TestCase(true, true)]
        public void RoundedTranslatedCentersEncloseEveryPermutation(bool three, bool single)
        {
            double origin = Math.Pow(2, single ? 23 : 52);
            var shapes = new[]
            {
                new[] { V3d.Zero, V3d.Zero, V3d.XAxis },
                new[] { V3d.Zero, V3d.YAxis, new V3d(3, 1, 0) },
                new[] { V3d.Zero, new V3d(4, 0, 0), new V3d(2, 3, 0) }
            };
            var shift = new V3d(origin, origin, three ? origin : 0);
            foreach (var shape in shapes)
                foreach (var order in Permutations)
                {
                    var a = shift + shape[order[0]];
                    var b = shift + shape[order[1]];
                    var c = shift + shape[order[2]];
                    var bound = Get(a, b, c, three, single);
                    Assert.That(bound.Valid && bound.Center.IsFinite && double.IsFinite(bound.Radius), Is.True);
                    ContainsAtTargetPrecision(bound, a, three, single);
                    ContainsAtTargetPrecision(bound, b, three, single);
                    ContainsAtTargetPrecision(bound, c, three, single);
                }
        }

        [TestCase(false, false), TestCase(false, true), TestCase(true, false), TestCase(true, true)]
        public void NonFiniteInputsReturnInvalid(bool three, bool single)
        {
            foreach (double bad in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity })
                for (int vertex = 0; vertex < 3; vertex++)
                    for (int axis = 0; axis < (three ? 3 : 2); axis++)
                    {
                        var p = new[] { V3d.Zero, V3d.XAxis, V3d.YAxis };
                        p[vertex][axis] = bad;
                        var bound = Get(p[0], p[1], p[2], three, single);
                        Assert.That(bound.Valid, Is.False);
                        Assert.That(bound.Radius, Is.EqualTo(-1));
                    }
        }

        [TestCase(false, false), TestCase(false, true), TestCase(true, false), TestCase(true, true)]
        public void SeededBoundsMatchIndependentReference(bool three, bool single)
        {
            var random = new Random(7013);
            V3d Next() => new V3d(random.Next(-8, 9), random.Next(-8, 9), three ? random.Next(-8, 9) : 0);
            for (int i = 0; i < 5000; i++)
            {
                var a = Next(); var b = Next(); var c = Next();
                var reference = Reference(a, b, c, three);
                Check(a, b, c, reference.Center, reference.Radius, 16, three, single);
            }
        }

        private static (V3d Center, double Radius, bool Valid) Get(V3d a, V3d b, V3d c, bool three, bool single)
        {
            if (three)
            {
                if (single)
                {
                    var bound = new Triangle3f((V3f)a, (V3f)b, (V3f)c).BoundingSphere3f;
                    return ((V3d)bound.Center, bound.Radius, bound.IsValid);
                }
                else
                {
                    var bound = new Triangle3d(a, b, c).BoundingSphere3d;
                    return (bound.Center, bound.Radius, bound.IsValid);
                }
            }
            else
            {
                if (single)
                {
                    var bound = new Triangle2f(new V2f((float)a.X, (float)a.Y), new V2f((float)b.X, (float)b.Y), new V2f((float)c.X, (float)c.Y)).BoundingCircle2f;
                    return (new V3d(bound.Center.X, bound.Center.Y, 0), bound.Radius, bound.IsValid);
                }
                else
                {
                    var bound = new Triangle2d(a.XY, b.XY, c.XY).BoundingCircle2d;
                    return (new V3d(bound.Center.X, bound.Center.Y, 0), bound.Radius, bound.IsValid);
                }
            }
        }

        private static void Check(V3d a, V3d b, V3d c, V3d center, double radius, double scale, bool three, bool single)
        {
            var result = Get(a, b, c, three, single);
            string context = $"a={a}, b={b}, c={c}, single={single}, three={three}";
            double tolerance = single ? 4e-5 + 8 * ((double)float.Epsilon / scale) : 4e-13 + 8 * (double.Epsilon / scale);
            Assert.That(result.Valid && result.Center.IsFinite && double.IsFinite(result.Radius), Is.True, context);
            Assert.That((result.Center / scale - center / scale).NormMax, Is.LessThanOrEqualTo(tolerance), context);
            Assert.That(Math.Abs(result.Radius / scale - radius / scale), Is.LessThanOrEqualTo(tolerance), context);
            if (single) { a = (V3d)(V3f)a; b = (V3d)(V3f)b; c = (V3d)(V3f)c; }
            Contains(result, a, scale, tolerance);
            Contains(result, b, scale, tolerance);
            Contains(result, c, scale, tolerance);
            ContainsAtTargetPrecision(result, a, three, single);
            ContainsAtTargetPrecision(result, b, three, single);
            ContainsAtTargetPrecision(result, c, three, single);
        }

        private static void Contains((V3d Center, double Radius, bool Valid) bound, V3d point, double scale, double tolerance)
        {
            var delta = point / scale - bound.Center / scale;
            Assert.That(Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y + delta.Z * delta.Z),
                Is.LessThanOrEqualTo(bound.Radius / scale + tolerance));
        }

        private static void ContainsAtTargetPrecision((V3d Center, double Radius, bool Valid) bound, V3d point, bool three, bool single)
        {
            double distance;
            if (single)
            {
                var delta = (V3f)point - (V3f)bound.Center;
                distance = three ? delta.Length : delta.XY.Length;
            }
            else
            {
                var delta = point - bound.Center;
                distance = three ? delta.Length : delta.XY.Length;
            }
            if (double.IsFinite(distance)) Assert.That(distance, Is.LessThanOrEqualTo(bound.Radius));
        }

        // Bounded integer inputs only: enumerate diameter candidates, then solve the absolute
        // equidistance/plane equations by independent Gaussian elimination for a circum-bound.
        private static (V3d Center, double Radius) Reference(V3d a, V3d b, V3d c, bool three)
        {
            var points = new[] { a, b, c };
            V3d best = V3d.NaN;
            double bestSquared = double.PositiveInfinity;
            void Candidate(V3d center)
            {
                double r2 = (center - a).LengthSquared;
                double rb = (center - b).LengthSquared;
                double rc = (center - c).LengthSquared;
                // A candidate radius may be determined by any two endpoints.
                double radius = Math.Max(r2, Math.Max(rb, rc));
                if (radius < bestSquared) { best = center; bestSquared = radius; }
            }
            for (int i = 0; i < 3; i++)
                for (int j = i + 1; j < 3; j++)
                {
                    var center = (points[i] + points[j]) * 0.5;
                    double r2 = (points[i] - center).LengthSquared;
                    bool contains = true;
                    foreach (var p in points) contains &= (p - center).LengthSquared <= r2 + 1e-12;
                    if (contains) Candidate(center);
                }
            int n = three ? 3 : 2;
            var matrix = new double[n, n + 1];
            var u = b - a; var v = c - a;
            for (int j = 0; j < n; j++) { matrix[0, j] = 2 * u[j]; matrix[1, j] = 2 * v[j]; }
            matrix[0, n] = b.LengthSquared - a.LengthSquared;
            matrix[1, n] = c.LengthSquared - a.LengthSquared;
            if (three)
            {
                var normal = new V3d(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X);
                for (int j = 0; j < 3; j++) matrix[2, j] = normal[j];
                matrix[2, 3] = normal.X * a.X + normal.Y * a.Y + normal.Z * a.Z;
            }
            bool nonsingular = true;
            for (int k = 0; k < n; k++)
            {
                int pivot = k;
                for (int i = k + 1; i < n; i++) if (Math.Abs(matrix[i, k]) > Math.Abs(matrix[pivot, k])) pivot = i;
                if (matrix[pivot, k] == 0) { nonsingular = false; break; }
                for (int j = k; j <= n; j++) (matrix[k, j], matrix[pivot, j]) = (matrix[pivot, j], matrix[k, j]);
                double divisor = matrix[k, k];
                for (int j = k; j <= n; j++) matrix[k, j] /= divisor;
                for (int i = 0; i < n; i++)
                    if (i != k)
                    {
                        double factor = matrix[i, k];
                        for (int j = k; j <= n; j++) matrix[i, j] -= factor * matrix[k, j];
                    }
            }
            if (nonsingular) Candidate(new V3d(matrix[0, n], matrix[1, n], three ? matrix[2, n] : 0));
            return (best, Math.Sqrt(bestSquared));
        }
    }
}

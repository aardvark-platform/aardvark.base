using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RotateIntoTests
    {
        private const double DoubleTolerance = 8e-15;
        private const double FloatTolerance = 2e-6;
        private static double s_sink;

        private static V3d Unit(V3d v)
        {
            double length = Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            return new V3d(v.X / length, v.Y / length, v.Z / length);
        }

        private static readonly V3d[] Directions =
        {
            V3d.XAxis, V3d.YAxis, V3d.ZAxis,
            Unit(new V3d(1, 2, 3)), Unit(new V3d(-3, 5, -2)), Unit(new V3d(1, 1, 1))
        };

        private static V3d Tangent(V3d v)
        {
            var basis = Math.Abs(v.X) < 0.8 ? V3d.XAxis : V3d.YAxis;
            return Unit(basis - v * (basis.X * v.X + basis.Y * v.Y + basis.Z * v.Z));
        }

        private static V3d RandomDirection(Random random)
        {
            V3d v;
            do v = new V3d(random.NextDouble() - 0.5, random.NextDouble() - 0.5, random.NextDouble() - 0.5);
            while (v.LengthSquared < 0.01);
            return Unit(v);
        }

        private static void Close(V3d actual, V3d expected, double tolerance)
        {
            // Cartesian target residual, independent of RotateInto or AngleBetween formulas.
            double error = Math.Max(Math.Abs(actual.X - expected.X), Math.Max(Math.Abs(actual.Y - expected.Y), Math.Abs(actual.Z - expected.Z)));
            Assert.That(error, Is.LessThanOrEqualTo(tolerance), $"{actual} != {expected}");
        }

        private static Rot3d Check(V3d from, V3d into)
        {
            var q = Rot3d.RotateInto(from, into);
            Assert.That(double.IsFinite(q.W) && double.IsFinite(q.X) && double.IsFinite(q.Y) && double.IsFinite(q.Z), Is.True);
            Assert.That(Math.Abs(q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z - 1), Is.LessThanOrEqualTo(DoubleTolerance));
            Close(q * from, into, DoubleTolerance);
            Close(q.Inverse * into, from, DoubleTolerance);
            var probe = new V3d(0.25, -0.5, 0.75);
            Close(q.Inverse * (q * probe), probe, 2 * DoubleTolerance);
            return q;
        }

        private static Rot3f Check(V3f from, V3f into)
        {
            var q = Rot3f.RotateInto(from, into);
            Assert.That(float.IsFinite(q.W) && float.IsFinite(q.X) && float.IsFinite(q.Y) && float.IsFinite(q.Z), Is.True);
            double norm = (double)q.W * q.W + (double)q.X * q.X + (double)q.Y * q.Y + (double)q.Z * q.Z;
            Assert.That(Math.Abs(norm - 1), Is.LessThanOrEqualTo(FloatTolerance));
            Close(new V3d(q * from), new V3d(into), FloatTolerance);
            Close(new V3d(q.Inverse * into), new V3d(from), FloatTolerance);
            var probe = new V3f(0.25f, -0.5f, 0.75f);
            Close(new V3d(q.Inverse * (q * probe)), new V3d(probe), 2 * FloatTolerance);
            return q;
        }

        [Test]
        public void DoubleRequiredNearOppositeExample()
        {
            var q = Check(V3d.XAxis, Unit(new V3d(-1, 1e-8, 0)));
            Assert.That(q.W, Is.EqualTo(Math.Sin(Math.Atan2(1e-8, 1) / 2)).Within(1e-22));
        }

        [Test]
        public void FloatRequiredNearOppositeExample()
        {
            var q = Check(V3f.XAxis, new V3f(-1, 0.001f, 0).Normalized);
            Assert.That(q.W, Is.EqualTo(Math.Sin(Math.Atan2(0.001f, 1) / 2)).Within(2e-10));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void LogarithmicDeviationsAndArbitraryOrientations(bool single)
        {
            foreach (var from in Directions)
            for (int exponent = 0; exponent <= (single ? 8 : 16); exponent++)
            foreach (int sign in new[] { -1, 1 })
            {
                double deviation = sign * Math.Pow(10, -exponent);
                var into = Unit(-from + deviation * Tangent(from));
                if (single) Check(new V3f(from).Normalized, new V3f(into).Normalized);
                else Check(from, into);
            }
        }

        [TestCase(1e-100)]
        [TestCase(1e-150)]
        [TestCase(1e-200)]
        [TestCase(1e-300)]
        public void DoubleSmallCrossDoesNotUnderflow(double deviation)
        {
            var q = Check(V3d.XAxis, new V3d(-1, deviation, 0));
            Assert.That(q.W, Is.GreaterThan(0));
            // The scalar remains representable even when the squared cross magnitude does not.
            Assert.That(q.W / (deviation / 2), Is.EqualTo(1).Within(2e-15));
            Assert.That(q.Z, Is.EqualTo(1).Within(2e-15));
        }

        [TestCase(1e-15f)]
        [TestCase(1e-20f)]
        [TestCase(1e-30f)]
        [TestCase(1e-38f)]
        public void FloatSmallCrossDoesNotUnderflow(float deviation)
        {
            var q = Check(V3f.XAxis, new V3f(-1, deviation, 0));
            Assert.That(q.W, Is.GreaterThan(0));
            Assert.That((double)q.W / ((double)deviation / 2), Is.EqualTo(1).Within(1e-6));
            Assert.That(q.Z, Is.EqualTo(1).Within(1e-6));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void IdentityAndDeterministicExactOpposition(bool single)
        {
            foreach (var direction in Directions)
            {
                if (single)
                {
                    var from = new V3f(direction).Normalized;
                    var identity = Check(from, from);
                    Assert.That(((M33f)identity).IsIdentity(0), Is.True);
                    var opposite = Check(from, -from);
                    Assert.That(opposite, Is.EqualTo(new Rot3f(0, from.AxisAlignedNormal())));
                }
                else
                {
                    var identity = Check(direction, direction);
                    Assert.That(((M33d)identity).IsIdentity(0), Is.True);
                    var opposite = Check(direction, -direction);
                    Assert.That(opposite, Is.EqualTo(new Rot3d(0, direction.AxisAlignedNormal())));
                }
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void NearBranchBoundaryIsContinuous(bool single)
        {
            double step = single ? 1e-5 : 1e-11;
            var left = new double[4];
            foreach (int sign in new[] { -1, 1 })
            {
                double cosine = -0.9 + sign * step;
                var into = Unit(new V3d(cosine, Math.Sqrt(1 - cosine * cosine), 0));
                var q = single ? new Rot3d(Check(V3f.XAxis, new V3f(into).Normalized)) : Check(V3d.XAxis, into);
                double angle = Math.Atan2(into.Y, into.X);
                double tolerance = single ? FloatTolerance : DoubleTolerance;
                Assert.That(q.W, Is.EqualTo(Math.Cos(angle / 2)).Within(tolerance));
                Assert.That(q.Z, Is.EqualTo(Math.Sin(angle / 2)).Within(tolerance));
                if (sign < 0) { left[0] = q.W; left[1] = q.X; left[2] = q.Y; left[3] = q.Z; }
                else
                {
                    Assert.That(Math.Abs(q.W - left[0]), Is.LessThan(4 * step));
                    Assert.That(Math.Abs(q.X - left[1]) + Math.Abs(q.Y - left[2]) + Math.Abs(q.Z - left[3]), Is.LessThan(4 * step));
                }
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void SeededDirectionMappings(bool single)
        {
            var random = new Random(892317);
            for (int i = 0; i < 20000; i++)
            {
                var from = RandomDirection(random);
                double deviation = Math.Pow(10, -random.NextDouble() * (single ? 8 : 15));
                var into = i % 3 == 0 ? RandomDirection(random) : Unit(-from + Tangent(from) * deviation);
                if (single) Check(new V3f(from).Normalized, new V3f(into).Normalized);
                else Check(from, into);
            }
        }

        [Test]
        public void DoubleTransformationWrappersInheritMapping()
        {
            foreach (var from in Directions)
            {
                var into = Unit(-from + Tangent(from) * 1e-8);
                Check(from, into);
                Close(M33d.RotateInto(from, into) * from, into, DoubleTolerance);
                Close(M34d.RotateInto(from, into).TransformDir(from), into, DoubleTolerance);
                Close(M44d.RotateInto(from, into).TransformDir(from), into, DoubleTolerance);
                Close(Euclidean3d.RotateInto(from, into).TransformPos(from), into, DoubleTolerance);
                Close(Similarity3d.RotateInto(from, into).TransformPos(from), into, DoubleTolerance);
                Close(Affine3d.RotateInto(from, into).TransformPos(from), into, DoubleTolerance);
                var trafo = Trafo3d.RotateInto(from, into);
                Close(trafo.Forward.TransformDir(from), into, DoubleTolerance);
                Close(trafo.Backward.TransformDir(into), from, DoubleTolerance);
            }
        }

        [Test]
        public void FloatTransformationWrappersInheritMapping()
        {
            foreach (var direction in Directions)
            {
                var from = new V3f(direction).Normalized;
                var into = new V3f(Unit(-direction + Tangent(direction) * 0.001)).Normalized;
                Check(from, into);
                Close(new V3d(M33f.RotateInto(from, into) * from), new V3d(into), FloatTolerance);
                Close(new V3d(M34f.RotateInto(from, into).TransformDir(from)), new V3d(into), FloatTolerance);
                Close(new V3d(M44f.RotateInto(from, into).TransformDir(from)), new V3d(into), FloatTolerance);
                Close(new V3d(Euclidean3f.RotateInto(from, into).TransformPos(from)), new V3d(into), FloatTolerance);
                Close(new V3d(Similarity3f.RotateInto(from, into).TransformPos(from)), new V3d(into), FloatTolerance);
                Close(new V3d(Affine3f.RotateInto(from, into).TransformPos(from)), new V3d(into), FloatTolerance);
                var trafo = Trafo3f.RotateInto(from, into);
                Close(new V3d(trafo.Forward.TransformDir(from)), new V3d(into), FloatTolerance);
                Close(new V3d(trafo.Backward.TransformDir(into)), new V3d(from), FloatTolerance);
            }
        }

        [Test]
        public void WarmedQueriesAllocateNothing()
        {
            var a = new[] { V3d.XAxis, Unit(new V3d(1, 2, 3)), V3d.YAxis, V3d.XAxis };
            var b = new[] { V3d.XAxis, -a[1], V3d.ZAxis, new V3d(-1, 1e-30, 0) };
            var af = Array.ConvertAll(a, v => new V3f(v).Normalized);
            var bf = Array.ConvertAll(b, v => new V3f(v).Normalized);
            double Run()
            {
                double sum = 0;
                for (int i = 0; i < 4096; i++)
                {
                    var d = Rot3d.RotateInto(a[i % 4], b[i % 4]);
                    var f = Rot3f.RotateInto(af[i % 4], bf[i % 4]);
                    sum += d.W + d.X + d.Y + d.Z + f.W + f.X + f.Y + f.Z;
                }
                return sum;
            }
            for (int i = 0; i < 8; i++) s_sink = Run();
            long before = GC.GetAllocatedBytesForCurrentThread();
            s_sink = Run();
            Assert.That(GC.GetAllocatedBytesForCurrentThread() - before, Is.Zero);
            Assert.That(double.IsFinite(s_sink), Is.True);
        }
    }
}

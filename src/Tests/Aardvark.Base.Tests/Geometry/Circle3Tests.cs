using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Circle3Tests
    {
        [Test]
        public void Circle3f_PointIsRepresentativeCircumferencePoint()
        {
            var circle = new Circle3f(
                new V3f(1.25f, -2.5f, 4.0f),
                new V3f(1.0f, 2.0f, 3.0f).Normalized,
                3.5f);

            Assert.That((circle.Point - circle.Center).Length, Is.EqualTo(circle.Radius).Within(1e-5f));
            Assert.That(circle.Point, Is.EqualTo(circle.Center + circle.AxisU));
            Assert.That(circle.Point, Is.EqualTo(circle.GetPoint(0.0f)));
        }

        [Test]
        public void Circle3d_PointIsRepresentativeCircumferencePoint()
        {
            var circle = new Circle3d(
                new V3d(1.25, -2.5, 4.0),
                new V3d(1.0, 2.0, 3.0).Normalized,
                3.5);

            Assert.That((circle.Point - circle.Center).Length, Is.EqualTo(circle.Radius).Within(1e-12));
            Assert.That(circle.Point, Is.EqualTo(circle.Center + circle.AxisU));
            Assert.That(circle.Point, Is.EqualTo(circle.GetPoint(0.0)));
        }

        [Test]
        public void Circle3f_AxesFormOrientedRadiusLengthFrame()
        {
            var normal = new V3f(1.0f, 2.0f, 3.0f).Normalized;
            var circle = new Circle3f(V3f.Zero, normal, 7.0f);
            var reference = Math.Abs(normal.X) > 0.9f ? V3f.YAxis : V3f.XAxis;
            var expectedU = normal.Cross(reference).Normalized * circle.Radius;
            var expectedV = expectedU.Cross(normal).Normalized * circle.Radius;

            AssertVector(circle.AxisU, expectedU, 2e-5f);
            AssertVector(circle.AxisV, expectedV, 2e-5f);
            Assert.That(circle.AxisU.Length, Is.EqualTo(circle.Radius).Within(2e-5f));
            Assert.That(circle.AxisV.Length, Is.EqualTo(circle.Radius).Within(2e-5f));
            Assert.That(Math.Abs(circle.AxisU.Dot(normal)), Is.LessThan(2e-5f));
            Assert.That(Math.Abs(circle.AxisV.Dot(normal)), Is.LessThan(2e-5f));
            Assert.That(Math.Abs(circle.AxisU.Dot(circle.AxisV)), Is.LessThan(2e-4f));
        }

        [Test]
        public void Circle3d_AxesFormOrientedRadiusLengthFrame()
        {
            var normal = new V3d(1.0, 2.0, 3.0).Normalized;
            var circle = new Circle3d(V3d.Zero, normal, 7.0);
            var reference = Math.Abs(normal.X) > 0.9 ? V3d.YAxis : V3d.XAxis;
            var expectedU = normal.Cross(reference).Normalized * circle.Radius;
            var expectedV = expectedU.Cross(normal).Normalized * circle.Radius;

            AssertVector(circle.AxisU, expectedU, 2e-12);
            AssertVector(circle.AxisV, expectedV, 2e-12);
            Assert.That(circle.AxisU.Length, Is.EqualTo(circle.Radius).Within(2e-12));
            Assert.That(circle.AxisV.Length, Is.EqualTo(circle.Radius).Within(2e-12));
            Assert.That(Math.Abs(circle.AxisU.Dot(normal)), Is.LessThan(2e-12));
            Assert.That(Math.Abs(circle.AxisV.Dot(normal)), Is.LessThan(2e-12));
            Assert.That(Math.Abs(circle.AxisU.Dot(circle.AxisV)), Is.LessThan(2e-11));
        }

        [Test]
        public void Circle3f_BoundsUseExactObliqueProjectionExtents()
        {
            var normal = new V3f(1.0f, 1.0f, 1.0f).Normalized;
            var circle = new Circle3f(new V3f(2.0f, -3.0f, 5.0f), normal, 4.0f);
            var expectedExtent = new V3f(
                circle.Radius * MathF.Sqrt(MathF.Max(0.0f, 1.0f - normal.X * normal.X)),
                circle.Radius * MathF.Sqrt(MathF.Max(0.0f, 1.0f - normal.Y * normal.Y)),
                circle.Radius * MathF.Sqrt(MathF.Max(0.0f, 1.0f - normal.Z * normal.Z)));
            var bounds = circle.BoundingBox3f;

            AssertVector(bounds.Min, circle.Center - expectedExtent, 2e-5f);
            AssertVector(bounds.Max, circle.Center + expectedExtent, 2e-5f);
            AssertContainsDenseCircumference(circle, bounds, 2e-5f);
        }

        [Test]
        public void Circle3d_BoundsUseExactObliqueProjectionExtents()
        {
            var normal = new V3d(1.0, 1.0, 1.0).Normalized;
            var circle = new Circle3d(new V3d(2.0, -3.0, 5.0), normal, 4.0);
            var expectedExtent = new V3d(
                circle.Radius * Math.Sqrt(Math.Max(0.0, 1.0 - normal.X * normal.X)),
                circle.Radius * Math.Sqrt(Math.Max(0.0, 1.0 - normal.Y * normal.Y)),
                circle.Radius * Math.Sqrt(Math.Max(0.0, 1.0 - normal.Z * normal.Z)));
            var bounds = circle.BoundingBox3d;

            AssertVector(bounds.Min, circle.Center - expectedExtent, 2e-12);
            AssertVector(bounds.Max, circle.Center + expectedExtent, 2e-12);
            AssertContainsDenseCircumference(circle, bounds, 2e-12);
        }

        [Test]
        public void Circle3_ZeroRadiusCollapsesFramePointsAndBoundsToCenter()
        {
            var centerF = new V3f(1.0f, -2.0f, 3.0f);
            var circleF = new Circle3f(centerF, new V3f(1.0f, 2.0f, 3.0f).Normalized, 0.0f);
            Assert.That(circleF.AxisU, Is.EqualTo(V3f.Zero));
            Assert.That(circleF.AxisV, Is.EqualTo(V3f.Zero));
            Assert.That(circleF.Point, Is.EqualTo(centerF));
            Assert.That(circleF.GetPoint(1.25f), Is.EqualTo(centerF));
            Assert.That(circleF.BoundingBox3f.Min, Is.EqualTo(centerF));
            Assert.That(circleF.BoundingBox3f.Max, Is.EqualTo(centerF));

            var centerD = new V3d(1.0, -2.0, 3.0);
            var circleD = new Circle3d(centerD, new V3d(1.0, 2.0, 3.0).Normalized, 0.0);
            Assert.That(circleD.AxisU, Is.EqualTo(V3d.Zero));
            Assert.That(circleD.AxisV, Is.EqualTo(V3d.Zero));
            Assert.That(circleD.Point, Is.EqualTo(centerD));
            Assert.That(circleD.GetPoint(1.25), Is.EqualTo(centerD));
            Assert.That(circleD.BoundingBox3d.Min, Is.EqualTo(centerD));
            Assert.That(circleD.BoundingBox3d.Max, Is.EqualTo(centerD));
        }

        [TestCase(1e-30f)]
        [TestCase(1e30f)]
        public void Circle3f_AxisAlignedExtremeRadiusKeepsFiniteAxesAndBounds(float radius)
        {
            var circle = new Circle3f(V3f.Zero, V3f.ZAxis, radius);

            Assert.That(circle.AxisU, Is.EqualTo(new V3f(0.0f, radius, 0.0f)));
            Assert.That(circle.AxisV, Is.EqualTo(new V3f(radius, 0.0f, 0.0f)));
            Assert.That(circle.Point, Is.EqualTo(new V3f(0.0f, radius, 0.0f)));
            Assert.That(circle.BoundingBox3f.Min, Is.EqualTo(new V3f(-radius, -radius, 0.0f)));
            Assert.That(circle.BoundingBox3f.Max, Is.EqualTo(new V3f(radius, radius, 0.0f)));
            Assert.That(AllFinite(circle.AxisU) && AllFinite(circle.AxisV), Is.True);
        }

        [TestCase(1e-200)]
        [TestCase(1e200)]
        public void Circle3d_AxisAlignedExtremeRadiusKeepsFiniteAxesAndBounds(double radius)
        {
            var circle = new Circle3d(V3d.Zero, V3d.ZAxis, radius);

            Assert.That(circle.AxisU, Is.EqualTo(new V3d(0.0, radius, 0.0)));
            Assert.That(circle.AxisV, Is.EqualTo(new V3d(radius, 0.0, 0.0)));
            Assert.That(circle.Point, Is.EqualTo(new V3d(0.0, radius, 0.0)));
            Assert.That(circle.BoundingBox3d.Min, Is.EqualTo(new V3d(-radius, -radius, 0.0)));
            Assert.That(circle.BoundingBox3d.Max, Is.EqualTo(new V3d(radius, radius, 0.0)));
            Assert.That(AllFinite(circle.AxisU) && AllFinite(circle.AxisV), Is.True);
        }

        [Test]
        public void NearAxisBoundsRetainThinProjectionExtents()
        {
            foreach (var core in BoundsCores())
            foreach (var tilt in Enumerable.Range(core.Wide ? -40 : -20, core.Wide ? 19 : 13)
                .Select(e => Math.ScaleB(1, e)).Concat(new[] { core.Wide ? 1e-8 : 1e-4 }))
            foreach (var radius in new[] { Math.ScaleB(1, core.Wide ? -200 : -40), 0.125, 1.0, 16.0, Math.ScaleB(1, core.Wide ? 200 : 40) })
            for (var axis = 0; axis < 3; axis++)
            foreach (var sign in new[] { -1, 1 })
            foreach (var tail in new[] { 0.0, tilt, -0.5 * tilt })
            {
                var normal = new double[3]; normal[axis] = sign; normal[(axis + 1) % 3] = tilt; normal[(axis + 2) % 3] = tail;
                normal = core.Round(Unit(normal));
                foreach (var placement in new[] { 0, 1, 2 })
                    CheckProjectionBounds(core, normal, radius, placement, $"near axis={axis}, sign={sign}, tilt={tilt:R}, tail={tail:R}");
            }
        }

        [Test]
        public void TinyComponentsWithLargeRadiiKeepRepresentableExtents()
        {
            foreach (var core in BoundsCores())
            foreach (var exponent in core.Wide ? new[] { -1074, -1073, -1050, -1023, -1022, -1000, -600 } : new[] { -149, -148, -140, -126, -100, -80 })
            foreach (var radiusExponent in core.Wide ? new[] { 512, 900, 1000 } : new[] { 80, 100, 120 })
            for (var axis = 0; axis < 3; axis++)
            foreach (var sign in new[] { -1, 1 })
            foreach (var both in new[] { false, true })
            {
                var normal = new double[3]; normal[axis] = sign; normal[(axis + 1) % 3] = Math.ScaleB(1, exponent);
                if (both) normal[(axis + 2) % 3] = Math.ScaleB(1, exponent);
                normal = core.Round(Unit(normal));
                foreach (var placement in new[] { 0, 1 })
                    CheckProjectionBounds(core, normal, Math.ScaleB(1, radiusExponent), placement,
                        $"tiny axis={axis}, sign={sign}, exponent={exponent}, both={both}");
            }
        }

        [Test]
        public void BoundsKeepOrdinaryAxisAlignedZeroAndInvalidCases()
        {
            foreach (var core in BoundsCores())
            {
                foreach (var raw in new[] { new[] { 1.0, 2.0, 3.0 }, new[] { -3.0, 1.0, 2.0 }, new[] { 1.0, 1.0, 1.0 } })
                foreach (var radius in new[] { 0.0, 0.125, 4.0, Math.ScaleB(1, core.Wide ? 200 : 30) })
                    CheckProjectionBounds(core, core.Round(Unit(raw)), radius, 1, "ordinary oblique");
                for (var axis = 0; axis < 3; axis++)
                foreach (var sign in new[] { -1, 1 })
                foreach (var radius in new[] { 0.0, 0.125, 1.0, Math.ScaleB(1, core.Wide ? -1000 : -140), Math.ScaleB(1, core.Wide ? 1000 : 120) })
                {
                    var normal = new double[3]; normal[axis] = sign;
                    var center = new[] { 1.25, -2.5, 4.0 };
                    var bounds = core.Bounds(core.Circle(center, normal, radius));
                    for (var k = 0; k < 3; k++)
                    {
                        var extent = k == axis ? 0 : radius;
                        Assert.AreEqual(core.Round(center[k] - extent), bounds[k], core.Name + $" exact axis={axis}, sign={sign}, radius={radius:R}, component={k}");
                        Assert.AreEqual(core.Round(center[k] + extent), bounds[k + 3], core.Name + " exact maximum");
                    }
                }
                var invalid = core.Invalid;
                Assert.IsTrue(core.IsInvalid(invalid), core.Name + " Invalid sentinel");
                Assert.That(core.Bounds(invalid).All(double.IsNaN), Is.True, core.Name + " Invalid bounds stay NaN");
            }
        }

        private static void CheckProjectionBounds(BoundsCore core, double[] normal, double radius, int placement, string label)
        {
            radius = core.Round(radius);
            // An independently selected least-aligned reference yields a tangent frame.
            // No production frame property, Pythag, or 1-n_i^2 formula is the oracle.
            var n = Unit(normal); var reference = new double[3]; reference[Array.IndexOf(n.Select(Math.Abs).ToArray(), n.Select(Math.Abs).Min())] = 1;
            var u = Unit(Cross(n, reference)); var v = Unit(Cross(n, u));
            var extent = Enumerable.Range(0, 3).Select(k => ScaledLength(new[] { radius * u[k], radius * v[k] })).ToArray();
            var center = radius == 0 ? new[] { 1.25, -2.5, 4.0 } : extent.Select((e, k) => core.Round(placement == 0 ? 0 : e == 0 ? 2 + k : e * (placement == 1 ? (k % 2 == 0 ? 2 : -4) : 16))).ToArray();
            var circle = core.Circle(center, normal, radius); var bounds = core.Bounds(circle);
            var context = $"{core.Name}: {label}, radius={radius:R}, placement={placement}";
            var tolerance = new double[3];
            for (var k = 0; k < 3; k++)
            {
                tolerance[k] = 16 * core.UnitRoundoff * (Math.Abs(center[k]) + extent[k]);
                if (radius == 0) { Assert.AreEqual(center[k], bounds[k], context + " zero minimum"); Assert.AreEqual(center[k], bounds[k + 3], context + " zero maximum"); }
                else
                {
                    Assert.That(Math.Abs(bounds[k] - (center[k] - extent[k])), Is.LessThanOrEqualTo(tolerance[k]), context + $" support minimum[{k}], extent={extent[k]:R}");
                    Assert.That(Math.Abs(bounds[k + 3] - (center[k] + extent[k])), Is.LessThanOrEqualTo(tolerance[k]), context + $" support maximum[{k}], extent={extent[k]:R}");
                    if (extent[k] > 0) Assert.That(bounds[k + 3], Is.GreaterThan(bounds[k]), context + $" noncollapsed component[{k}]");
                }
            }
            var angles = Enumerable.Range(0, 16).Select(i => 2 * Math.PI * i / 16).Concat(Enumerable.Range(0, 3)
                .SelectMany(k => new[] { Math.Atan2(v[k], u[k]), Math.Atan2(v[k], u[k]) + Math.PI }));
            foreach (var angle in angles)
            {
                var analytic = Enumerable.Range(0, 3).Select(k => center[k] + radius * u[k] * Math.Cos(angle) + radius * v[k] * Math.Sin(angle)).ToArray();
                var actual = core.Point(circle, angle);
                for (var k = 0; k < 3; k++)
                {
                    Assert.That(analytic[k], Is.InRange(bounds[k] - tolerance[k], bounds[k + 3] + tolerance[k]), context + $" independent circumference[{k}], angle={angle:R}");
                    Assert.That(actual[k], Is.InRange(bounds[k] - tolerance[k], bounds[k + 3] + tolerance[k]), context + $" API circumference[{k}], angle={angle:R}");
                }
            }
        }
        private static double[] Cross(double[] a, double[] b) => new[] { a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0] };
        private static double ScaledLength(double[] a)
        {
            var scale = a.Select(Math.Abs).Max();
            return scale == 0 ? 0 : scale * Math.Sqrt(a.Sum(x => (x / scale) * (x / scale)));
        }
        private static double[] Unit(double[] a) { var length = ScaledLength(a); return a.Select(x => x / length).ToArray(); }

        private sealed class BoundsCore
        {
            public readonly string Name;
            public readonly bool Wide;
            public readonly double UnitRoundoff;
            private readonly Type vector, circle;
            private readonly ConstructorInfo vectorCtor, circleCtor;
            private readonly PropertyInfo bounds;
            private readonly MethodInfo point;
            public BoundsCore(Assembly assembly, string target, bool wide)
            {
                Wide = wide; UnitRoundoff = Math.ScaleB(1, wide ? -53 : -24); var suffix = wide ? "d" : "f"; Name = target + "/Circle3" + suffix;
                vector = assembly.GetType("Aardvark.Base.V3" + suffix, true); circle = assembly.GetType("Aardvark.Base.Circle3" + suffix, true);
                var scalar = wide ? typeof(double) : typeof(float);
                vectorCtor = vector.GetConstructor(new[] { scalar, scalar, scalar }); circleCtor = circle.GetConstructor(new[] { vector, vector, scalar });
                bounds = circle.GetProperty("BoundingBox3" + suffix); point = circle.GetMethod("GetPoint", new[] { scalar });
            }
            public double Round(double value) => Wide ? value : (double)(float)value;
            public double[] Round(double[] values) => values.Select(Round).ToArray();
            private object Scalar(double value) => Wide ? (object)value : (object)(float)value;
            private object Vector(double[] values) => vectorCtor.Invoke(values.Select(Scalar).ToArray());
            private double[] Coordinates(object value) => "XYZ".Select(c => Convert.ToDouble(vector.GetField(c.ToString()).GetValue(value))).ToArray();
            public object Circle(double[] center, double[] normal, double radius) => circleCtor.Invoke(new[] { Vector(center), Vector(normal), Scalar(radius) });
            public double[] Bounds(object value) { var box = bounds.GetValue(value); return Coordinates(box.GetType().GetField("Min").GetValue(box)).Concat(Coordinates(box.GetType().GetField("Max").GetValue(box))).ToArray(); }
            public double[] Point(object value, double angle) => Coordinates(point.Invoke(value, new[] { Scalar(angle) }));
            public object Invalid => circle.GetProperty("Invalid").GetValue(null);
            public bool IsInvalid(object value) => (bool)circle.GetProperty("IsInvalid").GetValue(value);
        }
        private static IEnumerable<BoundsCore> BoundsCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Circle3d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path); var assembly = Assembly.LoadFile(path);
                yield return new BoundsCore(assembly, target, false); yield return new BoundsCore(assembly, target, true);
            }
        }

        private static void AssertContainsDenseCircumference(Circle3f circle, Box3f bounds, float epsilon)
        {
            for (int i = 0; i < 4096; i++)
            {
                var point = circle.GetPoint(i * (ConstantF.PiTimesTwo / 4096.0f));
                Assert.That(point.X, Is.InRange(bounds.Min.X - epsilon, bounds.Max.X + epsilon));
                Assert.That(point.Y, Is.InRange(bounds.Min.Y - epsilon, bounds.Max.Y + epsilon));
                Assert.That(point.Z, Is.InRange(bounds.Min.Z - epsilon, bounds.Max.Z + epsilon));
            }
        }

        private static void AssertContainsDenseCircumference(Circle3d circle, Box3d bounds, double epsilon)
        {
            for (int i = 0; i < 4096; i++)
            {
                var point = circle.GetPoint(i * (Constant.PiTimesTwo / 4096.0));
                Assert.That(point.X, Is.InRange(bounds.Min.X - epsilon, bounds.Max.X + epsilon));
                Assert.That(point.Y, Is.InRange(bounds.Min.Y - epsilon, bounds.Max.Y + epsilon));
                Assert.That(point.Z, Is.InRange(bounds.Min.Z - epsilon, bounds.Max.Z + epsilon));
            }
        }

        private static void AssertVector(V3f actual, V3f expected, float epsilon)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(epsilon));
        }

        private static void AssertVector(V3d actual, V3d expected, double epsilon)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(epsilon));
        }

        private static bool AllFinite(V3f value)
            => float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);

        private static bool AllFinite(V3d value)
            => double.IsFinite(value.X) && double.IsFinite(value.Y) && double.IsFinite(value.Z);
    }
}

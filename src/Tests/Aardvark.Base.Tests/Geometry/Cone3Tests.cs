using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Cone3Tests
    {
        private static IEnumerable<(V3d axis, V3d radial, V3d other)> Frames()
        {
            var frames = new[]
            {
                (V3d.XAxis, V3d.YAxis, V3d.ZAxis),
                (V3d.YAxis, V3d.ZAxis, V3d.XAxis),
                (V3d.ZAxis, V3d.XAxis, V3d.YAxis),
                (new V3d(3.0/5, 4.0/5, 0), new V3d(-4.0/5, 3.0/5, 0), V3d.ZAxis),
                (new V3d(0, 3.0/5, 4.0/5), V3d.XAxis, new V3d(0, 4.0/5, -3.0/5)),
                (new V3d(1.0/3, 2.0/3, 2.0/3), new V3d(2.0/3, 1.0/3, -2.0/3), new V3d(-2.0/3, 2.0/3, -1.0/3))
            };
            foreach (var frame in frames)
            foreach (var sign in new[] { -1, 1 })
                yield return (sign * frame.Item1, frame.Item2, sign * frame.Item3);
        }
        private static readonly V3d[] Origins = { V3d.Zero, new V3d(8, -16, 4), new V3d(-32, 16, 8) };
        private static readonly double[] AxisScales = { .125, .5, 1, 2, 8, 64 };

        [Test]
        public void ConeHeightUsesSignedMetricProjection()
        {
            foreach (var core in Cores())
            foreach (var frame in Frames())
            foreach (var origin in Origins)
            foreach (var scale in AxisScales)
            {
                var cone = core.Cone(origin, scale * frame.axis, Math.PI / 4);
                core.StorageAndRay(cone, origin, scale * frame.axis);
                foreach (var height in new[] { -8.0, -2.0, -.25, -0.0, 0.0, .25, 2.0, 8.0 })
                foreach (var offset in new[] { (0.0, 0.0), (1.0, 0.0), (-2.0, 3.0) })
                {
                    var point = origin + height * frame.axis + offset.Item1 * frame.radial + offset.Item2 * frame.other;
                    double budget = core.Budget(origin, Math.Abs(height) + Math.Abs(offset.Item1) + Math.Abs(offset.Item2), 16);
                    var context = core.Context(origin, scale * frame.axis, point) + $", signed metric height={height:R}";
                    core.Close(height, core.Height(cone, point), budget, context);
                    var center = core.CircleCenter(cone, height);
                    core.VectorClose(origin + height * frame.axis, center, budget, context + " circle center");
                    core.Close(height, core.Height(cone, center), budget, context + " circle/height round trip");
                    core.Close(height / scale, core.AxisParameter(cone, point), budget / scale, context + " unchanged raw ray parameter");
                }
            }
        }

        [Test]
        public void ConeSurfaceQueriesUseMetricHeight()
        {
            foreach (var core in Cores())
            {
                // A metric height of two, not the raw ray parameter one.
                core.Surface(V3d.Zero, V3d.ZAxis, V3d.XAxis, 2, Math.PI / 4, 2, 2, "Direction=(0,0,2), point=(2,0,2)");
                foreach (var frame in Frames())
                foreach (var origin in Origins)
                foreach (var scale in AxisScales)
                foreach (var angle in new[] { Math.Atan(.5), Math.PI / 4, Math.Atan(2) })
                foreach (var height in new[] { .5, 2.0, 8.0 })
                foreach (var radial in new[] { frame.radial, frame.other, -frame.radial })
                foreach (var factor in new[] { .5, 1.0, 1.5 })
                {
                    // Construct in an independent orthonormal meridian frame, not
                    // through GetHeight, GetCircle or GetRadius.
                    double storedAngle = core.Scalar(angle);
                    double radius = height * Math.Tan(storedAngle) * factor;
                    core.Surface(origin, frame.axis, radial, scale, angle, height, radius,
                        $"analytic meridian projection, radius factor={factor:R}");
                }
            }
        }

        [Test]
        public void ConeUnitAxisAndStoredDirectionContractsRemain()
        {
            foreach (var core in Cores())
            foreach (var axis in new[] { V3d.XAxis, -V3d.XAxis, V3d.YAxis, -V3d.YAxis, V3d.ZAxis, -V3d.ZAxis })
            foreach (var origin in Origins)
            {
                var cone = core.Cone(origin, axis, Math.PI / 4);
                core.StorageAndRay(cone, origin, axis);
                var random = new Random(187139);
                for (int i = 0; i < 256; i++)
                {
                    var point = origin + new V3d(random.Next(-64, 65), random.Next(-64, 65), random.Next(-64, 65)) * .25;
                    double expected = core.AxisParameter(cone, point);
                    core.Bits(expected, core.Height(cone, point), core.Context(origin, axis, point) + ", unit-axis legacy parameter identity, seed187139 case=" + i);
                }
            }
        }

        private sealed class ConeCore
        {
            private readonly Type scalar, vector, coneType;
            private readonly ConstructorInfo vectorConstructor, coneConstructor;
            private readonly MethodInfo height, circle, axis, closest, distance;
            private readonly FieldInfo x, y, z;
            private readonly bool single;
            private readonly string name;
            public ConeCore(Assembly assembly, bool single, string target)
            {
                this.single = single; name = target + (single ? "/Cone3f" : "/Cone3d"); scalar = single ? typeof(float) : typeof(double);
                vector = assembly.GetType(single ? "Aardvark.Base.V3f" : "Aardvark.Base.V3d", true);
                coneType = assembly.GetType(single ? "Aardvark.Base.Cone3f" : "Aardvark.Base.Cone3d", true);
                Assert.AreSame(assembly, vector.Assembly, name); Assert.AreSame(assembly, coneType.Assembly, name);
                vectorConstructor = vector.GetConstructor(new[] { scalar, scalar, scalar }); coneConstructor = coneType.GetConstructor(new[] { vector, vector, scalar });
                Assert.AreSame(assembly, vectorConstructor.DeclaringType.Assembly, name); Assert.AreSame(assembly, coneConstructor.DeclaringType.Assembly, name);
                x = vector.GetField("X"); y = vector.GetField("Y"); z = vector.GetField("Z");
                foreach (var field in new[] { x, y, z }) { Assert.AreEqual(scalar, field.FieldType, name); Assert.AreSame(assembly, field.DeclaringType.Assembly, name); }
                height = Method(coneType, "GetHeight", scalar, vector); circle = Method(coneType, "GetCircle", assembly.GetType(single ? "Aardvark.Base.Circle3f" : "Aardvark.Base.Circle3d", true), scalar);
                axis = Method(coneType, "GetAxis", assembly.GetType(single ? "Aardvark.Base.Ray3f" : "Aardvark.Base.Ray3d", true));
                closest = Method(coneType, "GetClosestPoint", vector, vector); distance = Method(coneType, "GetDistance", scalar, vector);
            }
            private MethodInfo Method(Type owner, string method, Type result, params Type[] arguments)
            {
                var value = owner.GetMethod(method, arguments); Assert.IsNotNull(value, name + method); Assert.AreSame(owner.Assembly, value.DeclaringType.Assembly, name); Assert.AreEqual(result, value.ReturnType, name); return value;
            }
            public double Scalar(double value) => single ? (float)value : value;
            private object Box(double value) => single ? (object)(float)value : value;
            private object Vector(V3d value) => vectorConstructor.Invoke(new[] { Box(value.X), Box(value.Y), Box(value.Z) });
            private V3d Read(object value) => new V3d(Convert.ToDouble(x.GetValue(value)), Convert.ToDouble(y.GetValue(value)), Convert.ToDouble(z.GetValue(value)));
            private V3d Rounded(V3d value) => new V3d(Scalar(value.X), Scalar(value.Y), Scalar(value.Z));
            public object Cone(V3d origin, V3d direction, double angle) => coneConstructor.Invoke(new[] { Vector(origin), Vector(direction), Box(angle) });
            public string Context(V3d origin, V3d direction, V3d point) => $"{name}: origin={origin}, direction={direction}, point={point}";
            public double Budget(V3d origin, double extent, double operations) => operations * (single ? Math.ScaleB(1, -23) : Math.ScaleB(1, -52)) * (Math.Abs(origin.X) + Math.Abs(origin.Y) + Math.Abs(origin.Z) + extent);
            public void Close(double expected, double actual, double budget, string context)
            {
                Assert.IsTrue(double.IsFinite(actual), context + " finite result");
                Assert.That(Math.Abs(actual - expected), Is.LessThanOrEqualTo(budget), context + $", expected={expected:R}, actual={actual:R}, rounding bound={budget:R}");
            }
            public void VectorClose(V3d expected, V3d actual, double budget, string context)
            {
                Close(expected.X, actual.X, budget, context + " X"); Close(expected.Y, actual.Y, budget, context + " Y"); Close(expected.Z, actual.Z, budget, context + " Z");
            }
            public void Bits(double expected, double actual, string context) => Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context + $", expected={expected:R}, actual={actual:R}");
            public double Height(object cone, V3d point) => Convert.ToDouble(height.Invoke(cone, new[] { Vector(point) }));
            public V3d CircleCenter(object cone, double value)
            {
                var c = circle.Invoke(cone, new[] { Box(value) }); var field = circle.ReturnType.GetField("Center"); Assert.AreSame(coneType.Assembly, field.DeclaringType.Assembly, name); return Read(field.GetValue(c));
            }
            public double AxisParameter(object cone, V3d point)
            {
                var ray = axis.Invoke(cone, null); var project = Method(axis.ReturnType, "GetTOfProjectedPoint", scalar, vector); return Convert.ToDouble(project.Invoke(ray, new[] { Vector(point) }));
            }
            public void StorageAndRay(object cone, V3d origin, V3d direction)
            {
                var context = Context(origin, direction, origin); var ray = axis.Invoke(cone, null);
                foreach (var fieldName in new[] { "Origin", "Direction" })
                {
                    var expected = Rounded(fieldName == "Origin" ? origin : direction);
                    var stored = Read(coneType.GetField(fieldName).GetValue(cone)); var rayValue = Read(axis.ReturnType.GetField(fieldName).GetValue(ray));
                    foreach (int component in new[] { 0, 1, 2 }) { Bits(expected[component], stored[component], context + " stored " + fieldName); Bits(expected[component], rayValue[component], context + " ray " + fieldName); }
                }
                var point = Method(axis.ReturnType, "GetPointOnRay", vector, scalar);
                foreach (var parameter in new[] { -2.0, 0.0, 1.0, 3.0 })
                {
                    var expected = Rounded(Rounded(origin) + Rounded(Rounded(direction) * Scalar(parameter)));
                    var actual = Read(point.Invoke(ray, new[] { Box(parameter) }));
                    for (int i = 0; i < 3; i++) Bits(expected[i], actual[i], context + ", raw ray parameter=" + parameter + ", component=" + i);
                }
            }
            public void Surface(V3d origin, V3d unit, V3d radial, double scale, double angle, double axialHeight, double radius, string label)
            {
                var cone = Cone(origin, scale * unit, angle); var point = origin + axialHeight * unit + radius * radial;
                double a = Scalar(angle), sine = Math.Sin(a), cosine = Math.Cos(a);
                double along = axialHeight * cosine + radius * sine;
                var expected = origin + along * cosine * unit + along * sine * radial;
                double expectedDistance = Math.Abs(radius * cosine - axialHeight * sine);
                double budget = Budget(origin, axialHeight + Math.Abs(radius), 48);
                var context = Context(origin, scale * unit, point) + ", " + label + $", angle={a:R}, h={axialHeight:R}, radius={radius:R}";
                var actual = Read(closest.Invoke(cone, new[] { Vector(point) }));
                VectorClose(expected, actual, budget, context + " independent closest point");
                Close(expectedDistance, Convert.ToDouble(distance.Invoke(cone, new[] { Vector(point) })), budget, context + " independent normal distance");
                if (Math.Abs(radius - axialHeight * Math.Tan(a)) <= 8 * (single ? Math.ScaleB(1, -23) : Math.ScaleB(1, -52)) * Math.Abs(radius))
                    VectorClose(point, actual, budget, context + " independently constructed surface fixed point");
            }
        }
        private static IEnumerable<ConeCore> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Cone3d).Assembly.Location), "..", target, "Aardvark.Base.dll")); Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path); Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new ConeCore(assembly, true, target); yield return new ConeCore(assembly, false, target);
            }
        }
    }
}

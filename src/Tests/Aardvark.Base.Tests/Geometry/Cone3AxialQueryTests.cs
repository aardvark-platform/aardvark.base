using Aardvark.Base;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests.Geometry
{
    public class Cone3AxialQueryTests
    {
        private sealed class Api
        {
            public readonly bool Single;
            public readonly string Name;
            private readonly ConstructorInfo vector, cone, ray;
            private readonly MethodInfo closest, distance, height, radius, projection;
            private readonly FieldInfo x, y, z;
            public Api(string target, bool single)
            {
                Single = single; Name = target + (single ? "/float" : "/double");
                var path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, path);
                var assembly = Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                string suffix = single ? "f" : "d";
                var scalar = single ? typeof(float) : typeof(double);
                var v = assembly.GetType("Aardvark.Base.V3" + suffix, true);
                var c = assembly.GetType("Aardvark.Base.Cone3" + suffix, true);
                var r = assembly.GetType("Aardvark.Base.Ray3" + suffix, true);
                vector = v.GetConstructor(new[] { scalar, scalar, scalar }); cone = c.GetConstructor(new[] { v, v, scalar }); ray = r.GetConstructor(new[] { v, v });
                closest = c.GetMethod("GetClosestPoint", new[] { v }); distance = c.GetMethod("GetDistance", new[] { v });
                height = c.GetMethod("GetHeight", new[] { v }); radius = c.GetMethod("GetRadius", new[] { scalar });
                projection = assembly.GetType("Aardvark.Base.GeometryFun", true).GetMethod("GetClosestPointOn", new[] { v, r });
                foreach (var m in new[] { closest, distance, height, radius, projection }) Assert.That(m.DeclaringType.Assembly, Is.SameAs(assembly), Name);
                Assert.That(closest.ReturnType, Is.EqualTo(v)); Assert.That(distance.ReturnType, Is.EqualTo(scalar));
                x = v.GetField("X"); y = v.GetField("Y"); z = v.GetField("Z");
            }
            public double Round(double value) => Single ? (float)value : value;
            public V3d Round(V3d value) => Single ? (V3d)(V3f)value : value;
            private object Vector(V3d p) => vector.Invoke(Single ? new object[] { (float)p.X, (float)p.Y, (float)p.Z } : new object[] { p.X, p.Y, p.Z });
            private object Number(double value) => Single ? (object)(float)value : value;
            private V3d Read(object p) => new V3d(Convert.ToDouble(x.GetValue(p)), Convert.ToDouble(y.GetValue(p)), Convert.ToDouble(z.GetValue(p)));
            public object Cone(V3d origin, V3d axis, double angle) => cone.Invoke(new[] { Vector(origin), Vector(axis), Number(angle) });
            public V3d Closest(object c, V3d p) => Read(closest.Invoke(c, new[] { Vector(p) }));
            public double Distance(object c, V3d p) => Convert.ToDouble(distance.Invoke(c, new[] { Vector(p) }));
            public double Height(object c, V3d p) => Convert.ToDouble(height.Invoke(c, new[] { Vector(p) }));
            public double Radius(object c, double h) => Convert.ToDouble(radius.Invoke(c, new[] { Number(h) }));
            public V3d Project(V3d p, V3d origin, V3d axis) => Read(projection.Invoke(null, new[] { Vector(p), ray.Invoke(new[] { Vector(origin), Vector(axis) }) }));
        }
        private static Api[] Implementations() => new[] { new Api("net10.0", false), new Api("netstandard2.0", false), new Api("net10.0", true), new Api("netstandard2.0", true) };
        private static readonly V3d[] Axes = { V3d.XAxis, -V3d.XAxis, V3d.YAxis, -V3d.YAxis, V3d.ZAxis, -V3d.ZAxis };
        private static void Bits(double actual, double expected, string context)
        {
            if (double.IsNaN(expected)) Assert.That(double.IsNaN(actual), Is.True, context);
            else Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), context);
        }
        private static void Bits(V3d actual, V3d expected, string context)
        {
            Bits(actual.X, expected.X, context + " X"); Bits(actual.Y, expected.Y, context + " Y"); Bits(actual.Z, expected.Z, context + " Z");
        }
        private static void Analytic(Api api, V3d origin, V3d direction, V3d point, double angle)
        {
            origin = api.Round(origin); direction = api.Round(direction); point = api.Round(point); angle = api.Round(angle);
            string context = $"{api.Name} origin={origin} direction={direction} point={point} angle={angle:R}";
            Assert.That(api.Project(point, origin, direction), Is.EqualTo(point), context + " exact axial predicate");
            var axis = direction / direction.Abs().MaxElement; axis /= axis.Length;
            double h = (point - origin).Dot(axis), sin = Math.Sin(angle), cos = Math.Cos(angle);
            double tolerance = 8 * (api.Single ? Math.Pow(2, -23) : Math.Pow(2, -52)) * (h + origin.Abs().MaxElement);
            var cone = api.Cone(origin, direction, angle); var actual = api.Closest(cone, point); var offset = actual - origin;
            double axial = offset.Dot(axis), radial = (offset - axis * axial).Length;
            Assert.That(actual.IsFinite, Is.True, context);
            Assert.That(axial, Is.EqualTo(h * cos * cos).Within(tolerance), context + " axial displacement");
            Assert.That(radial, Is.EqualTo(h * sin * cos).Within(tolerance), context + " radius");
            Assert.That(radial, Is.EqualTo(axial * Math.Tan(angle)).Within(tolerance * (1 + Math.Tan(angle))), context + " surface membership");
            Assert.That(api.Distance(cone, point), Is.EqualTo(h * sin).Within(3 * tolerance), context + " nearest distance");
            var tangent = axis.Cross(Math.Abs(axis.X) > .9 ? V3d.YAxis : V3d.XAxis).Normalized;
            var expected = origin + axis * (h * cos * cos) + tangent * (h * sin * cos);
            Assert.That((actual - expected).Abs().MaxElement, Is.LessThanOrEqualTo(2 * tolerance), context + " deterministic Circle3 tie");
            Bits(api.Closest(cone, point), actual, context + " repeat");
            double legacyHeight = api.Single ? (float)((V3f)(point - origin)).Dot((V3f)direction) / ((V3f)direction).LengthSquared : (point - origin).Dot(direction) / direction.LengthSquared;
            Bits(api.Height(cone, point), legacyHeight, context + " unchanged GetHeight");
        }

        [Test]
        public void AxialQueriesChooseNearestDeterministicSurfacePoints()
        {
            foreach (var api in Implementations())
            {
                // The reported 45-degree, height-four failure is an explicit control.
                Analytic(api, V3d.Zero, V3d.ZAxis, new V3d(0, 0, 4), Math.PI / 4);
                foreach (var unit in Axes)
                    foreach (var origin in new[] { V3d.Zero, new V3d(8, -16, 32) })
                        foreach (double h in new[] { .125, 1, 4, 32, 128 })
                            foreach (double angle in new[] { Math.PI / 12, Math.PI / 6, Math.PI / 4, Math.PI / 3, 5 * Math.PI / 12 })
                                foreach (int power in api.Single ? new[] { -100, -60, -8, -2, 0, 3, 8, 60, 100 } : new[] { -1000, -500, -8, -2, 0, 3, 8, 500, 1000 })
                                    Analytic(api, origin, unit * Math.Pow(2, power), origin + unit * h, angle);
                foreach (var direction in new[] { new V3d(1, 1, 0), new V3d(1, 2, 2), new V3d(-1, -2, -2) })
                    foreach (int power in new[] { -8, 0, 8 })
                        Analytic(api, new V3d(8, -16, 32), direction * Math.Pow(2, power), new V3d(8, -16, 32) + direction * 4, Math.PI / 6);
            }
        }

        // Literal legacy path, using the actual target's unchanged projection/height/radius APIs.
        private static V3d Legacy(Api api, object cone, V3d origin, V3d direction, V3d point)
        {
            var cp = api.Project(point, origin, direction); double radius = api.Radius(cone, api.Height(cone, point));
            V3d p0, edge;
            if (api.Single)
            {
                var f0 = (V3f)cp + ((V3f)point - (V3f)cp).Normalized * (float)radius;
                p0 = (V3d)f0; edge = (V3d)(f0 - (V3f)origin).Normalized;
            }
            else { p0 = cp + (point - cp).Normalized * radius; edge = (p0 - origin).Normalized; }
            var p1 = api.Project(point, origin, edge);
            double d0 = api.Single ? ((V3f)point - (V3f)p0).Length : (point - p0).Length;
            double d1 = api.Single ? ((V3f)point - (V3f)p1).Length : (point - p1).Length;
            return d1 < d0 ? p1 : p0;
        }

        [Test]
        public void ExactAxisGuardPreservesNearAxisAndLegacyPaths()
        {
            foreach (var api in Implementations())
            {
                void Check(V3d origin, V3d direction, double angle, V3d point, bool offAxis)
                {
                    origin = api.Round(origin); direction = api.Round(direction); point = api.Round(point); angle = api.Round(angle);
                    string context = $"{api.Name} origin={origin} direction={direction} angle={angle:R} point={point}";
                    if (offAxis) Assert.That(api.Project(point, origin, direction), Is.Not.EqualTo(point), context + " noncoincident");
                    var cone = api.Cone(origin, direction, angle); var expected = Legacy(api, cone, origin, direction, point);
                    Bits(api.Closest(cone, point), expected, context);
                    double distance = api.Single ? ((V3f)point - (V3f)expected).Length : (point - expected).Length;
                    Bits(api.Distance(cone, point), distance, context + " distance");
                }
                foreach (var axis in Axes)
                {
                    var side = axis.Cross(Math.Abs(axis.X) > .9 ? V3d.YAxis : V3d.XAxis);
                    foreach (double scale in new[] { .25, 1, 4 })
                        foreach (double angle in new[] { .25, Math.PI / 4, 1.25 })
                        {
                            foreach (double radial in new[] { .25, 2, api.Single ? Math.Pow(2, -70) : Math.Pow(2, -500), api.Single ? float.Epsilon : double.Epsilon })
                                Check(V3d.Zero, axis * scale, angle, axis * 4 + side * radial, true);
                            Check(new V3d(8, -16, 32), axis * scale, angle, new V3d(8, -16, 32), false);
                            Check(V3d.Zero, axis * scale, angle, axis * -4, false);
                        }
                }
                foreach (double angle in new[] { 0, -.25, api.Single ? (double)(float)(Math.PI / 2) : Math.PI / 2, 2, double.NaN, double.PositiveInfinity, double.NegativeInfinity })
                    Check(V3d.Zero, V3d.ZAxis, angle, new V3d(0, 0, 4), false);
                Check(V3d.Zero, V3d.Zero, .5, V3d.Zero, false);
                Check(new V3d(double.NaN, 0, 0), V3d.ZAxis, .5, new V3d(0, 0, 4), false);
                Check(V3d.Zero, new V3d(0, 0, double.PositiveInfinity), .5, new V3d(0, 0, 4), false);
                Check(V3d.Zero, V3d.ZAxis, .5, new V3d(0, 0, double.PositiveInfinity), false);
            }
        }
    }
}

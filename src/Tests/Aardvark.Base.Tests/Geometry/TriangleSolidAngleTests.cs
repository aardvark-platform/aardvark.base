using Aardvark.Base;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests.Geometry
{
    public class TriangleSolidAngleTests
    {
        private static readonly int[][] Orders = { new[] { 0, 1, 2 }, new[] { 0, 2, 1 }, new[] { 1, 0, 2 }, new[] { 1, 2, 0 }, new[] { 2, 0, 1 }, new[] { 2, 1, 0 } };

        private sealed class Api
        {
            public readonly bool Single;
            public readonly string Name;
            private readonly ConstructorInfo vector, triangle;
            private readonly MethodInfo[] methods;
            public Api(string target, bool single)
            {
                Single = single; Name = target + (single ? "/float" : "/double");
                var path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, path);
                var asm = Assembly.LoadFile(path);
                Assert.That(asm.Location, Is.EqualTo(path));
                Assert.That(asm.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                var scalar = single ? typeof(float) : typeof(double);
                var v = asm.GetType("Aardvark.Base.V3" + (single ? "f" : "d"), true);
                var t = asm.GetType("Aardvark.Base.Triangle3" + (single ? "f" : "d"), true);
                vector = v.GetConstructor(new[] { scalar, scalar, scalar });
                triangle = t.GetConstructor(new[] { v, v, v });
                var owner = asm.GetType("Aardvark.Base.Triangle", true);
                methods = new[] { owner.GetMethod("SolidAngle", new[] { v, v, v }), owner.GetMethod("SolidAngle", new[] { t }),
                    owner.GetMethod("SolidAngle", new[] { v, v, v, v }), owner.GetMethod("SolidAngle", new[] { t, v }) };
                foreach (var m in methods)
                {
                    Assert.That(m.DeclaringType.Assembly, Is.SameAs(asm), Name);
                    Assert.That(m.ReturnType, Is.EqualTo(scalar), Name);
                }
            }
            public V3d Round(V3d p) => Single ? new V3d((float)p.X, (float)p.Y, (float)p.Z) : p;
            private object Vector(V3d p) => vector.Invoke(Single
                ? new object[] { (float)p.X, (float)p.Y, (float)p.Z } : new object[] { p.X, p.Y, p.Z });
            public double Call(V3d a, V3d b, V3d c, V3d p, int form)
            {
                var va = Vector(a); var vb = Vector(b); var vc = Vector(c); var vp = Vector(p);
                var t = triangle.Invoke(new[] { va, vb, vc });
                var args = form == 0 ? new[] { va, vb, vc } : form == 1 ? new[] { t } : form == 2 ? new[] { va, vb, vc, vp } : new[] { t, vp };
                return Convert.ToDouble(methods[form].Invoke(null, args));
            }
        }

        private static Api[] Implementations() => new[] { new Api("net10.0", false), new Api("netstandard2.0", false), new Api("net10.0", true), new Api("netstandard2.0", true) };
        // Independent spherical excess: angles between the great-circle tangent planes, not the triple-product quotient.
        private static double Excess(V3d a, V3d b, V3d c)
        {
            V3d Unit(V3d v) { v /= Math.Max(Math.Abs(v.X), Math.Max(Math.Abs(v.Y), Math.Abs(v.Z))); return v / Math.Sqrt(v.Dot(v)); }
            double Angle(V3d l, V3d m, V3d r) => Math.Acos(Math.Clamp(Unit(l.Cross(m)).Dot(Unit(r.Cross(m))), -1, 1));
            a = Unit(a); b = Unit(b); c = Unit(c);
            return Angle(b, a, c) + Angle(a, b, c) + Angle(a, c, b) - Math.PI;
        }
        private static void Check(Api api, V3d a, V3d b, V3d c, V3d p, double expected, string context, double? tolerance = null)
        {
            for (int form = 0; form < 4; form++)
            {
                var result = api.Call(a, b, c, p, form);
                Assert.That(double.IsFinite(result) && result >= 0 && result <= 2 * Math.PI + 1e-6, Is.True, api.Name + context + " form=" + form);
                Assert.That(result, Is.EqualTo(expected).Within(tolerance ?? (api.Single ? 4e-6 : 2e-13)), api.Name + context + " form=" + form);
            }
        }

        [Test]
        public void AxisTrianglesKeepTheirAngleAcrossRayScalesAndObservers()
        {
            foreach (var api in Implementations())
            {
                double[] scales = api.Single
                    ? new[] { (double)float.Epsilon, Math.Pow(2, -140), 1e-20, 1e-10, 1, 1e10, 1e20, Math.Pow(2, 126), float.MaxValue }
                    : new[] { double.Epsilon, Math.Pow(2, -1070), 1e-200, 1e-100, 1, 1e100, 1e200, Math.Pow(2, 1022), double.MaxValue };
                for (int i = 0; i < scales.Length; i++)
                    for (int j = 0; j < scales.Length; j++)
                        for (int sign = 0; sign < 8; sign++)
                        {
                            var rays = new[] { api.Round(new V3d((sign & 1) == 0 ? scales[i] : -scales[i], 0, 0)),
                                api.Round(new V3d(0, (sign & 2) == 0 ? scales[j] : -scales[j], 0)),
                                api.Round(new V3d(0, 0, (sign & 4) == 0 ? scales[(i + j) % scales.Length] : -scales[(i + j) % scales.Length])) };
                            foreach (var order in Orders)
                                Check(api, rays[order[0]], rays[order[1]], rays[order[2]], V3d.Zero, Math.PI / 2, $" axes i={i} j={j} sign={sign} order={string.Join(",", order)}", api.Single ? 4e-7 : 8e-16);
                        }
                foreach (int power in api.Single ? new[] { -140, -80, 0, 80, 120 } : new[] { -1070, -600, 0, 600, 1000 })
                {
                    double s = Math.Pow(2, power);
                    var p = api.Round(new V3d(-4, 8, -16) * s);
                    var rays = new[] { new V3d(s, 0, 0), new V3d(0, 2 * s, 0), new V3d(0, 0, 4 * s) };
                    foreach (var order in Orders)
                    {
                        var a = api.Round(p + rays[order[0]]); var b = api.Round(p + rays[order[1]]); var c = api.Round(p + rays[order[2]]);
                        Assert.That(new[] { api.Round(a - p), api.Round(b - p), api.Round(c - p) },
                            Is.EqualTo(new[] { rays[order[0]], rays[order[1]], rays[order[2]] }), api.Name + " representable observer differences");
                        for (int form = 2; form < 4; form++)
                            Assert.That(api.Call(a, b, c, p, form), Is.EqualTo(Math.PI / 2).Within(api.Single ? 4e-7 : 8e-16), api.Name + $" observer power={power} form={form}");
                    }
                }
            }
        }

        [Test]
        public void NonAxisTrianglesKeepSphericalExcessUnderIndependentScaling()
        {
            var shapes = new[] { new[] { new V3d(1, .5, .25), new V3d(.25, 1, .5), new V3d(.5, .25, 1) },
                new[] { new V3d(1, 0, .25), new V3d(-.5, 1, .25), new V3d(-.5, -1, .25) } };
            foreach (var api in Implementations())
            {
                int[] powers = api.Single ? new[] { -147, -125, -80, -35, 0, 35, 80, 125 } : new[] { -1072, -1020, -600, -335, 0, 335, 600, 1020 };
                foreach (var shape in shapes)
                    foreach (int x in powers) foreach (int y in powers)
                    {
                        var rays = new[] { api.Round(shape[0] * Math.Pow(2, x)), api.Round(shape[1] * Math.Pow(2, y)), api.Round(shape[2] * Math.Pow(2, -Math.Min(x, y) / 2)) };
                        var expected = Excess(rays[0], rays[1], rays[2]);
                        foreach (var order in Orders)
                            Check(api, rays[order[0]], rays[order[1]], rays[order[2]], V3d.Zero, expected, $" powers={x},{y} shape={shape[0]} order={string.Join(",", order)}");
                    }
            }
        }

        [Test]
        public void OrdinaryAndExceptionalPathsKeepTheirLiteralFormula()
        {
            foreach (var api in Implementations())
            {
                var random = new Random(211187);
                V3d Next() => api.Round(new V3d(.25 + random.NextDouble(), .25 + random.NextDouble(), .25 + random.NextDouble()));
                for (int i = 0; i < 2048; i++)
                {
                    var a = Next(); var b = Next(); var c = Next();
                    Check(api, a, b, c, V3d.Zero, Excess(a, b, c), " seeded i=" + i);
                    Literal(api, a, b, c, " seeded i=" + i);
                }
                var controls = new[] { V3d.Zero, new V3d(-0.0, 0, -0.0), new V3d(double.NaN, 1, 2), new V3d(double.PositiveInfinity, 0, 0), new V3d(0, double.NegativeInfinity, 1) };
                foreach (var bad in controls) for (int slot = 0; slot < 3; slot++)
                    foreach (double s in new[] { 1.0, api.Single ? 1e30 : 1e200, api.Single ? 1e-30 : 1e-200 })
                    {
                        var v = new[] { api.Round(V3d.XAxis * s), api.Round(V3d.YAxis * s), api.Round(V3d.ZAxis * s) }; v[slot] = api.Round(bad);
                        Literal(api, v[0], v[1], v[2], $" legacy bad={bad} slot={slot} s={s}");
                    }
                Literal(api, V3d.XAxis, -V3d.XAxis, V3d.YAxis, " planar");
                foreach (var observer in new[] { new V3d(double.NaN, 0, 0), new V3d(double.PositiveInfinity, 0, 0) })
                    for (int form = 2; form < 4; form++)
                        Assert.That(double.IsNaN(api.Call(V3d.XAxis, V3d.YAxis, V3d.ZAxis, observer, form)), Is.True, api.Name + " nonfinite observer");
                var maximum = api.Single ? float.MaxValue : double.MaxValue;
                for (int form = 2; form < 4; form++)
                    Assert.That(api.Call(V3d.One, V3d.One + V3d.XAxis, V3d.One + V3d.YAxis, V3d.One, form), Is.EqualTo(0), api.Name + " observer at vertex");
                Literal(api, new V3d(-maximum, 0, 0), V3d.YAxis, V3d.ZAxis, " overflowing observer subtraction", new V3d(maximum, 0, 0));
            }
        }
        private static void Literal(Api api, V3d a, V3d b, V3d c, string context, V3d? observer = null)
        {
            var p = api.Round(observer ?? V3d.Zero);
            var ra = api.Round(a - p); var rb = api.Round(b - p); var rc = api.Round(c - p);
            double expected;
            if (api.Single)
            {
                var x = (V3f)ra; var y = (V3f)rb; var z = (V3f)rc;
                var n = Fun.Abs(x.Dot(y.Cross(z)));
                var d = x.Length * y.Length * z.Length + x.Dot(y) * z.Length + x.Dot(z) * y.Length + y.Dot(z) * x.Length;
                var h = api.Name.StartsWith("net10.0") ? MathF.Atan2(n, d) : (float)Math.Atan2(n, d);
                expected = 2 * (h >= 0 ? h : h + ConstantF.Pi);
            }
            else
            {
                var n = Fun.Abs(ra.Dot(rb.Cross(rc)));
                var d = ra.Length * rb.Length * rc.Length + ra.Dot(rb) * rc.Length + ra.Dot(rc) * rb.Length + rb.Dot(rc) * ra.Length;
                var h = Math.Atan2(n, d); expected = 2 * (h >= 0 ? h : h + Math.PI);
            }
            for (int form = observer.HasValue ? 2 : 0; form < 4; form++)
            {
                var actual = api.Call(a, b, c, p, form);
                if (double.IsNaN(expected)) Assert.That(double.IsNaN(actual), Is.True, api.Name + context);
                else Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), api.Name + context + " form=" + form);
            }
        }
    }
}

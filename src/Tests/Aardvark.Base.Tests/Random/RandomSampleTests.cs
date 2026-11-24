using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests
{
    public class RandomSampleTests
    {
        private static readonly double[] Azimuths = { 0, .125, .25, .375, .5, .75, 1, -.125 };
        private sealed class Api
        {
            public readonly bool Single, Native;
            public readonly string Name;
            public readonly Type SeriesType;
            private readonly MethodInfo scalar, series;
            private readonly FieldInfo x, y, z;
            public Api(string target, bool single)
            {
                Single = single; Native = target == "net10.0"; Name = target + (single ? "/float" : "/double");
                var path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, path);
                var assembly = Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(Native ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                var owner = assembly.GetType("Aardvark.Base.RandomSample", true);
                var number = single ? typeof(float) : typeof(double);
                var vector = assembly.GetType("Aardvark.Base.V3" + (single ? "f" : "d"), true);
                scalar = owner.GetMethod("Spherical", new[] { number, number });
                Assert.That(scalar.DeclaringType.Assembly, Is.SameAs(assembly));
                Assert.That(scalar.ReturnType, Is.EqualTo(vector));
                SeriesType = assembly.GetType("Aardvark.Base.IRandomSeries", true);
                series = owner.GetMethod("Spherical", new[] { SeriesType, typeof(int) });
                Assert.That(series.DeclaringType.Assembly, Is.SameAs(assembly));
                x = vector.GetField("X"); y = vector.GetField("Y"); z = vector.GetField("Z");
            }
            private V3d Read(object result) => new V3d(Convert.ToDouble(x.GetValue(result)), Convert.ToDouble(y.GetValue(result)), Convert.ToDouble(z.GetValue(result)));
            public V3d Call(double a, double b) => Read(scalar.Invoke(null, Single ? new object[] { (float)a, (float)b } : new object[] { a, b }));
            public V3d Forward(object random, int index) => Read(series.Invoke(null, new[] { random, (object)index }));
            public double Round(double v) => Single ? (float)v : v;
            public double Z(double u) => Single ? 1f - 2f * (float)u : 1 - 2 * u;
            public (double Cos, double Sin) Trig(double a)
            {
                if (!Single) { double p = 2 * Math.PI * a; return (Math.Cos(p), Math.Sin(p)); }
                float phi = (float)(2 * Math.PI) * (float)a;
                return Native ? (MathF.Cos(phi), MathF.Sin(phi)) : ((float)Math.Cos(phi), (float)Math.Sin(phi));
            }
        }
        private static Api[] Implementations() => new[] { new Api("net10.0", false), new Api("netstandard2.0", false), new Api("net10.0", true), new Api("netstandard2.0", true) };
        private static void Bits(double actual, double expected, string context)
        {
            if (double.IsNaN(expected)) Assert.That(double.IsNaN(actual), Is.True, context);
            else Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), context);
        }
        private static void Component(Api api, double actual, double expected, string context)
        {
            expected = api.Round(expected);
            Assert.That(double.IsFinite(actual), Is.True, api.Name + context);
            ulong a = api.Single ? (uint)BitConverter.SingleToInt32Bits((float)actual) : BitConverter.DoubleToUInt64Bits(actual);
            ulong b = api.Single ? (uint)BitConverter.SingleToInt32Bits((float)expected) : BitConverter.DoubleToUInt64Bits(expected);
            Assert.That(a > b ? a - b : b - a, Is.LessThanOrEqualTo(8UL), api.Name + context + $" actual={actual:R} expected={expected:R}");
        }
        private static void Accurate(Api api, double a, double u, V3d actual)
        {
            a = api.Round(a); u = api.Round(u);
            // Independent product of square roots, never derived from the rounded Z or 1-Z*Z.
            double radius = 2 * Math.Sqrt(u) * Math.Sqrt(1 - u);
            var trig = api.Trig(a);
            string context = $" azimuth={a:R} draw={u:R}";
            Component(api, actual.X, radius * trig.Cos, context + " X");
            Component(api, actual.Y, radius * trig.Sin, context + " Y");
            Bits(actual.Z, api.Z(u), api.Name + context + " unchanged Z");
        }

        [Test]
        public void NearPoleDrawsKeepResolvableTransverseComponents()
        {
            foreach (var api in Implementations())
            {
                var draws = new HashSet<double>();
                for (int power = 1; power <= (api.Single ? 149 : 1074); power++) draws.Add(api.Round(Math.Pow(2, -power)));
                double minimum = api.Single ? float.Epsilon : double.Epsilon;
                foreach (int multiplier in new[] { 1, 3, 7, 31 }) draws.Add(api.Round(multiplier * minimum));
                double collapse = Math.Pow(2, api.Single ? -26 : -55);
                double Next(double value, bool up) => api.Single
                    ? (up ? MathF.BitIncrement((float)value) : MathF.BitDecrement((float)value))
                    : (up ? Math.BitIncrement(value) : Math.BitDecrement(value));
                double low = collapse, high = collapse;
                for (int i = 0; i < 4; i++) { draws.Add(low); draws.Add(high); low = Next(low, false); high = Next(high, true); }
                for (int power = 1; power <= (api.Single ? 24 : 53); power++) draws.Add(api.Round(1 - Math.Pow(2, -power)));
                foreach (double u in draws) foreach (double a in Azimuths) Accurate(api, a, u, api.Call(a, u));
            }
        }

        [Test]
        public void OrdinaryDrawsPolesAndExceptionalInputsKeepTheirContracts()
        {
            foreach (var api in Implementations())
            {
                var random = new Random(223211);
                for (int i = 0; i < 2048; i++)
                {
                    double a = api.Round(random.NextDouble()), u = api.Round(random.NextDouble());
                    Accurate(api, a, u, api.Call(a, u));
                }
                var azimuths = new List<double>(Azimuths) { -0.0, double.NaN, double.NegativeInfinity, double.PositiveInfinity, api.Single ? float.MaxValue : double.MaxValue };
                var inputs = new[] { -0.0, 0.0, 1.0, -.25, -1.0, 1.25, 2.0, double.NaN, double.NegativeInfinity, double.PositiveInfinity, api.Single ? float.MaxValue : double.MaxValue, api.Single ? float.MinValue : double.MinValue };
                foreach (double a in azimuths) foreach (double input in inputs)
                {
                    double u = api.Round(input), zz = api.Z(u);
                    double n = api.Single ? 1f - (float)zz * (float)zz : 1 - zz * zz;
                    double radius = api.Single ? (api.Native ? MathF.Sqrt((float)(n > 0 ? n : 0)) : (float)Math.Sqrt(n > 0 ? n : 0)) : Math.Sqrt(n > 0 ? n : 0);
                    var trig = api.Trig(a); var actual = api.Call(a, u);
                    Bits(actual.X, api.Round(radius * trig.Cos), api.Name + $" legacy a={a:R} u={u:R} X");
                    Bits(actual.Y, api.Round(radius * trig.Sin), api.Name + $" legacy a={a:R} u={u:R} Y");
                    Bits(actual.Z, zz, api.Name + $" legacy a={a:R} u={u:R} Z");
                }
            }
        }

        public class ScriptedSeries : DispatchProxy
        {
            public readonly List<int> Indices = new List<int>();
            public readonly Queue<double> Values = new Queue<double>();
            protected override object Invoke(MethodInfo method, object[] args)
            {
                Assert.That(method.Name, Is.EqualTo("UniformDouble"));
                Indices.Add((int)args[0]);
                return Values.Dequeue();
            }
        }

        [Test]
        public void SeriesForwardingKeepsDrawOrderIndicesAndScalarResults()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                var api = new Api(target, false);
                using var scope = System.Runtime.Loader.AssemblyLoadContext.EnterContextualReflection(api.SeriesType.Assembly);
                // DispatchProxy caches by its base type's load context, not just interface identity.
                var module = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName("SeriesProxy_" + Guid.NewGuid().ToString("N")), System.Reflection.Emit.AssemblyBuilderAccess.Run).DefineDynamicModule("Proxy");
                var proxyBase = module.DefineType("SeriesProxyBase", TypeAttributes.Public, typeof(ScriptedSeries)).CreateType();
                foreach (int index in new[] { -3, 0, 7, 31, int.MaxValue - 1, int.MaxValue })
                    foreach (double u in new[] { double.Epsilon, Math.Pow(2, -55), 1e-12, .125, .5, Math.BitDecrement(1.0), 0, 1 })
                        foreach (double a in Azimuths)
                        {
                            var random = (ScriptedSeries)DispatchProxy.Create(api.SeriesType, proxyBase);
                            Assert.That(api.SeriesType.IsInstanceOfType(random), Is.True, target + " exact series-interface identity");
                            random.Values.Enqueue(a); random.Values.Enqueue(u);
                            var actual = api.Forward(random, index); var expected = api.Call(a, u);
                            string context = $"{target} seriesIndex={index} a={a:R} u={u:R}";
                            Bits(actual.X, expected.X, context + " X"); Bits(actual.Y, expected.Y, context + " Y"); Bits(actual.Z, expected.Z, context + " Z");
                            Assert.That(random.Indices, Is.EqualTo(new[] { index, unchecked(index + 1) }), context);
                            Assert.That(random.Values.Count, Is.Zero, context);
                            Accurate(api, a, u, actual);
                        }
            }
        }
    }
}

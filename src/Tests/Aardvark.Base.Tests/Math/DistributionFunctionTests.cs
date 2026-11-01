using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using System.Runtime.Versioning;

namespace Aardvark.Tests
{
    [TestFixture]
    public class DistributionFunctionTests
    {
        [Test]
        public void TerminalThresholdsSkipZeroMassTails()
        {
            foreach (var core in Cores())
            {
                foreach (var tail in new[] { 0, 1, 31, 4095, 65535 })
                foreach (var factor in new[] { double.Epsilon, 1.0 })
                {
                    var pdf = new double[6 + tail];
                    pdf[1] = factor; pdf[3] = 3 * factor; pdf[5] = 2 * factor;
                    CheckConstructors(core, pdf, new[] { 0.0, .5, .75, Math.BitDecrement(1.0) });
                }
                foreach (var length in new[] { 1, 2, 17, 1025, 65537 })
                foreach (var index in new HashSet<int> { 0, length / 2, length - 1 })
                foreach (var mass in new[] { double.Epsilon, 2 * double.Epsilon, 3 * double.Epsilon, Math.ScaleB(1, -1022), 1.0, Math.ScaleB(1, 900) })
                {
                    var pdf = new double[length]; pdf[index] = mass;
                    CheckConstructors(core, pdf, new[] { 0.0, .5, Math.BitIncrement(.5), .75, Math.BitDecrement(1.0) });
                }
                Assert.AreEqual(double.Epsilon, .75 * double.Epsilon, "Interior draw rounds to the terminal threshold");
                CheckConstructors(core, new[] { double.Epsilon, 0.0 }, new[] { .75 });
            }
        }

        [Test]
        public void InteriorThresholdsKeepExactCutoffSelection()
        {
            foreach (var core in Cores())
            for (var length = 1; length <= 5; length++)
            {
                var combinations = (int)Math.Pow(3, length);
                for (var code = 1; code < combinations; code++)
                {
                    var pdf = new double[length]; var digits = code;
                    for (var i = 0; i < length; i++) { pdf[i] = digits % 3; digits /= 3; }
                    var cdf = Integrate(pdf);
                    var draws = new HashSet<double> { 0, .125, .25, .5, .75, Math.BitDecrement(1.0) };
                    for (var i = 1; i < cdf.Length - 1; i++)
                    {
                        var cutoff = cdf[i] / cdf[cdf.Length - 1];
                        if (cutoff < 1) draws.Add(cutoff);
                        if (cutoff > 0) draws.Add(Math.BitDecrement(cutoff));
                        if (cutoff < 1) draws.Add(Math.BitIncrement(cutoff));
                    }
                    CheckConstructors(core, pdf, draws);
                }
            }
        }

        [Test]
        public void UpdatedCdfsUseCurrentWeightsAndOneRandomDraw()
        {
            var stages = new[]
            {
                new[] { 1.0, 0, 0, 0, 0 }, new[] { 0.0, 0, 2, 0, 0 },
                new[] { 0.0, 1, 0, 3, 0 }, new[] { double.Epsilon, 0, 0, 0, 0 },
                new[] { 0.0, 0, 0, 0, 4 }
            };
            foreach (var core in Cores())
            foreach (var supplied in new[] { false, true })
            {
                var pdf = (double[])stages[0].Clone(); var cdf = Integrate(pdf);
                var distribution = core.Create(pdf, supplied ? cdf : null);
                var storage = core.Cdf(distribution);
                foreach (var stage in stages)
                {
                    Array.Copy(stage, pdf, pdf.Length);
                    core.Update(distribution);
                    Assert.AreSame(storage, core.Cdf(distribution), core.Name + " update preserves CDF storage");
                    CollectionAssert.AreEqual(Integrate(pdf), storage, core.Name + " current PDF integration");
                    foreach (var draw in new[] { 0.0, .25, .5, .75, Math.BitDecrement(1.0) })
                        Check(core, distribution, storage, draw, "updated PDF=[" + string.Join(",", pdf) + "]");
                    CollectionAssert.AreEqual(stage, pdf, core.Name + " sampling preserves PDF");
                }
            }
        }

        private static double[] Integrate(double[] pdf)
        {
            var cdf = new double[pdf.Length + 1];
            for (var i = 0; i < pdf.Length; i++) cdf[i + 1] = cdf[i] + pdf[i];
            return cdf;
        }

        private static int Expected(double[] cdf, double draw)
        {
            var total = cdf[cdf.Length - 1]; var threshold = draw * total;
            // Independent interval-membership oracle: a half-open draw can round its
            // threshold to the total; this does not make the draw interval closed.
            for (var i = 0; i < cdf.Length - 1; i++)
                if (cdf[i] < cdf[i + 1] && cdf[i] <= threshold &&
                    (threshold < cdf[i + 1] || threshold == total && cdf[i + 1] == total)) return i;
            throw new InvalidOperationException("No positive-mass interval for the valid draw");
        }

        private static void CheckConstructors(SamplingCore core, double[] pdf, IEnumerable<double> draws)
        {
            foreach (var supplied in new[] { false, true })
            {
                var expectedCdf = Integrate(pdf); var distribution = core.Create(pdf, supplied ? expectedCdf : null);
                var cdf = core.Cdf(distribution); var snapshot = (double[])cdf.Clone();
                CollectionAssert.AreEqual(expectedCdf, cdf, core.Name + " constructor integration");
                var label = $"length={pdf.Length}, supplied={supplied}, total={cdf[cdf.Length - 1]:R}";
                foreach (var draw in draws) Check(core, distribution, cdf, draw, label);
                CollectionAssert.AreEqual(snapshot, cdf, core.Name + ": sampling preserves CDF, " + label);
            }
        }

        private static void Check(SamplingCore core, object distribution, double[] cdf, double draw, string label)
        {
            Assert.That(draw >= 0 && draw < 1, Is.True, $"{core.Name}: {label}, fixture draw={draw:R} must be in [0, 1)");
            var expected = Expected(cdf, draw); var context = $"{core.Name}: {label}, draw={draw:R}, expected={expected}";
            Assert.That(cdf[expected + 1], Is.GreaterThan(cdf[expected]), context + " positive interval");
            for (var api = 0; api < 4; api++) Assert.AreEqual(expected, core.Sample(distribution, cdf, draw, api), context + ", API=" + api);
        }

        public class SingleDrawRandom : DispatchProxy
        {
            public double Value;
            public int Calls;
            protected override object Invoke(MethodInfo method, object[] args)
            {
                if (method.Name != "UniformDouble" || ++Calls != 1) throw new InvalidOperationException("Unexpected random operation: " + method.Name);
                return Value;
            }
        }

        private sealed class SamplingCore
        {
            public readonly string Name;
            private readonly Type distributionType, randomType, proxyBaseType;
            private readonly MethodInfo direct, random, staticDirect, staticRandom;
            public SamplingCore(Assembly assembly, string name)
            {
                Name = name;
                distributionType = assembly.GetType("Aardvark.Base.DistributionFunction", true);
                randomType = assembly.GetType("Aardvark.Base.IRandomUniform", true);
                Assert.AreSame(assembly, distributionType.Assembly, name); Assert.AreSame(assembly, randomType.Assembly, name);
                direct = distributionType.GetMethod("Sample", new[] { typeof(double) });
                random = distributionType.GetMethod("Sample", new[] { randomType });
                staticDirect = distributionType.GetMethod("SampleCDF", new[] { typeof(double[]), typeof(double) });
                staticRandom = distributionType.GetMethod("SampleCDF", new[] { typeof(double[]), randomType });
                foreach (var method in new[] { direct, random, staticDirect, staticRandom }) Assert.IsNotNull(method, name);
                // DispatchProxy caches its generated assembly by the base type's load context.
                // Give each actual core its own base type, so equal assembly names cannot
                // bind the portable interface to the native core's interface identity.
                using (AssemblyLoadContext.EnterContextualReflection(assembly))
                {
                    var module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("SamplingProxy_" + Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.Run).DefineDynamicModule("Proxy");
                    proxyBaseType = module.DefineType("SamplingProxyBase", TypeAttributes.Public, typeof(SingleDrawRandom)).CreateType();
                }
            }
            public object Create(double[] pdf, double[] cdf) => cdf == null
                ? Activator.CreateInstance(distributionType, new object[] { pdf })
                : Activator.CreateInstance(distributionType, new object[] { pdf, cdf });
            public double[] Cdf(object distribution) => (double[])distributionType.GetProperty("CDF").GetValue(distribution);
            public void Update(object distribution) => distributionType.GetMethod("UpdateCDF").Invoke(distribution, null);
            public int Sample(object distribution, double[] cdf, double draw, int api)
            {
                if (api == 0) return (int)direct.Invoke(distribution, new object[] { draw });
                if (api == 1) return (int)staticDirect.Invoke(null, new object[] { cdf, draw });
                var proxy = DispatchProxy.Create(randomType, proxyBaseType); var scripted = (SingleDrawRandom)proxy; scripted.Value = draw;
                Assert.That(randomType.IsInstanceOfType(proxy), Is.True, Name + " exact random-interface identity");
                var result = api == 2 ? random.Invoke(distribution, new[] { proxy }) : staticRandom.Invoke(null, new[] { (object)cdf, proxy });
                Assert.AreEqual(1, scripted.Calls, Name + " one UniformDouble per random-forwarding API");
                return (int)result;
            }
        }

        private static IEnumerable<SamplingCore> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(DistributionFunction).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new SamplingCore(assembly, target);
            }
        }
    }
}

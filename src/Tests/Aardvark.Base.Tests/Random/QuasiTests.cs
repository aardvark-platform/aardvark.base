using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests
{
    [TestFixture]
    public class QuasiTests
    {
        private static readonly int[] Indices = { 0, 1, 2, 3, 4, 5, 6, 10, 24 };
        private static readonly int[] Radices = { 2, 3, 5, 7, 11, 13, 17, 31, 97 };
        private static double RadicalInverse(ulong index, uint radix)
        {
            ulong numerator = 0, denominator = 1;
            while (index != 0)
            {
                numerator = numerator * radix + index % radix;
                denominator *= radix; index /= radix;
            }
            return (double)numerator / denominator;
        }
        private static void Sample(double value, string context)
        {
            Assert.IsTrue(double.IsFinite(value), context + " finite");
            Assert.That(value, Is.GreaterThanOrEqualTo(0).And.LessThan(1), context + ", value=" + value.ToString("R"));
        }
        private static void Bits(double expected, double actual, string context)
            => Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context + $"; expected={expected:R}, actual={actual:R}");

        [Test]
        public void BinaryNearOneCarriesKeepExactSuccessors()
        {
            foreach (var core in Cores())
            for (int k = 34; k <= 53; k++)
            {
                double previous = 1 - Math.ScaleB(1, -k), expected = Math.ScaleB(1, -k - 1);
                string context = $"{core.Name}: binary k={k}, previous={previous:R}";
                double actual = core.Direct(.5, previous); Sample(actual, context);
                Bits(expected, actual, context); Bits(expected, core.Indexed(0, previous), context + " indexed");
            }
        }

        [Test]
        public void PrimeNearOneStatesAndIndexedForwardingStayHalfOpen()
        {
            foreach (var core in Cores())
            for (int p = 0; p < Indices.Length; p++)
            for (int k = 34; k <= 53; k++)
            {
                double previous = 1 - Math.ScaleB(1, -k), inverse = 1.0 / Radices[p];
                for (int step = 0; step < 64; step++)
                {
                    string context = $"{core.Name}: radix={Radices[p]}, k={k}, step={step}, previous={previous:R}";
                    double actual = core.Direct(inverse, previous); Sample(actual, context);
                    Bits(actual, core.Indexed(Indices[p], previous), context + " forwarding");
                    if (step == 0) Assert.That(actual, Is.GreaterThan(0), context + " positive carry");
                    previous = actual;
                }
            }
        }

        [Test]
        public void OrdinaryPrefixesRetainRadicalInverseCarries()
        {
            foreach (var core in Cores())
            {
                for (int p = 0; p < Indices.Length; p++)
                {
                    double previous = 0, inverse = 1.0 / Radices[p];
                    for (ulong index = 1; index <= 16384; index++)
                    {
                        string context = $"{core.Name}: radix={Radices[p]}, index={index}, previous={previous:R}";
                        double expected = RadicalInverse(index, (uint)Radices[p]);
                        double actual = core.Direct(inverse, previous); Sample(actual, context);
                        Assert.That(actual, Is.EqualTo(expected).Within(1e-12), context + " integer digit-reversal oracle");
                        Bits(LegacyPositive(inverse, previous), actual, context + " original arithmetic");
                        Bits(actual, core.Indexed(Indices[p], previous), context + " indexed"); previous = actual;
                    }
                }
                // Explicit base-three powers include the old relative-gap
                // failure at2187 and carries beyond the mandatory long prefix.
                ulong power = 1;
                for (int exponent = 1; exponent <= 10; exponent++)
                {
                    power *= 3;
                    for (ulong index = power - 1; index <= power + 1; index++)
                    {
                        double previous = RadicalInverse(index - 1, 3), actual = core.Direct(1.0 / 3, previous);
                        string context = $"{core.Name}: base3 carry exponent={exponent}, index={index}";
                        Assert.That(actual, Is.EqualTo(RadicalInverse(index, 3)).Within(1e-12), context);
                        Bits(LegacyPositive(1.0 / 3, previous), actual, context + " original positive-threshold arithmetic");
                    }
                }
                // Preserve even the old numerical limitations while the adjusted
                // threshold remains positive; this repair is termination-only.
                foreach (double previous in new[] { 1 - Math.ScaleB(1, -33), Math.BitDecrement(1 - 1e-10), 1 - 1e-10 })
                    if (1 - previous - 1e-10 > 0) Bits(LegacyPositive(.5, previous), core.Direct(.5, previous), core.Name + " positive-threshold compatibility");
            }
        }

        // Only positive adjusted thresholds reach this original-arithmetic
        // oracle, so original nontermination is never entered by the test host.
        private static double LegacyPositive(double inverse, double value)
        {
            double r = 1 - value - 1e-10; Assert.That(r, Is.GreaterThan(0), "bounded legacy oracle setup");
            if (inverse < r) value += inverse;
            else
            {
                double h = inverse * inverse, hh = inverse;
                while (h >= r) { hh = h; h *= inverse; }
                value += hh + h - 1;
            }
            return value;
        }

        [Test]
        public void ScriptedHaltonSeriesContinuesWithoutExtraDraws()
        {
            foreach (var core in Cores())
            for (int k = 34; k <= 53; k++)
            {
                const int dimensions = 7;
                var draws = new double[dimensions + 2]; var expected = new double[dimensions];
                for (int d = 0; d < dimensions; d++) draws[d] = expected[d] = 1 - Math.ScaleB(1, -k);
                draws[dimensions] = .375; draws[dimensions + 1] = .625;
                var random = new ScriptedRandom(draws); var series = core.CreateSeries(dimensions, random);
                Assert.AreEqual(dimensions, random.Draws, core.Name + " initialization draws");
                for (int step = 0; step < 64; step++)
                for (int d = dimensions - 1; d >= 0; d--)
                {
                    string context = $"{core.Name}: series k={k}, step={step}, dimension={d}";
                    expected[d] = core.Indexed(d, expected[d]); double actual = series(d); Sample(actual, context);
                    Bits(expected[d], actual, context + " state advancement");
                    Assert.AreEqual(dimensions, random.Draws, context + " no extra random draws");
                    if (step == 0 && d == 0) Bits(Math.ScaleB(1, -k - 1), actual, context + " exact first binary successor");
                }
                Bits(.375, series(dimensions), core.Name + " first random fallback");
                Bits(.625, series(dimensions + 3), core.Name + " later random fallback");
                Assert.AreEqual(draws.Length, random.Draws, core.Name + " exactly one draw per fallback");
            }
        }
        private sealed class ScriptedRandom : Random
        {
            private readonly double[] values;
            public int Draws { get; private set; }
            public ScriptedRandom(double[] values) => this.values = values;
            public override double NextDouble()
            {
                if (Draws == values.Length) throw new InvalidOperationException("Unexpected uniform draw");
                return values[Draws++];
            }
        }
        private sealed class HaltonCore
        {
            public readonly string Name;
            public readonly Func<double, double, double> Direct;
            public readonly Func<int, double, double> Indexed;
            private readonly Type randomSystem, series, uniform;
            public HaltonCore(Assembly assembly, string target)
            {
                Name = target;
                var quasi = assembly.GetType("Aardvark.Base.Quasi", true); var prime = assembly.GetType("Aardvark.Base.Prime", true);
                randomSystem = assembly.GetType("Aardvark.Base.RandomSystem", true); series = assembly.GetType("Aardvark.Base.HaltonRandomSeries", true); uniform = assembly.GetType("Aardvark.Base.IRandomUniform", true);
                foreach (var type in new[] { quasi, prime, randomSystem, series, uniform }) Assert.AreSame(assembly, type.Assembly, target);
                var direct = quasi.GetMethod("QuasiHalton", new[] { typeof(double), typeof(double) });
                var indexed = quasi.GetMethod("QuasiHaltonWithIndex", new[] { typeof(int), typeof(double) });
                foreach (var method in new[] { direct, indexed }) { Assert.AreSame(assembly, method.DeclaringType.Assembly, target); Assert.AreEqual(typeof(double), method.ReturnType, target); Assert.IsTrue(method.IsStatic, target); }
                Direct = (Func<double, double, double>)direct.CreateDelegate(typeof(Func<double, double, double>));
                Indexed = (Func<int, double, double>)indexed.CreateDelegate(typeof(Func<int, double, double>));
                var lookup = prime.GetMethod("WithIndex", new[] { typeof(int) }); Assert.AreSame(assembly, lookup.DeclaringType.Assembly, target);
                for (int p = 0; p < Indices.Length; p++) Assert.AreEqual(Radices[p], lookup.Invoke(null, new object[] { Indices[p] }), target + " fixed radix oracle");
            }
            public Func<int, double> CreateSeries(int dimensions, Random random)
            {
                var constructor = randomSystem.GetConstructor(new[] { typeof(int) }); Assert.AreSame(randomSystem.Assembly, constructor.DeclaringType.Assembly, Name);
                var generator = randomSystem.GetField("Generator"); Assert.AreSame(randomSystem.Assembly, generator.DeclaringType.Assembly, Name); Assert.AreEqual(typeof(Random), generator.FieldType, Name);
                var rng = constructor.Invoke(new object[] { 0 }); generator.SetValue(rng, random);
                var ctor = series.GetConstructor(new[] { typeof(int), uniform }); Assert.AreSame(series.Assembly, ctor.DeclaringType.Assembly, Name);
                var instance = ctor.Invoke(new[] { (object)dimensions, rng }); var method = series.GetMethod("UniformDouble", new[] { typeof(int) });
                Assert.AreSame(series.Assembly, method.DeclaringType.Assembly, Name); Assert.AreEqual(typeof(double), method.ReturnType, Name);
                return (Func<int, double>)method.CreateDelegate(typeof(Func<int, double>), instance);
            }
        }
        private static IEnumerable<HaltonCore> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Quasi).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path); var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new HaltonCore(assembly, target);
            }
        }
    }
}

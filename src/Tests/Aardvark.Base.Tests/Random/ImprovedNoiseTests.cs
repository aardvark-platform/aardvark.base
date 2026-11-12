using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ImprovedNoiseTests
    {
        private static readonly double[][] Anchors = { new[] { .25, .375, .625 }, new[] { -1.125, 2.75, -.5 }, new[] { 3.5, -.125, 1.25 } };

        [Test]
        public void LargeCoordinatesKeepTheLatticePeriod()
        {
            foreach (var core in Cores())
            {
                Same(core.Noise(1.25, .375, .625), core.Noise(2147483649.25, .375, .625), core.Name + " required reproduction");
                var values = new List<double>();
                foreach (int exponent in new[] { 31, 32, 40, 48, 50 })
                foreach (int sign in new[] { -1, 1 })
                foreach (int integer in new[] { 1, 2, 17, 127, 255, 256, 511 })
                foreach (double fraction in new[] { 0, .25, .5, .75 })
                    values.Add(sign * (Math.ScaleB(1, exponent) + integer) + fraction);
                foreach (int exponent in new[] { 52, 53, 60, 128, 512, 1023 })
                foreach (int sign in new[] { -1, 1 })
                foreach (double value in new[] { Math.BitDecrement(Math.ScaleB(1, exponent)), Math.ScaleB(1, exponent), Math.BitIncrement(Math.ScaleB(1, exponent)) })
                    values.Add(sign * value);
                values.AddRange(new[] { double.MaxValue, -double.MaxValue, Math.BitDecrement(double.MaxValue), -Math.BitDecrement(double.MaxValue) });
                foreach (double value in values)
                {
                    Assert.IsTrue(Math.Floor(value) < int.MinValue || Math.Floor(value) > int.MaxValue, "large-coordinate setup");
                    foreach (int axis in new[] { 0, 1, 2 })
                    foreach (var anchor in Anchors)
                    {
                        var p = (double[])anchor.Clone(); p[axis] = value;
                        var q = (double[])p.Clone(); q[axis] = Cell(value) + (value - Math.Floor(value));
                        Check(core, p, "large axis=" + axis);
                        Same(core.Noise(q[0], q[1], q[2]), core.Noise(p[0], p[1], p[2]), Context(core, p, "period-equivalent axis=" + axis));
                    }
                }
                foreach (int exponent in new[] { 31, 40, 50, 60, 512, 1023 })
                foreach (int sign in new[] { -1, 1 })
                foreach (var anchor in Anchors)
                    Check(core, new[] { sign * Math.ScaleB(1, exponent) + 1.25, -sign * Math.ScaleB(1, exponent) - 17.375, sign * Math.ScaleB(1, exponent) + 255.625 + anchor[2] }, "multiple large axes");
            }
        }

        [Test]
        public void NoiseStaysContinuousAtInt32LatticeLimits()
        {
            foreach (var core in Cores())
            foreach (double boundary in new[] { (double)int.MinValue, int.MaxValue, 2147483648.0 })
            foreach (int axis in new[] { 0, 1, 2 })
            foreach (var anchor in Anchors)
            {
                var node = (double[])anchor.Clone(); node[axis] = boundary;
                Check(core, node, "integer limit");
                double atNode = core.Noise(node[0], node[1], node[2]);
                foreach (int direction in new[] { -1, 1 })
                {
                    double value = boundary;
                    for (int step = 1; step <= 8; step++)
                    {
                        value = direction < 0 ? Math.BitDecrement(value) : Math.BitIncrement(value);
                        var p = (double[])node.Clone(); p[axis] = value;
                        Check(core, p, $"limit neighbor axis={axis}, step={direction * step}");
                        // |gradient| <= 2, |fade'| <= 1.875, and each direct
                        // gradient derivative <= 1 give |dNoise/dAxis| <= 8.5.
                        // The factor16 also covers evaluation roundoff here;
                        // the bound scales only with the represented displacement.
                        Assert.That(Math.Abs(core.Noise(p[0], p[1], p[2]) - atNode), Is.LessThanOrEqualTo(16 * Math.Abs(value - boundary)), Context(core, p, "continuity"));
                    }
                }
            }
        }

        [Test]
        public void OrdinaryBitsAndNonfiniteResultsStayCompatible()
        {
            foreach (var core in Cores())
            {
                double[] ordinary = { -0.0, 0.0, double.Epsilon, -double.Epsilon, -.25, .5, Math.BitDecrement(-1), -1, Math.BitIncrement(-1), Math.BitDecrement(1), 1, Math.BitIncrement(1), int.MinValue, Math.BitIncrement(int.MinValue), int.MaxValue, int.MaxValue + .75 };
                foreach (double x in ordinary)
                foreach (double y in ordinary)
                foreach (double z in ordinary)
                    Check(core, new[] { x, y, z }, "ordinary boundary bits");
                var random = new Random(203173);
                for (int i = 0; i < 8192; i++)
                    Check(core, new[] { random.Next(-1000000, 1000001) / 16.0, random.Next(-1000000, 1000001) / 16.0, random.Next(-1000000, 1000001) / 16.0 }, "ordinary seed203173 case=" + i);
                double[] special = { double.NaN, double.NegativeInfinity, double.PositiveInfinity };
                foreach (double value in special)
                foreach (int axis in new[] { 0, 1, 2 })
                foreach (var anchor in Anchors)
                {
                    var p = (double[])anchor.Clone(); p[axis] = value;
                    Assert.IsTrue(double.IsNaN(core.Noise(p[0], p[1], p[2])), Context(core, p, "nonfinite axis=" + axis));
                }
                foreach (double x in special)
                foreach (double y in special)
                foreach (double z in special)
                    Assert.IsTrue(double.IsNaN(core.Noise(x, y, z)), Context(core, new[] { x, y, z }, "multiple nonfinite axes"));
            }
        }

        private static string Context(NoiseCore core, double[] p, string label)
            => $"{core.Name}: {label}; ({p[0]:R},{p[1]:R},{p[2]:R})";
        private static void Same(double expected, double actual, string context)
            => Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context + $"; expected={expected:R}, actual={actual:R}");
        private static void Check(NoiseCore core, double[] p, string label)
            => Same(Reference(p[0], p[1], p[2]), core.Noise(p[0], p[1], p[2]), Context(core, p, label));

        // BigInteger preserves the exact integral double before extracting its
        // low eight bits, independently of narrowing conversions or remainder.
        private static int Cell(double value) => (int)(new BigInteger(Math.Floor(value)) & 255);
        private static double Reference(double x, double y, double z)
        {
            int cx = Cell(x), cy = Cell(y), cz = Cell(z);
            x -= Math.Floor(x); y -= Math.Floor(y); z -= Math.Floor(z);
            double u = Fade(x), v = Fade(y), w = Fade(z);
            int a = P(cx) + cy, aa = P(a) + cz, ab = P(a + 1) + cz;
            int b = P(cx + 1) + cy, ba = P(b) + cz, bb = P(b + 1) + cz;
            return Lerp(w, Lerp(v, Lerp(u, Gradient(P(aa), x, y, z), Gradient(P(ba), x - 1, y, z)),
                                  Lerp(u, Gradient(P(ab), x, y - 1, z), Gradient(P(bb), x - 1, y - 1, z))),
                           Lerp(v, Lerp(u, Gradient(P(aa + 1), x, y, z - 1), Gradient(P(ba + 1), x - 1, y, z - 1)),
                                  Lerp(u, Gradient(P(ab + 1), x, y - 1, z - 1), Gradient(P(bb + 1), x - 1, y - 1, z - 1))));
        }
        private static double Fade(double t) => t * t * t * (t * (t * 6 - 15) + 10);
        private static double Lerp(double t, double a, double b) => a + t * (b - a);
        private static double Gradient(int hash, double x, double y, double z)
        {
            switch (hash & 15)
            {
                case 0: return x + y; case 1: return -x + y; case 2: return x - y; case 3: return -x - y;
                case 4: return x + z; case 5: return -x + z; case 6: return x - z; case 7: return -x - z;
                case 8: return y + z; case 9: return -y + z; case 10: return y - z; case 11: return -y - z;
                case 12: return y + x; case 13: return -y + z; case 14: return y - x; default: return -y - z;
            }
        }
        private static int P(int i) => Permutation[i & 255];
        private static readonly int[] Permutation = {
            151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,
            30,69,142,8,99,37,240,21,10,23,190,6,148,247,120,234,75,0,26,197,
            62,94,252,219,203,117,35,11,32,57,177,33,88,237,149,56,87,174,20,
            125,136,171,168,68,175,74,165,71,134,139,48,27,166,77,146,158,231,
            83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,102,
            143,54,65,25,63,161,1,216,80,73,209,76,132,187,208,89,18,169,200,
            196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,52,217,
            226,250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,
            47,16,58,17,182,189,28,42,223,183,170,213,119,248,152,2,44,154,
            163,70,221,153,101,155,167,43,172,9,129,22,39,253,19,98,108,110,
            79,113,224,232,178,185,112,104,218,246,97,228,251,34,242,193,238,
            210,144,12,191,179,162,241,81,51,145,235,249,14,239,107,49,192,
            214,31,181,199,106,157,184,84,204,176,115,121,50,45,127,4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,
            156,180
        };

        private sealed class NoiseCore
        {
            public readonly string Name;
            public readonly Func<double, double, double, double> Noise;
            public NoiseCore(Assembly assembly, string target)
            {
                Name = target;
                var type = assembly.GetType("Aardvark.Base.ImprovedNoise", true);
                Assert.AreSame(assembly, type.Assembly, target);
                var method = type.GetMethod("Noise", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly, null, new[] { typeof(double), typeof(double), typeof(double) }, null);
                Assert.IsNotNull(method, target); Assert.AreSame(assembly, method.DeclaringType.Assembly, target); Assert.AreEqual(typeof(double), method.ReturnType, target);
                Noise = (Func<double, double, double, double>)method.CreateDelegate(typeof(Func<double, double, double, double>));
            }
        }
        private static IEnumerable<NoiseCore> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(ImprovedNoise).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new NoiseCore(assembly, target);
            }
        }
    }
}

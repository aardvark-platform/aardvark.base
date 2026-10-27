using System.Linq;
using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ColorTests : TestSuite
    {
        public ColorTests() : base() { }
        public ColorTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestColor()
        {
            ConversionTests();
        }

        public void ConversionTests()
        {
            using (Report.JobTimed("color conversion tests"))
            {
                Report.BeginTimed("create color arrays");
                var c3bVol = new Volume<C3b>(17, 17, 17).SetByCoord((r, g, b) => new C3b(15 * r, 15 * g, 15 * b));
                var c3bArray = c3bVol.Data;
                var c3fArray = c3bArray.Map(C3f.FromC3b);
                var cieXYZArray = c3fArray.Map(c => c.SRGBToCieXYZf());
                Report.End();

                var count = c3bArray.Length;

                Test.Begin("CieXYZ tests");
                for (int i = 0; i < count; i++)
                {
                    var ciexyz = cieXYZArray[i];
                    var c3b = ciexyz.ToSRGB().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

                Test.Begin("CieYxy tests");
                for (int i = 0; i < count; i++)
                {
                    var cieyxy = cieXYZArray[i].ToCieYxyf();
                    var c3b = cieyxy.ToCieXYZf().ToSRGB().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

                Test.Begin("HSV tests");
                for (int i = 0; i < count; i++)
                {
                    var hsvf = c3fArray[i].ToHSVf();
                    var c3b = hsvf.ToC3f().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

                Test.Begin("HSL tests");
                for (int i = 0; i < count; i++)
                {
                    var hslf = c3fArray[i].ToHSLf();
                    var c3b = hslf.ToC3f().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

            }
        }

        [Test]
        public void IndexerC4b()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = new C4b(rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt());

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void IndexerC4us()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = new C4us(rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt());

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void IndexerC4ui()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = rnd.UniformC4ui();

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void IndexerC4f()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = rnd.UniformC4f();

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void AverageC4fPreservesChannelOrder()
        {
            var colors = new[]
            {
                new C4f(0.10f, 0.20f, 0.70f, 0.30f),
                new C4f(0.30f, 0.60f, 0.10f, 0.90f),
            };

            var average = Col.Average(colors);

            Assert.AreEqual(0.20f, average.R, 1e-6f);
            Assert.AreEqual(0.40f, average.G, 1e-6f);
            Assert.AreEqual(0.40f, average.B, 1e-6f);
            Assert.AreEqual(0.60f, average.A, 1e-6f);
        }

        [Test]
        public void IndexerC4d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = rnd.UniformC4d();

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void Parse()
        {
            var rnd = new RandomSystem(0);

            for (int i = 0; i < 100; i++)
            {
                var c = rnd.UniformC4ui();
                var s1 = $"[{c.R}, {c.G}, {c.B}, {c.A}]";
                var s2 = $"[{c.R}, {c.G}, {c.B}]";

                Assert.AreEqual(c.RGB, C3ui.Parse(s1));
                Assert.AreEqual(c, C4ui.Parse(s1));
                Assert.AreEqual(c.RGB, C3ui.Parse(s2));
                Assert.AreEqual(new C4ui(c.RGB, uint.MaxValue), C4ui.Parse(s2));
            }
        }

        [Test]
        public void ParseTooManyComponents()
        {
            var t1 = new Text("[1,2,3");
            var c1 = t1.NestedBracketSplitCount2(1);
            var r1 = t1.NestedBracketSplit(1).ToArray();

            var t2 = new Text("[42]");
            var c2 = t2.NestedBracketSplitCount2(1);
            var r2 = t2.NestedBracketSplit(1).ToArray();

            var t3 = new Text("[42,43]");
            var c3 = t3.NestedBracketSplitCount2(1);
            var r3 = t3.NestedBracketSplit(1).ToArray();

            Assert.AreEqual(r1.Length, c1);
            Assert.AreEqual(r2.Length, c2);
            Assert.AreEqual(r3.Length, c3);

            Assert.IsFalse(C4d.TryParse("[1, 2, 3, 4, 5]", out C4d _));
        }

        [Test]
        public void ParseHex()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 100; i++)
            {
                var color = rnd.UniformC4d().ToC4b();
                var str = color.ToHexString();

                Assert.IsTrue(Col.TryParseHex(str, out C4b result));
                Assert.IsTrue(C4b.TryParse(str, out C4b result2));
                Assert.AreEqual(color, result);
                Assert.AreEqual(result, result2);
            }
        }

        [Test]
        public void ParseHexSingleDigit()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 100; i++)
            {
                var str = rnd.UniformC4d().ToHexString();
                var dbl = $"#{str[0]}{str[0]}{str[2]}{str[2]}{str[4]}{str[4]}";
                var sgl = $"0x{str[0]}{str[2]}{str[4]}F";

                Assert.IsTrue(Col.TryParseHex(dbl, out C4b a));
                Assert.IsTrue(C4b.TryParse(sgl, out C4b b));
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        [SetCulture("de-DE")]
        public void ParseWithBadCulture()
        {
            var rnd = new RandomSystem(1);
            var col = rnd.UniformC4d();
            var str = col.ToString();
            var res = C4d.Parse(str);

            Assert.True(Fun.ApproximateEquals(col, res));
        }

        [Test]
        public void C4fArithmetic()
        {
            var rnd = new RandomSystem(1);
            var c0 = rnd.UniformC4f();
            var c1 = rnd.UniformC4f();

            Assert.AreEqual( c0.ToV4f() * c1.ToV4f(), (c0   * c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() * c1.R,       (c0   * c1.R).ToV4f() );
            Assert.AreEqual( c0.R       * c1.ToV4f(), (c0.R * c1).ToV4f()   );

            Assert.AreEqual( c0.ToV4f() / c1.ToV4f(), (c0   / c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() / c1.R,       (c0   / c1.R).ToV4f() );
            Assert.AreEqual( c0.R       / c1.ToV4f(), (c0.R / c1).ToV4f()   );

            Assert.AreEqual( c0.ToV4f() + c1.ToV4f(), (c0   + c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() + c1.R,       (c0   + c1.R).ToV4f() );
            Assert.AreEqual( c0.R       + c1.ToV4f(), (c0.R + c1).ToV4f()   );

            Assert.AreEqual( c0.ToV4f() - c1.ToV4f(), (c0   - c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() - c1.R,       (c0   - c1.R).ToV4f() );
            Assert.AreEqual( c0.R       - c1.ToV4f(), (c0.R - c1).ToV4f()   );
        }

        [Test]
        public void BytePremultiplicationMatchesEveryChannelAlphaPair()
        {
            foreach (var core in PremultiplicationCores().Where(c => c.Maximum == byte.MaxValue))
            {
                core.Check(255, 17, 0, 33, "full red at alpha 33");
                for (uint alpha = 0; alpha <= byte.MaxValue; alpha++)
                for (uint channel = 0; channel <= byte.MaxValue; channel++)
                    core.Check(channel, 255 - channel, (73 * channel + 19) & 255, alpha, "exhaustive byte pairs");
            }
        }

        [Test]
        public void WidePremultiplicationMatchesExactProductsAndQuotientEdges()
        {
            foreach (var core in PremultiplicationCores().Where(c => c.Maximum != byte.MaxValue))
            {
                if (core.Maximum == ushort.MaxValue)
                {
                    core.Check(65535, 17, 0, 33, "full red at alpha 33");
                    for (uint alpha = 0; alpha <= ushort.MaxValue; alpha++)
                        core.Check(65535, 65535 - alpha, alpha, alpha, "full ushort channel across every alpha");
                }
                else
                {
                    var reproduction = core.Check(608754615, 123456789, 4000000001, 3689012193, "reported uint quotient");
                    Assert.AreEqual(522868521u, reproduction[0], core.Name + " reported exact red");
                    var alphas = new uint[] { 1, 2, 3, 5, 17, 33, 255, 257, 65535, 65537,
                        2147483647, 2147483648, 3689012193, uint.MaxValue - 1, uint.MaxValue };
                    foreach (var alpha in alphas)
                    foreach (var quotient in new uint[] { 0, 1, alpha / 2, alpha - 1, alpha }.Distinct())
                    {
                        // Locate c*alpha = quotient*maximum with BigInteger, then test
                        // adjacent representable channels on both sides of the boundary.
                        var boundary = (BigInteger)quotient * uint.MaxValue / alpha;
                        for (var offset = -2; offset <= 2; offset++)
                        {
                            var channel = boundary + offset;
                            if (channel < 0 || channel > uint.MaxValue) continue;
                            core.Check((uint)channel, uint.MaxValue - (uint)channel,
                                (uint)((channel * 73 + 19) % ((BigInteger)uint.MaxValue + 1)), alpha,
                                $"uint quotient={quotient}, offset={offset}");
                        }
                    }
                    for (var dr = -2; dr <= 2; dr++)
                    for (var da = -2; da <= 2; da++)
                        core.Check((uint)(608754615 + dr), 1, uint.MaxValue, (uint)(3689012193L + da), "uint reproduction neighbors");
                }
                var anchors = PremultiplicationAnchors(core.Maximum);
                foreach (var channel in anchors)
                foreach (var alpha in anchors)
                    core.Check(channel, core.Maximum - channel, channel / 3, alpha, "wide power/extremum anchors");
                var random = new Random(core.Maximum == ushort.MaxValue ? 71813 : 71817);
                var bytes = new byte[16];
                for (var i = 0; i < 4096; i++)
                {
                    random.NextBytes(bytes);
                    core.Check(BitConverter.ToUInt32(bytes, 0) & core.Maximum, BitConverter.ToUInt32(bytes, 4) & core.Maximum,
                        BitConverter.ToUInt32(bytes, 8) & core.Maximum, BitConverter.ToUInt32(bytes, 12) & core.Maximum, "seeded full-width colors");
                }
            }
        }

        [Test]
        public void PremultiplicationKeepsZeroAndOpaqueColors()
        {
            foreach (var core in PremultiplicationCores())
            {
                foreach (var channel in PremultiplicationAnchors(core.Maximum))
                foreach (var alpha in new[] { 0u, core.Maximum })
                    core.Check(channel, core.Maximum - channel, channel / 3, alpha, "zero/opaque anchors");
                var random = new Random(71823);
                var bytes = new byte[12];
                for (var i = 0; i < 4096; i++)
                {
                    random.NextBytes(bytes);
                    var r = BitConverter.ToUInt32(bytes, 0) & core.Maximum;
                    var g = BitConverter.ToUInt32(bytes, 4) & core.Maximum;
                    var b = BitConverter.ToUInt32(bytes, 8) & core.Maximum;
                    core.Check(r, g, b, 0, "zero alpha");
                    core.Check(r, g, b, core.Maximum, "opaque alpha");
                }
            }
        }

        private static uint[] PremultiplicationAnchors(uint maximum)
        {
            var values = new HashSet<uint> { 0, 1, 2, 3, 17, 33, maximum / 2, maximum - 1, maximum };
            for (ulong power = 1; power <= maximum; power <<= 1)
            foreach (var value in new[] { power - 1, power, power + 1 })
                if (value <= maximum) values.Add((uint)value);
            return values.OrderBy(x => x).ToArray();
        }

        private sealed class PremultiplicationCore
        {
            public readonly string Name;
            public readonly uint Maximum;
            private readonly Type scalar;
            private readonly ConstructorInfo constructor;
            private readonly MethodInfo convert;
            private readonly FieldInfo[] channels;

            public PremultiplicationCore(Assembly assembly, string target, string suffix, Type scalar, uint maximum)
            {
                Name = target + "/C4" + suffix; Maximum = maximum; this.scalar = scalar;
                var color = assembly.GetType("Aardvark.Base.C4" + suffix, true);
                constructor = color.GetConstructor(Enumerable.Repeat(scalar, 4).ToArray());
                convert = assembly.GetType("Aardvark.Base.Col", true).GetMethod("AlphaToPremultipliedAlpha", new[] { color });
                channels = "RGBA".Select(c => color.GetField(c.ToString())).ToArray();
            }

            public uint[] Check(uint r, uint g, uint b, uint alpha, string label)
            {
                var input = new[] { r, g, b, alpha };
                var color = constructor.Invoke(input.Select(v => Convert.ChangeType(v, scalar)).ToArray());
                var result = convert.Invoke(null, new[] { color });
                var values = channels.Select(c => Convert.ToUInt32(c.GetValue(result))).ToArray();
                var context = $"{Name}: {label}, rgba=({r},{g},{b},{alpha})";
                for (var i = 0; i < 3; i++)
                    Assert.AreEqual((uint)((BigInteger)input[i] * alpha / Maximum), values[i], context + " channel=" + "RGB"[i]);
                Assert.AreEqual(alpha, values[3], context + " alpha preserved");
                Assert.That(channels.Select(c => Convert.ToUInt32(c.GetValue(color))), Is.EqualTo(input), context + " input preserved");
                return values;
            }
        }

        private static IEnumerable<PremultiplicationCore> PremultiplicationCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(C4b).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path);
                var assembly = Assembly.LoadFile(path);
                yield return new PremultiplicationCore(assembly, target, "b", typeof(byte), byte.MaxValue);
                yield return new PremultiplicationCore(assembly, target, "us", typeof(ushort), ushort.MaxValue);
                yield return new PremultiplicationCore(assembly, target, "ui", typeof(uint), uint.MaxValue);
            }
        }

        [Test]
        public void C4bArithmetic()
        {
            var c0 = new C4b(199, 203, 151, 130);
            var c1 = new C4b(35, 11, 66, 47);

            Assert.AreEqual( c0.ToV4i() + c1.ToV4i(),      (c0   + c1).ToV4i()   );
            Assert.AreEqual( c0.ToV4i() + c1.R,            (c0   + c1.R).ToV4i() );
            Assert.AreEqual( c1.R       + c0.ToV4i(),      (c1.R + c0).ToV4i()   );

            Assert.AreEqual( C3b.White.ToV4i(),            (c0   + c0).ToV4i()   );
            Assert.AreEqual( C3b.White.ToV4i(),            (c0   + c0.R).ToV4i() );
            Assert.AreEqual( C3b.White.ToV4i(),            (c0.R + c0).ToV4i()   );

            Assert.AreEqual( c0.ToV4i() - c1.ToV4i(),      (c0   - c1).ToV4i()   );
            Assert.AreEqual( c0.ToV4i() - c1.R,            (c0   - c1.R).ToV4i() );
            Assert.AreEqual( c0.R       - c1.ToV4i(),      (c0.R - c1).ToV4i()   );

            Assert.AreEqual( V4i.Zero,                     (c1   - c0).ToV4i()   );
            Assert.AreEqual( V4i.Zero,                     (c1   - c0.R).ToV4i() );
            Assert.AreEqual( V4i.Zero,                     (c1.R - c0).ToV4i()   );
        }
    }
}

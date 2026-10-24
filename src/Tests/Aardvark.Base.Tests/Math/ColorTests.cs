using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

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
        public void ChromaticHsvPreservesSaturationAcrossDarkScales()
        {
            foreach (var core in HsvCores())
            {
                var scales = Enumerable.Range(-149, 150).Select(e => MathF.ScaleB(1, e))
                    .Concat(new[] { 1e-8f, MathF.BitDecrement(MathF.ScaleB(1, -126)),
                        MathF.BitIncrement(MathF.ScaleB(1, -126)) });
                foreach (var v in scales)
                foreach (var rgb in new[] { new[] { v, 0f, 0f }, new[] { v, v, 0f },
                    new[] { v, v * 0.5f, 0f }, new[] { v, v * 0.75f, v * 0.25f } })
                    CheckHsvPermutations(core, rgb);

                var random = new Random(68831);
                for (var i = 0; i < 4096; i++)
                {
                    // Include dense subnormal encodings as well as normal RGB intensities.
                    var bits = i % 2 == 0 ? random.Next(1, 0x00800000)
                                         : random.Next(1, 0x3f800001);
                    var v = BitConverter.Int32BitsToSingle(bits);
                    CheckHsvPermutations(core, new[] { v, v * (float)random.NextDouble(),
                        v * (float)random.NextDouble() });
                }
            }
        }

        [Test]
        public void HsvSaturationUsesPositiveChromaAtFormerCutoff()
        {
            // The former cutoff was four float epsilons, not a relative color tolerance.
            var cutoff = MathF.ScaleB(1, -21);
            foreach (var core in HsvCores())
            {
                foreach (var cr in new[] { MathF.BitDecrement(cutoff), cutoff,
                    MathF.BitIncrement(cutoff) })
                {
                    CheckHsvPermutations(core, new[] { cr, 0f, 0f });
                    CheckHsvPermutations(core, new[] { cr, cr, 0f });
                    CheckHsvPermutations(core, new[] { cr, cr * 0.375f, 0f });
                }
                // At a nonzero offset, use adjacent representable differences around it.
                foreach (var steps in new[] { 7, 8, 9 })
                {
                    var max = 0.5f + steps * MathF.ScaleB(1, -24);
                    CheckHsvPermutations(core, new[] { max, 0.5f, 0.5f });
                    CheckHsvPermutations(core, new[] { max, max, 0.5f });
                }
            }
        }

        [Test]
        public void HsvKeepsBlackGrayscaleAndOrdinaryConversions()
        {
            foreach (var core in HsvCores())
            {
                foreach (var gray in new[] { 0f, float.Epsilon, 3 * float.Epsilon,
                    MathF.BitDecrement(MathF.ScaleB(1, -126)), MathF.ScaleB(1, -126),
                    1e-8f, MathF.ScaleB(1, -21), 0.125f, 0.5f, 1f })
                {
                    var result = core.Convert(gray, gray, gray);
                    var context = $"{core.Name}: gray {gray:R}";
                    Assert.AreEqual(0, BitConverter.SingleToInt32Bits(result[0]), context);
                    Assert.AreEqual(0, BitConverter.SingleToInt32Bits(result[1]), context);
                    CheckHsv(core, gray, gray, gray);
                }
                for (var mask = 0; mask < 8; mask++)
                {
                    var r = (mask & 1) == 0 ? 0f : -0f;
                    var g = (mask & 2) == 0 ? 0f : -0f;
                    var b = (mask & 4) == 0 ? 0f : -0f;
                    var result = core.Convert(r, g, b);
                    var context = $"{core.Name}: zero mask {mask}";
                    Assert.AreEqual(0, BitConverter.SingleToInt32Bits(result[0]), context);
                    Assert.AreEqual(0, BitConverter.SingleToInt32Bits(result[1]), context);
                    Assert.AreEqual(BitConverter.SingleToInt32Bits(b),
                        BitConverter.SingleToInt32Bits(result[2]), context);
                }
                var random = new Random(68839);
                for (var i = 0; i < 4096; i++)
                {
                    var rgb = new[] { (float)random.NextDouble(), (float)random.NextDouble(),
                        (float)random.NextDouble() };
                    CheckHsvPermutations(core, rgb);
                }
            }
        }

        private static void CheckHsvPermutations(
            (string Name, Func<float, float, float, float[]> Convert) core, float[] rgb)
        {
            for (var r = 0; r < 3; r++)
            for (var g = 0; g < 3; g++)
                if (r != g) CheckHsv(core, rgb[r], rgb[g], rgb[3 - r - g]);
        }

        private static void CheckHsv(
            (string Name, Func<float, float, float, float[]> Convert) core,
            float r, float g, float b)
        {
            const double u = 1.0 / (1 << 24);
            var result = core.Convert(r, g, b);
            var max = Math.Max((double)r, Math.Max((double)g, b));
            var min = Math.Min((double)r, Math.Min((double)g, b));
            var saturation = max == 0 ? 0 : (max - min) / max;
            var context = $"{core.Name}: RGB({r:R}, {g:R}, {b:R})";
            Assert.AreEqual(saturation, result[1], 2 * u * saturation, context + " saturation");
            Assert.AreEqual(BitConverter.SingleToInt32Bits((float)max),
                BitConverter.SingleToInt32Bits(result[2]), context + " value");
            // Float hue/sector arithmetic scales its error with the color, not with one.
            var tolerance = 8 * u * max + 2.0 * float.Epsilon;
            Assert.AreEqual(r, result[3], tolerance, context + " round-trip R");
            Assert.AreEqual(g, result[4], tolerance, context + " round-trip G");
            Assert.AreEqual(b, result[5], tolerance, context + " round-trip B");
        }

        private static IEnumerable<(string Name, Func<float, float, float, float[]> Convert)> HsvCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(C3f).Assembly.Location),
                    "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path);
                var assembly = Assembly.LoadFile(path);
                var rgbType = assembly.GetType("Aardvark.Base.C3f", true);
                var hsvType = assembly.GetType("Aardvark.Base.HSVf", true);
                var col = assembly.GetType("Aardvark.Base.Col", true);
                var r = Expression.Parameter(typeof(float));
                var g = Expression.Parameter(typeof(float));
                var b = Expression.Parameter(typeof(float));
                var hsv = Expression.Variable(hsvType);
                var rgb = Expression.Variable(rgbType);
                var body = Expression.Block(new[] { hsv, rgb },
                    Expression.Assign(hsv, Expression.Call(col.GetMethod("ToHSVf", new[] { rgbType }),
                        Expression.New(rgbType.GetConstructor(new[] { typeof(float), typeof(float), typeof(float) }),
                            r, g, b))),
                    Expression.Assign(rgb, Expression.Call(col.GetMethod("ToC3f", new[] { hsvType }), hsv)),
                    Expression.NewArrayInit(typeof(float),
                        Expression.Field(hsv, "H"), Expression.Field(hsv, "S"), Expression.Field(hsv, "V"),
                        Expression.Field(rgb, "R"), Expression.Field(rgb, "G"), Expression.Field(rgb, "B")));
                yield return (target, Expression.Lambda<Func<float, float, float, float[]>>(body, r, g, b).Compile());
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

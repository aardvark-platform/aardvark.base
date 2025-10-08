using System.Linq;
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

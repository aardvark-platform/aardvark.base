using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}

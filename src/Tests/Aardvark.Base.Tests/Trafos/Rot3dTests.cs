using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class Rot3dTests
    {
        [Test]
        public static void FromM33d()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 1000000; i++)
            {
                var rot = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var mat = M44d.Rotation(rot);
                var q = Rot3d.FromM33d((M33d)mat); // TODO tweak default epsilon
                //var q = Rot3d.FromM33d((M33d)mat, 1e-16); // this epsilon work...

                var mat2 = (M44d)q;

                Assert.IsFalse(mat.Elements.Any(x => x.IsNaN()), "NaN");

                if (!Fun.ApproximateEquals(mat, mat2, 1e-8))
                    Assert.Fail("FAIL");
            }
        }
    }
}

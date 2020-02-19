using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class SimilarityTests
    {
        [Test]
        public static void InverseTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 1000000; i++)
            {
                var scale = rnd.UniformDouble() * 5;
                var rotation = Rot3d.Rotation(rnd.UniformV3dDirection(), rnd.UniformDouble() * Constant.PiTimesTwo);
                var translation = rnd.UniformV3d() * 10;
                
                var s = new Similarity3d(scale, new Euclidean3d(rotation, translation));

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = s.TransformPos(p);

                // Inverse property
                var res = s.Inverse.TransformPos(q);

                // Invert method
                Similarity.Invert(ref s);
                var res2 = s.TransformPos(q);

                Assert.IsTrue(Fun.ApproximateEquals(p, res, 0.00001), "{0} != {1}", p, res);
                Assert.IsTrue(Fun.ApproximateEquals(p, res2, 0.00001), "{0} != {1}", p, res2);
            }
        }
    }
}

using Aardvark.Base;
using Aardvark.Base.Rendering;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class SamplerStateTest : TestSuite
    {
        public SamplerStateTest() : base() { }

        [Test]
        public void SamplerStateTestHashCollision()
        {
            var samA = new SamplerStateDescription() { Filter = TextureFilter.MinMagLinearMipPoint };
            var samB = new SamplerStateDescription() { Filter = TextureFilter.MinMagLinear };

            var hashA = samA.GetHashCode();
            var hashB = samB.GetHashCode();

            Report.Line("SamA={0}, SamB={1}", samA, samB);
            Report.Line("HashA={0}, HashB={1}", hashA, hashB);

            Report.Line("Equal={0}", Object.Equals(samA, samB));

            if (hashA == hashB)
                Report.Line("BAD HASH :(");
            else
                Report.Line("Good Hash :)");
        }
    }
}

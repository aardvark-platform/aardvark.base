using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Aardvark.Base;

namespace Aardvark.Tests.Images
{
    [TestFixture]
    class PixImageTests
    {
        [Test]
        public void MipMapCreate1x1()
        {
            var pix = new PixImage<byte>(1, 1, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 1);
        }

        [Test]
        public void MipMapCreate2x2()
        {
            var pix = new PixImage<byte>(2, 2, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 2);
        }

        [Test]
        public void MipMapCreate3x3()
        {
            var pix = new PixImage<byte>(3, 3, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 2);
        }

        [Test]
        public void MipMapCreate256()
        {
            var pix = new PixImage<byte>(256, 256, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 9);
        }

        [Test]
        public void MipMapCreate255()
        {
            var pix = new PixImage<byte>(255, 255, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 8);
        }

        [Test]
        public void MipMapCreate257()
        {
            var pix = new PixImage<byte>(257, 257, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 9);
        }

        [Test]
        public void MipMapCreate57x43()
        {
            var pix = new PixImage<byte>(57, 43, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 6);
            //level 0: 57x43
            //level 1: 28x21
            //level 2: 14x10
            //level 3: 7x5
            //level 4: 3x2
            //level 5: 1x1
        }

        [Test]
        public void MipMapCreate57x11()
        {
            var pix = new PixImage<byte>(57, 11, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 6);
            //level 0: 57x11
            //level 1: 28x5
            //level 2: 14x2
            //level 3: 7x1
            //level 4: 3x1
            //level 5: 1x1
        }
    }
}

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
        static Volume<byte> Scaled(Volume<byte> source, V2d scaleFactor, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            var targetSize = (new V2d(0.5, 0.5) + scaleFactor * (V2d)source.Size.XY).ToV2l();
            var target = new V3l(targetSize.X, targetSize.Y, source.Size.Z).CreateImageVolume<byte>();
            for (int c = 0; c < source.Size.Z; c++)
            {
                var targetMat = target.SubXYMatrixWindow(0);
                var sourceMat = source.SubXYMatrixWindow(0);
                switch(ip)
                {
                    case ImageInterpolation.Near: targetMat.SetScaledNearest(sourceMat); break;
                    case ImageInterpolation.Linear:
                        targetMat.SetScaledLinear(sourceMat, (s, a, b) => (byte)s.Lerp(a, b), (s, a, b) => (byte)s.Lerp(a, b));
                        break;
                    case ImageInterpolation.Cubic: targetMat.SetScaledCubic(sourceMat); break;
                    case ImageInterpolation.Lanczos: targetMat.SetScaledLanczos(sourceMat); break;
                    default: throw new NotImplementedException();
                }
            }
            return target;
        }

        static void Init()
        {
            PixImage<byte>.SetScaledFun(Scaled);
            //PixImage<byte>.SetScaledFun(Aardvark.VRVis.TensorExtensions.Scaled);
        }

        [Test]
        public void MipMapCreate1x1()
        {
            Init();
            var pix = new PixImage<byte>(1, 1, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 1);
        }

        [Test]
        public void MipMapCreate2x2()
        {
            Init();
            var pix = new PixImage<byte>(2, 2, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 2);
        }

        [Test]
        public void MipMapCreate3x3()
        {
            Init();
            var pix = new PixImage<byte>(3, 3, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 2);
        }

        [Test]
        public void MipMapCreate256()
        {
            Init();
            var pix = new PixImage<byte>(256, 256, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 9);
        }

        [Test]
        public void MipMapCreate255()
        {
            Init();
            var pix = new PixImage<byte>(255, 255, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 8);
        }

        [Test]
        public void MipMapCreate257()
        {
            Init();
            var pix = new PixImage<byte>(257, 257, 4);
            var mip = PixImageMipMap.Create(pix);
            Assert.IsTrue(mip.LevelCount == 9);
        }

        [Test]
        public void MipMapCreate57x43()
        {
            Init();
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
            Init();
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

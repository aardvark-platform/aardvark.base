using NUnit.Framework;
using System;
using Aardvark.Base;

namespace Aardvark.Tests.Images
{
    [TestFixture]
    public class PixSaveParamsTests
    {
        [TestCase(-1)]
        [TestCase(101)]
        public void InvalidJpegQualityReportsNumericValue(int quality)
        {
            var e = Assert.Throws<ArgumentException>(() => new PixJpegSaveParams(quality));
            Assert.AreEqual($"Quality must be within 0 - 100, is {quality}.", e.Message);
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void InvalidWebpQualityReportsNumericValue(int quality)
        {
            var e = Assert.Throws<ArgumentException>(() => new PixWebpSaveParams(quality));
            Assert.AreEqual($"Quality must be within 0 - 100, is {quality}.", e.Message);
        }

        [TestCase(-1)]
        [TestCase(10)]
        public void InvalidPngCompressionLevelReportsNumericValue(int compressionLevel)
        {
            var e = Assert.Throws<ArgumentException>(() => new PixPngSaveParams(compressionLevel));
            Assert.AreEqual($"Compression level must be within 0 - 9, is {compressionLevel}.", e.Message);
        }

        [TestCase(0)]
        [TestCase(100)]
        public void JpegQualityBoundariesAreValid(int quality)
        {
            var p = new PixJpegSaveParams(quality);

            Assert.AreEqual(PixFileFormat.Jpeg, p.Format);
            Assert.AreEqual(quality, p.Quality);
        }

        [TestCase(0)]
        [TestCase(100)]
        public void WebpQualityBoundariesAreValid(int quality)
        {
            var p = new PixWebpSaveParams(quality);

            Assert.AreEqual(PixFileFormat.Webp, p.Format);
            Assert.AreEqual(quality, p.Quality);
            Assert.IsFalse(p.Lossless);
        }

        [TestCase(0)]
        [TestCase(9)]
        public void PngCompressionLevelBoundariesAreValid(int compressionLevel)
        {
            var p = new PixPngSaveParams(compressionLevel);

            Assert.AreEqual(PixFileFormat.Png, p.Format);
            Assert.AreEqual(compressionLevel, p.CompressionLevel);
        }
    }
}

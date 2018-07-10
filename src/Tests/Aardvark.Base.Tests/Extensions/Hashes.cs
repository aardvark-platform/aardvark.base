using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Extensions
{
    public static class Hashes
    {
        [Test]
        public static void ArrayHashExtensions()
        {
            var floats = new float[10].SetByIndex(i => i);

            var md5 = floats.ComputeMD5Hash();
            Report.Line("MD5: {0}", md5.ToHex());
            Assert.IsTrue(md5.Length == 16);

            var sha1 = floats.ComputeSHA1Hash();
            Report.Line("SHA1: {0}", sha1.ToHex());
            Assert.IsTrue(sha1.Length == 20);

            var sha256 = floats.ComputeSHA256Hash();
            Report.Line("SHA256: {0}", sha256.ToHex());
            Assert.IsTrue(sha256.Length == 32);

            var sha512 = floats.ComputeSHA512Hash();
            Report.Line("SHA512: {0}", sha512.ToHex());
            Assert.IsTrue(sha512.Length == 64);

            var adler = floats.ComputeAdler32Checksum();
            Report.Line("Checksum: {0:X}", adler);

            floats[9] = 10;

            var md5New = floats.ComputeMD5Hash();
            Report.Line("MD5: {0}", md5New.ToHex());
            Assert.True(md5New.ToHex() != md5.ToHex());

            var sha1New = floats.ComputeSHA1Hash();
            Report.Line("SHA1: {0}", sha1New.ToHex());
            Assert.True(sha1New.ToHex() != sha1.ToHex());

            var sha256New = floats.ComputeSHA256Hash();
            Report.Line("SHA256: {0}", sha256New.ToHex());
            Assert.True(sha256New.ToHex() != sha256.ToHex());

            var sha512New = floats.ComputeSHA512Hash();
            Report.Line("SHA512: {0}", sha512New.ToHex());
            Assert.True(sha512New.ToHex() != sha512.ToHex());

            var adlerNew = floats.ComputeAdler32Checksum();
            Report.Line("Checksum: {0:X}", adlerNew);
            Assert.True(adlerNew != adler);
        }

        [Test]
        public static void ArrayHashPerformance()
        {
            var mb = 256;
            var huge = new byte[(1 << 20) * mb].SetByIndex(i => (byte)((i * 7L) & 0xff));

            Report.BeginTimed("Computing MD5 Hash of {0}MB data array", mb);
            huge.ComputeMD5Hash();
            Report.End();

            Report.BeginTimed("Computing SHA1 Hash of {0}MB data array", mb);
            huge.ComputeSHA1Hash();
            Report.End();

            Report.BeginTimed("Computing SHA256 Hash of {0}MB data array", mb);
            huge.ComputeSHA256Hash();
            Report.End();

            Report.BeginTimed("Computing SHA512 Hash of {0}MB data array", mb);
            huge.ComputeSHA512Hash();
            Report.End();

            Report.BeginTimed("Computing Adler-32 Checksum of {0}MB data array", mb);
            huge.ComputeAdler32Checksum();
            Report.End();
        }
    }
}

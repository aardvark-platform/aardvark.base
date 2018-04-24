using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}

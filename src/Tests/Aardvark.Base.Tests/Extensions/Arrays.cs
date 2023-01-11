using NUnit.Framework;
using System;
using Aardvark.Base;

namespace Aardvark.Tests.Extensions
{
    static class Arrays
    {
        [Test]
        public unsafe static void CopyArrayToNative()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[3];

            fixed (ushort* ptr = dst)
            {
                src.CopyTo(1, 3, (IntPtr)ptr);
            }

            Assert.AreEqual(src[1], dst[0]);
            Assert.AreEqual(src[2], dst[1]);
            Assert.AreEqual(src[3], dst[2]);
        }

        [Test]
        public unsafe static void CopyNativeToArray()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[5];

            fixed (ushort* ptr = src)
            {
                ((IntPtr)ptr).CopyTo(dst, 1, 3);
            }

            Assert.AreEqual(src[0], dst[1]);
            Assert.AreEqual(src[1], dst[2]);
            Assert.AreEqual(src[2], dst[3]);
        }

        [Test]
        public unsafe static void CopyNativeToNative()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[src.Length];

            fixed (ushort* srcPtr = src, dstPtr = dst)
            {
                ((IntPtr)srcPtr).CopyTo((IntPtr)dstPtr, sizeof(ushort) * src.Length);
            }

            for (int i = 0; i < src.Length; i++)
                Assert.AreEqual(src[i], dst[i]);
        }
    }
}

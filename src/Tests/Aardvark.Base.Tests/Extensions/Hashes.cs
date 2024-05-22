using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Aardvark.Tests.Extensions
{
    public static class Hashes
    {
        [Test]
        public static void ArrayHashExtensions()
        {
            var floats = new float[10].SetByIndex(i => i);


            var inty = new int[20].SetByIndex(i => i);
            var v2iy = new V2i[10].SetByIndex(i => new V2i(2 * i, 2 * i + 1));

            var intHash = inty.ComputeSHA1Hash().ToHex();
            var v2iHash = v2iy.ComputeSHA1Hash().ToHex();
            
            Assert.IsTrue(intHash == v2iHash);
            
            
            
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

        /// <summary>
        /// Would be nice utils but are not .net standard compatible
        /// </summary>
        public static class CLRUtils
        {
            public static int SizeOf<T>(T obj)
            {
                return SizeOfCache<T>.SizeOf;
            }

            private static class SizeOfCache<T>
            {
                public static readonly int SizeOf;

                static SizeOfCache()
                {
                    var dm = new DynamicMethod("func", typeof(int),
                                               Type.EmptyTypes, typeof(CLRUtils));

                    ILGenerator il = dm.GetILGenerator();
                    il.Emit(OpCodes.Sizeof, typeof(T));
                    il.Emit(OpCodes.Ret);

                    var func = (Func<int>)dm.CreateDelegate(typeof(Func<int>));
                    SizeOf = func();
                }
            }

            private static Dictionary<Type, int> _sizeCache = new Dictionary<Type, int>();
            public static int SizeOf(Type type)
            {
                return _sizeCache.GetCreate(type, t =>
                {
                    var dm = new DynamicMethod("func", typeof(int),
                                                   Type.EmptyTypes, typeof(CLRUtils));

                    ILGenerator il = dm.GetILGenerator();
                    il.Emit(OpCodes.Sizeof, t);
                    il.Emit(OpCodes.Ret);

                    var func = (Func<int>)dm.CreateDelegate(typeof(Func<int>));
                    return func();
                });
            }
        }

        [Test]
        public static void HashEquality()
        {   // -> scroll                                                                                                                                                                                                                                                                   //######################//
            var a = "PFDMHE3qf5BD2GVXHSb7Pw==Xu6fEt3zjgNOhgcgU6P3VA==hrptgTUY1q95BRjYpBlcuA==rKx+IsdlT3wMNiC1su/Ciw==yWz34lIfOgRXNbbCM2Xg6Q==OkYVqxAg1+r/xOoNgTzO5w==jHm9kCMmSmFskgSQYae3EA==9osN2zSiH4XFSOdNoQq7Yg==xhvrPfi5l7GYy0Ka4pwp7w==aWze5ohFCFkxZMZPqTLfsA==iUU+2nRevMAYlHQgrjKfyQ==74FhaizLzpPAPHZ0h/Emsw==KbzIWiTHRlFG5Whs3YYzyA==M/G8RpD5bp9kRKLDFHy9qg==Wv+3r9IY0lfPq+LUAtNPsQ==j8/23gUX4FzrEOPzcpAhcA==";
            var b = "PFDMHE3qf5BD2GVXHSb7Pw==Xu6fEt3zjgNOhgcgU6P3VA==hrptgTUY1q95BRjYpBlcuA==rKx+IsdlT3wMNiC1su/Ciw==yWz34lIfOgRXNbbCM2Xg6Q==OkYVqxAg1+r/xOoNgTzO5w==jHm9kCMmSmFskgSQYae3EA==9osN2zSiH4XFSOdNoQq7Yg==xhvrPfi5l7GYy0Ka4pwp7w==aWze5ohFCFkxZMZPqTLfsA==iUU+2nRevMAYlHQgrjKfyQ==dqrZpWI5/PzQhFpHLVPaKw==KbzIWiTHRlFG5Whs3YYzyA==M/G8RpD5bp9kRKLDFHy9qg==Wv+3r9IY0lfPq+LUAtNPsQ==j8/23gUX4FzrEOPzcpAhcA==";

            var arrayA = a.ToArray();
            var arrayB = b.ToArray();

            var md5A = arrayA.ComputeMD5Hash().ToHex();
            var md5B = arrayB.ComputeMD5Hash().ToHex();

            if (md5A == md5B)
                Assert.Fail("MD5 BAD -> actually UnsafeCoerce char to byte fails due to Marshal.SizeOf != sizeof()");
            
            var etA = arrayA.GetType().GetElementType();
            var clrSizeA = CLRUtils.SizeOf(etA); // 2
            var marshalSizeA = Marshal.SizeOf(etA); // 1
            
            var bytesA = arrayA.Map(x => (short)x);
            var bytesB = arrayB.Map(x => (short)x);

            var byteHashA = bytesA.ComputeMD5Hash().ToHex();
            var byteHashB = bytesB.ComputeMD5Hash().ToHex();

            if (byteHashA == byteHashB)
                Assert.Fail("MD5 REAL BAD");

            if (byteHashA != md5A)
                Assert.Fail("HASH A BAD");

            if (byteHashB != md5B)
                Assert.Fail("HASH B BAD");
        }

        public struct MyStruct1
        {
            public bool fun1;
            public bool fun2;
        }

        public struct MyStruct2
        {
            public char fun1;
            public char fun2;
        }

        public struct MyStruct3
        {
            public bool fun1;
            public char fun2;
        }

        public struct MyStruct4
        {
            public MyStruct2 fun1;
            public MyStruct1 fun2;
        }

        public static void SizeOfTest()
        {
            TestSize<byte>();
            TestSize<short>();;
            TestSize<int>();
            TestSize<long>();
            TestSize<char>();
            TestSize<float>();
            TestSize<double>();
            TestSize<decimal>();
            TestSize<bool>();
            TestSize<MyStruct1>();
            TestSize<MyStruct2>();
            TestSize<MyStruct3>();
            TestSize<MyStruct4>();
        }

        static void TestSize<T>()
        {
            var clrSize = CLRUtils.SizeOf(typeof(T));
            var marshalSize = Marshal.SizeOf<T>();
            if (clrSize != marshalSize)
                Report.Line("{0} size {1} != {2}", typeof(T).Name, clrSize, marshalSize);
        }
    }
}

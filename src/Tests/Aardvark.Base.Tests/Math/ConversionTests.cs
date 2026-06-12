using System;
using System.Linq;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ConversionTests : TestSuite
    {
        public ConversionTests() : base() { }
        public ConversionTests(TestSuite.Options options) : base(options) { }

        private static byte[] WithTrailingBytes(byte[] data)
        {
            return data.Concat(new byte[] { 0xAA, 0xBB, 0xCC }).ToArray();
        }

        [Test]
        public void NetworkToHostOrderInt32ReadsOnlyPrimitivePrefix()
        {
            const int value = unchecked((int)0x89ABCDEF);
            var exact = Conversion.HostToNetworkOrder(value);
            var oversized = WithTrailingBytes(exact);

            Assert.AreEqual(value, Conversion.NetworkToHostOrderInt32(exact));
            Assert.AreEqual(value, Conversion.NetworkToHostOrderInt32(oversized));
            CollectionAssert.AreEqual(WithTrailingBytes(exact), oversized);
        }

        [Test]
        public void NetworkToHostOrderUInt64ReadsOnlyPrimitivePrefix()
        {
            const ulong value = 0x0123456789ABCDEFUL;
            var exact = Conversion.HostToNetworkOrder(value);
            var oversized = WithTrailingBytes(exact);

            Assert.AreEqual(value, Conversion.NetworkToHostOrderUInt64(exact));
            Assert.AreEqual(value, Conversion.NetworkToHostOrderUInt64(oversized));
            CollectionAssert.AreEqual(WithTrailingBytes(exact), oversized);
        }

        [Test]
        public void NetworkToHostOrderSingleReadsOnlyPrimitivePrefix()
        {
            const float value = 123.5f;
            var exact = Conversion.HostToNetworkOrder(value);
            var oversized = WithTrailingBytes(exact);

            Assert.AreEqual(value, Conversion.NetworkToHostOrderSingle(exact));
            Assert.AreEqual(value, Conversion.NetworkToHostOrderSingle(oversized));
            CollectionAssert.AreEqual(WithTrailingBytes(exact), oversized);
        }

        [Test]
        public void NetworkToHostOrderDoubleReadsOnlyPrimitivePrefix()
        {
            const double value = -9876.25;
            var exact = Conversion.HostToNetworkOrder(value);
            var oversized = WithTrailingBytes(exact);

            Assert.AreEqual(value, Conversion.NetworkToHostOrderDouble(exact));
            Assert.AreEqual(value, Conversion.NetworkToHostOrderDouble(oversized));
            CollectionAssert.AreEqual(WithTrailingBytes(exact), oversized);
        }

        [Test]
        public void NetworkToHostOrderInPlaceLeavesTrailingBytesUnchanged()
        {
            const int value = 0x01234567;
            var exact = Conversion.HostToNetworkOrder(value);
            var oversized = WithTrailingBytes(exact);
            var trailingBytes = oversized.Skip(sizeof(int)).ToArray();

            Assert.AreEqual(value, Conversion.NetworkToHostOrderInt32InPlace(oversized));
            CollectionAssert.AreEqual(trailingBytes, oversized.Skip(sizeof(int)).ToArray());
        }
    }
}

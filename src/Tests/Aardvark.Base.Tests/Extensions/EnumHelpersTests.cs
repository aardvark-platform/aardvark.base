using System;
using System.Threading;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Extensions
{
    static class EnumHelpersTests
    {
        private enum ByteEnum : byte
        {
            Zero = 0,
            Max = byte.MaxValue,
        }

        private enum SByteEnum : sbyte
        {
            Zero = 0,
            Min = sbyte.MinValue,
            NegativeOne = -1,
        }

        private enum ShortEnum : short
        {
            Zero = 0,
            Min = short.MinValue,
            NegativeOne = -1,
        }

        private enum UShortEnum : ushort
        {
            Zero = 0,
            Max = ushort.MaxValue,
        }

        private enum IntEnum : int
        {
            Zero = 0,
            Min = int.MinValue,
            NegativeOne = -1,
        }

        private enum UIntEnum : uint
        {
            Zero = 0,
            Max = uint.MaxValue,
        }

        private enum SignedEnum : long
        {
            Zero = 0,
            One = 1,
            Min = long.MinValue,
            NegativeOne = -1,
        }

        private enum UnsignedEnum : ulong
        {
            Zero = 0,
            High = 1UL << 63,
            Max = ulong.MaxValue,
        }

        private enum AliasedEnum
        {
            Zero = 0,
            One = 1,
            AlsoOne = 1,
            Two = 2,
        }

        private enum ConcurrentFirstUseEnum : ulong
        {
            Zero = 0,
            High = 1UL << 63,
            Max = ulong.MaxValue,
        }

        [Test]
        public static void SupportsEveryUnderlyingIntegralTypeLosslessly()
        {
            Assert.AreEqual(1, EnumHelpers.GetIndex(ByteEnum.Max));
            Assert.AreEqual(1, EnumHelpers.GetIndex(SByteEnum.Min));
            Assert.AreEqual(2, EnumHelpers.GetIndex(SByteEnum.NegativeOne));
            Assert.AreEqual(1, EnumHelpers.GetIndex(ShortEnum.Min));
            Assert.AreEqual(2, EnumHelpers.GetIndex(ShortEnum.NegativeOne));
            Assert.AreEqual(1, EnumHelpers.GetIndex(UShortEnum.Max));
            Assert.AreEqual(1, EnumHelpers.GetIndex(IntEnum.Min));
            Assert.AreEqual(2, EnumHelpers.GetIndex(IntEnum.NegativeOne));
            Assert.AreEqual(1, EnumHelpers.GetIndex(UIntEnum.Max));
            Assert.AreEqual(2, EnumHelpers.GetIndex(SignedEnum.Min));
            Assert.AreEqual(3, EnumHelpers.GetIndex(SignedEnum.NegativeOne));
            Assert.AreEqual(2, EnumHelpers.GetIndex(UnsignedEnum.Max));
        }

        [Test]
        public static void SignedValuesFollowEnumGetValuesOrderAndWrap()
        {
            Assert.AreEqual(0, EnumHelpers.GetIndex(SignedEnum.Zero));
            Assert.AreEqual(1, EnumHelpers.GetIndex(SignedEnum.One));
            Assert.AreEqual(2, EnumHelpers.GetIndex(SignedEnum.Min));
            Assert.AreEqual(3, EnumHelpers.GetIndex(SignedEnum.NegativeOne));

            Assert.AreEqual(SignedEnum.NegativeOne, EnumHelpers.GetPrevValue(SignedEnum.Zero));
            Assert.AreEqual(SignedEnum.Zero, EnumHelpers.GetNextValue(SignedEnum.NegativeOne));
            Assert.AreEqual(SignedEnum.Min, EnumHelpers.GetNextValue(SignedEnum.One));
        }

        [Test]
        public static void ULongValuesAboveInt64MaxSupportLookupAndWrap()
        {
            Assert.AreEqual(1, EnumHelpers.GetIndex(UnsignedEnum.High));
            Assert.AreEqual(2, EnumHelpers.GetIndex(typeof(UnsignedEnum), UnsignedEnum.Max));
            Assert.AreEqual(UnsignedEnum.Max, EnumHelpers.GetValue<UnsignedEnum>(2));
            Assert.AreEqual(UnsignedEnum.Max, EnumHelpers.GetPrevValue(UnsignedEnum.Zero));
            Assert.AreEqual(UnsignedEnum.Zero, EnumHelpers.GetNextValue(UnsignedEnum.Max));
        }

        [Test]
        public static void AliasesShareOneIndexAndTraversalPosition()
        {
            Assert.AreEqual(1, EnumHelpers.GetIndex(AliasedEnum.One));
            Assert.AreEqual(1, EnumHelpers.GetIndex(AliasedEnum.AlsoOne));
            Assert.AreEqual(1, EnumHelpers.GetIndex(typeof(AliasedEnum), AliasedEnum.AlsoOne));
            Assert.AreEqual(2, EnumHelpers.GetIndex(AliasedEnum.Two));

            Assert.AreEqual(AliasedEnum.One, EnumHelpers.GetValue<AliasedEnum>(1));
            Assert.AreEqual(2, EnumHelpers.GetValue(typeof(AliasedEnum), 2));
            Assert.AreEqual(AliasedEnum.Two, EnumHelpers.GetNextValue(AliasedEnum.AlsoOne));
            Assert.AreEqual(AliasedEnum.One, EnumHelpers.GetPrevValue(AliasedEnum.Two));
        }

        [Test]
        public static void ConcurrentFirstUseBuildsConsistentMetadata()
        {
            const int threadCount = 16;
            var errors = new Exception[threadCount];
            using (var barrier = new Barrier(threadCount + 1))
            {
                var threads = new Thread[threadCount];
                for (var i = 0; i < threads.Length; i++)
                {
                    var index = i;
                    threads[i] = new Thread(() =>
                    {
                        try
                        {
                            barrier.SignalAndWait();
                            for (var iteration = 0; iteration < 100; iteration++)
                            {
                                if (EnumHelpers.GetIndex(ConcurrentFirstUseEnum.Max) != 2)
                                    throw new InvalidOperationException("Generic index lookup was inconsistent.");
                                if (EnumHelpers.GetIndex(typeof(ConcurrentFirstUseEnum), ConcurrentFirstUseEnum.High) != 1)
                                    throw new InvalidOperationException("Non-generic index lookup was inconsistent.");
                                if (EnumHelpers.GetValue<ConcurrentFirstUseEnum>(1) != ConcurrentFirstUseEnum.High)
                                    throw new InvalidOperationException("Value lookup was inconsistent.");
                                if (EnumHelpers.GetPrevValue(ConcurrentFirstUseEnum.Zero) != ConcurrentFirstUseEnum.Max)
                                    throw new InvalidOperationException("Previous-value lookup was inconsistent.");
                                if (EnumHelpers.GetNextValue(ConcurrentFirstUseEnum.Max) != ConcurrentFirstUseEnum.Zero)
                                    throw new InvalidOperationException("Next-value lookup was inconsistent.");
                            }
                        }
                        catch (Exception e)
                        {
                            errors[index] = e;
                        }
                    });
                    threads[i].Start();
                }

                barrier.SignalAndWait();
                foreach (var thread in threads) thread.Join();
            }

            foreach (var error in errors)
                Assert.IsNull(error, error?.ToString());
        }
    }
}

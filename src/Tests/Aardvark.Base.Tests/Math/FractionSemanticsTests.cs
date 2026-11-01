using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FractionSemanticsTests
    {
        [Test]
        public void UnreducedFiniteValuesUseNumericalEqualityAndHashing()
        {
            var half = new Fraction(1, 2);
            var equivalent = new Fraction(3, 6);
            var negativeHalf = new Fraction(-5, 10);
            var zeroA = new Fraction(0, 7);
            var zeroB = new Fraction(0, long.MaxValue);

            Assert.IsTrue(half == equivalent);
            Assert.IsFalse(half != equivalent);
            Assert.IsTrue(half.Equals(equivalent));
            Assert.AreEqual(half.GetHashCode(), equivalent.GetHashCode());

            Assert.IsTrue(half != negativeHalf);
            Assert.IsTrue(negativeHalf < half);
            Assert.IsTrue(zeroA == zeroB);
            Assert.IsTrue(zeroA.Equals(zeroB));
            Assert.AreEqual(zeroA.GetHashCode(), zeroB.GetHashCode());
            Assert.IsTrue(typeof(IEquatable<Fraction>).IsAssignableFrom(typeof(Fraction)));
        }

        [Test]
        public void HashSetUsesNumericalAndCollectionEquality()
        {
            var values = new HashSet<Fraction>
            {
                new Fraction(1, 2),
                new Fraction(2, 4),
                new Fraction(0, 3),
                new Fraction(0, 17),
                Fraction.NaN,
                new Fraction(0, 0),
                Fraction.PositiveInfinity,
                new Fraction(19, 0),
                Fraction.NegativeInfinity,
                new Fraction(-23, 0),
            };

            Assert.AreEqual(5, values.Count);
            Assert.IsTrue(values.Contains(new Fraction(17, 34)));
            Assert.IsTrue(values.Contains(Fraction.NaN));
            Assert.IsTrue(values.Contains(new Fraction(123, 0)));
            Assert.IsTrue(values.Contains(new Fraction(-123, 0)));
        }

        [Test]
        public void RoundedDoubleTiesUseExactOrdering()
        {
            var integer = new Fraction(9_007_199_254_740_992L, 1);
            var nextInteger = new Fraction(9_007_199_254_740_993L, 1);
            Assert.AreEqual(integer.Value, nextInteger.Value);
            AssertExactOrder(integer, nextInteger, -1);

            long max = long.MaxValue;
            var a = new Fraction(max, max - 1);
            var b = new Fraction(max - 1, max - 2);
            Assert.AreEqual(a.Value, b.Value);
            AssertExactOrder(a, b, -1);

            var negativeA = new Fraction(-max, max - 1);
            var negativeB = new Fraction(-(max - 1), max - 2);
            Assert.AreEqual(negativeA.Value, negativeB.Value);
            AssertExactOrder(negativeA, negativeB, 1);
        }

        [Test]
        public void RandomFiniteComparisonsMatchBigIntegerReference()
        {
            ulong state = 0xd1b54a32d192ed03UL;
            for (int i = 0; i < 25_000; i++)
            {
                long aNumerator = unchecked((long)Next(ref state));
                long bNumerator = unchecked((long)Next(ref state));
                long aDenominator = (long)(Next(ref state) & long.MaxValue);
                long bDenominator = (long)(Next(ref state) & long.MaxValue);
                if (aDenominator == 0) aDenominator = 1;
                if (bDenominator == 0) bDenominator = 1;

                var a = new Fraction(aNumerator, aDenominator);
                var b = new Fraction(bNumerator, bDenominator);
                int expected = System.Math.Sign(
                    ((BigInteger)a.Numerator * b.Denominator)
                    .CompareTo((BigInteger)b.Numerator * a.Denominator));

                AssertExactOrder(a, b, expected);
            }
        }

        [Test]
        public void ExtremeRoundedTiesMatchBigIntegerReference()
        {
            ulong state = 0x94d049bb133111ebUL;
            for (int i = 0; i < 2_000; i++)
            {
                long aDenominator = (long.MaxValue - 2048 - (long)(Next(ref state) & 0xfffffUL)) & ~2047L;
                long bDenominator = (long.MaxValue - 2048 - (long)(Next(ref state) & 0xfffffUL)) & ~2047L;
                long aNumerator = aDenominator + 1 + (long)(Next(ref state) & 31UL);
                long bNumerator = bDenominator + 1 + (long)(Next(ref state) & 31UL);
                var a = new Fraction(aNumerator, aDenominator);
                var b = new Fraction(bNumerator, bDenominator);

                Assert.AreEqual(a.Value, b.Value);
                int expected = System.Math.Sign(
                    ((BigInteger)a.Numerator * b.Denominator)
                    .CompareTo((BigInteger)b.Numerator * a.Denominator));
                AssertExactOrder(a, b, expected);
            }
        }

        [Test]
        public void LongExtremeStoredRepresentationsRemainExact()
        {
            var values = new[]
            {
                Raw(long.MinValue, 1),
                Raw(long.MaxValue, 1),
                Raw(long.MinValue, -1),
                Raw(long.MinValue, long.MinValue),
                Raw(1, long.MinValue),
                Raw(-1, long.MinValue),
                Raw(0, long.MinValue),
            };

            foreach (var a in values)
            {
                var reduced = a.Reduced;
                AssertExactOrder(a, reduced, 0);
                foreach (var b in values)
                    AssertExactOrder(a, b, ReferenceOrder(a, b));
            }
        }

        [Test]
        public void NaNAndInfinityPredicatesAreMutuallyCorrect()
        {
            var positive = new Fraction(7, 0);
            var negative = new Fraction(-7, 0);

            Assert.IsTrue(Fraction.IsNaN(Fraction.NaN));
            Assert.IsFalse(Fraction.IsInfinity(Fraction.NaN));
            Assert.IsFalse(Fraction.IsPositiveInfinity(Fraction.NaN));
            Assert.IsFalse(Fraction.IsNegativeInfinity(Fraction.NaN));

            Assert.IsFalse(Fraction.IsNaN(positive));
            Assert.IsTrue(Fraction.IsInfinity(positive));
            Assert.IsTrue(Fraction.IsPositiveInfinity(positive));
            Assert.IsFalse(Fraction.IsNegativeInfinity(positive));

            Assert.IsFalse(Fraction.IsNaN(negative));
            Assert.IsTrue(Fraction.IsInfinity(negative));
            Assert.IsFalse(Fraction.IsPositiveInfinity(negative));
            Assert.IsTrue(Fraction.IsNegativeInfinity(negative));

            Assert.IsFalse(Fraction.IsNaN(Fraction.Zero));
            Assert.IsFalse(Fraction.IsInfinity(Fraction.Zero));
        }

        [Test]
        public void OperatorsUseIeeeNaNSemanticsAndExactComplement()
        {
            var nan = Fraction.NaN;
            var anotherNaN = Fraction.NaN;
            var finite = new Fraction(1, 2);

            Assert.IsFalse(nan == anotherNaN);
            Assert.IsTrue(nan != anotherNaN);
            Assert.IsFalse(nan < anotherNaN);
            Assert.IsFalse(nan <= anotherNaN);
            Assert.IsFalse(nan > anotherNaN);
            Assert.IsFalse(nan >= anotherNaN);

            Assert.IsFalse(nan == finite);
            Assert.IsTrue(nan != finite);
            Assert.IsFalse(nan < finite);
            Assert.IsFalse(nan <= finite);
            Assert.IsFalse(nan > finite);
            Assert.IsFalse(nan >= finite);

            Assert.IsTrue(nan.Equals(Fraction.NaN));
            Assert.IsTrue(nan.Equals((object)Fraction.NaN));
            Assert.AreEqual(nan.GetHashCode(), Fraction.NaN.GetHashCode());

            var values = new[]
            {
                Fraction.NaN,
                Fraction.NegativeInfinity,
                new Fraction(-1, 3),
                Fraction.Zero,
                new Fraction(1, 3),
                Fraction.PositiveInfinity,
            };
            foreach (var a in values)
            foreach (var b in values)
                Assert.AreEqual(!(a == b), a != b);
        }

        [Test]
        public void InfinityOperatorsUseSignedIeeeOrdering()
        {
            var positive = new Fraction(17, 0);
            var negative = new Fraction(-19, 0);

            Assert.IsTrue(positive == Fraction.PositiveInfinity);
            Assert.IsTrue(negative == Fraction.NegativeInfinity);
            Assert.IsTrue(positive.Equals(Fraction.PositiveInfinity));
            Assert.IsTrue(negative.Equals(Fraction.NegativeInfinity));
            Assert.AreEqual(positive.GetHashCode(), Fraction.PositiveInfinity.GetHashCode());
            Assert.AreEqual(negative.GetHashCode(), Fraction.NegativeInfinity.GetHashCode());

            Assert.IsTrue(positive > Fraction.MaxValue);
            Assert.IsTrue(positive >= Fraction.PositiveInfinity);
            Assert.IsTrue(negative < Fraction.MinValue);
            Assert.IsTrue(negative <= Fraction.NegativeInfinity);
            Assert.IsTrue(negative < positive);
            Assert.IsTrue(positive != negative);
        }

        [Test]
        public void ReducedCanonicalizesFiniteAndSpecialValues()
        {
            AssertRaw(new Fraction(6, 8).Reduced, 3, 4);
            AssertRaw(new Fraction(-6, 8).Reduced, -3, 4);
            AssertRaw(new Fraction(0, long.MaxValue).Reduced, 0, 1);
            AssertRaw(new Fraction(long.MinValue, 2).Reduced, long.MinValue / 2, 1);

            AssertRaw(Fraction.NaN.Reduced, 0, 0);
            AssertRaw(new Fraction(17, 0).Reduced, 1, 0);
            AssertRaw(new Fraction(-17, 0).Reduced, -1, 0);
        }

        [Test]
        public void InfinityAdditionAndSubtractionAreDefined()
        {
            var finite = new Fraction(3, 7);
            var positive = new Fraction(17, 0);
            var negative = new Fraction(-19, 0);

            AssertRaw(positive + positive, 1, 0);
            AssertRaw(negative + negative, -1, 0);
            AssertRaw(positive + finite, 1, 0);
            AssertRaw(finite + positive, 1, 0);
            AssertRaw(negative + finite, -1, 0);
            AssertRaw(finite + negative, -1, 0);
            Assert.IsTrue(Fraction.IsNaN(positive + negative));
            Assert.IsTrue(Fraction.IsNaN(negative + positive));
            Assert.IsTrue(Fraction.IsNaN(Fraction.NaN + finite));
            Assert.IsTrue(Fraction.IsNaN(finite + Fraction.NaN));

            Assert.IsTrue(Fraction.IsNaN(positive - positive));
            Assert.IsTrue(Fraction.IsNaN(negative - negative));
            AssertRaw(positive - negative, 1, 0);
            AssertRaw(negative - positive, -1, 0);
            AssertRaw(finite - positive, -1, 0);
            AssertRaw(finite - negative, 1, 0);
            Assert.IsTrue(Fraction.IsNaN(Fraction.NaN - positive));
        }

        [Test]
        public void StructLayoutAndRawFieldsRemainStable()
        {
            Assert.AreEqual(16, Marshal.SizeOf<Fraction>());
            Assert.AreEqual(IntPtr.Zero, Marshal.OffsetOf<Fraction>(nameof(Fraction.Numerator)));
            Assert.AreEqual(new IntPtr(8), Marshal.OffsetOf<Fraction>(nameof(Fraction.Denominator)));

            var value = new Fraction(12, 30);
            AssertRaw(value, 12, 30);
            Assert.AreEqual("12/30", value.ToString());
        }

        [Test]
        public void ConstructedSignedExtremesPreserveValuesAndStorage()
        {
            foreach (var core in ConstructorCores())
            {
                foreach (var numerator in ConstructorBoundaryValues)
                foreach (var denominator in ConstructorBoundaryValues)
                    AssertConstructor(core, numerator, denominator);
                ulong state = 0xb624439ea3d53ce3UL;
                for (var i = 0; i < 4096; i++)
                {
                    var numerator = unchecked((long)Next(ref state));
                    var denominator = unchecked((long)Next(ref state));
                    if (i % 7 == 0) numerator = long.MinValue;
                    if (i % 7 == 1) denominator = long.MinValue;
                    if (i % 7 == 2) numerator = denominator = long.MinValue;
                    if (i % 7 == 3) denominator = 0;
                    AssertConstructor(core, numerator, denominator);
                }
            }
        }

        [Test]
        public void ExtremeFractionTextRoundTripsPreserveValues()
        {
            foreach (var core in ConstructorCores())
            foreach (var numerator in ConstructorBoundaryValues)
            foreach (var denominator in ConstructorBoundaryValues)
            {
                var raw = core.Raw(numerator, denominator);
                foreach (var source in new[] { raw, core.Reduce(raw) })
                {
                    var fields = core.Fields(source);
                    var context = $"{core.Name}: text round-trip {fields.N}/{fields.D}";
                    Assert.AreEqual(fields.N + "/" + fields.D, source.ToString(), context);
                    var parsed = core.Parse(source.ToString());
                    AssertFractionValue(core, parsed, fields.N, fields.D, context);
                    AssertNormalizedStorage(core, parsed, fields.N, fields.D, context);
                    Assert.IsTrue(core.Equal(source, parsed), context + " numerical Equals");
                    Assert.AreEqual(source.GetHashCode(), parsed.GetHashCode(), context + " hash");
                }
            }
        }

        [Test]
        public void ConstructedFractionsKeepCollectionAndStorageContracts()
        {
            foreach (var core in ConstructorCores())
            {
                Assert.AreEqual(16, Marshal.SizeOf(core.Type), core.Name);
                Assert.AreEqual(IntPtr.Zero, Marshal.OffsetOf(core.Type, "Numerator"), core.Name);
                Assert.AreEqual(new IntPtr(8), Marshal.OffsetOf(core.Type, "Denominator"), core.Name);
                Assert.IsNotNull(core.Type.GetCustomAttribute<DataContractAttribute>(), core.Name);
                foreach (var field in new[] { "Numerator", "Denominator" })
                    Assert.IsNotNull(core.Type.GetField(field).GetCustomAttribute<DataMemberAttribute>(), core.Name + " " + field);
                foreach (var n in ConstructorBoundaryValues)
                    Assert.AreEqual((n, 1L), core.Fields(core.Integer(n)), core.Name + " single-argument constructor");

                foreach (var numerator in ConstructorBoundaryValues)
                foreach (var denominator in ConstructorBoundaryValues)
                {
                    var context = $"{core.Name}: collections/reduction/storage {numerator}/{denominator}";
                    var value = core.Create(numerator, denominator); var raw = core.Raw(numerator, denominator);
                    Assert.IsTrue(core.Equal(raw, value), context + " numerical Equals");
                    Assert.AreEqual(numerator != 0 || denominator != 0, core.OperatorEqual(raw, value), context + " IEEE equality");
                    Assert.AreEqual(raw.GetHashCode(), value.GetHashCode(), context + " hash");
                    var reduced = core.Reduce(value); var fields = core.Fields(reduced);
                    AssertFractionValue(core, reduced, numerator, denominator, context);
                    Assert.IsTrue(core.Equal(raw, reduced), context + " reduced Equals");
                    Assert.AreEqual(raw.GetHashCode(), reduced.GetHashCode(), context + " reduced hash");
                    Assert.AreEqual(fields, core.Fields(core.Reduce(reduced)), context + " reduction idempotence");
                    if (denominator != 0)
                    {
                        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), BigInteger.Abs(denominator));
                        BigInteger n = numerator / gcd, d = denominator / gcd;
                        if (d < 0) { n = -n; d = -d; }
                        Assert.AreEqual(BigInteger.One, BigInteger.GreatestCommonDivisor(BigInteger.Abs(fields.N), BigInteger.Abs(fields.D)), context + " lowest terms");
                        if (n >= long.MinValue && n <= long.MaxValue && d <= long.MaxValue)
                            Assert.AreEqual(((long)n, (long)d), fields, context + " representable canonical reduction");
                    }
                    else Assert.AreEqual(((long)Math.Sign(numerator), 0L), fields, context + " canonical special value");

                    foreach (var stored in new[] { raw, value })
                    {
                        using (var stream = new MemoryStream())
                        {
                            var serializer = new DataContractSerializer(core.Type);
                            serializer.WriteObject(stream, stored); stream.Position = 0;
                            var restored = serializer.ReadObject(stream);
                            Assert.That(core.Type.IsInstanceOfType(restored), Is.True, context + " serialized type identity");
                            Assert.AreEqual(core.Fields(stored), core.Fields(restored), context + " serialized raw fields");
                        }
                    }
                }
            }
        }

        private static readonly long[] ConstructorBoundaryValues =
        {
            long.MinValue, long.MinValue + 1, long.MinValue + 2,
            -2, -1, 0, 1, 2, long.MaxValue - 1, long.MaxValue
        };

        private static void AssertConstructor(ConstructorCore core, long n, long d)
        {
            var context = $"{core.Name}: construct {n}/{d}";
            var actual = core.Create(n, d);
            AssertFractionValue(core, actual, n, d, context);
            AssertNormalizedStorage(core, actual, n, d, context);
        }

        private static void AssertFractionValue(ConstructorCore core, object actual, long n, long d, string context)
        {
            var fields = core.Fields(actual);
            if (d == 0)
            {
                Assert.AreEqual(0, fields.D, context + " zero denominator");
                Assert.AreEqual(Math.Sign(n), Math.Sign(fields.N), context + " NaN/infinity sign");
            }
            else
            {
                Assert.AreNotEqual(0, fields.D, context + " finite denominator");
                Assert.AreEqual((BigInteger)n * fields.D, (BigInteger)fields.N * d, context + " exact cross-products");
            }
        }

        private static void AssertNormalizedStorage(ConstructorCore core, object actual, long n, long d, string context)
        {
            BigInteger numerator = n, denominator = d;
            if (d < 0 && -numerator >= long.MinValue && -numerator <= long.MaxValue && -denominator <= long.MaxValue)
            {
                numerator = -numerator; denominator = -denominator;
            }
            Assert.AreEqual(((long)numerator, (long)denominator), core.Fields(actual), context + " exact unreduced storage");
        }

        private sealed class ConstructorCore
        {
            public readonly Type Type;
            public readonly string Name;
            private readonly ConstructorInfo pair, integer;
            private readonly FieldInfo numerator, denominator;
            private readonly MethodInfo parse, reduced, equals, operatorEqual;
            public ConstructorCore(Assembly assembly, string name)
            {
                Name = name; Type = assembly.GetType("Aardvark.Base.Fraction", true);
                Assert.AreSame(assembly, Type.Assembly, name);
                pair = Type.GetConstructor(new[] { typeof(long), typeof(long) }); integer = Type.GetConstructor(new[] { typeof(long) });
                numerator = Type.GetField("Numerator"); denominator = Type.GetField("Denominator");
                parse = Type.GetMethod("Parse", new[] { typeof(string) }); reduced = Type.GetProperty("Reduced").GetMethod;
                equals = Type.GetMethod("Equals", new[] { Type }); operatorEqual = Type.GetMethod("op_Equality", new[] { Type, Type });
                Assert.IsNotNull(pair, name); Assert.IsNotNull(integer, name); Assert.IsNotNull(parse, name); Assert.IsNotNull(equals, name);
            }
            public object Create(long n, long d) => pair.Invoke(new object[] { n, d });
            public object Integer(long n) => integer.Invoke(new object[] { n });
            public object Raw(long n, long d)
            {
                var value = Activator.CreateInstance(Type); numerator.SetValue(value, n); denominator.SetValue(value, d); return value;
            }
            public (long N, long D) Fields(object value) => ((long)numerator.GetValue(value), (long)denominator.GetValue(value));
            public object Parse(string text) => parse.Invoke(null, new object[] { text });
            public object Reduce(object value) => reduced.Invoke(value, null);
            public bool Equal(object a, object b) => (bool)equals.Invoke(a, new[] { b });
            public bool OperatorEqual(object a, object b) => (bool)operatorEqual.Invoke(null, new[] { a, b });
        }

        private static IEnumerable<ConstructorCore> ConstructorCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fraction).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new ConstructorCore(assembly, target);
            }
        }

        private static void AssertExactOrder(Fraction a, Fraction b, int expected)
        {
            Assert.AreEqual(expected < 0, a < b);
            Assert.AreEqual(expected <= 0, a <= b);
            Assert.AreEqual(expected == 0, a == b);
            Assert.AreEqual(expected != 0, a != b);
            Assert.AreEqual(expected >= 0, a >= b);
            Assert.AreEqual(expected > 0, a > b);
            Assert.AreEqual(expected == 0, a.Equals(b));
            if (expected == 0) Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        private static void AssertRaw(Fraction actual, long numerator, long denominator)
        {
            Assert.AreEqual(numerator, actual.Numerator);
            Assert.AreEqual(denominator, actual.Denominator);
        }

        private static Fraction Raw(long numerator, long denominator)
            => new Fraction { Numerator = numerator, Denominator = denominator };

        private static int ReferenceOrder(Fraction a, Fraction b)
        {
            BigInteger aNumerator = a.Numerator;
            BigInteger aDenominator = a.Denominator;
            BigInteger bNumerator = b.Numerator;
            BigInteger bDenominator = b.Denominator;
            if (aDenominator < 0)
            {
                aNumerator = -aNumerator;
                aDenominator = -aDenominator;
            }
            if (bDenominator < 0)
            {
                bNumerator = -bNumerator;
                bDenominator = -bDenominator;
            }
            return System.Math.Sign((aNumerator * bDenominator).CompareTo(bNumerator * aDenominator));
        }

        private static ulong Next(ref ulong state)
        {
            state ^= state >> 12;
            state ^= state << 25;
            state ^= state >> 27;
            return state * 0x2545f4914f6cdd1dUL;
        }
    }
}

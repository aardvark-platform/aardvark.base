using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests.Extensions
{
    static class DictionaryTests
    {
        private static Dictionary<int, T> MakeDictionary<T>(T[] values, bool reverse = false)
        {
            var result = new Dictionary<int, T>();
            for (var i = 0; i < values.Length; i++)
            {
                var key = reverse ? values.Length - 1 - i : i;
                result.Add(key, values[key]);
            }
            return result;
        }

        private static void AssertPair<T>(T[] left, T[] right, bool expected, string context)
        {
            var a = MakeDictionary(left);
            var b = MakeDictionary(right, true);
            Assert.AreEqual(expected, a.DictionaryEquals(b), context + ", left to right");
            Assert.AreEqual(expected, b.DictionaryEquals(a), context + ", right to left");
        }

        [Test]
        public static void DictionaryEqualsMatchesNullMixedAndOrdinaryValues()
        {
            // Independent string instances distinguish value equality from identity.
            for (var length = 0; length <= 3; length++)
            {
                var variants = 1;
                for (var i = 0; i < length; i++) variants *= 3;
                for (var leftMask = 0; leftMask < variants; leftMask++)
                for (var rightMask = 0; rightMask < variants; rightMask++)
                {
                    var left = new string[length];
                    var right = new string[length];
                    var a = leftMask;
                    var b = rightMask;
                    var expected = true;
                    for (var i = 0; i < length; i++, a /= 3, b /= 3)
                    {
                        left[i] = a % 3 == 0 ? null : new string(a % 3 == 1 ? 'a' : 'b', 1);
                        right[i] = b % 3 == 0 ? null : new string(b % 3 == 1 ? 'a' : 'b', 1);
                        expected &= string.Equals(left[i], right[i], StringComparison.Ordinal);
                    }
                    AssertPair(left, right, expected, $"length={length}, left mask={leftMask}, right mask={rightMask}");
                }
            }
            foreach (var length in new[] { 0, 1, 8, 64 })
            {
                var left = new int[length];
                var right = new int[length];
                for (var i = 0; i < length; i++) left[i] = right[i] = i - 3;
                AssertPair(left, right, true, $"int length={length}, equal");
                if (length == 0) continue;
                foreach (var mismatch in new[] { 0, length / 2, length - 1 })
                {
                    right[mismatch]++;
                    AssertPair(left, right, false, $"int length={length}, mismatch={mismatch}");
                    right[mismatch]--;
                }
            }
            var shared = new object();
            AssertPair(new[] { shared, null }, new[] { shared, null }, true, "shared reference and null");
            AssertPair(new[] { shared }, new[] { new object() }, false, "distinct references");
            foreach (var a in new int?[] { null, 0, 7 })
            foreach (var b in new int?[] { null, 0, 7 })
                AssertPair(new int?[] { 1, a, null }, new int?[] { 1, b, null }, a == b, $"nullable left={a}, right={b}");
        }

        [Test]
        public static void DictionaryEqualsRetainsCountsMissingKeysAndLookupDirection()
        {
            var one = new Dictionary<int, string> { [0] = null };
            foreach (var other in new[] { new Dictionary<int, string>(),
                                         new Dictionary<int, string> { [1] = null },
                                         new Dictionary<int, string> { [0] = null, [1] = "a" } })
            {
                var context = $"other count={other.Count}, has key zero={other.ContainsKey(0)}";
                Assert.IsFalse(one.DictionaryEquals(other), context);
                Assert.IsFalse(other.DictionaryEquals(one), context);
                Func<string, string, bool> unexpected = (a, b) => throw new AssertionException(context);
                Assert.IsFalse(one.DictionaryEquals(other, unexpected), context);
                Assert.IsFalse(other.DictionaryEquals(one, unexpected), context);
            }
            var ordinal = new Dictionary<string, string>(StringComparer.Ordinal) { ["key"] = "value" };
            var insensitive = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["KEY"] = "value" };
            Assert.IsTrue(ordinal.DictionaryEquals(insensitive), "lookup uses the other dictionary's comparer");
            Assert.IsFalse(insensitive.DictionaryEquals(ordinal), "reverse lookup remains case-sensitive");
        }

        private sealed class ObjectEqualityValue : IEquatable<ObjectEqualityValue>
        {
            private readonly Func<object, bool> m_compare;
            public int TypedCalls;
            public ObjectEqualityValue(Func<object, bool> compare) { m_compare = compare; }
            public bool Equals(ObjectEqualityValue other) { TypedCalls++; return false; }
            public override bool Equals(object other) => m_compare(other);
            public override int GetHashCode() => 0;
        }

        [Test]
        public static void DictionaryEqualsRetainsLeftObjectDispatchAndFailures()
        {
            var calls = new List<object>();
            var right = new ObjectEqualityValue(other => false);
            var left = new ObjectEqualityValue(other => { calls.Add(other); return ReferenceEquals(other, right); });
            Assert.IsTrue(MakeDictionary(new[] { left }).DictionaryEquals(MakeDictionary(new[] { right })));
            Assert.AreSame(right, calls[0]);
            Assert.IsFalse(MakeDictionary(new[] { right }).DictionaryEquals(MakeDictionary(new[] { left })));
            Assert.AreEqual(0, left.TypedCalls + right.TypedCalls, "Equals(object), not IEquatable<T>.Equals");

            calls.Clear();
            var nullFriendly = new ObjectEqualityValue(other => { calls.Add(other); return other == null; });
            var nullValue = MakeDictionary(new ObjectEqualityValue[] { null });
            var nonNullValue = MakeDictionary(new[] { nullFriendly });
            Assert.IsTrue(nonNullValue.DictionaryEquals(nullValue), "non-null receiver may consider null equal");
            Assert.IsFalse(nullValue.DictionaryEquals(nonNullValue), "null receiver equals only null");
            CollectionAssert.AreEqual(new object[] { null }, calls, "reverse comparison must not dispatch to the other value");
            Assert.AreEqual(0, nullFriendly.TypedCalls);

            foreach (var stopAt in new[] { 0, 1, 2 })
            foreach (var throws in new[] { false, true })
            {
                var visited = new List<int>();
                var failure = new InvalidOperationException("value equality failed");
                var self = new SortedDictionary<int, ObjectEqualityValue>();
                var other = new Dictionary<int, ObjectEqualityValue>();
                for (var i = 0; i < 3; i++)
                {
                    var key = i;
                    self.Add(key, new ObjectEqualityValue(value =>
                    {
                        Assert.AreSame(right, value);
                        visited.Add(key);
                        if (key == stopAt && throws) throw failure;
                        return key != stopAt;
                    }));
                    other.Add(key, right);
                }
                var context = $"default stop={stopAt}, throws={throws}";
                if (throws) Assert.AreSame(failure, Assert.Throws<InvalidOperationException>(() => self.DictionaryEquals(other), context));
                else Assert.IsFalse(self.DictionaryEquals(other), context);
                CollectionAssert.AreEqual(new[] { 0, 1, 2 }.Take(stopAt + 1), visited, context);
            }
        }

        [Test]
        public static void DictionaryEqualsCustomComparatorReceivesNullsInOrderAndPropagatesFailures()
        {
            var self = new SortedDictionary<int, string> { [2] = "left2", [0] = null, [1] = "left1" };
            var other = new Dictionary<int, string> { [2] = "right2", [1] = null, [0] = null };
            foreach (var stopAt in new[] { -1, 0, 1, 2 })
            foreach (var throws in new[] { false, true })
            {
                if (stopAt == -1 && throws) continue;
                var calls = new List<(string, string)>();
                var failure = new InvalidOperationException("custom comparison failed");
                Func<string, string, bool> compare = (a, b) =>
                {
                    var index = calls.Count;
                    calls.Add((a, b));
                    if (index == stopAt && throws) throw failure;
                    return index != stopAt;
                };
                var context = $"custom stop={stopAt}, throws={throws}";
                if (throws) Assert.AreSame(failure, Assert.Throws<InvalidOperationException>(() => self.DictionaryEquals(other, compare), context));
                else Assert.AreEqual(stopAt == -1, self.DictionaryEquals(other, compare), context);
                var expected = new (string, string)[] { (null, null), ("left1", null), ("left2", "right2") };
                CollectionAssert.AreEqual(expected.Take(stopAt < 0 ? 3 : stopAt + 1), calls, context);
            }
            Func<string, string, bool> missing = null;
            Assert.IsTrue(new Dictionary<int, string>().DictionaryEquals(new Dictionary<int, string>(), missing));
            Assert.IsFalse(new Dictionary<int, string> { [9] = null }.DictionaryEquals(new Dictionary<int, string> { [8] = null }, missing));
            Assert.Throws<NullReferenceException>(() => self.DictionaryEquals(other, missing));
        }
    }
}

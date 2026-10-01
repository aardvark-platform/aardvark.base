using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Tests.Extensions
{
    public static class DictionaryExtensionsTests
    {
        private static Dictionary<TKey, TValue> CopyIdentity<TKey, TValue>(Dictionary<TKey, TValue> source, int variant)
            => variant switch
            {
                0 => DictionaryFun.Copy(source),
                1 => DictionaryFun.Copy(source, value => value),
                2 => DictionaryFun.Copy(source, new Dictionary<TKey, Func<TValue, TValue>>(), value => value),
                _ => throw new ArgumentOutOfRangeException(nameof(variant))
            };

        [Test]
        public static void CopiesPreserveStringEqualityAndIndependentStorage()
        {
            foreach (var comparer in new IEqualityComparer<string>[]
            {
                StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, EqualityComparer<string>.Default
            })
            foreach (var empty in new[] { false, true })
            for (var variant = 0; variant < 3; variant++)
            {
                var context = $"comparer={comparer}, empty={empty}, overload={variant}";
                var source = new Dictionary<string, object>(comparer);
                var original = new object();
                if (!empty) { source.Add("Alpha", original); source.Add("Beta", new object()); }
                var copy = CopyIdentity(source, variant);
                Assert.AreNotSame(source, copy, context);
                Assert.AreSame(source.Comparer, copy.Comparer, context);
                Assert.AreEqual(source.Count, copy.Count, context);
                foreach (var pair in source)
                {
                    Assert.AreSame(pair.Value, copy[pair.Key], context);
                    Assert.AreEqual(comparer.Equals(pair.Key, pair.Key.ToUpperInvariant()),
                        copy.ContainsKey(pair.Key.ToUpperInvariant()), context);
                }

                var first = new object();
                var second = new object();
                copy["Alpha"] = first;
                copy["ALPHA"] = second;
                var equal = comparer.Equals("Alpha", "ALPHA");
                Assert.AreEqual((empty ? 1 : 2) + (equal ? 0 : 1), copy.Count, context);
                Assert.AreSame(equal ? second : first, copy["Alpha"], context);
                Assert.AreSame(second, copy["ALPHA"], context);
                if (empty) Assert.IsFalse(source.ContainsKey("Alpha"), context);
                else Assert.AreSame(original, source["Alpha"], context);
                copy["CopyOnly"] = new object();
                source["SourceOnly"] = new object();
                Assert.IsFalse(source.ContainsKey("CopyOnly"), context);
                Assert.IsFalse(copy.ContainsKey("SourceOnly"), context);
                if (!empty)
                {
                    Assert.IsTrue(copy.Remove("Alpha"), context);
                    Assert.IsTrue(source.ContainsKey("Alpha"), context);
                    Assert.IsTrue(source.Remove("Beta"), context);
                    Assert.IsTrue(copy.ContainsKey("Beta"), context);
                }
            }
        }

        [Test]
        public static void CopiesDoNotCollapseDistinctReferenceIdentityKeys()
        {
            foreach (var identity in new[] { true, false })
            foreach (var empty in new[] { false, true })
            for (var variant = 0; variant < 3; variant++)
            {
                var context = $"referenceIdentity={identity}, empty={empty}, overload={variant}";
                var first = new string(new[] { 'k', 'e', 'y' });
                var second = new string(new[] { 'k', 'e', 'y' });
                Assert.AreEqual(first, second, context);
                Assert.AreNotSame(first, second, context);
                IEqualityComparer<string> comparer = identity ? ReferenceEqualityComparer.Instance : EqualityComparer<string>.Default;
                var source = new Dictionary<string, object>(comparer);
                var firstValue = new object();
                var secondValue = new object();
                if (!empty) { source[first] = firstValue; source[second] = secondValue; }
                var copy = CopyIdentity(source, variant);
                Assert.AreEqual(source.Count, copy.Count, context + ": no entry loss");
                Assert.AreSame(source.Comparer, copy.Comparer, context);
                if (empty) { copy[first] = firstValue; copy[second] = secondValue; }
                Assert.AreEqual(identity ? 2 : 1, copy.Count, context);
                Assert.AreSame(identity ? firstValue : secondValue, copy[first], context);
                Assert.AreSame(secondValue, copy[second], context);
                Assert.AreEqual(!identity, copy.ContainsKey(new string(new[] { 'k', 'e', 'y' })), context);
                copy[first] = new object();
                Assert.AreEqual(empty ? 0 : identity ? 2 : 1, source.Count, context);
                if (!empty) Assert.AreSame(identity ? firstValue : secondValue, source[first], context);
            }
        }

        [Test]
        public static void MappedCopiesKeepFunctionMapLookupFallbackAndOmissionSemantics()
        {
            foreach (var sourceComparer in new IEqualityComparer<string>[] { StringComparer.OrdinalIgnoreCase, EqualityComparer<string>.Default })
            foreach (var mapComparer in new IEqualityComparer<string>[] { StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal })
            foreach (var useFallback in new[] { false, true })
            {
                var context = $"sourceComparer={sourceComparer}, mapComparer={mapComparer}, fallback={useFallback}";
                var source = new Dictionary<string, int>(sourceComparer) { ["Alpha"] = 2, ["Beta"] = 3, ["Gamma"] = 5 };
                var calls = new List<int>();
                var mapped = DictionaryFun.Copy(source, value => { calls.Add(value); return $"mapped:{value}"; });
                Assert.AreSame(source.Comparer, mapped.Comparer, context);
                CollectionAssert.AreEquivalent(new[] { 2, 3, 5 }, calls, context);
                foreach (var pair in source) Assert.AreEqual($"mapped:{pair.Value}", mapped[pair.Key], context);

                var alphaCalls = 0;
                var betaCalls = 0;
                var fallbackCalls = new List<int>();
                var functions = new Dictionary<string, Func<int, string>>(mapComparer)
                {
                    ["ALPHA"] = value => { alphaCalls++; return $"alpha:{value}"; },
                    ["Beta"] = value => { betaCalls++; return $"beta:{value}"; }
                };
                Func<int, string> fallback = useFallback ? value => { fallbackCalls.Add(value); return $"fallback:{value}"; } : null;
                var result = DictionaryFun.Copy(source, functions, fallback);
                var alphaMapped = mapComparer.Equals("Alpha", "ALPHA");
                Assert.AreSame(source.Comparer, result.Comparer, context);
                Assert.AreSame(mapComparer, functions.Comparer, context);
                Assert.AreEqual(useFallback ? 3 : alphaMapped ? 2 : 1, result.Count, context);
                Assert.AreEqual(alphaMapped ? 1 : 0, alphaCalls, context);
                Assert.AreEqual(1, betaCalls, context);
                Assert.AreEqual("beta:3", result["Beta"], context);
                Assert.AreEqual(alphaMapped || useFallback, result.ContainsKey("Alpha"), context);
                if (alphaMapped || useFallback) Assert.AreEqual(alphaMapped ? "alpha:2" : "fallback:2", result["Alpha"], context);
                Assert.AreEqual(useFallback, result.ContainsKey("Gamma"), context);
                if (useFallback) Assert.AreEqual("fallback:5", result["Gamma"], context);
                var expectedFallback = !useFallback ? Array.Empty<int>() : alphaMapped ? new[] { 5 } : new[] { 2, 5 };
                CollectionAssert.AreEquivalent(expectedFallback, fallbackCalls, context);
                Assert.AreEqual(3, source.Count, context);
                Assert.AreEqual(2, source["Alpha"], context);
                Assert.AreEqual(2, functions.Count, context);
            }
        }

        [Test]
        public static void CopyCallbacksAndNullArgumentsKeepTheirExceptionBehavior()
        {
            var source = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["Alpha"] = 17 };
            for (var variant = 0; variant < 3; variant++)
            {
                var context = $"callback={variant}";
                var failure = new InvalidOperationException(context);
                var calls = 0;
                Func<int, string> fail = value => { calls++; throw failure; };
                var functions = new Dictionary<string, Func<int, string>>();
                if (variant == 1) functions.Add("Alpha", fail);
                var error = Assert.Throws<InvalidOperationException>(() =>
                {
                    if (variant == 0) DictionaryFun.Copy(source, fail);
                    else DictionaryFun.Copy(source, functions, fail);
                });
                Assert.AreSame(failure, error, context);
                Assert.AreEqual(1, calls, context);
                Assert.AreEqual(1, source.Count, context);
                Assert.AreEqual(17, source["Alpha"], context);
            }

            var empty = new Dictionary<string, int>(source.Comparer);
            Assert.AreEqual(0, DictionaryFun.Copy(empty, (Func<int, string>)null).Count);
            Assert.AreEqual(0, DictionaryFun.Copy(empty, (Dictionary<string, Func<int, string>>)null, null).Count);
            Assert.Throws<NullReferenceException>(() => DictionaryFun.Copy(source, (Func<int, string>)null));
            Assert.Throws<NullReferenceException>(() => DictionaryFun.Copy(source, (Dictionary<string, Func<int, string>>)null, null));
            var nullFunction = new Dictionary<string, Func<int, string>> { ["Alpha"] = null };
            Assert.Throws<NullReferenceException>(() => DictionaryFun.Copy(source, nullFunction, value => "unused"));
            for (var variant = 0; variant < 3; variant++)
                Assert.Throws<NullReferenceException>(() => CopyIdentity((Dictionary<string, int>)null, variant));
        }

        [Test]
        public static void CombineUsesTheLeftComparerAndIndependentResultStorage()
        {
            foreach (var leftComparer in new IEqualityComparer<string>[] { StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, EqualityComparer<string>.Default })
            foreach (var rightComparer in new IEqualityComparer<string>[] { StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, EqualityComparer<string>.Default })
            foreach (var emptyLeft in new[] { false, true })
            {
                var context = $"left={leftComparer}, right={rightComparer}, emptyLeft={emptyLeft}";
                var left = new Dictionary<string, int>(leftComparer);
                if (!emptyLeft) { left["Alpha"] = 1; left["LeftOnly"] = 10; }
                var right = new Dictionary<string, int>(rightComparer);
                right["ALPHA"] = 2;
                right["alpha"] = 3;
                right["Beta"] = 4;
                var expected = new Dictionary<string, int>(left, left.Comparer);
                foreach (var pair in right) expected[pair.Key] = pair.Value;
                var result = DictionaryFun.Combine(left, right);
                Assert.AreNotSame(left, result, context);
                Assert.AreNotSame(right, result, context);
                Assert.AreSame(left.Comparer, result.Comparer, context);
                Assert.AreEqual(expected.Count, result.Count, context);
                foreach (var pair in expected) Assert.AreEqual(pair.Value, result[pair.Key], context + $", key={pair.Key}");
                Assert.AreEqual(leftComparer.Equals("Beta", "BETA"), result.ContainsKey("BETA"), context);
                result["Beta"] = 100;
                result["ResultOnly"] = 200;
                Assert.AreEqual(4, right["Beta"], context);
                Assert.IsFalse(left.ContainsKey("ResultOnly"), context);
                Assert.IsFalse(right.ContainsKey("ResultOnly"), context);
                left["LeftOnly"] = 300;
                right["RightOnly"] = 400;
                Assert.AreEqual(!emptyLeft, result.ContainsKey("LeftOnly"), context);
                if (!emptyLeft) Assert.AreEqual(10, result["LeftOnly"], context);
                Assert.IsFalse(result.ContainsKey("RightOnly"), context);
            }
        }
    }
}

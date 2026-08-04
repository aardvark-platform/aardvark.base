using System;
using System.Collections.Generic;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class LruCacheTests
    {
        private static void AssertArgumentOutOfRange(TestDelegate action, string parameterName)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.AreEqual(parameterName, ex.ParamName);
        }

        private static LruCache<string, string> CreateStringCache(long capacity, List<string> deletes = null)
        {
            return new LruCache<string, string>(
                capacity,
                key => 1,
                key => "loaded-" + key,
                (key, value) => deletes?.Add(key + ":" + value));
        }

        [Test]
        public void TryRemoveReturnsValueAndRunsCleanupActions()
        {
            var cacheWideDeletes = new List<string>();
            var perEntryDeleteCount = 0;
            var valueFactoryCount = 0;
            var cache = new LruCache<string, string>(
                10,
                key => 1,
                key => "loaded-" + key,
                (key, value) => cacheWideDeletes.Add(key + ":" + value));

            var value = cache.GetOrAdd("a", 1, () =>
            {
                valueFactoryCount++;
                return "value-a";
            }, () => perEntryDeleteCount++);

            Assert.AreEqual("value-a", value);

            Assert.IsTrue(cache.TryRemove("a", out var removed));
            Assert.AreEqual("value-a", removed);
            CollectionAssert.AreEqual(new[] { "a:value-a" }, cacheWideDeletes);
            Assert.AreEqual(1, perEntryDeleteCount);

            Assert.IsFalse(cache.TryRemove("a", out removed));
            Assert.AreEqual(default(string), removed);
            CollectionAssert.AreEqual(new[] { "a:value-a" }, cacheWideDeletes);
            Assert.AreEqual(1, perEntryDeleteCount);

            var reloaded = cache.GetOrAdd("a", 1, () =>
            {
                valueFactoryCount++;
                return "value-a2";
            });
            Assert.AreEqual("value-a2", reloaded);
            Assert.AreEqual(2, valueFactoryCount);
        }

        [Test]
        public void TryRemoveMissingKeyDoesNotRunCleanupActions()
        {
            var cacheWideDeleteCount = 0;
            var perEntryDeleteCount = 0;
            var cache = new LruCache<string, int>(
                10,
                key => 1,
                key => 0,
                (key, value) => cacheWideDeleteCount++);

            cache.GetOrAdd("a", 1, () => 1, () => perEntryDeleteCount++);

            Assert.IsFalse(cache.TryRemove("b", out var removed));
            Assert.AreEqual(default(int), removed);
            Assert.AreEqual(0, cacheWideDeleteCount);
            Assert.AreEqual(0, perEntryDeleteCount);

            Assert.IsTrue(cache.TryRemove("a", out removed));
            Assert.AreEqual(1, removed);
            Assert.AreEqual(1, cacheWideDeleteCount);
            Assert.AreEqual(1, perEntryDeleteCount);
        }

        [Test]
        public void RemoveTryRemoveAndEvictionRunPerEntryCleanup()
        {
            var perEntryDeletes = new List<string>();
            var cache = new LruCache<string, string>(1);

            cache.GetOrAdd("remove", 1, () => "remove-value", () => perEntryDeletes.Add("remove"));
            Assert.IsTrue(cache.Remove("remove"));

            cache.GetOrAdd("try-remove", 1, () => "try-remove-value", () => perEntryDeletes.Add("try-remove"));
            Assert.IsTrue(cache.TryRemove("try-remove", out var removed));
            Assert.AreEqual("try-remove-value", removed);

            cache.GetOrAdd("evict", 1, () => "evict-value", () => perEntryDeletes.Add("evict"));
            cache.GetOrAdd("replacement", 1, () => "replacement-value");

            CollectionAssert.AreEqual(new[] { "remove", "try-remove", "evict" }, perEntryDeletes);
        }

        [Test]
        public void IndexerReadExceptionDoesNotEvictExistingEntryOrRunCleanup()
        {
            var deletes = new List<string>();
            var cache = new LruCache<string, string>(
                1,
                key => 1,
                key =>
                {
                    if (key == "b") throw new InvalidOperationException("read failed");
                    return "loaded-" + key;
                },
                (key, value) => deletes.Add(key + ":" + value));

            Assert.AreEqual("loaded-a", cache["a"]);

            Assert.Throws<InvalidOperationException>(() => { var _ = cache["b"]; });
            CollectionAssert.IsEmpty(deletes);

            Assert.IsTrue(cache.TryRemove("a", out var value));
            Assert.AreEqual("loaded-a", value);
            CollectionAssert.AreEqual(new[] { "a:loaded-a" }, deletes);
        }

        [Test]
        public void GetOrAddFactoryExceptionDoesNotEvictExistingEntryOrRunCleanup()
        {
            var deletes = new List<string>();
            var failedEntryDeleteCount = 0;
            var cache = CreateStringCache(1, deletes);

            Assert.AreEqual("value-a", cache.GetOrAdd("a", 1, () => "value-a"));

            Assert.Throws<InvalidOperationException>(() =>
                cache.GetOrAdd("b", 1, () => throw new InvalidOperationException("factory failed"), () => failedEntryDeleteCount++));

            CollectionAssert.IsEmpty(deletes);
            Assert.AreEqual(0, failedEntryDeleteCount);

            Assert.IsTrue(cache.TryRemove("a", out var value));
            Assert.AreEqual("value-a", value);
            CollectionAssert.AreEqual(new[] { "a:value-a" }, deletes);
        }

        [Test]
        public void IndexerEvictionCacheCleanupExceptionLeavesCacheConsistent()
        {
            var expected = new InvalidOperationException("cache cleanup failed");
            var callbacks = new List<string>();
            var reads = new List<string>();
            var throwCleanup = true;
            var cache = new LruCache<string, string>(
                2,
                key => 1,
                key =>
                {
                    reads.Add(key);
                    return "loaded-" + key;
                },
                (key, value) =>
                {
                    callbacks.Add("cache:" + key);
                    if (key == "a" && throwCleanup)
                    {
                        throwCleanup = false;
                        throw expected;
                    }
                });

            cache.GetOrAdd("a", 1, () => "value-a", () => callbacks.Add("entry:a"));
            cache.GetOrAdd("b", 1, () => "value-b", () => callbacks.Add("entry:b"));

            var exception = Assert.Throws<InvalidOperationException>(() => { var _ = cache["c"]; });
            Assert.AreSame(expected, exception);
            CollectionAssert.AreEqual(new[] { "cache:a" }, callbacks);
            Assert.IsFalse(cache.TryRemove("a", out _));
            Assert.AreEqual(
                "value-b",
                cache.GetOrAdd("b", 1, () => throw new AssertionException("Survivor was reloaded.")));

            Assert.AreEqual("loaded-c", cache["c"]);
            CollectionAssert.AreEqual(new[] { "cache:a" }, callbacks);

            Assert.AreEqual("loaded-d", cache["d"]);
            CollectionAssert.AreEqual(new[] { "cache:a", "cache:b", "entry:b" }, callbacks);
            Assert.IsFalse(cache.TryRemove("b", out _));
            Assert.AreEqual("loaded-c", cache["c"]);
            Assert.AreEqual("loaded-d", cache["d"]);
            CollectionAssert.AreEqual(new[] { "c", "c", "d" }, reads);
        }

        [Test]
        public void GetOrAddEvictionEntryCleanupExceptionLeavesCacheConsistent()
        {
            var expected = new InvalidOperationException("entry cleanup failed");
            var callbacks = new List<string>();
            var throwCleanup = true;
            var cache = new LruCache<string, string>(
                1,
                key => 1,
                key => "loaded-" + key,
                (key, value) => callbacks.Add("cache:" + key));

            cache.GetOrAdd("a", 1, () => "value-a", () =>
            {
                callbacks.Add("entry:a");
                if (throwCleanup)
                {
                    throwCleanup = false;
                    throw expected;
                }
            });

            int factoryCount = 0;
            Action deleteB = () => callbacks.Add("entry:b");
            var exception = Assert.Throws<InvalidOperationException>(() =>
                cache.GetOrAdd("b", 1, () => "value-b-" + ++factoryCount, deleteB));

            Assert.AreSame(expected, exception);
            CollectionAssert.AreEqual(new[] { "cache:a", "entry:a" }, callbacks);
            Assert.IsFalse(cache.TryRemove("a", out _));

            Assert.AreEqual(
                "value-b-2",
                cache.GetOrAdd("b", 1, () => "value-b-" + ++factoryCount, deleteB));
            Assert.AreEqual(2, factoryCount);
            CollectionAssert.AreEqual(new[] { "cache:a", "entry:a" }, callbacks);

            Assert.AreEqual("value-c", cache.GetOrAdd("c", 1, () => "value-c"));
            CollectionAssert.AreEqual(
                new[] { "cache:a", "entry:a", "cache:b", "entry:b" }, callbacks);
            Assert.IsFalse(cache.TryRemove("b", out _));
            Assert.AreEqual(
                "value-c",
                cache.GetOrAdd("c", 1, () => throw new AssertionException("Current entry was reloaded.")));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CapacityShrinkCleanupExceptionLeavesCacheConsistent(bool cacheCleanupThrows)
        {
            var expected = new InvalidOperationException("cleanup failed");
            var callbacks = new List<string>();
            var throwCleanup = true;
            var cache = new LruCache<string, string>(
                3,
                key => 1,
                key => "loaded-" + key,
                (key, value) =>
                {
                    callbacks.Add("cache:" + key);
                    if (cacheCleanupThrows && key == "a" && throwCleanup)
                    {
                        throwCleanup = false;
                        throw expected;
                    }
                });

            Action Delete(string key)
            {
                return () =>
                {
                    callbacks.Add("entry:" + key);
                    if (!cacheCleanupThrows && key == "a" && throwCleanup)
                    {
                        throwCleanup = false;
                        throw expected;
                    }
                };
            }

            cache.GetOrAdd("a", 1, () => "value-a", Delete("a"));
            cache.GetOrAdd("b", 1, () => "value-b", Delete("b"));
            cache.GetOrAdd("c", 1, () => "value-c", Delete("c"));

            var exception = Assert.Throws<InvalidOperationException>(() => cache.Capacity = 2);
            Assert.AreSame(expected, exception);
            Assert.AreEqual(2, cache.Capacity);
            CollectionAssert.AreEqual(
                cacheCleanupThrows
                    ? new[] { "cache:a" }
                    : new[] { "cache:a", "entry:a" },
                callbacks);
            Assert.IsFalse(cache.TryRemove("a", out _));
            Assert.AreEqual(
                "value-b",
                cache.GetOrAdd("b", 1, () => throw new AssertionException("Survivor was reloaded.")));
            Assert.AreEqual(
                "value-c",
                cache.GetOrAdd("c", 1, () => throw new AssertionException("Survivor was reloaded.")));

            cache.Capacity = 2;
            int callbackCount = callbacks.Count;
            cache.Capacity = 1;

            Assert.AreEqual(callbackCount + 2, callbacks.Count);
            Assert.AreEqual("cache:b", callbacks[callbackCount]);
            Assert.AreEqual("entry:b", callbacks[callbackCount + 1]);
            Assert.IsFalse(cache.TryRemove("b", out _));
            Assert.AreEqual(
                "value-c",
                cache.GetOrAdd("c", 1, () => throw new AssertionException("Current entry was reloaded.")));
        }

        [Test]
        public void ConstructorsRejectNegativeCapacity()
        {
            AssertArgumentOutOfRange(
                () => new LruCache<string, string>(-1, key => 1, key => key),
                "capacity");
            AssertArgumentOutOfRange(
                () => new LruCache<string, string>(-1),
                "capacity");
        }

        [Test]
        public void CapacityRejectsNegativeValueWithoutEvicting()
        {
            var deletes = new List<string>();
            var cache = CreateStringCache(1, deletes);

            Assert.AreEqual("value-a", cache.GetOrAdd("a", 1, () => "value-a"));

            AssertArgumentOutOfRange(() => cache.Capacity = -1, "value");
            Assert.AreEqual(1, cache.Capacity);
            CollectionAssert.IsEmpty(deletes);

            Assert.IsTrue(cache.TryRemove("a", out var value));
            Assert.AreEqual("value-a", value);
        }

        [Test]
        public void IndexerRejectsNegativeSizeWithoutEvicting()
        {
            var deletes = new List<string>();
            var readKeys = new List<string>();
            var cache = new LruCache<string, string>(
                1,
                key => key == "b" ? -1 : 1,
                key =>
                {
                    readKeys.Add(key);
                    return "loaded-" + key;
                },
                (key, value) => deletes.Add(key + ":" + value));

            Assert.AreEqual("loaded-a", cache["a"]);

            AssertArgumentOutOfRange(() => { var _ = cache["b"]; }, "sizeFun");
            CollectionAssert.AreEqual(new[] { "a" }, readKeys);
            CollectionAssert.IsEmpty(deletes);

            Assert.IsTrue(cache.TryRemove("a", out var value));
            Assert.AreEqual("loaded-a", value);
        }

        [Test]
        public void GetOrAddRejectsNegativeSizeWithoutEvicting()
        {
            var deletes = new List<string>();
            var valueFactoryCount = 0;
            var cache = CreateStringCache(1, deletes);

            Assert.AreEqual("value-a", cache.GetOrAdd("a", 1, () => "value-a"));

            AssertArgumentOutOfRange(
                () => cache.GetOrAdd("b", -1, () =>
                {
                    valueFactoryCount++;
                    return "value-b";
                }),
                "size");

            Assert.AreEqual(0, valueFactoryCount);
            CollectionAssert.IsEmpty(deletes);
            Assert.IsTrue(cache.TryRemove("a", out var value));
            Assert.AreEqual("value-a", value);
        }

        [Test]
        public void IndexerRejectsOversizedEntryWithoutEvicting()
        {
            var deletes = new List<string>();
            var readKeys = new List<string>();
            var cache = new LruCache<string, string>(
                1,
                key => key == "b" ? 2 : 1,
                key =>
                {
                    readKeys.Add(key);
                    return "loaded-" + key;
                },
                (key, value) => deletes.Add(key + ":" + value));

            Assert.AreEqual("loaded-a", cache["a"]);

            AssertArgumentOutOfRange(() => { var _ = cache["b"]; }, "sizeFun");
            CollectionAssert.AreEqual(new[] { "a" }, readKeys);
            CollectionAssert.IsEmpty(deletes);

            Assert.IsTrue(cache.TryRemove("a", out var value));
            Assert.AreEqual("loaded-a", value);
        }

        [Test]
        public void GetOrAddRejectsOversizedEntryWithoutEvicting()
        {
            var deletes = new List<string>();
            var valueFactoryCount = 0;
            var cache = CreateStringCache(1, deletes);

            Assert.AreEqual("value-a", cache.GetOrAdd("a", 1, () => "value-a"));

            AssertArgumentOutOfRange(
                () => cache.GetOrAdd("b", 2, () =>
                {
                    valueFactoryCount++;
                    return "value-b";
                }),
                "size");

            Assert.AreEqual(0, valueFactoryCount);
            CollectionAssert.IsEmpty(deletes);
            Assert.IsTrue(cache.TryRemove("a", out var value));
            Assert.AreEqual("value-a", value);
        }

        [Test]
        public void GetOrAddEvictsForLargeLogicalSizesWithoutOverflow()
        {
            var cache = new LruCache<string, string>(long.MaxValue);

            Assert.AreEqual("old-value", cache.GetOrAdd("old", long.MaxValue, () => "old-value"));
            Assert.AreEqual("new-value", cache.GetOrAdd("new", 1, () => "new-value"));

            Assert.IsFalse(cache.TryRemove("old", out var removed));
            Assert.AreEqual(default(string), removed);
            Assert.IsTrue(cache.TryRemove("new", out removed));
            Assert.AreEqual("new-value", removed);

            Assert.AreEqual("next-value", cache.GetOrAdd("next", long.MaxValue, () => "next-value"));
            Assert.IsTrue(cache.TryRemove("next", out removed));
            Assert.AreEqual("next-value", removed);
        }

        [Test]
        public void IndexerEvictsForLargeLogicalSizesWithoutOverflow()
        {
            var cache = new LruCache<string, string>(
                long.MaxValue,
                key => key == "old" ? long.MaxValue : 1,
                key => key + "-value");

            Assert.AreEqual("old-value", cache["old"]);
            Assert.AreEqual("new-value", cache["new"]);

            Assert.IsFalse(cache.TryRemove("old", out var removed));
            Assert.AreEqual(default(string), removed);
            Assert.IsTrue(cache.TryRemove("new", out removed));
            Assert.AreEqual("new-value", removed);

            Assert.AreEqual("next-value", cache["next"]);
            Assert.IsTrue(cache.TryRemove("next", out removed));
            Assert.AreEqual("next-value", removed);
        }
    }
}

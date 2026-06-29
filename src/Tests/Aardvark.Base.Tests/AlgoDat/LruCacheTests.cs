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
    }
}

using System.Collections.Generic;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class LruCacheTests
    {
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
    }
}

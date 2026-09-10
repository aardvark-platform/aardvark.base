using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aardvark.Tests.AlgoDat
{
    [TestFixture]
    public class SortedSetExtTests
    {
        [TestCase(0, false, 0, false, 0, true, 3)]
        [TestCase(3, false, 0, true, 3, true, 4)]
        [TestCase(5, true, 4, true, 5, false, 0)]
        [TestCase(8, true, 5, false, 0, false, 0)]
        public void InclusiveViewBounds(int query, bool hasLower, int lower, bool hasSelf, int self, bool hasUpper, int upper)
        {
            var parent = new SortedSetExt<int>(Enumerable.Range(1, 7));
            var view = parent.GetViewBetween(3, 5);
            Check(view, query, (hasLower, hasSelf, hasUpper), lower, self, upper);
        }

        [TestCase(false), TestCase(true)]
        public void EmptySingletonNestedAndMissingBoundaries(bool descending)
        {
            var comparer = Order(descending);
            var parent = new SortedSetExt<int>(new[] { -10, -4, 0, 4, 10 }, comparer);
            var outer = parent.GetViewBetween(descending ? 10 : -10, descending ? -10 : 10);
            var nested = outer.GetViewBetween(descending ? 5 : -5, descending ? -5 : 5);
            var empty = parent.GetViewBetween(descending ? 3 : 1, descending ? 1 : 3);
            var singleton = nested.GetViewBetween(0, 0);
            foreach (int query in new[] { -20, -5, -4, -1, 0, 1, 3, 4, 5, 20 })
            {
                CheckReference(nested, query, new[] { -4, 0, 4 }, comparer);
                CheckReference(empty, query, Array.Empty<int>(), comparer);
                CheckReference(singleton, query, new[] { 0 }, comparer);
            }
        }

        [TestCase(false), TestCase(true)]
        public void ParentMutationsAreVisibleWithoutRefreshingTheView(bool descending)
        {
            var comparer = Order(descending);
            var parent = new SortedSetExt<int>(Enumerable.Range(1, 7), comparer);
            var view = parent.GetViewBetween(descending ? 5 : 3, descending ? 3 : 5);
            void Verify(params int[] expected)
            {
                foreach (int query in new[] { 0, 3, 4, 5, 8 }) CheckReference(view, query, expected, comparer);
            }
            parent.Clear(); Verify();
            parent.Add(4); Verify(4);
            parent.Add(3); parent.Add(5); Verify(3, 4, 5);
            parent.Remove(4); Verify(3, 5);
            parent.UnionWith(Enumerable.Range(-200, 400)); Verify(3, 4, 5);
            parent.ExceptWith(new[] { 3, 4, 5 }); Verify();
            parent.Clear();
            parent.UnionWith(new[] { 0, 3, 5, 8 }); Verify(3, 5);
        }

        [TestCase(false), TestCase(true)]
        public void RefreshedBoundaryCacheTracksParentChanges(bool descending)
        {
            var comparer = Order(descending);
            var parent = new SortedSetExt<int>(Enumerable.Range(1, 7), comparer);
            var view = parent.GetViewBetween(descending ? 5 : 3, descending ? 3 : 5);

            parent.Remove(3);
            parent.Remove(5);
            Assert.That(view.Count, Is.EqualTo(1));
            foreach (int query in new[] { 0, 4, 8 }) CheckReference(view, query, new[] { 4 }, comparer);

            parent.Clear();
            Assert.That(view.Count, Is.Zero);
            foreach (int query in new[] { 0, 4, 8 }) CheckReference(view, query, Array.Empty<int>(), comparer);

            parent.UnionWith(new[] { 3, 5 });
            Assert.That(view.Count, Is.EqualTo(2));
            foreach (int query in new[] { 0, 3, 4, 5, 8 }) CheckReference(view, query, new[] { 3, 5 }, comparer);
        }

        private sealed class Item
        {
            public readonly int Key;
            public Item(int key) { Key = key; }
        }

        [TestCase(false), TestCase(true)]
        public void ComparerEqualQueriesReturnStoredReferences(bool descending)
        {
            var comparer = Comparer<Item>.Create((a, b) => descending ? b.Key.CompareTo(a.Key) : a.Key.CompareTo(b.Key));
            var items = Enumerable.Range(1, 7).Select(i => new Item(i)).ToArray();
            var parent = new SortedSetExt<Item>(items, comparer);
            var view = parent.GetViewBetween(new Item(descending ? 5 : 3), new Item(descending ? 3 : 5));
            var query = new Item(4);
            var flags = view.FindNeighboursV(query, out var lower, out var self, out var upper);
            Assert.That(flags, Is.EqualTo((true, true, true)));
            Assert.That(lower, Is.SameAs(items[descending ? 4 : 2]));
            Assert.That(self, Is.SameAs(items[3]));
            Assert.That(upper, Is.SameAs(items[descending ? 2 : 4]));
            view.FindNeighbours(query, out var ol, out var os, out var ou);
            Assert.That((ol.HasValue, os.HasValue, ou.HasValue), Is.EqualTo(flags));
            Assert.That(ol.Value, Is.SameAs(lower)); Assert.That(os.Value, Is.SameAs(self)); Assert.That(ou.Value, Is.SameAs(upper));
            Assert.That(view.TryFindSmaller(query, out var smaller), Is.True);
            Assert.That(smaller, Is.SameAs(lower));
            Assert.That(view.TryFindGreater(query, out var greater), Is.True);
            Assert.That(greater, Is.SameAs(upper));

            parent.Remove(items[3]);
            var replacement = new Item(4);
            parent.Add(replacement);
            view.FindNeighboursV(query, out _, out self, out _);
            Assert.That(self, Is.SameAs(replacement));
            var outside = new Item(descending ? 100 : -100);
            flags = view.FindNeighboursV(outside, out lower, out self, out upper);
            Assert.That(flags, Is.EqualTo((false, false, true)));
            Assert.That(lower, Is.Null); Assert.That(self, Is.Null);
            Assert.That(upper, Is.SameAs(items[descending ? 4 : 2]));
            parent.Clear();
            view.FindNeighbours(outside, out ol, out os, out ou);
            Assert.That((ol.HasValue, os.HasValue, ou.HasValue), Is.EqualTo((false, false, false)));
            Assert.That(ol.Value, Is.Null); Assert.That(os.Value, Is.Null); Assert.That(ou.Value, Is.Null);
        }

        [TestCase(7, false), TestCase(31, false), TestCase(2027, false)]
        [TestCase(7, true), TestCase(31, true), TestCase(2027, true)]
        public void SeededMutationsMatchFilteredSortedReference(int seed, bool descending)
        {
            var random = new Random(seed);
            var comparer = Order(descending);
            var parent = new SortedSetExt<int>(comparer);
            var reference = new SortedSet<int>(comparer);
            var view = parent.GetViewBetween(descending ? 40 : -40, descending ? -40 : 40);
            var nested = view.GetViewBetween(descending ? 17 : -13, descending ? -13 : 17);
            for (int i = 0; i < 2000; i++)
            {
                int value = random.Next(-80, 81);
                switch (i % 43)
                {
                    case 0: parent.Clear(); reference.Clear(); break;
                    case 1:
                        var values = Enumerable.Range(0, 32).Select(_ => random.Next(-80, 81)).ToArray();
                        parent.UnionWith(values); reference.UnionWith(values); break;
                    default:
                        if ((i & 1) == 0) { parent.Add(value); reference.Add(value); }
                        else { parent.Remove(value); reference.Remove(value); }
                        break;
                }
                int query = random.Next(-100, 101);
                CheckReference(parent, query, reference, comparer);
                CheckReference(view, query, reference.Where(x => x >= -40 && x <= 40), comparer);
                CheckReference(nested, query, reference.Where(x => x >= -13 && x <= 17), comparer);
            }
        }

        private sealed class CountingComparer : IComparer<int>
        {
            public int Comparisons;
            public int Compare(int a, int b) { Comparisons++; return a.CompareTo(b); }
        }

        [TestCase(1024), TestCase(65536)]
        public void StaleViewQueriesRemainLogarithmicWithoutRecounting(int count)
        {
            var comparer = new CountingComparer();
            var parent = new SortedSetExt<int>(Enumerable.Range(0, count), comparer);
            var view = parent.GetViewBetween(1, count - 2);
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var version = typeof(SortedSetExt<int>).GetField("_version", flags);
            var cachedCount = typeof(SortedSetExt<int>).GetField("_count", flags);
            var cachedRoot = typeof(SortedSetExt<int>).GetField("_root", flags);
            var before = (version.GetValue(view), cachedCount.GetValue(view), cachedRoot.GetValue(view));
            parent.Remove(count / 2); parent.Add(count + 1);
            foreach (int query in new[] { -1, 1, count / 2, count - 2, count + 2 })
            {
                comparer.Comparisons = 0;
                view.FindNeighboursV(query, out _, out _, out _);
                Assert.That(comparer.Comparisons, Is.LessThanOrEqualTo(6 * (int)Math.Ceiling(Math.Log(count + 1, 2)) + 12));
            }
            Assert.That(version.GetValue(view), Is.EqualTo(before.Item1), "No VersionCheck/recount");
            Assert.That(cachedCount.GetValue(view), Is.EqualTo(before.Item2));
            Assert.That(cachedRoot.GetValue(view), Is.SameAs(before.Item3));
            var found = view.FindNeighboursV(count / 2, out var l, out var s, out var u);
            Assert.That(found, Is.EqualTo((true, false, true)));
            Assert.That((l, s, u), Is.EqualTo((count / 2 - 1, 0, count / 2 + 1)));
        }

        [TestCase(false), TestCase(true)]
        public void ValueQueriesAllocateNothingAfterWarmup(bool bounded)
        {
            var parent = new SortedSetExt<int>(Enumerable.Range(0, 1024));
            var target = bounded ? parent.GetViewBetween(400, 600) : parent;
            RunValueQueries(target, 20000);
            parent.Remove(512); // Leave the view stale during measurement.
            long before = GC.GetAllocatedBytesForCurrentThread();
            int checksum = RunValueQueries(target, 20000);
            long bytes = GC.GetAllocatedBytesForCurrentThread() - before;
            Assert.That(checksum, Is.GreaterThan(0));
            Assert.That(bytes, Is.Zero);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int RunValueQueries(SortedSetExt<int> set, int count)
        {
            int checksum = 0;
            for (int i = 0; i < count; i++)
            {
                int q = i % 1200 - 100;
                var (hl, hs, hu) = set.FindNeighboursV(q, out var l, out var s, out var u);
                checksum += l + s + u + (hl ? 1 : 0) + (hs ? 1 : 0) + (hu ? 1 : 0);
                if (set.TryFindGreater(q, out var greater)) checksum += greater;
                if (set.TryFindSmaller(q, out var smaller)) checksum += smaller;
            }
            return checksum;
        }

        private static IComparer<int> Order(bool descending)
            => Comparer<int>.Create((a, b) => descending ? b.CompareTo(a) : a.CompareTo(b));

        private static void CheckReference(SortedSetExt<int> set, int query, IEnumerable<int> values, IComparer<int> comparer)
        {
            var sorted = values.OrderBy(x => x, comparer).ToArray();
            int lower = 0, self = 0, upper = 0;
            bool hl = false, hs = false, hu = false;
            foreach (int value in sorted)
            {
                int c = comparer.Compare(value, query);
                if (c < 0) { hl = true; lower = value; }
                else if (c == 0) { hs = true; self = value; }
                else if (!hu) { hu = true; upper = value; }
            }
            Check(set, query, (hl, hs, hu), lower, self, upper);
        }

        private static void Check(SortedSetExt<int> set, int query, (bool, bool, bool) expected, int lower, int self, int upper)
        {
            var flags = set.FindNeighboursV(query, out var l, out var s, out var u);
            Assert.That(flags, Is.EqualTo(expected), $"query={query}");
            Assert.That((l, s, u), Is.EqualTo((lower, self, upper)));
            set.FindNeighbours(query, out var ol, out var os, out var ou);
            Assert.That((ol.HasValue, os.HasValue, ou.HasValue), Is.EqualTo(expected));
            Assert.That((ol.Value, os.Value, ou.Value), Is.EqualTo((lower, self, upper)));
            Assert.That(set.TryFindSmaller(query, out var smaller), Is.EqualTo(expected.Item1));
            Assert.That(smaller, Is.EqualTo(lower));
            Assert.That(set.TryFindGreater(query, out var greater), Is.EqualTo(expected.Item3));
            Assert.That(greater, Is.EqualTo(upper));
        }
    }
}

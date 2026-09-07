using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FibonacciHeapTests
    {
        // Reflection keeps the implementation internal and avoids a production friend assembly.
        private sealed class Heap
        {
            private static readonly Type HeapType = typeof(ShortestPath<int>).Assembly
                .GetType("Aardvark.Base.FibonacciHeap`1", true).MakeGenericType(typeof(int));
            private static readonly Type NodeType = HeapType.GetMethod("DecreaseKey").GetParameters()[0].ParameterType;
            private static readonly MethodInfo InsertMethod = HeapType.GetMethod("Insert", new[] { typeof(float), typeof(int) });
            private static readonly MethodInfo ExtractMethod = HeapType.GetMethod("DeleteMin");
            private static readonly MethodInfo DecreaseMethod = HeapType.GetMethod("DecreaseKey");
            private readonly object _heap = Activator.CreateInstance(HeapType, true);

            public object Insert(float key, int value) => InsertMethod.Invoke(_heap, new object[] { key, value });
            public int Extract() => (int)ExtractMethod.Invoke(_heap, null);
            public void Decrease(object node, float key) => DecreaseMethod.Invoke(_heap, new object[] { node, key });
            public object Min => Field("_min");
            public object Field(string name) => HeapType.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_heap);
            public static T Get<T>(object node, string name) => (T)NodeType.GetProperty(name).GetValue(node);
            public static object Parent(object node) => Get<object>(node, "Parent");
            public static float Key(object node) => Get<float>(node, "Key");

            public void Validate(Dictionary<int, (object Node, float Key)> expected)
            {
                Assert.That((int)Field("_n"), Is.EqualTo(expected.Count), "Node count");
                Assert.That((Array)Field("_degreeTable"), Has.All.Null, "Scratch storage must not retain nodes");
                if (expected.Count == 0)
                {
                    Assert.That(Min, Is.Null);
                    return;
                }

                Assert.That(Min, Is.Not.Null);
                Assert.That(Key(Min), Is.EqualTo(expected.Values.Min(e => e.Key)), "Minimum over all roots");
                var seen = new HashSet<object>();
                var rings = new Stack<(object Start, object Parent)>();
                rings.Push((Min, null));
                while (rings.Count > 0)
                {
                    var (start, parent) = rings.Pop();
                    var node = start;
                    int degree = 0;
                    do
                    {
                        Assert.That(node, Is.Not.Null);
                        Assert.That(seen.Add(node), Is.True, "Node appears in multiple rings or a ring does not close");
                        int value = Get<int>(node, "Value");
                        Assert.That(expected.ContainsKey(value), Is.True, "Unexpected node");
                        Assert.That(node, Is.SameAs(expected[value].Node));
                        Assert.That(Key(node), Is.EqualTo(expected[value].Key));
                        Assert.That(Parent(node), Is.SameAs(parent), "Parent pointer");
                        Assert.That(Get<object>(Get<object>(node, "Right"), "Left"), Is.SameAs(node));
                        Assert.That(Get<object>(Get<object>(node, "Left"), "Right"), Is.SameAs(node));
                        if (parent != null)
                            Assert.That(Key(node), Is.GreaterThanOrEqualTo(Key(parent)), "Heap order");
                        else
                            Assert.That(Get<bool>(node, "Marked"), Is.False, "Roots must be unmarked");

                        var child = Get<object>(node, "Child");
                        if (child == null)
                            Assert.That(Get<int>(node, "Degree"), Is.Zero);
                        else
                            rings.Push((child, node));
                        degree++;
                        node = Get<object>(node, "Right");
                    } while (!ReferenceEquals(node, start));

                    if (parent != null)
                        Assert.That(Get<int>(parent, "Degree"), Is.EqualTo(degree), "Child count");
                }
                Assert.That(seen.Count, Is.EqualTo(expected.Count), "Reachable node count");
            }
        }

        [Test]
        public void ConsolidationIncludesUnlinkedRootsInMinimumSelection()
        {
            var heap = new Heap();
            foreach (int key in new[] { 0, 3, 1, 2 }) heap.Insert(key, key);
            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, Enumerable.Range(0, 4).Select(_ => heap.Extract()).ToArray());
        }

        [Test]
        public void DecreasedChildBecomesNextMinimum()
        {
            var heap = new Heap();
            object child = null;
            foreach (int key in new[] { 0, 10, 20, 30, 40 })
            {
                var node = heap.Insert(key, key);
                if (key == 20) child = node;
            }
            Assert.That(heap.Extract(), Is.Zero);
            Assert.That(Heap.Parent(child), Is.Not.Null);
            heap.Decrease(child, -1);
            Assert.That(heap.Extract(), Is.EqualTo(20));
        }

        [TestCase(7)]
        [TestCase(31)]
        [TestCase(101)]
        [TestCase(2027)]
        public void MixedOperationsMatchReferencePriorityModel(int seed)
        {
            var random = new Random(seed);
            var heap = new Heap();
            var expected = new Dictionary<int, (object Node, float Key)>();
            int nextId = 0;
            for (int step = 0; step < 2500; step++)
            {
                int operation = random.Next(100);
                if (expected.Count == 0 || operation < 42)
                {
                    float key = random.Next(-20, 101);
                    expected.Add(nextId, (heap.Insert(key, nextId), key));
                    nextId++;
                }
                else if (operation < 72)
                {
                    int id = expected.Keys.ElementAt(random.Next(expected.Count));
                    var entry = expected[id];
                    float key = entry.Key - random.Next(0, 41); // Includes equal-key decreases.
                    heap.Decrease(entry.Node, key);
                    expected[id] = (entry.Node, key);
                }
                else
                {
                    ExtractMinimum(heap, expected);
                }
                heap.Validate(expected);
            }
            while (expected.Count > 0)
            {
                ExtractMinimum(heap, expected);
                heap.Validate(expected);
            }
        }

        [Test]
        public void EqualKeysAndHeapReusePreserveEveryNode()
        {
            var heap = new Heap();
            var expected = new Dictionary<int, (object Node, float Key)>();
            foreach (int count in new[] { 1, 2, 3, 32, 129, 1025, 7 })
            {
                for (int id = 0; id < count; id++) expected.Add(id, (heap.Insert(1, id), 1));
                heap.Validate(expected);
                while (expected.Count > 0)
                {
                    ExtractMinimum(heap, expected);
                    heap.Validate(expected);
                }
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CutsMarkFirstLossAndCascadeThroughMarkedAncestors(bool markUpperFirst)
        {
            var heap = new Heap();
            var expected = new Dictionary<int, (object Node, float Key)>();
            for (int id = 0; id <= 256; id++) expected.Add(id, (heap.Insert(id, id), id));
            Assert.That(heap.Extract(), Is.Zero);
            expected.Remove(0);
            heap.Validate(expected);

            var lower = expected.Values.Select(e => e.Node).First(n =>
                Heap.Get<int>(n, "Degree") >= 2 && Heap.Parent(n) != null &&
                Heap.Parent(Heap.Parent(n)) != null);
            var upper = Heap.Parent(lower);
            var otherChild = expected.Values.Select(e => e.Node).First(n =>
                ReferenceEquals(Heap.Parent(n), upper) && !ReferenceEquals(n, lower));
            var children = expected.Values.Select(e => e.Node).Where(n => ReferenceEquals(Heap.Parent(n), lower)).Take(2).ToArray();

            if (markUpperFirst)
            {
                Decrease(otherChild, -1);
                Assert.That(Heap.Get<bool>(upper, "Marked"), Is.True);
            }
            Decrease(children[0], -2);
            Assert.That(Heap.Get<bool>(lower, "Marked"), Is.True);
            Decrease(children[1], -3);
            Assert.That(Heap.Parent(lower), Is.Null);
            Assert.That(Heap.Get<bool>(lower, "Marked"), Is.False);
            if (!markUpperFirst)
            {
                Assert.That(Heap.Parent(upper), Is.Not.Null, "Cascade stops at the first unmarked ancestor");
                Assert.That(Heap.Get<bool>(upper, "Marked"), Is.True, "That ancestor records its first lost child");
                Decrease(upper, -4); // Cutting an already marked node also clears its mark.
            }
            Assert.That(Heap.Parent(upper), Is.Null);
            Assert.That(Heap.Get<bool>(upper, "Marked"), Is.False);
            while (expected.Count > 0)
            {
                ExtractMinimum(heap, expected);
                heap.Validate(expected);
            }

            void Decrease(object node, float key)
            {
                heap.Decrease(node, key);
                expected[Heap.Get<int>(node, "Value")] = (node, key);
                heap.Validate(expected);
            }
        }

        [Test]
        public void EverySmallInsertionPermutationDrainsInOrder()
        {
            var keys = Enumerable.Range(0, 7).ToArray();
            Permute(0);

            void Permute(int start)
            {
                if (start == keys.Length)
                {
                    var heap = new Heap();
                    foreach (int key in keys) heap.Insert(key, key);
                    for (int key = 0; key < keys.Length; key++) Assert.That(heap.Extract(), Is.EqualTo(key));
                    return;
                }
                for (int i = start; i < keys.Length; i++)
                {
                    (keys[start], keys[i]) = (keys[i], keys[start]);
                    Permute(start + 1);
                    (keys[start], keys[i]) = (keys[i], keys[start]);
                }
            }
        }

        [Test]
        public void PromotedChildrenLoseMarksAndExtractedHandlesAreIsolated()
        {
            var heap = new Heap();
            var expected = new Dictionary<int, (object Node, float Key)>();
            for (int id = 0; id <= 256; id++) expected.Add(id, (heap.Insert(id, id), id));
            ExtractMinimum(heap, expected);
            var root = heap.Min;
            var parent = expected.Values.Select(e => e.Node).First(n =>
                ReferenceEquals(Heap.Parent(n), root) && Heap.Get<int>(n, "Degree") > 0);
            var child = Heap.Get<object>(parent, "Child");
            heap.Decrease(child, -1);
            expected[Heap.Get<int>(child, "Value")] = (child, -1);
            Assert.That(Heap.Get<bool>(parent, "Marked"), Is.True);
            heap.Decrease(root, -2);
            expected[Heap.Get<int>(root, "Value")] = (root, -2);
            ExtractMinimum(heap, expected);
            Assert.That(Heap.Get<bool>(parent, "Marked"), Is.False);
            Assert.That(Heap.Get<object>(root, "Child"), Is.Null);
            Assert.That(Heap.Get<int>(root, "Degree"), Is.Zero);
            Assert.That(Heap.Get<object>(root, "Left"), Is.SameAs(root));
            Assert.That(Heap.Get<object>(root, "Right"), Is.SameAs(root));
            heap.Validate(expected);
        }

        [Test]
        public void DegreeStorageGrowsIsClearedAndIsReusedAfterDraining()
        {
            var heap = new Heap();
            for (int i = 0; i <= 16; i++) heap.Insert(i, i);
            heap.Extract();
            var small = (Array)heap.Field("_degreeTable");
            Assert.That(small.Length, Is.GreaterThan(0));
            for (int i = 0; i < 16; i++) heap.Extract();

            for (int i = 0; i <= 4096; i++) heap.Insert(i, i);
            heap.Extract();
            var grown = (Array)heap.Field("_degreeTable");
            Assert.That(grown.Length, Is.GreaterThan(small.Length));
            for (int i = 0; i < 4096; i++)
            {
                heap.Extract();
                Assert.That((Array)heap.Field("_degreeTable"), Is.SameAs(grown));
                Assert.That(grown, Has.All.Null);
            }
            for (int i = 0; i <= 4096; i++) heap.Insert(i, i);
            heap.Extract();
            Assert.That((Array)heap.Field("_degreeTable"), Is.SameAs(grown));
            Assert.That(grown, Has.All.Null);
        }

        private static void ExtractMinimum(Heap heap, Dictionary<int, (object Node, float Key)> expected)
        {
            float minimum = expected.Values.Min(e => e.Key);
            int value = heap.Extract();
            Assert.That(expected.ContainsKey(value), Is.True);
            Assert.That(expected[value].Key, Is.EqualTo(minimum));
            expected.Remove(value);
        }
    }
}

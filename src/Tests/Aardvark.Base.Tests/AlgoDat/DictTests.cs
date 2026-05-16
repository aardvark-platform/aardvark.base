using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class DictTests : TestSuite
    {
        private sealed class CollisionKey : IEquatable<CollisionKey>
        {
            public int Id { get; }

            public CollisionKey(int id)
            {
                Id = id;
            }

            public bool Equals(CollisionKey other)
            {
                return other != null && Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                return obj is CollisionKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }

        public DictTests() : base() { }
        public DictTests(TestSuite.Options options) : base(options) { }

        string[] m_stringTable;
        string[] m_stringSubsetTable;
        Symbol[] m_symbolTable;
        Symbol[] m_symbolSubsetTable;

        [Test]
        public void TestDict()
        {
            CreateStringTable(50000, 5000);
            SymbolDictTestQuiet(5);
            ConcurrentSymbolDictTestQuiet(5, 100);
        }

        public void Run() // performance tests
        {
            int count = 1000000;
            // int growingRounds = 500;
            int preallocRounds = 5000;

            Report.Begin("Symbol Tests");

            CreateStringTable(count, count/50);

            // SymbolDictInitializationTest();
            // ConcurrentSymbolDictTest();
            // SymbolDictTest();

            foreach (var prealloc in new[] { false, true })
            {
                //StringTest(growingRounds, prealloc, false); DictStringTest(growingRounds, prealloc, false);
                //SymbolTest(growingRounds, prealloc, false); DictSymbolTest(growingRounds, prealloc, false);
                //SymbolDictTest(growingRounds, prealloc, false);

                StringTest(preallocRounds, prealloc, true); DictStringTest(preallocRounds, prealloc, true);
                SymbolTest(preallocRounds, prealloc, true); DictSymbolTest(preallocRounds, prealloc, true);
                SymbolDictTest(preallocRounds, prealloc, true);
            }
            Report.End();

        }

        public void CreateStringTable(int count, int subCount)
        {
            Report.BeginTimed("creating string table with {0} items", count);
            m_stringTable = new string[count];
            for (int i = 0; i < count; i++)
                m_stringTable[i] = Guid.NewGuid().ToString();
                                    //+ Guid.NewGuid().ToString() + Guid.NewGuid().ToString()
                                    //+ Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            Report.End();
            m_symbolTable = new Symbol[count];
            Report.BeginTimed("create symbols from strings");
            for (int i = 0; i < count; i++)
                m_symbolTable[i] = Symbol.Create(m_stringTable[i]);
            Report.End();
            Report.BeginTimed("create subset");
            var rnd = new RandomSystem(13);
            var perm = rnd.CreatePermutationArray(count);
            m_stringSubsetTable = new string[subCount];
            m_symbolSubsetTable = new Symbol[subCount];
            for (int i = 0; i < subCount; i++)
            {
                m_stringSubsetTable[i] = m_stringTable[perm[i]];
                m_symbolSubsetTable[i] = m_symbolTable[perm[i]];
            }
            Report.End();        
        }

        public void StringTest(int rounds, bool prealloc, bool subset)
        {
            var table = subset ? m_stringSubsetTable : m_stringTable;
            int count = table.Length;
            var map = prealloc ? new Dictionary<string, int>(count) : new Dictionary<string, int>();
            Report.Begin("Dictionary<string, int>" + (subset ? " subset" : "") +
                       " tests" + (prealloc ? " (prealloc)" : ""));
            Report.BeginTimed("create {0}", count);
            for (int i = 0; i < count; i++) map[table[i]] = i;
            Report.End();
            Test.Begin("access {0} x {1}", count, rounds);
            for (int j = 0; j < rounds; j++)
                for (int i = 0; i < count; i++)
                    Test.IsTrue(map[table[i]] == i);
            Test.End();
            Report.End();
        }

        public void SymbolTest(int rounds, bool prealloc, bool subset)
        {
            var table = subset ? m_symbolSubsetTable : m_symbolTable;
            int count = table.Length;
            var map = prealloc ? new Dictionary<Symbol, int>(count) : new Dictionary<Symbol, int>();
            Report.Begin("Dictionary<Symbol, int>" + (subset ? " subset" : "") +
                       " tests" + (prealloc ? " (prealloc)" : ""));
            Report.BeginTimed("create {0}", count);
            for (int i = 0; i < count; i++) map[table[i]] = i;
            Report.End();
            Test.Begin("access {0} x {1}", count, rounds);
            for (int j = 0; j < rounds; j++)
                for (int i = 0; i < count; i++)
                    Test.IsTrue(map[table[i]] == i);
            Test.End();
            Report.End();
        }

        public void DictStringTest(int rounds, bool prealloc, bool subset)
        {
            var table = subset ? m_stringSubsetTable : m_stringTable;
            int count = table.Length;
            Report.Begin("Dict<string, int>" + (subset ? " subset" : "") +
                       " tests" + (prealloc ? " (prealloc)" : ""));
            var map = prealloc ? new Dict<string, int>(count) : new Dict<string, int>();
            // map.Report = DictReport.Resize;
            Report.BeginTimed("create {0}", count);
            for (int i = 0; i < count; i++) map[table[i]] = i;
            Report.End();
            Test.Begin("access {0} x {1}", count, rounds);
            for (int j = 0; j < rounds; j++)
                for (int i = 0; i < count; i++)
                    Test.IsTrue(map[table[i]] == i);
            Test.End();
            Report.End();
        }

        public void DictSymbolTest(int rounds, bool prealloc, bool subset)
        {
            var table = subset ? m_symbolSubsetTable : m_symbolTable;
            int count = table.Length;
            var map = prealloc ? new Dict<Symbol, int>(count) : new Dict<Symbol, int>();
            // map.Report = DictReport.Resize;
            Report.Begin("Dict<Symbol, int>" + (subset ? " subset" : "") +
                       " tests" + (prealloc ? " (prealloc)" : ""));
            Report.BeginTimed("create {0}", count);
            for (int i = 0; i < count; i++) map[table[i]] = i;
            Report.End();
            Test.Begin("access {0} x {1}", count, rounds);
            for (int j = 0; j < rounds; j++)
                for (int i = 0; i < count; i++)
                    Test.IsTrue(map[table[i]] == i);
            Test.End();
            Report.End();
        }

        public void SymbolDictTest(int rounds, bool prealloc, bool subset)
        {
            var table = subset ? m_symbolSubsetTable : m_symbolTable;
            int count = table.Length;
            var map = prealloc ? new SymbolDict<int>(count) : new SymbolDict<int>();
            // map.Report = DictReport.Resize;
            Report.Begin("SymbolDict<int>" + (subset ? " subset" : "") +
                       " tests" + (prealloc ? " (prealloc)" : ""));
            Report.BeginTimed("create {0}", count);
            for (int i = 0; i < count; i++) map[table[i]] = i;
            Report.End();
            Test.Begin("access {0} x {1}", count, rounds);
            for (int j = 0; j < rounds; j++)
                for (int i = 0; i < count; i++)
                    Test.IsTrue(map[table[i]] == i);
            Test.End();
            Report.End();
        }

        public void SymbolDictInitializationTest()
        {
            var map = new SymbolDict<int>()
            {
                { "zero", 0 },
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 },
                { "ten", 10 },
            };

            foreach (var kvp in map)
            {
                Report.Line("{0}: {1}", kvp.Key, kvp.Value);
            }

            var set = new SymbolSet()
            {
                "zero", "one", "two", "three", "four", "five", "six", "seven", "eight",
                "nine", "ten",
            };

            foreach (var item in set)
            {
                Report.Line("{0}, ", item);
            }

        }

        public void ConcurrentSymbolDictTest(int rounds, int tasks)
        {
            int count = m_stringTable.Length;
            int block = count / tasks;

            var rnd = new RandomSystem();
            var perm = new int[count].SetByIndex(i => i);

            var map = new SymbolDict<int>().AsConcurrent();
            // var map = new FastConcurrentSymbolDict<int>();

            Test.Begin("parallel adding and removing tests {0}", count);
            for (int r = 0; r < rounds; r++)
            {
                Test.Begin("round {0}", r);
                Test.Begin("adding in parallel");
                Parallel.For(0, tasks, t =>
                {
                    for (int i = t * block, e = i + block; i < e; i++)
                        map[m_symbolTable[perm[i]]] = i;
                });
                Test.IsTrue(count == map.Count);
                Test.End();

                Test.Begin("checking");
                for (int i = 0; i < count; i++)
                    Test.IsTrue(map[m_symbolTable[perm[i]]] == i);
                Test.End();

                rnd.Randomize(perm);
                Test.Begin("removing in parallel");
                Parallel.For(0, tasks, t =>
                {
                    for (int i = t * block, e = i + block; i < e; i++)
                        map.Remove(m_symbolTable[perm[i]]);
                });
                Test.IsTrue(map.Count == 0);
                Test.End();
                Test.End();
            }
            Test.End();
        }

        private void ConcurrentSymbolDictTestQuiet(int rounds, int tasks)
        {
            int count = m_stringTable.Length;
            int block = count / tasks;

            var rnd = new RandomSystem();
            var perm = new int[count].SetByIndex(i => i);

            var map = new SymbolDict<int>().AsConcurrent();

            for (int r = 0; r < rounds; r++)
            {
                Parallel.For(0, tasks, t =>
                {
                    for (int i = t * block, e = i + block; i < e; i++)
                        map[m_symbolTable[perm[i]]] = i;
                });

                Assert.That(map.Count, Is.EqualTo(count), $"parallel add count mismatch in round {r}");

                for (int i = 0; i < count; i++)
                {
                    var key = m_symbolTable[perm[i]];
                    Assert.That(map[key], Is.EqualTo(i), $"parallel add/check mismatch in round {r} for key {key}");
                }

                rnd.Randomize(perm);
                Parallel.For(0, tasks, t =>
                {
                    for (int i = t * block, e = i + block; i < e; i++)
                        map.Remove(m_symbolTable[perm[i]]);
                });

                Assert.That(map.Count, Is.Zero, $"parallel remove count mismatch in round {r}");
            }
        }

        public void SymbolDictTest(int rounds)
        {
            int count = m_stringTable.Length;
            int block = count / 10;

            var rnd = new RandomSystem();
            var perm = new int[count].SetByIndex(i => i);

            var map = new SymbolDict<int>();

            Test.Begin("adding removing tests {0}", count);

            for (int r = 0; r < rounds; r++)
            {
                Test.Begin("round {0}", r);
                int added = 0;
                while (added < count)
                {
                    Test.Begin("adding 20%");
                    for (int i = added; i < added + 2 * block; i++)
                        map[m_symbolTable[perm[i]]] = i;
                    added += 2 * block;
                    Test.IsTrue(added == map.Count);
                    Test.End();

                    if (added == count) break;

                    Test.Begin("removing 10%");
                    for (int i = added - block; i < added; i++)
                    {
                        int index = 0;
                        var removed = map.TryRemove(m_symbolTable[perm[i]], out index);
                        Test.IsTrue(removed);
                        Test.IsTrue(index == i);
                    }
                    added -= block;
                    Test.IsTrue(added == map.Count);
                    Test.End();
                }
                Test.Begin("checking");
                for (int i = 0; i < count; i++)
                    Test.IsTrue(map[m_symbolTable[perm[i]]] == i);
                Test.IsTrue(count == map.Count);
                Test.End();
                added += 2 * block;
                rnd.Randomize(perm);
                Test.Begin("removing 100%");
                if ((r & 1) == 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int index = 0;
                        bool removed = map.TryRemove(m_symbolTable[perm[i]], out index);
                        Test.IsTrue(removed);
                    }
                }
                else
                    map.Clear();
                Test.IsTrue(map.Count == 0);
                Test.End();
                Test.End();
            }
            Test.End();
        }

        private void SymbolDictTestQuiet(int rounds)
        {
            int count = m_stringTable.Length;
            int block = count / 10;

            var rnd = new RandomSystem();
            var perm = new int[count].SetByIndex(i => i);

            var map = new SymbolDict<int>();

            for (int r = 0; r < rounds; r++)
            {
                int added = 0;
                while (added < count)
                {
                    for (int i = added; i < added + 2 * block; i++)
                        map[m_symbolTable[perm[i]]] = i;

                    added += 2 * block;
                    Assert.That(map.Count, Is.EqualTo(added), $"count mismatch after add phase in round {r}");

                    if (added == count) break;

                    for (int i = added - block; i < added; i++)
                    {
                        var key = m_symbolTable[perm[i]];
                        Assert.That(map.TryRemove(key, out var index), Is.True, $"remove failed in round {r} for key {key}");
                        Assert.That(index, Is.EqualTo(i), $"removed value mismatch in round {r} for key {key}");
                    }

                    added -= block;
                    Assert.That(map.Count, Is.EqualTo(added), $"count mismatch after partial remove in round {r}");
                }

                for (int i = 0; i < count; i++)
                {
                    var key = m_symbolTable[perm[i]];
                    Assert.That(map[key], Is.EqualTo(i), $"lookup mismatch in round {r} for key {key}");
                }

                Assert.That(map.Count, Is.EqualTo(count), $"full map count mismatch in round {r}");

                added += 2 * block;
                rnd.Randomize(perm);

                if ((r & 1) == 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var key = m_symbolTable[perm[i]];
                        Assert.That(map.TryRemove(key, out _), Is.True, $"final remove failed in round {r} for key {key}");
                    }
                }
                else
                {
                    map.Clear();
                }

                Assert.That(map.Count, Is.Zero, $"map not empty after cleanup in round {r}");
            }
        }

        // should work / works if there are no collisions and resizes / works with Dictionary / would also work if GetOrCreate would be implemented as extension using ContainsKey + Add
        // BUT it is implemented directly in Dict and at first makes the lookup with the given key and if not found invokes the createFunc 
        // it then does not check if the Dict has been modified during that and records the new entry to where it was previously tried be found -> location potentially invalid
        // NOTE: This is pattern is useful in recursive conversion (with cache) of graph like data structures
        [Test]
        public void TestDictGetCreate()
        {
            var dict = new Dict<string, string>();

            var rnd = new Random(0);

            for (int i = 0; i < 1000; i++)
            {
                var key = i.ToString();
                var test = dict.GetOrCreate(key, x =>
                {
                    var recKey = "rec - " + rnd.Next(0, 1000).ToString();
                    var testRec = dict.GetOrCreate(recKey, r => r);

                    Test.IsTrue(dict.ContainsKey(recKey), $"dict.ContainsKey({recKey})");
                    Test.IsTrue(dict[testRec] == recKey, $"dict[{testRec}] == {testRec}");

                    return x;
                });

                Test.IsTrue(dict.ContainsKey(key), $"dict.ContainsKey({key})");
                var foo = dict[test];
                Test.IsTrue(foo == key, $"dict[{test}] == {key}");
            }
        }

        // Explicitly enumerates the sequences
        public static IEnumerable<T> Enumerate<T>(IEnumerable<T> data)
        {
            var res = new List<T>();
            foreach (T item in data) { res.Add(item); }
            return res;
        }

        [Test]
        public void ContainsKeyValueForCollisionChainEntry()
        {
            var dict = new Dict<CollisionKey, string>();
            var firstKey = new CollisionKey(1);
            var secondKey = new CollisionKey(2);

            dict.Add(firstKey, "first");
            dict.Add(secondKey, "second");

            Assert.IsTrue(dict.Contains(firstKey, "first"));
            Assert.IsTrue(dict.Contains(secondKey, "second"));
            Assert.IsFalse(dict.Contains(secondKey, "first"));
        }

        [Test]
        public void ContainsKeyValueForStackedDuplicateKeyEntry()
        {
            var dict = new Dict<CollisionKey, string>(stackDuplicateKeys: true);
            var key = new CollisionKey(1);

            dict.Add(key, "first");
            dict.Add(key, "second");

            Assert.IsTrue(dict.Contains(key, "second"));
            Assert.IsTrue(dict.Contains(key, "first"));
            Assert.IsFalse(dict.Contains(key, "third"));
        }

        [Test]
        public void ContainsAndContainsValueSupportNullForOrdinaryStringEntries()
        {
            var dict = new Dict<string, string>();

            dict.Add("ordinary", null);

            Assert.IsTrue(dict.Contains("ordinary", null));
            Assert.IsTrue(dict.ContainsValue(null));
            Assert.IsFalse(dict.Contains("ordinary", "value"));
        }

        [Test]
        public void ContainsAndContainsValueSupportNullForCollisionChainStringEntries()
        {
            const int forcedHash = 1;
            var dict = new Dict<string, string>();

            dict.Add("first", forcedHash, "value");
            dict.Add("second", forcedHash, null);

            Assert.IsTrue(dict.Contains("second", forcedHash, null));
            Assert.IsTrue(dict.ContainsValue(null));
            Assert.IsFalse(dict.Contains("second", forcedHash, "value"));
        }

        [Test]
        public void ContainsAndContainsValueSupportNullForStackedDuplicateStringEntries()
        {
            var dict = new Dict<string, string>(stackDuplicateKeys: true);

            dict.Add("duplicate", "value");
            dict.Add("duplicate", null);

            Assert.IsTrue(dict.Contains("duplicate", null));
            Assert.IsTrue(dict.ContainsValue(null));
            Assert.IsFalse(dict.Contains("duplicate", "missing"));
        }

        [Test]
        public void SingleEntryDictSetterUpdatesExistingKeyWithoutThrowing()
        {
            var dict = new SingleEntryDict<string, int>("key", 1);

            Assert.DoesNotThrow(() => dict["key"] = 2);
            Assert.AreEqual(2, dict["key"]);
            Assert.Throws<ArgumentException>(() => dict["other"] = 3);
        }

        [Test]
        public void SingleEntryDictTryGetValueReportsPresentAndMissingKeys()
        {
            var dict = new SingleEntryDict<string, int>("key", 1);

            Assert.IsTrue(dict.TryGetValue("key", out var value));
            Assert.AreEqual(1, value);
            Assert.IsFalse(dict.TryGetValue("other", out value));
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void SingleEntryDictRemoveOnlyRemovesMatchingKey()
        {
            var dict = new SingleEntryDict<string, int>("key", 1);

            Assert.IsFalse(dict.Remove("other"));
            Assert.IsTrue(dict.ContainsKey("key"));

            Assert.IsTrue(dict.Remove("key"));
            Assert.IsFalse(dict.ContainsKey("key"));
            Assert.IsFalse(dict.TryGetValue("key", out var value));
            Assert.AreEqual(default(int), value);
            Assert.Throws<KeyNotFoundException>(() => { var _ = dict["key"]; });
            CollectionAssert.IsEmpty(dict.Keys);
            CollectionAssert.IsEmpty(dict.Values);
            CollectionAssert.IsEmpty(dict.KeyValuePairs);
        }

        [Test]
        public void SingleEntryDictAddRestoresRemovedEntryAndRejectsInvalidAdds()
        {
            var dict = new SingleEntryDict<string, int>("key", 1);

            Assert.Throws<ArgumentException>(() => dict.Add("key", 2));
            Assert.Throws<ArgumentException>(() => dict.Add("other", 2));

            Assert.IsTrue(dict.Remove("key"));
            Assert.DoesNotThrow(() => dict.Add("key", 3));
            Assert.IsTrue(dict.ContainsKey("key"));
            Assert.AreEqual(3, dict["key"]);
            CollectionAssert.AreEqual(new[] { "key" }, dict.Keys);
            CollectionAssert.AreEqual(new[] { 3 }, dict.Values);
        }

        [Test]
        public void SingleEntryDictSupportsNullKey()
        {
            var dict = new SingleEntryDict<string, int>(null, 1);

            Assert.IsTrue(dict.ContainsKey(null));
            Assert.IsTrue(dict.TryGetValue(null, out var value));
            Assert.AreEqual(1, value);
            Assert.IsTrue(dict.Remove(null));
            Assert.IsFalse(dict.ContainsKey(null));
            Assert.DoesNotThrow(() => dict[null] = 2);
            Assert.AreEqual(2, dict[null]);
        }

        [Test]
        public void SingleValueDictSetterAndAddAcceptSharedValueWithoutThrowing()
        {
            var dict = new SingleValueDict<string, int>(new DictSet<string>(), 7);

            Assert.DoesNotThrow(() => dict["first"] = 7);
            Assert.IsTrue(dict.ContainsKey("first"));
            Assert.AreEqual(7, dict["first"]);

            Assert.DoesNotThrow(() => dict.Add("second", 7));
            Assert.IsTrue(dict.ContainsKey("second"));
            Assert.AreEqual(7, dict["second"]);

            Assert.Throws<ArgumentException>(() => dict["third"] = 8);
            Assert.Throws<ArgumentException>(() => dict.Add("fourth", 8));
            Assert.IsFalse(dict.ContainsKey("third"));
            Assert.IsFalse(dict.ContainsKey("fourth"));
        }

        [Test]
        public void SingleValueDictValueOnlyConstructorCreatesUsableEmptyDict()
        {
            var dict = new SingleValueDict<string, int>(7);

            CollectionAssert.IsEmpty(dict.Keys);
            CollectionAssert.IsEmpty(dict.KeyValuePairs);
            Assert.IsFalse(dict.ContainsKey("missing"));
            Assert.IsFalse(dict.TryGetValue("missing", out var missing));
            Assert.AreEqual(default(int), missing);
            Assert.Throws<KeyNotFoundException>(() => { var _ = dict["missing"]; });

            Assert.DoesNotThrow(() => dict.Add("first", 7));
            Assert.DoesNotThrow(() => dict["second"] = 7);

            Assert.IsTrue(dict.ContainsKey("first"));
            Assert.IsTrue(dict.ContainsKey("second"));
            Assert.AreEqual(7, dict["first"]);
            Assert.AreEqual(7, dict["second"]);
            Assert.IsTrue(dict.TryGetValue("first", out var value));
            Assert.AreEqual(7, value);

            CollectionAssert.AreEquivalent(new[] { "first", "second" }, dict.Keys);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    new KeyValuePair<string, int>("first", 7),
                    new KeyValuePair<string, int>("second", 7),
                },
                dict.KeyValuePairs);

            Assert.Throws<ArgumentException>(() => dict.Add("third", 8));
            Assert.Throws<ArgumentException>(() => dict["third"] = 8);
            Assert.IsFalse(dict.ContainsKey("third"));

            Assert.IsTrue(dict.Remove("first"));
            Assert.IsFalse(dict.Remove("first"));
            Assert.IsFalse(dict.ContainsKey("first"));
            Assert.IsTrue(dict.ContainsKey("second"));
        }

        [Test]
        public void SingleValueSymbolDictValueOnlyConstructorCreatesUsableEmptyDict()
        {
            var dict = new SingleValueSymbolDict<int>(7);
            var first = Symbol.Create("single-value-symbol-dict-first");
            var second = Symbol.Create("single-value-symbol-dict-second");
            var third = Symbol.Create("single-value-symbol-dict-third");
            var fourth = Symbol.Create("single-value-symbol-dict-fourth");
            var missingKey = Symbol.Create("single-value-symbol-dict-missing");

            CollectionAssert.IsEmpty(dict.Keys);
            CollectionAssert.IsEmpty(dict.KeyValuePairs);
            Assert.IsFalse(dict.ContainsKey(missingKey));
            Assert.IsFalse(dict.TryGetValue(missingKey, out var missing));
            Assert.AreEqual(default(int), missing);
            Assert.Throws<KeyNotFoundException>(() => { var _ = dict[missingKey]; });

            Assert.DoesNotThrow(() => dict.Add(first, 7));
            Assert.DoesNotThrow(() => dict[second] = 7);

            Assert.IsTrue(dict.ContainsKey(first));
            Assert.IsTrue(dict.ContainsKey(second));
            Assert.AreEqual(7, dict[first]);
            Assert.AreEqual(7, dict[second]);
            Assert.IsTrue(dict.TryGetValue(first, out var value));
            Assert.AreEqual(7, value);

            CollectionAssert.AreEquivalent(new[] { first, second }, dict.Keys);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    new KeyValuePair<Symbol, int>(first, 7),
                    new KeyValuePair<Symbol, int>(second, 7),
                },
                dict.KeyValuePairs);

            Assert.Throws<ArgumentException>(() => dict.Add(third, 8));
            Assert.Throws<ArgumentException>(() => dict[fourth] = 8);
            Assert.IsFalse(dict.ContainsKey(third));
            Assert.IsFalse(dict.ContainsKey(fourth));

            Assert.IsTrue(dict.Remove(first));
            Assert.IsFalse(dict.Remove(first));
            Assert.IsFalse(dict.ContainsKey(first));
            Assert.IsTrue(dict.ContainsKey(second));
            CollectionAssert.AreEqual(new[] { second }, dict.Keys);
            CollectionAssert.AreEqual(new[] { new KeyValuePair<Symbol, int>(second, 7) }, dict.KeyValuePairs);
        }

        [Theory]
        public void ContainsValue(bool stackDuplicateKeys)
        {
            Dict<int, int> dict = new(stackDuplicateKeys)
            {
                {1, 2},
                {2, 3},
                {3, 4},
                {42, 5},
                {-13, 7},
            };

            if (stackDuplicateKeys)
            {
                dict.Add(42, 80);
                dict.Add(42, -80);
                Assert.IsTrue(dict.ContainsValue(-80));
            }

            var actualValues = dict.Values.ToArray();
            int[] expectedValues = stackDuplicateKeys ? [-80, 2, 3, 4, 5, 7, 80] : [ 2, 3, 4, 5, 7 ];
            Array.Sort(actualValues);

            Assert.IsTrue(dict.ContainsValue(7));
            Assert.IsFalse(dict.ContainsValue(6));
            Assert.AreEqual(expectedValues, actualValues);
        }

        [Theory]
        public void FastValuesEnumeratorMatchesValues(bool stackDuplicateKeys)
        {
            Dict<int, int> dict = new(stackDuplicateKeys)
            {
                {1, 2},
                {2, 3},
                {3, 4},
                {42, 5},
                {-13, 7},
            };

            if (stackDuplicateKeys)
            {
                dict.Add(42, 80);
                dict.Add(42, -80);
            }

            CollectionAssert.AreEqual(dict.Values.ToArray(), EnumerateFastValues(dict));
        }

        [Test]
        public void FastValuesEnumeratorEmpty()
        {
            var dict = new Dict<int, int>();
            var values = dict.GetValuesEnumerator();
            Assert.IsFalse(values.MoveNext());

            var symbolDict = new SymbolDict<int>();
            var symbolValues = symbolDict.GetValuesEnumerator();
            Assert.IsFalse(symbolValues.MoveNext());
        }

        [Test]
        public void SymbolDictFastValuesEnumeratorMatchesValues()
        {
            var dict = new SymbolDict<int>
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
            };

            CollectionAssert.AreEqual(dict.Values.ToArray(), EnumerateFastValues(dict));
        }

        [Theory]
        public void IDictionaryKeys(bool stackDuplicateKeys)
        {
            Dict<int, int> dict = new(stackDuplicateKeys)
            {
                {1, 2},
                {2, 3},
                {3, 4},
                {42, 5},
                {-13, 7},
            };

            int[] expectedKeys;

            if (stackDuplicateKeys)
            {
                dict.Add(42, 80);
                dict.Add(42, -80);
                expectedKeys = [-13, 1, 2, 3, 42, 42, 42];
            }
            else
            {
                expectedKeys = [-13, 1, 2, 3, 42];
            }

            var keys = ((IDictionary<int, int>)dict).Keys;
            var actualKeys = Enumerable.Order(Enumerate(keys));

            Assert.AreEqual(expectedKeys.Length, keys.Count);
            Assert.AreEqual(expectedKeys, actualKeys);
        }

        [Theory]
        public void IDictionaryValues(bool stackDuplicateKeys)
        {
            Dict<int, int> dict = new(stackDuplicateKeys)
            {
                {1, 2},
                {2, 3},
                {3, 4},
                {42, 5},
                {-13, 7},
            };

            int[] expectedValues;

            if (stackDuplicateKeys)
            {
                dict.Add(42, 80);
                dict.Add(42, -80);
                expectedValues = [-80, 2, 3, 4, 5, 7, 80];
            }
            else
            {
                expectedValues = [2, 3, 4, 5, 7];
            }

            var values = ((IDictionary<int, int>)dict).Values;
            var actualValues = Enumerable.Order(Enumerate(values));

            Assert.AreEqual(expectedValues.Length, values.Count);
            Assert.AreEqual(expectedValues, actualValues);
        }

        private static int[] EnumerateFastValues(Dict<int, int> dict)
        {
            var result = new List<int>();
            var values = dict.GetValuesEnumerator();

            while (values.MoveNext())
                result.Add(values.Current);

            return result.ToArray();
        }

        private static int[] EnumerateFastValues(SymbolDict<int> dict)
        {
            var result = new List<int>();
            var values = dict.GetValuesEnumerator();

            while (values.MoveNext())
                result.Add(values.Current);

            return result.ToArray();
        }
    }
}

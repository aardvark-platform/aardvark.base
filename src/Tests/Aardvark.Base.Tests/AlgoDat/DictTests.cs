using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class DictTests : TestSuite
    {
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
            SymbolDictTest(5);
            ConcurrentSymbolDictTest(5, 100);
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
            Report.Begin("SymolDict<int>" + (subset ? " subset" : "") +
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

        [Test, Ignore("should that even work?")]
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
    }
}

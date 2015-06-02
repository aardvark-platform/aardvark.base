using System;
using System.Collections.Generic;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ListHeapTests : TestSuite
    {
        public ListHeapTests() : base() { }
        public ListHeapTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestListHeap()
        {
            ListHeapTest(1 << 18, i => false);
            ListHeapRandomDequeueTest(1 << 18, i => false);
        }

        public void Run()
        {
            ListHeapTest(1 << 22, i => i <= 256);
            ListHeapRandomDequeueTest(1 << 22, i => i <= 256);
        }

        public void ListHeapTest(int maxCount, Func<int, bool> count_printFun)
        {
            Test.Begin("HeapTest");
            for (int i = 1; i <= maxCount; i *= 2)
                ListHeapTest(i, count_printFun(i));
            Test.End();
        }

        public void ListHeapTest(int count, bool print)
        {
            Test.Begin("heap size {0}", count);
            var rnd = new RandomSystem(count);
            Report.BeginTimed("creating array");
            var array = rnd.CreatePermutationArray(count);
            Report.End();
            var heap = new List<int>();

            Report.BeginTimed("enqueueing items");
            foreach (var item in array)
                heap.HeapAscendingEnqueue(item);
            Report.End();

            Test.Begin("dequeueing items");

            var pos = 0;
            if (print)
            {
                while (heap.Count > 0)
                {
                    var item = heap.HeapAscendingDequeue();
                    Test.IsTrue(pos == item, "item[{0}] = {1}", pos, item);
                    ++pos;
                    Report.Text("{0,3},", item);
                    if ((pos & 0xf) == 0) Report.Line();
                }
            }
            else
            {
                while (heap.Count > 0)
                {
                    var item = heap.HeapAscendingDequeue();
                    Test.IsTrue(pos == item, "item[{0}] = {1}", pos, item);
                    ++pos;
                }
            }
            Test.End();
            Test.End();
        }

        public void ListHeapRandomDequeueTest(int maxCount, Func<int, bool> count_printFun)
        {
            Report.BeginTimed("HeapRandomDequeueTest");
            for (int i = 1; i <= maxCount; i *= 2)
                ListHeapRandomDequeueTest(i, count_printFun(i));
            Report.End();
        }

        public void ListHeapRandomDequeueTest(int count, bool print)
        {
            Test.Begin("heap size {0}", count);
            var rnd = new RandomSystem(count);
            Report.BeginTimed("creating array");
            var array = rnd.CreatePermutationArray(count);
            Report.End();
            var heap = new List<int>();

            Report.BeginTimed("enqueueing items");
            foreach (var item in array)
                heap.HeapAscendingEnqueue(item);
            Report.End();

            Report.BeginTimed("dequeueing random items");

            var last = -1;
            int removed;
            for (removed = 0; last != 0; removed++)
                last = heap.HeapAscendingRemoveAt(rnd.UniformInt(heap.Count));

            Report.End(": {0}", removed);

            Test.Begin("dequeueing items");

            if (print)
            {
                int pos = 0;
                int old = -1;
                while (heap.Count > 0)
                {
                    var item = heap.HeapAscendingDequeue();
                    Test.IsTrue(old <= item, "wrong order: {0} > {1}", old, item);
                    old = item;
                    Report.Text("{0,3},", item);
                    if ((++pos & 0xf) == 0) Report.Line();
                }
            }
            else
            {
                int old = -1;
                while (heap.Count > 0)
                {
                    var item = heap.HeapAscendingDequeue();
                    Test.IsTrue(old <= item, "wrong order: {0} > {1}", old, item);
                    old = item;
                }
            }
            Test.End();
            Test.End();
        }
    }
}

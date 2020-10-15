using BenchmarkDotNet.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Aardvark.Base;
using System.Linq;
using BenchmarkDotNet.Jobs;
using System.Runtime.InteropServices;
using System.Collections;

namespace Aardvark.Base.Benchmarks
{
    //|                  Method | Count |        Mean |     Error |    StdDev |      Median |
    //|------------------------ |------ |------------:|----------:|----------:|------------:|
    //|            ForEachArray |     1 |   0.2825 ns | 0.0341 ns | 0.0379 ns |   0.2895 ns |
    //|             ForEachList |     1 |   5.4612 ns | 0.1341 ns | 0.1596 ns |   5.4791 ns |
    //|          ForEachHashSet |     1 |   5.6378 ns | 0.1343 ns | 0.1256 ns |   5.6246 ns |
    //| ForEachDictionaryValues |     1 |   6.0464 ns | 0.0992 ns | 0.0928 ns |   6.0032 ns |
    //|   ForEachDictionaryKeys |     1 |   6.0921 ns | 0.0758 ns | 0.0709 ns |   6.0765 ns |
    //|       ForEachDictionary |     1 |  22.6252 ns | 0.2217 ns | 0.2074 ns |  22.6664 ns |
    //|           ForEachIntSet |     1 |  17.9450 ns | 0.3709 ns | 0.5077 ns |
    //|       ForEachDictValues |     1 |  47.0700 ns | 0.9420 ns | 0.8810 ns |
    //|         ForEachDictKeys |     1 |  44.2800 ns | 0.2590 ns | 0.2420 ns |
    //|             ForEachDict |     1 |  31.9100 ns | 0.3090 ns | 0.2890 ns |
    //| ForEachSymbolDictValues |     1 |  46.0600 ns | 0.9700 ns | 1.1910 ns |
    //|            ForEachArray |    10 |   4.8354 ns | 0.0848 ns | 0.0793 ns |   4.8315 ns |
    //|             ForEachList |    10 |  25.4319 ns | 0.7352 ns | 2.1093 ns |  24.2291 ns |
    //|          ForEachHashSet |    10 |  31.0342 ns | 0.4605 ns | 0.4307 ns |  31.0219 ns |
    //| ForEachDictionaryValues |    10 |  28.0571 ns | 0.2260 ns | 0.1887 ns |  28.0309 ns |
    //|   ForEachDictionaryKeys |    10 |  27.8722 ns | 0.5367 ns | 0.5020 ns |  27.8217 ns |
    //|       ForEachDictionary |    10 |  81.7141 ns | 0.5350 ns | 0.5004 ns |  81.7748 ns |
    //|           ForEachIntSet |    10 |  28.5740 ns | 0.3391 ns | 0.3172 ns |
    //|       ForEachDictValues |    10 | 109.1000 ns | 0.9110 ns | 0.7600 ns |
    //|         ForEachDictKeys |    10 | 117.8300 ns | 2.3690 ns | 2.9960 ns |
    //|             ForEachDict |    10 |  97.1700 ns | 1.9020 ns | 1.6860 ns |
    //| ForEachSymbolDictValues |    10 | 111.6800 ns | 2.1870 ns | 2.6040 ns |
    //|            ForEachArray |   100 |  41.0627 ns | 0.3538 ns | 0.3309 ns |  41.0444 ns |
    //|             ForEachList |   100 | 229.5243 ns | 2.7209 ns | 2.4120 ns | 229.6100 ns |
    //|          ForEachHashSet |   100 | 284.1676 ns | 4.9822 ns | 4.6603 ns | 282.3070 ns |
    //| ForEachDictionaryValues |   100 | 244.8932 ns | 5.2041 ns | 4.6133 ns | 244.5696 ns |
    //|   ForEachDictionaryKeys |   100 | 260.9201 ns | 4.6225 ns | 4.3239 ns | 260.8970 ns |
    //|       ForEachDictionary |   100 | 687.7312 ns | 3.6999 ns | 3.4609 ns | 687.1103 ns |
    //|           ForEachIntSet |   100 | 302.3810 ns | 1.8455 ns | 1.7262 ns |
    //|       ForEachDictValues |   100 | 693.5000 ns |13.5200 ns |12.6500 ns |
    //|         ForEachDictKeys |   100 | 668.2000 ns |12.9200 ns |12.6800 ns |
    //|             ForEachDict |   100 | 692.1000 ns | 1.9000 ns | 1.6800 ns |
    //| ForEachSymbolDictValues |   100 | 632.1000 ns | 7.6100 ns | 7.1200 ns |

    [PlainExporter]
    public class Enumerators
    {
        int[] m_array;
        List<int> m_list;
        HashSet<int> m_hashSet;
        Dictionary<int, int> m_dictionary;
        IntSet m_intSet;
        Dict<int, int> m_dict;
        SymbolDict<int> m_symDict;

        [Params(100)]
        public int Count;

        [GlobalSetup]
        public void GlobalSetup()
        {
            m_array = new int[Count].SetByIndex(i => i + 1);
            m_list = new List<int>(1.UpTo(Count));
            m_hashSet = new HashSet<int>(1.UpTo(Count));
            m_dictionary = new Dictionary<int, int>(Count);
            for (int i = 1; i <= Count; i++)
                m_dictionary.Add(i, i);
            m_intSet = new IntSet(1.UpTo(Count));
            m_dict = new Dict<int, int>(Count);
            for (int i = 1; i <= Count; i++)
                m_dict.Add(i, i);
            m_symDict = new SymbolDict<int>(Count);
            for (int i = 1; i <= Count; i++)
                m_symDict.Add(i.ToString(), i);
        }

        [Benchmark]
        public int ForEachArray()
        {
            int sum = 0;
            foreach (var x in m_array)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachList()
        {
            int sum = 0;
            foreach (var x in m_list)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachHashSet()
        {
            int sum = 0;
            foreach (var x in m_hashSet)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachDictionaryValues()
        {
            int sum = 0;
            foreach (var x in m_dictionary.Values)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachDictionaryKeys()
        {
            int sum = 0;
            foreach (var x in m_dictionary.Keys)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachDictionary()
        {
            int sum = 0;
            foreach (var x in m_dictionary)
                sum += x.Value;
            return sum;
        }

        [Benchmark]
        public int ForEachIntSet()
        {
            int sum = 0;
            foreach (var x in m_intSet)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachDictValues()
        {
            int sum = 0;
            foreach (var x in m_dict.Values)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachDictKeys()
        {
            int sum = 0;
            foreach (var x in m_dict.Keys)
                sum += x;
            return sum;
        }

        [Benchmark]
        public int ForEachDict()
        {
            int sum = 0;
            foreach (var x in m_dict)
                sum += x.Value;
            return sum;
        }

        [Benchmark]
        public int ForEachSymbolDictValues()
        {
            int sum = 0;
            foreach (var x in m_symDict.Values)
                sum += x;
            return sum;
        }

        [Test]
        public void Test()
        {
            var rnd = new Random(1);
            for (int i = 0; i < 1000; i++)
            {
                Count = rnd.Next() & 0xfff;
                GlobalSetup();

                var refSum = ForEachArray();
                Assert.IsTrue(refSum == ForEachList());
                Assert.IsTrue(refSum == ForEachHashSet());
                Assert.IsTrue(refSum == ForEachDictionaryValues());
                Assert.IsTrue(refSum == ForEachDictionaryKeys());
                Assert.IsTrue(refSum == ForEachDictionary());
                Assert.IsTrue(refSum == ForEachIntSet());
                Assert.IsTrue(refSum == ForEachDictValues());
                Assert.IsTrue(refSum == ForEachDictKeys());
                Assert.IsTrue(refSum == ForEachDict());
                Assert.IsTrue(refSum == ForEachSymbolDictValues());
            }
        }
    }
}

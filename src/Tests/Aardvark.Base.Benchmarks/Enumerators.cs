using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    //|                  Method | Count |        Mean |     Error |    StdDev |      Median |
    //|------------------------ |------ |------------:|----------:|----------:|------------:|
    //|            ForEachArray |     1 |   0.2825 ns | 0.0341 ns | 0.0379 ns |   0.2895 ns |
    //|             ForEachList |     1 |   5.4612 ns | 0.1341 ns | 0.1596 ns |   5.4791 ns |
    //|          ForEachHashSet |     1 |   5.6378 ns | 0.1343 ns | 0.1256 ns |   5.6246 ns |
    //| ForEachDictionaryValues |     1 |   6.0464 ns | 0.0992 ns | 0.0928 ns |   6.0032 ns |
    //|    ForEachDictionaryKey |     1 |   6.0921 ns | 0.0758 ns | 0.0709 ns |   6.0765 ns |
    //|       ForEachDictionary |     1 |  22.6252 ns | 0.2217 ns | 0.2074 ns |  22.6664 ns |
    //|           ForEachIntSet |     1 |  43.9464 ns | 0.4670 ns | 0.3899 ns |
    //|       ForEachDictValues |     1 |  47.8454 ns | 0.8833 ns | 0.8263 ns |
    //|          ForEachDictKey |     1 |  47.7405 ns | 0.9538 ns | 1.5402 ns |
    //|             ForEachDict |     1 |  47.0618 ns | 0.7569 ns | 0.6321 ns |
    //|            ForEachArray |    10 |   4.8354 ns | 0.0848 ns | 0.0793 ns |   4.8315 ns |
    //|             ForEachList |    10 |  25.4319 ns | 0.7352 ns | 2.1093 ns |  24.2291 ns |
    //|          ForEachHashSet |    10 |  31.0342 ns | 0.4605 ns | 0.4307 ns |  31.0219 ns |
    //| ForEachDictionaryValues |    10 |  28.0571 ns | 0.2260 ns | 0.1887 ns |  28.0309 ns |
    //|    ForEachDictionaryKey |    10 |  27.8722 ns | 0.5367 ns | 0.5020 ns |  27.8217 ns |
    //|       ForEachDictionary |    10 |  81.7141 ns | 0.5350 ns | 0.5004 ns |  81.7748 ns |
    //|           ForEachIntSet |    10 |  84.1546 ns | 1.0811 ns | 1.0113 ns |
    //|       ForEachDictValues |    10 | 112.5240 ns | 1.4207 ns | 1.2594 ns |
    //|          ForEachDictKey |    10 | 119.0715 ns | 1.9915 ns | 1.7654 ns |
    //|             ForEachDict |    10 | 133.0506 ns | 1.1218 ns | 1.0493 ns |
    //|            ForEachArray |   100 |  41.0627 ns | 0.3538 ns | 0.3309 ns |  41.0444 ns |
    //|             ForEachList |   100 | 229.5243 ns | 2.7209 ns | 2.4120 ns | 229.6100 ns |
    //|          ForEachHashSet |   100 | 284.1676 ns | 4.9822 ns | 4.6603 ns | 282.3070 ns |
    //| ForEachDictionaryValues |   100 | 244.8932 ns | 5.2041 ns | 4.6133 ns | 244.5696 ns |
    //|    ForEachDictionaryKey |   100 | 260.9201 ns | 4.6225 ns | 4.3239 ns | 260.8970 ns |
    //|       ForEachDictionary |   100 | 687.7312 ns | 3.6999 ns | 3.4609 ns | 687.1103 ns |
    //|           ForEachIntSet |   100 | 667.4754 ns | 6.9928 ns | 6.1989 ns |
    //|       ForEachDictValues |   100 | 647.8472 ns | 5.7402 ns | 5.3694 ns |
    //|          ForEachDictKey |   100 | 661.1481 ns | 9.0808 ns | 8.4941 ns |
    //|             ForEachDict |   100 | 878.1257 ns | 9.0881 ns | 8.0564 ns |
    public class Enumerators
    {
        int[] m_array;
        List<int> m_list;
        HashSet<int> m_hashSet;
        Dictionary<int, int> m_dictionary;
        IntSet m_intSet;
        Dict<int, int> m_dict;

        [Params(1, 10, 100)]
        public int Count;

        [GlobalSetup]
        public void GlobalSetup()
        {
            m_array = new int[Count].SetByIndex(i => i + 1);
            m_list = new List<int>(1.UpTo(Count));
            m_hashSet = new HashSet<int>(1.UpTo(Count));
            m_dictionary = new Dictionary<int, int>(Count);
            for (int i = 0; i < Count; i++)
                m_dictionary.Add(i, i);
            m_intSet = new IntSet(1.UpTo(Count));
            m_dict = new Dict<int, int>(Count);
            for (int i = 0; i < Count; i++)
                m_dict.Add(i, i);
        }

        [Benchmark]
        public int ForEachArray()
        {
            int sum = 0;
            foreach (var x in m_array)
                sum += x;
            return sum;
        }

        //[Benchmark]
        //public int ForEachList()
        //{
        //    int sum = 0;
        //    foreach (var x in m_list)
        //        sum += x;
        //    return sum;
        //}

        //[Benchmark]
        //public int ForEachHashSet()
        //{
        //    int sum = 0;
        //    foreach (var x in m_hashSet)
        //        sum += x;
        //    return sum;
        //}

        //[Benchmark]
        //public int ForEachDictionaryValues()
        //{
        //    int sum = 0;
        //    foreach (var x in m_dictionary.Values)
        //        sum += x;
        //    return sum;
        //}

        //[Benchmark]
        //public int ForEachDictionaryKey()
        //{
        //    int sum = 0;
        //    foreach (var x in m_dictionary.Keys)
        //        sum += x;
        //    return sum;
        //}

        //[Benchmark]
        //public int ForEachDictionary()
        //{
        //    int sum = 0;
        //    foreach (var x in m_dictionary)
        //        sum += x.Value;
        //    return sum;
        //}

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
        public int ForEachDictKey()
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
    }
}

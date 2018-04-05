using Aardvark.Base;
using Aardvark.Base.Sorting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class SortingTests : TestSuite
    {
        public SortingTests() : base() { }
        public SortingTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestSorting()
        {
            SortTests(1 << 10);
            MedianTests(1 << 10);
        }

        public void Run()
        {
            DetailedSortTests(      // too long for N-Unit, just for performance measurements
                    1 << 20, // 26
                    1 << 4,
                    4.0);
        }

        public void MedianTests(int maxCount)
        {
            Test.Begin("median tests");
            foreach (var pm in new[] { false, true })
            {
                for (int count = 1; count <= maxCount; count *= 2)
                {
                    var rnd = new RandomSystem(count);
                    var array = rnd.CreatePermutationArray(count);
                    var a = new int[count];

                    Test.Begin("{0}quick-median {1} items", pm ? "permutation " : "", count);
                    if (pm)
                        for (int i = 0; i < a.Length; i++)
                        {
                            a.SetByIndex(pi => pi);
                            a.PermutationQuickMedianAscending(array, i);
                            Test.IsTrue(array[a[i]] == i);
                        }
                    else
                        for (int i = 0; i < a.Length; i++)
                        {
                            array.CopyTo(a, 0);
                            a.QuickMedianAscending(i);
                            Test.IsTrue(a[i] == i);
                        }
                    Test.End();
                }
            }
            Test.End();
        }

        public void SortTests(int maxCount)
        {
            var sortDict = new Dict<string, Action<int[]>>
            {
                { "heap", SortingExtensions.HeapSortAscending },
                { "quick", SortingExtensions.QuickSortAscending },
                { "smooth", SortingExtensions.SmoothSortAscending },
                { "tim", SortingExtensions.TimSortAscending },
            };

            var permSortDict = new Dict<string, Action<int[],int[]>>
            {
                { "heap", SortingExtensions.PermutationHeapSortAscending },
                { "quick", SortingExtensions.PermutationQuickSortAscending },
                { "smooth", SortingExtensions.PermutationSmoothSortAscending },
                { "tim", SortingExtensions.PermutationTimSortAscending },
            };

            Test.Begin("sort tests");
            foreach (var pm in new[] { false, true })
            {
                for (int count = 1; count <= maxCount; count *= 2)
                {
                    var rnd = new RandomSystem(count);
                    var array = rnd.CreatePermutationArray(count);
                    var a = new int[count];

                    foreach (var sort in new[] { "heap", "quick", "smooth", "tim" })
                    {
                        Test.Begin("{0}{1}-sort {2} items", pm ? "permutation " : "", sort, count);
                        if (pm)
                        {
                            a.SetByIndex(pi => pi);
                            permSortDict[sort](a, array);
                            for (int i = 0; i < count; i++)
                                Test.IsTrue(array[a[i]] == i);
                        }
                        else
                        {
                            array.CopyTo(a, 0);
                            sortDict[sort](a);
                            for (int i = 0; i < count; i++)
                                Test.IsTrue(a[i] == i);
                        }
                        Test.End();
                    }
                }
            }
            Test.End();
        }

        public abstract class Sorter
        {
            public Symbol Name;
            public Type Type;
            public Type PermType;
            public bool UseCmp;
            public abstract object SortFunction { get; }
            public Sorter(Symbol name, Type type, Type permType, bool useCmp)
            { Name = name; Type = type; PermType = permType; UseCmp = useCmp; }
        }

        public class SimpleSorter<T> : Sorter
        {
            public Action<T[], long, long> Sort;
            public SimpleSorter(Symbol name, Action<T[], long, long> sort)
                : base(name, typeof(T), null, false)
            { Sort = sort; }
            public override object SortFunction  { get { return Sort; } }
        }

        public class FunSorter<T> : Sorter
        {
            public Action<T[], Func<T, T, int>, long, long> Sort;
            public FunSorter(Symbol name, Action<T[], Func<T, T, int>, long, long> sort)
                : base(name, typeof(T), null, true)
            { Sort = sort; }
            public override object SortFunction { get { return Sort; } }
        }

        public class PermSorter<Tp, T> : Sorter
        {
            public Action<Tp[], T[], Tp, Tp> Sort;
            public PermSorter(Symbol name, Action<Tp[], T[], Tp, Tp> sort)
                : base(name, typeof(T), typeof(Tp), false)
            { Sort = sort; }
            public override object SortFunction { get { return Sort; } }
        }

        public class PermFunSorter<Tp, T> : Sorter
        {
            public Action<Tp[], T[], Func<T, T, int>, Tp, Tp> Sort;
            public PermFunSorter(Symbol name, Action<Tp[], T[], Func<T, T, int>, Tp, Tp> sort)
                : base(name, typeof(T), typeof(Tp), true)
            { Sort = sort; }
            public override object SortFunction { get { return Sort; } }
        }

        public abstract class SortArray
        {
            public Type Type;
            public Type PermType;
            public bool UseCmp;

            public SortArray(Type type, Type permType) { Type = type; PermType = permType; }
            public abstract void CopyFrom(long[] master, long count);
            public abstract void Sort(object sortFunction, long count, long repeat);
            public abstract void Test(Func<bool, bool> testFunction, long count, long repeat);

        }

        public class SortArray<T> : SortArray
        {
            public Func<long, T> TofLong;
            public T[] Array;
            public Func<T, T, int> Cmp;

            public SortArray(long count, Func<long, T> tOfLong, Func<T, T, int> cmp)
                : base(typeof(T), null)
            { TofLong = tOfLong; Array = new T[count]; Cmp = cmp; UseCmp = false; }

            public override void CopyFrom(long[] master, long count)
            {
                for (long i = 0; i < count; i++) Array[i] = TofLong(master[i]);
            }

            public override void Sort(object sortFunction, long count, long repeat)
            {
                var typedSortFunction = (Action<T[], long, long>)sortFunction;
                for (long start = 0, end = count; repeat-- > 0; start = end, end += count)
                    typedSortFunction(Array, start, end);
            }

            public override void Test(Func<bool, bool> testFunction, long count, long repeat)
            {
                long fails = 0;
                for (long start = 0, end = count; repeat-- > 0; start = end, end += count)
                {
                    var last = Array[start];
                    for (var i = start + 1; i < end; i++)
                    {
                        var item = Array[i];
                        var check = Cmp(last, item) <= 0;
                        if (!check)
                            ++fails;
                        last = item;
                    }
                }
                testFunction(fails == 0);
            }
        }

        public class FunSortArray<T> : SortArray<T>
        {
            public FunSortArray(long count, Func<long, T> tOfLong, Func<T, T, int> cmp)
                : base(count, tOfLong, cmp)
            { UseCmp = true; }

            public override void Sort(object sortFunction, long count, long repeat)
            {
                var typedSortFunction = (Action<T[], Func<T,T,int>, long, long>)sortFunction;
                for (long start = 0, end = count; repeat-- > 0; start = end, end += count)
                    typedSortFunction(Array, Cmp, start, end);
            }
        }

        public class PermSortArray<Tp, T> : SortArray
        {
            public Func<long, T> TOfLong;
            public Func<long, Tp> TpOfLong;
            public Func<Tp, long> LongOfTp;
            public Tp[] PermArray;
            public T[] Array;
            public Func<T, T, int> Cmp;

            public PermSortArray(long count, Func<long, Tp> tpOfLong, Func<Tp, long> longOfTp,
                    Func<long, T> tOfLong, Func<T, T, int> cmp)
                : base(typeof(T), typeof(Tp))
            {
                TOfLong = tOfLong; TpOfLong = tpOfLong; LongOfTp = longOfTp;
                PermArray = new Tp[count]; Array = new T[count];
                Cmp = cmp; UseCmp = false;
            }

            public override void CopyFrom(long[] master, long count)
            {
                for (long i = 0; i < count; i++) { Array[i] = TOfLong(master[i]); PermArray[i] = TpOfLong(i); }
            }

            public override void Sort(object sortFunction, long count, long repeat)
            {
                var typedSortFunction = (Action<Tp[], T[], Tp, Tp>)sortFunction;
                for (long start = 0, end = count; repeat-- > 0; start = end, end += count)
                    typedSortFunction(PermArray, Array, TpOfLong(start), TpOfLong(end));
            }

            public override void Test(Func<bool, bool> testFunction, long count, long repeat)
            {
                long fails = 0;
                for (long start = 0, end = count; repeat-- > 0; start = end, end += count)
                {
                    var last = Array[LongOfTp(PermArray[start])];
                    for (var i = start; i < end; i++)
                    {
                        var item = Array[LongOfTp(PermArray[i])];
                        var check = Cmp(last, item) <= 0;
                        if (!check)
                            ++fails;
                        last = item;
                    }
                }
                testFunction(fails == 0);
            }
        }

        public class PermFunSortArray<Tp, T> : PermSortArray<Tp, T>
        {
            public PermFunSortArray(long count, Func<long, Tp> tpOfLong, Func<Tp, long> longOfTp,
                    Func<long, T> tOfLong, Func<T, T, int> cmp)
                : base(count, tpOfLong, longOfTp, tOfLong, cmp)
            { UseCmp = true; }

            public override void Sort(object sortFunction, long count, long repeat)
            {
                var typedSortFunction = (Action<Tp[], T[], Func<T, T, int>, Tp, Tp>)sortFunction;
                for (long start = 0, end = count; repeat-- > 0; start = end, end += count)
                    typedSortFunction(PermArray, Array, Cmp, TpOfLong(start), TpOfLong(end));
            }
        }

        public class Initializer
        {
            public string Name;
            public Action<long[], long, long, long> Init;
            public Initializer(string name, Action<long[], long, long, long> init) { Name = name; Init = init; }
        }

        public static readonly Symbol QuickSort = "quick";
        public static readonly Symbol HeapSort = "heap";
        public static readonly Symbol SmoothSort = "smooth";
        public static readonly Symbol TimSort = "tim";

        static readonly Func<V3d, V3d, int> V3dCmp = (a, b) => a.Z < b.Z ? -1 : a.Z > b.Z ? 1 : 0;
        static readonly Func<V2d, V2d, int> V2dCmp = (a, b) => a.Y < b.Y ? -1 : a.Y > b.Y ? 1 : 0;
        static readonly Func<double, double, int> DCmp = (a, b) => a < b ? -1 : a > b ? 1 : 0;
        static readonly Func<float, float, int> FCmp = (a, b) => a < b ? -1 : a > b ? 1 : 0;
        static readonly Func<long, long, int> LCmp = (a, b) => a < b ? -1 : a > b ? 1 : 0;
        static readonly Func<int, int, int> ICmp = (a, b) => a < b ? -1 : a > b ? 1 : 0;

        public void DetailedSortTests(int arrayCount, int minimalCount, double countFactor)
        {
            // re-order this array to see certain results first
            var arrayCreators = new Func<long, SortArray>[]
            {
                c => new SortArray<int>(c, i => (int)i, ICmp),
                c => new SortArray<long>(c, i => i, LCmp),
                c => new SortArray<float>(c, i => i, FCmp),
                c => new SortArray<double>(c, i => i, DCmp),
                c => new FunSortArray<V2d>(c, i => new V2d(0,i), V2dCmp),
                c => new FunSortArray<V3d>(c, i => new V3d(0,0,i), V3dCmp),

                c => new PermSortArray<int, int>(c, i => (int)i, i => i, i => (int)i, ICmp),
                c => new PermSortArray<int, long>(c, i => (int)i, i => i, i => i, LCmp),
                c => new PermSortArray<int, float>(c, i => (int)i, i => i, i => i, FCmp),
                c => new PermSortArray<int, double>(c, i => (int)i, i => i, i => i, DCmp),
                c => new PermFunSortArray<int, V2d>(c, i => (int)i, i => i, i => new V2d(0,i), V2dCmp),
                c => new PermFunSortArray<int, V3d>(c, i => (int)i, i => i, i => new V3d(0,0,i), V3dCmp),                

                c => new PermSortArray<long, int>(c, i => i, i => i, i => (int)i, ICmp),
                c => new PermSortArray<long, long>(c, i => i, i => i, i => i, LCmp),
                c => new PermSortArray<long, float>(c, i => i, i => i, i => i, FCmp),
                c => new PermSortArray<long, double>(c, i => i, i => i, i => i, DCmp),
                c => new PermFunSortArray<long, V2d>(c, i => i, i => i, i => new V2d(0,i), V2dCmp),
                c => new PermFunSortArray<long, V3d>(c, i => i, i => i, i => new V3d(0,0,i), V3dCmp),
            };

            const int seed = 19680713;

            Action<long[], long, long, long, double> randomFraction = (a, count, repeat, total, fraction) =>
            {
                var rc = (long)(count * fraction);
                var rnd = new RandomSystem(seed);
                for (long s = 0; s < total; s += count)
                {
                    for (long r = rc; r > 0; r--)
                    {
                        long i = -1; do { i = s + rnd.UniformLong(count); } while (a[i] < 0);
                        long j = -1; do { j = s + rnd.UniformLong(count); } while (j == i);
                        var aj = a[j]; if (aj >= 0) { --r; aj = -1 - aj; }
                        var ai = -1 - a[i]; a[i] = aj; a[j] = ai;
                    }
                }
                a.Apply(v => v < 0 ? -1 - v : v);
            };

            // re-order this array to see certain results first
            var initializers = new Initializer[]
            {
                new Initializer("randomized", (a, c, r, t) =>
                    { a.SetByIndexLong(t, i => i); new RandomSystem(seed).Randomize(a, t); }),
                new Initializer("sorted", (a, c, r, t) => a.SetByIndexLong(t, i => i)),
                new Initializer("sorted, 1% random", (a, c, r, t) =>
                    { a.SetByIndexLong(t, i => i); randomFraction(a, c, r, t, 0.01); }),
                new Initializer("sorted, 10% random", (a, c, r, t) =>
                    { a.SetByIndexLong(t, i => i); randomFraction(a, c, r, t, 0.1); }),
                new Initializer("sorted, 30% random", (a, c, r, t) =>
                    { a.SetByIndexLong(t, i => i); randomFraction(a, c, r, t, 0.3); }),
                new Initializer("reversed", (a, c, r, t) => a.SetByIndexLong(t, i => t - 1 - i)),
                new Initializer("reversed, 1% random", (a, c, r, t) =>
                    { a.SetByIndexLong(t, i => t - 1 - i); randomFraction(a, c, r, t, 0.01); }),
                new Initializer("reversed, 10% random", (a, c, r, t) =>
                    { a.SetByIndexLong(t, i => t - 1 - i); randomFraction(a, c, r, t, 0.1); }),
                new Initializer("reversed, 30% random", (a, c, r, t) =>
                    { a.SetByIndexLong(t, i => t - 1 - i); randomFraction(a, c, r, t, 0.3); }),
                new Initializer("all equal", (a, c, r, t) => a.Set(t, 0)),
            };

            var sortNames = new Symbol[] { HeapSort, QuickSort, SmoothSort, TimSort };
            var sorters = new Sorter[]
            {
                new SimpleSorter<int>       (HeapSort,      SortingExtensions.HeapSortAscending),
                new SimpleSorter<long>      (HeapSort,      SortingExtensions.HeapSortAscending),
                new SimpleSorter<float>     (HeapSort,      SortingExtensions.HeapSortAscending),
                new SimpleSorter<double>    (HeapSort,      SortingExtensions.HeapSortAscending),
                new FunSorter<V2d>          (HeapSort,      SortingExtensions.HeapSort),
                new FunSorter<V3d>          (HeapSort,      SortingExtensions.HeapSort),

                new PermSorter<int, int>        (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermSorter<int, long>       (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermSorter<int, float>      (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermSorter<int, double>     (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermFunSorter<int, V2d>     (HeapSort,      SortingExtensions.PermutationHeapSort),
                new PermFunSorter<int, V3d>     (HeapSort,      SortingExtensions.PermutationHeapSort),

                new PermSorter<long, int>       (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermSorter<long, long>      (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermSorter<long, float>     (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermSorter<long, double>    (HeapSort,      SortingExtensions.PermutationHeapSortAscending),
                new PermFunSorter<long, V2d>    (HeapSort,      SortingExtensions.PermutationHeapSort),
                new PermFunSorter<long, V3d>    (HeapSort,      SortingExtensions.PermutationHeapSort),

                new SimpleSorter<int>       (QuickSort,     SortingExtensions.QuickSortAscending),
                new SimpleSorter<long>      (QuickSort,     SortingExtensions.QuickSortAscending),
                new SimpleSorter<float>     (QuickSort,     SortingExtensions.QuickSortAscending),
                new SimpleSorter<double>    (QuickSort,     SortingExtensions.QuickSortAscending),
                new FunSorter<V2d>          (QuickSort,     SortingExtensions.QuickSort),
                new FunSorter<V3d>          (QuickSort,     SortingExtensions.QuickSort),

                new PermSorter<int, int>        (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermSorter<int, long>       (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermSorter<int, float>      (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermSorter<int, double>     (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermFunSorter<int, V2d>     (QuickSort,     SortingExtensions.PermutationQuickSort),
                new PermFunSorter<int, V3d>     (QuickSort,     SortingExtensions.PermutationQuickSort),

                new PermSorter<long, int>       (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermSorter<long, long>      (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermSorter<long, float>     (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermSorter<long, double>    (QuickSort,     SortingExtensions.PermutationQuickSortAscending),
                new PermFunSorter<long, V2d>    (QuickSort,     SortingExtensions.PermutationQuickSort),
                new PermFunSorter<long, V3d>    (QuickSort,     SortingExtensions.PermutationQuickSort),

                new SimpleSorter<int>       (SmoothSort,    SortingExtensions.SmoothSortAscending),
                new SimpleSorter<long>      (SmoothSort,    SortingExtensions.SmoothSortAscending),
                new SimpleSorter<float>     (SmoothSort,    SortingExtensions.SmoothSortAscending),
                new SimpleSorter<double>    (SmoothSort,    SortingExtensions.SmoothSortAscending),
                new FunSorter<V2d>          (SmoothSort,    SortingExtensions.SmoothSort),
                new FunSorter<V3d>          (SmoothSort,    SortingExtensions.SmoothSort),

                new PermSorter<int, int>        (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermSorter<int, long>       (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermSorter<int, float>      (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermSorter<int, double>     (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermFunSorter<int, V2d>     (SmoothSort,    SortingExtensions.PermutationSmoothSort),
                new PermFunSorter<int, V3d>     (SmoothSort,    SortingExtensions.PermutationSmoothSort),

                new PermSorter<long, int>       (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermSorter<long, long>      (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermSorter<long, float>     (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermSorter<long, double>    (SmoothSort,    SortingExtensions.PermutationSmoothSortAscending),
                new PermFunSorter<long, V2d>    (SmoothSort,    SortingExtensions.PermutationSmoothSort),
                new PermFunSorter<long, V3d>    (SmoothSort,    SortingExtensions.PermutationSmoothSort),

                new SimpleSorter<int>       (TimSort,       SortingExtensions.TimSortAscending),
                new SimpleSorter<long>      (TimSort,       SortingExtensions.TimSortAscending),
                new SimpleSorter<float>     (TimSort,       SortingExtensions.TimSortAscending),
                new SimpleSorter<double>    (TimSort,       SortingExtensions.TimSortAscending),
                new FunSorter<V2d>          (TimSort,       SortingExtensions.TimSort),
                new FunSorter<V3d>          (TimSort,       SortingExtensions.TimSort),

                new PermSorter<int, int>        (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermSorter<int, long>       (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermSorter<int, float>      (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermSorter<int, double>     (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermFunSorter<int, V2d>     (TimSort,       SortingExtensions.PermutationTimSort),
                new PermFunSorter<int, V3d>     (TimSort,       SortingExtensions.PermutationTimSort),

                new PermSorter<long, int>       (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermSorter<long, long>      (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermSorter<long, float>     (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermSorter<long, double>    (TimSort,       SortingExtensions.PermutationTimSortAscending),
                new PermFunSorter<long, V2d>    (TimSort,       SortingExtensions.PermutationTimSort),
                new PermFunSorter<long, V3d>    (TimSort,       SortingExtensions.PermutationTimSort),
            };

            var trialCounts = new List<long>();
            for (double doubleCount = minimalCount; doubleCount < 0.5 + arrayCount; doubleCount *= countFactor)
                trialCounts.Add((long)doubleCount);

            var sorterMap = new Dict<Tup<Symbol, Type, Type, bool>, Sorter>(
                    from s in sorters select KeyValuePairs.Create(Tup.Create(s.Name, s.PermType, s.Type, s.UseCmp), s));

            var master = new long[arrayCount];

            foreach (var creator in arrayCreators)
            {
                var sortArray = creator(arrayCount);
                var type = sortArray.Type;
                var permType = sortArray.PermType;
                var useCmp = sortArray.UseCmp;
                foreach (var initializer in initializers)
                {
                    string permText = permType == null ? "" : String.Format("{0} permuted ", permType.Name);
                    string sectionText = String.Format("{0}{1} {2}s", permText, initializer.Name, type.Name);
                    Test.Begin(sectionText);
                    var results = new Tup<long, double>[sortNames.Length, trialCounts.Count];
                    trialCounts.ForEach((trialCount, tci) =>
                    {
                        Test.Begin("array size {0}", trialCount);
                        var trialRepeat = arrayCount / trialCount;
                        var totalCount = trialRepeat * trialCount;
                        initializer.Init(master, trialCount, trialRepeat, totalCount);
                        for (int si = 0; si < sortNames.Length; si++)
                            for (int i = 0; i < sortNames.Length; i++)
                            {
                                var sni = (si + i) % sortNames.Length;
                                var sorter = sorterMap[Tup.Create(sortNames[sni], permType, type, useCmp)];
                                sortArray.CopyFrom(master, totalCount);
                                Report.BeginTimed("{0} sorting {1}x {2} {3}s",
                                                  sorter.Name, trialRepeat, trialCount, type.Name);
                                sortArray.Sort(sorter.SortFunction, trialCount, trialRepeat);
                                var time = Report.End();
                                results[sni, tci].E0 += totalCount;
                                results[sni, tci].E1 += time;
                                sortArray.Test(Test.IsTrue, trialCount, trialRepeat);
                            }
                        Test.End(" [{0}x]", sortNames.LongLength * trialRepeat);
                    });
                    Test.End();
                    Report.Begin("{0} [microsecs/item]:", sectionText);
                    Report.Text("{0,10} :", "length");
                    for (int sni = 0; sni < sortNames.Length; sni++)
                        Report.Text(" {0,8}", sortNames[sni]);
                    Report.Line();

                    for (int tci = 0; tci < trialCounts.Count; tci++)
                    {
                        Report.Text("{0, 10} :", trialCounts[tci]);
                        for (int sni = 0; sni < sortNames.Length; sni++)
                        {
                            var microsecs = 1000000.0 * results[sni, tci].E1 / results[sni, tci].E0;
                            Report.Text(" {0,8:0.0000}", microsecs);
                        }
                        Report.Line();
                    }
                    Report.End();
                }
            }
        }

    }

}

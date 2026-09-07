using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*ShortestPathBenchmark*'
    /// Heap workloads include insertion and draining; inputs, handle storage and delegate binding are setup-only.
    /// Graph workloads invoke the unchanged calculation body synchronously, including run-owned allocations,
    /// but excluding graph construction, reflection, thread-pool scheduling and asynchronous waiting.
    /// </summary>
    [MemoryDiagnoser]
    public class ShortestPathBenchmark
    {
        [Params(32, 4096)]
        public int Count { get; set; }

        private object _heap;
        private object[] _nodes;
        private float[] _keys;
        private Func<object, float, int, object> _insert;
        private Func<object, int> _extract;
        private Action<object, object, float> _decrease;
        private Action<ShortestPath<int>> _calculate;
        private ShortestPath<int> _sparse;
        private ShortestPath<int> _grid;

        [GlobalSetup]
        public void Setup()
        {
            Report.RootTarget = Report.NoTarget;
            BindHeap();
            _nodes = new object[Count];
            var random = new Random(731);
            _keys = Enumerable.Range(0, Count).Select(_ => (float)random.Next(1, 100000)).ToArray();
            _sparse = CreateGraph(false);
            _grid = CreateGraph(true);
            BindCalculation();
        }

        [Benchmark]
        public long InsertDrain()
        {
            for (int i = 0; i < Count; i++) _insert(_heap, _keys[i], i);
            long checksum = 0;
            for (int i = 0; i < Count; i++) checksum += _extract(_heap);
            return checksum;
        }

        [Benchmark]
        public long InsertDecreaseDrain()
        {
            _insert(_heap, -1, -1);
            for (int i = 0; i < Count; i++) _nodes[i] = _insert(_heap, _keys[i], i);
            _extract(_heap); // Consolidate so decreases exercise child cuts, not just root updates.
            for (int i = 0; i < Count; i++) _decrease(_heap, _nodes[i], -2 - i);
            long checksum = 0;
            for (int i = 0; i < Count; i++) checksum += _extract(_heap);
            return checksum;
        }

        [Benchmark]
        public void SparseGraph() => _calculate(_sparse);

        [Benchmark]
        public void GridGraph() => _calculate(_grid);

        private void BindHeap()
        {
            var heapType = typeof(ShortestPath<int>).Assembly.GetType("Aardvark.Base.FibonacciHeap`1", true)
                .MakeGenericType(typeof(int));
            _heap = Activator.CreateInstance(heapType, true);
            var heap = Expression.Parameter(typeof(object), "heap");
            var key = Expression.Parameter(typeof(float), "key");
            var value = Expression.Parameter(typeof(int), "value");
            var node = Expression.Parameter(typeof(object), "node");
            var insert = heapType.GetMethod("Insert", new[] { typeof(float), typeof(int) });
            var decrease = heapType.GetMethod("DecreaseKey");
            _insert = Expression.Lambda<Func<object, float, int, object>>(
                Expression.Convert(Expression.Call(Expression.Convert(heap, heapType), insert, key, value), typeof(object)),
                heap, key, value).Compile();
            _extract = Expression.Lambda<Func<object, int>>(
                Expression.Call(Expression.Convert(heap, heapType), heapType.GetMethod("DeleteMin")), heap).Compile();
            _decrease = Expression.Lambda<Action<object, object, float>>(
                Expression.Call(Expression.Convert(heap, heapType), decrease,
                    Expression.Convert(node, decrease.GetParameters()[0].ParameterType), key), heap, node, key).Compile();
        }

        private void BindCalculation()
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var type = typeof(ShortestPath<int>);
            var currentRun = type.GetField("m_currentRun", flags);
            var runType = currentRun.FieldType;
            var path = Expression.Parameter(type, "path");
            var run = Expression.Variable(runType, "run");
            _calculate = Expression.Lambda<Action<ShortestPath<int>>>(Expression.Block(new[] { run },
                Expression.Assign(run, Expression.New(runType.GetConstructor(new[] { typeof(int), typeof(int) }),
                    Expression.Constant(0), Expression.Constant(Count))),
                Expression.Assign(Expression.Field(path, currentRun), run),
                Expression.Call(path, type.GetMethod("Calculate", flags), run)), path).Compile();
        }

        private ShortestPath<int> CreateGraph(bool grid)
        {
            var random = new Random(137);
            var neighbors = Enumerable.Range(0, Count).Select(_ => new List<int>()).ToArray();
            int width = (int)Math.Sqrt(Count);
            for (int i = 0; i < Count; i++)
            {
                if (i > 0 && (!grid || i % width != 0)) AddEdge(i - 1, i);
                if (grid)
                {
                    if (i >= width) AddEdge(i - width, i);
                }
                else
                {
                    for (int j = 0; j < 5; j++)
                    {
                        int other = random.Next(Count);
                        if (other != i) AddEdge(i, other);
                    }
                }
            }
            return new ShortestPath<int>(Enumerable.Range(0, Count).ToArray(), neighbors,
                (a, b) => 1 + ((Math.Min(a, b) * 31 + Math.Max(a, b) * 17) % 97));

            void AddEdge(int a, int b)
            {
                neighbors[a].Add(b);
                neighbors[b].Add(a);
            }
        }
    }
}

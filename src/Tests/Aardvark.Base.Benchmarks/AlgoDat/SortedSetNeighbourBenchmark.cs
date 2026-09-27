using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Loader;

namespace Aardvark.Base.Benchmarks
{
    // Set AARDVARK_SORTEDSET_ASSEMBLY to the Release/net8.0 assembly to measure; defaults to the current build.
    // dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*SortedSetNeighbourBenchmark*'
    [MemoryDiagnoser]
    public class SortedSetNeighbourBenchmark
    {
        private const int Count = 65536;
        private const int Queries = 256;
        private Subject _subject;
        private int[] _queries;
        private int _mutationKey;

        [Params("Ordinary", "Narrow", "Wide")]
        public string Scope { get; set; }
        [Params("FindNeighboursV", "TryFindSmaller", "TryFindGreater")]
        public string Api { get; set; }
        [Params(false, true)]
        public bool Mutate { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            string path = Environment.GetEnvironmentVariable("AARDVARK_SORTEDSET_ASSEMBLY") ?? typeof(SortedSetExt<int>).Assembly.Location;
            var context = new AssemblyLoadContext("SortedSet benchmark target");
            var assembly = context.LoadFromAssemblyPath(Path.GetFullPath(path));
            var type = assembly.GetType("Aardvark.Base.SortedSetExt`1", true).MakeGenericType(typeof(int));
            int low = Scope == "Narrow" ? Count - 16 : Scope == "Wide" ? Count / 4 : 0;
            int high = Scope == "Narrow" ? Count + 16 : Scope == "Wide" ? 7 * Count / 4 : 2 * Count - 2;
            _mutationKey = Count;
            var values = Enumerable.Range(0, Count).Select(x => 2 * x).ToArray();
            _subject = new Subject(type, values, Scope != "Ordinary", low, high, Api);
            _queries = new int[Queries];
            var random = new Random(2731);
            for (int i = 0; i < Queries; i++)
                _queries[i] = (i % 5) switch
                {
                    0 => low - 3,
                    1 => low,
                    2 => random.Next(low, high + 1),
                    3 => high,
                    _ => high + 3
                };
        }

        [Benchmark(OperationsPerInvoke = Queries)]
        public int Query()
        {
            var subject = _subject;
            int checksum = 0;
            foreach (int query in _queries)
            {
                // This row measures mutation + first query, including the mutation's node allocation.
                if (Mutate) subject.Mutate(subject.Parent, _mutationKey);
                checksum += subject.Query(subject.Target, query);
            }
            return checksum;
        }

        // Baseline and revised runs use the same compiled adapter in separate processes.
        // Reflection, compilation and construction are outside timing; no private tree access.
        private sealed class Subject
        {
            public readonly object Parent, Target;
            public readonly Func<object, int, int> Query;
            public readonly Action<object, int> Mutate;

            public Subject(Type type, int[] values, bool view, int low, int high, string api)
            {
                Parent = type.GetConstructor(new[] { typeof(IEnumerable<int>) }).Invoke(new object[] { values });
                Target = view ? type.GetMethod("GetViewBetween").Invoke(Parent, new object[] { low, high }) : Parent;
                var instance = Expression.Parameter(typeof(object));
                var key = Expression.Parameter(typeof(int));
                var receiver = Expression.Convert(instance, type);
                var byrefInt = typeof(int).MakeByRefType();
                if (api == "FindNeighboursV")
                {
                    var lower = Expression.Variable(typeof(int));
                    var self = Expression.Variable(typeof(int));
                    var upper = Expression.Variable(typeof(int));
                    var flags = Expression.Variable(typeof(ValueTuple<bool, bool, bool>));
                    var method = type.GetMethod(api, new[] { typeof(int), byrefInt, byrefInt, byrefInt });
                    Expression sum = Expression.Add(Expression.Add(lower, self), upper);
                    foreach (string field in new[] { "Item1", "Item2", "Item3" })
                        sum = Expression.Add(sum, Expression.Condition(Expression.Field(flags, field), Expression.Constant(1), Expression.Constant(0)));
                    Query = Expression.Lambda<Func<object, int, int>>(Expression.Block(new[] { lower, self, upper, flags },
                        Expression.Assign(flags, Expression.Call(receiver, method, key, lower, self, upper)), sum), instance, key).Compile();
                }
                else
                {
                    var value = Expression.Variable(typeof(int));
                    var found = Expression.Variable(typeof(bool));
                    var method = type.GetMethod(api, new[] { typeof(int), byrefInt });
                    Query = Expression.Lambda<Func<object, int, int>>(Expression.Block(new[] { value, found },
                        Expression.Assign(found, Expression.Call(receiver, method, key, value)),
                        Expression.Add(value, Expression.Condition(found, Expression.Constant(1), Expression.Constant(0)))), instance, key).Compile();
                }
                Mutate = Expression.Lambda<Action<object, int>>(Expression.Block(
                    Expression.Call(receiver, type.GetMethod("Remove", new[] { typeof(int) }), key),
                    Expression.Call(receiver, type.GetMethod("Add", new[] { typeof(int) }), key),
                    Expression.Empty()), instance, key).Compile();
            }
        }
    }
}

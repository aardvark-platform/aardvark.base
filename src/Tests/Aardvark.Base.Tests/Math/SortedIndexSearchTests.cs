using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class SortedIndexSearchTests
    {
        private static IEnumerable<Assembly> Assemblies()
        {
            yield return typeof(Fun).Assembly;
            var directory = Path.GetDirectoryName(typeof(SortedIndexSearchTests).Assembly.Location);
            var path = Path.GetFullPath(Path.Combine(directory!, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Expected sibling core assembly at '{path}'.");
            yield return Assembly.LoadFile(path);
        }

        private static IEnumerable<(string Name, Func<T[], List<T>, T, long> Find)> Searches<T>(Assembly assembly)
            where T : IComparable
        {
            var methods = assembly.GetType(typeof(Fun).FullName!)!.GetMethods();
            var array = methods.Single(m => m.Name == nameof(Fun.IndexOfLargestLessOrEqual)
                && m.GetParameters()[0].ParameterType.IsArray).MakeGenericMethod(typeof(T))
                .CreateDelegate<Func<T[], T, int>>();
            var list = methods.Single(m => m.Name == nameof(Fun.IndexOfLargestLessOrEqual)
                && !m.GetParameters()[0].ParameterType.IsArray).MakeGenericMethod(typeof(T))
                .CreateDelegate<Func<List<T>, T, int>>();
            var longArray = methods.Single(m => m.Name == nameof(Fun.LongIndexOfLargestLessOrEqual))
                .MakeGenericMethod(typeof(T)).CreateDelegate<Func<T[], T, long>>();
            yield return ("array", (a, l, x) => array(a, x));
            yield return ("list", (a, l, x) => list(l, x));
            yield return ("long array", (a, l, x) => longArray(a, x));
        }

        private static long LinearFloor<T>(T[] values, T query) where T : IComparable
        {
            long result = -1;
            for (int i = 0; i < values.Length; i++)
                if (values[i].CompareTo(query) <= 0) result = i;
            return values[values.Length - 1].CompareTo(query) < 0 ? values.Length : result;
        }

        private static void CheckSequences<T>(Assembly assembly, IEnumerable<T[]> sequences, T[] queries)
            where T : IComparable
        {
            foreach (var (name, find) in Searches<T>(assembly))
            foreach (var values in sequences)
            {
                var list = values.ToList();
                foreach (var query in queries)
                    Assert.AreEqual(LinearFloor(values, query), find(values, list, query),
                        $"{assembly.Location}: {name}, [{string.Join(",", values)}], query={query}");
            }
        }

        private static IEnumerable<int[]> NondecreasingSequences()
        {
            for (int length = 1; length <= 8; length++)
            for (int zeros = 0; zeros <= length; zeros++)
            for (int ones = 0; ones <= length - zeros; ones++)
                yield return Enumerable.Repeat(0, zeros).Concat(Enumerable.Repeat(1, ones))
                    .Concat(Enumerable.Repeat(2, length - zeros - ones)).ToArray();
        }

        [Test]
        public void NondecreasingIntegerSequencesMatchLinearFloorOracle()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in Assemblies())
                    CheckSequences(assembly, NondecreasingSequences(), new[] { -1, 0, 1, 2, 3 });
            });
        }

        [Test]
        public void StringsSignedLimitsAndSingletonsKeepRightmostMatches()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in Assemblies())
                {
                    CheckSequences(assembly, new[]
                    {
                        new[] { int.MinValue }, new[] { int.MaxValue },
                        new[] { int.MinValue, int.MinValue, 0, int.MaxValue, int.MaxValue },
                        new[] { -7, -7, -3, 0, 0 }
                    }, new[] { int.MinValue, int.MinValue + 1, -8, -7, -3, -1, 0, 1, int.MaxValue - 1, int.MaxValue });
                    CheckSequences(assembly, new[]
                    {
                        new[] { long.MinValue }, new[] { long.MaxValue },
                        new[] { long.MinValue, long.MinValue + 1, -1, 0, 1, long.MaxValue - 1, long.MaxValue, long.MaxValue }
                    }, new[] { long.MinValue, long.MinValue + 1, -2, -1, 0, 1, 2, long.MaxValue - 1, long.MaxValue });
                    CheckSequences(assembly, new[]
                    {
                        new[] { "m" }, new[] { "z", "z" }, new[] { "a", "a", "m", "z", "z" }
                    }, new[] { "", "a", "b", "m", "y", "z", "zz" });
                }
            });
        }

        private sealed class ComparableProbe : IComparable, IComparable<ComparableProbe>
        {
            public readonly int Value;
            public readonly List<int> ComparedValues = new List<int>();
            public int ThrowAt;
            public Exception Failure;
            public ComparableProbe(int value) { Value = value; }
            public int CompareTo(object other)
            {
                var value = ((ComparableProbe)other).Value;
                ComparedValues.Add(value);
                if (ComparedValues.Count == ThrowAt) throw Failure;
                return Value < value ? int.MinValue : Value > value ? int.MaxValue : 0;
            }
            int IComparable<ComparableProbe>.CompareTo(ComparableProbe other)
                => throw new NotSupportedException("Generic comparison must not be used.");
        }

        [Test]
        public void ComparisonDispatchAndInputFailuresRemainUnchanged()
        {
            foreach (var assembly in Assemblies())
            {
                foreach (var (name, find) in Searches<int>(assembly))
                {
                    var context = $"{assembly.Location}: {name}";
                    Assert.Throws<NullReferenceException>(() => find(null, null, 0), context);
                    Type emptyError = name == "list" ? typeof(ArgumentOutOfRangeException) : typeof(IndexOutOfRangeException);
                    Assert.Throws(emptyError, () => find(Array.Empty<int>(), new List<int>(), 0), context);
                }
                foreach (var (name, find) in Searches<string>(assembly))
                    Assert.Throws<NullReferenceException>(() => find(new[] { "a", "z" }, new List<string> { "a", "z" }, null),
                        $"{assembly.Location}: {name}, null query");

                foreach (var (name, find) in Searches<ComparableProbe>(assembly))
                {
                    var values = new[] { -2, 0, 3, 7 }.Select(x => new ComparableProbe(x)).ToArray();
                    var list = values.ToList();
                    var query = new ComparableProbe(6);
                    var context = $"{assembly.Location}: {name}";
                    Assert.AreEqual(2, find(values, list, query), context);
                    CollectionAssert.AreEqual(new[] { -2, 7, 0, 3 }, query.ComparedValues, context);
                    for (int throwAt = 1; throwAt <= 3; throwAt++)
                    {
                        var failure = new InvalidOperationException("comparison failure");
                        query = new ComparableProbe(6) { ThrowAt = throwAt, Failure = failure };
                        var actual = Assert.Throws<InvalidOperationException>(() => find(values, list, query), context);
                        Assert.AreSame(failure, actual, context);
                        CollectionAssert.AreEqual(new[] { -2, 7, 0 }.Take(throwAt), query.ComparedValues, context);
                    }
                    Assert.That(values.All(x => x.ComparedValues.Count == 0), Is.True, context + ", query is comparison receiver");
                }
            }
        }

        [Test]
        public void CurveNeighbourhoodClampsEndpointsIncludingExtendedWindows()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in Assemblies())
                foreach (var parameters in new[] { new[] { 4.0 }, new[] { -3.0, 0.0, 2.0, 8.0 } })
                {
                    var items = Enumerable.Range(0, parameters.Length).Select(i => $"point {i}").ToArray();
                    var curveType = assembly.GetType("Aardvark.Base.CurvePoints`1")!.MakeGenericType(typeof(string));
                    var curve = Activator.CreateInstance(curveType, parameters, items);
                    var neighbourhood = curveType.GetMethod("Neighbourhood", new[] { typeof(double) })!
                        .CreateDelegate<Func<double, Array>>(curve);
                    var extended = curveType.GetMethod("Neighbourhood", new[] { typeof(double), typeof(int), typeof(int) })!
                        .CreateDelegate<Func<double, int, int, Array>>(curve);
                    var tupleType = curveType.GetMethod("Neighbourhood", new[] { typeof(double) })!.ReturnType.GetElementType();
                    Assert.AreSame(assembly, tupleType!.Assembly);
                    var parameterField = tupleType.GetField("E0");
                    var itemField = tupleType.GetField("E1");
                    foreach (var query in parameters.Concat(new[] { parameters[0] - 1.0, parameters[parameters.Length - 1] + 1.0 }))
                    foreach (var (left, right) in new[] { (0, 0), (1, 0), (0, 2), (2, 1) })
                    {
                        var actual = extended(query, left, right);
                        int floor = Math.Clamp((int)LinearFloor(parameters, query), 0, parameters.Length - 1);
                        var context = $"{assembly.Location}: [{string.Join(",", parameters)}], query={query}, extensions={left}/{right}";
                        Assert.AreEqual(2 + left + right, actual.Length, context);
                        if (left == 0 && right == 0)
                            CollectionAssert.AreEqual(neighbourhood(query), actual, context);
                        for (int i = 0; i < actual.Length; i++)
                        {
                            int index = Math.Clamp(floor - left + i, 0, parameters.Length - 1);
                            var pair = actual.GetValue(i);
                            Assert.AreEqual(parameters[index], parameterField!.GetValue(pair), context + $", item {i}");
                            Assert.AreEqual(items[index], itemField!.GetValue(pair), context + $", item {i}");
                        }
                    }
                }
            });
        }
    }
}

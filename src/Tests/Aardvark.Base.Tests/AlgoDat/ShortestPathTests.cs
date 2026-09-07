using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ShortestPathTests
    {
        private static readonly TimeSpan CompletionTimeout = TimeSpan.FromSeconds(10);

        [Test, Timeout(30000)]
        public void CalculatesWeightedPathsAndPreservesUnreachableShape()
        {
            var costs = new float[5, 5];
            SetCost(costs, 0, 1, 1);
            SetCost(costs, 0, 2, 10);
            SetCost(costs, 1, 2, 2);
            SetCost(costs, 1, 3, 8);
            SetCost(costs, 2, 3, 1);

            var shortestPath = new ShortestPath<int>(
                new[] { 0, 1, 2, 3, 4 },
                new[]
                {
                    new List<int> { 1, 2 },
                    new List<int> { 0, 2, 3 },
                    new List<int> { 0, 1, 3 },
                    new List<int> { 1, 2 },
                    new List<int>()
                },
                (a, b) => costs[a, b]);

            shortestPath.CalculateShortestPaths(0);
            WaitForPath(shortestPath, 3, 3, 2, 1);

            CollectionAssert.AreEqual(new[] { 3, 2, 1 }, shortestPath.GetMinimalPath(3));
            CollectionAssert.AreEqual(new[] { 4, 0 }, shortestPath.GetMinimalPathByIndex(4));
            shortestPath.Cancel();
        }

        [Test, Timeout(30000)]
        public void InvalidSeedsAndIndexesDoNotReplaceActiveRun()
        {
            var entered = new ManualResetEventSlim();
            var release = new ManualResetEventSlim();
            var shortestPath = new ShortestPath<int>(
                new[] { 0, 1 },
                new[] { new List<int> { 1 }, new List<int>() },
                (a, b) =>
                {
                    entered.Set();
                    WaitOrThrow(release, "The active cost callback was not released.");
                    return 1;
                });

            try
            {
                shortestPath.CalculateShortestPaths(0);
                WaitOrThrow(entered, "The active calculation did not reach its cost callback.");

                var seedException = Assert.Throws<ArgumentException>(
                    () => shortestPath.CalculateShortestPaths(99));
                Assert.AreEqual("seed", seedException.ParamName);

                Assert.AreEqual(
                    "index",
                    Assert.Throws<ArgumentOutOfRangeException>(
                        () => shortestPath.CalculateShortestPathsByIndex(-1)).ParamName);
                Assert.AreEqual(
                    "index",
                    Assert.Throws<ArgumentOutOfRangeException>(
                        () => shortestPath.CalculateShortestPathsByIndex(2)).ParamName);

                release.Set();
                WaitForPath(shortestPath, 1, 1);
            }
            finally
            {
                release.Set();
                shortestPath.Cancel();
                entered.Dispose();
                release.Dispose();
            }
        }

        [Test, Timeout(30000)]
        public void ReplacementCancelsStaleWorkerAndPreventsPublication()
        {
            var oldCostEntered = new ManualResetEventSlim();
            var releaseOldCost = new ManualResetEventSlim();
            var oldCostReturned = new ManualResetEventSlim();
            var staleWorkerContinued = new ManualResetEventSlim();
            var shortestPath = new ShortestPath<int>(
                new[] { 0, 1, 2, 3 },
                new[]
                {
                    new List<int> { 1 },
                    new List<int> { 2 },
                    new List<int>(),
                    new List<int> { 2 }
                },
                (a, b) =>
                {
                    if (a == 0)
                    {
                        oldCostEntered.Set();
                        WaitOrThrow(releaseOldCost, "The stale cost callback was not released.");
                        oldCostReturned.Set();
                    }
                    else if (a == 1)
                    {
                        staleWorkerContinued.Set();
                    }

                    return 1;
                });

            try
            {
                shortestPath.CalculateShortestPathsByIndex(0);
                WaitOrThrow(oldCostEntered, "The first calculation did not reach its cost callback.");

                shortestPath.CalculateShortestPathsByIndex(3);
                WaitForPath(shortestPath, 2, 2);

                releaseOldCost.Set();
                WaitOrThrow(oldCostReturned, "The stale cost callback did not return.");
                Assert.That(
                    staleWorkerContinued.Wait(TimeSpan.FromSeconds(1)),
                    Is.False,
                    "The replaced calculation continued into another edge.");
                CollectionAssert.AreEqual(new[] { 2 }, shortestPath.GetMinimalPathByIndex(2));
            }
            finally
            {
                releaseOldCost.Set();
                shortestPath.Cancel();
                oldCostEntered.Dispose();
                releaseOldCost.Dispose();
                oldCostReturned.Dispose();
                staleWorkerContinued.Dispose();
            }
        }

        [Test, Timeout(30000)]
        public void ReadersObserveOnlyCompletedSnapshotsDuringReplacement()
        {
            var replacementEntered = new ManualResetEventSlim();
            var releaseReplacement = new ManualResetEventSlim();
            var readerErrors = new ConcurrentQueue<int[]>();
            var readerCount = 0;
            var stopReaders = new CancellationTokenSource();
            var shortestPath = new ShortestPath<int>(
                new[] { 0, 1, 2, 3 },
                new[]
                {
                    new List<int> { 1 },
                    new List<int>(),
                    new List<int> { 1 },
                    new List<int> { 2 }
                },
                (a, b) =>
                {
                    if (a == 3)
                    {
                        replacementEntered.Set();
                        WaitOrThrow(releaseReplacement, "The replacement cost callback was not released.");
                    }

                    return 1;
                });

            Task reader = null;
            try
            {
                shortestPath.CalculateShortestPathsByIndex(0);
                WaitForPath(shortestPath, 1, 1);

                shortestPath.CalculateShortestPathsByIndex(3);
                WaitOrThrow(replacementEntered, "The replacement calculation did not reach its cost callback.");

                reader = StartLongRunning(() =>
                {
                    while (!stopReaders.IsCancellationRequested)
                    {
                        var path = shortestPath.GetMinimalPathByIndex(1).ToArray();
                        if (!path.SequenceEqual(new[] { 1 }) && !path.SequenceEqual(new[] { 1, 2 }))
                            readerErrors.Enqueue(path);

                        Interlocked.Increment(ref readerCount);
                    }
                });

                Assert.That(
                    SpinWait.SpinUntil(() => Volatile.Read(ref readerCount) >= 1000, CompletionTimeout),
                    Is.True,
                    "The concurrent reader did not make progress.");

                releaseReplacement.Set();
                WaitForPath(shortestPath, 1, 1, 2);
                stopReaders.Cancel();
                AssertTaskCompletes(reader);

                Assert.That(readerErrors, Is.Empty);
            }
            finally
            {
                releaseReplacement.Set();
                stopReaders.Cancel();
                if (reader != null && !reader.IsCompleted)
                    AssertTaskCompletes(reader);
                shortestPath.Cancel();
                replacementEntered.Dispose();
                releaseReplacement.Dispose();
                stopReaders.Dispose();
            }
        }

        [Test, Timeout(30000)]
        public void CancelWaitsForCurrentRunAndLeavesLastSnapshotIntact()
        {
            var costEntered = new ManualResetEventSlim();
            var releaseCost = new ManualResetEventSlim();
            var cancelStarted = new ManualResetEventSlim();
            var blockFirstCost = 1;
            var shortestPath = new ShortestPath<int>(
                new[] { 0, 1 },
                new[] { new List<int> { 1 }, new List<int>() },
                (a, b) =>
                {
                    if (Interlocked.Exchange(ref blockFirstCost, 0) == 1)
                    {
                        costEntered.Set();
                        WaitOrThrow(releaseCost, "The canceled cost callback was not released.");
                    }

                    return 1;
                });

            Task cancelTask = null;
            try
            {
                shortestPath.CalculateShortestPathsByIndex(0);
                WaitOrThrow(costEntered, "The calculation did not reach its cost callback.");

                cancelTask = StartLongRunning(() =>
                {
                    cancelStarted.Set();
                    shortestPath.Cancel();
                });

                WaitOrThrow(cancelStarted, "Cancel did not start.");
                Assert.That(
                    cancelTask.Wait(TimeSpan.FromMilliseconds(100)),
                    Is.False,
                    "Cancel returned before the active cost callback completed.");

                releaseCost.Set();
                AssertTaskCompletes(cancelTask);
                CollectionAssert.AreEqual(new[] { 1, 0 }, shortestPath.GetMinimalPathByIndex(1));

                shortestPath.CalculateShortestPathsByIndex(0);
                WaitForPath(shortestPath, 1, 1);
            }
            finally
            {
                releaseCost.Set();
                if (cancelTask != null && !cancelTask.IsCompleted)
                    AssertTaskCompletes(cancelTask);
                shortestPath.Cancel();
                costEntered.Dispose();
                releaseCost.Dispose();
                cancelStarted.Dispose();
            }
        }

        [Test, Timeout(30000)]
        public void CancelPropagatesGenuineWorkerFailures()
        {
            var costEntered = new ManualResetEventSlim();
            var shortestPath = new ShortestPath<int>(
                new[] { 0, 1 },
                new[] { new List<int> { 1 }, new List<int>() },
                (a, b) =>
                {
                    costEntered.Set();
                    throw new InvalidOperationException("Cost calculation failed.");
                });

            shortestPath.CalculateShortestPathsByIndex(0);
            WaitOrThrow(costEntered, "The failing calculation did not reach its cost callback.");

            var exception = Assert.Throws<InvalidOperationException>(() => shortestPath.Cancel());
            Assert.AreEqual("Cost calculation failed.", exception.Message);
            costEntered.Dispose();
        }

        [Test, Timeout(30000)]
        public void DecreasedFrontierNodeIsExpandedBeforeMoreExpensivePaths()
        {
            var costs = new float[6, 6];
            var edges = new List<(int, int)>();
            foreach (var (a, b, cost) in new[]
            {
                (0, 1, 10), (0, 2, 20), (0, 3, 30), (0, 4, 40), (0, 5, 50),
                (1, 3, 1), (3, 2, 1)
            })
            {
                edges.Add((a, b));
                SetCost(costs, a, b, cost);
            }

            var shortestPath = new ShortestPath<int>(Enumerable.Range(0, 6).ToList(), edges, (a, b) => costs[a, b]);
            try
            {
                shortestPath.CalculateShortestPathsByIndex(0);
                WaitForCalculation(shortestPath);
                CollectionAssert.AreEqual(new[] { 2, 3, 1 }, shortestPath.GetMinimalPathByIndex(2));
                Assert.That(PathCost(shortestPath.GetMinimalPathByIndex(2), 0, costs), Is.EqualTo(12));
            }
            finally
            {
                shortestPath.Cancel();
            }
        }

        [TestCase(7, 16, false)]
        [TestCase(31, 64, false)]
        [TestCase(101, 128, false)]
        [TestCase(7, 16, true)]
        [TestCase(31, 64, true)]
        [TestCase(101, 128, true)]
        [Timeout(30000)]
        public void SeededGraphsMatchReferenceDijkstra(int randomSeed, int count, bool directed)
        {
            var random = new Random(randomSeed);
            var costs = new float[count, count];
            var neighbors = Enumerable.Range(0, count).Select(_ => new List<int>()).ToArray();
            for (int a = 0; a < count - 1; a++) // The last node is deliberately unreachable.
            {
                for (int b = directed ? 0 : a + 1; b < count - 1; b++)
                {
                    if (a == b || random.Next(5) != 0) continue;
                    costs[a, b] = random.Next(0, 101); // Finite, non-negative, with ties and zero costs.
                    neighbors[a].Add(b);
                    if (!directed)
                    {
                        costs[b, a] = costs[a, b];
                        neighbors[b].Add(a);
                    }
                }
            }

            var shortestPath = new ShortestPath<int>(Enumerable.Range(0, count).ToArray(), neighbors, (a, b) => costs[a, b]);
            try
            {
                foreach (int seed in new[] { 0, count / 2, count - 1 })
                {
                    var expected = ReferenceDijkstra(neighbors, costs, seed);
                    shortestPath.CalculateShortestPathsByIndex(seed);
                    WaitForCalculation(shortestPath);
                    for (int target = 0; target < count; target++)
                    {
                        var path = shortestPath.GetMinimalPathByIndex(target);
                        if (target == seed)
                            Assert.That(path, Is.Empty, "Seed paths exclude the seed");
                        else if (float.IsPositiveInfinity(expected[target]))
                            CollectionAssert.AreEqual(new[] { target, seed }, path, "Unreachable path convention");
                        else
                        {
                            Assert.That(path[0], Is.EqualTo(target));
                            Assert.That(path, Does.Not.Contain(seed));
                            Assert.That(path.Distinct().Count(), Is.EqualTo(path.Count), "No predecessor cycle");
                            int previous = seed;
                            for (int i = path.Count - 1; i >= 0; i--)
                            {
                                Assert.That(neighbors[previous], Does.Contain(path[i]), "Path must follow graph edges");
                                previous = path[i];
                            }
                            Assert.That(PathCost(path, seed, costs), Is.EqualTo(expected[target]), $"Seed {seed}, target {target}");
                        }
                    }
                }
            }
            finally
            {
                shortestPath.Cancel();
            }
        }

        private static float[] ReferenceDijkstra(List<int>[] neighbors, float[,] costs, int seed)
        {
            var distances = Enumerable.Repeat(float.PositiveInfinity, neighbors.Length).ToArray();
            var expanded = new bool[neighbors.Length];
            distances[seed] = 0;
            for (int i = 0; i < neighbors.Length; i++)
            {
                int next = -1;
                for (int node = 0; node < neighbors.Length; node++)
                    if (!expanded[node] && (next == -1 || distances[node] < distances[next])) next = node;
                if (next == -1 || float.IsPositiveInfinity(distances[next])) break;
                expanded[next] = true;
                foreach (int neighbor in neighbors[next])
                    distances[neighbor] = Math.Min(distances[neighbor], distances[next] + costs[next, neighbor]);
            }
            return distances;
        }

        private static float PathCost(List<int> path, int seed, float[,] costs)
        {
            float cost = 0;
            int previous = seed;
            for (int i = path.Count - 1; i >= 0; i--)
            {
                cost += costs[previous, path[i]];
                previous = path[i];
            }
            return cost;
        }

        private static void WaitForCalculation(ShortestPath<int> shortestPath)
        {
            // Await the actual worker, not an expected path that could mask a failed calculation.
            var run = typeof(ShortestPath<int>).GetField("m_currentRun", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(shortestPath);
            var task = (Task)run.GetType().GetProperty("Task").GetValue(run);
            AssertTaskCompletes(task);
        }

        private static void SetCost(float[,] costs, int a, int b, float value)
        {
            costs[a, b] = value;
            costs[b, a] = value;
        }

        private static void WaitForPath(
            ShortestPath<int> shortestPath,
            int target,
            params int[] expected)
        {
            Assert.That(
                SpinWait.SpinUntil(
                    () => shortestPath.GetMinimalPathByIndex(target).SequenceEqual(expected),
                    CompletionTimeout),
                Is.True,
                $"The expected path [{string.Join(", ", expected)}] was not published.");
        }

        private static void WaitOrThrow(ManualResetEventSlim signal, string message)
        {
            if (!signal.Wait(CompletionTimeout))
                throw new TimeoutException(message);
        }

        private static Task StartLongRunning(Action action)
            => Task.Factory.StartNew(
                action,
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

        private static void AssertTaskCompletes(Task task)
        {
            Assert.That(task.Wait(CompletionTimeout), Is.True, "The background task did not complete in time.");
        }
    }
}

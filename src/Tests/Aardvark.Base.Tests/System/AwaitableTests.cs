using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Tests
{
    [TestFixture]
    public class AwaitableTests
    {
        private const int RaceIterations = 5000;
        private static readonly TimeSpan CompletionTimeout = TimeSpan.FromSeconds(15);

        [Test, Timeout(30000)]
        public void ResultObservesValuePublishedByConcurrentCompletion()
        {
            var sources = new Awaitable<int>[RaceIterations];
            var observed = new int[RaceIterations];
            for (var i = 0; i < sources.Length; i++)
                sources[i] = new Awaitable<int>();

            using (var barrier = new Barrier(2))
            {
                var producer = Task.Run(() =>
                {
                    for (var i = 0; i < sources.Length; i++)
                    {
                        Synchronize(barrier);
                        sources[i].Emit(i + 1);
                        Synchronize(barrier);
                    }
                });

                var consumer = Task.Run(() =>
                {
                    for (var i = 0; i < sources.Length; i++)
                    {
                        Synchronize(barrier);
                        if (!SpinWait.SpinUntil(() => sources[i].IsCompleted, CompletionTimeout))
                            throw new TimeoutException("Awaitable completion was not observed.");

                        observed[i] = sources[i].Result;
                        Synchronize(barrier);
                    }
                });

                AssertTasksComplete(producer, consumer);
            }

            for (var i = 0; i < observed.Length; i++)
                Assert.That(observed[i], Is.EqualTo(i + 1), $"Iteration {i}");
        }

        [Test, Timeout(30000)]
        public void MultipleResultReadersAreReleasedByCompletion()
        {
            const int readerCount = 16;
            const int expected = 123456;
            var source = new Awaitable<int>();
            var readers = new Task<int>[readerCount];

            using (var ready = new CountdownEvent(readerCount))
            using (var start = new ManualResetEventSlim())
            {
                for (var i = 0; i < readers.Length; i++)
                {
                    readers[i] = Task.Run(() =>
                    {
                        ready.Signal();
                        if (!start.Wait(CompletionTimeout))
                            throw new TimeoutException("Reader start was not signaled.");

                        return source.Result;
                    });
                }

                var emitter = Task.Run(() =>
                {
                    if (!start.Wait(CompletionTimeout))
                        throw new TimeoutException("Emitter start was not signaled.");

                    source.Emit(expected);
                });

                Assert.That(ready.Wait(CompletionTimeout), Is.True, "Readers did not become ready.");
                start.Set();

                var tasks = new Task[readerCount + 1];
                for (var i = 0; i < readers.Length; i++)
                    tasks[i] = readers[i];
                tasks[readerCount] = emitter;
                AssertTasksComplete(tasks);
            }

            foreach (var reader in readers)
                Assert.That(reader.Result, Is.EqualTo(expected));
        }

        [Test, Timeout(30000)]
        public void GenericSubscriptionsRacingWithEmitRunExactlyOnce()
        {
            var sources = new Awaitable<int>[RaceIterations];
            var callbackCounts = new int[RaceIterations];
            var observed = new int[RaceIterations];
            var errors = new ConcurrentQueue<Exception>();
            for (var i = 0; i < sources.Length; i++)
                sources[i] = new Awaitable<int>();

            using (var barrier = new Barrier(2))
            {
                var subscriber = Task.Run(() =>
                {
                    for (var i = 0; i < sources.Length; i++)
                    {
                        Synchronize(barrier);
                        var index = i;
                        try
                        {
                            sources[index].Subscribe(value =>
                            {
                                observed[index] = value;
                                Interlocked.Increment(ref callbackCounts[index]);
                            });
                        }
                        catch (Exception e)
                        {
                            errors.Enqueue(e);
                        }
                        Synchronize(barrier);
                    }
                });

                var emitter = Task.Run(() =>
                {
                    for (var i = 0; i < sources.Length; i++)
                    {
                        Synchronize(barrier);
                        sources[i].Emit(i + 1);
                        Synchronize(barrier);
                    }
                });

                AssertTasksComplete(subscriber, emitter);
            }

            Assert.That(errors, Is.Empty);
            for (var i = 0; i < sources.Length; i++)
            {
                Assert.That(callbackCounts[i], Is.EqualTo(1), $"Callback count at iteration {i}");
                Assert.That(observed[i], Is.EqualTo(i + 1), $"Result at iteration {i}");
            }
        }

        [Test, Timeout(30000)]
        public void LateSubscriptionsRunSynchronouslyAndRegisteredExceptionsAreIsolated()
        {
            var completed = new Awaitable<int>();
            completed.Emit(42);

            var actionCount = 0;
            var genericCount = 0;
            var observed = 0;
            completed.Subscribe(() => actionCount++);
            completed.Subscribe(value =>
            {
                genericCount++;
                observed = value;
            });

            Assert.That(actionCount, Is.EqualTo(1));
            Assert.That(genericCount, Is.EqualTo(1));
            Assert.That(observed, Is.EqualTo(42));

            var pending = new Awaitable<int>();
            var deliveredAfterException = 0;
            var reentrantResult = 0;
            pending.Subscribe(_ => throw new InvalidOperationException("Expected callback failure."));
            pending.Subscribe(value => deliveredAfterException = value);
            pending.Subscribe(_ => pending.Subscribe(value => reentrantResult = value));

            Assert.DoesNotThrow(() => pending.Emit(7));
            Assert.That(deliveredAfterException, Is.EqualTo(7));
            Assert.That(reentrantResult, Is.EqualTo(7));
        }

        [Test, Timeout(30000)]
        public void CompetingEmitCallsPublishOnlyOneResult()
        {
            const int emitterCount = 16;
            var source = new Awaitable<int>();
            var actionCount = 0;
            var genericCount = 0;
            var observed = 0;
            source.Subscribe(() => Interlocked.Increment(ref actionCount));
            source.Subscribe(value =>
            {
                observed = value;
                Interlocked.Increment(ref genericCount);
            });

            var tasks = new Task[emitterCount];
            using (var barrier = new Barrier(emitterCount + 1))
            {
                for (var i = 0; i < tasks.Length; i++)
                {
                    var value = i + 1;
                    tasks[i] = Task.Run(() =>
                    {
                        Synchronize(barrier);
                        source.Emit(value);
                    });
                }

                Synchronize(barrier);
                AssertTasksComplete(tasks);
            }

            var result = source.Result;
            Assert.That(result, Is.InRange(1, emitterCount));
            Assert.That(observed, Is.EqualTo(result));
            Assert.That(actionCount, Is.EqualTo(1));
            Assert.That(genericCount, Is.EqualTo(1));

            source.Emit(-1);
            Assert.That(source.Result, Is.EqualTo(result));
            Assert.That(actionCount, Is.EqualTo(1));
            Assert.That(genericCount, Is.EqualTo(1));
        }

        private static void Synchronize(Barrier barrier)
        {
            if (!barrier.SignalAndWait(CompletionTimeout))
                throw new TimeoutException("Concurrent test participants did not synchronize.");
        }

        private static void AssertTasksComplete(params Task[] tasks)
        {
            Assert.That(
                Task.WaitAll(tasks, CompletionTimeout),
                Is.True,
                "Concurrent test tasks did not complete in time.");
        }
    }
}

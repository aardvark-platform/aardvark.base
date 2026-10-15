using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Tests.Extensions
{
    static class TaskWithCancellationTests
    {
        private static Task<int> Wrap(Task<int> task, CancellationToken token, bool nullable)
            => nullable ? task.WithCancellation((CancellationToken?)token) : task.WithCancellation(token);

        private static async Task AssertCanceled(Task<int> task, CancellationToken token, string context)
        {
            try
            {
                await task.WaitAsync(TimeSpan.FromSeconds(10));
            }
            catch (TaskCanceledException error)
            {
                Assert.That(error.GetType(), Is.EqualTo(typeof(TaskCanceledException)), context);
                Assert.That(error.CancellationToken, Is.EqualTo(token), context);
                Assert.That(task.IsCanceled, Is.True, context);
                return;
            }
            Assert.Fail(context + ": expected TaskCanceledException");
        }

        [Test]
        public static void CancellationAfterWrappingPreservesTokenAndOriginalTaskIndependence()
        {
            Assert.Multiple(async () =>
            {
                foreach (bool nullable in new[] { false, true })
                {
                    using var cancellation = new CancellationTokenSource();
                    var source = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
                    var result = Wrap(source.Task, cancellation.Token, nullable);
                    string context = $"nullable={nullable}, cancellation after wrapping";
                    Assert.That(result, Is.Not.SameAs(source.Task), context);
                    Assert.That(result.IsCompleted, Is.False, context);

                    cancellation.Cancel();
                    await AssertCanceled(result, cancellation.Token, context);
                    Assert.That(source.Task.IsCompleted, Is.False, context + ", input remains pending");
                    source.SetResult(37);
                    Assert.That(await source.Task, Is.EqualTo(37), context + ", input subsequently succeeds");
                    await AssertCanceled(result, cancellation.Token, context + ", wrapper stays canceled");
                }
            });
        }

        [Test]
        public static async Task PreCanceledTokensProduceCanceledWrappersWithoutCancelingInput()
        {
            foreach (bool nullable in new[] { false, true })
            {
                using var cancellation = new CancellationTokenSource();
                cancellation.Cancel();
                var source = new TaskCompletionSource<int>();
                var result = Wrap(source.Task, cancellation.Token, nullable);
                string context = $"nullable={nullable}, pre-canceled token";
                Assert.That(result.IsCompleted, Is.True, context);
                await AssertCanceled(result, cancellation.Token, context);
                Assert.That(source.Task.IsCompleted, Is.False, context);
                source.SetResult(23);
                Assert.That(await source.Task, Is.EqualTo(23), context);
            }
        }

        [Test]
        public static async Task FastPathsKeepTaskIdentityAndCompletedInputPrecedence()
        {
            using var cancellation = new CancellationTokenSource();
            using var inputCancellation = new CancellationTokenSource();
            inputCancellation.Cancel();
            var failure = new InvalidOperationException("completed input failure");
            var completed = new[] { Task.FromResult(17), Task.FromException<int>(failure), Task.FromCanceled<int>(inputCancellation.Token) };
            foreach (bool cancelFirst in new[] { false, true })
            {
                if (cancelFirst) cancellation.Cancel();
                foreach (bool nullable in new[] { false, true })
                foreach (var task in completed)
                    Assert.That(Wrap(task, cancellation.Token, nullable), Is.SameAs(task),
                        $"nullable={nullable}, canceled token={cancelFirst}, input={task.Status}");
            }
            Assert.That(await completed[0], Is.EqualTo(17));
            Assert.That(Assert.ThrowsAsync<InvalidOperationException>(async () => await completed[1]), Is.SameAs(failure));
            await AssertCanceled(completed[2], inputCancellation.Token, "completed input token takes precedence");

            var source = new TaskCompletionSource<int>();
            foreach (bool nullable in new[] { false, true })
            {
                Assert.That(Wrap(source.Task, CancellationToken.None, nullable), Is.SameAs(source.Task), $"nullable={nullable}, noncancelable");
                foreach (var token in new[] { CancellationToken.None, cancellation.Token })
                {
                    string context = $"nullable={nullable}, null input, cancelable={token.CanBeCanceled}";
                    var error = Assert.Throws<ArgumentNullException>(() => Wrap(null, token, nullable), context);
                    Assert.That(error.ParamName, Is.EqualTo("task"), context);
                }
            }
            Assert.That(source.Task.WithCancellation((CancellationToken?)null), Is.SameAs(source.Task), "absent token");
            Assert.That(Assert.Throws<ArgumentNullException>(() => ((Task<int>)null).WithCancellation((CancellationToken?)null)).ParamName,
                Is.EqualTo("task"), "null task with absent token");
            source.SetResult(19);
            Assert.That(await source.Task, Is.EqualTo(19));
        }

        [Test]
        public static async Task PendingInputSuccessFaultAndCancellationPropagateBeforeWrapperCancellation()
        {
            foreach (bool nullable in new[] { false, true })
            foreach (int outcome in new[] { 0, 1, 2 })
            {
                using var cancellation = new CancellationTokenSource();
                using var inputCancellation = new CancellationTokenSource();
                var source = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
                var result = Wrap(source.Task, cancellation.Token, nullable);
                var failure = new InvalidOperationException("pending input failure");
                string context = $"nullable={nullable}, outcome={outcome}";
                Assert.That(result.IsCompleted, Is.False, context);
                if (outcome == 0)
                {
                    source.SetResult(41);
                    Assert.That(await result.WaitAsync(TimeSpan.FromSeconds(10)), Is.EqualTo(41), context);
                }
                else if (outcome == 1)
                {
                    source.SetException(failure);
                    var error = Assert.ThrowsAsync<InvalidOperationException>(async () => await result.WaitAsync(TimeSpan.FromSeconds(10)), context);
                    Assert.That(error, Is.SameAs(failure), context);
                    Assert.That(result.IsFaulted, Is.True, context);
                }
                else
                {
                    inputCancellation.Cancel();
                    source.SetCanceled(inputCancellation.Token);
                    await AssertCanceled(result, inputCancellation.Token, context);
                }
                cancellation.Cancel();
                Assert.That(result.Status, Is.EqualTo(source.Task.Status), context + ", later wrapper cancellation cannot replace input outcome");
                if (outcome == 2) await AssertCanceled(result, inputCancellation.Token, context + ", input token retained");
            }
        }
    }
}

using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Tests.Extensions
{
    static class TaskWithCancellationTests
    {
        [Test]
        public static void WithCancellationReturnsCanceledTaskForAlreadyCanceledToken()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var source = new TaskCompletionSource<int>();
            var result = source.Task.WithCancellation(cts.Token);

            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.IsCanceled, Is.True);
        }

        [Test]
        public static void WithCancellationRejectsNullTask()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => ((Task<int>)null).WithCancellation(CancellationToken.None));

            Assert.That(exception.ParamName, Is.EqualTo("task"));
        }

        [Test]
        public static void WithCancellationReturnsCanceledTaskForAlreadyCanceledNullableToken()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var source = new TaskCompletionSource<int>();
            var result = source.Task.WithCancellation((CancellationToken?)cts.Token);

            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.IsCanceled, Is.True);
        }

        [Test]
        public static void WithCancellationRejectsNullTaskForNullableToken()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => ((Task<int>)null).WithCancellation((CancellationToken?)CancellationToken.None));

            Assert.That(exception.ParamName, Is.EqualTo("task"));
        }

        [Test]
        public static void WithCancellationKeepsCompletedTaskForAlreadyCanceledToken()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var task = Task.FromResult(1);
            var result = task.WithCancellation(cts.Token);

            Assert.That(result, Is.SameAs(task));
            Assert.That(result.Result, Is.EqualTo(1));
        }
    }
}

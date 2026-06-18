using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Threading;

namespace Aardvark.Tests.Extensions
{
    static class AwaitableWithCancellationTests
    {
        [Test]
        public static void GenericWithCancellationRejectsNullInput()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => ((IAwaitable<int>)null).WithCancellation(CancellationToken.None));

            Assert.That(exception.ParamName, Is.EqualTo("input"));
        }

        [Test]
        public static void NonGenericWithCancellationRejectsNullInput()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => ((IAwaitable)null).WithCancellation(CancellationToken.None));

            Assert.That(exception.ParamName, Is.EqualTo("input"));
        }
    }
}

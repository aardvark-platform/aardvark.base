using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests.Extensions
{
    static class AwaitableWhenAllTests
    {
        [Test]
        public static void GenericWhenAllCompletesImmediatelyForEmptyInput()
        {
            var result = Await.WhenAll<int>();

            Assert.That(result.IsCompleted, Is.True);
            CollectionAssert.IsEmpty(result.Result);
        }

        [Test]
        public static void NonGenericWhenAllCompletesImmediatelyForEmptyInput()
        {
            var result = Await.WhenAll(new IAwaitable[0]);

            Assert.That(result.IsCompleted, Is.True);
        }

        [Test]
        public static void GenericWhenAllPreservesDuplicateInputs()
        {
            var input = new Awaitable<int>();
            var result = Await.WhenAll(input, input);

            Assert.That(result.IsCompleted, Is.False);

            input.Emit(42);

            Assert.That(result.IsCompleted, Is.True);
            CollectionAssert.AreEqual(new[] { 42, 42 }, result.Result);
        }

        [Test]
        public static void GenericWhenAllPreservesInputOrder()
        {
            var first = new Awaitable<int>();
            var second = new Awaitable<int>();
            var third = new Awaitable<int>();
            var result = Await.WhenAll(first, second, third);

            second.Emit(2);
            Assert.That(result.IsCompleted, Is.False);

            first.Emit(1);
            Assert.That(result.IsCompleted, Is.False);

            third.Emit(3);

            Assert.That(result.IsCompleted, Is.True);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result.Result);
        }

        [Test]
        public static void GenericWhenAllRejectsNullInputArray()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => Await.WhenAll<int>((IAwaitable<int>[])null));

            Assert.That(exception.ParamName, Is.EqualTo("inputs"));
        }

        [Test]
        public static void NonGenericWhenAllRejectsNullInputArray()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => Await.WhenAll((IAwaitable[])null));

            Assert.That(exception.ParamName, Is.EqualTo("inputs"));
        }

        [Test]
        public static void GenericWhenAllRejectsNullInputElement()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => Await.WhenAll(new Awaitable<int>(), null));

            Assert.That(exception.ParamName, Is.EqualTo("inputs"));
        }

        [Test]
        public static void NonGenericWhenAllRejectsNullInputElement()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => Await.WhenAll(new Awaitable(), null));

            Assert.That(exception.ParamName, Is.EqualTo("inputs"));
        }
    }
}

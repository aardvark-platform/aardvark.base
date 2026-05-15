using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Tests
{
    [TestFixture]
    public class EventSourceSlimTests
    {
        [Test]
        public void LatestReturnsInitialValueAndUpdatesAfterEmit()
        {
            var source = new EventSourceSlim<int>(10);

            Assert.That(source.Latest, Is.EqualTo(10));

            source.Emit(20);

            Assert.That(source.Latest, Is.EqualTo(20));
        }

        [Test]
        public void NextCompletesWithNextEmittedValueAndRefreshes()
        {
            var source = new EventSourceSlim<int>(0);
            var first = source.Next;

            Assert.That(first.IsCompleted, Is.False);

            source.Emit(1);

            Assert.That(first.IsCompleted, Is.True);
            Assert.That(first.Result, Is.EqualTo(1));

            var second = source.Next;

            Assert.That(second, Is.Not.SameAs(first));
            Assert.That(second.IsCompleted, Is.False);

            source.Emit(2);

            Assert.That(second.IsCompleted, Is.True);
            Assert.That(second.Result, Is.EqualTo(2));
        }

        [Test]
        public void NonGenericNextCompletesWithoutThrowing()
        {
            var source = new EventSourceSlim<string>("initial");
            var genericEvent = (IEvent<string>)source;
            var nonGenericEvent = (IEvent)source;

            var typedNext = genericEvent.Next;
            var next = nonGenericEvent.Next;

            Assert.That(next, Is.SameAs(typedNext));
            Assert.That(next.IsCompleted, Is.False);

            source.Emit("next");

            Assert.That(next.IsCompleted, Is.True);
            Assert.DoesNotThrow(() => next.GetAwaiter().GetResult());
            Assert.That(typedNext.Result, Is.EqualTo("next"));
        }

        [Test]
        public void NonGenericValuesEmitUnitNotifications()
        {
            var source = new EventSourceSlim<int>(0);
            var observer = new RecordingObserver<Unit>();

            using (((IEvent)source).Values.Subscribe(observer))
            {
                source.Emit(1);
                source.Emit(2);
            }

            Assert.That(observer.Values.Count, Is.EqualTo(2));
            Assert.That(observer.Values[0], Is.SameAs(Unit.Default));
            Assert.That(observer.Values[1], Is.SameAs(Unit.Default));
            Assert.That(observer.Error, Is.Null);
            Assert.That(observer.CompletedCount, Is.EqualTo(0));
        }

        private sealed class RecordingObserver<T> : IObserver<T>
        {
            public readonly List<T> Values = new List<T>();
            public Exception Error;
            public int CompletedCount;

            public void OnCompleted()
            {
                CompletedCount++;
            }

            public void OnError(Exception error)
            {
                Error = error;
            }

            public void OnNext(T value)
            {
                Values.Add(value);
            }
        }
    }
}

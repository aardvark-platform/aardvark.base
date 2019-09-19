using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Aardvark.Base;
using System.Linq;

namespace Aardvark.Tests.Extensions
{
    static class Enumerables
    {
        [Test]
        public static void MaxIndex()
        {
            var en = 0.UpTo(5);

            var maxIndex = en.MaxIndex();
            var maxValue = en.Max();
            Assert.True(en.ElementAt(maxIndex) == maxValue);

            Assert.True(0.IntoIEnumerable().MaxIndex() == 0);

            Assert.True(Enumerable.Empty<int>().MaxIndex() < 0);
        }

        [Test]
        public static void MinIndex()
        {
            var en = 0.UpTo(5);

            var minIndex = en.MinIndex();
            var minValue = en.Min();
            Assert.True(en.ElementAt(minIndex) == minValue);

            Assert.True(0.IntoIEnumerable().MinIndex() == 0);

            Assert.True(Enumerable.Empty<int>().MinIndex() < 0);
        }
    }
}

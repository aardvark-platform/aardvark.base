using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RangeElementsTests
    {
        private static readonly (Type Range, BigInteger Min, BigInteger Max)[] s_types =
        {
            (typeof(Range1b), byte.MinValue, byte.MaxValue),
            (typeof(Range1sb), sbyte.MinValue, sbyte.MaxValue),
            (typeof(Range1s), short.MinValue, short.MaxValue),
            (typeof(Range1us), ushort.MinValue, ushort.MaxValue),
            (typeof(Range1i), int.MinValue, int.MaxValue),
            (typeof(Range1ui), uint.MinValue, uint.MaxValue),
            (typeof(Range1l), long.MinValue, long.MaxValue),
            (typeof(Range1ul), ulong.MinValue, ulong.MaxValue)
        };

        private static IEnumerable<Assembly> CoreAssemblies()
        {
            yield return typeof(Range1i).Assembly;
            string path = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(typeof(Range1i).Assembly.Location)!,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core assembly: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static void Verify(Assembly assembly, Type nativeRange, BigInteger min, BigInteger max)
        {
            var rangeType = assembly.GetType(nativeRange.FullName!, throwOnError: true)!;
            Assert.That(rangeType.Assembly, Is.SameAs(assembly));
            var scalar = rangeType.GetField("Min")!.FieldType;
            object ConvertScalar(BigInteger value)
                => Convert.ChangeType((decimal)value, scalar, CultureInfo.InvariantCulture);
            var range = rangeType.GetConstructor(new[] { scalar, scalar })!
                .Invoke(new[] { ConvertScalar(min), ConvertScalar(max) });
            var elements = (IEnumerable)rangeType.GetProperty("Elements")!.GetValue(range)!;
            int count = max < min ? 0 : (int)(max - min + 1);
            string target = assembly == typeof(Range1i).Assembly ? "native" : "standard";
            string context = $"{target} {nativeRange.Name} [{min},{max}]";

            // Fixed MoveNext budgets, never foreach/ToArray on the candidate sequence:
            // the old maximum-ending iterators keep yielding after integer overflow.
            for (int repeat = 0; repeat < 2; repeat++)
            {
                var iterator = elements.GetEnumerator();
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        bool moved = iterator.MoveNext();
                        Assert.That(moved, Is.True, $"{context}, repeat {repeat}, element {i}");
                        if (!moved) break; // Do not inspect undefined Current on an early end.
                        Assert.That(iterator.Current, Is.TypeOf(scalar), context);
                        var actual = new BigInteger(Convert.ToDecimal(iterator.Current, CultureInfo.InvariantCulture));
                        Assert.That(actual, Is.EqualTo(min + i), $"{context}, repeat {repeat}, element {i}");
                    }
                    for (int tail = 0; tail < 4; tail++)
                        Assert.That(iterator.MoveNext(), Is.False,
                            $"{context}, repeat {repeat}, exhaustion {tail}");
                }
                finally
                {
                    (iterator as IDisposable)?.Dispose();
                }
            }
        }

        [Test]
        public void IntegerMaximumEndpointsTerminateWithoutWrapping()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in CoreAssemblies())
                foreach (var type in s_types)
                for (int width = 0; width < 8; width++)
                    Verify(assembly, type.Range, type.Max - width, type.Max);
            });
        }

        [Test]
        public void IntegerMinimumAndOrdinaryRangesKeepInclusiveValues()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in CoreAssemblies())
                foreach (var type in s_types)
                {
                    for (int offset = 0; offset < 8; offset++)
                    for (int width = 0; width < 8; width++)
                        Verify(assembly, type.Range, type.Min + offset, type.Min + offset + width);
                    int start = type.Min < 0 ? -8 : 0;
                    for (int offset = start; offset < start + 16; offset++)
                    for (int width = 0; width < 8; width++)
                        Verify(assembly, type.Range, offset, offset + width);
                }
            });
        }

        [Test]
        public void InvalidIntegerRangesStayEmptyAcrossRepeatedEnumeration()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in CoreAssemblies())
                foreach (var type in s_types)
                for (int min = 1; min <= 5; min++)
                for (int max = 0; max < min; max++)
                    Verify(assembly, type.Range, min, max);
            });
        }
    }
}

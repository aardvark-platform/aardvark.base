using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ApproximateEqualsTests
    {
        private static IEnumerable<Assembly> Assemblies()
        {
            yield return typeof(Fun).Assembly;
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location)!,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core assembly: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static Func<T, T, T, bool> Scalar<T>(Assembly assembly)
            => assembly.GetType(typeof(Fun).FullName!, true)!
                .GetMethod(nameof(Fun.ApproximateEquals), new[] { typeof(T), typeof(T), typeof(T) })!
                .CreateDelegate<Func<T, T, T, bool>>();

        private static Func<long, long, long, bool> SignedScalar(Assembly assembly, bool wide)
        {
            if (wide) return Scalar<long>(assembly);
            var call = Scalar<int>(assembly);
            return (x, y, e) => call((int)x, (int)y, (int)e);
        }

        private static bool Within(long x, long y, long epsilon)
            => BigInteger.Abs(new BigInteger(x) - y) <= epsilon;

        private static long Next(Random random, bool wide)
        {
            var bytes = new byte[8];
            random.NextBytes(bytes);
            return wide ? BitConverter.ToInt64(bytes) : BitConverter.ToInt32(bytes);
        }

        private static IEnumerable<(long X, long Y, long Epsilon)> Cases(bool wide, bool vector)
        {
            long min = wide ? long.MinValue : int.MinValue, max = wide ? long.MaxValue : int.MaxValue;
            yield return (min, max, 1);
            yield return (min, 0, max);
            long[] anchors = vector
                ? new[] { min, min + 1, -1, 0, 1, max - 1, max }
                : new[] { min, min + 1, min + 2, min / 2, -1024, -129, -128, -2, -1, 0, 1, 2, 127, 128, 1024, max / 2, max - 2, max - 1, max };
            IEnumerable<long> Boundaries(long x, long y, IEnumerable<long> tolerances)
            {
                var values = new HashSet<long>(tolerances);
                BigInteger distance = BigInteger.Abs(new BigInteger(x) - y);
                for (int delta = -1; delta <= 1; delta++)
                {
                    BigInteger epsilon = distance + delta;
                    if (epsilon >= min && epsilon <= max) values.Add((long)epsilon);
                }
                return values;
            }
            foreach (long x in anchors)
            foreach (long y in anchors)
            foreach (long epsilon in Boundaries(x, y, new[] { min, -1, 0, 1, 2, 127, max / 2, max - 1, max }))
                yield return (x, y, epsilon);
            var random = new Random(53147 + (wide ? 64 : 32));
            for (int i = 0; i < (vector ? 256 : 4096); i++)
            {
                long x = Next(random, wide), y = Next(random, wide), epsilon = Next(random, wide);
                foreach (long e in Boundaries(x, y, new[] { epsilon })) yield return (x, y, e);
                if ((i & 7) == 0) { yield return (x, x, 0); yield return (x, x, -1); }
            }
        }

        [Test]
        public void SignedScalarsMatchExactDistancesAcrossTheFullDomain()
        {
            foreach (var assembly in Assemblies())
            foreach (bool wide in new[] { false, true })
            {
                var call = SignedScalar(assembly, wide);
                foreach (var c in Cases(wide, false))
                {
                    string context = $"{assembly.Location}, {(wide ? "long" : "int")}, x {c.X}, y {c.Y}, epsilon {c.Epsilon}";
                    bool expected = Within(c.X, c.Y, c.Epsilon);
                    Assert.That(call(c.X, c.Y, c.Epsilon), Is.EqualTo(expected), context);
                    Assert.That(call(c.Y, c.X, c.Epsilon), Is.EqualTo(expected), context + ", swapped");
                }
            }
        }

        private static Func<long[], long[], long, bool> Vector(Assembly assembly, int dimension, bool wide)
        {
            var scalar = wide ? typeof(long) : typeof(int);
            var vector = assembly.GetType($"Aardvark.Base.V{dimension}{(wide ? "l" : "i")}", true)!;
            Assert.That(vector.Assembly, Is.SameAs(assembly));
            var ctor = vector.GetConstructor(Enumerable.Repeat(scalar, dimension).ToArray())!;
            var x = Expression.Parameter(typeof(long[])); var y = Expression.Parameter(typeof(long[]));
            var epsilon = Expression.Parameter(typeof(long));
            Expression Value(ParameterExpression array) => Expression.New(ctor, Enumerable.Range(0, dimension).Select(i =>
                Expression.Convert(Expression.ArrayIndex(array, Expression.Constant(i)), scalar)));
            var method = assembly.GetType(typeof(Fun).FullName!, true)!
                .GetMethod(nameof(Fun.ApproximateEquals), new[] { vector, vector, scalar })!;
            Assert.That(method.GetParameters()[0].ParameterType.Assembly, Is.SameAs(assembly));
            return Expression.Lambda<Func<long[], long[], long, bool>>(
                Expression.Call(method, Value(x), Value(y), Expression.Convert(epsilon, scalar)), x, y, epsilon).Compile();
        }

        private static void CheckVector(Func<long[], long[], long, bool> call, long[] x, long[] y, long epsilon, string context)
        {
            bool expected = Enumerable.Range(0, x.Length).All(i => Within(x[i], y[i], epsilon));
            string message = $"{context}, x [{string.Join(",", x)}], y [{string.Join(",", y)}], epsilon {epsilon}";
            Assert.That(call(x, y, epsilon), Is.EqualTo(expected), message);
            Assert.That(call(y, x, epsilon), Is.EqualTo(expected), message + ", swapped");
        }

        [Test]
        public void SignedVectorsCheckEveryComponentWithInclusiveTolerance()
        {
            foreach (var assembly in Assemblies())
            foreach (bool wide in new[] { false, true })
            for (int dimension = 2; dimension <= 4; dimension++)
            {
                var call = Vector(assembly, dimension, wide);
                string context = $"{assembly.Location}, V{dimension}{(wide ? "l" : "i")}";
                var x = new long[dimension]; var y = new long[dimension];
                for (int axis = 0; axis < dimension; axis++)
                {
                    foreach (var c in Cases(wide, true))
                    {
                        x[axis] = c.X; y[axis] = c.Y;
                        CheckVector(call, x, y, c.Epsilon, context + $", axis {axis}");
                    }
                    x[axis] = y[axis] = 0;
                }
                var random = new Random(91731 + dimension + (wide ? 64 : 32));
                for (int i = 0; i < 256; i++)
                {
                    for (int axis = 0; axis < dimension; axis++)
                    {
                        x[axis] = Next(random, wide);
                        y[axis] = (i + axis) % 3 == 0 ? Next(random, wide) : x[axis];
                    }
                    long epsilon = i % 3 == 0 ? 0 : i % 3 == 1 ? (wide ? long.MaxValue : int.MaxValue) : Next(random, wide);
                    CheckVector(call, x, y, epsilon, context + $", mixed {i}");
                }
            }
        }

        private static void CheckUnchanged<T>(Assembly assembly, T[] anchors, Func<T, BigInteger> integer)
        {
            var call = Scalar<T>(assembly);
            foreach (T x in anchors)
            foreach (T y in anchors)
            foreach (T epsilon in anchors)
                Assert.That(call(x, y, epsilon), Is.EqualTo(BigInteger.Abs(integer(x) - integer(y)) <= integer(epsilon)),
                    $"{assembly.Location}, {typeof(T).Name}, x {x}, y {y}, epsilon {epsilon}");
        }

        [Test]
        public void OtherIntegerScalarsKeepExactToleranceSemantics()
        {
            foreach (var assembly in Assemblies())
            {
                CheckUnchanged(assembly, new byte[] { 0, 1, 2, 127, 128, 254, 255 }, x => x);
                CheckUnchanged(assembly, new sbyte[] { sbyte.MinValue, -127, -1, 0, 1, 126, sbyte.MaxValue }, x => x);
                CheckUnchanged(assembly, new short[] { short.MinValue, -32767, -1, 0, 1, 32766, short.MaxValue }, x => x);
                CheckUnchanged(assembly, new ushort[] { 0, 1, 2, 32767, 32768, 65534, ushort.MaxValue }, x => x);
                CheckUnchanged(assembly, new uint[] { 0, 1, 2, int.MaxValue, (uint)int.MaxValue + 1, uint.MaxValue - 1, uint.MaxValue }, x => x);
                CheckUnchanged(assembly, new ulong[] { 0, 1, 2, long.MaxValue, (ulong)long.MaxValue + 1, ulong.MaxValue - 1, ulong.MaxValue }, x => x);
            }
        }
    }
}

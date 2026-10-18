using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class CbrtTests
    {
        private static IEnumerable<Assembly> Assemblies()
        {
            yield return typeof(Fun).Assembly;
            var directory = Path.GetDirectoryName(typeof(CbrtTests).Assembly.Location);
            var path = Path.GetFullPath(Path.Combine(directory!, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Expected sibling core assembly at '{path}'.");
            yield return Assembly.LoadFile(path);
        }

        private static Func<T, double> Scalar<T>(Assembly assembly)
            => assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.Cbrt), new[] { typeof(T) })!
                .CreateDelegate<Func<T, double>>();

        private static void CheckResult(double input, double actual, string context)
        {
            var expected = Math.Cbrt(input);
            Assert.That(double.IsFinite(actual), Is.True, context);
            Assert.AreEqual(expected, actual, Math.Abs(expected) * 3e-15, context);
            if (input < 0) Assert.That(actual, Is.LessThan(0.0), context);
            if (input > 0) Assert.That(actual, Is.GreaterThan(0.0), context);
            if (input == 0) Assert.AreEqual(0L, BitConverter.DoubleToInt64Bits(actual), context);
        }

        private static IEnumerable<int> IntInputs()
        {
            foreach (int value in new[] { int.MinValue, int.MinValue + 1, int.MinValue + 2, int.MaxValue - 1, int.MaxValue })
                yield return value;
            foreach (int root in new[] { -1290, -129, -17, -10, -3, -2, -1, 0, 1, 2, 3, 10, 17, 129, 1290 })
            {
                int cube = checked(root * root * root);
                for (int offset = -1; offset <= 1; offset++) yield return cube + offset;
            }
        }

        private static IEnumerable<long> LongInputs()
        {
            foreach (long value in new[] { long.MinValue, long.MinValue + 1, long.MinValue + 2,
                -9007199254740993L, -9007199254740992L, -9007199254740991L,
                9007199254740991L, 9007199254740992L, 9007199254740993L, long.MaxValue - 1, long.MaxValue })
                yield return value;
            foreach (long root in new[] { -2097151L, -1290, -129, -17, -10, -3, -2, -1, 0, 1, 2, 3, 10, 17, 129, 1290, 2097151 })
            {
                long cube = checked(root * root * root);
                for (int offset = -1; offset <= 1; offset++) yield return cube + offset;
            }
        }

        private static void CheckScalars<T>(Assembly assembly, IEnumerable<T> values) where T : IConvertible
        {
            var cbrt = Scalar<T>(assembly);
            foreach (var value in values)
                CheckResult(Convert.ToDouble(value), cbrt(value), $"{assembly.Location}: {typeof(T).Name} {value}");
        }

        [Test]
        public void SignedIntegerScalarsIncludeMinimaCubesAndLimits()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in Assemblies())
                {
                    CheckScalars(assembly, IntInputs());
                    CheckScalars(assembly, LongInputs());
                    var minimumRoot = Scalar<long>(assembly)(long.MinValue);
                    Assert.AreEqual(-2097152.0, minimumRoot, 2097152.0 * 3e-15, assembly.Location);
                }
            });
        }

        [Test]
        public void SmallerSignedAndUnsignedOverloadsKeepTheirResults()
        {
            foreach (var assembly in Assemblies())
            {
                CheckScalars(assembly, Enumerable.Range(sbyte.MinValue, 256).Select(x => (sbyte)x));
                CheckScalars(assembly, Enumerable.Range(0, 256).Select(x => (byte)x));
                CheckScalars(assembly, new short[] { short.MinValue, -1000, -27, -8, -1, 0, 1, 8, 27, 1000, short.MaxValue });
                CheckScalars(assembly, new ushort[] { 0, 1, 8, 27, 1000, ushort.MaxValue });
                CheckScalars(assembly, new uint[] { 0, 1, 8, 27, 1000, uint.MaxValue });
                CheckScalars(assembly, new ulong[] { 0, 1, 8, 27, 1000, 9007199254740993, ulong.MaxValue });
            }
        }

        private static Func<T[], double[]> Vector<T>(Assembly assembly, int dimension, string suffix)
        {
            var vectorType = assembly.GetType($"Aardvark.Base.V{dimension}{suffix}");
            var method = assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.Cbrt), new[] { vectorType! });
            Assert.AreSame(assembly, method!.ReturnType.Assembly);
            var values = Expression.Parameter(typeof(T[]), "values");
            var arguments = Enumerable.Range(0, dimension).Select(i => Expression.ArrayIndex(values, Expression.Constant(i)));
            var input = Expression.New(vectorType!.GetConstructor(Enumerable.Repeat(typeof(T), dimension).ToArray())!, arguments);
            var result = Expression.Variable(method.ReturnType, "result");
            var components = "XYZW".Take(dimension).Select(name => Expression.Field(result, name.ToString()));
            return Expression.Lambda<Func<T[], double[]>>(Expression.Block(new[] { result },
                Expression.Assign(result, Expression.Call(method, input)), Expression.NewArrayInit(typeof(double), components)), values).Compile();
        }

        private static void CheckVectors<T>(Assembly assembly, int dimension, string suffix, T[] values) where T : IConvertible
        {
            var cbrt = Vector<T>(assembly, dimension, suffix);
            for (int offset = 0; offset < values.Length; offset++)
            {
                var input = Enumerable.Range(0, dimension).Select(i => values[(offset + i) % values.Length]).ToArray();
                var actual = cbrt(input);
                for (int i = 0; i < dimension; i++)
                    CheckResult(Convert.ToDouble(input[i]), actual[i],
                        $"{assembly.Location}: V{dimension}{suffix}, [{string.Join(",", input)}], component {i}");
            }
        }

        [Test]
        public void SignedIntegerVectorsApplyCubeRootsToEveryComponent()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in Assemblies())
                for (int dimension = 2; dimension <= 4; dimension++)
                {
                    CheckVectors(assembly, dimension, "i", IntInputs().Distinct().ToArray());
                    CheckVectors(assembly, dimension, "l", LongInputs().Distinct().ToArray());
                }
            });
        }
    }
}

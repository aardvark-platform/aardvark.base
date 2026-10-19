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
    public class AcoshTests
    {
        private static IEnumerable<Assembly> CoreAssemblies()
        {
            yield return typeof(Fun).Assembly;
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location)!,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core assembly: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static Func<double, double> Scalar(Assembly assembly, bool single)
        {
            var type = single ? typeof(float) : typeof(double);
            var x = Expression.Parameter(typeof(double));
            var call = Expression.Call(assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.Acosh), new[] { type })!,
                Expression.Convert(x, type));
            return Expression.Lambda<Func<double, double>>(Expression.Convert(call, typeof(double)), x).Compile();
        }

        private static double Next(double x, bool single, bool up)
            => single ? (up ? MathF.BitIncrement((float)x) : MathF.BitDecrement((float)x))
                      : (up ? Math.BitIncrement(x) : Math.BitDecrement(x));

        private static double[] FiniteInputs(bool single)
        {
            var values = new List<double> { single ? float.MaxValue : double.MaxValue, 1.0 };
            foreach (double center in new[] { 1.0, single ? 4096.0 : 67108864.0,
                single ? MathF.Sqrt(float.MaxValue) : Math.Sqrt(double.MaxValue),
                Math.ScaleB(1.0, single ? 64 : 512) })
            {
                values.Add(center);
                values.Add(Next(center, single, false)); values.Add(Next(center, single, true));
                values.Add(Next(Next(center, single, false), single, false));
                values.Add(Next(Next(center, single, true), single, true));
            }
            // Include near-one product-rounding transitions, not just the nearest value.
            foreach (int k in new[] { 1, 2, 3, 7, 15, 31, 1023, 2047, 2048, 2049, 4095, 4096 })
                values.Add(1.0 + k * Math.ScaleB(1.0, single ? -23 : -52));
            for (int i = 0; i <= 1024; i++) values.Add(1.0 + i / 16.0);
            for (int e = 0; e <= (single ? 127 : 1023); e++)
            {
                values.Add(Math.ScaleB(1.0, e)); values.Add(Math.ScaleB(1.5, e));
            }
            var random = new Random(63281 + (single ? 1 : 0));
            for (int i = 0; i < 512; i++)
                values.Add(single ? BitConverter.Int32BitsToSingle(random.Next(0x3f800000, 0x7f800000))
                    : BitConverter.Int64BitsToDouble(random.NextInt64(0x3ff0000000000000L, 0x7ff0000000000000L)));
            return values.Select(x => single ? (double)(float)x : x)
                .Where(x => x >= 1 && double.IsFinite(x)).Distinct().ToArray();
        }

        private static double[] ExceptionalInputs(bool single)
            => new[] { -1e10, -1e30, -1e200, single ? -float.MaxValue : -double.MaxValue,
                double.NegativeInfinity, -2.0, -1.0, -0.0, 0.0, 0.5,
                Next(1.0, single, false), double.PositiveInfinity, double.NaN };

        private static void Check(double x, double actual, bool single, string context)
        {
            double expected = single ? MathF.Acosh((float)x) : Math.Acosh(x);
            string message = $"{context}, x={x:R}, actual={actual:R}, expected={expected:R}";
            if (double.IsNaN(expected)) { Assert.That(double.IsNaN(actual), Is.True, message); return; }
            if (double.IsPositiveInfinity(expected)) { Assert.That(actual, Is.EqualTo(expected), message); return; }
            Assert.That(double.IsFinite(actual), Is.True, message);
            Assert.That(actual, Is.GreaterThanOrEqualTo(0.0), message);
            if (x == 1) { Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(0L), message); return; }
            double tolerance = (single ? 2e-7 : 4e-16) + Math.Abs(expected) * (single ? 4e-7 : 3e-15);
            if (x > 1 && x < 1.5)
            {
                // The retained x*x-1 formula amplifies multiplication rounding near one.
                // 1+2*(x-1) is representable here, bounding product error by the smaller
                // of (x-1)^2 and unitRoundoff*x*x; sqrt/log propagation adds this bound.
                double delta = x - 1;
                double unitRoundoff = Math.ScaleB(1.0, single ? -24 : -53);
                tolerance += Math.Min(delta * delta, unitRoundoff * x * x) / Math.Sqrt(delta * (x + 1));
            }
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance), message + $", tolerance={tolerance:R}");
        }

        [Test]
        public void FiniteScalarsMatchRuntimeAcrossRangeAndTransitions()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (bool single in new[] { true, false })
            {
                var call = Scalar(assembly, single);
                foreach (double x in FiniteInputs(single)) Check(x, call(x), single, $"scalar single={single}");
            }
        }

        [Test]
        public void DomainAndExceptionalScalarsFollowAcoshContract()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (bool single in new[] { true, false })
            {
                var call = Scalar(assembly, single);
                foreach (double value in ExceptionalInputs(single))
                {
                    double x = single ? (float)value : value;
                    Check(x, call(x), single, $"domain single={single}");
                }
            }
        }

        [Test]
        public void OrdinaryScalarsKeepNativeOrPortableFormula()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (bool single in new[] { true, false })
            {
                var call = Scalar(assembly, single);
                foreach (double x in FiniteInputs(single).Where(x => x <= (single ? 4096 : 67108864)))
                {
                    double expected;
                    if (assembly == typeof(Fun).Assembly) expected = single ? MathF.Acosh((float)x) : Math.Acosh(x);
                    else if (single)
                    {
                        float f = (float)x;
                        expected = (float)Math.Log(f + (float)Math.Sqrt(f * f - 1));
                    }
                    else expected = Math.Log(x + Math.Sqrt(x * x - 1));
                    Assert.That(BitConverter.DoubleToInt64Bits(call(x)), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)),
                        $"ordinary single={single}, x={x:R}");
                }
            }
        }

        [Test]
        public void FloatingVectorsApplyAcoshToEveryComponent()
        {
            foreach (var assembly in CoreAssemblies())
            {
                int overloads = 0;
                foreach (bool single in new[] { true, false })
                {
                    var data = FiniteInputs(single).Concat(ExceptionalInputs(single))
                        .Select(x => single ? (double)(float)x : x).ToArray();
                    var scalarType = single ? typeof(float) : typeof(double);
                    foreach (int dimension in new[] { 2, 3, 4 })
                    {
                        var vectorType = assembly.GetType($"Aardvark.Base.V{dimension}{(single ? "f" : "d")}", throwOnError: true)!;
                        var input = Expression.Parameter(typeof(double[]));
                        var vector = Expression.New(vectorType.GetConstructor(Enumerable.Repeat(scalarType, dimension).ToArray())!,
                            Enumerable.Range(0, dimension).Select(i => Expression.Convert(Expression.ArrayIndex(input, Expression.Constant(i)), scalarType)));
                        var method = assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.Acosh), new[] { vectorType })!;
                        Assert.That(method.ReturnType.Assembly, Is.SameAs(assembly));
                        var result = Expression.Variable(vectorType);
                        var body = Expression.Block(new[] { result }, Expression.Assign(result, Expression.Call(method, vector)),
                            Expression.NewArrayInit(typeof(double), Enumerable.Range(0, dimension)
                                .Select(i => Expression.Convert(Expression.Field(result, "XYZW"[i].ToString()), typeof(double)))));
                        var call = Expression.Lambda<Func<double[], double[]>>(body, input).Compile();
                        overloads++;
                        for (int offset = 0; offset < data.Length; offset++)
                        {
                            var values = Enumerable.Range(0, dimension).Select(i => data[(offset + i) % data.Length]).ToArray();
                            var actual = call(values);
                            for (int i = 0; i < dimension; i++)
                                Check(values[i], actual[i], single, $"V{dimension}, single={single}, component {i}");
                        }
                    }
                }
                Assert.That(overloads, Is.EqualTo(6));
            }
        }
    }
}

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
    public class AtanhTests
    {
        private static double Tiny(bool single) => Math.ScaleB(1.0, single ? -12 : -27);

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
            var call = Expression.Call(assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.Atanh), new[] { type })!,
                Expression.Convert(x, type));
            return Expression.Lambda<Func<double, double>>(Expression.Convert(call, typeof(double)), x).Compile();
        }

        private static double Next(double x, bool single, bool up)
            => single ? (up ? MathF.BitIncrement((float)x) : MathF.BitDecrement((float)x))
                      : (up ? Math.BitIncrement(x) : Math.BitDecrement(x));

        private static double[] TinyInputs(bool single)
        {
            var values = new List<double> { 0.0, 1e-30, 1e-12, Next(Tiny(single), single, false) };
            for (int bits = 1; bits <= 4096; bits++)
                values.Add(single ? BitConverter.Int32BitsToSingle(bits) : BitConverter.Int64BitsToDouble(bits));
            for (int exponent = single ? -149 : -1074; exponent <= (single ? -13 : -28); exponent++)
            foreach (double multiplier in new[] { 1.0, 1.5, single ? 1.9999998807907104 : 1.9999999999999998 })
            {
                double x = Math.ScaleB(multiplier, exponent);
                values.Add(x); values.Add(Next(x, single, false)); values.Add(Next(x, single, true));
            }
            // Expand signs only after deduplication so both zero bit patterns survive.
            return values.Select(x => single ? (double)(float)x : x).Where(x => x >= 0 && x < Tiny(single))
                .Distinct().SelectMany(x => new[] { x, -x }).ToArray();
        }

        private static double[] FiniteInputs(bool single)
        {
            var values = new List<double>();
            foreach (double center in new[] { Tiny(single), 0.5 })
            {
                values.Add(center);
                values.Add(Next(center, single, false)); values.Add(Next(center, single, true));
                values.Add(Next(Next(center, single, false), single, false));
                values.Add(Next(Next(center, single, true), single, true));
            }
            double endpoint = 1;
            for (int i = 0; i < 32; i++) { endpoint = Next(endpoint, single, false); values.Add(endpoint); }
            for (int i = 1; i < 2048; i++) values.Add(i / 2048.0);
            for (int exponent = single ? -12 : -27; exponent <= -1; exponent++)
            {
                values.Add(Math.ScaleB(1.0, exponent)); values.Add(Math.ScaleB(1.5, exponent));
            }
            var random = new Random(43189 + (single ? 1 : 0));
            for (int i = 0; i < 1024; i++)
                values.Add(single ? BitConverter.Int32BitsToSingle(random.Next(
                    BitConverter.SingleToInt32Bits((float)Tiny(single)), 0x3f800000))
                    : BitConverter.Int64BitsToDouble(random.NextInt64(BitConverter.DoubleToInt64Bits(Tiny(single)), 0x3ff0000000000000L)));
            return values.Select(x => single ? (double)(float)x : x).Where(x => x >= Tiny(single) && x < 1)
                .Distinct().SelectMany(x => new[] { x, -x }).ToArray();
        }

        private static double[] ExceptionalInputs(bool single)
            => new[] { 1.0, -1.0, Next(1, single, true), -Next(1, single, true), 1.5, -1.5,
                1e10, -1e10, single ? float.MaxValue : double.MaxValue, single ? -float.MaxValue : -double.MaxValue,
                double.PositiveInfinity, double.NegativeInfinity, double.NaN,
                BitConverter.Int64BitsToDouble(0x7ff8000000000042L) };

        private static void Check(double x, double actual, bool single, bool portable, string context)
        {
            string message = $"{context}, x={x:R}, actual={actual:R}";
            if (x == 0 || (portable && Math.Abs(x) < Tiny(single)))
            {
                // |atanh(x)-x|/|x| < x*x/(3*(1-x*x)), below half an ULP at these cutoffs.
                Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(x)), message);
                return;
            }
            double expected = single ? MathF.Atanh((float)x) : Math.Atanh(x);
            if (double.IsNaN(expected)) { Assert.That(double.IsNaN(actual), Is.True, message); return; }
            if (double.IsInfinity(expected) || Math.Abs(x) < Tiny(single))
            {
                // Unchanged native libraries can differ from exact tiny identity by one ULP.
                Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), message);
                return;
            }
            Assert.That(double.IsFinite(actual), Is.True, message);
            Assert.That(Math.Sign(actual), Is.EqualTo(Math.Sign(x)), message);
            double tolerance = Math.Abs(expected) * (single ? 4e-7 : 1e-15);
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance), message + $", expected={expected:R}, tolerance={tolerance:R}");
        }

        [Test]
        public void TinyScalarsPreservePortableInputsAndSignedZeros()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (bool single in new[] { true, false })
            {
                var call = Scalar(assembly, single);
                foreach (double x in TinyInputs(single))
                    Check(x, call(x), single, assembly != typeof(Fun).Assembly, $"tiny single={single}");
            }
        }

        [Test]
        public void FiniteScalarsMatchRuntimeAcrossFormulationTransitions()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (bool single in new[] { true, false })
            {
                var call = Scalar(assembly, single);
                foreach (double x in FiniteInputs(single))
                    Check(x, call(x), single, assembly != typeof(Fun).Assembly, $"finite single={single}");
                if (assembly != typeof(Fun).Assembly)
                    foreach (double x in FiniteInputs(single).Where(x => x > 0))
                        Assert.That(BitConverter.DoubleToInt64Bits(call(-x)),
                            Is.EqualTo(BitConverter.DoubleToInt64Bits(call(x)) ^ long.MinValue), $"odd symmetry single={single}, x={x:R}");
            }
        }

        [Test]
        public void EndpointsAndOutsideDomainKeepExceptionalResults()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (bool single in new[] { true, false })
            {
                var call = Scalar(assembly, single);
                foreach (double value in ExceptionalInputs(single))
                {
                    double x = single ? (float)value : value;
                    Check(x, call(x), single, assembly != typeof(Fun).Assembly, $"exceptional single={single}");
                }
            }
        }

        [Test]
        public void FloatingVectorsApplyAtanhToEveryComponent()
        {
            foreach (var assembly in CoreAssemblies())
            {
                int overloads = 0;
                foreach (bool single in new[] { true, false })
                {
                    var data = TinyInputs(single).Concat(FiniteInputs(single)).Concat(ExceptionalInputs(single))
                        .Select(x => single ? (double)(float)x : x).ToArray();
                    var scalarType = single ? typeof(float) : typeof(double);
                    foreach (int dimension in new[] { 2, 3, 4 })
                    {
                        var vectorType = assembly.GetType($"Aardvark.Base.V{dimension}{(single ? "f" : "d")}", throwOnError: true)!;
                        var input = Expression.Parameter(typeof(double[]));
                        var vector = Expression.New(vectorType.GetConstructor(Enumerable.Repeat(scalarType, dimension).ToArray())!,
                            Enumerable.Range(0, dimension).Select(i => Expression.Convert(Expression.ArrayIndex(input, Expression.Constant(i)), scalarType)));
                        var method = assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.Atanh), new[] { vectorType })!;
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
                                Check(values[i], actual[i], single, assembly != typeof(Fun).Assembly, $"V{dimension}, single={single}, component {i}");
                        }
                    }
                }
                Assert.That(overloads, Is.EqualTo(6));
            }
        }
    }
}

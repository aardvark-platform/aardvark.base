using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Aardvark.Tests
{
    [TestFixture]
    public class AsinhTests
    {
        private static IEnumerable<Assembly> CoreAssemblies()
        {
            yield return typeof(Fun).Assembly;
            string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, path);
            yield return Assembly.LoadFile(path);
        }

        private static Func<T, T> Scalar<T>(Assembly assembly)
            => (Func<T, T>)assembly.GetType(typeof(Fun).FullName, true)
                .GetMethod(nameof(Fun.Asinh), new[] { typeof(T) }).CreateDelegate(typeof(Func<T, T>));

        private static bool Verify(double input, double actual, bool single, string context, bool fallback = false)
        {
            double expected = single ? MathF.Asinh((float)input) : Math.Asinh(input);
            long Bits(double value) => single ? BitConverter.SingleToInt32Bits((float)value) : BitConverter.DoubleToInt64Bits(value);
            bool correct;
            if (double.IsNaN(expected)) correct = double.IsNaN(actual);
            else if (double.IsInfinity(expected)) correct = actual == expected;
            else if (input == 0) correct = Bits(actual) == Bits(input);
            else
            {
                double tolerance = Math.Max(Math.Abs(expected) * (single ? 4e-7 : 1e-15), single ? float.Epsilon : double.Epsilon);
                correct = double.IsFinite(actual) && Math.Abs(actual - expected) <= tolerance;
                // Require exact subnormal preservation on both targets and exact tiny-input preservation in the fallback.
                // Native Asinh may differ from x by an ulp near powers of two; compare it to the runtime oracle above.
                if (Math.Abs(input) < (single ? 1.1754943508222875e-38 : 2.2250738585072014e-308) ||
                    fallback && Math.Abs(input) < (single ? 0.000244140625 : 1.4901161193847656e-8))
                    correct &= Bits(actual) == Bits(input);
            }
            Assert.That(correct, Is.True,
                $"{context}, x={input:R}, expected={expected:R}, actual={actual:R}, bits={Bits(actual):X16}");
            return correct;
        }

        private static double[] Regressions(bool single) => single
            ? new[] { (double)float.Epsilon, -(double)float.Epsilon, 1e-10f, -1e-10f, -1e10f, 1e10f, float.MaxValue, -float.MaxValue }
            : new[] { double.Epsilon, -double.Epsilon, 1e-100, -1e-100, -1e10, 1e10, 1e200, -1e200, double.MaxValue, -double.MaxValue };

        private static IEnumerable<double> PositiveValues(bool single)
        {
            var values = new HashSet<double>();
            void Add(double value)
            {
                if (single) value = (float)value;
                if (value > 0 && double.IsFinite(value)) values.Add(value);
            }
            // Every binary exponent, including the normal/subnormal boundary and all formulation cutoffs.
            for (int exponent = single ? -149 : -1074; exponent <= (single ? 127 : 1023); exponent++)
            {
                double power = Math.ScaleB(1.0, exponent);
                Add(power);
                Add(single ? MathF.BitDecrement((float)power) : Math.BitDecrement(power));
                Add(single ? MathF.BitIncrement((float)power) : Math.BitIncrement(power));
                Add(1.5 * power);
            }
            Add(single ? float.MaxValue : double.MaxValue);
            for (int i = 1; i <= 256; i++) Add(i / 16.0);
            var random = new Random(53819 + (single ? 32 : 64));
            var bytes = new byte[8];
            for (int i = 0; i < 512; i++)
            {
                random.NextBytes(bytes);
                Add(Math.Abs(single ? BitConverter.ToSingle(bytes, 0) : BitConverter.ToDouble(bytes, 0)));
            }
            return values;
        }

        [Test]
        public void FiniteScalarsMatchRuntimeAcrossRangesAndFormulationTransitions()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in CoreAssemblies())
                foreach (bool single in new[] { false, true })
                {
                    var f = single ? Scalar<float>(assembly) : null;
                    var d = single ? null : Scalar<double>(assembly);
                    double Apply(double input) => single ? f((float)input) : d(input);
                    string context = $"{assembly.Location}, {(single ? "float" : "double")}";
                    bool valid = true;
                    bool fallback = assembly != typeof(Fun).Assembly;
                    foreach (double input in Regressions(single)) valid &= Verify(input, Apply(input), single, context, fallback);
                    if (!valid) continue; // Bound original-source diagnostics while reporting both precisions.
                    foreach (double magnitude in PositiveValues(single))
                    {
                        if (!Verify(magnitude, Apply(magnitude), single, context, fallback) ||
                            !Verify(-magnitude, Apply(-magnitude), single, context, fallback)) break;
                    }
                }
            });
        }

        [Test]
        public void SpecialScalarsPreserveZeroSignInfinitiesAndNaN()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in CoreAssemblies())
                {
                    var f = Scalar<float>(assembly);
                    var d = Scalar<double>(assembly);
                    foreach (float x in new[] { 0f, BitConverter.Int32BitsToSingle(int.MinValue), float.PositiveInfinity,
                        float.NegativeInfinity, float.NaN, BitConverter.Int32BitsToSingle(0x7FC00042), BitConverter.Int32BitsToSingle(unchecked((int)0xFFC00042)) })
                        Verify(x, f(x), true, assembly.Location + ", float special");
                    foreach (double x in new[] { 0.0, BitConverter.Int64BitsToDouble(long.MinValue), double.PositiveInfinity,
                        double.NegativeInfinity, double.NaN, BitConverter.Int64BitsToDouble(0x7FF8000000000042),
                        BitConverter.Int64BitsToDouble(unchecked((long)0xFFF8000000000042)) })
                        Verify(x, d(x), false, assembly.Location + ", double special");
                }
            });
        }

        private static void Vectors<T>(Assembly assembly, bool single, T[] values)
        {
            string suffix = single ? "f" : "d";
            foreach (int dimension in new[] { 2, 3, 4 })
            {
                var type = assembly.GetType($"Aardvark.Base.V{dimension}{suffix}", true);
                var constructor = type.GetConstructor(Enumerable.Repeat(typeof(T), dimension).ToArray());
                var method = assembly.GetType(typeof(Fun).FullName, true).GetMethod(nameof(Fun.Asinh), new[] { type });
                var fields = new[] { "X", "Y", "Z", "W" }.Take(dimension).Select(name => type.GetField(name)).ToArray();
                Assert.That(constructor, Is.Not.Null, type.FullName);
                Assert.That(method, Is.Not.Null, type.FullName);
                for (int i = 0; i < values.Length; i++)
                {
                    // Use each assembly's own vector type; sibling vectors have distinct runtime identities.
                    object[] components = Enumerable.Range(0, dimension).Select(j => (object)values[(i + j) % values.Length]).ToArray();
                    object result = method.Invoke(null, new[] { constructor.Invoke(components) });
                    bool valid = true;
                    for (int j = 0; j < dimension; j++)
                        valid &= Verify(Convert.ToDouble(components[j]), Convert.ToDouble(fields[j].GetValue(result)), single,
                            $"{assembly.Location}, {type.Name}, row={i}, component={fields[j].Name}", assembly != typeof(Fun).Assembly);
                    if (!valid) break;
                }
            }
        }

        [Test]
        public void FloatingVectorsApplyAsinhToEveryComponent()
        {
            Assert.Multiple(() =>
            {
                foreach (var assembly in CoreAssemblies())
                foreach (bool single in new[] { false, true })
                {
                    var values = new List<double> { single ? float.Epsilon : double.Epsilon, -1e10,
                        single ? float.MaxValue : double.MaxValue, BitConverter.Int64BitsToDouble(long.MinValue), 0,
                        double.PositiveInfinity, double.NegativeInfinity, double.NaN, 0.125, -1, 2 };
                    values.AddRange(Regressions(single));
                    foreach (double cutoff in single ? new[] { 0.000244140625, 0.5, 4096.0 } : new[] { 1.4901161193847656e-8, 0.5, 67108864.0 })
                    {
                        values.Add(cutoff);
                        values.Add(single ? MathF.BitDecrement((float)cutoff) : Math.BitDecrement(cutoff));
                        values.Add(single ? MathF.BitIncrement((float)cutoff) : Math.BitIncrement(cutoff));
                        values.Add(-cutoff);
                    }
                    if (single) Vectors(assembly, true, values.Select(x => (float)x).ToArray());
                    else Vectors(assembly, false, values.ToArray());
                }
            });
        }
    }
}

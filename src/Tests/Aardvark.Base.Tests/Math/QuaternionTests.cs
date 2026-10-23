using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class QuaternionTests
    {
        private static readonly int iterations = 10000;

        private static QuaternionD GetRandomQuat(RandomSystem rnd)
        {
            return new QuaternionD(rnd.UniformV4d() * 10);
        }

        [Test]
        public static void Comparison()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q0 = GetRandomQuat(rnd);
                var q1 = new QuaternionD((V4d)q0 + V4d.OIOI);

                Assert.IsFalse(q0.Equals(q1));
                Assert.IsFalse(q0 == q1);
                Assert.IsTrue(q0 != q1);
            }
        }

        [Test]
        public static void HamiltonTest()
        {
            var i = QuaternionD.I;
            var j = QuaternionD.J;
            var k = QuaternionD.K;
            var minus_one = new QuaternionD(-1, 0, 0, 0);

            Assert.IsTrue(Fun.ApproximateEquals(i * i, minus_one, 0.00001));
            Assert.IsTrue(Fun.ApproximateEquals(j * j, minus_one, 0.00001));
            Assert.IsTrue(Fun.ApproximateEquals(k * k, minus_one, 0.00001));
            Assert.IsTrue(Fun.ApproximateEquals(i * j * k, minus_one, 0.00001));
        }

        [Test]
        public static void IdentityTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);
                var id = QuaternionD.Identity;

                Assert.IsTrue(Fun.ApproximateEquals(q * id, q, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(id * q, q, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(id * id, id, 0.00001));
            }
        }

        [Test]
        public static void NormalizeTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);

                // Normalized property
                var q1 = q.Normalized;

                // Normalize method
                var q2 = new QuaternionD(q);
                Quaternion.Normalize(ref q2);

                Assert.IsTrue(Fun.ApproximateEquals(q1.Norm, 1, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q2.Norm, 1, 0.00001));
            }
        }

        [Test]
        public static void InverseTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);
                var id = QuaternionD.Identity;

                // Inverse property
                var qi1 = q.Inverse;

                // Invert method
                var qi2 = new QuaternionD(q);
                Quaternion.Invert(ref qi2);

                Assert.IsTrue(Fun.ApproximateEquals(q * qi1, id, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(qi1 * q, id, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q * qi2, id, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(qi2 * q, id, 0.00001));
            }
        }

        [Test]
        public static void ConjugateTest()
        {
            var rnd = new RandomSystem(1);

            for (int n = 0; n < iterations; n++)
            {
                var q1 = GetRandomQuat(rnd);
                var q2 = GetRandomQuat(rnd);

                var q1_x_q2_conj = Quaternion.Conjugated(q1 * q2);
                var q2_conj_x_q1_conj = q2.Conjugated * q1.Conjugated;

                Assert.IsTrue(Fun.ApproximateEquals(q1_x_q2_conj, q2_conj_x_q1_conj, 0.00001));

                var i = QuaternionD.I;
                var j = QuaternionD.J;
                var k = QuaternionD.K;
                var q1_conj = -0.5 * (q1 + (i * q1 * i) + (j * q1 * j) + (k * q1 * k));

                Assert.IsTrue(Fun.ApproximateEquals(q1.Conjugated, q1_conj, 0.00001));
            }
        }

        [Test]
        public static void NormTest()
        {
            var rnd = new RandomSystem(1);

            for (int n = 0; n < iterations; n++)
            {
                var q1 = GetRandomQuat(rnd);
                var q2 = GetRandomQuat(rnd);

                var s = rnd.UniformDouble() * 100;
                var q1_x_s_norm = Quaternion.Norm(q1 * s);
                var q1_norm_x_s_norm = q1.Norm * s;

                Assert.IsTrue(Fun.ApproximateEquals(q1_x_s_norm, q1_norm_x_s_norm, 0.00001));

                var q1_x_q2_norm = Quaternion.Norm(q1 * q2);
                var q1_norm_x_q2_norm = q1.Norm * q2.Norm;

                Assert.IsTrue(Fun.ApproximateEquals(q1_x_q2_norm, q1_norm_x_q2_norm, 0.00001));
            }
        }

        [Test]
        [Obsolete]
        public static void MatrixRepresentation()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q1 = GetRandomQuat(rnd);
                var q2 = GetRandomQuat(rnd);

                var m1 = (M44d)q1;
                var m2 = (M44d)q2;

                var q1_plus_q2 = (M44d)(q1 + q2);
                var m1_plus_m2 = m1 + m2;

                var q1q2 = (M44d)(q1 * q2);
                var m1m2 = m1 * m2;

                var q2q1 = (M44d)(q2 * q1);
                var m2m1 = m2 * m1;

                Assert.IsTrue(Fun.ApproximateEquals(q1_plus_q2, m1_plus_m2, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q1q2, m1m2, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q2q1, m2m1, 0.00001));
            }
        }

        [Test]
        public static void ToStringAndParse()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);

                var str = q.ToString();
                var parsed = QuaternionD.Parse(str);

                Assert.IsTrue(Fun.ApproximateEquals(parsed, q, 0.00001));
            }
        }

        private static IEnumerable<Assembly> NormalizationCores()
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            yield return typeof(QuaternionD).Assembly;
            yield return Assembly.LoadFile(path);
        }

        // Target-owned quaternion structs; only primitive arrays cross assemblies.
        // Mode 3 independently retains the original normalization arithmetic.
        private static Func<T[], T[]> Normalizer<T>(Assembly core, int mode)
        {
            var scalar = typeof(T);
            var type = core.GetType("Aardvark.Base.Quaternion" + (scalar == typeof(float) ? "F" : "D"), true)!;
            var methods = core.GetType("Aardvark.Base.Quaternion", true)!;
            var input = Expression.Parameter(typeof(T[]));
            var q = Expression.Variable(type); var norm = Expression.Variable(scalar); var scale = Expression.Variable(scalar);
            var fields = new[] { "W", "X", "Y", "Z" }.Select(f => Expression.Field(q, f)).ToArray();
            var body = new List<Expression> { Expression.Assign(q, Expression.New(type.GetConstructor(Enumerable.Repeat(scalar, 4).ToArray())!,
                Enumerable.Range(0, 4).Select(i => Expression.ArrayIndex(input, Expression.Constant(i))))) };
            if (mode == 0) body.Add(Expression.Assign(q, Expression.Property(q, "Normalized")));
            else if (mode == 1) body.Add(Expression.Assign(q, Expression.Call(methods.GetMethod("Normalized", new[] { type })!, q)));
            else if (mode == 2) body.Add(Expression.Call(methods.GetMethod("Normalize", new[] { type.MakeByRefType() })!, q));
            else
            {
                body.Add(Expression.Assign(norm, Expression.Property(q, "Norm")));
                var operations = new List<Expression> { Expression.Assign(scale, Expression.Divide(Expression.Constant(FromDouble<T>(1)), norm)) };
                operations.AddRange(fields.Select(f => Expression.Assign(f, Expression.Multiply(f, scale))));
                body.Add(Expression.IfThen(Expression.GreaterThan(norm, Expression.Constant(FromDouble<T>(0))), Expression.Block(operations)));
            }
            body.Add(Expression.NewArrayInit(scalar, fields));
            return Expression.Lambda<Func<T[], T[]>>(Expression.Block(new[] { q, norm, scale }, body), input).Compile();
        }

        private static T FromDouble<T>(double x) => typeof(T) == typeof(float) ? (T)(object)(float)x : (T)(object)x;
        private static double AsDouble<T>(T x) => Convert.ToDouble(x);
        private static long NormalizationBits<T>(T x) => typeof(T) == typeof(float)
            ? BitConverter.SingleToInt32Bits((float)(object)x!) : BitConverter.DoubleToInt64Bits((double)(object)x!);
        private static double UnitRoundoff<T>() => typeof(T) == typeof(float) ? 5.9604644775390625e-8 : 1.1102230246251565e-16;
        private static string NormalizationContext<T>(Assembly core, T[] q, string label)
            => $"{core.Location}: {typeof(T).Name}, {label}, q=({string.Join(",", q.Select(x => AsDouble(x).ToString("R")))})";

        private static IEnumerable<double> NormalizationScales<T>()
        {
            bool single = typeof(T) == typeof(float);
            var values = new HashSet<double>();
            void Add(double x) { x = AsDouble(FromDouble<T>(x)); if (x > 0 && double.IsFinite(x)) values.Add(x); }
            double max = single ? float.MaxValue : double.MaxValue;
            double minimum = single ? float.Epsilon : double.Epsilon;
            double normal = Math.ScaleB(1, single ? -126 : -1022);
            Add(max); Add(minimum); Add(normal);
            for (int e = single ? -149 : -1074; e <= (single ? 127 : 1023); e++) Add(Math.ScaleB(1, e));
            double halfSubnormalRoot = single ? Math.ScaleB(1, -75) : Math.ScaleB(Math.Sqrt(0.5), -537);
            foreach (double center in new[] { Math.Sqrt(normal), Math.Sqrt(normal) * 0.5, Math.Sqrt(max), Math.Sqrt(max) * 0.5,
                Math.Sqrt(minimum), halfSubnormalRoot, halfSubnormalRoot * 0.5, normal, minimum })
            {
                double low = AsDouble(FromDouble<T>(center)), high = low;
                Add(low);
                for (int i = 0; i < 5; i++)
                {
                    low = single ? MathF.BitDecrement((float)low) : Math.BitDecrement(low);
                    high = single ? MathF.BitIncrement((float)high) : Math.BitIncrement(high);
                    Add(low); Add(high);
                }
            }
            return values;
        }

        private static void CheckUnitQuaternion<T>(Assembly core, T[] input, Func<T[], T[]>[] normalize, string label)
        {
            var v = input.Select(AsDouble).ToArray();
            int exponent = v.Where(x => x != 0).Max(x => Math.ILogB(Math.Abs(x)));
            // Power-of-two scaling, rather than division by the largest magnitude.
            var s = v.Select(x => Math.ScaleB(x, -exponent)).ToArray();
            double length = Math.Sqrt((s[0] * s[0] + s[1] * s[1]) + (s[2] * s[2] + s[3] * s[3]));
            var expected = s.Select(x => x / length).ToArray();
            double tiny = typeof(T) == typeof(float) ? float.Epsilon : double.Epsilon;
            string context = NormalizationContext(core, input, label);
            for (int mode = 0; mode < normalize.Length; mode++)
            {
                var actual = normalize[mode](input);
                double square = 0;
                for (int i = 0; i < 4; i++)
                {
                    double value = AsDouble(actual[i]);
                    Assert.That(double.IsFinite(value), Is.True, $"{context}, api={mode}, field={i}");
                    Assert.That(value, Is.EqualTo(expected[i]).Within(8 * UnitRoundoff<T>() * Math.Abs(expected[i]) + tiny), $"{context}, api={mode}, field={i}");
                    if (v[i] == 0 || value == 0)
                        Assert.That(NormalizationBits(actual[i]), Is.EqualTo(NormalizationBits(FromDouble<T>(Math.CopySign(0, v[i])))), $"{context}, api={mode}, zero field={i}");
                    square += value * value;
                }
                Assert.That(Math.Sqrt(square), Is.EqualTo(1).Within(12 * UnitRoundoff<T>()), $"{context}, api={mode}, length");
            }
        }

        [Test]
        public static void ExtremeNormalizationMatchesPowerOfTwoOracle()
        {
            foreach (var core in NormalizationCores()) { CheckExtremeNormalization<float>(core); CheckExtremeNormalization<double>(core); }
        }

        private static void CheckExtremeNormalization<T>(Assembly core)
        {
            var normalize = Enumerable.Range(0, 3).Select(mode => Normalizer<T>(core, mode)).ToArray();
            var patterns = new[] { new[] { 1.0, -1.0, 1.0, -1.0 }, new[] { 1.0, 0.5, -0.25, 0.125 }, new[] { 1.0, -0.75, 0.625, -0.5 } };
            foreach (double magnitude in NormalizationScales<T>())
            foreach (int sign in new[] { -1, 1 })
            {
                for (int axis = 0; axis < 4; axis++)
                {
                    var input = new T[4]; input[axis] = FromDouble<T>(sign * magnitude);
                    CheckUnitQuaternion(core, input, normalize, $"axis={axis}, magnitude={magnitude:R}");
                }
                foreach (var pattern in patterns)
                for (int rotation = 0; rotation < 4; rotation++)
                {
                    var input = Enumerable.Range(0, 4).Select(i => FromDouble<T>(sign * magnitude * pattern[(i + rotation) % 4])).ToArray();
                    CheckUnitQuaternion(core, input, normalize, $"mixed rotation={rotation}, magnitude={magnitude:R}");
                }
            }
        }

        [Test]
        public static void NormalizationKeepsOrdinaryArithmeticAndNonfiniteInputs()
        {
            foreach (var core in NormalizationCores()) { CheckLegacyNormalization<float>(core); CheckLegacyNormalization<double>(core); }
        }

        private static void CheckLegacyNormalization<T>(Assembly core)
        {
            bool single = typeof(T) == typeof(float);
            var normalize = Enumerable.Range(0, 3).Select(mode => Normalizer<T>(core, mode)).ToArray();
            var legacy = Normalizer<T>(core, 3);
            void Check(T[] input, string label)
            {
                var expected = legacy(input); string context = NormalizationContext(core, input, label);
                for (int mode = 0; mode < 3; mode++)
                {
                    var actual = normalize[mode](input);
                    for (int i = 0; i < 4; i++) Assert.That(NormalizationBits(actual[i]), Is.EqualTo(NormalizationBits(expected[i])), $"{context}, api={mode}, field={i}");
                }
            }
            int seed = single ? 61831 : 61832;
            var random = new Random(seed);
            for (int sample = 0; sample < 4096; sample++)
            {
                int exponent = random.Next(single ? -20 : -100, single ? 21 : 101);
                var input = Enumerable.Range(0, 4).Select(i => FromDouble<T>(Math.ScaleB(random.NextDouble() * 4 - 2, exponent))).ToArray();
                Check(input, $"seed={seed}, sample={sample}");
            }
            // Exact original arithmetic at representable, finite squared-norm boundaries.
            foreach (double magnitude in NormalizationScales<T>())
            {
                var input = new[] { magnitude, -magnitude * 0.5, magnitude * 0.25, -magnitude * 0.125 }.Select(FromDouble<T>).ToArray();
                double squared;
                if (single)
                {
                    var a = input.Select(x => (float)(object)x!).ToArray();
                    squared = a[0] * a[0] + a[1] * a[1] + a[2] * a[2] + a[3] * a[3];
                }
                else { var a = input.Select(AsDouble).ToArray(); squared = a[0] * a[0] + a[1] * a[1] + a[2] * a[2] + a[3] * a[3]; }
                if (squared >= Math.ScaleB(1, single ? -126 : -1022) && double.IsFinite(squared)) Check(input, $"ordinary boundary={magnitude:R}");
            }
            var specials = single
                ? new[] { float.PositiveInfinity, float.NegativeInfinity, BitConverter.Int32BitsToSingle(0x7fc12345), BitConverter.Int32BitsToSingle(unchecked((int)0xffc54321)), BitConverter.Int32BitsToSingle(0x7f812345) }.Select(x => (T)(object)x).ToArray()
                : new[] { double.PositiveInfinity, double.NegativeInfinity, BitConverter.Int64BitsToDouble(0x7ff8000000001234), BitConverter.Int64BitsToDouble(unchecked((long)0xfff8000000005678)), BitConverter.Int64BitsToDouble(0x7ff0000000001234) }.Select(x => (T)(object)x).ToArray();
            foreach (var special in specials)
            for (int position = 0; position < 4; position++)
            {
                var input = new[] { 1.0, -2.0, 0.0, -0.0 }.Select(FromDouble<T>).ToArray(); input[position] = special;
                Check(input, $"special at {position}");
                for (int second = 0; second < specials.Length; second++)
                {
                    var mixed = (T[])input.Clone(); mixed[(position + 1) % 4] = specials[second];
                    Check(mixed, $"special pair at {position}, second={second}");
                }
            }
        }

        [Test]
        public static void NormalizationRetainsSignedZerosAcrossScales()
        {
            foreach (var core in NormalizationCores()) { CheckNormalizationZeros<float>(core); CheckNormalizationZeros<double>(core); }
        }

        private static void CheckNormalizationZeros<T>(Assembly core)
        {
            bool single = typeof(T) == typeof(float);
            var normalize = Enumerable.Range(0, 3).Select(mode => Normalizer<T>(core, mode)).ToArray();
            for (int mask = 0; mask < 16; mask++)
            {
                var zeros = Enumerable.Range(0, 4).Select(i => FromDouble<T>((mask & (1 << i)) == 0 ? 0.0 : -0.0)).ToArray();
                foreach (var magnitude in new[] { 0.0, 1.0, 2.0, single ? float.MaxValue : double.MaxValue, single ? float.Epsilon : double.Epsilon,
                    Math.ScaleB(1, single ? -126 : -1022), Math.ScaleB(1, single ? -75 : -538) })
                for (int axis = 0; axis < 4; axis++)
                foreach (int sign in new[] { -1, 1 })
                {
                    var input = (T[])zeros.Clone();
                    if (magnitude != 0) input[axis] = FromDouble<T>(sign * magnitude);
                    var expected = (T[])zeros.Clone(); if (magnitude != 0) expected[axis] = FromDouble<T>(sign);
                    for (int mode = 0; mode < 3; mode++)
                    {
                        var actual = normalize[mode](input);
                        for (int i = 0; i < 4; i++) Assert.That(NormalizationBits(actual[i]), Is.EqualTo(NormalizationBits(expected[i])),
                            $"{core.Location}: {typeof(T).Name}, zero mask={mask}, axis={axis}, sign={sign}, magnitude={magnitude:R}, api={mode}, field={i}");
                    }
                }
            }
        }
    }
}
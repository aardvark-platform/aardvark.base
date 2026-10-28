using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

using Num = System.Numerics;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class ComplexTests
    {
        private static readonly int Iterations = 10000;

        private static readonly double Epsilon = 1e-8;

        private static bool IsZero(this Num.Complex x)
            => x.Real == 0 && x.Imaginary == 0;

        private static void AreEqual(Num.Complex a, ComplexD b)
        {
            bool cond =
                (a.Real.ApproximateEquals(b.Real, Epsilon) &&
                a.Imaginary.ApproximateEquals(b.Imag, Epsilon)) ||
                (a.Real.Equals(b.Real) && a.Imaginary.Equals(b.Imag));

            if (!cond) Debugger.Break();
            Assert.IsTrue(cond, "{0} != {1}", a, b);
        }

        private static void AreEqualAngle(Num.Complex a, ComplexD b)
        {
            AreEqual(
                new Num.Complex(a.Real % Constant.PiHalf, a.Imaginary), 
                new ComplexD(b.Real % Constant.PiHalf, b.Imag)
            );
        }

        private static void AreEqual(dynamic a, dynamic b)
        {
            bool cond = Fun.ApproximateEquals(a, b, Epsilon) || a.Equals(b);

            if (!cond) Debugger.Break();
            Assert.IsTrue(cond, "{0} != {1}", a, b);
        }

        private static void AssertClose(double expected, double actual, double relativeTolerance = 4e-15, double absoluteTolerance = 0.0)
        {
            if (expected.Equals(actual)) return;
            double tolerance = Math.Max(absoluteTolerance, relativeTolerance * Math.Max(Math.Abs(expected), Math.Abs(actual)));
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }

        private static void AssertClose(float expected, float actual, float relativeTolerance = 3e-6f, float absoluteTolerance = 0.0f)
        {
            if (expected.Equals(actual)) return;
            float tolerance = Math.Max(absoluteTolerance, relativeTolerance * Math.Max(Math.Abs(expected), Math.Abs(actual)));
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }

        private static void AssertClose(ComplexD expected, ComplexD actual, double relativeTolerance = 4e-15, double absoluteTolerance = 0.0)
        {
            AssertClose(expected.Real, actual.Real, relativeTolerance, absoluteTolerance);
            AssertClose(expected.Imag, actual.Imag, relativeTolerance, absoluteTolerance);
        }

        private static void AssertClose(ComplexF expected, ComplexF actual, float relativeTolerance = 3e-6f, float absoluteTolerance = 0.0f)
        {
            AssertClose(expected.Real, actual.Real, relativeTolerance, absoluteTolerance);
            AssertClose(expected.Imag, actual.Imag, relativeTolerance, absoluteTolerance);
        }

        private static double ScaledNorm(double real, double imag)
        {
            double ar = Math.Abs(real);
            double ai = Math.Abs(imag);
            double max = Math.Max(ar, ai);
            if (max == 0) return 0;
            double ratio = Math.Min(ar, ai) / max;
            return max * Math.Sqrt(1 + ratio * ratio);
        }

        private static float ScaledNorm(float real, float imag)
        {
            float ar = Math.Abs(real);
            float ai = Math.Abs(imag);
            float max = Math.Max(ar, ai);
            if (max == 0) return 0;
            float ratio = Math.Min(ar, ai) / max;
            return max * MathF.Sqrt(1 + ratio * ratio);
        }

        private static void GetRandomComplex(RandomSystem rnd, out Num.Complex c1, out ComplexD c2, bool withInf = true)
        {
            var type = rnd.UniformDouble();
            
            if (type < 0.1)
            {
                var v = rnd.UniformV2i(2);
                c1 = new Num.Complex(v.X, v.Y);
                c2 = new ComplexD(v.X, v.Y);
            }
            else if (type < 0.2 && withInf)
            {
                var i = rnd.UniformV2i(3);
                var v = new V2d(
                    (i.X == 0) ? 0 : ((i.X == 1) ? double.NegativeInfinity : double.PositiveInfinity),
                    (i.Y == 0) ? 0 : ((i.Y == 1) ? double.NegativeInfinity : double.PositiveInfinity)
                );

                c1 = new Num.Complex(v.X, v.Y);
                c2 = new ComplexD(v.X, v.Y);
            }
            else
            {
                var v = (rnd.UniformV2d() - 0.5) * 100;
                if (type < 0.4)
                {
                    c1 = new Num.Complex(v.X, 0);
                    c2 = new ComplexD(v.X, 0);
                }
                else if (type < 0.5)
                {
                    c1 = new Num.Complex(0, v.Y);
                    c2 = new ComplexD(0, v.Y);
                }
                else
                {
                    c1 = new Num.Complex(v.X, v.Y);
                    c2 = new ComplexD(v.X, v.Y);
                }
            }
        }

        private static void GenericTest(Action<RandomSystem, int> f)
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iterations; i++)
            {
                f(rnd, i);
            }
        }

        private static void GenericTest(Action<RandomSystem> f)
            => GenericTest((rnd, i) => f(rnd));

        private static void UnaryTest(Func<Num.Complex, Num.Complex> fa, Func<ComplexD, ComplexD> fb, bool withInf = true)
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex a, out ComplexD b, withInf);
                AreEqual(fa(a), fb(b));
            });

        private static void UnaryAngleTest(Func<Num.Complex, Num.Complex> fa, Func<ComplexD, ComplexD> fb)
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex a, out ComplexD b);
                AreEqualAngle(fa(a), fb(b));
            });

        private static void UnaryRealTest(Func<Num.Complex, double> fa, Func<ComplexD, double> fb)
            => UnaryTest(
                c => new Num.Complex(fa(c), 0),
                c => new ComplexD(fb(c), 0)
            );

        private static void BinaryTest(Func<Num.Complex, Num.Complex, Num.Complex> fa, Func<ComplexD, ComplexD, ComplexD> fb, bool withInf = true)
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex a1, out ComplexD b1, withInf);
                GetRandomComplex(rnd, out Num.Complex a2, out ComplexD b2, withInf);
                AreEqual(fa(a1, a2), fb(b1, b2));
            });

        private static void BinaryPowTest(Func<Num.Complex, Num.Complex, Num.Complex> fa, Func<ComplexD, ComplexD, ComplexD> fb, bool withInf = true)
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex a1, out ComplexD b1, withInf);
                GetRandomComplex(rnd, out Num.Complex a2, out ComplexD b2, withInf);
                var a = fa(a1, a2);

                var tmp = fb(b1, b2);
                var b = new Num.Complex(tmp.Real, tmp.Imag);

                if (!a.Equals(b))
                    AreEqual(a / b, ComplexD.One);
            });

        private static void UnaryPowTest(Func<Num.Complex, Num.Complex> fa, Func<ComplexD, ComplexD> fb, bool withInf = true)
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex a, out ComplexD b, withInf);
                var x = fa(a);

                var tmp = fb(b);
                var y = new Num.Complex(tmp.Real, tmp.Imag);
 
                if (!x.Equals(y))
                    AreEqual(x / y, ComplexD.One);
            });

        #region ToString and Parse

        [Test]
        public static void ToStringAndParse()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c);
                var str = c.ToString();
                var x = ComplexD.Parse(str);

                AreEqual(c, x);
            });

        #endregion

        #region Properties

        [Test]
        public static void Conjugated()
            => UnaryTest(
                Num.Complex.Conjugate,
                Complex.Conjugated
            );

        [Test]
        public static void NormSquared()
            => UnaryRealTest(
                c => c.Magnitude * c.Magnitude,
                Complex.NormSquared
            );

        [Test]
        public static void Norm()
            => UnaryRealTest(
                c => c.Magnitude,
                Complex.Norm
            );

        [Test]
        public static void Argument()
            => UnaryRealTest(
                c => c.Phase,
                Complex.Argument
            );

        #endregion

        #region Binary operations

        [Test]
        public static void Addition()
            => BinaryTest(
                (a, b) => a + b,
                (a, b) => a + b
            );

        [Test]
        public static void AdditionReal()
            => BinaryTest(
                (a, b) => b.Real + a + b.Real,
                (a, b) => b.Real + a + b.Real
            );

        [Test]
        public static void Subtraction()
            => BinaryTest(
                (a, b) => a - b,
                (a, b) => a - b
            );

        [Test]
        public static void SubtractionReal()
            => BinaryTest(
                (a, b) => b.Real - a - b.Real,
                (a, b) => b.Real - a - b.Real
            );

        [Test]
        public static void Multiplication()
            => BinaryTest(
                (a, b) => a * b,
                (a, b) => a * b
            );

        [Test]
        public static void MultiplicationReal()
            => BinaryTest(
                (a, b) => b.Real * a * b.Real,
                (a, b) => b.Real * a * b.Real,
                false
            );

        [Test]
        public static void Divison()
            => BinaryTest(
                (a, b) => (b.Magnitude != 0) ? (a / b) : Num.Complex.Zero,
                (a, b) => (b.Norm != 0) ? (a / b) : ComplexD.Zero,
                false
            );

        [Test]
        public static void DivisionReal()
            => BinaryTest(
                (a, b) => (a.Magnitude != 0 && b.Real != 0) ? (b.Real / a / b.Real) : Num.Complex.Zero,
                (a, b) => (a.Norm != 0 && b.Real != 0) ? (b.Real / a / b.Real) : ComplexD.Zero,
                false
            );

        #endregion

        #region Fun methods

        [Test]
        public static void Power()
            => BinaryPowTest(
                (a, b) =>
                {
                    if (a.IsZero() && b.IsZero())
                        return Num.Complex.Zero;
                    else
                        return Num.Complex.Pow(a, b);
                },
                (a, b) =>
                {
                    if (a.IsZero && b.IsZero)
                        return ComplexD.Zero;
                    else
                        return Fun.Pow(a, b);
                }
            );

        [Test]
        public static void PowerReal()
            => BinaryPowTest(
                (a, b) =>
                {
                    if ((a.Real == 0 && b.IsZero()) || (a.IsZero() && b.Real == 0))
                        return Num.Complex.Zero;
                    else
                        return Num.Complex.Pow(a.Real, b) + Num.Complex.Pow(a, b.Real);
                },
                (a, b) =>
                {
                    if ((a.Real == 0 && b.IsZero) || (a.IsZero && b.Real == 0))
                        return ComplexD.Zero;
                    else
                        return Fun.Pow(a.Real, b) + Fun.Pow(a, b.Real);
                },
                false
            );

        [Test]
        public static void Acos()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);
                var x = Fun.Cos(Fun.Acos(c));
                AreEqual(c, x);
            });

        [Test]
        public static void Acosh()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);
                var x = Fun.Cosh(Fun.Acosh(c));
                AreEqual(c, x);
            });

        [Test]
        public static void Cosh()
            => UnaryPowTest(
                Num.Complex.Cosh,
                Fun.Cosh
            );

        [Test]
        public static void Cos()
            => UnaryPowTest(
                Num.Complex.Cos,
                Fun.Cos
            );

        [Test]
        public static void Asin()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);
                var x = Fun.Sin(Fun.Asin(c));
                AreEqual(c, x);
            });

        [Test]
        public static void Asinh()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);
                var x = Fun.Sinh(Fun.Asinh(c));
                AreEqual(c, x);
            });

        [Test]
        public static void Sinh()
            => UnaryPowTest(
                Num.Complex.Sinh,
                Fun.Sinh
            );

        [Test]
        public static void Sin()
            => UnaryPowTest(
                Num.Complex.Sin,
                Fun.Sin
            );

        [Test]
        public static void Atan()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex a, out ComplexD b);
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);

                // https://mathworld.wolfram.com/InverseTangent.html
                var values = new Dictionary<ComplexD, ComplexD>()
                {
                    { ComplexD.NegativeInfinity, new ComplexD(-Constant.PiHalf) },
                    { ComplexD.PositiveInfinity, new ComplexD(Constant.PiHalf) },
                    { ComplexD.I, new ComplexD(0, double.PositiveInfinity) },
                    { -ComplexD.I, new ComplexD(0, double.NegativeInfinity) },
                    { ComplexD.Zero, ComplexD.Zero },
                };

                Num.Complex x;

                if (values.TryGetValue(b, out ComplexD reference))
                    x = new Num.Complex(reference.Real, reference.Imag);
                else
                    x = Num.Complex.Atan(a);

                var y = Fun.Atan(b);

                AreEqualAngle(x, y);  
                AreEqual(c, Fun.Tan(Fun.Atan(c)));
            });

        [Test]
        public static void Atanh()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex a, out ComplexD b);
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);

                // https://mathworld.wolfram.com/InverseHyperbolicTangent.html
                var values = new Dictionary<ComplexD, ComplexD>()
                {
                    { ComplexD.Zero, ComplexD.Zero },
                    { ComplexD.One, ComplexD.PositiveInfinity },
                    { ComplexD.PositiveInfinity, -Constant.PiHalf * ComplexD.I },
                    { ComplexD.I, Constant.PiQuarter * ComplexD.I }
                };

                if (values.TryGetValue(b, out ComplexD reference))
                {
                    var x = new Num.Complex(reference.Real, reference.Imag);
                    var y = Fun.Atanh(b);
                    AreEqualAngle(x, y);
                }

                AreEqual(c, Fun.Tanh(Fun.Atanh(c)));
            });

        [Test]
        public static void Tanh()
            => UnaryPowTest(
                Num.Complex.Tanh,
                Fun.Tanh,
                false
            );

        [Test]
        public static void Tan()
            => UnaryPowTest(
                Num.Complex.Tan,
                Fun.Tan,
                false
            );

        [Test]
        public static void Sqrt()
            => UnaryTest(
                Num.Complex.Sqrt,
                Fun.Sqrt,
                false
            );

        [Test]
        public static void Exp()
            => UnaryTest(
                Num.Complex.Exp,
                Fun.Exp
            );

        [Test]
        public static void Ln()
            => UnaryTest(
                c => c.IsZero() ? Num.Complex.Zero : Num.Complex.Log(c),
                c => c.IsZero ? ComplexD.Zero : Fun.Log(c)
            );

        [Test]
        public static void Log()
            => BinaryTest(
                (c, basis) =>
                {
                    if (c.IsZero() || basis.Real.Abs() <= 1)
                        return Num.Complex.Zero;
                    else
                        return Num.Complex.Log(c, basis.Real.Abs());
                },
                (c, basis) =>
                {
                    if (c.IsZero || basis.Real.Abs() <= 1)
                        return ComplexD.Zero;
                    else
                        return Fun.Log(c, basis.Real.Abs());
                },
                false
            );

        [Test]
        public static void Log10()
            => UnaryTest(
                c => c.IsZero() ? Num.Complex.Zero : Num.Complex.Log10(c),
                c => c.IsZero ? ComplexD.Zero : Fun.Log10(c),
                false
            );

        [Test]
        public static void Log2()
            => UnaryTest(
                c => c.IsZero() ? Num.Complex.Zero : Num.Complex.Log(c) * Constant.Ln2Inv,
                c => c.IsZero ? ComplexD.Zero : Fun.Log2(c),
                false
            );

        [Test]
        public static void Cbrt()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);
                var x = c.Cbrt();
                var y = c.Root(3);
                AreEqual(x * x * x, c);
                AreEqual(x, y[0]);
            });

        [Test]
        public static void Csqrt()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);
                c = new ComplexD(c.Real, 0);
                var x = c.Sqrt();

                var y = c.Real.Csqrt();
                var z = c.Csqrt();
                var w = c.Root(2);

                AreEqual(x, y);
                AreEqual(x, z[0]);
                AreEqual(x, w[0]);
            });

        [Test]
        public static void SignedMinimumRealRootsMatchNumerics()
        {
            foreach (var core in RealRootCores())
            foreach (var type in new[] { typeof(int), typeof(long) })
            {
                long value = type == typeof(int) ? int.MinValue : long.MinValue;
                var actual = CheckIntegerRealRoot(core, type, value, false);
                Assert.That(double.IsFinite(actual.Imag) && actual.Imag > 0, Is.True, core.Name + "/" + type.Name);
            }
        }

        [Test]
        public static void OrdinaryIntegerRealRootsKeepTheirRounding()
        {
            foreach (var core in RealRootCores())
            foreach (var type in new[] { typeof(int), typeof(long) })
            foreach (var value in OrdinaryRootInputs(type == typeof(long)))
                CheckIntegerRealRoot(core, type, value, true);
        }

        [Test]
        public static void OtherRealRootsKeepZerosAndExceptionalValues()
        {
            foreach (var core in RealRootCores())
            {
                for (var value = (int)sbyte.MinValue; value <= sbyte.MaxValue; value++)
                    CheckIntegerRealRoot(core, typeof(sbyte), value, true);
                var random = new Random(75143);
                var shorts = new[] { short.MinValue, short.MinValue + 1, -16384, -1024, -81, -16, -9, -4, -2, -1, 0, 1, 2, 4, 9, 16, 81, 1024, 16384, short.MaxValue - 1, short.MaxValue };
                foreach (var value in shorts.Concat(Enumerable.Range(0, 1024).Select(_ => random.Next(short.MinValue, short.MaxValue + 1))))
                    CheckIntegerRealRoot(core, typeof(short), value, true);

                foreach (var single in new[] { true, false })
                {
                    var type = single ? typeof(float) : typeof(double);
                    var exponents = single ? new[] { -149, -126, -64, -1, 0, 1, 64, 127 } : new[] { -1074, -1022, -512, -1, 0, 1, 512, 1023 };
                    var finite = exponents.Select(e => Math.ScaleB(1, e)).Concat(new[] { 0.125, 2.0, 4, 9, 16, 81, single ? float.MaxValue : double.MaxValue });
                    foreach (var magnitude in finite)
                    foreach (var sign in new[] { -1, 1 })
                        CheckOtherFloatingRoot(core, type, magnitude * sign);
                    foreach (var special in new[] { 0.0, -0.0, double.PositiveInfinity, double.NegativeInfinity, double.NaN })
                        CheckOtherFloatingRoot(core, type, special);
                }
            }
        }

        private static (double Real, double Imag) CheckIntegerRealRoot(RealRootCore core, Type type, long value, bool legacy)
        {
            var actual = core.Evaluate(type, Convert.ChangeType(value, type));
            var expected = Num.Complex.Sqrt(new Num.Complex((double)value, 0));
            var context = core.Name + "/" + type.Name + ": " + value;
            AssertRootBits(expected.Real, actual.Real, context + " real");
            AssertRootBits(expected.Imaginary, actual.Imag, context + " imaginary");
            if (legacy)
            {
                // The minimum long is excluded here: characterize the former
                // integer-negation path only where it already returned a root.
                var component = Math.Sqrt((double)(value < 0 ? -value : value));
                AssertRootBits(value < 0 ? 0 : component, actual.Real, context + " legacy real");
                AssertRootBits(value < 0 ? component : 0, actual.Imag, context + " legacy imaginary");
            }
            return actual;
        }

        private static void CheckOtherFloatingRoot(RealRootCore core, Type type, double input)
        {
            var single = type == typeof(float);
            var value = single ? (double)(float)input : input;
            var actual = core.Evaluate(type, single ? (object)(float)value : (object)value);
            var magnitude = value >= 0 ? value : -value;
            var component = single ? (double)(core.Modern ? MathF.Sqrt((float)magnitude) : (float)Math.Sqrt(magnitude)) : Math.Sqrt(magnitude);
            var context = core.Name + "/" + type.Name + ": " + value.ToString("R");
            AssertRootBits(value >= 0 ? component : 0, actual.Real, context + " legacy real");
            AssertRootBits(value >= 0 ? 0 : component, actual.Imag, context + " legacy imaginary");
            if (double.IsFinite(value) && value != 0)
            {
                var reference = Num.Complex.Sqrt(new Num.Complex(value, 0));
                if (single)
                {
                    // This independent reference takes a double root before
                    // narrowing; keep its possible one-ULP difference scoped
                    // to unchanged float controls, never integer roots.
                    Assert.That(Math.Abs((long)BitConverter.SingleToInt32Bits((float)actual.Real) - BitConverter.SingleToInt32Bits((float)reference.Real)), Is.LessThanOrEqualTo(1), context);
                    Assert.That(Math.Abs((long)BitConverter.SingleToInt32Bits((float)actual.Imag) - BitConverter.SingleToInt32Bits((float)reference.Imaginary)), Is.LessThanOrEqualTo(1), context);
                }
                else
                {
                    AssertRootBits(reference.Real, actual.Real, context + " reference real");
                    AssertRootBits(reference.Imaginary, actual.Imag, context + " reference imaginary");
                }
            }
        }

        private static void AssertRootBits(double expected, double actual, string context)
        {
            if (double.IsNaN(expected)) Assert.That(double.IsNaN(actual), Is.True, context);
            else Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context);
        }

        private static IEnumerable<long> OrdinaryRootInputs(bool wide)
        {
            long min = wide ? long.MinValue : int.MinValue;
            long max = wide ? long.MaxValue : int.MaxValue;
            var values = new HashSet<long> { -1, 0, 1, min + 1, min + 2, min + 3, min + 7, min + 16, min + 1024, max, max - 1, max - 2 };
            void Add(long value) { if (value > min && value <= max) values.Add(value); }
            for (var exponent = 0; exponent < (wide ? 63 : 31); exponent++)
            {
                var power = 1L << exponent;
                foreach (var delta in new[] { -2, -1, 0, 1, 2 })
                {
                    Add(power + delta);
                    Add(-power + delta);
                }
            }
            foreach (var root in wide ? new[] { 0L, 1, 2, 3, 7, 127, 65535, 65536, 94906265, 3037000499 } : new[] { 0L, 1, 2, 3, 7, 15, 127, 255, 256, 4095, 46340 })
            {
                var square = root * root;
                foreach (var delta in new[] { -1, 0, 1 }) { Add(square + delta); Add(-square + delta); }
            }
            var random = new Random(75131 + (wide ? 64 : 32));
            var bytes = new byte[wide ? 8 : 4];
            for (var i = 0; i < 10000; i++)
            {
                random.NextBytes(bytes);
                Add(wide ? BitConverter.ToInt64(bytes, 0) : BitConverter.ToInt32(bytes, 0));
            }
            return values;
        }

        private sealed class RealRootCore
        {
            public readonly string Name;
            public readonly bool Modern;
            private readonly Dictionary<Type, MethodInfo> methods;
            public RealRootCore(Assembly assembly, string target)
            {
                Name = target;
                Modern = target == "net10.0";
                var fun = assembly.GetType("Aardvark.Base.Fun", true);
                methods = new[] { typeof(sbyte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double) }
                    .ToDictionary(type => type, type => fun.GetMethod("Csqrt", new[] { type }));
            }
            public (double Real, double Imag) Evaluate(Type scalar, object value)
            {
                var method = methods[scalar];
                var result = method.Invoke(null, new[] { value });
                return (Convert.ToDouble(method.ReturnType.GetField("Real").GetValue(result)),
                    Convert.ToDouble(method.ReturnType.GetField("Imag").GetValue(result)));
            }
        }

        private static IEnumerable<RealRootCore> RealRootCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(ComplexD).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0",
                    assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new RealRootCore(assembly, target);
            }
        }

        [Test]
        public static void Root()
            => GenericTest(rnd =>
            {
                GetRandomComplex(rnd, out Num.Complex _, out ComplexD c, false);
                int order = 2 + rnd.UniformInt(14);

                var roots = Fun.Root(c, order);

                foreach (var r in roots)
                {
                    var result = r;
                    for (int i = 1; i < order; i++)
                        result *= r;

                    AreEqual(c, result);
                }
            });

        #endregion

        #region Extreme finite range

        [Test]
        public static void ExtremeMagnitudeDouble()
        {
            var values = new[]
            {
                new ComplexD(1e308, 1e308),
                new ComplexD(double.MaxValue, 1.0),
                new ComplexD(1e-308, -1e-308),
                new ComplexD(double.Epsilon, double.Epsilon),
                new ComplexD(-3e200, 4e200),
                new ComplexD(3e-250, -4e-250),
            };

            foreach (var value in values)
            {
                double expected = ScaledNorm(value.Real, value.Imag);
                AssertClose(expected, value.Norm, absoluteTolerance: double.Epsilon);
            }

            Assert.That(new ComplexD(1e308, 1e308).NormSquared, Is.EqualTo(double.PositiveInfinity));
            Assert.That(new ComplexD(1e-308, 1e-308).NormSquared, Is.EqualTo(0.0));
        }

        [Test]
        public static void ExtremeMagnitudeFloat()
        {
            var values = new[]
            {
                new ComplexF(2e38f, 2e38f),
                new ComplexF(float.MaxValue, 1.0f),
                new ComplexF(1e-38f, -1e-38f),
                new ComplexF(float.Epsilon, float.Epsilon),
                new ComplexF(-3e20f, 4e20f),
                new ComplexF(3e-30f, -4e-30f),
            };

            foreach (var value in values)
            {
                float expected = ScaledNorm(value.Real, value.Imag);
                AssertClose(expected, value.Norm, absoluteTolerance: float.Epsilon);
            }

            Assert.That(new ComplexF(2e38f, 2e38f).NormSquared, Is.EqualTo(float.PositiveInfinity));
            Assert.That(new ComplexF(1e-38f, 1e-38f).NormSquared, Is.EqualTo(0.0f));
        }

        [Test]
        public static void ExtremeReciprocalAndDivisionDouble()
        {
            var large = new ComplexD(1e308, -1e308);
            var tiny = new ComplexD(1e-308, -1e-308);
            AssertClose(new ComplexD(5e-309, 5e-309), large.Reciprocal, absoluteTolerance: double.Epsilon);
            AssertClose(new ComplexD(5e307, 5e307), tiny.Reciprocal);
            AssertClose(ComplexD.One, large * large.Reciprocal);
            AssertClose(ComplexD.One, tiny * tiny.Reciprocal);

            var identities = new[]
            {
                large,
                tiny,
                new ComplexD(double.Epsilon, -double.Epsilon),
                new ComplexD(double.MaxValue, 1e292),
                new ComplexD(1e-200, -3e-210),
            };
            foreach (var value in identities)
                AssertClose(ComplexD.One, value / value, absoluteTolerance: double.Epsilon);

            AssertClose(new ComplexD(0.0, -1.0), new ComplexD(1e308, -1e308) / new ComplexD(1e308, 1e308));
            AssertClose(new ComplexD(0.25, 0.05), new ComplexD(3e307, -2e307) / new ComplexD(1e308, -1e308));
            AssertClose(new ComplexD(1e150, 1e-50), new ComplexD(1.0, 1e-200) / new ComplexD(1e-150, 0.0));
            AssertClose(new ComplexD(1e-50, -2e-50), new ComplexD(1e-200, -2e-200) / new ComplexD(1e-150, 0.0));
            AssertClose(new ComplexD(0.1, 0.1), 2e307 / new ComplexD(1e308, -1e308));
            AssertClose(new ComplexD(1e-50, -1e-50), 2e-200 / new ComplexD(1e-150, 1e-150));
        }

        [Test]
        public static void ExtremeReciprocalAndDivisionFloat()
        {
            var large = new ComplexF(2e38f, -2e38f);
            var tiny = new ComplexF(1e-38f, -1e-38f);
            AssertClose(new ComplexF(2.5e-39f, 2.5e-39f), large.Reciprocal, absoluteTolerance: float.Epsilon);
            AssertClose(new ComplexF(5e37f, 5e37f), tiny.Reciprocal);
            AssertClose(ComplexF.One, large * large.Reciprocal);
            AssertClose(ComplexF.One, tiny * tiny.Reciprocal);

            var identities = new[]
            {
                large,
                tiny,
                new ComplexF(float.Epsilon, -float.Epsilon),
                new ComplexF(float.MaxValue, 1e31f),
                new ComplexF(1e-25f, -3e-30f),
            };
            foreach (var value in identities)
                AssertClose(ComplexF.One, value / value, absoluteTolerance: float.Epsilon);

            AssertClose(new ComplexF(0.0f, -1.0f), new ComplexF(2e38f, -2e38f) / new ComplexF(2e38f, 2e38f));
            AssertClose(new ComplexF(0.25f, 0.05f), new ComplexF(6e37f, -4e37f) / new ComplexF(2e38f, -2e38f));
            AssertClose(new ComplexF(1e25f, 1e-5f), new ComplexF(1.0f, 1e-30f) / new ComplexF(1e-25f, 0.0f));
            AssertClose(new ComplexF(1e-5f, -2e-5f), new ComplexF(1e-30f, -2e-30f) / new ComplexF(1e-25f, 0.0f));
            AssertClose(new ComplexF(0.1f, 0.1f), 4e37f / new ComplexF(2e38f, -2e38f));
            AssertClose(new ComplexF(1e-5f, -1e-5f), 2e-30f / new ComplexF(1e-25f, 1e-25f));
        }

        [Test]
        public static void ExtremeSquareRootDouble()
        {
            double equalReal = Math.Sqrt(1e308) * Math.Sqrt((Math.Sqrt(2.0) + 1.0) * 0.5);
            double equalImag = Math.Sqrt(1e308) * Math.Sqrt((Math.Sqrt(2.0) - 1.0) * 0.5);
            AssertClose(new ComplexD(equalReal, equalImag), new ComplexD(1e308, 1e308).Sqrt());
            AssertClose(new ComplexD(equalReal, -equalImag), new ComplexD(1e308, -1e308).Sqrt());

            double tinyReal = Math.Sqrt(1e-308) * Math.Sqrt((Math.Sqrt(2.0) + 1.0) * 0.5);
            double tinyImag = Math.Sqrt(1e-308) * Math.Sqrt((Math.Sqrt(2.0) - 1.0) * 0.5);
            AssertClose(new ComplexD(tinyReal, tinyImag), new ComplexD(1e-308, 1e-308).Sqrt());

            double subReal = Math.Sqrt(double.Epsilon) * Math.Sqrt((Math.Sqrt(2.0) + 1.0) * 0.5);
            double subImag = Math.Sqrt(double.Epsilon) * Math.Sqrt((Math.Sqrt(2.0) - 1.0) * 0.5);
            AssertClose(new ComplexD(subReal, subImag), new ComplexD(double.Epsilon, double.Epsilon).Sqrt());
            AssertClose(new ComplexD(5e-301, 1.0), new ComplexD(-1.0, 1e-300).Sqrt());

            var values = new[]
            {
                new ComplexD(1e308, 1e308),
                new ComplexD(1e-308, -1e-308),
                new ComplexD(double.Epsilon, double.Epsilon),
                new ComplexD(-1.0, 1e-300),
            };
            foreach (var value in values)
            {
                ComplexD root = value.Sqrt();
                AssertClose(value, root * root, relativeTolerance: 8e-15, absoluteTolerance: 2 * double.Epsilon);
                AssertClose(root.Conjugated, value.Conjugated.Sqrt());
            }
        }

        [Test]
        public static void ExtremeSquareRootFloat()
        {
            float equalReal = MathF.Sqrt(2e38f) * MathF.Sqrt((MathF.Sqrt(2.0f) + 1.0f) * 0.5f);
            float equalImag = MathF.Sqrt(2e38f) * MathF.Sqrt((MathF.Sqrt(2.0f) - 1.0f) * 0.5f);
            AssertClose(new ComplexF(equalReal, equalImag), new ComplexF(2e38f, 2e38f).Sqrt());
            AssertClose(new ComplexF(equalReal, -equalImag), new ComplexF(2e38f, -2e38f).Sqrt());

            float tinyReal = MathF.Sqrt(1e-38f) * MathF.Sqrt((MathF.Sqrt(2.0f) + 1.0f) * 0.5f);
            float tinyImag = MathF.Sqrt(1e-38f) * MathF.Sqrt((MathF.Sqrt(2.0f) - 1.0f) * 0.5f);
            AssertClose(new ComplexF(tinyReal, tinyImag), new ComplexF(1e-38f, 1e-38f).Sqrt());

            float subReal = MathF.Sqrt(float.Epsilon) * MathF.Sqrt((MathF.Sqrt(2.0f) + 1.0f) * 0.5f);
            float subImag = MathF.Sqrt(float.Epsilon) * MathF.Sqrt((MathF.Sqrt(2.0f) - 1.0f) * 0.5f);
            AssertClose(new ComplexF(subReal, subImag), new ComplexF(float.Epsilon, float.Epsilon).Sqrt());
            AssertClose(new ComplexF(5e-31f, 1.0f), new ComplexF(-1.0f, 1e-30f).Sqrt());

            var values = new[]
            {
                new ComplexF(2e38f, 2e38f),
                new ComplexF(1e-38f, -1e-38f),
                new ComplexF(float.Epsilon, float.Epsilon),
                new ComplexF(-1.0f, 1e-30f),
            };
            foreach (var value in values)
            {
                ComplexF root = value.Sqrt();
                AssertClose(value, root * root, relativeTolerance: 8e-6f, absoluteTolerance: 2 * float.Epsilon);
                AssertClose(root.Conjugated, value.Conjugated.Sqrt());
            }
        }

        [Test]
        public static void SpecialValuesRemainCompatibleDouble()
        {
            Assert.That(new ComplexD(double.PositiveInfinity, double.NaN).Norm, Is.NaN);

            var zeroReciprocal = ComplexD.Zero.Reciprocal;
            Assert.That(zeroReciprocal.Real, Is.NaN);
            Assert.That(zeroReciprocal.Imag, Is.NaN);
            var zeroDivision = ComplexD.Zero / ComplexD.Zero;
            Assert.That(zeroDivision.Real, Is.NaN);
            Assert.That(zeroDivision.Imag, Is.NaN);

            var negativeCut = new ComplexD(-4.0, -0.0).Sqrt();
            Assert.That(BitConverter.DoubleToInt64Bits(negativeCut.Real), Is.EqualTo(0L));
            Assert.That(negativeCut.Imag, Is.EqualTo(2.0));
            Assert.That(BitConverter.DoubleToInt64Bits(new ComplexD(-0.0, 0.0).Sqrt().Real), Is.EqualTo(long.MinValue));
            Assert.That(BitConverter.DoubleToInt64Bits(new ComplexD(-4.0, -0.0).Reciprocal.Imag), Is.EqualTo(0L));
            Assert.That(BitConverter.DoubleToInt64Bits(new ComplexD(-4.0, 0.0).Reciprocal.Imag), Is.EqualTo(long.MinValue));

            var realInfinityRoot = new ComplexD(double.PositiveInfinity, 0.0).Sqrt();
            Assert.That(realInfinityRoot.Real, Is.EqualTo(double.PositiveInfinity));
            Assert.That(realInfinityRoot.Imag, Is.EqualTo(0.0));
            var imaginaryInfinityRoot = new ComplexD(0.0, double.PositiveInfinity).Sqrt();
            Assert.That(imaginaryInfinityRoot.Real, Is.NaN);
            Assert.That(imaginaryInfinityRoot.Imag, Is.NaN);
            var nanRoot = new ComplexD(double.NaN, 0.0).Sqrt();
            Assert.That(nanRoot.Real, Is.NaN);
            Assert.That(nanRoot.Imag, Is.EqualTo(0.0));
        }

        [Test]
        public static void SpecialValuesRemainCompatibleFloat()
        {
            Assert.That(new ComplexF(float.PositiveInfinity, float.NaN).Norm, Is.NaN);

            var zeroReciprocal = ComplexF.Zero.Reciprocal;
            Assert.That(zeroReciprocal.Real, Is.NaN);
            Assert.That(zeroReciprocal.Imag, Is.NaN);
            var zeroDivision = ComplexF.Zero / ComplexF.Zero;
            Assert.That(zeroDivision.Real, Is.NaN);
            Assert.That(zeroDivision.Imag, Is.NaN);

            var negativeCut = new ComplexF(-4.0f, -0.0f).Sqrt();
            Assert.That(BitConverter.SingleToInt32Bits(negativeCut.Real), Is.EqualTo(0));
            Assert.That(negativeCut.Imag, Is.EqualTo(2.0f));
            Assert.That(BitConverter.SingleToInt32Bits(new ComplexF(-0.0f, 0.0f).Sqrt().Real), Is.EqualTo(int.MinValue));
            Assert.That(BitConverter.SingleToInt32Bits(new ComplexF(-4.0f, -0.0f).Reciprocal.Imag), Is.EqualTo(0));
            Assert.That(BitConverter.SingleToInt32Bits(new ComplexF(-4.0f, 0.0f).Reciprocal.Imag), Is.EqualTo(int.MinValue));

            var realInfinityRoot = new ComplexF(float.PositiveInfinity, 0.0f).Sqrt();
            Assert.That(realInfinityRoot.Real, Is.EqualTo(float.PositiveInfinity));
            Assert.That(realInfinityRoot.Imag, Is.EqualTo(0.0f));
            var imaginaryInfinityRoot = new ComplexF(0.0f, float.PositiveInfinity).Sqrt();
            Assert.That(imaginaryInfinityRoot.Real, Is.NaN);
            Assert.That(imaginaryInfinityRoot.Imag, Is.NaN);
            var nanRoot = new ComplexF(float.NaN, 0.0f).Sqrt();
            Assert.That(nanRoot.Real, Is.NaN);
            Assert.That(nanRoot.Imag, Is.EqualTo(0.0f));
        }

        #endregion
    }
}

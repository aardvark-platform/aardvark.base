using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private sealed class PowerApi
        {
            public readonly bool Single;
            public readonly string Name;
            public readonly double Tiny;
            private readonly System.Reflection.ConstructorInfo constructor;
            private readonly System.Reflection.MethodInfo cc, cr, rc, forwardCC, forwardCR, forwardRC, radial, exp, log, pow;
            private readonly System.Reflection.PropertyInfo norm, argument, zero;
            private readonly System.Reflection.FieldInfo real, imag;
            public PowerApi(string target, bool single)
            {
                Single = single; Name = target + (single ? "/float" : "/double");
                string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(System.IO.File.Exists(path), Is.True, path);
                var assembly = System.Reflection.Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(System.Reflection.CustomAttributeExtensions.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>(assembly).FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                var scalar = single ? typeof(float) : typeof(double);
                var type = assembly.GetType("Aardvark.Base.Complex" + (single ? "F" : "D"), true);
                var fun = assembly.GetType("Aardvark.Base.Fun", true);
                constructor = type.GetConstructor(new[] { scalar, scalar });
                cc = fun.GetMethod("Pow", new[] { type, type }); cr = fun.GetMethod("Pow", new[] { type, scalar }); rc = fun.GetMethod("Pow", new[] { scalar, type });
                forwardCC = fun.GetMethod("Power", new[] { type, type }); forwardCR = fun.GetMethod("Power", new[] { type, scalar }); forwardRC = fun.GetMethod("Power", new[] { scalar, type });
                radial = type.GetMethod("CreateRadial", new[] { scalar, scalar });
                exp = fun.GetMethod("Exp", new[] { scalar }); log = fun.GetMethod("Log", new[] { scalar }); pow = fun.GetMethod("Pow", new[] { scalar, scalar });
                foreach (var method in new[] { cc, cr, rc, forwardCC, forwardCR, forwardRC, radial, exp, log, pow }) Assert.That(method.DeclaringType.Assembly, Is.SameAs(assembly), Name);
                foreach (var method in new[] { cc, cr, rc, forwardCC, forwardCR, forwardRC, radial }) Assert.That(method.ReturnType, Is.EqualTo(type), Name);
                norm = type.GetProperty("Norm"); argument = type.GetProperty("Argument"); zero = type.GetProperty("IsZero");
                real = type.GetField("Real"); imag = type.GetField("Imag");
                Tiny = Convert.ToDouble(assembly.GetType("Aardvark.Base.Constant`1", true).MakeGenericType(scalar).GetField("PositiveTinyValue").GetValue(null));
            }
            public double Round(double x) => Single ? (float)x : x;
            public Num.Complex Round(Num.Complex x) => new Num.Complex(Round(x.Real), Round(x.Imaginary));
            private object Number(double x) => Single ? (object)(float)x : x;
            private object Value(Num.Complex x) => constructor.Invoke(new[] { Number(x.Real), Number(x.Imaginary) });
            private Num.Complex Read(object x) => new Num.Complex(Convert.ToDouble(real.GetValue(x)), Convert.ToDouble(imag.GetValue(x)));
            public bool IsZero(Num.Complex x) => (bool)zero.GetValue(Value(x));
            public Num.Complex Call(Num.Complex x, Num.Complex y, int form, bool forward = false)
                => Read((form == 0 ? (forward ? forwardCC : cc) : form == 1 ? (forward ? forwardCR : cr) : (forward ? forwardRC : rc))
                    .Invoke(null, form == 0 ? new[] { Value(x), Value(y) } : form == 1 ? new[] { Value(x), Number(y.Real) } : new[] { Number(x.Real), Value(y) }));
            public Num.Complex Legacy(Num.Complex x, Num.Complex y, int form)
            {
                // Literal old shortcuts/formulas, with the actual target's unchanged scalar kernels.
                if (form == 2 ? x.Real == 0 : IsZero(x)) return Num.Complex.Zero;
                if (form == 0 && IsZero(y)) return Num.Complex.One;
                double Apply(System.Reflection.MethodInfo m, double a) => Convert.ToDouble(m.Invoke(null, new[] { Number(a) }));
                double r = form == 2 ? Math.Abs(x.Real) : Convert.ToDouble(norm.GetValue(Value(x)));
                double phi = form == 2 ? (x.Real < 0 ? Round(Math.PI) : 0) : Convert.ToDouble(argument.GetValue(Value(x)));
                double magnitude, phase;
                if (form == 1 || (form == 2 && !(x.Real < 0)))
                {
                    magnitude = Convert.ToDouble(pow.Invoke(null, new[] { Number(r), Number(y.Real) }));
                    phase = Round((form == 1 ? y.Real : y.Imaginary) * (form == 1 ? phi : Apply(log, r)));
                }
                else
                {
                    double l = Apply(log, r);
                    magnitude = Apply(exp, Round(Round(l * y.Real) - Round(y.Imaginary * phi)));
                    phase = Round(Round(y.Real * phi) + Round(y.Imaginary * l));
                }
                return Read(radial.Invoke(null, new[] { Number(magnitude), Number(phase) }));
            }
        }
        private static PowerApi[] PowerImplementations() => new[] { new PowerApi("net10.0", false), new PowerApi("netstandard2.0", false), new PowerApi("net10.0", true), new PowerApi("netstandard2.0", true) };
        private static IEnumerable<double> SmallPowerValues(PowerApi api)
        {
            foreach (int power in api.Single ? new[] { -140, -80, -35, -25 } : new[] { -900, -500, -100, -55 }) yield return Math.Pow(2, power);
            yield return api.Single ? float.Epsilon : double.Epsilon;
            for (int i = -3; i <= 3; i++)
            {
                double t = api.Tiny;
                for (int j = 0; j < Math.Abs(i); j++) t = api.Single ? (i < 0 ? MathF.BitDecrement((float)t) : MathF.BitIncrement((float)t)) : (i < 0 ? Math.BitDecrement(t) : Math.BitIncrement(t));
                yield return t;
            }
            yield return api.Tiny / 2; yield return api.Tiny * 2;
        }
        private static void PowerBits(Num.Complex actual, Num.Complex expected, string context)
        {
            void Component(double a, double e, string part)
            {
                if (double.IsNaN(e)) Assert.That(double.IsNaN(a), Is.True, context + part);
                else Assert.That(BitConverter.DoubleToInt64Bits(a), Is.EqualTo(BitConverter.DoubleToInt64Bits(e)), context + part);
            }
            Component(actual.Real, expected.Real, " real"); Component(actual.Imaginary, expected.Imaginary, " imaginary");
        }
        private static Num.Complex PowerRoot(Num.Complex x)
        {
            // System.Numerics.Sqrt can divide by zero for a minimum-subnormal imaginary input.
            if (x.Real == 0)
            {
                double component = Math.Sqrt(Math.Abs(x.Imaginary)) / Math.Sqrt(2);
                return new Num.Complex(component, Math.CopySign(component, x.Imaginary));
            }
            if (x.Imaginary == 0) return x.Real < 0 ? new Num.Complex(0, Math.Sqrt(-x.Real)) : new Num.Complex(Math.Sqrt(x.Real), 0);
            return Num.Complex.Sqrt(x);
        }
        private static void PowerOracle(PowerApi api, Num.Complex actual, Num.Complex expected, string context, bool strictReal = false)
        {
            expected = api.Round(expected);
            void Component(double a, double e, bool realPart)
            {
                string message = context + (realPart ? " real" : " imaginary");
                Assert.That(double.IsFinite(e), Is.True, message + " finite oracle");
                Assert.That(double.IsFinite(a), Is.True, message);
                if (e != 0) Assert.That(a != 0 && Math.Sign(a) == Math.Sign(e), Is.True, message + " nonzero component retained");
                double ulp = api.Single ? Math.Abs((double)MathF.BitIncrement((float)Math.Abs(e)) - Math.Abs(e)) : Math.Abs(Math.BitIncrement(Math.Abs(e)) - Math.Abs(e));
                double tolerance = strictReal && realPart ? ulp : Math.Max(2 * ulp, (api.Single ? 2e-4 : 4e-12) * Math.Abs(e));
                // Analytically zero axial components may contain polar sin/cos roundoff.
                if (e == 0) tolerance = 8 * (api.Single ? Math.Pow(2, -23) : Math.Pow(2, -52)) * Math.Max(Math.Abs(expected.Real), Math.Abs(expected.Imaginary));
                Assert.That(a, Is.EqualTo(e).Within(tolerance), message);
            }
            Component(actual.Real, expected.Real, true); Component(actual.Imaginary, expected.Imaginary, false);
        }

        [Test]
        public static void TinyNonzeroBasesKeepTheirPowers()
        {
            foreach (var api in PowerImplementations())
                foreach (double t in SmallPowerValues(api))
                    foreach (var input in new[] { new Num.Complex(t, 0), new Num.Complex(-t, 0), new Num.Complex(0, t), new Num.Complex(0, -t), new Num.Complex(t, 2 * t), new Num.Complex(-t, -2 * t) })
                    {
                        var x = api.Round(input);
                        PowerBits(api.Call(x, Num.Complex.Zero, 0), Num.Complex.One, api.Name + " exact zero exponent " + x);
                        if (t == (api.Single ? float.Epsilon : double.Epsilon) && x.Real != 0 && x.Imaginary != 0) continue; // unchanged rounded subnormal norm is not a full-range power repair
                        foreach (var y in new[] { Num.Complex.One, new Num.Complex(.5, 0), new Num.Complex(-1, 0), new Num.Complex(.5, .25) })
                        {
                            if (y.Real == -1 && t < (api.Single ? Math.Pow(2, -120) : Math.Pow(2, -1000))) continue; // reciprocal is outside this precision
                            var expected = y == Num.Complex.One ? x : y == new Num.Complex(.5, 0) ? PowerRoot(x) : y == new Num.Complex(-1, 0) ? Num.Complex.One / x : Num.Complex.Pow(x, y);
                            if (api.Single && t < Math.Pow(2, -126) && x.Real != 0 && x.Imaginary != 0 && y.Imaginary != 0)
                            {
                                // Preserve the formula's subnormal magnitude quantization, not a new full-range accuracy promise.
                                double r = api.Round(Num.Complex.Abs(x)), phi = Math.Atan2(x.Imaginary, x.Real);
                                expected = Num.Complex.FromPolarCoordinates(Math.Pow(r, y.Real) * Math.Exp(-y.Imaginary * phi), y.Real * phi + y.Imaginary * Math.Log(r));
                            }
                            foreach (int form in y.Imaginary == 0 ? new[] { 0, 1 } : new[] { 0 })
                            {
                                string context = $"{api.Name} base={x} exponent={y} form={form}";
                                var actual = api.Call(x, y, form); PowerOracle(api, actual, expected, context); PowerBits(api.Call(x, y, form, true), actual, context + " Power");
                            }
                        }
                    }
        }

        [Test]
        public static void TinyNonzeroExponentsKeepTheirImaginaryContribution()
        {
            foreach (var api in PowerImplementations())
                foreach (double t in SmallPowerValues(api))
                    foreach (var x in new[] { new Num.Complex(2, 0), new Num.Complex(.5, 0), new Num.Complex(1, 2), new Num.Complex(-2, 3) })
                        foreach (var y in new[] { new Num.Complex(0, t), new Num.Complex(0, -t), new Num.Complex(t, 0), new Num.Complex(t, -t) })
                        {
                            string context = $"{api.Name} base={x} exponent={y}";
                            var actual = api.Call(x, y, 0); PowerOracle(api, actual, Num.Complex.Pow(x, y), context, true); PowerBits(api.Call(x, y, 0, true), actual, context + " Power");
                        }
        }

        [Test]
        public static void ExactZerosAndOrdinaryOrExceptionalPowersKeepTheirContracts()
        {
            foreach (var api in PowerImplementations())
            {
                var values = new[] { Num.Complex.Zero, new Num.Complex(-0.0, 0), new Num.Complex(0, -0.0), new Num.Complex(-0.0, -0.0), Num.Complex.One, new Num.Complex(-2, 3), new Num.Complex(.25, -.5), new Num.Complex(double.NaN, 0), new Num.Complex(0, double.NaN), new Num.Complex(double.PositiveInfinity, 0), new Num.Complex(0, double.NegativeInfinity) };
                foreach (var x in values) foreach (var y in values) foreach (int form in new[] { 0, 1, 2 })
                {
                    string context = $"{api.Name} base={x} exponent={y} form={form}";
                    var actual = api.Call(x, y, form); PowerBits(actual, api.Legacy(x, y, form), context); PowerBits(api.Call(x, y, form, true), actual, context + " Power");
                }
                foreach (double t in SmallPowerValues(api)) foreach (var x in new[] { new Num.Complex(t, 0), new Num.Complex(0, -t), new Num.Complex(t, -t) })
                {
                    Assert.That(api.IsZero(x), Is.EqualTo(Math.Abs(x.Real) < api.Tiny && Math.Abs(x.Imaginary) < api.Tiny), api.Name + " unchanged IsZero " + x);
                    // The scalar-base path has no approximate shortcuts and stays literal even for tiny inputs.
                    PowerBits(api.Call(x, x, 2), api.Legacy(x, x, 2), api.Name + " scalar base " + x);
                }
            }
        }

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

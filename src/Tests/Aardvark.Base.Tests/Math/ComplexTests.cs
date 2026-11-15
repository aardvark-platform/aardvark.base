using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

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

        private sealed class AcoshApi
        {
            internal readonly string Name;
            internal readonly double Eps;
            private readonly System.Reflection.ConstructorInfo constructor;
            private readonly System.Reflection.FieldInfo real, imag;
            private readonly System.Reflection.MethodInfo acosh, cosh, sqrt, log, multiply, subtract, add;
            private readonly bool single;

            internal AcoshApi(System.Reflection.Assembly assembly, bool single, string target)
            {
                this.single = single;
                Name = target + (single ? "/ComplexF" : "/ComplexD");
                Eps = single ? Math.Pow(2, -23) : Math.Pow(2, -52);
                var type = assembly.GetType("Aardvark.Base.Complex" + (single ? "F" : "D"), true);
                var scalar = single ? typeof(float) : typeof(double);
                constructor = type.GetConstructor(new[] { scalar, scalar });
                real = type.GetField("Real");
                imag = type.GetField("Imag");
                var fun = assembly.GetType("Aardvark.Base.Fun", true);
                acosh = fun.GetMethod("Acosh", new[] { type });
                cosh = fun.GetMethod("Cosh", new[] { type });
                sqrt = fun.GetMethod("Sqrt", new[] { type });
                log = fun.GetMethod("Log", new[] { type });
                multiply = type.GetMethod("op_Multiply", new[] { type, type });
                subtract = type.GetMethod("op_Subtraction", new[] { type, scalar });
                add = type.GetMethod("op_Addition", new[] { type, type });
                Assert.That(constructor, Is.Not.Null, Name);
                Assert.That(real.FieldType, Is.EqualTo(scalar), Name);
                Assert.That(imag.FieldType, Is.EqualTo(scalar), Name);
                foreach (var method in new[] { acosh, cosh, sqrt, log, multiply, subtract, add })
                {
                    Assert.That(method, Is.Not.Null, Name);
                    Assert.That(method.ReturnType, Is.EqualTo(type), Name);
                    Assert.That(method.DeclaringType.Assembly, Is.SameAs(assembly), Name);
                }
            }

            private object Scalar(double x) => single ? (object)(float)x : x;
            private object Box(Num.Complex x) => constructor.Invoke(new[] { Scalar(x.Real), Scalar(x.Imaginary) });
            private Num.Complex Unbox(object x) => new Num.Complex(Convert.ToDouble(real.GetValue(x)), Convert.ToDouble(imag.GetValue(x)));
            internal Num.Complex Input(double a, double b) => Unbox(Box(new Num.Complex(a, b)));
            internal Num.Complex Apply(Num.Complex x) => Unbox(acosh.Invoke(null, new[] { Box(x) }));
            internal Num.Complex RoundTrip(Num.Complex x) => Unbox(cosh.Invoke(null, new[] { acosh.Invoke(null, new[] { Box(x) }) }));
            internal Num.Complex Legacy(Num.Complex x)
            {
                var z = Box(x);
                var square = multiply.Invoke(null, new[] { z, z });
                var root = sqrt.Invoke(null, new[] { subtract.Invoke(null, new[] { square, Scalar(1) }) });
                return Unbox(log.Invoke(null, new[] { add.Invoke(null, new[] { z, root }) }));
            }
        }

        private static IEnumerable<AcoshApi> AcoshImplementations()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(
                    TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(System.IO.File.Exists(path), Is.True, "Required actual core: " + path);
                var assembly = System.Reflection.Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                var framework = assembly.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>();
                Assert.That(framework.FrameworkName, Is.EqualTo(target == "net10.0"
                    ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                yield return new AcoshApi(assembly, false, target);
                yield return new AcoshApi(assembly, true, target);
            }
        }

        private static void AssertAcoshPrincipal(AcoshApi api, Num.Complex input, Num.Complex expected, double tolerance)
        {
            var actual = api.Apply(input);
            string context = $"{api.Name}: Acosh({input}) = {actual}, expected {expected}";
            Assert.That(actual.Real, Is.EqualTo(expected.Real).Within(tolerance), context);
            Assert.That(actual.Imaginary, Is.EqualTo(expected.Imaginary).Within(tolerance), context);
            Assert.That(actual.Real, Is.GreaterThanOrEqualTo(-tolerance), context);
            Assert.That(Math.Abs(actual.Imaginary), Is.LessThanOrEqualTo(Math.PI + tolerance), context);
            var recovered = api.RoundTrip(input);
            double inverseTolerance = 64 * api.Eps * Math.Max(1, input.Magnitude);
            Assert.That(recovered.Real, Is.EqualTo(input.Real).Within(inverseTolerance), context + " inverse real");
            Assert.That(recovered.Imaginary, Is.EqualTo(input.Imaginary).Within(inverseTolerance), context + " inverse imaginary");
            if (input.Imaginary != 0)
            {
                var conjugate = api.Apply(Num.Complex.Conjugate(input));
                Assert.That(conjugate.Real, Is.EqualTo(actual.Real), context + " conjugate real");
                Assert.That(conjugate.Imaginary, Is.EqualTo(-actual.Imaginary), context + " conjugate imaginary");
            }
        }

        [Test]
        public static void AcoshSelectsPrincipalValuesOnAxesAndCuts()
        {
            foreach (var api in AcoshImplementations())
            {
                // Include both immediate representable neighbors of both branch points.
                foreach (double a in new[] { -16.0, -4, -2, -1 - api.Eps, -1, -1 + api.Eps / 2,
                                             -0.75, -0.25, -0.0, 0.0, 0.25, 0.75,
                                             1 - api.Eps / 2, 1, 1 + api.Eps, 2, 4, 16 })
                foreach (double zero in new[] { 0.0, -0.0 })
                {
                    var input = api.Input(a, zero);
                    double magnitude = Math.Abs(input.Real);
                    var expected = magnitude <= 1
                        ? new Num.Complex(0, Math.Acos(input.Real))
                        : new Num.Complex(Math.Log(magnitude + Math.Sqrt((magnitude - 1) * (magnitude + 1))), a < 0 ? Math.PI : 0);
                    AssertAcoshPrincipal(api, input, expected, 16 * api.Eps * Math.Max(1, expected.Magnitude));
                }
                foreach (double b in new[] { -16.0, -4, -1, -0.25, 0.25, 1, 4, 16 })
                foreach (double zero in new[] { 0.0, -0.0 })
                {
                    var input = api.Input(zero, b);
                    var expected = new Num.Complex(Math.Log(Math.Abs(b) + Math.Sqrt(b * b + 1)), Math.Sign(b) * Math.PI / 2);
                    AssertAcoshPrincipal(api, input, expected, 16 * api.Eps * Math.Max(1, expected.Magnitude));
                }
                double tiny = api.Eps > 1e-10 ? float.Epsilon : double.Epsilon;
                double normal = Math.Pow(2, api.Eps > 1e-10 ? -126 : -1022);
                // A lost cross-product sign must not flip an already-correct upper-axis value.
                foreach (double a in new[] { -tiny, tiny, -normal, normal })
                foreach (double b in new[] { -0.25, -0.125, 0.125, 0.25 })
                {
                    var expected = new Num.Complex(Math.Log(Math.Abs(b) + Math.Sqrt(b * b + 1)), Math.Sign(b) * Math.PI / 2);
                    AssertAcoshPrincipal(api, api.Input(a, b), expected, 16 * api.Eps * Math.Max(1, expected.Magnitude));
                }
                foreach (double a in new[] { -4.0, -2, 2, 4 })
                foreach (double b in new[] { -tiny, tiny, -normal, normal })
                {
                    double magnitude = Math.Abs(a);
                    var expected = new Num.Complex(Math.Log(magnitude + Math.Sqrt((magnitude - 1) * (magnitude + 1))), a < 0 ? Math.Sign(b) * Math.PI : 0);
                    AssertAcoshPrincipal(api, api.Input(a, b), expected, 16 * api.Eps * Math.Max(1, expected.Magnitude));
                }
            }
        }

        [Test]
        public static void AcoshSelectsPrincipalValuesAcrossQuadrantsAndBranchPoints()
        {
            foreach (var api in AcoshImplementations())
            foreach (double u in new[] { 0.03125, 0.0625, 0.125, 0.5, 1.0, 2.0, 3.0 })
            foreach (double v in new[] { -Math.PI + 0.03125, -3.0, -2.5, -2.0, -Math.PI / 2,
                                         -1.0, -0.125, -0.03125, 0.03125, 0.125, 1.0,
                                         Math.PI / 2, 2.0, 2.5, 3.0, Math.PI - 0.03125 })
            {
                // Invert elliptic coordinates analytically, independently of complex Sqrt/Log.
                var input = api.Input(Math.Cosh(u) * Math.Cos(v), Math.Sinh(u) * Math.Sin(v));
                var expected = new Num.Complex(u, v);
                double derivative = 1 / Math.Sqrt((input - Num.Complex.One).Magnitude * (input + Num.Complex.One).Magnitude);
                double tolerance = 16 * api.Eps * (Math.Max(1, expected.Magnitude) + Math.Max(1, input.Magnitude) * derivative);
                AssertAcoshPrincipal(api, input, expected, tolerance);
            }
        }

        [Test]
        public static void AcoshKeepsRightHalfPlaneAndExceptionalContracts()
        {
            foreach (var api in AcoshImplementations())
            {
                void Check(Num.Complex input)
                {
                    var actual = api.Apply(input);
                    var expected = api.Legacy(input);
                    foreach (var pair in new[] { (expected.Real, actual.Real), (expected.Imaginary, actual.Imaginary) })
                        Assert.That(double.IsNaN(pair.Item1) ? double.IsNaN(pair.Item2)
                            : BitConverter.DoubleToInt64Bits(pair.Item2) == BitConverter.DoubleToInt64Bits(pair.Item1),
                            Is.True, $"{api.Name}: compatibility at {input}, expected {expected}, actual {actual}");
                }
                double tiny = api.Eps > 1e-10 ? float.Epsilon : double.Epsilon;
                foreach (double a in new[] { -tiny, tiny })
                foreach (double b in new[] { 0.125, 0.25 }) Check(api.Input(a, b));
                var random = new Random(197153);
                for (int i = 0; i < 4096; i++)
                    Check(api.Input(0.125 + random.NextDouble() * 16, (random.NextDouble() - 0.5) * 32));
                foreach (double a in new[] { -0.0, 0.0, -1.0, -0.75, 0.75, 1.0, 2.0 })
                foreach (double b in new[] { 0.0, -0.0 }) Check(api.Input(a, b));
                foreach (double a in new[] { double.NegativeInfinity, -2, -0.0, 0.0, 2, double.PositiveInfinity, double.NaN })
                foreach (double b in new[] { double.NegativeInfinity, -2, -0.0, 0.0, 2, double.PositiveInfinity, double.NaN })
                    if (!double.IsFinite(a) || !double.IsFinite(b)) Check(api.Input(a, b));
            }
        }

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

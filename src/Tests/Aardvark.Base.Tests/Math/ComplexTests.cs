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
    }
}

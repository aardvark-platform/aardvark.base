using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        [Test]
        public static void TinyComplexTrigonometryRetainsEachComponent()
        {
            foreach (var core in TrigCores())
            foreach (var exponent in core.Single ? new[] { -149, -148, -126, -80, -40, -30, -24, -20, -12 }
                                                : new[] { -1074, -1073, -1022, -600, -300, -100, -60, -54, -53, -40, -20 })
            foreach (var sign in new[] { -1, 1 })
            {
                double t = sign * Math.ScaleB(1, exponent);
                foreach (var pair in new[] { (0.0, t), (t, 0.0), (t, t), (t, -t), (1.0, t), (-1.0, t), (t, .5), (.5, t) })
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh", "Tan", "Tanh" })
                {
                    if ((operation == "Tan" || operation == "Tanh") && Math.Max(Math.Abs(pair.Item1), Math.Abs(pair.Item2)) > .001) continue;
                    core.Reference(operation, pair.Item1, pair.Item2, "tiny exponent=" + exponent);
                    if (pair.Item1 == 0 || pair.Item2 == 0) core.Axis(operation, pair.Item1, pair.Item2);
                }
            }
        }

        [Test]
        public static void ComplexTrigonometryMatchesAxesAndOrdinaryReferences()
        {
            foreach (var core in TrigCores())
            {
                foreach (var value in new[] { -2.0, -1.0, -.5, -.125, -0.0, 0.0, .125, .5, 1.0, 2.0 })
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh" })
                { core.Axis(operation, value, 0); core.Axis(operation, 0, value); }
                var random = new Random(183137);
                for (int i = 0; i < 2048; i++)
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh" })
                {
                    double a = (random.NextDouble() - .5) * 4, b = (random.NextDouble() - .5) * 4;
                    core.Reference(operation, a, b, "ordinary seed=183137 case=" + i);
                }
                for (int i = 0; i < 1024; i++)
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh" })
                    core.Reference(operation, .25 + (i % 29) * .125, -1.75 + (i % 31) * .1, "existing benchmark input=" + i);
            }
        }

        [Test]
        public static void ComplexTrigonometryKeepsClassificationsAndProductZeroSigns()
        {
            foreach (var core in TrigCores())
            {
                var values = new[] { -0.0, 0.0, -1.0, 1.0, -.5, .5, -Math.PI / 2, Math.PI / 2,
                    double.NaN, double.NegativeInfinity, double.PositiveInfinity,
                    core.Single ? -float.MaxValue : -double.MaxValue, core.Single ? float.MaxValue : double.MaxValue,
                    core.Single ? -float.Epsilon : -double.Epsilon, core.Single ? float.Epsilon : double.Epsilon };
                foreach (var a in values)
                foreach (var b in values)
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh", "Tan", "Tanh" })
                    if (!double.IsFinite(a) || !double.IsFinite(b)) core.Classifications(operation, a, b);
                double boundary = core.Single ? (double)(float)Math.Log(float.MaxValue) : Math.Log(double.MaxValue);
                var adjacent = new[] { core.Next(boundary, false), boundary, core.Next(boundary, true),
                                      core.Single ? 89.0 : 710.0, core.Single ? 90.0 : 711.0 };
                foreach (var a in new[] { -1.0, -0.0, 0.0, 1.0 })
                foreach (var b in adjacent)
                foreach (var sign in new[] { -1, 1 })
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh", "Tan", "Tanh" })
                { core.Classifications(operation, a, sign * b); core.Classifications(operation, sign * b, a); }
                foreach (var a in new[] { -0.0, 0.0, -1.0, 1.0 })
                foreach (var b in new[] { -0.0, 0.0, -1.0, 1.0 })
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh" })
                    if (a == 0 || b == 0) core.ZeroSigns(operation, a, b);
            }
        }

        private sealed class TrigCore
        {
            private readonly Type scalar, complex;
            private readonly Type fun;
            private readonly ConstructorInfo constructor;
            private readonly FieldInfo real, imag;
            private readonly Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();
            private readonly Dictionary<string, MethodInfo> forwarders = new Dictionary<string, MethodInfo>();
            private readonly MethodInfo add, subtract, multiply, divide;
            public bool Single { get; }
            private readonly string name;
            public TrigCore(Assembly assembly, bool single, string target)
            {
                Single = single; scalar = single ? typeof(float) : typeof(double); name = target + "/" + scalar.Name;
                complex = assembly.GetType(single ? "Aardvark.Base.ComplexF" : "Aardvark.Base.ComplexD", true);
                fun = assembly.GetType("Aardvark.Base.Fun", true); Assert.AreSame(assembly, complex.Assembly, name); Assert.AreSame(assembly, fun.Assembly, name);
                constructor = complex.GetConstructor(new[] { scalar, scalar }); Assert.IsNotNull(constructor, name); Assert.AreSame(assembly, constructor.DeclaringType.Assembly, name);
                real = complex.GetField("Real"); imag = complex.GetField("Imag"); Assert.AreEqual(scalar, real.FieldType, name); Assert.AreEqual(scalar, imag.FieldType, name);
                foreach (var operation in new[] { "Sin", "Cos", "Sinh", "Cosh", "Tan", "Tanh", "Exp" })
                {
                    var method = fun.GetMethod(operation, new[] { complex }); Assert.IsNotNull(method, name + operation); Assert.AreSame(assembly, method.DeclaringType.Assembly, name); Assert.AreEqual(complex, method.ReturnType, name); methods.Add(operation, method);
                    if (operation == "Exp") continue;
                    var forward = complex.GetMethod(operation, new[] { complex }); Assert.IsNotNull(forward, name + operation); Assert.AreSame(assembly, forward.DeclaringType.Assembly, name); Assert.AreEqual(complex, forward.ReturnType, name); forwarders.Add(operation, forward);
                }
                add = complex.GetMethod("op_Addition", new[] { complex, complex }); subtract = complex.GetMethod("op_Subtraction", new[] { complex, complex });
                multiply = complex.GetMethod("op_Multiply", new[] { complex, scalar }); divide = complex.GetMethod("op_Division", new[] { complex, complex });
                foreach (var method in new[] { add, subtract, multiply, divide }) { Assert.IsNotNull(method, name); Assert.AreSame(assembly, method.DeclaringType.Assembly, name); }
            }
            private object ScalarValue(double x) => Single ? (object)(float)x : x;
            private double Round(double x) => Single ? (float)x : x;
            private object Value(double a, double b) => constructor.Invoke(new[] { ScalarValue(a), ScalarValue(b) });
            private (double, double) Read(object z) => (Convert.ToDouble(real.GetValue(z)), Convert.ToDouble(imag.GetValue(z)));
            private double Scalar(string operation, double x)
            {
                var method = fun.GetMethod(operation, new[] { scalar }); Assert.AreSame(fun.Assembly, method.DeclaringType.Assembly, name);
                return Convert.ToDouble(method.Invoke(null, new[] { ScalarValue(x) }));
            }
            private static void Bits(double expected, double actual, string context)
            {
                if (double.IsNaN(expected)) Assert.IsTrue(double.IsNaN(actual), context + " NaN");
                else Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context + $", expected={expected:R}, actual={actual:R}");
            }
            private (double, double) Call(string operation, double a, double b, string context)
            {
                var value = Value(a, b); var result = Read(methods[operation].Invoke(null, new[] { value })); var forwarded = Read(forwarders[operation].Invoke(null, new[] { value }));
                Bits(result.Item1, forwarded.Item1, context + " static-forwarder real"); Bits(result.Item2, forwarded.Item2, context + " static-forwarder imag"); return result;
            }
            private void Component(double expected, double actual, string context, int ulps)
            {
                expected = Round(expected); Assert.IsTrue(double.IsFinite(expected) && double.IsFinite(actual), context + " finite reference/component");
                if (expected == 0) { Assert.AreEqual(0, actual, context + " exact zero"); return; }
                Assert.IsTrue(actual != 0 && Math.Sign(actual) == Math.Sign(expected), context + " nonzero component and sign");
                double step = Single ? Math.Abs((double)float.BitIncrement(Math.Abs((float)expected)) - Math.Abs(expected)) : Math.BitIncrement(Math.Abs(expected)) - Math.Abs(expected);
                Assert.That(Math.Abs(actual - expected), Is.LessThanOrEqualTo(ulps * step), context + $", expected={expected:R}, actual={actual:R}, ulps={ulps}");
            }
            public void Reference(string operation, double a, double b, string label)
            {
                a = Round(a); b = Round(b); var z = new Num.Complex(a, b);
                var expected = operation == "Sin" ? Num.Complex.Sin(z) : operation == "Cos" ? Num.Complex.Cos(z) : operation == "Sinh" ? Num.Complex.Sinh(z) : operation == "Cosh" ? Num.Complex.Cosh(z) : operation == "Tan" ? Num.Complex.Tan(z) : Num.Complex.Tanh(z);
                var context = $"{name}: {label}, {operation}({a:R},{b:R})"; var actual = Call(operation, a, b, context); int ulps = operation == "Tan" || operation == "Tanh" ? 32 : 16;
                Component(expected.Real, actual.Item1, context + " real", ulps); Component(expected.Imaginary, actual.Item2, context + " imag", ulps);
            }
            public void Axis(string operation, double a, double b)
            {
                a = Round(a); b = Round(b); var context = $"{name}: independent {operation} axis({a:R},{b:R})"; var actual = Call(operation, a, b, context);
                if (b == 0) { Component(Scalar(operation, a), actual.Item1, context + " scalar real", 32); Component(0, actual.Item2, context + " zero imag", 0); }
                else
                {
                    var scalarOperation = operation == "Sin" ? "Sinh" : operation == "Cos" ? "Cosh" : operation == "Sinh" ? "Sin" : operation == "Cosh" ? "Cos" : operation == "Tan" ? "Tanh" : "Tan";
                    double expected = Scalar(scalarOperation, b); bool imaginary = operation != "Cos" && operation != "Cosh";
                    Component(imaginary ? 0 : expected, actual.Item1, context + " real", 32); Component(imaginary ? expected : 0, actual.Item2, context + " imag", 32);
                }
            }
            private object Legacy(string operation, double a, double b)
            {
                a = Round(a); b = Round(b);
                if (operation == "Sinh") { var t = Read(Legacy("Sin", -b, a)); return Value(t.Item2, -t.Item1); }
                if (operation == "Cosh") return Legacy("Cos", -b, a);
                if (operation == "Tanh") { var t = Read(Legacy("Tan", -b, a)); return Value(t.Item2, -t.Item1); }
                if (operation == "Tan") return a == 0 && double.IsInfinity(b) ? Value(0, Math.Sign(b)) : divide.Invoke(null, new[] { Legacy("Sin", a, b), Legacy("Cos", a, b) });
                var e0 = methods["Exp"].Invoke(null, new[] { Value(-b, a) }); var e1 = methods["Exp"].Invoke(null, new[] { Value(b, -a) });
                var value = (operation == "Cos" ? add : subtract).Invoke(null, new[] { e0, e1 });
                if (operation == "Sin") { var t = Read(value); value = Value(t.Item2, -t.Item1); }
                return multiply.Invoke(null, new[] { value, ScalarValue(.5) });
            }
            private static int Classification(double x) => double.IsNaN(x) ? 2 : double.IsPositiveInfinity(x) ? 1 : double.IsNegativeInfinity(x) ? -1 : 0;
            public void Classifications(string operation, double a, double b)
            {
                var context = $"{name}: legacy classification {operation}({a:R},{b:R})"; var expected = Read(Legacy(operation, a, b)); var actual = Call(operation, a, b, context);
                Assert.AreEqual(Classification(expected.Item1), Classification(actual.Item1), context + " real"); Assert.AreEqual(Classification(expected.Item2), Classification(actual.Item2), context + " imag");
            }
            public void ZeroSigns(string operation, double a, double b)
            {
                var context = $"{name}: product zero signs {operation}({a:R},{b:R})"; var actual = Call(operation, a, b, context);
                double er, ei;
                if (operation == "Sin") { er = Round(Scalar("Sin", a) * Scalar("Cosh", b)); ei = Round(Scalar("Cos", a) * Scalar("Sinh", b)); }
                else if (operation == "Cos") { er = Round(Scalar("Cos", a) * Scalar("Cosh", b)); ei = Round(-Scalar("Sin", a) * Scalar("Sinh", b)); }
                else if (operation == "Sinh") { er = Round(Scalar("Cos", -b) * Scalar("Sinh", a)); ei = -Round(Scalar("Sin", -b) * Scalar("Cosh", a)); }
                else { er = Round(Scalar("Cos", -b) * Scalar("Cosh", a)); ei = Round(-Scalar("Sin", -b) * Scalar("Sinh", a)); }
                Bits(er, actual.Item1, context + " real"); Bits(ei, actual.Item2, context + " imag");
            }
            public double Next(double x, bool up) => Single ? up ? float.BitIncrement((float)x) : float.BitDecrement((float)x) : up ? Math.BitIncrement(x) : Math.BitDecrement(x);
        }
        private static IEnumerable<TrigCore> TrigCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location), "..", target, "Aardvark.Base.dll")); Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path); Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new TrigCore(assembly, true, target); yield return new TrigCore(assembly, false, target);
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

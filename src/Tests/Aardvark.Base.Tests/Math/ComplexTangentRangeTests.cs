using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using N = System.Numerics.Complex;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class ComplexTangentRangeTests
    {
        private sealed class Api
        {
            internal readonly bool Single;
            internal readonly string Name;
            internal double Gate => Single ? 88 : 709;
            internal double Maximum => Single ? float.MaxValue : double.MaxValue;
            private readonly ConstructorInfo constructor;
            private readonly FieldInfo real, imag;
            private readonly MethodInfo tan, tanh, forwardTan, forwardTanh, sin, cos, divide;

            internal Api(Assembly assembly, bool single, string target)
            {
                Single = single;
                Name = target + (single ? "/ComplexF" : "/ComplexD");
                var scalar = single ? typeof(float) : typeof(double);
                var type = assembly.GetType("Aardvark.Base.Complex" + (single ? "F" : "D"), true);
                var fun = assembly.GetType("Aardvark.Base.Fun", true);
                constructor = type.GetConstructor(new[] { scalar, scalar });
                real = type.GetField("Real"); imag = type.GetField("Imag");
                tan = fun.GetMethod("Tan", new[] { type }); tanh = fun.GetMethod("Tanh", new[] { type });
                forwardTan = type.GetMethod("Tan", new[] { type }); forwardTanh = type.GetMethod("Tanh", new[] { type });
                sin = fun.GetMethod("Sin", new[] { type }); cos = fun.GetMethod("Cos", new[] { type });
                divide = type.GetMethod("op_Division", new[] { type, type });
                Assert.That(constructor, Is.Not.Null, Name);
                Assert.That(real.FieldType, Is.EqualTo(scalar), Name);
                Assert.That(imag.FieldType, Is.EqualTo(scalar), Name);
                foreach (var method in new[] { tan, tanh, forwardTan, forwardTanh, sin, cos, divide })
                {
                    Assert.That(method, Is.Not.Null, Name);
                    Assert.That(method.ReturnType, Is.EqualTo(type), Name);
                    Assert.That(method.DeclaringType.Assembly, Is.SameAs(assembly), Name);
                }
            }
            private object Scalar(double x) => Single ? (object)(float)x : x;
            private object Box(N x) => constructor.Invoke(new[] { Scalar(x.Real), Scalar(x.Imaginary) });
            private N Unbox(object x) => new N(Convert.ToDouble(real.GetValue(x)), Convert.ToDouble(imag.GetValue(x)));
            internal N Input(double a, double b) => Unbox(Box(new N(a, b)));
            internal N Apply(N x, bool hyperbolic, bool forwarded)
                => Unbox((hyperbolic ? (forwarded ? forwardTanh : tanh) : (forwarded ? forwardTan : tan)).Invoke(null, new[] { Box(x) }));
            internal N Legacy(N x, bool hyperbolic)
            {
                if (hyperbolic)
                {
                    var t = Legacy(new N(-x.Imaginary, x.Real), false);
                    return new N(t.Imaginary, -t.Real);
                }
                if (x.Real == 0 && double.IsPositiveInfinity(x.Imaginary)) return new N(0, 1);
                if (x.Real == 0 && double.IsNegativeInfinity(x.Imaginary)) return new N(-0.0, -1);
                var value = Box(x);
                return Unbox(divide.Invoke(null, new[] { sin.Invoke(null, new[] { value }), cos.Invoke(null, new[] { value }) }));
            }
            internal double Neighbor(double x, int direction) => Single
                ? BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits((float)x) + direction)
                : BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(x) + direction);
        }

        private static IEnumerable<Api> Implementations()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required actual core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                yield return new Api(assembly, true, target);
                yield return new Api(assembly, false, target);
            }
        }
        private static void SameBits(double expected, double actual, string context)
        {
            Assert.That(double.IsNaN(expected) ? double.IsNaN(actual)
                : BitConverter.DoubleToInt64Bits(actual) == BitConverter.DoubleToInt64Bits(expected), Is.True, context);
        }
        private static void Unchanged(Api api, N tangentInput)
        {
            foreach (bool hyperbolic in new[] { false, true })
            {
                var input = hyperbolic ? api.Input(tangentInput.Imaginary, -tangentInput.Real) : tangentInput;
                var expected = api.Legacy(input, hyperbolic);
                foreach (bool forwarded in new[] { false, true })
                {
                    var actual = api.Apply(input, hyperbolic, forwarded);
                    string context = $"{api.Name}: input={input}, hyperbolic={hyperbolic}, forwarded={forwarded}, expected={expected}, actual={actual}";
                    SameBits(expected.Real, actual.Real, context + " real");
                    SameBits(expected.Imaginary, actual.Imaginary, context + " imaginary");
                }
            }
        }
        private static (double Phase, bool NegativeZero)[] Phases(Api api) => new[] {
            (0.0, false), (-0.0, true), (0.25, false), (-0.25, true), (1.0, false), (-1.0, true),
            (2.0, true), (-2.0, false), (api.Maximum, true), (-api.Maximum, false)
        };
        private static void Limiting(Api api, double magnitude, double phase, bool negativeZero)
        {
            // Known signs of sin(2*phase), without evaluating a doubled machine argument.
            // For MaxValue, a 450-digit decimal Pi reduction independently gives a negative sign.
            double zero = negativeZero ? -0.0 : 0.0;
            double unit = Math.Sign(magnitude);
            foreach (bool hyperbolic in new[] { false, true })
            {
                var input = hyperbolic ? api.Input(magnitude, phase) : api.Input(phase, magnitude);
                var expected = hyperbolic ? new N(unit, zero) : new N(zero, unit);
                foreach (bool forwarded in new[] { false, true })
                {
                    var actual = api.Apply(input, hyperbolic, forwarded);
                    string context = $"{api.Name}: input={input}, hyperbolic={hyperbolic}, forwarded={forwarded}, actual={actual}";
                    SameBits(expected.Real, actual.Real, context + " limiting real");
                    SameBits(expected.Imaginary, actual.Imaginary, context + " limiting imaginary");
                    if (phase == 0)
                        Assert.That(hyperbolic ? actual.Real : actual.Imaginary, Is.EqualTo(Math.Tanh(magnitude)), context + " scalar identity");
                    if (Math.Abs(phase) <= 2 && Math.Abs(magnitude) <= 2048)
                    {
                        var reference = hyperbolic ? N.Tanh(input) : N.Tan(input);
                        double rr = api.Single ? (float)reference.Real : reference.Real;
                        double ri = api.Single ? (float)reference.Imaginary : reference.Imaginary;
                        Assert.That(actual.Real, Is.EqualTo(rr), context + " independent complex real");
                        Assert.That(actual.Imaginary, Is.EqualTo(ri), context + " independent complex imaginary");
                    }
                }
            }
        }

        [Test]
        public static void LargeFiniteArgumentsReachRepresentableLimits()
        {
            foreach (var api in Implementations())
            foreach (double magnitude in api.Single ? new[] { 89.0, 100, 256, api.Maximum } : new[] { 710.0, 1000, 2048, api.Maximum })
            foreach (int sign in new[] { -1, 1 })
            foreach (var phase in Phases(api))
                Limiting(api, sign * magnitude, phase.Phase, phase.NegativeZero);
        }

        [Test]
        public static void OverflowAndFallbackBoundariesKeepTheirContracts()
        {
            foreach (var api in Implementations())
            {
                // q=exp(-2|b|), Tan=(2q sin(2a) + i sign(b)(1-q*q))/(1+2q cos(2a)+q*q).
                // For q<1/8, both |Re| and ||Im|-1| are bounded by 4q.
                // The gate precedes Exp overflow, yet this bound is below half MinSubnormal.
                double logarithm = Math.Log(api.Maximum);
                Assert.That(api.Gate, Is.LessThan(logarithm), api.Name);
                Assert.That(api.Gate + 1, Is.GreaterThan(logarithm), api.Name);
                double logBound = Math.Log(4) - 2 * api.Gate;
                Assert.That(logBound, Is.LessThan(-(api.Single ? 150 : 1075) * Math.Log(2)), api.Name + " zero rounding");
                Assert.That(logBound, Is.LessThan(-(api.Single ? 25 : 54) * Math.Log(2)), api.Name + " unit rounding");
                double overflow = api.Single ? (float)logarithm : logarithm;
                foreach (double magnitude in new[] { api.Neighbor(api.Gate, -1), api.Gate, api.Neighbor(api.Gate, 1),
                    api.Neighbor(overflow, -1), overflow, api.Neighbor(overflow, 1) })
                foreach (int sign in new[] { -1, 1 })
                foreach (var phase in Phases(api))
                {
                    if (magnitude > api.Gate) Limiting(api, sign * magnitude, phase.Phase, phase.NegativeZero);
                    else Unchanged(api, api.Input(phase.Phase, sign * magnitude));
                }
            }
        }

        [Test]
        public static void OrdinaryPolesZerosAndNonfinitePathsStayLiteral()
        {
            foreach (var api in Implementations())
            {
                var random = new Random(201163);
                for (int i = 0; i < 2048; i++)
                    Unchanged(api, api.Input((random.NextDouble() - 0.5) * 32, (random.NextDouble() - 0.5) * 32));
                foreach (double a in new[] { -2.0, -1, -0.25, -0.0, 0.0, 0.25, 1, 2, -api.Maximum, api.Maximum })
                foreach (double b in new[] { -0.25, -0.0, 0.0, 0.25 }) Unchanged(api, api.Input(a, b));
                double pole = api.Single ? (float)(Math.PI / 2) : Math.PI / 2;
                double tiny = api.Single ? float.Epsilon : double.Epsilon;
                foreach (double a in new[] { api.Neighbor(pole, -1), pole, api.Neighbor(pole, 1) })
                foreach (int sign in new[] { -1, 1 })
                foreach (double b in new[] { -0.0001, -tiny, -0.0, 0.0, tiny, 0.0001 })
                    Unchanged(api, api.Input(sign * a, b));
                foreach (double a in new[] { double.NegativeInfinity, -1, -0.0, 0.0, 1, double.PositiveInfinity, double.NaN })
                foreach (double b in new[] { double.NegativeInfinity, -1, -0.0, 0.0, 1, double.PositiveInfinity, double.NaN })
                    if (!double.IsFinite(a) || !double.IsFinite(b)) Unchanged(api, api.Input(a, b));
            }
        }
    }
}

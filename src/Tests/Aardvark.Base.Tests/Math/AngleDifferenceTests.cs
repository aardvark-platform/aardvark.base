using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class AngleDifferenceTests
    {
        [Test]
        public void UnwrappedDifferencesKeepEveryRepresentableBit()
        {
            foreach (var core in Cores())
            foreach (var wide in new[] { false, true })
            {
                var minExponent = wide ? -1074 : -149;
                for (var e = minExponent; e <= 0; e++)
                    CheckSmall(Math.ScaleB(1, e));
                for (var n = 1; n <= 4096; n++)
                    CheckSmall(Math.ScaleB(n, minExponent));
                CheckSmall(1e-20);
                CheckSmall(1e-8);

                var random = new Random(39173);
                for (var i = 0; i < 4096; i++)
                {
                    var a = Round((random.NextDouble() - 0.5) * Pi(wide), wide);
                    var b = Round((random.NextDouble() - 0.5) * Pi(wide), wide);
                    var d = Difference(a, b, wide);
                    if (d >= -Pi(wide) && d < Pi(wide))
                        Check(core, a, b, wide, d == 0 ? 0 : d);
                }

                void CheckSmall(double value)
                {
                    var x = Round(value, wide);
                    foreach (var pair in new[] { (0.0, x), (x, 0.0), (0.0, -x), (-x, 0.0),
                        (x, 2 * x), (-x, -2 * x) })
                    {
                        var d = Difference(pair.Item1, pair.Item2, wide);
                        Check(core, pair.Item1, pair.Item2, wide, d == 0 ? 0 : d);
                    }
                }
            }
        }

        [Test]
        public void HalfTurnNeighborsChooseTheCorrectDirection()
        {
            foreach (var core in Cores())
            foreach (var wide in new[] { false, true })
            {
                var pi = Pi(wide);
                for (var step = -8; step <= 8; step++)
                {
                    var neighbor = wide
                        ? BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(pi) + step)
                        : BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits((float)pi) + step);
                    foreach (var sign in new[] { -1, 1 })
                    foreach (var origin in new[] { 0.0, -pi / 2, pi / 2, -2 * pi, 2 * pi })
                        Check(core, origin, Round(origin + sign * neighbor, wide), wide);
                }
                foreach (var pair in new[] { (0.0, pi), (0.0, -pi), (-pi, 0.0), (pi, 0.0),
                    (-pi / 2, pi / 2), (pi / 2, -pi / 2) })
                    Check(core, pair.Item1, pair.Item2, wide, -pi);
            }
        }

        [Test]
        public void WrappedDifferencesMatchExactBinaryRemainders()
        {
            foreach (var core in Cores())
            foreach (var wide in new[] { false, true })
            {
                var pi = Pi(wide);
                var max = wide ? double.MaxValue : float.MaxValue;
                var tiny = wide ? double.Epsilon : float.Epsilon;
                foreach (var x in new[] { 2 * pi, -2 * pi })
                {
                    Check(core, 0, x, wide, 0);
                    Check(core, x, 0, wide, 0);
                }
                var anchors = new[] { -max, -2000 * pi, -2 * pi, -pi, -1.0, -tiny,
                    0.0, tiny, 1.0, pi, 2 * pi, 2000 * pi, max };
                foreach (var a in anchors)
                foreach (var b in anchors)
                    Check(core, Round(a, wide), Round(b, wide), wide);
                for (var e = 2; e <= (wide ? 1023 : 127); e++)
                for (var offset = -1; offset <= 1; offset++)
                {
                    var power = Math.ScaleB(1, e);
                    var x = wide
                        ? BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(power) + offset)
                        : BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits((float)power) + offset);
                    Check(core, 0, x, wide);
                    Check(core, x, 0, wide);
                }
                var random = new Random(wide ? 39181 : 39182);
                for (var i = 0; i < 4096; i++)
                {
                    double Next() => wide
                        ? BitConverter.Int64BitsToDouble(random.NextInt64(long.MinValue, long.MaxValue))
                        : BitConverter.Int32BitsToSingle(unchecked((int)random.NextInt64(1L << 32)));
                    var a = Next();
                    var b = Next();
                    if (double.IsFinite(a) && double.IsFinite(b)) Check(core, a, b, wide);
                }
            }
        }

        [Test]
        public void ZerosNonfiniteAndAngleDistanceStayCompatible()
        {
            foreach (var core in Cores())
            foreach (var wide in new[] { false, true })
            {
                var pi = Pi(wide);
                var max = wide ? double.MaxValue : float.MaxValue;
                foreach (var a in new[] { 0.0, -0.0 })
                foreach (var b in new[] { 0.0, -0.0 })
                    Check(core, a, b, wide, 0);
                foreach (var x in new[] { 0.0, -0.0, 1.0, -1.0, pi, -pi, max, -max })
                    Check(core, x, x, wide, 0);
                var values = new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity,
                    0.0, -0.0, 1.0, -1.0, max, -max, pi, -pi, 2 * pi, -2 * pi };
                foreach (var a in values)
                foreach (var b in values)
                {
                    var delta = Difference(a, b, wide);
                    var context = Context(core, a, b, wide);
                    if (!double.IsFinite(delta))
                        Assert.IsTrue(double.IsNaN(Apply(core, a, b, wide)), context + " difference");
                    var distance = wide ? core.DistanceD(a, b) : core.DistanceF((float)a, (float)b);
                    var phi = wide ? Math.Abs(delta) % (2 * pi) : (float)(Math.Abs((float)delta) % (float)(2 * pi));
                    var expected = phi > pi ? Round(2 * pi - phi, wide) : phi;
                    if (double.IsNaN(expected)) Assert.IsTrue(double.IsNaN(distance), context + " distance");
                    else AssertBits(expected, distance, context + " distance");
                }
            }
        }

        private static double Pi(bool wide) => wide ? Math.PI : (float)Math.PI;
        private static double Round(double x, bool wide) => wide ? x : (float)x;
        private static double Difference(double a, double b, bool wide) => wide ? b - a : (float)((float)b - (float)a);
        private static double Apply(Core core, double a, double b, bool wide) =>
            wide ? core.DifferenceD(a, b) : core.DifferenceF((float)a, (float)b);
        private static string Context(Core core, double a, double b, bool wide) =>
            $"{core.Name}/{(wide ? "double" : "float")}: alpha={a:R}, beta={b:R} " +
            $"[{BitConverter.DoubleToInt64Bits(a):X16}, {BitConverter.DoubleToInt64Bits(b):X16}]";
        private static void AssertBits(double expected, double actual, string context) =>
            Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context);

        private static void Check(Core core, double a, double b, bool wide, double? expected = null)
        {
            var delta = Difference(a, b, wide);
            var actual = Apply(core, a, b, wide);
            var context = Context(core, a, b, wide);
            if (!double.IsFinite(delta)) Assert.IsTrue(double.IsNaN(actual), context);
            else
            {
                AssertBits(expected ?? ExactRemainder(delta, Pi(wide)), actual, context);
                Assert.IsTrue(actual >= -Pi(wide) && actual < Pi(wide), context + " range");
            }
        }

        // Exact integer arithmetic in units of the smallest double subnormal.
        // Neither the production remainder nor its Pi-shift expression is used.
        private static BigInteger Units(double value)
        {
            var bits = BitConverter.DoubleToInt64Bits(value);
            var exponent = (int)((bits >> 52) & 0x7ff);
            var significand = new BigInteger(bits & 0x000fffffffffffffL);
            if (exponent != 0) significand = (significand | (BigInteger.One << 52)) << (exponent - 1);
            return bits < 0 ? -significand : significand;
        }

        private static double ExactRemainder(double delta, double pi)
        {
            var half = Units(pi);
            var turn = half * 2;
            var remainder = Units(delta) % turn;
            if (remainder >= half) remainder -= turn;
            else if (remainder < -half) remainder += turn;
            if (remainder.IsZero) return 0;
            var magnitude = BigInteger.Abs(remainder);
            var shift = (int)Math.Max(0, magnitude.GetBitLength() - 53);
            var leading = magnitude >> shift;
            Assert.AreEqual(magnitude, leading << shift, "Oracle remainder must be exactly representable");
            return (remainder.Sign < 0 ? -1 : 1) * Math.ScaleB((double)leading, shift - 1074);
        }

        private sealed class Core
        {
            public string Name;
            public Func<float, float, float> DifferenceF, DistanceF;
            public Func<double, double, double> DifferenceD, DistanceD;
            public Core(string name, Assembly assembly)
            {
                Name = name;
                var fun = assembly.GetType("Aardvark.Base.Fun", true);
                DifferenceF = (Func<float, float, float>)fun.GetMethod("AngleDifference", new[] { typeof(float), typeof(float) }).CreateDelegate(typeof(Func<float, float, float>));
                DifferenceD = (Func<double, double, double>)fun.GetMethod("AngleDifference", new[] { typeof(double), typeof(double) }).CreateDelegate(typeof(Func<double, double, double>));
                DistanceF = (Func<float, float, float>)fun.GetMethod("AngleDistance", new[] { typeof(float), typeof(float) }).CreateDelegate(typeof(Func<float, float, float>));
                DistanceD = (Func<double, double, double>)fun.GetMethod("AngleDistance", new[] { typeof(double), typeof(double) }).CreateDelegate(typeof(Func<double, double, double>));
            }
        }

        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location),
                    "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path);
                yield return new Core(target, Assembly.LoadFile(path));
            }
        }
    }
}

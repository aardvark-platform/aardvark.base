using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PythagTests
    {
        private static IEnumerable<(string Target, Func<double, double, double> Pythag)> Functions()
        {
            yield return (typeof(Fun).Assembly.Location, Fun.Pythag);
            var directory = Path.GetDirectoryName(typeof(PythagTests).Assembly.Location);
            var path = Path.GetFullPath(Path.Combine(directory!, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Expected sibling core assembly at '{path}'.");
            var method = Assembly.LoadFile(path).GetType(typeof(Fun).FullName!)!
                .GetMethod(nameof(Fun.Pythag), new[] { typeof(double), typeof(double) });
            yield return (path, method!.CreateDelegate<Func<double, double, double>>());
        }

        [Test]
        public void ExceptionalPairsFollowHypotRegardlessOfOperandOrder()
        {
            var nanBits = new ulong[]
            {
                0x7ff0000000000001, 0x7ff0000000000800, 0x7ff7ffffffffffff,
                0x7ff8000000000000, 0x7ff8123456789abc, 0xfff0000000000001,
                0xfff8000000000000, 0xfff8abcdef012345
            };
            var values = nanBits.Select(bits => BitConverter.Int64BitsToDouble(unchecked((long)bits)))
                .Concat(new[] { double.PositiveInfinity, double.NegativeInfinity,
                    0.0, -0.0, 1.0, -1.0, double.Epsilon, -double.Epsilon, double.MaxValue, -double.MaxValue })
                .ToArray();
            Assert.Multiple(() =>
            {
                foreach (var (target, pythag) in Functions())
                foreach (var a in values)
                foreach (var b in values)
                {
                    if (double.IsFinite(a) && double.IsFinite(b)) continue;
                    var expected = double.Hypot(a, b);
                    foreach (var (x, y) in new[] { (a, b), (b, a) })
                    {
                        var actual = pythag(x, y);
                        var context = $"{target}: a=0x{BitConverter.DoubleToInt64Bits(x):x16}, b=0x{BitConverter.DoubleToInt64Bits(y):x16}";
                        if (double.IsNaN(expected))
                            Assert.That(double.IsNaN(actual), Is.True, context);
                        else
                            Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context);
                    }
                }
            });
        }

        private static void ExactBothOrders(Func<double, double, double> pythag,
            double a, double b, double expected, string target)
        {
            var context = $"{target}: a={a:R}, b={b:R}";
            long bits = BitConverter.DoubleToInt64Bits(expected);
            Assert.AreEqual(bits, BitConverter.DoubleToInt64Bits(pythag(a, b)), context);
            Assert.AreEqual(bits, BitConverter.DoubleToInt64Bits(pythag(b, a)), context + ", reversed");
        }

        [Test]
        public void SignedZerosAndFiniteAxesRetainExactMagnitudes()
        {
            foreach (var (target, pythag) in Functions())
            {
                var magnitudes = Enumerable.Range(-1074, 2098).Select(exponent => Math.ScaleB(1.0, exponent))
                    .Concat(new[] { 0.0, double.MaxValue, BitConverter.Int64BitsToDouble(0x000fffffffffffff), 3.0 * double.Epsilon });
                foreach (var magnitude in magnitudes)
                foreach (var sign in new[] { -1.0, 1.0 })
                foreach (var zero in new[] { 0.0, -0.0 })
                    ExactBothOrders(pythag, sign * magnitude, zero, magnitude, target);
            }
        }

        [Test]
        public void ScaledPythagoreanTriplesAndFiniteExtremesStayExact()
        {
            foreach (var (target, pythag) in Functions())
            {
                // Powers of two make every input and the exact 3-4-5 result representable,
                // including subnormal triples and magnitudes whose squares would overflow.
                for (int exponent = -1074; exponent <= 1021; exponent++)
                foreach (var signA in new[] { -1.0, 1.0 })
                foreach (var signB in new[] { -1.0, 1.0 })
                    ExactBothOrders(pythag, signA * Math.ScaleB(3.0, exponent), signB * Math.ScaleB(4.0, exponent),
                        Math.ScaleB(5.0, exponent), target);

                foreach (var signA in new[] { -1.0, 1.0 })
                foreach (var signB in new[] { -1.0, 1.0 })
                {
                    ExactBothOrders(pythag, signA * double.MaxValue, signB * double.MaxValue, double.PositiveInfinity, target);
                    foreach (var tiny in new[] { double.Epsilon, Math.ScaleB(1.0, -1022), 1.0 })
                        ExactBothOrders(pythag, signA * double.MaxValue, signB * tiny, double.MaxValue, target);
                }
            }
        }
    }
}

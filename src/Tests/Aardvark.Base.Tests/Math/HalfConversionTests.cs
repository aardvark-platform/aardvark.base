using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AardvarkHalf = Aardvark.Base.Half;

namespace Aardvark.Tests
{
    [TestFixture]
    public class HalfConversionTests
    {
        private sealed class Conversions
        {
            public readonly string Name;
            public readonly Func<float, ushort> Construct;
            public readonly Func<float, ushort> Cast;
            public readonly Func<ushort, float> Widen;

            public Conversions(Assembly assembly)
            {
                Name = assembly.Location;
                var half = assembly.GetType(typeof(AardvarkHalf).FullName, true);
                var getBits = half.GetMethod(nameof(AardvarkHalf.GetBits), new[] { half });
                var input = Expression.Parameter(typeof(float));
                Construct = Expression.Lambda<Func<float, ushort>>(Expression.Call(getBits,
                    Expression.New(half.GetConstructor(new[] { typeof(float) }), input)), input).Compile();
                Cast = Expression.Lambda<Func<float, ushort>>(Expression.Call(getBits,
                    Expression.Call(half.GetMethod("op_Explicit", new[] { typeof(float) }), input)), input).Compile();
                var bits = Expression.Parameter(typeof(ushort));
                var toFloat = half.GetMethods().Single(m => m.Name == "op_Implicit" && m.ReturnType == typeof(float)
                    && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == half);
                Widen = Expression.Lambda<Func<ushort, float>>(Expression.Call(toFloat,
                    Expression.Call(half.GetMethod(nameof(AardvarkHalf.ToHalf), new[] { typeof(ushort) }), bits)), bits).Compile();
            }
        }

        private static IEnumerable<Conversions> Targets()
        {
            yield return new Conversions(typeof(AardvarkHalf).Assembly);
            string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, path);
            yield return new Conversions(Assembly.LoadFile(path));
        }

        private static float FromBits(uint bits) => BitConverter.Int32BitsToSingle(unchecked((int)bits));

        // Decode IEEE binary32 and truncate its significand directly, independently of the conversion tables.
        private static ushort Expected(uint bits)
        {
            int sign = (int)((bits >> 16) & 0x8000);
            int exponent = (int)((bits >> 23) & 0xff);
            uint fraction = bits & 0x007fffff;
            if (exponent == 255)
            {
                int payload = (int)(fraction >> 13);
                if (fraction != 0 && payload == 0) payload = 1;
                return (ushort)(sign | 0x7c00 | payload);
            }
            int unbiased = exponent - 127;
            if (unbiased < -24) return (ushort)sign;
            if (unbiased > 15) return (ushort)(sign | 0x7c00);
            if (unbiased < -14) return (ushort)(sign | (int)((0x00800000 | fraction) >> (-unbiased - 1)));
            return (ushort)(sign | ((unbiased + 15) << 10) | (int)(fraction >> 13));
        }

        private static void Check(Conversions target, uint bits, ushort expected)
        {
            float input = FromBits(bits);
            ushort constructed = target.Construct(input), cast = target.Cast(input);
            if (constructed != expected || cast != expected)
                Assert.Fail($"{target.Name}, input=0x{bits:X8}, expected=0x{expected:X4}, constructor=0x{constructed:X4}, cast=0x{cast:X4}");
        }

        [Test]
        public void DiscardedNaNPayloadsRemainNaNsWithTheirOriginalSigns()
        {
            Assert.Multiple(() =>
            {
                foreach (var target in Targets())
                {
                    int mismatches = 0;
                    var examples = new List<string>();
                    foreach (uint sign in new[] { 0U, 0x80000000U })
                    for (uint payload = 1; payload <= 8191; payload++)
                    {
                        uint bits = sign | 0x7f800000 | payload;
                        float input = FromBits(bits);
                        ushort constructed = target.Construct(input), cast = target.Cast(input);
                        bool IsSignedNaN(ushort value) => (value & 0x7c00) == 0x7c00 &&
                            (value & 0x03ff) != 0 && (value & 0x8000) == (sign >> 16);
                        if (!IsSignedNaN(constructed) || !IsSignedNaN(cast) || constructed != cast)
                        {
                            mismatches++;
                            if (examples.Count < 4) examples.Add($"0x{bits:X8}: constructor=0x{constructed:X4}, cast=0x{cast:X4}");
                        }
                    }
                    Assert.That(mismatches, Is.Zero, $"{target.Name}, discarded-payload NaN mismatches={mismatches}; {string.Join("; ", examples)}");
                }
            });
        }

        [Test]
        public void RetainedNaNPayloadsAndFiniteBoundariesKeepTruncation()
        {
            var finite = new HashSet<uint> { 0, 0x7f800000 };
            uint[] fractions = { 0, 1, 0x0fff, 0x1000, 0x1fff, 0x2000, 0x3fffff, 0x7fffff };
            for (uint exponent = 0; exponent < 255; exponent++)
                foreach (uint fraction in fractions) finite.Add((exponent << 23) | fraction);
            foreach (float boundary in new[] { 0f, float.Epsilon, MathF.ScaleB(1f, -25), MathF.ScaleB(1f, -24),
                MathF.ScaleB(1f, -14), 1f, 1.00048828125f, 1.00146484375f, 65504f, 65520f, 65536f, float.MaxValue })
                foreach (float value in new[] { MathF.BitDecrement(boundary), boundary, MathF.BitIncrement(boundary) })
                    finite.Add(unchecked((uint)BitConverter.SingleToInt32Bits(value)) & 0x7fffffff);
            var random = new Random(71933);
            var bytes = new byte[4];
            for (int i = 0; i < 4096; i++)
            {
                random.NextBytes(bytes);
                uint bits = BitConverter.ToUInt32(bytes, 0) & 0x7fffffff;
                if (bits < 0x7f800000) finite.Add(bits);
            }
            foreach (var target in Targets())
            foreach (uint sign in new[] { 0U, 0x80000000U })
            {
                foreach (uint magnitude in finite) Check(target, sign | magnitude, Expected(sign | magnitude));
                for (uint retained = 1; retained <= 1023; retained++)
                foreach (uint discarded in new[] { 1U, 8191U })
                {
                    uint bits = sign | 0x7f800000 | (retained << 13) | discarded;
                    Check(target, bits, (ushort)((sign >> 16) | 0x7c00 | retained));
                }
            }
        }

        [Test]
        public void EveryRawHalfEncodingRoundTripsWithoutLosingBits()
        {
            foreach (var target in Targets())
            for (int bits = 0; bits <= ushort.MaxValue; bits++)
            {
                float single = target.Widen((ushort)bits);
                ushort constructed = target.Construct(single), cast = target.Cast(single);
                if (constructed != bits || cast != bits)
                    Assert.Fail($"{target.Name}, Half=0x{bits:X4}, constructor=0x{constructed:X4}, cast=0x{cast:X4}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ClampWrapTests
    {
        private static readonly (Type Scalar, string Suffix, int Bits, bool Signed)[] s_types =
        {
            (typeof(int), "i", 32, true), (typeof(uint), "ui", 32, false), (typeof(long), "l", 64, true)
        };

        private static IEnumerable<Assembly> CoreAssemblies()
        {
            yield return typeof(Fun).Assembly;
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Fun).Assembly.Location)!,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core assembly: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static BigInteger Wrap(BigInteger x, BigInteger a, BigInteger b)
        {
            var remainder = (x - a) % (b - a);
            return a + (remainder < 0 ? remainder + b - a : remainder);
        }

        private static BigInteger[] Anchors(int bits, bool signed)
        {
            var min = signed ? -(BigInteger.One << (bits - 1)) : BigInteger.Zero;
            var max = (BigInteger.One << (signed ? bits - 1 : bits)) - 1;
            return new BigInteger[] { min, min + 1, min + 2, max - 2, max - 1, max,
                -7, -2, -1, 0, 1, 2, 3, 4, 7, 100,
                -(BigInteger.One << (bits - 2)), BigInteger.One << (bits - 2),
                (BigInteger.One << (bits - 2)) - 1, (BigInteger.One << (bits - 2)) + 1 }
                .Where(v => v >= min && v <= max).Distinct().OrderBy(v => v).ToArray();
        }

        private static IEnumerable<(BigInteger X, BigInteger A, BigInteger B)> Cases(int bits, bool signed, int seeds)
        {
            var anchors = Anchors(bits, signed);
            foreach (var a in anchors)
            foreach (var b in anchors.Where(b => b > a))
            foreach (var x in anchors)
                yield return (x, a, b);
            var random = new Random(41357 + bits + (signed ? 1 : 0));
            var bytes = new byte[bits / 8];
            BigInteger Next() { random.NextBytes(bytes); return new BigInteger(bytes, isUnsigned: !signed); }
            for (int i = 0; i < seeds; i++)
            {
                var a = Next(); var b = Next();
                if (a > b) (a, b) = (b, a);
                if (a == b) { if (b < anchors.Last()) b++; else a--; }
                var x = Next();
                if (i % 8 == 0) x = a;
                else if (i % 8 == 1) x = b;
                else if (i % 8 == 2 && a > anchors.First()) x = a - 1;
                else if (i % 8 == 3 && b < anchors.Last()) x = b + 1;
                yield return (x, a, b);
            }
        }

        private static Func<BigInteger, BigInteger, BigInteger, BigInteger> BindInteger(
            Assembly assembly, Type scalar, string suffix, bool range)
        {
            var x = Expression.Parameter(typeof(BigInteger));
            var a = Expression.Parameter(typeof(BigInteger));
            var b = Expression.Parameter(typeof(BigInteger));
            var xx = Expression.Convert(x, scalar); var aa = Expression.Convert(a, scalar); var bb = Expression.Convert(b, scalar);
            var fun = assembly.GetType(typeof(Fun).FullName!)!;
            Expression call;
            if (range)
            {
                var rt = assembly.GetType($"Aardvark.Base.Range1{suffix}", throwOnError: true)!;
                Assert.That(rt.Assembly, Is.SameAs(assembly));
                call = Expression.Call(fun.GetMethod(nameof(Fun.ClampWrap), new[] { scalar, rt })!, xx,
                    Expression.New(rt.GetConstructor(new[] { scalar, scalar })!, aa, bb));
            }
            else call = Expression.Call(fun.GetMethod(nameof(Fun.ClampWrap), new[] { scalar, scalar, scalar })!, xx, aa, bb);
            return Expression.Lambda<Func<BigInteger, BigInteger, BigInteger, BigInteger>>(
                Expression.Convert(call, typeof(BigInteger)), x, a, b).Compile();
        }

        [Test]
        public void FullDomainIntegerScalarsAndRangesMatchFloorModulo()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (var t in s_types)
            {
                var scalar = BindInteger(assembly, t.Scalar, t.Suffix, false);
                var range = BindInteger(assembly, t.Scalar, t.Suffix, true);
                foreach (var c in Cases(t.Bits, t.Signed, 4096))
                {
                    var expected = Wrap(c.X, c.A, c.B);
                    string context = $"{t.Scalar.Name} x={c.X}, [{c.A},{c.B})";
                    Assert.That(scalar(c.X, c.A, c.B), Is.EqualTo(expected), context);
                    Assert.That(range(c.X, c.A, c.B), Is.EqualTo(expected), context + " range");
                }
            }
        }

        private static Func<BigInteger[], BigInteger[], BigInteger[], BigInteger[]> BindVector(
            Assembly assembly, Type scalar, string suffix, int dimension, bool scalarBounds)
        {
            var vt = assembly.GetType($"Aardvark.Base.V{dimension}{suffix}", throwOnError: true)!;
            var ctor = vt.GetConstructor(Enumerable.Repeat(scalar, dimension).ToArray())!;
            var x = Expression.Parameter(typeof(BigInteger[]));
            var a = Expression.Parameter(typeof(BigInteger[]));
            var b = Expression.Parameter(typeof(BigInteger[]));
            Expression Component(Expression p, int i) => Expression.Convert(Expression.ArrayIndex(p, Expression.Constant(i)), scalar);
            Expression Vector(Expression p) => Expression.New(ctor, Enumerable.Range(0, dimension).Select(i => Component(p, i)));
            var method = assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.ClampWrap),
                new[] { vt, scalarBounds ? scalar : vt, scalarBounds ? scalar : vt })!;
            Assert.That(method.ReturnType.Assembly, Is.SameAs(assembly));
            var result = Expression.Variable(vt);
            var body = Expression.Block(new[] { result }, Expression.Assign(result, Expression.Call(method,
                Vector(x), scalarBounds ? Component(a, 0) : Vector(a), scalarBounds ? Component(b, 0) : Vector(b))),
                Expression.NewArrayInit(typeof(BigInteger), Enumerable.Range(0, dimension)
                    .Select(i => Expression.Convert(Expression.Field(result, "XYZW"[i].ToString()), typeof(BigInteger)))));
            return Expression.Lambda<Func<BigInteger[], BigInteger[], BigInteger[], BigInteger[]>>(body, x, a, b).Compile();
        }

        [Test]
        public void AllIntegerVectorsWrapEveryComponentWithBothBoundForms()
        {
            foreach (var assembly in CoreAssemblies())
            {
                int overloads = 0;
                foreach (var t in s_types)
                {
                    var cases = Cases(t.Bits, t.Signed, 256).ToArray();
                    foreach (int dimension in new[] { 2, 3, 4 })
                    foreach (bool scalarBounds in new[] { false, true })
                    {
                        var call = BindVector(assembly, t.Scalar, t.Suffix, dimension, scalarBounds);
                        overloads++;
                        for (int i = 0; i < cases.Length; i++)
                        {
                            var components = Enumerable.Range(0, dimension).Select(j => cases[(i + j) % cases.Length]).ToArray();
                            var xs = components.Select(c => c.X).ToArray();
                            var aa = components.Select(c => c.A).ToArray();
                            var bb = components.Select(c => c.B).ToArray();
                            var actual = call(xs, aa, bb);
                            for (int j = 0; j < dimension; j++)
                            {
                                int k = scalarBounds ? 0 : j;
                                Assert.That(actual[j], Is.EqualTo(Wrap(xs[j], aa[k], bb[k])),
                                    $"V{dimension}{t.Suffix}, scalar bounds={scalarBounds}, component {j}, x={xs[j]}, [{aa[k]},{bb[k]})");
                            }
                        }
                    }
                }
                Assert.That(overloads, Is.EqualTo(18));
            }
        }

        private static BigInteger LegacyInteger(BigInteger x, BigInteger a, BigInteger b, int bits, bool signed)
        {
            BigInteger Narrow(BigInteger value)
            {
                var modulus = BigInteger.One << bits;
                value = (value % modulus + modulus) % modulus;
                return signed && value >= (modulus >> 1) ? value - modulus : value;
            }
            var delta = Narrow(x - a); var width = Narrow(b - a);
            if (width == 0) throw new DivideByZeroException();
            if (signed && delta == -(BigInteger.One << (bits - 1)) && width == -1) throw new OverflowException();
            var remainder = delta % width;
            if (remainder < 0) remainder = Narrow(remainder + width);
            return Narrow(remainder + a);
        }

        private static void CheckLegacy<T>(Func<T> actual, Func<T> oracle, string context)
        {
            T expected;
            try { expected = oracle(); }
            catch (Exception e) when (e is DivideByZeroException || e is OverflowException)
            {
                Assert.Throws(e.GetType(), () => actual(), context);
                return;
            }
            Assert.That(actual(), Is.EqualTo(expected), context);
        }

        [Test]
        public void InvalidIntegerIntervalsKeepLegacyResultsAndExceptions()
        {
            foreach (var assembly in CoreAssemblies())
            foreach (var t in s_types)
            {
                var scalar = BindInteger(assembly, t.Scalar, t.Suffix, false);
                var range = BindInteger(assembly, t.Scalar, t.Suffix, true);
                var values = Anchors(t.Bits, t.Signed);
                foreach (var a in values)
                foreach (var b in values.Where(b => b <= a))
                foreach (var x in values)
                {
                    string context = $"invalid {t.Scalar.Name} x={x}, [{a},{b})";
                    Func<BigInteger> oracle = () => LegacyInteger(x, a, b, t.Bits, t.Signed);
                    CheckLegacy(() => scalar(x, a, b), oracle, context);
                    CheckLegacy(() => range(x, a, b), oracle, context + " range");
                }
            }
        }

        [Test]
        public void SmallerIntegersRealValuesAndModPKeepTheirBehavior()
        {
            foreach (var assembly in CoreAssemblies())
            {
                foreach (var t in new[] { (typeof(byte), 8, false), (typeof(sbyte), 8, true),
                    (typeof(short), 16, true), (typeof(ushort), 16, false) })
                {
                    var call = BindInteger(assembly, t.Item1, "", false);
                    var values = t.Item2 == 8 ? Enumerable.Range(t.Item3 ? -128 : 0, 256).Select(i => new BigInteger(i)).ToArray()
                        : Anchors(t.Item2, t.Item3);
                    var bounds = Anchors(t.Item2, t.Item3);
                    foreach (var a in bounds)
                    foreach (var b in bounds.Where(b => b > a))
                    foreach (var x in values)
                        Assert.That(call(x, a, b), Is.EqualTo(Wrap(x, a, b)), $"{t.Item1.Name} x={x}, [{a},{b})");
                    Assert.Throws<DivideByZeroException>(() => call(0, 0, 0));
                }
                var fun = assembly.GetType(typeof(Fun).FullName!)!;
                void Real<T>(T[] values, Func<T, T, T, T> legacy, string suffix)
                {
                    var call = (Func<T, T, T, T>)fun.GetMethod(nameof(Fun.ClampWrap), new[] { typeof(T), typeof(T), typeof(T) })!
                        .CreateDelegate(typeof(Func<T, T, T, T>));
                    var range = suffix == "" ? null : BindRealRange<T>(assembly, suffix);
                    foreach (var x in values)
                    foreach (var a in values)
                    foreach (var b in values)
                    {
                        string context = $"{typeof(T).Name} x={x}, [{a},{b})";
                        CheckLegacy(() => call(x, a, b), () => legacy(x, a, b), context);
                        if (range != null) CheckLegacy(() => range(x, a, b), () => legacy(x, a, b), context + " range");
                    }
                }
                Real(new[] { -7.25f, -0.0f, 0f, 2.5f, 9f, float.PositiveInfinity, float.NaN },
                    (x, a, b) => { var r = (x - a) % (b - a); return (r < 0 ? r + (b - a) : r) + a; }, "f");
                Real(new[] { -7.25, -0.0, 0.0, 2.5, 9.0, double.NegativeInfinity, double.NaN },
                    (x, a, b) => { var r = (x - a) % (b - a); return (r < 0 ? r + (b - a) : r) + a; }, "d");
                Real(new[] { decimal.MinValue, -7.25m, 0m, 2.5m, 9m, decimal.MaxValue },
                    (x, a, b) => { var r = (x - a) % (b - a); return (r < 0 ? r + (b - a) : r) + a; }, "");
                var mod = (Func<long, long, long>)fun.GetMethod(nameof(Fun.ModP), new[] { typeof(long), typeof(long) })!
                    .CreateDelegate(typeof(Func<long, long, long>));
                foreach (long x in new[] { long.MinValue, -17L, -1L, 0L, 1L, 17L, long.MaxValue })
                foreach (long width in new[] { -7L, -1L, 0L, 1L, 7L, long.MaxValue })
                    CheckLegacy(() => mod(x, width), () => { var r = x % width; return r < 0 ? unchecked(r + width) : r; }, $"ModP({x},{width})");
            }
        }

        private static Func<T, T, T, T> BindRealRange<T>(Assembly assembly, string suffix)
        {
            var x = Expression.Parameter(typeof(T)); var a = Expression.Parameter(typeof(T)); var b = Expression.Parameter(typeof(T));
            var range = assembly.GetType($"Aardvark.Base.Range1{suffix}", throwOnError: true)!;
            var call = Expression.Call(assembly.GetType(typeof(Fun).FullName!)!.GetMethod(nameof(Fun.ClampWrap), new[] { typeof(T), range })!,
                x, Expression.New(range.GetConstructor(new[] { typeof(T), typeof(T) })!, a, b));
            return Expression.Lambda<Func<T, T, T, T>>(call, x, a, b).Compile();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ColorTests : TestSuite
    {
        public ColorTests() : base() { }
        public ColorTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestColor()
        {
            ConversionTests();
        }

        public void ConversionTests()
        {
            using (Report.JobTimed("color conversion tests"))
            {
                Report.BeginTimed("create color arrays");
                var c3bVol = new Volume<C3b>(17, 17, 17).SetByCoord((r, g, b) => new C3b(15 * r, 15 * g, 15 * b));
                var c3bArray = c3bVol.Data;
                var c3fArray = c3bArray.Map(C3f.FromC3b);
                var cieXYZArray = c3fArray.Map(c => c.SRGBToCieXYZf());
                Report.End();

                var count = c3bArray.Length;

                Test.Begin("CieXYZ tests");
                for (int i = 0; i < count; i++)
                {
                    var ciexyz = cieXYZArray[i];
                    var c3b = ciexyz.ToSRGB().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

                Test.Begin("CieYxy tests");
                for (int i = 0; i < count; i++)
                {
                    var cieyxy = cieXYZArray[i].ToCieYxyf();
                    var c3b = cieyxy.ToCieXYZf().ToSRGB().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

                Test.Begin("HSV tests");
                for (int i = 0; i < count; i++)
                {
                    var hsvf = c3fArray[i].ToHSVf();
                    var c3b = hsvf.ToC3f().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

                Test.Begin("HSL tests");
                for (int i = 0; i < count; i++)
                {
                    var hslf = c3fArray[i].ToHSLf();
                    var c3b = hslf.ToC3f().ToC3b();
                    var original = c3bArray[i];
                    Test.IsFalse(c3b.AnyDifferent(original), "{0} != {1}", c3b, original);
                }
                Test.End();

            }
        }

        [Test]
        public void IndexerC4b()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = new C4b(rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt());

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void IndexerC4us()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = new C4us(rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt(), rnd.UniformInt());

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void IndexerC4ui()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = rnd.UniformC4ui();

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void IndexerC4f()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = rnd.UniformC4f();

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        private static void CheckByteAverageTargets(string check)
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            foreach (var assembly in new[] { typeof(C4b).Assembly, Assembly.LoadFile(path) })
            {
                var color = assembly.GetType("Aardvark.Base.C4b", true)!;
                try
                {
                    typeof(ColorTests).GetMethod(check, BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(color).Invoke(null, new object[] { assembly });
                }
                catch (TargetInvocationException e) when (e.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                }
            }
        }

        private static (Func<IEnumerable<T>, T> Average, Func<C4b, T> Encode, Func<T, C4b> Decode)
            ByteAverageApi<T>(Assembly assembly)
        {
            Assert.That(typeof(T).Assembly, Is.SameAs(assembly));
            var average = assembly.GetType("Aardvark.Base.Col", true)!.GetMethod("Average", new[] { typeof(IEnumerable<T>) })!;
            var input = Expression.Parameter(typeof(C4b));
            var output = Expression.Parameter(typeof(T));
            string[] channels = { "R", "G", "B", "A" };
            var arguments = new[] { typeof(byte), typeof(byte), typeof(byte), typeof(byte) };
            return (
                (Func<IEnumerable<T>, T>)average.CreateDelegate(typeof(Func<IEnumerable<T>, T>)),
                Expression.Lambda<Func<C4b, T>>(Expression.New(typeof(T).GetConstructor(arguments)!,
                    channels.Select(c => Expression.Field(input, c))), input).Compile(),
                Expression.Lambda<Func<T, C4b>>(Expression.New(typeof(C4b).GetConstructor(arguments)!,
                    channels.Select(c => Expression.Field(output, c))), output).Compile());
        }

        [Test]
        public void AverageByteColorsMatchesIntegerChannelSums()
            => CheckByteAverageTargets(nameof(CheckByteAverageValues));

        private static void CheckByteAverageValues<T>(Assembly assembly)
        {
            var api = ByteAverageApi<T>(assembly);
            void Check(C4b[] colors, string context)
            {
                long r = 0, g = 0, b = 0, a = 0;
                foreach (var c in colors) { r += c.R; g += c.G; b += c.B; a += c.A; }
                var n = colors.Length;
                var expected = new C4b((byte)(r / n), (byte)(g / n), (byte)(b / n), (byte)(a / n));
                var actual = api.Decode(api.Average(colors.Select(api.Encode).ToArray()));
                Assert.That(actual, Is.EqualTo(expected), $"{assembly.Location}: {context}; sums={r},{g},{b},{a}; n={n}");
            }

            for (int a = 0; a < 256; a++)
                Check(new[] { new C4b((byte)a, (byte)(255 - a), (byte)(a ^ 0x5a), (byte)a) }, $"singleton alpha={a}");
            for (int a = 0; a < 256; a++)
            for (int b = 0; b < 256; b++)
                Check(new[] { new C4b((byte)a, (byte)b, (byte)0, (byte)a), new C4b((byte)b, (byte)a, (byte)255, (byte)b) },
                    $"pair alpha={a},{b}");
            foreach (int n in new[] { 3, 4, 7, 8, 16, 31, 64, 127, 256, 257, 1024 })
            {
                var colors = Enumerable.Range(0, n).Select(i =>
                    new C4b((byte)(13 * i + 31), (byte)(7 * i + 9), (byte)(255 - (i & 255)), (byte)(i == 0 ? 0 : 1))).ToArray();
                Check(colors, $"distributed alpha sum=n-1; n={n}");
                int remaining = n - 1;
                for (int i = 0; i < n; i++) { colors[i].A = (byte)Math.Min(255, remaining); remaining -= colors[i].A; }
                Check(colors, $"packed alpha sum=n-1; n={n}");
            }
            foreach (int n in new[] { 1, 2, 4, 8, 64, 1024 })
            foreach (byte alpha in new byte[] { 0, 255 })
                Check(Enumerable.Range(0, n).Select(i => new C4b((byte)i, (byte)(255 - (i & 255)), (byte)51, alpha)).ToArray(),
                    $"transparent/opaque; n={n}, alpha={alpha}");
        }

        [Test]
        public void AverageByteColorsKeepsEnumerationAndEmptyContracts()
            => CheckByteAverageTargets(nameof(CheckByteAverageEnumeration));

        // Keep the original empty arithmetic, including runtime-dependent unchecked byte conversions.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static C4b LegacyEmptyByteAverage(long count)
        {
            var scale = 1.0 / count;
            return new C4b((byte)(0.0 * scale), (byte)(0.0 * scale), (byte)(0.0 * scale), (byte)(1.0 * scale));
        }

        private static void CheckByteAverageEnumeration<T>(Assembly assembly)
        {
            var api = ByteAverageApi<T>(assembly);
            var empty = new AverageSequence<T>(Array.Empty<T>(), 0);
            Assert.That(api.Decode(api.Average(empty)), Is.EqualTo(LegacyEmptyByteAverage(0)), assembly.Location);
            Assert.That(string.Join(",", empty.Events), Is.EqualTo("get,move0,dispose"), assembly.Location);
            Assert.That(api.Decode(api.Average(Array.Empty<T>())), Is.EqualTo(LegacyEmptyByteAverage(0)), assembly.Location);
            Assert.Throws<NullReferenceException>(() => api.Average(null!), assembly.Location);

            var colors = new[] { new C4b(10, 20, 30, 0), new C4b(40, 50, 60, 2) }.Select(api.Encode).ToArray();
            for (int fault = 0; fault <= 5; fault++)
            {
                var source = new AverageSequence<T>(colors, fault);
                Assert.That(source.Events, Is.Empty, $"{assembly.Location}: deferred source, fault={fault}");
                if (fault == 0)
                    Assert.That(api.Decode(api.Average(source)), Is.EqualTo(new C4b(25, 35, 45, 1)), assembly.Location);
                else
                    Assert.That(Assert.Throws<InvalidOperationException>(() => api.Average(source)),
                        Is.SameAs(fault >= 4 ? source.DisposeFailure : source.Failure), $"{assembly.Location}: fault={fault}");
                string expected = fault switch
                {
                    1 => "get",
                    2 or 5 => "get,move0,current0,move1,dispose",
                    3 => "get,move0,current0,move1,current1,dispose",
                    _ => "get,move0,current0,move1,current1,move2,dispose"
                };
                Assert.That(string.Join(",", source.Events), Is.EqualTo(expected), $"{assembly.Location}: fault={fault}");
            }
        }

        private sealed class AverageSequence<T> : IEnumerable<T>, IEnumerator<T>
        {
            private readonly T[] values;
            private readonly int fault;
            private int index = -1;
            public readonly List<string> Events = new();
            public readonly InvalidOperationException Failure = new("source failure");
            public readonly InvalidOperationException DisposeFailure = new("dispose failure");
            public AverageSequence(T[] values, int fault) { this.values = values; this.fault = fault; }
            public IEnumerator<T> GetEnumerator()
            {
                if (Events.Count != 0) throw new InvalidOperationException("Sequence acquired twice.");
                Events.Add("get");
                if (fault == 1) throw Failure;
                return this;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public bool MoveNext()
            {
                Events.Add($"move{++index}");
                if ((fault == 2 || fault == 5) && index == 1) throw Failure;
                return index < values.Length;
            }
            public T Current
            {
                get
                {
                    Events.Add($"current{index}");
                    if (fault == 3 && index == 1) throw Failure;
                    return values[index];
                }
            }
            object IEnumerator.Current => Current!;
            public void Reset() => throw new NotSupportedException();
            public void Dispose() { Events.Add("dispose"); if (fault >= 4) throw DisposeFailure; }
        }

        [Test]
        public void AverageC4fPreservesChannelOrder()
        {
            var colors = new[]
            {
                new C4f(0.10f, 0.20f, 0.70f, 0.30f),
                new C4f(0.30f, 0.60f, 0.10f, 0.90f),
            };

            var average = Col.Average(colors);

            Assert.AreEqual(0.20f, average.R, 1e-6f);
            Assert.AreEqual(0.40f, average.G, 1e-6f);
            Assert.AreEqual(0.40f, average.B, 1e-6f);
            Assert.AreEqual(0.60f, average.A, 1e-6f);
        }

        [Test]
        public void IndexerC4d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 32; i++)
            {
                var color = rnd.UniformC4d();

                Assert.AreEqual(color.R, color[0]);
                Assert.AreEqual(color.G, color[1]);
                Assert.AreEqual(color.B, color[2]);
                Assert.AreEqual(color.A, color[3]);
            }
        }

        [Test]
        public void Parse()
        {
            var rnd = new RandomSystem(0);

            for (int i = 0; i < 100; i++)
            {
                var c = rnd.UniformC4ui();
                var s1 = $"[{c.R}, {c.G}, {c.B}, {c.A}]";
                var s2 = $"[{c.R}, {c.G}, {c.B}]";

                Assert.AreEqual(c.RGB, C3ui.Parse(s1));
                Assert.AreEqual(c, C4ui.Parse(s1));
                Assert.AreEqual(c.RGB, C3ui.Parse(s2));
                Assert.AreEqual(new C4ui(c.RGB, uint.MaxValue), C4ui.Parse(s2));
            }
        }

        [Test]
        public void ParseTooManyComponents()
        {
            var t1 = new Text("[1,2,3");
            var c1 = t1.NestedBracketSplitCount2(1);
            var r1 = t1.NestedBracketSplit(1).ToArray();

            var t2 = new Text("[42]");
            var c2 = t2.NestedBracketSplitCount2(1);
            var r2 = t2.NestedBracketSplit(1).ToArray();

            var t3 = new Text("[42,43]");
            var c3 = t3.NestedBracketSplitCount2(1);
            var r3 = t3.NestedBracketSplit(1).ToArray();

            Assert.AreEqual(r1.Length, c1);
            Assert.AreEqual(r2.Length, c2);
            Assert.AreEqual(r3.Length, c3);

            Assert.IsFalse(C4d.TryParse("[1, 2, 3, 4, 5]", out C4d _));
        }

        [Test]
        public void ParseHex()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 100; i++)
            {
                var color = rnd.UniformC4d().ToC4b();
                var str = color.ToHexString();

                Assert.IsTrue(Col.TryParseHex(str, out C4b result));
                Assert.IsTrue(C4b.TryParse(str, out C4b result2));
                Assert.AreEqual(color, result);
                Assert.AreEqual(result, result2);
            }
        }

        [Test]
        public void ParseHexSingleDigit()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 100; i++)
            {
                var str = rnd.UniformC4d().ToHexString();
                var dbl = $"#{str[0]}{str[0]}{str[2]}{str[2]}{str[4]}{str[4]}";
                var sgl = $"0x{str[0]}{str[2]}{str[4]}F";

                Assert.IsTrue(Col.TryParseHex(dbl, out C4b a));
                Assert.IsTrue(C4b.TryParse(sgl, out C4b b));
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        [SetCulture("de-DE")]
        public void ParseWithBadCulture()
        {
            var rnd = new RandomSystem(1);
            var col = rnd.UniformC4d();
            var str = col.ToString();
            var res = C4d.Parse(str);

            Assert.True(Fun.ApproximateEquals(col, res));
        }

        [Test]
        public void C4fArithmetic()
        {
            var rnd = new RandomSystem(1);
            var c0 = rnd.UniformC4f();
            var c1 = rnd.UniformC4f();

            Assert.AreEqual( c0.ToV4f() * c1.ToV4f(), (c0   * c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() * c1.R,       (c0   * c1.R).ToV4f() );
            Assert.AreEqual( c0.R       * c1.ToV4f(), (c0.R * c1).ToV4f()   );

            Assert.AreEqual( c0.ToV4f() / c1.ToV4f(), (c0   / c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() / c1.R,       (c0   / c1.R).ToV4f() );
            Assert.AreEqual( c0.R       / c1.ToV4f(), (c0.R / c1).ToV4f()   );

            Assert.AreEqual( c0.ToV4f() + c1.ToV4f(), (c0   + c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() + c1.R,       (c0   + c1.R).ToV4f() );
            Assert.AreEqual( c0.R       + c1.ToV4f(), (c0.R + c1).ToV4f()   );

            Assert.AreEqual( c0.ToV4f() - c1.ToV4f(), (c0   - c1).ToV4f()   );
            Assert.AreEqual( c0.ToV4f() - c1.R,       (c0   - c1.R).ToV4f() );
            Assert.AreEqual( c0.R       - c1.ToV4f(), (c0.R - c1).ToV4f()   );
        }

        [Test]
        public void C4bArithmetic()
        {
            var c0 = new C4b(199, 203, 151, 130);
            var c1 = new C4b(35, 11, 66, 47);

            Assert.AreEqual( c0.ToV4i() + c1.ToV4i(),      (c0   + c1).ToV4i()   );
            Assert.AreEqual( c0.ToV4i() + c1.R,            (c0   + c1.R).ToV4i() );
            Assert.AreEqual( c1.R       + c0.ToV4i(),      (c1.R + c0).ToV4i()   );

            Assert.AreEqual( C3b.White.ToV4i(),            (c0   + c0).ToV4i()   );
            Assert.AreEqual( C3b.White.ToV4i(),            (c0   + c0.R).ToV4i() );
            Assert.AreEqual( C3b.White.ToV4i(),            (c0.R + c0).ToV4i()   );

            Assert.AreEqual( c0.ToV4i() - c1.ToV4i(),      (c0   - c1).ToV4i()   );
            Assert.AreEqual( c0.ToV4i() - c1.R,            (c0   - c1.R).ToV4i() );
            Assert.AreEqual( c0.R       - c1.ToV4i(),      (c0.R - c1).ToV4i()   );

            Assert.AreEqual( V4i.Zero,                     (c1   - c0).ToV4i()   );
            Assert.AreEqual( V4i.Zero,                     (c1   - c0.R).ToV4i() );
            Assert.AreEqual( V4i.Zero,                     (c1.R - c0).ToV4i()   );
        }
    }
}

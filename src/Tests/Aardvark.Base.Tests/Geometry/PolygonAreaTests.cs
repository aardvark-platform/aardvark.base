using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class PolygonAreaTests
    {
        private static readonly (long X, long Y)[][] Outlines =
        {
            new[] { (0L, 0L), (1L, 20L), (2L, 41L) },
            new[] { (0L, 0L), (20L, 1L), (41L, 2L) },
            new[] { (0L, 0L), (4L, 0L), (0L, 3L) },
            new[] { (0L, 0L), (5L, 0L), (5L, 1L), (2L, 1L), (2L, 4L), (0L, 4L) },
            new[] { (0L, 0L), (6L, 0L), (6L, 5L), (4L, 5L), (4L, 2L), (2L, 2L), (2L, 5L), (0L, 5L) },
            new[] { (0L, 0L), (1L, 20L), (2L, 40L), (3L, 60L) },
            new[] { (2L, -3L), (2L, -3L), (2L, -3L), (2L, -3L) }
        };

        [Test]
        public void TranslatedOutlinesKeepSignedAreaAcrossOrderings()
        {
            foreach (var core in Cores())
            {
                var shift = 1L << (core.Single ? 20 : 49);
                foreach (var outline in Outlines)
                foreach (var offset in new[] { (0L, 0L), (shift, shift), (-shift, -shift), (shift, -shift), (-shift, shift), (0L, shift), (shift, 0L), (shift + 8, -shift + 16) })
                {
                    var points = outline.Select(p => (p.X + offset.Item1, p.Y + offset.Item2)).ToArray();
                    foreach (var ordered in Orderings(points))
                        Check(core, ordered, 0, 0, "known outline, translation=" + offset);
                }
                var required = Outlines[0].Select(p => (p.X + shift, p.Y + shift)).ToArray();
                Assert.AreEqual(0.5, ExactSignedArea(required), "Independent required triangle area");
                var polygon = core.Create(required, 0, 0).Polygon;
                Assert.AreEqual(0.5, core.Signed(polygon), core.Name + " required translated half-unit triangle");
            }
        }

        [Test]
        public void SeededIntegerOutlinesMatchExactArea()
        {
            foreach (var core in Cores())
            {
                var random = new Random(91931);
                var shift = 1L << (core.Single ? 20 : 49);
                for (var sample = 0; sample < 512; sample++)
                {
                    (long X, long Y)[] points;
                    if (sample % 2 == 0)
                        points = Enumerable.Range(0, 3).Select(_ => ((long)random.Next(-32, 33), (long)random.Next(-32, 33))).ToArray();
                    else
                        points = MonotoneOutline(random.Next(2, 17), random);
                    var offset = (sample % 3 == 0 ? -shift : shift, sample % 5 == 0 ? -shift : shift);
                    foreach (var translation in new[] { (0L, 0L), offset })
                    foreach (var ordered in Orderings(points.Select(p => (p.X + translation.Item1, p.Y + translation.Item2)).ToArray()))
                        Check(core, ordered, 0, 0, $"seed91931, sample={sample}, translation={translation}");
                }
            }
        }

        [Test]
        public void OrdinaryShortAndPaddedPolygonsKeepTheirContracts()
        {
            foreach (var core in Cores())
            {
                Assert.AreEqual(0L, BitConverter.DoubleToInt64Bits(core.Signed(core.Default)), core.Name + " default signed zero");
                Assert.AreEqual(0L, BitConverter.DoubleToInt64Bits(core.Absolute(core.Default)), core.Name + " default area zero");
                Assert.AreEqual(0, core.Signed(core.NullBacked), core.Name + " null-backed short polygon");
                (long X, long Y)[][] shapes = new[]
                {
                    Array.Empty<(long X, long Y)>(), new[] { (7L, -9L) }, new[] { (-3L, 4L), (8L, -2L) },
                    Outlines[0], Outlines[2], Outlines[3], Outlines[5], Outlines[6],
                    MonotoneOutline(8, new Random(91937)), MonotoneOutline(16, new Random(91939))
                };
                foreach (var shape in shapes)
                foreach (var offset in new[] { (0L, 0L), (8L, -16L) })
                foreach (var ordered in Orderings(shape.Select(p => (p.X + offset.Item1, p.Y + offset.Item2)).ToArray()))
                {
                    Check(core, ordered, 0, 0, "ordinary exact-sized");
                    foreach (var spare in new[] { 1, 5, 17 })
                    foreach (var poison in new[] { 0, 1, 2 })
                        Check(core, ordered, spare, poison, "ordinary active prefix");
                }
            }
        }

        private static void Check(Core core, (long X, long Y)[] points, int spare, int poison, string label)
        {
            var expected = ExactSignedArea(points);
            Assert.AreEqual(expected, core.Single ? (double)(float)expected : expected, "Exactly representable area oracle");
            var fixture = core.Create(points, spare, poison);
            var before = core.Snapshot(fixture.Storage);
            var context = $"{core.Name}: {label}, count={points.Length}, spare={spare}, poison={poison}, points={string.Join(";", points)}";
            var signed = core.Signed(fixture.Polygon);
            var area = core.Absolute(fixture.Polygon);
            Assert.AreEqual(expected, signed, context + " signed area");
            Assert.AreEqual(Math.Abs(expected), area, context + " absolute area");
            Assert.AreEqual(Math.Abs(signed), area, context + " forwarding");
            if (points.Length < 3)
                Assert.AreEqual(0L, BitConverter.DoubleToInt64Bits(signed), context + " short positive zero");
            Assert.AreEqual(points.Length, core.Count(fixture.Polygon), context + " source count");
            Assert.AreSame(fixture.Storage, core.Storage(fixture.Polygon), context + " source storage");
            CollectionAssert.AreEqual(before, core.Snapshot(fixture.Storage), context + " source coordinates");
        }

        private static double ExactSignedArea((long X, long Y)[] points)
        {
            BigInteger twice = 0;
            for (var i = 0; i < points.Length; i++)
            {
                var next = points[(i + 1) % points.Length];
                twice += (BigInteger)points[i].X * next.Y - (BigInteger)points[i].Y * next.X;
            }
            var area = (double)twice / 2;
            Assert.AreEqual(twice, new BigInteger(area * 2), "Exact bounded integer oracle conversion");
            return area;
        }

        private static IEnumerable<(long X, long Y)[]> Orderings((long X, long Y)[] source)
        {
            if (source.Length == 0) { yield return source; yield break; }
            foreach (var reverse in new[] { false, true })
            for (var shift = 0; shift < source.Length; shift++)
            {
                var points = new (long X, long Y)[source.Length];
                for (var i = 0; i < points.Length; i++)
                    points[i] = source[(shift + (reverse ? -i + source.Length : i)) % source.Length];
                yield return points;
            }
        }

        private static (long X, long Y)[] MonotoneOutline(int halfCount, Random random)
        {
            var points = new (long X, long Y)[2 * halfCount];
            for (var i = 0; i < halfCount; i++)
            {
                points[i] = (4L * i, random.Next(1, 33));
                points[2 * halfCount - 1 - i] = (4L * i, -random.Next(1, 33));
            }
            return points;
        }

        private sealed class Core
        {
            public readonly string Name;
            public readonly bool Single;
            private readonly Type vector, polygon;
            private readonly ConstructorInfo vectorCtor, polygonCtor;
            private readonly MethodInfo signed, absolute;
            private readonly FieldInfo x, y, storage;
            private readonly PropertyInfo count;
            public Core(Assembly assembly, string target, bool single)
            {
                Single = single;
                var suffix = single ? "f" : "d";
                Name = target + "/Polygon2" + suffix;
                vector = assembly.GetType("Aardvark.Base.V2" + suffix, true);
                polygon = assembly.GetType("Aardvark.Base.Polygon2" + suffix, true);
                var extensions = assembly.GetType("Aardvark.Base.Polygon2" + suffix + "Extensions", true);
                var scalar = single ? typeof(float) : typeof(double);
                vectorCtor = vector.GetConstructor(new[] { scalar, scalar });
                polygonCtor = polygon.GetConstructor(new[] { vector.MakeArrayType(), typeof(int) });
                signed = extensions.GetMethod("ComputeSignedArea", new[] { polygon });
                absolute = extensions.GetMethod("ComputeArea", new[] { polygon });
                x = vector.GetField("X"); y = vector.GetField("Y");
                storage = polygon.GetField("m_pointArray", BindingFlags.NonPublic | BindingFlags.Instance);
                count = polygon.GetProperty("PointCount");
            }
            public (object Polygon, Array Storage) Create((long X, long Y)[] points, int spare, int poison)
            {
                var array = Array.CreateInstance(vector, points.Length + spare);
                for (var i = 0; i < array.Length; i++)
                {
                    double px, py;
                    if (i < points.Length)
                    {
                        px = points[i].X; py = points[i].Y;
                        Assert.AreEqual(px, Single ? (double)(float)px : px, Name + " representable X");
                        Assert.AreEqual(py, Single ? (double)(float)py : py, Name + " representable Y");
                    }
                    else { px = poison == 0 ? 1000000 : poison == 1 ? double.NaN : double.PositiveInfinity; py = -px; }
                    array.SetValue(vectorCtor.Invoke(new[] { Scalar(px), Scalar(py) }), i);
                }
                return (polygonCtor.Invoke(new object[] { array, points.Length }), array);
            }
            private object Scalar(double value) => Single ? (object)(float)value : (object)value;
            public object Default => Activator.CreateInstance(polygon);
            public object NullBacked => polygonCtor.Invoke(new object[] { null, 0 });
            public double Signed(object value) => Convert.ToDouble(signed.Invoke(null, new[] { value }));
            public double Absolute(object value) => Convert.ToDouble(absolute.Invoke(null, new[] { value }));
            public int Count(object value) => (int)count.GetValue(value);
            public object Storage(object value) => storage.GetValue(value);
            public long[] Snapshot(Array values) => values.Cast<object>().SelectMany(p => new[]
            {
                BitConverter.DoubleToInt64Bits(Convert.ToDouble(x.GetValue(p))),
                BitConverter.DoubleToInt64Bits(Convert.ToDouble(y.GetValue(p)))
            }).ToArray();
        }

        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Polygon2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0",
                    assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new Core(assembly, target, true);
                yield return new Core(assembly, target, false);
            }
        }
    }
}

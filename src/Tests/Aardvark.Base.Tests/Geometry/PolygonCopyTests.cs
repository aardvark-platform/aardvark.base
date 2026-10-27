using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Aardvark.Tests.Geometry
{
    public class PolygonCopyTests
    {
        [Test]
        public void CopiesKeepOnlyActiveVerticesAndIndependentStorage()
        {
            foreach (var core in Cores())
            foreach (var count in new[] { 0, 1, 2, 3, 4, 7, 16 })
            foreach (var spare in new[] { 1, 5, 17 })
            foreach (var suffix in new[] { 0.0, 1000.0, double.NaN })
            {
                var points = Prefix(core.Dimension, count);
                var source = core.Create(points, spare, suffix);
                var context = $"{core.Name}: active={count}, spare={spare}, suffix={suffix}";
                var copy = core.Duplicate(source, "Copy");
                Check(core, source, copy, points, context);
                Check(core, copy, core.Duplicate(copy, "Copy"), points, context + " repeated copy");
                CheckMutations(core, source, copy, points, context);
            }
        }

        [Test]
        public void ClippedCopiesKeepActiveGeometry()
        {
            foreach (var core in Cores())
            for (var axis = 0; axis < core.Dimension; axis++)
            foreach (var shift in new[] { 0.0, 10.0, -16.0, 0.5 })
            foreach (var kind in new[] { "retained", "crossing", "rejected" })
            {
                var origin = new[] { shift, shift - 8, shift + 4 }.Take(core.Dimension).ToArray();
                var points = PlanePoints(core.Dimension, axis, origin, new[] { 2.0, 0.0, -2.0, 2.0, -2.0, -2.0 });
                var source = core.Create(points, 0, 0);
                var planePoint = (double[])origin.Clone(); planePoint[axis] += kind == "retained" ? -3 : kind == "rejected" ? 3 : 0;
                var clipped = core.Clip(source, axis, planePoint);
                var expected = kind == "retained" ? points : kind == "rejected" ? Array.Empty<double>()
                    : PlanePoints(core.Dimension, axis, origin, new[] { 2.0, 0.0, 0.0, 1.0, 0.0, -1.0 });
                var context = $"{core.Name}: {kind}, axis={axis}, shift={shift}";
                Assert.That(core.Points(clipped), Is.EqualTo(expected), context + " analytic clipping fixture");
                Check(core, clipped, core.Duplicate(clipped, "Copy"), expected, context);
                Assert.That(core.Points(source), Is.EqualTo(points), context + " clipping source unchanged");
            }
        }

        [Test]
        public void ExactSizedAndAlternativeCopiesKeepTheirContracts()
        {
            foreach (var core in Cores())
            {
                foreach (var count in new[] { 0, 1, 3, 8, 32 })
                foreach (var mode in new[] { "Copy", "Constructor", "Map" })
                {
                    var points = Prefix(core.Dimension, count); var source = core.Create(points, 0, 0);
                    var copy = core.Duplicate(source, mode); var context = $"{core.Name}: exact count={count}, {mode}";
                    Check(core, source, copy, points, context); CheckMutations(core, source, copy, points, context);
                }
                foreach (var count in new[] { 0, 3, 8 })
                foreach (var mode in new[] { "Constructor", "Map" })
                {
                    var points = Prefix(core.Dimension, count); var source = core.Create(points, 9, double.NaN);
                    var copy = core.Duplicate(source, mode); var context = $"{core.Name}: padded count={count}, {mode}";
                    Check(core, source, copy, points, context); CheckMutations(core, source, copy, points, context);
                }
                foreach (var source in core.NullBacked())
                {
                    var error = Assert.Throws<TargetInvocationException>(() => core.Duplicate(source, "Copy"), core.Name + " null-backed Copy");
                    Assert.That(error.InnerException, Is.TypeOf<NullReferenceException>(), core.Name + " existing null exception");
                }
            }
        }

        private static double[] Prefix(int dimension, int count) =>
            Enumerable.Range(0, count).SelectMany(i => dimension == 2
                ? new[] { i + 2.0, (i + 1.0) * (i + 1.0) + 3 }
                : new[] { i + 2.0, (i + 1.0) * (i + 1.0) + 3, 7.0 }).ToArray();
        private static double[] PlanePoints(int dimension, int axis, double[] origin, double[] uv)
        {
            var points = new double[uv.Length / 2 * dimension];
            for (var i = 0; i < uv.Length / 2; i++)
            for (var k = 0; k < dimension; k++)
                points[i * dimension + k] = origin[k] + (k == axis ? uv[2 * i] : k == (axis + 1) % dimension ? uv[2 * i + 1] : 0);
            return points;
        }
        private static void Check(Core core, object source, object copy, double[] expected, string context)
        {
            var count = expected.Length / core.Dimension;
            Assert.AreEqual(count, core.Count(copy), context + " active count");
            Assert.That(core.Points(copy), Is.EqualTo(expected), context + " ordered active coordinates");
            Assert.That(core.Points(source), Is.EqualTo(expected), context + " source coordinates");
            Assert.AreEqual(count, core.Storage(copy).Length, context + " exact-sized copied storage");
            Assert.AreNotSame(core.Storage(source), core.Storage(copy), context + " independent arrays");
            Assert.That(core.Bounds(copy), Is.EqualTo(core.Bounds(source)), context + " bounds preserved");
            if (count == 0) { Assert.IsTrue(core.InvalidBounds(copy), context + " empty bounds"); return; }
            var bounds = Enumerable.Range(0, core.Dimension).Select(k => Enumerable.Range(0, count).Min(i => expected[i * core.Dimension + k]))
                .Concat(Enumerable.Range(0, core.Dimension).Select(k => Enumerable.Range(0, count).Max(i => expected[i * core.Dimension + k]))).ToArray();
            Assert.That(core.Bounds(copy), Is.EqualTo(bounds), context + " independent prefix bounds");
            if (count < 3) return;
            var area = core.Area(source);
            Assert.AreEqual(area, core.Area(copy), context + " area preserved");
            var normal = new double[3];
            for (var i = 0; i < count; i++)
            {
                var j = (i + 1) % count;
                var a = expected.Skip(i * core.Dimension).Take(core.Dimension).Concat(new[] { 0.0 }).Take(3).ToArray();
                var b = expected.Skip(j * core.Dimension).Take(core.Dimension).Concat(new[] { 0.0 }).Take(3).ToArray();
                normal[0] += a[1] * b[2] - a[2] * b[1]; normal[1] += a[2] * b[0] - a[0] * b[2]; normal[2] += a[0] * b[1] - a[1] * b[0];
            }
            var analytic = 0.5 * Math.Sqrt(normal.Sum(x => x * x));
            Assert.That(Math.Abs(area - analytic), Is.LessThanOrEqualTo(8 * core.UnitRoundoff * analytic), context + " independent planar area");
        }
        private static void CheckMutations(Core core, object source, object copy, double[] expected, string context)
        {
            if (expected.Length == 0) return;
            var replacement = Enumerable.Range(0, core.Dimension).Select(i => 111.0 + i).ToArray();
            core.Storage(copy).SetValue(core.Vector(replacement), 0);
            Assert.That(core.Points(source), Is.EqualTo(expected), context + " copied mutation cannot change source");
            var copied = core.Points(copy);
            core.Storage(source).SetValue(core.Vector(replacement.Select(x => -x).ToArray()), 0);
            Assert.That(core.Points(copy), Is.EqualTo(copied), context + " source mutation cannot change copy");
        }

        private sealed class Core
        {
            public readonly string Name;
            public readonly int Dimension;
            public readonly double UnitRoundoff;
            private readonly Type vector, polygon, plane, extensions;
            private readonly ConstructorInfo vectorCtor, polygonCtor;
            private readonly MethodInfo copy, readPoints;
            private readonly FieldInfo storage;
            private readonly Delegate identity;
            private readonly bool wide;
            public Core(Assembly assembly, string target, int dimension, bool wide)
            {
                Dimension = dimension; this.wide = wide; UnitRoundoff = Math.ScaleB(1, wide ? -53 : -24);
                var suffix = dimension + (wide ? "d" : "f"); Name = target + "/Polygon" + suffix;
                vector = assembly.GetType("Aardvark.Base.V" + suffix, true); polygon = assembly.GetType("Aardvark.Base.Polygon" + suffix, true);
                plane = assembly.GetType("Aardvark.Base.Plane" + suffix, true); extensions = assembly.GetType("Aardvark.Base.Polygon" + suffix + "Extensions", true);
                vectorCtor = vector.GetConstructor(Enumerable.Repeat(wide ? typeof(double) : typeof(float), dimension).ToArray());
                polygonCtor = polygon.GetConstructor(new[] { vector.MakeArrayType(), typeof(int) });
                copy = polygon.GetMethod("Copy", Type.EmptyTypes); readPoints = polygon.GetMethod("GetPointArray", Type.EmptyTypes);
                storage = polygon.GetField("m_pointArray", BindingFlags.Instance | BindingFlags.NonPublic);
                var parameter = Expression.Parameter(vector); identity = Expression.Lambda(Expression.GetFuncType(vector, vector), parameter, parameter).Compile();
            }
            public object Vector(double[] values) => vectorCtor.Invoke(values.Select(x => wide ? (object)x : (object)(float)x).ToArray());
            private double[] Coordinates(object value) => "XYZ".Take(Dimension).Select(c => Convert.ToDouble(vector.GetField(c.ToString()).GetValue(value))).ToArray();
            public object Create(double[] points, int spare, double suffix)
            {
                var count = points.Length / Dimension; var array = Array.CreateInstance(vector, count + spare);
                for (var i = 0; i < array.Length; i++) array.SetValue(Vector(i < count ? points.Skip(i * Dimension).Take(Dimension).ToArray()
                    : Enumerable.Repeat(suffix, Dimension).ToArray()), i);
                return polygonCtor.Invoke(new object[] { array, count });
            }
            public Array Storage(object value) => (Array)storage.GetValue(value);
            public int Count(object value) => (int)polygon.GetProperty("PointCount").GetValue(value);
            public double[] Points(object value) => ((Array)readPoints.Invoke(value, null)).Cast<object>().SelectMany(Coordinates).ToArray();
            public object Duplicate(object value, string mode) => mode == "Copy" ? copy.Invoke(value, null) : mode == "Constructor"
                ? polygon.GetConstructor(new[] { polygon }).Invoke(new[] { value }) : polygon.GetMethod("Map").Invoke(value, new object[] { identity });
            private object Box(object value) => polygon.GetProperty("BoundingBox" + Dimension + (wide ? "d" : "f")).GetValue(value);
            public double[] Bounds(object value) { var box = Box(value); return Coordinates(box.GetType().GetField("Min").GetValue(box)).Concat(Coordinates(box.GetType().GetField("Max").GetValue(box))).ToArray(); }
            public bool InvalidBounds(object value) { var box = Box(value); return (bool)box.GetType().GetProperty("IsInvalid").GetValue(box); }
            public double Area(object value) => Convert.ToDouble(extensions.GetMethod("ComputeArea", new[] { polygon }).Invoke(null, new[] { value }));
            public object Clip(object value, int axis, double[] point)
            {
                var normal = new double[Dimension]; normal[axis] = 1;
                var p = plane.GetConstructor(new[] { vector, vector }).Invoke(new[] { Vector(normal), Vector(point) });
                return extensions.GetMethod("ConvexClipped", new[] { polygon, plane, wide ? typeof(double) : typeof(float) })
                    .Invoke(null, new[] { value, p, wide ? (object)0.0 : (object)0.0f });
            }
            public IEnumerable<object> NullBacked()
            {
                yield return Activator.CreateInstance(polygon);
                yield return polygon.GetField("Invalid").GetValue(null);
                yield return polygonCtor.Invoke(new object[] { null, 3 });
            }
        }
        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Polygon2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path); var assembly = Assembly.LoadFile(path);
                foreach (var dimension in new[] { 2, 3 })
                foreach (var wide in new[] { false, true }) yield return new Core(assembly, target, dimension, wide);
            }
        }
    }
}

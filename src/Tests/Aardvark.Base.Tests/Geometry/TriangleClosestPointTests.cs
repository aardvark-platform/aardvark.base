using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class TriangleClosestPointTests
    {
        private static readonly int[][] Orders =
        {
            new[] { 0, 1, 2 }, new[] { 0, 2, 1 }, new[] { 1, 0, 2 },
            new[] { 1, 2, 0 }, new[] { 2, 0, 1 }, new[] { 2, 1, 0 }
        };
        private static readonly double[] Parameters = { -2, -0.25, 0, 0.25, 0.5, 0.75, 1, 1.25, 3 };

        [Test]
        public void RepeatedVerticesUseTheRemainingClosedSegment()
        {
            foreach (var core in Cores())
            foreach (var origin in Origins(core))
            foreach (var direction in Directions(core.Dimension))
            foreach (var perpendicular in Perpendiculars(direction, core.Dimension))
            foreach (var scale in new[] { 0.125, 1.0, 8.0 })
            foreach (var t in Parameters)
            foreach (var offset in new[] { 0.0, -0.5, 0.5 })
            {
                var d = Multiply(direction, scale);
                var n = Multiply(perpendicular, scale * offset);
                var end = Add(origin, d);
                var query = Add(origin, Add(Multiply(d, t), n));
                var clamped = Math.Max(0, Math.Min(1, t));
                var expected = Add(origin, Multiply(d, clamped));
                var residual = Add(Multiply(d, t - clamped), n);
                foreach (var repeated in new[] { origin, end })
                {
                    var vertices = new[] { repeated, repeated, ReferenceEquals(repeated, origin) ? end : origin };
                    foreach (var order in new[] { new[] { 0, 1, 2 }, new[] { 0, 2, 1 }, new[] { 2, 0, 1 } })
                        Check(core, order.Select(i => vertices[i]).ToArray(), query, expected, residual, 0,
                            $"segment t={t:R}, offset={offset:R}, scale={scale:R}, order={string.Join(",", order)}");
                }
            }
        }

        [Test]
        public void CoincidentVerticesRemainThePoint()
        {
            foreach (var core in Cores())
            foreach (var point in Origins(core))
            foreach (var scale in new[] { 0.125, 1.0, 8.0 })
            foreach (var rawOffset in new[] { new[] { 0.0, 0, 0 }, new[] { 1.0, 0, 0 }, new[] { -1.0, 2, -3 }, new[] { 0.25, -0.5, 1.5 }, new[] { 16.0, -8, 4 } })
            {
                var residual = Multiply(rawOffset, scale);
                if (core.Dimension == 2) residual[2] = 0;
                Check(core, new[] { point, point, point }, Add(point, residual), point, residual, 0, "coincident vertices");
            }
        }

        [Test]
        public void OrdinaryAndCollinearTrianglesKeepAnalyticResults()
        {
            var regions = new[]
            {
                new[] { -1.0, -1, 0, 0 }, new[] { 6.0, -1, 4, 0 }, new[] { -1.0, 6, 0, 4 },
                new[] { 2.0, -1, 2, 0 }, new[] { -1.0, 2, 0, 2 }, new[] { 3.0, 3, 2, 2 },
                new[] { 1.0, 1, 1, 1 }, new[] { 1.5, 1.5, 1.5, 1.5 },
                new[] { 2.0, 0, 2, 0 }, new[] { 0.0, 2, 0, 2 }, new[] { 2.0, 2, 2, 2 },
                new[] { 0.0, 0, 0, 0 }, new[] { 4.0, 0, 4, 0 }, new[] { 0.0, 4, 0, 4 }
            };
            foreach (var core in Cores())
            {
                foreach (var frame in Frames(core.Dimension))
                foreach (var origin in Origins(core))
                foreach (var scale in new[] { 0.125, 1.0, 8.0 })
                foreach (var region in regions)
                foreach (var height in core.Dimension == 2 ? new[] { 0.0 } : new[] { -0.5, 0.0, 0.5 })
                {
                    var u = Multiply(frame[0], scale);
                    var v = Multiply(frame[1], scale);
                    var normal = Multiply(Cross(frame[0], frame[1]), scale * height);
                    var vertices = new[] { origin, Add(origin, Multiply(u, 4)), Add(origin, Multiply(v, 4)) };
                    var expected = Add(origin, Add(Multiply(u, region[2]), Multiply(v, region[3])));
                    var residual = Add(Add(Multiply(u, region[0] - region[2]), Multiply(v, region[1] - region[3])), normal);
                    var query = Add(origin, Add(Add(Multiply(u, region[0]), Multiply(v, region[1])), normal));
                    var pointAllowance = 8 * core.UnitRoundoff * scale * Math.Max(4 * Norm(frame[0]), 4 * Norm(frame[1]));
                    foreach (var order in Orders)
                        Check(core, order.Select(i => vertices[i]).ToArray(), query, expected, residual, pointAllowance,
                            $"ordinary region=({string.Join(",", region)}), height={height:R}, order={string.Join(",", order)}");
                }

                foreach (var direction in Directions(core.Dimension).Where((_, i) => i == 0 || i == (core.Dimension == 2 ? 3 : 5)))
                foreach (var origin in Origins(core).Take(2))
                foreach (var scale in new[] { 0.125, 8.0 })
                foreach (var t in Parameters)
                foreach (var offset in new[] { 0.0, 0.5 })
                {
                    var d = Multiply(direction, scale);
                    var n = Multiply(Perpendiculars(direction, core.Dimension).First(), scale * offset);
                    var vertices = new[] { origin, Add(origin, Multiply(d, 0.25)), Add(origin, d) };
                    var clamped = Math.Max(0, Math.Min(1, t));
                    foreach (var order in Orders)
                        Check(core, order.Select(i => vertices[i]).ToArray(), Add(origin, Add(Multiply(d, t), n)),
                            Add(origin, Multiply(d, clamped)), Add(Multiply(d, t - clamped), n), 0,
                            $"distinct collinear t={t:R}, offset={offset:R}, order={string.Join(",", order)}");
                }
            }
        }

        [Test]
        public void NonrepeatedTinyEdgesKeepExistingRangeLimits()
        {
            foreach (var core in Cores())
            {
                // Squared edge length underflows, but the first vertices differ.
                // This numeric-range limitation is outside the repeated-vertex fix.
                var a = core.Vector(new[] { 0.0, 0, 0 });
                var b = core.Vector(new[] { Math.ScaleB(1, core.Single ? -80 : -600), 0, 0 });
                var c = core.Vector(new[] { 0.0, 1, 0 });
                var triangle = core.Triangle(new[] { a, b, c });
                var results = new[]
                {
                    core.Direct.Invoke(null, new[] { c, a, b, c }),
                    core.PointTriangle.Invoke(null, new[] { c, triangle }),
                    core.TrianglePoint.Invoke(null, new[] { triangle, c })
                };
                foreach (var result in results)
                    Assert.That(core.Coordinates(result).All(double.IsNaN), Is.True, core.Name + " nonrepeated underflow");
                Assert.That(double.IsNaN(Convert.ToDouble(core.Distance.Invoke(null, new[] { a, b, c, c }))),
                    Is.True, core.Name + " nonrepeated underflow distance");
            }
        }

        private static void Check(Core core, double[][] vertices, double[] query, double[] expected, double[] residual,
            double pointAllowance, string label)
        {
            var q = core.Vector(query);
            var points = vertices.Select(core.Vector).ToArray();
            var triangle = core.Triangle(points);
            var results = new[]
            {
                core.Direct.Invoke(null, new[] { q, points[0], points[1], points[2] }),
                core.PointTriangle.Invoke(null, new[] { q, triangle }),
                core.TrianglePoint.Invoke(null, new[] { triangle, q })
            };
            var context = $"{core.Name}: {label}, a=({string.Join(",", vertices[0])}), b=({string.Join(",", vertices[1])}), c=({string.Join(",", vertices[2])}), q=({string.Join(",", query)})";
            var direct = core.Coordinates(results[0]);
            for (var api = 0; api < results.Length; api++)
            {
                var actual = core.Coordinates(results[api]);
                for (var k = 0; k < core.Dimension; k++)
                {
                    Assert.That(double.IsFinite(actual[k]), Is.True, context + $" api={api}, component={k}");
                    if (pointAllowance == 0) Assert.AreEqual(expected[k], actual[k], context + $" exact api={api}, component={k}");
                    else Assert.That(Math.Abs(actual[k] - expected[k]), Is.LessThanOrEqualTo(pointAllowance), context + $" api={api}, component={k}");
                    Assert.AreEqual(direct[k], actual[k], context + " wrapper forwarding");
                }
            }
            var distance = Convert.ToDouble(core.Distance.Invoke(null, new[] { points[0], points[1], points[2], q }));
            var expectedDistance = Norm(residual);
            var distanceAllowance = Math.Sqrt(core.Dimension) * pointAllowance + 2 * core.UnitRoundoff * expectedDistance;
            Assert.That(double.IsFinite(distance), Is.True, context + " distance finite");
            if (expectedDistance == 0 && pointAllowance == 0) Assert.AreEqual(0, distance, context + " exact zero distance");
            else Assert.That(Math.Abs(distance - expectedDistance), Is.LessThanOrEqualTo(distanceAllowance), context + " analytic distance");
        }

        private static IEnumerable<double[]> Origins(Core core)
        {
            var large = Math.ScaleB(1, core.Single ? 16 : 40);
            yield return new[] { 0.0, 0, 0 };
            yield return new[] { 8.0, -16, core.Dimension == 3 ? 32 : 0 };
            yield return new[] { -0.5, 0.25, core.Dimension == 3 ? -1.5 : 0 };
            yield return new[] { large, -large, core.Dimension == 3 ? large : 0 };
        }
        private static IEnumerable<double[]> Directions(int dimension)
        {
            yield return new[] { 2.0, 0, 0 };
            yield return new[] { 0.0, 2, 0 };
            if (dimension == 3) yield return new[] { 0.0, 0, 2 };
            yield return new[] { 2.0, 4, 0 };
            yield return new[] { 4.0, -2, dimension == 3 ? 2 : 0 };
            if (dimension == 3) yield return new[] { 2.0, 4, 4 };
        }
        private static IEnumerable<double[]> Perpendiculars(double[] d, int dimension)
        {
            var n = d[0] == 0 && d[1] == 0 ? new[] { 1.0, 0, 0 } : new[] { -d[1], d[0], 0.0 };
            yield return n;
            if (dimension == 3) yield return Cross(d, n);
        }
        private static IEnumerable<double[][]> Frames(int dimension)
        {
            yield return new[] { new[] { 1.0, 0, 0 }, new[] { 0.0, 1, 0 } };
            if (dimension == 2) yield return new[] { new[] { 3.0, 4, 0 }, new[] { -4.0, 3, 0 } };
            else
            {
                yield return new[] { new[] { 0.0, 1, 0 }, new[] { 0.0, 0, 1 } };
                yield return new[] { new[] { 1.0, 2, 2 }, new[] { 2.0, 1, -2 } };
            }
        }
        private static double[] Add(double[] a, double[] b) => a.Zip(b, (x, y) => x + y).ToArray();
        private static double[] Multiply(double[] a, double scale) => a.Select(x => x * scale).ToArray();
        private static double[] Cross(double[] a, double[] b) => new[] { a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0] };
        private static double Norm(double[] a) => Math.Sqrt(a.Sum(x => x * x));

        private sealed class Core
        {
            public readonly string Name;
            public readonly int Dimension;
            public readonly bool Single;
            public readonly double UnitRoundoff;
            public readonly MethodInfo Direct, PointTriangle, TrianglePoint, Distance;
            private readonly Type vector;
            private readonly ConstructorInfo vectorCtor, triangleCtor;

            public Core(Assembly assembly, string target, int dimension, bool single)
            {
                Dimension = dimension;
                Single = single;
                UnitRoundoff = Math.ScaleB(1, single ? -24 : -53);
                var suffix = dimension.ToString() + (single ? "f" : "d");
                Name = target + "/Triangle" + suffix;
                vector = assembly.GetType("Aardvark.Base.V" + suffix, true);
                var triangle = assembly.GetType("Aardvark.Base.Triangle" + suffix, true);
                var geometry = assembly.GetType("Aardvark.Base.GeometryFun", true);
                var scalar = single ? typeof(float) : typeof(double);
                vectorCtor = vector.GetConstructor(Enumerable.Repeat(scalar, dimension).ToArray());
                triangleCtor = triangle.GetConstructor(new[] { vector, vector, vector });
                Direct = geometry.GetMethod("GetClosestPointOnTriangle", Enumerable.Repeat(vector, 4).ToArray());
                PointTriangle = geometry.GetMethod("GetClosestPointOn", new[] { vector, triangle });
                TrianglePoint = geometry.GetMethod("GetClosestPointOn", new[] { triangle, vector });
                Distance = assembly.GetType("Aardvark.Base.Triangle", true).GetMethod("Distance", Enumerable.Repeat(vector, 4).ToArray());
            }
            public object Vector(double[] coordinates)
            {
                foreach (var value in coordinates.Take(Dimension))
                    Assert.AreEqual(value, Single ? (double)(float)value : value, Name + " exactly representable input");
                return vectorCtor.Invoke(coordinates.Take(Dimension).Select(x => Single ? (object)(float)x : (object)x).ToArray());
            }
            public object Triangle(object[] points) => triangleCtor.Invoke(points);
            public double[] Coordinates(object value) => "XYZ".Take(Dimension)
                .Select(c => Convert.ToDouble(vector.GetField(c.ToString()).GetValue(value))).ToArray();
        }
        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(V3d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                var expected = target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0";
                Assert.AreEqual(expected, assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                foreach (var dimension in new[] { 2, 3 })
                foreach (var single in new[] { true, false })
                    yield return new Core(assembly, target, dimension, single);
            }
        }
    }
}

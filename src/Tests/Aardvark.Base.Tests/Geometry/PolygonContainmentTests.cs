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
    public class PolygonContainmentTests
    {
        private static readonly V2d[][] Shapes =
        {
            new[] { new V2d(0,0), new V2d(4,0), new V2d(4,4), new V2d(0,4) },
            new[] { new V2d(0,0), new V2d(8,0), new V2d(0,4) },
            new[] { new V2d(0,0), new V2d(4,0), new V2d(8,4), new V2d(8,8), new V2d(4,12), new V2d(0,8) },
            new[] { new V2d(0,0), new V2d(8,0), new V2d(8,4), new V2d(4,4), new V2d(4,8), new V2d(0,8) },
            new[] { new V2d(0,0), new V2d(12,0), new V2d(12,8), new V2d(8,8), new V2d(8,4), new V2d(4,4), new V2d(4,8), new V2d(0,8) },
            new[] { new V2d(0,0), new V2d(2,0), new V2d(4,0), new V2d(4,2), new V2d(4,4), new V2d(2,4), new V2d(0,4), new V2d(0,2) },
            new[] { new V2d(0,0), new V2d(4,0), new V2d(4,0), new V2d(4,4), new V2d(0,4), new V2d(0,0) }
        };

        [Test]
        public void ClosedPolygonEdgesAndVerticesAreContained()
        {
            foreach (var core in Cores())
            foreach (var shape in Shapes)
            foreach (var scale in new[] { .25, 1, 4 })
            foreach (var origin in Origins(core.Wide))
            {
                var vertices = shape.Select(p => origin + scale * p).ToArray();
                var queries = new List<V2d>();
                for (var i = 0; i < vertices.Length; i++)
                foreach (var t in new[] { 0.0, .25, .5, .75 })
                    queries.Add((1 - t) * vertices[i] + t * vertices[(i + 1) % vertices.Length]);
                foreach (var query in queries) Assert.AreEqual(0, ExactClassification(vertices, query), "Constructed actual-edge point");
                foreach (var order in Orders(vertices))
                {
                    var polygon = core.Create(order.Vertices);
                    foreach (var query in queries)
                        Check(core, polygon, query, 0, order.Ccw, $"edge/vertex, count={vertices.Length}, scale={scale}, origin={origin}, start={order.Start}");
                }
            }
        }

        [Test]
        public void InteriorExteriorAndFanDiagonalsPreserveWindingSelection()
        {
            foreach (var core in Cores())
            foreach (var shape in Shapes)
            foreach (var scale in new[] { .25, 1, 4 })
            foreach (var origin in Origins(core.Wide))
            {
                var vertices = shape.Select(p => origin + scale * p).ToArray();
                var queries = new List<V2d>();
                for (var i = 2; i < shape.Length - 1; i++)
                foreach (var t in new[] { .25, .5, .75 }) queries.Add(origin + scale * ((1 - t) * shape[0] + t * shape[i]));
                var random = new Random(137119 + shape.Length);
                for (var i = 0; i < 64; i++) queries.Add(origin + scale * new V2d(random.Next(-8, 57) * .25, random.Next(-8, 57) * .25));
                var expected = queries.Select(p => ExactClassification(vertices, p)).ToArray();
                foreach (var order in Orders(vertices))
                {
                    var polygon = core.Create(order.Vertices);
                    for (var i = 0; i < queries.Count; i++)
                        if (expected[i] != 0) // Actual edges are covered separately; fan diagonals are not polygon edges.
                            Check(core, polygon, queries[i], expected[i], order.Ccw, $"interior/exterior/fan, count={vertices.Length}, scale={scale}, origin={origin}, start={order.Start}, query={i}");
                }
            }
        }

        [Test]
        public void AdjacentRepresentablePointsDoNotExpandEdges()
        {
            foreach (var core in Cores())
            foreach (var shape in new[] { Shapes[0], Shapes[4] })
            {
                var vertices = shape.Select(p => new V2d(16,32) + p).ToArray();
                var queries = new List<V2d>();
                for (var i = 0; i < vertices.Length; i++)
                {
                    var a = vertices[i]; var b = vertices[(i + 1) % vertices.Length]; var mid = .5 * (a + b);
                    double Next(double x, bool up) => core.Wide ? (up ? Math.BitIncrement(x) : Math.BitDecrement(x))
                        : (up ? float.BitIncrement((float)x) : float.BitDecrement((float)x));
                    foreach (var up in new[] { false, true })
                        queries.Add(a.Y == b.Y ? new V2d(mid.X, Next(mid.Y, up)) : new V2d(Next(mid.X, up), mid.Y));
                }
                foreach (var order in Orders(vertices))
                {
                    var polygon = core.Create(order.Vertices);
                    foreach (var query in queries)
                    {
                        var expected = ExactClassification(vertices, query);
                        Assert.AreNotEqual(0, expected, "Adjacent representable point is not on an edge");
                        Check(core, polygon, query, expected, order.Ccw, "one-ULP inside/outside, start=" + order.Start);
                    }
                }
            }
        }

        [Test]
        public void ShortPolygonsAndRepeatedVerticesKeepClosedRules()
        {
            foreach (var core in Cores())
            {
                for (var count = 0; count < 3; count++)
                {
                    var vertices = Shapes[0].Take(count).ToArray(); var polygon = core.Create(vertices, true);
                    foreach (var query in new[] { V2d.Zero, new V2d(2,0), new V2d(2,2), new V2d(8,8) })
                        Check(core, polygon, query, -1, true, "fewer than three active vertices, count=" + count);
                }
                var padded = core.Create(Shapes[0], true);
                Check(core, padded, new V2d(2,0), 0, true, "poisoned spare storage is not an edge");
                Check(core, padded, new V2d(2,2), 1, true, "active-prefix interior");
                foreach (var vertices in new[]
                {
                    new[] { V2d.Zero, new V2d(4,0), new V2d(8,0) },
                    new[] { new V2d(2,3), new V2d(2,3), new V2d(2,3) }
                })
                foreach (var order in Orders(vertices))
                {
                    var polygon = core.Create(order.Vertices);
                    Check(core, polygon, vertices[0], 0, order.Ccw, "repeated/collinear boundary");
                    Check(core, polygon, new V2d(20,20), -1, order.Ccw, "repeated/collinear exterior");
                }
                var twice = Shapes[0].Concat(Shapes[0]).ToArray();
                foreach (var order in Orders(twice))
                    Check(core, core.Create(order.Vertices), new V2d(2,2), 1, order.Ccw, "signed winding magnitude two, not even-odd");
            }
        }

        private static V2d[] Origins(bool wide) => new[] { V2d.Zero, new V2d(16,-32), new V2d(-.5,.25), new V2d(wide ? Math.ScaleB(1,40) : Math.ScaleB(1,16), wide ? -Math.ScaleB(1,40) : -Math.ScaleB(1,16)) };
        private static IEnumerable<(V2d[] Vertices, bool Ccw, int Start)> Orders(V2d[] vertices)
        {
            foreach (var reverse in new[] { false, true })
            for (var start = 0; start < vertices.Length; start++)
                yield return (Enumerable.Range(0,vertices.Length).Select(i => vertices[reverse ? (start - i + vertices.Length) % vertices.Length : (start + i) % vertices.Length]).ToArray(), !reverse, start);
        }

        private static int ExactClassification(V2d[] vertices, V2d point)
        {
            // Exact dyadic coordinates and independent even-odd horizontal-ray membership.
            // Simple fixtures use this oracle; the twice-traversed contour has an explicit winding expectation.
            BigInteger Coordinate(double value)
            {
                var scaled = Math.ScaleB(value,54);
                Assert.AreEqual(Math.Truncate(scaled), scaled, "Oracle requires exactly representable scaled integers");
                return new BigInteger(scaled);
            }
            var x = Coordinate(point.X); var y = Coordinate(point.Y); var inside = false;
            for (var i = 0; i < vertices.Length; i++)
            {
                var a = vertices[i]; var b = vertices[(i + 1) % vertices.Length];
                var ax = Coordinate(a.X); var ay = Coordinate(a.Y); var bx = Coordinate(b.X); var by = Coordinate(b.Y);
                if ((bx-ax)*(y-ay) == (by-ay)*(x-ax) && x >= BigInteger.Min(ax,bx) && x <= BigInteger.Max(ax,bx) && y >= BigInteger.Min(ay,by) && y <= BigInteger.Max(ay,by)) return 0;
                if ((ay > y) == (by > y)) continue;
                var deltaY = by-ay; var intersectionNumerator = ax*deltaY + (y-ay)*(bx-ax);
                if (deltaY > 0 ? x*deltaY < intersectionNumerator : x*deltaY > intersectionNumerator) inside = !inside;
            }
            return inside ? 1 : -1;
        }

        private static void Check(Core core, object polygon, V2d point, int classification, bool ccw, string label)
        {
            var context = $"{core.Name}: {label}, query={point}, CCW={ccw}, classification={classification}";
            var boundary = classification == 0; var inside = classification > 0;
            Assert.AreEqual(boundary || inside && ccw, core.Contains(polygon,point,null), context + " default CCW");
            foreach (var flag in new[] { false, true })
                Assert.AreEqual(boundary || inside && flag == ccw, core.Contains(polygon,point,flag), context + ", explicit=" + flag);
        }

        private sealed class Core
        {
            public readonly bool Wide;
            public readonly string Name;
            private readonly Type vectorType;
            private readonly ConstructorInfo vector, polygon;
            private readonly MethodInfo direct, oriented;
            public Core(Assembly assembly, bool wide, string target)
            {
                Wide = wide; Name = target + (wide ? "/double" : "/float"); var suffix = wide ? "d" : "f";
                vectorType = assembly.GetType("Aardvark.Base.V2"+suffix,true); var polygonType = assembly.GetType("Aardvark.Base.Polygon2"+suffix,true);
                Assert.AreSame(assembly,vectorType.Assembly,Name); Assert.AreSame(assembly,polygonType.Assembly,Name);
                vector = vectorType.GetConstructor(new[] { wide ? typeof(double) : typeof(float), wide ? typeof(double) : typeof(float) });
                polygon = polygonType.GetConstructor(new[] { vectorType.MakeArrayType(), typeof(int) });
                var geometry = assembly.GetType("Aardvark.Base.GeometryFun",true);
                direct = geometry.GetMethod("Contains",new[] { polygonType,vectorType }); oriented = geometry.GetMethod("Contains",new[] { polygonType,vectorType,typeof(bool) });
                Assert.IsNotNull(vector,Name); Assert.IsNotNull(polygon,Name); Assert.IsNotNull(direct,Name); Assert.IsNotNull(oriented,Name);
            }
            private object Scalar(double value) => Wide ? (object)value : (float)value;
            private object Point(V2d p)
            {
                if (!Wide && p.IsFinite) Assert.AreEqual(p,new V2d((float)p.X,(float)p.Y),Name+" exactly representable input");
                return vector.Invoke(new[] { Scalar(p.X),Scalar(p.Y) });
            }
            public object Create(V2d[] vertices, bool padded = false)
            {
                var array = Array.CreateInstance(vectorType,vertices.Length+(padded?3:0));
                for (var i=0;i<array.Length;i++) array.SetValue(Point(i<vertices.Length?vertices[i]:V2d.NaN),i);
                return polygon.Invoke(new object[] { array,vertices.Length });
            }
            public bool Contains(object poly,V2d point,bool? flag) => (bool)(flag.HasValue ? oriented.Invoke(null,new[] { poly,Point(point),(object)flag.Value }) : direct.Invoke(null,new[] { poly,Point(point) }));
        }
        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0","netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Polygon2d).Assembly.Location),"..",target,"Aardvark.Base.dll"));
                Assert.That(File.Exists(path),Is.True,"Required sibling core: "+path); var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target=="net10.0"?".NETCoreApp,Version=v10.0":".NETStandard,Version=v2.0",assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,path);
                yield return new Core(assembly,false,target); yield return new Core(assembly,true,target);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Line2ProjectionTests
    {
        [Test]
        public void TranslatedProjectionsMatchAnalyticParameters()
        {
            foreach (var core in Cores())
            foreach (var c in Cases(core.Wide))
            {
                var result = core.Query(c.Args, 0);
                AssertPoint(c.X, c.Y, result, core.Name + c.Name);
            }
        }

        [Test]
        public void DistancesAndThresholdsInheritTranslatedProjections()
        {
            foreach (var core in Cores())
            foreach (var c in Cases(core.Wide))
            {
                var context = core.Name + c.Name;
                var large = c.Distance == 0 ? 0 : Round(2 * c.Distance, core.Wide);
                var result = core.Query(c.Args, large);
                AssertDistance(c.Distance, result[2], core.Wide, context);
                Assert.AreEqual(1, result[3], context + " inclusive/generous threshold");
                if (c.Distance > 0)
                {
                    Assert.AreEqual(0, core.Query(c.Args, Round(c.Distance / 2, core.Wide))[3],
                        context + " below distance");
                    // Integer distances are exactly representable, so test the inclusive boundary
                    // and its immediate predecessor without a tolerance-dependent decision.
                    if (c.Distance == Math.Truncate(c.Distance))
                    {
                        Assert.AreEqual(1, core.Query(c.Args, c.Distance)[3], context + " equal distance");
                        var below = core.Wide ? Math.BitDecrement(c.Distance)
                            : MathF.BitDecrement((float)c.Distance);
                        Assert.AreEqual(0, core.Query(c.Args, below)[3], context + " adjacent threshold");
                    }
                }
            }
        }

        [Test]
        public void TranslatedCollinearPolylinesKeepOnlyEndpoints()
        {
            foreach (var core in Cores())
            foreach (var s in Segments(core.Wide))
            foreach (var reverse in new[] { false, true })
            {
                var points = new[] { s.X, s.Y, s.X + s.Vx / 2, s.Y + s.Vy / 2,
                    s.X + s.Vx, s.Y + s.Vy };
                if (reverse) points = new[] { points[4], points[5], points[2], points[3], points[0], points[1] };
                var context = $"{core.Name}: origin=({s.X:R},{s.Y:R}), v=({s.Vx},{s.Vy}), reverse={reverse}";
                Assert.That(core.Simplify(points), Is.EqualTo(new[] { 0, 2 }), context);
            }
        }

        [Test]
        public void CoincidentAndTinySegmentsRetainTheExistingGuard()
        {
            foreach (var core in Cores())
            {
                foreach (var s in Segments(core.Wide))
                {
                    var args = new[] { s.X, s.Y, s.X, s.Y, s.X + 3, s.Y + 4 };
                    var context = $"{core.Name}: coincident ({s.X:R},{s.Y:R})";
                    var result = core.Query(args, 5);
                    AssertPoint(s.X, s.Y, result, context);
                    AssertDistance(5, result[2], core.Wide, context);
                    Assert.AreEqual(1, result[3], context + " inclusive distance");
                }
                foreach (var origin in new[] { 0.0, 1.0, -1.0 })
                foreach (var exponent in core.Wide ? new[] { -26, -24 } : new[] { -11, -10 })
                foreach (var reverse in new[] { false, true })
                {
                    var v = Math.ScaleB(1, exponent);
                    var args = new[] { origin, origin, origin + v, origin, origin + v / 2, origin };
                    if (reverse) (args[0], args[2]) = (args[2], args[0]);
                    var tiny = exponent == (core.Wide ? -26 : -11);
                    var expected = tiny ? args[0] : args[4];
                    var context = $"{core.Name}: origin={origin}, exponent={exponent}, reverse={reverse}";
                    var result = core.Query(args, 0);
                    AssertPoint(expected, origin, result, context);
                    AssertDistance(tiny ? v / 2 : 0, result[2], core.Wide, context);
                    Assert.AreEqual(tiny ? 0 : 1, result[3], context + " zero threshold");
                }
            }
        }

        private static void AssertPoint(double x, double y, double[] actual, string context)
        {
            Assert.AreEqual(x, actual[0], context + " closest X");
            Assert.AreEqual(y, actual[1], context + " closest Y");
        }

        private static void AssertDistance(double expected, double actual, bool wide, string context)
        {
            var u = Math.ScaleB(1, wide ? -53 : -24);
            Assert.AreEqual(expected, actual, 2 * u * expected, context + " distance");
        }

        private static double Round(double value, bool wide) => wide ? value : (float)value;

        private static IEnumerable<(double X, double Y, double Vx, double Vy)> Segments(bool wide)
        {
            var o = Math.ScaleB(1, wide ? 50 : 20);
            var shapes = new[] { wide ? (1.0, 5.0) : (3.0, 6.0), (3.0, 4.0), (4.0, 0.0), (0.0, 4.0) };
            foreach (var origin in new[] { (o, o), (-o, -o), (o, -o), (-o, o), (0.0, o),
                (o, 0.0), (0.0, 0.0), (0.25, -0.5) })
            foreach (var shape in shapes)
            foreach (var sx in new[] { -1, 1 })
            foreach (var sy in new[] { -1, 1 })
                yield return (origin.Item1, origin.Item2, sx * shape.Item1, sy * shape.Item2);
        }

        private static IEnumerable<(double[] Args, double X, double Y, double Distance, string Name)> Cases(bool wide)
        {
            foreach (var s in Segments(wide))
            foreach (var t in new[] { -0.5, 0, 0.25, 0.5, 0.75, 1, 1.5 })
            foreach (var n in new[] { -1.0, -0.5, 0.0, 0.5, 1.0 })
            foreach (var reverse in new[] { false, true })
            {
                // Construct the query from a known tangent parameter and an orthogonal offset.
                // Expectations never subtract world-space dot products or call production geometry.
                var px = s.X + t * s.Vx - n * s.Vy;
                var py = s.Y + t * s.Vy + n * s.Vx;
                var clamped = Math.Clamp(t, 0, 1);
                var x = s.X + clamped * s.Vx;
                var y = s.Y + clamped * s.Vy;
                var dx = (t - clamped) * s.Vx - n * s.Vy;
                var dy = (t - clamped) * s.Vy + n * s.Vx;
                var args = new[] { s.X, s.Y, s.X + s.Vx, s.Y + s.Vy, px, py };
                if (reverse) (args[0], args[1], args[2], args[3]) = (args[2], args[3], args[0], args[1]);
                var context = $": origin=({s.X:R},{s.Y:R}), v=({s.Vx},{s.Vy}), t={t}, n={n}, reverse={reverse}";
                // These dyadic fixtures retain every endpoint/query coordinate at both precisions.
                foreach (var value in args) Assert.AreEqual(value, Round(value, wide), context + " representability");
                yield return (args, x, y, Math.Sqrt(dx * dx + dy * dy), context);
            }
        }

        private sealed class Core
        {
            public string Name;
            public bool Wide;
            public Func<double[], double, double[]> Query;
            public Func<double[], int[]> Simplify;
            public Core(Assembly assembly, string target, bool wide)
            {
                Name = target + (wide ? "/double" : "/float"); Wide = wide;
                var scalar = wide ? typeof(double) : typeof(float);
                var vector = assembly.GetType("Aardvark.Base.V2" + (wide ? "d" : "f"), true);
                var line = assembly.GetType("Aardvark.Base.Line2" + (wide ? "d" : "f"), true);
                var values = Expression.Parameter(typeof(double[]));
                var maxDist = Expression.Parameter(typeof(double));
                NewExpression Point(int offset) => Expression.New(vector.GetConstructor(new[] { scalar, scalar }),
                    Expression.Convert(Expression.ArrayIndex(values, Expression.Constant(offset)), scalar),
                    Expression.Convert(Expression.ArrayIndex(values, Expression.Constant(offset + 1)), scalar));
                var segment = Expression.Variable(line);
                var point = Expression.Variable(vector);
                var closest = Expression.Variable(vector);
                var body = Expression.Block(new[] { segment, point, closest },
                    Expression.Assign(segment, Expression.New(line.GetConstructor(new[] { vector, vector }), Point(0), Point(2))),
                    Expression.Assign(point, Point(4)),
                    Expression.Assign(closest, Expression.Call(segment, line.GetMethod("GetClosestPointOnLine"), point)),
                    Expression.NewArrayInit(typeof(double),
                        Expression.Convert(Expression.Field(closest, "X"), typeof(double)),
                        Expression.Convert(Expression.Field(closest, "Y"), typeof(double)),
                        Expression.Convert(Expression.Call(segment, line.GetMethod("GetDistanceToLine"), point), typeof(double)),
                        Expression.Condition(Expression.Call(segment, line.GetMethod("IsDistanceToPointSmallerThan"), point,
                            Expression.Convert(maxDist, scalar)), Expression.Constant(1.0), Expression.Constant(0.0))));
                Query = Expression.Lambda<Func<double[], double, double[]>>(body, values, maxDist).Compile();
                var simplify = assembly.GetType("Aardvark.Base.GeometryFun", true).GetMethod("Simplify",
                    new[] { vector.MakeArrayType(), scalar });
                Simplify = Expression.Lambda<Func<double[], int[]>>(Expression.Call(simplify,
                    Expression.NewArrayInit(vector, Point(0), Point(2), Point(4)),
                    Expression.Convert(Expression.Constant(0.1), scalar)), values).Compile();
            }
        }

        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Line2d).Assembly.Location),
                    "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path);
                var assembly = Assembly.LoadFile(path);
                yield return new Core(assembly, target, false);
                yield return new Core(assembly, target, true);
            }
        }
    }
}

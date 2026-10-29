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
    public class RadialClosestPointTests
    {
        [Test]
        public void ExactCentersChoosePositiveXSurfacePoints()
        {
            foreach (var core in Cores())
            {
                var large = Math.ScaleB(1, core.Single ? 20 : 49);
                foreach (var center in new[]
                {
                    core.Point(0, 0, 0), core.Point(4, -8, 16), core.Point(-16, 2, -4),
                    core.Point(.5, -.25, .125), core.Point(large, -large, large), core.Point(-large, large, -large)
                })
                foreach (var radius in new[] { .125, .5, 1, 2, 8, 32 })
                {
                    var expected = core.Coordinates(center); expected[0] = core.Round(expected[0] + radius);
                    var result = Check(core, center, radius, center, expected, "centered surface tie");
                    Assert.AreEqual(radius, result[0] - core.Coordinates(center)[0], core.Name + " representable surface displacement");
                    Assert.AreEqual(radius * radius, result.Zip(core.Coordinates(center), (a, b) => (a - b) * (a - b)).Sum(), core.Name + " surface membership");
                }
                foreach (var radius in new[] { Math.ScaleB(1, core.Single ? -149 : -1074), Math.ScaleB(1, core.Single ? -126 : -1022), core.Single ? (double)float.MaxValue : double.MaxValue })
                {
                    var expected = new double[core.Dimension]; expected[0] = radius;
                    Check(core, core.Point(0, 0, 0), radius, core.Point(0, 0, 0), expected, "finite positive radius boundary");
                }
                var negativeZero = BitConverter.Int64BitsToDouble(long.MinValue);
                for (var mask = 0; mask < 1 << core.Dimension; mask++)
                for (var queryMask = 0; queryMask < 1 << core.Dimension; queryMask++)
                {
                    var center = core.Vector(Enumerable.Range(0, core.Dimension).Select(i => (mask & 1 << i) == 0 ? 0.0 : negativeZero).ToArray());
                    var query = core.Vector(Enumerable.Range(0, core.Dimension).Select(i => (queryMask & 1 << i) == 0 ? 0.0 : negativeZero).ToArray());
                    var expected = core.Coordinates(center); expected[0] = 1;
                    Check(core, center, 1, query, expected, $"zero-sign center mask={mask}, query mask={queryMask}");
                }
            }
        }

        [Test]
        public void OrdinaryQueriesKeepRadialResultsAndSurfaceGeometry()
        {
            foreach (var core in Cores())
            {
                var directions = new List<double[]>();
                for (var axis = 0; axis < core.Dimension; axis++)
                foreach (var sign in new[] { -1, 1 })
                {
                    var direction = new double[core.Dimension]; direction[axis] = 5 * sign; directions.Add(direction);
                }
                for (var axis = 0; axis < core.Dimension; axis++)
                foreach (var sign in new[] { -1, 1 })
                {
                    var direction = new double[core.Dimension]; direction[axis] = 3 * sign; direction[(axis + 1) % core.Dimension] = 4; directions.Add(direction);
                }
                foreach (var center in new[] { core.Point(0, 0, 0), core.Point(4, -8, 16), core.Point(-16, 2, -4) })
                foreach (var direction in directions)
                foreach (var radiusScale in new[] { .25, 1, 4 })
                foreach (var queryScale in new[] { .125, .5, 1, 2, 8 })
                {
                    var c = core.Coordinates(center); var radius = 5 * radiusScale;
                    var query = core.Vector(c.Select((v, i) => v + direction[i] * radiusScale * queryScale).ToArray());
                    var actual = CheckLegacy(core, center, radius, query, $"analytic direction={string.Join(",", direction)}, radiusScale={radiusScale}, queryScale={queryScale}");
                    var unitRoundoff = Math.ScaleB(1, core.Single ? -24 : -53);
                    for (var i = 0; i < core.Dimension; i++)
                        Assert.That(Math.Abs(actual[i] - (c[i] + direction[i] * radiusScale)), Is.LessThanOrEqualTo(8 * unitRoundoff * radius), core.Name + " analytic surface coordinate " + i);
                    var distance = Math.Sqrt(actual.Zip(c, (a, b) => (a - b) * (a - b)).Sum());
                    Assert.That(Math.Abs(distance - radius), Is.LessThanOrEqualTo(16 * unitRoundoff * radius), core.Name + " analytic surface distance");
                }
                var random = new Random(97337 + core.Dimension);
                for (var sample = 0; sample < 4096; sample++)
                {
                    var c = Enumerable.Range(0, core.Dimension).Select(_ => random.Next(-128, 129) / 4.0).ToArray();
                    var delta = Enumerable.Range(0, core.Dimension).Select(_ => random.Next(-256, 257) / 16.0).ToArray();
                    if (delta.All(v => v == 0)) delta[0] = .0625;
                    var query = core.Vector(c.Zip(delta, (a, b) => a + b).ToArray());
                    CheckLegacy(core, core.Vector(c), random.Next(1, 513) / 8.0, query, $"seed={97337 + core.Dimension}, sample={sample}");
                }
            }
        }

        [Test]
        public void TinyNoncentersAndInvalidInputsKeepLegacyResults()
        {
            foreach (var core in Cores())
            {
                var exponents = core.Single ? new[] { -149, -148, -126, -100, -80, -75, -74, -70, -60 } : new[] { -1074, -1073, -1023, -1000, -600, -538, -537, -536, -530 };
                foreach (var exponent in exponents)
                foreach (var sign in new[] { -1, 1 })
                for (var axis = 0; axis < core.Dimension; axis++)
                foreach (var translated in new[] { false, true })
                foreach (var radius in new[] { 0.0, 1.0, Math.ScaleB(1, core.Single ? 100 : 900) })
                {
                    var center = new double[core.Dimension]; if (translated) center[(axis + 1) % core.Dimension] = 16;
                    var query = (double[])center.Clone(); query[axis] = sign * Math.ScaleB(1, exponent);
                    Assert.AreNotEqual(0, core.Round(query[axis]), core.Name + " tiny displacement must remain representable");
                    var actual = CheckLegacy(core, core.Vector(center), radius, core.Vector(query), $"tiny exponent={exponent}, sign={sign}, axis={axis}, translated={translated}");
                    if (exponent <= (core.Single ? -80 : -600))
                        CollectionAssert.AreEqual(center, actual, core.Name + " existing squared-length underflow remains a center result");
                }
                var negativeZero = BitConverter.Int64BitsToDouble(long.MinValue);
                foreach (var radius in new[] { 0.0, negativeZero, -.5, -1, double.NaN, double.PositiveInfinity, double.NegativeInfinity })
                foreach (var center in new[] { core.Point(0, negativeZero, negativeZero), core.Point(4, -8, 16) })
                foreach (var query in new[] { center, core.Point(1, 2, 3), core.Point(-4, 2, -8) })
                    CheckLegacy(core, center, radius, query, "zero/invalid radius");
                foreach (var special in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity })
                for (var axis = 0; axis < core.Dimension; axis++)
                foreach (var radius in new[] { 0.0, -1, 1, double.NaN, double.PositiveInfinity })
                {
                    var invalid = new double[core.Dimension]; invalid[axis] = special;
                    CheckLegacy(core, core.Vector(invalid), radius, core.Vector(invalid), "nonfinite coincident coordinates");
                    CheckLegacy(core, core.Vector(invalid), radius, core.Point(1, 2, 3), "nonfinite center");
                    CheckLegacy(core, core.Point(0, 0, 0), radius, core.Vector(invalid), "nonfinite query");
                }
                var huge = Math.ScaleB(1, core.Single ? 100 : 900);
                CheckLegacy(core, core.Point(0, 0, 0), 1, core.Point(huge, -huge, huge), "existing squared-length overflow");
                CheckLegacy(core, core.Point(huge, -huge, huge), 1, core.Point(0, 0, 0), "existing translated range limit");
            }
        }

        private static double[] CheckLegacy(Core core, object center, double radius, object query, string label)
            => Check(core, center, radius, query, core.Coordinates(core.Legacy(center, radius, query)), label);

        private static double[] Check(Core core, object center, double radius, object query, double[] expected, string label)
        {
            var shape = core.Shape(center, radius);
            var context = $"{core.Name}: {label}, center=({string.Join(",", core.Coordinates(center))}), radius={radius}, query=({string.Join(",", core.Coordinates(query))})";
            double[] result = null;
            foreach (var reverse in new[] { false, true })
            {
                var actual = core.Coordinates(core.Closest(query, shape, reverse));
                Equal(expected, actual, context + " reverse=" + reverse);
                Equal(actual, core.Coordinates(core.Closest(query, shape, reverse)), context + " repeated reverse=" + reverse);
                result = actual;
            }
            return result;
        }

        private static void Equal(double[] expected, double[] actual, string context)
        {
            for (var i = 0; i < expected.Length; i++)
                if (double.IsNaN(expected[i])) Assert.That(double.IsNaN(actual[i]), Is.True, context + " NaN coordinate=" + i);
                else Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected[i]), BitConverter.DoubleToInt64Bits(actual[i]), context + " exact coordinate=" + i);
        }

        private sealed class Core
        {
            public readonly string Name;
            public readonly bool Single;
            public readonly int Dimension;
            private readonly Type vector;
            private readonly ConstructorInfo vectorCtor, shapeCtor;
            private readonly MethodInfo forward, reverse, subtract, multiply, add;
            private readonly PropertyInfo normalized;
            private readonly FieldInfo[] fields;
            public Core(Assembly assembly, string target, bool single, int dimension)
            {
                Single = single; Dimension = dimension;
                var suffix = single ? "f" : "d"; var scalar = single ? typeof(float) : typeof(double);
                vector = assembly.GetType($"Aardvark.Base.V{dimension}{suffix}", true);
                var shape = assembly.GetType("Aardvark.Base." + (dimension == 2 ? "Circle2" : "Sphere3") + suffix, true);
                Name = target + "/" + shape.Name;
                vectorCtor = vector.GetConstructor(Enumerable.Repeat(scalar, dimension).ToArray());
                shapeCtor = shape.GetConstructor(new[] { vector, scalar });
                var geometry = assembly.GetType("Aardvark.Base.GeometryFun", true);
                forward = geometry.GetMethod("GetClosestPointOn", new[] { vector, shape });
                reverse = geometry.GetMethod("GetClosestPointOn", new[] { shape, vector });
                subtract = vector.GetMethod("op_Subtraction", new[] { vector, vector });
                multiply = vector.GetMethod("op_Multiply", new[] { scalar, vector });
                add = vector.GetMethod("op_Addition", new[] { vector, vector });
                normalized = vector.GetProperty("Normalized");
                fields = new[] { "X", "Y", "Z" }.Take(dimension).Select(vector.GetField).ToArray();
            }
            private object Scalar(double value) => Single ? (object)(float)value : (object)value;
            public double Round(double value) => Single ? (double)(float)value : value;
            public object Point(double x, double y, double z) => Vector(new[] { x, y, z }.Take(Dimension).ToArray());
            public object Vector(double[] values) => vectorCtor.Invoke(values.Select(Scalar).ToArray());
            public object Shape(object center, double radius) => shapeCtor.Invoke(new[] { center, Scalar(radius) });
            public double[] Coordinates(object value) => fields.Select(f => Convert.ToDouble(f.GetValue(value))).ToArray();
            public object Closest(object query, object shape, bool swapped) => swapped ? reverse.Invoke(null, new[] { shape, query }) : forward.Invoke(null, new[] { query, shape });
            public object Legacy(object center, double radius, object query)
            {
                var delta = subtract.Invoke(null, new[] { query, center });
                var direction = normalized.GetValue(delta);
                return add.Invoke(null, new[] { center, multiply.Invoke(null, new[] { Scalar(radius), direction }) });
            }
        }

        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(V2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                foreach (var single in new[] { true, false })
                foreach (var dimension in new[] { 2, 3 })
                    yield return new Core(assembly, target, single, dimension);
            }
        }
    }
}

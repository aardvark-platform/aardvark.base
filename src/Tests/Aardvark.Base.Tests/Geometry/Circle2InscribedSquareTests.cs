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
    public class Circle2InscribedSquareTests
    {
        private static IEnumerable<Assembly> Cores()
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Required sibling core: {path}");
            yield return typeof(Circle2d).Assembly;
            yield return Assembly.LoadFile(path);
        }

        // Construct and access the vector, circle and box belonging to this core.
        // Only primitive inputs and endpoint arrays cross assembly identities.
        private static Func<double, double, double, double[]> Square(Assembly core, bool single)
        {
            string suffix = single ? "f" : "d";
            var scalar = single ? typeof(float) : typeof(double);
            var vector = core.GetType("Aardvark.Base.V2" + suffix, true)!;
            var circle = core.GetType("Aardvark.Base.Circle2" + suffix, true)!;
            var box = core.GetType("Aardvark.Base.Box2" + suffix, true)!;
            var x = Expression.Parameter(typeof(double)); var y = Expression.Parameter(typeof(double)); var r = Expression.Parameter(typeof(double));
            var center = Expression.New(vector.GetConstructor(new[] { scalar, scalar })!, Expression.Convert(x, scalar), Expression.Convert(y, scalar));
            var value = Expression.New(circle.GetConstructor(new[] { vector, scalar })!, center, Expression.Convert(r, scalar));
            var bounds = Expression.Variable(box);
            Expression Coordinate(string bound, string axis)
                => Expression.Convert(Expression.Field(Expression.Field(bounds, bound), axis), typeof(double));
            return Expression.Lambda<Func<double, double, double, double[]>>(
                Expression.Block(new[] { bounds }, Expression.Assign(bounds, Expression.Property(value, "InscribedSquare")),
                    Expression.NewArrayInit(typeof(double), Coordinate("Min", "X"), Coordinate("Min", "Y"), Coordinate("Max", "X"), Coordinate("Max", "Y"))), x, y, r).Compile();
        }

        private static double Quantize(double value, bool single) => single ? (float)value : value;
        private static double Unit(bool single) => single ? 5.9604644775390625e-8 : 1.1102230246251565e-16;
        private static double EndpointTolerance(double center, double radius, bool single)
            => 4 * Unit(single) * (Math.Abs(center) + radius);

        private static void Check(double[] b, double x, double y, double radius, bool single, string context)
        {
            // Independent of sqrt(radius * radius * 0.5) in production.
            double half = radius / Math.Sqrt(2);
            double tx = EndpointTolerance(x, radius, single), ty = EndpointTolerance(y, radius, single);
            var expected = new[] { x - half, y - half, x + half, y + half };
            for (int i = 0; i < 4; i++)
                Assert.That(b[i], Is.EqualTo(expected[i]).Within((i & 1) == 0 ? tx : ty), $"{context}, endpoint={i}");
            if (radius == 0)
                for (int i = 0; i < 4; i++) Assert.That(b[i], Is.EqualTo((i & 1) == 0 ? x : y), $"{context}, point endpoint={i}");
            double width = b[2] - b[0], height = b[3] - b[1];
            Assert.That(width, Is.GreaterThanOrEqualTo(0), context);
            Assert.That(height, Is.GreaterThanOrEqualTo(0), context);
            Assert.That(width, Is.EqualTo(2 * half).Within(2 * tx), $"{context}, width");
            Assert.That(height, Is.EqualTo(2 * half).Within(2 * ty), $"{context}, height");
            Assert.That(width, Is.EqualTo(height).Within(2 * (tx + ty)), $"{context}, equal sides");
            Assert.That((b[0] + b[2]) * 0.5, Is.EqualTo(x).Within(tx), $"{context}, center X");
            Assert.That((b[1] + b[3]) * 0.5, Is.EqualTo(y).Within(ty), $"{context}, center Y");
            // Coordinate rounding grows with the center as well as the radius.
            double radialTolerance = 8 * Unit(single) * (Math.Max(Math.Abs(x), Math.Abs(y)) + radius);
            for (int ix = 0; ix <= 2; ix += 2)
            for (int iy = 1; iy <= 3; iy += 2)
            {
                double dx = b[ix] - x, dy = b[iy] - y;
                Assert.That(Math.Sqrt(dx * dx + dy * dy), Is.EqualTo(radius).Within(radialTolerance), $"{context}, corner={ix},{iy}");
            }
        }

        [Test]
        public void InscribedSquaresHaveCenteredEndpointsAndEqualCorners()
        {
            foreach (var core in Cores())
            foreach (bool single in new[] { true, false })
            {
                var square = Square(core, single);
                var centers = new[] { (10.0, 20.0), (-10.0, -20.0), (-10.0, 20.0), (10.0, -20.0), (0.0, 17.0), (-17.0, 0.0), (0.25, -3.5), (-0.5, 0.75), (128.0, -64.0), (0.0, 0.0) };
                foreach (var center in centers)
                foreach (double input in new[] { 2.0, 0.0, 0.125, 0.5, 1.0, 3.25, Math.Sqrt(2), 10.0, 64.0 })
                {
                    double x = Quantize(center.Item1, single), y = Quantize(center.Item2, single), radius = Quantize(input, single);
                    Check(square(x, y, radius), x, y, radius, single, $"{core.Location}: single={single}, center=({x:R},{y:R}), radius={radius:R}");
                }
            }
        }

        [Test]
        public void InscribedSquaresKeepOriginExtentsAndTranslateWithCircles()
        {
            foreach (var core in Cores())
            foreach (bool single in new[] { true, false })
            {
                var square = Square(core, single);
                Func<double, double> legacyHalf;
                var fun = core.GetType("Aardvark.Base.Fun", true)!;
                if (single)
                {
                    var sqrt = (Func<float, float>)fun.GetMethod("Sqrt", new[] { typeof(float) })!.CreateDelegate(typeof(Func<float, float>));
                    legacyHalf = r => sqrt((float)r * (float)r * 0.5f);
                }
                else
                {
                    var sqrt = (Func<double, double>)fun.GetMethod("Sqrt", new[] { typeof(double) })!.CreateDelegate(typeof(Func<double, double>));
                    legacyHalf = r => sqrt(r * r * 0.5);
                }
                int seed = single ? 73051 : 73052;
                var random = new Random(seed);
                for (int sample = 0; sample < 1024; sample++)
                {
                    double x = Quantize((random.NextDouble() * 2 - 1) * 128, single), y = Quantize((random.NextDouble() * 2 - 1) * 128, single);
                    double radius = Quantize(sample % 16 == 0 ? 0 : Math.ScaleB(1 + random.NextDouble(), random.Next(-4, 7)), single);
                    double nx = Quantize(x + (random.NextDouble() * 2 - 1) * 32, single), ny = Quantize(y + (random.NextDouble() * 2 - 1) * 32, single);
                    string context = $"{core.Location}: single={single}, seed={seed}, sample={sample}, center=({x:R},{y:R}), translated=({nx:R},{ny:R}), radius={radius:R}";
                    var origin = square(0, 0, radius); double h = legacyHalf(radius);
                    for (int i = 0; i < 4; i++)
                    {
                        double expected = i < 2 ? -h : h;
                        if (h == 0) Assert.That(origin[i], Is.EqualTo(0), $"{context}, origin endpoint={i}");
                        else Assert.That(BitConverter.DoubleToInt64Bits(origin[i]), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), $"{context}, unchanged origin endpoint={i}");
                    }
                    var before = square(x, y, radius); var after = square(nx, ny, radius);
                    Check(origin, 0, 0, radius, single, context + ", origin");
                    Check(before, x, y, radius, single, context + ", before");
                    Check(after, nx, ny, radius, single, context + ", after");
                    for (int i = 0; i < 4; i++)
                    {
                        bool horizontal = (i & 1) == 0;
                        double delta = horizontal ? nx - x : ny - y;
                        double tolerance = horizontal
                            ? EndpointTolerance(x, radius, single) + EndpointTolerance(nx, radius, single)
                            : EndpointTolerance(y, radius, single) + EndpointTolerance(ny, radius, single);
                        Assert.That(after[i], Is.EqualTo(before[i] + delta).Within(tolerance), $"{context}, translated endpoint={i}");
                    }
                }
            }
        }
    }
}

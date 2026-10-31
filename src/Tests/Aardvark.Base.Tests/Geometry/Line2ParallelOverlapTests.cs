using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Line2ParallelOverlapTests
    {
        private static readonly V2d[] Directions = { new V2d(1, 0), new V2d(0, 1), new V2d(3, 4), new V2d(4, 3), new V2d(-3, 4), new V2d(4, -3) };
        private static readonly double[] Scales = { .25, 1, 4 };

        [Test]
        public void SeparatedParallelSegmentsAreRejected()
        {
            foreach (var core in Cores())
            foreach (var direction in Directions)
            foreach (var origin in new[] { V2d.Zero, new V2d(16, -32), new V2d(-.5, .25), new V2d(core.Wide ? Math.ScaleB(1, 40) : Math.ScaleB(1, 16), core.Wide ? -Math.ScaleB(1, 40) : -Math.ScaleB(1, 16)) })
            foreach (var scale in Scales)
            foreach (var separation in new[] { .5, 1, 3, 8 })
            {
                var u = direction * scale; var n = new V2d(-u.Y, u.X);
                // Includes (0,0)-(4,0) versus (1,3)-(3,3), epsilon=.125.
                foreach (var pair in Orders(origin, origin + 4 * u, origin + u + separation * n, origin + 3 * u + separation * n))
                foreach (var epsilon in new double?[] { null, .125 * scale })
                foreach (var overlapping in new[] { false, true })
                    Miss(core, pair, epsilon, overlapping, $"separated, direction={direction}, origin={origin}, scale={scale}, separation={separation}");
            }
        }

        [Test]
        public void ParallelOverlapDistanceTolerancesRespectBoundaries()
        {
            foreach (var core in Cores())
            foreach (var direction in new[] { V2d.XAxis, V2d.YAxis, -V2d.XAxis, -V2d.YAxis })
            foreach (var scale in Scales)
            foreach (var along in new[] { 0, 16 })
            foreach (var epsilon in new double?[] { null, 0, .125 })
            {
                var threshold = epsilon ?? Math.ScaleB(1, core.Wide ? -50 : -21);
                var below = threshold == 0 ? 0 : Neighbor(threshold, core.Wide, false);
                var above = Neighbor(threshold, core.Wide, true);
                foreach (var distance in new[] { 0, below, threshold, above, threshold == 0 ? 1 : 2 * threshold })
                foreach (var sign in new[] { -1, 1 })
                {
                    var origin = direction * along; var u = direction * scale;
                    var offset = new V2d(-direction.Y, direction.X) * (sign * distance);
                    foreach (var pair in Orders(origin, origin + 4 * u, origin + u + offset, origin + 3 * u + offset))
                    {
                        var label = $"distance boundary, direction={direction}, scale={scale}, along={along}, distance={sign * distance}, threshold={threshold}";
                        var accepted = epsilon.HasValue ? distance <= threshold : distance < threshold;
                        if (!accepted) Miss(core, pair, epsilon, true, label);
                        else
                        {
                            // Fixed interior overlap [1,3]; no interval-tolerance expansion.
                            var expected = origin + (pair.Forward ? 1 : 3) * u + (pair.Swapped ? offset : V2d.Zero);
                            Hit(core, pair, epsilon, true, expected, 0, label);
                        }
                    }
                }
            }
        }

        [Test]
        public void CollinearOverlapsKeepProjectedEndpointSelection()
        {
            foreach (var core in Cores())
            foreach (var direction in new[] { V2d.XAxis, V2d.YAxis, new V2d(3, 4) })
            foreach (var origin in new[] { V2d.Zero, new V2d(16, -32), new V2d(-.5, .25) })
            foreach (var scale in Scales)
            foreach (var interval in new (double Min, double Max)[] { (1, 3), (2, 6), (-1, 5), (4, 6), (5, 7), (0, 4) })
            {
                var u = direction * scale; var length = direction.Length;
                var lo = Math.Max(0, interval.Min); var hi = Math.Min(4, interval.Max);
                foreach (var pair in Orders(origin, origin + 4 * u, origin + interval.Min * u, origin + interval.Max * u))
                foreach (var epsilon in new double?[] { null, .125 })
                foreach (var overlapping in new[] { false, true })
                {
                    var label = $"collinear, direction={direction}, origin={origin}, scale={scale}, interval=[{interval.Min},{interval.Max}]";
                    var shared = pair.A0 == pair.B0;
                    if (!shared && (!overlapping || lo > hi)) Miss(core, pair, epsilon, overlapping, label);
                    else
                    {
                        var expected = shared ? pair.B0 : origin + (pair.Forward ? lo : hi) * u;
                        if (!shared && lo == hi && epsilon.HasValue)
                            expected -= (pair.Forward ? 1 : -1) * direction / length * epsilon.Value;
                        var tolerance = direction == V2d.XAxis || direction == V2d.YAxis ? 0 : 8 * Math.ScaleB(1, core.Wide ? -53 : -24) * (4 * scale * length);
                        Hit(core, pair, epsilon, overlapping, core.Round(expected), tolerance, label);
                    }
                }
            }
        }

        [Test]
        public void CrossingsMissesAndSharedOriginsKeepTheirResults()
        {
            foreach (var core in Cores())
            {
                foreach (var direction in Directions)
                foreach (var origin in new[] { V2d.Zero, new V2d(16, -32), new V2d(-.5, .25) })
                foreach (var scale in Scales)
                foreach (var crossing in new[] { true, false })
                {
                    var u = direction * scale; var n = new V2d(-u.Y, u.X); var location = crossing ? 2 : 6;
                    foreach (var pair in Orders(origin, origin + 4 * u, origin + location * u - 2 * n, origin + location * u + 2 * n))
                    foreach (var epsilon in new double?[] { null, 0, .125 })
                    foreach (var overlapping in new[] { false, true })
                    {
                        var label = $"nonparallel crossing={crossing}, direction={direction}, origin={origin}, scale={scale}";
                        if (crossing) Hit(core, pair, epsilon, overlapping, origin + 2 * u, 0, label);
                        else Miss(core, pair, epsilon, overlapping, label);
                    }
                }
                // Keep the pre-existing shared-origin shortcut, including its squared-distance threshold.
                foreach (var delta in new[] { 0, Math.ScaleB(1, core.Wide ? -27 : -13) })
                foreach (var epsilon in new double?[] { null, 0, .125 })
                foreach (var overlapping in new[] { false, true })
                {
                    var p0 = new V2d(delta, delta);
                    var pair = new Pair(V2d.Zero, new V2d(4, 0), p0, new V2d(2, 3), false, true);
                    Hit(core, pair, epsilon, overlapping, p0, 0, "existing shared-origin shortcut, delta=" + delta);
                }
            }
        }

        private static double Neighbor(double value, bool wide, bool above)
            => wide ? (above ? Math.BitIncrement(value) : Math.BitDecrement(value)) : (above ? MathF.BitIncrement((float)value) : MathF.BitDecrement((float)value));

        private readonly struct Pair
        {
            public readonly V2d A0, A1, B0, B1;
            public readonly bool Swapped, Forward;
            public Pair(V2d a0, V2d a1, V2d b0, V2d b1, bool swapped, bool forward)
            { A0 = a0; A1 = a1; B0 = b0; B1 = b1; Swapped = swapped; Forward = forward; }
            public override string ToString() => $"[{A0},{A1}] vs [{B0},{B1}]";
        }

        private static IEnumerable<Pair> Orders(V2d a0, V2d a1, V2d b0, V2d b1)
        {
            for (var swap = 0; swap < 2; swap++)
            for (var ra = 0; ra < 2; ra++)
            for (var rb = 0; rb < 2; rb++)
            {
                var x0 = ra == 0 ? a0 : a1; var x1 = ra == 0 ? a1 : a0;
                var y0 = rb == 0 ? b0 : b1; var y1 = rb == 0 ? b1 : b0;
                yield return swap == 0 ? new Pair(x0, x1, y0, y1, false, ra == 0) : new Pair(y0, y1, x0, x1, true, rb == 0);
            }
        }

        private static void Miss(Variant core, Pair pair, double? epsilon, bool overlapping, string label)
        {
            var result = core.Query(pair, epsilon, overlapping); var context = $"{core.Name}: {label}, {pair}, epsilon={epsilon}, overlapping={overlapping}";
            Assert.IsFalse(result.Hit, context);
            Assert.That(double.IsNaN(result.Point.X) && double.IsNaN(result.Point.Y), Is.True, context + " NaN miss point");
        }

        private static void Hit(Variant core, Pair pair, double? epsilon, bool overlapping, V2d expected, double tolerance, string label)
        {
            var result = core.Query(pair, epsilon, overlapping); var context = $"{core.Name}: {label}, {pair}, epsilon={epsilon}, overlapping={overlapping}, expected={expected}";
            Assert.IsTrue(result.Hit, context);
            if (tolerance == 0) Assert.AreEqual(expected, result.Point, context + " exact point");
            else for (var i = 0; i < 2; i++) Assert.That(Math.Abs(result.Point[i] - expected[i]), Is.LessThanOrEqualTo(tolerance), context + ", component=" + i);
        }

        private sealed class Variant
        {
            public readonly string Name;
            public readonly bool Wide;
            private readonly ConstructorInfo pointConstructor, lineConstructor;
            private readonly MethodInfo ordinary, epsilonMethod;
            private readonly FieldInfo x, y;
            public Variant(Assembly assembly, bool wide, string target)
            {
                Wide = wide; Name = target + (wide ? "/double" : "/float");
                var scalar = wide ? typeof(double) : typeof(float); var suffix = wide ? "d" : "f";
                var vector = assembly.GetType("Aardvark.Base.V2" + suffix, true); var line = assembly.GetType("Aardvark.Base.Line2" + suffix, true);
                Assert.AreSame(assembly, vector.Assembly, Name); Assert.AreSame(assembly, line.Assembly, Name);
                pointConstructor = vector.GetConstructor(new[] { scalar, scalar }); lineConstructor = line.GetConstructor(new[] { vector, vector });
                var tests = assembly.GetType("Aardvark.Base.GeometryFun", true);
                ordinary = tests.GetMethod("IntersectsLine", new[] { line, vector, vector, typeof(bool), vector.MakeByRefType() });
                epsilonMethod = tests.GetMethod("IntersectsLine", new[] { line, vector, vector, scalar, typeof(bool), vector.MakeByRefType() });
                Assert.IsNotNull(ordinary, Name); Assert.IsNotNull(epsilonMethod, Name);
                x = vector.GetField("X"); y = vector.GetField("Y");
            }
            private object Scalar(double value) => Wide ? (object)value : (float)value;
            private object Point(V2d p) => pointConstructor.Invoke(new[] { Scalar(p.X), Scalar(p.Y) });
            public V2d Round(V2d p) => Wide ? p : new V2d((float)p.X, (float)p.Y);
            public (bool Hit, V2d Point) Query(Pair pair, double? epsilon, bool overlapping)
            {
                var line = lineConstructor.Invoke(new[] { Point(pair.A0), Point(pair.A1) });
                var b0 = Point(pair.B0); var b1 = Point(pair.B1);
                var args = epsilon.HasValue ? new[] { line, b0, b1, Scalar(epsilon.Value), (object)overlapping, null } : new[] { line, b0, b1, (object)overlapping, null };
                var hit = (bool)(epsilon.HasValue ? epsilonMethod : ordinary).Invoke(null, args); var result = args[args.Length - 1];
                return (hit, new V2d(Convert.ToDouble(x.GetValue(result)), Convert.ToDouble(y.GetValue(result))));
            }
        }

        private static IEnumerable<Variant> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Line2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new Variant(assembly, false, target); yield return new Variant(assembly, true, target);
            }
        }
    }
}

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
    public class BoxSphereContainmentTests
    {
        [Test]
        public void ClosedBoxesContainInteriorAndFaceTangentSpheres()
        {
            foreach (var core in Cores())
            {
                foreach (var shift in Shifts(core))
                foreach (var scale in new[] { .5, 1, 4 })
                foreach (var radiusScale in new[] { .25, 1, 2, 4 })
                {
                    var min = Offset(shift, new[] { -8 * scale, -12 * scale, -16 * scale });
                    var max = Offset(shift, new[] { 8 * scale, 12 * scale, 16 * scale });
                    var radius = radiusScale * scale;
                    Check(core, min, max, shift, radius, true, "interior");
                    for (var axis = 0; axis < 3; axis++)
                    foreach (var upper in new[] { false, true })
                    {
                        var center = (double[])shift.Clone(); center[axis] = upper ? max[axis] - radius : min[axis] + radius;
                        Check(core, min, max, center, radius, true, $"face tangent axis={axis}, upper={upper}");
                    }
                }
                var random = new Random(102337);
                for (var sample = 0; sample < 4096; sample++)
                {
                    var shift = Shifts(core)[sample % Shifts(core).Length];
                    var min = Enumerable.Range(0, 3).Select(i => shift[i] + random.Next(-64, 65) / 8.0).ToArray();
                    var max = min.Select(v => v + random.Next(0, 129) / 8.0).ToArray();
                    var center = min.Select(v => v + random.Next(-32, 161) / 8.0).ToArray();
                    var radius = random.Next(0, 65) / 8.0;
                    Check(core, min, max, center, radius, Oracle(min, max, center, radius), $"seed102337, sample={sample}");
                }
            }
        }

        [Test]
        public void ProtrudingOversizedAndOutsideSpheresAreRejected()
        {
            foreach (var core in Cores())
            foreach (var shift in Shifts(core))
            foreach (var scale in new[] { .5, 1, 4 })
            foreach (var radiusScale in new[] { .25, 1, 2, 4 })
            {
                var min = Offset(shift, new[] { -8 * scale, -12 * scale, -16 * scale });
                var max = Offset(shift, new[] { 8 * scale, 12 * scale, 16 * scale });
                var radius = radiusScale * scale;
                for (var axis = 0; axis < 3; axis++)
                foreach (var upper in new[] { false, true })
                {
                    var center = (double[])shift.Clone(); center[axis] = upper ? max[axis] - radius : min[axis] + radius;
                    Check(core, min, max, center, core.NextUp(radius), false, $"one-ULP radius protrusion axis={axis}, upper={upper}");
                    center[axis] = upper ? max[axis] + scale : min[axis] - scale;
                    Check(core, min, max, center, radius, false, $"outside center axis={axis}, upper={upper}");
                }
                Check(core, min, max, shift, 9 * scale, false, "oversized centered sphere");
                Check(core, min, max, shift, 64 * scale, false, "sphere surrounding box");
            }
        }

        [Test]
        public void ZeroRadiusAndInvalidInputsRespectClosedBoxes()
        {
            foreach (var core in Cores())
            {
                foreach (var shift in Shifts(core))
                for (var mask = 0; mask < 8; mask++)
                {
                    var extents = Enumerable.Range(0, 3).Select(i => (mask & 1 << i) == 0 ? 0 : 2.0 * (1 << i)).ToArray();
                    var min = shift.Zip(extents, (c, e) => c - e).ToArray(); var max = shift.Zip(extents, (c, e) => c + e).ToArray();
                    var positions = Enumerable.Range(0, 3).Select(i => new[] { min[i], shift[i], max[i] }.Distinct().ToArray()).ToArray();
                    foreach (var x in positions[0]) foreach (var y in positions[1]) foreach (var z in positions[2])
                    foreach (var radius in new[] { 0.0, -0.0 })
                        Check(core, min, max, new[] { x, y, z }, radius, true, $"zero-radius interior/face/edge/corner, degeneracy mask={mask}");
                    for (var axis = 0; axis < 3; axis++)
                    {
                        var center = (double[])shift.Clone(); center[axis] = core.NextDown(min[axis]);
                        Check(core, min, max, center, 0, false, $"zero-radius outside axis={axis}, mask={mask}");
                    }
                }
                var low = new[] { -8.0, -8, -8 }; var high = new[] { 8.0, 8, 8 }; var zero = new double[3];
                var tiny = Math.ScaleB(1, core.Single ? -149 : -1074);
                Check(core, low, high, zero, tiny, true, "positive subnormal radius");
                Check(core, zero, zero, zero, tiny, false, "point box cannot contain positive radius");
                foreach (var radius in new[] { -tiny, -1, double.NaN, double.NegativeInfinity, double.PositiveInfinity })
                {
                    Check(core, low, high, zero, radius, false, "invalid radius in bounded box");
                    Check(core, new[] { double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity }, new[] { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity }, zero, radius, false, "invalid radius in unbounded box");
                }
                for (var axis = 0; axis < 3; axis++)
                {
                    var min = (double[])low.Clone(); var max = (double[])high.Clone(); min[axis] = 1; max[axis] = -1;
                    Check(core, min, max, zero, 0, false, $"reversed box axis={axis}");
                    foreach (var radius in new[] { 0.0, 1 })
                    {
                        min = (double[])low.Clone(); min[axis] = double.NaN;
                        Check(core, min, high, zero, radius, false, $"NaN minimum axis={axis}");
                        max = (double[])high.Clone(); max[axis] = double.NaN;
                        Check(core, low, max, zero, radius, false, $"NaN maximum axis={axis}");
                        foreach (var value in new[] { double.NaN, double.NegativeInfinity, double.PositiveInfinity })
                        {
                            var center = new double[3]; center[axis] = value;
                            Check(core, low, high, center, radius, false, $"nonfinite center axis={axis}");
                        }
                    }
                    min = (double[])low.Clone(); max = (double[])high.Clone(); min[axis] = double.NegativeInfinity; max[axis] = double.PositiveInfinity;
                    Check(core, min, max, zero, 1, true, $"valid unbounded axis={axis}");
                }
                Check(core, new[] { 1.0, 1, 1 }, new[] { -1.0, -1, -1 }, zero, 0, false, "invalid sentinel ordering");
                Check(core, new[] { double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity }, new[] { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity }, zero, 1, true, "valid unbounded box");
            }
        }

        private static double[][] Shifts(Core core)
        {
            var large = Math.ScaleB(1, core.Single ? 20 : 49);
            return new[] { new double[3], new[] { 16.0, -32, 8 }, new[] { -.5, .25, -.125 }, new[] { large, -large, large }, new[] { -large, large, -large } };
        }
        private static double[] Offset(double[] a, double[] b) => a.Zip(b, (x, y) => x + y).ToArray();

        private static void Check(Core core, double[] min, double[] max, double[] center, double radius, bool expected, string label)
        {
            var context = $"{core.Name}: {label}, min=({string.Join(",", min)}), max=({string.Join(",", max)}), center=({string.Join(",", center)}), radius={radius}";
            foreach (var v in min.Concat(max).Concat(center).Concat(new[] { radius }).Where(v => !double.IsNaN(v)))
                Assert.AreEqual(v, core.Round(v), context + " representable input");
            Assert.AreEqual(expected, Oracle(min, max, center, radius), context + " independent exact support oracle");
            Assert.AreEqual(expected, core.Contains(min, max, center, radius), context);
        }

        private static bool Oracle(double[] min, double[] max, double[] center, double radius)
        {
            if (double.IsNaN(radius) || double.IsInfinity(radius) || radius < 0 || center.Any(v => double.IsNaN(v) || double.IsInfinity(v))) return false;
            for (var i = 0; i < 3; i++)
            {
                if (double.IsNaN(min[i]) || double.IsNaN(max[i]) || min[i] > max[i]) return false;
                if (min[i] != double.NegativeInfinity && (double.IsInfinity(min[i]) || Units(center[i]) - Units(radius) < Units(min[i]))) return false;
                if (max[i] != double.PositiveInfinity && (double.IsInfinity(max[i]) || Units(center[i]) + Units(radius) > Units(max[i]))) return false;
            }
            return true;
        }

        // Exact finite binary value scaled by 2^1074; no floating support arithmetic.
        private static BigInteger Units(double value)
        {
            var bits = BitConverter.DoubleToInt64Bits(value); var magnitude = (ulong)bits & 0x7fffffffffffffffUL;
            var exponent = (int)(magnitude >> 52); var mantissa = magnitude & 0xfffffffffffffUL;
            var result = new BigInteger(exponent == 0 ? mantissa : mantissa | 0x10000000000000UL) << (exponent == 0 ? 0 : exponent - 1);
            return bits < 0 ? -result : result;
        }

        private sealed class Core
        {
            public readonly bool Single;
            public readonly string Name;
            private readonly ConstructorInfo vector, box, sphere;
            private readonly MethodInfo contains;
            public Core(Assembly assembly, string target, bool single)
            {
                Single = single; Name = target + (single ? "/float" : "/double");
                var suffix = single ? "f" : "d"; var scalar = single ? typeof(float) : typeof(double);
                var v = assembly.GetType("Aardvark.Base.V3" + suffix, true);
                var b = assembly.GetType("Aardvark.Base.Box3" + suffix, true);
                var s = assembly.GetType("Aardvark.Base.Sphere3" + suffix, true);
                vector = v.GetConstructor(new[] { scalar, scalar, scalar }); box = b.GetConstructor(new[] { v, v }); sphere = s.GetConstructor(new[] { v, scalar });
                contains = assembly.GetType("Aardvark.Base.GeometryFun", true).GetMethod("Contains", new[] { b, s });
            }
            public double Round(double value) => Single ? (double)(float)value : value;
            public double NextUp(double value) => Single ? MathF.BitIncrement((float)value) : Math.BitIncrement(value);
            public double NextDown(double value) => Single ? MathF.BitDecrement((float)value) : Math.BitDecrement(value);
            private object Scalar(double value) => Single ? (object)(float)value : (object)value;
            private object Vector(double[] values) => vector.Invoke(values.Select(Scalar).ToArray());
            public bool Contains(double[] min, double[] max, double[] center, double radius) => (bool)contains.Invoke(null, new[]
            {
                box.Invoke(new[] { Vector(min), Vector(max) }), sphere.Invoke(new[] { Vector(center), Scalar(radius) })
            });
        }

        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Box3d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                foreach (var single in new[] { true, false }) yield return new Core(assembly, target, single);
            }
        }
    }
}

using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Aardvark.Tests.Geometry
{
    public class EllipseTests
    {
        [Test]
        public void SmallConjugateDiametersRecoverPrincipalAxes()
        {
            foreach (var core in Cores())
            foreach (var scale in core.Wide ? new[] { 1e-8, Math.ScaleB(1, -60), Math.ScaleB(1, -40), Math.ScaleB(1, -28) }
                : new[] { 1e-4, Math.ScaleB(1, -30), Math.ScaleB(1, -20), Math.ScaleB(1, -14) })
            foreach (var pair in ConjugatePairs())
            foreach (var basis in Bases(core.Dimension))
            foreach (var swap in new[] { false, true })
            foreach (var signA in new[] { -1, 1 })
            foreach (var signB in new[] { -1, 1 })
            {
                var a = Embed(swap ? pair.B : pair.A, basis, scale * signA);
                var b = Embed(swap ? pair.A : pair.B, basis, scale * signB);
                Check(core, a, b, basis, $"small scale={scale:R}, swap={swap}, signs={signA}/{signB}");
            }
        }

        [Test]
        public void OrdinaryConjugateDiametersKeepPrincipalGeometry()
        {
            foreach (var core in Cores())
            foreach (var scale in new[] { 0.5, 1.0, 16.0 })
            foreach (var pair in ConjugatePairs())
            foreach (var basis in Bases(core.Dimension))
            foreach (var swap in new[] { false, true })
                Check(core, Embed(swap ? pair.B : pair.A, basis, scale),
                    Embed(swap ? pair.A : pair.B, basis, -scale), basis, $"ordinary scale={scale}, swap={swap}");
        }

        [Test]
        public void OrthogonalAndZeroDiametersKeepTheirAxesAndOrdering()
        {
            foreach (var core in Cores())
            foreach (var scale in new[] { Math.ScaleB(1, core.Wide ? -60 : -30), 0.5, 1.0, 16.0 })
            foreach (var basis in Bases(core.Dimension))
            foreach (var pair in new[] { (A: new[] { 1.0, 0.0 }, B: new[] { 0.0, 0.5 }),
                (A: new[] { 0.0, 0.5 }, B: new[] { 1.0, 0.0 }),
                (A: new[] { 1.0, 0.0 }, B: new[] { 0.0, 1.0 }),
                (A: new[] { 0.0, 0.0 }, B: new[] { 0.0, -1.0 }),
                (A: new[] { -1.0, 0.0 }, B: new[] { 0.0, 0.0 }),
                (A: new[] { 0.0, 0.0 }, B: new[] { 0.0, 0.0 }) })
            {
                var a = Embed(pair.A, basis, scale); var b = Embed(pair.B, basis, scale);
                var result = Check(core, a, b, basis, $"orthogonal/zero scale={scale:R}");
                var aa = core.Round(a); var bb = core.Round(b);
                var first = Norm2(aa) >= Norm2(bb) ? aa : bb;
                var second = Norm2(aa) >= Norm2(bb) ? bb : aa;
                Assert.That(result.Axis0, Is.EqualTo(first), core.Name + " unchanged major/tied axis");
                Assert.That(result.Axis1, Is.EqualTo(second), core.Name + " unchanged minor axis");
            }
            foreach (var core in Cores())
            {
                var zero = new double[core.Dimension];
                var result = core.Create(zero, zero, zero, zero, true);
                Assert.That(result.Fields, Is.EqualTo(core.Zero), core.Name + " zero ellipse");
                Assert.That(result.Major2, Is.Zero, core.Name); Assert.That(result.Minor2, Is.Zero, core.Name);
            }
        }

        private static IEnumerable<(double[] A, double[] B)> ConjugatePairs()
        {
            yield return (new[] { 1.0, 0.0 }, new[] { 1.0, 1.0 }); // Reported reproduction.
            yield return (new[] { 2.0, 1.0 }, new[] { -1.0, 3.0 });
            yield return (new[] { 1.0, -2.0 }, new[] { 3.0, 1.0 });
            yield return (new[] { 1.0, 0.0 }, new[] { 2.0, 0.0 }); // Collinear, unequal lengths.
            yield return (new[] { 1.0, 2.0 }, new[] { 1.0, 2.0 }); // Collinear, equal lengths.
        }

        private static IEnumerable<(double[] U, double[] V)> Bases(int dimension)
        {
            if (dimension == 2) { yield return (new[] { 1.0, 0.0 }, new[] { 0.0, 1.0 }); yield break; }
            yield return (new[] { 1.0, 0.0, 0.0 }, new[] { 0.0, 1.0, 0.0 });
            yield return (new[] { 0.0, 1.0, 0.0 }, new[] { 0.0, 0.0, 1.0 });
            yield return (new[] { 0.0, 0.0, 1.0 }, new[] { 1.0, 0.0, 0.0 });
            yield return (new[] { 1.0, 2.0, 2.0 }, new[] { 2.0, 1.0, -2.0 });
        }
        private static double[] Embed(double[] a, (double[] U, double[] V) basis, double scale) =>
            basis.U.Select((u, i) => (a[0] * u + a[1] * basis.V[i]) * scale).ToArray();
        private static double Dot(double[] a, double[] b) => a.Select((x, i) => x * b[i]).Sum();
        private static double Norm2(double[] a) => Dot(a, a);

        private static Result Check(Core core, double[] a, double[] b, (double[] U, double[] V) basis, string label)
        {
            var center = new[] { 2.5, -3.0, 7.0 }.Take(core.Dimension).ToArray();
            var normal = core.Dimension == 2 ? new double[2] : new[] {
                basis.U[1] * basis.V[2] - basis.U[2] * basis.V[1],
                basis.U[2] * basis.V[0] - basis.U[0] * basis.V[2],
                basis.U[0] * basis.V[1] - basis.U[1] * basis.V[0] };
            a = core.Round(a); b = core.Round(b);
            var context = $"{core.Name}: {label}, a=({string.Join(",", a)}), b=({string.Join(",", b)})";
            var plain = core.Create(center, normal, a, b, false);
            var result = core.Create(center, normal, a, b, true);
            Assert.That(result.Fields, Is.EqualTo(plain.Fields), context + " overload agreement");
            var frame = core.Round(center.Concat(core.Dimension == 3 ? normal : Array.Empty<double>()).ToArray());
            for (var i = 0; i < frame.Length; i++)
                Assert.AreEqual(BitConverter.DoubleToInt64Bits(frame[i]), BitConverter.DoubleToInt64Bits(result.Fields[i]), context + " center/normal");
            Assert.AreEqual(result.Axis0Squared, result.Major2, context + " major squared output");
            Assert.AreEqual(result.Axis1Squared, result.Minor2, context + " minor squared output");

            var scale = a.Concat(b).Select(Math.Abs).Max();
            if (scale == 0) { Assert.That(result.Axis0.Concat(result.Axis1), Is.All.Zero, context); return result; }
            var an = a.Select(x => x / scale).ToArray(); var bn = b.Select(x => x / scale).ToArray();
            var x0 = result.Axis0.Select(x => x / scale).ToArray(); var x1 = result.Axis1.Select(x => x / scale).ToArray();
            var aa = Norm2(an); var bb = Norm2(bn); var ab = Dot(an, bn);
            // Eigenvalues of the Gram matrix, not the production rotation. Wedge squares
            // recover the determinant without cancellation, including collinear inputs.
            var disc = Math.Sqrt((aa - bb) * (aa - bb) + 4 * ab * ab);
            var major = (aa + bb + disc) / 2;
            var determinant = 0.0;
            for (var i = 0; i < core.Dimension; i++)
            for (var j = i + 1; j < core.Dimension; j++)
            { var wedge = an[i] * bn[j] - an[j] * bn[i]; determinant += wedge * wedge; }
            var minor = determinant / major;
            var allowance = 64 * core.UnitRoundoff * major;
            Assert.That(Math.Abs(Norm2(x0) - major), Is.LessThanOrEqualTo(allowance), context + " analytic major squared length");
            Assert.That(Math.Abs(Norm2(x1) - minor), Is.LessThanOrEqualTo(allowance), context + " analytic minor squared length");
            Assert.That(result.Major2, Is.GreaterThanOrEqualTo(result.Minor2), context + " axis order");
            var orthogonalScale = minor > 0 ? Math.Sqrt(major * minor) : major;
            Assert.That(Math.Abs(Dot(x0, x1)), Is.LessThanOrEqualTo(64 * core.UnitRoundoff * orthogonalScale), context + " relative orthogonality (absolute residual for rank one)");
            for (var i = 0; i < core.Dimension; i++)
            for (var j = 0; j < core.Dimension; j++)
                Assert.That(Math.Abs(x0[i] * x0[j] + x1[i] * x1[j] - an[i] * an[j] - bn[i] * bn[j]),
                    Is.LessThanOrEqualTo(allowance), context + $" covariance[{i},{j}]");
            return result;
        }

        private sealed class Result
        {
            public double[] Fields, Axis0, Axis1;
            public double Major2, Minor2, Axis0Squared, Axis1Squared;
        }
        private sealed class Core
        {
            public readonly string Name;
            public readonly int Dimension;
            public readonly bool Wide;
            public double UnitRoundoff => Math.ScaleB(1, Wide ? -53 : -24);
            private readonly Type vector, ellipse;
            private readonly ConstructorInfo vectorCtor;
            private readonly MethodInfo plain, measured;
            public Core(Assembly assembly, string target, int dimension, bool wide)
            {
                Dimension = dimension; Wide = wide; var suffix = dimension + (wide ? "d" : "f"); Name = target + "/Ellipse" + suffix;
                vector = assembly.GetType("Aardvark.Base.V" + suffix, true); ellipse = assembly.GetType("Aardvark.Base.Ellipse" + suffix, true);
                var scalar = wide ? typeof(double) : typeof(float);
                vectorCtor = vector.GetConstructor(Enumerable.Repeat(scalar, dimension).ToArray());
                plain = ellipse.GetMethod("FromConjugateDiameters", Enumerable.Repeat(vector, dimension == 2 ? 3 : 4).ToArray());
                measured = ellipse.GetMethod("FromConjugateDiameters", plain.GetParameters().Select(p => p.ParameterType)
                    .Concat(new[] { scalar.MakeByRefType(), scalar.MakeByRefType() }).ToArray());
            }
            public double[] Round(double[] values) => values.Select(x => Wide ? x : (double)(float)x).ToArray();
            private object Vector(double[] values) => vectorCtor.Invoke(values.Select(x => Wide ? (object)x : (object)(float)x).ToArray());
            private double[] Coordinates(object value) => "XYZ".Take(Dimension).Select(c => Convert.ToDouble(vector.GetField(c.ToString()).GetValue(value))).ToArray();
            private double[] Fields(object value) => new[] { "Center", "Normal", "Axis0", "Axis1" }.Where(n => Dimension == 3 || n != "Normal")
                .SelectMany(n => Coordinates(ellipse.GetField(n).GetValue(value))).ToArray();
            public double[] Zero => Fields(ellipse.GetProperty("Zero").GetValue(null));
            public Result Create(double[] center, double[] normal, double[] a, double[] b, bool withLengths)
            {
                var args = new List<object> { Vector(center) }; if (Dimension == 3) args.Add(Vector(normal));
                args.Add(Vector(a)); args.Add(Vector(b)); if (withLengths) { args.Add(null); args.Add(null); }
                var array = args.ToArray(); var value = (withLengths ? measured : plain).Invoke(null, array);
                var axis0 = ellipse.GetField("Axis0").GetValue(value); var axis1 = ellipse.GetField("Axis1").GetValue(value);
                return new Result { Fields = Fields(value), Axis0 = Coordinates(axis0), Axis1 = Coordinates(axis1),
                    Major2 = withLengths ? Convert.ToDouble(array[array.Length - 2]) : 0, Minor2 = withLengths ? Convert.ToDouble(array[array.Length - 1]) : 0,
                    Axis0Squared = Convert.ToDouble(vector.GetProperty("LengthSquared").GetValue(axis0)),
                    Axis1Squared = Convert.ToDouble(vector.GetProperty("LengthSquared").GetValue(axis1)) };
            }
        }
        private static IEnumerable<Core> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Ellipse2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path); var assembly = Assembly.LoadFile(path);
                foreach (var dimension in new[] { 2, 3 })
                foreach (var wide in new[] { false, true }) yield return new Core(assembly, target, dimension, wide);
            }
        }
    }
}

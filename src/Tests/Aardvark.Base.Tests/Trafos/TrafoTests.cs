using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using NUnit.Framework;
using Aardvark.Base;

namespace Aardvark.Tests
{
    [TestFixture]
    public class TrafoTests : TestSuite
    {
        [Test]
        public static void Comparison()
            => TrafoTesting.GenericComparisonTest(TrafoTesting.GetRandomTrafo,
                t => new Trafo3d(t.Forward + 1, t.Backward + 1));

        [Test]
        public static void InverseTest()
            => TrafoTesting.GenericTest(rnd =>
            {
                var t = TrafoTesting.GetRandomTrafo(rnd, false);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = t.Forward.TransformPos(p);

                // Inverse property
                var res = t.Inverse.Forward.TransformPos(q);

                // Backward
                var res2 = t.Backward.TransformPos(q);

                TrafoTesting.AreEqual(p, res);
                TrafoTesting.AreEqual(p, res2);
            });

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomTrafo, Trafo3d.Parse);

        [Test]
        public void TrafoDecomposeCornerCasesTest()
        {
            TrafoTesting.GenericTest((rnd, i) =>
            {
                var jitter = (i / 100) * 1e-15;
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.YAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.ZAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.XAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.ZAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.YAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.XAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.YAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.ZAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.XAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.ZAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.YAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.XAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.YAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.ZAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.XAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.ZAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.YAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.XAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.YAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.ZAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.XAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.ZAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.YAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.XAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.YAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.ZAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.XAxis, V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.ZAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.YAxis, V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.XAxis, V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.YAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.ZAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.XAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.ZAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.YAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.XAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.YAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.ZAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.XAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.ZAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.YAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.XAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.YAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.ZAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.XAxis, -V3d.ZAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.ZAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.YAxis, -V3d.XAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.XAxis, -V3d.YAxis), Rot3d.Rotation(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
            });
        }

        void TestDecompose(Trafo3d trafo, Rot3d jitter)
        {
            var x = (M44d)jitter;
            trafo = trafo * new Trafo3d(x, x.Inverse);

            V3d r_d, s_d, t_d;
            trafo.Decompose(out s_d, out r_d, out t_d);

            var recomposed = Trafo3d.FromComponents(s_d, r_d, t_d);

            Assert.IsFalse(s_d.AnyNaN || r_d.AnyNaN || t_d.AnyNaN, "something NaN");

            var dt = trafo.Forward - recomposed.Forward;
            var e = dt.NormMax.Abs();
            Assert.IsTrue(e < 1e-9, "DIFF");

            var eq = CheckForwardBackwardConsistency(new Trafo3d(trafo.Forward, recomposed.Backward))
                && CheckForwardBackwardConsistency(new Trafo3d(recomposed.Forward, trafo.Backward));



            Assert.True(eq, "trafo not consistent");
        }

        [Test]
        public void TrafoDecomposeTest()
        {
            TrafoTesting.GenericTest((rnd, i) =>
            {
                var rot = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;
                var trans = rnd.UniformV3dFull() * 10 - 5;
                var scale = rnd.UniformV3dFull() * 4 - 2;

                TestDecompose(scale, rot, trans);
            });
        }

        void TestDecompose(V3d scale, V3d rotation, V3d translation)
        {
            var trafo = Trafo3d.FromComponents(scale, rotation, translation);
            V3d r_d, s_d, t_d;
            trafo.Decompose(out s_d, out r_d, out t_d);

            var recomposed = Trafo3d.FromComponents(s_d, r_d, t_d);

            Assert.IsFalse(s_d.AnyNaN || r_d.AnyNaN || t_d.AnyNaN, "something NaN");

            var e_scale = (s_d.Abs() - scale.Abs()).LengthSquared;
            var e_trans = (t_d - translation).LengthSquared;
            Assert.True(e_scale < 1e-5, "Scale");
            Assert.True(e_trans < 1e-5, "Translation");

            ValidateTrafos(trafo, recomposed);
        }

        [Test]
        public void TrafoRotIntoTest()
        {
            TrafoTesting.GenericTest((rnd, i) =>
            {
                var rx = new V3d(rnd.UniformDouble() * 1e-17, 0, 0) * (rnd.UniformDouble() > 0.5 ? 1 : -1);
                var ry = new V3d(0, rnd.UniformDouble() * 1e-17, 0) * (rnd.UniformDouble() > 0.5 ? 1 : -1);
                var rz = new V3d(0, 0, rnd.UniformDouble() * 1e-17) * (rnd.UniformDouble() > 0.5 ? 1 : -1);

                // equal cases
                var req = new[]
                {
                    Trafo3d.RotateInto(V3d.XAxis, (V3d.XAxis + ry).Normalized),
                    Trafo3d.RotateInto(V3d.YAxis, (V3d.YAxis + rz).Normalized),
                    Trafo3d.RotateInto(V3d.ZAxis, (V3d.ZAxis + rx).Normalized),
                };
                foreach (var r in req)
                    Assert.True(CheckForwardBackwardConsistency(r));

                // 180° cases
                var r180 = new[]
                {
                    Trafo3d.RotateInto(-V3d.XAxis, ( V3d.XAxis + ry).Normalized),
                    Trafo3d.RotateInto( V3d.XAxis, (-V3d.XAxis + rz).Normalized),
                    Trafo3d.RotateInto(-V3d.YAxis, ( V3d.YAxis + rz).Normalized),
                    Trafo3d.RotateInto( V3d.YAxis, (-V3d.YAxis + rx).Normalized),
                    Trafo3d.RotateInto(-V3d.ZAxis, ( V3d.ZAxis + rx).Normalized),
                    Trafo3d.RotateInto( V3d.ZAxis, (-V3d.ZAxis + ry).Normalized)
                };
                foreach (var r in r180)
                    Assert.True(CheckForwardBackwardConsistency(r));

                // 90° cases
                var r90 = new[]
                {
                    Trafo3d.RotateInto((-V3d.XAxis + rz).Normalized, V3d.ZAxis),
                    Trafo3d.RotateInto(( V3d.XAxis + ry).Normalized, V3d.ZAxis),
                    Trafo3d.RotateInto((-V3d.YAxis + rx).Normalized, V3d.ZAxis),
                    Trafo3d.RotateInto(( V3d.YAxis + rz).Normalized, V3d.ZAxis),

                    Trafo3d.RotateInto(-V3d.XAxis, (-V3d.ZAxis + rx).Normalized),
                    Trafo3d.RotateInto( V3d.XAxis, (-V3d.ZAxis + rx).Normalized),
                    Trafo3d.RotateInto(-V3d.YAxis, (-V3d.ZAxis + ry).Normalized),
                    Trafo3d.RotateInto( V3d.YAxis, (-V3d.ZAxis + ry).Normalized),
                };
                foreach (var r in r90)
                    Assert.True(CheckForwardBackwardConsistency(r));
            });
        }
        
        void ValidateTrafos(Trafo3d a, Trafo3d b)
        {
            var e = Mat.DistanceMax(a.Forward, b.Forward);
            Assert.IsTrue(e.Abs() < 1e-8, "not equal");

            var eq = CheckForwardBackwardConsistency(new Trafo3d(a.Forward, b.Backward))
                  && CheckForwardBackwardConsistency(new Trafo3d(b.Forward, a.Backward));

            Assert.True(eq, "trafo not consistent");
        }

        bool CheckForwardBackwardConsistency(Trafo3d trafo)
        {
            var i = trafo.Forward * trafo.Backward;
            // i should be Identity
            return i.C0.ApproximateEquals(V4d.IOOO, 1e-7)
                && i.C1.ApproximateEquals(V4d.OIOO, 1e-7)
                && i.C2.ApproximateEquals(V4d.OOIO, 1e-7)
                && i.C3.ApproximateEquals(V4d.OOOI, 1e-7);
        }
        
        [Test]
        public void CoordinateUnitsConvertMillimetersAndMeters()
        {
            foreach (var core in CoordinateSystemCores())
            foreach (var hand in new[] { CoordinateSystem.Handedness.Left, CoordinateSystem.Handedness.Right })
            foreach (var up in new[] { CoordinateSystem.Axis.X, CoordinateSystem.Axis.Y, CoordinateSystem.Axis.Z })
            {
                var millimeters = core.Info(.001, hand, up); var meters = core.Info(1, hand, up);
                var mmToM = core.FromTo(millimeters, meters); var mToMm = core.FromTo(meters, millimeters);
                var mm = new V3d(1000, -2500, 750); var m = new V3d(1, -2.5, .75);
                var context = $"{core.Name}, {hand}/{up}, millimeters/meters";
                Assert.AreEqual(m, core.Position(mmToM, false, mm), context + " mm -> m");
                Assert.AreEqual(mm, core.Position(mmToM, true, m), context + " mm <- m");
                Assert.AreEqual(mm, core.Position(mToMm, false, m), context + " m -> mm");
                Assert.AreEqual(m, core.Position(mToMm, true, mm), context + " m <- mm");
                CheckCoordinatePosition(core.Position(core.ToAardvark(millimeters), false, mm),
                    core.Position(core.ToAardvark(meters), false, m), context + " physical meters");
            }
        }

        [Test]
        public void CoordinateUnitPairsPreservePhysicalPositions()
        {
            foreach (var core in CoordinateSystemCores())
            foreach (var hand in new[] { CoordinateSystem.Handedness.Left, CoordinateSystem.Handedness.Right })
            foreach (var up in new[] { CoordinateSystem.Axis.X, CoordinateSystem.Axis.Y, CoordinateSystem.Axis.Z })
            foreach (var fromScale in CoordinateUnitScales)
            foreach (var toScale in CoordinateUnitScales)
            {
                if (fromScale == toScale) continue;
                var from = core.Info(fromScale, hand, up); var to = core.Info(toScale, hand, up);
                var direct = core.FromTo(from, to); var reverse = core.FromTo(to, from);
                var fromPhysical = core.ToAardvark(from); var toPhysical = core.ToAardvark(to);
                foreach (var point in CoordinateUnitPoints)
                {
                    var context = $"{core.Name}, {hand}/{up}, from={fromScale}, to={toScale}, point={point}";
                    // Independent physical-coordinate anchors, not merely mutually inverse ratios.
                    var sourceMeters = core.Position(fromPhysical, false, point);
                    var targetMeters = core.Position(toPhysical, false, point);
                    var expectedForward = core.Position(toPhysical, true, sourceMeters);
                    var expectedBackward = core.Position(fromPhysical, true, targetMeters);
                    var forward = core.Position(direct, false, point); var backward = core.Position(direct, true, point);
                    CheckCoordinatePosition(expectedForward, forward, context + " forward");
                    CheckCoordinatePosition(expectedBackward, backward, context + " backward");
                    CheckCoordinatePosition(expectedBackward, core.Position(reverse, false, point), context + " reverse forward");
                    CheckCoordinatePosition(expectedForward, core.Position(reverse, true, point), context + " reverse backward");
                    CheckCoordinatePosition(sourceMeters, core.Position(toPhysical, false, forward), context + " source physical meters");
                    CheckCoordinatePosition(targetMeters, core.Position(fromPhysical, false, backward), context + " target physical meters");
                }
            }
        }

        [Test]
        public void EqualCoordinateUnitsKeepIdentity()
        {
            foreach (var core in CoordinateSystemCores())
            foreach (var hand in new[] { CoordinateSystem.Handedness.Left, CoordinateSystem.Handedness.Right })
            foreach (var up in new[] { CoordinateSystem.Axis.X, CoordinateSystem.Axis.Y, CoordinateSystem.Axis.Z })
            foreach (var scale in CoordinateUnitScales)
            {
                var info = core.Info(scale, hand, up); var direct = core.FromTo(info, info);
                var context = $"{core.Name}, {hand}/{up}, equal scale={scale}";
                Assert.AreEqual(core.Identity, direct, context + " exact identity matrices");
                foreach (var point in CoordinateUnitPoints)
                {
                    Assert.AreEqual(point, core.Position(direct, false, point), context + " forward, point=" + point);
                    Assert.AreEqual(point, core.Position(direct, true, point), context + " backward, point=" + point);
                }
            }
        }

        private static readonly double[] CoordinateUnitScales = { .001, .01, .25, 1, 2, 10, 1000 };
        private static readonly V3d[] CoordinateUnitPoints =
            { new V3d(1, 2, 3), new V3d(-1, -2, -3), new V3d(1, -2, 3), new V3d(-.5, 1.25, -4) };

        private static void CheckCoordinatePosition(V3d expected, V3d actual, string context)
        {
            for (var i = 0; i < 3; i++)
                Assert.That(Math.Abs(actual[i] - expected[i]), Is.LessThanOrEqualTo(16 * Math.ScaleB(1, -53) * Math.Abs(expected[i])), context + ", component=" + i);
        }

        private sealed class CoordinateCore
        {
            public readonly string Name;
            public readonly object Identity;
            private readonly Type hand, axis;
            private readonly ConstructorInfo infoConstructor, pointConstructor;
            private readonly MethodInfo fromTo, toAardvark, transformPos;
            private readonly FieldInfo forward, backward;
            private readonly FieldInfo[] components;

            public CoordinateCore(Assembly assembly, string name)
            {
                Name = name;
                var coordinate = assembly.GetType("Aardvark.Base.CoordinateSystem", true);
                var info = coordinate.GetNestedType("Info"); hand = coordinate.GetNestedType("Handedness"); axis = coordinate.GetNestedType("Axis");
                var trafo = assembly.GetType("Aardvark.Base.Trafo3d", true);
                var matrix = assembly.GetType("Aardvark.Base.M44d", true); var vector = assembly.GetType("Aardvark.Base.V3d", true);
                foreach (var type in new[] { info, hand, axis, trafo, matrix, vector }) Assert.AreSame(assembly, type.Assembly, name);
                infoConstructor = info.GetConstructor(new[] { typeof(double), hand, axis });
                pointConstructor = vector.GetConstructor(new[] { typeof(double), typeof(double), typeof(double) });
                fromTo = coordinate.GetMethod("FromTo", new[] { info, info });
                toAardvark = coordinate.GetMethod("ToAardvark", new[] { info });
                transformPos = assembly.GetType("Aardvark.Base.Mat", true).GetMethod("TransformPos", new[] { matrix, vector });
                Assert.IsNotNull(transformPos, name + " Mat.TransformPos(M44d,V3d)");
                Assert.AreEqual(trafo, fromTo.ReturnType, name); Assert.AreEqual(vector, transformPos.ReturnType, name);
                forward = trafo.GetField("Forward"); backward = trafo.GetField("Backward");
                components = new[] { "X", "Y", "Z" }.Select(vector.GetField).ToArray();
                Identity = trafo.GetProperty("Identity").GetValue(null);
            }
            public object Info(double scale, CoordinateSystem.Handedness h, CoordinateSystem.Axis up)
                => infoConstructor.Invoke(new[] { (object)scale, Enum.ToObject(hand, (int)h), Enum.ToObject(axis, (int)up) });
            public object FromTo(object from, object to) => fromTo.Invoke(null, new[] { from, to });
            public object ToAardvark(object from) => toAardvark.Invoke(null, new[] { from });
            public V3d Position(object trafo, bool inverse, V3d point)
            {
                var input = pointConstructor.Invoke(new object[] { point.X, point.Y, point.Z });
                var result = transformPos.Invoke(null, new[] { (inverse ? backward : forward).GetValue(trafo), input });
                return new V3d((double)components[0].GetValue(result), (double)components[1].GetValue(result), (double)components[2].GetValue(result));
            }
        }

        private static IEnumerable<CoordinateCore> CoordinateSystemCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Trafo3d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new CoordinateCore(assembly, target);
            }
        }
        
        [Test]
        public static void NormalFrame()
        {
            TrafoTesting.GenericTest((rnd, i) =>
            {
                var n = rnd.UniformV3d().Normalized;

                var basis = M33d.NormalFrame(n);

                Assert.IsTrue(basis.C0.Length.ApproximateEquals(1, 1e-7));
                Assert.IsTrue(basis.C1.Length.ApproximateEquals(1, 1e-7));
                Assert.IsTrue(basis.C2.Length.ApproximateEquals(1, 1e-7));

                Assert.IsTrue(basis.C0.AngleBetween(basis.C1).ApproximateEquals(Constant.PiHalf, 1e-7));
                Assert.IsTrue(basis.C0.AngleBetween(basis.C2).ApproximateEquals(Constant.PiHalf, 1e-7));
                Assert.IsTrue(basis.C1.AngleBetween(basis.C2).ApproximateEquals(Constant.PiHalf, 1e-7));
            });
        }

        [Test]
        public static void OrthoNormalOrientation()
        {
            // Previous implementation
            Func<Trafo3d, Trafo3d> reference = trafo =>
            {
                var x = trafo.Forward.C0.XYZ.Normalized; // TransformDir(V3f.XAxis)
                var y = trafo.Forward.C1.XYZ.Normalized; // TransformDir(V3f.YAxis)
                var z = trafo.Forward.C2.XYZ.Normalized; // TransformDir(V3f.ZAxis)

                y = z.Cross(x).Normalized;
                z = x.Cross(y).Normalized;

                return Trafo3d.FromBasis(x, y, z, V3d.Zero);
            };

            var rnd = new RandomSystem(1);

            TrafoTesting.GenericTest(rnd =>
            {
                var trafo = TrafoTesting.GetRandomTrafo(rnd);

                var res = trafo.GetOrthoNormalOrientation();
                var res_ref = reference(trafo);

                TrafoTesting.AreEqual(res, res_ref);
            });
        }

        [Test]
        public static void TransformTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var trafo = TrafoTesting.GetRandomTrafo(rnd);
                var v3d = rnd.UniformV3d() * rnd.UniformInt(100);
                var v4d = rnd.UniformV4d() * rnd.UniformInt(100);

                Assert.AreEqual(trafo.Transform(v4d), trafo.Forward.Transform(v4d));
                Assert.AreEqual(trafo.TransformPos(v3d), trafo.Forward.TransformPos(v3d));
                Assert.AreEqual(trafo.TransformDir(v3d), trafo.Forward.TransformDir(v3d));
                Assert.AreEqual(trafo.TransformNormal(v3d), trafo.Backward.TransposedTransformDir(v3d));
            });
        }

        [Test]
        public static void InvTransformTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var trafo = TrafoTesting.GetRandomTrafo(rnd);
                var v3d = rnd.UniformV3d() * rnd.UniformInt(100);
                var v4d = rnd.UniformV4d() * rnd.UniformInt(100);

                Assert.AreEqual(trafo.InvTransform(v4d), trafo.Backward.Transform(v4d));
                Assert.AreEqual(trafo.InvTransformPos(v3d), trafo.Backward.TransformPos(v3d));
                Assert.AreEqual(trafo.InvTransformDir(v3d), trafo.Backward.TransformDir(v3d));
                Assert.AreEqual(trafo.InvTransformNormal(v3d), trafo.Forward.TransposedTransformDir(v3d));
            });
        }
    }
}

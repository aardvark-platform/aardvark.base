using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class GeoConversionTests
    {
        // WGS84 geographic 3D (EPSG:4979) to geocentric (EPSG:4978), independently generated with PROJ 9.5.1.
        private static readonly (V3d Geodetic, V3d Ecef)[] s_wgs84Fixtures =
        {
            (new V3d(0.0, 0.0, 0.0), new V3d(6378137.0, 0.0, 0.0)),
            (new V3d(45.0, 20.0, 125.0), new V3d(4239779.623816183, 4239779.623816182, 2167739.540346673)),
            (new V3d(135.0, 45.0, 2500.0), new V3d(-3195669.145060574, 3195669.145060575, 4489116.175818886)),
            (new V3d(-135.0, -30.0, -430.0), new V3d(-3908804.437636168, -3908804.437636169, -3170158.735383637)),
            (new V3d(-45.0, 70.0, 12000.0), new V3d(1550000.614702514, -1550000.614702514, 5982316.318567995)),
            (new V3d(179.5, 10.0, 400000.0), new V3d(-6675541.736955074, 58256.570170099, 1169707.818802134)),
            (new V3d(-73.9857, 40.7484, 15.0), new V3d(1334938.801255782, -4651103.377719582, 4141305.810633753)),
            (new V3d(33.0, 89.999999, 35786.0), new V3d(0.094198272598, 0.061173073491, 6392538.314245178)),
            (new V3d(-120.0, -89.999999, 50.0), new V3d(-0.055847425818, -0.096730578989, -6356802.314245178))
        };

        // Bessel 1841 transverse Mercator with lon_0=13°20', k=1, x_0=0, y_0=-5,000,000; PROJ 9.5.1.
        private static readonly (V3d Geodetic, V3d Plane)[] s_gaussKruegerFixtures =
        {
            (new V3d(13.333333333333334, 47.5, 123.0), new V3d(0.000000002516, 262298.7502174312, 123.0)),
            (new V3d(12.25, 47.0, 456.0), new V3d(-82383.66273394474, 207286.76686794497, 456.0)),
            (new V3d(14.75, 48.25, -20.0), new V3d(105193.47588708386, 346650.6186176641, -20.0))
        };

        private static void AssertClose(V3d actual, V3d expected, V3d tolerance, string context)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(tolerance.X), $"{context} X");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(tolerance.Y), $"{context} Y");
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(tolerance.Z), $"{context} Z");
        }

        private sealed class DatumApi
        {
            public readonly string Target;
            private readonly Type datumType;
            private readonly ConstructorInfo constructor, vector;
            private readonly MethodInfo transform;
            private readonly FieldInfo x, y, z;
            public DatumApi(string target)
            {
                Target = target;
                var path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(File.Exists(path), Is.True, path);
                var assembly = Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                datumType = assembly.GetType("Aardvark.Base.GeoDatum", true);
                var v = assembly.GetType("Aardvark.Base.V3d", true);
                constructor = datumType.GetConstructor(new[] { typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double), typeof(double) });
                vector = v.GetConstructor(new[] { typeof(double), typeof(double), typeof(double) });
                transform = assembly.GetType("Aardvark.Base.Geo", true).GetMethod("HelmertsTransformXyzToXyz", new[] { v, datumType });
                Assert.That(constructor.DeclaringType.Assembly, Is.SameAs(assembly));
                Assert.That(transform.DeclaringType.Assembly, Is.SameAs(assembly));
                Assert.That(transform.ReturnType, Is.EqualTo(v));
                x = v.GetField("X"); y = v.GetField("Y"); z = v.GetField("Z");
            }
            public object Create(double[] parameters) => constructor.Invoke(Array.ConvertAll(parameters, p => (object)p));
            public double Get(object datum, string name) => (double)datumType.GetProperty(name).GetValue(datum);
            public object Preset(string name) => datumType.GetField(name).GetValue(null);
            public V3d Transform(V3d point, object datum)
            {
                var result = transform.Invoke(null, new[] { vector.Invoke(new object[] { point.X, point.Y, point.Z }), datum });
                return new V3d((double)x.GetValue(result), (double)y.GetValue(result), (double)z.GetValue(result));
            }
        }

        private static DatumApi[] DatumImplementations() => new[] { new DatumApi("net10.0"), new DatumApi("netstandard2.0") };

        [Test]
        public void DatumConstructorRetainsAllSevenIndependentParameters()
        {
            var names = new[] { "dX", "dY", "dZ", "rXsec", "rYsec", "rZsec", "dS" };
            foreach (var api in DatumImplementations())
                for (int parameter = 0; parameter < 7; parameter++)
                    foreach (double value in new[] { -17.25, -0.0, 0.0, double.Epsilon, -double.Epsilon, double.MinValue, double.MaxValue, double.NegativeInfinity, double.PositiveInfinity, double.NaN })
                    {
                        var supplied = new[] { 101.25, -202.5, 303.75, -.125, .25, -.5, 1.75 };
                        supplied[parameter] = value;
                        var datum = api.Create(supplied);
                        void Check(double actual, double expected, string name)
                        {
                            var context = $"{api.Target} varied={names[parameter]} value={value:R} getter={name}";
                            if (double.IsNaN(expected)) Assert.That(double.IsNaN(actual), Is.True, context);
                            else Assert.That(BitConverter.DoubleToInt64Bits(actual), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), context);
                        }
                        for (int i = 0; i < names.Length; i++) Check(api.Get(datum, names[i]), supplied[i], names[i]);
                        for (int i = 0; i < 3; i++)
                            Check(api.Get(datum, new[] { "rXrad", "rYrad", "rZrad" }[i]), supplied[3 + i] * (Math.PI / (180 * 60 * 60)), "radians " + i);
                    }
        }

        [Test]
        public void HelmertPureTranslationsKeepIndependentSignedOffsets()
        {
            var offsets = new[] { new V3d(1, 2, 3), new V3d(-4, 5, -6), new V3d(7, -8, 9),
                new V3d(0, 0, 17), new V3d(0, -17, 0), new V3d(32, -.5, -.25), new V3d(3, -4, -4), V3d.Zero };
            var points = new[] { V3d.Zero, new V3d(8, -16, 32), new V3d(-100, 200, -300), new V3d(4e6, -1e6, 5e6) };
            foreach (var api in DatumImplementations())
                foreach (var offset in offsets)
                {
                    var datum = api.Create(new[] { offset.X, offset.Y, offset.Z, 0.0, 0.0, 0.0, 0.0 });
                    foreach (var point in points)
                        AssertClose(api.Transform(point, datum), point + offset, V3d.Zero, $"{api.Target} offset={offset} point={point}");
                }
        }

        [Test]
        public void AustriaDatumsUseTheirSuppliedTranslations()
        {
            var presets = new[] {
                ("Wgs84toAustriaNs1", new[] { -575.0, -93.0, -466.0, 5.1, 1.6, 5.2, -2.5 }),
                ("Wgs84toAustriaNs2", new[] { -577.326, -90.129, -463.919, 5.14, 1.47, 5.30, -2.4232 }),
                ("AustriaNs1toWgs84", new[] { 575.0, 93.0, 466.0, -5.1, -1.6, -5.2, 2.5 }),
                ("AustriaNs2toWgs84", new[] { 577.326, 90.129, 463.919, -5.14, -1.47, -5.30, 2.4232 }) };
            foreach (var api in DatumImplementations())
                foreach (var (name, supplied) in presets)
                {
                    var datum = api.Preset(name);
                    Assert.That(api.Get(datum, "dZ"), Is.EqualTo(supplied[2]), api.Target + " " + name + " Z offset");
                    AssertClose(api.Transform(V3d.Zero, datum), new V3d(supplied[0], supplied[1], supplied[2]), V3d.Zero, api.Target + " " + name + " origin");
                    var point = new V3d(4e6, -1e6, 5e6);
                    double rx = supplied[3] * (Math.PI / 648000), ry = supplied[4] * (Math.PI / 648000), rz = supplied[5] * (Math.PI / 648000);
                    double scale = 1 + supplied[6] * 1e-6;
                    var expected = new V3d(supplied[0] + scale * point.X + scale * rz * point.Y - scale * ry * point.Z,
                        supplied[1] - scale * rz * point.X + scale * point.Y + scale * rx * point.Z,
                        supplied[2] + scale * ry * point.X - scale * rx * point.Y + scale * point.Z);
                    AssertClose(api.Transform(point, datum), expected, new V3d(2e-9), api.Target + " " + name + " nonzero point");
                }
        }

        [Test]
        public void Wgs84ForwardMatchesIndependentEcefFixtures()
        {
            foreach ((V3d geodetic, V3d expected) in s_wgs84Fixtures)
            {
                V3d actual = Geo.XyzFromLonLatHeight(geodetic, GeoEllipsoid.Wgs84);
                AssertClose(actual, expected, new V3d(1e-6), $"geodetic {geodetic}");
            }
        }

        [Test]
        public void Wgs84InverseMatchesIndependentEcefFixturesInEveryQuadrant()
        {
            foreach ((V3d expected, V3d ecef) in s_wgs84Fixtures)
            {
                V3d actual = Geo.LonLatHeightFromXyz(ecef, GeoEllipsoid.Wgs84);
                AssertClose(actual, expected, new V3d(1e-9, 1e-9, 2e-5), $"ECEF {ecef}");
            }
        }

        [Test]
        public void Wgs84InverseDefinesExactPolesAndLeavesCenterUndefined()
        {
            double b = GeoEllipsoid.Wgs84.B;
            AssertClose(
                Geo.LonLatHeightFromXyz(new V3d(0.0, 0.0, b + 432.125), GeoEllipsoid.Wgs84),
                new V3d(0.0, 90.0, 432.125),
                new V3d(0.0, 0.0, 1e-9),
                "north pole"
            );
            AssertClose(
                Geo.LonLatHeightFromXyz(new V3d(0.0, 0.0, -b - 1200.5), GeoEllipsoid.Wgs84),
                new V3d(0.0, -90.0, 1200.5),
                new V3d(0.0, 0.0, 1e-9),
                "south pole"
            );

            V3d center = Geo.LonLatHeightFromXyz(V3d.Zero, GeoEllipsoid.Wgs84);
            Assert.That(double.IsNaN(center.X), Is.True);
            Assert.That(double.IsNaN(center.Y), Is.True);
            Assert.That(double.IsNaN(center.Z), Is.True);
        }

        [Test]
        public void Wgs84GeodeticEcefRoundTripsPreserveCoordinates()
        {
            foreach (var (expected, _) in s_wgs84Fixtures)
            {
                V3d ecef = Geo.XyzFromLonLatHeight(expected, GeoEllipsoid.Wgs84);
                V3d actual = Geo.LonLatHeightFromXyz(ecef, GeoEllipsoid.Wgs84);
                AssertClose(actual, expected, new V3d(1e-9, 1e-9, 2e-5), $"round trip {expected}");
            }
        }

        [Test]
        public void BesselGaussKruegerForwardMatchesProjControlPoints()
        {
            foreach ((V3d geodetic, V3d expected) in s_gaussKruegerFixtures)
            {
                V3d actual = Geo.GaussKruegerEllipsoidToPlane(
                    geodetic, GeoEllipsoid.Bessel1841, GeoConstant.AustriaM31
                );
                AssertClose(actual, expected, new V3d(5e-4, 5e-4, 0.0), $"forward {geodetic}");
            }
        }

        [Test]
        public void BesselGaussKruegerInverseMatchesProjControlPoints()
        {
            foreach ((V3d expected, V3d plane) in s_gaussKruegerFixtures)
            {
                V3d actual = Geo.GaussKruegerPlaneToEllipsoid(
                    plane, GeoEllipsoid.Bessel1841, GeoConstant.AustriaM31
                );
                AssertClose(actual, expected, new V3d(1e-9, 1e-9, 0.0), $"inverse {plane}");
            }
        }
    }
}

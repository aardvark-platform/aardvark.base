using System;
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

        private sealed class DirectApi
        {
            public readonly string Name;
            private readonly System.Reflection.ConstructorInfo vector, ellipsoid;
            private readonly System.Reflection.MethodInfo direct, directOnly, wgs, wgsOnly;
            private readonly System.Reflection.FieldInfo x, y;
            public DirectApi(string target)
            {
                Name = target;
                string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.dll"));
                Assert.That(System.IO.File.Exists(path), Is.True, path);
                var assembly = System.Reflection.Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(System.Reflection.CustomAttributeExtensions.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>(assembly).FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                var v = assembly.GetType("Aardvark.Base.V2d", true); var e = assembly.GetType("Aardvark.Base.GeoEllipsoid", true);
                var geo = assembly.GetType("Aardvark.Base.Geo", true); var number = typeof(double); var output = number.MakeByRefType();
                vector = v.GetConstructor(new[] { number, number }); ellipsoid = e.GetConstructor(new[] { number, number, number });
                direct = geo.GetMethod("DirectionVincenty", new[] { v, number, number, e, output });
                directOnly = geo.GetMethod("DirectionVincenty", new[] { v, number, number, e });
                wgs = geo.GetMethod("DirectionVincentyWgs84", new[] { v, number, number, output });
                wgsOnly = geo.GetMethod("DirectionVincentyWgs84", new[] { v, number, number });
                foreach (var m in new[] { direct, directOnly, wgs, wgsOnly })
                {
                    Assert.That(m.DeclaringType.Assembly, Is.SameAs(assembly), Name); Assert.That(m.ReturnType, Is.EqualTo(v), Name);
                }
                x = v.GetField("X"); y = v.GetField("Y");
            }
            public V3d Call(V2d start, double bearing, double distance, double a, double b, double f, int form = 0)
            {
                object point = vector.Invoke(new object[] { start.X, start.Y });
                object ell = ellipsoid.Invoke(new object[] { a, b, f });
                object[] args = form == 0 ? new[] { point, bearing, distance, ell, (object)0.0 } : form == 1 ? new[] { point, bearing, distance, ell } : form == 2 ? new[] { point, bearing, distance, (object)0.0 } : new[] { point, bearing, distance };
                object result = (form == 0 ? direct : form == 1 ? directOnly : form == 2 ? wgs : wgsOnly).Invoke(null, args);
                return new V3d((double)x.GetValue(result), (double)y.GetValue(result), form == 0 || form == 2 ? (double)args[args.Length - 1] : double.NaN);
            }
        }
        private static DirectApi[] DirectImplementations() => new[] { new DirectApi("net10.0"), new DirectApi("netstandard2.0") };
        private static readonly V2d[] s_directStarts = { new V2d(12, 45), new V2d(-179.75, -70), new V2d(-73.9857, 40.7484), V2d.Zero, new V2d(179.75, 70), new V2d(33, 85) };
        private static readonly double[] s_directBearings = { -170, -90, -30, 0, 30, 90, 135, 179.5 };
        private static V3d GreatCircle(V2d start, double bearing, double distance, double radius)
        {
            const double radians = Math.PI / 180;
            double lon = start.X * radians, lat = start.Y * radians, azimuth = bearing * radians, arc = distance / radius;
            var position = new V3d(Math.Cos(lat) * Math.Cos(lon), Math.Cos(lat) * Math.Sin(lon), Math.Sin(lat));
            var north = new V3d(-Math.Sin(lat) * Math.Cos(lon), -Math.Sin(lat) * Math.Sin(lon), Math.Cos(lat));
            var east = new V3d(-Math.Sin(lon), Math.Cos(lon), 0);
            var direction = north * Math.Cos(azimuth) + east * Math.Sin(azimuth);
            var destination = position * Math.Cos(arc) + direction * Math.Sin(arc);
            var heading = -position * Math.Sin(arc) + direction * Math.Cos(arc);
            double lon2 = Math.Atan2(destination.Y, destination.X), lat2 = Math.Atan2(destination.Z, Math.Sqrt(destination.X * destination.X + destination.Y * destination.Y));
            var north2 = new V3d(-Math.Sin(lat2) * Math.Cos(lon2), -Math.Sin(lat2) * Math.Sin(lon2), Math.Cos(lat2));
            var east2 = new V3d(-Math.Sin(lon2), Math.Cos(lon2), 0);
            return new V3d(lon2 / radians, lat2 / radians, Math.Atan2(heading.Dot(east2), heading.Dot(north2)) / radians);
        }
        private static void DirectClose(V3d actual, V3d expected, string context, bool bearing = true)
        {
            Assert.That(double.IsFinite(actual.X) && double.IsFinite(actual.Y), Is.True, context);
            Assert.That(Math.IEEERemainder(actual.X - expected.X, 360), Is.EqualTo(0).Within(1e-9), context + " longitude");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(1e-9), context + " latitude");
            if (bearing) Assert.That(Math.IEEERemainder(actual.Z - expected.Z, 360), Is.EqualTo(0).Within(1e-9), context + " final bearing");
        }
        private static void DirectDestinationBits(V3d actual, V3d expected, string context)
        {
            Assert.That(BitConverter.DoubleToInt64Bits(actual.X), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected.X)), context + " forwarded longitude");
            Assert.That(BitConverter.DoubleToInt64Bits(actual.Y), Is.EqualTo(BitConverter.DoubleToInt64Bits(expected.Y)), context + " forwarded latitude");
        }

        [Test]
        public void FullSphericalCircuitsRetainDestinationAndArrivalBearing()
        {
            foreach (var api in DirectImplementations())
            {
                var reported = api.Call(new V2d(12, 45), 30, 40030173.59204114, 6371000, 6371000, 0);
                DirectClose(reported, new V3d(12, 45, 30), api.Name + " reported full circuit");
                bool inside = false, outside = false;
                foreach (double radius in new[] { 1, 6371000, 6378137 })
                {
                    double circuit = 2 * Math.PI * radius;
                    var distances = new System.Collections.Generic.List<double>();
                    for (int neighbor = -3; neighbor <= 3; neighbor++)
                    {
                        double d = circuit;
                        for (int j = 0; j < Math.Abs(neighbor); j++) d = neighbor < 0 ? Math.BitDecrement(d) : Math.BitIncrement(d);
                        distances.Add(d);
                    }
                    foreach (double offset in new[] { -2e-12, -1e-12, -5e-13, 5e-13, 1e-12, 2e-12 }) distances.Add((2 * Math.PI + offset) * radius);
                    foreach (var start in s_directStarts) foreach (double bearing in s_directBearings) foreach (double distance in distances)
                    {
                        string context = $"{api.Name} sphere radius={radius:R} start={start} bearing={bearing:R} distance={distance:R}";
                        inside |= Math.Abs(distance / radius - 2 * Math.PI) <= 1e-12; outside |= Math.Abs(distance / radius - 2 * Math.PI) > 1e-12;
                        var actual = api.Call(start, bearing, distance, radius, radius, 0);
                        DirectClose(actual, GreatCircle(start, bearing, distance, radius), context);
                        DirectDestinationBits(api.Call(start, bearing, distance, radius, radius, 0, 1), actual, context);
                    }
                }
                Assert.That(inside && outside, Is.True, api.Name + " old sentinel boundary covered on both sides");
            }
        }

        [Test]
        public void Wgs84FullMeridiansAndForwardersRetainPositionAndBearing()
        {
            const double a = 6378137, f = 1 / 298.257223563;
            double b = a * (1 - f), e2 = f * (2 - f), sum = 0, correction = 0;
            const int count = 4096;
            // Independently integrate meridional curvature, rather than Vincenty's arc coefficient.
            for (int i = 0; i <= count; i++)
            {
                double phi = i * (Math.PI / 2 / count), sin = Math.Sin(phi);
                double term = (i == 0 || i == count ? 1 : i % 2 == 0 ? 2 : 4) * a * (1 - e2) / Math.Pow(1 - e2 * sin * sin, 1.5);
                double adjusted = term - correction, next = sum + adjusted; correction = (next - sum) - adjusted; sum = next;
            }
            double circuit = 4 * (Math.PI / 2 / count) * sum / 3;
            Assert.That(circuit, Is.EqualTo(40007862.9172509).Within(1e-6), "WGS84 complete meridian quadrature");
            foreach (var api in DirectImplementations())
                foreach (double lon in new[] { -179.75, 0, 12, 179.75 }) foreach (double lat in new[] { -70, -45, 0, 45, 70 }) foreach (double bearing in new[] { 0, 180, -180 })
                {
                    var start = new V2d(lon, lat); string context = $"{api.Name} WGS84 start={start} bearing={bearing:R} distance={circuit:R}";
                    var expected = new V3d(lon, lat, bearing); var actual = api.Call(start, bearing, circuit, a, b, f);
                    DirectClose(actual, expected, context);
                    for (int form = 1; form <= 3; form++)
                    {
                        var forwarded = api.Call(start, bearing, circuit, a, b, f, form);
                        DirectClose(forwarded, expected, context + " form=" + form, form == 2);
                        DirectDestinationBits(forwarded, actual, context + " form=" + form);
                        if (form == 2) Assert.That(BitConverter.DoubleToInt64Bits(forwarded.Z), Is.EqualTo(BitConverter.DoubleToInt64Bits(actual.Z)), context + " forwarded bearing");
                    }
                }
        }

        [Test]
        public void ZeroDistanceAndOrdinaryDirectRoutesKeepTheirContracts()
        {
            foreach (var api in DirectImplementations())
            {
                foreach (double radius in new[] { 1, 6371000, 6378137 }) foreach (var start in s_directStarts) foreach (double bearing in s_directBearings)
                    foreach (double arc in new[] { -2 * Math.PI, -1.5, -.25, 0, .125, .5, 1.75, 3, 8 })
                    {
                        double distance = arc * radius; string context = $"{api.Name} ordinary sphere radius={radius:R} start={start} bearing={bearing:R} arc={arc:R}";
                        var actual = api.Call(start, bearing, distance, radius, radius, 0);
                        DirectClose(actual, GreatCircle(start, bearing, distance, radius), context);
                        DirectDestinationBits(api.Call(start, bearing, distance, radius, radius, 0, 1), actual, context);
                    }
                const double a = 6378137, f = 1 / 298.257223563;
                foreach (double lon in new[] { -179.75, 0, 12, 179.75 }) foreach (double bearing in new[] { -90, 90 }) foreach (double distance in new[] { 0, 1, 1000, 1000000, 10000000, 2 * Math.PI * a })
                {
                    string context = $"{api.Name} WGS84 equator lon={lon:R} bearing={bearing:R} distance={distance:R}";
                    var expected = new V3d(lon + Math.Sign(bearing) * distance / a * (180 / Math.PI), 0, bearing);
                    var actual = api.Call(new V2d(lon, 0), bearing, distance, a, a * (1 - f), f);
                    DirectClose(actual, expected, context);
                    for (int form = 1; form <= 3; form++) DirectDestinationBits(api.Call(new V2d(lon, 0), bearing, distance, a, a * (1 - f), f, form), actual, context);
                }
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

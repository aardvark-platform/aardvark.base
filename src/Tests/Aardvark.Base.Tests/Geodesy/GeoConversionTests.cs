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

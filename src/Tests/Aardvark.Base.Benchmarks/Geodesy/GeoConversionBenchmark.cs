using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*GeoConversionBenchmark*'
    [MemoryDiagnoser]
    public class GeoConversionBenchmark
    {
        private const int Count = 1024;
        private readonly V3d[] m_geodetic = new V3d[Count];
        private readonly V3d[] m_ecef = new V3d[Count];
        private readonly V3d[] m_gaussKruegerGeodetic = new V3d[Count];
        private readonly V3d[] m_gaussKrueger = new V3d[Count];

        [GlobalSetup]
        public void Setup()
        {
            V3d[] geodetic =
            {
                new V3d(45.0, 20.0, 125.0),
                new V3d(135.0, 45.0, 2500.0),
                new V3d(-135.0, -30.0, -430.0),
                new V3d(-45.0, 70.0, 12000.0),
                new V3d(179.5, 10.0, 400000.0),
                new V3d(-73.9857, 40.7484, 15.0)
            };

            V3d[] ecef =
            {
                new V3d(4239779.623816183, 4239779.623816182, 2167739.540346673),
                new V3d(-3195669.145060574, 3195669.145060575, 4489116.175818886),
                new V3d(-3908804.437636168, -3908804.437636169, -3170158.735383637),
                new V3d(1550000.614702514, -1550000.614702514, 5982316.318567995),
                new V3d(-6675541.736955074, 58256.570170099, 1169707.818802134),
                new V3d(1334938.801255782, -4651103.377719582, 4141305.810633753)
            };

            V3d[] gaussKruegerGeodetic =
            {
                new V3d(13.333333333333334, 47.5, 123.0),
                new V3d(12.25, 47.0, 456.0),
                new V3d(14.75, 48.25, -20.0)
            };

            V3d[] gaussKrueger =
            {
                new V3d(0.0000000025, 262298.7502174312, 123.0),
                new V3d(-82383.6627339447, 207286.7668679450, 456.0),
                new V3d(105193.4758870839, 346650.6186176641, -20.0)
            };

            for (int i = 0; i < Count; i++)
            {
                m_geodetic[i] = geodetic[i % geodetic.Length];
                m_ecef[i] = ecef[i % ecef.Length];
                m_gaussKruegerGeodetic[i] = gaussKruegerGeodetic[i % gaussKruegerGeodetic.Length];
                m_gaussKrueger[i] = gaussKrueger[i % gaussKrueger.Length];
            }
        }

        [Benchmark]
        public V3d EcefForward()
        {
            V3d sum = V3d.Zero;
            for (int i = 0; i < Count; i++)
                sum += Geo.XyzFromLonLatHeight(m_geodetic[i], GeoEllipsoid.Wgs84);
            return sum;
        }

        [Benchmark]
        public V3d EcefInverse()
        {
            V3d sum = V3d.Zero;
            for (int i = 0; i < Count; i++)
                sum += Geo.LonLatHeightFromXyz(m_ecef[i], GeoEllipsoid.Wgs84);
            return sum;
        }

        [Benchmark]
        public V3d GaussKruegerForward()
        {
            V3d sum = V3d.Zero;
            for (int i = 0; i < Count; i++)
                sum += Geo.GaussKruegerEllipsoidToPlane(m_gaussKruegerGeodetic[i], GeoEllipsoid.Bessel1841, GeoConstant.AustriaM31);
            return sum;
        }

        [Benchmark]
        public V3d GaussKruegerInverse()
        {
            V3d sum = V3d.Zero;
            for (int i = 0; i < Count; i++)
                sum += Geo.GaussKruegerPlaneToEllipsoid(m_gaussKrueger[i], GeoEllipsoid.Bessel1841, GeoConstant.AustriaM31);
            return sum;
        }
    }
}

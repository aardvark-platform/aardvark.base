using BenchmarkDotNet.Attributes;
using System;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*PolygonCentroidBenchmark*'
    [MemoryDiagnoser]
    public class PolygonCentroidBenchmark
    {
        private Polygon2f m_polygon2f;
        private Polygon2d m_polygon2d;
        private Polygon3f m_polygon3f;
        private Polygon3d m_polygon3d;

        [Params(8, 64)]
        public int PointCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var points2f = new V2f[PointCount];
            var points2d = new V2d[PointCount];
            var points3f = new V3f[PointCount];
            var points3d = new V3d[PointCount];
            var origin = new V3d(3.0, -4.0, 2.0);
            var axisU = new V3d(2.0, 1.0, -1.0);
            var axisV = new V3d(-1.0, 3.0, 2.0);

            for (int i = 0; i < PointCount; i++)
            {
                var angle = Constant.PiTimesTwo * i / PointCount;
                var radius = 4.0 + 0.2 * Math.Sin(3.0 * angle);
                var p = new V2d(radius * Math.Cos(angle), radius * Math.Sin(angle));
                var q = origin + p.X * axisU + p.Y * axisV;
                points2d[i] = p;
                points2f[i] = (V2f)p;
                points3d[i] = q;
                points3f[i] = (V3f)q;
            }

            m_polygon2f = new Polygon2f(points2f);
            m_polygon2d = new Polygon2d(points2d);
            m_polygon3f = new Polygon3f(points3f);
            m_polygon3d = new Polygon3d(points3d);
        }

        [Benchmark]
        public V2f Polygon2fCentroid() => m_polygon2f.ComputeCentroid();

        [Benchmark]
        public V2d Polygon2dCentroid() => m_polygon2d.ComputeCentroid();

        [Benchmark]
        public V3f Polygon3fCentroid() => m_polygon3f.ComputeCentroid();

        [Benchmark]
        public V3d Polygon3dCentroid() => m_polygon3d.ComputeCentroid();
    }
}

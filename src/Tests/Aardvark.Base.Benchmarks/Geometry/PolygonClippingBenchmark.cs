using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.Geometry
{
    [MemoryDiagnoser]
    public class PolygonClippingBenchmark
    {
        private readonly Polygon2d m_polygon = new Polygon2d(
            new V2d(0.0, 0.0),
            new V2d(1.0, 0.0),
            new V2d(1.0, 1.0),
            new V2d(0.0, 1.0)
        );

        private readonly Line2d m_inside = new Line2d(
            new V2d(0.25, 0.25), new V2d(0.75, 0.75)
        );

        private readonly Line2d m_crossing = new Line2d(
            new V2d(-1.0, 0.5), new V2d(2.0, 0.5)
        );

        private readonly Line2d m_rejected = new Line2d(
            new V2d(-1.0, 2.0), new V2d(2.0, 2.0)
        );

        [Benchmark]
        public Line2d Inside() => m_inside.ClipWithConvex(m_polygon);

        [Benchmark]
        public Line2d Crossing() => m_crossing.ClipWithConvex(m_polygon);

        [Benchmark]
        public Line2d Rejected() => m_rejected.ClipWithConvex(m_polygon);
    }
}

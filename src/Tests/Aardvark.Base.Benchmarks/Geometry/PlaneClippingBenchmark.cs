using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

namespace Aardvark.Base.Benchmarks.Geometry
{
    public enum PlaneClippingScenario
    {
        Retained,
        Crossing,
        Rejected
    }

    [MemoryDiagnoser]
    [CategoriesColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PlaneClippingBenchmark
    {
        private readonly Plane2f m_plane2f = new Plane2f(V2f.XAxis, 0.0f);
        private readonly Plane2d m_plane2d = new Plane2d(V2d.XAxis, 0.0);
        private readonly Plane3f m_plane3f = new Plane3f(V3f.XAxis, 0.0f);
        private readonly Plane3d m_plane3d = new Plane3d(V3d.XAxis, 0.0);

        private Line2f m_line2f;
        private Line2d m_line2d;
        private Line3f m_line3f;
        private Line3d m_line3d;

        [ParamsAllValues]
        public PlaneClippingScenario Scenario { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            switch (Scenario)
            {
                case PlaneClippingScenario.Retained:
                    m_line2f = new Line2f(new V2f(1.0f, 2.0f), new V2f(3.0f, 4.0f));
                    m_line2d = new Line2d(new V2d(1.0, 2.0), new V2d(3.0, 4.0));
                    m_line3f = new Line3f(new V3f(1.0f, 2.0f, 3.0f), new V3f(3.0f, 4.0f, 5.0f));
                    m_line3d = new Line3d(new V3d(1.0, 2.0, 3.0), new V3d(3.0, 4.0, 5.0));
                    break;

                case PlaneClippingScenario.Crossing:
                    m_line2f = new Line2f(new V2f(-2.0f, 2.0f), new V2f(2.0f, 4.0f));
                    m_line2d = new Line2d(new V2d(-2.0, 2.0), new V2d(2.0, 4.0));
                    m_line3f = new Line3f(new V3f(-2.0f, 2.0f, 3.0f), new V3f(2.0f, 4.0f, 5.0f));
                    m_line3d = new Line3d(new V3d(-2.0, 2.0, 3.0), new V3d(2.0, 4.0, 5.0));
                    break;

                case PlaneClippingScenario.Rejected:
                    m_line2f = new Line2f(new V2f(-3.0f, 2.0f), new V2f(-1.0f, 4.0f));
                    m_line2d = new Line2d(new V2d(-3.0, 2.0), new V2d(-1.0, 4.0));
                    m_line3f = new Line3f(new V3f(-3.0f, 2.0f, 3.0f), new V3f(-1.0f, 4.0f, 5.0f));
                    m_line3d = new Line3d(new V3d(-3.0, 2.0, 3.0), new V3d(-1.0, 4.0, 5.0));
                    break;
            }
        }

        [Benchmark(Baseline = true), BenchmarkCategory("Line2f")]
        public Line2f Line2fExplicitTolerance()
            => m_line2f.ClipByPlane(m_plane2f, Constant<float>.PositiveTinyValue);

        [Benchmark, BenchmarkCategory("Line2f")]
        public Line2f Line2fDefaultTolerance()
            => m_line2f.ClipByPlane(m_plane2f);

        [Benchmark(Baseline = true), BenchmarkCategory("Line2d")]
        public Line2d Line2dExplicitTolerance()
            => m_line2d.ClipByPlane(m_plane2d, Constant<double>.PositiveTinyValue);

        [Benchmark, BenchmarkCategory("Line2d")]
        public Line2d Line2dDefaultTolerance()
            => m_line2d.ClipByPlane(m_plane2d);

        [Benchmark(Baseline = true), BenchmarkCategory("Line3f")]
        public Line3f Line3fExplicitTolerance()
            => m_line3f.ClipByPlane(m_plane3f, Constant<float>.PositiveTinyValue);

        [Benchmark, BenchmarkCategory("Line3f")]
        public Line3f Line3fDefaultTolerance()
            => m_line3f.ClipByPlane(m_plane3f);

        [Benchmark(Baseline = true), BenchmarkCategory("Line3d")]
        public Line3d Line3dExplicitTolerance()
            => m_line3d.ClipByPlane(m_plane3d, Constant<double>.PositiveTinyValue);

        [Benchmark, BenchmarkCategory("Line3d")]
        public Line3d Line3dDefaultTolerance()
            => m_line3d.ClipByPlane(m_plane3d);
    }
}

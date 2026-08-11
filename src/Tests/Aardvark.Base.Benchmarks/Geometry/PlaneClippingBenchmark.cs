using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.Geometry
{
    public enum PlaneClippingScenario
    {
        Retained,
        Crossing,
        Rejected
    }

    [MemoryDiagnoser]
    public class PlaneClipping2dBenchmark
    {
        private readonly Plane2d m_plane = new Plane2d(V2d.XAxis, 0.0);
        private Line2d m_line;

        [Params(
            PlaneClippingScenario.Retained,
            PlaneClippingScenario.Crossing,
            PlaneClippingScenario.Rejected
        )]
        public PlaneClippingScenario Scenario { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            m_line = Scenario switch
            {
                PlaneClippingScenario.Retained => new Line2d(new V2d(1.0, 2.0), new V2d(3.0, 4.0)),
                PlaneClippingScenario.Crossing => new Line2d(new V2d(-2.0, 2.0), new V2d(2.0, 4.0)),
                PlaneClippingScenario.Rejected => new Line2d(new V2d(-3.0, 2.0), new V2d(-1.0, 4.0)),
                _ => default
            };
        }

        [Benchmark(Baseline = true)]
        public Line2d DirectScalar()
            => ClipDirect(m_line, m_plane, Constant<double>.PositiveTinyValue);

        [Benchmark]
        public Line2d Extension()
            => m_line.ClipByPlane(m_plane);

        private static Line2d ClipDirect(Line2d line, Plane2d plane, double absoluteEpsilon)
        {
            var normalLength = plane.Normal.Length;
            if (normalLength == 0.0) return line;

            var boundary = -absoluteEpsilon * normalLength;
            var h0 = plane.Height(line.P0);
            var h1 = plane.Height(line.P1);
            var p0Inside = h0 >= boundary;
            var p1Inside = h1 >= boundary;

            if (p0Inside)
            {
                if (p1Inside) return line;
                if (h0 == boundary) return new Line2d(line.P0, line.P0);

                var t = (boundary - h0) / (h1 - h0);
                return new Line2d(line.P0, line.P0 + t * (line.P1 - line.P0));
            }

            if (!p1Inside) return new Line2d(V2d.NaN, V2d.NaN);
            if (h1 == boundary) return new Line2d(line.P1, line.P1);

            var t0 = (boundary - h0) / (h1 - h0);
            return new Line2d(line.P0 + t0 * (line.P1 - line.P0), line.P1);
        }
    }

    [MemoryDiagnoser]
    public class PlaneClipping3dBenchmark
    {
        private readonly Plane3d m_plane = new Plane3d(V3d.XAxis, 0.0);
        private Line3d m_line;

        [Params(
            PlaneClippingScenario.Retained,
            PlaneClippingScenario.Crossing,
            PlaneClippingScenario.Rejected
        )]
        public PlaneClippingScenario Scenario { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            m_line = Scenario switch
            {
                PlaneClippingScenario.Retained => new Line3d(new V3d(1.0, 2.0, 3.0), new V3d(3.0, 4.0, 5.0)),
                PlaneClippingScenario.Crossing => new Line3d(new V3d(-2.0, 2.0, 3.0), new V3d(2.0, 4.0, 5.0)),
                PlaneClippingScenario.Rejected => new Line3d(new V3d(-3.0, 2.0, 3.0), new V3d(-1.0, 4.0, 5.0)),
                _ => default
            };
        }

        [Benchmark(Baseline = true)]
        public Line3d DirectScalar()
            => ClipDirect(m_line, m_plane, Constant<double>.PositiveTinyValue);

        [Benchmark]
        public Line3d Extension()
            => m_line.ClipByPlane(m_plane);

        private static Line3d ClipDirect(Line3d line, Plane3d plane, double absoluteEpsilon)
        {
            var normalLength = plane.Normal.Length;
            if (normalLength == 0.0) return line;

            var boundary = -absoluteEpsilon * normalLength;
            var h0 = plane.Height(line.P0);
            var h1 = plane.Height(line.P1);
            var p0Inside = h0 >= boundary;
            var p1Inside = h1 >= boundary;

            if (p0Inside)
            {
                if (p1Inside) return line;
                if (h0 == boundary) return new Line3d(line.P0, line.P0);

                var t = (boundary - h0) / (h1 - h0);
                return new Line3d(line.P0, line.P0 + t * (line.P1 - line.P0));
            }

            if (!p1Inside) return new Line3d(V3d.NaN, V3d.NaN);
            if (h1 == boundary) return new Line3d(line.P1, line.P1);

            var t0 = (boundary - h0) / (h1 - h0);
            return new Line3d(line.P0 + t0 * (line.P1 - line.P0), line.P1);
        }
    }
}

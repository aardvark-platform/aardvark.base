using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*BoxPlaneIntersectionBenchmark*'
    [MemoryDiagnoser]
    public class BoxPlaneIntersectionBenchmark
    {
        private static readonly Box2f s_boxFloat = new(new V2f(0.125f, -0.5f), new V2f(2.25f, 1.75f));
        private static readonly Plane2f s_axisFloat = new(new V2f(0.0f, 1.0f), 0.75f);
        private static readonly Plane2f s_generalFloat = new(new V2f(0.6f, -0.8f), 0.2f);

        private static readonly Box2d s_boxDouble = new(new V2d(0.125, -0.5), new V2d(2.25, 1.75));
        private static readonly Plane2d s_axisDouble = new(new V2d(0.0, 1.0), 0.75);
        private static readonly Plane2d s_generalDouble = new(new V2d(0.6, -0.8), 0.2);

        [Benchmark]
        public bool BooleanFloatAxis()
            => s_boxFloat.Intersects(s_axisFloat);

        [Benchmark]
        public bool BooleanFloatGeneral()
            => s_boxFloat.Intersects(s_generalFloat);

        [Benchmark]
        public Line2f SegmentFloatAxis()
        {
            s_boxFloat.Intersects(s_axisFloat, out Line2f line);
            return line;
        }

        [Benchmark]
        public Line2f SegmentFloatGeneral()
        {
            s_boxFloat.Intersects(s_generalFloat, out Line2f line);
            return line;
        }

        [Benchmark]
        public bool BooleanDoubleAxis()
            => s_boxDouble.Intersects(s_axisDouble);

        [Benchmark]
        public bool BooleanDoubleGeneral()
            => s_boxDouble.Intersects(s_generalDouble);

        [Benchmark]
        public Line2d SegmentDoubleAxis()
        {
            s_boxDouble.Intersects(s_axisDouble, out Line2d line);
            return line;
        }

        [Benchmark]
        public Line2d SegmentDoubleGeneral()
        {
            s_boxDouble.Intersects(s_generalDouble, out Line2d line);
            return line;
        }
    }
}

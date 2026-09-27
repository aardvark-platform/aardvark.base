using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*Circle3Benchmark*'
    [MemoryDiagnoser]
    public class Circle3Benchmark
    {
        private Circle3f m_generalFloat;
        private Circle3f m_axisFloat;
        private Circle3d m_generalDouble;
        private Circle3d m_axisDouble;

        [GlobalSetup]
        public void Setup()
        {
            m_generalFloat = new Circle3f(
                new V3f(1.25f, -2.5f, 4.0f), new V3f(1.0f, 2.0f, 3.0f).Normalized, 3.5f);
            m_axisFloat = new Circle3f(
                new V3f(1.25f, -2.5f, 4.0f), V3f.ZAxis, 3.5f);
            m_generalDouble = new Circle3d(
                new V3d(1.25, -2.5, 4.0), new V3d(1.0, 2.0, 3.0).Normalized, 3.5);
            m_axisDouble = new Circle3d(
                new V3d(1.25, -2.5, 4.0), V3d.ZAxis, 3.5);
        }

        [Benchmark]
        public V3f RepresentativePointFloat() => m_generalFloat.Point;

        [Benchmark]
        public V3f TangentAxesFloat() => m_generalFloat.AxisU + m_generalFloat.AxisV;

        [Benchmark]
        public V3f GetPointFloat() => m_generalFloat.GetPoint(1.2345f);

        [Benchmark]
        public Box3f GeneralBoundsFloat() => m_generalFloat.BoundingBox3f;

        [Benchmark]
        public Box3f AxisAlignedBoundsFloat() => m_axisFloat.BoundingBox3f;

        [Benchmark]
        public V3d RepresentativePointDouble() => m_generalDouble.Point;

        [Benchmark]
        public V3d TangentAxesDouble() => m_generalDouble.AxisU + m_generalDouble.AxisV;

        [Benchmark]
        public V3d GetPointDouble() => m_generalDouble.GetPoint(1.2345);

        [Benchmark]
        public Box3d GeneralBoundsDouble() => m_generalDouble.BoundingBox3d;

        [Benchmark]
        public Box3d AxisAlignedBoundsDouble() => m_axisDouble.BoundingBox3d;
    }
}

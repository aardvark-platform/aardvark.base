using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

namespace Aardvark.Base.Benchmarks.Geometry
{
    [MemoryDiagnoser]
    [CategoriesColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class FastRayBenchmark
    {
        private readonly FastRay2f m_ray2f = new FastRay2f(new V2f(-1.0f, -1.0f), V2f.II);
        private readonly FastRay2d m_ray2d = new FastRay2d(new V2d(-1.0, -1.0), V2d.II);
        private readonly FastRay3f m_ray3f = new FastRay3f(new V3f(-1.0f, -1.0f, -1.0f), V3f.III);
        private readonly FastRay3d m_ray3d = new FastRay3d(new V3d(-1.0, -1.0, -1.0), V3d.III);

        private readonly Box2f m_box2f = new Box2f(V2f.Zero, V2f.II);
        private readonly Box2d m_box2d = new Box2d(V2d.Zero, V2d.II);
        private readonly Box3f m_box3f = new Box3f(V3f.Zero, V3f.III);
        private readonly Box3d m_box3d = new Box3d(V3d.Zero, V3d.III);

        [Benchmark(Baseline = true), BenchmarkCategory("FastRay2f")]
        public float Intersects2f()
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;
            return m_ray2f.Intersects(m_box2f, ref tmin, ref tmax) ? tmin + tmax : -1.0f;
        }

        [Benchmark, BenchmarkCategory("FastRay2f")]
        public float Intersects2fWithFlags()
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;
            return m_ray2f.Intersects(m_box2f, ref tmin, ref tmax, out var minFlags, out var maxFlags)
                ? tmin + tmax + (int)(minFlags | maxFlags)
                : -1.0f;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("FastRay2d")]
        public double Intersects2d()
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;
            return m_ray2d.Intersects(m_box2d, ref tmin, ref tmax) ? tmin + tmax : -1.0;
        }

        [Benchmark, BenchmarkCategory("FastRay2d")]
        public double Intersects2dWithFlags()
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;
            return m_ray2d.Intersects(m_box2d, ref tmin, ref tmax, out var minFlags, out var maxFlags)
                ? tmin + tmax + (int)(minFlags | maxFlags)
                : -1.0;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("FastRay3f")]
        public float Intersects3f()
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;
            return m_ray3f.Intersects(m_box3f, ref tmin, ref tmax) ? tmin + tmax : -1.0f;
        }

        [Benchmark, BenchmarkCategory("FastRay3f")]
        public float Intersects3fWithFlags()
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;
            return m_ray3f.Intersects(m_box3f, ref tmin, ref tmax, out var minFlags, out var maxFlags)
                ? tmin + tmax + (int)(minFlags | maxFlags)
                : -1.0f;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("FastRay3d")]
        public double Intersects3d()
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;
            return m_ray3d.Intersects(m_box3d, ref tmin, ref tmax) ? tmin + tmax : -1.0;
        }

        [Benchmark, BenchmarkCategory("FastRay3d")]
        public double Intersects3dWithFlags()
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;
            return m_ray3d.Intersects(m_box3d, ref tmin, ref tmax, out var minFlags, out var maxFlags)
                ? tmin + tmax + (int)(minFlags | maxFlags)
                : -1.0;
        }
    }
}

using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*CylinderIntersectionBenchmark*'
    [MemoryDiagnoser]
    public class CylinderIntersectionBenchmark
    {
        private const int Count = 1024;

        private static readonly V3f P0f = V3f.Zero;
        private static readonly V3f P1f = 4.0f * V3f.ZAxis;
        private static readonly Ray3f BarrelRayF = new(new V3f(-3.0f, 0.0f, 0.5f), new V3f(2.0f, 0.0f, 1.0f));
        private static readonly Ray3f CapRayF = new(new V3f(0.0f, 0.0f, 6.0f), new V3f(0.25f, 0.0f, -1.0f));
        private static readonly Ray3f ParallelRayF = new(new V3f(0.5f, 0.0f, -2.0f), V3f.ZAxis);
        private static readonly Ray3f MissRayF = new(new V3f(2.0f, 0.0f, -2.0f), V3f.ZAxis);

        private static readonly V3d P0d = V3d.Zero;
        private static readonly V3d P1d = 4.0 * V3d.ZAxis;
        private static readonly Ray3d BarrelRayD = new(new V3d(-3.0, 0.0, 0.5), new V3d(2.0, 0.0, 1.0));
        private static readonly Ray3d CapRayD = new(new V3d(0.0, 0.0, 6.0), new V3d(0.25, 0.0, -1.0));
        private static readonly Ray3d ParallelRayD = new(new V3d(0.5, 0.0, -2.0), V3d.ZAxis);
        private static readonly Ray3d MissRayD = new(new V3d(2.0, 0.0, -2.0), V3d.ZAxis);

        [Benchmark]
        public float BarrelFloat() => Intersect(BarrelRayF);

        [Benchmark]
        public float CapFloat() => Intersect(CapRayF);

        [Benchmark]
        public float ParallelFloat() => Intersect(ParallelRayF);

        [Benchmark]
        public float MissFloat() => Intersect(MissRayF);

        [Benchmark]
        public double BarrelDouble() => Intersect(BarrelRayD);

        [Benchmark]
        public double CapDouble() => Intersect(CapRayD);

        [Benchmark]
        public double ParallelDouble() => Intersect(ParallelRayD);

        [Benchmark]
        public double MissDouble() => Intersect(MissRayD);

        private static float Intersect(Ray3f ray)
        {
            var sum = 0.0f;
            for (var i = 0; i < Count; i++)
                if (ray.HitsCylinder(P0f, P1f, 1.0f, 0.0f, float.MaxValue, out var t))
                    sum += t;
            return sum;
        }

        private static double Intersect(Ray3d ray)
        {
            var sum = 0.0;
            for (var i = 0; i < Count; i++)
                if (ray.HitsCylinder(P0d, P1d, 1.0, 0.0, double.MaxValue, out var t))
                    sum += t;
            return sum;
        }
    }
}

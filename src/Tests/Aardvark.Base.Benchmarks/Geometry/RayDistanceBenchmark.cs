using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

namespace Aardvark.Base.Benchmarks.Geometry
{
    [MemoryDiagnoser]
    [CategoriesColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class RayDistanceBenchmark
    {
        private readonly Ray3f m_pointRayF = new Ray3f(
            new V3f(1.0f, -2.0f, 0.5f), new V3f(2.0f, 0.5f, -1.0f)
        );
        private readonly V3f m_pointF = new V3f(4.6f, -1.525f, -1.25f);

        private readonly Ray3d m_pointRayD = new Ray3d(
            new V3d(1.0, -2.0, 0.5), new V3d(2.0, 0.5, -1.0)
        );
        private readonly V3d m_pointD = new V3d(4.6, -1.525, -1.25);

        private readonly Ray2f m_ray2f0 = new Ray2f(new V2f(1.0f, -2.0f), new V2f(2.0f, 0.5f));
        private readonly Ray2f m_ray2f1 = new Ray2f(new V2f(-0.5f, 1.5f), new V2f(-0.75f, 1.25f));
        private readonly Ray2d m_ray2d0 = new Ray2d(new V2d(1.0, -2.0), new V2d(2.0, 0.5));
        private readonly Ray2d m_ray2d1 = new Ray2d(new V2d(-0.5, 1.5), new V2d(-0.75, 1.25));

        private readonly Ray3f m_ray3f0 = new Ray3f(
            new V3f(1.0f, -2.0f, 0.5f), new V3f(2.0f, 0.5f, -1.0f)
        );
        private readonly Ray3f m_ray3f1 = new Ray3f(
            new V3f(-0.5f, 1.5f, 2.0f), new V3f(-0.75f, 1.25f, 0.25f)
        );
        private readonly Ray3d m_ray3d0 = new Ray3d(
            new V3d(1.0, -2.0, 0.5), new V3d(2.0, 0.5, -1.0)
        );
        private readonly Ray3d m_ray3d1 = new Ray3d(
            new V3d(-0.5, 1.5, 2.0), new V3d(-0.75, 1.25, 0.25)
        );

        private static float PreviousPointRay(V3f point, Ray3f ray, out float t)
        {
            var a = point - ray.Origin;
            var lengthSquared = ray.Direction.LengthSquared;
            var normalSquared = Vec.Cross(a, ray.Direction).LengthSquared / lengthSquared;
            var parallelSquared = lengthSquared - normalSquared;

            t = Fun.Sqrt(parallelSquared / lengthSquared);
            return Fun.Sqrt(normalSquared);
        }

        private static double PreviousPointRay(V3d point, Ray3d ray, out double t)
        {
            var a = point - ray.Origin;
            var lengthSquared = ray.Direction.LengthSquared;
            var normalSquared = Vec.Cross(a, ray.Direction).LengthSquared / lengthSquared;
            var parallelSquared = lengthSquared - normalSquared;

            t = Fun.Sqrt(parallelSquared / lengthSquared);
            return Fun.Sqrt(normalSquared);
        }

        private static float PreviousRayPair(Ray2f ray0, Ray2f ray1, out float t0, out float t1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;
            var uDotv = u.Dot(v);

            if (uDotv.Abs().ApproximateEquals(1.0f, Constant<float>.PositiveTinyValue))
            {
                t1 = 0.0f;
                t0 = Vec.Dot(ray1.Origin - ray0.Origin, ray0.Direction) / ray0.Direction.LengthSquared;
            }
            else
            {
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1.0f);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 /= ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        private static double PreviousRayPair(Ray2d ray0, Ray2d ray1, out double t0, out double t1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;
            var uDotv = u.Dot(v);

            if (uDotv.Abs().ApproximateEquals(1.0, Constant<double>.PositiveTinyValue))
            {
                t1 = 0.0;
                t0 = Vec.Dot(ray1.Origin - ray0.Origin, ray0.Direction) / ray0.Direction.LengthSquared;
            }
            else
            {
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1.0);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 /= ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        private static float PreviousRayPair(Ray3f ray0, Ray3f ray1, out float t0, out float t1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;
            var uDotv = u.Dot(v);

            if (uDotv.Abs().ApproximateEquals(1.0f, Constant<float>.PositiveTinyValue))
            {
                t1 = 0.0f;
                t0 = Vec.Dot(ray1.Origin - ray0.Origin, ray0.Direction) / ray0.Direction.LengthSquared;
            }
            else
            {
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1.0f);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 /= ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        private static double PreviousRayPair(Ray3d ray0, Ray3d ray1, out double t0, out double t1)
        {
            var a = ray0.Origin - ray1.Origin;
            var u = ray0.Direction.Normalized;
            var v = ray1.Direction.Normalized;
            var uDotv = u.Dot(v);

            if (uDotv.Abs().ApproximateEquals(1.0, Constant<double>.PositiveTinyValue))
            {
                t1 = 0.0;
                t0 = Vec.Dot(ray1.Origin - ray0.Origin, ray0.Direction) / ray0.Direction.LengthSquared;
            }
            else
            {
                t1 = (a.Dot(u) * uDotv - a.Dot(v)) / (uDotv * uDotv - 1.0);
                t0 = (t1 * uDotv - a.Dot(u)) / ray0.Direction.Length;
                t1 /= ray1.Direction.Length;
            }

            return (t1 * ray1.Direction - a - t0 * ray0.Direction).Length;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("PointRayFloat")]
        public float PreviousPointRayFloat()
        {
            var distance = PreviousPointRay(m_pointF, m_pointRayF, out var t);
            return distance + t;
        }

        [Benchmark, BenchmarkCategory("PointRayFloat")]
        public float CurrentPointRayFloat()
        {
            var distance = m_pointF.GetMinimalDistanceTo(m_pointRayF, out var t);
            return distance + t;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("PointRayDouble")]
        public double PreviousPointRayDouble()
        {
            var distance = PreviousPointRay(m_pointD, m_pointRayD, out var t);
            return distance + t;
        }

        [Benchmark, BenchmarkCategory("PointRayDouble")]
        public double CurrentPointRayDouble()
        {
            var distance = m_pointD.GetMinimalDistanceTo(m_pointRayD, out var t);
            return distance + t;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("RayPair2f")]
        public float PreviousRayPair2f()
        {
            var distance = PreviousRayPair(m_ray2f0, m_ray2f1, out var t0, out var t1);
            return distance + t0 + t1;
        }

        [Benchmark, BenchmarkCategory("RayPair2f")]
        public float CurrentRayPair2f()
        {
            var distance = m_ray2f0.GetMinimalDistanceTo(m_ray2f1, out var t0, out var t1);
            return distance + t0 + t1;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("RayPair2d")]
        public double PreviousRayPair2d()
        {
            var distance = PreviousRayPair(m_ray2d0, m_ray2d1, out var t0, out var t1);
            return distance + t0 + t1;
        }

        [Benchmark, BenchmarkCategory("RayPair2d")]
        public double CurrentRayPair2d()
        {
            var distance = m_ray2d0.GetMinimalDistanceTo(m_ray2d1, out var t0, out var t1);
            return distance + t0 + t1;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("RayPair3f")]
        public float PreviousRayPair3f()
        {
            var distance = PreviousRayPair(m_ray3f0, m_ray3f1, out var t0, out var t1);
            return distance + t0 + t1;
        }

        [Benchmark, BenchmarkCategory("RayPair3f")]
        public float CurrentRayPair3f()
        {
            var distance = m_ray3f0.GetMinimalDistanceTo(m_ray3f1, out var t0, out var t1);
            return distance + t0 + t1;
        }

        [Benchmark(Baseline = true), BenchmarkCategory("RayPair3d")]
        public double PreviousRayPair3d()
        {
            var distance = PreviousRayPair(m_ray3d0, m_ray3d1, out var t0, out var t1);
            return distance + t0 + t1;
        }

        [Benchmark, BenchmarkCategory("RayPair3d")]
        public double CurrentRayPair3d()
        {
            var distance = m_ray3d0.GetMinimalDistanceTo(m_ray3d1, out var t0, out var t1);
            return distance + t0 + t1;
        }
    }
}

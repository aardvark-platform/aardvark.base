using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*TriangleBoundingVolumeBenchmark*'
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class TriangleBoundingVolumeBenchmark
    {
        private const int Count = 256;
        private Triangle2f[] _t2f;
        private Triangle2d[] _t2d;
        private Triangle3f[] _t3f;
        private Triangle3d[] _t3d;

        [Params("Acute", "Right", "Obtuse", "Mixed", "Exceptional")]
        public string Case { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _t2f = new Triangle2f[Count]; _t2d = new Triangle2d[Count];
            _t3f = new Triangle3f[Count]; _t3d = new Triangle3d[Count];
            var random = new Random(6151);
            for (int i = 0; i < Count; i++)
            {
                int shape = Case == "Acute" ? 0 : Case == "Right" ? 1 : Case == "Obtuse" ? 2 : i % 3;
                var c = shape == 0 ? new V2d(1, 2) : shape == 1 ? new V2d(0, 2) : new V2d(1, 0.25);
                var origin = new V2d(random.Next(-32, 33) * 0.25, random.Next(-32, 33) * 0.25);
                double scale = Math.Pow(2, i % 3 - 1);
                V2d Map(V2d p) => origin + scale * ((i & 1) == 0 ? p : new V2d(-p.Y, p.X));
                var a = Map(V2d.Zero); var b = Map(new V2d(2, 0)); c = Map(c);
                var af = (V2f)a; var bf = (V2f)b; var cf = (V2f)c;
                if (Case == "Exceptional")
                {
                    double sd = Math.Pow(2, (i & 1) == 0 ? 600 : -600);
                    float sf = (float)Math.Pow(2, (i & 1) == 0 ? 80 : -80);
                    a *= sd; b *= sd; c *= sd;
                    af *= sf; bf *= sf; cf *= sf;
                }
                _t2d[i] = new Triangle2d(a, b, c);
                _t2f[i] = new Triangle2f(af, bf, cf);
                V3d Embed(V2d p) => p.X * new V3d(0.6, 0.8, 0) + p.Y * new V3d(-0.48, 0.36, 0.8);
                V3f Embedf(V2f p) => p.X * new V3f(0.6f, 0.8f, 0) + p.Y * new V3f(-0.48f, 0.36f, 0.8f);
                _t3d[i] = new Triangle3d(Embed(a), Embed(b), Embed(c));
                _t3f[i] = new Triangle3f(Embedf(af), Embedf(bf), Embedf(cf));
            }
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("2D float")]
        public float Previous2f()
        {
            float sum = 0;
            for (int i = 0; i < Count; i++) { var b = Previous(in _t2f[i]); sum += b.Center.X + b.Radius; }
            return sum;
        }
        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("2D float")]
        public float Revised2f()
        {
            float sum = 0;
            for (int i = 0; i < Count; i++) { var b = _t2f[i].BoundingCircle2f; sum += b.Center.X + b.Radius; }
            return sum;
        }
        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("2D double")]
        public double Previous2d()
        {
            double sum = 0;
            for (int i = 0; i < Count; i++) { var b = Previous(in _t2d[i]); sum += b.Center.X + b.Radius; }
            return sum;
        }
        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("2D double")]
        public double Revised2d()
        {
            double sum = 0;
            for (int i = 0; i < Count; i++) { var b = _t2d[i].BoundingCircle2d; sum += b.Center.X + b.Radius; }
            return sum;
        }
        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("3D float")]
        public float Previous3f()
        {
            float sum = 0;
            for (int i = 0; i < Count; i++) { var b = Previous(in _t3f[i]); sum += b.Center.X + b.Radius; }
            return sum;
        }
        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("3D float")]
        public float Revised3f()
        {
            float sum = 0;
            for (int i = 0; i < Count; i++) { var b = _t3f[i].BoundingSphere3f; sum += b.Center.X + b.Radius; }
            return sum;
        }
        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("3D double")]
        public double Previous3d()
        {
            double sum = 0;
            for (int i = 0; i < Count; i++) { var b = Previous(in _t3d[i]); sum += b.Center.X + b.Radius; }
            return sum;
        }
        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("3D double")]
        public double Revised3d()
        {
            double sum = 0;
            for (int i = 0; i < Count; i++) { var b = _t3d[i].BoundingSphere3d; sum += b.Center.X + b.Radius; }
            return sum;
        }

        // Original property bodies at 3db25671; readonly-reference receivers preserve their calling convention.
        private static Circle2f Previous(in Triangle2f tri)
        {
            var a = tri.P1 - tri.P0; var b = tri.P2 - tri.P0;
            float aa = a.Dot(a), ab = a.Dot(b), bb = b.Dot(b);
            float d = 2 * (aa * bb - ab * ab);
            if (d.Abs() <= 1e-4f) return Circle2f.Invalid;
            float s = (aa * bb - bb * ab) / d;
            float t = (bb * aa - aa * ab) / d;
            var p = tri.P0; var bound = new Circle2f();
            if (s <= 0) bound.Center = 0.5f * (tri.P0 + tri.P2);
            else if (t <= 0) bound.Center = 0.5f * (tri.P0 + tri.P1);
            else if (s + t >= 1) { bound.Center = 0.5f * (tri.P1 + tri.P2); p = tri.P1; }
            else bound.Center = tri.P0 + s * a + t * b;
            bound.Radius = (bound.Center - p).Length;
            return bound;
        }
        private static Circle2d Previous(in Triangle2d tri)
        {
            var a = tri.P1 - tri.P0; var b = tri.P2 - tri.P0;
            double aa = a.Dot(a), ab = a.Dot(b), bb = b.Dot(b);
            double d = 2 * (aa * bb - ab * ab);
            if (d.Abs() <= 1e-6) return Circle2d.Invalid;
            double s = (aa * bb - bb * ab) / d;
            double t = (bb * aa - aa * ab) / d;
            var p = tri.P0; var bound = new Circle2d();
            if (s <= 0) bound.Center = 0.5 * (tri.P0 + tri.P2);
            else if (t <= 0) bound.Center = 0.5 * (tri.P0 + tri.P1);
            else if (s + t >= 1) { bound.Center = 0.5 * (tri.P1 + tri.P2); p = tri.P1; }
            else bound.Center = tri.P0 + s * a + t * b;
            bound.Radius = (bound.Center - p).Length;
            return bound;
        }
        private static Sphere3f Previous(in Triangle3f tri)
        {
            var a = tri.Edge01; var b = tri.Edge02;
            float aa = a.Dot(a), ab = a.Dot(b), bb = b.Dot(b);
            float d = 2 * (aa * bb - ab * ab);
            if (d.Abs() <= 1e-5f) return Sphere3f.Invalid;
            float s = (aa * bb - bb * ab) / d;
            float t = (bb * aa - aa * ab) / d;
            var p = tri.P0; var bound = new Sphere3f();
            if (s <= 0) bound.Center = 0.5f * (tri.P0 + tri.P2);
            else if (t <= 0) bound.Center = 0.5f * (tri.P0 + tri.P1);
            else if (s + t >= 1) { bound.Center = 0.5f * (tri.P1 + tri.P2); p = tri.P1; }
            else bound.Center = tri.P0 + s * a + t * b;
            bound.Radius = (bound.Center - p).Length;
            return bound;
        }
        private static Sphere3d Previous(in Triangle3d tri)
        {
            var a = tri.Edge01; var b = tri.Edge02;
            double aa = a.Dot(a), ab = a.Dot(b), bb = b.Dot(b);
            double d = 2 * (aa * bb - ab * ab);
            if (d.Abs() <= 1e-9) return Sphere3d.Invalid;
            double s = (aa * bb - bb * ab) / d;
            double t = (bb * aa - aa * ab) / d;
            var p = tri.P0; var bound = new Sphere3d();
            if (s <= 0) bound.Center = 0.5 * (tri.P0 + tri.P2);
            else if (t <= 0) bound.Center = 0.5 * (tri.P0 + tri.P1);
            else if (s + t >= 1) { bound.Center = 0.5 * (tri.P1 + tri.P2); p = tri.P1; }
            else bound.Center = tri.P0 + s * a + t * b;
            bound.Radius = (bound.Center - p).Length;
            return bound;
        }
    }
}

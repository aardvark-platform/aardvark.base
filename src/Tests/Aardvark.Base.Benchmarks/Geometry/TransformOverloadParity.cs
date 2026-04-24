using BenchmarkDotNet.Attributes;
using Aardvark.Geometry;

namespace Aardvark.Base.Benchmarks.Geometry
{
    [PlainExporter, MemoryDiagnoser]
    public class TransformOverloadParity
    {
        private Trafo3d _trafo3d;
        private Box3d _box3d;
        private Hull3d _hull3d;
        private FastHull3d _fastHull3d;
        private Plane3d _plane3d;
        private Ray3d _ray3d;
        private Shift2d _shift2d;
        private PolyRegion _polyRegion;

        [GlobalSetup]
        public void Setup()
        {
            _trafo3d = Trafo3d.FromComponents(
                new V3d(1.15, 0.9, 1.35),
                new V3d(0.2, -0.35, 0.5),
                new V3d(-1.5, 2.0, 0.75));

            _box3d = Box3d.FromCenterAndSize(new V3d(1.5, -0.5, 2.0), new V3d(3.0, 4.0, 2.5));
            _hull3d = new Hull3d(_box3d);
            _fastHull3d = new FastHull3d(_hull3d);
            _plane3d = new Plane3d(new V3d(1.0, -2.0, 3.0).Normalized, new V3d(-1.5, 0.75, 2.25));
            _ray3d = new Ray3d(new V3d(-1.5, 2.25, -0.5), new V3d(0.75, -2.0, 3.5));
            _shift2d = new Shift2d(3.5, -1.25);
            _polyRegion = new PolyRegion(new Polygon2d(new[]
            {
                new V2d(-2.0, -1.0),
                new V2d(3.0, -1.0),
                new V2d(3.0, 2.0),
                new V2d(-2.0, 2.0),
            }));
        }

        [Benchmark]
        public Box3d BoxInvTransformed_Indirect()
            => _box3d.Transformed(_trafo3d.Inverse);

        [Benchmark]
        public Box3d BoxInvTransformed_Direct()
            => _box3d.InvTransformed(_trafo3d);

        [Benchmark]
        public Hull3d HullInvTransformed_Indirect()
            => _hull3d.Transformed(_trafo3d.Inverse);

        [Benchmark]
        public Hull3d HullInvTransformed_Direct()
            => _hull3d.InvTransformed(_trafo3d);

        [Benchmark]
        public FastHull3d FastHullInvTransformed_Indirect()
            => _fastHull3d.Transformed(_trafo3d.Inverse);

        [Benchmark]
        public FastHull3d FastHullInvTransformed_Direct()
            => _fastHull3d.InvTransformed(_trafo3d);

        [Benchmark]
        public Plane3d PlaneInvTransformed_Indirect()
            => _plane3d.Transformed(_trafo3d.Inverse);

        [Benchmark]
        public Plane3d PlaneInvTransformed_Direct()
            => _plane3d.InvTransformed(_trafo3d);

        [Benchmark]
        public Ray3d RayTransformedTrafo_Manual()
            => new Ray3d(_trafo3d.TransformPos(_ray3d.Origin), _trafo3d.TransformDir(_ray3d.Direction));

        [Benchmark]
        public Ray3d RayTransformedTrafo_Direct()
            => _ray3d.Transformed(_trafo3d);

        [Benchmark]
        public Ray3d RayInvTransformed_Indirect()
            => _ray3d.Transformed(_trafo3d.Inverse);

        [Benchmark]
        public Ray3d RayInvTransformed_Direct()
            => _ray3d.InvTransformed(_trafo3d);

        [Benchmark]
        public PolyRegion PolyRegionInvTransformed_Indirect()
            => _polyRegion.Transformed(_shift2d.Inverse);

        [Benchmark]
        public PolyRegion PolyRegionInvTransformed_Direct()
            => _polyRegion.InvTransformed(_shift2d);
    }
}

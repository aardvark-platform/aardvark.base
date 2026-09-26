namespace Aardvark.Base.Benchmarks.Geometry
{
    // Shared input data for transform-overload perf comparisons. Keep setup outside the
    // measured delegates so targeted perf cases compare only the conversion baseline and
    // specialized overload bodies.
    internal static class TransformOverloadBenchData
    {
        public static readonly Box2i Box2i = new(new V2i(-3, 2), new V2i(5, 9));
        public static readonly Box2l Box2l = new(new V2l(-7, 3), new V2l(4, 12));
        public static readonly Box2f Box2f = new(new V2f(-3.5f, 2.25f), new V2f(5.5f, 9.75f));
        public static readonly Box2d Box2d = new(new V2d(-6.25, 1.5), new V2d(3.75, 8.125));

        public static readonly Box3i Box3i = new(new V3i(-4, 2, -1), new V3i(6, 9, 7));
        public static readonly Box3l Box3l = new(new V3l(-9, 3, -5), new V3l(5, 12, 11));
        public static readonly Box3f Box3f = new(new V3f(-4.5f, 2.25f, -1.75f), new V3f(6.5f, 9.75f, 7.25f));
        public static readonly Box3d Box3d = new(new V3d(-7.25, 1.5, -2.5), new V3d(4.5, 8.125, 6.75));

        public static readonly Hull2f Hull2f = new(Box2f);
        public static readonly Hull2d Hull2d = new(Box2d);
        public static readonly Hull3f Hull3f = new(Box3f);
        public static readonly Hull3d Hull3d = new(Box3d);
        public static readonly FastHull3f FastHull3f = new(Hull3f);
        public static readonly FastHull3d FastHull3d = new(Hull3d);

        public static readonly Plane3f Plane3f = new(new V3f(1.0f, -2.0f, 3.0f).Normalized, new V3f(-1.5f, 0.75f, 2.25f));
        public static readonly Plane3d Plane3d = new(new V3d(1.0, -2.0, 3.0).Normalized, new V3d(-1.5, 0.75, 2.25));
        public static readonly Ray3f Ray3f = new(new V3f(-1.5f, 2.25f, -0.5f), new V3f(0.75f, -2.0f, 3.5f));
        public static readonly Ray3d Ray3d = new(new V3d(-1.5, 2.25, -0.5), new V3d(0.75, -2.0, 3.5));

        public static readonly global::Aardvark.Geometry.PolyRegion PolyRegion2d = new(new Polygon2d(new[]
        {
            new V2d(-2.0, -1.0),
            new V2d(3.0, -1.0),
            new V2d(3.0, 2.0),
            new V2d(-2.0, 2.0),
        }));

        public static readonly Rot2d Rot2d = Rot2d.FromDegrees(37.0);
        public static readonly Euclidean2d Euclidean2d = new(Rot2d, new V2d(2.5, -1.75));
        public static readonly Similarity2d Similarity2d = new(-0.65, Rot2d.FromDegrees(-113.0), new V2d(1.5, -2.75));
        public static readonly Affine2d Affine2d = new(new M22d(1.2, 0.35, -0.2, 0.9), new V2d(5.0, -2.0));
        public static readonly Shift2d Shift2d = new(3.5, -1.25);
        public static readonly Scale2d Scale2d = new(-1.5, 0.8);
        public static readonly Trafo2d Trafo2d = new(Affine2d);

        public static readonly Rot2f Rot2f = Rot2f.FromDegrees(37.0f);
        public static readonly Euclidean2f Euclidean2f = new(Rot2f, new V2f(2.5f, -1.75f));
        public static readonly Similarity2f Similarity2f = new(-0.65f, Rot2f.FromDegrees(-113.0f), new V2f(1.5f, -2.75f));
        public static readonly Affine2f Affine2f = new(new M22f(1.2f, 0.35f, -0.2f, 0.9f), new V2f(5.0f, -2.0f));
        public static readonly Shift2f Shift2f = new(3.5f, -1.25f);
        public static readonly Scale2f Scale2f = new(-1.5f, 0.8f);
        public static readonly Trafo2f Trafo2f = new(Affine2f);

        public static readonly Rot3d Rot3d = Rot3d.Rotation(new V3d(-0.9, 0.2, 0.35).Normalized, -1.1);
        public static readonly Euclidean3d Euclidean3d = new(Rot3d.Rotation(new V3d(0.3, -0.5, 0.8).Normalized, 0.41), new V3d(2.5, -1.75, 4.0));
        public static readonly Similarity3d Similarity3d = new(-0.65, Rot3d, new V3d(1.5, -2.75, 0.5));
        public static readonly Affine3d Affine3d = new(new M33d(1.2, 0.35, -0.1, -0.2, 0.9, 0.15, 0.05, -0.25, 1.1), new V3d(5.0, -2.0, 1.75));
        public static readonly Shift3d Shift3d = new(3.5, -1.25, 2.0);
        public static readonly Scale3d Scale3d = new(-1.5, 0.8, -1.25);
        public static readonly Trafo3d Trafo3d = new(Affine3d);
        public static readonly Trafo3d RayTrafo3d = new(Similarity3d);

        public static readonly Rot3f Rot3f = Rot3f.Rotation(new V3f(-0.9f, 0.2f, 0.35f).Normalized, -1.1f);
        public static readonly Euclidean3f Euclidean3f = new(Rot3f.Rotation(new V3f(0.3f, -0.5f, 0.8f).Normalized, 0.41f), new V3f(2.5f, -1.75f, 4.0f));
        public static readonly Similarity3f Similarity3f = new(-0.65f, Rot3f, new V3f(1.5f, -2.75f, 0.5f));
        public static readonly Affine3f Affine3f = new(new M33f(1.2f, 0.35f, -0.1f, -0.2f, 0.9f, 0.15f, 0.05f, -0.25f, 1.1f), new V3f(5.0f, -2.0f, 1.75f));
        public static readonly Shift3f Shift3f = new(3.5f, -1.25f, 2.0f);
        public static readonly Scale3f Scale3f = new(-1.5f, 0.8f, -1.25f);
        public static readonly Trafo3f Trafo3f = new(Affine3f);
        public static readonly Trafo3f RayTrafo3f = new(Similarity3f);
    }
}

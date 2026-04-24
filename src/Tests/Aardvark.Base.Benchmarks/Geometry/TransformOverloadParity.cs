using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // Keep each benchmark class scoped to a single specialization so perf validation can
    // run only the exact overload under investigation via BenchmarkDotNet filters.
    internal static class TransformOverloadBenchData
    {
        public static readonly Box2d Box2d = Box2d.FromCenterAndSize(new V2d(1.5, -0.5), new V2d(3.0, 4.0));
        public static readonly Box3d Box3d = Box3d.FromCenterAndSize(new V3d(1.5, -0.5, 2.0), new V3d(3.0, 4.0, 2.5));
        public static readonly Hull2d Hull2d = new(Box2d);
        public static readonly Hull3d Hull3d = new(Box3d);
        public static readonly FastHull3d FastHull3d = new(Hull3d);
        public static readonly Plane3d Plane3d = new(new V3d(1.0, -2.0, 3.0).Normalized, new V3d(-1.5, 0.75, 2.25));

        public static readonly Rot2d Rot2d = Rot2d.FromDegrees(37.0);
        public static readonly Euclidean2d Euclidean2d = new(Rot2d, new V2d(2.5, -1.75));
        public static readonly Similarity2d Similarity2d = new(-0.65, Rot2d.FromDegrees(-113.0), new V2d(1.5, -2.75));
        public static readonly Affine2d Affine2d = new(new M22d(1.2, 0.35, -0.2, 0.9), new V2d(5.0, -2.0));
        public static readonly Shift2d Shift2d = new(3.5, -1.25);
        public static readonly Scale2d Scale2d = new(-1.5, 0.8);

        public static readonly Rot3d Rot3d = Rot3d.Rotation(new V3d(-0.9, 0.2, 0.35).Normalized, -1.1);
        public static readonly Euclidean3d Euclidean3d = new(Rot3d.Rotation(new V3d(0.3, -0.5, 0.8).Normalized, 0.41), new V3d(2.5, -1.75, 4.0));
        public static readonly Similarity3d Similarity3d = new(-0.65, Rot3d, new V3d(1.5, -2.75, 0.5));
        public static readonly Affine3d Affine3d = new(new M33d(1.2, 0.35, -0.1, -0.2, 0.9, 0.15, 0.05, -0.25, 1.1), new V3d(5.0, -2.0, 1.75));
        public static readonly Shift3d Shift3d = new(3.5, -1.25, 2.0);
        public static readonly Scale3d Scale3d = new(-1.5, 0.8, -1.25);
    }

    public abstract class Box2dForwardBenchmark
    {
        [Benchmark(Baseline = true)]
        public Box2d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Box2d Specialized() => SpecializedImpl();

        protected abstract Box2d ConversionBaselineImpl();
        protected abstract Box2d SpecializedImpl();
    }

    public abstract class Box2dInverseBenchmark
    {
        [Benchmark(Baseline = true)]
        public Box2d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Box2d Specialized() => SpecializedImpl();

        protected abstract Box2d ConversionBaselineImpl();
        protected abstract Box2d SpecializedImpl();
    }

    public abstract class Box3dForwardBenchmark
    {
        [Benchmark(Baseline = true)]
        public Box3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Box3d Specialized() => SpecializedImpl();

        protected abstract Box3d ConversionBaselineImpl();
        protected abstract Box3d SpecializedImpl();
    }

    public abstract class Box3dInverseBenchmark
    {
        [Benchmark(Baseline = true)]
        public Box3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Box3d Specialized() => SpecializedImpl();

        protected abstract Box3d ConversionBaselineImpl();
        protected abstract Box3d SpecializedImpl();
    }

    public abstract class Hull2dForwardBenchmark
    {
        [Benchmark(Baseline = true)]
        public Hull2d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Hull2d Specialized() => SpecializedImpl();

        protected abstract Hull2d ConversionBaselineImpl();
        protected abstract Hull2d SpecializedImpl();
    }

    public abstract class Hull2dInverseBenchmark
    {
        [Benchmark(Baseline = true)]
        public Hull2d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Hull2d Specialized() => SpecializedImpl();

        protected abstract Hull2d ConversionBaselineImpl();
        protected abstract Hull2d SpecializedImpl();
    }

    public abstract class Hull3dForwardBenchmark
    {
        [Benchmark(Baseline = true)]
        public Hull3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Hull3d Specialized() => SpecializedImpl();

        protected abstract Hull3d ConversionBaselineImpl();
        protected abstract Hull3d SpecializedImpl();
    }

    public abstract class Hull3dInverseBenchmark
    {
        [Benchmark(Baseline = true)]
        public Hull3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Hull3d Specialized() => SpecializedImpl();

        protected abstract Hull3d ConversionBaselineImpl();
        protected abstract Hull3d SpecializedImpl();
    }

    public abstract class FastHull3dForwardBenchmark
    {
        [Benchmark(Baseline = true)]
        public FastHull3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public FastHull3d Specialized() => SpecializedImpl();

        protected abstract FastHull3d ConversionBaselineImpl();
        protected abstract FastHull3d SpecializedImpl();
    }

    public abstract class FastHull3dInverseBenchmark
    {
        [Benchmark(Baseline = true)]
        public FastHull3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public FastHull3d Specialized() => SpecializedImpl();

        protected abstract FastHull3d ConversionBaselineImpl();
        protected abstract FastHull3d SpecializedImpl();
    }

    public abstract class Plane3dForwardBenchmark
    {
        [Benchmark(Baseline = true)]
        public Plane3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Plane3d Specialized() => SpecializedImpl();

        protected abstract Plane3d ConversionBaselineImpl();
        protected abstract Plane3d SpecializedImpl();
    }

    public abstract class Plane3dInverseBenchmark
    {
        [Benchmark(Baseline = true)]
        public Plane3d ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Plane3d Specialized() => SpecializedImpl();

        protected abstract Plane3d ConversionBaselineImpl();
        protected abstract Plane3d SpecializedImpl();
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dForwardEuclidean : Box2dForwardBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Euclidean2d);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Euclidean2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dForwardSimilarity : Box2dForwardBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Similarity2d);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Similarity2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dForwardAffine : Box2dForwardBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Affine2d);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Affine2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dForwardShift : Box2dForwardBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Shift2d);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Shift2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dForwardRot : Box2dForwardBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Rot2d);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Rot2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dForwardScale : Box2dForwardBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Scale2d);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Scale2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dInverseEuclidean : Box2dInverseBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Euclidean2d).Inverse);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Euclidean2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dInverseSimilarity : Box2dInverseBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Similarity2d).Inverse);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Similarity2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dInverseShift : Box2dInverseBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Shift2d).Inverse);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Shift2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dInverseRot : Box2dInverseBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Rot2d).Inverse);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Rot2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box2dInverseScale : Box2dInverseBenchmark
    {
        protected override Box2d ConversionBaselineImpl() => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Scale2d).Inverse);
        protected override Box2d SpecializedImpl() => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Scale2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dForwardEuclidean : Box3dForwardBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Euclidean3d);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Euclidean3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dForwardSimilarity : Box3dForwardBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Similarity3d);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dForwardAffine : Box3dForwardBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Affine3d);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Affine3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dForwardShift : Box3dForwardBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Shift3d);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dForwardRot : Box3dForwardBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Rot3d);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dForwardScale : Box3dForwardBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Scale3d);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Scale3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dInverseEuclidean : Box3dInverseBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Euclidean3d).Inverse);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Euclidean3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dInverseSimilarity : Box3dInverseBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Similarity3d).Inverse);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dInverseShift : Box3dInverseBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Shift3d).Inverse);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dInverseRot : Box3dInverseBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Rot3d).Inverse);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Box3dInverseScale : Box3dInverseBenchmark
    {
        protected override Box3d ConversionBaselineImpl() => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Scale3d).Inverse);
        protected override Box3d SpecializedImpl() => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Scale3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dForwardEuclidean : Hull2dForwardBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Euclidean2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Euclidean2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dForwardSimilarity : Hull2dForwardBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Similarity2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Similarity2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dForwardAffine : Hull2dForwardBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Affine2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Affine2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dForwardShift : Hull2dForwardBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Shift2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Shift2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dForwardRot : Hull2dForwardBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Rot2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Rot2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dForwardScale : Hull2dForwardBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Scale2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Scale2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dInverseEuclidean : Hull2dInverseBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Euclidean2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Euclidean2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dInverseSimilarity : Hull2dInverseBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Similarity2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Similarity2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dInverseShift : Hull2dInverseBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Shift2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Shift2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dInverseRot : Hull2dInverseBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Rot2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Rot2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull2dInverseScale : Hull2dInverseBenchmark
    {
        protected override Hull2d ConversionBaselineImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Scale2d));
        protected override Hull2d SpecializedImpl() => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Scale2d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dForwardEuclidean : Hull3dForwardBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Euclidean3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dForwardSimilarity : Hull3dForwardBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dForwardAffine : Hull3dForwardBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Affine3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dForwardShift : Hull3dForwardBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dForwardRot : Hull3dForwardBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dForwardScale : Hull3dForwardBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Scale3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dInverseEuclidean : Hull3dInverseBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Euclidean3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dInverseSimilarity : Hull3dInverseBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Similarity3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dInverseShift : Hull3dInverseBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Shift3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dInverseRot : Hull3dInverseBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Rot3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Hull3dInverseScale : Hull3dInverseBenchmark
    {
        protected override Hull3d ConversionBaselineImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Scale3d));
        protected override Hull3d SpecializedImpl() => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Scale3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dForwardEuclidean : FastHull3dForwardBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Euclidean3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dForwardSimilarity : FastHull3dForwardBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dForwardAffine : FastHull3dForwardBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Affine3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dForwardShift : FastHull3dForwardBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dForwardRot : FastHull3dForwardBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dForwardScale : FastHull3dForwardBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Scale3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dInverseEuclidean : FastHull3dInverseBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Euclidean3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dInverseSimilarity : FastHull3dInverseBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dInverseShift : FastHull3dInverseBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Shift3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dInverseRot : FastHull3dInverseBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Rot3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class FastHull3dInverseScale : FastHull3dInverseBenchmark
    {
        protected override FastHull3d ConversionBaselineImpl() => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Scale3d)));
        protected override FastHull3d SpecializedImpl() => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Scale3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dForwardSimilarity : Plane3dForwardBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dForwardAffine : Plane3dForwardBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Affine3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dForwardShift : Plane3dForwardBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dForwardRot : Plane3dForwardBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dForwardScale : Plane3dForwardBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Scale3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dInverseEuclidean : Plane3dInverseBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Euclidean3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dInverseSimilarity : Plane3dInverseBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Similarity3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Similarity3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dInverseShift : Plane3dInverseBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Shift3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Shift3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dInverseRot : Plane3dInverseBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Rot3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Rot3d);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3dInverseScale : Plane3dInverseBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Scale3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Scale3d);
    }
}

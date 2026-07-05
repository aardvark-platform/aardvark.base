using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // Keep each benchmark class scoped to a single specialization so perf validation can
    // run only the exact overload under investigation via BenchmarkDotNet filters.
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

    public abstract class Plane3fForwardBenchmark
    {
        [Benchmark(Baseline = true)]
        public Plane3f ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Plane3f Specialized() => SpecializedImpl();

        protected abstract Plane3f ConversionBaselineImpl();
        protected abstract Plane3f SpecializedImpl();
    }

    public abstract class Plane3fInverseBenchmark
    {
        [Benchmark(Baseline = true)]
        public Plane3f ConversionBaseline() => ConversionBaselineImpl();

        [Benchmark]
        public Plane3f Specialized() => SpecializedImpl();

        protected abstract Plane3f ConversionBaselineImpl();
        protected abstract Plane3f SpecializedImpl();
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
    public class Plane3dForwardEuclidean : Plane3dForwardBenchmark
    {
        protected override Plane3d ConversionBaselineImpl() => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d));
        protected override Plane3d SpecializedImpl() => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Euclidean3d);
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

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fForwardEuclidean : Plane3fForwardBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Euclidean3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fForwardSimilarity : Plane3fForwardBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Similarity3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fForwardAffine : Plane3fForwardBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Affine3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Affine3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fForwardShift : Plane3fForwardBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Shift3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fForwardRot : Plane3fForwardBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Rot3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fForwardScale : Plane3fForwardBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Scale3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fInverseEuclidean : Plane3fInverseBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Euclidean3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fInverseSimilarity : Plane3fInverseBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(new Trafo3f(TransformOverloadBenchData.Similarity3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Similarity3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fInverseShift : Plane3fInverseBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(new Trafo3f(TransformOverloadBenchData.Shift3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Shift3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fInverseRot : Plane3fInverseBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(new Trafo3f(TransformOverloadBenchData.Rot3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Rot3f);
    }

    [PlainExporter, MemoryDiagnoser, MediumRunJob]
    public class Plane3fInverseScale : Plane3fInverseBenchmark
    {
        protected override Plane3f ConversionBaselineImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(new Trafo3f(TransformOverloadBenchData.Scale3f));
        protected override Plane3f SpecializedImpl() => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Scale3f);
    }
}

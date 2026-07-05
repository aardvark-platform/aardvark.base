using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Aardvark.Base.Benchmarks.Geometry
{
    public static class TransformOverloadTargetedPerf
    {
        private static readonly TransformOverloadPerfSettings DefaultSettings = new(2, 7, 150.0);
        private static readonly TransformOverloadPerfSettings QuickSettings = new(1, 3, 40.0);

        private static readonly ITransformOverloadPerfCase[] Cases =
        {
            // Boxes: all correctness-test numeric variants. Integer and long boxes intentionally use the double transform overload family.
            Create("Box2iForwardEuclidean", () => TransformOverloadBenchData.Box2i.Transformed((M33d)TransformOverloadBenchData.Euclidean2d), () => TransformOverloadBenchData.Box2i.Transformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2iForwardSimilarity", () => TransformOverloadBenchData.Box2i.Transformed((M33d)TransformOverloadBenchData.Similarity2d), () => TransformOverloadBenchData.Box2i.Transformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2iForwardAffine", () => TransformOverloadBenchData.Box2i.Transformed((M33d)TransformOverloadBenchData.Affine2d), () => TransformOverloadBenchData.Box2i.Transformed(TransformOverloadBenchData.Affine2d)),
            Create("Box2iForwardShift", () => TransformOverloadBenchData.Box2i.Transformed((M33d)TransformOverloadBenchData.Shift2d), () => TransformOverloadBenchData.Box2i.Transformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2iForwardRot", () => TransformOverloadBenchData.Box2i.Transformed((M33d)TransformOverloadBenchData.Rot2d), () => TransformOverloadBenchData.Box2i.Transformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2iForwardScale", () => TransformOverloadBenchData.Box2i.Transformed((M33d)TransformOverloadBenchData.Scale2d), () => TransformOverloadBenchData.Box2i.Transformed(TransformOverloadBenchData.Scale2d)),
            Create("Box2iInverseTrafo", () => TransformOverloadBenchData.Box2i.Transformed(TransformOverloadBenchData.Trafo2d.Backward), () => TransformOverloadBenchData.Box2i.InvTransformed(TransformOverloadBenchData.Trafo2d)),
            Create("Box2iInverseEuclidean", () => TransformOverloadBenchData.Box2i.Transformed(((M33d)TransformOverloadBenchData.Euclidean2d).Inverse), () => TransformOverloadBenchData.Box2i.InvTransformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2iInverseSimilarity", () => TransformOverloadBenchData.Box2i.Transformed(((M33d)TransformOverloadBenchData.Similarity2d).Inverse), () => TransformOverloadBenchData.Box2i.InvTransformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2iInverseShift", () => TransformOverloadBenchData.Box2i.Transformed(((M33d)TransformOverloadBenchData.Shift2d).Inverse), () => TransformOverloadBenchData.Box2i.InvTransformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2iInverseRot", () => TransformOverloadBenchData.Box2i.Transformed(((M33d)TransformOverloadBenchData.Rot2d).Inverse), () => TransformOverloadBenchData.Box2i.InvTransformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2iInverseScale", () => TransformOverloadBenchData.Box2i.Transformed(((M33d)TransformOverloadBenchData.Scale2d).Inverse), () => TransformOverloadBenchData.Box2i.InvTransformed(TransformOverloadBenchData.Scale2d)),

            Create("Box2lForwardEuclidean", () => TransformOverloadBenchData.Box2l.Transformed((M33d)TransformOverloadBenchData.Euclidean2d), () => TransformOverloadBenchData.Box2l.Transformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2lForwardSimilarity", () => TransformOverloadBenchData.Box2l.Transformed((M33d)TransformOverloadBenchData.Similarity2d), () => TransformOverloadBenchData.Box2l.Transformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2lForwardAffine", () => TransformOverloadBenchData.Box2l.Transformed((M33d)TransformOverloadBenchData.Affine2d), () => TransformOverloadBenchData.Box2l.Transformed(TransformOverloadBenchData.Affine2d)),
            Create("Box2lForwardShift", () => TransformOverloadBenchData.Box2l.Transformed((M33d)TransformOverloadBenchData.Shift2d), () => TransformOverloadBenchData.Box2l.Transformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2lForwardRot", () => TransformOverloadBenchData.Box2l.Transformed((M33d)TransformOverloadBenchData.Rot2d), () => TransformOverloadBenchData.Box2l.Transformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2lForwardScale", () => TransformOverloadBenchData.Box2l.Transformed((M33d)TransformOverloadBenchData.Scale2d), () => TransformOverloadBenchData.Box2l.Transformed(TransformOverloadBenchData.Scale2d)),
            Create("Box2lInverseTrafo", () => TransformOverloadBenchData.Box2l.Transformed(TransformOverloadBenchData.Trafo2d.Backward), () => TransformOverloadBenchData.Box2l.InvTransformed(TransformOverloadBenchData.Trafo2d)),
            Create("Box2lInverseEuclidean", () => TransformOverloadBenchData.Box2l.Transformed(((M33d)TransformOverloadBenchData.Euclidean2d).Inverse), () => TransformOverloadBenchData.Box2l.InvTransformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2lInverseSimilarity", () => TransformOverloadBenchData.Box2l.Transformed(((M33d)TransformOverloadBenchData.Similarity2d).Inverse), () => TransformOverloadBenchData.Box2l.InvTransformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2lInverseShift", () => TransformOverloadBenchData.Box2l.Transformed(((M33d)TransformOverloadBenchData.Shift2d).Inverse), () => TransformOverloadBenchData.Box2l.InvTransformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2lInverseRot", () => TransformOverloadBenchData.Box2l.Transformed(((M33d)TransformOverloadBenchData.Rot2d).Inverse), () => TransformOverloadBenchData.Box2l.InvTransformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2lInverseScale", () => TransformOverloadBenchData.Box2l.Transformed(((M33d)TransformOverloadBenchData.Scale2d).Inverse), () => TransformOverloadBenchData.Box2l.InvTransformed(TransformOverloadBenchData.Scale2d)),

            Create("Box2fForwardEuclidean", () => TransformOverloadBenchData.Box2f.Transformed((M33f)TransformOverloadBenchData.Euclidean2f), () => TransformOverloadBenchData.Box2f.Transformed(TransformOverloadBenchData.Euclidean2f)),
            Create("Box2fForwardSimilarity", () => TransformOverloadBenchData.Box2f.Transformed((M33f)TransformOverloadBenchData.Similarity2f), () => TransformOverloadBenchData.Box2f.Transformed(TransformOverloadBenchData.Similarity2f)),
            Create("Box2fForwardAffine", () => TransformOverloadBenchData.Box2f.Transformed((M33f)TransformOverloadBenchData.Affine2f), () => TransformOverloadBenchData.Box2f.Transformed(TransformOverloadBenchData.Affine2f)),
            Create("Box2fForwardShift", () => TransformOverloadBenchData.Box2f.Transformed((M33f)TransformOverloadBenchData.Shift2f), () => TransformOverloadBenchData.Box2f.Transformed(TransformOverloadBenchData.Shift2f)),
            Create("Box2fForwardRot", () => TransformOverloadBenchData.Box2f.Transformed((M33f)TransformOverloadBenchData.Rot2f), () => TransformOverloadBenchData.Box2f.Transformed(TransformOverloadBenchData.Rot2f)),
            Create("Box2fForwardScale", () => TransformOverloadBenchData.Box2f.Transformed((M33f)TransformOverloadBenchData.Scale2f), () => TransformOverloadBenchData.Box2f.Transformed(TransformOverloadBenchData.Scale2f)),
            Create("Box2fInverseTrafo", () => TransformOverloadBenchData.Box2f.Transformed(TransformOverloadBenchData.Trafo2f.Backward), () => TransformOverloadBenchData.Box2f.InvTransformed(TransformOverloadBenchData.Trafo2f)),
            Create("Box2fInverseEuclidean", () => TransformOverloadBenchData.Box2f.Transformed(((M33f)TransformOverloadBenchData.Euclidean2f).Inverse), () => TransformOverloadBenchData.Box2f.InvTransformed(TransformOverloadBenchData.Euclidean2f)),
            Create("Box2fInverseSimilarity", () => TransformOverloadBenchData.Box2f.Transformed(((M33f)TransformOverloadBenchData.Similarity2f).Inverse), () => TransformOverloadBenchData.Box2f.InvTransformed(TransformOverloadBenchData.Similarity2f)),
            Create("Box2fInverseShift", () => TransformOverloadBenchData.Box2f.Transformed(((M33f)TransformOverloadBenchData.Shift2f).Inverse), () => TransformOverloadBenchData.Box2f.InvTransformed(TransformOverloadBenchData.Shift2f)),
            Create("Box2fInverseRot", () => TransformOverloadBenchData.Box2f.Transformed(((M33f)TransformOverloadBenchData.Rot2f).Inverse), () => TransformOverloadBenchData.Box2f.InvTransformed(TransformOverloadBenchData.Rot2f)),
            Create("Box2fInverseScale", () => TransformOverloadBenchData.Box2f.Transformed(((M33f)TransformOverloadBenchData.Scale2f).Inverse), () => TransformOverloadBenchData.Box2f.InvTransformed(TransformOverloadBenchData.Scale2f)),

            Create("Box2dForwardEuclidean", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Euclidean2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2dForwardSimilarity", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Similarity2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2dForwardAffine", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Affine2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Affine2d)),
            Create("Box2dForwardShift", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Shift2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2dForwardRot", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Rot2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2dForwardScale", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Scale2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Scale2d)),
            Create("Box2dInverseTrafo", () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Trafo2d.Backward), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Trafo2d)),
            Create("Box2dInverseEuclidean", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Euclidean2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2dInverseSimilarity", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Similarity2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2dInverseShift", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Shift2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2dInverseRot", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Rot2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2dInverseScale", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Scale2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Scale2d)),

            Create("Box3iForwardEuclidean", () => TransformOverloadBenchData.Box3i.Transformed((M44d)TransformOverloadBenchData.Euclidean3d), () => TransformOverloadBenchData.Box3i.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3iForwardSimilarity", () => TransformOverloadBenchData.Box3i.Transformed((M44d)TransformOverloadBenchData.Similarity3d), () => TransformOverloadBenchData.Box3i.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3iForwardAffine", () => TransformOverloadBenchData.Box3i.Transformed((M44d)TransformOverloadBenchData.Affine3d), () => TransformOverloadBenchData.Box3i.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Box3iForwardShift", () => TransformOverloadBenchData.Box3i.Transformed((M44d)TransformOverloadBenchData.Shift3d), () => TransformOverloadBenchData.Box3i.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3iForwardRot", () => TransformOverloadBenchData.Box3i.Transformed((M44d)TransformOverloadBenchData.Rot3d), () => TransformOverloadBenchData.Box3i.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3iForwardScale", () => TransformOverloadBenchData.Box3i.Transformed((M44d)TransformOverloadBenchData.Scale3d), () => TransformOverloadBenchData.Box3i.Transformed(TransformOverloadBenchData.Scale3d)),
            Create("Box3iInverseTrafo", () => TransformOverloadBenchData.Box3i.Transformed(TransformOverloadBenchData.Trafo3d.Backward), () => TransformOverloadBenchData.Box3i.InvTransformed(TransformOverloadBenchData.Trafo3d)),
            Create("Box3iInverseEuclidean", () => TransformOverloadBenchData.Box3i.Transformed(((M44d)TransformOverloadBenchData.Euclidean3d).Inverse), () => TransformOverloadBenchData.Box3i.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3iInverseSimilarity", () => TransformOverloadBenchData.Box3i.Transformed(((M44d)TransformOverloadBenchData.Similarity3d).Inverse), () => TransformOverloadBenchData.Box3i.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3iInverseShift", () => TransformOverloadBenchData.Box3i.Transformed(((M44d)TransformOverloadBenchData.Shift3d).Inverse), () => TransformOverloadBenchData.Box3i.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3iInverseRot", () => TransformOverloadBenchData.Box3i.Transformed(((M44d)TransformOverloadBenchData.Rot3d).Inverse), () => TransformOverloadBenchData.Box3i.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3iInverseScale", () => TransformOverloadBenchData.Box3i.Transformed(((M44d)TransformOverloadBenchData.Scale3d).Inverse), () => TransformOverloadBenchData.Box3i.InvTransformed(TransformOverloadBenchData.Scale3d)),

            Create("Box3lForwardEuclidean", () => TransformOverloadBenchData.Box3l.Transformed((M44d)TransformOverloadBenchData.Euclidean3d), () => TransformOverloadBenchData.Box3l.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3lForwardSimilarity", () => TransformOverloadBenchData.Box3l.Transformed((M44d)TransformOverloadBenchData.Similarity3d), () => TransformOverloadBenchData.Box3l.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3lForwardAffine", () => TransformOverloadBenchData.Box3l.Transformed((M44d)TransformOverloadBenchData.Affine3d), () => TransformOverloadBenchData.Box3l.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Box3lForwardShift", () => TransformOverloadBenchData.Box3l.Transformed((M44d)TransformOverloadBenchData.Shift3d), () => TransformOverloadBenchData.Box3l.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3lForwardRot", () => TransformOverloadBenchData.Box3l.Transformed((M44d)TransformOverloadBenchData.Rot3d), () => TransformOverloadBenchData.Box3l.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3lForwardScale", () => TransformOverloadBenchData.Box3l.Transformed((M44d)TransformOverloadBenchData.Scale3d), () => TransformOverloadBenchData.Box3l.Transformed(TransformOverloadBenchData.Scale3d)),
            Create("Box3lInverseTrafo", () => TransformOverloadBenchData.Box3l.Transformed(TransformOverloadBenchData.Trafo3d.Backward), () => TransformOverloadBenchData.Box3l.InvTransformed(TransformOverloadBenchData.Trafo3d)),
            Create("Box3lInverseEuclidean", () => TransformOverloadBenchData.Box3l.Transformed(((M44d)TransformOverloadBenchData.Euclidean3d).Inverse), () => TransformOverloadBenchData.Box3l.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3lInverseSimilarity", () => TransformOverloadBenchData.Box3l.Transformed(((M44d)TransformOverloadBenchData.Similarity3d).Inverse), () => TransformOverloadBenchData.Box3l.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3lInverseShift", () => TransformOverloadBenchData.Box3l.Transformed(((M44d)TransformOverloadBenchData.Shift3d).Inverse), () => TransformOverloadBenchData.Box3l.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3lInverseRot", () => TransformOverloadBenchData.Box3l.Transformed(((M44d)TransformOverloadBenchData.Rot3d).Inverse), () => TransformOverloadBenchData.Box3l.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3lInverseScale", () => TransformOverloadBenchData.Box3l.Transformed(((M44d)TransformOverloadBenchData.Scale3d).Inverse), () => TransformOverloadBenchData.Box3l.InvTransformed(TransformOverloadBenchData.Scale3d)),

            Create("Box3fForwardEuclidean", () => TransformOverloadBenchData.Box3f.Transformed((M44f)TransformOverloadBenchData.Euclidean3f), () => TransformOverloadBenchData.Box3f.Transformed(TransformOverloadBenchData.Euclidean3f)),
            Create("Box3fForwardSimilarity", () => TransformOverloadBenchData.Box3f.Transformed((M44f)TransformOverloadBenchData.Similarity3f), () => TransformOverloadBenchData.Box3f.Transformed(TransformOverloadBenchData.Similarity3f)),
            Create("Box3fForwardAffine", () => TransformOverloadBenchData.Box3f.Transformed((M44f)TransformOverloadBenchData.Affine3f), () => TransformOverloadBenchData.Box3f.Transformed(TransformOverloadBenchData.Affine3f)),
            Create("Box3fForwardShift", () => TransformOverloadBenchData.Box3f.Transformed((M44f)TransformOverloadBenchData.Shift3f), () => TransformOverloadBenchData.Box3f.Transformed(TransformOverloadBenchData.Shift3f)),
            Create("Box3fForwardRot", () => TransformOverloadBenchData.Box3f.Transformed((M44f)TransformOverloadBenchData.Rot3f), () => TransformOverloadBenchData.Box3f.Transformed(TransformOverloadBenchData.Rot3f)),
            Create("Box3fForwardScale", () => TransformOverloadBenchData.Box3f.Transformed((M44f)TransformOverloadBenchData.Scale3f), () => TransformOverloadBenchData.Box3f.Transformed(TransformOverloadBenchData.Scale3f)),
            Create("Box3fInverseTrafo", () => TransformOverloadBenchData.Box3f.Transformed(TransformOverloadBenchData.Trafo3f.Backward), () => TransformOverloadBenchData.Box3f.InvTransformed(TransformOverloadBenchData.Trafo3f)),
            Create("Box3fInverseEuclidean", () => TransformOverloadBenchData.Box3f.Transformed(((M44f)TransformOverloadBenchData.Euclidean3f).Inverse), () => TransformOverloadBenchData.Box3f.InvTransformed(TransformOverloadBenchData.Euclidean3f)),
            Create("Box3fInverseSimilarity", () => TransformOverloadBenchData.Box3f.Transformed(((M44f)TransformOverloadBenchData.Similarity3f).Inverse), () => TransformOverloadBenchData.Box3f.InvTransformed(TransformOverloadBenchData.Similarity3f)),
            Create("Box3fInverseShift", () => TransformOverloadBenchData.Box3f.Transformed(((M44f)TransformOverloadBenchData.Shift3f).Inverse), () => TransformOverloadBenchData.Box3f.InvTransformed(TransformOverloadBenchData.Shift3f)),
            Create("Box3fInverseRot", () => TransformOverloadBenchData.Box3f.Transformed(((M44f)TransformOverloadBenchData.Rot3f).Inverse), () => TransformOverloadBenchData.Box3f.InvTransformed(TransformOverloadBenchData.Rot3f)),
            Create("Box3fInverseScale", () => TransformOverloadBenchData.Box3f.Transformed(((M44f)TransformOverloadBenchData.Scale3f).Inverse), () => TransformOverloadBenchData.Box3f.InvTransformed(TransformOverloadBenchData.Scale3f)),

            Create("Box3dForwardEuclidean", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Euclidean3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3dForwardSimilarity", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Similarity3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3dForwardAffine", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Affine3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Box3dForwardShift", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Shift3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3dForwardRot", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Rot3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3dForwardScale", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Scale3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Scale3d)),
            Create("Box3dInverseTrafo", () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Trafo3d.Backward), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Trafo3d)),
            Create("Box3dInverseEuclidean", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Euclidean3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3dInverseSimilarity", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Similarity3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3dInverseShift", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Shift3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3dInverseRot", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Rot3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3dInverseScale", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Scale3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            // Hulls: direct typed overloads versus the existing Trafo path. Inverse baselines deliberately use Transformed(inverse Trafo), not another new inverse overload.
            Create("Hull2fForwardEuclidean", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Euclidean2f)), () => TransformOverloadBenchData.Hull2f.Transformed(TransformOverloadBenchData.Euclidean2f)),
            Create("Hull2fForwardSimilarity", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Similarity2f)), () => TransformOverloadBenchData.Hull2f.Transformed(TransformOverloadBenchData.Similarity2f)),
            Create("Hull2fForwardAffine", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Affine2f)), () => TransformOverloadBenchData.Hull2f.Transformed(TransformOverloadBenchData.Affine2f)),
            Create("Hull2fForwardShift", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Shift2f)), () => TransformOverloadBenchData.Hull2f.Transformed(TransformOverloadBenchData.Shift2f)),
            Create("Hull2fForwardRot", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Rot2f)), () => TransformOverloadBenchData.Hull2f.Transformed(TransformOverloadBenchData.Rot2f)),
            Create("Hull2fForwardScale", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Scale2f)), () => TransformOverloadBenchData.Hull2f.Transformed(TransformOverloadBenchData.Scale2f)),
            Create("Hull2fInverseTrafo", () => TransformOverloadBenchData.Hull2f.Transformed(TransformOverloadBenchData.Trafo2f.Inverse), () => TransformOverloadBenchData.Hull2f.InvTransformed(TransformOverloadBenchData.Trafo2f)),
            Create("Hull2fInverseEuclidean", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Euclidean2f).Inverse), () => TransformOverloadBenchData.Hull2f.InvTransformed(TransformOverloadBenchData.Euclidean2f)),
            Create("Hull2fInverseSimilarity", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Similarity2f).Inverse), () => TransformOverloadBenchData.Hull2f.InvTransformed(TransformOverloadBenchData.Similarity2f)),
            Create("Hull2fInverseShift", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Shift2f).Inverse), () => TransformOverloadBenchData.Hull2f.InvTransformed(TransformOverloadBenchData.Shift2f)),
            Create("Hull2fInverseRot", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Rot2f).Inverse), () => TransformOverloadBenchData.Hull2f.InvTransformed(TransformOverloadBenchData.Rot2f)),
            Create("Hull2fInverseScale", () => TransformOverloadBenchData.Hull2f.Transformed(new Trafo2f(TransformOverloadBenchData.Scale2f).Inverse), () => TransformOverloadBenchData.Hull2f.InvTransformed(TransformOverloadBenchData.Scale2f)),

            Create("Hull2dForwardEuclidean", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Euclidean2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Hull2dForwardSimilarity", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Similarity2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Similarity2d)),
            Create("Hull2dForwardAffine", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Affine2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Affine2d)),
            Create("Hull2dForwardShift", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Shift2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Shift2d)),
            Create("Hull2dForwardRot", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Rot2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Rot2d)),
            Create("Hull2dForwardScale", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Scale2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Scale2d)),
            Create("Hull2dInverseTrafo", () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Trafo2d.Inverse), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Trafo2d)),
            Create("Hull2dInverseEuclidean", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Euclidean2d).Inverse), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Hull2dInverseSimilarity", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Similarity2d).Inverse), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Similarity2d)),
            Create("Hull2dInverseShift", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Shift2d).Inverse), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Shift2d)),
            Create("Hull2dInverseRot", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Rot2d).Inverse), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Rot2d)),
            Create("Hull2dInverseScale", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Scale2d).Inverse), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Scale2d)),

            Create("Hull3fForwardEuclidean", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f)), () => TransformOverloadBenchData.Hull3f.Transformed(TransformOverloadBenchData.Euclidean3f)),
            Create("Hull3fForwardSimilarity", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f)), () => TransformOverloadBenchData.Hull3f.Transformed(TransformOverloadBenchData.Similarity3f)),
            Create("Hull3fForwardAffine", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Affine3f)), () => TransformOverloadBenchData.Hull3f.Transformed(TransformOverloadBenchData.Affine3f)),
            Create("Hull3fForwardShift", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f)), () => TransformOverloadBenchData.Hull3f.Transformed(TransformOverloadBenchData.Shift3f)),
            Create("Hull3fForwardRot", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f)), () => TransformOverloadBenchData.Hull3f.Transformed(TransformOverloadBenchData.Rot3f)),
            Create("Hull3fForwardScale", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f)), () => TransformOverloadBenchData.Hull3f.Transformed(TransformOverloadBenchData.Scale3f)),
            Create("Hull3fInverseTrafo", () => TransformOverloadBenchData.Hull3f.Transformed(TransformOverloadBenchData.Trafo3f.Inverse), () => TransformOverloadBenchData.Hull3f.InvTransformed(TransformOverloadBenchData.Trafo3f)),
            Create("Hull3fInverseEuclidean", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f).Inverse), () => TransformOverloadBenchData.Hull3f.InvTransformed(TransformOverloadBenchData.Euclidean3f)),
            Create("Hull3fInverseSimilarity", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f).Inverse), () => TransformOverloadBenchData.Hull3f.InvTransformed(TransformOverloadBenchData.Similarity3f)),
            Create("Hull3fInverseShift", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f).Inverse), () => TransformOverloadBenchData.Hull3f.InvTransformed(TransformOverloadBenchData.Shift3f)),
            Create("Hull3fInverseRot", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f).Inverse), () => TransformOverloadBenchData.Hull3f.InvTransformed(TransformOverloadBenchData.Rot3f)),
            Create("Hull3fInverseScale", () => TransformOverloadBenchData.Hull3f.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f).Inverse), () => TransformOverloadBenchData.Hull3f.InvTransformed(TransformOverloadBenchData.Scale3f)),

            Create("Hull3dForwardEuclidean", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Hull3dForwardSimilarity", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Hull3dForwardAffine", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Hull3dForwardShift", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Hull3dForwardRot", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Hull3dForwardScale", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Scale3d)),
            Create("Hull3dInverseTrafo", () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Trafo3d.Inverse), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Trafo3d)),
            Create("Hull3dInverseEuclidean", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d).Inverse), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Hull3dInverseSimilarity", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d).Inverse), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Hull3dInverseShift", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d).Inverse), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Hull3dInverseRot", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d).Inverse), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Hull3dInverseScale", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d).Inverse), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            // FastHull3: direct wrappers versus explicit wrapped-hull baselines.
            Create("FastHull3fForwardTrafo", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(TransformOverloadBenchData.Trafo3f)), () => TransformOverloadBenchData.FastHull3f.Transformed(TransformOverloadBenchData.Trafo3f)),
            Create("FastHull3fForwardEuclidean", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f))), () => TransformOverloadBenchData.FastHull3f.Transformed(TransformOverloadBenchData.Euclidean3f)),
            Create("FastHull3fForwardSimilarity", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f))), () => TransformOverloadBenchData.FastHull3f.Transformed(TransformOverloadBenchData.Similarity3f)),
            Create("FastHull3fForwardAffine", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Affine3f))), () => TransformOverloadBenchData.FastHull3f.Transformed(TransformOverloadBenchData.Affine3f)),
            Create("FastHull3fForwardShift", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f))), () => TransformOverloadBenchData.FastHull3f.Transformed(TransformOverloadBenchData.Shift3f)),
            Create("FastHull3fForwardRot", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f))), () => TransformOverloadBenchData.FastHull3f.Transformed(TransformOverloadBenchData.Rot3f)),
            Create("FastHull3fForwardScale", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f))), () => TransformOverloadBenchData.FastHull3f.Transformed(TransformOverloadBenchData.Scale3f)),
            Create("FastHull3fInverseTrafo", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(TransformOverloadBenchData.Trafo3f.Inverse)), () => TransformOverloadBenchData.FastHull3f.InvTransformed(TransformOverloadBenchData.Trafo3f)),
            Create("FastHull3fInverseEuclidean", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f).Inverse)), () => TransformOverloadBenchData.FastHull3f.InvTransformed(TransformOverloadBenchData.Euclidean3f)),
            Create("FastHull3fInverseSimilarity", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f).Inverse)), () => TransformOverloadBenchData.FastHull3f.InvTransformed(TransformOverloadBenchData.Similarity3f)),
            Create("FastHull3fInverseShift", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f).Inverse)), () => TransformOverloadBenchData.FastHull3f.InvTransformed(TransformOverloadBenchData.Shift3f)),
            Create("FastHull3fInverseRot", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f).Inverse)), () => TransformOverloadBenchData.FastHull3f.InvTransformed(TransformOverloadBenchData.Rot3f)),
            Create("FastHull3fInverseScale", () => new FastHull3f(TransformOverloadBenchData.FastHull3f.Hull.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f).Inverse)), () => TransformOverloadBenchData.FastHull3f.InvTransformed(TransformOverloadBenchData.Scale3f)),

            Create("FastHull3dForwardTrafo", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(TransformOverloadBenchData.Trafo3d)), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Trafo3d)),
            Create("FastHull3dForwardEuclidean", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("FastHull3dForwardSimilarity", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("FastHull3dForwardAffine", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("FastHull3dForwardShift", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("FastHull3dForwardRot", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("FastHull3dForwardScale", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Scale3d)),
            Create("FastHull3dInverseTrafo", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(TransformOverloadBenchData.Trafo3d.Inverse)), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Trafo3d)),
            Create("FastHull3dInverseEuclidean", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d).Inverse)), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("FastHull3dInverseSimilarity", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d).Inverse)), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("FastHull3dInverseShift", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d).Inverse)), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("FastHull3dInverseRot", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d).Inverse)), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("FastHull3dInverseScale", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d).Inverse)), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            // Planes.
            Create("Plane3fForwardEuclidean", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f)), () => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Euclidean3f)),
            Create("Plane3fForwardSimilarity", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f)), () => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Similarity3f)),
            Create("Plane3fForwardAffine", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Affine3f)), () => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Affine3f)),
            Create("Plane3fForwardShift", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f)), () => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Shift3f)),
            Create("Plane3fForwardRot", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f)), () => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Rot3f)),
            Create("Plane3fForwardScale", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f)), () => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Scale3f)),
            Create("Plane3fInverseTrafo", () => TransformOverloadBenchData.Plane3f.Transformed(TransformOverloadBenchData.Trafo3f.Inverse), () => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Trafo3f)),
            Create("Plane3fInverseEuclidean", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f).Inverse), () => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Euclidean3f)),
            Create("Plane3fInverseSimilarity", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f).Inverse), () => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Similarity3f)),
            Create("Plane3fInverseShift", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f).Inverse), () => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Shift3f)),
            Create("Plane3fInverseRot", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f).Inverse), () => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Rot3f)),
            Create("Plane3fInverseScale", () => TransformOverloadBenchData.Plane3f.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f).Inverse), () => TransformOverloadBenchData.Plane3f.InvTransformed(TransformOverloadBenchData.Scale3f)),

            Create("Plane3dForwardEuclidean", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Plane3dForwardSimilarity", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Plane3dForwardAffine", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Plane3dForwardShift", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Plane3dForwardRot", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Plane3dForwardScale", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Scale3d)),
            Create("Plane3dInverseTrafo", () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Trafo3d.Inverse), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Trafo3d)),
            Create("Plane3dInverseEuclidean", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d).Inverse), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Plane3dInverseSimilarity", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d).Inverse), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Plane3dInverseShift", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d).Inverse), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Plane3dInverseRot", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d).Inverse), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Plane3dInverseScale", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d).Inverse), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            // Rays: new forward Trafo and inverse overloads only.
            Create("Ray3fForwardTrafo", () => TransformOverloadBenchData.Ray3f.Transformed(TransformOverloadBenchData.RayTrafo3f.Forward), () => TransformOverloadBenchData.Ray3f.Transformed(TransformOverloadBenchData.RayTrafo3f)),
            Create("Ray3fInverseTrafo", () => TransformOverloadBenchData.Ray3f.Transformed(TransformOverloadBenchData.RayTrafo3f.Backward), () => TransformOverloadBenchData.Ray3f.InvTransformed(TransformOverloadBenchData.RayTrafo3f)),
            Create("Ray3fInverseEuclidean", () => TransformOverloadBenchData.Ray3f.Transformed(new Trafo3f(TransformOverloadBenchData.Euclidean3f).Backward), () => TransformOverloadBenchData.Ray3f.InvTransformed(TransformOverloadBenchData.Euclidean3f)),
            Create("Ray3fInverseSimilarity", () => TransformOverloadBenchData.Ray3f.Transformed(new Trafo3f(TransformOverloadBenchData.Similarity3f).Backward), () => TransformOverloadBenchData.Ray3f.InvTransformed(TransformOverloadBenchData.Similarity3f)),
            Create("Ray3fInverseShift", () => TransformOverloadBenchData.Ray3f.Transformed(new Trafo3f(TransformOverloadBenchData.Shift3f).Backward), () => TransformOverloadBenchData.Ray3f.InvTransformed(TransformOverloadBenchData.Shift3f)),
            Create("Ray3fInverseRot", () => TransformOverloadBenchData.Ray3f.Transformed(new Trafo3f(TransformOverloadBenchData.Rot3f).Backward), () => TransformOverloadBenchData.Ray3f.InvTransformed(TransformOverloadBenchData.Rot3f)),
            Create("Ray3fInverseScale", () => TransformOverloadBenchData.Ray3f.Transformed(new Trafo3f(TransformOverloadBenchData.Scale3f).Backward), () => TransformOverloadBenchData.Ray3f.InvTransformed(TransformOverloadBenchData.Scale3f)),

            Create("Ray3dForwardTrafo", () => TransformOverloadBenchData.Ray3d.Transformed(TransformOverloadBenchData.RayTrafo3d.Forward), () => TransformOverloadBenchData.Ray3d.Transformed(TransformOverloadBenchData.RayTrafo3d)),
            Create("Ray3dInverseTrafo", () => TransformOverloadBenchData.Ray3d.Transformed(TransformOverloadBenchData.RayTrafo3d.Backward), () => TransformOverloadBenchData.Ray3d.InvTransformed(TransformOverloadBenchData.RayTrafo3d)),
            Create("Ray3dInverseEuclidean", () => TransformOverloadBenchData.Ray3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d).Backward), () => TransformOverloadBenchData.Ray3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Ray3dInverseSimilarity", () => TransformOverloadBenchData.Ray3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d).Backward), () => TransformOverloadBenchData.Ray3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Ray3dInverseShift", () => TransformOverloadBenchData.Ray3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d).Backward), () => TransformOverloadBenchData.Ray3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Ray3dInverseRot", () => TransformOverloadBenchData.Ray3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d).Backward), () => TransformOverloadBenchData.Ray3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Ray3dInverseScale", () => TransformOverloadBenchData.Ray3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d).Backward), () => TransformOverloadBenchData.Ray3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            // PolyRegion: instance inverse overloads versus the pre-existing inverse-matrix transform path.
            Create("PolyRegion2dInverseEuclidean", () => TransformOverloadBenchData.PolyRegion2d.Transformed(((M33d)TransformOverloadBenchData.Euclidean2d).Inverse), () => TransformOverloadBenchData.PolyRegion2d.InvTransformed(TransformOverloadBenchData.Euclidean2d)),
            Create("PolyRegion2dInverseSimilarity", () => TransformOverloadBenchData.PolyRegion2d.Transformed(((M33d)TransformOverloadBenchData.Similarity2d).Inverse), () => TransformOverloadBenchData.PolyRegion2d.InvTransformed(TransformOverloadBenchData.Similarity2d)),
            Create("PolyRegion2dInverseShift", () => TransformOverloadBenchData.PolyRegion2d.Transformed(((M33d)TransformOverloadBenchData.Shift2d).Inverse), () => TransformOverloadBenchData.PolyRegion2d.InvTransformed(TransformOverloadBenchData.Shift2d)),
            Create("PolyRegion2dInverseRot", () => TransformOverloadBenchData.PolyRegion2d.Transformed(((M33d)TransformOverloadBenchData.Rot2d).Inverse), () => TransformOverloadBenchData.PolyRegion2d.InvTransformed(TransformOverloadBenchData.Rot2d)),
            Create("PolyRegion2dInverseScale", () => TransformOverloadBenchData.PolyRegion2d.Transformed(((M33d)TransformOverloadBenchData.Scale2d).Inverse), () => TransformOverloadBenchData.PolyRegion2d.InvTransformed(TransformOverloadBenchData.Scale2d)),
        };

        public static bool TryHandle(string[] args)
        {
            if (args.Contains("--list-transform-perf-cases", StringComparer.OrdinalIgnoreCase))
            {
                foreach (var @case in Cases.OrderBy(static c => c.Name, StringComparer.OrdinalIgnoreCase))
                    Console.WriteLine(@case.Name);
                return true;
            }

            if (args.Contains("--verify-transform-perf-coverage", StringComparer.OrdinalIgnoreCase))
            {
                VerifyCoverage();
                return true;
            }

            if (!args.Contains("--targeted-transform-perf", StringComparer.OrdinalIgnoreCase))
                return false;

            VerifyCoverage();

            var settings = args.Contains("--quick", StringComparer.OrdinalIgnoreCase) ? QuickSettings : DefaultSettings;
            var filter = GetOption(args, "--case");
            var outputDir = GetOption(args, "--output-dir");
            if (string.IsNullOrWhiteSpace(outputDir))
                outputDir = Path.Combine("BenchmarkDotNet.Artifacts", "results");
            var selected = string.IsNullOrWhiteSpace(filter)
                ? Cases
                : Cases.Where(c => c.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToArray();

            if (selected.Length == 0)
                throw new InvalidOperationException($"No targeted transform perf cases matched '{filter}'.");

            Directory.CreateDirectory(outputDir);

            var results = new List<TransformOverloadPerfResult>(selected.Length);
            foreach (var @case in selected)
            {
                var result = @case.Run(settings);
                results.Add(result);
                Console.WriteLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-32} ops={1,8} old={2,10:F3} ns/op new={3,10:F3} ns/op ratio={4,6:F3} alloc={5,8:F3}/{6,8:F3} B/op",
                    result.Name,
                    result.Operations,
                    result.BaselineNanosecondsPerOperation,
                    result.SpecializedNanosecondsPerOperation,
                    result.Ratio,
                    result.BaselineAllocatedBytesPerOperation,
                    result.SpecializedAllocatedBytesPerOperation));
            }

            WriteReports(outputDir, results, settings);
            return true;
        }

        private static void VerifyCoverage()
        {
            var actualNames = Cases.Select(static c => c.Name).ToArray();
            var duplicateNames = actualNames.GroupBy(static n => n, StringComparer.OrdinalIgnoreCase)
                                           .Where(static g => g.Count() > 1)
                                           .Select(static g => g.Key)
                                           .OrderBy(static n => n, StringComparer.OrdinalIgnoreCase)
                                           .ToArray();
            var expectedNames = ExpectedCaseNames().ToArray();
            var actual = new HashSet<string>(actualNames, StringComparer.OrdinalIgnoreCase);
            var expected = new HashSet<string>(expectedNames, StringComparer.OrdinalIgnoreCase);
            var missing = expected.Except(actual, StringComparer.OrdinalIgnoreCase).OrderBy(static n => n, StringComparer.OrdinalIgnoreCase).ToArray();
            var extra = actual.Except(expected, StringComparer.OrdinalIgnoreCase).OrderBy(static n => n, StringComparer.OrdinalIgnoreCase).ToArray();

            if (duplicateNames.Length > 0 || missing.Length > 0 || extra.Length > 0)
            {
                var message = new StringBuilder("Transform perf case coverage mismatch.");
                if (duplicateNames.Length > 0) message.AppendLine().Append("Duplicate: ").Append(string.Join(", ", duplicateNames));
                if (missing.Length > 0) message.AppendLine().Append("Missing: ").Append(string.Join(", ", missing));
                if (extra.Length > 0) message.AppendLine().Append("Extra: ").Append(string.Join(", ", extra));
                throw new InvalidOperationException(message.ToString());
            }

            Console.WriteLine($"Transform perf coverage verified: {actual.Count} cases.");
        }

        private static IEnumerable<string> ExpectedCaseNames()
        {
            string[] forward = ["Euclidean", "Similarity", "Affine", "Shift", "Rot", "Scale"];
            string[] inverse = ["Trafo", "Euclidean", "Similarity", "Shift", "Rot", "Scale"];

            foreach (var box in new[] { "Box2i", "Box2l", "Box2f", "Box2d", "Box3i", "Box3l", "Box3f", "Box3d" })
            {
                foreach (var family in forward) yield return box + "Forward" + family;
                foreach (var family in inverse) yield return box + "Inverse" + family;
            }

            foreach (var hull in new[] { "Hull2f", "Hull2d", "Hull3f", "Hull3d" })
            {
                foreach (var family in forward) yield return hull + "Forward" + family;
                foreach (var family in inverse) yield return hull + "Inverse" + family;
            }

            foreach (var fastHull in new[] { "FastHull3f", "FastHull3d" })
            {
                foreach (var family in new[] { "Trafo", "Euclidean", "Similarity", "Affine", "Shift", "Rot", "Scale" }) yield return fastHull + "Forward" + family;
                foreach (var family in inverse) yield return fastHull + "Inverse" + family;
            }

            foreach (var plane in new[] { "Plane3f", "Plane3d" })
            {
                foreach (var family in forward) yield return plane + "Forward" + family;
                foreach (var family in inverse) yield return plane + "Inverse" + family;
            }

            foreach (var ray in new[] { "Ray3f", "Ray3d" })
            {
                yield return ray + "ForwardTrafo";
                foreach (var family in inverse) yield return ray + "Inverse" + family;
            }

            foreach (var family in new[] { "Euclidean", "Similarity", "Shift", "Rot", "Scale" })
                yield return "PolyRegion2dInverse" + family;
        }

        private static void WriteReports(string outputDir, IReadOnlyList<TransformOverloadPerfResult> results, TransformOverloadPerfSettings settings)
        {
            var generatedUtc = DateTime.UtcNow;
            var timestamp = generatedUtc.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            var jsonPath = Path.Combine(outputDir, $"TransformOverloadTargetedPerf-{timestamp}.json");
            var csvPath = Path.Combine(outputDir, $"TransformOverloadTargetedPerf-{timestamp}.csv");
            var markdownPath = Path.Combine(outputDir, $"TransformOverloadTargetedPerf-{timestamp}.md");

            var rows = results.Select(result =>
            {
                DescribeComparison(result.Name, out var oldPath, out var newOverload);
                return new
                {
                    Name = result.Name,
                    OldPath = oldPath,
                    NewOverload = newOverload,
                    Operations = result.Operations,
                    OldPathNsPerOp = result.BaselineNanosecondsPerOperation,
                    NewOverloadNsPerOp = result.SpecializedNanosecondsPerOperation,
                    Ratio = result.Ratio,
                    OldPathAllocatedBytesPerOp = result.BaselineAllocatedBytesPerOperation,
                    NewOverloadAllocatedBytesPerOp = result.SpecializedAllocatedBytesPerOperation,
                };
            }).ToArray();

            var report = new
            {
                FormatVersion = 1,
                GeneratedUtc = generatedUtc.ToString("O", CultureInfo.InvariantCulture),
                Settings = new
                {
                    settings.WarmupRounds,
                    settings.MeasurementRounds,
                    settings.TargetMillisecondsPerRound,
                },
                Count = rows.Length,
                Results = rows,
            };

            File.WriteAllText(jsonPath, JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));

            var csv = new StringBuilder();
            csv.AppendLine("Name,OldPath,NewOverload,Operations,OldPathNsPerOp,NewOverloadNsPerOp,Ratio,OldPathAllocatedBytesPerOp,NewOverloadAllocatedBytesPerOp");
            foreach (var row in rows)
            {
                AppendCsvField(csv, row.Name).Append(',');
                AppendCsvField(csv, row.OldPath).Append(',');
                AppendCsvField(csv, row.NewOverload).Append(',')
                   .Append(row.Operations.ToString(CultureInfo.InvariantCulture)).Append(',')
                   .Append(row.OldPathNsPerOp.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(row.NewOverloadNsPerOp.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(row.Ratio.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(row.OldPathAllocatedBytesPerOp.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(row.NewOverloadAllocatedBytesPerOp.ToString("F6", CultureInfo.InvariantCulture)).AppendLine();
            }
            File.WriteAllText(csvPath, csv.ToString());

            var markdown = new StringBuilder();
            markdown.AppendLine("# Transform Overload Old Path vs New Overload Perf");
            markdown.AppendLine();
            markdown.AppendLine("Canonical machine-readable result: `" + Path.GetFileName(jsonPath) + "`.");
            markdown.AppendLine($"Settings: warmup rounds = {settings.WarmupRounds}, measurement rounds = {settings.MeasurementRounds}, target round = {settings.TargetMillisecondsPerRound.ToString("F1", CultureInfo.InvariantCulture)} ms.");
            markdown.AppendLine();
            markdown.AppendLine("| Case | Old path | New overload | Ops | Old ns/op | New ns/op | Ratio | Old B/op | New B/op |");
            markdown.AppendLine("| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |");
            foreach (var row in rows)
            {
                markdown.Append("| ").Append(row.Name)
                        .Append(" | `").Append(row.OldPath).Append('`')
                        .Append(" | `").Append(row.NewOverload).Append('`')
                        .Append(" | ").Append(row.Operations.ToString(CultureInfo.InvariantCulture))
                        .Append(" | ").Append(row.OldPathNsPerOp.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(row.NewOverloadNsPerOp.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(row.Ratio.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(row.OldPathAllocatedBytesPerOp.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(row.NewOverloadAllocatedBytesPerOp.ToString("F3", CultureInfo.InvariantCulture))
                        .AppendLine(" |");
            }
            File.WriteAllText(markdownPath, markdown.ToString());

            Console.WriteLine($"Wrote targeted perf JSON to {jsonPath}");
            Console.WriteLine($"Wrote derived CSV/Markdown reports to {csvPath} and {markdownPath}");
        }

        private static StringBuilder AppendCsvField(StringBuilder builder, string value)
            => builder.Append('"').Append(value.Replace("\"", "\"\"", StringComparison.Ordinal)).Append('"');

        private static void DescribeComparison(string caseName, out string oldPath, out string newOverload)
        {
            var forwardIndex = caseName.IndexOf("Forward", StringComparison.Ordinal);
            var inverseIndex = caseName.IndexOf("Inverse", StringComparison.Ordinal);
            if (forwardIndex < 0 && inverseIndex < 0)
                throw new InvalidOperationException($"Could not parse transform perf case name '{caseName}'.");

            var isForward = forwardIndex >= 0;
            var splitIndex = isForward ? forwardIndex : inverseIndex;
            var subject = caseName[..splitIndex];
            var family = caseName[(splitIndex + (isForward ? "Forward".Length : "Inverse".Length))..];
            var dimension = GetDimension(subject);
            var scalarSuffix = GetScalarSuffix(subject);
            var matrix = $"M{dimension + 1}{dimension + 1}{scalarSuffix}";
            var trafo = $"Trafo{dimension}{scalarSuffix}";
            var transform = family == "Trafo" ? trafo : $"{family}{dimension}{scalarSuffix}";

            if (subject.StartsWith("Box", StringComparison.Ordinal))
            {
                oldPath = isForward
                    ? $"Transformed(({matrix}){transform})"
                    : family == "Trafo"
                        ? $"Transformed({trafo}.Backward)"
                        : $"Transformed((({matrix}){transform}).Inverse)";
                newOverload = isForward ? $"Transformed({transform})" : $"InvTransformed({transform})";
                return;
            }

            if (subject.StartsWith("FastHull", StringComparison.Ordinal))
            {
                oldPath = isForward
                    ? family == "Trafo"
                        ? $"new {subject}(Hull.Transformed({trafo}))"
                        : $"new {subject}(Hull.Transformed(new {trafo}({transform})))"
                    : family == "Trafo"
                        ? $"new {subject}(Hull.Transformed({trafo}.Inverse))"
                        : $"new {subject}(Hull.Transformed(new {trafo}({transform}).Inverse))";
                newOverload = isForward ? $"Transformed({transform})" : $"InvTransformed({transform})";
                return;
            }

            if (subject.StartsWith("Hull", StringComparison.Ordinal) || subject.StartsWith("Plane", StringComparison.Ordinal))
            {
                oldPath = isForward
                    ? $"Transformed(new {trafo}({transform}))"
                    : family == "Trafo"
                        ? $"Transformed({trafo}.Inverse)"
                        : $"Transformed(new {trafo}({transform}).Inverse)";
                newOverload = isForward ? $"Transformed({transform})" : $"InvTransformed({transform})";
                return;
            }

            if (subject.StartsWith("Ray", StringComparison.Ordinal))
            {
                oldPath = isForward
                    ? $"Transformed({trafo}.Forward)"
                    : family == "Trafo"
                        ? $"Transformed({trafo}.Backward)"
                        : $"Transformed(new {trafo}({transform}).Backward)";
                newOverload = isForward ? $"Transformed({transform})" : $"InvTransformed({transform})";
                return;
            }

            if (subject == "PolyRegion2d")
            {
                oldPath = $"Transformed(((M33d){transform}).Inverse)";
                newOverload = $"InvTransformed({transform})";
                return;
            }

            throw new InvalidOperationException($"Unsupported transform perf case subject '{subject}'.");
        }

        private static int GetDimension(string subject)
            => subject.Contains('2', StringComparison.Ordinal) ? 2 : 3;

        private static string GetScalarSuffix(string subject)
            => subject.EndsWith("f", StringComparison.Ordinal) ? "f" : "d";

        private static string GetOption(string[] args, string name)
        {
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
                    return args[i + 1];
            }

            return string.Empty;
        }

        private static ITransformOverloadPerfCase Create<T>(string name, Func<T> baseline, Func<T> specialized)
            => new TransformOverloadPerfCase<T>(name, baseline, specialized);
    }

    public readonly struct TransformOverloadPerfSettings
    {
        public TransformOverloadPerfSettings(int warmupRounds, int measurementRounds, double targetMillisecondsPerRound)
        {
            WarmupRounds = warmupRounds;
            MeasurementRounds = measurementRounds;
            TargetMillisecondsPerRound = targetMillisecondsPerRound;
        }

        public int WarmupRounds { get; }
        public int MeasurementRounds { get; }
        public double TargetMillisecondsPerRound { get; }
    }

    public readonly struct TransformOverloadPerfResult
    {
        public TransformOverloadPerfResult(string name, int operations, double baselineNanosecondsPerOperation, double specializedNanosecondsPerOperation, double baselineAllocatedBytesPerOperation, double specializedAllocatedBytesPerOperation)
        {
            Name = name;
            Operations = operations;
            BaselineNanosecondsPerOperation = baselineNanosecondsPerOperation;
            SpecializedNanosecondsPerOperation = specializedNanosecondsPerOperation;
            BaselineAllocatedBytesPerOperation = baselineAllocatedBytesPerOperation;
            SpecializedAllocatedBytesPerOperation = specializedAllocatedBytesPerOperation;
        }

        public string Name { get; }
        public int Operations { get; }
        public double BaselineNanosecondsPerOperation { get; }
        public double SpecializedNanosecondsPerOperation { get; }
        public double BaselineAllocatedBytesPerOperation { get; }
        public double SpecializedAllocatedBytesPerOperation { get; }
        public double Ratio => SpecializedNanosecondsPerOperation / BaselineNanosecondsPerOperation;
    }

    internal interface ITransformOverloadPerfCase
    {
        string Name { get; }
        TransformOverloadPerfResult Run(TransformOverloadPerfSettings settings);
    }

    internal sealed class TransformOverloadPerfCase<T> : ITransformOverloadPerfCase
    {
        private readonly Func<T> _baseline;
        private readonly Func<T> _specialized;

        public TransformOverloadPerfCase(string name, Func<T> baseline, Func<T> specialized)
        {
            Name = name;
            _baseline = baseline;
            _specialized = specialized;
        }

        public string Name { get; }

        public TransformOverloadPerfResult Run(TransformOverloadPerfSettings settings)
        {
            Consume(_baseline());
            Consume(_specialized());

            var operations = CalibrateOperations(settings.TargetMillisecondsPerRound);

            for (var i = 0; i < settings.WarmupRounds; i++)
            {
                Measure(_baseline, operations, collectAllocations: false);
                Measure(_specialized, operations, collectAllocations: false);
            }

            var baselineTimes = new double[settings.MeasurementRounds];
            var specializedTimes = new double[settings.MeasurementRounds];
            var baselineAllocations = new double[settings.MeasurementRounds];
            var specializedAllocations = new double[settings.MeasurementRounds];

            for (var i = 0; i < settings.MeasurementRounds; i++)
            {
                var baseline = Measure(_baseline, operations, collectAllocations: true);
                var specialized = Measure(_specialized, operations, collectAllocations: true);

                baselineTimes[i] = baseline.Elapsed.TotalMilliseconds * 1_000_000.0 / operations;
                specializedTimes[i] = specialized.Elapsed.TotalMilliseconds * 1_000_000.0 / operations;
                baselineAllocations[i] = baseline.AllocatedBytes / (double)operations;
                specializedAllocations[i] = specialized.AllocatedBytes / (double)operations;
            }

            Array.Sort(baselineTimes);
            Array.Sort(specializedTimes);
            Array.Sort(baselineAllocations);
            Array.Sort(specializedAllocations);

            return new TransformOverloadPerfResult(
                Name,
                operations,
                baselineTimes[settings.MeasurementRounds / 2],
                specializedTimes[settings.MeasurementRounds / 2],
                baselineAllocations[settings.MeasurementRounds / 2],
                specializedAllocations[settings.MeasurementRounds / 2]);
        }

        private int CalibrateOperations(double targetMillisecondsPerRound)
        {
            var operations = 1;
            while (operations < 1 << 26)
            {
                var measurement = Measure(_baseline, operations, collectAllocations: false);
                if (measurement.Elapsed.TotalMilliseconds >= targetMillisecondsPerRound)
                    return operations;
                operations <<= 1;
            }

            return operations;
        }

        private static PerfMeasurement Measure(Func<T> func, int operations, bool collectAllocations)
        {
            if (collectAllocations)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            var allocatedBefore = collectAllocations ? GC.GetAllocatedBytesForCurrentThread() : 0L;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < operations; i++)
                Consume(func());
            stopwatch.Stop();
            var allocatedAfter = collectAllocations ? GC.GetAllocatedBytesForCurrentThread() : allocatedBefore;

            return new PerfMeasurement(stopwatch.Elapsed, allocatedAfter - allocatedBefore);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Consume(T value) => PerfSink<T>.Value = value;
    }

    internal readonly struct PerfMeasurement
    {
        public PerfMeasurement(TimeSpan elapsed, long allocatedBytes)
        {
            Elapsed = elapsed;
            AllocatedBytes = allocatedBytes;
        }

        public TimeSpan Elapsed { get; }
        public long AllocatedBytes { get; }
    }

    internal static class PerfSink<T>
    {
        public static T Value = default!;
    }
}

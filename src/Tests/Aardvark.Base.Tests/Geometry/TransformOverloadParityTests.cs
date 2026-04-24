using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class TransformOverloadParityTests
    {
        private static readonly Dictionary<Type, object> TransformSamples = CreateTransformSamples();

        private static readonly object[] BoxSamples =
        {
            new Box2i(new V2i(-3, 2), new V2i(5, 9)),
            new Box2l(new V2l(-7, 3), new V2l(4, 12)),
            new Box2f(new V2f(-3.5f, 2.25f), new V2f(5.5f, 9.75f)),
            new Box2d(new V2d(-6.25, 1.5), new V2d(3.75, 8.125)),
            new Box3i(new V3i(-4, 2, -1), new V3i(6, 9, 7)),
            new Box3l(new V3l(-9, 3, -5), new V3l(5, 12, 11)),
            new Box3f(new V3f(-4.5f, 2.25f, -1.75f), new V3f(6.5f, 9.75f, 7.25f)),
            new Box3d(new V3d(-7.25, 1.5, -2.5), new V3d(4.5, 8.125, 6.75)),
        };

        private static readonly object[] Hull2Samples =
        {
            new Hull2f(new Box2f(new V2f(-2.5f, -1.5f), new V2f(4.0f, 3.25f))),
            new Hull2d(new Box2d(new V2d(-3.5, -2.25), new V2d(5.5, 4.75))),
        };

        private static readonly object[] Hull3Samples =
        {
            new Hull3f(new Box3f(new V3f(-2.5f, -1.5f, -3.25f), new V3f(4.0f, 3.25f, 2.75f))),
            new Hull3d(new Box3d(new V3d(-3.5, -2.25, -4.75), new V3d(5.5, 4.75, 3.5))),
        };

        private static readonly object[] FastHull3Samples =
        {
            new FastHull3f(new Hull3f(new Box3f(new V3f(-2.5f, -1.5f, -3.25f), new V3f(4.0f, 3.25f, 2.75f)))),
            new FastHull3d(new Hull3d(new Box3d(new V3d(-3.5, -2.25, -4.75), new V3d(5.5, 4.75, 3.5)))),
        };

        private static readonly object[] PlaneSamples =
        {
            new Plane3f(new V3f(1.0f, -2.0f, 3.0f).Normalized, new V3f(-1.5f, 0.75f, 2.25f)),
            new Plane3d(new V3d(1.0, -2.0, 3.0).Normalized, new V3d(-1.5, 0.75, 2.25)),
        };

        private static readonly object[] RaySamples =
        {
            new Ray3f(new V3f(-1.5f, 2.25f, -0.5f), new V3f(0.75f, -2.0f, 3.5f)),
            new Ray3d(new V3d(-1.5, 2.25, -0.5), new V3d(0.75, -2.0, 3.5)),
        };

        [Test]
        public void BoxForwardOverloadsMatchMatrixPath()
        {
            foreach (var box in BoxSamples)
            {
                foreach (var transformType in GetBoxForwardTransformTypes(box.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(box, nameof(Box3d.Transformed), transform);
                    var expected = InvokeInstance(box, nameof(Box3d.Transformed), ToForwardMatrix(transform));
                    AssertBoxesEquivalent(actual, expected, $"{box.GetType().Name}.Transformed({transformType.Name})");
                }
            }
        }

        [Test]
        public void BoxInverseOverloadsMatchInverseMatrixPath()
        {
            foreach (var box in BoxSamples)
            {
                foreach (var transformType in GetBoxInverseTransformTypes(box.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(box, "InvTransformed", transform);
                    var expected = InvokeInstance(box, nameof(Box3d.Transformed), transform is Trafo2f or Trafo2d or Trafo3f or Trafo3d ? GetBackwardMatrix(transform) : ToInverseMatrix(transform));
                    AssertBoxesEquivalent(actual, expected, $"{box.GetType().Name}.InvTransformed({transformType.Name})");
                }
            }
        }

        [Test]
        public void Box2InvalidLinearInverseOverloadsRemainInvalid()
        {
            var invalidInt = Box2i.Invalid;
            var infiniteInt = Box2i.Infinite;
            var crossedIntX = new Box2i(new V2i(2, -1), new V2i(1, 3));
            var crossedIntY = new Box2i(new V2i(-1, 3), new V2i(2, 1));
            var validInt = new Box2i(new V2i(-2, -1), new V2i(3, 4));
            var crossedX = new Box2d(new V2d(2.0, -1.0), new V2d(1.0, 3.0));
            var crossedY = new Box2d(new V2d(-1.0, 3.0), new V2d(2.0, 1.0));
            var affine = new Affine2d(new M22d(1.2, 0.35, -0.2, 0.9), new V2d(5.0, -2.0));

            Assert.That(invalidInt.InvTransformed(Rot2d.FromDegrees(17.0)), Is.EqualTo(Box2d.Invalid));
            Assert.That(invalidInt.InvTransformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.InvTransformed(Rot2d.FromDegrees(17.0)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.InvTransformed(new Euclidean2d(Rot2d.FromDegrees(17.0), new V2d(2.5, -1.75))), Is.EqualTo(Box2d.Invalid));
            Assert.That(invalidInt.Transformed(new Euclidean2d(Rot2d.FromDegrees(17.0), new V2d(2.5, -1.75))), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.Transformed(new Euclidean2d(Rot2d.FromDegrees(17.0), new V2d(2.5, -1.75))), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.Transformed(new Euclidean2d(Rot2d.FromDegrees(17.0), new V2d(2.5, -1.75))), Is.EqualTo(Box2d.Invalid));
            Assert.That(invalidInt.Transformed(new Similarity2d(-0.65, Rot2d.FromDegrees(-21.0), new V2d(-3.0, 4.25))), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.Transformed(new Similarity2d(-0.65, Rot2d.FromDegrees(-21.0), new V2d(-3.0, 4.25))), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.Transformed(new Similarity2d(-0.65, Rot2d.FromDegrees(-21.0), new V2d(-3.0, 4.25))), Is.EqualTo(Box2d.Invalid));
            Assert.That(invalidInt.Transformed(affine), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.Transformed(affine), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.Transformed(affine), Is.EqualTo(Box2d.Invalid));
            Assert.That(invalidInt.Transformed(new Shift2d(3.5, -1.25)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.Transformed(new Shift2d(3.5, -1.25)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.Transformed(new Shift2d(3.5, -1.25)), Is.EqualTo(Box2d.Invalid));
            Assert.That(invalidInt.Transformed(Rot2d.FromDegrees(17.0)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.Transformed(Rot2d.FromDegrees(17.0)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.Transformed(Rot2d.FromDegrees(17.0)), Is.EqualTo(Box2d.Invalid));
            Assert.That(invalidInt.Transformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.Transformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.Transformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
            AssertBoxesEquivalent(
                validInt.Transformed(new Euclidean2d(Rot2d.FromDegrees(137.0), new V2d(2.5, -1.75))),
                validInt.Transformed((M33d)new Euclidean2d(Rot2d.FromDegrees(137.0), new V2d(2.5, -1.75))),
                "Box2i.Transformed(Euclidean2d negative cosine)");
            AssertBoxesEquivalent(
                validInt.Transformed(new Euclidean2d(Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75))),
                validInt.Transformed((M33d)new Euclidean2d(Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75))),
                "Box2i.Transformed(Euclidean2d positive sine)");
            AssertBoxesEquivalent(
                validInt.Transformed(new Similarity2d(1.25, Rot2d.FromDegrees(137.0), new V2d(2.5, -1.75))),
                validInt.Transformed((M33d)new Similarity2d(1.25, Rot2d.FromDegrees(137.0), new V2d(2.5, -1.75))),
                "Box2i.Transformed(Similarity2d positive scale)");
            AssertBoxesEquivalent(
                validInt.Transformed(new Similarity2d(1.25, Rot2d.FromDegrees(17.0), new V2d(2.5, -1.75))),
                validInt.Transformed((M33d)new Similarity2d(1.25, Rot2d.FromDegrees(17.0), new V2d(2.5, -1.75))),
                "Box2i.Transformed(Similarity2d positive cosine)");
            AssertBoxesEquivalent(
                validInt.Transformed(new Similarity2d(1.25, Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75))),
                validInt.Transformed((M33d)new Similarity2d(1.25, Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75))),
                "Box2i.Transformed(Similarity2d positive sine)");
            AssertBoxesEquivalent(
                validInt.Transformed(new Similarity2d(-1.25, Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75))),
                validInt.Transformed((M33d)new Similarity2d(-1.25, Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75))),
                "Box2i.Transformed(Similarity2d negative scale)");
            AssertBoxesEquivalent(
                validInt.Transformed(Rot2d.FromDegrees(137.0)),
                validInt.Transformed((M33d)Rot2d.FromDegrees(137.0)),
                "Box2i.Transformed(Rot2d negative cosine)");
            AssertBoxesEquivalent(
                validInt.Transformed(Rot2d.FromDegrees(-17.0)),
                validInt.Transformed((M33d)Rot2d.FromDegrees(-17.0)),
                "Box2i.Transformed(Rot2d positive sine)");
            AssertBoxesEquivalent(
                validInt.Transformed(new Scale2d(1.5, -0.8)),
                validInt.Transformed((M33d)new Scale2d(1.5, -0.8)),
                "Box2i.Transformed(Scale2d negative Y)");
            AssertBoxesEquivalent(
                infiniteInt.Transformed(affine),
                infiniteInt.Transformed((M33d)affine),
                "Box2i.Transformed(Affine2d infinite fallback)");
            AssertBoxesEquivalent(
                validInt.InvTransformed(Rot2d.FromDegrees(-17.0)),
                validInt.Transformed(((M33d)Rot2d.FromDegrees(-17.0)).Inverse),
                "Box2i.InvTransformed(Rot2d negative angle)");
            AssertBoxesEquivalent(
                validInt.InvTransformed(new Scale2d(1.5, -0.8)),
                validInt.Transformed(((M33d)new Scale2d(1.5, -0.8)).Inverse),
                "Box2i.InvTransformed(Scale2d negative Y)");
            Assert.That(invalidInt.InvTransformed(new Shift2d(3.5, -1.25)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.InvTransformed(new Shift2d(3.5, -1.25)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.InvTransformed(new Shift2d(3.5, -1.25)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntX.InvTransformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedIntY.InvTransformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
            Assert.That(Box2d.Invalid.InvTransformed(Rot2d.FromDegrees(17.0)), Is.EqualTo(Box2d.Invalid));
            Assert.That(Box2d.Invalid.InvTransformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedX.InvTransformed(Rot2d.FromDegrees(17.0)), Is.EqualTo(Box2d.Invalid));
            Assert.That(crossedY.InvTransformed(new Scale2d(-1.5, 0.8)), Is.EqualTo(Box2d.Invalid));
        }

        [Test]
        public void Box2DirectControlFlowCoverageCoversAllNumericKinds()
        {
            foreach (var boxType in BoxSamples.Select(static b => b.GetType()).Where(static t => t.Name.StartsWith("Box2", StringComparison.Ordinal)))
            {
                var valid = CreateValidBox2(boxType);
                var invalid = GetStaticPropertyValue(boxType, "Invalid");
                var infinite = GetStaticPropertyValue(boxType, "Infinite");
                var crossedX = CreateCrossedBox2(boxType, crossX: true);
                var crossedY = CreateCrossedBox2(boxType, crossX: false);
                var invalidCandidates = new[] { invalid, crossedX, crossedY };

                foreach (var transformType in GetBoxForwardTransformTypes(boxType))
                {
                    var transform = TransformSamples[transformType];
                    var returnType = boxType.GetMethod(nameof(Box3d.Transformed), new[] { transformType })!.ReturnType;
                    var expectedInvalid = GetStaticPropertyValue(returnType, "Invalid");

                    foreach (var candidate in invalidCandidates)
                    {
                        var actual = InvokeInstance(candidate, nameof(Box3d.Transformed), transform);
                        AssertBoxesEquivalent(actual, expectedInvalid, $"{boxType.Name}.Transformed({transformType.Name}) invalid");
                    }
                }

                foreach (var transformType in GetBoxInverseTransformTypes(boxType))
                {
                    var transform = TransformSamples[transformType];
                    var returnType = boxType.GetMethod("InvTransformed", new[] { transformType })!.ReturnType;
                    var expectedInvalid = GetStaticPropertyValue(returnType, "Invalid");

                    foreach (var candidate in invalidCandidates)
                    {
                        var actual = InvokeInstance(candidate, "InvTransformed", transform);
                        AssertBoxesEquivalent(actual, expectedInvalid, $"{boxType.Name}.InvTransformed({transformType.Name}) invalid");
                    }
                }

                var affine = GetAffine2CoverageTransform(boxType);
                var affineMethod = boxType.GetMethod(nameof(Box3d.Transformed), new[] { affine.GetType() })!;
                var infiniteActual = InvokeInstance(infinite, nameof(Box3d.Transformed), affine);
                Assert.That(infiniteActual.GetType(), Is.EqualTo(affineMethod.ReturnType), $"{boxType.Name}.Transformed(Affine2*) infinite");

                foreach (var transform in GetAdditionalBox2ForwardCoverageTransforms(boxType))
                {
                    AssertBoxesEquivalent(
                        InvokeInstance(valid, nameof(Box3d.Transformed), transform),
                        InvokeInstance(valid, nameof(Box3d.Transformed), ToForwardMatrix(transform)),
                        $"{boxType.Name}.Transformed({transform.GetType().Name}) sign coverage");
                }

                foreach (var transform in GetAdditionalBox2InverseCoverageTransforms(boxType))
                {
                    AssertBoxesEquivalent(
                        InvokeInstance(valid, "InvTransformed", transform),
                        InvokeInstance(valid, nameof(Box3d.Transformed), ToInverseMatrix(transform)),
                        $"{boxType.Name}.InvTransformed({transform.GetType().Name}) sign coverage");
                }
            }
        }

        [Test]
        public void Box3DirectControlFlowCoverageCoversAllNumericKinds()
        {
            foreach (var boxType in BoxSamples.Select(static b => b.GetType()).Where(static t => t.Name.StartsWith("Box3", StringComparison.Ordinal)))
            {
                var valid = CreateValidBox3(boxType);
                var invalid = GetStaticPropertyValue(boxType, "Invalid");
                var crossedX = CreateCrossedBox3(boxType, 0);
                var crossedY = CreateCrossedBox3(boxType, 1);
                var crossedZ = CreateCrossedBox3(boxType, 2);
                var invalidCandidates = new[] { invalid, crossedX, crossedY, crossedZ };

                foreach (var transformType in GetBoxForwardTransformTypes(boxType))
                {
                    var transform = TransformSamples[transformType];
                    var returnType = boxType.GetMethod(nameof(Box3d.Transformed), new[] { transformType })!.ReturnType;
                    var expectedInvalid = GetStaticPropertyValue(returnType, "Invalid");

                    foreach (var candidate in invalidCandidates)
                    {
                        var actual = InvokeInstance(candidate, nameof(Box3d.Transformed), transform);
                        AssertBoxesEquivalent(actual, expectedInvalid, $"{boxType.Name}.Transformed({transformType.Name}) invalid");
                    }
                }

                foreach (var transformType in GetBoxInverseTransformTypes(boxType))
                {
                    var transform = TransformSamples[transformType];
                    var returnType = boxType.GetMethod("InvTransformed", new[] { transformType })!.ReturnType;
                    var expectedInvalid = GetStaticPropertyValue(returnType, "Invalid");

                    foreach (var candidate in invalidCandidates)
                    {
                        var actual = InvokeInstance(candidate, "InvTransformed", transform);
                        AssertBoxesEquivalent(actual, expectedInvalid, $"{boxType.Name}.InvTransformed({transformType.Name}) invalid");
                    }
                }

                foreach (var transform in GetAdditionalBox3ForwardCoverageTransforms(boxType))
                {
                    AssertBoxesEquivalent(
                        InvokeInstance(valid, nameof(Box3d.Transformed), transform),
                        InvokeInstance(valid, nameof(Box3d.Transformed), ToForwardMatrix(transform)),
                        $"{boxType.Name}.Transformed({transform.GetType().Name}) sign coverage");
                }

                foreach (var transform in GetAdditionalBox3InverseCoverageTransforms(boxType))
                {
                    AssertBoxesEquivalent(
                        InvokeInstance(valid, "InvTransformed", transform),
                        InvokeInstance(valid, nameof(Box3d.Transformed), ToInverseMatrix(transform)),
                        $"{boxType.Name}.InvTransformed({transform.GetType().Name}) sign coverage");
                }
            }
        }

        [Test]
        public void HullForwardOverloadsMatchTrafoPath()
        {
            foreach (var hull in Hull2Samples.Concat(Hull3Samples))
            {
                foreach (var transformType in GetHullForwardTransformTypes(hull.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(hull, nameof(Hull3d.Transformed), transform);
                    var expected = InvokeInstance(hull, nameof(Hull3d.Transformed), ToTrafo(transform));
                    AssertHullsEquivalent(actual, expected, $"{hull.GetType().Name}.Transformed({transformType.Name})");
                }
            }
        }

        [Test]
        public void HullInverseOverloadsMatchInverseTrafoPath()
        {
            foreach (var hull in Hull2Samples.Concat(Hull3Samples))
            {
                foreach (var transformType in GetHullInverseTransformTypes(hull.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(hull, "InvTransformed", transform);
                    var expected = InvokeInstance(hull, nameof(Hull3d.Transformed), ToInverseTrafo(transform));
                    AssertHullsEquivalent(actual, expected, $"{hull.GetType().Name}.InvTransformed({transformType.Name})");
                }
            }
        }

        [Test]
        public void FastHullForwardOverloadsMatchHullPath()
        {
            foreach (var fastHull in FastHull3Samples)
            {
                foreach (var transformType in GetFastHullForwardTransformTypes(fastHull.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(fastHull, nameof(FastHull3d.Transformed), transform);
                    object expected = fastHull switch
                    {
                        FastHull3f hull => new FastHull3f(hull.Hull.Transformed((dynamic)ToTrafo(transform))),
                        FastHull3d hull => new FastHull3d(hull.Hull.Transformed((dynamic)ToTrafo(transform))),
                        _ => throw new NotSupportedException(fastHull.GetType().FullName)
                    };
                    AssertHullsEquivalent(actual, expected, $"{fastHull.GetType().Name}.Transformed({transformType.Name})");
                }
            }
        }

        [Test]
        public void FastHullInverseOverloadsMatchInverseHullPath()
        {
            foreach (var fastHull in FastHull3Samples)
            {
                foreach (var transformType in GetFastHullInverseTransformTypes(fastHull.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(fastHull, "InvTransformed", transform);
                    object expected = fastHull switch
                    {
                        FastHull3f hull => new FastHull3f(hull.Hull.Transformed((dynamic)ToInverseTrafo(transform))),
                        FastHull3d hull => new FastHull3d(hull.Hull.Transformed((dynamic)ToInverseTrafo(transform))),
                        _ => throw new NotSupportedException(fastHull.GetType().FullName)
                    };
                    AssertHullsEquivalent(actual, expected, $"{fastHull.GetType().Name}.InvTransformed({transformType.Name})");
                }
            }
        }

        [Test]
        public void PlaneOverloadsMatchTrafoPath()
        {
            foreach (var plane in PlaneSamples)
            {
                foreach (var transformType in GetPlaneForwardTransformTypes(plane.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(plane, nameof(Plane3d.Transformed), transform);
                    var expected = InvokeInstance(plane, nameof(Plane3d.Transformed), ToTrafo(transform));
                    AssertPlanesEquivalent(actual, expected, $"{plane.GetType().Name}.Transformed({transformType.Name})");
                }

                foreach (var transformType in GetPlaneInverseTransformTypes(plane.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(plane, "InvTransformed", transform);
                    var expected = InvokeInstance(plane, nameof(Plane3d.Transformed), ToInverseTrafo(transform));
                    AssertPlanesEquivalent(actual, expected, $"{plane.GetType().Name}.InvTransformed({transformType.Name})");
                }
            }
        }

        [Test]
        public void RayOverloadsMatchPointAndDirectionHelpers()
        {
            foreach (var ray in RaySamples)
            {
                foreach (var transformType in GetRayForwardTransformTypes(ray.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(ray, nameof(Ray3d.Transformed), transform);
                    object expected = ray switch
                    {
                        Ray3f r when transform is Trafo3f t => new Ray3f(t.TransformPos(r.Origin), t.TransformDir(r.Direction)),
                        Ray3d r when transform is Trafo3d t => new Ray3d(t.TransformPos(r.Origin), t.TransformDir(r.Direction)),
                        _ => throw new NotSupportedException($"{ray.GetType().Name}/{transformType.Name}")
                    };
                    AssertRaysEquivalent(actual, expected, $"{ray.GetType().Name}.Transformed({transformType.Name})");
                }

                foreach (var transformType in GetRayInverseTransformTypes(ray.GetType()))
                {
                    var transform = TransformSamples[transformType];
                    var actual = InvokeInstance(ray, "InvTransformed", transform);
                    object expected = ray switch
                    {
                        Ray3f r => (object)(transform switch
                        {
                            Trafo3f t => new Ray3f(t.InvTransformPos(r.Origin), t.InvTransformDir(r.Direction)),
                            Euclidean3f t => new Ray3f(t.InvTransformPos(r.Origin), t.InvTransformDir(r.Direction)),
                            Similarity3f t => new Ray3f(t.InvTransformPos(r.Origin), t.InvTransformDir(r.Direction)),
                            Shift3f t => new Ray3f(t.Inverse.Transform(r.Origin), r.Direction),
                            Rot3f t => new Ray3f(t.InvTransform(r.Origin), t.InvTransform(r.Direction)),
                            Scale3f t => new Ray3f(t.InvTransform(r.Origin), t.InvTransform(r.Direction)),
                            _ => throw new NotSupportedException(transformType.Name)
                        }),
                        Ray3d r => (object)(transform switch
                        {
                            Trafo3d t => new Ray3d(t.InvTransformPos(r.Origin), t.InvTransformDir(r.Direction)),
                            Euclidean3d t => new Ray3d(t.InvTransformPos(r.Origin), t.InvTransformDir(r.Direction)),
                            Similarity3d t => new Ray3d(t.InvTransformPos(r.Origin), t.InvTransformDir(r.Direction)),
                            Shift3d t => new Ray3d(t.Inverse.Transform(r.Origin), r.Direction),
                            Rot3d t => new Ray3d(t.InvTransform(r.Origin), t.InvTransform(r.Direction)),
                            Scale3d t => new Ray3d(t.InvTransform(r.Origin), t.InvTransform(r.Direction)),
                            _ => throw new NotSupportedException(transformType.Name)
                        }),
                        _ => throw new NotSupportedException(ray.GetType().FullName)
                    };

                    AssertRaysEquivalent(actual, expected, $"{ray.GetType().Name}.InvTransformed({transformType.Name})");
                }
            }
        }

        private static Dictionary<Type, object> CreateTransformSamples()
        {
            var rot2d = Rot2d.FromDegrees(37.0);
            var rot2f = Rot2f.FromDegrees(37.0f);
            var euclidean2d = new Euclidean2d(rot2d, new V2d(2.5, -1.75));
            var euclidean2f = new Euclidean2f(rot2f, new V2f(2.5f, -1.75f));
            var similarity2d = new Similarity2d(-0.65, Rot2d.FromDegrees(-21.0), new V2d(-3.0, 4.25));
            var similarity2f = new Similarity2f(-0.65f, Rot2f.FromDegrees(-21.0f), new V2f(-3.0f, 4.25f));
            var affine2d = new Affine2d(new M22d(1.2, 0.35, -0.2, 0.9), new V2d(5.0, -2.0));
            var affine2f = new Affine2f(new M22f(1.2f, 0.35f, -0.2f, 0.9f), new V2f(5.0f, -2.0f));
            var shift2d = new Shift2d(3.5, -1.25);
            var shift2f = new Shift2f(3.5f, -1.25f);
            var scale2d = new Scale2d(-1.5, 0.8);
            var scale2f = new Scale2f(-1.5f, 0.8f);
            var trafo2d = new Trafo2d(new Affine2d(new M22d(1.15, 0.2, -0.15, 0.95), new V2d(-1.5, 2.0)));
            var trafo2f = new Trafo2f(new Affine2f(new M22f(1.15f, 0.2f, -0.15f, 0.95f), new V2f(-1.5f, 2.0f)));

            var rot3d = Rot3d.Rotation(new V3d(0.3, -0.5, 0.8).Normalized, 0.41);
            var rot3f = Rot3f.Rotation(new V3f(0.3f, -0.5f, 0.8f).Normalized, 0.41f);
            var euclidean3d = new Euclidean3d(rot3d, new V3d(2.5, -1.75, 4.0));
            var euclidean3f = new Euclidean3f(rot3f, new V3f(2.5f, -1.75f, 4.0f));
            var similarity3d = new Similarity3d(-0.65, rot3d, new V3d(-3.0, 4.25, 1.5));
            var similarity3f = new Similarity3f(-0.65f, rot3f, new V3f(-3.0f, 4.25f, 1.5f));
            var affine3d = new Affine3d(new M33d(1.2, 0.35, -0.1, -0.2, 0.9, 0.15, 0.05, -0.25, 1.1), new V3d(5.0, -2.0, 1.75));
            var affine3f = new Affine3f(new M33f(1.2f, 0.35f, -0.1f, -0.2f, 0.9f, 0.15f, 0.05f, -0.25f, 1.1f), new V3f(5.0f, -2.0f, 1.75f));
            var shift3d = new Shift3d(3.5, -1.25, 2.0);
            var shift3f = new Shift3f(3.5f, -1.25f, 2.0f);
            var scale3d = new Scale3d(-1.5, 0.8, -1.25);
            var scale3f = new Scale3f(-1.5f, 0.8f, -1.25f);
            var trafo3d = Trafo3d.FromComponents(new V3d(1.15, 0.9, 1.35), new V3d(0.2, -0.35, 0.5), new V3d(-1.5, 2.0, 0.75));
            var trafo3f = Trafo3f.FromComponents(new V3f(1.15f, 0.9f, 1.35f), new V3f(0.2f, -0.35f, 0.5f), new V3f(-1.5f, 2.0f, 0.75f));

            return new Dictionary<Type, object>
            {
                [typeof(Trafo2d)] = trafo2d,
                [typeof(Trafo2f)] = trafo2f,
                [typeof(Euclidean2d)] = euclidean2d,
                [typeof(Euclidean2f)] = euclidean2f,
                [typeof(Similarity2d)] = similarity2d,
                [typeof(Similarity2f)] = similarity2f,
                [typeof(Affine2d)] = affine2d,
                [typeof(Affine2f)] = affine2f,
                [typeof(Shift2d)] = shift2d,
                [typeof(Shift2f)] = shift2f,
                [typeof(Rot2d)] = rot2d,
                [typeof(Rot2f)] = rot2f,
                [typeof(Scale2d)] = scale2d,
                [typeof(Scale2f)] = scale2f,
                [typeof(Trafo3d)] = trafo3d,
                [typeof(Trafo3f)] = trafo3f,
                [typeof(Euclidean3d)] = euclidean3d,
                [typeof(Euclidean3f)] = euclidean3f,
                [typeof(Similarity3d)] = similarity3d,
                [typeof(Similarity3f)] = similarity3f,
                [typeof(Affine3d)] = affine3d,
                [typeof(Affine3f)] = affine3f,
                [typeof(Shift3d)] = shift3d,
                [typeof(Shift3f)] = shift3f,
                [typeof(Rot3d)] = rot3d,
                [typeof(Rot3f)] = rot3f,
                [typeof(Scale3d)] = scale3d,
                [typeof(Scale3f)] = scale3f,
            };
        }

        private static object CreateValidBox2(Type boxType)
            => boxType.Name switch
            {
                nameof(Box2i) => new Box2i(new V2i(-2, -1), new V2i(3, 4)),
                nameof(Box2l) => new Box2l(new V2l(-2, -1), new V2l(3, 4)),
                nameof(Box2f) => new Box2f(new V2f(-2f, -1f), new V2f(3f, 4f)),
                nameof(Box2d) => new Box2d(new V2d(-2.0, -1.0), new V2d(3.0, 4.0)),
                _ => throw new NotSupportedException(boxType.FullName)
            };

        private static object CreateCrossedBox2(Type boxType, bool crossX)
            => boxType.Name switch
            {
                nameof(Box2i) when crossX => new Box2i(new V2i(2, -1), new V2i(1, 3)),
                nameof(Box2i) => new Box2i(new V2i(-1, 3), new V2i(2, 1)),
                nameof(Box2l) when crossX => new Box2l(new V2l(2, -1), new V2l(1, 3)),
                nameof(Box2l) => new Box2l(new V2l(-1, 3), new V2l(2, 1)),
                nameof(Box2f) when crossX => new Box2f(new V2f(2f, -1f), new V2f(1f, 3f)),
                nameof(Box2f) => new Box2f(new V2f(-1f, 3f), new V2f(2f, 1f)),
                nameof(Box2d) when crossX => new Box2d(new V2d(2.0, -1.0), new V2d(1.0, 3.0)),
                nameof(Box2d) => new Box2d(new V2d(-1.0, 3.0), new V2d(2.0, 1.0)),
                _ => throw new NotSupportedException(boxType.FullName)
            };

        private static object CreateCrossedBox3(Type boxType, int axis)
            => boxType.Name switch
            {
                nameof(Box3i) => axis switch
                {
                    0 => new Box3i(new V3i(2, -1, -2), new V3i(1, 3, 4)),
                    1 => new Box3i(new V3i(-1, 3, -2), new V3i(2, 1, 4)),
                    2 => new Box3i(new V3i(-1, -2, 4), new V3i(2, 3, 1)),
                    _ => throw new ArgumentOutOfRangeException(nameof(axis))
                },
                nameof(Box3l) => axis switch
                {
                    0 => new Box3l(new V3l(2, -1, -2), new V3l(1, 3, 4)),
                    1 => new Box3l(new V3l(-1, 3, -2), new V3l(2, 1, 4)),
                    2 => new Box3l(new V3l(-1, -2, 4), new V3l(2, 3, 1)),
                    _ => throw new ArgumentOutOfRangeException(nameof(axis))
                },
                nameof(Box3f) => axis switch
                {
                    0 => new Box3f(new V3f(2f, -1f, -2f), new V3f(1f, 3f, 4f)),
                    1 => new Box3f(new V3f(-1f, 3f, -2f), new V3f(2f, 1f, 4f)),
                    2 => new Box3f(new V3f(-1f, -2f, 4f), new V3f(2f, 3f, 1f)),
                    _ => throw new ArgumentOutOfRangeException(nameof(axis))
                },
                nameof(Box3d) => axis switch
                {
                    0 => new Box3d(new V3d(2.0, -1.0, -2.0), new V3d(1.0, 3.0, 4.0)),
                    1 => new Box3d(new V3d(-1.0, 3.0, -2.0), new V3d(2.0, 1.0, 4.0)),
                    2 => new Box3d(new V3d(-1.0, -2.0, 4.0), new V3d(2.0, 3.0, 1.0)),
                    _ => throw new ArgumentOutOfRangeException(nameof(axis))
                },
                _ => throw new NotSupportedException(boxType.FullName)
            };

        private static object CreateValidBox3(Type boxType)
            => boxType.Name switch
            {
                nameof(Box3i) => new Box3i(new V3i(-2, -1, -3), new V3i(3, 4, 5)),
                nameof(Box3l) => new Box3l(new V3l(-2, -1, -3), new V3l(3, 4, 5)),
                nameof(Box3f) => new Box3f(new V3f(-2f, -1f, -3f), new V3f(3f, 4f, 5f)),
                nameof(Box3d) => new Box3d(new V3d(-2.0, -1.0, -3.0), new V3d(3.0, 4.0, 5.0)),
                _ => throw new NotSupportedException(boxType.FullName)
            };

        private static object GetAffine2CoverageTransform(Type boxType)
            => boxType.Name.EndsWith("f", StringComparison.Ordinal)
                ? new Affine2f(new M22f(1.2f, 0.35f, -0.2f, 0.9f), new V2f(5.0f, -2.0f))
                : new Affine2d(new M22d(1.2, 0.35, -0.2, 0.9), new V2d(5.0, -2.0));

        private static object[] GetAdditionalBox2ForwardCoverageTransforms(Type boxType)
        {
            if (boxType.Name.EndsWith("f", StringComparison.Ordinal))
            {
                return
                [
                    new Euclidean2f(Rot2f.FromDegrees(137.0f), new V2f(2.5f, -1.75f)),
                    new Euclidean2f(Rot2f.FromDegrees(-17.0f), new V2f(2.5f, -1.75f)),
                    new Similarity2f(1.25f, Rot2f.FromDegrees(137.0f), new V2f(2.5f, -1.75f)),
                    new Similarity2f(1.25f, Rot2f.FromDegrees(17.0f), new V2f(2.5f, -1.75f)),
                    new Similarity2f(1.25f, Rot2f.FromDegrees(-17.0f), new V2f(2.5f, -1.75f)),
                    new Similarity2f(-1.25f, Rot2f.FromDegrees(-17.0f), new V2f(2.5f, -1.75f)),
                    Rot2f.FromDegrees(137.0f),
                    Rot2f.FromDegrees(-17.0f),
                    new Scale2f(1.5f, -0.8f),
                ];
            }

            return
            [
                new Euclidean2d(Rot2d.FromDegrees(137.0), new V2d(2.5, -1.75)),
                new Euclidean2d(Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75)),
                new Similarity2d(1.25, Rot2d.FromDegrees(137.0), new V2d(2.5, -1.75)),
                new Similarity2d(1.25, Rot2d.FromDegrees(17.0), new V2d(2.5, -1.75)),
                new Similarity2d(1.25, Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75)),
                new Similarity2d(-1.25, Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75)),
                Rot2d.FromDegrees(137.0),
                Rot2d.FromDegrees(-17.0),
                new Scale2d(1.5, -0.8),
            ];
        }

        private static object[] GetAdditionalBox2InverseCoverageTransforms(Type boxType)
        {
            if (boxType.Name.EndsWith("f", StringComparison.Ordinal))
            {
                return
                [
                    new Euclidean2f(Rot2f.FromDegrees(-17.0f), new V2f(2.5f, -1.75f)),
                    new Similarity2f(1.25f, Rot2f.FromDegrees(-17.0f), new V2f(2.5f, -1.75f)),
                    Rot2f.FromDegrees(-17.0f),
                    new Scale2f(1.5f, -0.8f),
                ];
            }

            return
            [
                new Euclidean2d(Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75)),
                new Similarity2d(1.25, Rot2d.FromDegrees(-17.0), new V2d(2.5, -1.75)),
                Rot2d.FromDegrees(-17.0),
                new Scale2d(1.5, -0.8),
            ];
        }

        private static object[] GetAdditionalBox3ForwardCoverageTransforms(Type boxType)
        {
            if (boxType.Name.EndsWith("f", StringComparison.Ordinal))
            {
                return
                [
                    new Euclidean3f(Rot3f.Rotation(V3f.ZAxis, 137.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Euclidean3f(Rot3f.Rotation(V3f.ZAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Euclidean3f(Rot3f.Rotation(V3f.YAxis, 17.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Euclidean3f(Rot3f.Rotation(V3f.YAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Euclidean3f(Rot3f.Rotation(V3f.XAxis, 137.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Euclidean3f(Rot3f.Rotation(V3f.XAxis, 17.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Euclidean3f(Rot3f.Rotation(V3f.XAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.ZAxis, 137.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.ZAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.YAxis, 17.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.YAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.XAxis, 137.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.XAxis, 17.0f.RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.XAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(-1.25f, Rot3f.Rotation(V3f.ZAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    Rot3f.Rotation(V3f.ZAxis, 137.0f.RadiansFromDegrees()),
                    Rot3f.Rotation(V3f.ZAxis, (-17.0f).RadiansFromDegrees()),
                    Rot3f.Rotation(V3f.YAxis, 17.0f.RadiansFromDegrees()),
                    Rot3f.Rotation(V3f.YAxis, (-17.0f).RadiansFromDegrees()),
                    Rot3f.Rotation(V3f.XAxis, 137.0f.RadiansFromDegrees()),
                    Rot3f.Rotation(V3f.XAxis, 17.0f.RadiansFromDegrees()),
                    Rot3f.Rotation(V3f.XAxis, (-17.0f).RadiansFromDegrees()),
                    new Scale3f(1.5f, -0.8f, 1.25f),
                    new Scale3f(1.5f, 0.8f, -1.25f),
                ];
            }

            return
            [
                new Euclidean3d(Rot3d.Rotation(V3d.ZAxis, 137.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Euclidean3d(Rot3d.Rotation(V3d.ZAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Euclidean3d(Rot3d.Rotation(V3d.YAxis, 17.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Euclidean3d(Rot3d.Rotation(V3d.YAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Euclidean3d(Rot3d.Rotation(V3d.XAxis, 137.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Euclidean3d(Rot3d.Rotation(V3d.XAxis, 17.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Euclidean3d(Rot3d.Rotation(V3d.XAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.ZAxis, 137.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.ZAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.YAxis, 17.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.YAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.XAxis, 137.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.XAxis, 17.0.RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.XAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(-1.25, Rot3d.Rotation(V3d.ZAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                Rot3d.Rotation(V3d.ZAxis, 137.0.RadiansFromDegrees()),
                Rot3d.Rotation(V3d.ZAxis, (-17.0).RadiansFromDegrees()),
                Rot3d.Rotation(V3d.YAxis, 17.0.RadiansFromDegrees()),
                Rot3d.Rotation(V3d.YAxis, (-17.0).RadiansFromDegrees()),
                Rot3d.Rotation(V3d.XAxis, 137.0.RadiansFromDegrees()),
                Rot3d.Rotation(V3d.XAxis, 17.0.RadiansFromDegrees()),
                Rot3d.Rotation(V3d.XAxis, (-17.0).RadiansFromDegrees()),
                new Scale3d(1.5, -0.8, 1.25),
                new Scale3d(1.5, 0.8, -1.25),
            ];
        }

        private static object[] GetAdditionalBox3InverseCoverageTransforms(Type boxType)
        {
            if (boxType.Name.EndsWith("f", StringComparison.Ordinal))
            {
                return
                [
                    new Euclidean3f(Rot3f.Rotation(V3f.ZAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    new Similarity3f(1.25f, Rot3f.Rotation(V3f.ZAxis, (-17.0f).RadiansFromDegrees()), new V3f(2.5f, -1.75f, 4.0f)),
                    Rot3f.Rotation(V3f.ZAxis, (-17.0f).RadiansFromDegrees()),
                    new Scale3f(1.5f, -0.8f, 1.25f),
                    new Scale3f(1.5f, 0.8f, -1.25f),
                ];
            }

            return
            [
                new Euclidean3d(Rot3d.Rotation(V3d.ZAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                new Similarity3d(1.25, Rot3d.Rotation(V3d.ZAxis, (-17.0).RadiansFromDegrees()), new V3d(2.5, -1.75, 4.0)),
                Rot3d.Rotation(V3d.ZAxis, (-17.0).RadiansFromDegrees()),
                new Scale3d(1.5, -0.8, 1.25),
                new Scale3d(1.5, 0.8, -1.25),
            ];
        }

        private static object GetStaticPropertyValue(Type type, string propertyName)
            => type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;

        private static IEnumerable<Type> GetBoxForwardTransformTypes(Type boxType)
        {
            var isFloat = boxType.Name.EndsWith("f", StringComparison.Ordinal);
            var is2D = boxType.Name.StartsWith("Box2", StringComparison.Ordinal);
            return is2D
                ? isFloat
                    ? new[] { typeof(Euclidean2f), typeof(Similarity2f), typeof(Affine2f), typeof(Shift2f), typeof(Rot2f), typeof(Scale2f) }
                    : new[] { typeof(Euclidean2d), typeof(Similarity2d), typeof(Affine2d), typeof(Shift2d), typeof(Rot2d), typeof(Scale2d) }
                : isFloat
                    ? new[] { typeof(Euclidean3f), typeof(Similarity3f), typeof(Affine3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                    : new[] { typeof(Euclidean3d), typeof(Similarity3d), typeof(Affine3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetBoxInverseTransformTypes(Type boxType)
        {
            var isFloat = boxType.Name.EndsWith("f", StringComparison.Ordinal);
            var is2D = boxType.Name.StartsWith("Box2", StringComparison.Ordinal);
            return is2D
                ? isFloat
                    ? new[] { typeof(Trafo2f), typeof(Euclidean2f), typeof(Similarity2f), typeof(Shift2f), typeof(Rot2f), typeof(Scale2f) }
                    : new[] { typeof(Trafo2d), typeof(Euclidean2d), typeof(Similarity2d), typeof(Shift2d), typeof(Rot2d), typeof(Scale2d) }
                : isFloat
                    ? new[] { typeof(Trafo3f), typeof(Euclidean3f), typeof(Similarity3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                    : new[] { typeof(Trafo3d), typeof(Euclidean3d), typeof(Similarity3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetHullForwardTransformTypes(Type hullType)
        {
            var isFloat = hullType.Name.EndsWith("f", StringComparison.Ordinal);
            var is2D = hullType.Name.StartsWith("Hull2", StringComparison.Ordinal);
            return is2D
                ? isFloat
                    ? new[] { typeof(Euclidean2f), typeof(Similarity2f), typeof(Affine2f), typeof(Shift2f), typeof(Rot2f), typeof(Scale2f) }
                    : new[] { typeof(Euclidean2d), typeof(Similarity2d), typeof(Affine2d), typeof(Shift2d), typeof(Rot2d), typeof(Scale2d) }
                : isFloat
                    ? new[] { typeof(Euclidean3f), typeof(Similarity3f), typeof(Affine3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                    : new[] { typeof(Euclidean3d), typeof(Similarity3d), typeof(Affine3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetHullInverseTransformTypes(Type hullType)
        {
            var isFloat = hullType.Name.EndsWith("f", StringComparison.Ordinal);
            var is2D = hullType.Name.StartsWith("Hull2", StringComparison.Ordinal);
            return is2D
                ? isFloat
                    ? new[] { typeof(Trafo2f), typeof(Euclidean2f), typeof(Similarity2f), typeof(Shift2f), typeof(Rot2f), typeof(Scale2f) }
                    : new[] { typeof(Trafo2d), typeof(Euclidean2d), typeof(Similarity2d), typeof(Shift2d), typeof(Rot2d), typeof(Scale2d) }
                : isFloat
                    ? new[] { typeof(Trafo3f), typeof(Euclidean3f), typeof(Similarity3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                    : new[] { typeof(Trafo3d), typeof(Euclidean3d), typeof(Similarity3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetFastHullForwardTransformTypes(Type hullType)
        {
            var isFloat = hullType.Name.EndsWith("f", StringComparison.Ordinal);
            return isFloat
                ? new[] { typeof(Trafo3f), typeof(Euclidean3f), typeof(Similarity3f), typeof(Affine3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                : new[] { typeof(Trafo3d), typeof(Euclidean3d), typeof(Similarity3d), typeof(Affine3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetFastHullInverseTransformTypes(Type hullType)
        {
            var isFloat = hullType.Name.EndsWith("f", StringComparison.Ordinal);
            return isFloat
                ? new[] { typeof(Trafo3f), typeof(Euclidean3f), typeof(Similarity3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                : new[] { typeof(Trafo3d), typeof(Euclidean3d), typeof(Similarity3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetPlaneForwardTransformTypes(Type planeType)
        {
            var isFloat = planeType.Name.EndsWith("f", StringComparison.Ordinal);
            return isFloat
                ? new[] { typeof(Similarity3f), typeof(Affine3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                : new[] { typeof(Similarity3d), typeof(Affine3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetPlaneInverseTransformTypes(Type planeType)
        {
            var isFloat = planeType.Name.EndsWith("f", StringComparison.Ordinal);
            return isFloat
                ? new[] { typeof(Trafo3f), typeof(Euclidean3f), typeof(Similarity3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                : new[] { typeof(Trafo3d), typeof(Euclidean3d), typeof(Similarity3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static IEnumerable<Type> GetRayForwardTransformTypes(Type rayType)
        {
            yield return rayType.Name.EndsWith("f", StringComparison.Ordinal) ? typeof(Trafo3f) : typeof(Trafo3d);
        }

        private static IEnumerable<Type> GetRayInverseTransformTypes(Type rayType)
        {
            var isFloat = rayType.Name.EndsWith("f", StringComparison.Ordinal);
            return isFloat
                ? new[] { typeof(Trafo3f), typeof(Euclidean3f), typeof(Similarity3f), typeof(Shift3f), typeof(Rot3f), typeof(Scale3f) }
                : new[] { typeof(Trafo3d), typeof(Euclidean3d), typeof(Similarity3d), typeof(Shift3d), typeof(Rot3d), typeof(Scale3d) };
        }

        private static object InvokeInstance(object target, string methodName, object arg)
        {
            var method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, new[] { arg.GetType() }, null);
            Assert.That(method, Is.Not.Null, $"{target.GetType().Name}.{methodName}({arg.GetType().Name}) missing");
            return method!.Invoke(target, new[] { arg })!;
        }

        private static object ToForwardMatrix(object transform)
            => transform switch
            {
                Euclidean2f t => (M33f)t,
                Similarity2f t => (M33f)t,
                Affine2f t => (M33f)t,
                Shift2f t => (M33f)t,
                Rot2f t => (M33f)t,
                Scale2f t => (M33f)t,
                Euclidean2d t => (M33d)t,
                Similarity2d t => (M33d)t,
                Affine2d t => (M33d)t,
                Shift2d t => (M33d)t,
                Rot2d t => (M33d)t,
                Scale2d t => (M33d)t,
                Euclidean3f t => (M44f)t,
                Similarity3f t => (M44f)t,
                Affine3f t => (M44f)t,
                Shift3f t => (M44f)t,
                Rot3f t => (M44f)t,
                Scale3f t => (M44f)t,
                Euclidean3d t => (M44d)t,
                Similarity3d t => (M44d)t,
                Affine3d t => (M44d)t,
                Shift3d t => (M44d)t,
                Rot3d t => (M44d)t,
                Scale3d t => (M44d)t,
                _ => throw new NotSupportedException(transform.GetType().FullName)
            };

        private static object ToInverseMatrix(object transform)
            => transform switch
            {
                Euclidean2f t => (M33f)t.Inverse,
                Similarity2f t => (M33f)t.Inverse,
                Shift2f t => (M33f)t.Inverse,
                Rot2f t => (M33f)t.Inverse,
                Scale2f t => (M33f)t.Inverse,
                Euclidean2d t => (M33d)t.Inverse,
                Similarity2d t => (M33d)t.Inverse,
                Shift2d t => (M33d)t.Inverse,
                Rot2d t => (M33d)t.Inverse,
                Scale2d t => (M33d)t.Inverse,
                Euclidean3f t => (M44f)t.Inverse,
                Similarity3f t => (M44f)t.Inverse,
                Shift3f t => (M44f)t.Inverse,
                Rot3f t => (M44f)t.Inverse,
                Scale3f t => (M44f)t.Inverse,
                Euclidean3d t => (M44d)t.Inverse,
                Similarity3d t => (M44d)t.Inverse,
                Shift3d t => (M44d)t.Inverse,
                Rot3d t => (M44d)t.Inverse,
                Scale3d t => (M44d)t.Inverse,
                _ => throw new NotSupportedException(transform.GetType().FullName)
            };

        private static object GetBackwardMatrix(object transform)
            => transform switch
            {
                Trafo2f t => t.Backward,
                Trafo2d t => t.Backward,
                Trafo3f t => t.Backward,
                Trafo3d t => t.Backward,
                _ => throw new NotSupportedException(transform.GetType().FullName)
            };

        private static object ToTrafo(object transform)
            => transform switch
            {
                Trafo2f t => t,
                Trafo2d t => t,
                Trafo3f t => t,
                Trafo3d t => t,
                Euclidean2f t => new Trafo2f(t),
                Similarity2f t => new Trafo2f(t),
                Affine2f t => new Trafo2f(t),
                Shift2f t => new Trafo2f(t),
                Rot2f t => new Trafo2f(t),
                Scale2f t => new Trafo2f(t),
                Euclidean2d t => new Trafo2d(t),
                Similarity2d t => new Trafo2d(t),
                Affine2d t => new Trafo2d(t),
                Shift2d t => new Trafo2d(t),
                Rot2d t => new Trafo2d(t),
                Scale2d t => new Trafo2d(t),
                Euclidean3f t => new Trafo3f(t),
                Similarity3f t => new Trafo3f(t),
                Affine3f t => new Trafo3f(t),
                Shift3f t => new Trafo3f(t),
                Rot3f t => new Trafo3f(t),
                Scale3f t => new Trafo3f(t),
                Euclidean3d t => new Trafo3d(t),
                Similarity3d t => new Trafo3d(t),
                Affine3d t => new Trafo3d(t),
                Shift3d t => new Trafo3d(t),
                Rot3d t => new Trafo3d(t),
                Scale3d t => new Trafo3d(t),
                _ => throw new NotSupportedException(transform.GetType().FullName)
            };

        private static object ToInverseTrafo(object transform)
            => transform switch
            {
                Trafo2f t => t.Inverse,
                Trafo2d t => t.Inverse,
                Trafo3f t => t.Inverse,
                Trafo3d t => t.Inverse,
                Euclidean2f t => new Trafo2f(t).Inverse,
                Similarity2f t => new Trafo2f(t).Inverse,
                Shift2f t => new Trafo2f(t).Inverse,
                Rot2f t => new Trafo2f(t).Inverse,
                Scale2f t => new Trafo2f(t).Inverse,
                Euclidean2d t => new Trafo2d(t).Inverse,
                Similarity2d t => new Trafo2d(t).Inverse,
                Shift2d t => new Trafo2d(t).Inverse,
                Rot2d t => new Trafo2d(t).Inverse,
                Scale2d t => new Trafo2d(t).Inverse,
                Euclidean3f t => new Trafo3f(t).Inverse,
                Similarity3f t => new Trafo3f(t).Inverse,
                Shift3f t => new Trafo3f(t).Inverse,
                Rot3f t => new Trafo3f(t).Inverse,
                Scale3f t => new Trafo3f(t).Inverse,
                Euclidean3d t => new Trafo3d(t).Inverse,
                Similarity3d t => new Trafo3d(t).Inverse,
                Shift3d t => new Trafo3d(t).Inverse,
                Rot3d t => new Trafo3d(t).Inverse,
                Scale3d t => new Trafo3d(t).Inverse,
                _ => throw new NotSupportedException(transform.GetType().FullName)
            };

        private static void AssertBoxesEquivalent(object actual, object expected, string name)
        {
            switch (actual)
            {
                case Box2f a when expected is Box2f e:
                    Assert.True(a.Min.ApproximateEquals(e.Min, 1e-5f) && a.Max.ApproximateEquals(e.Max, 1e-5f), name);
                    return;
                case Box2d a when expected is Box2d e:
                    Assert.True(a.Min.ApproximateEquals(e.Min, 1e-10) && a.Max.ApproximateEquals(e.Max, 1e-10), name);
                    return;
                case Box3f a when expected is Box3f e:
                    Assert.True(a.Min.ApproximateEquals(e.Min, 1e-5f) && a.Max.ApproximateEquals(e.Max, 1e-5f), name);
                    return;
                case Box3d a when expected is Box3d e:
                    Assert.True(a.Min.ApproximateEquals(e.Min, 1e-10) && a.Max.ApproximateEquals(e.Max, 1e-10), name);
                    return;
            }

            throw new NotSupportedException($"{actual.GetType().FullName} vs {expected.GetType().FullName}");
        }

        private static void AssertHullsEquivalent(object actual, object expected, string name)
        {
            switch (actual)
            {
                case Hull2f a when expected is Hull2f e:
                    AssertHullPlanesEqual(a.PlaneArray, e.PlaneArray, 1e-5f, name);
                    return;
                case Hull2d a when expected is Hull2d e:
                    AssertHullPlanesEqual(a.PlaneArray, e.PlaneArray, 1e-10, name);
                    return;
                case Hull3f a when expected is Hull3f e:
                    AssertHullPlanesEqual(a.PlaneArray, e.PlaneArray, 1e-5f, name);
                    return;
                case Hull3d a when expected is Hull3d e:
                    AssertHullPlanesEqual(a.PlaneArray, e.PlaneArray, 1e-10, name);
                    return;
                case FastHull3f a when expected is FastHull3f e:
                    AssertHullsEquivalent(a.Hull, e.Hull, name);
                    Assert.That(a.MinCornerIndexArray, Is.EqualTo(e.MinCornerIndexArray), name);
                    return;
                case FastHull3d a when expected is FastHull3d e:
                    AssertHullsEquivalent(a.Hull, e.Hull, name);
                    Assert.That(a.MinCornerIndexArray, Is.EqualTo(e.MinCornerIndexArray), name);
                    return;
            }

            throw new NotSupportedException($"{actual.GetType().FullName} vs {expected.GetType().FullName}");
        }

        private static void AssertHullPlanesEqual(Plane2f[] actual, Plane2f[] expected, float eps, string name)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length), name);
            for (var i = 0; i < actual.Length; i++)
                Assert.True(actual[i].Coefficients.ApproximateEquals(expected[i].Coefficients, eps), $"{name} plane {i}");
        }

        private static void AssertHullPlanesEqual(Plane2d[] actual, Plane2d[] expected, double eps, string name)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length), name);
            for (var i = 0; i < actual.Length; i++)
                Assert.True(actual[i].Coefficients.ApproximateEquals(expected[i].Coefficients, eps), $"{name} plane {i}");
        }

        private static void AssertHullPlanesEqual(Plane3f[] actual, Plane3f[] expected, float eps, string name)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length), name);
            for (var i = 0; i < actual.Length; i++)
                Assert.True(actual[i].Coefficients.ApproximateEquals(expected[i].Coefficients, eps), $"{name} plane {i}");
        }

        private static void AssertHullPlanesEqual(Plane3d[] actual, Plane3d[] expected, double eps, string name)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length), name);
            for (var i = 0; i < actual.Length; i++)
                Assert.True(actual[i].Coefficients.ApproximateEquals(expected[i].Coefficients, eps), $"{name} plane {i}");
        }

        private static void AssertPlanesEquivalent(object actual, object expected, string name)
        {
            switch (actual)
            {
                case Plane3f a when expected is Plane3f e:
                    Assert.True(a.Coefficients.ApproximateEquals(e.Coefficients, 1e-5f), name);
                    return;
                case Plane3d a when expected is Plane3d e:
                    Assert.True(a.Coefficients.ApproximateEquals(e.Coefficients, 1e-10), name);
                    return;
            }

            throw new NotSupportedException($"{actual.GetType().FullName} vs {expected.GetType().FullName}");
        }

        private static void AssertRaysEquivalent(object actual, object expected, string name)
        {
            switch (actual)
            {
                case Ray3f a when expected is Ray3f e:
                    Assert.True(a.Origin.ApproximateEquals(e.Origin, 1e-5f) && a.Direction.ApproximateEquals(e.Direction, 1e-5f), name);
                    return;
                case Ray3d a when expected is Ray3d e:
                    Assert.True(a.Origin.ApproximateEquals(e.Origin, 1e-10) && a.Direction.ApproximateEquals(e.Direction, 1e-10), name);
                    return;
            }

            throw new NotSupportedException($"{actual.GetType().FullName} vs {expected.GetType().FullName}");
        }
    }
}

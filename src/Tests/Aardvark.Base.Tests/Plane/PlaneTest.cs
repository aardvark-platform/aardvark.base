using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PlaneTest : TestSuite
    {
        public PlaneTest() { }
        public PlaneTest(Options options) : base(options) { }
        
        [Test]
        public void TransformedTrafo()
        {
            var rand = new RandomSystem();
            for(int i = 0; i < 100; i++) 
            {
                var p = new Plane3d(rand.UniformV3dDirection() * rand.UniformDouble(), rand.UniformDouble());
                var trafo = 
                    Trafo3d.Rotation(rand.UniformV3dDirection(), rand.UniformDouble() * Constant.PiTimesTwo) *
                    Trafo3d.Translation(rand.UniformV3d()) *
                    Trafo3d.Scale(rand.UniformV3d()) *
                    Trafo3d.Rotation(rand.UniformV3dDirection(), rand.UniformDouble() * Constant.PiTimesTwo) *
                    Trafo3d.Translation(rand.UniformV3d());

                var p0 = p.Point;
                var p1 = p0 + 3.0 * p.Normal;

                var h00 = p.Height(p0);
                var h01 = p.Transformed(trafo).Height(trafo.TransformPos(p0));
                if(!Fun.ApproximateEquals(h00, h01, 1E-8)) throw new Exception("heights differ");
                if(!Fun.ApproximateEquals(h00, 0.0, 1E-8)) throw new Exception("heights not zero");
                

                var h10 = p.Height(p1);
                var h11 = p.Transformed(trafo).Height(trafo.TransformPos(p1));
                if(!Fun.ApproximateEquals(h10, h11, 1E-8)) throw new Exception("heights differ");
            }
        }

        [Test]
        public void TransformedMatrix()
        {
            var rand = new RandomSystem();
            for(int i = 0; i < 100; i++) 
            {
                var p = new Plane3d(rand.UniformV3dDirection() * rand.UniformDouble() , rand.UniformDouble());
                var trafo = 
                    M44d.Translation(rand.UniformV3d()) *
                    M44d.Rotation(rand.UniformV3dDirection(), rand.UniformDouble() * Constant.PiTimesTwo) *
                    M44d.Translation(rand.UniformV3d());

                var p0 = p.Point;
                var p1 = p0 + 3.0 * p.Normal;

                var h00 = p.Height(p0);
                var h01 = p.Transformed(trafo).Height(trafo.TransformPos(p0));
                if(!Fun.ApproximateEquals(h00, h01, 1E-8)) throw new Exception("heights differ");
                if(!Fun.ApproximateEquals(h00, 0.0, 1E-8)) throw new Exception("heights not zero");
                

                var h10 = p.Height(p1);
                var h11 = p.Transformed(trafo).Height(trafo.TransformPos(p1));
                if(!Fun.ApproximateEquals(h10, h11, 1E-8)) throw new Exception("heights differ");
            }
        }


        [Test]
        public void TransformedEuclidean()
        {
            var rand = new RandomSystem();
            for(int i = 0; i < 100; i++) 
            {
                var p = new Plane3d(rand.UniformV3dDirection() * rand.UniformDouble(), rand.UniformDouble());
                var trafo = 
                    Euclidean3d.Translation(rand.UniformV3d()) *
                    Euclidean3d.Rotation(rand.UniformV3dDirection(), rand.UniformDouble() * Constant.PiTimesTwo);

                var p0 = p.Point;
                var p1 = p0 + 3.0 * p.Normal;

                var h00 = p.Height(p0);
                var h01 = p.Transformed(trafo).Height(trafo.TransformPos(p0));
                if(!Fun.ApproximateEquals(h00, h01, 1E-8)) throw new Exception("heights differ");
                if(!Fun.ApproximateEquals(h00, 0.0, 1E-8)) throw new Exception("heights not zero");
                

                var h10 = p.Height(p1);
                var h11 = p.Transformed(trafo).Height(trafo.TransformPos(p1));
                if(!Fun.ApproximateEquals(h10, h11, 1E-8)) throw new Exception("heights differ");
            }
        }

        [Test]
        public void ProjectUnprojectTest()
        {
            var normal = new V3d(-3.38012399699281E-17, -0.000407306344606881, -0.999999917050768);
            var distance = -13.109839358218661;
            var plane = new Plane3d(normal, distance);

            var testPoint = new V3d(-97.076092093139, -148.399088699813, 13.170284341045);

            // NOTE: Rot3d in Euclidean3d has numerical issue
            var projectedPoint = plane.ProjectToPlaneSpace(testPoint);
            var unprojectedPoint = plane.Unproject(projectedPoint);

            var planeTrafo = Trafo3d.FromNormalFrame(plane.Point, plane.Normal);
            var projectedPoint2 = planeTrafo.Backward.TransformPos(testPoint).XY;
            var unprojectedPoint2 = planeTrafo.Forward.TransformPos(projectedPoint2.XYO);

            if (!((unprojectedPoint - unprojectedPoint2).Length < 0.01)) throw new Exception();
        }
    }
}

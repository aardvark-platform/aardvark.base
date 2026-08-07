using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class FastRayTests
    {
        private static void AssertHit(FastRay2d ray, Box2d box, double expectedT)
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;

            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(expectedT));
            Assert.That(tmax, Is.EqualTo(expectedT));
        }

        private static void AssertHit(FastRay2f ray, Box2f box, float expectedT)
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;

            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(expectedT));
            Assert.That(tmax, Is.EqualTo(expectedT));
        }

        private static void AssertHit(FastRay3d ray, Box3d box, double expectedT)
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;

            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(expectedT));
            Assert.That(tmax, Is.EqualTo(expectedT));
        }

        private static void AssertHit(FastRay3f ray, Box3f box, float expectedT)
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;

            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(expectedT));
            Assert.That(tmax, Is.EqualTo(expectedT));
        }

        [Test]
        public void Issue55ZeroThicknessBoxIntersects()
        {
            var ray = new FastRay3d(10.0 * V3d.OOI, -V3d.OOI);
            var box = new Box3d(-V3d.IIO, V3d.IIO);

            AssertHit(ray, box, 10.0);
        }

        [Test]
        public void CollapsedAndPointBoxesIntersectIn2d()
        {
            var lineD = new Box2d(new V2d(0.0, -1.0), new V2d(0.0, 1.0));
            AssertHit(new FastRay2d(new V2d(-2.0, 0.0), V2d.XAxis), lineD, 2.0);
            AssertHit(new FastRay2d(new V2d(2.0, 0.0), -V2d.XAxis), lineD, 2.0);

            var pointD = new Box2d(V2d.Zero, V2d.Zero);
            AssertHit(new FastRay2d(new V2d(-1.0, -1.0), V2d.II), pointD, 1.0);
            AssertHit(new FastRay2d(new V2d(1.0, 1.0), -V2d.II), pointD, 1.0);

            var lineF = new Box2f(new V2f(0.0f, -1.0f), new V2f(0.0f, 1.0f));
            AssertHit(new FastRay2f(new V2f(-2.0f, 0.0f), V2f.XAxis), lineF, 2.0f);
            AssertHit(new FastRay2f(new V2f(2.0f, 0.0f), -V2f.XAxis), lineF, 2.0f);

            var pointF = new Box2f(V2f.Zero, V2f.Zero);
            AssertHit(new FastRay2f(new V2f(-1.0f, -1.0f), V2f.II), pointF, 1.0f);
            AssertHit(new FastRay2f(new V2f(1.0f, 1.0f), -V2f.II), pointF, 1.0f);
        }

        [Test]
        public void CollapsedAndPointBoxesIntersectIn3d()
        {
            var planeD = new Box3d(new V3d(-1.0, -1.0, 0.0), new V3d(1.0, 1.0, 0.0));
            AssertHit(new FastRay3d(new V3d(0.0, 0.0, -2.0), V3d.ZAxis), planeD, 2.0);
            AssertHit(new FastRay3d(new V3d(0.0, 0.0, 2.0), -V3d.ZAxis), planeD, 2.0);

            var pointD = new Box3d(V3d.Zero, V3d.Zero);
            AssertHit(new FastRay3d(new V3d(-1.0, -1.0, -1.0), V3d.III), pointD, 1.0);
            AssertHit(new FastRay3d(new V3d(1.0, 1.0, 1.0), -V3d.III), pointD, 1.0);

            var planeF = new Box3f(new V3f(-1.0f, -1.0f, 0.0f), new V3f(1.0f, 1.0f, 0.0f));
            AssertHit(new FastRay3f(new V3f(0.0f, 0.0f, -2.0f), V3f.ZAxis), planeF, 2.0f);
            AssertHit(new FastRay3f(new V3f(0.0f, 0.0f, 2.0f), -V3f.ZAxis), planeF, 2.0f);

            var pointF = new Box3f(V3f.Zero, V3f.Zero);
            AssertHit(new FastRay3f(new V3f(-1.0f, -1.0f, -1.0f), V3f.III), pointF, 1.0f);
            AssertHit(new FastRay3f(new V3f(1.0f, 1.0f, 1.0f), -V3f.III), pointF, 1.0f);
        }

        [Test]
        public void ParallelRaysRespectCollapsedSlabs()
        {
            var lineD = new Box2d(new V2d(0.0, 0.0), new V2d(2.0, 0.0));
            Assert.That(Intersects(new FastRay2d(new V2d(-1.0, 0.0), V2d.XAxis), lineD), Is.True);
            Assert.That(Intersects(new FastRay2d(new V2d(-1.0, 1.0), V2d.XAxis), lineD), Is.False);

            var lineF = new Box2f(new V2f(0.0f, 0.0f), new V2f(2.0f, 0.0f));
            Assert.That(Intersects(new FastRay2f(new V2f(-1.0f, 0.0f), V2f.XAxis), lineF), Is.True);
            Assert.That(Intersects(new FastRay2f(new V2f(-1.0f, 1.0f), V2f.XAxis), lineF), Is.False);

            var planeD = new Box3d(new V3d(0.0, -1.0, 0.0), new V3d(2.0, 1.0, 0.0));
            Assert.That(Intersects(new FastRay3d(new V3d(-1.0, 0.0, 0.0), V3d.XAxis), planeD), Is.True);
            Assert.That(Intersects(new FastRay3d(new V3d(-1.0, 0.0, 1.0), V3d.XAxis), planeD), Is.False);

            var planeF = new Box3f(new V3f(0.0f, -1.0f, 0.0f), new V3f(2.0f, 1.0f, 0.0f));
            Assert.That(Intersects(new FastRay3f(new V3f(-1.0f, 0.0f, 0.0f), V3f.XAxis), planeF), Is.True);
            Assert.That(Intersects(new FastRay3f(new V3f(-1.0f, 0.0f, 1.0f), V3f.XAxis), planeF), Is.False);
        }

        [Test]
        public void EdgeAndCornerGrazingIntersects()
        {
            var box2d = new Box2d(V2d.Zero, V2d.II);
            AssertHit(new FastRay2d(new V2d(-1.0, 0.0), V2d.II), box2d, 1.0);
            AssertHit(new FastRay2d(new V2d(2.0, 1.0), -V2d.II), box2d, 1.0);

            var box2f = new Box2f(V2f.Zero, V2f.II);
            AssertHit(new FastRay2f(new V2f(-1.0f, 0.0f), V2f.II), box2f, 1.0f);
            AssertHit(new FastRay2f(new V2f(2.0f, 1.0f), -V2f.II), box2f, 1.0f);

            var box3d = new Box3d(V3d.Zero, V3d.III);
            AssertHit(new FastRay3d(new V3d(-1.0, 0.0, 0.5), new V3d(1.0, 1.0, 0.0)), box3d, 1.0);
            AssertHit(new FastRay3d(new V3d(-1.0, 0.0, 0.0), V3d.III), box3d, 1.0);

            var box3f = new Box3f(V3f.Zero, V3f.III);
            AssertHit(new FastRay3f(new V3f(-1.0f, 0.0f, 0.5f), new V3f(1.0f, 1.0f, 0.0f)), box3f, 1.0f);
            AssertHit(new FastRay3f(new V3f(-1.0f, 0.0f, 0.0f), V3f.III), box3f, 1.0f);
        }

        [Test]
        public void SuppliedRangeEndpointsAreInclusiveAndDisjointRangesMiss()
        {
            AssertRangeBoundaries(
                new FastRay2d(new V2d(-1.0, 0.5), V2d.XAxis),
                new Box2d(V2d.Zero, V2d.II)
            );
            AssertRangeBoundaries(
                new FastRay2f(new V2f(-1.0f, 0.5f), V2f.XAxis),
                new Box2f(V2f.Zero, V2f.II)
            );
            AssertRangeBoundaries(
                new FastRay3d(new V3d(-1.0, 0.5, 0.5), V3d.XAxis),
                new Box3d(V3d.Zero, V3d.III)
            );
            AssertRangeBoundaries(
                new FastRay3f(new V3f(-1.0f, 0.5f, 0.5f), V3f.XAxis),
                new Box3f(V3f.Zero, V3f.III)
            );
        }

        [Test]
        public void OutputFaceFlagsSupportCollapsedHits()
        {
            var box2d = new Box2d(new V2d(0.0, -1.0), new V2d(0.0, 1.0));
            var tmin2d = 0.0;
            var tmax2d = double.MaxValue;
            Assert.That(
                new FastRay2d(new V2d(-1.0, 0.0), V2d.XAxis).Intersects(
                    box2d, ref tmin2d, ref tmax2d, out var tminFlags2d, out var tmaxFlags2d
                ),
                Is.True
            );
            Assert.That(tminFlags2d, Is.EqualTo(Box.Flags.MinX));
            Assert.That(tmaxFlags2d, Is.EqualTo(Box.Flags.MaxX));

            var box2f = new Box2f(new V2f(0.0f, -1.0f), new V2f(0.0f, 1.0f));
            var tmin2f = 0.0f;
            var tmax2f = float.MaxValue;
            Assert.That(
                new FastRay2f(new V2f(-1.0f, 0.0f), V2f.XAxis).Intersects(
                    box2f, ref tmin2f, ref tmax2f, out var tminFlags2f, out var tmaxFlags2f
                ),
                Is.True
            );
            Assert.That(tminFlags2f, Is.EqualTo(Box.Flags.MinX));
            Assert.That(tmaxFlags2f, Is.EqualTo(Box.Flags.MaxX));

            var box3d = new Box3d(new V3d(0.0, -1.0, -1.0), new V3d(0.0, 1.0, 1.0));
            var tmin3d = 0.0;
            var tmax3d = double.MaxValue;
            Assert.That(
                new FastRay3d(new V3d(-1.0, 0.0, 0.0), V3d.XAxis).Intersects(
                    box3d, ref tmin3d, ref tmax3d, out var tminFlags3d, out var tmaxFlags3d
                ),
                Is.True
            );
            Assert.That(tminFlags3d, Is.EqualTo(Box.Flags.MinX));
            Assert.That(tmaxFlags3d, Is.EqualTo(Box.Flags.MaxX));

            var box3f = new Box3f(new V3f(0.0f, -1.0f, -1.0f), new V3f(0.0f, 1.0f, 1.0f));
            var tmin3f = 0.0f;
            var tmax3f = float.MaxValue;
            Assert.That(
                new FastRay3f(new V3f(-1.0f, 0.0f, 0.0f), V3f.XAxis).Intersects(
                    box3f, ref tmin3f, ref tmax3f, out var tminFlags3f, out var tmaxFlags3f
                ),
                Is.True
            );
            Assert.That(tminFlags3f, Is.EqualTo(Box.Flags.MinX));
            Assert.That(tmaxFlags3f, Is.EqualTo(Box.Flags.MaxX));
        }

        [Test]
        public void MaskedOverloadsSupportCollapsedHitsAndOutputFlags()
        {
            var boxD = new Box3d(new V3d(0.0, -1.0, -1.0), new V3d(0.0, 1.0, 1.0));
            var rayD = new FastRay3d(new V3d(-1.0, 2.0, 2.0), V3d.XAxis);
            var tminD = 0.0;
            var tmaxD = double.MaxValue;
            Assert.That(rayD.Intersects(boxD, Box.Flags.X, ref tminD, ref tmaxD), Is.True);
            Assert.That(tminD, Is.EqualTo(1.0));
            Assert.That(tmaxD, Is.EqualTo(1.0));

            tminD = 0.0;
            tmaxD = double.MaxValue;
            Assert.That(
                rayD.Intersects(
                    boxD, Box.Flags.X, ref tminD, ref tmaxD, out var tminFlagsD, out var tmaxFlagsD
                ),
                Is.True
            );
            Assert.That(tminFlagsD, Is.EqualTo(Box.Flags.MinX));
            Assert.That(tmaxFlagsD, Is.EqualTo(Box.Flags.MaxX));

            var boxF = new Box3f(new V3f(0.0f, -1.0f, -1.0f), new V3f(0.0f, 1.0f, 1.0f));
            var rayF = new FastRay3f(new V3f(-1.0f, 2.0f, 2.0f), V3f.XAxis);
            var tminF = 0.0f;
            var tmaxF = float.MaxValue;
            Assert.That(rayF.Intersects(boxF, Box.Flags.X, ref tminF, ref tmaxF), Is.True);
            Assert.That(tminF, Is.EqualTo(1.0f));
            Assert.That(tmaxF, Is.EqualTo(1.0f));

            tminF = 0.0f;
            tmaxF = float.MaxValue;
            Assert.That(
                rayF.Intersects(
                    boxF, Box.Flags.X, ref tminF, ref tmaxF, out var tminFlagsF, out var tmaxFlagsF
                ),
                Is.True
            );
            Assert.That(tminFlagsF, Is.EqualTo(Box.Flags.MinX));
            Assert.That(tmaxFlagsF, Is.EqualTo(Box.Flags.MaxX));
        }

        private static bool Intersects(FastRay2d ray, Box2d box)
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;
            return ray.Intersects(box, ref tmin, ref tmax);
        }

        private static bool Intersects(FastRay2f ray, Box2f box)
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;
            return ray.Intersects(box, ref tmin, ref tmax);
        }

        private static bool Intersects(FastRay3d ray, Box3d box)
        {
            var tmin = 0.0;
            var tmax = double.MaxValue;
            return ray.Intersects(box, ref tmin, ref tmax);
        }

        private static bool Intersects(FastRay3f ray, Box3f box)
        {
            var tmin = 0.0f;
            var tmax = float.MaxValue;
            return ray.Intersects(box, ref tmin, ref tmax);
        }

        private static void AssertRangeBoundaries(FastRay2d ray, Box2d box)
        {
            var tmin = 2.0;
            var tmax = 10.0;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(2.0));
            Assert.That(tmax, Is.EqualTo(2.0));

            tmin = -10.0;
            tmax = 1.0;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(1.0));
            Assert.That(tmax, Is.EqualTo(1.0));

            tmin = 2.0001;
            tmax = 10.0;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);

            tmin = -10.0;
            tmax = 0.9999;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);
        }

        private static void AssertRangeBoundaries(FastRay2f ray, Box2f box)
        {
            var tmin = 2.0f;
            var tmax = 10.0f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(2.0f));
            Assert.That(tmax, Is.EqualTo(2.0f));

            tmin = -10.0f;
            tmax = 1.0f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(1.0f));
            Assert.That(tmax, Is.EqualTo(1.0f));

            tmin = 2.0001f;
            tmax = 10.0f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);

            tmin = -10.0f;
            tmax = 0.9999f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);
        }

        private static void AssertRangeBoundaries(FastRay3d ray, Box3d box)
        {
            var tmin = 2.0;
            var tmax = 10.0;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(2.0));
            Assert.That(tmax, Is.EqualTo(2.0));

            tmin = -10.0;
            tmax = 1.0;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(1.0));
            Assert.That(tmax, Is.EqualTo(1.0));

            tmin = 2.0001;
            tmax = 10.0;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);

            tmin = -10.0;
            tmax = 0.9999;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);
        }

        private static void AssertRangeBoundaries(FastRay3f ray, Box3f box)
        {
            var tmin = 2.0f;
            var tmax = 10.0f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(2.0f));
            Assert.That(tmax, Is.EqualTo(2.0f));

            tmin = -10.0f;
            tmax = 1.0f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.True);
            Assert.That(tmin, Is.EqualTo(1.0f));
            Assert.That(tmax, Is.EqualTo(1.0f));

            tmin = 2.0001f;
            tmax = 10.0f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);

            tmin = -10.0f;
            tmax = 0.9999f;
            Assert.That(ray.Intersects(box, ref tmin, ref tmax), Is.False);
        }
    }
}

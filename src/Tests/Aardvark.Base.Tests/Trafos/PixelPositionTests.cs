using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PixelPositionTests
    {
        private static readonly V2i[] Origins =
        {
            V2i.Zero, new V2i(100, 200), new V2i(-100, -200),
            new V2i(-100, 200), new V2i(100, -200), new V2i(13, -41)
        };

        private static readonly V2i[] Sizes =
        {
            new V2i(800, 600), new V2i(7, 3), new V2i(3, 7),
            new V2i(1, 1), new V2i(1, 9), new V2i(9, 1)
        };

        private static V2i Expected(Ndc2d ndc, Box2i region)
            => new V2i(region.Min.X + (int)((ndc.Position.X * 0.5 + 0.5) * region.Size.X),
                       region.Min.Y + (int)((-ndc.Position.Y * 0.5 + 0.5) * region.Size.Y));

        private static void AssertPixel(PixelPosition actual, V2i expected, Box2i region, string context)
        {
            Assert.AreEqual(expected, actual.Position, context + ", position");
            Assert.AreEqual(region, actual.Bounds, context + ", bounds");
        }

        [Test]
        public void NdcConstructorsAddRegionOriginAfterTruncation()
        {
            var reproduction = new Box2i(new V2i(100, 200), new V2i(900, 800));
            AssertPixel(new PixelPosition(new Ndc2d(0, 0), reproduction), new V2i(500, 500), reproduction, "translated center");
            var fractional = new Box2i(new V2i(100, 200), new V2i(107, 203));
            AssertPixel(new PixelPosition(new Ndc2d(-1.1, 1.1), fractional), fractional.Min, fractional, "negative fraction truncates before translation");
            AssertPixel(new PixelPosition(new Ndc2d(1, -1), fractional), fractional.Max, fractional, "exclusive maximum remains unclamped");

            var coordinates = new[]
            {
                new Ndc2d(-1, 1), new Ndc2d(1, -1), new Ndc2d(-1, -1), new Ndc2d(1, 1),
                new Ndc2d(0, 0), new Ndc2d(0.25, -0.5), new Ndc2d(-1.1, 1.1),
                new Ndc2d(-2, 2), new Ndc2d(2.25, -2.5)
            };
            foreach (var origin in Origins)
            foreach (var size in Sizes)
            foreach (var ndc in coordinates)
            {
                var region = new Box2i(origin, origin + size);
                var expected = Expected(ndc, region);
                string context = $"origin={origin}, size={size}, ndc={ndc.Position}";
                AssertPixel(new PixelPosition(ndc, region), expected, region, context);
                foreach (double depth in new[] { -3.0, 0.0, 0.25, 1.0, 12.0 })
                    AssertPixel(new PixelPosition(new Ndc3d(ndc.Position.X, ndc.Position.Y, depth), region),
                        expected, region, context + $", depth={depth}");
                if (origin == V2i.Zero)
                {
                    var zeroRegion = Box2i.FromSize(size);
                    var zeroExpected = Expected(ndc, zeroRegion);
                    AssertPixel(new PixelPosition(ndc, size), zeroExpected, zeroRegion, context + ", size-only 2d");
                    AssertPixel(new PixelPosition(new Ndc3d(ndc.Position.X, ndc.Position.Y, 0.7), size),
                        zeroExpected, zeroRegion, context + ", size-only 3d");
                }
            }
        }

        [Test]
        public void PixelCentersRoundTripThroughNdcForTranslatedRegions()
        {
            foreach (var origin in Origins)
            foreach (var size in Sizes)
            foreach (int x in new[] { 0, size.X / 2, size.X - 1 })
            foreach (int y in new[] { 0, size.Y / 2, size.Y - 1 })
            {
                var region = new Box2i(origin, origin + size);
                var position = origin + new V2i(x, y);
                var pixel = new PixelPosition(position, region);
                var ndc = new Ndc2d(pixel);
                string context = $"region={region}, pixel={position}";
                AssertPixel(new PixelPosition(ndc, region), position, region, context);
                var normalized = new V2d((x + 0.5) / size.X, (y + 0.5) / size.Y);
                Assert.AreEqual(normalized, pixel.NormalizedPosition, context + ", original pixel center");
                Assert.AreEqual(normalized, new PixelPosition(ndc, region).NormalizedPosition, context + ", reconstructed pixel center");
            }
        }

        [Test]
        public void ProjectPixelMapsIdentityViewIntoItsViewportRegion()
        {
            ICameraProjection[] projections =
            {
                new CameraProjectionPerspective(-2, 6, -3, 1, 1, 20),
                new CameraProjectionOrtho(-2, 6, -3, 1, 1, 20)
            };
            foreach (var projection in projections)
            foreach (var origin in Origins)
            foreach (var size in new[] { new V2i(801, 603), new V2i(17, 9), new V2i(1, 1) })
            foreach (var uv in new[] { new V2d(0.125, 0.875), new V2d(0.5, 0.5), new V2d(0.875, 0.125), new V2d(-0.2, 1.2) })
            foreach (double depth in new[] { 1.0, 3.0, 17.0 })
            {
                var region = new Box2i(origin, origin + size);
                var view = new RenderView(projection, region);
                double scale = projection is ICameraProjectionPerspective ? depth : 1.0;
                var world = new V3d((-2 + uv.X * 8) * scale, (-3 + uv.Y * 4) * scale, -depth);
                var expected = origin + new V2i((int)(uv.X * size.X), (int)((1 - uv.Y) * size.Y));
                string context = $"{projection.GetType().Name}, region={region}, uv={uv}, depth={depth}";
                AssertPixel(view.ProjectPixel(world), expected, region, context);
            }
        }

        private sealed class RenderView : IRenderView
        {
            public ICameraView View { get; } = new CameraViewRaw { ViewTrafo = Trafo3d.Identity };
            public ICameraProjection Projection { get; }
            public Box2i Region { get; }

            public RenderView(ICameraProjection projection, Box2i region)
            {
                Projection = projection;
                Region = region;
            }
        }
    }
}

using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PerlinNoiseTests
    {
        private const float ComparisonTolerance = 1e-6f;
        private const float BoundaryTolerance = 1e-4f;

        private static float Interpolate(float a, float b, float fraction)
        {
            float blend = (1.0f - (fraction * ConstantF.Pi).Cos()) * 0.5f;
            return a * (1.0f - blend) + b * blend;
        }

        private static float Expected(PerlinNoise noise, float x)
        {
            int ix = (int)Math.Floor(x);
            return Interpolate(noise.SmoothNoise(ix), noise.SmoothNoise(ix + 1), x - ix);
        }

        private static float Expected(PerlinNoise noise, float x, float y)
        {
            int ix = (int)Math.Floor(x);
            int iy = (int)Math.Floor(y);
            float fx = x - ix;
            float fy = y - iy;

            float x0 = Interpolate(noise.SmoothNoise(ix, iy), noise.SmoothNoise(ix + 1, iy), fx);
            float x1 = Interpolate(noise.SmoothNoise(ix, iy + 1), noise.SmoothNoise(ix + 1, iy + 1), fx);
            return Interpolate(x0, x1, fy);
        }

        private static float Expected(PerlinNoise noise, float x, float y, float z)
        {
            int ix = (int)Math.Floor(x);
            int iy = (int)Math.Floor(y);
            int iz = (int)Math.Floor(z);
            float fx = x - ix;
            float fy = y - iy;
            float fz = z - iz;

            float x00 = Interpolate(noise.SmoothNoise(ix, iy, iz), noise.SmoothNoise(ix + 1, iy, iz), fx);
            float x10 = Interpolate(noise.SmoothNoise(ix, iy + 1, iz), noise.SmoothNoise(ix + 1, iy + 1, iz), fx);
            float x01 = Interpolate(noise.SmoothNoise(ix, iy, iz + 1), noise.SmoothNoise(ix + 1, iy, iz + 1), fx);
            float x11 = Interpolate(noise.SmoothNoise(ix, iy + 1, iz + 1), noise.SmoothNoise(ix + 1, iy + 1, iz + 1), fx);
            return Interpolate(Interpolate(x00, x10, fy), Interpolate(x01, x11, fy), fz);
        }

        private static void AssertSameBits(float expected, float actual)
            => Assert.That(BitConverter.SingleToInt32Bits(actual),
                Is.EqualTo(BitConverter.SingleToInt32Bits(expected)));

        [Test]
        public void MixedNegativeCoordinatesUseContainingLatticeCells()
        {
            var noise = new PerlinNoise();

            Assert.That(noise.InterpolateNoise(-0.25f),
                Is.EqualTo(Expected(noise, -0.25f)).Within(ComparisonTolerance));
            Assert.That(noise.InterpolateNoise(-0.25f, 2.75f),
                Is.EqualTo(Expected(noise, -0.25f, 2.75f)).Within(ComparisonTolerance));
            Assert.That(noise.InterpolateNoise(-0.25f, 1.5f, -2.75f),
                Is.EqualTo(Expected(noise, -0.25f, 1.5f, -2.75f)).Within(ComparisonTolerance));
        }

        [Test]
        public void InterpolationIsContinuousAcrossNegativeIntegerBoundaries()
        {
            const float delta = 1e-4f;
            var noise = new PerlinNoise();

            float node1 = noise.InterpolateNoise(-1.0f);
            Assert.That(noise.InterpolateNoise(-1.0f - delta), Is.EqualTo(node1).Within(BoundaryTolerance));
            Assert.That(noise.InterpolateNoise(-1.0f + delta), Is.EqualTo(node1).Within(BoundaryTolerance));

            float node2 = noise.InterpolateNoise(-1.0f, -0.35f);
            Assert.That(noise.InterpolateNoise(-1.0f - delta, -0.35f), Is.EqualTo(node2).Within(BoundaryTolerance));
            Assert.That(noise.InterpolateNoise(-1.0f + delta, -0.35f), Is.EqualTo(node2).Within(BoundaryTolerance));

            float node3 = noise.InterpolateNoise(-0.25f, 1.5f, -2.0f);
            Assert.That(noise.InterpolateNoise(-0.25f, 1.5f, -2.0f - delta), Is.EqualTo(node3).Within(BoundaryTolerance));
            Assert.That(noise.InterpolateNoise(-0.25f, 1.5f, -2.0f + delta), Is.EqualTo(node3).Within(BoundaryTolerance));
        }

        [Test]
        public void LatticeNodesEqualSmoothedNoiseExactly()
        {
            var noise = new PerlinNoise();

            foreach (int x in new[] { -3, -1, 0, 4 })
                AssertSameBits(noise.SmoothNoise(x), noise.InterpolateNoise(x));

            foreach (var p in new[] { (-3, 2), (0, -2), (4, 5) })
                AssertSameBits(noise.SmoothNoise(p.Item1, p.Item2), noise.InterpolateNoise(p.Item1, p.Item2));

            foreach (var p in new[] { (-3, 2, -1), (0, -2, 3), (4, 5, 6) })
                AssertSameBits(noise.SmoothNoise(p.Item1, p.Item2, p.Item3), noise.InterpolateNoise(p.Item1, p.Item2, p.Item3));
        }

        [Test]
        public void PositiveDomainResultsRemainBitExact()
        {
            var noise = new PerlinNoise();

            Assert.That(BitConverter.SingleToInt32Bits(noise.InterpolateNoise(4.25f)), Is.EqualTo(0x3cc1727c));
            Assert.That(BitConverter.SingleToInt32Bits(noise.InterpolateNoise(1.25f, 2.75f)), Is.EqualTo(unchecked((int)0xbdee3b62)));
            Assert.That(BitConverter.SingleToInt32Bits(noise.InterpolateNoise(0.25f, 1.5f, 2.75f)), Is.EqualTo(unchecked((int)0xbdc41315)));
        }
    }
}

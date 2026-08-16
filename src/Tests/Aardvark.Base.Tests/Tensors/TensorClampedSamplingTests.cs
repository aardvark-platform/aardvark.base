using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class TensorClampedSamplingTests
    {
        private static long ReferenceOffset(long sample, long center, long first, long end, long stride)
        {
            if (sample < first) sample = first;
            else if (sample >= end) sample = end - 1;
            return (sample - center) * stride;
        }

        private static long[] GetClampedOffsets(int sampleCount, long center, long first, long end, long stride)
        {
            switch (sampleCount)
            {
                case 2:
                {
                    var value = Tensor.Index2SamplesClamped(center, first, end, stride);
                    return new[] { value.E0, value.E1 };
                }
                case 3:
                {
                    var value = Tensor.Index3SamplesClamped(center, first, end, stride);
                    return new[] { value.E0, value.E1, value.E2 };
                }
                case 4:
                {
                    var value = Tensor.Index4SamplesClamped(center, first, end, stride);
                    return new[] { value.E0, value.E1, value.E2, value.E3 };
                }
                case 5:
                {
                    var value = Tensor.Index5SamplesClamped(center, first, end, stride);
                    return new[] { value.E0, value.E1, value.E2, value.E3, value.E4 };
                }
                case 6:
                {
                    var value = Tensor.Index6SamplesClamped(center, first, end, stride);
                    return new[] { value.E0, value.E1, value.E2, value.E3, value.E4, value.E5 };
                }
                case 7:
                {
                    var value = Tensor.Index7SamplesClamped(center, first, end, stride);
                    return new[] { value.E0, value.E1, value.E2, value.E3, value.E4, value.E5, value.E6 };
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(sampleCount));
            }
        }

        [Test]
        public void ClampedSampleIndicesMatchReferenceFormula()
        {
            var firstCoordinates = new[] { -11L, 7L };
            var strides = new[] { 1L, 5L, -1L, -4L };

            for (var sampleCount = 2; sampleCount <= 7; sampleCount++)
            {
                var firstTap = -(sampleCount - 1) / 2;
                foreach (var first in firstCoordinates)
                {
                    for (var length = 1L; length <= 8; length++)
                    {
                        var end = first + length;
                        foreach (var stride in strides)
                        {
                            for (var center = first - sampleCount - 1; center <= end + sampleCount; center++)
                            {
                                var actual = GetClampedOffsets(sampleCount, center, first, end, stride);
                                for (var tap = 0; tap < sampleCount; tap++)
                                {
                                    var sample = center + firstTap + tap;
                                    var expected = ReferenceOffset(sample, center, first, end, stride);
                                    Assert.AreEqual(expected, actual[tap],
                                        $"samples={sampleCount}, first={first}, end={end}, center={center}, stride={stride}, tap={tap}");
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void SetScaledLanczosPreservesSmallConstantMatrices()
        {
            AssertSmallConstantScaling((target, source) => target.SetScaledLanczos(source));
        }

        [Test]
        public void SetScaledBSpline5PreservesSmallConstantMatrices()
        {
            AssertSmallConstantScaling((target, source) => target.SetScaledBSpline5(source));
        }

        private static void AssertSmallConstantScaling(Action<Matrix<byte>, Matrix<byte>> scale)
        {
            const byte expected = 1;
            for (var sourceY = 1; sourceY <= 4; sourceY++)
            {
                for (var sourceX = 1; sourceX <= 4; sourceX++)
                {
                    var source = new Matrix<byte>(sourceX, sourceY, expected);
                    for (var targetY = 1; targetY <= 4; targetY++)
                    {
                        for (var targetX = 1; targetX <= 4; targetX++)
                        {
                            var target = new Matrix<byte>(targetX, targetY);
                            scale(target, source);

                            for (var index = 0; index < target.Data.Length; index++)
                            {
                                Assert.AreEqual(expected, target.Data[index],
                                    $"source={sourceX}x{sourceY}, target={targetX}x{targetY}, index={index}");
                            }
                        }
                    }
                }
            }
        }
    }
}

using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class GenericInterpolationTests
    {
        private static void Check<T, U>((T a, T b)[] pairs, U[] weights, Func<T, T, U, T> formula)
        {
            string types = $"T={typeof(T).Name}, U={typeof(U).Name}";
            Func<T, T, U, T> lerp = null;
            Assert.DoesNotThrow(() => lerp = Ipol<T, U>.Lerp, types);
            Assert.That(lerp, Is.Not.Null, types);
            // Allow all affected types to report construction failures inside Assert.Multiple.
            if (lerp == null) return;
            Assert.That(Ipol<T, U>.Lerp, Is.SameAs(lerp), types);
            foreach (var (a, b) in pairs)
            {
                var generated = Ipol.Generate(a, b, lerp);
                var genericGenerated = Ipol<T, U>.Generate(a, b, lerp);
                foreach (var t in weights)
                {
                    string context = $"{types}, a={a}, b={b}, t={t}";
                    T expected = formula(a, b, t);
                    Assert.That(lerp(a, b, t), Is.EqualTo(expected), context);
                    Assert.That(generated(t), Is.EqualTo(expected), $"Ipol.Generate: {context}");
                    Assert.That(genericGenerated(t), Is.EqualTo(expected), $"Ipol<T,U>.Generate: {context}");
                    Assert.That(Ipol<T, U>.Lerp, Is.SameAs(lerp), context);
                }
            }
        }

        [Test]
        public void FloatWeightsConstructCachedScalarAndVectorInterpolation()
        {
            var weights = new[] { -2.5f, -0.25f, 0f, 0.1f, 0.125f, 0.5f, 0.9f, 1f, 1.25f, 2.5f };
            Assert.Multiple(() =>
            {
                Check<float, float>(new[] { (-8f, 12f), (1.25f, -3.5f), (float.MaxValue, -float.MaxValue) },
                    weights, (a, b, t) => a * (1f - t) + b * t);
                Check<V2f, float>(new[] {
                    (new V2f(-8f, 1.25f), new V2f(12f, -3.5f)),
                    (new V2f(float.MaxValue, 0f), new V2f(-float.MaxValue, 1f)) },
                    weights, (a, b, t) => a * (1f - t) + b * t);
                Check<V3f, float>(new[] {
                    (new V3f(-8f, 1.25f, -4f), new V3f(12f, -3.5f, 16f)),
                    (new V3f(float.MaxValue, 0f, 16f), new V3f(-float.MaxValue, 1f, -4f)) },
                    weights, (a, b, t) => a * (1f - t) + b * t);
                Check<V4f, float>(new[] {
                    (new V4f(-8f, 1.25f, -4f, 0f), new V4f(12f, -3.5f, 16f, 1f)),
                    (new V4f(float.MaxValue, 0f, 16f, -3.5f), new V4f(-float.MaxValue, 1f, -4f, 1.25f)) },
                    weights, (a, b, t) => a * (1f - t) + b * t);
            });
        }

        [Test]
        public void DoubleWeightsRetainCachedScalarAndVectorInterpolation()
        {
            var weights = new[] { -2.5, -0.25, 0.0, 0.1, 0.125, 0.5, 0.9, 1.0, 1.25, 2.5 };
            Assert.Multiple(() =>
            {
                Check<double, double>(new[] { (-8.0, 12.0), (1.25, -3.5), (double.MaxValue, -double.MaxValue) },
                    weights, (a, b, t) => a * (1.0 - t) + b * t);
                Check<V2d, double>(new[] {
                    (new V2d(-8.0, 1.25), new V2d(12.0, -3.5)),
                    (new V2d(double.MaxValue, 0.0), new V2d(-double.MaxValue, 1.0)) },
                    weights, (a, b, t) => a * (1.0 - t) + b * t);
                Check<V3d, double>(new[] {
                    (new V3d(-8.0, 1.25, -4.0), new V3d(12.0, -3.5, 16.0)),
                    (new V3d(double.MaxValue, 0.0, 16.0), new V3d(-double.MaxValue, 1.0, -4.0)) },
                    weights, (a, b, t) => a * (1.0 - t) + b * t);
                Check<V4d, double>(new[] {
                    (new V4d(-8.0, 1.25, -4.0, 0.0), new V4d(12.0, -3.5, 16.0, 1.0)),
                    (new V4d(double.MaxValue, 0.0, 16.0, -3.5), new V4d(-double.MaxValue, 1.0, -4.0, 1.25)) },
                    weights, (a, b, t) => a * (1.0 - t) + b * t);
            });
        }
    }
}

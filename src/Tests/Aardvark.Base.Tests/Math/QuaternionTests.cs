using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class QuaternionTests
    {
        private static readonly int iterations = 10000;

        private static QuaternionD GetRandomQuat(RandomSystem rnd)
        {
            return new QuaternionD(rnd.UniformV4d() * 10);
        }

        [Test]
        public static void Comparison()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q0 = GetRandomQuat(rnd);
                var q1 = new QuaternionD((V4d)q0 + V4d.OIOI);

                Assert.IsFalse(q0.Equals(q1));
                Assert.IsFalse(q0 == q1);
                Assert.IsTrue(q0 != q1);
            }
        }

        [Test]
        public static void HamiltonTest()
        {
            var i = QuaternionD.I;
            var j = QuaternionD.J;
            var k = QuaternionD.K;
            var minus_one = new QuaternionD(-1, 0, 0, 0);

            Assert.IsTrue(Fun.ApproximateEquals(i * i, minus_one, 0.00001));
            Assert.IsTrue(Fun.ApproximateEquals(j * j, minus_one, 0.00001));
            Assert.IsTrue(Fun.ApproximateEquals(k * k, minus_one, 0.00001));
            Assert.IsTrue(Fun.ApproximateEquals(i * j * k, minus_one, 0.00001));
        }

        [Test]
        public static void IdentityTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);
                var id = QuaternionD.Identity;

                Assert.IsTrue(Fun.ApproximateEquals(q * id, q, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(id * q, q, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(id * id, id, 0.00001));
            }
        }

        [Test]
        public static void NormalizeTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);

                // Normalized property
                var q1 = q.Normalized;

                // Normalize method
                var q2 = new QuaternionD(q);
                Quaternion.Normalize(ref q2);

                Assert.IsTrue(Fun.ApproximateEquals(q1.Norm, 1, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q2.Norm, 1, 0.00001));
            }
        }

        [Test]
        public static void InverseTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);
                var id = QuaternionD.Identity;

                // Inverse property
                var qi1 = q.Inverse;

                // Invert method
                var qi2 = new QuaternionD(q);
                Quaternion.Invert(ref qi2);

                Assert.IsTrue(Fun.ApproximateEquals(q * qi1, id, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(qi1 * q, id, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q * qi2, id, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(qi2 * q, id, 0.00001));
            }
        }

        [Test]
        public static void ConjugateTest()
        {
            var rnd = new RandomSystem(1);

            for (int n = 0; n < iterations; n++)
            {
                var q1 = GetRandomQuat(rnd);
                var q2 = GetRandomQuat(rnd);

                var q1_x_q2_conj = Quaternion.Conjugated(q1 * q2);
                var q2_conj_x_q1_conj = q2.Conjugated * q1.Conjugated;

                Assert.IsTrue(Fun.ApproximateEquals(q1_x_q2_conj, q2_conj_x_q1_conj, 0.00001));

                var i = QuaternionD.I;
                var j = QuaternionD.J;
                var k = QuaternionD.K;
                var q1_conj = -0.5 * (q1 + (i * q1 * i) + (j * q1 * j) + (k * q1 * k));

                Assert.IsTrue(Fun.ApproximateEquals(q1.Conjugated, q1_conj, 0.00001));
            }
        }

        [Test]
        public static void NormTest()
        {
            var rnd = new RandomSystem(1);

            for (int n = 0; n < iterations; n++)
            {
                var q1 = GetRandomQuat(rnd);
                var q2 = GetRandomQuat(rnd);

                var s = rnd.UniformDouble() * 100;
                var q1_x_s_norm = Quaternion.Norm(q1 * s);
                var q1_norm_x_s_norm = q1.Norm * s;

                Assert.IsTrue(Fun.ApproximateEquals(q1_x_s_norm, q1_norm_x_s_norm, 0.00001));

                var q1_x_q2_norm = Quaternion.Norm(q1 * q2);
                var q1_norm_x_q2_norm = q1.Norm * q2.Norm;

                Assert.IsTrue(Fun.ApproximateEquals(q1_x_q2_norm, q1_norm_x_q2_norm, 0.00001));
            }
        }

        [Test]
        public static void MatrixRepresentation()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q1 = GetRandomQuat(rnd);
                var q2 = GetRandomQuat(rnd);

                var m1 = (M44d)q1;
                var m2 = (M44d)q2;

                var q1_plus_q2 = (M44d)(q1 + q2);
                var m1_plus_m2 = m1 + m2;

                var q1q2 = (M44d)(q1 * q2);
                var m1m2 = m1 * m2;

                var q2q1 = (M44d)(q2 * q1);
                var m2m1 = m2 * m1;

                Assert.IsTrue(Fun.ApproximateEquals(q1_plus_q2, m1_plus_m2, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q1q2, m1m2, 0.00001));
                Assert.IsTrue(Fun.ApproximateEquals(q2q1, m2m1, 0.00001));
            }
        }

        [Test]
        public static void ToStringAndParse()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var q = GetRandomQuat(rnd);

                var str = q.ToString();
                var parsed = QuaternionD.Parse(str);

                Assert.IsTrue(Fun.ApproximateEquals(parsed, q, 0.00001));
            }
        }
    }
}
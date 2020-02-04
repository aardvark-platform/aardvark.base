using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    [DisassemblyDiagnoser(printAsm: true)]
    public class Rot3dTransform
    {
        V3d[] arr = new V3d[100000];
        Rot3d rotG = Rot3d.RotationX(Constant.Pi);
        M33d matG = M33d.Identity;

        //[GlobalSetup]
        public Rot3dTransform()
        {
            var rnd1 = new Random(1);
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new V3d(rnd1.NextDouble(), rnd1.NextDouble(), rnd1.NextDouble());
        }

        [Benchmark]
        public void Rot3d_Transform()
        {
            var rot = rotG;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = rot.Transform(arr[i]);
        }

        static V3d TransformM33dPrecomputed(M33d mat, V3d v)
        {
            return mat.Transform(v);
        }

        [Benchmark]
        public void Rot3d_TransformM33dPrecomputed()
        {
            var local = arr;
            var mat = matG;
            for (int i = 0; i < local.Length; i++)
                arr[i] = TransformM33dPrecomputed(mat, arr[i]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // does not fully inline without
        static V3d TransformM33dPrecomputed_Inline(M33d m, V3d v)
        {
            return new V3d(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z
                );
        }

        [Benchmark]
        public void Rot3d_TransformM33dPrecomputed_Inline()
        {
            var local = arr;
            var mat = matG;
            for (int i = 0; i < local.Length; i++)
                arr[i] = TransformM33dPrecomputed_Inline(mat, arr[i]);
        }

        static V3d TransformUsingM33d(Rot3d rot, V3d v)
        {
            return ((M33d)rot).Transform(v);
        }

        [Benchmark]
        public void Rot3d_TransformUsingM33d()
        {
            var rot = rotG;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = TransformUsingM33d(rot, arr[i]);
        }

        static V3d TransformUsingQuaternion(Rot3d q, V3d v)
        {
            var r = q * new Rot3d(0, v) * q.Conjugated;
            return new V3d(r.X, r.Y, r.Z);
        }

        [Benchmark]
        public void Rot3d_TransformUsingQuaternion()
        {
            var rot = rotG;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = TransformUsingQuaternion(rot, arr[i]);
        }

        static V3d TransformUsingQuaternionOpt1(Rot3d rot, V3d v)
        {
            // Multiply(a, b)
            //return new Rot3d(
            //    a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            //    a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            //    a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            //    a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
            //    );

            // rot * Rot3d(0, v)
            var q = new Rot3d(
              - rot.X * v.X - rot.Y * v.Y - rot.Z * v.Z,
                rot.W * v.X + rot.Y * v.Z - rot.Z * v.Y,
                rot.W * v.Y + rot.Z * v.X - rot.X * v.Z,
                rot.W * v.Z + rot.X * v.Y - rot.Y * v.X);
            
            // q * Rot3d(rot.W, -rot.V) (rot.Conungated)
            return new V3d(
                -q.W * rot.X + q.X * rot.W - q.Y * rot.Z + q.Z * rot.Y,
                -q.W * rot.Y + q.Y * rot.W - q.Z * rot.X + q.X * rot.Z,
                -q.W * rot.Z + q.Z * rot.W - q.X * rot.Y + q.Y * rot.X
                );
        }

        static V3d TransformUsingQuaternionOpt2(Rot3d rot, V3d v)
        {
            // rot * Rot3d(0, v)
            var W = -rot.X * v.X - rot.Y * v.Y - rot.Z * v.Z;
            var X = rot.W * v.X + rot.Y * v.Z - rot.Z * v.Y;
            var Y = rot.W * v.Y + rot.Z * v.X - rot.X * v.Z;
            var Z = rot.W * v.Z + rot.X * v.Y - rot.Y * v.X;

            // q * Rot3d(rot.W, -rot.V) (rot.Conungated)
            return new V3d(
                -W * rot.X + X * rot.W - Y * rot.Z + Z * rot.Y,
                -W * rot.Y + Y * rot.W - Z * rot.X + X * rot.Z,
                -W * rot.Z + Z * rot.W - X * rot.Y + Y * rot.X
                );
        }

        [Benchmark]
        public void Rot3d_TransformUsingQuaternionOpt1()
        {
            var rot = rotG;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = TransformUsingQuaternionOpt1(rot, arr[i]);
        }

        [Benchmark]
        public void Rot3d_TransformUsingQuaternionOpt2()
        {
            var rot = rotG;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = TransformUsingQuaternionOpt2(rot, arr[i]);
        }

        [Test]
        public static void Rot3dTransformTest()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 10000; i++)
            {
                var v = rnd.UniformV3d();
                var axis = rnd.UniformV3dDirection();
                var ang = rnd.UniformDouble();
                var rot = new Rot3d(axis, ang);

                var test1 = TransformUsingM33d(rot, v);
                var test2 = rot.Transform(v);
                var test3 = TransformUsingQuaternion(rot, v);
                var test4 = TransformUsingQuaternionOpt1(rot, v);

                Assert.IsTrue(test1.ApproximateEquals(test2, 1e-5));
                Assert.IsTrue(test1.ApproximateEquals(test3, 1e-5));
                Assert.IsTrue(test1.ApproximateEquals(test4, 1e-5));
            }
        }

        static V3d InvTransformUsingM33d(Rot3d rot, V3d v)
        {
            return ((M33d)rot).TransposedTransform(v);
        }

        static V3d InvTransformUsingQuaternion(Rot3d rot, V3d v)
        {
            var r = rot * new Rot3d(0, v) * rot.Conjugated;
            return new V3d(r.X, r.Y, r.Z);
        }

        static V3d InvTransformUsingQuaternionOpt(Rot3d rot, V3d v)
        {
            // TODO
            // Multiply(a, b)
            //return new Rot3d(
            //    a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            //    a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            //    a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            //    a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X
            //    );

            // rot.Conungated * Rot3d(0, v)
            var q = new Rot3d(
               +rot.X * v.X + rot.Y * v.Y + rot.Z * v.Z,
                rot.W * v.X - rot.Y * v.Z + rot.Z * v.Y,
                rot.W * v.Y - rot.Z * v.X + rot.X * v.Z,
                rot.W * v.Z - rot.X * v.Y + rot.Y * v.X);

            // q * Rot3d(rot.W, -rot.V) (rot.Conungated)

            return new V3d(
                q.W * rot.X + q.X * rot.W + q.Y * rot.Z - q.Z * rot.Y,
                q.W * rot.Y + q.Y * rot.W + q.Z * rot.X - q.X * rot.Z,
                q.W * rot.Z + q.Z * rot.W + q.X * rot.Y - q.Y * rot.X
                );
        }

        [Test]
        public static void Rot3dInvTransformTest()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 10000; i++)
            {
                var v = rnd.UniformV3d();
                var axis = rnd.UniformV3dDirection();
                var ang = rnd.UniformDouble();
                var rot = new Rot3d(axis, ang);

                var test1 = InvTransformUsingM33d(rot, v);
                var test2 = rot.InvTransform(v);
                var test3 = InvTransformUsingQuaternion(rot, v);
                var test4 = InvTransformUsingQuaternionOpt(rot, v);

                Assert.IsTrue(test1.ApproximateEquals(test2, 1e-5));
                Assert.IsTrue(test1.ApproximateEquals(test3, 1e-5));
                Assert.IsTrue(test1.ApproximateEquals(test4, 1e-5));
            }
        }
    }
}

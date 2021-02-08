using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Rot3f

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class RotateIntoFloat
    {
        private static class Implementations
        {
            public static Rot3f Original(V3f from, V3f into)
            {
                var angle = from.AngleBetween(into);

                if (angle < 1e-6f)
                    return Rot3f.Identity;
                else if (ConstantF.Pi - angle < 1e-6f)
                    return new Rot3f(0, from.AxisAlignedNormal());
                else
                {
                    V3f axis = Vec.Cross(from, into).Normalized;
                    return Rot3f.Rotation(axis, angle);
                }
            }

            // https://stackoverflow.com/questions/1171849/finding-quaternion-representing-the-rotation-from-one-vector-to-another
            public static Rot3f HalfWayVec(V3f from, V3f into)
            {
                if (from.Dot(into).ApproximateEquals(-1))
                    return new Rot3f(0, from.AxisAlignedNormal());
                else
                {
                    V3f half = Vec.Normalized(from + into);
                    QuaternionF q = new QuaternionF(Vec.Dot(from, half), Vec.Cross(from, half));
                    return new Rot3f(q.Normalized);
                }
            }

            public static Rot3f HalfWayQuat(V3f from, V3f into)
            {
                var d = Vec.Dot(from, into);

                if (d.ApproximateEquals(-1))
                    return new Rot3f(0, from.AxisAlignedNormal());
                else
                {
                    QuaternionF q = new QuaternionF(d + 1, Vec.Cross(from, into));
                    return new Rot3f(q.Normalized);
                }
            }

            public static Dictionary<string, Func<V3f, V3f, Rot3f>> All
            {
                get => new Dictionary<string, Func<V3f, V3f, Rot3f>>()
                {
                    { "Original", Original },
                    { "Half-way vector", HalfWayVec },
                    { "Half-way quaternion", HalfWayQuat }
                };
            }
        }

        const int count = 1000000;
        readonly V3f[] A = new V3f[count];
        readonly V3f[] B = new V3f[count];

        private static V3f RndAxis(RandomSystem rnd)
            => rnd.UniformV3fDirection();

        public RotateIntoFloat()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => RndAxis(rnd));
            B.SetByIndex(i =>
            {
                // type = 0 -> random
                // type = 1 -> 0 or 180° degrees with some error
                var type = rnd.UniformInt(2);

                if (type == 0)
                {
                    return RndAxis(rnd);
                }
                else if (type == 1)
                {
                    // Some vectors will not normalize to 1.0 -> provoke numerical issues in V3f
                    var v = Vec.Normalized(A[i] * rnd.UniformFloat());

                    // Add some small error
                    if (rnd.UniformDouble() < 0.5)
                    {
                        var eps = rnd.UniformV3f() * (i / 100) * 1e-12f;
                        v = Vec.Normalized(v + eps);
                    }

                    // Flip
                    if (rnd.UniformDouble() < 0.5)
                        v = -v;

                    return v;
                }

                return V3f.Zero;
            });
        }

        private double[] ComputeAbsoluteError(Func<V3f, V3f, Rot3f> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                V3f from = A[i];
                V3f into = B[i];

                Rot3f r = f(from, into);
                V3f res = r * from;

                error[i] = Vec.AngleBetween(new V3d(res), new V3d(into));
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            Console.WriteLine("Benchmarking numerical stability for Rot3f.RotateInto() variants");
            Console.WriteLine();

            foreach (var m in Implementations.All)
            {
                var errors = ComputeAbsoluteError(m.Value);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        [Test]
        public void CorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.All)
                {
                    V3f from = A[i];
                    V3f into = B[i];

                    Rot3f r = f.Value(from, into);
                    V3f res = r * from;

                    Assert.Less(
                        res.AngleBetween(into), 1e-3f,
                        string.Format("{0} implementation incorrect!", f.Key)
                    );
                }
            }
        }

        [Benchmark]
        public Rot3f Original()
        {
            Rot3f accum = Rot3f.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.Original(A[i], B[i]);
            }

            return accum;
        }

        [Benchmark]
        public Rot3f HalfWayVec()
        {
            Rot3f accum = Rot3f.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.HalfWayVec(A[i], B[i]);
            }

            return accum;
        }

        [Benchmark]
        public Rot3f HalfWayQuat()
        {
            Rot3f accum = Rot3f.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.HalfWayQuat(A[i], B[i]);
            }

            return accum;
        }
    }

    #endregion

    #region Rot3d

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class RotateIntoDouble
    {
        private static class Implementations
        {
            public static Rot3d Original(V3d from, V3d into)
            {
                var angle = from.AngleBetween(into);

                if (angle < 1e-15)
                    return Rot3d.Identity;
                else if (Constant.Pi - angle < 1e-15)
                    return new Rot3d(0, from.AxisAlignedNormal());
                else
                {
                    V3d axis = Vec.Cross(from, into).Normalized;
                    return Rot3d.Rotation(axis, angle);
                }
            }

            // https://stackoverflow.com/questions/1171849/finding-quaternion-representing-the-rotation-from-one-vector-to-another
            public static Rot3d HalfWayVec(V3d from, V3d into)
            {
                if (from.Dot(into).ApproximateEquals(-1))
                    return new Rot3d(0, from.AxisAlignedNormal());
                else
                {
                    V3d half = Vec.Normalized(from + into);
                    QuaternionD q = new QuaternionD(Vec.Dot(from, half), Vec.Cross(from, half));
                    return new Rot3d(q.Normalized);
                }
            }

            public static Rot3d HalfWayQuat(V3d from, V3d into)
            {
                var d = Vec.Dot(from, into);

                if (d.ApproximateEquals(-1))
                    return new Rot3d(0, from.AxisAlignedNormal());
                else
                {
                    QuaternionD q = new QuaternionD(d + 1, Vec.Cross(from, into));
                    return new Rot3d(q.Normalized);
                }
            }

            public static Dictionary<string, Func<V3d, V3d, Rot3d>> All
            {
                get => new Dictionary<string, Func<V3d, V3d, Rot3d>>()
                {
                    { "Original", Original },
                    { "Half-way vector", HalfWayVec },
                    { "Half-way quaternion", HalfWayQuat }
                };
            }
        }

        const int count = 1000000;
        readonly V3d[] A = new V3d[count];
        readonly V3d[] B = new V3d[count];

        private static V3d RndAxis(RandomSystem rnd)
            => rnd.UniformV3dDirection();

        public RotateIntoDouble()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => RndAxis(rnd));
            B.SetByIndex(i =>
            {
                // type = 0 -> random
                // type = 1 -> 0 or 180° degrees with some error
                var type = rnd.UniformInt(2);

                if (type == 0)
                {
                    return RndAxis(rnd);
                }
                else if (type == 1)
                {
                    // Some vectors will not normalize to 1.0 -> provoke numerical issues in V3d
                    var v = Vec.Normalized(A[i] * rnd.UniformDouble());

                    // Add some small error
                    if (rnd.UniformDouble() < 0.5)
                    {
                        var eps = rnd.UniformV3d() * (i / 100) * 1e-22;
                        v = Vec.Normalized(v + eps);
                    }

                    // Flip
                    if (rnd.UniformDouble() < 0.5)
                        v = -v;

                    return v;
                }

                return V3d.Zero;
            });
        }

        private double[] ComputeAbsoluteError(Func<V3d, V3d, Rot3d> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                V3d from = A[i];
                V3d into = B[i];

                Rot3d r = f(from, into);
                V3d res = r * from;

                error[i] = Vec.AngleBetween(new V3d(res), new V3d(into));
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            Console.WriteLine("Benchmarking numerical stability for Rot3d.RotateInto() variants");
            Console.WriteLine();

            foreach (var m in Implementations.All)
            {
                var errors = ComputeAbsoluteError(m.Value);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        [Test]
        public void CorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.All)
                {
                    V3d from = A[i];
                    V3d into = B[i];

                    Rot3d r = f.Value(from, into);
                    V3d res = r * from;

                    Assert.Less(
                        res.AngleBetween(into), 1e-12,
                        string.Format("{0} implementation incorrect!", f.Key)
                    );
                }
            }
        }

        [Benchmark]
        public Rot3d Original()
        {
            Rot3d accum = Rot3d.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.Original(A[i], B[i]);
            }

            return accum;
        }

        [Benchmark]
        public Rot3d HalfWayVec()
        {
            Rot3d accum = Rot3d.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.HalfWayVec(A[i], B[i]);
            }

            return accum;
        }

        [Benchmark]
        public Rot3d HalfWayQuat()
        {
            Rot3d accum = Rot3d.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.HalfWayQuat(A[i], B[i]);
            }

            return accum;
        }
    }

    #endregion

}
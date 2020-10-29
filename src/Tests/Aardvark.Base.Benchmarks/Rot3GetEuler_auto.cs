using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Rot3f

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class Rot3fGetEuler
    {
        private static class Implementations
        {
            public static V3f Original(Rot3f r)
            {
                var test = r.W * r.Y - r.X * r.Z;
                if (test > 0.5f - Constant<float>.PositiveTinyValue) // singularity at north pole
                {
                    return new V3f(
                        2 * Fun.Atan2(r.X, r.W),
                        ConstantF.PiHalf,
                        0);
                }
                if (test < -0.5f + Constant<float>.PositiveTinyValue) // singularity at south pole
                {
                    return new V3f(
                        2 * Fun.Atan2(r.X, r.W),
                        -ConstantF.PiHalf,
                        0);
                }
                // From Wikipedia, conversion between quaternions and Euler angles.
                return new V3f(
                            Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                      1 - 2 * (r.X * r.X + r.Y * r.Y)),
                            Fun.AsinClamped(2 * test),
                            Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                      1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
            }

            public static V3f CopySign(Rot3f r)
            {
                var test = r.W * r.Y - r.X * r.Z;

                if (test.Abs() >= 0.5f - Constant<float>.PositiveTinyValue)
                {
                    return new V3f(
                        2 * Fun.Atan2(r.X, r.W),
                        Fun.CopySign(ConstantF.PiHalf, test),
                        0);
                }
                else
                {
                    return new V3f(
                                Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                          1 - 2 * (r.X * r.X + r.Y * r.Y)),
                                Fun.AsinClamped(2 * test),
                                Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                          1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
                }
            }

            public static Dictionary<string, Func<Rot3f, V3f>> All
            {
                get => new Dictionary<string, Func<Rot3f, V3f>>()
                {
                    { "Original", Original },
                    { "CopySign", CopySign }
                };
            }
        }

        const int count = 1000000;
        readonly Rot3f[] Rots = new Rot3f[count];
        readonly V3f[] EulerAngles = new V3f[count];

        public Rot3fGetEuler()
        {
            var rnd = new RandomSystem(1);
            EulerAngles.SetByIndex(i =>
            {
                var vrnd = rnd.UniformV3f() * ConstantF.Pi * 2;
                var veps = (rnd.UniformDouble() < 0.5) ? rnd.UniformV3f() * (i / (float)100) * 1e-12f : V3f.Zero;
                var vspc = new V3f(rnd.UniformV3i(4)) * ConstantF.PiHalf + veps;

                float roll = (rnd.UniformDouble() < 0.5) ? vrnd.X : vspc.X;
                float pitch = (rnd.UniformDouble() < 0.5) ? vrnd.Y : vspc.Y;
                float yaw = (rnd.UniformDouble() < 0.5) ? vrnd.Z : vspc.Z;

                return new V3f(roll, pitch, yaw);
            });
            Rots.SetByIndex(i => Rot3f.RotationEuler(EulerAngles[i]));
        }

        private double[] ComputeAbsoluteError(Func<Rot3f, V3f> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var r1 = Rots[i];
                var r2 = Rot3f.RotationEuler(f(r1)).Normalized;

                error[i] = Rot.Distance(r1, r2);
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            Console.WriteLine("Benchmarking numerical stability for Rot3f.GetEulerAngles() variants");
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
                    Rot3f r1 = Rots[i];
                    Rot3f r2 = Rot3f.RotationEuler(f.Value(r1));

                    Assert.Less(
                        Rot.Distance(r1, r2), 1e-2f,
                        string.Format("{0} implementation incorrect!", f.Key)
                    );
                }
            }
        }

        [Benchmark]
        public V3f Original()
        {
            V3f accum = V3f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.Original(Rots[i]);
            }

            return accum;
        }

        [Benchmark]
        public V3f CopySign()
        {
            V3f accum = V3f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.CopySign(Rots[i]);
            }

            return accum;
        }
    }

    #endregion

    #region Rot3d

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class Rot3dGetEuler
    {
        private static class Implementations
        {
            public static V3d Original(Rot3d r)
            {
                var test = r.W * r.Y - r.X * r.Z;
                if (test > 0.5 - Constant<double>.PositiveTinyValue) // singularity at north pole
                {
                    return new V3d(
                        2 * Fun.Atan2(r.X, r.W),
                        Constant.PiHalf,
                        0);
                }
                if (test < -0.5 + Constant<double>.PositiveTinyValue) // singularity at south pole
                {
                    return new V3d(
                        2 * Fun.Atan2(r.X, r.W),
                        -Constant.PiHalf,
                        0);
                }
                // From Wikipedia, conversion between quaternions and Euler angles.
                return new V3d(
                            Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                      1 - 2 * (r.X * r.X + r.Y * r.Y)),
                            Fun.AsinClamped(2 * test),
                            Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                      1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
            }

            public static V3d CopySign(Rot3d r)
            {
                var test = r.W * r.Y - r.X * r.Z;

                if (test.Abs() >= 0.5 - Constant<double>.PositiveTinyValue)
                {
                    return new V3d(
                        2 * Fun.Atan2(r.X, r.W),
                        Fun.CopySign(Constant.PiHalf, test),
                        0);
                }
                else
                {
                    return new V3d(
                                Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                          1 - 2 * (r.X * r.X + r.Y * r.Y)),
                                Fun.AsinClamped(2 * test),
                                Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                          1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
                }
            }

            public static Dictionary<string, Func<Rot3d, V3d>> All
            {
                get => new Dictionary<string, Func<Rot3d, V3d>>()
                {
                    { "Original", Original },
                    { "CopySign", CopySign }
                };
            }
        }

        const int count = 1000000;
        readonly Rot3d[] Rots = new Rot3d[count];
        readonly V3d[] EulerAngles = new V3d[count];

        public Rot3dGetEuler()
        {
            var rnd = new RandomSystem(1);
            EulerAngles.SetByIndex(i =>
            {
                var vrnd = rnd.UniformV3d() * Constant.Pi * 2;
                var veps = (rnd.UniformDouble() < 0.5) ? rnd.UniformV3d() * (i / (double)100) * 1e-22 : V3d.Zero;
                var vspc = new V3d(rnd.UniformV3i(4)) * Constant.PiHalf + veps;

                double roll = (rnd.UniformDouble() < 0.5) ? vrnd.X : vspc.X;
                double pitch = (rnd.UniformDouble() < 0.5) ? vrnd.Y : vspc.Y;
                double yaw = (rnd.UniformDouble() < 0.5) ? vrnd.Z : vspc.Z;

                return new V3d(roll, pitch, yaw);
            });
            Rots.SetByIndex(i => Rot3d.RotationEuler(EulerAngles[i]));
        }

        private double[] ComputeAbsoluteError(Func<Rot3d, V3d> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var r1 = Rots[i];
                var r2 = Rot3d.RotationEuler(f(r1)).Normalized;

                error[i] = Rot.Distance(r1, r2);
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            Console.WriteLine("Benchmarking numerical stability for Rot3d.GetEulerAngles() variants");
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
                    Rot3d r1 = Rots[i];
                    Rot3d r2 = Rot3d.RotationEuler(f.Value(r1));

                    Assert.Less(
                        Rot.Distance(r1, r2), 1e-7,
                        string.Format("{0} implementation incorrect!", f.Key)
                    );
                }
            }
        }

        [Benchmark]
        public V3d Original()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.Original(Rots[i]);
            }

            return accum;
        }

        [Benchmark]
        public V3d CopySign()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.CopySign(Rots[i]);
            }

            return accum;
        }
    }

    #endregion

}
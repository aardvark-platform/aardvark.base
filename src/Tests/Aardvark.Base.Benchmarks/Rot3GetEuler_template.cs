using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# var fields = new[] {"X", "Y", "Z"};
    //# foreach (var rt in Meta.RealTypes) {
    //#     var isDouble = rt == Meta.DoubleType;
    //#     var rtype = rt.Name;
    //#     var Rtype = rt.Caps;
    //#     var fc = rt.Char;
    //#     var rot3t = "Rot3" + fc;
    //#     var rot2t = "Rot2" + fc;
    //#     var quatt = "Quaternion" + fc.ToUpper();
    //#     var v3t = Meta.VecTypeOf(3, rt).Name;
    //#     var half = isDouble ? "0.5" : "0.5f";
    //#     var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#     var piHalf = isDouble ? "Constant.PiHalf" : "ConstantF.PiHalf";
    //#     var eps = isDouble ? "1e-7" : "1e-2f";
    #region __rot3t__

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class __rot3t__GetEuler
    {
        private static class Implementations
        {
            public static __v3t__ Original(__rot3t__ r)
            {
                var test = r.W * r.Y - r.X * r.Z;
                if (test > __half__ - Constant<__rtype__>.PositiveTinyValue) // singularity at north pole
                {
                    return new __v3t__(
                        2 * Fun.Atan2(r.X, r.W),
                        __piHalf__,
                        0);
                }
                if (test < -__half__ + Constant<__rtype__>.PositiveTinyValue) // singularity at south pole
                {
                    return new __v3t__(
                        2 * Fun.Atan2(r.X, r.W),
                        -__piHalf__,
                        0);
                }
                // From Wikipedia, conversion between quaternions and Euler angles.
                return new __v3t__(
                            Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                      1 - 2 * (r.X * r.X + r.Y * r.Y)),
                            Fun.AsinClamped(2 * test),
                            Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                      1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
            }

            public static __v3t__ CopySign(__rot3t__ r)
            {
                var test = r.W * r.Y - r.X * r.Z;

                if (test.Abs() >= __half__ - Constant<__rtype__>.PositiveTinyValue)
                {
                    return new __v3t__(
                        2 * Fun.Atan2(r.X, r.W),
                        Fun.CopySign(__piHalf__, test),
                        0);
                }
                else
                {
                    return new __v3t__(
                                Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                          1 - 2 * (r.X * r.X + r.Y * r.Y)),
                                Fun.AsinClamped(2 * test),
                                Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                          1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
                }
            }

            public static Dictionary<string, Func<__rot3t__, __v3t__>> All
            {
                get => new Dictionary<string, Func<__rot3t__, __v3t__>>()
                {
                    { "Original", Original },
                    { "CopySign", CopySign }
                };
            }
        }

        const int count = 1000000;
        readonly __rot3t__[] Rots = new __rot3t__[count];
        readonly __v3t__[] EulerAngles = new __v3t__[count];

        public __rot3t__GetEuler()
        {
            var rnd = new RandomSystem(1);
            EulerAngles.SetByIndex(i =>
            {
                //# var err = isDouble ? "1e-22" : "1e-12f";
                var vrnd = rnd.Uniform__v3t__() * __pi__ * 2;
                var veps = (rnd.UniformDouble() < 0.5) ? rnd.Uniform__v3t__() * (i / (__rtype__)100) * __err__ : __v3t__.Zero;
                var vspc = new __v3t__(rnd.UniformV3i(4)) * __piHalf__ + veps;

                __rtype__ roll = (rnd.UniformDouble() < 0.5) ? vrnd.X : vspc.X;
                __rtype__ pitch = (rnd.UniformDouble() < 0.5) ? vrnd.Y : vspc.Y;
                __rtype__ yaw = (rnd.UniformDouble() < 0.5) ? vrnd.Z : vspc.Z;

                return new __v3t__(roll, pitch, yaw);
            });
            Rots.SetByIndex(i => __rot3t__.RotationEuler(EulerAngles[i]));
        }

        private double[] ComputeAbsoluteError(Func<__rot3t__, __v3t__> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var r1 = Rots[i];
                var r2 = __rot3t__.RotationEuler(f(r1)).Normalized;

                error[i] = Rot.Distance(r1, r2);
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            Console.WriteLine("Benchmarking numerical stability for __rot3t__.GetEulerAngles() variants");
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
                    __rot3t__ r1 = Rots[i];
                    __rot3t__ r2 = __rot3t__.RotationEuler(f.Value(r1));

                    Assert.Less(
                        Rot.Distance(r1, r2), __eps__,
                        string.Format("{0} implementation incorrect!", f.Key)
                    );
                }
            }
        }

        [Benchmark]
        public __v3t__ Original()
        {
            __v3t__ accum = __v3t__.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.Original(Rots[i]);
            }

            return accum;
        }

        [Benchmark]
        public __v3t__ CopySign()
        {
            __v3t__ accum = __v3t__.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.CopySign(Rots[i]);
            }

            return accum;
        }
    }

    #endregion

    //# }
}
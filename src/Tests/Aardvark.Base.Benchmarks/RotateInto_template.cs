using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var rt in Meta.RealTypes) {
    //#     var isDouble = rt == Meta.DoubleType;
    //#     var rtype = rt.Name;
    //#     var Rtype = rt.Caps;
    //#     var fc = rt.Char;
    //#     var rot3t = "Rot3" + fc;
    //#     var quatt = "Quaternion" + fc.ToUpper();
    //#     var v3t = Meta.VecTypeOf(3, rt).Name;
    //#     var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#     var piHalf = isDouble ? "Constant.PiHalf" : "ConstantF.PiHalf";
    //#     var eps = isDouble ? "1e-12" : "1e-3f";
    #region __rot3t__

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class RotateInto__Rtype__
    {
        private static class Implementations
        {
            public static __rot3t__ Original(__v3t__ from, __v3t__ into)
            {
                var angle = from.AngleBetween(into);

                //# var rotIntoEps = isDouble ? "1e-15" : "1e-6f";
                if (angle < __rotIntoEps__)
                    return __rot3t__.Identity;
                else if (__pi__ - angle < __rotIntoEps__)
                    return new __rot3t__(0, from.AxisAlignedNormal());
                else
                {
                    __v3t__ axis = Vec.Cross(from, into).Normalized;
                    return __rot3t__.Rotation(axis, angle);
                }
            }

            // https://stackoverflow.com/questions/1171849/finding-quaternion-representing-the-rotation-from-one-vector-to-another
            public static __rot3t__ HalfWayVec(__v3t__ from, __v3t__ into)
            {
                if (from.Dot(into).ApproximateEquals(-1))
                    return new __rot3t__(0, from.AxisAlignedNormal());
                else
                {
                    __v3t__ half = Vec.Normalized(from + into);
                    __quatt__ q = new __quatt__(Vec.Dot(from, half), Vec.Cross(from, half));
                    return new __rot3t__(q.Normalized);
                }
            }

            public static __rot3t__ HalfWayQuat(__v3t__ from, __v3t__ into)
            {
                var d = Vec.Dot(from, into);

                if (d.ApproximateEquals(-1))
                    return new __rot3t__(0, from.AxisAlignedNormal());
                else
                {
                    __quatt__ q = new __quatt__(d + 1, Vec.Cross(from, into));
                    return new __rot3t__(q.Normalized);
                }
            }

            public static Dictionary<string, Func<__v3t__, __v3t__, __rot3t__>> All
            {
                get => new Dictionary<string, Func<__v3t__, __v3t__, __rot3t__>>()
                {
                    { "Original", Original },
                    { "Half-way vector", HalfWayVec },
                    { "Half-way quaternion", HalfWayQuat }
                };
            }
        }

        const int count = 1000000;
        readonly __v3t__[] A = new __v3t__[count];
        readonly __v3t__[] B = new __v3t__[count];

        private static __v3t__ RndAxis(RandomSystem rnd)
            => rnd.Uniform__v3t__Direction();

        public RotateInto__Rtype__()
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
                    // Some vectors will not normalize to 1.0 -> provoke numerical issues in __v3t__
                    var v = Vec.Normalized(A[i] * rnd.Uniform__Rtype__());

                    // Add some small error
                    if (rnd.UniformDouble() < 0.5)
                    {
                        //# var err = isDouble ? "1e-22" : "1e-12f";
                        var eps = rnd.Uniform__v3t__() * (i / 100) * __err__;
                        v = Vec.Normalized(v + eps);
                    }

                    // Flip
                    if (rnd.UniformDouble() < 0.5)
                        v = -v;

                    return v;
                }

                return __v3t__.Zero;
            });
        }

        private double[] ComputeAbsoluteError(Func<__v3t__, __v3t__, __rot3t__> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                __v3t__ from = A[i];
                __v3t__ into = B[i];

                __rot3t__ r = f(from, into);
                __v3t__ res = r * from;

                error[i] = Vec.AngleBetween(new V3d(res), new V3d(into));
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            Console.WriteLine("Benchmarking numerical stability for __rot3t__.RotateInto() variants");
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
                    __v3t__ from = A[i];
                    __v3t__ into = B[i];

                    __rot3t__ r = f.Value(from, into);
                    __v3t__ res = r * from;

                    Assert.Less(
                        res.AngleBetween(into), __eps__,
                        string.Format("{0} implementation incorrect!", f.Key)
                    );
                }
            }
        }

        [Benchmark]
        public __rot3t__ Original()
        {
            __rot3t__ accum = __rot3t__.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.Original(A[i], B[i]);
            }

            return accum;
        }

        [Benchmark]
        public __rot3t__ HalfWayVec()
        {
            __rot3t__ accum = __rot3t__.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.HalfWayVec(A[i], B[i]);
            }

            return accum;
        }

        [Benchmark]
        public __rot3t__ HalfWayQuat()
        {
            __rot3t__ accum = __rot3t__.Identity;

            for (int i = 0; i < count; i++)
            {
                accum *= Implementations.HalfWayQuat(A[i], B[i]);
            }

            return accum;
        }
    }

    #endregion

    //# }
}
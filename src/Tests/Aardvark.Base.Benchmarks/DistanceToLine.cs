using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.1110 (2004/?/20H1)
    //Intel Core i7-4790K CPU 4.00GHz(Haswell), 1 CPU, 8 logical and 4 physical cores
    //.NET Core SDK = 5.0.302

    // [Host]        : .NET Core 3.1.17 (CoreCLR 4.700.21.31506, CoreFX 4.700.21.31502), X64 RyuJIT
    // .NET Core 3.1 : .NET Core 3.1.17 (CoreCLR 4.700.21.31506, CoreFX 4.700.21.31502), X64 RyuJIT

    //Job=.NET Core 3.1  Runtime=.NET Core 3.1  

    //|                Method |     Mean |     Error |    StdDev | Code Size |
    //|---------------------- |---------:|----------:|----------:|----------:|
    //|    DistanceToLine_Opt | 1.014 ms | 0.0073 ms | 0.0064 ms |     599 B |
    //|        DistanceToLine | 1.061 ms | 0.0123 ms | 0.0115 ms |     606 B |
    //|  GetMinimalDistanceTo | 1.346 ms | 0.0093 ms | 0.0087 ms |     709 B |
    //|   DistanceToLineF_Opt | 1.200 ms | 0.0106 ms | 0.0100 ms |     600 B |
    //|       DistanceToLineF | 1.243 ms | 0.0069 ms | 0.0064 ms |     671 B |
    //| GetMinimalDistanceToF | 1.231 ms | 0.0099 ms | 0.0088 ms |     687 B |

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class DistanceToLineTest
    {
        private static class Impl
        {
            public static float DistanceToLineF_Opt(V3f query, V3f p0, V3f p1)
            {
                var p0p1 = p1 - p0;
                var p0q = query - p0;

                var t = Vec.Dot(p0q, p0p1);
                if (t <= 0) return p0q.Length;
                var denom = p0p1.LengthSquared;
                if (t >= denom) { return Vec.Distance(query, p1); }
                t /= denom;
                return (p0q - t * p0p1).Length;
            }

            public static float DistanceToLineF(V3f query, V3f p0, V3f p1)
            {
                var p0p1 = p1 - p0;
                var p0q = query - p0;

                var t = Vec.Dot(p0q, p0p1);
                if (t <= 0) return Vec.Distance(query, p0);
                var denom = p0p1.LengthSquared;
                if (t >= denom) return Vec.Distance(query, p1);
                t /= denom;
                return Vec.Distance(query, p0 + t * p0p1);
            }

            public static float GetMinimalDistanceToF(V3f point, V3f p0, V3f p1)
            {
                var a = point - p0;
                var u = p1 - p0;

                var lu2 = u.LengthSquared;
                var adu = Vec.Dot(a, u);

                if (adu > lu2)
                {
                    var acu2 = Vec.Cross(a, u).LengthSquared;
                    var s1 = (adu * adu - 2 * adu * lu2 + lu2 * lu2);

                    return Fun.Sqrt((acu2 + s1) / lu2);
                }
                else if (adu >= 0)
                {
                    var acu2 = Vec.Cross(a, u).LengthSquared;
                    return Fun.Sqrt(acu2 / lu2);
                }
                else
                {
                    return a.Length;
                }
            }

            public static double DistanceToLine_Opt(V3d query, V3d p0, V3d p1)
            {
                var p0p1 = p1 - p0;
                var p0q = query - p0;

                var t = Vec.Dot(p0q, p0p1);
                if (t <= 0) return p0q.Length;
                var denom = p0p1.LengthSquared;
                if (t >= denom) { return Vec.Distance(query, p1); }
                t /= denom;
                return (p0q - t * p0p1).Length;
            }

            public static double DistanceToLine(V3d query, V3d p0, V3d p1)
            {
                var p0p1 = p1 - p0;
                var p0q = query - p0;

                var t = Vec.Dot(p0q, p0p1);
                if (t <= 0) { return Vec.Distance(query, p0); }
                var denom = p0p1.LengthSquared;
                if (t >= denom) { return Vec.Distance(query, p1); }
                t /= denom;
                return Vec.Distance(query, p0 + t * p0p1);
            }

            public static double GetMinimalDistanceTo(V3d point, V3d p0, V3d p1)
            {
                var a = point - p0;
                var u = p1 - p0;

                var lu2 = u.LengthSquared;
                var adu = Vec.Dot(a, u);

                if (adu > lu2)
                {
                    var acu2 = Vec.Cross(a, u).LengthSquared;
                    var s1 = (adu * adu - 2 * adu * lu2 + lu2 * lu2);

                    return Fun.Sqrt((acu2 + s1) / lu2);
                }
                else if (adu >= 0)
                {
                    var acu2 = Vec.Cross(a, u).LengthSquared;
                    return Fun.Sqrt(acu2 / lu2);
                }
                else
                {
                    return a.Length;
                }
            }
        }

        const int count = 100000;
        readonly V3d[] x = new V3d[count];
        readonly V3d[] y = new V3d[count];
        readonly V3d[] z = new V3d[count];
        readonly V3f[] xf = new V3f[count];
        readonly V3f[] yf = new V3f[count];
        readonly V3f[] zf = new V3f[count];

        public DistanceToLineTest()
        {
            var rnd = new RandomSystem(0);
            x.SetByIndex(i => rnd.UniformV3d() * 1000);
            y.SetByIndex(i => rnd.UniformV3d() * 1000);
            z.SetByIndex(i => rnd.UniformV3d() * 1000);
            var rnd2 = new RandomSystem(0);
            xf.SetByIndex(i => rnd2.UniformV3f() * 1000);
            yf.SetByIndex(i => rnd2.UniformV3f() * 1000);
            zf.SetByIndex(i => rnd2.UniformV3f() * 1000);
        }

        [Benchmark]
        public double DistanceToLine_Opt()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Impl.DistanceToLine_Opt(x[i], y[i], z[i]);
            }

            return sum;
        }

        [Benchmark]
        public double DistanceToLine()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Impl.DistanceToLine(x[i], y[i], z[i]);
            }

            return sum;
        }

        [Benchmark]
        public double GetMinimalDistanceTo()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Impl.GetMinimalDistanceTo(x[i], y[i], z[i]);
            }

            return sum;
        }

        [Benchmark]
        public float DistanceToLineF_Opt()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Impl.DistanceToLineF_Opt(xf[i], yf[i], zf[i]);
            }

            return sum;
        }

        [Benchmark]
        public float DistanceToLineF()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Impl.DistanceToLineF(xf[i], yf[i], zf[i]);
            }

            return sum;
        }

        [Benchmark]
        public float GetMinimalDistanceToF()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Impl.GetMinimalDistanceToF(xf[i], yf[i], zf[i]);
            }

            return sum;
        }
    }
}

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class DistanceToLineTest
    {
        private static class Impl
        {
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

        public DistanceToLineTest()
        {
            var rnd = new RandomSystem(0);
            x.SetByIndex(i => rnd.UniformV3d() * 1000);
            y.SetByIndex(i => rnd.UniformV3d() * 1000);
            z.SetByIndex(i => rnd.UniformV3d() * 1000);
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
    }
}

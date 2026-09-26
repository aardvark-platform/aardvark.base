using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Runtime.CompilerServices;

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
    //#     var minExponent = isDouble ? "4" : "2";
    //#     var exponentCount = isDouble ? "9" : "5";
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class RotateInto__Rtype__
    {
        private const int Count = 256;
        private readonly __v3t__[] _from = new __v3t__[Count];
        private readonly __v3t__[] _into = new __v3t__[Count];

        [Params("Ordinary", "Parallel", "Opposite", "NearOpposite")]
        public string Workload { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var random = new RandomSystem(812731);
            for (int i = 0; i < Count; i++)
            {
                var from = random.Uniform__v3t__Direction();
                _from[i] = from;
                switch (Workload)
                {
                    case "Parallel": _into[i] = from; break;
                    case "Opposite": _into[i] = -from; break;
                    case "NearOpposite":
                        var deviation = (__rtype__)System.Math.Pow(10, -(__minExponent__ + i % __exponentCount__));
                        _into[i] = (-from + deviation * from.AxisAlignedNormal()).Normalized;
                        break;
                    default: _into[i] = random.Uniform__v3t__Direction(); break;
                }
            }
        }

        // Exact pre-change public kernel (9bf23a1d), with the same inlining attribute.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static __rot3t__ HalfWayQuat(__v3t__ from, __v3t__ into)
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

        [Benchmark(Baseline = true, OperationsPerInvoke = Count)]
        public __rtype__ HalfWayQuat()
        {
            __rtype__ checksum = 0;
            for (int i = 0; i < Count; i++)
            {
                var q = HalfWayQuat(_from[i], _into[i]);
                checksum += q.W + q.X + q.Y + q.Z;
            }
            return checksum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public __rtype__ PublicApi()
        {
            __rtype__ checksum = 0;
            for (int i = 0; i < Count; i++)
            {
                var q = __rot3t__.RotateInto(_from[i], _into[i]);
                checksum += q.W + q.X + q.Y + q.Z;
            }
            return checksum;
        }
    }
    //# }
}

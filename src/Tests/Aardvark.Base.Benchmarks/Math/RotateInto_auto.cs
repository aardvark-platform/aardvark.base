using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class RotateIntoFloat
    {
        private const int Count = 256;
        private readonly V3f[] _from = new V3f[Count];
        private readonly V3f[] _into = new V3f[Count];

        [Params("Ordinary", "Parallel", "Opposite", "NearOpposite")]
        public string Workload { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var random = new RandomSystem(812731);
            for (int i = 0; i < Count; i++)
            {
                var from = random.UniformV3fDirection();
                _from[i] = from;
                switch (Workload)
                {
                    case "Parallel": _into[i] = from; break;
                    case "Opposite": _into[i] = -from; break;
                    case "NearOpposite":
                        var deviation = (float)System.Math.Pow(10, -(2 + i % 5));
                        _into[i] = (-from + deviation * from.AxisAlignedNormal()).Normalized;
                        break;
                    default: _into[i] = random.UniformV3fDirection(); break;
                }
            }
        }

        // Exact pre-change public kernel (9bf23a1d), with the same inlining attribute.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Rot3f HalfWayQuat(V3f from, V3f into)
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

        [Benchmark(Baseline = true, OperationsPerInvoke = Count)]
        public float HalfWayQuat()
        {
            float checksum = 0;
            for (int i = 0; i < Count; i++)
            {
                var q = HalfWayQuat(_from[i], _into[i]);
                checksum += q.W + q.X + q.Y + q.Z;
            }
            return checksum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float PublicApi()
        {
            float checksum = 0;
            for (int i = 0; i < Count; i++)
            {
                var q = Rot3f.RotateInto(_from[i], _into[i]);
                checksum += q.W + q.X + q.Y + q.Z;
            }
            return checksum;
        }
    }
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class RotateIntoDouble
    {
        private const int Count = 256;
        private readonly V3d[] _from = new V3d[Count];
        private readonly V3d[] _into = new V3d[Count];

        [Params("Ordinary", "Parallel", "Opposite", "NearOpposite")]
        public string Workload { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var random = new RandomSystem(812731);
            for (int i = 0; i < Count; i++)
            {
                var from = random.UniformV3dDirection();
                _from[i] = from;
                switch (Workload)
                {
                    case "Parallel": _into[i] = from; break;
                    case "Opposite": _into[i] = -from; break;
                    case "NearOpposite":
                        var deviation = (double)System.Math.Pow(10, -(4 + i % 9));
                        _into[i] = (-from + deviation * from.AxisAlignedNormal()).Normalized;
                        break;
                    default: _into[i] = random.UniformV3dDirection(); break;
                }
            }
        }

        // Exact pre-change public kernel (9bf23a1d), with the same inlining attribute.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Rot3d HalfWayQuat(V3d from, V3d into)
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

        [Benchmark(Baseline = true, OperationsPerInvoke = Count)]
        public double HalfWayQuat()
        {
            double checksum = 0;
            for (int i = 0; i < Count; i++)
            {
                var q = HalfWayQuat(_from[i], _into[i]);
                checksum += q.W + q.X + q.Y + q.Z;
            }
            return checksum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public double PublicApi()
        {
            double checksum = 0;
            for (int i = 0; i < Count; i++)
            {
                var q = Rot3d.RotateInto(_from[i], _into[i]);
                checksum += q.W + q.X + q.Y + q.Z;
            }
            return checksum;
        }
    }
}

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;

namespace Aardvark.Base.Benchmarks
{
    //[SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class V4fLength
    {
        readonly V4f[] arr = new V4f[1000000];

        //[GlobalSetup]
        public V4fLength()
        {
            var rnd1 = new RandomSystem(1);
            arr.SetByIndex(i => new V4f(rnd1.UniformFloat(), rnd1.UniformFloat(), rnd1.UniformFloat(), rnd1.UniformFloat()));
        }

        [Benchmark]
        public float V4fLengthReference()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
            {
                var v = local[i];
                sum += MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W);
            }
            return sum;
        }

        [Benchmark]
        public float V4fLengthSse()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
            {
                sum += local[i].Length;
            }
            return sum;
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class V3fLength
    {
        readonly V3f[] arr = new V3f[1000000];

        //[GlobalSetup]
        public V3fLength()
        {
            var rnd1 = new RandomSystem(1);
            arr.SetByIndex(i => new V3f(rnd1.UniformFloat(), rnd1.UniformFloat(), rnd1.UniformFloat()));
        }

        [Benchmark]
        public float V3fLengthReference()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
            {
                var v = local[i];
                sum += MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            }
            return sum;
        }

        [Benchmark]
        public float V3fLengthSse()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
            {
                sum += local[i].Length;
            }
            return sum;
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class V2fLength
    {
        readonly V2f[] arr = new V2f[1000000];

        //[GlobalSetup]
        public V2fLength()
        {
            var rnd1 = new RandomSystem(1);
            arr.SetByIndex(i => new V2f(rnd1.UniformFloat(), rnd1.UniformFloat()));
        }

        [Benchmark]
        public float V2fLengthReference()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
            {
                var v = local[i];
                sum += MathF.Sqrt(v.X * v.X + v.Y * v.Y);
            }
            return sum;
        }

        [Benchmark]
        public float V2fLengthSse()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
            {
                sum += local[i].Length;
            }
            return sum;
        }
    }

    public class Tests
    {
        [Test]
        public void V2fLength()
        {
            var test = new V2fLength();
            Assert.IsTrue(test.V2fLengthReference() == test.V2fLengthSse());
        }

        [Test]
        public void V3fLength()
        {
            var test = new V3fLength();
            Assert.IsTrue(test.V3fLengthReference() == test.V3fLengthSse());
        }

        [Test]
        public void V4fLength()
        {
            var test = new V4fLength();
            Assert.IsTrue(test.V4fLengthReference() == test.V4fLengthSse());
        }
    }
}

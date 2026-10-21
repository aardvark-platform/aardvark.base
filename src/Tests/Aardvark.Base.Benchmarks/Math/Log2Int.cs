using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET Core SDK = 3.1.201

    // [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
    // Job-RRXOFS : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

    //Runtime=.NET Core 3.0

    //|                      Method |       Mean |    Error |   StdDev |
    //|---------------------------- |-----------:|---------:|---------:|
    //|      Log2Int_Float_FloatExp | 1,157.1 us |  8.47 us |  7.50 us |
    //|          Log2Int_Float_Log2 | 5,136.2 us | 14.32 us | 13.39 us |
    //|    Log2Int_Double_DoubleExp | 1,408.4 us | 10.12 us |  8.97 us |
    //|         Log2Int_Double_Log2 | 9,366.9 us | 44.45 us | 41.58 us |
    //|     Log2Int_Int_Bithack_Cmp | 6,088.0 us | 63.27 us | 56.08 us |
    //| Log2Int_Int_Bithack_Convert | 5,982.9 us |  9.49 us |  7.41 us |
    //|  Log2Int_Int_Bithack_Unsafe | 4,906.8 us | 22.28 us | 19.75 us |
    //|  Log2Int_Int_Bithack_Unroll | 4,894.8 us |  9.66 us |  8.56 us |
    //|        Log2Int_Int_Numerics |   687.1 us |  6.25 us |  5.85 us |
    //|    Log2Int_Int_NumericsUint |   686.7 us |  4.32 us |  3.83 us |
    //|        Log2Int_Int_FloatExp | 1,234.9 us | 12.14 us | 10.76 us |
    //|       Log2Int_Int_DoubleExp | 1,381.9 us |  3.47 us |  2.71 us |


    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class Log2Int
    {
        readonly int[] m_numbersInt = new int[1000000];
        readonly uint[] m_numbersUInt = new uint[1000000];
        readonly float[] m_numbersFloat = new float[1000000];
        readonly double[] m_numbersDouble = new double[1000000];

        public Log2Int()
        {
            var rnd1 = new Random(1);
            m_numbersInt.SetByIndex(i => rnd1.Next());
            m_numbersUInt.SetByIndex(i => (uint)rnd1.Next());
            m_numbersFloat.SetByIndex(i => (float)rnd1.NextDouble());
            m_numbersDouble.SetByIndex(i => rnd1.NextDouble());
        }

        [Benchmark]
        public int Log2Int_Float_FloatExp()
        {
            var sum = 0;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log2Int(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Float_Log2()
        {
            var sum = 0;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += (int)Fun.Log2(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Double_DoubleExp()
        {
            var sum = 0;
            var local = m_numbersDouble;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log2Int(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Double_Log2()
        {
            var sum = 0;
            var local = m_numbersDouble;
            for (int i = 0; i < local.Length; i++)
                sum += (int)Fun.Log2(local[i]);
            return sum;
        }

        static int Log2Int_Bithack_Convert(int v)
        {
            var r = Convert.ToInt32(v > 0xFFFF) << 4; v >>= r;
            var shift = Convert.ToInt32(v > 0xFF) << 3; v >>= shift; r |= shift;
            shift = Convert.ToInt32(v > 0xF) << 2; v >>= shift; r |= shift;
            shift = Convert.ToInt32(v > 0x3) << 1; v >>= shift; r |= shift;
            return r |= (v >> 1);
        }

        static int Log2Int_Bithack_Cmp(int v)
        {
            var r = (v > 0xFFFF ? 1 : 0) << 4; v >>= r;
            var shift = (v > 0xFF ? 1 : 0) << 3; v >>= shift; r |= shift;
            shift = (v > 0xF ? 1 : 0) << 2; v >>= shift; r |= shift;
            shift = (v > 0x3 ? 1 : 0) << 1; v >>= shift; r |= shift;
            return r |= (v >> 1);
        }

        static unsafe int convert(bool input)
        {
            return (int)*((byte*)(&input));
        }

        static unsafe int Log2Int_Bithack_Unsafe(int v)
        {
            var r = convert(v > 0xFFFF) << 4; v >>= r;
            var shift = convert(v > 0xFF) << 3; v >>= shift; r |= shift;
            shift = convert(v > 0xF) << 2; v >>= shift; r |= shift;
            shift = convert(v > 0x3) << 1; v >>= shift; r |= shift;
            return r |= (v >> 1);
        }

        static unsafe int Log2Int_Bithack_Unroll(int v)
        {
            var r = 0;
            if ((v & 0x7FFF0000) != 0) { v >>= 16; r |= 16; }
            if ((v & 0xFF00) != 0) { v >>= 8; r |= 8; }
            if ((v & 0xF0) != 0) { v >>= 4; r |= 4; }
            if ((v & 0xC) != 0) { v >>= 2; r |= 2; }
            if ((v & 0x2) != 0) { v >>= 1; r |= 1; }
            return r;
        }

        [Benchmark]
        public int Log2Int_Int_Bithack_Cmp()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += Log2Int_Bithack_Cmp(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Int_Bithack_Convert()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += Log2Int_Bithack_Convert(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Int_Bithack_Unsafe()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += Log2Int_Bithack_Unsafe(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Int_Bithack_Unroll()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += Log2Int_Bithack_Unsafe(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Int_Numerics()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += System.Numerics.BitOperations.Log2((uint)local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Int_NumericsUint()
        {
            var sum = 0;
            var local = m_numbersUInt;
            for (int i = 0; i < local.Length; i++)
                sum += System.Numerics.BitOperations.Log2(local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Int_FloatExp()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log2Int((float)local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Int_DoubleExp()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log2Int((double)local[i]);
            return sum;
        }

        [Benchmark]
        public int Log2Int_Baseline()
        {
            var sum = 0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += (int)Fun.Log2(local[i]).Floor();
            return sum;
        }

        [Test]
        public void Log2Int_AardvarkFloatExp_Floor()
        {
            for(int i = 1; i < 1000; i++)
            {
                var a = Fun.Log2Int((float)i); // aardvark bithack
                var b = (int)Math.Log2(i).Floor();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2Int_AardvarkFloatExp_Floor_Frac()
        {
            for (int i = 1; i < 1000; i++)
            {
                var v = i / 1000f;
                var a = Fun.Log2Int(v); // aardvark bithack
                var b = (int)Math.Log2(v).Floor();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2Int_AardvarkDoubleExp_Floor()
        {
            for(int i = 1; i < 1000; i++)
            {
                var a = Fun.Log2Int((double)i); // aardvark bithack
                var b = (int)Math.Log2(i).Floor();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2Int_AardvarkDoubleExp_Floor_Frac()
        {
            for (int i = 1; i < 1000; i++)
            {
                var v = i / 1000.0;
                var a = Fun.Log2Int(v); // aardvark bithack
                var b = (int)Math.Log2(v).Floor();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2Int_NetCore()
        {
            for (int i = 1; i < 1000; i++)
            {
                var a = System.Numerics.BitOperations.Log2((uint)i);
                var b = (int)Math.Log2(i).Floor();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2Int_SoftwareConvert()
        {
            for (int i = 1; i < 1000; i++)
            {
                var a = Log2Int_Bithack_Unsafe(i);
                var b = (int)Math.Log2(i).Floor();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2Int_SoftwareCompare()
        {
            for (int i = 1; i < 1000; i++)
            {
                var a = Log2Int_Bithack_Cmp(i);
                var b = (int)Math.Log2(i).Floor();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2CeilingInt_AardvarkFloatExp_Ceiling()
        {
            for (int i = 1; i < 1000; i++)
            {
                var a = Fun.Log2CeilingInt((float)i); // aardvark bithack
                var b = (int)Math.Log2(i).Ceiling();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2CeilingInt_AardvarkFloatExp_Ceiling_Frac()
        {
            for (int i = 1; i < 1000; i++)
            {
                var v = i / 1000f;
                var a = Fun.Log2CeilingInt(v); // aardvark bithack
                var b = (int)Math.Log2(v).Ceiling();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2CeilingInt_AardvarkDoubleExp_Ceiling()
        {
            for (int i = 1; i < 1000; i++)
            {
                var a = Fun.Log2CeilingInt((double)i); // aardvark bithack
                var b = (int)Math.Log2(i).Ceiling();
                Assert.AreEqual(a, b);
            }
        }

        [Test]
        public void Log2CeilingInt_AardvarkDoubleExp_Ceiling_Frac()
        {
            for (int i = 1; i < 1000; i++)
            {
                var v = i / 1000.0;
                var a = Fun.Log2CeilingInt(v); // aardvark bithack
                var b = (int)Math.Log2(v).Ceiling();
                Assert.AreEqual(a, b);
            }
        }
    }

    //BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET Core SDK = 3.1.201

    // [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
    // Job-AVRUGN : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

    //Runtime=.NET Core 3.0

    //|                 Method |      Mean |     Error |    StdDev |
    //|----------------------- |----------:|----------:|----------:|
    //|    Log2_Int_MathDouble |  9.465 ms | 0.1865 ms | 0.1831 ms |
    //|   Log2_Float_MathFLog2 | 25.313 ms | 0.0338 ms | 0.0264 ms |
    //|    Log2_Float_MathFLog |  4.945 ms | 0.0338 ms | 0.0316 ms |
    //|      Log2_Float_Double | 26.679 ms | 0.2531 ms | 0.2367 ms |
    //| Log10_Float_MathFLog10 |  5.135 ms | 0.0388 ms | 0.0344 ms |
    //|        Log10_Float_Log |  5.032 ms | 0.0326 ms | 0.0305 ms |
    //|            Log10_Float |  5.156 ms | 0.0634 ms | 0.0529 ms |
    //|            Log2_Double |  9.018 ms | 0.0563 ms | 0.0527 ms |


    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class Log2
    {
        readonly int[] m_numbersInt = new int[1000000];
        readonly float[] m_numbersFloat = new float[1000000];
        readonly double[] m_numbersDouble = new double[1000000];

        public Log2()
        {
            var rnd1 = new Random(1);
            m_numbersInt.SetByIndex(i => rnd1.Next());
            m_numbersFloat.SetByIndex(i => (float)rnd1.NextDouble());
            m_numbersDouble.SetByIndex(i => rnd1.NextDouble());
        }

        [Benchmark]
        public double Log2_Int_MathDouble()
        {
            var sum = 0.0;
            var local = m_numbersInt;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log2(local[i]);
            return sum;
        }

        [Benchmark]
        public float Log2_Float_MathFLog2()
        {
            var sum = 0.0f;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += MathF.Log2(local[i]); // MathF.Log2 is super slow !?!? looks like it is using double conversion internally
            return sum;
        }

        [Benchmark]
        public float Log2_Float_MathFLog()
        {
            var sum = 0.0f;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += MathF.Log(local[i]) * (float)Constant.Ln2Inv;
            return sum;
        }

        [Benchmark]
        public float Log2_Float_Double()
        {
            var sum = 0.0f;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += (float)Math.Log2((double)local[i]);
            return sum;
        }

        [Benchmark]
        public float Log10_Float_MathFLog10()
        {
            var sum = 0.0f;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log10(local[i]);
            return sum;
        }

        const float InvLog10 = 2.30258509f;

        [Benchmark]
        public float Log10_Float_Log()
        {
            var sum = 0.0f;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log(local[i]) * InvLog10;
            return sum;
        }

        [Benchmark]
        public float Log10_Float()
        {
            var sum = 0.0f;
            var local = m_numbersFloat;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log10(local[i]);
            return sum;
        }

        [Benchmark]
        public double Log2_Double()
        {
            var sum = 0.0;
            var local = m_numbersDouble;
            for (int i = 0; i < local.Length; i++)
                sum += Fun.Log2(local[i]);
            return sum;
        }
    }

    [MemoryDiagnoser]
    public class FloatingLog2Int
    {
        public const int Count = 4096;
        readonly float[] m_float = new float[Count];
        readonly double[] m_double = new double[Count];
        readonly V4f[] m_vectorFloat = new V4f[Count];
        readonly V4d[] m_vectorDouble = new V4d[Count];

        [Params(false, true)]
        public bool Subnormal { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(41973);
            var bytes = new byte[8];
            for (int i = 0; i < Count; i++)
            {
                random.NextBytes(bytes);
                uint f = BitConverter.ToUInt32(bytes, 0) & 0x007fffff;
                ulong d = BitConverter.ToUInt64(bytes, 0) & 0x000fffffffffffff;
                if (Subnormal) { if (f == 0) f = 1; if (d == 0) d = 1; }
                else { f |= (uint)random.Next(1, 255) << 23; d |= (ulong)random.Next(1, 2047) << 52; }
                m_float[i] = BitConverter.Int32BitsToSingle((int)f);
                m_double[i] = BitConverter.Int64BitsToDouble((long)d);
            }
            for (int i = 0; i < Count; i++)
            {
                m_vectorFloat[i] = new V4f(m_float[i], m_float[(i + 17) % Count], m_float[(i + 31) % Count], m_float[(i + 47) % Count]);
                m_vectorDouble[i] = new V4d(m_double[i], m_double[(i + 17) % Count], m_double[(i + 31) % Count], m_double[(i + 47) % Count]);
            }
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public int FloatFloor()
        {
            int sum = 0;
            for (int i = 0; i < Count; i++) sum += Fun.Log2Int(m_float[i]);
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public int FloatCeiling()
        {
            int sum = 0;
            for (int i = 0; i < Count; i++) sum += Fun.Log2CeilingInt(m_float[i]);
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public int DoubleFloor()
        {
            int sum = 0;
            for (int i = 0; i < Count; i++) sum += Fun.Log2Int(m_double[i]);
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public int DoubleCeiling()
        {
            int sum = 0;
            for (int i = 0; i < Count; i++) sum += Fun.Log2CeilingInt(m_double[i]);
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public int VectorFloatFloor()
        {
            int sum = 0;
            for (int i = 0; i < Count; i++)
            {
                var value = Fun.Log2Int(m_vectorFloat[i]);
                sum += value.X + value.Y + value.Z + value.W;
            }
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public int VectorDoubleFloor()
        {
            int sum = 0;
            for (int i = 0; i < Count; i++)
            {
                var value = Fun.Log2Int(m_vectorDouble[i]);
                sum += value.X + value.Y + value.Z + value.W;
            }
            return sum;
        }
    }
}

using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET Core SDK = 5.0.100

    // [Host]     : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
    // DefaultJob : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT

    //|                       Method |     Mean |   Error |   StdDev |    Gen 0 | Gen 1 | Gen 2 | Allocated |
    //|----------------------------- |---------:|--------:|---------:|---------:|------:|------:|----------:|
    //|                StopWatchTime | 347.2 us | 6.90 us | 13.47 us | 127.4414 |     - |     - |  800000 B |
    //|     StopWatchTime (No Alloc) | 258.1 us | 5.10 us |  9.70 us |  31.7383 |     - |     - |  200000 B |
    //|                      CPUTime | 267.2 us | 5.84 us |  6.25 us |  76.1719 |     - |     - | 468.75 KB |
    //|           CPUTime (No Alloc) | 197.6 us | 3.52 us |  2.94 us |        - |     - |     - |         - |
    //|                    WallClock | 308.3 us | 4.79 us |  4.48 us |  69.8242 |     - |     - |  440000 B |
    //|         WallClock (No Alloc) | 243.4 us | 2.94 us |  2.60 us |        - |     - |     - |       1 B |
    //|                  CpuTimeUser | 266.9 us | 1.49 us |  1.40 us |  50.7813 |     - |     - |  320001 B |
    //|       CpuTimeUser (No Alloc) | 234.4 us | 2.73 us |  2.28 us |        - |     - |     - |         - |
    //|            CpuTimePrivileged | 279.9 us | 4.85 us |  4.05 us |  50.7813 |     - |     - |  320001 B |
    //| CpuTimePrivileged (No Alloc) | 226.9 us | 5.45 us |  9.96 us |        - |     - |     - |         - |

    [MemoryDiagnoser, PlainExporter]
    public class TelemetryProbesBenchmark
    {
        readonly Telemetry.StopWatchTime StopWatchProbe = new Telemetry.StopWatchTime();
        readonly Telemetry.CpuTime CPUProbe = new Telemetry.CpuTime();
        readonly Telemetry.WallClockTime WallProbe = new Telemetry.WallClockTime();
        readonly Telemetry.CpuTimeUser CPUUserProbe = new Telemetry.CpuTimeUser();
        readonly Telemetry.CpuTimePrivileged CPUPrivilegedProbe = new Telemetry.CpuTimePrivileged();

        readonly double[] m_data = new double[10000].SetByIndex(i => i + 1);

        [Benchmark]
        public int StopWatchTime()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 2;)
                using (StopWatchProbe.Timer)
                    sum += HashCode.Combine(m_data[i++].GetHashCode(), m_data[i++].GetHashCode());
            return sum;
        }

        [Benchmark]
        public int CPUTime()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 2;)
                using (CPUProbe.Timer)
                    sum += HashCode.Combine(m_data[i++].GetHashCode(), m_data[i++].GetHashCode());
            return sum;
        }

        [Benchmark]
        public int WallClock()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 2;)
                using (WallProbe.Timer)
                    sum += HashCode.Combine(m_data[i++].GetHashCode(), m_data[i++].GetHashCode());
            return sum;
        }

        [Benchmark]
        public int CpuTimeUser()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 2;)
                using (CPUUserProbe.Timer)
                    sum += HashCode.Combine(m_data[i++].GetHashCode(), m_data[i++].GetHashCode());
            return sum;
        }

        [Benchmark]
        public int CpuTimePrivileged()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 2;)
                using (CPUPrivilegedProbe.Timer)
                    sum += HashCode.Combine(m_data[i++].GetHashCode(), m_data[i++].GetHashCode());
            return sum;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var wide in new[] { false, true }) {
    //# var scalar = wide ? "long" : "int";
    //# var vector = wide ? "V2l" : "V2i";
    //# var bits = wide ? "64" : "32";
    //# var large = wide ? "6000000000000000000L" : "1500000000";
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class BresenhamLineIteratorInt__bits__
    {
        private const int Count = 32;
        private readonly __vector__[] _start = new __vector__[Count];
        private readonly __vector__[] _end = new __vector__[Count];
        private int _limit;

        [Params("Singleton", "ShortHorizontal", "LongHorizontal", "ShortVertical", "LongVertical",
            "ShortDiagonal", "LongDiagonal", "ShortShallow", "LongShallow", "ShortSteep", "LongSteep",
            "ShortHalfTie", "LongHalfTie", "ExtremeFull", "ExtremePositive", "ExtremeDiagonal", "ExtremeHalfTie")]
        public string Workload { get; set; }

        // Diagnostic counts, outside timing. Extreme baselines can terminate prematurely.
        public int BaselinePoints { get; private set; }
        public int CurrentPoints { get; private set; }
        public bool BaselineMatches { get; private set; }

        [GlobalSetup(Target = nameof(Baseline))]
        public void SetupBaseline()
        {
            Setup();
            Warmup(true);
        }

        [GlobalSetup(Target = nameof(PublicApi))]
        public void SetupPublicApi()
        {
            Setup();
            Warmup(false);
        }

        private static long s_checksum;

        // Warm the selected subject before pilot sizing, so tiered compilation cannot
        // shrink the requested iterations into short, misleading measurements.
        private void Warmup(bool baseline)
        {
            long start = Stopwatch.GetTimestamp();
            do
                s_checksum = baseline ? Baseline() : PublicApi();
            while (Stopwatch.GetTimestamp() - start < 2 * Stopwatch.Frequency);
        }

        public void Setup()
        {
            _limit = Workload.StartsWith("Extreme", StringComparison.Ordinal) ? 64 : int.MaxValue;
            int length = Workload.StartsWith("Long", StringComparison.Ordinal) ? 4096 : 16;
            for (int i = 0; i < Count; i++)
            {
                __scalar__ x = length, y = length / 3;
                if (Workload.EndsWith("Horizontal", StringComparison.Ordinal)) y = 0;
                else if (Workload.EndsWith("Vertical", StringComparison.Ordinal)) { x = 0; y = length; }
                else if (Workload.EndsWith("Diagonal", StringComparison.Ordinal)) y = length;
                else if (Workload.EndsWith("Steep", StringComparison.Ordinal)) { x = length / 3; y = length; }
                else if (Workload.EndsWith("HalfTie", StringComparison.Ordinal)) y = length / 2;
                else if (Workload == "Singleton") { x = 0; y = 0; }
                if ((i & 1) != 0) x = -x;
                if ((i & 2) != 0) y = -y;
                var start = new __vector__(i * 7 - 112, i * 11 - 176);
                var end = start + new __vector__(x, y);
                switch (Workload)
                {
                    case "ExtremeFull":
                        start = new __vector__(__scalar__.MinValue + i, 0);
                        end = new __vector__(__scalar__.MaxValue - i, 1 + i % 7);
                        break;
                    case "ExtremePositive":
                        start = __vector__.Zero;
                        end = new __vector__(__large__, __large__ / 15);
                        if ((i & 1) != 0) end.X = -end.X;
                        if ((i & 2) != 0) end.Y = -end.Y;
                        break;
                    case "ExtremeDiagonal":
                        start = new __vector__(__scalar__.MinValue + i, __scalar__.MinValue + i);
                        end = new __vector__(__scalar__.MaxValue - i, __scalar__.MaxValue - i);
                        break;
                    case "ExtremeHalfTie":
                        start = new __vector__(__scalar__.MinValue + i, 0);
                        end = new __vector__(__scalar__.MaxValue - 1 - i, __scalar__.MaxValue - i);
                        break;
                }
                if ((i & 4) != 0) (start, end) = (end, start);
                _start[i] = start;
                _end[i] = end;
            }

            BaselinePoints = 0;
            CurrentPoints = 0;
            BaselineMatches = true;
            for (int i = 0; i < Count; i++)
            {
                using var before = Previous(_start[i], _end[i]).GetEnumerator();
                using var after = GeometryFun.BresenhamLineIterator(_start[i], _end[i]).GetEnumerator();
                for (int k = 0; k < _limit; k++)
                {
                    bool a = before.MoveNext(), b = after.MoveNext();
                    if (a) BaselinePoints++;
                    if (b) CurrentPoints++;
                    if (a != b || (a && before.Current != after.Current)) BaselineMatches = false;
                    if (!a && !b) break;
                }
            }
        }

        // Both subjects use this same consumer and IEnumerable dispatch. No output materialization.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static long Consume(IEnumerable<__vector__> sequence, int limit)
        {
            long checksum = 0;
            using var iterator = sequence.GetEnumerator();
            for (int k = 0; k < limit && iterator.MoveNext(); k++)
            {
                var p = iterator.Current;
                checksum = unchecked(checksum + p.X + p.Y);
            }
            return checksum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count)]
        public long Baseline()
        {
            long checksum = 0;
            for (int i = 0; i < Count; i++)
                checksum = unchecked(checksum + Consume(Previous(_start[i], _end[i]), _limit));
            return checksum;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public long PublicApi()
        {
            long checksum = 0;
            for (int i = 0; i < Count; i++)
                checksum = unchecked(checksum + Consume(GeometryFun.BresenhamLineIterator(_start[i], _end[i]), _limit));
            return checksum;
        }

        // Exact pre-change iterator from cc2c3ebd (including its overflow defects).
        private static IEnumerable<__vector__> Previous(__vector__ p0, __vector__ p1)
        {
            __scalar__ x0 = p0.X, y0 = p0.Y, x1 = p1.X, y1 = p1.Y;
            __scalar__ dx, dy;
            __scalar__ incx, incy;
            __scalar__ balance;

            if (x1 >= x0)
            {
                dx = x1 - x0;
                incx = 1;
            }
            else
            {
                dx = x0 - x1;
                incx = -1;
            }

            if (y1 >= y0)
            {
                dy = y1 - y0;
                incy = 1;
            }
            else
            {
                dy = y0 - y1;
                incy = -1;
            }

            if (dx >= dy)
            {
                dy <<= 1;
                balance = dy - dx;
                dx <<= 1;

                while (x0 != x1)
                {
                    yield return new __vector__(x0, y0);
                    if (balance >= 0)
                    {
                        y0 += incy;
                        balance -= dx;
                    }
                    balance += dy;
                    x0 += incx;
                }

                yield return new __vector__(x0, y0);
            }
            else
            {
                dx <<= 1;
                balance = dx - dy;
                dy <<= 1;

                while (y0 != y1)
                {
                    yield return new __vector__(x0, y0);
                    if (balance >= 0)
                    {
                        x0 += incx;
                        balance -= dy;
                    }
                    balance += dx;
                    y0 += incy;
                }

                yield return new __vector__(x0, y0);
            }
        }
    }
    //# }
}

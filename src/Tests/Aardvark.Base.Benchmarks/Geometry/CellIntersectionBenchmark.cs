using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*CellIntersectionBenchmark*'
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class CellIntersectionBenchmark
    {
        private const int PairCount = 1024;
        private Cell[] _a3, _b3;
        private Cell2d[] _a2, _b2;

        [Params("Equal", "Overlapping", "Disjoint", "Centered", "MixedScale")]
        public string Case { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _a3 = new Cell[PairCount];
            _b3 = new Cell[PairCount];
            _a2 = new Cell2d[PairCount];
            _b2 = new Cell2d[PairCount];
            var random = new Random(731);
            for (int i = 0; i < PairCount; i++)
            {
                int exponent = random.Next(-8, 9);
                var a = new Cell(random.Next(-16, 17), random.Next(-16, 17), random.Next(-16, 17), exponent);
                Cell b;
                switch (Case)
                {
                    case "Equal":
                        if (i % 4 == 0) a = new Cell(exponent);
                        b = a;
                        break;
                    case "Overlapping":
                        int gap = random.Next(1, 11);
                        int scale = 1 << gap;
                        b = new Cell(a.X * scale + random.Next(scale), a.Y * scale + random.Next(scale),
                            a.Z * scale + random.Next(scale), exponent - gap);
                        break;
                    case "Disjoint":
                        b = i % 2 == 0 ? new Cell(a.X + 2, a.Y, a.Z, exponent) : new Cell(a.X, a.Y + 2, a.Z, exponent);
                        break;
                    case "Centered":
                        a = new Cell(exponent);
                        b = (i % 4) switch
                        {
                            0 => new Cell(exponent + 1),
                            1 => new Cell(-1, 0, -1, exponent - 2),
                            2 => new Cell(0, -1, 0, exponent + 2),
                            _ => new Cell(2, 0, 0, exponent)
                        };
                        break;
                    case "MixedScale":
                        (a, b) = (i % 8) switch
                        {
                            0 => (new Cell(1L << 53, 0, 0, 0), new Cell(1L << 54, 0, 0, -1)),
                            1 => (new Cell(0, 0, 0, -1100), new Cell(0, 0, 0, -1101)),
                            2 => (new Cell(0, 0, 0, 1100), new Cell(2, 0, 0, 1100)),
                            3 => (new Cell(-1, 0, 0, 64), new Cell(long.MinValue, 0, 0, 0)),
                            4 => (new Cell(0, 0, 0, int.MaxValue), new Cell(long.MaxValue, 0, 0, int.MinValue)),
                            5 => (new Cell(-1100), new Cell(0, 0, 0, 1100)),
                            6 => (new Cell(-1, 0, 0, 65), new Cell(long.MaxValue, 0, 0, 0)),
                            _ => (new Cell(0, 0, 0, 10), new Cell(17, 33, 5, -8))
                        };
                        break;
                    default:
                        throw new InvalidOperationException("Unknown benchmark case.");
                }
                if ((i & 1) != 0) (a, b) = (b, a);
                _a3[i] = a;
                _b3[i] = b;
                _a2[i] = new Cell2d(a.X, a.Y, a.Exponent);
                _b2[i] = new Cell2d(b.X, b.Y, b.Exponent);
            }
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = PairCount)]
        [BenchmarkCategory("Cell")]
        public int PreviousCell()
        {
            int hits = 0;
            for (int i = 0; i < PairCount; i++) if (Previous(in _a3[i], _b3[i])) hits++;
            return hits;
        }

        [Benchmark(OperationsPerInvoke = PairCount)]
        [BenchmarkCategory("Cell")]
        public int IntegerCell()
        {
            int hits = 0;
            for (int i = 0; i < PairCount; i++) if (_a3[i].Intersects(_b3[i])) hits++;
            return hits;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = PairCount)]
        [BenchmarkCategory("Cell2d")]
        public int PreviousCell2d()
        {
            int hits = 0;
            for (int i = 0; i < PairCount; i++) if (Previous(in _a2[i], _b2[i])) hits++;
            return hits;
        }

        [Benchmark(OperationsPerInvoke = PairCount)]
        [BenchmarkCategory("Cell2d")]
        public int IntegerCell2d()
        {
            int hits = 0;
            for (int i = 0; i < PairCount; i++) if (_a2[i].Intersects(_b2[i])) hits++;
            return hits;
        }

        // Baseline from d1e00e27; in matches the readonly instance receiver.
        private static bool Previous(in Cell a, Cell b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.Exponent == b.Exponent) return true;
            return a.BoundingBox.Intersects(b.BoundingBox);
        }

        private static bool Previous(in Cell2d a, Cell2d b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Exponent == b.Exponent) return true;
            return a.BoundingBox.Intersects(b.BoundingBox);
        }
    }
}

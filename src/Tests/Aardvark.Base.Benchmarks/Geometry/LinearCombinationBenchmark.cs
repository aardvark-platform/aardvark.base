using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks.Geometry
{
    // dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*LinearCombinationBenchmark*'
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class LinearCombinationBenchmark
    {
        private const int Count = 256;
        private V3d[] _x, _single, _u, _v;
        private V3f[] _xf, _singlef, _uf, _vf;

        [Params("Hit", "Miss", "Dependent")]
        public string Case { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _x = new V3d[Count]; _single = new V3d[Count]; _u = new V3d[Count]; _v = new V3d[Count];
            _xf = new V3f[Count]; _singlef = new V3f[Count]; _uf = new V3f[Count]; _vf = new V3f[Count];
            for (int i = 0; i < Count; i++)
            {
                var u = new V3d(1 + i % 7, 2, 1);
                var v = new V3d(0, 1 + i % 7, 3);
                bool hit = Case == "Hit";
                if (Case == "Dependent")
                {
                    (u, v) = (i % 6) switch
                    {
                        0 => (u, 2 * u),
                        1 => (u, u),
                        2 => (u, -3 * u),
                        3 => (V3d.Zero, u),
                        4 => (u, V3d.Zero),
                        _ => (V3d.Zero, V3d.Zero)
                    };
                    hit = i / 6 % 2 == 0;
                }
                _u[i] = u; _v[i] = v;
                _x[i] = hit ? 2 * u - 3 * v : Case == "Dependent" ? V3d.ZAxis : 2 * u - 3 * v + u.Cross(v);
                _single[i] = hit ? 2 * u : V3d.ZAxis;
                _uf[i] = (V3f)u; _vf[i] = (V3f)v; _xf[i] = (V3f)_x[i]; _singlef[i] = (V3f)_single[i];
            }
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("DoubleSingle")]
        public int PreviousDoubleSingle()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (Previous(_single[i], _u[i])) hits++;
            return hits;
        }

        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("DoubleSingle")]
        public int ScalarDoubleSingle()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (_single[i].IsLinearCombinationOf(_u[i])) hits++;
            return hits;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("DoublePair")]
        public int PreviousDoublePair()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (Previous(_x[i], _u[i], _v[i])) hits++;
            return hits;
        }

        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("DoublePair")]
        public int ScalarDoublePair()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (_x[i].IsLinearCombinationOf(_u[i], _v[i])) hits++;
            return hits;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("DoubleCoefficients")]
        public double PreviousDoubleCoefficients()
        {
            double sum = 0;
            for (int i = 0; i < Count; i++)
                try { if (Previous(_x[i], _u[i], _v[i], out var a, out var b)) sum += a + b; }
                catch (ArgumentException) { sum--; }
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("DoubleCoefficients")]
        public double ScalarDoubleCoefficients()
        {
            double sum = 0;
            for (int i = 0; i < Count; i++)
                try { if (_x[i].IsLinearCombinationOf(_u[i], _v[i], out var a, out var b)) sum += a + b; }
                catch (ArgumentException) { sum--; }
            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("FloatSingle")]
        public int PreviousFloatSingle()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (Previous(_singlef[i], _uf[i])) hits++;
            return hits;
        }

        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("FloatSingle")]
        public int ScalarFloatSingle()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (_singlef[i].IsLinearCombinationOf(_uf[i])) hits++;
            return hits;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("FloatPair")]
        public int PreviousFloatPair()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (Previous(_xf[i], _uf[i], _vf[i])) hits++;
            return hits;
        }

        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("FloatPair")]
        public int ScalarFloatPair()
        {
            int hits = 0;
            for (int i = 0; i < Count; i++) if (_xf[i].IsLinearCombinationOf(_uf[i], _vf[i])) hits++;
            return hits;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Count), BenchmarkCategory("FloatCoefficients")]
        public float PreviousFloatCoefficients()
        {
            float sum = 0;
            for (int i = 0; i < Count; i++)
                try { if (Previous(_xf[i], _uf[i], _vf[i], out var a, out var b)) sum += a + b; }
                catch (ArgumentException) { sum--; }
            return sum;
        }

        [Benchmark(OperationsPerInvoke = Count), BenchmarkCategory("FloatCoefficients")]
        public float ScalarFloatCoefficients()
        {
            float sum = 0;
            for (int i = 0; i < Count; i++)
                try { if (_xf[i].IsLinearCombinationOf(_uf[i], _vf[i], out var a, out var b)) sum += a + b; }
                catch (ArgumentException) { sum--; }
            return sum;
        }

        // Method bodies at eaa8f342. Exception handling belongs to both timing adapters,
        // not to the baseline: dependent coefficient queries originally throw.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Previous(V3d x, V3d u) => x.IsParallelTo(u);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Previous(V3f x, V3f u) => x.IsParallelTo(u);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Previous(V3d x, V3d u, V3d v) => u.Cross(v).IsOrthogonalTo(x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Previous(V3f x, V3f u, V3f v) => u.Cross(v).IsOrthogonalTo(x);

        private static bool Previous(V3d x, V3d u, V3d v, out double a, out double b)
        {
            var n = u.Cross(v);
            var mat = new[,] { { u.X, v.X, n.X }, { u.Y, v.Y, n.Y }, { u.Z, v.Z, n.Z } };
            var rhs = new[] { x.X, x.Y, x.Z };
            var perm = mat.LuFactorize();
            var t = new V3d(mat.LuSolve(perm, rhs));
            if (Fun.IsTiny(t.Z)) { a = t.X; b = t.Y; return true; }
            a = b = double.NaN;
            return false;
        }

        private static bool Previous(V3f x, V3f u, V3f v, out float a, out float b)
        {
            var n = u.Cross(v);
            var mat = new[,] { { u.X, v.X, n.X }, { u.Y, v.Y, n.Y }, { u.Z, v.Z, n.Z } };
            var rhs = new[] { x.X, x.Y, x.Z };
            var perm = mat.LuFactorize();
            var t = new V3f(mat.LuSolve(perm, rhs));
            if (Fun.IsTiny(t.Z)) { a = t.X; b = t.Y; return true; }
            a = b = float.NaN;
            return false;
        }
    }
}

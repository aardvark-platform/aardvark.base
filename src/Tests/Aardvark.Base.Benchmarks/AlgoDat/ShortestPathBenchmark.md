# Shortest-path priority benchmarks

Tracking: [#131](https://github.com/aardvark-platform/aardvark.base/issues/131).

Run the retained fixture without changing the benchmark dispatcher:

```sh
dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- \
  --filter '*ShortestPathBenchmark*' --inProcess \
  --warmupCount 8 --iterationCount 30 --iterationTime 300
```

## Workloads and measurement boundaries

- `InsertDrain`: insert deterministic random keys, then extract every item.
- `InsertDecreaseDrain`: insert a sentinel and random keys, extract the sentinel to
  consolidate, decrease all remaining keys, then drain. Insertion and extraction
  are intentionally included; this is not an isolated decrease-key latency test.
- `SparseGraph` and `GridGraph`: calculate paths on fixed non-negative weighted
  graphs. These invoke the existing calculation body synchronously, including
  allocation of run state and result publication, but excluding scheduling and
  waiting. They measure calculation work, not asynchronous API latency.

Reflection and delegate compilation, input generation, graph construction, and
handle-array allocation occur only in global setup. The heap instance is reused
between invocations, so retained degree storage is warmed. Node allocations remain
inside the measured heap workloads. No reflection occurs in the timed methods.

## Baseline comparison

Baseline: unchanged library at `c79192f8`, with the identical benchmark fixture.
Both versions used Release, .NET 8.0.26, the same processor affinity, eight warmup
iterations and thirty 300-ms measurement iterations. Each version was measured
in two passes, with the order reversed for the second pair. Times below average
those two BenchmarkDotNet means. Throughput change is `baseline time / new time - 1`.
Allocated bytes are per complete invocation; heap workloads retain the same node
allocation count, but eliminate repeated consolidation scratch allocations.

| Workload | Count | Baseline (µs) | Revised (µs) | Throughput change | Baseline bytes | Revised bytes |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Insert/drain | 32 | 3.633 | 2.789 | +30.2% | 5,368 | 2,048 |
| Insert/decrease/drain | 32 | 3.459 | 3.493 | -1.0% | 5,328 | 2,112 |
| Sparse graph | 32 | 6.578 | 5.635 | +16.7% | 8,072 | 4,952 |
| Grid graph | 32 | 4.588 | 3.537 | +29.7% | 5,864 | 3,336 |
| Insert/drain | 4096 | 1296.112 | 1352.092 | **-4.1%** | 1,054,440 | 262,144 |
| Insert/decrease/drain | 4096 | 747.560 | 796.808 | **-6.2%** | 1,003,504 | 262,208 |
| Sparse graph | 4096 | 2215.660 | 2109.934 | +5.0% | 1,291,648 | 515,336 |
| Grid graph | 4096 | 1070.752 | 883.695 | +21.2% | 914,208 | 304,168 |

Allocation reductions range from 38.7% to 75.1%. Small-heap decrease/drain is within
measurement variation, but the larger heap-only regressions repeat across both
passes. **The no-throughput-regression acceptance gate is not met.**

An additional alternating, warmed comparison of the same fixture methods in one
process found neutral or improved large-heap results. That does not override the
regressions in the isolated BenchmarkDotNet workloads above; sharing GC pressure
between baseline and revised workloads changes the measurement environment.

The baseline is incorrect: these are matched inputs, not equivalent priority
results. Its early-stop consolidation can leave most roots unlinked, doing fewer
links and cuts. The correctness regressions and reference-model tests are separate
from timing; a checksum in the benchmark only consumes results and is not an
ordering oracle. Further optimization must preserve those correctness checks.

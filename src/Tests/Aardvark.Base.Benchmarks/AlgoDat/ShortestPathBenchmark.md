# Shortest-path priority benchmarks

Tracking: [#131](https://github.com/aardvark-platform/aardvark.base/issues/131).

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
between heap-workload invocations, so retained degree, node-registry, and free-index
storage is warmed. Node allocations remain inside the measured heap workloads.
Graph workloads create their heap inside the calculation and include all of its
storage. No reflection occurs in the timed methods.

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
| Insert/drain | 32 | 3.724 | 1.830 | +103.5% | 5,368 | 2,048 |
| Insert/decrease/drain | 32 | 3.582 | 2.350 | +52.4% | 5,328 | 2,112 |
| Sparse graph | 32 | 6.666 | 5.374 | +24.0% | 8,072 | 5,792 |
| Grid graph | 32 | 4.755 | 3.155 | +50.7% | 5,864 | 3,504 |
| Insert/drain | 4096 | 1312.584 | 1068.664 | +22.8% | 1,054,440 | 262,144 |
| Insert/decrease/drain | 4096 | 769.705 | 660.887 | +16.5% | 1,003,504 | 262,208 |
| Sparse graph | 4096 | 2277.810 | 2059.592 | +10.6% | 1,291,648 | 614,072 |
| Grid graph | 4096 | 1107.959 | 821.209 | +34.9% | 914,208 | 307,412 |

The baseline is incorrect: these are matched inputs, not equivalent priority
results. Its early-stop consolidation can leave most roots unlinked, doing fewer
links and cuts. The correctness regressions and reference-model tests are separate
from timing; a checksum in the benchmark only consumes results and is not an
ordering oracle. Further optimization must preserve those correctness checks.

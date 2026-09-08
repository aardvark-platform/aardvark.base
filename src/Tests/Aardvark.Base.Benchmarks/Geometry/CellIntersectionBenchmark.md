# Exact cell-intersection benchmarks

Tracking: [#133](https://github.com/aardvark-platform/aardvark.base/issues/133).

```sh
dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- \
  --filter '*CellIntersectionBenchmark*' \
  --warmupCount 8 --iterationCount 20 --iterationTime 1000 --buildTimeout 600
```

The fixture compares the previous `Intersects` bodies at `d1e00e27` with the
integer implementation. Baseline receivers are passed by readonly reference,
matching the original instance methods; `BoundingBox` is unchanged. Each
invocation processes the same 1,024 prebuilt pairs, alternating receiver order.
Input generation and array allocation are outside timing. Reported time and
allocations are per pair, not per batch.

- **Equal:** ordinary and centered self-intersections.
- **Overlapping:** ordinary parents and children at ordinary representable scales.
- **Disjoint:** separated ordinary cells at the same exponent.
- **Centered:** centered/centered and centered/ordinary hits and misses, including partial overlap.
- **MixedScale:** large coordinates, underflow, overflow, saturated shifts and full-int-range exponent differences.

Mixed-scale answers intentionally differ where the previous floating-point
implementation was incorrect. Correctness is checked independently by regression
and BigInteger interval-oracle tests, not by requiring the two benchmark hit
counts to agree. All benchmark pairs are valid cells; Invalid compatibility has
separate characterization tests.

## Results

Release, .NET 8.0.26, isolated BenchmarkDotNet processes on the same processor
affinity, eight warmup iterations and twenty one-second target iterations:

| Type | Case | Previous (ns) | Integer (ns) | Speedup | Allocated bytes, both |
| --- | --- | ---: | ---: | ---: | ---: |
| Cell | Equal | 1.452 | 1.411 | 1.03x | 0 |
| Cell | Overlapping | 26.532 | 3.576 | 7.42x | 0 |
| Cell | Disjoint | 24.624 | 2.875 | 8.56x | 0 |
| Cell | Centered | 24.941 | 4.802 | 5.19x | 0 |
| Cell | MixedScale | 24.828 | 3.652 | 6.80x | 0 |
| Cell2d | Equal | 1.191 | 1.138 | 1.05x | 0 |
| Cell2d | Overlapping | 24.951 | 3.282 | 7.60x | 0 |
| Cell2d | Disjoint | 23.343 | 3.553 | 6.57x | 0 |
| Cell2d | Centered | 24.824 | 4.292 | 5.78x | 0 |
| Cell2d | MixedScale | 24.150 | 3.464 | 6.97x | 0 |

Non-equal queries improve 5.2–8.6x. Equality retains its fast path without a
measured regression; its tiny differences should not be treated as substantial
speedups. Every measured workload remains allocation-free. The final isolated
run completed without BenchmarkDotNet warnings; shorter preliminary in-process
iterations were insufficient for reliable equality-path comparisons.

# Full-domain Bresenham iteration

Tracking: [#151](https://github.com/aardvark-platform/aardvark.base/issues/151).

**Performance acceptance is blocked.** Correctness and reduced allocations do not
authorize merging while ordinary workloads regress.

## Exact arithmetic

Ordered unsigned subtraction represents the exact coordinate distances across the
complete signed domain. Let `D` be the major distance and `M <= D` the minor one.
The complementary remainder starts at `ceil(D / 2)`, computed without overflowing
as `(D >> 1) + (D & 1)`.

For a nonzero span the remainder `r` stays in `[1, D]`. If `r <= M`, advance the
minor coordinate and set `r += D - M`; the sum is at most `D`. Otherwise subtract
`M`, leaving a positive remainder. Thus no doubled error, widened integer,
floating-point operation or arbitrary-precision value is needed. This produces
minor displacement `floor((k*M + floor(D/2)) / D)` at major step `k`, including
the existing forward-directed half-step tie rule. Signed coordinates move
monotonically between their endpoints and are never incremented beyond them.

The iterator retains scalar coordinates for the hot traversal and stores its
unit directions in signed bytes to keep the hoisted state compact. The last yield
is shared between both major-axis branches. There is no forwarding iterator or
output materialization.

## Correctness coverage

`BresenhamLineIteratorTests` contains 18 cases across both precisions. Eight cases
fail before the repair. Coverage includes premature termination at full signed
spans, the `(9,2)` rather than `(9,1)` prefix defect, reversed endpoints, axis
swaps, all slope classes, singleton lines, exact ties and translated short lines
near coordinate limits.

An independent BigInteger oracle rounds rational displacements directly, rather
than reproducing the production recurrence. It checks 6,561 exhaustive small
lines per precision and 2,000 seeded full-domain pairs per precision, each also
reversed and axis-swapped. Huge lines consume at most 64 points. Short lines
check complete cardinality, endpoint inclusion and 8-connectivity. Additional
checks cover lazy/independent/repeated enumeration and allocation budgets of
104 B (`V2i`) / 176 B (`V2l`), with no per-point allocation.

## Benchmark protocol

```sh
dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- \
  --filter '*BresenhamLineIteratorInt*' \
  --warmupCount 8 --iterationCount 16 --iterationTime 500 --buildTimeout 600 \
  --exporters json
```

The generated .NET 8 fixtures retain the exact pre-change iterator from
`cc2c3ebd` and call the actual corrected public APIs. Both subjects pass their
`IEnumerable` to the same non-inlined consumer and accumulate scalar coordinate
checksums. The consumer neither materializes output nor adds an iterator layer.
Each invocation traverses 32 precomputed directed lines; BenchmarkDotNet reports
latency and allocated bytes per line, not per invocation.

Workloads cover singleton, 16-step and 4,096-step lines, separately for horizontal,
vertical, diagonal, shallow, steep and exact half-slope classes. Sign reflections
and reversed endpoints are included in each ordinary batch. Four extreme-span
workloads consume only bounded 64-point prefixes. Setup records the actual
before/after point counts and whether the baseline sequence matches.

The selected subject is warmed for two seconds before pilot sizing, followed by
the matched BenchmarkDotNet warmup/measurement iterations. This avoids sizing
iterations from tier-zero execution and then accidentally measuring short
iterations or tier transitions. Input preparation and correctness diagnostics
remain outside timing, and both kernels use the same warmup policy.

Throughput must use the arithmetic mean: `1000 / mean_ns` gives millions of lines
per second; multiply by the actual points per line for millions of emitted points
per second. Allocated bytes are reported separately, without subtracting iterator
or consumer overhead.

### Non-equivalent extreme baselines

All four extreme workloads contain incorrect baseline outputs. In particular,
`ExtremeFull` prematurely emits only 154 points across 32 iterators (4.8125 per
line), versus the requested 2,048 points (64 per line) after repair. Their line
latencies do not represent equal amounts of work. Retain these rows and actual
point counts; do not claim that incorrect old output is an equivalent-result
performance comparison or use it to conceal an ordinary-path regression.

## Current isolated results

.NET 8.0.26, Release, matched affinity and input batches. All 68 measurements for
the current kernel completed: a 20-case screening run followed by the remaining
48 cases, using the same eight warmups and sixteen 500 ms target iterations.
Four multimodality advisories remain; these are not warning-free acceptance runs.
The [CSV](BresenhamLineIterator.csv) retains confidence intervals, deviations,
point counts, latency, throughput, allocations and earlier contradictory evidence.

The clearest blocker is the valid **Int64 ShortHalfTie** comparison:
**63.378 -> 76.398 ns**, with 99.9% confidence half-widths **2.195 / 1.670 ns**.
That is a 20.5% latency increase / 17.0% throughput decrease. Other losing rows
also remain visible. The noisy Int32 short half-tie measurement is not used to
quantify a precise slowdown or dismissed in favor of faster workloads.

Iterator allocations fall **104 -> 96 B** for V2i and **176 -> 168 B** for V2l,
with no per-point allocation. This does not offset the throughput blocker.
`ExtremeFull` before/after point counts differ as explained above; its reported
point rates are descriptive, not equivalent-result speedup claims.

| Precision | Workload | Before ns/line | After ns/line | Before Mpoints/s | After Mpoints/s | Before B | After B |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Int32 | ExtremeDiagonal | 138.645 | 141.635 | 461.611 | 451.867 | 104 | 96 |
| Int32 | ExtremeFull | 31.497 | 146.153 | 152.795 | 437.897 | 104 | 96 |
| Int32 | ExtremeHalfTie | 161.163 | 163.580 | 397.115 | 391.246 | 104 | 96 |
| Int32 | ExtremePositive | 182.233 | 153.807 | 351.199 | 416.107 | 104 | 96 |
| Int32 | LongDiagonal | 9435.124 | 7715.541 | 434.229 | 531.006 | 104 | 96 |
| Int32 | LongHalfTie | 9076.056 | 8639.004 | 451.408 | 474.244 | 104 | 96 |
| Int32 | LongHorizontal | 8416.645 | 8875.887 | 486.774 | 461.588 | 104 | 96 |
| Int32 | LongShallow | 8825.850 | 8136.383 | 464.205 | 503.541 | 104 | 96 |
| Int32 | LongSteep | 9052.929 | 8727.080 | 452.561 | 469.458 | 104 | 96 |
| Int32 | LongVertical | 8184.713 | 7468.020 | 500.567 | 548.606 | 104 | 96 |
| Int32 | ShortDiagonal | 68.697 | 52.948 | 247.465 | 321.072 | 104 | 96 |
| Int32 | ShortHalfTie | 57.446 | 105.617 | 295.929 | 160.959 | 104 | 96 |
| Int32 | ShortHorizontal | 56.669 | 54.721 | 299.990 | 310.667 | 104 | 96 |
| Int32 | ShortShallow | 58.310 | 58.509 | 291.548 | 290.552 | 104 | 96 |
| Int32 | ShortSteep | 58.792 | 57.093 | 289.154 | 297.761 | 104 | 96 |
| Int32 | ShortVertical | 52.922 | 55.234 | 321.230 | 307.784 | 104 | 96 |
| Int32 | Singleton | 22.698 | 20.204 | 44.057 | 49.495 | 104 | 96 |
| Int64 | ExtremeDiagonal | 154.110 | 167.073 | 415.289 | 383.067 | 176 | 168 |
| Int64 | ExtremeFull | 34.707 | 138.791 | 138.662 | 461.124 | 176 | 168 |
| Int64 | ExtremeHalfTie | 173.239 | 169.074 | 369.431 | 378.533 | 176 | 168 |
| Int64 | ExtremePositive | 177.140 | 152.442 | 361.297 | 419.831 | 176 | 168 |
| Int64 | LongDiagonal | 9627.187 | 8167.529 | 425.566 | 501.621 | 176 | 168 |
| Int64 | LongHalfTie | 10030.869 | 9369.060 | 408.439 | 437.290 | 176 | 168 |
| Int64 | LongHorizontal | 7448.864 | 7138.000 | 550.017 | 573.970 | 176 | 168 |
| Int64 | LongShallow | 9944.572 | 8559.083 | 411.984 | 478.673 | 176 | 168 |
| Int64 | LongSteep | 8854.195 | 8542.022 | 462.719 | 479.629 | 176 | 168 |
| Int64 | LongVertical | 7673.126 | 7015.754 | 533.941 | 583.971 | 176 | 168 |
| Int64 | ShortDiagonal | 76.904 | 57.373 | 221.054 | 296.308 | 176 | 168 |
| Int64 | ShortHalfTie | 63.378 | 76.398 | 268.230 | 222.519 | 176 | 168 |
| Int64 | ShortHorizontal | 58.438 | 60.439 | 290.906 | 281.277 | 176 | 168 |
| Int64 | ShortShallow | 60.359 | 64.114 | 281.647 | 265.153 | 176 | 168 |
| Int64 | ShortSteep | 63.531 | 62.501 | 267.588 | 271.997 | 176 | 168 |
| Int64 | ShortVertical | 58.509 | 60.371 | 290.554 | 281.593 | 176 | 168 |
| Int64 | Singleton | 24.565 | 23.468 | 40.709 | 42.612 | 176 | 168 |

### Earlier exploratory measurements

The initial correct remainder implementation retained duplicate coordinate
locals and allocated the original 104/176 B. Its short sweep had tier transitions
and short-iteration warnings; several means changed by an order of magnitude
within measurement. Those values are retained, not accepted as warmed results.
Stable-looking losing rows also motivated reducing iterator state: 32-bit short
steep lines measured 54.36 to 58.51 ns, and 64-bit short half-tie lines measured
67.17 to 72.30 ns. The revised fixture explicitly warms before pilot sizing.

An attempted reduction of all four coordinate locals by mutating the working
vector parameter passed the tests but substantially regressed traversal. The
warmed 32-bit long diagonal case measured **9.385 to 30.600 microseconds** and
long steep **8.742 to 29.816 microseconds**. That variant was rejected and its
run stopped after the regression was established; its partial raw measurements
are retained. A subsequent endpoint-only state reduction passed Release tests,
but supplementary isolated F# probes still showed short half-tie losses (32-bit
102.11 to 111.66 ns; 64-bit 103.40 to 119.54 ns). Those probes are diagnostic,
not BenchmarkDotNet acceptance results. The current variant restores all scalar
coordinates and compacts only the signed unit directions.

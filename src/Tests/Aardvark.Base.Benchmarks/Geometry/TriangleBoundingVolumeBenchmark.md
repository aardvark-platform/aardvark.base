# Triangle bounding-volume benchmarks

Tracking: [#137](https://github.com/aardvark-platform/aardvark.base/issues/137).

**Performance acceptance passed.** No ordinary revised workload shows a
statistically distinguishable regression, and most show clear gains.
Exceptional-scale costs are reported separately because the baseline does not
produce equivalent valid bounds.

```sh
dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- \
  --filter '*TriangleBoundingVolumeBenchmark*' \
  --warmupCount 8 --iterationCount 16 --iterationTime 1000 --buildTimeout 600
```

Baseline methods reproduce the property bodies at `3db25671`, with readonly
reference receivers. Each invocation processes 256 prebuilt triangles; generation,
translation, orientation and scaling are outside timing. Both implementations
receive identical arrays. The 3D datasets use an oblique orthonormal embedding.
The mixed dataset alternates acute, right and obtuse input shapes. Floating-point
rounding of the embedding can move nominally right cases slightly to either side
of the right-angle boundary.

## Ordinary inputs

Release, .NET 8.0.26, isolated BenchmarkDotNet processes on the same processor
affinity, eight warmups and sixteen one-second target iterations. Times are
arithmetic means per bound. Throughput is `1000 / mean_ns` in millions of bounds
per second, not an average of reciprocal samples.

| Variant | Case | Before ns | After ns | Before Mbound/s | After Mbound/s |
| --- | --- | ---: | ---: | ---: | ---: |
| 2D double | Acute | 6.924 | 6.472 | 144.425 | 154.512 |
| 2D double | Right | 5.394 | 3.937 | 185.391 | 254.001 |
| 2D double | Obtuse | 5.095 | 4.452 | 196.271 | 224.618 |
| 2D double | Mixed | 5.939 | 5.142 | 168.379 | 194.477 |
| 2D float | Acute | 11.169 | 6.293 | 89.534 | 158.907 |
| 2D float | Right | 10.947 | 3.520 | 91.349 | 284.091 |
| 2D float | Obtuse | 10.649 | 3.829 | 93.906 | 261.165 |
| 2D float | Mixed | 11.150 | 4.527 | 89.686 | 220.897 |
| 3D double | Acute | 8.991 | 8.487 | 111.222 | 117.827 |
| 3D double | Right | 7.949 | 7.201 | 125.802 | 138.870 |
| 3D double | Obtuse | 6.319 | 5.881 | 158.253 | 170.039 |
| 3D double | Mixed | 7.435 | 7.471 | 134.499 | 133.851 |
| 3D float | Acute | 11.570 | 8.302 | 86.430 | 120.453 |
| 3D float | Right | 12.453 | 6.917 | 80.302 | 144.571 |
| 3D float | Obtuse | 11.684 | 5.719 | 85.587 | 174.856 |
| 3D float | Mixed | 11.936 | 7.092 | 83.780 | 141.004 |

The full forty-method run supplied the 2D values. The 3D values are from focused
warning-free reruns with identical settings after making determinant evaluation
lazy on diameter paths. The extended 32-iteration 3D-double run measured mixed at
`7.435 ± 0.147` versus `7.471 ± 0.192 ns` (99.9% confidence, ratio `1.01 ± 0.06`),
which is statistically unchanged; acute, right, and obtuse improved. Every run
compares each implementation in the same generated executable.

## Exceptional scales

These datasets multiply the ordinary coordinates by alternating large/small
powers of two: ±600 exponents for double, ±80 for float. The previous algorithm
can return Invalid or non-finite bounds; these are not equivalent correct-result
throughput comparisons and are kept separate from ordinary-path acceptance.

| Variant | Before ns | After ns | Before Mbound/s | After Mbound/s |
| --- | ---: | ---: | ---: | ---: |
| 2D double | 4.784 | 34.211 | 209.030 | 29.230 |
| 2D float | 42.046 | 87.608 | 23.783 | 11.414 |
| 3D double | 5.864 | 54.262 | 170.532 | 18.429 |
| 3D float | 23.655 | 85.008 | 42.274 | 11.764 |

## Allocations and run quality

MemoryDiagnoser measured **0 B per bound for every baseline and revised case**,
including exceptional scales. One baseline 3D-float distribution was multimodal
in the full run, so all 3D-float values above come from a clean focused rerun.
No inputs or correctness checks were weakened to claim acceptance.

# Triangle bounding-volume benchmarks

Tracking: [#137](https://github.com/aardvark-platform/aardvark.base/issues/137).

**Performance acceptance is blocked.** Faster 2D float queries do not offset
regressions in other independently used variants.

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
| 2D double | Acute | 7.201 | 8.607 | 138.870 | 116.185 |
| 2D double | Right | 5.716 | 7.459 | 174.948 | 134.066 |
| 2D double | Obtuse | 5.207 | 6.593 | 192.049 | 151.676 |
| 2D double | Mixed | 6.093 | 7.776 | 164.123 | 128.601 |
| 2D float | Acute | 11.300 | 8.001 | 88.496 | 124.984 |
| 2D float | Right | 11.584 | 6.838 | 86.326 | 146.242 |
| 2D float | Obtuse | 11.017 | 5.891 | 90.769 | 169.750 |
| 2D float | Mixed | 11.638 | 6.882 | 85.925 | 145.307 |
| 3D double | Acute | 8.991 | 18.417 | 111.222 | 54.298 |
| 3D double | Right | 8.142 | 17.468 | 122.820 | 57.248 |
| 3D double | Obtuse | 6.294 | 9.672 | 158.881 | 103.391 |
| 3D double | Mixed | 7.701 | 16.359 | 129.853 | 61.128 |
| 3D float | Acute | 12.172 | 17.721 | 82.156 | 56.430 |
| 3D float | Right | 13.131 | 17.187 | 76.156 | 58.184 |
| 3D float | Obtuse | 12.360 | 8.928 | 80.906 | 112.007 |
| 3D float | Mixed | 12.513 | 14.947 | 79.917 | 66.903 |

For acute 3D double queries, the 99.9% confidence half-widths are 0.1948 ns before
and 0.2034 ns after. The regression is not dismissed as measurement noise.

## Exceptional scales

These datasets multiply the ordinary coordinates by alternating large/small
powers of two: ±600 exponents for double, ±80 for float. The previous algorithm
can return Invalid or non-finite bounds; these are not equivalent correct-result
throughput comparisons and are kept separate from ordinary-path acceptance.

| Variant | Before ns | After ns | Before Mbound/s | After Mbound/s |
| --- | ---: | ---: | ---: | ---: |
| 2D double | 5.013 | 30.436 | 199.481 | 32.856 |
| 2D float | 46.532 | 94.407 | 21.491 | 10.592 |
| 3D double | 6.152 | 51.770 | 162.549 | 19.316 |
| 3D float | 25.367 | 90.714 | 39.421 | 11.024 |

## Allocations and run quality

MemoryDiagnoser measured **0 B per bound for every baseline and revised case**,
including exceptional scales. The isolated run completed all forty measurements
without BenchmarkDotNet warnings. A preliminary in-process run also showed
regressions; it is not used to override the isolated results. No inputs or
correctness checks were weakened to claim acceptance.

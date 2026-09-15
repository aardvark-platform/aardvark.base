# RotateInto numerical repair and performance

Tracking: [#149](https://github.com/aardvark-platform/aardvark.base/issues/149).

**Performance acceptance is blocked.** The revised implementation fixes the
near-antiparallel mappings, but the float exact-opposition workload still regresses.
No speedup elsewhere is used to offset that loss.

## Reproduction

```sh
dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- \
  --filter '*RotateIntoDouble*' '*RotateIntoFloat*' \
  --warmupCount 8 --iterationCount 20 --iterationTime 1000 --buildTimeout 600 \
  --exporters json
```

The generated .NET 8 fixtures compare the actual public API against the
pre-change `HalfWayQuat` kernel from `9bf23a1d`, with matching inlining attributes.
Each invocation handles 256 precomputed direction pairs. Both methods consume all
four quaternion components in a scalar checksum; neither multiplies a long chain
of quaternions. Input generation and normalization are outside the measured loop.
The benchmark dispatcher and project files are unchanged.

Workloads:

- **Ordinary:** independent seeded unit directions, including obtuse angles.
- **Parallel:** identical unit vectors.
- **Opposite:** component-exact negations of unit vectors.
- **NearOpposite:** seeded orientations with logarithmic tangent deviations,
  `1e-4` through `1e-12` for double and `1e-2` through `1e-6` for float.

The old near-opposition kernel can snap to the wrong rotation or lose the desired
deviation through cancellation. Those timings are retained but are not equivalent
correct-result comparisons. In contrast, the exact-opposition baseline is correct;
its measured regression is an acceptance blocker in its own right.

## Isolated results

.NET 8.0.26, Release, matched processor affinity, eight warmups and twenty one-second
target iterations. All 16 measurements completed without BenchmarkDotNet warnings.
Throughput is `1000 / mean_ns` in millions of rotations/second, calculated from the
arithmetic mean, not an average of reciprocal samples. Allocations are reported
separately by MemoryDiagnoser.

| Precision | Workload | Before ns | After ns | Before Mop/s | After Mop/s | Before B | After B |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| double | NearOpposite | 4.826 | 5.512 | 207.232 | 181.412 | 0 | 0 |
| double | Opposite | 4.631 | 4.465 | 215.946 | 223.979 | 0 | 0 |
| double | Ordinary | 4.741 | 4.839 | 210.947 | 206.639 | 0 | 0 |
| double | Parallel | 4.464 | 4.480 | 224.037 | 223.213 | 0 | 0 |
| float | NearOpposite | 4.457 | 5.486 | 224.387 | 182.294 | 0 | 0 |
| float | Opposite | 4.305 | 4.893 | 232.275 | 204.385 | 0 | 0 |
| float | Ordinary | 4.948 | 4.998 | 202.105 | 200.072 | 0 | 0 |
| float | Parallel | 4.838 | 4.928 | 206.682 | 202.930 | 0 | 0 |

For float exact opposition, the 99.9% confidence half-widths are **0.0695 ns**
before and **0.1540 ns** after. The approximately 13.7% latency increase / 12.0%
throughput decrease is not dismissed as noise. The small ordinary/parallel timing
differences are also retained without claiming a performance win.

## Correctness and implementation

The ordinary kernel retains normalized `(1 + dot, cross)`. Near opposition uses
`from × (from + into)` and normalizes a proportional quaternion formed using
`1 - dot`, avoiding cancellation in the small scalar component. A scaled fallback
avoids squaring tiny cross products or forming overflowing reciprocals. Exactly
opposite inputs retain the deterministic `AxisAlignedNormal()` choice. No
trigonometric calls or managed allocations are added.

The dedicated NUnit fixture covers the requested X-axis examples, arbitrary
orientations, signed logarithmic deviations, branch continuity, identical and
exactly opposite inputs, finite unit quaternions, inverse round-trips, and all
inherited 3D matrix/transformation factories. It includes 40,000 seeded mappings
checked against Cartesian target residuals and analytic scalar checks extending
to `1e-300` double / `1e-38` float deviations. Sixteen of the 21 cases failed
before the repair; all 21 pass after it. Warmed allocation checks independently
confirm 0 B.

## Retained earlier evidence

The initial correct implementation outlined near-opposition handling and normalized
`(crossSquared / (1 - dot), cross)`. It regressed exact opposition as well:
**4.354 → 5.144 ns** double and **4.254 → 5.656 ns** float. Moving exact detection
before the dot and reusing the squared cross in normalization produced the main
results above. An intervening run was interrupted and is not an acceptance run.

A subsequent bitwise, all-components equality check was rejected: isolated double
ordinary queries measured **4.674 → 5.115 ns**, with the in-process comparison
also slower (**4.607 → 5.144 ns**). That exploratory run used default isolated
settings plus six warmups/twelve 500 ms target iterations in-process and had
short-iteration/statistical warnings. The change was reverted; neither those
warnings nor the contradictory measurements are hidden or treated as acceptance.

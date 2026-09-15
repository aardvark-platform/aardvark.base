# RotateInto numerical repair and performance

Tracking: [#149](https://github.com/aardvark-platform/aardvark.base/issues/149) and
[PR #150](https://github.com/aardvark-platform/aardvark.base/pull/150).

**Performance acceptance is satisfied.** The revised implementation fixes the
near-antiparallel mappings while ordinary, parallel, and exactly opposite workloads
are statistically unchanged or faster. The slower near-opposition measurements are
retained separately because the baseline does not compute the requested rotation.

## Reproduction

```sh
dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- \
  --filter '*RotateIntoDouble*' '*RotateIntoFloat*' \
  --warmupCount 8 --iterationCount 20 --iterationTime 1000 --buildTimeout 600 \
  --affinity 32768 --exporters json
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
correct-result comparisons. Exact opposition and the remaining workloads are
equivalent before/after comparisons and are evaluated independently.

## Isolated results

.NET 8.0.26, Release, matched processor affinity, eight warmups and twenty one-second
target iterations. The retained double run and warning-free float rerun completed
without BenchmarkDotNet warnings. Throughput is `1000 / mean_ns` in millions of
rotations/second, calculated from the arithmetic mean, not an average of reciprocal
samples. Allocations are reported separately by MemoryDiagnoser.

| Precision | Workload | Before ns | After ns | Before Mop/s | After Mop/s | Before B | After B |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| double | NearOpposite | 8.671 | 10.246 | 115.327 | 97.599 | 0 | 0 |
| double | Opposite | 8.637 | 8.493 | 115.781 | 117.744 | 0 | 0 |
| double | Ordinary | 9.127 | 9.133 | 109.565 | 109.493 | 0 | 0 |
| double | Parallel | 9.139 | 9.113 | 109.421 | 109.733 | 0 | 0 |
| float | NearOpposite | 7.890 | 10.452 | 126.743 | 95.675 | 0 | 0 |
| float | Opposite | 7.746 | 7.822 | 129.099 | 127.845 | 0 | 0 |
| float | Ordinary | 9.017 | 8.896 | 110.902 | 112.410 | 0 | 0 |
| float | Parallel | 8.949 | 8.875 | 111.744 | 112.676 | 0 | 0 |

For float exact opposition, the reported 99.9% confidence intervals overlap broadly:
**7.746 +/- 0.154 ns** before and **7.822 +/- 0.205 ns** after. The former 13.7%
regression is no longer present after outlining the exceptional fallback. Double
exact opposition is faster, while ordinary and parallel means are unchanged or
improved in both precisions. No winning workload is averaged against a losing one.

## Correctness and implementation

The ordinary kernel retains normalized `(1 + dot, cross)`. Near opposition uses
`from × (from + into)` and normalizes a proportional quaternion formed using
`1 - dot`, avoiding cancellation in the small scalar component. A scaled fallback
avoids squaring tiny cross products or forming overflowing reciprocals. Exactly
opposite inputs retain the deterministic `AxisAlignedNormal()` choice. No
trigonometric calls or managed allocations are added.

The scaled fallback is marked `NoInlining`: it is reached only when the cross-product
square would underflow, and keeping it out of the caller reduced the inlined float
benchmark kernel from 858 to 686 bytes. This restores the common and exact-opposite
paths without changing any numerical operation or exceptional-input result.

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
before the dot and reusing the squared cross in normalization produced the retained
implementation. Before outlining the exceptional fallback, the complete isolated
run still measured float exact opposition at **4.305 → 4.893 ns** and near opposition
at **4.457 → 5.486 ns**. Those adverse results remain part of the record. An
intervening run was interrupted and is not an acceptance run.

A subsequent bitwise, all-components equality check was rejected: isolated double
ordinary queries measured **4.674 → 5.115 ns**, with the in-process comparison
also slower (**4.607 → 5.144 ns**). That exploratory run used default isolated
settings plus six warmups/twelve 500 ms target iterations in-process and had
short-iteration/statistical warnings. The change was reverted; neither those
warnings nor the contradictory measurements are hidden or treated as acceptance.

Two arithmetic experiments were also rejected. Replacing the normalization identity
was repeatedly slower in a diagnostic probe (representative **10.291 → 10.367 ns**
and **10.378 → 10.742 ns** comparisons). A multiply-add variant made the screened
float near-opposition result **7.842 → 11.607 ns**. Both changes were reverted and
neither screening run is used as acceptance evidence. An earlier full float run with
the retained code reported a multimodal baseline ordinary distribution; the
warning-free rerun above supersedes it while preserving that raw result.

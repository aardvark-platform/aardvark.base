# SortedSetExt neighbour benchmarks

Tracking: [#139](https://github.com/aardvark-platform/aardvark.base/issues/139).

The final matched run satisfies the no-regression gate. Read-only view queries
improve by 15-31%; mutation-plus-query workloads improve or are statistically
unchanged. Ordinary lookup source remains unchanged and is retained as a control.

## Reproduction

Build an unchanged baseline in a separate worktree:

```sh
git worktree add --detach /tmp/aardvark-neighbours-baseline 5f86b344
(cd /tmp/aardvark-neighbours-baseline && ./build.sh restore && \
  dotnet build src/Aardvark.Base/Aardvark.Base.csproj -c Release -f net8.0)
dotnet build src/Tests/Aardvark.Base.Benchmarks -c Release
```

Run the same benchmark first with the revised assembly, then the baseline:

```sh
for assembly in "$PWD/bin/Release/net8.0/Aardvark.Base.dll" \
  /tmp/aardvark-neighbours-baseline/bin/Release/net8.0/Aardvark.Base.dll; do
  AARDVARK_SORTEDSET_ASSEMBLY="$assembly" \
    dotnet run -c Release --no-build --project src/Tests/Aardvark.Base.Benchmarks -- \
    --filter '*SortedSetNeighbourBenchmark*' \
    --warmupCount 8 --iterationCount 20 --iterationTime 1000 --buildTimeout 600 \
    --affinity 32768
done
```

The fixture loads one selected assembly into an isolated load context and builds
one parent/tree per benchmark process. Both versions use the same public-API
adapter; reflection, delegate compilation, construction and view creation are
outside timing. No private lookup or nonvirtual baseline shortcuts are used.

The parent has 65,536 even integers. Narrow views contain 17 entries, wide views
49,153. Each invocation processes 256 prebuilt queries covering both bounds,
inside hits/misses, and values just outside the bounds. `Mutate=true` removes
and re-adds the middle key immediately before **each** lookup. These rows measure
the complete mutation-plus-first-query operation, not lookup alone. Neither a
view count nor its enumeration is touched to refresh it before querying.

The original view results can be incorrect. Their numbers are retained rather
than treated as equivalent correct implementations.

## Final isolated Release results

.NET 8.0.26, same processor affinity, eight warmup iterations and twenty one-second
target iterations. The revised suite ran before the baseline suite. Latencies
are arithmetic means; throughput is `1000 / mean_ns` in millions of operations
per second. For mutation rows an operation includes remove, add and lookup.

| Scope | API | Mutate | Before ns | After ns | Before Mop/s | After Mop/s | Time delta |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| Narrow | FindNeighboursV | False | 48.49 | 39.58 | 20.623 | 25.265 | -18.37% |
| Narrow | FindNeighboursV | True | 190.83 | 190.89 | 5.240 | 5.239 | +0.03% |
| Narrow | TryFindGreater | False | 53.86 | 37.95 | 18.567 | 26.350 | -29.54% |
| Narrow | TryFindGreater | True | 195.57 | 197.42 | 5.113 | 5.065 | +0.95% |
| Narrow | TryFindSmaller | False | 54.42 | 37.68 | 18.376 | 26.539 | -30.76% |
| Narrow | TryFindSmaller | True | 200.67 | 191.10 | 4.983 | 5.233 | -4.77% |
| Wide | FindNeighboursV | False | 48.91 | 41.48 | 20.446 | 24.108 | -15.19% |
| Wide | FindNeighboursV | True | 201.38 | 193.87 | 4.966 | 5.158 | -3.73% |
| Wide | TryFindGreater | False | 51.74 | 38.98 | 19.327 | 25.654 | -24.66% |
| Wide | TryFindGreater | True | 205.15 | 194.56 | 4.874 | 5.140 | -5.16% |
| Wide | TryFindSmaller | False | 52.96 | 40.84 | 18.882 | 24.486 | -22.89% |
| Wide | TryFindSmaller | True | 199.71 | 193.04 | 5.007 | 5.180 | -3.34% |

The two nominally slower rows are statistically unchanged: their 99.9%
confidence intervals overlap. The full revised suite completed without warnings.
The baseline ordinary `TryFindGreater` control initially triggered a multimodal
distribution warning at 56.43 ns; an exact one-row rerun was unimodal at
58.72 ns. The revised source-identical control measured 57.89 ns.

Ordinary `FindNeighboursV`, `TryFindGreater`, and `TryFindSmaller` read-only
controls measured 53.73/56.43/58.24 ns before and 53.24/57.89/58.25 ns after.
Their confidence intervals overlap, confirming no ordinary lookup change.

## Allocation and complexity checks

- MemoryDiagnoser reports **0 B per operation** for all three APIs on ordinary
  sets and both view sizes without mutation.
- Mutation rows report **40 B** before and after, from the reinserted tree node.
  They are not claims that parent mutation allocates nothing.
- Warmed unit assertions independently verify zero query allocation after parent
  mutation. Counting-comparer tests bound lookup work by a logarithmic budget at
  1,024 and 65,536 parent entries and verify the view's cached root, count and
  version remain untouched. No view-size scan is hidden outside the counter.
- The optional-returning API and F# wrapper retain their existing optional/option
  allocations; they are covered for correctness, not claimed allocation-free.

## Earlier harness controls

An initial two-subject harness compared an isolated baseline to the default
assembly. A same-binary in-process control reported ordinary `FindNeighboursV`
at 37.49 versus 45.77 ns despite identical code. Making both contexts isolated
still gave 37.58 versus 41.58 ns. These systematic differences exposed a harness
confound; the retained fixture therefore uses one subject and one adapter per
process. The earlier full comparison also showed regressions (narrow
`FindNeighboursV` 34.23 versus 41.14 ns). None of these controls is used to
assert acceptance; the final one-assembly-per-process matrix supersedes them.

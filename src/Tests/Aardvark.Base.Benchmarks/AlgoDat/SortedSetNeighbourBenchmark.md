# SortedSetExt neighbour benchmarks

Tracking: [#139](https://github.com/aardvark-platform/aardvark.base/issues/139).

**The no-regression gate is not satisfied.** Several view workloads improve,
but narrow-view and other measured regressions remain. Ordinary lookup source
is unchanged; its timing differences also require investigation, not dismissal.

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
    --warmupCount 8 --iterationCount 20 --iterationTime 1000 --buildTimeout 600
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

## Isolated Release results

.NET 8.0.26, same processor affinity, eight warmup iterations and twenty one-second
target iterations. The revised suite ran before the baseline suite. Latencies
are arithmetic means; throughput is `1000 / mean_ns` in millions of operations
per second. For mutation rows an operation includes remove, add and lookup.

| Scope | API | Mutate | Before ns | After ns | Before Mop/s | After Mop/s |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| Narrow | FindNeighboursV | False | 33.93 | 39.38 | 29.472 | 25.394 |
| Narrow | FindNeighboursV | True | 112.25 | 113.02 | 8.909 | 8.848 |
| Narrow | TryFindGreater | False | 39.48 | 41.98 | 25.329 | 23.821 |
| Narrow | TryFindGreater | True | 126.64 | 109.97 | 7.896 | 9.093 |
| Narrow | TryFindSmaller | False | 35.19 | 41.38 | 28.417 | 24.166 |
| Narrow | TryFindSmaller | True | 117.67 | 120.02 | 8.498 | 8.332 |
| Ordinary | FindNeighboursV | False | 37.15 | 41.61 | 26.918 | 24.033 |
| Ordinary | FindNeighboursV | True | 113.20 | 123.04 | 8.834 | 8.127 |
| Ordinary | TryFindGreater | False | 43.18 | 38.51 | 23.159 | 25.967 |
| Ordinary | TryFindGreater | True | 121.56 | 121.91 | 8.226 | 8.203 |
| Ordinary | TryFindSmaller | False | 39.54 | 44.87 | 25.291 | 22.287 |
| Ordinary | TryFindSmaller | True | 125.64 | 122.04 | 7.959 | 8.194 |
| Wide | FindNeighboursV | False | 35.97 | 33.73 | 27.801 | 29.647 |
| Wide | FindNeighboursV | True | 129.63 | 161.54 | 7.714 | 6.190 |
| Wide | TryFindGreater | False | 40.53 | 31.49 | 24.673 | 31.756 |
| Wide | TryFindGreater | True | 116.06 | 111.31 | 8.616 | 8.984 |
| Wide | TryFindSmaller | False | 37.33 | 33.49 | 26.788 | 29.860 |
| Wide | TryFindSmaller | True | 128.08 | 117.62 | 7.808 | 8.502 |

For narrow `FindNeighboursV` without mutation, the 99.9% confidence half-widths
are 0.446 ns before and 0.877 ns after. This regression also appeared in earlier
measurements and is not dismissed as noise. Both final suites completed without
BenchmarkDotNet warnings.

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
assert acceptance or override the remaining regressions above.

# Bounded fixed-size array construction

Tracking: [#153](https://github.com/aardvark-platform/aardvark.base/issues/153).
Baseline: `cc2c3ebd`. Results are retained in
[FixedSizeArrayConstructionBench.csv](FixedSizeArrayConstructionBench.csv).

## Method

The fixture measures the actual public `Arr` constructor, not a duplicated kernel.
Each isolated process loads one selected F# assembly and calls its constructor
through the same compiled delegate. Binding, source creation and an independent
prefix/padding result check happen outside measurement. Both implementations
return the same values on every benchmark input.

Precomputed arrays, F# lists and genuine lazy sequences cover capacities 0, 1, 4
and 32; empty, short, exact, 4,096-element and million-element inputs give 39
workload pairs. Each invocation constructs 64 arrays, returning the final result;
reported time and allocation are per construction, not per batch. The inputs are
never mutated. A two-second selected-constructor warmup precedes pilot sizing.

.NET 8.0.26, warmed Release, matched affinity, paired isolated before/after jobs.
The complete screening matrix uses six warmups and twelve 500-ms target iterations.
Throughput is `1000 / mean_ns` in millions of constructions per second; managed
allocation is measured separately with MemoryDiagnoser. Reflection and delegate
creation are not part of either timed path. The existing benchmark dispatcher
remains unchanged.

## Initial small-input results

These measurements are not performance acceptance. The general enumerator path
regresses on small F# lists, including allocations for empty/short inputs.
Oversized-input improvements cannot offset those losses.

| Source, input (capacity 4) | Before ns | After ns | Before B | After B |
| --- | ---: | ---: | ---: | ---: |
| Array, empty | 60.644 | 28.412 | 88 | 64 |
| Array, short (2) | 58.320 | 27.482 | 96 | 64 |
| Array, exact (4) | 60.616 | 29.133 | 104 | 64 |
| List, empty | 28.878 | 33.683 | 88 | 104 |
| List, short (2) | 32.918 | 53.026 | 96 | 104 |
| List, exact (4) | 37.990 | 50.597 | 104 | 104 |
| Lazy sequence, empty | 56.388 | 64.198 | 216 | 216 |
| Lazy sequence, short (2) | 89.886 | 83.135 | 272 | 200 |
| Lazy sequence, exact (4) | 100.144 | 92.848 | 240 | 200 |

## Complete matrix and confirmation

All **78 primary measurements / 39 workload pairs** completed. `screen` contains
18 measurements and `remaining` the other 60; the constructor and inputs did not
change between runs. All rows remain in the CSV, including slower list and lazy
sequence cases at other capacities. Part of the remaining sweep overlapped
background compilation, so those timings are exploratory, not acceptance evidence.
Neither primary run emitted BenchmarkDotNet warnings.

Allocation no longer scales with discarded input length. At capacity four,
nonempty arrays allocate **64 B**, lists **104 B**, and lazy sequences **200 B**
for exact, 4,096-element, and million-element inputs. For one million inputs,
measured allocations fall from **4,001,594 to 64 B** (array), **4,000,574 to 104 B**
(list), and **12,394,574 to 200 B** (lazy sequence). These gains do not excuse
increased allocation or slower traversal on short inputs.

The four slower capacity-four cases were repeated in the opposite job order,
after waiting for compilation to stop, with twelve warmups and twenty-four
one-second target iterations. The additional eight measurements are retained as
`confirmation`, not substituted for the original readings:

| Source, input | Before ns | After ns | Before M/s | After M/s | Before B | After B |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| List, empty | 28.284 | 36.747 | 35.355 | 27.213 | 88 | 104 |
| List, short (2) | 32.172 | 52.540 | 31.083 | 19.033 | 96 | 104 |
| List, exact (4) | 38.036 | 53.339 | 26.291 | 18.748 | 104 | 104 |
| Lazy sequence, empty | 56.116 | 60.384 | 17.820 | 16.561 | 216 | 216 |

The short-list confirmation has 99.9% confidence half-widths **0.458/0.807 ns**:
**63.3% higher latency / 38.8% lower throughput**, plus 8 B more allocation.
Exact-list latency rises 40.2%; empty-list allocation rises by 16 B. Empty lazy
sequences also remain slower. One multimodality advisory was emitted in the
confirmation and is retained; it is not a warning-free acceptance run.

**Performance acceptance remains blocked.** No release-note update or merge is
justified. The general enumerator path cannot be called a no-regression repair
on the strength of its oversized-input improvements.

## Reproduction

Build the old assembly in a separate worktree:

```sh
git worktree add --detach /tmp/fixed-array-baseline cc2c3ebd
(cd /tmp/fixed-array-baseline && ./build.sh restore && \
  dotnet build src/Aardvark.Base.FSharp/Aardvark.Base.FSharp.fsproj -c Release -f net8.0)
```

The standard F# benchmark program defaults to in-process measurement. For isolated
paired jobs, create a runner in the ignored `bin/fixed-array-runner` directory.
`runner.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework></PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="../../src/Tests/Aardvark.Base.FSharp.Benchmarks/Aardvark.Base.FSharp.Benchmarks.fsproj" />
  </ItemGroup>
</Project>
```

`Program.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Aardvark.Base.FSharp.Benchmarks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

var before = Environment.GetEnvironmentVariable("AARDVARK_FIXEDARRAY_BASELINE")
    ?? throw new InvalidOperationException("Set AARDVARK_FIXEDARRAY_BASELINE.");
var after = Environment.GetEnvironmentVariable("AARDVARK_FIXEDARRAY_CURRENT")
    ?? throw new InvalidOperationException("Set AARDVARK_FIXEDARRAY_CURRENT.");
var config = ManualConfig.Create(DefaultConfig.Instance)
    .WithOrderer(new PairedOrderer())
    .AddJob(Job.Default.WithId("Before").AsBaseline()
        .WithEnvironmentVariable("AARDVARK_FIXEDARRAY_ASSEMBLY", before))
    .AddJob(Job.Default.WithId("After")
        .WithEnvironmentVariable("AARDVARK_FIXEDARRAY_ASSEMBLY", after));
BenchmarkSwitcher.FromTypes(new[] { typeof(FixedSizeArrayConstructionBenchmark) }).Run(args, config);

sealed class PairedOrderer : DefaultOrderer
{
    public override IEnumerable<BenchmarkCase> GetExecutionOrder(
        ImmutableArray<BenchmarkCase> cases, IEnumerable<BenchmarkLogicalGroupRule> rules) =>
        cases.OrderBy(b => string.Join(";", b.Parameters.Items.Select(p => p.Name + "=" + p.Value)))
             .ThenBy(b => b.Job.Id == (Environment.GetEnvironmentVariable("AARDVARK_FIXEDARRAY_REVERSE") == "1" ? "After" : "Before") ? 0 : 1);
}
```

Run from the repository root:

```sh
dotnet build bin/fixed-array-runner/runner.csproj -c Release
AARDVARK_FIXEDARRAY_BASELINE=/tmp/fixed-array-baseline/bin/Release/net8.0/Aardvark.Base.FSharp.dll \
AARDVARK_FIXEDARRAY_CURRENT="$PWD/bin/Release/net8.0/Aardvark.Base.FSharp.dll" \
  dotnet run -c Release --no-build --project bin/fixed-array-runner -- \
  --filter '*FixedSizeArrayConstructionBenchmark*' --warmupCount 6 --iterationCount 12 \
  --iterationTime 500 --buildTimeout 600 --exporters json
```

Use the same processor affinity for both subjects. For the longer confirmation,
set `AARDVARK_FIXEDARRAY_REVERSE=1`, use twelve warmups and twenty-four one-second
iterations, and filter the three capacity-four list cases and empty lazy sequence.
Retain `FullName` when matching JSON rows; abbreviated display parameters need not
be unique. The CSV preserves each run separately, rather than replacing slower
readings with favorable repeats.

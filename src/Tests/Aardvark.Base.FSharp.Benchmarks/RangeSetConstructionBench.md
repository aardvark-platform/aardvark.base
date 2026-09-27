# Integer RangeSet construction

Tracking: [#143](https://github.com/aardvark-platform/aardvark.base/issues/143).
Baseline: `f185cb56`. Full latency, throughput, allocation and confidence-interval
results are retained in [RangeSetConstructionBench.csv](RangeSetConstructionBench.csv).

## Method

The F# fixture covers `ofArray`, `ofList`, and `ofSeq` for representative 32-bit
signed and 64-bit unsigned types. Each has empty/singleton cases and 32/4,096
intervals with sorted, reversed and deterministically shuffled disjoint, adjacent
and overlapping inputs: 120 combinations in total. Correctness tests cover all
four types and all constructors, including their extreme endpoints.

Each isolated process loads **one** selected F# assembly and constructs through
the same strongly typed public-method delegate. Reflection/delegate binding,
input construction, and checking results against repeated `Add` are outside
timing. Sequence inputs are genuine sequence expressions, not runtime array/list
shortcuts. Inputs are immutable across invocations, so no timed reset or copying
by the harness is necessary; ownership copies made by the constructor are timed.
The returned set is consumed by BenchmarkDotNet.

.NET 8.0.26, Release, matched processor affinity. The primary run uses eight
warmups and twenty one-second target iterations. Before/after jobs execute
consecutively for each parameter combination, not as two hours-apart suites.
Throughput is `1000 / mean_ns` in millions of complete constructions per second;
it is not an average of reciprocal timing samples. Managed allocation is reported
separately by MemoryDiagnoser.

## Results

Every disjoint/overlapping bulk combination measured faster: **1.04–5.36×**,
with **12.7–97.0% less managed allocation**. These are equivalent correct-result
comparisons. Selected Int32 array means:

| Input | Before ns | After ns | Before B | After B |
| --- | ---: | ---: | ---: | ---: |
| Disjoint sorted, 4096 | 3,452,154 | 3,240,671 | 6,454,037 | 5,503,386 |
| Disjoint reversed, 4096 | 3,514,583 | 3,132,761 | 6,454,038 | 5,503,386 |
| Disjoint shuffled, 4096 | 3,837,158 | 3,505,386 | 6,454,038 | 5,503,390 |
| Overlapping sorted, 4096 | 471,379 | 185,213 | 1,114,496 | 33,040 |
| Overlapping reversed, 4096 | 475,009 | 247,964 | 1,114,496 | 33,040 |
| Overlapping shuffled, 4096 | 776,435 | 434,090 | 1,114,497 | 33,040 |
| Singleton | 60.436 | 39.429 | 216 | 160 |

UInt64 array singleton construction improves **63.857 → 39.076 ns**,
**248 → 176 B**. All singleton constructor variants improve. Empty construction
remains **0 B**, with no measured regression in the batched confirmation below.
No meaningful empty-path speedup is claimed at this scale.

The old implementation is **incorrect** on sorted/shuffled adjacent cases;
setup reports that disagreement. Their measurements remain in the CSV, clearly
marked `BaselineCorrect=False`, and are not counted as equivalent-result wins.
Reversed adjacent inputs agree with the reference for this dataset. All revised
cases agree with it.

## Controls and statistical follow-up

Evidence is retained rather than dropping inconvenient rows:

- An initial 500 ms sweep, before singleton cleanup, had short-iteration and
  multimodality warnings. It included slower readings such as UInt64 empty-list
  construction **2.988 → 3.399 ns** and Int32 sorted-disjoint list construction
  **3.526 → 3.577 ms**. It is not the final implementation/acceptance run.
- The complete one-second paired matrix has six multimodality advisories. Its
  four affected bulk pairs were repeated with reversed job order, sixteen
  warmups and thirty one-second iterations. All remained faster, without bulk
  warnings. Both sets of means are in the CSV; the repeat is not substituted
  silently into the primary table.
- Empty single-call timings are near harness overhead. The primary matrix gave,
  for example, Int32 array **2.798 → 3.059 ns**; reversing order gave
  **3.069 → 3.039 ns**. Such changes in sign/magnitude prevent treating tenths of
  a nanosecond as a reliable regression or improvement.
- The separate `RangeSetEmptyConstructionBenchmark` batches 1,024 unchanged
  construction calls per invocation, amortizing overhead estimation. With
  sixteen warmups and thirty one-second iterations, all six before/after means
  were unchanged or faster and remained 0 B. One UInt64-array advisory was
  checked again in the opposite job order with forty two-second iterations:
  **3.301 → 3.045 ns**, 0 B, no warnings. The other five batched pairs had no
  warnings. These controls support no empty-path regression; they do not offset
  a losing bulk workload.

## Reproduction

Build the baseline in a separate worktree:

```sh
git worktree add --detach /tmp/rangeset-baseline f185cb56
(cd /tmp/rangeset-baseline && ./build.sh restore && \
  dotnet build src/Aardvark.Base.FSharp/Aardvark.Base.FSharp.fsproj -c Release -f net8.0)
```

The existing F# benchmark program defaults to in-process measurement. To retain
that dispatcher unchanged while measuring isolated matched jobs, create a small
runner under the repository's ignored `bin/rangeset-runner` directory.
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
using Aardvark.Base;
using Aardvark.Base.FSharp.Benchmarks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

var baseline = Environment.GetEnvironmentVariable("AARDVARK_RANGESET_BASELINE")
    ?? throw new InvalidOperationException("Set AARDVARK_RANGESET_BASELINE.");
var config = ManualConfig.Create(DefaultConfig.Instance)
    .WithOrderer(new PairedOrderer())
    .AddJob(Job.Default.WithId("Before").AsBaseline()
        .WithEnvironmentVariable("AARDVARK_RANGESET_ASSEMBLY", baseline))
    .AddJob(Job.Default.WithId("After")
        .WithEnvironmentVariable("AARDVARK_RANGESET_ASSEMBLY", typeof(RangeSet1i).Assembly.Location));
BenchmarkSwitcher.FromAssembly(typeof(RangeSetConstructionBenchmark).Assembly).Run(args, config);

sealed class PairedOrderer : DefaultOrderer
{
    public override IEnumerable<BenchmarkCase> GetExecutionOrder(
        ImmutableArray<BenchmarkCase> cases, IEnumerable<BenchmarkLogicalGroupRule> rules) =>
        cases.OrderBy(b => string.Join(";", b.Parameters.Items.Select(p => p.Name + "=" + p.Value)))
             .ThenBy(b => b.Job.Id == "Before" ? 0 : 1);
}
```

Run from the repository root so BenchmarkDotNet can locate the F# project:

```sh
dotnet build bin/rangeset-runner/runner.csproj -c Release
AARDVARK_RANGESET_BASELINE=/tmp/rangeset-baseline/bin/Release/net8.0/Aardvark.Base.FSharp.dll \
  dotnet run -c Release --no-build --project bin/rangeset-runner -- \
  --filter '*RangeSetConstructionBenchmark*' --warmupCount 8 --iterationCount 20 \
  --iterationTime 1000 --buildTimeout 600 --exporters json
```

Use the same processor affinity for both jobs. Reverse the last ordering key for
opposite-order confirmation. For batched empty checks, filter
`*RangeSetEmptyConstructionBenchmark*`; the final individual confirmation used
`*RangeSetEmptyConstructionBenchmark*UInt64*array*`. Preserve JSON `FullName`
when processing results: the human-readable `Parameters` field truncates long
input labels and is **not** a unique key.

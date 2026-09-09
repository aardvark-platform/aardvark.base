# Linear-combination benchmarks

Tracking: [#135](https://github.com/aardvark-platform/aardvark.base/issues/135).

**Acceptance is blocked by predicate regressions.** Coefficient improvements do
not offset regressions in independently used predicates.

```sh
dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- \
  --filter '*LinearCombinationBenchmark*' \
  --warmupCount 8 --iterationCount 20 --iterationTime 1000 --buildTimeout 600
```

The baseline copies the method bodies at `eaa8f342`. Each invocation processes
256 identical prebuilt inputs; setup is outside timing. Both precisions cover
ordinary hits, ordinary misses, and dependent bases (parallel, repeated,
antiparallel, one zero, both zero; alternating hits and misses). Single-basis
queries use corresponding line targets rather than two-basis plane targets.
Both coefficient timing adapters catch `ArgumentException`: the old dependent
queries throw, while the revised queries return a result. The old dependent
predicates also give incorrect answers. Those rows are retained, but are not
comparisons between two correct implementations.

## Isolated Release results

.NET 8.0.26; separate BenchmarkDotNet processes with the same processor affinity,
eight warmup iterations and twenty one-second target iterations. Latency is the
arithmetic mean per query. Throughput is `1000 / mean_ns` in millions of queries
per second, not an average of individual sample reciprocals. Managed allocations
are measured separately by MemoryDiagnoser and reported per query.

| Operation | Case | Before ns | After ns | Before Mquery/s | After Mquery/s | Before B | After B |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Double coefficients | Dependent | 6706.316 | 7.644 | 0.149 | 130.822 | 432 | 0 |
| Double coefficients | Hit | 101.110 | 9.391 | 9.890 | 106.485 | 248 | 0 |
| Double coefficients | Miss | 95.966 | 5.302 | 10.420 | 188.608 | 248 | 0 |
| Double pair | Dependent | 2.021 | 5.587 | 494.805 | 178.987 | 0 | 0 |
| Double pair | Hit | 2.009 | 2.310 | 497.760 | 432.900 | 0 | 0 |
| Double pair | Miss | 1.958 | 2.022 | 510.725 | 494.560 | 0 | 0 |
| Double single | Dependent | 1.813 | 2.161 | 551.572 | 462.749 | 0 | 0 |
| Double single | Hit | 1.736 | 2.028 | 576.037 | 493.097 | 0 | 0 |
| Double single | Miss | 1.691 | 1.943 | 591.366 | 514.668 | 0 | 0 |
| Float coefficients | Dependent | 8252.234 | 7.307 | 0.121 | 136.855 | 728 | 0 |
| Float coefficients | Hit | 88.034 | 11.116 | 11.359 | 89.960 | 200 | 0 |
| Float coefficients | Miss | 98.288 | 7.891 | 10.174 | 126.727 | 200 | 0 |
| Float pair | Dependent | 2.009 | 5.886 | 497.760 | 169.895 | 0 | 0 |
| Float pair | Hit | 2.014 | 2.251 | 496.524 | 444.247 | 0 | 0 |
| Float pair | Miss | 1.930 | 2.007 | 518.135 | 498.256 | 0 | 0 |
| Float single | Dependent | 1.825 | 2.216 | 547.945 | 451.264 | 0 | 0 |
| Float single | Hit | 1.806 | 2.021 | 553.710 | 494.805 | 0 | 0 |
| Float single | Miss | 1.765 | 2.002 | 566.572 | 499.500 | 0 | 0 |

For ordinary two-basis hits, the 99.9% confidence half-widths are 0.0369 / 0.0385 ns
before/after for double, and 0.0445 / 0.0953 ns for float. The latency increases are
not dismissed as noise. Ordinary single-basis predicates also regress.

BenchmarkDotNet flagged multimodality in two double coefficient measurements;
this is diagnostic evidence, not a warning-free performance acceptance run.
An earlier in-process prototype also regressed on predicates. It is not used to
override these isolated measurements. No benchmark inputs or tolerance rules
were weakened to obtain better results.

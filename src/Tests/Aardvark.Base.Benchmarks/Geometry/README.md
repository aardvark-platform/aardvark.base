# Geometry transform overload perf

`TransformOverloadTargetedPerf` compares the pre-existing conversion/inverse path (`old path`) with the typed overload or direct specialization (`new overload`). It is intended for local evidence, not CI pass/fail timing thresholds.

Build in `Release` and keep dependency builds serial to avoid output-file locks:

```powershell
dotnet build src/Aardvark.Base/Aardvark.Base.csproj -c Release -p:BuildInParallel=false
dotnet build src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -c Release -p:BuildInParallel=false --no-restore
```

Useful commands:

```powershell
# Validate that the targeted perf registry still covers every overload family covered by correctness tests.
dotnet run --no-build -c Release --project src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -- --verify-transform-perf-coverage

# List exact case names.
dotnet run --no-build -c Release --project src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -- --list-transform-perf-cases

# Run one exact case or family. --case is a case-insensitive substring filter.
dotnet run --no-build -c Release --project src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -- --targeted-transform-perf --case Box3dForwardEuclidean --output-dir BenchmarkDotNet.Artifacts/results/transform-overloads

# Use --quick for smoke/dogfood runs only; use the default settings for evidence.
dotnet run --no-build -c Release --project src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -- --targeted-transform-perf --quick --case Ray3d --output-dir BenchmarkDotNet.Artifacts/results/transform-overloads-smoke
```

The targeted runner writes a canonical JSON result plus derived CSV and Markdown summaries:

```text
TransformOverloadTargetedPerf-<timestamp>.json
TransformOverloadTargetedPerf-<timestamp>.csv
TransformOverloadTargetedPerf-<timestamp>.md
```

The JSON result includes settings and one row per case with old-path expression, new-overload expression, operation count, old/new ns/op, ratio, and allocation bytes/op. Use JSON for programmatic stats/filtering/regression checks; the Markdown report is a human-readable table derived from the same rows.

Coverage policy:

- Correctness tests remain the authoritative semantic proof.
- The targeted perf registry mirrors the same overload families with direct measured delegates; do not use reflection or `dynamic` inside measured paths.
- `--verify-transform-perf-coverage` guards the registry against missing/extra case names as overload families evolve.
- BenchmarkDotNet classes in this folder are for exact spot confirmation when the targeted runner reports a suspicious or borderline case.

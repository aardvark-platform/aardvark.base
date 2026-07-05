# AI Agent Guide

Repository-specific rules and verified operational facts for coding agents.

Primary AI reference index: `ai/README.md`

## Supported Commands

Use these commands for restore/build/test/codegen:

| Task | Command | Notes |
|------|---------|-------|
| Restore only | `./build.sh restore` or `.\build.cmd restore` | Restores dotnet tools + paket packages, auto-repairing missing Paket targets with `dotnet paket install`, and returns a nonzero exit code on the first failure |
| Build all | `./build.sh` or `.\build.cmd` | Builds `src/Aardvark.sln` and stops on the first failing step |
| Build one project | `dotnet build src/Aardvark.Base/Aardvark.Base.csproj -c Debug` | Use explicit project path |
| Test all | `./test.sh` or `.\test.cmd` | Runs the five maintained test projects, stopping on the first failing step; excludes benchmark projects and the deprecated incremental test project |
| Test one project | `dotnet test src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj -c Debug` | Prefer this over whole-solution test |
| Test with filter | `dotnet test src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj --filter "FullyQualifiedName~Vector"` | Works with NUnit adapter; use a concrete test project |
| Verify transform perf coverage | `dotnet run --no-build -c Release --project src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -- --verify-transform-perf-coverage` | Checks targeted transform perf registry covers the correctness-test overload matrix |
| Benchmark one targeted subset | `dotnet run --no-build -c Release --project src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -- --targeted-transform-perf --case Box3dForwardEuclidean` | Build dependencies serially first, then rerun exact perf cases with `--no-build`; add `--quick` only for smoke/dogfood runs |
| Codegen | `./generate.sh` or `.\generate.cmd` | Required after template changes |
| Check docs drift | `./check-docs.sh` or `.\check-docs.cmd` | Validates docs against source anchors and anti-drift rules |

## Performance Benchmarking

- Design benchmark fixtures so they can be executed independently in the smallest useful subset right now
- Prefer one benchmark class or targeted perf case per exact specialization rather than monolithic benchmark types
- When iterating, run only the exact perf case you changed before expanding to broader perf coverage
- For repeated perf runs, build dependencies serially once:
  `dotnet build src/Aardvark.Base/Aardvark.Base.csproj -c Release`
  `dotnet build src/Tests/Aardvark.Base.Benchmarks/Aardvark.Base.Benchmarks.csproj -c Release`
  then execute targeted cases with `--no-build`
- Do not build `Aardvark.Base` and dependent projects like `Aardvark.Base.Benchmarks` in parallel; they share transitive outputs and can collide on `src/Aardvark.Base/obj/...`
- Keep benchmark commands filterable and document representative exact-case commands near the benchmark source when useful
- For transform overload perf, run `--verify-transform-perf-coverage` after changing the registry and inspect generated JSON/Markdown reports when dogfooding timing runs
- Use `Release` for all benchmark runs

## Dependency Management (Paket)

This repo uses Paket, not `dotnet add package`.

| Task | Command |
|------|---------|
| Add package | `dotnet paket add <pkg> --project <proj>` |
| Update group | `dotnet paket update --group Main` |
| Re-resolve lock | `dotnet paket install` |
| Restore packages | `dotnet paket restore` |

Rules:
- Never edit `paket.lock` manually
- Never use `dotnet add package` in this repo
- Change constraints in `paket.dependencies`, then regenerate lock with Paket
- Top-level `build.*` / `test.*` scripts auto-run `dotnet paket install` when `.paket/Paket.Restore.targets` is missing; otherwise they use `dotnet paket restore`
- Top-level `build.*` / `test.*` scripts now stop immediately on the first failing restore/build/test command and return that nonzero exit code

## Release Notes

When a change needs release notes:
- `aardvark.build` reads the first `### <version>` section as the current package version
- unreleased notes may appear above that first version section as plain text or bullet points
- do not add a markdown heading such as `### Preliminary` above the first version section
- do not append new notes inside an older released version block
- if you add pending/preliminary notes, place them above the first version section instead of inside the previous released block

## File Ownership by Change Type

| Change Type | Files to Modify | Files to Avoid |
|-------------|-----------------|----------------|
| Add C# feature | `src/Aardvark.Base/**/*.cs`, `src/Aardvark.Base.IO/**/*.cs`, `src/Aardvark.Base.Essentials/**/*.cs` | `*_auto.cs`, `*_template.cs` unless you are updating generation templates intentionally |
| Add F# feature | `src/Aardvark.Base.FSharp/**/*.fs`, `src/Aardvark.Base.Incremental/**/*.fs`, `src/Aardvark.Base.Tensors/**/*.fs`, `src/Aardvark.Geometry/**/*.fs`, `src/Aardvark.Base.Runtime/**/*.fs`, `src/Aardvark.Base.Fonts/**/*.fs` | Generated `*_auto.fs` unless template-driven change |
| Add tests | `src/Tests/**/*Tests.cs`, `src/Tests/**/*Tests.fs` | Unrelated production modules |
| Fix bug | Smallest relevant source area + regression test | Broad refactors without tests |
| Update dependencies | `paket.dependencies` (+ Paket-generated lock update) | Manual lock edits |

## Cross-Language Reality (Important)

Do not assume strict one-way C#/F# dependency rules here. This solution intentionally mixes languages:

- F# projects reference C# projects
- Some C# projects also reference F# projects (for example `Aardvark.Base.IO` references `Aardvark.Base.Tensors.fsproj`)

Guideline for agents:
- Preserve existing dependency direction used by neighboring projects
- If introducing a new reference, check existing project patterns first
- Avoid creating dependency cycles

## Framework, SDK, and Project Matrix

- SDK pin: `.NET SDK 8.0.0` with `rollForward: latestFeature` (see `global.json`)
- Not all projects target the same framework

Current project reality:
- Mixed `net8.0;netstandard2.0`: `Aardvark.Base`, `Aardvark.Base.FSharp`, `Aardvark.Base.IO`
- `netstandard2.0` only: `Aardvark.Base.Essentials`, `Aardvark.Base.Telemetry`, `Aardvark.Base.Tensors`, `Aardvark.Base.Tensors.CSharp`, `Aardvark.Base.Runtime`, `Aardvark.Base.Fonts`, `Aardvark.Geometry`
- `net8.0` test/demo projects: most projects in `src/Tests` and `src/Demo`
- Deprecated legacy test project: `src/Tests/Aardvark.Base.Incremental.Tests/Aardvark.Base.Incremental.Tests.fsproj` still targets `netcoreapp3.0`, is intentionally excluded from `src/Aardvark.sln` and the standard `test.sh` / `test.cmd` path, and is tracked for removal-or-migration in GitHub issue `#94`
- C# language version is not uniform (`12.0` in `Aardvark.Base`, `10.0` in some other C# projects)

## Common Failure Modes

| Symptom | Cause | Fix |
|---------|-------|-----|
| `dotnet paket restore` fails | Paket/tool state mismatch | `dotnet tool restore` then `dotnet paket restore`; if `.paket/Paket.Restore.targets` is missing or scripts are not being used, run `dotnet paket install` |
| Compile errors in generated files | Template/output out of sync | Run `./generate.sh` or `.\generate.cmd` |
| Build fails due framework mismatch | Running old SDK/runtime | Install .NET 8 SDK; verify `dotnet --info` and `global.json` |
| `CS2012` / cannot open `Aardvark.Base.dll` for writing | Parallel builds or a still-running benchmark process is holding the dependency output | Stop the active process, build dependent projects serially, then rerun targeted perf commands with `--no-build` |
| Test filter returns zero tests | Wrong filter syntax | Use `FullyQualifiedName~...` pattern |
| Docs check fails | Broken links, stale examples, missing anchors, mojibake | Run `./check-docs.sh` or `.\check-docs.cmd` and fix the reported file/pattern |
| Rendering namespace not found | Wrong package assumption | `Aardvark.Rendering` comes from downstream package, not this repo |

## Project Structure

```text
src/
|- Aardvark.Base/                (core C# math/geometry/types)
|- Aardvark.Base.Essentials/     (additional C# utilities)
|- Aardvark.Base.IO/             (I/O and image codecs; C#)
|- Aardvark.Base.Telemetry/      (telemetry probes; C#)
|- Aardvark.Base.Tensors.CSharp/ (C# tensor/piximage layer)
|- Aardvark.Base.FSharp/         (F# functional extensions)
|- Aardvark.Base.Incremental/    (adaptive system; F#)
|- Aardvark.Base.Tensors/        (tensor features; F#)
|- Aardvark.Base.Runtime/        (runtime helpers; F#)
|- Aardvark.Base.Fonts/          (font/text support; F#)
|- Aardvark.Geometry/            (geometry algorithms; F#)
|- Tests/                        (C# + F# test projects)
|- Demo/                         (sample apps)
|- CodeGenerator/                (template/code generation tooling)
```

## AI Reference Docs

See `ai/README.md` for task-based lookup across:
- primitive math/geometry types
- linear algebra semantics (layout/interoperability)
- geometry semantics (transform conventions)
- piximage/tensor APIs
- algorithms and collections
- serialization
- utilities
- F# interop
- incremental/adaptive system

## Agent Workflow Tips

1. Read only the doc needed for your task, not all docs
2. Verify API names with `rg` before using examples from docs
3. Prefer local source as the final truth if docs and code disagree
4. Run focused tests for touched modules before broad test runs
5. When changing templates, regenerate before building
6. When changing docs, run `./check-docs.sh` or `.\check-docs.cmd`

---

Last updated: 2026-02-26

# Contributing to Aardvark.Base

This guide is aligned with the current repository scripts and project layout.

## Prerequisites

- .NET SDK from `global.json` (`8.0.0` with `rollForward: latestFeature`)
- Git

## Restore

```bash
# Windows
.\build.cmd restore

# Linux/macOS
./build.sh restore
```

This restores local tools and Paket dependencies.

## Build

```bash
# Windows
.\build.cmd

# Linux/macOS
./build.sh
```

Builds `src/Aardvark.sln`.

Build one project directly with `dotnet`:

```bash
dotnet build src/Aardvark.Base/Aardvark.Base.csproj -c Debug
```

## Test

Run all tests:

```bash
# Windows
.\test.cmd

# Linux/macOS
./test.sh
```

The standard test scripts restore tools/packages and then run only the maintained test projects:

- `src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj`
- `src/Tests/Aardvark.Base.Runtime.Tests/Aardvark.Base.Runtime.Tests.fsproj`
- `src/Tests/Aardvark.Base.Fonts.Tests/Aardvark.Base.Fonts.Tests.fsproj`
- `src/Tests/Aardvark.Geometry.Tests/Aardvark.Geometry.Tests.fsproj`
- `src/Tests/Aardvark.Base.FSharp.Tests/Aardvark.Base.FSharp.Tests.fsproj`

The benchmark projects are intentionally excluded from the default `test.sh` / `test.cmd` path.
The legacy incremental test project (`src/Tests/Aardvark.Base.Incremental.Tests/Aardvark.Base.Incremental.Tests.fsproj`) is also intentionally excluded for now. It still depends on an older adaptive test/helper surface and is tracked for explicit removal-or-migration in GitHub issue `#94`, rather than being silently treated as part of the normal green test suite.

Run focused tests directly with `dotnet test`:

```bash
dotnet test src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj -c Debug
dotnet test src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj --filter "FullyQualifiedName~Vector"
dotnet test src/Aardvark.sln --filter "FullyQualifiedName~Vector"
```

Use the solution-level filtered form only for targeted discovery/debugging. The default all-tests path remains `./test.sh` / `.\test.cmd`, which now skips benchmark projects.

## Code Generation

If you changed templates (`*_template.cs` / `*_template.fs`), regenerate:

```bash
# Windows
.\generate.cmd
.\generate.cmd --force

# Linux/macOS
./generate.sh
./generate.sh --force
```

Do not edit generated `*_auto.cs` / `*_auto.fs` manually.
The scripts forward generator CLI arguments, so `--force` can be used for a full regeneration pass.
CI uses forced regeneration in build and publish workflows to detect stale generated files before packages are produced.

## Publish

The publish workflow parses the top version from `RELEASE_NOTES.md` with `dotnet aardpack --parse-only`.
If that version already matches the latest GitHub Release, the workflow exits after the release check and skips package build, upload, and push steps.
Add a new top-level release-notes version when a new package release should be published.

## Dependency Management

This repository uses Paket.

- Add package: `dotnet paket add <pkg> --project <proj>`
- Update lock from dependency changes: `dotnet paket install` or `dotnet paket update`

Do not use `dotnet add package`.

## Release Notes

If a change needs a release-notes entry:

- `aardvark.build` reads the first `### <version>` section as the current package version
- unreleased notes may appear above that first version section as plain text or bullet points
- do not add a markdown heading such as `### Preliminary` above the first version section
- do not place new unreleased entries inside an already released version block
- if you add pending/preliminary notes, place them above the first version section instead of inside the previous released block

## Pull Request Checklist

1. Build succeeds (`./build.sh` or `.\build.cmd`)
2. Relevant tests pass (prefer targeted `dotnet test ...`)
3. Codegen rerun when templates changed
4. Docs updated if behavior/API changed
5. If docs changed, run `./check-docs.sh` or `.\check-docs.cmd`

## Docs Checker Failure Classes

When `check-docs` fails, common categories are:

1. Broken markdown links in `AGENTS.md`, `ai/*.md`, or `docs/*.md`
2. Stale command/API examples caught by forbidden-pattern rules
3. Missing required semantic phrases in key docs
4. Source-anchor drift (doc claims no longer match source snippets)
5. Mojibake/encoding artifacts in markdown files

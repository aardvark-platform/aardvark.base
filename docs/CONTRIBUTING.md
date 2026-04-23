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

Run focused tests directly with `dotnet test`:

```bash
dotnet test src/Tests/Aardvark.Base.Tests/Aardvark.Base.Tests.csproj -c Debug
dotnet test src/Aardvark.sln --filter "FullyQualifiedName~Vector"
```

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

## Dependency Management

This repository uses Paket.

- Add package: `dotnet paket add <pkg> --project <proj>`
- Update lock from dependency changes: `dotnet paket install` or `dotnet paket update`

Do not use `dotnet add package`.

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

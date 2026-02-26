# F#/C# Interop Notes for Aardvark.Base

This repository intentionally uses mixed C#/F# project references.

## Dependency Reality

Do not assume strict one-way language layering.

Current solution contains both:

- F# projects referencing C# projects
- C# projects referencing F# projects in specific areas

When adding a project reference:

1. Check neighboring project patterns first.
2. Avoid introducing dependency cycles.
3. Keep changes local to the feature area.

## API Surface Guidance

- Keep public API naming and behavior stable across C# and F# entry points.
- For C# consumers of F# types (for example `FSharpOption<T>`), provide wrapper helpers where practical.
- For F# convenience wrappers (`Vec`, `Mat`, `Trafo`), keep names idiomatic while preserving core semantics.

## Async Interop

- C#: `Task` / `Task<T>`
- F#: `Async<'T>`

Use explicit conversions at boundaries (`Async.StartAsTask`, `Async.AwaitTask`) instead of implicit assumptions.

## Collections and Options

Interop-heavy boundaries should prefer explicit conversions:

- `Option<T>` <-> nullable/wrapper methods
- F# collections <-> `System.Collections.Generic` where needed

## Verification Workflow

Before documenting or changing interop behavior:

1. Verify project references in `src/*.csproj` and `src/*.fsproj`.
2. Validate API names in source with `rg`.
3. Keep docs aligned with real project dependency direction.

# AI Agent Guide

This repository has AI-targeted reference documentation in `ai/README.md` for types, algorithms, and utilities.

## Supported Commands

Only use these commands for restore/build/test:

| Task | Command | Notes |
|------|---------|-------|
| Restore only | `./build.sh restore` (Linux/Mac) or `.\build.cmd restore` (Windows) | Restore tools + packages |
| Build all | `./build.sh` or `.\build.cmd` | Builds entire solution |
| Build one | `dotnet build src/PROJECT.sln -c Debug` | Single project build |
| Test all | `./test.sh` or `.\test.cmd` | Runs all tests |
| Test one | `dotnet test src/Tests/PROJECT/PROJECT.csproj -c Debug` | Single test project |
| Test filter | `dotnet test src/Aardvark.sln --filter "TestClass=VectorTests"` | NUnit filter |
| Codegen | `./generate.sh` or `.\generate.cmd` | After template changes |

## Dependency Management (Paket)

This project uses **Paket**, not NuGet PackageReference.

| Task | Command |
|------|---------|
| Add package | `dotnet paket add <pkg> --project <proj>` |
| Update group | `dotnet paket update --group Main` |
| Update lock | `dotnet paket install` |
| Restore only | `dotnet paket restore` |

**Rules:**
- Never edit `paket.lock` manually; always use `dotnet paket` commands
- Never use `dotnet add package`; use `dotnet paket add` instead
- `paket.dependencies` declares constraints; `paket.lock` is auto-generated

## File Ownership by Change Type

| Change Type | Files to Modify | Files to NOT Touch |
|-------------|-----------------|-------------------|
| Add C# feature | `src/Aardvark.Base/**/*.cs` | `*_auto.cs` (generated), `*_template.cs` |
| Add F# feature | `src/Aardvark.Base.FSharp/**/*.fs` | C# projects, generated files |
| Add test | `src/Tests/**/*Tests.cs` or `src/Tests/**/*Tests.fs` | Source code, other test suites |
| Fix bug | Relevant source + add/update test | Unrelated modules |
| Update deps | `paket.dependencies` | `paket.lock` (auto-updated) |

## Cross-Language Rules

- **F# → C#**: F# projects MAY reference C# projects (e.g., FSharp project → Base project)
- **C# → F#**: C# projects MUST NOT reference F# projects (dependency direction)
- **Public API**: Lives in C# base layer (`Aardvark.Base`, `Aardvark.Base.Essentials`)
- **Functional Extensions**: F# provides extension methods/utilities on C# types

## Framework & SDK Rules

- **.NET Version**: 8.0 (see `global.json`)
- **Target Frameworks**: `netstandard2.0` (compatibility) + `net8.0` (modern)
- **LangVersion**: 12 (C#), supports recent features
- **Unsafe Code**: Allowed in math/imaging modules (declared per-project)

## Common Failure Modes & Fixes

| Symptom | Cause | Fix |
|---------|-------|-----|
| `dotnet paket restore` fails | Outdated lock file | `dotnet paket update` to regenerate |
| SDK not found error | Wrong .NET version | Install .NET 8.0.x; check `global.json` |
| `*_auto.cs` or `*_auto.fs` compile errors | Tensor/template generation out of sync | Run `./generate.sh` or `.\generate.cmd` |
| F# build fails before C# | Wrong project dependency order | F# projects must reference C# projects, not vice versa |
| Cross-platform line ending issues | Inconsistent CRLF/LF | Use `.editorconfig` and `.gitattributes` settings |
| `paket.lock` merge conflicts | Manual edits to lock file | Resolve in source, regenerate: `dotnet paket install` |
| `Aardvark.Base.Rendering` not found | Wrong namespace | Use `Aardvark.Rendering` (separate package) |
| Type mismatch `V3d` vs `V3f` | Precision confusion | Check API requirements; prefer `d` (double) types |

## Project Structure

```
src/
├── Aardvark.Base/           # Core C# mathematics, geometry, types
├── Aardvark.Base.Essentials # Additional C# utilities
├── Aardvark.Base.FSharp/    # F# functional extensions
├── Aardvark.Base.Incremental # Reactive programming (F#)
├── Aardvark.Base.IO/        # File I/O, image loading (C#)
├── Aardvark.Base.Telemetry/ # Performance monitoring (C#)
├── Aardvark.Base.Tensors/   # N-dimensional arrays (F#)
├── Aardvark.Geometry/       # Advanced geometry algorithms (F#)
├── Tests/                   # Test projects (C# + F#)
├── Demo/                    # Examples and demos
└── CodeGenerator/           # Build-time code generation
```

## Reference Documentation

See `ai/README.md` for indexed type/algorithm docs. Quick links by task:

| Task | Doc |
|------|-----|
| Vectors, matrices, colors, transforms | [PRIMITIVE_TYPES.md](ai/PRIMITIVE_TYPES.md) |
| Images: load, save, scale, formats | [PIXIMAGE.md](ai/PIXIMAGE.md) |
| N-D arrays, tensors, stride views | [TENSORS.md](ai/TENSORS.md) |
| Graphs, spatial, numerical algorithms | [ALGORITHMS.md](ai/ALGORITHMS.md) |
| Binary/stream serialization | [SERIALIZATION.md](ai/SERIALIZATION.md) |
| Logging, telemetry, random, geodesy | [UTILITIES.md](ai/UTILITIES.md) |
| Symbols, caches, specialized dicts | [COLLECTIONS.md](ai/COLLECTIONS.md) |
| F# modules, lenses, interop | [FSHARP_INTEROP.md](ai/FSHARP_INTEROP.md) |
| Reactive programming, adaptive values | [INCREMENTAL.md](ai/INCREMENTAL.md) |

## Related Packages (Not in This Repo)

Aardvark.Base is the foundation layer. Downstream packages build on it:

| Package | Purpose | Repo |
|---------|---------|------|
| Aardvark.Rendering | GPU rendering, shaders, scene graphs | aardvark.rendering |
| Aardvark.UI | Elm-architecture web UI framework | aardvark.media |
| Aardvark.Data.* | File format loaders (OBJ, GLTF, IFC) | aardvark.data |
| Aardvark.Geometry.* | Advanced geometry (PolyMesh, PointTree) | aardvark.algodat |
| Adaptify | F# lens/adaptive code generation | adaptify |

**Note**: `Aardvark.Rendering` is NOT `Aardvark.Base.Rendering`. The `Rendering` namespace is in a separate package.

## Version Compatibility

| Aardvark.Base | .NET SDK | Target Frameworks | Notes |
|--------------|----------|-------------------|-------|
| 5.3.x | 8.0+ | netstandard2.0, net8.0 | Current |
| 5.2.x | 6.0+ | netstandard2.0, net6.0 | Legacy |

## Tips for AI Agents

1. **Read only what you need**: Each doc is self-contained; don't read all 7 at once
2. **Check cross-references**: Docs link to related content (see "See Also" sections)
3. **Know the gotchas**: Each doc has a "Gotchas" section with common mistakes
4. **Verify framework before coding**: Check global.json and project files for constraints
5. **Run tests after changes**: CI validates on Linux/Windows/macOS; test locally first
6. **Follow the build**: Use provided scripts; they handle tool restore and Paket

---

**Last updated**: December 2025

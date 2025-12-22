# Recipe: Making a Repository AI-Friendly

Step-by-step guide for Claude instances to add AI-optimized documentation to similar codebases.

---

## Overview

This recipe transforms a typical .NET/F# repository into one that AI coding assistants can navigate and modify efficiently. The pattern was extracted from commits that added ~3300 lines of targeted documentation to Aardvark.Base.

**Time estimate**: 2-4 hours for a medium codebase (50-200 source files).

---

## Step 1: Add `.editorconfig`

Consistent formatting reduces diff noise and prevents AI-generated code from introducing style conflicts.

```ini
root = true

[*]
charset = utf-8
insert_final_newline = true
trim_trailing_whitespace = true
end_of_line = lf

[*.{cs,fs,fsx,fsi}]
indent_style = space
indent_size = 4

[*.{csproj,fsproj,props,targets,sln}]
indent_style = space
indent_size = 2

[*.{json,yml,yaml,xml}]
indent_style = space
indent_size = 2

[*.md]
trim_trailing_whitespace = false

[*.sh]
end_of_line = lf

[*.cmd]
end_of_line = crlf
```

---

## Step 2: Add `.gitattributes`

Cross-platform line endings prevent spurious diffs when AI generates code on different OSes.

```
* text=auto

# Source code
*.cs text eol=lf
*.fs text eol=lf
*.fsx text eol=lf
*.fsi text eol=lf

# Build files
*.csproj text eol=lf
*.fsproj text eol=lf
*.sln text eol=lf
*.props text eol=lf
*.targets text eol=lf

# Config
*.json text eol=lf
*.yml text eol=lf
*.yaml text eol=lf

# Docs
*.md text eol=lf
*.txt text eol=lf

# Scripts
*.sh text eol=lf
*.cmd text eol=crlf
*.bat text eol=crlf

# Binary
*.png binary
*.jpg binary
*.dll binary
*.exe binary
*.pdb binary
*.nupkg binary
*.zip binary
```

---

## Step 3: Create `AGENTS.md` at Repo Root

This is the entry point for AI agents. Include:

1. **Link to detailed docs** (if any)
2. **Supported commands table** (build, test, restore)
3. **Dependency management rules** (npm, pip, NuGet, Paket, etc.)
4. **File ownership by change type**
5. **Framework/SDK constraints**
6. **Common failure modes with fixes**
7. **Project structure overview**
8. **Tips for AI agents**

### Template

```markdown
# AI Agent Guide

This repository has AI-targeted reference documentation in `ai/README.md`.

## Supported Commands

| Task | Command | Notes |
|------|---------|-------|
| Restore | `./build.sh restore` or `.\build.cmd restore` | Restores tools + packages |
| Build | `./build.sh` or `.\build.cmd` | Builds entire solution |
| Test | `./test.sh` or `.\test.cmd` | Runs all tests |
| Build one | `dotnet build src/Foo/Foo.csproj` | Single project |

## Dependency Management

| Task | Command |
|------|---------|
| Add package | `<your package manager command>` |
| Update | `<your update command>` |

**Rules:**
- <package manager specific rules>

## File Ownership by Change Type

| Change Type | Files to Modify | Files to NOT Touch |
|-------------|-----------------|-------------------|
| Add feature | `src/**/*.cs` | `*_auto.cs` (generated) |
| Add test | `tests/**/*Tests.cs` | Source files, other tests |
| Fix bug | Relevant source + test | Unrelated modules |

## Framework & SDK Rules

- **.NET Version**: X.Y (see `global.json`)
- **Target Frameworks**: netstandard2.0, net8.0
- **LangVersion**: 12

## Common Failure Modes & Fixes

| Symptom | Cause | Fix |
|---------|-------|-----|
| Package restore fails | Outdated lock file | `<regenerate command>` |
| SDK not found | Wrong .NET version | Install .NET X.Y |

## Project Structure

```
src/
├── Core/           # Main library
├── Extensions/     # Optional modules
└── Tests/          # Test projects
```

## Tips for AI Agents

1. Read only what you need; each doc is self-contained
2. Check the "Gotchas" section before writing code
3. Run tests after changes
4. Use provided build scripts
```

---

## Step 4: Create `ai/README.md` Index

This is a compact index so the AI knows which doc to read for each task.

### Template

```markdown
# <Project> AI Reference

Index for AI coding assistants. Read only the doc you need.

## By Task

| Task | Document | Size |
|------|----------|------|
| Core types, APIs | CORE.md | ~10 KB |
| Data structures | DATA_STRUCTURES.md | ~8 KB |
| Algorithms | ALGORITHMS.md | ~8 KB |
| Serialization | SERIALIZATION.md | ~7 KB |
| Configuration | CONFIG.md | ~5 KB |

## By Type

- `FooClass`, `BarStruct` → CORE.md
- `MyCollection<T>` → DATA_STRUCTURES.md
```

**Key rules:**
- Include approximate file sizes (AI can estimate reading cost)
- Group by task AND by type name
- Keep it under 50 lines

---

## Step 5: Create Topic-Specific Reference Docs in `ai/`

Each doc covers one topic. Target 6-12 KB per doc (500-1000 lines).

### Document Structure

```markdown
# <Project> <Topic> Reference

AI-targeted reference for <topic description>.

---

## <Major Section 1>

### <Subsection>

| Type | Properties | Notes |
|------|------------|-------|
| Foo | Bar, Baz | Description |

### Usage
```<lang>
// Example code
```

---

## <Major Section 2>

...

---

## Usage Patterns

### Pattern Name
```<lang>
// Complete working example
```

---

## Gotchas

1. **Gotcha Title**: Explanation of the trap and how to avoid it
2. **Another Gotcha**: ...

---

## See Also

- [OTHER_DOC.md](OTHER_DOC.md) - Why this is related
```

### Formatting Rules

1. **Tables over prose** - Types, properties, commands in tables
2. **Code examples** - Complete, copy-paste-ready
3. **Gotchas section** - Common mistakes (3-5 items)
4. **See Also** - Cross-references to related docs
5. **No fluff** - Skip introductions, motivation, history

### Step 5b: F# Considerations

If your repo has F# code alongside C#:

1. **Document `open` statements** - F# uses `open Namespace` instead of `using`. List common opens and what they provide

2. **Cover F# modules** - F# modules (like `Vec`, `Mat`) wrap C# static methods. Document these separately from the C# API

3. **Note function conventions**:
   - F# prefers curried functions: `transformPos matrix point`
   - C# uses tupled: `TransformPos(matrix, point)`
   - Document both forms if available

4. **Dual-language examples** - For key operations, show both:
   ```csharp
   // C#
   var result = V3d.Cross(a, b);
   ```
   ```fsharp
   // F#
   let result = Vec.cross a b
   ```

5. **Document lenses** - If using Adaptify or similar, document the lens system and composition operators

---

## Step 6: Add `.claude/CLAUDE.md` Pointer

Minimal file that tells Claude Code where to find docs.

```markdown
# AI Documentation

Read `ai/README.md` for indexed reference docs.
```

---

## Step 7: Document Complex Build Systems

If your repo has code generation, custom build steps, or unusual tooling, create a README in that component's directory.

### Template for Code Generators

```markdown
# <Generator Name>

## Purpose

<One paragraph explaining what it generates and why>

## How to Run

```bash
./generate.sh   # Unix
.\generate.cmd  # Windows
```

## When to Run

Run after modifying any `*_template.*` file.

## Input Files

`*_template.cs` files containing:
- <syntax description>

## Output Files

`*_auto.cs` files that are:
- AUTO GENERATED - DO NOT EDIT
- <what they contain>

## Generated Files Summary

| Template | Output | Purpose |
|----------|--------|---------|
| Foo_template.cs | Foo_auto.cs | Generates type variants |
```

---

## Step 8: Improve CI Configuration

AI agents benefit from CI that:
- Uses caching (faster feedback loop)
- Sets `fail-fast: false` (see all failures, not just first)
- Has correct path-ignore globs

### GitHub Actions Example

```yaml
name: Build

on:
  push:
    paths-ignore:
      - 'README.md'
      - 'docs/**'
  pull_request:
    paths-ignore:
      - 'README.md'
      - 'docs/**'

jobs:
  build:
    runs-on: ${{ matrix.os }}
    strategy:
      fail-fast: false
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          global-json-file: global.json

      - name: Cache packages
        uses: actions/cache@v4
        with:
          path: |
            ~/.nuget/packages
          key: ${{ runner.os }}-nuget-${{ hashFiles('**/packages.lock.json') }}

      - name: Build
        run: <build command>
```

---

## Checklist

Before considering the repository AI-friendly, verify:

- [ ] `.editorconfig` exists with language-specific rules
- [ ] `.gitattributes` handles line endings and binary files
- [ ] `AGENTS.md` at root with commands, rules, structure
- [ ] `ai/README.md` index with task-based lookup
- [ ] `ai/*.md` docs for each major topic (3-10 docs typical)
- [ ] `.claude/CLAUDE.md` points to ai/README.md
- [ ] Build system READMEs for code generators or unusual tooling
- [ ] CI uses caching and fail-fast: false
- [ ] All `ai/*.md` docs have Gotchas and See Also sections
- [ ] All `ai/*.md` docs are 6-12 KB (not too long to read, not too short to be useful)

---

## Anti-Patterns to Avoid

1. **Walls of prose** - AI skims; tables and code are better
2. **Incomplete examples** - Every code block should work if pasted
3. **Missing gotchas** - The mistakes AI makes repeatedly belong here
4. **Giant monolithic docs** - Split by topic; 6-12 KB per file
5. **Stale docs** - Update when behavior changes; delete obsolete content
6. **Documenting the obvious** - Skip "what is a vector"; focus on API specifics
7. **Duplicating source comments** - Reference docs should add value beyond inline docs

---

## Maintenance

When making changes to the codebase:

1. **New types/APIs**: Update relevant `ai/*.md` doc
2. **New failure mode**: Add to AGENTS.md failure table
3. **New build step**: Add to AGENTS.md commands table
4. **Breaking change**: Update Gotchas section
5. **Removing feature**: Delete from docs (don't leave stale references)

---

*Last updated: December 2025*

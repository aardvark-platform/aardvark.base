# CodeGenerator

Expands `*_template.cs`, `*_template.fs`, and `*_template.cl` files into their
`*_auto.*` counterparts using C# directives embedded in comments.

## How to Run

### Using the shell script (Unix/Linux/macOS)
```bash
./generate.sh
./generate.sh --force
```

### Using the command script (Windows)
```cmd
generate.cmd
generate.cmd --force
```

### Using dotnet directly
```bash
dotnet run --project src/CodeGenerator/CodeGenerator.csproj
dotnet run --project src/CodeGenerator/CodeGenerator.csproj -- --force
dotnet run --project src/CodeGenerator/CodeGenerator.csproj -- path/to/tasks.conf
```

Both wrapper scripts forward additional CLI arguments to `CodeGenerator.dll`.

### Using a `.conf` file

When invoked with a single `.conf` argument, CodeGenerator reads one generation task per line:

```text
path/to/template_template.cs path/to/output_auto.cs
path/to/other_template.fs path/to/other_auto.fs
```

Rules:
- Each non-empty row must contain exactly two whitespace-separated paths: `<template> <output>`
- Blank lines are ignored
- Comment-only lines starting with `#` are ignored
- Malformed rows fail with an error message and a nonzero exit code

## When to Run

Run after changing templates and before building or testing. Templates older than
their outputs are skipped. Use `-f` / `--force` to regenerate regardless of timestamps.

The scripts print:
- `#` prefix: Template is being processed
- `-` prefix: Template is skipped (older than output)

## Input Files

Templates under `src/` contain:

1. **Literal text** that appears in the output as-is
2. **Generator directives** in special comments:
   - `/*# ... */` — inline code generation
   - `/*CLASS# ... */` — code appended to a helper class
   - `/*USING# ... */` — using statements for the generator
   - `//# ...` — single-line generation directives
   - `//BEGIN CLASS#` / `//END CLASS#` — switch output to/from class section
3. **Placeholder expressions** using `__ ... __` syntax that are evaluated during generation

Example template snippet:
```csharp
/*# var types = new[] { "int", "float", "double" }; */
/*# foreach(var type in types) { */
    public __type__ Max(__type__ a, __type__ b) => a > b ? a : b;
/*# } */
```

## Output Files

Do not edit generated files manually; regeneration overwrites them.
List them with `rg --files -g '*_auto.*' src`.

Example output (from above template):
```csharp
public int Max(int a, int b) => a > b ? a : b;
public float Max(float a, float b) => a > b ? a : b;
public double Max(double a, double b) => a > b ? a : b;
```

## Implementation

- `Program.cs`: task discovery, configuration, and output files.
- `TemplateProcessor.cs`: translates template directives into a C# generator.
- `CompilerServices.cs`: Roslyn compilation and assembly references.

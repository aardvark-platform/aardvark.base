# CodeGenerator

## Purpose

The CodeGenerator is a code expansion tool that transforms `*_template.cs` files into `*_auto.cs` files with type-specific variants. It uses a custom template syntax embedded in comments to generate repetitive code for multiple numeric types (int, float, double, long, etc.), avoiding manual duplication.

The generator processes template files containing:
- **C# code generation directives** (comment-based syntax)
- **Placeholder expressions** that expand for each numeric type
- **Custom template syntax** with embedding code that generates code

This approach keeps the codebase DRY (Don't Repeat Yourself) by maintaining a single template while automatically producing variants for all supported types.

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
- Malformed rows fail with a clear error message and a nonzero exit code

## When to Run

Run the code generator **immediately after modifying any `*_template.cs` file**. The generator:
- Detects which template files have been modified since their output was last generated
- Only regenerates files that need updating (skips unchanged templates)
- Can regenerate everything with `-f` / `--force`, regardless of timestamps
- Must be run before building/testing to ensure generated code is up-to-date

Use `--force` when you want a deterministic full regeneration pass, for example in CI when verifying that committed `*_auto.*` files match their templates.

The scripts print:
- `#` prefix: Template is being processed (newer than output)
- `-` prefix: Template is unchanged (output is up-to-date, skipped)

## Input Files

Template files matching the pattern `*_template.cs` throughout the `src/` directory. These files contain:

1. **Regular C# code** that appears in the output as-is
2. **Generator directives** in special comments:
   - `/*# ... */` — inline code generation
   - `/*CLASS# ... */` — code appended to a helper class
   - `/*USING# ... */` — using statements for the generated code
   - `//# ... */` — single-line generation directives
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

Generated files matching the pattern `*_auto.cs`. These files:
- **DO NOT EDIT MANUALLY** — all edits are lost on regeneration
- Contain a header comment: `// AUTO GENERATED CODE - DO NOT CHANGE!`
- Are fully expanded variants for each numeric type
- Include all variations generated from the template

Example output (from above template):
```csharp
public int Max(int a, int b) => a > b ? a : b;
public float Max(float a, float b) => a > b ? a : b;
public double Max(double a, double b) => a > b ? a : b;
```

## How It Works (Technical Overview)

### 1. Template Discovery
The CodeGenerator scans the project directory for all `*_template.cs`, `*_template.fs`, and `*_template.cl` files (supporting C#, F#, and OpenCL; no `*_template.cl` files currently exist in this repository).

### 2. Template Parsing
Each template is parsed by `TemplateProcessor` using a custom syntax:
- **Comment-based directives** are extracted and executed as C# code
- **Placeholder expressions** (`__ ... __`) are evaluated and replaced
- **Output sections** (Code, Using, Class) are accumulated

### 3. Generator Creation
A dynamic C# generator class is created from the template:
```csharp
public static class SourceGenerator
{
    public static StringBuilder ___sb = new StringBuilder();
    public static string Generate()
    {
        // Generated code from template here
        return ___sb.ToString();
    }
}
```

### 4. Compilation & Execution
- The generator code is compiled on-the-fly using Roslyn (Microsoft.CodeAnalysis)
- The compiled `Generate()` method is invoked via reflection
- Output is captured and written to the `*_auto.cs` file

### 5. Assembly References
The generator can reference these assemblies during compilation:
- `CodeGenerator.dll`
- `System.Runtime.dll`
- `System.Linq.dll`
- `System.Collections.dll`
- `System.Xml.dll` / `System.Xml.Linq.dll`
- `Aardvark.Base.dll` (for custom types and helpers)

## Generated Files Summary

Below is a representative sample of template → generated file mappings:

| Template | Generated File | Purpose |
|----------|---|---------|
| `src/Aardvark.Base/Math/Vectors/Vector_template.cs` | `Vector_auto.cs` | Vector struct variants (V2i, V3i, V2f, V3f, V2d, V3d, etc.) |
| `src/Aardvark.Base/Math/Trafos/Matrix_template.cs` | `Matrix_auto.cs` | Matrix struct variants for different sizes and types |
| `src/Aardvark.Base/Math/Trafos/Rot3_template.cs` | `Rot3_auto.cs` | 3D rotation struct for multiple numeric types |
| `src/Aardvark.Base.IO/BinaryReadingCoder_template.cs` | `BinaryReadingCoder_auto.cs` | Serialization variants for numeric types |
| `src/Aardvark.Base/Geometry/IntersectionTests_template.cs` | `IntersectionTests_auto.cs` | Geometric intersection test functions for float/double |

The full set of generated files is whatever `*_auto.cs` / `*_auto.fs` matches under `src/` (C# outputs plus three F# outputs: `RangeSet_auto.fs`, `TypeMeta_auto.fs`, `Tuples_auto.fs`). List them with `rg --files -g '*_auto.*' src` instead of relying on a hand-maintained inventory.

## Implementation Details

### TemplateProcessor Class
Located in `TemplateProcessor.cs`, handles:
- Parsing template syntax using a state machine
- Managing output sections (Code, Using, Class)
- Creating the dynamic generator class from template directives
- Compiling and executing the generator

### Program Class
Located in `Program.cs`, handles:
- Scanning directories for template files
- Checking modification times to skip unchanged templates
- Managing generation tasks
- Writing output files and diagnostic reports

### CompilerServices Class
Located in `CompilerServices.cs`, handles:
- Compiling generated C# code on-the-fly using Roslyn
- Resolving assembly references dynamically
- Capturing compilation diagnostics (errors/warnings)

## Example: Generating Vector Types

**Template** (`Vector_template.cs` excerpt):
```csharp
/*# var types = new[] { typeof(int), typeof(float), typeof(double) }; */
/*# foreach(var t in types) {
    var typeName = "V" + dimensions + t.Name[0];
*/
public struct __typeName__
{
    public __t.Name__ X, Y, Z;
}
/*# } */
```

**Generated** (`Vector_auto.cs` excerpt):
```csharp
public struct V3i
{
    public int X, Y, Z;
}
public struct V3f
{
    public float X, Y, Z;
}
public struct V3d
{
    public double X, Y, Z;
}
```

## Notes

- The generator is intentionally lightweight and uses reflection-based runtime generation to keep build times reasonable
- Template syntax is C#-compatible; any valid C# expressions can be used in placeholders
- Output files use "AUTO GENERATED CODE - DO NOT CHANGE!" header to signal automated generation
- The generator is run explicitly via `generate.sh`, `generate.cmd`, or direct `dotnet run` invocation when templates need regeneration

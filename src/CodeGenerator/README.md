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
```

### Using the command script (Windows)
```cmd
generate.cmd
```

### Using dotnet directly
```bash
dotnet run --project src/CodeGenerator/CodeGenerator.csproj
```

## When to Run

Run the code generator **immediately after modifying any `*_template.cs` file**. The generator:
- Detects which template files have been modified since their output was last generated
- Only regenerates files that need updating (skips unchanged templates)
- Must be run before building/testing to ensure generated code is up-to-date

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
The CodeGenerator scans the project directory for all `*_template.cs`, `*_template.fs`, and `*_template.cl` files (supporting C#, F#, and OpenCL).

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

## Complete Generated Files List

All `*_auto.cs` files (97 total):

**Aardvark.Base:**
- Math/Vectors: `Vector_auto.cs`, `VectorIEnumerableExtensions_auto.cs`, `VectorTypeConverter_auto.cs`
- Math/Trafos: `Matrix_auto.cs`, `M33_auto.cs`, `M44_auto.cs`, `Rot2_auto.cs`, `Rot3_auto.cs`, `Affine_auto.cs`, `Euclidean_auto.cs`, `Similarity_auto.cs`, `Scale_auto.cs`, `Shift_auto.cs`, `Trafo_auto.cs`
- Math/Base: `Fun_auto.cs`, `Complex_auto.cs`, `Quaternion_auto.cs`, `AliasTable_auto.cs`
- Math/Colors: `Color_auto.cs`
- Math/RangesBoxes: `Box_auto.cs`, `OrientedBox_auto.cs`
- Math/Interfaces: `ISize_auto.cs`
- Geometry/Types: `Geometry_auto.cs`, `Geometry1i_auto.cs`, `Vector_auto.cs`, `Box_auto.cs`, `Capsule3_auto.cs`, `Circle2_auto.cs`, `Circle3_auto.cs`, `Cone3_auto.cs`, `Conic2_auto.cs`, `Cylinder3_auto.cs`, `Ellipse_auto.cs`, `Ellipse2_auto.cs`, `Ellipse3_auto.cs`, `Hull2_auto.cs`, `Hull3_auto.cs`, `Line2_auto.cs`, `Line3_auto.cs`, `Plane2_auto.cs`, `Plane3_auto.cs`, `Polygon2_auto.cs`, `Polygon3_auto.cs`, `PolygonExtensions_auto.cs`, `IImmutablePolygonExtensions_auto.cs`, `Quad2_auto.cs`, `Quad3_auto.cs`, `Quadric_auto.cs`, `Ray2_auto.cs`, `Ray3_auto.cs`, `Sphere3_auto.cs`, `Torus3_auto.cs`, `Triangle2_auto.cs`, `Triangle3_auto.cs`
- Geometry/Relations: `LinearCombination_auto.cs`, `Orthogonality_auto.cs`, `Parallelism_auto.cs`, `SubPrimitives_auto.cs`
- Geometry/Interfaces: `IBoundingBox_auto.cs`
- Geometry/Algorithms: `Algorithms_auto.cs`, `ClippingFunctions_auto.cs`, `IntersectionTests_auto.cs`, `SpecialPoints_auto.cs`
- Sorting: `Sorting_auto.cs`
- Delegates: `Delegates_auto.cs`
- Extensions: `FuncActionExtensions_auto.cs`, `SequenceExtensions_auto.cs`
- Symbol: `Dict_auto.cs`
- Tup: `Tuples_auto.cs`

**Aardvark.Base.IO:**
- `BinaryReadingCoder_auto.cs`, `BinaryWritingCoder_auto.cs`, `ICoder_auto.cs`, `StreamCodeReader_auto.cs`, `StreamCodeWriter_auto.cs`, `TypeCoder_auto.cs`, `XmlReadingCoder_auto.cs`, `XmlWritingCoder_auto.cs`

**Aardvark.Base.Tensors.CSharp:**
- `Tensor_auto.cs`, `TensorExtensions_auto.cs`, `TensorMathExt_auto.cs`
- Tensors: `Accessors_auto.cs`

**Benchmarks:**
- `Indexers_auto.cs`
- Math: `AngleBetween_auto.cs`, `DistanceRot3_auto.cs`, `IntegerPower_auto.cs`, `PowerOfTwo_auto.cs`, `Rot3GetEuler_auto.cs`, `RotateInto_auto.cs`
- Math/Tensors: `MatrixOrthogonalize_auto.cs`

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
- The generator is re-run on every build to ensure consistency with templates

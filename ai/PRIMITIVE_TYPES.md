# Aardvark.Base Primitive Types Reference

Fast orientation for core math and geometry types in `Aardvark.Base`.

Use this with:
- `SEMANTICS_LINEAR_ALGEBRA.md` for matrix/vector layout and interop details
- `SEMANTICS_GEOMETRY_CORE.md` for geometry conventions and transform semantics

## Naming Convention

Pattern: `{TypeName}{Dimension}{Suffix}`

| Suffix | Meaning |
|--------|---------|
| `i` | `int` |
| `l` | `long` |
| `f` | `float` |
| `d` | `double` |
| `b` | `byte` |

Examples:
- `V3d` = 3D vector (`double`)
- `M44f` = 4x4 matrix (`float`)
- `Box3d` = 3D axis-aligned box (`double`)

## Important Reality

- Core vector/matrix structs are mutable value types (`struct`), not uniformly `readonly struct`.
- For matrix/vector math in 3D, prefer explicit methods (`TransformPos`, `TransformDir`) over ambiguous shorthand.

## Vector Families

Common families:
- `V2*`, `V3*`, `V4*`
- integer and floating-point variants (`i`, `l`, `f`, `d`)

Typical APIs:
```csharp
var a = new V3d(1, 2, 3);
var b = new V3d(4, 5, 6);

var dot = V3d.Dot(a, b);
var cross = V3d.Cross(a, b);
var dist = V3d.Distance(a, b);
var unit = a.Normalized;
```

## Matrix Families

Common families:
- `M22*`, `M23*`, `M33*`, `M34*`, `M44*`

### M44d Construction (Verified)
```csharp
var t = M44d.Translation(new V3d(1, 2, 3));
var s = M44d.Scale(new V3d(2, 2, 2));
var r = M44d.RotationZ(0.5);

var fromRows = M44d.FromRows(
    new V4d(1, 0, 0, 0),
    new V4d(0, 1, 0, 0),
    new V4d(0, 0, 1, 0),
    new V4d(0, 0, 0, 1)
);

var rot = Rot3d.RotationZ(0.5);
var rotAsMatrix = (M44d)rot;
```

### M44d Operations (Verified)
```csharp
var m = M44d.Translation(new V3d(1, 2, 3));

var p = m.TransformPos(new V3d(5, 6, 7));   // includes translation
var d = m.TransformDir(new V3d(0, 1, 0));   // ignores translation

var h = m * new V4d(5, 6, 7, 1);            // valid homogeneous multiply
```

Notes:
- `M44d * V4d` and `V4d * M44d` are defined.
- `M44d * V3d` is not defined.
- `M44d.Transform(V3d)` is not a supported API; use `TransformPos`/`TransformDir`.

## Transformation Types

3D families:
- `Rot3*`, `Shift3*`, `Scale3*`, `Euclidean3*`, `Similarity3*`, `Affine3*`, `Trafo3*`

Typical `Trafo3d` usage:
```csharp
var trafo = Trafo3d.Translation(new V3d(1, 0, 0)) * Trafo3d.Scale(2.0);

var fwd = trafo.Forward;    // M44d
var bwd = trafo.Backward;   // M44d inverse

var p = trafo.TransformPos(new V3d(1, 2, 3));
```

## Geometry Core Families

Common primitives:
- `Box2*`, `Box3*`
- `Ray2*`, `Ray3*`
- `Plane2*`, `Plane3*`
- `Sphere3*`, `Circle2*`, `Circle3*`
- `Triangle2*`, `Triangle3*`
- `Hull2*`, `Hull3*`

Typical APIs:
```csharp
var box = new Box3d(V3d.Zero, V3d.One);
var ray = new Ray3d(V3d.Zero, V3d.XAxis);

var contains = box.Contains(new V3d(0.5, 0.5, 0.5));
var hit = ray.Hits(box, out double t);
```

## Gotchas

1. Matrix convention details (layout, multiplication side, interop) are critical for performance and correctness: use `SEMANTICS_LINEAR_ALGEBRA.md`.
2. `TransformPos` vs `TransformDir` matters for translation handling.
3. Subtle precision loss exists when converting from `d` variants to `f` variants.

## Source Anchors

- `src/Aardvark.Base/Math/Vectors/Vector_auto.cs` (`V3d`)
- `src/Aardvark.Base/Math/Trafos/Matrix_auto.cs` (`M44d`, transforms, operators)
- `src/Aardvark.Base/Math/Trafos/Rot3_auto.cs` (`Rot3d` to `M44d` cast)
- `src/Aardvark.Base/Math/RangesBoxes/Box_auto.cs` (`Box3d`)
- `src/Aardvark.Base/Geometry/Types/Ray/Ray3_auto.cs` (`Ray3d` hit methods)

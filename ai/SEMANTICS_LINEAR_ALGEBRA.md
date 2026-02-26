# Aardvark.Base Linear Algebra Semantics

Use this when matrix/vector layout and interop correctness matter.

## M44d Convention (Verified)

`M44d` fields are named by row/column: `Mrc`.

- `M00..M03` = row 0
- `M10..M13` = row 1
- `M20..M23` = row 2
- `M30..M33` = row 3

`double[]` export order is row-major:

```csharp
var a = (double[])m;
// a = [M00, M01, M02, M03, M10, ... , M33]
```

## Multiplication Semantics

`M44d` supports both:

- `m * v` where `v` is a column vector
- `v * m` where `v` is a row vector

Column-vector form (`m * v`) is the canonical transform style for `TransformPos`/`TransformDir`.

```csharp
V4d c = m * v;
V4d r = v * m;
```

## Point vs Direction

For `M44d`:

- `TransformDir(v)` ignores translation (`w = 0`)
- `TransformPos(p)` applies translation (`w = 1`)
- translation lives in `M03/M13/M23`

## Direct Answer: Row-Major or Column-Major?

Both concerns exist, but they are different:

- In-memory field/array layout is row-major.
- Algebra supports column-vector and row-vector multiplication operators.

If your external system is column-major memory, conversion is required at the boundary.

## Efficient Layout Conversion

### Row-major array -> M44d

```csharp
var m = new M44d(rowMajor16);
```

### Column-major array -> M44d

```csharp
var m = M44d.FromCols(
    new V4d(cm[0], cm[1], cm[2], cm[3]),
    new V4d(cm[4], cm[5], cm[6], cm[7]),
    new V4d(cm[8], cm[9], cm[10], cm[11]),
    new V4d(cm[12], cm[13], cm[14], cm[15])
);
```

### M44d -> column-major array

```csharp
var cm = new[]
{
    m.M00, m.M10, m.M20, m.M30,
    m.M01, m.M11, m.M21, m.M31,
    m.M02, m.M12, m.M22, m.M32,
    m.M03, m.M13, m.M23, m.M33
};
```

### Opposite algebra convention

If the other side interprets transforms with opposite multiplication side, transpose at the boundary:

```csharp
var boundaryMatrix = m.Transposed;
```

For one-off transforms, use `TransposedTransformDir` / `TransposedTransformPos` to avoid manual transpose logic.

## Trafo3d Composition Note

`Trafo3d` multiplication order is intentionally reversed relative to raw `M44d` multiplication:

```csharp
var t = t0 * t1;
// forward = t1.Forward * t0.Forward
```

This is documented in `Trafo_auto.cs` and affects composition assumptions.

## Source Anchors

- `src/Aardvark.Base/Math/Trafos/Matrix_auto.cs` (`M44d`, `FromRows`, `FromCols`, `operator*`, `TransformPos`, `TransposedTransformPos`)
- `src/Aardvark.Base/Math/Trafos/Trafo_auto.cs` (`Trafo3d` operator `*` composition semantics)

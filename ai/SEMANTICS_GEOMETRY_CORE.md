# Aardvark.Base Geometry Semantics

Use this for transform correctness questions in geometry code.

## Transform Semantics

For `M44d` and `Trafo3d`:

- `TransformPos` treats input as point (`w=1`)
- `TransformDir` treats input as direction (`w=0`)
- `TransformPosProj` performs perspective division

Use the right method for normals/vectors/points. Most correctness bugs come from mixing them.

## Trafo3d Forward/Backward

`Trafo3d` stores both matrices:

- `Forward`: model -> transformed space
- `Backward`: inverse transform

`InvTransformPos` and related methods use `Backward`.

## Trafo3d Multiplication Order

`Trafo3d` composition is backward relative to raw matrix multiplication for natural postfix usage.

```csharp
var combined = t0 * t1; // not equivalent to new Trafo3d(t0.Forward * t1.Forward, ...)
```

For ambiguity-sensitive code, inspect resulting `Forward` explicitly.

## Matrix and Geometry Interop

When crossing APIs that use transposed conventions, use:

- `m.Transposed`
- `m.TransposedTransformPos(...)`
- `m.TransposedTransformDir(...)`

Do not assume identical handedness or memory/algebra conventions across systems.

## Geodesy Units (Geo)

`Geo.XyzFromLonLatHeight` and `Geo.LonLatHeightFromXyz` use:

- longitude/latitude in degrees
- height in meters
- ellipsoid from `GeoEllipsoid` (`Wgs84`, `Grs80`, `Bessel1841`, ...)

## Source Anchors

- `src/Aardvark.Base/Math/Trafos/Matrix_auto.cs` (`TransformPos`, `TransformDir`, transposed variants)
- `src/Aardvark.Base/Math/Trafos/Trafo_auto.cs` (`Trafo3d`, `Forward`, `Backward`, operator `*`)
- `src/Aardvark.Base/Geodesy/GeoConversion.cs` (`XyzFromLonLatHeight`, `LonLatHeightFromXyz`)
- `src/Aardvark.Base/Geodesy/GeoConsts.cs` (`GeoEllipsoid`)

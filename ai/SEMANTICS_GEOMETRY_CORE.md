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

## Geometry Transform Overloads

For the geometry value types touched by issue 57, the canonical transform semantics are:

- boxes still compute axis-aligned bounds from linear coefficients plus translation; the specialized overloads now use direct specialized paths except `Box3*.Transformed(Affine3*)`, which intentionally stays on the homogeneous-matrix path because repeated `Release` measurements did not beat that baseline
- hulls keep the same inverse-transpose normal semantics as the `Trafo*` path, but the specialized overloads now apply the equivalent direct normal/point math without first constructing a `Trafo*`
- planes keep the same coefficient semantics as the `Trafo*` path, but the specialized overloads now use direct formulas for similarity/affine/shift/rotation/scale instead of first constructing a `Trafo*`
- rays use position-vs-direction aware transform helpers directly (`TransformPos`/`TransformDir`, `InvTransformPos`/`InvTransformDir`)
- inverse convenience APIs are intentionally available on the touched types for `Trafo*`, `Euclidean*`, `Similarity*`, `Shift*`, `Rot*`, and `Scale*`; raw matrices remain forward-only convenience APIs

When a direct specialization cannot be kept both correct and performance-competitive, the source keeps the indirect implementation with an explicit comment documenting the retained fallback.

There is no `Rigid2d`/`Rigid3d` public transform type in this repo. Use `Euclidean2d`/`Euclidean3d` instead when mapping older issue text to the current API.

## Geometry Transform Performance Workflow

Use `Release` for transform overload performance work. Start with the smallest relevant targeted run, then broaden only when the local question requires it.

```powershell
dotnet build src\Tests\Aardvark.Base.Benchmarks\Aardvark.Base.Benchmarks.csproj -c Release -p:BuildInParallel=false --no-restore
dotnet run --no-build -c Release --project src\Tests\Aardvark.Base.Benchmarks\Aardvark.Base.Benchmarks.csproj -- --list-transform-perf-cases
dotnet run --no-build -c Release --project src\Tests\Aardvark.Base.Benchmarks\Aardvark.Base.Benchmarks.csproj -- --targeted-transform-perf --case Plane3dForwardEuclidean
```

The `--case` value is a substring filter. Prefer a single function name while iterating, a family name such as `Plane` for final local evidence, and BenchmarkDotNet filters only for confirmation of specific suspicious cases.

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

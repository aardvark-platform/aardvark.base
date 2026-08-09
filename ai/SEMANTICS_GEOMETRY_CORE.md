# Aardvark.Base Geometry Semantics

Conventions for geometric queries and transforms in geometry code.

## Transform Semantics

For `M44d` and `Trafo3d`:

- `TransformPos` treats input as point (`w=1`)
- `TransformDir` treats input as direction (`w=0`)
- `TransformPosProj` performs perspective division

`Trafo3d` stores both matrices: `Forward` (model -> transformed space) and `Backward` (inverse). `InvTransformPos` and related methods use `Backward`.

Layout, multiplication side, interop conversion, and `Trafo3d` composition order are covered in `SEMANTICS_LINEAR_ALGEBRA.md`.

## Intersection Receiver Conventions

Which type carries the intersection method is fixed and not symmetric:

- Ray vs. box: extension on the **box** — `box.Intersects(ray, out double t)` (`IntersectionTests_auto.cs`). There is no `ray.Intersects(box, ...)` and no `ray.Hits(box, ...)`.
- Ray vs. primitive: instance methods on the **ray** — `ray.Hits(triangle, out t)`, with overloads for `Ray3d`, `Triangle3d`, `Quad3d`, `Sphere3d`, `Circle3d`, `Cylinder3d`, each also available with `(tmin, tmax, ref RayHit3d hit)` range clamping (`Ray3_auto.cs`).
- `RayHit3d` accumulates the closest hit: `Hits` only updates `hit` when the new `t` is inside `[tmin, tmax)` and smaller than `hit.T`; initialize with `RayHit3d.MaxRange`.

```csharp
var box = new Box3d(V3d.Zero, V3d.One);
var ray = new Ray3d(new V3d(0.5, 0.5, -1), V3d.ZAxis);

bool hitsBox = box.Intersects(ray, out double t);

var hit = RayHit3d.MaxRange;
bool hitsTri = ray.Hits(triangle, 0.0, double.MaxValue, ref hit);
```

## FastRay Slab Test

`FastRay2d`/`FastRay2f`/`FastRay3d`/`FastRay3f` wrap a ray with precomputed `InvDir` and `DirFlags` for repeated axis-aligned box tests (kd-tree/BbTree traversal):

```csharp
var fast = new FastRay3d(ray);
double tmin = 0.0, tmax = double.MaxValue;
bool hits = fast.Intersects(box, ref tmin, ref tmax);
// on success, [tmin, tmax] is narrowed to the parameter interval inside the box
```

Semantics (`Ray3_auto.cs`, `FastRay3d.Intersects`):

- `tmin`/`tmax` are in/out: they seed the search interval and are narrowed to the intersection interval on success.
- Slab overlap is inclusive: a far endpoint is rejected only below `tmin`, and a near endpoint only above `tmax`. Exact endpoint, edge, and corner contacts therefore count as hits.
- Boxes may be degenerate in one or more axes (`Min == Max`), including point boxes. A hit can narrow the interval to a single parameter with `tmin == tmax`.
- Axes with zero direction components are handled via `DirFlags`; the ray origin must lie between the slabs of such an axis for a hit.

## Convex Polygon Line Clipping

`Line2f.ClipWithConvex` and `Line2d.ClipWithConvex` clip a segment against a convex polygon whose points are ordered counter-clockwise:

- Each non-zero polygon edge defines an inclusive left half-plane; duplicate consecutive points and other zero-length edges are ignored.
- The default overload uses `Constant<float/double>.PositiveTinyValue` as an absolute point-distance tolerance. The explicit overload accepts a non-negative absolute epsilon. Internally, that distance is multiplied by each edge length before signed-cross-product comparisons.
- Boundary-collinear segments and single-point vertex contacts are retained.
- The result keeps the input `P0`-to-`P1` direction. An endpoint that does not require clipping is returned bit-for-bit unchanged.
- If the segment has no non-empty parameter interval inside the polygon, both result points are NaN.

## Source Anchors

- `src/Aardvark.Base/Math/Trafos/Matrix_auto.cs` (`TransformPos`, `TransformDir`, `TransformPosProj`)
- `src/Aardvark.Base/Math/Trafos/Trafo_auto.cs` (`Trafo3d`, `Forward`, `Backward`)
- `src/Aardvark.Base/Geometry/IntersectionTests_auto.cs` (`Box3d.Intersects(Ray3d, out t)`)
- `src/Aardvark.Base/Geometry/Types/Ray/Ray3_auto.cs` (`Ray3d.Hits` overloads, `RayHit3d`, `FastRay3d`)
- `src/Aardvark.Base/Geometry/ClippingFunctions_auto.cs` (`Line2f.ClipWithConvex`, `Line2d.ClipWithConvex`)

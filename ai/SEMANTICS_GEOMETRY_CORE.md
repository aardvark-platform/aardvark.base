# Aardvark.Base Geometry Semantics

Conventions for geometric queries and transforms in geometry code.

## Transform Semantics

For `M44d` and `Trafo3d`:

- `TransformPos` treats input as point (`w=1`)
- `TransformDir` treats input as direction (`w=0`)
- `TransformPosProj` performs perspective division

`Trafo3d` stores both matrices: `Forward` (model -> transformed space) and `Backward` (inverse). `InvTransformPos` and related methods use `Backward`.

Layout, multiplication side, interop conversion, and `Trafo3d` composition order are covered in `SEMANTICS_LINEAR_ALGEBRA.md`.

## Linear Combinations

For `V3f` and `V3d`, `IsLinearCombinationOf` treats zero bases as spanning only
zero, and dependent nonzero bases as spanning their common line within the default
parallelism tolerance. Coefficient queries return finite coefficients, or `false`
with both outputs NaN. Dependent bases use the basis with the largest absolute
component (first on ties), with the unused coefficient zero; two zero bases give
zero coefficients for a zero target.

For independent bases, the predicate tests whether `(u.Cross(v)).Dot(x)` is tiny.
The coefficient overload instead tests whether the coefficient along `u.Cross(v)`
is tiny. These scale-dependent tolerance checks are not interchangeable.
All overloads are allocation-free.

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

## Circle3 Frame And Bounds

`Circle3f` and `Circle3d` represent a circle by `Center`, a normalized `Normal`,
and `Radius`. Their tangent frame is deterministic and independent of radius:
the first unit tangent is the normalized cross product of `Normal` with X, except
that Y is used when the normal is nearly X-aligned; the second is the normalized
cross product of the first tangent with `Normal`. `AxisU` and `AxisV` are these
unit tangents scaled by `Radius`. A zero radius produces two zero axes.

`Point` is `Center + AxisU` and equals `GetPoint(0)`. `GetPoint` and `Points`
use the same oriented frame. These operations are allocation-free.

For a normalized normal `n`, the axis-aligned bound extent in component `i`
is `Radius * sqrt(max(0, 1 - n[i] * n[i]))`. `BoundingBox3f` and `BoundingBox3d`
use these extents and are allocation-free.

## Sphere Ray Intersections

`Ray3f.HitsSphere` and `Ray3d.HitsSphere` intersect finite rays with finite
spheres. The radius must be finite and non-negative, and the ray direction must
be finite and non-zero. A zero radius retains the center as a point sphere.
Non-finite origins, directions, centers, or radii, zero directions, and empty or
NaN parameter ranges report no hit.

The supplied parameter interval is half-open: each candidate must be finite and
lie in `[tmin, tmax)`. Roots are considered in increasing parameter order rather
than by absolute distance, so the first permitted contact is returned even for
negative intervals. Exact tangencies and zero-radius point contacts are closed
hits. The lower root is a front-side contact and the upper root is a back-side
contact; a repeated tangent root is reported as front-side. Every sphere `out t`
miss writes NaN.

The `ref RayHit3f`/`ref RayHit3d` overloads update `T`, `Point`, `Coord`, and
`BackSide` only when the nearest permitted sphere contact is strictly closer
than the existing `hit.T`. They preserve `Part` on success and leave every field
unchanged on geometric, range, validity, or non-closer misses.

The queries are allocation-free.

## Circle Ray Intersections

`Ray3f.HitsCircle` and `Ray3d.HitsCircle` intersect the ray with the circle's
supporting plane and then test the resulting point against the closed disk. The
radius must be finite and non-negative; a zero radius retains the center point.
Parallel rays, invalid radii, empty or NaN ranges, non-finite candidates, and
plane contacts outside the disk report no hit.

The circle's supplied parameter interval is half-open: a finite candidate must
lie in `[tmin, tmax)`, so `tmin` is included and `tmax` is excluded.
Every `out t` miss writes NaN, including off-disk and invalid-input misses.

The `ref RayHit3f`/`ref RayHit3d` overloads update `T`, `Point`, `Coord`, and
`BackSide` only after confirming a valid disk contact that is strictly closer
than the existing `hit.T`. They preserve `Part` on success and leave every field unchanged
on geometric, range, validity, or farther-candidate misses.

The queries are allocation-free.

## Capped-Cylinder Ray Intersections

`Ray3f.HitsCylinder` and `Ray3d.HitsCylinder` intersect the finite capped surface
whose axis runs from `p0` to `p1` and whose radius is non-negative. Both end caps
are included. Endpoint order does not affect the geometry. Axis-parallel rays can
hit the caps, rays starting inside select their first permitted exit, and a tangent
barrel contact counts as a hit. A zero
radius retains the degenerate axis segment; a zero-length axis, zero ray direction,
negative/non-finite radius, non-finite geometry, or empty/NaN parameter interval
reports no hit.

Candidates are ordered by their ray parameter, not by absolute distance. The
supplied interval is half-open: `t` must be finite and in `[tmin, tmax)`. Thus
`tmin` is included and `tmax` is excluded; default overloads use a non-negative
interval and therefore reject intersections behind the ray origin. An `out t`
miss always writes NaN.

The `ref RayHit3f`/`ref RayHit3d` overloads are closest-hit accumulators. They
return true and update `T`, `Point`, `Coord`, and `BackSide` only when the nearest
valid cylinder candidate is also strictly closer than the existing `hit.T`.
They preserve `Part`, and leave the entire hit unchanged on geometric misses,
range misses, invalid input, and candidates hidden by an existing closer hit.
A nonzero `distanceScale` grows the effective radius with distance.

The `RayPart` cylinder option and value-option overloads in `Boundable.fs` share
these semantics. The queries are allocation-free.

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
- Flag-returning overloads report the union of every box face tied at each interval bound; a corner hit can therefore return two or three face bits. Masked overloads report only selected faces.
- Axes with zero direction components are handled via `DirFlags`; the ray origin must lie between the slabs of such an axis for a hit.

## Hierarchical Cell Intersections

`Cell.Intersects(Cell)` and `Cell2d.Intersects(Cell2d)` require positive volume
or area overlap. Boundary-only contact does not count; a cell intersects itself.

Centered-origin cells can intersect ordinary cells without either containing the other.

`Invalid` is not an empty cell: it intersects itself and a unit-size centered
cell, but not `Unit`. Other pairs involving `Invalid` use `BoundingBox.Intersects`.

## Box/Plane Intersection

`Box2f`/`Box2d` plane intersections use the full plane equation `Normal dot point == Distance`:

- Both the boolean and segment-producing overloads accept finite valid boxes and finite planes with a non-zero normal. Invalid or non-finite inputs report no intersection.
- Intersection is closed: edge and corner contact count as hits. Tangency returns a point segment, and line or point boxes are supported.
- Scaling both `Normal` and `Distance` by the same non-zero factor, including a negative factor, preserves the result.
- On a miss, the segment-producing overload returns `false` and leaves its output at `default`. Ordinary crossing segments retain the established order from lower to higher Y, or lower to higher X for horizontal results.
- The boolean overload is allocation-free.

## Polygon Centroids

`Polygon2f`/`Polygon2d` and `Polygon3f`/`Polygon3d` centroids are independent of
winding and cyclic vertex order. The 3D outline must be planar; collinear first
vertices are allowed.

Fewer than three vertices or zero signed area return `V2*.Zero` or `V3*.Zero`.
Computation is linear-time and allocation-free.

## Polyline Simplification

`GeometryFun.Simplify` implements Ramer-Douglas-Peucker simplification for
`V2f[]` and `V2d[]` polylines and returns strictly increasing source indices.

- A null polyline throws `ArgumentNullException`. A negative or NaN `epsilon` throws `ArgumentOutOfRangeException`; positive infinity is allowed.
- Empty, singleton, and two-point inputs return `[]`, `[0]`, and `[0, 1]`, respectively. For every longer input the first and last indices are retained.
- Error is measured from each source point to the current endpoint segment. A distance equal to `epsilon` is within tolerance and does not cause a split.
- Equal farthest distances select the first source index, preserving deterministic left-to-right output.
- After warmup, queries allocate only the result array. Traversal does not consume call stack proportional to input size.

## Supporting-Line Distance And Parameters

The closest-point and minimal-distance extensions in `SpecialPoints_auto.cs` treat `Ray2f`/`Ray2d` and `Ray3f`/`Ray3d` as unbounded supporting lines. They do not clamp parameters to a forward half-ray:

- A point projection returns the signed parameter `t = dot(point - origin, direction) / direction.LengthSquared`. Values below zero and above one are valid, and reconstruct the closest point as `origin + t * direction`.
- Ray-pair overloads return `t0` and `t1` in each input's original direction parameterization. Rescaling a direction therefore inversely rescales its parameter without changing the reconstructed closest point or distance.
- Parallel and near-parallel pairs keep the established asymmetric convention: `t1` is zero, while `t0` projects the second origin onto the first supporting line. The angular threshold is independent of direction lengths.
- A finite zero direction represents a point and receives parameter zero. If only one direction is zero, the other parameter projects that point onto the non-degenerate supporting line.
- Segment (`Line2*`/`Line3*`) and line/ray callers apply their own `[0, 1]` bounds after obtaining these supporting-line parameters.

## Convex Polygon Line Clipping

`Line2f.ClipWithConvex` and `Line2d.ClipWithConvex` clip a segment against a convex polygon whose points are ordered counter-clockwise:

- Each non-zero polygon edge defines an inclusive left half-plane; duplicate consecutive points and other zero-length edges are ignored.
- The default overload uses `Constant<float/double>.PositiveTinyValue` as an absolute point-distance tolerance. The explicit overload accepts a non-negative absolute epsilon.
- Boundary-collinear segments and single-point vertex contacts are retained.
- The result keeps the input `P0`-to-`P1` direction. An endpoint that does not require clipping is returned bit-for-bit unchanged.
- If the segment has no non-empty parameter interval inside the polygon, both result points are NaN.

## Line Segment Plane Clipping

`Line2f`/`Line2d` and `Line3f`/`Line3d` provide `ClipByPlane` overloads for their matching plane types:

- The retained region is the inclusive positive half-space. For a non-zero normal, a point is retained when `(Normal dot point - Distance) / |Normal| >= -absoluteEpsilon`.
- The default overload uses `Constant<float/double>.PositiveTinyValue`; the explicit overload accepts a non-negative absolute point-distance tolerance. Scaling both a plane normal and its distance by the same positive factor does not change the result.
- Results preserve the input `P0`-to-`P1` order. Endpoints that do not require clipping are returned bit-for-bit unchanged, and a single boundary contact is returned as a point segment.
- Fully rejected segments use NaN for both result points. A plane with a zero normal is treated as a no-op, including `Plane2f.Invalid`/`Plane2d.Invalid` and `Plane3f.Invalid`/`Plane3d.Invalid`.

## Polygon Region Containment

`PolyRegion.Contains(V2d)` interprets all contours together using the even-odd rule:

- Contour orientation does not affect containment. Clockwise holes, reversed contours, and contours transformed by a negative determinant retain their geometric meaning.
- Each contour crossing toggles containment, so holes are excluded and nested islands are included.
- Points on contour edges or vertices are contained. Contours with fewer than three vertices do not contribute.
- `Contains` is allocation-free.

## Attributed Polygon Regions

`Polygon2d<'a>` stores parallel point and attribute arrays. `PolyRegion<'a>` carries those attributes through normalization, boolean operations, and triangulation:

- Point and attribute arrays must have equal lengths. Closing an open contour duplicates both its first point and first attribute; orientation reversal and redundant-collinear-point removal always apply the same indices to both arrays.
- Constructors accept a `TessellationRule` and interpolation callback, with an even-odd convenience overload. Constructor output contours are normalized counter-clockwise without modifying the source arrays.
- `Union` uses positive winding. `Difference` uses positive winding after reversing the right operand's contours and attributes. `Intersection` retains winding magnitude greater than one, while `Xor` uses even-odd winding.
- Boolean results retain LibTess boundary orientation, including clockwise hole contours. `Triangulate` uses even-odd winding.
- Every boolean method and `Triangulate` requires `float[] -> 'a[] -> 'a`. LibTess calls it when an edge crossing or tessellation step invents a vertex; the weights and contributing attributes determine that vertex's attribute.
- Attributed boolean operations have no operators.

## Source Anchors

- `src/Aardvark.Base/Geometry/Relations/LinearCombination_template.cs` / `LinearCombination_auto.cs` (`LinearCombination.IsLinearCombinationOf`)

- `src/Aardvark.Base/Math/RangesBoxes/Cell.cs` (`Cell.Intersects`)
- `src/Aardvark.Base/Math/RangesBoxes/Cell2d.cs` (`Cell2d.Intersects`)
- `src/Aardvark.Base/Math/Trafos/Matrix_auto.cs` (`TransformPos`, `TransformDir`, `TransformPosProj`)
- `src/Aardvark.Base/Math/Trafos/Trafo_auto.cs` (`Trafo3d`, `Forward`, `Backward`)
- `src/Aardvark.Base/Geometry/IntersectionTests_auto.cs` (`Box3d.Intersects(Ray3d, out t)`, `Box2f`/`Box2d` plane intersections)
- `src/Aardvark.Base/Geometry/Algorithms_auto.cs` (`GeometryFun.Simplify` for `V2f[]`/`V2d[]`)
- `src/Aardvark.Base/Geometry/Types/Ray/Ray3_auto.cs` (`Ray3d.Hits` overloads, circle/capped-cylinder kernels, `RayHit3d`, `FastRay3d`)
- `src/Aardvark.Base.FSharp/Datastructures/Geometry/Boundable.fs` (`RayPart` cylinder option/value-option delegation)
- `src/Aardvark.Base/Geometry/Types/Circle/Circle3_auto.cs` (`Circle3f`/`Circle3d` frame, points, and bounds)
- `src/Aardvark.Base/Geometry/Types/Polygon/Polygon2_auto.cs` (`Polygon2f`/`Polygon2d` centroids)
- `src/Aardvark.Base/Geometry/Types/Polygon/Polygon3_auto.cs` (`Polygon3f`/`Polygon3d` centroids)
- `src/Aardvark.Base/Geometry/SpecialPoints_auto.cs` (point/ray and ray/ray closest-distance parameters)
- `src/Aardvark.Base/Geometry/ClippingFunctions_auto.cs` (`Line2f.ClipWithConvex`, `Line2d.ClipWithConvex`)
- `src/Aardvark.Base/Geometry/ClippingFunctions_auto.cs` (`Line2f`/`Line2d`/`Line3f`/`Line3d.ClipByPlane`)
- `src/Aardvark.Geometry/PolyRegion2d.fs` (`PolyRegion.Contains`, `Polygon2d<'a>`, `PolyRegion<'a>`, `PolygonTessellator`)

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

## Circle3 Frame And Bounds

`Circle3f` and `Circle3d` represent a circle by `Center`, a normalized `Normal`,
and `Radius`. Their tangent frame is deterministic and independent of radius:
the first unit tangent is the normalized cross product of `Normal` with X, except
that Y is used when the normal is nearly X-aligned; the second is the normalized
cross product of the first tangent with `Normal`. `AxisU` and `AxisV` scale these
unit tangents by `Radius` only after constructing the frame. This preserves the
established orientation for ordinary normalized normals and keeps finite extreme
radii from collapsing `AxisV` through intermediate normalization overflow or
underflow. A zero radius produces two zero axes.

`Point` is `Center + AxisU` and equals `GetPoint(0)`. `GetPoint` and `Points`
use the same oriented frame; `Points` constructs its scaled axes once per enumeration.
These frame and point operations do not allocate.

For a normalized normal `n`, the exact axis-aligned bound extent in component `i`
is `Radius * sqrt(max(0, 1 - n[i] * n[i]))`. The non-negative clamp handles
normalization round-off before the square root. `BoundingBox3f` and `BoundingBox3d`
use these projection extents directly rather than bounding only four frame-cardinal
points, so oblique-circle extrema are not underestimated; the properties do not allocate.

## Capped-Cylinder Ray Intersections

`Ray3f.HitsCylinder` and `Ray3d.HitsCylinder` intersect the finite capped surface
whose axis runs from `p0` to `p1` and whose radius is non-negative. The candidate
set consists of both roots of the perpendicular barrel quadratic, clipped to the
closed axial extent, and both circular end caps. Endpoint order does not affect
the geometry. Axis-parallel rays still test the caps, rays starting inside select
their first permitted exit, and a tangent barrel contact counts as a hit. A zero
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
A nonzero `distanceScale` retains the established distance-based effective-radius
growth before candidates pass through the same finite capped-cylinder kernel.

The `RayPart` cylinder option and value-option forms in `Boundable.fs` delegate
to the double-precision core, so they share the same cap, range, and finite-result
semantics without constructing candidate arrays. The core and accumulator paths
do not allocate.

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

## Box/Plane Intersection

`Box2f`/`Box2d` plane intersections use the full plane equation `Normal dot point == Distance`:

- Both the boolean and segment-producing overloads accept finite valid boxes and finite planes with a non-zero normal. Invalid or non-finite inputs report no intersection.
- Intersection is closed: edge and corner contact count as hits. Tangency returns a point segment, and line or point boxes are supported.
- Scaling both `Normal` and `Distance` by the same non-zero factor, including a negative factor, preserves the result.
- On a miss, the segment-producing overload returns `false` and leaves its output at `default`. Ordinary crossing segments retain the established order from lower to higher Y, or lower to higher X for horizontal results.
- The boolean overload tests the box's normal-projection interval directly and does not allocate a corner array.

## Polyline Simplification

`GeometryFun.Simplify` implements Ramer-Douglas-Peucker simplification for
`V2f[]` and `V2d[]` polylines and returns strictly increasing source indices.

- A null polyline throws `ArgumentNullException`. A negative or NaN `epsilon` throws `ArgumentOutOfRangeException`; positive infinity is allowed.
- Empty, singleton, and two-point inputs return `[]`, `[0]`, and `[0, 1]`, respectively. For every longer input the first and last indices are retained.
- Error is measured from each source point to the current endpoint segment. A distance equal to `epsilon` is within tolerance and does not cause a split.
- Equal farthest distances select the first source index, preserving deterministic left-to-right output.
- A root segment already within tolerance allocates only its final two-index result. Inputs requiring splits use pooled, exception-safely returned iterative workspace and allocate only the final result after warmup; traversal does not consume call stack proportional to input size.

## Supporting-Line Distance And Parameters

The closest-point and minimal-distance extensions in `SpecialPoints_auto.cs` treat `Ray2f`/`Ray2d` and `Ray3f`/`Ray3d` as unbounded supporting lines. They do not clamp parameters to a forward half-ray:

- A point projection returns the signed parameter `t = dot(point - origin, direction) / direction.LengthSquared`. Values below zero and above one are valid, and reconstruct the closest point as `origin + t * direction`.
- Ray-pair overloads return `t0` and `t1` in each input's original direction parameterization. Rescaling a direction therefore inversely rescales its parameter without changing the reconstructed closest point or distance.
- Parallel and near-parallel pairs keep the established asymmetric convention: `t1` is zero, while `t0` projects the second origin onto the first supporting line. The angular threshold is independent of direction lengths.
- A finite zero direction represents a point and receives parameter zero. If only one direction is zero, the other parameter projects that point onto the non-degenerate supporting line.
- Extreme finite directions use component-scaled fallbacks only when raw squared norms or norm products underflow or overflow; ordinary finite magnitudes stay on the direct arithmetic path.
- Segment (`Line2*`/`Line3*`) and line/ray callers apply their own `[0, 1]` bounds after obtaining these supporting-line parameters.

## Convex Polygon Line Clipping

`Line2f.ClipWithConvex` and `Line2d.ClipWithConvex` clip a segment against a convex polygon whose points are ordered counter-clockwise:

- Each non-zero polygon edge defines an inclusive left half-plane; duplicate consecutive points and other zero-length edges are ignored.
- The default overload uses `Constant<float/double>.PositiveTinyValue` as an absolute point-distance tolerance. The explicit overload accepts a non-negative absolute epsilon. Internally, that distance is multiplied by each edge length before signed-cross-product comparisons.
- If a non-zero finite edge's raw length underflows to zero or overflows to infinity, a component-scaled fallback preserves the same half-plane and point-distance tolerance.
- Boundary-collinear segments and single-point vertex contacts are retained.
- The result keeps the input `P0`-to-`P1` direction. An endpoint that does not require clipping is returned bit-for-bit unchanged.
- If the segment has no non-empty parameter interval inside the polygon, both result points are NaN.

## Line Segment Plane Clipping

`Line2f`/`Line2d` and `Line3f`/`Line3d` provide `ClipByPlane` overloads for their matching plane types:

- The retained region is the inclusive positive half-space. For a non-zero normal, a point is retained when `(Normal dot point - Distance) / |Normal| >= -absoluteEpsilon`.
- The default overload uses `Constant<float/double>.PositiveTinyValue`; the explicit overload accepts a non-negative absolute point-distance tolerance. Scaling both a plane normal and its distance by the same positive factor does not change the result, including when a finite raw normal length underflows or overflows.
- Results preserve the input `P0`-to-`P1` order. Endpoints that do not require clipping are returned bit-for-bit unchanged, and a single boundary contact is returned as a point segment.
- Fully rejected segments use NaN for both result points. A plane with a zero normal is treated as a no-op, including `Plane2f.Invalid`/`Plane2d.Invalid` and `Plane3f.Invalid`/`Plane3d.Invalid`.

## Polygon Region Containment

`PolyRegion.Contains(V2d)` interprets all contours together using the even-odd rule:

- Contour orientation does not affect containment. Clockwise holes, reversed contours, and contours transformed by a negative determinant retain their geometric meaning.
- Each contour crossing toggles containment, so holes are excluded and nested islands are included.
- Points on contour edges or vertices are contained. Contours with fewer than three vertices do not contribute.
- Containment scans contour edges directly without tessellation or per-query allocation.

## Attributed Polygon Regions

`Polygon2d<'a>` stores parallel point and attribute arrays. `PolyRegion<'a>` carries those attributes through normalization, boolean operations, and triangulation:

- Point and attribute arrays must have equal lengths. Closing an open contour duplicates both its first point and first attribute; orientation reversal and redundant-collinear-point removal always apply the same indices to both arrays.
- Constructors accept a `TessellationRule` and interpolation callback, with an even-odd convenience overload. Constructor output contours are normalized counter-clockwise without modifying the source arrays.
- `Union` uses positive winding. `Difference` uses positive winding after reversing the right operand's contours and attributes. `Intersection` retains winding magnitude greater than one, while `Xor` uses even-odd winding.
- Boolean results retain LibTess boundary orientation, including clockwise hole contours. `Triangulate` uses even-odd winding.
- Every boolean method and `Triangulate` requires `float[] -> 'a[] -> 'a`. LibTess calls it when an edge crossing or tessellation step invents a vertex; the weights and contributing attributes determine that vertex's attribute.
- Attributed boolean operations intentionally have no operators, so call sites cannot conceal the interpolation policy.

## Source Anchors

- `src/Aardvark.Base/Math/Trafos/Matrix_auto.cs` (`TransformPos`, `TransformDir`, `TransformPosProj`)
- `src/Aardvark.Base/Math/Trafos/Trafo_auto.cs` (`Trafo3d`, `Forward`, `Backward`)
- `src/Aardvark.Base/Geometry/IntersectionTests_auto.cs` (`Box3d.Intersects(Ray3d, out t)`, `Box2f`/`Box2d` plane intersections)
- `src/Aardvark.Base/Geometry/Algorithms_auto.cs` (`GeometryFun.Simplify` for `V2f[]`/`V2d[]`)
- `src/Aardvark.Base/Geometry/Types/Ray/Ray3_auto.cs` (`Ray3d.Hits` overloads, capped-cylinder kernel, `RayHit3d`, `FastRay3d`)
- `src/Aardvark.Base.FSharp/Datastructures/Geometry/Boundable.fs` (`RayPart` cylinder option/value-option delegation)
- `src/Aardvark.Base/Geometry/Types/Circle/Circle3_auto.cs` (`Circle3f`/`Circle3d` frame, points, and bounds)
- `src/Aardvark.Base/Geometry/SpecialPoints_auto.cs` (point/ray and ray/ray closest-distance parameters)
- `src/Aardvark.Base/Geometry/ClippingFunctions_auto.cs` (`Line2f.ClipWithConvex`, `Line2d.ClipWithConvex`)
- `src/Aardvark.Base/Geometry/ClippingFunctions_auto.cs` (`Line2f`/`Line2d`/`Line3f`/`Line3d.ClipByPlane`)
- `src/Aardvark.Geometry/PolyRegion2d.fs` (`PolyRegion.Contains`, `Polygon2d<'a>`, `PolyRegion<'a>`, `PolygonTessellator`)

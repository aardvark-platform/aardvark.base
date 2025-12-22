# Aardvark.Base Primitive Types Reference

AI-targeted reference for Aardvark.Base mathematical primitives. All types are `readonly struct` with aggressive inlining.

## Naming Convention

| Suffix | C# Type | Category |
|--------|---------|----------|
| `b` | byte | Integer |
| `sb` | sbyte | Integer |
| `s` | short | Integer |
| `us` | ushort | Integer |
| `i` | int | Integer |
| `ui` | uint | Integer |
| `l` | long | Integer |
| `ul` | ulong | Integer |
| `h` | Half | Real |
| `f` | float | Real |
| `d` | double | Real |

Pattern: `{TypeName}{Dimension}{Suffix}` (e.g., `V3d` = 3D vector of double)

---

## Vectors

### Types

| 2D | 3D | 4D |
|----|----|----|
| V2i, V2ui, V2l, V2f, V2d | V3i, V3ui, V3l, V3f, V3d | V4i, V4ui, V4l, V4f, V4d |

### Properties
- `X`, `Y`, `Z` (3D/4D), `W` (4D only)
- `Length`, `LengthSquared`
- `Normalized` - unit vector
- `XY`, `XZ`, `YZ` - swizzles (V3/V4)

### Static Fields
- `Zero`, `One`, `XAxis`, `YAxis`, `ZAxis` (3D), `WAxis` (4D)
- `MinValue`, `MaxValue`, `NaN` (real types)

### Operations
```csharp
V3d.Dot(a, b)              // dot product
V3d.Cross(a, b)            // cross product (V3 only)
V3d.Distance(a, b)         // Euclidean distance
V3d.DistanceSquared(a, b)  // squared distance
V3d.Lerp(a, b, t)          // linear interpolation
V3d.Min(a, b)              // component-wise minimum
V3d.Max(a, b)              // component-wise maximum
a.Normalized               // unit vector
a.Length                   // magnitude
```

### Operators
`+`, `-`, `*`, `/` (component-wise and scalar), unary `-`

---

## Matrices

### Types

| Size | Types | Use Case |
|------|-------|----------|
| 2x2 | M22i, M22f, M22d | 2D rotation/scale |
| 2x3 | M23f, M23d | 2D affine |
| 3x3 | M33i, M33f, M33d | 2D homogeneous, 3D rotation/scale |
| 3x4 | M34f, M34d | 3D affine |
| 4x4 | M44i, M44f, M44d | 3D homogeneous, projection |

### Properties
- `M00`..`M33` - element access
- `C0`, `C1`, `C2`, `C3` - column vectors
- `R0`, `R1`, `R2`, `R3` - row vectors
- `Determinant`
- `Inverse`
- `Transposed`

### Static Fields
- `Identity`, `Zero`

### Construction
```csharp
M44d.Translation(V3d)
M44d.Scale(V3d)
M44d.Rotation(V3d axis, double angle)
M44d.RotationX(angle), RotationY, RotationZ
new M44d(Rot3d)           // from rotation
new M44d(Trafo3d)         // from transformation
```

### Operations
```csharp
m.Transform(V3d)           // transform point
m.TransformDir(V3d)        // transform direction (no translation)
m.TransformPos(V3d)        // transform position (with translation)
m * v                      // matrix-vector multiply
m1 * m2                    // matrix-matrix multiply
```

---

## Colors

### Types

| RGB | RGBA |
|-----|------|
| C3b, C3us, C3ui, C3f, C3d | C4b, C4us, C4ui, C4f, C4d |

Byte types: [0, 255]. Float/double types: [0.0, 1.0].

### Properties
- `R`, `G`, `B`, `A` (C4 only)
- `ToV3f()`, `ToV4f()` - convert to vector

### Static Fields
- `Black`, `White`, `Red`, `Green`, `Blue`, `Yellow`, `Cyan`, `Magenta`
- `Gray`, `DarkGray`, `LightGray`

### Construction
```csharp
new C3f(r, g, b)
new C4b(r, g, b, a)
C3f.FromHSV(h, s, v)
new C3f(C3b)               // automatic range conversion
```

---

## Transformations

Transformation types compose via `*` operator. All have `Inverse` property.

### Hierarchy (least to most general)

| Type | Components | 2D | 3D |
|------|------------|----|----|
| Rot | Rotation only | Rot2f, Rot2d | Rot3f, Rot3d |
| Scale | Non-uniform scale | Scale2f, Scale2d | Scale3f, Scale3d |
| Shift | Translation only | Shift2f, Shift2d | Shift3f, Shift3d |
| Euclidean | Rot + Shift | Euclidean2f, Euclidean2d | Euclidean3f, Euclidean3d |
| Similarity | Uniform scale + Rot + Shift | Similarity2f, Similarity2d | Similarity3f, Similarity3d |
| Affine | Scale + Rot + Shift | Affine2f, Affine2d | Affine3f, Affine3d |
| Trafo | General (forward + backward matrix) | Trafo2f, Trafo2d | Trafo3f, Trafo3d |

### Rot3 (Quaternion)
```csharp
Rot3d.Identity
Rot3d.Rotation(V3d axis, double angle)
Rot3d.RotationX(angle), RotationY, RotationZ
Rot3d.FromFrame(x, y, z)   // from orthonormal basis
rot.Transform(V3d)         // rotate vector
rot.Inverse                // conjugate
```

### Trafo3 (General)
```csharp
Trafo3d.Identity
Trafo3d.Translation(V3d)
Trafo3d.Scale(V3d)
Trafo3d.Scale(double)      // uniform
Trafo3d.Rotation(V3d axis, double angle)
Trafo3d.RotationX(angle), RotationY, RotationZ
Trafo3d.FromOrthoNormalBasis(x, y, z)

trafo.Forward              // M44d forward matrix
trafo.Backward             // M44d inverse matrix
trafo.Inverse              // inverted Trafo3d
trafo.TransformPos(V3d)    // transform point
trafo.TransformDir(V3d)    // transform direction

trafo1 * trafo2            // compose transformations
```

### Euclidean3 (Rigid Body)
```csharp
Euclidean3d.Identity
Euclidean3d.Translation(V3d)
Euclidean3d.Rotation(V3d axis, double angle)

eucl.Rot                   // Rot3d component
eucl.Trans                 // V3d translation
eucl.TransformPos(V3d)
```

---

## Geometric Primitives

All have `f` and `d` variants. Pattern: `{Shape}{Dim}{Suffix}`

### Bounding Boxes

| Type | Properties |
|------|------------|
| Box2f, Box2d | Min, Max (V2), Size, Center |
| Box3f, Box3d | Min, Max (V3), Size, Center, Volume |

```csharp
Box3d.Invalid              // empty box
Box3d.Unit                 // [0,1]^3
new Box3d(V3d min, V3d max)
new Box3d(V3d center, double radius)  // from sphere

box.Contains(V3d)
box.Contains(Box3d)
box.Intersects(Box3d)
box.ExtendedBy(V3d)        // return enlarged box
box.ExtendedBy(Box3d)
box.Transformed(M44d)
```

### Rays

| Type | Properties |
|------|------------|
| Ray2f, Ray2d | Origin, Direction (V2) |
| Ray3f, Ray3d | Origin, Direction (V3) |

```csharp
new Ray3d(V3d origin, V3d direction)
ray.GetPointOnRay(double t)           // origin + t * direction
ray.Normalized                        // unit direction
ray.Reversed

// Hit testing
ray.Hits(Triangle3d)
ray.Hits(Box3d)
ray.Hits(Sphere3d)
ray.Hits(Plane3d)
ray.HitsTriangle(p0, p1, p2, tmin, tmax, ref RayHit3d)
```

### Planes

| Type | Properties |
|------|------------|
| Plane2f, Plane2d | Normal, Distance |
| Plane3f, Plane3d | Normal, Distance, Point |

```csharp
new Plane3d(V3d normal, double distance)
new Plane3d(V3d normal, V3d point)
new Plane3d(V3d p0, V3d p1, V3d p2)   // from 3 points

Plane3d.XPlane, YPlane, ZPlane        // axis-aligned

plane.Height(V3d)          // signed distance to point
plane.Sign(V3d)            // -1, 0, +1
plane.NearestPoint(V3d)
plane.Normalized
```

### Lines

| Type | Properties |
|------|------------|
| Line2f, Line2d | P0, P1 (endpoints) |
| Line3f, Line3d | P0, P1, Direction, Ray3 |

```csharp
new Line3d(V3d p0, V3d p1)
line.GetClosestPointOnLine(V3d)
line.GetDistanceToLine(V3d)
line.Direction              // P1 - P0
```

### Circles and Spheres

| Type | Properties |
|------|------------|
| Circle2f, Circle2d | Center, Radius |
| Circle3f, Circle3d | Center, Normal, Radius |
| Sphere3f, Sphere3d | Center, Radius |

```csharp
new Sphere3d(V3d center, double radius)
sphere.Contains(V3d)
sphere.Volume
sphere.SurfaceArea
```

### Triangles

| Type | Properties |
|------|------------|
| Triangle2f, Triangle2d | P0, P1, P2, Area |
| Triangle3f, Triangle3d | P0, P1, P2, Normal, Area |

```csharp
new Triangle3d(V3d p0, V3d p1, V3d p2)
tri.Normal
tri.Area
tri.Centroid
tri.BoundingBox3d
```

### Polygons

| Type | Properties |
|------|------------|
| Polygon2f, Polygon2d | PointArray, PointCount |
| Polygon3f, Polygon3d | PointArray, PointCount |

```csharp
new Polygon3d(V3d[])
polygon.ComputeArea()
polygon.ComputeCentroid()
polygon.BoundingBox3d
```

### Hulls (Convex Polyhedra)

| Type | Properties |
|------|------------|
| Hull2f, Hull2d | PlaneArray (Plane2[]) |
| Hull3f, Hull3d | PlaneArray (Plane3[]) |

```csharp
new Hull3d(Box3d)          // from AABB
hull.Contains(V3d)
```

### Other 3D Primitives

| Type | Properties |
|------|------------|
| Cylinder3f, Cylinder3d | P0, P1 (axis), Radius |
| Cone3f, Cone3d | Origin (apex), Circle (base) |
| Capsule3f, Capsule3d | P0, P1, Radius |
| Torus3f, Torus3d | Position, Direction, MajorRadius, MinorRadius |

---

## Other Types

### Quaternion
General quaternion (not necessarily unit). For rotations, use `Rot3f`/`Rot3d`.

| Type | Properties |
|------|------------|
| QuaternionF, QuaternionD | W, X, Y, Z (W is scalar) |

### Complex
| Type | Properties |
|------|------------|
| ComplexF, ComplexD | Real, Imag, Magnitude, Phase |

### Ranges
1D intervals for various numeric types.

| Type | Properties |
|------|------------|
| Range1b, Range1i, Range1l, Range1f, Range1d | Min, Max, Size, Center |

---

## Type Conversion

### Between Precisions
```csharp
new V3d(V3f)               // float -> double (explicit)
new V3f(V3d)               // double -> float (explicit, may lose precision)
(V3f)v3d                   // explicit cast
```

### Between Dimensions
```csharp
new V3d(V2d, z)            // 2D -> 3D with z component
new V2d(V3d)               // 3D -> 2D (drops Z)
v.XY                       // swizzle to lower dimension
```

### Transformation to Matrix
```csharp
(M44d)trafo3d              // explicit cast
trafo.Forward              // forward transformation matrix
rot3d.IsNormalized         // verify unit quaternion
```

---

## Common Patterns

### Transform Composition
```csharp
// Right-to-left: scale, then rotate, then translate
var trafo = Trafo3d.Translation(pos) * Trafo3d.Rotation(axis, angle) * Trafo3d.Scale(s);
var transformed = trafo.TransformPos(point);
```

### Bounding Box Accumulation
```csharp
var box = Box3d.Invalid;
foreach (var p in points)
    box.ExtendBy(p);       // mutating version
// or
box = points.Aggregate(Box3d.Invalid, (b, p) => b.ExtendedBy(p));
```

### Ray Intersection
```csharp
var ray = new Ray3d(origin, direction.Normalized);
var hit = new RayHit3d();
if (ray.HitsTriangle(p0, p1, p2, 0, double.MaxValue, ref hit))
{
    var point = hit.Point;
    var t = hit.T;         // ray parameter
    var bary = hit.Coord;  // barycentric coordinates (V2d)
}
```

### Plane Operations
```csharp
var plane = new Plane3d(V3d.ZAxis, 0);  // XY plane at origin
var dist = plane.Height(point);          // signed distance
var proj = plane.NearestPoint(point);    // project onto plane
```

---

## Gotchas

1. **Matrix-Vector Multiplication Order**: `M44d * V3d` is valid but ambiguous. Use `m.TransformPos(v)` or `m.TransformDir(v)` for clarity on translation behavior
2. **Precision Loss**: Converting `V3d` → `V3f` loses precision silently. Convert toward *higher* precision (f→d) when possible, avoid d→f
3. **Rotation Composition**: `T * R * S` applies scale first (right-most), then rotate, then translate (left-most). Order matters!
4. **Normalized Assumption**: Many functions assume unit vectors. Call `.Normalized` on directions before Ray construction or Plane creation

---

## Camera Views

Interface and implementations for camera positioning.

### ICameraView Interface

```csharp
public interface ICameraView
{
    Trafo3d ViewTrafo { get; set; }  // World → Camera space
    V3d Location { get; set; }       // Camera position (world space)
    V3d Forward { get; set; }        // View direction (-Z in camera space)
    V3d Up { get; set; }             // Up vector (+Y in camera space)
    V3d Right { get; set; }          // Right vector (+X in camera space)
    IEvent<Trafo3d> ViewTrafos { get; }  // Change notifications
    void Set(V3d location, V3d right, V3d up, V3d forward);
}
```

### Implementations

| Type | Description |
|------|-------------|
| `CameraViewRaw` | Direct axis control, no constraints |
| `CameraViewWithSky` | Maintains sky direction, auto-adjusts axes |

### Camera Space Convention

In camera space:
- Camera at origin
- Looking down **-Z axis**
- **+Y** is up
- **+X** is right

### View Transformation

```csharp
// Create view transformation manually
Trafo3d.ViewTrafo(location, right, up, -forward)

// LookAt helper (forward-up-based)
Trafo3d.ViewTrafoRH(location, forward, up)
```

### Usage

```csharp
var camera = new CameraViewWithSky();
camera.Location = new V3d(0, 0, 10);
camera.Forward = -V3d.ZAxis;
camera.Sky = V3d.YAxis;

// Get view matrix for rendering
var viewMatrix = camera.ViewTrafo.Forward;
```

---

## See Also

- [TENSORS.md](TENSORS.md) - N-dimensional arrays and stride indexing; concepts apply to geometric data storage
- [PIXIMAGE.md](PIXIMAGE.md) - Images use `V2i` for sizes and `C3b`/`C4b` for colors; color conversions defined here
- [SERIALIZATION.md](SERIALIZATION.md) - All primitive types support `ICoder` (e.g., `CodeV3d`, `CodeM44d`, `CodeTrafo3d`)

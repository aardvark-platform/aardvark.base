# Aardvark.Base F# Interop Reference

AI-targeted reference for using Aardvark.Base types from F#. Covers modules, extension functions, and idiomatic patterns.

---

## Namespace and Module Structure

### Common Open Statements

```fsharp
open Aardvark.Base                // Core types: V3d, M44d, Box3d, etc.
open Aardvark.Base.Sorting        // Sorting extension methods
```

### Module Hierarchy

| Namespace | Contents |
|-----------|----------|
| `Aardvark.Base` | All primitive types + F# modules |
| `Aardvark.Base.Sorting` | Array sorting extensions |

---

## Vec Module

Functional wrappers for vector operations. Works with any vector type (V2d, V3f, V4i, etc.).

```fsharp
open Aardvark.Base

// Dot product
let d = Vec.dot V3d.XAxis V3d.YAxis  // 0.0

// Cross product (3D only)
let c = Vec.cross V3d.XAxis V3d.YAxis  // V3d.ZAxis

// Length and normalization
let len = Vec.length V3d.One           // sqrt(3)
let lenSq = Vec.lengthSquared V3d.One  // 3.0
let unit = Vec.normalize V3d.One       // unit vector

// Distance
let dist = Vec.distance V3d.Zero V3d.One
let distSq = Vec.distanceSquared V3d.Zero V3d.One

// Swizzles
let x = Vec.x v3d
let xy = Vec.xy v3d      // V2d
let xyz = Vec.xyz v4d    // V3d

// Reflection/refraction
let refl = Vec.reflect normal incident
let refr = Vec.refract eta normal incident

// Component-wise comparisons
Vec.anySmaller V3d.One V3d.Zero    // false
Vec.allGreater V3d.One V3d.Zero    // true
Vec.anyEqual v1 v2
Vec.allDifferent v1 v2
```

---

## Mat Module

Functional wrappers for matrix operations.

```fsharp
open Aardvark.Base

// Transpose, determinant, inverse
let mt = Mat.transpose M33d.Identity
let d = Mat.det M22d.Identity
let inv = Mat.inverse M33d.Identity

// Transform vectors
let v4 = Mat.transform M44d.Identity V4d.One
let pos = Mat.transformPos M44d.Identity V3d.One   // w=1
let dir = Mat.transformDir M44d.Identity V3d.One   // w=0
let proj = Mat.transformPosProj M44d.Identity V3d.One  // with perspective divide

// Component-wise comparisons
Mat.anyEqual m1 m2
Mat.allSmaller m1 m2
```

---

## Trafo Module

Functional wrappers for transformation types.

```fsharp
open Aardvark.Base

let t = Trafo3d.Translation(V3d(1, 2, 3))

// Access matrices
let fwd = Trafo.forward t    // M44d forward matrix
let bwd = Trafo.backward t   // M44d inverse matrix

// Invert
let inv = Trafo.inverse t

// Compose and transform
let pos = V3d.Zero |> Mat.transformPos (t |> Trafo.forward)
```

---

## Lens System

Functional lenses for immutable state updates. Used extensively with Adaptify-generated models.

### Lens Type

```fsharp
type Lens<'s, 'a> =
    abstract Get : 's -> 'a
    abstract Set : 's * 'a -> 's
    abstract Update : 's * ('a -> 'a) -> 's
```

### Operators

| Operator | Signature | Description |
|----------|-----------|-------------|
| `\|.` | `Lens<'s,'a> -> Lens<'a,'b> -> Lens<'s,'b>` | Compose lenses |
| `\|?` | `Lens<'s, Option<'a>> -> 'a -> Lens<'s,'a>` | Default for None |

### Usage

```fsharp
open Aardvark.Base

// Adaptify generates lenses like Model.position_
// Compose with |. operator
let nestedLens = Model.inner_ |. Inner.value_

// Get value
let v = nestedLens.Get model

// Set value
let model' = nestedLens.Set(model, newValue)

// Update value
let model'' = nestedLens.Update(model, fun v -> v + 1)
```

### Predefined Lenses

```fsharp
// Set membership lens
Set.Lens.contains "item"  // Lens<Set<string>, bool>

// Map item lens
Map.Lens.item "key"       // Lens<Map<string,'v>, Option<'v>>

// List item lens
List.Lens.item 0          // Lens<list<'a>, Option<'a>>
```

---

## Color Module (ColorBrewer)

Color scheme generation for data visualization.

```fsharp
open Aardvark.Base

// Get sequential color scheme
let colors = ColorBrewer.getColors ColorBrewer.SchemeType.Sequential "Blues" 5

// Available scheme types
ColorBrewer.SchemeType.Sequential    // Light to dark
ColorBrewer.SchemeType.Diverging     // Two-ended
ColorBrewer.SchemeType.Qualitative   // Distinct categories
```

---

## Interop Modules

F# extensions for .NET collections.

### Dictionary Extensions

```fsharp
open Aardvark.Base

let dict = System.Collections.Generic.Dictionary<string, int>()

// F#-style access
dict.["key"] <- 42
let v = dict.["key"]

// TryFind returns Option
match dict.TryFind "key" with
| Some v -> ...
| None -> ...
```

### HashSet Extensions

```fsharp
open Aardvark.Base

let set = System.Collections.Generic.HashSet<int>()

// Set operations return new sets
let union = set.Union otherSet
let inter = set.Intersect otherSet
```

---

## Memory and Native Utilities

### MicroTime (High-Resolution Timing)

```fsharp
open Aardvark.Base

let start = MicroTime.Now
// ... work ...
let elapsed = MicroTime.Now - start
printfn "Elapsed: %A" elapsed
```

### Mem (Memory Sizes)

```fsharp
open Aardvark.Base

let size = Mem.mebibytes 512L
printfn "%d bytes" size.Bytes
```

---

## Common Patterns

### Pipeline with Transformations

```fsharp
let result =
    V3d.Zero
    |> Mat.transformPos (Trafo3d.Translation(V3d.One) |> Trafo.forward)
    |> Mat.transformDir (Trafo3d.RotationZ(Constant.Pi) |> Trafo.forward)
```

### Lens-Based State Updates

```fsharp
// Update nested model immutably
let model' =
    model
    |> (Model.camera_ |. Camera.position_).Set(V3d(0, 0, 10))
    |> (Model.camera_ |. Camera.target_).Set(V3d.Zero)
```

### Vector Comparisons in Conditionals

```fsharp
if Vec.allSmaller point boxMax && Vec.allGreater point boxMin then
    // point is inside box
    ()
```

---

## Gotchas

1. **Module Suffix Convention**: F# modules like `Vec`, `Mat`, `Trafo` use `CompilationRepresentationFlags.ModuleSuffix` to avoid name collisions with types. Don't confuse `Vec` (module) with `V3d` (type)

2. **Inline Functions**: Most `Vec` and `Mat` functions are `inline` with SRTP constraints. This means they work on any type with matching static members, but error messages can be cryptic if types don't match

3. **Lens Composition Order**: `outer |. inner` reads left-to-right (outer first, then inner). This is opposite to function composition `>>`. Think of `|.` as "then focus on"

4. **Option Lenses**: `Map.Lens.item` returns `Lens<_, Option<_>>`. Use `|?` operator to provide a default, or handle `None` explicitly

---

## See Also

- [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) - C# API reference for V3d, M44d, Trafo3d (same types, method syntax)
- [COLLECTIONS.md](COLLECTIONS.md) - Symbol and SymbolDict (usable from F# with same API)
- [ALGORITHMS.md](ALGORITHMS.md) - Graph and spatial algorithms callable from F#

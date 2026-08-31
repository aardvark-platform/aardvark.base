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
| `ui` | `uint` (vectors and `Range1ui`; not matrices/boxes) |
| `l` | `long` |
| `f` | `float` |
| `d` | `double` |

Examples:
- `V3d` = 3D vector (`double`)
- `M44f` = 4x4 matrix (`float`)
- `Box3d` = 3D axis-aligned box (`double`)

1D ranges cover more element types than the multi-dimensional families (`Range1b`, `Range1sb`, `Range1s`, `Range1us`, `Range1ui`, ...). Color types use their own suffixes (`C3b`, `C4b`, `C4f`, ...).

## Struct Semantics

- Core vector/matrix structs are mutable value types (`struct`), not uniformly `readonly struct`.
- For matrix/vector math in 3D, prefer explicit methods (`TransformPos`, `TransformDir`) over ambiguous shorthand.

## Half Comparison and Collection Semantics

`Aardvark.Base.Half` follows `System.Half` comparison conventions while retaining its existing conversion and arithmetic behavior.

- Comparison operators use IEEE semantics: every NaN is unequal through `==`, `!=` is its exact complement, all ordered relations involving NaN are `false`, and positive/negative zero compare equal.
- `CompareTo` provides collection ordering: all NaN encodings compare equal to each other and before every non-NaN value; signed zeros compare equal.
- `Equals` treats all NaN encodings as equal and also equates signed zeros. `GetHashCode` canonicalizes both groups, so dictionaries and sets satisfy the equality/hash contract.
- `Sign` returns zero for either signed zero and throws `ArithmeticException` for every NaN encoding.
- `Max` and `Min` propagate NaN regardless of operand position. Between opposite-signed zeros, `Max` selects positive zero and `Min` selects negative zero.

Raw encodings remain available through `Half.ToHalf(ushort)` and `Half.GetBits(Half)` when payload or zero-sign distinctions are required.

## Fraction Semantics

`Fraction` stores its serialized representation directly as mutable `long Numerator` and `long Denominator` fields. Construction normally makes the denominator non-negative but does not automatically reduce finite values.

- Finite comparison operators are mathematically exact across the full stored 64-bit range. They use raw equality and rounded `double` ordering as fast paths, then an allocation-free continued-fraction comparison only when the rounded values tie; production comparison does not use `BigInteger` or cross-products that can overflow.
- Unreduced equivalents such as `new Fraction(1, 2)` and `new Fraction(3, 6)` compare equal. `Equals` and `GetHashCode` use the same numerical finite equality, so dictionaries and sets group equivalent fractions and all finite zero representations.
- A zero denominator with zero numerator is `Fraction.NaN`; a zero denominator with a negative or positive numerator is signed infinity. `IsNaN` and `IsInfinity` are mutually exclusive.
- Operators follow IEEE-style NaN semantics: NaN is unequal through `==`, `!=` is the exact complement of `==`, and all ordered comparisons involving NaN are false. `Equals` treats NaNs as equal for collection semantics. Signed infinities compare and hash by sign.
- `Reduced` returns finite values in lowest terms and canonicalizes special values to `0/0`, `-1/0`, or `1/0`.
- Addition and subtraction propagate NaN; equal-signed infinities remain infinite, while opposite infinities produce NaN.

The struct remains two sequential 64-bit fields, and binary/XML coders continue to serialize those raw fields. Existing finite arithmetic still uses `long` operations and can overflow independently of the exact comparison implementation.

## Integer Primality

`Fun.IsPrime(int)` and `Fun.IsPrime(long)` return `true` exactly for prime integers: values greater than or equal to 2 whose only positive divisors are 1 and themselves. Negative values, zero, and one return `false`.

```csharp
var smallPrime = Fun.IsPrime(17);       // true
var notPrime = Fun.IsPrime(1L);         // false
```

## Integer Common Divisors and Multiples

`Fun.GreatestCommonDivisor` and `Fun.LeastCommonMultiple` are defined for `int`, `long`, `uint`, and `ulong`, with component-wise overloads for generated integer vectors.

- GCD is non-negative and `gcd(0, 0)` is zero.
- LCM is non-negative and is zero if either input is zero.
- Signed inputs use their mathematical magnitudes, including `MinValue`.
- An `OverflowException` is thrown only when the exact non-negative result cannot fit the return type. In particular, signed `gcd(MinValue, 0)` overflows, while zero LCM values and representable results do not.

```csharp
var gcd = Fun.GreatestCommonDivisor(-18, 24);  // 6
var lcm = Fun.LeastCommonMultiple(-18L, 24L); // 72
var zero = Fun.LeastCommonMultiple(0u, 0u);   // 0
```

## Vector Families

Common families:
- `V2*`, `V3*`, `V4*`
- integer and floating-point variants (`i`, `ui`, `l`, `f`, `d`)

Typical APIs (`Dot`/`Cross`/`Distance` are extension methods on the static class `Vec`, not static members of `V3d`):
```csharp
var a = new V3d(1, 2, 3);
var b = new V3d(4, 5, 6);

var dot = a.Dot(b);          // or Vec.Dot(a, b)
var cross = a.Cross(b);      // or Vec.Cross(a, b)
var dist = a.Distance(b);    // or Vec.Distance(a, b)
var unit = a.Normalized;
```

## Matrix Families

Common families:
- `M22*`, `M23*`, `M33*`, `M34*`, `M44*`

### M44d Construction
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

### M44d Operations
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
- `Ray2*`, `Ray3*` and precomputed `FastRay2*`, `FastRay3*`
- `Plane2*`, `Plane3*`
- `Sphere3*`, `Circle2*`, `Circle3*`
- `Triangle2*`, `Triangle3*`
- `Hull2*`, `Hull3*`

Typical APIs (ray-box intersection is an extension on the box, not the ray; see `SEMANTICS_GEOMETRY_CORE.md`):
```csharp
var box = new Box3d(V3d.Zero, V3d.One);
var ray = new Ray3d(V3d.Zero, V3d.XAxis);

var contains = box.Contains(new V3d(0.5, 0.5, 0.5));
var hit = box.Intersects(ray, out double t);
```

## Gotchas

`TransformPos` vs `TransformDir` matters for translation handling.

## Source Anchors

- `src/Aardvark.Base/Math/Base/Half.cs` (`Half` comparison, equality, hashing, ordering, and extrema semantics)
- `src/Aardvark.Base/Math/Base/Fun_auto.cs` (`Fun.IsPrime`, `Fun.GreatestCommonDivisor`, `Fun.LeastCommonMultiple`)
- `src/Aardvark.Base/Math/Vectors/Vector_auto.cs` (`V3d`)
- `src/Aardvark.Base/Math/Trafos/Matrix_auto.cs` (`M44d`, transforms, operators)
- `src/Aardvark.Base/Math/Trafos/Rot3_auto.cs` (`Rot3d` to `M44d` cast)
- `src/Aardvark.Base/Math/RangesBoxes/Box_auto.cs` (`Box3d`)
- `src/Aardvark.Base/Geometry/Types/Ray/Ray3_auto.cs` (`Ray3d` hit methods)

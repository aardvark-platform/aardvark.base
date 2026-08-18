# Aardvark.Base Symbol Index

Quick symbol-to-doc map for incremental discovery.

## Core Math / Geometry

| Symbol | Primary Doc |
|--------|-------------|
| `V2d`, `V3d`, `V4d` | [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) |
| `M22d..M44d` | [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) |
| `Rot3d`, `Trafo3d` | [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) |
| `M44d` layout/interoperability | [SEMANTICS_LINEAR_ALGEBRA.md](SEMANTICS_LINEAR_ALGEBRA.md) |
| Transform semantics (`TransformPos`, `TransformDir`) | [SEMANTICS_GEOMETRY_CORE.md](SEMANTICS_GEOMETRY_CORE.md) |
| `Box3d`, `Ray3d`, `Plane3d` | [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) |
| `FastRay2d`, `FastRay3d`, `RayHit3d`, intersection conventions | [SEMANTICS_GEOMETRY_CORE.md](SEMANTICS_GEOMETRY_CORE.md) |

## Images / Tensors

| Symbol | Primary Doc |
|--------|-------------|
| `PixImage`, `PixImage<T>` | [PIXIMAGE.md](PIXIMAGE.md) |
| `PixVolume`, `PixVolume<T>` | [PIXIMAGE.md](PIXIMAGE.md) |
| `PixCube`, `PixImageMipMap` | [PIXIMAGE.md](PIXIMAGE.md) |
| `PixFormat`, `PixFileFormat`, `PixProcessorCaps` | [PIXIMAGE.md](PIXIMAGE.md) |
| `Vector<T>`, `Matrix<T>`, `Volume<T>`, `Tensor4<T>` | [TENSORS.md](TENSORS.md) |
| `MatrixInfo`, `VolumeInfo`, `Tensor4Info` | [TENSORS.md](TENSORS.md) |

## Algorithms / Numerics

| Symbol | Primary Doc |
|--------|-------------|
| `Fun.IsPrime(int)`, `Fun.IsPrime(long)` | [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) |
| `ShortestPath<T>` | [ALGORITHMS.md](ALGORITHMS.md) |
| `BbTree` | [ALGORITHMS.md](ALGORITHMS.md) |
| `AliasTableF`, `AliasTableD` | [ALGORITHMS.md](ALGORITHMS.md) |
| `DistributionFunction` | [ALGORITHMS.md](ALGORITHMS.md) |
| `Polynomial` | [ALGORITHMS.md](ALGORITHMS.md) |
| `LuFactorize`, `LuSolve`, `QrFactorize` | [ALGORITHMS.md](ALGORITHMS.md) |
| `Stats<T>`, `Histogram` | [ALGORITHMS.md](ALGORITHMS.md) |

## Collections / Infrastructure

| Symbol | Primary Doc |
|--------|-------------|
| `Symbol`, `TypedSymbol<T>` | [COLLECTIONS.md](COLLECTIONS.md) |
| `Dict<TKey,TValue>`, `SymbolDict<T>`, `SymbolSet` | [COLLECTIONS.md](COLLECTIONS.md) |
| `LruCache<TKey,TValue>` | [COLLECTIONS.md](COLLECTIONS.md) |
| `ConcurrentHashSet<T>` | [COLLECTIONS.md](COLLECTIONS.md) |
| `ToDictionaryDistinct`, `DictFun`, `SingleValueDict`, `SingleEntryDict` | [COLLECTIONS.md](COLLECTIONS.md) |
| `Report` | [UTILITIES.md](UTILITIES.md) |
| `Telemetry` | [UTILITIES.md](UTILITIES.md) |
| `RandomSystem`, `RandomSample`, `HaltonRandomSeries`, `Quasi` | [UTILITIES.md](UTILITIES.md) |
| `Geo`, `GeoEllipsoid` | [UTILITIES.md](UTILITIES.md) |
| `Constant`, `ConstantF`, `INode` | [UTILITIES.md](UTILITIES.md) |
| `Dir` (relative path utilities) | [UTILITIES.md](UTILITIES.md) |

## Serialization

| Symbol | Primary Doc |
|--------|-------------|
| `ICoder`, `IReadingCoder`, `IWritingCoder` | [SERIALIZATION.md](SERIALIZATION.md) |
| `BinaryReadingCoder`, `BinaryWritingCoder`, `XmlReadingCoder`, `XmlWritingCoder` | [SERIALIZATION.md](SERIALIZATION.md) |
| `TypeInfo` | [SERIALIZATION.md](SERIALIZATION.md) |

## F# / Incremental

| Symbol | Primary Doc |
|--------|-------------|
| `Vec`, `Mat`, `Trafo`, `Lens` | [FSHARP_INTEROP.md](FSHARP_INTEROP.md) |
| `ColorBrewer`, `MicroTime`, `Mem` | [FSHARP_INTEROP.md](FSHARP_INTEROP.md) |
| `aval`, `cval`, `aset`, `amap`, `alist` | [INCREMENTAL.md](INCREMENTAL.md) |
| `afun`, `astate`, `Controller`, `astream`, `Proc`, `ChangeTracker` | [INCREMENTAL.md](INCREMENTAL.md) |

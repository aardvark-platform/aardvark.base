# Aardvark.Base Tensor Types Reference

Stride-based tensor containers.

## Core Model

Indexing is stride-based:

```
index = Origin + x * DX + y * DY + z * DZ + ...
```

Metadata structs:

- `VectorInfo`
- `MatrixInfo`
- `VolumeInfo`
- `Tensor4Info`

Data containers:

- `Vector<Td>`
- `Matrix<Td>`
- `Volume<Td>`
- `Tensor4<Td>`

These are `struct` types in generated `Tensor_auto.cs`.

Each container also has an accessor-based two-parameter form `Vector<Td,Tv>` / `Matrix<Td,Tv>` / `Volume<Td,Tv>` / `Tensor4<Td,Tv>` (raw data type `Td`, view type `Tv`, e.g. `Matrix<byte, C4b>` for typed pixel access).

## Default Dense Layouts

`MatrixInfo(size)` defaults to:

- `DX = 1`
- `DY = SX`

Image layouts (`ImageTensors.cs`):

- Matrix: `DX = 1`, `DY = SX`
- Volume: `DZ = 1`, `DX = SZ`, `DY = SX * DX`
- Tensor4: `DW = 1`, `DX = SW`, `DY = SX * DX`, `DZ = SY * DY`

## Views vs Copies

Subview methods return views on shared data:

- `SubMatrix(...)`, `SubMatrixWindow(...)`
- `SubVolume(...)`, `SubVolumeWindow(...)`
- `SubTensor4(...)`, `SubTensor4Window(...)`

`Transposed` is a stride/view transform, not deep copy.

Use `Copy()` or `CopyWindow()` when you need independent storage.

## Matrix Convenience Methods

On `Matrix<T>`:

- `Row(y)` and `Col(x)` (return `Vector<T>`)
- `Transposed`
- `SetByCoord(...)`
- `ForeachIndex(...)` / `ForeachCoord(...)` for iteration

## Native Decomposition Kernels

`QR`, `RQ`, and `SVD` in `Aardvark.Base.Tensors` provide managed, native in-place, and fixed-size
matrix decompositions:

- QR: `A = Q * R`, with orthogonal `Q` and upper-triangular `R`.
- RQ: `A = R * Q`, with upper-triangular `R` and orthogonal `Q`.
- Bidiagonalization: `A = U * B * Vt`, with orthogonal factors and upper-bidiagonal `B`.
- SVD: `A = U * S * Vt`, with orthogonal factors and diagonal `S`.

SVD orders diagonal entries by descending absolute magnitude. Ordering uses deterministic in-place
selection: when magnitudes are equal, the first remaining position wins. Coupled `U`/`S`/`Vt`
swaps preserve reconstruction and the established sign/orientation normalization.

Native in-place paths honor `Origin`, `DX`, and `DY`; identity initialization touches only logical
matrix elements, including for offset and non-dense views. Fixed-size `DecomposeV`,
`BidiagonalizeV`, and SVD value-option kernels use stack storage and internal value views, so their
warmed hot paths have no transient managed allocations. Managed allocating overloads still create
their documented result containers.

## Image Layout Helpers

In `Tensors/ImageTensors.cs`:

- `HasImageLayout(...)` / `HasImageWindowLayout(...)`
- `CreateImageMatrix(...)` / `CreateImageVolume(...)` / `CreateImageTensor4(...)`
- `ToImage(...)` / `ToImageWindow(...)`
- `MapToImage(...)` / `MapToImageWindow(...)`
- `CopyToImage(...)` / `CopyToImageWindow(...)`

## Source Anchors

- `src/Aardvark.Base.Tensors.CSharp/Tensor_auto.cs`
- `src/Aardvark.Base.Tensors.CSharp/Tensors/ImageTensors.cs`
- `src/Aardvark.Base.Tensors/Algorithms/QR.fs`
- `src/Aardvark.Base.Tensors/Algorithms/SVD.fs`

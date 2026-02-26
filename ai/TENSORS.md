# Aardvark.Base Tensor Types Reference

Source-verified reference for stride-based tensor containers.

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
- `Foreach...` variants for iteration

## Image Layout Helpers

In `Tensors/ImageTensors.cs`:

- `HasImageLayout(...)`
- `CreateImageVolume(...)`
- `CreateImageTensor4(...)`
- `ToImage(...)` / `ToImageWindow(...)`
- `CopyToImage(...)` / `CopyToImageWindow(...)`

These are important for high-performance pix/tensor interoperability.

## Source Anchors

- `src/Aardvark.Base.Tensors.CSharp/Tensor_auto.cs`
- `src/Aardvark.Base.Tensors.CSharp/Tensors/ImageTensors.cs`

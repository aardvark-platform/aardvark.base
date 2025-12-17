# Aardvark.Base Tensor Types Reference

AI-targeted reference for stride-based n-dimensional arrays. Key concept: tensors use **stride-based indexing** allowing views into data without copying.

## Core Concept: Stride-Based Indexing

```
array_index = Origin + coord[0] * Delta[0] + coord[1] * Delta[1] + ...
```

- **Origin**: Index of element at coordinate (0, 0, ...) in underlying array
- **Delta** (stride): Step in array per coordinate increment
- **First**: Starting coordinate (for subwindows retaining original coordinate space)
- **Size**: Number of elements per dimension

This enables:
- **Non-copying views**: Subregions reference same underlying array
- **Transposition**: Swap deltas without moving data
- **Row/column access**: Different strides select rows vs columns

---

## Type Hierarchy

### Info Structs (Metadata Only)

| Type | Dimensions | Fields |
|------|------------|--------|
| `VectorInfo` | 1D | Origin, Size, Delta, First |
| `MatrixInfo` | 2D | Origin, Size (V2l), Delta (V2l), First (V2l) |
| `VolumeInfo` | 3D | Origin, Size (V3l), Delta (V3l), First (V3l) |
| `Tensor4Info` | 4D | Origin, Size (V4l), Delta (V4l), First (V4l) |

Info structs contain indexing metadata without data array. Use for computing indices or describing layouts.

### Data Containers

| Type | Description |
|------|-------------|
| `Vector<T>` | 1D array with stride |
| `Matrix<T>` | 2D array with strides |
| `Volume<T>` | 3D array with strides |
| `Tensor4<T>` | 4D array with strides |
| `Tensor<T>` | N-dimensional (arbitrary rank) |

All store: `T[] Data` + corresponding Info struct.

---

## VectorInfo / Vector<T>

### Fields
```csharp
long Origin;   // Index of element [0] in Data
long Size;     // Number of elements (S, SX)
long Delta;    // Stride per element (D, DX)
long First;    // First coordinate (F, FX) - for subwindows
```

### Properties
- `Count` - total elements (= Size)
- `End`, `E`, `EX` - First + Size
- `DS` - Delta * Size (cumulative stride)

### Construction
```csharp
new Vector<float>(100);                    // dense 100-element vector
new Vector<float>(data, origin, size, delta);  // view into existing array
new Vector<float>(info, data);             // from VectorInfo + array
```

---

## MatrixInfo / Matrix<T>

### Fields
```csharp
long Origin;   // Index of element [0,0] in Data
V2l Size;      // (SX, SY) - columns, rows
V2l Delta;     // (DX, DY) - column stride, row stride
V2l First;     // (FX, FY) - first coordinates
```

### Properties
- `SX`, `SY` - size per dimension
- `DX`, `DY` - delta per dimension
- `FX`, `FY` - first coordinate per dimension
- `EX`, `EY` - end per dimension (First + Size)
- `DSX`, `DSY` - cumulative strides (Delta * Size)

### Construction
```csharp
new Matrix<float>(100, 50);                // 100 columns x 50 rows, dense
new Matrix<float>(size);                   // from V2l
new Matrix<float>(data, origin, size, delta);
new Matrix<float>(info, data);
```

### Dense Layout (Default)
```csharp
DX = 1          // adjacent columns are adjacent in memory
DY = SX         // rows are SX elements apart
```

---

## VolumeInfo / Volume<T>

### Fields
```csharp
long Origin;
V3l Size;      // (SX, SY, SZ)
V3l Delta;     // (DX, DY, DZ)
V3l First;     // (FX, FY, FZ)
```

### Properties
- `SX`, `SY`, `SZ`, `DX`, `DY`, `DZ`, `FX`, `FY`, `FZ`
- `EX`, `EY`, `EZ` - end coordinates
- `DSX`, `DSY`, `DSZ` - cumulative strides

### Image Layout Convention
For images with channels (RGB):
```csharp
DZ = 1              // channels adjacent
DX = SZ             // pixels in row: step by channel count
DY = SZ * SX        // rows: step by row size
```

### Construction
```csharp
new Volume<byte>(width, height, channels);
new Volume<byte>(size);                    // from V3l
new Volume<byte>(data, origin, size, delta);
```

---

## Tensor4Info / Tensor4<T>

### Fields
```csharp
long Origin;
V4l Size;      // (SX, SY, SZ, SW)
V4l Delta;     // (DX, DY, DZ, DW)
V4l First;     // (FX, FY, FZ, FW)
```

Used for 4D data (e.g., video: width x height x channels x frames).

---

## Tensor<T> (N-Dimensional)

For arbitrary rank tensors.

### Fields
```csharp
T[] Data;
long Origin;
int Rank;          // number of dimensions
long[] Size;       // size per dimension
long[] Delta;      // stride per dimension
```

### Construction
```csharp
new Tensor<float>(new long[] { 10, 20, 30 });  // 3D tensor 10x20x30
new Tensor<float>(size);                        // from long[]
```

---

## Common Operations

### Element Access
```csharp
// Direct indexing
matrix[x, y] = value;
volume[x, y, z] = value;

// With info struct (compute index manually)
long index = info.Origin + x * info.DX + y * info.DY;
data[index] = value;
```

### Subregions (Windows)
```csharp
// Get submatrix (view, not copy)
var sub = matrix.SubMatrix(x, y, width, height);
sub[0, 0] = 5;  // modifies original matrix!

// With coordinate space retention
var sub = matrix.SubMatrix(x, y, width, height, retainCoordinateSpace: true);
// sub.FX = x, sub.FY = y (original coordinates preserved)
```

### Row/Column Access
```csharp
var row = matrix.GetRow(y);      // Vector<T> view of row
var col = matrix.GetCol(x);      // Vector<T> view of column
```

### Transposition
```csharp
var transposed = matrix.Transposed;  // swaps DX/DY, no data copy
```

### Copying
```csharp
var copy = matrix.Copy();        // deep copy to new dense array
matrix.CopyTo(target);           // copy to existing matrix
```

### Iteration
```csharp
// Element-wise
matrix.ForeachIndex((x, y) => { /* ... */ });
matrix.SetByCoord((x, y) => x + y);

// With data
matrix.ForeachXYIndex((x, y, i) => data[i] = value);
```

---

## Memory Layout Patterns

### Dense (Default)
Elements contiguous, inner dimension has Delta=1:
```csharp
// Matrix: row-major
DX = 1, DY = SX

// Volume (image): channel-major
DZ = 1, DX = SZ, DY = SZ * SX
```

### Sparse/Strided
Non-contiguous elements:
```csharp
// Every other element
var sparse = new Vector<float>(data, 0, 50, 2);  // Delta=2

// Reverse order
var reversed = new Vector<float>(data, 99, 100, -1);  // Delta=-1
```

### Window (View)
References subset of parent array:
```csharp
var window = volume.SubVolume(10, 10, 0, 20, 20, 3);
// window.Data == volume.Data (same array)
// Modifications to window affect volume
```

---

## Border Handling

For operations needing samples outside bounds:

| Mode | Behavior |
|------|----------|
| `Clamped` | Clamp coordinates to valid range |
| `Cyclic` | Wrap around (modulo) |
| `Raw` | No checking (undefined if out of bounds) |

```csharp
// Tensor static functions for sample access
Tensor.Index2SamplesClamped(i, first, end, delta)  // returns clamped offset
Tensor.Index2SamplesCyclic1(i, first, end, delta)  // returns wrapped offset
```

---

## Key Interfaces

| Interface | Description |
|-----------|-------------|
| `ITensor<T>` | Generic indexed tensor access |
| `ITensorInfo` | Dimension-independent indexing metadata |
| `IArrayTensor` | Array-backed tensor base |
| `IArrayVector` | 1D array tensor |
| `IArrayMatrix` | 2D array tensor |
| `IArrayVolume` | 3D array tensor |
| `IArrayTensorN` | N-dimensional array tensor |

---

## Usage Patterns

### Creating Image Volume
```csharp
// RGB image 1920x1080
var image = new Volume<byte>(1920, 1080, 3);
// Access pixel (x, y), channel c:
image[x, y, c] = value;
```

### Zero-Copy Subregion
```csharp
var full = new Matrix<float>(1000, 1000);
var roi = full.SubMatrix(100, 100, 200, 200);  // region of interest
// roi shares data with full
ProcessRegion(roi);  // operates on original data
```

### Transposed View
```csharp
var m = new Matrix<float>(100, 50);
var t = m.Transposed;  // 50x100, same data
// t[x, y] == m[y, x]
```

### Channel Extraction
```csharp
var rgb = new Volume<byte>(width, height, 3);
var red = rgb.SubVolume(0, 0, 0, width, height, 1);    // R channel
var green = rgb.SubVolume(0, 0, 1, width, height, 1);  // G channel
var blue = rgb.SubVolume(0, 0, 2, width, height, 1);   // B channel
```

---

## Gotchas

1. **Stride vs Size Confusion**: `Delta[i]` is the *stride* (step per increment), not the actual size. To get span in bytes, multiply `Delta * Size`. Don't confuse dense layout assumptions with arbitrary strides
2. **SubRegion Mutability**: Views from `SubMatrix()`, `SubVolume()` share the underlying `Data` array. Modifications affect the parent! Use `.Copy()` for independence
3. **Transposition Side Effect**: `matrix.Transposed` swaps deltas but *doesn't* copy data. Subsequent operations reflect the transposed view, but iteration order matters for performance

---

## See Also

- [PIXIMAGE.md](PIXIMAGE.md) - `PixImage<T>` is backed by `Volume<byte/float>`; image pixel layout uses channel-major stride convention
- [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) - Tensors store geometric data (point clouds, meshes); coordinate systems use `V2l`/`V3l` for sizes
- [SERIALIZATION.md](SERIALIZATION.md) - Tensors serialize via `ICoder` (e.g., `CodeIArrayMatrix`, `CodeIArrayVolume`)

# Aardvark.Base PixImage Reference

AI-targeted reference for image loading, saving, and manipulation. Images are backed by `Volume<T>` tensors (see TENSORS.md).

## Core Types

| Type | Description |
|------|-------------|
| `PixImage` | Abstract base class (non-generic) |
| `PixImage<T>` | Generic image with element type T |
| `PixVolume<T>` | 3D pixel container (width x height x depth) |
| `PixCube` | Cube map (6 faces with mipmap chains) |
| `PixImageMipMap` | Mipmap pyramid for single image |

---

## PixFormat

Combines element type and color format.

```csharp
public readonly struct PixFormat
{
    public readonly Type Type;        // byte, ushort, float, etc.
    public readonly Col.Format Format; // RGB, RGBA, Gray, etc.
}
```

### Common PixFormats
```csharp
PixFormat.ByteGray      // Gray byte image
PixFormat.ByteRGB       // RGB byte image
PixFormat.ByteRGBA      // RGBA byte image
PixFormat.ByteBGR       // BGR byte image (Windows bitmap order)
PixFormat.ByteBGRA      // BGRA byte image
PixFormat.FloatGray     // Single-channel float
PixFormat.FloatRGB      // RGB float image
```

---

## Col.Format (Color Formats)

| Format | Channels | Description |
|--------|----------|-------------|
| `None` | 0 | No format |
| `Alpha` | 1 | Alpha only |
| `BW` | 1 | Black/white |
| `Gray` | 1 | Grayscale |
| `GrayAlpha` | 2 | Gray + alpha |
| `RG` | 2 | Red + green |
| `RGB` | 3 | Red, green, blue |
| `BGR` | 3 | Blue, green, red (Windows order) |
| `RGBA` | 4 | RGB + alpha |
| `BGRA` | 4 | BGR + alpha |
| `RGBP` | 4 | RGB premultiplied alpha |
| `BGRP` | 4 | BGR premultiplied alpha |
| `HSL` | 3 | Hue, saturation, lightness |
| `HSV` | 3 | Hue, saturation, value |
| `Yuv` | 3 | Luma + chroma |
| `CieXYZ` | 3 | CIE XYZ |
| `CieLab` | 3 | CIE L*a*b* |
| `CieLuv` | 3 | CIE L*u*v* |

---

## PixFileFormat

Supported file formats for loading/saving:

| Format | Extension | Notes |
|--------|-----------|-------|
| `Png` | .png | Lossless, alpha support |
| `Jpeg` | .jpg, .jpeg | Lossy, no alpha |
| `Webp` | .webp | Lossy/lossless, alpha |
| `Bmp` | .bmp | Windows bitmap |
| `Tiff` | .tiff, .tif | Multi-page, HDR support |
| `Gif` | .gif | Animated, palette |
| `Exr` | .exr | HDR, OpenEXR |
| `Hdr` | .hdr | Radiance HDR |
| `Targa` | .tga | Targa |
| `Psd` | .psd | Photoshop |
| `Dds` | .dds | DirectDraw Surface |
| `Pbm`, `Pgm`, `Ppm` | .pbm, .pgm, .ppm | Netpbm formats |
| `Pfm` | .pfm | Portable float map |
| `J2k`, `Jp2` | .j2k, .jp2 | JPEG 2000 |

Plus: `Ico`, `Jng`, `Koala`, `Lbm`, `Iff`, `Mng`, `Pcd`, `Pcx`, `Ras`, `Wbmp`, `Cut`, `Xbm`, `Xpm`, `Faxg3`, `Sgi`, `Pict`, `Raw`, `Wmp`

---

## PixImage<T>

### Construction

```csharp
// From dimensions
new PixImage<byte>(width, height, channels);
new PixImage<byte>(V2i size, channels);

// With explicit format
new PixImage<byte>(Col.Format.RGB, width, height);
new PixImage<byte>(Col.Format.RGBA, V2i size);

// From Volume (no copy)
new PixImage<byte>(Col.Format.RGB, volume);
new PixImage<byte>(volume);  // uses default format

// From channel matrices (copies data)
new PixImage<float>(redMatrix, greenMatrix, blueMatrix);

// From file
PixImage.Load(filename);
PixImage.Load(stream);
```

### Properties
```csharp
image.Size          // V2i (width, height)
image.Width         // int
image.Height        // int
image.ChannelCount  // long
image.Format        // Col.Format
image.PixFormat     // PixFormat (Type + Format)
image.Volume        // Volume<T> - underlying data
image.Data          // T[] - raw array
```

### Loading
```csharp
// Load with automatic type detection
var img = PixImage.Load("image.png");           // returns PixImage (base)
var img = PixImage.Load<byte>("image.png");     // returns PixImage<byte>

// Load from stream
using var stream = File.OpenRead("image.png");
var img = PixImage.Load(stream);

// Get info without loading pixels
var info = PixImage.GetInfoFromFile("image.png");  // PixImageInfo
// info.Format, info.Size
```

### Saving
```csharp
// Basic save (format from extension)
image.Save("output.png");
image.Save("output.jpg");

// With parameters
image.Save("output.jpg", new PixJpegSaveParams(quality: 90));
image.Save("output.png", new PixPngSaveParams(compressionLevel: 6));
image.Save("output.webp", new PixWebpSaveParams(quality: 80, lossless: false));
image.Save("output.tiff", new PixTiffSaveParams(compression: PixTiffCompression.Lzw));
image.Save("output.exr", new PixExrSaveParams(compression: PixExrCompression.Piz));

// To stream
image.Save(stream, PixFileFormat.Png);
image.SaveAsJpeg(stream, quality: 85);
```

### Save Parameters

```csharp
// JPEG
new PixJpegSaveParams(quality);  // 0-100, default 90

// PNG
new PixPngSaveParams(compressionLevel);  // 0-9, default 6

// WebP
new PixWebpSaveParams(quality, lossless);

// TIFF
new PixTiffSaveParams(compression);
// PixTiffCompression: None, Lzw, Jpeg, Deflate, Ccitt3, Ccitt4, etc.

// EXR
new PixExrSaveParams(compression);
// PixExrCompression: None, Rle, ZipS, Zip, Piz, Pxr24, B44, B44A, Dwaa, Dwab
```

---

## Type Conversion

```csharp
// Convert element type
var byteImg = floatImg.ToPixImage<byte>();
var floatImg = byteImg.ToPixImage<float>();

// Convert format
var rgba = rgb.ToFormat(Col.Format.RGBA);
var gray = rgb.ToGrayscale();

// Ensure specific layout
var standard = img.ToImageLayout();  // ensures standard memory layout
```

---

## Transformations

### Scaling
```csharp
// Scale to specific size
var scaled = image.Scaled(newWidth, newHeight);
var scaled = image.Scaled(V2i newSize);

// Scale by factor
var half = image.Scaled(0.5);
var doubled = image.Scaled(2.0);

// With interpolation mode
var scaled = image.Scaled(size, ImageInterpolation.Cubic);
```

### Interpolation Modes
| Mode | Description |
|------|-------------|
| `Near` | Nearest neighbor |
| `Linear` | Bilinear |
| `Cubic` | Bicubic |
| `Lanczos` | Lanczos (high quality) |

### Rotation
```csharp
var rotated = image.Rotated(angleDegrees);
var rotated = image.Rotated90();
var rotated = image.Rotated180();
var rotated = image.Rotated270();
```

### Mirroring
```csharp
var flippedH = image.MirroredX();  // horizontal flip
var flippedV = image.MirroredY();  // vertical flip
```

### Transposition
```csharp
var transposed = image.Transposed();  // swap X and Y
```

---

## Channel Operations

```csharp
// Get single channel as matrix
var red = image.GetChannel(0);      // Matrix<T>
var green = image.GetChannel(1);
var blue = image.GetChannel(2);

// Get channel by name
var red = image.GetChannel(Col.Channel.Red);

// Set channel
image.SetChannel(0, redMatrix);
```

---

## Subimages

```csharp
// Get subregion (view, not copy)
var roi = image.SubImage(x, y, width, height);
var roi = image.SubImage(Box2i region);

// Copy subregion
var copy = image.SubImageCopy(x, y, width, height);
```

---

## Visitor Pattern

For type-agnostic operations on `PixImage`:

```csharp
public interface IPixImageVisitor<TResult>
{
    TResult Visit<TData>(PixImage<TData> image);
}

// Usage
var result = pixImage.Visit(new MyVisitor());
```

---

## PixVolume<T>

3D pixel data (width x height x depth/slices).

```csharp
new PixVolume<byte>(width, height, depth, channels);
new PixVolume<byte>(V3i size, channels);
new PixVolume<byte>(Col.Format.RGB, Tensor4<byte> tensor);

volume.Size       // V3i
volume.Tensor4    // Tensor4<T> - underlying data
```

---

## PixCube

Cube map with 6 faces, each potentially with mipmaps.

```csharp
cube[CubeSide.PositiveX]  // PixImageMipMap for +X face
cube[CubeSide.NegativeX]  // -X face
// PositiveY, NegativeY, PositiveZ, NegativeZ
```

---

## PixImageMipMap

Mipmap chain for a single image.

```csharp
var mipmap = new PixImageMipMap(baseImage);
mipmap.ImageCount       // number of levels
mipmap[0]               // base image (largest)
mipmap[level]           // specific mip level
```

---

## Loaders and Processors

### Adding Custom Loader
```csharp
PixImage.AddLoader(myLoader);           // highest priority
PixImage.SetLoader(myLoader, priority); // specific priority
PixImage.RemoveLoader(myLoader);
```

### Processor Capabilities
```csharp
[Flags]
public enum PixProcessorCaps
{
    None = 0,
    Scale = 1,
    Remap = 2,
    // ...
}

// Get available processors
var processors = PixImage.GetProcessors(PixProcessorCaps.Scale);
```

---

## Usage Patterns

### Load, Process, Save
```csharp
var img = PixImage.Load<byte>("input.png");
var gray = img.ToGrayscale();
var scaled = gray.Scaled(0.5);
scaled.Save("output.jpg", new PixJpegSaveParams(85));
```

### Check Format Before Loading
```csharp
var info = PixImage.GetInfoFromFile("large.tiff");
if (info.Size.X > 4096 || info.Size.Y > 4096)
    throw new Exception("Image too large");
var img = PixImage.Load<float>("large.tiff");
```

### Batch Processing
```csharp
foreach (var file in Directory.GetFiles("*.png"))
{
    var img = PixImage.Load<byte>(file);
    var processed = img.Scaled(0.5).ToFormat(Col.Format.RGB);
    processed.Save(Path.ChangeExtension(file, ".jpg"));
}
```

### Working with HDR
```csharp
var hdr = PixImage.Load<float>("scene.exr");
// Process in float space
var tonemapped = ToneMap(hdr);
// Save as LDR
tonemapped.ToPixImage<byte>().Save("output.png");
```

---

## Gotchas

1. **Channel Count vs Format**: Channel count (1, 3, 4) and color format (RGB, RGBA, Gray) can diverge. Always check `image.PixFormat` to understand both type and layout
2. **SubImage Mutability**: `SubImage()` returns a view into the same backing data. Modifications affect the original image. Use `SubImageCopy()` for independence
3. **Format/Type Conversion Cost**: `ToFormat()` and `ToPixImage<T>()` perform deep copies and color space conversions. Cache results if calling repeatedly on same data

---

## See Also

- [TENSORS.md](TENSORS.md) - `PixImage<T>` backed by `Volume<T>`; stride-based memory layout and zero-copy views explained
- [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) - Images use `V2i` for dimensions, `C3b`/`C4b` for colors, `Box2i` for regions
- [SERIALIZATION.md](SERIALIZATION.md) - Binary serialization of images via `ICoder` interface

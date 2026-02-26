# Aardvark.Base Pix/Image Reference

Source-verified orientation for `PixImage`, `PixVolume`, and related types.

## Core Types

- `PixImage` / `PixImage<T>`
- `PixVolume` / `PixVolume<T>`
- `PixCube`
- `PixImageMipMap`
- `PixFormat`
- `PixFileFormat`
- `PixProcessorCaps`

## Loading and Info

Static load methods are on non-generic `PixImage`:

```csharp
PixImage img = PixImage.Load("a.png");
PixImage img2 = PixImage.Load(stream);
PixImage raw = PixImage.LoadRaw("a.png");
PixImageInfo info = PixImage.GetInfoFromFile("a.png");
```

Typed load is via constructor:

```csharp
var typed = new PixImage<byte>("a.png");
```

No static `PixImage.Load<T>(...)` API exists.

## Saving

```csharp
img.Save("out.png");
img.Save("out.jpg", new PixJpegSaveParams(90));
img.Save(stream, PixFileFormat.Png);
img.SaveAsJpeg("out.jpg", 90);
img.SaveAsPng("out.png", 6);
```

## Processing and Conversion

Common `PixImage<T>` methods:

- `Resized(...)`
- `Scaled(...)`
- `Rotated(...)`
- `SubImage(...)` (view on shared storage)
- `ToPixImage<TOut>()`
- `ToFormat(Col.Format)`
- `ToImageLayout()`
- `GetChannel(long)` / `GetChannel(Col.Channel)`

`SubImage(...)` returns a view; edits affect shared data.

## Loaders and Processors

Loader hooks:

```csharp
PixImage.SetLoader(loader, priority);
PixImage.AddLoader(loader);
```

Processor lookup:

```csharp
var p = PixImage.GetProcessors(PixProcessorCaps.Scale);
```

`PixProcessorCaps` values:

- `None`
- `Scale`
- `Rotate`
- `Remap`
- `All`

## PixFormat and File Formats

`PixFormat` is defined in `Aardvark.Base` (`Color.cs`) as `(Type, Col.Format)`.

Common predefined values:

- `PixFormat.ByteGray`
- `PixFormat.ByteRGB`
- `PixFormat.ByteRGBA`
- `PixFormat.FloatGray`
- `PixFormat.FloatRGB`

`PixFileFormat` enum is in `PixImage.cs` and contains formats such as `Png`, `Jpeg`, `Bmp`, `Tiff`, `Exr`, `Webp`, and others.

## Mipmaps and Cubemaps

`PixImageMipMap`:

- `Load(string|Stream, IPixLoader?)`
- `Create(baseImage, interpolation, maxCount, powerOfTwo)`
- `LevelCount`

`PixCube`:

- six `PixImageMipMap` faces addressed by `CubeSide`

## Source Anchors

- `src/Aardvark.Base.Tensors.CSharp/PixImage/PixImage.cs`
- `src/Aardvark.Base.Tensors.CSharp/PixImage/PixVolume.cs`
- `src/Aardvark.Base.Tensors.CSharp/PixImage/PixCube.cs`
- `src/Aardvark.Base.Tensors.CSharp/PixImage/PixImageMipMap.cs`
- `src/Aardvark.Base.Tensors.CSharp/PixImage/PixProcessor.cs`
- `src/Aardvark.Base/Math/Colors/Color.cs`

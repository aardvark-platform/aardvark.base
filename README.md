# Aardvark.Base

[![Build](https://github.com/aardvark-platform/aardvark.base/actions/workflows/build.yml/badge.svg)](https://github.com/aardvark-platform/aardvark.base/actions/workflows/build.yml)
[![Publish](https://github.com/aardvark-platform/aardvark.base/actions/workflows/publish.yml/badge.svg)](https://github.com/aardvark-platform/aardvark.base/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/aardvark-platform/aardvark.base.svg?label=License)](https://github.com/aardvark-platform/aardvark.rendering/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Aardvark.Base.svg)](https://www.nuget.org/packages/Aardvark.Base/)

High-performance .NET foundation for visual computing, real-time graphics and geometry processing. Cross-platform (Windows/Linux/macOS), targets .NET Standard 2.0 and .NET 8.0.

Foundation of the [Aardvark Platform](https://github.com/aardvark-platform) ecosystem, powering advanced libraries for rendering (Aardvark.Rendering), UI (Aardvark.Media), VR/AR applications, and scientific visualization.

## Core Components

**Mathematics & Geometry**
- Vectors, matrices, quaternions, transformations (2D/3D/4D)
- Ranges, boxes, spheres, planes, rays, frustums
- Polygons, meshes, BVH acceleration structures
- Linear/ellipsoid regression, geometric algorithms

**Image Processing**
- PixImage with full tensor capabilities
- Format support: JPEG, PNG, TIFF, EXR, WebP, DDS
- Mipmaps, cube maps, volume textures
- Hardware-accelerated scaling and transformations

**Data Structures & Algorithms**
- Incremental/reactive programming primitives
- Efficient collections (IntDict, SymbolDict, MapExt)
- Spatial indexing (KdTree, BVH)
- Fast serialization/deserialization

## Quick Start

```csharp
using Aardvark.Base;

// Vectors and matrices
var v = new V3d(1, 2, 3);
var m = M44d.RotationX(45.0.RadiansFromDegrees());
var transformed = m.TransformPos(v);

// Image processing
var image = PixImage.Load("image.jpg");
var scaled = image.Scaled(0.5);
scaled.Save("output.png");

// Geometry
var box = new Box3d(V3d.Zero, V3d.One);
var ray = new Ray3d(V3d.Zero, V3d.XAxis);
var hit = ray.Intersects(box, out double t);
```

## Installation

```
dotnet add package Aardvark.Base
dotnet add package Aardvark.Base.FSharp  # F# extensions
```

## Resources

- [Gallery](https://github.com/aardvark-platform/aardvark.docs/wiki/Gallery)
- [Packages & Repositories](https://github.com/aardvark-platform/aardvark.docs/wiki/Packages-and-Repositories)
- [Documentation](https://github.com/aardvark-platform/aardvark.base/wiki)
- [API Reference](https://aardvarkians.com/)
- [Platform Overview](https://github.com/aardvark-platform/aardvark.docs/wiki)
- [Discord Community](https://discord.gg/UyecnhM)
- [AI Reference](./ai/) - Technical docs for AI coding assistants

## License

[Apache 2.0](LICENSE)

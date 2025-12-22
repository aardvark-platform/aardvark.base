# Aardvark.Base AI Reference

Index for AI coding assistants. Read only the doc you need.

## By Task

| Task | Document | Size |
|------|----------|------|
| Vectors, matrices, colors, transforms | PRIMITIVE_TYPES.md | 11 KB |
| Images: load, save, scale, formats | PIXIMAGE.md | 9 KB |
| N-D arrays, tensors, stride views | TENSORS.md | 8 KB |
| Graphs, spatial, numerical algorithms | ALGORITHMS.md | 8 KB |
| Binary/stream serialization | SERIALIZATION.md | 7 KB |
| Logging, telemetry, random, geodesy | UTILITIES.md | 7 KB |
| Symbols, caches, specialized dicts | COLLECTIONS.md | 6 KB |
| F# modules, lenses, interop | FSHARP_INTEROP.md | 6 KB |
| Reactive/incremental computation | INCREMENTAL.md | 10 KB |

## Meta

| Task | Document | Size |
|------|----------|------|
| Make another repo AI-friendly | RECIPE_AI_FRIENDLINESS.md | 8 KB |

## By Type

- V2d, V3d, V4d, M22d..M44d, C3f, C4b, Rot3d, Trafo3d → PRIMITIVE_TYPES.md
- PixImage, PixVolume, PixFormat → PIXIMAGE.md
- Vector<T>, Matrix<T>, Volume<T>, Tensor4<T> → TENSORS.md
- Symbol, SymbolDict, IntDict, LruCache → COLLECTIONS.md
- KdTree, BbTree, ShortestPath → ALGORITHMS.md
- BinaryWritingCoder, ICoder → SERIALIZATION.md
- Report, Telemetry, RandomSystem → UTILITIES.md
- Vec, Mat, Trafo, Lens → FSHARP_INTEROP.md
- aval, cval, aset, amap, alist, afun, astate, Proc → INCREMENTAL.md

## For Downstream Projects

If you're consuming Aardvark.Base in your project:

1. Use Paket for consistency with aardvark ecosystem
2. Match .NET SDK version to aardvark requirements (see `global.json`)
3. F# projects: also reference `Aardvark.Base.FSharp`
4. See AGENTS.md for related packages (Rendering, UI, etc.)

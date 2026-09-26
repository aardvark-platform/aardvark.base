# Aardvark.Base AI Reference

Task-first docs for coding agents.

## Fast Path

1. Find symbols in [SYMBOL_INDEX.md](SYMBOL_INDEX.md).
2. Open one task doc from the table below.
3. Verify APIs and examples in source with `rg`; source is truth.
4. Correct stale docs in the same change and run `./check-docs.sh` or `.\check-docs.cmd`.

## Task Docs

| Need | Read |
|------|------|
| Primitive math and geometry types | [PRIMITIVE_TYPES.md](PRIMITIVE_TYPES.md) |
| Matrix/vector layout and interop semantics | [SEMANTICS_LINEAR_ALGEBRA.md](SEMANTICS_LINEAR_ALGEBRA.md) |
| Geometry and transform semantics | [SEMANTICS_GEOMETRY_CORE.md](SEMANTICS_GEOMETRY_CORE.md) |
| Images, volumes, loaders, processors | [PIXIMAGE.md](PIXIMAGE.md) |
| Tensor containers, views, strides | [TENSORS.md](TENSORS.md) |
| Graph/spatial/numeric algorithms | [ALGORITHMS.md](ALGORITHMS.md) |
| Symbols, dicts, caches, concurrent set | [COLLECTIONS.md](COLLECTIONS.md) |
| Logging, telemetry, random, geodesy | [UTILITIES.md](UTILITIES.md) |
| Serialization/coder APIs | [SERIALIZATION.md](SERIALIZATION.md) |
| F# wrappers and idioms | [FSHARP_INTEROP.md](FSHARP_INTEROP.md) |
| Incremental/adaptive system | [INCREMENTAL.md](INCREMENTAL.md) |

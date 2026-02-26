# Aardvark.Base AI Reference

Task-first docs for coding agents.

Goal: open one focused document, not the whole `ai/` folder.

## Fast Path

1. Find symbols in [SYMBOL_INDEX.md](SYMBOL_INDEX.md).
2. Open one task doc from the table below.
3. Verify critical names in source with `rg`.

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

## Meta Docs

| Need | Read |
|------|------|
| Symbol-to-doc lookup | [SYMBOL_INDEX.md](SYMBOL_INDEX.md) |
| Drift and accuracy audit log | [DOC_ACCURACY_AUDIT.md](DOC_ACCURACY_AUDIT.md) |
| AI-friendliness recipe for other repos | [RECIPE_AI_FRIENDLINESS.md](RECIPE_AI_FRIENDLINESS.md) |

## Accuracy Contract

- Docs are orientation, source is truth.
- If a method/type matters, verify with `rg` before coding.
- If docs and code differ, fix docs in the same change.
- Prefer examples that reflect current scripts and project targets.

---

Last verified against repository state: 2026-02-26

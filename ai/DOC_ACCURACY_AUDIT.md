# AI Docs Accuracy Audit

Audit baseline for coding-agent docs in this repository.

## Last Audit

- Date: 2026-02-26
- Scope: `AGENTS.md`, `ai/*.md`, `docs/*.md`, selected source anchors

## Verified Corrections in This Audit

1. Added task-split docs for semantic depth:
   - `ai/SEMANTICS_LINEAR_ALGEBRA.md`
   - `ai/SEMANTICS_GEOMETRY_CORE.md`
2. Added symbol-first discovery index:
   - `ai/SYMBOL_INDEX.md`
3. Rewrote inaccurate AI topic docs:
   - `ai/UTILITIES.md`
   - `ai/COLLECTIONS.md`
   - `ai/ALGORITHMS.md`
   - `ai/PIXIMAGE.md`
   - `ai/TENSORS.md`
4. Corrected user-facing docs that had stale command guidance:
   - `docs/CONTRIBUTING.md`
   - `docs/INTEROP.md`
   - `docs/TROUBLESHOOTING.md`
5. Added drift tooling:
   - `tools/DocsChecker/*`
   - `check-docs.sh`
   - `check-docs.cmd`
   - `.github/workflows/docs-check.yml`

## Known High-Risk Drift Areas

- Generated code APIs (`*_auto.cs`, `*_auto.fs`)
- Cross-language dependencies between C# and F# projects
- Matrix/transform semantics and interop assumptions
- Pix/tensor APIs where historical examples often reference removed overloads

## Maintenance Rule

If a PR changes public API behavior or key semantics:

1. Update the relevant doc in `ai/` or `docs/`.
2. Run docs checker (`./check-docs.sh` or `.\check-docs.cmd`).
3. Keep source anchors in sync with actual file locations/symbol names.

## Rules Touched In This Iteration

Docs checker hardening added:

1. Expanded required file coverage to additional AI topic docs.
2. Expanded forbidden/required pattern checks for common stale examples.
3. Expanded source-anchor coverage across primitives, tensors/pix, telemetry/random, collections, and algorithms.
4. Added fixture-based checker tests (`tools/DocsChecker.Tests`).
5. Enabled Linux + Windows hard-fail docs-check workflow matrix.
